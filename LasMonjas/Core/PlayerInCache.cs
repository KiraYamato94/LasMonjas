using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace LasMonjas.Core;

// This class is taken from The Other Roles,https://github.com/TheOtherRolesAU/TheOtherRoles/blob/main/TheOtherRoles/Players/CachedPlayer.cs Licensed under GPLv3

public class PlayerInCache
{
    public static readonly Dictionary<IntPtr, PlayerInCache> PlayerPtrs = new();
    public static readonly List<PlayerInCache> AllPlayers = new();
    public static PlayerInCache LocalPlayer;

    public Transform transform;
    public PlayerControl PlayerControl;
    public PlayerPhysics PlayerPhysics;
    public CustomNetworkTransform NetTransform;
    public NetworkedPlayerInfo Data;
    public byte PlayerId;
    
    public static implicit operator bool(PlayerInCache player)
    {
        return player != null && player.PlayerControl;
    }
    
    public static implicit operator PlayerControl(PlayerInCache player) => player.PlayerControl;
    public static implicit operator PlayerPhysics(PlayerInCache player) => player.PlayerPhysics;

}

[HarmonyPatch]
public static class CachedPlayerPatches
{
    [HarmonyPatch]
    private class CacheLocalPlayerPatch
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var type = typeof(PlayerControl).GetNestedTypes(AccessTools.all).FirstOrDefault(t => t.Name.Contains("Start"));
            return AccessTools.Method(type, nameof(IEnumerator.MoveNext));
        }

        [HarmonyPostfix]
        public static void SetLocalPlayer()
        {
            var localPlayer = PlayerControl.LocalPlayer;
            if (!localPlayer )
            {
                PlayerInCache.LocalPlayer = null;
                return;
            }
            
            var cached = PlayerInCache.AllPlayers.FirstOrDefault(p => p.PlayerControl.Pointer == localPlayer.Pointer);
            if (cached != null)
            {
                PlayerInCache.LocalPlayer = cached;
                return;
            }
        }
    }
    
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Awake))]
    [HarmonyPostfix]
    public static void CachePlayerPatch(PlayerControl __instance)
    {
        if (__instance.notRealPlayer) return;
        var player = new PlayerInCache
        {
            transform = __instance.transform,
            PlayerControl = __instance,
            PlayerPhysics = __instance.MyPhysics,
            NetTransform = __instance.NetTransform
        };
        PlayerInCache.AllPlayers.Add(player);
        PlayerInCache.PlayerPtrs[__instance.Pointer] = player;
        
#if DEBUG
        foreach (var cachedPlayer in PlayerInCache.AllPlayers)
        {
            if (!cachedPlayer.PlayerControl || !cachedPlayer.PlayerPhysics || !cachedPlayer.NetTransform || !cachedPlayer.transform)
            {
                LasMonjasPlugin.Logger.LogError($"PlayerInCache {cachedPlayer.PlayerControl.name} has null fields");
            }
        }
#endif
    }
    
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnDestroy))]
    [HarmonyPostfix]
    public static void RemoveCachedPlayerPatch(PlayerControl __instance)
    {
        if (__instance.notRealPlayer) return;
        PlayerInCache.AllPlayers.RemoveAll(p => p.PlayerControl.Pointer == __instance.Pointer);
        PlayerInCache.PlayerPtrs.Remove(__instance.Pointer);
    }

    [HarmonyPatch(typeof(NetworkedPlayerInfo), nameof(NetworkedPlayerInfo.Deserialize))]
    [HarmonyPostfix]
    public static void AddCachedDataOnDeserialize()
    {
        foreach (PlayerInCache cachedPlayer in PlayerInCache.AllPlayers)
        {
            cachedPlayer.Data = cachedPlayer.PlayerControl.Data;
            cachedPlayer.PlayerId = cachedPlayer.PlayerControl.PlayerId;
        }
    }
    
    [HarmonyPatch(typeof(GameData), nameof(GameData.AddPlayer))]
    [HarmonyPostfix]
    public static void AddCachedDataOnAddPlayer()
    {
        foreach (PlayerInCache cachedPlayer in PlayerInCache.AllPlayers)
        {
            cachedPlayer.Data = cachedPlayer.PlayerControl.Data;
            cachedPlayer.PlayerId = cachedPlayer.PlayerControl.PlayerId;
        }
    }
    
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Deserialize))]
    [HarmonyPostfix]
    public static void SetCachedPlayerId(PlayerControl __instance)
    {
        PlayerInCache.PlayerPtrs[__instance.Pointer].PlayerId = __instance.PlayerId;
    }
}