using System;
using System.Collections.Generic;
using UnityEngine;
using Hazel;
using LasMonjas.Patches;
using LasMonjas.Core;

namespace LasMonjas.Objects
{
    class Trap
    {
        public static List<Trap> traps = new List<Trap>();
        private static Sprite sprite;
        private Color color;
        public GameObject trap;
        private SpriteRenderer spriteRenderer;
        private bool touched = false;
        private Vector3 position;

        public static Sprite getTrapSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.trapperTrap.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public Trap(float duration, Vector2 player) {

            this.color = new Color(1f, 1f, 1f, 1f);

            trap = new GameObject("Trap" + traps.Count.ToString());
            trap.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                position = new Vector3(player.x, player.y, -0.5f);
            }
            else {
                position = new Vector3(player.x, player.y, 1f);
            }
            trap.transform.position = position;
            trap.transform.localPosition = position;
            trap.transform.SetParent(Trapper.trapper.transform.parent);

            spriteRenderer = trap.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getTrapSprite();
            spriteRenderer.color = color;

            var playerIsTrapper = PlayerInCache.LocalPlayer.PlayerControl == Trapper.trapper;
            trap.SetActive(playerIsTrapper);

            traps.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {

                var player = PlayerInCache.LocalPlayer.PlayerControl;
                if (Vector2.Distance(player.transform.position, trap.transform.position) < 0.3f && !touched && player != Trapper.trapper && !player.Data.IsDead) {
                    touched = true;
                    trap.SetActive(true);
                    if (player == PlayerInCache.LocalPlayer.PlayerControl) {
                        PlayerControl target = Helpers.playerById(player.PlayerId);
                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ActivateTrap, Hazel.SendOption.Reliable, -1);
                        killWriter.Write(player.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.activateTrap(target.PlayerId);
                    }
                }

                if (p == 1f && trap != null) {
                    Trapper.currentTrapNumber -= 1;
                    trap.transform.position = new Vector3 (-1000, 500, 0);
                    //traps.Remove(this);
                    //UnityEngine.Object.Destroy(trap);
                }

            })));
        }

        public static void clearTraps() {
            traps = new List<Trap>();
        }
    }
}