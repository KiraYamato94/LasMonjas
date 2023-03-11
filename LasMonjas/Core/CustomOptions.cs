using System.Collections.Generic;
using UnityEngine;
using BepInEx.Configuration;
using System;
using System.Linq;
using HarmonyLib;
using Hazel;
using System.Reflection;
using System.Text;
using AmongUs.GameOptions;

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
                    if (id == 0 && selection != preset) {
                        switchPreset(selection); // Switch presets
                        ShareOptionSelections();
                    }
                    else if (entry != null) {
                        entry.Value = selection; // Save selection to config
                        ShareOptionChange((uint)id);// Share single selection
                    }
                }
            }
            else if (id == 0 && AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer) {  // Share the preset
                switchPreset(selection);
                ShareOptionSelections();// Share all selections
            }
        }
        public static void ShareOptionChange(uint optionId) {
            var option = options.FirstOrDefault(x => x.id == optionId);
            if (option == null) return;
            var writer = AmongUsClient.Instance!.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareOptions, SendOption.Reliable, -1);
            writer.Write((byte)1);
            writer.WritePacked((uint)option.id);
            writer.WritePacked(Convert.ToUInt32(option.selection));
            AmongUsClient.Instance.FinishRpcImmediately(writer);
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

            if (GameObject.Find("LasMonjasModifiers") != null) {
                GameObject.Find("LasMonjasModifiers").transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText("Las Monjas - Modifiers");
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

            var lasMonjasModifiers = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var lasMonjasModifiersMenu = lasMonjasModifiers.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
            lasMonjasModifiers.name = "LasMonjasModifiers"; 

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

            var lasMonjasModifiersTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var lasMonjasModifiersTabHighlight = lasMonjasModifiersTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            lasMonjasModifiersTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TabIconModifiers.png", 100f);
            lasMonjasModifiersTab.name = "LasMonjasModifiersTab";

            // Position of Tab Icons
            gameTab.transform.position += Vector3.left * 3.5f;
            roleTab.transform.position += Vector3.left * 3.5f;
            lasMonjasTab.transform.position += Vector3.left * 2.5f;
            lasMonjasGamemodeTab.transform.position += Vector3.left * 1.75f;
            lasMonjasImpostorTab.transform.position += Vector3.left * 1f;
            lasMonjasRebelTab.transform.position += Vector3.left * 0.25f;
            lasMonjasNeutralTab.transform.position += Vector3.right * 0.5f;
            lasMonjasCrewmateTab.transform.position += Vector3.right * 1.25f;
            lasMonjasModifiersTab.transform.position += Vector3.right * 2f;

            var tabs = new GameObject[] { gameTab, roleTab, lasMonjasTab, lasMonjasGamemodeTab, lasMonjasImpostorTab, lasMonjasRebelTab, lasMonjasNeutralTab, lasMonjasCrewmateTab, lasMonjasModifiersTab };
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
                    lasMonjasModifiers.gameObject.SetActive(false);
                    gameSettingMenu.GameSettingsHightlight.enabled = false;
                    gameSettingMenu.RolesSettingsHightlight.enabled = false;
                    lasMonjasTabHighlight.enabled = false;
                    lasMonjasGamemodeTabHighlight.enabled = false;
                    lasMonjasImpostorTabHighlight.enabled = false;
                    lasMonjasRebelTabHighlight.enabled = false;
                    lasMonjasNeutralTabHighlight.enabled = false;
                    lasMonjasCrewmateTabHighlight.enabled = false;
                    lasMonjasModifiersTabHighlight.enabled = false;
                    switch (copiedIndex) {
                        case 0:
                            gameSettingMenu.RegularGameSettings.SetActive(true);
                            gameSettingMenu.GameSettingsHightlight.enabled = true;
                            break;
                        case 1:
                            gameSettingMenu.RolesSettings.gameObject.SetActive(true);
                            gameSettingMenu.RolesSettingsHightlight.enabled = true;
                            break;
                        case 2:
                            lasMonjasSettings.gameObject.SetActive(true);
                            lasMonjasTabHighlight.enabled = true;
                            break;
                        case 3:
                            lasMonjasGamemodes.gameObject.SetActive(true);
                            lasMonjasGamemodeTabHighlight.enabled = true;
                            break;
                        case 4:
                            lasMonjasImpostors.gameObject.SetActive(true);
                            lasMonjasImpostorTabHighlight.enabled = true;
                            break;
                        case 5:
                            lasMonjasRebels.gameObject.SetActive(true);
                            lasMonjasRebelTabHighlight.enabled = true;
                            break;
                        case 6:
                            lasMonjasNeutrals.gameObject.SetActive(true);
                            lasMonjasNeutralTabHighlight.enabled = true;
                            break;
                        case 7:
                            lasMonjasCrewmates.gameObject.SetActive(true);
                            lasMonjasCrewmateTabHighlight.enabled = true;
                            break;
                        case 8:
                            lasMonjasModifiers.gameObject.SetActive(true);
                            lasMonjasModifiersTabHighlight.enabled = true;
                            break;
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

            foreach (OptionBehaviour option in lasMonjasModifiersMenu.GetComponentsInChildren<OptionBehaviour>())
                UnityEngine.Object.Destroy(option.gameObject);
            List<OptionBehaviour> lasMonjasModifiersOptions = new List<OptionBehaviour>();

            for (int i = 0; i < CustomOption.options.Count; i++) {
                CustomOption option = CustomOption.options[i];
                if (option.optionBehaviour == null) {
                    StringOption stringOption;
                    switch (option.type) {
                        case "gamemode":
                            stringOption = UnityEngine.Object.Instantiate(template, lasMonjasGamemodesMenu.transform);
                            lasMonjasGamemodesOptions.Add(stringOption);
                            break;
                        case "impostor":
                            stringOption = UnityEngine.Object.Instantiate(template, lasMonjasImpostorsMenu.transform);
                            lasMonjasImpostorOptions.Add(stringOption);
                            break;
                        case "rebel":
                            stringOption = UnityEngine.Object.Instantiate(template, lasMonjasRebelsMenu.transform);
                            lasMonjasRebelOptions.Add(stringOption);
                            break;
                        case "neutral":
                            stringOption = UnityEngine.Object.Instantiate(template, lasMonjasNeutralsMenu.transform);
                            lasMonjasNeutralOptions.Add(stringOption);
                            break;
                        case "crewmate":
                            stringOption = UnityEngine.Object.Instantiate(template, lasMonjasCrewmatesMenu.transform);
                            lasMonjasCrewmateOptions.Add(stringOption);
                            break;
                        case "modifier":
                            stringOption = UnityEngine.Object.Instantiate(template, lasMonjasModifiersMenu.transform);
                            lasMonjasModifiersOptions.Add(stringOption);
                            break;
                        default:
                            stringOption = UnityEngine.Object.Instantiate(template, lasMonjasMenu.transform);
                            lasMonjasOptions.Add(stringOption);
                            break;
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

            lasMonjasModifiersMenu.Children = lasMonjasModifiersOptions.ToArray();
            lasMonjasModifiers.gameObject.SetActive(false);
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

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
    public class AmongUsClientOnPlayerJoinedPatch
    {
        public static void Postfix() {
            if (PlayerControl.LocalPlayer != null) {
                CustomOption.ShareOptionSelections();
            }
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
                if (GameObject.Find("LasMonjasModifiers") && option.type != "modifier")
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

    /*[HarmonyPatch(typeof(Constants), nameof(Constants.ShouldFlipSkeld))]
    class ConstantsShouldFlipSkeldPatch
    {
        public static bool Prefix(ref bool __result) {
            if (PlayerControl.GameOptions == null) return true;
            __result = GameOptionsManager.Instance.currentGameOptions.MapId == 3;
            return false;
        }
    }*/

    [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
    class GameOptionsDataPatch
    {
        private static IEnumerable<MethodBase> TargetMethods() {
            return typeof(GameOptionsData).GetMethods().Where(x => x.ReturnType == typeof(string) && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(int));
        }

        private static void Postfix(ref string __result) {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;
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
            int roleSettingsLines = defaultSettingsLines + 41;
            int detailedSettingsP1 = roleSettingsLines + 25;
            int detailedSettingsP2 = detailedSettingsP1 + 28;
            int detailedSettingsP3 = detailedSettingsP2 + 25;
            int detailedSettingsP4 = detailedSettingsP3 + 45;
            int detailedSettingsP5 = detailedSettingsP4 + 40;
            int detailedSettingsP6 = detailedSettingsP5 + 26;
            int detailedSettingsP7 = detailedSettingsP6 + 34;
            int detailedSettingsP8 = detailedSettingsP7 + 24;
            int end1 = hudString.TakeWhile(c => (defaultSettingsLines -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end2 = hudString.TakeWhile(c => (roleSettingsLines -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end3 = hudString.TakeWhile(c => (detailedSettingsP1 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end4 = hudString.TakeWhile(c => (detailedSettingsP2 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end5 = hudString.TakeWhile(c => (detailedSettingsP3 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end6 = hudString.TakeWhile(c => (detailedSettingsP4 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end7 = hudString.TakeWhile(c => (detailedSettingsP5 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end8 = hudString.TakeWhile(c => (detailedSettingsP6 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end9 = hudString.TakeWhile(c => (detailedSettingsP7 -= (c == '\n' ? 1 : 0)) > 0).Count();
            int end10 = hudString.TakeWhile(c => (detailedSettingsP8 -= (c == '\n' ? 1 : 0)) > 0).Count();
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
                    gap = 7;
                    index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(index, "\n");
                    gap = 23;
                    index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(index, "\n");
                    gap = 34;
                    index = hudString.TakeWhile(c => (gap -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(index + 1, "\n"); 
                    break;
                case 2:
                    hudString = hudString.Substring(end2 + 1, end3 - end2);
                    int gaptwo = 0;
                    int indextwo = hudString.TakeWhile(c => (gaptwo -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(indextwo, "\n");
                    break;
                case 3:
                    hudString = hudString.Substring(end3 + 1, end4 - end3);
                    int gapthree = 0;
                    int indexthree = hudString.TakeWhile(c => (gapthree -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(indexthree, "\n"); 
                    break;
                case 4:
                    hudString = hudString.Substring(end4 + 1, end5 - end4);
                    int gapfour = 2;
                    int indexfour = hudString.TakeWhile(c => (gapfour -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(indexfour, "\n");
                    gapfour = 10;
                    indexfour = hudString.TakeWhile(c => (gapfour -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(indexfour, "\n");
                    gapfour = 14;
                    indexfour = hudString.TakeWhile(c => (gapfour -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(indexfour, "\n");
                    gapfour = 19;
                    indexfour = hudString.TakeWhile(c => (gapfour -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(indexfour, "\n");
                    gapfour = 25;
                    indexfour = hudString.TakeWhile(c => (gapfour -= (c == '\n' ? 1 : 0)) > 0).Count();
                    hudString = hudString.Insert(indexfour, "\n");
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
                    hudString = hudString.Substring(end8 + 1, end9 - end8);
                    break;
                case 9:
                    hudString = hudString.Substring(end9 + 1, end10 - end9);
                    break;
                case 10:
                    hudString = hudString.Substring(end10 + 1);
                    break;
            }

            hudString += $"\n{Language.helpersTexts[5]} ({counter + 1}/11)";
            __result = hudString;
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                LasMonjasPlugin.optionsPage = (LasMonjasPlugin.optionsPage + 1) % 11;
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

    // This class is taken from Town of Us Reactivated, https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/CustomOption/Patches.cs, Licensed under GPLv3
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        public static float
            MinX,/*-5.3F*/
            OriginalY = 2.9F,
            MinY = 2.9F;


        public static Scroller Scroller;
        private static Vector3 LastPosition;
        private static float lastAspect;
        private static bool setLastPosition = false;

        public static void Prefix(HudManager __instance) {
            if (__instance.GameSettings?.transform == null) return;

            // Sets the MinX position to the left edge of the screen + 0.1 units
            Rect safeArea = Screen.safeArea;
            float aspect = Mathf.Min((Camera.main).aspect, safeArea.width / safeArea.height);
            float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
            MinX = 0.1f - safeOrthographicSize * aspect;

            if (!setLastPosition || aspect != lastAspect) {
                LastPosition = new Vector3(MinX, MinY);
                lastAspect = aspect;
                setLastPosition = true;
                if (Scroller != null) Scroller.ContentXBounds = new FloatRange(MinX, MinX);
            }

            CreateScroller(__instance);

            Scroller.gameObject.SetActive(__instance.GameSettings.gameObject.activeSelf);

            if (!Scroller.gameObject.active) return;

            var rows = __instance.GameSettings.text.Count(c => c == '\n');
            float LobbyTextRowHeight = 0.06F;
            var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

            Scroller.ContentYBounds = new FloatRange(MinY, maxY);

            // Prevent scrolling when the player is interacting with a menu
            if (PlayerControl.LocalPlayer?.CanMove != true) {
                __instance.GameSettings.transform.localPosition = LastPosition;

                return;
            }

            if (__instance.GameSettings.transform.localPosition.x != MinX ||
                __instance.GameSettings.transform.localPosition.y < MinY) return;

            LastPosition = __instance.GameSettings.transform.localPosition;
        }

        private static void CreateScroller(HudManager __instance) {
            if (Scroller != null) return;

            Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
            Scroller.transform.SetParent(__instance.GameSettings.transform.parent);
            Scroller.gameObject.layer = 5;

            Scroller.transform.localScale = Vector3.one;
            Scroller.allowX = false;
            Scroller.allowY = true;
            Scroller.active = true;
            Scroller.velocity = new Vector2(0, 0);
            Scroller.ScrollbarYBounds = new FloatRange(0, 0);
            Scroller.ContentXBounds = new FloatRange(MinX, MinX);
            Scroller.enabled = true;

            Scroller.Inner = __instance.GameSettings.transform;
            __instance.GameSettings.transform.SetParent(Scroller.transform);
        }
    }
}