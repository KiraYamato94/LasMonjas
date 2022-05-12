  using HarmonyLib;
using static LasMonjas.LasMonjas;
using static LasMonjas.GameHistory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hazel;
using UnhollowerBaseLib;
using System;
using System.Text;

namespace LasMonjas.Patches {
    enum CustomGameOverReason {
        LoversWin = 10,
        TeamRenegadeWin = 11,
        KidLose = 12,
        JokerWin = 13,
        PyromaniacWin = 14,
        BombExploded = 15,
        BountyHunterWin = 16,
        TreasureHunterWin = 17,
        DevourerWin = 18,
        TrapperWin = 19,
        YinyangerWin = 20,
        ChallengerWin = 21,
        RedTeamFlagWin = 22,
        BlueTeamFlagWin = 23,
        DrawTeamWin = 24,
        ThiefModeThiefWin = 25,
        ThiefModePoliceWin = 26, 
        GreenTeamHillWin = 27,
        YellowTeamHillWin = 28,
        TeamHillDraw = 29,
        HotPotatoEnd = 30,
        ZombieWin = 31,
        SurvivorWin = 32
    }

    enum WinCondition {
        Default,
        LoversTeamWin,
        LoversSoloWin,
        JokerWin,
        RenegadeWin,
        KidLose,
        PyromaniacWin,
        BombExploded,
        BountyHunterWin,
        TreasureHunterWin,
        DevourerWin,
        TrapperWin,
        YinyangerWin,
        ChallengerWin,
        RedTeamFlagWin,
        BlueTeamFlagWin,
        DrawTeamWin,
        ThiefModeThiefWin,
        ThiefModePoliceWin,
        GreenTeamHillWin,
        YellowTeamHillWin,
        TeamHillDraw,
        HotPotatoEnd,
        ZombieWin,
        SurvivorWin
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
        }
    }


    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public class OnGameEndPatch {
        private static GameOverReason gameOverReason;
        public static void Prefix(AmongUsClient __instance, [HarmonyArgument(0)]ref EndGameResult endGameResult) {
            gameOverReason = endGameResult.GameOverReason;
            if ((int)endGameResult.GameOverReason >= 10) endGameResult.GameOverReason = GameOverReason.ImpostorByKill;
        }

        public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)]ref EndGameResult endGameResult) {
            AdditionalTempData.clear();

            foreach(var playerControl in PlayerControl.AllPlayerControls) {
                var roles = RoleInfo.getRoleInfoForPlayer(playerControl);
                var (tasksCompleted, tasksTotal) = TasksHandler.taskInfo(playerControl.Data);
                AdditionalTempData.playerRoles.Add(new AdditionalTempData.PlayerRoleInfo() { PlayerName = playerControl.Data.PlayerName, Roles = roles, TasksTotal = tasksTotal, TasksCompleted = tasksCompleted });
            }

            List<PlayerControl> notWinners = new List<PlayerControl>();
            if (Joker.joker != null) notWinners.Add(Joker.joker);
            if (RoleThief.rolethief != null) notWinners.Add(RoleThief.rolethief);
            if (Pyromaniac.pyromaniac != null) notWinners.Add(Pyromaniac.pyromaniac);
            if (TreasureHunter.treasureHunter != null) notWinners.Add(TreasureHunter.treasureHunter);
            if (Devourer.devourer != null) notWinners.Add(Devourer.devourer);
            if (Renegade.renegade != null) notWinners.Add(Renegade.renegade);
            if (Minion.minion != null) notWinners.Add(Minion.minion);
            if (BountyHunter.bountyhunter != null) notWinners.Add(BountyHunter.bountyhunter);
            if (Trapper.trapper != null) notWinners.Add(Trapper.trapper);
            if (Yinyanger.yinyanger != null) notWinners.Add(Yinyanger.yinyanger);
            if (Challenger.challenger != null) notWinners.Add(Challenger.challenger);

            // Remove neutral custom gamemode roles from winners
            if (CaptureTheFlag.stealerPlayer != null) notWinners.Add(CaptureTheFlag.stealerPlayer);
            if (KingOfTheHill.usurperPlayer != null) notWinners.Add(KingOfTheHill.usurperPlayer);
            if (HotPotato.hotPotatoPlayer != null) notWinners.Add(HotPotato.hotPotatoPlayer);

            notWinners.AddRange(Renegade.formerRenegades);

            List<WinningPlayerData> winnersToRemove = new List<WinningPlayerData>();
            foreach (WinningPlayerData winner in TempData.winners) {
                if (notWinners.Any(x => x.Data.PlayerName == winner.PlayerName)) winnersToRemove.Add(winner);
            }
            foreach (var winner in winnersToRemove) TempData.winners.Remove(winner);

            bool jokerWin = Joker.joker != null && gameOverReason == (GameOverReason)CustomGameOverReason.JokerWin;
            bool pyromaniacWin = Pyromaniac.pyromaniac != null && gameOverReason == (GameOverReason)CustomGameOverReason.PyromaniacWin;
            bool kidLose = Kid.kid != null && gameOverReason == (GameOverReason)CustomGameOverReason.KidLose;
            bool loversWin = Modifiers.existingAndAlive() && (gameOverReason == (GameOverReason)CustomGameOverReason.LoversWin || (TempData.DidHumansWin(gameOverReason) && !Modifiers.existingWithKiller())); 
            bool teamRenegadeWin = gameOverReason == (GameOverReason)CustomGameOverReason.TeamRenegadeWin && ((Renegade.renegade != null && !Renegade.renegade.Data.IsDead) || (Minion.minion != null && !Minion.minion.Data.IsDead));
            bool bombExploded = Bomberman.bomberman != null && gameOverReason == (GameOverReason)CustomGameOverReason.BombExploded;
            bool bountyhunterWin = BountyHunter.bountyhunter != null && gameOverReason == (GameOverReason)CustomGameOverReason.BountyHunterWin;
            bool treasurehunterWin = TreasureHunter.treasureHunter != null && gameOverReason == (GameOverReason)CustomGameOverReason.TreasureHunterWin;
            bool devourerWin = Devourer.devourer != null && gameOverReason == (GameOverReason)CustomGameOverReason.DevourerWin;
            bool trapperWin = Trapper.trapper != null && gameOverReason == (GameOverReason)CustomGameOverReason.TrapperWin;
            bool yinyangerWin = Yinyanger.yinyanger != null && gameOverReason == (GameOverReason)CustomGameOverReason.YinyangerWin;
            bool challengerWin = Challenger.challenger != null && gameOverReason == (GameOverReason)CustomGameOverReason.ChallengerWin;
            bool redTeamFlagWin = CaptureTheFlag.captureTheFlagMode && gameOverReason == (GameOverReason)CustomGameOverReason.RedTeamFlagWin;
            bool blueTeamFlagWin = CaptureTheFlag.captureTheFlagMode && gameOverReason == (GameOverReason)CustomGameOverReason.BlueTeamFlagWin;
            bool drawTeamWin = CaptureTheFlag.captureTheFlagMode && gameOverReason == (GameOverReason)CustomGameOverReason.DrawTeamWin;
            bool thiefModeThiefWin = PoliceAndThief.policeAndThiefMode && gameOverReason == (GameOverReason)CustomGameOverReason.ThiefModeThiefWin;
            bool thiefModePoliceWin = PoliceAndThief.policeAndThiefMode && gameOverReason == (GameOverReason)CustomGameOverReason.ThiefModePoliceWin;
            bool greenTeamHillWin = KingOfTheHill.kingOfTheHillMode && gameOverReason == (GameOverReason)CustomGameOverReason.GreenTeamHillWin;
            bool yellowTeamHillWin = KingOfTheHill.kingOfTheHillMode && gameOverReason == (GameOverReason)CustomGameOverReason.YellowTeamHillWin;
            bool teamHillDraw = KingOfTheHill.kingOfTheHillMode && gameOverReason == (GameOverReason)CustomGameOverReason.TeamHillDraw;
            bool hotPotatoEnd = HotPotato.hotPotatoMode && gameOverReason == (GameOverReason)CustomGameOverReason.HotPotatoEnd;
            bool zombieWin = ZombieLaboratory.zombieLaboratoryMode && gameOverReason == (GameOverReason)CustomGameOverReason.ZombieWin;
            bool survivorWin = ZombieLaboratory.zombieLaboratoryMode && gameOverReason == (GameOverReason)CustomGameOverReason.SurvivorWin;

            // Kid lose
            if (kidLose) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Kid.kid.Data);
                wpd.IsYou = false;
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.KidLose;
            }

            // Joker win
            else if (jokerWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Joker.joker.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.JokerWin;
            }

            // Pyromaniac win
            else if (pyromaniacWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Pyromaniac.pyromaniac.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.PyromaniacWin;
            }

            // Lovers win conditions
            else if (loversWin) {
                // Double win for lovers with crewmates
                if (!Modifiers.existingWithKiller()) {
                    AdditionalTempData.winCondition = WinCondition.LoversTeamWin;
                    TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                    foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                        if (p == null) continue;
                        if (p == Modifiers.lover1 || p == Modifiers.lover2)
                            TempData.winners.Add(new WinningPlayerData(p.Data));
                        else if (p != Joker.joker && p != RoleThief.rolethief && p != Pyromaniac.pyromaniac && p != TreasureHunter.treasureHunter && p != Devourer.devourer && p != Renegade.renegade && p != Minion.minion && !Renegade.formerRenegades.Contains(p) && p != BountyHunter.bountyhunter && p != Trapper.trapper && p != Yinyanger.yinyanger && p != Challenger.challenger && !p.Data.Role.IsImpostor)
                            TempData.winners.Add(new WinningPlayerData(p.Data));
                    }
                }
                // Lovers solo win
                else {
                    AdditionalTempData.winCondition = WinCondition.LoversSoloWin;
                    TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                    TempData.winners.Add(new WinningPlayerData(Modifiers.lover1.Data));
                    TempData.winners.Add(new WinningPlayerData(Modifiers.lover2.Data));
                }
            }

            // Renegade win condition
            else if (teamRenegadeWin) {
                // Renegade wins if nobody except renegade is alive
                AdditionalTempData.winCondition = WinCondition.RenegadeWin;
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Renegade.renegade.Data);
                wpd.IsImpostor = false;
                TempData.winners.Add(wpd);
                // If there is a minion. The minion also wins
                if (Minion.minion != null) {
                    WinningPlayerData wpdMinion = new WinningPlayerData(Minion.minion.Data);
                    wpdMinion.IsImpostor = false;
                    TempData.winners.Add(wpdMinion);
                }
                foreach (var player in Renegade.formerRenegades) {
                    WinningPlayerData wpdFormerRenegade = new WinningPlayerData(player.Data);
                    wpdFormerRenegade.IsImpostor = false;
                    TempData.winners.Add(wpdFormerRenegade);
                }
            }

            // Bomb exploded
            else if (bombExploded) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl imimpostor in PlayerControl.AllPlayerControls) {
                    if (imimpostor.Data.Role.IsImpostor == true) {
                        WinningPlayerData wpd = new WinningPlayerData(imimpostor.Data);
                        TempData.winners.Add(wpd);
                    }
                }
                AdditionalTempData.winCondition = WinCondition.BombExploded;
            }

            // BountyHunter win
            else if (bountyhunterWin) {
                // BountyHunter wins if he kills his target 
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(BountyHunter.bountyhunter.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.BountyHunterWin;
            }

            // TreasureHunter win
            else if (treasurehunterWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(TreasureHunter.treasureHunter.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.TreasureHunterWin;
            }

            // Devourer win
            else if (devourerWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Devourer.devourer.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.DevourerWin;
            }

            // Trapper win
            else if (trapperWin) {
                // Trapper wins if nobody except Trapper is alive
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Trapper.trapper.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.TrapperWin;
            }

            // Yinyanger win
            else if (yinyangerWin) {
                // Yinyanger wins if nobody except Yinyanger is alive
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Yinyanger.yinyanger.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.YinyangerWin;
            }

            // Challenger win
            else if (challengerWin) {
                // Challenger wins if nobody except Challenger is alive
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Challenger.challenger.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.ChallengerWin;
            }

            // Flag Game Mode Win
            // Draw
            else if (drawTeamWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.DrawTeamWin;
            }
            // Red Team Win
            else if (redTeamFlagWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.RedTeamFlagWin;
            }
            // Blue Team Win
            else if (blueTeamFlagWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.BlueTeamFlagWin;
            }

            // Thief Mode Win
            // Thief Team Win
            else if (thiefModeThiefWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.ThiefModeThiefWin;
            }
            // Police Team Win
            else if (thiefModePoliceWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.ThiefModePoliceWin;
            }

            // King Game Mode Win
            // Draw
            else if (teamHillDraw) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.TeamHillDraw;
            }
            // Green Team Win
            else if (greenTeamHillWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.GreenTeamHillWin;
            }
            // Yellow Team Win
            else if (yellowTeamHillWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.YellowTeamHillWin;
            }

            // Hot Potato Game Mode Win
            else if (hotPotatoEnd) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in HotPotato.notPotatoTeamAlive) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.HotPotatoEnd;
            }

            // ZombieLaboratory zombie Win
            else if (zombieWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.ZombieWin;
            }
            else if (survivorWin) {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                    WinningPlayerData wpd = new WinningPlayerData(player.Data);
                    TempData.winners.Add(wpd);
                }
                AdditionalTempData.winCondition = WinCondition.SurvivorWin;
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

            if (AdditionalTempData.winCondition == WinCondition.JokerWin) {
                textRenderer.text = "Joker Wins";
                textRenderer.color = Joker.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(3);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(3);
            }
            else if (AdditionalTempData.winCondition == WinCondition.PyromaniacWin) {
                textRenderer.text = "Pyromaniac Wins";
                textRenderer.color = Pyromaniac.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(3);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(3);
            }
            else if (AdditionalTempData.winCondition == WinCondition.LoversTeamWin) {
                textRenderer.text = "Lovers and Crewmates Win";
                textRenderer.color = Modifiers.loverscolor;
                __instance.BackgroundBar.material.SetColor("_Color", Modifiers.loverscolor);
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(5);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(5);
            }
            else if (AdditionalTempData.winCondition == WinCondition.LoversSoloWin) {
                textRenderer.text = "Lover Team Win";
                textRenderer.color = Modifiers.loverscolor;
                __instance.BackgroundBar.material.SetColor("_Color", Modifiers.loverscolor);
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(6);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(6);
            }
            else if (AdditionalTempData.winCondition == WinCondition.RenegadeWin) {
                textRenderer.text = "Renegade Team Win";
                textRenderer.color = Renegade.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(4);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(4);
            }
            else if (AdditionalTempData.winCondition == WinCondition.KidLose) {
                textRenderer.text = "Kid Win";
                textRenderer.color = Kid.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(6);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(6);
            }
            else if (AdditionalTempData.winCondition == WinCondition.BombExploded) {
                textRenderer.text = "Bomb Exploded";
                textRenderer.color = Bomberman.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(6);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(6);
            }
            else if (AdditionalTempData.winCondition == WinCondition.BountyHunterWin) {
                textRenderer.text = "Bounty Hunter Wins";
                textRenderer.color = BountyHunter.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(4);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(4);
            }
            else if (AdditionalTempData.winCondition == WinCondition.TreasureHunterWin) {
                textRenderer.text = "Treasure Hunter Wins";
                textRenderer.color = TreasureHunter.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(3);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(3);
            }
            else if (AdditionalTempData.winCondition == WinCondition.DevourerWin) {
                textRenderer.text = "Devourer Wins";
                textRenderer.color = Devourer.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(3);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(3);
            }
            else if (AdditionalTempData.winCondition == WinCondition.TrapperWin) {
                textRenderer.text = "Trapper Wins";
                textRenderer.color = Trapper.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(4);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(4);
            }
            else if (AdditionalTempData.winCondition == WinCondition.YinyangerWin) {
                textRenderer.text = "Yinyanger Wins";
                textRenderer.color = Yinyanger.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(4);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(4);
            }
            else if (AdditionalTempData.winCondition == WinCondition.ChallengerWin) {
                textRenderer.text = "Challenger Wins";
                textRenderer.color = Challenger.color;
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(4);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(4);
            }
            else if (AdditionalTempData.winCondition == WinCondition.DrawTeamWin || AdditionalTempData.winCondition == WinCondition.TeamHillDraw) {
                textRenderer.text = "Draw";
                textRenderer.color = new Color32(255, 128, 0, byte.MaxValue); 
            }
            else if (AdditionalTempData.winCondition == WinCondition.RedTeamFlagWin) {
                textRenderer.text = "Red Team Win";
                textRenderer.color = Color.red; 
            }
            else if (AdditionalTempData.winCondition == WinCondition.BlueTeamFlagWin) {
                textRenderer.text = "Blue Team Win";
                textRenderer.color = Color.blue; 
            }
            else if (AdditionalTempData.winCondition == WinCondition.ThiefModePoliceWin) {
                textRenderer.text = "Police Team Win";
                textRenderer.color = Color.cyan; 
            }
            else if (AdditionalTempData.winCondition == WinCondition.ThiefModeThiefWin) {
                textRenderer.text = "Thief Team Win";
                textRenderer.color = Mechanic.color;
            }
            else if (AdditionalTempData.winCondition == WinCondition.GreenTeamHillWin) {
                textRenderer.text = "Green Team Win";
                textRenderer.color = Color.green;
            }
            else if (AdditionalTempData.winCondition == WinCondition.YellowTeamHillWin) {
                textRenderer.text = "Yellow Team Win";
                textRenderer.color = Color.yellow;
            }
            else if (AdditionalTempData.winCondition == WinCondition.HotPotatoEnd) {
                textRenderer.text = "Cold Potato Team Win";
                textRenderer.color = Color.cyan;
            }
            else if (AdditionalTempData.winCondition == WinCondition.ZombieWin) {
                textRenderer.text = "Zombie Team Win";
                textRenderer.color = Mechanic.color;
            }
            else if (AdditionalTempData.winCondition == WinCondition.SurvivorWin) {
                textRenderer.text = "Survivor Team Win";
                textRenderer.color = Medusa.color;
            }
            else {
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(5);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(5);
            }

            if (MapOptions.showRoleSummary) {
                var position = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, Camera.main.nearClipPlane));
                GameObject roleSummary = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
                roleSummary.transform.position = new Vector3(__instance.Navigation.ExitButton.transform.position.x + 0.1f, position.y - 0.1f, -14f);
                roleSummary.transform.localScale = new Vector3(1f, 1f, 1f);

                var roleSummaryText = new StringBuilder();
                roleSummaryText.AppendLine("Game Summary (Players / Roles / Tasks):");
                foreach (var data in AdditionalTempData.playerRoles) {
                    var roles = string.Join(" ", data.Roles.Select(x => Helpers.cs(x.color, x.name)));
                    var taskInfo = data.TasksTotal > 0 ? $" - <color=#FAD934FF>({data.TasksCompleted}/{data.TasksTotal})</color>" : "";
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

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CheckEndCriteria))] 
    class CheckEndCriteriaPatch {
        public static bool Prefix(ShipStatus __instance) {
            if (!GameData.Instance) return false;
            if (DestroyableSingleton<TutorialManager>.InstanceExists) 
                return true;
            var statistics = new PlayerStatistics(__instance);
            if (CheckAndEndGameForKidLose(__instance)) return false;
            if (CheckAndEndGameForBountyHunterWin(__instance)) return false;
            if (CheckAndEndGameForJokerWin(__instance)) return false;
            if (CheckAndEndGameForPyromaniacWin(__instance)) return false;
            if (CheckAndEndGameForTreasureHunterWin(__instance)) return false;
            if (CheckAndEndGameForDevourerWin(__instance)) return false;
            if (CheckAndEndGameForTrapperWin(__instance, statistics)) return false;
            if (CheckAndEndGameForYinyangerWin(__instance, statistics)) return false;
            if (CheckAndEndGameForChallengerWin(__instance, statistics)) return false;
            if (CheckAndEndGameForSabotageWin(__instance)) return false;
            if (CheckAndEndGameForTaskWin(__instance)) return false;
            if (CheckAndEndGameForLoverWin(__instance, statistics)) return false;
            if (CheckAndEndGameForRenegadeWin(__instance, statistics)) return false;
            if (CheckAndEndGameForImpostorWin(__instance, statistics)) return false;
            if (CheckAndEndGameForCrewmateWin(__instance, statistics)) return false;
            if (CheckAndEndGameForBombExploded(__instance)) return false;
            if (CheckAndEndGameForRedTeamFlagWin(__instance)) return false;
            if (CheckAndEndGameForBlueTeamFlagWin(__instance)) return false;
            if (CheckAndEndGameForDrawFlagWin(__instance)) return false;
            if (CheckAndEndGameForThiefModeThiefWin(__instance)) return false;
            if (CheckAndEndGameForThiefModePoliceWin(__instance)) return false;
            if (CheckAndEndGameForGreenTeamHillWin(__instance)) return false;
            if (CheckAndEndGameForYellowTeamHillWin(__instance)) return false;
            if (CheckAndEndGameForDrawHillWin(__instance)) return false;
            if (CheckAndEndGameForHotPotatoEnd(__instance)) return false;
            if (CheckAndEndGameForZombieWin(__instance)) return false;
            if (CheckAndEndGameForSurvivorWin(__instance)) return false;
            return false;
        }

        private static bool CheckAndEndGameForKidLose(ShipStatus __instance) {
            if (Kid.triggerKidLose) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.KidLose, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBountyHunterWin(ShipStatus __instance) {
            if (BountyHunter.triggerBountyHunterWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.BountyHunterWin, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForJokerWin(ShipStatus __instance) {
            if (Joker.triggerJokerWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.JokerWin, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForPyromaniacWin(ShipStatus __instance) {
            if (Pyromaniac.triggerPyromaniacWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.PyromaniacWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForTreasureHunterWin(ShipStatus __instance) {
            if (TreasureHunter.triggertreasureHunterWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.TreasureHunterWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForDevourerWin(ShipStatus __instance) {
            if (Devourer.triggerdevourerWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.DevourerWin, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForSabotageWin(ShipStatus __instance) {
            if (__instance.Systems == null) return false;
            ISystemType systemType = __instance.Systems.ContainsKey(SystemTypes.LifeSupp) ? __instance.Systems[SystemTypes.LifeSupp] : null;
            if (systemType != null) {
                LifeSuppSystemType lifeSuppSystemType = systemType.TryCast<LifeSuppSystemType>();
                if (lifeSuppSystemType != null && lifeSuppSystemType.Countdown < 0f) {
                    EndGameForSabotage(__instance);
                    lifeSuppSystemType.Countdown = 10000f;
                    return true;
                }
            }
            ISystemType systemType2 = __instance.Systems.ContainsKey(SystemTypes.Reactor) ? __instance.Systems[SystemTypes.Reactor] : null;
            if (systemType2 == null) {
                systemType2 = __instance.Systems.ContainsKey(SystemTypes.Laboratory) ? __instance.Systems[SystemTypes.Laboratory] : null;
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

        private static bool CheckAndEndGameForTaskWin(ShipStatus __instance) {
            if (GameData.Instance.TotalTasks <= GameData.Instance.CompletedTasks && howmanygamemodesareon != 1) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame(GameOverReason.HumansByTask, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForLoverWin(ShipStatus __instance, PlayerStatistics statistics) {
            if (statistics.TeamLoversAlive == 2 && statistics.TotalAlive <= 3) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.LoversWin, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForRenegadeWin(ShipStatus __instance, PlayerStatistics statistics) {
            if (statistics.TeamRenegadeAlive >= statistics.TotalAlive - statistics.TeamRenegadeAlive + statistics.TeamCaptainAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive != statistics.TeamRenegadeAlive && !(statistics.TeamRenegadeHasAliveLover && statistics.TeamLoversAlive == 2)) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.TeamRenegadeWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForTrapperWin(ShipStatus __instance, PlayerStatistics statistics) {
            if (statistics.TeamTrapperAlive >= statistics.TotalAlive - statistics.TeamTrapperAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.TrapperWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForYinyangerWin(ShipStatus __instance, PlayerStatistics statistics) {
            if (statistics.TeamYinyangerAlive >= statistics.TotalAlive - statistics.TeamYinyangerAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.YinyangerWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForChallengerWin(ShipStatus __instance, PlayerStatistics statistics) {
            if (statistics.TeamChallengerAlive >= statistics.TotalAlive - statistics.TeamChallengerAlive && statistics.TeamImpostorsAlive == 0 && statistics.TeamCaptainAlive == 0 && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.ChallengerWin, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForImpostorWin(ShipStatus __instance, PlayerStatistics statistics) {
            if (howmanygamemodesareon != 1 && statistics.TeamImpostorsAlive >= statistics.TotalAlive - statistics.TeamImpostorsAlive + statistics.TeamCaptainAlive && statistics.TeamRenegadeAlive == 0 && statistics.TeamBountyHunterAlive == 0 && statistics.TeamTrapperAlive == 0 && statistics.TeamYinyangerAlive == 0 && statistics.TeamChallengerAlive == 0 && statistics.TeamCaptainAlive != statistics.TeamImpostorsAlive && !(statistics.TeamImpostorHasAliveLover && statistics.TeamLoversAlive == 2)) {
                __instance.enabled = false;
                GameOverReason endReason;
                switch (TempData.LastDeathReason) {
                    case DeathReason.Exile:
                        endReason = GameOverReason.ImpostorByVote;
                        break;
                    case DeathReason.Kill:
                        endReason = GameOverReason.ImpostorByKill;
                        break;
                    default:
                        endReason = GameOverReason.ImpostorByVote;
                        break;
                }
                ShipStatus.RpcEndGame(endReason, false);
                return true;
            }
            return false;
        }

        private static bool CheckAndEndGameForCrewmateWin(ShipStatus __instance, PlayerStatistics statistics) {
            if (howmanygamemodesareon != 1 && statistics.TeamImpostorsAlive == 0 && statistics.TeamRenegadeAlive == 0 && statistics.TeamBountyHunterAlive == 0 && statistics.TeamTrapperAlive == 0 && statistics.TeamYinyangerAlive == 0 && statistics.TeamChallengerAlive == 0) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame(GameOverReason.HumansByVote, false);
                return true;
            }
            return false;
        }

        private static void EndGameForSabotage(ShipStatus __instance) {
            __instance.enabled = false;
            ShipStatus.RpcEndGame(GameOverReason.ImpostorBySabotage, false);
            return;
        }
        private static bool CheckAndEndGameForBombExploded(ShipStatus __instance) {
            if (Bomberman.triggerBombExploded) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.BombExploded, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForRedTeamFlagWin(ShipStatus __instance) {
            if (CaptureTheFlag.triggerRedTeamWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.RedTeamFlagWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForBlueTeamFlagWin(ShipStatus __instance) {
            if (CaptureTheFlag.triggerBlueTeamWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.BlueTeamFlagWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForDrawFlagWin(ShipStatus __instance) {
            if (CaptureTheFlag.triggerDrawWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.DrawTeamWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForThiefModeThiefWin(ShipStatus __instance) {
            if (PoliceAndThief.triggerThiefWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModeThiefWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForThiefModePoliceWin(ShipStatus __instance) {
            if (PoliceAndThief.triggerPoliceWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModePoliceWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForGreenTeamHillWin(ShipStatus __instance) {
            if (KingOfTheHill.triggerGreenTeamWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.GreenTeamHillWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForYellowTeamHillWin(ShipStatus __instance) {
            if (KingOfTheHill.triggerYellowTeamWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.YellowTeamHillWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForDrawHillWin(ShipStatus __instance) {
            if (KingOfTheHill.triggerDrawWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.TeamHillDraw, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForHotPotatoEnd(ShipStatus __instance) {
            if (HotPotato.triggerHotPotatoEnd) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.HotPotatoEnd, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForZombieWin(ShipStatus __instance) {
            if (ZombieLaboratory.triggerZombieWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.ZombieWin, false);
                return true;
            }
            return false;
        }
        private static bool CheckAndEndGameForSurvivorWin(ShipStatus __instance) {
            if (ZombieLaboratory.triggerSurvivorWin) {
                __instance.enabled = false;
                ShipStatus.RpcEndGame((GameOverReason)CustomGameOverReason.SurvivorWin, false);
                return true;
            }
            return false;
        }

    }

    internal class PlayerStatistics
    {
        public int TeamImpostorsAlive { get; set; }
        public int TeamRenegadeAlive { get; set; }
        public int TeamLoversAlive { get; set; }
        public int TotalAlive { get; set; }
        public bool TeamImpostorHasAliveLover { get; set; }
        public bool TeamRenegadeHasAliveLover { get; set; }
        public int TeamTrapperAlive { get; set; }
        public int TeamBountyHunterAlive { get; set; }
        public int TeamYinyangerAlive { get; set; }
        public int TeamCaptainAlive { get; set; }
        public int TeamChallengerAlive { get; set; }

        public PlayerStatistics(ShipStatus __instance) {
            GetPlayerCounts();
        }

        private bool isLover(GameData.PlayerInfo p) {
            return (Modifiers.lover1 != null && Modifiers.lover1.PlayerId == p.PlayerId) || (Modifiers.lover2 != null && Modifiers.lover2.PlayerId == p.PlayerId);
        }

        private void GetPlayerCounts() {
            int numRenegadeAlive = 0;
            int numImpostorsAlive = 0;
            int numLoversAlive = 0;
            int numTotalAlive = 0;
            bool impLover = false;
            bool renegadeLover = false;
            int numTrapperAlive = 0;
            int numBountyHunterAlive = 0;
            int numYinyangerAlive = 0;
            int numCaptainAlive = 0;
            int numChallengerAlive = 0;

            for (int i = 0; i < GameData.Instance.PlayerCount; i++) {
                GameData.PlayerInfo playerInfo = GameData.Instance.AllPlayers[i];
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
                        if (Trapper.trapper != null && Trapper.trapper.PlayerId == playerInfo.PlayerId) {
                            numTrapperAlive++;
                        }
                        if (BountyHunter.bountyhunter != null && BountyHunter.bountyhunter.PlayerId == playerInfo.PlayerId) {
                            numBountyHunterAlive++;
                        }
                        if (Yinyanger.yinyanger != null && Yinyanger.yinyanger.PlayerId == playerInfo.PlayerId) {
                            numYinyangerAlive++;
                        }
                        if (Captain.captain != null && Captain.captain.PlayerId == playerInfo.PlayerId) {
                            numCaptainAlive++;
                        }
                        if (Challenger.challenger != null && Challenger.challenger.PlayerId == playerInfo.PlayerId) {
                            numChallengerAlive++;
                        }
                    }
                }
            }

            TeamRenegadeAlive = numRenegadeAlive;
            TeamImpostorsAlive = numImpostorsAlive;
            TeamLoversAlive = numLoversAlive;
            TotalAlive = numTotalAlive;
            TeamImpostorHasAliveLover = impLover;
            TeamRenegadeHasAliveLover = renegadeLover;
            TeamTrapperAlive = numTrapperAlive;
            TeamBountyHunterAlive = numBountyHunterAlive;
            TeamYinyangerAlive = numYinyangerAlive;
            TeamCaptainAlive = numCaptainAlive;
            TeamChallengerAlive = numChallengerAlive;
        }
    }
}