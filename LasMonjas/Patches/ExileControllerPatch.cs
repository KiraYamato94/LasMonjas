using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using static LasMonjas.LasMonjas;
using LasMonjas.Objects;
using System;
using UnityEngine;
using LasMonjas.Core;
using static LasMonjas.GameHistory;

namespace LasMonjas.Patches {
    [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
    [HarmonyPriority(Priority.First)]
    class ExileControllerBeginPatch {
        public static GameData.PlayerInfo lastExiled; 
        public static void Prefix(ExileController __instance, [HarmonyArgument(0)]ref GameData.PlayerInfo exiled, [HarmonyArgument(1)]bool tie) {
            lastExiled = exiled;

            // Sorcerer execute casted spells
            if (Sorcerer.sorcerer != null && Sorcerer.spelledPlayers != null && AmongUsClient.Instance.AmHost) {
                bool exiledIsSorcerer = exiled != null && exiled.PlayerId == Sorcerer.sorcerer.PlayerId;
                bool sorcererDiesWithExiledLover = exiled != null && Modifiers.existing() && (Modifiers.lover1.PlayerId == Sorcerer.sorcerer.PlayerId || Modifiers.lover2.PlayerId == Sorcerer.sorcerer.PlayerId) && (exiled.PlayerId == Modifiers.lover1.PlayerId || exiled.PlayerId == Modifiers.lover2.PlayerId);

                if ((sorcererDiesWithExiledLover || exiledIsSorcerer)) Sorcerer.spelledPlayers = new List<PlayerControl>();
                foreach (PlayerControl target in Sorcerer.spelledPlayers) {
                    if (target != null && !target.Data.IsDead && Helpers.checkMurderAttempt(Sorcerer.sorcerer, target, true) == MurderAttemptResult.PerformKill)
                    {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedExilePlayer, Hazel.SendOption.Reliable, -1);
                        writer.Write(target.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.uncheckedExilePlayer(target.PlayerId);
                    }
                }
            }
            Sorcerer.spelledPlayers = new List<PlayerControl>();

            // Hypnotist Spirals activate after meeting
            if (Hypnotist.hypnotist != null && HypnotistSpiral.hypnotistSpirals.Count != 0) {
                HypnotistSpiral.activateSpirals();
            }

            // Plumber make vents
            if (Plumber.currentVents == Plumber.maxVents && !Plumber.madeVents) {
                var ventId = ShipStatus.Instance.AllVents.Select(x => x.Id).Max() + 1;
                var allVents = ShipStatus.Instance.AllVents.ToList();
                for (var i = 0; i < Plumber.Vents.Count - 1; i++) {
                    var a = Plumber.Vents[i];
                    var b = Plumber.Vents[i + 1];
                    a.Right = b;
                    b.Left = a;
                }
                foreach (Vent vent in Plumber.Vents) {
                    vent.Center = null;
                    vent.Id = ventId;
                    vent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    vent.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    allVents.Add(vent);
                    ventId += 1;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                        vent.gameObject.GetComponent<CircleCollider2D>().enabled = true;
                    }
                }
                ShipStatus.Instance.AllVents = allVents.ToArray();
                Plumber.Vents.First().Left = Plumber.Vents.Last();
                Plumber.Vents.Last().Right = Plumber.Vents.First();
                Plumber.madeVents = true;
            }

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

            // Engineer Traps activate after meeting
            if (Engineer.engineer != null && EngineerTrap.engineerTraps.Count != 0) {
                EngineerTrap.activateTraps();
            }

            // Reset Puppeteer morph
            if (Puppeteer.puppeteer != null) {
                Puppeteer.Reset();
            }

            // Run a postfix on submerged exile cutscene
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                ExileControllerWrapUpPatch.WrapUpPostfix(ExileControllerBeginPatch.lastExiled);
            }
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

        public static void WrapUpPostfix(GameData.PlayerInfo exiled) {
            // Kid win condition if exiled
            if (exiled != null && Kid.kid != null && Kid.kid.PlayerId == exiled.PlayerId) {
                Kid.triggerKidLose = true;
            }

            // Joker win condition if exiled
            else if (exiled != null && Joker.joker != null && Joker.joker.PlayerId == exiled.PlayerId) {
                Joker.triggerJokerWin = true;
            }

            // Reset custom button timers where necessary
            CustomButton.MeetingEndedUpdate();

            // Librarian reset target
            if (Librarian.librarian != null && Librarian.targetLibrary != null) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ResetSilenced, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.resetSilenced();
            }

            // BountyHunter exile if the exiled player was the bounty hunter target
            if (BountyHunter.bountyhunter != null && !BountyHunter.bountyhunter.Data.IsDead && BountyHunter.hasToKill.Data.IsDead) {
                BountyHunter.bountyhunter.Exiled();
            }

            // Yinyanger reset button after meeting
            if (Yinyanger.yinyanger != null && !Yinyanger.yinyanger.Data.IsDead) {
                Yinyanger.usedYined = false;
                Yinyanger.usedYanged = false;
                Yinyanger.yinyedplayer = null;
                Yinyanger.yangyedplayer = null;
                Yinyanger.colision = false;
            }

            // Yandere rampage mode
            if (exiled != null && Yandere.yandere != null && Yandere.target != null && Yandere.target.PlayerId == exiled.PlayerId) {
                Yandere.rampageMode = true;
            }

            // Pyromaniac deactivate dead players icons
            if (Pyromaniac.pyromaniac != null && Pyromaniac.pyromaniac == PlayerInCache.LocalPlayer.PlayerControl) {
                int visibleCounter = 0;
                Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.parent.localPosition.x, HudManager.Instance.UseButton.transform.parent.localPosition.y, HudManager.Instance.UseButton.transform.parent.localPosition.z);
                bottomLeft += new Vector3(-0.25f, -0.25f, 0);
                foreach (PlayerControl p in PlayerInCache.AllPlayers) {
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

            // Poisoner deactivate dead poolable players
            if (Poisoner.poisoner != null && Poisoner.poisoner == PlayerInCache.LocalPlayer.PlayerControl) {
                int visibleCounter = 0;
                Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.parent.localPosition.x, HudManager.Instance.UseButton.transform.parent.localPosition.y, HudManager.Instance.UseButton.transform.parent.localPosition.z);
                bottomLeft += new Vector3(-0.25f, -0.25f, 0);
                foreach (PlayerControl p in PlayerInCache.AllPlayers) {
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

            // Exiler win condition
            if (exiled != null && Exiler.exiler != null && Exiler.target != null && Exiler.target.PlayerId == exiled.PlayerId && !Exiler.exiler.Data.IsDead) {
                Exiler.triggerExilerWin = true;
            }

            // Captain reset specialTarget
            if (Captain.captain != null && !Captain.captain.Data.IsDead && Captain.usedSpecialVote) {
                if (Captain.specialVoteTarget != null && Captain.specialVoteTarget.Data.IsDead && !Captain.specialVoteTarget.Data.Role.IsImpostor && Captain.specialVoteTarget != Renegade.renegade && Captain.specialVoteTarget != Minion.minion && Captain.specialVoteTarget != BountyHunter.bountyhunter && Captain.specialVoteTarget != Trapper.trapper && Captain.specialVoteTarget != Yinyanger.yinyanger && Captain.specialVoteTarget != Challenger.challenger && Captain.specialVoteTarget != Ninja.ninja && Captain.specialVoteTarget != Berserker.berserker && Captain.specialVoteTarget != Yandere.yandere && Captain.specialVoteTarget != Stranded.stranded && Captain.specialVoteTarget != Monja.monja && Captain.specialVoteTarget != Joker.joker && Captain.specialVoteTarget != RoleThief.rolethief && Captain.specialVoteTarget != Pyromaniac.pyromaniac && Captain.specialVoteTarget != TreasureHunter.treasureHunter && Captain.specialVoteTarget != Devourer.devourer && Captain.specialVoteTarget != Poisoner.poisoner && Captain.specialVoteTarget != Puppeteer.puppeteer && Captain.specialVoteTarget != Exiler.exiler && Captain.specialVoteTarget != Amnesiac.amnesiac && Captain.specialVoteTarget != Seeker.seeker) {
                    Captain.captain.Exiled();
                }
                Captain.specialVoteTargetPlayerId = byte.MaxValue;
                Captain.specialVoteTarget = null;
            }

            // Forensic spawn ghosts after meeting
            if (Forensic.forensic != null && PlayerInCache.LocalPlayer.PlayerControl == Forensic.forensic) {
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

            // Squire reset shielded if exiled
            if (Squire.resetShieldAfterMeeting || exiled != null && Squire.squire != null && Squire.shielded != null && Squire.squire.PlayerId == exiled.PlayerId) {
                Squire.shielded = null;
            }

            // Cheater exile if the cheated player was innocent, rebels and neutrals counts as impostors
            if (Cheater.cheater != null && !Cheater.cheater.Data.IsDead) {
                if (Cheater.usedCheat == true && Cheater.cheatedP1.Data.IsDead && !Cheater.cheatedP1.Data.Role.IsImpostor && Cheater.cheatedP1 != Renegade.renegade && Cheater.cheatedP1 != Minion.minion && Cheater.cheatedP1 != BountyHunter.bountyhunter && Cheater.cheatedP1 != Trapper.trapper && Cheater.cheatedP1 != Yinyanger.yinyanger && Cheater.cheatedP1 != Challenger.challenger && Cheater.cheatedP1 != Ninja.ninja && Cheater.cheatedP1 != Berserker.berserker && Cheater.cheatedP1 != Yandere.yandere && Cheater.cheatedP1 != Stranded.stranded && Cheater.cheatedP1 != Monja.monja && Cheater.cheatedP1 != Joker.joker && Cheater.cheatedP1 != RoleThief.rolethief && Cheater.cheatedP1 != Pyromaniac.pyromaniac && Cheater.cheatedP1 != TreasureHunter.treasureHunter && Cheater.cheatedP1 != Devourer.devourer && Cheater.cheatedP1 != Poisoner.poisoner && Cheater.cheatedP1 != Puppeteer.puppeteer && Cheater.cheatedP1 != Exiler.exiler && Cheater.cheatedP1 != Amnesiac.amnesiac && Cheater.cheatedP1 != Seeker.seeker) {
                    Cheater.cheater.Exiled();
                }
                else if (Cheater.usedCheat == true && Cheater.cheatedP2.Data.IsDead && !Cheater.cheatedP2.Data.Role.IsImpostor && Cheater.cheatedP2 != Renegade.renegade && Cheater.cheatedP2 != Minion.minion && Cheater.cheatedP2 != BountyHunter.bountyhunter && Cheater.cheatedP2 != Trapper.trapper && Cheater.cheatedP2 != Yinyanger.yinyanger && Cheater.cheatedP2 != Challenger.challenger && Cheater.cheatedP2 != Ninja.ninja && Cheater.cheatedP2 != Berserker.berserker && Cheater.cheatedP2 != Yandere.yandere && Cheater.cheatedP2 != Stranded.stranded && Cheater.cheatedP2 != Monja.monja && Cheater.cheatedP2 != Joker.joker && Cheater.cheatedP2 != RoleThief.rolethief && Cheater.cheatedP2 != Pyromaniac.pyromaniac && Cheater.cheatedP2 != TreasureHunter.treasureHunter && Cheater.cheatedP2 != Devourer.devourer && Cheater.cheatedP2 != Poisoner.poisoner && Cheater.cheatedP2 != Puppeteer.puppeteer && Cheater.cheatedP2 != Exiler.exiler && Cheater.cheatedP2 != Amnesiac.amnesiac && Cheater.cheatedP2 != Seeker.seeker) {
                    Cheater.cheater.Exiled();
                }
                Cheater.cheatedP1 = null;
                Cheater.cheatedP2 = null;
                Cheater.usedCheat = false;
            }

            // Sleuth reset deadBodyPositions after meeting
            Sleuth.deadBodyPositions = new List<Vector3>();

            //Change Music based on alive player number if not on submerged
            MessageWriter musicwriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
            musicwriter.Write(2);
            AmongUsClient.Instance.FinishRpcImmediately(musicwriter);
            RPCProcedure.changeMusic(2);

            // Show roles after meeting for dead players if the option is active
            if (MapOptions.ghostsSeeRoles && gameType <= 1) {
                foreach (PlayerControl p in PlayerInCache.AllPlayers) {
                    if (p == PlayerInCache.LocalPlayer.PlayerControl || PlayerInCache.LocalPlayer.Data.IsDead) {
                        Transform playerInfoTransform = p.cosmetics.nameText.transform.parent.FindChild("Info");
                        TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                        if (playerInfo == null) {
                            playerInfo = UnityEngine.Object.Instantiate(p.cosmetics.nameText, p.cosmetics.nameText.transform.parent);
                            playerInfo.transform.localPosition += Vector3.up * 0.5f;
                            playerInfo.fontSize *= 0.75f;
                            playerInfo.gameObject.name = "Info";
                        }

                        string roleNames = RoleInfo.GetRolesString(p, true);

                        string playerInfoText = "";
                        if (PlayerInCache.LocalPlayer.Data.IsDead) {
                            playerInfoText = $"{roleNames}";
                        }

                        playerInfo.text = playerInfoText;
                        playerInfo.gameObject.SetActive(p.Visible);
                    }
                }
            }

            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                if (player == PlayerInCache.LocalPlayer.PlayerControl) {
                    HudManager.Instance.AbilityButton.Hide();
                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == player.PlayerId).FirstOrDefault();
                    if (deadPlayerEntry != null && player.Data.IsDead) {
                        HudManager.Instance.AbilityButton.Show();
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
                        __result = player.Data.PlayerName + Language.exileControllerTexts[0] + String.Join(" ", RoleInfo.getRoleInfoForPlayer(player).Select(x => x.name).ToArray());
                    }
                    // Custom text on Joker exile instead remaining impostors
                    if (id == StringNames.ImpostorsRemainP || id == StringNames.ImpostorsRemainS) {
                        if (Joker.joker != null && player.PlayerId == Joker.joker.PlayerId) __result = Language.exileControllerTexts[1];
                    }
                    // Custom text on Kid exile instead remaining impostors
                    if (id == StringNames.ImpostorsRemainP || id == StringNames.ImpostorsRemainS) {
                        if (Kid.kid != null && player.PlayerId == Kid.kid.PlayerId) __result = Language.exileControllerTexts[2];
                    }
                }
            }
            catch {
                // prevent softlock game if someone leaves while exiling
            }
        }
    }
}