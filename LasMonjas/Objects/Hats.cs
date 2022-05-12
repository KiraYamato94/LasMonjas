using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using LasMonjas.Patches;

namespace LasMonjas.Objects {

    public class Hats {
        public static System.Collections.Generic.List<Hats> AllHats = new System.Collections.Generic.List<Hats>();
        public static int HatLimit = 3;
        public static bool hatsConvertedToVents = false;
        public static Sprite[] hatAnimationSprites = new Sprite[18];
        private Vector3 position;

        public static Sprite getHatAnimationSprite(int index) {
            if (hatAnimationSprites == null || hatAnimationSprites.Length == 0) return null;
            index = Mathf.Clamp(index, 0, hatAnimationSprites.Length - 1);
            if (hatAnimationSprites[index] == null)
                hatAnimationSprites[index] = (Helpers.loadSpriteFromResources($"LasMonjas.Images.IlusionistAnimation.ilusionist_hat_00{(index + 1):00}.png", 175f));
            return hatAnimationSprites[index];
        }

        public static void startAnimation(int ventId) {
            Hats hat = AllHats.FirstOrDefault((x) => x?.vent != null && x.vent.Id == ventId);
            if (hat == null) return;
            Vent vent = hat.vent;

            HudManager.Instance.StartCoroutine(Effects.Lerp(0.6f, new Action<float>((p) => {
                if (vent != null && vent.myRend != null) {
                    vent.myRend.sprite = getHatAnimationSprite((int)(p * hatAnimationSprites.Length));
                    if (p == 1f) vent.myRend.sprite = getHatAnimationSprite(0);
                }
            })));
        }

        private GameObject gameObject;
        public Vent vent;

        public Hats(Vector2 p) {
            gameObject = new GameObject("Hat");
            gameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover); 
            if (PlayerControl.GameOptions.MapId == 5) {
                position = new Vector3(p.x, p.y, -0.5f);
            }
            else {
                position = new Vector3(p.x, p.y, PlayerControl.LocalPlayer.transform.position.z + 1f);
            }
            position += (Vector3)PlayerControl.LocalPlayer.Collider.offset; 
            
            gameObject.transform.position = position;
            var hatRenderer = gameObject.AddComponent<SpriteRenderer>();
            hatRenderer.sprite = getHatAnimationSprite(0);

            
            var referenceVent = UnityEngine.Object.FindObjectOfType<Vent>();
            vent = UnityEngine.Object.Instantiate<Vent>(referenceVent);
            vent.gameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover); 
            vent.transform.position = gameObject.transform.position;
            vent.Left = null;
            vent.Right = null;
            vent.Center = null;
            vent.EnterVentAnim = null;
            vent.ExitVentAnim = null;
            vent.Offset = new Vector3(0f, 0.25f, 0f);
            vent.GetComponent<PowerTools.SpriteAnim>()?.Stop();
            vent.Id = ShipStatus.Instance.AllVents.Select(x => x.Id).Max() + 1;
            var ventRenderer = vent.GetComponent<SpriteRenderer>();
            ventRenderer.sprite = getHatAnimationSprite(0);
            vent.myRend = ventRenderer;
            var allVentsList = ShipStatus.Instance.AllVents.ToList();
            allVentsList.Add(vent);
            ShipStatus.Instance.AllVents = allVentsList.ToArray();
            vent.gameObject.SetActive(false);
            vent.name = "Hat_" + vent.Id;

            
            var playerIsIlusionist = PlayerControl.LocalPlayer == Ilusionist.ilusionist;
            gameObject.SetActive(playerIsIlusionist);

            AllHats.Add(this);

            if (AllHats.Count == HatLimit)
                convertToVents();
        }

        public static void UpdateStates() {
            if (hatsConvertedToVents == true) return;
            foreach (var hat in AllHats) {
                var playerIsIlusionist = PlayerControl.LocalPlayer == Ilusionist.ilusionist;
                hat.gameObject.SetActive(playerIsIlusionist);
            }
        }

        public void convertToVent() {
            gameObject.SetActive(false);
            vent.gameObject.SetActive(true);
            return;
        }

        public static void convertToVents() {
            foreach (var hat in AllHats) {
                hat.convertToVent();
            }
            connectVents();
            hatsConvertedToVents = true;
            return;
        }

        public static bool hasHatLimitReached() {
            return (AllHats.Count >= HatLimit);
        }

        private static void connectVents() {
            for (var i = 0; i < AllHats.Count - 1; i++) {
                var a = AllHats[i];
                var b = AllHats[i + 1];
                a.vent.Right = b.vent;
                b.vent.Left = a.vent;
            }
            
            AllHats.First().vent.Left = AllHats.Last().vent;
            AllHats.Last().vent.Right = AllHats.First().vent;
        }

        public static void clearHats() {
            hatsConvertedToVents = false;
            AllHats = new List<Hats>();
        }

    }

}