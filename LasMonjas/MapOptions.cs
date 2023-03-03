using System.Collections.Generic;
using UnityEngine;

namespace LasMonjas
{
    static class MapOptions {
        // Set values
        public static bool hidePlayerNames = false;
        public static bool showRoleSummary = true;
        public static bool activateMusic = false;
        public static bool ghostsSeeRoles = true;
        //public static bool horseMode = false;
        public static bool monjaCursor = true;

        // Updating values
        public static List<SurvCamera> camerasToAdd = new List<SurvCamera>();
        public static List<Vent> ventsToSeal = new List<Vent>();
        public static Dictionary<byte, PoolablePlayer> playerIcons = new Dictionary<byte, PoolablePlayer>();
        public static Vector3 positionBeforeDuel = new Vector3();
        public static Vector3 positionBeforeMinigame = new Vector3();

        public static void clearAndReloadMapOptions() {
            camerasToAdd = new List<SurvCamera>();
            ventsToSeal = new List<Vent>();
            playerIcons = new Dictionary<byte, PoolablePlayer>();

            showRoleSummary = LasMonjasPlugin.ShowRoleSummary.Value;
            activateMusic = LasMonjasPlugin.ActivateMusic.Value;
            ghostsSeeRoles = LasMonjasPlugin.GhostsSeeRoles.Value;
            //horseMode = LasMonjasPlugin.HorseMode.Value;
            monjaCursor = LasMonjasPlugin.MonjaCursor.Value;
        }

        public static void checkMusic() {
            activateMusic = LasMonjasPlugin.ActivateMusic.Value;
        }
    }
} 