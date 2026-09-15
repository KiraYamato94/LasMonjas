using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using UnityEngine.AddressableAssets;
using PowerTools;
using AmongUs.Data;
using Reactor.Utilities.Extensions;
using Reactor.Utilities;
using TMPro;

// Adapted from https://github.com/xxomega77xx/HatPack

namespace LasMonjas.Core
{
    class CustomHats
    {
        public static Material MagicShader;

        public record AuthorData(
            string AuthorName,
            string HatName,
            bool NoBounce = false,
            bool AltShader = false,
            string FloorHatName = null,
            string ClimbHatName = null,
            string LeftImageName = null
        );

        public static List<AuthorData> authorDatas = new List<AuthorData>()
        {
            new ("Allul", "Monja", true),
            new ("Allul", "Minion Monja", true, false, "Minion Monja Climb","Minion Monja Climb", "Minion Monja"),
            new ("Sensei", "Cursed Monja", true),
            new ("Sensei", "Abombg Man", true, true),
            new ("Sensei", "Among Ass", false, true),
            new ("Sensei", "Time To Duel", true, true),
            new ("Sensei", "Medusa"),
            new ("Sensei", "Mega Hat", true, true),
            new ("Sensei", "Egyptian", true),
            new ("Sensei", "Joker", true),
            new ("Sensei", "SrCobra", true, true),
            new ("Sensei", "Dinoseto", true, true),
            new ("Sensei", "Super Red Sus"),
            new ("Sensei", "Super Green Sus"),
            new ("Sensei", "Super Yellow Sus"),
            new ("Sensei", "Super Purple Sus"),
            new ("Sensei", "Chadsito", true, true),
            new ("Sensei", "Scars", true, true),
            new ("Sensei", "Sus Man", true),
            new ("Sensei", "Take It Easy", true),
            new ("Sensei", "Moon Face", true, true),
            new ("Sensei", "Pepper Carrot", true, true),
            new ("Sensei", "Battle Armor", true, true),
            new ("Sensei", "Space Captain", true),
            new ("Sensei", "Dontaegamez", true),
            new ("Sensei", "Blocky", true),
            new ("Sensei", "Glitch", true, true),
            new ("Sensei", "Boot", true, true),
            new ("Sensei", "Monja Cloth", true, true),
            new ("Sensei", "Majin Sus", true, true),
            new ("Sensei", "El Mauro", true),
            new ("Sensei", "Angry Dontae", true),
            new ("Sensei", "Blocky 16bits", true),
            new ("Sensei", "Fascinante", true),
            new ("Sensei", "Pingas", true, true),
            new ("Sensei", "Suavemente", true),
            new ("Sensei", "Suscolo", true, true),
            new ("Muaresito", "Cell", true, true),
            new ("Muaresito", "Ghost", true, true),
            new ("Muaresito", "Goodbye", true),
            new ("Muaresito", "Muaresito Joy", true),
            new ("Muaresito", "Avatar", true, true),
            new ("Muaresito", "Scallop Walker", true),
            new ("Muaresito", "Unknown Race", true, true),
            new ("Muaresito", "Kill Palex", true),
            new ("Muaresito", "Olmaito", true),
            new ("Muaresito", "Susking", true, true),
            new ("Muaresito", "Tree Brows", true),
            new ("Muaresito", "Susboy", true, true),
            new ("Muaresito", "Xabasus", true, true),
            new ("Muaresito", "Bee", true, true),
            new ("Muaresito", "Bounty Hunter"),
            new ("Muaresito", "King Skull", true),
            new ("Muaresito", "Josefa", true),
            new ("Muaresito", "Monjart", true),
            new ("Muaresito", "Susnic", true, true),
            new ("Muaresito", "Homunculus", true, true),
            new ("Muaresito", "2nd Actor Hair", true, true),
            new ("Muaresito", "4th Anniversary", true, true),
            new ("Muaresito", "Exsusdia", true),
            new ("Muaresito", "True Exsusdia", true),
            new ("Muaresito", "Worker Hat", true),
            new ("Muaresito", "Raul", true, true),
            new ("Muaresito", "Chainsus Man", true, true),
            new ("Muaresito", "4M0NJ-4S Tank", true),
            new ("Muaresito", "Too much tasks", true, true),
            new ("Muaresito", "Octosus", true, true),
            new ("Muaresito", "Sustrio", true, true),
            new ("Muaresito", "Penguin", true),
            new ("Muaresito", "Happy 1st Birthday Monjas"),
            new ("Muaresito", "1st Monjiversario", false, true),
            new ("Muaresito", "Carmina Vacaloura", true, true),
            new ("Muaresito", "Bunny Hood", true, true),
            new ("Muaresito", "Sussykill", true, true),
            new ("Muaresito", "Stuffwell", true, true),
            new ("Muaresito", "Sussybara", true, true),
            new ("Muaresito", "Sin Embargo", true, true),
            new ("Xago", "World Destroyer", true),
            new ("Xago", "Amazing Robot", true),
            new ("Xago", "Fourze", true),
            new ("Xago", "Zargothrax", true),
            new ("Xago", "Chaos Wizard", true),
            new ("Xago", "Robot Armor", true, true),
            new ("Xago", "Canon Robot", true),
            new ("Hige", "Punsus", true),
            new ("IceCreamGuy", "Ice Cream Man"),
            new ("IceCreamGuy", "Devious Bling", true, true),
            new ("IceCreamGuy", "Hungry Hat", true),
            new ("Sen", "Artist", true, false, "Artist_climb", "Artist_climb", "Artist"),
            new ("lotty", "Bubbles", true),
            new ("lotty", "Black Cat"),
            new ("lotty", "White Cat"),
            new ("lotty", "Clown", true),
            new ("lotty", "Raccoon"),
            new ("lotty", "Impostor Raccoon"),
            new ("lotty", "Periodt", true),
            new ("lotty", "GD"),
            new ("lotty", "Card", true, true),
            new ("lotty", "Flower Crown", true),
            new ("lotty", "Good Noodle", true),
            new ("lotty", "Long Wiggle", true, true),
            new ("lotty", "Neon Devil Horns", true),
            new ("lotty", "Purple Halo", true),
            new ("lotty", "Sword", true),
            new ("lotty", "uwu", true, true),
            new ("lotty", "Shark", true, true),
            new ("lotty", "All Ears", true, true),
            new ("lotty", "Babies"),
            new ("lotty", "Beans", true),
            new ("lotty", "Cat", true, true),
            new ("lotty", "Dress", true, true),
            new ("lotty", "Ghost Hat", true, true),
            new ("lotty", "Rainbow"),
            new ("lotty", "Shark Plush"),
            new ("lotty", "Top Hat", true, true),
            new ("lotty", "Umbrella", true, true),
            new ("lotty", "Axolotl", true, true),
            new ("lotty", "(je)Sushi"),
            new ("lotty", "Angry Chicken"),
            new ("lotty", "Best Friend", true),
            new ("lotty", "Bug Girl", true),
            new ("lotty", "Cat Thief", true),
            new ("ERIKHAPPY", "Blue Scarf", true),
            new ("ERIKHAPPY", "Egg"),
            new ("Jesushi", "Jester", true),
            new ("Jesushi", "Crown", true),
            new ("Booman", "Sniper", true),
            new ("Booman", "Rocketman", true, true),
            new ("Booman", "Wooden Box", true, true),
            new ("Dontae", "Tea Cup", true, true),
            new ("Boa", "Cat Princess", true, false, "Cat Princess_climb","Cat Princess_climb", "Cat Princess"),
            new ("Xeno<33", "3rd Eye", true, true),
            new ("Xeno<33", "Candles"),
            new ("Xeno<33", "Double Visor", true, true),
            new ("Xeno<33", "Green Hat", true),
            new ("Xeno<33", "Idea"),
            new ("Xeno<33", "Sheep"),
            new ("Xeno<33", "Sus Guy", true),
            new ("Xeno<33", "UFO", true),
            new ("Xeno<33", "Electric Rat", true, true),
            new ("Xeno<33", "Royal Blonde Hair", true),
            new ("Xeno<33", "Blue Hat", true),
            new ("Xeno<33", "Cloak", true),
            new ("Xeno<33", "Empty Charge"),
            new ("Xeno<33", "Charging"),
            new ("Xeno<33", "Full Charge"),
            new ("Xeno<33", "Funny Ghost", true),
            new ("Xeno<33", "Mushrooms"),
            new ("Xeno<33", "Pink Flower", true),
            new ("Xeno<33", "Purple Animatronic", true),
            new ("Xeno<33", "Watermelon", true, true),
            new ("Xeno<33", "Pet Cat", true, true),
            new ("Xeno<33", "Ganso", true, true),
            new ("Xeno<33", "Ninja", true),
            new ("Xeno<33", "Susnana", true),
            new ("Xeno<33", "Ghostly", true),
            new ("Xeno<33", "Suspucha", true),
            new ("Xeno<33", "Kitty Hat", true),
            new ("Nyxx", "Headphone gamer", true),
            new ("Nyxx", "Heart Tiera", true),
            new ("Nyxx", "Rubber Ring", true),
            new ("Nyxx", "Shsusrek", true, true),
            new ("Nyxx", "Strawberry", true, true),
            new ("Nyxx", "Sustalian", true),
            new ("Nyxx", "Love You", true, true),
            new ("Nyxx", "Ponycorn", true, true),
            new ("Nyxx", "Fox", true, true),
            new ("Nyxx", "Mantis", true, true),
            new ("Nyxx", "Magic Hat", true),
            new ("Nyxx", "Nurse", true),
            new ("Nyxx", "Sustich", true, true),
            new ("Nyxx", "Dog", true, true),
            new ("Nyxx", "Bunny", true, true),
            new ("Nyxx", "Cactus", true, true),
            new ("Nyxx", "Lasus", true, true),
            new ("Nyxx", "Susken", true, true),
            new ("Nyxx", "Cat Face", true, true),
            new ("Nyxx", "Fluffy Scarf", true, true),
            new ("Nyxx", "Flower Lady", true, true),
            new ("Nyxx", "Halo", true),
            new ("Nyxx", "On Fire", true),
            new ("Nyxx", "Frog Hat", true),
            new ("Nyxx", "Magician Hat", true),
            new ("Sonrio", "Flaming", true),
            new ("Sonrio", "Freezing", true),
            new ("Sonrio", "Night Friday", true, true),
            new ("Sonrio", "Puppetist", true, true),
            new ("Sonrio", "Calling All Crewmates", true),
            new ("Sonrio", "Fighter", true, true),
            new ("Sonrio", "Woomy Girl", true, true),
            new ("Sonrio", "Woomy Boy", true, true),
            new ("Sonrio", "Tanuki", true, true),
            new ("Ravengirl", "Flag", true, true),
            new ("Dr Blockhead", "Got Any Grapes", true),
            new ("Dr Blockhead", "Bucket", true),
        }; 

        private static bool _customHatsLoaded = false;
        
        internal static Dictionary<int, AuthorData> IdToData = new Dictionary<int, AuthorData>();
        public static Dictionary<string, HatViewData> CustomHatViewDatas = new Dictionary<string, HatViewData>();

        
        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetHatById))]
        public static class AddCustomHats
        {
            public static void Postfix(HatManager __instance) {

                if (!_customHatsLoaded) {
                    var allHats = __instance.allHats.ToList();

                    foreach (var data in authorDatas) {
                        HatID++;
                        allHats.Add(CreateHatFromData(data));
                        IdToData.Add(HatManager.Instance.allHats.Count + HatID, data);
                    }

                    _customHatsLoaded = true;
                    __instance.allHats = allHats.ToArray();
                }
            }

            public static int HatID = 0;

            private static HatData CreateHatFromData(AuthorData data) {
                Sprite main = GetSprite(data.HatName);
                //Sprite floor = string.IsNullOrEmpty(data.FloorHatName) ? null : GetSprite(data.FloorHatName);
                //Sprite climb = string.IsNullOrEmpty(data.ClimbHatName) ? null : GetSprite(data.ClimbHatName);
                //Sprite left = string.IsNullOrEmpty(data.LeftImageName) ? null : GetSprite(data.LeftImageName);

                return CreateHat(
                    main,
                    data.AuthorName,
                    data.NoBounce,
                    data.AltShader/*,
                    floor,
                    climb,
                    left*/
                    );

            }
            
            private static HatData CreateHat(Sprite sprite, string author, bool bounce = false, bool altshader = false/*, Sprite floor = null, Sprite climb = null, Sprite left = null*/) {
                //Borrowed from Other Roles to get hats alt shaders to work
                if (MagicShader == null) {
                    Material hatShader = FastDestroyableSingleton<HatManager>.Instance.PlayerMaterial;
                    MagicShader = hatShader;
                }

                var viewdata = ScriptableObject.CreateInstance<HatViewData>();
                viewdata.MainImage = sprite;
                viewdata.FloorImage = viewdata.MainImage;                
                
                HatData newHat = ScriptableObject.CreateInstance<HatData>();
                newHat.name = $"{sprite.name} (by {author})";
                newHat.ProductId = "lmj_" + sprite.name.Replace(' ', '_');
                newHat.displayOrder = 99 + HatID;
                newHat.InFront = true;
                newHat.NoBounce = bounce;                
                newHat.Free = true;
                newHat.ChipOffset = new Vector2(-0.1f, 0.2f);
                if (altshader) { viewdata.MatchPlayerColor = true; }                
                CustomHatViewDatas.Add(newHat.name, viewdata);
                var assetRef = new AssetReference(viewdata.Pointer);

                newHat.ViewDataRef = assetRef;
                newHat.CreateAddressableAsset();
                return newHat;
            }
        }

        [HarmonyPatch(typeof(HatsTab), nameof(HatsTab.OnEnable))]
        public static class HatsTabOnEnablePatch
        {
            private static TMP_Text Template;

            private static float CreateHatPackage(List<HatData> hats, string packageName, float YStart, HatsTab __instance) {
                
                var offset = YStart;

                if (Template) {
                    var title = UnityEngine.Object.Instantiate(Template, __instance.scroller.Inner);
                    title.transform.localPosition = new(2.25f, YStart, -1f);
                    title.transform.localScale = Vector3.one * 1.5f;
                    title.fontSize *= 0.5f;
                    title.enableAutoSizing = false;
                    Coroutines.Start(Helpers.PerformTimedAction(0.1f, _ => title.SetText(packageName)));
                    offset -= 0.8f * __instance.YOffset;
                }

                for (var i = 0; i < hats.Count; i++) {
                    var hat = hats[i];
                    var xpos = __instance.XRange.Lerp(i % __instance.NumPerRow / (__instance.NumPerRow - 1f));
                    var ypos = offset - (i / __instance.NumPerRow * __instance.YOffset);
                    var colorChip = UnityEngine.Object.Instantiate(__instance.ColorTabPrefab, __instance.scroller.Inner);

                    if (ActiveInputManager.currentControlType == ActiveInputManager.InputType.Keyboard) {
                        colorChip.Button.OverrideOnMouseOverListeners(() => __instance.SelectHat(hat));
                        colorChip.Button.OverrideOnMouseOutListeners(() => __instance.SelectHat(HatManager.Instance.GetHatById(DataManager.Player.Customization.Hat)));
                        colorChip.Button.OverrideOnClickListeners(__instance.ClickEquip);
                    }
                    else
                        colorChip.Button.OverrideOnClickListeners(() => __instance.SelectHat(hat));

                    colorChip.transform.localPosition = new(xpos, ypos, -1f);
                    colorChip.Button.ClickMask = __instance.scroller.Hitbox;
                    colorChip.Inner.SetMaskType(PlayerMaterial.MaskType.SimpleUI);
                    __instance.UpdateMaterials(colorChip.Inner.FrontLayer, hat);
                    colorChip.Inner.SetHat(hat, __instance.HasLocalPlayer() ? PlayerInCache.LocalPlayer.PlayerControl.Data.DefaultOutfit.ColorId : DataManager.Player.Customization.Color);
                    colorChip.Inner.transform.localPosition = hat.ChipOffset;
                    colorChip.Tag = hat;
                    colorChip.SelectionHighlight.gameObject.SetActive(false);
                    __instance.ColorChips.Add(colorChip);
                }

                return offset - ((hats.Count - 1) / __instance.NumPerRow * __instance.YOffset) - 1.75f;
            }

            public static bool Prefix(HatsTab __instance) {
                for (var i = 0; i < __instance.scroller.Inner.childCount; i++)
                    __instance.scroller.Inner.GetChild(i).gameObject.Destroy();

                __instance.ColorChips = new();
                var array = HatManager.Instance.GetUnlockedHats();
                var packages = new Dictionary<string, List<HatData>>();

                foreach (var data in array) {
                    
                    var package = "Innersloth";

                    if (data.ProductId.StartsWith("lmj_"))
                        package = "Las Monjas";
                  
                        packages.TryAdd(package, []); 

                    packages[package].Add(data);
                }

                var YOffset = __instance.YStart;
                Template = GameObject.Find("HatsGroup").transform.FindChild("Text").GetComponent<TMP_Text>();
                var keys = packages.Keys.OrderBy(x => x switch
                {
                    "Innersloth" => 4,
                    "Las Monjas" => 1,
                    _ => 2
                });
                keys.ForEach(key => YOffset = CreateHatPackage(packages[key], key, YOffset, __instance));
                __instance.currentHatIsEquipped = true;
                __instance.SetScrollerBounds();
                __instance.scroller.ContentYBounds.max = -(YOffset + 4.1f);
                return false;
            }
        }

        public static Sprite GetSprite(string name)
                => AssetLoader.LoadHatAsset(name).Cast<GameObject>().GetComponent<SpriteRenderer>().sprite;


        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.HandleAnimation))]
        public static class PlayerPhysicsHandleAnimationPatch
        {
            public static void Postfix(PlayerPhysics __instance) {
                try {
                    if (!__instance.myPlayer || !CustomHatViewDatas.TryGetValue(__instance.myPlayer.cosmetics.hat.Hat.ProductId, out var viewData))
                        return;

                    var currentAnimation = __instance.Animations.Animator.GetCurrentAnimation();

                    if (currentAnimation == __instance.Animations.group.ClimbUpAnim || currentAnimation == __instance.Animations.group.ClimbDownAnim)
                        return;

                    var hp = __instance.myPlayer.cosmetics.hat;

                    if (!hp || !hp.Hat)
                        return;                    
                }
                catch { }
            }
        }

        [HarmonyPatch(typeof(HatParent), nameof(HatParent.SetHat), typeof(int))]
        public class SetHatPatch
        {
            public static bool Prefix(HatParent __instance, int color) {
                if (!__instance.Hat.ProductId.StartsWith("lmj_")) return true;
                __instance.viewAsset = null;
                __instance.PopulateFromViewData();
                __instance.SetMaterialColor(color);
                return false;
            }
        }

        [HarmonyPatch(typeof(HatParent), nameof(HatParent.UpdateMaterial))]
        public class UpdateMaterialPatch
        {
            public static bool Prefix(HatParent __instance) {
                HatViewData asset;
                try {
                    HatViewData vanillaAsset = __instance.viewAsset.GetAsset();
                    return true;
                }
                catch {
                    try {
                        asset = CustomHatViewDatas[__instance.Hat.name];
                    }
                    catch {
                        return false;
                    }
                }
                if (asset.MatchPlayerColor) {
                    __instance.FrontLayer.sharedMaterial = MagicShader;
                    if (__instance.BackLayer) {
                        __instance.BackLayer.sharedMaterial = MagicShader;
                    }
                }
                else {
                    __instance.FrontLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.DefaultShader;
                    if (__instance.BackLayer) {
                        __instance.BackLayer.sharedMaterial = DestroyableSingleton<HatManager>.Instance.DefaultShader;
                    }
                }
                int colorId = __instance.matProperties.ColorId;
                PlayerMaterial.SetColors(colorId, __instance.FrontLayer);
                if (__instance.BackLayer) {
                    PlayerMaterial.SetColors(colorId, __instance.BackLayer);
                }
                __instance.FrontLayer.material.SetInt(PlayerMaterial.MaskLayer, __instance.matProperties.MaskLayer);
                if (__instance.BackLayer) {
                    __instance.BackLayer.material.SetInt(PlayerMaterial.MaskLayer, __instance.matProperties.MaskLayer);
                }
                PlayerMaterial.MaskType maskType = __instance.matProperties.MaskType;
                if (maskType == PlayerMaterial.MaskType.ScrollingUI) {
                    if (__instance.FrontLayer) {
                        __instance.FrontLayer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    }
                    if (__instance.BackLayer) {
                        __instance.BackLayer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                        return false;
                    }
                }
                else if (maskType == PlayerMaterial.MaskType.Exile) {
                    if (__instance.FrontLayer) {
                        __instance.FrontLayer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    }
                    if (__instance.BackLayer) {
                        __instance.BackLayer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                        return false;
                    }
                }
                else {
                    if (__instance.FrontLayer) {
                        __instance.FrontLayer.maskInteraction = SpriteMaskInteraction.None;
                    }
                    if (__instance.BackLayer) {
                        __instance.BackLayer.maskInteraction = SpriteMaskInteraction.None;
                    }
                }
                if (__instance.matProperties.MaskLayer <= 0) {
                    PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(__instance.FrontLayer, __instance.matProperties.IsLocalPlayer);
                    if (__instance.BackLayer) {
                        PlayerMaterial.SetMaskLayerBasedOnLocalPlayer(__instance.BackLayer, __instance.matProperties.IsLocalPlayer);
                    }
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(HatParent), nameof(HatParent.SetFloorAnim))]
        public class HatParentSetFloorAnimPatch
        {
            public static bool Prefix(HatParent __instance) {
                try {
                    HatViewData vanillaAsset = __instance.viewAsset.GetAsset();
                    return true;
                }
                catch { }
                HatViewData hatViewData = CustomHatViewDatas[__instance.Hat.name];
                __instance.BackLayer.enabled = false;
                __instance.FrontLayer.enabled = true;
                __instance.FrontLayer.sprite = hatViewData.FloorImage;
                return false;
            }
        }

        [HarmonyPatch(typeof(HatParent), nameof(HatParent.SetIdleAnim))]
        public class HatParentSetIdleAnimPatch
        {
            public static bool Prefix(HatParent __instance, int colorId) {
                if (!__instance.Hat) return false;
                if (!__instance.Hat.ProductId.StartsWith("lmj_")) return true;
               
                HatViewData hatViewData = CustomHatViewDatas[__instance.Hat.name];
                __instance.viewAsset = null;
                __instance.PopulateFromViewData();
                __instance.SetMaterialColor(colorId);
                return false;
            }
        }

        [HarmonyPatch(typeof(HatParent), nameof(HatParent.SetClimbAnim))]
        public class HatParentSetClimbAnimPatch
        {
            public static bool Prefix(HatParent __instance) {
                try {
                    HatViewData vanillaAsset = __instance.viewAsset.GetAsset();
                    return true;
                }
                catch { }

                HatViewData hatViewData = CustomHatViewDatas[__instance.Hat.name];
                if (!__instance.options.ShowForClimb) {
                    return false;
                }
                __instance.BackLayer.enabled = false;
                __instance.FrontLayer.enabled = true;
                __instance.FrontLayer.sprite = hatViewData.ClimbImage;
                return false;
            }
        }


        [HarmonyPatch(typeof(HatParent), nameof(HatParent.PopulateFromViewData))]
        public class PopulateFromHatViewDataPatch
        {
            public static bool Prefix(HatParent __instance) {
                try {
                    HatViewData vanillaAsset = __instance.viewAsset.GetAsset();
                    return true;
                }
                catch {
                    if (__instance.Hat && !CustomHatViewDatas.ContainsKey(__instance.Hat.name))
                        return true;
                }


                HatViewData asset = CustomHatViewDatas[__instance.Hat.name];

                if (!asset) {
                    return true;
                }
                __instance.UpdateMaterial();

                SpriteAnimNodeSync spriteAnimNodeSync = __instance.SpriteSyncNode ?? __instance.GetComponent<SpriteAnimNodeSync>();
                if (spriteAnimNodeSync) {
                    spriteAnimNodeSync.NodeId = (__instance.Hat.NoBounce ? 1 : 0);
                }
                if (__instance.Hat.InFront) {
                    __instance.BackLayer.enabled = false;
                    __instance.FrontLayer.enabled = true;
                    __instance.FrontLayer.sprite = asset.MainImage;
                }
                else if (asset.BackImage) {
                    __instance.BackLayer.enabled = true;
                    __instance.FrontLayer.enabled = true;
                    __instance.BackLayer.sprite = asset.BackImage;
                    __instance.FrontLayer.sprite = asset.MainImage;
                }
                else {
                    __instance.BackLayer.enabled = true;
                    __instance.FrontLayer.enabled = false;
                    __instance.FrontLayer.sprite = null;
                    __instance.BackLayer.sprite = asset.MainImage;
                }
                if (__instance.options.Initialized && __instance.HideHat()) {
                    __instance.FrontLayer.enabled = false;
                    __instance.BackLayer.enabled = false;
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(HatParent), nameof(HatParent.LateUpdate))]
        public static class HatParentLateUpdatePatch
        {
            public static bool Prefix(HatParent __instance) {
                if (!__instance.Parent || !__instance.Hat)
                    return false;

                HatViewData hatViewData;

                try {
                    hatViewData = __instance.viewAsset.GetAsset();
                    return true;
                }
                catch {
                    try {
                        CustomHatViewDatas.TryGetValue(__instance.Hat.ProductId, out hatViewData);
                    }
                    catch {
                        return false;
                    }
                }

                if (!hatViewData)
                    return false;

                if (__instance.FrontLayer.sprite != hatViewData.ClimbImage && __instance.FrontLayer.sprite != hatViewData.FloorImage) {
                    if ((__instance.Hat.InFront || hatViewData.BackImage) && hatViewData.LeftMainImage)
                        __instance.FrontLayer.sprite = __instance.Parent.flipX ? hatViewData.LeftMainImage : hatViewData.MainImage;

                    if (hatViewData.BackImage && hatViewData.LeftBackImage) {
                        __instance.BackLayer.sprite = __instance.Parent.flipX ? hatViewData.LeftBackImage : hatViewData.BackImage;
                        return false;
                    }

                    if (!hatViewData.BackImage && !__instance.Hat.InFront && hatViewData.LeftMainImage) {
                        __instance.BackLayer.sprite = __instance.Parent.flipX ? hatViewData.LeftMainImage : hatViewData.MainImage;
                        return false;
                    }
                }
                else if (__instance.FrontLayer.sprite == hatViewData.ClimbImage || __instance.FrontLayer.sprite == hatViewData.LeftClimbImage) {
                    __instance.SpriteSyncNode ??= __instance.GetComponent<SpriteAnimNodeSync>();

                    if (__instance.SpriteSyncNode)
                        __instance.SpriteSyncNode.NodeId = 0;
                }

                return false;
            }
        }
    }
}