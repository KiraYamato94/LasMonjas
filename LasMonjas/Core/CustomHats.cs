using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using static Rewired.Controller;
using UnityEngine.AddressableAssets;
using PowerTools;

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
        public class HatExtension
        {
            public string author { get; set; }            
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
            new AuthorData {AuthorName = "Sensei", HatName = "Majin Sus", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "El Mauro", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Angry Dontae", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Blocky 16bits", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Fascinante", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Pingas", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Suavemente", NoBounce = true},
            new AuthorData {AuthorName = "Sensei", HatName = "Suscolo", NoBounce = true, altShader = true},
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
            new AuthorData {AuthorName = "Muaresito", HatName = "Happy 1st Birthday Monjas", NoBounce = false},
            new AuthorData {AuthorName = "Muaresito", HatName = "1st Monjiversario", NoBounce = false, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Carmina Vacaloura", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Bunny Hood", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Sussykill", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Stuffwell", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Muaresito", HatName = "Sussybara", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Xago", HatName = "World Destroyer", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Amazing Robot", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Fourze", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Zargothrax", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Chaos Wizard", NoBounce = true},
            new AuthorData {AuthorName = "Xago", HatName = "Robot Armor", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Xago", HatName = "Canon Robot", NoBounce = true},
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
            new AuthorData {AuthorName = "Booman", HatName = "Rocketman", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Boa", HatName = "Cat Princess", FloorHatName ="Cat Princess_climb", ClimbHatName = "Cat Princess_climb", LeftImageName = "Cat Princess", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "3rd Eye", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Candles", NoBounce = false},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Double Visor", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Green Hat", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Idea", NoBounce = false},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Sheep", NoBounce = false},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Sus Guy", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "UFO", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Electric Rat", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Royal Blonde Hair", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Blue Hat", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Cloak", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Empty Charge", NoBounce = false},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Charging", NoBounce = false},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Full Charge", NoBounce = false},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Funny Ghost", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Mushrooms", NoBounce = false},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Pink Flower", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Purple Animatronic", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Watermelon", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Pet Cat", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Ganso", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Ninja", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Susnana", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Ghostly", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Suspucha", NoBounce = true},
            new AuthorData {AuthorName = "Xeno<33", HatName = "Kitty Hat", NoBounce = true},
            new AuthorData {AuthorName = "Dontae", HatName = "Tea Cup", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Headphone gamer", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Heart Tiera", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Rubber Ring", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Shsusrek", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Strawberry", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Sustalian", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Love You", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Ponycorn", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Fox", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Mantis", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Magic Hat", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Nurse", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Sustich", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Dog", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Bunny", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Cactus", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Lasus", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Susken", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Cat Face", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Fluffy Scarf", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Flower Lady", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Halo", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "On Fire", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Frog Hat", NoBounce = true},
            new AuthorData {AuthorName = "Nyxx", HatName = "Magician Hat", NoBounce = true},
            new AuthorData {AuthorName = "Ravengirl", HatName = "Flag", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sonrio", HatName = "Flaming", NoBounce = true},
            new AuthorData {AuthorName = "Sonrio", HatName = "Freezing", NoBounce = true},
            new AuthorData {AuthorName = "Sonrio", HatName = "Night Friday", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sonrio", HatName = "Puppetist", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Sonrio", HatName = "Calling All Crewmates", NoBounce = true},
            new AuthorData {AuthorName = "Sonrio", HatName = "Fighter", NoBounce = true, altShader = true},
            new AuthorData {AuthorName = "Dr Blockhead", HatName = "Got Any Grapes", NoBounce = true},
            new AuthorData {AuthorName = "Dr Blockhead", HatName = "Bucket", NoBounce = true},
        };

        internal static Dictionary<int, AuthorData> IdToData = new Dictionary<int, AuthorData>();
        public static Dictionary<string, HatExtension> CustomHatRegistry = new Dictionary<string, HatExtension>();
        public static Dictionary<string, HatViewData> CustomHatViewDatas = new Dictionary<string, HatViewData>();

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

            public static int HatID = 0;
            
            private static HatData CreateHat(Sprite sprite, string author, Sprite climb = null, Sprite floor = null, Sprite leftimage = null, bool bounce = false, bool altshader = false) {
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
                newHat.ProductId = "hat_" + sprite.name.Replace(' ', '_');
                newHat.displayOrder = 99 + HatID;
                newHat.InFront = true;
                newHat.NoBounce = bounce;                
                newHat.Free = true;
                newHat.ChipOffset = new Vector2(-0.1f, 0.4f);
                if (altshader == true) { viewdata.MatchPlayerColor = true; }
                HatExtension extend = new HatExtension(); 
                CustomHatRegistry.Add(newHat.name, extend);
                CustomHatViewDatas.Add(newHat.name, viewdata);
                var assetRef = new AssetReference(viewdata.Pointer);

                newHat.ViewDataRef = assetRef;
                newHat.CreateAddressableAsset();
                return newHat;
            }
        }

        [HarmonyPatch(typeof(HatsTab), nameof(HatsTab.OnEnable))]
        public static class EnableSprite
        {
            public static void Postfix() {
                GameObject innerHats = GameObject.Find("HatsGroup").transform.GetChild(1).transform.GetChild(0).gameObject;
                int hat = 0;
                for (int i = 1; i < innerHats.transform.GetChildCount(); i++) {
                    if (innerHats.transform.GetChild(i).transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == null) {
                        innerHats.transform.GetChild(i).transform.GetChild(2).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GetSprite(authorDatas[hat].HatName);
                        hat += 1;
                    }
                }
            }
        }
        public static Sprite GetSprite(string name)
                => AssetLoader.LoadHatAsset(name).Cast<GameObject>().GetComponent<SpriteRenderer>().sprite;


        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.HandleAnimation))]
        private static class PlayerPhysicsHandleAnimationPatch
        {
            private static void Postfix(PlayerPhysics __instance) {
                if (!CustomHatViewDatas.ContainsKey(__instance.myPlayer.cosmetics.hat.Hat.name)) return;
                AnimationClip currentAnimation = __instance.Animations.Animator.GetCurrentAnimation();
                if (currentAnimation == __instance.Animations.group.ClimbUpAnim || currentAnimation == __instance.Animations.group.ClimbDownAnim) return;
                HatParent hp = __instance.myPlayer.cosmetics.hat;
                if (hp == null || hp.Hat == null) return;                
            }
        }

        [HarmonyPatch(typeof(HatParent), nameof(HatParent.SetHat), typeof(int))]
        public class SetHatPatch
        {
            public static bool Prefix(HatParent __instance, int color) {
                if (!CustomHatRegistry.ContainsKey(__instance.Hat.name)) return true;
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
                if (!CustomHatRegistry.ContainsKey(__instance.Hat.name))
                    return true;
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
    }
}