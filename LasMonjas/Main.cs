using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Net;
using System.IO;
using System;
using System.Reflection;
using Il2CppInterop;
using UnityEngine;
using LasMonjas.Core;
using LasMonjas.Patches;
using BepInEx.Unity.IL2CPP;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using AmongUs.Data;
using AmongUs.Data.Legacy;
using AmongUs.GameOptions;

namespace LasMonjas
{
    [BepInPlugin(Id, "Las Monjas", VersionString)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("Among Us.exe")]
    [ReactorModFlags(ModFlags.RequireOnAllClients)]
    public class LasMonjasPlugin : BasePlugin
    {
        public const string Id = "me.allul.lasmonjas";

        public const string VersionString = "3.1.0";

        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;

        public Harmony Harmony { get; } = new Harmony(Id);
        public static LasMonjasPlugin Instance;

        public static int optionsPage = 1;

        public static ConfigEntry<bool> ShowRoleSummary { get; set; }
        public static ConfigEntry<bool> ActivateMusic { get; set; }
        public static ConfigEntry<bool> GhostsSeeRoles { get; set; }
        //public static ConfigEntry<bool> HorseMode { get; set; }
        public static ConfigEntry<bool> MonjaCursor { get; set; }
        public static ConfigEntry<int> modLanguage { get; set; }

        public override void Load() {
            Logger = Log;
            AssetLoader.LoadAssets();
            ShowRoleSummary = Config.Bind("Custom", "Show Role Summary", true);
            ActivateMusic = Config.Bind("Custom", "Activate Music", true);
            GhostsSeeRoles = Config.Bind("Custom", "Ghosts See Roles", true);
            //HorseMode = Config.Bind("Custom", "Horse Mode", false);
            MonjaCursor = Config.Bind("Custom", "Monja Cursor", true);

            modLanguage = Config.Bind("Custom", "Mod Language", 1);

            GameOptionsData.RecommendedImpostors = GameOptionsData.MaxImpostors = Enumerable.Repeat(3, 16).ToArray(); // Max Imp = Recommended Imp = 3
            GameOptionsData.MinPlayers = Enumerable.Repeat(4, 15).ToArray(); // Min Players = 4

            Instance = this;
            CustomOptionHolder.Load();
            CustomColors.Load();

            Harmony.PatchAll();
            SubmergedCompatibility.Initialize();
            AddComponent<ModUpdateBehaviour>();
            if (MonjaCursor.Value) {
                Helpers.enableCursor("start");
            }
            Language.LoadLanguage();
        }
    }

    // Deactivate bans, since I always leave my local testing game and ban myself
    [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.AmBanned), MethodType.Getter)]
    public static class AmBannedPatch
    {
        public static void Postfix(out bool __result)
        {
            __result = false;
        }
    }
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    public static class ChatControllerAwakePatch {
        private static void Prefix() {
            if (!EOSManager.Instance.isKWSMinor) {
                DataManager.Settings.Multiplayer.ChatMode = InnerNet.QuickChatModes.FreeChatOrQuickChat;
            }
        }
    }
}
