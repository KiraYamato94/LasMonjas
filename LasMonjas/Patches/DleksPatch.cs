using AmongUs.GameOptions;
using HarmonyLib;
using Rewired.Utils.Platforms.Windows;
using System;
using System.Linq;
using UnityEngine;

namespace LasMonjas.Patches;

// This class is taken from TOHE, https://github.com/0xDrMoe/TownofHost-Enhanced/blob/main/Patches/DleksPatch.cs, Licensed under GPLv3
[HarmonyPatch(typeof(AmongUsClient._CoStartGameHost_d__32), nameof(AmongUsClient._CoStartGameHost_d__32.MoveNext))]
public static class DleksPatch
{
    /*private static bool Prefix(AmongUsClient._CoStartGameHost_d__32 __instance, ref bool __result)
    {
        if (__instance.__1__state != 0)
        {
            return true;
        }

        __instance.__1__state = -1;
        if (LobbyBehaviour.Instance)
        {
            LobbyBehaviour.Instance.Despawn();
        }

        if (ShipStatus.Instance)
        {
            __instance.__2__current = null;
            __instance.__1__state = 2;
            __result = true;
            return false;
        }

        // removed dleks check as it's always false
        var num2 = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.MapId, 0, Constants.MapNames.Length - 1);
        __instance.__2__current = __instance.__4__this.ShipLoadingAsyncHandle = __instance.__4__this.ShipPrefabs[num2].InstantiateAsync();
        __instance.__1__state = 1;

        __result = true;
        return false;
    }
}
[HarmonyPatch(typeof(GameStartManager))]
class AllMapIconsPatch
{
    // Vanilla players getting error when trying get dleks map icon
    [HarmonyPatch(nameof(GameStartManager.Start)), HarmonyPostfix]
    public static void Postfix_AllMapIcons(GameStartManager __instance)
    {
        if (__instance == null) return;

        if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal && GameOptionsManager.Instance.currentNormalGameOptions.MapId == 3)
        {
            GameOptionsManager.Instance.currentNormalGameOptions.MapId = 0;
            __instance.UpdateMapImage(MapNames.Skeld);
            CreateOptionsPickerPatch.SetDleks = true;
        }
        else if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek && GameOptionsManager.Instance.currentHideNSeekGameOptions.MapId == 3)
        {
            GameOptionsManager.Instance.currentHideNSeekGameOptions.MapId = 0;
            __instance.UpdateMapImage(MapNames.Skeld);
            CreateOptionsPickerPatch.SetDleks = true;
        }

        MapIconByName DleksIncon = UnityEngine.Object.Instantiate(__instance, __instance.gameObject.transform).AllMapIcons[0];
        DleksIncon.Name = MapNames.Dleks;
        DleksIncon.MapImage = Helpers.loadSpriteFromResources($"LasMonjas.Images.Dleks_Map_Banner.png", 100f);
        DleksIncon.NameImage = Helpers.loadSpriteFromResources($"LasMonjas.Images.Dleks_Text.png", 100f);

        __instance.AllMapIcons.Add(DleksIncon);
    }
}
[HarmonyPatch(typeof(StringOption), nameof(StringOption.Start))]
class AutoSelectDleksPatch
{
    private static void Postfix(StringOption __instance)
    {
        if (__instance.Title == StringNames.GameMapName)
        {
            // vanilla clamps this to not auto select dleks
            __instance.Value = GameOptionsManager.Instance.CurrentGameOptions.MapId;
        }
    }
}
[HarmonyPatch(typeof(Vent), nameof(Vent.SetButtons))]
public static class VentSetButtonsPatch
{
    public static bool ShowButtons = false;
    // Fix arrows buttons in vent on Dleks map and "Index was outside the bounds of the array" errors
    private static bool Prefix(Vent __instance, [HarmonyArgument(0)] ref bool enabled)
    {
        // if map is Dleks
        if (GameOptionsManager.Instance.CurrentGameOptions.MapId == 3)
        {
            enabled = false;
            if (MeetingHud.Instance) 
                ShowButtons = false;
        }
        return true;
    }
    public static void Postfix(Vent __instance, [HarmonyArgument(0)] bool enabled)
    {
        if (GameOptionsManager.Instance.CurrentGameOptions.MapId != 3) return;
        if (enabled) return;

        var setActive = ShowButtons || !PlayerControl.LocalPlayer.inVent && !MeetingHud.Instance;
        switch (__instance.Id)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 5:
            case 6:
                __instance.Buttons[0].gameObject.SetActive(setActive);
                __instance.Buttons[1].gameObject.SetActive(setActive);
                break;
            case 7:
            case 12:
            case 13:
                __instance.Buttons[0].gameObject.SetActive(setActive);
                break;
            case 4:
            case 8:
            case 9:
            case 10:
            case 11:
                __instance.Buttons[1].gameObject.SetActive(setActive);
                break;
        }
    }
}
[HarmonyPatch(typeof(Vent), nameof(Vent.TryMoveToVent))]
class VentTryMoveToVentPatch
{
    // Update arrows buttons when player move to vents
    private static void Postfix(Vent __instance, [HarmonyArgument(0)] Vent otherVent)
    {
        if (__instance == null || otherVent == null || GameOptionsManager.Instance.CurrentGameOptions.MapId != 3) return;

        VentSetButtonsPatch.ShowButtons = true;
        VentSetButtonsPatch.Postfix(otherVent, false);
        VentSetButtonsPatch.ShowButtons = false;
    }
}
[HarmonyPatch(typeof(Vent), nameof(Vent.UpdateArrows))]
class VentUpdateArrowsPatch
{
    // Fixes "Index was outside the bounds of the array" errors when arrows updates in vent on Dleks map
    private static bool Prefix()
    {
        // if map is not Dleks
        return GameOptionsManager.Instance.CurrentGameOptions.MapId != 3;
    }*/
}