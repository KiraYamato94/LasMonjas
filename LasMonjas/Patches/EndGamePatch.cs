using HarmonyLib;
using static LasMonjas.LasMonjas;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text;
using LasMonjas.Core;

namespace LasMonjas.Patches {
    enum CustomGameOverReason {
        BombExploded = 10,
        TeamRenegadeWin = 11,
        BountyHunterWin = 12,
        TrapperWin = 13,
        YinyangerWin = 14,
        ChallengerWin = 15,
        NinjaWin = 16,
        BerserkerWin = 17,
        YandereWin = 18,
        StrandedWin = 19,
        MonjaWin = 20,
        JokerWin = 21,
        PyromaniacWin = 22,
        TreasureHunterWin = 23,
        DevourerWin = 24,
        PoisonerWin = 25,
        PuppeteerWin = 26,
        ExilerWin = 27,
        SeekerWin = 28,
        LoversWin = 29,
        KidLose = 30,
        TaskMasterCrewWin = 31,
        DrawTeamWin = 32,
        RedTeamFlagWin = 33,
        BlueTeamFlagWin = 34,
        ThiefModeThiefWin = 35,
        ThiefModePoliceWin = 36,
        TeamHillDraw = 37,
        GreenTeamHillWin = 38,
        YellowTeamHillWin = 39,
        HotPotatoEnd = 40,
        ZombieWin = 41,
        SurvivorWin = 42,
        BattleRoyaleSoloWin = 43,
        BattleRoyaleTimeWin = 44,
        BattleRoyaleDraw = 45,
        BattleRoyaleLimeTeamWin = 46,
        BattleRoyalePinkTeamWin = 47,
        BattleRoyaleSerialKillerWin = 48,
        MonjaFestivalGreenWin = 49,
        MonjaFestivalCyanWin = 50,
        MonjaFestivalBigMonjaWin = 51,
        MonjaFestivalDraw = 52
    }

    enum WinCondition {
        Default,
        BombExploded,
        RenegadeWin,
        BountyHunterWin,
        TrapperWin,
        YinyangerWin,
        ChallengerWin,
        NinjaWin,
        BerserkerWin,
        YandereWin,
        StrandedWin,
        MonjaWin,
        JokerWin,
        PyromaniacWin,
        TreasureHunterWin,
        DevourerWin,
        PoisonerWin,
        PuppeteerWin,
        ExilerWin,
        SeekerWin,
        LoversTeamWin,
        LoversSoloWin,
        KidLose,
        TaskMasterCrewWin,
        DrawTeamWin,
        RedTeamFlagWin,
        BlueTeamFlagWin,
        ThiefModeThiefWin,
        ThiefModePoliceWin,
        TeamHillDraw,
        GreenTeamHillWin,
        YellowTeamHillWin,
        HotPotatoEnd,
        ZombieWin,
        SurvivorWin,
        BattleRoyaleSoloWin,
        BattleRoyaleTimeWin,
        BattleRoyaleDraw,
        BattleRoyaleLimeTeamWin,
        BattleRoyalePinkTeamWin,
        BattleRoyaleSerialKillerWin,
        MonjaFestivalGreenWin,
        MonjaFestivalCyanWin,
        MonjaFestivalBigMonjaWin,
        MonjaFestivalDraw
    }

    static class AdditionalTempData {

        public static WinCondition winCondition = WinCondition.Default;
        public static List<PlayerRoleInfo> playerRoles = new List<PlayerRoleInfo>();

        public static void clear() {
            playerRoles.Clear();
            winCondition = WinCondition.Default;
        }

        internal class PlayerRoleInfo {
            public string PlayerName { get; set; }
            public List<RoleInfo> Roles {get;set;}
            public int TasksCompleted  {get;set;}
            public int TasksTotal  {get;set;}
            public int? Kills { get; set; }
        }
    }


    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public class OnGameEndPatch {
        private static GameOverReason gameOverReason;
        public static void Prefix(AmongUsClient __instance, [HarmonyArgument(0)]ref EndGameResult endGameResult) {
            gameOverReason = endGameResult.GameOverReason;
            if ((int)endGameResult.GameOverReason >= 10) endGameResult.GameOverReason = GameOverReason.ImpostorsByKill;

            // Reset zoomed out ghosts
            Helpers.toggleZoom(reset: true);
        }

        public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)]ref EndGameResult endGameResult) {
            AdditionalTempData.clear();

            foreach(var playerControl in PlayerInCache.AllPlayers) {
                var roles = RoleInfo.getRoleInfoForPlayer(playerControl);
                var (tasksCompleted, tasksTotal) = TasksHandler.taskInfo(playerControl.PlayerControl.Data);
                int? killCount = GameHistory.deadPlayers.FindAll(x => x.killerIfExisting != null && x.killerIfExisting.PlayerId == playerControl.PlayerId).Count;

                if (gameType >= 2 || killCount == 0 && !(new List<RoleInfo>() { RoleInfo.devourer, RoleInfo.sheriff, RoleInfo.welder, RoleInfo.taskMaster, RoleInfo.renegade, RoleInfo.minion, RoleInfo.bountyHunter, RoleInfo.trapper, RoleInfo.yinyanger, RoleInfo.challenger, RoleInfo.ninja, RoleInfo.berserker, RoleInfo.yandere, RoleInfo.stranded, RoleInfo.monja }.Contains(RoleInfo.getRoleInfoForPlayer(playerControl).FirstOrDefault()) || playerControl.PlayerControl.Data.Role.IsImpostor)) {
                    killCount = null;
                }

                AdditionalTempData.playerRoles.Add(new AdditionalTempData.PlayerRoleInfo() { PlayerName = playerControl.PlayerControl.Data.PlayerName, Roles = roles, TasksTotal = tasksTotal, TasksCompleted = tasksCompleted, Kills = killCount });
            }

            // Remove rebel roles from winners
            List<PlayerControl> notWinners = new List<PlayerControl>();
            if (Renegade.renegade != null) notWinners.Add(Renegade.renegade);
            if (Minion.minion != null) notWinners.Add(Minion.minion);
            notWinners.AddRange(Renegade.formerRenegades);
            if (BountyHunter.bountyhunter != null) notWinners.Add(BountyHunter.bountyhunter);
            if (Trapper.trapper != null) notWinners.Add(Trapper.trapper);
            if (Yinyanger.yinyanger != null) notWinners.Add(Yinyanger.yinyanger);
            if (Challenger.challenger != null) notWinners.Add(Challenger.challenger);
            if (Ninja.ninja != null) notWinners.Add(Ninja.ninja);
            if (Berserker.berserker != null) notWinners.Add(Berserker.berserker);
            if (Yandere.yandere != null) notWinners.Add(Yandere.yandere);
            if (Stranded.stranded != null) notWinners.Add(Stranded.stranded);
            if (Monja.monja != null) notWinners.Add(Monja.monja);

            // Remove neutral roles from winners
            if (Joker.joker != null) notWinners.Add(Joker.joker);
            if (RoleThief.rolethief != null) notWinners.Add(RoleThief.rolethief);
            if (Pyromaniac.pyromaniac != null) notWinners.Add(Pyromaniac.pyromaniac);
            if (TreasureHunter.treasureHunter != null) notWinners.Add(TreasureHunter.treasureHunter);
            if (Devourer.devourer != null) notWinners.Add(Devourer.devourer);
            if (Poisoner.poisoner != null) notWinners.Add(Poisoner.poisoner);
            if (Puppeteer.puppeteer != null) notWinners.Add(Puppeteer.puppeteer);
            if (Exiler.exiler != null) notWinners.Add(Exiler.exiler);
            if (Amnesiac.amnesiac != null) notWinners.Add(Amnesiac.amnesiac);
            if (Seeker.seeker != null) notWinners.Add(Seeker.seeker);

            // Remove neutral custom gamemode roles from winners
            if (CaptureTheFlag.stealerPlayer != null) notWinners.Add(CaptureTheFlag.stealerPlayer);
            if (KingOfTheHill.usurperPlayer != null) notWinners.Add(KingOfTheHill.usurperPlayer);
            if (HotPotato.hotPotatoPlayer != null) notWinners.Add(HotPotato.hotPotatoPlayer);

            List<CachedPlayerData> winnersToRemove = new List<CachedPlayerData>();
            //foreach (CachedPlayerData winner in EndGameResult.CachedWinners.GetFastEnumerator()) {
            foreach (CachedPlayerData winner in EndGameResult.CachedWinners) {
                if (notWinners.Any(x => x.Data.PlayerName == winner.PlayerName)) winnersToRemove.Add(winner);
            }
            foreach (var winner in winnersToRemove) EndGameResult.CachedWinners.Remove(winner);

            bool kidLose = Kid.kid != null && gameOverReason == (GameOverReason)CustomGameOverReason.KidLose;
            bool bombExploded = Bomberman.bomberman != null && gameOverReason == (GameOverReason)CustomGameOverReason.BombExploded;
            bool teamRenegadeWin = gameOverReason == (GameOverReason)CustomGameOverReason.TeamRenegadeWin && ((Renegade.renegade != null && !Renegade.renegade.Data.IsDead) || (Minion.minion != null && !Minion.minion.Data.IsDead));
            bool bountyhunterWin = BountyHunter.bountyhunter != null && gameOverReason == (GameOverReason)CustomGameOverReason.BountyHunterWin;
            bool trapperWin = Trapper.trapper != null && gameOverReason == (GameOverReason)CustomGameOverReason.TrapperWin;
            bool yinyangerWin = Yinyanger.yinyanger != null && gameOverReason == (GameOverReason)CustomGameOverReason.YinyangerWin;
            bool challengerWin = Challenger.challenger != null && gameOverReason == (GameOverReason)CustomGameOverReason.ChallengerWin;
            bool ninjaWin = Ninja.ninja != null && gameOverReason == (GameOverReason)CustomGameOverReason.NinjaWin;
            bool berserkerWin = Berserker.berserker != null && gameOverReason == (GameOverReason)CustomGameOverReason.BerserkerWin;
            bool yandereWin = Yandere.yandere != null && gameOverReason == (GameOverReason)CustomGameOverReason.YandereWin;
            bool strandedWin = Stranded.stranded != null && gameOverReason == (GameOverReason)CustomGameOverReason.StrandedWin;
            bool monjaWin = Monja.monja != null && gameOverReason == (GameOverReason)CustomGameOverReason.MonjaWin;
            bool jokerWin = Joker.joker != null && gameOverReason == (GameOverReason)CustomGameOverReason.JokerWin;
            bool pyromaniacWin = Pyromaniac.pyromaniac != null && gameOverReason == (GameOverReason)CustomGameOverReason.PyromaniacWin;
            bool treasurehunterWin = TreasureHunter.treasureHunter != null && gameOverReason == (GameOverReason)CustomGameOverReason.TreasureHunterWin;
            bool devourerWin = Devourer.devourer != null && gameOverReason == (GameOverReason)CustomGameOverReason.DevourerWin;
            bool poisonerWin = Poisoner.poisoner != null && gameOverReason == (GameOverReason)CustomGameOverReason.PoisonerWin;
            bool puppeteerWin = Puppeteer.puppeteer != null && gameOverReason == (GameOverReason)CustomGameOverReason.PuppeteerWin;
            bool exilerWin = Exiler.exiler != null && gameOverReason == (GameOverReason)CustomGameOverReason.ExilerWin;
            bool seekerWin = Seeker.seeker != null && gameOverReason == (GameOverReason)CustomGameOverReason.SeekerWin;
            bool loversWin = Modifiers.existingAndAlive() && (gameOverReason == (GameOverReason)CustomGameOverReason.LoversWin || (GameManager.Instance.DidHumansWin(gameOverReason) && !Modifiers.existingWithKiller()));
            bool taskMasterCrewWin = TaskMaster.taskMaster != null && gameOverReason == (GameOverReason)CustomGameOverReason.TaskMasterCrewWin;
            bool drawTeamWin = gameType == 2 && gameOverReason == (GameOverReason)CustomGameOverReason.DrawTeamWin;
            bool redTeamFlagWin = gameType == 2 && gameOverReason == (GameOverReason)CustomGameOverReason.RedTeamFlagWin;
            bool blueTeamFlagWin = gameType == 2 && gameOverReason == (GameOverReason)CustomGameOverReason.BlueTeamFlagWin;
            bool thiefModeThiefWin = gameType == 3 && gameOverReason == (GameOverReason)CustomGameOverReason.ThiefModeThiefWin;
            bool thiefModePoliceWin = gameType == 3 && gameOverReason == (GameOverReason)CustomGameOverReason.ThiefModePoliceWin;
            bool teamHillDraw = gameType == 4 && gameOverReason == (GameOverReason)CustomGameOverReason.TeamHillDraw;
            bool greenTeamHillWin = gameType == 4 && gameOverReason == (GameOverReason)CustomGameOverReason.GreenTeamHillWin;
            bool yellowTeamHillWin = gameType == 4 && gameOverReason == (GameOverReason)CustomGameOverReason.YellowTeamHillWin;
            bool hotPotatoEnd = gameType == 5 && gameOverReason == (GameOverReason)CustomGameOverReason.HotPotatoEnd;
            bool zombieWin = gameType == 6 && gameOverReason == (GameOverReason)CustomGameOverReason.ZombieWin;
            bool survivorWin = gameType == 6 && gameOverReason == (GameOverReason)CustomGameOverReason.SurvivorWin;
            bool battleRoyaleSoloWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleSoloWin;
            bool battleRoyaleTimeWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleTimeWin;
            bool battleRoyaleDraw = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleDraw;
            bool battleRoyaleLimeTeamWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin;
            bool battleRoyalePinkTeamWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin;
            bool battleRoyaleSerialKillerWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleSerialKillerWin;
            bool monjaFestivalGreenWin = gameType == 8 && gameOverReason == (GameOverReason)CustomGameOverReason.MonjaFestivalGreenWin;
            bool monjaFestivalCyanWin = gameType == 8 && gameOverReason == (GameOverReason)CustomGameOverReason.MonjaFestivalCyanWin;
            bool monjaFestivalBigMonjaWin = gameType == 8 && gameOverReason == (GameOverReason)CustomGameOverReason.MonjaFestivalBigMonjaWin;
            bool monjaFestivalDraw = gameType == 8 && gameOverReason == (GameOverReason)CustomGameOverReason.MonjaFestivalDraw;

            // Kid lose
            if (kidLose) {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Kid.kid.Data);
                wpd.IsYou = false;
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.KidLose;
            }

            // Bomb exploded
            else if (bombExploded) {
                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl imimpostor in PlayerInCache.AllPlayers) {
                    if (imimpostor.Data.Role.IsImpostor == true) {
                        CachedPlayerData wpd = new CachedPlayerData(imimpostor.Data);
                        EndGameResult.CachedWinners.Add(wpd);
                    }
                }
                AdditionalTempData.winCondition = WinCondition.BombExploded;
            }

            // Lovers win conditions
            else if (loversWin) {
                // Double win for lovers with crewmates
                if (!Modifiers.existingWithKiller()) {
                    EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                    foreach (PlayerControl p in PlayerInCache.AllPlayers) {
                        if (p == null) continue;
                        if (p == Modifiers.lover1 || p == Modifiers.lover2)
                            EndGameResult.CachedWinners.Add(new CachedPlayerData(p.Data));
                        else if (p != Joker.joker && p != RoleThief.rolethief && p != Pyromaniac.pyromaniac && p != TreasureHunter.treasureHunter && p != Devourer.devourer && p != Poisoner.poisoner && p != Puppeteer.puppeteer && p != Exiler.exiler && p != Amnesiac.amnesiac && p != Seeker.seeker && p != Renegade.renegade && p != Minion.minion && !Renegade.formerRenegades.Contains(p) && p != BountyHunter.bountyhunter && p != Trapper.trapper && p != Yinyanger.yinyanger && p != Challenger.challenger && p != Ninja.ninja && p != Berserker.berserker && p != Yandere.yandere && p != Stranded.stranded && p != Monja.monja && !p.Data.Role.IsImpostor)
                            EndGameResult.CachedWinners.Add(new CachedPlayerData(p.Data));
                    }
                    AdditionalTempData.winCondition = WinCondition.LoversTeamWin;

                }
                // Lovers solo win
                else {
                    EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(Modifiers.lover1.Data));
                    EndGameResult.CachedWinners.Add(new CachedPlayerData(Modifiers.lover2.Data));
                    AdditionalTempData.winCondition = WinCondition.LoversSoloWin;
                }
            }

            // TaskMaster crew win
            else if (taskMasterCrewWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl p in PlayerInCache.AllPlayers) {
                    if (p == null) continue;
                    if (p != Joker.joker && p != RoleThief.rolethief && p != Pyromaniac.pyromaniac && p != TreasureHunter.treasureHunter && p != Devourer.devourer && p != Poisoner.poisoner && p != Puppeteer.puppeteer && p != Exiler.exiler && p != Amnesiac.amnesiac && p != Seeker.seeker && p != Renegade.renegade && p != Minion.minion && !Renegade.formerRenegades.Contains(p) && p != BountyHunter.bountyhunter && p != Trapper.trapper && p != Yinyanger.yinyanger && p != Challenger.challenger && p != Ninja.ninja && p != Berserker.berserker && p != Yandere.yandere && p != Stranded.stranded && p != Monja.monja && !p.Data.Role.IsImpostor)
                        EndGameResult.CachedWinners.Add(new CachedPlayerData(p.Data));
                }
                AdditionalTempData.winCondition = WinCondition.TaskMasterCrewWin;
            }

            // Joker win
            else if (jokerWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Joker.joker.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.JokerWin;
            }

            // Pyromaniac win
            else if (pyromaniacWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Pyromaniac.pyromaniac.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.PyromaniacWin;
            }

            // TreasureHunter win
            else if (treasurehunterWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(TreasureHunter.treasureHunter.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.TreasureHunterWin;
            }

            // Devourer win
            else if (devourerWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Devourer.devourer.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.DevourerWin;
            }

            // Poisoner win
            else if (poisonerWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Poisoner.poisoner.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.PoisonerWin;
            }

            // Puppeteer win
            else if (puppeteerWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Puppeteer.puppeteer.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.PuppeteerWin;
            }

            // Exiler win
            else if (exilerWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Exiler.exiler.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.ExilerWin;
            }

            // Seeker win
            else if (seekerWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Seeker.seeker.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.SeekerWin;
            }

            // Renegade win condition
            else if (teamRenegadeWin) {
                // Renegade wins if nobody except renegade is alive
                AdditionalTempData.winCondition = WinCondition.RenegadeWin;
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Renegade.renegade.Data);
                wpd.IsImpostor = false;
                EndGameResult.CachedWinners.Add(wpd);
                // If there is a minion. The minion also wins
                if (Minion.minion != null) {
                    CachedPlayerData wpdMinion = new CachedPlayerData(Minion.minion.Data);
                    wpdMinion.IsImpostor = false;
                    EndGameResult.CachedWinners.Add(wpdMinion);
                }
                foreach (var player in Renegade.formerRenegades) {
                    CachedPlayerData wpdFormerRenegade = new CachedPlayerData(player.Data);
                    wpdFormerRenegade.IsImpostor = false;
                    EndGameResult.CachedWinners.Add(wpdFormerRenegade);
                }
            }

            // BountyHunter win
            else if (bountyhunterWin) {
                // BountyHunter wins if he kills his target 
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(BountyHunter.bountyhunter.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.BountyHunterWin;
            }

            // Trapper win
            else if (trapperWin) {
                // Trapper wins if nobody except Trapper is alive
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Trapper.trapper.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.TrapperWin;
            }

            // Yinyanger win
            else if (yinyangerWin) {
                // Yinyanger wins if nobody except Yinyanger is alive
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Yinyanger.yinyanger.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.YinyangerWin;
            }

            // Challenger win
            else if (challengerWin) {
                // Challenger wins if nobody except Challenger is alive
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Challenger.challenger.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.ChallengerWin;
            }

            // Ninja win
            else if (ninjaWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Ninja.ninja.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.NinjaWin;
            }

            // Berserker win
            else if (berserkerWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Berserker.berserker.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.BerserkerWin;
            }

            // Yandere win
            else if (yandereWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Yandere.yandere.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.YandereWin;
            }

            // Stranded win
            else if (strandedWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Stranded.stranded.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.StrandedWin;
            }

            // Monja win
            else if (monjaWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(Monja.monja.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.MonjaWin;
            }

            // Flag Game Mode Win
            // Draw
            else if (drawTeamWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.DrawTeamWin;
            }
            // Red Team Win
            else if (redTeamFlagWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.RedTeamFlagWin;
            }
            // Blue Team Win
            else if (blueTeamFlagWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.BlueTeamFlagWin;
            }

            // Thief Mode Win
            // Thief Team Win
            else if (thiefModeThiefWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.ThiefModeThiefWin;
            }
            // Police Team Win
            else if (thiefModePoliceWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.ThiefModePoliceWin;
            }

            // King Game Mode Win
            // Draw
            else if (teamHillDraw) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.TeamHillDraw;
            }
            // Green Team Win
            else if (greenTeamHillWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.GreenTeamHillWin;
            }
            // Yellow Team Win
            else if (yellowTeamHillWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.YellowTeamHillWin;
            }

            // Hot Potato Game Mode Win
            else if (hotPotatoEnd) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in HotPotato.notPotatoTeamAlive) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.HotPotatoEnd;
            }

            // ZombieLaboratory zombie Win
            else if (zombieWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.ZombieWin;
            }
            else if (survivorWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.SurvivorWin;
            }

            // BattleRoyale Win
            else if (battleRoyaleSoloWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in BattleRoyale.soloPlayerTeam) {
                    if (!player.Data.IsDead) {
                        CachedPlayerData wpd = new CachedPlayerData(player.Data);
                        EndGameResult.CachedWinners.Add(wpd);
                    }
                }
                AdditionalTempData.winCondition = WinCondition.BattleRoyaleSoloWin;
            }
            // BattleRoyale Time Win
            else if (battleRoyaleTimeWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                if (BattleRoyale.matchType == 0) {
                    foreach (PlayerControl player in BattleRoyale.soloPlayerTeam) {
                        if (!player.Data.IsDead) {
                            CachedPlayerData wpd = new CachedPlayerData(player.Data);
                            EndGameResult.CachedWinners.Add(wpd);
                        }
                    }
                }
                else {
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        if (!player.Data.IsDead) {
                            CachedPlayerData wpd = new CachedPlayerData(player.Data);
                            EndGameResult.CachedWinners.Add(wpd);
                        }
                    }
                }
                AdditionalTempData.winCondition = WinCondition.BattleRoyaleTimeWin;
            }
            // BattleRoyale Lime Team Win
            else if (battleRoyaleLimeTeamWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in BattleRoyale.limeTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.BattleRoyaleLimeTeamWin;
            }
            // BattleRoyale Pink Team Win
            else if (battleRoyalePinkTeamWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.BattleRoyalePinkTeamWin;
            }
            // BattleRoyale Serial Killer Win
            else if (battleRoyaleSerialKillerWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(BattleRoyale.serialKiller.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.BattleRoyaleSerialKillerWin;
            }
            // BattleRoyale Draw
            else if (battleRoyaleDraw) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.BattleRoyaleDraw; 
            }

            // MonjaFestival Green Team Win
            else if (monjaFestivalGreenWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in MonjaFestival.greenTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.MonjaFestivalGreenWin;
            }
            // MonjaFestival Pink Team Win
            else if (monjaFestivalCyanWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in MonjaFestival.cyanTeam) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.MonjaFestivalCyanWin;
            }
            // MonjaFestival Big Monja Win
            else if (monjaFestivalBigMonjaWin) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                CachedPlayerData wpd = new CachedPlayerData(MonjaFestival.bigMonjaPlayer.Data);
                EndGameResult.CachedWinners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.MonjaFestivalBigMonjaWin;
            }
            // MonjaFestival Draw
            else if (monjaFestivalDraw) {
                                EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
                foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                    CachedPlayerData wpd = new CachedPlayerData(player.Data);
                    EndGameResult.CachedWinners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.MonjaFestivalDraw;
            }

            // Reset Settings
            RPCProcedure.resetVariables();
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public class EndGameManagerSetUpPatch {
        public static void Postfix(EndGameManager __instance) {

            GameObject bonusText = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            bonusText.transform.position = new Vector3(__instance.WinText.transform.position.x, __instance.WinText.transform.position.y - 0.5f, __instance.WinText.transform.position.z);
            bonusText.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            TMPro.TMP_Text textRenderer = bonusText.GetComponent<TMPro.TMP_Text>();
            textRenderer.text = "";

            switch (AdditionalTempData.winCondition) {
                case WinCondition.KidLose:
                    textRenderer.text = Language.endGameTexts[0];
                    textRenderer.color = Kid.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Kid.color);
                    Helpers.playEndMusic(5);
                    break;
                case WinCondition.BombExploded:
                    textRenderer.text = Language.endGameTexts[1];
                    textRenderer.color = Bomberman.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Bomberman.color);
                    Helpers.playEndMusic(6);
                    break;
                case WinCondition.LoversTeamWin:
                    textRenderer.text = Language.endGameTexts[2];
                    textRenderer.color = Modifiers.loverscolor;
                    __instance.BackgroundBar.material.SetColor("_Color", Modifiers.loverscolor);
                    Helpers.playEndMusic(5);
                    break;
                case WinCondition.LoversSoloWin:
                    textRenderer.text = Language.endGameTexts[3];
                    textRenderer.color = Modifiers.loverscolor;
                    __instance.BackgroundBar.material.SetColor("_Color", Modifiers.loverscolor);
                    Helpers.playEndMusic(6);
                    break;
                case WinCondition.TaskMasterCrewWin:
                    textRenderer.text = Language.endGameTexts[4];
                    textRenderer.color = TaskMaster.color;
                    __instance.BackgroundBar.material.SetColor("_Color", TaskMaster.color);
                    Helpers.playEndMusic(5);
                    break;
                case WinCondition.JokerWin:
                    textRenderer.text = Language.endGameTexts[5];
                    textRenderer.color = Joker.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Joker.color);
                    Helpers.playEndMusic(3);
                    break;
                case WinCondition.PyromaniacWin:
                    textRenderer.text = Language.endGameTexts[6];
                    textRenderer.color = Pyromaniac.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Pyromaniac.color);
                    Helpers.playEndMusic(3);
                    break;
                case WinCondition.TreasureHunterWin:
                    textRenderer.text = Language.endGameTexts[7];
                    textRenderer.color = TreasureHunter.color;
                    __instance.BackgroundBar.material.SetColor("_Color", TreasureHunter.color);
                    Helpers.playEndMusic(3);
                    break;
                case WinCondition.DevourerWin:
                    textRenderer.text = Language.endGameTexts[8];
                    textRenderer.color = Devourer.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Devourer.color);
                    Helpers.playEndMusic(3);
                    break;
                case WinCondition.PoisonerWin:
                    textRenderer.text = Language.endGameTexts[9];
                    textRenderer.color = Poisoner.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Poisoner.color);
                    Helpers.playEndMusic(3);
                    break;
                case WinCondition.PuppeteerWin:
                    textRenderer.text = Language.endGameTexts[10];
                    textRenderer.color = Puppeteer.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Puppeteer.color);
                    Helpers.playEndMusic(3);
                    break;
                case WinCondition.ExilerWin:
                    textRenderer.text = Language.endGameTexts[11];
                    textRenderer.color = Exiler.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Exiler.color);
                    Helpers.playEndMusic(3);
                    break;
                case WinCondition.SeekerWin:
                    textRenderer.text = Language.endGameTexts[12];
                    textRenderer.color = Seeker.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Seeker.color);
                    Helpers.playEndMusic(3);
                    break;
                case WinCondition.RenegadeWin:
                    textRenderer.text = Language.endGameTexts[13];
                    textRenderer.color = Renegade.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Renegade.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.BountyHunterWin:
                    textRenderer.text = Language.endGameTexts[14];
                    textRenderer.color = BountyHunter.color;
                    __instance.BackgroundBar.material.SetColor("_Color", BountyHunter.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.TrapperWin:
                    textRenderer.text = Language.endGameTexts[15];
                    textRenderer.color = Trapper.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Trapper.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.YinyangerWin:
                    textRenderer.text = Language.endGameTexts[16];
                    textRenderer.color = Yinyanger.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Yinyanger.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.ChallengerWin:
                    textRenderer.text = Language.endGameTexts[17];
                    textRenderer.color = Challenger.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Challenger.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.NinjaWin:
                    textRenderer.text = Language.endGameTexts[18];
                    textRenderer.color = Ninja.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Ninja.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.BerserkerWin:
                    textRenderer.text = Language.endGameTexts[19];
                    textRenderer.color = Berserker.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Berserker.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.YandereWin:
                    textRenderer.text = Language.endGameTexts[20];
                    textRenderer.color = Yandere.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Yandere.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.StrandedWin:
                    textRenderer.text = Language.endGameTexts[21];
                    textRenderer.color = Stranded.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Stranded.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.MonjaWin:
                    textRenderer.text = Language.endGameTexts[22];
                    textRenderer.color = Monja.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Monja.color);
                    Helpers.playEndMusic(4);
                    break;
                case WinCondition.DrawTeamWin:
                case WinCondition.TeamHillDraw:
                case WinCondition.BattleRoyaleDraw:
                case WinCondition.MonjaFestivalDraw:
                    textRenderer.text = Language.endGameTexts[23];
                    textRenderer.color = new Color32(255, 128, 0, byte.MaxValue);
                    __instance.BackgroundBar.material.SetColor("_Color", Joker.color);
                    break;
                case WinCondition.RedTeamFlagWin:
                    textRenderer.text = Language.endGameTexts[24];
                    textRenderer.color = Color.red;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.red);
                    break;
                case WinCondition.BlueTeamFlagWin:
                    textRenderer.text = Language.endGameTexts[25];
                    textRenderer.color = Color.blue;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.blue);
                    break;
                case WinCondition.ThiefModePoliceWin:
                    textRenderer.text = Language.endGameTexts[26];
                    textRenderer.color = Color.cyan;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.cyan);
                    break;
                case WinCondition.ThiefModeThiefWin:
                    textRenderer.text = Language.endGameTexts[27];
                    textRenderer.color = Mechanic.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Mechanic.color);
                    break;
                case WinCondition.GreenTeamHillWin:
                    textRenderer.text = Language.endGameTexts[28];
                    textRenderer.color = Color.green;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.green);
                    break;
                case WinCondition.YellowTeamHillWin:
                    textRenderer.text = Language.endGameTexts[29];
                    textRenderer.color = Color.yellow;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.yellow);
                    break;
                case WinCondition.HotPotatoEnd:
                    textRenderer.text = Language.endGameTexts[30];
                    textRenderer.color = Color.cyan;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.cyan);
                    break;
                case WinCondition.ZombieWin:
                    textRenderer.text = Language.endGameTexts[31];
                    textRenderer.color = Mechanic.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Mechanic.color);
                    break;
                case WinCondition.SurvivorWin:
                    textRenderer.text = Language.endGameTexts[32];
                    textRenderer.color = Locksmith.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Locksmith.color);
                    break;
                case WinCondition.BattleRoyaleSoloWin:
                    textRenderer.text = Language.endGameTexts[33];
                    textRenderer.color = Sleuth.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Sleuth.color);
                    break;
                case WinCondition.BattleRoyaleTimeWin:
                    textRenderer.text = Language.endGameTexts[34];
                    textRenderer.color = Sleuth.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Sleuth.color);
                    break;
                case WinCondition.BattleRoyaleLimeTeamWin:
                    textRenderer.text = Language.endGameTexts[35];
                    textRenderer.color = FortuneTeller.color;
                    __instance.BackgroundBar.material.SetColor("_Color", FortuneTeller.color);
                    break;
                case WinCondition.BattleRoyalePinkTeamWin:
                    textRenderer.text = Language.endGameTexts[36];
                    textRenderer.color = Locksmith.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Locksmith.color);
                    break;
                case WinCondition.BattleRoyaleSerialKillerWin:
                    textRenderer.text = Language.endGameTexts[37];
                    textRenderer.color = Joker.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Joker.color);
                    break;
                case WinCondition.MonjaFestivalGreenWin:
                    textRenderer.text = Language.endGameTexts[38];
                    textRenderer.color = Color.green;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.green);
                    break;
                case WinCondition.MonjaFestivalCyanWin:
                    textRenderer.text = Language.endGameTexts[39];
                    textRenderer.color = Color.cyan;
                    __instance.BackgroundBar.material.SetColor("_Color", Color.cyan);
                    break;
                case WinCondition.MonjaFestivalBigMonjaWin:
                    textRenderer.text = Language.endGameTexts[40];
                    textRenderer.color = Joker.color;
                    __instance.BackgroundBar.material.SetColor("_Color", Joker.color);
                    break;
                default:
                    Helpers.playEndMusic(5);
                    break;
            }

            if (MapOptions.showRoleSummary) {
                var position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.main.nearClipPlane));
                GameObject roleSummary = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
                roleSummary.transform.position = new Vector3(__instance.Navigation.ExitButton.transform.position.x + 0.1f, position.y - 0.1f, -14f);
                roleSummary.transform.localScale = new Vector3(1f, 1f, 1f);

                var roleSummaryText = new StringBuilder();
                roleSummaryText.AppendLine(Language.endGameTexts[41]);
                foreach (var data in AdditionalTempData.playerRoles) {
                    var roles = string.Join(" ", data.Roles.Select(x => Helpers.cs(x.color, x.name)));
                    var taskInfo = data.TasksTotal > 0 ? $" - <color=#FAD934FF>({data.TasksCompleted}/{data.TasksTotal})</color>" : "";
                    if (data.Kills != null) taskInfo += $" - <color=#FF0000FF>({Language.endGameTexts[42]}: {data.Kills})</color>";
                    roleSummaryText.AppendLine($"{data.PlayerName} - {roles}{taskInfo}");
                }
                TMPro.TMP_Text roleSummaryTextMesh = roleSummary.GetComponent<TMPro.TMP_Text>();
                roleSummaryTextMesh.alignment = TMPro.TextAlignmentOptions.TopLeft;
                roleSummaryTextMesh.color = Color.white;
                roleSummaryTextMesh.fontSizeMin = 1.5f;
                roleSummaryTextMesh.fontSizeMax = 1.5f;
                roleSummaryTextMesh.fontSize = 1.5f;

                var roleSummaryTextMeshRectTransform = roleSummaryTextMesh.GetComponent<RectTransform>();
                roleSummaryTextMeshRectTransform.anchoredPosition = new Vector2(position.x + 3.5f, position.y - 0.1f);
                roleSummaryTextMesh.text = roleSummaryText.ToString();
                Helpers.previousEndGameSummary = $"<size=110%>{roleSummaryText.ToString()}</size>";
            }
            AdditionalTempData.clear();
        }
    }

    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
    class CheckEndCriteriaPatch {
        public static bool Prefix(LogicGameFlowNormal __instance) {
            if (!GameData.Instance) return false;
            if (DestroyableSingleton<TutorialManager>.InstanceExists)
                return true;
            var statistics = new PlayerStatistics(ShipStatus.Instance);
            if (CheckAndEndGameForBombExploded(__instance)) return false;
            if (CheckAndEndGameForLoverWin(__instance, statistics)) return false;
            if (CheckAndEndGameForTaskMasterWin(__instance)) return false;
            if (CheckAndEndGameForJokerWin(__instance)) return false;
            if (CheckAndEndGameForPyromaniacWin(__instance)) return false;
            if (CheckAndEndGameForTreasureHunterWin(__instance)) return false;
            if (CheckAndEndGameForDevourerWin(__instance)) return false;
            if (CheckAndEndGameForPoisonerWin(__instance)) return false;
            if (CheckAndEndGameForPuppeteerWin(__instance)) return false;
            if (CheckAndEndGameForExilerWin(__instance)) return false;
            if (CheckAndEndGameForSeekerWin(__instance)) return false;
            if (CheckAndEndGameForKidLose(__instance)) return false;
            if (CheckAndEndGameForRenegadeWin(__instance, statistics)) return false;
            if (CheckAndEndGameForBountyHunterWin(__instance)) return false;
            if (CheckAndEndGameForTrapperWin(__instance, statistics)) return false;
            if (CheckAndEndGameForYinyangerWin(__instance, statistics)) return false;
            if (CheckAndEndGameForChallengerWin(__instance, statistics)) return false;
            if (CheckAndEndGameForNinjaWin(__instance, statistics)) return false;
            if (CheckAndEndGameForBerserkerWin(__instance, statistics)) return false;
            if (CheckAndEndGameForYandereWin(__instance, statistics)) return false;
            if (CheckAndEndGameForStrandedWin(__instance, statistics)) return false;
            if (CheckAndEndGameForMonjaWin(__instance, statistics)) return false;
            if (CheckAndEndGameForSabotageWin(__instance)) return false;
            if (CheckAndEndGameForTaskWin(__instance)) return false;
            if (CheckAndEndGameForImpostorWin(__instance, statistics)) return false;
            if (CheckAndEndGameForCrewmateWin(__instance, statistics)) return false;
            if (CheckAndEndGameForDrawFlagWin(__instance)) return false;
            if (CheckAndEndGameForRedTeamFlagWin(__instance)) return false;
            if (CheckAndEndGameForBlueTeamFlagWin(__instance)) return false;
            if (CheckAndEndGameForThiefModeThiefWin(__instance)) return false;
            if (CheckAndEndGameForThiefModePoliceWin(__instance)) return false;
            if (CheckAndEndGameForDrawHillWin(__instance)) return false;
            if (CheckAndEndGameForGreenTeamHillWin(__instance)) return false;
            if (CheckAndEndGameForYellowTeamHillWin(__instance)) return false;
            if (CheckAndEndGameForHotPotatoEnd(__instance)) return false;
            if (CheckAndEndGameForZombieWin(__instance)) return false;
            if (CheckAndEndGameForSurvivorWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleSoloWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleTimeWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleDraw(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleLimeTeamWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyalePinkTeamWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleSerialKillerWin(__instance)) return false;
            if (CheckAndEndGameForMonjaFestivalDraw(__instance)) return false;
            if (CheckAndEndGameForMonjaFestivalGreenTeamWin(__instance)) return false;
            if (CheckAndEndGameForMonjaFestivalCyanTeamWin(__instance)) return false;
            if (CheckAndEndGameForMonjaFestivalBigMonjaWin(__instance)) return false; 
            return false;
        }

        private static bool CheckAndEndGameForBombExploded(LogicGameFlowNormal __instance) {
            if (Bomberman.triggerBombExploded) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BombExploded, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForLoverWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamLoversAlive == 2 && statistics.TotalAlive <= 3) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.LoversWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForTaskMasterWin(LogicGameFlowNormal __instance) {
            if (TaskMaster.triggerTaskMasterCrewWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.TaskMasterCrewWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForJokerWin(LogicGameFlowNormal __instance) {
            if (Joker.triggerJokerWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.JokerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForPyromaniacWin(LogicGameFlowNormal __instance) {
            if (Pyromaniac.triggerPyromaniacWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.PyromaniacWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForTreasureHunterWin(LogicGameFlowNormal __instance) {
            if (TreasureHunter.triggertreasureHunterWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.TreasureHunterWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForDevourerWin(LogicGameFlowNormal __instance) {
            if (Devourer.triggerdevourerWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.DevourerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForPoisonerWin(LogicGameFlowNormal __instance) {
            if (Poisoner.triggerPoisonerWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.PoisonerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForPuppeteerWin(LogicGameFlowNormal __instance) {
            if (Puppeteer.triggerPuppeteerWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.PuppeteerWin, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForExilerWin(LogicGameFlowNormal __instance) {
            if (Exiler.triggerExilerWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ExilerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForSeekerWin(LogicGameFlowNormal __instance) {
            if (Seeker.triggerSeekerWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.SeekerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForKidLose(LogicGameFlowNormal __instance) {
            if (Kid.triggerKidLose) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.KidLose, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForRenegadeWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamRenegadeAlive >= statistics.TotalAlive - statistics.TeamRenegadeAlive + statistics.TeamCaptainAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive != statistics.TeamRenegadeAlive && !(statistics.TeamRenegadeHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.TeamRenegadeWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBountyHunterWin(LogicGameFlowNormal __instance) {
            if (BountyHunter.triggerBountyHunterWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BountyHunterWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForTrapperWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamTrapperAlive >= statistics.TotalAlive - statistics.TeamTrapperAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.TrapperWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForYinyangerWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamYinyangerAlive >= statistics.TotalAlive - statistics.TeamYinyangerAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.YinyangerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForChallengerWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (Challenger.triggerChallengerWin || (statistics.TeamChallengerAlive >= statistics.TotalAlive - statistics.TeamChallengerAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2))) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ChallengerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForNinjaWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamNinjaAlive >= statistics.TotalAlive - statistics.TeamNinjaAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.NinjaWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBerserkerWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamBerserkerAlive >= statistics.TotalAlive - statistics.TeamBerserkerAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BerserkerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForYandereWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (Yandere.triggerYandereWin && !Yandere.rampageMode) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.YandereWin, false);
                return true;
            }

            if (Yandere.rampageMode && statistics.TeamYandereAlive >= statistics.TotalAlive - statistics.TeamYandereAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.YandereWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForStrandedWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (Stranded.triggerStrandedWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.StrandedWin, false);
                return true;
            }
            if (statistics.TeamStrandedAlive >= statistics.TotalAlive - statistics.TeamStrandedAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.StrandedWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForMonjaWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamMonjaAlive >= statistics.TotalAlive - statistics.TeamMonjaAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForSabotageWin(LogicGameFlowNormal __instance) {
            if (ShipStatus.Instance.Systems == null) return false;
            ISystemType systemType = ShipStatus.Instance.Systems.ContainsKey(SystemTypes.LifeSupp) ? ShipStatus.Instance.Systems[SystemTypes.LifeSupp] : null;
            if (systemType != null) {
                LifeSuppSystemType lifeSuppSystemType = systemType.TryCast<LifeSuppSystemType>();
                if (lifeSuppSystemType != null && lifeSuppSystemType.Countdown < 0f) {
                    EndGameForSabotage(__instance);
                    lifeSuppSystemType.Countdown = 10000f;
                    return true;
                }
            }
            ISystemType systemType2 = ShipStatus.Instance.Systems.ContainsKey(SystemTypes.HeliSabotage) ? ShipStatus.Instance.Systems[SystemTypes.HeliSabotage] : null; 
            if (systemType2 == null) {
                systemType2 = ShipStatus.Instance.Systems.ContainsKey(SystemTypes.Laboratory) ? ShipStatus.Instance.Systems[SystemTypes.Laboratory] : null;
            }
            if (systemType2 == null) {
                systemType2 = ShipStatus.Instance.Systems.ContainsKey(SystemTypes.Reactor) ? ShipStatus.Instance.Systems[SystemTypes.Reactor] : null;
            }
            if (systemType2 != null) {
                ICriticalSabotage criticalSystem = systemType2.TryCast<ICriticalSabotage>();
                if (criticalSystem != null && criticalSystem.Countdown < 0f) {
                    EndGameForSabotage(__instance);
                    criticalSystem.ClearSabotage();
                    return true;
                }
            }
            return false;
        }
        private static bool CheckAndEndGameForTaskWin(LogicGameFlowNormal __instance) {
            if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks && gameType <= 1) {
                GameManager.Instance.RpcEndGame(GameOverReason.CrewmatesByTask, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForImpostorWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (gameType <= 1 && statistics.TeamImpostorsAlive >= statistics.TotalAlive - statistics.TeamImpostorsAlive + statistics.TeamCaptainAlive && statistics.TeamRenegadeAlive == 0 && statistics.TeamBountyHunterAlive == 0 && statistics.TeamTrapperAlive == 0 && statistics.TeamYinyangerAlive == 0 && statistics.TeamChallengerAlive == 0 && statistics.TeamNinjaAlive == 0 && statistics.TeamBerserkerAlive == 0 && statistics.TeamYandereAlive == 0 && statistics.TeamStrandedAlive == 0 && statistics.TeamMonjaAlive == 0 && statistics.TeamCaptainAlive != statistics.TeamImpostorsAlive && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameOverReason endReason;
                switch (GameData.LastDeathReason) {
                    case DeathReason.Exile:
                        endReason = GameOverReason.ImpostorsByVote;
                        break;
                    case DeathReason.Kill:
                        endReason = GameOverReason.ImpostorsByKill;
                        break;
                    default:
                        endReason = GameOverReason.ImpostorsByVote;
                        break;
                }
                GameManager.Instance.RpcEndGame(endReason, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForCrewmateWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (gameType <= 1 && statistics.TeamImpostorsAlive == 0 && statistics.TeamRenegadeAlive == 0 && statistics.TeamBountyHunterAlive == 0 && statistics.TeamTrapperAlive == 0 && statistics.TeamYinyangerAlive == 0 && statistics.TeamChallengerAlive == 0 && statistics.TeamNinjaAlive == 0 && statistics.TeamBerserkerAlive == 0 && statistics.TeamYandereAlive == 0 && statistics.TeamStrandedAlive == 0 && statistics.TeamMonjaAlive == 0) {
                GameManager.Instance.RpcEndGame(GameOverReason.CrewmatesByVote, false);
                return true;
            }
            return false;
        }
        private static void EndGameForSabotage(LogicGameFlowNormal __instance) {
            GameManager.Instance.RpcEndGame(GameOverReason.ImpostorsBySabotage, false);
            return;
        }
        private static bool CheckAndEndGameForDrawFlagWin(LogicGameFlowNormal __instance) {
            if (CaptureTheFlag.triggerDrawWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.DrawTeamWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForRedTeamFlagWin(LogicGameFlowNormal __instance) {
            if (CaptureTheFlag.triggerRedTeamWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.RedTeamFlagWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBlueTeamFlagWin(LogicGameFlowNormal __instance) {
            if (CaptureTheFlag.triggerBlueTeamWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BlueTeamFlagWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForThiefModeThiefWin(LogicGameFlowNormal __instance) {
            if (PoliceAndThief.triggerThiefWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModeThiefWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForThiefModePoliceWin(LogicGameFlowNormal __instance) {
            if (PoliceAndThief.triggerPoliceWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModePoliceWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForDrawHillWin(LogicGameFlowNormal __instance) {
            if (KingOfTheHill.triggerDrawWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.TeamHillDraw, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForGreenTeamHillWin(LogicGameFlowNormal __instance) {
            if (KingOfTheHill.triggerGreenTeamWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.GreenTeamHillWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForYellowTeamHillWin(LogicGameFlowNormal __instance) {
            if (KingOfTheHill.triggerYellowTeamWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.YellowTeamHillWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForHotPotatoEnd(LogicGameFlowNormal __instance) {
            if (HotPotato.triggerHotPotatoEnd) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HotPotatoEnd, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForZombieWin(LogicGameFlowNormal __instance) {
            if (ZombieLaboratory.triggerZombieWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ZombieWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForSurvivorWin(LogicGameFlowNormal __instance) {
            if (ZombieLaboratory.triggerSurvivorWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.SurvivorWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBattleRoyaleSoloWin(LogicGameFlowNormal __instance) {
            if (BattleRoyale.triggerSoloWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleSoloWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBattleRoyaleTimeWin(LogicGameFlowNormal __instance) {
            if (BattleRoyale.triggerTimeWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleTimeWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBattleRoyaleDraw(LogicGameFlowNormal __instance) {
            if (BattleRoyale.triggerDrawWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleDraw, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBattleRoyaleLimeTeamWin(LogicGameFlowNormal __instance) {
            if (BattleRoyale.triggerLimeTeamWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBattleRoyalePinkTeamWin(LogicGameFlowNormal __instance) {
            if (BattleRoyale.triggerPinkTeamWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBattleRoyaleSerialKillerWin(LogicGameFlowNormal __instance) {
            if (BattleRoyale.triggerSerialKillerWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleSerialKillerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForMonjaFestivalDraw(LogicGameFlowNormal __instance) {
            if (MonjaFestival.triggerDrawWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalDraw, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForMonjaFestivalGreenTeamWin(LogicGameFlowNormal __instance) {
            if (MonjaFestival.triggerGreenTeamWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalGreenWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForMonjaFestivalCyanTeamWin(LogicGameFlowNormal __instance) {
            if (MonjaFestival.triggerCyanTeamWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalCyanWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForMonjaFestivalBigMonjaWin(LogicGameFlowNormal __instance) {
            if (MonjaFestival.triggerBigMonjaWin) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.MonjaFestivalBigMonjaWin, false);
                return true;
            }
            return false;
        }
    }

    internal class PlayerStatistics
    {
        public int TotalAlive { get; set; }
        public int TeamImpostorsAlive { get; set; }
        public int TeamLoversAlive { get; set; }
        public bool TeamImpostorHasAliveLover { get; set; }
        public int TeamRenegadeAlive { get; set; }
        public bool TeamRenegadeHasAliveLover { get; set; }
        public int TeamBountyHunterAlive { get; set; }
        public int TeamTrapperAlive { get; set; }
        public int TeamYinyangerAlive { get; set; }
        public int TeamChallengerAlive { get; set; }
        public int TeamNinjaAlive { get; set; }
        public int TeamBerserkerAlive { get; set; }
        public int TeamYandereAlive { get; set; }
        public int TeamStrandedAlive { get; set; }
        public int TeamMonjaAlive { get; set; }
        public int TeamCaptainAlive { get; set; }

        public PlayerStatistics(ShipStatus __instance) {
            GetPlayerCounts();
        }

        private bool isLover(NetworkedPlayerInfo p) {
            return (Modifiers.lover1 != null && Modifiers.lover1.PlayerId == p.PlayerId) || (Modifiers.lover2 != null && Modifiers.lover2.PlayerId == p.PlayerId);
        }

        private void GetPlayerCounts() {
            int numTotalAlive = 0;
            int numImpostorsAlive = 0;
            int numLoversAlive = 0;
            bool impLover = false;
            int numRenegadeAlive = 0;
            bool renegadeLover = false;
            int numBountyHunterAlive = 0;
            int numTrapperAlive = 0;
            int numYinyangerAlive = 0;
            int numChallengerAlive = 0;
            int numNinjaAlive = 0;
            int numBerserkerAlive = 0;
            int numYandereAlive = 0;
            int numStrandedAlive = 0;
            int numMonjaAlive = 0;
            int numCaptainAlive = 0;

            for (int i = 0; i < GameData.Instance.PlayerCount; i++) {
                NetworkedPlayerInfo playerInfo = GameData.Instance.AllPlayers[i];
                if (!playerInfo.Disconnected) {
                    if (!playerInfo.IsDead) {
                        numTotalAlive++;

                        bool lover = isLover(playerInfo);
                        if (lover) numLoversAlive++;

                        if (playerInfo.Role.IsImpostor) {
                            numImpostorsAlive++;
                            if (lover) impLover = true;
                        }
                        if (Renegade.renegade != null && Renegade.renegade.PlayerId == playerInfo.PlayerId) {
                            numRenegadeAlive++;
                            if (lover) renegadeLover = true;
                        }
                        if (Minion.minion != null && Minion.minion.PlayerId == playerInfo.PlayerId) {
                            numRenegadeAlive++;
                            if (lover) renegadeLover = true;
                        }
                        if (BountyHunter.bountyhunter != null && BountyHunter.bountyhunter.PlayerId == playerInfo.PlayerId) {
                            numBountyHunterAlive++;
                        }
                        if (Trapper.trapper != null && Trapper.trapper.PlayerId == playerInfo.PlayerId) {
                            numTrapperAlive++;
                        }
                        if (Yinyanger.yinyanger != null && Yinyanger.yinyanger.PlayerId == playerInfo.PlayerId) {
                            numYinyangerAlive++;
                        }
                        if (Challenger.challenger != null && Challenger.challenger.PlayerId == playerInfo.PlayerId) {
                            numChallengerAlive++;
                        }
                        if (Ninja.ninja != null && Ninja.ninja.PlayerId == playerInfo.PlayerId) {
                            numNinjaAlive++;
                        }
                        if (Berserker.berserker != null && Berserker.berserker.PlayerId == playerInfo.PlayerId) {
                            numBerserkerAlive++;
                        }
                        if (Yandere.yandere != null && Yandere.yandere.PlayerId == playerInfo.PlayerId) {
                            numYandereAlive++;
                        }
                        if (Stranded.stranded != null && Stranded.stranded.PlayerId == playerInfo.PlayerId) {
                            numStrandedAlive++;
                        }
                        if (Monja.monja != null && Monja.monja.PlayerId == playerInfo.PlayerId) {
                            numMonjaAlive++;
                        }
                        if (Captain.captain != null && Captain.captain.PlayerId == playerInfo.PlayerId) {
                            numCaptainAlive++;
                        }
                    }
                }
            }

            TotalAlive = numTotalAlive;
            TeamImpostorsAlive = numImpostorsAlive;
            TeamLoversAlive = numLoversAlive;
            TeamImpostorHasAliveLover = impLover;
            TeamRenegadeAlive = numRenegadeAlive;
            TeamRenegadeHasAliveLover = renegadeLover;
            TeamBountyHunterAlive = numBountyHunterAlive;
            TeamTrapperAlive = numTrapperAlive;
            TeamYinyangerAlive = numYinyangerAlive;
            TeamChallengerAlive = numChallengerAlive;
            TeamNinjaAlive = numNinjaAlive;
            TeamBerserkerAlive = numBerserkerAlive;
            TeamYandereAlive = numYandereAlive;
            TeamStrandedAlive = numStrandedAlive;
            TeamMonjaAlive = numMonjaAlive;
            TeamCaptainAlive = numCaptainAlive;
        }
    }
}