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
    class YinYang
    {
        public static List<YinYang> yinyangs = new List<YinYang>();
        private static Sprite sprite;
        private Color color;
        private GameObject yinyang;
        private SpriteRenderer spriteRenderer;

        public static Sprite getYinyangSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.yinyangerYinyang.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public YinYang(float yinyangDuration, PlayerControl player) {

            this.color = new Color(1f, 1f, 1f, 1f);

            yinyang = new GameObject("YinYang");
            yinyang.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y + 1f, player.transform.position.z - 1f);
            yinyang.transform.position = position;
            yinyang.transform.localPosition = position;
            yinyang.transform.SetParent(player.transform.parent);

            spriteRenderer = yinyang.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getYinyangSprite();
            spriteRenderer.color = color;

            yinyang.SetActive(true);
            yinyangs.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(yinyangDuration, new Action<float>((p) => {

                if (p == 1f && yinyang != null) {
                    UnityEngine.Object.Destroy(yinyang);
                    yinyangs.Remove(this);
                }

            })));

        }
    }
}
