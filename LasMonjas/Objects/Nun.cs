using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using LasMonjas.Patches;

namespace LasMonjas.Objects {
    class Nun {
        public static List<Nun> nuns = new List<Nun>();

        public GameObject nun;
        private GameObject background;
        private Vector3 position;

        private static Sprite nunSprite;
        public static Sprite getNunSprite() {
            if (nunSprite) return nunSprite;
            nunSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.DemonNun.png", 300f);
            return nunSprite;
        }

        private static Sprite backgroundSprite;
        public static Sprite getBackgroundSprite() {
            if (backgroundSprite) return backgroundSprite;
            backgroundSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.DemonNunBackground.png", 60f);
            return backgroundSprite;
        }

        public Nun(Vector2 p) {
            nun = new GameObject("Nun");
            nun.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover); 
            background = new GameObject("Background");
            background.transform.SetParent(nun.transform);
            if (PlayerControl.GameOptions.MapId == 5) {
                position = new Vector3(p.x, p.y, -0.5f);
            }
            else {
                position = new Vector3(p.x, p.y, 0.1f);
            }
            nun.transform.position = position;
            nun.transform.localPosition = position;
            background.transform.localPosition = new Vector3(0 , 0, 0.2f); 

            var nunRenderer = nun.AddComponent<SpriteRenderer>();
            nunRenderer.sprite = getNunSprite();
            var backgroundRenderer = background.AddComponent<SpriteRenderer>();
            backgroundRenderer.sprite = getBackgroundSprite();


            nun.SetActive(true);
            nuns.Add(this);
        }

        public static void clearNuns() {
            nuns = new List<Nun>();
        }

        public static void UpdateAll() {
            foreach (Nun nun in nuns) {
                if (nun != null)
                    nun.Update();
            }
        }

        public void Update() {
            if (background != null)
                background.transform.Rotate(Vector3.forward * 6 * Time.fixedDeltaTime);
        }
    }
}