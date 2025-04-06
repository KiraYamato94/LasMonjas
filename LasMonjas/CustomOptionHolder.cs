using UnityEngine;
using LasMonjas.Core;
using Types = LasMonjas.Core.CustomOption.CustomOptionType;

namespace LasMonjas
{
    public class CustomOptionHolder {
        public static string[] rates = new string[]{"0%", "100%"}; 
        public static string[] presets = new string[]{"Roles", "Gamemodes", "Minigame Roles", "Preset 4", "Preset 5" };

        // Presets
        public static CustomOption presetSelection;

        // Game Type
        public static CustomOption gameType;

        // Global Settings
        public static CustomOption globalSettings;
        public static CustomOption activateSenseiMap;
        public static CustomOption hideVentAnimOnShadows;
        public static CustomOption activateDleksMap;

        // Roles Settings
        public static CustomOption rolesSettings;
        public static CustomOption removeSwipeCard;
        public static CustomOption nightVisionLightSabotage;
        public static CustomOption screenShakeReactorSabotage;
        public static CustomOption anonymousCommsSabotage;
        public static CustomOption slowSpeedOxigenSabotage;

        // Gamemode Settings
        public static CustomOption gamemodeSettings;
        public static CustomOption gamemodeMatchDuration;
        public static CustomOption gamemodeKillCooldown;
        public static CustomOption gamemodeEnableFlashlight;
        public static CustomOption gamemodeFlashlightRange;
        public static CustomOption gamemodeReviveTime;
        public static CustomOption gamemodeInvincibilityTimeAfterRevive;

        // Gamemode Individual Settings
        public static CustomOption gamemodeIndividualSettings;
        // Capture the flag
        public static CustomOption requiredFlags;
        // Police And Thief
        public static CustomOption thiefModerequiredJewels;
        public static CustomOption thiefModePoliceTaseCooldown;
        public static CustomOption thiefModePoliceTaseDuration;
        public static CustomOption thiefModePoliceCanSeeJewels;
        public static CustomOption thiefModePoliceCatchCooldown;
        public static CustomOption thiefModecaptureThiefTime;
        public static CustomOption thiefModeWhoCanThiefsKill;
        // King of the hill
        public static CustomOption kingRequiredPoints;
        public static CustomOption kingCaptureCooldown;
        // Hot Potato
        public static CustomOption hotPotatoTransferLimit;
        public static CustomOption hotPotatoCooldown;
        public static CustomOption hotPotatoResetTimeForTransfer;
        public static CustomOption hotPotatoIncreaseTimeIfNoReset;
        // ZombieLaboratory
        public static CustomOption zombieLaboratoryStartZombies;
        public static CustomOption zombieLaboratoryInfectCooldown;
        public static CustomOption zombieLaboratoryInfectTime;
        public static CustomOption zombieLaboratoryMaxTimeForHeal;
        public static CustomOption zombieLaboratorySearchBoxTimer;
        // BattleRoyale
        public static CustomOption battleRoyaleMatchType;
        public static CustomOption battleRoyaleKillCooldown;
        public static CustomOption battleRoyaleLifes;
        public static CustomOption battleRoyaleScoreNeeded;


        // Impostors configurable options

        // Mimic
        public static CustomOption mimicSpawnRate;
        public static CustomOption mimicDuration;

        // Painter
        public static CustomOption painterSpawnRate;
        public static CustomOption painterCooldown;
        public static CustomOption painterDuration;

        // Demon
        public static CustomOption demonSpawnRate;
        public static CustomOption demonKillDelay;
        public static CustomOption demonCanKillNearNuns;

        // Janitor
        public static CustomOption janitorSpawnRate;
        public static CustomOption janitorCooldown;

        // Illusionist
        public static CustomOption illusionistSpawnRate;
        public static CustomOption illusionistPlaceHatCooldown;
        public static CustomOption illusionistLightsOutCooldown;
        public static CustomOption illusionistLightsOutDuration;

        // Manipulator
        public static CustomOption manipulatorSpawnRate;

        // Bomberman
        public static CustomOption bombermanSpawnRate;
        public static CustomOption bombermanBombCooldown;
        public static CustomOption bombermanSelfBombDuration;

        // Chameleon
        public static CustomOption chameleonSpawnRate;
        public static CustomOption chameleonCooldown;
        public static CustomOption chameleonDuration;

        // Gambler
        public static CustomOption gamblerSpawnRate;
        public static CustomOption gamblerCanCallEmergency;
        public static CustomOption gamblerCanKillThroughShield;

        // Sorcerer
        public static CustomOption sorcererSpawnRate;
        public static CustomOption sorcererCooldown;
        public static CustomOption sorcererSpellDuration;
        public static CustomOption sorcererCanCallEmergency;

        // Medusa
        public static CustomOption medusaSpawnRate;
        public static CustomOption medusaCooldown;
        public static CustomOption medusaDelay;

        // Hypnotist
        public static CustomOption hypnotistSpawnRate;
        public static CustomOption hypnotistCooldown;
        public static CustomOption hypnotistNumberOfSpirals;
        public static CustomOption hypnotistSpiralsDuration;

        // Archer
        public static CustomOption archerSpawnRate;
        public static CustomOption archerShotRange;
        public static CustomOption archerNoticeRange;
        public static CustomOption archerAimAssistDuration;

        // Plumber
        public static CustomOption plumberSpawnRate;
        public static CustomOption plumberCooldown;

        public static CustomOption librarianSpawnRate;
        public static CustomOption librarianCooldown;


        // Rebels configurable options

        // Renegade & Minion
        public static CustomOption renegadeSpawnRate;
        public static CustomOption renegadeCanUseVents;
        public static CustomOption renegadeCanRecruitMinion;

        // Bountyhunter
        public static CustomOption bountyHunterSpawnRate;

        // Trapper
        public static CustomOption trapperSpawnRate;
        public static CustomOption trapperCooldown;
        public static CustomOption trapperMineNumber;
        public static CustomOption trapperMineDuration; 
        public static CustomOption trapperTrapNumber;
        public static CustomOption trapperTrapDuration;

        // Yinyanger
        public static CustomOption yinyangerSpawnRate;
        public static CustomOption yinyangerCooldown;

        // Challenger
        public static CustomOption challengerSpawnRate;
        public static CustomOption challengerCooldown;
        public static CustomOption challengerKillsForWin;

        // Ninja
        public static CustomOption ninjaSpawnRate;

        // Berserker
        public static CustomOption berserkerSpawnRate;
        public static CustomOption berserkerTimeToKill;

        // Yandere
        public static CustomOption yandereSpawnRate;
        public static CustomOption yandereCooldown;
        public static CustomOption yandereStareTimes;
        public static CustomOption yandereStareDuration;

        // Stranded
        public static CustomOption strandedSpawnRate;

        // Monja
        public static CustomOption monjaSpawnRate;


        // Neutral configurable options

        // Joker
        public static CustomOption jokerSpawnRate;
        public static CustomOption jokerCanSabotage;

        // Role Thief
        public static CustomOption rolethiefSpawnRate;
        public static CustomOption rolethiefCooldown;

        // Pyromaniac
        public static CustomOption pyromaniacSpawnRate;
        public static CustomOption pyromaniacCooldown;
        public static CustomOption pyromaniacDuration;

        // Treasure Hunter
        public static CustomOption treasureHunterSpawnRate;
        public static CustomOption treasureHunterTreasureNumber;

        // Devourer
        public static CustomOption devourerSpawnRate;
        public static CustomOption devourerBodiesNumber;

        // Poisoner
        public static CustomOption poisonerSpawnRate;
        public static CustomOption poisonerInfectRange;
        public static CustomOption poisonerInfectDuration;
        public static CustomOption poisonerCanSabotage;

        // Puppeteer
        public static CustomOption puppeteerSpawnRate;
        public static CustomOption puppeteerNumberOfKills;

        // Exiler
        public static CustomOption exilerSpawnRate;

        // Amnesiac
        public static CustomOption amnesiacSpawnRate;

        // Seeker
        public static CustomOption seekerSpawnRate;
        public static CustomOption seekerCooldown;
        public static CustomOption seekerPointsNumber;


        // Crewmates configurable options

        // Captain
        public static CustomOption captainSpawnRate;
        public static CustomOption captainCanSpecialVoteOneTime;

        // Mechanic
        public static CustomOption mechanicSpawnRate;
        public static CustomOption mechanicNumberOfRepairs;
        public static CustomOption mechanicRechargeTasksNumber; 
        public static CustomOption mechanicExpertRepairs;

        // Sheriff
        public static CustomOption sheriffSpawnRate;
        public static CustomOption sheriffCanKillNeutrals;

        // Detective
        public static CustomOption detectiveSpawnRate;
        public static CustomOption detectiveShowFootprints;
        public static CustomOption detectiveCooldown;
        public static CustomOption detectiveShowFootPrintDuration;
        public static CustomOption detectiveAnonymousFootprints;

        // Forensic
        public static CustomOption forensicSpawnRate;
        public static CustomOption forensicReportNameDuration;
        public static CustomOption forensicReportColorDuration;
        public static CustomOption forensicReportClueDuration;
        public static CustomOption forensicDuration;
        public static CustomOption forensicOneTimeUse;

        // TimeTraveler
        public static CustomOption timeTravelerSpawnRate;
        public static CustomOption timeTravelerCooldown;
        public static CustomOption timeTravelerStopTime;

        // Squire
        public static CustomOption squireSpawnRate;
        public static CustomOption squireShowShielded;
        public static CustomOption squireShowAttemptToShielded;
        public static CustomOption squireResetShieldAfterMeeting;

        // Cheater
        public static CustomOption cheaterSpawnRate;
        public static CustomOption cheaterCanCallEmergency;
        public static CustomOption cheatercanOnlyCheatOthers;

        // FortuneTeller
        public static CustomOption fortuneTellerSpawnRate;
        public static CustomOption fortuneTellerCooldown;
        public static CustomOption fortuneTellerDuration; 
        public static CustomOption fortuneTellerNumberOfSee;
        public static CustomOption fortuneTellerRechargeTasksNumber;
        public static CustomOption fortuneTellerKindOfInfo;
        public static CustomOption fortuneTellerPlayersWithNotification;
        public static CustomOption fortuneTellerCanCallEmergency;

        // Hacker
        public static CustomOption hackerSpawnRate;
        public static CustomOption hackerCooldown;
        public static CustomOption hackerHackeringDuration;
        public static CustomOption hackerToolsNumber;
        public static CustomOption hackerRechargeTasksNumber;

        // Sleuth
        public static CustomOption sleuthSpawnRate;
        public static CustomOption sleuthUpdateIntervall;
        public static CustomOption sleuthResetTargetAfterMeeting;
        public static CustomOption sleuthCorpsesPathfindCooldown;
        public static CustomOption sleuthCorpsesPathfindDuration;
        public static CustomOption sleuthDuration;

        // Fink
        public static CustomOption finkSpawnRate;
        public static CustomOption finkLeftTasksForImpostors;
        public static CustomOption finkCooldown;
        public static CustomOption finkDuration;

        // Kid
        public static CustomOption kidSpawnRate;

        // Welder
        public static CustomOption welderSpawnRate;
        public static CustomOption welderCooldown;
        public static CustomOption welderTotalWelds;

        // Spiritualist
        public static CustomOption spiritualistSpawnRate;

        // Vigilant
        public static CustomOption vigilantSpawnRate;
        public static CustomOption vigilantCooldown;
        public static CustomOption vigilantCamDuration;
        public static CustomOption vigilantCamMaxCharges;
        public static CustomOption vigilantCamRechargeTasksNumber;

        // Hunter
        public static CustomOption hunterSpawnRate;
        public static CustomOption hunterResetTargetAfterMeeting;

        // Jinx
        public static CustomOption jinxSpawnRate;
        public static CustomOption jinxCooldown;
        public static CustomOption jinxJinxsNumber;

        // Coward
        public static CustomOption cowardSpawnRate;
        public static CustomOption cowardNumberOfCalls;
        public static CustomOption cowardRechargeTasksNumber;

        // Bat
        public static CustomOption batSpawnRate;
        public static CustomOption batCooldown;
        public static CustomOption batDuration;
        public static CustomOption batRange;

        // Necromancer
        public static CustomOption necromancerSpawnRate;
        public static CustomOption necromancerReviveCooldown;
        public static CustomOption necromancerReviveTimer;
        public static CustomOption necromancerMaxReviveRoomDistance;

        // Engineer
        public static CustomOption engineerSpawnRate;
        public static CustomOption engineerCooldown;
        public static CustomOption engineerNumberOfTraps;
        public static CustomOption engineerTrapsDuration;
        public static CustomOption engineerAccelTrapIncrease;
        public static CustomOption engineerDecelTrapDecrease;

        // Locksmith
        public static CustomOption locksmithSpawnRate;
        public static CustomOption locksmithCooldown;

        // TimeMaster
        public static CustomOption taskMasterSpawnRate;
        public static CustomOption taskMasterCooldown;
        public static CustomOption taskMasterDuration; 
        public static CustomOption taskMasterExtraCommonTasks;
        public static CustomOption taskMasterExtraShortTasks;
        public static CustomOption taskMasterExtraLongTasks;
        public static CustomOption taskMasterRewardType;

        // Jailer
        public static CustomOption jailerSpawnRate;
        public static CustomOption jailerCooldown;
        public static CustomOption jailerDuration;

        // Modifiers
        public static CustomOption loverPlayer;
        public static CustomOption lighterPlayer;
        public static CustomOption blindPlayer;
        public static CustomOption flashPlayer;
        public static CustomOption bigchungusPlayer;
        public static CustomOption theChosenOnePlayer;
        public static CustomOption theChosenOneReportDelay;
        public static CustomOption performerPlayer;
        public static CustomOption performerDuration;
        public static CustomOption proPlayer;
        public static CustomOption paintballPlayer;
        public static CustomOption paintballDuration;
        public static CustomOption electricianPlayer;
        public static CustomOption electricianDuration;

        public static string cs(Color c, string s) {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
 
        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static void Load() {

            // Game Options
            presetSelection = CustomOption.Create(0, Types.General, cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Preset"), presets, null, true);

            // Game Type
            gameType = CustomOption.Create(1, Types.General, cs(TaskMaster.color, "Game Type"), new string[] { "Roles", "Find a Role", "Capture the Flag", "Police and Thieves", "King of the Hill", "Hot Potato", "Zombie Laboratory", "Battle Royale", "Monja Festival" }, null, true);

            // Global Settings
            globalSettings = CustomOption.Create(2, Types.General, cs(Jailer.color, "Global Settings"), false, null, true);
            activateSenseiMap = CustomOption.Create(3, Types.General, cs(Jailer.color, "Activate Custom Skeld Map"), false, globalSettings);
            hideVentAnimOnShadows = CustomOption.Create(4, Types.General, cs(Jailer.color, "Hide Vent Anim on Shadows"), false, globalSettings);
            activateDleksMap = CustomOption.Create(5, Types.General, cs(Jailer.color, "Activate Dleks Map"), false, globalSettings);

            // Roles Settings
            rolesSettings = CustomOption.Create(10, Types.General, cs(Locksmith.color, "Roles Settings"), false, null, true);
            removeSwipeCard = CustomOption.Create(11, Types.General, cs(Locksmith.color, "Remove Swipe Card Task"), false, rolesSettings);
            nightVisionLightSabotage = CustomOption.Create(12, Types.General, cs(Locksmith.color, "Night vision for lights sabotage"), false, rolesSettings);
            screenShakeReactorSabotage = CustomOption.Create(13, Types.General, cs(Locksmith.color, "Screen shake for reactor sabotage"), false, rolesSettings);
            anonymousCommsSabotage = CustomOption.Create(14, Types.General, cs(Locksmith.color, "Anonymous players for comms sabotage"), false, rolesSettings);
            slowSpeedOxigenSabotage = CustomOption.Create(15, Types.General, cs(Locksmith.color, "Decrease speed for oxygen sabotage"), false, rolesSettings);

            // Gamemode Settings
            gamemodeSettings = CustomOption.Create(20, Types.General, cs(Sheriff.color, "Gamemode Global Settings"), false, null, true);
            gamemodeMatchDuration = CustomOption.Create(21, Types.General, cs(Sheriff.color, "Match Duration"), 180f, 180f, 420f, 30f, gamemodeSettings);
            gamemodeKillCooldown = CustomOption.Create(22, Types.General, cs(Sheriff.color, "Kill Cooldown"), 10f, 10f, 20f, 1f, gamemodeSettings);
            gamemodeEnableFlashlight = CustomOption.Create(23, Types.General, cs(Sheriff.color, "Enable flashlight if possible"), true, gamemodeSettings);
            gamemodeFlashlightRange = CustomOption.Create(24, Types.General, cs(Sheriff.color, "Flashlight range"), 0.8f, 0.6f, 1.2f, 0.2f, gamemodeSettings);
            gamemodeReviveTime = CustomOption.Create(25, Types.General, cs(Sheriff.color, "Revive Wait Time"), 8f, 7f, 15f, 1f, gamemodeSettings);
            gamemodeInvincibilityTimeAfterRevive = CustomOption.Create(26, Types.General, cs(Sheriff.color, "Invincibility Time After Revive"), 3f, 2f, 5f, 1f, gamemodeSettings);

            // Gamemode Individual Settings
            gamemodeIndividualSettings = CustomOption.Create(30, Types.General, cs(Sheriff.color, "Gamemode Individual Settings"), false, null, true);
            // Capture the flag mode
            requiredFlags = CustomOption.Create(31, Types.General, cs(Sheriff.color, "Capture the Flag") + ": Score Number", 3f, 3f, 5f, 1f, gamemodeIndividualSettings);
            // Police and Thief mode
            thiefModerequiredJewels = CustomOption.Create(41, Types.General, cs(Coward.color, "Police and Thieves") + ": Jewel Number", 15f, 8f, 15f, 1f, gamemodeIndividualSettings);
            thiefModePoliceCatchCooldown = CustomOption.Create(42, Types.General, cs(Coward.color, "Police and Thieves") + ": Arrest Cooldown", 10f, 5f, 15f, 1f, gamemodeIndividualSettings);
            thiefModecaptureThiefTime = CustomOption.Create(43, Types.General, cs(Coward.color, "Police and Thieves") + ": Time to Arrest", 3f, 2f, 3f, 1f, gamemodeIndividualSettings);
            thiefModePoliceTaseCooldown = CustomOption.Create(44, Types.General, cs(Coward.color, "Police and Thieves") + ": Tase Cooldown", 15f, 10f, 15f, 1f, gamemodeIndividualSettings);
            thiefModePoliceTaseDuration = CustomOption.Create(45, Types.General, cs(Coward.color, "Police and Thieves") + ": Tase Duration", 3f, 3f, 5f, 1f, gamemodeIndividualSettings);
            thiefModePoliceCanSeeJewels = CustomOption.Create(46, Types.General, cs(Coward.color, "Police and Thieves") + ": Police can see Jewels", false, gamemodeIndividualSettings);
            thiefModeWhoCanThiefsKill = CustomOption.Create(47, Types.General, cs(Coward.color, "Police and Thieves") + ": Who Can Thieves Kill", new string[] { "Taser", "All", "Nobody" }, gamemodeIndividualSettings);
            // King of the hill mode
            kingRequiredPoints = CustomOption.Create(51, Types.General, cs(Squire.color, "King of the Hill") + ": Score Number", 200f, 100f, 300f, 10f, gamemodeIndividualSettings);
            kingCaptureCooldown = CustomOption.Create(52, Types.General, cs(Squire.color, "King of the Hill") + ": Capture Cooldown", 10f, 5f, 15f, 1f, gamemodeIndividualSettings);
            // Hot Potato
            hotPotatoTransferLimit = CustomOption.Create(61, Types.General, cs(Locksmith.color, "Hot Potato") + ": Time Limit for Transfer", 20f, 10f, 30f, 1f, gamemodeIndividualSettings);
            hotPotatoCooldown = CustomOption.Create(62, Types.General, cs(Locksmith.color, "Hot Potato") + ": Transfer Cooldown", 5f, 5f, 10f, 1f, gamemodeIndividualSettings);
            hotPotatoResetTimeForTransfer = CustomOption.Create(63, Types.General, cs(Locksmith.color, "Hot Potato") + ": Reset timer after Transfer", true, gamemodeIndividualSettings);
            hotPotatoIncreaseTimeIfNoReset = CustomOption.Create(64, Types.General, cs(Locksmith.color, "Hot Potato") + ": Extra Time when timer doesn't reset", 10f, 10f, 15f, 1f, gamemodeIndividualSettings);
            // ZombieLaboratory
            zombieLaboratoryStartZombies = CustomOption.Create(71, Types.General, cs(Hunter.color, "Zombie Laboratory") + ": Initial Zombies", 1f, 1f, 5f, 1f, gamemodeIndividualSettings);
            zombieLaboratoryInfectTime = CustomOption.Create(72, Types.General, cs(Hunter.color, "Zombie Laboratory") + ": Time to Infect", 3f, 2f, 3f, 1f, gamemodeIndividualSettings);
            zombieLaboratoryInfectCooldown = CustomOption.Create(73, Types.General, cs(Hunter.color, "Zombie Laboratory") + ": Infect Cooldown", 10f, 10f, 20f, 1f, gamemodeIndividualSettings);
            zombieLaboratorySearchBoxTimer = CustomOption.Create(74, Types.General, cs(Hunter.color, "Zombie Laboratory") + ": Search Box Timer", 3f, 2f, 3f, 1f, gamemodeIndividualSettings);
            zombieLaboratoryMaxTimeForHeal = CustomOption.Create(75, Types.General, cs(Hunter.color, "Zombie Laboratory") + ": Time to use Medkit", 45f, 30f, 90f, 5f, gamemodeIndividualSettings);
            // Battle Royale
            battleRoyaleMatchType = CustomOption.Create(81, Types.General, cs(Sleuth.color, "Battle Royale") + ": Match Type", new string[] { "All vs All", "Team Battle", "Score Battle" }, gamemodeIndividualSettings);
            battleRoyaleKillCooldown = CustomOption.Create(82, Types.General, cs(Sleuth.color, "Battle Royale") + ": Shoot Cooldown", 1f, 1f, 3f, 1f, gamemodeIndividualSettings);
            battleRoyaleLifes = CustomOption.Create(83, Types.General, cs(Sleuth.color, "Battle Royale") + ": Fighter Lifes", 3f, 3f, 10f, 1f, gamemodeIndividualSettings);
            battleRoyaleScoreNeeded = CustomOption.Create(84, Types.General, cs(Sleuth.color, "Battle Royale") + ": Score Number", 200f, 100f, 300f, 10f, gamemodeIndividualSettings);

            // Monja Festival, reserved 90-99

            // Mimic options
            mimicSpawnRate = CustomOption.Create(150, Types.Impostor, cs(Mimic.color, "Mimic"),  rates, null, true);
            mimicDuration = CustomOption.Create(151, Types.Impostor, cs(Mimic.color, "Mimic") + ": Duration",  10f, 10f, 15f, 1f, mimicSpawnRate);

            // Painter options
            painterSpawnRate = CustomOption.Create(160, Types.Impostor, cs(Painter.color, "Painter"),  rates, null, true);
            painterCooldown = CustomOption.Create(161, Types.Impostor, cs(Painter.color, "Painter") + ": Cooldown",  30f, 20f, 40f, 2.5f, painterSpawnRate);
            painterDuration = CustomOption.Create(162, Types.Impostor, cs(Painter.color, "Painter") + ": Duration",  10f, 10f, 15f, 1f, painterSpawnRate);

            // Demon options
            demonSpawnRate = CustomOption.Create(170, Types.Impostor, cs(Demon.color, "Demon"),  rates, null, true);
            demonKillDelay = CustomOption.Create(171, Types.Impostor, cs(Demon.color, "Demon") + ": Delay Time",  10f, 5f, 15f, 1f, demonSpawnRate);
            demonCanKillNearNuns = CustomOption.Create(172, Types.Impostor, cs(Demon.color, "Demon") + ": Can Kill near Nuns",  false, demonSpawnRate);

            // Janitor options
            janitorSpawnRate = CustomOption.Create(180, Types.Impostor, cs(Janitor.color, "Janitor"),  rates, null, true);
            janitorCooldown = CustomOption.Create(181, Types.Impostor, cs(Janitor.color, "Janitor") + ": Cooldown",  30f, 20f, 40f, 2.5f, janitorSpawnRate);

            // Illusionist options
            illusionistSpawnRate = CustomOption.Create(190, Types.Impostor, cs(Illusionist.color, "Illusionist"),  rates, null, true);
            illusionistPlaceHatCooldown = CustomOption.Create(191, Types.Impostor, cs(Illusionist.color, "Illusionist") + ": Hats Cooldown",  20f, 15f, 30f, 1f, illusionistSpawnRate);
            illusionistLightsOutCooldown = CustomOption.Create(192, Types.Impostor, cs(Illusionist.color, "Illusionist") + ": Lights Cooldown",  30f, 20f, 40f, 1f, illusionistSpawnRate);
            illusionistLightsOutDuration = CustomOption.Create(193, Types.Impostor, cs(Illusionist.color, "Illusionist") + ": Blackout Duration",  10f, 5f, 15f, 1f, illusionistSpawnRate);

            // Manipulator options
            manipulatorSpawnRate = CustomOption.Create(200, Types.Impostor, cs(Manipulator.color, "Manipulator"),  rates, null, true);

            // Bomberman options
            bombermanSpawnRate = CustomOption.Create(210, Types.Impostor, cs(Bomberman.color, "Bomberman"),  rates, null, true);
            bombermanBombCooldown = CustomOption.Create(211, Types.Impostor, cs(Bomberman.color, "Bomberman") + ": Cooldown",  30f, 30f, 60f, 5f, bombermanSpawnRate);
            bombermanSelfBombDuration = CustomOption.Create(212, Types.Impostor, cs(Bomberman.color, "Bomberman") + ": Self Bomb Timer",  10f, 5f, 15f, 1f, bombermanSpawnRate);

            // Chameleon options
            chameleonSpawnRate = CustomOption.Create(220, Types.Impostor, cs(Chameleon.color, "Chameleon"),  rates, null, true);
            chameleonCooldown = CustomOption.Create(221, Types.Impostor, cs(Chameleon.color, "Chameleon") + ": Cooldown",  30f, 20f, 40f, 2.5f, chameleonSpawnRate);
            chameleonDuration = CustomOption.Create(222, Types.Impostor, cs(Chameleon.color, "Chameleon") + ": Duration",  10f, 10f, 15f, 1f, chameleonSpawnRate);

            // Gambler options
            gamblerSpawnRate = CustomOption.Create(230, Types.Impostor, cs(Gambler.color, "Gambler"),  rates, null, true);
            gamblerCanCallEmergency = CustomOption.Create(231, Types.Impostor, cs(Gambler.color, "Gambler") + ": Can use emergency button",  false, gamblerSpawnRate);
            gamblerCanKillThroughShield = CustomOption.Create(232, Types.Impostor, cs(Gambler.color, "Gambler") + ": Ignore shields",  false, gamblerSpawnRate);

            // Sorcerer Options
            sorcererSpawnRate = CustomOption.Create(240, Types.Impostor, cs(Sorcerer.color, "Sorcerer"),  rates, null, true);
            sorcererCooldown = CustomOption.Create(241, Types.Impostor, cs(Sorcerer.color, "Sorcerer") + ": Cooldown",  30f, 30f, 40f, 2.5f, sorcererSpawnRate);
            sorcererSpellDuration = CustomOption.Create(242, Types.Impostor, cs(Sorcerer.color, "Sorcerer") + ": Spell Duration",  3f, 3f, 5f, 1f, sorcererSpawnRate);
            sorcererCanCallEmergency = CustomOption.Create(243, Types.Impostor, cs(Sorcerer.color, "Sorcerer") + ": Can use emergency button",  false, sorcererSpawnRate);

            // Medusa options
            medusaSpawnRate = CustomOption.Create(250, Types.Impostor, cs(Medusa.color, "Medusa"),  rates, null, true);
            medusaCooldown = CustomOption.Create(251, Types.Impostor, cs(Medusa.color, "Medusa") + ": Cooldown",  20f, 20f, 30f, 1f, medusaSpawnRate);
            medusaDelay = CustomOption.Create(252, Types.Impostor, cs(Medusa.color, "Medusa") + ": Petrify Delay",  10f, 10f, 15f, 1f, medusaSpawnRate);

            // Hypnotist options
            hypnotistSpawnRate = CustomOption.Create(260, Types.Impostor, cs(Hypnotist.color, "Hypnotist"),  rates, null, true);
            hypnotistCooldown = CustomOption.Create(261, Types.Impostor, cs(Hypnotist.color, "Hypnotist") + ": Cooldown",  20f, 15f, 30f, 1f, hypnotistSpawnRate);
            hypnotistNumberOfSpirals = CustomOption.Create(262, Types.Impostor, cs(Hypnotist.color, "Hypnotist") + ": Spiral Number",  5f, 1f, 5f, 1f, hypnotistSpawnRate);
            hypnotistSpiralsDuration = CustomOption.Create(263, Types.Impostor, cs(Hypnotist.color, "Hypnotist") + ": Spiral Effect Duration",  20f, 10f, 30f, 1f, hypnotistSpawnRate);

            // Archer options
            archerSpawnRate = CustomOption.Create(270, Types.Impostor, cs(Archer.color, "Archer"),  rates, null, true);
            archerShotRange = CustomOption.Create(271, Types.Impostor, cs(Archer.color, "Archer") + ": Arrow Range",  15f, 5f, 15f, 1f, archerSpawnRate);
            archerNoticeRange = CustomOption.Create(272, Types.Impostor, cs(Archer.color, "Archer") + ": Notify Range",  10f, 10f, 30f, 2.5f, archerSpawnRate);
            archerAimAssistDuration = CustomOption.Create(273, Types.Impostor, cs(Archer.color, "Archer") + ": Aim Duration",  10f, 3f, 10f, 1f, archerSpawnRate);

            // Plumber options
            plumberSpawnRate = CustomOption.Create(280, Types.Impostor, cs(Plumber.color, "Plumber"),  rates, null, true);
            plumberCooldown = CustomOption.Create(281, Types.Impostor, cs(Plumber.color, "Plumber") + ": Cooldown",  20f, 15f, 30f, 1f, plumberSpawnRate);

            // Librarian options
            librarianSpawnRate = CustomOption.Create(290, Types.Impostor, cs(Librarian.color, "Librarian"),  rates, null, true);
            librarianCooldown = CustomOption.Create(291, Types.Impostor, cs(Librarian.color, "Librarian") + ": Cooldown",  20f, 20f, 30f, 1f, librarianSpawnRate);

            // Renegade & Minion options
            renegadeSpawnRate = CustomOption.Create(300, Types.Rebel, cs(Renegade.color, "Renegade"),  rates, null, true);
            renegadeCanUseVents = CustomOption.Create(301, Types.Rebel, cs(Renegade.color, "Renegade") + ": Can use Vents",  true, renegadeSpawnRate);
            renegadeCanRecruitMinion = CustomOption.Create(302, Types.Rebel, cs(Renegade.color, "Renegade") + ": Can Recruit a Minion",  true, renegadeSpawnRate);

            // Bountyhunter options
            bountyHunterSpawnRate = CustomOption.Create(310, Types.Rebel, cs(BountyHunter.color, "Bounty Hunter"),  rates, null, true);

            // Trapper options
            trapperSpawnRate = CustomOption.Create(320, Types.Rebel, cs(Trapper.color, "Trapper"),  rates, null, true);
            trapperCooldown = CustomOption.Create(321, Types.Rebel, cs(Trapper.color, "Trapper") + ": Cooldown",  15f, 15f, 30f, 1f, trapperSpawnRate);
            trapperMineNumber = CustomOption.Create(322, Types.Rebel, cs(Trapper.color, "Trapper") + ": Mine Number",  3f, 1f, 3f, 1f, trapperSpawnRate);
            trapperMineDuration = CustomOption.Create(323, Types.Rebel, cs(Trapper.color, "Trapper") + ": Mine Duration",  45f, 30f, 60f, 5f, trapperSpawnRate);
            trapperTrapNumber = CustomOption.Create(324, Types.Rebel, cs(Trapper.color, "Trapper") + ": Trap Number",  3f, 1f, 5f, 1f, trapperSpawnRate);
            trapperTrapDuration = CustomOption.Create(325, Types.Rebel, cs(Trapper.color, "Trapper") + ": Trap Duration",  60f, 30f, 120f, 5f, trapperSpawnRate);
            
            // Yinyanger options
            yinyangerSpawnRate = CustomOption.Create(330, Types.Rebel, cs(Yinyanger.color, "Yinyanger"),  rates, null, true);
            yinyangerCooldown = CustomOption.Create(331, Types.Rebel, cs(Yinyanger.color, "Yinyanger") + ": Cooldown",  15f, 15f, 30f, 1f, yinyangerSpawnRate);

            // Challenger options
            challengerSpawnRate = CustomOption.Create(340, Types.Rebel, cs(Challenger.color, "Challenger"),  rates, null, true);
            challengerCooldown = CustomOption.Create(341, Types.Rebel, cs(Challenger.color, "Challenger") + ": Cooldown",  15f, 15f, 30f, 1f, challengerSpawnRate);
            challengerKillsForWin = CustomOption.Create(342, Types.Rebel, cs(Challenger.color, "Challenger") + ": Kills to Win",  3f, 3f, 5f, 1f, challengerSpawnRate);

            // Ninja options
            ninjaSpawnRate = CustomOption.Create(350, Types.Rebel, cs(Ninja.color, "Ninja"),  rates, null, true);

            // Berserker options
            berserkerSpawnRate = CustomOption.Create(360, Types.Rebel, cs(Berserker.color, "Berserker"),  rates, null, true);
            berserkerTimeToKill = CustomOption.Create(361, Types.Rebel, cs(Berserker.color, "Berserker") + ": Kill Time Limit",  30f, 20f, 40f, 2.5f, berserkerSpawnRate);

            // Yandere options
            yandereSpawnRate = CustomOption.Create(370, Types.Rebel, cs(Yandere.color, "Yandere"),  rates, null, true);
            yandereCooldown = CustomOption.Create(371, Types.Rebel, cs(Yandere.color, "Yandere") + ": Stare Cooldown",  30f, 15f, 30f, 1f, yandereSpawnRate);
            yandereStareTimes = CustomOption.Create(372, Types.Rebel, cs(Yandere.color, "Yandere") + ": Stare Times",  5f, 3f, 5f, 1f, yandereSpawnRate);
            yandereStareDuration = CustomOption.Create(373, Types.Rebel, cs(Yandere.color, "Yandere") + ": Stare Duration",  3f, 2f, 4f, 1f, yandereSpawnRate);

            // Stranded options
            strandedSpawnRate = CustomOption.Create(380, Types.Rebel, cs(Stranded.color, "Stranded"),  rates, null, true);

            // Monja options
            monjaSpawnRate = CustomOption.Create(390, Types.Rebel, cs(Monja.color, "Monja"),  rates, null, true);

            // Joker options
            jokerSpawnRate = CustomOption.Create(400, Types.Neutral, cs(Joker.color, "Joker"),  rates, null, true);
            jokerCanSabotage = CustomOption.Create(402, Types.Neutral, cs(Joker.color, "Joker") + ": Can Sabotage",  true, jokerSpawnRate);

            // RoleThief options
            rolethiefSpawnRate = CustomOption.Create(410, Types.Neutral, cs(RoleThief.color, "Role Thief"),  rates, null, true);
            rolethiefCooldown = CustomOption.Create(411, Types.Neutral, cs(RoleThief.color, "Role Thief") + ": Cooldown",  20f, 10f, 30f, 2.5f, rolethiefSpawnRate);

            // Pyromaniac options
            pyromaniacSpawnRate = CustomOption.Create(420, Types.Neutral, cs(Pyromaniac.color, "Pyromaniac"),  rates, null, true);
            pyromaniacCooldown = CustomOption.Create(421, Types.Neutral, cs(Pyromaniac.color, "Pyromaniac") + ": Cooldown",  15f, 10f, 20f, 1f, pyromaniacSpawnRate);
            pyromaniacDuration = CustomOption.Create(422, Types.Neutral, cs(Pyromaniac.color, "Pyromaniac") + ": Ignite Duration",  3f, 1f, 5f, 1f, pyromaniacSpawnRate);

            // Treasure hunter options
            treasureHunterSpawnRate = CustomOption.Create(430, Types.Neutral, cs(TreasureHunter.color, "Treasure Hunter"),  rates, null, true);
            treasureHunterTreasureNumber = CustomOption.Create(431, Types.Neutral, cs(TreasureHunter.color, "Treasure Hunter") + ": Treasures to Win",  5f, 5f, 10f, 1f, treasureHunterSpawnRate);

            // Devourer options
            devourerSpawnRate = CustomOption.Create(440, Types.Neutral, cs(Devourer.color, "Devourer"),  rates, null, true);
            devourerBodiesNumber = CustomOption.Create(441, Types.Neutral, cs(Devourer.color, "Devourer") + ": Devours to Win",  4f, 3f, 7f, 1f, devourerSpawnRate);

            // Poisoner options
            poisonerSpawnRate = CustomOption.Create(450, Types.Neutral, cs(Poisoner.color, "Poisoner"),  rates, null, true);
            poisonerInfectRange = CustomOption.Create(451, Types.Neutral, cs(Poisoner.color, "Poisoner") + ": Poison Infect Range",  2f, 0.5f, 2f, 0.25f, poisonerSpawnRate);
            poisonerInfectDuration = CustomOption.Create(452, Types.Neutral, cs(Poisoner.color, "Poisoner") + ": Time to fully Poison",  20f, 15f, 30f, 1f, poisonerSpawnRate);
            poisonerCanSabotage = CustomOption.Create(453, Types.Neutral, cs(Poisoner.color, "Poisoner") + ": Can Sabotage",  true, poisonerSpawnRate);

            // Puppeteer options
            puppeteerSpawnRate = CustomOption.Create(460, Types.Neutral, cs(Puppeteer.color, "Puppeteer"),  rates, null, true);
            puppeteerNumberOfKills = CustomOption.Create(461, Types.Neutral, cs(Puppeteer.color, "Puppeteer") + ": Number of Kills",  3f, 2f, 4f, 1f, puppeteerSpawnRate);

            // Exiler options
            exilerSpawnRate = CustomOption.Create(470, Types.Neutral, cs(Exiler.color, "Exiler"),  rates, null, true);

            // Amnesiac options
            amnesiacSpawnRate = CustomOption.Create(480, Types.Neutral, cs(Amnesiac.color, "Amnesiac"),  rates, null, true);

            // Seeker options
            seekerSpawnRate = CustomOption.Create(490, Types.Neutral, cs(Seeker.color, "Seeker"),  rates, null, true);
            seekerCooldown = CustomOption.Create(491, Types.Neutral, cs(Seeker.color, "Seeker") + ": Cooldown",  5f, 5f, 10f, 1f, seekerSpawnRate);
            seekerPointsNumber = CustomOption.Create(492, Types.Neutral, cs(Seeker.color, "Seeker") + ": Points to Win",  5f, 5f, 10f, 1f, seekerSpawnRate);

            // Captain options
            captainSpawnRate = CustomOption.Create(500, Types.Crewmate, cs(Captain.color, "Captain"),  rates, null, true);
            captainCanSpecialVoteOneTime = CustomOption.Create(501, Types.Crewmate, cs(Captain.color, "Captain") + ": Can Special Vote one time",  true, captainSpawnRate);

            // Mechanic options
            mechanicSpawnRate = CustomOption.Create(510, Types.Crewmate, cs(Mechanic.color, "Mechanic"),  rates, null, true);
            mechanicNumberOfRepairs = CustomOption.Create(511, Types.Crewmate, cs(Mechanic.color, "Mechanic") + ": Repairs Number",  2f, 1f, 2f, 1f, mechanicSpawnRate);
            mechanicRechargeTasksNumber = CustomOption.Create(512, Types.Crewmate, cs(Mechanic.color, "Mechanic") + ": Tasks for recharge batteries",  2f, 1f, 3f, 1f, mechanicSpawnRate);
            mechanicExpertRepairs = CustomOption.Create(513, Types.Crewmate, cs(Mechanic.color, "Mechanic") + ": Expert Repairs",  true, mechanicSpawnRate);

            // Sheriff options
            sheriffSpawnRate = CustomOption.Create(520, Types.Crewmate, cs(Sheriff.color, "Sheriff"),  rates, null, true);
            sheriffCanKillNeutrals = CustomOption.Create(521, Types.Crewmate, cs(Sheriff.color, "Sheriff") + ": Can Kill Neutrals",  true, sheriffSpawnRate);

            // Detective options
            detectiveSpawnRate = CustomOption.Create(530, Types.Crewmate, cs(Detective.color, "Detective"),  rates, null, true);
            detectiveShowFootprints = CustomOption.Create(531, Types.Crewmate, cs(Detective.color, "Detective") + ": Show Footprints",  new string[] { "Button Use", "Always" }, detectiveSpawnRate);
            detectiveCooldown = CustomOption.Create(532, Types.Crewmate, cs(Detective.color, "Detective") + ": Cooldown",  15f, 10f, 20f, 1f, detectiveSpawnRate);
            detectiveShowFootPrintDuration = CustomOption.Create(533, Types.Crewmate, cs(Detective.color, "Detective") + ": Show Footprints Duration",  10f, 10f, 15f, 1f, detectiveSpawnRate); 
            detectiveAnonymousFootprints = CustomOption.Create(534, Types.Crewmate, cs(Detective.color, "Detective") + ": Anonymous Footprints",  false, detectiveSpawnRate);

            // Forensic options
            forensicSpawnRate = CustomOption.Create(540, Types.Crewmate, cs(Forensic.color, "Forensic"),  rates, null, true);
            forensicReportNameDuration = CustomOption.Create(541, Types.Crewmate, cs(Forensic.color, "Forensic") + ": Time to know the name",  10, 2.5f, 10, 2.5f, forensicSpawnRate);
            forensicReportColorDuration = CustomOption.Create(542, Types.Crewmate, cs(Forensic.color, "Forensic") + ": Time to know the color type",  20, 10, 20, 2.5f, forensicSpawnRate);
            forensicReportClueDuration = CustomOption.Create(543, Types.Crewmate, cs(Forensic.color, "Forensic") + ": Time to know if the killer has hat, outfit, pet or visor",  30, 20, 30, 2.5f, forensicSpawnRate);
            forensicDuration = CustomOption.Create(544, Types.Crewmate, cs(Forensic.color, "Forensic") + ": Question Duration",  5f, 5f, 10f, 1f, forensicSpawnRate);
            forensicOneTimeUse = CustomOption.Create(545, Types.Crewmate, cs(Forensic.color, "Forensic") + ": One question per Ghost",  true, forensicSpawnRate);

            // TimeTraveler options
            timeTravelerSpawnRate = CustomOption.Create(550, Types.Crewmate, cs(TimeTraveler.color, "Time Traveler"),  rates, null, true);
            timeTravelerCooldown = CustomOption.Create(551, Types.Crewmate, cs(TimeTraveler.color, "Time Traveler") + ": Cooldown",  15f, 10f, 20f, 1f, timeTravelerSpawnRate);
            timeTravelerStopTime = CustomOption.Create(552, Types.Crewmate, cs(TimeTraveler.color, "Time Traveler") + ": Stop Duration",  10f, 5f, 10f, 1f, timeTravelerSpawnRate);

            // Squire options
            squireSpawnRate = CustomOption.Create(560, Types.Crewmate, cs(Squire.color, "Squire"),  rates, null, true);
            squireShowShielded = CustomOption.Create(561, Types.Crewmate, cs(Squire.color, "Squire") + ": Show Shielded Player to",  new string[] { "Squire", "Both", "All" }, squireSpawnRate);
            squireShowAttemptToShielded = CustomOption.Create(562, Types.Crewmate, cs(Squire.color, "Squire") + ": Play murder attempt sound if shielded",  true, squireSpawnRate);
            squireResetShieldAfterMeeting = CustomOption.Create(563, Types.Crewmate, cs(Squire.color, "Squire") + ": Can shield again after meeting",  true, squireSpawnRate);

            // Cheater options
            cheaterSpawnRate = CustomOption.Create(570, Types.Crewmate, cs(Cheater.color, "Cheater"),  rates, null, true);
            cheaterCanCallEmergency = CustomOption.Create(571, Types.Crewmate, cs(Cheater.color, "Cheater") + ": Can use emergency button",  false, cheaterSpawnRate);
            cheatercanOnlyCheatOthers = CustomOption.Create(572, Types.Crewmate, cs(Cheater.color, "Cheater") + ": Can swap himself",  false, cheaterSpawnRate);

            // FortuneTeller options
            fortuneTellerSpawnRate = CustomOption.Create(580, Types.Crewmate, cs(FortuneTeller.color, "Fortune Teller"),  rates, null, true);
            fortuneTellerCooldown = CustomOption.Create(581, Types.Crewmate, cs(FortuneTeller.color, "Fortune Teller") + ": Cooldown",  30f, 30f, 40f, 2.5f, fortuneTellerSpawnRate);
            fortuneTellerDuration = CustomOption.Create(582, Types.Crewmate, cs(FortuneTeller.color, "Fortune Teller") + ": Reveal Time",  3f, 3f, 5f, 1f, fortuneTellerSpawnRate);
            fortuneTellerNumberOfSee = CustomOption.Create(583, Types.Crewmate, cs(FortuneTeller.color, "Fortune Teller") + ": Reveal Number",  1f, 1f, 2f, 1f, fortuneTellerSpawnRate);
            fortuneTellerRechargeTasksNumber = CustomOption.Create(584, Types.Crewmate, cs(FortuneTeller.color, "Fortune Teller") + ": Tasks for recharge batteries",  3f, 3f, 4f, 1f, fortuneTellerSpawnRate);
            fortuneTellerKindOfInfo = CustomOption.Create(585, Types.Crewmate, cs(FortuneTeller.color, "Fortune Teller") + ": Revealed Information",  new string[] { "Good / Bad", "Rol Name" }, fortuneTellerSpawnRate);
            fortuneTellerPlayersWithNotification = CustomOption.Create(586, Types.Crewmate, cs(FortuneTeller.color, "Fortune Teller") + ": Show Notification to",  new string[] { "Impostors", "Crewmates", "All", "Nobody" }, fortuneTellerSpawnRate);
            fortuneTellerCanCallEmergency = CustomOption.Create(587, Types.Crewmate, cs(FortuneTeller.color, "Fortune Teller") + ": Can use emergency button",  false, fortuneTellerSpawnRate);

            // Hacker options
            hackerSpawnRate = CustomOption.Create(590, Types.Crewmate, cs(Hacker.color, "Hacker"),  rates, null, true);
            hackerCooldown = CustomOption.Create(591, Types.Crewmate, cs(Hacker.color, "Hacker") + ": Cooldown",  20f, 20f, 40f, 5f, hackerSpawnRate);
            hackerHackeringDuration = CustomOption.Create(592, Types.Crewmate, cs(Hacker.color, "Hacker") + ": Duration",  15f, 10f, 15f, 1f, hackerSpawnRate);
            hackerToolsNumber = CustomOption.Create(593, Types.Crewmate, cs(Hacker.color, "Hacker") + ": Battery Uses",  3f, 1f, 5f, 1f, hackerSpawnRate);
            hackerRechargeTasksNumber = CustomOption.Create(594, Types.Crewmate, cs(Hacker.color, "Hacker") + ": Tasks for recharge batteries",  2f, 1f, 3f, 1f, hackerSpawnRate);

            // Sleuth options
            sleuthSpawnRate = CustomOption.Create(600, Types.Crewmate, cs(Sleuth.color, "Sleuth"),  rates, null, true);
            sleuthUpdateIntervall = CustomOption.Create(601, Types.Crewmate, cs(Sleuth.color, "Sleuth") + ": Track Interval",  0f, 0f, 3f, 1f, sleuthSpawnRate);
            sleuthResetTargetAfterMeeting = CustomOption.Create(602, Types.Crewmate, cs(Sleuth.color, "Sleuth") + ": Can Track again after meeting",  true, sleuthSpawnRate);
            sleuthCorpsesPathfindCooldown = CustomOption.Create(604, Types.Crewmate, cs(Sleuth.color, "Sleuth") + ": Track Corpses Cooldown",  30f, 20f, 40f, 2.5f, sleuthSpawnRate);
            sleuthCorpsesPathfindDuration = CustomOption.Create(605, Types.Crewmate, cs(Sleuth.color, "Sleuth") + ": Track Corpses Duration",  10f, 5f, 15f, 2.5f, sleuthSpawnRate);
            sleuthDuration = CustomOption.Create(606, Types.Crewmate, cs(Sleuth.color, "Sleuth") + ": Who's There Duration",  10f, 5f, 15f, 1f, sleuthSpawnRate);

            // Fink options
            finkSpawnRate = CustomOption.Create(610, Types.Crewmate, cs(Fink.color, "Fink"),  rates, null, true);
            finkLeftTasksForImpostors = CustomOption.Create(611, Types.Crewmate, cs(Fink.color, "Fink") + ": Tasks remaining for being revealed to Impostors",  1f, 1f, 3f, 1f, finkSpawnRate);
            finkCooldown = CustomOption.Create(612, Types.Crewmate, cs(Fink.color, "Fink") + ": Cooldown",  20f, 20f, 30f, 1f, finkSpawnRate);
            finkDuration = CustomOption.Create(613, Types.Crewmate, cs(Fink.color, "Fink") + ": Hawkeye Duration",  5f, 3f, 5f, 1f, finkSpawnRate);

            // Kid options
            kidSpawnRate = CustomOption.Create(620, Types.Crewmate, cs(Kid.color, "Kid"),  rates, null, true);

            // Welder options
            welderSpawnRate = CustomOption.Create(630, Types.Crewmate, cs(Welder.color, "Welder"),  rates, null, true);
            welderCooldown = CustomOption.Create(631, Types.Crewmate, cs(Welder.color, "Welder") + ": Cooldown",  20f, 10f, 40f, 2.5f, welderSpawnRate);
            welderTotalWelds = CustomOption.Create(632, Types.Crewmate, cs(Welder.color, "Welder") + ": Seal Number",  5f, 3f, 5f, 1f, welderSpawnRate);

            // Spiritualist options
            spiritualistSpawnRate = CustomOption.Create(640, Types.Crewmate, cs(Spiritualist.color, "Spiritualist"),  rates, null, true);

            // Vigilant options
            vigilantSpawnRate = CustomOption.Create(650, Types.Crewmate, cs(Vigilant.color, "Vigilant"),  rates, null, true);
            vigilantCooldown = CustomOption.Create(651, Types.Crewmate, cs(Vigilant.color, "Vigilant") + ": Cooldown",  20f, 10f, 30f, 2.5f, vigilantSpawnRate);
            vigilantCamDuration = CustomOption.Create(653, Types.Crewmate, cs(Vigilant.color, "Vigilant") + ": Remote Camera Duration",  10f, 5f, 15f, 1f, vigilantSpawnRate);
            vigilantCamMaxCharges = CustomOption.Create(654, Types.Crewmate, cs(Vigilant.color, "Vigilant") + ": Battery Uses",  5f, 1f, 5f, 1f, vigilantSpawnRate);
            vigilantCamRechargeTasksNumber = CustomOption.Create(655, Types.Crewmate, cs(Vigilant.color, "Vigilant") + ": Tasks for recharge batteries",  2f, 1f, 3f, 1f, vigilantSpawnRate);

            // Hunter options
            hunterSpawnRate = CustomOption.Create(660, Types.Crewmate, cs(Hunter.color, "Hunter"),  rates, null, true);
            hunterResetTargetAfterMeeting = CustomOption.Create(661, Types.Crewmate, cs(Hunter.color, "Hunter") + ": Can mark again after meeting",  true, hunterSpawnRate);

            // Jinx
            jinxSpawnRate = CustomOption.Create(670, Types.Crewmate, cs(Jinx.color, "Jinx"),  rates, null, true);
            jinxCooldown = CustomOption.Create(671, Types.Crewmate, cs(Jinx.color, "Jinx") + ": Cooldown",  20f, 10f, 30f, 2.5f, jinxSpawnRate);
            jinxJinxsNumber = CustomOption.Create(672, Types.Crewmate, cs(Jinx.color, "Jinx") + ": Jinx Number",  15f, 5f, 15f, 1f, jinxSpawnRate);

            // Coward options
            cowardSpawnRate = CustomOption.Create(680, Types.Crewmate, cs(Coward.color, "Coward"),  rates, null, true);
            cowardNumberOfCalls = CustomOption.Create(681, Types.Crewmate, cs(Coward.color, "Coward") + ": Number Of Meetings",  2f, 1f, 2f, 1f, cowardSpawnRate);
            cowardRechargeTasksNumber = CustomOption.Create(682, Types.Crewmate, cs(Coward.color, "Coward") + ": Tasks for recharge batteries",  2f, 2f, 3f, 1f, cowardSpawnRate);

            // Bat options
            batSpawnRate = CustomOption.Create(690, Types.Crewmate, cs(Bat.color, "Bat"),  rates, null, true);
            batCooldown = CustomOption.Create(691, Types.Crewmate, cs(Bat.color, "Bat") + ": Cooldown",  15f, 10f, 20f, 1f, batSpawnRate);
            batDuration = CustomOption.Create(692, Types.Crewmate, cs(Bat.color, "Bat") + ": Emit Duration",  10f, 5f, 10f, 1f, batSpawnRate);
            batRange = CustomOption.Create(693, Types.Crewmate, cs(Bat.color, "Bat") + ": Emit Range",  2f, 0.5f, 2f, 0.25f, batSpawnRate);

            // Necromancer options
            necromancerSpawnRate = CustomOption.Create(700, Types.Crewmate, cs(Necromancer.color, "Necromancer"),  rates, null, true);
            necromancerReviveCooldown = CustomOption.Create(701, Types.Crewmate, cs(Necromancer.color, "Necromancer") + ": Cooldown",  20f, 20f, 40f, 1f, necromancerSpawnRate);
            necromancerReviveTimer = CustomOption.Create(702, Types.Crewmate, cs(Necromancer.color, "Necromancer") + ": Revive Duration",  5f, 5f, 10f, 1f, necromancerSpawnRate);
            necromancerMaxReviveRoomDistance = CustomOption.Create(703, Types.Crewmate, cs(Necromancer.color, "Necromancer") + ": Room Distance",  25f, 25f, 50f, 2.5f, necromancerSpawnRate);

            // Engineer options
            engineerSpawnRate = CustomOption.Create(710, Types.Crewmate, cs(Engineer.color, "Engineer"),  rates, null, true);
            engineerCooldown = CustomOption.Create(711, Types.Crewmate, cs(Engineer.color, "Engineer") + ": Cooldown",  10f, 10f, 20f, 1f, engineerSpawnRate);
            engineerNumberOfTraps = CustomOption.Create(712, Types.Crewmate, cs(Engineer.color, "Engineer") + ": Trap Number",  5f, 3f, 5f, 1f, engineerSpawnRate);
            engineerTrapsDuration = CustomOption.Create(713, Types.Crewmate, cs(Engineer.color, "Engineer") + ": Trap Duration",  60f, 30f, 120f, 5f, engineerSpawnRate);
            engineerAccelTrapIncrease = CustomOption.Create(714, Types.Crewmate, cs(Engineer.color, "Engineer") + ": Speed Increase",  1.1f, 1.1f, 1.3f, 0.2f, engineerSpawnRate);
            engineerDecelTrapDecrease = CustomOption.Create(715, Types.Crewmate, cs(Engineer.color, "Engineer") + ": Speed Decrease",  0.4f, 0.4f, 0.8f, 0.2f, engineerSpawnRate);

            // Locksmith options
            locksmithSpawnRate = CustomOption.Create(720, Types.Crewmate, cs(Locksmith.color, "Locksmith"),  rates, null, true);
            locksmithCooldown = CustomOption.Create(721, Types.Crewmate, cs(Locksmith.color, "Locksmith") + ": Cooldown",  20f, 20f, 40f, 1f, locksmithSpawnRate);

            // Task Master
            taskMasterSpawnRate = CustomOption.Create(730, Types.Crewmate, cs(TaskMaster.color, "Task Master"),  rates, null, true);
            taskMasterExtraCommonTasks = CustomOption.Create(731, Types.Crewmate, cs(TaskMaster.color, "Task Master") + ": Extra Common Tasks",  1f, 1f, 2f, 1f, taskMasterSpawnRate);
            taskMasterExtraLongTasks = CustomOption.Create(732, Types.Crewmate, cs(TaskMaster.color, "Task Master") + ": Extra Long Tasks",  1f, 1f, 3f, 1f, taskMasterSpawnRate);
            taskMasterExtraShortTasks = CustomOption.Create(733, Types.Crewmate, cs(TaskMaster.color, "Task Master") + ": Extra Short Tasks",  1f, 1f, 5f, 1f, taskMasterSpawnRate);
            taskMasterCooldown = CustomOption.Create(734, Types.Crewmate, cs(TaskMaster.color, "Task Master") + ": Speed Cooldown",  20f, 20f, 30f, 1f, taskMasterSpawnRate);
            taskMasterDuration = CustomOption.Create(735, Types.Crewmate, cs(TaskMaster.color, "Task Master") + ": Speed Duration",  10f, 5f, 10f, 1f, taskMasterSpawnRate);
            taskMasterRewardType = CustomOption.Create(736, Types.Crewmate, cs(TaskMaster.color, "Task Master") + ": Reward Type",  new string[] { "Extra tasks", "Kill button" }, taskMasterSpawnRate);

            // Jailer
            jailerSpawnRate = CustomOption.Create(740, Types.Crewmate, cs(Jailer.color, "Jailer"),  rates, null, true);
            jailerCooldown = CustomOption.Create(741, Types.Crewmate, cs(Jailer.color, "Jailer") + ": Cooldown",  20f, 15f, 30f, 1f, jailerSpawnRate);
            jailerDuration = CustomOption.Create(742, Types.Crewmate, cs(Jailer.color, "Jailer") + ": Jail Duration",  10f, 5f, 15f, 1f, jailerSpawnRate);

            // Modifiers
            loverPlayer = CustomOption.Create(801, Types.Modifier, cs(Modifiers.color, "Lovers"),  rates, null, true);
            lighterPlayer = CustomOption.Create(802, Types.Modifier, cs(Modifiers.color, "Lighter"),  rates, null, true);
            blindPlayer = CustomOption.Create(803, Types.Modifier, cs(Modifiers.color, "Blind"),  rates, null, true);
            flashPlayer = CustomOption.Create(804, Types.Modifier, cs(Modifiers.color, "Flash"),  rates, null, true);
            bigchungusPlayer = CustomOption.Create(805, Types.Modifier, cs(Modifiers.color, "Big Chungus"),  rates, null, true);
            theChosenOnePlayer = CustomOption.Create(806, Types.Modifier, cs(Modifiers.color, "The Chosen One"),  rates, null, true);
            theChosenOneReportDelay = CustomOption.Create(807, Types.Modifier, cs(Modifiers.color, "The Chosen One") + ": Report Delay",  0f, 0f, 5f, 1f, theChosenOnePlayer);
            performerPlayer = CustomOption.Create(808, Types.Modifier, cs(Modifiers.color, "Performer"),  rates, null, true);
            performerDuration = CustomOption.Create(809, Types.Modifier, cs(Modifiers.color, "Performer") + ": Alarm Duration",  30f, 15f, 30f, 1f, performerPlayer);
            proPlayer = CustomOption.Create(810, Types.Modifier, cs(Modifiers.color, "Pro"),  rates, null, true);
            paintballPlayer = CustomOption.Create(811, Types.Modifier, cs(Modifiers.color, "Paintball"),  rates, null, true);
            paintballDuration = CustomOption.Create(812, Types.Modifier, cs(Modifiers.color, "Paintball") + ": Paint Duration",  10f, 5f, 15f, 1f, paintballPlayer);
            electricianPlayer = CustomOption.Create(813, Types.Modifier, cs(Modifiers.color, "Electrician"),  rates, null, true);
            electricianDuration = CustomOption.Create(814, Types.Modifier, cs(Modifiers.color, "Electrician") + ": Discharge Duration",  5f, 5f, 10f, 1f, electricianPlayer);
        }
    }
}