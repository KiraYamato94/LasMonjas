using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using static LasMonjas.HudManagerStartPatch;
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

        public HypnotistSpiral(float duration, PlayerControl player) {

            this.color = new Color(1f, 1f, 1f, 1f);

            hypnotistSpiral = new GameObject("HypnotistSpiral" + hypnotistSpirals.Count.ToString());
            hypnotistSpiral.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            if (PlayerControl.GameOptions.MapId == 5) {
                position = new Vector3(player.transform.position.x, player.transform.position.y, -0.5f);
            }
            else {
                position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 1f);
            }
            hypnotistSpiral.transform.position = position;
            hypnotistSpiral.transform.localPosition = position;
            hypnotistSpiral.transform.SetParent(player.transform.parent);

            spriteRenderer = hypnotistSpiral.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getSpiralSprite();

            mySpiralDuration = duration;
            spriteRenderer.color = color;

            // Only render the trap for the Hypnotist
            var playerIsHypnotist = PlayerControl.LocalPlayer == Hypnotist.hypnotist;
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

                    var player = PlayerControl.LocalPlayer;

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

                        // Prevent using movement mechanic while being hypnotized
                        switch (PlayerControl.GameOptions.MapId) {
                            case 0:
                                GameObject skeldMedScanner = GameObject.Find("MedScanner");
                                skeldMedScanner.GetComponent<CircleCollider2D>().enabled = false;
                                objectCoundown(0);
                                break;
                            case 1:
                                GameObject miraMedScanner = GameObject.Find("MedScanner");
                                miraMedScanner.GetComponent<CircleCollider2D>().enabled = false;
                                objectCoundown(1);
                                break;
                            case 2:
                                GameObject polusMedScanner = GameObject.Find("panel_medplatform");
                                polusMedScanner.GetComponent<CircleCollider2D>().enabled = false;
                                objectCoundown(2);
                                break;
                            case 3:
                                GameObject dleksMedScanner = GameObject.Find("MedScanner");
                                dleksMedScanner.GetComponent<CircleCollider2D>().enabled = false;
                                objectCoundown(3);
                                break;
                            case 4:
                                GameObject airshipMeetingLadderTop = GameObject.Find("Airship(Clone)/MeetingRoom/ladder_meeting/LadderTop");
                                airshipMeetingLadderTop.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject airshipMeetingLadderBottom = GameObject.Find("Airship(Clone)/MeetingRoom/ladder_meeting/LadderBottom");
                                airshipMeetingLadderBottom.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject airshipPlatformLeft = GameObject.Find("PlatformLeft");
                                airshipPlatformLeft.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject airshipPlatformRight = GameObject.Find("PlatformRight");
                                airshipPlatformRight.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject airshipgapLadderTop = GameObject.Find("Airship(Clone)/GapRoom/ladder_gap/LadderTop");
                                airshipgapLadderTop.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject airshipgapLadderBottom = GameObject.Find("Airship(Clone)/GapRoom/ladder_gap/LadderBottom");
                                airshipgapLadderBottom.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject airshipelectricalLadderTop = GameObject.Find("Airship(Clone)/HallwayMain/ladder_electrical/LadderTop");
                                airshipelectricalLadderTop.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject airshipelectricalLadderBottom = GameObject.Find("Airship(Clone)/HallwayMain/ladder_electrical/LadderBottom");
                                airshipelectricalLadderBottom.GetComponent<CircleCollider2D>().enabled = false;
                                objectCoundown(4);
                                break;
                            case 5:
                                GameObject submergedMedScanner = GameObject.Find("console_medscan");
                                submergedMedScanner.GetComponent<CircleCollider2D>().enabled = false;
                                objectCoundown(5);
                                break;
                        }

                        // Assign hypnotized to renegade and minion so they can't vent
                        if (Renegade.renegade != null && player == Renegade.renegade) {
                            Renegade.isHypnotized = true;
                            rebelHypnotized(1);
                        }
                        if (Minion.minion != null && player == Minion.minion) {
                            Minion.isHypnotized = true;
                            rebelHypnotized(2);
                        }

                        Hypnotist.messageTimer = Hypnotist.spiralDuration;
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.medusaPetrify, false, 100f);
                        if (MapBehaviour.Instance) {
                            MapBehaviour.Instance.Close();
                        }
                        new CustomMessage("Hypnotized!", Hypnotist.spiralDuration, -1, 1.3f, 22);
                        PlayerControl target = Helpers.playerById(player.PlayerId);
                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ActivateSpiralTrap, Hazel.SendOption.Reliable, -1);
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

        public static void rebelHypnotized(int player) {
            HudManager.Instance.StartCoroutine(Effects.Lerp(Hypnotist.spiralDuration, new Action<float>((p) => {
                if (p == 1f) {
                    switch (player) {
                        case 1:
                            Renegade.isHypnotized = false;
                            break;
                        case 2:
                            Minion.isHypnotized = false;
                            break;
                    }
                }
            })));
        }

        public static void objectCoundown(int whichMap) {
            HudManager.Instance.StartCoroutine(Effects.Lerp(Hypnotist.spiralDuration, new Action<float>((p) => {
                if (p == 1f) {
                    switch (whichMap) {
                        case 0:
                            GameObject skeldMedScanner = GameObject.Find("MedScanner");
                            skeldMedScanner.GetComponent<CircleCollider2D>().enabled = true;
                            break;
                        case 1:
                            GameObject miraMedScanner = GameObject.Find("MedScanner");
                            miraMedScanner.GetComponent<CircleCollider2D>().enabled = true;
                            break;
                        case 2:
                            GameObject polusMedScanner = GameObject.Find("panel_medplatform");
                            polusMedScanner.GetComponent<CircleCollider2D>().enabled = true;
                            break;
                        case 3:
                            GameObject dleksMedScanner = GameObject.Find("MedScanner");
                            dleksMedScanner.GetComponent<CircleCollider2D>().enabled = true;
                            break;
                        case 4:
                            GameObject airshipMeetingLadderTop = GameObject.Find("Airship(Clone)/MeetingRoom/ladder_meeting/LadderTop");
                            airshipMeetingLadderTop.GetComponent<CircleCollider2D>().enabled = true;
                            GameObject airshipMeetingLadderBottom = GameObject.Find("Airship(Clone)/MeetingRoom/ladder_meeting/LadderBottom");
                            airshipMeetingLadderBottom.GetComponent<CircleCollider2D>().enabled = true;
                            GameObject airshipPlatformLeft = GameObject.Find("PlatformLeft");
                            airshipPlatformLeft.GetComponent<CircleCollider2D>().enabled = true;
                            GameObject airshipPlatformRight = GameObject.Find("PlatformRight");
                            airshipPlatformRight.GetComponent<CircleCollider2D>().enabled = true;
                            GameObject airshipgapLadderTop = GameObject.Find("Airship(Clone)/GapRoom/ladder_gap/LadderTop");
                            airshipgapLadderTop.GetComponent<CircleCollider2D>().enabled = true;
                            GameObject airshipgapLadderBottom = GameObject.Find("Airship(Clone)/GapRoom/ladder_gap/LadderBottom");
                            airshipgapLadderBottom.GetComponent<CircleCollider2D>().enabled = true;
                            GameObject airshipelectricalLadderTop = GameObject.Find("Airship(Clone)/HallwayMain/ladder_electrical/LadderTop");
                            airshipelectricalLadderTop.GetComponent<CircleCollider2D>().enabled = true;
                            GameObject airshipelectricalLadderBottom = GameObject.Find("Airship(Clone)/HallwayMain/ladder_electrical/LadderBottom");
                            airshipelectricalLadderBottom.GetComponent<CircleCollider2D>().enabled = true;
                            break;
                        case 5:
                            GameObject submergedMedScanner = GameObject.Find("console_medscan");
                            submergedMedScanner.GetComponent<CircleCollider2D>().enabled = true;
                            break;
                    }
                }
            }))); 
        }
    }
}