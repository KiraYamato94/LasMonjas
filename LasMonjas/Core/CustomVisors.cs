using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using BepInEx.Logging;
using UnityEngine;

namespace LasMonjas.Core
{
    class CustomVisors
    {

        public struct AuthorData
        {
            public string AuthorName;
            public string VisorName;
        }

        public static List<AuthorData> authorDatas = new List<AuthorData>()
        {
            new AuthorData {AuthorName = "Sensei", VisorName = "Alien"},
            new AuthorData {AuthorName = "Sensei", VisorName = "Fortune Teller"},
            new AuthorData {AuthorName = "Sensei", VisorName = "Over 9 Sus"},
            new AuthorData {AuthorName = "Sensei", VisorName = "PC Error"},
            new AuthorData {AuthorName = "Sensei", VisorName = "The Eye"},
            new AuthorData {AuthorName = "Sensei", VisorName = "TV Error"},
            new AuthorData {AuthorName = "Sensei", VisorName = "Visor Cleaner"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Muaresito Joy"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Olmaito"},
            new AuthorData {AuthorName = "Muaresito", VisorName = "Impostor Bros"},
            new AuthorData {AuthorName = "IceCreamGuy", VisorName = "Mungus"},
         };

        internal static Dictionary<int, AuthorData> IdToData = new Dictionary<int, AuthorData>();

        private static bool _customVisorLoaded = false;
        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetVisorById))]
        public static class AddCustomVisors
        {

            public static void Postfix(HatManager __instance) {

                if (!_customVisorLoaded) {
                    var allVisors = __instance.allVisors;

                    foreach (var data in authorDatas) {
                        VisorID++;

                        allVisors.Add(CreateVisor(GetSprite(data.VisorName), data.AuthorName));

                        IdToData.Add(HatManager.Instance.allVisors.Count - 1, data);

                        _customVisorLoaded = true;
                    }
                    _customVisorLoaded = true;
                }
            }

            public static Sprite GetSprite(string name)
                => AssetLoader.LoadVisorsAsset(name).Cast<GameObject>().GetComponent<SpriteRenderer>().sprite;

            public static int VisorID = 0;
            /// <summary>
            /// Creates hat based on specified values
            /// </summary>
            /// <param name="sprite"></param>
            /// <param name="author"></param>
            /// <returns>VisorData</returns>
            private static VisorData CreateVisor(Sprite sprite, string author) {

                VisorData newVisor = ScriptableObject.CreateInstance<VisorData>();
                newVisor.viewData.viewData = ScriptableObject.CreateInstance<VisorViewData>();
                newVisor.name = $"{sprite.name} (by {author})";
                newVisor.viewData.viewData.IdleFrame = sprite;
                newVisor.ProductId = "visor_" + sprite.name.Replace(' ', '_');
                newVisor.BundleId = "visor_" + sprite.name.Replace(' ', '_');
                newVisor.displayOrder = 99 + VisorID;
                newVisor.Free = true;
                newVisor.ChipOffset = new Vector2(0f, 0.2f);

                return newVisor;
            }
        }
    }
}