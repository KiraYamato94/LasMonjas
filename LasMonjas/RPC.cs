using HarmonyLib;
using Hazel;
using static LasMonjas.LasMonjas;
using static LasMonjas.HudManagerStartPatch;
using static LasMonjas.GameHistory;
using static LasMonjas.MapOptions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Reactor;
using LasMonjas.Objects;
using LasMonjas.Patches;
using static LasMonjas.RoleInfo;
using LasMonjas.Core;
using MS.Internal.Xml.XPath;
using static UnityEngine.GraphicsBuffer;
using AmongUs.GameOptions;

namespace LasMonjas
{
    enum RoleId
    {
        Mimic,
        Painter,
        Demon,
        Janitor,
        Illusionist,
        Manipulator,
        Bomberman,
        Chameleon,
        Gambler,
        Sorcerer,
        Medusa,
        Hypnotist,
        Archer,
        Plumber,
        Librarian,
        Renegade,
        Minion,
        BountyHunter,
        Trapper,
        Yinyanger,
        Challenger,
        Ninja,
        Berserker, 
        Yandere, 
        Stranded, 
        Monja, 
        Joker,
        RoleThief,
        Pyromaniac,
        TreasureHunter,
        Devourer,
        Poisoner,
        Puppeteer, 
        Exiler, 
        Amnesiac, 
        Seeker, 
        Captain,
        Mechanic,
        Sheriff,
        Detective,
        Forensic,
        TimeTraveler,
        Squire,
        Cheater,
        FortuneTeller,
        Hacker,
        Sleuth,
        Fink,
        Kid,
        Welder,
        Spiritualist,
        Coward,
        Vigilant,
        VigilantMira,
        Hunter,
        Jinx,
        Bat,
        Necromancer,
        Engineer,
        Shy, 
        TaskMaster,
        Jailer,
        Lover,
        Lighter,
        Blind,
        Flash,
        BigChungus,
        TheChosenOne,
        Performer,
        Pro,
        Paintball,
        Electrician,
        Crewmate,
        Impostor,

        // Capture the flag
        RedPlayer01,
        RedPlayer02,
        RedPlayer03,
        RedPlayer04,
        RedPlayer05,
        RedPlayer06,
        RedPlayer07,
        BluePlayer01,
        BluePlayer02,
        BluePlayer03,
        BluePlayer04,
        BluePlayer05,
        BluePlayer06,
        BluePlayer07,
        StealerPlayer,

        // Police and Thief
        PolicePlayer01,
        PolicePlayer02,
        PolicePlayer03,
        PolicePlayer04,
        PolicePlayer05,
        PolicePlayer06,
        ThiefPlayer01,
        ThiefPlayer02,
        ThiefPlayer03,
        ThiefPlayer04,
        ThiefPlayer05,
        ThiefPlayer06,
        ThiefPlayer07,
        ThiefPlayer08,
        ThiefPlayer09,

        // King of the Hill
        GreenKing,
        GreenPlayer01,
        GreenPlayer02,
        GreenPlayer03,
        GreenPlayer04,
        GreenPlayer05,
        GreenPlayer06,
        YellowKing,
        YellowPlayer01,
        YellowPlayer02,
        YellowPlayer03,
        YellowPlayer04,
        YellowPlayer05,
        YellowPlayer06,
        UsurperPlayer,

        // Hot Potato
        HotPotato,
        NotPotato01,
        NotPotato02,
        NotPotato03,
        NotPotato04,
        NotPotato05,
        NotPotato06,
        NotPotato07,
        NotPotato08,
        NotPotato09,
        NotPotato10,
        NotPotato11,
        NotPotato12,
        NotPotato13,
        NotPotato14,
        ExplodedPotato01,
        ExplodedPotato02,
        ExplodedPotato03,
        ExplodedPotato04,
        ExplodedPotato05,
        ExplodedPotato06,
        ExplodedPotato07,
        ExplodedPotato08,
        ExplodedPotato09,
        ExplodedPotato10,
        ExplodedPotato11,
        ExplodedPotato12,
        ExplodedPotato13,
        ExplodedPotato14,

        // ZombieLaboratory
        NursePlayer,
        SurvivorPlayer01,
        SurvivorPlayer02,
        SurvivorPlayer03,
        SurvivorPlayer04,
        SurvivorPlayer05,
        SurvivorPlayer06,
        SurvivorPlayer07,
        SurvivorPlayer08,
        SurvivorPlayer09,
        SurvivorPlayer10,
        SurvivorPlayer11,
        SurvivorPlayer12,
        SurvivorPlayer13,
        ZombiePlayer01,
        ZombiePlayer02,
        ZombiePlayer03,
        ZombiePlayer04,
        ZombiePlayer05,
        ZombiePlayer06,
        ZombiePlayer07,
        ZombiePlayer08,
        ZombiePlayer09,
        ZombiePlayer10,
        ZombiePlayer11,
        ZombiePlayer12,
        ZombiePlayer13,
        ZombiePlayer14,

        // Battle Royale
        SoloPlayer01,
        SoloPlayer02,
        SoloPlayer03,
        SoloPlayer04,
        SoloPlayer05,
        SoloPlayer06,
        SoloPlayer07,
        SoloPlayer08,
        SoloPlayer09,
        SoloPlayer10,
        SoloPlayer11,
        SoloPlayer12,
        SoloPlayer13,
        SoloPlayer14,
        SoloPlayer15,
        LimePlayer01,
        LimePlayer02,
        LimePlayer03,
        LimePlayer04,
        LimePlayer05,
        LimePlayer06,
        LimePlayer07,
        PinkPlayer01,
        PinkPlayer02,
        PinkPlayer03,
        PinkPlayer04,
        PinkPlayer05,
        PinkPlayer06,
        PinkPlayer07,
        SerialKiller
    }

    enum CustomRPC
    {
        // Main Controls

        ResetVaribles = 60,
        ShareOptions,
        ForceEnd,
        SetRole,
        SetModifier, 
        UseUncheckedVent,
        UncheckedMurderPlayer,
        UncheckedCmdReportDeadBody,
        UncheckedExilePlayer,
        RandomizeCustomSkeldOnHS,

        // Role functionality

        MimicTransform = 75,
        PainterPaint,
        DemonSetBitten,
        PlaceNun,
        RemoveBody,
        DragPlaceBody,
        PlaceHat,
        LightsOut,
        ManipulatorKill,
        PlaceBomb,
        FixBomb,
        BombermanWin,
        ChameleonInvisible,
        GamblerShoot,
        SetSpelledPlayer,
        MedusaPetrify,
        PlaceSpiralTrap,
        ActivateSpiralTrap,
        ShowArcherNotification,
        PlumberMakeVent,
        SilencePlayer,
        ResetSilenced,

        RenegadeRecruitMinion,
        SetRandomTarget,
        BountyHunterKill,
        PlaceMine,
        PlaceTrap,
        MineKill,
        ActivateTrap,
        YinyangerSetYinyang,
        ChallengerSetRival,
        ChallengerPerformDuel,
        ChallengerSelectAttack,
        NinjaKill,
        BerserkerKill,
        YandereKill,
        StrandedFindBoxes,
        StrandedKill,
        StrandedInvisible,
        MonjaTakeItem,
        MonjaDeliverItem,
        MonjaRevertItemPosition,
        MonjaAwakened,
        MonjaKill,
        MonjaReset,

        RoleThiefSteal,
        PyromaniacWin,
        PlaceTreasure,
        CollectedTreasure,
        DevourBody,
        PoisonerWin,
        PuppeteerWin,
        PuppeteerTransform,
        PuppeteerResetTransform,
        ExilerTriggerWin,
        AmnesiacReportAndTakeRole,
        SeekerSetMinigamePlayers,
        SeekerResetMinigamePlayers,
        SeekerPerformMinigame,
        SeekerSelectAttack,

        CaptainSpecialVote,
        CaptainAutoCastSpecialVote, 
        MechanicFixLights,
        MechanicUsedRepair,
        SheriffKill,
        TimeTravelerShield,
        TimeTravelerRewindTime,
        TimeTravelerRevive,
        SquireSetShielded,
        ShieldedMurderAttempt,
        CheaterCheat,
        FortuneTellerReveal,
        HackerAbilityUses,
        SleuthUsedLocate,
        FinkHawkEye,
        SealVent,
        SpiritualistRevive,
        SendSpiritualistIsReviving,
        MurderSpiritualistIfReportWhileReviving,
        ResetSpiritualistReviveValues,
        CowardUsedCall,
        PlaceCamera,
        VigilantAbilityUses,
        PerformerIsReported,
        HunterUsedHunted,
        SetJinxed,
        BatFrequency,
        PlaceEngineerTrap,
        ActivateEngineerTrap,
        TaskMasterSetExtraTasks,
        TaskMasterTriggerCrewWin,
        TaskMasterActivateSpeed,
        JailerSetJailed,
        PrisonPlayer,

        ChangeMusic,
        WhoWasI,

        // Capture the flag
        CapturetheFlagKills,
        CaptureTheFlagWhoTookTheFlag,
        CaptureTheFlagWhichTeamScored,

        // Police and Thief
        PoliceandThiefKills,
        PoliceandThiefJail,
        PoliceandThiefFreeThief,
        PoliceandThiefTakeJewel,
        PoliceandThiefDeliverJewel,
        PoliceandThiefRevertedJewelPosition,
        PoliceandThiefsTased,

        // King of the hill
        KingoftheHillKills,
        KingoftheHillCapture,

        // Hot Potato
        HotPotatoTransfer,

        // ZombieLaboratory
        ZombieInfect,
        ZombieKills,
        ZombieAmmoRecover,
        ZombieNurseHeal,
        ZombieSurvivorsWin,
        ZombieTakeKeyItem,
        ZombieDeliverKeyItem,
        EnterLeaveInfirmary,
        NurseHasMedKit,
        ZombieLaboratoryTurnZombie,

        // Battle Royale
        BattleRoyaleKills
    }

    public static class RPCProcedure
    {

        // Main Controls

        public static void resetVariables() {
            Nun.clearNuns();
            Hats.clearHats();
            HypnotistSpiral.clearHypnotistSpirals();
            Mine.clearMines();
            Trap.clearTraps();
            Footprint.clearFootprints();
            EngineerTrap.clearTraps();
            PaintballTrail.resetTrail();
            BattleRoyaleFootprint.clearBattleRoyaleFootprints();
            BattleRoyaleShoot.clearBattleRoyaleShoots(); 
            clearAndReloadMapOptions();
            clearAndReloadRoles();
            clearGameHistory();
            setCustomButtonCooldowns();
            Helpers.toggleZoom(reset : true);
        }

        public static void ShareOptions(int numberOfOptions, MessageReader reader) {
            try {
                for (int i = 0; i < numberOfOptions; i++) {
                    uint optionId = reader.ReadPackedUInt32();
                    uint selection = reader.ReadPackedUInt32();
                    CustomOption option = CustomOption.options.FirstOrDefault(option => option.id == (int)optionId);
                    option.updateSelection((int)selection);
                }
            }
            catch (Exception e) {
                LasMonjasPlugin.Logger.LogError("Error while deserializing options: " + e.Message);
            }
        }

        public static void forceEnd() {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (!player.Data.Role.IsImpostor) {
                    //player.RemoveInfected();
                    player.MurderPlayer(player);
                    player.Data.IsDead = true;
                }
            }
        }

        public static void setRole(byte roleId, byte playerId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == playerId) {
                    switch ((RoleId)roleId) {
                        case RoleId.Mimic:
                            Mimic.mimic = player;
                            break;
                        case RoleId.Painter:
                            Painter.painter = player;
                            break;
                        case RoleId.Demon:
                            Demon.demon = player;
                            break;
                        case RoleId.Janitor:
                            Janitor.janitor = player;
                            break;
                        case RoleId.Illusionist:
                            Illusionist.illusionist = player;
                            break;
                        case RoleId.Manipulator:
                            Manipulator.manipulator = player;
                            break;
                        case RoleId.Bomberman:
                            Bomberman.bomberman = player;
                            break;
                        case RoleId.Chameleon:
                            Chameleon.chameleon = player;
                            break;
                        case RoleId.Gambler:
                            Gambler.gambler = player;
                            break;
                        case RoleId.Sorcerer:
                            Sorcerer.sorcerer = player;
                            break;
                        case RoleId.Medusa:
                            Medusa.medusa = player;
                            break;
                        case RoleId.Hypnotist:
                            Hypnotist.hypnotist = player;
                            break;
                        case RoleId.Archer:
                            Archer.archer = player;
                            break;
                        case RoleId.Plumber:
                            Plumber.plumber = player;
                            break;
                        case RoleId.Librarian:
                            Librarian.librarian = player;
                            break;
                        case RoleId.Renegade:
                            Renegade.renegade = player;
                            break;
                        case RoleId.Minion:
                            Minion.minion = player;
                            break;
                        case RoleId.BountyHunter:
                            BountyHunter.bountyhunter = player;
                            break;
                        case RoleId.Trapper:
                            Trapper.trapper = player;
                            break;
                        case RoleId.Yinyanger:
                            Yinyanger.yinyanger = player;
                            break;
                        case RoleId.Challenger:
                            Challenger.challenger = player;
                            break;
                        case RoleId.Ninja:
                            Ninja.ninja = player;
                            break;
                        case RoleId.Berserker:
                            Berserker.berserker = player;
                            break;
                        case RoleId.Yandere:
                            Yandere.yandere = player;
                            break;
                        case RoleId.Stranded:
                            Stranded.stranded = player;
                            break;
                        case RoleId.Monja:
                            Monja.monja = player;
                            break;
                        case RoleId.Joker:
                            Joker.joker = player;
                            break;
                        case RoleId.RoleThief:
                            RoleThief.rolethief = player;
                            break;
                        case RoleId.Pyromaniac:
                            Pyromaniac.pyromaniac = player;
                            break;
                        case RoleId.TreasureHunter:
                            TreasureHunter.treasureHunter = player;
                            break;
                        case RoleId.Devourer:
                            Devourer.devourer = player;
                            break;
                        case RoleId.Poisoner:
                            Poisoner.poisoner = player;
                            break;
                        case RoleId.Puppeteer:
                            Puppeteer.puppeteer = player;
                            break;
                        case RoleId.Exiler:
                            Exiler.exiler = player;
                            break;
                        case RoleId.Amnesiac:
                            Amnesiac.amnesiac = player;
                            break;
                        case RoleId.Seeker:
                            Seeker.seeker = player;
                            break;
                        case RoleId.Captain:
                            Captain.captain = player;
                            break;
                        case RoleId.Mechanic:
                            Mechanic.mechanic = player;
                            break;
                        case RoleId.Sheriff:
                            Sheriff.sheriff = player;
                            break;
                        case RoleId.Detective:
                            Detective.detective = player;
                            break;
                        case RoleId.Forensic:
                            Forensic.forensic = player;
                            break;
                        case RoleId.TimeTraveler:
                            TimeTraveler.timeTraveler = player;
                            break;
                        case RoleId.Squire:
                            Squire.squire = player;
                            break;
                        case RoleId.Cheater:
                            Cheater.cheater = player;
                            break;
                        case RoleId.FortuneTeller:
                            FortuneTeller.fortuneTeller = player;
                            break;
                        case RoleId.Hacker:
                            Hacker.hacker = player;
                            break;
                        case RoleId.Sleuth:
                            Sleuth.sleuth = player;
                            break;
                        case RoleId.Fink:
                            Fink.fink = player;
                            break;
                        case RoleId.Kid:
                            Kid.kid = player;
                            break;
                        case RoleId.Welder:
                            Welder.welder = player;
                            break;
                        case RoleId.Spiritualist:
                            Spiritualist.spiritualist = player;
                            break;
                        case RoleId.Coward:
                            Coward.coward = player;
                            break;
                        case RoleId.Vigilant:
                            Vigilant.vigilant = player;
                            break;
                        case RoleId.VigilantMira:
                            Vigilant.vigilantMira = player;
                            break;
                        case RoleId.Hunter:
                            Hunter.hunter = player;
                            break;
                        case RoleId.Jinx:
                            Jinx.jinx = player;
                            break;
                        case RoleId.Bat:
                            Bat.bat = player;
                            break;
                        case RoleId.Necromancer:
                            Necromancer.necromancer = player;
                            break;
                        case RoleId.Engineer:
                            Engineer.engineer = player;
                            break;
                        case RoleId.Shy:
                            Shy.shy = player;
                            break;
                        case RoleId.TaskMaster:
                            TaskMaster.taskMaster = player;
                            break;
                        case RoleId.Jailer:
                            Jailer.jailer = player;
                            break;

                        // Capture the Flag
                        case RoleId.RedPlayer01:
                            CaptureTheFlag.redplayer01 = player;
                            CaptureTheFlag.redteamFlag.Add(player);
                            break;
                        case RoleId.RedPlayer02:
                            CaptureTheFlag.redplayer02 = player;
                            CaptureTheFlag.redteamFlag.Add(player);
                            break;
                        case RoleId.RedPlayer03:
                            CaptureTheFlag.redplayer03 = player;
                            CaptureTheFlag.redteamFlag.Add(player);
                            break;
                        case RoleId.RedPlayer04:
                            CaptureTheFlag.redplayer04 = player;
                            CaptureTheFlag.redteamFlag.Add(player);
                            break;
                        case RoleId.RedPlayer05:
                            CaptureTheFlag.redplayer05 = player;
                            CaptureTheFlag.redteamFlag.Add(player);
                            break;
                        case RoleId.RedPlayer06:
                            CaptureTheFlag.redplayer06 = player;
                            CaptureTheFlag.redteamFlag.Add(player);
                            break;
                        case RoleId.RedPlayer07:
                            CaptureTheFlag.redplayer07 = player;
                            CaptureTheFlag.redteamFlag.Add(player);
                            break;
                        case RoleId.BluePlayer01:
                            CaptureTheFlag.blueplayer01 = player;
                            CaptureTheFlag.blueteamFlag.Add(player);
                            break;
                        case RoleId.BluePlayer02:
                            CaptureTheFlag.blueplayer02 = player;
                            CaptureTheFlag.blueteamFlag.Add(player);
                            break;
                        case RoleId.BluePlayer03:
                            CaptureTheFlag.blueplayer03 = player;
                            CaptureTheFlag.blueteamFlag.Add(player);
                            break;
                        case RoleId.BluePlayer04:
                            CaptureTheFlag.blueplayer04 = player;
                            CaptureTheFlag.blueteamFlag.Add(player);
                            break;
                        case RoleId.BluePlayer05:
                            CaptureTheFlag.blueplayer05 = player;
                            CaptureTheFlag.blueteamFlag.Add(player);
                            break;
                        case RoleId.BluePlayer06:
                            CaptureTheFlag.blueplayer06 = player;
                            CaptureTheFlag.blueteamFlag.Add(player);
                            break;
                        case RoleId.BluePlayer07:
                            CaptureTheFlag.blueplayer07 = player;
                            CaptureTheFlag.blueteamFlag.Add(player);
                            break;
                        case RoleId.StealerPlayer:
                            CaptureTheFlag.stealerPlayer = player;
                            break;

                        // Police and Thief
                        case RoleId.PolicePlayer01:
                            PoliceAndThief.policeplayer01 = player;
                            PoliceAndThief.policeTeam.Add(player);
                            break;
                        case RoleId.PolicePlayer02:
                            PoliceAndThief.policeplayer02 = player;
                            PoliceAndThief.policeTeam.Add(player);
                            break;
                        case RoleId.PolicePlayer03:
                            PoliceAndThief.policeplayer03 = player;
                            PoliceAndThief.policeTeam.Add(player);
                            break;
                        case RoleId.PolicePlayer04:
                            PoliceAndThief.policeplayer04 = player;
                            PoliceAndThief.policeTeam.Add(player);
                            break;
                        case RoleId.PolicePlayer05:
                            PoliceAndThief.policeplayer05 = player;
                            PoliceAndThief.policeTeam.Add(player);
                            break;
                        case RoleId.PolicePlayer06:
                            PoliceAndThief.policeplayer06 = player;
                            PoliceAndThief.policeTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer01:
                            PoliceAndThief.thiefplayer01 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer02:
                            PoliceAndThief.thiefplayer02 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer03:
                            PoliceAndThief.thiefplayer03 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer04:
                            PoliceAndThief.thiefplayer04 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer05:
                            PoliceAndThief.thiefplayer05 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer06:
                            PoliceAndThief.thiefplayer06 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer07:
                            PoliceAndThief.thiefplayer07 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer08:
                            PoliceAndThief.thiefplayer08 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;
                        case RoleId.ThiefPlayer09:
                            PoliceAndThief.thiefplayer09 = player;
                            PoliceAndThief.thiefTeam.Add(player);
                            break;

                        // King of the Hill
                        case RoleId.GreenKing:
                            KingOfTheHill.greenKingplayer = player;
                            KingOfTheHill.greenTeam.Add(player);
                            break;
                        case RoleId.GreenPlayer01:
                            KingOfTheHill.greenplayer01 = player;
                            KingOfTheHill.greenTeam.Add(player);
                            break;
                        case RoleId.GreenPlayer02:
                            KingOfTheHill.greenplayer02 = player;
                            KingOfTheHill.greenTeam.Add(player);
                            break;
                        case RoleId.GreenPlayer03:
                            KingOfTheHill.greenplayer03 = player;
                            KingOfTheHill.greenTeam.Add(player);
                            break;
                        case RoleId.GreenPlayer04:
                            KingOfTheHill.greenplayer04 = player;
                            KingOfTheHill.greenTeam.Add(player);
                            break;
                        case RoleId.GreenPlayer05:
                            KingOfTheHill.greenplayer05 = player;
                            KingOfTheHill.greenTeam.Add(player);
                            break;
                        case RoleId.GreenPlayer06:
                            KingOfTheHill.greenplayer06 = player;
                            KingOfTheHill.greenTeam.Add(player);
                            break;
                        case RoleId.YellowKing:
                            KingOfTheHill.yellowKingplayer = player;
                            KingOfTheHill.yellowTeam.Add(player);
                            break;
                        case RoleId.YellowPlayer01:
                            KingOfTheHill.yellowplayer01 = player;
                            KingOfTheHill.yellowTeam.Add(player);
                            break;
                        case RoleId.YellowPlayer02:
                            KingOfTheHill.yellowplayer02 = player;
                            KingOfTheHill.yellowTeam.Add(player);
                            break;
                        case RoleId.YellowPlayer03:
                            KingOfTheHill.yellowplayer03 = player;
                            KingOfTheHill.yellowTeam.Add(player);
                            break;
                        case RoleId.YellowPlayer04:
                            KingOfTheHill.yellowplayer04 = player;
                            KingOfTheHill.yellowTeam.Add(player);
                            break;
                        case RoleId.YellowPlayer05:
                            KingOfTheHill.yellowplayer05 = player;
                            KingOfTheHill.yellowTeam.Add(player);
                            break;
                        case RoleId.YellowPlayer06:
                            KingOfTheHill.yellowplayer06 = player;
                            KingOfTheHill.yellowTeam.Add(player);
                            break;
                        case RoleId.UsurperPlayer:
                            KingOfTheHill.usurperPlayer = player;
                            break;

                        // Hot Potato 
                        case RoleId.HotPotato:
                            HotPotato.hotPotatoPlayer = player;
                            break;
                        case RoleId.NotPotato01:
                            HotPotato.notPotato01 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato02:
                            HotPotato.notPotato02 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato03:
                            HotPotato.notPotato03 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato04:
                            HotPotato.notPotato04 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato05:
                            HotPotato.notPotato05 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato06:
                            HotPotato.notPotato06 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato07:
                            HotPotato.notPotato07 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato08:
                            HotPotato.notPotato08 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato09:
                            HotPotato.notPotato09 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato10:
                            HotPotato.notPotato10 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato11:
                            HotPotato.notPotato11 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato12:
                            HotPotato.notPotato12 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato13:
                            HotPotato.notPotato13 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.NotPotato14:
                            HotPotato.notPotato14 = player;
                            HotPotato.notPotatoTeam.Add(player);
                            break;
                        case RoleId.ExplodedPotato01:
                            HotPotato.explodedPotato01 = player;
                            break;
                        case RoleId.ExplodedPotato02:
                            HotPotato.explodedPotato02 = player;
                            break;
                        case RoleId.ExplodedPotato03:
                            HotPotato.explodedPotato03 = player;
                            break;
                        case RoleId.ExplodedPotato04:
                            HotPotato.explodedPotato04 = player;
                            break;
                        case RoleId.ExplodedPotato05:
                            HotPotato.explodedPotato05 = player;
                            break;
                        case RoleId.ExplodedPotato06:
                            HotPotato.explodedPotato06 = player;
                            break;
                        case RoleId.ExplodedPotato07:
                            HotPotato.explodedPotato07 = player;
                            break;
                        case RoleId.ExplodedPotato08:
                            HotPotato.explodedPotato08 = player;
                            break;
                        case RoleId.ExplodedPotato09:
                            HotPotato.explodedPotato09 = player;
                            break;
                        case RoleId.ExplodedPotato10:
                            HotPotato.explodedPotato10 = player;
                            break;
                        case RoleId.ExplodedPotato11:
                            HotPotato.explodedPotato11 = player;
                            break;
                        case RoleId.ExplodedPotato12:
                            HotPotato.explodedPotato12 = player;
                            break;
                        case RoleId.ExplodedPotato13:
                            HotPotato.explodedPotato13 = player;
                            break;
                        case RoleId.ExplodedPotato14:
                            HotPotato.explodedPotato14 = player;
                            break;

                        // ZombieLaboratory
                        case RoleId.NursePlayer:
                            ZombieLaboratory.nursePlayer = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer01:
                            ZombieLaboratory.zombiePlayer01 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer02:
                            ZombieLaboratory.zombiePlayer02 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer03:
                            ZombieLaboratory.zombiePlayer03 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer04:
                            ZombieLaboratory.zombiePlayer04 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer05:
                            ZombieLaboratory.zombiePlayer05 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer06:
                            ZombieLaboratory.zombiePlayer06 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer07:
                            ZombieLaboratory.zombiePlayer07 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer08:
                            ZombieLaboratory.zombiePlayer08 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer09:
                            ZombieLaboratory.zombiePlayer09 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer10:
                            ZombieLaboratory.zombiePlayer10 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer11:
                            ZombieLaboratory.zombiePlayer11 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer12:
                            ZombieLaboratory.zombiePlayer12 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer13:
                            ZombieLaboratory.zombiePlayer13 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.ZombiePlayer14:
                            ZombieLaboratory.zombiePlayer14 = player;
                            ZombieLaboratory.zombieTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer01:
                            ZombieLaboratory.survivorPlayer01 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer02:
                            ZombieLaboratory.survivorPlayer02 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer03:
                            ZombieLaboratory.survivorPlayer03 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer04:
                            ZombieLaboratory.survivorPlayer04 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer05:
                            ZombieLaboratory.survivorPlayer05 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer06:
                            ZombieLaboratory.survivorPlayer06 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer07:
                            ZombieLaboratory.survivorPlayer07 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer08:
                            ZombieLaboratory.survivorPlayer08 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer09:
                            ZombieLaboratory.survivorPlayer09 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer10:
                            ZombieLaboratory.survivorPlayer10 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer11:
                            ZombieLaboratory.survivorPlayer11 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer12:
                            ZombieLaboratory.survivorPlayer12 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;
                        case RoleId.SurvivorPlayer13:
                            ZombieLaboratory.survivorPlayer13 = player;
                            ZombieLaboratory.survivorTeam.Add(player);
                            break;

                        // Battle Royale
                        case RoleId.SoloPlayer01:
                            BattleRoyale.soloPlayer01 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer02:
                            BattleRoyale.soloPlayer02 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer03:
                            BattleRoyale.soloPlayer03 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer04:
                            BattleRoyale.soloPlayer04 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer05:
                            BattleRoyale.soloPlayer05 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer06:
                            BattleRoyale.soloPlayer06 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer07:
                            BattleRoyale.soloPlayer07 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer08:
                            BattleRoyale.soloPlayer08 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer09:
                            BattleRoyale.soloPlayer09 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer10:
                            BattleRoyale.soloPlayer10 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer11:
                            BattleRoyale.soloPlayer11 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer12:
                            BattleRoyale.soloPlayer12 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer13:
                            BattleRoyale.soloPlayer13 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer14:
                            BattleRoyale.soloPlayer14 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.SoloPlayer15:
                            BattleRoyale.soloPlayer15 = player;
                            BattleRoyale.soloPlayerTeam.Add(player);
                            break;
                        case RoleId.LimePlayer01:
                            BattleRoyale.limePlayer01 = player;
                            BattleRoyale.limeTeam.Add(player);
                            break;
                        case RoleId.LimePlayer02:
                            BattleRoyale.limePlayer02 = player;
                            BattleRoyale.limeTeam.Add(player);
                            break;
                        case RoleId.LimePlayer03:
                            BattleRoyale.limePlayer03 = player;
                            BattleRoyale.limeTeam.Add(player);
                            break;
                        case RoleId.LimePlayer04:
                            BattleRoyale.limePlayer04 = player;
                            BattleRoyale.limeTeam.Add(player);
                            break;
                        case RoleId.LimePlayer05:
                            BattleRoyale.limePlayer05 = player;
                            BattleRoyale.limeTeam.Add(player);
                            break;
                        case RoleId.LimePlayer06:
                            BattleRoyale.limePlayer06 = player;
                            BattleRoyale.limeTeam.Add(player);
                            break;
                        case RoleId.LimePlayer07:
                            BattleRoyale.limePlayer07 = player;
                            BattleRoyale.limeTeam.Add(player);
                            break;
                        case RoleId.PinkPlayer01:
                            BattleRoyale.pinkPlayer01 = player;
                            BattleRoyale.pinkTeam.Add(player);
                            break;
                        case RoleId.PinkPlayer02:
                            BattleRoyale.pinkPlayer02 = player;
                            BattleRoyale.pinkTeam.Add(player);
                            break;
                        case RoleId.PinkPlayer03:
                            BattleRoyale.pinkPlayer03 = player;
                            BattleRoyale.pinkTeam.Add(player);
                            break;
                        case RoleId.PinkPlayer04:
                            BattleRoyale.pinkPlayer04 = player;
                            BattleRoyale.pinkTeam.Add(player);
                            break;
                        case RoleId.PinkPlayer05:
                            BattleRoyale.pinkPlayer05 = player;
                            BattleRoyale.pinkTeam.Add(player);
                            break;
                        case RoleId.PinkPlayer06:
                            BattleRoyale.pinkPlayer06 = player;
                            BattleRoyale.pinkTeam.Add(player);
                            break;
                        case RoleId.PinkPlayer07:
                            BattleRoyale.pinkPlayer07 = player;
                            BattleRoyale.pinkTeam.Add(player);
                            break;
                        case RoleId.SerialKiller:
                            BattleRoyale.serialKiller = player;
                            BattleRoyale.serialKillerTeam.Add(player);
                            break;
                    }
                }
        }

        public static void setModifier(byte modifierId, byte playerId, byte flag) {
            PlayerControl player = Helpers.playerById(playerId);
            switch ((RoleId)modifierId) {
                case RoleId.Lover:
                    if (flag == 0) Modifiers.lover1 = player;
                    else Modifiers.lover2 = player;
                    break;
                case RoleId.Lighter:
                    Modifiers.lighter = player;
                    break;
                case RoleId.Blind:
                    Modifiers.blind = player;
                    break;
                case RoleId.Flash:
                    Modifiers.flash = player;
                    break;
                case RoleId.BigChungus:
                    Modifiers.bigchungus = player;
                    break;
                case RoleId.TheChosenOne:
                    Modifiers.theChosenOne = player;
                    break;
                case RoleId.Performer:
                    Modifiers.performer = player;
                    break;
                case RoleId.Pro:
                    Modifiers.pro = player;
                    break;
                case RoleId.Paintball:
                    Modifiers.paintball = player;
                    break;
                case RoleId.Electrician:
                    Modifiers.electrician = player;
                    break;
            }
        }

        public static void useUncheckedVent(int ventId, byte playerId, byte isEnter) {
            PlayerControl player = Helpers.playerById(playerId);
            if (player == null) return;
            // Fill dummy MessageReader and call MyPhysics.HandleRpc as the corountines cannot be accessed
            MessageReader reader = new MessageReader();
            byte[] bytes = BitConverter.GetBytes(ventId);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            reader.Buffer = bytes;
            reader.Length = bytes.Length;

            Hats.startAnimation(ventId);
            player.MyPhysics.HandleRpc(isEnter != 0 ? (byte)19 : (byte)20, reader);
        }

        public static void uncheckedMurderPlayer(byte sourceId, byte targetId, byte showAnimation) {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return; 
            PlayerControl source = Helpers.playerById(sourceId);
            PlayerControl target = Helpers.playerById(targetId);
            if (source != null && target != null) {
                if (showAnimation == 0) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
                source.MurderPlayer(target);
            }
        }

        public static void uncheckedCmdReportDeadBody(byte sourceId, byte targetId) {
            PlayerControl source = Helpers.playerById(sourceId);
            PlayerControl target = Helpers.playerById(targetId);
            if (source != null && target != null) source.ReportDeadBody(target.Data);
        }

        public static void uncheckedExilePlayer(byte targetId) {
            PlayerControl target = Helpers.playerById(targetId);
            if (target != null) target.Exiled();
        }

        public static void randomizeCustomSkeldOnHS(int randomNumber) {
            customSkeldHS = randomNumber;
        }

        // Role functionality

        public static void mimicTransform(byte playerId) {
            PlayerControl target = Helpers.playerById(playerId);
            if (Mimic.mimic == null || target == null) return;

            Mimic.transformTimer = Mimic.duration;
            Mimic.transformTarget = target;
            if (Painter.painterTimer <= 0f)
                Mimic.mimic.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void painterPaint(int colorId) {
            if (Painter.painter == null) return;

            SoundManager.Instance.PlaySound(CustomMain.customAssets.painterPaint, false, 100f);
            Painter.painterTimer = Painter.duration;
            Detective.footprintcolor = colorId;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                player.setLook("", colorId, "", "", "", "");           
        }

        public static void demonSetBitten(byte targetId, byte performReset) {
            if (performReset != 0) {
                Demon.bitten = null;
                return;
            }

            if (Demon.demon == null) return;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId && !player.Data.IsDead) {
                    Demon.bitten = player;
                }
            }
        }

        public static void placeNun(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                position.z = -0.5f;
            }
            new Nun(position);
        }

        public static void removeBody(byte playerId) {
            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++) {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == playerId) {
                    UnityEngine.Object.Destroy(array[i].gameObject);
                }
            }
            if (Modifiers.performer != null && playerId == Modifiers.performer.PlayerId)
                performerIsReported(0);
        }

        public static void dragPlaceBody(byte playerId, byte carrierId) {
            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++) {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == playerId) {
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (Janitor.janitor != null && player.PlayerId == carrierId && carrierId == Janitor.janitor.Data.PlayerId) {
                            if (!Janitor.dragginBody) {
                                Janitor.dragginBody = true;
                                Janitor.bodyId = playerId;
                                if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                                    GameObject vent = GameObject.Find("LowerCentralVent");
                                    vent.GetComponent<BoxCollider2D>().enabled = false;
                                }
                            }
                            else {
                                Janitor.dragginBody = false;
                                Janitor.bodyId = 0;
                                var currentPosition = Janitor.janitor.GetTruePosition();
                                var velocity = Janitor.janitor.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
                                var newPos = ((Vector2)Janitor.janitor.GetTruePosition()) - (velocity / 3) + new Vector2(0.15f, 0.25f) + array[i].myCollider.offset;
                                if (!PhysicsHelpers.AnythingBetween(
                                    currentPosition,
                                    newPos,
                                    Constants.ShipAndObjectsMask,
                                    false
                                )) {
                                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                                        array[i].transform.position = newPos;
                                        array[i].transform.position += new Vector3(0, 0, -0.5f);
                                        GameObject vent = GameObject.Find("LowerCentralVent");
                                        vent.GetComponent<BoxCollider2D>().enabled = true;
                                    }
                                    else {
                                        array[i].transform.position = newPos;
                                    }
                                }
                            }
                        }
                        else if (Necromancer.necromancer != null && player.PlayerId == carrierId && carrierId == Necromancer.necromancer.Data.PlayerId) {
                            if (!Necromancer.dragginBody) {
                                Necromancer.dragginBody = true;
                                Necromancer.bodyId = playerId;
                            }
                            else {
                                Necromancer.dragginBody = false;
                                Necromancer.bodyId = 0;
                                var currentPosition = Necromancer.necromancer.GetTruePosition();
                                var velocity = Necromancer.necromancer.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
                                var newPos = ((Vector2)Necromancer.necromancer.GetTruePosition()) - (velocity / 3) + new Vector2(0.15f, 0.25f) + array[i].myCollider.offset;
                                if (!PhysicsHelpers.AnythingBetween(
                                    currentPosition,
                                    newPos,
                                    Constants.ShipAndObjectsMask,
                                    false
                                )) array[i].transform.position = newPos;
                            }
                        }
                    }
                }
            }
        }

        public static void janitorResetValues() {
            // Restore janitor values when rewind time
            if (Janitor.janitor != null && Janitor.dragginBody) {
                Janitor.dragginBody = false;
                Janitor.bodyId = 0;
            }
        }

        public static void placeHat(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new Hats(position);
        }

        public static void lightsOut() {
            if (MapBehaviour.Instance) {
                MapBehaviour.Instance.Close();
            }
            Illusionist.lightsOutTimer = Illusionist.lightsOutDuration;
            // If the local player is impostor indicate lights out
            SoundManager.Instance.PlaySound(CustomMain.customAssets.illusionistLightsOut, false, 100f);
            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && PlayerControl.LocalPlayer != Illusionist.illusionist) {
                new CustomMessage(Language.statusRolesTexts[5], Illusionist.lightsOutDuration, -1, -1.3f, 9);
            }
        }


        public static void manipulatorKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Manipulator.manipulator.MurderPlayer(player);
                    return;
                }
            }
        }

        public static void placeBomb(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            if (MapBehaviour.Instance) {
                MapBehaviour.Instance.Close();
            }
            Bomberman.activeBomb = true;
            Bomberman.currentBombNumber += 1;
            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    Bomberman.bombDuration = 60;
                    break;
                case 1:
                    Bomberman.bombDuration = 60;
                    break;
                case 2:
                    Bomberman.bombDuration = 90;
                    break;
                case 3:
                    Bomberman.bombDuration = 60;
                    break;
                case 4:
                    Bomberman.bombDuration = 180;
                    break;
                case 5:
                    Bomberman.bombDuration = 90;
                    break;
            }
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.Data.IsDead && !player.Data.Role.IsImpostor) {
                    Bomberman.bombDuration += 5;
                }
            }
            Bomberman.bombTimer = Bomberman.bombDuration;
            new Bomb(Bomberman.bombDuration, position, Bomberman.currentBombNumber);

            // Music Stop and play bomb music
            changeMusic(7);
            SoundManager.Instance.PlaySound(CustomMain.customAssets.bombermanBombMusic, true, 75f);
            SoundManager.Instance.StopSound(CustomMain.customAssets.performerMusic);
            // Indicate bomb text
            new CustomMessage(Language.statusRolesTexts[6], Bomberman.bombTimer, Bomberman.currentBombNumber, -1.3f, 1);
        }

        public static void fixBomb() {
            GameObject bomb = GameObject.Find("Bomb");
            if (bomb != null) {
                bomb.name = "DefusedBomb";
                Bomberman.activeBomb = false;
                bomb.SetActive(false);
                resetBomberBombButton();

                //Music after fix bomb
                if (!Monja.awakened) {
                    changeMusic(2);
                }
            }
        }
        public static void bombermanWin() {
            SoundManager.Instance.PlaySound(CustomMain.customAssets.bombermanBombClip, false, 100f);
            Bomberman.triggerBombExploded = true;
        }

        public static void chameleonInvisible() {
            if (Chameleon.chameleon == null) return;

            Chameleon.chameleonTimer = Chameleon.duration;
        }

        public static void gamblerShoot(byte playerId) {
            PlayerControl target = Helpers.playerById(playerId);
            if (target == null) return;
            target.Exiled();
            PlayerControl partner = target.getPartner(); // Lover check
            byte partnerId = partner != null ? partner.PlayerId : playerId;
            Gambler.numberOfShots = Mathf.Max(0, Gambler.numberOfShots - 1);
            if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(target.KillSfx, false, 0.8f);
            if (MeetingHud.Instance) {
                foreach (PlayerVoteArea pva in MeetingHud.Instance.playerStates) {
                    if (pva.TargetPlayerId == playerId || pva.TargetPlayerId == partnerId) {
                        pva.SetDead(pva.DidReport, true);
                        pva.Overlay.gameObject.SetActive(true);
                    }

                    //Give players back their vote if target is shot dead
                    if (pva.VotedFor != playerId || pva.VotedFor != partnerId) continue;
                    pva.UnsetVote();
                    var voteAreaPlayer = Helpers.playerById(pva.TargetPlayerId);
                    if (!voteAreaPlayer.AmOwner) continue;
                    MeetingHud.Instance.ClearVote();
                }
                if (AmongUsClient.Instance.AmHost)
                    MeetingHud.Instance.CheckForEndVoting();
            }
            if (HudManager.Instance != null && Gambler.gambler != null)
                if (PlayerControl.LocalPlayer == target)
                    HudManager.Instance.KillOverlay.ShowKillAnimation(Gambler.gambler.Data, target.Data);
                else if (partner != null && PlayerControl.LocalPlayer == partner)
                    HudManager.Instance.KillOverlay.ShowKillAnimation(partner.Data, partner.Data);
        }

        public static void setSpelledPlayer(byte playerId) {
            PlayerControl player = Helpers.playerById(playerId);
            if (Sorcerer.spelledPlayers == null)
                Sorcerer.spelledPlayers = new List<PlayerControl>();
            if (player != null) {
                Sorcerer.spelledPlayers.Add(player);
            }
        }

        public static void medusaPetrify(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Medusa.messageTimer = Medusa.duration;
                    if (PlayerControl.LocalPlayer == player) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.medusaPetrify, false, 100f);
                        if (MapBehaviour.Instance) {
                            MapBehaviour.Instance.Close();
                        }
                        new CustomMessage(Language.statusRolesTexts[7], Medusa.duration, -1, 1.6f, 14);
                    }
                    player.moveable = false;
                    player.NetTransform.Halt(); // Stop current movement
                    HudManager.Instance.StartCoroutine(Effects.Lerp(Medusa.duration, new Action<float>((p) => { // Delayed action
                        if (p == 1f) {
                            player.moveable = true;
                        }
                    })));
                    return;
                }
            }
        }        

        public static void placeSpiralTrap(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            Hypnotist.currentSpiralNumber += 1;
            Hypnotist.trapsCounterButtonText.text = $"{Hypnotist.currentSpiralNumber} / {Hypnotist.numberOfSpirals}";
            new HypnotistSpiral(Hypnotist.spiralDuration, position);
        }

        public static void activateSpiralTrap(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Hypnotist.hypnotizedPlayers.Add(player);

                    if (player == PlayerControl.LocalPlayer) {
                        GameObject camera = GameObject.Find("Main Camera");
                        camera.transform.rotation = Quaternion.Euler(0, 0, 180);

                        HudManager.Instance.StartCoroutine(Effects.Lerp(Hypnotist.spiralDuration, new Action<float>((p) => {
                            if (p == 1f) {
                                camera.transform.rotation = Quaternion.Euler(0, 0, 0);
                                foreach (HypnotistSpiral spiral in HypnotistSpiral.hypnotistSpirals) {
                                    spiral.isActive = true;
                                }
                            }
                        })));
                    }
                }
            }
        }

        public static void showArcherNotification(byte murderId) {

            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player == PlayerControl.LocalPlayer && Vector2.Distance(player.transform.position, Helpers.playerById(murderId).transform.position) < Archer.noticeRange) {
                    Arrow arrow = new Arrow(Color.white);
                    arrow.image.sprite = Archer.getArcherWarningSprite();
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.archerBowClip, false, 100f);

                    Vector3 pos = Helpers.playerById(murderId).transform.position;

                    HudManager.Instance.StartCoroutine(Effects.Lerp(10f, new Action<float>((p) => {
                        arrow.Update(pos);
                        arrow.arrow.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                        if (p > 0.8f) {
                            arrow.image.color = new Color(1f, 1f, 1f, (1f - p) * 5f);
                        }
                        if (p == 1f) {
                            UnityEngine.Object.Destroy(arrow.arrow);
                        }
                    })));
                }
            }
        }
        
        public static void plumberMakeVent(byte[] buff) {

            Plumber.currentVents += 1;
            Plumber.plumberVentButtonText.text = $"{Plumber.currentVents} / {Plumber.maxVents}";

            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));

            var ventPrefab = UnityEngine.Object.FindObjectOfType<Vent>();
            var vent = UnityEngine.Object.Instantiate(ventPrefab, ventPrefab.transform.parent);
            vent.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                vent.gameObject.layer = 12;
                vent.gameObject.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                vent.transform.position = new Vector3(position.x, position.y - 0.25f, -0.5f);
                vent.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
            else {
                vent.transform.position = new Vector3(position.x, position.y - 0.25f, 1f);
            }
            var ventRenderer = vent.GetComponent<SpriteRenderer>();
            vent.myRend = ventRenderer; 
            if (PlayerControl.LocalPlayer == Plumber.plumber) {
                ventRenderer.color = new Color(1, 1, 1, 0.5f);
            }
            else {
                ventRenderer.color = new Color(1, 1, 1, 0f);
            }

            Plumber.Vents.Add(vent);
        }

        public static void silencePlayer(byte playerId) {
            PlayerControl target = Helpers.playerById(playerId);
            Librarian.targetLibrary = target;
            Librarian.targetNameButtonText.text = Librarian.targetLibrary.name;
        }
        public static void resetSilenced() {
            Librarian.targetLibrary = null;
            Librarian.targetNameButtonText.text = "";
        }

        public static void renegadeRecruitMinion(byte targetId) {
            PlayerControl player = Helpers.playerById(targetId);
            if (player == null) return;

            Renegade.usedRecruit = true;

            if (Chameleon.chameleon != null && player == Chameleon.chameleon) {
                Chameleon.resetChameleon();
            }

            if (Archer.archer != null && player == Archer.archer) {
                if (Archer.Guides.Count != 0) {
                    foreach (var guide in Archer.Guides) {
                        guide.Value.color = Color.clear;
                    }
                }
                Archer.weaponEquiped = false;
                if (Archer.bow != null) {
                    Archer.bow.gameObject.SetActive(Archer.weaponEquiped);
                }
            }

            DestroyableSingleton<RoleManager>.Instance.SetRole(player, RoleTypes.Crewmate);
            erasePlayerRoles(player.PlayerId, true);
            Minion.minion = player;
            if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId) PlayerControl.LocalPlayer.moveable = true;

            // Sound for both renegade and minion
            if (PlayerControl.LocalPlayer == Minion.minion || PlayerControl.LocalPlayer == Renegade.renegade) {
                SoundManager.Instance.PlaySound(CustomMain.customAssets.renegadeRecruitMinionClip, false, 100f);
            }

            // Green screen notification for the minion and renegade
            if (PlayerControl.LocalPlayer == Minion.minion || PlayerControl.LocalPlayer == Renegade.renegade) {
                HudManager.Instance.FullScreen.enabled = true;
                HudManager.Instance.FullScreen.gameObject.SetActive(true);
                HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) => {
                    var renderer = HudManager.Instance.FullScreen;
                    Color c = new Color(0f / 255f, 255f / 255f, 157f / 255f, 0f);
                    if (p < 0.5) {
                        if (renderer != null)
                            renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(p * 2 * 0.75f));
                    }
                    else {
                        if (renderer != null)
                            renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                    }
                })));
            }
            Renegade.canRecruitMinion = false;
            return;
        }

        public static void erasePlayerRoles(byte playerId, bool ignoreLovers = false) {
            PlayerControl player = Helpers.playerById(playerId);
            if (player == null) return;

            // Crewmate roles
            if (player == Captain.captain) Captain.clearAndReload();
            if (player == Mechanic.mechanic) Mechanic.clearAndReload();
            if (player == Sheriff.sheriff) Sheriff.clearAndReload();
            if (player == Detective.detective) Detective.clearAndReload();
            if (player == Forensic.forensic) Forensic.clearAndReload();
            if (player == TimeTraveler.timeTraveler) TimeTraveler.clearAndReload();
            if (player == Squire.squire) Squire.clearAndReload();
            if (player == Cheater.cheater) Cheater.clearAndReload();
            if (player == FortuneTeller.fortuneTeller) FortuneTeller.clearAndReload();
            if (player == Hacker.hacker) Hacker.clearAndReload();
            if (player == Sleuth.sleuth) Sleuth.clearAndReload();
            if (player == Fink.fink) Fink.clearAndReload();
            if (player == Kid.kid) Kid.clearAndReload();
            if (player == Welder.welder) Welder.clearAndReload();
            if (player == Spiritualist.spiritualist) Spiritualist.clearAndReload();
            if (player == Coward.coward) Coward.clearAndReload();
            if (player == Vigilant.vigilant) Vigilant.clearAndReload();
            if (player == Vigilant.vigilantMira) Vigilant.clearAndReload();
            if (player == Hunter.hunter) Hunter.clearAndReload();
            if (player == Jinx.jinx) Jinx.clearAndReload();
            if (player == Bat.bat) Bat.clearAndReload();
            if (player == Necromancer.necromancer) Necromancer.clearAndReload();
            if (player == Engineer.engineer) Engineer.clearAndReload();
            if (player == Shy.shy) Shy.clearAndReload();
            if (player == TaskMaster.taskMaster) TaskMaster.clearAndReload();
            if (player == Jailer.jailer) Jailer.clearAndReload();

            // Impostor roles
            if (player == Mimic.mimic) Mimic.clearAndReload();
            if (player == Painter.painter) Painter.clearAndReload();
            if (player == Demon.demon) Demon.clearAndReload();
            if (player == Janitor.janitor) Janitor.clearAndReload();
            if (player == Illusionist.illusionist) Illusionist.clearAndReload();
            if (player == Manipulator.manipulator) Manipulator.clearAndReload();
            if (player == Bomberman.bomberman) Bomberman.clearAndReload();
            if (player == Chameleon.chameleon) Chameleon.clearAndReload();
            if (player == Gambler.gambler) Gambler.clearAndReload();
            if (player == Sorcerer.sorcerer) Sorcerer.clearAndReload();
            if (player == Medusa.medusa) Medusa.clearAndReload();
            if (player == Hypnotist.hypnotist) Hypnotist.clearAndReload();
            if (player == Archer.archer) Archer.clearAndReload();
            if (player == Plumber.plumber) Plumber.clearAndReload();
            if (player == Librarian.librarian) Librarian.clearAndReload();

            // Neutral roles
            if (player == Joker.joker) Joker.clearAndReload();
            if (player == RoleThief.rolethief) RoleThief.clearAndReload();
            if (player == Pyromaniac.pyromaniac) Pyromaniac.clearAndReload();
            if (player == TreasureHunter.treasureHunter) TreasureHunter.clearAndReload();
            if (player == Devourer.devourer) Devourer.clearAndReload();
            if (player == Poisoner.poisoner) Poisoner.clearAndReload();
            if (player == Puppeteer.puppeteer) Puppeteer.clearAndReload();
            if (player == Exiler.exiler) Exiler.clearAndReload();
            if (player == Amnesiac.amnesiac) Amnesiac.clearAndReload();
            if (player == Seeker.seeker) Seeker.clearAndReload();

            if (!ignoreLovers && (player == Modifiers.lover1 || player == Modifiers.lover2)) { // The whole Lover couple is being erased
                Modifiers.ClearLovers();
            }
            if (player == Renegade.renegade) {
                Renegade.clearAndReload();
            }
            if (player == Minion.minion) Minion.clearAndReload();
        }

        public static void setRandomTarget(byte playerid, byte whichPlayer) {
            
            switch (whichPlayer) {
                // Bounty Hunter
                case 0:
                    if (BountyHunter.bountyhunter == null) return; 
                    {
                        foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                            if (player.PlayerId == playerid) {
                                BountyHunter.hasToKill = player;
                            }
                        }

                        if (BountyHunter.hasToKill.Data.IsDead) {
                            BountyHunter.bountyhunter.MurderPlayer(BountyHunter.bountyhunter);
                        }

                        if (BountyHunter.hasToKill == Joker.joker) {
                            BountyHunter.rolName = "<color=#808080FF>Joker</color>";
                        }
                        else if (BountyHunter.hasToKill == RoleThief.rolethief) {
                            BountyHunter.rolName = "<color=#808080FF>Role Thief</color>";
                        }
                        else if (BountyHunter.hasToKill == Pyromaniac.pyromaniac) {
                            BountyHunter.rolName = "<color=#808080FF>Pyromaniac</color>";
                        }
                        else if (BountyHunter.hasToKill == TreasureHunter.treasureHunter) {
                            BountyHunter.rolName = "<color=#808080FF>Treasure Hunter</color>";
                        }
                        else if (BountyHunter.hasToKill == Devourer.devourer) {
                            BountyHunter.rolName = "<color=#808080FF>Devourer</color>";
                        }
                        else if (BountyHunter.hasToKill == Poisoner.poisoner) {
                            BountyHunter.rolName = "<color=#808080FF>Poisoner</color>";
                        }
                        else if (BountyHunter.hasToKill == Puppeteer.puppeteer) {
                            BountyHunter.rolName = "<color=#808080FF>Puppeteer</color>";
                        }
                        else if (BountyHunter.hasToKill == Exiler.exiler) {
                            BountyHunter.rolName = "<color=#808080FF>Exiler</color>";
                        }
                        else if (BountyHunter.hasToKill == Seeker.seeker) {
                            BountyHunter.rolName = "<color=#808080FF>Seeker</color>";
                        }
                        else if (BountyHunter.hasToKill == Captain.captain) {
                            BountyHunter.rolName = "<color=#5E3E7DFF>Captain</color>";
                        }
                        else if (BountyHunter.hasToKill == Mechanic.mechanic) {
                            BountyHunter.rolName = "<color=#7F4C00FF>Mechanic</color>"; ;
                        }
                        else if (BountyHunter.hasToKill == Sheriff.sheriff) {
                            BountyHunter.rolName = "<color=#FFFF00FF>Sheriff</color>";
                        }
                        else if (BountyHunter.hasToKill == Detective.detective) {
                            BountyHunter.rolName = "<color=#C85ABEFF>Detective</color>";
                        }
                        else if (BountyHunter.hasToKill == Forensic.forensic) {
                            BountyHunter.rolName = "<color=#4E61FFFF>Forensic</color>";
                        }
                        else if (BountyHunter.hasToKill == TimeTraveler.timeTraveler) {
                            BountyHunter.rolName = "<color=#00BDFFFF>Time Traveler</color>";
                        }
                        else if (BountyHunter.hasToKill == Squire.squire) {
                            BountyHunter.rolName = "<color=#00FF00FF>Squire</color>";
                        }
                        else if (BountyHunter.hasToKill == Cheater.cheater) {
                            BountyHunter.rolName = "<color=#666699FF>Cheater</color>";
                        }
                        else if (BountyHunter.hasToKill == FortuneTeller.fortuneTeller) {
                            BountyHunter.rolName = "<color=#00C642FF>Fortune Teller</color>";
                        }
                        else if (BountyHunter.hasToKill == Hacker.hacker) {
                            BountyHunter.rolName = "<color=#72FFACFF>Hacker</color>";
                        }
                        else if (BountyHunter.hasToKill == Sleuth.sleuth) {
                            BountyHunter.rolName = "<color=#009F57FF>Sleuth</color>";
                        }
                        else if (BountyHunter.hasToKill == Fink.fink) {
                            BountyHunter.rolName = "<color=#FF73F6FF>Fink</color>";
                        }
                        else if (BountyHunter.hasToKill == Welder.welder) {
                            BountyHunter.rolName = "<color=#6D5B2FFF>Welder</color>";
                        }
                        else if (BountyHunter.hasToKill == Spiritualist.spiritualist) {
                            BountyHunter.rolName = "<color=#FFC5E1FF>Spiritualist</color>";
                        }
                        else if (BountyHunter.hasToKill == Coward.coward) {
                            BountyHunter.rolName = "<color=#00F7E1FF>Coward</color>";
                        }
                        else if (BountyHunter.hasToKill == Vigilant.vigilant) {
                            BountyHunter.rolName = "<color=#E3E15AFF>Vigilant</color>";
                        }
                        else if (BountyHunter.hasToKill == Vigilant.vigilantMira) {
                            BountyHunter.rolName = "<color=#E3E15AFF>Vigilant</color>";
                        }
                        else if (BountyHunter.hasToKill == Hunter.hunter) {
                            BountyHunter.rolName = "<color=#E1EB90FF>Hunter</color>";
                        }
                        else if (BountyHunter.hasToKill == Jinx.jinx) {
                            BountyHunter.rolName = "<color=#928B55FF>Jinx</color>";
                        }
                        else if (BountyHunter.hasToKill == Bat.bat) {
                            BountyHunter.rolName = "<color=#A600FFFF>Bat</color>";
                        }
                        else if (BountyHunter.hasToKill == Necromancer.necromancer) {
                            BountyHunter.rolName = "<color=#FF73F6FF>Necromancer</color>";
                        }
                        else if (BountyHunter.hasToKill == Engineer.engineer) {
                            BountyHunter.rolName = "<color=#7F4C00FF>Engineer</color>";
                        }
                        else if (BountyHunter.hasToKill == Shy.shy) {
                            BountyHunter.rolName = "<color=#F2BEFFFF>Shy</color>";
                        }
                        else if (BountyHunter.hasToKill == TaskMaster.taskMaster) {
                            BountyHunter.rolName = "<color=#999999FF>Task Master</color>";
                        }
                        else if (BountyHunter.hasToKill == Jailer.jailer) {
                            BountyHunter.rolName = "<color=#CCFF99FF>Jailer</color>";
                        }
                        else if (BountyHunter.hasToKill == Mimic.mimic) {
                            BountyHunter.rolName = "<color=#FF0000FF>Mimic</color>";
                        }
                        else if (BountyHunter.hasToKill == Painter.painter) {
                            BountyHunter.rolName = "<color=#FF0000FF>Painter</color>";
                        }
                        else if (BountyHunter.hasToKill == Demon.demon) {
                            BountyHunter.rolName = "<color=#FF0000FF>Demon</color>";
                        }
                        else if (BountyHunter.hasToKill == Janitor.janitor) {
                            BountyHunter.rolName = "<color=#FF0000FF>Janitor</color>";
                        }
                        else if (BountyHunter.hasToKill == Illusionist.illusionist) {
                            BountyHunter.rolName = "<color=#FF0000FF>Illusionist</color>";
                        }
                        else if (BountyHunter.hasToKill == Manipulator.manipulator) {
                            BountyHunter.rolName = "<color=#FF0000FF>Manipulator</color>";
                        }
                        else if (BountyHunter.hasToKill == Bomberman.bomberman) {
                            BountyHunter.rolName = "<color=#FF0000FF>Bomberman</color>";
                        }
                        else if (BountyHunter.hasToKill == Chameleon.chameleon) {
                            BountyHunter.rolName = "<color=#FF0000FF>Chameleon</color>";
                        }
                        else if (BountyHunter.hasToKill == Gambler.gambler) {
                            BountyHunter.rolName = "<color=#FF0000FF>Gambler</color>";
                        }
                        else if (BountyHunter.hasToKill == Sorcerer.sorcerer) {
                            BountyHunter.rolName = "<color=#FF0000FF>Sorcerer</color>";
                        }
                        else if (BountyHunter.hasToKill == Medusa.medusa) {
                            BountyHunter.rolName = "<color=#FF0000FF>Medusa</color>";
                        }
                        else if (BountyHunter.hasToKill == Archer.archer) {
                            BountyHunter.rolName = "<color=#FF0000FF>Archer</color>";
                        }
                        else if (BountyHunter.hasToKill == Hypnotist.hypnotist) {
                            BountyHunter.rolName = "<color=#FF0000FF>Hypnotist</color>";
                        }
                        else if (BountyHunter.hasToKill == Plumber.plumber) {
                            BountyHunter.rolName = "<color=#FF0000FF>Plumber</color>";
                        }
                        else if (BountyHunter.hasToKill == Librarian.librarian) {
                            BountyHunter.rolName = "<color=#FF0000FF>Librarian</color>";
                        }
                        else if (BountyHunter.hasToKill.Data.Role.IsImpostor) {
                            BountyHunter.rolName = "<color=#FF0000FF>Impostor</color>";
                        }
                        else {
                            BountyHunter.rolName = "<color=#8DFFFFFF>Crewmate</color>";
                        }
                    }

                    BountyHunter.targetNameButtonText.text = BountyHunter.rolName;
                    BountyHunter.usedTarget = true;
                    break;
                // Exiler
                case 1:
                    if (Exiler.exiler == null) return; 
                    {
                        foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                            if (player.PlayerId == playerid) {
                                Exiler.target = player;
                            }
                        }

                        if (Exiler.target.Data.IsDead) {
                            Exiler.exiler.MurderPlayer(Exiler.exiler);
                        }
                        Exiler.targetNameButtonText.text = Exiler.target.name;
                        Exiler.usedTarget = true;
                    }
                    break;
                // Yandere:
                case 2:
                    if (Yandere.yandere == null) return; 
                    {
                        foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                            if (player.PlayerId == playerid) {
                                Yandere.target = player;
                            }
                        }

                        if (Yandere.target.Data.IsDead) {
                            Yandere.rampageMode = true;
                        }
                        Yandere.yandereTargetButtonText.text = Yandere.target.name + " " + Yandere.currenStareTimes + " / " + Yandere.stareTimes;
                        Yandere.usedTarget = true;
                    }
                    break;
            }          
        }
        
        public static void bountyHunterKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId && BountyHunter.hasToKill.PlayerId == targetId) {
                    BountyHunter.triggerBountyHunterWin = true;
                    BountyHunter.bountyhunter.MurderPlayer(player);
                    return;
                }
                else if (player.PlayerId == targetId) {
                    BountyHunter.bountyhunter.MurderPlayer(player);
                    return;
                }
            }
        }

        public static void placeMine(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            Trapper.currentMineNumber += 1;
            new Mine(Trapper.durationOfMines, position);
        }
        public static void placeTrap(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            Trapper.currentTrapNumber += 1;
            new Trap(Trapper.durationOfTraps, position);
        }

        public static void mineKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {

                HudManager.Instance.StartCoroutine(Effects.Lerp(1, new Action<float>((p) => {
                    if (p == 1f) {
                        foreach (Mine mine in Mine.mines) {
                            if (Vector2.Distance(player.transform.position, mine.mine.transform.position) < 1f && player != Trapper.trapper) {
                                mine.mine.transform.position = new Vector3(-1000, 500, 0);
                            }
                        }
                    }
                })));

                if (player.PlayerId == targetId) {
                    player.moveable = false;
                    player.NetTransform.Halt();
                    Trapper.mined = player;
                    HudManager.Instance.StartCoroutine(Effects.Lerp(1, new Action<float>((p) => {
                        if (player == PlayerControl.LocalPlayer) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.trapperStepMineClip, false, 100f);
                        }
                        if (p == 1f) {
                            player.moveable = true;
                            if (Jailer.jailedPlayer == null || (Jailer.jailedPlayer != null && player != Jailer.jailedPlayer)) {
                                uncheckedMurderPlayer(Trapper.trapper.PlayerId, player.PlayerId, 0);
                            }
                            Trapper.mined = null;
                        }
                    })));
                }
            }
        }

        public static void activateTrap(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {

                if (player.PlayerId == targetId) {
                    foreach (Trap trap in Trap.traps) {
                        if (Vector2.Distance(player.transform.position, trap.trap.transform.position) < 1f && player != Trapper.trapper) {
                            player.transform.position = trap.trap.transform.position;
                        }
                    }
                    if (player == PlayerControl.LocalPlayer) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.trapperStepTrapClip, false, 100f);
                    }
                    player.moveable = false;
                    player.NetTransform.Halt();
                    HudManager.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>((p) => {
                        if (p == 1f) {
                            player.moveable = true;
                            foreach (Trap trap in Trap.traps) {
                                if (Vector2.Distance(player.transform.position, trap.trap.transform.position) < 1f && player != Trapper.trapper) {
                                    trap.trap.transform.position = new Vector3(-1000, 500, 0);
                                }
                            }
                        }
                    })));
                }
            }
        }
        public static void yinyangerSetYinyang(byte targetId, byte yinflag) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId && !player.Data.IsDead) {
                    if (yinflag == 0) {
                        Yinyanger.yinyedplayer = player;
                        Yinyanger.usedYined = true;
                        Yinyanger.yinedButtonText.text = Yinyanger.yinyedplayer.name;
                    }
                    else {
                        Yinyanger.yangyedplayer = player;
                        Yinyanger.usedYanged = true;
                        Yinyanger.yanedButtonText.text = Yinyanger.yangyedplayer.name;
                    }
                }
            }
        }

        public static void challengerSetRival(byte targetId, byte resetRival) {
            if (resetRival != 0) {
                Challenger.rivalPlayer = null;
                return;
            }

            if (Challenger.challenger == null) return;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId && !player.Data.IsDead) {
                    Challenger.rivalPlayer = player;
                }
            }
        }

        public static void challengerPerformDuel() {

            // Remove body dragging for janitor
            if (Janitor.janitor != null && Janitor.dragginBody) {
                Janitor.janitorResetValuesAtDead();
            }
            
            // Remove body dragging for necromancer
            if (Necromancer.necromancer != null && Necromancer.dragginBody) {
                Necromancer.necromancerResetValuesAtDead();
            }

            // Reset chameleon invisibility
            if (Chameleon.chameleon != null && Chameleon.chameleonTimer > 0) {
                Chameleon.resetChameleon();
            }

            // Reset puppeteer morph
            if (Puppeteer.puppeteer != null && Puppeteer.morphed) {
                Puppeteer.Reset();
            }

            // Force exit from vents to all players
            if (PlayerControl.LocalPlayer.inVent) {
                foreach (Vent vent in ShipStatus.Instance.AllVents) {
                    bool canUse;
                    bool couldUse;
                    vent.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
                    if (canUse) {
                        PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(vent.Id);
                        vent.SetButtons(false);
                    }
                }
            }

            // Force map close to all players
            if (MapBehaviour.Instance) {
                MapBehaviour.Instance.Close();
            }
            // Force task close to all players
            if (Minigame.Instance)
                Minigame.Instance.ForceClose();

            new CustomMessage(Language.introTexts[1], Challenger.duelDuration, -1, -1.3f, 2);

            // music stop and play duel music
            changeMusic(8);
            SoundManager.Instance.PlaySound(CustomMain.customAssets.challengerDuelMusic, false, 5f);
            SoundManager.Instance.StopSound(CustomMain.customAssets.performerMusic);

            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player == PlayerControl.LocalPlayer) {
                    positionBeforeDuel = player.transform.position;
                }
            }

            Challenger.duelDuration = 30f;
            Challenger.isDueling = true;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player == Challenger.challenger) {
                    player.transform.position = new Vector3(45.26f, 0f, player.transform.position.z);
                }
                else if (player == Challenger.rivalPlayer) {
                    player.transform.position = new Vector3(48f, 0f, player.transform.position.z);
                }
                else {
                    player.transform.position = new Vector3(46.7f, 1f, player.transform.position.z);
                }
            }

            resetDuelButtons();
        }

        public static void challengerSelectAttack(byte challengerAttack) {
            switch (challengerAttack) {
                case 1:
                    Challenger.challengerRock = true;
                    break;
                case 2:
                    Challenger.challengerPaper = true;
                    break;
                case 3:
                    Challenger.challengerScissors = true;
                    break;
                case 4:
                    Challenger.rivalRock = true;
                    break;
                case 5:
                    Challenger.rivalPaper = true;
                    break;
                case 6:
                    Challenger.rivalScissors = true;
                    break;
            }
        }

        public static void ninjaKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Ninja.ninja.MurderPlayer(player);
                    Ninja.ninja.transform.position = player.transform.position;
                    return;
                }
            }
        }

        public static void berserkerKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Berserker.berserker.MurderPlayer(player);
                    if (!Berserker.killedFirstTime) {
                        Berserker.killedFirstTime = true;
                    }
                    else {
                        Berserker.timeToKill = Berserker.backupTimeToKill;
                    }
                    return;
                }
            }
        }

        public static void yandereKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Yandere.yandere.MurderPlayer(player);
                    if (!Yandere.rampageMode) {
                        Yandere.yandere.MurderPlayer(Yandere.yandere);
                        Yandere.triggerYandereWin = true;
                    }
                    return;
                }
            }
        }

        public static void strandedFindBoxes(byte strandedFindBoxes) {
            switch (strandedFindBoxes) {
                case 1:
                    Stranded.storedAmmo += 1;
                    Stranded.strandedSearchButtonText.text = $"{Stranded.storedAmmo} / 3"; 
                    break;
                case 2:
                    Stranded.canVent = true;
                    break;
                case 3:
                    Stranded.canTurnInvisible = true;
                    break;
            }
        }
        
        public static void strandedKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Stranded.stranded.MurderPlayer(player);
                    Stranded.howManyKills += 1;
                    Stranded.storedAmmo -= 1;
                    Stranded.strandedSearchButtonText.text = $"{Stranded.storedAmmo} / 3"; 
                    Stranded.strandedKillButtonText.text = $"{Stranded.howManyKills} / 3"; 
                    if (Stranded.howManyKills >= 3) {
                        Stranded.triggerStrandedWin = true;
                    }
                    return;
                }
            }
        }

        public static void strandedInvisible() {
            if (Stranded.stranded == null) return;

            Stranded.invisibleTimer = 60;
            Stranded.isInvisible = true;
            Stranded.canTurnInvisible = false;
        }

        public static void monjaTakeItem(byte monjaItemId) {
            if (Monja.monja != null) {
                Monja.isDeliveringItem = true;
                Monja.itemId = monjaItemId;
                switch(monjaItemId) {
                    case 1:
                        Monja.item01.transform.parent = Monja.monja.transform;
                        Monja.item01.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                        break;
                    case 2:
                        Monja.item02.transform.parent = Monja.monja.transform;
                        Monja.item02.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                        break;
                    case 3:
                        Monja.item03.transform.parent = Monja.monja.transform;
                        Monja.item03.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                        break;
                    case 4:
                        Monja.item04.transform.parent = Monja.monja.transform;
                        Monja.item04.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                        break;
                    case 5:
                        Monja.item05.transform.parent = Monja.monja.transform;
                        Monja.item05.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                        break;
                }
            }
        }

        public static void monjaDeliverItem(byte monjaItemId) {
            if (Monja.monja != null) {
                Monja.isDeliveringItem = false;
                Monja.itemId = 0;
                switch (monjaItemId) {
                    case 1:
                        if (PlayerControl.LocalPlayer == Monja.monja) {
                            Monja.item01.SetActive(true);
                        }
                        else {
                            HudManager.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>((p) => {
                                if (p == 1f) {
                                    Monja.item01.SetActive(true);
                                }
                            })));
                        }
                        Monja.item01.transform.SetParent(null);
                        Monja.item01.transform.localPosition = Monja.ritualObject.transform.GetChild(0).transform.position + new Vector3(0f, 0f, -0.1f);
                        Monja.item01.name = "deliveredItem";
                        break;
                    case 2:
                        if (PlayerControl.LocalPlayer == Monja.monja) {
                            Monja.item02.SetActive(true);
                        }
                        else {
                            HudManager.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>((p) => {
                                if (p == 1f) {
                                    Monja.item02.SetActive(true);
                                }
                            })));
                        }
                        Monja.item02.transform.SetParent(null);
                        Monja.item02.transform.localPosition = Monja.ritualObject.transform.GetChild(1).transform.position + new Vector3(0f, 0f, -0.1f);
                        Monja.item02.name = "deliveredItem"; 
                        break;
                    case 3:
                        if (PlayerControl.LocalPlayer == Monja.monja) {
                            Monja.item03.SetActive(true);
                        }
                        else {
                            HudManager.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>((p) => {
                                if (p == 1f) {
                                    Monja.item03.SetActive(true);
                                }
                            })));
                        }
                        Monja.item03.transform.SetParent(null);
                        Monja.item03.transform.localPosition = Monja.ritualObject.transform.GetChild(2).transform.position + new Vector3(0f, 0f, -0.1f);
                        Monja.item03.name = "deliveredItem"; 
                        break;
                    case 4:
                        if (PlayerControl.LocalPlayer == Monja.monja) {
                            Monja.item04.SetActive(true);
                        }
                        else {
                            HudManager.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>((p) => {
                                if (p == 1f) {
                                    Monja.item04.SetActive(true);
                                }
                            })));
                        }
                        Monja.item04.transform.SetParent(null);
                        Monja.item04.transform.localPosition = Monja.ritualObject.transform.GetChild(3).transform.position + new Vector3(0f, 0f, -0.1f);
                        Monja.item04.name = "deliveredItem"; 
                        break;
                    case 5:
                        if (PlayerControl.LocalPlayer == Monja.monja) {
                            Monja.item05.SetActive(true);
                        }
                        else {
                            HudManager.Instance.StartCoroutine(Effects.Lerp(5, new Action<float>((p) => {
                                if (p == 1f) {
                                    Monja.item05.SetActive(true);
                                }
                            })));
                        }
                        Monja.item05.transform.SetParent(null);
                        Monja.item05.transform.localPosition = Monja.ritualObject.transform.GetChild(4).transform.position + new Vector3(0f, 0f, -0.1f);
                        Monja.item05.name = "deliveredItem"; 
                        break;
                }
            }                                                   
            Monja.deliveredItems += 1;
            Monja.objectCountButtonText.text = $"{Monja.deliveredItems} / 5";
            if (Monja.deliveredItems >= 5) {
                Monja.canAwake = true;
            }
        }

        public static void monjaRevertItemPosition(byte monjaItemId) {
            if (Monja.monja != null) {
                Monja.isDeliveringItem = false;
                Monja.itemId = 0;
                switch (monjaItemId) {
                    case 1:
                        Monja.item01.transform.SetParent(null);
                        Monja.item01.transform.localPosition = Monja.itemListPositions[0];
                        break;
                    case 2:
                        Monja.item02.transform.SetParent(null);
                        Monja.item02.transform.localPosition = Monja.itemListPositions[1];
                        break;
                    case 3:
                        Monja.item03.transform.SetParent(null);
                        Monja.item03.transform.localPosition = Monja.itemListPositions[2];
                        break;
                    case 4:
                        Monja.item04.transform.SetParent(null);
                        Monja.item04.transform.localPosition = Monja.itemListPositions[3];
                        break;
                    case 5:
                        Monja.item05.transform.SetParent(null);
                        Monja.item05.transform.localPosition = Monja.itemListPositions[4];
                        break;
                }
            }
        }

        public static void monjaAwakened() {
            if (Monja.monja == null) return;

            // Remove body dragging for janitor
            if (Janitor.janitor != null && Janitor.dragginBody) {
                Janitor.janitorResetValuesAtDead();
            }

            // Remove body dragging for necromancer
            if (Necromancer.necromancer != null && Necromancer.dragginBody) {
                Necromancer.necromancerResetValuesAtDead();
            }
            
            Monja.canAwake = false;
            Monja.awakened = true;
            Monja.awakenTimer = 60;
            Monja.monjaSprite.SetActive(true);

            if (MapBehaviour.Instance) {
                MapBehaviour.Instance.Close();
            }
            if (Minigame.Instance) {
                Minigame.Instance.ForceClose();
            }
            // Teleport and bodies to other place
            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            if (array.Count() != 0) {
                for (int i = 0; i < array.Length; i++) {
                    array[i].gameObject.transform.position = new Vector3(50, 50, array[i].gameObject.transform.position.z);
                }
            }
            // Exit current vent if necessary
            if (PlayerControl.LocalPlayer.inVent) {
                foreach (Vent vent in ShipStatus.Instance.AllVents) {
                    bool canUse;
                    bool couldUse;
                    vent.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
                    if (canUse) {
                        PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(vent.Id);
                        vent.SetButtons(false);
                    }
                }
            }
            // Music Stop and play bomb music
            changeMusic(7);
            SoundManager.Instance.PlaySound(CustomMain.customAssets.monjaAwakeMusic, true, 75f);
            SoundManager.Instance.StopSound(CustomMain.customAssets.bombermanBombMusic);
            SoundManager.Instance.StopSound(CustomMain.customAssets.performerMusic);
            // Indicate monja text
            new CustomMessage(Language.statusRolesTexts[8], 60, -1, 1.6f, 27);

            HudManager.Instance.FullScreen.color = new Color(0.75f, 0f, 0f, 0.5f);
            HudManager.Instance.FullScreen.enabled = true;
            HudManager.Instance.FullScreen.gameObject.SetActive(true);
            HudManager.Instance.StartCoroutine(Effects.Lerp(60, new Action<float>((p) => {
                if (p == 1f && Monja.monja != null && !Monja.monja.Data.IsDead) {
                    monjaReset();
                }
            })));
        }

        public static void monjaKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Monja.monja.MurderPlayer(player);                    
                    return;
                }
            }
        }

        public static void monjaReset () {
            HudManager.Instance.FullScreen.enabled = false;
            Monja.monjaSprite.SetActive(false);
            Monja.awakened = false;
            timeTravelerRewindTimeButton.Timer = timeTravelerRewindTimeButton.MaxTimer;
            timeTravelerShieldButton.Timer = timeTravelerShieldButton.MaxTimer;
            monjaFindDeliverButton.HasEffect = false;
            HudManager.Instance.PlayerCam.shakeAmount = 0f;
            HudManager.Instance.PlayerCam.shakePeriod = 0;
            changeMusic(2);
        }

        public static PlayerControl oldRoleThief = null;

        public static void roleThiefSteal(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId && RoleThief.rolethief != null) {
                    // Suicide when impostor or rebel variants
                    if (player.Data.Role.IsImpostor || player == Renegade.renegade || player == Minion.minion || player == BountyHunter.bountyhunter || player == Trapper.trapper || player == Yinyanger.yinyanger || player == Challenger.challenger || player == Ninja.ninja || player == Berserker.berserker || player == Yandere.yandere || player == Stranded.stranded || player == Monja.monja) {
                        RoleThief.rolethief.MurderPlayer(RoleThief.rolethief);
                        return;
                    }

                    oldRoleThief = RoleThief.rolethief;
                    // Switch tasks
                    var roleThiefSabotageTasks = oldRoleThief.myTasks;
                    oldRoleThief.myTasks = player.myTasks;
                    player.myTasks = roleThiefSabotageTasks;

                    // Switch shield
                    if (Squire.shielded != null && Squire.shielded == player) {
                        Squire.shielded.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 0f);
                        Squire.shielded = oldRoleThief;
                    }
                    else if (Squire.shielded != null && Squire.shielded == oldRoleThief) {
                        Squire.shielded.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 0f);
                        Squire.shielded = player;
                    }

                    // Switch role
                    if (Captain.captain != null && Captain.captain == player) {
                        Captain.captain = oldRoleThief;
                    }
                    else if (Mechanic.mechanic != null && Mechanic.mechanic == player) {
                        Mechanic.mechanic = oldRoleThief;
                    }
                    else if (Sheriff.sheriff != null && Sheriff.sheriff == player) {
                        Sheriff.sheriff = oldRoleThief;
                    }
                    else if (Detective.detective != null && Detective.detective == player) {
                        Detective.detective = oldRoleThief;
                    }
                    else if (Forensic.forensic != null && Forensic.forensic == player) {
                        Forensic.forensic = oldRoleThief;
                    }
                    else if (TimeTraveler.timeTraveler != null && TimeTraveler.timeTraveler == player) {
                        TimeTraveler.timeTraveler = oldRoleThief;
                    }
                    else if (Squire.squire != null && Squire.squire == player) {
                        Squire.squire = oldRoleThief;
                    }
                    else if (Cheater.cheater != null && Cheater.cheater == player) {
                        Cheater.cheater = oldRoleThief;
                    }
                    else if (FortuneTeller.fortuneTeller != null && FortuneTeller.fortuneTeller == player) {
                        FortuneTeller.fortuneTeller = oldRoleThief;
                    }
                    else if (Hacker.hacker != null && Hacker.hacker == player) {
                        if (Hacker.vitals != null) {
                            Hacker.vitals.ForceClose();
                        }
                        if (Hacker.hacker == PlayerControl.LocalPlayer) {
                            if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
                        }
                        Hacker.hacker = oldRoleThief;
                    }
                    else if (Sleuth.sleuth != null && Sleuth.sleuth == player) {
                        Sleuth.sleuth = oldRoleThief;
                    }
                    else if (Fink.fink != null && Fink.fink == player) {
                        Fink.resetCamera();
                        Fink.fink.moveable = true;
                        Fink.fink = oldRoleThief;
                    }
                    else if (Kid.kid != null && Kid.kid == player) {
                        Kid.kid = oldRoleThief;
                    }
                    else if (Welder.welder != null && Welder.welder == player) {
                        Welder.welder = oldRoleThief;
                    }
                    else if (Spiritualist.spiritualist != null && Spiritualist.spiritualist == player) {
                        Spiritualist.spiritualist = oldRoleThief;
                    }
                    else if (Coward.coward != null && Coward.coward == player) {
                        Coward.coward = oldRoleThief;
                    }
                    else if (Vigilant.vigilant != null && Vigilant.vigilant == player) {
                        if (Vigilant.minigame != null) {
                            Vigilant.minigame.ForceClose();
                            Vigilant.minigame = null;
                        }
                        Vigilant.vigilant = oldRoleThief;
                    }
                    else if (Vigilant.vigilantMira != null && Vigilant.vigilantMira == player) {
                        // Vigilant delete doorlog item when switch rol
                        GameObject vigilantdoorlog = GameObject.Find("VigilantDoorLog");
                        if (vigilantdoorlog != null) {
                            UnityEngine.Object.Destroy(vigilantdoorlog);
                        }
                        Vigilant.vigilantMira = oldRoleThief;
                        // Recreate the doorlog access from anywhere to the new Vigilant after rol switch
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 1 && Vigilant.vigilantMira == PlayerControl.LocalPlayer) {
                            GameObject vigilantDoorLog = GameObject.Find("SurvLogConsole");
                            Vigilant.doorLog = GameObject.Instantiate(vigilantDoorLog, Vigilant.vigilantMira.transform);
                            Vigilant.doorLog.name = "VigilantDoorLog";
                            Vigilant.doorLog.layer = 8; // Player layer to ignore collisions
                            Vigilant.doorLog.GetComponent<SpriteRenderer>().enabled = false;
                            Vigilant.doorLog.transform.localPosition = new Vector2(0, -0.5f);
                        }
                    }
                    else if (Hunter.hunter != null && Hunter.hunter == player) {
                        Hunter.hunter = oldRoleThief;
                    }
                    else if (Jinx.jinx != null && Jinx.jinx == player) {
                        Jinx.jinx = oldRoleThief;
                    }
                    else if (Bat.bat != null && Bat.bat == player) {
                        Bat.bat = oldRoleThief;
                    }
                    else if (Necromancer.necromancer != null && Necromancer.necromancer == player) {
                        Necromancer.necromancer = oldRoleThief;
                    }
                    else if (Engineer.engineer != null && Engineer.engineer == player) {
                        Engineer.engineer = oldRoleThief;
                    }
                    else if (Shy.shy != null && Shy.shy == player) {
                        Shy.shy = oldRoleThief;
                    }
                    else if (TaskMaster.taskMaster != null && TaskMaster.taskMaster == player) {
                        TaskMaster.taskMaster = oldRoleThief;
                        // Randomize again and reset the tasks for taskmaster after steal role
                        if (TaskMaster.clearedInitialTasks) {
                            byte[] taskTypeIds = TasksHandler.GetTaskMasterTasks(TaskMaster.taskMaster);
                            taskMasterSetExTasks(TaskMaster.taskMaster.PlayerId, byte.MaxValue, taskTypeIds);
                        }
                    }
                    else if (Jailer.jailer != null && Jailer.jailer == player) {
                        Jailer.jailer = oldRoleThief;
                    }
                    else { // Crewmate
                    }

                    RoleThief.rolethief = player;

                    // Set cooldowns to max for both players
                    if (PlayerControl.LocalPlayer == RoleThief.rolethief || PlayerControl.LocalPlayer == oldRoleThief)
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.roleThiefStealRole, false, 100f);
                    CustomButton.ResetAllCooldowns();
                }
            }
        }

        public static void pyromaniacWin() {
            SoundManager.Instance.PlaySound(CustomMain.customAssets.pyromaniacIgniteClip, false, 100f);
            Pyromaniac.triggerPyromaniacWin = true;
            foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                if (p != Pyromaniac.pyromaniac && (Kid.kid != null && p != Kid.kid)) p.Exiled();
            }
        }

        public static void placeTreasure() {
            TreasureHunter.canPlace = false;
            new Treasure(1800);
        }

        public static void collectedTreasure() {
            if (PlayerControl.LocalPlayer == TreasureHunter.treasureHunter) {
                SoundManager.Instance.PlaySound(CustomMain.customAssets.treasureHunterCollectTreasure, false, 100f);
            }
            TreasureHunter.treasureCollected += 1;
            TreasureHunter.treasureCounterButtonText.text = $"{TreasureHunter.treasureCollected} / {TreasureHunter.neededTreasure}";
            TreasureHunter.canPlace = true;
            if (TreasureHunter.treasureCollected >= TreasureHunter.neededTreasure) {
                TreasureHunter.triggertreasureHunterWin = true;
            }
        }

        public static void devourBody(byte playerId) {
            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++) {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == playerId) {
                    UnityEngine.Object.Destroy(array[i].gameObject);
                    if (Janitor.janitor != null && Janitor.dragginBody && Janitor.bodyId == playerId) {
                        janitorResetValues();
                    }
                    else if (Necromancer.necromancer != null && Necromancer.dragginBody && Necromancer.bodyId == playerId) {
                        necromancerResetValues();
                    }
                }
            }
            if (Modifiers.performer != null && playerId == Modifiers.performer.PlayerId)
                performerIsReported(0);
            if (PlayerControl.LocalPlayer == Devourer.devourer) {
                SoundManager.Instance.PlaySound(CustomMain.customAssets.devourerDevourClip, false, 100f);
            }
            Devourer.devouredBodies += 1;
            Devourer.devourCounterButtonText.text = $"{Devourer.devouredBodies} / {Devourer.neededBodies}";
            if (Devourer.devouredBodies >= Devourer.neededBodies) {
                Devourer.triggerdevourerWin = true;
            }
        }

        public static void poisonerWin() {
            SoundManager.Instance.PlaySound(CustomMain.customAssets.poisonerPoisonClip, false, 100f);
            Poisoner.triggerPoisonerWin = true;
            foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                if (p != Poisoner.poisoner && (Kid.kid != null && p != Kid.kid)) p.Exiled();
            }
        }
        public static void puppeteerTransform(byte playerId) {
            PlayerControl target = Helpers.playerById(playerId);
            if (Puppeteer.puppeteer == null || target == null) return;
            Puppeteer.positionPreMorphed = Puppeteer.puppeteer.transform.position;
            Puppeteer.morphed = true;
            Puppeteer.transformTarget = target;
            if (Painter.painterTimer <= 0f)
                Puppeteer.puppeteer.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void puppeteerWin() {
            Puppeteer.triggerPuppeteerWin = true;
        }

        public static void puppeteerResetTransform() {
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                if (Puppeteer.puppeteer.transform.position.y > 0) {
                    Puppeteer.puppeteer.transform.position = new Vector3(5.5f, 31.5f, -5);
                }
                else {
                    Puppeteer.puppeteer.transform.position = new Vector3(-4.75f, -33.25f, -5);
                }
            }
            else {
                Puppeteer.puppeteer.transform.position = Puppeteer.positionPreMorphed;
            }
            HudManagerStartPatch.puppeteerTransformButton.Timer = HudManagerStartPatch.puppeteerTransformButton.MaxTimer;
            HudManagerStartPatch.puppeteerSampleButton.Timer = HudManagerStartPatch.puppeteerSampleButton.MaxTimer;
            Puppeteer.morphed = false;
            Puppeteer.puppeteer.setDefaultLook();
            Puppeteer.transformTarget = null;
            Puppeteer.pickTarget = null;
            Puppeteer.currentTarget = null;

        }
        
        public static void exilerWin() {
            Exiler.triggerExilerWin = true;
        }
        
        public static void amnesiacReportAndTakeRole(byte targetId) {
            PlayerControl target = Helpers.playerById(targetId);
            GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(targetId);
            PlayerControl oldAmnesiac = Amnesiac.amnesiac;
            if (target == null || oldAmnesiac == null) return;
            List<RoleInfo> targetInfo = RoleInfo.getRoleInfoForPlayer(target);
            RoleInfo roleInfo = targetInfo.Where(info => !info.isModifier).FirstOrDefault();
            switch ((RoleId)roleInfo.roleId) {
                case RoleId.Mimic:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Mimic.clearAndReload();
                    Mimic.mimic = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Painter:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Painter.clearAndReload();
                    Painter.painter = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Demon:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Demon.clearAndReload();
                    Demon.demon = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Janitor:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Janitor.clearAndReload();
                    Janitor.janitor = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Illusionist:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Illusionist.clearAndReload();
                    Illusionist.illusionist = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Manipulator:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Manipulator.clearAndReload();
                    Manipulator.manipulator = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Bomberman:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Bomberman.clearAndReload();
                    Bomberman.bomberman = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Chameleon:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Chameleon.clearAndReload();
                    Chameleon.chameleon = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Gambler:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Gambler.clearAndReload();
                    Gambler.gambler = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Sorcerer:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Sorcerer.clearAndReload();
                    Sorcerer.sorcerer = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Medusa:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Medusa.clearAndReload();
                    Medusa.medusa = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Hypnotist:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Hypnotist.clearAndReload();
                    Hypnotist.hypnotist = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Archer:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Archer.clearAndReload();
                    Archer.archer = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Plumber:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Plumber.clearAndReload();
                    Plumber.plumber = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Librarian:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Librarian.clearAndReload();
                    Librarian.librarian = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Helpers.turnIntoCrewmate(Amnesiac.amnesiac);
                    break;
                case RoleId.Renegade:
                    //Renegade.clearAndReload(); Don't reset Renegade to prevent bugs
                    Renegade.renegade = oldAmnesiac;
                    Renegade.formerRenegades.Add(oldAmnesiac);
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Renegade.formerRenegades.Remove(target);
                    break;
                case RoleId.Minion:
                    Renegade.formerRenegades.Add(target);
                    Minion.clearAndReload();
                    Minion.minion = oldAmnesiac;
                    Renegade.formerRenegades.Add(oldAmnesiac);
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    Renegade.formerRenegades.Remove(target);
                    break;
                case RoleId.BountyHunter:
                    // Can't remember BountyHunter role
                    amnesiacCantRemember();
                    break;
                case RoleId.Trapper:
                    Trapper.clearAndReload();
                    Trapper.trapper = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Yinyanger:
                    Yinyanger.clearAndReload();
                    Yinyanger.yinyanger = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Challenger:
                    Challenger.clearAndReload();
                    Challenger.challenger = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Ninja:
                    Ninja.clearAndReload();
                    Ninja.ninja = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Berserker:
                    Berserker.clearAndReload();
                    Berserker.berserker = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Yandere:
                    // Can't remember Yandere role
                    amnesiacCantRemember();
                    break;
                case RoleId.Stranded:
                    // Can't remember Stranded role
                    amnesiacCantRemember();
                    break;
                case RoleId.Monja:
                    // Can't remember Monja role
                    amnesiacCantRemember();
                    break;
                case RoleId.Captain:
                    Captain.clearAndReload();
                    Captain.captain = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Mechanic:
                    Mechanic.clearAndReload();
                    Mechanic.mechanic = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Sheriff:
                    //Sheriff.clearAndReload(); No reset needed
                    Sheriff.sheriff = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Detective:
                    Detective.clearAndReload();
                    Detective.detective = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Forensic:
                    Forensic.clearAndReload();
                    Forensic.forensic = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.TimeTraveler:
                    TimeTraveler.clearAndReload();
                    TimeTraveler.timeTraveler = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Squire:
                    Squire.clearAndReload();
                    Squire.squire = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Cheater:
                    Cheater.clearAndReload();
                    Cheater.cheater = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.FortuneTeller:
                    FortuneTeller.clearAndReload();
                    FortuneTeller.fortuneTeller = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Hacker:
                    Hacker.clearAndReload();
                    Hacker.hacker = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Sleuth:
                    Sleuth.clearAndReload();
                    Sleuth.sleuth = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Fink:
                    Fink.clearAndReload();
                    Fink.fink = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Welder:
                    // Welder.clearAndReload(); Keep already sealed vents
                    Welder.welder = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Spiritualist:
                    Spiritualist.clearAndReload();
                    Spiritualist.spiritualist = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Vigilant:
                    //Vigilant.clearAndReload(); Keep already placed cameras
                    Vigilant.vigilant = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.VigilantMira:
                    // Vigilant delete doorlog item when switch rol
                    if (Vigilant.vigilantMira != null && Vigilant.vigilantMira == PlayerControl.LocalPlayer) {
                        GameObject vigilantdoorlog = GameObject.Find("VigilantDoorLog");
                        if (vigilantdoorlog != null) {
                            UnityEngine.Object.Destroy(vigilantdoorlog);
                        }
                    }
                    //Vigilant.clearAndReload(); Keep already placed cameras
                    Vigilant.vigilantMira = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    // Recreate the doorlog access from anywhere to the new Vigilant after rol switch
                    if (Vigilant.vigilantMira == PlayerControl.LocalPlayer) {
                        GameObject vigilantDoorLog = GameObject.Find("SurvLogConsole");
                        Vigilant.doorLog = GameObject.Instantiate(vigilantDoorLog, Vigilant.vigilantMira.transform);
                        Vigilant.doorLog.name = "VigilantDoorLog";
                        Vigilant.doorLog.layer = 8; // Player layer to ignore collisions
                        Vigilant.doorLog.GetComponent<SpriteRenderer>().enabled = false;
                        Vigilant.doorLog.transform.localPosition = new Vector2(0, -0.5f);
                    }
                    break;
                case RoleId.Hunter:
                    Hunter.clearAndReload();
                    Hunter.hunter = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Jinx:
                    Jinx.clearAndReload();
                    Jinx.jinx = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Coward:
                    Coward.clearAndReload();
                    Coward.coward = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Bat:
                    Bat.clearAndReload();
                    Bat.bat = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Necromancer:
                    //Necromancer.clearAndReload(); No reset needed
                    Necromancer.necromancer = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Engineer:
                    //Engineer.clearAndReload(); Keep placed traps
                    Engineer.engineer = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Shy:
                    Shy.clearAndReload();
                    Shy.shy = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.TaskMaster:
                    TaskMaster.clearAndReload();
                    TaskMaster.taskMaster = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    // Randomize again and reset the tasks for taskmaster after steal role
                    if (TaskMaster.clearedInitialTasks) {
                        byte[] taskTypeIds = TasksHandler.GetTaskMasterTasks(TaskMaster.taskMaster);
                        taskMasterSetExTasks(TaskMaster.taskMaster.PlayerId, byte.MaxValue, taskTypeIds);
                    }
                    break;
                case RoleId.Jailer:
                    Jailer.clearAndReload();
                    Jailer.jailer = oldAmnesiac;
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Crewmate:
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
                case RoleId.Impostor:
                    Helpers.turnIntoImpostor(Amnesiac.amnesiac);
                    Amnesiac.clearAndReload();
                    Amnesiac.amnesiac = target;
                    break;
            }

            // Bomberman bomb reset when report the chosen one
            if (Bomberman.bomberman != null && Bomberman.activeBomb == true) {
                fixBomb();
            }
            Helpers.handleDemonBiteOnBodyReport(); // Manually call Demon handling, since the CmdReportDeadBody Prefix won't be called
            HudManager.Instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => { // Delayed action
                if (p == 1f) {
                    oldAmnesiac.CmdReportDeadBody(playerInfo);
                }
            })));
            changeMusic(1);
        }

        public static void amnesiacCantRemember() {
            if (Amnesiac.amnesiac == PlayerControl.LocalPlayer) {
                string msg = $"{Language.statusRolesTexts[9]}";
                if (!string.IsNullOrWhiteSpace(msg)) {
                    if (AmongUsClient.Instance.AmClient && DestroyableSingleton<HudManager>.Instance) {
                        DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, msg);
                    }
                    if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0) {
                        DestroyableSingleton<Assets.CoreScripts.Telemetry>.Instance.SendWho();
                    }
                }
            }
        }

        public static void seekerSetMinigamePlayers(byte targetId) {

            if (Seeker.seeker == null) return;

            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId && !player.Data.IsDead) {
                    if (Seeker.hidedPlayerOne == null) {
                        Seeker.hidedPlayerOne = player;
                        Seeker.currentPlayers += 1;
                    }
                    else if (Seeker.hidedPlayerTwo == null) {
                        Seeker.hidedPlayerTwo = player;
                        Seeker.currentPlayers += 1;
                    }
                    else if (Seeker.hidedPlayerThree == null) {
                        Seeker.hidedPlayerThree = player;
                        Seeker.currentPlayers += 1;
                    }
                }
                Seeker.seekerPlayerPointsCount.text = $"{Seeker.currentPlayers} / 3";
            }

            if (Seeker.currentPlayers == 3) {
                Seeker.minigameReady = true;
            }
        }

        public static void seekerResetMinigamePlayers(byte option) {

            if (Seeker.seeker == null) return;

            switch (option) {
                case 1:
                    Seeker.ResetOnePlayer(1);
                    break;
                case 2:
                    Seeker.ResetOnePlayer(2);
                    break;
                case 3:
                    Seeker.ResetOnePlayer(3);
                    break;
                case 4:
                    Seeker.ResetValues(false);
                    break;
            }
        }

        public static void seekerPerformMinigame() {

            // Remove body dragging for janitor
            if (Janitor.janitor != null && Janitor.dragginBody) {
                Janitor.janitorResetValuesAtDead();
            }

            // Remove body dragging for necromancer
            if (Necromancer.necromancer != null && Necromancer.dragginBody) {
                Necromancer.necromancerResetValuesAtDead();
            }

            // Force exit from vents to all players
            if (PlayerControl.LocalPlayer.inVent) {
                foreach (Vent vent in ShipStatus.Instance.AllVents) {
                    bool canUse;
                    bool couldUse;
                    vent.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
                    if (canUse) {
                        PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(vent.Id);
                        vent.SetButtons(false);
                    }
                }
            }

            // Force map close to all players
            if (MapBehaviour.Instance) {
                MapBehaviour.Instance.Close();
            }
            // Force task close to all players
            if (Minigame.Instance)
                Minigame.Instance.ForceClose();

            new CustomMessage(Language.introTexts[1], Seeker.minigameDuration, -1, -1.3f, 28);

            // music stop and play duel music
            changeMusic(8);
            SoundManager.Instance.PlaySound(CustomMain.customAssets.seekerMinigameMusic, false, 5f);
            SoundManager.Instance.StopSound(CustomMain.customAssets.performerMusic);

            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player == PlayerControl.LocalPlayer) {
                    positionBeforeMinigame = player.transform.position;
                }
            }

            Seeker.minigameDuration = 20f;
            Seeker.isMinigaming = true;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player == Seeker.seeker) {
                    player.transform.position = new Vector3(-39.5f, 1.9f, player.transform.position.z);
                }
                else if (player == Seeker.hidedPlayerOne || player == Seeker.hidedPlayerTwo || player == Seeker.hidedPlayerThree) {
                    player.transform.position = new Vector3(-37.75f, 0.75f, player.transform.position.z);
                }
                else {
                    player.transform.position = new Vector3(-35.5f, 1.9f, player.transform.position.z);
                }
            }

            resetMinigameButtons();
        }

        public static void seekerSelectAttack(byte whichAttack) {
            switch (whichAttack) {
                case 1:
                    Seeker.seekerSelectedHiding = 1;
                    break;
                case 2:
                    Seeker.seekerSelectedHiding = 2;
                    break;
                case 3:
                    Seeker.seekerSelectedHiding = 3;
                    break;
                case 4:
                    Seeker.hidedPlayerOneSelectedHiding = 1;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerOne.transform.position.y > 0)) {
                        Seeker.hidedPlayerOne.transform.position = new Vector3(Seeker.minigameArenaHideOnePointOne.transform.position.x, Seeker.minigameArenaHideOnePointOne.transform.position.y, Seeker.hidedPlayerOne.transform.position.z);
                    } else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerOne.transform.position.y < 0) {
                        Seeker.hidedPlayerOne.transform.position = new Vector3(Seeker.lowerminigameArenaHideOnePointOne.transform.position.x, Seeker.lowerminigameArenaHideOnePointOne.transform.position.y, Seeker.hidedPlayerOne.transform.position.z);
                    }
                    seekerStopMovements(1);
                    break;
                case 5:
                    Seeker.hidedPlayerOneSelectedHiding = 2;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerOne.transform.position.y > 0)) {
                        Seeker.hidedPlayerOne.transform.position = new Vector3(Seeker.minigameArenaHideTwoPointOne.transform.position.x, Seeker.minigameArenaHideTwoPointOne.transform.position.y, Seeker.hidedPlayerOne.transform.position.z);
                    }
                    else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerOne.transform.position.y < 0) {
                        Seeker.hidedPlayerOne.transform.position = new Vector3(Seeker.lowerminigameArenaHideTwoPointOne.transform.position.x, Seeker.lowerminigameArenaHideTwoPointOne.transform.position.y, Seeker.hidedPlayerOne.transform.position.z);
                    }
                    seekerStopMovements(1);
                    break;
                case 6:
                    Seeker.hidedPlayerOneSelectedHiding = 3;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerOne.transform.position.y > 0)) {
                        Seeker.hidedPlayerOne.transform.position = new Vector3(Seeker.minigameArenaHideThreePointOne.transform.position.x, Seeker.minigameArenaHideThreePointOne.transform.position.y, Seeker.hidedPlayerOne.transform.position.z);
                    }
                    else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerOne.transform.position.y < 0) {
                        Seeker.hidedPlayerOne.transform.position = new Vector3(Seeker.lowerminigameArenaHideThreePointOne.transform.position.x, Seeker.lowerminigameArenaHideThreePointOne.transform.position.y, Seeker.hidedPlayerOne.transform.position.z);
                    }
                    seekerStopMovements(1);
                    break;
                case 7:
                    Seeker.hidedPlayerTwoSelectedHiding = 1;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerTwo.transform.position.y > 0)) {
                        Seeker.hidedPlayerTwo.transform.position = new Vector3(Seeker.minigameArenaHideOnePointTwo.transform.position.x, Seeker.minigameArenaHideOnePointTwo.transform.position.y, Seeker.hidedPlayerTwo.transform.position.z);
                    }
                    else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerTwo.transform.position.y < 0) {
                        Seeker.hidedPlayerTwo.transform.position = new Vector3(Seeker.lowerminigameArenaHideOnePointTwo.transform.position.x, Seeker.lowerminigameArenaHideOnePointTwo.transform.position.y, Seeker.hidedPlayerTwo.transform.position.z);
                    }
                    seekerStopMovements(2); 
                    break;
                case 8:
                    Seeker.hidedPlayerTwoSelectedHiding = 2;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerTwo.transform.position.y > 0)) {
                        Seeker.hidedPlayerTwo.transform.position = new Vector3(Seeker.minigameArenaHideTwoPointTwo.transform.position.x, Seeker.minigameArenaHideTwoPointTwo.transform.position.y, Seeker.hidedPlayerTwo.transform.position.z);
                    }
                    else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerTwo.transform.position.y < 0) {
                        Seeker.hidedPlayerTwo.transform.position = new Vector3(Seeker.lowerminigameArenaHideTwoPointTwo.transform.position.x, Seeker.lowerminigameArenaHideTwoPointTwo.transform.position.y, Seeker.hidedPlayerTwo.transform.position.z);
                    }
                    seekerStopMovements(2); 
                    break;
                case 9:
                    Seeker.hidedPlayerTwoSelectedHiding = 3;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerTwo.transform.position.y > 0)) {
                        Seeker.hidedPlayerTwo.transform.position = new Vector3(Seeker.minigameArenaHideThreePointTwo.transform.position.x, Seeker.minigameArenaHideThreePointTwo.transform.position.y, Seeker.hidedPlayerTwo.transform.position.z);
                    }
                    else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerTwo.transform.position.y < 0) {
                        Seeker.hidedPlayerTwo.transform.position = new Vector3(Seeker.lowerminigameArenaHideThreePointTwo.transform.position.x, Seeker.lowerminigameArenaHideThreePointTwo.transform.position.y, Seeker.hidedPlayerTwo.transform.position.z);
                    }
                    seekerStopMovements(2); 
                    break;
                case 10:
                    Seeker.hidedPlayerThreeSelectedHiding = 1;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerThree.transform.position.y > 0)) {
                        Seeker.hidedPlayerThree.transform.position = new Vector3(Seeker.minigameArenaHideOnePointThree.transform.position.x, Seeker.minigameArenaHideOnePointThree.transform.position.y, Seeker.hidedPlayerThree.transform.position.z);
                    }
                    else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerThree.transform.position.y < 0) {
                        Seeker.hidedPlayerThree.transform.position = new Vector3(Seeker.lowerminigameArenaHideOnePointThree.transform.position.x, Seeker.lowerminigameArenaHideOnePointThree.transform.position.y, Seeker.hidedPlayerThree.transform.position.z);
                    }
                    seekerStopMovements(3);
                    break;
                case 11:
                    Seeker.hidedPlayerThreeSelectedHiding = 2;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerThree.transform.position.y > 0)) {
                        Seeker.hidedPlayerThree.transform.position = new Vector3(Seeker.minigameArenaHideTwoPointThree.transform.position.x, Seeker.minigameArenaHideTwoPointThree.transform.position.y, Seeker.hidedPlayerThree.transform.position.z);
                    }
                    else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerThree.transform.position.y < 0) {
                        Seeker.hidedPlayerThree.transform.position = new Vector3(Seeker.lowerminigameArenaHideTwoPointThree.transform.position.x, Seeker.lowerminigameArenaHideTwoPointThree.transform.position.y, Seeker.hidedPlayerThree.transform.position.z);
                    }
                    seekerStopMovements(3); 
                    break;
                case 12:
                    Seeker.hidedPlayerThreeSelectedHiding = 3;
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5 || (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerThree.transform.position.y > 0)) {
                        Seeker.hidedPlayerThree.transform.position = new Vector3(Seeker.minigameArenaHideThreePointThree.transform.position.x, Seeker.minigameArenaHideThreePointThree.transform.position.y, Seeker.hidedPlayerThree.transform.position.z);
                    }
                    else if (GameOptionsManager.Instance.currentGameOptions.MapId == 5 && Seeker.hidedPlayerThree.transform.position.y < 0) {
                        Seeker.hidedPlayerThree.transform.position = new Vector3(Seeker.lowerminigameArenaHideThreePointThree.transform.position.x, Seeker.lowerminigameArenaHideThreePointThree.transform.position.y, Seeker.hidedPlayerThree.transform.position.z);
                    }
                    seekerStopMovements(3); 
                    break;
            }
            Seeker.howmanyselectedattacks += 1;
        }

        public static void seekerStopMovements(byte whichHider) {
            switch (whichHider) {
                case 1:
                    Seeker.hidedPlayerOne.moveable = false;
                    Seeker.hidedPlayerOne.NetTransform.Halt();
                    break;
                case 2:
                    Seeker.hidedPlayerTwo.moveable = false;
                    Seeker.hidedPlayerTwo.NetTransform.Halt();
                    break;
                case 3:
                    Seeker.hidedPlayerThree.moveable = false;
                    Seeker.hidedPlayerThree.NetTransform.Halt();
                    break;
            }
        }
        
        public static void captainSpecialVote(byte playerid, byte targetid) {
            if (!MeetingHud.Instance) return;
            PlayerControl target = Helpers.playerById(targetid);
            if (target == null) return;
            Captain.specialVoteTargetPlayerId = targetid;
            Captain.specialVoteTarget = target;
            Captain.usedSpecialVote = true;
            if (Captain.captain.PlayerId != playerid) return;
            captainAutoCastSpecialVote();
        }

        public static void captainAutoCastSpecialVote() {
            if (!MeetingHud.Instance) return;
            if (Captain.captain != PlayerControl.LocalPlayer) return;
            PlayerControl target = Helpers.playerById(Captain.specialVoteTargetPlayerId);
            if (target == null) return;
            MeetingHud.Instance.CmdCastVote(PlayerControl.LocalPlayer.PlayerId, target.PlayerId);
        }
        
        public static void mechanicFixLights() {
            SwitchSystem switchSystem = ShipStatus.Instance.Systems[SystemTypes.Electrical].Cast<SwitchSystem>();
            switchSystem.ActualSwitches = switchSystem.ExpectedSwitches;
        }

        public static void mechanicUsedRepair() {
            SoundManager.Instance.PlaySound(CustomMain.customAssets.mechanicWelderAction, false, 100f);
            if (Mechanic.timesUsedRepairs < Mechanic.numberOfRepairs) {
                Mechanic.timesUsedRepairs += 1;
                if (Mechanic.timesUsedRepairs == Mechanic.numberOfRepairs) {
                    Mechanic.usedRepair = true;
                }
            }
            Mechanic.mechanicRepairButtonText.text = $"{Mechanic.numberOfRepairs - Mechanic.timesUsedRepairs} / {Mechanic.numberOfRepairs}";
        }

        public static void sheriffKill(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Sheriff.sheriff.MurderPlayer(player);
                    return;
                }
            }
        }

        public static void timeTravelerShield() {
            TimeTraveler.shieldActive = true;
            HudManager.Instance.StartCoroutine(Effects.Lerp(TimeTraveler.shieldDuration, new Action<float>((p) => {
                if (p == 1f) TimeTraveler.shieldActive = false;
            })));
        }

        public static void timeTravelerRewindTime() {
            if (TimeTraveler.shieldActive == true) {
                TimeTraveler.usedShield = true;
            }
            TimeTraveler.shieldActive = false; // Shield is no longer active when rewinding
            if (TimeTraveler.timeTraveler != null && TimeTraveler.timeTraveler == PlayerControl.LocalPlayer) {
                resetTimeTravelerButton();
            }
            HudManager.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
            HudManager.Instance.FullScreen.enabled = true;
            HudManager.Instance.FullScreen.gameObject.SetActive(true);
            HudManager.Instance.StartCoroutine(Effects.Lerp(TimeTraveler.rewindTime / 2, new Action<float>((p) => {
                if (p == 1f) HudManager.Instance.FullScreen.enabled = false;
            })));

            SoundManager.Instance.PlaySound(CustomMain.customAssets.timeTravelerTimeReverseClip, false, 100f);

            if (TimeTraveler.timeTraveler == null || PlayerControl.LocalPlayer == TimeTraveler.timeTraveler) return; // TimeTraveler himself does not rewind

            TimeTraveler.isRewinding = true;

            if (MapBehaviour.Instance)
                MapBehaviour.Instance.Close();
            if (Minigame.Instance)
                Minigame.Instance.ForceClose();
            PlayerControl.LocalPlayer.moveable = false;
        }

        public static void timeTravelerRevive(byte playerId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == playerId) {
                    player.Revive();
                    var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == playerId);
                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == playerId).FirstOrDefault();
                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                    if (Modifiers.performer != null && player.PlayerId == Modifiers.performer.PlayerId) {
                        performerIsReported(1);
                    }
                    if (Vigilant.vigilantMira != null && player.PlayerId == Vigilant.vigilantMira.PlayerId) {
                        vigilantMiraRestoreDoorlogItem();
                    }
                    if (Hunter.hunter != null && player.PlayerId == Hunter.hunter.PlayerId) {
                        Hunter.resetHunted();
                    }
                    if (Janitor.janitor != null && Janitor.dragginBody) {
                        janitorResetValues();
                    }
                    if (Necromancer.necromancer != null && Necromancer.dragginBody) {
                        necromancerResetValues();
                    }
                    if (TaskMaster.taskMaster != null && player == TaskMaster.taskMaster && TaskMaster.clearedInitialTasks) {
                        setNewExtraTasks();
                    }
                    // Reset zoomed out ghosts
                    Helpers.toggleZoom(reset: true);
                }
        }

        public static void squireSetShielded(byte shieldedId) {
            Squire.usedShield = true;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == shieldedId)
                    Squire.shielded = player;
        }        
        
        public static void shieldedMurderAttempt() {
            if (Squire.shielded != null && Squire.shielded == PlayerControl.LocalPlayer && Squire.showAttemptToShielded || Squire.showAttemptToShielded && (PlayerControl.LocalPlayer.Data.Role.IsImpostor || Sheriff.sheriff != null && Sheriff.sheriff == PlayerControl.LocalPlayer || Renegade.renegade != null && Renegade.renegade == PlayerControl.LocalPlayer || Minion.minion != null && Minion.minion == PlayerControl.LocalPlayer || BountyHunter.bountyhunter != null && BountyHunter.bountyhunter == PlayerControl.LocalPlayer || Trapper.trapper != null && Trapper.trapper == PlayerControl.LocalPlayer || Yinyanger.yinyanger != null && Yinyanger.yinyanger == PlayerControl.LocalPlayer || Challenger.challenger != null && Challenger.challenger == PlayerControl.LocalPlayer || Ninja.ninja != null && Ninja.ninja == PlayerControl.LocalPlayer || Berserker.berserker != null && Berserker.berserker == PlayerControl.LocalPlayer || Yandere.yandere != null && Yandere.yandere == PlayerControl.LocalPlayer || Stranded.stranded != null && Stranded.stranded == PlayerControl.LocalPlayer || Monja.monja != null && Monja.monja == PlayerControl.LocalPlayer)) {
                SoundManager.Instance.PlaySound(CustomMain.customAssets.squireShieldClip, false, 100f);
                return;
            }
        }

        public static void cheaterCheat(byte playerId1, byte playerId2) {
            if (MeetingHud.Instance) {
                Cheater.playerId1 = playerId1;
                Cheater.playerId2 = playerId2;
                GameData.PlayerInfo playerById1 = GameData.Instance.GetPlayerById((byte)Cheater.playerId1);
                Cheater.cheatedP1 = playerById1.Object;
                GameData.PlayerInfo playerById2 = GameData.Instance.GetPlayerById((byte)Cheater.playerId2);
                Cheater.cheatedP2 = playerById2.Object;
                Cheater.usedCheat = true;
            }
        }

        public static void fortuneTellerReveal(byte targetId) {
            if (FortuneTeller.fortuneTeller == null) return;

            PlayerControl target = Helpers.playerById(targetId);

            FortuneTeller.revealedPlayers.Add(target);

            if (PlayerControl.LocalPlayer == FortuneTeller.fortuneTeller) {
                SoundManager.Instance.PlaySound(CustomMain.customAssets.fortuneTellerRevealClip, false, 100f);
            }

            if (PlayerControl.LocalPlayer == target && HudManager.Instance?.FullScreen != null) {
                RoleFortuneTellerInfo si = RoleFortuneTellerInfo.getFortuneTellerRoleInfoForPlayer(target); // Use RoleInfo of target here, because we need the isGood of the targets role
                bool showNotification = false;
                if (FortuneTeller.playersWithNotification == 0 && !si.isGood) showNotification = true;
                else if (FortuneTeller.playersWithNotification == 1 && si.isGood) showNotification = true;
                else if (FortuneTeller.playersWithNotification == 2) showNotification = true;
                else if (FortuneTeller.playersWithNotification == 3) showNotification = false;

                if (showNotification) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.fortuneTellerRevealClip, false, 100f);
                    HudManager.Instance.FullScreen.enabled = true;
                    HudManager.Instance.FullScreen.gameObject.SetActive(true);
                    HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) => {
                        var renderer = HudManager.Instance.FullScreen;
                        Color c = new Color(42f / 255f, 187f / 255f, 245f / 255f, 0f);
                        if (p < 0.5) {
                            if (renderer != null)
                                renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(p * 2 * 0.75f));
                        }
                        else {
                            if (renderer != null)
                                renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                        }
                    })));
                }
            }

            if (FortuneTeller.timesUsedFortune < FortuneTeller.numberOfFortunes) {
                FortuneTeller.timesUsedFortune += 1;
                if (FortuneTeller.timesUsedFortune == FortuneTeller.numberOfFortunes) {
                    FortuneTeller.usedFortune = true;
                }
            }
            FortuneTeller.fortuneTellerRevealButtonText.text = $"{FortuneTeller.numberOfFortunes - FortuneTeller.timesUsedFortune} / {FortuneTeller.numberOfFortunes}";
        }

        public static void hackerAbilityUses(byte value) {
            if (value == 0) {
                Hacker.chargesAdminTable--;
                Hacker.hackerAdminTableChargesText.text = $"{Hacker.chargesAdminTable} / {Hacker.toolsNumber}";
            }
            else if (value == 1) {
                Hacker.chargesVitals--;
                Hacker.hackerVitalsChargesText.text = $"{Hacker.chargesVitals} / {Hacker.toolsNumber}";
            }
            else {
                Hacker.rechargedTasks += Hacker.rechargeTasksNumber;
                if (Hacker.toolsNumber > Hacker.chargesVitals) Hacker.chargesVitals++;
                if (Hacker.toolsNumber > Hacker.chargesAdminTable) Hacker.chargesAdminTable++;
            }
        }
        
        public static void sleuthUsedLocate(byte targetId) {
            Sleuth.usedLocate = true;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == targetId)
                    Sleuth.located = player;
        }

        public static void finkHawkEye() {
            Fink.finkTimer = Fink.duration;
            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor || Renegade.renegade != null && PlayerControl.LocalPlayer == Renegade.renegade || Minion.minion != null && PlayerControl.LocalPlayer == Minion.minion) {
                if (MapBehaviour.Instance) {
                    MapBehaviour.Instance.Close();
                }
                new CustomMessage(Language.statusRolesTexts[10], Fink.duration, -1, -1f, 13);
            }
        }

        public static void sealVent(int ventId) {
            Vent vent = ShipStatus.Instance.AllVents.FirstOrDefault((x) => x != null && x.Id == ventId);
            if (vent == null) return;

            Welder.remainingWelds -= 1;
            MapOptions.ventsToSeal.Add(vent);
            Welder.welderButtonText.text = $"{Welder.remainingWelds} / {Welder.totalWelds}";

            if (PlayerControl.LocalPlayer == Welder.welder) {
                // Welder vents seal sprite    
                if (GameOptionsManager.Instance.currentGameOptions.MapId != 2) {
                    PowerTools.SpriteAnim animator = vent.GetComponent<PowerTools.SpriteAnim>();
                    animator?.Stop();
                    vent.EnterVentAnim = vent.ExitVentAnim = null;
                    vent.myRend.sprite = Welder.getAnimatedVentSealedSprite();
                }
                else {
                    vent.myRend.sprite = Welder.getStaticVentSealedSprite();
                }
                vent.myRend.color = new Color(1, 1, 1, 0.5f);
            }
        }

        public static void spiritualistRevive(byte playerId, byte reviverId) {

            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (Spiritualist.spiritualist != null && Spiritualist.spiritualist.PlayerId == reviverId && PlayerControl.LocalPlayer == Spiritualist.spiritualist) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.spiritualistRevive, false, 100f);
                }
                else if (Necromancer.necromancer != null && Necromancer.necromancer.PlayerId == reviverId && PlayerControl.LocalPlayer == Necromancer.necromancer) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.spiritualistRevive, false, 100f);
                }

                if (player.PlayerId == playerId) {
                    if (Spiritualist.spiritualist != null && Spiritualist.spiritualist.PlayerId == reviverId) {
                        Spiritualist.revivedPlayer = Helpers.playerById(playerId);
                        Spiritualist.usedRevive = true;
                    }
                    if (Necromancer.necromancer != null && Necromancer.necromancer.PlayerId == reviverId) {
                        Necromancer.revivedPlayer = Helpers.playerById(playerId);
                    }
                    var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == playerId);
                    teleportSpiritualistNecromancerBodies(player, body);
                    
                    // Check roles that needs reset on revive
                    if (Modifiers.performer != null && player == Modifiers.performer)
                        performerIsReported(1);
                    if (Vigilant.vigilantMira != null && player == Vigilant.vigilantMira)
                        vigilantMiraRestoreDoorlogItem();
                    if (Hunter.hunter != null && player == Hunter.hunter)
                        Hunter.resetHunted();
                    if (Janitor.janitor != null && Janitor.dragginBody) {
                        janitorResetValues();
                    }
                    if (Necromancer.necromancer != null && Necromancer.dragginBody) {
                        necromancerResetValues();
                    }
                    if (TaskMaster.taskMaster != null && player == TaskMaster.taskMaster && TaskMaster.clearedInitialTasks) {
                        setNewExtraTasks();
                    }

                    // Check lovers, revive lover2 or lover1 too
                    if (player == Modifiers.lover1) {
                        PlayerControl lovertwo = Modifiers.lover2;
                        var bodytwo = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == lovertwo.PlayerId);
                        teleportSpiritualistNecromancerBodies(lovertwo, bodytwo);                        
                    }
                    else if (player == Modifiers.lover2) {
                        PlayerControl loverone = Modifiers.lover1;
                        var bodyone = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == loverone.PlayerId);
                        teleportSpiritualistNecromancerBodies(loverone, bodyone);                                               
                    }

                    // Check bountyhunter kill and revive bountyhunter
                    if (player == BountyHunter.hasToKill) {
                        PlayerControl bountyhunter = BountyHunter.bountyhunter;
                        var bodybountyhunter = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == bountyhunter.PlayerId);
                        teleportSpiritualistNecromancerBodies(bountyhunter, bodybountyhunter);                        
                    }

                    // Check bountyhunter and try reviving their target
                    if (player == BountyHunter.bountyhunter) {
                        if (BountyHunter.hasToKill != null && BountyHunter.hasToKill.Data.IsDead) {
                            PlayerControl bountytarget = BountyHunter.hasToKill;
                            var bodybountytarget = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == bountytarget.PlayerId);
                            teleportSpiritualistNecromancerBodies(bountytarget, bodybountytarget);                        
                        }
                    }

                    // Check exiler target and revive exiler
                    if (player == Exiler.target) {
                        PlayerControl exilerPlayer = Exiler.exiler;
                        var bodyExilerPlayer = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == exilerPlayer.PlayerId);
                        teleportSpiritualistNecromancerBodies(exilerPlayer, bodyExilerPlayer);                       
                    }
                    
                    // Check exiler and try reviving their target
                    if (player == Exiler.exiler) {
                        if (Exiler.target != null && Exiler.target.Data.IsDead) {
                            PlayerControl exilerTarget = Exiler.target;
                            var bodyexilerTarget = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == exilerTarget.PlayerId);
                            teleportSpiritualistNecromancerBodies(exilerTarget, bodyexilerTarget);                  
                        }
                    }
                }
            }

            // Reset zoomed out ghosts
            Helpers.toggleZoom(reset: true);

            if (Spiritualist.spiritualist != null && Spiritualist.spiritualist.PlayerId == reviverId) {
                Spiritualist.preventReport = true;
                HudManager.Instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => { // Delayed action
                    if (p == 1f) {
                        murderSpiritualistIfReportWhileReviving();
                    }
                }))); 
                HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) => { // Delayed action
                    if (p == 1f) {
                        removeBody(Spiritualist.spiritualist.PlayerId);
                        Spiritualist.preventReport = false;
                    }
                })));
            }
        }

        public static void spiritualistPinkScreen(byte playerId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (PlayerControl.LocalPlayer.Data.Role.IsImpostor || Renegade.renegade != null && Renegade.renegade == PlayerControl.LocalPlayer || Minion.minion != null && Minion.minion == PlayerControl.LocalPlayer || BountyHunter.bountyhunter != null && BountyHunter.bountyhunter == PlayerControl.LocalPlayer || Trapper.trapper != null && Trapper.trapper == PlayerControl.LocalPlayer || Yinyanger.yinyanger != null && Yinyanger.yinyanger == PlayerControl.LocalPlayer || Challenger.challenger != null && Challenger.challenger == PlayerControl.LocalPlayer || Ninja.ninja != null && Ninja.ninja == PlayerControl.LocalPlayer || Berserker.berserker != null && Berserker.berserker == PlayerControl.LocalPlayer || Yandere.yandere != null && Yandere.yandere == PlayerControl.LocalPlayer || Stranded.stranded != null && Stranded.stranded == PlayerControl.LocalPlayer || Monja.monja != null && Monja.monja == PlayerControl.LocalPlayer) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.spiritualistRevive, false, 100f);
                }
                if (player.PlayerId == playerId && playerId == PlayerControl.LocalPlayer.PlayerId) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.spiritualistRevive, false, 100f);
                    HudManager.Instance.FullScreen.enabled = true;
                    HudManager.Instance.FullScreen.gameObject.SetActive(true);
                    HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) => {
                        var renderer = HudManager.Instance.FullScreen;
                        Color c = new Color(255f / 255f, 197f / 255f, 255f / 255f, 0f);
                        if (p < 0.5) {
                            if (renderer != null)
                                renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(p * 2 * 0.75f));
                        }
                        else {
                            if (renderer != null)
                                renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                        }
                    })));
                }
            }
        }

        public static void sendSpiritualistIsReviving() {
            Spiritualist.canRevive = true;
            Spiritualist.isReviving = true;
        }

        public static void murderSpiritualistIfReportWhileReviving() {
            Spiritualist.spiritualist.MurderPlayer(Spiritualist.spiritualist);
            resetSpiritualistReviveValues();
        }

        public static void resetSpiritualistReviveValues() {
            Spiritualist.canRevive = false;
            Spiritualist.isReviving = false;
            resetSpiritualistReviveButton();
        }

        public static void teleportSpiritualistNecromancerBodies(PlayerControl player, DeadBody body) {
            player.Revive();
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                if (player.transform.position.y > 0) {
                    player.transform.position = new Vector3(5.5f, 31.5f, -5);
                }
                else {
                    player.transform.position = new Vector3(-4.75f, -33.25f, -5);
                }
            }
            else {
                player.transform.position = body.transform.position;
            }
            DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == player.PlayerId).FirstOrDefault();
            if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
            if (body != null) UnityEngine.Object.Destroy(body.gameObject);
            spiritualistPinkScreen(player.PlayerId);
        }
        
        public static void cowardUsedCall() {
            if (Coward.timesUsedCalls < Coward.numberOfCalls) {
                Coward.timesUsedCalls += 1;
                if (Coward.timesUsedCalls == Coward.numberOfCalls) {
                    Coward.usedCalls = true;
                }
            }
            Coward.cowardCallButtonText.text = $"{Coward.numberOfCalls - Coward.timesUsedCalls} / {Coward.numberOfCalls}";
        }

        public static void placeCamera(byte[] buff) {
            var referenceCamera = UnityEngine.Object.FindObjectOfType<SurvCamera>();
            if (referenceCamera == null) return; // Mira HQ

            Vigilant.placedCameras++;

            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));

            var camera = UnityEngine.Object.Instantiate<SurvCamera>(referenceCamera);
            camera.transform.position = new Vector3(position.x, position.y, referenceCamera.transform.position.z - 1f);
            camera.CamName = $"Security Camera {Vigilant.placedCameras}";
            camera.Offset = new Vector3(0f, 0f, camera.Offset.z);
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 2 || GameOptionsManager.Instance.currentGameOptions.MapId == 4) camera.transform.localRotation = new Quaternion(0, 0, 1, 1); // Polus and Airship 

            if (PlayerControl.LocalPlayer == Vigilant.vigilant) {
                camera.gameObject.SetActive(true);
                camera.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }
            else {
                camera.gameObject.SetActive(false);
            }
            MapOptions.camerasToAdd.Add(camera);

            Vigilant.remainingCameras -= 1;
            Vigilant.vigilantButtonCameraText.text = $"{Vigilant.remainingCameras} / {Vigilant.totalCameras}";
        }

        public static void vigilantAbilityUses(byte value) {
            if (value == 0) {
                Vigilant.charges--;
                Vigilant.vigilantButtonCameraUsesText.text = $"{Vigilant.charges} / {Vigilant.maxCharges}";
            }
            else {
                Vigilant.rechargedTasks += Vigilant.rechargeTasksNumber;
                if (Vigilant.maxCharges > Vigilant.charges) Vigilant.charges++;
            }
        }

        public static void vigilantMiraRestoreDoorlogItem() {
            // Vigilant restore doorlog item when revive
            if (Vigilant.vigilantMira != null && PlayerControl.LocalPlayer == Vigilant.vigilantMira) {
                Vigilant.doorLog.SetActive(true);
            }
        }        

        public static void hunterUsedHunted(byte targetId) {
            Hunter.usedHunted = true;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    Hunter.hunted = player;
                }
            }
            Hunter.targetButtonText.text = $"{Hunter.hunted.name}";
        }

        public static void setJinxed(byte playerId, byte value) {
            PlayerControl target = Helpers.playerById(playerId);
            if (target == null) return;
            Jinx.jinxedList.RemoveAll(x => x.PlayerId == playerId);
            if (value > 0) {
                Jinx.jinxedList.Add(target);
                Jinx.jinxs++;
                Jinx.jinxButtonJinxsText.text = $"{Jinx.jinxNumber - Jinx.jinxs} / {Jinx.jinxNumber}";
            }
        }             

        public static void batFrequency() {
            if (Bat.bat == null) return;

            Bat.frequencyTimer = Bat.duration;
        }

        public static void necromancerResetValues() {
            // Restore necromancer values when rewind time
            if (Necromancer.necromancer != null && Necromancer.dragginBody) {
                Necromancer.dragginBody = false;
                Necromancer.bodyId = 0;
            }
        }        

        public static void placeEngineerTrap(byte trapType, byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            Engineer.currentTrapNumber += 1;
            new EngineerTrap(Engineer.trapsDuration, position, trapType);
            if (!Engineer.savedOldSpeed) {
                if (PlayerControl.LocalPlayer) {
                    Engineer.oldSpeed = PlayerControl.LocalPlayer.MyPhysics.Speed;
                    Engineer.savedOldSpeed = true;
                }
            }
        }

        public static void activateEngineerTrap(byte targetId, byte trapType) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    switch (trapType) {
                        case 1:
                            player.MyPhysics.Speed *= Engineer.accelTrapIncrease;
                            HudManager.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>((p) => {
                                if (p == 1f) {
                                    player.MyPhysics.Speed = Engineer.oldSpeed;
                                    if (player == PlayerControl.LocalPlayer) {
                                        foreach (EngineerTrap trap in EngineerTrap.engineerTraps) {
                                            if (trap.myTrapType != 3) {
                                                trap.isActive = true;
                                            }
                                        }
                                    }
                                }
                            })));
                            break;
                        case 2:
                            player.MyPhysics.Speed *= Engineer.decelTrapDecrease;
                            HudManager.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>((p) => {
                                if (p == 1f) {
                                    player.MyPhysics.Speed = Engineer.oldSpeed;
                                    if (player == PlayerControl.LocalPlayer) {
                                        foreach (EngineerTrap trap in EngineerTrap.engineerTraps) {
                                            if (trap.myTrapType != 3) {
                                                trap.isActive = true;
                                            }
                                        }
                                    }
                                }
                            })));
                            break;
                        case 3:
                            if (Engineer.engineer != null && !Engineer.engineer.Data.IsDead && PlayerControl.LocalPlayer == Engineer.engineer) {

                                Arrow arrow = new Arrow(Palette.PlayerColors[player.CurrentOutfit.ColorId]);
                                arrow.arrow.SetActive(true);
                                arrow.Update(player.transform.position);

                                HudManager.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>((p) => {
                                    arrow.Update(player.transform.position);
                                    if (p > 0.8f) {
                                        arrow.image.color = new Color(arrow.image.color.r, arrow.image.color.g, arrow.image.color.b, (1f - p) * 5f);
                                    }
                                    if (p == 1f) {
                                        UnityEngine.Object.Destroy(arrow.arrow);
                                    }
                                })));

                            }
                            break;
                    }
                }
            }
        }

        public static void setNewExtraTasks() {
            byte[] taskTypeIds = TasksHandler.GetTaskMasterTasks(TaskMaster.taskMaster);
            taskMasterSetExTasks(TaskMaster.taskMaster.PlayerId, byte.MaxValue, taskTypeIds);
        }

        public static void taskMasterSetExTasks(byte playerId, byte oldTaskMasterPlayerId, byte[] taskTypeIds) {
            PlayerControl oldTaskMasterPlayer = Helpers.playerById(oldTaskMasterPlayerId);
            if (oldTaskMasterPlayer != null) {
                oldTaskMasterPlayer.clearAllTasks();
                TaskMaster.oldTaskMasterPlayerId = oldTaskMasterPlayerId;
            }

            GameData.PlayerInfo player = GameData.Instance.GetPlayerById(playerId);
            if (player == null)
                return;

            if (taskTypeIds != null && taskTypeIds.Length > 0) {
                player.Object.clearAllTasks();
                player.Tasks = new Il2CppSystem.Collections.Generic.List<GameData.TaskInfo>(taskTypeIds.Length);
                for (int i = 0; i < taskTypeIds.Length; i++) {
                    player.Tasks.Add(new GameData.TaskInfo(taskTypeIds[i], (uint)i));
                    player.Tasks[i].Id = (uint)i;
                }
                for (int i = 0; i < player.Tasks.Count; i++) {
                    GameData.TaskInfo taskInfo = player.Tasks[i];
                    NormalPlayerTask normalPlayerTask = UnityEngine.Object.Instantiate(ShipStatus.Instance.GetTaskById(taskInfo.TypeId), player.Object.transform);
                    normalPlayerTask.Id = taskInfo.Id;
                    normalPlayerTask.Owner = player.Object;
                    normalPlayerTask.Initialize();
                    player.Object.myTasks.Add(normalPlayerTask);
                }
                TaskMaster.clearedInitialTasks = true;
            }
            else {
                TaskMaster.clearedInitialTasks = false;
            }
        }

        public static void taskMasterTriggerCrewWin() {
            if (TaskMaster.taskMaster == null) return;
            TaskMaster.triggerTaskMasterCrewWin = true;
        }

        public static void taskMasterActivateSpeed() {
            TaskMaster.taskTimer = TaskMaster.duration;
        }

        public static void jailedSetJailed(byte jailedId) {
            Jailer.usedJail = true;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == jailedId) {
                    Jailer.jailedPlayer = player;
                }
            }
            Jailer.jailButtonText.text = $"{Jailer.jailedPlayer.name}";
        }
        public static void prisonPlayer(byte prisonerId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == prisonerId) {
                    if (PlayerControl.LocalPlayer == player) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jailerJail, false, 100f);
                    }
                    if (Ninja.ninja != null && player.PlayerId == Ninja.ninja.PlayerId) {
                        Ninja.markedTarget = null;
                    }
                    var oldPos = player.transform.position;
                    Jailer.prisonPlayer = player;
                    Jailer.jailedPlayer = null;
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        // Skeld
                        case 0:
                            if (activatedSensei) {
                                player.transform.position = new Vector3(-12f, 7.15f, player.transform.position.z);
                            }
                            else {
                                player.transform.position = new Vector3(-10.2f, 3.6f, player.transform.position.z);
                            }
                            break;
                        // MiraHQ
                        case 1:
                            player.transform.position = new Vector3(1.8f, 1.25f, player.transform.position.z);
                            break;
                        // Polus
                        case 2:
                            player.transform.position = new Vector3(8.25f, -5f, player.transform.position.z);
                            break;
                        // Dleks
                        case 3:
                            player.transform.position = new Vector3(10.2f, 3.6f, player.transform.position.z);
                            break;
                        // Airship
                        case 4:
                            player.transform.position = new Vector3(-18.5f, 3.5f, player.transform.position.z);
                            break;
                        // Submerged
                        case 5:
                            if (player.transform.position.y > 0) {
                                player.transform.position = new Vector3(-15.25f, 28.4f, player.transform.position.z);
                            }
                            else {
                                player.transform.position = new Vector3(-1.15f, -21f, player.transform.position.z);
                            }
                            break;
                    }
                    HudManager.Instance.StartCoroutine(Effects.Lerp(Jailer.prisonDuration, new Action<float>((p) => {

                        if (p == 1f && Jailer.prisonPlayer != null && !Challenger.isDueling && !Seeker.isMinigaming) {
                            if (MeetingHud.Instance == null && !Jailer.prisonPlayer.Data.IsDead) {
                                Jailer.prisonPlayer.transform.position = oldPos;
                            }
                            Jailer.prisonPlayer = null;
                            Jailer.usedJail = false;
                            jailerJailButton.Timer = jailerJailButton.MaxTimer;
                            Jailer.jailButtonText.text = $" ";
                        }

                    })));
                }
            }
        }

        public static void performerIsReported(byte check) {
            if (check == 0) {
                Modifiers.performerReported = true;
            }
            else {
                Modifiers.performerReported = false;
            }
            SoundManager.Instance.StopSound(CustomMain.customAssets.performerMusic);
        }
        
        public static void changeMusic(byte whichmusic) {
            SoundManager.Instance.StopSound(CustomMain.customAssets.bombermanBombMusic);
            SoundManager.Instance.StopSound(CustomMain.customAssets.challengerDuelMusic);
            SoundManager.Instance.StopSound(CustomMain.customAssets.seekerMinigameMusic);
            SoundManager.Instance.StopSound(CustomMain.customAssets.monjaAwakeMusic);
            if (MapOptions.activateMusic && howmanygamemodesareon != 1) {
                int alivePlayers = 0;
                int totalPlayers = 0;
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (!player.Data.IsDead) {
                        alivePlayers += 1;
                    }
                    totalPlayers += 1;
                }

                // Stop all background music
                SoundManager.Instance.StopSound(CustomMain.customAssets.tasksCalmMusic);
                SoundManager.Instance.StopSound(CustomMain.customAssets.tasksCoreMusic);
                SoundManager.Instance.StopSound(CustomMain.customAssets.tasksFinalMusic);
                SoundManager.Instance.StopSound(CustomMain.customAssets.meetingCalmMusic);
                SoundManager.Instance.StopSound(CustomMain.customAssets.meetingCoreMusic);
                SoundManager.Instance.StopSound(CustomMain.customAssets.meetingFinalMusic);

                // Select which music moment
                switch (whichmusic) {
                    case 1:
                        // Meeting music                        
                        if (alivePlayers >= MathF.Round(totalPlayers / 1.25f)) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.meetingCalmMusic, true, 5f);
                        }
                        else if (alivePlayers >= MathF.Round(totalPlayers / 1.5f) && alivePlayers < MathF.Round(totalPlayers / 1.25f)) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.meetingCoreMusic, true, 5f);
                        }
                        else {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.meetingFinalMusic, true, 5f);
                        }
                        break;
                    case 2:
                        // Tasks music
                        if (alivePlayers >= MathF.Round(totalPlayers / 1.25f)) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.tasksCalmMusic, true, 5f);
                        }
                        else if (alivePlayers >= MathF.Round(totalPlayers / 1.5f) && alivePlayers < MathF.Round(totalPlayers / 1.25f)) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.tasksCoreMusic, true, 5f);
                        }
                        else {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.tasksFinalMusic, true, 5f);
                        }
                        break;
                    case 3:
                        // Neutrals win
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.winNeutralsMusic, false, 5f);
                        break;
                    case 4:
                        // Rebels win
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.winRebelsMusic, false, 5f);
                        break;
                    case 5:
                        //Crewmates win
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.winCrewmatesMusic, false, 5f);
                        break;
                    case 6:
                        //Impostor win
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.winImpostorsMusic, false, 5f);
                        break;
                    case 7:
                        //Music from outside musicbundle (bomb, duel, performer, gamemodes)

                        break;
                }

            }
        }

        public static void whoWasI(byte playerId, string roleName) {
            whoAmIButton.Timer = 1f;
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == playerId) {
                    GameObject roleToHide = GameObject.Find(roleName);
                    roleToHide.transform.position = new Vector3(100, 100, 0);
                    switch (roleName) {
                        // Crewmates
                        case "captainRole":
                            Captain.captain = player;
                            break;
                        case "mechanicRole":
                            Mechanic.mechanic = player;
                            break;
                        case "sheriffRole":
                            Sheriff.sheriff = player;
                            break;
                        case "detectiveRole":
                            Detective.detective = player;
                            break;
                        case "forensicRole":
                            Forensic.forensic = player;
                            break;
                        case "timetravelerRole":
                            TimeTraveler.timeTraveler = player;
                            if (player == PlayerControl.LocalPlayer) {
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
                            break;
                        case "squireRole":
                            Squire.squire = player;
                            break;
                        case "cheaterRole":
                            Cheater.cheater = player;
                            break;
                        case "fortunetellerRole":
                            FortuneTeller.fortuneTeller = player;
                            break;
                        case "hackerRole":
                            Hacker.hacker = player;
                            break;
                        case "sleuthRole":
                            Sleuth.sleuth = player;
                            break;
                        case "finkRole":
                            Fink.fink = player;
                            break;
                        case "kidRole":
                            Kid.kid = player;
                            break;
                        case "welderRole":
                            Welder.welder = player;
                            break;
                        case "spiritualistRole":
                            Spiritualist.spiritualist = player;
                            break;
                        case "vigilantRole":
                            if (GameOptionsManager.Instance.currentGameOptions.MapId == 1 && player == PlayerControl.LocalPlayer) {
                                Vigilant.vigilantMira = player;
                                GameObject vigilantDoorLog = GameObject.Find("SurvLogConsole");
                                Vigilant.doorLog = GameObject.Instantiate(vigilantDoorLog, Vigilant.vigilantMira.transform);
                                Vigilant.doorLog.name = "VigilantDoorLog";
                                Vigilant.doorLog.layer = 8; // Assign player layer to ignore collisions
                                Vigilant.doorLog.GetComponent<SpriteRenderer>().enabled = false;
                                Vigilant.doorLog.transform.localPosition = new Vector2(0, -0.5f);
                            } else {
                                Vigilant.vigilant = player;
                            }
                            break;
                        case "hunterRole":
                            Hunter.hunter = player;
                            break;
                        case "jinxRole":
                            Jinx.jinx = player;
                            break;
                        case "cowardRole":
                            Coward.coward = player;
                            break;
                        case "batRole":
                            Bat.bat = player;
                            break;
                        case "necromancerRole":
                            Necromancer.necromancer = player;
                            break;
                        case "engineerRole":
                            Engineer.engineer = player;
                            break;
                        case "shyRole":
                            Shy.shy = player;
                            break;
                        case "taskmasterRole":
                            TaskMaster.taskMaster = player;
                            break;
                        case "jailerRole":
                            Jailer.jailer = player;
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
                            break;

                        // Impostors
                        case "mimicRole":
                            Mimic.mimic = player;
                            break;
                        case "painterRole":
                            Painter.painter = player;
                            break;
                        case "demonRole":
                            Demon.demon = player;
                            break;
                        case "janitorRole":
                            Janitor.janitor = player;
                            break;
                        case "illusionistRole":
                            Illusionist.illusionist = player;
                            break;
                        case "manipulatorRole":
                            Manipulator.manipulator = player;
                            break;
                        case "bombermanRole":
                            Bomberman.bomberman = player;
                            break;
                        case "chameleonRole":
                            Chameleon.chameleon = player;
                            if (PlayerControl.LocalPlayer == player && GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                                GameObject vent = GameObject.Find("LowerCentralVent");
                                vent.GetComponent<BoxCollider2D>().enabled = false;
                            }
                            break;
                        case "gamblerRole":
                            Gambler.gambler = player;
                            break;
                        case "sorcererRole":
                            Sorcerer.sorcerer = player;
                            break;
                        case "medusaRole":
                            Medusa.medusa = player;
                            break;
                        case "hypnotistRole":
                            Hypnotist.hypnotist = player;
                            if (PlayerControl.LocalPlayer == player) {
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
                            break;
                        case "archerRole":
                            Archer.archer = player;
                            break;
                        case "plumberRole":
                            Plumber.plumber = player;
                            break;
                        case "librarianRole":
                            Librarian.librarian = player;
                            break;

                        // Neutrals:
                        case "jokerRole":
                            Joker.joker = player;
                            hideWhoWasIBoxesForEveryone(true);
                            break;
                        case "pyromaniacRole":
                            Pyromaniac.pyromaniac = player;
                            if (PlayerControl.LocalPlayer == player) {
                                int visibleCounter = 0;
                                Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.parent.localPosition.x, HudManager.Instance.UseButton.transform.parent.localPosition.y, HudManager.Instance.UseButton.transform.parent.localPosition.z);
                                bottomLeft += new Vector3(-0.25f, -0.25f, 0);
                                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                                    if (!MapOptions.playerIcons.ContainsKey(p.PlayerId)) continue;
                                    if (p.Data.IsDead || p.Data.Disconnected || p == Pyromaniac.pyromaniac) {
                                        MapOptions.playerIcons[p.PlayerId].gameObject.SetActive(false);
                                    }
                                    else {
                                        MapOptions.playerIcons[p.PlayerId].gameObject.SetActive(true);
                                        MapOptions.playerIcons[p.PlayerId].setSemiTransparent(true);
                                        MapOptions.playerIcons[p.PlayerId].transform.localPosition = bottomLeft + Vector3.right * visibleCounter * 0.35f;
                                        MapOptions.playerIcons[p.PlayerId].transform.localScale = Vector3.one * 0.2f;
                                        visibleCounter++; 
                                    }
                                }
                            }                            
                            hideWhoWasIBoxesForEveryone(true);
                            break;
                        case "treasurehunterRole":
                            TreasureHunter.treasureHunter = player;
                            hideWhoWasIBoxesForEveryone(true);
                            break;
                        case "devourerRole":
                            Devourer.devourer = player;
                            hideWhoWasIBoxesForEveryone(true);
                            break;
                        case "poisonerRole":
                            Poisoner.poisoner = player;
                            if (PlayerControl.LocalPlayer == player) {
                                int visibleCounter = 0;
                                Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.parent.localPosition.x, HudManager.Instance.UseButton.transform.parent.localPosition.y, HudManager.Instance.UseButton.transform.parent.localPosition.z);
                                bottomLeft += new Vector3(-0.25f, -0.25f, 0);
                                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                                    if (!MapOptions.playerIcons.ContainsKey(p.PlayerId)) continue;
                                    if (p.Data.IsDead || p.Data.Disconnected || p == Poisoner.poisoner) {
                                        MapOptions.playerIcons[p.PlayerId].gameObject.SetActive(false);
                                    }
                                    else {
                                        MapOptions.playerIcons[p.PlayerId].gameObject.SetActive(true);
                                        MapOptions.playerIcons[p.PlayerId].setSemiTransparent(true);
                                        MapOptions.playerIcons[p.PlayerId].transform.localPosition = bottomLeft + Vector3.right * visibleCounter * 0.35f;
                                        MapOptions.playerIcons[p.PlayerId].transform.localScale = Vector3.one * 0.2f; visibleCounter++;
                                    }
                                }
                            }
                            hideWhoWasIBoxesForEveryone(true);
                            break;
                        case "puppeteerRole":
                            Puppeteer.puppeteer = player;
                            hideWhoWasIBoxesForEveryone(true);
                            break;
                        case "exilerRole":
                            Exiler.exiler = player;
                            hideWhoWasIBoxesForEveryone(true);
                            break;
                        case "seekerRole":
                            Seeker.seeker = player;
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
                            hideWhoWasIBoxesForEveryone(true);
                            break;
                            
                            // Rebels:
                        case "renegadeRole":
                            Renegade.renegade = player;
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                        case "trapperRole":
                            Trapper.trapper = player;
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                        case "yinyangerRole":
                            Yinyanger.yinyanger = player;
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                        case "challengerRole":
                            Challenger.challenger = player;
                            GameObject duelArena = GameObject.Instantiate(CustomMain.customAssets.challengerDuelArena, PlayerControl.LocalPlayer.transform.parent);
                            duelArena.name = "duelArena";
                            duelArena.transform.position = new Vector3(40, 0f, 1f);
                            if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) { // Create another duel arena on submerged lower floor
                                GameObject lowerduelArena = GameObject.Instantiate(CustomMain.customAssets.challengerDuelArena, PlayerControl.LocalPlayer.transform.parent);
                                lowerduelArena.name = "lowerduelArena";
                                lowerduelArena.transform.position = new Vector3(40, -48.119f, 1f);
                            }
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                        case "ninjaRole":
                            Ninja.ninja = player;
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                        case "berserkerRole":
                            Berserker.berserker = player;
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                        case "yandereRole":
                            Yandere.yandere = player;
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                        case "strandedRole":
                            Stranded.stranded = player;
                            if (player == PlayerControl.LocalPlayer) {
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
                            }
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                        case "monjaRole":
                            Monja.monja = player;
                            GameObject monjaRitual = GameObject.Instantiate(CustomMain.customAssets.monjaRitual, PlayerControl.LocalPlayer.transform.parent);
                            Monja.ritualObject = monjaRitual;
                            Monja.ritualObject.layer = 9;
                            GameObject monjaSprite = GameObject.Instantiate(CustomMain.customAssets.monjaSprite, PlayerControl.LocalPlayer.transform.parent);
                            Monja.monjaSprite = monjaSprite;
                            Monja.monjaSprite.transform.parent = Monja.monja.transform;
                            Monja.monjaSprite.transform.position = new Vector3(0, 0, -1);
                            Monja.monjaSprite.transform.localPosition = new Vector3(0, 0, -1);
                            Monja.monjaSprite.SetActive(false);
                            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                case 0:
                                    if (activatedSensei) {
                                        Monja.ritualObject.transform.position = new Vector3(3f, 2.5f, 0.5f);
                                    }
                                    else {
                                        Monja.ritualObject.transform.position = new Vector3(-0.9f, 5.25f, 0.5f);
                                    }
                                    break;
                                case 1:
                                    Monja.ritualObject.transform.position = new Vector3(17.85f, 11.5f, 0.5f);
                                    break;
                                case 2:
                                    Monja.ritualObject.transform.position = new Vector3(13.75f, -9.5f, 0.5f);
                                    break;
                                case 3:
                                    Monja.ritualObject.transform.position = new Vector3(0.9f, 5.25f, 0.5f);
                                    break;
                                case 4:
                                    Monja.ritualObject.transform.position = new Vector3(10.75f, -0.25f, 0.5f);
                                    break;
                                case 5:
                                    Monja.ritualObject.transform.position = new Vector3(-6.35f, 14f, -0.5f);
                                    break;
                            }
                            GameObject keyitem01 = GameObject.Instantiate(CustomMain.customAssets.keyItem01, PlayerControl.LocalPlayer.transform.parent);
                            keyitem01.transform.position = Monja.itemListPositions[0];
                            keyitem01.name = "item01";
                            Monja.item01 = keyitem01;
                            GameObject keyitem02 = GameObject.Instantiate(CustomMain.customAssets.keyItem02, PlayerControl.LocalPlayer.transform.parent);
                            keyitem02.transform.position = Monja.itemListPositions[1];
                            keyitem02.name = "item02";
                            Monja.item02 = keyitem02;
                            GameObject keyitem03 = GameObject.Instantiate(CustomMain.customAssets.keyItem03, PlayerControl.LocalPlayer.transform.parent);
                            keyitem03.transform.position = Monja.itemListPositions[2];
                            keyitem03.name = "item03";
                            Monja.item03 = keyitem03;
                            GameObject keyitem04 = GameObject.Instantiate(CustomMain.customAssets.keyItem04, PlayerControl.LocalPlayer.transform.parent);
                            keyitem04.transform.position = Monja.itemListPositions[3];
                            keyitem04.name = "item04";
                            Monja.item04 = keyitem04;
                            GameObject keyitem05 = GameObject.Instantiate(CustomMain.customAssets.keyItem05, PlayerControl.LocalPlayer.transform.parent);
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
                            hideWhoWasIBoxesForEveryone(false);
                            break;
                    }
                    if (PlayerControl.LocalPlayer == player) {
                        if (player.Data.Role.IsImpostor) {
                            foreach (GameObject item in whoAmIModeImpostorItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                        }
                        else {
                            foreach (GameObject item in whoAmIModeCrewItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                            foreach (GameObject item in whoAmIModeNeutralsItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                            foreach (GameObject item in whoAmIModeRebelsItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                        }
                    }
                }
            }
        }

        public static void hideWhoWasIBoxesForEveryone(bool isNeutral) {
            if (isNeutral) {
                foreach (GameObject item in whoAmIModeNeutralsItems) {
                    item.transform.position = new Vector3(100, 100, 0);
                }
            } else {
                foreach (GameObject item in whoAmIModeRebelsItems) {
                    item.transform.position = new Vector3(100, 100, 0);
                }               
            }
        }

        public static void capturetheFlagKills(byte targetId, int whichplayer) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    switch (whichplayer) {
                        case 1:
                            CaptureTheFlag.redplayer01.MurderPlayer(player);
                            break;
                        case 2:
                            CaptureTheFlag.redplayer02.MurderPlayer(player);
                            break;
                        case 3:
                            CaptureTheFlag.redplayer03.MurderPlayer(player);
                            break;
                        case 4:
                            CaptureTheFlag.redplayer04.MurderPlayer(player);
                            break;
                        case 5:
                            CaptureTheFlag.redplayer05.MurderPlayer(player);
                            break;
                        case 6:
                            CaptureTheFlag.redplayer06.MurderPlayer(player);
                            break;
                        case 7:
                            CaptureTheFlag.redplayer07.MurderPlayer(player);
                            break;
                        case 9:
                            CaptureTheFlag.blueplayer01.MurderPlayer(player);
                            break;
                        case 10:
                            CaptureTheFlag.blueplayer02.MurderPlayer(player);
                            break;
                        case 11:
                            CaptureTheFlag.blueplayer03.MurderPlayer(player);
                            break;
                        case 12:
                            CaptureTheFlag.blueplayer04.MurderPlayer(player);
                            break;
                        case 13:
                            CaptureTheFlag.blueplayer05.MurderPlayer(player);
                            break;
                        case 14:
                            CaptureTheFlag.blueplayer06.MurderPlayer(player);
                            break;
                        case 15:
                            CaptureTheFlag.blueplayer07.MurderPlayer(player);
                            break;
                        case 16:
                            if (CaptureTheFlag.redPlayerWhoHasBlueFlag != null && player.PlayerId == CaptureTheFlag.redPlayerWhoHasBlueFlag.PlayerId) {
                                if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                    new CustomMessage(Language.statusCaptureTheFlagTexts[0], 5, -1, 1f, 4);
                                }
                                if (CaptureTheFlag.redplayer01 != null && CaptureTheFlag.redplayer01 == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                                    CaptureTheFlag.redteamFlag.Remove(CaptureTheFlag.redplayer01);
                                    CaptureTheFlag.redplayer01 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.redteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.redplayer01.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.redplayer02 != null && CaptureTheFlag.redplayer02 == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                                    CaptureTheFlag.redteamFlag.Remove(CaptureTheFlag.redplayer02);
                                    CaptureTheFlag.redplayer02 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.redteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.redplayer02.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.redplayer03 != null && CaptureTheFlag.redplayer03 == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                                    CaptureTheFlag.redteamFlag.Remove(CaptureTheFlag.redplayer03);
                                    CaptureTheFlag.redplayer03 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.redteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.redplayer03.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.redplayer04 != null && CaptureTheFlag.redplayer04 == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                                    CaptureTheFlag.redteamFlag.Remove(CaptureTheFlag.redplayer04);
                                    CaptureTheFlag.redplayer04 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.redteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.redplayer04.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.redplayer05 != null && CaptureTheFlag.redplayer05 == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                                    CaptureTheFlag.redteamFlag.Remove(CaptureTheFlag.redplayer05);
                                    CaptureTheFlag.redplayer05 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.redteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.redplayer05.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.redplayer06 != null && CaptureTheFlag.redplayer06 == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                                    CaptureTheFlag.redteamFlag.Remove(CaptureTheFlag.redplayer06);
                                    CaptureTheFlag.redplayer06 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.redteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.redplayer06.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.redplayer07 != null && CaptureTheFlag.redplayer07 == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                                    CaptureTheFlag.redteamFlag.Remove(CaptureTheFlag.redplayer07);
                                    CaptureTheFlag.redplayer07 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.redteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.redplayer07.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                            }
                            else if (CaptureTheFlag.bluePlayerWhoHasRedFlag != null && player.PlayerId == CaptureTheFlag.bluePlayerWhoHasRedFlag.PlayerId) {
                                if (PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer) {
                                    new CustomMessage(Language.statusCaptureTheFlagTexts[1], 5, -1, 1f, 4);
                                }
                                if (CaptureTheFlag.blueplayer01 != null && CaptureTheFlag.blueplayer01 == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                                    CaptureTheFlag.blueteamFlag.Remove(CaptureTheFlag.blueplayer01);
                                    CaptureTheFlag.blueplayer01 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.blueteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.blueplayer01.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.blueplayer02 != null && CaptureTheFlag.blueplayer02 == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                                    CaptureTheFlag.blueteamFlag.Remove(CaptureTheFlag.blueplayer02);
                                    CaptureTheFlag.blueplayer02 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.blueteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.blueplayer02.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.blueplayer03 != null && CaptureTheFlag.blueplayer03 == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                                    CaptureTheFlag.blueteamFlag.Remove(CaptureTheFlag.blueplayer03);
                                    CaptureTheFlag.blueplayer03 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.blueteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.blueplayer03.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.blueplayer04 != null && CaptureTheFlag.blueplayer04 == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                                    CaptureTheFlag.blueteamFlag.Remove(CaptureTheFlag.blueplayer04);
                                    CaptureTheFlag.blueplayer04 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.blueteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.blueplayer04.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.blueplayer05 != null && CaptureTheFlag.blueplayer05 == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                                    CaptureTheFlag.blueteamFlag.Remove(CaptureTheFlag.blueplayer05);
                                    CaptureTheFlag.blueplayer05 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.blueteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.blueplayer05.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.blueplayer06 != null && CaptureTheFlag.blueplayer06 == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                                    CaptureTheFlag.blueteamFlag.Remove(CaptureTheFlag.blueplayer06);
                                    CaptureTheFlag.blueplayer06 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.blueteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.blueplayer06.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                                else if (CaptureTheFlag.blueplayer07 != null && CaptureTheFlag.blueplayer07 == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                                    CaptureTheFlag.blueteamFlag.Remove(CaptureTheFlag.blueplayer07);
                                    CaptureTheFlag.blueplayer07 = CaptureTheFlag.stealerPlayer;
                                    CaptureTheFlag.blueteamFlag.Add(CaptureTheFlag.stealerPlayer);
                                    CaptureTheFlag.stealerPlayer = player;
                                    CaptureTheFlag.blueplayer07.MurderPlayer(CaptureTheFlag.stealerPlayer);
                                }
                            }
                            else {
                                CaptureTheFlag.stealerPlayer.MurderPlayer(player);
                            }
                            break;
                    }
                    return;
                }
            }
        }

        public static void captureTheFlagWhoTookTheFlag(byte playerWhoStoleTheFlag, int redorblue) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                // Red team steal blue flag
                if (player.PlayerId == playerWhoStoleTheFlag && redorblue == 1) {
                    CaptureTheFlag.blueflagtaken = true;
                    CaptureTheFlag.blueteamAlerted = false;
                    CaptureTheFlag.redPlayerWhoHasBlueFlag = player;
                    CaptureTheFlag.blueflag.transform.parent = player.transform;
                    CaptureTheFlag.blueflag.transform.localPosition = new Vector3(0f, 0f, -0.1f);
                    foreach (PlayerControl redplayer in CaptureTheFlag.redteamFlag) {
                        if (redplayer == PlayerControl.LocalPlayer && redplayer != null) {
                            new CustomMessage(Language.statusCaptureTheFlagTexts[2] + CaptureTheFlag.redPlayerWhoHasBlueFlag.name + "</color>!", 5, -1, 1.6f, 4);
                        }
                    }
                    if (CaptureTheFlag.stealerPlayer != null && CaptureTheFlag.stealerPlayer == PlayerControl.LocalPlayer) {
                        new CustomMessage(Language.statusCaptureTheFlagTexts[2] + CaptureTheFlag.redPlayerWhoHasBlueFlag.name + "</color>!", 5, -1, 1.6f, 4);
                    }
                }

                // Alert red team players
                if (CaptureTheFlag.redflagtaken && !CaptureTheFlag.redteamAlerted) {
                    CaptureTheFlag.redteamAlerted = true;
                    foreach (PlayerControl redplayer in CaptureTheFlag.redteamFlag) {
                        if (redplayer == PlayerControl.LocalPlayer && redplayer != null) {
                            new CustomMessage(Language.statusCaptureTheFlagTexts[3], 5, -1, 1f, 4);
                        }
                    }
                }

                // Blue team steal red flag
                if (player.PlayerId == playerWhoStoleTheFlag && redorblue == 2) {
                    CaptureTheFlag.redflagtaken = true;
                    CaptureTheFlag.redteamAlerted = false;
                    CaptureTheFlag.bluePlayerWhoHasRedFlag = player;
                    CaptureTheFlag.redflag.transform.parent = player.transform;
                    CaptureTheFlag.redflag.transform.localPosition = new Vector3(0f, 0f, -0.1f);
                    foreach (PlayerControl blueplayer in CaptureTheFlag.blueteamFlag) {
                        if (blueplayer == PlayerControl.LocalPlayer && blueplayer != null) {
                            new CustomMessage(Language.statusCaptureTheFlagTexts[4] + CaptureTheFlag.bluePlayerWhoHasRedFlag.name + "</color>!", 5, -1, 1.6f, 4);
                        }
                    }
                    if (CaptureTheFlag.stealerPlayer != null && CaptureTheFlag.stealerPlayer == PlayerControl.LocalPlayer) {
                        new CustomMessage(Language.statusCaptureTheFlagTexts[4] + CaptureTheFlag.bluePlayerWhoHasRedFlag.name + "</color>!", 5, -1, 1.3f, 4);
                    }
                }

                // Alert blue team players
                if (CaptureTheFlag.blueflagtaken && !CaptureTheFlag.blueteamAlerted) {
                    CaptureTheFlag.blueteamAlerted = true;
                    foreach (PlayerControl blueplayer in CaptureTheFlag.blueteamFlag) {
                        if (blueplayer == PlayerControl.LocalPlayer && blueplayer != null) {
                            new CustomMessage(Language.statusCaptureTheFlagTexts[3], 5, -1, 1f, 4);
                        }
                    }
                }
            }
        }

        public static void captureTheFlagWhichTeamScored(int whichteam) {
            // Red team
            if (whichteam == 1) {
                CaptureTheFlag.blueflagtaken = false;
                CaptureTheFlag.redPlayerWhoHasBlueFlag = null;
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
                    // Submerged
                    case 5:
                        CaptureTheFlag.blueflag.transform.position = new Vector3(12.5f, -31.45f, -0.011f);
                        break;
                }
                CaptureTheFlag.currentRedTeamPoints += 1;
                new CustomMessage(Language.statusCaptureTheFlagTexts[5], 5, -1, 1.6f, 4);
                CaptureTheFlag.flagpointCounter = Language.introTexts[2] + "<color=#FF0000FF>" + CaptureTheFlag.currentRedTeamPoints + "</color> - " + "<color=#0000FFFF>" + CaptureTheFlag.currentBlueTeamPoints + "</color>";
                if (CaptureTheFlag.currentRedTeamPoints >= CaptureTheFlag.requiredFlags) {
                    CaptureTheFlag.triggerRedTeamWin = true;
                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.RedTeamFlagWin, false);
                }
            }

            // Blue team
            if (whichteam == 2) {
                CaptureTheFlag.redflagtaken = false;
                CaptureTheFlag.bluePlayerWhoHasRedFlag = null;
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
                    // Submerged
                    case 5:
                        CaptureTheFlag.redflag.transform.position = new Vector3(-8.35f, 28.05f, -0.011f);
                        break;
                }
                CaptureTheFlag.currentBlueTeamPoints += 1;
                new CustomMessage(Language.statusCaptureTheFlagTexts[6], 5, -1, 1.3f, 4);
                CaptureTheFlag.flagpointCounter = Language.introTexts[2] + "<color=#FF0000FF>" + CaptureTheFlag.currentRedTeamPoints + "</color> - " + "<color=#0000FFFF>" + CaptureTheFlag.currentBlueTeamPoints + "</color>";
                if (CaptureTheFlag.currentBlueTeamPoints >= CaptureTheFlag.requiredFlags) {
                    CaptureTheFlag.triggerBlueTeamWin = true;
                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BlueTeamFlagWin, false);
                }
            }
        }

        public static void policeandThiefKills(byte targetId, int whichplayer) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    switch (whichplayer) {
                        case 1:
                            PoliceAndThief.policeplayer01.MurderPlayer(player);
                            break;
                        case 2:
                            PoliceAndThief.policeplayer02.MurderPlayer(player);
                            break;
                        case 3:
                            PoliceAndThief.policeplayer03.MurderPlayer(player);
                            break;
                        case 4:
                            PoliceAndThief.policeplayer04.MurderPlayer(player);
                            break;
                        case 5:
                            PoliceAndThief.policeplayer05.MurderPlayer(player);
                            break;
                        case 6:
                            PoliceAndThief.policeplayer06.MurderPlayer(player);
                            break;
                        case 7:
                            PoliceAndThief.thiefplayer01.MurderPlayer(player);
                            break;
                        case 8:
                            PoliceAndThief.thiefplayer02.MurderPlayer(player);
                            break;
                        case 9:
                            PoliceAndThief.thiefplayer03.MurderPlayer(player);
                            break;
                        case 10:
                            PoliceAndThief.thiefplayer04.MurderPlayer(player);
                            break;
                        case 11:
                            PoliceAndThief.thiefplayer05.MurderPlayer(player);
                            break;
                        case 12:
                            PoliceAndThief.thiefplayer06.MurderPlayer(player);
                            break;
                        case 13:
                            PoliceAndThief.thiefplayer07.MurderPlayer(player);
                            break;
                        case 14:
                            PoliceAndThief.thiefplayer08.MurderPlayer(player);
                            break;
                        case 15:
                            PoliceAndThief.thiefplayer09.MurderPlayer(player);
                            break;
                    }
                    return;
                }
            }
        }

        public static void policeandThiefJail(byte thiefId) {
            foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                if (player.PlayerId == thiefId) {
                    if (player.inVent) {
                        foreach (Vent vent in ShipStatus.Instance.AllVents) {
                            bool canUse;
                            bool couldUse;
                            vent.CanUse(player.Data, out canUse, out couldUse);
                            if (canUse) {
                                player.MyPhysics.RpcExitVent(vent.Id);
                                vent.SetButtons(false);
                            }
                        }
                    }
                    if (PlayerControl.LocalPlayer == player) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jailerJail, false, 100f);
                    }
                    PoliceAndThief.thiefArrested.Add(player);
                    if (PoliceAndThief.thiefplayer01 != null && thiefId == PoliceAndThief.thiefplayer01.PlayerId) {
                        if (PoliceAndThief.thiefplayer01IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer01JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer02 != null && thiefId == PoliceAndThief.thiefplayer02.PlayerId) {
                        if (PoliceAndThief.thiefplayer02IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer02JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer03 != null && thiefId == PoliceAndThief.thiefplayer03.PlayerId) {
                        if (PoliceAndThief.thiefplayer03IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer03JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer04 != null && thiefId == PoliceAndThief.thiefplayer04.PlayerId) {
                        if (PoliceAndThief.thiefplayer04IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer04JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer05 != null && thiefId == PoliceAndThief.thiefplayer05.PlayerId) {
                        if (PoliceAndThief.thiefplayer05IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer05JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer06 != null && thiefId == PoliceAndThief.thiefplayer06.PlayerId) {
                        if (PoliceAndThief.thiefplayer06IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer06JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer07 != null && thiefId == PoliceAndThief.thiefplayer07.PlayerId) {
                        if (PoliceAndThief.thiefplayer07IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer07JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer08 != null && thiefId == PoliceAndThief.thiefplayer08.PlayerId) {
                        if (PoliceAndThief.thiefplayer08IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer08JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer09 != null && thiefId == PoliceAndThief.thiefplayer09.PlayerId) {
                        if (PoliceAndThief.thiefplayer09IsStealing) {
                            policeandThiefRevertedJewelPosition(thiefId, PoliceAndThief.thiefplayer09JewelId);
                        }
                    }
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        // Skeld
                        case 0:
                            if (activatedSensei) {
                                player.transform.position = new Vector3(-12f, 7.15f, player.transform.position.z);
                            }
                            else {
                                player.transform.position = new Vector3(-10.2f, 3.6f, player.transform.position.z);
                            }
                            break;
                        // MiraHQ
                        case 1:
                            player.transform.position = new Vector3(1.8f, 1.25f, player.transform.position.z);
                            break;
                        // Polus
                        case 2:
                            player.transform.position = new Vector3(8.25f, -5f, player.transform.position.z);
                            break;
                        // Dleks
                        case 3:
                            player.transform.position = new Vector3(10.2f, 3.6f, player.transform.position.z);
                            break;
                        // Airship
                        case 4:
                            player.transform.position = new Vector3(-18.5f, 3.5f, player.transform.position.z);
                            break;
                        // Submerged
                        case 5:
                            if (player.transform.position.y > 0) {
                                player.transform.position = new Vector3(-6f, 32f, player.transform.position.z);
                            }
                            else {
                                player.transform.position = new Vector3(-14.1f, -39f, player.transform.position.z);
                            }
                            break;
                    }
                    PoliceAndThief.currentThiefsCaptured += 1;
                    new CustomMessage(Language.statusPoliceAndThiefsTexts[0], 5, -1, 1.3f, 7);
                    PoliceAndThief.thiefpointCounter = Language.introTexts[3] + "<color=#00F7FFFF>" + PoliceAndThief.currentJewelsStoled + " / " + PoliceAndThief.requiredJewels + "</color> | " + Language.introTexts[4] + "<color=#928B55FF>" + PoliceAndThief.currentThiefsCaptured + " / " + PoliceAndThief.thiefTeam.Count + "</color>";
                    if (PoliceAndThief.currentThiefsCaptured == PoliceAndThief.thiefTeam.Count) {
                        PoliceAndThief.triggerPoliceWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModePoliceWin, false);
                    }
                }
            }

        }

        public static void policeandThiefFreeThief() {
            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                // Skeld
                case 0:
                    if (activatedSensei) {
                        PoliceAndThief.thiefArrested[0].transform.position = new Vector3(13.75f, -0.2f, PoliceAndThief.thiefArrested[0].transform.position.z);
                    }
                    else {
                        PoliceAndThief.thiefArrested[0].transform.position = new Vector3(-1.31f, -16.25f, PoliceAndThief.thiefArrested[0].transform.position.z);
                    }
                    break;
                // MiraHQ
                case 1:
                    PoliceAndThief.thiefArrested[0].transform.position = new Vector3(17.75f, 11.5f, PoliceAndThief.thiefArrested[0].transform.position.z);
                    break;
                // Polus
                case 2:
                    PoliceAndThief.thiefArrested[0].transform.position = new Vector3(30f, -15.75f, PoliceAndThief.thiefArrested[0].transform.position.z);
                    break;
                // Dleks
                case 3:
                    PoliceAndThief.thiefArrested[0].transform.position = new Vector3(1.31f, -16.25f, PoliceAndThief.thiefArrested[0].transform.position.z);
                    break;
                // Airship
                case 4:
                    PoliceAndThief.thiefArrested[0].transform.position = new Vector3(7.15f, -14.5f, PoliceAndThief.thiefArrested[0].transform.position.z);
                    break;
                // Submerged
                case 5:
                    if (PoliceAndThief.thiefArrested[0].transform.position.y > 0) {
                        PoliceAndThief.thiefArrested[0].transform.position = new Vector3(1f, 10f, PoliceAndThief.thiefArrested[0].transform.position.z);
                    }
                    else {
                        PoliceAndThief.thiefArrested[0].transform.position = new Vector3(12.5f, -31.75f, PoliceAndThief.thiefArrested[0].transform.position.z);
                    }
                    break;
            }
            PoliceAndThief.thiefArrested.RemoveAt(0);
            PoliceAndThief.currentThiefsCaptured = PoliceAndThief.currentThiefsCaptured - 1;
            new CustomMessage(Language.statusPoliceAndThiefsTexts[1], 5, -1, 1f, 7);
            PoliceAndThief.thiefpointCounter = Language.introTexts[3] + "<color=#00F7FFFF>" + PoliceAndThief.currentJewelsStoled + " / " + PoliceAndThief.requiredJewels + "</color> | " + Language.introTexts[4] + "<color=#928B55FF>" + PoliceAndThief.currentThiefsCaptured + " / " + PoliceAndThief.thiefTeam.Count + "</color>";
        }

        public static void policeandThiefTakeJewel(byte thiefWhoTookATreasure, byte jewelId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                // Thief player steal a jewel
                if (player.PlayerId == thiefWhoTookATreasure) {
                    if (PoliceAndThief.thiefplayer01 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer01.PlayerId) {
                        PoliceAndThief.thiefplayer01IsStealing = true;
                        PoliceAndThief.thiefplayer01JewelId = jewelId;
                    }
                    else if (PoliceAndThief.thiefplayer02 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer02.PlayerId) {
                        PoliceAndThief.thiefplayer02IsStealing = true;
                        PoliceAndThief.thiefplayer02JewelId = jewelId;
                    }
                    else if (PoliceAndThief.thiefplayer03 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer03.PlayerId) {
                        PoliceAndThief.thiefplayer03IsStealing = true;
                        PoliceAndThief.thiefplayer03JewelId = jewelId;
                    }
                    else if (PoliceAndThief.thiefplayer04 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer04.PlayerId) {
                        PoliceAndThief.thiefplayer04IsStealing = true;
                        PoliceAndThief.thiefplayer04JewelId = jewelId;
                    }
                    else if (PoliceAndThief.thiefplayer05 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer05.PlayerId) {
                        PoliceAndThief.thiefplayer05IsStealing = true;
                        PoliceAndThief.thiefplayer05JewelId = jewelId;
                    }
                    else if (PoliceAndThief.thiefplayer06 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer06.PlayerId) {
                        PoliceAndThief.thiefplayer06IsStealing = true;
                        PoliceAndThief.thiefplayer06JewelId = jewelId;
                    }
                    else if (PoliceAndThief.thiefplayer07 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer07.PlayerId) {
                        PoliceAndThief.thiefplayer07IsStealing = true;
                        PoliceAndThief.thiefplayer07JewelId = jewelId;
                    }
                    else if (PoliceAndThief.thiefplayer08 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer08.PlayerId) {
                        PoliceAndThief.thiefplayer08IsStealing = true;
                        PoliceAndThief.thiefplayer08JewelId = jewelId;
                    }
                    else if (PoliceAndThief.thiefplayer09 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer09.PlayerId) {
                        PoliceAndThief.thiefplayer09IsStealing = true;
                        PoliceAndThief.thiefplayer09JewelId = jewelId;
                    }
                    switch (jewelId) {
                        case 1:
                            PoliceAndThief.jewel01BeingStealed = player;
                            PoliceAndThief.jewel01.SetActive(true);
                            PoliceAndThief.jewel01.transform.parent = player.transform;
                            PoliceAndThief.jewel01.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 2:
                            PoliceAndThief.jewel02BeingStealed = player;
                            PoliceAndThief.jewel02.SetActive(true);
                            PoliceAndThief.jewel02.transform.parent = player.transform;
                            PoliceAndThief.jewel02.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 3:
                            PoliceAndThief.jewel03BeingStealed = player;
                            PoliceAndThief.jewel03.SetActive(true);
                            PoliceAndThief.jewel03.transform.parent = player.transform;
                            PoliceAndThief.jewel03.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 4:
                            PoliceAndThief.jewel04BeingStealed = player;
                            PoliceAndThief.jewel04.SetActive(true);
                            PoliceAndThief.jewel04.transform.parent = player.transform;
                            PoliceAndThief.jewel04.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 5:
                            PoliceAndThief.jewel05BeingStealed = player;
                            PoliceAndThief.jewel05.SetActive(true);
                            PoliceAndThief.jewel05.transform.parent = player.transform;
                            PoliceAndThief.jewel05.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 6:
                            PoliceAndThief.jewel06BeingStealed = player;
                            PoliceAndThief.jewel06.SetActive(true);
                            PoliceAndThief.jewel06.transform.parent = player.transform;
                            PoliceAndThief.jewel06.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 7:
                            PoliceAndThief.jewel07BeingStealed = player;
                            PoliceAndThief.jewel07.SetActive(true);
                            PoliceAndThief.jewel07.transform.parent = player.transform;
                            PoliceAndThief.jewel07.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 8:
                            PoliceAndThief.jewel08BeingStealed = player;
                            PoliceAndThief.jewel08.SetActive(true);
                            PoliceAndThief.jewel08.transform.parent = player.transform;
                            PoliceAndThief.jewel08.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 9:
                            PoliceAndThief.jewel09BeingStealed = player;
                            PoliceAndThief.jewel09.SetActive(true);
                            PoliceAndThief.jewel09.transform.parent = player.transform;
                            PoliceAndThief.jewel09.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 10:
                            PoliceAndThief.jewel10BeingStealed = player;
                            PoliceAndThief.jewel10.SetActive(true);
                            PoliceAndThief.jewel10.transform.parent = player.transform;
                            PoliceAndThief.jewel10.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 11:
                            PoliceAndThief.jewel11BeingStealed = player;
                            PoliceAndThief.jewel11.SetActive(true);
                            PoliceAndThief.jewel11.transform.parent = player.transform;
                            PoliceAndThief.jewel11.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 12:
                            PoliceAndThief.jewel12BeingStealed = player;
                            PoliceAndThief.jewel12.SetActive(true);
                            PoliceAndThief.jewel12.transform.parent = player.transform;
                            PoliceAndThief.jewel12.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 13:
                            PoliceAndThief.jewel13BeingStealed = player;
                            PoliceAndThief.jewel13.SetActive(true);
                            PoliceAndThief.jewel13.transform.parent = player.transform;
                            PoliceAndThief.jewel13.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 14:
                            PoliceAndThief.jewel14BeingStealed = player;
                            PoliceAndThief.jewel14.SetActive(true);
                            PoliceAndThief.jewel14.transform.parent = player.transform;
                            PoliceAndThief.jewel14.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                        case 15:
                            PoliceAndThief.jewel15BeingStealed = player;
                            PoliceAndThief.jewel15.SetActive(true);
                            PoliceAndThief.jewel15.transform.parent = player.transform;
                            PoliceAndThief.jewel15.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            break;
                    }
                }

            }
        }

        public static void policeandThiefDeliverJewel(byte thiefWhoTookATreasure, byte jewelId) {
            // Thief team
            foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                // Thief player steal a jewel
                if (player.PlayerId == thiefWhoTookATreasure) {
                    if (PoliceAndThief.thiefplayer01 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer01.PlayerId) {
                        PoliceAndThief.thiefplayer01IsStealing = false;
                        PoliceAndThief.thiefplayer01JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer02 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer02.PlayerId) {
                        PoliceAndThief.thiefplayer02IsStealing = false;
                        PoliceAndThief.thiefplayer02JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer03 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer03.PlayerId) {
                        PoliceAndThief.thiefplayer03IsStealing = false;
                        PoliceAndThief.thiefplayer03JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer04 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer04.PlayerId) {
                        PoliceAndThief.thiefplayer04IsStealing = false;
                        PoliceAndThief.thiefplayer04JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer05 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer05.PlayerId) {
                        PoliceAndThief.thiefplayer05IsStealing = false;
                        PoliceAndThief.thiefplayer05JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer06 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer06.PlayerId) {
                        PoliceAndThief.thiefplayer06IsStealing = false;
                        PoliceAndThief.thiefplayer06JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer07 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer07.PlayerId) {
                        PoliceAndThief.thiefplayer07IsStealing = false;
                        PoliceAndThief.thiefplayer07JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer08 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer08.PlayerId) {
                        PoliceAndThief.thiefplayer08IsStealing = false;
                        PoliceAndThief.thiefplayer08JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer09 != null && thiefWhoTookATreasure == PoliceAndThief.thiefplayer09.PlayerId) {
                        PoliceAndThief.thiefplayer09IsStealing = false;
                        PoliceAndThief.thiefplayer09JewelId = 0;
                    }
                    GameObject myJewel = null;
                    bool isDiamond = true;
                    switch (jewelId) {
                        case 1:
                            myJewel = PoliceAndThief.jewel01;
                            PoliceAndThief.jewel01BeingStealed = null;
                            break;
                        case 2:
                            myJewel = PoliceAndThief.jewel02;
                            PoliceAndThief.jewel02BeingStealed = null;
                            break;
                        case 3:
                            myJewel = PoliceAndThief.jewel03;
                            PoliceAndThief.jewel03BeingStealed = null;
                            break;
                        case 4:
                            myJewel = PoliceAndThief.jewel04;
                            PoliceAndThief.jewel04BeingStealed = null;
                            break;
                        case 5:
                            myJewel = PoliceAndThief.jewel05;
                            PoliceAndThief.jewel05BeingStealed = null;
                            break;
                        case 6:
                            myJewel = PoliceAndThief.jewel06;
                            PoliceAndThief.jewel06BeingStealed = null;
                            break;
                        case 7:
                            myJewel = PoliceAndThief.jewel07;
                            PoliceAndThief.jewel07BeingStealed = null;
                            break;
                        case 8:
                            myJewel = PoliceAndThief.jewel08;
                            PoliceAndThief.jewel08BeingStealed = null;
                            break;
                        case 9:
                            myJewel = PoliceAndThief.jewel09;
                            PoliceAndThief.jewel09BeingStealed = null;
                            isDiamond = !isDiamond;
                            break;
                        case 10:
                            myJewel = PoliceAndThief.jewel10;
                            PoliceAndThief.jewel10BeingStealed = null;
                            isDiamond = !isDiamond;
                            break;
                        case 11:
                            myJewel = PoliceAndThief.jewel11;
                            PoliceAndThief.jewel11BeingStealed = null;
                            isDiamond = !isDiamond;
                            break;
                        case 12:
                            myJewel = PoliceAndThief.jewel12;
                            PoliceAndThief.jewel12BeingStealed = null;
                            isDiamond = !isDiamond;
                            break;
                        case 13:
                            myJewel = PoliceAndThief.jewel13;
                            PoliceAndThief.jewel13BeingStealed = null;
                            isDiamond = !isDiamond;
                            break;
                        case 14:
                            myJewel = PoliceAndThief.jewel14;
                            PoliceAndThief.jewel14BeingStealed = null;
                            isDiamond = !isDiamond;
                            break;
                        case 15:
                            myJewel = PoliceAndThief.jewel15;
                            PoliceAndThief.jewel15BeingStealed = null;
                            isDiamond = !isDiamond;
                            break;
                    }
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        // Skeld
                        case 0:
                            if (activatedSensei) {
                                if (isDiamond) {
                                    myJewel.transform.position = new Vector3(15.25f, -0.33f, player.transform.position.z);
                                }
                                else {
                                    myJewel.transform.position = new Vector3(15.7f, -0.33f, player.transform.position.z);
                                }
                            }
                            else {
                                if (isDiamond) {

                                    myJewel.transform.position = new Vector3(0f, -19.4f, player.transform.position.z);
                                }
                                else {
                                    myJewel.transform.position = new Vector3(0.4f, -19.4f, player.transform.position.z);
                                }
                            }
                            break;
                        // MiraHQ
                        case 1:
                            if (isDiamond) {
                                myJewel.transform.position = new Vector3(19.65f, 13.9f, player.transform.position.z);
                            }
                            else {
                                myJewel.transform.position = new Vector3(20.075f, 13.9f, player.transform.position.z);
                            }
                            break;
                        // Polus
                        case 2:
                            if (isDiamond) {
                                myJewel.transform.position = new Vector3(33.6f, 13.9f, player.transform.position.z);
                            }
                            else {
                                myJewel.transform.position = new Vector3(34.05f, -15.9f, player.transform.position.z);
                            }
                            break;
                        // Dleks
                        case 3:
                            if (isDiamond) {
                                myJewel.transform.position = new Vector3(0f, -19.4f, player.transform.position.z);
                            }
                            else {
                                myJewel.transform.position = new Vector3(-0.4f, -19.4f, player.transform.position.z);
                            }
                            break;
                        // Airship
                        case 4:
                            if (isDiamond) {
                                myJewel.transform.position = new Vector3(11.75f, -16.35f, player.transform.position.z);
                            }
                            else {
                                myJewel.transform.position = new Vector3(12.2f, -16.35f, player.transform.position.z);
                            }
                            break;
                        // Submerged
                        case 5:
                            if (isDiamond) {
                                if (myJewel.transform.position.y > 0) {
                                    myJewel.transform.position = new Vector3(-1.4f, 8.65f, player.transform.position.z);
                                }
                                else {
                                    myJewel.transform.position = new Vector3(12.7f, -35.35f, player.transform.position.z);
                                }
                            }
                            else {
                                if (myJewel.transform.position.y > 0) {
                                    myJewel.transform.position = new Vector3(-1f, 8.65f, player.transform.position.z);
                                }
                                else {
                                    myJewel.transform.position = new Vector3(13.1f, -35.35f, player.transform.position.z);
                                }
                            }
                            break;
                    }
                    myJewel.transform.SetParent(null);
                }

            }
            PoliceAndThief.currentJewelsStoled += 1;
            new CustomMessage(Language.statusPoliceAndThiefsTexts[2], 5, -1, 1.6f, 7);
            PoliceAndThief.thiefpointCounter = Language.introTexts[3] + "<color=#00F7FFFF>" + PoliceAndThief.currentJewelsStoled + " / " + PoliceAndThief.requiredJewels + "</color> | " + Language.introTexts[4] + "<color=#928B55FF>" + PoliceAndThief.currentThiefsCaptured + " / " + PoliceAndThief.thiefTeam.Count + "</color>";
            if (PoliceAndThief.currentJewelsStoled >= PoliceAndThief.requiredJewels) {
                PoliceAndThief.triggerThiefWin = true;
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.ThiefModeThiefWin, false);
            }
        }

        public static void policeandThiefRevertedJewelPosition(byte thiefWhoLostJewel, byte jewelRevertedId) {
            foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                if (player.PlayerId == thiefWhoLostJewel) {
                    if (PoliceAndThief.thiefplayer01 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer01.PlayerId) {
                        PoliceAndThief.thiefplayer01IsStealing = false;
                        PoliceAndThief.thiefplayer01JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer02 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer02.PlayerId) {
                        PoliceAndThief.thiefplayer02IsStealing = false;
                        PoliceAndThief.thiefplayer02JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer03 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer03.PlayerId) {
                        PoliceAndThief.thiefplayer03IsStealing = false;
                        PoliceAndThief.thiefplayer03JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer04 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer04.PlayerId) {
                        PoliceAndThief.thiefplayer04IsStealing = false;
                        PoliceAndThief.thiefplayer04JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer05 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer05.PlayerId) {
                        PoliceAndThief.thiefplayer05IsStealing = false;
                        PoliceAndThief.thiefplayer05JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer06 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer06.PlayerId) {
                        PoliceAndThief.thiefplayer06IsStealing = false;
                        PoliceAndThief.thiefplayer06JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer07 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer07.PlayerId) {
                        PoliceAndThief.thiefplayer07IsStealing = false;
                        PoliceAndThief.thiefplayer07JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer08 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer08.PlayerId) {
                        PoliceAndThief.thiefplayer08IsStealing = false;
                        PoliceAndThief.thiefplayer08JewelId = 0;
                    }
                    else if (PoliceAndThief.thiefplayer09 != null && thiefWhoLostJewel == PoliceAndThief.thiefplayer09.PlayerId) {
                        PoliceAndThief.thiefplayer09IsStealing = false;
                        PoliceAndThief.thiefplayer09JewelId = 0;
                    }
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        // Skeld
                        case 0:
                            if (activatedSensei) {
                                switch (jewelRevertedId) {
                                    case 1:
                                        PoliceAndThief.jewel01.transform.SetParent(null);
                                        PoliceAndThief.jewel01.transform.position = new Vector3(6.95f, 4.95f, 1f);
                                        PoliceAndThief.jewel01BeingStealed = null;
                                        break;
                                    case 2:
                                        PoliceAndThief.jewel02.transform.SetParent(null);
                                        PoliceAndThief.jewel02.transform.position = new Vector3(-3.75f, 5.35f, 1f);
                                        PoliceAndThief.jewel02BeingStealed = null;
                                        break;
                                    case 3:
                                        PoliceAndThief.jewel03.transform.SetParent(null);
                                        PoliceAndThief.jewel03.transform.position = new Vector3(-7.7f, 11.3f, 1f);
                                        PoliceAndThief.jewel03BeingStealed = null;
                                        break;
                                    case 4:
                                        PoliceAndThief.jewel04.transform.SetParent(null);
                                        PoliceAndThief.jewel04.transform.position = new Vector3(-19.65f, 5.3f, 1f);
                                        PoliceAndThief.jewel04BeingStealed = null;
                                        break;
                                    case 5:
                                        PoliceAndThief.jewel05.transform.SetParent(null);
                                        PoliceAndThief.jewel05.transform.position = new Vector3(-19.65f, -8, 1f);
                                        PoliceAndThief.jewel05BeingStealed = null;
                                        break;
                                    case 6:
                                        PoliceAndThief.jewel06.transform.SetParent(null);
                                        PoliceAndThief.jewel06.transform.position = new Vector3(-5.45f, -13f, 1f);
                                        PoliceAndThief.jewel06BeingStealed = null;
                                        break;
                                    case 7:
                                        PoliceAndThief.jewel07.transform.SetParent(null);
                                        PoliceAndThief.jewel07.transform.position = new Vector3(-7.65f, -4.2f, 1f);
                                        PoliceAndThief.jewel07BeingStealed = null;
                                        break;
                                    case 8:
                                        PoliceAndThief.jewel08.transform.SetParent(null);
                                        PoliceAndThief.jewel08.transform.position = new Vector3(2f, -6.75f, 1f);
                                        PoliceAndThief.jewel08BeingStealed = null;
                                        break;
                                    case 9:
                                        PoliceAndThief.jewel09.transform.SetParent(null);
                                        PoliceAndThief.jewel09.transform.position = new Vector3(8.9f, 1.45f, 1f);
                                        PoliceAndThief.jewel09BeingStealed = null;
                                        break;
                                    case 10:
                                        PoliceAndThief.jewel10.transform.SetParent(null);
                                        PoliceAndThief.jewel10.transform.position = new Vector3(4.6f, -2.25f, 1f);
                                        PoliceAndThief.jewel10BeingStealed = null;
                                        break;
                                    case 11:
                                        PoliceAndThief.jewel11.transform.SetParent(null);
                                        PoliceAndThief.jewel11.transform.position = new Vector3(-5.05f, -0.88f, 1f);
                                        PoliceAndThief.jewel11BeingStealed = null;
                                        break;
                                    case 12:
                                        PoliceAndThief.jewel12.transform.SetParent(null);
                                        PoliceAndThief.jewel12.transform.position = new Vector3(-8.25f, -0.45f, 1f);
                                        PoliceAndThief.jewel12BeingStealed = null;
                                        break;
                                    case 13:
                                        PoliceAndThief.jewel13.transform.SetParent(null);
                                        PoliceAndThief.jewel13.transform.position = new Vector3(-19.75f, -1.55f, 1f);
                                        PoliceAndThief.jewel13BeingStealed = null;
                                        break;
                                    case 14:
                                        PoliceAndThief.jewel14.transform.SetParent(null);
                                        PoliceAndThief.jewel14.transform.position = new Vector3(-12.1f, -13.15f, 1f);
                                        PoliceAndThief.jewel14BeingStealed = null;
                                        break;
                                    case 15:
                                        PoliceAndThief.jewel15.transform.SetParent(null);
                                        PoliceAndThief.jewel15.transform.position = new Vector3(7.15f, -14.45f, 1f);
                                        PoliceAndThief.jewel15BeingStealed = null;
                                        break;
                                }
                            }
                            else {
                                switch (jewelRevertedId) {
                                    case 1:
                                        PoliceAndThief.jewel01.transform.SetParent(null);
                                        PoliceAndThief.jewel01.transform.position = new Vector3(-18.65f, -9.9f, 1f);
                                        PoliceAndThief.jewel01BeingStealed = null;
                                        break;
                                    case 2:
                                        PoliceAndThief.jewel02.transform.SetParent(null);
                                        PoliceAndThief.jewel02.transform.position = new Vector3(-21.5f, -2, 1f);
                                        PoliceAndThief.jewel02BeingStealed = null;
                                        break;
                                    case 3:
                                        PoliceAndThief.jewel03.transform.SetParent(null);
                                        PoliceAndThief.jewel03.transform.position = new Vector3(-5.9f, -8.25f, 1f);
                                        PoliceAndThief.jewel03BeingStealed = null;
                                        break;
                                    case 4:
                                        PoliceAndThief.jewel04.transform.SetParent(null);
                                        PoliceAndThief.jewel04.transform.position = new Vector3(4.5f, -7.5f, 1f);
                                        PoliceAndThief.jewel04BeingStealed = null;
                                        break;
                                    case 5:
                                        PoliceAndThief.jewel05.transform.SetParent(null);
                                        PoliceAndThief.jewel05.transform.position = new Vector3(7.85f, -14.45f, 1f);
                                        PoliceAndThief.jewel05BeingStealed = null;
                                        break;
                                    case 6:
                                        PoliceAndThief.jewel06.transform.SetParent(null);
                                        PoliceAndThief.jewel06.transform.position = new Vector3(6.65f, -4.8f, 1f);
                                        PoliceAndThief.jewel06BeingStealed = null;
                                        break;
                                    case 7:
                                        PoliceAndThief.jewel07.transform.SetParent(null);
                                        PoliceAndThief.jewel07.transform.position = new Vector3(10.5f, 2.15f, 1f);
                                        PoliceAndThief.jewel07BeingStealed = null;
                                        break;
                                    case 8:
                                        PoliceAndThief.jewel08.transform.SetParent(null);
                                        PoliceAndThief.jewel08.transform.position = new Vector3(-5.5f, 3.5f, 1f);
                                        PoliceAndThief.jewel08BeingStealed = null;
                                        break;
                                    case 9:
                                        PoliceAndThief.jewel09.transform.SetParent(null);
                                        PoliceAndThief.jewel09.transform.position = new Vector3(-19, -1.2f, 1f);
                                        PoliceAndThief.jewel09BeingStealed = null;
                                        break;
                                    case 10:
                                        PoliceAndThief.jewel10.transform.SetParent(null);
                                        PoliceAndThief.jewel10.transform.position = new Vector3(-21.5f, -8.35f, 1f);
                                        PoliceAndThief.jewel10BeingStealed = null;
                                        break;
                                    case 11:
                                        PoliceAndThief.jewel11.transform.SetParent(null);
                                        PoliceAndThief.jewel11.transform.position = new Vector3(-12.5f, -3.75f, 1f);
                                        PoliceAndThief.jewel11BeingStealed = null;
                                        break;
                                    case 12:
                                        PoliceAndThief.jewel12.transform.SetParent(null);
                                        PoliceAndThief.jewel12.transform.position = new Vector3(-5.9f, -5.25f, 1f);
                                        PoliceAndThief.jewel12BeingStealed = null;
                                        break;
                                    case 13:
                                        PoliceAndThief.jewel13.transform.SetParent(null);
                                        PoliceAndThief.jewel13.transform.position = new Vector3(2.65f, -16.5f, 1f);
                                        PoliceAndThief.jewel13BeingStealed = null;
                                        break;
                                    case 14:
                                        PoliceAndThief.jewel14.transform.SetParent(null);
                                        PoliceAndThief.jewel14.transform.position = new Vector3(16.75f, -4.75f, 1f);
                                        PoliceAndThief.jewel14BeingStealed = null;
                                        break;
                                    case 15:
                                        PoliceAndThief.jewel15.transform.SetParent(null);
                                        PoliceAndThief.jewel15.transform.position = new Vector3(3.8f, 3.5f, 1f);
                                        PoliceAndThief.jewel15BeingStealed = null;
                                        break;
                                }
                            }
                            break;
                        // MiraHQ
                        case 1:
                            switch (jewelRevertedId) {
                                case 1:
                                    PoliceAndThief.jewel01.transform.SetParent(null);
                                    PoliceAndThief.jewel01.transform.position = new Vector3(-4.5f, 2.5f, 1f);
                                    PoliceAndThief.jewel01BeingStealed = null;
                                    break;
                                case 2:
                                    PoliceAndThief.jewel02.transform.SetParent(null);
                                    PoliceAndThief.jewel02.transform.position = new Vector3(6.25f, 14f, 1f);
                                    PoliceAndThief.jewel02BeingStealed = null;
                                    break;
                                case 3:
                                    PoliceAndThief.jewel03.transform.SetParent(null);
                                    PoliceAndThief.jewel03.transform.position = new Vector3(9.15f, 4.75f, 1f);
                                    PoliceAndThief.jewel03BeingStealed = null;
                                    break;
                                case 4:
                                    PoliceAndThief.jewel04.transform.SetParent(null);
                                    PoliceAndThief.jewel04.transform.position = new Vector3(14.75f, 20.5f, 1f);
                                    PoliceAndThief.jewel04BeingStealed = null;
                                    break;
                                case 5:
                                    PoliceAndThief.jewel05.transform.SetParent(null);
                                    PoliceAndThief.jewel05.transform.position = new Vector3(19.5f, 17.5f, 1f);
                                    PoliceAndThief.jewel05BeingStealed = null;
                                    break;
                                case 6:
                                    PoliceAndThief.jewel06.transform.SetParent(null);
                                    PoliceAndThief.jewel06.transform.position = new Vector3(21, 24.1f, 1f);
                                    PoliceAndThief.jewel06BeingStealed = null;
                                    break;
                                case 7:
                                    PoliceAndThief.jewel07.transform.SetParent(null);
                                    PoliceAndThief.jewel07.transform.position = new Vector3(19.5f, 4.75f, 1f);
                                    PoliceAndThief.jewel07BeingStealed = null;
                                    break;
                                case 8:
                                    PoliceAndThief.jewel08.transform.SetParent(null);
                                    PoliceAndThief.jewel08.transform.position = new Vector3(28.25f, 0, 1f);
                                    PoliceAndThief.jewel08BeingStealed = null;
                                    break;
                                case 9:
                                    PoliceAndThief.jewel09.transform.SetParent(null);
                                    PoliceAndThief.jewel09.transform.position = new Vector3(2.45f, 11.25f, 1f);
                                    PoliceAndThief.jewel09BeingStealed = null;
                                    break;
                                case 10:
                                    PoliceAndThief.jewel10.transform.SetParent(null);
                                    PoliceAndThief.jewel10.transform.position = new Vector3(4.4f, 1.75f, 1f);
                                    PoliceAndThief.jewel10BeingStealed = null;
                                    break;
                                case 11:
                                    PoliceAndThief.jewel11.transform.SetParent(null);
                                    PoliceAndThief.jewel11.transform.position = new Vector3(9.25f, 13f, 1f);
                                    PoliceAndThief.jewel11BeingStealed = null;
                                    break;
                                case 12:
                                    PoliceAndThief.jewel12.transform.SetParent(null);
                                    PoliceAndThief.jewel12.transform.position = new Vector3(13.75f, 23.5f, 1f);
                                    PoliceAndThief.jewel12BeingStealed = null;
                                    break;
                                case 13:
                                    PoliceAndThief.jewel13.transform.SetParent(null);
                                    PoliceAndThief.jewel13.transform.position = new Vector3(16, 4, 1f);
                                    PoliceAndThief.jewel13BeingStealed = null;
                                    break;
                                case 14:
                                    PoliceAndThief.jewel14.transform.SetParent(null);
                                    PoliceAndThief.jewel14.transform.position = new Vector3(15.35f, -0.9f, 1f);
                                    PoliceAndThief.jewel14BeingStealed = null;
                                    break;
                                case 15:
                                    PoliceAndThief.jewel15.transform.SetParent(null);
                                    PoliceAndThief.jewel15.transform.position = new Vector3(19.5f, -1.75f, 1f);
                                    PoliceAndThief.jewel15BeingStealed = null;
                                    break;
                            }
                            break;
                        // Polus
                        case 2:
                            switch (jewelRevertedId) {
                                case 1:
                                    PoliceAndThief.jewel01.transform.SetParent(null);
                                    PoliceAndThief.jewel01.transform.position = new Vector3(16.7f, -2.65f, 0.75f);
                                    PoliceAndThief.jewel01BeingStealed = null;
                                    break;
                                case 2:
                                    PoliceAndThief.jewel02.transform.SetParent(null);
                                    PoliceAndThief.jewel02.transform.position = new Vector3(25.35f, -7.35f, 0.75f);
                                    PoliceAndThief.jewel02BeingStealed = null;
                                    break;
                                case 3:
                                    PoliceAndThief.jewel03.transform.SetParent(null);
                                    PoliceAndThief.jewel03.transform.position = new Vector3(34.9f, -9.75f, 0.75f);
                                    PoliceAndThief.jewel03BeingStealed = null;
                                    break;
                                case 4:
                                    PoliceAndThief.jewel04.transform.SetParent(null);
                                    PoliceAndThief.jewel04.transform.position = new Vector3(36.5f, -21.75f, 0.75f);
                                    PoliceAndThief.jewel04BeingStealed = null;
                                    break;
                                case 5:
                                    PoliceAndThief.jewel05.transform.SetParent(null);
                                    PoliceAndThief.jewel05.transform.position = new Vector3(17.25f, -17.5f, 0.75f);
                                    PoliceAndThief.jewel05BeingStealed = null;
                                    break;
                                case 6:
                                    PoliceAndThief.jewel06.transform.SetParent(null);
                                    PoliceAndThief.jewel06.transform.position = new Vector3(10.9f, -20.5f, -0.75f);
                                    PoliceAndThief.jewel06BeingStealed = null;
                                    break;
                                case 7:
                                    PoliceAndThief.jewel07.transform.SetParent(null);
                                    PoliceAndThief.jewel07.transform.position = new Vector3(1.5f, -20.25f, 0.75f);
                                    PoliceAndThief.jewel07BeingStealed = null;
                                    break;
                                case 08:
                                    PoliceAndThief.jewel08.transform.SetParent(null);
                                    PoliceAndThief.jewel08.transform.position = new Vector3(3f, -12f, 0.75f);
                                    PoliceAndThief.jewel08BeingStealed = null;
                                    break;
                                case 09:
                                    PoliceAndThief.jewel09.transform.SetParent(null);
                                    PoliceAndThief.jewel09.transform.position = new Vector3(30f, -7.35f, 0.75f);
                                    PoliceAndThief.jewel09BeingStealed = null;
                                    break;
                                case 10:
                                    PoliceAndThief.jewel10.transform.SetParent(null);
                                    PoliceAndThief.jewel10.transform.position = new Vector3(40.25f, -8f, 0.75f);
                                    PoliceAndThief.jewel10BeingStealed = null;
                                    break;
                                case 11:
                                    PoliceAndThief.jewel11.transform.SetParent(null);
                                    PoliceAndThief.jewel11.transform.position = new Vector3(26f, -17.15f, 0.75f);
                                    PoliceAndThief.jewel11BeingStealed = null;
                                    break;
                                case 12:
                                    PoliceAndThief.jewel12.transform.SetParent(null);
                                    PoliceAndThief.jewel12.transform.position = new Vector3(22f, -25.25f, 0.75f);
                                    PoliceAndThief.jewel12BeingStealed = null;
                                    break;
                                case 13:
                                    PoliceAndThief.jewel13.transform.SetParent(null);
                                    PoliceAndThief.jewel13.transform.position = new Vector3(20.65f, -12f, 0.75f);
                                    PoliceAndThief.jewel13BeingStealed = null;
                                    break;
                                case 14:
                                    PoliceAndThief.jewel14.transform.SetParent(null);
                                    PoliceAndThief.jewel14.transform.position = new Vector3(9.75f, -12.25f, 0.75f);
                                    PoliceAndThief.jewel14BeingStealed = null;
                                    break;
                                case 15:
                                    PoliceAndThief.jewel15.transform.SetParent(null);
                                    PoliceAndThief.jewel15.transform.position = new Vector3(2.25f, -24f, 0.75f);
                                    PoliceAndThief.jewel15BeingStealed = null;
                                    break;
                            }
                            break;
                        // Dleks
                        case 3:
                            switch (jewelRevertedId) {
                                case 1:
                                    PoliceAndThief.jewel01.transform.SetParent(null);
                                    PoliceAndThief.jewel01.transform.position = new Vector3(18.65f, -9.9f, 1f);
                                    PoliceAndThief.jewel01BeingStealed = null;
                                    break;
                                case 2:
                                    PoliceAndThief.jewel02.transform.SetParent(null);
                                    PoliceAndThief.jewel02.transform.position = new Vector3(21.5f, -2, 1f);
                                    PoliceAndThief.jewel02BeingStealed = null;
                                    break;
                                case 3:
                                    PoliceAndThief.jewel03.transform.SetParent(null);
                                    PoliceAndThief.jewel03.transform.position = new Vector3(5.9f, -8.25f, 1f);
                                    PoliceAndThief.jewel03BeingStealed = null;
                                    break;
                                case 4:
                                    PoliceAndThief.jewel04.transform.SetParent(null);
                                    PoliceAndThief.jewel04.transform.position = new Vector3(-4.5f, -7.5f, 1f);
                                    PoliceAndThief.jewel04BeingStealed = null;
                                    break;
                                case 5:
                                    PoliceAndThief.jewel05.transform.SetParent(null);
                                    PoliceAndThief.jewel05.transform.position = new Vector3(-7.85f, -14.45f, 1f);
                                    PoliceAndThief.jewel05BeingStealed = null;
                                    break;
                                case 6:
                                    PoliceAndThief.jewel06.transform.SetParent(null);
                                    PoliceAndThief.jewel06.transform.position = new Vector3(-6.65f, -4.8f, 1f);
                                    PoliceAndThief.jewel06BeingStealed = null;
                                    break;
                                case 7:
                                    PoliceAndThief.jewel07.transform.SetParent(null);
                                    PoliceAndThief.jewel07.transform.position = new Vector3(-10.5f, 2.15f, 1f);
                                    PoliceAndThief.jewel07BeingStealed = null;
                                    break;
                                case 8:
                                    PoliceAndThief.jewel08.transform.SetParent(null);
                                    PoliceAndThief.jewel08.transform.position = new Vector3(5.5f, 3.5f, 1f);
                                    PoliceAndThief.jewel08BeingStealed = null;
                                    break;
                                case 9:
                                    PoliceAndThief.jewel09.transform.SetParent(null);
                                    PoliceAndThief.jewel09.transform.position = new Vector3(19, -1.2f, 1f);
                                    PoliceAndThief.jewel09BeingStealed = null;
                                    break;
                                case 10:
                                    PoliceAndThief.jewel10.transform.SetParent(null);
                                    PoliceAndThief.jewel10.transform.position = new Vector3(21.5f, -8.35f, 1f);
                                    PoliceAndThief.jewel10BeingStealed = null;
                                    break;
                                case 11:
                                    PoliceAndThief.jewel11.transform.SetParent(null);
                                    PoliceAndThief.jewel11.transform.position = new Vector3(12.5f, -3.75f, 1f);
                                    PoliceAndThief.jewel11BeingStealed = null;
                                    break;
                                case 12:
                                    PoliceAndThief.jewel12.transform.SetParent(null);
                                    PoliceAndThief.jewel12.transform.position = new Vector3(5.9f, -5.25f, 1f);
                                    PoliceAndThief.jewel12BeingStealed = null;
                                    break;
                                case 13:
                                    PoliceAndThief.jewel13.transform.SetParent(null);
                                    PoliceAndThief.jewel13.transform.position = new Vector3(-2.65f, -16.5f, 1f);
                                    PoliceAndThief.jewel13BeingStealed = null;
                                    break;
                                case 14:
                                    PoliceAndThief.jewel14.transform.SetParent(null);
                                    PoliceAndThief.jewel14.transform.position = new Vector3(-16.75f, -4.75f, 1f);
                                    PoliceAndThief.jewel14BeingStealed = null;
                                    break;
                                case 15:
                                    PoliceAndThief.jewel15.transform.SetParent(null);
                                    PoliceAndThief.jewel15.transform.position = new Vector3(-3.8f, 3.5f, 1f);
                                    PoliceAndThief.jewel15BeingStealed = null;
                                    break;
                            }
                            break;
                        // Airship
                        case 4:
                            switch (jewelRevertedId) {
                                case 1:
                                    PoliceAndThief.jewel01.transform.SetParent(null);
                                    PoliceAndThief.jewel01.transform.position = new Vector3(-23.5f, -1.5f, 1f);
                                    PoliceAndThief.jewel01BeingStealed = null;
                                    break;
                                case 2:
                                    PoliceAndThief.jewel02.transform.SetParent(null);
                                    PoliceAndThief.jewel02.transform.position = new Vector3(-14.15f, -4.85f, 1f);
                                    PoliceAndThief.jewel02BeingStealed = null;
                                    break;
                                case 3:
                                    PoliceAndThief.jewel03.transform.SetParent(null);
                                    PoliceAndThief.jewel03.transform.position = new Vector3(-13.9f, -16.25f, 1f);
                                    PoliceAndThief.jewel03BeingStealed = null;
                                    break;
                                case 4:
                                    PoliceAndThief.jewel04.transform.SetParent(null);
                                    PoliceAndThief.jewel04.transform.position = new Vector3(-0.85f, -2.5f, 1f);
                                    PoliceAndThief.jewel04BeingStealed = null;
                                    break;
                                case 5:
                                    PoliceAndThief.jewel05.transform.SetParent(null);
                                    PoliceAndThief.jewel05.transform.position = new Vector3(-5, 8.5f, 1f);
                                    PoliceAndThief.jewel05BeingStealed = null;
                                    break;
                                case 6:
                                    PoliceAndThief.jewel06.transform.SetParent(null);
                                    PoliceAndThief.jewel06.transform.position = new Vector3(19.3f, -4.15f, 1f);
                                    PoliceAndThief.jewel06BeingStealed = null;
                                    break;
                                case 7:
                                    PoliceAndThief.jewel07.transform.SetParent(null);
                                    PoliceAndThief.jewel07.transform.position = new Vector3(19.85f, 8, 1f);
                                    PoliceAndThief.jewel07BeingStealed = null;
                                    break;
                                case 8:
                                    PoliceAndThief.jewel08.transform.SetParent(null);
                                    PoliceAndThief.jewel08.transform.position = new Vector3(28.85f, -1.75f, 1f);
                                    PoliceAndThief.jewel08BeingStealed = null;
                                    break;
                                case 9:
                                    PoliceAndThief.jewel09.transform.SetParent(null);
                                    PoliceAndThief.jewel09.transform.position = new Vector3(-14.5f, -8.5f, 1f);
                                    PoliceAndThief.jewel09BeingStealed = null;
                                    break;
                                case 10:
                                    PoliceAndThief.jewel10.transform.SetParent(null);
                                    PoliceAndThief.jewel10.transform.position = new Vector3(6.3f, -2.75f, 1f);
                                    PoliceAndThief.jewel10BeingStealed = null;
                                    break;
                                case 11:
                                    PoliceAndThief.jewel11.transform.SetParent(null);
                                    PoliceAndThief.jewel11.transform.position = new Vector3(20.75f, 2.5f, 1f);
                                    PoliceAndThief.jewel11BeingStealed = null;
                                    break;
                                case 12:
                                    PoliceAndThief.jewel12.transform.SetParent(null);
                                    PoliceAndThief.jewel12.transform.position = new Vector3(29.25f, 7, 1f);
                                    PoliceAndThief.jewel12BeingStealed = null;
                                    break;
                                case 13:
                                    PoliceAndThief.jewel13.transform.SetParent(null);
                                    PoliceAndThief.jewel13.transform.position = new Vector3(37.5f, -3.5f, 1f);
                                    PoliceAndThief.jewel13BeingStealed = null;
                                    break;
                                case 14:
                                    PoliceAndThief.jewel14.transform.SetParent(null);
                                    PoliceAndThief.jewel14.transform.position = new Vector3(25.2f, -8.75f, 1f);
                                    PoliceAndThief.jewel14BeingStealed = null;
                                    break;
                                case 15:
                                    PoliceAndThief.jewel15.transform.SetParent(null);
                                    PoliceAndThief.jewel15.transform.position = new Vector3(16.3f, -11, 1f);
                                    PoliceAndThief.jewel15BeingStealed = null;
                                    break;
                            }
                            break;
                        // Submerged
                        case 5:
                            switch (jewelRevertedId) {
                                case 1:
                                    PoliceAndThief.jewel01.transform.SetParent(null);
                                    PoliceAndThief.jewel01.transform.position = new Vector3(-15f, 17.5f, -1f);
                                    PoliceAndThief.jewel01BeingStealed = null;
                                    break;
                                case 2:
                                    PoliceAndThief.jewel02.transform.SetParent(null);
                                    PoliceAndThief.jewel02.transform.position = new Vector3(8f, 32f, -1f);
                                    PoliceAndThief.jewel02BeingStealed = null;
                                    break;
                                case 3:
                                    PoliceAndThief.jewel03.transform.SetParent(null);
                                    PoliceAndThief.jewel03.transform.position = new Vector3(-6.75f, 10f, -1f);
                                    PoliceAndThief.jewel03BeingStealed = null;
                                    break;
                                case 4:
                                    PoliceAndThief.jewel04.transform.SetParent(null);
                                    PoliceAndThief.jewel04.transform.position = new Vector3(5.15f, 8f, -1f);
                                    PoliceAndThief.jewel04BeingStealed = null;
                                    break;
                                case 5:
                                    PoliceAndThief.jewel05.transform.SetParent(null);
                                    PoliceAndThief.jewel05.transform.position = new Vector3(5f, -33.5f, -1f);
                                    PoliceAndThief.jewel05BeingStealed = null;
                                    break;
                                case 6:
                                    PoliceAndThief.jewel06.transform.SetParent(null);
                                    PoliceAndThief.jewel06.transform.position = new Vector3(-4.15f, -33.5f, -1f);
                                    PoliceAndThief.jewel06BeingStealed = null;
                                    break;
                                case 7:
                                    PoliceAndThief.jewel07.transform.SetParent(null);
                                    PoliceAndThief.jewel07.transform.position = new Vector3(-14f, -27.75f, -1f);
                                    PoliceAndThief.jewel07BeingStealed = null;
                                    break;
                                case 8:
                                    PoliceAndThief.jewel08.transform.SetParent(null);
                                    PoliceAndThief.jewel08.transform.position = new Vector3(7.8f, -23.75f, -1f);
                                    PoliceAndThief.jewel08BeingStealed = null;
                                    break;
                                case 9:
                                    PoliceAndThief.jewel09.transform.SetParent(null);
                                    PoliceAndThief.jewel09.transform.position = new Vector3(-6.75f, -42.75f, -1f);
                                    PoliceAndThief.jewel09BeingStealed = null;
                                    break;
                                case 10:
                                    PoliceAndThief.jewel10.transform.SetParent(null);
                                    PoliceAndThief.jewel10.transform.position = new Vector3(13f, -25.25f, -1f);
                                    PoliceAndThief.jewel10BeingStealed = null;
                                    break;
                                case 11:
                                    PoliceAndThief.jewel11.transform.SetParent(null);
                                    PoliceAndThief.jewel11.transform.position = new Vector3(-14f, -34.25f, -1f);
                                    PoliceAndThief.jewel11BeingStealed = null;
                                    break;
                                case 12:
                                    PoliceAndThief.jewel12.transform.SetParent(null);
                                    PoliceAndThief.jewel12.transform.position = new Vector3(0f, -33.5f, -1f);
                                    PoliceAndThief.jewel12BeingStealed = null;
                                    break;
                                case 13:
                                    PoliceAndThief.jewel13.transform.SetParent(null);
                                    PoliceAndThief.jewel13.transform.position = new Vector3(-6.5f, 14f, -1f);
                                    PoliceAndThief.jewel13BeingStealed = null;
                                    break;
                                case 14:
                                    PoliceAndThief.jewel14.transform.SetParent(null);
                                    PoliceAndThief.jewel14.transform.position = new Vector3(14.25f, 24.5f, -1f);
                                    PoliceAndThief.jewel14BeingStealed = null;
                                    break;
                                case 15:
                                    PoliceAndThief.jewel15.transform.SetParent(null);
                                    PoliceAndThief.jewel15.transform.position = new Vector3(-12.25f, 31f, -1f);
                                    PoliceAndThief.jewel15BeingStealed = null;
                                    break;
                            }
                            break;
                    }

                    // if police can't see jewels, hide it after jailing a player
                    if (PlayerControl.LocalPlayer == PoliceAndThief.policeplayer01 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer02 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer03 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer04 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer05 || PlayerControl.LocalPlayer == PoliceAndThief.policeplayer06) {
                        if (!PoliceAndThief.policeCanSeeJewels) {
                            switch (jewelRevertedId) {
                                case 1:
                                    PoliceAndThief.jewel01.SetActive(false);
                                    break;
                                case 2:
                                    PoliceAndThief.jewel02.SetActive(false);
                                    break;
                                case 3:
                                    PoliceAndThief.jewel03.SetActive(false);
                                    break;
                                case 4:
                                    PoliceAndThief.jewel04.SetActive(false);
                                    break;
                                case 5:
                                    PoliceAndThief.jewel05.SetActive(false);
                                    break;
                                case 6:
                                    PoliceAndThief.jewel06.SetActive(false);
                                    break;
                                case 7:
                                    PoliceAndThief.jewel07.SetActive(false);
                                    break;
                                case 8:
                                    PoliceAndThief.jewel08.SetActive(false);
                                    break;
                                case 9:
                                    PoliceAndThief.jewel09.SetActive(false);
                                    break;
                                case 10:
                                    PoliceAndThief.jewel10.SetActive(false);
                                    break;
                                case 11:
                                    PoliceAndThief.jewel11.SetActive(false);
                                    break;
                                case 12:
                                    PoliceAndThief.jewel12.SetActive(false);
                                    break;
                                case 13:
                                    PoliceAndThief.jewel13.SetActive(false);
                                    break;
                                case 14:
                                    PoliceAndThief.jewel14.SetActive(false);
                                    break;
                                case 15:
                                    PoliceAndThief.jewel15.SetActive(false);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void policeandThiefsTased(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    if (PlayerControl.LocalPlayer == player) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.policeTaser, false, 100f);
                        if (MapBehaviour.Instance) {
                            MapBehaviour.Instance.Close();
                        }
                    }
                    new Tased(PoliceAndThief.policeTaseDuration, player);
                    if (PoliceAndThief.thiefplayer01 != null && targetId == PoliceAndThief.thiefplayer01.PlayerId) {
                        if (PoliceAndThief.thiefplayer01IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer01JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer02 != null && targetId == PoliceAndThief.thiefplayer02.PlayerId) {
                        if (PoliceAndThief.thiefplayer02IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer02JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer03 != null && targetId == PoliceAndThief.thiefplayer03.PlayerId) {
                        if (PoliceAndThief.thiefplayer03IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer03JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer04 != null && targetId == PoliceAndThief.thiefplayer04.PlayerId) {
                        if (PoliceAndThief.thiefplayer04IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer04JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer05 != null && targetId == PoliceAndThief.thiefplayer05.PlayerId) {
                        if (PoliceAndThief.thiefplayer05IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer05JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer06 != null && targetId == PoliceAndThief.thiefplayer06.PlayerId) {
                        if (PoliceAndThief.thiefplayer06IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer06JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer07 != null && targetId == PoliceAndThief.thiefplayer07.PlayerId) {
                        if (PoliceAndThief.thiefplayer07IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer07JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer08 != null && targetId == PoliceAndThief.thiefplayer08.PlayerId) {
                        if (PoliceAndThief.thiefplayer08IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer08JewelId);
                        }
                    }
                    else if (PoliceAndThief.thiefplayer09 != null && targetId == PoliceAndThief.thiefplayer09.PlayerId) {
                        if (PoliceAndThief.thiefplayer09IsStealing) {
                            policeandThiefRevertedJewelPosition(targetId, PoliceAndThief.thiefplayer09JewelId);
                        }
                    }
                    return;
                }
            }
        }

        public static void kingOfTheHillKills(byte targetId, int whichplayer) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    switch (whichplayer) {
                        case 1:
                            KingOfTheHill.greenplayer01.MurderPlayer(player);
                            break;
                        case 2:
                            KingOfTheHill.greenplayer02.MurderPlayer(player);
                            break;
                        case 3:
                            KingOfTheHill.greenplayer03.MurderPlayer(player);
                            break;
                        case 4:
                            KingOfTheHill.greenplayer04.MurderPlayer(player);
                            break;
                        case 5:
                            KingOfTheHill.greenplayer05.MurderPlayer(player);
                            break;
                        case 6:
                            KingOfTheHill.greenplayer06.MurderPlayer(player);
                            break;
                        case 7:
                            KingOfTheHill.greenKingplayer.MurderPlayer(player);
                            break;
                        case 9:
                            KingOfTheHill.yellowplayer01.MurderPlayer(player);
                            break;
                        case 10:
                            KingOfTheHill.yellowplayer02.MurderPlayer(player);
                            break;
                        case 11:
                            KingOfTheHill.yellowplayer03.MurderPlayer(player);
                            break;
                        case 12:
                            KingOfTheHill.yellowplayer04.MurderPlayer(player);
                            break;
                        case 13:
                            KingOfTheHill.yellowplayer05.MurderPlayer(player);
                            break;
                        case 14:
                            KingOfTheHill.yellowplayer06.MurderPlayer(player);
                            break;
                        case 15:
                            if (player.PlayerId == KingOfTheHill.greenKingplayer.PlayerId) {
                                KingOfTheHill.greenTeam.Remove(KingOfTheHill.greenKingplayer);
                                KingOfTheHill.greenKingplayer = KingOfTheHill.usurperPlayer;
                                KingOfTheHill.greenTeam.Add(KingOfTheHill.usurperPlayer);
                                KingOfTheHill.usurperPlayer = player;
                                if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                                    KingOfTheHill.greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, -0.5f);
                                }
                                else {
                                    KingOfTheHill.greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                                }
                                KingOfTheHill.greenkingaura.transform.parent = KingOfTheHill.greenKingplayer.transform;
                                if (PlayerControl.LocalPlayer == KingOfTheHill.greenKingplayer) {
                                    new CustomMessage(Language.statusKingOfTheHillTexts[0], 5, -1, 1.6f, 11);
                                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[4].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[5].arrow.SetActive(false);
                                }
                                KingOfTheHill.greenKingplayer.MurderPlayer(KingOfTheHill.usurperPlayer);
                                if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[4].arrow.SetActive(true);
                                    KingOfTheHill.localArrows[5].arrow.SetActive(true);
                                }
                            }
                            else if (player.PlayerId == KingOfTheHill.yellowKingplayer.PlayerId) {
                                KingOfTheHill.yellowTeam.Remove(KingOfTheHill.yellowKingplayer);
                                KingOfTheHill.yellowKingplayer = KingOfTheHill.usurperPlayer;
                                KingOfTheHill.yellowTeam.Add(KingOfTheHill.usurperPlayer);
                                KingOfTheHill.usurperPlayer = player;
                                if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                                    KingOfTheHill.yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, -0.5f);
                                }
                                else {
                                    KingOfTheHill.yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                                }
                                KingOfTheHill.yellowkingaura.transform.parent = KingOfTheHill.yellowKingplayer.transform;
                                if (PlayerControl.LocalPlayer == KingOfTheHill.yellowKingplayer) {
                                    new CustomMessage(Language.statusKingOfTheHillTexts[1], 5, -1, 1.6f, 11);
                                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[4].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[5].arrow.SetActive(false);
                                }
                                KingOfTheHill.yellowKingplayer.MurderPlayer(KingOfTheHill.usurperPlayer);
                                if (PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer) {
                                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[4].arrow.SetActive(true);
                                    KingOfTheHill.localArrows[5].arrow.SetActive(true);
                                }
                            }
                            else {
                                KingOfTheHill.usurperPlayer.MurderPlayer(player);
                            }
                            break;
                        case 16:
                            KingOfTheHill.yellowKingplayer.MurderPlayer(player);
                            break;
                    }
                    return;
                }
            }
        }

        public static void kingoftheHillCapture(int whichzone, int whichking) {

            // Green team
            if (whichking == 1) {
                switch (whichzone) {
                    case 1:
                        if (KingOfTheHill.yellowKinghaszoneone) {
                            KingOfTheHill.yellowKinghaszoneone = false;
                            KingOfTheHill.totalYellowKingzonescaptured -= 1;
                        }
                        KingOfTheHill.flagzoneone.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.greenflag.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zoneone.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.greenbase.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zoneonecolor = Color.green;
                        KingOfTheHill.greenKinghaszoneone = true;
                        KingOfTheHill.yellowteamAlerted = false;
                        KingOfTheHill.totalGreenKingzonescaptured += 1;
                        break;
                    case 2:
                        if (KingOfTheHill.yellowKinghaszonetwo) {
                            KingOfTheHill.yellowKinghaszonetwo = false;
                            KingOfTheHill.totalYellowKingzonescaptured -= 1;
                        }
                        KingOfTheHill.flagzonetwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.greenflag.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zonetwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.greenbase.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zonetwocolor = Color.green;
                        KingOfTheHill.greenKinghaszonetwo = true;
                        KingOfTheHill.yellowteamAlerted = false;
                        KingOfTheHill.totalGreenKingzonescaptured += 1;
                        break;
                    case 3:
                        if (KingOfTheHill.yellowKinghaszonethree) {
                            KingOfTheHill.yellowKinghaszonethree = false;
                            KingOfTheHill.totalYellowKingzonescaptured -= 1;
                        }
                        KingOfTheHill.flagzonethree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.greenflag.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zonethree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.greenbase.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zonethreecolor = Color.green;
                        KingOfTheHill.greenKinghaszonethree = true;
                        KingOfTheHill.yellowteamAlerted = false;
                        KingOfTheHill.totalGreenKingzonescaptured += 1;
                        break;
                }

                // Alert yellow team players
                if (!KingOfTheHill.yellowteamAlerted) {
                    KingOfTheHill.yellowteamAlerted = true;
                    foreach (PlayerControl yellowplayer in KingOfTheHill.yellowTeam) {
                        if (yellowplayer == PlayerControl.LocalPlayer && yellowplayer != null) {
                            new CustomMessage(Language.statusKingOfTheHillTexts[2], 5, -1, 1.3f, 11);
                        }
                    }
                }
            }

            // Yellow team
            if (whichking == 2) {
                switch (whichzone) {
                    case 1:
                        if (KingOfTheHill.greenKinghaszoneone) {
                            KingOfTheHill.greenKinghaszoneone = false;
                            KingOfTheHill.totalGreenKingzonescaptured -= 1;
                        }
                        KingOfTheHill.flagzoneone.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.yellowflag.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zoneone.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.yellowbase.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zoneonecolor = Color.yellow;
                        KingOfTheHill.yellowKinghaszoneone = true;
                        KingOfTheHill.greenteamAlerted = false;
                        KingOfTheHill.totalYellowKingzonescaptured += 1;
                        break;
                    case 2:
                        if (KingOfTheHill.greenKinghaszonetwo) {
                            KingOfTheHill.greenKinghaszonetwo = false;
                            KingOfTheHill.totalGreenKingzonescaptured -= 1;
                        }
                        KingOfTheHill.flagzonetwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.yellowflag.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zonetwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.yellowbase.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zonetwocolor = Color.yellow;
                        KingOfTheHill.yellowKinghaszonetwo = true;
                        KingOfTheHill.greenteamAlerted = false;
                        KingOfTheHill.totalYellowKingzonescaptured += 1;
                        break;
                    case 3:
                        if (KingOfTheHill.greenKinghaszonethree) {
                            KingOfTheHill.greenKinghaszonethree = false;
                            KingOfTheHill.totalGreenKingzonescaptured -= 1;
                        }
                        KingOfTheHill.flagzonethree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.yellowflag.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zonethree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.yellowbase.GetComponent<SpriteRenderer>().sprite;
                        KingOfTheHill.zonethreecolor = Color.yellow;
                        KingOfTheHill.yellowKinghaszonethree = true;
                        KingOfTheHill.greenteamAlerted = false;
                        KingOfTheHill.totalYellowKingzonescaptured += 1;
                        break;
                }

                // Alert green team players
                if (!KingOfTheHill.greenteamAlerted) {
                    KingOfTheHill.greenteamAlerted = true;
                    foreach (PlayerControl greenplayer in KingOfTheHill.greenTeam) {
                        if (greenplayer == PlayerControl.LocalPlayer && greenplayer != null) {
                            new CustomMessage(Language.statusKingOfTheHillTexts[3], 5, -1, 1.3f, 11);
                        }
                    }
                }
            }
        }

        public static PlayerControl oldHotPotato = null;

        public static void hotPotatoTransfer(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId && HotPotato.hotPotatoPlayer != null) {

                    if (!HotPotato.firstPotatoTransfered) {
                        HotPotato.firstPotatoTransfered = true;
                        new CustomMessage(Language.introTexts[5], HotPotato.matchDuration, -1, -1f, 18);
                        new CustomMessage(Language.introTexts[1], HotPotato.matchDuration, -1, -1.3f, 15);
                        HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + HotPotato.notPotatoTeam.Count + "</color>";
                        new CustomMessage(HotPotato.hotpotatopointCounter, HotPotato.matchDuration, -1, 1.9f, 17);
                    }

                    if (HotPotato.resetTimeForTransfer) {
                        HotPotato.timeforTransfer = HotPotato.savedtimeforTransfer + 3f;
                    }
                    else {
                        HotPotato.timeforTransfer = (HotPotato.timeforTransfer + HotPotato.increaseTimeIfNoReset + 3f);
                    }

                    oldHotPotato = HotPotato.hotPotatoPlayer;

                    HotPotato.notPotatoTeam.Add(oldHotPotato);

                    // Switch role
                    if (HotPotato.notPotato01 != null && HotPotato.notPotato01 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato01);
                        HotPotato.notPotato01 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato02 != null && HotPotato.notPotato02 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato02);
                        HotPotato.notPotato02 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato03 != null && HotPotato.notPotato03 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato03);
                        HotPotato.notPotato03 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato04 != null && HotPotato.notPotato04 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato04);
                        HotPotato.notPotato04 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato05 != null && HotPotato.notPotato05 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato05);
                        HotPotato.notPotato05 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato06 != null && HotPotato.notPotato06 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato06);
                        HotPotato.notPotato06 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato07 != null && HotPotato.notPotato07 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato07);
                        HotPotato.notPotato07 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato08 != null && HotPotato.notPotato08 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato08);
                        HotPotato.notPotato08 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato09 != null && HotPotato.notPotato09 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato09);
                        HotPotato.notPotato09 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato10 != null && HotPotato.notPotato10 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato10);
                        HotPotato.notPotato10 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato11 != null && HotPotato.notPotato11 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato11);
                        HotPotato.notPotato11 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato12 != null && HotPotato.notPotato12 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato12);
                        HotPotato.notPotato12 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato13 != null && HotPotato.notPotato13 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato13);
                        HotPotato.notPotato13 = oldHotPotato;
                    }
                    else if (HotPotato.notPotato14 != null && HotPotato.notPotato14 == player) {
                        HotPotato.notPotatoTeam.Remove(HotPotato.notPotato14);
                        HotPotato.notPotato14 = oldHotPotato;
                    }

                    HotPotato.hotPotatoPlayer = player;
                    HotPotato.hotPotatoPlayer.NetTransform.Halt();
                    HotPotato.hotPotatoPlayer.moveable = false;
                    HotPotato.hotPotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                    HotPotato.hotPotato.transform.parent = HotPotato.hotPotatoPlayer.transform;

                    HudManager.Instance.StartCoroutine(Effects.Lerp(3, new Action<float>((p) => { // Delayed action
                        if (p == 1f) {
                            HotPotato.hotPotatoPlayer.moveable = true;
                        }
                    })));

                    int notPotatosAlives = 0;
                    HotPotato.notPotatoTeamAlive.Clear();
                    foreach (PlayerControl notPotato in HotPotato.notPotatoTeam) {
                        if (!notPotato.Data.IsDead) {
                            notPotatosAlives += 1;
                            HotPotato.notPotatoTeamAlive.Add(notPotato);
                        }
                    }

                    new CustomMessage("<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color>" + Language.statusHotPotatoTexts[0], 5, -1, 1f, 16);

                    HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + HotPotato.notPotatoTeam.Count + "</color>";

                    // Set custom cooldown to the hotpotato button
                    hotPotatoButton.Timer = HotPotato.transferCooldown;
                    if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer || PlayerControl.LocalPlayer == oldHotPotato)
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.roleThiefStealRole, false, 100f);
                }
            }
        }

        public static void zombieTakeKeyItem(byte survivorWhoTookTheKey, byte keyId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                // survivor player take key item
                if (player.PlayerId == survivorWhoTookTheKey) {
                    if (ZombieLaboratory.survivorPlayer01 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer01.PlayerId) {
                        ZombieLaboratory.survivorPlayer01HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer01FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer02 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer02.PlayerId) {
                        ZombieLaboratory.survivorPlayer02HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer02FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer03 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer03.PlayerId) {
                        ZombieLaboratory.survivorPlayer03HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer03FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer04 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer04.PlayerId) {
                        ZombieLaboratory.survivorPlayer04HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer04FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer05 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer05.PlayerId) {
                        ZombieLaboratory.survivorPlayer05HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer05FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer06 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer06.PlayerId) {
                        ZombieLaboratory.survivorPlayer06HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer06FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer07 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer07.PlayerId) {
                        ZombieLaboratory.survivorPlayer07HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer07FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer08 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer08.PlayerId) {
                        ZombieLaboratory.survivorPlayer08HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer08FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer09 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer09.PlayerId) {
                        ZombieLaboratory.survivorPlayer09HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer09FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer10 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer10.PlayerId) {
                        ZombieLaboratory.survivorPlayer10HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer10FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer11 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer11.PlayerId) {
                        ZombieLaboratory.survivorPlayer11HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer11FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer12 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer12.PlayerId) {
                        ZombieLaboratory.survivorPlayer12HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer12FoundBox = keyId;
                    }
                    else if (ZombieLaboratory.survivorPlayer13 != null && survivorWhoTookTheKey == ZombieLaboratory.survivorPlayer13.PlayerId) {
                        ZombieLaboratory.survivorPlayer13HasKeyItem = true;
                        ZombieLaboratory.survivorPlayer13FoundBox = keyId;
                    }
                    switch (keyId) {
                        case 1:
                            ZombieLaboratory.keyItem01BeingHeld = true;
                            ZombieLaboratory.laboratoryKeyItem01.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.keyItem01.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.laboratoryKeyItem01.transform.parent = player.transform;
                            ZombieLaboratory.laboratoryKeyItem01.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            ZombieLaboratory.laboratoryKeyItem01.SetActive(true);
                            break;
                        case 2:
                            ZombieLaboratory.keyItem02BeingHeld = true;
                            ZombieLaboratory.laboratoryKeyItem02.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.keyItem02.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.laboratoryKeyItem02.transform.parent = player.transform;
                            ZombieLaboratory.laboratoryKeyItem02.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            ZombieLaboratory.laboratoryKeyItem02.SetActive(true);
                            break;
                        case 3:
                            ZombieLaboratory.keyItem03BeingHeld = true;
                            ZombieLaboratory.laboratoryKeyItem03.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.keyItem03.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.laboratoryKeyItem03.transform.parent = player.transform;
                            ZombieLaboratory.laboratoryKeyItem03.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            ZombieLaboratory.laboratoryKeyItem03.SetActive(true);
                            break;
                        case 4:
                            ZombieLaboratory.keyItem04BeingHeld = true;
                            ZombieLaboratory.laboratoryKeyItem04.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.keyItem04.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.laboratoryKeyItem04.transform.parent = player.transform;
                            ZombieLaboratory.laboratoryKeyItem04.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            ZombieLaboratory.laboratoryKeyItem04.SetActive(true);
                            break;
                        case 5:
                            ZombieLaboratory.keyItem05BeingHeld = true;
                            ZombieLaboratory.laboratoryKeyItem05.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.keyItem05.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.laboratoryKeyItem05.transform.parent = player.transform;
                            ZombieLaboratory.laboratoryKeyItem05.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            ZombieLaboratory.laboratoryKeyItem05.SetActive(true);
                            break;
                        case 6:
                            ZombieLaboratory.keyItem06BeingHeld = true;
                            ZombieLaboratory.laboratoryKeyItem06.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.keyItem06.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.laboratoryKeyItem06.transform.parent = player.transform;
                            ZombieLaboratory.laboratoryKeyItem06.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                            ZombieLaboratory.laboratoryKeyItem06.SetActive(true);
                            break;
                    }
                }

            }
        }

        public static void zombieDeliverKeyItem(byte survivorWhoHasKey, byte keyId) {
            foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                // survivor deliver keyitem
                if (player.PlayerId == survivorWhoHasKey) {
                    if (ZombieLaboratory.survivorPlayer01 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer01.PlayerId) {
                        ZombieLaboratory.survivorPlayer01HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer01FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer02 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer02.PlayerId) {
                        ZombieLaboratory.survivorPlayer02HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer02FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer03 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer03.PlayerId) {
                        ZombieLaboratory.survivorPlayer03HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer03FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer04 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer04.PlayerId) {
                        ZombieLaboratory.survivorPlayer04HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer04FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer05 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer05.PlayerId) {
                        ZombieLaboratory.survivorPlayer05HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer05FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer06 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer06.PlayerId) {
                        ZombieLaboratory.survivorPlayer06HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer06FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer07 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer07.PlayerId) {
                        ZombieLaboratory.survivorPlayer07HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer07FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer08 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer08.PlayerId) {
                        ZombieLaboratory.survivorPlayer08HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer08FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer09 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer09.PlayerId) {
                        ZombieLaboratory.survivorPlayer09HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer09FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer10 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer10.PlayerId) {
                        ZombieLaboratory.survivorPlayer10HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer10FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer11 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer11.PlayerId) {
                        ZombieLaboratory.survivorPlayer11HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer11FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer12 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer12.PlayerId) {
                        ZombieLaboratory.survivorPlayer12HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer12FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer13 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer13.PlayerId) {
                        ZombieLaboratory.survivorPlayer13HasKeyItem = false;
                        ZombieLaboratory.survivorPlayer13FoundBox = 0;
                    }
                    GameObject myKeyItem = null;
                    switch (keyId) {
                        case 1:
                            myKeyItem = ZombieLaboratory.laboratoryKeyItem01;
                            ZombieLaboratory.keyItem01BeingHeld = true;
                            break;
                        case 2:
                            myKeyItem = ZombieLaboratory.laboratoryKeyItem02;
                            ZombieLaboratory.keyItem02BeingHeld = true;
                            break;
                        case 3:
                            myKeyItem = ZombieLaboratory.laboratoryKeyItem03;
                            ZombieLaboratory.keyItem03BeingHeld = true;
                            break;
                        case 4:
                            myKeyItem = ZombieLaboratory.laboratoryKeyItem04;
                            ZombieLaboratory.keyItem04BeingHeld = true;
                            break;
                        case 5:
                            myKeyItem = ZombieLaboratory.laboratoryKeyItem05;
                            ZombieLaboratory.keyItem05BeingHeld = true;
                            break;
                        case 6:
                            myKeyItem = ZombieLaboratory.laboratoryKeyItem06;
                            ZombieLaboratory.keyItem06BeingHeld = true;
                            break;
                    }
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        // Skeld
                        case 0:
                            if (activatedSensei) {
                                switch (myKeyItem.name) {
                                    case "keyItem01":
                                        myKeyItem.transform.position = new Vector3(-13.5f, 8.25f, -0.2f);
                                        break;
                                    case "keyItem02":
                                        myKeyItem.transform.position = new Vector3(-13.15f, 8.25f, -0.2f);
                                        break;
                                    case "keyItem03":
                                        myKeyItem.transform.position = new Vector3(-12.8f, 8.25f, -0.2f);
                                        break;
                                    case "keyItem04":
                                        myKeyItem.transform.position = new Vector3(-12.45f, 8.25f, -0.2f);
                                        break;
                                    case "keyItem05":
                                        myKeyItem.transform.position = new Vector3(-12.1f, 8.25f, -0.2f);
                                        break;
                                    case "keyItem06":
                                        myKeyItem.transform.position = new Vector3(-11.85f, 8.25f, -0.2f);
                                        break;
                                }
                            }
                            else {
                                switch (myKeyItem.name) {
                                    case "keyItem01":
                                        myKeyItem.transform.position = new Vector3(-11.85f, 4.5f, -0.2f);
                                        break;
                                    case "keyItem02":
                                        myKeyItem.transform.position = new Vector3(-11.5f, 4.5f, -0.2f);
                                        break;
                                    case "keyItem03":
                                        myKeyItem.transform.position = new Vector3(-11.15f, 4.5f, -0.2f);
                                        break;
                                    case "keyItem04":
                                        myKeyItem.transform.position = new Vector3(-10.8f, 4.5f, -0.2f);
                                        break;
                                    case "keyItem05":
                                        myKeyItem.transform.position = new Vector3(-10.45f, 4.5f, -0.2f);
                                        break;
                                    case "keyItem06":
                                        myKeyItem.transform.position = new Vector3(-10.1f, 4.5f, -0.2f);
                                        break;
                                }
                            }
                            break;
                        // MiraHQ
                        case 1:
                            switch (myKeyItem.name) {
                                case "keyItem01":
                                    myKeyItem.transform.position = new Vector3(0.25f, 2.15f, -0.2f);
                                    break;
                                case "keyItem02":
                                    myKeyItem.transform.position = new Vector3(0.6f, 2.15f, -0.2f);
                                    break;
                                case "keyItem03":
                                    myKeyItem.transform.position = new Vector3(0.95f, 2.15f, -0.2f);
                                    break;
                                case "keyItem04":
                                    myKeyItem.transform.position = new Vector3(1.3f, 2.15f, -0.2f);
                                    break;
                                case "keyItem05":
                                    myKeyItem.transform.position = new Vector3(1.65f, 2.15f, -0.2f);
                                    break;
                                case "keyItem06":
                                    myKeyItem.transform.position = new Vector3(2f, 2.15f, -0.2f);
                                    break;
                            }
                            break;
                        // Polus
                        case 2:
                            switch (myKeyItem.name) {
                                case "keyItem01":
                                    myKeyItem.transform.position = new Vector3(15.2f, -1.5f, -0.2f);
                                    break;
                                case "keyItem02":
                                    myKeyItem.transform.position = new Vector3(15.55f, -1.5f, -0.2f);
                                    break;
                                case "keyItem03":
                                    myKeyItem.transform.position = new Vector3(15.9f, -1.5f, -0.2f);
                                    break;
                                case "keyItem04":
                                    myKeyItem.transform.position = new Vector3(16.25f, -1.5f, -0.2f);
                                    break;
                                case "keyItem05":
                                    myKeyItem.transform.position = new Vector3(16.6f, -1.5f, -0.2f);
                                    break;
                                case "keyItem06":
                                    myKeyItem.transform.position = new Vector3(16.95f, -1.5f, -0.2f);
                                    break;
                            }
                            break;
                        // Dleks
                        case 3:
                            switch (myKeyItem.name) {
                                case "keyItem01":
                                    myKeyItem.transform.position = new Vector3(11.85f, 4.5f, -0.2f);
                                    break;
                                case "keyItem02":
                                    myKeyItem.transform.position = new Vector3(11.5f, 4.5f, -0.2f);
                                    break;
                                case "keyItem03":
                                    myKeyItem.transform.position = new Vector3(11.15f, 4.5f, -0.2f);
                                    break;
                                case "keyItem04":
                                    myKeyItem.transform.position = new Vector3(10.8f, 4.5f, -0.2f);
                                    break;
                                case "keyItem05":
                                    myKeyItem.transform.position = new Vector3(10.45f, 4.5f, -0.2f);
                                    break;
                                case "keyItem06":
                                    myKeyItem.transform.position = new Vector3(10.1f, 4.5f, -0.2f);
                                    break;
                            }
                            break;
                        // Airship
                        case 4:
                            switch (myKeyItem.name) {
                                case "keyItem01":
                                    myKeyItem.transform.position = new Vector3(-19.95f, 4f, -0.2f);
                                    break;
                                case "keyItem02":
                                    myKeyItem.transform.position = new Vector3(-19.6f, 4f, -0.2f);
                                    break;
                                case "keyItem03":
                                    myKeyItem.transform.position = new Vector3(-19.25f, 4f, -0.2f);
                                    break;
                                case "keyItem04":
                                    myKeyItem.transform.position = new Vector3(-18.9f, 4f, -0.2f);
                                    break;
                                case "keyItem05":
                                    myKeyItem.transform.position = new Vector3(-18.55f, 4f, -0.2f);
                                    break;
                                case "keyItem06":
                                    myKeyItem.transform.position = new Vector3(-18.2f, 4f, -0.2f);
                                    break;
                            }
                            break;
                        // Submerged
                        case 5:
                            switch (myKeyItem.name) {
                                case "keyItem01":
                                    if (myKeyItem.transform.position.y > 0) {
                                        myKeyItem.transform.position = new Vector3(-7.45f, 32.9f, -1f);
                                    }
                                    else {
                                        myKeyItem.transform.position = new Vector3(-15.65f, -37.95f, -1f);
                                    }
                                    break;
                                case "keyItem02":
                                    if (myKeyItem.transform.position.y > 0) {
                                        myKeyItem.transform.position = new Vector3(-7.1f, 32.9f, -1f);
                                    }
                                    else {
                                        myKeyItem.transform.position = new Vector3(-15.3f, -37.95f, -1f);
                                    }
                                    break;
                                case "keyItem03":
                                    if (myKeyItem.transform.position.y > 0) {
                                        myKeyItem.transform.position = new Vector3(-6.75f, 32.9f, -1f);
                                    }
                                    else {
                                        myKeyItem.transform.position = new Vector3(-14.95f, -37.95f, -1f);
                                    }
                                    break;
                                case "keyItem04":
                                    if (myKeyItem.transform.position.y > 0) {
                                        myKeyItem.transform.position = new Vector3(-6.4f, 32.9f, -1f);
                                    }
                                    else {
                                        myKeyItem.transform.position = new Vector3(-14.6f, -37.95f, -1f);
                                    }
                                    break;
                                case "keyItem05":
                                    if (myKeyItem.transform.position.y > 0) {
                                        myKeyItem.transform.position = new Vector3(-6.05f, 32.9f, -1f);
                                    }
                                    else {
                                        myKeyItem.transform.position = new Vector3(-14.25f, -37.95f, -1f);
                                    }
                                    break;
                                case "keyItem06":
                                    if (myKeyItem.transform.position.y > 0) {
                                        myKeyItem.transform.position = new Vector3(-5.7f, 32.9f, -1f);
                                    }
                                    else {
                                        myKeyItem.transform.position = new Vector3(-13.9f, -37.95f, -1f);
                                    }
                                    break;
                            }
                            break;
                    }
                    myKeyItem.transform.SetParent(null);
                }
            }

            ZombieLaboratory.currentKeyItems += 1;
            new CustomMessage(Language.statusZombieLaboratoryTexts[0], 5, -1, 1.6f, 21);
            ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
            if (ZombieLaboratory.currentKeyItems >= 6) {
                ZombieLaboratory.nursePlayerHasCureReady = true;
            }
        }

        public static void zombieLaboratoryRevertedKeyPosition(byte survivorWhoHasKey, byte keyRevertedId) {
            foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                if (player.PlayerId == survivorWhoHasKey) {
                    if (ZombieLaboratory.survivorPlayer01 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer01.PlayerId) {
                        ZombieLaboratory.survivorPlayer01FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer02 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer02.PlayerId) {
                        ZombieLaboratory.survivorPlayer02FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer03 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer03.PlayerId) {
                        ZombieLaboratory.survivorPlayer03FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer04 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer04.PlayerId) {
                        ZombieLaboratory.survivorPlayer04FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer05 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer05.PlayerId) {
                        ZombieLaboratory.survivorPlayer05FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer06 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer06.PlayerId) {
                        ZombieLaboratory.survivorPlayer06FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer07 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer07.PlayerId) {
                        ZombieLaboratory.survivorPlayer07FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer08 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer08.PlayerId) {
                        ZombieLaboratory.survivorPlayer08FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer09 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer09.PlayerId) {
                        ZombieLaboratory.survivorPlayer09FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer10 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer10.PlayerId) {
                        ZombieLaboratory.survivorPlayer10FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer11 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer11.PlayerId) {
                        ZombieLaboratory.survivorPlayer11FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer12 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer12.PlayerId) {
                        ZombieLaboratory.survivorPlayer12FoundBox = 0;
                    }
                    else if (ZombieLaboratory.survivorPlayer13 != null && survivorWhoHasKey == ZombieLaboratory.survivorPlayer13.PlayerId) {
                        ZombieLaboratory.survivorPlayer13FoundBox = 0;
                    }

                    switch (keyRevertedId) {
                        case 1:
                            ZombieLaboratory.laboratoryKeyItem01.transform.SetParent(null);
                            ZombieLaboratory.laboratoryKeyItem01.transform.position = ZombieLaboratory.susBoxPositions[0];
                            ZombieLaboratory.keyItem01BeingHeld = false;
                            break;
                        case 2:
                            ZombieLaboratory.laboratoryKeyItem02.transform.SetParent(null);
                            ZombieLaboratory.laboratoryKeyItem02.transform.position = ZombieLaboratory.susBoxPositions[1];
                            ZombieLaboratory.keyItem02BeingHeld = false;
                            break;
                        case 3:
                            ZombieLaboratory.laboratoryKeyItem03.transform.SetParent(null);
                            ZombieLaboratory.laboratoryKeyItem03.transform.position = ZombieLaboratory.susBoxPositions[2];
                            ZombieLaboratory.keyItem03BeingHeld = false;
                            break;
                        case 4:
                            ZombieLaboratory.laboratoryKeyItem04.transform.SetParent(null);
                            ZombieLaboratory.laboratoryKeyItem04.transform.position = ZombieLaboratory.susBoxPositions[3];
                            ZombieLaboratory.keyItem04BeingHeld = false;
                            break;
                        case 5:
                            ZombieLaboratory.laboratoryKeyItem05.transform.SetParent(null);
                            ZombieLaboratory.laboratoryKeyItem05.transform.position = ZombieLaboratory.susBoxPositions[4];
                            ZombieLaboratory.keyItem05BeingHeld = false;
                            break;
                        case 6:
                            ZombieLaboratory.laboratoryKeyItem06.transform.SetParent(null);
                            ZombieLaboratory.laboratoryKeyItem06.transform.position = ZombieLaboratory.susBoxPositions[5];
                            ZombieLaboratory.keyItem06BeingHeld = false;
                            break;
                    }

                    // zombies can't see key items, hide it after infecting a player
                    foreach (PlayerControl zombie in ZombieLaboratory.zombieTeam) {
                        if (zombie != null & zombie == PlayerControl.LocalPlayer) {
                            switch (keyRevertedId) {
                                case 1:
                                    ZombieLaboratory.laboratoryKeyItem01.SetActive(false);
                                    break;
                                case 2:
                                    ZombieLaboratory.laboratoryKeyItem02.SetActive(false);
                                    break;
                                case 3:
                                    ZombieLaboratory.laboratoryKeyItem03.SetActive(false);
                                    break;
                                case 4:
                                    ZombieLaboratory.laboratoryKeyItem04.SetActive(false);
                                    break;
                                case 5:
                                    ZombieLaboratory.laboratoryKeyItem05.SetActive(false);
                                    break;
                                case 6:
                                    ZombieLaboratory.laboratoryKeyItem06.SetActive(false);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void zombieInfect(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {

                    // Assign infect bool
                    if (ZombieLaboratory.survivorPlayer01 != null && player == ZombieLaboratory.survivorPlayer01) {
                        if (!ZombieLaboratory.survivorPlayer01IsInfected) {
                            ZombieLaboratory.survivorPlayer01IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer01);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer02 != null && player == ZombieLaboratory.survivorPlayer02) {
                        if (!ZombieLaboratory.survivorPlayer02IsInfected) {
                            ZombieLaboratory.survivorPlayer02IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer02);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer03 != null && player == ZombieLaboratory.survivorPlayer03) {
                        if (!ZombieLaboratory.survivorPlayer03IsInfected) {
                            ZombieLaboratory.survivorPlayer03IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer03);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer04 != null && player == ZombieLaboratory.survivorPlayer04) {
                        if (!ZombieLaboratory.survivorPlayer04IsInfected) {
                            ZombieLaboratory.survivorPlayer04IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer04);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer05 != null && player == ZombieLaboratory.survivorPlayer05) {
                        if (!ZombieLaboratory.survivorPlayer05IsInfected) {
                            ZombieLaboratory.survivorPlayer05IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer05);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer06 != null && player == ZombieLaboratory.survivorPlayer06) {
                        if (!ZombieLaboratory.survivorPlayer06IsInfected) {
                            ZombieLaboratory.survivorPlayer06IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer06);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer07 != null && player == ZombieLaboratory.survivorPlayer07) {
                        if (!ZombieLaboratory.survivorPlayer07IsInfected) {
                            ZombieLaboratory.survivorPlayer07IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer07);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer08 != null && player == ZombieLaboratory.survivorPlayer08) {
                        if (!ZombieLaboratory.survivorPlayer08IsInfected) {
                            ZombieLaboratory.survivorPlayer08IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer08);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer09 != null && player == ZombieLaboratory.survivorPlayer09) {
                        if (!ZombieLaboratory.survivorPlayer09IsInfected) {
                            ZombieLaboratory.survivorPlayer09IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer09);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer10 != null && player == ZombieLaboratory.survivorPlayer10) {
                        if (!ZombieLaboratory.survivorPlayer10IsInfected) {
                            ZombieLaboratory.survivorPlayer10IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer10);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer11 != null && player == ZombieLaboratory.survivorPlayer11) {
                        if (!ZombieLaboratory.survivorPlayer11IsInfected) {
                            ZombieLaboratory.survivorPlayer11IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer11);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer12 != null && player == ZombieLaboratory.survivorPlayer12) {
                        if (!ZombieLaboratory.survivorPlayer12IsInfected) {
                            ZombieLaboratory.survivorPlayer12IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer12);
                        }
                        else {
                            return;
                        }
                    }
                    else if (ZombieLaboratory.survivorPlayer13 != null && player == ZombieLaboratory.survivorPlayer13) {
                        if (!ZombieLaboratory.survivorPlayer13IsInfected) {
                            ZombieLaboratory.survivorPlayer13IsInfected = true;
                            ZombieLaboratory.infectedTeam.Add(ZombieLaboratory.survivorPlayer13);
                        }
                        else {
                            return;
                        }
                    }

                    new CustomMessage(Language.statusZombieLaboratoryTexts[1], 5, -1, 1.3f, 21);
                    ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                }
            }
        }

        public static void zombieLaboratoryTurnZombie(byte playerId) {
            PlayerControl player = Helpers.playerById(playerId);

            // Add zombie role
            if (ZombieLaboratory.zombiePlayer02 == null) {
                ZombieLaboratory.zombiePlayer02 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer03 == null) {
                ZombieLaboratory.zombiePlayer03 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer04 == null) {
                ZombieLaboratory.zombiePlayer04 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer05 == null) {
                ZombieLaboratory.zombiePlayer05 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer06 == null) {
                ZombieLaboratory.zombiePlayer06 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer07 == null) {
                ZombieLaboratory.zombiePlayer07 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer08 == null) {
                ZombieLaboratory.zombiePlayer08 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer09 == null) {
                ZombieLaboratory.zombiePlayer09 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer10 == null) {
                ZombieLaboratory.zombiePlayer10 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer11 == null) {
                ZombieLaboratory.zombiePlayer11 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer12 == null) {
                ZombieLaboratory.zombiePlayer12 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer13 == null) {
                ZombieLaboratory.zombiePlayer13 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }
            else if (ZombieLaboratory.zombiePlayer14 == null) {
                ZombieLaboratory.zombiePlayer14 = player;
                ZombieLaboratory.zombieTeam.Add(player);
            }

            if (!player.Data.IsDead) {
                switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                    case 0:
                        if (activatedSensei) {
                            player.transform.position = new Vector3(-4.85f, 6, player.transform.position.z);
                        }
                        else {
                            player.transform.position = new Vector3(-17.25f, -13.25f, player.transform.position.z);
                        }
                        break;
                    case 1:
                        player.transform.position = new Vector3(18.5f, -1.85f, player.transform.position.z);
                        break;
                    case 2:
                        player.transform.position = new Vector3(17.15f, -17.15f, player.transform.position.z);
                        break;
                    case 3:
                        player.transform.position = new Vector3(17.25f, -13.25f, player.transform.position.z);
                        break;
                    case 4:
                        player.transform.position = new Vector3(32.35f, 7.25f, player.transform.position.z);
                        break;
                    case 5:
                        if (player.transform.position.y > 0) {
                            player.transform.position = new Vector3(1f, 10f, player.transform.position.z);
                        }
                        else {
                            player.transform.position = new Vector3(-4.15f, -33.5f, player.transform.position.z);
                        }
                        break;
                }
            }

            new CustomMessage(Language.statusZombieLaboratoryTexts[2], 5, -1, 1f, 21);
            ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
            HudManager.Instance.StartCoroutine(Effects.Lerp(1, new Action<float>((p) => { // Delayed action
                if (p == 1f) {
                    // Check win condition
                    if (ZombieLaboratory.survivorTeam.Count == 1) {
                        ZombieLaboratory.triggerZombieWin = true;
                    }
                }
            })));
        }

        public static void zombieKills(byte targetId, int whichplayer) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    switch (whichplayer) {
                        case 1:
                            ZombieLaboratory.zombiePlayer01.MurderPlayer(player);
                            break;
                        case 2:
                            ZombieLaboratory.zombiePlayer02.MurderPlayer(player);
                            break;
                        case 3:
                            ZombieLaboratory.zombiePlayer03.MurderPlayer(player);
                            break;
                        case 4:
                            ZombieLaboratory.zombiePlayer04.MurderPlayer(player);
                            break;
                        case 5:
                            ZombieLaboratory.zombiePlayer05.MurderPlayer(player);
                            break;
                        case 6:
                            ZombieLaboratory.zombiePlayer06.MurderPlayer(player);
                            break;
                        case 7:
                            ZombieLaboratory.zombiePlayer07.MurderPlayer(player);
                            break;
                        case 8:
                            ZombieLaboratory.zombiePlayer08.MurderPlayer(player);
                            break;
                        case 9:
                            ZombieLaboratory.zombiePlayer09.MurderPlayer(player);
                            break;
                        case 10:
                            ZombieLaboratory.zombiePlayer10.MurderPlayer(player);
                            break;
                        case 11:
                            ZombieLaboratory.zombiePlayer11.MurderPlayer(player);
                            break;
                        case 12:
                            ZombieLaboratory.zombiePlayer12.MurderPlayer(player);
                            break;
                        case 13:
                            ZombieLaboratory.zombiePlayer13.MurderPlayer(player);
                            break;
                        case 14:
                            ZombieLaboratory.zombiePlayer14.MurderPlayer(player);
                            break;
                        case 15:
                            ZombieLaboratory.survivorPlayer01.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer01CanKill = false;
                            break;
                        case 16:
                            ZombieLaboratory.survivorPlayer02.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer02CanKill = false;
                            break;
                        case 17:
                            ZombieLaboratory.survivorPlayer03.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer03CanKill = false;
                            break;
                        case 18:
                            ZombieLaboratory.survivorPlayer04.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer04CanKill = false;
                            break;
                        case 19:
                            ZombieLaboratory.survivorPlayer05.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer05CanKill = false;
                            break;
                        case 20:
                            ZombieLaboratory.survivorPlayer06.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer06CanKill = false;
                            break;
                        case 21:
                            ZombieLaboratory.survivorPlayer07.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer07CanKill = false;
                            break;
                        case 22:
                            ZombieLaboratory.survivorPlayer08.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer08CanKill = false;
                            break;
                        case 23:
                            ZombieLaboratory.survivorPlayer09.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer09CanKill = false;
                            break;
                        case 24:
                            ZombieLaboratory.survivorPlayer10.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer10CanKill = false;
                            break;
                        case 25:
                            ZombieLaboratory.survivorPlayer11.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer11CanKill = false;
                            break;
                        case 26:
                            ZombieLaboratory.survivorPlayer12.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer12CanKill = false;
                            break;
                        case 27:
                            ZombieLaboratory.survivorPlayer13.MurderPlayer(player);
                            ZombieLaboratory.survivorPlayer13CanKill = false;
                            break;
                    }
                    return;
                }
            }
        }

        public static void zombieAmmoRecover(int whichplayer) {
            switch (whichplayer) {
                case 1:
                    ZombieLaboratory.survivorPlayer01CanKill = true;
                    break;
                case 2:
                    ZombieLaboratory.survivorPlayer02CanKill = true;
                    break;
                case 3:
                    ZombieLaboratory.survivorPlayer03CanKill = true;
                    break;
                case 4:
                    ZombieLaboratory.survivorPlayer04CanKill = true;
                    break;
                case 5:
                    ZombieLaboratory.survivorPlayer05CanKill = true;
                    break;
                case 6:
                    ZombieLaboratory.survivorPlayer06CanKill = true;
                    break;
                case 7:
                    ZombieLaboratory.survivorPlayer07CanKill = true;
                    break;
                case 8:
                    ZombieLaboratory.survivorPlayer08CanKill = true;
                    break;
                case 9:
                    ZombieLaboratory.survivorPlayer09CanKill = true;
                    break;
                case 10:
                    ZombieLaboratory.survivorPlayer10CanKill = true;
                    break;
                case 11:
                    ZombieLaboratory.survivorPlayer11CanKill = true;
                    break;
                case 12:
                    ZombieLaboratory.survivorPlayer12CanKill = true;
                    break;
                case 13:
                    ZombieLaboratory.survivorPlayer13CanKill = true;
                    break;
            }
        }

        public static void zombieNurseHeal(byte targetId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {
                    if (ZombieLaboratory.survivorPlayer01 != null && player == ZombieLaboratory.survivorPlayer01) {
                        ZombieLaboratory.survivorPlayer01IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer01);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer02 != null && player == ZombieLaboratory.survivorPlayer02) {
                        ZombieLaboratory.survivorPlayer02IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer02);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer03 != null && player == ZombieLaboratory.survivorPlayer03) {
                        ZombieLaboratory.survivorPlayer03IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer03);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer04 != null && player == ZombieLaboratory.survivorPlayer04) {
                        ZombieLaboratory.survivorPlayer04IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer04);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer05 != null && player == ZombieLaboratory.survivorPlayer05) {
                        ZombieLaboratory.survivorPlayer05IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer05);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer06 != null && player == ZombieLaboratory.survivorPlayer06) {
                        ZombieLaboratory.survivorPlayer06IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer06);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer07 != null && player == ZombieLaboratory.survivorPlayer07) {
                        ZombieLaboratory.survivorPlayer07IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer07);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer08 != null && player == ZombieLaboratory.survivorPlayer08) {
                        ZombieLaboratory.survivorPlayer08IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer08);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer09 != null && player == ZombieLaboratory.survivorPlayer09) {
                        ZombieLaboratory.survivorPlayer09IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer09);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer10 != null && player == ZombieLaboratory.survivorPlayer10) {
                        ZombieLaboratory.survivorPlayer10IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer10);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer11 != null && player == ZombieLaboratory.survivorPlayer11) {
                        ZombieLaboratory.survivorPlayer11IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer11);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer12 != null && player == ZombieLaboratory.survivorPlayer12) {
                        ZombieLaboratory.survivorPlayer12IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer12);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.survivorPlayer13 != null && player == ZombieLaboratory.survivorPlayer13) {
                        ZombieLaboratory.survivorPlayer13IsInfected = false;
                        ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer13);
                        nurseHealSound(targetId);
                    }
                    else if (ZombieLaboratory.zombiePlayer01 != null && player == ZombieLaboratory.zombiePlayer01) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer01);
                    }
                    else if (ZombieLaboratory.zombiePlayer02 != null && player == ZombieLaboratory.zombiePlayer02) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer02);
                    }
                    else if (ZombieLaboratory.zombiePlayer03 != null && player == ZombieLaboratory.zombiePlayer03) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer03);
                    }
                    else if (ZombieLaboratory.zombiePlayer04 != null && player == ZombieLaboratory.zombiePlayer04) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer04);
                    }
                    else if (ZombieLaboratory.zombiePlayer05 != null && player == ZombieLaboratory.zombiePlayer05) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer05);
                    }
                    else if (ZombieLaboratory.zombiePlayer06 != null && player == ZombieLaboratory.zombiePlayer06) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer06);
                    }
                    else if (ZombieLaboratory.zombiePlayer07 != null && player == ZombieLaboratory.zombiePlayer07) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer07);
                    }
                    else if (ZombieLaboratory.zombiePlayer08 != null && player == ZombieLaboratory.zombiePlayer08) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer08);
                    }
                    else if (ZombieLaboratory.zombiePlayer09 != null && player == ZombieLaboratory.zombiePlayer09) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer09);
                    }
                    else if (ZombieLaboratory.zombiePlayer10 != null && player == ZombieLaboratory.zombiePlayer10) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer10);
                    }
                    else if (ZombieLaboratory.zombiePlayer11 != null && player == ZombieLaboratory.zombiePlayer11) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer11);
                    }
                    else if (ZombieLaboratory.zombiePlayer12 != null && player == ZombieLaboratory.zombiePlayer12) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer12);
                    }
                    else if (ZombieLaboratory.zombiePlayer13 != null && player == ZombieLaboratory.zombiePlayer13) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer13);
                    }
                    else if (ZombieLaboratory.zombiePlayer14 != null && player == ZombieLaboratory.zombiePlayer14) {
                        ZombieLaboratory.nursePlayer.MurderPlayer(ZombieLaboratory.zombiePlayer14);
                    }
                    ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                    ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                }
            }
        }

        public static void nurseHealSound(byte playerId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                SoundManager.Instance.PlaySound(CustomMain.customAssets.spiritualistRevive, false, 100f);
                if (player.PlayerId == playerId && playerId == PlayerControl.LocalPlayer.PlayerId) {
                    HudManager.Instance.FullScreen.enabled = true;
                    HudManager.Instance.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((p) => {
                        var renderer = HudManager.Instance.FullScreen;
                        Color c = new Color(255f / 255f, 197f / 255f, 255f / 255f, 0f);
                        if (p < 0.5) {
                            if (renderer != null)
                                renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(p * 2 * 0.75f));
                        }
                        else {
                            if (renderer != null)
                                renderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                        }
                    })));
                }
            }
        }

        public static void enterLeaveInfirmary(byte survivorId, bool isEntering, byte whichExit) {
            foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                if (player.PlayerId == survivorId) {
                    if (isEntering) {
                        switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                            case 0:
                                if (activatedSensei) {
                                    player.transform.position = new Vector3(-12f, 7.15f, player.transform.position.z);
                                }
                                else {
                                    player.transform.position = new Vector3(-10.2f, 3.6f, player.transform.position.z);
                                }
                                break;
                            case 1:
                                player.transform.position = new Vector3(1.8f, 1.25f, player.transform.position.z);
                                break;
                            case 2:
                                player.transform.position = new Vector3(16.65f, -2.5f, player.transform.position.z);
                                break;
                            case 3:
                                player.transform.position = new Vector3(10.2f, 3.6f, player.transform.position.z);
                                break;
                            case 4:
                                player.transform.position = new Vector3(-18.5f, 2.9f, player.transform.position.z);
                                break;
                            case 5:
                                if (player.transform.position.y > 0) {
                                    player.transform.position = new Vector3(-6f, 31.85f, player.transform.position.z);
                                }
                                else {
                                    player.transform.position = new Vector3(-14.15f, -39.25f, player.transform.position.z);
                                }
                                break;
                        }
                    }
                    else {
                        switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                            case 0:
                                if (activatedSensei) {
                                    switch (whichExit) {
                                        case 1:
                                            player.transform.position = new Vector3(-12f, 5f, player.transform.position.z);
                                            break;
                                        case 2:
                                            player.transform.position = new Vector3(-15.5f, 3f, player.transform.position.z);
                                            break;
                                        case 3:
                                            player.transform.position = new Vector3(-6.75f, 8.25f, player.transform.position.z);
                                            break;
                                    }
                                }
                                else {
                                    switch (whichExit) {
                                        case 1:
                                            player.transform.position = new Vector3(-10.2f, 1.18f, player.transform.position.z);
                                            break;
                                        case 2:
                                            player.transform.position = new Vector3(-17f, -1f, player.transform.position.z);
                                            break;
                                        case 3:
                                            player.transform.position = new Vector3(-3.25f, 5.25f, player.transform.position.z);
                                            break;
                                    }
                                }
                                break;
                            case 1:
                                switch (whichExit) {
                                    case 1:
                                        player.transform.position = new Vector3(1.8f, -1f, player.transform.position.z);
                                        break;
                                    case 2:
                                        player.transform.position = new Vector3(-3.4f, 3.5f, player.transform.position.z);
                                        break;
                                    case 3:
                                        player.transform.position = new Vector3(4.5f, 1.5f, player.transform.position.z);
                                        break;
                                }
                                break;
                            case 2:
                                switch (whichExit) {
                                    case 1:
                                        player.transform.position = new Vector3(16.65f, -5f, player.transform.position.z);
                                        break;
                                    case 2:
                                        player.transform.position = new Vector3(5.5f, -9.5f, player.transform.position.z);
                                        break;
                                    case 3:
                                        player.transform.position = new Vector3(34.75f, -6f, player.transform.position.z);
                                        break;
                                }
                                break;
                            case 3:
                                switch (whichExit) {
                                    case 1:
                                        player.transform.position = new Vector3(10.2f, 1.18f, player.transform.position.z);
                                        break;
                                    case 2:
                                        player.transform.position = new Vector3(17f, -1f, player.transform.position.z);
                                        break;
                                    case 3:
                                        player.transform.position = new Vector3(3.25f, 5.25f, player.transform.position.z);
                                        break;
                                }
                                break;
                            case 4:
                                switch (whichExit) {
                                    case 1:
                                        player.transform.position = new Vector3(-18.5f, 0.75f, player.transform.position.z);
                                        break;
                                    case 2:
                                        player.transform.position = new Vector3(-14.25f, -8f, player.transform.position.z);
                                        break;
                                    case 3:
                                        player.transform.position = new Vector3(-10.75f, 8.5f, player.transform.position.z);
                                        break;
                                }
                                break;
                            case 5:
                                switch (whichExit) {
                                    case 1:
                                        if (player.transform.position.y > 0) {
                                            player.transform.position = new Vector3(-6f, 28.5f, player.transform.position.z);
                                        }
                                        else {
                                            player.transform.position = new Vector3(-11f, -39.5f, player.transform.position.z);
                                        }
                                        break;
                                    case 2:
                                        if (player.transform.position.y > 0) {
                                            player.transform.position = new Vector3(-12.15f, 20.15f, player.transform.position.z);
                                        }
                                        else {
                                            player.transform.position = new Vector3(-13.5f, -34.25f, player.transform.position.z);
                                        }
                                        break;
                                    case 3:
                                        if (player.transform.position.y > 0) {
                                            player.transform.position = new Vector3(0f, 32f, player.transform.position.z);
                                        }
                                        else {
                                            player.transform.position = new Vector3(5.15f, -39.25f, player.transform.position.z);
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
        }

        public static void nurseHasMedKit() {
            ZombieLaboratory.laboratoryNurseMedKit.SetActive(true);
        }

        public static void zombieSurvivorsWin() {
            SoundManager.Instance.PlaySound(CustomMain.customAssets.spiritualistRevive, false, 100f);
            ZombieLaboratory.triggerSurvivorWin = true;
        }

        public static void battleRoyaleKills(byte targetId, byte sourceId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == targetId) {

                    // Hit sound
                    if (player == PlayerControl.LocalPlayer) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.royaleGetHit, false, 100f);
                    }

                    if (BattleRoyale.matchType == 0) {
                        new BattleRoyaleFootprint(player, 0);

                        // Remove 1 life and check remaining lifes
                        if (BattleRoyale.soloPlayer01 != null && player == BattleRoyale.soloPlayer01) {
                            BattleRoyale.soloPlayer01Lifes -= 1;
                            if (BattleRoyale.soloPlayer01Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer02 != null && player == BattleRoyale.soloPlayer02) {
                            BattleRoyale.soloPlayer02Lifes -= 1;
                            if (BattleRoyale.soloPlayer02Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer03 != null && player == BattleRoyale.soloPlayer03) {
                            BattleRoyale.soloPlayer03Lifes -= 1;
                            if (BattleRoyale.soloPlayer03Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer04 != null && player == BattleRoyale.soloPlayer04) {
                            BattleRoyale.soloPlayer04Lifes -= 1;
                            if (BattleRoyale.soloPlayer04Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer05 != null && player == BattleRoyale.soloPlayer05) {
                            BattleRoyale.soloPlayer05Lifes -= 1;
                            if (BattleRoyale.soloPlayer05Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer06 != null && player == BattleRoyale.soloPlayer06) {
                            BattleRoyale.soloPlayer06Lifes -= 1;
                            if (BattleRoyale.soloPlayer06Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer07 != null && player == BattleRoyale.soloPlayer07) {
                            BattleRoyale.soloPlayer07Lifes -= 1;
                            if (BattleRoyale.soloPlayer07Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer08 != null && player == BattleRoyale.soloPlayer08) {
                            BattleRoyale.soloPlayer08Lifes -= 1;
                            if (BattleRoyale.soloPlayer08Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer09 != null && player == BattleRoyale.soloPlayer09) {
                            BattleRoyale.soloPlayer09Lifes -= 1;
                            if (BattleRoyale.soloPlayer09Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer10 != null && player == BattleRoyale.soloPlayer10) {
                            BattleRoyale.soloPlayer10Lifes -= 1;
                            if (BattleRoyale.soloPlayer10Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer11 != null && player == BattleRoyale.soloPlayer11) {
                            BattleRoyale.soloPlayer11Lifes -= 1;
                            if (BattleRoyale.soloPlayer11Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer12 != null && player == BattleRoyale.soloPlayer12) {
                            BattleRoyale.soloPlayer12Lifes -= 1;
                            if (BattleRoyale.soloPlayer12Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer13 != null && player == BattleRoyale.soloPlayer13) {
                            BattleRoyale.soloPlayer13Lifes -= 1;
                            if (BattleRoyale.soloPlayer13Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer14 != null && player == BattleRoyale.soloPlayer14) {
                            BattleRoyale.soloPlayer14Lifes -= 1;
                            if (BattleRoyale.soloPlayer14Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                        else if (BattleRoyale.soloPlayer15 != null && player == BattleRoyale.soloPlayer15) {
                            BattleRoyale.soloPlayer15Lifes -= 1;
                            if (BattleRoyale.soloPlayer15Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                battleRoyaleCheckWin(0);
                            }
                        }
                    }
                    else {
                        // Remove 1 life and check remaining lifes
                        if (BattleRoyale.limePlayer01 != null && player == BattleRoyale.limePlayer01) {
                            BattleRoyale.limePlayer01Lifes -= 1;
                            new BattleRoyaleFootprint(player, 1);
                            if (BattleRoyale.limePlayer01Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(1);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(2, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.limePlayer02 != null && player == BattleRoyale.limePlayer02) {
                            BattleRoyale.limePlayer02Lifes -= 1;
                            new BattleRoyaleFootprint(player, 1);
                            if (BattleRoyale.limePlayer02Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(1);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(2, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.limePlayer03 != null && player == BattleRoyale.limePlayer03) {
                            BattleRoyale.limePlayer03Lifes -= 1;
                            new BattleRoyaleFootprint(player, 1);
                            if (BattleRoyale.limePlayer03Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(1);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(2, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.limePlayer04 != null && player == BattleRoyale.limePlayer04) {
                            BattleRoyale.limePlayer04Lifes -= 1;
                            new BattleRoyaleFootprint(player, 1);
                            if (BattleRoyale.limePlayer04Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(1);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(2, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.limePlayer05 != null && player == BattleRoyale.limePlayer05) {
                            BattleRoyale.limePlayer05Lifes -= 1;
                            new BattleRoyaleFootprint(player, 1);
                            if (BattleRoyale.limePlayer05Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(1);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(2, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.limePlayer06 != null && player == BattleRoyale.limePlayer06) {
                            BattleRoyale.limePlayer06Lifes -= 1;
                            new BattleRoyaleFootprint(player, 1);
                            if (BattleRoyale.limePlayer06Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(1);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(2, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.limePlayer07 != null && player == BattleRoyale.limePlayer07) {
                            BattleRoyale.limePlayer07Lifes -= 1;
                            new BattleRoyaleFootprint(player, 1);
                            if (BattleRoyale.limePlayer07Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(1);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(2, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.pinkPlayer01 != null && player == BattleRoyale.pinkPlayer01) {
                            BattleRoyale.pinkPlayer01Lifes -= 1;
                            new BattleRoyaleFootprint(player, 2);
                            if (BattleRoyale.pinkPlayer01Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(2);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(1, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.pinkPlayer02 != null && player == BattleRoyale.pinkPlayer02) {
                            BattleRoyale.pinkPlayer02Lifes -= 1;
                            new BattleRoyaleFootprint(player, 2);
                            if (BattleRoyale.pinkPlayer02Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(2);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(1, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.pinkPlayer03 != null && player == BattleRoyale.pinkPlayer03) {
                            BattleRoyale.pinkPlayer03Lifes -= 1;
                            new BattleRoyaleFootprint(player, 2);
                            if (BattleRoyale.pinkPlayer03Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(2);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(1, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.pinkPlayer04 != null && player == BattleRoyale.pinkPlayer04) {
                            BattleRoyale.pinkPlayer04Lifes -= 1;
                            new BattleRoyaleFootprint(player, 2);
                            if (BattleRoyale.pinkPlayer04Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(2);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(1, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.pinkPlayer05 != null && player == BattleRoyale.pinkPlayer05) {
                            BattleRoyale.pinkPlayer05Lifes -= 1;
                            new BattleRoyaleFootprint(player, 2);
                            if (BattleRoyale.pinkPlayer05Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(2);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(1, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.pinkPlayer06 != null && player == BattleRoyale.pinkPlayer06) {
                            BattleRoyale.pinkPlayer06Lifes -= 1;
                            new BattleRoyaleFootprint(player, 2);
                            if (BattleRoyale.pinkPlayer06Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(2);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(1, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.pinkPlayer07 != null && player == BattleRoyale.pinkPlayer07) {
                            BattleRoyale.pinkPlayer07Lifes -= 1;
                            new BattleRoyaleFootprint(player, 2);
                            if (BattleRoyale.pinkPlayer07Lifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(2);
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                                            battleRoyaleScoreCheck(3, 1);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(1, 1);
                                        }
                                        break;
                                }
                            }
                        }
                        else if (BattleRoyale.serialKiller != null && player == BattleRoyale.serialKiller) {
                            BattleRoyale.serialKillerLifes -= 1;
                            new BattleRoyaleFootprint(player, 3);
                            if (BattleRoyale.serialKillerLifes <= 0) {
                                uncheckedMurderPlayer(sourceId, targetId, 0);
                                switch (BattleRoyale.matchType) {
                                    case 1:
                                        battleRoyaleCheckWin(3);
                                        break;
                                    case 2:
                                        if (BattleRoyale.limePlayer01 != null && sourceId == BattleRoyale.limePlayer01.PlayerId || BattleRoyale.limePlayer02 != null && sourceId == BattleRoyale.limePlayer02.PlayerId || BattleRoyale.limePlayer03 != null && sourceId == BattleRoyale.limePlayer03.PlayerId || BattleRoyale.limePlayer04 != null && sourceId == BattleRoyale.limePlayer04.PlayerId || BattleRoyale.limePlayer05 != null && sourceId == BattleRoyale.limePlayer05.PlayerId || BattleRoyale.limePlayer06 != null && sourceId == BattleRoyale.limePlayer06.PlayerId || BattleRoyale.limePlayer07 != null && sourceId == BattleRoyale.limePlayer07.PlayerId) {
                                            battleRoyaleScoreCheck(1, 3);
                                        }
                                        else {
                                            battleRoyaleScoreCheck(2, 3);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void battleRoyaleCheckWin(int whichTeamCheck) {

            SoundManager.Instance.PlaySound(CustomMain.customAssets.yinyangerYinyangColisionClip, false, 100f);

            if (BattleRoyale.matchType == 0) {
                int soloPlayersAlives = 0;

                new CustomMessage(Language.statusBattleRoyaleTexts[0], 5, -1, 1.6f, 26);

                foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {

                    if (!soloPlayer.Data.IsDead) {
                        soloPlayersAlives += 1;
                    }

                }

                BattleRoyale.battleRoyalepointCounter = Language.introTexts[11] + "<color=#009F57FF>" + soloPlayersAlives + "</color>";

                if (soloPlayersAlives <= 1) {
                    BattleRoyale.triggerSoloWin = true;
                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleSoloWin, false);
                }
            }
            else {

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

                if (whichTeamCheck == 1) {
                    new CustomMessage(Language.statusBattleRoyaleTexts[1], 5, -1, 1.6f, 26);
                }
                else if (whichTeamCheck == 2) {
                    new CustomMessage(Language.statusBattleRoyaleTexts[2], 5, -1, 1.3f, 26);
                }
                else if (whichTeamCheck == 3) {
                    new CustomMessage(Language.statusBattleRoyaleTexts[3], 5, -1, 1f, 26);
                    if (limePlayersAlive <= 0) {
                        BattleRoyale.triggerPinkTeamWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
                    }
                    else if (pinkPlayersAlive <= 0) {
                        BattleRoyale.triggerLimeTeamWin = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
                    }
                }

                if (BattleRoyale.serialKiller != null) {

                    int serialKillerAlive = 0;

                    foreach (PlayerControl serialKiller in BattleRoyale.serialKillerTeam) {

                        if (!serialKiller.Data.IsDead) {
                            serialKillerAlive += 1;
                        }

                    }
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
                else {
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
        }

        public static void battleRoyaleScoreCheck(int whichTeamCheck, int multiplier) {
            switch (whichTeamCheck) {
                case 1:
                    BattleRoyale.limePoints += 10 * multiplier;
                    new CustomMessage(Language.statusBattleRoyaleTexts[4], 5, -1, 1.6f, 26);
                    break;
                case 2:
                    BattleRoyale.pinkPoints += 10 * multiplier;
                    new CustomMessage(Language.statusBattleRoyaleTexts[5], 5, -1, 1.3f, 26);
                    break;
                case 3:
                    BattleRoyale.serialKillerPoints += 10 * multiplier;
                    new CustomMessage(Language.statusBattleRoyaleTexts[6], 5, -1, 1f, 26);
                    break;
            }
            
            if (BattleRoyale.serialKiller != null) {
                BattleRoyale.battleRoyalepointCounter = Language.introTexts[15] + BattleRoyale.requiredScore + " | <color=#39FF14FF>" + Language.introTexts[12] + BattleRoyale.limePoints + " </color> | " + "<color=#F2BEFFFF>" + Language.introTexts[13] + BattleRoyale.pinkPoints + "</color> | " + "<color=#808080FF>" + Language.introTexts[16] + BattleRoyale.serialKillerPoints + " </color>";
            }
            else {
                BattleRoyale.battleRoyalepointCounter = Language.introTexts[15] + BattleRoyale.requiredScore + " | <color=#39FF14FF>" + Language.introTexts[12] + BattleRoyale.limePoints + " </color> | " + "<color=#F2BEFFFF>" + Language.introTexts[13] + BattleRoyale.pinkPoints + "</color>";
            }

            if (BattleRoyale.limePoints >= BattleRoyale.requiredScore) {
                BattleRoyale.triggerLimeTeamWin = true;
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleLimeTeamWin, false);
            }
            else if (BattleRoyale.pinkPoints >= BattleRoyale.requiredScore) {
                BattleRoyale.triggerPinkTeamWin = true;
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyalePinkTeamWin, false);
            }
            else if (BattleRoyale.serialKillerPoints >= BattleRoyale.requiredScore) {
                BattleRoyale.triggerSerialKillerWin = true;
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.BattleRoyaleSerialKillerWin, false);
            }
        }
    }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader) {
            byte packetId = callId;
            switch (packetId) {

                // Main Controls

                case (byte)CustomRPC.ResetVaribles:
                    RPCProcedure.resetVariables();
                    break;
                case (byte)CustomRPC.ShareOptions:
                    RPCProcedure.ShareOptions((int)reader.ReadPackedUInt32(), reader);
                    break;
                case (byte)CustomRPC.ForceEnd:
                    RPCProcedure.forceEnd();
                    break;
                case (byte)CustomRPC.SetRole:
                    byte roleId = reader.ReadByte();
                    byte playerId = reader.ReadByte();
                    RPCProcedure.setRole(roleId, playerId);
                    break;
                case (byte)CustomRPC.SetModifier:
                    byte modifierId = reader.ReadByte();
                    byte pId = reader.ReadByte();
                    byte flag = reader.ReadByte();
                    RPCProcedure.setModifier(modifierId, pId, flag);
                    break;
                case (byte)CustomRPC.UseUncheckedVent:
                    int ventId = reader.ReadPackedInt32();
                    byte ventingPlayer = reader.ReadByte();
                    byte isEnter = reader.ReadByte();
                    RPCProcedure.useUncheckedVent(ventId, ventingPlayer, isEnter);
                    break;
                case (byte)CustomRPC.UncheckedMurderPlayer:
                    byte source = reader.ReadByte();
                    byte target = reader.ReadByte();
                    byte showAnimation = reader.ReadByte();
                    RPCProcedure.uncheckedMurderPlayer(source, target, showAnimation);
                    break;
                case (byte)CustomRPC.UncheckedCmdReportDeadBody:
                    byte reportSource = reader.ReadByte();
                    byte reportTarget = reader.ReadByte();
                    RPCProcedure.uncheckedCmdReportDeadBody(reportSource, reportTarget);
                    break;
                case (byte)CustomRPC.UncheckedExilePlayer:
                    byte exileTarget = reader.ReadByte();
                    RPCProcedure.uncheckedExilePlayer(exileTarget);
                    break;
                case (byte)CustomRPC.RandomizeCustomSkeldOnHS:
                    int theNumber = reader.ReadPackedInt32();
                    RPCProcedure.randomizeCustomSkeldOnHS(theNumber);
                    break;

                // Role impostor functionality

                case (byte)CustomRPC.MimicTransform:
                    RPCProcedure.mimicTransform(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PainterPaint:
                    int colorId = reader.ReadPackedInt32();
                    RPCProcedure.painterPaint(colorId);
                    break;
                case (byte)CustomRPC.DemonSetBitten:
                    byte bittenId = reader.ReadByte();
                    byte reset = reader.ReadByte();
                    RPCProcedure.demonSetBitten(bittenId, reset);
                    break;
                case (byte)CustomRPC.PlaceNun:
                    RPCProcedure.placeNun(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.RemoveBody:
                    RPCProcedure.removeBody(reader.ReadByte());
                    break;
                case (byte)CustomRPC.DragPlaceBody:
                    RPCProcedure.dragPlaceBody(reader.ReadByte(), reader.ReadByte());
                    break;
                case (byte)CustomRPC.PlaceHat:
                    RPCProcedure.placeHat(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.LightsOut:
                    RPCProcedure.lightsOut();
                    break;
                case (byte)CustomRPC.ManipulatorKill:
                    RPCProcedure.manipulatorKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PlaceBomb:
                    RPCProcedure.placeBomb(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.FixBomb:
                    RPCProcedure.fixBomb();
                    break;
                case (byte)CustomRPC.BombermanWin:
                    RPCProcedure.bombermanWin();
                    break;
                case (byte)CustomRPC.ChameleonInvisible:
                    RPCProcedure.chameleonInvisible();
                    break;
                case (byte)CustomRPC.GamblerShoot:
                    RPCProcedure.gamblerShoot(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SetSpelledPlayer:
                    RPCProcedure.setSpelledPlayer(reader.ReadByte());
                    break;
                case (byte)CustomRPC.MedusaPetrify:
                    RPCProcedure.medusaPetrify(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PlaceSpiralTrap:
                    RPCProcedure.placeSpiralTrap(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.ActivateSpiralTrap:
                    RPCProcedure.activateSpiralTrap(reader.ReadByte());
                    break;
                case (byte)CustomRPC.ShowArcherNotification:
                    RPCProcedure.showArcherNotification(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PlumberMakeVent:
                    var pos = reader.ReadBytesAndSize();
                    RPCProcedure.plumberMakeVent(pos);
                    break;
                case (byte)CustomRPC.SilencePlayer:
                    RPCProcedure.silencePlayer(reader.ReadByte());
                    break;
                case (byte)CustomRPC.ResetSilenced:
                    RPCProcedure.resetSilenced();
                    break;
                    
                // Role rebeldes functionality

                case (byte)CustomRPC.RenegadeRecruitMinion:
                    RPCProcedure.renegadeRecruitMinion(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SetRandomTarget:
                    byte bountyId = reader.ReadByte();
                    byte playerInt = reader.ReadByte();
                    RPCProcedure.setRandomTarget(bountyId, playerInt);
                    break;
                case (byte)CustomRPC.BountyHunterKill:
                    RPCProcedure.bountyHunterKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PlaceMine:
                    RPCProcedure.placeMine(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.PlaceTrap:
                    RPCProcedure.placeTrap(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.MineKill:
                    RPCProcedure.mineKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.ActivateTrap:
                    RPCProcedure.activateTrap(reader.ReadByte());
                    break;
                case (byte)CustomRPC.YinyangerSetYinyang:
                    byte yinedId = reader.ReadByte();
                    byte yinflag = reader.ReadByte();
                    RPCProcedure.yinyangerSetYinyang(yinedId, yinflag);
                    break;
                case (byte)CustomRPC.ChallengerPerformDuel:
                    RPCProcedure.challengerPerformDuel();
                    break;
                case (byte)CustomRPC.ChallengerSetRival:
                    byte rivalId = reader.ReadByte();
                    byte rivalflag = reader.ReadByte(); 
                    RPCProcedure.challengerSetRival(rivalId, rivalflag);
                    break;
                case (byte)CustomRPC.ChallengerSelectAttack:
                    byte challengerAttack = reader.ReadByte();
                    RPCProcedure.challengerSelectAttack(challengerAttack);
                    break;
                case (byte)CustomRPC.NinjaKill:
                    RPCProcedure.ninjaKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.BerserkerKill:
                    RPCProcedure.berserkerKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.YandereKill:
                    RPCProcedure.yandereKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.StrandedFindBoxes:
                    byte strandedBox = reader.ReadByte();
                    RPCProcedure.strandedFindBoxes(strandedBox);
                    break;
                case (byte)CustomRPC.StrandedKill:
                    RPCProcedure.strandedKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.StrandedInvisible:
                    RPCProcedure.strandedInvisible();
                    break;
                case (byte)CustomRPC.MonjaTakeItem:
                    byte monjaItemId = reader.ReadByte();
                    RPCProcedure.monjaTakeItem(monjaItemId);
                    break;
                case (byte)CustomRPC.MonjaDeliverItem:
                    byte monjaDeliveredItemId = reader.ReadByte();
                    RPCProcedure.monjaDeliverItem(monjaDeliveredItemId);
                    break;
                case (byte)CustomRPC.MonjaRevertItemPosition:
                    byte monjaRevertedItemId = reader.ReadByte();
                    RPCProcedure.monjaRevertItemPosition(monjaRevertedItemId);
                    break;
                case (byte)CustomRPC.MonjaAwakened:
                    RPCProcedure.monjaAwakened();
                    break;
                case (byte)CustomRPC.MonjaKill:
                    RPCProcedure.monjaKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.MonjaReset:
                    RPCProcedure.monjaReset();
                    break;

                // Role neutrals functionality

                case (byte)CustomRPC.RoleThiefSteal:
                    RPCProcedure.roleThiefSteal(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PyromaniacWin:
                    RPCProcedure.pyromaniacWin();
                    break;
                case (byte)CustomRPC.PlaceTreasure:
                    RPCProcedure.placeTreasure();
                    break;
                case (byte)CustomRPC.CollectedTreasure:
                    RPCProcedure.collectedTreasure();
                    break;
                case (byte)CustomRPC.DevourBody:
                    RPCProcedure.devourBody(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PoisonerWin:
                    RPCProcedure.poisonerWin();
                    break;
                case (byte)CustomRPC.PuppeteerWin:
                    RPCProcedure.puppeteerWin();
                    break;
                case (byte)CustomRPC.PuppeteerTransform:
                    RPCProcedure.puppeteerTransform(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PuppeteerResetTransform:
                    RPCProcedure.puppeteerResetTransform();
                    break;
                case (byte)CustomRPC.ExilerTriggerWin:
                    RPCProcedure.exilerWin();
                    break;
                case (byte)CustomRPC.AmnesiacReportAndTakeRole:
                    RPCProcedure.amnesiacReportAndTakeRole(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SeekerSetMinigamePlayers:
                    byte minigamePlayerId = reader.ReadByte();
                    RPCProcedure.seekerSetMinigamePlayers(minigamePlayerId);
                    break;
                case (byte)CustomRPC.SeekerResetMinigamePlayers:
                    byte minigameFlag = reader.ReadByte();
                    RPCProcedure.seekerResetMinigamePlayers(minigameFlag);
                    break;
                case (byte)CustomRPC.SeekerPerformMinigame:
                    RPCProcedure.seekerPerformMinigame();
                    break;
                case (byte)CustomRPC.SeekerSelectAttack:
                    byte seekerAttack = reader.ReadByte();
                    RPCProcedure.seekerSelectAttack(seekerAttack);
                    break;

                // Role crewmates functionality

                case (byte)CustomRPC.CaptainSpecialVote:
                    byte id = reader.ReadByte();
                    byte targetId = reader.ReadByte();
                    RPCProcedure.captainSpecialVote(id, targetId);
                    break;
                case (byte)CustomRPC.CaptainAutoCastSpecialVote:
                    RPCProcedure.captainAutoCastSpecialVote();
                    break;
                case (byte)CustomRPC.MechanicFixLights:
                    RPCProcedure.mechanicFixLights();
                    break;
                case (byte)CustomRPC.MechanicUsedRepair:
                    RPCProcedure.mechanicUsedRepair();
                    break;
                case (byte)CustomRPC.SheriffKill:
                    RPCProcedure.sheriffKill(reader.ReadByte());
                    break;
                case (byte)CustomRPC.TimeTravelerShield:
                    RPCProcedure.timeTravelerShield();
                    break;
                case (byte)CustomRPC.TimeTravelerRewindTime:
                    RPCProcedure.timeTravelerRewindTime();
                    break;
                case (byte)CustomRPC.TimeTravelerRevive:
                    RPCProcedure.timeTravelerRevive(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SquireSetShielded:
                    RPCProcedure.squireSetShielded(reader.ReadByte());
                    break;
                case (byte)CustomRPC.ShieldedMurderAttempt:
                    RPCProcedure.shieldedMurderAttempt();
                    break;
                case (byte)CustomRPC.CheaterCheat:
                    byte playerId1 = reader.ReadByte();
                    byte playerId2 = reader.ReadByte();
                    RPCProcedure.cheaterCheat(playerId1, playerId2);
                    break;
                case (byte)CustomRPC.FortuneTellerReveal:
                    byte targetFortuneId = reader.ReadByte();
                    RPCProcedure.fortuneTellerReveal(targetFortuneId);
                    break;
                case (byte)CustomRPC.HackerAbilityUses:
                    byte adminOrVitals = reader.ReadByte();
                    RPCProcedure.hackerAbilityUses(adminOrVitals);
                    break;
                case (byte)CustomRPC.SleuthUsedLocate:
                    RPCProcedure.sleuthUsedLocate(reader.ReadByte());
                    break;
                case (byte)CustomRPC.FinkHawkEye:
                    RPCProcedure.finkHawkEye();
                    break;
                case (byte)CustomRPC.SealVent:
                    RPCProcedure.sealVent(reader.ReadPackedInt32());
                    break;
                case (byte)CustomRPC.SpiritualistRevive:
                    RPCProcedure.spiritualistRevive(reader.ReadByte(), reader.ReadByte());
                    break;
                case (byte)CustomRPC.SendSpiritualistIsReviving:
                    RPCProcedure.sendSpiritualistIsReviving();
                    break;
                case (byte)CustomRPC.MurderSpiritualistIfReportWhileReviving:
                    RPCProcedure.murderSpiritualistIfReportWhileReviving();
                    break;
                case (byte)CustomRPC.ResetSpiritualistReviveValues:
                    RPCProcedure.resetSpiritualistReviveValues();
                    break;
                case (byte)CustomRPC.CowardUsedCall:
                    RPCProcedure.cowardUsedCall();
                    break;
                case (byte)CustomRPC.PlaceCamera:
                    RPCProcedure.placeCamera(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.VigilantAbilityUses:
                    byte vigilantCharges = reader.ReadByte();
                    RPCProcedure.vigilantAbilityUses(vigilantCharges);
                    break;
                case (byte)CustomRPC.PerformerIsReported:
                    byte check = reader.ReadByte();
                    RPCProcedure.performerIsReported(check);
                    break;
                case (byte)CustomRPC.HunterUsedHunted:
                    RPCProcedure.hunterUsedHunted(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SetJinxed:
                    var pid = reader.ReadByte();
                    var jinxedValue = reader.ReadByte();
                    RPCProcedure.setJinxed(pid, jinxedValue);
                    break;
                case (byte)CustomRPC.BatFrequency:
                    RPCProcedure.batFrequency();
                    break;
                case (byte)CustomRPC.PlaceEngineerTrap:
                    RPCProcedure.placeEngineerTrap(reader.ReadByte(), reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.ActivateEngineerTrap:
                    RPCProcedure.activateEngineerTrap(reader.ReadByte(), reader.ReadByte());
                    break;
                case (byte)CustomRPC.TaskMasterSetExtraTasks:
                    playerId = reader.ReadByte();
                    byte oldTaskMasterPlayerId = reader.ReadByte();
                    byte[] taskTypeIds = reader.BytesRemaining > 0 ? reader.ReadBytes(reader.BytesRemaining) : null;
                    RPCProcedure.taskMasterSetExTasks(playerId, oldTaskMasterPlayerId, taskTypeIds);
                    break;
                case (byte)CustomRPC.TaskMasterTriggerCrewWin:
                    RPCProcedure.taskMasterTriggerCrewWin();
                    break;
                case (byte)CustomRPC.TaskMasterActivateSpeed:
                    RPCProcedure.taskMasterActivateSpeed();
                    break;
                case (byte)CustomRPC.JailerSetJailed:
                    RPCProcedure.jailedSetJailed(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PrisonPlayer:
                    byte prisonerId = reader.ReadByte();
                    RPCProcedure.prisonPlayer(prisonerId);
                    break;

                // Other funtionality

                case (byte)CustomRPC.ChangeMusic:
                    byte whichmusic = reader.ReadByte();
                    RPCProcedure.changeMusic(whichmusic);
                    break;
                case (byte)CustomRPC.WhoWasI:
                    byte playerWhoWasID = reader.ReadByte();
                    string roleName = reader.ReadString();
                    RPCProcedure.whoWasI(playerWhoWasID, roleName);
                    break;

                // Capture the flag funtionality

                case (byte)CustomRPC.CapturetheFlagKills:
                    byte killId = reader.ReadByte();
                    byte whichplayer = reader.ReadByte();
                    RPCProcedure.capturetheFlagKills(killId, whichplayer);
                    break;
                case (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag:
                    byte bluePlayerWhoHasRedFlag = reader.ReadByte();
                    byte redorblue = reader.ReadByte();
                    RPCProcedure.captureTheFlagWhoTookTheFlag(bluePlayerWhoHasRedFlag, redorblue);
                    break;
                case (byte)CustomRPC.CaptureTheFlagWhichTeamScored:
                    byte whichteam = reader.ReadByte();
                    RPCProcedure.captureTheFlagWhichTeamScored(whichteam);
                    break;

                // Police and Thief funtionality
                case (byte)CustomRPC.PoliceandThiefKills:
                    byte policeandthiefId = reader.ReadByte();
                    byte whichpoliceorthief = reader.ReadByte();
                    RPCProcedure.policeandThiefKills(policeandthiefId, whichpoliceorthief);
                    break;
                case (byte)CustomRPC.PoliceandThiefJail:
                    byte thiefId = reader.ReadByte();
                    RPCProcedure.policeandThiefJail(thiefId);
                    break;
                case (byte)CustomRPC.PoliceandThiefFreeThief:
                    RPCProcedure.policeandThiefFreeThief();
                    break;
                case (byte)CustomRPC.PoliceandThiefTakeJewel:
                    byte thiefwhotookjewel = reader.ReadByte();
                    byte jewelTakeId = reader.ReadByte();
                    RPCProcedure.policeandThiefTakeJewel(thiefwhotookjewel, jewelTakeId);
                    break;
                case (byte)CustomRPC.PoliceandThiefDeliverJewel:
                    byte thiefwhodeliverjewel = reader.ReadByte();
                    byte jewelDeliverId = reader.ReadByte();
                    RPCProcedure.policeandThiefDeliverJewel(thiefwhodeliverjewel, jewelDeliverId);
                    break;
                case (byte)CustomRPC.PoliceandThiefRevertedJewelPosition:
                    byte thiefWhoLostJewel = reader.ReadByte();
                    byte jewelRevertedId = reader.ReadByte();
                    RPCProcedure.policeandThiefRevertedJewelPosition(thiefWhoLostJewel, jewelRevertedId);
                    break;
                case (byte)CustomRPC.PoliceandThiefsTased:
                    RPCProcedure.policeandThiefsTased(reader.ReadByte());
                    break;

                // King of the hill funtionality
                case (byte)CustomRPC.KingoftheHillKills:
                    byte killerId = reader.ReadByte();
                    byte whichteamplayer = reader.ReadByte();
                    RPCProcedure.kingOfTheHillKills(killerId, whichteamplayer);
                    break;
                case (byte)CustomRPC.KingoftheHillCapture:
                    byte capturedId = reader.ReadByte();
                    byte whichKingCaptured = reader.ReadByte();
                    RPCProcedure.kingoftheHillCapture(capturedId, whichKingCaptured);
                    break;

                // Hot Potato
                case (byte)CustomRPC.HotPotatoTransfer:
                    RPCProcedure.hotPotatoTransfer(reader.ReadByte());
                    break;

                // ZombieLaboratory
                case (byte)CustomRPC.ZombieInfect:
                    RPCProcedure.zombieInfect(reader.ReadByte());
                    break;
                case (byte)CustomRPC.ZombieKills:
                    byte zombiekillId = reader.ReadByte();
                    byte whichkill = reader.ReadByte();
                    RPCProcedure.zombieKills(zombiekillId, whichkill);
                    break;
                case (byte)CustomRPC.ZombieAmmoRecover:
                    byte whichAmmo = reader.ReadByte();
                    RPCProcedure.zombieAmmoRecover(whichAmmo);
                    break;
                case (byte)CustomRPC.ZombieNurseHeal:
                    byte whichHeal = reader.ReadByte();
                    RPCProcedure.zombieNurseHeal(whichHeal);
                    break;
                case (byte)CustomRPC.ZombieSurvivorsWin:
                    RPCProcedure.zombieSurvivorsWin();
                    break;
                case (byte)CustomRPC.ZombieTakeKeyItem:
                    byte survivorWhoTookKeyItem = reader.ReadByte();
                    byte keyId = reader.ReadByte();
                    RPCProcedure.zombieTakeKeyItem(survivorWhoTookKeyItem, keyId);
                    break;
                case (byte)CustomRPC.ZombieDeliverKeyItem:
                    byte survivorWhoDeliverKey = reader.ReadByte();
                    byte keyDeliveredId = reader.ReadByte();
                    RPCProcedure.zombieDeliverKeyItem(survivorWhoDeliverKey, keyDeliveredId);
                    break;
                case (byte)CustomRPC.EnterLeaveInfirmary:
                    byte survivorWhoEntered = reader.ReadByte();
                    bool enterOrLeave = reader.ReadBoolean();
                    byte whichExit = reader.ReadByte();
                    RPCProcedure.enterLeaveInfirmary(survivorWhoEntered, enterOrLeave, whichExit);
                    break;
                case (byte)CustomRPC.NurseHasMedKit:
                    RPCProcedure.nurseHasMedKit();
                    break;
                case (byte)CustomRPC.ZombieLaboratoryTurnZombie:
                    byte survivorWhoTurned = reader.ReadByte();
                    RPCProcedure.zombieLaboratoryTurnZombie(survivorWhoTurned);
                    break;

                // Battle Royale
                case (byte)CustomRPC.BattleRoyaleKills:
                    byte battlekillId = reader.ReadByte();
                    byte battlewhichplayer = reader.ReadByte();
                    RPCProcedure.battleRoyaleKills(battlekillId, battlewhichplayer);
                    break;
            }
        }
    }
}
