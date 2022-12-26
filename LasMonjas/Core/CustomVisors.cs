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
    class CustomVisors
    {
        public static Material MagicShader;

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
        };

        internal static Dictionary<int, AuthorData> IdToData = new Dictionary<int, AuthorData>();

        private static bool _customVisorLoaded = false;
        [HarmonyPatch(typeof(HatManager), nameof(HatManager.GetVisorById))]
        public static class AddCustomVisors
        {

            public static void Postfix(HatManager __instance) {

                if (!_customVisorLoaded) {
                    var allVisors = __instance.allVisors.ToList(); ;

                    foreach (var data in authorDatas) {
                        VisorID++;

                        if (data.altShader) {
                            allVisors.Add(CreateVisor(GetSprite(data.VisorName), data.AuthorName, true));
                        }
                        else {
                            allVisors.Add(CreateVisor(GetSprite(data.VisorName), data.AuthorName, false));
                        }

                        IdToData.Add(HatManager.Instance.allVisors.Count + VisorID, data);

                        _customVisorLoaded = true;
                    }
                    _customVisorLoaded = true;
                    __instance.allVisors = allVisors.ToArray();
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
            private static VisorData CreateVisor(Sprite sprite, string author, bool altshader) {
                //Borrowed from Other Roles to get hats alt shaders to work
                if (MagicShader == null) {
                    Material visorShader = DestroyableSingleton<HatManager>.Instance.PlayerMaterial;
                    MagicShader = visorShader;
                }

                VisorData newVisor = ScriptableObject.CreateInstance<VisorData>();
                newVisor.viewData.viewData = ScriptableObject.CreateInstance<VisorViewData>();
                newVisor.name = $"{sprite.name} (by {author})";
                newVisor.viewData.viewData.IdleFrame = sprite;
                newVisor.ProductId = "visor_" + sprite.name.Replace(' ', '_');
                newVisor.BundleId = "visor_" + sprite.name.Replace(' ', '_');
                newVisor.displayOrder = 99 + VisorID;
                newVisor.Free = true;
                newVisor.ChipOffset = new Vector2(0f, 0.2f);
                if (altshader == true) { newVisor.viewData.viewData.AltShader = MagicShader; }

                return newVisor;
            }
        }
    }
}