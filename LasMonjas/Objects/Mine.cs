using System;
using System.Collections.Generic;
using UnityEngine;
using Hazel;
using LasMonjas.Patches;
using LasMonjas.Core;

namespace LasMonjas.Objects
{
    class Mine
    {
        public static List<Mine> mines = new List<Mine>();
        private static Sprite sprite;
        private Color color;
        public GameObject mine;
        private SpriteRenderer spriteRenderer;
        public bool touched = false;
        private Vector3 position;
        public int mineId;

        public static Sprite getMineSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.trapperMine.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public Mine(float duration, Vector2 player) {
            mineId = Trapper.mineCountId;

            this.color = new Color(1f, 1f, 1f, 1f);

            mine = new GameObject("Mine" + mines.Count.ToString());
            mine.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (Helpers.isSubmergedMap()) {
                position = new Vector3(player.x, player.y, -0.5f);
            }
            else {
                position = new Vector3(player.x, player.y, 1f);
            }
            mine.transform.position = position;
            mine.transform.localPosition = position;
            mine.transform.SetParent(Trapper.trapper.transform.parent);

            spriteRenderer = mine.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getMineSprite();
            spriteRenderer.color = color;

            var playerIsTrapper = PlayerInCache.LocalPlayer.PlayerControl == Trapper.trapper;
            mine.SetActive(playerIsTrapper);

            mines.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {

                var player = PlayerInCache.LocalPlayer.PlayerControl;

                if (Vector2.Distance(player.transform.position, mine.transform.position) < 0.3f && !touched && player != Trapper.trapper && !player.Data.IsDead) {
                    touched = true;
                    mine.SetActive(true);
                    if (player == PlayerInCache.LocalPlayer.PlayerControl) {
                        PlayerControl target = Helpers.playerById(player.PlayerId);
                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.MineKill, Hazel.SendOption.Reliable, -1);
                        killWriter.Write(target.PlayerId);
                        killWriter.Write(mineId);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.mineKill(target.PlayerId, mineId);
                    }
                }

                if (p == 1f && mine != null) {
                    Trapper.currentMineNumber -= 1;
                    mine.transform.position = new Vector3 (-1000, 500, 0);
                    //mines.Remove(this);
                    //UnityEngine.Object.Destroy(mine);
                }

            })));
        }

        public static void clearMines() {
            mines = new List<Mine>();
        }
    }
}