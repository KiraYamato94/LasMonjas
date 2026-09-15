using System;
using System.Collections.Generic;
using UnityEngine;
using static LasMonjas.LasMonjas;
using Hazel;
using LasMonjas.Patches;
using LasMonjas.Core;

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

        private static readonly Vector3[] SenseiMapTreasures = {
            Vector3.zero, // placeholder for case 0 that it has no use
            new (-3.7f, 5.4f, 1f),
            new (6.75f, 4.9f, 1f),
            new (7.5f, -1.15f, 1f),
            new (12.75f, -0.25f, 1f),
            new (4.9f, -8.5f, 1f),
            new (7.5f, -14.35f, 1f),
            new (-5.3f, -13, 1f),
            new (-12.15f, -13f, 1f),
            new (-19.85f, -8f, 1f),
            new (-21.5f, -3.75f, 1f),
            new (-19.75f, 5.25f, 1f),
            new (-8.15f, -0.5f, 1f),
            new (-7.5f, -4f, 1f),
            new (-7.75f, 11.35f, 1f)
        };

        private static readonly Vector3[] DleksMapTreasures = {
            Vector3.zero, // placeholder for case 0 that it has no use
            new (-10, 2.5f, 1f),
            new (-5.25f, -4.75f, 1f),
            new (-16.85f, -6.25f, 1f),
            new (-11.35f, -10.25f, 1f),
            new (-2, -15, 1f),
            new (-6.5f, -8.75f, 1f),
            new (1.5f, -17, 1f),
            new (0.75f, 5.5f, 1f),
            new (5.9f, -5.25f, 1f),
            new (14, -6.85f, 1f),
            new (19.75f, -3.85f, 1f),
            new (18.25f, 2.5f, 1f),
            new (19, -9.75f, 1f),
            new (9.75f, -8.75f, 1f)
        };

        private static readonly Vector3[] SkeldMapTreasures = {
            Vector3.zero, // placeholder for case 0 that it has no use
            new (10, 2.5f, 1f),
            new (5.25f, -4.75f, 1f),
            new (16.85f, -6.25f, 1f),
            new (11.35f, -10.25f, 1f),
            new (2, -15, 1f),
            new (6.5f, -8.75f, 1f),
            new (-1.5f, -17, 1f),
            new (-0.75f, 5.5f, 1f),
            new (-5.9f, -5.25f, 1f),
            new (-14, -6.85f, 1f),
            new (-19.75f, -3.85f, 1f),
            new (-18.25f, 2.5f, 1f),
            new (-19, -9.75f, 1f),
            new (-9.75f, -8.75f, 1f)
        };

        private static readonly Vector3[] MiraMapTreasures = {
            Vector3.zero, // placeholder for case 0 that it has no use
            new (-4.5f, 2.75f, 1f),
            new (9.15f, 4.75f, 1f),
            new (11.5f, 10.3f, 1f),
            new (2.5f, 13f, 1f),
            new (14.25f, 2.85f, 1f),
            new (14.25f, -1.5f, 1f),
            new (19.5f, -1.75f, 1f),
            new (19.5f, 4.35f, 1f),
            new (28.25f, 0f, 1f),
            new (22.45f, 18.75f, 1f),
            new (13.75f, 18.75f, 1f),
            new (19.25f, 24.25f, 1f)
        };

        private static readonly Vector3[] PolusMapTreasures = {
            Vector3.zero, // placeholder for case 0 that it has no use
            new (16.65f, -2, -1f),
            new (3.5f, -7.75f, -1f),
            new (3.75f, -11.75f, -1f),
            new (10.6f, -12.25f, -1f),
            new (20.65f, -12.25f, -1f),
            new (32.5f, -15.85f, -1f),
            new (34.9f, -9.75f, -1f),
            new (40.3f, -8.15f, -1f),
            new (29.75f, -7.5f, -1f),
            new (25.5f, -7.5f, -1f),
            new (36.5f, -21.75f, -1f),
            new (22.1f, -25.25f, -1f),
            new (16.25f, -25.25f, -1f),
            new (12.75f, -24.25f, -1f),
            new (2.25f, -24.25f, -1f),
            new (12.5f, -17.25f, -1f),
            new (1.5f, -18.9f, -1f),
            new (-1.25f, -17.5f, -1f),
            new (8.25f, -16.5f, -1f),
            new (23.05f, -17.15f, -1f)
        };

        private static readonly Vector3[] AirshipMapTreasures = {
            Vector3.zero, // placeholder for case 0 that it has no use
            new (5.75f, -14.5f, 1f),
            new (-4.5f, -9.5f, 1f),
            new (-16, -12.5f, 1f),
            new (-13.15f, -14.5f, 1f),
            new (-14.5f, -8.3f, 1f),
            new (-14.15f, -4.85f, 1f),
            new (-23.5f, -1.35f, 1f),
            new (-13.35f, 1.4f, 1f),
            new (-7.4f, 0.6f, 1f),
            new (16, 15.25f, 1f),
            new (26, 0.4f, 1f),
            new (21.7f, 2.7f, 1f),
            new (25.25f, -9.65f, 1f),
            new (18.25f, -4, 1f),
            new (29, -1.5f, 1f),
            new (37.35f, -3.6f, 1f),
            new (30.8f, 7.25f, 1f),
            new (13.5f, 6, 1f),
            new (6.5f, 14.1f, 1f),
            new (19.85f, 11.5f, 1f),
            new (15.35f, 2, 1f),
            new (12.3f, 2, 1f),
            new (9.3f, 2, 1f),
            new (6.3f, -3, 1f),
            new (-8.85f, 12.4f, 1f),
            new (6.3f, 2.5f, 1f)
        };

        private static readonly Vector3[] FungleMapTreasures = {
            Vector3.zero, // placeholder for case 0 that it has no use
            new (-11f, 12.75f, -1f),
            new (-11.5f, 8.5f, -1f),
            new (-17.35f, 7.15f, -1f),
            new (-22.15f, -2.25f, -1f),
            new (-15.5f, -2.575f, -1f),
            new (-22.75f, -7.25f, -1f),
            new (-8.85f, -14.25f, -1f),
            new (-3.3f, -10.5f, -1f),
            new (10.75f, -15f, -1f),
            new (7.7f, -10f, -1f),
            new (0.55f, -1.75f, -1f),
            new (2.25f, 4.25f, -1f),
            new (2.175f, 1.5f, -1f),
            new (23.25f, -7.75f, -1f),
            new (10f, 1.15f, -1f),
            new (15.25f, 4.75f, -1f),
            new (13.25f, 10.25f, -1f),
            new (20.25f, 7.5f, -1f),
            new (24.4f, 14.45f, -1f)
        };

        private static readonly Vector3[] SubmergedMapTreasures = {
            Vector3.zero, // placeholder for case 0 that it has no use
            new (-12.85f, -27.75f, -1f),
            new (-14.75f, -34.25f, -1f),
            new (-11f, -39f, -1f),
            new (-6.85f, -42.75f, -1f),
            new (6.5f, -39.5f, -1f),
            new (5f, -33.7f, -1f),
            new (0.2f, -33.9f, -1f),
            new (-4.25f, -33.5f, -1f),
            new (7.75f, -23.5f, -1f),
            new (13, -25.25f, -1f),
            new (12.85f, -32f, -1f),
            new (4.35f, 8.35f, -1f),
            new (14.25f, 24.35f, -1f),
            new (5.55f, 31.25f, -1f),
            new (0f, 33.75f, -1f),
            new (-11.5f, 30.5f, -1f),
            new (-12.5f, 15.5f, -1f),
            new (-6.75f, 10f, -1f),
            new (-6.65f, 15.25f, -1f),
            new (0.65f, 10f, -1f),
            new (-1.8f, 12f, -1f),
            new (-6.5f, 28.5f, -1f)
        };


        public Treasure(float chestDuration) {

            this.color = new Color(1f, 1f, 1f, 1f);

            treasure = new GameObject("Treasure");
            treasure.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    if (activatedSensei) {
                        position = SenseiMapTreasures[TreasureHunter.randomSpawn];
                    }
                    else if (activatedDleks) {
                        position = DleksMapTreasures[TreasureHunter.randomSpawn];
                    }
                    else {
                        position = SkeldMapTreasures[TreasureHunter.randomSpawn];
                    }
                    break;
                case 1:
                    position = MiraMapTreasures[TreasureHunter.randomSpawn];
                    break;
                case 2:
                    position = PolusMapTreasures[TreasureHunter.randomSpawn];
                    break;
                case 3:
                    position = DleksMapTreasures[TreasureHunter.randomSpawn];
                    break;
                case 4:
                    position = AirshipMapTreasures[TreasureHunter.randomSpawn];
                    break;
                case 5:
                    position = FungleMapTreasures[TreasureHunter.randomSpawn];
                    break;
                case 6:
                    position = SubmergedMapTreasures[TreasureHunter.randomSpawn];
                    break;
            }
            treasure.transform.position = position;
            treasure.transform.localPosition = position;
            treasure.transform.SetParent(TreasureHunter.treasureHunter.transform.parent);

            spriteRenderer = treasure.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = getTreasureSprite();
            spriteRenderer.color = color;

            var playerIsTreasureHunter = PlayerInCache.LocalPlayer.PlayerControl == TreasureHunter.treasureHunter;
            treasure.SetActive(playerIsTreasureHunter);

            treasures.Add(this);

            HudManager.Instance.StartCoroutine(Effects.Lerp(chestDuration, new Action<float>((p) => {

                var player = PlayerInCache.LocalPlayer.PlayerControl;
                if (!touchedPlayer && Vector2.Distance(player.transform.position, treasure.transform.position) < 0.5f && player == TreasureHunter.treasureHunter && !player.Data.IsDead) {
                    touchedPlayer = true;
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.CollectedTreasure, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.collectedTreasure();
                    treasure.SetActive(false);
                }

            })));

        }
    }
}