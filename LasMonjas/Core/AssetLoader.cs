using Reactor.Extensions;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace LasMonjas.Core
{
    public static class AssetLoader
    {
        private static readonly Assembly allulCustomBundle = Assembly.GetExecutingAssembly();
        private static readonly Assembly allulCustomLobby = Assembly.GetExecutingAssembly();
        private static readonly Assembly allulCustomMusic = Assembly.GetExecutingAssembly();
        private static readonly Assembly allulCustomMap = Assembly.GetExecutingAssembly();
        private static readonly Assembly allulCustomGamemodeMusic = Assembly.GetExecutingAssembly();

        private static AssetBundle AssetBundleHats;
        private static AssetBundle AssetBundleNamePlates;
        private static AssetBundle AssetBundleVisors;

        public static void LoadAssets() {

            // Custom Bundle Assets
            var resourceStreamBundle = allulCustomBundle.GetManifestResourceStream("LasMonjas.Images.MyAssets.allulcustombundle");
            var assetBundleBundle = AssetBundle.LoadFromMemory(resourceStreamBundle.ReadFully());

            CustomMain.customAssets.bombermanBomb = assetBundleBundle.LoadAsset<GameObject>("Bomb.prefab").DontUnload();
            CustomMain.customAssets.bombermanPlaceBombClip = assetBundleBundle.LoadAsset<AudioClip>("bombermanPlaceBombClip_Switch2.ogg").DontUnload();
            CustomMain.customAssets.bombermanBombClip = assetBundleBundle.LoadAsset<AudioClip>("bombermanBombClip_Explosion2.ogg").DontUnload();
            CustomMain.customAssets.medusaPetrify = assetBundleBundle.LoadAsset<AudioClip>("medusaPretrify_Earth2.ogg").DontUnload();
            CustomMain.customAssets.hypnotistReverse = assetBundleBundle.LoadAsset<GameObject>("hypnotistReverse.prefab").DontUnload();
            CustomMain.customAssets.archerBowClip = assetBundleBundle.LoadAsset<AudioClip>("archer_Bow3.ogg").DontUnload();
            CustomMain.customAssets.renegadeRecruitMinionClip = assetBundleBundle.LoadAsset<AudioClip>("renegadeRecruitMinionClip_Sword4.ogg").DontUnload();
            CustomMain.customAssets.trapperMine = assetBundleBundle.LoadAsset<GameObject>("mine.prefab").DontUnload();
            CustomMain.customAssets.trapperStepMineClip = assetBundleBundle.LoadAsset<AudioClip>("trapperStepMineClip_Explosion1.ogg").DontUnload();
            CustomMain.customAssets.trapperTrap = assetBundleBundle.LoadAsset<GameObject>("trap.prefab").DontUnload();
            CustomMain.customAssets.trapperStepTrapClip = assetBundleBundle.LoadAsset<AudioClip>("trapperStepTrapClip_Slash10.ogg").DontUnload();
            CustomMain.customAssets.yinyangerYinyang = assetBundleBundle.LoadAsset<GameObject>("yinyang.prefab").DontUnload();
            CustomMain.customAssets.yinyangerYinyangClip = assetBundleBundle.LoadAsset<AudioClip>("yinyangerYinyangClip_Skill3.ogg").DontUnload();
            CustomMain.customAssets.yinyangerYinyangColisionClip = assetBundleBundle.LoadAsset<AudioClip>("yinyangerYinyangColisionClip_Bell1.ogg").DontUnload();
            CustomMain.customAssets.challengerDuelArena = assetBundleBundle.LoadAsset<GameObject>("challenger_Arena.prefab").DontUnload();
            CustomMain.customAssets.challengerRock = assetBundleBundle.LoadAsset<GameObject>("rockshow.prefab").DontUnload();
            CustomMain.customAssets.challengerPaper = assetBundleBundle.LoadAsset<GameObject>("papershow.prefab").DontUnload();
            CustomMain.customAssets.challengerScissors = assetBundleBundle.LoadAsset<GameObject>("scissorsshow.prefab").DontUnload();
            CustomMain.customAssets.challengerDuelKillClip = assetBundleBundle.LoadAsset<AudioClip>("challengerDuelKillClip_Blow8.ogg").DontUnload();
            CustomMain.customAssets.roleThiefStealRole = assetBundleBundle.LoadAsset<AudioClip>("roleThiefStealRole_Miss.ogg").DontUnload();
            CustomMain.customAssets.pyromaniacIgniteClip = assetBundleBundle.LoadAsset<AudioClip>("pyromaniacIgniteClip_Fire2.ogg").DontUnload();
            CustomMain.customAssets.treasureHunterTreasure = assetBundleBundle.LoadAsset<GameObject>("treasure.prefab").DontUnload();
            CustomMain.customAssets.treasureHunterPlaceTreasure = assetBundleBundle.LoadAsset<AudioClip>("treasureHunterPlaceTreasure_Flash2.ogg").DontUnload();
            CustomMain.customAssets.treasureHunterCollectTreasure = assetBundleBundle.LoadAsset<AudioClip>("treasureHunterCollectTreasure_Item3.ogg").DontUnload();
            CustomMain.customAssets.devourerDingClip = assetBundleBundle.LoadAsset<AudioClip>("devourerDingClip_Bell3.ogg").DontUnload();
            CustomMain.customAssets.devourerDevourClip = assetBundleBundle.LoadAsset<AudioClip>("devourerDevourClip_Slash1.ogg").DontUnload();
            CustomMain.customAssets.poisonerPoisonClip = assetBundleBundle.LoadAsset<AudioClip>("poisonerPoison.ogg").DontUnload();
            CustomMain.customAssets.puppeteerClip = assetBundleBundle.LoadAsset<AudioClip>("puppeteer_Crow.ogg").DontUnload();
            CustomMain.customAssets.timeTravelerTimeReverseClip = assetBundleBundle.LoadAsset<AudioClip>("timeTravelerTimeReverseClip_Magic3.ogg").DontUnload();
            CustomMain.customAssets.squireShieldClip = assetBundleBundle.LoadAsset<AudioClip>("squireShieldClip_Parry.ogg").DontUnload();
            CustomMain.customAssets.fortuneTellerRevealClip = assetBundleBundle.LoadAsset<AudioClip>("fortuneTellerRevealClip_Magic2.ogg").DontUnload(); 
            CustomMain.customAssets.spiritualistRevive = assetBundleBundle.LoadAsset<AudioClip>("spiritualistRevive_Recovery.ogg").DontUnload();
            CustomMain.customAssets.performerDio = assetBundleBundle.LoadAsset<GameObject>("dio.prefab").DontUnload();
            CustomMain.customAssets.jinxQuack = assetBundleBundle.LoadAsset<AudioClip>("jinxQuack_quack.mp3").DontUnload();
            CustomMain.customAssets.accelSprite = assetBundleBundle.LoadAsset<GameObject>("engineer_accelerate.prefab").DontUnload();
            CustomMain.customAssets.decelSprite = assetBundleBundle.LoadAsset<GameObject>("engineer_slow.prefab").DontUnload();
            CustomMain.customAssets.positionSprite = assetBundleBundle.LoadAsset<GameObject>("engineer_detect.prefab").DontUnload();

            // Capture the flag
            CustomMain.customAssets.redflag = assetBundleBundle.LoadAsset<GameObject>("redFlag.prefab").DontUnload();
            CustomMain.customAssets.redflagbase = assetBundleBundle.LoadAsset<GameObject>("redFlagBase.prefab").DontUnload();
            CustomMain.customAssets.blueflag = assetBundleBundle.LoadAsset<GameObject>("blueFlag.prefab").DontUnload();
            CustomMain.customAssets.blueflagbase = assetBundleBundle.LoadAsset<GameObject>("blueFlagBase.prefab").DontUnload();
            CustomMain.customAssets.redfloor = assetBundleBundle.LoadAsset<GameObject>("redfloorbase.prefab").DontUnload();
            CustomMain.customAssets.bluefloor = assetBundleBundle.LoadAsset<GameObject>("bluefloorbase.prefab").DontUnload();

            // Police and Thief
            CustomMain.customAssets.cell = assetBundleBundle.LoadAsset<GameObject>("Cell.prefab").DontUnload();
            CustomMain.customAssets.jewelbutton = assetBundleBundle.LoadAsset<GameObject>("deliverjewel_floor.prefab").DontUnload();
            CustomMain.customAssets.freethiefbutton = assetBundleBundle.LoadAsset<GameObject>("cell_button.prefab").DontUnload();
            CustomMain.customAssets.jeweldiamond = assetBundleBundle.LoadAsset<GameObject>("jewel_diamond.prefab").DontUnload();
            CustomMain.customAssets.jewelruby = assetBundleBundle.LoadAsset<GameObject>("jewel_ruby.prefab").DontUnload();
            CustomMain.customAssets.thiefspaceship = assetBundleBundle.LoadAsset<GameObject>("thief_spaceship.prefab").DontUnload();
            CustomMain.customAssets.thiefspaceshiphatch = assetBundleBundle.LoadAsset<GameObject>("thief_spaceship_hatch.prefab").DontUnload();
            CustomMain.customAssets.policeParalyze = assetBundleBundle.LoadAsset<GameObject>("Tased.prefab").DontUnload();
            CustomMain.customAssets.policeTaser = assetBundleBundle.LoadAsset<AudioClip>("policeandThiefsTase_Paralyze3.ogg").DontUnload();

            // King of the hill
            CustomMain.customAssets.whiteflag = assetBundleBundle.LoadAsset<GameObject>("whiteFlag.prefab").DontUnload();
            CustomMain.customAssets.greenflag = assetBundleBundle.LoadAsset<GameObject>("greenFlag.prefab").DontUnload();
            CustomMain.customAssets.yellowflag = assetBundleBundle.LoadAsset<GameObject>("yellowFlag.prefab").DontUnload();
            CustomMain.customAssets.whitebase = assetBundleBundle.LoadAsset<GameObject>("whitebase.prefab").DontUnload();
            CustomMain.customAssets.greenbase = assetBundleBundle.LoadAsset<GameObject>("greenbase.prefab").DontUnload();
            CustomMain.customAssets.yellowbase = assetBundleBundle.LoadAsset<GameObject>("yellowbase.prefab").DontUnload();
            CustomMain.customAssets.greenaura = assetBundleBundle.LoadAsset<GameObject>("greenkingaura.prefab").DontUnload();
            CustomMain.customAssets.yellowaura = assetBundleBundle.LoadAsset<GameObject>("yellowkingaura.prefab").DontUnload();
            CustomMain.customAssets.greenfloor = assetBundleBundle.LoadAsset<GameObject>("greenfloorbase.prefab").DontUnload();
            CustomMain.customAssets.yellowfloor = assetBundleBundle.LoadAsset<GameObject>("yellowfloorbase.prefab").DontUnload();

            // Hot Potato
            CustomMain.customAssets.hotPotato = assetBundleBundle.LoadAsset<GameObject>("Hot_Potato.prefab").DontUnload();

            // ZombieLaboratory
            CustomMain.customAssets.laboratory = assetBundleBundle.LoadAsset<GameObject>("Laboratory.prefab").DontUnload();
            CustomMain.customAssets.keyItem01 = assetBundleBundle.LoadAsset<GameObject>("keyItem01.prefab").DontUnload();
            CustomMain.customAssets.keyItem02 = assetBundleBundle.LoadAsset<GameObject>("keyItem02.prefab").DontUnload();
            CustomMain.customAssets.keyItem03 = assetBundleBundle.LoadAsset<GameObject>("keyItem03.prefab").DontUnload();
            CustomMain.customAssets.keyItem04 = assetBundleBundle.LoadAsset<GameObject>("keyItem04.prefab").DontUnload();
            CustomMain.customAssets.keyItem05 = assetBundleBundle.LoadAsset<GameObject>("keyItem05.prefab").DontUnload();
            CustomMain.customAssets.keyItem06 = assetBundleBundle.LoadAsset<GameObject>("keyItem06.prefab").DontUnload();
            CustomMain.customAssets.susBox = assetBundleBundle.LoadAsset<GameObject>("susBox.prefab").DontUnload();
            CustomMain.customAssets.emptyBox = assetBundleBundle.LoadAsset<GameObject>("emptyBox.prefab").DontUnload();
            CustomMain.customAssets.ammoBox = assetBundleBundle.LoadAsset<GameObject>("ammoBox.prefab").DontUnload();
            CustomMain.customAssets.rechargeAmmoClip = assetBundleBundle.LoadAsset<AudioClip>("zombieLaboratoryRecharge_Equip1.ogg").DontUnload();
            CustomMain.customAssets.nurseMedKit = assetBundleBundle.LoadAsset<GameObject>("nurseMedKit.prefab").DontUnload();
            CustomMain.customAssets.mapMedKit = assetBundleBundle.LoadAsset<GameObject>("medKit.prefab").DontUnload();
            
            // Custom Lobby Assets
            var resourceStreamLobby = allulCustomLobby.GetManifestResourceStream("LasMonjas.Images.MyAssets.allulcustomlobby");
            var assetBundleLobby = AssetBundle.LoadFromMemory(resourceStreamLobby.ReadFully());

            CustomMain.customAssets.customLobby = assetBundleLobby.LoadAsset<GameObject>("allul_customLobby.prefab").DontDestroy();
            CustomMain.customAssets.allulfitti = assetBundleLobby.LoadAsset<GameObject>("Allulfitti.prefab").DontDestroy();

            // Custom Map Assets
            var resourceStreamMap = allulCustomMap.GetManifestResourceStream("LasMonjas.Images.MyAssets.allulcustommap");
            var assetBundleMap = AssetBundle.LoadFromMemory(resourceStreamMap.ReadFully());

            CustomMain.customAssets.customMap = assetBundleMap.LoadAsset<GameObject>("HalconUI.prefab").DontUnload();
            CustomMain.customAssets.customMinimap = assetBundleMap.LoadAsset<GameObject>("Minimap.prefab").DontUnload();
            CustomMain.customAssets.customComms = assetBundleMap.LoadAsset<GameObject>("new_comms.prefab").DontUnload();

            // Custom Music Assets
            var resourceStream = allulCustomMusic.GetManifestResourceStream("LasMonjas.Images.MyAssets.allulcustommusic");
            var assetBundleMusic = AssetBundle.LoadFromMemory(resourceStream.ReadFully());

            CustomMain.customAssets.bombermanBombMusic = assetBundleMusic.LoadAsset<AudioClip>("BombermanTheme_Run Amok.mp3").DontUnload();
            CustomMain.customAssets.challengerDuelMusic = assetBundleMusic.LoadAsset<AudioClip>("ChallengerTheme_FutureGladiator.mp3").DontUnload();
            CustomMain.customAssets.performerMusic = assetBundleMusic.LoadAsset<AudioClip>("PerformerTheme_Spazzmatica Polka.mp3").DontUnload();           
            CustomMain.customAssets.lobbyMusic = assetBundleMusic.LoadAsset<AudioClip>("Lobby_Hyperfun.mp3").DontUnload();
            CustomMain.customAssets.tasksCalmMusic = assetBundleMusic.LoadAsset<AudioClip>("TasksCalm_Sneaky Adventure.mp3").DontUnload();
            CustomMain.customAssets.tasksCoreMusic = assetBundleMusic.LoadAsset<AudioClip>("TasksCore_Investigations.mp3").DontUnload();
            CustomMain.customAssets.tasksFinalMusic = assetBundleMusic.LoadAsset<AudioClip>("TasksFinal_Hidden Agenda.mp3").DontUnload(); 
            CustomMain.customAssets.meetingCalmMusic = assetBundleMusic.LoadAsset<AudioClip>("MeetingCalm_Fluffing a Duck.mp3").DontUnload();
            CustomMain.customAssets.meetingCoreMusic = assetBundleMusic.LoadAsset<AudioClip>("MeetingCore_Local Forecast.mp3").DontUnload();
            CustomMain.customAssets.meetingFinalMusic = assetBundleMusic.LoadAsset<AudioClip>("MeetingFinal_Heavy Interlude.mp3").DontUnload();            
            CustomMain.customAssets.winCrewmatesMusic = assetBundleMusic.LoadAsset<AudioClip>("WinCremates_Take a Chance.mp3").DontUnload();
            CustomMain.customAssets.winImpostorsMusic = assetBundleMusic.LoadAsset<AudioClip>("WinImpostors_Who Likes to Party.mp3").DontUnload();
            CustomMain.customAssets.winNeutralsMusic = assetBundleMusic.LoadAsset<AudioClip>("WinNeutrals_Mistake the Getaway.mp3").DontUnload();
            CustomMain.customAssets.winRebelsMusic = assetBundleMusic.LoadAsset<AudioClip>("WinRebels_Danse Macabre - Low Strings Finale.mp3").DontUnload();

            // Custom Gamemode Music Assets
            var resourceGamemodeMusicStream = allulCustomGamemodeMusic.GetManifestResourceStream("LasMonjas.Images.MyAssets.allulcustomgamemodemusic");
            var assetBundleGamemodeMusic = AssetBundle.LoadFromMemory(resourceGamemodeMusicStream.ReadFully());

            CustomMain.customAssets.captureTheFlagMusic = assetBundleGamemodeMusic.LoadAsset<AudioClip>("CaptureTheFlagMusic_BeachfrontCelebration.mp3").DontUnload();
            CustomMain.customAssets.policeAndThiefMusic = assetBundleGamemodeMusic.LoadAsset<AudioClip>("PoliceAndThief_Unity.mp3").DontUnload();
            CustomMain.customAssets.kingOfTheHillMusic = assetBundleGamemodeMusic.LoadAsset<AudioClip>("KingOfTheHill_Bama Country.mp3").DontUnload();
            CustomMain.customAssets.hotPotatoMusic = assetBundleGamemodeMusic.LoadAsset<AudioClip>("HotPotato_Batty McFaddin.mp3").DontUnload();
            CustomMain.customAssets.zombieLaboratoryMusic = assetBundleGamemodeMusic.LoadAsset<AudioClip>("ZombieLaboratoryMusic_Anachronist.mp3").DontUnload();

            // Custom Bundle Hat Assets
            byte[] bundleHatRead = Assembly.GetCallingAssembly().GetManifestResourceStream("LasMonjas.Images.MyAssets.allulcustomhats").ReadFully();
            AssetBundleHats = AssetBundle.LoadFromMemory(bundleHatRead);

            // Custom Bundle Nameplates Assets
            byte[] bundleNamePlateRead = Assembly.GetCallingAssembly().GetManifestResourceStream("LasMonjas.Images.MyAssets.allulcustomnameplates").ReadFully();
            AssetBundleNamePlates = AssetBundle.LoadFromMemory(bundleNamePlateRead);

            // Custom Bundle Visors Assets
            byte[] bundleVisorsRead = Assembly.GetCallingAssembly().GetManifestResourceStream("LasMonjas.Images.MyAssets.allulcustomvisors").ReadFully();
            AssetBundleVisors = AssetBundle.LoadFromMemory(bundleVisorsRead);

            assetBundleBundle.Unload(false);
            assetBundleLobby.Unload(false);
            assetBundleMap.Unload(false);
            assetBundleMusic.Unload(false);
            assetBundleGamemodeMusic.Unload(false);
        }

        public static UnityEngine.Object LoadHatAsset(string name)
            => AssetBundleHats.LoadAsset(name);
        public static UnityEngine.Object LoadNamePlateAsset(string name)
             => AssetBundleNamePlates.LoadAsset(name);
        public static UnityEngine.Object LoadVisorsAsset(string name)
              => AssetBundleVisors.LoadAsset(name);
    }
}