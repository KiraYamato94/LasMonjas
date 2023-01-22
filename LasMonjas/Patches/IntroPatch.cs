using HarmonyLib;
using System;
using static LasMonjas.LasMonjas;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LasMonjas.Objects;
using LasMonjas.Core;
using Hazel;
using AmongUs.GameOptions;

namespace LasMonjas.Patches
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    class IntroCutsceneOnDestroyPatch
    {
        public static void Prefix(IntroCutscene __instance) {

            Helpers.activateSenseiMap();          

            GameObject allulfitti = GameObject.Instantiate(CustomMain.customAssets.allulfitti, PlayerControl.LocalPlayer.transform.parent);
            GameObject allulbanner = GameObject.Instantiate(CustomMain.customAssets.allulbanner, PlayerControl.LocalPlayer.transform.parent);
            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    if (activatedSensei) {
                        allulfitti.transform.position = new Vector3(-6.8f, -5.2f, 0.5f);
                        allulbanner.SetActive(false);
                    }
                    else {
                        allulfitti.transform.position = new Vector3(-13.75f, 2, 0.5f);
                        allulbanner.transform.position = new Vector3(-0.75f, -3.5f, 0.5f);
                    }
                    break;
                case 1:
                    allulfitti.transform.position = new Vector3(6.2f, -0.25f, 0.5f);
                    allulbanner.transform.position = new Vector3(25.5f, 4.75f, 0.5f);
                    break;
                case 2:
                    allulfitti.transform.position = new Vector3(19.3f, -13.3f, 0.5f);
                    allulbanner.transform.position = new Vector3(19.8f, -19.25f, 0.5f);
                    break;
                case 3:
                    allulfitti.transform.position = new Vector3(13.75f, 2, 0.5f);
                    allulbanner.transform.position = new Vector3(13.75f, 2, 0.5f);
                    break;
                case 4:
                    allulfitti.transform.position = new Vector3(7.75f, 7.5f, 0.5f);
                    allulbanner.transform.position = new Vector3(11f, 16.75f, 0.5f);
                    break;
                case 5:
                    allulfitti.transform.position = new Vector3(6f, 22.65f, -0.5f);
                    allulbanner.transform.position = new Vector3(3.85f, -19.35f, -0.5f);
                    break;
            }

            // Generate alive player icons for Pyromaniac and Poisoner
            int playerCounter = 0;
            if (PlayerControl.LocalPlayer != null && HudManager.Instance != null && howmanygamemodesareon != 1) {
                Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.parent.localPosition.x, HudManager.Instance.UseButton.transform.parent.localPosition.y, HudManager.Instance.UseButton.transform.parent.localPosition.z);
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                    GameData.PlayerInfo data = p.Data;
                    PoolablePlayer player = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, HudManager.Instance.transform);
                    p.SetPlayerMaterialColors(player.cosmetics.currentBodySprite.BodySprite);
                    player.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
                    player.cosmetics.hat.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
                    player.cosmetics.visor.SetVisor(data.DefaultOutfit.VisorId, data.DefaultOutfit.ColorId);
                    player.cosmetics.nameText.text = data.PlayerName;
                    player.SetFlipX(true);
                    MapOptions.playerIcons[p.PlayerId] = player;

                    if (PlayerControl.LocalPlayer == Pyromaniac.pyromaniac && p != Pyromaniac.pyromaniac || PlayerControl.LocalPlayer == Poisoner.poisoner && p != Poisoner.poisoner) {
                        player.transform.localPosition = bottomLeft + new Vector3(-0.25f, -0.25f, 0) + Vector3.right * playerCounter++ * 0.35f;
                        player.transform.localScale = Vector3.one * 0.2f;
                        player.setSemiTransparent(true);
                        player.gameObject.SetActive(true);
                    }
                    else {
                        player.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    [HarmonyPatch]
    class IntroPatch
    {
        public static void setupIntroTeamIcons(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {

            SoundManager.Instance.StopSound(CustomMain.customAssets.lobbyMusic);

            howmanygamemodesareon = 0;
            whichgamemodeHUD = 0;

            if (CaptureTheFlag.captureTheFlagMode) {
                howmanygamemodesareon += 1;
                whichgamemodeHUD = 1;
            }
            if (PoliceAndThief.policeAndThiefMode) {
                howmanygamemodesareon += 1;
                whichgamemodeHUD = 2;
            }
            if (KingOfTheHill.kingOfTheHillMode) {
                howmanygamemodesareon += 1;
                whichgamemodeHUD = 3;
            }
            if (HotPotato.hotPotatoMode) {
                howmanygamemodesareon += 1;
                whichgamemodeHUD = 4;
            }
            if (ZombieLaboratory.zombieLaboratoryMode) {
                howmanygamemodesareon += 1;
                whichgamemodeHUD = 5;
            }
            if (BattleRoyale.battleRoyaleMode) {
                howmanygamemodesareon += 1;
                whichgamemodeHUD = 6;
                howmanyBattleRoyaleplayers = 0;
            }

            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                if (howmanygamemodesareon == 1) {
                    if (CaptureTheFlag.captureTheFlagMode) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.captureTheFlagMusic, true, 25f);
                        // Intro capture the flag teams
                        var redTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        if (PlayerControl.LocalPlayer == CaptureTheFlag.redplayer01 || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer02 || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer03 || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer04 || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer05 || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer06 || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer07) {
                            redTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = redTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        var blueTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        if (PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer01 || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer02 || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer03 || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer04 || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer05 || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer06 || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer07) {
                            blueTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = blueTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                            var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            greyTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = greyTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                        }
                    }
                    else if (PoliceAndThief.policeAndThiefMode) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.policeAndThiefMusic, true, 25f);
                        // Intro police and thiefs teams
                        var thiefTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        if (PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer01 || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer02 || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer03 || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer04 || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer05 || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer06 || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer07 || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer08 || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer09) {
                            thiefTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = thiefTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Impostor);
                        }
                        var policeTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        if (PlayerControl.LocalPlayer == PoliceAndThief.policeplayer01 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer02 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer03 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer04 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer05 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer06) {
                            policeTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = policeTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                    }
                    else if (KingOfTheHill.kingOfTheHillMode) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.kingOfTheHillMusic, true, 25f);
                        // Intro king of the hill teams
                        var greenTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        if (PlayerControl.LocalPlayer == KingOfTheHill.greenKingplayer || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer01 || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer02 || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer03 || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer04 || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer05 || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer06) {
                            greenTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = greenTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        var yellowTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        if (PlayerControl.LocalPlayer == KingOfTheHill.yellowKingplayer || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer01 || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer02 || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer03 || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer04 || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer05 || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer06) {
                            yellowTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = yellowTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                            var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            greyTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = greyTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                        }
                    }
                    else if (HotPotato.hotPotatoMode) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.hotPotatoMusic, true, 25f);
                        // Intro hot potato teams
                        if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                            var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            greyTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = greyTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Impostor);
                        }

                        var notPotatoTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        if (PlayerControl.LocalPlayer == HotPotato.notPotato01 || PlayerControl.LocalPlayer == HotPotato.notPotato02 || PlayerControl.LocalPlayer == HotPotato.notPotato03 || PlayerControl.LocalPlayer == HotPotato.notPotato04 || PlayerControl.LocalPlayer == HotPotato.notPotato05 || PlayerControl.LocalPlayer == HotPotato.notPotato06 || PlayerControl.LocalPlayer == HotPotato.notPotato07 || PlayerControl.LocalPlayer == HotPotato.notPotato08 || PlayerControl.LocalPlayer == HotPotato.notPotato09 || PlayerControl.LocalPlayer == HotPotato.notPotato10 || PlayerControl.LocalPlayer == HotPotato.notPotato11 || PlayerControl.LocalPlayer == HotPotato.notPotato12 || PlayerControl.LocalPlayer == HotPotato.notPotato13 || PlayerControl.LocalPlayer == HotPotato.notPotato14) {
                            notPotatoTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = notPotatoTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                    }
                    else if (ZombieLaboratory.zombieLaboratoryMode) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.zombieLaboratoryMusic, true, 25f);
                        // Intro zombie teams
                        if (PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer01) {
                            var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            greyTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = greyTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Impostor);
                        }

                        var survivorTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        if (PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer01 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer02 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer03 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer04 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer05 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer06 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer07 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer08 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer09 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer10 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer11 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer12 || PlayerControl.LocalPlayer == ZombieLaboratory.survivorPlayer13) {
                            survivorTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = survivorTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        if (PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer) {
                            survivorTeam.Add(PlayerControl.LocalPlayer);
                            yourTeam = survivorTeam;
                            PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Scientist);
                        }
                    }
                    else if (BattleRoyale.battleRoyaleMode) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.battleRoyaleMusic, true, 25f);
                        // Intro Battle Royale
                        if (BattleRoyale.matchType == 0) {
                            var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            if (PlayerControl.LocalPlayer == BattleRoyale.soloPlayer01 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer02 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer03 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer04 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer05 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer06 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer07 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer08 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer09 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer10 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer11 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer12 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer13 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer14 || PlayerControl.LocalPlayer == BattleRoyale.soloPlayer15) {
                                soloTeam.Add(PlayerControl.LocalPlayer);
                                yourTeam = soloTeam;
                                PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                            }
                        }
                        else {
                            var limeTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            if (PlayerControl.LocalPlayer == BattleRoyale.limePlayer01 || PlayerControl.LocalPlayer == BattleRoyale.limePlayer02 || PlayerControl.LocalPlayer == BattleRoyale.limePlayer03 || PlayerControl.LocalPlayer == BattleRoyale.limePlayer04 || PlayerControl.LocalPlayer == BattleRoyale.limePlayer05 || PlayerControl.LocalPlayer == BattleRoyale.limePlayer06 || PlayerControl.LocalPlayer == BattleRoyale.limePlayer07) {
                                limeTeam.Add(PlayerControl.LocalPlayer);
                                yourTeam = limeTeam;
                                PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                            }
                            var pinkTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            if (PlayerControl.LocalPlayer == BattleRoyale.pinkPlayer01 || PlayerControl.LocalPlayer == BattleRoyale.pinkPlayer02 || PlayerControl.LocalPlayer == BattleRoyale.pinkPlayer03 || PlayerControl.LocalPlayer == BattleRoyale.pinkPlayer04 || PlayerControl.LocalPlayer == BattleRoyale.pinkPlayer05 || PlayerControl.LocalPlayer == BattleRoyale.pinkPlayer06 || PlayerControl.LocalPlayer == BattleRoyale.pinkPlayer07) {
                                pinkTeam.Add(PlayerControl.LocalPlayer);
                                yourTeam = pinkTeam;
                                PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                            }
                            if (PlayerControl.LocalPlayer == BattleRoyale.serialKiller) {
                                var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                                greyTeam.Add(PlayerControl.LocalPlayer);
                                yourTeam = greyTeam;
                                PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                            }
                        }
                    }
                }
                else {
                    // Intro solo teams (rebels and neutrals)
                    if (PlayerControl.LocalPlayer == Joker.joker || PlayerControl.LocalPlayer == RoleThief.rolethief || PlayerControl.LocalPlayer == Pyromaniac.pyromaniac || PlayerControl.LocalPlayer == TreasureHunter.treasureHunter || PlayerControl.LocalPlayer == Devourer.devourer || PlayerControl.LocalPlayer == Poisoner.poisoner || PlayerControl.LocalPlayer == Puppeteer.puppeteer || PlayerControl.LocalPlayer == Exiler.exiler || PlayerControl.LocalPlayer == Amnesiac.amnesiac || PlayerControl.LocalPlayer == Seeker.seeker) {
                        var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        soloTeam.Add(PlayerControl.LocalPlayer);
                        yourTeam = soloTeam;
                        PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Engineer);
                    }

                    if (PlayerControl.LocalPlayer == Renegade.renegade || PlayerControl.LocalPlayer == BountyHunter.bountyhunter || PlayerControl.LocalPlayer == Trapper.trapper || PlayerControl.LocalPlayer == Yinyanger.yinyanger || PlayerControl.LocalPlayer == Challenger.challenger || PlayerControl.LocalPlayer == Ninja.ninja || PlayerControl.LocalPlayer == Berserker.berserker || PlayerControl.LocalPlayer == Yandere.yandere || PlayerControl.LocalPlayer == Stranded.stranded || PlayerControl.LocalPlayer == Monja.monja) {
                        var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                        soloTeam.Add(PlayerControl.LocalPlayer);
                        yourTeam = soloTeam;
                        PlayerControl.LocalPlayer.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                    }

                    if (MapOptions.activateMusic) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.tasksCalmMusic, true, 25f);
                    }

                    CaptureTheFlag.captureTheFlagMode = false;
                    PoliceAndThief.policeAndThiefMode = false;
                    KingOfTheHill.kingOfTheHillMode = false;
                    HotPotato.hotPotatoMode = false;
                    ZombieLaboratory.zombieLaboratoryMode = false;
                    BattleRoyale.battleRoyaleMode = false;
                }
            }
        }

        public static void setupIntroTeam(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
            List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer);
            RoleInfo roleInfo = infos.Where(info => info.roleId != RoleId.Lover).FirstOrDefault();
            if (roleInfo == null) return;
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                if (howmanygamemodesareon == 1) {
                    __instance.ImpostorText.text = "";
                    if (CaptureTheFlag.captureTheFlagMode) {
                        __instance.BackgroundBar.material.color = Sheriff.color;
                        __instance.TeamTitle.text = "Capture \nThe Flag";
                        __instance.TeamTitle.color = Sheriff.color;
                    }
                    else if (PoliceAndThief.policeAndThiefMode) {
                        __instance.BackgroundBar.material.color = Coward.color;
                        __instance.TeamTitle.text = "Police \nAnd Thieves";
                        __instance.TeamTitle.color = Coward.color;
                    }
                    else if (KingOfTheHill.kingOfTheHillMode) {
                        __instance.BackgroundBar.material.color = Squire.color;
                        __instance.TeamTitle.text = "King Of \nThe Hill";
                        __instance.TeamTitle.color = Squire.color;
                    }
                    else if (HotPotato.hotPotatoMode) {
                        __instance.BackgroundBar.material.color = Shy.color;
                        __instance.TeamTitle.text = "Hot Potato";
                        __instance.TeamTitle.color = Shy.color;
                    }
                    else if (ZombieLaboratory.zombieLaboratoryMode) {
                        __instance.BackgroundBar.material.color = Hunter.color;
                        __instance.TeamTitle.text = "Zombie \nLaboratory";
                        __instance.TeamTitle.color = Hunter.color;
                    }
                    else if (BattleRoyale.battleRoyaleMode) {
                        __instance.BackgroundBar.material.color = Sleuth.color;
                        __instance.TeamTitle.text = "Battle \nRoyale";
                        __instance.TeamTitle.color = Sleuth.color;
                    }
                }
                else {
                    if (roleInfo.isNeutral) {
                        var neutralColor = new Color32(128, 128, 128, 255);
                        __instance.BackgroundBar.material.color = neutralColor;
                        __instance.TeamTitle.text = "Neutral";
                        __instance.TeamTitle.color = neutralColor;
                    }
                    else if (roleInfo.isRebel) {
                        var rebelColor = new Color32(79, 125, 0, 255);
                        __instance.BackgroundBar.material.color = rebelColor;
                        __instance.TeamTitle.text = "Rebel";
                        __instance.TeamTitle.color = rebelColor;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
        class ShowRolePatch
        {
            public static void Postfix(IntroCutscene __instance) {
                
                if (!CustomOptionHolder.activateRoles.getBool()) return;

                List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer);
                RoleInfo roleInfo = infos.Where(info => info.roleId != RoleId.Lover).FirstOrDefault();

                Color color = new Color(__instance.YouAreText.color.r, __instance.YouAreText.color.g, __instance.YouAreText.color.b, 0f);
                __instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((t) => {

                    if (roleInfo != null) {
                        __instance.RoleText.text = roleInfo.name;
                        __instance.RoleBlurbText.text = roleInfo.introDescription;
                        color = roleInfo.color;
                    }

                    if (infos.Any(info => info.roleId == RoleId.Lover)) {
                        PlayerControl otherLover = PlayerControl.LocalPlayer == Modifiers.lover1 ? Modifiers.lover2 : Modifiers.lover1;
                        __instance.RoleBlurbText.text = PlayerControl.LocalPlayer.Data.Role.IsImpostor ? "<color=#FF00D1FF>Lover</color><color=#FF0000FF>stor</color>" : "<color=#FF00D1FF>Lover</color>";
                        __instance.RoleBlurbText.color = PlayerControl.LocalPlayer.Data.Role.IsImpostor ? Color.white : Modifiers.loverscolor;
                        __instance.ImpostorText.text = Helpers.cs(Modifiers.loverscolor, $"{Language.introTexts[0]} + {otherLover?.Data?.PlayerName ?? ""} ♥");
                        __instance.ImpostorText.gameObject.SetActive(true);
                        __instance.BackgroundBar.material.color = Modifiers.loverscolor;
                    }

                    color.a = t;
                    __instance.YouAreText.color = color;
                    __instance.RoleText.color = color;
                    __instance.RoleBlurbText.color = color;
                })));

                // Create the doorlog access from anywhere to the Vigilant on MiraHQ
                if (Vigilant.vigilantMira != null && GameOptionsManager.Instance.currentGameOptions.MapId == 1 && Vigilant.vigilantMira == PlayerControl.LocalPlayer && !Vigilant.createdDoorLog) {
                    GameObject vigilantDoorLog = GameObject.Find("SurvLogConsole");
                    Vigilant.doorLog = GameObject.Instantiate(vigilantDoorLog, Vigilant.vigilantMira.transform);
                    Vigilant.doorLog.name = "VigilantDoorLog";
                    Vigilant.doorLog.layer = 8; // Assign player layer to ignore collisions
                    Vigilant.doorLog.GetComponent<SpriteRenderer>().enabled = false;
                    Vigilant.doorLog.transform.localPosition = new Vector2(0, -0.5f);
                    Vigilant.createdDoorLog = true;
                }

                // Create the duel arena if there's a Challenger
                if (Challenger.challenger != null && PlayerControl.LocalPlayer != null && !createdduelarena) {
                    GameObject duelArena = GameObject.Instantiate(CustomMain.customAssets.challengerDuelArena, PlayerControl.LocalPlayer.transform.parent);
                    duelArena.name = "duelArena";
                    duelArena.transform.position = new Vector3(40, 0f, 1f);
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) { // Create another duel arena on submerged lower floor
                        GameObject lowerduelArena = GameObject.Instantiate(CustomMain.customAssets.challengerDuelArena, PlayerControl.LocalPlayer.transform.parent);
                        lowerduelArena.name = "lowerduelArena";
                        lowerduelArena.transform.position = new Vector3(40, -48.119f, 1f);
                    }
                    createdduelarena = true;
                }
                
                // Create the seaker arena if there's a Seeker
                if (Seeker.seeker != null && PlayerControl.LocalPlayer != null && !createdseekerarena) {
                    GameObject seekerArena = GameObject.Instantiate(CustomMain.customAssets.seekerArena, PlayerControl.LocalPlayer.transform.parent);
                    seekerArena.name = "seekerArena";
                    seekerArena.transform.position = new Vector3(-40, 0f, 1f);
                    Seeker.minigameArenaHideOnePointOne = seekerArena.transform.GetChild(0).transform.GetChild(0).gameObject;
                    Seeker.minigameArenaHideOnePointOne.transform.parent.transform.position = Seeker.minigameArenaHideOnePointOne.transform.parent.transform.position + new Vector3(0, 0, -2);
                    Seeker.minigameArenaHideOnePointTwo = seekerArena.transform.GetChild(0).transform.GetChild(1).gameObject;
                    Seeker.minigameArenaHideOnePointThree = seekerArena.transform.GetChild(0).transform.GetChild(2).gameObject;
                    Seeker.minigameArenaHideTwoPointOne = seekerArena.transform.GetChild(1).transform.GetChild(0).gameObject;
                    Seeker.minigameArenaHideTwoPointOne.transform.parent.transform.position = Seeker.minigameArenaHideTwoPointOne.transform.parent.transform.position + new Vector3(0, 0, -2);
                    Seeker.minigameArenaHideTwoPointTwo = seekerArena.transform.GetChild(1).transform.GetChild(1).gameObject;
                    Seeker.minigameArenaHideTwoPointThree = seekerArena.transform.GetChild(1).transform.GetChild(2).gameObject;
                    Seeker.minigameArenaHideThreePointOne = seekerArena.transform.GetChild(2).transform.GetChild(0).gameObject;
                    Seeker.minigameArenaHideThreePointOne.transform.parent.transform.position = Seeker.minigameArenaHideThreePointOne.transform.parent.transform.position + new Vector3(0, 0, -2);
                    Seeker.minigameArenaHideThreePointTwo = seekerArena.transform.GetChild(2).transform.GetChild(1).gameObject;
                    Seeker.minigameArenaHideThreePointThree = seekerArena.transform.GetChild(2).transform.GetChild(2).gameObject; 
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) { // Create another duel arena on submerged lower floor
                        GameObject lowerseekerArena = GameObject.Instantiate(CustomMain.customAssets.seekerArena, PlayerControl.LocalPlayer.transform.parent);
                        lowerseekerArena.name = "lowerseekerArena";
                        lowerseekerArena.transform.position = new Vector3(-40, -48.119f, 1f);
                        Seeker.lowerminigameArenaHideOnePointOne = lowerseekerArena.transform.GetChild(0).transform.GetChild(0).gameObject;
                        Seeker.lowerminigameArenaHideOnePointOne.transform.parent.transform.position = Seeker.lowerminigameArenaHideOnePointOne.transform.parent.transform.position + new Vector3(0, 0, -2);
                        Seeker.lowerminigameArenaHideOnePointTwo = lowerseekerArena.transform.GetChild(0).transform.GetChild(1).gameObject;
                        Seeker.lowerminigameArenaHideOnePointThree = lowerseekerArena.transform.GetChild(0).transform.GetChild(2).gameObject;
                        Seeker.lowerminigameArenaHideTwoPointOne = lowerseekerArena.transform.GetChild(1).transform.GetChild(0).gameObject;
                        Seeker.lowerminigameArenaHideTwoPointOne.transform.parent.transform.position = Seeker.lowerminigameArenaHideTwoPointOne.transform.parent.transform.position + new Vector3(0, 0, -2);
                        Seeker.lowerminigameArenaHideTwoPointTwo = lowerseekerArena.transform.GetChild(1).transform.GetChild(1).gameObject;
                        Seeker.lowerminigameArenaHideTwoPointThree = lowerseekerArena.transform.GetChild(1).transform.GetChild(2).gameObject;
                        Seeker.lowerminigameArenaHideThreePointOne = lowerseekerArena.transform.GetChild(2).transform.GetChild(0).gameObject;
                        Seeker.lowerminigameArenaHideThreePointOne.transform.parent.transform.position = Seeker.lowerminigameArenaHideThreePointOne.transform.parent.transform.position + new Vector3(0, 0, -2);
                        Seeker.lowerminigameArenaHideThreePointTwo = lowerseekerArena.transform.GetChild(2).transform.GetChild(1).gameObject;
                        Seeker.lowerminigameArenaHideThreePointThree = lowerseekerArena.transform.GetChild(2).transform.GetChild(2).gameObject;
                    }
                    createdseekerarena = true;
                }

                // Remove vitals use for TimeTraveler
                if (TimeTraveler.timeTraveler != null && PlayerControl.LocalPlayer == TimeTraveler.timeTraveler) {
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        case 2:
                            GameObject polusvitals = GameObject.Find("panel_vitals");
                            polusvitals.GetComponent<BoxCollider2D>().enabled = false;
                            break;
                        case 4:
                            GameObject airshipvitals = GameObject.Find("panel_vitals");
                            airshipvitals.GetComponent<CircleCollider2D>().enabled = false;
                            break;
                        case 5:
                            GameObject submergedvitals = GameObject.Find("panel_vitals(Clone)");
                            submergedvitals.GetComponent<CircleCollider2D>().enabled = false;
                            break;
                    }
                }

                // Submerged remove Chameleon special vent
                if (Chameleon.chameleon != null && PlayerControl.LocalPlayer == Chameleon.chameleon && GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                    GameObject vent = GameObject.Find("LowerCentralVent");
                    vent.GetComponent<BoxCollider2D>().enabled = false;
                }

                // Make object list for Hypnotist for traps
                if (Hypnotist.hypnotist != null && PlayerControl.LocalPlayer == Hypnotist.hypnotist) {
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        case 0:
                            GameObject skeldMedScanner = GameObject.Find("MedScanner");
                            Hypnotist.objectsCantPlaceTraps.Add(skeldMedScanner);
                            break;
                        case 1:
                            GameObject miraMedScanner = GameObject.Find("MedScanner");
                            Hypnotist.objectsCantPlaceTraps.Add(miraMedScanner);
                            break;
                        case 2:
                            GameObject polusMedScanner = GameObject.Find("panel_medplatform");
                            Hypnotist.objectsCantPlaceTraps.Add(polusMedScanner);
                            break;
                        case 3:
                            GameObject dleksMedScanner = GameObject.Find("MedScanner");
                            Hypnotist.objectsCantPlaceTraps.Add(dleksMedScanner);
                            break;
                        case 4:
                            GameObject airshipMeetingLadderTop = GameObject.Find("Airship(Clone)/MeetingRoom/ladder_meeting/LadderTop");
                            GameObject airshipMeetingLadderBottom = GameObject.Find("Airship(Clone)/MeetingRoom/ladder_meeting/LadderBottom");
                            GameObject airshipPlatformLeft = GameObject.Find("PlatformLeft");
                            GameObject airshipPlatformRight = GameObject.Find("PlatformRight");
                            GameObject airshipgapLadderTop = GameObject.Find("Airship(Clone)/GapRoom/ladder_gap/LadderTop");
                            GameObject airshipgapLadderBottom = GameObject.Find("Airship(Clone)/GapRoom/ladder_gap/LadderBottom");
                            GameObject airshipelectricalLadderTop = GameObject.Find("Airship(Clone)/HallwayMain/ladder_electrical/LadderTop");
                            GameObject airshipelectricalLadderBottom = GameObject.Find("Airship(Clone)/HallwayMain/ladder_electrical/LadderBottom");
                            Hypnotist.objectsCantPlaceTraps.Add(airshipMeetingLadderTop);
                            Hypnotist.objectsCantPlaceTraps.Add(airshipMeetingLadderBottom);
                            Hypnotist.objectsCantPlaceTraps.Add(airshipPlatformLeft);
                            Hypnotist.objectsCantPlaceTraps.Add(airshipPlatformRight);
                            Hypnotist.objectsCantPlaceTraps.Add(airshipgapLadderTop);
                            Hypnotist.objectsCantPlaceTraps.Add(airshipgapLadderBottom);
                            Hypnotist.objectsCantPlaceTraps.Add(airshipelectricalLadderTop);
                            Hypnotist.objectsCantPlaceTraps.Add(airshipelectricalLadderBottom);
                            break;
                        case 5:
                            GameObject submergedMedScanner = GameObject.Find("console_medscan");
                            Hypnotist.objectsCantPlaceTraps.Add(submergedMedScanner);
                            break;
                    }
                }

                // Remove the swipe card task
                clearSwipeCardTask();

                // Remove airship doors
                removeAirshipDoors();

                // Activate sensei map
                Helpers.activateSenseiMap();               

                // Create the jail if there's a Jailer
                if (Jailer.jailer != null && PlayerControl.LocalPlayer != null && !createdjail) {
                    GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                    cell.name = "cell";
                    cell.gameObject.layer = 9;
                    cell.transform.GetChild(0).gameObject.layer = 9;

                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        case 0:
                            if (activatedSensei) {
                                cell.transform.position = new Vector3(-12f, 7.2f, 0.5f);
                            }
                            else {
                                cell.transform.position = new Vector3(-10.25f, 3.38f, 0.5f);
                            }
                            break;
                        case 1:
                            cell.transform.position = new Vector3(1.75f, 1.125f, 0.5f);
                            break;
                        case 2:
                            cell.transform.position = new Vector3(8.25f, -5.15f, 0.5f);
                            break;
                        case 3:
                            cell.transform.position = new Vector3(10.25f, 3.38f, 0.5f);
                            break;
                        case 4:
                            cell.transform.position = new Vector3(-18.45f, 3.55f, 0.5f);
                            break;
                        case 5:
                            cell.transform.position = new Vector3(-15.25f, 28.4f, 0.5f);
                            // Create another jail on submerged lower floor
                            GameObject celltwo = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                            celltwo.name = "cell_lower";
                            celltwo.transform.position = new Vector3(-1.15f, -21f, -0.01f);
                            celltwo.gameObject.layer = 9;
                            celltwo.transform.GetChild(0).gameObject.layer = 9;
                            break;
                    }
                    createdjail = true;
                }

                // Create susBoxes for Stranded
                if (Stranded.stranded != null && Stranded.stranded == PlayerControl.LocalPlayer && !createdStrandedBoxes) {                                       
                    GameObject ammoBox01 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                    ammoBox01.transform.position = Stranded.susBoxPositions[0];
                    ammoBox01.name = "ammoBox";
                    GameObject ammoBox02 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                    ammoBox02.transform.position = Stranded.susBoxPositions[1];
                    ammoBox02.name = "ammoBox";
                    GameObject ammoBox03 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                    ammoBox03.transform.position = Stranded.susBoxPositions[2];
                    ammoBox03.name = "ammoBox";
                    GameObject ventBox = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                    ventBox.transform.position = Stranded.susBoxPositions[3];
                    ventBox.name = "ventBox";
                    GameObject invisibleBox = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                    invisibleBox.transform.position = Stranded.susBoxPositions[4];
                    invisibleBox.name = "invisibleBox";
                    Stranded.groundItems.Add(ammoBox01);
                    Stranded.groundItems.Add(ammoBox02);
                    Stranded.groundItems.Add(ammoBox03);
                    Stranded.groundItems.Add(ventBox);
                    Stranded.groundItems.Add(invisibleBox);                  
                    // Nothing boxes
                    for (int i = 0; i < Stranded.susBoxPositions.Count - 5; i++) {
                        GameObject nothingBox = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                        nothingBox.transform.position = Stranded.susBoxPositions[i + 5];
                        nothingBox.name = "nothingBox";
                        Stranded.groundItems.Add(nothingBox);
                    }
                    createdStrandedBoxes = true;
                }

                // Create items for Monja
                if (Monja.monja != null && !createdMonjaItems) {
                    GameObject monjaRitual = GameObject.Instantiate(CustomMain.customAssets.monjaRitual, PlayerControl.LocalPlayer.transform.parent);
                    Monja.ritualObject = monjaRitual;
                    Monja.ritualObject.layer = 9;
                    GameObject monjaSprite = GameObject.Instantiate(CustomMain.customAssets.monjaSprite, PlayerControl.LocalPlayer.transform.parent);
                    Monja.monjaSprite = monjaSprite;
                    Monja.monjaSprite.transform.parent = Monja.monja.transform;
                    Monja.monjaSprite.transform.position = new Vector3 (0 ,0, -1);
                    Monja.monjaSprite.transform.localPosition = new Vector3 (0 ,0, -1);
                    Monja.monjaSprite.SetActive(false);
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        case 0:
                            if (activatedSensei) {
                                Monja.ritualObject.transform.position = new Vector3(3f, 2.25f, 0.5f);
                            }
                            else {
                                Monja.ritualObject.transform.position = new Vector3(-0.9f, 5f, 0.5f);
                            }
                            break;
                        case 1:
                            Monja.ritualObject.transform.position = new Vector3(17.85f, 11.35f, 0.5f);
                            break;
                        case 2:
                            Monja.ritualObject.transform.position = new Vector3(13.75f, -9.75f, 0.5f);
                            break;
                        case 3:
                            Monja.ritualObject.transform.position = new Vector3(0.9f, 5f, 0.5f);
                            break;
                        case 4:
                            Monja.ritualObject.transform.position = new Vector3(10.75f, -0.25f, 0.5f);
                            break;
                        case 5:
                            Monja.ritualObject.transform.position = new Vector3(-6.35f, 13.85f, -0.005f);
                            break;
                    }
                    GameObject keyitem01 = GameObject.Instantiate(CustomMain.customAssets.monjaOneSprite, PlayerControl.LocalPlayer.transform.parent);
                    keyitem01.transform.position = Monja.itemListPositions[0];
                    keyitem01.name = "item01";
                    Monja.item01 = keyitem01;
                    GameObject keyitem02 = GameObject.Instantiate(CustomMain.customAssets.monjaTwoSprite, PlayerControl.LocalPlayer.transform.parent);
                    keyitem02.transform.position = Monja.itemListPositions[1];
                    keyitem02.name = "item02";
                    Monja.item02 = keyitem02;
                    GameObject keyitem03 = GameObject.Instantiate(CustomMain.customAssets.monjaThreeSprite, PlayerControl.LocalPlayer.transform.parent);
                    keyitem03.transform.position = Monja.itemListPositions[2];
                    keyitem03.name = "item03";
                    Monja.item03 = keyitem03;
                    GameObject keyitem04 = GameObject.Instantiate(CustomMain.customAssets.monjaFourSprite, PlayerControl.LocalPlayer.transform.parent);
                    keyitem04.transform.position = Monja.itemListPositions[3];
                    keyitem04.name = "item04";
                    Monja.item04 = keyitem04;
                    GameObject keyitem05 = GameObject.Instantiate(CustomMain.customAssets.monjaFiveSprite, PlayerControl.LocalPlayer.transform.parent);
                    keyitem05.transform.position = Monja.itemListPositions[4];
                    keyitem05.name = "item05";
                    Monja.item05 = keyitem05;
                    Monja.objectList.Add(keyitem01);
                    Monja.objectList.Add(keyitem02);
                    Monja.objectList.Add(keyitem03);
                    Monja.objectList.Add(keyitem04);
                    Monja.objectList.Add(keyitem05);
                    if (Monja.monja != PlayerControl.LocalPlayer) {
                        foreach (GameObject keyItem in Monja.objectList) {
                            keyItem.SetActive(false);
                        }
                    }
                    createdMonjaItems = true;
                }

                if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                    // Create who Am I Mode
                    if (whoAmIMode && howmanygamemodesareon != 1 && !createdWhoAmI) {

                        string[] crewRoleNames = new string[25] { "captainRole", "mechanicRole", "sheriffRole", "detectiveRole", "forensicRole", "timetravelerRole", "squireRole", "cheaterRole", "fortunetellerRole", "hackerRole", "sleuthRole", "finkRole", "kidRole", "welderRole", "spiritualistRole", "vigilantRole", "hunterRole", "jinxRole", "cowardRole", "batRole", "necromancerRole", "engineerRole", "shyRole", "taskmasterRole", "jailerRole" };
                        // Crew boxes
                        for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 35; i++) {
                            GameObject whoAmICrewBox = GameObject.Instantiate(CustomMain.customAssets.susBoxThreeColor, PlayerControl.LocalPlayer.transform.parent);
                            whoAmICrewBox.transform.position = ZombieLaboratory.susBoxPositions[i];
                            whoAmICrewBox.name = crewRoleNames[i];
                            whoAmIModeCrewItems.Add(whoAmICrewBox);
                            whoAmIModeGlobalItems.Add(whoAmICrewBox);
                        }

                        string[] impostorRoleNames = new string[15] { "mimicRole", "painterRole", "demonRole", "janitorRole", "illusionistRole", "manipulatorRole", "bombermanRole", "chameleonRole", "gamblerRole", "sorcererRole", "medusaRole", "hypnotistRole", "archerRole", "plumberRole", "librarianRole" };
                        // Impostor boxes
                        for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 45; i++) {
                            GameObject whoAmIImpostorBox = GameObject.Instantiate(CustomMain.customAssets.susBoxRed, PlayerControl.LocalPlayer.transform.parent);
                            whoAmIImpostorBox.transform.position = ZombieLaboratory.susBoxPositions[i + 25];
                            whoAmIImpostorBox.name = impostorRoleNames[i];
                            whoAmIModeImpostorItems.Add(whoAmIImpostorBox);
                            whoAmIModeGlobalItems.Add(whoAmIImpostorBox);
                        }

                        string[] rebelsRoleNames = new string[9] { "renegadeRole", "trapperRole", "yinyangerRole", "challengerRole", "ninjaRole", "berserkerRole", "yandereRole", "strandedRole", "monjaRole" };
                        // Rebel boxes
                        for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 51; i++) {
                            GameObject whoAmIRebelBox = GameObject.Instantiate(CustomMain.customAssets.susBoxThreeColor, PlayerControl.LocalPlayer.transform.parent);
                            whoAmIRebelBox.transform.position = ZombieLaboratory.susBoxPositions[i + 40];
                            whoAmIRebelBox.name = rebelsRoleNames[i];
                            whoAmIModeRebelsItems.Add(whoAmIRebelBox);
                            whoAmIModeGlobalItems.Add(whoAmIRebelBox);
                        }

                        string[] neutralsRoleNames = new string[8] { "jokerRole", "pyromaniacRole", "treasurehunterRole", "devourerRole", "poisonerRole", "puppeteerRole", "exilerRole", "seekerRole" };
                        // Neutral boxes
                        for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 52; i++) {
                            GameObject whoAmINeutralBox = GameObject.Instantiate(CustomMain.customAssets.susBoxThreeColor, PlayerControl.LocalPlayer.transform.parent);
                            whoAmINeutralBox.transform.position = ZombieLaboratory.susBoxPositions[i + 50];
                            whoAmINeutralBox.name = neutralsRoleNames[i];
                            whoAmIModeNeutralsItems.Add(whoAmINeutralBox);
                            whoAmIModeGlobalItems.Add(whoAmINeutralBox);
                            if (whoAmINeutralBox.name == "pyromaniacRole") {
                                whoAmINeutralBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.susBoxRed.GetComponent<SpriteRenderer>().sprite;
                            }
                        }

                        if (PlayerControl.LocalPlayer.Data.Role.IsImpostor) {
                            foreach (GameObject item in whoAmIModeCrewItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                            foreach (GameObject item in whoAmIModeRebelsItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                            foreach (GameObject item in whoAmIModeNeutralsItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                        }
                        else {
                            foreach (GameObject item in whoAmIModeImpostorItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                        }
                        createdWhoAmI = true;
                    }

                    if (howmanygamemodesareon == 1) { 
                        switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                            // Skeld / Custom Skeld
                            case 0:
                                // Capture the flag
                                if (CaptureTheFlag.captureTheFlagMode) {
                                    if (activatedSensei) {
                                        if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                            CaptureTheFlag.stealerPlayer.transform.position = new Vector3(-3.65f, 5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(CaptureTheFlag.stealerPlayer);
                                        }

                                        foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(-17.5f, -1.15f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(7.7f, -0.95f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        if (PlayerControl.LocalPlayer != null && !createdcapturetheflag) {
                                            GameObject redflag = GameObject.Instantiate(CustomMain.customAssets.redflag, PlayerControl.LocalPlayer.transform.parent);
                                            redflag.name = "redflag";
                                            redflag.transform.position = new Vector3(-17.5f, -1.35f, 0.5f);
                                            CaptureTheFlag.redflag = redflag;
                                            GameObject redflagbase = GameObject.Instantiate(CustomMain.customAssets.redflagbase, PlayerControl.LocalPlayer.transform.parent);
                                            redflagbase.name = "redflagbase";
                                            redflagbase.transform.position = new Vector3(-17.5f, -1.4f, 1f);
                                            CaptureTheFlag.redflagbase = redflagbase;
                                            GameObject blueflag = GameObject.Instantiate(CustomMain.customAssets.blueflag, PlayerControl.LocalPlayer.transform.parent);
                                            blueflag.name = "blueflag";
                                            blueflag.transform.position = new Vector3(7.7f, -1.15f, 0.5f);
                                            CaptureTheFlag.blueflag = blueflag;
                                            GameObject blueflagbase = GameObject.Instantiate(CustomMain.customAssets.blueflagbase, PlayerControl.LocalPlayer.transform.parent);
                                            blueflagbase.name = "blueflagbase";
                                            blueflagbase.transform.position = new Vector3(7.7f, -1.2f, 1f);
                                            CaptureTheFlag.blueflagbase = blueflagbase;
                                            CaptureTheFlag.stealerSpawns.Add(redflagbase);
                                            CaptureTheFlag.stealerSpawns.Add(blueflagbase);
                                            createdcapturetheflag = true;
                                        }
                                    }
                                    else {
                                        if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                            CaptureTheFlag.stealerPlayer.transform.position = new Vector3(6.35f, -7.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(CaptureTheFlag.stealerPlayer);
                                        }

                                        foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(-20.5f, -5.15f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(16.5f, -4.45f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        if (PlayerControl.LocalPlayer != null && !createdcapturetheflag) {
                                            GameObject redflag = GameObject.Instantiate(CustomMain.customAssets.redflag, PlayerControl.LocalPlayer.transform.parent);
                                            redflag.name = "redflag";
                                            redflag.transform.position = new Vector3(-20.5f, -5.35f, 0.5f);
                                            CaptureTheFlag.redflag = redflag;
                                            GameObject redflagbase = GameObject.Instantiate(CustomMain.customAssets.redflagbase, PlayerControl.LocalPlayer.transform.parent);
                                            redflagbase.name = "redflagbase";
                                            redflagbase.transform.position = new Vector3(-20.5f, -5.4f, 1f);
                                            CaptureTheFlag.redflagbase = redflagbase;
                                            GameObject blueflag = GameObject.Instantiate(CustomMain.customAssets.blueflag, PlayerControl.LocalPlayer.transform.parent);
                                            blueflag.name = "blueflag";
                                            blueflag.transform.position = new Vector3(16.5f, -4.65f, 0.5f);
                                            CaptureTheFlag.blueflag = blueflag;
                                            GameObject blueflagbase = GameObject.Instantiate(CustomMain.customAssets.blueflagbase, PlayerControl.LocalPlayer.transform.parent);
                                            blueflagbase.name = "blueflagbase";
                                            blueflagbase.transform.position = new Vector3(16.5f, -4.7f, 1f);
                                            CaptureTheFlag.blueflagbase = blueflagbase;
                                            CaptureTheFlag.stealerSpawns.Add(redflagbase);
                                            CaptureTheFlag.stealerSpawns.Add(blueflagbase);
                                            createdcapturetheflag = true;
                                        }
                                    }
                                }
                                // Police And Thiefs
                                else if (PoliceAndThief.policeAndThiefMode) {
                                    if (activatedSensei) {
                                        foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(-12f, 5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                                            if (player == PlayerControl.LocalPlayer) {
                                                player.transform.position = new Vector3(13.75f, -0.2f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(player);

                                                // Add Arrows pointing the release and deliver point
                                                if (PoliceAndThief.localThiefReleaseArrow.Count == 0) {
                                                    PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                                                    PoliceAndThief.localThiefReleaseArrow[0].arrow.SetActive(true);
                                                }
                                                if (PoliceAndThief.localThiefDeliverArrow.Count == 0) {
                                                    PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                                                    PoliceAndThief.localThiefDeliverArrow[0].arrow.SetActive(true);
                                                }
                                            }
                                        }
                                        if (PlayerControl.LocalPlayer != null && !createdpoliceandthief) {
                                            GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                                            cell.name = "cell";
                                            cell.transform.position = new Vector3(-12f, 7.2f, 0.5f);
                                            cell.gameObject.layer = 9;
                                            cell.transform.GetChild(0).gameObject.layer = 9;
                                            PoliceAndThief.cell = cell;
                                            GameObject cellbutton = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerControl.LocalPlayer.transform.parent);
                                            cellbutton.name = "cellbutton";
                                            cellbutton.transform.position = new Vector3(-12f, 4.7f, 0.5f);
                                            PoliceAndThief.cellbutton = cellbutton;
                                            GameObject jewelbutton = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerControl.LocalPlayer.transform.parent);
                                            jewelbutton.name = "jewelbutton";
                                            jewelbutton.transform.position = new Vector3(13.75f, -0.42f, 0.5f);
                                            PoliceAndThief.jewelbutton = jewelbutton;
                                            GameObject thiefspaceship = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerControl.LocalPlayer.transform.parent);
                                            thiefspaceship.name = "thiefspaceship";
                                            thiefspaceship.transform.position = new Vector3(17f, 0f, 0.6f);
                                            createdpoliceandthief = true;

                                            // Spawn jewels
                                            GameObject jewel01 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel01.transform.position = new Vector3(6.95f, 4.95f, 1f);
                                            jewel01.name = "jewel01";
                                            PoliceAndThief.jewel01 = jewel01;
                                            GameObject jewel02 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel02.transform.position = new Vector3(-3.75f, 5.35f, 1f);
                                            jewel02.name = "jewel02";
                                            PoliceAndThief.jewel02 = jewel02;
                                            GameObject jewel03 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel03.transform.position = new Vector3(-7.7f, 11.3f, 1f);
                                            jewel03.name = "jewel03";
                                            PoliceAndThief.jewel03 = jewel03;
                                            GameObject jewel04 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel04.transform.position = new Vector3(-19.65f, 5.3f, 1f);
                                            jewel04.name = "jewel04";
                                            PoliceAndThief.jewel04 = jewel04;
                                            GameObject jewel05 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel05.transform.position = new Vector3(-19.65f, -8, 1f);
                                            jewel05.name = "jewel05";
                                            PoliceAndThief.jewel05 = jewel05;
                                            GameObject jewel06 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel06.transform.position = new Vector3(-5.45f, -13f, 1f);
                                            jewel06.name = "jewel06";
                                            PoliceAndThief.jewel06 = jewel06;
                                            GameObject jewel07 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel07.transform.position = new Vector3(-7.65f, -4.2f, 1f);
                                            jewel07.name = "jewel07";
                                            PoliceAndThief.jewel07 = jewel07;
                                            GameObject jewel08 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel08.transform.position = new Vector3(2f, -6.75f, 1f);
                                            jewel08.name = "jewel08";
                                            PoliceAndThief.jewel08 = jewel08;
                                            GameObject jewel09 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel09.transform.position = new Vector3(8.9f, 1.45f, 1f);
                                            jewel09.name = "jewel09";
                                            PoliceAndThief.jewel09 = jewel09;
                                            GameObject jewel10 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel10.transform.position = new Vector3(4.6f, -2.25f, 1f);
                                            jewel10.name = "jewel10";
                                            PoliceAndThief.jewel10 = jewel10;
                                            GameObject jewel11 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel11.transform.position = new Vector3(-5.05f, -0.88f, 1f);
                                            jewel11.name = "jewel11";
                                            PoliceAndThief.jewel11 = jewel11;
                                            GameObject jewel12 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel12.transform.position = new Vector3(-8.25f, -0.45f, 1f);
                                            jewel12.name = "jewel12";
                                            PoliceAndThief.jewel12 = jewel12;
                                            GameObject jewel13 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel13.transform.position = new Vector3(-19.75f, -1.55f, 1f);
                                            jewel13.name = "jewel13";
                                            PoliceAndThief.jewel13 = jewel13;
                                            GameObject jewel14 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel14.transform.position = new Vector3(-12.1f, -13.15f, 1f);
                                            jewel14.name = "jewel14";
                                            PoliceAndThief.jewel14 = jewel14;
                                            GameObject jewel15 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel15.transform.position = new Vector3(7.15f, -14.45f, 1f);
                                            jewel15.name = "jewel15";
                                            PoliceAndThief.jewel15 = jewel15;
                                            PoliceAndThief.thiefTreasures.Add(jewel01);
                                            PoliceAndThief.thiefTreasures.Add(jewel02);
                                            PoliceAndThief.thiefTreasures.Add(jewel03);
                                            PoliceAndThief.thiefTreasures.Add(jewel04);
                                            PoliceAndThief.thiefTreasures.Add(jewel05);
                                            PoliceAndThief.thiefTreasures.Add(jewel06);
                                            PoliceAndThief.thiefTreasures.Add(jewel07);
                                            PoliceAndThief.thiefTreasures.Add(jewel08);
                                            PoliceAndThief.thiefTreasures.Add(jewel09);
                                            PoliceAndThief.thiefTreasures.Add(jewel10);
                                            PoliceAndThief.thiefTreasures.Add(jewel11);
                                            PoliceAndThief.thiefTreasures.Add(jewel12);
                                            PoliceAndThief.thiefTreasures.Add(jewel13);
                                            PoliceAndThief.thiefTreasures.Add(jewel14);
                                            PoliceAndThief.thiefTreasures.Add(jewel15);
                                        }
                                    }
                                    else {
                                        foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(-10.2f, 1.18f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                                            if (player == PlayerControl.LocalPlayer) {
                                                player.transform.position = new Vector3(-1.31f, -16.25f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(player);

                                                // Add Arrows pointing the release and deliver point
                                                if (PoliceAndThief.localThiefReleaseArrow.Count == 0) {
                                                    PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                                                    PoliceAndThief.localThiefReleaseArrow[0].arrow.SetActive(true);
                                                }
                                                if (PoliceAndThief.localThiefDeliverArrow.Count == 0) {
                                                    PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                                                    PoliceAndThief.localThiefDeliverArrow[0].arrow.SetActive(true);
                                                }
                                            }
                                        }
                                        if (PlayerControl.LocalPlayer != null && !createdpoliceandthief) {
                                            GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                                            cell.name = "cell";
                                            cell.transform.position = new Vector3(-10.25f, 3.38f, 0.5f);
                                            cell.gameObject.layer = 9;
                                            cell.transform.GetChild(0).gameObject.layer = 9;
                                            PoliceAndThief.cell = cell;
                                            GameObject cellbutton = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerControl.LocalPlayer.transform.parent);
                                            cellbutton.name = "cellbutton";
                                            cellbutton.transform.position = new Vector3(-10.2f, 0.93f, 0.5f);
                                            PoliceAndThief.cellbutton = cellbutton;
                                            GameObject jewelbutton = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerControl.LocalPlayer.transform.parent);
                                            jewelbutton.name = "jewelbutton";
                                            jewelbutton.transform.position = new Vector3(0.20f, -17.15f, 0.5f);
                                            PoliceAndThief.jewelbutton = jewelbutton;
                                            GameObject thiefspaceship = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerControl.LocalPlayer.transform.parent);
                                            thiefspaceship.name = "thiefspaceship";
                                            thiefspaceship.transform.position = new Vector3(1.765f, -19.16f, 0.6f);
                                            GameObject thiefspaceshiphatch = GameObject.Instantiate(CustomMain.customAssets.thiefspaceshiphatch, PlayerControl.LocalPlayer.transform.parent);
                                            thiefspaceshiphatch.name = "thiefspaceshiphatch";
                                            thiefspaceshiphatch.transform.position = new Vector3(1.765f, -19.16f, 0.6f);
                                            createdpoliceandthief = true;

                                            // Spawn jewels
                                            GameObject jewel01 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel01.transform.position = new Vector3(-18.65f, -9.9f, 1f);
                                            jewel01.name = "jewel01";
                                            PoliceAndThief.jewel01 = jewel01;
                                            GameObject jewel02 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel02.transform.position = new Vector3(-21.5f, -2, 1f);
                                            jewel02.name = "jewel02";
                                            PoliceAndThief.jewel02 = jewel02;
                                            GameObject jewel03 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel03.transform.position = new Vector3(-5.9f, -8.25f, 1f);
                                            jewel03.name = "jewel03";
                                            PoliceAndThief.jewel03 = jewel03;
                                            GameObject jewel04 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel04.transform.position = new Vector3(4.5f, -7.5f, 1f);
                                            jewel04.name = "jewel04";
                                            PoliceAndThief.jewel04 = jewel04;
                                            GameObject jewel05 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel05.transform.position = new Vector3(7.85f, -14.45f, 1f);
                                            jewel05.name = "jewel05";
                                            PoliceAndThief.jewel05 = jewel05;
                                            GameObject jewel06 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel06.transform.position = new Vector3(6.65f, -4.8f, 1f);
                                            jewel06.name = "jewel06";
                                            PoliceAndThief.jewel06 = jewel06;
                                            GameObject jewel07 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel07.transform.position = new Vector3(10.5f, 2.15f, 1f);
                                            jewel07.name = "jewel07";
                                            PoliceAndThief.jewel07 = jewel07;
                                            GameObject jewel08 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                            jewel08.transform.position = new Vector3(-5.5f, 3.5f, 1f);
                                            jewel08.name = "jewel08";
                                            PoliceAndThief.jewel08 = jewel08;
                                            GameObject jewel09 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel09.transform.position = new Vector3(-19, -1.2f, 1f);
                                            jewel09.name = "jewel09";
                                            PoliceAndThief.jewel09 = jewel09;
                                            GameObject jewel10 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel10.transform.position = new Vector3(-21.5f, -8.35f, 1f);
                                            jewel10.name = "jewel10";
                                            PoliceAndThief.jewel10 = jewel10;
                                            GameObject jewel11 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel11.transform.position = new Vector3(-12.5f, -3.75f, 1f);
                                            jewel11.name = "jewel11";
                                            PoliceAndThief.jewel11 = jewel11;
                                            GameObject jewel12 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel12.transform.position = new Vector3(-5.9f, -5.25f, 1f);
                                            jewel12.name = "jewel12";
                                            PoliceAndThief.jewel12 = jewel12;
                                            GameObject jewel13 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel13.transform.position = new Vector3(2.65f, -16.5f, 1f);
                                            jewel13.name = "jewel13";
                                            PoliceAndThief.jewel13 = jewel13;
                                            GameObject jewel14 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel14.transform.position = new Vector3(16.75f, -4.75f, 1f);
                                            jewel14.name = "jewel14";
                                            PoliceAndThief.jewel14 = jewel14;
                                            GameObject jewel15 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                            jewel15.transform.position = new Vector3(3.8f, 3.5f, 1f);
                                            jewel15.name = "jewel15";
                                            PoliceAndThief.jewel15 = jewel15;
                                            PoliceAndThief.thiefTreasures.Add(jewel01);
                                            PoliceAndThief.thiefTreasures.Add(jewel02);
                                            PoliceAndThief.thiefTreasures.Add(jewel03);
                                            PoliceAndThief.thiefTreasures.Add(jewel04);
                                            PoliceAndThief.thiefTreasures.Add(jewel05);
                                            PoliceAndThief.thiefTreasures.Add(jewel06);
                                            PoliceAndThief.thiefTreasures.Add(jewel07);
                                            PoliceAndThief.thiefTreasures.Add(jewel08);
                                            PoliceAndThief.thiefTreasures.Add(jewel09);
                                            PoliceAndThief.thiefTreasures.Add(jewel10);
                                            PoliceAndThief.thiefTreasures.Add(jewel11);
                                            PoliceAndThief.thiefTreasures.Add(jewel12);
                                            PoliceAndThief.thiefTreasures.Add(jewel13);
                                            PoliceAndThief.thiefTreasures.Add(jewel14);
                                            PoliceAndThief.thiefTreasures.Add(jewel15);
                                        }
                                    }
                                }
                                // King Of The Hill
                                else if (KingOfTheHill.kingOfTheHillMode) {
                                    if (activatedSensei) {
                                        if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                            KingOfTheHill.usurperPlayer.transform.position = new Vector3(-6.8f, 10.75f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(KingOfTheHill.usurperPlayer);
                                        }

                                        foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(-16.4f, -10.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(7f, -14.15f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        if (PlayerControl.LocalPlayer != null && !createdkingofthehill) {
                                            GameObject greenteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                            greenteamfloor.name = "greenteamfloor";
                                            greenteamfloor.transform.position = new Vector3(-16.4f, -10.5f, 0.5f);
                                            GameObject yellowteamfloor = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerControl.LocalPlayer.transform.parent);
                                            yellowteamfloor.name = "yellowteamfloor";
                                            yellowteamfloor.transform.position = new Vector3(7f, -14.4f, 0.5f);
                                            GameObject greenkingaura = GameObject.Instantiate(CustomMain.customAssets.greenaura, KingOfTheHill.greenKingplayer.transform);
                                            greenkingaura.name = "greenkingaura";
                                            greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                                            KingOfTheHill.greenkingaura = greenkingaura;
                                            GameObject yellowkingaura = GameObject.Instantiate(CustomMain.customAssets.yellowaura, KingOfTheHill.yellowKingplayer.transform);
                                            yellowkingaura.name = "yellowkingaura";
                                            yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                                            KingOfTheHill.yellowkingaura = yellowkingaura;
                                            GameObject flagzoneone = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                            flagzoneone.name = "flagzoneone";
                                            flagzoneone.transform.position = new Vector3(7.85f, -1.5f, 0.4f);
                                            KingOfTheHill.flagzoneone = flagzoneone;
                                            GameObject zoneone = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                            zoneone.name = "zoneone";
                                            zoneone.transform.position = new Vector3(7.85f, -1.5f, 0.5f);
                                            KingOfTheHill.zoneone = zoneone;
                                            GameObject flagzonetwo = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                            flagzonetwo.name = "flagzonetwo";
                                            flagzonetwo.transform.position = new Vector3(-6.35f, -1.1f, 0.4f);
                                            KingOfTheHill.flagzonetwo = flagzonetwo;
                                            GameObject zonetwo = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                            zonetwo.name = "zonetwo";
                                            zonetwo.transform.position = new Vector3(-6.35f, -1.1f, 0.5f);
                                            KingOfTheHill.zonetwo = zonetwo;
                                            GameObject flagzonethree = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                            flagzonethree.name = "flagzonethree";
                                            flagzonethree.transform.position = new Vector3(-12.15f, 7.35f, 0.4f);
                                            KingOfTheHill.flagzonethree = flagzonethree;
                                            GameObject zonethree = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                            zonethree.name = "zonethree";
                                            zonethree.transform.position = new Vector3(-12.15f, 7.35f, 0.5f);
                                            KingOfTheHill.zonethree = zonethree;
                                            KingOfTheHill.kingZones.Add(zoneone);
                                            KingOfTheHill.kingZones.Add(zonetwo);
                                            KingOfTheHill.kingZones.Add(zonethree);
                                            KingOfTheHill.usurperSpawns.Add(greenteamfloor);
                                            KingOfTheHill.usurperSpawns.Add(yellowteamfloor);
                                            createdkingofthehill = true;
                                        }
                                    }
                                    else {
                                        if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                            KingOfTheHill.usurperPlayer.transform.position = new Vector3(-1f, 5.35f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(KingOfTheHill.usurperPlayer);
                                        }

                                        foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(-7f, -8.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(6.25f, -3.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        if (PlayerControl.LocalPlayer != null && !createdkingofthehill) {
                                            GameObject greenteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                            greenteamfloor.name = "greenteamfloor";
                                            greenteamfloor.transform.position = new Vector3(-7f, -8.5f, 0.5f);
                                            GameObject yellowteamfloor = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerControl.LocalPlayer.transform.parent);
                                            yellowteamfloor.name = "yellowteamfloor";
                                            yellowteamfloor.transform.position = new Vector3(6.25f, -3.75f, 0.5f);
                                            GameObject greenkingaura = GameObject.Instantiate(CustomMain.customAssets.greenaura, KingOfTheHill.greenKingplayer.transform);
                                            greenkingaura.name = "greenkingaura";
                                            greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                                            KingOfTheHill.greenkingaura = greenkingaura;
                                            GameObject yellowkingaura = GameObject.Instantiate(CustomMain.customAssets.yellowaura, KingOfTheHill.yellowKingplayer.transform);
                                            yellowkingaura.name = "yellowkingaura";
                                            yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                                            KingOfTheHill.yellowkingaura = yellowkingaura;
                                            GameObject flagzoneone = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                            flagzoneone.name = "flagzoneone";
                                            flagzoneone.transform.position = new Vector3(-9.1f, -2.25f, 0.4f);
                                            KingOfTheHill.flagzoneone = flagzoneone;
                                            GameObject zoneone = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                            zoneone.name = "zoneone";
                                            zoneone.transform.position = new Vector3(-9.1f, -2.25f, 0.5f);
                                            KingOfTheHill.zoneone = zoneone;
                                            GameObject flagzonetwo = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                            flagzonetwo.name = "flagzonetwo";
                                            flagzonetwo.transform.position = new Vector3(4.5f, -7.5f, 0.4f);
                                            KingOfTheHill.flagzonetwo = flagzonetwo;
                                            GameObject zonetwo = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                            zonetwo.name = "zonetwo";
                                            zonetwo.transform.position = new Vector3(4.5f, -7.5f, 0.5f);
                                            KingOfTheHill.zonetwo = zonetwo;
                                            GameObject flagzonethree = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                            flagzonethree.name = "flagzonethree";
                                            flagzonethree.transform.position = new Vector3(3.25f, -15.5f, 0.4f);
                                            KingOfTheHill.flagzonethree = flagzonethree;
                                            GameObject zonethree = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                            zonethree.name = "zonethree";
                                            zonethree.transform.position = new Vector3(3.25f, -15.5f, 0.5f);
                                            KingOfTheHill.zonethree = zonethree;
                                            KingOfTheHill.kingZones.Add(zoneone);
                                            KingOfTheHill.kingZones.Add(zonetwo);
                                            KingOfTheHill.kingZones.Add(zonethree);
                                            KingOfTheHill.usurperSpawns.Add(greenteamfloor);
                                            KingOfTheHill.usurperSpawns.Add(yellowteamfloor);
                                            createdkingofthehill = true;
                                        }
                                    }
                                }
                                // Hot Potato
                                else if (HotPotato.hotPotatoMode) {
                                    if (activatedSensei) {
                                        if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                                            HotPotato.hotPotatoPlayer.transform.position = new Vector3(-6.5f, -2.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(HotPotato.hotPotatoPlayer);
                                        }

                                        foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(12.5f, -0.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }

                                        if (PlayerControl.LocalPlayer != null && !createdhotpotato) {
                                            GameObject hotpotato = GameObject.Instantiate(CustomMain.customAssets.hotPotato, HotPotato.hotPotatoPlayer.transform);
                                            hotpotato.name = "hotpotato";
                                            hotpotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                            HotPotato.hotPotato = hotpotato;
                                            createdhotpotato = true;
                                        }
                                    }
                                    else {
                                        if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                                            HotPotato.hotPotatoPlayer.transform.position = new Vector3(-0.75f, -7f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(HotPotato.hotPotatoPlayer);
                                        }

                                        foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(6.25f, -3.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }

                                        if (PlayerControl.LocalPlayer != null && !createdhotpotato) {
                                            GameObject hotpotato = GameObject.Instantiate(CustomMain.customAssets.hotPotato, HotPotato.hotPotatoPlayer.transform);
                                            hotpotato.name = "hotpotato";
                                            hotpotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                            HotPotato.hotPotato = hotpotato;
                                            createdhotpotato = true;
                                        }
                                    }
                                }
                                // Zombie Laboratory
                                else if (ZombieLaboratory.zombieLaboratoryMode) {
                                    if (activatedSensei) {
                                        foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(-4.85f, 6, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                                            if (player == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != ZombieLaboratory.nursePlayer) {
                                                player.transform.position = new Vector3(4.75f, -8.5f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(player);
                                                // Add Arrows pointing the deliver point
                                                if (ZombieLaboratory.localSurvivorsDeliverArrow.Count == 0) {
                                                    ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                                                    ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(true);
                                                }
                                            }
                                        }

                                        if (PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer) {
                                            ZombieLaboratory.nursePlayer.transform.position = new Vector3(-12f, 7.15f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(ZombieLaboratory.nursePlayer);
                                            GameObject mapMedKit = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                            mapMedKit.name = "mapMedKit";
                                            mapMedKit.transform.position = new Vector3(-6.5f, -0.85f, -0.1f);
                                            GameObject mapMedKittwo = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                            mapMedKittwo.name = "mapMedKittwo";
                                            mapMedKittwo.transform.position = new Vector3(-18.85f, 2f, -0.1f);
                                            GameObject mapMedKitthree = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                            mapMedKitthree.name = "mapMedKitthree";
                                            mapMedKitthree.transform.position = new Vector3(-5.75f, 11.75f, -0.1f);
                                            ZombieLaboratory.nurseMedkits.Add(mapMedKit);
                                            ZombieLaboratory.nurseMedkits.Add(mapMedKittwo);
                                            ZombieLaboratory.nurseMedkits.Add(mapMedKitthree);
                                            // Add Arrows pointing the medkit only for nurse
                                            if (ZombieLaboratory.localNurseArrows.Count == 0 && ZombieLaboratory.localNurseArrows.Count < 3) {
                                                ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                                ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                                ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            }
                                            ZombieLaboratory.localNurseArrows[0].arrow.SetActive(true);
                                            ZombieLaboratory.localNurseArrows[1].arrow.SetActive(true);
                                            ZombieLaboratory.localNurseArrows[2].arrow.SetActive(true);
                                        }

                                        if (PlayerControl.LocalPlayer != null && !createdzombielaboratory) {
                                            GameObject laboratory = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerControl.LocalPlayer.transform.parent);
                                            laboratory.name = "laboratory";
                                            laboratory.transform.position = new Vector3(-12f, 7.2f, 0.5f);
                                            laboratory.gameObject.layer = 9;
                                            laboratory.transform.GetChild(0).gameObject.layer = 9;
                                            ZombieLaboratory.laboratory = laboratory;
                                            ZombieLaboratory.laboratoryEnterButton = laboratory.transform.GetChild(1).gameObject;
                                            ZombieLaboratory.laboratoryExitButton = laboratory.transform.GetChild(2).gameObject;
                                            ZombieLaboratory.laboratoryCreateCureButton = laboratory.transform.GetChild(3).gameObject;
                                            ZombieLaboratory.laboratoryPutKeyItemButton = laboratory.transform.GetChild(4).gameObject;
                                            ZombieLaboratory.laboratoryExitLeftButton = laboratory.transform.GetChild(5).gameObject;
                                            ZombieLaboratory.laboratoryExitRightButton = laboratory.transform.GetChild(6).gameObject;
                                            ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryEnterButton);
                                            ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitButton);
                                            ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitLeftButton);
                                            ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitRightButton);

                                            GameObject nurseMedKit = GameObject.Instantiate(CustomMain.customAssets.nurseMedKit, PlayerControl.LocalPlayer.transform.parent);
                                            nurseMedKit.name = "nurseMedKit";
                                            nurseMedKit.transform.parent = ZombieLaboratory.nursePlayer.transform;
                                            nurseMedKit.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                                            ZombieLaboratory.laboratoryNurseMedKit = nurseMedKit;
                                            ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                                            ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratoryEnterButton);
                                            createdzombielaboratory = true;
                                        }
                                    }
                                    else {
                                        foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                                            if (player == PlayerControl.LocalPlayer)
                                                player.transform.position = new Vector3(-17.25f, -13.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }

                                        foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                                            if (player == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != ZombieLaboratory.nursePlayer) {
                                                player.transform.position = new Vector3(11.75f, -4.75f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(player);
                                                // Add Arrows pointing the deliver point
                                                if (ZombieLaboratory.localSurvivorsDeliverArrow.Count == 0) {
                                                    ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                                                    ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(true);
                                                }
                                            }
                                        }

                                        if (PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer) {
                                            ZombieLaboratory.nursePlayer.transform.position = new Vector3(-10.2f, 3.6f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(ZombieLaboratory.nursePlayer);
                                            GameObject mapMedKit = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                            mapMedKit.name = "mapMedKit";
                                            mapMedKit.transform.position = new Vector3(-7.25f, -5f, -0.1f);
                                            GameObject mapMedKittwo = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                            mapMedKittwo.name = "mapMedKittwo";
                                            mapMedKittwo.transform.position = new Vector3(3.75f, 3.5f, -0.1f);
                                            GameObject mapMedKitthree = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                            mapMedKitthree.name = "mapMedKitthree";
                                            mapMedKitthree.transform.position = new Vector3(-13.75f, -3.75f, -0.1f);
                                            ZombieLaboratory.nurseMedkits.Add(mapMedKit);
                                            ZombieLaboratory.nurseMedkits.Add(mapMedKittwo);
                                            ZombieLaboratory.nurseMedkits.Add(mapMedKitthree);
                                            // Add Arrows pointing the medkit only for nurse
                                            if (ZombieLaboratory.localNurseArrows.Count == 0 && ZombieLaboratory.localNurseArrows.Count < 3) {
                                                ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                                ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                                ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            }
                                            ZombieLaboratory.localNurseArrows[0].arrow.SetActive(true);
                                            ZombieLaboratory.localNurseArrows[1].arrow.SetActive(true);
                                            ZombieLaboratory.localNurseArrows[2].arrow.SetActive(true);
                                        }

                                        if (PlayerControl.LocalPlayer != null && !createdzombielaboratory) {
                                            GameObject laboratory = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerControl.LocalPlayer.transform.parent);
                                            laboratory.name = "laboratory";
                                            laboratory.transform.position = new Vector3(-10.25f, 3.38f, 0.5f);
                                            laboratory.gameObject.layer = 9;
                                            laboratory.transform.GetChild(0).gameObject.layer = 9;
                                            ZombieLaboratory.laboratory = laboratory;
                                            ZombieLaboratory.laboratoryEnterButton = laboratory.transform.GetChild(1).gameObject;
                                            ZombieLaboratory.laboratoryExitButton = laboratory.transform.GetChild(2).gameObject;
                                            ZombieLaboratory.laboratoryCreateCureButton = laboratory.transform.GetChild(3).gameObject;
                                            ZombieLaboratory.laboratoryPutKeyItemButton = laboratory.transform.GetChild(4).gameObject;
                                            ZombieLaboratory.laboratoryExitLeftButton = laboratory.transform.GetChild(5).gameObject;
                                            ZombieLaboratory.laboratoryExitRightButton = laboratory.transform.GetChild(6).gameObject;
                                            ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryEnterButton);
                                            ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitButton);
                                            ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitLeftButton);
                                            ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitRightButton);

                                            GameObject nurseMedKit = GameObject.Instantiate(CustomMain.customAssets.nurseMedKit, PlayerControl.LocalPlayer.transform.parent);
                                            nurseMedKit.name = "nurseMedKit";
                                            nurseMedKit.transform.parent = ZombieLaboratory.nursePlayer.transform;
                                            nurseMedKit.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                                            ZombieLaboratory.laboratoryNurseMedKit = nurseMedKit;
                                            ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                                            ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratoryEnterButton);
                                            createdzombielaboratory = true;
                                        }
                                    }
                                }
                                // Battle Royale
                                else if (BattleRoyale.battleRoyaleMode) {
                                    if (activatedSensei) {

                                        if (BattleRoyale.matchType == 0) {
                                            foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                                                soloPlayer.transform.position = new Vector3(BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].x, BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].y, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(soloPlayer);
                                                howmanyBattleRoyaleplayers += 1;
                                            }
                                        }
                                        else {
                                            if (PlayerControl.LocalPlayer == BattleRoyale.serialKiller) {
                                                BattleRoyale.serialKiller.transform.position = new Vector3(-3.65f, 5f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(BattleRoyale.serialKiller);
                                            }

                                            foreach (PlayerControl player in BattleRoyale.limeTeam) {
                                                player.transform.position = new Vector3(-17.5f, -1.15f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(player);
                                            }
                                            foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                                                player.transform.position = new Vector3(7.7f, -0.95f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(player);
                                            }
                                        }

                                        if (PlayerControl.LocalPlayer != null && !createdbattleroyale) {
                                            if (BattleRoyale.matchType != 0) {
                                                GameObject limeteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                                limeteamfloor.name = "limeteamfloor";
                                                limeteamfloor.transform.position = new Vector3(-17.5f, -1.15f, 0.5f);
                                                GameObject pinkteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                                pinkteamfloor.name = "pinkteamfloor";
                                                pinkteamfloor.transform.position = new Vector3(7.7f, -0.95f, 0.5f);
                                                BattleRoyale.serialKillerSpawns.Add(limeteamfloor);
                                                BattleRoyale.serialKillerSpawns.Add(pinkteamfloor);
                                            }
                                            createdbattleroyale = true;
                                        }
                                    }
                                    else {

                                        if (BattleRoyale.matchType == 0) {
                                            foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                                                soloPlayer.transform.position = new Vector3(BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].x, BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].y, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(soloPlayer);
                                                howmanyBattleRoyaleplayers += 1;
                                            }
                                        }
                                        else {

                                            if (PlayerControl.LocalPlayer == BattleRoyale.serialKiller) {
                                                BattleRoyale.serialKiller.transform.position = new Vector3(6.35f, -7.5f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(BattleRoyale.serialKiller);
                                            }

                                            foreach (PlayerControl player in BattleRoyale.limeTeam) {
                                                player.transform.position = new Vector3(-17f, -5.5f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(player);
                                            }
                                            foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                                                player.transform.position = new Vector3(12f, -4.75f, PlayerControl.LocalPlayer.transform.position.z);
                                                Helpers.clearAllTasks(player);
                                            }
                                        }

                                        if (PlayerControl.LocalPlayer != null && !createdbattleroyale) {
                                            if (BattleRoyale.matchType != 0) {
                                                GameObject limeteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                                limeteamfloor.name = "limeteamfloor";
                                                limeteamfloor.transform.position = new Vector3(-17f, -5.5f, 0.5f);
                                                GameObject pinkteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                                pinkteamfloor.name = "pinkteamfloor";
                                                pinkteamfloor.transform.position = new Vector3(12f, -4.75f, 0.5f);
                                                BattleRoyale.serialKillerSpawns.Add(limeteamfloor);
                                                BattleRoyale.serialKillerSpawns.Add(pinkteamfloor);
                                            }
                                            createdbattleroyale = true;
                                        }
                                    }
                                }
                                // Remove camera use and admin table on Skeld / Custom Skeld
                                GameObject cameraStand = GameObject.Find("SurvConsole");
                                cameraStand.GetComponent<PolygonCollider2D>().enabled = false;
                                GameObject admin = GameObject.Find("MapRoomConsole");
                                admin.GetComponent<CircleCollider2D>().enabled = false;
                                break;
                            // Mira HQ
                            case 1:
                                // Capture the flag
                                if (CaptureTheFlag.captureTheFlagMode) {
                                    if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(17.75f, 24f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(CaptureTheFlag.stealerPlayer);
                                    }

                                    foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(2.53f, 10.75f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(23.25f, 5.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdcapturetheflag) {
                                        GameObject redflag = GameObject.Instantiate(CustomMain.customAssets.redflag, PlayerControl.LocalPlayer.transform.parent);
                                        redflag.name = "redflag";
                                        redflag.transform.position = new Vector3(2.525f, 10.55f, 0.5f);
                                        CaptureTheFlag.redflag = redflag;
                                        GameObject redflagbase = GameObject.Instantiate(CustomMain.customAssets.redflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        redflagbase.name = "redflagbase";
                                        redflagbase.transform.position = new Vector3(2.53f, 10.5f, 1f);
                                        CaptureTheFlag.redflagbase = redflagbase;
                                        GameObject blueflag = GameObject.Instantiate(CustomMain.customAssets.blueflag, PlayerControl.LocalPlayer.transform.parent);
                                        blueflag.name = "blueflag";
                                        blueflag.transform.position = new Vector3(23.25f, 5.05f, 0.5f);
                                        CaptureTheFlag.blueflag = blueflag;
                                        GameObject blueflagbase = GameObject.Instantiate(CustomMain.customAssets.blueflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        blueflagbase.name = "blueflagbase";
                                        blueflagbase.transform.position = new Vector3(23.25f, 5f, 1f);
                                        CaptureTheFlag.blueflagbase = blueflagbase;
                                        CaptureTheFlag.stealerSpawns.Add(redflagbase);
                                        CaptureTheFlag.stealerSpawns.Add(blueflagbase);
                                        createdcapturetheflag = true;
                                    }
                                }
                                // Police And Thiefs
                                else if (PoliceAndThief.policeAndThiefMode) {
                                    foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(1.8f, -1f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                                        if (player == PlayerControl.LocalPlayer) {
                                            player.transform.position = new Vector3(17.75f, 11.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);

                                            // Add Arrows pointing the release and deliver point
                                            if (PoliceAndThief.localThiefReleaseArrow.Count == 0) {
                                                PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                                                PoliceAndThief.localThiefReleaseArrow[0].arrow.SetActive(true);
                                            }
                                            if (PoliceAndThief.localThiefDeliverArrow.Count == 0) {
                                                PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                                                PoliceAndThief.localThiefDeliverArrow[0].arrow.SetActive(true);
                                            }
                                        }
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdpoliceandthief) {
                                        GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                                        cell.name = "cell";
                                        cell.transform.position = new Vector3(1.75f, 1.125f, 0.5f);
                                        cell.gameObject.layer = 9;
                                        cell.transform.GetChild(0).gameObject.layer = 9;
                                        PoliceAndThief.cell = cell;
                                        GameObject cellbutton = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerControl.LocalPlayer.transform.parent);
                                        cellbutton.name = "cellbutton";
                                        cellbutton.transform.position = new Vector3(1.8f, -1.25f, 0.5f);
                                        PoliceAndThief.cellbutton = cellbutton;
                                        GameObject jewelbutton = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerControl.LocalPlayer.transform.parent);
                                        jewelbutton.name = "jewelbutton";
                                        jewelbutton.transform.position = new Vector3(18.5f, 13.85f, 0.5f);
                                        PoliceAndThief.jewelbutton = jewelbutton;
                                        GameObject thiefspaceship = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerControl.LocalPlayer.transform.parent);
                                        thiefspaceship.name = "thiefspaceship";
                                        thiefspaceship.transform.position = new Vector3(21.4f, 14.2f, 0.6f);
                                        createdpoliceandthief = true;

                                        // Spawn jewels
                                        GameObject jewel01 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel01.transform.position = new Vector3(-4.5f, 2.5f, 1f);
                                        jewel01.name = "jewel01";
                                        PoliceAndThief.jewel01 = jewel01;
                                        GameObject jewel02 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel02.transform.position = new Vector3(6.25f, 14f, 1f);
                                        jewel02.name = "jewel02";
                                        PoliceAndThief.jewel02 = jewel02;
                                        GameObject jewel03 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel03.transform.position = new Vector3(9.15f, 4.75f, 1f);
                                        jewel03.name = "jewel03";
                                        PoliceAndThief.jewel03 = jewel03;
                                        GameObject jewel04 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel04.transform.position = new Vector3(14.75f, 20.5f, 1f);
                                        jewel04.name = "jewel04";
                                        PoliceAndThief.jewel04 = jewel04;
                                        GameObject jewel05 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel05.transform.position = new Vector3(19.5f, 17.5f, 1f);
                                        jewel05.name = "jewel05";
                                        PoliceAndThief.jewel05 = jewel05;
                                        GameObject jewel06 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel06.transform.position = new Vector3(21, 24.1f, 1f);
                                        jewel06.name = "jewel06";
                                        PoliceAndThief.jewel06 = jewel06;
                                        GameObject jewel07 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel07.transform.position = new Vector3(19.5f, 4.75f, 1f);
                                        jewel07.name = "jewel07";
                                        PoliceAndThief.jewel07 = jewel07;
                                        GameObject jewel08 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel08.transform.position = new Vector3(28.25f, 0, 1f);
                                        jewel08.name = "jewel08";
                                        PoliceAndThief.jewel08 = jewel08;
                                        GameObject jewel09 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel09.transform.position = new Vector3(2.45f, 11.25f, 1f);
                                        jewel09.name = "jewel09";
                                        PoliceAndThief.jewel09 = jewel09;
                                        GameObject jewel10 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel10.transform.position = new Vector3(4.4f, 1.75f, 1f);
                                        jewel10.name = "jewel10";
                                        PoliceAndThief.jewel10 = jewel10;
                                        GameObject jewel11 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel11.transform.position = new Vector3(9.25f, 13f, 1f);
                                        jewel11.name = "jewel11";
                                        PoliceAndThief.jewel11 = jewel11;
                                        GameObject jewel12 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel12.transform.position = new Vector3(13.75f, 23.5f, 1f);
                                        jewel12.name = "jewel12";
                                        PoliceAndThief.jewel12 = jewel12;
                                        GameObject jewel13 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel13.transform.position = new Vector3(16, 4, 1f);
                                        jewel13.name = "jewel13";
                                        PoliceAndThief.jewel13 = jewel13;
                                        GameObject jewel14 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel14.transform.position = new Vector3(15.35f, -0.9f, 1f);
                                        jewel14.name = "jewel14";
                                        PoliceAndThief.jewel14 = jewel14;
                                        GameObject jewel15 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel15.transform.position = new Vector3(19.5f, -1.75f, 1f);
                                        jewel15.name = "jewel15";
                                        PoliceAndThief.jewel15 = jewel15;
                                        PoliceAndThief.thiefTreasures.Add(jewel01);
                                        PoliceAndThief.thiefTreasures.Add(jewel02);
                                        PoliceAndThief.thiefTreasures.Add(jewel03);
                                        PoliceAndThief.thiefTreasures.Add(jewel04);
                                        PoliceAndThief.thiefTreasures.Add(jewel05);
                                        PoliceAndThief.thiefTreasures.Add(jewel06);
                                        PoliceAndThief.thiefTreasures.Add(jewel07);
                                        PoliceAndThief.thiefTreasures.Add(jewel08);
                                        PoliceAndThief.thiefTreasures.Add(jewel09);
                                        PoliceAndThief.thiefTreasures.Add(jewel10);
                                        PoliceAndThief.thiefTreasures.Add(jewel11);
                                        PoliceAndThief.thiefTreasures.Add(jewel12);
                                        PoliceAndThief.thiefTreasures.Add(jewel13);
                                        PoliceAndThief.thiefTreasures.Add(jewel14);
                                        PoliceAndThief.thiefTreasures.Add(jewel15);
                                    }
                                }
                                // King Of The Hill
                                else if (KingOfTheHill.kingOfTheHillMode) {
                                    if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(2.5f, 11f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(KingOfTheHill.usurperPlayer);
                                    }

                                    foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-4.45f, 1.75f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(19.5f, 4.7f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdkingofthehill) {
                                        GameObject greenteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                        greenteamfloor.name = "greenteamfloor";
                                        greenteamfloor.transform.position = new Vector3(-4.45f, 1.5f, 0.5f);
                                        GameObject yellowteamfloor = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerControl.LocalPlayer.transform.parent);
                                        yellowteamfloor.name = "yellowteamfloor";
                                        yellowteamfloor.transform.position = new Vector3(19.5f, 4.45f, 0.5f);
                                        GameObject greenkingaura = GameObject.Instantiate(CustomMain.customAssets.greenaura, KingOfTheHill.greenKingplayer.transform);
                                        greenkingaura.name = "greenkingaura";
                                        greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                                        KingOfTheHill.greenkingaura = greenkingaura;
                                        GameObject yellowkingaura = GameObject.Instantiate(CustomMain.customAssets.yellowaura, KingOfTheHill.yellowKingplayer.transform);
                                        yellowkingaura.name = "yellowkingaura";
                                        yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                                        KingOfTheHill.yellowkingaura = yellowkingaura;
                                        GameObject flagzoneone = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzoneone.name = "flagzoneone";
                                        flagzoneone.transform.position = new Vector3(15.25f, 4f, 0.4f);
                                        KingOfTheHill.flagzoneone = flagzoneone;
                                        GameObject zoneone = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zoneone.name = "zoneone";
                                        zoneone.transform.position = new Vector3(15.25f, 4f, 0.5f);
                                        KingOfTheHill.zoneone = zoneone;
                                        GameObject flagzonetwo = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonetwo.name = "flagzonetwo";
                                        flagzonetwo.transform.position = new Vector3(17.85f, 19.5f, 0.4f);
                                        KingOfTheHill.flagzonetwo = flagzonetwo;
                                        GameObject zonetwo = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonetwo.name = "zonetwo";
                                        zonetwo.transform.position = new Vector3(17.85f, 19.5f, 0.5f);
                                        KingOfTheHill.zonetwo = zonetwo;
                                        GameObject flagzonethree = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonethree.name = "flagzonethree";
                                        flagzonethree.transform.position = new Vector3(6.15f, 12.5f, 0.4f);
                                        KingOfTheHill.flagzonethree = flagzonethree;
                                        GameObject zonethree = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonethree.name = "zonethree";
                                        zonethree.transform.position = new Vector3(6.15f, 12.5f, 0.5f);
                                        KingOfTheHill.zonethree = zonethree;
                                        KingOfTheHill.kingZones.Add(zoneone);
                                        KingOfTheHill.kingZones.Add(zonetwo);
                                        KingOfTheHill.kingZones.Add(zonethree);
                                        KingOfTheHill.usurperSpawns.Add(greenteamfloor);
                                        KingOfTheHill.usurperSpawns.Add(yellowteamfloor);
                                        createdkingofthehill = true;
                                    }
                                }
                                // Hot Potato
                                else if (HotPotato.hotPotatoMode) {
                                    if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                                        HotPotato.hotPotatoPlayer.transform.position = new Vector3(6.15f, 6.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(HotPotato.hotPotatoPlayer);
                                    }

                                    foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(17.75f, 11.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdhotpotato) {
                                        GameObject hotpotato = GameObject.Instantiate(CustomMain.customAssets.hotPotato, HotPotato.hotPotatoPlayer.transform);
                                        hotpotato.name = "hotpotato";
                                        hotpotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                        HotPotato.hotPotato = hotpotato;
                                        createdhotpotato = true;
                                    }
                                }
                                // Zombie Laboratory
                                else if (ZombieLaboratory.zombieLaboratoryMode) {
                                    foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(18.5f, -1.85f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                                        if (player == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != ZombieLaboratory.nursePlayer) {
                                            player.transform.position = new Vector3(6.1f, 5.75f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                            // Add Arrows pointing the deliver point
                                            if (ZombieLaboratory.localSurvivorsDeliverArrow.Count == 0) {
                                                ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                                                ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(true);
                                            }
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer) {
                                        ZombieLaboratory.nursePlayer.transform.position = new Vector3(1.8f, 1.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(ZombieLaboratory.nursePlayer);
                                        GameObject mapMedKit = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKit.name = "mapMedKit";
                                        mapMedKit.transform.position = new Vector3(16.25f, 0.25f, -0.1f);
                                        GameObject mapMedKittwo = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKittwo.name = "mapMedKittwo";
                                        mapMedKittwo.transform.position = new Vector3(8.5f, 13.75f, -0.1f);
                                        GameObject mapMedKitthree = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKitthree.name = "mapMedKitthree";
                                        mapMedKitthree.transform.position = new Vector3(-4.5f, 3.5f, -0.1f);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKit);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKittwo);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKitthree);
                                        // Add Arrows pointing the medkit only for nurse
                                        if (ZombieLaboratory.localNurseArrows.Count == 0 && ZombieLaboratory.localNurseArrows.Count < 3) {
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                        }
                                        ZombieLaboratory.localNurseArrows[0].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[1].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[2].arrow.SetActive(true);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdzombielaboratory) {
                                        GameObject laboratory = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerControl.LocalPlayer.transform.parent);
                                        laboratory.name = "laboratory";
                                        laboratory.transform.position = new Vector3(1.75f, 1.125f, 0.5f);
                                        laboratory.gameObject.layer = 9;
                                        laboratory.transform.GetChild(0).gameObject.layer = 9;
                                        ZombieLaboratory.laboratory = laboratory;
                                        ZombieLaboratory.laboratoryEnterButton = laboratory.transform.GetChild(1).gameObject;
                                        ZombieLaboratory.laboratoryExitButton = laboratory.transform.GetChild(2).gameObject;
                                        ZombieLaboratory.laboratoryCreateCureButton = laboratory.transform.GetChild(3).gameObject;
                                        ZombieLaboratory.laboratoryPutKeyItemButton = laboratory.transform.GetChild(4).gameObject;
                                        ZombieLaboratory.laboratoryExitLeftButton = laboratory.transform.GetChild(5).gameObject;
                                        ZombieLaboratory.laboratoryExitRightButton = laboratory.transform.GetChild(6).gameObject;
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryEnterButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitLeftButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitRightButton);

                                        GameObject nurseMedKit = GameObject.Instantiate(CustomMain.customAssets.nurseMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        nurseMedKit.name = "nurseMedKit";
                                        nurseMedKit.transform.parent = ZombieLaboratory.nursePlayer.transform;
                                        nurseMedKit.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                                        ZombieLaboratory.laboratoryNurseMedKit = nurseMedKit;
                                        ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                                        ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratoryEnterButton);
                                        createdzombielaboratory = true;
                                    }
                                }
                                // Battle Royale
                                else if (BattleRoyale.battleRoyaleMode) {
                                    if (BattleRoyale.matchType == 0) {
                                        foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                                            soloPlayer.transform.position = new Vector3(BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].x, BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].y, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(soloPlayer);
                                            howmanyBattleRoyaleplayers += 1;
                                        }
                                    }
                                    else {
                                        if (PlayerControl.LocalPlayer == BattleRoyale.serialKiller) {
                                            BattleRoyale.serialKiller.transform.position = new Vector3(16.25f, 24.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(BattleRoyale.serialKiller);
                                        }

                                        foreach (PlayerControl player in BattleRoyale.limeTeam) {
                                            player.transform.position = new Vector3(6.15f, 13.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                                            player.transform.position = new Vector3(22.25f, 3f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdbattleroyale) {
                                        if (BattleRoyale.matchType != 0) {
                                            GameObject limeteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                            limeteamfloor.name = "limeteamfloor";
                                            limeteamfloor.transform.position = new Vector3(6.15f, 13.25f, 0.5f);
                                            GameObject pinkteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                            pinkteamfloor.name = "pinkteamfloor";
                                            pinkteamfloor.transform.position = new Vector3(22.25f, 3f, 0.5f);
                                            BattleRoyale.serialKillerSpawns.Add(limeteamfloor);
                                            BattleRoyale.serialKillerSpawns.Add(pinkteamfloor);
                                        }
                                        createdbattleroyale = true;
                                    }
                                }
                                // Remove Doorlog use, Decontamination doors and admin table on MiraHQ
                                GameObject DoorLog = GameObject.Find("SurvLogConsole");
                                DoorLog.GetComponent<BoxCollider2D>().enabled = false;
                                GameObject deconUpperDoor = GameObject.Find("UpperDoor");
                                deconUpperDoor.SetActive(false);
                                GameObject deconLowerDoor = GameObject.Find("LowerDoor");
                                deconLowerDoor.SetActive(false);
                                GameObject deconUpperDoorPanelTop = GameObject.Find("DeconDoorPanel-Top");
                                deconUpperDoorPanelTop.SetActive(false);
                                GameObject deconUpperDoorPanelHigh = GameObject.Find("DeconDoorPanel-High");
                                deconUpperDoorPanelHigh.SetActive(false);
                                GameObject deconUpperDoorPanelBottom = GameObject.Find("DeconDoorPanel-Bottom");
                                deconUpperDoorPanelBottom.SetActive(false);
                                GameObject deconUpperDoorPanelLow = GameObject.Find("DeconDoorPanel-Low");
                                deconUpperDoorPanelLow.SetActive(false);
                                GameObject miraAdmin = GameObject.Find("AdminMapConsole");
                                miraAdmin.GetComponent<CircleCollider2D>().enabled = false;
                                break;
                            // Polus
                            case 2:
                                // Capture The Flag
                                if (CaptureTheFlag.captureTheFlagMode) {
                                    if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(31.75f, -13f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(CaptureTheFlag.stealerPlayer);
                                    }

                                    foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(36.4f, -21.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(5.4f, -9.45f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdcapturetheflag) {
                                        GameObject redflag = GameObject.Instantiate(CustomMain.customAssets.redflag, PlayerControl.LocalPlayer.transform.parent);
                                        redflag.name = "redflag";
                                        redflag.transform.position = new Vector3(36.4f, -21.7f, 0.5f);
                                        CaptureTheFlag.redflag = redflag;
                                        GameObject redflagbase = GameObject.Instantiate(CustomMain.customAssets.redflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        redflagbase.name = "redflagbase";
                                        redflagbase.transform.position = new Vector3(36.4f, -21.75f, 1f);
                                        CaptureTheFlag.redflagbase = redflagbase;
                                        GameObject blueflag = GameObject.Instantiate(CustomMain.customAssets.blueflag, PlayerControl.LocalPlayer.transform.parent);
                                        blueflag.name = "blueflag";
                                        blueflag.transform.position = new Vector3(5.4f, -9.65f, 0.5f);
                                        CaptureTheFlag.blueflag = blueflag;
                                        GameObject blueflagbase = GameObject.Instantiate(CustomMain.customAssets.blueflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        blueflagbase.name = "blueflagbase";
                                        blueflagbase.transform.position = new Vector3(5.4f, -9.7f, 1f);
                                        CaptureTheFlag.blueflagbase = blueflagbase;
                                        CaptureTheFlag.stealerSpawns.Add(redflagbase);
                                        CaptureTheFlag.stealerSpawns.Add(blueflagbase);
                                        createdcapturetheflag = true;
                                    }
                                }
                                // Police And Thiefs
                                else if (PoliceAndThief.policeAndThiefMode) {
                                    foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(8.18f, -7.4f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                                        if (player == PlayerControl.LocalPlayer) {
                                            player.transform.position = new Vector3(30f, -15.75f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);

                                            // Add Arrows pointing the release and deliver point
                                            if (PoliceAndThief.localThiefReleaseArrow.Count == 0) {
                                                PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                                                PoliceAndThief.localThiefReleaseArrow[0].arrow.SetActive(true);
                                            }
                                            if (PoliceAndThief.localThiefDeliverArrow.Count == 0) {
                                                PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                                                PoliceAndThief.localThiefDeliverArrow[0].arrow.SetActive(true);
                                            }
                                        }
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdpoliceandthief) {
                                        GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                                        cell.name = "cell";
                                        cell.transform.position = new Vector3(8.25f, -5.15f, 0.5f);
                                        cell.gameObject.layer = 9;
                                        cell.transform.GetChild(0).gameObject.layer = 9;
                                        PoliceAndThief.cell = cell;
                                        GameObject cellbutton = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerControl.LocalPlayer.transform.parent);
                                        cellbutton.name = "cellbutton";
                                        cellbutton.transform.position = new Vector3(8.2f, -7.5f, 0.5f);
                                        PoliceAndThief.cellbutton = cellbutton;
                                        GameObject jewelbutton = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerControl.LocalPlayer.transform.parent);
                                        jewelbutton.name = "jewelbutton";
                                        jewelbutton.transform.position = new Vector3(32.25f, -15.9f, 0.5f);
                                        PoliceAndThief.jewelbutton = jewelbutton;
                                        GameObject thiefspaceship = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerControl.LocalPlayer.transform.parent);
                                        thiefspaceship.name = "thiefspaceship";
                                        thiefspaceship.transform.position = new Vector3(35.35f, -15.55f, 0.8f);
                                        createdpoliceandthief = true;

                                        // Spawn jewels
                                        GameObject jewel01 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel01.transform.position = new Vector3(16.7f, -2.65f, 0.75f);
                                        jewel01.name = "jewel01";
                                        PoliceAndThief.jewel01 = jewel01;
                                        GameObject jewel02 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel02.transform.position = new Vector3(25.35f, -7.35f, 0.75f);
                                        jewel02.name = "jewel02";
                                        PoliceAndThief.jewel02 = jewel02;
                                        GameObject jewel03 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel03.transform.position = new Vector3(34.9f, -9.75f, 0.75f);
                                        jewel03.name = "jewel03";
                                        PoliceAndThief.jewel03 = jewel03;
                                        GameObject jewel04 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel04.transform.position = new Vector3(36.5f, -21.75f, 0.75f);
                                        jewel04.name = "jewel04";
                                        PoliceAndThief.jewel04 = jewel04;
                                        GameObject jewel05 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel05.transform.position = new Vector3(17.25f, -17.5f, 0.75f);
                                        jewel05.name = "jewel05";
                                        PoliceAndThief.jewel05 = jewel05;
                                        GameObject jewel06 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel06.transform.position = new Vector3(10.9f, -20.5f, -0.75f);
                                        jewel06.name = "jewel06";
                                        PoliceAndThief.jewel06 = jewel06;
                                        GameObject jewel07 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel07.transform.position = new Vector3(1.5f, -20.25f, 0.75f);
                                        jewel07.name = "jewel07";
                                        PoliceAndThief.jewel07 = jewel07;
                                        GameObject jewel08 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel08.transform.position = new Vector3(3f, -12f, 0.75f);
                                        jewel08.name = "jewel08";
                                        PoliceAndThief.jewel08 = jewel08;
                                        GameObject jewel09 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel09.transform.position = new Vector3(30f, -7.35f, 0.75f);
                                        jewel09.name = "jewel09";
                                        PoliceAndThief.jewel09 = jewel09;
                                        GameObject jewel10 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel10.transform.position = new Vector3(40.25f, -8f, 0.75f);
                                        jewel10.name = "jewel10";
                                        PoliceAndThief.jewel10 = jewel10;
                                        GameObject jewel11 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel11.transform.position = new Vector3(26f, -17.15f, 0.75f);
                                        jewel11.name = "jewel11";
                                        PoliceAndThief.jewel11 = jewel11;
                                        GameObject jewel12 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel12.transform.position = new Vector3(22f, -25.25f, 0.75f);
                                        jewel12.name = "jewel12";
                                        PoliceAndThief.jewel12 = jewel12;
                                        GameObject jewel13 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel13.transform.position = new Vector3(20.65f, -12f, 0.75f);
                                        jewel13.name = "jewel13";
                                        PoliceAndThief.jewel13 = jewel13;
                                        GameObject jewel14 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel14.transform.position = new Vector3(9.75f, -12.25f, 0.75f);
                                        jewel14.name = "jewel14";
                                        PoliceAndThief.jewel14 = jewel14;
                                        GameObject jewel15 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel15.transform.position = new Vector3(2.25f, -24f, 0.75f);
                                        jewel15.name = "jewel15";
                                        PoliceAndThief.jewel15 = jewel15;
                                        PoliceAndThief.thiefTreasures.Add(jewel01);
                                        PoliceAndThief.thiefTreasures.Add(jewel02);
                                        PoliceAndThief.thiefTreasures.Add(jewel03);
                                        PoliceAndThief.thiefTreasures.Add(jewel04);
                                        PoliceAndThief.thiefTreasures.Add(jewel05);
                                        PoliceAndThief.thiefTreasures.Add(jewel06);
                                        PoliceAndThief.thiefTreasures.Add(jewel07);
                                        PoliceAndThief.thiefTreasures.Add(jewel08);
                                        PoliceAndThief.thiefTreasures.Add(jewel09);
                                        PoliceAndThief.thiefTreasures.Add(jewel10);
                                        PoliceAndThief.thiefTreasures.Add(jewel11);
                                        PoliceAndThief.thiefTreasures.Add(jewel12);
                                        PoliceAndThief.thiefTreasures.Add(jewel13);
                                        PoliceAndThief.thiefTreasures.Add(jewel14);
                                        PoliceAndThief.thiefTreasures.Add(jewel15);
                                    }
                                }
                                // King Of The Hill
                                else if (KingOfTheHill.kingOfTheHillMode) {
                                    if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(20.5f, -12f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(KingOfTheHill.usurperPlayer);
                                    }

                                    foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(2.25f, -23.75f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(36.35f, -6.15f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdkingofthehill) {
                                        GameObject greenteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                        greenteamfloor.name = "greenteamfloor";
                                        greenteamfloor.transform.position = new Vector3(2.25f, -24f, 0.5f);
                                        GameObject yellowteamfloor = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerControl.LocalPlayer.transform.parent);
                                        yellowteamfloor.name = "yellowteamfloor";
                                        yellowteamfloor.transform.position = new Vector3(36.35f, -6.4f, 0.5f);
                                        GameObject greenkingaura = GameObject.Instantiate(CustomMain.customAssets.greenaura, KingOfTheHill.greenKingplayer.transform);
                                        greenkingaura.name = "greenkingaura";
                                        greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                                        KingOfTheHill.greenkingaura = greenkingaura;
                                        GameObject yellowkingaura = GameObject.Instantiate(CustomMain.customAssets.yellowaura, KingOfTheHill.yellowKingplayer.transform);
                                        yellowkingaura.name = "yellowkingaura";
                                        yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                                        KingOfTheHill.yellowkingaura = yellowkingaura;
                                        GameObject flagzoneone = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzoneone.name = "flagzoneone";
                                        flagzoneone.transform.position = new Vector3(15f, -13.5f, 0.4f);
                                        KingOfTheHill.flagzoneone = flagzoneone;
                                        GameObject zoneone = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zoneone.name = "zoneone";
                                        zoneone.transform.position = new Vector3(15f, -13.5f, 0.5f);
                                        KingOfTheHill.zoneone = zoneone;
                                        GameObject flagzonetwo = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonetwo.name = "flagzonetwo";
                                        flagzonetwo.transform.position = new Vector3(20.75f, -22.75f, 0.4f);
                                        KingOfTheHill.flagzonetwo = flagzonetwo;
                                        GameObject zonetwo = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonetwo.name = "zonetwo";
                                        zonetwo.transform.position = new Vector3(20.75f, -22.75f, 0.5f);
                                        KingOfTheHill.zonetwo = zonetwo;
                                        GameObject flagzonethree = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonethree.name = "flagzonethree";
                                        flagzonethree.transform.position = new Vector3(16.65f, -1.5f, 0.4f);
                                        KingOfTheHill.flagzonethree = flagzonethree;
                                        GameObject zonethree = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonethree.name = "zonethree";
                                        zonethree.transform.position = new Vector3(16.65f, -1.5f, 0.5f);
                                        KingOfTheHill.zonethree = zonethree;
                                        KingOfTheHill.kingZones.Add(zoneone);
                                        KingOfTheHill.kingZones.Add(zonetwo);
                                        KingOfTheHill.kingZones.Add(zonethree);
                                        KingOfTheHill.usurperSpawns.Add(greenteamfloor);
                                        KingOfTheHill.usurperSpawns.Add(yellowteamfloor);
                                        createdkingofthehill = true;
                                    }
                                }
                                // Hot Potato
                                else if (HotPotato.hotPotatoMode) {
                                    if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                                        HotPotato.hotPotatoPlayer.transform.position = new Vector3(20.5f, -11.75f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(HotPotato.hotPotatoPlayer);
                                    }

                                    foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(12.25f, -16f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdhotpotato) {
                                        GameObject hotpotato = GameObject.Instantiate(CustomMain.customAssets.hotPotato, HotPotato.hotPotatoPlayer.transform);
                                        hotpotato.name = "hotpotato";
                                        hotpotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                        HotPotato.hotPotato = hotpotato;
                                        createdhotpotato = true;
                                    }
                                }
                                // Zombie Laboratory
                                else if (ZombieLaboratory.zombieLaboratoryMode) {
                                    foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(17.15f, -17.15f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                                        if (player == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != ZombieLaboratory.nursePlayer) {
                                            player.transform.position = new Vector3(40.4f, -6.8f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                            // Add Arrows pointing the deliver point
                                            if (ZombieLaboratory.localSurvivorsDeliverArrow.Count == 0) {
                                                ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                                                ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(true);
                                            }
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer) {
                                        ZombieLaboratory.nursePlayer.transform.position = new Vector3(16.65f, -2.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(ZombieLaboratory.nursePlayer);
                                        GameObject mapMedKit = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKit.name = "mapMedKit";
                                        mapMedKit.transform.position = new Vector3(20.75f, -12f, -0.1f);
                                        GameObject mapMedKittwo = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKittwo.name = "mapMedKittwo";
                                        mapMedKittwo.transform.position = new Vector3(3.5f, -11.75f, -0.1f);
                                        GameObject mapMedKitthree = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKitthree.name = "mapMedKitthree";
                                        mapMedKitthree.transform.position = new Vector3(31.5f, -7.5f, -0.1f);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKit);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKittwo);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKitthree);
                                        // Add Arrows pointing the medkit only for nurse
                                        if (ZombieLaboratory.localNurseArrows.Count == 0 && ZombieLaboratory.localNurseArrows.Count < 3) {
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                        }
                                        ZombieLaboratory.localNurseArrows[0].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[1].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[2].arrow.SetActive(true);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdzombielaboratory) {
                                        GameObject laboratory = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerControl.LocalPlayer.transform.parent);
                                        laboratory.name = "laboratory";
                                        laboratory.transform.position = new Vector3(16.68f, -2.52f, 0.5f);
                                        laboratory.gameObject.layer = 9;
                                        laboratory.transform.GetChild(0).gameObject.layer = 9;
                                        ZombieLaboratory.laboratory = laboratory;
                                        ZombieLaboratory.laboratoryEnterButton = laboratory.transform.GetChild(1).gameObject;
                                        ZombieLaboratory.laboratoryExitButton = laboratory.transform.GetChild(2).gameObject;
                                        ZombieLaboratory.laboratoryCreateCureButton = laboratory.transform.GetChild(3).gameObject;
                                        ZombieLaboratory.laboratoryPutKeyItemButton = laboratory.transform.GetChild(4).gameObject;
                                        ZombieLaboratory.laboratoryExitLeftButton = laboratory.transform.GetChild(5).gameObject;
                                        ZombieLaboratory.laboratoryExitRightButton = laboratory.transform.GetChild(6).gameObject;
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryEnterButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitLeftButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitRightButton);

                                        GameObject nurseMedKit = GameObject.Instantiate(CustomMain.customAssets.nurseMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        nurseMedKit.name = "nurseMedKit";
                                        nurseMedKit.transform.parent = ZombieLaboratory.nursePlayer.transform;
                                        nurseMedKit.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                                        ZombieLaboratory.laboratoryNurseMedKit = nurseMedKit;
                                        ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                                        ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratoryEnterButton);
                                        createdzombielaboratory = true;
                                    }
                                }
                                // Battle Royale
                                else if (BattleRoyale.battleRoyaleMode) {
                                    if (BattleRoyale.matchType == 0) {
                                        foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                                            soloPlayer.transform.position = new Vector3(BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].x, BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].y, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(soloPlayer);
                                            howmanyBattleRoyaleplayers += 1;
                                        }
                                    }
                                    else {
                                        if (PlayerControl.LocalPlayer == BattleRoyale.serialKiller) {
                                            BattleRoyale.serialKiller.transform.position = new Vector3(22.3f, -19.15f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(BattleRoyale.serialKiller);
                                        }

                                        foreach (PlayerControl player in BattleRoyale.limeTeam) {
                                            player.transform.position = new Vector3(2.35f, -23.75f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                                            player.transform.position = new Vector3(36.35f, -8f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdbattleroyale) {
                                        if (BattleRoyale.matchType != 0) {
                                            GameObject limeteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                            limeteamfloor.name = "limeteamfloor";
                                            limeteamfloor.transform.position = new Vector3(2.35f, -23.75f, 0.5f);
                                            GameObject pinkteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                            pinkteamfloor.name = "pinkteamfloor";
                                            pinkteamfloor.transform.position = new Vector3(36.35f, -8f, 0.5f);
                                            BattleRoyale.serialKillerSpawns.Add(limeteamfloor);
                                            BattleRoyale.serialKillerSpawns.Add(pinkteamfloor);
                                        }
                                        createdbattleroyale = true;
                                    }
                                }
                                // Remove Decon doors, camera use, vitals, admin tables on Polus
                                GameObject lowerdecon = GameObject.Find("LowerDecon");
                                lowerdecon.SetActive(false);
                                GameObject upperdecon = GameObject.Find("UpperDecon");
                                upperdecon.SetActive(false);
                                GameObject survCameras = GameObject.Find("Surv_Panel");
                                survCameras.GetComponent<BoxCollider2D>().enabled = false;
                                GameObject vitals = GameObject.Find("panel_vitals");
                                vitals.GetComponent<BoxCollider2D>().enabled = false;
                                GameObject adminone = GameObject.Find("panel_map");
                                adminone.GetComponent<BoxCollider2D>().enabled = false;
                                GameObject admintwo = GameObject.Find("panel_map (1)");
                                admintwo.GetComponent<BoxCollider2D>().enabled = false;
                                GameObject ramp = GameObject.Find("ramp");
                                ramp.transform.position = new Vector3(ramp.transform.position.x, ramp.transform.position.y, 0.75f);
                                break;
                            // Dleks
                            case 3:
                                // Capture the flag
                                if (CaptureTheFlag.captureTheFlagMode) {
                                    if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(-6.35f, -7.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(CaptureTheFlag.stealerPlayer);
                                    }

                                    foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(20.5f, -5.15f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-16.5f, -4.45f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdcapturetheflag) {
                                        GameObject redflag = GameObject.Instantiate(CustomMain.customAssets.redflag, PlayerControl.LocalPlayer.transform.parent);
                                        redflag.name = "redflag";
                                        redflag.transform.position = new Vector3(20.5f, -5.35f, 0.5f);
                                        CaptureTheFlag.redflag = redflag;
                                        GameObject redflagbase = GameObject.Instantiate(CustomMain.customAssets.redflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        redflagbase.name = "redflagbase";
                                        redflagbase.transform.position = new Vector3(20.5f, -5.4f, 1f);
                                        CaptureTheFlag.redflagbase = redflagbase;
                                        GameObject blueflag = GameObject.Instantiate(CustomMain.customAssets.blueflag, PlayerControl.LocalPlayer.transform.parent);
                                        blueflag.name = "blueflag";
                                        blueflag.transform.position = new Vector3(-16.5f, -4.65f, 0.5f);
                                        CaptureTheFlag.blueflag = blueflag;
                                        GameObject blueflagbase = GameObject.Instantiate(CustomMain.customAssets.blueflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        blueflagbase.name = "blueflagbase";
                                        blueflagbase.transform.position = new Vector3(-16.5f, -4.7f, 1f);
                                        CaptureTheFlag.blueflagbase = blueflagbase;
                                        CaptureTheFlag.stealerSpawns.Add(redflagbase);
                                        CaptureTheFlag.stealerSpawns.Add(blueflagbase);
                                        createdcapturetheflag = true;
                                    }
                                }
                                // Police And Thiefs
                                else if (PoliceAndThief.policeAndThiefMode) {
                                    foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(10.2f, 1.18f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                                        if (player == PlayerControl.LocalPlayer) {
                                            player.transform.position = new Vector3(1.31f, -16.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);

                                            // Add Arrows pointing the release and deliver point
                                            if (PoliceAndThief.localThiefReleaseArrow.Count == 0) {
                                                PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                                                PoliceAndThief.localThiefReleaseArrow[0].arrow.SetActive(true);
                                            }
                                            if (PoliceAndThief.localThiefDeliverArrow.Count == 0) {
                                                PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                                                PoliceAndThief.localThiefDeliverArrow[0].arrow.SetActive(true);
                                            }
                                        }
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdpoliceandthief) {
                                        GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                                        cell.name = "cell";
                                        cell.transform.position = new Vector3(10.25f, 3.38f, 0.5f);
                                        cell.gameObject.layer = 9;
                                        cell.transform.GetChild(0).gameObject.layer = 9;
                                        PoliceAndThief.cell = cell;
                                        GameObject cellbutton = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerControl.LocalPlayer.transform.parent);
                                        cellbutton.name = "cellbutton";
                                        cellbutton.transform.position = new Vector3(10.2f, 0.93f, 0.5f);
                                        PoliceAndThief.cellbutton = cellbutton;
                                        GameObject jewelbutton = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerControl.LocalPlayer.transform.parent);
                                        jewelbutton.name = "jewelbutton";
                                        jewelbutton.transform.position = new Vector3(-0.20f, -17.15f, 0.5f);
                                        PoliceAndThief.jewelbutton = jewelbutton;
                                        GameObject thiefspaceship = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerControl.LocalPlayer.transform.parent);
                                        thiefspaceship.name = "thiefspaceship";
                                        thiefspaceship.transform.position = new Vector3(1.345f, -19.16f, 0.6f);
                                        GameObject thiefspaceshiphatch = GameObject.Instantiate(CustomMain.customAssets.thiefspaceshiphatch, PlayerControl.LocalPlayer.transform.parent);
                                        thiefspaceshiphatch.name = "thiefspaceshiphatch";
                                        thiefspaceshiphatch.transform.position = new Vector3(1.345f, -19.16f, 0.6f);
                                        createdpoliceandthief = true;

                                        // Spawn jewels
                                        GameObject jewel01 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel01.transform.position = new Vector3(18.65f, -9.9f, 1f);
                                        jewel01.name = "jewel01";
                                        PoliceAndThief.jewel01 = jewel01;
                                        GameObject jewel02 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel02.transform.position = new Vector3(21.5f, -2, 1f);
                                        jewel02.name = "jewel02";
                                        PoliceAndThief.jewel02 = jewel02;
                                        GameObject jewel03 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel03.transform.position = new Vector3(5.9f, -8.25f, 1f);
                                        jewel03.name = "jewel03";
                                        PoliceAndThief.jewel03 = jewel03;
                                        GameObject jewel04 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel04.transform.position = new Vector3(-4.5f, -7.5f, 1f);
                                        jewel04.name = "jewel04";
                                        PoliceAndThief.jewel04 = jewel04;
                                        GameObject jewel05 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel05.transform.position = new Vector3(-7.85f, -14.45f, 1f);
                                        jewel05.name = "jewel05";
                                        PoliceAndThief.jewel05 = jewel05;
                                        GameObject jewel06 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel06.transform.position = new Vector3(-6.65f, -4.8f, 1f);
                                        jewel06.name = "jewel06";
                                        PoliceAndThief.jewel06 = jewel06;
                                        GameObject jewel07 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel07.transform.position = new Vector3(-10.5f, 2.15f, 1f);
                                        jewel07.name = "jewel07";
                                        PoliceAndThief.jewel07 = jewel07;
                                        GameObject jewel08 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel08.transform.position = new Vector3(5.5f, 3.5f, 1f);
                                        jewel08.name = "jewel08";
                                        PoliceAndThief.jewel08 = jewel08;
                                        GameObject jewel09 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel09.transform.position = new Vector3(19, -1.2f, 1f);
                                        jewel09.name = "jewel09";
                                        PoliceAndThief.jewel09 = jewel09;
                                        GameObject jewel10 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel10.transform.position = new Vector3(21.5f, -8.35f, 1f);
                                        jewel10.name = "jewel10";
                                        PoliceAndThief.jewel10 = jewel10;
                                        GameObject jewel11 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel11.transform.position = new Vector3(12.5f, -3.75f, 1f);
                                        jewel11.name = "jewel11";
                                        PoliceAndThief.jewel11 = jewel11;
                                        GameObject jewel12 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel12.transform.position = new Vector3(5.9f, -5.25f, 1f);
                                        jewel12.name = "jewel12";
                                        PoliceAndThief.jewel12 = jewel12;
                                        GameObject jewel13 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel13.transform.position = new Vector3(-2.65f, -16.5f, 1f);
                                        jewel13.name = "jewel13";
                                        PoliceAndThief.jewel13 = jewel13;
                                        GameObject jewel14 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel14.transform.position = new Vector3(-16.75f, -4.75f, 1f);
                                        jewel14.name = "jewel14";
                                        PoliceAndThief.jewel14 = jewel14;
                                        GameObject jewel15 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel15.transform.position = new Vector3(-3.8f, 3.5f, 1f);
                                        jewel15.name = "jewel15";
                                        PoliceAndThief.jewel15 = jewel15;
                                        PoliceAndThief.thiefTreasures.Add(jewel01);
                                        PoliceAndThief.thiefTreasures.Add(jewel02);
                                        PoliceAndThief.thiefTreasures.Add(jewel03);
                                        PoliceAndThief.thiefTreasures.Add(jewel04);
                                        PoliceAndThief.thiefTreasures.Add(jewel05);
                                        PoliceAndThief.thiefTreasures.Add(jewel06);
                                        PoliceAndThief.thiefTreasures.Add(jewel07);
                                        PoliceAndThief.thiefTreasures.Add(jewel08);
                                        PoliceAndThief.thiefTreasures.Add(jewel09);
                                        PoliceAndThief.thiefTreasures.Add(jewel10);
                                        PoliceAndThief.thiefTreasures.Add(jewel11);
                                        PoliceAndThief.thiefTreasures.Add(jewel12);
                                        PoliceAndThief.thiefTreasures.Add(jewel13);
                                        PoliceAndThief.thiefTreasures.Add(jewel14);
                                        PoliceAndThief.thiefTreasures.Add(jewel15);
                                    }
                                }
                                // King Of The Hill
                                else if (KingOfTheHill.kingOfTheHillMode) {
                                    if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(1f, 5.35f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(KingOfTheHill.usurperPlayer);
                                    }

                                    foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(7f, -8.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-6.25f, -3.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdkingofthehill) {
                                        GameObject greenteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                        greenteamfloor.name = "greenteamfloor";
                                        greenteamfloor.transform.position = new Vector3(7f, -8.5f, 0.5f);
                                        GameObject yellowteamfloor = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerControl.LocalPlayer.transform.parent);
                                        yellowteamfloor.name = "yellowteamfloor";
                                        yellowteamfloor.transform.position = new Vector3(-6.25f, -3.75f, 0.5f);
                                        GameObject greenkingaura = GameObject.Instantiate(CustomMain.customAssets.greenaura, KingOfTheHill.greenKingplayer.transform);
                                        greenkingaura.name = "greenkingaura";
                                        greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                                        KingOfTheHill.greenkingaura = greenkingaura;
                                        GameObject yellowkingaura = GameObject.Instantiate(CustomMain.customAssets.yellowaura, KingOfTheHill.yellowKingplayer.transform);
                                        yellowkingaura.name = "yellowkingaura";
                                        yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                                        KingOfTheHill.yellowkingaura = yellowkingaura;
                                        GameObject flagzoneone = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzoneone.name = "flagzoneone";
                                        flagzoneone.transform.position = new Vector3(9.1f, -2.25f, 0.4f);
                                        KingOfTheHill.flagzoneone = flagzoneone;
                                        GameObject zoneone = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zoneone.name = "zoneone";
                                        zoneone.transform.position = new Vector3(9.1f, -2.25f, 0.5f);
                                        KingOfTheHill.zoneone = zoneone;
                                        GameObject flagzonetwo = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonetwo.name = "flagzonetwo";
                                        flagzonetwo.transform.position = new Vector3(-4.5f, -7.5f, 0.4f);
                                        KingOfTheHill.flagzonetwo = flagzonetwo;
                                        GameObject zonetwo = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonetwo.name = "zonetwo";
                                        zonetwo.transform.position = new Vector3(-4.5f, -7.5f, 0.5f);
                                        KingOfTheHill.zonetwo = zonetwo;
                                        GameObject flagzonethree = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonethree.name = "flagzonethree";
                                        flagzonethree.transform.position = new Vector3(-3.25f, -15.5f, 0.4f);
                                        KingOfTheHill.flagzonethree = flagzonethree;
                                        GameObject zonethree = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonethree.name = "zonethree";
                                        zonethree.transform.position = new Vector3(-3.25f, -15.5f, 0.5f);
                                        KingOfTheHill.zonethree = zonethree;
                                        KingOfTheHill.kingZones.Add(zoneone);
                                        KingOfTheHill.kingZones.Add(zonetwo);
                                        KingOfTheHill.kingZones.Add(zonethree);
                                        KingOfTheHill.usurperSpawns.Add(greenteamfloor);
                                        KingOfTheHill.usurperSpawns.Add(yellowteamfloor);
                                        createdkingofthehill = true;
                                    }
                                }
                                // Hot Potato
                                else if (HotPotato.hotPotatoMode) {
                                    if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                                        HotPotato.hotPotatoPlayer.transform.position = new Vector3(0.75f, -7f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(HotPotato.hotPotatoPlayer);
                                    }

                                    foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-6.25f, -3.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdhotpotato) {
                                        GameObject hotpotato = GameObject.Instantiate(CustomMain.customAssets.hotPotato, HotPotato.hotPotatoPlayer.transform);
                                        hotpotato.name = "hotpotato";
                                        hotpotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                        HotPotato.hotPotato = hotpotato;
                                        createdhotpotato = true;
                                    }
                                }
                                // Zombie Laboratory
                                else if (ZombieLaboratory.zombieLaboratoryMode) {
                                    foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(17.25f, -13.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }

                                    foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                                        if (player == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != ZombieLaboratory.nursePlayer) {
                                            player.transform.position = new Vector3(-11.75f, -4.75f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                            // Add Arrows pointing the deliver point
                                            if (ZombieLaboratory.localSurvivorsDeliverArrow.Count == 0) {
                                                ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                                                ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(true);
                                            }
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer) {
                                        ZombieLaboratory.nursePlayer.transform.position = new Vector3(10.2f, 3.6f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(ZombieLaboratory.nursePlayer);
                                        GameObject mapMedKit = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKit.name = "mapMedKit";
                                        mapMedKit.transform.position = new Vector3(7.25f, -5f, -0.1f);
                                        GameObject mapMedKittwo = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKittwo.name = "mapMedKittwo";
                                        mapMedKittwo.transform.position = new Vector3(-3.75f, 3.5f, -0.1f);
                                        GameObject mapMedKitthree = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKitthree.name = "mapMedKitthree";
                                        mapMedKitthree.transform.position = new Vector3(13.75f, -3.75f, -0.1f);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKit);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKittwo);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKitthree);
                                        // Add Arrows pointing the medkit only for nurse
                                        if (ZombieLaboratory.localNurseArrows.Count == 0 && ZombieLaboratory.localNurseArrows.Count < 3) {
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                        }
                                        ZombieLaboratory.localNurseArrows[0].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[1].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[2].arrow.SetActive(true);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdzombielaboratory) {
                                        GameObject laboratory = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerControl.LocalPlayer.transform.parent);
                                        laboratory.name = "laboratory";
                                        laboratory.transform.position = new Vector3(10.25f, 3.38f, 0.5f);
                                        laboratory.gameObject.layer = 9;
                                        laboratory.transform.GetChild(0).gameObject.layer = 9;
                                        ZombieLaboratory.laboratory = laboratory;
                                        ZombieLaboratory.laboratoryEnterButton = laboratory.transform.GetChild(1).gameObject;
                                        ZombieLaboratory.laboratoryExitButton = laboratory.transform.GetChild(2).gameObject;
                                        ZombieLaboratory.laboratoryCreateCureButton = laboratory.transform.GetChild(3).gameObject;
                                        ZombieLaboratory.laboratoryPutKeyItemButton = laboratory.transform.GetChild(4).gameObject;
                                        ZombieLaboratory.laboratoryExitLeftButton = laboratory.transform.GetChild(5).gameObject;
                                        ZombieLaboratory.laboratoryExitRightButton = laboratory.transform.GetChild(6).gameObject;
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryEnterButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitLeftButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitRightButton);

                                        GameObject nurseMedKit = GameObject.Instantiate(CustomMain.customAssets.nurseMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        nurseMedKit.name = "nurseMedKit";
                                        nurseMedKit.transform.parent = ZombieLaboratory.nursePlayer.transform;
                                        nurseMedKit.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                                        ZombieLaboratory.laboratoryNurseMedKit = nurseMedKit;
                                        ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                                        ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratoryEnterButton);
                                        createdzombielaboratory = true;
                                    }
                                }
                                // Battle Royale
                                else if (BattleRoyale.battleRoyaleMode) {
                                    if (BattleRoyale.matchType == 0) {
                                        foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                                            soloPlayer.transform.position = new Vector3(BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].x, BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].y, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(soloPlayer);
                                            howmanyBattleRoyaleplayers += 1;
                                        }
                                    }
                                    else {

                                        if (PlayerControl.LocalPlayer == BattleRoyale.serialKiller) {
                                            BattleRoyale.serialKiller.transform.position = new Vector3(-6.35f, -7.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(BattleRoyale.serialKiller);
                                        }

                                        foreach (PlayerControl player in BattleRoyale.limeTeam) {
                                            player.transform.position = new Vector3(17f, -5.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                                            player.transform.position = new Vector3(-12f, -4.75f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdbattleroyale) {
                                        if (BattleRoyale.matchType != 0) {
                                            GameObject limeteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                            limeteamfloor.name = "limeteamfloor";
                                            limeteamfloor.transform.position = new Vector3(17f, -5.5f, 0.5f);
                                            GameObject pinkteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                            pinkteamfloor.name = "pinkteamfloor";
                                            pinkteamfloor.transform.position = new Vector3(-12f, -4.75f, 0.5f);
                                            BattleRoyale.serialKillerSpawns.Add(limeteamfloor);
                                            BattleRoyale.serialKillerSpawns.Add(pinkteamfloor);
                                        }
                                        createdbattleroyale = true;
                                    }
                                }
                                // Remove camera use and admin table on Dleks
                                GameObject dlekscameraStand = GameObject.Find("SurvConsole");
                                dlekscameraStand.GetComponent<PolygonCollider2D>().enabled = false;
                                GameObject dleksadmin = GameObject.Find("MapRoomConsole");
                                dleksadmin.GetComponent<CircleCollider2D>().enabled = false;
                                break;
                            // Airship
                            case 4:
                                // Capture The Flag
                                if (CaptureTheFlag.captureTheFlagMode) {
                                    if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(10.25f, -15.35f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(CaptureTheFlag.stealerPlayer);
                                    }

                                    foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-17.5f, -1f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(33.6f, 1.45f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdcapturetheflag) {
                                        GameObject redflag = GameObject.Instantiate(CustomMain.customAssets.redflag, PlayerControl.LocalPlayer.transform.parent);
                                        redflag.name = "redflag";
                                        redflag.transform.position = new Vector3(-17.5f, -1.2f, 0.5f);
                                        CaptureTheFlag.redflag = redflag;
                                        GameObject redflagbase = GameObject.Instantiate(CustomMain.customAssets.redflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        redflagbase.name = "redflagbase";
                                        redflagbase.transform.position = new Vector3(-17.5f, -1.25f, 1f);
                                        CaptureTheFlag.redflagbase = redflagbase;
                                        GameObject blueflag = GameObject.Instantiate(CustomMain.customAssets.blueflag, PlayerControl.LocalPlayer.transform.parent);
                                        blueflag.name = "blueflag";
                                        blueflag.transform.position = new Vector3(33.6f, 1.25f, 0.5f);
                                        CaptureTheFlag.blueflag = blueflag;
                                        GameObject blueflagbase = GameObject.Instantiate(CustomMain.customAssets.blueflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        blueflagbase.name = "blueflagbase";
                                        blueflagbase.transform.position = new Vector3(33.6f, 1.2f, 1f);
                                        CaptureTheFlag.blueflagbase = blueflagbase;
                                        CaptureTheFlag.stealerSpawns.Add(redflagbase);
                                        CaptureTheFlag.stealerSpawns.Add(blueflagbase);
                                        createdcapturetheflag = true;
                                    }
                                }
                                // Police And Thiefs
                                else if (PoliceAndThief.policeAndThiefMode) {
                                    foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-18.5f, 0.75f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                                        if (player == PlayerControl.LocalPlayer) {
                                            player.transform.position = new Vector3(7.15f, -14.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);

                                            // Add Arrows pointing the release and deliver point
                                            if (PoliceAndThief.localThiefReleaseArrow.Count == 0) {
                                                PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                                                PoliceAndThief.localThiefReleaseArrow[0].arrow.SetActive(true);
                                            }
                                            if (PoliceAndThief.localThiefDeliverArrow.Count == 0) {
                                                PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                                                PoliceAndThief.localThiefDeliverArrow[0].arrow.SetActive(true);
                                            }
                                        }
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdpoliceandthief) {
                                        GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                                        cell.name = "cell";
                                        cell.transform.position = new Vector3(-18.45f, 3.55f, 0.5f);
                                        cell.gameObject.layer = 9;
                                        cell.transform.GetChild(0).gameObject.layer = 9;
                                        PoliceAndThief.cell = cell;
                                        GameObject cellbutton = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerControl.LocalPlayer.transform.parent);
                                        cellbutton.name = "cellbutton";
                                        cellbutton.transform.position = new Vector3(-18.5f, 0.5f, 0.5f);
                                        PoliceAndThief.cellbutton = cellbutton;
                                        GameObject jewelbutton = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerControl.LocalPlayer.transform.parent);
                                        jewelbutton.name = "jewelbutton";
                                        jewelbutton.transform.position = new Vector3(10.275f, -16.3f, -0.01f);
                                        PoliceAndThief.jewelbutton = jewelbutton;
                                        GameObject thiefspaceship = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerControl.LocalPlayer.transform.parent);
                                        thiefspaceship.name = "thiefspaceship";
                                        thiefspaceship.transform.position = new Vector3(13.5f, -16f, 0.6f);
                                        createdpoliceandthief = true;

                                        // Spawn jewels
                                        GameObject jewel01 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel01.transform.position = new Vector3(-23.5f, -1.5f, 1f);
                                        jewel01.name = "jewel01";
                                        PoliceAndThief.jewel01 = jewel01;
                                        GameObject jewel02 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel02.transform.position = new Vector3(-14.15f, -4.85f, 1f);
                                        jewel02.name = "jewel02";
                                        PoliceAndThief.jewel02 = jewel02;
                                        GameObject jewel03 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel03.transform.position = new Vector3(-13.9f, -16.25f, 1f);
                                        jewel03.name = "jewel03";
                                        PoliceAndThief.jewel03 = jewel03;
                                        GameObject jewel04 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel04.transform.position = new Vector3(-0.85f, -2.5f, 1f);
                                        jewel04.name = "jewel04";
                                        PoliceAndThief.jewel04 = jewel04;
                                        GameObject jewel05 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel05.transform.position = new Vector3(-5, 8.5f, 1f);
                                        jewel05.name = "jewel05";
                                        PoliceAndThief.jewel05 = jewel05;
                                        GameObject jewel06 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel06.transform.position = new Vector3(19.3f, -4.15f, 1f);
                                        jewel06.name = "jewel06";
                                        PoliceAndThief.jewel06 = jewel06;
                                        GameObject jewel07 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel07.transform.position = new Vector3(19.85f, 8, 1f);
                                        jewel07.name = "jewel07";
                                        PoliceAndThief.jewel07 = jewel07;
                                        GameObject jewel08 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel08.transform.position = new Vector3(28.85f, -1.75f, 1f);
                                        jewel08.name = "jewel08";
                                        PoliceAndThief.jewel08 = jewel08;
                                        GameObject jewel09 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel09.transform.position = new Vector3(-14.5f, -8.5f, 1f);
                                        jewel09.name = "jewel09";
                                        PoliceAndThief.jewel09 = jewel09;
                                        GameObject jewel10 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel10.transform.position = new Vector3(6.3f, -2.75f, 1f);
                                        jewel10.name = "jewel10";
                                        PoliceAndThief.jewel10 = jewel10;
                                        GameObject jewel11 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel11.transform.position = new Vector3(20.75f, 2.5f, 1f);
                                        jewel11.name = "jewel11";
                                        PoliceAndThief.jewel11 = jewel11;
                                        GameObject jewel12 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel12.transform.position = new Vector3(29.25f, 7, 1f);
                                        jewel12.name = "jewel12";
                                        PoliceAndThief.jewel12 = jewel12;
                                        GameObject jewel13 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel13.transform.position = new Vector3(37.5f, -3.5f, 1f);
                                        jewel13.name = "jewel13";
                                        PoliceAndThief.jewel13 = jewel13;
                                        GameObject jewel14 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel14.transform.position = new Vector3(25.2f, -8.75f, 1f);
                                        jewel14.name = "jewel14";
                                        PoliceAndThief.jewel14 = jewel14;
                                        GameObject jewel15 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel15.transform.position = new Vector3(16.3f, -11, 1f);
                                        jewel15.name = "jewel15";
                                        PoliceAndThief.jewel15 = jewel15;
                                        PoliceAndThief.thiefTreasures.Add(jewel01);
                                        PoliceAndThief.thiefTreasures.Add(jewel02);
                                        PoliceAndThief.thiefTreasures.Add(jewel03);
                                        PoliceAndThief.thiefTreasures.Add(jewel04);
                                        PoliceAndThief.thiefTreasures.Add(jewel05);
                                        PoliceAndThief.thiefTreasures.Add(jewel06);
                                        PoliceAndThief.thiefTreasures.Add(jewel07);
                                        PoliceAndThief.thiefTreasures.Add(jewel08);
                                        PoliceAndThief.thiefTreasures.Add(jewel09);
                                        PoliceAndThief.thiefTreasures.Add(jewel10);
                                        PoliceAndThief.thiefTreasures.Add(jewel11);
                                        PoliceAndThief.thiefTreasures.Add(jewel12);
                                        PoliceAndThief.thiefTreasures.Add(jewel13);
                                        PoliceAndThief.thiefTreasures.Add(jewel14);
                                        PoliceAndThief.thiefTreasures.Add(jewel15);
                                    }
                                }
                                // King Of The Hill
                                else if (KingOfTheHill.kingOfTheHillMode) {
                                    if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(12.25f, 2f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(KingOfTheHill.usurperPlayer);
                                    }

                                    foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-13.9f, -14.45f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(37.35f, -3.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdkingofthehill) {
                                        GameObject greenteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                        greenteamfloor.name = "greenteamfloor";
                                        greenteamfloor.transform.position = new Vector3(-13.9f, -14.7f, 0.5f);
                                        GameObject yellowteamfloor = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerControl.LocalPlayer.transform.parent);
                                        yellowteamfloor.name = "yellowteamfloor";
                                        yellowteamfloor.transform.position = new Vector3(37.35f, -3.5f, 0.5f);
                                        GameObject greenkingaura = GameObject.Instantiate(CustomMain.customAssets.greenaura, KingOfTheHill.greenKingplayer.transform);
                                        greenkingaura.name = "greenkingaura";
                                        greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                                        KingOfTheHill.greenkingaura = greenkingaura;
                                        GameObject yellowkingaura = GameObject.Instantiate(CustomMain.customAssets.yellowaura, KingOfTheHill.yellowKingplayer.transform);
                                        yellowkingaura.name = "yellowkingaura";
                                        yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                                        KingOfTheHill.yellowkingaura = yellowkingaura;
                                        GameObject flagzoneone = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzoneone.name = "flagzoneone";
                                        flagzoneone.transform.position = new Vector3(-8.75f, 5.1f, 0.4f);
                                        KingOfTheHill.flagzoneone = flagzoneone;
                                        GameObject zoneone = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zoneone.name = "zoneone";
                                        zoneone.transform.position = new Vector3(-8.75f, 5.1f, 0.5f);
                                        KingOfTheHill.zoneone = zoneone;
                                        GameObject flagzonetwo = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonetwo.name = "flagzonetwo";
                                        flagzonetwo.transform.position = new Vector3(19.9f, 11.25f, 0.4f);
                                        KingOfTheHill.flagzonetwo = flagzonetwo;
                                        GameObject zonetwo = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonetwo.name = "zonetwo";
                                        zonetwo.transform.position = new Vector3(19.9f, 11.25f, 0.5f);
                                        KingOfTheHill.zonetwo = zonetwo;
                                        GameObject flagzonethree = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonethree.name = "flagzonethree";
                                        flagzonethree.transform.position = new Vector3(16.3f, -8.6f, 0.4f);
                                        KingOfTheHill.flagzonethree = flagzonethree;
                                        GameObject zonethree = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonethree.name = "zonethree";
                                        zonethree.transform.position = new Vector3(16.3f, -8.6f, 0.5f);
                                        KingOfTheHill.zonethree = zonethree;
                                        KingOfTheHill.kingZones.Add(zoneone);
                                        KingOfTheHill.kingZones.Add(zonetwo);
                                        KingOfTheHill.kingZones.Add(zonethree);
                                        KingOfTheHill.usurperSpawns.Add(greenteamfloor);
                                        KingOfTheHill.usurperSpawns.Add(yellowteamfloor);
                                        createdkingofthehill = true;
                                    }
                                }
                                // Hot Potato
                                else if (HotPotato.hotPotatoMode) {
                                    if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                                        HotPotato.hotPotatoPlayer.transform.position = new Vector3(12.25f, 2f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(HotPotato.hotPotatoPlayer);
                                    }

                                    foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(6.25f, 2.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdhotpotato) {
                                        GameObject hotpotato = GameObject.Instantiate(CustomMain.customAssets.hotPotato, HotPotato.hotPotatoPlayer.transform);
                                        hotpotato.name = "hotpotato";
                                        hotpotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                        HotPotato.hotPotato = hotpotato;
                                        createdhotpotato = true;
                                    }
                                }
                                // Zombie Laboratory
                                else if (ZombieLaboratory.zombieLaboratoryMode) {
                                    foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(32.35f, 7.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }

                                    foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                                        if (player == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != ZombieLaboratory.nursePlayer) {
                                            player.transform.position = new Vector3(25.25f, -8.65f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                            // Add Arrows pointing the deliver point
                                            if (ZombieLaboratory.localSurvivorsDeliverArrow.Count == 0) {
                                                ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                                                ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(true);
                                            }
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer) {
                                        ZombieLaboratory.nursePlayer.transform.position = new Vector3(-18.5f, 2.9f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(ZombieLaboratory.nursePlayer);
                                        ZombieLaboratory.nursePlayerInsideLaboratory = false;
                                        GameObject mapMedKit = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKit.name = "mapMedKit";
                                        mapMedKit.transform.position = new Vector3(-12f, 2.5f, -0.1f);
                                        GameObject mapMedKittwo = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKittwo.name = "mapMedKittwo";
                                        mapMedKittwo.transform.position = new Vector3(-13.5f, -9.75f, -0.1f);
                                        GameObject mapMedKitthree = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKitthree.name = "mapMedKitthree";
                                        mapMedKitthree.transform.position = new Vector3(-8.85f, 7.5f, -0.1f);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKit);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKittwo);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKitthree);
                                        // Add Arrows pointing the medkit only for nurse
                                        if (ZombieLaboratory.localNurseArrows.Count == 0 && ZombieLaboratory.localNurseArrows.Count < 3) {
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                        }
                                        ZombieLaboratory.localNurseArrows[0].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[1].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[2].arrow.SetActive(true);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdzombielaboratory) {
                                        GameObject laboratory = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerControl.LocalPlayer.transform.parent);
                                        laboratory.name = "laboratory";
                                        laboratory.transform.position = new Vector3(-18.45f, 3f, 0.5f);
                                        laboratory.gameObject.layer = 9;
                                        laboratory.transform.GetChild(0).gameObject.layer = 9;
                                        ZombieLaboratory.laboratory = laboratory;
                                        ZombieLaboratory.laboratoryEnterButton = laboratory.transform.GetChild(1).gameObject;
                                        ZombieLaboratory.laboratoryExitButton = laboratory.transform.GetChild(2).gameObject;
                                        ZombieLaboratory.laboratoryCreateCureButton = laboratory.transform.GetChild(3).gameObject;
                                        ZombieLaboratory.laboratoryPutKeyItemButton = laboratory.transform.GetChild(4).gameObject;
                                        ZombieLaboratory.laboratoryExitLeftButton = laboratory.transform.GetChild(5).gameObject;
                                        ZombieLaboratory.laboratoryExitRightButton = laboratory.transform.GetChild(6).gameObject;
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryEnterButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitLeftButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitRightButton);

                                        GameObject nurseMedKit = GameObject.Instantiate(CustomMain.customAssets.nurseMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        nurseMedKit.name = "nurseMedKit";
                                        nurseMedKit.transform.parent = ZombieLaboratory.nursePlayer.transform;
                                        nurseMedKit.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                                        ZombieLaboratory.laboratoryNurseMedKit = nurseMedKit;
                                        ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                                        ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratoryEnterButton);
                                        createdzombielaboratory = true;
                                    }
                                }
                                // Battle Royale
                                else if (BattleRoyale.battleRoyaleMode) {
                                    if (BattleRoyale.matchType == 0) {
                                        foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                                            soloPlayer.transform.position = new Vector3(-0.5f, -1, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(soloPlayer);
                                        }
                                    }
                                    else {

                                        if (PlayerControl.LocalPlayer == BattleRoyale.serialKiller) {
                                            BattleRoyale.serialKiller.transform.position = new Vector3(12.25f, 2f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(BattleRoyale.serialKiller);
                                        }

                                        foreach (PlayerControl player in BattleRoyale.limeTeam) {
                                            player.transform.position = new Vector3(-13.9f, -14.45f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                                            player.transform.position = new Vector3(37.35f, -3.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdbattleroyale) {
                                        if (BattleRoyale.matchType != 0) {
                                            GameObject limeteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                            limeteamfloor.name = "limeteamfloor";
                                            limeteamfloor.transform.position = new Vector3(-13.9f, -14.45f, 0.5f);
                                            GameObject pinkteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                            pinkteamfloor.name = "pinkteamfloor";
                                            pinkteamfloor.transform.position = new Vector3(37.35f, -3.25f, 0.5f);
                                            BattleRoyale.serialKillerSpawns.Add(limeteamfloor);
                                            BattleRoyale.serialKillerSpawns.Add(pinkteamfloor);
                                        }
                                        createdbattleroyale = true;
                                    }
                                }

                                // Remove camera use, admin table, vitals, electrical doors on Airship
                                GameObject cameras = GameObject.Find("task_cams");
                                cameras.GetComponent<BoxCollider2D>().enabled = false;
                                GameObject airshipadmin = GameObject.Find("panel_cockpit_map");
                                airshipadmin.GetComponent<BoxCollider2D>().enabled = false;
                                GameObject airshipvitals = GameObject.Find("panel_vitals");
                                airshipvitals.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject laddermeeting = GameObject.Find("ladder_meeting");
                                laddermeeting.SetActive(false);
                                GameObject platform = GameObject.Find("Platform");
                                platform.SetActive(false);
                                GameObject platformleft = GameObject.Find("PlatformLeft");
                                platformleft.SetActive(false);
                                GameObject platformright = GameObject.Find("PlatformRight");
                                platformright.SetActive(false);
                                GameObject recordsadmin = GameObject.Find("records_admin_map");
                                recordsadmin.GetComponent<BoxCollider2D>().enabled = false;

                                Helpers.GetStaticDoor("TopLeftVert").SetOpen(true);
                                Helpers.GetStaticDoor("TopLeftHort").SetOpen(true);
                                Helpers.GetStaticDoor("BottomHort").SetOpen(true);
                                Helpers.GetStaticDoor("TopCenterHort").SetOpen(true);
                                Helpers.GetStaticDoor("LeftVert").SetOpen(true);
                                Helpers.GetStaticDoor("RightVert").SetOpen(true);
                                Helpers.GetStaticDoor("TopRightVert").SetOpen(true);
                                Helpers.GetStaticDoor("TopRightHort").SetOpen(true);
                                Helpers.GetStaticDoor("BottomRightHort").SetOpen(true);
                                Helpers.GetStaticDoor("BottomRightVert").SetOpen(true);
                                Helpers.GetStaticDoor("LeftDoorTop").SetOpen(true);
                                Helpers.GetStaticDoor("LeftDoorBottom").SetOpen(true);

                                break;
                            // Submerged
                            case 5:
                                // Capture The Flag
                                if (CaptureTheFlag.captureTheFlagMode) {
                                    if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(1f, 10f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(CaptureTheFlag.stealerPlayer);
                                    }

                                    foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-8.35f, 28.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(12.5f, -31.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdcapturetheflag) {
                                        GameObject redflag = GameObject.Instantiate(CustomMain.customAssets.redflag, PlayerControl.LocalPlayer.transform.parent);
                                        redflag.name = "redflag";
                                        redflag.transform.position = new Vector3(-8.35f, 28.05f, 0.03f);
                                        CaptureTheFlag.redflag = redflag;
                                        GameObject redflagbase = GameObject.Instantiate(CustomMain.customAssets.redflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        redflagbase.name = "redflagbase";
                                        redflagbase.transform.position = new Vector3(-8.35f, 28, 0.031f);
                                        CaptureTheFlag.redflagbase = redflagbase;
                                        GameObject blueflag = GameObject.Instantiate(CustomMain.customAssets.blueflag, PlayerControl.LocalPlayer.transform.parent);
                                        blueflag.name = "blueflag";
                                        blueflag.transform.position = new Vector3(12.5f, -31.45f, -0.011f);
                                        CaptureTheFlag.blueflag = blueflag;
                                        GameObject blueflagbase = GameObject.Instantiate(CustomMain.customAssets.blueflagbase, PlayerControl.LocalPlayer.transform.parent);
                                        blueflagbase.name = "blueflagbase";
                                        blueflagbase.transform.position = new Vector3(12.5f, -31.5f, -0.01f);
                                        CaptureTheFlag.blueflagbase = blueflagbase;

                                        GameObject redteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                        redteamfloor.name = "redteamfloor";
                                        redteamfloor.transform.position = new Vector3(-14f, -27.5f, -0.01f);
                                        GameObject blueteamfloor = GameObject.Instantiate(CustomMain.customAssets.bluefloor, PlayerControl.LocalPlayer.transform.parent);
                                        blueteamfloor.name = "blueteamfloor";
                                        blueteamfloor.transform.position = new Vector3(14.25f, 24.25f, 0.03f);

                                        CaptureTheFlag.stealerSpawns.Add(redflagbase);
                                        CaptureTheFlag.stealerSpawns.Add(blueflagbase);
                                        createdcapturetheflag = true;
                                    }
                                }
                                // Police And Thiefs
                                else if (PoliceAndThief.policeAndThiefMode) {
                                    foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-8.45f, 27f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                                        if (player == PlayerControl.LocalPlayer) {
                                            player.transform.position = new Vector3(1f, 10f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);

                                            // Add Arrows pointing the release and deliver point
                                            if (PoliceAndThief.localThiefReleaseArrow.Count == 0) {
                                                PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                                                PoliceAndThief.localThiefReleaseArrow[0].arrow.SetActive(true);
                                                PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                                                PoliceAndThief.localThiefReleaseArrow[1].arrow.SetActive(true);
                                            }
                                            if (PoliceAndThief.localThiefDeliverArrow.Count == 0) {
                                                PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                                                PoliceAndThief.localThiefDeliverArrow[0].arrow.SetActive(true);
                                                PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                                                PoliceAndThief.localThiefDeliverArrow[1].arrow.SetActive(true);
                                            }
                                        }
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdpoliceandthief) {
                                        GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                                        cell.name = "cell";
                                        cell.transform.position = new Vector3(-5.9f, 31.85f, 0.5f);
                                        cell.gameObject.layer = 9;
                                        cell.transform.GetChild(0).gameObject.layer = 9;
                                        PoliceAndThief.cell = cell;
                                        GameObject cellbutton = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerControl.LocalPlayer.transform.parent);
                                        cellbutton.name = "cellbutton";
                                        cellbutton.transform.position = new Vector3(-6f, 28.5f, 0.03f);
                                        PoliceAndThief.cellbutton = cellbutton;
                                        GameObject jewelbutton = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerControl.LocalPlayer.transform.parent);
                                        jewelbutton.name = "jewelbutton";
                                        jewelbutton.transform.position = new Vector3(1f, 10f, 0.03f);
                                        PoliceAndThief.jewelbutton = jewelbutton;
                                        GameObject thiefspaceship = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerControl.LocalPlayer.transform.parent);
                                        thiefspaceship.name = "thiefspaceship";
                                        thiefspaceship.transform.position = new Vector3(-2.75f, 9f, 0.031f);
                                        thiefspaceship.transform.localScale = new Vector3(-1f, 1f, 1f);

                                        GameObject celltwo = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerControl.LocalPlayer.transform.parent);
                                        celltwo.name = "celltwo";
                                        celltwo.transform.position = new Vector3(-14.1f, -39f, -0.01f);
                                        celltwo.gameObject.layer = 9;
                                        celltwo.transform.GetChild(0).gameObject.layer = 9;
                                        PoliceAndThief.celltwo = celltwo;
                                        GameObject cellbuttontwo = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerControl.LocalPlayer.transform.parent);
                                        cellbuttontwo.name = "cellbuttontwo";
                                        cellbuttontwo.transform.position = new Vector3(-11f, -39.35f, -0.01f);
                                        PoliceAndThief.cellbuttontwo = cellbuttontwo;
                                        GameObject jewelbuttontwo = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerControl.LocalPlayer.transform.parent);
                                        jewelbuttontwo.name = "jewelbuttontwo";
                                        jewelbuttontwo.transform.position = new Vector3(13f, -32.5f, -0.01f);
                                        PoliceAndThief.jewelbuttontwo = jewelbuttontwo;
                                        GameObject thiefspaceshiptwo = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerControl.LocalPlayer.transform.parent);
                                        thiefspaceshiptwo.name = "thiefspaceshiptwo";
                                        thiefspaceshiptwo.transform.position = new Vector3(14.5f, -35f, -0.011f);

                                        createdpoliceandthief = true;

                                        // Spawn jewels
                                        GameObject jewel01 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel01.transform.position = new Vector3(-15f, 17.5f, -1f);
                                        jewel01.name = "jewel01";
                                        PoliceAndThief.jewel01 = jewel01;
                                        GameObject jewel02 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel02.transform.position = new Vector3(8f, 32f, -1f);
                                        jewel02.name = "jewel02";
                                        PoliceAndThief.jewel02 = jewel02;
                                        GameObject jewel03 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel03.transform.position = new Vector3(-6.75f, 10f, -1f);
                                        jewel03.name = "jewel03";
                                        PoliceAndThief.jewel03 = jewel03;
                                        GameObject jewel04 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel04.transform.position = new Vector3(5.15f, 8f, -1f);
                                        jewel04.name = "jewel04";
                                        PoliceAndThief.jewel04 = jewel04;
                                        GameObject jewel05 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel05.transform.position = new Vector3(5f, -33.5f, -1f);
                                        jewel05.name = "jewel05";
                                        PoliceAndThief.jewel05 = jewel05;
                                        GameObject jewel06 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel06.transform.position = new Vector3(-4.15f, -33.5f, -1f);
                                        jewel06.name = "jewel06";
                                        PoliceAndThief.jewel06 = jewel06;
                                        GameObject jewel07 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel07.transform.position = new Vector3(-14f, -27.75f, -1f);
                                        jewel07.name = "jewel07";
                                        PoliceAndThief.jewel07 = jewel07;
                                        GameObject jewel08 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerControl.LocalPlayer.transform.parent);
                                        jewel08.transform.position = new Vector3(7.8f, -23.75f, -1f);
                                        jewel08.name = "jewel08";
                                        PoliceAndThief.jewel08 = jewel08;
                                        GameObject jewel09 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel09.transform.position = new Vector3(-6.75f, -42.75f, -1f);
                                        jewel09.name = "jewel09";
                                        PoliceAndThief.jewel09 = jewel09;
                                        GameObject jewel10 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel10.transform.position = new Vector3(13f, -25.25f, -1f);
                                        jewel10.name = "jewel10";
                                        PoliceAndThief.jewel10 = jewel10;
                                        GameObject jewel11 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel11.transform.position = new Vector3(-14f, -34.25f, -1f);
                                        jewel11.name = "jewel11";
                                        PoliceAndThief.jewel11 = jewel11;
                                        GameObject jewel12 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel12.transform.position = new Vector3(0f, -33.5f, -1f);
                                        jewel12.name = "jewel12";
                                        PoliceAndThief.jewel12 = jewel12;
                                        GameObject jewel13 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel13.transform.position = new Vector3(-6.5f, 14f, -1f);
                                        jewel13.name = "jewel13";
                                        PoliceAndThief.jewel13 = jewel13;
                                        GameObject jewel14 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel14.transform.position = new Vector3(14.25f, 24.5f, -1f);
                                        jewel14.name = "jewel14";
                                        PoliceAndThief.jewel14 = jewel14;
                                        GameObject jewel15 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerControl.LocalPlayer.transform.parent);
                                        jewel15.transform.position = new Vector3(-12.25f, 31f, -1f);
                                        jewel15.name = "jewel15";
                                        PoliceAndThief.jewel15 = jewel15;
                                        PoliceAndThief.thiefTreasures.Add(jewel01);
                                        PoliceAndThief.thiefTreasures.Add(jewel02);
                                        PoliceAndThief.thiefTreasures.Add(jewel03);
                                        PoliceAndThief.thiefTreasures.Add(jewel04);
                                        PoliceAndThief.thiefTreasures.Add(jewel05);
                                        PoliceAndThief.thiefTreasures.Add(jewel06);
                                        PoliceAndThief.thiefTreasures.Add(jewel07);
                                        PoliceAndThief.thiefTreasures.Add(jewel08);
                                        PoliceAndThief.thiefTreasures.Add(jewel09);
                                        PoliceAndThief.thiefTreasures.Add(jewel10);
                                        PoliceAndThief.thiefTreasures.Add(jewel11);
                                        PoliceAndThief.thiefTreasures.Add(jewel12);
                                        PoliceAndThief.thiefTreasures.Add(jewel13);
                                        PoliceAndThief.thiefTreasures.Add(jewel14);
                                        PoliceAndThief.thiefTreasures.Add(jewel15);
                                    }
                                }
                                // King Of The Hill
                                else if (KingOfTheHill.kingOfTheHillMode) {
                                    if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(5.75f, 31.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(KingOfTheHill.usurperPlayer);
                                    }

                                    foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-12.25f, 18.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(-8.5f, -39.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }
                                    if (PlayerControl.LocalPlayer != null && !createdkingofthehill) {
                                        GameObject greenteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                        greenteamfloor.name = "greenteamfloor";
                                        greenteamfloor.transform.position = new Vector3(-12.25f, 18.25f, 0.03f);
                                        GameObject greenteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                        greenteamfloortwo.name = "greenteamfloortwo";
                                        greenteamfloortwo.transform.position = new Vector3(-14.5f, -34.35f, -0.01f);
                                        GameObject yellowteamfloor = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerControl.LocalPlayer.transform.parent);
                                        yellowteamfloor.name = "yellowteamfloor";
                                        yellowteamfloor.transform.position = new Vector3(-8.5f, -39.5f, -0.01f);
                                        GameObject yellowteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerControl.LocalPlayer.transform.parent);
                                        yellowteamfloortwo.name = "yellowteamfloortwo";
                                        yellowteamfloortwo.transform.position = new Vector3(0f, 33.5f, 0.03f);
                                        GameObject greenkingaura = GameObject.Instantiate(CustomMain.customAssets.greenaura, KingOfTheHill.greenKingplayer.transform);
                                        greenkingaura.name = "greenkingaura";
                                        greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, -0.5f);
                                        KingOfTheHill.greenkingaura = greenkingaura;
                                        GameObject yellowkingaura = GameObject.Instantiate(CustomMain.customAssets.yellowaura, KingOfTheHill.yellowKingplayer.transform);
                                        yellowkingaura.name = "yellowkingaura";
                                        yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, -0.5f);
                                        KingOfTheHill.yellowkingaura = yellowkingaura;
                                        GameObject flagzoneone = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzoneone.name = "flagzoneone";
                                        flagzoneone.transform.position = new Vector3(1f, 10f, 0.029f);
                                        KingOfTheHill.flagzoneone = flagzoneone;
                                        GameObject zoneone = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zoneone.name = "zoneone";
                                        zoneone.transform.position = new Vector3(1f, 10f, 0.03f);
                                        KingOfTheHill.zoneone = zoneone;
                                        GameObject flagzonetwo = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonetwo.name = "flagzonetwo";
                                        flagzonetwo.transform.position = new Vector3(2.5f, -35.5f, -0.01f);
                                        KingOfTheHill.flagzonetwo = flagzonetwo;
                                        GameObject zonetwo = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonetwo.name = "zonetwo";
                                        zonetwo.transform.position = new Vector3(2.5f, -35.5f, -0.011f);
                                        KingOfTheHill.zonetwo = zonetwo;
                                        GameObject flagzonethree = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerControl.LocalPlayer.transform.parent);
                                        flagzonethree.name = "flagzonethree";
                                        flagzonethree.transform.position = new Vector3(10f, -31.5f, -0.01f);
                                        KingOfTheHill.flagzonethree = flagzonethree;
                                        GameObject zonethree = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerControl.LocalPlayer.transform.parent);
                                        zonethree.name = "zonethree";
                                        zonethree.transform.position = new Vector3(10f, -31.5f, -0.011f);
                                        KingOfTheHill.zonethree = zonethree;
                                        KingOfTheHill.kingZones.Add(zoneone);
                                        KingOfTheHill.kingZones.Add(zonetwo);
                                        KingOfTheHill.kingZones.Add(zonethree);
                                        KingOfTheHill.usurperSpawns.Add(greenteamfloor);
                                        KingOfTheHill.usurperSpawns.Add(yellowteamfloor);
                                        KingOfTheHill.usurperSpawns.Add(greenteamfloortwo);
                                        KingOfTheHill.usurperSpawns.Add(yellowteamfloortwo);
                                        createdkingofthehill = true;
                                    }
                                }
                                // Hot Potato
                                else if (HotPotato.hotPotatoMode) {
                                    if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                                        HotPotato.hotPotatoPlayer.transform.position = new Vector3(-4.25f, -33.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(HotPotato.hotPotatoPlayer);
                                    }

                                    foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(13f, -25.25f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdhotpotato) {
                                        GameObject hotpotato = GameObject.Instantiate(CustomMain.customAssets.hotPotato, HotPotato.hotPotatoPlayer.transform);
                                        hotpotato.name = "hotpotato";
                                        hotpotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                        HotPotato.hotPotato = hotpotato;
                                        createdhotpotato = true;
                                    }
                                }
                                // Zombie Laboratory
                                else if (ZombieLaboratory.zombieLaboratoryMode) {
                                    foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                                        if (player == PlayerControl.LocalPlayer)
                                            player.transform.position = new Vector3(1f, 10f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(player);
                                    }

                                    foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                                        if (player == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != ZombieLaboratory.nursePlayer) {
                                            player.transform.position = new Vector3(5.5f, 31.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                            // Add Arrows pointing the deliver point
                                            if (ZombieLaboratory.localSurvivorsDeliverArrow.Count == 0) {
                                                ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                                                ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(true);
                                                ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                                                ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(true);
                                            }
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer) {
                                        ZombieLaboratory.nursePlayer.transform.position = new Vector3(-6f, 31.5f, PlayerControl.LocalPlayer.transform.position.z);
                                        Helpers.clearAllTasks(ZombieLaboratory.nursePlayer);
                                        ZombieLaboratory.nursePlayerInsideLaboratory = false;
                                        GameObject mapMedKit = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKit.name = "mapMedKit";
                                        mapMedKit.transform.position = new Vector3(0f, 32f, -1f);
                                        GameObject mapMedKittwo = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKittwo.name = "mapMedKittwo";
                                        mapMedKittwo.transform.position = new Vector3(6f, -34f, -1f);
                                        GameObject mapMedKitthree = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        mapMedKitthree.name = "mapMedKitthree";
                                        mapMedKitthree.transform.position = new Vector3(-11.25f, -27.75f, -1f);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKit);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKittwo);
                                        ZombieLaboratory.nurseMedkits.Add(mapMedKitthree);
                                        // Add Arrows pointing the medkit only for nurse
                                        if (ZombieLaboratory.localNurseArrows.Count == 0 && ZombieLaboratory.localNurseArrows.Count < 3) {
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                            ZombieLaboratory.localNurseArrows.Add(new Arrow(Shy.color));
                                        }
                                        ZombieLaboratory.localNurseArrows[0].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[1].arrow.SetActive(true);
                                        ZombieLaboratory.localNurseArrows[2].arrow.SetActive(true);
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdzombielaboratory) {
                                        GameObject laboratory = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerControl.LocalPlayer.transform.parent);
                                        laboratory.name = "laboratory";
                                        laboratory.transform.position = new Vector3(-5.9f, 31.85f, 0.5f);
                                        laboratory.gameObject.layer = 9;
                                        laboratory.transform.GetChild(0).gameObject.layer = 9;
                                        ZombieLaboratory.laboratory = laboratory;
                                        ZombieLaboratory.laboratoryEnterButton = laboratory.transform.GetChild(1).gameObject;
                                        ZombieLaboratory.laboratoryEnterButton.transform.position = new Vector3(-5.45f, 29.4f, -1.5f);
                                        ZombieLaboratory.laboratoryExitButton = laboratory.transform.GetChild(2).gameObject;
                                        ZombieLaboratory.laboratoryCreateCureButton = laboratory.transform.GetChild(3).gameObject;
                                        ZombieLaboratory.laboratoryPutKeyItemButton = laboratory.transform.GetChild(4).gameObject;
                                        ZombieLaboratory.laboratoryExitLeftButton = laboratory.transform.GetChild(5).gameObject;
                                        ZombieLaboratory.laboratoryExitRightButton = laboratory.transform.GetChild(6).gameObject;
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryEnterButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitLeftButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitRightButton);

                                        GameObject laboratorytwo = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerControl.LocalPlayer.transform.parent);
                                        laboratorytwo.name = "laboratorytwo";
                                        laboratorytwo.transform.position = new Vector3(-14.1f, -39f, -0.01f);
                                        laboratorytwo.gameObject.layer = 9;
                                        laboratorytwo.transform.GetChild(0).gameObject.layer = 9;
                                        ZombieLaboratory.laboratorytwo = laboratorytwo;
                                        ZombieLaboratory.laboratorytwoEnterButton = laboratorytwo.transform.GetChild(1).gameObject;
                                        ZombieLaboratory.laboratorytwoEnterButton.transform.position = new Vector3(-10.08f, -39.5f, -0.11f);
                                        ZombieLaboratory.laboratorytwoExitButton = laboratorytwo.transform.GetChild(2).gameObject;
                                        ZombieLaboratory.laboratorytwoCreateCureButton = laboratorytwo.transform.GetChild(3).gameObject;
                                        ZombieLaboratory.laboratorytwoPutKeyItemButton = laboratorytwo.transform.GetChild(4).gameObject;
                                        ZombieLaboratory.laboratorytwoExitLeftButton = laboratorytwo.transform.GetChild(5).gameObject;
                                        ZombieLaboratory.laboratorytwoExitRightButton = laboratorytwo.transform.GetChild(6).gameObject;
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratorytwoEnterButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratorytwoExitButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratorytwoExitLeftButton);
                                        ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratorytwoExitRightButton);

                                        GameObject nurseMedKit = GameObject.Instantiate(CustomMain.customAssets.nurseMedKit, PlayerControl.LocalPlayer.transform.parent);
                                        nurseMedKit.name = "nurseMedKit";
                                        nurseMedKit.transform.parent = ZombieLaboratory.nursePlayer.transform;
                                        nurseMedKit.transform.localPosition = new Vector3(0f, 0.7f, -0.5f);
                                        ZombieLaboratory.laboratoryNurseMedKit = nurseMedKit;
                                        ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                                        ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratoryEnterButton);
                                        ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratorytwoEnterButton);
                                        createdzombielaboratory = true;
                                    }
                                }
                                // Battle Royale
                                else if (BattleRoyale.battleRoyaleMode) {
                                    if (BattleRoyale.matchType == 0) {
                                        foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                                            soloPlayer.transform.position = new Vector3(3.75f, -26.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(soloPlayer);
                                        }
                                    }
                                    else {

                                        if (PlayerControl.LocalPlayer == BattleRoyale.serialKiller) {
                                            BattleRoyale.serialKiller.transform.position = new Vector3(5.75f, 31.25f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(BattleRoyale.serialKiller);
                                        }

                                        foreach (PlayerControl player in BattleRoyale.limeTeam) {
                                            player.transform.position = new Vector3(-12.25f, 18.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                        foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                                            player.transform.position = new Vector3(-8.5f, -39.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            Helpers.clearAllTasks(player);
                                        }
                                    }

                                    if (PlayerControl.LocalPlayer != null && !createdbattleroyale) {
                                        if (BattleRoyale.matchType != 0) {
                                            GameObject limeteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                            limeteamfloor.name = "limeteamfloor";
                                            limeteamfloor.transform.position = new Vector3(-12.25f, 18.5f, 0.03f);
                                            GameObject limeteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerControl.LocalPlayer.transform.parent);
                                            limeteamfloortwo.name = "limeteamfloortwo";
                                            limeteamfloortwo.transform.position = new Vector3(-14.5f, -34.35f, -0.01f);
                                            GameObject pinkteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                            pinkteamfloor.name = "pinkteamfloor";
                                            pinkteamfloor.transform.position = new Vector3(-8.5f, -39.5f, -0.01f);
                                            GameObject pinkteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerControl.LocalPlayer.transform.parent);
                                            pinkteamfloortwo.name = "pinkteamfloortwo";
                                            pinkteamfloortwo.transform.position = new Vector3(0f, 33.5f, 0.03f);
                                            BattleRoyale.serialKillerSpawns.Add(limeteamfloor);
                                            BattleRoyale.serialKillerSpawns.Add(pinkteamfloor);
                                            BattleRoyale.serialKillerSpawns.Add(limeteamfloortwo);
                                            BattleRoyale.serialKillerSpawns.Add(pinkteamfloortwo);
                                        }
                                        createdbattleroyale = true;
                                    }
                                }
                                // Remove camera use, admin table, vitals, on Submerged
                                GameObject upperCentralVent = GameObject.Find("UpperCentralVent");
                                upperCentralVent.GetComponent<CircleCollider2D>().enabled = false;
                                upperCentralVent.GetComponent<PolygonCollider2D>().enabled = false;
                                GameObject lowerCentralVent = GameObject.Find("LowerCentralVent");
                                lowerCentralVent.GetComponent<BoxCollider2D>().enabled = false;
                                GameObject securityCams = GameObject.Find("SecurityConsole");
                                securityCams.GetComponent<PolygonCollider2D>().enabled = false;
                                GameObject submergedvitals = GameObject.Find("panel_vitals(Clone)");
                                submergedvitals.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject submergedadminone = GameObject.Find("console-adm-admintable");
                                submergedadminone.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject submergedadmintwo = GameObject.Find("console-adm-admintable (1)");
                                submergedadmintwo.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject deconVLower = GameObject.Find("DeconDoorVLower");
                                deconVLower.SetActive(false);
                                GameObject deconVUpper = GameObject.Find("DeconDoorVUpper");
                                deconVUpper.SetActive(false);
                                GameObject deconHLower = GameObject.Find("DeconDoorHLower");
                                deconHLower.SetActive(false);
                                GameObject deconHUpper = GameObject.Find("DeconDoorHUpper");
                                deconHUpper.SetActive(false);
                                GameObject camsone = GameObject.Find("Submerged(Clone)/Cameras/LowerDeck/Electrical/FixConsole");
                                camsone.GetComponent<PolygonCollider2D>().enabled = false;
                                GameObject camstwo = GameObject.Find("Submerged(Clone)/Cameras/LowerDeck/Lobby/FixConsole");
                                camstwo.GetComponent<BoxCollider2D>().enabled = false;
                                camstwo.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject camsthree = GameObject.Find("Submerged(Clone)/Cameras/UpperDeck/Comms/FixConsole");
                                camsthree.GetComponent<PolygonCollider2D>().enabled = false;
                                GameObject camsfour = GameObject.Find("Submerged(Clone)/Cameras/UpperDeck/Lobby/FixConsole");
                                camsfour.GetComponent<BoxCollider2D>().enabled = false;
                                camsfour.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject camsfive = GameObject.Find("Submerged(Clone)/Cameras/UpperDeck/WestHallway/FixConsole");
                                camsfive.GetComponent<BoxCollider2D>().enabled = false;
                                camsfive.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject camssix = GameObject.Find("Submerged(Clone)/Cameras/UpperDeck/YHallway/FixConsole");
                                camssix.GetComponent<BoxCollider2D>().enabled = false;
                                camssix.GetComponent<CircleCollider2D>().enabled = false;
                                GameObject camsseven = GameObject.Find("Submerged(Clone)/Cameras/LowerDeck/WestHallway/FixConsole");
                                camsseven.GetComponent<BoxCollider2D>().enabled = false;
                                break;
                        }

                        switch (whichgamemodeHUD) {
                            // Capture The Flag
                            case 1:
                                new CustomMessage(Language.introTexts[1], CaptureTheFlag.matchDuration, -1, -1.3f, 3);
                                new CustomMessage(CaptureTheFlag.flagpointCounter, CaptureTheFlag.matchDuration, -1, 1.9f, 5);
                                // Add Arrows pointing the flags
                                if (CaptureTheFlag.localRedFlagArrow.Count == 0) CaptureTheFlag.localRedFlagArrow.Add(new Arrow(Color.red));
                                CaptureTheFlag.localRedFlagArrow[0].arrow.SetActive(true);
                                if (CaptureTheFlag.localBlueFlagArrow.Count == 0) CaptureTheFlag.localBlueFlagArrow.Add(new Arrow(Color.blue));
                                CaptureTheFlag.localBlueFlagArrow[0].arrow.SetActive(true);
                                break;
                            // Police And Thiefs
                            case 2:
                                if (!PoliceAndThief.policeCanSeeJewels) {
                                    foreach (PlayerControl police in PoliceAndThief.policeTeam) {
                                        if (police == PlayerControl.LocalPlayer) {
                                            foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                                                jewel.SetActive(false);
                                            }
                                        }
                                    }
                                }
                                new CustomMessage(Language.introTexts[1], PoliceAndThief.matchDuration, -1, -1.3f, 6);
                                PoliceAndThief.thiefpointCounter = Language.introTexts[3] + "<color=#00F7FFFF>" + PoliceAndThief.currentJewelsStoled + " / " + PoliceAndThief.requiredJewels + "</color> | " + Language.introTexts[4] + "<color=#928B55FF>" + PoliceAndThief.currentThiefsCaptured + " / " + PoliceAndThief.thiefTeam.Count + "</color>";
                                new CustomMessage(PoliceAndThief.thiefpointCounter, PoliceAndThief.matchDuration, -1, 1.9f, 8);
                                break;
                            // King Of The Hill
                            case 3:
                                new CustomMessage(Language.introTexts[1], KingOfTheHill.matchDuration, -1, -1.3f, 10);
                                new CustomMessage(KingOfTheHill.kingpointCounter, KingOfTheHill.matchDuration, -1, 1.9f, 12);
                                // Add Arrows pointing the zones
                                if (KingOfTheHill.localArrows.Count == 0 && KingOfTheHill.localArrows.Count < 4) {
                                    KingOfTheHill.localArrows.Add(new Arrow(KingOfTheHill.zoneonecolor));
                                    KingOfTheHill.localArrows.Add(new Arrow(KingOfTheHill.zonetwocolor));
                                    KingOfTheHill.localArrows.Add(new Arrow(KingOfTheHill.zonethreecolor));
                                    KingOfTheHill.localArrows.Add(new Arrow(Color.cyan));
                                    KingOfTheHill.localArrows.Add(new Arrow(Color.cyan));
                                    KingOfTheHill.localArrows.Add(new Arrow(Color.cyan));
                                }
                                KingOfTheHill.localArrows[0].arrow.SetActive(true);
                                KingOfTheHill.localArrows[1].arrow.SetActive(true);
                                KingOfTheHill.localArrows[2].arrow.SetActive(true);
                                if (PlayerControl.LocalPlayer == KingOfTheHill.greenKingplayer || PlayerControl.LocalPlayer == KingOfTheHill.yellowKingplayer) {
                                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                                }
                                else {
                                    KingOfTheHill.localArrows[3].arrow.SetActive(true);
                                }
                                if (KingOfTheHill.usurperPlayer != null && PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[4].arrow.SetActive(true);
                                    KingOfTheHill.localArrows[5].arrow.SetActive(true);
                                }
                                else {
                                    KingOfTheHill.localArrows[4].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[5].arrow.SetActive(false);
                                }
                                break;
                            // Hot Potato
                            case 4:
                                break;
                            // Zombie Laboratory
                            case 5:
                                // Spawn key items
                                GameObject keyitem01 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                keyitem01.transform.position = ZombieLaboratory.susBoxPositions[0];
                                keyitem01.name = "keyItem01";
                                ZombieLaboratory.laboratoryKeyItem01 = keyitem01;
                                GameObject keyitem02 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                keyitem02.transform.position = ZombieLaboratory.susBoxPositions[1];
                                keyitem02.name = "keyItem02";
                                ZombieLaboratory.laboratoryKeyItem02 = keyitem02;
                                GameObject keyitem03 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                keyitem03.transform.position = ZombieLaboratory.susBoxPositions[2];
                                keyitem03.name = "keyItem03";
                                ZombieLaboratory.laboratoryKeyItem03 = keyitem03;
                                GameObject keyitem04 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                keyitem04.transform.position = ZombieLaboratory.susBoxPositions[3];
                                keyitem04.name = "keyItem04";
                                ZombieLaboratory.laboratoryKeyItem04 = keyitem04;
                                GameObject keyitem05 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                keyitem05.transform.position = ZombieLaboratory.susBoxPositions[4];
                                keyitem05.name = "keyItem05";
                                ZombieLaboratory.laboratoryKeyItem05 = keyitem05;
                                GameObject keyitem06 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                keyitem06.transform.position = ZombieLaboratory.susBoxPositions[5];
                                keyitem06.name = "keyItem06";
                                ZombieLaboratory.laboratoryKeyItem06 = keyitem06;
                                // Ammoboxes
                                GameObject ammoBox01 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                ammoBox01.transform.position = ZombieLaboratory.susBoxPositions[6];
                                ammoBox01.name = "ammoBox";
                                GameObject ammoBox02 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                ammoBox02.transform.position = ZombieLaboratory.susBoxPositions[7];
                                ammoBox02.name = "ammoBox";
                                GameObject ammoBox03 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                ammoBox03.transform.position = ZombieLaboratory.susBoxPositions[8];
                                ammoBox03.name = "ammoBox";
                                GameObject ammoBox04 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                ammoBox04.transform.position = ZombieLaboratory.susBoxPositions[9];
                                ammoBox04.name = "ammoBox";
                                ZombieLaboratory.groundItems.Add(keyitem01);
                                ZombieLaboratory.groundItems.Add(keyitem02);
                                ZombieLaboratory.groundItems.Add(keyitem03);
                                ZombieLaboratory.groundItems.Add(keyitem04);
                                ZombieLaboratory.groundItems.Add(keyitem05);
                                ZombieLaboratory.groundItems.Add(keyitem06);
                                ZombieLaboratory.groundItems.Add(ammoBox01);
                                ZombieLaboratory.groundItems.Add(ammoBox02);
                                ZombieLaboratory.groundItems.Add(ammoBox03);
                                ZombieLaboratory.groundItems.Add(ammoBox04);
                                // Nothing boxes
                                for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 10; i++) {
                                    GameObject nothingBox = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerControl.LocalPlayer.transform.parent);
                                    nothingBox.transform.position = ZombieLaboratory.susBoxPositions[i + 10];
                                    nothingBox.name = "nothingBox";
                                    ZombieLaboratory.groundItems.Add(nothingBox);
                                }
                                new CustomMessage(Language.introTexts[1], ZombieLaboratory.matchDuration, -1, -1.3f, 19);
                                ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                                new CustomMessage(ZombieLaboratory.zombieLaboratoryCounter, ZombieLaboratory.matchDuration, -1, 1.9f, 20);
                                break;
                            // Battle Royale
                            case 6:
                                new CustomMessage(Language.introTexts[1], BattleRoyale.matchDuration, -1, -1.3f, 24);
                                switch (BattleRoyale.matchType) {
                                    case 0:
                                        BattleRoyale.battleRoyalepointCounter = Language.introTexts[11] + "<color=#009F57FF>" + BattleRoyale.soloPlayerTeam.Count + "</color>";
                                        break;
                                    case 1:
                                        if (BattleRoyale.serialKiller != null) {
                                            BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + BattleRoyale.limeTeam.Count + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + BattleRoyale.pinkTeam.Count + "</color> | " + Language.introTexts[14] + "<color=#808080FF>" + BattleRoyale.serialKillerTeam.Count + "</color>";
                                        }
                                        else {
                                            BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + BattleRoyale.limeTeam.Count + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + BattleRoyale.pinkTeam.Count + "</color>";
                                        }
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null) {
                                            BattleRoyale.battleRoyalepointCounter = Language.introTexts[15] + BattleRoyale.requiredScore + " | <color=#39FF14FF>" + Language.introTexts[12] + BattleRoyale.limePoints + "</color> | " + "<color=#F2BEFFFF>" + Language.introTexts[13] + BattleRoyale.pinkPoints + " </color> | " + "<color=#808080FF>" + Language.introTexts[16] + BattleRoyale.serialKillerPoints + " </color>";
                                        }
                                        else {
                                            BattleRoyale.battleRoyalepointCounter = Language.introTexts[15] + BattleRoyale.requiredScore + " | <color=#39FF14FF>" + Language.introTexts[12] + BattleRoyale.limePoints + "</color> | " + "<color=#F2BEFFFF>" + Language.introTexts[13] + BattleRoyale.pinkPoints + " </color>";
                                        }
                                        break;
                                }
                                new CustomMessage(BattleRoyale.battleRoyalepointCounter, BattleRoyale.matchDuration, -1, 1.9f, 25);
                                break;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch
        {
            public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                setupIntroTeamIcons(__instance, ref teamToDisplay);
            }

            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                setupIntroTeam(__instance, ref teamToDisplay);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch
        {
            public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                setupIntroTeamIcons(__instance, ref yourTeam);
            }

            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                setupIntroTeam(__instance, ref yourTeam);
            }
        }

        public static void clearSwipeCardTask() {

            bool removeSwipeCard = CustomOptionHolder.removeSwipeCard.getBool();

            if (removeSwipeCard && removedSwipe == false && GameOptionsManager.Instance.currentGameOptions.MapId != 1 && GameOptionsManager.Instance.currentGameOptions.MapId != 4) {
                foreach (PlayerControl myplayer in PlayerControl.AllPlayerControls) {
                    if (myplayer != Joker.joker && myplayer != RoleThief.rolethief && myplayer != Pyromaniac.pyromaniac && myplayer != TreasureHunter.treasureHunter && myplayer != Devourer.devourer && myplayer != Poisoner.poisoner && myplayer != Puppeteer.puppeteer && myplayer != Exiler.exiler && myplayer != Amnesiac.amnesiac && myplayer != Seeker.seeker && myplayer != Renegade.renegade && myplayer != Minion.minion && myplayer != BountyHunter.bountyhunter && myplayer != Trapper.trapper && myplayer != Yinyanger.yinyanger && myplayer != Challenger.challenger && myplayer != Ninja.ninja && myplayer != Berserker.berserker && myplayer != Yandere.yandere && myplayer != Stranded.stranded && myplayer != Monja.monja && !myplayer.Data.Role.IsImpostor) {
                        var toRemove = new List<PlayerTask>();
                        foreach (PlayerTask task in myplayer.myTasks)
                            if (task.TaskType == TaskTypes.SwipeCard)
                                toRemove.Add(task);
                        foreach (PlayerTask task in toRemove)
                            if (task.TaskType == TaskTypes.SwipeCard)
                                myplayer.RemoveTask(task);
                    }
                }
                removedSwipe = true;
            }
        }

        public static void removeAirshipDoors() {

            bool removeAirshipDoors = CustomOptionHolder.removeAirshipDoors.getBool();

            if (removeAirshipDoors && removedAirshipDoors == false && GameOptionsManager.Instance.currentGameOptions.MapId == 4) {
                List<GameObject> doors = new List<GameObject>();
                
                GameObject celldoor01 = GameObject.Find("doorsideOpen (2)");
                doors.Add(celldoor01);
                GameObject celldoor02 = GameObject.Find("door_vault");
                doors.Add(celldoor02);
                GameObject celldoor04 = GameObject.Find("door_gap");
                doors.Add(celldoor04);

                GameObject bighallwaydoor01 = GameObject.Find("Door_VertOpen");
                doors.Add(bighallwaydoor01);
                GameObject bighallwaydoor02 = GameObject.Find("Door_VertOpen (4)");
                doors.Add(bighallwaydoor02);
                GameObject bighallwaydoor03 = GameObject.Find("Door_HortOpen");
                doors.Add(bighallwaydoor03);
                GameObject bighallwaydoor04 = GameObject.Find("Door_HortOpen (1)");
                doors.Add(bighallwaydoor04);

                GameObject kitchendoor01 = GameObject.Find("Door_VertOpen (1)");
                doors.Add(kitchendoor01);
                GameObject kitchendoor02 = GameObject.Find("Door_VertOpen (2)");
                doors.Add(kitchendoor02);
                GameObject kitchendoor03 = GameObject.Find("Door_VertOpen (3)");
                doors.Add(kitchendoor03);

                GameObject medbeydoor01 = GameObject.Find("Door_VertOpen (10)");
                doors.Add(medbeydoor01);
                GameObject medbeydoor02 = GameObject.Find("Door_HortOpen (3)");
                doors.Add(medbeydoor02);

                GameObject recorddoor01 = GameObject.Find("Door_VertOpen (11)");
                doors.Add(recorddoor01);
                GameObject recorddoor02 = GameObject.Find("Door_VertOpen (12)");
                doors.Add(recorddoor02);
                GameObject recorddoor03 = GameObject.Find("Door_HortOpen (2)");
                doors.Add(recorddoor03);

                GameObject hallway01 = GameObject.Find("Door_VertOpen (5)");
                doors.Add(hallway01);
                GameObject hallway02 = GameObject.Find("Door_VertOpen (6)");
                doors.Add(hallway02);

                foreach (GameObject door in doors) {
                    door.GetComponent<BoxCollider2D>().enabled = false;
                    door.GetComponent<SpriteRenderer>().enabled = false;
                }

                removedAirshipDoors = true;
            }
        }      
    }
}