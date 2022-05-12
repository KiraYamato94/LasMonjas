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
    class Bomb
    {
        public static List<Bomb> bombs = new List<Bomb>();
        private static Sprite sprite;
        private Color color;
        private GameObject bomb;
        private SpriteRenderer spriteRenderer;
        private float timer;
        private bool touchedPlayer = false;
        private int localBombNumber = 0;

        public static Sprite getBombSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.bombermanBomb.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public Bomb(float bombDuration, PlayerControl player, int localcurrentBombNumber) {

            this.color = new Color(1f, 1f, 1f, 1f);

            bomb = new GameObject("Bomb");
            bomb.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y, -0.5f);
            bomb.transform.position = position;
            bomb.transform.localPosition = position;
            bomb.transform.SetParent(player.transform.parent);

            spriteRenderer = bomb.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getBombSprite();
            spriteRenderer.color = color;

            bomb.SetActive(true);
            bombs.Add(this);

            timer = bombDuration;
            localBombNumber = localcurrentBombNumber;

            HudManager.Instance.StartCoroutine(Effects.Lerp(bombDuration, new Action<float>((p) => {

                timer -= Time.deltaTime;

                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (!touchedPlayer && Vector2.Distance(player.transform.position, bomb.transform.position) < 0.5f && player == PlayerControl.LocalPlayer && player != Bomberman.bomberman && !player.Data.IsDead) {
                        touchedPlayer = true; 
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FixBomb, Hazel.SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.fixBomb();
                    }
                }

                if (timer <= 0f) {
                    if (Bomberman.activeBomb == true && localBombNumber == Bomberman.currentBombNumber) { 
                        MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.BombermanWin, Hazel.SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                        RPCProcedure.bombermanWin(); 
                    }
                } 
                
                if (p == 1f && bomb != null) {
                    UnityEngine.Object.Destroy(bomb);
                    bombs.Remove(this);
                }
               
            })));

        }        
    }
}
