using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using static LasMonjas.HudManagerStartPatch;
using Hazel;
using LasMonjas.Patches;

namespace LasMonjas.Objects
{
    class MonjaCuloDio
    {
        public static List<MonjaCuloDio> monjaculodios = new List<MonjaCuloDio>();
        private static Sprite monjasprite; 
        private static Sprite culosprite; 
        private static Sprite diosprite;
        private Color color;
        private GameObject monjaculodio;
        private SpriteRenderer spriteRenderer;

        public static Sprite getMonjaSprite() {
            if (monjasprite) return monjasprite;
            monjasprite = CustomMain.customAssets.monjashow.GetComponent<SpriteRenderer>().sprite;
            return monjasprite;
        }
        public static Sprite getCuloSprite() {
            if (culosprite) return culosprite;
            culosprite = CustomMain.customAssets.culoshow.GetComponent<SpriteRenderer>().sprite;
            return culosprite;
        }
        public static Sprite getDioSprite() {
            if (diosprite) return diosprite;
            diosprite = CustomMain.customAssets.dioshow.GetComponent<SpriteRenderer>().sprite;
            return diosprite;
        }

        public MonjaCuloDio(float duration, PlayerControl player, int type) {

            this.color = new Color(1f, 1f, 1f, 1f);

            monjaculodio = new GameObject("MonjaCuloDio");
            monjaculodio.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y - 0.5f, player.transform.position.z - 1f);
            monjaculodio.transform.position = position;
            monjaculodio.transform.localPosition = position;
            monjaculodio.transform.SetParent(player.transform);

            spriteRenderer = monjaculodio.AddComponent<SpriteRenderer>();
            switch (type) {
                case 1:
                    spriteRenderer.sprite = getMonjaSprite();
                    break;
                case 2:
                    spriteRenderer.sprite = getCuloSprite();
                    break;
                case 3:
                    spriteRenderer.sprite = getDioSprite();
                    break;
            }           
            spriteRenderer.color = color;

            monjaculodio.SetActive(true);
            monjaculodios.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {

                if (p == 1f && monjaculodio != null) {
                    UnityEngine.Object.Destroy(monjaculodio);
                    monjaculodios.Remove(this);
                }

            })));

        }
    }
}
