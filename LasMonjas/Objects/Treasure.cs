using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using static LasMonjas.HudManagerStartPatch;
using static LasMonjas.LasMonjas;
using Hazel;
using LasMonjas.Patches;

namespace LasMonjas.Objects
{
    class Treasure
    {
        public static List<Treasure> treasures = new List<Treasure>();
        private static Sprite sprite;
        private Color color;
        private GameObject treasure;
        private SpriteRenderer spriteRenderer;
        private bool touchedPlayer = false;
        private Vector3 position;

        public static Sprite getTreasureSprite() {
            if (sprite) return sprite;
            sprite = CustomMain.customAssets.treasureHunterTreasure.GetComponent<SpriteRenderer>().sprite;
            return sprite;
        }

        public Treasure(float chestDuration, PlayerControl player) {

            this.color = new Color(1f, 1f, 1f, 1f);

            treasure = new GameObject("Treasure");
            treasure.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            switch (PlayerControl.GameOptions.MapId) {
                case 0:
                    if (activatedSensei) {
                        switch (TreasureHunter.randomSpawn) {
                            case 1:
                                position = new Vector3(-3.7f, 5.4f, 1f);
                                break;
                            case 2:
                                position = new Vector3(6.75f, 4.9f, 1f);
                                break;
                            case 3:
                                position = new Vector3(7.5f, -1.15f, 1f);
                                break;
                            case 4:
                                position = new Vector3(12.75f, -0.25f, 1f);
                                break;
                            case 5:
                                position = new Vector3(4.9f, -8.5f, 1f);
                                break;
                            case 6:
                                position = new Vector3(7.5f, -14.35f, 1f);
                                break;
                            case 7:
                                position = new Vector3(-5.3f, -13, 1f);
                                break;
                            case 8:
                                position = new Vector3(-12.15f, -13f, 1f);
                                break;
                            case 9:
                                position = new Vector3(-19.85f, -8f, 1f);
                                break;
                            case 10:
                                position = new Vector3(-21.5f, -3.75f, 1f);
                                break;
                            case 11:
                                position = new Vector3(-19.75f, 5.25f, 1f);
                                break;
                            case 12:
                                position = new Vector3(-8.15f, -0.5f, 1f);
                                break;
                            case 13:
                                position = new Vector3(-7.5f, -4f, 1f);
                                break;
                            case 14:
                                position = new Vector3(-7.75f, 11.35f, 1f);
                                break;
                        }
                    }
                    else {
                        switch (TreasureHunter.randomSpawn) {
                            case 1:
                                position = new Vector3(10, 2.5f, 1f);
                                break;
                            case 2:
                                position = new Vector3(5.25f, -4.7f, 1f);
                                break;
                            case 3:
                                position = new Vector3(16.85f, -6.25f, 1f);
                                break;
                            case 4:
                                position = new Vector3(11.35f, -10.25f, 1f);
                                break;
                            case 5:
                                position = new Vector3(2, -15, 1f);
                                break;
                            case 6:
                                position = new Vector3(6.5f, -8.75f, 1f);
                                break;
                            case 7:
                                position = new Vector3(-1.5f, -17, 1f);
                                break;
                            case 8:
                                position = new Vector3(-0.75f, 5.5f, 1f);
                                break;
                            case 9:
                                position = new Vector3(-5.9f, -5.25f, 1f);
                                break;
                            case 10:
                                position = new Vector3(-14, -6.85f, 1f);
                                break;
                            case 11:
                                position = new Vector3(-19.75f, -3.85f, 1f);
                                break;
                            case 12:
                                position = new Vector3(-18.25f, 2.5f, 1f);
                                break;
                            case 13:
                                position = new Vector3(-19, -9.75f, 1f);
                                break;
                            case 14:
                                position = new Vector3(-9.75f, -8.75f, 1f);
                                break;
                        }
                    }
                    treasure.transform.position = position;
                    treasure.transform.localPosition = position;
                    treasure.transform.SetParent(TreasureHunter.treasureHunter.transform.parent);
                    break;
                case 1:
                    switch (TreasureHunter.randomSpawn) {
                        case 1:
                            position = new Vector3(-4.5f, 2.75f, 1f);
                            break;
                        case 2:
                            position = new Vector3(9.15f, 4.75f, 1f);
                            break;
                        case 3:
                            position = new Vector3(11.5f, 10.3f, 1f);
                            break;
                        case 4:
                            position = new Vector3(2.5f, 13f, 1f);
                            break;
                        case 5:
                            position = new Vector3(14.25f, 2.85f, 1f);
                            break;
                        case 6:
                            position = new Vector3(14.25f, -1.5f, 1f);
                            break;
                        case 7:
                            position = new Vector3(19.5f, -1.75f, 1f);
                            break;
                        case 8:
                            position = new Vector3(19.5f, 4.35f, 1f);
                            break;
                        case 9:
                            position = new Vector3(28.25f, 0f, 1f);
                            break;
                        case 10:
                            position = new Vector3(22.45f, 18.75f, 1f);
                            break;
                        case 11:
                            position = new Vector3(13.75f, 18.75f, 1f);
                            break;
                        case 12:
                            position = new Vector3(19.25f, 24.25f, 1f);
                            break;
                    }
                    treasure.transform.position = position;
                    treasure.transform.localPosition = position;
                    treasure.transform.SetParent(TreasureHunter.treasureHunter.transform.parent);
                    break;
                case 2:
                    switch (TreasureHunter.randomSpawn) {
                        case 1:
                            position = new Vector3(16.65f, -2, 1f);
                            break;
                        case 2:
                            position = new Vector3(3.5f, -7.75f, 1f);
                            break;
                        case 3:
                            position = new Vector3(3.75f, -11.75f, 1f);
                            break;
                        case 4:
                            position = new Vector3(10.6f, -12.25f, 1f);
                            break;
                        case 5:
                            position = new Vector3(20.65f, -12.25f, 1f);
                            break;
                        case 6:
                            position = new Vector3(32.5f, -15.85f, 1f);
                            break;
                        case 7:
                            position = new Vector3(34.9f, -9.75f, 1f);
                            break;
                        case 8:
                            position = new Vector3(40.3f, -8.15f, 1f);
                            break;
                        case 9:
                            position = new Vector3(29.75f, -7.5f, 1f);
                            break;
                        case 10:
                            position = new Vector3(25.5f, -7.5f, 1f);
                            break;
                        case 11:
                            position = new Vector3(36.5f, -21.75f, 1f);
                            break;
                        case 12:
                            position = new Vector3(22.1f, -25.25f, 1f);
                            break;
                        case 13:
                            position = new Vector3(16.25f, -25.25f, 1f);
                            break;
                        case 14:
                            position = new Vector3(12.75f, -24.25f, 1f);
                            break;
                        case 15:
                            position = new Vector3(2.25f, -24.25f, 1f);
                            break;
                        case 16:
                            position = new Vector3(12.5f, -17.25f, 1f);
                            break;
                        case 17:
                            position = new Vector3(1.5f, -18.9f, 1f);
                            break;
                        case 18:
                            position = new Vector3(-1.25f, -17.5f, 1f);
                            break;
                        case 19:
                            position = new Vector3(8.25f, -16.5f, 1f);
                            break;
                        case 20:
                            position = new Vector3(23.05f, -17.15f, 1f);
                            break;
                    }
                    treasure.transform.position = position;
                    treasure.transform.localPosition = position;
                    treasure.transform.SetParent(TreasureHunter.treasureHunter.transform.parent);
                    break;
                case 3:
                    switch (TreasureHunter.randomSpawn) {
                        case 1:
                            position = new Vector3(-10, 2.5f, 1f);
                            break;
                        case 2:
                            position = new Vector3(-5.25f, -4.75f, 1f);
                            break;
                        case 3:
                            position = new Vector3(-16.85f, -6.25f, 1f);
                            break;
                        case 4:
                            position = new Vector3(-11.35f, -10.25f, 1f);
                            break;
                        case 5:
                            position = new Vector3(-2, -15, 1f);
                            break;
                        case 6:
                            position = new Vector3(-6.5f, -8.75f, 1f);
                            break;
                        case 7:
                            position = new Vector3(1.5f, -17, 1f);
                            break;
                        case 8:
                            position = new Vector3(0.75f, 5.5f, 1f);
                            break;
                        case 9:
                            position = new Vector3(5.9f, -5.25f, 1f);
                            break;
                        case 10:
                            position = new Vector3(14, -6.85f, 1f);
                            break;
                        case 11:
                            position = new Vector3(19.75f, -3.85f, 1f);
                            break;
                        case 12:
                            position = new Vector3(18.25f, 2.5f, 1f);
                            break;
                        case 13:
                            position = new Vector3(19, -9.75f, 1f);
                            break;
                        case 14:
                            position = new Vector3(9.75f, -8.75f, 1f);
                            break;
                    }
                    treasure.transform.position = position;
                    treasure.transform.localPosition = position;
                    treasure.transform.SetParent(TreasureHunter.treasureHunter.transform.parent);
                    break;
                case 4:
                    switch (TreasureHunter.randomSpawn) {
                        case 1:
                            position = new Vector3(5.75f, -14.5f, 1f);
                            break;
                        case 2:
                            position = new Vector3(-4.5f, -9.5f, 1f);
                            break;
                        case 3:
                            position = new Vector3(-16, -12.5f, 1f);
                            break;
                        case 4:
                            position = new Vector3(-13.15f, -14.5f, 1f);
                            break;
                        case 5:
                            position = new Vector3(-14.5f, -8.3f, 1f);
                            break;
                        case 6:
                            position = new Vector3(-14.15f, -4.85f, 1f);
                            break;
                        case 7:
                            position = new Vector3(-23.5f, -1.35f, 1f);
                            break;
                        case 8:
                            position = new Vector3(-13.35f, 1.4f, 1f);
                            break;
                        case 9:
                            position = new Vector3(-7.4f, 0.6f, 1f);
                            break;
                        case 10:
                            position = new Vector3(16, 15.25f, 1f);
                            break;
                        case 11:
                            position = new Vector3(26, 0.4f, 1f);
                            break;
                        case 12:
                            position = new Vector3(21.7f, 2.7f, 1f);
                            break;
                        case 13:
                            position = new Vector3(25.25f, -9.65f, 1f);
                            break;
                        case 14:
                            position = new Vector3(18.25f, -4, 1f);
                            break;
                        case 15:
                            position = new Vector3(29, -1.5f, 1f);
                            break;
                        case 16:
                            position = new Vector3(37.35f, -3.6f, 1f);
                            break;
                        case 17:
                            position = new Vector3(30.8f, 7.25f, 1f);
                            break;
                        case 18:
                            position = new Vector3(13.5f, 6, 1f);
                            break;
                        case 19:
                            position = new Vector3(6.5f, 14.1f, 1f);
                            break;
                        case 20:
                            position = new Vector3(19.85f, 11.5f, 1f);
                            break;
                        case 21:
                            position = new Vector3(15.35f, 2, 1f);
                            break;
                        case 22:
                            position = new Vector3(12.3f, 2, 1f);
                            break;
                        case 23:
                            position = new Vector3(9.3f, 2, 1f);
                            break;
                        case 24:
                            position = new Vector3(6.3f, -3, 1f);
                            break;
                        case 25:
                            position = new Vector3(-8.85f, 12.4f, 1f);
                            break;
                        case 26:
                            position = new Vector3(6.3f, 2.5f, 1f);
                            break;
                    }
                    treasure.transform.position = position;
                    treasure.transform.localPosition = position;
                    treasure.transform.SetParent(TreasureHunter.treasureHunter.transform.parent);
                    break;
                case 5:
                    switch (TreasureHunter.randomSpawn) {
                        case 1:
                            position = new Vector3(-12.85f, -27.75f, -1f);
                            break;
                        case 2:
                            position = new Vector3(-14.75f, -34.25f, -1f);
                            break;
                        case 3:
                            position = new Vector3(-11f, -39f, -1f);
                            break;
                        case 4:
                            position = new Vector3(-6.85f, -42.75f, -1f);
                            break;
                        case 5:
                            position = new Vector3(6.5f, -39.5f, -1f);
                            break;
                        case 6:
                            position = new Vector3(5f, -33.7f, -1f);
                            break;
                        case 7:
                            position = new Vector3(0.2f, -33.9f, -1f);
                            break;
                        case 8:
                            position = new Vector3(-4.25f, -33.5f, -1f);
                            break;
                        case 9:
                            position = new Vector3(7.75f, -23.5f, -1f);
                            break;
                        case 10:
                            position = new Vector3(13, -25.25f, -1f);
                            break;
                        case 11:
                            position = new Vector3(12.85f, -32f, -1f);
                            break;
                        case 12:
                            position = new Vector3(4.35f, 8.35f, -1f);
                            break;
                        case 13:
                            position = new Vector3(14.25f, 24.35f, -1f);
                            break;
                        case 14:
                            position = new Vector3(5.55f, 31.25f, -1f);
                            break;
                        case 15:
                            position = new Vector3(0f, 33.75f, -1f);
                            break;
                        case 16:
                            position = new Vector3(-11.5f, 30.5f, -1f);
                            break;
                        case 17:
                            position = new Vector3(-12.5f, 15.5f, -1f);
                            break;
                        case 18:
                            position = new Vector3(-6.75f, 10f, -1f);
                            break;
                        case 19:
                            position = new Vector3(-6.65f, 15.25f, -1f);
                            break;
                        case 20:
                            position = new Vector3(0.65f, 10f, -1f);
                            break;
                        case 21:
                            position = new Vector3(-1.8f, 12f, -1f);
                            break;
                        case 22:
                            position = new Vector3(-6.5f, 28.5f, -1f);
                            break;
                    }
                    treasure.transform.position = position;
                    treasure.transform.localPosition = position;
                    treasure.transform.SetParent(TreasureHunter.treasureHunter.transform.parent);
                    break;
            }

            spriteRenderer = treasure.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getTreasureSprite();
            spriteRenderer.color = color;

            var playerIsTreasureHunter = PlayerControl.LocalPlayer == TreasureHunter.treasureHunter;
            treasure.SetActive(playerIsTreasureHunter);

            treasures.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(chestDuration, new Action<float>((p) => {

                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (!touchedPlayer && Vector2.Distance(player.transform.position, treasure.transform.position) < 0.5f && player == PlayerControl.LocalPlayer && player == TreasureHunter.treasureHunter && !player.Data.IsDead) {
                        touchedPlayer = true;
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CollectedTreasure, Hazel.SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.collectedTreasure();
                        treasure.SetActive(false); 
                    }
                }

            })));

        }
    }
}
