using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using static LasMonjas.LasMonjas;

namespace LasMonjas
{
    static class MapOptions {
        // Set values
        public static bool hidePlayerNames = false;
        public static bool showRoleSummary = true;
        public static bool activateMusic = false;
        public static bool ghostsSeeRoles = true;
        public static bool horseMode = false;

        // Updating values
        public static List<SurvCamera> camerasToAdd = new List<SurvCamera>();
        public static List<Vent> ventsToSeal = new List<Vent>();
        public static Dictionary<byte, PoolablePlayer> playerIcons = new Dictionary<byte, PoolablePlayer>();
        public static Vector3 positionBeforeDuel = new Vector3();

        public static void clearAndReloadMapOptions() {
            camerasToAdd = new List<SurvCamera>();
            ventsToSeal = new List<Vent>();
            playerIcons = new Dictionary<byte, PoolablePlayer>(); ;

            showRoleSummary = LasMonjasPlugin.ShowRoleSummary.Value;
            activateMusic = LasMonjasPlugin.ActivateMusic.Value;
            ghostsSeeRoles = LasMonjasPlugin.GhostsSeeRoles.Value;
            horseMode = LasMonjasPlugin.HorseMode.Value;
        }

        public static void checkMusic() {
            activateMusic = LasMonjasPlugin.ActivateMusic.Value;
        }
    }
} 