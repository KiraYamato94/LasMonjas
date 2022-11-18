using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using UnityEngine;
using System.Linq;

// Adapted from https://github.com/xxomega77xx/HatPack

namespace LasMonjas.Core
{
    class CustomHats
    {
        public static Material MagicShader;

        public struct AuthorData
        {
            public string AuthorName;
            public string HatName;
            public string FloorHatName;
            public string ClimbHatName;
            public string LeftImageName;
            public bool NoBounce;
            public bool altShader;
        }

        public static List<AuthorData> authorDatas = new List<AuthorData>()
        {
            new AuthorData {AuthorName = "Allul", HatName = "Monja", NoBounce = true},
            new AuthorData {AuthorName = "Allul", HatName = "Minion Monja", FloorHatName ="Minion Monja Climb", ClimbHatName = "Minion Monja Climb", LeftImageName = "Minion Monja", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Cursed Monja", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Abombg Man", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Among Ass", NoBounce = false, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Time To Duel", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Medusa", NoBounce = false},
            new AuthorData {AuthorName = "Sensei", HatName = "Mega Hat", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Egyptian", NoBounce = true },
            new AuthorData {AuthorName = "Sensei", HatName = "Joker", NoBounce = true },
            new AuthorData {AuthorName = "Sensei", HatName = "SrCobra", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Dinoseto", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Super Red Sus", NoBounce = false},
            new AuthorData {AuthorName = "Sensei", HatName = "Super Green Sus", NoBounce = false},
            new AuthorData {AuthorName = "Sensei", HatName = "Super Yellow Sus", NoBounce = false},
            new AuthorData {AuthorName = "Sensei", HatName = "Super Purple Sus", NoBounce = false},
            new AuthorData {AuthorName = "Sensei", HatName = "Chadsito", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Scars", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Sus Man", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Take It Easy", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Moon Face", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Pepper Carrot", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Battle Armor", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Space Captain", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Dontaegamez", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Blocky", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Glitch", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Boot", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Monja Cloth", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Cell", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Ghost", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Goodbye", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Muaresito Joy", NoBounce = false},
            new AuthorData {AuthorName = "Muaresito", HatName = "Avatar", NoBounce = false, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Scallop Walker", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Unknown Race", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Kill Palex", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Olmaito", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Susking", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Tree Brows", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Susboy", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Xabasus", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Bee", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Bounty Hunter", NoBounce = false},
            new AuthorData {AuthorName = "Muaresito", HatName = "King Skull", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Josefa", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Monjart", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Susnic", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Homunculus", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "2nd Actor Hair", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "4th Anniversary", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Exsusdia", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "True Exsusdia", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Worker Hat", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Raul", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Chainsus Man", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "4M0NJ-4S Tank", NoBounce = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Too much tasks", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Octosus", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Sustrio", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Penguin", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "World Destroyer", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Amazing Robot", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Fourze", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Zargothrax", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Chaos Wizard", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Robot Armor", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Hige", HatName = "Punsus", NoBounce = true},
            new AuthorData {AuthorName = "IceCreamGuy", HatName = "Ice Cream Man", NoBounce = false},
            new AuthorData {AuthorName = "IceCreamGuy", HatName = "Devious Bling", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "IceCreamGuy", HatName = "Hungry Hat", NoBounce = true},
            new AuthorData {AuthorName = "Sen", HatName = "Artist", FloorHatName ="Artist_climb", ClimbHatName = "Artist_climb", LeftImageName = "Artist", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Bubbles", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Black Cat", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "White Cat", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Clown", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Raccoon", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Impostor Raccoon", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Periodt", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "GD", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Card", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Flower Crown", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Good Noodle", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Long Wiggle", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Neon Devil Horns", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Purple Halo", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Sword", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "uwu", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Shark", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "All Ears", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Babies", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Beans", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Cat", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Dress", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Ghost Hat", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Rainbow", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Shark Plush", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Top Hat", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Umbrella", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "Axolotl", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "lotty", HatName = "(je)Sushi", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Angry Chicken", NoBounce = false},
            new AuthorData {AuthorName = "lotty", HatName = "Best Friend", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Bug Girl", NoBounce = true},
            new AuthorData {AuthorName = "lotty", HatName = "Cat Thief", NoBounce = true},
            new AuthorData {AuthorName = "ERIKHAPPY", HatName = "Blue Scarf", NoBounce = true},
            new AuthorData {AuthorName = "ERIKHAPPY", HatName = "Egg", NoBounce = false},
            new AuthorData {AuthorName = "Jesushi", HatName = "Jester", NoBounce = true},
            new AuthorData {AuthorName = "Jesushi", HatName = "Crown", NoBounce = true},
            new AuthorData {AuthorName = "Booman", HatName = "Sniper", NoBounce = true},
            new AuthorData {AuthorName = "Boa", HatName = "Cat Princess", FloorHatName ="Cat Princess_climb", ClimbHatName = "Cat Princess_climb", LeftImageName = "Cat Princess", NoBounce = true},
        };

        internal static Dictionary<int, AuthorData> IdToData = new Dictionary<int, AuthorData>();

        private static bool _customHatsLoaded = false;
        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetHatById))]
        public static class AddCustomHats
        {
            public static void Postfix(HatManager __instance) {

                if (!_customHatsLoaded) {
                    var allHats = __instance.allHats.ToList();

                    foreach (var data in authorDatas) {
                        HatID++;

                        if (data.FloorHatName != null && data.ClimbHatName != null && data.LeftImageName != null) {
                            if (data.NoBounce) {
                                if (data.altShader == true) {
                                    allHats.Add(CreateHat(GetSprite(data.HatName), data.AuthorName, GetSprite(data.ClimbHatName), GetSprite(data.FloorHatName), GetSprite(data.LeftImageName), true, true));
                                }
                                else {
                                    allHats.Add(CreateHat(GetSprite(data.HatName), data.AuthorName, GetSprite(data.ClimbHatName), GetSprite(data.FloorHatName), GetSprite(data.LeftImageName), true, false));
                                }
                            }
                            else {
                                if (data.altShader == true) {
                                    allHats.Add(CreateHat(GetSprite(data.HatName), data.AuthorName, GetSprite(data.ClimbHatName), GetSprite(data.FloorHatName), GetSprite(data.LeftImageName), false, true));
                                }
                                else {
                                    allHats.Add(CreateHat(GetSprite(data.HatName), data.AuthorName, GetSprite(data.ClimbHatName), GetSprite(data.FloorHatName), GetSprite(data.LeftImageName)));
                                }
                            }

                        }
                        else {
                            if (data.NoBounce) {
                                if (data.altShader == true) {
                                    allHats.Add(CreateHat(GetSprite(data.HatName), data.AuthorName, null, null, null, true, true));
                                }
                                else {
                                    allHats.Add(CreateHat(GetSprite(data.HatName), data.AuthorName, null, null, null, true, false));
                                }
                            }
                            else {
                                if (data.altShader == true) {
                                    allHats.Add(CreateHat(GetSprite(data.HatName), data.AuthorName, null, null, null, false, true));
                                }
                                else {
                                    allHats.Add(CreateHat(GetSprite(data.HatName), data.AuthorName, null, null, null, false, false));
                                }
                            }
                            
                        }
                        IdToData.Add(HatManager.Instance.allHats.Count + HatID, data);

                        _customHatsLoaded = true;
                    }
                    _customHatsLoaded = true;
                    __instance.allHats = allHats.ToArray();
                }
            }

            public static Sprite GetSprite(string name)
                => AssetLoader.LoadHatAsset(name).Cast<GameObject>().GetComponent<SpriteRenderer>().sprite;

            public static int HatID = 0;
            /// <summary>
            /// Creates hat based on specified values
            /// </summary>
            /// <param name="sprite"></param>
            /// <param name="author"></param>
            /// <param name="climb"></param>
            /// <param name="floor"></param>
            /// <param name="leftimage"></param>
            /// <param name="bounce"></param>
            /// <param name="altshader"></param>
            /// <returns>HatData</returns>
            private static HatData CreateHat(Sprite sprite, string author, Sprite climb = null, Sprite floor = null, Sprite leftimage = null, bool bounce = false, bool altshader = false) {
				//Borrowed from Other Roles to get hats alt shaders to work
                if (MagicShader == null) {
                    Material hatShader = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;
                    MagicShader = hatShader;
                }

                HatData newHat = ScriptableObject.CreateInstance<HatData>();
                newHat.hatViewData.viewData = ScriptableObject.CreateInstance<HatViewData>();
                newHat.name = $"{sprite.name} (by {author})";
                newHat.hatViewData.viewData.MainImage = sprite;
                newHat.ProductId = "hat_" + sprite.name.Replace(' ', '_');
                newHat.BundleId = "hat_" + sprite.name.Replace(' ', '_');
                newHat.displayOrder = 99 + HatID;
                newHat.InFront = true;
                newHat.NoBounce = bounce;
                newHat.hatViewData.viewData.FloorImage = floor;
                newHat.hatViewData.viewData.ClimbImage = climb;
                newHat.Free = true;
                newHat.hatViewData.viewData.LeftMainImage = leftimage;
                newHat.ChipOffset = new Vector2(-0.1f, 0.4f);
                if (altshader == true) { newHat.hatViewData.viewData.AltShader = MagicShader; }

                return newHat;
            }
        }
    }
}