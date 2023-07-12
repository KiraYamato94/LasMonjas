using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils;
using Mono.Cecil;
using Newtonsoft.Json.Linq;
using TMPro;
using Twitch;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Version = SemanticVersioning.Version;
using LasMonjas.Patches;
using Il2CppInterop.Runtime.Attributes;
using AmongUs.Data;
using static Il2CppSystem.Globalization.CultureInfo;

namespace LasMonjas.Core
{
    public class ModUpdateBehaviour : MonoBehaviour
    {
        // Borrowed from The Other Roles to make people able to update the mod and Submerged easier https://github.com/TheOtherRolesAU/TheOtherRoles/blob/main/TheOtherRoles/Modules/ModUpdater.cs

        public static readonly bool CheckForSubmergedUpdates = true;
        public static bool showPopUp = true;
        public static bool updateInProgress = false;

        public static ModUpdateBehaviour Instance { get; private set; }
        public ModUpdateBehaviour(IntPtr ptr) : base(ptr) { }
        public class UpdateData
        {
            public string Content;
            public string Tag;
            public string TimeString;
            public JObject Request;
            public Version Version => Version.Parse(Tag);

            public UpdateData(JObject data) {
                Tag = data["tag_name"]?.ToString().TrimStart('v');
                Content = data["body"]?.ToString();
                TimeString = DateTime.FromBinary(((Il2CppSystem.DateTime)data["published_at"]).ToBinaryRaw()).ToString(); 
                Request = data;
            }

            public bool IsNewer(Version version) {
                if (!Version.TryParse(Tag, out var myVersion)) return false;
                return myVersion.BaseVersion() > version.BaseVersion();
            }
        }

        public UpdateData LMJUpdate;
        public UpdateData SubmergedUpdate;

        [HideFromIl2Cpp]
        public UpdateData RequiredUpdateData => LMJUpdate ?? SubmergedUpdate;

        public void Awake() {
            if (Instance) Destroy(this);
            Instance = this;

            SceneManager.add_sceneLoaded((System.Action<Scene, LoadSceneMode>)(OnSceneLoaded));
            this.StartCoroutine(CoCheckUpdates());

            foreach (var file in Directory.GetFiles(Paths.PluginPath, "*.old")) {
                File.Delete(file);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (updateInProgress || scene.name != "MainMenu") return;
            if (RequiredUpdateData is null) {
                showPopUp = false;
                return;
            }

            var template = GameObject.Find("ExitGameButton");
            if (!template) return;

            var button = Instantiate(template, null);

            button.GetComponent<AspectPosition>().enabled = false;
            button.transform.position = new Vector3(template.transform.position.x + 2.08f, template.transform.position.y + 0.62f, template.transform.position.z);

            var buttonTransform = button.transform;
            var pos = buttonTransform.localPosition;
            pos.y += 1.2f;
            buttonTransform.localPosition = pos;

            PassiveButton passiveButton = button.GetComponent<PassiveButton>();
            SpriteRenderer buttonSprite = button.transform.GetChild(2).GetComponent<SpriteRenderer>();
            passiveButton.OnClick = new Button.ButtonClickedEvent();
            passiveButton.OnClick.AddListener((Action)(() => {
                this.StartCoroutine(CoUpdate());
                button.SetActive(false);
            }));

            var text = button.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>();
            string t = "Update Las Monjas";
            if (LMJUpdate == null && SubmergedUpdate != null) t = SubmergedCompatibility.Loaded ? $"Update Submerged" : $"Download Submerged";

            StartCoroutine(Effects.Lerp(0.1f, (System.Action<float>)(p => text.SetText(t))));

            buttonSprite.color = text.color = Color.cyan;
            passiveButton.OnMouseOut.AddListener((Action)(() => buttonSprite.color = text.color = Color.cyan));

            var isSubmerged = LMJUpdate == null;
            var announcement = $"<size=150%>A new <color=#00FFFF>{(isSubmerged ? "Submerged" : "Las Monjas")}</color> update to {(isSubmerged ? SubmergedUpdate.Tag : LMJUpdate.Tag)} is available</size>\n{(isSubmerged ? SubmergedUpdate.Content : LMJUpdate.Content)}";
            var mgr = FindObjectOfType<MainMenuManager>(true);
            if (isSubmerged && !SubmergedCompatibility.Loaded) showPopUp = false;
            if (showPopUp) mgr.StartCoroutine(CoShowAnnouncement(announcement));
            showPopUp = false;
        }

        [HideFromIl2Cpp]
        public IEnumerator CoUpdate() {
            updateInProgress = true;
            var isSubmerged = LMJUpdate == null;
            var updateName = (isSubmerged ? "Submerged" : "Las Monjas");

            var popup = Instantiate(TwitchManager.Instance.TwitchPopup);
            popup.TextAreaTMP.fontSize *= 0.7f;
            popup.TextAreaTMP.enableAutoSizing = false;

            popup.Show();

            var button = popup.transform.GetChild(2).gameObject;
            button.SetActive(false);
            popup.TextAreaTMP.text = $"Updating {updateName}\nPlease wait...";

            var download = Task.Run(DownloadUpdate);
            while (!download.IsCompleted) yield return null;

            button.SetActive(true);
            popup.TextAreaTMP.text = download.Result ? $"{updateName}\nupdated successfully\nPlease restart the game." : "Update wasn't successful\nPlease use Las Monjas Downloader\nto update manually.";
        }

        private static int announcementNumber = 501;
        [HideFromIl2Cpp]
        public IEnumerator CoShowAnnouncement(string announcement, string date = "") {
            var popUp = Instantiate(FindObjectOfType<AnnouncementPopUp>(true));
            popUp.gameObject.SetActive(true);
            yield return popUp.Init(true);

            var announcementS = new Assets.InnerNet.Announcement();
            announcementS.Title = "Las Monjas Announcement";
            announcementS.Text = announcement;
            announcementS.ShortTitle = "Las Monjas Update";
            announcementS.Id = "lmjUpdateAnnouncement_" + announcementNumber.ToString();
            announcementS.Language = 0;
            announcementS.Number = announcementNumber++;
            announcementS.SubTitle = "";
            announcementS.PinState = true;
            announcementS.Date = date == "" ? DateTime.Today.ToString() : date;

            DataManager.Player.Announcements.allAnnouncements.Insert(0, announcementS);
            popUp.CreateAnnouncementList();
        }

        [HideFromIl2Cpp]
        public static IEnumerator CoCheckUpdates() {
            var lmjUpdateCheck = Task.Run(() => Instance.GetGithubUpdate("KiraYamato94", "LasMonjas"));
            while (!lmjUpdateCheck.IsCompleted) yield return null;

            if (lmjUpdateCheck.Result != null && lmjUpdateCheck.Result.IsNewer(Version.Parse(LasMonjasPlugin.VersionString))) {
                Instance.LMJUpdate = lmjUpdateCheck.Result;
            }

            if (CheckForSubmergedUpdates) {
                var submergedUpdateCheck = Task.Run(() => Instance.GetGithubUpdate("SubmergedAmongUs", "Submerged"));
                while (!submergedUpdateCheck.IsCompleted) yield return null;
                if (submergedUpdateCheck.Result != null && (!SubmergedCompatibility.Loaded || submergedUpdateCheck.Result.IsNewer(SubmergedCompatibility.Version))) {
                    Instance.SubmergedUpdate = submergedUpdateCheck.Result;
                    if (Instance.SubmergedUpdate.Tag.Equals("2022.10.26")) Instance.SubmergedUpdate = null; // Thx TOR
                }
            }

            Instance.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        [HideFromIl2Cpp]
        public async Task<UpdateData> GetGithubUpdate(string owner, string repo) {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "LasMonjas Updater");

            var req = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/releases/latest", HttpCompletionOption.ResponseContentRead);
            if (!req.IsSuccessStatusCode) return null;

            var dataString = await req.Content.ReadAsStringAsync();
            JObject data = JObject.Parse(dataString);
            return new UpdateData(data);
        }

        private bool TryUpdateSubmergedInternally() {
            if (SubmergedUpdate == null) return false;
            try {
                if (!SubmergedCompatibility.LoadedExternally) return false;
                var thisAsm = Assembly.GetCallingAssembly();
                var resourceName = thisAsm.GetManifestResourceNames().FirstOrDefault(s => s.EndsWith("Submerged.dll"));
                if (resourceName == default) return false;

                using var submergedStream = thisAsm.GetManifestResourceStream(resourceName)!;
                var asmDef = AssemblyDefinition.ReadAssembly(submergedStream, TypeLoader.ReaderParameters);
                var pluginType = asmDef.MainModule.Types.FirstOrDefault(t => t.IsSubtypeOf(typeof(BasePlugin)));
                var info = IL2CPPChainloader.ToPluginInfo(pluginType, "");
                if (SubmergedUpdate.IsNewer(info.Metadata.Version)) return false;
                File.Delete(SubmergedCompatibility.Assembly.Location);

            }
            catch (Exception e) {
                LasMonjasPlugin.Logger.LogError(e);
                return false;
            }
            return true;
        }


        [HideFromIl2Cpp]
        public async Task<bool> DownloadUpdate() {
            var isSubmerged = LMJUpdate == null;
            if (isSubmerged && TryUpdateSubmergedInternally()) return true;
            var data = isSubmerged ? SubmergedUpdate : LMJUpdate;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "LasMonjas Updater");

            JToken assets = data.Request["assets"];
            string downloadURI = "";
            for (JToken current = assets.First; current != null; current = current.Next) {
                string browser_download_url = current["browser_download_url"]?.ToString();
                if (browser_download_url != null && current["content_type"] != null) {
                    if (current["content_type"].ToString().Equals("application/x-msdownload") &&
                        browser_download_url.EndsWith(".dll")) {
                        downloadURI = browser_download_url;
                        break;
                    }
                }
            }

            if (downloadURI.Length == 0) return false;

            var res = await client.GetAsync(downloadURI, HttpCompletionOption.ResponseContentRead);
            string filePath = Path.Combine(Paths.PluginPath, isSubmerged ? "Submerged.dll" : "LasMonjas.dll");
            if (File.Exists(filePath + ".old")) File.Delete(filePath + ".old");
            if (File.Exists(filePath)) File.Move(filePath, filePath + ".old");

            await using var responseStream = await res.Content.ReadAsStreamAsync();
            await using var fileStream = File.Create(filePath);
            await responseStream.CopyToAsync(fileStream);

            return true;
        }
    }
}