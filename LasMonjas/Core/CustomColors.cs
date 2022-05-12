using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Il2CppSystem;
using HarmonyLib;
using UnhollowerBaseLib;
using Assets.CoreScripts;

namespace LasMonjas.Core {
    public class CustomColors {
        protected static Dictionary<int, string> ColorStrings = new Dictionary<int, string>();
        public static List<int> lighterColors = new List<int>(){ 3, 4, 5, 7, 10, 11, 13, 14, 17 };
        public static uint pickableColors = (uint)Palette.ColorNames.Length;

        public static void Load() {
            List<StringNames> longlist = Enumerable.ToList<StringNames>(Palette.ColorNames);
            List<Color32> colorlist = Enumerable.ToList<Color32>(Palette.PlayerColors);
            List<Color32> shadowlist = Enumerable.ToList<Color32>(Palette.ShadowColors);

            List<CustomColor> colors = new List<CustomColor>();

            colors.Add(new CustomColor { longname = "Lavender",
                                        color = new Color32(207, 161, 241, byte.MaxValue),
                                        shadow = new Color32(165, 93, 243, byte.MaxValue),
                                        isLighterColor = true });
            colors.Add(new CustomColor { longname = "Petrol", 
                                        color = new Color32(0, 99, 105, byte.MaxValue),
                                        shadow = new Color32(0, 61, 54, byte.MaxValue),
                                        isLighterColor = false });
            colors.Add(new CustomColor { longname = "Mint",
                                         color = new Color32(168, 235, 195, byte.MaxValue),
                                         shadow = new Color32(123, 156, 143, byte.MaxValue),
                                         isLighterColor = true });
            colors.Add(new CustomColor { longname = "Olive",
                                         color = new Color32(97, 114, 24, byte.MaxValue),
                                         shadow = new Color32(66, 91, 15, byte.MaxValue),
                                         isLighterColor = false }); 

            pickableColors += (uint)colors.Count;
                    
            int id = 50000;
            foreach (CustomColor cc in colors) {
                longlist.Add((StringNames)id);
                CustomColors.ColorStrings[id++] = cc.longname;
                colorlist.Add(cc.color);
                shadowlist.Add(cc.shadow);
                if (cc.isLighterColor)
                    lighterColors.Add(colorlist.Count - 1);
            }

            Palette.ColorNames = longlist.ToArray();
            Palette.PlayerColors = colorlist.ToArray();
            Palette.ShadowColors = shadowlist.ToArray();
        }

        protected internal struct CustomColor {
            public string longname;
            public Color32 color;
            public Color32 shadow;
            public bool isLighterColor;
        }

        [HarmonyPatch]
        public static class CustomColorPatches {
            [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), new[] {
                typeof(StringNames),
                typeof(Il2CppReferenceArray<Il2CppSystem.Object>)
            })]
            private class ColorStringPatch {
                public static bool Prefix(ref string __result, [HarmonyArgument(0)] StringNames name) {
                    if ((int)name >= 50000) {
                        string text = CustomColors.ColorStrings[(int)name];
                        if (text != null) {
                            __result = text;
                            return false;
                        }
                    }
                    return true;
                }
            }

            [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.LoadPlayerPrefs))]
            private static class LoadPlayerPrefsPatch {
                private static bool needsPatch = false;
                public static void Prefix([HarmonyArgument(0)] bool overrideLoad) {
                    if (!SaveManager.loaded || overrideLoad)
                        needsPatch = true;
                }
                public static void Postfix() {
                    if (!needsPatch) return;
                    SaveManager.colorConfig %= CustomColors.pickableColors;
                    needsPatch = false;
                }
            }

            [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckColor))]
            private static class PlayerControlCheckColorPatch {
                private static bool isTaken(PlayerControl player, uint color) {
                    foreach (GameData.PlayerInfo p in GameData.Instance.AllPlayers)
                        if (!p.Disconnected && p.PlayerId != player.PlayerId && p.DefaultOutfit.ColorId == color)
                            return true;
                    return false;
                }
                public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] byte bodyColor) {
                    uint color = (uint)bodyColor;
                   if (isTaken(__instance, color) || color >= Palette.PlayerColors.Length) {
                        int num = 0;
                        while (num++ < 50 && (color >= CustomColors.pickableColors || isTaken(__instance, color))) {
                            color = (color + 1) % CustomColors.pickableColors;
                        }
                    }
                    __instance.RpcSetColor((byte)color);
                    return false;
                }
            }
        }
    }
}