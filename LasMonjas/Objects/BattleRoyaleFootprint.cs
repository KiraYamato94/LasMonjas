using LasMonjas.Patches;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LasMonjas.Objects
{
    class BattleRoyaleFootprint {
        private static List<BattleRoyaleFootprint> footprints = new List<BattleRoyaleFootprint>();
        private static Sprite sprite;
        private Color color;
        private GameObject footprint;
        private SpriteRenderer spriteRenderer;
        private Vector3 position;

        public static Sprite getFootprintSprite() {
            if (sprite) return sprite;
            sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.BattleroyaleFootprint.png", 100f);
            return sprite;
        }

        public BattleRoyaleFootprint(PlayerControl player, int whichColor) {

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

            footprint = new GameObject("BattleRoyaleFootprint");
            footprint.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                position = new Vector3(player.transform.position.x, player.transform.position.y, -0.5f);
            }
            else {
                position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1f);
            }
            footprint.transform.position = position;
            footprint.transform.localPosition = position;
            footprint.transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));
            footprint.transform.SetParent(player.transform.parent);

            spriteRenderer = footprint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getFootprintSprite();
            spriteRenderer.color = color;

            footprints.Add(this);
            footprint.SetActive(true);

            if (BattleRoyale.matchType == 2) {
                HudManager.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>((p) => {

                    if (p == 1f && footprint != null) {
                        UnityEngine.Object.Destroy(footprint);
                        footprints.Remove(this);
                    }
                })));
            }
            else {
                HudManager.Instance.StartCoroutine(Effects.Lerp(420, new Action<float>((p) => {

                    if (p == 1f && footprint != null) {
                        UnityEngine.Object.Destroy(footprint);
                        footprints.Remove(this);
                    }
                })));
            }
        }
        public static void clearBattleRoyaleFootprints() {
            footprints = new List<BattleRoyaleFootprint>();
        }
    }
}