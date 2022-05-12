using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using static LasMonjas.LasMonjas;
using LasMonjas.Objects;
using static LasMonjas.MapOptions;
using System.Collections;
using System;
using System.Text;
using UnityEngine;
using System.Reflection;
using LasMonjas.Core;

namespace LasMonjas.Patches {
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    class ExileControllerBeginPatch {
        public static void Prefix(ExileController __instance, [HarmonyArgument(0)]ref GameData.PlayerInfo exiled, [HarmonyArgument(1)]bool tie) {

            if (PlayerControl.GameOptions.MapId == 5) {

                // Reset custom button timers where necessary
                CustomButton.MeetingEndedUpdate();
                
                //Change Music based on alive player number on submerged
                MessageWriter musicwriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                musicwriter.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(musicwriter);
                RPCProcedure.changeMusic(2);


                // Forensic spawn ghosts after meeting
                if (Forensic.forensic != null && PlayerControl.LocalPlayer == Forensic.forensic) {
                    if (Forensic.souls != null) {
                        foreach (SpriteRenderer sr in Forensic.souls) UnityEngine.Object.Destroy(sr.gameObject);
                        Forensic.souls = new List<SpriteRenderer>();
                    }

                    if (Forensic.featureDeadBodies != null) {
                        foreach ((DeadPlayer db, Vector3 ps) in Forensic.featureDeadBodies) {
                            GameObject s = new GameObject();
                            s.transform.position = ps;
                            s.layer = 5;
                            var rend = s.AddComponent<SpriteRenderer>();
                            s.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                            rend.sprite = Forensic.getSoulSprite();
                            Forensic.souls.Add(rend);
                        }
                        Forensic.deadBodies = Forensic.featureDeadBodies;
                        Forensic.featureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
                    }
                }

                // BountyHunter exile if the exiled player was the bounty hunter target
                if (BountyHunter.bountyhunter != null && !BountyHunter.bountyhunter.Data.IsDead && exiled.PlayerId == BountyHunter.hasToKill.PlayerId) {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedExilePlayer, Hazel.SendOption.Reliable, -1);
                    writer.Write(BountyHunter.bountyhunter.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.uncheckedExilePlayer(BountyHunter.bountyhunter.PlayerId);
                }

                // Treasure Hunter reset button after meeting
                if (TreasureHunter.treasureHunter != null && !TreasureHunter.treasureHunter.Data.IsDead) {
                    TreasureHunter.canPlace = true;
                }

                // Pyromaniac deactivate dead players icons
                if (Pyromaniac.pyromaniac != null && Pyromaniac.pyromaniac == PlayerControl.LocalPlayer) {
                    int visibleCounter = 0;
                    Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.localPosition.x, HudManager.Instance.UseButton.transform.localPosition.y, HudManager.Instance.UseButton.transform.localPosition.z);
                    bottomLeft += new Vector3(-0.25f, -0.25f, 0);
                    foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                        if (!MapOptions.playerIcons.ContainsKey(p.PlayerId)) continue;
                        if (p.Data.IsDead || p.Data.Disconnected) {
                            MapOptions.playerIcons[p.PlayerId].gameObject.SetActive(false);
                        }
                        else {
                            MapOptions.playerIcons[p.PlayerId].transform.localPosition = bottomLeft + Vector3.right * visibleCounter * 0.35f;
                            visibleCounter++;
                        }
                    }
                }

                // Sleuth reset deadBodyPositions after meeting
                Sleuth.deadBodyPositions = new List<Vector3>();

                // Show roles after meeting for dead players if the option is active
                if (MapOptions.ghostsSeeRoles && howmanygamemodesareon != 1) {
                    foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                        if (p == PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data.IsDead) {
                            Transform playerInfoTransform = p.nameText.transform.parent.FindChild("Info");
                            TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                            if (playerInfo == null) {
                                playerInfo = UnityEngine.Object.Instantiate(p.nameText, p.nameText.transform.parent);
                                playerInfo.transform.localPosition += Vector3.up * 0.5f;
                                playerInfo.fontSize *= 0.75f;
                                playerInfo.gameObject.name = "Info";
                            }

                            string roleNames = RoleInfo.GetRolesString(p, true);

                            string playerInfoText = "";
                            if (PlayerControl.LocalPlayer.Data.IsDead) {
                                playerInfoText = $"{roleNames}";
                            }

                            playerInfo.text = playerInfoText;
                            playerInfo.gameObject.SetActive(p.Visible);
                        }
                    }
                }

                // Cheater exile if the cheated player was innocent, rebels and neutrals counts as impostors
                if (Cheater.cheater != null && !Cheater.cheater.Data.IsDead) {
                    if (Cheater.usedCheat == true && exiled.PlayerId == Cheater.cheatedP1.PlayerId && !Cheater.cheatedP1.Data.Role.IsImpostor && Cheater.cheatedP1 != Renegade.renegade && Cheater.cheatedP1 != Minion.minion && Cheater.cheatedP1 != BountyHunter.bountyhunter && Cheater.cheatedP1 != Trapper.trapper && Cheater.cheatedP1 != Yinyanger.yinyanger && Cheater.cheatedP1 != Challenger.challenger && Cheater.cheatedP1 != Joker.joker && Cheater.cheatedP1 != RoleThief.rolethief && Cheater.cheatedP1 != Pyromaniac.pyromaniac && Cheater.cheatedP1 != TreasureHunter.treasureHunter && Cheater.cheatedP1 != Devourer.devourer) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedExilePlayer, Hazel.SendOption.Reliable, -1);
                        writer.Write(Cheater.cheater.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.uncheckedExilePlayer(Cheater.cheater.PlayerId);
                    }
                    else if (Cheater.usedCheat == true && exiled.PlayerId == Cheater.cheatedP2.PlayerId && !Cheater.cheatedP2.Data.Role.IsImpostor && Cheater.cheatedP2 != Renegade.renegade && Cheater.cheatedP2 != Minion.minion && Cheater.cheatedP2 != BountyHunter.bountyhunter && Cheater.cheatedP2 != Trapper.trapper && Cheater.cheatedP2 != Yinyanger.yinyanger && Cheater.cheatedP2 != Challenger.challenger && Cheater.cheatedP2 != Joker.joker && Cheater.cheatedP2 != RoleThief.rolethief && Cheater.cheatedP2 != Pyromaniac.pyromaniac && Cheater.cheatedP2 != TreasureHunter.treasureHunter && Cheater.cheatedP2 != Devourer.devourer) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedExilePlayer, Hazel.SendOption.Reliable, -1);
                        writer.Write(Cheater.cheater.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.uncheckedExilePlayer(Cheater.cheater.PlayerId);
                    }
                    Cheater.cheatedP1 = null;
                    Cheater.cheatedP2 = null;
                    Cheater.usedCheat = false;
                }

                // Yinyanger reset button after meeting
                if (Yinyanger.yinyanger != null && !Yinyanger.yinyanger.Data.IsDead) {
                    Yinyanger.usedYined = false;
                    Yinyanger.usedYanged = false;
                    Yinyanger.yinyedplayer = null;
                    Yinyanger.yangyedplayer = null;
                    Yinyanger.colision = false;
                }
            }
            
            // Sorcerer execute casted spells
            if (Sorcerer.sorcerer != null && Sorcerer.spelledPlayers != null && AmongUsClient.Instance.AmHost) {
                bool exiledIsSorcerer = exiled != null && exiled.PlayerId == Sorcerer.sorcerer.PlayerId;
                bool sorcererDiesWithExiledLover = exiled != null && Modifiers.existing() && (Modifiers.lover1.PlayerId == Sorcerer.sorcerer.PlayerId || Modifiers.lover2.PlayerId == Sorcerer.sorcerer.PlayerId) && (exiled.PlayerId == Modifiers.lover1.PlayerId || exiled.PlayerId == Modifiers.lover2.PlayerId);

                if ((sorcererDiesWithExiledLover || exiledIsSorcerer)) Sorcerer.spelledPlayers = new List<PlayerControl>();
                foreach (PlayerControl target in Sorcerer.spelledPlayers) {
                    if (target != null && !target.Data.IsDead && Helpers.checkMurderAttempt(Sorcerer.sorcerer, target, true) == MurderAttemptResult.PerformKill)
                    {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedExilePlayer, Hazel.SendOption.Reliable, -1);
                        writer.Write(target.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.uncheckedExilePlayer(target.PlayerId);
                    }
                }
            }
            Sorcerer.spelledPlayers = new List<PlayerControl>();

            // Welder vents seal after meeting
            foreach (Vent vent in MapOptions.ventsToSeal) {
                PowerTools.SpriteAnim animator = vent.GetComponent<PowerTools.SpriteAnim>();
                animator?.Stop();
                vent.EnterVentAnim = vent.ExitVentAnim = null;
                vent.myRend.sprite = animator == null ? Welder.getStaticVentSealedSprite() : Welder.getAnimatedVentSealedSprite();
                vent.myRend.color = Color.white;
                vent.name = "SealedVent_" + vent.name;
            }
            MapOptions.ventsToSeal = new List<Vent>();

            // Vigilant cameras activate after meeting
            var allCameras = ShipStatus.Instance.AllCameras.ToList();
            MapOptions.camerasToAdd.ForEach(camera => {
                camera.gameObject.SetActive(true);
                camera.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                allCameras.Add(camera);
            });
            ShipStatus.Instance.AllCameras = allCameras.ToArray();
            MapOptions.camerasToAdd = new List<SurvCamera>();
        }
    }

    [HarmonyPatch]
    class ExileControllerWrapUpPatch {

        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        class BaseExileControllerPatch {
            public static void Postfix(ExileController __instance) {
                WrapUpPostfix(__instance.exiled);
            }
        }
        
        [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
        class AirshipExileControllerPatch {
            public static void Postfix(AirshipExileController __instance) {
                WrapUpPostfix(__instance.exiled);
            }
        }

        static void WrapUpPostfix(GameData.PlayerInfo exiled) {
            // Kid win condition if exiled
            if (exiled != null && Kid.kid != null && Kid.kid.PlayerId == exiled.PlayerId) {
                Kid.triggerKidLose = true;
            }

            // Joker win condition if exiled
            else if (exiled != null && Joker.joker != null && Joker.joker.PlayerId == exiled.PlayerId) {
                Joker.triggerJokerWin = true;
            }

            if (PlayerControl.GameOptions.MapId != 5) {

                // Reset custom button timers where necessary
                CustomButton.MeetingEndedUpdate();

                // Pyromaniac deactivate dead players icons
                if (Pyromaniac.pyromaniac != null && Pyromaniac.pyromaniac == PlayerControl.LocalPlayer) {
                    int visibleCounter = 0;
                    Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.localPosition.x, HudManager.Instance.UseButton.transform.localPosition.y, HudManager.Instance.UseButton.transform.localPosition.z);
                    bottomLeft += new Vector3(-0.25f, -0.25f, 0);
                    foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                        if (!MapOptions.playerIcons.ContainsKey(p.PlayerId)) continue;
                        if (p.Data.IsDead || p.Data.Disconnected) {
                            MapOptions.playerIcons[p.PlayerId].gameObject.SetActive(false);
                        }
                        else {
                            MapOptions.playerIcons[p.PlayerId].transform.localPosition = bottomLeft + Vector3.right * visibleCounter * 0.35f;
                            visibleCounter++;
                        }
                    }
                }

                // Cheater exile if the cheated player was innocent, rebels and neutrals counts as impostors
                if (Cheater.cheater != null && !Cheater.cheater.Data.IsDead) {
                    if (Cheater.usedCheat == true && Cheater.cheatedP1.Data.IsDead && !Cheater.cheatedP1.Data.Role.IsImpostor && Cheater.cheatedP1 != Renegade.renegade && Cheater.cheatedP1 != Minion.minion && Cheater.cheatedP1 != BountyHunter.bountyhunter && Cheater.cheatedP1 != Trapper.trapper && Cheater.cheatedP1 != Yinyanger.yinyanger && Cheater.cheatedP1 != Challenger.challenger && Cheater.cheatedP1 != Joker.joker && Cheater.cheatedP1 != RoleThief.rolethief && Cheater.cheatedP1 != Pyromaniac.pyromaniac && Cheater.cheatedP1 != TreasureHunter.treasureHunter && Cheater.cheatedP1 != Devourer.devourer) {
                        Cheater.cheater.Exiled();
                    }
                    else if (Cheater.usedCheat == true && Cheater.cheatedP2.Data.IsDead && !Cheater.cheatedP2.Data.Role.IsImpostor && Cheater.cheatedP2 != Renegade.renegade && Cheater.cheatedP2 != Minion.minion && Cheater.cheatedP2 != BountyHunter.bountyhunter && Cheater.cheatedP2 != Trapper.trapper && Cheater.cheatedP2 != Yinyanger.yinyanger && Cheater.cheatedP2 != Challenger.challenger && Cheater.cheatedP2 != Joker.joker && Cheater.cheatedP2 != RoleThief.rolethief && Cheater.cheatedP2 != Pyromaniac.pyromaniac && Cheater.cheatedP2 != TreasureHunter.treasureHunter && Cheater.cheatedP2 != Devourer.devourer) {
                        Cheater.cheater.Exiled();
                    }
                    Cheater.cheatedP1 = null;
                    Cheater.cheatedP2 = null;
                    Cheater.usedCheat = false;
                }

                // BountyHunter exile if the exiled player was the bounty hunter target
                if (BountyHunter.bountyhunter != null && !BountyHunter.bountyhunter.Data.IsDead && BountyHunter.hasToKill.Data.IsDead) {
                    BountyHunter.bountyhunter.Exiled();
                }

                // Treasure Hunter reset button after meeting
                if (TreasureHunter.treasureHunter != null && !TreasureHunter.treasureHunter.Data.IsDead) {
                    TreasureHunter.canPlace = true;
                }

                // Yinyanger reset button after meeting
                if (Yinyanger.yinyanger != null && !Yinyanger.yinyanger.Data.IsDead) {
                    Yinyanger.usedYined = false;
                    Yinyanger.usedYanged = false;
                    Yinyanger.yinyedplayer = null;
                    Yinyanger.yangyedplayer = null;
                    Yinyanger.colision = false;
                }

                // Forensic spawn ghosts after meeting
                if (Forensic.forensic != null && PlayerControl.LocalPlayer == Forensic.forensic) {
                    if (Forensic.souls != null) {
                        foreach (SpriteRenderer sr in Forensic.souls) UnityEngine.Object.Destroy(sr.gameObject);
                        Forensic.souls = new List<SpriteRenderer>();
                    }

                    if (Forensic.featureDeadBodies != null) {
                        foreach ((DeadPlayer db, Vector3 ps) in Forensic.featureDeadBodies) {
                            GameObject s = new GameObject();
                            s.transform.position = ps;
                            s.layer = 5;
                            var rend = s.AddComponent<SpriteRenderer>();
                            s.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                            rend.sprite = Forensic.getSoulSprite();
                            Forensic.souls.Add(rend);
                        }
                        Forensic.deadBodies = Forensic.featureDeadBodies;
                        Forensic.featureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
                    }
                }

                // Sleuth reset deadBodyPositions after meeting
                Sleuth.deadBodyPositions = new List<Vector3>();

                //Change Music based on alive player number if not on submerged
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writer.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.changeMusic(2);

                // Show roles after meeting for dead players if the option is active
                if (MapOptions.ghostsSeeRoles && howmanygamemodesareon != 1) {
                    foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                        if (p == PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data.IsDead) {
                            Transform playerInfoTransform = p.nameText.transform.parent.FindChild("Info");
                            TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                            if (playerInfo == null) {
                                playerInfo = UnityEngine.Object.Instantiate(p.nameText, p.nameText.transform.parent);
                                playerInfo.transform.localPosition += Vector3.up * 0.5f;
                                playerInfo.fontSize *= 0.75f;
                                playerInfo.gameObject.name = "Info";
                            }

                            string roleNames = RoleInfo.GetRolesString(p, true);

                            string playerInfoText = "";
                            if (PlayerControl.LocalPlayer.Data.IsDead) {
                                playerInfoText = $"{roleNames}";
                            }

                            playerInfo.text = playerInfoText;
                            playerInfo.gameObject.SetActive(p.Visible);
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), new Type[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    class ExileControllerMessagePatch
    {
        static void Postfix(ref string __result, [HarmonyArgument(0)] StringNames id) {
            try {
                if (ExileController.Instance != null && ExileController.Instance.exiled != null) {
                    PlayerControl player = Helpers.playerById(ExileController.Instance.exiled.Object.PlayerId);
                    if (player == null) return;
                    // Exile role text
                    if (id == StringNames.ExileTextPN || id == StringNames.ExileTextSN || id == StringNames.ExileTextPP || id == StringNames.ExileTextSP) {
                        __result = player.Data.PlayerName + " was the " + String.Join(" ", RoleInfo.getRoleInfoForPlayer(player).Select(x => x.name).ToArray());
                    }
                    // Custom text on Joker exile instead remaining impostors
                    if (id == StringNames.ImpostorsRemainP || id == StringNames.ImpostorsRemainS) {
                        if (Joker.joker != null && player.PlayerId == Joker.joker.PlayerId) __result = "You thought I was the Impostor but it was me, Joker!";
                    }
                    // Custom text on Kid exile instead remaining impostors
                    if (id == StringNames.ImpostorsRemainP || id == StringNames.ImpostorsRemainS) {
                        if (Kid.kid != null && player.PlayerId == Kid.kid.PlayerId) __result = "That's all folks!";
                    }
                }
            }
            catch {
                // prevent softlock game if someone leaves while exiling
            }
        }
    }
}