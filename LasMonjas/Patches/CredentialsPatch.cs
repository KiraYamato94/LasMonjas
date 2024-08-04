using HarmonyLib;
using TMPro;
using UnityEngine;

namespace LasMonjas.Patches {
    [HarmonyPatch]
    [HarmonyPriority(Priority.First)]

    public static class CredentialsPatch {

        public static string mainMenuCredentials = $@"By <color=#CC00FFFF>Allul</color>";

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        [HarmonyPriority(Priority.First)]

        // IF YOU FORK THIS MOD, PLEASE DON'T REMOVE THIS

        private static class PingTrackerPatch
        {

            static void Postfix(PingTracker __instance) {

                __instance.text.alignment = TextAlignmentOptions.Top;
                var position = __instance.GetComponent<AspectPosition>();
                position.Alignment = AspectPosition.EdgeAlignments.Top; 
                __instance.text.text += "\n<color=#CC00FFFF>Las Monjas "+ LasMonjasPlugin.Version.ToString() + "</color>";
                position.DistanceFromEdge = new Vector3(0f, 0.1f, 0);
                __instance.transform.localPosition = new Vector3(1.25f, 3f, __instance.transform.localPosition.z);
            }
        }

        //

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        [HarmonyPriority(Priority.First)]

        private static class LogoPatch
        {
            static void Postfix(PingTracker __instance) {
                
                var lasMonjasLogo = new GameObject("bannerLogo_LasMonjas");
                lasMonjasLogo.transform.position = new Vector3(0.8f, 0.8f, 1f);
                var renderer = lasMonjasLogo.AddComponent<SpriteRenderer>();
                renderer.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Banner.png", 300f);
                var position = lasMonjasLogo.AddComponent<AspectPosition>();
                position.DistanceFromEdge = new Vector3(-0.2f, 2f, 8f);
                position.Alignment = AspectPosition.EdgeAlignments.Top;

                position.StartCoroutine(Effects.Lerp(0.1f, new System.Action<float>((p) =>
                {
                    position.AdjustPosition();
                })));


                var scaler = lasMonjasLogo.AddComponent<AspectScaledAsset>();
                var renderers = new Il2CppSystem.Collections.Generic.List<SpriteRenderer>();
                renderers.Add(renderer);

                scaler.spritesToScale = renderers;
                scaler.aspectPosition = position;

                lasMonjasLogo.transform.SetParent(GameObject.Find("RightPanel").transform);

                var credentials = new GameObject("LMJCredentials");
                credentials.AddComponent<TextMeshPro>();
                var textCredentials = credentials.GetComponent<TextMeshPro>();
                credentials.transform.position = new Vector3(-0.3f, -0.25f, 5);
                textCredentials.SetText($"v{LasMonjasPlugin.Version.ToString()}\n<size=30f%>\n</size>{mainMenuCredentials}\n<size=30%>\n</size>");
                textCredentials.alignment = TMPro.TextAlignmentOptions.Center;
                textCredentials.fontSize *= 0.075f;

                credentials.transform.SetParent(lasMonjasLogo.transform);
            }
        }
    }
}