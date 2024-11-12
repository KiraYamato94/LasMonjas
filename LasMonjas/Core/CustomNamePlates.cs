using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using Innersloth.Assets;
using System;
using UnityEngine.AddressableAssets;
using static Il2CppSystem.Globalization.CultureInfo;
using AmongUs.Data;
using Reactor.Utilities.Extensions;
using Reactor.Utilities;
using TMPro;

namespace LasMonjas.Core
{
    public class CustomNamePlates : NamePlateData
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

        public static bool _customNameplatesLoaded = false;
        static readonly List<NamePlateData> namePlateData = new();
        public static readonly List<CustomNamePlates> customPlateData = new();
        public static readonly Dictionary<string, NamePlateViewData> CustomNameplateViewDatas = [];

        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetNamePlateById))]
        class UnlockedNamePlatesPatch
        {
            public static void Postfix(HatManager __instance) {

                if (_customNameplatesLoaded) return;
                _customNameplatesLoaded = true;
                var AllPlates = __instance.allNamePlates.ToList();

                foreach (var data in authorDatas) {
                    NamePlateViewData nvd = new NamePlateViewData();
                    nvd.Image = GetSprite(data.NamePlateName);

                    var nameplate = new CustomNamePlates(nvd);
                    nameplate.name = $"{data.NamePlateName} (by {data.AuthorName})";
                    nameplate.ProductId = "lmj_" + nameplate.name.Replace(' ', '_');
                    nameplate.BundleId = "lmj_" + nameplate.name.Replace(' ', '_');
                    nameplate.displayOrder = 99;
                    nameplate.ChipOffset = new Vector2(0f, 0.2f);
                    nameplate.Free = true;
                    namePlateData.Add(nameplate);
                    customPlateData.Add(nameplate);
                    var assetRef = new AssetReference(nvd.Pointer);
                    nameplate.ViewDataRef = assetRef;
                    nameplate.CreateAddressableAsset();
                    CustomNameplateViewDatas.TryAdd(nameplate.ProductId, nvd);
                }

                AllPlates.AddRange(namePlateData);
                __instance.allNamePlates = AllPlates.ToArray();
            }
        }

        [HarmonyPatch(typeof(NameplatesTab), nameof(NameplatesTab.OnEnable))]
        public static class NameplatesTabOnEnablePatch
        {
            private static TMP_Text Template;

            private static float CreateNameplatePackage(List<NamePlateData> nameplates, string packageName, float YStart, NameplatesTab __instance) {
                
                var offset = YStart;

                if (Template) {
                    var title = UnityEngine.Object.Instantiate(Template, __instance.scroller.Inner);
                    var material = title.GetComponent<MeshRenderer>().material;
                    material.SetFloat("_StencilComp", 4f);
                    material.SetFloat("_Stencil", 1f);
                    title.transform.localPosition = new(2.25f, YStart, -1f);
                    title.transform.localScale = Vector3.one * 1.5f;
                    title.fontSize *= 0.5f;
                    title.enableAutoSizing = false;
                    Coroutines.Start(Helpers.PerformTimedAction(0.1f, _ => title.SetText(packageName, true)));
                    offset -= 0.8f * __instance.YOffset;
                }

                for (var i = 0; i < nameplates.Count; i++) {
                    var nameplate = nameplates[i];
                    var xpos = __instance.XRange.Lerp(i % __instance.NumPerRow / (__instance.NumPerRow - 1f));
                    var ypos = offset - (i / __instance.NumPerRow * __instance.YOffset);
                    var colorChip = UnityEngine.Object.Instantiate(__instance.ColorTabPrefab, __instance.scroller.Inner);

                    if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard) {
                        colorChip.Button.OverrideOnMouseOverListeners(() => __instance.SelectNameplate(nameplate));
                        colorChip.Button.OverrideOnMouseOutListeners(() => __instance.SelectNameplate(HatManager.Instance.GetNamePlateById(DataManager.Player.Customization.NamePlate)));
                        colorChip.Button.OverrideOnClickListeners(__instance.ClickEquip);
                    }
                    else
                        colorChip.Button.OverrideOnClickListeners(() => __instance.SelectNameplate(nameplate));

                    colorChip.Button.ClickMask = __instance.scroller.Hitbox;
                    colorChip.transform.localPosition = new(xpos, ypos, -1f);
                    colorChip.ProductId = nameplate.ProductId;
                    colorChip.Tag = nameplate;
                    colorChip.SelectionHighlight.gameObject.SetActive(false);

                    if (CustomNameplateViewDatas.TryGetValue(colorChip.ProductId, out var viewData))
                        colorChip.gameObject.GetComponent<NameplateChip>().image.sprite = viewData.Image;
                    else
                        DefaultNameplateCoro(__instance, colorChip.gameObject.GetComponent<NameplateChip>());

                    __instance.ColorChips.Add(colorChip);
                }

                return offset - ((nameplates.Count - 1) / __instance.NumPerRow * __instance.YOffset) - 1.5f;
            }

            private static void DefaultNameplateCoro(NameplatesTab __instance, NameplateChip chip) => __instance.StartCoroutine(__instance.CoLoadAssetAsync<NamePlateViewData>(HatManager.Instance
                .GetNamePlateById(chip.ProductId).ViewDataRef, (Action<NamePlateViewData>)(viewData => chip.image.sprite = viewData?.Image)));

            public static bool Prefix(NameplatesTab __instance) {
                for (var i = 0; i < __instance.scroller.Inner.childCount; i++)
                    __instance.scroller.Inner.GetChild(i).gameObject.Destroy();

                __instance.ColorChips = new();
                var array = HatManager.Instance.GetUnlockedNamePlates();
                var packages = new Dictionary<string, List<NamePlateData>>();

                foreach (var data in array) {
                    
                    var package = "Innersloth";

                    if (data.ProductId.StartsWith("lmj_"))
                        package = "Las Monjas";

                    if (!packages.ContainsKey(package))
                        packages[package] = [];

                    packages[package].Add(data);
                }

                var YOffset = __instance.YStart;
                Template = __instance.transform.FindChild("Text").gameObject.GetComponent<TMP_Text>();
                var keys = packages.Keys.OrderBy(x => x switch
                {
                    "Innersloth" => 4,
                    "Las Monjas" => 1,
                    _ => 2
                });
                keys.ForEach(key => YOffset = CreateNameplatePackage(packages[key], key, YOffset, __instance));

                if (array.Length != 0)
                    __instance.GetDefaultSelectable().PlayerEquippedForeground.SetActive(true);

                __instance.plateId = DataManager.Player.Customization.NamePlate;
                __instance.currentNameplateIsEquipped = true;
                __instance.SetScrollerBounds();
                __instance.scroller.ContentYBounds.max = -(YOffset + 3.8f);
                return false;
            }
        }

        public static Sprite GetSprite(string name)
                    => AssetLoader.LoadNamePlateAsset(name).Cast<GameObject>().GetComponent<SpriteRenderer>().sprite;
        
        public NamePlateViewData nameplateViewData;
        public CustomNamePlates(NamePlateViewData hvd) {
            nameplateViewData = hvd;
        }
        static Dictionary<string, NamePlateViewData> cache = new();
        static NamePlateViewData getbycache(string id) {
            if (!cache.ContainsKey(id)) {
                cache[id] = customPlateData.FirstOrDefault(x => x.ProductId == id).nameplateViewData;
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

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.PreviewNameplate))]
        class PreviewNameplatesPatch
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
}