using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.AddressableAssets;

namespace LasMonjas.Core
{
    class CustomVisors : VisorData
    {
        public static Material MagicShader = new Material(Shader.Find("Unlit/PlayerShader"));
        public struct AuthorData
        {
            public string AuthorName;
            public string VisorName;
            public bool altShader;
        }

        public static List<AuthorData> authorDatas = new List<AuthorData>()
        {
            new AuthorData {AuthorName = "Sensei", VisorName = "Alien"},
            new AuthorData {AuthorName = "Sensei", VisorName = "Fortune Teller"},
            new AuthorData {AuthorName = "Sensei", VisorName = "Over 9 Sus", altShader = true},
            new AuthorData {AuthorName = "Sensei", VisorName = "PC Error"},
            new AuthorData {AuthorName = "Sensei", VisorName = "The Eye"},
            new AuthorData {AuthorName = "Sensei", VisorName = "Visor Cleaner"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Muaresito Joy"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Olmaito"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Impostor Bros"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Juice"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Josefa Shoe"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Menacing"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Susonal"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Loading"},
            new AuthorData {AuthorName = "Xago", VisorName = "Zargothrax"},
            new AuthorData {AuthorName = "IceCreamGuy", VisorName = "Mungus"},
            new AuthorData {AuthorName = "ERIKHAPPY", VisorName = "Bubble Gum"},
            new AuthorData {AuthorName = "lotty", VisorName = "Flower", altShader = true},
            new AuthorData {AuthorName = "lotty", VisorName = "Disco Ball"},
            new AuthorData {AuthorName = "lotty", VisorName = "Eye see you"},
            new AuthorData {AuthorName = "lotty", VisorName = "Not Sus"},
            new AuthorData {AuthorName = "lotty", VisorName = "Shopping"},
            new AuthorData {AuthorName = "lotty", VisorName = "Inu"},
            new AuthorData {AuthorName = "lotty", VisorName = "Butterfly"},
            new AuthorData {AuthorName = "lotty", VisorName = "Confetti"},
            new AuthorData {AuthorName = "lotty", VisorName = "Kek Smile"},
            new AuthorData {AuthorName = "lotty", VisorName = "Golf Club"},
            new AuthorData {AuthorName = "Xeno<33", VisorName = "Wand"},
            new AuthorData {AuthorName = "Nyxx", VisorName = "Sunglasses"},
        };

        public static bool _customVisorLoaded = false;

        static readonly List<VisorData> visorData = new();

        public static readonly List<CustomVisors> customVisorData = new();

        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetVisorById))]
        class AddCustomVisors
        {
            public static void Postfix(HatManager __instance) {
                if (_customVisorLoaded) return;
                _customVisorLoaded = true;
                var AllVisors = __instance.allVisors.ToList();

                foreach (var data in authorDatas) {
                    VisorViewData vvd = new VisorViewData();
                    vvd.IdleFrame = GetSprite(data.VisorName);
                    if (data.altShader) {
                        vvd.AltShader = MagicShader;
                    }
                    var plate = new CustomVisors(vvd);
                    plate.name = $"{data.VisorName} (by {data.AuthorName})";
                    plate.ProductId = "lmj_" + plate.name.Replace(' ', '_');
                    plate.BundleId = "lmj_" + plate.name.Replace(' ', '_');
                    plate.displayOrder = 99;
                    plate.ChipOffset = new Vector2(0f, 0.2f);
                    plate.Free = true;
                    plate.SpritePreview = vvd.IdleFrame;
                    visorData.Add(plate);
                    customVisorData.Add(plate);
                    var assetRef = new AssetReference(vvd.Pointer);
                    plate.ViewDataRef = assetRef;
                    plate.CreateAddressableAsset();
                }
                AllVisors.AddRange(visorData);
                __instance.allVisors = AllVisors.ToArray();                
            }
        }

        [HarmonyPatch(typeof(VisorsTab), nameof(VisorsTab.OnEnable))]
        public static class EnableSprite
        {
            public static void Postfix() {
                GameObject innerVisors = GameObject.Find("VisorGroup").transform.GetChild(0).transform.GetChild(0).gameObject;
                int visor = 0;
                for (int i = 1; i < innerVisors.transform.GetChildCount(); i++) {
                    if (innerVisors.transform.GetChild(i).transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == null) {
                        innerVisors.transform.GetChild(i).transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetSprite(authorDatas[visor].VisorName);
                        visor += 1;
                    }
                }
            }
        }

        public static Sprite GetSprite(string name)
                => AssetLoader.LoadVisorsAsset(name).Cast<GameObject>().GetComponent<SpriteRenderer>().sprite;

        public VisorViewData visorViewData;
        public CustomVisors(VisorViewData hvd) {
            visorViewData = hvd;
        }

        static Dictionary<string, VisorViewData> cache = new();
        static VisorViewData getbycache(string id) {
            if (!cache.ContainsKey(id)) {
                cache[id] = customVisorData.FirstOrDefault(x => x.ProductId == id).visorViewData;
            }
            return cache[id];
        }

        [HarmonyPatch(typeof(CosmeticsCache), nameof(CosmeticsCache.GetVisor))]
        class CosmeticsCacheGetVisorPatch
        {
            public static bool Prefix(CosmeticsCache __instance, string id, ref VisorViewData __result) {
                if (!id.StartsWith("lmj_")) return true;
                __result = getbycache(id);
                if (__result == null)
                    __result = __instance.visors["visor_EmptyVisor"].GetAsset();
                return false;
            }
        }

        [HarmonyPatch(typeof(VisorLayer), nameof(VisorLayer.UpdateMaterial))]
        class VisorLayerUpdateMaterialPatch
        {
            public static bool Prefix(VisorLayer __instance) {
                if (__instance.currentVisor == null || !__instance.currentVisor.ProductId.StartsWith("lmj_")) return true;
                VisorViewData asset = getbycache(__instance.currentVisor.ProductId);                
                if (asset.AltShader) {
                    __instance.Image.sharedMaterial = asset.AltShader;
                }
                else {
                    __instance.Image.sharedMaterial = FastDestroyableSingleton<HatManager>.Instance.DefaultShader;
                }
                PlayerMaterial.SetColors(__instance.matProperties.ColorId, __instance.Image);
                switch (__instance.matProperties.MaskType) {
                    case PlayerMaterial.MaskType.SimpleUI:
                    case PlayerMaterial.MaskType.ScrollingUI:
                        __instance.Image.maskInteraction = (SpriteMaskInteraction)1;
                        break;
                    case PlayerMaterial.MaskType.Exile:
                        __instance.Image.maskInteraction = (SpriteMaskInteraction)2;
                        break;
                    default:
                        __instance.Image.maskInteraction = (SpriteMaskInteraction)0;
                        break;
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(VisorLayer), nameof(VisorLayer.SetFlipX))]
        class VisorLayerSetFlipXPatch
        {
            public static bool Prefix(VisorLayer __instance, bool flipX) {
                if (__instance.currentVisor == null || !__instance.currentVisor.ProductId.StartsWith("lmj_")) return true;
                __instance.Image.flipX = flipX;
                VisorViewData asset = getbycache(__instance.currentVisor.ProdId);
                if (flipX && asset.LeftIdleFrame) {
                    __instance.Image.sprite = asset.LeftIdleFrame;
                }
                else {
                    __instance.Image.sprite = asset.IdleFrame;
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(VisorLayer), nameof(VisorLayer.SetVisor), new Type[] { typeof(VisorData), typeof(VisorViewData), typeof(int) })]
        class VisorLayerSetVisorPositionPatch
        {
            public static bool Prefix(VisorLayer __instance, VisorData data, VisorViewData visorView, int colorId) {
                if (!data.ProductId.StartsWith("lmj_")) return true;
                __instance.currentVisor = data;
                if (data.BehindHats) {
                    __instance.transform.SetLocalZ(__instance.ZIndexSpacing * -1.5f);
                }
                else {
                    __instance.transform.SetLocalZ(__instance.ZIndexSpacing * -3f);
                }
                __instance.SetFlipX(__instance.Image.flipX);
                __instance.SetMaterialColor(colorId);
                return false;
            }
        }

        [HarmonyPatch(typeof(VisorLayer), nameof(VisorLayer.SetVisor), new Type[] { typeof(VisorData), typeof(int) })]
        class VisorLayerSetVisorPatch
        {
            public static bool Prefix(VisorLayer __instance, VisorData data, int colorId) {
                if (!data.ProductId.StartsWith("lmj_")) return true;
                __instance.currentVisor = data;
                __instance.SetVisor(__instance.currentVisor, getbycache(data.ProdId), colorId);
                return false;
            }
        }
    }
}