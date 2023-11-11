using HarmonyLib;
using System;
using UnityEngine;
using static LasMonjas.LasMonjas;
using LasMonjas.Objects;
using System.Collections.Generic;
using System.Linq;
using Hazel;
using static LasMonjas.RoleInfo;
using static LasMonjas.MapOptions;
using LasMonjas.Core;
using static LasMonjas.HudManagerStartPatch;
using AmongUs.GameOptions;
using System.Collections;
using TMPro;
using static UnityEngine.GraphicsBuffer;

namespace LasMonjas.Patches {
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    class HudManagerUpdatePatch
    {
        static void resetNameTagsAndColors() {
            Dictionary<byte, PlayerControl> playersById = Helpers.allPlayersById();

            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                String playerName = player.Data.PlayerName;
                if (Mimic.transformTimer > 0f && Mimic.mimic == player && Mimic.transformTarget != null) playerName = Mimic.transformTarget.Data.PlayerName;
                if (Puppeteer.morphed && Puppeteer.puppeteer == player && Puppeteer.transformTarget != null) playerName = Puppeteer.transformTarget.Data.PlayerName;

                player.cosmetics.nameText.text = Helpers.hidePlayerName(PlayerInCache.LocalPlayer.PlayerControl, player) ? "" : playerName;
                if (PlayerInCache.LocalPlayer.Data.Role.IsImpostor && player.Data.Role.IsImpostor) {
                    player.cosmetics.nameText.color = Palette.ImpostorRed;
                } else {
                    player.cosmetics.nameText.color = Color.white;
                }
            }
            if (MeetingHud.Instance != null) {
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates) {
                    PlayerControl playerControl = playersById.ContainsKey((byte)player.TargetPlayerId) ? playersById[(byte)player.TargetPlayerId] : null;
                    if (playerControl != null) {
                        player.NameText.text = playerControl.Data.PlayerName;
                        if (PlayerInCache.LocalPlayer.Data.Role.IsImpostor && playerControl.Data.Role.IsImpostor) {
                            player.NameText.color = Palette.ImpostorRed;
                        } else {
                            player.NameText.color = Color.white;
                        }
                    }
                }
            }
            if (PlayerInCache.LocalPlayer.Data.Role.IsImpostor) {
                List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToList();
                impostors.RemoveAll(x => !x.Data.Role.IsImpostor);
                foreach (PlayerControl player in impostors)
                    player.cosmetics.nameText.color = Palette.ImpostorRed;
                if (MeetingHud.Instance != null)
                    foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates) {
                        PlayerControl playerControl = Helpers.playerById((byte)player.TargetPlayerId);
                        if (playerControl != null && playerControl.Data.Role.IsImpostor)
                            player.NameText.color =  Palette.ImpostorRed;
                    }
            }
        }
        static void setPlayerNameColor(PlayerControl p, Color color) {
            p.cosmetics.nameText.color = color;
            if (MeetingHud.Instance != null)
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                    if (player.NameText != null && p.PlayerId == player.TargetPlayerId)
                        player.NameText.color = color;
        }
        static void setNameColors() {

            switch (gameType) {
                case 0:
                case 1:
                    // Crewmates name color
                    var localPlayer = PlayerInCache.LocalPlayer.PlayerControl;
                    var localRole = RoleInfo.getRoleInfoForPlayer(localPlayer).FirstOrDefault();
                    setPlayerNameColor(localPlayer, localRole.color);

                    /*if (Captain.captain != null && Captain.captain == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Captain.captain, Captain.color);
                    else if (Mechanic.mechanic != null && Mechanic.mechanic == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Mechanic.mechanic, Mechanic.color);
                    else if (Sheriff.sheriff != null && Sheriff.sheriff == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Sheriff.sheriff, Sheriff.color);
                    else if (Detective.detective != null && Detective.detective == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Detective.detective, Detective.color);
                    else if (Forensic.forensic != null && Forensic.forensic == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Forensic.forensic, Forensic.color);
                    else if (TimeTraveler.timeTraveler != null && TimeTraveler.timeTraveler == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(TimeTraveler.timeTraveler, TimeTraveler.color);
                    else if (Squire.squire != null && Squire.squire == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Squire.squire, Squire.color);
                    else if (Cheater.cheater != null && Cheater.cheater == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Cheater.cheater, Cheater.color);
                    else if (FortuneTeller.fortuneTeller != null && FortuneTeller.fortuneTeller == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(FortuneTeller.fortuneTeller, FortuneTeller.color);
                    else if (Hacker.hacker != null && Hacker.hacker == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Hacker.hacker, Hacker.color);
                    else if (Sleuth.sleuth != null && Sleuth.sleuth == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Sleuth.sleuth, Sleuth.color);
                    else if (Fink.fink != null && Fink.fink == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Fink.fink, Fink.color);
                    else if (Kid.kid != null && Kid.kid == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Kid.kid, Kid.color);
                    else if (Welder.welder != null && Welder.welder == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Welder.welder, Welder.color);
                    else if (Spiritualist.spiritualist != null && Spiritualist.spiritualist == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Spiritualist.spiritualist, Spiritualist.color);
                    else if (Coward.coward != null && Coward.coward == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Coward.coward, Coward.color);
                    else if (Vigilant.vigilant != null && Vigilant.vigilant == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Vigilant.vigilant, Vigilant.color);
                    else if (Vigilant.vigilantMira != null && Vigilant.vigilantMira == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Vigilant.vigilantMira, Vigilant.color);
                    else if (Hunter.hunter != null && Hunter.hunter == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Hunter.hunter, Hunter.color);
                    else if (Jinx.jinx != null && Jinx.jinx == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Jinx.jinx, Jinx.color);
                    else if (Bat.bat != null && Bat.bat == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Bat.bat, Bat.color);
                    else if (Necromancer.necromancer != null && Necromancer.necromancer == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Necromancer.necromancer, Necromancer.color);
                    else if (Engineer.engineer != null && Engineer.engineer == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Engineer.engineer, Engineer.color);
                    else if (Locksmith.locksmith != null && Locksmith.locksmith == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Locksmith.locksmith, Locksmith.color);
                    else if (TaskMaster.taskMaster != null && TaskMaster.taskMaster == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(TaskMaster.taskMaster, TaskMaster.color);
                    else if (Jailer.jailer != null && Jailer.jailer == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Jailer.jailer, Jailer.color);

                    // Neutrals name color
                    else if (Joker.joker != null && Joker.joker == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Joker.joker, Joker.color);
                    else if (RoleThief.rolethief != null && RoleThief.rolethief == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(RoleThief.rolethief, RoleThief.color);
                    else if (Pyromaniac.pyromaniac != null && Pyromaniac.pyromaniac == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(Pyromaniac.pyromaniac, Pyromaniac.color);
                    }
                    else if (TreasureHunter.treasureHunter != null && TreasureHunter.treasureHunter == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(TreasureHunter.treasureHunter, TreasureHunter.color);
                    }
                    else if (Devourer.devourer != null && Devourer.devourer == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(Devourer.devourer, Devourer.color);
                    }
                    else if (Poisoner.poisoner != null && Poisoner.poisoner == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(Poisoner.poisoner, Poisoner.color);
                    }
                    else if (Puppeteer.puppeteer != null && Puppeteer.puppeteer == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(Puppeteer.puppeteer, Puppeteer.color);
                    }
                    else*/
                    if (Exiler.exiler != null && Exiler.exiler == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(Exiler.exiler, Exiler.color);
                        if (Exiler.target != null) {
                            setPlayerNameColor(Exiler.target, Sheriff.color);
                            if (Challenger.isDueling) {
                                setPlayerNameColor(Exiler.target, Color.white);
                            }
                        }
                    }/*
                    else if (Amnesiac.amnesiac != null && Amnesiac.amnesiac == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(Amnesiac.amnesiac, Amnesiac.color);
                    }
                    else if (Seeker.seeker != null && Seeker.seeker == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(Seeker.seeker, Seeker.color);
                    }

                    // Rebels name color
                    */else if (Renegade.renegade != null && Renegade.renegade == PlayerInCache.LocalPlayer.PlayerControl) {
                        // Renegade can see his minion
                        setPlayerNameColor(Renegade.renegade, Renegade.color);
                        if (Minion.minion != null) {
                            setPlayerNameColor(Minion.minion, Renegade.color);
                        }
                        if (Renegade.fakeMinion != null) {
                            setPlayerNameColor(Renegade.fakeMinion, Renegade.color);
                        }
                    }
                    /*else if (BountyHunter.bountyhunter != null && BountyHunter.bountyhunter == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(BountyHunter.bountyhunter, BountyHunter.color);
                    else if (Trapper.trapper != null && Trapper.trapper == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Trapper.trapper, Trapper.color);
                    else if (Yinyanger.yinyanger != null && Yinyanger.yinyanger == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Yinyanger.yinyanger, Yinyanger.color);
                    else if (Challenger.challenger != null && Challenger.challenger == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Challenger.challenger, Challenger.color);
                    else if (Ninja.ninja != null && Ninja.ninja == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Ninja.ninja, Ninja.color);
                    else if (Berserker.berserker != null && Berserker.berserker == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Berserker.berserker, Berserker.color);
                    */
                    else if (Yandere.yandere != null && Yandere.yandere == PlayerInCache.LocalPlayer.PlayerControl) {
                        setPlayerNameColor(Yandere.yandere, Yandere.color);
                        if (Yandere.target != null) {
                            setPlayerNameColor(Yandere.target, Sheriff.color);
                            if (Seeker.isMinigaming) {
                                setPlayerNameColor(Yandere.target, Color.white);
                            }
                        }
                    }/*
                    else if (Stranded.stranded != null && Stranded.stranded == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Stranded.stranded, Stranded.color);
                    else if (Monja.monja != null && Monja.monja == PlayerInCache.LocalPlayer.PlayerControl)
                        setPlayerNameColor(Monja.monja, Monja.color);

                    else if (Modifiers.lover1 != null && Modifiers.lover2 != null && (Modifiers.lover1 == PlayerInCache.LocalPlayer.PlayerControl || Modifiers.lover2 == PlayerInCache.LocalPlayer.PlayerControl)) {
                        setPlayerNameColor(Modifiers.lover1, Modifiers.loverscolor);
                        setPlayerNameColor(Modifiers.lover2, Modifiers.loverscolor);
                    }*/
                    // No else if here, as a Lover of team Renegade needs the colors
                    if (Minion.minion != null && Minion.minion == PlayerInCache.LocalPlayer.PlayerControl) {
                        // Minion can see the renegade
                        setPlayerNameColor(Minion.minion, Minion.color);
                        if (Renegade.renegade != null) {
                            setPlayerNameColor(Renegade.renegade, Renegade.color);
                        }
                    }

                    // Impostor roles with no color changes: Mimic, Painter, Demon, Janitor, Illusionist, Manipulator, Bomberman, Chameleon, Gambler, Sorcerer, Medusa, Hypnotist, Archer, Plumber, Librarian 
                    break;
                case 2:
                    // CTF
                    if (CaptureTheFlag.stealerPlayer != null) {
                        setPlayerNameColor(CaptureTheFlag.stealerPlayer, Palette.PlayerColors[15]);
                    }

                    foreach (PlayerControl redplayer in CaptureTheFlag.redteamFlag) {
                        if (redplayer != null) {
                            setPlayerNameColor(redplayer, Palette.PlayerColors[0]);
                        }
                    }
                    foreach (PlayerControl blueplayer in CaptureTheFlag.blueteamFlag) {
                        if (blueplayer != null) {
                            setPlayerNameColor(blueplayer, Palette.PlayerColors[1]);
                        }
                    }
                    break;
                case 3:
                    // PT
                    foreach (PlayerControl policeplayer in PoliceAndThief.policeTeam) {
                        if (policeplayer != null) {
                            if (PoliceAndThief.policeplayer02 != null && policeplayer == PoliceAndThief.policeplayer02 || PoliceAndThief.policeplayer04 != null && policeplayer == PoliceAndThief.policeplayer04) {
                                setPlayerNameColor(policeplayer, Palette.PlayerColors[5]);
                            }
                            else {
                                setPlayerNameColor(policeplayer, Palette.PlayerColors[10]);
                            }
                        }
                    }
                    foreach (PlayerControl thiefplayer in PoliceAndThief.thiefTeam) {
                        if (thiefplayer != null) {
                            setPlayerNameColor(thiefplayer, Palette.PlayerColors[16]);
                        }
                    }
                    break;
                case 4:
                    // KOTH
                    if (KingOfTheHill.usurperPlayer != null) {
                        setPlayerNameColor(KingOfTheHill.usurperPlayer, Palette.PlayerColors[15]);
                    }

                    foreach (PlayerControl greenplayer in KingOfTheHill.greenTeam) {
                        if (greenplayer != null) {
                            setPlayerNameColor(greenplayer, Palette.PlayerColors[2]);
                        }
                    }
                    foreach (PlayerControl yellowplayer in KingOfTheHill.yellowTeam) {
                        if (yellowplayer != null) {
                            setPlayerNameColor(yellowplayer, Palette.PlayerColors[5]);
                        }
                    }
                    break;
                case 5:
                    // HP
                    foreach (PlayerControl notpotatoplayer in HotPotato.notPotatoTeam) {
                        if (notpotatoplayer != null) {
                            setPlayerNameColor(notpotatoplayer, Palette.PlayerColors[10]);
                        }
                    }

                    foreach (PlayerControl explodedpotatoplayer in HotPotato.explodedPotatoTeam) {
                        if (explodedpotatoplayer != null) {
                            setPlayerNameColor(explodedpotatoplayer, Palette.PlayerColors[9]);
                        }
                    }

                    if (HotPotato.hotPotatoPlayer != null) {
                        setPlayerNameColor(HotPotato.hotPotatoPlayer, Palette.PlayerColors[15]);
                    }
                    break;
                case 6:
                    // ZL
                    foreach (PlayerControl survivorPlayer in ZombieLaboratory.survivorTeam) {
                        if (survivorPlayer != null) {
                            setPlayerNameColor(survivorPlayer, Palette.PlayerColors[10]);
                        }
                    }

                    foreach (PlayerControl infectedPlayer in ZombieLaboratory.infectedTeam) {
                        if (infectedPlayer != null) {
                            setPlayerNameColor(infectedPlayer, Palette.PlayerColors[5]);
                        }
                    }

                    foreach (PlayerControl zombiePlayer in ZombieLaboratory.zombieTeam) {
                        if (zombiePlayer != null) {
                            setPlayerNameColor(zombiePlayer, Palette.PlayerColors[16]);
                        }
                    }

                    if (ZombieLaboratory.nursePlayer != null) {
                        setPlayerNameColor(ZombieLaboratory.nursePlayer, Palette.PlayerColors[3]);
                    }
                    break;
                case 7:
                    // BR
                    if (BattleRoyale.matchType == 0) {
                        foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                            if (soloPlayer != null) {
                                setPlayerNameColor(soloPlayer, Palette.PlayerColors[2]);
                            }
                        }
                    }
                    else {
                        if (BattleRoyale.serialKiller != null) {
                            setPlayerNameColor(BattleRoyale.serialKiller, Palette.PlayerColors[15]);
                        }

                        foreach (PlayerControl limeplayer in BattleRoyale.limeTeam) {
                            if (limeplayer != null) {
                                setPlayerNameColor(limeplayer, Palette.PlayerColors[11]);
                            }
                        }

                        foreach (PlayerControl pinkplayer in BattleRoyale.pinkTeam) {
                            if (pinkplayer != null) {
                                setPlayerNameColor(pinkplayer, Palette.PlayerColors[13]);
                            }
                        }
                    }
                    break;
                case 8:
                    // MF
                    if (MonjaFestival.bigMonjaPlayer != null) {
                        setPlayerNameColor(MonjaFestival.bigMonjaPlayer, Palette.PlayerColors[15]);
                    }

                    foreach (PlayerControl greenplayer in MonjaFestival.greenTeam) {
                        if (greenplayer != null) {
                            setPlayerNameColor(greenplayer, Palette.PlayerColors[2]);
                        }
                    }

                    foreach (PlayerControl cyanplayer in MonjaFestival.cyanTeam) {
                        if (cyanplayer != null) {
                            setPlayerNameColor(cyanplayer, Palette.PlayerColors[10]);
                        }
                    }
                    break;
            }
        }
        static void setNameTags() {

            switch (gameType) {
                case 0:
                case 1:
                    // Lovers add a heart to their names
                    if (Modifiers.lover1 != null && Modifiers.lover2 != null && (Modifiers.lover1 == PlayerInCache.LocalPlayer.PlayerControl || Modifiers.lover2 == PlayerInCache.LocalPlayer.PlayerControl)) {
                        string suffix = Helpers.cs(Modifiers.loverscolor, " ♥");
                        Modifiers.lover1.cosmetics.nameText.text += suffix;
                        Modifiers.lover2.cosmetics.nameText.text += suffix;

                        if (MeetingHud.Instance != null)
                            foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                                if (Modifiers.lover1.PlayerId == player.TargetPlayerId || Modifiers.lover2.PlayerId == player.TargetPlayerId)
                                    player.NameText.text += suffix;
                    }

                    // Forensic show color type on meeting
                    if (Forensic.forensic != null && PlayerInCache.LocalPlayer.PlayerControl == Forensic.forensic) {
                        if (MeetingHud.Instance != null) {
                            foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates) {
                                var target = Helpers.playerById(player.TargetPlayerId);
                                if (target != null) player.NameText.text += $" ({(Helpers.isLighterColor(target.Data.DefaultOutfit.ColorId) ? "L" : "D")})";
                            }
                        }
                    }
                    break;
                case 6:
                    // ZL Timers
                    foreach (PlayerControl survivorPlayer in ZombieLaboratory.survivorTeam) {
                        if (survivorPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                            if (ZombieLaboratory.survivorPlayer01 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer01Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer01.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer02 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer02Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer02.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer03 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer03Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer03.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer04 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer04Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer04.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer05 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer05Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer05.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer06 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer06Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer06.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer07 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer07Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer07.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer08 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer08Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer08.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer09 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer09Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer09.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer10 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer10Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer10.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer11 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer11Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer11.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer12 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer12Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer12.cosmetics.nameText.text += suffix;
                            }
                            if (ZombieLaboratory.survivorPlayer13 != null) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + ZombieLaboratory.survivorPlayer13Timer.ToString("F0") + ")");
                                ZombieLaboratory.survivorPlayer13.cosmetics.nameText.text += suffix;
                            }
                        }
                    }
                    break;
                case 7:
                    // BR Lives
                    if (BattleRoyale.matchType == 0) {
                        if (PlayerInCache.LocalPlayer.Data.IsDead) {
                            if (BattleRoyale.soloPlayer01 != null) {
                                BattleRoyale.soloPlayer01.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer01Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer02 != null) {
                                BattleRoyale.soloPlayer02.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer02Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer03 != null) {
                                BattleRoyale.soloPlayer03.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer03Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer04 != null) {
                                BattleRoyale.soloPlayer04.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer04Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer05 != null) {
                                BattleRoyale.soloPlayer05.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer05Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer06 != null) {
                                BattleRoyale.soloPlayer06.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer06Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer07 != null) {
                                BattleRoyale.soloPlayer07.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer07Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer08 != null) {
                                BattleRoyale.soloPlayer08.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer08Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer09 != null) {
                                BattleRoyale.soloPlayer09.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer09Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer10 != null) {
                                BattleRoyale.soloPlayer10.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer10Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer11 != null) {
                                BattleRoyale.soloPlayer11.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer11Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer12 != null) {
                                BattleRoyale.soloPlayer12.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer12Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer13 != null) {
                                BattleRoyale.soloPlayer13.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer13Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer14 != null) {
                                BattleRoyale.soloPlayer14.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer14Lifes + "♥)");
                            }
                            if (BattleRoyale.soloPlayer15 != null) {
                                BattleRoyale.soloPlayer15.cosmetics.nameText.text += Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer15Lifes + "♥)");
                            }
                        }
                        else {
                            if (BattleRoyale.soloPlayer01 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer01) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer01Lifes + "♥)");
                                BattleRoyale.soloPlayer01.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer02 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer02) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer02Lifes + "♥)");
                                BattleRoyale.soloPlayer02.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer03 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer03) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer03Lifes + "♥)");
                                BattleRoyale.soloPlayer03.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer04 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer04) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer04Lifes + "♥)");
                                BattleRoyale.soloPlayer04.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer05 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer05) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer05Lifes + "♥)");
                                BattleRoyale.soloPlayer05.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer06 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer06) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer06Lifes + "♥)");
                                BattleRoyale.soloPlayer06.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer07 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer07) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer07Lifes + "♥)");
                                BattleRoyale.soloPlayer07.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer08 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer08) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer08Lifes + "♥)");
                                BattleRoyale.soloPlayer08.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer09 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer09) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer09Lifes + "♥)");
                                BattleRoyale.soloPlayer09.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer10 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer10) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer10Lifes + "♥)");
                                BattleRoyale.soloPlayer10.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer11 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer11) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer11Lifes + "♥)");
                                BattleRoyale.soloPlayer11.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer12 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer12) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer12Lifes + "♥)");
                                BattleRoyale.soloPlayer12.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer13 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer13) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer13Lifes + "♥)");
                                BattleRoyale.soloPlayer13.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer14 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer14) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer14Lifes + "♥)");
                                BattleRoyale.soloPlayer14.cosmetics.nameText.text += suffix;
                            }
                            if (BattleRoyale.soloPlayer15 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer15) {
                                string suffix = Helpers.cs(Sheriff.color, " (" + BattleRoyale.soloPlayer15Lifes + "♥)");
                                BattleRoyale.soloPlayer15.cosmetics.nameText.text += suffix;
                            }
                        }
                    }
                    else {
                        foreach (PlayerControl limePlayer in BattleRoyale.limeTeam) {
                            if (limePlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                                if (BattleRoyale.limePlayer01 != null) {
                                    string suffix = Helpers.cs(FortuneTeller.color, " (" + BattleRoyale.limePlayer01Lifes + "♥)");
                                    BattleRoyale.limePlayer01.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.limePlayer02 != null) {
                                    string suffix = Helpers.cs(FortuneTeller.color, " (" + BattleRoyale.limePlayer02Lifes + "♥)");
                                    BattleRoyale.limePlayer02.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.limePlayer03 != null) {
                                    string suffix = Helpers.cs(FortuneTeller.color, " (" + BattleRoyale.limePlayer03Lifes + "♥)");
                                    BattleRoyale.limePlayer03.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.limePlayer04 != null) {
                                    string suffix = Helpers.cs(FortuneTeller.color, " (" + BattleRoyale.limePlayer04Lifes + "♥)");
                                    BattleRoyale.limePlayer04.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.limePlayer05 != null) {
                                    string suffix = Helpers.cs(FortuneTeller.color, " (" + BattleRoyale.limePlayer05Lifes + "♥)");
                                    BattleRoyale.limePlayer05.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.limePlayer06 != null) {
                                    string suffix = Helpers.cs(FortuneTeller.color, " (" + BattleRoyale.limePlayer06Lifes + "♥)");
                                    BattleRoyale.limePlayer06.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.limePlayer07 != null) {
                                    string suffix = Helpers.cs(FortuneTeller.color, " (" + BattleRoyale.limePlayer07Lifes + "♥)");
                                    BattleRoyale.limePlayer07.cosmetics.nameText.text += suffix;
                                }
                            }
                        }
                        foreach (PlayerControl pinkPlayer in BattleRoyale.pinkTeam) {
                            if (pinkPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                                if (BattleRoyale.pinkPlayer01 != null) {
                                    string suffix = Helpers.cs(Locksmith.color, " (" + BattleRoyale.pinkPlayer01Lifes + "♥)");
                                    BattleRoyale.pinkPlayer01.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.pinkPlayer02 != null) {
                                    string suffix = Helpers.cs(Locksmith.color, " (" + BattleRoyale.pinkPlayer02Lifes + "♥)");
                                    BattleRoyale.pinkPlayer02.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.pinkPlayer03 != null) {
                                    string suffix = Helpers.cs(Locksmith.color, " (" + BattleRoyale.pinkPlayer03Lifes + "♥)");
                                    BattleRoyale.pinkPlayer03.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.pinkPlayer04 != null) {
                                    string suffix = Helpers.cs(Locksmith.color, " (" + BattleRoyale.pinkPlayer04Lifes + "♥)");
                                    BattleRoyale.pinkPlayer04.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.pinkPlayer05 != null) {
                                    string suffix = Helpers.cs(Locksmith.color, " (" + BattleRoyale.pinkPlayer05Lifes + "♥)");
                                    BattleRoyale.pinkPlayer05.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.pinkPlayer06 != null) {
                                    string suffix = Helpers.cs(Locksmith.color, " (" + BattleRoyale.pinkPlayer06Lifes + "♥)");
                                    BattleRoyale.pinkPlayer06.cosmetics.nameText.text += suffix;
                                }
                                if (BattleRoyale.pinkPlayer07 != null) {
                                    string suffix = Helpers.cs(Locksmith.color, " (" + BattleRoyale.pinkPlayer07Lifes + "♥)");
                                    BattleRoyale.pinkPlayer07.cosmetics.nameText.text += suffix;
                                }
                            }
                        }
                        if (BattleRoyale.serialKiller != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.serialKiller) {
                            string suffix = Helpers.cs(Joker.color, " (" + BattleRoyale.serialKillerLifes + "♥)");
                            BattleRoyale.serialKiller.cosmetics.nameText.text += suffix;
                        }
                    }
                    break;
            }
        }
        static void UpdateMiniMap() {

            if (MapBehaviour.Instance != null && MapBehaviour.Instance.IsOpen && gameType >= 2) {
                switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                    case 0:
                        GameObject minimapSabotageSkeld = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/InfectedOverlay");
                        minimapSabotageSkeld.SetActive(false);
                        if (activatedSensei && !updatedSenseiMinimap) {
                            GameObject mymap = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/Background");
                            mymap.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.customMinimap.GetComponent<SpriteRenderer>().sprite;
                            GameObject hereindicator = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/HereIndicatorParent");
                            hereindicator.transform.position = hereindicator.transform.position + new Vector3(0.23f, -0.8f, 0);

                            // Map room names
                            GameObject minimapNames = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/RoomNames (1)");
                            minimapNames.transform.GetChild(0).transform.position = minimapNames.transform.GetChild(0).transform.position + new Vector3(0f, -0.5f, 0); // Upper engine
                            minimapNames.transform.GetChild(2).transform.position = minimapNames.transform.GetChild(2).transform.position + new Vector3(0.7f, -0.55f, 0); // Reactor
                            minimapNames.transform.GetChild(3).transform.position = minimapNames.transform.GetChild(3).transform.position + new Vector3(1.75f, 2.37f, 0); // security
                            minimapNames.transform.GetChild(4).transform.position = minimapNames.transform.GetChild(4).transform.position + new Vector3(0.89f, -1.18f, 0); // medbey
                            minimapNames.transform.GetChild(5).transform.position = minimapNames.transform.GetChild(5).transform.position + new Vector3(0.52f, -1.32f, 0); // Cafetería
                            minimapNames.transform.GetChild(6).transform.position = minimapNames.transform.GetChild(6).transform.position + new Vector3(1f, -1.59f, 0); // weapons
                            minimapNames.transform.GetChild(7).transform.position = minimapNames.transform.GetChild(7).transform.position + new Vector3(-1.72f, -3.03f, 0); // nav
                            minimapNames.transform.GetChild(8).transform.position = minimapNames.transform.GetChild(8).transform.position + new Vector3(-0.08f, 1.45f, 0); // shields
                            minimapNames.transform.GetChild(9).transform.position = minimapNames.transform.GetChild(9).transform.position + new Vector3(1.1f, 2.88f, 0); // cooms
                            minimapNames.transform.GetChild(10).transform.position = minimapNames.transform.GetChild(10).transform.position + new Vector3(-2.2f, -0.82f, 0); // storage
                            minimapNames.transform.GetChild(11).transform.position = minimapNames.transform.GetChild(11).transform.position + new Vector3(0.32f, -1.02f, 0); // Admin
                            minimapNames.transform.GetChild(12).transform.position = minimapNames.transform.GetChild(12).transform.position + new Vector3(0.53f, -2.1f, 0); // electrical
                            minimapNames.transform.GetChild(13).transform.position = minimapNames.transform.GetChild(13).transform.position + new Vector3(-3.5f, -0.5f, 0); // o2

                            // Map sabotage
                            GameObject minimapSabotage = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/InfectedOverlay");
                            minimapSabotage.transform.GetChild(0).gameObject.SetActive(false); // cafeteria doors
                            minimapSabotage.transform.GetChild(2).gameObject.SetActive(false); // medbey doors
                            minimapSabotage.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false); // electrical doors
                            minimapSabotage.transform.GetChild(5).gameObject.SetActive(false); // upper engine doors
                            minimapSabotage.transform.GetChild(6).gameObject.SetActive(false); // lower engine doors
                            minimapSabotage.transform.GetChild(7).gameObject.SetActive(false); // storage doors
                            minimapSabotage.transform.GetChild(9).gameObject.SetActive(false); // security doors

                            minimapSabotage.transform.GetChild(1).transform.position = minimapSabotage.transform.GetChild(1).transform.position + new Vector3(0.95f, 3.3f, 0); // Sabotage cooms
                            minimapSabotage.transform.GetChild(3).transform.GetChild(1).transform.position = minimapSabotage.transform.GetChild(3).transform.GetChild(1).transform.position + new Vector3(0.165f, -1.2f, 0); // Sabotage electrical
                            minimapSabotage.transform.GetChild(4).transform.position = minimapSabotage.transform.GetChild(4).transform.position + new Vector3(-3f, 0.05f, 0); // Sabotage o2
                            minimapSabotage.transform.GetChild(8).transform.position = minimapSabotage.transform.GetChild(8).transform.position + new Vector3(0.6f, 0.1f, 0); // Sabotage reactor


                            updatedSenseiMinimap = true;
                        }
                        break;
                    case 1:
                        GameObject minimapSabotageMira = GameObject.Find("Main Camera/Hud/HqMap(Clone)/InfectedOverlay");
                        minimapSabotageMira.SetActive(false);
                        break;
                    case 2:
                        GameObject minimapSabotagePolus = GameObject.Find("Main Camera/Hud/PbMap(Clone)/InfectedOverlay");
                        minimapSabotagePolus.SetActive(false);
                        break;
                    case 3:
                        GameObject minimapSabotageDleks = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/InfectedOverlay");
                        minimapSabotageDleks.SetActive(false);
                        break;
                    case 4:
                        GameObject minimapSabotageAirship = GameObject.Find("Main Camera/Hud/AirshipMap(Clone)/InfectedOverlay");
                        minimapSabotageAirship.SetActive(false);
                        break;
                    case 5:
                        GameObject minimapSabotageFungle = GameObject.Find("Main Camera/Hud/FungleMap(Clone)/InfectedOverlay");
                        minimapSabotageFungle.SetActive(false);
                        break;
                    case 6:
                        GameObject minimapSabotageSubmerged = GameObject.Find("Main Camera/Hud/HudMapPrefab(Clone)(Clone)/MapHud/InfectedOverlay");
                        minimapSabotageSubmerged.SetActive(false);
                        break;
                }
            }
            else if (MapBehaviour.Instance != null && MapBehaviour.Instance.IsOpen && GameOptionsManager.Instance.currentGameOptions.MapId == 0 && activatedSensei && !updatedSenseiMinimap && gameType <= 1) {
                GameObject mymap = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/Background");
                mymap.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.customMinimap.GetComponent<SpriteRenderer>().sprite;
                GameObject hereindicator = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/HereIndicatorParent");
                hereindicator.transform.position = hereindicator.transform.position + new Vector3(0.23f, -0.8f, 0);

                // Map room names
                GameObject minimapNames = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/RoomNames (1)");
                minimapNames.transform.GetChild(0).transform.position = minimapNames.transform.GetChild(0).transform.position + new Vector3(0f, -0.5f, 0); // upper engine
                minimapNames.transform.GetChild(2).transform.position = minimapNames.transform.GetChild(2).transform.position + new Vector3(0.7f, -0.55f, 0); // Reactor
                minimapNames.transform.GetChild(3).transform.position = minimapNames.transform.GetChild(3).transform.position + new Vector3(1.75f, 2.37f, 0); // security
                minimapNames.transform.GetChild(4).transform.position = minimapNames.transform.GetChild(4).transform.position + new Vector3(0.89f, -1.18f, 0); // medbey
                minimapNames.transform.GetChild(5).transform.position = minimapNames.transform.GetChild(5).transform.position + new Vector3(0.52f, -1.32f, 0); // Cafetería
                minimapNames.transform.GetChild(6).transform.position = minimapNames.transform.GetChild(6).transform.position + new Vector3(1f, -1.59f, 0); // weapons
                minimapNames.transform.GetChild(7).transform.position = minimapNames.transform.GetChild(7).transform.position + new Vector3(-1.72f, -3.03f, 0); // nav
                minimapNames.transform.GetChild(8).transform.position = minimapNames.transform.GetChild(8).transform.position + new Vector3(-0.08f, 1.45f, 0); // shields
                minimapNames.transform.GetChild(9).transform.position = minimapNames.transform.GetChild(9).transform.position + new Vector3(1.1f, 2.88f, 0); // cooms
                minimapNames.transform.GetChild(10).transform.position = minimapNames.transform.GetChild(10).transform.position + new Vector3(-2.2f, -0.82f, 0); // storage
                minimapNames.transform.GetChild(11).transform.position = minimapNames.transform.GetChild(11).transform.position + new Vector3(0.32f, -1.02f, 0); // Admin
                minimapNames.transform.GetChild(12).transform.position = minimapNames.transform.GetChild(12).transform.position + new Vector3(0.53f, -2.1f, 0); // elec
                minimapNames.transform.GetChild(13).transform.position = minimapNames.transform.GetChild(13).transform.position + new Vector3(-3.5f, -0.5f, 0); // o2

                // Map sabotage
                GameObject minimapSabotage = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/InfectedOverlay");
                minimapSabotage.transform.GetChild(0).gameObject.SetActive(false); // cafeteria doors
                minimapSabotage.transform.GetChild(2).gameObject.SetActive(false); // medbey doors
                minimapSabotage.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false); // Puertas electricidad
                minimapSabotage.transform.GetChild(5).gameObject.SetActive(false); // upper engine doors
                minimapSabotage.transform.GetChild(6).gameObject.SetActive(false); // lower engine doors
                minimapSabotage.transform.GetChild(7).gameObject.SetActive(false); // storage doors
                minimapSabotage.transform.GetChild(9).gameObject.SetActive(false); // security doors

                minimapSabotage.transform.GetChild(1).transform.position = minimapSabotage.transform.GetChild(1).transform.position + new Vector3(0.95f, 3.3f, 0); // Sabotage cooms
                minimapSabotage.transform.GetChild(3).transform.GetChild(1).transform.position = minimapSabotage.transform.GetChild(3).transform.GetChild(1).transform.position + new Vector3(0.165f, -1.2f, 0); // Sabotage elec
                minimapSabotage.transform.GetChild(4).transform.position = minimapSabotage.transform.GetChild(4).transform.position + new Vector3(-3f, 0.05f, 0); // Sabotage o2
                minimapSabotage.transform.GetChild(8).transform.position = minimapSabotage.transform.GetChild(8).transform.position + new Vector3(0.6f, 0.1f, 0); // Sabotage reactor


                updatedSenseiMinimap = true;
            }

            // If bomb, lights actives or special 1vs1 condition, prevent sabotage open map
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal && gameType <= 1 && PlayerInCache.LocalPlayer.Data.Role.IsImpostor && MapBehaviour.Instance != null && MapBehaviour.Instance.IsOpen && (alivePlayers <= 2 || Bomberman.activeBomb || Challenger.isDueling || Seeker.isMinigaming || Illusionist.lightsOutTimer > 0 || Monja.awakened)) {
                MapBehaviour.Instance.Close();
            }
        }
        static void shakeScreenIfReactorSabotage() {
            if (Monja.awakened) {
                HudManager.Instance.PlayerCam.shakeAmount = 0.05f;
                HudManager.Instance.PlayerCam.shakePeriod = 400;
            }
            if (shakeScreenReactor) {
                foreach (PlayerTask task in PlayerInCache.LocalPlayer.PlayerControl.myTasks) {
                    if (task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.StopCharles) {
                        HudManager.Instance.PlayerCam.shakeAmount = 0.025f;
                        HudManager.Instance.PlayerCam.shakePeriod = 400;
                    }
                }
            }
        }
        static void anonymousCommsSabotage() {
            if (anonymousComms) {
                foreach (PlayerTask task in PlayerInCache.LocalPlayer.PlayerControl.myTasks) {
                    if (task.TaskType == TaskTypes.FixComms) {
                        // Set grey painting while comms sabotage
                        foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                            player.setLook("", 6, "", "", "", "");
                            if (player.cosmetics.currentPet) player.cosmetics.currentPet.gameObject.SetActive(false);
                        }
                        isHappeningAnonymousComms = true;
                    }
                }
            }
        }
        static void slowSpeedIfOxigenSabotage() {
            if (slowSpeedOxigen) {
                foreach (PlayerTask task in PlayerInCache.LocalPlayer.PlayerControl.myTasks) {
                    if (task.TaskType == TaskTypes.RestoreOxy) {
                        // Set slow speed while oxygen sabotage
                        NoOxyTask oxygenTask = UnityEngine.Object.FindObjectOfType<NoOxyTask>();
                        foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                            player.MyPhysics.Speed = Math.Max(1.5f,Math.Min(2.5f, 2.5f * oxygenTask.reactor.Countdown / oxygenTask.reactor.LifeSuppDuration));
                        }
                    }
                }
            }
        }
        static void updateImpostorKillButton(HudManager __instance) {
            if (!PlayerInCache.LocalPlayer.Data.Role.IsImpostor || MeetingHud.Instance || MapBehaviour.Instance != null && MapBehaviour.Instance.IsOpen) return;
            bool enabled = true;
            if (Demon.demon != null && Demon.demon == PlayerInCache.LocalPlayer.PlayerControl && !Challenger.isDueling && !Seeker.isMinigaming)
                enabled = false;
            else if (Janitor.janitor != null && Janitor.dragginBody && PlayerInCache.LocalPlayer.PlayerControl == Janitor.janitor)
                enabled = false;
            else if (Archer.archer != null && PlayerInCache.LocalPlayer.PlayerControl == Archer.archer && !Challenger.isDueling && !Seeker.isMinigaming)
                enabled = false;
            else if (Challenger.isDueling || Seeker.isMinigaming || Monja.awakened || gameType >= 2)
                enabled = false;
            if (enabled) __instance.KillButton.Show();
            else __instance.KillButton.Hide();
        }
        static void updateReportButton(HudManager __instance) {
            if (gameType <= 1) {
                if (!activatedReportButtonAfterCustomMode) {
                    __instance.ReportButton.gameObject.SetActive(true);
                    __instance.ReportButton.graphic.enabled = true;
                    __instance.ReportButton.enabled = true;
                    activatedReportButtonAfterCustomMode = true;
                }
                return;
            }

            bool enabled = true;
            if (gameType >= 2 || Monja.awakened)
                enabled = false;
            enabled &= __instance.ReportButton.isActiveAndEnabled;

            __instance.ReportButton.gameObject.SetActive(enabled);
            __instance.ReportButton.graphic.enabled = enabled;
            __instance.ReportButton.enabled = enabled;
        }
        static void timerUpdate() {
            var deltaTime = Time.deltaTime;

            switch (gameType) {
                case 0:
                case 1:
                    if (Illusionist.illusionist != null) {
                        Illusionist.lightsOutTimer -= deltaTime;
                    }
                    if (Manipulator.manipulatedVictim != null && !MeetingHud.Instance && !Seeker.isMinigaming && !Challenger.isDueling && Manipulator.manipulatedVictim.CanMove) {
                        Manipulator.manipulatedVictimTimer -= deltaTime;
                        Manipulator.manipulatedVictimTimerCountButtonText.text = $"{Manipulator.manipulatedVictimTimer.ToString("F0")}";                        
                    }
                    if (Bomberman.bomberman != null) {
                        Bomberman.bombTimer -= deltaTime;
                    }
                    if (Hypnotist.hypnotist != null) {
                        Hypnotist.messageTimer -= deltaTime;
                    }
                    if (Berserker.berserker != null && Berserker.killedFirstTime && MeetingHud.Instance == null && !Seeker.isMinigaming && !Berserker.berserker.Data.IsDead && Berserker.berserker.CanMove) {
                        Berserker.timeToKill -= deltaTime;
                        Berserker.berserkerCountButtonText.text = $"{Berserker.timeToKill.ToString("F0")}";
                        if (Berserker.timeToKill <= 0) {
                            Berserker.berserker.MurderPlayer(Berserker.berserker, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                        }
                    }
                    if (Monja.monja != null && Monja.awakened) {
                        Monja.awakenTimer -= deltaTime;
                    }
                    if (Detective.detective != null) {
                        Detective.detectiveTimer -= deltaTime;
                    }
                    if (Hacker.hacker != null) {
                        Hacker.hackerTimer -= deltaTime;
                    }
                    if (Sleuth.sleuth != null) {
                        Sleuth.corpsesPathfindTimer -= deltaTime;
                        Sleuth.timer -= deltaTime;
                    }
                    if (Fink.fink != null) {
                        Fink.finkTimer -= deltaTime;
                    }
                    if (Bat.bat != null) {
                        Bat.frequencyTimer -= deltaTime;
                    }
                    if (Engineer.engineer != null) {
                        Engineer.messageTimer -= deltaTime;
                    }
                    if (TaskMaster.taskMaster != null) {
                        TaskMaster.taskTimer -= deltaTime;
                    }
                    if (Modifiers.performer != null) {
                        Modifiers.performerDuration -= deltaTime;
                    }
                    break;
                case 2:
                    // CTF timers
                    progressStart += deltaTime;
                    gamemodeMatchDuration -= deltaTime;
                    if (progress != null) {
                        progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" + Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0");
                        progress.GetComponent<ProgressTracker>().curValue = Mathf.Lerp(PlayerInCache.AllPlayers.Count - 1, 0, progressStart / progressEnd);
                    }
                    if (gamemodeMatchDuration <= 0) {
                        // both teams with same points = Draw
                        if (CaptureTheFlag.currentRedTeamPoints == CaptureTheFlag.currentBlueTeamPoints) {
                            CaptureTheFlag.triggerDrawWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.DrawTeamWin, false);
                        }
                        // Red team more points than blue team = red team win
                        else if (CaptureTheFlag.currentRedTeamPoints > CaptureTheFlag.currentBlueTeamPoints) {
                            CaptureTheFlag.triggerRedTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.RedTeamFlagWin, false);
                        }
                        // otherwise blue team win
                        else {
                            CaptureTheFlag.triggerBlueTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BlueTeamFlagWin, false);
                        }
                    }
                    break;
                case 3:
                    // PT timers
                    PoliceAndThief.policeplayer01lightTimer -= deltaTime;
                    PoliceAndThief.policeplayer02lightTimer -= deltaTime;
                    PoliceAndThief.policeplayer03lightTimer -= deltaTime;
                    PoliceAndThief.policeplayer04lightTimer -= deltaTime;
                    PoliceAndThief.policeplayer05lightTimer -= deltaTime;
                    PoliceAndThief.policeplayer06lightTimer -= deltaTime;

                    progressStart += deltaTime;
                    gamemodeMatchDuration -= deltaTime;
                    if (progress != null) {
                        progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" + Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0");
                        progress.GetComponent<ProgressTracker>().curValue = Mathf.Lerp(PlayerInCache.AllPlayers.Count - 1, 0, progressStart / progressEnd);
                    }
                    if (gamemodeMatchDuration <= 0) {
                        PoliceAndThief.triggerPoliceWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModePoliceWin, false);
                    }
                    break;
                case 4:
                    // KOTH:
                    progressStart += deltaTime;
                    gamemodeMatchDuration -= deltaTime;
                    if (progress != null) {
                        progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" + Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0");
                        progress.GetComponent<ProgressTracker>().curValue = Mathf.Lerp(PlayerInCache.AllPlayers.Count - 1, 0, progressStart / progressEnd);
                    }
                    if (gamemodeMatchDuration <= 0) {
                        // both teams with same points = draw
                        if (KingOfTheHill.currentGreenTeamPoints == KingOfTheHill.currentYellowTeamPoints) {
                            KingOfTheHill.triggerDrawWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.TeamHillDraw, false);
                        }
                        // green team more points than yellow team = green team win
                        else if (KingOfTheHill.currentGreenTeamPoints > KingOfTheHill.currentYellowTeamPoints) {
                            KingOfTheHill.triggerGreenTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GreenTeamHillWin, false);
                        }
                        // otherwise yellow team win
                        else {
                            KingOfTheHill.triggerYellowTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.YellowTeamHillWin, false);
                        }
                    }

                    if (KingOfTheHill.totalGreenKingzonescaptured != 0) {
                        KingOfTheHill.currentGreenTeamPoints += KingOfTheHill.totalGreenKingzonescaptured * deltaTime;
                        if (KingOfTheHill.currentGreenTeamPoints >= KingOfTheHill.requiredPoints) {
                            KingOfTheHill.triggerGreenTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GreenTeamHillWin, false);
                        }
                    }
                    if (KingOfTheHill.totalYellowKingzonescaptured != 0) {
                        KingOfTheHill.currentYellowTeamPoints += KingOfTheHill.totalYellowKingzonescaptured * deltaTime;
                        if (KingOfTheHill.currentYellowTeamPoints >= KingOfTheHill.requiredPoints) {
                            KingOfTheHill.triggerYellowTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.YellowTeamHillWin, false);
                        }
                    }

                    KingOfTheHill.kingpointCounter = Language.introTexts[2] + "<color=#00FF00FF>" + KingOfTheHill.currentGreenTeamPoints.ToString("F0") + "</color> - " + "<color=#FFFF00FF>" + KingOfTheHill.currentYellowTeamPoints.ToString("F0") + "</color>";
                    break;
                case 5:
                    // HP timers
                    if (HotPotato.firstPotatoTransfered) {
                        HotPotato.timeforTransfer -= deltaTime;

                        if (HotPotato.timeforTransfer <= 0 && !HotPotato.hotPotatoPlayer.Data.IsDead && AmongUsClient.Instance.AmHost) {
                            // Ensure host send an RPC so the time doesn't bug
                            MessageWriter winWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.HotPotatoExploded, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(winWriter);
                            RPCProcedure.hotPotatoExploded(); 
                        }

                        progressStart += deltaTime;
                        gamemodeMatchDuration -= deltaTime;
                        if (progress != null) {
                            progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" + Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0") + " | <color=#FF8000FF>" + Language.introTexts[5] + "</color>" + HotPotato.timeforTransfer.ToString("F0");
                            progress.GetComponent<ProgressTracker>().curValue = Mathf.Lerp(PlayerInCache.AllPlayers.Count - 1, 0, progressStart / progressEnd);
                        }
                        if (gamemodeMatchDuration <= 0) {
                            HotPotato.triggerHotPotatoEnd = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HotPotatoEnd, false);
                        }
                    }
                    break;
                case 6:
                    // ZL timers
                    progressStart += deltaTime;
                    gamemodeMatchDuration -= deltaTime;
                    if (progress != null) {
                        progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" + Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0");
                        progress.GetComponent<ProgressTracker>().curValue = Mathf.Lerp(PlayerInCache.AllPlayers.Count - 1, 0, progressStart / progressEnd);
                    }
                    if (gamemodeMatchDuration <= 0) {
                        ZombieLaboratory.triggerZombieWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ZombieWin, false);
                    }
                    break;
                case 7:
                    // BR timers
                    progressStart += deltaTime;
                    gamemodeMatchDuration -= deltaTime;
                    if (progress != null) {
                        progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" +Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0");
                        progress.GetComponent<ProgressTracker>().curValue = Mathf.Lerp(PlayerInCache.AllPlayers.Count - 1, 0, progressStart / progressEnd);
                    }
                    if (gamemodeMatchDuration <= 0) {
                        if (BattleRoyale.matchType == 2) {
                            if (BattleRoyale.serialKiller != null) {
                                // all teams with same points = Draw
                                if (BattleRoyale.limePoints == BattleRoyale.pinkPoints && BattleRoyale.pinkPoints == BattleRoyale.serialKillerPoints) {
                                    BattleRoyale.triggerDrawWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleDraw, false);
                                }
                                // Lime team more points than pink team and serial killer = lime team win
                                else if (BattleRoyale.limePoints > BattleRoyale.pinkPoints && BattleRoyale.limePoints > BattleRoyale.serialKillerPoints) {
                                    BattleRoyale.triggerLimeTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                                }
                                // otherwise pink team win
                                else if (BattleRoyale.pinkPoints > BattleRoyale.limePoints && BattleRoyale.pinkPoints > BattleRoyale.serialKillerPoints) {
                                    BattleRoyale.triggerPinkTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                                }
                                // otherwise serial killer win
                                else if (BattleRoyale.serialKillerPoints > BattleRoyale.limePoints && BattleRoyale.serialKillerPoints > BattleRoyale.pinkPoints) {
                                    BattleRoyale.triggerSerialKillerWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleSerialKillerWin, false);
                                }
                                // draw between some of the teams
                                else {
                                    BattleRoyale.triggerDrawWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleDraw, false);
                                }
                            }
                            else {
                                // both teams with same points = Draw
                                if (BattleRoyale.limePoints == BattleRoyale.pinkPoints) {
                                    BattleRoyale.triggerDrawWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleDraw, false);
                                }
                                // Lime team more points than pink team = lime team win
                                else if (BattleRoyale.limePoints > BattleRoyale.pinkPoints) {
                                    BattleRoyale.triggerLimeTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                                }
                                // otherwise pink team win
                                else {
                                    BattleRoyale.triggerPinkTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                                }
                            }
                        }
                        else {
                            BattleRoyale.triggerTimeWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleTimeWin, false);
                        }
                    }
                    break;
                case 8:
                    // MF
                    progressStart += deltaTime;
                    gamemodeMatchDuration -= deltaTime;
                    if (progress != null) {
                        progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" + Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0");
                        progress.GetComponent<ProgressTracker>().curValue = Mathf.Lerp(PlayerInCache.AllPlayers.Count - 1, 0, progressStart / progressEnd);
                    }
                    if (gamemodeMatchDuration <= 0) {
                        if (MonjaFestival.bigMonjaPlayer != null) {
                            // all teams with same points = Draw
                            if (MonjaFestival.greenPoints == MonjaFestival.cyanPoints && MonjaFestival.cyanPoints == MonjaFestival.bigMonjaPoints) {
                                MonjaFestival.triggerDrawWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalDraw, false);
                            }
                            // Green team more points than cyan team and big monja = green team win
                            else if (MonjaFestival.greenPoints > MonjaFestival.cyanPoints && MonjaFestival.greenPoints > MonjaFestival.bigMonjaPoints) {
                                MonjaFestival.triggerGreenTeamWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalGreenWin, false);
                            }
                            // otherwise cyan team win
                            else if (MonjaFestival.cyanPoints > MonjaFestival.greenPoints && MonjaFestival.cyanPoints > MonjaFestival.bigMonjaPoints) {
                                MonjaFestival.triggerCyanTeamWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalCyanWin, false);
                            }
                            // otherwise big monja win
                            else if (MonjaFestival.bigMonjaPoints > MonjaFestival.greenPoints && MonjaFestival.bigMonjaPoints > MonjaFestival.cyanPoints) {
                                MonjaFestival.triggerBigMonjaWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalBigMonjaWin, false);
                            }
                            // draw between some of the teams
                            else {
                                MonjaFestival.triggerDrawWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalDraw, false);
                            }
                        }
                        else {
                            // both teams with same points = Draw
                            if (MonjaFestival.greenPoints == MonjaFestival.cyanPoints) {
                                MonjaFestival.triggerDrawWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalDraw, false);
                            }
                            // Green team more points than cyan team = green team win
                            else if (MonjaFestival.greenPoints > MonjaFestival.cyanPoints) {
                                MonjaFestival.triggerGreenTeamWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalGreenWin, false);
                            }
                            // otherwise cyan team win
                            else {
                                MonjaFestival.triggerCyanTeamWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalCyanWin, false);
                            }
                        }
                    }
                    break;
            }
        }

        static void janitorUpdate() {

            if (Janitor.janitor == null)
                return;

            if (Janitor.dragginBody) {
                DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
                for (int i = 0; i < array.Length; i++) {
                    if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == Janitor.bodyId) {
                        var currentPosition = Janitor.janitor.GetTruePosition();
                        var velocity = Janitor.janitor.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
                        var newPos = ((Vector2)Janitor.janitor.GetTruePosition()) - (velocity / 3) + new Vector2(0.15f, 0.25f) + array[i].myCollider.offset;
                        if (!PhysicsHelpers.AnythingBetween(
                            currentPosition,
                            newPos,
                            Constants.ShipAndObjectsMask,
                            false
                        )) {
                            if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                                array[i].transform.position = newPos;
                                array[i].transform.position += new Vector3(0, 0, -0.5f);
                            }
                            else {
                                array[i].transform.position = newPos;
                            }
                        }
                    }
                }
            }
        }
        static void chameleonUpdate() {

            if (Chameleon.chameleon == null) return;

            Chameleon.chameleonTimer -= Time.deltaTime;

            if (Chameleon.chameleonTimer > 0f) {
                if (Chameleon.chameleon == PlayerInCache.LocalPlayer.PlayerControl) {
                    Helpers.alphaPlayer(true, Chameleon.chameleon.PlayerId);
                }
                else {
                    Helpers.invisiblePlayer(Chameleon.chameleon.PlayerId);
                }
            }

            // Chameleon reset
            if (Chameleon.chameleonTimer <= 0f) {
                Chameleon.resetChameleon();
            }
        }
        static void bountyHunterSuicideIfDisconnect() {
            if (BountyHunter.bountyhunter == null) return;

            if (BountyHunter.usedTarget && BountyHunter.hasToKill.Data.Disconnected && BountyHunter.bountyhunter == PlayerInCache.LocalPlayer.PlayerControl && !BountyHunter.bountyhunter.Data.IsDead) {
                BountyHunter.bountyhunter.MurderPlayer(BountyHunter.bountyhunter, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
            }
        }

        static void yinyangerUpdate() {

            if (Yinyanger.yinyanger == null || Yinyanger.yinyanger.Data.IsDead) {
                return;
            }

            if (Yinyanger.yinyedplayer != null && (Yinyanger.yinyedplayer.Data.Disconnected || Yinyanger.yinyedplayer.Data.IsDead)) {
                // If the yined victim is disconnected or dead reset the yined use so a new target can be selected
                Yinyanger.resetYined();
            }
            if (Yinyanger.yangyedplayer != null && (Yinyanger.yangyedplayer.Data.Disconnected || Yinyanger.yangyedplayer.Data.IsDead)) {
                // If the yanged victim is disconnected or dead reset the yanged use so a new target can be selectet
                Yinyanger.resetYanged();
            }
        }
        
        static void challengerUpdate() {

            if (Challenger.challenger == null || !Challenger.isDueling) {
                return;
            }

            // Set grey painting while dueling
            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                player.setLook("", 6, "", "", "", "");
                if (player.cosmetics.currentPet) player.cosmetics.currentPet.gameObject.SetActive(false);
            }

            // 30 sec duel duration
            Challenger.duelDuration -= Time.deltaTime;
            if (Challenger.duelDuration < 0 && Challenger.onlyOneFinishDuel && !Challenger.timeOutDuel) {
                Challenger.onlyOneFinishDuel = false;
                Challenger.timeOutDuel = true;
                challengerFinishDuel(1);
            }

            while ((!Challenger.challengerRock && !Challenger.challengerPaper && !Challenger.challengerScissors) || (!Challenger.rivalRock && !Challenger.rivalPaper && !Challenger.rivalScissors))
                return;

            if (Challenger.onlyOneFinishDuel && !Challenger.timeOutDuel) {
                Challenger.onlyOneFinishDuel = false;
                challengerFinishDuel(0);
            }
        }
        public static void challengerFinishDuel(byte duelflag) {

            if (Challenger.challengerRock) {
                new RockPaperScissors(3, Challenger.challenger, 1);
            }
            else if (Challenger.challengerPaper) {
                new RockPaperScissors(3, Challenger.challenger, 2);
            }
            else if (Challenger.challengerScissors) {
                new RockPaperScissors(3, Challenger.challenger, 3);
            }

            if (Challenger.rivalRock) {
                new RockPaperScissors(3, Challenger.rivalPlayer, 1);
            }
            else if (Challenger.rivalPaper) {
                new RockPaperScissors(3, Challenger.rivalPlayer, 2);
            }
            else if (Challenger.rivalScissors) {
                new RockPaperScissors(3, Challenger.rivalPlayer, 3);
            }

            if (duelflag == 0) {
                HudManager.Instance.StartCoroutine(Effects.Lerp(3, new Action<float>((p) => {

                    if (p == 1f) {

                        int whoDied = 0;

                        if (Challenger.challengerRock && Challenger.rivalPaper) {
                            Challenger.rivalPlayer.MurderPlayer(Challenger.challenger, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            whoDied = 1;
                        }
                        else if (Challenger.challengerRock && Challenger.rivalScissors) {
                            Challenger.challenger.MurderPlayer(Challenger.rivalPlayer, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            Challenger.duelKills += 1;
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            whoDied = 2;
                        }
                        else if (Challenger.challengerPaper && Challenger.rivalRock) {
                            Challenger.challenger.MurderPlayer(Challenger.rivalPlayer, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            Challenger.duelKills += 1; 
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            whoDied = 2;
                        }
                        else if (Challenger.challengerPaper && Challenger.rivalScissors) {
                            Challenger.rivalPlayer.MurderPlayer(Challenger.challenger, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            whoDied = 1;
                        }
                        else if (Challenger.challengerScissors && Challenger.rivalPaper) {
                            Challenger.challenger.MurderPlayer(Challenger.rivalPlayer, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            Challenger.duelKills += 1; 
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            whoDied = 2;
                        }
                        else if (Challenger.challengerScissors && Challenger.rivalRock) {
                            Challenger.rivalPlayer.MurderPlayer(Challenger.challenger, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            whoDied = 1;
                        }

                        switch (whoDied) {
                            case 1:
                                var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.challenger.PlayerId);
                                body.transform.position = new Vector3(75f, 0f, -5);
                                break;
                            case 2:
                                var bodytwo = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.rivalPlayer.PlayerId);
                                bodytwo.transform.position = new Vector3(75f, 0f, -5);
                                break;
                        }
                    }
                })));
            }
            else {
                HudManager.Instance.StartCoroutine(Effects.Lerp(3, new Action<float>((p) => {

                    if (p == 1f) {

                        int whoDied = 0;

                        if ((Challenger.challengerRock || Challenger.challengerPaper || Challenger.challengerScissors) && (!Challenger.rivalRock && !Challenger.rivalPaper && !Challenger.rivalScissors)) {
                            Challenger.challenger.MurderPlayer(Challenger.rivalPlayer, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            Challenger.duelKills += 1; 
                            whoDied = 2;
                        }
                        else if ((!Challenger.challengerRock && !Challenger.challengerPaper && !Challenger.challengerScissors) && (Challenger.rivalRock || Challenger.rivalPaper || Challenger.rivalScissors)) {
                            Challenger.rivalPlayer.MurderPlayer(Challenger.challenger, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            whoDied = 1;
                        }
                        else if ((!Challenger.challengerRock && !Challenger.challengerPaper && !Challenger.challengerScissors) && (!Challenger.rivalRock || !Challenger.rivalPaper || !Challenger.rivalScissors)) {
                            Challenger.challenger.MurderPlayer(Challenger.rivalPlayer, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            Challenger.rivalPlayer.MurderPlayer(Challenger.challenger, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelKillClip, false, 5f);
                            whoDied = 3;
                        }

                        switch (whoDied) {
                            case 1:
                                var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.challenger.PlayerId);
                                body.transform.position = new Vector3(75f, 0f, -5);
                                break;
                            case 2:
                                var bodytwo = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.rivalPlayer.PlayerId);
                                bodytwo.transform.position = new Vector3(75f, 0f, -5);
                                break;
                            case 3:
                                var bodythree = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.rivalPlayer.PlayerId);
                                bodythree.transform.position = new Vector3(75f, 0f, -5);
                                var bodyfour = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.challenger.PlayerId);
                                bodyfour.transform.position = new Vector3(75f, 0f, -5);
                                break;
                        }
                    }
                })));
            }

            HudManager.Instance.StartCoroutine(Effects.Lerp(6, new Action<float>((p) => {

                if (p == 1f) {
                    // Undo the character transform
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        if (player == PlayerInCache.LocalPlayer.PlayerControl) {
                            player.transform.position = positionBeforeDuel;
                        }
                    }
                    RPCProcedure.changeMusic(2);
                    Challenger.timeOutDuel = false;
                }
            })));

            HudManager.Instance.StartCoroutine(Effects.Lerp(7, new Action<float>((p) => {

                if (p == 1f) {

                    // If after the duel both are dead, teleport their body to the emergency button
                    if (Challenger.challenger.Data.IsDead && Challenger.rivalPlayer.Data.IsDead) {
                        var bodyChallenger = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.challenger.PlayerId);
                        challengerTeleportBodies(bodyChallenger);                        
                        var bodyRival = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.rivalPlayer.PlayerId);
                        challengerTeleportBodies(bodyRival);                        
                        // If after the duel one of them was a lover, teleport the other lover body too
                        if (Modifiers.lover1 != null && (Challenger.rivalPlayer.PlayerId == Modifiers.lover1.PlayerId || Challenger.challenger.PlayerId == Modifiers.lover1.PlayerId)) {
                            var bodyLover2 = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Modifiers.lover2.PlayerId);
                            challengerTeleportBodies(bodyLover2);                            
                        }
                        else if (Modifiers.lover2 != null && (Challenger.rivalPlayer.PlayerId == Modifiers.lover2.PlayerId || Challenger.challenger.PlayerId == Modifiers.lover2.PlayerId)) {
                            var bodyLover1 = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Modifiers.lover1.PlayerId);
                            challengerTeleportBodies(bodyLover1);                            
                        }
                    }
                    // If after the duel the challenger is dead, teleport his body to the player location
                    else if (Challenger.challenger.Data.IsDead) {
                        var bodyC = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.challenger.PlayerId);
                        challengerTeleportBodies(bodyC);                        
                        // If after the duel one of them was a lover, teleport the other lover body too
                        if (Modifiers.lover1 != null && Challenger.challenger.PlayerId == Modifiers.lover1.PlayerId) {
                            var bodyLover2 = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Modifiers.lover2.PlayerId);
                            challengerTeleportBodies(bodyLover2);                            
                        }
                        else if (Modifiers.lover2 != null && Challenger.challenger.PlayerId == Modifiers.lover2.PlayerId) {
                            var bodyLover1 = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Modifiers.lover1.PlayerId);
                            challengerTeleportBodies(bodyLover1);                            
                        }
                    }
                    // If after the duel the rival is dead, teleport his body to the player location
                    else if (Challenger.rivalPlayer.Data.IsDead) {
                        var bodyR = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Challenger.rivalPlayer.PlayerId);
                        challengerTeleportBodies(bodyR);                        
                        // If after the duel one of them was a lover, teleport the other lover body too
                        if (Modifiers.lover1 != null && Challenger.rivalPlayer.PlayerId == Modifiers.lover1.PlayerId) {
                            var bodyLover2 = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Modifiers.lover2.PlayerId);
                            challengerTeleportBodies(bodyLover2);                            
                        }
                        else if (Modifiers.lover2 != null && Challenger.rivalPlayer.PlayerId == Modifiers.lover2.PlayerId) {
                            var bodyLover1 = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Modifiers.lover1.PlayerId);
                            challengerTeleportBodies(bodyLover1);                            
                        }
                    }
                }
            })));

            HudManager.Instance.StartCoroutine(Effects.Lerp(8, new Action<float>((p) => {
                if (p == 1f) {
                    // Reset painting after dueling
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        if (player == null) continue;
                        player.setDefaultLook();
                        if (player.cosmetics.currentPet) player.cosmetics.currentPet.gameObject.SetActive(true);
                    }

                    timeTravelerRewindTimeButton.Timer = timeTravelerRewindTimeButton.MaxTimer;
                    timeTravelerShieldButton.Timer = timeTravelerShieldButton.MaxTimer;

                    Challenger.challengerDuelButtonText.text = $"{Challenger.duelKills} / {Challenger.neededKills}";
                    if (Challenger.duelKills >= Challenger.neededKills) {
                        Challenger.triggerChallengerWin = true;
                    }
                    // Reset challenger values after dueling
                    Challenger.ResetValues();
                }
            })));
        }

        static void challengerTeleportBodies(DeadBody body) {

            GameObject emerButton = null;

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    emerButton = GameObject.Find("EmergencyConsole");
                    body.transform.position = emerButton.transform.position + new Vector3(2.02f, 0f, -0.5f); 
                    break;
                case 1:
                    emerButton = GameObject.Find("EmergencyConsole");
                    body.transform.position = emerButton.transform.position + new Vector3(1.5f, 0f, -0.5f);
                    break;
                case 2:
                    emerButton = GameObject.Find("EmergencyButton");
                    body.transform.position = emerButton.transform.position + new Vector3(2.4f, 0f, -0.5f);
                    break;
                case 3:
                    emerButton = GameObject.Find("EmergencyConsole");
                    body.transform.position = emerButton.transform.position + new Vector3(-2.02f, 0f, -0.5f); 
                    break;
                case 4:
                    emerButton = GameObject.Find("task_emergency");
                    body.transform.position = emerButton.transform.position + new Vector3(-2.875f, 0f, -0.5f);
                    break;
                case 5:
                    emerButton = GameObject.Find("ConchEmergencyButton");
                    body.transform.position = emerButton.transform.position + new Vector3(1.5f, 0f, -0.5f);
                    break;
                case 6:
                    //emerButton = GameObject.Find("console-mr-callmeeting");
                    if (body.transform.position.y > 0) {
                        body.transform.position = new Vector3(5f, 19.5f, -5);
                    }
                    else {
                        body.transform.position = new Vector3(1.35f, -28.25f, -5);
                    }
                    break;
            }
        }

        static void yandereUpdate() {

            if (Yandere.yandere == null) return;

            // Yandere rampage mode if target disconnects
            if (Yandere.yandere != null && Yandere.target != null && Yandere.target.Data.Disconnected && !Yandere.rampageMode) {
                Yandere.rampageMode = true;
                Yandere.yandereTargetButtonText.text = Language.statusRolesTexts[2];
                Yandere.yandereKillButtonText.text = Language.statusRolesTexts[3];
                if (PlayerInCache.LocalPlayer.PlayerControl == Yandere.yandere) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.hunterTarget, false, 100f);
                }
            }
        }

        static void strandedUpdate() {

            if (Stranded.stranded == null) return;

            if (Stranded.isInvisible) {
                Stranded.invisibleTimer -= Time.deltaTime;

                if (Stranded.invisibleTimer > 0f) {
                    if (Stranded.stranded == PlayerInCache.LocalPlayer.PlayerControl) {
                        Helpers.alphaPlayer(true, Stranded.stranded.PlayerId);
                    }
                    else {
                        Helpers.invisiblePlayer(Stranded.stranded.PlayerId);
                    }
                }

                // Stranded reset
                if (Stranded.invisibleTimer <= 0f) {
                    Stranded.resetStranded();
                }
            }
        }

        static void exilerWinIfDisconnect() {
            if (Exiler.exiler == null) return;

            if (Exiler.usedTarget && Exiler.target.Data.Disconnected && Exiler.exiler == PlayerInCache.LocalPlayer.PlayerControl && !Exiler.exiler.Data.IsDead) {

                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ExilerTriggerWin, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.exilerWin();
            }
        }

        static void seekerUpdate() {

            if (Seeker.seeker == null || !Seeker.isMinigaming) {
                return;
            }

            // Set grey painting while dueling
            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                player.setLook("", 6, "", "", "", "");
                if (player.cosmetics.currentPet) player.cosmetics.currentPet.gameObject.SetActive(false);
            }

            // 20 sec duel duration
            Seeker.minigameDuration -= Time.deltaTime;
            if (Seeker.minigameDuration < 0 && Seeker.onlyOneFinishMinigame && !Seeker.timeOutMinigame) {
                Seeker.onlyOneFinishMinigame = false;
                Seeker.timeOutMinigame = true;
                seekerFinishMinigame(1);
            }

            while (Seeker.howmanyselectedattacks < Seeker.currentPlayers + 1)
                return;

            if (Seeker.onlyOneFinishMinigame && !Seeker.timeOutMinigame) {
                Seeker.onlyOneFinishMinigame = false;
                seekerFinishMinigame(0);
            }
        }
        public static void seekerFinishMinigame(byte timeOut) {

            switch (Seeker.seekerSelectedHiding) {
                case 1:
                    new MonjaCuloDio(3, Seeker.seeker, 1);
                    break;
                case 2:
                    new MonjaCuloDio(3, Seeker.seeker, 2);
                    break;
                case 3:
                    new MonjaCuloDio(3, Seeker.seeker, 3);
                    break;
            }

            if (timeOut == 0) {
                HudManager.Instance.StartCoroutine(Effects.Lerp(3, new Action<float>((p) => {

                    if (p == 1f) {

                        switch (Seeker.seekerSelectedHiding) {
                            case 1:
                                Seeker.minigameArenaHideOnePointOne.transform.parent.gameObject.SetActive(false);
                                break;
                            case 2:
                                Seeker.minigameArenaHideTwoPointOne.transform.parent.gameObject.SetActive(false);
                                break;
                            case 3:
                                Seeker.minigameArenaHideThreePointOne.transform.parent.gameObject.SetActive(false);
                                break;
                        }

                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            switch (Seeker.seekerSelectedHiding) {
                                case 1:
                                    Seeker.lowerminigameArenaHideOnePointOne.transform.parent.gameObject.SetActive(false);
                                    break;
                                case 2:
                                    Seeker.lowerminigameArenaHideTwoPointOne.transform.parent.gameObject.SetActive(false);
                                    break;
                                case 3:
                                    Seeker.lowerminigameArenaHideThreePointOne.transform.parent.gameObject.SetActive(false);
                                    break;
                            }
                        }
                        
                        if (Seeker.hidedPlayerOne != null && Seeker.seekerSelectedHiding == Seeker.hidedPlayerOneSelectedHiding) {
                            Seeker.currentPoints += 1;
                        }
                        if (Seeker.hidedPlayerTwo != null && Seeker.seekerSelectedHiding == Seeker.hidedPlayerTwoSelectedHiding) {
                            Seeker.currentPoints += 1;
                        }
                        if (Seeker.hidedPlayerThree != null && Seeker.seekerSelectedHiding == Seeker.hidedPlayerThreeSelectedHiding) {
                            Seeker.currentPoints += 1;
                        }
                        Seeker.seekerPlayerPointsCount.text = $"{Seeker.currentPlayers} / 3";
                        Seeker.seekerPerformMinigamePlayerPointsCount.text = $"{Seeker.currentPoints} / {Seeker.neededPoints}"; 
                        new CustomMessage(Language.statusRolesTexts[4] + Seeker.currentPoints, 3, new Vector2(0f, 1.6f), 8);
                    }
                })));
            }
            else {
                HudManager.Instance.StartCoroutine(Effects.Lerp(3, new Action<float>((p) => {

                    if (p == 1f) {

                        if (Seeker.seekerSelectedHiding != 0) {

                            switch (Seeker.seekerSelectedHiding) {
                                case 1:
                                    Seeker.minigameArenaHideOnePointOne.transform.parent.gameObject.SetActive(false);
                                    break;
                                case 2:
                                    Seeker.minigameArenaHideTwoPointOne.transform.parent.gameObject.SetActive(false);
                                    break;
                                case 3:
                                    Seeker.minigameArenaHideThreePointOne.transform.parent.gameObject.SetActive(false);
                                    break;
                            }

                            if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                                switch (Seeker.seekerSelectedHiding) {
                                    case 1:
                                        Seeker.lowerminigameArenaHideOnePointOne.transform.parent.gameObject.SetActive(false);
                                        break;
                                    case 2:
                                        Seeker.lowerminigameArenaHideTwoPointOne.transform.parent.gameObject.SetActive(false);
                                        break;
                                    case 3:
                                        Seeker.lowerminigameArenaHideThreePointOne.transform.parent.gameObject.SetActive(false);
                                        break;
                                }
                            }
                            
                            if (Seeker.hidedPlayerOne != null && Seeker.seekerSelectedHiding == Seeker.hidedPlayerOneSelectedHiding) {
                                Seeker.currentPoints += 1;
                            }
                            if (Seeker.hidedPlayerTwo != null && Seeker.seekerSelectedHiding == Seeker.hidedPlayerTwoSelectedHiding) {
                                Seeker.currentPoints += 1;
                            }
                            if (Seeker.hidedPlayerThree != null && Seeker.seekerSelectedHiding == Seeker.hidedPlayerThreeSelectedHiding) {
                                Seeker.currentPoints += 1;
                            }
                            if (Seeker.hidedPlayerOne != null && Seeker.hidedPlayerOneSelectedHiding == 0) {
                                Seeker.currentPoints += 1;
                            }
                            if (Seeker.hidedPlayerTwo != null && Seeker.hidedPlayerTwoSelectedHiding == 0) {
                                Seeker.currentPoints += 1;
                            }
                            if (Seeker.hidedPlayerThree != null && Seeker.hidedPlayerThreeSelectedHiding == 0) {
                                Seeker.currentPoints += 1;
                            }
                        }
                        Seeker.seekerPlayerPointsCount.text = $"{Seeker.currentPlayers} / 3";
                        Seeker.seekerPerformMinigamePlayerPointsCount.text = $"{Seeker.currentPoints} / {Seeker.neededPoints}"; 
                        new CustomMessage(Language.statusRolesTexts[4] + Seeker.currentPoints, 3, new Vector2(0f, 1.6f), 8);
                    }
                })));
            }

            HudManager.Instance.StartCoroutine(Effects.Lerp(6, new Action<float>((p) => {

                if (p == 1f) {
                    // Undo the character transform
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        if (player == PlayerInCache.LocalPlayer.PlayerControl) {
                            player.transform.position = positionBeforeMinigame;
                        }
                    }
                    RPCProcedure.changeMusic(2);
                    Seeker.timeOutMinigame = false;
                    if (Seeker.hidedPlayerOne != null) {
                        Seeker.hidedPlayerOne.moveable = true;
                    }
                    if (Seeker.hidedPlayerTwo != null) {
                        Seeker.hidedPlayerTwo.moveable = true;
                    }
                    if (Seeker.hidedPlayerThree != null) {
                        Seeker.hidedPlayerThree.moveable = true;
                    }
                }
            })));

            HudManager.Instance.StartCoroutine(Effects.Lerp(7, new Action<float>((p) => {
                if (p == 1f) {
                    // Reset painting after dueling
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        if (player == null) continue;
                        player.setDefaultLook();
                        if (player.cosmetics.currentPet) player.cosmetics.currentPet.gameObject.SetActive(true);
                    }

                    timeTravelerRewindTimeButton.Timer = timeTravelerRewindTimeButton.MaxTimer;
                    timeTravelerShieldButton.Timer = timeTravelerShieldButton.MaxTimer;
                    seekerMinigameButton.Timer = 15f;

                    switch (Seeker.seekerSelectedHiding) {
                        case 1:
                            Seeker.minigameArenaHideOnePointOne.transform.parent.gameObject.SetActive(true);
                            break;
                        case 2:
                            Seeker.minigameArenaHideTwoPointOne.transform.parent.gameObject.SetActive(true);
                            break;
                        case 3:
                            Seeker.minigameArenaHideThreePointOne.transform.parent.gameObject.SetActive(true);
                            break;
                    }

                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                        switch (Seeker.seekerSelectedHiding) {
                            case 1:
                                Seeker.lowerminigameArenaHideOnePointOne.transform.parent.gameObject.SetActive(true);
                                break;
                            case 2:
                                Seeker.lowerminigameArenaHideTwoPointOne.transform.parent.gameObject.SetActive(true);
                                break;
                            case 3:
                                Seeker.lowerminigameArenaHideThreePointOne.transform.parent.gameObject.SetActive(true);
                                break;
                        }
                    }

                    // Reset Seeker values after dueling
                    Seeker.ResetValues(true);

                    if (Seeker.currentPoints >= Seeker.neededPoints) {
                        Seeker.triggerSeekerWin = true;
                    }
                }
            })));
        }

        static void fortuneTellerUpdate() {
            if (FortuneTeller.fortuneTeller == null || FortuneTeller.fortuneTeller != PlayerInCache.LocalPlayer.PlayerControl) return;

            // Update revealed players names if not in the duel
            if (!Challenger.isDueling && !Seeker.isMinigaming) {
                foreach (PlayerControl p in FortuneTeller.revealedPlayers) {
                    // Update color and name regarding settings and given info
                    string result = p.Data.PlayerName;
                    RoleFortuneTellerInfo si = RoleFortuneTellerInfo.getFortuneTellerRoleInfoForPlayer(p);
                    if (FortuneTeller.kindOfInfo == 0)
                        si.color = si.isGood ? new Color(141f / 255f, 255f / 255f, 255f / 255f, 1) : new Color(255f / 255f, 0f / 255f, 0f / 255f, 1);
                    else if (FortuneTeller.kindOfInfo == 1) {
                        result = p.Data.PlayerName + " (" + si.name + ")";
                    }

                    // Set color and name
                    p.cosmetics.nameText.color = si.color;
                    p.cosmetics.nameText.text = result;
                    if (MeetingHud.Instance != null) {
                        foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates) {
                            if (p.PlayerId == player.TargetPlayerId) {
                                player.NameText.text = result;
                                player.NameText.color = si.color;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public static void kidUpdate() {
            foreach (PlayerControl p in PlayerInCache.AllPlayers) {
                if (p == null) continue;

                if (Kid.kid != null && Kid.kid == p)
                    p.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
                else if (Mimic.mimic != null && Mimic.mimic == p && Mimic.transformTarget != null && Mimic.transformTarget == Kid.kid && Mimic.transformTimer > 0f)
                    p.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
                else if (Puppeteer.puppeteer != null && Puppeteer.puppeteer == p && Puppeteer.transformTarget != null && Puppeteer.transformTarget == Kid.kid && Puppeteer.morphed)
                    p.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
                // big chungus update, restore original scale on duel and painting to be more fair
                else if (Modifiers.bigchungus != null && Modifiers.bigchungus == p && !Challenger.isDueling && !Seeker.isMinigaming && Painter.painterTimer <= 0 && !isHappeningAnonymousComms && !Helpers.MushroomSabotageActive()) {
                    if (Mimic.mimic != null && Mimic.transformTimer > 0f && Mimic.mimic.PlayerId == Modifiers.bigchungus.PlayerId) {
                        p.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
                    }
                    else if (Puppeteer.puppeteer != null && Puppeteer.morphed && Puppeteer.puppeteer.PlayerId == Modifiers.bigchungus.PlayerId) {
                        p.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
                    }
                    else {
                        p.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
                    }
                }
                // Mimic and Puppeteer big chungus update
                else if (Mimic.mimic != null && Mimic.mimic == p && Mimic.transformTarget != null && Mimic.transformTarget == Modifiers.bigchungus && Mimic.transformTimer > 0f && !isHappeningAnonymousComms && !Helpers.MushroomSabotageActive())
                    p.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
                else if (Puppeteer.puppeteer != null && Puppeteer.puppeteer == p && Puppeteer.transformTarget != null && Puppeteer.transformTarget == Modifiers.bigchungus && Puppeteer.morphed && !isHappeningAnonymousComms && !Helpers.MushroomSabotageActive())
                    p.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
                else
                    p.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            }
        }      
        static void spiritualistUpdate() {

            if (PlayerInCache.LocalPlayer.PlayerControl == Spiritualist.spiritualist && Spiritualist.spiritualist != null) {
                foreach (var player in PlayerInCache.AllPlayers) {
                    if (player.Data.IsDead) {
                        player.PlayerControl.cosmetics.currentBodySprite.BodySprite.gameObject.SetActive(true);
                        player.PlayerControl.cosmetics.currentBodySprite.BodySprite.enabled = true;
                        player.PlayerControl.cosmetics.nameText.enabled = true;
                        player.PlayerControl.cosmetics.nameText.gameObject.SetActive(true);
                    }
                }
            }

            // Identify Spiritualist by name color if you're dead
            foreach (PlayerControl p in PlayerInCache.AllPlayers) {
                if (Spiritualist.spiritualist != null && !Spiritualist.spiritualist.Data.IsDead && p == PlayerInCache.LocalPlayer.PlayerControl && p.Data.IsDead) {
                    Spiritualist.spiritualist.cosmetics.nameText.color = Spiritualist.color;
                }
            }
        }
        static void vigilantMiraUpdate() {

            if (Vigilant.vigilantMira == null || Vigilant.vigilantMira.Data.IsDead || Vigilant.vigilantMira != PlayerInCache.LocalPlayer.PlayerControl || GameOptionsManager.Instance.currentGameOptions.MapId != 1) {
                return;
            }

            // Vigilant activate/deactivate doorlog item with Q
            if (Input.GetKeyDown(KeyCode.Q)) {
                Vigilant.doorLogActivated = !Vigilant.doorLogActivated;
                Vigilant.doorLog.SetActive(Vigilant.doorLogActivated);
            }
        }
        static void batUpdate() {
            if (Bat.bat == null)
                return;

            if (Bat.frequencyTimer > 0 && Bat.bat != PlayerInCache.LocalPlayer.PlayerControl) {
                if (!Bat.bat.Data.IsDead && Vector2.Distance(Bat.bat.transform.position, PlayerInCache.LocalPlayer.PlayerControl.transform.position) < (1f * Bat.frequencyRange)) {

                    PlayerInCache.LocalPlayer.PlayerControl.killTimer += Time.fixedDeltaTime;

                    foreach (CustomButton button in CustomButton.buttons) {
                        if (button.isEffectActive) continue;

                        if (!PlayerInCache.LocalPlayer.Data.Role.IsImpostor) {
                            if (button.Timer > 1f)
                                button.Timer -= Time.fixedDeltaTime * 0.5f;
                        }
                        else {
                            if (button.MaxTimer > 0f)
                                if (button.Timer > 1f)
                                    button.Timer += Time.fixedDeltaTime;
                        }
                    }
                }
            }
        }
        static void necromancerUpdate() {

            if (Necromancer.necromancer == null)
                return;

            if (Necromancer.dragginBody) {
                DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
                for (int i = 0; i < array.Length; i++) {
                    if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == Necromancer.bodyId) {
                        var currentPosition = Necromancer.necromancer.GetTruePosition();
                        var velocity = Necromancer.necromancer.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
                        var newPos = ((Vector2)Necromancer.necromancer.GetTruePosition()) - (velocity / 3) + new Vector2(0.15f, 0.25f) + array[i].myCollider.offset;
                        if (!PhysicsHelpers.AnythingBetween(
                            currentPosition,
                            newPos,
                            Constants.ShipAndObjectsMask,
                            false
                        )) {
                            if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                                array[i].transform.position = newPos;
                                array[i].transform.position += new Vector3(0, 0, -0.5f);
                            }
                            else {
                                array[i].transform.position = newPos;
                            }
                        }
                    }
                }
            }
        }
        static void captureTheFlagUpdate() {

            if (gameType != 2)
                return;

            if (CaptureTheFlag.redPlayerWhoHasBlueFlag != null && CaptureTheFlag.redPlayerWhoHasBlueFlag.Data.Disconnected) {
                CaptureTheFlag.blueflag.transform.parent = CaptureTheFlag.blueflagbase.transform.parent;
                switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                    // Skeld
                    case 0:
                        if (activatedSensei) {
                            CaptureTheFlag.blueflag.transform.position = new Vector3(7.7f, -1.15f, 0.5f);
                        }
                        else {
                            CaptureTheFlag.blueflag.transform.position = new Vector3(16.5f, -4.65f, 0.5f);
                        }
                        break;
                    // MiraHQ
                    case 1:
                        CaptureTheFlag.blueflag.transform.position = new Vector3(23.25f, 5.05f, 0.5f);
                        break;
                    // Polus
                    case 2:
                        CaptureTheFlag.blueflag.transform.position = new Vector3(5.4f, -9.65f, 0.5f);
                        break;
                    // Dleks
                    case 3:
                        CaptureTheFlag.blueflag.transform.position = new Vector3(-16.5f, -4.65f, 0.5f);
                        break;
                    // Airship
                    case 4:
                        CaptureTheFlag.blueflag.transform.position = new Vector3(33.6f, 1.25f, 0.5f);
                        break;
                    // Fungle
                    case 5:
                        CaptureTheFlag.blueflag.transform.position = new Vector3(19.25f, 2.15f, 0.5f);
                        break;
                    // Submerged
                    case 6:
                        CaptureTheFlag.blueflag.transform.position = new Vector3(12.5f, -31.45f, -0.011f);
                        break;
                }
                CaptureTheFlag.blueflagtaken = false;
                CaptureTheFlag.redPlayerWhoHasBlueFlag = null;
            }

            if (CaptureTheFlag.bluePlayerWhoHasRedFlag != null && CaptureTheFlag.bluePlayerWhoHasRedFlag.Data.Disconnected) {
                CaptureTheFlag.redflag.transform.parent = CaptureTheFlag.redflagbase.transform.parent;
                switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                    // Skeld
                    case 0:
                        if (activatedSensei) {
                            CaptureTheFlag.redflag.transform.position = new Vector3(-17.5f, -1.35f, 0.5f);
                        }
                        else {
                            CaptureTheFlag.redflag.transform.position = new Vector3(-20.5f, -5.35f, 0.5f);
                        }
                        break;
                    // MiraHQ
                    case 1:
                        CaptureTheFlag.redflag.transform.position = new Vector3(2.525f, 10.55f, 0.5f);
                        break;
                    // Polus
                    case 2:
                        CaptureTheFlag.redflag.transform.position = new Vector3(36.4f, -21.7f, 0.5f);
                        break;
                    // Dleks
                    case 3:
                        CaptureTheFlag.redflag.transform.position = new Vector3(20.5f, -5.35f, 0.5f);
                        break;
                    // Airship
                    case 4:
                        CaptureTheFlag.redflag.transform.position = new Vector3(-17.5f, -1.2f, 0.5f);
                        break;
                    // Fungle
                    case 5:
                        CaptureTheFlag.redflag.transform.position = new Vector3(-23f, -0.65f, 0.5f);
                        break;
                    // Submerged
                    case 6:
                        CaptureTheFlag.redflag.transform.position = new Vector3(-8.35f, 28.05f, 0.03f);
                        break;
                }
                CaptureTheFlag.redflagtaken = false;
                CaptureTheFlag.bluePlayerWhoHasRedFlag = null;
            }
        }
        static void policeandthiefUpdate() {

            if (gameType != 3)
                return;

            // Check number of thiefs if a thief disconnects
            foreach (PlayerControl thief in PoliceAndThief.thiefTeam) {
                if (thief.Data.Disconnected) {

                    if (PoliceAndThief.thiefplayer01 != null && thief.PlayerId == PoliceAndThief.thiefplayer01.PlayerId && PoliceAndThief.thiefplayer01IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer01);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer01JewelId);
                    }
                    else if (PoliceAndThief.thiefplayer02 != null && thief.PlayerId == PoliceAndThief.thiefplayer02.PlayerId && PoliceAndThief.thiefplayer02IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer02);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer02JewelId);
                    }
                    else if (PoliceAndThief.thiefplayer03 != null && thief.PlayerId == PoliceAndThief.thiefplayer03.PlayerId && PoliceAndThief.thiefplayer03IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer03);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer03JewelId);
                    }
                    else if (PoliceAndThief.thiefplayer04 != null && thief.PlayerId == PoliceAndThief.thiefplayer04.PlayerId && PoliceAndThief.thiefplayer04IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer04);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer04JewelId);
                    }
                    else if (PoliceAndThief.thiefplayer05 != null && thief.PlayerId == PoliceAndThief.thiefplayer05.PlayerId && PoliceAndThief.thiefplayer05IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer05);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer05JewelId);
                    }
                    else if (PoliceAndThief.thiefplayer06 != null && thief.PlayerId == PoliceAndThief.thiefplayer06.PlayerId && PoliceAndThief.thiefplayer06IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer06);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer06JewelId);
                    }
                    else if (PoliceAndThief.thiefplayer07 != null && thief.PlayerId == PoliceAndThief.thiefplayer07.PlayerId && PoliceAndThief.thiefplayer07IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer07);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer07JewelId);
                    }
                    else if (PoliceAndThief.thiefplayer08 != null && thief.PlayerId == PoliceAndThief.thiefplayer08.PlayerId && PoliceAndThief.thiefplayer08IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer08);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer08JewelId);
                    }
                    else if (PoliceAndThief.thiefplayer09 != null && thief.PlayerId == PoliceAndThief.thiefplayer09.PlayerId && PoliceAndThief.thiefplayer09IsStealing) {
                        PoliceAndThief.thiefTeam.Remove(PoliceAndThief.thiefplayer09);
                        RPCProcedure.policeandThiefRevertedJewelPosition(thief.PlayerId, PoliceAndThief.thiefplayer09JewelId);
                    }

                    PoliceAndThief.thiefpointCounter = Language.introTexts[3] + "<color=#00F7FFFF>" + PoliceAndThief.currentJewelsStoled + " / " + PoliceAndThief.requiredJewels + "</color> | " + Language.introTexts[4] + "<color=#928B55FF>" + PoliceAndThief.currentThiefsCaptured + " / " + PoliceAndThief.thiefTeam.Count + "</color>";
                    if (PoliceAndThief.currentThiefsCaptured == PoliceAndThief.thiefTeam.Count) {
                        PoliceAndThief.triggerPoliceWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModePoliceWin, false);
                    }
                    break;
                }
            }

            foreach (PlayerControl police in PoliceAndThief.policeTeam) {
                if (police.Data.Disconnected) {
                    if (PoliceAndThief.policeplayer01 != null && police.PlayerId == PoliceAndThief.policeplayer01.PlayerId) {
                        PoliceAndThief.policeTeam.Remove(PoliceAndThief.policeplayer01);
                    }
                    else if (PoliceAndThief.policeplayer02 != null && police.PlayerId == PoliceAndThief.policeplayer02.PlayerId) {
                        PoliceAndThief.policeTeam.Remove(PoliceAndThief.policeplayer02);
                    }
                    else if (PoliceAndThief.policeplayer03 != null && police.PlayerId == PoliceAndThief.policeplayer03.PlayerId) {
                        PoliceAndThief.policeTeam.Remove(PoliceAndThief.policeplayer03);
                    }
                    else if (PoliceAndThief.policeplayer04 != null && police.PlayerId == PoliceAndThief.policeplayer04.PlayerId) {
                        PoliceAndThief.policeTeam.Remove(PoliceAndThief.policeplayer04);
                    }
                    else if (PoliceAndThief.policeplayer05 != null && police.PlayerId == PoliceAndThief.policeplayer05.PlayerId) {
                        PoliceAndThief.policeTeam.Remove(PoliceAndThief.policeplayer05);
                    }
                    else if (PoliceAndThief.policeplayer06 != null && police.PlayerId == PoliceAndThief.policeplayer06.PlayerId) {
                        PoliceAndThief.policeTeam.Remove(PoliceAndThief.policeplayer06);
                    }

                    if (PoliceAndThief.policeTeam.Count <= 0) {
                        PoliceAndThief.triggerThiefWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModeThiefWin, false);
                    }
                    break;
                }
            }
        }
        static void kingOfTheHillUpdate() {

            if (gameType != 4)
                return;

            // If king disconnects, assing new king
            if (KingOfTheHill.greenKingplayer != null && KingOfTheHill.greenKingplayer.Data.Disconnected) {
                KingOfTheHill.greenTeam.Remove(KingOfTheHill.greenKingplayer);
                KingOfTheHill.greenKingplayer = null;
                KingOfTheHill.greenKingplayer = KingOfTheHill.greenTeam[0];
                if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                    KingOfTheHill.greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, -0.5f);
                }
                else {
                    KingOfTheHill.greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                }
                KingOfTheHill.greenkingaura.transform.parent = KingOfTheHill.greenKingplayer.transform;
                if (PlayerInCache.LocalPlayer.PlayerControl == KingOfTheHill.greenKingplayer) {
                    Helpers.showGamemodesPopUp(3, Helpers.playerById(KingOfTheHill.greenKingplayer.PlayerId));
                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                }
                KingOfTheHill.greenKingIsReviving = false;

                // Remove minion player from new king
                if (KingOfTheHill.greenplayer01 != null && KingOfTheHill.greenTeam[0] == KingOfTheHill.greenplayer01) {
                    KingOfTheHill.greenplayer01 = null;
                }
                else if (KingOfTheHill.greenplayer02 != null && KingOfTheHill.greenTeam[0] == KingOfTheHill.greenplayer02) {
                    KingOfTheHill.greenplayer02 = null;
                }
                else if (KingOfTheHill.greenplayer03 != null && KingOfTheHill.greenTeam[0] == KingOfTheHill.greenplayer03) {
                    KingOfTheHill.greenplayer03 = null;
                }
                else if (KingOfTheHill.greenplayer04 != null && KingOfTheHill.greenTeam[0] == KingOfTheHill.greenplayer04) {
                    KingOfTheHill.greenplayer04 = null;
                }
                else if (KingOfTheHill.greenplayer05 != null && KingOfTheHill.greenTeam[0] == KingOfTheHill.greenplayer05) {
                    KingOfTheHill.greenplayer05 = null;
                }
                else if (KingOfTheHill.greenplayer06 != null && KingOfTheHill.greenTeam[0] == KingOfTheHill.greenplayer06) {
                    KingOfTheHill.greenplayer06 = null;
                }

                KingOfTheHill.greenTeam.RemoveAt(0);
                KingOfTheHill.greenTeam.Add(KingOfTheHill.greenKingplayer);
                return;
            }

            if (KingOfTheHill.yellowKingplayer != null && KingOfTheHill.yellowKingplayer.Data.Disconnected) {
                KingOfTheHill.yellowTeam.Remove(KingOfTheHill.yellowKingplayer);
                KingOfTheHill.yellowKingplayer = null;
                KingOfTheHill.yellowKingplayer = KingOfTheHill.yellowTeam[0];
                if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                    KingOfTheHill.yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, -0.5f);
                }
                else {
                    KingOfTheHill.yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                }
                KingOfTheHill.yellowkingaura.transform.parent = KingOfTheHill.yellowKingplayer.transform;
                if (PlayerInCache.LocalPlayer.PlayerControl == KingOfTheHill.yellowKingplayer) {
                    Helpers.showGamemodesPopUp(4, Helpers.playerById(KingOfTheHill.yellowKingplayer.PlayerId));
                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                }
                KingOfTheHill.yellowKingIsReviving = false;

                // Remove minion player from new king
                if (KingOfTheHill.yellowplayer01 != null && KingOfTheHill.yellowTeam[0] == KingOfTheHill.yellowplayer01) {
                    KingOfTheHill.yellowplayer01 = null;
                }
                else if (KingOfTheHill.yellowplayer02 != null && KingOfTheHill.yellowTeam[0] == KingOfTheHill.yellowplayer02) {
                    KingOfTheHill.yellowplayer02 = null;
                }
                else if (KingOfTheHill.yellowplayer03 != null && KingOfTheHill.yellowTeam[0] == KingOfTheHill.yellowplayer03) {
                    KingOfTheHill.yellowplayer03 = null;
                }
                else if (KingOfTheHill.yellowplayer04 != null && KingOfTheHill.yellowTeam[0] == KingOfTheHill.yellowplayer04) {
                    KingOfTheHill.yellowplayer04 = null;
                }
                else if (KingOfTheHill.yellowplayer05 != null && KingOfTheHill.yellowTeam[0] == KingOfTheHill.yellowplayer05) {
                    KingOfTheHill.yellowplayer05 = null;
                }
                else if (KingOfTheHill.yellowplayer06 != null && KingOfTheHill.yellowTeam[0] == KingOfTheHill.yellowplayer06) {
                    KingOfTheHill.yellowplayer06 = null;
                }

                KingOfTheHill.yellowTeam.RemoveAt(0);
                KingOfTheHill.yellowTeam.Add(KingOfTheHill.yellowKingplayer);
                return;
            }
        }
        static void hotPotatoUpdate() {

            if (gameType != 5)
                return;

            // Fill the Danger Metter for hotPotato and update its distance for coldpotatoes
            if (HotPotato.hotPotatoPlayer != null && HudManager.Instance.DangerMeter.gameObject.active) {
                float leftdistance = 55f;
                float rightdistance = 15f;
                float currentdistance = float.MaxValue;

                float sqrMagnitude = (HotPotato.hotPotatoPlayer.transform.position - PlayerControl.LocalPlayer.transform.position).sqrMagnitude;
                if (sqrMagnitude < leftdistance && currentdistance > sqrMagnitude) {
                    currentdistance = sqrMagnitude;
                }

                float dangerLevelLeft = Mathf.Clamp01((leftdistance - currentdistance) / (leftdistance - rightdistance));
                float dangerLevelRight = Mathf.Clamp01((rightdistance - currentdistance) / rightdistance);
                HudManager.Instance.DangerMeter.SetDangerValue(dangerLevelLeft, dangerLevelRight);
            }
            
            // Hide hot potato sprite if in vent
            if (HotPotato.hotPotatoPlayer != null && HotPotato.hotPotato != null) {
                if (HotPotato.hotPotatoPlayer.inVent) {
                    HotPotato.hotPotato.SetActive(false);
                }
                else {
                    HotPotato.hotPotato.SetActive(true);
                }
            }

            // If hot potato disconnects, assing new potato and reset timer
            if (HotPotato.hotPotatoPlayer != null && HotPotato.hotPotatoPlayer.Data.Disconnected) {

                if (!HotPotato.firstPotatoTransfered) {
                    HotPotato.firstPotatoTransfered = true;
                }

                HotPotato.timeforTransfer = HotPotato.savedtimeforTransfer;

                int notPotatosAlives = -1;
                HotPotato.notPotatoTeamAlive.Clear();
                foreach (PlayerControl remainPotato in HotPotato.notPotatoTeam) {
                    if (!remainPotato.Data.IsDead) {
                        notPotatosAlives += 1;
                        HotPotato.notPotatoTeamAlive.Add(remainPotato);
                    }
                }

                if (notPotatosAlives < 1) {
                    HotPotato.triggerHotPotatoEnd = true;
                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HotPotatoEnd, false);
                }

                HotPotato.hotPotatoPlayer = HotPotato.notPotatoTeam[0];
                HotPotato.hotPotatoPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                HotPotato.hotPotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                HotPotato.hotPotato.transform.parent = HotPotato.hotPotatoPlayer.transform;

                // If hot potato timed out, assing new potato
                if (HotPotato.notPotato01 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato01) {
                    HotPotato.notPotato01 = null;
                }
                else if (HotPotato.notPotato02 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato02) {
                    HotPotato.notPotato02 = null;
                }
                else if (HotPotato.notPotato03 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato03) {
                    HotPotato.notPotato03 = null;
                }
                else if (HotPotato.notPotato04 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato04) {
                    HotPotato.notPotato04 = null;
                }
                else if (HotPotato.notPotato05 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato05) {
                    HotPotato.notPotato05 = null;
                }
                else if (HotPotato.notPotato06 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato06) {
                    HotPotato.notPotato06 = null;
                }
                else if (HotPotato.notPotato07 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato07) {
                    HotPotato.notPotato07 = null;
                }
                else if (HotPotato.notPotato08 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato08) {
                    HotPotato.notPotato08 = null;
                }
                else if (HotPotato.notPotato09 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato09) {
                    HotPotato.notPotato09 = null;
                }
                else if (HotPotato.notPotato10 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato10) {
                    HotPotato.notPotato10 = null;
                }
                else if (HotPotato.notPotato11 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato11) {
                    HotPotato.notPotato11 = null;
                }
                else if (HotPotato.notPotato12 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato12) {
                    HotPotato.notPotato12 = null;
                }
                else if (HotPotato.notPotato13 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato13) {
                    HotPotato.notPotato13 = null;
                }
                else if (HotPotato.notPotato14 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato14) {
                    HotPotato.notPotato14 = null;
                }

                HotPotato.notPotatoTeam.RemoveAt(0);

                hotPotatoButton.Timer = HotPotato.transferCooldown;

                Helpers.showGamemodesPopUp(1, Helpers.playerById(HotPotato.hotPotatoPlayer.PlayerId));
                HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + notPotatosAlives + "</color>";
            }

            // If notpotato disconnects, check number of notpotatos
            foreach (PlayerControl notPotato in HotPotato.notPotatoTeam) {
                if (notPotato.Data.Disconnected) {

                    int notPotatosAlives = -1;
                    HotPotato.notPotatoTeamAlive.Clear();
                    foreach (PlayerControl remainPotato in HotPotato.notPotatoTeam) {
                        if (!remainPotato.Data.IsDead) {
                            notPotatosAlives += 1;
                            HotPotato.notPotatoTeamAlive.Add(remainPotato);
                        }
                    }

                    if (notPotatosAlives < 1) {
                        HotPotato.triggerHotPotatoEnd = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HotPotatoEnd, false);
                    }
                    
                    if (HotPotato.notPotato01 != null && notPotato.PlayerId == HotPotato.notPotato01.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato01);
                    }
                    else if (HotPotato.notPotato02 != null && notPotato.PlayerId == HotPotato.notPotato02.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato02);
                    }
                    else if (HotPotato.notPotato03 != null && notPotato.PlayerId == HotPotato.notPotato03.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato03);
                    }
                    else if (HotPotato.notPotato04 != null && notPotato.PlayerId == HotPotato.notPotato04.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato04);
                    }
                    else if (HotPotato.notPotato05 != null && notPotato.PlayerId == HotPotato.notPotato05.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato05);
                    }
                    else if (HotPotato.notPotato06 != null && notPotato.PlayerId == HotPotato.notPotato06.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato06);
                    }
                    else if (HotPotato.notPotato07 != null && notPotato.PlayerId == HotPotato.notPotato07.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato07);
                    }
                    else if (HotPotato.notPotato08 != null && notPotato.PlayerId == HotPotato.notPotato08.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato08);
                    }
                    else if (HotPotato.notPotato09 != null && notPotato.PlayerId == HotPotato.notPotato09.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato09);
                    }
                    else if (HotPotato.notPotato10 != null && notPotato.PlayerId == HotPotato.notPotato10.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato10);
                    }
                    else if (HotPotato.notPotato11 != null && notPotato.PlayerId == HotPotato.notPotato11.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato11);
                    }
                    else if (HotPotato.notPotato12 != null && notPotato.PlayerId == HotPotato.notPotato12.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato12);
                    }
                    else if (HotPotato.notPotato13 != null && notPotato.PlayerId == HotPotato.notPotato13.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato13);
                    }
                    else if (HotPotato.notPotato14 != null && notPotato.PlayerId == HotPotato.notPotato14.PlayerId) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato14);
                    }

                    HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + notPotatosAlives + "</color>";
                    break;
                }
            }
        }

        static void zombieLaboratoryUpdate() {

            if(gameType != 6)
                return;

            var deltaTime = Time.deltaTime;
            // Check timers for survivors
            if (ZombieLaboratory.survivorPlayer01 != null && ZombieLaboratory.survivorPlayer01Timer > 0 && ZombieLaboratory.survivorPlayer01IsInfected) {
                ZombieLaboratory.survivorPlayer01Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer01Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer01HasKeyItem) {
                        ZombieLaboratory.survivorPlayer01HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer01.PlayerId, ZombieLaboratory.survivorPlayer01FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer01.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer01);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer01);
                    ZombieLaboratory.survivorPlayer01 = null;
                    ZombieLaboratory.survivorPlayer01IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer01 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer02 != null && ZombieLaboratory.survivorPlayer02Timer > 0 && ZombieLaboratory.survivorPlayer02IsInfected) {
                ZombieLaboratory.survivorPlayer02Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer02Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer02HasKeyItem) {
                        ZombieLaboratory.survivorPlayer02HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer02.PlayerId, ZombieLaboratory.survivorPlayer02FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer02.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer02);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer02);
                    ZombieLaboratory.survivorPlayer02 = null;
                    ZombieLaboratory.survivorPlayer02IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer02 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer03 != null && ZombieLaboratory.survivorPlayer03Timer > 0 && ZombieLaboratory.survivorPlayer03IsInfected) {
                ZombieLaboratory.survivorPlayer03Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer03Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer03HasKeyItem) {
                        ZombieLaboratory.survivorPlayer03HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer03.PlayerId, ZombieLaboratory.survivorPlayer03FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer03.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer03);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer03);
                    ZombieLaboratory.survivorPlayer03 = null;
                    ZombieLaboratory.survivorPlayer03IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer03 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer04 != null && ZombieLaboratory.survivorPlayer04Timer > 0 && ZombieLaboratory.survivorPlayer04IsInfected) {
                ZombieLaboratory.survivorPlayer04Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer04Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer04HasKeyItem) {
                        ZombieLaboratory.survivorPlayer04HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer04.PlayerId, ZombieLaboratory.survivorPlayer04FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer04.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer04);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer04);
                    ZombieLaboratory.survivorPlayer04 = null;
                    ZombieLaboratory.survivorPlayer04IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer04 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer05 != null && ZombieLaboratory.survivorPlayer05Timer > 0 && ZombieLaboratory.survivorPlayer05IsInfected) {
                ZombieLaboratory.survivorPlayer05Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer05Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer05HasKeyItem) {
                        ZombieLaboratory.survivorPlayer05HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer05.PlayerId, ZombieLaboratory.survivorPlayer05FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer05.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer05);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer05);
                    ZombieLaboratory.survivorPlayer05 = null;
                    ZombieLaboratory.survivorPlayer05IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer05 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer06 != null && ZombieLaboratory.survivorPlayer06Timer > 0 && ZombieLaboratory.survivorPlayer06IsInfected) {
                ZombieLaboratory.survivorPlayer06Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer06Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer06HasKeyItem) {
                        ZombieLaboratory.survivorPlayer06HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer06.PlayerId, ZombieLaboratory.survivorPlayer06FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer06.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer06);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer06);
                    ZombieLaboratory.survivorPlayer06 = null;
                    ZombieLaboratory.survivorPlayer06IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer06 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer07 != null && ZombieLaboratory.survivorPlayer07Timer > 0 && ZombieLaboratory.survivorPlayer07IsInfected) {
                ZombieLaboratory.survivorPlayer07Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer07Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer07HasKeyItem) {
                        ZombieLaboratory.survivorPlayer07HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer07.PlayerId, ZombieLaboratory.survivorPlayer07FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer07.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer07);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer07);
                    ZombieLaboratory.survivorPlayer07 = null;
                    ZombieLaboratory.survivorPlayer07IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer07 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer08 != null && ZombieLaboratory.survivorPlayer08Timer > 0 && ZombieLaboratory.survivorPlayer08IsInfected) {
                ZombieLaboratory.survivorPlayer08Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer08Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer08HasKeyItem) {
                        ZombieLaboratory.survivorPlayer08HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer08.PlayerId, ZombieLaboratory.survivorPlayer08FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer08.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer08);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer08);
                    ZombieLaboratory.survivorPlayer08 = null;
                    ZombieLaboratory.survivorPlayer08IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer08 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer09 != null && ZombieLaboratory.survivorPlayer09Timer > 0 && ZombieLaboratory.survivorPlayer09IsInfected) {
                ZombieLaboratory.survivorPlayer09Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer09Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer09HasKeyItem) {
                        ZombieLaboratory.survivorPlayer09HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer09.PlayerId, ZombieLaboratory.survivorPlayer09FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer09.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer09);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer09);
                    ZombieLaboratory.survivorPlayer09 = null;
                    ZombieLaboratory.survivorPlayer09IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer09 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer10 != null && ZombieLaboratory.survivorPlayer10Timer > 0 && ZombieLaboratory.survivorPlayer10IsInfected) {
                ZombieLaboratory.survivorPlayer10Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer10Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer10HasKeyItem) {
                        ZombieLaboratory.survivorPlayer10HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer10.PlayerId, ZombieLaboratory.survivorPlayer10FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer10.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer10);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer10);
                    ZombieLaboratory.survivorPlayer10 = null;
                    ZombieLaboratory.survivorPlayer10IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer10 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer11 != null && ZombieLaboratory.survivorPlayer11Timer > 0 && ZombieLaboratory.survivorPlayer11IsInfected) {
                ZombieLaboratory.survivorPlayer11Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer11Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer11HasKeyItem) {
                        ZombieLaboratory.survivorPlayer11HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer11.PlayerId, ZombieLaboratory.survivorPlayer11FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer11.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer11);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer11);
                    ZombieLaboratory.survivorPlayer11 = null;
                    ZombieLaboratory.survivorPlayer11IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer11 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer12 != null && ZombieLaboratory.survivorPlayer12Timer > 0 && ZombieLaboratory.survivorPlayer12IsInfected) {
                ZombieLaboratory.survivorPlayer12Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer12Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer12HasKeyItem) {
                        ZombieLaboratory.survivorPlayer12HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer12.PlayerId, ZombieLaboratory.survivorPlayer12FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer12.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer12);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer12);
                    ZombieLaboratory.survivorPlayer12 = null;
                    ZombieLaboratory.survivorPlayer12IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer12 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }
            if (ZombieLaboratory.survivorPlayer13 != null && ZombieLaboratory.survivorPlayer13Timer > 0 && ZombieLaboratory.survivorPlayer13IsInfected) {
                ZombieLaboratory.survivorPlayer13Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer13Timer <= 0) {
                    // Remove Survivor role
                    if (ZombieLaboratory.survivorPlayer13HasKeyItem) {
                        ZombieLaboratory.survivorPlayer13HasKeyItem = false;
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(ZombieLaboratory.survivorPlayer13.PlayerId, ZombieLaboratory.survivorPlayer13FoundBox);
                    }
                    RPCProcedure.zombieLaboratoryTurnZombie(ZombieLaboratory.survivorPlayer13.PlayerId);
                    ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer13);
                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer13);
                    ZombieLaboratory.survivorPlayer13 = null;
                    ZombieLaboratory.survivorPlayer13IsInfected = false;
                    if (ZombieLaboratory.survivorPlayer13 == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                        }
                    }
                }
            }

            // Check number of survivors if a survivor disconnects
            foreach (PlayerControl survivor in ZombieLaboratory.survivorTeam) {
                if (survivor.Data.Disconnected) {

                    if (ZombieLaboratory.nursePlayer != null && survivor.PlayerId == ZombieLaboratory.nursePlayer.PlayerId) {
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.nursePlayer);
                        ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                        ZombieLaboratory.triggerZombieWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ZombieWin, false);
                    }
                    else if (ZombieLaboratory.survivorPlayer01 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer01.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer01IsInfected) {
                            ZombieLaboratory.survivorPlayer01IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer01);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer01);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer01FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer02 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer02.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer02IsInfected) {
                            ZombieLaboratory.survivorPlayer02IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer02);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer02);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer02FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer03 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer03.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer03IsInfected) {
                            ZombieLaboratory.survivorPlayer03IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer03);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer03);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer03FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer04 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer04.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer04IsInfected) {
                            ZombieLaboratory.survivorPlayer04IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer04);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer04);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer04FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer05 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer05.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer05IsInfected) {
                            ZombieLaboratory.survivorPlayer05IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer05);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer05);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer05FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer06 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer06.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer06IsInfected) {
                            ZombieLaboratory.survivorPlayer06IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer06);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer06);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer06FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer07 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer07.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer07IsInfected) {
                            ZombieLaboratory.survivorPlayer07IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer07);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer07);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer07FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer08 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer08.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer08IsInfected) {
                            ZombieLaboratory.survivorPlayer08IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer08);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer08);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer08FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer09 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer09.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer09IsInfected) {
                            ZombieLaboratory.survivorPlayer09IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer09);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer09);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer09FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer10 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer10.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer10IsInfected) {
                            ZombieLaboratory.survivorPlayer10IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer10);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer10);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer10FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer11 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer11.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer11IsInfected) {
                            ZombieLaboratory.survivorPlayer11IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer11);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer11);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer11FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer12 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer12.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer12IsInfected) {
                            ZombieLaboratory.survivorPlayer12IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer12);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer12);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer12FoundBox);
                    }
                    else if (ZombieLaboratory.survivorPlayer13 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer13.PlayerId) {
                        if (ZombieLaboratory.survivorPlayer13IsInfected) {
                            ZombieLaboratory.survivorPlayer13IsInfected = false;
                            ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer13);
                        }
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.survivorPlayer13);
                        RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, ZombieLaboratory.survivorPlayer13FoundBox);
                    }

                    // Check win condition
                    ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                    if (ZombieLaboratory.survivorTeam.Count == 1) {
                        ZombieLaboratory.triggerZombieWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ZombieWin, false);
                    }
                    break;
                }
            }

            foreach (PlayerControl zombie in ZombieLaboratory.zombieTeam) {
                if (zombie.Data.Disconnected) {
                    // Check win condition
                    if (ZombieLaboratory.zombiePlayer01 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer01.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer01);
                    }
                    else if (ZombieLaboratory.zombiePlayer02 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer02.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer02);
                    }
                    else if (ZombieLaboratory.zombiePlayer03 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer03.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer03);
                    }
                    else if (ZombieLaboratory.zombiePlayer04 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer04.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer04);
                    }
                    else if (ZombieLaboratory.zombiePlayer05 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer05.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer05);
                    }
                    else if (ZombieLaboratory.zombiePlayer06 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer06.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer06);
                    }
                    else if (ZombieLaboratory.zombiePlayer07 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer07.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer07);
                    }
                    else if (ZombieLaboratory.zombiePlayer08 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer08.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer08);
                    }
                    else if (ZombieLaboratory.zombiePlayer09 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer09.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer09);
                    }
                    else if (ZombieLaboratory.zombiePlayer10 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer10.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer10);
                    }
                    else if (ZombieLaboratory.zombiePlayer11 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer11.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer11);
                    }
                    else if (ZombieLaboratory.zombiePlayer12 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer12.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer12);
                    }
                    else if (ZombieLaboratory.zombiePlayer13 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer13.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer13);
                    }
                    else if (ZombieLaboratory.zombiePlayer14 != null && zombie.PlayerId == ZombieLaboratory.zombiePlayer14.PlayerId) {
                        ZombieLaboratory.zombieTeam.Remove(ZombieLaboratory.zombiePlayer14);
                    }
                    ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                    if (ZombieLaboratory.zombieTeam.Count <= 0) {
                        ZombieLaboratory.triggerSurvivorWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.SurvivorWin, false);
                    }
                    break;
                }
            }
        }
        static void battleRoyaleUpdate() {

            if (gameType != 7)
                return;

            if (BattleRoyale.matchType == 0) {
                // If solo player disconnects, check number of players
                foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                    if (soloPlayer.Data.Disconnected) {

                        if (BattleRoyale.soloPlayer01 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer01.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer01);
                        }
                        else if (BattleRoyale.soloPlayer02 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer02.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer02);
                        }
                        else if (BattleRoyale.soloPlayer03 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer03.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer03);
                        }
                        else if (BattleRoyale.soloPlayer04 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer04.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer04);
                        }
                        else if (BattleRoyale.soloPlayer05 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer05.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer05);
                        }
                        else if (BattleRoyale.soloPlayer06 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer06.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer06);
                        }
                        else if (BattleRoyale.soloPlayer07 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer07.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer07);
                        }
                        else if (BattleRoyale.soloPlayer08 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer08.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer08);
                        }
                        else if (BattleRoyale.soloPlayer09 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer09.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer09);
                        }
                        else if (BattleRoyale.soloPlayer10 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer10.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer10);
                        }
                        else if (BattleRoyale.soloPlayer11 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer11.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer11);
                        }
                        else if (BattleRoyale.soloPlayer12 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer12.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer12);
                        }
                        else if (BattleRoyale.soloPlayer13 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer13.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer13);
                        }
                        else if (BattleRoyale.soloPlayer14 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer14.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer14);
                        }
                        else if (BattleRoyale.soloPlayer15 != null && soloPlayer.PlayerId == BattleRoyale.soloPlayer15.PlayerId) {
                            BattleRoyale.soloPlayerTeam.Remove(BattleRoyale.soloPlayer15);
                        }

                        int soloPlayersAlives = 0;

                        foreach (PlayerControl remainPlayer in BattleRoyale.soloPlayerTeam) {

                            if (!remainPlayer.Data.IsDead) {
                                soloPlayersAlives += 1;
                            }

                        }

                        BattleRoyale.battleRoyalepointCounter = Language.introTexts[11] + "<color=#009F57FF>" + soloPlayersAlives + "</color>";

                        if (soloPlayersAlives <= 1) {
                            BattleRoyale.triggerSoloWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleSoloWin, false);
                        }
                        break;
                    }
                }
            }
            else {
                // lime Team disconnects
                foreach (PlayerControl limePlayer in BattleRoyale.limeTeam) {
                    if (limePlayer.Data.Disconnected) {

                        if (BattleRoyale.limePlayer01 != null && limePlayer.PlayerId == BattleRoyale.limePlayer01.PlayerId) {
                            BattleRoyale.limeTeam.Remove(BattleRoyale.limePlayer01);
                        }
                        else if (BattleRoyale.limePlayer02 != null && limePlayer.PlayerId == BattleRoyale.limePlayer02.PlayerId) {
                            BattleRoyale.limeTeam.Remove(BattleRoyale.limePlayer02);
                        }
                        else if (BattleRoyale.limePlayer03 != null && limePlayer.PlayerId == BattleRoyale.limePlayer03.PlayerId) {
                            BattleRoyale.limeTeam.Remove(BattleRoyale.limePlayer03);
                        }
                        else if (BattleRoyale.limePlayer04 != null && limePlayer.PlayerId == BattleRoyale.limePlayer04.PlayerId) {
                            BattleRoyale.limeTeam.Remove(BattleRoyale.limePlayer04);
                        }
                        else if (BattleRoyale.limePlayer05 != null && limePlayer.PlayerId == BattleRoyale.limePlayer05.PlayerId) {
                            BattleRoyale.limeTeam.Remove(BattleRoyale.limePlayer05);
                        }
                        else if (BattleRoyale.limePlayer06 != null && limePlayer.PlayerId == BattleRoyale.limePlayer06.PlayerId) {
                            BattleRoyale.limeTeam.Remove(BattleRoyale.limePlayer06);
                        }
                        else if (BattleRoyale.limePlayer07 != null && limePlayer.PlayerId == BattleRoyale.limePlayer07.PlayerId) {
                            BattleRoyale.limeTeam.Remove(BattleRoyale.limePlayer07);
                        }

                        int limePlayersAlive = 0;

                        foreach (PlayerControl remainingLimePlayer in BattleRoyale.limeTeam) {

                            if (!remainingLimePlayer.Data.IsDead) {
                                limePlayersAlive += 1;
                            }

                        }

                        int pinkPlayersAlive = 0;

                        foreach (PlayerControl remainingPinkPlayer in BattleRoyale.pinkTeam) {

                            if (!remainingPinkPlayer.Data.IsDead) {
                                pinkPlayersAlive += 1;
                            }

                        }

                        if (BattleRoyale.serialKiller != null) {

                            int serialKillerAlive = 0;

                            foreach (PlayerControl serialKiller in BattleRoyale.serialKillerTeam) {

                                if (!serialKiller.Data.IsDead) {
                                    serialKillerAlive += 1;
                                }

                            }

                            if (BattleRoyale.matchType == 1) {
                                BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + limePlayersAlive + "</color> | " + Language.introTexts[13] + " <color=#F2BEFFFF>" + pinkPlayersAlive + "</color> | " + Language.introTexts[14] + "<color=#808080FF>" + serialKillerAlive + "</color>";
                                if (limePlayersAlive <= 0 && pinkPlayersAlive <= 0 && !BattleRoyale.serialKiller.Data.IsDead) {
                                    BattleRoyale.triggerSerialKillerWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleSerialKillerWin, false);
                                }
                                else if (pinkPlayersAlive <= 0 && BattleRoyale.serialKiller.Data.IsDead) {
                                    BattleRoyale.triggerLimeTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                                }
                                else if (limePlayersAlive <= 0 && BattleRoyale.serialKiller.Data.IsDead) {
                                    BattleRoyale.triggerPinkTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                                }
                            }
                        }
                        else {
                            if (BattleRoyale.matchType == 1) {
                                BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + limePlayersAlive + "</color> | " + Language.introTexts[13] + " <color=#F2BEFFFF>" + pinkPlayersAlive + "</color>";
                                if (pinkPlayersAlive <= 0) {
                                    BattleRoyale.triggerLimeTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                                }
                                else if (limePlayersAlive <= 0) {
                                    BattleRoyale.triggerPinkTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                                }
                            }
                        }
                        break;
                    }
                }
                // Pink Team disconnects
                foreach (PlayerControl pinkPlayer in BattleRoyale.pinkTeam) {
                    if (pinkPlayer.Data.Disconnected) {

                        if (BattleRoyale.pinkPlayer01 != null && pinkPlayer.PlayerId == BattleRoyale.pinkPlayer01.PlayerId) {
                            BattleRoyale.pinkTeam.Remove(BattleRoyale.pinkPlayer01);
                        }
                        else if (BattleRoyale.pinkPlayer02 != null && pinkPlayer.PlayerId == BattleRoyale.pinkPlayer02.PlayerId) {
                            BattleRoyale.pinkTeam.Remove(BattleRoyale.pinkPlayer02);
                        }
                        else if (BattleRoyale.pinkPlayer03 != null && pinkPlayer.PlayerId == BattleRoyale.pinkPlayer03.PlayerId) {
                            BattleRoyale.pinkTeam.Remove(BattleRoyale.pinkPlayer03);
                        }
                        else if (BattleRoyale.pinkPlayer04 != null && pinkPlayer.PlayerId == BattleRoyale.pinkPlayer04.PlayerId) {
                            BattleRoyale.pinkTeam.Remove(BattleRoyale.pinkPlayer04);
                        }
                        else if (BattleRoyale.pinkPlayer05 != null && pinkPlayer.PlayerId == BattleRoyale.pinkPlayer05.PlayerId) {
                            BattleRoyale.pinkTeam.Remove(BattleRoyale.pinkPlayer05);
                        }
                        else if (BattleRoyale.pinkPlayer06 != null && pinkPlayer.PlayerId == BattleRoyale.pinkPlayer06.PlayerId) {
                            BattleRoyale.pinkTeam.Remove(BattleRoyale.pinkPlayer06);
                        }
                        else if (BattleRoyale.pinkPlayer07 != null && pinkPlayer.PlayerId == BattleRoyale.pinkPlayer07.PlayerId) {
                            BattleRoyale.pinkTeam.Remove(BattleRoyale.pinkPlayer07);
                        }

                        int limePlayersAlive = 0;

                        foreach (PlayerControl remainingLimePlayer in BattleRoyale.limeTeam) {

                            if (!remainingLimePlayer.Data.IsDead) {
                                limePlayersAlive += 1;
                            }

                        }

                        int pinkPlayersAlive = 0;

                        foreach (PlayerControl remainingPinkPlayer in BattleRoyale.pinkTeam) {

                            if (!remainingPinkPlayer.Data.IsDead) {
                                pinkPlayersAlive += 1;
                            }

                        }

                        if (BattleRoyale.serialKiller != null) {

                            int serialKillerAlive = 0;

                            foreach (PlayerControl serialKiller in BattleRoyale.serialKillerTeam) {

                                if (!serialKiller.Data.IsDead) {
                                    serialKillerAlive += 1;
                                }

                            }

                            if (BattleRoyale.matchType == 1) {
                                BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + limePlayersAlive + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + pinkPlayersAlive + "</color> | " + Language.introTexts[14] + "<color=#808080FF>" + serialKillerAlive + "</color>";
                                if (limePlayersAlive <= 0 && pinkPlayersAlive <= 0 && !BattleRoyale.serialKiller.Data.IsDead) {
                                    BattleRoyale.triggerSerialKillerWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleSerialKillerWin, false);
                                }
                                else if (pinkPlayersAlive <= 0 && BattleRoyale.serialKiller.Data.IsDead) {
                                    BattleRoyale.triggerLimeTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                                }
                                else if (limePlayersAlive <= 0 && BattleRoyale.serialKiller.Data.IsDead) {
                                    BattleRoyale.triggerPinkTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                                }
                            }
                        }
                        else {
                            if (BattleRoyale.matchType == 1) {
                                BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + limePlayersAlive + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + pinkPlayersAlive + "</color>";
                                if (pinkPlayersAlive <= 0) {
                                    BattleRoyale.triggerLimeTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                                }
                                else if (limePlayersAlive <= 0) {
                                    BattleRoyale.triggerPinkTeamWin = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                                }
                            }
                        }
                        break;
                    }
                }
                // Serial Killer disconnects
                if (BattleRoyale.serialKiller != null && BattleRoyale.serialKiller.Data.Disconnected) {

                    BattleRoyale.serialKillerTeam.Remove(BattleRoyale.serialKiller);

                    int limePlayersAlive = 0;

                    foreach (PlayerControl limePlayer in BattleRoyale.limeTeam) {

                        if (!limePlayer.Data.IsDead) {
                            limePlayersAlive += 1;
                        }

                    }

                    int pinkPlayersAlive = 0;

                    foreach (PlayerControl pinkPlayer in BattleRoyale.pinkTeam) {

                        if (!pinkPlayer.Data.IsDead) {
                            pinkPlayersAlive += 1;
                        }

                    }

                    int serialKillerAlive = 0;

                    if (BattleRoyale.matchType == 1) {
                        BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + limePlayersAlive + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + pinkPlayersAlive + "</color> | " + Language.introTexts[14] + " <color=#808080FF>" + serialKillerAlive + "</color>";
                        if (pinkPlayersAlive <= 0) {
                            BattleRoyale.triggerLimeTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                        }
                        else if (limePlayersAlive <= 0) {
                            BattleRoyale.triggerPinkTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                        }
                    }

                }
            }
        }

        static void monjaFestivalUpdate() {

            if (gameType != 8)
                return;

            // Big Monja invisible
            if (MonjaFestival.bigMonjaPlayer != null && MonjaFestival.bigMonjaPlayerInvisibleTimer > 0) {
                MonjaFestival.bigMonjaPlayerInvisibleTimer -= Time.deltaTime;

                if (MonjaFestival.bigMonjaPlayerInvisibleTimer > 0f) {
                    if (MonjaFestival.bigMonjaPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                        Helpers.alphaPlayer(true, MonjaFestival.bigMonjaPlayer.PlayerId);
                    }
                    else {
                        Helpers.invisiblePlayer(MonjaFestival.bigMonjaPlayer.PlayerId);
                    }
                }

                // Big Monja reset
                if (MonjaFestival.bigMonjaPlayerInvisibleTimer <= 0f) {
                    MonjaFestival.resetBigMonja();
                }
            }

            // Green Team disconnects
            foreach (PlayerControl greenPlayer in MonjaFestival.greenTeam) {
                if (greenPlayer.Data.Disconnected) {

                    if (MonjaFestival.greenPlayer01 != null && greenPlayer.PlayerId == MonjaFestival.greenPlayer01.PlayerId) {
                        MonjaFestival.greenTeam.Remove(MonjaFestival.greenPlayer01);
                    }
                    else if (MonjaFestival.greenPlayer02 != null && greenPlayer.PlayerId == MonjaFestival.greenPlayer02.PlayerId) {
                        MonjaFestival.greenTeam.Remove(MonjaFestival.greenPlayer02);
                    }
                    else if (MonjaFestival.greenPlayer03 != null && greenPlayer.PlayerId == MonjaFestival.greenPlayer03.PlayerId) {
                        MonjaFestival.greenTeam.Remove(MonjaFestival.greenPlayer03);
                    }
                    else if (MonjaFestival.greenPlayer04 != null && greenPlayer.PlayerId == MonjaFestival.greenPlayer04.PlayerId) {
                        MonjaFestival.greenTeam.Remove(MonjaFestival.greenPlayer04);
                    }
                    else if (MonjaFestival.greenPlayer05 != null && greenPlayer.PlayerId == MonjaFestival.greenPlayer05.PlayerId) {
                        MonjaFestival.greenTeam.Remove(MonjaFestival.greenPlayer05);
                    }
                    else if (MonjaFestival.greenPlayer06 != null && greenPlayer.PlayerId == MonjaFestival.greenPlayer06.PlayerId) {
                        MonjaFestival.greenTeam.Remove(MonjaFestival.greenPlayer06);
                    }
                    else if (MonjaFestival.greenPlayer07 != null && greenPlayer.PlayerId == MonjaFestival.greenPlayer07.PlayerId) {
                        MonjaFestival.greenTeam.Remove(MonjaFestival.greenPlayer07);
                    }

                    int greenPlayersAlive = 0;

                    foreach (PlayerControl remainingGreenPlayer in MonjaFestival.greenTeam) {

                        if (!remainingGreenPlayer.Data.IsDead) {
                            greenPlayersAlive += 1;
                        }

                    }

                    int cyanPlayersAlive = 0;

                    foreach (PlayerControl remainingCyanPlayer in MonjaFestival.cyanTeam) {

                        if (!remainingCyanPlayer.Data.IsDead) {
                            cyanPlayersAlive += 1;
                        }

                    }

                    if (MonjaFestival.bigMonjaPlayer != null) {

                        int bigMonjaAlive = 0;

                        foreach (PlayerControl bigMonja in MonjaFestival.bigMonjaTeam) {

                            if (!bigMonja.Data.IsDead) {
                                bigMonjaAlive += 1;
                            }

                        }

                        MonjaFestival.monjaFestivalCounter = "<color=#00FF00FF>" + Language.introTexts[17] + MonjaFestival.greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + MonjaFestival.cyanPoints + "</color> | " + "<color=#808080FF>" + Language.introTexts[19] + MonjaFestival.bigMonjaPoints + "</color>";
                        if (greenPlayersAlive <= 0 && cyanPlayersAlive <= 0) {
                            MonjaFestival.triggerBigMonjaWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalBigMonjaWin, false);
                        }
                    }
                    else {
                        MonjaFestival.monjaFestivalCounter = "<color=#00FF00FF>" + Language.introTexts[17] + MonjaFestival.greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + MonjaFestival.cyanPoints + "</color>";
                        if (cyanPlayersAlive <= 0) {
                            MonjaFestival.triggerGreenTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalGreenWin, false);
                        }
                        else if (greenPlayersAlive <= 0) {
                            MonjaFestival.triggerCyanTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalCyanWin, false);
                        }
                    }
                    break;
                }
            }

            // Cyan Team disconnects
            foreach (PlayerControl cyanPlayer in MonjaFestival.cyanTeam) {
                if (cyanPlayer.Data.Disconnected) {

                    if (MonjaFestival.cyanPlayer01 != null && cyanPlayer.PlayerId == MonjaFestival.cyanPlayer01.PlayerId) {
                        MonjaFestival.cyanTeam.Remove(MonjaFestival.cyanPlayer01);
                    }
                    else if (MonjaFestival.cyanPlayer02 != null && cyanPlayer.PlayerId == MonjaFestival.cyanPlayer02.PlayerId) {
                        MonjaFestival.cyanTeam.Remove(MonjaFestival.cyanPlayer02);
                    }
                    else if (MonjaFestival.cyanPlayer03 != null && cyanPlayer.PlayerId == MonjaFestival.cyanPlayer03.PlayerId) {
                        MonjaFestival.cyanTeam.Remove(MonjaFestival.cyanPlayer03);
                    }
                    else if (MonjaFestival.cyanPlayer04 != null && cyanPlayer.PlayerId == MonjaFestival.cyanPlayer04.PlayerId) {
                        MonjaFestival.cyanTeam.Remove(MonjaFestival.cyanPlayer04);
                    }
                    else if (MonjaFestival.cyanPlayer05 != null && cyanPlayer.PlayerId == MonjaFestival.cyanPlayer05.PlayerId) {
                        MonjaFestival.cyanTeam.Remove(MonjaFestival.cyanPlayer05);
                    }
                    else if (MonjaFestival.cyanPlayer06 != null && cyanPlayer.PlayerId == MonjaFestival.cyanPlayer06.PlayerId) {
                        MonjaFestival.cyanTeam.Remove(MonjaFestival.cyanPlayer06);
                    }
                    else if (MonjaFestival.cyanPlayer07 != null && cyanPlayer.PlayerId == MonjaFestival.cyanPlayer07.PlayerId) {
                        MonjaFestival.cyanTeam.Remove(MonjaFestival.cyanPlayer07);
                    }

                    int greenPlayersAlive = 0;

                    foreach (PlayerControl remainingGreenPlayer in MonjaFestival.greenTeam) {

                        if (!remainingGreenPlayer.Data.IsDead) {
                            greenPlayersAlive += 1;
                        }

                    }

                    int cyanPlayersAlive = 0;

                    foreach (PlayerControl remainingCyanPlayer in MonjaFestival.cyanTeam) {

                        if (!remainingCyanPlayer.Data.IsDead) {
                            cyanPlayersAlive += 1;
                        }

                    }

                    if (MonjaFestival.bigMonjaPlayer != null) {

                        int bigMonjaAlive = 0;

                        foreach (PlayerControl bigMonja in MonjaFestival.bigMonjaTeam) {

                            if (!bigMonja.Data.IsDead) {
                                bigMonjaAlive += 1;
                            }

                        }

                        MonjaFestival.monjaFestivalCounter = "<color=#00FF00FF>" + Language.introTexts[17] + MonjaFestival.greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + MonjaFestival.cyanPoints + "</color> | " + "<color=#808080FF>" + Language.introTexts[19] + MonjaFestival.bigMonjaPoints + "</color>";
                        if (greenPlayersAlive <= 0 && cyanPlayersAlive <= 0) {
                            MonjaFestival.triggerBigMonjaWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalBigMonjaWin, false);
                        }
                    }
                    else {
                        MonjaFestival.monjaFestivalCounter = "<color=#00FF00FF>" + Language.introTexts[17] + MonjaFestival.greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + MonjaFestival.cyanPoints + "</color>";
                        if (cyanPlayersAlive <= 0) {
                            MonjaFestival.triggerGreenTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalGreenWin, false);
                        }
                        else if (greenPlayersAlive <= 0) {
                            MonjaFestival.triggerCyanTeamWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalCyanWin, false);
                        }
                    }
                    break;
                }
            }

            // Big Monja disconnects
            if (MonjaFestival.bigMonjaPlayer != null && MonjaFestival.bigMonjaPlayer.Data.Disconnected) {

                MonjaFestival.bigMonjaTeam.Remove(MonjaFestival.bigMonjaPlayer);

                int greenPlayersAlive = 0;

                foreach (PlayerControl greenPlayer in MonjaFestival.greenTeam) {

                    if (!greenPlayer.Data.IsDead) {
                        greenPlayersAlive += 1;
                    }

                }

                int cyanPlayersAlive = 0;

                foreach (PlayerControl cyanPlayer in MonjaFestival.cyanTeam) {

                    if (!cyanPlayer.Data.IsDead) {
                        cyanPlayersAlive += 1;
                    }

                }

                MonjaFestival.bigMonjaPoints = 0;

                MonjaFestival.monjaFestivalCounter = "<color=#00FF00FF>" + Language.introTexts[17] + MonjaFestival.greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + MonjaFestival.cyanPoints + "</color>";
                if (cyanPlayersAlive <= 0) {
                    MonjaFestival.triggerGreenTeamWin = true;
                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalGreenWin, false);
                }
                else if (greenPlayersAlive <= 0) {
                    MonjaFestival.triggerCyanTeamWin = true;
                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalCyanWin, false);
                }
            }
        }

        public static IEnumerator monjaBigOneReload() {
            MonjaFestival.bigSpawnOneReloading = true;
            while (MonjaFestival.bigSpawnOnePoints < 30) {
                yield return new WaitForSeconds(10);
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended) {
                    break;
                }
                MonjaFestival.bigSpawnOnePoints += 1;
                MonjaFestival.bigSpawnOne.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.bigSpawnOneFull.GetComponent<SpriteRenderer>().sprite;
                MonjaFestival.bigSpawnOneCount.text = $"{MonjaFestival.bigSpawnOnePoints} / 30";
                if (MonjaFestival.bigSpawnOnePoints == 30) {
                    MonjaFestival.bigSpawnOneReloading = false;
                }
            }
        }

        public static IEnumerator monjaBigTwoReload() {
            MonjaFestival.bigSpawnTwoReloading = true;
            while (MonjaFestival.bigSpawnTwoPoints < 30) {
                yield return new WaitForSeconds(10);
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended) {
                    break;
                }
                MonjaFestival.bigSpawnTwoPoints += 1;
                MonjaFestival.bigSpawnTwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.bigSpawnOneFull.GetComponent<SpriteRenderer>().sprite;
                MonjaFestival.bigSpawnTwoCount.text = $"{MonjaFestival.bigSpawnTwoPoints} / 30";
                if (MonjaFestival.bigSpawnTwoPoints == 30) {
                    MonjaFestival.bigSpawnTwoReloading = false;
                }
            }
        }

        public static IEnumerator monjaLittleOneReload() {
            MonjaFestival.littleSpawnOneReloading = true;
            while (MonjaFestival.littleSpawnOnePoints < 10) {
                yield return new WaitForSeconds(20);
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended) {
                    break;
                }
                MonjaFestival.littleSpawnOnePoints += 1;
                MonjaFestival.littleSpawnOne.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.littleSpawnOneFull.GetComponent<SpriteRenderer>().sprite;
                MonjaFestival.littleSpawnOneCount.text = $"{MonjaFestival.littleSpawnOnePoints} / 10";
                if (MonjaFestival.littleSpawnOnePoints == 10) {
                    MonjaFestival.littleSpawnOneReloading = false;
                }
            }
        }

        public static IEnumerator monjaLittleTwoReload() {
            MonjaFestival.littleSpawnTwoReloading = true;
            while (MonjaFestival.littleSpawnTwoPoints < 10) {
                yield return new WaitForSeconds(20);
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended) {
                    break;
                }
                MonjaFestival.littleSpawnTwoPoints += 1;
                MonjaFestival.littleSpawnTwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.littleSpawnOneFull.GetComponent<SpriteRenderer>().sprite;
                MonjaFestival.littleSpawnTwoCount.text = $"{MonjaFestival.littleSpawnTwoPoints} / 10";
                if (MonjaFestival.littleSpawnTwoPoints == 10) {
                    MonjaFestival.littleSpawnTwoReloading = false;
                }
            }
        }

        public static IEnumerator monjaLittleThreeReload() {
            MonjaFestival.littleSpawnThreeReloading = true;
            while (MonjaFestival.littleSpawnThreePoints < 10) {
                yield return new WaitForSeconds(20);
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended) {
                    break;
                }
                MonjaFestival.littleSpawnThreePoints += 1;
                MonjaFestival.littleSpawnThree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.littleSpawnOneFull.GetComponent<SpriteRenderer>().sprite;
                MonjaFestival.littleSpawnThreeCount.text = $"{MonjaFestival.littleSpawnThreePoints} / 10";
                if (MonjaFestival.littleSpawnThreePoints == 10) {
                    MonjaFestival.littleSpawnThreeReloading = false;
                }
            }
        }

        public static IEnumerator monjaLittleFourReload() {
            MonjaFestival.littleSpawnFourReloading = true;
            while (MonjaFestival.littleSpawnFourPoints < 10) {
                yield return new WaitForSeconds(20);
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended) {
                    break;
                }
                MonjaFestival.littleSpawnFourPoints += 1;
                MonjaFestival.littleSpawnFour.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.littleSpawnOneFull.GetComponent<SpriteRenderer>().sprite;
                MonjaFestival.littleSpawnFourCount.text = $"{MonjaFestival.littleSpawnFourPoints} / 10";
                if (MonjaFestival.littleSpawnFourPoints == 10) {
                    MonjaFestival.littleSpawnFourReloading = false;
                }
            }
        }
        public static IEnumerator allulMonjaReload() {
            MonjaFestival.allulMonja.SetActive(false);
            int randomPosition = rnd.Next(0, 5);
            MonjaFestival.allulMonja.transform.position = MonjaFestival.allulMonjaPositions[randomPosition];
            yield return new WaitForSeconds(45);
            if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) {
                MonjaFestival.allulMonja.SetActive(true);
            }
        }

        static void Postfix(HudManager __instance) {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;


            CustomButton.HudUpdate();
            resetNameTagsAndColors();
            setNameColors();
            setNameTags();
            UpdateMiniMap();

            // Better Sabotages
            shakeScreenIfReactorSabotage();
            anonymousCommsSabotage();
            slowSpeedIfOxigenSabotage();

            // Impostors
            updateImpostorKillButton(__instance);

            // Custom gamemode report button update
            updateReportButton(__instance);

            // Timer updates
            timerUpdate();

            // Janitor corpse moving
            janitorUpdate();

            // Chameleon update
            chameleonUpdate();

            //BountyHunter update
            bountyHunterSuicideIfDisconnect();

            // Yinyanger update
            yinyangerUpdate();

            // Challenger update
            challengerUpdate();

            // Yandere update
            yandereUpdate();
            
            // Stranded update
            strandedUpdate();

            // Exiler update
            exilerWinIfDisconnect();

            // Seeker update
            seekerUpdate();

            // FortuneTeller update
            fortuneTellerUpdate();

            // Kid
            kidUpdate();

            // Spiritualist update
            spiritualistUpdate();

            // VigilantMira update
            vigilantMiraUpdate();

            // Bat update
            batUpdate();

            // Necromancer corpse moving
            necromancerUpdate();

            // Capture the flag flags movement + fix if someone disconnnects
            captureTheFlagUpdate();

            // Police and thief jewel restore values if someone disconnnects
            policeandthiefUpdate();

            // King of the hill point time count
            kingOfTheHillUpdate();

            // Hot Potato disconnect update
            hotPotatoUpdate();

            // ZombieLaboratory disconnect update
            zombieLaboratoryUpdate();

            // Battle Royale disconnect update
            battleRoyaleUpdate();

            // Monja Festival disconnect update
            monjaFestivalUpdate();
        }
    }
}