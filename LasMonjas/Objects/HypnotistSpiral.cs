using System;
using System.Collections.Generic;
using UnityEngine;
using Hazel;
using LasMonjas.Patches;
using LasMonjas.Core;

namespace LasMonjas.Objects
{
    class HypnotistSpiral
    {
        public static List<HypnotistSpiral> hypnotistSpirals = new List<HypnotistSpiral>();
        private Color color;
        public GameObject hypnotistSpiral;
        private SpriteRenderer spriteRenderer;
        private bool touched = false;
        private float mySpiralDuration = 20;
        private Vector3 position;
        public bool isActive = true;

        private static Sprite spiralSprite;

        public static Sprite getSpiralSprite() {
            if (spiralSprite) return spiralSprite;
            spiralSprite = CustomMain.customAssets.hypnotistReverse.GetComponent<SpriteRenderer>().sprite;
            return spiralSprite;
        }

        public HypnotistSpiral(float duration, Vector2 player) {

            this.color = new Color(1f, 1f, 1f, 1f);

            hypnotistSpiral = new GameObject("HypnotistSpiral" + hypnotistSpirals.Count.ToString());
            hypnotistSpiral.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                position = new Vector3(player.x, player.y, -0.5f);
            }
            else {
                position = new Vector3(player.x, player.y, 1f);
            }
            hypnotistSpiral.transform.position = position;
            hypnotistSpiral.transform.localPosition = position;
            hypnotistSpiral.transform.SetParent(Hypnotist.hypnotist.transform.parent);

            spriteRenderer = hypnotistSpiral.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getSpiralSprite();

            mySpiralDuration = duration;
            spriteRenderer.color = color;

            // Only render the trap for the Hypnotist
            var playerIsHypnotist = PlayerInCache.LocalPlayer.PlayerControl == Hypnotist.hypnotist;
            if (playerIsHypnotist) {
                hypnotistSpiral.gameObject.SetActive(true);
                hypnotistSpiral.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else {
                hypnotistSpiral.gameObject.SetActive(false);
            }

            hypnotistSpirals.Add(this);
        }

        public static void clearHypnotistSpirals() {
            hypnotistSpirals = new List<HypnotistSpiral>();
        }

        public static void activateSpirals() {
            foreach (HypnotistSpiral hypnotistSpiral in hypnotistSpirals) {
                hypnotistSpiral.hypnotistSpiral.gameObject.SetActive(true);

                hypnotistSpiral.hypnotistSpiral.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                hypnotistSpiral.spriteRenderer.sprite = getSpiralSprite();


                HudManager.Instance.StartCoroutine(Effects.Lerp(1800, new Action<float>((p) => {

                    var player = PlayerInCache.LocalPlayer.PlayerControl;

                    if (Vector2.Distance(player.transform.position, hypnotistSpiral.hypnotistSpiral.transform.position) < 0.5f && !hypnotistSpiral.touched && !player.Data.IsDead && !player.Data.Role.IsImpostor && hypnotistSpiral.isActive) {
                        hypnotistSpiral.touched = true;

                        HudManager.Instance.StartCoroutine(Effects.Lerp(hypnotistSpiral.mySpiralDuration, new Action<float>((p) => {

                            if (p == 1f && hypnotistSpiral.touched) {
                                hypnotistSpiral.touched = false;
                            }

                        })));

                        foreach (HypnotistSpiral spiral in hypnotistSpirals) {
                            spiral.isActive = false;
                        }

                        Hypnotist.messageTimer = Hypnotist.spiralDuration;
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.medusaPetrify, false, 100f);
                        if (MapBehaviour.Instance) {
                            MapBehaviour.Instance.Close();
                        }
                        new CustomMessage(Language.statusRolesTexts[1], Hypnotist.spiralDuration, new Vector2(0f, 1.3f), 4);
                        PlayerControl target = Helpers.playerById(player.PlayerId);
                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ActivateSpiralTrap, Hazel.SendOption.Reliable, -1);
                        killWriter.Write(player.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.activateSpiralTrap(target.PlayerId);
                    }

                    if (p == 1f && hypnotistSpiral != null) {
                        hypnotistSpiral.hypnotistSpiral.transform.position = new Vector3(-1000, 500, 0);
                        //UnityEngine.Object.Destroy(trap);
                        //traps.Remove(this);
                    }

                })));
            }
            return;
        }
    }
}