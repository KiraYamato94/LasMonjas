using LasMonjas.Patches;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LasMonjas.Objects
{
    class Footprint {
        private static List<Footprint> footprints = new List<Footprint>();
        private static Sprite sprite;
        private Color color;
        private GameObject footprint;
        private SpriteRenderer spriteRenderer;
        private PlayerControl owner;
        private bool anonymousFootprints;
        private Vector3 position;

        public static Sprite getFootprintSprite() {
            if (sprite) return sprite;
            sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.DetectiveFootprint.png", 400f);
            return sprite;
        }

        public Footprint(float footprintDuration, bool anonymousFootprints, PlayerControl player) {
            this.owner = player;
            this.anonymousFootprints = anonymousFootprints;
            if (anonymousFootprints)
                this.color = Palette.PlayerColors[6];
            else
                this.color = Palette.PlayerColors[(int) player.Data.DefaultOutfit.ColorId];

            footprint = new GameObject("Footprint");
            footprint.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (PlayerControl.GameOptions.MapId == 5) {
                position = new Vector3(player.transform.position.x, player.transform.position.y, -0.5f);
            }
            else {
                position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1f);
            }
            footprint.transform.position = position;
            footprint.transform.localPosition = position;
            footprint.transform.SetParent(player.transform.parent);

            spriteRenderer = footprint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getFootprintSprite();
            spriteRenderer.color = color;

            footprints.Add(this);
            if (Detective.showFootPrints == 0) {
                if (Detective.detectiveTimer >= 0) {
                    foreach (Footprint myfootpint in footprints) {
                        myfootpint.footprint.SetActive(true);
                    }
                }
                else {
                    foreach (Footprint myfootpint in footprints) {
                        myfootpint.footprint.SetActive(false);
                    }
                }
            }
            else {
                footprint.SetActive(true);
            }

            Color c = color;
            if (!anonymousFootprints && owner != null) {
                if (owner == Mimic.mimic && Mimic.transformTimer > 0 && Mimic.transformTarget?.Data != null)
                    c = Palette.ShadowColors[Mimic.transformTarget.Data.DefaultOutfit.ColorId];
                else if (Painter.painterTimer > 0)
                    c = Palette.PlayerColors[Detective.footprintcolor];
                else if (Challenger.isDueling)
                    c = Palette.PlayerColors[6];
            }

            HudManager.Instance.StartCoroutine(Effects.Lerp(footprintDuration, new Action<float>((p) => {

            if (spriteRenderer) spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1 - p));

            if (p == 1f && footprint != null) {
                UnityEngine.Object.Destroy(footprint);
                footprints.Remove(this);
            }
            })));
        }
    }
}