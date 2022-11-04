using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using UnityEngine;
using System.Linq;

namespace LasMonjas.Core
{
    class CustomNamePlates
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
            new AuthorData {AuthorName = "Blocky", NamePlateName = "Submerged"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "What the cow"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "Bluescreen"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "Lot of bodies"},
            new AuthorData {AuthorName = "Blocky", NamePlateName = "RIP"},
            new AuthorData {AuthorName = "AD", NamePlateName = "Jailed"},
        };

        internal static Dictionary<int, AuthorData> IdToData = new Dictionary<int, AuthorData>();

        private static bool _customNamePlatesLoaded = false;
        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetNamePlateById))]
        public static class AddCustomNamePlates
        {

            public static void Postfix(HatManager __instance) {
                if (!_customNamePlatesLoaded) {
                    var allPlates = __instance.allNamePlates.ToList();

                    foreach (var data in authorDatas) {
                        NamePlateID++;

                        allPlates.Add(CreateNamePlate(GetSprite(data.NamePlateName), data.AuthorName));

                        IdToData.Add(HatManager.Instance.allNamePlates.Count + NamePlateID, data);

                        _customNamePlatesLoaded = true;
                    }
                    _customNamePlatesLoaded = true;
                    __instance.allNamePlates = allPlates.ToArray();
                }
            }

            public static Sprite GetSprite(string name)
                => AssetLoader.LoadNamePlateAsset(name).Cast<GameObject>().GetComponent<SpriteRenderer>().sprite;

            public static int NamePlateID = 0;
            /// <summary>
            /// Creates hat based on specified values
            /// </summary>
            /// <param name="sprite"></param>
            /// <param name="author"></param>
            /// <returns>NamePlateData</returns>
            private static NamePlateData CreateNamePlate(Sprite sprite, string author) {

                NamePlateData newPlate = ScriptableObject.CreateInstance<NamePlateData>();
                newPlate.viewData.viewData = ScriptableObject.CreateInstance<NamePlateViewData>();
                newPlate.name = $"{sprite.name} (by {author})";
                newPlate.viewData.viewData.Image = sprite;
                newPlate.ProductId = "nameplate_" + sprite.name.Replace(' ', '_');
                newPlate.BundleId = "nameplate_" + sprite.name.Replace(' ', '_');
                newPlate.displayOrder = 99 + NamePlateID;
                newPlate.Free = true;
                newPlate.ChipOffset = new Vector2(0f, 0.2f);

                return newPlate;
            }
        }
    }
}