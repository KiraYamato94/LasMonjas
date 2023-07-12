using AmongUs.GameOptions;
using HarmonyLib;
using static LasMonjas.LasMonjas;

namespace LasMonjas.Patches
{
    [HarmonyPatch]
    public static class SpeedPatch
    {
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixPhysics(PlayerPhysics __instance) {
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) {
                if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && Janitor.janitor != null && __instance.myPlayer.PlayerId == Janitor.janitor.PlayerId && Janitor.dragginBody)
                    __instance.body.velocity *= 0.80f;
                else if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && TaskMaster.taskMaster != null && __instance.myPlayer.PlayerId == TaskMaster.taskMaster.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !LasMonjas.isHappeningAnonymousComms && TaskMaster.taskTimer > 0)
                    __instance.body.velocity *= 1.20f;
                else if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && Monja.monja != null && __instance.myPlayer.PlayerId == Monja.monja.PlayerId && Monja.awakened)
                    __instance.body.velocity *= 1.30f;
                else if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && Modifiers.flash != null && __instance.myPlayer.PlayerId == Modifiers.flash.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !LasMonjas.isHappeningAnonymousComms)
                    __instance.body.velocity *= 1.10f;
                else if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && Modifiers.bigchungus != null && __instance.myPlayer.PlayerId == Modifiers.bigchungus.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !LasMonjas.isHappeningAnonymousComms)
                    __instance.body.velocity *= 0.90f;
                else if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && Modifiers.pro != null && __instance.myPlayer.PlayerId == Modifiers.pro.PlayerId)
                    __instance.body.velocity *= -1;
                else if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && ZombieLaboratory.nursePlayer != null && __instance.myPlayer.PlayerId == ZombieLaboratory.nursePlayer.PlayerId && gameType == 6 && ZombieLaboratory.currentKeyItems >= 3)
                    __instance.body.velocity *= 1.10f;
            }
        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixNetwork(CustomNetworkTransform __instance) {
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal && AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) {

                if (!__instance.AmOwner && __instance.interpolateMovement != 0.0f && Janitor.janitor != null && __instance.gameObject.GetComponent<PlayerControl>().PlayerId == Janitor.janitor.PlayerId && Janitor.dragginBody)
                    __instance.body.velocity *= 0.80f;
                else if (!__instance.AmOwner && __instance.interpolateMovement != 0.0f && TaskMaster.taskMaster != null && __instance.gameObject.GetComponent<PlayerControl>().PlayerId == TaskMaster.taskMaster.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !LasMonjas.isHappeningAnonymousComms && TaskMaster.taskTimer > 0)
                    __instance.body.velocity *= 1.20f;
                else if (!__instance.AmOwner && __instance.interpolateMovement != 0.0f && Monja.monja != null && __instance.gameObject.GetComponent<PlayerControl>().PlayerId == Monja.monja.PlayerId && Monja.awakened)
                    __instance.body.velocity *= 1.30f;
                else if (!__instance.AmOwner && __instance.interpolateMovement != 0.0f && Modifiers.flash != null && __instance.gameObject.GetComponent<PlayerControl>().PlayerId == Modifiers.flash.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !LasMonjas.isHappeningAnonymousComms)
                    __instance.body.velocity *= 1.10f;
                else if (!__instance.AmOwner && __instance.interpolateMovement != 0.0f && Modifiers.bigchungus != null && __instance.gameObject.GetComponent<PlayerControl>().PlayerId == Modifiers.bigchungus.PlayerId && !Challenger.isDueling && !Seeker.isMinigaming && !LasMonjas.isHappeningAnonymousComms)
                    __instance.body.velocity *= 0.90f;
                else if (!__instance.AmOwner && __instance.interpolateMovement != 0.0f && ZombieLaboratory.nursePlayer != null && __instance.gameObject.GetComponent<PlayerControl>().PlayerId == ZombieLaboratory.nursePlayer.PlayerId && gameType == 6 && ZombieLaboratory.currentKeyItems >= 3)
                    __instance.body.velocity *= 1.10f;
            }
        }
    }
}