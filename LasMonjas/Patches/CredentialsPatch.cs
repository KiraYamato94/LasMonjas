using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LasMonjas.Patches {
    [HarmonyPatch]
    public static class CredentialsPatch {

        public static string mainMenuCredentials = $@"By <color=#CC00FFFF>Allul</color>";

        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        private static class VersionShowerPatch
        {
            static void Postfix(VersionShower __instance) {
                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                if (amongUsLogo == null) return;

                var credentials = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(__instance.text);
                credentials.transform.position = new Vector3(0, 0, 0);
                credentials.SetText($"v{LasMonjasPlugin.Version.ToString()}\n<size=30f%>\n</size>{mainMenuCredentials}\n<size=30%>\n</size>");
                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.75f;

                credentials.transform.SetParent(amongUsLogo.transform);
            }
        }

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        private static class PingTrackerPatch
        {

            static void Postfix(PingTracker __instance) {

                __instance.text.text += "\n<color=#CC00FFFF>Las Monjas v2.0.1</color>";
                __instance.transform.localPosition = new Vector3(1.25f, 3f, __instance.transform.localPosition.z);
            }
        }     

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        private static class LogoPatch
        {
            static void Postfix(PingTracker __instance) {
                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                if (amongUsLogo != null) {
                    amongUsLogo.transform.localScale *= 0.6f;
                    amongUsLogo.transform.position += Vector3.up * 0.25f;
                }

                var lasMonjasLogo = new GameObject("bannerLogo_LasMonjas");
                lasMonjasLogo.transform.position = Vector3.up;
                var renderer = lasMonjasLogo.AddComponent<SpriteRenderer>();
                renderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Banner.png", 300f);                                
            }
        }
    }
}
