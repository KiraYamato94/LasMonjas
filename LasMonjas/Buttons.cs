using HarmonyLib;
using Hazel;
using System;
using UnityEngine;
using static LasMonjas.LasMonjas;
using LasMonjas.Objects;
using System.Linq;
using LasMonjas.Core;

namespace LasMonjas
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    static class HudManagerStartPatch
    {
        // Impostor buttons
        private static CustomButton mimicTransformButton;
        private static CustomButton painterPaintButton;
        private static CustomButton demonKillButton;
        private static CustomButton nunButton;
        public static CustomButton janitorCleanButton;
        public static CustomButton janitorDragBodyButton;
        private static CustomButton placeHatButton;
        private static CustomButton ilusionistLightsOutButton;
        public static CustomButton manipulatorManipulateButton;
        public static CustomButton bombermanBombButton;
        public static CustomButton chameleonInvisibleButton;
        public static CustomButton sorcererSpellButton;

        // Rebels buttons
        private static CustomButton renegadeKillButton;
        private static CustomButton renegadeMinionButton;
        private static CustomButton minionKillButton;
        private static CustomButton bountyHunterKillButton;
        private static CustomButton bountyHunterSetKillButton;
        private static CustomButton trapperMineButton;
        private static CustomButton trapperTrapButton;
        private static CustomButton trapperKillButton;
        private static CustomButton yinyangerYinButton;
        private static CustomButton yinyangerYangButton;
        private static CustomButton yinyangerKillButton;
        private static CustomButton challengerChallengeButton;
        private static CustomButton challengerKillButton;
        public static CustomButton challengerRockButton;
        public static CustomButton challengerPaperButton;
        public static CustomButton challengerScissorsButton;
        public static CustomButton rivalplayerRockButton;
        public static CustomButton rivalplayerPaperButton;
        public static CustomButton rivalplayerScissorsButton;

        // Neutral buttons
        private static CustomButton roleThiefStealButton;
        public static CustomButton pyromaniacButton;
        private static CustomButton treasureHunterButton;
        private static CustomButton devourerButton;

        // Crewmate buttons
        public static CustomButton captainCallButton;
        private static CustomButton mechanicRepairButton;
        private static CustomButton sheriffKillButton;
        private static CustomButton detectiveButton;
        private static CustomButton forensicButton;
        private static CustomButton timeTravelerShieldButton;
        private static CustomButton timeTravelerRewindTimeButton;
        private static CustomButton squireShieldButton;
        private static CustomButton fortuneTellerRevealButton;
        private static CustomButton hackerButton;
        private static CustomButton hackerVitalsButton;
        private static CustomButton hackerAdminTableButton;
        private static CustomButton sleuthLocatePlayerButton;
        private static CustomButton sleuthLocateCorpsesButton;
        private static CustomButton finkButton;
        public static CustomButton welderSealButton;
        public static CustomButton spiritualistReviveButton;
        public static CustomButton cowardCallButton;
        public static CustomButton vigilantButton;
        public static CustomButton vigilantCamButton;
        private static CustomButton medusaPetrifyButton;
        private static CustomButton hunterButton;
        public static CustomButton jinxButton;


        // Capture the flag buttons
        private static CustomButton redplayer01KillButton;
        private static CustomButton redplayer01TakeFlagButton;
        private static CustomButton redplayer02KillButton;
        private static CustomButton redplayer02TakeFlagButton;
        private static CustomButton redplayer03KillButton;
        private static CustomButton redplayer03TakeFlagButton;
        private static CustomButton redplayer04KillButton;
        private static CustomButton redplayer04TakeFlagButton;
        private static CustomButton redplayer05KillButton;
        private static CustomButton redplayer05TakeFlagButton;
        private static CustomButton redplayer06KillButton;
        private static CustomButton redplayer06TakeFlagButton;
        private static CustomButton redplayer07KillButton;
        private static CustomButton redplayer07TakeFlagButton;
        private static CustomButton blueplayer01KillButton;
        private static CustomButton blueplayer01TakeFlagButton;
        private static CustomButton blueplayer02KillButton;
        private static CustomButton blueplayer02TakeFlagButton;
        private static CustomButton blueplayer03KillButton;
        private static CustomButton blueplayer03TakeFlagButton;
        private static CustomButton blueplayer04KillButton;
        private static CustomButton blueplayer04TakeFlagButton;
        private static CustomButton blueplayer05KillButton;
        private static CustomButton blueplayer05TakeFlagButton;
        private static CustomButton blueplayer06KillButton;
        private static CustomButton blueplayer06TakeFlagButton;
        private static CustomButton blueplayer07KillButton;
        private static CustomButton blueplayer07TakeFlagButton;
        private static CustomButton stealerPlayerKillButton;

        // Police and Thief
        private static CustomButton policeplayer01JailButton;
        private static CustomButton policeplayer01KillButton;
        private static CustomButton policeplayer01LightButton;
        private static CustomButton policeplayer02JailButton;
        private static CustomButton policeplayer02KillButton;
        private static CustomButton policeplayer02LightButton;
        private static CustomButton policeplayer03JailButton;
        private static CustomButton policeplayer03KillButton;
        private static CustomButton policeplayer03LightButton;
        private static CustomButton policeplayer04JailButton;
        private static CustomButton policeplayer04KillButton;
        private static CustomButton policeplayer04LightButton;
        private static CustomButton policeplayer05JailButton;
        private static CustomButton policeplayer05KillButton;
        private static CustomButton policeplayer05LightButton;

        private static CustomButton thiefplayer01KillButton;
        private static CustomButton thiefplayer01FreeThiefButton;
        private static CustomButton thiefplayer01TakeDeliverJewelButton;
        private static CustomButton thiefplayer02KillButton;
        private static CustomButton thiefplayer02FreeThiefButton;
        private static CustomButton thiefplayer02TakeDeliverJewelButton;
        private static CustomButton thiefplayer03KillButton;
        private static CustomButton thiefplayer03FreeThiefButton;
        private static CustomButton thiefplayer03TakeDeliverJewelButton;
        private static CustomButton thiefplayer04KillButton;
        private static CustomButton thiefplayer04FreeThiefButton;
        private static CustomButton thiefplayer04TakeDeliverJewelButton;
        private static CustomButton thiefplayer05KillButton;
        private static CustomButton thiefplayer05FreeThiefButton;
        private static CustomButton thiefplayer05TakeDeliverJewelButton;
        private static CustomButton thiefplayer06KillButton;
        private static CustomButton thiefplayer06FreeThiefButton;
        private static CustomButton thiefplayer06TakeDeliverJewelButton;
        private static CustomButton thiefplayer07KillButton;
        private static CustomButton thiefplayer07FreeThiefButton;
        private static CustomButton thiefplayer07TakeDeliverJewelButton;
        private static CustomButton thiefplayer08KillButton;
        private static CustomButton thiefplayer08FreeThiefButton;
        private static CustomButton thiefplayer08TakeDeliverJewelButton;
        private static CustomButton thiefplayer09KillButton;
        private static CustomButton thiefplayer09FreeThiefButton;
        private static CustomButton thiefplayer09TakeDeliverJewelButton;
        private static CustomButton thiefplayer10KillButton;
        private static CustomButton thiefplayer10FreeThiefButton;
        private static CustomButton thiefplayer10TakeDeliverJewelButton;

        // King of the hill buttons
        private static CustomButton greenKingplayerKillButton;
        private static CustomButton greenKingplayerCaptureZoneButton;
        private static CustomButton greenplayer01KillButton;
        private static CustomButton greenplayer02KillButton;
        private static CustomButton greenplayer03KillButton;
        private static CustomButton greenplayer04KillButton;
        private static CustomButton greenplayer05KillButton;
        private static CustomButton greenplayer06KillButton;
        private static CustomButton yellowKingplayerKillButton;
        private static CustomButton yellowKingplayerCaptureZoneButton;
        private static CustomButton yellowplayer01KillButton;
        private static CustomButton yellowplayer02KillButton;
        private static CustomButton yellowplayer03KillButton;
        private static CustomButton yellowplayer04KillButton;
        private static CustomButton yellowplayer05KillButton;
        private static CustomButton yellowplayer06KillButton;
        private static CustomButton usurperPlayerKillButton;

        // Hot Potato button
        public static CustomButton hotPotatoButton;

        // ZombieLaboratory
        private static CustomButton zombie01InfectButton;
        private static CustomButton zombie01KillButton;
        private static CustomButton zombie02InfectButton;
        private static CustomButton zombie02KillButton;
        private static CustomButton zombie03InfectButton;
        private static CustomButton zombie03KillButton;
        private static CustomButton zombie04InfectButton;
        private static CustomButton zombie04KillButton;
        private static CustomButton zombie05InfectButton;
        private static CustomButton zombie05KillButton;
        private static CustomButton zombie06InfectButton;
        private static CustomButton zombie06KillButton;
        private static CustomButton zombie07InfectButton;
        private static CustomButton zombie07KillButton;
        private static CustomButton zombie08InfectButton;
        private static CustomButton zombie08KillButton;
        private static CustomButton zombie09InfectButton;
        private static CustomButton zombie09KillButton;
        private static CustomButton zombie10InfectButton;
        private static CustomButton zombie10KillButton;
        private static CustomButton zombie11InfectButton;
        private static CustomButton zombie11KillButton;
        private static CustomButton zombie12InfectButton;
        private static CustomButton zombie12KillButton;
        private static CustomButton zombie13InfectButton;
        private static CustomButton zombie13KillButton;
        private static CustomButton zombie14InfectButton;
        private static CustomButton zombie14KillButton;
        private static CustomButton survivor01KillButton;
        private static CustomButton survivor01FindDeliverButton;
        private static CustomButton survivor01EnterExitButton;
        private static CustomButton survivor02KillButton;
        private static CustomButton survivor02FindDeliverButton;
        private static CustomButton survivor02EnterExitButton;
        private static CustomButton survivor03KillButton;
        private static CustomButton survivor03FindDeliverButton;
        private static CustomButton survivor03EnterExitButton;
        private static CustomButton survivor04KillButton;
        private static CustomButton survivor04FindDeliverButton;
        private static CustomButton survivor04EnterExitButton;
        private static CustomButton survivor05KillButton;
        private static CustomButton survivor05FindDeliverButton;
        private static CustomButton survivor05EnterExitButton;
        private static CustomButton survivor06KillButton;
        private static CustomButton survivor06FindDeliverButton;
        private static CustomButton survivor06EnterExitButton;
        private static CustomButton survivor07KillButton;
        private static CustomButton survivor07FindDeliverButton;
        private static CustomButton survivor07EnterExitButton;
        private static CustomButton survivor08KillButton;
        private static CustomButton survivor08FindDeliverButton;
        private static CustomButton survivor08EnterExitButton;
        private static CustomButton survivor09KillButton;
        private static CustomButton survivor09FindDeliverButton;
        private static CustomButton survivor09EnterExitButton;
        private static CustomButton survivor10KillButton;
        private static CustomButton survivor10FindDeliverButton;
        private static CustomButton survivor10EnterExitButton;
        private static CustomButton survivor11KillButton;
        private static CustomButton survivor11FindDeliverButton;
        private static CustomButton survivor11EnterExitButton;
        private static CustomButton survivor12KillButton;
        private static CustomButton survivor12FindDeliverButton;
        private static CustomButton survivor12EnterExitButton;
        private static CustomButton survivor13KillButton;
        private static CustomButton survivor13FindDeliverButton;
        private static CustomButton survivor13EnterExitButton;
        private static CustomButton nurseEnterExitButton;
        private static CustomButton nurseMedKitButton;
        private static CustomButton nurseCreateCureButton;

        public static void setCustomButtonCooldowns() {
            // Impostor buttons
            mimicTransformButton.MaxTimer = Mimic.cooldown;
            mimicTransformButton.EffectDuration = Mimic.duration;
            painterPaintButton.MaxTimer = Painter.cooldown;
            painterPaintButton.EffectDuration = Painter.duration;
            demonKillButton.MaxTimer = Demon.cooldown;
            demonKillButton.EffectDuration = Demon.delay;
            nunButton.MaxTimer = 10f;
            janitorCleanButton.MaxTimer = Janitor.cooldown;
            janitorDragBodyButton.MaxTimer = 10;
            placeHatButton.MaxTimer = Ilusionist.placeHatCooldown;
            ilusionistLightsOutButton.MaxTimer = Ilusionist.lightsOutCooldown;
            ilusionistLightsOutButton.EffectDuration = Ilusionist.lightsOutDuration;
            ilusionistLightsOutButton.Timer = ilusionistLightsOutButton.MaxTimer;
            manipulatorManipulateButton.MaxTimer = Manipulator.cooldown;
            bombermanBombButton.MaxTimer = Bomberman.bombCooldown;
            bombermanBombButton.EffectDuration = Bomberman.bombDuration;
            chameleonInvisibleButton.MaxTimer = Chameleon.cooldown;
            chameleonInvisibleButton.EffectDuration = Chameleon.duration;
            sorcererSpellButton.MaxTimer = Sorcerer.cooldown;
            sorcererSpellButton.EffectDuration = Sorcerer.spellDuration;

            // Rebels buttons
            renegadeKillButton.MaxTimer = Renegade.cooldown;
            renegadeMinionButton.MaxTimer = Renegade.createMinionCooldown;
            minionKillButton.MaxTimer = Minion.cooldown;
            bountyHunterKillButton.MaxTimer = BountyHunter.cooldown;
            bountyHunterSetKillButton.MaxTimer = BountyHunter.cooldown;
            trapperMineButton.MaxTimer = Trapper.cooldown;
            trapperTrapButton.MaxTimer = Trapper.cooldown;
            trapperKillButton.MaxTimer = 30f;
            yinyangerYinButton.MaxTimer = Yinyanger.cooldown;
            yinyangerYangButton.MaxTimer = Yinyanger.cooldown;
            yinyangerKillButton.MaxTimer = 30f;
            challengerChallengeButton.MaxTimer = Challenger.cooldown;
            challengerChallengeButton.EffectDuration = Challenger.duration;
            challengerKillButton.MaxTimer = 30f;
            challengerRockButton.MaxTimer = 10f;
            challengerPaperButton.MaxTimer = 10f;
            challengerScissorsButton.MaxTimer = 10f;
            rivalplayerRockButton.MaxTimer = 10f;
            rivalplayerPaperButton.MaxTimer = 10f;
            rivalplayerScissorsButton.MaxTimer = 10f;

            // Neutral buttons
            roleThiefStealButton.MaxTimer = RoleThief.cooldown;
            pyromaniacButton.MaxTimer = Pyromaniac.cooldown;
            pyromaniacButton.EffectDuration = Pyromaniac.duration;
            treasureHunterButton.MaxTimer = TreasureHunter.cooldown;
            devourerButton.MaxTimer = Devourer.cooldown;

            // Crewmate buttons
            captainCallButton.MaxTimer = 10f;
            mechanicRepairButton.MaxTimer = 10f;
            sheriffKillButton.MaxTimer = Sheriff.cooldown;
            detectiveButton.MaxTimer = Detective.cooldown;
            detectiveButton.EffectDuration = Detective.duration; 
            forensicButton.MaxTimer = Forensic.cooldown;
            forensicButton.EffectDuration = Forensic.duration;
            timeTravelerShieldButton.MaxTimer = TimeTraveler.cooldown;
            timeTravelerRewindTimeButton.MaxTimer = 30f;
            timeTravelerShieldButton.EffectDuration = TimeTraveler.shieldDuration;
            squireShieldButton.MaxTimer = 10f;
            fortuneTellerRevealButton.MaxTimer = FortuneTeller.cooldown;
            fortuneTellerRevealButton.EffectDuration = FortuneTeller.duration;
            hackerButton.MaxTimer = Hacker.cooldown;
            hackerButton.EffectDuration = Hacker.duration;
            hackerVitalsButton.MaxTimer = Hacker.cooldown;
            hackerAdminTableButton.MaxTimer = Hacker.cooldown;
            hackerVitalsButton.EffectDuration = Hacker.duration;
            hackerAdminTableButton.EffectDuration = Hacker.duration;
            sleuthLocatePlayerButton.MaxTimer = 10f;
            sleuthLocateCorpsesButton.MaxTimer = Sleuth.corpsesPathfindCooldown;
            sleuthLocateCorpsesButton.EffectDuration = Sleuth.corpsesPathfindDuration;
            finkButton.MaxTimer = Fink.cooldown;
            finkButton.EffectDuration = Fink.duration; 
            welderSealButton.MaxTimer = Welder.cooldown;
            spiritualistReviveButton.MaxTimer = 30f;
            spiritualistReviveButton.EffectDuration = Spiritualist.spiritualistReviveTime;
            cowardCallButton.MaxTimer = 10f;
            vigilantButton.MaxTimer = Vigilant.cooldown;
            vigilantCamButton.MaxTimer = Vigilant.cooldown;
            vigilantCamButton.EffectDuration = Vigilant.duration;
            medusaPetrifyButton.MaxTimer = Medusa.cooldown;
            medusaPetrifyButton.EffectDuration = Medusa.delay; 
            hunterButton.MaxTimer = 10f;
            jinxButton.MaxTimer = Jinx.cooldown;

            // Remaining uses text
            Mechanic.mechanicRepairButtonText.text = $"{Mechanic.numberOfRepairs - Mechanic.timesUsedRepairs} / {Mechanic.numberOfRepairs}";
            FortuneTeller.fortuneTellerRevealButtonText.text = $"{FortuneTeller.numberOfFortunes - FortuneTeller.timesUsedFortune} / {FortuneTeller.numberOfFortunes}";
            Welder.welderButtonText.text = $"{Welder.remainingWelds} / {Welder.totalWelds}";
            Vigilant.vigilantButtonCameraText.text = $"{Vigilant.remainingCameras} / {Vigilant.totalCameras}";
            Vigilant.vigilantButtonCameraUsesText.text = $"{Vigilant.charges} / {Vigilant.maxCharges}";
            Jinx.jinxButtonJinxsText.text = $"{Jinx.jinxNumber - Jinx.jinxs} / {Jinx.jinxNumber}";
            Hacker.hackerAdminTableChargesText.text = $"{Hacker.chargesAdminTable} / {Hacker.toolsNumber}";
            Hacker.hackerVitalsChargesText.text = $"{Hacker.chargesVitals} / {Hacker.toolsNumber}";
            Coward.cowardCallButtonText.text = $"{Coward.numberOfCalls - Coward.timesUsedCalls} / {Coward.numberOfCalls}";

            // Capture the flag buttons
            redplayer01KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            redplayer01TakeFlagButton.MaxTimer = 0;
            redplayer02KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            redplayer02TakeFlagButton.MaxTimer = 0;
            redplayer03KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            redplayer03TakeFlagButton.MaxTimer = 0;
            redplayer04KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            redplayer04TakeFlagButton.MaxTimer = 0;
            redplayer05KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            redplayer05TakeFlagButton.MaxTimer = 0;
            redplayer06KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            redplayer06TakeFlagButton.MaxTimer = 0;
            redplayer07KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            redplayer07TakeFlagButton.MaxTimer = 0;
            blueplayer01KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            blueplayer01TakeFlagButton.MaxTimer = 0;
            blueplayer02KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            blueplayer02TakeFlagButton.MaxTimer = 0;
            blueplayer03KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            blueplayer03TakeFlagButton.MaxTimer = 0;
            blueplayer04KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            blueplayer04TakeFlagButton.MaxTimer = 0;
            blueplayer05KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            blueplayer05TakeFlagButton.MaxTimer = 0;
            blueplayer06KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            blueplayer06TakeFlagButton.MaxTimer = 0;
            blueplayer07KillButton.MaxTimer = CaptureTheFlag.killCooldown;
            blueplayer07TakeFlagButton.MaxTimer = 0;
            stealerPlayerKillButton.MaxTimer = CaptureTheFlag.killCooldown;

            // Police And Thief buttons
            policeplayer01KillButton.MaxTimer = PoliceAndThief.policeKillCooldown;
            policeplayer01JailButton.MaxTimer = PoliceAndThief.policeCatchCooldown;
            policeplayer01JailButton.EffectDuration = PoliceAndThief.captureThiefTime;
            policeplayer01LightButton.MaxTimer = 20;
            policeplayer01LightButton.EffectDuration = 10;
            policeplayer02KillButton.MaxTimer = PoliceAndThief.policeKillCooldown;
            policeplayer02JailButton.MaxTimer = PoliceAndThief.policeCatchCooldown;
            policeplayer02JailButton.EffectDuration = PoliceAndThief.captureThiefTime;
            policeplayer02LightButton.MaxTimer = 20;
            policeplayer02LightButton.EffectDuration = 10;
            policeplayer03KillButton.MaxTimer = PoliceAndThief.policeKillCooldown;
            policeplayer03JailButton.MaxTimer = PoliceAndThief.policeCatchCooldown;
            policeplayer03JailButton.EffectDuration = PoliceAndThief.captureThiefTime;
            policeplayer03LightButton.MaxTimer = 20;
            policeplayer03LightButton.EffectDuration = 10;
            policeplayer04KillButton.MaxTimer = PoliceAndThief.policeKillCooldown;
            policeplayer04JailButton.MaxTimer = PoliceAndThief.policeCatchCooldown;
            policeplayer04JailButton.EffectDuration = PoliceAndThief.captureThiefTime;
            policeplayer04LightButton.MaxTimer = 20;
            policeplayer04LightButton.EffectDuration = 10;
            policeplayer05KillButton.MaxTimer = PoliceAndThief.policeKillCooldown;
            policeplayer05JailButton.MaxTimer = PoliceAndThief.policeCatchCooldown;
            policeplayer05JailButton.EffectDuration = PoliceAndThief.captureThiefTime;
            policeplayer05LightButton.MaxTimer = 20;
            policeplayer05LightButton.EffectDuration = 10;

            thiefplayer01KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer01FreeThiefButton.MaxTimer = 20f;
            thiefplayer01TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer02KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer02FreeThiefButton.MaxTimer = 20f;
            thiefplayer02TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer03KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer03FreeThiefButton.MaxTimer = 20f;
            thiefplayer03TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer04KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer04FreeThiefButton.MaxTimer = 20f;
            thiefplayer04TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer05KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer05FreeThiefButton.MaxTimer = 20f;
            thiefplayer05TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer06KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer06FreeThiefButton.MaxTimer = 20f;
            thiefplayer06TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer07KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer07FreeThiefButton.MaxTimer = 20f;
            thiefplayer07TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer08KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer08FreeThiefButton.MaxTimer = 20f;
            thiefplayer08TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer09KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer09FreeThiefButton.MaxTimer = 20f;
            thiefplayer09TakeDeliverJewelButton.MaxTimer = 5f;
            thiefplayer10KillButton.MaxTimer = PoliceAndThief.thiefKillCooldown;
            thiefplayer10FreeThiefButton.MaxTimer = 20f;
            thiefplayer10TakeDeliverJewelButton.MaxTimer = 5f;

            // King of the hill buttons
            greenKingplayerCaptureZoneButton.MaxTimer = KingOfTheHill.captureCooldown;
            greenKingplayerKillButton.MaxTimer = KingOfTheHill.killCooldown;
            greenplayer01KillButton.MaxTimer = KingOfTheHill.killCooldown;
            greenplayer02KillButton.MaxTimer = KingOfTheHill.killCooldown;
            greenplayer03KillButton.MaxTimer = KingOfTheHill.killCooldown;
            greenplayer04KillButton.MaxTimer = KingOfTheHill.killCooldown;
            greenplayer05KillButton.MaxTimer = KingOfTheHill.killCooldown;
            greenplayer06KillButton.MaxTimer = KingOfTheHill.killCooldown;
            yellowKingplayerCaptureZoneButton.MaxTimer = KingOfTheHill.captureCooldown;
            yellowKingplayerKillButton.MaxTimer = KingOfTheHill.killCooldown;
            yellowplayer01KillButton.MaxTimer = KingOfTheHill.killCooldown;
            yellowplayer02KillButton.MaxTimer = KingOfTheHill.killCooldown;
            yellowplayer03KillButton.MaxTimer = KingOfTheHill.killCooldown;
            yellowplayer04KillButton.MaxTimer = KingOfTheHill.killCooldown;
            yellowplayer05KillButton.MaxTimer = KingOfTheHill.killCooldown;
            yellowplayer06KillButton.MaxTimer = KingOfTheHill.killCooldown;
            usurperPlayerKillButton.MaxTimer = KingOfTheHill.killCooldown;

            // Hot Potato buttons
            hotPotatoButton.MaxTimer = HotPotato.transferCooldown;

            // ZombieLaboratory
            zombie01InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie01InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie01KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie02InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie02InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie02KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie03InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie03InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie03KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie04InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie04InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie04KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie05InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie05InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie05KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie06InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie06InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie06KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie07InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie07InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie07KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie08InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie08InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie08KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie09InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie09InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie09KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie10InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie10InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie10KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie11InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie11InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie11KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie12InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie12InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie12KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie13InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie13InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie13KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            zombie14InfectButton.MaxTimer = ZombieLaboratory.infectCooldown;
            zombie14InfectButton.EffectDuration = ZombieLaboratory.infectTime;
            zombie14KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor01KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor01FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor01FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor01EnterExitButton.MaxTimer = 10f;
            survivor02KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor02FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor02FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor02EnterExitButton.MaxTimer = 10f;
            survivor03KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor03FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor03FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor03EnterExitButton.MaxTimer = 10f;
            survivor04KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor04FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor04FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor04EnterExitButton.MaxTimer = 10f;
            survivor05KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor05FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor05FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor05EnterExitButton.MaxTimer = 10f;
            survivor06KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor06FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor06FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor06EnterExitButton.MaxTimer = 10f;
            survivor07KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor07FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor07FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor07EnterExitButton.MaxTimer = 10f;
            survivor08KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor08FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor08FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor08EnterExitButton.MaxTimer = 10f;
            survivor09KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor09FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor09FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor09EnterExitButton.MaxTimer = 10f;
            survivor10KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor10FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor10FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor10EnterExitButton.MaxTimer = 10f;
            survivor11KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor11FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor11FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor11EnterExitButton.MaxTimer = 10f;
            survivor12KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor12FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor12FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor12EnterExitButton.MaxTimer = 10f;
            survivor13KillButton.MaxTimer = ZombieLaboratory.killCooldown;
            survivor13FindDeliverButton.MaxTimer = ZombieLaboratory.searchBoxTimer;
            survivor13FindDeliverButton.EffectDuration = ZombieLaboratory.searchBoxTimer;
            survivor13EnterExitButton.MaxTimer = 10f;
            nurseEnterExitButton.MaxTimer = 5f;
            nurseMedKitButton.MaxTimer = 5f;
            nurseCreateCureButton.MaxTimer = 5f;
        }

        public static void resetBomberBombButton() {
            bombermanBombButton.Timer = bombermanBombButton.MaxTimer;
            bombermanBombButton.isEffectActive = false;
            bombermanBombButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        }

        public static void resetTimeTravelerButton() {
            timeTravelerShieldButton.Timer = timeTravelerShieldButton.MaxTimer;
            timeTravelerShieldButton.isEffectActive = false;
            timeTravelerShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        }
        public static void resetSpiritualistReviveButton() {
            spiritualistReviveButton.Timer = 0f;
            spiritualistReviveButton.isEffectActive = false;
        }

        public static void resetDuelButtons() {
            challengerRockButton.Timer = 10f;
            challengerPaperButton.Timer = 10f;
            challengerScissorsButton.Timer = 10f;
            rivalplayerRockButton.Timer = 10f;
            rivalplayerPaperButton.Timer = 10f;
            rivalplayerScissorsButton.Timer = 10f;
        }

        public static void Postfix(HudManager __instance) {
            // Impostor buttons code

            // Mimic transform
            mimicTransformButton = new CustomButton(
                () => {
                    if (Mimic.pickTarget != null) {
                        if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Mimic.mimic.Data.PlayerId)) {
                            MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                            writerKiller.Write(Mimic.mimic.PlayerId);
                            writerKiller.Write((byte)0);
                            AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                            RPCProcedure.setJinxed(Mimic.mimic.PlayerId, 0);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                            Mimic.pickTarget = null;
                            Mimic.duration = quackNumber;
                            mimicTransformButton.EffectDuration = Mimic.duration;
                            return;
                        }

                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MimicTransform, Hazel.SendOption.Reliable, -1);
                        writer.Write(Mimic.pickTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.mimicTransform(Mimic.pickTarget.PlayerId);
                        Mimic.pickTarget = null;
                        Mimic.duration = Mimic.backUpduration;
                        mimicTransformButton.EffectDuration = Mimic.duration;
                    }
                    else if (Mimic.currentTarget != null) {

                        Mimic.pickTarget = Mimic.currentTarget;
                        mimicTransformButton.Sprite = Mimic.getTransformSprite();
                        mimicTransformButton.EffectDuration = 1f;
                    }
                },
                () => { return Mimic.mimic != null && Mimic.mimic == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return (Mimic.currentTarget || Mimic.pickTarget) && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => {
                    mimicTransformButton.Timer = mimicTransformButton.MaxTimer;
                    mimicTransformButton.Sprite = Mimic.getpickTargetSprite();
                    mimicTransformButton.isEffectActive = false;
                    mimicTransformButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    Mimic.pickTarget = null;
                },
                Mimic.getpickTargetSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                Mimic.duration,
                () => {
                    if (Mimic.pickTarget == null) {
                        mimicTransformButton.Timer = mimicTransformButton.MaxTimer;
                        mimicTransformButton.Sprite = Mimic.getpickTargetSprite();
                    }
                }
            );

            // Painter paint
            painterPaintButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Painter.painter.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Painter.painter.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Painter.painter.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        Painter.duration = quackNumber;
                        painterPaintButton.EffectDuration = Painter.duration;
                        return;
                    }

                    int colorNumber = rnd.Next(1, 18);

                    Painter.duration = Painter.backUpduration;
                    painterPaintButton.EffectDuration = Painter.duration;
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PainterPaint, Hazel.SendOption.Reliable, -1);
                    writer.Write(colorNumber);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.painterPaint(colorNumber);
                },
                () => { return Painter.painter != null && Painter.painter == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => {
                    painterPaintButton.Timer = painterPaintButton.MaxTimer;
                    painterPaintButton.isEffectActive = false;
                    painterPaintButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Painter.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                Painter.duration,
                () => { painterPaintButton.Timer = painterPaintButton.MaxTimer; }
            );

            // Demon bite
            demonKillButton = new CustomButton(
                () => {
                    MurderAttemptResult murder = Helpers.checkMurderAttempt(Demon.demon, Demon.currentTarget);
                    if (murder == MurderAttemptResult.PerformKill) {
                        if (Demon.targetNearNun) {
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                            writer.Write(Demon.demon.PlayerId);
                            writer.Write(Demon.currentTarget.PlayerId);
                            writer.Write(Byte.MaxValue);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.uncheckedMurderPlayer(Demon.demon.PlayerId, Demon.currentTarget.PlayerId, Byte.MaxValue);

                            demonKillButton.HasEffect = false;
                            demonKillButton.Timer = demonKillButton.MaxTimer + 20f;
                        }
                        else {
                            Demon.bitten = Demon.currentTarget;
                            
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DemonSetBitten, Hazel.SendOption.Reliable, -1);
                            writer.Write(Demon.bitten.PlayerId);
                            writer.Write((byte)0);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.demonSetBitten(Demon.bitten.PlayerId, 0);

                            HudManager.Instance.StartCoroutine(Effects.Lerp(Demon.delay, new Action<float>((p) => { 
                                if (p == 1f) {
                                   
                                    MurderAttemptResult murder = Helpers.checkMurderAttemptAndKill(Demon.demon, Demon.bitten, showAnimation: false);
                                    if (murder == MurderAttemptResult.JinxKill) {
                                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                                    }
                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DemonSetBitten, Hazel.SendOption.Reliable, -1);
                                    writer.Write(byte.MaxValue);
                                    writer.Write(byte.MaxValue);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.demonSetBitten(byte.MaxValue, byte.MaxValue);
                                }
                            })));

                            demonKillButton.HasEffect = true;
                        }
                    }
                    else if (murder == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        demonKillButton.Timer = demonKillButton.MaxTimer;
                        demonKillButton.HasEffect = false;
                    }
                    else {
                        demonKillButton.HasEffect = false;
                    }
                },
                () => { return Demon.demon != null && Demon.demon == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    if (Demon.targetNearNun && Demon.canKillNearNun || currentAlivePlayers <= 2) {
                        demonKillButton.actionButton.graphic.sprite = __instance.KillButton.graphic.sprite;
                        demonKillButton.showButtonText = true;
                    }
                    else {
                        demonKillButton.actionButton.graphic.sprite = Demon.getButtonSprite();
                        demonKillButton.showButtonText = false;
                    }
                    return Demon.currentTarget != null && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && ((!Demon.targetNearNun || Demon.canKillNearNun) || currentAlivePlayers <= 2);
                },
                () => {
                    demonKillButton.Timer = demonKillButton.MaxTimer;
                    demonKillButton.isEffectActive = false;
                    demonKillButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Demon.getButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q,
                false,
                0f,
                () => {
                    demonKillButton.Timer = demonKillButton.MaxTimer;
                }
            );

            // Nun button only if there's Demon ingame
            nunButton = new CustomButton(
                () => {
                    Demon.localPlacedNun = true;
                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceNun, Hazel.SendOption.Reliable);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.placeNun(buff);
                },
                () => { return !Demon.localPlacedNun && !PlayerControl.LocalPlayer.Data.IsDead && Demon.nunsActive && Demon.demon != null && !Challenger.isDueling; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Demon.localPlacedNun; },
                () => { },
                Demon.getNunButtonSprite(),
                new Vector3(0, -0.06f, 0),
                __instance,
                null,
                true
            );

            // Janitor clean body
            janitorCleanButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Janitor.janitor.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Janitor.janitor.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Janitor.janitor.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        janitorCleanButton.Timer = janitorCleanButton.MaxTimer;
                        return;
                    }

                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), 1f, Constants.PlayersOnlyMask)) {
                        if (collider2D.tag == "DeadBody") {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported) {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false)) {
                                    GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.RemoveBody, Hazel.SendOption.Reliable, -1);
                                    writer.Write(playerInfo.PlayerId);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.removeBody(playerInfo.PlayerId);

                                    Janitor.janitor.killTimer = janitorCleanButton.Timer = janitorCleanButton.MaxTimer;
                                    break;
                                }
                            }
                        }
                    }
                },
                () => { return Janitor.janitor != null && Janitor.janitor == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool canClean = false;
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), 1f, Constants.PlayersOnlyMask))
                        if (collider2D.tag == "DeadBody")
                            canClean = true;
                    return canClean && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && !Janitor.dragginBody;
                },
                () => { janitorCleanButton.Timer = janitorCleanButton.MaxTimer; },
                Janitor.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                null
            );

            // Janitor dragbody button
            janitorDragBodyButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Janitor.janitor.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Janitor.janitor.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Janitor.janitor.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        janitorDragBodyButton.Timer = janitorDragBodyButton.MaxTimer;
                        return;
                    }

                    if (Janitor.dragginBody) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DragPlaceBody, Hazel.SendOption.Reliable, -1);
                        writer.Write(Janitor.bodyId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.dragPlaceBody(Janitor.bodyId);

                        janitorDragBodyButton.Timer = janitorDragBodyButton.MaxTimer;
                    }
                    else {
                        foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), 1f, Constants.PlayersOnlyMask)) {
                            if (collider2D.tag == "DeadBody") {
                                DeadBody component = collider2D.GetComponent<DeadBody>();
                                if (component && !component.Reported) {
                                    Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                    Vector2 truePosition2 = component.TruePosition;
                                    if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false)) {
                                        GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);
                                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DragPlaceBody, Hazel.SendOption.Reliable, -1);
                                        writer.Write(playerInfo.PlayerId);
                                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                                        RPCProcedure.dragPlaceBody(playerInfo.PlayerId);

                                        janitorDragBodyButton.Timer = 1;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                },
                () => { return Janitor.janitor != null && Janitor.janitor == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (Janitor.dragginBody)
                        janitorDragBodyButton.actionButton.graphic.sprite = Janitor.getMoveBodyButtonSprite();
                    else
                        janitorDragBodyButton.actionButton.graphic.sprite = Janitor.getDragButtonSprite();
                    bool canDrag = false;
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), 1f, Constants.PlayersOnlyMask))
                        if (collider2D.tag == "DeadBody")
                            canDrag = true;
                    return canDrag && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => {
                    janitorDragBodyButton.Timer = janitorDragBodyButton.MaxTimer;
                    Janitor.janitorResetValuesAtDead();
                },
                Janitor.getDragButtonSprite(),
                new Vector3(-3f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Ilusionist place hats
            placeHatButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Ilusionist.ilusionist.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Ilusionist.ilusionist.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Ilusionist.ilusionist.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        placeHatButton.Timer = placeHatButton.MaxTimer;
                        return;
                    }

                    placeHatButton.Timer = placeHatButton.MaxTimer;

                    var pos = PlayerControl.LocalPlayer.transform.position;
                    byte[] buff = new byte[sizeof(float) * 2];
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                    Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                    MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceHat, Hazel.SendOption.Reliable);
                    writer.WriteBytesAndSize(buff);
                    writer.EndMessage();
                    RPCProcedure.placeHat(buff);
                },
                () => { return Ilusionist.ilusionist != null && Ilusionist.ilusionist == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && !Hats.hasHatLimitReached(); },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && !Hats.hasHatLimitReached(); },
                () => { placeHatButton.Timer = placeHatButton.MaxTimer; },
                Ilusionist.getPlaceHatButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Ilusionist light button
            ilusionistLightsOutButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Ilusionist.ilusionist.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Ilusionist.ilusionist.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Ilusionist.ilusionist.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        Ilusionist.lightsOutDuration = quackNumber;
                        ilusionistLightsOutButton.EffectDuration = Ilusionist.lightsOutDuration;
                        ilusionistLightsOutButton.Timer = ilusionistLightsOutButton.MaxTimer;
                        return;
                    }

                    Ilusionist.lightsOutDuration = Ilusionist.backUpduration;
                    ilusionistLightsOutButton.EffectDuration = Ilusionist.lightsOutDuration;
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.LightsOut, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.lightsOut();
                },
                () => { return Ilusionist.ilusionist != null && Ilusionist.ilusionist == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Hats.hasHatLimitReached() && Hats.hatsConvertedToVents; },
                () => {
                    bool sabotageActive = false;
                    if (Bomberman.activeBomb == true || Ilusionist.lightsOutTimer > 0) {
                        sabotageActive = true;
                    }
                    else {
                        sabotageActive = Helpers.AnySabotageActive(true);                       
                    }
                    return !sabotageActive && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && Hats.hasHatLimitReached() && Hats.hatsConvertedToVents;
                },
                () => {
                    ilusionistLightsOutButton.Timer = ilusionistLightsOutButton.MaxTimer;
                    ilusionistLightsOutButton.isEffectActive = false;
                    ilusionistLightsOutButton.actionButton.graphic.color = Palette.EnabledColor;
                },
                Ilusionist.getLightsOutButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                Ilusionist.lightsOutDuration,
                () => { ilusionistLightsOutButton.Timer = ilusionistLightsOutButton.MaxTimer; }
            );

            // Manipulator manipulate
            manipulatorManipulateButton = new CustomButton(
                () => {
                    if (Manipulator.manipulatedVictim == null) {
                        
                        Manipulator.manipulatedVictim = Manipulator.currentTarget;
                        manipulatorManipulateButton.Sprite = Manipulator.getManipulateKillButtonSprite();
                        manipulatorManipulateButton.Timer = 1f;
                    }
                    else if (Manipulator.manipulatedVictim != null && Manipulator.manipulatedVictimTarget != null) {
                        MurderAttemptResult murder = Helpers.checkMurderAttemptAndKill(Manipulator.manipulator, Manipulator.manipulatedVictimTarget, showAnimation: false);
                        if (murder == MurderAttemptResult.JinxKill) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                            manipulatorManipulateButton.Timer = manipulatorManipulateButton.MaxTimer;
                            manipulatorManipulateButton.Sprite = Manipulator.getManipulateButtonSprite();
                            Manipulator.manipulatedVictim = null;
                            Manipulator.manipulatedVictimTarget = null;
                            return;
                        }
                        else if (murder == MurderAttemptResult.SuppressKill) {
                            return;
                        }                       

                        Manipulator.manipulatedVictim = null;
                        Manipulator.manipulatedVictimTarget = null;
                        manipulatorManipulateButton.Sprite = Manipulator.getManipulateButtonSprite();
                        Manipulator.manipulator.killTimer = manipulatorManipulateButton.Timer = manipulatorManipulateButton.MaxTimer;
                        manipulatorManipulateButton.Timer = manipulatorManipulateButton.MaxTimer + 20f;

                    }
                },
                () => { return Manipulator.manipulator != null && Manipulator.manipulator == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return ((Manipulator.manipulatedVictim == null && Manipulator.currentTarget != null) || (Manipulator.manipulatedVictim != null && Manipulator.manipulatedVictimTarget != null)) && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => {
                    manipulatorManipulateButton.Timer = manipulatorManipulateButton.MaxTimer;
                    manipulatorManipulateButton.Sprite = Manipulator.getManipulateButtonSprite();
                    Manipulator.manipulatedVictim = null;
                    Manipulator.manipulatedVictimTarget = null;
                },
                Manipulator.getManipulateButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            //Bomberman place bomb
            bombermanBombButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Bomberman.bomberman.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Bomberman.bomberman.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Bomberman.bomberman.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        Bomberman.bombDuration = 0f;
                        bombermanBombButton.EffectDuration = Bomberman.bombDuration;
                        return;
                    }

                    switch (PlayerControl.GameOptions.MapId) {
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

                    bombermanBombButton.EffectDuration = Bomberman.bombDuration;

                    SoundManager.Instance.PlaySound(CustomMain.customAssets.bombermanPlaceBombClip, false, 100f);
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceBomb, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.placeBomb();
                },
                () => { return Bomberman.bomberman != null && Bomberman.bomberman == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool closetoPlayer = false;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (Vector2.Distance(player.transform.position, Bomberman.bomberman.transform.position) < 1.5f && player != Bomberman.bomberman && !player.Data.IsDead) {
                            closetoPlayer = true;
                        }
                    }
                    bool sabotageActive = false;
                    sabotageActive = Helpers.AnySabotageActive(true);

                    return !closetoPlayer && !sabotageActive && PlayerControl.LocalPlayer.CanMove && !Bomberman.activeBomb && !Challenger.isDueling && Ilusionist.lightsOutTimer <= 0;
                },
                () => {
                    bombermanBombButton.Timer = bombermanBombButton.MaxTimer;
                    bombermanBombButton.isEffectActive = false;
                    bombermanBombButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Bomberman.getBombButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                Bomberman.bombDuration,
                () => { bombermanBombButton.Timer = bombermanBombButton.MaxTimer; }
            );

            // Chameleon invisible
            chameleonInvisibleButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Chameleon.chameleon.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Chameleon.chameleon.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Chameleon.chameleon.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        Chameleon.duration = quackNumber;
                        chameleonInvisibleButton.EffectDuration = Chameleon.duration;
                        chameleonInvisibleButton.Timer = chameleonInvisibleButton.MaxTimer;
                        return;
                    }

                    Chameleon.duration = Chameleon.backUpduration;
                    chameleonInvisibleButton.EffectDuration = Chameleon.duration;
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChameleonInvisible, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.chameleonInvisible();
                },
                () => { return Chameleon.chameleon != null && Chameleon.chameleon == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.Data.Role.IsImpostor; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => {
                    chameleonInvisibleButton.Timer = chameleonInvisibleButton.MaxTimer;
                    chameleonInvisibleButton.isEffectActive = false;
                    chameleonInvisibleButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Chameleon.getInvisibleButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                0f,
                () => {
                    chameleonInvisibleButton.Timer = chameleonInvisibleButton.MaxTimer;
                }
            );

            // Sorcerer Spell button
            sorcererSpellButton = new CustomButton(
                () => {
                    if (Sorcerer.currentTarget != null) {
                        Sorcerer.spellTarget = Sorcerer.currentTarget;
                    }
                },
                () => { return Sorcerer.sorcerer != null && Sorcerer.sorcerer == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (sorcererSpellButton.isEffectActive && Sorcerer.spellTarget != Sorcerer.currentTarget) {
                        Sorcerer.spellTarget = null;
                        sorcererSpellButton.Timer = 0f;
                        sorcererSpellButton.isEffectActive = false;
                    }
                    return PlayerControl.LocalPlayer.CanMove && Sorcerer.currentTarget != null && !Challenger.isDueling;
                },
                () => {
                    sorcererSpellButton.Timer = sorcererSpellButton.MaxTimer;
                    sorcererSpellButton.isEffectActive = false;
                    Sorcerer.spellTarget = null;
                    Sorcerer.cooldownAddition = Sorcerer.cooldownAdditionInitial;
                },
                Sorcerer.getSpellButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                Sorcerer.spellDuration,
                () => {
                    if (Sorcerer.spellTarget == null) return;
                    MurderAttemptResult attempt = Helpers.checkMurderAttempt(Sorcerer.sorcerer, Sorcerer.spellTarget);
                    if (attempt == MurderAttemptResult.PerformKill) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetSpelledPlayer, Hazel.SendOption.Reliable, -1);
                        writer.Write(Sorcerer.currentTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.setSpelledPlayer(Sorcerer.currentTarget.PlayerId);
                    }
                    if (attempt == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        sorcererSpellButton.Timer = sorcererSpellButton.MaxTimer;
                        Sorcerer.sorcerer.killTimer = PlayerControl.GameOptions.KillCooldown;
                    }
                    else if (attempt == MurderAttemptResult.PerformKill) {
                        sorcererSpellButton.MaxTimer += Sorcerer.cooldownAddition;
                        sorcererSpellButton.Timer = sorcererSpellButton.MaxTimer;
                        Sorcerer.sorcerer.killTimer = PlayerControl.GameOptions.KillCooldown;
                    }
                    else {
                        sorcererSpellButton.Timer = 0f;
                    }
                    Sorcerer.spellTarget = null;
                }
            );


            // Rebels buttons

            // Renegade Kill
            renegadeKillButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Renegade.renegade, Renegade.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        renegadeKillButton.Timer = renegadeKillButton.MaxTimer;
                        Renegade.currentTarget = null;
                        return;
                    }
                    else if (murderAttemptResult == MurderAttemptResult.SuppressKill) {
                        return;
                    }

                    if (murderAttemptResult == MurderAttemptResult.PerformKill) {
                        
                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                        killWriter.Write(Renegade.renegade.Data.PlayerId);
                        killWriter.Write(Renegade.currentTarget.PlayerId);
                        killWriter.Write(byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.uncheckedMurderPlayer(Renegade.renegade.Data.PlayerId, Renegade.currentTarget.PlayerId, Byte.MaxValue);
                    }

                    renegadeKillButton.Timer = renegadeKillButton.MaxTimer;
                    Renegade.currentTarget = null; 
                },
                () => { return Renegade.renegade != null && Renegade.renegade == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Renegade.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { renegadeKillButton.Timer = renegadeKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Renegade recruit
            renegadeMinionButton = new CustomButton(
                () => {
                    if (Helpers.checkMurderAttempt(Renegade.renegade, Renegade.currentTarget) == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        renegadeMinionButton.Timer = renegadeMinionButton.MaxTimer;
                        Renegade.currentTarget = null;
                        return;
                    }

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.RenegadeRecruitMinion, Hazel.SendOption.Reliable, -1);
                    writer.Write(Renegade.currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.renegadeRecruitMinion(Renegade.currentTarget.PlayerId);
                },
                () => { return Renegade.canRecruitMinion && Renegade.renegade != null && Renegade.renegade == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && !Renegade.usedRecruit; },
                () => { return Renegade.canRecruitMinion && Renegade.currentTarget != null && PlayerControl.LocalPlayer.CanMove && !Renegade.usedRecruit && !Challenger.isDueling; },
                () => { renegadeMinionButton.Timer = renegadeMinionButton.MaxTimer; },
                Renegade.getMinionButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Minion Kill
            minionKillButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Minion.minion, Minion.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        minionKillButton.Timer = minionKillButton.MaxTimer;
                        Minion.currentTarget = null;
                        return;
                    }
                    else if (murderAttemptResult == MurderAttemptResult.SuppressKill) {
                        return;
                    }

                    if (murderAttemptResult == MurderAttemptResult.PerformKill) {

                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                        killWriter.Write(Minion.minion.Data.PlayerId);
                        killWriter.Write(Minion.currentTarget.PlayerId);
                        killWriter.Write(byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.uncheckedMurderPlayer(Minion.minion.Data.PlayerId, Minion.currentTarget.PlayerId, Byte.MaxValue);
                    }

                    minionKillButton.Timer = minionKillButton.MaxTimer;
                    Minion.currentTarget = null; 
                },
                () => { return Minion.minion != null && Minion.minion == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Minion.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { minionKillButton.Timer = minionKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // BountyHunter Kill
            bountyHunterKillButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(BountyHunter.bountyhunter, BountyHunter.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        bountyHunterKillButton.Timer = bountyHunterKillButton.MaxTimer;
                        BountyHunter.currentTarget = null;
                        return;
                    }
                    else if (murderAttemptResult == MurderAttemptResult.SuppressKill) {
                        return;
                    }
                    else if (murderAttemptResult == MurderAttemptResult.PerformKill) {
                        byte targetId = 0;
                        if (BountyHunter.currentTarget == BountyHunter.hasToKill) {
                            targetId = BountyHunter.currentTarget.PlayerId;
                        }
                        else {
                            targetId = PlayerControl.LocalPlayer.PlayerId;
                        }

                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.BountyHunterKill, Hazel.SendOption.Reliable, -1);
                        killWriter.Write(targetId);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.bountyHunterKill(targetId);
                    }

                    bountyHunterKillButton.Timer = bountyHunterKillButton.MaxTimer;
                    BountyHunter.currentTarget = null;
                },
                () => { return BountyHunter.bountyhunter != null && BountyHunter.bountyhunter == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return BountyHunter.usedTarget && BountyHunter.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { bountyHunterKillButton.Timer = bountyHunterKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Bounty hunter set target
            bountyHunterSetKillButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == BountyHunter.bountyhunter.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(BountyHunter.bountyhunter.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(BountyHunter.bountyhunter.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        bountyHunterSetKillButton.Timer = bountyHunterSetKillButton.MaxTimer;
                        return;
                    }

                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (player != Kid.kid && player != Modifiers.lover1 && player != Modifiers.lover2 && player != BountyHunter.bountyhunter && player != Modifiers.bigchungus) {
                            BountyHunter.possibleTargets.Add(player);
                        }
                    }

                    int bountyTarget = rnd.Next(1, BountyHunter.possibleTargets.Count);

                    PlayerControl finaltarget = Helpers.playerById(BountyHunter.possibleTargets[bountyTarget].PlayerId);

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.BountyHunterSetKill, Hazel.SendOption.Reliable, -1);
                    writer.Write(finaltarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.bountyHunterSetKill(finaltarget.PlayerId);
                },
                () => { return !BountyHunter.usedTarget && BountyHunter.bountyhunter != null && BountyHunter.bountyhunter == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { bountyHunterSetKillButton.Timer = bountyHunterSetKillButton.MaxTimer; },
                BountyHunter.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                0f,
                () => {
                    bountyHunterSetKillButton.Timer = bountyHunterSetKillButton.MaxTimer;
                }
            );

            // Trapper place mine
            trapperMineButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Trapper.trapper.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Trapper.trapper.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Trapper.trapper.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        trapperMineButton.Timer = trapperMineButton.MaxTimer;
                        return;
                    }

                    trapperMineButton.Timer = trapperMineButton.MaxTimer;

                    SoundManager.Instance.PlaySound(CustomMain.customAssets.bombermanPlaceBombClip, false, 100f);
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceMine, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.placeMine();
                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers > 2 && Trapper.trapper != null && Trapper.trapper == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    bool closetoPlayer = false;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (Vector2.Distance(player.transform.position, Trapper.trapper.transform.position) < 3f && player != Trapper.trapper && !player.Data.IsDead) {
                            closetoPlayer = true;
                        }
                    }
                    bool closetoMine = false;
                    foreach (Mine mine in Mine.mines) {
                        if (Vector2.Distance(mine.mine.transform.position, Trapper.trapper.transform.position) < 2f) {
                            closetoMine = true;
                        }
                    }
                    return !closetoPlayer && !closetoMine && PlayerControl.LocalPlayer.CanMove && Trapper.currentMineNumber < Trapper.numberOfMines && !Challenger.isDueling;
                },
                () => {
                    trapperMineButton.Timer = trapperMineButton.MaxTimer;
                },
                Trapper.getMineButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Trapper place trap
            trapperTrapButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Trapper.trapper.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Trapper.trapper.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Trapper.trapper.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        trapperTrapButton.Timer = trapperTrapButton.MaxTimer;
                        return;
                    }

                    trapperTrapButton.Timer = trapperTrapButton.MaxTimer;

                    SoundManager.Instance.PlaySound(CustomMain.customAssets.bombermanPlaceBombClip, false, 100f);
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceTrap, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.placeTrap();
                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers > 2 && Trapper.trapper != null && Trapper.trapper == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    bool closetoPlayer = false;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (Vector2.Distance(player.transform.position, Trapper.trapper.transform.position) < 3f && player != Trapper.trapper && !player.Data.IsDead) {
                            closetoPlayer = true;
                        }
                    }
                    bool closetoTrap = false;
                    foreach (Trap trap in Trap.traps) {
                        if (Vector2.Distance(trap.trap.transform.position, Trapper.trapper.transform.position) < 2f) {
                            closetoTrap = true;
                        }
                    }
                    return !closetoPlayer && !closetoTrap && PlayerControl.LocalPlayer.CanMove && Trapper.currentTrapNumber < Trapper.numberOfTraps && !Challenger.isDueling;
                },
                () => {
                    trapperTrapButton.Timer = trapperTrapButton.MaxTimer;
                },
                Trapper.getTrapButtonSprite(),
                new Vector3(-3f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Trapper Kill
            trapperKillButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Trapper.trapper, Trapper.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        trapperKillButton.Timer = trapperKillButton.MaxTimer;
                        Trapper.currentTarget = null;
                        return;
                    }

                    if (Helpers.checkMurderAttemptAndKill(Trapper.trapper, Trapper.currentTarget) == MurderAttemptResult.SuppressKill) return;

                    trapperKillButton.Timer = trapperKillButton.MaxTimer;
                    Trapper.currentTarget = null;
                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers <= 2 && Trapper.trapper != null && Trapper.trapper == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { return Trapper.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { trapperKillButton.Timer = trapperKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Yinyanger set Yin
            yinyangerYinButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Yinyanger.yinyanger, Yinyanger.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        yinyangerYinButton.Timer = yinyangerYinButton.MaxTimer;
                        return;
                    }

                    Yinyanger.yinyedplayer = Yinyanger.currentTarget;
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.yinyangerYinyangClip, false, 100f);
                    // Notify players about who is the Yined
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.YinyangerSetYinyang, Hazel.SendOption.Reliable, -1);
                    writer.Write(Yinyanger.yinyedplayer.PlayerId);
                    writer.Write(0);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.yinyangerSetYinyang(Yinyanger.yinyedplayer.PlayerId, 0);

                    yinyangerYinButton.Timer = yinyangerYangButton.Timer = yinyangerYinButton.MaxTimer;

                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers > 2 && Yinyanger.yinyanger == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    bool closetoYanged = false;
                    if (Yinyanger.yangyedplayer != null) {
                        if (Vector2.Distance(Yinyanger.yinyanger.transform.position, Yinyanger.yangyedplayer.transform.position) < 4f && !Yinyanger.yinyanger.Data.IsDead) {
                            closetoYanged = true;
                        }
                    }
                    bool canYin = true;
                    if (Yinyanger.yangyedplayer != null && Yinyanger.currentTarget == Yinyanger.yangyedplayer) {
                        canYin = false;
                    }
                    return !closetoYanged && canYin && Yinyanger.currentTarget && !Yinyanger.usedYined && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => {
                    yinyangerYinButton.Timer = yinyangerYinButton.MaxTimer;
                },
                Yinyanger.getYinButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Yinyanger set Yang
            yinyangerYangButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Yinyanger.yinyanger, Yinyanger.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        yinyangerYangButton.Timer = yinyangerYangButton.MaxTimer;
                        return;
                    }

                    Yinyanger.yangyedplayer = Yinyanger.currentTarget;
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.yinyangerYinyangClip, false, 100f);
                    // Notify players about who is the Yanged
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.YinyangerSetYinyang, Hazel.SendOption.Reliable, -1);
                    writer.Write(Yinyanger.yangyedplayer.PlayerId);
                    writer.Write(1); AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.yinyangerSetYinyang(Yinyanger.yangyedplayer.PlayerId, 1);

                    yinyangerYangButton.Timer = yinyangerYinButton.Timer = yinyangerYangButton.MaxTimer;
                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers > 2 && Yinyanger.yinyanger == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    bool closetoYined = false;
                    if (Yinyanger.yinyedplayer != null) {
                        if (Vector2.Distance(Yinyanger.yinyanger.transform.position, Yinyanger.yinyedplayer.transform.position) < 4f && !Yinyanger.yinyanger.Data.IsDead) {
                            closetoYined = true;
                        }
                    }
                    bool canYang = true;
                    if (Yinyanger.yinyedplayer != null && Yinyanger.currentTarget == Yinyanger.yinyedplayer) {
                        canYang = false;
                    }
                    return !closetoYined && canYang && Yinyanger.currentTarget && !Yinyanger.usedYanged && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => {
                    yinyangerYangButton.Timer = yinyangerYangButton.MaxTimer;
                },
                Yinyanger.getYangButtonSprite(),
                new Vector3(-3f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Yinyanger Kill
            yinyangerKillButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Yinyanger.yinyanger, Yinyanger.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        yinyangerKillButton.Timer = yinyangerKillButton.MaxTimer;
                        Yinyanger.currentTarget = null;
                        return;
                    }

                    if (Helpers.checkMurderAttemptAndKill(Yinyanger.yinyanger, Yinyanger.currentTarget) == MurderAttemptResult.SuppressKill) return;

                    yinyangerKillButton.Timer = yinyangerKillButton.MaxTimer;
                    Yinyanger.currentTarget = null;
                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers <= 2 && Yinyanger.yinyanger != null && Yinyanger.yinyanger == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { return Yinyanger.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { yinyangerKillButton.Timer = yinyangerKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            //Challenger challenge button
            challengerChallengeButton = new CustomButton(
                () => {
                    MurderAttemptResult murder = Helpers.checkMurderAttempt(Challenger.challenger, Challenger.currentTarget);
                    if (murder == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);

                        Challenger.duration = quackNumber;
                        challengerChallengeButton.EffectDuration = Challenger.duration;
                        challengerChallengeButton.Timer = challengerChallengeButton.MaxTimer;
                        Challenger.currentTarget = null;
                        return;
                    }

                    if (murder == MurderAttemptResult.PerformKill) {
                        Challenger.duration = 10f;
                        challengerChallengeButton.EffectDuration = Challenger.duration;

                        Challenger.rivalPlayer = Challenger.currentTarget;
                        // Notify players about the who is the rival
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerSetRival, Hazel.SendOption.Reliable, -1);
                        writer.Write(Challenger.rivalPlayer.PlayerId);
                        writer.Write(0);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.challengerSetRival(Challenger.rivalPlayer.PlayerId, 0);

                        HudManager.Instance.StartCoroutine(Effects.Lerp(Challenger.duration, new Action<float>((p) => { // Delayed action
                            if (p == 1f) {
                                MurderAttemptResult murder = Helpers.checkMurderAttempt(Challenger.challenger, Challenger.rivalPlayer);
                                if (murder == MurderAttemptResult.JinxKill) {
                                    SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                                    challengerChallengeButton.Timer = challengerChallengeButton.MaxTimer;
                                    Challenger.currentTarget = null;
                                    return;
                                }
                                else if (!Challenger.challenger.Data.IsDead && Challenger.rivalPlayer != null && !Challenger.rivalPlayer.Data.IsDead && murder == MurderAttemptResult.PerformKill /*Helpers.handleMurderAttempt(Challenger.challenger, Challenger.rivalPlayer)*/ && !Challenger.challengerIsInMeeting && !TimeTraveler.isRewinding) {
                                    // Perform duel if no sabotage and player has no squire shield
                                    bool sabotageActive = false;
                                    if (Bomberman.activeBomb == true || Ilusionist.lightsOutTimer > 0) {
                                        sabotageActive = true;
                                    }
                                    else {
                                        sabotageActive = Helpers.AnySabotageActive(true);

                                    }

                                    if (!sabotageActive) {
                                        // If the Demon bitten is the challenger or the rival player, murder him and cancel the duel
                                        if (Demon.demon != null && Demon.bitten != null && (Demon.bitten == Challenger.challenger || Demon.bitten == Challenger.rivalPlayer)) {
                                            Helpers.handleDemonBiteOnBodyReport();
                                        }
                                        else {
                                            // Activate the Demon bitten kill to spawn the body outside the duel arena
                                            if (Demon.demon != null && Demon.bitten != null) {
                                                Helpers.handleDemonBiteOnBodyReport();
                                            }
                                            MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerPerformDuel, Hazel.SendOption.Reliable, -1);
                                            AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                                            RPCProcedure.challengerPerformDuel();
                                        }
                                    }
                                }
                                else {
                                    // Notify players about clearing rivalplayer
                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerSetRival, Hazel.SendOption.Reliable, -1);
                                    writer.Write(byte.MaxValue);
                                    writer.Write(byte.MaxValue);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.challengerSetRival(byte.MaxValue, byte.MaxValue);
                                }
                            }
                        })));
                        challengerChallengeButton.HasEffect = true; // Trigger effect on this click

                    }
                    else {
                        challengerChallengeButton.HasEffect = false; // Block effect if no action was fired
                    }
                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers > 2 && !Challenger.isDueling && Challenger.challenger != null && Challenger.challenger == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    bool sabotageActive = false;
                    if (Bomberman.activeBomb == true || Ilusionist.lightsOutTimer > 0) {
                        sabotageActive = true;
                    }
                    else {
                        sabotageActive = Helpers.AnySabotageActive(true);

                    }
                    return !sabotageActive && Challenger.currentTarget && PlayerControl.LocalPlayer.CanMove;
                },
                () => {
                    challengerChallengeButton.Timer = challengerChallengeButton.MaxTimer;
                    challengerChallengeButton.isEffectActive = false;
                    challengerChallengeButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    Challenger.currentTarget = null;
                    Challenger.challengerIsInMeeting = false;
                },
                Challenger.getChallengeButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                false,
                0f,
                () => {
                    challengerChallengeButton.Timer = challengerChallengeButton.MaxTimer;
                }
            );

            // Challenger Kill
            challengerKillButton = new CustomButton(
                () => {
                    MurderAttemptResult murder = Helpers.checkMurderAttempt(Challenger.challenger, Challenger.currentTarget);
                    if (murder == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        challengerKillButton.Timer = challengerKillButton.MaxTimer;
                        Challenger.currentTarget = null;
                        return;
                    }

                    if (Helpers.checkMurderAttemptAndKill(Challenger.challenger, Challenger.currentTarget) == MurderAttemptResult.SuppressKill) return;

                    challengerKillButton.Timer = challengerKillButton.MaxTimer;
                    Challenger.currentTarget = null;
                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers <= 2 && Challenger.challenger != null && Challenger.challenger == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { return Challenger.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { challengerKillButton.Timer = challengerKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Challenger rock
            challengerRockButton = new CustomButton(
                () => {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerSelectAttack, Hazel.SendOption.Reliable, -1);
                    writer.Write(1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.challengerSelectAttack(1);
                },
                () => { return Challenger.challenger == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Challenger.isDueling; },
                () => { return !Challenger.challengerRock && !Challenger.challengerPaper && !Challenger.challengerScissors && PlayerControl.LocalPlayer.CanMove && Challenger.isDueling && !Challenger.timeOutDuel; },
                () => { },
                Challenger.getRockButtonSprite(),
                new Vector3(-7.4f, -0.06f, 0f),
                __instance,
                null,
                false
            );

            // Challenger paper
            challengerPaperButton = new CustomButton(
                () => {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerSelectAttack, Hazel.SendOption.Reliable, -1);
                    writer.Write(2);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.challengerSelectAttack(2);
                },
                () => { return Challenger.challenger == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Challenger.isDueling; },
                () => { return !Challenger.challengerRock && !Challenger.challengerPaper && !Challenger.challengerScissors && PlayerControl.LocalPlayer.CanMove && Challenger.isDueling && !Challenger.timeOutDuel; },
                () => { },
                Challenger.getPaperButtonSprite(),
                new Vector3(-6.3f, -0.06f, 0f),
                __instance,
                null,
                false
            );

            // Challenger scissors
            challengerScissorsButton = new CustomButton(
                () => {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerSelectAttack, Hazel.SendOption.Reliable, -1);
                    writer.Write(3);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.challengerSelectAttack(3);
                },
                () => { return Challenger.challenger == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Challenger.isDueling; },
                () => { return !Challenger.challengerRock && !Challenger.challengerPaper && !Challenger.challengerScissors && PlayerControl.LocalPlayer.CanMove && Challenger.isDueling && !Challenger.timeOutDuel; },
                () => { },
                Challenger.getScissorsButtonSprite(),
                new Vector3(-5.2f, -0.06f, 0f),
                __instance,
                null,
                false
            );

            // Rivalplayer rock
            rivalplayerRockButton = new CustomButton(
                () => {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerSelectAttack, Hazel.SendOption.Reliable, -1);
                    writer.Write(4);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.challengerSelectAttack(4);
                },
                () => { return Challenger.rivalPlayer == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Challenger.isDueling; },
                () => { return !Challenger.rivalRock && !Challenger.rivalPaper && !Challenger.rivalScissors && PlayerControl.LocalPlayer.CanMove && Challenger.isDueling && !Challenger.timeOutDuel; },
                () => { },
                Challenger.getRockButtonSprite(),
                new Vector3(-7.4f, -0.06f, 0f),
                __instance,
                null,
                false
            );

            // Rivalplayer paper
            rivalplayerPaperButton = new CustomButton(
                () => {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerSelectAttack, Hazel.SendOption.Reliable, -1);
                    writer.Write(5);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.challengerSelectAttack(5);
                },
                () => { return Challenger.rivalPlayer == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Challenger.isDueling; },
                () => { return !Challenger.rivalRock && !Challenger.rivalPaper && !Challenger.rivalScissors && PlayerControl.LocalPlayer.CanMove && Challenger.isDueling && !Challenger.timeOutDuel; },
                () => { },
                Challenger.getPaperButtonSprite(),
                new Vector3(-6.3f, -0.06f, 0f),
                __instance,
                null,
                false
            );

            // Rivalplayer scissors
            rivalplayerScissorsButton = new CustomButton(
                () => {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChallengerSelectAttack, Hazel.SendOption.Reliable, -1);
                    writer.Write(6);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.challengerSelectAttack(6);
                },
                () => { return Challenger.rivalPlayer == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Challenger.isDueling; },
                () => { return !Challenger.rivalRock && !Challenger.rivalPaper && !Challenger.rivalScissors && PlayerControl.LocalPlayer.CanMove && Challenger.isDueling && !Challenger.timeOutDuel; },
                () => { },
                Challenger.getScissorsButtonSprite(),
                new Vector3(-5.2f, -0.06f, 0f),
                __instance,
                null,
                false
            );


            // Neutral buttons code

            // RoleThief steal
            roleThiefStealButton = new CustomButton(
                () => {
                    MurderAttemptResult murder = Helpers.checkMurderAttempt(RoleThief.rolethief, RoleThief.currentTarget);
                    if (murder == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        roleThiefStealButton.Timer = roleThiefStealButton.MaxTimer;
                        RoleThief.currentTarget = null;
                        return;
                    }

                    roleThiefStealButton.Timer = roleThiefStealButton.MaxTimer;

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.RoleThiefSteal, Hazel.SendOption.Reliable, -1);
                    writer.Write(RoleThief.currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    RPCProcedure.roleThiefSteal(RoleThief.currentTarget.PlayerId);
                },
                () => { return RoleThief.rolethief != null && RoleThief.rolethief == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return RoleThief.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { roleThiefStealButton.Timer = roleThiefStealButton.MaxTimer; },
                RoleThief.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Pyromaniac douse ignite
            pyromaniacButton = new CustomButton(
                () => {
                    bool dousedEveryoneAlive = Pyromaniac.sprayedEveryoneAlive();
                    if (dousedEveryoneAlive) {
                        MessageWriter winWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PyromaniacWin, Hazel.SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(winWriter);
                        RPCProcedure.pyromaniacWin();
                        pyromaniacButton.HasEffect = false;
                    }
                    else if (Pyromaniac.currentTarget != null) {
                        Pyromaniac.sprayTarget = Pyromaniac.currentTarget;
                        pyromaniacButton.HasEffect = true;
                    }
                },
                () => { return Pyromaniac.pyromaniac != null && Pyromaniac.pyromaniac == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool dousedEveryoneAlive = Pyromaniac.sprayedEveryoneAlive();
                    if (dousedEveryoneAlive) pyromaniacButton.actionButton.graphic.sprite = Pyromaniac.getIgniteSprite();

                    if (pyromaniacButton.isEffectActive && Pyromaniac.sprayTarget != Pyromaniac.currentTarget) {
                        Pyromaniac.sprayTarget = null;
                        pyromaniacButton.Timer = 0f;
                        pyromaniacButton.isEffectActive = false;
                    }

                    return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && (dousedEveryoneAlive || Pyromaniac.currentTarget != null);
                },
                () => {
                    pyromaniacButton.Timer = pyromaniacButton.MaxTimer;
                    pyromaniacButton.isEffectActive = false;
                    pyromaniacButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    Pyromaniac.sprayTarget = null;
                },
                Pyromaniac.getSpraySprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                Pyromaniac.duration,
                () => {
                    MurderAttemptResult murder = Helpers.checkMurderAttempt(Pyromaniac.pyromaniac, Pyromaniac.sprayTarget);
                    if (murder == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        pyromaniacButton.Timer = pyromaniacButton.MaxTimer;
                        Pyromaniac.sprayTarget = null;
                        return;
                    }

                    if (Pyromaniac.sprayTarget != null) Pyromaniac.sprayedPlayers.Add(Pyromaniac.sprayTarget);
                    Pyromaniac.sprayTarget = null;
                    pyromaniacButton.Timer = Pyromaniac.sprayedEveryoneAlive() ? 0 : pyromaniacButton.MaxTimer;

                    foreach (PlayerControl p in Pyromaniac.sprayedPlayers) {
                        if (MapOptions.playerIcons.ContainsKey(p.PlayerId)) {
                            MapOptions.playerIcons[p.PlayerId].setSemiTransparent(false);
                        }
                    }
                }
            );

            // Treasure Hunter spawn random treasure
            treasureHunterButton = new CustomButton(
                () => {

                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == TreasureHunter.treasureHunter.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(TreasureHunter.treasureHunter.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(TreasureHunter.treasureHunter.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        treasureHunterButton.Timer = treasureHunterButton.MaxTimer;
                        return;
                    }

                    SoundManager.Instance.PlaySound(CustomMain.customAssets.treasureHunterPlaceTreasure, false, 100f);
                    switch (PlayerControl.GameOptions.MapId) {
                        case 0:
                            int skeldNumber = rnd.Next(1, 15);
                            TreasureHunter.randomSpawn = skeldNumber;
                            break;
                        case 1:
                            int miraHQNumber = rnd.Next(1, 13);
                            TreasureHunter.randomSpawn = miraHQNumber;
                            break;
                        case 2:
                            int polusNumber = rnd.Next(1, 21);
                            TreasureHunter.randomSpawn = polusNumber;
                            break;
                        case 3:
                            int dleksNumber = rnd.Next(1, 15);
                            TreasureHunter.randomSpawn = dleksNumber;
                            break;
                        case 4:
                            int airshipNumber = rnd.Next(1, 27);
                            TreasureHunter.randomSpawn = airshipNumber;
                            break;
                        case 5:
                            int submergedNumber = rnd.Next(1, 23);
                            TreasureHunter.randomSpawn = submergedNumber;
                            break;
                    }
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceTreasure, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.placeTreasure();
                },
                () => { return TreasureHunter.treasureHunter != null && TreasureHunter.canPlace == true && TreasureHunter.treasureHunter == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => {
                    treasureHunterButton.Timer = treasureHunterButton.MaxTimer;
                },
                TreasureHunter.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                false,
                0f,
                () => { treasureHunterButton.Timer = treasureHunterButton.MaxTimer; }
            );

            // Devourer devour
            devourerButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Devourer.devourer.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Devourer.devourer.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Devourer.devourer.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        devourerButton.Timer = devourerButton.MaxTimer;
                        return;
                    }

                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), 1f, Constants.PlayersOnlyMask)) {
                        if (collider2D.tag == "DeadBody") {
                            DeadBody component = collider2D.GetComponent<DeadBody>();
                            if (component && !component.Reported) {
                                Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                Vector2 truePosition2 = component.TruePosition;
                                if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false)) {
                                    GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DevourBody, Hazel.SendOption.Reliable, -1);
                                    writer.Write(playerInfo.PlayerId);
                                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                                    RPCProcedure.devourBody(playerInfo.PlayerId);

                                    devourerButton.Timer = devourerButton.MaxTimer;
                                    break;
                                }
                            }
                        }
                    }
                },
                () => { return Devourer.devourer != null && Devourer.devourer == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool canEat = false;
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), 1f, Constants.PlayersOnlyMask))
                        if (collider2D.tag == "DeadBody")
                            canEat = true;
                    return canEat && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => { devourerButton.Timer = devourerButton.MaxTimer; },
                Devourer.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );


            // Crewmates buttons

            // Captain emergency call
            captainCallButton = new CustomButton(
                () => {

                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Captain.captain.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Captain.captain.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Captain.captain.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        captainCallButton.Timer = captainCallButton.MaxTimer;
                        return;
                    }

                    foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks) {
                        if (task.TaskType == TaskTypes.FixLights) {
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MechanicFixLights, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.mechanicFixLights();
                        }
                        else if (task.TaskType == TaskTypes.RestoreOxy) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 0 | 64);
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 1 | 64);
                        }
                        else if (task.TaskType == TaskTypes.ResetReactor) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16);
                        }
                        else if (task.TaskType == TaskTypes.ResetSeismic) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Laboratory, 16);
                        }
                        else if (task.TaskType == TaskTypes.FixComms) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
                        }
                        else if (task.TaskType == TaskTypes.StopCharles) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 0 | 16);
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 1 | 16);
                        }
                    }
                    if (Bomberman.activeBomb == true) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FixBomb, Hazel.SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.fixBomb();
                    }

                    HudManager.Instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => { // Delayed action
                        if (p == 1f) {
                            PlayerControl.LocalPlayer.CmdReportDeadBody(null);
                        }
                    })));
                    captainCallButton.Timer = captainCallButton.MaxTimer;
                },
                () => {
                    int currentAlivePlayers = 0;
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                        if (!player.Data.IsDead) {
                            currentAlivePlayers += 1;
                        }
                    }
                    return currentAlivePlayers <= 2 && Captain.captain != null && Captain.captain == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { captainCallButton.Timer = captainCallButton.MaxTimer; },
                Captain.getCallButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Mechanic Repair
            mechanicRepairButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Mechanic.mechanic.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Mechanic.mechanic.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Mechanic.mechanic.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        mechanicRepairButton.Timer = mechanicRepairButton.MaxTimer;
                        return;
                    }

                    mechanicRepairButton.Timer = mechanicRepairButton.MaxTimer;
                    MessageWriter usedRepairWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MechanicUsedRepair, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(usedRepairWriter);
                    RPCProcedure.mechanicUsedRepair();

                    foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks) {
                        if (task.TaskType == TaskTypes.FixLights) {
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MechanicFixLights, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.mechanicFixLights();
                        }
                        else if (task.TaskType == TaskTypes.RestoreOxy) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 0 | 64);
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 1 | 64);
                        }
                        else if (task.TaskType == TaskTypes.ResetReactor) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 16);
                        }
                        else if (task.TaskType == TaskTypes.ResetSeismic) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Laboratory, 16);
                        }
                        else if (task.TaskType == TaskTypes.FixComms) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 0);
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 16 | 1);
                        }
                        else if (task.TaskType == TaskTypes.StopCharles) {
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 0 | 16);
                            ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 1 | 16);
                        }
                    }
                    if (Bomberman.activeBomb == true) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FixBomb, Hazel.SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.fixBomb();
                    }
                },
                () => { return Mechanic.mechanic != null && Mechanic.mechanic == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool sabotageActive = false;
                    if (Bomberman.activeBomb == true) {
                        sabotageActive = true;
                    }
                    else {
                        foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
                            if (task.TaskType == TaskTypes.FixLights || task.TaskType == TaskTypes.RestoreOxy || task.TaskType == TaskTypes.ResetReactor || task.TaskType == TaskTypes.ResetSeismic || task.TaskType == TaskTypes.FixComms || task.TaskType == TaskTypes.StopCharles)
                                sabotageActive = true; 
                    }
                    return sabotageActive && !Mechanic.usedRepair && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => { },
                Mechanic.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Mechanic button spawn remaining uses text
            Mechanic.mechanicRepairButtonText = GameObject.Instantiate(mechanicRepairButton.actionButton.cooldownTimerText, mechanicRepairButton.actionButton.cooldownTimerText.transform.parent);
            Mechanic.mechanicRepairButtonText.enableWordWrapping = false;
            Mechanic.mechanicRepairButtonText.transform.localScale = Vector3.one * 0.5f;
            Mechanic.mechanicRepairButtonText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Sheriff Kill
            sheriffKillButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Sheriff.sheriff, Sheriff.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                        Sheriff.currentTarget = null;
                        return;
                    }
                    else if (murderAttemptResult == MurderAttemptResult.SuppressKill) {
                        return;
                    }

                    if (murderAttemptResult == MurderAttemptResult.PerformKill) {
                        byte targetId = 0;
                        if ((Sheriff.currentTarget.Data.Role.IsImpostor) ||
                            (Sheriff.canKillNeutrals && (Joker.joker == Sheriff.currentTarget || RoleThief.rolethief == Sheriff.currentTarget || Pyromaniac.pyromaniac == Sheriff.currentTarget || TreasureHunter.treasureHunter == Sheriff.currentTarget || Devourer.devourer == Sheriff.currentTarget)) ||
                            (Renegade.renegade == Sheriff.currentTarget || Minion.minion == Sheriff.currentTarget || BountyHunter.bountyhunter == Sheriff.currentTarget || Trapper.trapper == Sheriff.currentTarget || Yinyanger.yinyanger == Sheriff.currentTarget || Challenger.challenger == Sheriff.currentTarget)) {
                            targetId = Sheriff.currentTarget.PlayerId;
                        }
                        else {
                            targetId = PlayerControl.LocalPlayer.PlayerId;
                        }

                        MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                        killWriter.Write(Sheriff.sheriff.Data.PlayerId);
                        killWriter.Write(targetId);
                        killWriter.Write(byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                        RPCProcedure.uncheckedMurderPlayer(Sheriff.sheriff.Data.PlayerId, targetId, Byte.MaxValue);
                    }

                    sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                    Sheriff.currentTarget = null;
                },
                () => { return Sheriff.sheriff != null && Sheriff.sheriff == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return Sheriff.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0f, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Detective button
            detectiveButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Detective.detective.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Detective.detective.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Detective.detective.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);

                        Detective.duration = quackNumber;
                        detectiveButton.EffectDuration = Detective.duration;
                        detectiveButton.Timer = detectiveButton.MaxTimer;
                        return;
                    }

                    Detective.duration = Detective.backUpduration;
                    detectiveButton.EffectDuration = Detective.duration;
                    detectiveButton.Timer = detectiveButton.MaxTimer;

                    Detective.detectiveTimer = Detective.duration;
                },
                () => { return Detective.detective != null && Detective.detective == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Detective.showFootPrints == 0; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => {
                    detectiveButton.Timer = detectiveButton.MaxTimer;
                    detectiveButton.isEffectActive = false;
                    detectiveButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Detective.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                0f,
                () => {
                    detectiveButton.Timer = detectiveButton.MaxTimer;
                }
            );
            
            // Forensic button
            forensicButton = new CustomButton(
                () => {
                    if (Forensic.target != null) {
                        Forensic.soulTarget = Forensic.target;
                        forensicButton.HasEffect = true;
                    }
                },
                () => { return Forensic.forensic != null && Forensic.forensic == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (forensicButton.isEffectActive && Forensic.target != Forensic.soulTarget) {
                        Forensic.soulTarget = null;
                        forensicButton.Timer = 0f;
                        forensicButton.isEffectActive = false;
                    }
                    return Forensic.target != null && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => {
                    forensicButton.Timer = forensicButton.MaxTimer;
                    forensicButton.isEffectActive = false;
                    Forensic.soulTarget = null;
                },
                Forensic.getQuestionSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                Forensic.duration,
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Forensic.forensic.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Forensic.forensic.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Forensic.forensic.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        forensicButton.Timer = forensicButton.MaxTimer;
                        return;
                    }

                    forensicButton.Timer = forensicButton.MaxTimer;
                    if (Forensic.target == null || Forensic.target.player == null) return;
                    string msg = "";

                    int randomNumber = Forensic.target.killerIfExisting.PlayerId == Kid.kid?.PlayerId ? LasMonjas.rnd.Next(3) : LasMonjas.rnd.Next(4);
                    string typeOfColor = Helpers.isLighterColor(Forensic.target.killerIfExisting.Data.DefaultOutfit.ColorId) ? "lighter (L)" : "darker (D)";
                    float timeSinceDeath = ((float)(Forensic.meetingStartTime - Forensic.target.timeOfDeath).TotalMilliseconds);

                    string name = "(" + Forensic.target.player.Data.PlayerName + "): ";
                    if (randomNumber == 0) msg = name + "I was the " + RoleInfo.GetRolesString(Forensic.target.player, false);
                    else if (randomNumber == 1) msg = name + "My killer has a " + typeOfColor + " color";
                    else if (randomNumber == 2) msg = name + "I've been dead for " + Math.Round(timeSinceDeath / 1000) + " seconds";
                    else msg = name + "My killer was the " + RoleInfo.GetRolesString(Forensic.target.killerIfExisting, false); 

                    DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, $"{msg}");

                    // Remove ghost
                    if (Forensic.oneTimeUse) {
                        float closestDistance = float.MaxValue;
                        SpriteRenderer target = null;

                        foreach ((DeadPlayer db, Vector3 ps) in Forensic.deadBodies) {
                            if (db == Forensic.target) {
                                Tuple<DeadPlayer, Vector3> deadBody = Tuple.Create(db, ps);
                                Forensic.deadBodies.Remove(deadBody);
                                break;
                            }

                        }
                        foreach (SpriteRenderer rend in Forensic.souls) {
                            float distance = Vector2.Distance(rend.transform.position, PlayerControl.LocalPlayer.GetTruePosition());
                            if (distance < closestDistance) {
                                closestDistance = distance;
                                target = rend;
                            }
                        }

                        HudManager.Instance.StartCoroutine(Effects.Lerp(5f, new Action<float>((p) => {
                            if (target != null) {
                                var tmp = target.color;
                                tmp.a = Mathf.Clamp01(1 - p);
                                target.color = tmp;
                            }
                            if (p == 1f && target != null && target.gameObject != null) UnityEngine.Object.Destroy(target.gameObject);
                        })));

                        Forensic.souls.Remove(target);
                    }
                }
            );

            // TimeTraveler shield
            timeTravelerShieldButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == TimeTraveler.timeTraveler.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(TimeTraveler.timeTraveler.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(TimeTraveler.timeTraveler.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        TimeTraveler.shieldDuration = quackNumber;
                        timeTravelerShieldButton.EffectDuration = TimeTraveler.shieldDuration;
                        timeTravelerShieldButton.Timer = timeTravelerShieldButton.MaxTimer;
                        return;
                    }

                    TimeTraveler.shieldDuration = TimeTraveler.backUpduration;
                    timeTravelerShieldButton.EffectDuration = TimeTraveler.shieldDuration;
                    timeTravelerRewindTimeButton.Timer = timeTravelerRewindTimeButton.MaxTimer;
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TimeTravelerShield, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.timeTravelerShield();
                },
                () => { return TimeTraveler.timeTraveler != null && TimeTraveler.timeTraveler == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool sabotageActive = false;
                    if (Bomberman.activeBomb == true || Ilusionist.lightsOutTimer > 0) {
                        sabotageActive = true;
                    }
                    else {
                        sabotageActive = Helpers.AnySabotageActive(true);

                    }
                    return !sabotageActive && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && !TimeTraveler.usedShield;
                },
                () => {
                    timeTravelerShieldButton.Timer = timeTravelerShieldButton.MaxTimer;
                    timeTravelerShieldButton.isEffectActive = false;
                    timeTravelerShieldButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                TimeTraveler.getShieldButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                TimeTraveler.shieldDuration,
                () => { timeTravelerShieldButton.Timer = timeTravelerShieldButton.MaxTimer; }
            );

            // TimeTraveler rewind time
            timeTravelerRewindTimeButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == TimeTraveler.timeTraveler.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(TimeTraveler.timeTraveler.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(TimeTraveler.timeTraveler.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        timeTravelerRewindTimeButton.Timer = timeTravelerRewindTimeButton.MaxTimer;
                        return;
                    }

                    timeTravelerRewindTimeButton.Timer = 0f;
                    TimeTraveler.usedRewind = true;
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TimeTravelerRewindTime, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.timeTravelerRewindTime();
                },
                () => { return TimeTraveler.timeTraveler != null && TimeTraveler.timeTraveler == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool sabotageActive = false;
                    if (Bomberman.activeBomb == true || Ilusionist.lightsOutTimer > 0) {
                        sabotageActive = true;
                    }
                    else {
                        sabotageActive = Helpers.AnySabotageActive(true);

                    }
                    return !sabotageActive && !TimeTraveler.usedRewind && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => { timeTravelerRewindTimeButton.Timer = timeTravelerRewindTimeButton.MaxTimer; },
                TimeTraveler.getRewindButtonSprite(),
                new Vector3(-3f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Squire Shield
            squireShieldButton = new CustomButton(
                () => {
                    MurderAttemptResult murder = Helpers.checkMurderAttempt(Squire.squire, Squire.currentTarget);
                    if (murder == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        squireShieldButton.Timer = squireShieldButton.MaxTimer;
                        Squire.currentTarget = null;
                        return;
                    }

                    squireShieldButton.Timer = 0f;

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SquireSetShielded, Hazel.SendOption.Reliable, -1);
                    writer.Write(Squire.currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.squireSetShielded(Squire.currentTarget.PlayerId);
                },
                () => { return Squire.squire != null && Squire.squire == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return !Squire.usedShield && Squire.currentTarget && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => { if (Squire.resetShieldAfterMeeting) Squire.resetShield(); },
                Squire.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // FortuneTeller reveal
            fortuneTellerRevealButton = new CustomButton(
                () => {
                    if (FortuneTeller.currentTarget != null) {
                        FortuneTeller.revealTarget = FortuneTeller.currentTarget;
                        fortuneTellerRevealButton.HasEffect = true;
                    }
                },
                () => { return FortuneTeller.fortuneTeller != null && FortuneTeller.fortuneTeller == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (fortuneTellerRevealButton.isEffectActive && FortuneTeller.revealTarget != FortuneTeller.currentTarget) {
                        FortuneTeller.revealTarget = null;
                        fortuneTellerRevealButton.Timer = 0f;
                        fortuneTellerRevealButton.isEffectActive = false;
                    }

                    return PlayerControl.LocalPlayer.CanMove && FortuneTeller.currentTarget != null && !FortuneTeller.usedFortune && !Challenger.isDueling;
                },
                () => {
                    FortuneTeller.revealTarget = null;
                    fortuneTellerRevealButton.isEffectActive = false;
                    fortuneTellerRevealButton.Timer = fortuneTellerRevealButton.MaxTimer;
                    fortuneTellerRevealButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                FortuneTeller.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                FortuneTeller.duration,
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == FortuneTeller.fortuneTeller.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(FortuneTeller.fortuneTeller.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(FortuneTeller.fortuneTeller.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        FortuneTeller.revealTarget = null;
                        fortuneTellerRevealButton.Timer = fortuneTellerRevealButton.MaxTimer;
                        return;
                    }

                    if (FortuneTeller.revealTarget != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FortuneTellerReveal, Hazel.SendOption.Reliable, -1);
                        writer.Write(FortuneTeller.currentTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.fortuneTellerReveal(FortuneTeller.currentTarget.PlayerId);
                        FortuneTeller.revealTarget = null;
                        fortuneTellerRevealButton.Timer = fortuneTellerRevealButton.MaxTimer;
                    }
                }
            );

            // FortuneTeller button remaining uses text
            FortuneTeller.fortuneTellerRevealButtonText = GameObject.Instantiate(fortuneTellerRevealButton.actionButton.cooldownTimerText, fortuneTellerRevealButton.actionButton.cooldownTimerText.transform.parent);
            FortuneTeller.fortuneTellerRevealButtonText.enableWordWrapping = false;
            FortuneTeller.fortuneTellerRevealButtonText.transform.localScale = Vector3.one * 0.5f;
            FortuneTeller.fortuneTellerRevealButtonText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Hacker button
            hackerButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Hacker.hacker.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Hacker.hacker.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Hacker.hacker.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);

                        Hacker.duration = quackNumber;
                        hackerButton.EffectDuration = Hacker.duration;
                        hackerButton.Timer = hackerButton.MaxTimer;
                        return;
                    }

                    Hacker.duration = Hacker.backUpduration;
                    hackerButton.EffectDuration = Hacker.duration;
                    hackerButton.Timer = hackerButton.MaxTimer;

                    Hacker.hackerTimer = Hacker.duration;
                },
                () => { return Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => {
                    hackerButton.Timer = hackerButton.MaxTimer;
                    hackerButton.isEffectActive = false;
                    hackerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Hacker.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                0f,
                () => {
                    hackerButton.Timer = hackerButton.MaxTimer;
                }
            );

            hackerAdminTableButton = new CustomButton(
               () => {
                   if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Hacker.hacker.Data.PlayerId)) {
                       MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                       writerKiller.Write(Hacker.hacker.PlayerId);
                       writerKiller.Write((byte)0);
                       AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                       RPCProcedure.setJinxed(Hacker.hacker.PlayerId, 0);

                       SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                       Hacker.duration = quackNumber;
                       hackerAdminTableButton.EffectDuration = Hacker.duration;
                       hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
                       return;
                   }

                   Hacker.duration = Hacker.backUpduration;
                   hackerAdminTableButton.EffectDuration = Hacker.duration;

                   if (!MapBehaviour.Instance || !MapBehaviour.Instance.isActiveAndEnabled)
                       DestroyableSingleton<HudManager>.Instance.ShowMap((System.Action<MapBehaviour>)(m => m.ShowCountOverlay()));

                   PlayerControl.LocalPlayer.NetTransform.Halt();

                   MessageWriter usedAdminWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.HackerAbilityUses, Hazel.SendOption.Reliable, -1);
                   usedAdminWriter.Write(0);
                   AmongUsClient.Instance.FinishRpcImmediately(usedAdminWriter);
                   RPCProcedure.hackerAbilityUses(0); 
               },
               () => { return Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
               () => {
                   if (Hacker.hackerAdminTableChargesText != null) Hacker.hackerAdminTableChargesText.text = $"{Hacker.chargesAdminTable} / {Hacker.toolsNumber}";
                   return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && Hacker.chargesAdminTable > 0;
               },
               () => {
                   hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
                   hackerAdminTableButton.isEffectActive = false;
                   hackerAdminTableButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
               },
               Hacker.getAdminSprite(),
               new Vector3(-3f, -0.06f, 0),
               __instance,
               KeyCode.Q,
               true,
               0f,
               () => {
                   hackerAdminTableButton.Timer = hackerAdminTableButton.MaxTimer;
                   if (MapBehaviour.Instance && MapBehaviour.Instance.isActiveAndEnabled) MapBehaviour.Instance.Close();
               }
           );

            // Hacker Admin Table Charges
            Hacker.hackerAdminTableChargesText = GameObject.Instantiate(hackerAdminTableButton.actionButton.cooldownTimerText, hackerAdminTableButton.actionButton.cooldownTimerText.transform.parent);
            Hacker.hackerAdminTableChargesText.text = "";
            Hacker.hackerAdminTableChargesText.enableWordWrapping = false;
            Hacker.hackerAdminTableChargesText.transform.localScale = Vector3.one * 0.5f;
            Hacker.hackerAdminTableChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            hackerVitalsButton = new CustomButton(
               () => {
                   if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Hacker.hacker.Data.PlayerId)) {
                       MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                       writerKiller.Write(Hacker.hacker.PlayerId);
                       writerKiller.Write((byte)0);
                       AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                       RPCProcedure.setJinxed(Hacker.hacker.PlayerId, 0);

                       SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                       Hacker.duration = quackNumber;
                       hackerVitalsButton.EffectDuration = Hacker.duration;
                       hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer; return;
                   }

                   Hacker.duration = Hacker.backUpduration;
                   hackerVitalsButton.EffectDuration = Hacker.duration;

                   if (Hacker.vitals == null) {
                       var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("panel_vitals"));
                       if (e == null || Camera.main == null) return;
                       Hacker.vitals = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                   }
                   Hacker.vitals.transform.SetParent(Camera.main.transform, false);
                   Hacker.vitals.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                   Hacker.vitals.Begin(null);

                   MessageWriter usedVitalsWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.HackerAbilityUses, Hazel.SendOption.Reliable, -1);
                   usedVitalsWriter.Write(1);
                   AmongUsClient.Instance.FinishRpcImmediately(usedVitalsWriter);
                   RPCProcedure.hackerAbilityUses(1);
               },
               () => { return Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.GameOptions.MapId != 0 && PlayerControl.GameOptions.MapId != 1 && PlayerControl.GameOptions.MapId != 3; },
               () => {
                if (Hacker.hackerVitalsChargesText != null) Hacker.hackerVitalsChargesText.text = $"{Hacker.chargesVitals} / {Hacker.toolsNumber}";
                return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && Hacker.chargesVitals > 0;
               },
               () => {
                   hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
                   hackerVitalsButton.isEffectActive = false;
                   hackerVitalsButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
               },
               Hacker.getVitalsSprite(),
               new Vector3(-4.1f, -0.06f, 0),
               __instance,
               KeyCode.Q,
               true,
               0f,
               () => {
                   hackerVitalsButton.Timer = hackerVitalsButton.MaxTimer;
                   if (Minigame.Instance) Hacker.vitals.ForceClose();
               },
               false
           );

            // Hacker Vitals Charges
            Hacker.hackerVitalsChargesText = GameObject.Instantiate(hackerVitalsButton.actionButton.cooldownTimerText, hackerVitalsButton.actionButton.cooldownTimerText.transform.parent);
            Hacker.hackerVitalsChargesText.text = "";
            Hacker.hackerVitalsChargesText.enableWordWrapping = false;
            Hacker.hackerVitalsChargesText.transform.localScale = Vector3.one * 0.5f;
            Hacker.hackerVitalsChargesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Sleuth locate button
            sleuthLocatePlayerButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Sleuth.sleuth, Sleuth.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        sleuthLocatePlayerButton.Timer = sleuthLocatePlayerButton.MaxTimer;
                        Sleuth.currentTarget = null;
                        return;
                    }

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SleuthUsedLocate, Hazel.SendOption.Reliable, -1);
                    writer.Write(Sleuth.currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.sleuthUsedLocate(Sleuth.currentTarget.PlayerId);
                },
                () => { return Sleuth.sleuth != null && Sleuth.sleuth == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && Sleuth.currentTarget != null && !Sleuth.usedLocate && !Challenger.isDueling; },
                () => { if (Sleuth.resetTargetAfterMeeting) Sleuth.resetLocated(); },
                Sleuth.getLocateButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Sleuth locate corpses
            sleuthLocateCorpsesButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Sleuth.sleuth.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Sleuth.sleuth.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Sleuth.sleuth.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);

                        Sleuth.corpsesPathfindDuration = quackNumber;
                        sleuthLocateCorpsesButton.EffectDuration = Sleuth.corpsesPathfindDuration;
                        sleuthLocateCorpsesButton.Timer = sleuthLocateCorpsesButton.MaxTimer;
                        return;
                    }

                    Sleuth.corpsesPathfindDuration = Sleuth.backUpduration;
                    sleuthLocateCorpsesButton.EffectDuration = Sleuth.corpsesPathfindDuration;

                    Sleuth.corpsesPathfindTimer = Sleuth.corpsesPathfindDuration;
                },
                () => { return Sleuth.sleuth != null && Sleuth.sleuth == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling; },
                () => {
                    sleuthLocateCorpsesButton.Timer = sleuthLocateCorpsesButton.MaxTimer;
                    sleuthLocateCorpsesButton.isEffectActive = false;
                    sleuthLocateCorpsesButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Sleuth.getCorpsePathfindButtonSprite(),
                new Vector3(-3f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                Sleuth.corpsesPathfindDuration,
                () => {
                    sleuthLocateCorpsesButton.Timer = sleuthLocateCorpsesButton.MaxTimer;
                }
            );

            // Fink button
            finkButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Fink.fink.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Fink.fink.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Fink.fink.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);

                        Fink.duration = quackNumber;
                        finkButton.EffectDuration = Fink.duration;
                        finkButton.Timer = finkButton.MaxTimer;
                        return;
                    }

                    Fink.duration = Fink.backUpduration;
                    finkButton.EffectDuration = Fink.duration;
                    finkButton.Timer = finkButton.MaxTimer;

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FinkHawkEye, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.finkHawkEye();

                    PlayerControl.LocalPlayer.NetTransform.Halt();
                    PlayerControl.LocalPlayer.moveable = false;

                    if (Fink.finkCamera == null && Fink.fink == PlayerControl.LocalPlayer) {
                        Fink.finkCamera = GameObject.Find("Main Camera");
                        Fink.finkShadow = GameObject.Find("ShadowQuad");
                    }
                    Fink.finkCamera.GetComponent<Camera>().orthographicSize = 4;
                    Fink.finkShadow.SetActive(false);
                },
                () => { return Fink.fink != null && Fink.fink == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool sabotageActive = false;
                    if (Bomberman.activeBomb == true || Ilusionist.lightsOutTimer > 0) {
                        sabotageActive = true;
                    }
                    else {
                        sabotageActive = Helpers.AnySabotageActive(true);

                    }
                    return !sabotageActive && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => {
                    finkButton.Timer = finkButton.MaxTimer;
                    finkButton.isEffectActive = false;
                    finkButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    Fink.finkCamera.GetComponent<Camera>().orthographicSize = 3;
                    Fink.finkShadow.SetActive(true);
                },
                Fink.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                0f,
                () => {
                    finkButton.Timer = finkButton.MaxTimer;
                    Fink.resetCamera();
                    PlayerControl.LocalPlayer.moveable = true;
                }
            );
            
            // Welder button
            welderSealButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Welder.welder.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Welder.welder.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Welder.welder.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        welderSealButton.Timer = welderSealButton.MaxTimer;
                        return;
                    }

                    if (Welder.ventTarget != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SealVent, Hazel.SendOption.Reliable);
                        writer.WritePacked(Welder.ventTarget.Id);
                        writer.EndMessage();
                        RPCProcedure.sealVent(Welder.ventTarget.Id);
                        Welder.ventsSealed.Add(Welder.ventTarget);
                        Welder.ventTarget = null;
                    }
                    welderSealButton.Timer = welderSealButton.MaxTimer;
                },
                () => { return Welder.welder != null && Welder.welder == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool canSeal = true;
                    if (Welder.ventTarget != null) {
                        if (PlayerControl.GameOptions.MapId == 5) {
                            if (Welder.ventTarget.name == "LowerCentralVent" || Welder.ventTarget.name == "UpperCentralVent" || Welder.ventTarget.name == "OpenEngineVent" || Welder.ventTarget.name == "NormalAdminVent") {
                                canSeal = false;
                            }
                            else {
                                foreach (Vent vent in Welder.ventsSealed) {
                                    if (vent == Welder.ventTarget) {
                                        canSeal = false;
                                    }
                                }
                            }
                        } else {
                            foreach (Vent vent in Welder.ventsSealed) {
                                if (vent == Welder.ventTarget) {
                                    canSeal = false;
                                }
                            }
                        }                      
                    }

                    return Welder.ventTarget != null && canSeal && Welder.remainingWelds > 0 && Welder.remainingWelds <= Welder.totalWelds && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => { welderSealButton.Timer = welderSealButton.MaxTimer; },
                Welder.getCloseVentButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Welder button remaining uses text
            Welder.welderButtonText = GameObject.Instantiate(welderSealButton.actionButton.cooldownTimerText, welderSealButton.actionButton.cooldownTimerText.transform.parent);
            Welder.welderButtonText.enableWordWrapping = false;
            Welder.welderButtonText.transform.localScale = Vector3.one * 0.5f;
            Welder.welderButtonText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            //Spiritualist revive
            spiritualistReviveButton = new CustomButton(
                () => {
                    spiritualistReviveButton.HasEffect = true;

                    MessageWriter isRevivingSpiritualist = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SendSpiritualistIsReviving, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(isRevivingSpiritualist);
                    RPCProcedure.sendSpiritualistIsReviving();
                },
                () => { return Spiritualist.spiritualist != null && !Spiritualist.usedRevive && Spiritualist.spiritualist == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (spiritualistReviveButton.isEffectActive && (!Spiritualist.isReviving || !Spiritualist.canRevive)) {
                        MessageWriter resetSpiritualist = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ResetSpiritualistReviveValues, Hazel.SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(resetSpiritualist);
                        RPCProcedure.resetSpiritualistReviveValues();
                    }
                    Spiritualist.canRevive = false;
                    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), 1f, Constants.PlayersOnlyMask))
                        if (collider2D.tag == "DeadBody")
                            Spiritualist.canRevive = true;
                    return Spiritualist.canRevive && !Spiritualist.usedRevive && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => {
                    spiritualistReviveButton.Timer = spiritualistReviveButton.MaxTimer;
                    spiritualistReviveButton.isEffectActive = false;
                    Spiritualist.isReviving = false;
                },
                Spiritualist.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                Spiritualist.spiritualistReviveTime,
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Spiritualist.spiritualist.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Spiritualist.spiritualist.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Spiritualist.spiritualist.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        spiritualistReviveButton.Timer = spiritualistReviveButton.MaxTimer;
                        return;
                    }

                    if (Spiritualist.isReviving & Spiritualist.canRevive) {
                        foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(PlayerControl.LocalPlayer.GetTruePosition(), 1f, Constants.PlayersOnlyMask)) {
                            if (collider2D.tag == "DeadBody") {
                                DeadBody component = collider2D.GetComponent<DeadBody>();
                                if (component && !component.Reported) {
                                    Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
                                    Vector2 truePosition2 = component.TruePosition;
                                    if (Vector2.Distance(truePosition2, truePosition) <= PlayerControl.LocalPlayer.MaxReportDistance && PlayerControl.LocalPlayer.CanMove && !PhysicsHelpers.AnythingBetween(truePosition, truePosition2, Constants.ShipAndObjectsMask, false)) {
                                        GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);

                                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SpiritualistRevive, Hazel.SendOption.Reliable, -1);
                                        writer.Write(playerInfo.PlayerId);
                                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                                        RPCProcedure.spiritualistRevive(playerInfo.PlayerId);

                                        spiritualistReviveButton.Timer = spiritualistReviveButton.MaxTimer;

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            );

            // Coward call button
            cowardCallButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Coward.coward.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Coward.coward.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Coward.coward.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        cowardCallButton.Timer = cowardCallButton.MaxTimer;
                        return;
                    }

                    cowardCallButton.Timer = cowardCallButton.MaxTimer;
                    MessageWriter usedCallWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CowardUsedCall, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(usedCallWriter);
                    RPCProcedure.cowardUsedCall();

                    HudManager.Instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => { // Delayed action
                        if (p == 1f) {
                            PlayerControl.LocalPlayer.CmdReportDeadBody(null);
                        }
                    })));
                },
                () => { return Coward.coward != null && Coward.coward == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    bool sabotageActive = false;
                    if (Bomberman.activeBomb == true || Ilusionist.lightsOutTimer > 0) {
                        sabotageActive = true;
                    }
                    else {
                        sabotageActive = Helpers.AnySabotageActive(true);

                    }
                    return !sabotageActive && !Coward.usedCalls && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => { },
                Coward.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Coward button spawn remaining uses text
            Coward.cowardCallButtonText = GameObject.Instantiate(cowardCallButton.actionButton.cooldownTimerText, cowardCallButton.actionButton.cooldownTimerText.transform.parent);
            Coward.cowardCallButtonText.enableWordWrapping = false;
            Coward.cowardCallButtonText.transform.localScale = Vector3.one * 0.5f;
            Coward.cowardCallButtonText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);
            
            // Vigilant camera button
            vigilantButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Vigilant.vigilant.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Vigilant.vigilant.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Vigilant.vigilant.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        vigilantButton.Timer = vigilantButton.MaxTimer;
                        return;
                    }

                    if (PlayerControl.GameOptions.MapId != 1) { 
                        var pos = PlayerControl.LocalPlayer.transform.position;
                        byte[] buff = new byte[sizeof(float) * 2];
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.x), 0, buff, 0 * sizeof(float), sizeof(float));
                        Buffer.BlockCopy(BitConverter.GetBytes(pos.y), 0, buff, 1 * sizeof(float), sizeof(float));

                        MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PlaceCamera, Hazel.SendOption.Reliable);
                        writer.WriteBytesAndSize(buff);
                        writer.EndMessage();
                        RPCProcedure.placeCamera(buff);
                    }
                    vigilantButton.Timer = vigilantButton.MaxTimer;
                },
                () => { return Vigilant.vigilant != null && Vigilant.vigilant == PlayerControl.LocalPlayer && Vigilant.placedCameras < 4 && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.GameOptions.MapId != 1; },
                () => {
                    return Vigilant.remainingCameras > 0 && Vigilant.remainingCameras <= Vigilant.totalCameras && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling;
                },
                () => { vigilantButton.Timer = vigilantButton.MaxTimer; },
                Vigilant.getPlaceCameraButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Vigilant button remaining camera text
            Vigilant.vigilantButtonCameraText = GameObject.Instantiate(vigilantButton.actionButton.cooldownTimerText, vigilantButton.actionButton.cooldownTimerText.transform.parent);
            Vigilant.vigilantButtonCameraText.enableWordWrapping = false;
            Vigilant.vigilantButtonCameraText.transform.localScale = Vector3.one * 0.5f;
            Vigilant.vigilantButtonCameraText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Vigilant view cam button
            vigilantCamButton = new CustomButton(
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Vigilant.vigilant.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Vigilant.vigilant.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Vigilant.vigilant.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);

                        Vigilant.duration = quackNumber;
                        vigilantCamButton.EffectDuration = Vigilant.duration;
                        vigilantCamButton.Timer = vigilantCamButton.MaxTimer;
                        return;
                    }

                    switch (PlayerControl.GameOptions.MapId) {
                        case 0:
                            if (Vigilant.minigame == null) {
                                var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvConsole"));
                                if (e == null || Camera.main == null) return;
                                Vigilant.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                            }
                            break;
                        case 2:
                            if (Vigilant.minigame == null) {
                                var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("Surv_Panel"));
                                if (e == null || Camera.main == null) return;
                                Vigilant.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                            }
                            break;
                        case 3:
                            if (Vigilant.minigame == null) {
                                var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SurvConsole"));
                                if (e == null || Camera.main == null) return;
                                Vigilant.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                            }                            
                            break;
                        case 4:
                            if (Vigilant.minigame == null) {
                                var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("task_cams"));
                                if (e == null || Camera.main == null) return;
                                Vigilant.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                            }
                            break;
                        case 5:
                            if (Vigilant.minigame == null) {
                                var e = UnityEngine.Object.FindObjectsOfType<SystemConsole>().FirstOrDefault(x => x.gameObject.name.Contains("SecurityConsole"));
                                if (e == null || Camera.main == null) return;
                                Vigilant.minigame = UnityEngine.Object.Instantiate(e.MinigamePrefab, Camera.main.transform, false);
                            }
                            break;
                    }
                    Vigilant.minigame.transform.SetParent(Camera.main.transform, false);
                    Vigilant.minigame.transform.localPosition = new Vector3(0.0f, 0.0f, -50f);
                    Vigilant.minigame.Begin(null);

                    MessageWriter usedCamsWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.VigilantAbilityUses, Hazel.SendOption.Reliable, -1);
                    usedCamsWriter.Write(0);
                    AmongUsClient.Instance.FinishRpcImmediately(usedCamsWriter);
                    RPCProcedure.vigilantAbilityUses(0);

                    Vigilant.duration = Vigilant.backUpduration;
                    vigilantCamButton.EffectDuration = Vigilant.duration;

                    PlayerControl.LocalPlayer.NetTransform.Halt();
                },
                () => { return Vigilant.vigilant != null && Vigilant.vigilant == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Vigilant.placedCameras >= 4 && PlayerControl.GameOptions.MapId != 1; },
                () => {
                    if (Vigilant.vigilantButtonCameraUsesText != null) Vigilant.vigilantButtonCameraUsesText.text = $"{Vigilant.charges} / {Vigilant.maxCharges}";
                    return PlayerControl.LocalPlayer.CanMove && Vigilant.charges > 0 && !Challenger.isDueling;
                },
                () => {
                    vigilantCamButton.Timer = vigilantCamButton.MaxTimer;
                    vigilantCamButton.isEffectActive = false;
                    vigilantCamButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Vigilant.getCamSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                0f,
                () => {
                    vigilantCamButton.Timer = vigilantCamButton.MaxTimer;
                    if(Minigame.Instance) {
                        Vigilant.minigame.ForceClose();
                    }
                    Vigilant.minigame = null;
                    PlayerControl.LocalPlayer.moveable = true;
                },
                false
            );

            // Vigilant cam button charges
            Vigilant.vigilantButtonCameraUsesText = GameObject.Instantiate(vigilantCamButton.actionButton.cooldownTimerText, vigilantCamButton.actionButton.cooldownTimerText.transform.parent);
            Vigilant.vigilantButtonCameraUsesText.text = "";
            Vigilant.vigilantButtonCameraUsesText.enableWordWrapping = false;
            Vigilant.vigilantButtonCameraUsesText.transform.localScale = Vector3.one * 0.5f;
            Vigilant.vigilantButtonCameraUsesText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);

            // Medusa petrify
            medusaPetrifyButton = new CustomButton(
                () => {
                    if (Medusa.currentTarget != null) {
                        Medusa.petrified = Medusa.currentTarget;
                        medusaPetrifyButton.HasEffect = true;
                    }
                },
                () => { return Medusa.medusa != null && Medusa.medusa == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    return PlayerControl.LocalPlayer.CanMove && Medusa.currentTarget != null && !Challenger.isDueling;
                },
                () => {
                    Medusa.petrified = null;
                    medusaPetrifyButton.isEffectActive = false;
                    medusaPetrifyButton.Timer = medusaPetrifyButton.MaxTimer;
                    medusaPetrifyButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Medusa.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q,
                true,
                Medusa.duration,
                () => {
                    if (Jinx.jinxedList.Any(p => p.Data.PlayerId == Medusa.medusa.Data.PlayerId)) {
                        MessageWriter writerKiller = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writerKiller.Write(Medusa.medusa.PlayerId);
                        writerKiller.Write((byte)0);
                        AmongUsClient.Instance.FinishRpcImmediately(writerKiller);
                        RPCProcedure.setJinxed(Medusa.medusa.PlayerId, 0);

                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        Medusa.petrified = null;
                        medusaPetrifyButton.Timer = medusaPetrifyButton.MaxTimer;
                        return;
                    }

                    if (Medusa.petrified != null && !Medusa.petrified.Data.IsDead) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.medusaPetrify, false, 100f);
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MedusaPetrify, Hazel.SendOption.Reliable, -1);
                        writer.Write(Medusa.petrified.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.medusaPetrify(Medusa.petrified.PlayerId);
                        Medusa.petrified = null;
                    }
                    medusaPetrifyButton.Timer = medusaPetrifyButton.MaxTimer;
                }
            );
            
            // Hunter button
            hunterButton = new CustomButton(
                () => {
                    MurderAttemptResult murderAttemptResult = Helpers.checkMurderAttempt(Hunter.hunter, Hunter.currentTarget);
                    if (murderAttemptResult == MurderAttemptResult.JinxKill) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                        hunterButton.Timer = hunterButton.MaxTimer;
                        Hunter.currentTarget = null;
                        return;
                    }

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.HunterUsedHunted, Hazel.SendOption.Reliable, -1);
                    writer.Write(Hunter.currentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.hunterUsedHunted(Hunter.currentTarget.PlayerId);
                },
                () => { return Hunter.hunter != null && Hunter.hunter == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && Hunter.currentTarget != null && !Hunter.usedHunted && !Challenger.isDueling; },
                () => { if (Hunter.resetTargetAfterMeeting) Hunter.resetHunted(); },
                Hunter.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Jinx button
            jinxButton = new CustomButton(
                () => {
                    if (Jinx.target != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                        writer.Write(Jinx.target.PlayerId);
                        writer.Write(Byte.MaxValue);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.setJinxed(Jinx.target.PlayerId, Byte.MaxValue);

                        Jinx.target = null;

                        jinxButton.Timer = jinxButton.MaxTimer;
                    }

                },
                () => { return Jinx.jinx != null && Jinx.jinx == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && Jinx.jinxs < Jinx.jinxNumber; },
                () => {
                    if (Jinx.jinxButtonJinxsText != null) Jinx.jinxButtonJinxsText.text = $"{Jinx.jinxNumber - Jinx.jinxs} / {Jinx.jinxNumber}";

                    return Jinx.jinxNumber > Jinx.jinxs && PlayerControl.LocalPlayer.CanMove && !Challenger.isDueling && Jinx.target != null;
                },
                () => { jinxButton.Timer = jinxButton.MaxTimer; },
                Jinx.getTargetSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Jinx button jinxs left
            Jinx.jinxButtonJinxsText = GameObject.Instantiate(jinxButton.actionButton.cooldownTimerText, jinxButton.actionButton.cooldownTimerText.transform.parent);
            Jinx.jinxButtonJinxsText.text = "";
            Jinx.jinxButtonJinxsText.enableWordWrapping = false;
            Jinx.jinxButtonJinxsText.transform.localScale = Vector3.one * 0.5f;
            Jinx.jinxButtonJinxsText.transform.localPosition += new Vector3(-0.05f, 0.7f, 0);


            // Capture the flag buttons
            // Redplayer01 Kill
            redplayer01KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.redplayer01currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(1);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 1);
                    redplayer01KillButton.Timer = redplayer01KillButton.MaxTimer;
                    CaptureTheFlag.redplayer01currentTarget = null;
                },
                () => { return CaptureTheFlag.redplayer01 != null && CaptureTheFlag.redplayer01 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.redplayer01currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.redplayer01IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.redPlayerWhoHasBlueFlag; },
                () => { redplayer01KillButton.Timer = redplayer01KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Redplayer01 TakeFlag Button
            redplayer01TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(1);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter redPlayerStoleBlueFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        redPlayerStoleBlueFlag.Write(targetId);
                        redPlayerStoleBlueFlag.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(redPlayerStoleBlueFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 1);
                    }
                    redplayer01TakeFlagButton.Timer = redplayer01TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.redplayer01 != null && CaptureTheFlag.redplayer01 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.redplayer01 == CaptureTheFlag.redPlayerWhoHasBlueFlag)
                        redplayer01TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverBlueFlagButtonSprite();
                    else
                        redplayer01TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeBlueFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.blueflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.redPlayerWhoHasBlueFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.redflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { redplayer01TakeFlagButton.Timer = redplayer01TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeBlueFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Redplayer02 Kill
            redplayer02KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.redplayer02currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(2);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 2);
                    redplayer02KillButton.Timer = redplayer02KillButton.MaxTimer;
                    CaptureTheFlag.redplayer02currentTarget = null;
                },
                () => { return CaptureTheFlag.redplayer02 != null && CaptureTheFlag.redplayer02 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.redplayer02currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.redplayer02IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.redPlayerWhoHasBlueFlag; },
                () => { redplayer02KillButton.Timer = redplayer02KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Redplayer02 TakeFlag Button
            redplayer02TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(1);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter redPlayerStoleBlueFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        redPlayerStoleBlueFlag.Write(targetId);
                        redPlayerStoleBlueFlag.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(redPlayerStoleBlueFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 1);
                    }
                    redplayer02TakeFlagButton.Timer = redplayer02TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.redplayer02 != null && CaptureTheFlag.redplayer02 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.redplayer02 == CaptureTheFlag.redPlayerWhoHasBlueFlag)
                        redplayer02TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverBlueFlagButtonSprite();
                    else
                        redplayer02TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeBlueFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.blueflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.redPlayerWhoHasBlueFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.redflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { redplayer02TakeFlagButton.Timer = redplayer02TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeBlueFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Redplayer03 Kill
            redplayer03KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.redplayer03currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(3);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 3);
                    redplayer03KillButton.Timer = redplayer03KillButton.MaxTimer;
                    CaptureTheFlag.redplayer03currentTarget = null;
                },
                () => { return CaptureTheFlag.redplayer03 != null && CaptureTheFlag.redplayer03 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.redplayer03currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.redplayer03IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.redPlayerWhoHasBlueFlag; },
                () => { redplayer03KillButton.Timer = redplayer03KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Redplayer03 TakeFlag Button
            redplayer03TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(1);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter redPlayerStoleBlueFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        redPlayerStoleBlueFlag.Write(targetId);
                        redPlayerStoleBlueFlag.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(redPlayerStoleBlueFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 1);
                    }
                    redplayer03TakeFlagButton.Timer = redplayer03TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.redplayer03 != null && CaptureTheFlag.redplayer03 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.redplayer03 == CaptureTheFlag.redPlayerWhoHasBlueFlag)
                        redplayer03TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverBlueFlagButtonSprite();
                    else
                        redplayer03TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeBlueFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.blueflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.redPlayerWhoHasBlueFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.redflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { redplayer03TakeFlagButton.Timer = redplayer03TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeBlueFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Redplayer04 Kill
            redplayer04KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.redplayer04currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(4);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 4);
                    redplayer04KillButton.Timer = redplayer04KillButton.MaxTimer;
                    CaptureTheFlag.redplayer04currentTarget = null;
                },
                () => { return CaptureTheFlag.redplayer04 != null && CaptureTheFlag.redplayer04 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.redplayer04currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.redplayer04IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.redPlayerWhoHasBlueFlag; },
                () => { redplayer04KillButton.Timer = redplayer04KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Redplayer04 TakeFlag Button
            redplayer04TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(1);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter redPlayerStoleBlueFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        redPlayerStoleBlueFlag.Write(targetId);
                        redPlayerStoleBlueFlag.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(redPlayerStoleBlueFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 1);
                    }
                    redplayer04TakeFlagButton.Timer = redplayer04TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.redplayer04 != null && CaptureTheFlag.redplayer04 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.redplayer04 == CaptureTheFlag.redPlayerWhoHasBlueFlag)
                        redplayer04TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverBlueFlagButtonSprite();
                    else
                        redplayer04TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeBlueFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.blueflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.redPlayerWhoHasBlueFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.redflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { redplayer04TakeFlagButton.Timer = redplayer04TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeBlueFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Redplayer05 Kill
            redplayer05KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.redplayer05currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(5);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 5);
                    redplayer05KillButton.Timer = redplayer05KillButton.MaxTimer;
                    CaptureTheFlag.redplayer05currentTarget = null;
                },
                () => { return CaptureTheFlag.redplayer05 != null && CaptureTheFlag.redplayer05 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.redplayer05currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.redplayer05IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.redPlayerWhoHasBlueFlag; },
                () => { redplayer05KillButton.Timer = redplayer05KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Redplayer05 TakeFlag Button
            redplayer05TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(1);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter redPlayerStoleBlueFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        redPlayerStoleBlueFlag.Write(targetId);
                        redPlayerStoleBlueFlag.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(redPlayerStoleBlueFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 1);
                    }
                    redplayer05TakeFlagButton.Timer = redplayer05TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.redplayer05 != null && CaptureTheFlag.redplayer05 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.redplayer05 == CaptureTheFlag.redPlayerWhoHasBlueFlag)
                        redplayer05TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverBlueFlagButtonSprite();
                    else
                        redplayer05TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeBlueFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.blueflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.redPlayerWhoHasBlueFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.redflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { redplayer05TakeFlagButton.Timer = redplayer05TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeBlueFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Redplayer06 Kill
            redplayer06KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.redplayer06currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(6);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 6);
                    redplayer06KillButton.Timer = redplayer06KillButton.MaxTimer;
                    CaptureTheFlag.redplayer06currentTarget = null;
                },
                () => { return CaptureTheFlag.redplayer06 != null && CaptureTheFlag.redplayer06 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.redplayer06currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.redplayer06IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.redPlayerWhoHasBlueFlag; },
                () => { redplayer06KillButton.Timer = redplayer06KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Redplayer06 TakeFlag Button
            redplayer06TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(1);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter redPlayerStoleBlueFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        redPlayerStoleBlueFlag.Write(targetId);
                        redPlayerStoleBlueFlag.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(redPlayerStoleBlueFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 1);
                    }
                    redplayer06TakeFlagButton.Timer = redplayer06TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.redplayer06 != null && CaptureTheFlag.redplayer06 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.redplayer06 == CaptureTheFlag.redPlayerWhoHasBlueFlag)
                        redplayer06TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverBlueFlagButtonSprite();
                    else
                        redplayer06TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeBlueFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.blueflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.redPlayerWhoHasBlueFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.redflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { redplayer06TakeFlagButton.Timer = redplayer06TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeBlueFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Redplayer07  Kill
            redplayer07KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.redplayer07currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(7);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 7);
                    redplayer07KillButton.Timer = redplayer07KillButton.MaxTimer;
                    CaptureTheFlag.redplayer07currentTarget = null;
                },
                () => { return CaptureTheFlag.redplayer07 != null && CaptureTheFlag.redplayer07 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.redplayer07currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.redplayer07IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.redPlayerWhoHasBlueFlag; },
                () => { redplayer07KillButton.Timer = redplayer07KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Redplayer07 TakeFlag Button
            redplayer07TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(1);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter redPlayerStoleBlueFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        redPlayerStoleBlueFlag.Write(targetId);
                        redPlayerStoleBlueFlag.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(redPlayerStoleBlueFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 1);
                    }
                    redplayer07TakeFlagButton.Timer = redplayer07TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.redplayer07 != null && CaptureTheFlag.redplayer07 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.redplayer07 == CaptureTheFlag.redPlayerWhoHasBlueFlag)
                        redplayer07TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverBlueFlagButtonSprite();
                    else
                        redplayer07TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeBlueFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.blueflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.redPlayerWhoHasBlueFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.redflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { redplayer07TakeFlagButton.Timer = redplayer07TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeBlueFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Blueplayer01 Kill
            blueplayer01KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.blueplayer01currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(9);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 9);
                    blueplayer01KillButton.Timer = blueplayer01KillButton.MaxTimer;
                    CaptureTheFlag.blueplayer01currentTarget = null;
                },
                () => { return CaptureTheFlag.blueplayer01 != null && CaptureTheFlag.blueplayer01 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.blueplayer01currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.blueplayer01IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.bluePlayerWhoHasRedFlag; },
                () => { blueplayer01KillButton.Timer = blueplayer01KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Blueplayer01 TakeFlag Button
            blueplayer01TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(2);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter bluePlayerStoleRedFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        bluePlayerStoleRedFlag.Write(targetId);
                        bluePlayerStoleRedFlag.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(bluePlayerStoleRedFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 2);
                    }
                    blueplayer01TakeFlagButton.Timer = blueplayer01TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.blueplayer01 != null && CaptureTheFlag.blueplayer01 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.blueplayer01 == CaptureTheFlag.bluePlayerWhoHasRedFlag)
                        blueplayer01TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverRedFlagButtonSprite();
                    else
                        blueplayer01TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeRedFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.redflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.bluePlayerWhoHasRedFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.blueflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { blueplayer01TakeFlagButton.Timer = blueplayer01TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeRedFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Blueplayer02 Kill
            blueplayer02KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.blueplayer02currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(10);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 10);
                    blueplayer02KillButton.Timer = blueplayer02KillButton.MaxTimer;
                    CaptureTheFlag.blueplayer02currentTarget = null;
                },
                () => { return CaptureTheFlag.blueplayer02 != null && CaptureTheFlag.blueplayer02 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.blueplayer02currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.blueplayer02IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.bluePlayerWhoHasRedFlag; },
                () => { blueplayer02KillButton.Timer = blueplayer02KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Blueplayer02 TakeFlag Button
            blueplayer02TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(2);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter bluePlayerStoleRedFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        bluePlayerStoleRedFlag.Write(targetId);
                        bluePlayerStoleRedFlag.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(bluePlayerStoleRedFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 2);
                    }
                    blueplayer02TakeFlagButton.Timer = blueplayer02TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.blueplayer02 != null && CaptureTheFlag.blueplayer02 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.blueplayer02 == CaptureTheFlag.bluePlayerWhoHasRedFlag)
                        blueplayer02TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverRedFlagButtonSprite();
                    else
                        blueplayer02TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeRedFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.redflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.bluePlayerWhoHasRedFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.blueflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { blueplayer02TakeFlagButton.Timer = blueplayer02TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeRedFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Blueplayer03 Kill
            blueplayer03KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.blueplayer03currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(11);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 11);
                    blueplayer03KillButton.Timer = blueplayer03KillButton.MaxTimer;
                    CaptureTheFlag.blueplayer03currentTarget = null;
                },
                () => { return CaptureTheFlag.blueplayer03 != null && CaptureTheFlag.blueplayer03 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.blueplayer03currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.blueplayer03IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.bluePlayerWhoHasRedFlag; },
                () => { blueplayer03KillButton.Timer = blueplayer03KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Blueplayer03 TakeFlag Button
            blueplayer03TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(2);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter bluePlayerStoleRedFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        bluePlayerStoleRedFlag.Write(targetId);
                        bluePlayerStoleRedFlag.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(bluePlayerStoleRedFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 2);
                    }
                    blueplayer03TakeFlagButton.Timer = blueplayer03TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.blueplayer03 != null && CaptureTheFlag.blueplayer03 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.blueplayer03 == CaptureTheFlag.bluePlayerWhoHasRedFlag)
                        blueplayer03TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverRedFlagButtonSprite();
                    else
                        blueplayer03TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeRedFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.redflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.bluePlayerWhoHasRedFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.blueflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { blueplayer03TakeFlagButton.Timer = blueplayer03TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeRedFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Blueplayer04 Kill
            blueplayer04KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.blueplayer04currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(12);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 12);
                    blueplayer04KillButton.Timer = blueplayer04KillButton.MaxTimer;
                    CaptureTheFlag.blueplayer04currentTarget = null;
                },
                () => { return CaptureTheFlag.blueplayer04 != null && CaptureTheFlag.blueplayer04 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.blueplayer04currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.blueplayer04IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.bluePlayerWhoHasRedFlag; },
                () => { blueplayer04KillButton.Timer = blueplayer04KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Blueplayer04 TakeFlag Button
            blueplayer04TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(2);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter bluePlayerStoleRedFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        bluePlayerStoleRedFlag.Write(targetId);
                        bluePlayerStoleRedFlag.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(bluePlayerStoleRedFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 2);
                    }
                    blueplayer04TakeFlagButton.Timer = blueplayer04TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.blueplayer04 != null && CaptureTheFlag.blueplayer04 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.blueplayer04 == CaptureTheFlag.bluePlayerWhoHasRedFlag)
                        blueplayer04TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverRedFlagButtonSprite();
                    else
                        blueplayer04TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeRedFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.redflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.bluePlayerWhoHasRedFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.blueflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { blueplayer04TakeFlagButton.Timer = blueplayer04TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeRedFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Blueplayer05 Kill
            blueplayer05KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.blueplayer05currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(13);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 13);
                    blueplayer05KillButton.Timer = blueplayer05KillButton.MaxTimer;
                    CaptureTheFlag.blueplayer05currentTarget = null;
                },
                () => { return CaptureTheFlag.blueplayer05 != null && CaptureTheFlag.blueplayer05 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.blueplayer05currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.blueplayer05IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.bluePlayerWhoHasRedFlag; },
                () => { blueplayer05KillButton.Timer = blueplayer05KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Blueplayer05 TakeFlag Button
            blueplayer05TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(2);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter bluePlayerStoleRedFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        bluePlayerStoleRedFlag.Write(targetId);
                        bluePlayerStoleRedFlag.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(bluePlayerStoleRedFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 2);
                    }
                    blueplayer05TakeFlagButton.Timer = blueplayer05TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.blueplayer05 != null && CaptureTheFlag.blueplayer05 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.blueplayer05 == CaptureTheFlag.bluePlayerWhoHasRedFlag)
                        blueplayer05TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverRedFlagButtonSprite();
                    else
                        blueplayer05TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeRedFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.redflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.bluePlayerWhoHasRedFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.blueflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { blueplayer05TakeFlagButton.Timer = blueplayer05TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeRedFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Blueplayer06 Kill
            blueplayer06KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.blueplayer06currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(14);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 14);
                    blueplayer06KillButton.Timer = blueplayer06KillButton.MaxTimer;
                    CaptureTheFlag.blueplayer06currentTarget = null;
                },
                () => { return CaptureTheFlag.blueplayer06 != null && CaptureTheFlag.blueplayer06 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.blueplayer06currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.blueplayer06IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.bluePlayerWhoHasRedFlag; },
                () => { blueplayer06KillButton.Timer = blueplayer06KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Blueplayer06 TakeFlag Button
            blueplayer06TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(2);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter bluePlayerStoleRedFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        bluePlayerStoleRedFlag.Write(targetId);
                        bluePlayerStoleRedFlag.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(bluePlayerStoleRedFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 2);
                    }
                    blueplayer06TakeFlagButton.Timer = blueplayer06TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.blueplayer06 != null && CaptureTheFlag.blueplayer06 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.blueplayer06 == CaptureTheFlag.bluePlayerWhoHasRedFlag)
                        blueplayer06TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverRedFlagButtonSprite();
                    else
                        blueplayer06TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeRedFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.redflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.bluePlayerWhoHasRedFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.blueflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { blueplayer06TakeFlagButton.Timer = blueplayer06TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeRedFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Blueplayer07 Kill
            blueplayer07KillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.blueplayer07currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(15);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 15);
                    blueplayer07KillButton.Timer = blueplayer07KillButton.MaxTimer;
                    CaptureTheFlag.blueplayer07currentTarget = null;
                },
                () => { return CaptureTheFlag.blueplayer07 != null && CaptureTheFlag.blueplayer07 == PlayerControl.LocalPlayer; },
                () => { return CaptureTheFlag.blueplayer07currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.blueplayer07IsReviving && PlayerControl.LocalPlayer != CaptureTheFlag.bluePlayerWhoHasRedFlag; },
                () => { blueplayer07KillButton.Timer = blueplayer07KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Blueplayer07 TakeFlag Button
            blueplayer07TakeFlagButton = new CustomButton(
                () => {
                    if (PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        MessageWriter whichTeamScored = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhichTeamScored, Hazel.SendOption.Reliable, -1);
                        whichTeamScored.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(whichTeamScored);
                        RPCProcedure.captureTheFlagWhichTeamScored(2);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        MessageWriter bluePlayerStoleRedFlag = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CaptureTheFlagWhoTookTheFlag, Hazel.SendOption.Reliable, -1);
                        bluePlayerStoleRedFlag.Write(targetId);
                        bluePlayerStoleRedFlag.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(bluePlayerStoleRedFlag);
                        RPCProcedure.captureTheFlagWhoTookTheFlag(targetId, 2);
                    }
                    blueplayer07TakeFlagButton.Timer = blueplayer07TakeFlagButton.MaxTimer;
                },
                () => { return CaptureTheFlag.blueplayer07 != null && CaptureTheFlag.blueplayer07 == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    if (CaptureTheFlag.blueplayer07 == CaptureTheFlag.bluePlayerWhoHasRedFlag)
                        blueplayer07TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getDeliverRedFlagButtonSprite();
                    else
                        blueplayer07TakeFlagButton.actionButton.graphic.sprite = CaptureTheFlag.getTakeRedFlagButtonSprite();
                    bool CanUse = false;
                    if (CaptureTheFlag.redflag != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.redflag.transform.position) < 0.5f && !PlayerControl.LocalPlayer.Data.IsDead && CaptureTheFlag.bluePlayerWhoHasRedFlag == null) {
                        CanUse = true;
                    }
                    else if (CaptureTheFlag.blueflagbase != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, CaptureTheFlag.blueflagbase.transform.position) < 0.5f && PlayerControl.LocalPlayer == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CanUse = true;
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { blueplayer07TakeFlagButton.Timer = blueplayer07TakeFlagButton.MaxTimer; },
                CaptureTheFlag.getTakeRedFlagButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // stealer Kill
            stealerPlayerKillButton = new CustomButton(
                () => {
                    byte targetId = CaptureTheFlag.stealerPlayercurrentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CapturetheFlagKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(16);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.capturetheFlagKills(targetId, 16);
                    stealerPlayerKillButton.Timer = stealerPlayerKillButton.MaxTimer;
                    CaptureTheFlag.stealerPlayercurrentTarget = null;
                },
                () => { return CaptureTheFlag.stealerPlayer != null && CaptureTheFlag.stealerPlayer == PlayerControl.LocalPlayer; },
                () => {
                    if (CaptureTheFlag.localRedFlagArrow.Count != 0) {
                        CaptureTheFlag.localRedFlagArrow[0].Update(CaptureTheFlag.redflag.transform.position);
                    }
                    if (CaptureTheFlag.localBlueFlagArrow.Count != 0) {
                        CaptureTheFlag.localBlueFlagArrow[0].Update(CaptureTheFlag.blueflag.transform.position);
                    }
                    return CaptureTheFlag.stealerPlayercurrentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !CaptureTheFlag.stealerPlayerIsReviving;
                },
                () => { stealerPlayerKillButton.Timer = stealerPlayerKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Police and Thief Mode
            // Policeplayer01 Kill
            policeplayer01KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.policeplayer01currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(1);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 1);
                    policeplayer01KillButton.Timer = policeplayer01KillButton.MaxTimer;
                    PoliceAndThief.policeplayer01currentTarget = null;
                },
                () => { return PoliceAndThief.policeplayer01 != null && PoliceAndThief.policeplayer01 == PlayerControl.LocalPlayer; },
                () => {
                    bool CanUse = true;
                    if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbuttontwo.transform.position) <= 3f || PoliceAndThief.cellbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbutton.transform.position) <= 3f) && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.policeCanKillNearPrison) {
                        CanUse = false;
                    }
                    return CanUse && PoliceAndThief.policeplayer01currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer01IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { policeplayer01KillButton.Timer = policeplayer01KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Policeplayer01 Jail
            policeplayer01JailButton = new CustomButton(
                () => {
                    if (PoliceAndThief.policeplayer01currentTarget != null) {
                        PoliceAndThief.policeplayer01targetedPlayer = PoliceAndThief.policeplayer01currentTarget;
                        policeplayer01JailButton.HasEffect = true;
                    }
                },
                () => { return PoliceAndThief.policeplayer01 != null && PoliceAndThief.policeplayer01 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (policeplayer01JailButton.isEffectActive && PoliceAndThief.policeplayer01targetedPlayer != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.policeplayer01targetedPlayer.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        PoliceAndThief.policeplayer01targetedPlayer = null;
                        policeplayer01JailButton.Timer = 0f;
                        policeplayer01JailButton.isEffectActive = false;
                    }
                    return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer01IsReviving && PoliceAndThief.policeplayer01currentTarget != null;
                },
                () => {
                    PoliceAndThief.policeplayer01targetedPlayer = null;
                    policeplayer01JailButton.isEffectActive = false;
                    policeplayer01JailButton.Timer = policeplayer01JailButton.MaxTimer;
                    policeplayer01JailButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                PoliceAndThief.getCaptureThiefButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                PoliceAndThief.captureThiefTime,
                () => {
                    if (PoliceAndThief.policeplayer01targetedPlayer != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefJail, Hazel.SendOption.Reliable, -1);
                        writer.Write(PoliceAndThief.policeplayer01targetedPlayer.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.policeandThiefJail(PoliceAndThief.policeplayer01targetedPlayer.PlayerId);
                        PoliceAndThief.policeplayer01targetedPlayer = null;
                        policeplayer01JailButton.Timer = policeplayer01JailButton.MaxTimer;
                    }
                }
            );

            // Policeplayer01 Light
            policeplayer01LightButton = new CustomButton(
                () => {
                    PoliceAndThief.policeplayer01lightTimer = 10;
                },
                () => { return PoliceAndThief.policeplayer01 != null && PoliceAndThief.policeplayer01 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer01IsReviving; },
                () => {
                    policeplayer01LightButton.Timer = policeplayer01LightButton.MaxTimer;
                    policeplayer01LightButton.isEffectActive = false;
                    policeplayer01LightButton.actionButton.graphic.color = Palette.EnabledColor;
                },
                PoliceAndThief.getLightButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.F,
                true,
                10,
                () => { policeplayer01LightButton.Timer = policeplayer01LightButton.MaxTimer; }
            );

            // Policeplayer02 Kill
            policeplayer02KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.policeplayer02currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(2);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 2);
                    policeplayer02KillButton.Timer = policeplayer02KillButton.MaxTimer;
                    PoliceAndThief.policeplayer02currentTarget = null;
                },
                () => { return PoliceAndThief.policeplayer02 != null && PoliceAndThief.policeplayer02 == PlayerControl.LocalPlayer; },
                () => {
                    bool CanUse = true;
                    if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbuttontwo.transform.position) <= 3f || PoliceAndThief.cellbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbutton.transform.position) <= 3f) && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.policeCanKillNearPrison) {
                        CanUse = false;
                    }
                    return CanUse && PoliceAndThief.policeplayer02currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer02IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { policeplayer02KillButton.Timer = policeplayer02KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Policeplayer02 Jail
            policeplayer02JailButton = new CustomButton(
                () => {
                    if (PoliceAndThief.policeplayer02currentTarget != null) {
                        PoliceAndThief.policeplayer02targetedPlayer = PoliceAndThief.policeplayer02currentTarget;
                        policeplayer02JailButton.HasEffect = true;
                    }
                },
                () => { return PoliceAndThief.policeplayer02 != null && PoliceAndThief.policeplayer02 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (policeplayer02JailButton.isEffectActive && PoliceAndThief.policeplayer02targetedPlayer != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.policeplayer02targetedPlayer.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        PoliceAndThief.policeplayer02targetedPlayer = null;
                        policeplayer02JailButton.Timer = 0f;
                        policeplayer02JailButton.isEffectActive = false;
                    }
                    return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer02IsReviving && PoliceAndThief.policeplayer02currentTarget != null;
                },
                () => {
                    PoliceAndThief.policeplayer02targetedPlayer = null;
                    policeplayer02JailButton.isEffectActive = false;
                    policeplayer02JailButton.Timer = policeplayer02JailButton.MaxTimer;
                    policeplayer02JailButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                PoliceAndThief.getCaptureThiefButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                PoliceAndThief.captureThiefTime,
                () => {
                    if (PoliceAndThief.policeplayer02targetedPlayer != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefJail, Hazel.SendOption.Reliable, -1);
                        writer.Write(PoliceAndThief.policeplayer02targetedPlayer.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.policeandThiefJail(PoliceAndThief.policeplayer02targetedPlayer.PlayerId);
                        PoliceAndThief.policeplayer02targetedPlayer = null;
                        policeplayer02JailButton.Timer = policeplayer02JailButton.MaxTimer;
                    }
                }
            );

            // Policeplayer02 Light
            policeplayer02LightButton = new CustomButton(
                () => {
                    PoliceAndThief.policeplayer02lightTimer = 10;
                },
                () => { return PoliceAndThief.policeplayer02 != null && PoliceAndThief.policeplayer02 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer02IsReviving; },
                () => {
                    policeplayer02LightButton.Timer = policeplayer02LightButton.MaxTimer;
                    policeplayer02LightButton.isEffectActive = false;
                    policeplayer02LightButton.actionButton.graphic.color = Palette.EnabledColor;
                },
                PoliceAndThief.getLightButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.F,
                true,
                10,
                () => { policeplayer02LightButton.Timer = policeplayer02LightButton.MaxTimer; }
            );

            // Policeplayer03 Kill
            policeplayer03KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.policeplayer03currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(3);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 3);
                    policeplayer03KillButton.Timer = policeplayer03KillButton.MaxTimer;
                    PoliceAndThief.policeplayer03currentTarget = null;
                },
                () => { return PoliceAndThief.policeplayer03 != null && PoliceAndThief.policeplayer03 == PlayerControl.LocalPlayer; },
                () => {
                    bool CanUse = true;
                    if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbuttontwo.transform.position) <= 3f || PoliceAndThief.cellbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbutton.transform.position) <= 3f) && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.policeCanKillNearPrison) {
                        CanUse = false;
                    }
                    return CanUse && PoliceAndThief.policeplayer03currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer03IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { policeplayer03KillButton.Timer = policeplayer03KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Policeplayer03 Jail
            policeplayer03JailButton = new CustomButton(
                () => {
                    if (PoliceAndThief.policeplayer03currentTarget != null) {
                        PoliceAndThief.policeplayer03targetedPlayer = PoliceAndThief.policeplayer03currentTarget;
                        policeplayer03JailButton.HasEffect = true;
                    }
                },
                () => { return PoliceAndThief.policeplayer03 != null && PoliceAndThief.policeplayer03 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (policeplayer03JailButton.isEffectActive && PoliceAndThief.policeplayer03targetedPlayer != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.policeplayer03targetedPlayer.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        PoliceAndThief.policeplayer03targetedPlayer = null;
                        policeplayer03JailButton.Timer = 0f;
                        policeplayer03JailButton.isEffectActive = false;
                    }

                    return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer03IsReviving && PoliceAndThief.policeplayer03currentTarget != null;
                },
                () => {
                    PoliceAndThief.policeplayer03targetedPlayer = null;
                    policeplayer03JailButton.isEffectActive = false;
                    policeplayer03JailButton.Timer = policeplayer03JailButton.MaxTimer;
                    policeplayer03JailButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                PoliceAndThief.getCaptureThiefButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                PoliceAndThief.captureThiefTime,
                () => {
                    if (PoliceAndThief.policeplayer03targetedPlayer != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefJail, Hazel.SendOption.Reliable, -1);
                        writer.Write(PoliceAndThief.policeplayer03targetedPlayer.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.policeandThiefJail(PoliceAndThief.policeplayer03targetedPlayer.PlayerId);
                        PoliceAndThief.policeplayer03targetedPlayer = null;
                        policeplayer03JailButton.Timer = policeplayer03JailButton.MaxTimer;
                    }
                }
            );

            // Policeplayer03 Light
            policeplayer03LightButton = new CustomButton(
                () => {
                    PoliceAndThief.policeplayer03lightTimer = 10;
                },
                () => { return PoliceAndThief.policeplayer03 != null && PoliceAndThief.policeplayer03 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer03IsReviving; },
                () => {
                    policeplayer03LightButton.Timer = policeplayer03LightButton.MaxTimer;
                    policeplayer03LightButton.isEffectActive = false;
                    policeplayer03LightButton.actionButton.graphic.color = Palette.EnabledColor;
                },
                PoliceAndThief.getLightButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.F,
                true,
                10,
                () => { policeplayer03LightButton.Timer = policeplayer03LightButton.MaxTimer; }
            );

            // Policeplayer04 Kill
            policeplayer04KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.policeplayer04currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(4);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 4);
                    policeplayer04KillButton.Timer = policeplayer04KillButton.MaxTimer;
                    PoliceAndThief.policeplayer04currentTarget = null;
                },
                () => { return PoliceAndThief.policeplayer04 != null && PoliceAndThief.policeplayer04 == PlayerControl.LocalPlayer; },
                () => {
                    bool CanUse = true;
                    if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbuttontwo.transform.position) <= 3f || PoliceAndThief.cellbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbutton.transform.position) <= 3f) && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.policeCanKillNearPrison) {
                        CanUse = false;
                    }
                    return CanUse && PoliceAndThief.policeplayer04currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer04IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { policeplayer04KillButton.Timer = policeplayer04KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Policeplayer04 Jail
            policeplayer04JailButton = new CustomButton(
                () => {
                    if (PoliceAndThief.policeplayer04currentTarget != null) {
                        PoliceAndThief.policeplayer04targetedPlayer = PoliceAndThief.policeplayer04currentTarget;
                        policeplayer04JailButton.HasEffect = true;
                    }
                },
                () => { return PoliceAndThief.policeplayer04 != null && PoliceAndThief.policeplayer04 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (policeplayer04JailButton.isEffectActive && PoliceAndThief.policeplayer04targetedPlayer != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.policeplayer04targetedPlayer.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        PoliceAndThief.policeplayer04targetedPlayer = null;
                        policeplayer04JailButton.Timer = 0f;
                        policeplayer04JailButton.isEffectActive = false;
                    }

                    return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer04IsReviving && PoliceAndThief.policeplayer04currentTarget != null;
                },
                () => {
                    PoliceAndThief.policeplayer04targetedPlayer = null;
                    policeplayer04JailButton.isEffectActive = false;
                    policeplayer04JailButton.Timer = policeplayer04JailButton.MaxTimer;
                    policeplayer04JailButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                PoliceAndThief.getCaptureThiefButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                PoliceAndThief.captureThiefTime,
                () => {
                    if (PoliceAndThief.policeplayer04targetedPlayer != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefJail, Hazel.SendOption.Reliable, -1);
                        writer.Write(PoliceAndThief.policeplayer04targetedPlayer.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.policeandThiefJail(PoliceAndThief.policeplayer04targetedPlayer.PlayerId);
                        PoliceAndThief.policeplayer04targetedPlayer = null;
                        policeplayer04JailButton.Timer = policeplayer04JailButton.MaxTimer;
                    }
                }
            );

            // Policeplayer04 Light
            policeplayer04LightButton = new CustomButton(
                () => {
                    PoliceAndThief.policeplayer04lightTimer = 10;
                },
                () => { return PoliceAndThief.policeplayer04 != null && PoliceAndThief.policeplayer04 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer04IsReviving; },
                () => {
                    policeplayer04LightButton.Timer = policeplayer04LightButton.MaxTimer;
                    policeplayer04LightButton.isEffectActive = false;
                    policeplayer04LightButton.actionButton.graphic.color = Palette.EnabledColor;
                },
                PoliceAndThief.getLightButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.F,
                true,
                10,
                () => { policeplayer04LightButton.Timer = policeplayer04LightButton.MaxTimer; }
            );

            // Policeplayer05 Kill
            policeplayer05KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.policeplayer05currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(5);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 5);
                    policeplayer05KillButton.Timer = policeplayer05KillButton.MaxTimer;
                    PoliceAndThief.policeplayer05currentTarget = null;
                },
                () => { return PoliceAndThief.policeplayer05 != null && PoliceAndThief.policeplayer05 == PlayerControl.LocalPlayer; },
                () => {
                    bool CanUse = true;
                    if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbuttontwo.transform.position) <= 3f || PoliceAndThief.cellbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.cellbutton.transform.position) <= 3f) && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.policeCanKillNearPrison) {
                        CanUse = false;
                    }
                    return CanUse && PoliceAndThief.policeplayer05currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer05IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { policeplayer05KillButton.Timer = policeplayer05KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Policeplayer05 Jail
            policeplayer05JailButton = new CustomButton(
                () => {
                    if (PoliceAndThief.policeplayer05currentTarget != null) {
                        PoliceAndThief.policeplayer05targetedPlayer = PoliceAndThief.policeplayer05currentTarget;
                        policeplayer05JailButton.HasEffect = true;
                    }
                },
                () => { return PoliceAndThief.policeplayer05 != null && PoliceAndThief.policeplayer05 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (policeplayer05JailButton.isEffectActive && PoliceAndThief.policeplayer05targetedPlayer != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.policeplayer05targetedPlayer.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        PoliceAndThief.policeplayer05targetedPlayer = null;
                        policeplayer05JailButton.Timer = 0f;
                        policeplayer05JailButton.isEffectActive = false;
                    }

                    return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer05IsReviving && PoliceAndThief.policeplayer05currentTarget != null;
                },
                () => {
                    PoliceAndThief.policeplayer05targetedPlayer = null;
                    policeplayer05JailButton.isEffectActive = false;
                    policeplayer05JailButton.Timer = policeplayer05JailButton.MaxTimer;
                    policeplayer05JailButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                PoliceAndThief.getCaptureThiefButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                PoliceAndThief.captureThiefTime,
                () => {
                    if (PoliceAndThief.policeplayer05targetedPlayer != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefJail, Hazel.SendOption.Reliable, -1);
                        writer.Write(PoliceAndThief.policeplayer05targetedPlayer.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.policeandThiefJail(PoliceAndThief.policeplayer05targetedPlayer.PlayerId);
                        PoliceAndThief.policeplayer05targetedPlayer = null;
                        policeplayer05JailButton.Timer = policeplayer05JailButton.MaxTimer;
                    }
                }
            );

            // Policeplayer05 Light
            policeplayer05LightButton = new CustomButton(
                () => {
                    PoliceAndThief.policeplayer05lightTimer = 10;
                },
                () => { return PoliceAndThief.policeplayer05 != null && PoliceAndThief.policeplayer05 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.policeplayer05IsReviving; },
                () => {
                    policeplayer05LightButton.Timer = policeplayer05LightButton.MaxTimer;
                    policeplayer05LightButton.isEffectActive = false;
                    policeplayer05LightButton.actionButton.graphic.color = Palette.EnabledColor;
                },
                PoliceAndThief.getLightButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.F,
                true,
                10,
                () => { policeplayer05LightButton.Timer = policeplayer05LightButton.MaxTimer; }
            );

            // Thiefplayer01 Kill
            thiefplayer01KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer01currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(7);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 7);
                    thiefplayer01KillButton.Timer = thiefplayer01KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer01currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer01 != null && PoliceAndThief.thiefplayer01 == PlayerControl.LocalPlayer && !PoliceAndThief.thiefplayer01IsReviving && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer01currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer01IsStealing; },
                () => { thiefplayer01KillButton.Timer = thiefplayer01KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer01 FreeThief Button
            thiefplayer01FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer01FreeThiefButton.Timer = thiefplayer01FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer01 != null && PoliceAndThief.thiefplayer01 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer01.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer01.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer01.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer01IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer01FreeThiefButton.Timer = thiefplayer01FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer01 Take/Deliver Jewel Button
            thiefplayer01TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer01IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer01JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer01JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer01TakeDeliverJewelButton.Timer = thiefplayer01TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer01 != null && PoliceAndThief.thiefplayer01 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer01IsStealing)
                        thiefplayer01TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer01TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer01IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer01JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer01JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer01JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer01JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer01JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer01JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer01JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer01JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer01JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer01JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer01JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer01JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer01JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer01JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer01JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer01IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer01IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer01TakeDeliverJewelButton.Timer = thiefplayer01TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer02 Kill
            thiefplayer02KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer02currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(8);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 8);
                    thiefplayer02KillButton.Timer = thiefplayer02KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer02currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer02 != null && PoliceAndThief.thiefplayer02 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer02currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer02IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer02IsStealing; },
                () => { thiefplayer02KillButton.Timer = thiefplayer02KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer02 FreeThief Button
            thiefplayer02FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer02FreeThiefButton.Timer = thiefplayer02FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer02 != null && PoliceAndThief.thiefplayer02 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }

                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer02.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer02.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer02.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer02IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer02FreeThiefButton.Timer = thiefplayer02FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer02 Take/Deliver Jewel Button
            thiefplayer02TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer02IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer02JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer02JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer02TakeDeliverJewelButton.Timer = thiefplayer02TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer02 != null && PoliceAndThief.thiefplayer02 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer02IsStealing)
                        thiefplayer02TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer02TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer02IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer02JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer02JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer02JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer02JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer02JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer02JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer02JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer02JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer02JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer02JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer02JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer02JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer02JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer02JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer02JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer02IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer02IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer02TakeDeliverJewelButton.Timer = thiefplayer02TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer03 Kill
            thiefplayer03KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer03currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(9);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 9);
                    thiefplayer03KillButton.Timer = thiefplayer03KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer03currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer03 != null && PoliceAndThief.thiefplayer03 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer03currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer03IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer03IsStealing; },
                () => { thiefplayer03KillButton.Timer = thiefplayer03KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer03 FreeThief Button
            thiefplayer03FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer03FreeThiefButton.Timer = thiefplayer03FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer03 != null && PoliceAndThief.thiefplayer03 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer03.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer03.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer03.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer03IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer03FreeThiefButton.Timer = thiefplayer03FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer03 Take/Deliver Jewel Button
            thiefplayer03TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer03IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer03JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer03JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer03TakeDeliverJewelButton.Timer = thiefplayer03TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer03 != null && PoliceAndThief.thiefplayer03 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer03IsStealing)
                        thiefplayer03TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer03TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer03IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer03JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer03JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer03JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer03JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer03JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer03JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer03JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer03JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer03JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer03JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer03JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer03JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer03JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer03JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer03JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer03IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer03IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer03TakeDeliverJewelButton.Timer = thiefplayer03TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer04 Kill
            thiefplayer04KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer04currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(10);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 10);
                    thiefplayer04KillButton.Timer = thiefplayer04KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer04currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer04 != null && PoliceAndThief.thiefplayer04 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer04currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer04IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer04IsStealing; },
                () => { thiefplayer04KillButton.Timer = thiefplayer04KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer04 FreeThief Button
            thiefplayer04FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer04FreeThiefButton.Timer = thiefplayer04FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer04 != null && PoliceAndThief.thiefplayer04 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer04.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer04.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer04.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer04IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer04FreeThiefButton.Timer = thiefplayer04FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer04 Take/Deliver Jewel Button
            thiefplayer04TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer04IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer04JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer04JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer04TakeDeliverJewelButton.Timer = thiefplayer04TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer04 != null && PoliceAndThief.thiefplayer04 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer04IsStealing)
                        thiefplayer04TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer04TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer04IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer04JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer04JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer04JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer04JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer04JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer04JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer04JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer04JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer04JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer04JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer04JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer04JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer04JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer04JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer04JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer04IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer04IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer04TakeDeliverJewelButton.Timer = thiefplayer04TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer05 Kill
            thiefplayer05KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer05currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(11);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 11);
                    thiefplayer05KillButton.Timer = thiefplayer05KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer05currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer05 != null && PoliceAndThief.thiefplayer05 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer05currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer05IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer05IsStealing; },
                () => { thiefplayer05KillButton.Timer = thiefplayer05KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer05 FreeThief Button
            thiefplayer05FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer05FreeThiefButton.Timer = thiefplayer05FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer05 != null && PoliceAndThief.thiefplayer05 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer05.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer05.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer05.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer05IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer05FreeThiefButton.Timer = thiefplayer05FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer05 Take/Deliver Jewel Button
            thiefplayer05TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer05IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer05JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer05JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer05TakeDeliverJewelButton.Timer = thiefplayer05TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer05 != null && PoliceAndThief.thiefplayer05 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer05IsStealing)
                        thiefplayer05TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer05TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer05IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer05JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer05JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer05JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer05JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer05JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer05JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer05JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer05JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer05JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer05JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer05JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer05JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer05JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer05JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer05JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer05IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer05IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer05TakeDeliverJewelButton.Timer = thiefplayer05TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer06 Kill
            thiefplayer06KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer06currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(12);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 12);
                    thiefplayer06KillButton.Timer = thiefplayer06KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer06currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer06 != null && PoliceAndThief.thiefplayer06 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer06currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer06IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer06IsStealing; },
                () => { thiefplayer06KillButton.Timer = thiefplayer06KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer06 FreeThief Button
            thiefplayer06FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer06FreeThiefButton.Timer = thiefplayer06FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer06 != null && PoliceAndThief.thiefplayer06 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer06.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer06.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer06.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer06IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer06FreeThiefButton.Timer = thiefplayer06FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer06 Take/Deliver Jewel Button
            thiefplayer06TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer06IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer06JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer06JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer06TakeDeliverJewelButton.Timer = thiefplayer06TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer06 != null && PoliceAndThief.thiefplayer06 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer06IsStealing)
                        thiefplayer06TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer06TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer06IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer06JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer06JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer06JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer06JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer06JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer06JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer06JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer06JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer06JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer06JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer06JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer06JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer06JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer06JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer06JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer06IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer06IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer06TakeDeliverJewelButton.Timer = thiefplayer06TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer07 Kill
            thiefplayer07KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer07currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(13);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 13);
                    thiefplayer07KillButton.Timer = thiefplayer07KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer07currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer07 != null && PoliceAndThief.thiefplayer07 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer07currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer07IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer07IsStealing; },
                () => { thiefplayer07KillButton.Timer = thiefplayer07KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer07 FreeThief Button
            thiefplayer07FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer07FreeThiefButton.Timer = thiefplayer07FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer07 != null && PoliceAndThief.thiefplayer07 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer07.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer07.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer07.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer07IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer07FreeThiefButton.Timer = thiefplayer07FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer07 Take/Deliver Jewel Button
            thiefplayer07TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer07IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer07JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer07JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer07TakeDeliverJewelButton.Timer = thiefplayer07TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer07 != null && PoliceAndThief.thiefplayer07 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer07IsStealing)
                        thiefplayer07TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer07TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer07IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer07JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer07JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer07JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer07JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer07JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer07JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer07JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer07JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer07JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer07JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer07JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer07JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer07JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer07JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer07JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer07IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer07IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer07TakeDeliverJewelButton.Timer = thiefplayer07TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer08 Kill
            thiefplayer08KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer08currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(14);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 14);
                    thiefplayer08KillButton.Timer = thiefplayer08KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer08currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer08 != null && PoliceAndThief.thiefplayer08 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer08currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer08IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer08IsStealing; },
                () => { thiefplayer08KillButton.Timer = thiefplayer08KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer08 FreeThief Button
            thiefplayer08FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer08FreeThiefButton.Timer = thiefplayer08FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer08 != null && PoliceAndThief.thiefplayer08 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer08.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer08.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer08.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer08IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer08FreeThiefButton.Timer = thiefplayer08FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer08 Take/Deliver Jewel Button
            thiefplayer08TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer08IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer08JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer08JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer08TakeDeliverJewelButton.Timer = thiefplayer08TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer08 != null && PoliceAndThief.thiefplayer08 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer08IsStealing)
                        thiefplayer08TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer08TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer08IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer08JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer08JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer08JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer08JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer08JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer08JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer08JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer08JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer08JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer08JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer08JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer08JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer08JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer08JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer08JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer08IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer08IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer08TakeDeliverJewelButton.Timer = thiefplayer08TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer09 Kill
            thiefplayer09KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer09currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(15);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 15);
                    thiefplayer09KillButton.Timer = thiefplayer09KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer09currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer09 != null && PoliceAndThief.thiefplayer09 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer09currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer09IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer09IsStealing; },
                () => { thiefplayer09KillButton.Timer = thiefplayer09KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer09 FreeThief Button
            thiefplayer09FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer09FreeThiefButton.Timer = thiefplayer09FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer09 != null && PoliceAndThief.thiefplayer09 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer09.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer09.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer09.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer09IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer09FreeThiefButton.Timer = thiefplayer09FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer09 Take/Deliver Jewel Button
            thiefplayer09TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer09IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer09JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer09JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer09TakeDeliverJewelButton.Timer = thiefplayer09TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer09 != null && PoliceAndThief.thiefplayer09 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer09IsStealing)
                        thiefplayer09TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer09TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer09IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer09JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer09JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer09JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer09JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer09JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer09JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer09JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer09JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer09JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer09JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer09JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer09JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer09JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer09JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer09JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer09IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer09IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer09TakeDeliverJewelButton.Timer = thiefplayer09TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer10 Kill
            thiefplayer10KillButton = new CustomButton(
                () => {
                    byte targetId = PoliceAndThief.thiefplayer10currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(16);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.policeandThiefKills(targetId, 16);
                    thiefplayer10KillButton.Timer = thiefplayer10KillButton.MaxTimer;
                    PoliceAndThief.thiefplayer10currentTarget = null;
                },
                () => { return PoliceAndThief.thiefplayer10 != null && PoliceAndThief.thiefplayer10 == PlayerControl.LocalPlayer && PoliceAndThief.thiefTeamCanKill; },
                () => { return PoliceAndThief.thiefplayer10currentTarget && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer10IsReviving && !PlayerControl.LocalPlayer.Data.IsDead && !PoliceAndThief.thiefplayer10IsStealing; },
                () => { thiefplayer10KillButton.Timer = thiefplayer10KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Thiefplayer10 FreeThief Button
            thiefplayer10FreeThiefButton = new CustomButton(
                () => {
                    MessageWriter thiefFree = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefFreeThief, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(thiefFree);
                    RPCProcedure.policeandThiefFreeThief();
                    thiefplayer10FreeThiefButton.Timer = thiefplayer10FreeThiefButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer10 != null && PoliceAndThief.thiefplayer10 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.localThiefReleaseArrow.Count != 0) {
                        PoliceAndThief.localThiefReleaseArrow[0].Update(PoliceAndThief.cellbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefReleaseArrow[1].Update(PoliceAndThief.cellbuttontwo.transform.position);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count != 0) {
                        PoliceAndThief.localThiefDeliverArrow[0].Update(PoliceAndThief.jewelbutton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            PoliceAndThief.localThiefDeliverArrow[1].Update(PoliceAndThief.jewelbuttontwo.transform.position);
                        }
                    }
                    
                    bool CanUse = false;
                    if (PoliceAndThief.currentThiefsCaptured > 0) {
                        if ((PoliceAndThief.cellbuttontwo != null && Vector2.Distance(PoliceAndThief.thiefplayer10.transform.position, PoliceAndThief.cellbuttontwo.transform.position) < 0.4f || Vector2.Distance(PoliceAndThief.thiefplayer10.transform.position, PoliceAndThief.cellbutton.transform.position) < 0.4f) && !PoliceAndThief.thiefplayer10.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer10IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer10FreeThiefButton.Timer = thiefplayer10FreeThiefButton.MaxTimer; },
                PoliceAndThief.getFreeThiefButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Thiefplayer10 Take/Deliver Jewel Button
            thiefplayer10TakeDeliverJewelButton = new CustomButton(
                () => {
                    if (PoliceAndThief.thiefplayer10IsStealing) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer10JewelId;
                        MessageWriter thiefScore = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefDeliverJewel, Hazel.SendOption.Reliable, -1);
                        thiefScore.Write(targetId);
                        thiefScore.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefScore);
                        RPCProcedure.policeandThiefDeliverJewel(targetId, jewelId);
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte jewelId = PoliceAndThief.thiefplayer10JewelId;
                        MessageWriter thiefWhoTookATreasure = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PoliceandThiefTakeJewel, Hazel.SendOption.Reliable, -1);
                        thiefWhoTookATreasure.Write(targetId);
                        thiefWhoTookATreasure.Write(jewelId);
                        AmongUsClient.Instance.FinishRpcImmediately(thiefWhoTookATreasure);
                        RPCProcedure.policeandThiefTakeJewel(targetId, jewelId);
                    }
                    thiefplayer10TakeDeliverJewelButton.Timer = thiefplayer10TakeDeliverJewelButton.MaxTimer;
                },
                () => { return PoliceAndThief.thiefplayer10 != null && PoliceAndThief.thiefplayer10 == PlayerControl.LocalPlayer; },
                () => {
                    if (PoliceAndThief.thiefplayer10IsStealing)
                        thiefplayer10TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getDeliverJewelButtonSprite();
                    else
                        thiefplayer10TakeDeliverJewelButton.actionButton.graphic.sprite = PoliceAndThief.getTakeJewelButtonSprite();
                    bool CanUse = false;
                    if (PoliceAndThief.thiefTreasures.Count != 0) {
                        foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                            if (jewel != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, jewel.transform.position) < 0.5f && !PoliceAndThief.thiefplayer10IsStealing) {
                                switch (jewel.name) {
                                    case "jewel01":
                                        PoliceAndThief.thiefplayer10JewelId = 1;
                                        CanUse = !PoliceAndThief.jewel01BeingStealed;
                                        break;
                                    case "jewel02":
                                        PoliceAndThief.thiefplayer10JewelId = 2;
                                        CanUse = !PoliceAndThief.jewel02BeingStealed;
                                        break;
                                    case "jewel03":
                                        PoliceAndThief.thiefplayer10JewelId = 3;
                                        CanUse = !PoliceAndThief.jewel03BeingStealed;
                                        break;
                                    case "jewel04":
                                        PoliceAndThief.thiefplayer10JewelId = 4;
                                        CanUse = !PoliceAndThief.jewel04BeingStealed;
                                        break;
                                    case "jewel05":
                                        PoliceAndThief.thiefplayer10JewelId = 5;
                                        CanUse = !PoliceAndThief.jewel05BeingStealed;
                                        break;
                                    case "jewel06":
                                        PoliceAndThief.thiefplayer10JewelId = 6;
                                        CanUse = !PoliceAndThief.jewel06BeingStealed;
                                        break;
                                    case "jewel07":
                                        PoliceAndThief.thiefplayer10JewelId = 7;
                                        CanUse = !PoliceAndThief.jewel07BeingStealed;
                                        break;
                                    case "jewel08":
                                        PoliceAndThief.thiefplayer10JewelId = 8;
                                        CanUse = !PoliceAndThief.jewel08BeingStealed;
                                        break;
                                    case "jewel09":
                                        PoliceAndThief.thiefplayer10JewelId = 9;
                                        CanUse = !PoliceAndThief.jewel09BeingStealed;
                                        break;
                                    case "jewel10":
                                        PoliceAndThief.thiefplayer10JewelId = 10;
                                        CanUse = !PoliceAndThief.jewel10BeingStealed;
                                        break;
                                    case "jewel11":
                                        PoliceAndThief.thiefplayer10JewelId = 11;
                                        CanUse = !PoliceAndThief.jewel11BeingStealed;
                                        break;
                                    case "jewel12":
                                        PoliceAndThief.thiefplayer10JewelId = 12;
                                        CanUse = !PoliceAndThief.jewel12BeingStealed;
                                        break;
                                    case "jewel13":
                                        PoliceAndThief.thiefplayer10JewelId = 13;
                                        CanUse = !PoliceAndThief.jewel13BeingStealed;
                                        break;
                                    case "jewel14":
                                        PoliceAndThief.thiefplayer10JewelId = 14;
                                        CanUse = !PoliceAndThief.jewel14BeingStealed;
                                        break;
                                    case "jewel15":
                                        PoliceAndThief.thiefplayer10JewelId = 15;
                                        CanUse = !PoliceAndThief.jewel15BeingStealed;
                                        break;
                                }
                            }
                            else if ((PoliceAndThief.jewelbuttontwo != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbuttontwo.transform.position) < 0.5f || PoliceAndThief.jewelbutton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, PoliceAndThief.jewelbutton.transform.position) < 0.5f) && PoliceAndThief.thiefplayer10IsStealing) {
                                CanUse = true;
                            }
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !PoliceAndThief.thiefplayer10IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { thiefplayer10TakeDeliverJewelButton.Timer = thiefplayer10TakeDeliverJewelButton.MaxTimer; },
                PoliceAndThief.getTakeJewelButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // King of the hill buttons
            // greenplayer01 Kill
            greenplayer01KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.greenplayer01currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(1);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 1);
                    greenplayer01KillButton.Timer = greenplayer01KillButton.MaxTimer;
                    KingOfTheHill.greenplayer01currentTarget = null;
                },
                () => { return KingOfTheHill.greenplayer01 != null && KingOfTheHill.greenplayer01 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.greenKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.greenplayer01currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.greenplayer01IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { greenplayer01KillButton.Timer = greenplayer01KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // greenplayer02 Kill
            greenplayer02KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.greenplayer02currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(2);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 2);
                    greenplayer02KillButton.Timer = greenplayer02KillButton.MaxTimer;
                    KingOfTheHill.greenplayer02currentTarget = null;
                },
                () => { return KingOfTheHill.greenplayer02 != null && KingOfTheHill.greenplayer02 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.greenKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.greenplayer02currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.greenplayer02IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { greenplayer02KillButton.Timer = greenplayer02KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // greenplayer03 Kill
            greenplayer03KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.greenplayer03currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(3);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 3);
                    greenplayer03KillButton.Timer = greenplayer03KillButton.MaxTimer;
                    KingOfTheHill.greenplayer03currentTarget = null;
                },
                () => { return KingOfTheHill.greenplayer03 != null && KingOfTheHill.greenplayer03 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.greenKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.greenplayer03currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.greenplayer03IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { greenplayer03KillButton.Timer = greenplayer03KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // greenplayer04 Kill
            greenplayer04KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.greenplayer04currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(4);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 4);
                    greenplayer04KillButton.Timer = greenplayer04KillButton.MaxTimer;
                    KingOfTheHill.greenplayer04currentTarget = null;
                },
                () => { return KingOfTheHill.greenplayer04 != null && KingOfTheHill.greenplayer04 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.greenKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.greenplayer04currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.greenplayer04IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { greenplayer04KillButton.Timer = greenplayer04KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // greenplayer05 Kill
            greenplayer05KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.greenplayer05currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(5);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 5);
                    greenplayer05KillButton.Timer = greenplayer05KillButton.MaxTimer;
                    KingOfTheHill.greenplayer05currentTarget = null;
                },
                () => { return KingOfTheHill.greenplayer05 != null && KingOfTheHill.greenplayer05 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.greenKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.greenplayer05currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.greenplayer05IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { greenplayer05KillButton.Timer = greenplayer05KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // greenplayer06 Kill
            greenplayer06KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.greenplayer06currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(6);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 6);
                    greenplayer06KillButton.Timer = greenplayer06KillButton.MaxTimer;
                    KingOfTheHill.greenplayer06currentTarget = null;
                },
                () => { return KingOfTheHill.greenplayer06 != null && KingOfTheHill.greenplayer06 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.greenKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.greenplayer06currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.greenplayer06IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { greenplayer06KillButton.Timer = greenplayer06KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // greenKingplayer Kill
            greenKingplayerKillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.greenKingplayercurrentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(7);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 7);
                    greenKingplayerKillButton.Timer = greenKingplayerKillButton.MaxTimer;
                    KingOfTheHill.greenKingplayercurrentTarget = null;
                },
                () => { return KingOfTheHill.greenKingplayer != null && KingOfTheHill.greenKingplayer == PlayerControl.LocalPlayer && KingOfTheHill.kingCanKill; },
                () => { return KingOfTheHill.greenKingplayercurrentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.greenKingIsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { greenKingplayerKillButton.Timer = greenKingplayerKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // yellowplayer01 Kill
            yellowplayer01KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.yellowplayer01currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(9);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 9);
                    yellowplayer01KillButton.Timer = yellowplayer01KillButton.MaxTimer;
                    KingOfTheHill.yellowplayer01currentTarget = null;
                },
                () => { return KingOfTheHill.yellowplayer01 != null && KingOfTheHill.yellowplayer01 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.yellowKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.yellowplayer01currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.yellowplayer01IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { yellowplayer01KillButton.Timer = yellowplayer01KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );


            // yellowplayer02 Kill
            yellowplayer02KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.yellowplayer02currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(10);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 10);
                    yellowplayer02KillButton.Timer = yellowplayer02KillButton.MaxTimer;
                    KingOfTheHill.yellowplayer02currentTarget = null;
                },
                () => { return KingOfTheHill.yellowplayer02 != null && KingOfTheHill.yellowplayer02 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.yellowKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.yellowplayer02currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.yellowplayer02IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { yellowplayer02KillButton.Timer = yellowplayer02KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // yellowplayer03 Kill
            yellowplayer03KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.yellowplayer03currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(11);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 11);
                    yellowplayer03KillButton.Timer = yellowplayer03KillButton.MaxTimer;
                    KingOfTheHill.yellowplayer03currentTarget = null;
                },
                () => { return KingOfTheHill.yellowplayer03 != null && KingOfTheHill.yellowplayer03 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.yellowKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.yellowplayer03currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.yellowplayer03IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { yellowplayer03KillButton.Timer = yellowplayer03KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // yellowplayer04 Kill
            yellowplayer04KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.yellowplayer04currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(12);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 12);
                    yellowplayer04KillButton.Timer = yellowplayer04KillButton.MaxTimer;
                    KingOfTheHill.yellowplayer04currentTarget = null;
                },
                () => { return KingOfTheHill.yellowplayer04 != null && KingOfTheHill.yellowplayer04 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.yellowKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.yellowplayer04currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.yellowplayer04IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { yellowplayer04KillButton.Timer = yellowplayer04KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // yellowplayer05 Kill
            yellowplayer05KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.yellowplayer05currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(13);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 13);
                    yellowplayer05KillButton.Timer = yellowplayer05KillButton.MaxTimer;
                    KingOfTheHill.yellowplayer05currentTarget = null;
                },
                () => { return KingOfTheHill.yellowplayer05 != null && KingOfTheHill.yellowplayer05 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.yellowKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.yellowplayer05currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.yellowplayer05IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { yellowplayer05KillButton.Timer = yellowplayer05KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // yellowplayer06 Kill
            yellowplayer06KillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.yellowplayer06currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(14);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 14);
                    yellowplayer06KillButton.Timer = yellowplayer06KillButton.MaxTimer;
                    KingOfTheHill.yellowplayer06currentTarget = null;
                },
                () => { return KingOfTheHill.yellowplayer06 != null && KingOfTheHill.yellowplayer06 == PlayerControl.LocalPlayer && PlayerControl.LocalPlayer != KingOfTheHill.yellowKingplayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.yellowplayer06currentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.yellowplayer06IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { yellowplayer06KillButton.Timer = yellowplayer06KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // UsurperPlayer Kill
            usurperPlayerKillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.usurperPlayercurrentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(15);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 15);
                    usurperPlayerKillButton.Timer = usurperPlayerKillButton.MaxTimer;
                    KingOfTheHill.usurperPlayercurrentTarget = null;
                },
                () => { return KingOfTheHill.usurperPlayer != null && KingOfTheHill.usurperPlayer == PlayerControl.LocalPlayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    return KingOfTheHill.usurperPlayercurrentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.usurperPlayerIsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { usurperPlayerKillButton.Timer = usurperPlayerKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // yellowKingplayer Kill
            yellowKingplayerKillButton = new CustomButton(
                () => {
                    byte targetId = KingOfTheHill.yellowKingplayercurrentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(16);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.kingOfTheHillKills(targetId, 16);
                    yellowKingplayerKillButton.Timer = yellowKingplayerKillButton.MaxTimer;
                    KingOfTheHill.yellowKingplayercurrentTarget = null;
                },
                () => { return KingOfTheHill.yellowKingplayer != null && KingOfTheHill.yellowKingplayer == PlayerControl.LocalPlayer && KingOfTheHill.kingCanKill; },
                () => { return KingOfTheHill.yellowKingplayercurrentTarget && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.yellowKingIsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { yellowKingplayerKillButton.Timer = yellowKingplayerKillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // greenKingplayer Capture
            greenKingplayerCaptureZoneButton = new CustomButton(
                () => {

                    if (KingOfTheHill.whichGreenKingplayerzone != 0) {
                        greenKingplayerCaptureZoneButton.HasEffect = true;
                    }

                },
                () => { return KingOfTheHill.greenKingplayer != null && KingOfTheHill.greenKingplayer == PlayerControl.LocalPlayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    bool CanUse = false;
                    KingOfTheHill.whichGreenKingplayerzone = 0;
                    if (KingOfTheHill.kingZones.Count != 0) {
                        foreach (GameObject zone in KingOfTheHill.kingZones) {
                            if (zone != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, zone.transform.position) < 0.5f) {
                                switch (zone.name) {
                                    case "zoneone":
                                        KingOfTheHill.whichGreenKingplayerzone = 1;
                                        CanUse = !KingOfTheHill.greenKinghaszoneone;
                                        break;
                                    case "zonetwo":
                                        KingOfTheHill.whichGreenKingplayerzone = 2;
                                        CanUse = !KingOfTheHill.greenKinghaszonetwo;
                                        break;
                                    case "zonethree":
                                        KingOfTheHill.whichGreenKingplayerzone = 3;
                                        CanUse = !KingOfTheHill.greenKinghaszonethree;
                                        break;
                                }
                            }
                        }
                        if (greenKingplayerCaptureZoneButton.isEffectActive && (KingOfTheHill.whichGreenKingplayerzone == 0 || PlayerControl.LocalPlayer.Data.IsDead)) {
                            greenKingplayerCaptureZoneButton.Timer = 0f;
                            greenKingplayerCaptureZoneButton.isEffectActive = false;
                        }

                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.greenKingIsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { greenKingplayerCaptureZoneButton.Timer = greenKingplayerCaptureZoneButton.MaxTimer; },
                KingOfTheHill.getPlaceGreenFlagButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                3,
                () => {
                    if (KingOfTheHill.whichGreenKingplayerzone != 0) {
                        byte zoneId = KingOfTheHill.whichGreenKingplayerzone;
                        MessageWriter greenKingCaptured = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillCapture, Hazel.SendOption.Reliable, -1);
                        greenKingCaptured.Write(zoneId);
                        greenKingCaptured.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(greenKingCaptured);
                        RPCProcedure.kingoftheHillCapture(zoneId, 1);

                        greenKingplayerCaptureZoneButton.Timer = greenKingplayerCaptureZoneButton.MaxTimer;
                    }
                }
            );

            // yellowKingplayer Capture
            yellowKingplayerCaptureZoneButton = new CustomButton(
                () => {

                    if (KingOfTheHill.whichYellowKingplayerzone != 0) {
                        yellowKingplayerCaptureZoneButton.HasEffect = true;
                    }

                },
                () => { return KingOfTheHill.yellowKingplayer != null && KingOfTheHill.yellowKingplayer == PlayerControl.LocalPlayer; },
                () => {
                    if (KingOfTheHill.localArrows.Count != 0) {
                        KingOfTheHill.localArrows[0].Update(KingOfTheHill.zoneone.transform.position, KingOfTheHill.zoneonecolor);
                        KingOfTheHill.localArrows[1].Update(KingOfTheHill.zonetwo.transform.position, KingOfTheHill.zonetwocolor);
                        KingOfTheHill.localArrows[2].Update(KingOfTheHill.zonethree.transform.position, KingOfTheHill.zonethreecolor);
                    }
                    bool CanUse = false;
                    KingOfTheHill.whichYellowKingplayerzone = 0;
                    if (KingOfTheHill.kingZones.Count != 0) {
                        foreach (GameObject zone in KingOfTheHill.kingZones) {
                            if (zone != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, zone.transform.position) < 0.5f) {
                                switch (zone.name) {
                                    case "zoneone":
                                        KingOfTheHill.whichYellowKingplayerzone = 1;
                                        CanUse = !KingOfTheHill.yellowKinghaszoneone;
                                        break;
                                    case "zonetwo":
                                        KingOfTheHill.whichYellowKingplayerzone = 2;
                                        CanUse = !KingOfTheHill.yellowKinghaszonetwo;
                                        break;
                                    case "zonethree":
                                        KingOfTheHill.whichYellowKingplayerzone = 3;
                                        CanUse = !KingOfTheHill.yellowKinghaszonethree;
                                        break;
                                }
                            }
                        }
                        if (yellowKingplayerCaptureZoneButton.isEffectActive && (KingOfTheHill.whichYellowKingplayerzone == 0 || PlayerControl.LocalPlayer.Data.IsDead)) {
                            yellowKingplayerCaptureZoneButton.Timer = 0f;
                            yellowKingplayerCaptureZoneButton.isEffectActive = false;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !KingOfTheHill.yellowKingIsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { yellowKingplayerCaptureZoneButton.Timer = yellowKingplayerCaptureZoneButton.MaxTimer; },
                KingOfTheHill.getPlaceYellowFlagButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                true,
                3,
                () => {
                    if (KingOfTheHill.whichYellowKingplayerzone != 0) {
                        byte zoneId = KingOfTheHill.whichYellowKingplayerzone;
                        MessageWriter yellowKingCaptured = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.KingoftheHillCapture, Hazel.SendOption.Reliable, -1);
                        yellowKingCaptured.Write(zoneId);
                        yellowKingCaptured.Write(2);
                        AmongUsClient.Instance.FinishRpcImmediately(yellowKingCaptured);
                        RPCProcedure.kingoftheHillCapture(zoneId, 2);

                        yellowKingplayerCaptureZoneButton.Timer = yellowKingplayerCaptureZoneButton.MaxTimer;
                    }
                }
            );

            // Hot Potato buttons code
            // Hot Potato transfer
            hotPotatoButton = new CustomButton(
                () => {

                    hotPotatoButton.Timer = hotPotatoButton.MaxTimer;

                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.HotPotatoTransfer, Hazel.SendOption.Reliable, -1);
                    writer.Write(HotPotato.hotPotatoPlayerCurrentTarget.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);

                    RPCProcedure.hotPotatoTransfer(HotPotato.hotPotatoPlayerCurrentTarget.PlayerId);
                },
                () => { return HotPotato.hotPotatoPlayer != null && HotPotato.hotPotatoPlayer == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => { return HotPotato.hotPotatoPlayerCurrentTarget && PlayerControl.LocalPlayer.CanMove;
                },
                () => { hotPotatoButton.Timer = hotPotatoButton.MaxTimer; },
                HotPotato.getButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // ZombieLaboratory buttons code
            // Zombie01 infect
            zombie01InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer01infectedTarget = ZombieLaboratory.zombiePlayer01currentTarget;
                    zombie01InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer01 != null && ZombieLaboratory.zombiePlayer01 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie01InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer01infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer01infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer01infectedTarget = null;
                        zombie01InfectButton.Timer = 0f;
                        zombie01InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer01currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer01currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer01IsReviving && ZombieLaboratory.zombiePlayer01currentTarget != null;
                },
                () => { zombie01InfectButton.Timer = zombie01InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer01infectedTarget != null && !ZombieLaboratory.zombiePlayer01infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer01infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer01infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer01infectedTarget = null;
                    }
                    zombie01InfectButton.Timer = zombie01InfectButton.MaxTimer;
                }
            );

            // Zombie02 infect
            zombie02InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer02infectedTarget = ZombieLaboratory.zombiePlayer02currentTarget;
                    zombie02InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer02 != null && ZombieLaboratory.zombiePlayer02 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie02InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer02infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer02infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer02infectedTarget = null;
                        zombie02InfectButton.Timer = 0f;
                        zombie02InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer02currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer02currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer02IsReviving && ZombieLaboratory.zombiePlayer02currentTarget != null;
                },
                () => { zombie02InfectButton.Timer = zombie02InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer02infectedTarget != null && !ZombieLaboratory.zombiePlayer02infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer02infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer02infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer02infectedTarget = null;
                    }
                    zombie02InfectButton.Timer = zombie02InfectButton.MaxTimer;
                }
            );

            // Zombie03 infect
            zombie03InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer03infectedTarget = ZombieLaboratory.zombiePlayer03currentTarget;
                    zombie03InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer03 != null && ZombieLaboratory.zombiePlayer03 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie03InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer03infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer03infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer03infectedTarget = null;
                        zombie03InfectButton.Timer = 0f;
                        zombie03InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer03currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer03currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer03IsReviving && ZombieLaboratory.zombiePlayer03currentTarget != null;
                },
                () => { zombie03InfectButton.Timer = zombie03InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer03infectedTarget != null && !ZombieLaboratory.zombiePlayer03infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer03infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer03infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer03infectedTarget = null;
                    }
                    zombie03InfectButton.Timer = zombie03InfectButton.MaxTimer;
                }
            );

            // Zombie04 infect
            zombie04InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer04infectedTarget = ZombieLaboratory.zombiePlayer04currentTarget;
                    zombie04InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer04 != null && ZombieLaboratory.zombiePlayer04 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie04InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer04infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer04infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer04infectedTarget = null;
                        zombie04InfectButton.Timer = 0f;
                        zombie04InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer04currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer04currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer04IsReviving && ZombieLaboratory.zombiePlayer04currentTarget != null;
                },
                () => { zombie04InfectButton.Timer = zombie04InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer04infectedTarget != null && !ZombieLaboratory.zombiePlayer04infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer04infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer04infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer04infectedTarget = null;
                    }
                    zombie04InfectButton.Timer = zombie04InfectButton.MaxTimer;
                }
            );

            // Zombie05 infect
            zombie05InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer05infectedTarget = ZombieLaboratory.zombiePlayer05currentTarget;
                    zombie05InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer05 != null && ZombieLaboratory.zombiePlayer05 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie05InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer05infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer05infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer05infectedTarget = null;
                        zombie05InfectButton.Timer = 0f;
                        zombie05InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer05currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer05currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer05IsReviving && ZombieLaboratory.zombiePlayer05currentTarget != null;
                },
                () => { zombie05InfectButton.Timer = zombie05InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer05infectedTarget != null && !ZombieLaboratory.zombiePlayer05infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer05infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer05infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer05infectedTarget = null;
                    }
                    zombie05InfectButton.Timer = zombie05InfectButton.MaxTimer;
                }
            );

            // Zombie06 infect
            zombie06InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer06infectedTarget = ZombieLaboratory.zombiePlayer06currentTarget;
                    zombie06InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer06 != null && ZombieLaboratory.zombiePlayer06 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie06InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer06infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer06infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer06infectedTarget = null;
                        zombie06InfectButton.Timer = 0f;
                        zombie06InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer06currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer06currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer06IsReviving && ZombieLaboratory.zombiePlayer06currentTarget != null;
                },
                () => { zombie06InfectButton.Timer = zombie06InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer06infectedTarget != null && !ZombieLaboratory.zombiePlayer06infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer06infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer06infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer06infectedTarget = null;
                    }
                    zombie06InfectButton.Timer = zombie06InfectButton.MaxTimer;
                }
            );

            // Zombie07 infect
            zombie07InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer07infectedTarget = ZombieLaboratory.zombiePlayer07currentTarget;
                    zombie07InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer07 != null && ZombieLaboratory.zombiePlayer07 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie07InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer07infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer07infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer07infectedTarget = null;
                        zombie07InfectButton.Timer = 0f;
                        zombie07InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer07currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer07currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer07IsReviving && ZombieLaboratory.zombiePlayer07currentTarget != null;
                },
                () => { zombie07InfectButton.Timer = zombie07InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer07infectedTarget != null && !ZombieLaboratory.zombiePlayer07infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer07infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer07infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer07infectedTarget = null;
                    }
                    zombie07InfectButton.Timer = zombie07InfectButton.MaxTimer;
                }
            );

            // Zombie08 infect
            zombie08InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer08infectedTarget = ZombieLaboratory.zombiePlayer08currentTarget;
                    zombie08InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer08 != null && ZombieLaboratory.zombiePlayer08 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie08InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer08infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer08infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer08infectedTarget = null;
                        zombie08InfectButton.Timer = 0f;
                        zombie08InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer08currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer08currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer08IsReviving && ZombieLaboratory.zombiePlayer08currentTarget != null;
                },
                () => { zombie08InfectButton.Timer = zombie08InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer08infectedTarget != null && !ZombieLaboratory.zombiePlayer08infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer08infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer08infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer08infectedTarget = null;
                    }
                    zombie08InfectButton.Timer = zombie08InfectButton.MaxTimer;
                }
            );

            // Zombie09 infect
            zombie09InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer09infectedTarget = ZombieLaboratory.zombiePlayer09currentTarget;
                    zombie09InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer09 != null && ZombieLaboratory.zombiePlayer09 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie09InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer09infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer09infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer09infectedTarget = null;
                        zombie09InfectButton.Timer = 0f;
                        zombie09InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer09currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer09currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer09IsReviving && ZombieLaboratory.zombiePlayer09currentTarget != null;
                },
                () => { zombie09InfectButton.Timer = zombie09InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer09infectedTarget != null && !ZombieLaboratory.zombiePlayer09infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer09infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer09infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer09infectedTarget = null;
                    }
                    zombie09InfectButton.Timer = zombie09InfectButton.MaxTimer;
                }
            );

            // Zombie10 infect
            zombie10InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer10infectedTarget = ZombieLaboratory.zombiePlayer10currentTarget;
                    zombie10InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer10 != null && ZombieLaboratory.zombiePlayer10 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie10InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer10infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer10infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer10infectedTarget = null;
                        zombie10InfectButton.Timer = 0f;
                        zombie10InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer10currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer10currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer10IsReviving && ZombieLaboratory.zombiePlayer10currentTarget != null;
                },
                () => { zombie10InfectButton.Timer = zombie10InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer10infectedTarget != null && !ZombieLaboratory.zombiePlayer10infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer10infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer10infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer10infectedTarget = null;
                    }
                    zombie10InfectButton.Timer = zombie10InfectButton.MaxTimer;
                }
            );

            // Zombie11 infect
            zombie11InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer11infectedTarget = ZombieLaboratory.zombiePlayer11currentTarget;
                    zombie11InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer11 != null && ZombieLaboratory.zombiePlayer11 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie11InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer11infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer11infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer11infectedTarget = null;
                        zombie11InfectButton.Timer = 0f;
                        zombie11InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer11currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer11currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer11IsReviving && ZombieLaboratory.zombiePlayer11currentTarget != null;
                },
                () => { zombie11InfectButton.Timer = zombie11InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer11infectedTarget != null && !ZombieLaboratory.zombiePlayer11infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer11infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer11infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer11infectedTarget = null;
                    }
                    zombie11InfectButton.Timer = zombie11InfectButton.MaxTimer;
                }
            );

            // Zombie12 infect
            zombie12InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer12infectedTarget = ZombieLaboratory.zombiePlayer12currentTarget;
                    zombie12InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer12 != null && ZombieLaboratory.zombiePlayer12 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie12InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer12infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer12infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer12infectedTarget = null;
                        zombie12InfectButton.Timer = 0f;
                        zombie12InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer12currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer12currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer12IsReviving && ZombieLaboratory.zombiePlayer12currentTarget != null;
                },
                () => { zombie12InfectButton.Timer = zombie12InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer12infectedTarget != null && !ZombieLaboratory.zombiePlayer12infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer12infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer12infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer12infectedTarget = null;
                    }
                    zombie12InfectButton.Timer = zombie12InfectButton.MaxTimer;
                }
            );

            // Zombie13 infect
            zombie13InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer13infectedTarget = ZombieLaboratory.zombiePlayer13currentTarget;
                    zombie13InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer13 != null && ZombieLaboratory.zombiePlayer13 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie13InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer13infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer13infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer13infectedTarget = null;
                        zombie13InfectButton.Timer = 0f;
                        zombie13InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer13currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer13currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer13IsReviving && ZombieLaboratory.zombiePlayer13currentTarget != null;
                },
                () => { zombie13InfectButton.Timer = zombie13InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer13infectedTarget != null && !ZombieLaboratory.zombiePlayer13infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer13infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer13infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer13infectedTarget = null;
                    }
                    zombie13InfectButton.Timer = zombie13InfectButton.MaxTimer;
                }
            );

            // Zombie14 infect
            zombie14InfectButton = new CustomButton(
                () => {
                    ZombieLaboratory.zombiePlayer14infectedTarget = ZombieLaboratory.zombiePlayer14currentTarget;
                    zombie14InfectButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.zombiePlayer14 != null && ZombieLaboratory.zombiePlayer14 == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead; },
                () => {
                    if (zombie14InfectButton.isEffectActive && ZombieLaboratory.zombiePlayer14infectedTarget != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.zombiePlayer14infectedTarget.transform.position) > GameOptionsData.KillDistances[Mathf.Clamp(PlayerControl.GameOptions.KillDistance, 0, 2)]) {
                        ZombieLaboratory.zombiePlayer14infectedTarget = null;
                        zombie14InfectButton.Timer = 0f;
                        zombie14InfectButton.isEffectActive = false;
                    }

                    bool canUse = false;
                    if (ZombieLaboratory.zombiePlayer14currentTarget != null) {
                        if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer01) {
                            canUse = !ZombieLaboratory.survivorPlayer01IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer02) {
                            canUse = !ZombieLaboratory.survivorPlayer02IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer03) {
                            canUse = !ZombieLaboratory.survivorPlayer03IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer04) {
                            canUse = !ZombieLaboratory.survivorPlayer04IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer05) {
                            canUse = !ZombieLaboratory.survivorPlayer05IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer06) {
                            canUse = !ZombieLaboratory.survivorPlayer06IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer07) {
                            canUse = !ZombieLaboratory.survivorPlayer07IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer08) {
                            canUse = !ZombieLaboratory.survivorPlayer08IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer09) {
                            canUse = !ZombieLaboratory.survivorPlayer09IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer10) {
                            canUse = !ZombieLaboratory.survivorPlayer10IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer11) {
                            canUse = !ZombieLaboratory.survivorPlayer11IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer12) {
                            canUse = !ZombieLaboratory.survivorPlayer12IsInfected;
                        }
                        else if (ZombieLaboratory.zombiePlayer14currentTarget == ZombieLaboratory.survivorPlayer13) {
                            canUse = !ZombieLaboratory.survivorPlayer13IsInfected;
                        }
                    }
                    return canUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.zombiePlayer14IsReviving && ZombieLaboratory.zombiePlayer14currentTarget != null;
                },
                () => { zombie14InfectButton.Timer = zombie14InfectButton.MaxTimer; },
                ZombieLaboratory.getInfectButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.infectTime,
                () => {
                    if (ZombieLaboratory.zombiePlayer14infectedTarget != null && !ZombieLaboratory.zombiePlayer14infectedTarget.Data.IsDead) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieInfect, Hazel.SendOption.Reliable, -1);
                        writer.Write(ZombieLaboratory.zombiePlayer14infectedTarget.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.zombieInfect(ZombieLaboratory.zombiePlayer14infectedTarget.PlayerId);
                        ZombieLaboratory.zombiePlayer14infectedTarget = null;
                    }
                    zombie14InfectButton.Timer = zombie14InfectButton.MaxTimer;
                }
            );

            // Zombie01 kill
            zombie01KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer01currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(1);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 1);
                    zombie01KillButton.Timer = zombie01KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer01currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer01 != null && ZombieLaboratory.zombiePlayer01 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer01currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer01IsReviving; },
                () => { zombie01KillButton.Timer = zombie01KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie02 kill
            zombie02KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer02currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(2);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 2);
                    zombie02KillButton.Timer = zombie02KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer02currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer02 != null && ZombieLaboratory.zombiePlayer02 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer02currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer02IsReviving; },
                () => { zombie02KillButton.Timer = zombie02KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie03 kill
            zombie03KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer03currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(3);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 3);
                    zombie03KillButton.Timer = zombie03KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer03currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer03 != null && ZombieLaboratory.zombiePlayer03 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer03currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer03IsReviving; },
                () => { zombie03KillButton.Timer = zombie03KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie04 kill
            zombie04KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer04currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(4);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 4);
                    zombie04KillButton.Timer = zombie04KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer04currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer04 != null && ZombieLaboratory.zombiePlayer04 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer04currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer04IsReviving; },
                () => { zombie04KillButton.Timer = zombie04KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie05 kill
            zombie05KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer05currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(5);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 5);
                    zombie05KillButton.Timer = zombie05KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer05currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer05 != null && ZombieLaboratory.zombiePlayer05 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer05currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer05IsReviving; },
                () => { zombie05KillButton.Timer = zombie05KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie06 kill
            zombie06KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer06currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(6);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 6);
                    zombie06KillButton.Timer = zombie06KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer06currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer06 != null && ZombieLaboratory.zombiePlayer06 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer06currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer06IsReviving; },
                () => { zombie06KillButton.Timer = zombie06KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie07 kill
            zombie07KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer07currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(7);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 7);
                    zombie07KillButton.Timer = zombie07KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer07currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer07 != null && ZombieLaboratory.zombiePlayer07 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer07currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer07IsReviving; },
                () => { zombie07KillButton.Timer = zombie07KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie08 kill
            zombie08KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer08currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(8);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 8);
                    zombie08KillButton.Timer = zombie08KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer08currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer08 != null && ZombieLaboratory.zombiePlayer08 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer08currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer08IsReviving; },
                () => { zombie08KillButton.Timer = zombie08KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie09 kill
            zombie09KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer09currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(9);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 9);
                    zombie09KillButton.Timer = zombie09KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer09currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer09 != null && ZombieLaboratory.zombiePlayer09 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer09currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer09IsReviving; },
                () => { zombie09KillButton.Timer = zombie09KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie10 kill
            zombie10KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer10currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(10);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 10);
                    zombie10KillButton.Timer = zombie10KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer10currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer10 != null && ZombieLaboratory.zombiePlayer10 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer10currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer10IsReviving; },
                () => { zombie10KillButton.Timer = zombie10KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie11 kill
            zombie11KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer11currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(11);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 11);
                    zombie11KillButton.Timer = zombie11KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer11currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer11 != null && ZombieLaboratory.zombiePlayer11 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer11currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer11IsReviving; },
                () => { zombie11KillButton.Timer = zombie11KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie12 kill
            zombie12KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer12currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(12);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 12);
                    zombie12KillButton.Timer = zombie12KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer12currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer12 != null && ZombieLaboratory.zombiePlayer12 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer12currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer12IsReviving; },
                () => { zombie12KillButton.Timer = zombie12KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie13 kill
            zombie13KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer13currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(13);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 13);
                    zombie13KillButton.Timer = zombie13KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer13currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer13 != null && ZombieLaboratory.zombiePlayer13 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer13currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer13IsReviving; },
                () => { zombie13KillButton.Timer = zombie13KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Zombie14 kill
            zombie14KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.zombiePlayer14currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(14);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 14);
                    zombie14KillButton.Timer = zombie14KillButton.MaxTimer;
                    ZombieLaboratory.zombiePlayer14currentTarget = null;
                },
                () => { return ZombieLaboratory.zombiePlayer14 != null && ZombieLaboratory.zombiePlayer14 == PlayerControl.LocalPlayer && ZombieLaboratory.whoCanZombiesKill != 2; },
                () => { return ZombieLaboratory.zombiePlayer14currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.zombiePlayer14IsReviving; },
                () => { zombie14KillButton.Timer = zombie14KillButton.MaxTimer; },
                __instance.KillButton.graphic.sprite,
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Nurse EnterExit Button
            nurseEnterExitButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.nursePlayerInsideLaboratory) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        byte whichExit = ZombieLaboratory.nursePlayerCurrentExit;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        enterInfirmary.Write(whichExit);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, whichExit);
                        ZombieLaboratory.nursePlayerInsideLaboratory = false;
                    }
                    else {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = true;
                        byte whichExit = 0;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        enterInfirmary.Write(whichExit);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, whichExit);
                        ZombieLaboratory.nursePlayerInsideLaboratory = true;
                    }
                    nurseEnterExitButton.Timer = nurseEnterExitButton.MaxTimer;
                },
                () => { return ZombieLaboratory.nursePlayer != null && ZombieLaboratory.nursePlayer == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.nursePlayerInsideLaboratory) {
                        nurseEnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getExitLaboratoryButtonSprite();
                    }
                    else {
                        nurseEnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.nurseExits.Count != 0) {
                        foreach (GameObject nurseExit in ZombieLaboratory.nurseExits) {
                            if (nurseExit != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, nurseExit.transform.position) < 0.5f && !ZombieLaboratory.nursePlayer.Data.IsDead) {
                                switch (nurseExit.name) {
                                    case "enterButton":
                                        ZombieLaboratory.nursePlayerCurrentExit = 0;
                                        break;
                                    case "exitButton":
                                        ZombieLaboratory.nursePlayerCurrentExit = 1;
                                        break;
                                    case "exitLeftButton":
                                        ZombieLaboratory.nursePlayerCurrentExit = 2;
                                        break;
                                    case "exitRightButton":
                                        ZombieLaboratory.nursePlayerCurrentExit = 3;
                                        break;
                                }
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.nursePlayerIsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    nurseEnterExitButton.Timer = nurseEnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getExitLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T
            );

            // Nurse medkit
            nurseMedKitButton = new CustomButton(
                () => {

                    if (ZombieLaboratory.nursePlayerHasMedKit) {
                        byte targetId = ZombieLaboratory.nursePlayercurrentTarget.PlayerId;
                        MessageWriter healWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieNurseHeal, Hazel.SendOption.Reliable, -1);
                        healWriter.Write(targetId);
                        AmongUsClient.Instance.FinishRpcImmediately(healWriter);
                        RPCProcedure.zombieNurseHeal(targetId);
                        ZombieLaboratory.nursePlayercurrentTarget = null;
                        ZombieLaboratory.nursePlayerHasMedKit = false;
                    }
                    else {
                        MessageWriter showMedkit = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.NurseHasMedKit, Hazel.SendOption.Reliable, -1);
                        AmongUsClient.Instance.FinishRpcImmediately(showMedkit);
                        RPCProcedure.nurseHasMedKit();
                        ZombieLaboratory.nursePlayerHasMedKit = true;
                    }

                    nurseMedKitButton.Timer = nurseMedKitButton.MaxTimer;
                },
                () => { return ZombieLaboratory.nursePlayer != null && ZombieLaboratory.nursePlayer == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localNurseArrows.Count != 0) {
                        ZombieLaboratory.localNurseArrows[0].Update(ZombieLaboratory.nurseMedkits[0].transform.position, Medusa.color);
                        ZombieLaboratory.localNurseArrows[1].Update(ZombieLaboratory.nurseMedkits[1].transform.position, Medusa.color);
                        ZombieLaboratory.localNurseArrows[2].Update(ZombieLaboratory.nurseMedkits[2].transform.position, Medusa.color);
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.nursePlayerHasMedKit) {
                        nurseMedKitButton.actionButton.graphic.sprite = ZombieLaboratory.getDeliverMedkitButtonSprite();
                        if (ZombieLaboratory.infectedTeam.Count != 0) {
                            foreach (PlayerControl infected in ZombieLaboratory.infectedTeam) {
                                if (infected == ZombieLaboratory.nursePlayercurrentTarget) {
                                    CanUse = true;
                                }
                            }
                        }
                    }
                    else {
                        nurseMedKitButton.actionButton.graphic.sprite = ZombieLaboratory.getPickMedkitButtonSprite();
                        if (ZombieLaboratory.nurseMedkits.Count != 0) {
                            foreach (GameObject nurseMedkit in ZombieLaboratory.nurseMedkits) {
                                if (nurseMedkit != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, nurseMedkit.transform.position) < 0.35f) {
                                    CanUse = !ZombieLaboratory.nursePlayerHasMedKit;
                                }
                            }
                        }
                    }
                    return CanUse && (ZombieLaboratory.nursePlayercurrentTarget || !ZombieLaboratory.nursePlayerHasMedKit) && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.nursePlayerIsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { nurseMedKitButton.Timer = nurseMedKitButton.MaxTimer; },
                ZombieLaboratory.getPickMedkitButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.Q
            );

            // Nurse creatuecure Button
            nurseCreateCureButton = new CustomButton(
                () => {
                    MessageWriter winSurvivorsWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieSurvivorsWin, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(winSurvivorsWriter);
                    RPCProcedure.zombieSurvivorsWin();
                    nurseCreateCureButton.Timer = nurseCreateCureButton.MaxTimer;
                },
                () => { return ZombieLaboratory.nursePlayer != null && ZombieLaboratory.nursePlayer == PlayerControl.LocalPlayer; },
                () => {
                    bool CanUse = false;
                    if (ZombieLaboratory.nursePlayerHasCureReady) {
                        if ((ZombieLaboratory.laboratorytwoCreateCureButton != null && Vector2.Distance(ZombieLaboratory.nursePlayer.transform.position, ZombieLaboratory.laboratorytwoCreateCureButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.nursePlayer.transform.position, ZombieLaboratory.laboratoryCreateCureButton.transform.position) < 0.35f) && !ZombieLaboratory.nursePlayer.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.nursePlayerIsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    nurseCreateCureButton.Timer = nurseCreateCureButton.MaxTimer;
                },
                ZombieLaboratory.getCreateCureButtonSprite(),
                new Vector3(0f, 1f, 0),
                __instance,
                KeyCode.T
            );

            // Survivor01 kill
            survivor01KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer01currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(15);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 15);
                    survivor01KillButton.Timer = survivor01KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer01currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer01 != null && ZombieLaboratory.survivorPlayer01 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer01CanKill) {
                        survivor01KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor01KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer01currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer01IsReviving && ZombieLaboratory.survivorPlayer01CanKill && !ZombieLaboratory.survivorPlayer01HasKeyItem;
                },
                () => { survivor01KillButton.Timer = survivor01KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor01 FindDeliver Button
            survivor01FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer01HasKeyItem) {
                        survivor01FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer01FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor01FindDeliverButton.Timer = survivor01FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor01FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer01SelectedBox = ZombieLaboratory.survivorPlayer01CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer01 != null && ZombieLaboratory.survivorPlayer01 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor01FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer01SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer01SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer01SelectedBox = null;
                        survivor01FindDeliverButton.Timer = 0f;
                        survivor01FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer01HasKeyItem)
                        survivor01FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor01FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer01HasKeyItem) {
                                ZombieLaboratory.survivorPlayer01CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer01FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer01FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer01FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer01FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer01FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer01FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer01FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer01HasKeyItem || !ZombieLaboratory.survivorPlayer01CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer01FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer01HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer01HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer01IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor01FindDeliverButton.Timer = survivor01FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer01.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer01FoundBox >= 1 && ZombieLaboratory.survivorPlayer01FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer01FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer01FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(1);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(1);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer01SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer01SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer01SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor01FindDeliverButton.Timer = survivor01FindDeliverButton.MaxTimer;
                }
            );

            // Survivor01 EnterExit Button
            survivor01EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor01EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer01 != null && ZombieLaboratory.survivorPlayer01 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor01EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer01.Data.IsDead) {
                            survivor01EnterExitButton.HasEffect = false;
                            survivor01EnterExitButton.Timer = survivor01EnterExitButton.MaxTimer;
                            survivor01EnterExitButton.isEffectActive = false;
                        }
                        survivor01EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor01EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }

                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer01.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer01.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer01.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer01IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor01EnterExitButton.Timer = survivor01EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer01.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor01EnterExitButton.Timer = survivor01EnterExitButton.MaxTimer;
                }
            );

            // Survivor02 kill
            survivor02KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer02currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(16);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 16);
                    survivor02KillButton.Timer = survivor02KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer02currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer02 != null && ZombieLaboratory.survivorPlayer02 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer02CanKill) {
                        survivor02KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor02KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer02currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer02IsReviving && ZombieLaboratory.survivorPlayer02CanKill && !ZombieLaboratory.survivorPlayer02HasKeyItem;
                },
                () => { survivor02KillButton.Timer = survivor02KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor02 FindDeliver Button
            survivor02FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer02HasKeyItem) {
                        survivor02FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer02FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor02FindDeliverButton.Timer = survivor02FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor02FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer02SelectedBox = ZombieLaboratory.survivorPlayer02CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer02 != null && ZombieLaboratory.survivorPlayer02 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor02FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer02SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer02SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer02SelectedBox = null;
                        survivor02FindDeliverButton.Timer = 0f;
                        survivor02FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer02HasKeyItem)
                        survivor02FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor02FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer02HasKeyItem) {
                                ZombieLaboratory.survivorPlayer02CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer02FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer02FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer02FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer02FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer02FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer02FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer02FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer02HasKeyItem || !ZombieLaboratory.survivorPlayer02CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer02FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer02HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer02HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer02IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor02FindDeliverButton.Timer = survivor02FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer02.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer02FoundBox >= 1 && ZombieLaboratory.survivorPlayer02FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer02FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer02FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(2);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(2);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer02SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer02SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer02SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor02FindDeliverButton.Timer = survivor02FindDeliverButton.MaxTimer;
                }
            );

            // Survivor02 EnterExit Button
            survivor02EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor02EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer02 != null && ZombieLaboratory.survivorPlayer02 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor02EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer02.Data.IsDead) {
                            survivor02EnterExitButton.HasEffect = false;
                            survivor02EnterExitButton.Timer = survivor02EnterExitButton.MaxTimer;
                            survivor02EnterExitButton.isEffectActive = false;
                        }
                        survivor02EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor02EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer02.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer02.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer02.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer02IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor02EnterExitButton.Timer = survivor02EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer02.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor02EnterExitButton.Timer = survivor02EnterExitButton.MaxTimer;
                }
            );

            // Survivor03 kill
            survivor03KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer03currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(17);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 17);
                    survivor03KillButton.Timer = survivor03KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer03currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer03 != null && ZombieLaboratory.survivorPlayer03 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer03CanKill) {
                        survivor03KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor03KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer03currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer03IsReviving && ZombieLaboratory.survivorPlayer03CanKill && !ZombieLaboratory.survivorPlayer03HasKeyItem;
                },
                () => { survivor03KillButton.Timer = survivor03KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor03 FindDeliver Button
            survivor03FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer03HasKeyItem) {
                        survivor03FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer03FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor03FindDeliverButton.Timer = survivor03FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor03FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer03SelectedBox = ZombieLaboratory.survivorPlayer03CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer03 != null && ZombieLaboratory.survivorPlayer03 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor03FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer03SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer03SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer03SelectedBox = null;
                        survivor03FindDeliverButton.Timer = 0f;
                        survivor03FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer03HasKeyItem)
                        survivor03FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor03FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer03HasKeyItem) {
                                ZombieLaboratory.survivorPlayer03CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer03FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer03FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer03FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer03FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer03FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer03FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer03FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer03HasKeyItem || !ZombieLaboratory.survivorPlayer03CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer03FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer03HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer03HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer03IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor03FindDeliverButton.Timer = survivor03FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer03.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer03FoundBox >= 1 && ZombieLaboratory.survivorPlayer03FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer03FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer03FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(3);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(3);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer03SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer03SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer03SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor03FindDeliverButton.Timer = survivor03FindDeliverButton.MaxTimer;
                }
            );

            // Survivor03 EnterExit Button
            survivor03EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor03EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer03 != null && ZombieLaboratory.survivorPlayer03 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor03EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer03.Data.IsDead) {
                            survivor03EnterExitButton.HasEffect = false;
                            survivor03EnterExitButton.Timer = survivor03EnterExitButton.MaxTimer;
                            survivor03EnterExitButton.isEffectActive = false;
                        }
                        survivor03EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor03EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer03.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer03.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer03.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer03IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor03EnterExitButton.Timer = survivor03EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer03.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor03EnterExitButton.Timer = survivor03EnterExitButton.MaxTimer;
                }
            );

            // Survivor04 kill
            survivor04KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer04currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(18);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 18);
                    survivor04KillButton.Timer = survivor04KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer04currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer04 != null && ZombieLaboratory.survivorPlayer04 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer04CanKill) {
                        survivor04KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor04KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer04currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer04IsReviving && ZombieLaboratory.survivorPlayer04CanKill && !ZombieLaboratory.survivorPlayer04HasKeyItem;
                },
                () => { survivor04KillButton.Timer = survivor04KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor04 FindDeliver Button
            survivor04FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer04HasKeyItem) {
                        survivor04FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer04FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor04FindDeliverButton.Timer = survivor04FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor04FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer04SelectedBox = ZombieLaboratory.survivorPlayer04CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer04 != null && ZombieLaboratory.survivorPlayer04 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor04FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer04SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer04SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer04SelectedBox = null;
                        survivor04FindDeliverButton.Timer = 0f;
                        survivor04FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer04HasKeyItem)
                        survivor04FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor04FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer04HasKeyItem) {
                                ZombieLaboratory.survivorPlayer04CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer04FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer04FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer04FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer04FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer04FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer04FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer04FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer04HasKeyItem || !ZombieLaboratory.survivorPlayer04CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer04FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer04HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer04HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer04IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor04FindDeliverButton.Timer = survivor04FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer04.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer04FoundBox >= 1 && ZombieLaboratory.survivorPlayer04FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer04FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer04FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(4);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(4);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer04SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer04SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer04SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor04FindDeliverButton.Timer = survivor04FindDeliverButton.MaxTimer;
                }
            );

            // Survivor04 EnterExit Button
            survivor04EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor04EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer04 != null && ZombieLaboratory.survivorPlayer04 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor04EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer04.Data.IsDead) {
                            survivor04EnterExitButton.HasEffect = false;
                            survivor04EnterExitButton.Timer = survivor04EnterExitButton.MaxTimer;
                            survivor04EnterExitButton.isEffectActive = false;
                        }
                        survivor04EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor04EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer04.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer04.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer04.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer04IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor04EnterExitButton.Timer = survivor04EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer04.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor04EnterExitButton.Timer = survivor04EnterExitButton.MaxTimer;
                }
            );

            // Survivor05 kill
            survivor05KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer05currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(19);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 19);
                    survivor05KillButton.Timer = survivor05KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer05currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer05 != null && ZombieLaboratory.survivorPlayer05 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer05CanKill) {
                        survivor05KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor05KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer05currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer05IsReviving && ZombieLaboratory.survivorPlayer05CanKill && !ZombieLaboratory.survivorPlayer05HasKeyItem;
                },
                () => { survivor05KillButton.Timer = survivor05KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor05 FindDeliver Button
            survivor05FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer05HasKeyItem) {
                        survivor05FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer05FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor05FindDeliverButton.Timer = survivor05FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor05FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer05SelectedBox = ZombieLaboratory.survivorPlayer05CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer05 != null && ZombieLaboratory.survivorPlayer05 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor05FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer05SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer05SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer05SelectedBox = null;
                        survivor05FindDeliverButton.Timer = 0f;
                        survivor05FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer05HasKeyItem)
                        survivor05FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor05FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer05HasKeyItem) {
                                ZombieLaboratory.survivorPlayer05CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer05FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer05FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer05FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer05FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer05FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer05FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer05FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer05HasKeyItem || !ZombieLaboratory.survivorPlayer05CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer05FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer05HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer05HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer05IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor05FindDeliverButton.Timer = survivor05FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer05.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer05FoundBox >= 1 && ZombieLaboratory.survivorPlayer05FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer05FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer05FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(5);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(5);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer05SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer05SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer05SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor05FindDeliverButton.Timer = survivor05FindDeliverButton.MaxTimer;
                }
            );

            // Survivor05 EnterExit Button
            survivor05EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor05EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer05 != null && ZombieLaboratory.survivorPlayer05 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor05EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer05.Data.IsDead) {
                            survivor05EnterExitButton.HasEffect = false;
                            survivor05EnterExitButton.Timer = survivor05EnterExitButton.MaxTimer;
                            survivor05EnterExitButton.isEffectActive = false;
                        }
                        survivor05EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor05EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer05.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer05.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer05.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer05IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor05EnterExitButton.Timer = survivor05EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer05.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor05EnterExitButton.Timer = survivor05EnterExitButton.MaxTimer;
                }
            );

            // Survivor06 kill
            survivor06KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer06currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(20);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 20);
                    survivor06KillButton.Timer = survivor06KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer06currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer06 != null && ZombieLaboratory.survivorPlayer06 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer06CanKill) {
                        survivor06KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor06KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer06currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer06IsReviving && ZombieLaboratory.survivorPlayer06CanKill && !ZombieLaboratory.survivorPlayer06HasKeyItem;
                },
                () => { survivor06KillButton.Timer = survivor06KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor06 FindDeliver Button
            survivor06FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer06HasKeyItem) {
                        survivor06FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer06FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor06FindDeliverButton.Timer = survivor06FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor06FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer06SelectedBox = ZombieLaboratory.survivorPlayer06CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer06 != null && ZombieLaboratory.survivorPlayer06 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor06FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer06SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer06SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer06SelectedBox = null;
                        survivor06FindDeliverButton.Timer = 0f;
                        survivor06FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer06HasKeyItem)
                        survivor06FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor06FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer06HasKeyItem) {
                                ZombieLaboratory.survivorPlayer06CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer06FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer06FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer06FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer06FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer06FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer06FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer06FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer06HasKeyItem || !ZombieLaboratory.survivorPlayer06CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer06FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer06HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer06HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer06IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor06FindDeliverButton.Timer = survivor06FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer06.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer06FoundBox >= 1 && ZombieLaboratory.survivorPlayer06FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer06FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer06FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(6);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(6);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer06SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer06SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer06SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor06FindDeliverButton.Timer = survivor06FindDeliverButton.MaxTimer;
                }
            );

            // Survivor06 EnterExit Button
            survivor06EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor06EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer06 != null && ZombieLaboratory.survivorPlayer06 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor06EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer06.Data.IsDead) {
                            survivor06EnterExitButton.HasEffect = false;
                            survivor06EnterExitButton.Timer = survivor06EnterExitButton.MaxTimer;
                            survivor06EnterExitButton.isEffectActive = false;
                        }
                        survivor06EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor06EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer06.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer06.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer06.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer06IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor06EnterExitButton.Timer = survivor06EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer06.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor06EnterExitButton.Timer = survivor06EnterExitButton.MaxTimer;
                }
            );

            // Survivor07 kill
            survivor07KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer07currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(21);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 21);
                    survivor07KillButton.Timer = survivor07KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer07currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer07 != null && ZombieLaboratory.survivorPlayer07 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer07CanKill) {
                        survivor07KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor07KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer07currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer07IsReviving && ZombieLaboratory.survivorPlayer07CanKill && !ZombieLaboratory.survivorPlayer07HasKeyItem;
                },
                () => { survivor07KillButton.Timer = survivor07KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor07 FindDeliver Button
            survivor07FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer07HasKeyItem) {
                        survivor07FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer07FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor07FindDeliverButton.Timer = survivor07FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor07FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer07SelectedBox = ZombieLaboratory.survivorPlayer07CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer07 != null && ZombieLaboratory.survivorPlayer07 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor07FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer07SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer07SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer07SelectedBox = null;
                        survivor07FindDeliverButton.Timer = 0f;
                        survivor07FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer07HasKeyItem)
                        survivor07FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor07FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer07HasKeyItem) {
                                ZombieLaboratory.survivorPlayer07CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer07FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer07FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer07FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer07FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer07FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer07FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer07FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer07HasKeyItem || !ZombieLaboratory.survivorPlayer07CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer07FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer07HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer07HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer07IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor07FindDeliverButton.Timer = survivor07FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer07.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer07FoundBox >= 1 && ZombieLaboratory.survivorPlayer07FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer07FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer07FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(7);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(7);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer07SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer07SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer07SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor07FindDeliverButton.Timer = survivor07FindDeliverButton.MaxTimer;
                }
            );

            // Survivor07 EnterExit Button
            survivor07EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor07EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer07 != null && ZombieLaboratory.survivorPlayer07 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor07EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer07.Data.IsDead) {
                            survivor07EnterExitButton.HasEffect = false;
                            survivor07EnterExitButton.Timer = survivor07EnterExitButton.MaxTimer;
                            survivor07EnterExitButton.isEffectActive = false;
                        }
                        survivor07EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor07EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer07.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer07.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer07.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer07IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor07EnterExitButton.Timer = survivor07EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer07.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor07EnterExitButton.Timer = survivor07EnterExitButton.MaxTimer;
                }
            );

            // Survivor08 kill
            survivor08KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer08currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(22);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 22);
                    survivor08KillButton.Timer = survivor08KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer08currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer08 != null && ZombieLaboratory.survivorPlayer08 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer08CanKill) {
                        survivor08KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor08KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer08currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer08IsReviving && ZombieLaboratory.survivorPlayer08CanKill && !ZombieLaboratory.survivorPlayer08HasKeyItem;
                },
                () => { survivor08KillButton.Timer = survivor08KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor08 FindDeliver Button
            survivor08FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer08HasKeyItem) {
                        survivor08FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer08FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor08FindDeliverButton.Timer = survivor08FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor08FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer08SelectedBox = ZombieLaboratory.survivorPlayer08CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer08 != null && ZombieLaboratory.survivorPlayer08 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor08FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer08SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer08SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer08SelectedBox = null;
                        survivor08FindDeliverButton.Timer = 0f;
                        survivor08FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer08HasKeyItem)
                        survivor08FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor08FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer08HasKeyItem) {
                                ZombieLaboratory.survivorPlayer08CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer08FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer08FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer08FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer08FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer08FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer08FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer08FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer08HasKeyItem || !ZombieLaboratory.survivorPlayer08CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer08FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer08HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer08HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer08IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor08FindDeliverButton.Timer = survivor08FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer08.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer08FoundBox >= 1 && ZombieLaboratory.survivorPlayer08FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer08FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer08FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(8);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(8);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer08SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer08SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer08SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor08FindDeliverButton.Timer = survivor08FindDeliverButton.MaxTimer;
                }
            );

            // Survivor08 EnterExit Button
            survivor08EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor08EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer08 != null && ZombieLaboratory.survivorPlayer08 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }

                    if (survivor08EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer08.Data.IsDead) {
                            survivor08EnterExitButton.HasEffect = false;
                            survivor08EnterExitButton.Timer = survivor08EnterExitButton.MaxTimer;
                            survivor08EnterExitButton.isEffectActive = false;
                        }
                        survivor08EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor08EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer08.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer08.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer08.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer08IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor08EnterExitButton.Timer = survivor08EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer08.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor08EnterExitButton.Timer = survivor08EnterExitButton.MaxTimer;
                }
            );

            // Survivor09 kill
            survivor09KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer09currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(23);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 23);
                    survivor09KillButton.Timer = survivor09KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer09currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer09 != null && ZombieLaboratory.survivorPlayer09 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer09CanKill) {
                        survivor09KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor09KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer09currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer09IsReviving && ZombieLaboratory.survivorPlayer09CanKill && !ZombieLaboratory.survivorPlayer09HasKeyItem;
                },
                () => { survivor09KillButton.Timer = survivor09KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor09 FindDeliver Button
            survivor09FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer09HasKeyItem) {
                        survivor09FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer09FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor09FindDeliverButton.Timer = survivor09FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor09FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer09SelectedBox = ZombieLaboratory.survivorPlayer09CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer09 != null && ZombieLaboratory.survivorPlayer09 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor09FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer09SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer09SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer09SelectedBox = null;
                        survivor09FindDeliverButton.Timer = 0f;
                        survivor09FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer09HasKeyItem)
                        survivor09FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor09FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer09HasKeyItem) {
                                ZombieLaboratory.survivorPlayer09CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer09FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer09FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer09FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer09FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer09FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer09FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer09FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer09HasKeyItem || !ZombieLaboratory.survivorPlayer09CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer09FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer09HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer09HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer09IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor09FindDeliverButton.Timer = survivor09FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer09.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer09FoundBox >= 1 && ZombieLaboratory.survivorPlayer09FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer09FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer09FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(9);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(9);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer09SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer09SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer09SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor09FindDeliverButton.Timer = survivor09FindDeliverButton.MaxTimer;
                }
            );

            // Survivor09 EnterExit Button
            survivor09EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor09EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer09 != null && ZombieLaboratory.survivorPlayer09 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor09EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer09.Data.IsDead) {
                            survivor09EnterExitButton.HasEffect = false;
                            survivor09EnterExitButton.Timer = survivor09EnterExitButton.MaxTimer;
                            survivor09EnterExitButton.isEffectActive = false;
                        }
                        survivor09EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor09EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer09.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer09.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer09.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer09IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor09EnterExitButton.Timer = survivor09EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer09.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor09EnterExitButton.Timer = survivor09EnterExitButton.MaxTimer;
                }
            );

            // Survivor10 kill
            survivor10KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer10currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(24);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 24);
                    survivor10KillButton.Timer = survivor10KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer10currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer10 != null && ZombieLaboratory.survivorPlayer10 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer10CanKill) {
                        survivor10KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor10KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer10currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer10IsReviving && ZombieLaboratory.survivorPlayer10CanKill && !ZombieLaboratory.survivorPlayer10HasKeyItem;
                },
                () => { survivor10KillButton.Timer = survivor10KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor10 FindDeliver Button
            survivor10FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer10HasKeyItem) {
                        survivor10FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer10FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor10FindDeliverButton.Timer = survivor10FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor10FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer10SelectedBox = ZombieLaboratory.survivorPlayer10CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer10 != null && ZombieLaboratory.survivorPlayer10 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor10FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer10SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer10SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer10SelectedBox = null;
                        survivor10FindDeliverButton.Timer = 0f;
                        survivor10FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer10HasKeyItem)
                        survivor10FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor10FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer10HasKeyItem) {
                                ZombieLaboratory.survivorPlayer10CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer10FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer10FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer10FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer10FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer10FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer10FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer10FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer10HasKeyItem || !ZombieLaboratory.survivorPlayer10CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer10FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer10HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer10HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer10IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor10FindDeliverButton.Timer = survivor10FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer10.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer10FoundBox >= 1 && ZombieLaboratory.survivorPlayer10FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer10FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer10FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(10);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(10);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer10SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer10SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer10SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor10FindDeliverButton.Timer = survivor10FindDeliverButton.MaxTimer;
                }
            );

            // Survivor10 EnterExit Button
            survivor10EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor10EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer10 != null && ZombieLaboratory.survivorPlayer10 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor10EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer10.Data.IsDead) {
                            survivor10EnterExitButton.HasEffect = false;
                            survivor10EnterExitButton.Timer = survivor10EnterExitButton.MaxTimer;
                            survivor10EnterExitButton.isEffectActive = false;
                        }
                        survivor10EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor10EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer10.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer10.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer10.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer10IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor10EnterExitButton.Timer = survivor10EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer10.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor10EnterExitButton.Timer = survivor10EnterExitButton.MaxTimer;
                }
            );

            // Survivor11 kill
            survivor11KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer11currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(25);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 25);
                    survivor11KillButton.Timer = survivor11KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer11currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer11 != null && ZombieLaboratory.survivorPlayer11 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer11CanKill) {
                        survivor11KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor11KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer11currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer11IsReviving && ZombieLaboratory.survivorPlayer11CanKill && !ZombieLaboratory.survivorPlayer11HasKeyItem;
                },
                () => { survivor11KillButton.Timer = survivor11KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor11 FindDeliver Button
            survivor11FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer11HasKeyItem) {
                        survivor11FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer11FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor11FindDeliverButton.Timer = survivor11FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor11FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer11SelectedBox = ZombieLaboratory.survivorPlayer11CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer11 != null && ZombieLaboratory.survivorPlayer11 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor11FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer11SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer11SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer11SelectedBox = null;
                        survivor11FindDeliverButton.Timer = 0f;
                        survivor11FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer11HasKeyItem)
                        survivor11FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor11FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer11HasKeyItem) {
                                ZombieLaboratory.survivorPlayer11CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer11FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer11FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer11FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer11FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer11FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer11FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer11FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer11HasKeyItem || !ZombieLaboratory.survivorPlayer11CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer11FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer11HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer11HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer11IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor11FindDeliverButton.Timer = survivor11FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer11.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer11FoundBox >= 1 && ZombieLaboratory.survivorPlayer11FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer11FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer11FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(11);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(11);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer11SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer11SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer11SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor11FindDeliverButton.Timer = survivor11FindDeliverButton.MaxTimer;
                }
            );

            // Survivor11 EnterExit Button
            survivor11EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor11EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer11 != null && ZombieLaboratory.survivorPlayer11 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor11EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer11.Data.IsDead) {
                            survivor11EnterExitButton.HasEffect = false;
                            survivor11EnterExitButton.Timer = survivor11EnterExitButton.MaxTimer;
                            survivor11EnterExitButton.isEffectActive = false;
                        }
                        survivor11EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor11EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer11.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer11.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer11.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer11IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor11EnterExitButton.Timer = survivor11EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer11.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor11EnterExitButton.Timer = survivor11EnterExitButton.MaxTimer;
                }
            );

            // Survivor12 kill
            survivor12KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer12currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(26);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 26);
                    survivor12KillButton.Timer = survivor12KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer12currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer12 != null && ZombieLaboratory.survivorPlayer12 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer12CanKill) {
                        survivor12KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor12KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer12currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer12IsReviving && ZombieLaboratory.survivorPlayer12CanKill && !ZombieLaboratory.survivorPlayer12HasKeyItem;
                },
                () => { survivor12KillButton.Timer = survivor12KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor12 FindDeliver Button
            survivor12FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer12HasKeyItem) {
                        survivor12FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer12FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor12FindDeliverButton.Timer = survivor12FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor12FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer12SelectedBox = ZombieLaboratory.survivorPlayer12CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer12 != null && ZombieLaboratory.survivorPlayer12 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor12FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer12SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer12SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer12SelectedBox = null;
                        survivor12FindDeliverButton.Timer = 0f;
                        survivor12FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer12HasKeyItem)
                        survivor12FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor12FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer12HasKeyItem) {
                                ZombieLaboratory.survivorPlayer12CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer12FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer12FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer12FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer12FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer12FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer12FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer12FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer12HasKeyItem || !ZombieLaboratory.survivorPlayer12CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer12FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer12HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer12HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer12IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor12FindDeliverButton.Timer = survivor12FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer12.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer12FoundBox >= 1 && ZombieLaboratory.survivorPlayer12FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer12FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer12FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(12);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(12);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer12SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer12SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer12SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor12FindDeliverButton.Timer = survivor12FindDeliverButton.MaxTimer;
                }
            );

            // Survivor12 EnterExit Button
            survivor12EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor12EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer12 != null && ZombieLaboratory.survivorPlayer12 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor12EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer12.Data.IsDead) {
                            survivor12EnterExitButton.HasEffect = false;
                            survivor12EnterExitButton.Timer = survivor12EnterExitButton.MaxTimer;
                            survivor12EnterExitButton.isEffectActive = false;
                        }
                        survivor12EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor12EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer12.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer12.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer12.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer12IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor12EnterExitButton.Timer = survivor12EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer12.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor12EnterExitButton.Timer = survivor12EnterExitButton.MaxTimer;
                }
            );

            // Survivor13 kill
            survivor13KillButton = new CustomButton(
                () => {
                    byte targetId = ZombieLaboratory.survivorPlayer13currentTarget.PlayerId;
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieKills, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(targetId);
                    killWriter.Write(27);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.zombieKills(targetId, 27);
                    survivor13KillButton.Timer = survivor13KillButton.MaxTimer;
                    ZombieLaboratory.survivorPlayer13currentTarget = null;
                },
                () => { return ZombieLaboratory.survivorPlayer13 != null && ZombieLaboratory.survivorPlayer13 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.survivorPlayer13CanKill) {
                        survivor13KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorFullShootButtonSprite();
                    }
                    else {
                        survivor13KillButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEmptyShootButtonSprite();
                    }
                    return ZombieLaboratory.survivorPlayer13currentTarget && PlayerControl.LocalPlayer.CanMove && !PlayerControl.LocalPlayer.Data.IsDead && !ZombieLaboratory.survivorPlayer13IsReviving && ZombieLaboratory.survivorPlayer13CanKill && !ZombieLaboratory.survivorPlayer13HasKeyItem;
                },
                () => { survivor13KillButton.Timer = survivor13KillButton.MaxTimer; },
                ZombieLaboratory.getSurvivorEmptyShootButtonSprite(),
                new Vector3(0, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // Survivor13 FindDeliver Button
            survivor13FindDeliverButton = new CustomButton(
                () => {
                    if (ZombieLaboratory.survivorPlayer13HasKeyItem) {
                        survivor13FindDeliverButton.HasEffect = false;
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        byte keyId = ZombieLaboratory.survivorPlayer13FoundBox;
                        MessageWriter deliverKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieDeliverKeyItem, Hazel.SendOption.Reliable, -1);
                        deliverKey.Write(targetId);
                        deliverKey.Write(keyId);
                        AmongUsClient.Instance.FinishRpcImmediately(deliverKey);
                        RPCProcedure.zombieDeliverKeyItem(targetId, keyId);
                        survivor13FindDeliverButton.Timer = survivor13FindDeliverButton.MaxTimer;
                    }
                    else {
                        survivor13FindDeliverButton.HasEffect = true;
                        ZombieLaboratory.survivorPlayer13SelectedBox = ZombieLaboratory.survivorPlayer13CurrentBox;
                    }
                },
                () => { return ZombieLaboratory.survivorPlayer13 != null && ZombieLaboratory.survivorPlayer13 == PlayerControl.LocalPlayer; },
                () => {
                    if (survivor13FindDeliverButton.isEffectActive && ZombieLaboratory.survivorPlayer13SelectedBox != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.survivorPlayer13SelectedBox.transform.position) > 0.5f) {
                        ZombieLaboratory.survivorPlayer13SelectedBox = null;
                        survivor13FindDeliverButton.Timer = 0f;
                        survivor13FindDeliverButton.isEffectActive = false;
                    }

                    if (ZombieLaboratory.survivorPlayer13HasKeyItem)
                        survivor13FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorDeliverBoxButtonSprite();
                    else
                        survivor13FindDeliverButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorTakeBoxButtonSprite();
                    bool CanUse = false;
                    if (ZombieLaboratory.groundItems.Count != 0) {
                        foreach (GameObject groundItem in ZombieLaboratory.groundItems) {
                            if (groundItem != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, groundItem.transform.position) < 0.5f && !ZombieLaboratory.survivorPlayer13HasKeyItem) {
                                ZombieLaboratory.survivorPlayer13CurrentBox = groundItem;
                                switch (groundItem.name) {
                                    case "keyItem01":
                                        ZombieLaboratory.survivorPlayer13FoundBox = 1;
                                        CanUse = !ZombieLaboratory.keyItem01BeingHeld;
                                        break;
                                    case "keyItem02":
                                        ZombieLaboratory.survivorPlayer13FoundBox = 2;
                                        CanUse = !ZombieLaboratory.keyItem02BeingHeld;
                                        break;
                                    case "keyItem03":
                                        ZombieLaboratory.survivorPlayer13FoundBox = 3;
                                        CanUse = !ZombieLaboratory.keyItem03BeingHeld;
                                        break;
                                    case "keyItem04":
                                        ZombieLaboratory.survivorPlayer13FoundBox = 4;
                                        CanUse = !ZombieLaboratory.keyItem04BeingHeld;
                                        break;
                                    case "keyItem05":
                                        ZombieLaboratory.survivorPlayer13FoundBox = 5;
                                        CanUse = !ZombieLaboratory.keyItem05BeingHeld;
                                        break;
                                    case "keyItem06":
                                        ZombieLaboratory.survivorPlayer13FoundBox = 6;
                                        CanUse = !ZombieLaboratory.keyItem06BeingHeld;
                                        break;
                                    case "ammoBox":
                                        ZombieLaboratory.survivorPlayer13FoundBox = 7;
                                        CanUse = (!ZombieLaboratory.survivorPlayer13HasKeyItem || !ZombieLaboratory.survivorPlayer13CanKill);
                                        break;
                                    case "nothingBox":
                                        ZombieLaboratory.survivorPlayer13FoundBox = 8;
                                        CanUse = !ZombieLaboratory.survivorPlayer13HasKeyItem;
                                        break;
                                    case "nothingBoxOpened":
                                        CanUse = false;
                                        break;
                                }
                            }
                            else if ((ZombieLaboratory.laboratorytwoPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratorytwoPutKeyItemButton.transform.position) < 0.5f || ZombieLaboratory.laboratoryPutKeyItemButton != null && Vector2.Distance(PlayerControl.LocalPlayer.transform.position, ZombieLaboratory.laboratoryPutKeyItemButton.transform.position) < 0.5f) && ZombieLaboratory.survivorPlayer13HasKeyItem) {
                                CanUse = true;
                            }
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer13IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => { survivor13FindDeliverButton.Timer = survivor13FindDeliverButton.MaxTimer; },
                ZombieLaboratory.getSurvivorTakeBoxButtonSprite(),
                new Vector3(-1.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                ZombieLaboratory.searchBoxTimer,
                () => {
                    if (!ZombieLaboratory.survivorPlayer13.Data.IsDead) {
                        if (ZombieLaboratory.survivorPlayer13FoundBox >= 1 && ZombieLaboratory.survivorPlayer13FoundBox <= 6) {
                            byte targetId = PlayerControl.LocalPlayer.PlayerId;
                            byte keyId = ZombieLaboratory.survivorPlayer13FoundBox;
                            MessageWriter survivorWhoFoundKey = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieTakeKeyItem, Hazel.SendOption.Reliable, -1);
                            survivorWhoFoundKey.Write(targetId);
                            survivorWhoFoundKey.Write(keyId);
                            AmongUsClient.Instance.FinishRpcImmediately(survivorWhoFoundKey);
                            RPCProcedure.zombieTakeKeyItem(targetId, keyId);
                        }
                        else if (ZombieLaboratory.survivorPlayer13FoundBox == 7) {
                            MessageWriter ammoRecovered = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ZombieAmmoRecover, Hazel.SendOption.Reliable, -1);
                            ammoRecovered.Write(13);
                            AmongUsClient.Instance.FinishRpcImmediately(ammoRecovered);
                            RPCProcedure.zombieAmmoRecover(13);

                            SoundManager.Instance.PlaySound(CustomMain.customAssets.rechargeAmmoClip, false, 100f);

                            ZombieLaboratory.survivorPlayer13SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.ammoBox.GetComponent<SpriteRenderer>().sprite;
                        }
                        else {
                            ZombieLaboratory.survivorPlayer13SelectedBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.emptyBox.GetComponent<SpriteRenderer>().sprite;
                            ZombieLaboratory.survivorPlayer13SelectedBox.name = "nothingBoxOpened";
                            // nothing box
                        }
                    }
                    survivor13FindDeliverButton.Timer = survivor13FindDeliverButton.MaxTimer;
                }
            );

            // Survivor13 EnterExit Button
            survivor13EnterExitButton = new CustomButton(
                () => {
                    byte targetId = PlayerControl.LocalPlayer.PlayerId;
                    bool entering = true;
                    MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                    enterInfirmary.Write(targetId);
                    enterInfirmary.Write(entering);
                    AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                    RPCProcedure.enterLeaveInfirmary(targetId, entering, 0);
                    survivor13EnterExitButton.HasEffect = true;
                },
                () => { return ZombieLaboratory.survivorPlayer13 != null && ZombieLaboratory.survivorPlayer13 == PlayerControl.LocalPlayer; },
                () => {
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count != 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].Update(ZombieLaboratory.laboratoryEnterButton.transform.position);
                        if (PlayerControl.GameOptions.MapId == 5) {
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].Update(ZombieLaboratory.laboratorytwoEnterButton.transform.position);
                        }
                    }
                    
                    if (survivor13EnterExitButton.isEffectActive) {
                        if (ZombieLaboratory.survivorPlayer13.Data.IsDead) {
                            survivor13EnterExitButton.HasEffect = false;
                            survivor13EnterExitButton.Timer = survivor13EnterExitButton.MaxTimer;
                            survivor13EnterExitButton.isEffectActive = false;
                        }
                        survivor13EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorExitLaboratoryButtonSprite();
                    }
                    else {
                        survivor13EnterExitButton.actionButton.graphic.sprite = ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite();
                    }
                    bool CanUse = false;
                    if (ZombieLaboratory.laboratory != null) {
                        if ((ZombieLaboratory.laboratorytwoEnterButton != null && Vector2.Distance(ZombieLaboratory.survivorPlayer13.transform.position, ZombieLaboratory.laboratorytwoEnterButton.transform.position) < 0.35f || Vector2.Distance(ZombieLaboratory.survivorPlayer13.transform.position, ZombieLaboratory.laboratoryEnterButton.transform.position) < 0.35f) && !ZombieLaboratory.survivorPlayer13.Data.IsDead) {
                            CanUse = true;
                        }
                    }
                    return CanUse && PlayerControl.LocalPlayer.CanMove && !ZombieLaboratory.survivorPlayer13IsReviving && !PlayerControl.LocalPlayer.Data.IsDead;
                },
                () => {
                    survivor13EnterExitButton.Timer = survivor13EnterExitButton.MaxTimer;
                },
                ZombieLaboratory.getSurvivorEnterLaboratoryButtonSprite(),
                new Vector3(-0.9f, -0.06f, 0),
                __instance,
                KeyCode.T,
                false,
                10f,
                () => {
                    if (!ZombieLaboratory.survivorPlayer13.Data.IsDead) {
                        byte targetId = PlayerControl.LocalPlayer.PlayerId;
                        bool entering = false;
                        MessageWriter enterInfirmary = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.EnterLeaveInfirmary, Hazel.SendOption.Reliable, -1);
                        enterInfirmary.Write(targetId);
                        enterInfirmary.Write(entering);
                        AmongUsClient.Instance.FinishRpcImmediately(enterInfirmary);
                        RPCProcedure.enterLeaveInfirmary(targetId, entering, 1);
                    }
                    survivor13EnterExitButton.Timer = survivor13EnterExitButton.MaxTimer;
                }
            );

            setCustomButtonCooldowns();
        }
    }
}