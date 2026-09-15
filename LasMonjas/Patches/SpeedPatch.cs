using AmongUs.GameOptions;
using HarmonyLib;
using static LasMonjas.LasMonjas;

namespace LasMonjas.Patches
{
    [HarmonyPatch]
    public static class SpeedPatch
    {

        private static float GetSpeedMultiplier(PlayerControl player) {
            if (Janitor.janitor != null && player.PlayerId == Janitor.janitor.PlayerId && Janitor.dragginBody) return 0.80f;
            if (TaskMaster.taskMaster != null && player.PlayerId == TaskMaster.taskMaster.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !isHappeningAnonymousComms && TaskMaster.taskTimer > 0) return 1.20f;
            if (Monja.monja != null && player.PlayerId == Monja.monja.PlayerId && Monja.awakened) return 1.30f;
            if (Modifiers.flash != null && player.PlayerId == Modifiers.flash.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !isHappeningAnonymousComms) return 1.10f;
            if (Modifiers.bigchungus != null && player.PlayerId == Modifiers.bigchungus.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !isHappeningAnonymousComms) return 0.90f;
            if (ZombieLaboratory.nursePlayer != null && player.PlayerId == ZombieLaboratory.nursePlayer.PlayerId && gameType == 6 && ZombieLaboratory.currentKeyItems >= 3) return 1.15f;

            return 1f;
        }

        private static bool isProModifier(PlayerControl player) {
            return Modifiers.pro != null && player.PlayerId == Modifiers.pro.PlayerId;
        }

        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixPhysics(PlayerPhysics __instance) {
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) {

                if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove) {
                    float multiplier = GetSpeedMultiplier(__instance.myPlayer);
                    if (multiplier != 1f) {
                        __instance.body.velocity *= multiplier;
                    }
                    else if (isProModifier(__instance.myPlayer)) {
                        __instance.body.velocity *= -1;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixNetwork(CustomNetworkTransform __instance) {
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) {

                if (!__instance.AmOwner) {
                    PlayerControl player = __instance.gameObject.GetComponent<PlayerControl>();

                    if (player == null) return;

                    float multiplier = GetSpeedMultiplier(player);
                    if (multiplier != 1f) {
                        __instance.body.velocity *= multiplier;
                    }
                }
            }
        }
    }
}