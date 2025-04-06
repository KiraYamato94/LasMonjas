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
using Reactor.Utilities.Extensions;
using static LasMonjas.Core.CustomOption;
using TMPro;
using BepInEx.Unity.IL2CPP;
using BepInEx;

namespace LasMonjas.Core
{
    public class CustomOption
    {
        public enum CustomOptionType
        {
            General,
            Impostor,
            Rebel,
            Neutral,
            Crewmate,
            Modifier,
        }

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
        public CustomOptionType type;
        public Action onChange = null;
        public string heading = "";

        public CustomOption(int id, CustomOptionType type, string name, System.Object[] selections, System.Object defaultValue, CustomOption parent, bool isHeader, Action onChange = null, string heading = "") {
            this.id = id;
            this.name = parent == null ? name : "- " + name;
            this.selections = selections;
            int index = Array.IndexOf(selections, defaultValue);
            this.defaultSelection = index >= 0 ? index : 0;
            this.parent = parent;
            this.isHeader = isHeader;
            this.type = type;
            this.onChange = onChange;
            this.heading = heading;
            selection = 0;
            if (id != 0) {
                entry = LasMonjasPlugin.Instance.Config.Bind($"Preset{preset}", id.ToString(), defaultSelection);
                selection = Mathf.Clamp(entry.Value, 0, selections.Length - 1);
            }
            options.Add(this);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, string[] selections, CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "") {
            return new CustomOption(id, type, name, selections, "", parent, isHeader, onChange, heading);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, float defaultValue, float min, float max, float step, CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "") {
            List<object> selections = new();
            for (float s = min; s <= max; s += step)
                selections.Add(s);
            return new CustomOption(id, type, name, selections.ToArray(), defaultValue, parent, isHeader, onChange, heading);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, bool defaultValue, CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "") {
            return new CustomOption(id, type, name, new string[] { "Off", "On" }, defaultValue ? "On" : "Off", parent, isHeader, onChange, heading);
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

                if (AmongUsClient.Instance?.AmHost == true && PlayerInCache.LocalPlayer.PlayerControl) {
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

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.ChangeTab))]
    class GameOptionsMenuChangeTabPatch
    {
        public static void Postfix(GameSettingMenu __instance, int tabNum, bool previewOnly) {
            if (previewOnly) return;
            foreach (var tab in GameOptionsMenuStartPatch.currentTabs) {
                if (tab != null)
                    tab.SetActive(false);
            }
            foreach (var pbutton in GameOptionsMenuStartPatch.currentButtons) {
                pbutton.SelectButton(false);
            }
            if (tabNum > 2) {
                tabNum -= 3;
                GameOptionsMenuStartPatch.currentTabs[tabNum].SetActive(true);
                GameOptionsMenuStartPatch.currentButtons[tabNum].SelectButton(true);
            }
        }
    }

    [HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.SetTab))]
    class LobbyViewSettingsPaneRefreshTabPatch
    {
        public static bool Prefix(LobbyViewSettingsPane __instance) {
            if ((int)__instance.currentTab < 15) {
                LobbyViewSettingsPaneChangeTabPatch.Postfix(__instance, __instance.currentTab);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.ChangeTab))]
    class LobbyViewSettingsPaneChangeTabPatch
    {
        public static void Postfix(LobbyViewSettingsPane __instance, StringNames category) {
            int tabNum = (int)category;

            foreach (var pbutton in LobbyViewSettingsPatch.currentButtons) {
                pbutton.SelectButton(false);
            }
            if (tabNum > 20) // StringNames are in the range of 3000+ 
                return;
            __instance.taskTabButton.SelectButton(false);

            if (tabNum > 2) {
                tabNum -= 3;
                //GameOptionsMenuStartPatch.currentTabs[tabNum].SetActive(true);
                LobbyViewSettingsPatch.currentButtons[tabNum].SelectButton(true);
                LobbyViewSettingsPatch.drawTab(__instance, LobbyViewSettingsPatch.currentButtonTypes[tabNum]);
            }
        }
    }

    [HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.Update))]
    class LobbyViewSettingsPaneUpdatePatch
    {
        public static void Postfix(LobbyViewSettingsPane __instance) {
            if (LobbyViewSettingsPatch.currentButtons.Count == 0) {
                LobbyViewSettingsPatch.gameModeChangedFlag = true;
                LobbyViewSettingsPatch.Postfix(__instance);

            }
        }
    }

    [HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.Awake))]
    class LobbyViewSettingsPatch
    {
        public static List<PassiveButton> currentButtons = new();
        public static List<CustomOptionType> currentButtonTypes = new();
        public static bool gameModeChangedFlag = false;

        public static void createCustomButton(LobbyViewSettingsPane __instance, int targetMenu, string buttonName, string buttonText, CustomOptionType optionType) {
            buttonName = "View" + buttonName;
            var buttonTemplate = GameObject.Find("OverviewTab");
            var lmjSettingsButton = GameObject.Find(buttonName);
            if (lmjSettingsButton == null) {
                lmjSettingsButton = GameObject.Instantiate(buttonTemplate, buttonTemplate.transform.parent);
                lmjSettingsButton.transform.localPosition += Vector3.right * 1.75f * (targetMenu - 2);
                lmjSettingsButton.name = buttonName;
                __instance.StartCoroutine(Effects.Lerp(2f, new Action<float>(p => { lmjSettingsButton.transform.FindChild("FontPlacer").GetComponentInChildren<TextMeshPro>().text = buttonText; })));
                var lmjSettingsPassiveButton = lmjSettingsButton.GetComponent<PassiveButton>();
                lmjSettingsPassiveButton.OnClick.RemoveAllListeners();
                lmjSettingsPassiveButton.OnClick.AddListener((System.Action)(() => {
                    __instance.ChangeTab((StringNames)targetMenu);
                }));
                lmjSettingsPassiveButton.OnMouseOut.RemoveAllListeners();
                lmjSettingsPassiveButton.OnMouseOver.RemoveAllListeners();
                lmjSettingsPassiveButton.SelectButton(false);
                currentButtons.Add(lmjSettingsPassiveButton);
                currentButtonTypes.Add(optionType);
            }
        }

        public static void Postfix(LobbyViewSettingsPane __instance) {
            currentButtons.ForEach(x => x?.Destroy());
            currentButtons.Clear();
            currentButtonTypes.Clear();

            removeVanillaTabs(__instance);

            createSettingTabs(__instance);

        }

        public static void removeVanillaTabs(LobbyViewSettingsPane __instance) {
            GameObject.Find("RolesTabs")?.Destroy();
            var overview = GameObject.Find("OverviewTab");
            if (!gameModeChangedFlag) {
                overview.transform.localScale = new Vector3(0.5f * overview.transform.localScale.x, overview.transform.localScale.y, overview.transform.localScale.z);
                overview.transform.localPosition += new Vector3(-1.2f, 0f, 0f);

            }
            overview.transform.Find("FontPlacer").transform.localScale = new Vector3(1.35f, 1f, 1f);
            overview.transform.Find("FontPlacer").transform.localPosition = new Vector3(-0.6f, -0.1f, 0f);
            gameModeChangedFlag = false;
        }

        public static void drawTab(LobbyViewSettingsPane __instance, CustomOptionType optionType) {

            var relevantOptions = options.Where(x => x.type == optionType || optionType == CustomOptionType.General).ToList();

            if ((int)optionType == 99) {
                // Create 5 Groups with Role settings
                relevantOptions.Clear();
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Impostor && x.isHeader));
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Rebel && x.isHeader));
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Neutral && x.isHeader));
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Crewmate && x.isHeader));
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Modifier && x.isHeader));
            }

            for (int j = 0; j < __instance.settingsInfo.Count; j++) {
                __instance.settingsInfo[j].gameObject.Destroy();
            }
            __instance.settingsInfo.Clear();

            float num = 1.44f;
            int i = 0;
            int singles = 1;
            int headers = 0;
            int lines = 0;
            var curType = CustomOptionType.Modifier;
            int numBonus = 0;

            foreach (var option in relevantOptions) {
                if (option.isHeader && (int)optionType != 99 || (int)optionType == 99 && curType != option.type) {
                    curType = option.type;
                    if (i != 0)
                    {
                        num -= 0.85f;
                        numBonus++;
                    }
                    if (i % 2 != 0) singles++;
                    headers++; // for header
                    CategoryHeaderMasked categoryHeaderMasked = UnityEngine.Object.Instantiate<CategoryHeaderMasked>(__instance.categoryHeaderOrigin);
                    categoryHeaderMasked.SetHeader(StringNames.ImpostorsCategory, 61);
                    categoryHeaderMasked.Title.text = option.heading != "" ? option.heading : option.name;
                    if ((int)optionType == 99)
                        categoryHeaderMasked.Title.text = new Dictionary<CustomOptionType, string>() {
                            { CustomOptionType.Impostor, "Impostor Roles" },
                            { CustomOptionType.Rebel, "Rebel Roles" },
                            { CustomOptionType.Neutral, "Neutral Roles" },
                            { CustomOptionType.Crewmate, "Crewmate Roles" },
                            { CustomOptionType.Modifier, "Modifiers" } }[curType];
                    //categoryHeaderMasked.Title.outlineColor = Color.white;
                    //categoryHeaderMasked.Title.outlineWidth = 0.2f;
                    categoryHeaderMasked.transform.SetParent(__instance.settingsContainer);
                    categoryHeaderMasked.transform.localScale = Vector3.one;
                    categoryHeaderMasked.transform.localPosition = new Vector3(-9.77f, num, -2f);
                    __instance.settingsInfo.Add(categoryHeaderMasked.gameObject);
                    num -= 1.05f;
                    i = 0;
                }

                ViewSettingsInfoPanel viewSettingsInfoPanel = UnityEngine.Object.Instantiate<ViewSettingsInfoPanel>(__instance.infoPanelOrigin);
                viewSettingsInfoPanel.transform.SetParent(__instance.settingsContainer);
                viewSettingsInfoPanel.transform.localScale = Vector3.one;
                float num2;
                if (i % 2 == 0) {
                    lines++;
                    num2 = -8.95f;
                    if (i > 0) {
                        num -= 0.85f;
                    }
                }
                else {
                    num2 = -3f;
                }
                viewSettingsInfoPanel.transform.localPosition = new Vector3(num2, num, -2f);
                int value = option.getSelection();
                viewSettingsInfoPanel.SetInfo(StringNames.ImpostorsCategory, option.selections[value].ToString(), 61);
                viewSettingsInfoPanel.titleText.text = option.name;
                if (option.isHeader && (int)optionType != 99 && option.heading == "" && (option.type == CustomOptionType.Rebel || option.type == CustomOptionType.Neutral || option.type == CustomOptionType.Crewmate || option.type == CustomOptionType.Impostor || option.type == CustomOptionType.Modifier)) {
                    viewSettingsInfoPanel.titleText.text = "Spawn Chance";
                }
                /*if ((int)optionType == 99) {
                    viewSettingsInfoPanel.titleText.outlineColor = Color.white;
                    viewSettingsInfoPanel.titleText.outlineWidth = 0.2f;
                }*/
                __instance.settingsInfo.Add(viewSettingsInfoPanel.gameObject);

                i++;
            }
            float actual_spacing = (headers * 1.05f + lines * 0.85f) / (headers + lines) * 1.01f;
            __instance.scrollBar.CalculateAndSetYBounds((float)(__instance.settingsInfo.Count + singles * 2 + headers), 2f, 5f, actual_spacing);
        }

        public static void createSettingTabs(LobbyViewSettingsPane __instance) {
            // Handle different gamemodes and tabs needed therein.
            int next = 3;

            // create LMJ Settings
            createCustomButton(__instance, next++, "lmjSettings", "LMJ Settings", CustomOptionType.General);
            // create Role Settings
            createCustomButton(__instance, next++, "RoleOverview", "Role Overview", (CustomOptionType)99);
            // Imp
            createCustomButton(__instance, next++, "ImpostorSettings", "Impostor Roles", CustomOptionType.Impostor);
            // Rebel
            createCustomButton(__instance, next++, "RebelSettings", "Rebel Roles", CustomOptionType.Rebel);
            // Neutral
            createCustomButton(__instance, next++, "NeutralSettings", "Neutral Roles", CustomOptionType.Neutral);
            // Crew
            createCustomButton(__instance, next++, "CrewmateSettings", "Crewmate Roles", CustomOptionType.Crewmate);
            // Modifier
            createCustomButton(__instance, next++, "ModifierSettings", "Modifiers", CustomOptionType.Modifier);
        }
    }

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
    class GameOptionsMenuStartPatch
    {
        public static List<GameObject> currentTabs = new();
        public static List<PassiveButton> currentButtons = new();

        public static void Postfix(GameSettingMenu __instance) {
            currentTabs.ForEach(x => x?.Destroy());
            currentButtons.ForEach(x => x?.Destroy());
            currentTabs = new();
            currentButtons = new();

            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;

            removeVanillaTabs(__instance);

            createSettingTabs(__instance);

            var GOMGameObject = GameObject.Find("GAME SETTINGS TAB");
        }

        private static void createSettings(GameOptionsMenu menu, List<CustomOption> options) {
            float num = 1.5f;
            foreach (CustomOption option in options) {
                if (option.isHeader) {
                    CategoryHeaderMasked categoryHeaderMasked = UnityEngine.Object.Instantiate<CategoryHeaderMasked>(menu.categoryHeaderOrigin, Vector3.zero, Quaternion.identity, menu.settingsContainer);
                    categoryHeaderMasked.SetHeader(StringNames.ImpostorsCategory, 20);
                    categoryHeaderMasked.Title.text = option.heading != "" ? option.heading : option.name;
                    //categoryHeaderMasked.Title.outlineColor = Color.white;
                    //categoryHeaderMasked.Title.outlineWidth = 0.2f;
                    categoryHeaderMasked.transform.localScale = Vector3.one * 0.63f;
                    categoryHeaderMasked.transform.localPosition = new Vector3(-0.903f, num, -2f);
                    num -= 0.63f;
                }
                OptionBehaviour optionBehaviour = UnityEngine.Object.Instantiate<StringOption>(menu.stringOptionOrigin, Vector3.zero, Quaternion.identity, menu.settingsContainer);
                optionBehaviour.transform.localPosition = new Vector3(0.952f, num, -2f);
                optionBehaviour.SetClickMask(menu.ButtonClickMask);

                // "SetUpFromData"
                SpriteRenderer[] componentsInChildren = optionBehaviour.GetComponentsInChildren<SpriteRenderer>(true);
                for (int i = 0; i < componentsInChildren.Length; i++) {
                    componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, 20);
                }
                foreach (TextMeshPro textMeshPro in optionBehaviour.GetComponentsInChildren<TextMeshPro>(true)) {
                    textMeshPro.fontMaterial.SetFloat("_StencilComp", 3f);
                    textMeshPro.fontMaterial.SetFloat("_Stencil", (float)20);
                }

                var stringOption = optionBehaviour as StringOption;
                stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                stringOption.TitleText.text = option.name;
                if (option.isHeader && option.heading == "" && (option.type == CustomOptionType.Rebel || option.type == CustomOptionType.Neutral || option.type == CustomOptionType.Crewmate || option.type == CustomOptionType.Impostor || option.type == CustomOptionType.Modifier)) {
                    stringOption.TitleText.text = "Spawn Role";
                }
                if (stringOption.TitleText.text.Length > 25)
                    stringOption.TitleText.fontSize = 2.2f;
                if (stringOption.TitleText.text.Length > 40)
                    stringOption.TitleText.fontSize = 2f;
                stringOption.Value = stringOption.oldValue = option.selection;
                stringOption.ValueText.text = option.selections[option.selection].ToString();
                option.optionBehaviour = stringOption;

                menu.Children.Add(optionBehaviour);
                num -= 0.45f;
                menu.scrollBar.SetYBoundsMax(-num - 1.65f);
            }

            for (int i = 0; i < menu.Children.Count; i++) {
                OptionBehaviour optionBehaviour = menu.Children[i];
                if (AmongUsClient.Instance && !AmongUsClient.Instance.AmHost) {
                    optionBehaviour.SetAsPlayer();
                }
            }
        }

        private static void removeVanillaTabs(GameSettingMenu __instance) {
            GameObject.Find("What Is This?")?.Destroy();
            GameObject.Find("GamePresetButton")?.Destroy();
            GameObject.Find("RoleSettingsButton")?.Destroy();
            __instance.ChangeTab(1, false);
        }

        public static void createCustomButton(GameSettingMenu __instance, int targetMenu, string buttonName, string buttonText) {
            var leftPanel = GameObject.Find("LeftPanel");
            var buttonTemplate = GameObject.Find("GameSettingsButton");
            if (targetMenu == 3) {
                buttonTemplate.transform.localPosition -= Vector3.up * 0.85f;
                buttonTemplate.transform.localScale *= Vector2.one * 0.75f;
            }
            var lmjSettingsButton = GameObject.Find(buttonName);
            if (lmjSettingsButton == null) {
                lmjSettingsButton = GameObject.Instantiate(buttonTemplate, leftPanel.transform);
                lmjSettingsButton.transform.localPosition += Vector3.up * 0.5f * (targetMenu - 2);
                lmjSettingsButton.name = buttonName;
                __instance.StartCoroutine(Effects.Lerp(2f, new Action<float>(p => { lmjSettingsButton.transform.FindChild("FontPlacer").GetComponentInChildren<TextMeshPro>().text = buttonText; })));
                var lmjSettingsPassiveButton = lmjSettingsButton.GetComponent<PassiveButton>();
                lmjSettingsPassiveButton.OnClick.RemoveAllListeners();
                lmjSettingsPassiveButton.OnClick.AddListener((System.Action)(() => {
                    __instance.ChangeTab(targetMenu, false);
                }));
                lmjSettingsPassiveButton.OnMouseOut.RemoveAllListeners();
                lmjSettingsPassiveButton.OnMouseOver.RemoveAllListeners();
                lmjSettingsPassiveButton.SelectButton(false);
                currentButtons.Add(lmjSettingsPassiveButton);
            }
        }

        public static void createGameOptionsMenu(GameSettingMenu __instance, CustomOptionType optionType, string settingName) {
            var tabTemplate = GameObject.Find("GAME SETTINGS TAB");
            currentTabs.RemoveAll(x => x == null);

            var lmjSettingsTab = GameObject.Instantiate(tabTemplate, tabTemplate.transform.parent);
            lmjSettingsTab.name = settingName;

            var lmjSettingsGOM = lmjSettingsTab.GetComponent<GameOptionsMenu>();
            foreach (var child in lmjSettingsGOM.Children) {
                child.Destroy();
            }
            lmjSettingsGOM.scrollBar.transform.FindChild("SliderInner").DestroyChildren();
            lmjSettingsGOM.Children.Clear();
            var relevantOptions = options.Where(x => x.type == optionType).ToList();
            createSettings(lmjSettingsGOM, relevantOptions);

            currentTabs.Add(lmjSettingsTab);
            lmjSettingsTab.SetActive(false);
        }

        private static void createSettingTabs(GameSettingMenu __instance) {
            int next = 3;

            // create LMJ Settings
            createCustomButton(__instance, next++, "lmjSettings", "LMJ Settings");
            createGameOptionsMenu(__instance, CustomOptionType.General, "lmjSettings");

            // Imp
            createCustomButton(__instance, next++, "ImpostorSettings", "Impostor Roles");
            createGameOptionsMenu(__instance, CustomOptionType.Impostor, "ImpostorSettings");

            // Rebel
            createCustomButton(__instance, next++, "RebelSettings", "Rebel Roles");
            createGameOptionsMenu(__instance, CustomOptionType.Rebel, "RebelSettings");

            // Neutral
            createCustomButton(__instance, next++, "NeutralSettings", "Neutral Roles");
            createGameOptionsMenu(__instance, CustomOptionType.Neutral, "NeutralSettings");

            // Crew
            createCustomButton(__instance, next++, "CrewmateSettings", "Crewmate Roles");
            createGameOptionsMenu(__instance, CustomOptionType.Crewmate, "CrewmateSettings");

            // Modifier
            createCustomButton(__instance, next++, "ModifierSettings", "Modifiers");
            createGameOptionsMenu(__instance, CustomOptionType.Modifier, "ModifierSettings");
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance) {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            //__instance.TitleText.text = option.name;
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

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.FixedUpdate))]
    public class StringOptionFixedUpdate
    {
        public static void Postfix(StringOption __instance) {
            if (!IL2CPPChainloader.Instance.Plugins.TryGetValue("com.DigiWorm.LevelImposter", out PluginInfo _)) return;
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return;
            if (GameOptionsManager.Instance.CurrentGameOptions.MapId == 6)
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption) {
                    stringOption.ValueText.text = option.selections[option.selection].ToString();
                }
                else if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOptionToo) {
                    stringOptionToo.oldValue = stringOptionToo.Value = option.selection;
                    stringOptionToo.ValueText.text = option.selections[option.selection].ToString();
                }
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
    public class AmongUsClientOnPlayerJoinedPatch
    {
        public static void Postfix(PlayerPhysics __instance) {
            if (PlayerControl.LocalPlayer != null && AmongUsClient.Instance.AmHost) {
                GameManager.Instance.LogicOptions.SyncOptions();
                CustomOption.ShareOptionSelections();
            }

            if (__instance.myPlayer == PlayerInCache.LocalPlayer.PlayerControl && MapOptions.showChatIntro) {
                ChatController chat = HudManager.Instance.Chat;
                chat.AddChat(PlayerInCache.LocalPlayer.PlayerControl, "Welcome to <color=#CC00FFFF>Las Monjas</color>! Thanks for playing!\n\n" +
                    "On Lobby:\n" +
                    "Type <color=#4F7D00FF>/language</color> plus <color=#4F7D00FF>english</color>, <color=#4F7D00FF>spanish</color>, <color=#4F7D00FF>japanese</color> or <color=#4F7D00FF>chinese</color> to change the mod's language.\n" +
                    "Type <color=#4E61FFFF>/help</color> plus a <color=#4E61FFFF>role name</color> to get its summary.\n\n" +
                    "On Meetings:\n" +
                    "Type <color=#00BDFFFF>/myrole</color> to get your role's summary.\n" +
                    "Type <color=#F08048FF>/mymodifier</color> to get your modifier's summary");
            }
        }
    }

    [HarmonyPatch]
    class LegacyGameOptionsPatch
    {
        private static string buildRoleOptions() {
            var impRoles = buildOptionsOfType(CustomOption.CustomOptionType.Impostor, true) + "\n";
            var rebelRoles = buildOptionsOfType(CustomOption.CustomOptionType.Rebel, true) + "\n";
            var neutralRoles = buildOptionsOfType(CustomOption.CustomOptionType.Neutral, true) + "\n";
            var crewRoles = buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, true) + "\n";
            var modifiers = buildOptionsOfType(CustomOption.CustomOptionType.Modifier, true);
            return impRoles + rebelRoles + neutralRoles + crewRoles + modifiers;
        }

        private static string buildOptionsOfType(CustomOption.CustomOptionType type, bool headerOnly) {
            StringBuilder sb = new StringBuilder("\n");
            var options = CustomOption.options.Where(o => o.type == type);
            foreach (var option in options) {
                if (option.parent == null) {
                    string line = $"{option.name}: {option.selections[option.selection].ToString()}";
                    sb.AppendLine(line);
                }
            }
            if (headerOnly) return sb.ToString();
            else sb = new StringBuilder();

            foreach (CustomOption option in options) {
                if (option.parent != null) {
                    bool isIrrelevant = option.parent.getSelection() == 0 || (option.parent.parent != null && option.parent.parent.getSelection() == 0);

                    Color c = isIrrelevant ? Color.grey : Color.white;  // No use for now
                    if (isIrrelevant) continue;
                    sb.AppendLine(Helpers.cs(c, $"{option.name}: {option.selections[option.selection].ToString()}"));
                }
                else {
                    sb.AppendLine($"\n{option.name}: {option.selections[option.selection].ToString()}");
                }
            }
            return sb.ToString();
        }


        public static int maxPage = 8;
        public static string buildAllOptions(string vanillaSettings = "", bool hideExtras = false) {
            if (vanillaSettings == "")
                vanillaSettings = GameOptionsManager.Instance.CurrentGameOptions.ToHudString(PlayerControl.AllPlayerControls.Count);
            int counter = LasMonjasPlugin.optionsPage;
            string hudString = counter != 0 && !hideExtras ? Helpers.cs(DateTime.Now.Second % 2 == 0 ? Color.white : Color.red, "(Use scroll wheel if necessary)\n\n") : "";

            maxPage = 8;
            switch (counter) {
                case 0:
                    hudString += (!hideExtras ? "" : "Page 1: Vanilla Settings \n\n") + vanillaSettings;
                    break;
                case 1:
                    hudString += "Page 2: Las Monjas Settings \n" + buildOptionsOfType(CustomOption.CustomOptionType.General, false);
                    break;
                case 2:
                    hudString += "Page 3: Role and Modifier Rates \n" + buildRoleOptions();
                    break;
                case 3:
                    hudString += "Page 4: Impostor Role Settings \n" + buildOptionsOfType(CustomOption.CustomOptionType.Impostor, false);
                    break;
                case 4:
                    hudString += "Page 5: Rebel Role Settings \n" + buildOptionsOfType(CustomOption.CustomOptionType.Rebel, false);
                    break;
                case 5:
                    hudString += "Page 6: Neutral Role Settings \n" + buildOptionsOfType(CustomOption.CustomOptionType.Neutral, false);
                    break;
                case 6:
                    hudString += "Page 7: Crewmate Role Settings \n" + buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, false);
                    break;
                case 7:
                    hudString += "Page 8: Modifier Settings \n" + buildOptionsOfType(CustomOption.CustomOptionType.Modifier, false);
                    break;
            }


            if (!hideExtras || counter != 0) hudString += $"\n{Language.helpersTexts[5]} ({counter + 1}/{maxPage})";
            return hudString;
        }
    }

    [HarmonyPatch]
    public class AddToKillDistanceSetting
    {
        [HarmonyPatch(typeof(LegacyGameOptions), nameof(LegacyGameOptions.AreInvalid))]
        [HarmonyPrefix]

        public static bool Prefix(LegacyGameOptions __instance, ref int maxExpectedPlayers) {
            //making the killdistances bound check higher since extra short is added
            return __instance.MaxPlayers > maxExpectedPlayers || __instance.NumImpostors < 1
                    || __instance.NumImpostors > 3 || __instance.KillDistance < 0
                    || __instance.KillDistance >= LegacyGameOptions.KillDistances.Count
                    || __instance.PlayerSpeedMod <= 0f || __instance.PlayerSpeedMod > 3f;
        }

        [HarmonyPatch(typeof(NormalGameOptionsV07), nameof(NormalGameOptionsV07.AreInvalid))]
        [HarmonyPrefix]

        public static bool Prefix(NormalGameOptionsV07 __instance, ref int maxExpectedPlayers) {
            return __instance.MaxPlayers > maxExpectedPlayers || __instance.NumImpostors < 1
                    || __instance.NumImpostors > 3 || __instance.KillDistance < 0
                    || __instance.KillDistance >= LegacyGameOptions.KillDistances.Count
                    || __instance.PlayerSpeedMod <= 0f || __instance.PlayerSpeedMod > 3f;
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
        [HarmonyPrefix]

        public static void Prefix(StringOption __instance) {
            //prevents indexoutofrange exception breaking the setting if long happens to be selected
            //when host opens the laptop
            if (__instance.Title == StringNames.GameKillDistance && __instance.Value == 3) {
                __instance.Value = 1;
                GameOptionsManager.Instance.currentNormalGameOptions.KillDistance = 1;
                GameManager.Instance.LogicOptions.SyncOptions();
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
        [HarmonyPostfix]

        public static void Postfix(StringOption __instance) {
            if (__instance.Title == StringNames.GameKillDistance && __instance.Values.Count == 3) {
                __instance.Values = new(
                        new StringNames[] { (StringNames)49999, StringNames.SettingShort, StringNames.SettingMedium, StringNames.SettingLong });
            }
        }

        [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.AppendItem),
            new Type[] { typeof(Il2CppSystem.Text.StringBuilder), typeof(StringNames), typeof(string) })]
        [HarmonyPrefix]

        public static void Prefix(ref StringNames stringName, ref string value) {
            if (stringName == StringNames.GameKillDistance) {
                int index;
                if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                    index = GameOptionsManager.Instance.currentNormalGameOptions.KillDistance;
                }
                else {
                    index = GameOptionsManager.Instance.currentHideNSeekGameOptions.KillDistance;
                }
                value = LegacyGameOptions.KillDistanceStrings[index];
            }
        }

        [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
            new[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
        [HarmonyPriority(Priority.Last)]

        public static bool Prefix(ref string __result, ref StringNames id) {
            if ((int)id == 49999) {
                __result = "Very Short";
                return false;
            }
            return true;
        }

        public static void addKillDistance() {
            LegacyGameOptions.KillDistances = new(new float[] { 0.5f, 1f, 1.8f, 2.5f });
            LegacyGameOptions.KillDistanceStrings = new(new string[] { "Very Short", "Short", "Medium", "Long" });
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance) {
            int page = LasMonjasPlugin.optionsPage;
            if (Input.GetKeyDown(KeyCode.Tab)) {
                LasMonjasPlugin.optionsPage = (LasMonjasPlugin.optionsPage + 1) % 8;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
                LasMonjasPlugin.optionsPage = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
                LasMonjasPlugin.optionsPage = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) {
                LasMonjasPlugin.optionsPage = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) {
                LasMonjasPlugin.optionsPage = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) {
                LasMonjasPlugin.optionsPage = 4;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) {
                LasMonjasPlugin.optionsPage = 5;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) {
                LasMonjasPlugin.optionsPage = 6;
            }
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) {
                LasMonjasPlugin.optionsPage = 7;
            }
            if (LasMonjasPlugin.optionsPage >= LegacyGameOptionsPatch.maxPage) LasMonjasPlugin.optionsPage = 0;
        }
    }


    //This class is taken and adapted from Town of Us Reactivated, https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/CustomOption/Patches.cs, Licensed under GPLv3
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        private static TextMeshPro GameSettings = null;
        public static float
            MinX,/*-5.3F*/
            OriginalY = 2.9F,
            MinY = 2.9F;

        public static Scroller Scroller;
        private static Vector3 LastPosition;
        private static float lastAspect;
        private static bool setLastPosition = false;

        public static void Prefix(HudManager __instance) {
            if (GameSettings?.transform == null) return;

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

            Scroller.gameObject.SetActive(GameSettings.gameObject.activeSelf);

            if (!Scroller.gameObject.active) return;

            var rows = GameSettings.text.Count(c => c == '\n');
            float LobbyTextRowHeight = 0.06F;
            var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

            Scroller.ContentYBounds = new FloatRange(MinY, maxY);

            // Prevent scrolling when the player is interacting with a menu
            if (PlayerInCache.LocalPlayer?.PlayerControl.CanMove != true) {
                GameSettings.transform.localPosition = LastPosition;

                return;
            }

            if (GameSettings.transform.localPosition.x != MinX ||
                GameSettings.transform.localPosition.y < MinY) return;

            LastPosition = GameSettings.transform.localPosition;
        }

        private static void CreateScroller(HudManager __instance) {
            if (Scroller != null) return;

            Transform target = GameSettings.transform;

            Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
            Scroller.transform.SetParent(GameSettings.transform.parent);
            Scroller.gameObject.layer = 5;

            Scroller.transform.localScale = Vector3.one;
            Scroller.allowX = false;
            Scroller.allowY = true;
            Scroller.active = true;
            Scroller.velocity = new Vector2(0, 0);
            Scroller.ScrollbarYBounds = new FloatRange(0, 0);
            Scroller.ContentXBounds = new FloatRange(MinX, MinX);
            Scroller.enabled = true;

            Scroller.Inner = target;
            target.SetParent(Scroller.transform);
        }
    }
}