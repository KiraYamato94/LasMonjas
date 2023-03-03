using System;
using System.Collections.Generic;
using UnityEngine;
using LasMonjas.Patches;

namespace LasMonjas.Objects
{
    class Tased
    {
        public static List<Tased> tasers = new List<Tased>();
        private static Sprite sprite;
        private Color color;
        private GameObject taser;
        private SpriteRenderer spriteRenderer;

        public static Sprite getTaserSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.policeParalyze.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public Tased(float taserDuration, PlayerControl player) {

            this.color = new Color(1f, 1f, 1f, 1f);

            taser = new GameObject("Tased");
            taser.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1f);
            taser.transform.position = position;
            taser.transform.localPosition = position;
            taser.transform.SetParent(player.transform);

            spriteRenderer = taser.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getTaserSprite();
            spriteRenderer.color = color;

            taser.SetActive(true);
            tasers.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(taserDuration, new Action<float>((p) => {
                player.moveable = false;
                player.NetTransform.Halt(); // Stop current movement
                if (p == 1f) {
                    player.moveable = true;
                    UnityEngine.Object.Destroy(taser);
                    tasers.Remove(this);
                }
            })));
        }
    }
}