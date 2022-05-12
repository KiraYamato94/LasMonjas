using System;
using System.Security.Cryptography;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using UnhollowerBaseLib;

namespace LasMonjas.Core {
    [HarmonyPatch]
    public static class ChatCommands {
        public static bool isLover(this PlayerControl player) => !(player == null) && (player == Modifiers.lover1 || player == Modifiers.lover2);      

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class EnableChat
        {
            public static void Postfix(HudManager __instance) {
                if (!__instance.Chat.isActiveAndEnabled && PlayerControl.LocalPlayer.isLover())
                    __instance.Chat.SetVisible(true);
            }
        }

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        public static class AddChat
        {
            public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer) {
                if (__instance != DestroyableSingleton<HudManager>.Instance.Chat)
                    return true;
                PlayerControl localPlayer = PlayerControl.LocalPlayer;
                return localPlayer == null || (MeetingHud.Instance != null || LobbyBehaviour.Instance != null || (localPlayer.Data.IsDead || localPlayer.isLover() || (int)sourcePlayer.PlayerId == (int)PlayerControl.LocalPlayer.PlayerId));

            }
        }
    }
}
