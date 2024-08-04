using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using Innersloth.Assets;
using System;
using UnityEngine.AddressableAssets;
using static LasMonjas.Core.CustomNameplates;
using static Il2CppSystem.Globalization.CultureInfo;

namespace LasMonjas.Core
{
    public class CustomNameplates : NamePlateData
    {
        public TempPlateViewData tpvd;
        public class TempPlateViewData
        {
            public Sprite Image;
            public NamePlateViewData Create {
                get {
                    return new() {
                        Image = Image
                    };
                }
            }
        };
        static Dictionary<string, NamePlateViewData> cache = new();
        static NamePlateViewData getbycache(string id) {
            if (!cache.ContainsKey(id) || cache[id] == null) {
                CustomNameplates cpd = CustomPlate.customPlateData.FirstOrDefault(x => x.ProductId == id);
                if (cpd != null) {
                    cache[id] = cpd.tpvd.Create;
                }
                else {
                    cache[id] = FastDestroyableSingleton<HatManager>.Instance.GetNamePlateById(id)?.CreateAddressableAsset()?.GetAsset();
                }
            }
            return cache[id];
        }
        [HarmonyPatch(typeof(CosmeticsCache), nameof(CosmeticsCache.GetNameplate))]
        class CosmeticsCacheGetPlatePatch
        {
            public static bool Prefix(CosmeticsCache __instance, string id, ref NamePlateViewData __result) {
                if (!id.StartsWith("lmj_")) return true;
                __result = getbycache(id);
                if (__result == null)
                    __result = __instance.nameplates["nameplate_NoPlate"].GetAsset();
                return false;
            }
        }
        [HarmonyPatch(typeof(NameplatesTab), nameof(NameplatesTab.OnEnable))]
        class NameplatesTabOnEnablePatch
        {
            static void makecoro(NameplatesTab __instance, NameplateChip chip) {
                __instance.StartCoroutine(AddressableAssetExtensions.CoLoadAssetAsync<NamePlateViewData>(__instance, FastDestroyableSingleton<HatManager>.Instance.GetNamePlateById(chip.ProductId).ViewDataRef, (Action<NamePlateViewData>)delegate (NamePlateViewData viewData)
                {
                    chip.image.sprite = viewData?.Image;
                }));
            }
            public static void Postfix(NameplatesTab __instance) {
                __instance.StopAllCoroutines();
                foreach (NameplateChip chip in __instance.scroller.Inner.GetComponentsInChildren<NameplateChip>()) {
                    if (chip.ProductId.StartsWith("lmj_")) {
                        NamePlateViewData npvd = getbycache(chip.ProductId);
                        chip.image.sprite = npvd.Image;
                    }
                    else {
                        makecoro(__instance, chip);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.PreviewNameplate))]
        class VisorLayerUpdateMaterialPatch
        {
            public static void Postfix(PlayerVoteArea __instance, string plateID) {
                if (!plateID.StartsWith("lmj_")) return;
                NamePlateViewData npvd = getbycache(plateID);
                if (npvd != null) {
                    __instance.Background.sprite = npvd.Image;
                }
            }
        }
    }

    public class CustomPlate
    {
        public struct AuthorData
        {
            public string AuthorName;
            public string NamePlateName;
        }

        public static List<AuthorData> authorDatas = new List<AuthorData>()
        {
            new AuthorData {AuthorName = "Allul", NamePlateName = "Monja"},
            new AuthorData {AuthorName = "Sensei", NamePlateName = "Among Ass"},
            new AuthorData {AuthorName = "Sensei", NamePlateName = "Millennium Lore"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Muaresito Joy"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Shusron Radar"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Winsus XP"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Be not afraid Allul"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Bounty Hunter"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Yinyanger"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Challenger"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Ninja"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Objsustion"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Deal with Pi"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "El Mauro"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "The Monja"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Lags the Game"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Happy 1st Birthday Monjas"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Sam va lentin"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Su... Suspai"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Yandere"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Samba Lentin"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Zargothrax Business Card"},
            new AuthorData {AuthorName = "Muaresito", NamePlateName = "Devourer"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "Submerged"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "What the cow"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "Bluescreen"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "Lot of bodies"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "RIP"},
            new AuthorData {AuthorName = "AD", NamePlateName = "Jailed"},
            new AuthorData {AuthorName = "Nyxx", NamePlateName = "Report"},
        }; 
        
        public static bool isAdded = false;
        static readonly List<NamePlateData> namePlateData = new();
        public static readonly List<CustomNameplates> customPlateData = new();
        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetNamePlateById))]
        class UnlockedNamePlatesPatch
        {
            public static void Postfix(HatManager __instance) {

                if (isAdded) return;
                isAdded = true;
                var AllPlates = __instance.allNamePlates.ToList();
                
                foreach (var file in authorDatas) {
                    try {
                        TempPlateViewData tpvd = new() {
                            Image = GetSprite(file.NamePlateName)
                        };

                        var assetRef = new AssetReference(tpvd.Create.Pointer);

                        var plate = new CustomNameplates {
                            tpvd = tpvd,
                            name = file.NamePlateName + "\nby " + file.AuthorName,
                            ProductId = "lmj_" + file.NamePlateName.Replace(' ', '_'),
                            BundleId = "lmj_" + file.NamePlateName.Replace(' ', '_'),
                            displayOrder = 99,
                            ChipOffset = new Vector2(0f, 0.2f),
                            Free = true,
                            ViewDataRef = assetRef,
                        };
                        plate.CreateAddressableAsset();
                        namePlateData.Add(plate);
                        customPlateData.Add(plate);
                    }
                    catch {
                    }
                }
                AllPlates.AddRange(namePlateData);
                __instance.allNamePlates = AllPlates.ToArray();
            }
        }
        public static Sprite GetSprite(string name)
                => AssetLoader.LoadNamePlateAsset(name).Cast<GameObject>().GetComponent<SpriteRenderer>().sprite;
    }
}