using System.Collections.Generic;
using UnityEngine;
using BepInEx.Configuration;
using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using System.Reflection;
using System.Text;
using static LasMonjas.LasMonjas;

namespace LasMonjas.Core
{
    public class CustomOption
    {
        public static List<CustomOption> options = new List<CustomOption>();
        public static int preset = 0;

        public int id;
        public string name;
        public System.Object[] selections;

        public int defaultSelection;
        public ConfigEntry<int> entry;
        public int selection;
        public OptionBehaviour optionBehaviour;
        public CustomOption parent;
        public bool isHeader;
        public string type;

        public CustomOption(int id, string name, System.Object[] selections, System.Object defaultValue, CustomOption parent, bool isHeader, String type) {
            this.id = id;
            this.name = parent == null ? name : "- " + name;
            this.selections = selections;
            int index = Array.IndexOf(selections, defaultValue);
            this.defaultSelection = index >= 0 ? index : 0;
            this.parent = parent;
            this.isHeader = isHeader;
            this.type = type;
            selection = 0;
            if (id != 0) {
                entry = LasMonjasPlugin.Instance.Config.Bind($"Preset{preset}", id.ToString(), defaultSelection);
                selection = Mathf.Clamp(entry.Value, 0, selections.Length - 1);
            }
            options.Add(this);
        }

        public static CustomOption Create(int id, string name, String type, string[] selections, CustomOption parent = null, bool isHeader = false) {
            return new CustomOption(id, name, selections, "", parent, isHeader, type);
        }

        public static CustomOption Create(int id, string name, String type, float defaultValue, float min, float max, float step, CustomOption parent = null, bool isHeader = false) {
            List<float> selections = new List<float>();
            for (float s = min; s <= max; s += step)
                selections.Add(s);
            return new CustomOption(id, name, selections.Cast<object>().ToArray(), defaultValue, parent, isHeader, type);
        }

        public static CustomOption Create(int id, string name, String type, bool defaultValue, CustomOption parent = null, bool isHeader = false) {
            return new CustomOption(id, name, new string[] { "Off", "On" }, defaultValue ? "On" : "Off", parent, isHeader, type);
        }

        public static void switchPreset(int newPreset) {
            CustomOption.preset = newPreset;
            foreach (CustomOption option in CustomOption.options) {
                if (option.id == 0) continue;

                option.entry = LasMonjasPlugin.Instance.Config.Bind($"Preset{preset}", option.id.ToString(), option.defaultSelection);
                option.selection = Mathf.Clamp(option.entry.Value, 0, option.selections.Length - 1);
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption) {
                    stringOption.oldValue = stringOption.Value = option.selection;
                    stringOption.ValueText.text = option.selections[option.selection].ToString();
                }
            }
        }

        public static void ShareOptionSelections() {
            if (PlayerControl.AllPlayerControls.Count <= 1 || AmongUsClient.Instance?.AmHost == false && PlayerControl.LocalPlayer == null) return;

            MessageWriter messageWriter = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareOptions, Hazel.SendOption.Reliable);
            messageWriter.WritePacked((uint)CustomOption.options.Count);
            foreach (CustomOption option in CustomOption.options) {
                messageWriter.WritePacked((uint)option.id);
                messageWriter.WritePacked((uint)Convert.ToUInt32(option.selection));
            }
            messageWriter.EndMessage();
        }

        public int getSelection() {
            return selection;
        }

        public bool getBool() {
            return selection > 0;
        }

        public float getFloat() {
            return (float)selections[selection];
        }

        public void updateSelection(int newSelection) {
            selection = Mathf.Clamp((newSelection + selections.Length) % selections.Length, 0, selections.Length - 1);
            if (optionBehaviour != null && optionBehaviour is StringOption stringOption) {
                stringOption.oldValue = stringOption.Value = selection;
                stringOption.ValueText.text = selections[selection].ToString();

                if (AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer) {
                    if (id == 0) switchPreset(selection); 
                    else if (entry != null) entry.Value = selection;

                    ShareOptionSelections();
                }
            }
        }
    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    class GameOptionsMenuStartPatch
    {
        public static void Postfix(GameOptionsMenu __instance) {
            if (GameObject.Find("LasMonjasSettings") != null) { 
                GameObject.Find("LasMonjasSettings").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("Las Monjas - Settings");
                return;
            }

            if (GameObject.Find("LasMonjasGamemodes") != null) {
                GameObject.Find("LasMonjasGamemodes").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("Las Monjas - Gamemodes");
                return;
            }

            if (GameObject.Find("LasMonjasImpostors") != null) {
                GameObject.Find("LasMonjasImpostors").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("Las Monjas - Impostors");
                return;
            }

            if (GameObject.Find("LasMonjasRebels") != null) {
                GameObject.Find("LasMonjasRebels").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("Las Monjas - Rebels");
                return;
            }

            if (GameObject.Find("LasMonjasNeutrals") != null) {
                GameObject.Find("LasMonjasNeutrals").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("Las Monjas - Neutrals");
                return;
            }
            
            if (GameObject.Find("LasMonjasCrewmates") != null) {
                GameObject.Find("LasMonjasCrewmates").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("Las Monjas - Crewmates");
                return;
            }

            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();
            if (template == null) return;
            var gameSettings = GameObject.Find("Game Settings");
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var lasMonjasSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var lasMonjasMenu = lasMonjasSettings.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            lasMonjasSettings.name = "LasMonjasSettings";

            var lasMonjasGamemodes = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var lasMonjasGamemodesMenu = lasMonjasGamemodes.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            lasMonjasGamemodes.name = "LasMonjasGamemodes"; 
            
            var lasMonjasImpostors = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var lasMonjasImpostorsMenu = lasMonjasImpostors.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            lasMonjasImpostors.name = "LasMonjasImpostors";

            var lasMonjasRebels = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var lasMonjasRebelsMenu = lasMonjasRebels.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            lasMonjasRebels.name = "LasMonjasRebels";
            
            var lasMonjasNeutrals = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var lasMonjasNeutralsMenu = lasMonjasNeutrals.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            lasMonjasNeutrals.name = "LasMonjasNeutrals";

            var lasMonjasCrewmates = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var lasMonjasCrewmatesMenu = lasMonjasCrewmates.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            lasMonjasCrewmates.name = "LasMonjasCrewmates"; 
            
            var roleTab = GameObject.Find("RoleTab");
            var gameTab = GameObject.Find("GameTab");

            var lasMonjasTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var lasMonjasTabHighlight = lasMonjasTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            lasMonjasTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TabIconSettings.png", 100f);
            lasMonjasTab.name = "LasMonjasSettingsTab";

            var lasMonjasGamemodeTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var lasMonjasGamemodeTabHighlight = lasMonjasGamemodeTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            lasMonjasGamemodeTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TabIconGamemodes.png", 100f);
            lasMonjasGamemodeTab.name = "LasMonjasGamemodesTab"; 
            
            var lasMonjasImpostorTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var lasMonjasImpostorTabHighlight = lasMonjasImpostorTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            lasMonjasImpostorTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TabIconImpostors.png", 100f);
            lasMonjasImpostorTab.name = "LasMonjasImpostorsTab";

            var lasMonjasRebelTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var lasMonjasRebelTabHighlight = lasMonjasRebelTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            lasMonjasRebelTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TabIconRebels.png", 100f);
            lasMonjasRebelTab.name = "LasMonjasRebelsTab"; 
            
            var lasMonjasNeutralTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var lasMonjasNeutralTabHighlight = lasMonjasNeutralTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            lasMonjasNeutralTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TabIconNeutrals.png", 100f);
            lasMonjasNeutralTab.name = "LasMonjasNeutralsTab";

            var lasMonjasCrewmateTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var lasMonjasCrewmateTabHighlight = lasMonjasCrewmateTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            lasMonjasCrewmateTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TabIconCrewmates.png", 100f);
            lasMonjasCrewmateTab.name = "LasMonjasCrewmatesTab";

            // Position of Tab Icons
            gameTab.transform.position += Vector3.left * 3.5f;
            roleTab.transform.position += Vector3.left * 3.5f;
            lasMonjasTab.transform.position += Vector3.left * 2.5f;
            lasMonjasGamemodeTab.transform.position += Vector3.left * 1.5f;
            lasMonjasImpostorTab.transform.position += Vector3.left * 0.5f;
            lasMonjasRebelTab.transform.position += Vector3.right * 0.5f;
            lasMonjasNeutralTab.transform.position += Vector3.right * 1.5f;
            lasMonjasCrewmateTab.transform.position += Vector3.right * 2.5f;

            var tabs = new GameObject[] { gameTab, roleTab, lasMonjasTab, lasMonjasGamemodeTab, lasMonjasImpostorTab, lasMonjasRebelTab, lasMonjasNeutralTab, lasMonjasCrewmateTab };
            for (int i = 0; i < tabs.Length; i++) {
                var button = tabs[i].GetComponentInChildren<PassiveButton>();
                if (button == null) continue;
                int copiedIndex = i;
                button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                button.OnClick.AddListener((System.Action)(() => {
                    gameSettingMenu.RegularGameSettings.SetActive(false);
                    gameSettingMenu.RolesSettings.gameObject.SetActive(false);
                    lasMonjasSettings.gameObject.SetActive(false);
                    lasMonjasGamemodes.gameObject.SetActive(false);
                    lasMonjasImpostors.gameObject.SetActive(false);
                    lasMonjasRebels.gameObject.SetActive(false);
                    lasMonjasNeutrals.gameObject.SetActive(false);
                    lasMonjasCrewmates.gameObject.SetActive(false); 
                    gameSettingMenu.GameSettingsHightlight.enabled = false;
                    gameSettingMenu.RolesSettingsHightlight.enabled = false;
                    lasMonjasTabHighlight.enabled = false;
                    lasMonjasGamemodeTabHighlight.enabled = false;
                    lasMonjasImpostorTabHighlight.enabled = false;
                    lasMonjasRebelTabHighlight.enabled = false;
                    lasMonjasNeutralTabHighlight.enabled = false;
                    lasMonjasCrewmateTabHighlight.enabled = false; 
                    if (copiedIndex == 0) {
                        gameSettingMenu.RegularGameSettings.SetActive(true);
                        gameSettingMenu.GameSettingsHightlight.enabled = true;
                    }
                    else if (copiedIndex == 1) {
                        gameSettingMenu.RolesSettings.gameObject.SetActive(true);
                        gameSettingMenu.RolesSettingsHightlight.enabled = true;
                    }
                    else if (copiedIndex == 2) {
                        lasMonjasSettings.gameObject.SetActive(true);
                        lasMonjasTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 3) {
                        lasMonjasGamemodes.gameObject.SetActive(true);
                        lasMonjasGamemodeTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 4) {
                        lasMonjasImpostors.gameObject.SetActive(true);
                        lasMonjasImpostorTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 5) {
                        lasMonjasRebels.gameObject.SetActive(true);
                        lasMonjasRebelTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 6) {
                        lasMonjasNeutrals.gameObject.SetActive(true);
                        lasMonjasNeutralTabHighlight.enabled = true;
                    }
                    else if (copiedIndex == 7) {
                        lasMonjasCrewmates.gameObject.SetActive(true);
                        lasMonjasCrewmateTabHighlight.enabled = true;
                    }
                }));
            }

            foreach (OptionBehaviour option in lasMonjasMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> lasMonjasOptions = new List<OptionBehaviour>();

            foreach (OptionBehaviour option in lasMonjasGamemodesMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> lasMonjasGamemodesOptions = new List<OptionBehaviour>(); 
            
            foreach (OptionBehaviour option in lasMonjasImpostorsMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> lasMonjasImpostorOptions = new List<OptionBehaviour>();

            foreach (OptionBehaviour option in lasMonjasRebelsMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> lasMonjasRebelOptions = new List<OptionBehaviour>(); 
            
            foreach (OptionBehaviour option in lasMonjasNeutralsMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> lasMonjasNeutralOptions = new List<OptionBehaviour>();

            foreach (OptionBehaviour option in lasMonjasCrewmatesMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> lasMonjasCrewmateOptions = new List<OptionBehaviour>(); 
            
            for (int i = 0; i < CustomOption.options.Count; i++) {
                CustomOption option = CustomOption.options[i];
                if (option.optionBehaviour == null) {
                    StringOption stringOption;
                    if (option.type == "gamemode") {
                        stringOption = UnityEngine.Object.Instantiate(template, lasMonjasGamemodesMenu.transform);
                        lasMonjasGamemodesOptions.Add(stringOption);
                    }
                    else if (option.type == "impostor") {
                        stringOption = UnityEngine.Object.Instantiate(template, lasMonjasImpostorsMenu.transform);
                        lasMonjasImpostorOptions.Add(stringOption);
                    }
                    else if (option.type == "rebel") {
                        stringOption = UnityEngine.Object.Instantiate(template, lasMonjasRebelsMenu.transform);
                        lasMonjasRebelOptions.Add(stringOption);
                    }
                    else if (option.type == "neutral") {
                        stringOption = UnityEngine.Object.Instantiate(template, lasMonjasNeutralsMenu.transform);
                        lasMonjasNeutralOptions.Add(stringOption);
                    }
                    else if (option.type == "crewmate") {
                        stringOption = UnityEngine.Object.Instantiate(template, lasMonjasCrewmatesMenu.transform);
                        lasMonjasCrewmateOptions.Add(stringOption);
                    }
                    else {
                        stringOption = UnityEngine.Object.Instantiate(template, lasMonjasMenu.transform);
                        lasMonjasOptions.Add(stringOption);
                    }
                    stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                    stringOption.TitleText.text = option.name;
                    stringOption.Value = stringOption.oldValue = option.selection;
                    stringOption.ValueText.text = option.selections[option.selection].ToString();

                    option.optionBehaviour = stringOption;
                }
                option.optionBehaviour.gameObject.SetActive(true);
            }

            lasMonjasMenu.Children = lasMonjasOptions.ToArray();
            lasMonjasSettings.gameObject.SetActive(false);

            lasMonjasGamemodesMenu.Children = lasMonjasGamemodesOptions.ToArray();
            lasMonjasGamemodes.gameObject.SetActive(false); 
            
            lasMonjasImpostorsMenu.Children = lasMonjasImpostorOptions.ToArray();
            lasMonjasImpostors.gameObject.SetActive(false);

            lasMonjasRebelsMenu.Children = lasMonjasRebelOptions.ToArray();
            lasMonjasRebels.gameObject.SetActive(false); 
            
            lasMonjasNeutralsMenu.Children = lasMonjasNeutralOptions.ToArray();
            lasMonjasNeutrals.gameObject.SetActive(false);

            lasMonjasCrewmatesMenu.Children = lasMonjasCrewmateOptions.ToArray();
            lasMonjasCrewmates.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance) {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            __instance.TitleText.text = option.name;
            __instance.Value = __instance.oldValue = option.selection;
            __instance.ValueText.text = option.selections[option.selection].ToString();

            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance) {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection + 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance) {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection - 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix() {
            CustomOption.ShareOptionSelections();
        }
    }


    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
    class GameOptionsMenuUpdatePatch
    {
        private static float timer = 1f;
        public static void Postfix(GameOptionsMenu __instance) {

            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
            if (gameSettingMenu.RegularGameSettings.active || gameSettingMenu.RolesSettings.gameObject.active) return;

            __instance.GetComponentInParent<Scroller>().ContentYBounds.max = -0.5F + __instance.Children.Length * 0.55F;
            timer += Time.deltaTime;
            if (timer < 0.1f) return;
            timer = 0f;

            float offset = 2.75f;
            foreach (CustomOption option in CustomOption.options) {
                if (GameObject.Find("LasMonjasSettings") && option.type != "setting")
                    continue;
                if (GameObject.Find("LasMonjasGamemodes") && option.type != "gamemode")
                    continue;
                if (GameObject.Find("LasMonjasImpostors") && option.type != "impostor")
                    continue;
                if (GameObject.Find("LasMonjasRebels") && option.type != "rebel")
                    continue;
                if (GameObject.Find("LasMonjasNeutrals") && option.type != "neutral")
                    continue;
                if (GameObject.Find("LasMonjasCrewmates") && option.type != "crewmate")
                    continue; 
                if (option?.optionBehaviour != null && option.optionBehaviour.gameObject != null) {
                    bool enabled = true;
                    var parent = option.parent;
                    while (parent != null && enabled) {
                        enabled = parent.selection != 0;
                        parent = parent.parent;
                    }
                    option.optionBehaviour.gameObject.SetActive(enabled);
                    if (enabled) {
                        offset -= option.isHeader ? 0.75f : 0.5f;
                        option.optionBehaviour.transform.localPosition = new Vector3(option.optionBehaviour.transform.localPosition.x, offset, option.optionBehaviour.transform.localPosition.z);
                    }
                }
            }
        }
    }

    /*[HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
    class GameSettingMenuStartPatch
    {
        public static void Prefix(GameSettingMenu __instance) {
            __instance.HideForOnline = new Transform[] { };
        }

        public static void Postfix(GameSettingMenu __instance) {

            var mapNameTransform = __instance.AllItems.FirstOrDefault(x => x.name.Equals("MapName", StringComparison.OrdinalIgnoreCase));
            if (mapNameTransform == null) return;

            var options = new Il2CppSystem.Collections.Generic.List<Il2CppSystem.Collections.Generic.KeyValuePair<string, int>>();
            for (int i = 0; i < Constants.MapNames.Length; i++) {
                // Dleks was removed from the game, so remove it from our selections.
                if (i == (int)MapNames.Dleks) continue; 
                
                var kvp = new Il2CppSystem.Collections.Generic.KeyValuePair<string, int>();
                kvp.key = Constants.MapNames[i];
                kvp.value = i;
                options.Add(kvp);
            }
            mapNameTransform.GetComponent<KeyValueOption>().Values = options;
            mapNameTransform.gameObject.active = true;

            foreach (Transform i in __instance.AllItems.ToList()) {
                float num = -0.5f;
                if (i.name.Equals("MapName", StringComparison.OrdinalIgnoreCase)) num = -0.25f;
                if (i.name.Equals("NumImpostors", StringComparison.OrdinalIgnoreCase) || i.name.Equals("ResetToDefault", StringComparison.OrdinalIgnoreCase)) num = 0f;
                i.position += new Vector3(0, num, 0);
            }
            __instance.Scroller.ContentYBounds.max += 0.5F;
        }
    }*/

    /*[HarmonyPatch(typeof(Constants), nameof(Constants.ShouldFlipSkeld))]
    class ConstantsShouldFlipSkeldPatch
    {
        public static bool Prefix(ref bool __result) {
            if (PlayerControl.GameOptions == null) return true;
            __result = PlayerControl.GameOptions.MapId == 3;
            return false;
        }
    }*/

    [HarmonyPatch]
    class GameOptionsDataPatch
    {
        private static IEnumerable<MethodBase> TargetMethods() {
            return typeof(GameOptionsData).GetMethods().Where(x => x.ReturnType == typeof(string) && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(int));
        }

        private static void Postfix(ref string __result) {
            StringBuilder sb = new StringBuilder(__result);
            foreach (CustomOption option in CustomOption.options) {
                if (option.parent == null) {
                    sb.AppendLine($"{option.name}: {option.selections[option.selection].ToString()}");
                }
            }
            CustomOption parent = null;
            foreach (CustomOption option in CustomOption.options)
                if (option.parent != null) {
                    if (option.parent != parent) {
                        sb.AppendLine();
                        parent = option.parent;
                    }
                    sb.AppendLine($"{option.name}: {option.selections[option.selection].ToString()}");
                }

            var hudString = sb.ToString();

            int defaultSettingsLines = 23;
            int roleSettingsLines = defaultSettingsLines + 37;
            int detailedSettingsP1 = roleSettingsLines + 43;
            int detailedSettingsP2 = detailedSettingsP1 + 29;
            int detailedSettingsP3 = detailedSettingsP2 + 27;
            int detailedSettingsP4 = detailedSettingsP3 + 42;
            int detailedSettingsP5 = detailedSettingsP4 + 40;
            int detailedSettingsP6 = detailedSettingsP5 + 46;
            int end1 = hudString.TakeWhile(c => (defaultSettingsLines -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end2 = hudString.TakeWhile(c => (roleSettingsLines -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end3 = hudString.TakeWhile(c => (detailedSettingsP1 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end4 = hudString.TakeWhile(c => (detailedSettingsP2 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end5 = hudString.TakeWhile(c => (detailedSettingsP3 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end6 = hudString.TakeWhile(c => (detailedSettingsP4 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end7 = hudString.TakeWhile(c => (detailedSettingsP5 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end8 = hudString.TakeWhile(c => (detailedSettingsP6 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int counter = LasMonjasPlugin.optionsPage;
            switch (counter) {
                case 0:
                    hudString = hudString.Substring(0, end1) + "\n";
                    break;
                case 1:
                    hudString = hudString.Substring(end1 + 1, end2 - end1);
                    int gap = 1;
                    int index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(index, "\n");
                    gap = 11;
                    index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(index, "\n");
                    gap = 25;
                    index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(index, "\n");
                    gap = 33;
                    index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(index + 1, "\n"); break;
                case 2:
                    hudString = hudString.Substring(end2 + 1, end3 - end2);
                    int gaptwo = 0;
                    int indextwo = hudString.TakeWhile(c => (gaptwo -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(indextwo, "\n"); break;
                case 3:
                    hudString = hudString.Substring(end3 + 1, end4 - end3);
                    break;
                case 4:
                    hudString = hudString.Substring(end4 + 1, end5 - end4);
                    break;
                case 5:
                    hudString = hudString.Substring(end5 + 1, end6 - end5);
                    break;
                case 6:
                    hudString = hudString.Substring(end6 + 1, end7 - end6);
                    break;
                case 7:
                    hudString = hudString.Substring(end7 + 1, end8 - end7);
                    break;
                case 8:
                    hudString = hudString.Substring(end8 + 1);
                    break;
            }

            hudString += $"\nTab for next page ({counter + 1}/9)";
            __result = hudString;
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                LasMonjasPlugin.optionsPage = (LasMonjasPlugin.optionsPage + 1) % 9;
            }
        }
    }


    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class GameSettingsScalePatch
    {
        public static void Prefix(HudManager __instance) {
            if (__instance.GameSettings != null) __instance.GameSettings.fontSize = 1.2f;
        }
    }
}