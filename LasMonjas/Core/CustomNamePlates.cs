using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using Innersloth.Assets;
using System;
using UnityEngine.AddressableAssets;
using AmongUs.Data;
using Reactor.Utilities.Extensions;
using Reactor.Utilities;
using TMPro;

namespace LasMonjas.Core
{
    public class CustomNameplates : NamePlateData
    {
        public record AuthorData(
            string AuthorName,
            string NamePlateName
        );

        public static List<AuthorData> authorDatas = new List<AuthorData>()
        {
            new ("Allul", "Monja"),
            new ("Sensei", "Among Ass"),
            new ("Sensei", "Millennium Lore"),
            new ("Muaresito", "Muaresito Joy"),
            new ("Muaresito", "Shusron Radar"),
            new ("Muaresito", "Winsus XP"),
            new ("Muaresito", "Be not afraid Allul"),
            new ("Muaresito", "Bounty Hunter"),
            new ("Muaresito", "Yinyanger"),
            new ("Muaresito", "Challenger"),
            new ("Muaresito", "Ninja"),
            new ("Muaresito", "Objsustion"),
            new ("Muaresito", "Deal with Pi"),
            new ("Muaresito", "El Mauro"),
            new ("Muaresito", "The Monja"),
            new ("Muaresito", "Lags the Game"),
            new ("Muaresito", "Happy 1st Birthday Monjas"),
            new ("Muaresito", "Sam va lentin"),
            new ("Muaresito", "Su... Suspai"),
            new ("Muaresito", "Yandere"),
            new ("Muaresito", "Samba Lentin"),
            new ("Muaresito", "Zargothrax Business Card"),
            new ("Muaresito", "Devourer"),
            new ("Muaresito", "Typical Ragder of Rubianes"),
            new ("Blocky", "Submerged"),
            new ("Blocky", "What the cow"),
            new ("Blocky", "Bluescreen"),
            new ("Blocky", "Lot of bodies"),
            new ("Blocky", "RIP"),
            new ("AD", "Jailed"),
            new ("Nyxx", "Report"),
        };

        public static bool _customNameplatesLoaded = false;
        static readonly List<NamePlateData> namePlateData = new();
        public static readonly List<CustomNameplates> customPlateData = new();
        public static readonly Dictionary<string, NamePlateViewData> CustomNameplateViewDatas = [];
        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetNamePlateById))]
        class UnlockedNamePlatesPatch
        {
            public static void Postfix(HatManager __instance) {
                if (_customNameplatesLoaded) return;
                _customNameplatesLoaded = true;
                var AllNameplates = __instance.allNamePlates.ToList();

                foreach (var data in authorDatas) {
                    NamePlateViewData npvd = new NamePlateViewData();
                    npvd.Image = GetSprite(data.NamePlateName);

                    var nameplate = new CustomNameplates(npvd);
                    nameplate.name = $"{data.NamePlateName} (by {data.AuthorName})";
                    nameplate.ProductId = "lmj_" + nameplate.name.Replace(' ', '_');
                    nameplate.BundleId = "lmj_" + nameplate.name.Replace(' ', '_');
                    nameplate.displayOrder = 99;
                    nameplate.ChipOffset = new Vector2(0f, 0.2f);
                    nameplate.Free = true;
                    namePlateData.Add(nameplate);
                    customPlateData.Add(nameplate);
                    var assetRef = new AssetReference(npvd.Pointer);
                    nameplate.ViewDataRef = assetRef;
                    nameplate.CreateAddressableAsset();
                    CustomNameplateViewDatas.TryAdd(nameplate.ProductId, npvd);
                }
                AllNameplates.AddRange(namePlateData);
                __instance.allNamePlates = AllNameplates.ToArray();
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

                    if (CustomNameplateViewDatas.TryGetValue(colorChip.ProductId, out var viewData)) {
                        colorChip.gameObject.GetComponent<NameplateChip>().image.sprite = viewData.Image;
                    }
                    else {
                        var plate = HatManager.Instance.GetNamePlateById(colorChip.ProductId);

                        if (plate?.ViewDataRef != null) {
                            DefaultNameplateCoro(__instance, colorChip.gameObject.GetComponent<NameplateChip>());
                        }
                    }

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

                    packages.TryAdd(package, []);

                    packages[package].Add(data);
                }

                var YOffset = __instance.YStart;
                Template = __instance.transform.FindChild("Text").gameObject.GetComponent<TMP_Text>();
                var keys = packages.Keys.OrderBy(x => x switch {
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

        public CustomNameplates(NamePlateViewData npvd) {
            nameplateViewData = npvd;
        }

        static readonly Dictionary<string, NamePlateViewData> cache = new();
        static NamePlateViewData getbycache(string id) {
            if (!cache.TryGetValue(id, out var value) || value == null) {
                value = customPlateData.FirstOrDefault(x => x.ProductId == id)?.nameplateViewData;

                cache[id] = value;
            }
            return value;
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
}