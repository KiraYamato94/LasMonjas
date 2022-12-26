using LasMonjas.Patches;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LasMonjas.Objects
{
    class BattleRoyaleShoot {
        private static List<BattleRoyaleShoot> shoots = new List<BattleRoyaleShoot>();
        private static Sprite sprite;
        private Color color;
        private GameObject shoot;
        private SpriteRenderer spriteRenderer;
        private Vector3 position;

        public static Sprite getShootSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.royaleShoot.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public BattleRoyaleShoot(PlayerControl player, int whichColor, float angle) {

            switch (whichColor) {
                case 0:
                    this.color = Palette.PlayerColors[(int)player.Data.DefaultOutfit.ColorId];
                    break;
                case 1:
                    this.color = Palette.PlayerColors[11];
                    break;
                case 2:
                    this.color = Palette.PlayerColors[13];
                    break;
                case 3:
                    this.color = Palette.PlayerColors[15];
                    break;
            }

            shoot = new GameObject("BattleRoyaleShoot");
            shoot.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                position = new Vector3(player.transform.position.x, player.transform.position.y, -0.5f);
            }
            else {
                position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1f);
            }
            shoot.transform.position = position;
            shoot.transform.localPosition = position;
            shoot.transform.SetParent(player.transform.parent);
            shoot.transform.eulerAngles = new Vector3(0f, 0f, (float)(angle * 360f / Math.PI / 2f));

            spriteRenderer = shoot.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getShootSprite();
            spriteRenderer.color = color;

            shoots.Add(this);
            shoot.SetActive(true);

            HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) => {

                if (p == 1f && shoot != null) {
                    UnityEngine.Object.Destroy(shoot);
                    shoots.Remove(this);
                }
            })));
        }
        public static void clearBattleRoyaleShoots() {
            shoots = new List<BattleRoyaleShoot>();
        }
    }
}