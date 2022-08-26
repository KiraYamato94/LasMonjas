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
using UnhollowerBaseLib;
using UnityEngine;
using LasMonjas.Core;
using LasMonjas.Patches;
using Reactor.Extensions;

namespace LasMonjas
{
    [BepInPlugin(Id, "Las Monjas", VersionString)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("Among Us.exe")]
    public class LasMonjasPlugin : BasePlugin
    {
        public const string Id = "me.allul.lasmonjas";

        public const string VersionString = "2.1.3";

        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;

        public Harmony Harmony { get; } = new Harmony(Id);
        public static LasMonjasPlugin Instance;

        public static int optionsPage = 1;

        public static ConfigEntry<bool> ShowRoleSummary { get; set; }
        public static ConfigEntry<bool> ActivateMusic { get; set; }
        public static ConfigEntry<bool> GhostsSeeRoles { get; set; }
        public static ConfigEntry<bool> HorseMode { get; set; }
        public static ConfigEntry<bool> MonjaCursor { get; set; }
        public static ConfigEntry<string> IpCustom { get; set; }
        public static ConfigEntry<ushort> PortCustom { get; set; }


        public static IRegionInfo[] defaultRegions;
        public static void UpdateRegions() {
            ServerManager serverManager = DestroyableSingleton<ServerManager>.Instance;
            IRegionInfo[] regions = defaultRegions;

            var CustomRegionCustom = new DnsRegionInfo(IpCustom.Value, "Custom", StringNames.NoTranslation, IpCustom.Value, PortCustom.Value, false);
            regions = regions.Concat(new IRegionInfo[] { CustomRegionCustom.Cast<IRegionInfo>() }).ToArray();
            ServerManager.DefaultRegions = regions;
            serverManager.AvailableRegions = regions;
        }

        public override void Load() {
            Logger = Log;
            AssetLoader.LoadAssets();
          
            ShowRoleSummary = Config.Bind("Custom", "Show Role Summary", true);
            ActivateMusic = Config.Bind("Custom", "Activate Music", true);
            GhostsSeeRoles = Config.Bind("Custom", "Ghosts See Roles", true);
            HorseMode = Config.Bind("Custom", "Horse Mode", false);
            MonjaCursor = Config.Bind("Custom", "Monja Cursor", true);

            IpCustom = Config.Bind("Custom", "Custom Server IP", "127.0.0.1");
            PortCustom = Config.Bind("Custom", "Custom Server Port", (ushort)22023);

            defaultRegions = ServerManager.DefaultRegions;

            UpdateRegions();

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
                SaveManager.chatModeType = 1;
                SaveManager.isGuest = false;
            }
        }
    }        
}
