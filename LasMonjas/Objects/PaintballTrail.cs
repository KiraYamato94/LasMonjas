using LasMonjas.Core;
using LasMonjas.Patches;
using System;
using System.Collections.Generic;
using UnityEngine;
using static LasMonjas.LasMonjas;

namespace LasMonjas.Objects {
    class PaintballTrail {
        private static List<PaintballTrail> paintballtrail = new List<PaintballTrail>();
        private static List<Sprite> sprites = new List<Sprite>();
        private Color color;
        private GameObject paint;
        private SpriteRenderer spriteRenderer;
        private Vector3 position;

        public static List<Sprite> getPaintSprites() {
            if (sprites.Count > 0) return sprites;
            sprites.Add(Helpers.loadSpriteFromResources("LasMonjas.Images.Paint1.png", 600));
            sprites.Add(Helpers.loadSpriteFromResources("LasMonjas.Images.Paint2.png", 400));
            sprites.Add(Helpers.loadSpriteFromResources("LasMonjas.Images.Paint3.png", 200));
            return sprites;
        }

        public PaintballTrail(PlayerControl player, PlayerControl paintedPlayer) {
            this.color = Palette.PlayerColors[(int)paintedPlayer.Data.DefaultOutfit.ColorId];
            var sp = getPaintSprites();
            var index = rnd.Next(0, sp.Count);


            paint = new GameObject("Paint" + index);
            paint.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                position = new Vector3(player.transform.position.x, player.transform.position.y, -0.5f);
            }
            else {
                position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1f);
            }
            paint.transform.position = position;
            paint.transform.localPosition = position;
            paint.transform.SetParent(player.transform.parent);

            paint.transform.Rotate(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));

            spriteRenderer = paint.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sp[index];
            spriteRenderer.material = FastDestroyableSingleton<HatManager>.Instance.PlayerMaterial;
            paintedPlayer.SetPlayerMaterialColors(spriteRenderer);

            paint.SetActive(true);
            paintballtrail.Add(this);

            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(10f, new Action<float>((p) => {
            Color c = color;
            if (Painter.painterTimer > 0 || Helpers.MushroomSabotageActive() || Challenger.isDueling || Seeker.isMinigaming) c = Palette.PlayerColors[6];
            if (spriteRenderer) spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1 - p));

            if (p == 1f && paint != null) {
                UnityEngine.Object.Destroy(paint);
                paintballtrail.Remove(this);
            }
            })));
        }

        public static void resetTrail()
        {
            sprites.Clear();
        }
    }
}