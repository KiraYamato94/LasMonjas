using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AmongUs.GameOptions;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Hazel;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;
using static LasMonjas.Core.CustomOption;

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
            Modifier
        }

        public static List<CustomOption> options = new List<CustomOption>();
        public static int preset = 0;
        public static ConfigEntry<string> vanillaSettings;

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
        public bool invertedParent;

        // Option creation

        public CustomOption(int id, CustomOptionType type, string name, System.Object[] selections, System.Object defaultValue, CustomOption parent, bool isHeader, Action onChange = null, string heading = "", bool invertedParent = false)
        {
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
            this.invertedParent = invertedParent;
            selection = 0;
            if (id != 0)
            {
                entry = LasMonjasPlugin.Instance.Config.Bind($"Preset{preset}", id.ToString(), defaultSelection);
                selection = Mathf.Clamp(entry.Value, 0, selections.Length - 1);
            }
            options.Add(this);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, string[] selections, CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "", bool invertedParent = false)
        {
            return new CustomOption(id, type, name, selections, "", parent, isHeader, onChange, heading, invertedParent);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, float defaultValue, float min, float max, float step, CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "", bool invertedParent = false)
        {
            List<object> selections = new();
            for (float s = min; s <= max; s += step)
                selections.Add(s);
            return new CustomOption(id, type, name, selections.ToArray(), defaultValue, parent, isHeader, onChange, heading, invertedParent);
        }

        public static CustomOption Create(int id, CustomOptionType type, string name, bool defaultValue, CustomOption parent = null, bool isHeader = false, Action onChange = null, string heading = "", bool invertedParent = false)
        {
            return new CustomOption(id, type, name, new string[] { Language.customOptionText[0], Language.customOptionText[1] }, defaultValue ? Language.customOptionText[1] : Language.customOptionText[0], parent, isHeader, onChange, heading, invertedParent);
        }

        // Static behaviour

        public static void switchPreset(int newPreset)
        {
            saveVanillaOptions();
            CustomOption.preset = newPreset;
            vanillaSettings = LasMonjasPlugin.Instance.Config.Bind($"Preset{preset}", "GameOptions", "");
            loadVanillaOptions();
            foreach (CustomOption option in CustomOption.options)
            {
                if (option.id == 0) continue;

                option.entry = LasMonjasPlugin.Instance.Config.Bind($"Preset{preset}", option.id.ToString(), option.defaultSelection);
                option.selection = Mathf.Clamp(option.entry.Value, 0, option.selections.Length - 1);
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
                {
                    stringOption.oldValue = stringOption.Value = option.selection;
                    stringOption.ValueText.text = option.getString();
                }
            }

            // make sure to reload all tabs, even the ones in the background, because they might have changed when the preset was switched!
            if (AmongUsClient.Instance?.AmHost == true)
            {
                foreach (var entry in GameOptionsMenuStartPatch.currentGOMs)
                {
                    CustomOptionType optionType = (CustomOptionType)entry.Key;
                    GameOptionsMenu gom = entry.Value;
                    if (gom != null)
                    {
                        GameOptionsMenuStartPatch.updateGameOptionsMenu(optionType, gom);
                    }
                }
            }
        }

        public static void saveVanillaOptions()
        {
            vanillaSettings.Value = Convert.ToBase64String(GameOptionsManager.Instance.gameOptionsFactory.ToBytes(GameManager.Instance.LogicOptions.currentGameOptions, false));
        }

        public static bool loadVanillaOptions()
        {
            string optionsString = vanillaSettings.Value;
            if (optionsString == "") return false;
            IGameOptions gameOptions = GameOptionsManager.Instance.gameOptionsFactory.FromBytes(Convert.FromBase64String(optionsString));
            if (gameOptions.Version < 8)
            {
                LasMonjasPlugin.Logger.LogMessage("tried to paste old settings, not doing this!");
                return false;
            }
            GameOptionsManager.Instance.GameHostOptions = gameOptions;
            GameOptionsManager.Instance.CurrentGameOptions = GameOptionsManager.Instance.GameHostOptions;
            GameManager.Instance.LogicOptions.SetGameOptions(GameOptionsManager.Instance.CurrentGameOptions);
            GameManager.Instance.LogicOptions.SyncOptions();
            return true;
        }

        public static void ShareOptionChange(uint optionId)
        {
            var option = options.FirstOrDefault(x => x.id == optionId);
            if (option == null) return;
            var writer = AmongUsClient.Instance!.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareOptions, SendOption.Reliable, -1);
            writer.Write((byte)1);
            writer.WritePacked((uint)option.id);
            writer.WritePacked(Convert.ToUInt32(option.selection));
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void ShareOptionSelections()
        {
            if (PlayerControl.AllPlayerControls.Count <= 1 || AmongUsClient.Instance!.AmHost == false && PlayerControl.LocalPlayer == null) return;
            var optionsList = new List<CustomOption>(CustomOption.options);
            while (optionsList.Any())
            {
                byte amount = (byte)Math.Min(optionsList.Count, 200); // takes less than 3 bytes per option on average
                var writer = AmongUsClient.Instance!.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShareOptions, SendOption.Reliable, -1);
                writer.Write(amount);
                for (int i = 0; i < amount; i++)
                {
                    var option = optionsList[0];
                    optionsList.RemoveAt(0);
                    writer.WritePacked((uint)option.id);
                    writer.WritePacked(Convert.ToUInt32(option.selection));
                }
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }

        // Getter

        public int getSelection()
        {
            return selection;
        }

        public bool getBool()
        {
            return selection > 0;
        }

        public float getFloat()
        {
            return (float)selections[selection];
        }

        public int getQuantity()
        {
            return selection + 1;
        }

        public string getString(int newSelection = default)
        {
            if (newSelection != default)
                return selections[newSelection].ToString();
            return selections[selection].ToString();
        }

        public string getName()
        {
            return name;
        }

        public string getHeading()
        {
            if (heading == "") return "";
            return heading;
        }

        public void updateSelection(int newSelection, bool notifyUsers = true)
        {
            newSelection = Mathf.Clamp((newSelection + selections.Length) % selections.Length, 0, selections.Length - 1);
            if (AmongUsClient.Instance?.AmClient == true && notifyUsers && selection != newSelection)
            {
                DestroyableSingleton<HudManager>.Instance.Notifier.AddModSettingsChangeMessage((StringNames)(this.id + 6000), getString(newSelection), getName().Replace("- ", ""));
                try
                {
                    selection = newSelection;
                    if (GameStartManager.Instance != null && GameStartManager.Instance.LobbyInfoPane != null && GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane != null && GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane.gameObject.activeSelf)
                    {
                        LobbyViewSettingsPaneChangeTabPatch.Postfix(GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane, GameStartManager.Instance.LobbyInfoPane.LobbyViewSettingsPane.currentTab);
                    }
                }
                catch { }
            }
            selection = newSelection;
            try
            {
                if (onChange != null) onChange();
            }
            catch { }


            if (optionBehaviour != null && optionBehaviour is StringOption stringOption)
            {
                stringOption.oldValue = stringOption.Value = selection;
                stringOption.ValueText.text = getString();
                if (AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer)
                {
                    if (id == 0 && selection != preset)
                    {
                        switchPreset(selection); // Switch presets
                        ShareOptionSelections();
                    }
                    else if (entry != null)
                    {
                        entry.Value = selection; // Save selection to config
                        ShareOptionChange((uint)id);// Share single selection
                    }
                }
            }
            else if (id == 0 && AmongUsClient.Instance?.AmHost == true && PlayerControl.LocalPlayer)
            {  // Share the preset switch for random maps, even if the menu isnt open!
                switchPreset(selection);
                ShareOptionSelections();// Share all selections
            }

            if (AmongUsClient.Instance?.AmHost == true)
            {
                var currentTab = GameOptionsMenuStartPatch.currentTabs.FirstOrDefault(x => x.active).GetComponent<GameOptionsMenu>();
                if (currentTab != null)
                {
                    var optionType = options.First(x => x.optionBehaviour == currentTab.Children[0]).type;
                    GameOptionsMenuStartPatch.updateGameOptionsMenu(optionType, currentTab);
                }

            }

        }

        public static byte[] serializeOptions()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
                {
                    int lastId = -1;
                    foreach (var option in CustomOption.options.OrderBy(x => x.id))
                    {
                        if (option.id == 0) continue;
                        bool consecutive = lastId + 1 == option.id;
                        lastId = option.id;

                        binaryWriter.Write((byte)(option.selection + (consecutive ? 128 : 0)));
                        if (!consecutive) binaryWriter.Write((ushort)option.id);
                    }
                    binaryWriter.Flush();
                    memoryStream.Position = 0L;
                    return memoryStream.ToArray();
                }
            }
        }

        public static int deserializeOptions(byte[] inputValues)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(inputValues));
            int lastId = -1;
            bool somethingApplied = false;
            int errors = 0;
            while (reader.BaseStream.Position < inputValues.Length)
            {
                try
                {
                    int selection = reader.ReadByte();
                    int id = -1;
                    bool consecutive = selection >= 128;
                    if (consecutive)
                    {
                        selection -= 128;
                        id = lastId + 1;
                    }
                    else
                    {
                        id = reader.ReadUInt16();
                    }
                    if (id == 0) continue;
                    lastId = id;
                    CustomOption option = options.First(option => option.id == id);
                    option.entry = LasMonjasPlugin.Instance.Config.Bind($"Preset{preset}", option.id.ToString(), option.defaultSelection);
                    option.selection = selection;
                    if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
                    {
                        stringOption.oldValue = stringOption.Value = option.selection;
                        stringOption.ValueText.text = option.getString();
                    }
                    somethingApplied = true;
                }
                catch (Exception e)
                {
                    LasMonjasPlugin.Logger.LogWarning($"id:{lastId}:{e}: while deserializing - tried to paste invalid settings!");
                    errors++;
                }
            }
            return Convert.ToInt32(somethingApplied) + (errors > 0 ? 0 : 1);
        }

        // Copy to or paste from clipboard (as string)
        public static void copyToClipboard()
        {
            GUIUtility.systemCopyBuffer = $"{LasMonjasPlugin.VersionString}!{Convert.ToBase64String(serializeOptions())}!{vanillaSettings.Value}";
        }

        public static int pasteFromClipboard()
        {
            string allSettings = GUIUtility.systemCopyBuffer;
            int torOptionsFine = 0;
            bool vanillaOptionsFine = false;
            try
            {
                var settingsSplit = allSettings.Split("!");
                Version versionInfo = Version.Parse(settingsSplit[0]);
                string torSettings = settingsSplit[1];
                string vanillaSettingsSub = settingsSplit[2];
                torOptionsFine = deserializeOptions(Convert.FromBase64String(torSettings));
                ShareOptionSelections();
                if (LasMonjasPlugin.Version > versionInfo && versionInfo < Version.Parse("3.8.8"))
                {
                    vanillaOptionsFine = false;
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, Language.customOptionText[2]);
                }
                else
                {
                    vanillaSettings.Value = vanillaSettingsSub;
                    vanillaOptionsFine = loadVanillaOptions();
                }

                foreach (var option in options)
                {
                    if (option.id != 0 && option.entry != null)
                    {
                        option.entry.Value = option.selection;
                    }
                }

                // make sure to reload all tabs, even the ones in the background, because they might have changed when the preset was switched!
                if (AmongUsClient.Instance?.AmHost == true)
                {
                    foreach (var entry in GameOptionsMenuStartPatch.currentGOMs)
                    {
                        CustomOptionType optionType = (CustomOptionType)entry.Key;
                        GameOptionsMenu gom = entry.Value;
                        if (gom != null)
                        {
                            GameOptionsMenuStartPatch.updateGameOptionsMenu(optionType, gom);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LasMonjasPlugin.Logger.LogWarning($"{e}: tried to paste invalid settings!\n{allSettings}");
                string errorStr = allSettings.Length > 2 ? allSettings.Substring(0, 3) : Language.customOptionText[4];
                FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"{Language.customOptionText[3]} \"{errorStr}...\"");
            }
            return Convert.ToInt32(vanillaOptionsFine) + torOptionsFine;
        }
    }




    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.ChangeTab))]
    class GameOptionsMenuChangeTabPatch
    {
        public static void Postfix(GameSettingMenu __instance, int tabNum, bool previewOnly)
        {
            if (previewOnly) return;
            foreach (var tab in GameOptionsMenuStartPatch.currentTabs)
            {
                if (tab != null)
                    tab.SetActive(false);
            }
            foreach (var pbutton in GameOptionsMenuStartPatch.currentButtons)
            {
                pbutton.SelectButton(false);
            }
            if (tabNum > 2)
            {
                tabNum -= 3;
                GameOptionsMenuStartPatch.currentTabs[tabNum].SetActive(true);
                GameOptionsMenuStartPatch.currentButtons[tabNum].SelectButton(true);
            }
        }
    }

    [HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.SetTab))]
    class LobbyViewSettingsPaneRefreshTabPatch
    {
        public static bool Prefix(LobbyViewSettingsPane __instance)
        {
            if ((int)__instance.currentTab < 15)
            {
                LobbyViewSettingsPaneChangeTabPatch.Postfix(__instance, __instance.currentTab);
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.ChangeTab))]
    class LobbyViewSettingsPaneChangeTabPatch
    {
        public static void Postfix(LobbyViewSettingsPane __instance, StringNames category)
        {
            int tabNum = (int)category;

            foreach (var pbutton in LobbyViewSettingsPatch.currentButtons)
            {
                pbutton.SelectButton(false);
            }
            if (tabNum > 20) // StringNames are in the range of 3000+ 
                return;
            __instance.taskTabButton.SelectButton(false);

            if (tabNum > 2)
            {
                tabNum -= 3;
                LobbyViewSettingsPatch.currentButtons[tabNum].SelectButton(true);
                LobbyViewSettingsPatch.drawTab(__instance, LobbyViewSettingsPatch.currentButtonTypes[tabNum]);
            }
        }
    }

    [HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.Update))]
    class LobbyViewSettingsPaneUpdatePatch
    {
        public static void Postfix(LobbyViewSettingsPane __instance)
        {
            if (LobbyViewSettingsPatch.currentButtons.Count == 0)
            {
                LobbyViewSettingsPatch.Postfix(__instance);

            }
        }
    }


    [HarmonyPatch(typeof(LobbyViewSettingsPane), nameof(LobbyViewSettingsPane.Awake))]
    class LobbyViewSettingsPatch
    {
        public static List<PassiveButton> currentButtons = new();
        public static List<CustomOptionType> currentButtonTypes = new();

        public static void createCustomButton(LobbyViewSettingsPane __instance, int targetMenu, string buttonName, int buttonLanguageId, Vector3 vector3, CustomOptionType optionType)
        {
            buttonName = "View" + buttonName;
            var buttonTemplate = GameObject.Find("OverviewTab");
            var lmjSettingsButton = GameObject.Find(buttonName);
            if (lmjSettingsButton == null)
            {
                lmjSettingsButton = GameObject.Instantiate(buttonTemplate, buttonTemplate.transform.parent);
                lmjSettingsButton.transform.localPosition = vector3;
                lmjSettingsButton.name = buttonName;
                __instance.StartCoroutine(Effects.Lerp(2f, new Action<float>(p => { lmjSettingsButton.transform.FindChild("FontPlacer").GetComponentInChildren<TextMeshPro>().text = Language.customOptionText[buttonLanguageId]; })));
                var torSettingsPassiveButton = lmjSettingsButton.GetComponent<PassiveButton>();
                torSettingsPassiveButton.OnClick.RemoveAllListeners();
                torSettingsPassiveButton.OnClick.AddListener((System.Action)(() => {
                    __instance.ChangeTab((StringNames)targetMenu);
                }));
                torSettingsPassiveButton.OnMouseOut.RemoveAllListeners();
                torSettingsPassiveButton.OnMouseOver.RemoveAllListeners();
                torSettingsPassiveButton.SelectButton(false);
                currentButtons.Add(torSettingsPassiveButton);
                currentButtonTypes.Add(optionType);
            }
        }

        public static void Postfix(LobbyViewSettingsPane __instance)
        {
            currentButtons.ForEach(x => x?.Destroy());
            currentButtons.Clear();
            currentButtonTypes.Clear();

            GameObject.Find("RolesTabs")?.Destroy();
            var overviewTab = GameObject.Find("OverviewTab");
            overviewTab.transform.localScale = new Vector3(0.7f, 1f, 1f);
            overviewTab.transform.localPosition = new Vector3(-5.771f, 1.404f, 0f);
            overviewTab.transform.GetChild(0).GetChild(0).localScale = new Vector3(1.3f, 0.9f, 1f);
            overviewTab.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().alignment = TextAlignmentOptions.Center;
            __instance.transform.GetChild(0).gameObject.SetActive(false);

            createSettingTabs(__instance);

        }

        public static void drawTab(LobbyViewSettingsPane __instance, CustomOptionType optionType)
        {
            var relevantOptions = options.Where(x => x.type == optionType || optionType == CustomOptionType.General).ToList();

            if ((int)optionType == 99)
            {
                // Create 5 Groups with Role settings
                relevantOptions.Clear();
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Impostor && x.isHeader));
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Rebel && x.isHeader));
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Neutral && x.isHeader));
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Crewmate && x.isHeader));
                relevantOptions.AddRange(options.Where(x => x.type == CustomOptionType.Modifier && x.isHeader));
            }

            for (int j = 0; j < __instance.settingsInfo.Count; j++)
            {
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

            foreach (var option in relevantOptions)
            {
                if (option.isHeader && (int)optionType != 99 || (int)optionType == 99 && curType != option.type)
                {
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
                    categoryHeaderMasked.Title.text = option.heading != "" ? option.getHeading() : option.getName();
                    if ((int)optionType == 99)
                        categoryHeaderMasked.Title.text = Language.customOptionText[new Dictionary<CustomOptionType, int>() {
                            { CustomOptionType.Impostor, 7 },
                            { CustomOptionType.Rebel, 8 },
                            { CustomOptionType.Neutral, 9 },
                            { CustomOptionType.Crewmate, 10 },
                            { CustomOptionType.Modifier, 11 } }[curType]];
                    categoryHeaderMasked.transform.SetParent(__instance.settingsContainer);
                    categoryHeaderMasked.transform.localScale = Vector3.one;
                    categoryHeaderMasked.transform.localPosition = new Vector3(-9.77f, num, -2f);
                    __instance.settingsInfo.Add(categoryHeaderMasked.gameObject);
                    num -= 1.05f;
                    i = 0;
                }
                else if (option.parent != null && (option.parent.selection == 0 || option.parent.parent != null && option.parent.parent.selection == 0)) continue;  // Hides options, for which the parent is disabled!

                ViewSettingsInfoPanel viewSettingsInfoPanel = UnityEngine.Object.Instantiate<ViewSettingsInfoPanel>(__instance.infoPanelOrigin);
                viewSettingsInfoPanel.transform.SetParent(__instance.settingsContainer);
                viewSettingsInfoPanel.transform.localScale = Vector3.one;
                float num2;
                if (i % 2 == 0)
                {
                    lines++;
                    num2 = -8.95f;
                    if (i > 0)
                    {
                        num -= 0.85f;
                    }
                }
                else
                {
                    num2 = -3f;
                }
                viewSettingsInfoPanel.transform.localPosition = new Vector3(num2, num, -2f);
                int value = option.getSelection();
                viewSettingsInfoPanel.SetInfo(StringNames.ImpostorsCategory, option.selections[value].ToString(), 61);
                viewSettingsInfoPanel.titleText.text = option.getName();
                if (option.isHeader && (int)optionType != 99 && option.heading == "" && (option.type == CustomOptionType.Neutral || option.type == CustomOptionType.Crewmate || option.type == CustomOptionType.Impostor || option.type == CustomOptionType.Rebel || option.type == CustomOptionType.Modifier))
                {
                    viewSettingsInfoPanel.titleText.text = Language.customOptionText[12];
                }
                __instance.settingsInfo.Add(viewSettingsInfoPanel.gameObject);

                i++;
            }
            float actual_spacing = (headers * 1.05f + lines * 0.85f) / (headers + lines) * 1.01f;
            __instance.scrollBar.CalculateAndSetYBounds((float)(__instance.settingsInfo.Count + singles * 2 + headers), 2f, 5f, actual_spacing);

        }

        public static void createSettingTabs(LobbyViewSettingsPane __instance)
        {
            // Handle different gamemodes and tabs needed therein.
            int next = 3;

            // create LMJ Settings
            createCustomButton(__instance, next++, "lmjSettings", 5, new Vector3(-3.271f, 1.404f, 0f), CustomOptionType.General);
            // create Role Settings
            createCustomButton(__instance, next++, "RoleOverview", 6, new Vector3(-0.771f, 1.404f, 0f), (CustomOptionType)99);
            // Imp
            createCustomButton(__instance, next++, "ImpostorSettings", 7, new Vector3(1.729f, 1.404f, 0f), CustomOptionType.Impostor);
            // Rebel
            createCustomButton(__instance, next++, "RebelSettings", 8, new Vector3(4.229f, 1.404f, 0f), CustomOptionType.Rebel);
            // Neutral
            createCustomButton(__instance, next++, "NeutralSettings", 9, new Vector3(-3.271f, 2.304f, 0f), CustomOptionType.Neutral);
            // Crew
            createCustomButton(__instance, next++, "CrewmateSettings", 10, new Vector3(-0.771f, 2.304f, 0f), CustomOptionType.Crewmate);
            // Modifier
            createCustomButton(__instance, next++, "ModifierSettings", 11, new Vector3(1.729f, 2.304f, 0f), CustomOptionType.Modifier);
        }
    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.CreateSettings))]
    class GameOptionsMenuCreateSettingsPatch
    {
        public static void Postfix(GameOptionsMenu __instance)
        {
            if (__instance.gameObject.name == "GAME SETTINGS TAB")
                adaptTaskCount(__instance);
        }

        private static void adaptTaskCount(GameOptionsMenu __instance)
        {
            // Adapt task count for main options
            var commonTasksOption = __instance.Children.ToArray().FirstOrDefault(x => x.TryCast<NumberOption>()?.intOptionName == Int32OptionNames.NumCommonTasks).Cast<NumberOption>();
            if (commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 4f);
            var shortTasksOption = __instance.Children.ToArray().FirstOrDefault(x => x.TryCast<NumberOption>()?.intOptionName == Int32OptionNames.NumShortTasks).TryCast<NumberOption>();
            if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);
            var longTasksOption = __instance.Children.ToArray().FirstOrDefault(x => x.TryCast<NumberOption>()?.intOptionName == Int32OptionNames.NumLongTasks).TryCast<NumberOption>();
            if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
        }
    }


    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
    class GameOptionsMenuStartPatch
    {
        public static List<GameObject> currentTabs = new();
        public static List<PassiveButton> currentButtons = new();
        public static Dictionary<byte, GameOptionsMenu> currentGOMs = new();
        public static void Postfix(GameSettingMenu __instance)
        {
            currentTabs.ForEach(x => x?.Destroy());
            currentButtons.ForEach(x => x?.Destroy());
            currentTabs = new();
            currentButtons = new();
            currentGOMs.Clear();

            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;

            removeVanillaTabs(__instance);

            createSettingTabs(__instance);

            var GOMGameObject = GameObject.Find("GAME SETTINGS TAB");


            // create copy to clipboard and paste from clipboard buttons.
            var template = GameObject.Find("PlayerOptionsMenu(Clone)").transform.Find("CloseButton").gameObject;
            var holderGO = new GameObject("copyPasteButtonParent");
            var bgrenderer = holderGO.AddComponent<SpriteRenderer>();
            bgrenderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.CopyPasteBG.png", 175f);
            holderGO.transform.SetParent(template.transform.parent, false);
            holderGO.transform.localPosition = template.transform.localPosition + new Vector3(-8.3f, 0.73f, -2f);
            holderGO.layer = template.layer;
            holderGO.SetActive(true);
            var copyButton = GameObject.Instantiate(template, holderGO.transform);
            copyButton.transform.localPosition = new Vector3(-0.3f, 0.02f, -2f);
            var copyButtonPassive = copyButton.GetComponent<PassiveButton>();
            var copyButtonRenderer = copyButton.GetComponentInChildren<SpriteRenderer>();
            var copyButtonActiveRenderer = copyButton.transform.GetChild(1).GetComponent<SpriteRenderer>();
            copyButtonRenderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Copy.png", 100f);
            copyButton.transform.GetChild(1).transform.localPosition = Vector3.zero;
            copyButtonActiveRenderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.CopyActive.png", 100f);
            copyButtonPassive.OnClick.RemoveAllListeners();
            copyButtonPassive.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            copyButtonPassive.OnClick.AddListener((System.Action)(() => {
                copyToClipboard();
                copyButtonRenderer.color = Color.green;
                copyButtonActiveRenderer.color = Color.green;
                __instance.StartCoroutine(Effects.Lerp(1f, new System.Action<float>((p) => {
                    if (p > 0.95)
                    {
                        copyButtonRenderer.color = Color.white;
                        copyButtonActiveRenderer.color = Color.white;
                    }
                })));
            }));
            var pasteButton = GameObject.Instantiate(template, holderGO.transform);
            pasteButton.transform.localPosition = new Vector3(0.3f, 0.02f, -2f);
            var pasteButtonPassive = pasteButton.GetComponent<PassiveButton>();
            var pasteButtonRenderer = pasteButton.GetComponentInChildren<SpriteRenderer>();
            var pasteButtonActiveRenderer = pasteButton.transform.GetChild(1).GetComponent<SpriteRenderer>();
            pasteButtonRenderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Paste.png", 100f);
            pasteButtonActiveRenderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PasteActive.png", 100f);
            pasteButtonPassive.OnClick.RemoveAllListeners();
            pasteButtonPassive.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            pasteButtonPassive.OnClick.AddListener((System.Action)(() => {
                pasteButtonRenderer.color = Color.yellow;
                int success = pasteFromClipboard();
                pasteButtonRenderer.color = success == 3 ? Color.green : success == 0 ? Color.red : Color.yellow;
                pasteButtonActiveRenderer.color = success == 3 ? Color.green : success == 0 ? Color.red : Color.yellow;
                __instance.StartCoroutine(Effects.Lerp(1f, new System.Action<float>((p) => {
                    if (p > 0.95)
                    {
                        pasteButtonRenderer.color = Color.white;
                        pasteButtonActiveRenderer.color = Color.white;
                    }
                })));
            }));
        }

        private static void createSettings(GameOptionsMenu menu, List<CustomOption> options)
        {
            float num = 1.5f;
            foreach (CustomOption option in options)
            {
                if (option.isHeader)
                {
                    CategoryHeaderMasked categoryHeaderMasked = UnityEngine.Object.Instantiate<CategoryHeaderMasked>(menu.categoryHeaderOrigin, Vector3.zero, Quaternion.identity, menu.settingsContainer);
                    categoryHeaderMasked.SetHeader(StringNames.ImpostorsCategory, 20);
                    categoryHeaderMasked.Title.text = option.heading != "" ? option.getHeading() : option.getName();
                    categoryHeaderMasked.transform.localScale = Vector3.one * 0.63f;
                    categoryHeaderMasked.transform.localPosition = new Vector3(-0.903f, num, -2f);
                    num -= 0.63f;
                }
                else if (option.parent != null && (option.parent.selection == 0 && !option.invertedParent || option.parent.parent != null && option.parent.parent.selection == 0 && !option.parent.invertedParent)) continue;  // Hides options, for which the parent is disabled!
                else if (option.parent != null && option.parent.selection != 0 && option.invertedParent) continue;
                OptionBehaviour optionBehaviour = UnityEngine.Object.Instantiate<StringOption>(menu.stringOptionOrigin, Vector3.zero, Quaternion.identity, menu.settingsContainer);
                optionBehaviour.transform.localPosition = new Vector3(0.952f, num, -2f);
                optionBehaviour.SetClickMask(menu.ButtonClickMask);

                // "SetUpFromData"
                SpriteRenderer[] componentsInChildren = optionBehaviour.GetComponentsInChildren<SpriteRenderer>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, 20);
                }
                foreach (TextMeshPro textMeshPro in optionBehaviour.GetComponentsInChildren<TextMeshPro>(true))
                {
                    textMeshPro.fontMaterial.SetFloat("_StencilComp", 3f);
                    textMeshPro.fontMaterial.SetFloat("_Stencil", (float)20);
                }

                var stringOption = optionBehaviour as StringOption;
                stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                stringOption.TitleText.text = option.getName();
                if (option.isHeader && option.heading == "" && (option.type == CustomOptionType.Neutral || option.type == CustomOptionType.Crewmate || option.type == CustomOptionType.Impostor || option.type == CustomOptionType.Rebel || option.type == CustomOptionType.Modifier))
                {
                    stringOption.TitleText.text = Language.customOptionText[12];
                }
                if (stringOption.TitleText.text.Length > 25)
                    stringOption.TitleText.fontSize = 2.2f;
                if (stringOption.TitleText.text.Length > 40)
                    stringOption.TitleText.fontSize = 2f;
                stringOption.Value = stringOption.oldValue = option.selection;
                stringOption.ValueText.text = option.getString();
                option.optionBehaviour = stringOption;

                menu.Children.Add(optionBehaviour);
                num -= 0.45f;
                menu.scrollBar.SetYBoundsMax(-num - 1.65f);
            }

            for (int i = 0; i < menu.Children.Count; i++)
            {
                OptionBehaviour optionBehaviour = menu.Children[i];
                if (AmongUsClient.Instance && !AmongUsClient.Instance.AmHost)
                {
                    optionBehaviour.SetAsPlayer();
                }
            }
        }

        private static void removeVanillaTabs(GameSettingMenu __instance)
        {
            GameObject.Find("What Is This?")?.Destroy();
            GameObject.Find("GamePresetButton")?.Destroy();
            GameObject.Find("RoleSettingsButton")?.Destroy();
            __instance.ChangeTab(1, false);
        }

        public static void createCustomButton(GameSettingMenu __instance, int targetMenu, string buttonName, int buttonLanguageId)
        {
            var leftPanel = GameObject.Find("LeftPanel");
            var buttonTemplate = GameObject.Find("GameSettingsButton");
            if (targetMenu == 3)
            {
                buttonTemplate.transform.localPosition -= Vector3.up * 0.85f;
                buttonTemplate.transform.localScale *= Vector2.one * 0.75f;
            }
            var lmjSettingsButton = GameObject.Find(buttonName);
            if (lmjSettingsButton == null)
            {
                lmjSettingsButton = GameObject.Instantiate(buttonTemplate, leftPanel.transform);
                lmjSettingsButton.transform.localPosition += Vector3.up * 0.5f * (targetMenu - 2);
                lmjSettingsButton.name = buttonName;
                __instance.StartCoroutine(Effects.Lerp(2f, new Action<float>(p => { lmjSettingsButton.transform.FindChild("FontPlacer").GetComponentInChildren<TextMeshPro>().text = Language.customOptionText[buttonLanguageId]; })));
                var torSettingsPassiveButton = lmjSettingsButton.GetComponent<PassiveButton>();
                torSettingsPassiveButton.OnClick.RemoveAllListeners();
                torSettingsPassiveButton.OnClick.AddListener((System.Action)(() => {
                    __instance.ChangeTab(targetMenu, false);
                }));
                torSettingsPassiveButton.OnMouseOut.RemoveAllListeners();
                torSettingsPassiveButton.OnMouseOver.RemoveAllListeners();
                torSettingsPassiveButton.SelectButton(false);
                currentButtons.Add(torSettingsPassiveButton);
            }
        }

        public static void createGameOptionsMenu(GameSettingMenu __instance, CustomOptionType optionType, string settingName)
        {
            var tabTemplate = GameObject.Find("GAME SETTINGS TAB");
            currentTabs.RemoveAll(x => x == null);

            var lmjSettingsTab = GameObject.Instantiate(tabTemplate, tabTemplate.transform.parent);
            lmjSettingsTab.name = settingName;

            var torSettingsGOM = lmjSettingsTab.GetComponent<GameOptionsMenu>();

            updateGameOptionsMenu(optionType, torSettingsGOM);

            currentTabs.Add(lmjSettingsTab);
            lmjSettingsTab.SetActive(false);
            currentGOMs.Add((byte)optionType, torSettingsGOM);
        }

        public static void updateGameOptionsMenu(CustomOptionType optionType, GameOptionsMenu torSettingsGOM)
        {
            foreach (var child in torSettingsGOM.Children)
            {
                child.Destroy();
            }
            torSettingsGOM.scrollBar.transform.FindChild("SliderInner").DestroyChildren();
            torSettingsGOM.Children.Clear();
            var relevantOptions = options.Where(x => x.type == optionType).ToList();
            createSettings(torSettingsGOM, relevantOptions);
        }

        private static void createSettingTabs(GameSettingMenu __instance)
        {
            int next = 3;

            // create LMJ Settings
            createCustomButton(__instance, next++, "lmjSettings", 5);
            createGameOptionsMenu(__instance, CustomOptionType.General, "lmjSettings");

            // Imp
            createCustomButton(__instance, next++, "ImpostorSettings", 7);
            createGameOptionsMenu(__instance, CustomOptionType.Impostor, "ImpostorSettings");

            // Rebel
            createCustomButton(__instance, next++, "RebelSettings", 8);
            createGameOptionsMenu(__instance, CustomOptionType.Rebel, "RebelSettings");

            // Neutral
            createCustomButton(__instance, next++, "NeutralSettings", 9);
            createGameOptionsMenu(__instance, CustomOptionType.Neutral, "NeutralSettings");

            // Crew
            createCustomButton(__instance, next++, "CrewmateSettings", 10);
            createGameOptionsMenu(__instance, CustomOptionType.Crewmate, "CrewmateSettings");

            // Modifier
            createCustomButton(__instance, next++, "ModifierSettings", 11);
            createGameOptionsMenu(__instance, CustomOptionType.Modifier, "ModifierSettings");
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
            __instance.Value = __instance.oldValue = option.selection;
            __instance.ValueText.text = option.getString();

            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection + 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection - 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.FixedUpdate))]
    public class StringOptionFixedUpdate
    {
        public static void Postfix(StringOption __instance)
        {
            if (!IL2CPPChainloader.Instance.Plugins.TryGetValue("com.DigiWorm.LevelImposter", out PluginInfo _)) return;
            CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return;
            if (GameOptionsManager.Instance.CurrentGameOptions.MapId == 6)
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
                {
                    stringOption.ValueText.text = option.getString();
                }
                else if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOptionToo)
                {
                    stringOptionToo.oldValue = stringOptionToo.Value = option.selection;
                    stringOptionToo.ValueText.text = option.getString();
                }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix()
        {
            saveVanillaOptions();
        }
    }

    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoSpawnPlayer))]
    public class AmongUsClientOnPlayerJoinedPatch
    {
        public static void Postfix(PlayerPhysics __instance)
        {
            if (PlayerControl.LocalPlayer != null && AmongUsClient.Instance.AmHost)
            {
                GameManager.Instance.LogicOptions.SyncOptions();
                ShareOptionSelections();
            }

            if (__instance.myPlayer == PlayerInCache.LocalPlayer.PlayerControl && MapOptions.showChatIntro)
            {
                ChatController chat = HudManager.Instance.Chat;
                chat.AddChat(PlayerInCache.LocalPlayer.PlayerControl, Language.customOptionText[13]);
            }
        }
    }


    [HarmonyPatch]
    class LegacyGameOptionsPatch
    {
        private static string buildRoleOptions()
        {
            var impRoles = buildOptionsOfType(CustomOption.CustomOptionType.Impostor, true) + "\n";
            var rebelRoles = buildOptionsOfType(CustomOption.CustomOptionType.Rebel, true) + "\n";
            var neutralRoles = buildOptionsOfType(CustomOption.CustomOptionType.Neutral, true) + "\n";
            var crewRoles = buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, true) + "\n";
            var modifiers = buildOptionsOfType(CustomOption.CustomOptionType.Modifier, true);
            return impRoles + rebelRoles + neutralRoles + crewRoles + modifiers;
        }

        private static string buildOptionsOfType(CustomOption.CustomOptionType type, bool headerOnly)
        {
            StringBuilder sb = new StringBuilder("\n");
            var options = CustomOption.options.Where(o => o.type == type);
            foreach (var option in options)
            {
                if (option.parent == null)
                {
                    string line = $"{option.getName()}: {option.getString()}";
                    sb.AppendLine(line);
                }
            }
            if (headerOnly) return sb.ToString();
            else sb = new StringBuilder();

            foreach (CustomOption option in options)
            {
                if (option.parent != null)
                {
                    bool isIrrelevant = option.parent.getSelection() == 0 || (option.parent.parent != null && option.parent.parent.getSelection() == 0);

                    Color c = isIrrelevant ? Color.grey : Color.white;  // No use for now
                    if (isIrrelevant) continue;
                    sb.AppendLine(Helpers.cs(c, $"{option.getName()}: {option.getString()}"));
                }
                else
                {
                    sb.AppendLine($"\n{option.getName()}: {option.getString()}");
                }
            }
            return sb.ToString();
        }

        public static int maxPage = 8;
        public static string buildAllOptions(string vanillaSettings = "", bool hideExtras = false)
        {
            if (vanillaSettings == "")
                vanillaSettings = GameOptionsManager.Instance.CurrentGameOptions.ToHudString(PlayerControl.AllPlayerControls.Count);
            int counter = LasMonjasPlugin.optionsPage;
            string hudString = counter != 0 && !hideExtras ? Helpers.cs(DateTime.Now.Second % 2 == 0 ? Color.white : Color.red, "(Use scroll wheel if necessary)\n\n") : "";

            switch (counter)
            {
                case 0:
                    hudString += (!hideExtras ? "" : $"{Language.customOptionText[14]} \n\n") + vanillaSettings;
                    break;
                case 1:
                    hudString += $"{Language.customOptionText[15]} \n" + buildOptionsOfType(CustomOption.CustomOptionType.General, false);
                    break;
                case 2:
                    hudString += $"{Language.customOptionText[16]} \n" + buildRoleOptions();
                    break;
                case 3:
                    hudString += $"{Language.customOptionText[17]} \n" + buildOptionsOfType(CustomOption.CustomOptionType.Impostor, false);
                    break;
                case 4:
                    hudString += $"{Language.customOptionText[18]} \n" + buildOptionsOfType(CustomOption.CustomOptionType.Rebel, false);
                    break;
                case 5:
                    hudString += $"{Language.customOptionText[19]} \n" + buildOptionsOfType(CustomOption.CustomOptionType.Neutral, false);
                    break;
                case 6:
                    hudString += $"{Language.customOptionText[20]} \n" + buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, false);
                    break;
                case 7:
                    hudString += $"{Language.customOptionText[21]} \n" + buildOptionsOfType(CustomOption.CustomOptionType.Modifier, false);
                    break;
            }

            if (!hideExtras || counter != 0) hudString += $"\n {Language.customOptionText[22]} ({counter + 1}/{maxPage})";
            return hudString;
        }


        [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
        private static void Postfix(ref string __result)
        {
            if (GameOptionsManager.Instance.currentGameOptions.GameMode == AmongUs.GameOptions.GameModes.HideNSeek) return; // Allow Vanilla Hide N Seek
            __result = buildAllOptions(vanillaSettings: __result);
        }
    }

    [HarmonyPatch]
    public class AddToKillDistanceSetting
    {
        [HarmonyPatch(typeof(LegacyGameOptions), nameof(LegacyGameOptions.AreInvalid))]
        [HarmonyPrefix]

        public static bool Prefix(LegacyGameOptions __instance, ref int maxExpectedPlayers)
        {
            //making the killdistances bound check higher since extra short is added
            return __instance.MaxPlayers > maxExpectedPlayers || __instance.NumImpostors < 1
                    || __instance.NumImpostors > 3 || __instance.KillDistance < 0
                    || __instance.KillDistance >= LegacyGameOptions.KillDistances.Count
                    || __instance.PlayerSpeedMod <= 0f || __instance.PlayerSpeedMod > 3f;
        }

        [HarmonyPatch(typeof(NormalGameOptionsV07), nameof(NormalGameOptionsV07.AreInvalid))]
        [HarmonyPrefix]

        public static bool Prefix(NormalGameOptionsV07 __instance, ref int maxExpectedPlayers)
        {
            return __instance.MaxPlayers > maxExpectedPlayers || __instance.NumImpostors < 1
                    || __instance.NumImpostors > 3 || __instance.KillDistance < 0
                    || __instance.KillDistance >= LegacyGameOptions.KillDistances.Count
                    || __instance.PlayerSpeedMod <= 0f || __instance.PlayerSpeedMod > 3f;
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
        [HarmonyPrefix]

        public static void Prefix(StringOption __instance)
        {
            //prevents indexoutofrange exception breaking the setting if long happens to be selected
            //when host opens the laptop
            if (__instance.Title == StringNames.GameKillDistance && __instance.Value == 3)
            {
                __instance.Value = 1;
                GameOptionsManager.Instance.currentNormalGameOptions.KillDistance = 1;
                GameManager.Instance.LogicOptions.SyncOptions();
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Initialize))]
        [HarmonyPostfix]

        public static void Postfix(StringOption __instance)
        {
            if (__instance.Title == StringNames.GameKillDistance && __instance.Values.Count == 3)
            {
                __instance.Values = new(
                        new StringNames[] { (StringNames)49999, StringNames.SettingShort, StringNames.SettingMedium, StringNames.SettingLong });
            }
        }

        [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.AppendItem),
            new Type[] { typeof(Il2CppSystem.Text.StringBuilder), typeof(StringNames), typeof(string) })]
        [HarmonyPrefix]

        public static void Prefix(ref StringNames stringName, ref string value)
        {
            if (stringName == StringNames.GameKillDistance)
            {
                int index;
                if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal)
                {
                    index = GameOptionsManager.Instance.currentNormalGameOptions.KillDistance;
                }
                else
                {
                    index = GameOptionsManager.Instance.currentHideNSeekGameOptions.KillDistance;
                }
                value = LegacyGameOptions.KillDistanceStrings[index];
            }
        }

        [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
            new[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
        [HarmonyPriority(Priority.Last)]

        public static bool Prefix(ref string __result, ref StringNames id)
        {
            if ((int)id == 49999)
            {
                __result = Language.customOptionText[23];
                return false;
            }
            return true;
        }

        public static void addKillDistance()
        {
            LegacyGameOptions.KillDistances = new(new float[] { 0.5f, 1f, 1.8f, 2.5f });
            LegacyGameOptions.KillDistanceStrings = new(new string[] { Language.customOptionText[23], Language.customOptionText[24], Language.customOptionText[25], Language.customOptionText[26] });
        }

        [HarmonyPatch(typeof(StringGameSetting), nameof(StringGameSetting.GetValueString))]
        [HarmonyPrefix]
        public static bool AjdustStringForViewPanel(StringGameSetting __instance, float value, ref string __result)
        {
            if (__instance.OptionName != Int32OptionNames.KillDistance) return true;
            __result = LegacyGameOptions.KillDistanceStrings[(int)value];
            return false;
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance)
        {
            int page = LasMonjasPlugin.optionsPage;
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                LasMonjasPlugin.optionsPage = (LasMonjasPlugin.optionsPage + 1) % 7;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                LasMonjasPlugin.optionsPage = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                LasMonjasPlugin.optionsPage = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                LasMonjasPlugin.optionsPage = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                LasMonjasPlugin.optionsPage = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                LasMonjasPlugin.optionsPage = 4;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                LasMonjasPlugin.optionsPage = 5;
            }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            {
                LasMonjasPlugin.optionsPage = 6;
            }
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                LasMonjasPlugin.optionsPage = 7;
            }
            if (Input.GetKeyDown(KeyCode.F1))
                HudManagerUpdate.ToggleSettings(HudManager.Instance);
            if (Input.GetKeyDown(KeyCode.F2) && LobbyBehaviour.Instance)
                HudManagerUpdate.ToggleSummary(HudManager.Instance);
            if (LasMonjasPlugin.optionsPage >= LegacyGameOptionsPatch.maxPage) LasMonjasPlugin.optionsPage = 0;
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    public class HudManagerUpdate
    {
        [HarmonyPrefix]
        public static void Prefix2(HudManager __instance)
        {
            if (!settingsTMPs[0]) return;
            foreach (var tmp in settingsTMPs) tmp.text = "";
            var settingsString = LegacyGameOptionsPatch.buildAllOptions(hideExtras: true);
            var blocks = settingsString.Split("\n\n", StringSplitOptions.RemoveEmptyEntries); ;
            string curString = "";
            string curBlock;
            int j = 0;
            for (int i = 0; i < blocks.Length; i++)
            {
                curBlock = blocks[i];
                if (Helpers.lineCount(curBlock) + Helpers.lineCount(curString) < 43)
                {
                    curString += curBlock + "\n\n";
                }
                else
                {
                    settingsTMPs[j].text = curString;
                    j++;

                    curString = "\n" + curBlock + "\n\n";
                    if (curString.Substring(0, 2) != "\n\n") curString = "\n" + curString;
                }
            }
            if (j < settingsTMPs.Length) settingsTMPs[j].text = curString;
            int blockCount = 0;
            foreach (var tmp in settingsTMPs)
            {
                if (tmp.text != "")
                    blockCount++;
            }
            for (int i = 0; i < blockCount; i++)
            {
                settingsTMPs[i].transform.localPosition = new Vector3(-blockCount * 1.2f + 2.7f * i, 2.2f, -500f);
            }
        }

        private static TMPro.TextMeshPro[] settingsTMPs = new TMPro.TextMeshPro[4];
        private static GameObject settingsBackground;
        public static void OpenSettings(HudManager __instance)
        {
            if (__instance.FullScreen == null || MapBehaviour.Instance && MapBehaviour.Instance.IsOpen) return;
            if (summaryTMP)
            {
                CloseSummary();
            }
            settingsBackground = GameObject.Instantiate(__instance.FullScreen.gameObject, __instance.transform);
            settingsBackground.SetActive(true);
            var renderer = settingsBackground.GetComponent<SpriteRenderer>();
            renderer.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            renderer.enabled = true;

            for (int i = 0; i < settingsTMPs.Length; i++)
            {
                settingsTMPs[i] = GameObject.Instantiate(__instance.KillButton.cooldownTimerText, __instance.transform);
                settingsTMPs[i].alignment = TMPro.TextAlignmentOptions.TopLeft;
                settingsTMPs[i].enableWordWrapping = false;
                settingsTMPs[i].transform.localScale = Vector3.one * 0.25f;
                settingsTMPs[i].gameObject.SetActive(true);
            }
        }

        public static void CloseSettings()
        {
            foreach (var tmp in settingsTMPs)
                if (tmp) tmp.gameObject.Destroy();

            if (settingsBackground) settingsBackground.Destroy();
        }

        public static void ToggleSettings(HudManager __instance)
        {
            if (settingsTMPs[0]) CloseSettings();
            else OpenSettings(__instance);
        }

        [HarmonyPrefix]
        public static void Prefix3(HudManager __instance)
        {
            if (!summaryTMP) return;
            summaryTMP.text = Helpers.previousEndGameSummary;

            summaryTMP.transform.localPosition = new Vector3(-3 * 1.2f, 2.2f, -500f);

        }

        private static TMPro.TextMeshPro summaryTMP = null;
        private static GameObject summaryBackground;
        public static void OpenSummary(HudManager __instance)
        {
            if (__instance.FullScreen == null || MapBehaviour.Instance && MapBehaviour.Instance.IsOpen || Helpers.previousEndGameSummary.IsNullOrWhiteSpace()) return;
            if (settingsTMPs[0])
            {
                CloseSettings();
            }
            summaryBackground = GameObject.Instantiate(__instance.FullScreen.gameObject, __instance.transform);
            summaryBackground.SetActive(true);
            var renderer = summaryBackground.GetComponent<SpriteRenderer>();
            renderer.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            renderer.enabled = true;


            summaryTMP = GameObject.Instantiate(__instance.KillButton.cooldownTimerText, __instance.transform);
            summaryTMP.alignment = TMPro.TextAlignmentOptions.TopLeft;
            summaryTMP.enableWordWrapping = false;
            summaryTMP.transform.localScale = Vector3.one * 0.3f;
            summaryTMP.gameObject.SetActive(true);

        }

        public static void CloseSummary()
        {
            summaryTMP?.gameObject.Destroy();
            summaryTMP = null;
            if (summaryBackground) summaryBackground.Destroy();
        }

        public static void ToggleSummary(HudManager __instance)
        {
            if (summaryTMP) CloseSummary();
            else OpenSummary(__instance);
        }

        static PassiveButton toggleSettingsButton;
        static GameObject toggleSettingsButtonObject;

        static PassiveButton toggleSummaryButton;
        static GameObject toggleSummaryButtonObject;

        [HarmonyPostfix]
        public static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            if (!toggleSettingsButton || !toggleSettingsButtonObject)
            {
                // add a special button for settings viewing:
                toggleSettingsButtonObject = GameObject.Instantiate(__instance.MapButton.gameObject, __instance.MapButton.transform.parent);
                toggleSettingsButtonObject.transform.localPosition = __instance.MapButton.transform.localPosition + new Vector3(0, -1.25f, -500f);
                toggleSettingsButtonObject.name = "TOGGLESETTINGSBUTTON";
                SpriteRenderer renderer = toggleSettingsButtonObject.transform.Find("Inactive").GetComponent<SpriteRenderer>();
                SpriteRenderer rendererActive = toggleSettingsButtonObject.transform.Find("Active").GetComponent<SpriteRenderer>();
                toggleSettingsButtonObject.transform.Find("Background").localPosition = Vector3.zero;
                renderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Settings_Button.png", 100f);
                rendererActive.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Settings_ButtonActive.png", 100);
                toggleSettingsButton = toggleSettingsButtonObject.GetComponent<PassiveButton>();
                toggleSettingsButton.OnClick.RemoveAllListeners();
                toggleSettingsButton.OnClick.AddListener((Action)(() => ToggleSettings(__instance)));
            }
            toggleSettingsButtonObject.SetActive(__instance.MapButton.gameObject.active && !(MapBehaviour.Instance && MapBehaviour.Instance.IsOpen) && GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.HideNSeek);
            toggleSettingsButtonObject.transform.localPosition = __instance.MapButton.transform.localPosition + new Vector3(0, -0.8f, -500f);
        }

        [HarmonyPostfix]
        public static void Postfix2(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
            {
                if (toggleSummaryButtonObject != null)
                {
                    toggleSummaryButtonObject.SetActive(false);
                    toggleSummaryButtonObject.Destroy();
                    toggleSummaryButton.Destroy();
                }
                return;
            }
            if (!toggleSummaryButton || !toggleSummaryButtonObject)
            {
                // add a special button for settings viewing:
                toggleSummaryButtonObject = GameObject.Instantiate(__instance.MapButton.gameObject, __instance.MapButton.transform.parent);
                toggleSummaryButtonObject.transform.localPosition = __instance.MapButton.transform.localPosition + new Vector3(0, -1.25f, -500f);
                toggleSummaryButtonObject.name = "TOGGLESUMMARYSBUTTON";
                SpriteRenderer renderer = toggleSummaryButtonObject.transform.Find("Inactive").GetComponent<SpriteRenderer>();
                SpriteRenderer rendererActive = toggleSummaryButtonObject.transform.Find("Active").GetComponent<SpriteRenderer>();
                toggleSummaryButtonObject.transform.Find("Background").localPosition = Vector3.zero;
                renderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Endscreen.png", 100f);
                rendererActive.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.EndscreenActive.png", 100f);
                toggleSummaryButton = toggleSummaryButtonObject.GetComponent<PassiveButton>();
                toggleSummaryButton.OnClick.RemoveAllListeners();
                toggleSummaryButton.OnClick.AddListener((Action)(() => ToggleSummary(__instance)));
            }
            toggleSummaryButtonObject.SetActive(__instance.SettingsButton.gameObject.active && LobbyBehaviour.Instance && !Helpers.previousEndGameSummary.IsNullOrWhiteSpace() && GameOptionsManager.Instance.currentGameOptions.GameMode != GameModes.HideNSeek
                && AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started);
            toggleSummaryButtonObject.transform.localPosition = __instance.SettingsButton.transform.localPosition + new Vector3(-1.45f, 0.03f, -500f);
        }
    }
}
