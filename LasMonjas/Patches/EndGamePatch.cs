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
        GamemodesDrawWin = 32,
        RedTeamFlagWin = 33,
        BlueTeamFlagWin = 34,
        ThiefModeThiefWin = 35,
        ThiefModePoliceWin = 36,
        GreenTeamHillWin = 37,
        YellowTeamHillWin = 38,
        HotPotatoEnd = 39,
        ZombieWin = 40,
        SurvivorWin = 41,
        BattleRoyaleSoloWin = 42,
        BattleRoyaleTimeWin = 43,
        BattleRoyaleLimeTeamWin = 44,
        BattleRoyalePinkTeamWin = 45,
        BattleRoyaleSerialKillerWin = 46,
        MonjaFestivalGreenWin = 47,
        MonjaFestivalCyanWin = 48,
        MonjaFestivalBigMonjaWin = 49
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
        GamemodesDrawWin,
        RedTeamFlagWin,
        BlueTeamFlagWin,
        ThiefModeThiefWin,
        ThiefModePoliceWin,
        GreenTeamHillWin,
        YellowTeamHillWin,
        HotPotatoEnd,
        ZombieWin,
        SurvivorWin,
        BattleRoyaleSoloWin,
        BattleRoyaleTimeWin,
        BattleRoyaleLimeTeamWin,
        BattleRoyalePinkTeamWin,
        BattleRoyaleSerialKillerWin,
        MonjaFestivalGreenWin,
        MonjaFestivalCyanWin,
        MonjaFestivalBigMonjaWin
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
        
        private static void SetWinner(PlayerControl player, WinCondition condition, Action<CachedPlayerData> configure = null) {
            EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();

            if (player != null) {
                CachedPlayerData wpd = new CachedPlayerData(player.Data);

                configure?.Invoke(wpd);

                EndGameResult.CachedWinners.Add(wpd);
            }

            AdditionalTempData.winCondition = condition;
        }
        private static void SetWinners(IEnumerable<PlayerControl> players, WinCondition condition, Action<CachedPlayerData> configure = null) {
            EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();

            foreach (var player in players) {
                if (player == null) {
                    continue;
                }
                CachedPlayerData wpd = new CachedPlayerData(player.Data);

                configure?.Invoke(wpd);

                EndGameResult.CachedWinners.Add(wpd);
            }

            AdditionalTempData.winCondition = condition;
        }

        private static void SetAllPlayers(WinCondition condition) {
            SetWinners(PlayerInCache.AllPlayers.Select(x => x.PlayerControl), condition);
        }

        private static void SetAlivePlayers(IEnumerable<PlayerControl> players, WinCondition condition) {
            EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();

            foreach (var player in players) {
                if (player == null || player.Data == null || player.Data.IsDead || player.Data.Disconnected) {
                    continue;
                }

                EndGameResult.CachedWinners.Add(new CachedPlayerData(player.Data));
            }

            AdditionalTempData.winCondition = condition;
        }

        private static void SetFilteredWinners(IEnumerable<PlayerControl> players, WinCondition condition, Func<PlayerControl, bool> filter) {
            EndGameResult.CachedWinners = new Il2CppSystem.Collections.Generic.List<CachedPlayerData>();
           
            foreach (var player in players) {
                if (player == null) {
                    continue;
                }

                if (!filter(player)) {
                    continue;
                }

                EndGameResult.CachedWinners.Add(new CachedPlayerData(player.Data));
            }

            AdditionalTempData.winCondition = condition;
        }


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
            bool gamemodesDrawWin = (gameType == 2 || gameType == 4 || gameType == 7 || gameType == 8) && gameOverReason == (GameOverReason)CustomGameOverReason.GamemodesDrawWin;
            bool redTeamFlagWin = gameType == 2 && gameOverReason == (GameOverReason)CustomGameOverReason.RedTeamFlagWin;
            bool blueTeamFlagWin = gameType == 2 && gameOverReason == (GameOverReason)CustomGameOverReason.BlueTeamFlagWin;
            bool thiefModeThiefWin = gameType == 3 && gameOverReason == (GameOverReason)CustomGameOverReason.ThiefModeThiefWin;
            bool thiefModePoliceWin = gameType == 3 && gameOverReason == (GameOverReason)CustomGameOverReason.ThiefModePoliceWin;
            bool greenTeamHillWin = gameType == 4 && gameOverReason == (GameOverReason)CustomGameOverReason.GreenTeamHillWin;
            bool yellowTeamHillWin = gameType == 4 && gameOverReason == (GameOverReason)CustomGameOverReason.YellowTeamHillWin;
            bool hotPotatoEnd = gameType == 5 && gameOverReason == (GameOverReason)CustomGameOverReason.HotPotatoEnd;
            bool zombieWin = gameType == 6 && gameOverReason == (GameOverReason)CustomGameOverReason.ZombieWin;
            bool survivorWin = gameType == 6 && gameOverReason == (GameOverReason)CustomGameOverReason.SurvivorWin;
            bool battleRoyaleSoloWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleSoloWin;
            bool battleRoyaleTimeWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleTimeWin;
            bool battleRoyaleLimeTeamWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin;
            bool battleRoyalePinkTeamWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin;
            bool battleRoyaleSerialKillerWin = gameType == 7 && gameOverReason == (GameOverReason)CustomGameOverReason.BattleRoyaleSerialKillerWin;
            bool monjaFestivalGreenWin = gameType == 8 && gameOverReason == (GameOverReason)CustomGameOverReason.MonjaFestivalGreenWin;
            bool monjaFestivalCyanWin = gameType == 8 && gameOverReason == (GameOverReason)CustomGameOverReason.MonjaFestivalCyanWin;
            bool monjaFestivalBigMonjaWin = gameType == 8 && gameOverReason == (GameOverReason)CustomGameOverReason.MonjaFestivalBigMonjaWin;

            // Kid lose
            if (kidLose) {
                SetWinner(Kid.kid, WinCondition.KidLose, wpd => wpd.IsYou = false);
            }

            // Bomb exploded
            else if (bombExploded) {
                SetFilteredWinners(PlayerInCache.AllPlayers.Select(x => x.PlayerControl), WinCondition.BombExploded, p => p.Data.Role.IsImpostor);
            }

            // Lovers win conditions
            else if (loversWin) {
                // Double win for lovers with crewmates
                if (!Modifiers.existingWithKiller()) {
                    SetFilteredWinners(PlayerInCache.AllPlayers.Select(x => x.PlayerControl), WinCondition.LoversTeamWin, p => p == Modifiers.lover1 || p == Modifiers.lover2 || (!p.Data.Role.IsImpostor && !Helpers.isNeutral(p) && !Helpers.isRebel(p)));
                }
                // Lovers solo win
                else {
                    SetWinners(new[] { Modifiers.lover1, Modifiers.lover2 }, WinCondition.LoversSoloWin);
                }
            }

            // TaskMaster crew win
            else if (taskMasterCrewWin) {
                SetFilteredWinners(PlayerInCache.AllPlayers.Select(x => x.PlayerControl), WinCondition.TaskMasterCrewWin, p => !p.Data.Role.IsImpostor && !Helpers.isNeutral(p) && !Helpers.isRebel(p));
            }

            // Joker win
            else if (jokerWin) {
                SetWinner(Joker.joker, WinCondition.JokerWin);
            }

            // Pyromaniac win
            else if (pyromaniacWin) {
                SetWinner(Pyromaniac.pyromaniac, WinCondition.PyromaniacWin);
            }

            // TreasureHunter win
            else if (treasurehunterWin) {
                SetWinner(TreasureHunter.treasureHunter, WinCondition.TreasureHunterWin);
            }

            // Devourer win
            else if (devourerWin) {
                SetWinner(Devourer.devourer, WinCondition.DevourerWin);
            }

            // Poisoner win
            else if (poisonerWin) {
                SetWinner(Poisoner.poisoner, WinCondition.PoisonerWin);
            }

            // Puppeteer win
            else if (puppeteerWin) {
                SetWinner(Puppeteer.puppeteer, WinCondition.PuppeteerWin);
            }

            // Exiler win
            else if (exilerWin) {
                SetWinner(Exiler.exiler, WinCondition.ExilerWin);
            }

            // Seeker win
            else if (seekerWin) {
                SetWinner(Seeker.seeker, WinCondition.SeekerWin);
            }

            // Renegade win condition
            else if (teamRenegadeWin) {
                // Renegade wins if nobody except renegade is alive, if there is a minion the minion also wins
                if (Minion.minion != null) {
                    SetWinners(new[] { Renegade.renegade, Minion.minion }, WinCondition.RenegadeWin, wpd => wpd.IsImpostor = false);
                }
                else {
                    SetWinner(Renegade.renegade, WinCondition.RenegadeWin, wpd => wpd.IsImpostor = false);
                }
            }

            // BountyHunter win
            else if (bountyhunterWin) {
                SetWinner(BountyHunter.bountyhunter, WinCondition.BountyHunterWin);
            }

            // Trapper win
            else if (trapperWin) {
                SetWinner(Trapper.trapper, WinCondition.TrapperWin);
            }

            // Yinyanger win
            else if (yinyangerWin) {
                SetWinner(Yinyanger.yinyanger, WinCondition.YinyangerWin);
            }

            // Challenger win
            else if (challengerWin) {
                SetWinner(Challenger.challenger, WinCondition.ChallengerWin);
            }

            // Ninja win
            else if (ninjaWin) {
                SetWinner(Ninja.ninja, WinCondition.NinjaWin);
            }

            // Berserker win
            else if (berserkerWin) {
                SetWinner(Berserker.berserker, WinCondition.BerserkerWin);
            }

            // Yandere win
            else if (yandereWin) {
                SetWinner(Yandere.yandere, WinCondition.YandereWin);
            }

            // Stranded win
            else if (strandedWin) {
                SetWinner(Stranded.stranded, WinCondition.StrandedWin);
            }

            // Monja win
            else if (monjaWin) {
                SetWinner(Monja.monja, WinCondition.MonjaWin);
            }

            // Gamemodes Draw
            else if (gamemodesDrawWin) {
                SetAllPlayers(WinCondition.GamemodesDrawWin);
            }

            // Flag Game Mode Win
            // Red Team Win
            else if (redTeamFlagWin) {
                SetWinners(CaptureTheFlag.redteamFlag, WinCondition.RedTeamFlagWin);
            }
            // Blue Team Win
            else if (blueTeamFlagWin) {
                SetWinners(CaptureTheFlag.blueteamFlag, WinCondition.BlueTeamFlagWin);
            }

            // Thief Mode Win
            // Thief Team Win
            else if (thiefModeThiefWin) {
                SetWinners(PoliceAndThief.thiefTeam, WinCondition.ThiefModeThiefWin);
            }
            // Police Team Win
            else if (thiefModePoliceWin) {
                SetWinners(PoliceAndThief.policeTeam, WinCondition.ThiefModePoliceWin);
            }

            // King Game Mode Win            
            // Green Team Win
            else if (greenTeamHillWin) {
                SetWinners(KingOfTheHill.greenTeam, WinCondition.GreenTeamHillWin);
            }
            // Yellow Team Win
            else if (yellowTeamHillWin) {
                SetWinners(KingOfTheHill.yellowTeam, WinCondition.YellowTeamHillWin);
            }

            // Hot Potato Game Mode Win
            else if (hotPotatoEnd) {
                SetWinners(HotPotato.notPotatoTeamAlive, WinCondition.HotPotatoEnd);
            }

            // ZombieLaboratory zombie Win
            else if (zombieWin) {
                SetWinners(ZombieLaboratory.zombieTeam, WinCondition.ZombieWin);
            }
            else if (survivorWin) {
                SetWinners(ZombieLaboratory.survivorTeam, WinCondition.SurvivorWin);
            }

            // BattleRoyale Win
            else if (battleRoyaleSoloWin) {
                SetAlivePlayers(BattleRoyale.soloPlayerTeam, WinCondition.BattleRoyaleSoloWin);
            }
            // BattleRoyale Time Win
            else if (battleRoyaleTimeWin) {
                if (BattleRoyale.matchType == 0) {
                    SetAlivePlayers(BattleRoyale.soloPlayerTeam, WinCondition.BattleRoyaleTimeWin);
                }
                else {
                    SetAlivePlayers(PlayerInCache.AllPlayers.Select(x => x.PlayerControl), WinCondition.BattleRoyaleTimeWin);
                }
            }
            // BattleRoyale Lime Team Win
            else if (battleRoyaleLimeTeamWin) {
                SetWinners(BattleRoyale.limeTeam, WinCondition.BattleRoyaleLimeTeamWin);
            }
            // BattleRoyale Pink Team Win
            else if (battleRoyalePinkTeamWin) {
                SetWinners(BattleRoyale.pinkTeam, WinCondition.BattleRoyalePinkTeamWin);
            }
            // BattleRoyale Serial Killer Win
            else if (battleRoyaleSerialKillerWin) {
                SetWinner(BattleRoyale.serialKiller, WinCondition.BattleRoyaleSerialKillerWin);
            }

            // MonjaFestival Green Team Win
            else if (monjaFestivalGreenWin) {
                SetWinners(MonjaFestival.greenTeam, WinCondition.MonjaFestivalGreenWin);
            }
            // MonjaFestival Pink Team Win
            else if (monjaFestivalCyanWin) {
                SetWinners(MonjaFestival.cyanTeam, WinCondition.MonjaFestivalCyanWin);
            }
            // MonjaFestival Big Monja Win
            else if (monjaFestivalBigMonjaWin) {
                SetWinner(MonjaFestival.bigMonjaPlayer, WinCondition.MonjaFestivalBigMonjaWin);
            }

            // Reset Settings
            RPCProcedure.resetVariables();
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public class EndGameManagerSetUpPatch {

        private static void ApplyWinCondition(EndGameManager manager, TMPro.TMP_Text textRenderer, string text, Color color, int? music = null) {
            textRenderer.text = text;
            textRenderer.color = color;

            manager.BackgroundBar.material.SetColor("_Color", color);

            if (music.HasValue) {
                Helpers.playEndMusic(music.Value);
            }
        }
        
        public static void Postfix(EndGameManager __instance) {

            GameObject bonusText = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            bonusText.transform.position = new Vector3(__instance.WinText.transform.position.x, __instance.WinText.transform.position.y - 0.5f, __instance.WinText.transform.position.z);
            bonusText.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            TMPro.TMP_Text textRenderer = bonusText.GetComponent<TMPro.TMP_Text>();
            textRenderer.text = "";

            switch (AdditionalTempData.winCondition) {
                case WinCondition.KidLose:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[0], Kid.color, 5);
                    break;
                case WinCondition.BombExploded:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[1], Bomberman.color, 6);
                    break;
                case WinCondition.LoversTeamWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[2], Modifiers.loverscolor, 5); 
                    break;
                case WinCondition.LoversSoloWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[3], Modifiers.loverscolor, 6);
                    break;
                case WinCondition.TaskMasterCrewWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[4], TaskMaster.color, 5);
                    break;
                case WinCondition.JokerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[5], Joker.color, 3);
                    break;
                case WinCondition.PyromaniacWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[6], Pyromaniac.color, 3);
                    break;
                case WinCondition.TreasureHunterWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[7], TreasureHunter.color, 3);
                    break;
                case WinCondition.DevourerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[8], Devourer.color, 3);
                    break;
                case WinCondition.PoisonerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[9], Poisoner.color, 3);
                    break;
                case WinCondition.PuppeteerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[10], Puppeteer.color, 3);
                    break;
                case WinCondition.ExilerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[11], Exiler.color, 3);
                    break;
                case WinCondition.SeekerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[12], Seeker.color, 3);
                    break;
                case WinCondition.RenegadeWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[13], Renegade.color, 4);
                    break;
                case WinCondition.BountyHunterWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[14], BountyHunter.color, 4);
                    break;
                case WinCondition.TrapperWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[15], Trapper.color, 4);
                    break;
                case WinCondition.YinyangerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[16], Yinyanger.color, 4);
                    break;
                case WinCondition.ChallengerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[17], Challenger.color, 4);
                    break;
                case WinCondition.NinjaWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[18], Ninja.color, 4); 
                    break;
                case WinCondition.BerserkerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[19], Berserker.color, 4);
                    break;
                case WinCondition.YandereWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[20], Yandere.color, 4);
                    break;
                case WinCondition.StrandedWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[21], Stranded.color, 4);
                    break;
                case WinCondition.MonjaWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[22], Monja.color, 4);
                    break;
                case WinCondition.GamemodesDrawWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[23], Joker.color);
                    break;
                case WinCondition.RedTeamFlagWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[24], Color.red);
                    break;
                case WinCondition.BlueTeamFlagWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[25], Color.blue);
                    break;
                case WinCondition.ThiefModePoliceWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[26], Color.cyan);
                    break;
                case WinCondition.ThiefModeThiefWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[27], Mechanic.color);
                    break;
                case WinCondition.GreenTeamHillWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[28], Color.green);
                    break;
                case WinCondition.YellowTeamHillWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[29], Color.yellow);
                    break;
                case WinCondition.HotPotatoEnd:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[30], Color.cyan);
                    break;
                case WinCondition.ZombieWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[31], Mechanic.color);
                    break;
                case WinCondition.SurvivorWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[32], Locksmith.color);
                    break;
                case WinCondition.BattleRoyaleSoloWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[33], Sleuth.color);
                    break;
                case WinCondition.BattleRoyaleTimeWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[34], Sleuth.color);
                    break;
                case WinCondition.BattleRoyaleLimeTeamWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[35], FortuneTeller.color); 
                    break;
                case WinCondition.BattleRoyalePinkTeamWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[36], Locksmith.color);
                    break;
                case WinCondition.BattleRoyaleSerialKillerWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[37], Joker.color);
                    break;
                case WinCondition.MonjaFestivalGreenWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[38], Color.green);
                    break;
                case WinCondition.MonjaFestivalCyanWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[39], Color.cyan);
                    break;
                case WinCondition.MonjaFestivalBigMonjaWin:
                    ApplyWinCondition(__instance, textRenderer, Language.endGameTexts[40], Joker.color);
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
            }
            AdditionalTempData.clear();
        }
    }

    [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
    class CheckEndCriteriaPatch {

        private static bool TryEndGame(bool trigger, CustomGameOverReason reason) {
            if (!trigger) {
                return false;
            }

            GameManager.Instance.RpcEndGame((GameOverReason)reason, false);

            return true;
        }
        private static bool TryEndStatisticsGame(int aliveCount, PlayerStatistics statistics, CustomGameOverReason reason) {
            if (aliveCount >= statistics.TotalAlive - aliveCount && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                
                GameManager.Instance.RpcEndGame((GameOverReason)reason, false);

                return true;
            }

            return false;
        }

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
            if (CheckAndEndGameForGameModeDrawWin(__instance)) return false;
            if (CheckAndEndGameForRedTeamFlagWin(__instance)) return false;
            if (CheckAndEndGameForBlueTeamFlagWin(__instance)) return false;
            if (CheckAndEndGameForThiefModeThiefWin(__instance)) return false;
            if (CheckAndEndGameForThiefModePoliceWin(__instance)) return false;
            if (CheckAndEndGameForGreenTeamHillWin(__instance)) return false;
            if (CheckAndEndGameForYellowTeamHillWin(__instance)) return false;
            if (CheckAndEndGameForHotPotatoEnd(__instance)) return false;
            if (CheckAndEndGameForZombieWin(__instance)) return false;
            if (CheckAndEndGameForSurvivorWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleSoloWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleTimeWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleLimeTeamWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyalePinkTeamWin(__instance)) return false;
            if (CheckAndEndGameForBattleRoyaleSerialKillerWin(__instance)) return false;
            if (CheckAndEndGameForMonjaFestivalGreenTeamWin(__instance)) return false;
            if (CheckAndEndGameForMonjaFestivalCyanTeamWin(__instance)) return false;
            if (CheckAndEndGameForMonjaFestivalBigMonjaWin(__instance)) return false; 
            return false;
        }

        private static bool CheckAndEndGameForBombExploded(LogicGameFlowNormal __instance) {
            return TryEndGame(Bomberman.triggerBombExploded, CustomGameOverReason.BombExploded); 
        }
        private static bool CheckAndEndGameForLoverWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamLoversAlive == 2 && statistics.TotalAlive <= 3) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.LoversWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForTaskMasterWin(LogicGameFlowNormal __instance) {
            return TryEndGame(TaskMaster.triggerTaskMasterCrewWin, CustomGameOverReason.TaskMasterCrewWin);
        }
        private static bool CheckAndEndGameForJokerWin(LogicGameFlowNormal __instance) {
            return TryEndGame(Joker.triggerJokerWin, CustomGameOverReason.JokerWin);
        }
        private static bool CheckAndEndGameForPyromaniacWin(LogicGameFlowNormal __instance) {
            return TryEndGame(Pyromaniac.triggerPyromaniacWin, CustomGameOverReason.PyromaniacWin); 
        }
        private static bool CheckAndEndGameForTreasureHunterWin(LogicGameFlowNormal __instance) {
            return TryEndGame(TreasureHunter.triggertreasureHunterWin, CustomGameOverReason.TreasureHunterWin);
        }
        private static bool CheckAndEndGameForDevourerWin(LogicGameFlowNormal __instance) {
            return TryEndGame(Devourer.triggerdevourerWin, CustomGameOverReason.DevourerWin);
        }
        private static bool CheckAndEndGameForPoisonerWin(LogicGameFlowNormal __instance) {
            return TryEndGame(Poisoner.triggerPoisonerWin, CustomGameOverReason.PoisonerWin);
        }
        private static bool CheckAndEndGameForPuppeteerWin(LogicGameFlowNormal __instance) {
            return TryEndGame(Puppeteer.triggerPuppeteerWin, CustomGameOverReason.PuppeteerWin); 
        }

        private static bool CheckAndEndGameForExilerWin(LogicGameFlowNormal __instance) {
            return TryEndGame(Exiler.triggerExilerWin, CustomGameOverReason.ExilerWin);
        }
        private static bool CheckAndEndGameForSeekerWin(LogicGameFlowNormal __instance) {
            return TryEndGame(Seeker.triggerSeekerWin, CustomGameOverReason.SeekerWin);
        }
        private static bool CheckAndEndGameForKidLose(LogicGameFlowNormal __instance) {
            return TryEndGame(Kid.triggerKidLose, CustomGameOverReason.KidLose);
        }
        private static bool CheckAndEndGameForRenegadeWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            if (statistics.TeamRenegadeAlive >= statistics.TotalAlive - statistics.TeamRenegadeAlive + statistics.TeamCaptainAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive != statistics.TeamRenegadeAlive && !(statistics.TeamRenegadeHasAliveLover && statistics.TeamLoversAlive == 2)) {
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.TeamRenegadeWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBountyHunterWin(LogicGameFlowNormal __instance) {
            return TryEndGame(BountyHunter.triggerBountyHunterWin, CustomGameOverReason.BountyHunterWin);
        }
        private static bool CheckAndEndGameForTrapperWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            return TryEndStatisticsGame(statistics.TeamTrapperAlive, statistics, CustomGameOverReason.TrapperWin);
        }
        private static bool CheckAndEndGameForYinyangerWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            return TryEndStatisticsGame(statistics.TeamYinyangerAlive, statistics, CustomGameOverReason.YinyangerWin);
        }
        private static bool CheckAndEndGameForChallengerWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            return TryEndGame(Challenger.triggerChallengerWin, CustomGameOverReason.ChallengerWin) || TryEndStatisticsGame(statistics.TeamChallengerAlive, statistics, CustomGameOverReason.ChallengerWin);
        }
        private static bool CheckAndEndGameForNinjaWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            return TryEndStatisticsGame(statistics.TeamNinjaAlive, statistics, CustomGameOverReason.NinjaWin);
        }
        private static bool CheckAndEndGameForBerserkerWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            return TryEndStatisticsGame(statistics.TeamBerserkerAlive, statistics, CustomGameOverReason.BerserkerWin);
        }
        private static bool CheckAndEndGameForYandereWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            return TryEndGame(Yandere.triggerYandereWin && !Yandere.rampageMode, CustomGameOverReason.YandereWin) || (Yandere.rampageMode && TryEndStatisticsGame(statistics.TeamYandereAlive, statistics, CustomGameOverReason.YandereWin));
        }
        private static bool CheckAndEndGameForStrandedWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            return TryEndGame(Stranded.triggerStrandedWin, CustomGameOverReason.StrandedWin) || TryEndStatisticsGame(statistics.TeamStrandedAlive, statistics, CustomGameOverReason.StrandedWin);
        }
        private static bool CheckAndEndGameForMonjaWin(LogicGameFlowNormal __instance, PlayerStatistics statistics) {
            return TryEndStatisticsGame(statistics.TeamMonjaAlive, statistics, CustomGameOverReason.MonjaWin);
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
        private static bool CheckAndEndGameForGameModeDrawWin(LogicGameFlowNormal __instance) {
            return TryEndGame(LasMonjas.triggerGamemodesDrawWin, CustomGameOverReason.GamemodesDrawWin);
        }
        private static bool CheckAndEndGameForRedTeamFlagWin(LogicGameFlowNormal __instance) {
            return TryEndGame(CaptureTheFlag.triggerRedTeamWin, CustomGameOverReason.RedTeamFlagWin);
        }
        private static bool CheckAndEndGameForBlueTeamFlagWin(LogicGameFlowNormal __instance) {
            return TryEndGame(CaptureTheFlag.triggerBlueTeamWin, CustomGameOverReason.BlueTeamFlagWin); 
        }
        private static bool CheckAndEndGameForThiefModeThiefWin(LogicGameFlowNormal __instance) {
            return TryEndGame(PoliceAndThief.triggerThiefWin, CustomGameOverReason.ThiefModeThiefWin);
        }
        private static bool CheckAndEndGameForThiefModePoliceWin(LogicGameFlowNormal __instance) {
            return TryEndGame(PoliceAndThief.triggerPoliceWin, CustomGameOverReason.ThiefModePoliceWin);
        }
        private static bool CheckAndEndGameForGreenTeamHillWin(LogicGameFlowNormal __instance) {
            return TryEndGame(KingOfTheHill.triggerGreenTeamWin, CustomGameOverReason.GreenTeamHillWin);
        }
        private static bool CheckAndEndGameForYellowTeamHillWin(LogicGameFlowNormal __instance) {
            return TryEndGame(KingOfTheHill.triggerYellowTeamWin, CustomGameOverReason.YellowTeamHillWin);
        }
        private static bool CheckAndEndGameForHotPotatoEnd(LogicGameFlowNormal __instance) {
            return TryEndGame(HotPotato.triggerHotPotatoEnd, CustomGameOverReason.HotPotatoEnd);
        }
        private static bool CheckAndEndGameForZombieWin(LogicGameFlowNormal __instance) {
            return TryEndGame(ZombieLaboratory.triggerZombieWin, CustomGameOverReason.ZombieWin);
        }
        private static bool CheckAndEndGameForSurvivorWin(LogicGameFlowNormal __instance) {
            return TryEndGame(ZombieLaboratory.triggerSurvivorWin, CustomGameOverReason.SurvivorWin);
        }
        private static bool CheckAndEndGameForBattleRoyaleSoloWin(LogicGameFlowNormal __instance) {
            return TryEndGame(BattleRoyale.triggerSoloWin, CustomGameOverReason.BattleRoyaleSoloWin);
        }
        private static bool CheckAndEndGameForBattleRoyaleTimeWin(LogicGameFlowNormal __instance) {
            return TryEndGame(BattleRoyale.triggerTimeWin, CustomGameOverReason.BattleRoyaleTimeWin);
        }
        private static bool CheckAndEndGameForBattleRoyaleLimeTeamWin(LogicGameFlowNormal __instance) {
            return TryEndGame(BattleRoyale.triggerLimeTeamWin, CustomGameOverReason.BattleRoyaleLimeTeamWin);
        }
        private static bool CheckAndEndGameForBattleRoyalePinkTeamWin(LogicGameFlowNormal __instance) {
            return TryEndGame(BattleRoyale.triggerPinkTeamWin, CustomGameOverReason.BattleRoyalePinkTeamWin); 
        }
        private static bool CheckAndEndGameForBattleRoyaleSerialKillerWin(LogicGameFlowNormal __instance) {
            return TryEndGame(BattleRoyale.triggerSerialKillerWin, CustomGameOverReason.BattleRoyaleSerialKillerWin); 
        }
        private static bool CheckAndEndGameForMonjaFestivalGreenTeamWin(LogicGameFlowNormal __instance) {
            return TryEndGame(MonjaFestival.triggerGreenTeamWin, CustomGameOverReason.MonjaFestivalGreenWin);
        }
        private static bool CheckAndEndGameForMonjaFestivalCyanTeamWin(LogicGameFlowNormal __instance) {
            return TryEndGame(MonjaFestival.triggerCyanTeamWin, CustomGameOverReason.MonjaFestivalCyanWin);
        }
        private static bool CheckAndEndGameForMonjaFestivalBigMonjaWin(LogicGameFlowNormal __instance) {
            return TryEndGame(MonjaFestival.triggerBigMonjaWin, CustomGameOverReason.MonjaFestivalBigMonjaWin);
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