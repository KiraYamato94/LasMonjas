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
    class RockPaperScissors
    {
        public static List<RockPaperScissors> rockpaperscissors = new List<RockPaperScissors>();
        private static Sprite rocksprite; 
        private static Sprite papersprite; 
        private static Sprite scissorsprite;
        private Color color;
        private GameObject rockpaperscissor;
        private SpriteRenderer spriteRenderer;

        public static Sprite getRockSprite() {
            if (rocksprite) return rocksprite;
            rocksprite = CustomMain.customAssets.challengerRock.GetComponent<SpriteRenderer>().sprite;
            return rocksprite;
        }
        public static Sprite getPaperSprite() {
            if (papersprite) return papersprite;
            papersprite = CustomMain.customAssets.challengerPaper.GetComponent<SpriteRenderer>().sprite;
            return papersprite;
        }
        public static Sprite getScissorsSprite() {
            if (scissorsprite) return scissorsprite;
            scissorsprite = CustomMain.customAssets.challengerScissors.GetComponent<SpriteRenderer>().sprite;
            return scissorsprite;
        }

        public RockPaperScissors(float duration, PlayerControl player, int type) {

            this.color = new Color(1f, 1f, 1f, 1f);

            rockpaperscissor = new GameObject("RockPaperScissors");
            rockpaperscissor.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z - 1f);
            rockpaperscissor.transform.position = position;
            rockpaperscissor.transform.localPosition = position;
            rockpaperscissor.transform.SetParent(player.transform);

            spriteRenderer = rockpaperscissor.AddComponent<SpriteRenderer>();
            switch (type) {
                case 1:
                    spriteRenderer.sprite = getRockSprite();
                    break;
                case 2:
                    spriteRenderer.sprite = getPaperSprite();
                    break;
                case 3:
                    spriteRenderer.sprite = getScissorsSprite();
                    break;
            }           
            spriteRenderer.color = color;

            rockpaperscissor.SetActive(true);
            rockpaperscissors.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {

                if (p == 1f && rockpaperscissor != null) {
                    UnityEngine.Object.Destroy(rockpaperscissor);
                    rockpaperscissors.Remove(this);
                }

            })));

        }
    }
}
