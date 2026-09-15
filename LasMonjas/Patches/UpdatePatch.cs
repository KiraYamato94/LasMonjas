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

namespace LasMonjas.Patches
{
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
                if (PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor && player.Data.Role.IsImpostor) {
                    player.cosmetics.nameText.color = Palette.ImpostorRed;
                }
                else {
                    player.cosmetics.nameText.color = Color.white;
                }
            }
            if (MeetingHud.Instance != null) {
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates) {
                    PlayerControl playerControl = playersById.ContainsKey((byte)player.PlayerId) ? playersById[(byte)player.PlayerId] : null;
                    if (playerControl != null) {
                        player.NameText.text = playerControl.Data.PlayerName;
                        if (PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor && playerControl.Data.Role.IsImpostor) {
                            player.NameText.color = Palette.ImpostorRed;
                        }
                        else {
                            player.NameText.color = Color.white;
                        }
                    }
                }
            }
            if (PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor) {
                List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToList();
                impostors.RemoveAll(x => !x.Data.Role.IsImpostor);
                foreach (PlayerControl player in impostors)
                    player.cosmetics.nameText.color = Palette.ImpostorRed;
                if (MeetingHud.Instance != null)
                    foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates) {
                        PlayerControl playerControl = Helpers.playerById((byte)player.PlayerId);
                        if (playerControl != null && playerControl.Data.Role.IsImpostor)
                            player.NameText.color = Palette.ImpostorRed;
                    }
            }
        }
        static void setPlayerNameColor(PlayerControl p, Color color) {
            p.cosmetics.nameText.color = color;
            if (MeetingHud.Instance != null)
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                    if (player.NameText != null && p.PlayerId == player.PlayerId)
                        player.NameText.color = color;
        }

        private static void SetGameModeTeamColor(IEnumerable<PlayerControl> team, Color color) {
            foreach (PlayerControl player in team) {
                if (player != null) {
                    setPlayerNameColor(player, color);
                }
            }
        }

        private static void SetNeutralGameModePlayerColor(PlayerControl player, Color color) {
            if (player != null) {
                setPlayerNameColor(player, color);
            }
        }

        static void setNameColors() {

            switch (gameType) {
                case 0:
                case 1:
                    // Crewmates name color
                    var localPlayer = PlayerInCache.LocalPlayer.PlayerControl;
                    var localRole = RoleInfo.getRoleInfoForPlayer(localPlayer).FirstOrDefault();
                    setPlayerNameColor(localPlayer, localRole.color);

                    // Exiler can see its target
                    if (Exiler.exiler != null && Exiler.exiler == PlayerInCache.LocalPlayer.PlayerControl) {
                        if (Exiler.target != null) {
                            setPlayerNameColor(Exiler.target, Sheriff.color);
                            if (Challenger.isDueling) {
                                setPlayerNameColor(Exiler.target, Color.white);
                            }
                        }
                    }

                    // Renegade can see his minion
                    else if (Renegade.renegade != null && Renegade.renegade == PlayerInCache.LocalPlayer.PlayerControl) {
                        if (Minion.minion != null) {
                            setPlayerNameColor(Minion.minion, Renegade.color);
                        }
                    }

                    // Minion can see the renegade
                    else if (Minion.minion != null && Minion.minion == PlayerInCache.LocalPlayer.PlayerControl) {
                        if (Renegade.renegade != null) {
                            setPlayerNameColor(Renegade.renegade, Renegade.color);
                        }
                    }

                    // Yandere can see her target
                    else if (Yandere.yandere != null && Yandere.yandere == PlayerInCache.LocalPlayer.PlayerControl) {
                        if (Yandere.target != null) {
                            setPlayerNameColor(Yandere.target, Sheriff.color);
                            if (Seeker.isMinigaming) {
                                setPlayerNameColor(Yandere.target, Color.white);
                            }
                        }
                    }

                    // Impostor roles with no color changes: Mimic, Painter, Demon, Janitor, Illusionist, Manipulator, Bomberman, Chameleon, Gambler, Sorcerer, Medusa, Hypnotist, Archer, Plumber, Librarian 
                    break;
                case 2:
                    // CTF
                    SetNeutralGameModePlayerColor(CaptureTheFlag.stealerPlayer, Palette.PlayerColors[15]);
                    SetGameModeTeamColor(CaptureTheFlag.redteamFlag, Palette.PlayerColors[0]);
                    SetGameModeTeamColor(CaptureTheFlag.blueteamFlag, Palette.PlayerColors[1]);
                    break;
                case 3:
                    // PT
                    SetGameModeTeamColor(PoliceAndThief.policeTeam.Where(p => p != PoliceAndThief.policeplayer02 && p != PoliceAndThief.policeplayer04), Palette.PlayerColors[10]);
                    SetGameModeTeamColor(new[] { PoliceAndThief.policeplayer02, PoliceAndThief.policeplayer04 }, Palette.PlayerColors[5]);
                    SetGameModeTeamColor(PoliceAndThief.thiefTeam, Palette.PlayerColors[16]);
                    break;
                case 4:
                    // KOTH
                    SetNeutralGameModePlayerColor(KingOfTheHill.usurperPlayer, Palette.PlayerColors[15]);
                    SetGameModeTeamColor(KingOfTheHill.greenTeam, Palette.PlayerColors[2]);
                    SetGameModeTeamColor(KingOfTheHill.yellowTeam, Palette.PlayerColors[5]);
                    break;
                case 5:
                    // HP
                    SetGameModeTeamColor(HotPotato.notPotatoTeam, Palette.PlayerColors[10]);
                    SetGameModeTeamColor(HotPotato.explodedPotatoTeam, Palette.PlayerColors[9]);
                    SetNeutralGameModePlayerColor(HotPotato.hotPotatoPlayer, Palette.PlayerColors[15]);
                    break;
                case 6:
                    // ZL
                    SetGameModeTeamColor(ZombieLaboratory.survivorTeam, Palette.PlayerColors[10]);
                    SetGameModeTeamColor(ZombieLaboratory.infectedPlayers, Palette.PlayerColors[5]);
                    SetGameModeTeamColor(ZombieLaboratory.zombieTeam, Palette.PlayerColors[16]);
                    SetNeutralGameModePlayerColor(ZombieLaboratory.nursePlayer, Palette.PlayerColors[3]);
                    break;
                case 7:
                    // BR
                    if (BattleRoyale.matchType == 0) {
                        SetGameModeTeamColor(BattleRoyale.soloPlayerTeam, Palette.PlayerColors[2]);
                    }
                    else {
                        SetNeutralGameModePlayerColor(BattleRoyale.serialKiller, Palette.PlayerColors[15]);
                        SetGameModeTeamColor(BattleRoyale.limeTeam, Palette.PlayerColors[11]);
                        SetGameModeTeamColor(BattleRoyale.pinkTeam, Palette.PlayerColors[13]);
                    }
                    break;
                case 8:
                    // MF
                    SetNeutralGameModePlayerColor(MonjaFestival.bigMonjaPlayer, Palette.PlayerColors[15]);
                    SetGameModeTeamColor(MonjaFestival.greenTeam, Palette.PlayerColors[2]);
                    SetGameModeTeamColor(MonjaFestival.cyanTeam, Palette.PlayerColors[10]);
                    break;
            }
        }

        private static void AddGameModeTimerTag(PlayerControl player, float timer, Color color) {
            if (player == null) return;

            string suffix = Helpers.cs(color, " (" + timer.ToString("F0") + ")");
            player.cosmetics.nameText.text += suffix;
        }

        private static void AddBattleRoyaleLivesTag(PlayerControl player, float lives, Color color) {
            if (player == null) return;

            player.cosmetics.nameText.text += Helpers.cs(color, " (" + lives + "♥)");
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
                                if (Modifiers.lover1.PlayerId == player.PlayerId || Modifiers.lover2.PlayerId == player.PlayerId)
                                    player.NameText.text += suffix;
                    }

                    // Forensic show color type on meeting
                    if (Forensic.forensic != null && PlayerInCache.LocalPlayer.PlayerControl == Forensic.forensic) {
                        if (MeetingHud.Instance != null) {
                            foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates) {
                                var target = Helpers.playerById(player.PlayerId);
                                if (target != null) player.NameText.text += $" ({(Helpers.isLighterColor(target.Data.DefaultOutfit.ColorId) ? "L" : "D")})";
                            }
                        }
                    }
                    break;
                case 6:
                    // ZL Timers
                    // don't show timers to Zombie team
                    bool isNotZombie = ZombieLaboratory.survivorTeam.Contains(PlayerInCache.LocalPlayer.PlayerControl);

                    if (isNotZombie) {
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer01, ZombieLaboratory.survivorPlayer01Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer02, ZombieLaboratory.survivorPlayer02Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer03, ZombieLaboratory.survivorPlayer03Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer04, ZombieLaboratory.survivorPlayer04Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer05, ZombieLaboratory.survivorPlayer05Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer06, ZombieLaboratory.survivorPlayer06Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer07, ZombieLaboratory.survivorPlayer07Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer08, ZombieLaboratory.survivorPlayer08Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer09, ZombieLaboratory.survivorPlayer09Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer10, ZombieLaboratory.survivorPlayer10Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer11, ZombieLaboratory.survivorPlayer11Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer12, ZombieLaboratory.survivorPlayer12Timer, Sheriff.color);
                        AddGameModeTimerTag(ZombieLaboratory.survivorPlayer13, ZombieLaboratory.survivorPlayer13Timer, Sheriff.color);
                    }
                    break;
                case 7:
                    // BR Lives
                    if (BattleRoyale.matchType == 0) {
                        // show the remaining lives of all players if you're dead
                        if (PlayerInCache.LocalPlayer.PlayerControl.Data.IsDead) {
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer01, BattleRoyale.soloPlayer01Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer02, BattleRoyale.soloPlayer02Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer03, BattleRoyale.soloPlayer03Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer04, BattleRoyale.soloPlayer04Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer05, BattleRoyale.soloPlayer05Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer06, BattleRoyale.soloPlayer06Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer07, BattleRoyale.soloPlayer07Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer08, BattleRoyale.soloPlayer08Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer09, BattleRoyale.soloPlayer09Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer10, BattleRoyale.soloPlayer10Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer11, BattleRoyale.soloPlayer11Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer12, BattleRoyale.soloPlayer12Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer13, BattleRoyale.soloPlayer13Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer14, BattleRoyale.soloPlayer14Lifes, Sheriff.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer15, BattleRoyale.soloPlayer15Lifes, Sheriff.color);
                        }
                        // only see your lives
                        else {
                            if (BattleRoyale.soloPlayer01 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer01) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer01, BattleRoyale.soloPlayer01Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer02 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer02) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer02, BattleRoyale.soloPlayer02Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer03 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer03) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer03, BattleRoyale.soloPlayer03Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer04 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer04) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer04, BattleRoyale.soloPlayer04Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer05 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer05) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer05, BattleRoyale.soloPlayer05Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer06 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer06) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer06, BattleRoyale.soloPlayer06Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer07 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer07) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer07, BattleRoyale.soloPlayer07Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer08 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer08) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer08, BattleRoyale.soloPlayer08Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer09 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer09) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer09, BattleRoyale.soloPlayer09Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer10 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer10) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer10, BattleRoyale.soloPlayer10Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer11 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer11) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer11, BattleRoyale.soloPlayer11Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer12 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer12) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer12, BattleRoyale.soloPlayer12Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer13 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer13) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer13, BattleRoyale.soloPlayer13Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer14 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer14) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer14, BattleRoyale.soloPlayer14Lifes, Sheriff.color);
                            }
                            if (BattleRoyale.soloPlayer15 != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.soloPlayer15) {
                                AddBattleRoyaleLivesTag(BattleRoyale.soloPlayer15, BattleRoyale.soloPlayer15Lifes, Sheriff.color);
                            }
                        }
                    }
                    else {
                        // show the remaining lives of all players if you're dead
                        if (PlayerInCache.LocalPlayer.PlayerControl.Data.IsDead) {
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer01, BattleRoyale.limePlayer01Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer02, BattleRoyale.limePlayer02Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer03, BattleRoyale.limePlayer03Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer04, BattleRoyale.limePlayer04Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer05, BattleRoyale.limePlayer05Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer06, BattleRoyale.limePlayer06Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer07, BattleRoyale.limePlayer07Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer01, BattleRoyale.pinkPlayer01Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer02, BattleRoyale.pinkPlayer02Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer03, BattleRoyale.pinkPlayer03Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer04, BattleRoyale.pinkPlayer04Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer05, BattleRoyale.pinkPlayer05Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer06, BattleRoyale.pinkPlayer06Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer07, BattleRoyale.pinkPlayer07Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.serialKiller, BattleRoyale.serialKillerLifes, Joker.color);
                        }
                        // if you're alive and in limeTeam, see the lives of your lime teammates
                        else if (BattleRoyale.limeTeam.Contains(PlayerInCache.LocalPlayer.PlayerControl)) {
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer01, BattleRoyale.limePlayer01Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer02, BattleRoyale.limePlayer02Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer03, BattleRoyale.limePlayer03Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer04, BattleRoyale.limePlayer04Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer05, BattleRoyale.limePlayer05Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer06, BattleRoyale.limePlayer06Lifes, FortuneTeller.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.limePlayer07, BattleRoyale.limePlayer07Lifes, FortuneTeller.color);
                        }
                        // if you're alive and in pinkTeam, see the lives of your pink teammates
                        else if (BattleRoyale.pinkTeam.Contains(PlayerInCache.LocalPlayer.PlayerControl)) {
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer01, BattleRoyale.pinkPlayer01Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer02, BattleRoyale.pinkPlayer02Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer03, BattleRoyale.pinkPlayer03Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer04, BattleRoyale.pinkPlayer04Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer05, BattleRoyale.pinkPlayer05Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer06, BattleRoyale.pinkPlayer06Lifes, Locksmith.color);
                            AddBattleRoyaleLivesTag(BattleRoyale.pinkPlayer07, BattleRoyale.pinkPlayer07Lifes, Locksmith.color);
                        }
                        // if you're the serial killer, only see your lives
                        else if (BattleRoyale.serialKiller != null && PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.serialKiller) {
                            AddBattleRoyaleLivesTag(BattleRoyale.serialKiller, BattleRoyale.serialKillerLifes, Joker.color);
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
                            Helpers.UpdateSenseiMap();
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
                Helpers.UpdateSenseiMap();
            }

            // If bomb, lights actives or special 1vs1 condition, prevent sabotage open map
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal && gameType <= 1 && PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor && MapBehaviour.Instance != null && MapBehaviour.Instance.IsOpen && (alivePlayers <= 2 || Bomberman.activeBomb || Challenger.isDueling || Seeker.isMinigaming || Illusionist.lightsOutTimer > 0 || Monja.awakened)) {
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
                            player.MyPhysics.Speed = Math.Max(1.5f, Math.Min(2.5f, 2.5f * oxygenTask.reactor.Countdown / oxygenTask.reactor.LifeSuppDuration));
                        }
                    }
                }
            }
        }
        static void updateImpostorKillButton(HudManager __instance) {
            if (!PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor || MeetingHud.Instance || MapBehaviour.Instance != null && MapBehaviour.Instance.IsOpen) return;
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
                        if (Manipulator.manipulatedVictimTimer <= 0) {
                            Manipulator.manipulatedVictim.MurderPlayer(Manipulator.manipulatedVictim, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                        }
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
                    if (Spiritualist.revivedPlayer != null && !MeetingHud.Instance && !Seeker.isMinigaming && !Challenger.isDueling && Spiritualist.revivedPlayer.CanMove) {
                        Spiritualist.revivedPlayerTimer -= deltaTime;
                        Spiritualist.revivedPlayerTimerCountButtonText.text = $"{Spiritualist.revivedPlayerKiller.name + " : " + Spiritualist.revivedPlayerTimer.ToString("F0")}";
                        if (Spiritualist.revivedPlayerTimer <= 0) {
                            RPCProcedure.murderSpiritualistRevivedPlayer();
                        }
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
                            LasMonjas.triggerGamemodesDrawWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GamemodesDrawWin, false);
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
                            LasMonjas.triggerGamemodesDrawWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GamemodesDrawWin, false);
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
                        progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" + Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0");
                        progress.GetComponent<ProgressTracker>().curValue = Mathf.Lerp(PlayerInCache.AllPlayers.Count - 1, 0, progressStart / progressEnd);
                    }
                    if (gamemodeMatchDuration <= 0) {
                        if (BattleRoyale.matchType == 2) {
                            // all teams with same points = Draw
                            if (BattleRoyale.limePoints == BattleRoyale.pinkPoints && BattleRoyale.pinkPoints == BattleRoyale.serialKillerPoints) {
                                LasMonjas.triggerGamemodesDrawWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GamemodesDrawWin, false);
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
                                LasMonjas.triggerGamemodesDrawWin = true;
                                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GamemodesDrawWin, false);
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
                        // all teams with same points = Draw
                        if (MonjaFestival.greenPoints == MonjaFestival.cyanPoints && MonjaFestival.cyanPoints == MonjaFestival.bigMonjaPoints) {
                            LasMonjas.triggerGamemodesDrawWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GamemodesDrawWin, false);
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
                            LasMonjas.triggerGamemodesDrawWin = true;
                            GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GamemodesDrawWin, false);
                        }
                    }
                    break;
            }
        }

        static void janitorUpdate() {
            Helpers.UpdateDraggedBody(Janitor.janitor, Janitor.dragginBody, Janitor.bodyId);
        }
        static void chameleonUpdate() {

            if (Chameleon.chameleon == null) return;

            Chameleon.chameleonTimer -= Time.deltaTime;

            if (Chameleon.chameleonTimer > 0f) {
                if (Chameleon.chameleon == PlayerInCache.LocalPlayer.PlayerControl) {
                    Helpers.alphaPlayer(Chameleon.chameleon.PlayerId, 0.5f);
                }
                else {
                    Helpers.alphaPlayer(Chameleon.chameleon.PlayerId, 0);
                }
            }

            // Chameleon reset
            if (Chameleon.chameleonTimer <= 0f) {
                Chameleon.resetChameleon();
            }
        }
        static void bountyHunterResetTargetIfDisconnect() {
            if (BountyHunter.bountyhunter == null) return;

            if (BountyHunter.usedTarget && BountyHunter.hasToKill.Data.Disconnected && BountyHunter.bountyhunter == PlayerInCache.LocalPlayer.PlayerControl && !BountyHunter.bountyhunter.Data.IsDead) {
                BountyHunter.hasToKill = null;
                BountyHunter.rolName = "";
                BountyHunter.targetNameButtonText.text = BountyHunter.rolName;
                BountyHunter.usedTarget = false;
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

            GameObject emerButton;

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    emerButton = GameObject.Find("EmergencyConsole");
                    if (activatedDleks) {
                        body.transform.position = emerButton.transform.position + new Vector3(-2.02f, 0f, -0.5f);
                    }
                    else {
                        body.transform.position = emerButton.transform.position + new Vector3(2.02f, 0f, -0.5f);
                    }
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
                        Helpers.alphaPlayer(Stranded.stranded.PlayerId, 0.5f);
                    }
                    else {
                        Helpers.alphaPlayer(Stranded.stranded.PlayerId, 0);
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

                        if (Helpers.isSubmergedMap()) {
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

                            if (Helpers.isSubmergedMap()) {
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

                    if (Helpers.isSubmergedMap()) {
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
                            if (p.PlayerId == player.PlayerId) {
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
        static void vigilantMiraUpdate() {

            if (Vigilant.vigilant == null || Vigilant.vigilant.Data.IsDead || Vigilant.vigilant != PlayerInCache.LocalPlayer.PlayerControl || GameOptionsManager.Instance.currentGameOptions.MapId != 1) {
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

                        if (!PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor) {
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
            Helpers.UpdateDraggedBody(Necromancer.necromancer, Necromancer.dragginBody, Necromancer.bodyId);
        }
        static void captureTheFlagUpdate() {

            if (gameType != 2)
                return;

            if (CaptureTheFlag.redPlayerWhoHasBlueFlag != null && CaptureTheFlag.redPlayerWhoHasBlueFlag.Data.Disconnected) {
                CaptureTheFlag.blueflag.transform.parent = CaptureTheFlag.blueflagbase.transform.parent;
                CaptureTheFlag.blueflag.transform.position = Helpers.CTFblueFlagPos;
                CaptureTheFlag.blueflagtaken = false;
                CaptureTheFlag.redPlayerWhoHasBlueFlag = null;
            }

            if (CaptureTheFlag.bluePlayerWhoHasRedFlag != null && CaptureTheFlag.bluePlayerWhoHasRedFlag.Data.Disconnected) {
                CaptureTheFlag.redflag.transform.parent = CaptureTheFlag.redflagbase.transform.parent;
                CaptureTheFlag.redflag.transform.position = Helpers.CTFredFlagPos;
                CaptureTheFlag.redflagtaken = false;
                CaptureTheFlag.bluePlayerWhoHasRedFlag = null;
            }
        }

        private static void policeandthiefHandleDisconnectedThief(PlayerControl disconnectedThief, PlayerControl thiefSlot, bool isStealing, byte jewelId) {
            if (thiefSlot == null) return;

            if (disconnectedThief.PlayerId != thiefSlot.PlayerId) return;

            if (isStealing) {
                RPCProcedure.policeandThiefRevertedJewelPosition(disconnectedThief.PlayerId, jewelId);
            }

            PoliceAndThief.thiefTeam.Remove(thiefSlot);
        }

        static void policeandthiefUpdate() {

            if (gameType != 3)
                return;

            // Check number of thiefs if a thief disconnects
            foreach (PlayerControl thief in PoliceAndThief.thiefTeam) {
                if (thief.Data.Disconnected) {
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer01, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer01JewelId);
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer02, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer02JewelId);
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer03, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer03JewelId);
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer04, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer04JewelId);
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer05, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer05JewelId);
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer06, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer06JewelId);
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer07, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer07JewelId);
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer08, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer08JewelId);
                    policeandthiefHandleDisconnectedThief(thief, PoliceAndThief.thiefplayer09, PoliceAndThief.stealingPlayers.Contains(thief), PoliceAndThief.thiefplayer09JewelId);

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
                    PoliceAndThief.policeTeam.Remove(police);

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
                if (Helpers.isSubmergedMap()) {
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
                KingOfTheHill.revivingPlayers.Remove(KingOfTheHill.greenKingplayer);

                // Remove minion player from new king
                Helpers.KOTHRemoveMinionPlayer(KingOfTheHill.greenTeam[0], true);

                KingOfTheHill.greenTeam.RemoveAt(0);
                KingOfTheHill.greenTeam.Add(KingOfTheHill.greenKingplayer);
                return;
            }

            if (KingOfTheHill.yellowKingplayer != null && KingOfTheHill.yellowKingplayer.Data.Disconnected) {
                KingOfTheHill.yellowTeam.Remove(KingOfTheHill.yellowKingplayer);
                KingOfTheHill.yellowKingplayer = null;
                KingOfTheHill.yellowKingplayer = KingOfTheHill.yellowTeam[0];
                if (Helpers.isSubmergedMap()) {
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
                KingOfTheHill.revivingPlayers.Remove(KingOfTheHill.yellowKingplayer);

                // Remove minion player from new king
                Helpers.KOTHRemoveMinionPlayer(KingOfTheHill.yellowTeam[0], false);

                KingOfTheHill.yellowTeam.RemoveAt(0);
                KingOfTheHill.yellowTeam.Add(KingOfTheHill.yellowKingplayer);
                return;
            }
        }

        private static int hotPotatoAlivePlayers() {
            int alivePlayers = -1;

            HotPotato.notPotatoTeamAlive.Clear();

            foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                if (!player.Data.IsDead) {
                    alivePlayers += 1;
                    HotPotato.notPotatoTeamAlive.Add(player);
                }
            }

            return alivePlayers;
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

                int notPotatosAlives = hotPotatoAlivePlayers();

                if (notPotatosAlives < 1) {
                    HotPotato.triggerHotPotatoEnd = true;
                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HotPotatoEnd, false);
                }

                HotPotato.hotPotatoPlayer = HotPotato.notPotatoTeam[0];
                Helpers.RestoreBodyTypeWithDelay(HotPotato.hotPotatoPlayer);
                HotPotato.hotPotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                HotPotato.hotPotato.transform.parent = HotPotato.hotPotatoPlayer.transform;

                // If hot potato timed out, assing new potato
                Helpers.RemoveNotPotato(HotPotato.notPotatoTeam[0]);

                HotPotato.notPotatoTeam.RemoveAt(0);

                hotPotatoButton.Timer = HotPotato.transferCooldown;

                Helpers.showGamemodesPopUp(1, Helpers.playerById(HotPotato.hotPotatoPlayer.PlayerId));
                HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + notPotatosAlives + "</color>";
            }

            // If notpotato disconnects, check number of notpotatos
            foreach (PlayerControl notPotato in HotPotato.notPotatoTeam) {
                if (notPotato.Data.Disconnected) {

                    int notPotatosAlives = hotPotatoAlivePlayers();

                    if (notPotatosAlives < 1) {
                        HotPotato.triggerHotPotatoEnd = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HotPotatoEnd, false);
                    }

                    HotPotato.notPotatoTeam.Remove(notPotato);

                    HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + notPotatosAlives + "</color>";
                    break;
                }
            }
        }

        private static void zombieLaboratoryHandleInfectedSurvivor(PlayerControl survivor, bool hasKeyItem, byte foundBox) {
            if (survivor == null) return;

            if (hasKeyItem) {
                RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, foundBox);
            }

            int survivorIndex = ZombieLaboratory.survivorTeam.IndexOf(survivor);

            if (survivorIndex != -1) {
                ZombieLaboratory.survivorTeam.RemoveAt(survivorIndex);
                ZombieLaboratory.survivorTeamCurrentargets.RemoveAt(survivorIndex);
            }
            ZombieLaboratory.infectedPlayers.Remove(survivor);

            RPCProcedure.zombieLaboratoryTurnZombie(survivor.PlayerId);

            if (survivor == PlayerInCache.LocalPlayer.PlayerControl && ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(false);

                if (Helpers.isSubmergedMap()) {
                    ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(false);
                }
            }
        }

        private static void zombieLaboratoryHandleDisconnectedSurvivor(PlayerControl survivor, bool isInfected, byte foundBox) {
            if (survivor == null) return;

            if (isInfected) {
                ZombieLaboratory.infectedPlayers.Remove(survivor);
            }

            ZombieLaboratory.survivorTeam.Remove(survivor);
            RPCProcedure.zombieLaboratoryRevertedKeyPosition(survivor.PlayerId, foundBox);
        }

        static void zombieLaboratoryUpdate() {

            if (gameType != 6)
                return;

            var deltaTime = Time.deltaTime;
            // Check timers for survivors
            if (ZombieLaboratory.survivorPlayer01 != null && ZombieLaboratory.survivorPlayer01Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer01)) {
                ZombieLaboratory.survivorPlayer01Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer01Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer01, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer01), ZombieLaboratory.survivorPlayer01FoundBox);

                    ZombieLaboratory.survivorPlayer01 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer01);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer01);
                }
            }
            if (ZombieLaboratory.survivorPlayer02 != null && ZombieLaboratory.survivorPlayer02Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer02)) {
                ZombieLaboratory.survivorPlayer02Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer02Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer02, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer02), ZombieLaboratory.survivorPlayer02FoundBox);

                    ZombieLaboratory.survivorPlayer02 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer02);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer02);
                }
            }
            if (ZombieLaboratory.survivorPlayer03 != null && ZombieLaboratory.survivorPlayer03Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer03)) {
                ZombieLaboratory.survivorPlayer03Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer03Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer03, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer03), ZombieLaboratory.survivorPlayer03FoundBox);

                    ZombieLaboratory.survivorPlayer03 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer03);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer03);
                }
            }
            if (ZombieLaboratory.survivorPlayer04 != null && ZombieLaboratory.survivorPlayer04Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer04)) {
                ZombieLaboratory.survivorPlayer04Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer04Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer04, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer04), ZombieLaboratory.survivorPlayer04FoundBox);

                    ZombieLaboratory.survivorPlayer04 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer04);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer04);
                }
            }
            if (ZombieLaboratory.survivorPlayer05 != null && ZombieLaboratory.survivorPlayer05Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer05)) {
                ZombieLaboratory.survivorPlayer05Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer05Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer05, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer05), ZombieLaboratory.survivorPlayer05FoundBox);

                    ZombieLaboratory.survivorPlayer05 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer05);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer05);
                }
            }
            if (ZombieLaboratory.survivorPlayer06 != null && ZombieLaboratory.survivorPlayer06Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer06)) {
                ZombieLaboratory.survivorPlayer06Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer06Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer06, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer06), ZombieLaboratory.survivorPlayer06FoundBox);

                    ZombieLaboratory.survivorPlayer06 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer06);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer06);
                }
            }
            if (ZombieLaboratory.survivorPlayer07 != null && ZombieLaboratory.survivorPlayer07Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer07)) {
                ZombieLaboratory.survivorPlayer07Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer07Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer07, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer07), ZombieLaboratory.survivorPlayer07FoundBox);

                    ZombieLaboratory.survivorPlayer07 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer07);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer07);
                }
            }
            if (ZombieLaboratory.survivorPlayer08 != null && ZombieLaboratory.survivorPlayer08Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer08)) {
                ZombieLaboratory.survivorPlayer08Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer08Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer08, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer08), ZombieLaboratory.survivorPlayer08FoundBox);

                    ZombieLaboratory.survivorPlayer08 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer08);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer08);
                }
            }
            if (ZombieLaboratory.survivorPlayer09 != null && ZombieLaboratory.survivorPlayer09Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer09)) {
                ZombieLaboratory.survivorPlayer09Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer09Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer09, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer09), ZombieLaboratory.survivorPlayer09FoundBox);

                    ZombieLaboratory.survivorPlayer09 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer09);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer09);
                }
            }
            if (ZombieLaboratory.survivorPlayer10 != null && ZombieLaboratory.survivorPlayer10Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer10)) {
                ZombieLaboratory.survivorPlayer10Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer10Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer10, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer10), ZombieLaboratory.survivorPlayer10FoundBox);

                    ZombieLaboratory.survivorPlayer10 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer10);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer10);
                }
            }
            if (ZombieLaboratory.survivorPlayer11 != null && ZombieLaboratory.survivorPlayer11Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer11)) {
                ZombieLaboratory.survivorPlayer11Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer11Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer11, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer11), ZombieLaboratory.survivorPlayer11FoundBox);

                    ZombieLaboratory.survivorPlayer11 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer11);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer11);
                }
            }
            if (ZombieLaboratory.survivorPlayer12 != null && ZombieLaboratory.survivorPlayer12Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer12)) {
                ZombieLaboratory.survivorPlayer12Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer12Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer12, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer12), ZombieLaboratory.survivorPlayer12FoundBox);

                    ZombieLaboratory.survivorPlayer12 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer12);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer12);
                }
            }
            if (ZombieLaboratory.survivorPlayer13 != null && ZombieLaboratory.survivorPlayer13Timer > 0 && ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer13)) {
                ZombieLaboratory.survivorPlayer13Timer -= deltaTime;
                if (ZombieLaboratory.survivorPlayer13Timer <= 0) {
                    // Remove Survivor role
                    zombieLaboratoryHandleInfectedSurvivor(ZombieLaboratory.survivorPlayer13, ZombieLaboratory.hasKeyItemPlayers.Contains(ZombieLaboratory.survivorPlayer13), ZombieLaboratory.survivorPlayer13FoundBox);

                    ZombieLaboratory.survivorPlayer13 = null;
                    ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer13);
                    ZombieLaboratory.hasKeyItemPlayers.Remove(ZombieLaboratory.survivorPlayer13);
                }
            }

            // Check number of survivors if a survivor disconnects
            foreach (PlayerControl survivor in ZombieLaboratory.survivorTeam) {
                if (survivor.Data.Disconnected) {

                    if (ZombieLaboratory.nursePlayer != null && survivor.PlayerId == ZombieLaboratory.nursePlayer.PlayerId) {
                        ZombieLaboratory.survivorTeam.Remove(ZombieLaboratory.nursePlayer);
                        ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedPlayers.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                        ZombieLaboratory.triggerZombieWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ZombieWin, false);
                    }
                    else if (ZombieLaboratory.survivorPlayer01 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer01.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer01, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer01), ZombieLaboratory.survivorPlayer01FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer01);
                    }
                    else if (ZombieLaboratory.survivorPlayer02 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer02.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer02, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer02), ZombieLaboratory.survivorPlayer02FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer02);
                    }
                    else if (ZombieLaboratory.survivorPlayer03 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer03.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer03, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer03), ZombieLaboratory.survivorPlayer03FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer03);
                    }
                    else if (ZombieLaboratory.survivorPlayer04 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer04.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer04, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer04), ZombieLaboratory.survivorPlayer04FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer04);
                    }
                    else if (ZombieLaboratory.survivorPlayer05 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer05.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer05, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer05), ZombieLaboratory.survivorPlayer05FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer05);
                    }
                    else if (ZombieLaboratory.survivorPlayer06 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer06.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer06, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer06), ZombieLaboratory.survivorPlayer06FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer06);
                    }
                    else if (ZombieLaboratory.survivorPlayer07 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer07.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer07, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer07), ZombieLaboratory.survivorPlayer07FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer07);
                    }
                    else if (ZombieLaboratory.survivorPlayer08 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer08.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer08, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer08), ZombieLaboratory.survivorPlayer08FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer08);
                    }
                    else if (ZombieLaboratory.survivorPlayer09 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer09.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer09, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer09), ZombieLaboratory.survivorPlayer09FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer09);
                    }
                    else if (ZombieLaboratory.survivorPlayer10 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer10.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer10, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer10), ZombieLaboratory.survivorPlayer10FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer10);
                    }
                    else if (ZombieLaboratory.survivorPlayer11 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer11.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer11, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer11), ZombieLaboratory.survivorPlayer11FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer11);
                    }
                    else if (ZombieLaboratory.survivorPlayer12 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer12.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer12, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer12), ZombieLaboratory.survivorPlayer12FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer12);
                    }
                    else if (ZombieLaboratory.survivorPlayer13 != null && survivor.PlayerId == ZombieLaboratory.survivorPlayer13.PlayerId) {
                        zombieLaboratoryHandleDisconnectedSurvivor(ZombieLaboratory.survivorPlayer13, ZombieLaboratory.infectedPlayers.Contains(ZombieLaboratory.survivorPlayer13), ZombieLaboratory.survivorPlayer13FoundBox);

                        ZombieLaboratory.infectedPlayers.Remove(ZombieLaboratory.survivorPlayer13);
                    }

                    // Check win condition
                    ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedPlayers.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                    if (ZombieLaboratory.survivorTeam.Count == 1) {
                        ZombieLaboratory.triggerZombieWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ZombieWin, false);
                    }
                    break;
                }
            }

            foreach (PlayerControl zombie in ZombieLaboratory.zombieTeam) {
                if (zombie.Data.Disconnected) {
                    ZombieLaboratory.zombieTeam.Remove(zombie);

                    // Check win condition
                    ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedPlayers.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                    if (ZombieLaboratory.zombieTeam.Count <= 0) {
                        ZombieLaboratory.triggerSurvivorWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.SurvivorWin, false);
                    }
                    break;
                }
            }
        }

        private static int gamemodeCountAlivePlayers(IEnumerable<PlayerControl> team) {
            int alive = 0;

            foreach (PlayerControl player in team) {
                if (!player.Data.IsDead) {
                    alive++;
                }
            }

            return alive;
        }

        static void battleRoyaleUpdate() {

            if (gameType != 7)
                return;

            if (BattleRoyale.matchType == 0) {
                // If solo player disconnects, check number of players
                foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                    if (soloPlayer.Data.Disconnected) {
                        BattleRoyale.soloPlayerTeam.Remove(soloPlayer);

                        int soloPlayersAlives = gamemodeCountAlivePlayers(BattleRoyale.soloPlayerTeam);

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
                        BattleRoyale.limeTeam.Remove(limePlayer);

                        int limePlayersAlive = gamemodeCountAlivePlayers(BattleRoyale.limeTeam);

                        int pinkPlayersAlive = gamemodeCountAlivePlayers(BattleRoyale.pinkTeam);

                        if (BattleRoyale.serialKiller != null) {

                            int serialKillerAlive = BattleRoyale.serialKiller != null && !BattleRoyale.serialKiller.Data.IsDead ? 1 : 0;

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
                        BattleRoyale.pinkTeam.Remove(pinkPlayer);

                        int limePlayersAlive = gamemodeCountAlivePlayers(BattleRoyale.limeTeam);

                        int pinkPlayersAlive = gamemodeCountAlivePlayers(BattleRoyale.pinkTeam);

                        if (BattleRoyale.serialKiller != null) {

                            int serialKillerAlive = BattleRoyale.serialKiller != null && !BattleRoyale.serialKiller.Data.IsDead ? 1 : 0;

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

                    int limePlayersAlive = gamemodeCountAlivePlayers(BattleRoyale.limeTeam);

                    int pinkPlayersAlive = gamemodeCountAlivePlayers(BattleRoyale.pinkTeam);

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
                        Helpers.alphaPlayer(MonjaFestival.bigMonjaPlayer.PlayerId, 0.5f);
                    }
                    else {
                        Helpers.alphaPlayer(MonjaFestival.bigMonjaPlayer.PlayerId, 0);
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
                    MonjaFestival.greenTeam.Remove(greenPlayer);

                    int greenPlayersAlive = gamemodeCountAlivePlayers(MonjaFestival.greenTeam);

                    int cyanPlayersAlive = gamemodeCountAlivePlayers(MonjaFestival.cyanTeam);

                    if (MonjaFestival.bigMonjaPlayer != null) {

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
                    MonjaFestival.cyanTeam.Remove(cyanPlayer);

                    int greenPlayersAlive = gamemodeCountAlivePlayers(MonjaFestival.greenTeam);

                    int cyanPlayersAlive = gamemodeCountAlivePlayers(MonjaFestival.cyanTeam);

                    if (MonjaFestival.bigMonjaPlayer != null) {

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

                int greenPlayersAlive = gamemodeCountAlivePlayers(MonjaFestival.greenTeam);

                int cyanPlayersAlive = gamemodeCountAlivePlayers(MonjaFestival.cyanTeam);

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

        public static IEnumerator monjaBigOneReload() { //get current points                / set current points            / max Points / reload Time  / isReloading                              / which spawn              / spawn text                    / spawn sprite
            return Helpers.MonjaSpawnReload(() => MonjaFestival.bigSpawnOnePoints, value => MonjaFestival.bigSpawnOnePoints = value, 30, 10f, value => MonjaFestival.bigSpawnOneReloading = value, MonjaFestival.bigSpawnOne, MonjaFestival.bigSpawnOneCount, CustomMain.customAssets.bigSpawnOneFull.GetComponent<SpriteRenderer>().sprite);
        }

        public static IEnumerator monjaBigTwoReload() {
            return Helpers.MonjaSpawnReload(() => MonjaFestival.bigSpawnTwoPoints, value => MonjaFestival.bigSpawnTwoPoints = value, 30, 10f, value => MonjaFestival.bigSpawnTwoReloading = value, MonjaFestival.bigSpawnTwo, MonjaFestival.bigSpawnTwoCount, CustomMain.customAssets.bigSpawnOneFull.GetComponent<SpriteRenderer>().sprite);
        }

        public static IEnumerator monjaLittleOneReload() {
            return Helpers.MonjaSpawnReload(() => MonjaFestival.littleSpawnOnePoints, value => MonjaFestival.littleSpawnOnePoints = value, 10, 20f, value => MonjaFestival.littleSpawnOneReloading = value, MonjaFestival.littleSpawnOne, MonjaFestival.littleSpawnOneCount, CustomMain.customAssets.littleSpawnOneFull.GetComponent<SpriteRenderer>().sprite);
        }

        public static IEnumerator monjaLittleTwoReload() {
            return Helpers.MonjaSpawnReload(() => MonjaFestival.littleSpawnTwoPoints, value => MonjaFestival.littleSpawnTwoPoints = value, 10, 20f, value => MonjaFestival.littleSpawnTwoReloading = value, MonjaFestival.littleSpawnTwo, MonjaFestival.littleSpawnTwoCount, CustomMain.customAssets.littleSpawnOneFull.GetComponent<SpriteRenderer>().sprite);
        }

        public static IEnumerator monjaLittleThreeReload() {
            return Helpers.MonjaSpawnReload(() => MonjaFestival.littleSpawnThreePoints, value => MonjaFestival.littleSpawnThreePoints = value, 10, 20f, value => MonjaFestival.littleSpawnThreeReloading = value, MonjaFestival.littleSpawnThree, MonjaFestival.littleSpawnThreeCount, CustomMain.customAssets.littleSpawnOneFull.GetComponent<SpriteRenderer>().sprite);
        }

        public static IEnumerator monjaLittleFourReload() {
            return Helpers.MonjaSpawnReload(() => MonjaFestival.littleSpawnFourPoints, value => MonjaFestival.littleSpawnFourPoints = value, 10, 20f, value => MonjaFestival.littleSpawnFourReloading = value, MonjaFestival.littleSpawnFour, MonjaFestival.littleSpawnFourCount, CustomMain.customAssets.littleSpawnOneFull.GetComponent<SpriteRenderer>().sprite);
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
            bountyHunterResetTargetIfDisconnect();

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