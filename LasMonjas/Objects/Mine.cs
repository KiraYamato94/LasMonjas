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
    class Mine
    {
        public static List<Mine> mines = new List<Mine>();
        private static Sprite sprite;
        private Color color;
        public GameObject mine;
        private SpriteRenderer spriteRenderer;
        private bool touched = false;
        private Vector3 position;

        public static Sprite getMineSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.trapperMine.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public Mine(float duration, PlayerControl player) {

            this.color = new Color(1f, 1f, 1f, 1f);

            mine = new GameObject("Mine" + mines.Count.ToString());
            mine.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (PlayerControl.GameOptions.MapId == 5) {
                position = new Vector3(player.transform.position.x, player.transform.position.y, -0.5f);
            }
            else {
                position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1f);
            }
            mine.transform.position = position;
            mine.transform.localPosition = position;
            mine.transform.SetParent(player.transform.parent);

            spriteRenderer = mine.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getMineSprite();
            spriteRenderer.color = color;

            var playerIsTrapper = PlayerControl.LocalPlayer == Trapper.trapper;
            mine.SetActive(playerIsTrapper);

            mines.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {

                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (Vector2.Distance(player.transform.position, mine.transform.position) < 0.3f && !touched && player != Trapper.trapper && !player.Data.IsDead) {
                        touched = true;
                        mine.SetActive(true);
                        if (player == PlayerControl.LocalPlayer) {
                            PlayerControl target = Helpers.playerById(player.PlayerId);
                            MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MineKill, Hazel.SendOption.Reliable, -1);
                            killWriter.Write(target.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                            RPCProcedure.mineKill(target.PlayerId);
                        }
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
