using HarmonyLib;

namespace LasMonjas.Patches
{
    [HarmonyPatch]
    public static class SpeedPatch
    {
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixPhysics(PlayerPhysics __instance)
        {
            if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && PlayerControl.LocalPlayer == Janitor.janitor && Janitor.dragginBody)
                __instance.body.velocity *= 0.80f; 
            else if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && PlayerControl.LocalPlayer == Modifiers.flash && !Challenger.isDueling)
                __instance.body.velocity *= 1.10f;
            else if (__instance.AmOwner && GameData.Instance && __instance.myPlayer.CanMove && PlayerControl.LocalPlayer == Modifiers.bigchungus && !Challenger.isDueling)
                __instance.body.velocity *= 0.90f;

        }

        [HarmonyPatch(typeof(CustomNetworkTransform), nameof(CustomNetworkTransform.FixedUpdate))]
        [HarmonyPostfix]
        public static void PostfixNetwork(CustomNetworkTransform __instance) {
            if (__instance.AmOwner && __instance.interpolateMovement != 0.0f && PlayerControl.LocalPlayer == Janitor.janitor && Janitor.dragginBody)
                __instance.body.velocity *= 0.80f; 
            else if (__instance.AmOwner && __instance.interpolateMovement != 0.0f && PlayerControl.LocalPlayer == Modifiers.flash && !Challenger.isDueling)
                __instance.body.velocity *= 1.10f;
            else if (__instance.AmOwner && __instance.interpolateMovement != 0.0f && PlayerControl.LocalPlayer == Modifiers.bigchungus && !Challenger.isDueling)
                __instance.body.velocity *= 0.90f;
        }
    }
}