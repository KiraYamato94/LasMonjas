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
    class DIO
    {
        public static List<DIO> dios = new List<DIO>();
        private static Sprite sprite;
        private Color color;
        private GameObject dio;
        private SpriteRenderer spriteRenderer;

        public static Sprite getDioSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.performerDio.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public DIO(float dioDuration, PlayerControl player) {

            this.color = new Color(1f, 1f, 1f, 1f);

            dio = new GameObject("DIO");
            dio.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(player.transform.position.x - 0.15f, player.transform.position.y + 0.35f, player.transform.position.z - 1f);
            dio.transform.position = position;
            dio.transform.localPosition = position;
            dio.transform.SetParent(player.transform.parent);

            spriteRenderer = dio.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getDioSprite();
            spriteRenderer.color = color;

            dio.SetActive(true);
            dios.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(dioDuration, new Action<float>((p) => {

                if (p == 1f && dio != null) {
                    UnityEngine.Object.Destroy(dio);
                    dios.Remove(this);
                }

            })));

        }
    }
}
