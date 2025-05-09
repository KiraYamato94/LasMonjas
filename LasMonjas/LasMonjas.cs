using System.Linq;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using LasMonjas.Objects;
using LasMonjas.Core;
using AmongUs.GameOptions;
using static UnityEngine.GraphicsBuffer;

namespace LasMonjas
{
    [HarmonyPatch]
    public static class LasMonjas
    {
        public static System.Random rnd = new System.Random((int)DateTime.Now.Ticks);

        public static bool removedSwipe = false;

        public static bool removedAirshipDoors = false;

        public static bool activatedSensei = false;

        public static bool updatedSenseiMinimap = false;

        public static bool updatedSenseiAdminmap = false;

        public static bool activatedDleks = false;

        public static bool createdduelarena = false;

        public static bool createdseekerarena = false;

        public static bool createddevourerarena = false;

        public static bool createdjail = false;

        public static bool createdStrandedBoxes = false;

        public static bool createdMonjaItems = false;

        public static bool createdcapturetheflag = false;

        public static bool createdpoliceandthief = false;

        public static bool createdkingofthehill = false;

        public static bool createdhotpotato = false;

        public static bool createdzombielaboratory = false;

        public static bool createdbattleroyale = false;

        public static bool createdmonjafestival = false;

        public static bool activatedReportButtonAfterCustomMode = false;

        public static int quackNumber = 0;

        public static int alivePlayers = 15;

        public static int howmanyBattleRoyaleplayers = 0;

        public static List<GameObject> nightOverlay = new List<GameObject>();
        public static bool canNightOverlay = true;
        public static bool removeNightOverlay = true;
        public static bool isLightsOut = false;
        public static bool nightVision = CustomOptionHolder.nightVisionLightSabotage.getBool();

        public static bool shakeScreenReactor = CustomOptionHolder.screenShakeReactorSabotage.getBool();

        public static bool anonymousComms = CustomOptionHolder.anonymousCommsSabotage.getBool();
        public static bool isHappeningAnonymousComms = false;

        public static bool slowSpeedOxigen = CustomOptionHolder.slowSpeedOxigenSabotage.getBool();

        public static bool hideVentAnim = CustomOptionHolder.hideVentAnimOnShadows.getBool();

        public static List<GameObject> whoAmIModeGlobalItems = new List<GameObject>();
        public static List<GameObject> whoAmIModeCrewItems = new List<GameObject>();
        public static List<GameObject> whoAmIModeImpostorItems = new List<GameObject>();
        public static List<GameObject> whoAmIModeRebelsItems = new List<GameObject>();
        public static List<GameObject> whoAmIModeNeutralsItems = new List<GameObject>();
        public static bool createdWhoAmI = false;
        public static bool whoAmIFoundRole = false;
        public static byte customSkeldHS = 0;

        public static int gameType = CustomOptionHolder.gameType.getSelection(); // 0 - Roles, 1 - Find a Role, 2 - CTF, 3 - PT, 4 - KOTH, 5 - HP, 6 - ZL, 7 - BR, 8 - MJ
        public static float gamemodeMatchDuration = CustomOptionHolder.gamemodeMatchDuration.getFloat();
        public static float gamemodeKillCooldown = CustomOptionHolder.gamemodeKillCooldown.getFloat();
        public static bool gamemodeEnableFlashlight = CustomOptionHolder.gamemodeEnableFlashlight.getBool();
        public static float gamemodeFlashlightRange = CustomOptionHolder.gamemodeFlashlightRange.getFloat();
        public static float gamemodeReviveTime = CustomOptionHolder.gamemodeReviveTime.getFloat();
        public static float gamemodeInvincibilityTime = CustomOptionHolder.gamemodeInvincibilityTimeAfterRevive.getFloat();

        public static GameObject progress = null;
        public static float progressStart = 0;
        public static float progressEnd = 0;

        public static void clearAndReloadRoles() {
            gameType = CustomOptionHolder.gameType.getSelection();
            gamemodeMatchDuration = CustomOptionHolder.gamemodeMatchDuration.getFloat();
            gamemodeKillCooldown = CustomOptionHolder.gamemodeKillCooldown.getFloat();
            gamemodeEnableFlashlight = CustomOptionHolder.gamemodeEnableFlashlight.getBool();
            gamemodeFlashlightRange = CustomOptionHolder.gamemodeFlashlightRange.getFloat(); 
            gamemodeReviveTime = CustomOptionHolder.gamemodeReviveTime.getFloat();
            gamemodeInvincibilityTime = CustomOptionHolder.gamemodeInvincibilityTimeAfterRevive.getFloat();
            progressStart = 0;
            progressEnd = gamemodeMatchDuration;

            Mimic.clearAndReload();
            Painter.clearAndReload();
            Demon.clearAndReload();
            Janitor.clearAndReload();
            Illusionist.clearAndReload();
            Manipulator.clearAndReload();
            Bomberman.clearAndReload();
            Chameleon.clearAndReload();
            Gambler.clearAndReload();
            Sorcerer.clearAndReload();
            Medusa.clearAndReload();
            Hypnotist.clearAndReload();
            Archer.clearAndReload();
            Plumber.clearAndReload();
            Librarian.clearAndReload();

            Renegade.clearAndReload();
            Minion.clearAndReload();
            BountyHunter.clearAndReload();
            Trapper.clearAndReload();
            Yinyanger.clearAndReload();
            Challenger.clearAndReload();
            Ninja.clearAndReload();
            Berserker.clearAndReload();
            Yandere.clearAndReload();
            Stranded.clearAndReload();
            Monja.clearAndReload();

            Joker.clearAndReload();
            RoleThief.clearAndReload();
            Pyromaniac.clearAndReload();
            TreasureHunter.clearAndReload();
            Devourer.clearAndReload();
            Poisoner.clearAndReload();
            Puppeteer.clearAndReload();
            Exiler.clearAndReload();
            Amnesiac.clearAndReload();
            Seeker.clearAndReload();

            Captain.clearAndReload();
            Mechanic.clearAndReload();
            Sheriff.clearAndReload();
            Detective.clearAndReload();
            Forensic.clearAndReload();
            TimeTraveler.clearAndReload();
            Squire.clearAndReload();
            Cheater.clearAndReload();
            FortuneTeller.clearAndReload();
            Hacker.clearAndReload();
            Sleuth.clearAndReload();
            Fink.clearAndReload();
            Kid.clearAndReload();
            Welder.clearAndReload();
            Spiritualist.clearAndReload();
            Coward.clearAndReload();
            Vigilant.clearAndReload();
            Hunter.clearAndReload();
            Jinx.clearAndReload();
            Bat.clearAndReload();
            Necromancer.clearAndReload();
            Engineer.clearAndReload();
            Locksmith.clearAndReload();
            TaskMaster.clearAndReload();
            Jailer.clearAndReload();

            Modifiers.clearAndReload();

            CaptureTheFlag.clearAndReload();

            PoliceAndThief.clearAndReload();

            KingOfTheHill.clearAndReload();

            HotPotato.clearAndReload();

            ZombieLaboratory.clearAndReload();

            BattleRoyale.clearAndReload();

            MonjaFestival.clearAndReload();

            removedSwipe = false;
            removedAirshipDoors = false;
            activatedSensei = false;
            updatedSenseiMinimap = false;
            updatedSenseiAdminmap = false;
            activatedDleks = false;
            createdduelarena = false;
            createdseekerarena = false;
            createddevourerarena = false;
            createdjail = false;
            createdStrandedBoxes = false;
            createdMonjaItems = false;
            createdcapturetheflag = false;
            createdpoliceandthief = false;
            createdkingofthehill = false;
            createdhotpotato = false;
            createdzombielaboratory = false;
            createdbattleroyale = false;
            createdmonjafestival = false;
            activatedReportButtonAfterCustomMode = false;
            quackNumber = 0;
            alivePlayers = PlayerInCache.AllPlayers.Count;
            howmanyBattleRoyaleplayers = 0;

            nightOverlay = new List<GameObject>();
            canNightOverlay = true;
            removeNightOverlay = true;
            isLightsOut = false;
            nightVision = CustomOptionHolder.nightVisionLightSabotage.getBool();
            shakeScreenReactor = CustomOptionHolder.screenShakeReactorSabotage.getBool();
            anonymousComms = CustomOptionHolder.anonymousCommsSabotage.getBool();
            isHappeningAnonymousComms = false;
            slowSpeedOxigen = CustomOptionHolder.slowSpeedOxigenSabotage.getBool();
            hideVentAnim = CustomOptionHolder.hideVentAnimOnShadows.getBool();

            whoAmIModeGlobalItems.Clear();
            whoAmIModeCrewItems.Clear();
            whoAmIModeImpostorItems.Clear();
            whoAmIModeRebelsItems.Clear();
            whoAmIModeNeutralsItems.Clear();
            createdWhoAmI = false;
            whoAmIFoundRole = false;
        }
    }

    public static class Mimic
    {
        public static PlayerControl mimic;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 5f;
        public static float duration = 10f;
        public static float backUpduration = 10f;

        public static PlayerControl currentTarget;
        public static PlayerControl pickTarget;
        public static PlayerControl transformTarget;
        public static float transformTimer = 0f;

        public static void resetMimic() {
            transformTarget = null;
            transformTimer = 0f;
            if (mimic == null) return;
            mimic.setDefaultLook();
        }

        public static void clearAndReload() {
            resetMimic();
            mimic = null;
            currentTarget = null;
            pickTarget = null;
            transformTarget = null;
            transformTimer = 0f;
            cooldown = 5f;
            duration = CustomOptionHolder.mimicDuration.getFloat();
            backUpduration = duration;
        }

        private static Sprite pickTargetSprite;

        public static Sprite getpickTargetSprite() {
            if (pickTargetSprite) return pickTargetSprite;
            pickTargetSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.MimicPickTargetButton.png", 90f);
            return pickTargetSprite;
        }

        private static Sprite transformSprite;

        public static Sprite getTransformSprite() {
            if (transformSprite) return transformSprite;
            transformSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.MimicTransformButton.png", 90f);
            return transformSprite;
        }
    }

    public static class Painter
    {
        public static PlayerControl painter;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;
        public static float duration = 10f;
        public static float backUpduration = 10f;
        public static float painterTimer = 0f;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PainterPaintButton.png", 90f);
            return buttonSprite;
        }

        public static void resetPaint() {
            painterTimer = 0f;
            foreach (PlayerControl p in PlayerInCache.AllPlayers)
                p.setDefaultLook();
        }

        public static void clearAndReload() {
            resetPaint();
            painter = null;
            painterTimer = 0f;
            cooldown = CustomOptionHolder.painterCooldown.getFloat();
            duration = CustomOptionHolder.painterDuration.getFloat();
            backUpduration = duration;
        }
    }

    public static class Demon
    {
        public static PlayerControl demon;
        public static Color color = Palette.ImpostorRed;

        public static float delay = 10f;
        public static float cooldown = 30f;
        public static bool canKillNearNun = true;
        public static bool localPlacedNun = false;

        public static PlayerControl currentTarget;
        public static PlayerControl bitten;
        public static bool targetNearNun = false;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.DemonBiteButton.png", 90f);
            return buttonSprite;
        }

        private static Sprite nunButtonSprite;
        public static Sprite getNunButtonSprite() {
            if (nunButtonSprite) return nunButtonSprite;
            nunButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.DemonNunButton.png", 90f);
            return nunButtonSprite;
        }

        public static void clearAndReload() {
            demon = null;
            bitten = null;
            targetNearNun = false;
            localPlacedNun = false;
            currentTarget = null;
            delay = CustomOptionHolder.demonKillDelay.getFloat();
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
            canKillNearNun = CustomOptionHolder.demonCanKillNearNuns.getBool();
        }
    }
    public static class Janitor
    {
        public static bool dragginBody = false;
        public static PlayerControl janitor;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;
        public static byte bodyId = 0;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.JanitorCleanButton.png", 90f);
            return buttonSprite;
        }

        private static Sprite buttonDragSprite;
        public static Sprite getDragButtonSprite() {
            if (buttonDragSprite) return buttonDragSprite;
            buttonDragSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.JanitorDragBodyButton.png", 90f);
            return buttonDragSprite;
        }

        private static Sprite buttonMoveSprite;
        public static Sprite getMoveBodyButtonSprite() {
            if (buttonMoveSprite) return buttonMoveSprite;
            buttonMoveSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.JanitorMoveBodyButton.png", 90f);
            return buttonMoveSprite;
        }

        public static void clearAndReload() {
            janitor = null;
            cooldown = CustomOptionHolder.janitorCooldown.getFloat();
            dragginBody = false;
            bodyId = 0;
        }
        public static void janitorResetValuesAtDead() {
            // Restore janitor values when dead
            dragginBody = false;
            bodyId = 0;
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                GameObject vent = GameObject.Find("LowerCentralVent");
                vent.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    public static class Illusionist
    {
        public static PlayerControl illusionist;
        public static Color color = Palette.ImpostorRed;
        public static float placeHatCooldown = 30f;

        private static Sprite placeHatButtonSprite;
        private static Sprite illusionistVentButtonSprite;
        private static Sprite lightOutButtonSprite;
        public static float backUpduration = 10f;

        public static float lightsOutCooldown = 30f;
        public static float lightsOutDuration = 10f;
        public static float lightsOutTimer = 0f;

        public static Sprite getPlaceHatButtonSprite() {
            if (placeHatButtonSprite) return placeHatButtonSprite;
            placeHatButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.IllusionistPlaceHatButton.png", 90f);
            return placeHatButtonSprite;
        }

        public static Sprite getIllusionistVentButtonSprite() {
            if (illusionistVentButtonSprite) return illusionistVentButtonSprite;
            illusionistVentButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.IllusionistVentButton.png", 90f);
            return illusionistVentButtonSprite;
        }

        public static Sprite getLightsOutButtonSprite() {
            if (lightOutButtonSprite) return lightOutButtonSprite;
            lightOutButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.IllusionistLightsOutButton.png", 90f);
            return lightOutButtonSprite;
        }

        public static void clearAndReload() {
            illusionist = null;
            placeHatCooldown = CustomOptionHolder.illusionistPlaceHatCooldown.getFloat();
            lightsOutTimer = 0f;
            lightsOutCooldown = CustomOptionHolder.illusionistLightsOutCooldown.getFloat();
            lightsOutDuration = CustomOptionHolder.illusionistLightsOutDuration.getFloat();
            Hats.UpdateStates(); 
            backUpduration = lightsOutDuration;
        }

    }

    public static class Manipulator
    {

        public static PlayerControl manipulator;
        public static Color color = Palette.ImpostorRed;

        public static PlayerControl currentTarget;
        public static PlayerControl manipulatedVictim;
        public static PlayerControl manipulatedVictimTarget;

        public static float cooldown = 30f;
        public static float manipulatedVictimTimer = 21f;
        public static TMPro.TMP_Text manipulatedVictimTimerCountButtonText;

        private static Sprite manipulateKillOrDieButtonSprite;
        private static Sprite manipulateKillButtonSprite;

        public static Sprite getKillOrDieButtonSprite() {
            if (manipulateKillOrDieButtonSprite) return manipulateKillOrDieButtonSprite;
            manipulateKillOrDieButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ManipulatorKillOrDieButton.png", 90f);
            return manipulateKillOrDieButtonSprite;
        }

        public static Sprite getManipulateKillButtonSprite() {
            if (manipulateKillButtonSprite) return manipulateKillButtonSprite;
            manipulateKillButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ManipulatorKillButton.png", 90f);
            return manipulateKillButtonSprite;
        }

        public static void clearAndReload() {
            manipulator = null;
            currentTarget = null;
            manipulatedVictim = null;
            manipulatedVictimTarget = null;
            manipulatedVictimTimer = 21f;
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
        }

        public static void resetManipulate() {
            HudManagerStartPatch.manipulatorManipulateButton.Timer = HudManagerStartPatch.manipulatorManipulateButton.MaxTimer;
            HudManagerStartPatch.manipulatorManipulateButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            currentTarget = null;
            manipulatedVictim = null;
            manipulatedVictimTarget = null;
            manipulatedVictimTimer = 21f;
        }
    }

    public static class Bomberman
    {
        public static PlayerControl bomberman;
        public static Color color = Palette.ImpostorRed;
        public static float bombCooldown = 30f;
        public static float bombDuration = 10f;
        public static float bombTimer = 0f;
        public static float selfbombDuration = 10f;
        public static bool activeBomb = false;
        public static bool triggerBombExploded = false;
        public static int currentBombNumber = 0;
        public static GameObject bombObject = null;
        public static GameObject bombArea = null;

        private static Sprite selfbombButtonSprite;
        public static Sprite getSelfBombButtonSprite() {
            if (selfbombButtonSprite) return selfbombButtonSprite;
            selfbombButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.BombermanSelfBombButton.png", 90f);
            return selfbombButtonSprite;
        }
        
        private static Sprite bombButtonSprite;
        public static Sprite getBombButtonSprite() {
            if (bombButtonSprite) return bombButtonSprite;
            bombButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.BombermanBombButton.png", 90f);
            return bombButtonSprite;
        }

        private static Sprite bombDefuseButtonSprite;
        public static Sprite getBombDefuseButtonSprite() {
            if (bombDefuseButtonSprite) return bombDefuseButtonSprite;
            bombDefuseButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.BombermanDefuseButton.png", 90f);
            return bombDefuseButtonSprite;
        }

        public static void clearAndReload() {
            bomberman = null;
            bombTimer = 0f;
            bombCooldown = CustomOptionHolder.bombermanBombCooldown.getFloat();
            selfbombDuration = CustomOptionHolder.bombermanSelfBombDuration.getFloat();
            bombDuration = 60;
            activeBomb = false;
            triggerBombExploded = false;
            currentBombNumber = 0;
            bombObject = null;
            bombArea = null;
        }

    }

    public static class Chameleon
    {
        public static PlayerControl chameleon;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;
        public static float duration = 10f;

        public static float chameleonTimer = 0f;
        public static float backUpduration = 10f;

        private static Sprite buttonInvisibleSprite;
        public static Sprite getInvisibleButtonSprite() {
            if (buttonInvisibleSprite) return buttonInvisibleSprite;
            buttonInvisibleSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ChameleonInvisibleButton.png", 90f);
            return buttonInvisibleSprite;
        }

        public static void resetChameleon() {
            chameleonTimer = 0f;
            if (chameleon != null) {
                Helpers.alphaPlayer(false, chameleon.PlayerId);
            }
        }

        public static void clearAndReload() {
            chameleon = null;
            chameleonTimer = 0f;
            cooldown = CustomOptionHolder.chameleonCooldown.getFloat();
            duration = CustomOptionHolder.chameleonDuration.getFloat();
            backUpduration = duration;
        }

    }

    public static class Gambler
    {
        public static PlayerControl gambler;
        public static Color color = Palette.ImpostorRed;
        private static Sprite targetSprite;
        public static bool canCallEmergency = false;
        public static bool canKillThroughShield = true;

        public static Sprite getTargetSprite() {
            if (targetSprite) return targetSprite;
            targetSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.GamblerTargetIcon.png", 150f);
            return targetSprite;
        }

        public static void clearAndReload() {
            gambler = null;
            canCallEmergency = CustomOptionHolder.gamblerCanCallEmergency.getBool();
            canKillThroughShield = CustomOptionHolder.gamblerCanKillThroughShield.getBool();
        }
    }

    public static class Sorcerer
    {
        public static PlayerControl sorcerer;
        public static Color color = Palette.ImpostorRed;

        public static List<PlayerControl> spelledPlayers = new List<PlayerControl>();
        public static PlayerControl currentTarget;
        public static PlayerControl spellTarget;
        public static float cooldown = 30f;
        public static float spellDuration = 2f;
        public static float currentCooldownAddition = 0f;
        public static bool canCallEmergency = false;

        private static Sprite buttonSpellSprite;
        public static Sprite getSpellButtonSprite() {
            if (buttonSpellSprite) return buttonSpellSprite;
            buttonSpellSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SorcererSpellButton.png", 90f);
            return buttonSpellSprite;
        }

        private static Sprite spelledMeetingSprite;
        public static Sprite getSpelledMeetingSprite() {
            if (spelledMeetingSprite) return spelledMeetingSprite;
            spelledMeetingSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SorcererSpellButtonMeeting.png", 225f);
            return spelledMeetingSprite;
        }


        public static void clearAndReload() {
            sorcerer = null;
            spelledPlayers = new List<PlayerControl>();
            currentTarget = spellTarget = null;
            cooldown = CustomOptionHolder.sorcererCooldown.getFloat();
            currentCooldownAddition = CustomOptionHolder.sorcererCooldown.getFloat();
            spellDuration = CustomOptionHolder.sorcererSpellDuration.getFloat();
            canCallEmergency = CustomOptionHolder.sorcererCanCallEmergency.getBool();
        }
    }

    public static class Medusa
    {
        public static PlayerControl medusa;
        public static Color color = Palette.ImpostorRed;
        public static List<PlayerControl> petrifiedPlayers = new List<PlayerControl>();

        public static float cooldown = 20f;
        public static float delay = 10f;

        public static PlayerControl currentTarget;
        public static PlayerControl petrified;
        public static PlayerControl localPetrifiedPlayer;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.MedusaPetrifyButton.png", 90f);
            return buttonSprite;
        }

        private static Sprite buttonUnpetrifySprite;
        public static Sprite getButtonUnpetrifySprite() {
            if (buttonUnpetrifySprite) return buttonUnpetrifySprite;
            buttonUnpetrifySprite = Helpers.loadSpriteFromResources("LasMonjas.Images.MedusaUnpetrifyButton.png", 90f);
            return buttonUnpetrifySprite;
        }

        public static void clearAndReload() {
            medusa = null;
            petrifiedPlayers = new List<PlayerControl>(); 
            petrified = null;
            localPetrifiedPlayer = null;
            currentTarget = null;
            cooldown = CustomOptionHolder.medusaCooldown.getFloat();
            delay = CustomOptionHolder.medusaDelay.getFloat();
        }

        public static void ResetMedusa() {
            foreach (PlayerControl depetrifiedPlayer in petrifiedPlayers) {
                if (PlayerInCache.LocalPlayer.PlayerControl == depetrifiedPlayer) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.hackerHack, false, 100f);
                }
                depetrifiedPlayer.moveable = true;

                GameObject petrifyZone = GameObject.Find(depetrifiedPlayer.name + "petrifyZone");
                if (petrifyZone != null) {
                    UnityEngine.Object.Destroy(petrifyZone);
                }
            }
            petrifiedPlayers = new List<PlayerControl>();
        }
    }
    public static class Hypnotist
    {

        public static PlayerControl hypnotist;
        public static Color color = Palette.ImpostorRed;
        public static List<PlayerControl> hypnotizedPlayers = new List<PlayerControl>();

        public static float cooldown = 0;
        public static float currentSpiralNumber = 0;
        public static float numberOfSpirals = 0;
        public static float spiralDuration = 0;

        public static float messageTimer = 0f;

        public static List<GameObject> objectsCantPlaceTraps = new List<GameObject>();

        public static TMPro.TMP_Text trapsCounterButtonText;
        
        private static Sprite spiralSprite;
        public static Sprite getSpiralSprite() {
            if (spiralSprite) return spiralSprite;
            spiralSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.HypnotistReverseButton.png", 90f);
            return spiralSprite;
        }

        public static void clearAndReload() {
            hypnotist = null;
            currentSpiralNumber = 0;
            cooldown = CustomOptionHolder.hypnotistCooldown.getFloat();
            numberOfSpirals = CustomOptionHolder.hypnotistNumberOfSpirals.getFloat();
            spiralDuration = CustomOptionHolder.hypnotistSpiralsDuration.getFloat();
            messageTimer = 0f;
            objectsCantPlaceTraps.Clear();
            hypnotizedPlayers.Clear();
        }
    }
    
    public static class Archer
    {
        public static PlayerControl archer;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;

        public static float shotSize = 2f;
        public static float shotRange = 20f;
        public static float noticeRange = 20f;
        public static float AimAssistDelay = 2f;
        public static float AimAssistDuration = 10f;

        public static float mouseArcherAngle = 0f;
        public static bool weaponEquiped = false;
        public static float weaponDuration = 0f;

        public static Dictionary<byte, SpriteRenderer> Guides = new Dictionary<byte, SpriteRenderer>();

        public static GameObject bow = null;
        private static Sprite bowSprite = null;

        private static Sprite guideSprite = null;


        public static Sprite getGuideSprite() {
            if (guideSprite) return guideSprite;
            guideSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ArcherGuide.png", 100f);
            return guideSprite;
        }
        public static Sprite getBowSprite() {
            if (bowSprite) return bowSprite;
            bowSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ArcherBow.png", 100f);
            return bowSprite;
        }

        private static Sprite buttonPickBowprite;
        public static Sprite getPickBowButtonSprite() {
            if (buttonPickBowprite) return buttonPickBowprite;
            buttonPickBowprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ArcherPickBowButton.png", 90f);
            return buttonPickBowprite;
        }

        private static Sprite buttonHideBowprite;
        public static Sprite getHideBowButtonSprite() {
            if (buttonHideBowprite) return buttonHideBowprite;
            buttonHideBowprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ArcherHideBowButton.png", 90f);
            return buttonHideBowprite;
        }

        private static Sprite archerWarning = null;
        public static Sprite getArcherWarningSprite() {
            if (archerWarning) return archerWarning;
            archerWarning = Helpers.loadSpriteFromResources("LasMonjas.Images.ArcherWarning.png", 200f);
            return archerWarning;
        }

        public static void clearAndReload() {
            archer = null;
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
            shotSize = 2f;
            shotRange = CustomOptionHolder.archerShotRange.getFloat();
            noticeRange = CustomOptionHolder.archerNoticeRange.getFloat();
            AimAssistDelay = 2f;
            AimAssistDuration = CustomOptionHolder.archerAimAssistDuration.getFloat();
            mouseArcherAngle = 0f;
            weaponEquiped = false;
            weaponDuration = 0f;
            Guides = new Dictionary<byte, SpriteRenderer>();
            bow = null;
        }

        public static PlayerControl GetShootPlayer(float shotSize, float effectiveRange) {
            PlayerControl result = null;
            float num = effectiveRange;
            Vector3 pos;
            float mouseAngle = mouseArcherAngle;
            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    result = player;
                }
            }
            return result;
        }

    }

    public class Plumber
    {
        public static PlayerControl plumber;
        public static Color color = Palette.ImpostorRed;

        public static float cooldown = 30f;
        public static List<Vent> Vents = new List<Vent>();
        public static Vector2 VentSize { get; set; }

        public static Sprite buttonSprite;
        public static Sprite getPlumberButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PlumberMakeVentButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            plumber = null;
            cooldown = CustomOptionHolder.plumberCooldown.getFloat();
            Vents = new List<Vent>();
        }
    }

    public static class Librarian
    {
        public static PlayerControl librarian;
        public static Color color = Palette.ImpostorRed;

        public static PlayerControl currentTarget;
        public static PlayerControl targetLibrary;
        public static float cooldown = 30f;

        public static TMPro.TMP_Text targetNameButtonText;

        private static Sprite overlaySprite;
        public static Sprite getLibrarianOverlaySprite() {
            if (overlaySprite) return overlaySprite;
            overlaySprite = Helpers.loadSpriteFromResources("LasMonjas.Images.LibrarianOverlay.png", 100f);
            return overlaySprite;
        }

        private static Sprite librarianButtonSprite;

        public static Sprite getLibrarianButtonSprite() {
            if (librarianButtonSprite) return librarianButtonSprite;
            librarianButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.LibrarianSilenceButton.png", 90f);
            return librarianButtonSprite;
        }

        public static void clearAndReload() {
            librarian = null;
            currentTarget = null;
            targetLibrary = null;
            cooldown = CustomOptionHolder.librarianCooldown.getFloat();
        }
    }

    public static class Renegade
    {
        public static PlayerControl renegade;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);
        public static PlayerControl fakeMinion;
        public static PlayerControl currentTarget;
        public static List<PlayerControl> formerRenegades = new List<PlayerControl>();

        public static float cooldown = 30f;
        public static bool canUseVents = true;
        public static bool canRecruitMinion = true;
        public static Sprite buttonSprite;
        public static bool usedRecruit = false;

        public static Sprite getMinionButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.RenegadeRecruitButton.png", 90f);
            return buttonSprite;
        }

        public static void removeCurrentRenegade() {
            if (!formerRenegades.Any(x => x.PlayerId == renegade.PlayerId)) formerRenegades.Add(renegade);
            renegade = null;
            currentTarget = null;
            fakeMinion = null;
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
        }

        public static void clearAndReload() {
            renegade = null;
            currentTarget = null;
            fakeMinion = null;
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
            canUseVents = CustomOptionHolder.renegadeCanUseVents.getBool();
            canRecruitMinion = CustomOptionHolder.renegadeCanRecruitMinion.getBool();
            usedRecruit = false;
            formerRenegades.Clear();
        }

    }

    public static class Minion
    {
        public static PlayerControl minion;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static PlayerControl currentTarget;

        public static float cooldown = 30f;

        public static void clearAndReload() {
            minion = null;
            currentTarget = null;
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
        }
    }

    public static class BountyHunter
    {
        public static PlayerControl bountyhunter;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static List<PlayerControl> possibleTargets = new List<PlayerControl>();

        public static float killCooldown = 30f;

        public static PlayerControl currentTarget;

        public static PlayerControl hasToKill;

        public static string rolName;

        public static bool triggerBountyHunterWin = false;

        public static bool usedTarget;

        public static TMPro.TMP_Text targetNameButtonText;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.BountyHunterSetTargetButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            bountyhunter = null;
            currentTarget = null;
            hasToKill = null;
            killCooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
            triggerBountyHunterWin = false;
            rolName = "";
            usedTarget = false;
            possibleTargets = new List<PlayerControl>();
        }
    }

    public static class Trapper
    {
        public static PlayerControl trapper;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static float cooldown = 30f;

        public static int numberOfMines;

        public static int numberOfTraps;

        public static float durationOfMines;

        public static float durationOfTraps;

        public static int currentMineNumber = 0;

        public static int currentTrapNumber = 0;

        public static PlayerControl mined;

        public static PlayerControl currentTarget;

        private static Sprite buttonTrapSprite;
        public static Sprite getTrapButtonSprite() {
            if (buttonTrapSprite) return buttonTrapSprite;
            buttonTrapSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TrapperTrapButton.png", 90f);
            return buttonTrapSprite;
        }

        private static Sprite buttonMineSprite;
        public static Sprite getMineButtonSprite() {
            if (buttonMineSprite) return buttonMineSprite;
            buttonMineSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TrapperMineButton.png", 90f);
            return buttonMineSprite;
        }

        public static void clearAndReload() {
            trapper = null;
            cooldown = CustomOptionHolder.trapperCooldown.getFloat();
            numberOfMines = (int)CustomOptionHolder.trapperMineNumber.getFloat();
            numberOfTraps = (int)CustomOptionHolder.trapperTrapNumber.getFloat();
            durationOfMines = CustomOptionHolder.trapperMineDuration.getFloat();
            durationOfTraps = CustomOptionHolder.trapperTrapDuration.getFloat();
            currentMineNumber = 0;
            currentTrapNumber = 0;
            mined = null;
            currentTarget = null;
        }
    }

    public static class Yinyanger
    {
        public static PlayerControl yinyanger;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static float cooldown = 30f;

        public static PlayerControl currentTarget;

        public static PlayerControl yinyedplayer;

        public static PlayerControl yangyedplayer;

        public static bool usedYined = false;

        public static bool usedYanged = false;

        public static bool colision = false;

        public static TMPro.TMP_Text yinedButtonText;

        public static TMPro.TMP_Text yanedButtonText;


        private static Sprite buttonSpriteYang;
        public static Sprite getYangButtonSprite() {
            if (buttonSpriteYang) return buttonSpriteYang;
            buttonSpriteYang = Helpers.loadSpriteFromResources("LasMonjas.Images.YinyangerYangButton.png", 90f);
            return buttonSpriteYang;
        }

        private static Sprite buttonSpriteYing;
        public static Sprite getYinButtonSprite() {
            if (buttonSpriteYing) return buttonSpriteYing;
            buttonSpriteYing = Helpers.loadSpriteFromResources("LasMonjas.Images.YinyangerYinButton.png", 90f);
            return buttonSpriteYing;
        }

        private static Sprite buttonSpriteYinYang;
        public static Sprite getYinYangButtonSprite() {
            if (buttonSpriteYinYang) return buttonSpriteYinYang;
            buttonSpriteYinYang = Helpers.loadSpriteFromResources("LasMonjas.Images.YinyangerYinYangButton.png", 90f);
            return buttonSpriteYinYang;
        }

        public static void clearAndReload() {
            yinyanger = null;
            cooldown = CustomOptionHolder.yinyangerCooldown.getFloat();
            currentTarget = null;
            yinyedplayer = null;
            yangyedplayer = null;
            usedYined = false;
            usedYanged = false;
            colision = false;
        }
        public static void resetYined() {
            yinyedplayer = null;
            usedYined = false;
            yinedButtonText.text = "";
        }
        public static void resetYanged() {
            yangyedplayer = null;
            usedYanged = false;
            yanedButtonText.text = "";
        }
    }

    public static class Challenger
    {
        public static PlayerControl challenger;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static float cooldown = 30f;

        public static float duration = 10f;

        public static PlayerControl currentTarget;

        public static bool challengerRock = false;

        public static bool challengerPaper = false;

        public static bool challengerScissors = false;

        public static PlayerControl rivalPlayer;

        public static bool rivalRock = false;

        public static bool rivalPaper = false;

        public static bool rivalScissors = false;

        public static bool isDueling = false;

        public static bool onlyOneFinishDuel = true;

        public static float duelDuration = 30f;

        public static bool timeOutDuel = false;
        public static bool triggerChallengerWin = false;
        public static float duelKills = 0;
        public static float neededKills = 0;
        public static TMPro.TMP_Text challengerDuelButtonText;

        private static Sprite buttonChallengeSprite;
        public static Sprite getChallengeButtonSprite() {
            if (buttonChallengeSprite) return buttonChallengeSprite;
            buttonChallengeSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ChallengerChallengeButton.png", 90f);
            return buttonChallengeSprite;
        }

        private static Sprite buttonRockSprite;
        public static Sprite getRockButtonSprite() {
            if (buttonRockSprite) return buttonRockSprite;
            buttonRockSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ChallengerRockButton.png", 90f);
            return buttonRockSprite;
        }

        private static Sprite buttonPaperSprite;
        public static Sprite getPaperButtonSprite() {
            if (buttonPaperSprite) return buttonPaperSprite;
            buttonPaperSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ChallengerPaperButton.png", 90f);
            return buttonPaperSprite;
        }

        private static Sprite buttonScissorsSprite;
        public static Sprite getScissorsButtonSprite() {
            if (buttonScissorsSprite) return buttonScissorsSprite;
            buttonScissorsSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ChallengerScissorsButton.png", 90f);
            return buttonScissorsSprite;
        }

        public static void clearAndReload() {
            challenger = null;
            cooldown = CustomOptionHolder.challengerCooldown.getFloat();
            duration = 10f;
            currentTarget = null;
            challengerRock = false;
            challengerPaper = false;
            challengerScissors = false;
            rivalPlayer = null;
            rivalRock = false;
            rivalPaper = false;
            rivalScissors = false;
            isDueling = false;
            onlyOneFinishDuel = true;
            duelDuration = 30f;
            timeOutDuel = false;
            triggerChallengerWin = false;
            duelKills = 0;
            neededKills = CustomOptionHolder.challengerKillsForWin.getFloat();
        }

        public static void ResetValues() {
            challengerRock = false;
            challengerPaper = false;
            challengerScissors = false;
            rivalPlayer = null;
            rivalRock = false;
            rivalPaper = false;
            rivalScissors = false;
            isDueling = false;
            onlyOneFinishDuel = true;
            duelDuration = 30f;
            if (Jailer.prisonPlayer != null) {
                switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                    // Skeld
                    case 0:
                        if (LasMonjas.activatedSensei) {
                            Jailer.prisonPlayer.transform.position = new Vector3(-0.6f, 3.5f, Jailer.prisonPlayer.transform.position.z);
                        }
                        else if (LasMonjas.activatedDleks) {
                            Jailer.prisonPlayer.transform.position = new Vector3(0.75f, 5.25f, Jailer.prisonPlayer.transform.position.z);
                        }
                        else {
                            Jailer.prisonPlayer.transform.position = new Vector3(-0.75f, 5.25f, Jailer.prisonPlayer.transform.position.z);
                        }
                        break;
                    // MiraHQ
                    case 1:
                        Jailer.prisonPlayer.transform.position = new Vector3(25.5f, 4.75f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Polus
                    case 2:
                        Jailer.prisonPlayer.transform.position = new Vector3(17.15f, -17.15f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Dleks
                    case 3:
                        Jailer.prisonPlayer.transform.position = new Vector3(0.75f, 5.25f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Airship
                    case 4:
                        Jailer.prisonPlayer.transform.position = new Vector3(16.25f, 15.25f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Fungle
                    case 5:
                        Jailer.prisonPlayer.transform.position = new Vector3(-5.5f, 1f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Submerged
                    case 6:
                        if (Jailer.prisonPlayer.transform.position.y > 0) {
                            Jailer.prisonPlayer.transform.position = new Vector3(-4.3f, 13.5f, Jailer.prisonPlayer.transform.position.z);
                        }
                        else {
                            Jailer.prisonPlayer.transform.position = new Vector3(1.85f, -26.65f, Jailer.prisonPlayer.transform.position.z);
                        }
                        break;
                }
                Jailer.prisonPlayer = null;
                Jailer.usedJail = false;
            }
        }
    }

    public static class Ninja
    {
        public static PlayerControl ninja;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static float cooldown = 30f;

        public static PlayerControl currentTarget;

        public static PlayerControl markedTarget;

        public static TMPro.TMP_Text targetNameButtonText;

        private static Sprite buttonMarkSprite;
        public static Sprite getMarkSprite() {
            if (buttonMarkSprite) return buttonMarkSprite;
            buttonMarkSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.NinjaTargetButton.png", 90f);
            return buttonMarkSprite;
        }

        private static Sprite buttonMarkedSprite;
        public static Sprite getMarkedSprite() {
            if (buttonMarkedSprite) return buttonMarkedSprite;
            buttonMarkedSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.NinjaKillButton.png", 90f);
            return buttonMarkedSprite;
        }

        public static void clearAndReload() {
            ninja = null;
            currentTarget = null;
            markedTarget = null;
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
        }
    }

    public static class Berserker
    {
        public static PlayerControl berserker;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static float cooldown = 30f;
        public static float timeToKill = 30f;
        public static float backupTimeToKill = 30f;
        public static bool killedFirstTime = false;

        public static PlayerControl currentTarget;

        public static TMPro.TMP_Text berserkerCountButtonText;

        public static void clearAndReload() {
            berserker = null;
            currentTarget = null;
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
            timeToKill = CustomOptionHolder.berserkerTimeToKill.getFloat();
            killedFirstTime = false;
            backupTimeToKill = timeToKill;
        }
    }
    public static class Yandere
    {
        public static PlayerControl yandere;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static PlayerControl currentTarget;
        public static PlayerControl stalkTarget;
        public static PlayerControl target;

        public static float killCooldown = 30f;
        public static float stareCooldown = 30f;
        public static float stareTimes = 5f;
        public static float currenStareTimes = 0f;
        public static float stareDuration = 3f;
        public static bool usedTarget;
        public static bool rampageMode;
        public static bool triggerYandereWin = false;

        public static TMPro.TMP_Text yandereTargetButtonText;
        public static TMPro.TMP_Text yandereKillButtonText;
        public static List<PlayerControl> possibleTargets = new List<PlayerControl>();

        private static Sprite buttonTargetSprite;
        public static Sprite getTargetSprite() {
            if (buttonTargetSprite) return buttonTargetSprite;
            buttonTargetSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.YandereTarget.png", 90f);
            return buttonTargetSprite;
        }

        private static Sprite buttonStareSprite;
        public static Sprite getStareSprite() {
            if (buttonStareSprite) return buttonStareSprite;
            buttonStareSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.YandereStalk.png", 90f);
            return buttonStareSprite;
        }

        public static void clearAndReload() {
            yandere = null;
            currentTarget = null;
            target = null;
            killCooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
            stareCooldown = CustomOptionHolder.yandereCooldown.getFloat();
            stareTimes = CustomOptionHolder.yandereStareTimes.getFloat();
            stareDuration = CustomOptionHolder.yandereStareDuration.getFloat();
            possibleTargets = new List<PlayerControl>();
            usedTarget = false;
            rampageMode = false;
            currenStareTimes = 0;
            stalkTarget = null;
            triggerYandereWin = false;
        }
    }

    public static class Stranded
    {
        public static PlayerControl stranded;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static List<Vector3> susBoxPositions = new List<Vector3>();
        public static GameObject selectedBox = null;
        public static GameObject currentBox = null;
        public static List<GameObject> groundItems = new List<GameObject>();
        public static byte foundBox = 0;
        public static bool spawnBoxOnCustomSkeld = false;
        public static bool spawnBoxOnDleks = false;
        public static PlayerControl currentTarget;
        public static float killCooldown = 30f;                
        public static bool triggerStrandedWin = false;
        public static int howManyKills = 0;
        public static int storedAmmo = 0;
        public static bool canVent = false;
        public static bool canTurnInvisible = false;
        public static bool isInvisible = false;
        public static float invisibleTimer = 0;

        public static TMPro.TMP_Text strandedSearchButtonText;
        public static TMPro.TMP_Text strandedKillButtonText;

        private static Sprite buttonSurvivorTakeBox;

        public static Sprite getSurvivorTakeBoxButtonSprite() {
            if (buttonSurvivorTakeBox) return buttonSurvivorTakeBox;
            buttonSurvivorTakeBox = Helpers.loadSpriteFromResources("LasMonjas.Images.StrandedSearchButton.png", 90f);
            return buttonSurvivorTakeBox;
        }

        private static Sprite buttonInvisibleSprite;
        public static Sprite getInvisibleButtonSprite() {
            if (buttonInvisibleSprite) return buttonInvisibleSprite;
            buttonInvisibleSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.StrandedInvisibleButton.png", 90f);
            return buttonInvisibleSprite;
        }

        public static void resetStranded() {
            invisibleTimer = 0f;
            isInvisible = false;
            HudManagerStartPatch.strandedInvisibleButton.Timer = HudManagerStartPatch.strandedInvisibleButton.MaxTimer;
            HudManagerStartPatch.strandedInvisibleButton.isEffectActive = false;
            HudManagerStartPatch.strandedInvisibleButton.actionButton.cooldownTimerText.color = Palette.EnabledColor; 
            if (stranded != null) {
                Helpers.alphaPlayer(false, stranded.PlayerId);
            }
        }

        public static void clearAndReload() {
            susBoxPositions.Clear();
            groundItems.Clear();
            stranded = null;
            currentTarget = null;
            killCooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
            foundBox = 0;
            selectedBox = null;
            currentBox = null;
            triggerStrandedWin = false;
            spawnBoxOnCustomSkeld = CustomOptionHolder.activateSenseiMap.getBool();
            spawnBoxOnDleks = CustomOptionHolder.activateDleksMap.getBool();
            howManyKills = 0;
            storedAmmo = 0;
            canVent = false;
            canTurnInvisible = false;
            isInvisible = false;
            invisibleTimer = 0;

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    if (spawnBoxOnCustomSkeld) {
                        susBoxPositions.Add(new Vector3(-5.25f, -0.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-4.5f, 1.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.85f, 5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-20, -1.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16f, -1.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-17.85f, 5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18.85f, -8f, 0.4f));
                        susBoxPositions.Add(new Vector3(-14.25f, -12.35f, 0.4f));
                        susBoxPositions.Add(new Vector3(-10.75f, -9f, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.5f, -12.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-7.75f, -11.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(1f, -12f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.5f, -14f, 0.4f));
                        susBoxPositions.Add(new Vector3(-8.5f, -0.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(12.5f, -0.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(10f, -2.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.75f, 5f, 0.4f));
                        susBoxPositions.Add(new Vector3(5f, -9f, 0.4f));
                        susBoxPositions.Add(new Vector3(5f, -5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.35f, 8f, 0.4f));
                        susBoxPositions.Add(new Vector3(7.5f, -1f, 0.4f));
                        susBoxPositions.Add(new Vector3(4.6f, -1f, 0.4f));
                        susBoxPositions.Add(new Vector3(1f, 3f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.75f, -4f, 0.4f));
                        susBoxPositions.Add(new Vector3(-13.5f, -1.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.85f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-10.35f, 2.85f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.75f, 3.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.75f, 8f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.75f, 10.85f, 0.4f));
                    }
                    else if (spawnBoxOnDleks) {
                        susBoxPositions.Add(new Vector3(15.75f, -0.9f, 0.4f));
                        susBoxPositions.Add(new Vector3(7.85f, -11.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-4.5f, -9.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.75f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-1.75f, -3.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(18.5f, -9.65f, 0.4f));
                        susBoxPositions.Add(new Vector3(3.5f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16.75f, -6f, 0.4f));
                        susBoxPositions.Add(new Vector3(3f, -10f, 0.4f));
                        susBoxPositions.Add(new Vector3(21.5f, -2.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(20f, -4f, 0.4f));
                        susBoxPositions.Add(new Vector3(21.5f, -8f, 0.4f));
                        susBoxPositions.Add(new Vector3(16.9f, -5.4f, 0.4f));
                        susBoxPositions.Add(new Vector3(13.5f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(18f, 2.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(10.1f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.4f, -4.65f, 0.4f));
                        susBoxPositions.Add(new Vector3(18.5f, -13.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(12.1f, -14.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.5f, -8.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(3.55f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.3f, -7.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-2.5f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-2.15f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.4f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-11.75f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.2f, -4.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.5f, 0f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-1.75f, 5.5f, 0.4f));
                    }
                    else {
                        susBoxPositions.Add(new Vector3(-15.75f, -0.9f, 0.4f));
                        susBoxPositions.Add(new Vector3(-7.85f, -11.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(4.5f, -9.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(5.75f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(1.75f, -3.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18.5f, -9.65f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.5f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(16.75f, -6f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3f, -10f, 0.4f));
                        susBoxPositions.Add(new Vector3(-21.5f, -2.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-20f, -4f, 0.4f));
                        susBoxPositions.Add(new Vector3(-21.5f, -8f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16.9f, -5.4f, 0.4f));
                        susBoxPositions.Add(new Vector3(-13.5f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18f, 2.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-10.1f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.4f, -4.65f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18.5f, -13.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-12.1f, -14.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.5f, -8.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.55f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.3f, -7.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(2.5f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(2.15f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.4f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(11.75f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(5.2f, -4.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.5f, 0f, 0.4f));
                        susBoxPositions.Add(new Vector3(6f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(1.75f, 5.5f, 0.4f));
                    }
                    break;
                case 1:
                    susBoxPositions.Add(new Vector3(-3.5f, 3.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.4f, -1.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.55f, -1.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.75f, -1.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(15.4f, -0.85f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.5f, 2.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(16f, 4f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.25f, 1.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(6f, 6f, 0.4f));
                    susBoxPositions.Add(new Vector3(6f, 12f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.8f, 10.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.7f, 12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.75f, 13.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.25f, 10.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.85f, 11.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.85f, 20.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(15.75f, 17.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(14.75f, 20.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, 20.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(21f, 17.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.5f, 23.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(22.15f, 23.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.15f, 24.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.25f, 24.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.5f, 4.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.5f, 2.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, 4.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, -1.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.5f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(28.25f, 0.25f, 0.4f));
                    break;
                case 2:
                    susBoxPositions.Add(new Vector3(13.75f, -9.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(2.75f, -11.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(5.5f, -11.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.75f, -12.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(24f, -12.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.25f, -16.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.45f, -14.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.85f, -19f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.35f, -17.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.35f, -20f, 0.4f));
                    susBoxPositions.Add(new Vector3(2.25f, -23.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(8f, -24.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.5f, -25f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, -25f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.75f, -21.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.65f, -14.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(24.5f, -17f, 0.4f));
                    susBoxPositions.Add(new Vector3(31f, -20.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(38.35f, -20.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(36.5f, -21.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(35.5f, -19.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(32f, -15.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(27.75f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(30f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(36.4f, -6.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(40.35f, -8f, 0.4f));
                    susBoxPositions.Add(new Vector3(39.1f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(31.5f, -9.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.5f, -9.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.5f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(15.75f, -21.75f, 0.4f));
                    break;
                case 3:
                    susBoxPositions.Add(new Vector3(15.75f, -0.9f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.85f, -11.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-4.5f, -9.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.75f, -15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-1.75f, -3.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(18.5f, -9.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.5f, 1.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-16.75f, -6f, 0.4f));
                    susBoxPositions.Add(new Vector3(3f, -10f, 0.4f));
                    susBoxPositions.Add(new Vector3(21.5f, -2.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, -4f, 0.4f));
                    susBoxPositions.Add(new Vector3(21.5f, -8f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.9f, -5.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.5f, -6.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(18f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.1f, 1.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.4f, -4.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(18.5f, -13.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.1f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.5f, -8.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.55f, -15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-6.3f, -7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-2.5f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-2.15f, -15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-9.4f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-11.75f, -6.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.2f, -4.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-9.5f, 0f, 0.4f));
                    susBoxPositions.Add(new Vector3(-6f, 1.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-1.75f, 5.5f, 0.4f));
                    break;
                case 4:
                    susBoxPositions.Add(new Vector3(12.5f, -2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.25f, -2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.25f, 2f, 0.4f));
                    susBoxPositions.Add(new Vector3(15.25f, 2f, 0.4f));
                    susBoxPositions.Add(new Vector3(24f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.75f, 0.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.5f, 6f, 0.4f));
                    susBoxPositions.Add(new Vector3(-23.25f, -0.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-7.5f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-7.5f, -1f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14.25f, 1.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14.25f, -4.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14.25f, -8.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-13.75f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-10.25f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.5f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(5.75f, -10.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(8.25f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.25f, -6.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.25f, -4f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.25f, -6.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.25f, -8.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(29f, -6f, 0.4f));
                    susBoxPositions.Add(new Vector3(29f, -1.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(37.25f, -3.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(33.75f, 7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(29.25f, 7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(24.75f, 7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, 11.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(4.25f, 8.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-8.75f, 12.5f, 0.4f));
                    break;
                case 5:
                    susBoxPositions.Add(new Vector3(-16.5f, 7.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-20.4f, -0.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-16f, -0.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14f, -7.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(-21.5f, -7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-8.35f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-7.3f, -11f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.25f, -8.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(-3.33f, -10.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.45f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.5f, -12f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.65f, -10f, 0.4f));
                    susBoxPositions.Add(new Vector3(-1f, -6f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.5f, -1.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(-0.45f, 1.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.25f, 4.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(23.85f, 13.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, 11f, 0.4f));
                    susBoxPositions.Add(new Vector3(18.5f, 7.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.25f, 10f, 0.4f));
                    susBoxPositions.Add(new Vector3(7f, 4.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(8.15f, 1.2f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.25f, 1.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(19f, 0.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.75f, -6.85f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.25f, -12f, 0.4f));
                    susBoxPositions.Add(new Vector3(11f, -7f, 0.4f));
                    susBoxPositions.Add(new Vector3(-8.85f, 5.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-11.35f, -4.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(-0.75f, -10.25f, 0.4f));
                    break;
                case 6:
                    susBoxPositions.Add(new Vector3(0, -33.75f, -1f));
                    susBoxPositions.Add(new Vector3(5.25f, -33.5f, -1f));
                    susBoxPositions.Add(new Vector3(7.8f, -39.5f, -1f));
                    susBoxPositions.Add(new Vector3(2.3f, -39.5f, -1f));
                    susBoxPositions.Add(new Vector3(-10.95f, -39f, -1f));
                    susBoxPositions.Add(new Vector3(-6.75f, -42.75f, -1f));
                    susBoxPositions.Add(new Vector3(-13.75f, -34.25f, -1f));
                    susBoxPositions.Add(new Vector3(-11.5f, -31.15f, -1f));
                    susBoxPositions.Add(new Vector3(-13.75f, -27.75f, -1f));
                    susBoxPositions.Add(new Vector3(-4.15f, -34f, -1f));
                    susBoxPositions.Add(new Vector3(13f, -25.25f, -1f));
                    susBoxPositions.Add(new Vector3(12.75f, -31.5f, -1f));
                    susBoxPositions.Add(new Vector3(4.75f, -26.75f, -1f));
                    susBoxPositions.Add(new Vector3(4f, -20.25f, -1f));
                    susBoxPositions.Add(new Vector3(7.7f, -23.25f, -1f));
                    susBoxPositions.Add(new Vector3(14.25f, 24.65f, -1f));
                    susBoxPositions.Add(new Vector3(5.75f, 31.5f, -1f));
                    susBoxPositions.Add(new Vector3(0f, 34f, -1f));
                    susBoxPositions.Add(new Vector3(4.5f, 8.4f, -1f));
                    susBoxPositions.Add(new Vector3(10.45f, 17.25f, -1f));
                    susBoxPositions.Add(new Vector3(5.75f, 20.25f, -1f));
                    susBoxPositions.Add(new Vector3(1f, 10f, -1f));
                    susBoxPositions.Add(new Vector3(1.35f, 14.5f, -1f));
                    susBoxPositions.Add(new Vector3(-6.4f, 14.15f, -1f));
                    susBoxPositions.Add(new Vector3(-6.7f, 10.25f, -1f));
                    susBoxPositions.Add(new Vector3(-15f, 18f, -1f));
                    susBoxPositions.Add(new Vector3(-12.15f, 20.25f, -1f));
                    susBoxPositions.Add(new Vector3(-10.25f, 25.5f, -1f));
                    susBoxPositions.Add(new Vector3(-4.75f, 28.45f, -1f));
                    susBoxPositions.Add(new Vector3(-12.25f, 31f, -1f));
                    break;
            }
            susBoxPositions.Shuffle();
        }
    }

    public static class Monja
    {
        public static bool awakened;
        public static float awakenTimer = 0f;
        public static PlayerControl monja;
        public static Color color = new Color32(79, 125, 0, byte.MaxValue);

        public static PlayerControl currentTarget;
        public static bool canAwake;
        public static bool isDeliveringItem;
        public static float killTimer;
        public static int deliveredItems;
        public static byte itemId;
        public static TMPro.TMP_Text killTimerButtonText;
        public static TMPro.TMP_Text objectCountButtonText;
        public static List<GameObject> objectList = new List<GameObject>();
        public static GameObject ritualObject = null;
        public static List<Vector3> itemListPositions = new List<Vector3>();
        public static bool spawnBoxOnCustomSkeld = false;
        public static bool spawnBoxOnDleks = false;
        public static GameObject item01 = null;
        public static GameObject item02 = null;
        public static GameObject item03 = null;
        public static GameObject item04 = null;
        public static GameObject item05 = null;
        public static GameObject monjaSprite = null;

        private static Sprite buttonSprite;
        public static Sprite getAwakeButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaRitual.png", 90f);
            return buttonSprite;
        }

        private static Sprite buttonSpriteDeliver;
        public static Sprite getDeliverButtonSprite() {
            if (buttonSpriteDeliver) return buttonSpriteDeliver;
            buttonSpriteDeliver = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaPlace.png", 90f);
            return buttonSpriteDeliver;
        }

        private static Sprite buttonSpritePick;
        public static Sprite getPickButtonSprite() {
            if (buttonSpritePick) return buttonSpritePick;
            buttonSpritePick = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaPick.png", 90f);
            return buttonSpritePick;
        }

        public static void clearAndReload() {
            monja = null;
            currentTarget = null;
            awakened = false;
            awakenTimer = 0;
            killTimer = 0f;
            deliveredItems = 0;
            canAwake = false;
            isDeliveringItem = false;
            itemId = 0;
            objectList = new List<GameObject>();
            ritualObject = null;
            itemListPositions.Clear();
            spawnBoxOnCustomSkeld = CustomOptionHolder.activateSenseiMap.getBool();
            spawnBoxOnDleks = CustomOptionHolder.activateDleksMap.getBool();
            item01 = null;
            item02 = null;
            item03 = null;
            item04 = null;
            item05 = null;
            monjaSprite = null;
            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    if (spawnBoxOnCustomSkeld) {
                        itemListPositions.Add(new Vector3(-5.25f, -0.75f, 0.4f));
                        itemListPositions.Add(new Vector3(-3.85f, 5.25f, 0.4f));
                        itemListPositions.Add(new Vector3(-20, -1.5f, 0.4f));
                        itemListPositions.Add(new Vector3(-17.85f, 5.25f, 0.4f));
                        itemListPositions.Add(new Vector3(-18.85f, -8f, 0.4f));
                        itemListPositions.Add(new Vector3(-14.25f, -12.35f, 0.4f));
                        itemListPositions.Add(new Vector3(-5.5f, -12.75f, 0.4f));
                        itemListPositions.Add(new Vector3(6.5f, -14f, 0.4f));
                        itemListPositions.Add(new Vector3(-8.5f, -0.5f, 0.4f));
                        itemListPositions.Add(new Vector3(12.5f, -0.25f, 0.4f));
                        itemListPositions.Add(new Vector3(6.75f, 5f, 0.4f));
                        itemListPositions.Add(new Vector3(5f, -9f, 0.4f));
                        itemListPositions.Add(new Vector3(7.5f, -1f, 0.4f));
                        itemListPositions.Add(new Vector3(4.6f, -1f, 0.4f));
                        itemListPositions.Add(new Vector3(-6.75f, 10.85f, 0.4f));
                    } 
                    else if (spawnBoxOnDleks) {
                        itemListPositions.Add(new Vector3(18f, 2.5f, 0.4f));
                        itemListPositions.Add(new Vector3(-4.5f, -9.75f, 0.4f));
                        itemListPositions.Add(new Vector3(-5.75f, -15f, 0.4f));
                        itemListPositions.Add(new Vector3(-16.75f, -6f, 0.4f));
                        itemListPositions.Add(new Vector3(21.5f, -2.15f, 0.4f));
                        itemListPositions.Add(new Vector3(21.5f, -8f, 0.4f));
                        itemListPositions.Add(new Vector3(13.5f, -6.5f, 0.4f));
                        itemListPositions.Add(new Vector3(9.4f, -4.65f, 0.4f));
                        itemListPositions.Add(new Vector3(18.5f, -13.15f, 0.4f));
                        itemListPositions.Add(new Vector3(6.5f, -8.25f, 0.4f));
                        itemListPositions.Add(new Vector3(3.55f, -15f, 0.4f));
                        itemListPositions.Add(new Vector3(-2.15f, -15f, 0.4f));
                        itemListPositions.Add(new Vector3(-8, -14.25f, 0.4f));
                        itemListPositions.Add(new Vector3(-5.2f, -4.5f, 0.4f));
                        itemListPositions.Add(new Vector3(-10.5f, 2.35f, 0.4f));
                    }
                    else {
                        itemListPositions.Add(new Vector3(-18f, 2.5f, 0.4f));
                        itemListPositions.Add(new Vector3(4.5f, -9.75f, 0.4f));
                        itemListPositions.Add(new Vector3(5.75f, -15f, 0.4f));
                        itemListPositions.Add(new Vector3(16.75f, -6f, 0.4f));
                        itemListPositions.Add(new Vector3(-21.5f, -2.15f, 0.4f));
                        itemListPositions.Add(new Vector3(-21.5f, -8f, 0.4f));
                        itemListPositions.Add(new Vector3(-13.5f, -6.5f, 0.4f));
                        itemListPositions.Add(new Vector3(-9.4f, -4.65f, 0.4f));
                        itemListPositions.Add(new Vector3(-18.5f, -13.15f, 0.4f));
                        itemListPositions.Add(new Vector3(-6.5f, -8.25f, 0.4f));
                        itemListPositions.Add(new Vector3(-3.55f, -15f, 0.4f));
                        itemListPositions.Add(new Vector3(2.15f, -15f, 0.4f));
                        itemListPositions.Add(new Vector3(8, -14.25f, 0.4f));
                        itemListPositions.Add(new Vector3(5.2f, -4.5f, 0.4f));
                        itemListPositions.Add(new Vector3(10.5f, 2.35f, 0.4f));
                    }
                    break;
                case 1:
                    itemListPositions.Add(new Vector3(-3.5f, 3.25f, 0.4f));
                    itemListPositions.Add(new Vector3(15.4f, -0.85f, 0.4f));
                    itemListPositions.Add(new Vector3(16f, 4f, 0.4f));
                    itemListPositions.Add(new Vector3(9.25f, 1.25f, 0.4f));
                    itemListPositions.Add(new Vector3(6f, 6f, 0.4f));
                    itemListPositions.Add(new Vector3(6f, 12f, 0.4f));
                    itemListPositions.Add(new Vector3(10.7f, 12.25f, 0.4f));
                    itemListPositions.Add(new Vector3(1.25f, 10.5f, 0.4f));
                    itemListPositions.Add(new Vector3(14.75f, 20.25f, 0.4f));
                    itemListPositions.Add(new Vector3(22f, 20.65f, 0.4f));
                    itemListPositions.Add(new Vector3(13.5f, 23.4f, 0.4f));
                    itemListPositions.Add(new Vector3(22.15f, 23.4f, 0.4f));
                    itemListPositions.Add(new Vector3(19.5f, 4.5f, 0.4f));
                    itemListPositions.Add(new Vector3(22f, -1.75f, 0.4f));
                    itemListPositions.Add(new Vector3(28.25f, 4.75f, 0.4f));
                    break;
                case 2:
                    itemListPositions.Add(new Vector3(2.75f, -11.75f, 0.4f));
                    itemListPositions.Add(new Vector3(9.75f, -12.15f, 0.4f));
                    itemListPositions.Add(new Vector3(16f, -12.15f, 0.4f));
                    itemListPositions.Add(new Vector3(19.85f, -19f, 0.4f));
                    itemListPositions.Add(new Vector3(1.35f, -17.4f, 0.4f));
                    itemListPositions.Add(new Vector3(1.35f, -20f, 0.4f));
                    itemListPositions.Add(new Vector3(2.25f, -23.75f, 0.4f));
                    itemListPositions.Add(new Vector3(16.5f, -25f, 0.4f));
                    itemListPositions.Add(new Vector3(22f, -25f, 0.4f));
                    itemListPositions.Add(new Vector3(24.5f, -17f, 0.4f));
                    itemListPositions.Add(new Vector3(38.35f, -20.65f, 0.4f));
                    itemListPositions.Add(new Vector3(32f, -15.75f, 0.4f));
                    itemListPositions.Add(new Vector3(27.75f, -7.5f, 0.4f));
                    itemListPositions.Add(new Vector3(40.35f, -8f, 0.4f));
                    itemListPositions.Add(new Vector3(6.5f, -7.5f, 0.4f));
                    break;
                case 3:
                    itemListPositions.Add(new Vector3(18f, 2.5f, 0.4f));
                    itemListPositions.Add(new Vector3(-4.5f, -9.75f, 0.4f));
                    itemListPositions.Add(new Vector3(-5.75f, -15f, 0.4f));
                    itemListPositions.Add(new Vector3(-16.75f, -6f, 0.4f));
                    itemListPositions.Add(new Vector3(21.5f, -2.15f, 0.4f));
                    itemListPositions.Add(new Vector3(21.5f, -8f, 0.4f));
                    itemListPositions.Add(new Vector3(13.5f, -6.5f, 0.4f));
                    itemListPositions.Add(new Vector3(9.4f, -4.65f, 0.4f));
                    itemListPositions.Add(new Vector3(18.5f, -13.15f, 0.4f));
                    itemListPositions.Add(new Vector3(6.5f, -8.25f, 0.4f));
                    itemListPositions.Add(new Vector3(3.55f, -15f, 0.4f));
                    itemListPositions.Add(new Vector3(-2.15f, -15f, 0.4f));
                    itemListPositions.Add(new Vector3(-8, -14.25f, 0.4f));
                    itemListPositions.Add(new Vector3(-5.2f, -4.5f, 0.4f));
                    itemListPositions.Add(new Vector3(-10.5f, 2.35f, 0.4f));
                    break;
                case 4:
                    itemListPositions.Add(new Vector3(12.5f, -2.5f, 0.4f));
                    itemListPositions.Add(new Vector3(24f, 2.5f, 0.4f));
                    itemListPositions.Add(new Vector3(13.5f, 6f, 0.4f));
                    itemListPositions.Add(new Vector3(-23.25f, -0.25f, 0.4f));
                    itemListPositions.Add(new Vector3(-14.25f, 1.5f, 0.4f));
                    itemListPositions.Add(new Vector3(-14.25f, -4.5f, 0.4f));
                    itemListPositions.Add(new Vector3(-14.25f, -8.25f, 0.4f));
                    itemListPositions.Add(new Vector3(-13.75f, -14.5f, 0.4f));
                    itemListPositions.Add(new Vector3(5.75f, -10.5f, 0.4f));
                    itemListPositions.Add(new Vector3(19.25f, -4f, 0.4f));
                    itemListPositions.Add(new Vector3(29f, -1.5f, 0.4f));
                    itemListPositions.Add(new Vector3(37.25f, -3.25f, 0.4f));
                    itemListPositions.Add(new Vector3(33.75f, 7.25f, 0.4f));
                    itemListPositions.Add(new Vector3(20f, 11.5f, 0.4f));
                    itemListPositions.Add(new Vector3(-8.75f, 12.5f, 0.4f));
                    break;
                case 5:
                    itemListPositions.Add(new Vector3(-3, 7.25f, 0.4f));
                    itemListPositions.Add(new Vector3(-0.4f, 6.4f, 0.4f));
                    itemListPositions.Add(new Vector3(-17.75f, 2.5f, 0.4f));
                    itemListPositions.Add(new Vector3(-17.15f, 0.55f, 0.4f));
                    itemListPositions.Add(new Vector3(-13.45f, -7f, 0.4f));
                    itemListPositions.Add(new Vector3(-5.3f, -8.75f, 0.4f));
                    itemListPositions.Add(new Vector3(9.25f, -12f, 0.4f));
                    itemListPositions.Add(new Vector3(14.75f, -6.135f, 0.4f));
                    itemListPositions.Add(new Vector3(2.35f, -5.75f, 0.4f));
                    itemListPositions.Add(new Vector3(9.25f, 4.25f, 0.4f));
                    itemListPositions.Add(new Vector3(11.85f, 8.45f, 0.4f));
                    itemListPositions.Add(new Vector3(20.25f, 1.35f, 0.4f));
                    itemListPositions.Add(new Vector3(18f, 11.75f, 0.4f));
                    itemListPositions.Add(new Vector3(22.25f, 13.5f, 0.4f));
                    itemListPositions.Add(new Vector3(-19.35f, -2f, 0.4f));
                    break;
                case 6:
                    itemListPositions.Add(new Vector3(0, -33.75f, -1f));
                    itemListPositions.Add(new Vector3(7.8f, -39.5f, -1f));
                    itemListPositions.Add(new Vector3(-10.95f, -39f, -1f));
                    itemListPositions.Add(new Vector3(-13.75f, -34.25f, -1f));
                    itemListPositions.Add(new Vector3(-4.15f, -34f, -1f));
                    itemListPositions.Add(new Vector3(12.75f, -31.5f, -1f));
                    itemListPositions.Add(new Vector3(7.7f, -23.25f, -1f));
                    itemListPositions.Add(new Vector3(14.25f, 24.65f, -1f));
                    itemListPositions.Add(new Vector3(5.75f, 31.5f, -1f));
                    itemListPositions.Add(new Vector3(0f, 34f, -1f));
                    itemListPositions.Add(new Vector3(4.5f, 8.4f, -1f));
                    itemListPositions.Add(new Vector3(1f, 10f, -1f));
                    itemListPositions.Add(new Vector3(-6.7f, 10.25f, -1f));
                    itemListPositions.Add(new Vector3(-15f, 18f, -1f));
                    itemListPositions.Add(new Vector3(-12.25f, 31f, -1f));
                    break;
            }
            itemListPositions.Shuffle();
        }
    }

    public static class Joker
    {
        public static PlayerControl joker;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);

        public static bool triggerJokerWin = false;
        public static bool canSabotage = true;

        public static void clearAndReload() {
            joker = null;
            triggerJokerWin = false;
            canSabotage = CustomOptionHolder.jokerCanSabotage.getBool();
        }
    }

    public static class RoleThief
    {
        public static PlayerControl rolethief;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);

        public static float cooldown = float.MaxValue;

        public static PlayerControl currentTarget;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.RoleThiefStealButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            rolethief = null;
            currentTarget = null;
            cooldown = CustomOptionHolder.rolethiefCooldown.getFloat();
        }
    }

    public static class Pyromaniac
    {
        public static PlayerControl pyromaniac;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);

        public static float cooldown = 30f;
        public static float duration = 3f;
        public static bool triggerPyromaniacWin = false;

        public static PlayerControl currentTarget;
        public static PlayerControl sprayTarget;
        public static List<PlayerControl> sprayedPlayers = new List<PlayerControl>();

        private static Sprite spraySprite;
        public static Sprite getSpraySprite() {
            if (spraySprite) return spraySprite;
            spraySprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PyromaniacSprayButton.png", 90f);
            return spraySprite;
        }

        private static Sprite igniteSprite;
        public static Sprite getIgniteSprite() {
            if (igniteSprite) return igniteSprite;
            igniteSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PyromaniacIgniteButton.png", 90f);
            return igniteSprite;
        }

        public static bool sprayedEveryoneAlive() {
            return PlayerInCache.AllPlayers.All(x => { return x.PlayerControl == Pyromaniac.pyromaniac || x.PlayerControl.Data.IsDead || x.PlayerControl.Data.Disconnected || Pyromaniac.sprayedPlayers.Any(y => y.PlayerId == x.PlayerId); });
        }

        public static void clearAndReload() {
            pyromaniac = null;
            currentTarget = null;
            sprayTarget = null;
            triggerPyromaniacWin = false;
            sprayedPlayers = new List<PlayerControl>();
            foreach (PoolablePlayer p in MapOptions.playerIcons.Values) {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }
            cooldown = CustomOptionHolder.pyromaniacCooldown.getFloat();
            duration = CustomOptionHolder.pyromaniacDuration.getFloat();
        }
    }

    public static class TreasureHunter
    {
        public static PlayerControl treasureHunter;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);

        public static bool triggertreasureHunterWin = false;
        public static bool canPlace = true;
        public static int treasureCollected = 0;
        public static float neededTreasure = 3;
        public static int randomSpawn = 0;

        public static TMPro.TMP_Text treasureCounterButtonText;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TreasureHunterPlaceChestButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            treasureHunter = null;
            triggertreasureHunterWin = false;
            canPlace = true;
            treasureCollected = 0;
            neededTreasure = CustomOptionHolder.treasureHunterTreasureNumber.getFloat();
            randomSpawn = 0;
        }
    }

    public static class Devourer
    {
        public static PlayerControl devourer;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);

        public static bool triggerdevourerWin = false;
        public static int devouredBodies = 0;
        public static float neededBodies = 3;
        public static PlayerControl currentTarget = null;
        public static List<PlayerControl> eatenPlayers = new List<PlayerControl>();

        public static TMPro.TMP_Text devourCounterButtonText;
        public static TMPro.TMP_Text devourEatCounterButtonText;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.DevourerDevourButton.png", 90f);
            return buttonSprite;
        }

        private static Sprite buttonEatSprite;
        public static Sprite getEatButtonSprite() {
            if (buttonEatSprite) return buttonSprite;
            buttonEatSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.DevourerEatButton.png", 90f);
            return buttonEatSprite;
        }

        public static void clearAndReload() {
            devourer = null;
            triggerdevourerWin = false;
            devouredBodies = 0;
            neededBodies = CustomOptionHolder.devourerBodiesNumber.getFloat();
            currentTarget = null;
            eatenPlayers = new List<PlayerControl>();
        }
    }

    public static class Poisoner
    {
        public static PlayerControl poisoner;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);

        public static float duration = 3f;
        public static bool triggerPoisonerWin = false;
        public static float infectRange = 1f;
        public static float infectDuration = 20f;
        public static bool canSabotage = true;

        public static PlayerControl currentTarget;
        public static PlayerControl poisonTarget;
        public static PlayerControl poisonedTarget;
        public static List<PlayerControl> poisonedPlayers = new List<PlayerControl>();
        public static Dictionary<byte, float> infectProgress;

        private static Sprite poisonSprite;
        public static Sprite getPoisonSprite() {
            if (poisonSprite) return poisonSprite;
            poisonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PoisonerPoisonButton.png", 90f);
            return poisonSprite;
        }

        private static Sprite plagueSprite;
        public static Sprite getPlagueSprite() {
            if (plagueSprite) return plagueSprite;
            plagueSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PoisonerPlagueButton.png", 90f);
            return plagueSprite;
        }

        public static bool poisonedEveryoneAlive() {
            return PlayerInCache.AllPlayers.All(x => { return x.PlayerControl == Poisoner.poisoner || x.PlayerControl.Data.IsDead || x.PlayerControl.Data.Disconnected || Poisoner.poisonedPlayers.Any(y => y.PlayerId == x.PlayerId); });
        }

        public static void clearAndReload() {
            poisoner = null;
            currentTarget = null;
            poisonTarget = null;
            poisonedTarget = null;
            triggerPoisonerWin = false;
            poisonedPlayers = new List<PlayerControl>();
            foreach (PoolablePlayer p in MapOptions.playerIcons.Values) {
                if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
            }
            duration = 3f;
            infectRange = CustomOptionHolder.poisonerInfectRange.getFloat();
            infectDuration = CustomOptionHolder.poisonerInfectDuration.getFloat();
            infectProgress = new Dictionary<byte, float>();
            canSabotage = CustomOptionHolder.poisonerCanSabotage.getBool();
        }
    }

    public static class Puppeteer
    {
        public static PlayerControl puppeteer;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);

        public static bool triggerPuppeteerWin = false;
        public static float numberOfKills = 0;
        public static int counter = 0;
        public static bool morphed = false;
        public static TMPro.TMP_Text puppeteerText;
        public static PlayerControl currentTarget;
        public static PlayerControl pickTarget;
        public static PlayerControl transformTarget;
        private static Sprite pickTargetSprite;
        public static Vector3 positionPreMorphed;

        public static Sprite getpickTargetSprite() {
            if (pickTargetSprite) return pickTargetSprite;
            pickTargetSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PuppeteerPickTargetButton.png", 90f);
            return pickTargetSprite;
        }

        private static Sprite transformSprite;

        public static Sprite getTransformSprite() {
            if (transformSprite) return transformSprite;
            transformSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PuppeteerTransformButton.png", 90f);
            return transformSprite;
        }

        private static Sprite uncoverSprite;

        public static Sprite getUncoverSprite() {
            if (uncoverSprite) return uncoverSprite;
            uncoverSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PuppeteerTransformButton.png", 90f);
            return uncoverSprite;
        }

        public static void clearAndReload() {
            puppeteer = null;
            triggerPuppeteerWin = false;
            numberOfKills = CustomOptionHolder.puppeteerNumberOfKills.getFloat();
            counter = 0;
            morphed = false;
            currentTarget = null;
            pickTarget = null;
            transformTarget = null;
            positionPreMorphed = new Vector3 (0,0,0);
        }

        public static void Reset() {
            morphed = false;
            puppeteer.setDefaultLook();
            transformTarget = null;
            pickTarget = null;
            currentTarget = null;
        }
    }

    public static class Exiler
    {
        public static PlayerControl exiler;
        public static PlayerControl target;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);
        public static Color targetColor = new Color32(0, 0, 0, byte.MaxValue);

        public static bool triggerExilerWin = false;
        public static List<PlayerControl> possibleTargets = new List<PlayerControl>();
        public static bool usedTarget = false;
        public static TMPro.TMP_Text targetNameButtonText;

        private static Sprite targetButtonSprite;
        public static Sprite getTargetButtonSprite() {
            if (targetButtonSprite) return targetButtonSprite;
            targetButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ExilerTargetButton.png", 90f);
            return targetButtonSprite;
        }

        public static void clearAndReload() {
            exiler = null;
            triggerExilerWin = false;
            target = null;
            possibleTargets = new List<PlayerControl>();
            usedTarget = false;
        }
    }

    public static class Amnesiac
    {
        public static PlayerControl amnesiac;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);
        
        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.AmnesiacRememberButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            amnesiac = null;

        }
    }

    public static class Seeker
    {
        public static PlayerControl seeker;
        public static Color color = new Color32(128, 128, 128, byte.MaxValue);

        public static bool triggerSeekerWin = false;
        public static float cooldown = 0;
        public static float currentPlayers = 0;
        public static float minigameDuration = 20;
        public static float currentPoints = 0;
        public static float neededPoints = 0;
        public static PlayerControl hidedPlayerOne = null;
        public static PlayerControl hidedPlayerTwo = null;
        public static PlayerControl hidedPlayerThree = null;
        public static PlayerControl currentTarget = null;
        public static bool isMinigaming = false;
        public static bool minigameReady = false;
        public static bool onlyOneFinishMinigame = true;
        public static bool timeOutMinigame = false;
        public static float seekerSelectedHiding = 0;
        public static float hidedPlayerOneSelectedHiding = 0;
        public static float hidedPlayerTwoSelectedHiding = 0;
        public static float hidedPlayerThreeSelectedHiding = 0;
        public static float howmanyselectedattacks = 0;
        public static GameObject minigameArenaHideOnePointOne = null;
        public static GameObject minigameArenaHideOnePointTwo = null;
        public static GameObject minigameArenaHideOnePointThree = null;
        public static GameObject minigameArenaHideTwoPointOne = null;
        public static GameObject minigameArenaHideTwoPointTwo = null;
        public static GameObject minigameArenaHideTwoPointThree = null;
        public static GameObject minigameArenaHideThreePointOne = null;
        public static GameObject minigameArenaHideThreePointTwo = null;
        public static GameObject minigameArenaHideThreePointThree = null;
        public static TMPro.TMP_Text seekerPlayerPointsCount;
        public static TMPro.TMP_Text seekerPerformMinigamePlayerPointsCount;
        public static GameObject lowerminigameArenaHideOnePointOne = null;
        public static GameObject lowerminigameArenaHideOnePointTwo = null;
        public static GameObject lowerminigameArenaHideOnePointThree = null;
        public static GameObject lowerminigameArenaHideTwoPointOne = null;
        public static GameObject lowerminigameArenaHideTwoPointTwo = null;
        public static GameObject lowerminigameArenaHideTwoPointThree = null;
        public static GameObject lowerminigameArenaHideThreePointOne = null;
        public static GameObject lowerminigameArenaHideThreePointTwo = null;
        public static GameObject lowerminigameArenaHideThreePointThree = null;

        private static Sprite buttonTargetSprite;
        public static Sprite getTargetButtonSprite() {
            if (buttonTargetSprite) return buttonTargetSprite;
            buttonTargetSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SeekerTarget.png", 90f);
            return buttonTargetSprite;
        }

        private static Sprite buttonMinigameSprite;
        public static Sprite getMinigameButtonSprite() {
            if (buttonMinigameSprite) return buttonMinigameSprite;
            buttonMinigameSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SeekerMinigameButton.png", 90f);
            return buttonMinigameSprite;
        }

        private static Sprite buttonMonjaSprite;
        public static Sprite getMonjaButtonSprite() {
            if (buttonMonjaSprite) return buttonMonjaSprite;
            buttonMonjaSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SeekerMonjaButton.png", 90f);
            return buttonMonjaSprite;
        }

        private static Sprite buttonCuloSprite;
        public static Sprite getCuloButtonSprite() {
            if (buttonCuloSprite) return buttonCuloSprite;
            buttonCuloSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SeekerCuloButton.png", 90f);
            return buttonCuloSprite;
        }

        private static Sprite buttonDioSprite;
        public static Sprite getDioButtonSprite() {
            if (buttonDioSprite) return buttonDioSprite;
            buttonDioSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SeekerDioButton.png", 90f);
            return buttonDioSprite;
        }

        private static Sprite buttonSearchMonjaSprite;
        public static Sprite getSearchMonjaButtonSprite() {
            if (buttonSearchMonjaSprite) return buttonSearchMonjaSprite;
            buttonSearchMonjaSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SeekerSearchMonja.png", 90f);
            return buttonSearchMonjaSprite;
        }

        private static Sprite buttonSearchCuloSprite;
        public static Sprite getSearchCuloButtonSprite() {
            if (buttonSearchCuloSprite) return buttonSearchCuloSprite;
            buttonSearchCuloSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SeekerSearchCulosito.png", 90f);
            return buttonSearchCuloSprite;
        }

        private static Sprite buttonSearchDioSprite;
        public static Sprite getSearchDioButtonSprite() {
            if (buttonSearchDioSprite) return buttonSearchDioSprite;
            buttonSearchDioSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SeekerSearchDio.png", 90f);
            return buttonSearchDioSprite;
        }


        public static void clearAndReload() {
            seeker = null;
            triggerSeekerWin = false;
            cooldown = CustomOptionHolder.seekerCooldown.getFloat();
            currentTarget = null;
            hidedPlayerOne = null;
            hidedPlayerTwo = null;
            hidedPlayerThree = null;
            isMinigaming = false;
            minigameReady = false;
            currentPlayers = 0;
            minigameDuration = 20f;
            onlyOneFinishMinigame = true;
            timeOutMinigame = false;
            seekerSelectedHiding = 0;
            hidedPlayerOneSelectedHiding = 0;
            hidedPlayerTwoSelectedHiding = 0;
            hidedPlayerThreeSelectedHiding = 0;
            currentPoints = 0;
            neededPoints = CustomOptionHolder.seekerPointsNumber.getFloat();
            minigameArenaHideOnePointOne = null;
            minigameArenaHideOnePointTwo = null;
            minigameArenaHideOnePointThree = null;
            minigameArenaHideTwoPointOne = null;
            minigameArenaHideTwoPointTwo = null;
            minigameArenaHideTwoPointThree = null;
            minigameArenaHideThreePointOne = null;
            minigameArenaHideThreePointTwo = null;
            minigameArenaHideThreePointThree = null;
            lowerminigameArenaHideOnePointOne = null;
            lowerminigameArenaHideOnePointTwo = null;
            lowerminigameArenaHideOnePointThree = null;
            lowerminigameArenaHideTwoPointOne = null;
            lowerminigameArenaHideTwoPointTwo = null;
            lowerminigameArenaHideTwoPointThree = null;
            lowerminigameArenaHideThreePointOne = null;
            lowerminigameArenaHideThreePointTwo = null;
            lowerminigameArenaHideThreePointThree = null;
            howmanyselectedattacks = 0;
        }

        public static void ResetOnePlayer(int whichPlayer) {
            switch (whichPlayer) {
                case 1:
                    hidedPlayerOne = null;
                    break;
                case 2:
                    hidedPlayerTwo = null;
                    break;
                case 3:
                    hidedPlayerThree = null;
                    break;
            }
            if (currentPlayers == 3) {
                minigameReady = false;
            }
            currentPlayers -= 1; 
            seekerPlayerPointsCount.text = $"{currentPlayers} / 3";
            seekerPerformMinigamePlayerPointsCount.text = $"{currentPoints} / {neededPoints}";
        }

        public static void ResetValues(bool checkJailer) {
            currentTarget = null;
            hidedPlayerOne = null;
            hidedPlayerTwo = null;
            hidedPlayerThree = null;
            seekerSelectedHiding = 0;
            hidedPlayerOneSelectedHiding = 0;
            hidedPlayerTwoSelectedHiding = 0;
            hidedPlayerThreeSelectedHiding = 0;
            currentPlayers = 0;
            minigameReady = false;
            isMinigaming = false;
            onlyOneFinishMinigame = true;
            minigameDuration = 20f;
            howmanyselectedattacks = 0;
            seekerPlayerPointsCount.text = $"{currentPlayers} / 3";
            seekerPerformMinigamePlayerPointsCount.text = $"{currentPoints} / {neededPoints}";
            if (checkJailer && Jailer.prisonPlayer != null) {
                switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                    // Skeld
                    case 0:
                        if (LasMonjas.activatedSensei) {
                            Jailer.prisonPlayer.transform.position = new Vector3(-0.6f, 3.5f, Jailer.prisonPlayer.transform.position.z);
                        }
                        else if (LasMonjas.activatedDleks) {
                            Jailer.prisonPlayer.transform.position = new Vector3(0.75f, 5.25f, Jailer.prisonPlayer.transform.position.z);
                        }
                        else {
                            Jailer.prisonPlayer.transform.position = new Vector3(-0.75f, 5.25f, Jailer.prisonPlayer.transform.position.z);
                        }
                        break;
                    // MiraHQ
                    case 1:
                        Jailer.prisonPlayer.transform.position = new Vector3(25.5f, 4.75f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Polus
                    case 2:
                        Jailer.prisonPlayer.transform.position = new Vector3(17.15f, -17.15f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Dleks
                    case 3:
                        Jailer.prisonPlayer.transform.position = new Vector3(0.75f, 5.25f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Airship
                    case 4:
                        Jailer.prisonPlayer.transform.position = new Vector3(16.25f, 15.25f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Fungle
                    case 5:
                        Jailer.prisonPlayer.transform.position = new Vector3(-5.5f, 1f, Jailer.prisonPlayer.transform.position.z);
                        break;
                    // Submerged
                    case 6:
                        if (Jailer.prisonPlayer.transform.position.y > 0) {
                            Jailer.prisonPlayer.transform.position = new Vector3(-4.3f, 13.5f, Jailer.prisonPlayer.transform.position.z);
                        }
                        else {
                            Jailer.prisonPlayer.transform.position = new Vector3(1.85f, -26.65f, Jailer.prisonPlayer.transform.position.z);
                        }
                        break;
                }
                Jailer.prisonPlayer = null;
                Jailer.usedJail = false;
            }
        }
    }

    public static class Captain
    {

        public static PlayerControl captain;
        public static Color color = new Color32(94, 62, 125, byte.MaxValue);
        public static byte specialVoteTargetPlayerId = byte.MaxValue;
        public static bool usedSpecialVote = false;
        public static PlayerControl specialVoteTarget = null;
        public static bool canUseSpecialVote = false;

        private static Sprite buttonSprite;

        public static Sprite getCallButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.CaptainMeetingButton.png", 90f);
            return buttonSprite;
        }

        private static Sprite targetSprite;

        public static Sprite getTargetSprite() {
            if (targetSprite) return targetSprite;
            targetSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.CaptainTargetIcon.png", 150f);
            return targetSprite;
        }

        public static void clearAndReload() {
            captain = null; 
            specialVoteTargetPlayerId = byte.MaxValue;
            usedSpecialVote = false;
            specialVoteTarget = null;
            canUseSpecialVote = CustomOptionHolder.captainCanSpecialVoteOneTime.getBool();
        }
    }

    public static class Mechanic
    {
        public static PlayerControl mechanic;
        public static Color color = new Color32(127, 76, 0, byte.MaxValue);
        public static int numberOfRepairs;
        public static TMPro.TMP_Text mechanicRepairButtonText;
        private static Sprite buttonSprite;
        public static bool expertRepairs;
        public static int rechargeTasksNumber = 3;
        public static int rechargedTasks = 3;
        public static int charges = 1;

        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.MechanicRepairButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            mechanic = null;
            numberOfRepairs = (int)CustomOptionHolder.mechanicNumberOfRepairs.getFloat();
            expertRepairs = CustomOptionHolder.mechanicExpertRepairs.getBool();
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.mechanicRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.mechanicRechargeTasksNumber.getFloat());
            charges = numberOfRepairs;        }
    }

    public static class Sheriff
    {
        public static PlayerControl sheriff;
        public static Color color = new Color32(255, 255, 0, byte.MaxValue);

        public static float cooldown = 30f;
        public static bool canKillNeutrals = false;

        public static PlayerControl currentTarget;

        public static void clearAndReload() {
            sheriff = null;
            currentTarget = null;
            cooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
            canKillNeutrals = CustomOptionHolder.sheriffCanKillNeutrals.getBool();
        }
    }

    public static class Detective
    {
        public static PlayerControl detective;
        public static Color color = new Color32(200, 90, 190, byte.MaxValue);

        public static float footprintIntervall = 1f;
        public static float footprintDuration = 1f;
        public static bool anonymousFootprints = false;
        public static float timer = 1f;
        public static int showFootPrints = 0;
        public static float cooldown = 30f;
        public static float duration = 10f;
        public static float detectiveTimer = 0f;
        public static float backUpduration = 10f;
        public static int footprintcolor = 6;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.DetectiveFootPrintButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            detective = null;
            detectiveTimer = 0f;
            showFootPrints = CustomOptionHolder.detectiveShowFootprints.getSelection();
            cooldown = CustomOptionHolder.detectiveCooldown.getFloat();
            duration = CustomOptionHolder.detectiveShowFootPrintDuration.getFloat();
            anonymousFootprints = CustomOptionHolder.detectiveAnonymousFootprints.getBool();
            footprintIntervall = 1f;
            footprintDuration = 15f;
            timer = footprintIntervall;
            footprintcolor = 6;
            backUpduration = duration;
        }
    }

    public static class Forensic
    {
        public static PlayerControl forensic;
        public static Color color = new Color32(78, 97, 255, byte.MaxValue);

        public static float reportNameDuration = 0f;
        public static float reportColorDuration = 20f;
        public static float reportClueDuration = 40f;

        public static DeadPlayer target;
        public static DeadPlayer soulTarget;
        public static List<Tuple<DeadPlayer, Vector3>> deadBodies = new List<Tuple<DeadPlayer, Vector3>>();
        public static List<Tuple<DeadPlayer, Vector3>> featureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
        public static List<SpriteRenderer> souls = new List<SpriteRenderer>();
        public static DateTime meetingStartTime = DateTime.UtcNow;
        public static float duration = 10f;
        public static bool oneTimeUse = false;

        private static Sprite soulSprite;
        public static Sprite getSoulSprite() {
            if (soulSprite) return soulSprite;
            soulSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.ForensicSoul.png", 500f);
            return soulSprite;
        }

        private static Sprite question;
        public static Sprite getQuestionSprite() {
            if (question) return question;
            question = Helpers.loadSpriteFromResources("LasMonjas.Images.ForensicAskButton.png", 90f);
            return question;
        }


        public static void clearAndReload() {
            forensic = null;
            reportNameDuration = CustomOptionHolder.forensicReportNameDuration.getFloat();
            reportColorDuration = CustomOptionHolder.forensicReportColorDuration.getFloat();
            reportClueDuration = CustomOptionHolder.forensicReportClueDuration.getFloat();
            target = null;
            soulTarget = null;
            deadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            featureDeadBodies = new List<Tuple<DeadPlayer, Vector3>>();
            souls = new List<SpriteRenderer>();
            meetingStartTime = DateTime.UtcNow;
            duration = CustomOptionHolder.forensicDuration.getFloat();
            oneTimeUse = CustomOptionHolder.forensicOneTimeUse.getBool();
        }
    }

    public static class TimeTraveler
    {
        public static PlayerControl timeTraveler;
        public static Color color = new Color32(0, 189, 255, byte.MaxValue);

        public static float stopTime = 10f;
        public static float cooldown = 10f;

        public static bool markedLocation = false;
        public static Vector3 teleportPos;
        public static bool isStoppingTime = false;
        public static bool shielded = false;

        private static Sprite teleportbuttonSprite;
        private static Sprite markedteleportbuttonSprite;
        private static Sprite floorbuttonSprite;
        private static Sprite stoptimebuttonSprite;
        public static GameObject teleportMark = null;
        public static List<GameObject> objectsCantPlaceTeleport = new List<GameObject>();

        public static Sprite getTeleportButtonSprite() {
            if (teleportbuttonSprite) return teleportbuttonSprite;
            teleportbuttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TimeTravelerPlaceWarpButton.png", 90f);
            return teleportbuttonSprite;
        }
        public static Sprite getMarkedTeleportButtonSprite() {
            if (markedteleportbuttonSprite) return markedteleportbuttonSprite;
            markedteleportbuttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TimeTravelerTeleportButton.png", 90f);
            return markedteleportbuttonSprite;
        }
        public static Sprite getStopTimeButtonSprite() {
            if (stoptimebuttonSprite) return stoptimebuttonSprite;
            stoptimebuttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TimeTravelerStopTimeButton.png", 90f);
            return stoptimebuttonSprite;
        }
        public static Sprite getFloorSprite() {
            if (floorbuttonSprite) return floorbuttonSprite;
            floorbuttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TimeTravelerFloorWarp.png", 120f);
            return floorbuttonSprite;
        }

        public static void clearAndReload() {
            timeTraveler = null;
            isStoppingTime = false;
            markedLocation = false;
            shielded = false;
            teleportPos = new Vector3 (0,0,0);
            teleportMark = null;
            stopTime = CustomOptionHolder.timeTravelerStopTime.getFloat();
            cooldown = CustomOptionHolder.timeTravelerCooldown.getFloat();
            objectsCantPlaceTeleport.Clear();
        }

        public static void resetTimeTraveler() {
            isStoppingTime = false;
            markedLocation = false;
            shielded = false;
            teleportPos = new Vector3(0, 0, 0);
            if (teleportMark != null) {
                teleportMark.SetActive(false);
            }
            teleportMark = null;
        }
    }

    public static class Squire
    {
        public static PlayerControl squire;
        public static PlayerControl shielded;
        public static Color color = new Color32(0, 255, 0, byte.MaxValue);
        public static bool usedShield;

        public static int showShielded = 0;
        public static bool showAttemptToShielded = false;
        public static bool resetShieldAfterMeeting = false;

        public static PlayerControl currentTarget;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SquireShieldButton.png", 90f);
            return buttonSprite;
        }

        public static void resetShield() {
            shielded = null;
            usedShield = false;
        }

        public static void clearAndReload() {
            squire = null;
            shielded = null;
            currentTarget = null;
            usedShield = false;
            showShielded = CustomOptionHolder.squireShowShielded.getSelection();
            showAttemptToShielded = CustomOptionHolder.squireShowAttemptToShielded.getBool();
            resetShieldAfterMeeting = CustomOptionHolder.squireResetShieldAfterMeeting.getBool();
        }
    }

    public static class Cheater
    {
        public static PlayerControl cheater;
        public static Color color = new Color32(102, 102, 153, byte.MaxValue);
        private static Sprite spriteCheck;
        public static bool canCallEmergency = false;
        public static bool canOnlyCheatOthers = false;

        public static byte playerId1 = Byte.MaxValue;
        public static byte playerId2 = Byte.MaxValue;

        public static PlayerControl cheatedP1;
        public static PlayerControl cheatedP2;
        public static bool usedCheat;
        public static Sprite getCheckSprite() {
            if (spriteCheck) return spriteCheck;
            spriteCheck = Helpers.loadSpriteFromResources("LasMonjas.Images.CheaterCheck.png", 150f);
            return spriteCheck;
        }

        public static void clearAndReload() {
            cheater = null;
            playerId1 = Byte.MaxValue;
            playerId2 = Byte.MaxValue;
            cheatedP1 = null;
            cheatedP2 = null;
            usedCheat = false;
            canCallEmergency = CustomOptionHolder.cheaterCanCallEmergency.getBool();
            canOnlyCheatOthers = CustomOptionHolder.cheatercanOnlyCheatOthers.getBool();
        }
    }

    public static class FortuneTeller
    {
        public static PlayerControl fortuneTeller;
        public static Color color = new Color32(0, 198, 66, byte.MaxValue);

        public static List<PlayerControl> revealedPlayers = new List<PlayerControl>();
        public static float cooldown = float.MaxValue;
        public static int kindOfInfo = 0;
        public static int playersWithNotification = 0;

        public static PlayerControl currentTarget;
        public static PlayerControl revealTarget;
        public static float duration = 3;

        public static int numberOfFortunes;
        public static TMPro.TMP_Text fortuneTellerRevealButtonText;
        public static bool canCallEmergency = false;
        public static int rechargeTasksNumber = 3;
        public static int rechargedTasks = 3;
        public static int charges = 1;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.FortuneTellerRevealButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            fortuneTeller = null;
            revealedPlayers = new List<PlayerControl>();
            cooldown = CustomOptionHolder.fortuneTellerCooldown.getFloat();
            duration = CustomOptionHolder.fortuneTellerDuration.getFloat();
            kindOfInfo = CustomOptionHolder.fortuneTellerKindOfInfo.getSelection();
            playersWithNotification = CustomOptionHolder.fortuneTellerPlayersWithNotification.getSelection();
            numberOfFortunes = (int)CustomOptionHolder.fortuneTellerNumberOfSee.getFloat();
            currentTarget = null;
            revealTarget = null;
            canCallEmergency = CustomOptionHolder.fortuneTellerCanCallEmergency.getBool();
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.fortuneTellerRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.fortuneTellerRechargeTasksNumber.getFloat());
            charges = 1;
        }
    }

    public static class Hacker
    {
        public static PlayerControl hacker;
        public static Color color = new Color32(83, 131, 219, byte.MaxValue);

        public static float cooldown = 30f;
        public static float duration = 10f;
        public static float hackerTimer = 0f;
        public static float backUpduration = 10f;

        public static TMPro.TMP_Text hackerAdminTableChargesText;
        public static TMPro.TMP_Text hackerVitalsChargesText;

        public static Minigame vitals = null;
        public static float toolsNumber = 5f;
        public static int rechargeTasksNumber = 2;
        public static int rechargedTasks = 2;
        public static int chargesVitals = 1;
        public static int chargesAdminTable = 1;
        private static Sprite vitalsSprite;
        private static Sprite adminSprite;

        public static Sprite getVitalsSprite() {
            if (vitalsSprite) return vitalsSprite;
            vitalsSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.HackerVitalsButton.png", 90f);
            return vitalsSprite;
        }

        public static Sprite getAdminSprite() {
            if (adminSprite) return adminSprite;
            adminSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.HackerAdminButton.png", 90f);
            return adminSprite;
        }

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.HackerInfoButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            hacker = null;
            hackerTimer = 0f;
            cooldown = CustomOptionHolder.hackerCooldown.getFloat();
            duration = CustomOptionHolder.hackerHackeringDuration.getFloat();
            vitals = null;
            adminSprite = null;
            toolsNumber = CustomOptionHolder.hackerToolsNumber.getFloat();
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.hackerRechargeTasksNumber.getFloat());
            chargesVitals = Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber.getFloat()) / 2;
            chargesAdminTable = Mathf.RoundToInt(CustomOptionHolder.hackerToolsNumber.getFloat()) / 2;
            backUpduration = duration;
        }
    }

    public static class Sleuth
    {
        public static PlayerControl sleuth;
        public static Color color = new Color32(0, 159, 87, byte.MaxValue);
        public static List<Arrow> localArrows = new List<Arrow>();

        public static float updateIntervall = 5f;
        public static bool resetTargetAfterMeeting = false;
        public static float corpsesPathfindCooldown = 30f;
        public static float corpsesPathfindDuration = 5f;
        public static float corpsesPathfindTimer = 0f;
        public static List<Vector3> deadBodyPositions = new List<Vector3>();
        public static float duration = 5f;
        public static float timer = 0f;

        public static float backUpduration = 10f;

        public static PlayerControl currentTarget;
        public static PlayerControl located;
        public static bool usedLocate = false;
        public static float timeUntilUpdate = 0f;
        public static Arrow arrow = new Arrow(Color.blue);
        public static Arrow arrowWho = new Arrow(Color.magenta);

        private static Sprite corpsePathfindButtonSprite;
        public static Sprite getCorpsePathfindButtonSprite() {
            if (corpsePathfindButtonSprite) return corpsePathfindButtonSprite;
            corpsePathfindButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SleuthPathfindButton.png", 90f);
            return corpsePathfindButtonSprite;
        }

        private static Sprite locateButtonSprite;
        public static Sprite getLocateButtonSprite() {
            if (locateButtonSprite) return locateButtonSprite;
            locateButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SleuthLocateButton.png", 90f);
            return locateButtonSprite;
        }

        private static Sprite sleuthWhosThereButtonSprite;
        public static Sprite getwhosThereButtonSprite() {
            if (sleuthWhosThereButtonSprite) return sleuthWhosThereButtonSprite;
            sleuthWhosThereButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SleuthWhosThereButton.png", 90f);
            return sleuthWhosThereButtonSprite;
        }

        public static void resetLocated() {
            currentTarget = located = null;
            usedLocate = false;
            if (arrow?.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
            arrow = new Arrow(Color.blue);
            if (arrow.arrow != null) arrow.arrow.SetActive(false);
        }
        public static void resetWhoLocated() {
            if (arrowWho?.arrow != null) UnityEngine.Object.Destroy(arrowWho.arrow);
            arrowWho = new Arrow(Color.magenta);
            if (arrowWho.arrow != null) arrowWho.arrow.SetActive(false);
        }

        public static void clearAndReload() {
            sleuth = null;
            resetLocated();
            resetWhoLocated();
            timeUntilUpdate = 0f;
            updateIntervall = CustomOptionHolder.sleuthUpdateIntervall.getFloat();
            resetTargetAfterMeeting = CustomOptionHolder.sleuthResetTargetAfterMeeting.getBool();
            if (localArrows != null) {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UnityEngine.Object.Destroy(arrow.arrow);
            }
            deadBodyPositions = new List<Vector3>();
            corpsesPathfindTimer = 0f;
            corpsesPathfindCooldown = CustomOptionHolder.sleuthCorpsesPathfindCooldown.getFloat();
            corpsesPathfindDuration = CustomOptionHolder.sleuthCorpsesPathfindDuration.getFloat();
            timer = 0;
            duration = CustomOptionHolder.sleuthCorpsesPathfindCooldown.getFloat();
            backUpduration = corpsesPathfindDuration;
        }
    }

    public static class Fink
    {
        public static PlayerControl fink;
        public static Color color = new Color32(184, 0, 50, byte.MaxValue);

        public static List<Arrow> localArrows = new List<Arrow>();
        public static int taskCountForImpostors = 1;
        public static float cooldown = 30f;
        public static float duration = 10f;
        public static float finkTimer = 0f;
        public static float backUpduration = 10f;
        public static GameObject finkCamera = null;
        public static GameObject finkShadow = null;

        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.FinkEyeButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            if (localArrows != null) {
                foreach (Arrow arrow in localArrows)
                    if (arrow?.arrow != null)
                        UnityEngine.Object.Destroy(arrow.arrow);
            }
            localArrows = new List<Arrow>();
            taskCountForImpostors = Mathf.RoundToInt(CustomOptionHolder.finkLeftTasksForImpostors.getFloat());
            fink = null;
            finkTimer = 0f;
            cooldown = CustomOptionHolder.finkCooldown.getFloat();
            duration = CustomOptionHolder.finkDuration.getFloat();
            backUpduration = duration;
            finkCamera = null;
            finkShadow = null;
        }

        public static void resetCamera() {
            finkTimer = 0f;
            if (finkCamera != null && finkShadow != null) {
                finkCamera.GetComponent<Camera>().orthographicSize = 3;
                finkShadow.SetActive(true);
            }
        }
    }

    public static class Kid
    {
        public static PlayerControl kid;
        public static Color color = new Color32(141, 255, 255, byte.MaxValue);
        public static bool triggerKidLose = false;

        public static void clearAndReload() {
            kid = null;
            triggerKidLose = false;
        }
    }

    public static class Welder
    {
        public static PlayerControl welder;
        public static Color color = new Color32(109, 91, 47, byte.MaxValue);

        public static float cooldown = 30f;
        public static int remainingWelds = 5;
        public static int totalWelds = 5;
        public static TMPro.TMP_Text welderButtonText;
        public static Vent ventTarget = null;
        public static List<Vent> ventsSealed = new List<Vent>();
        public static Vent bombedVent = null;

        private static Sprite bombVentButtonSprite;
        public static Sprite getBombVentButtonSprite() {
            if (bombVentButtonSprite) return bombVentButtonSprite;
            bombVentButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.WelderBombVentButton.png", 90f);
            return bombVentButtonSprite;
        }
        
        private static Sprite closeVentButtonSprite;
        public static Sprite getCloseVentButtonSprite() {
            if (closeVentButtonSprite) return closeVentButtonSprite;
            closeVentButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.WelderCloseVentButton.png", 90f);
            return closeVentButtonSprite;
        }

        private static Sprite animatedVentSealedSprite;
        public static Sprite getAnimatedVentSealedSprite() {
            if (animatedVentSealedSprite) return animatedVentSealedSprite;
            animatedVentSealedSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.WelderAnimatedVentSealed.png", 160f); // Change sprite and pixelPerUnit
            return animatedVentSealedSprite;
        }

        private static Sprite staticVentSealedSprite;
        public static Sprite getStaticVentSealedSprite() {
            if (staticVentSealedSprite) return staticVentSealedSprite;
            staticVentSealedSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.WelderStaticVentSealed.png", 160f); // Change sprite and pixelPerUnit
            return staticVentSealedSprite;
        }

        private static Sprite staticVentFungleSealedSprite;
        public static Sprite getFungleVentSealedSprite() {
            if (staticVentFungleSealedSprite) return staticVentFungleSealedSprite;
            staticVentFungleSealedSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.WelderPlantVentSealed.png", 160f); // Change sprite and pixelPerUnit
            return staticVentFungleSealedSprite;
        }

        public static void clearAndReload() {
            welder = null;
            ventTarget = null;
            cooldown = CustomOptionHolder.welderCooldown.getFloat();
            totalWelds = remainingWelds = Mathf.RoundToInt(CustomOptionHolder.welderTotalWelds.getFloat());
            ventsSealed.Clear();
            ventsSealed = new List<Vent>();
            bombedVent = null;
        }
    }

    public static class Spiritualist
    {
        public static PlayerControl spiritualist;
        public static Color color = new Color32(255, 197, 225, byte.MaxValue);
        public static float spiritualistReviveTime = 0f;
        private static Sprite buttonSprite;
        private static Sprite revivedPlayerKillButtonSprite;
        public static TMPro.TMP_Text revivedPlayerTimerCountButtonText;
        public static float revivedPlayerTimer = 21f;

        public static bool canRevive = false;
        public static PlayerControl revivedPlayer = null;
        public static PlayerControl revivedPlayerTarget = null;
        public static PlayerControl revivedPlayerKiller = null;
        public static bool revivePerRound = true;

        public static Sprite getRevivedPlayerRevengeButtonSprite() {
            if (revivedPlayerKillButtonSprite) return revivedPlayerKillButtonSprite;
            revivedPlayerKillButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SpiritualistRevenge.png", 90f);
            return revivedPlayerKillButtonSprite;
        }
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SpiritualistReviveButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            spiritualist = null;
            spiritualistReviveTime = 5f;
            canRevive = false;
            revivedPlayer = null;
            revivedPlayerTarget = null;
            revivedPlayerKiller = null;
            revivedPlayerTimer = 21;
            revivePerRound = true;
        }
    }

    public static class Coward
    {

        public static PlayerControl coward;
        public static Color color = new Color32(0, 247, 255, byte.MaxValue);

        public static int numberOfCalls;
        public static TMPro.TMP_Text cowardCallButtonText;
        private static Sprite buttonSprite;
        public static int rechargeTasksNumber = 3;
        public static int rechargedTasks = 3;
        public static int charges = 1;

        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.CowardCallButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            coward = null;
            numberOfCalls = (int)CustomOptionHolder.cowardNumberOfCalls.getFloat();
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.vigilantCamRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.vigilantCamRechargeTasksNumber.getFloat());
            charges = numberOfCalls;
        }
    }

    public static class Vigilant
    {
        public static PlayerControl vigilant;
        public static PlayerControl vigilantMira;
        public static bool doorLogActivated = true;

        public static Color color = new Color32(227, 225, 90, byte.MaxValue);

        public static float cooldown = 30f;
        public static TMPro.TMP_Text vigilantButtonCameraText;
        public static int totalCameras = 4;
        public static int remainingCameras = 0;
        public static int placedCameras = 0;

        public static bool createdDoorLog = false;
        public static GameObject doorLog = null;

        public static TMPro.TMP_Text vigilantButtonCameraUsesText;
        public static float duration = 10f;
        public static int maxCharges = 5;
        public static int rechargeTasksNumber = 3;
        public static int rechargedTasks = 3;
        public static int charges = 1;
        public static Minigame minigame = null;
        public static float backUpduration = 10f;

        private static Sprite camSprite;
        public static Sprite getCamSprite() {
            if (camSprite) return camSprite;
            camSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.VigilantViewCameraButton.png", 90f);
            return camSprite;
        }

        private static Sprite placeCameraButtonSprite;
        public static Sprite getPlaceCameraButtonSprite() {
            if (placeCameraButtonSprite) return placeCameraButtonSprite;
            placeCameraButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.VigilantCameraButton.png", 90f);
            return placeCameraButtonSprite;
        }

        public static void clearAndReload() {
            vigilant = null;
            vigilantMira = null;
            cooldown = CustomOptionHolder.vigilantCooldown.getFloat();
            totalCameras = 4;
            remainingCameras = totalCameras;
            if (GameOptionsManager.Instance.currentGameOptions.MapId >= 5) {
                placedCameras = 4;
            }
            else {
                placedCameras = 0;
            }
            createdDoorLog = false;
            doorLog = null;
            doorLogActivated = true;

            minigame = null;
            duration = CustomOptionHolder.vigilantCamDuration.getFloat();
            maxCharges = Mathf.RoundToInt(CustomOptionHolder.vigilantCamMaxCharges.getFloat());
            rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.vigilantCamRechargeTasksNumber.getFloat());
            rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.vigilantCamRechargeTasksNumber.getFloat());
            charges = Mathf.RoundToInt(CustomOptionHolder.vigilantCamMaxCharges.getFloat()) / 2;
            backUpduration = duration;
        }
    }

    public static class Hunter
    {
        public static PlayerControl hunter;
        public static Color color = new Color32(225, 235, 144, byte.MaxValue);

        public static bool resetTargetAfterMeeting = false;

        public static PlayerControl currentTarget;
        public static PlayerControl hunted;
        public static bool usedHunted = false;

        public static TMPro.TMP_Text targetButtonText;
        
        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.HunterSetMarkButton.png", 90f);
            return buttonSprite;
        }

        public static void resetHunted() {
            currentTarget = hunted = null;
            usedHunted = false; 
            targetButtonText.text = $" ";
        }

        public static void clearAndReload() {
            hunter = null;
            resetHunted();
            resetTargetAfterMeeting = CustomOptionHolder.hunterResetTargetAfterMeeting.getBool();
        }
    }

    public static class Jinx
    {
        public static PlayerControl jinx;
        public static PlayerControl target;
        public static Color color = new Color32(146, 139, 85, byte.MaxValue);
        public static List<PlayerControl> jinxedList = new List<PlayerControl>();
        public static int jinxs = 0;
        public static Sprite jinxButton;

        public static float cooldown = 30f;
        public static int jinxNumber = 5;

        public static TMPro.TMP_Text jinxButtonJinxsText;


        public static Sprite getTargetSprite() {
            if (jinxButton) return jinxButton;
            jinxButton = Helpers.loadSpriteFromResources("LasMonjas.Images.JinxButton.png", 90f);
            return jinxButton;
        }

        public static void clearAndReload() {
            jinx = null;
            target = null;
            jinxedList = new List<PlayerControl>();
            jinxs = 0;

            cooldown = CustomOptionHolder.jinxCooldown.getFloat();
            jinxNumber = Mathf.RoundToInt(CustomOptionHolder.jinxJinxsNumber.getFloat());
        }
    }

    public static class Bat
    {

        public static PlayerControl bat;
        public static Color color = new Color32(160, 50, 177, byte.MaxValue);

        public static float cooldown = 10f;
        public static float duration = 10f;
        public static float frequencyRange = 1f;
        public static float frequencyTimer = 0f;
        public static float backUpduration = 10f;

        private static Sprite buttonSprite;

        public static Sprite getFrequencyButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.BatEmitButton.png", 90f);
            return buttonSprite;
        }
        public static void clearAndReload() {
            bat = null;
            cooldown = CustomOptionHolder.batCooldown.getFloat();
            duration = CustomOptionHolder.batDuration.getFloat();
            frequencyRange = CustomOptionHolder.batRange.getFloat();
            frequencyTimer = 0f;
            backUpduration = duration;
        }
    }

    public static class Necromancer
    {

        public static PlayerControl necromancer;
        public static Color color = new Color32(184, 0, 50, byte.MaxValue);

        public static float cooldown;
        public static float duration;
        public static float roomDistance;

        public static Arrow reviveArrow = null;
        public static SystemTypes targetRoom;
        public static List<SystemTypes> Rooms;
        public static bool madeList = false;

        public static bool dragginBody = false;
        public static byte bodyId = 0;

        public static PlayerControl revivedPlayer = null;

        public static List<Arrow> localNecromancerArrows = new List<Arrow>(); 
        
        private static Sprite buttonSprite;
        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.NecromancerReviveButton.png", 90f);
            return buttonSprite;
        }

        private static Sprite buttonDragSprite;
        public static Sprite getDragButtonSprite() {
            if (buttonDragSprite) return buttonDragSprite;
            buttonDragSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.NecromancerPickButton.png", 90f);
            return buttonDragSprite;
        }

        private static Sprite buttonMoveSprite;
        public static Sprite getMoveBodyButtonSprite() {
            if (buttonMoveSprite) return buttonMoveSprite;
            buttonMoveSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.NecromancerDropButton.png", 90f);
            return buttonMoveSprite;
        }

        public static void clearAndReload() {
            necromancer = null;
            cooldown = CustomOptionHolder.necromancerReviveCooldown.getFloat();
            duration = CustomOptionHolder.necromancerReviveTimer.getFloat();
            roomDistance = CustomOptionHolder.necromancerMaxReviveRoomDistance.getFloat();
            reviveArrow = null;
            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    targetRoom = SystemTypes.MedBay;
                    break;
                case 1:
                    targetRoom = SystemTypes.Launchpad;
                    break;
                case 2:
                    targetRoom = SystemTypes.Office;
                    break;
                case 3:
                    targetRoom = SystemTypes.MedBay;
                    break;
                case 4:
                    targetRoom = SystemTypes.Medical;
                    break;
                case 5:
                    targetRoom = SystemTypes.FishingDock;
                    break;
                case 6:
                    targetRoom = SystemTypes.Comms;
                    break;
            }
            Rooms = new List<SystemTypes>();
            madeList = false;
            dragginBody = false;
            bodyId = 0;
            localNecromancerArrows = new List<Arrow>();
            revivedPlayer = null;
        }

        public static void CleanArrow() {
            if (reviveArrow != null) {
                UnityEngine.Object.Destroy(reviveArrow.arrow);
                reviveArrow = null;
            }
        }

        public static void necromancerResetValuesAtDead() {
            // Restore necromancer values when dead
            dragginBody = false;
            bodyId = 0; 
            CleanArrow();
        }
    }

    public static class Engineer
    {

        public static PlayerControl engineer;
        public static Color color = new Color32(127, 76, 0, byte.MaxValue);

        public static float cooldown = 0;
        public static float currentTrapNumber = 0;
        public static float numberOfTraps = 0;
        public static float trapsDuration = 0;
        public static float accelTrapIncrease = 0;
        public static float decelTrapDecrease = 0;
        public static byte trapType = 1;
        public static bool savedOldSpeed = false;
        public static float oldSpeed = 0;
        public static float messageTimer = 0f;

        private static Sprite buttonAccelSprite;
        public static Sprite getAccelButtonSprite() {
            if (buttonAccelSprite) return buttonAccelSprite;
            buttonAccelSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.EngineerAccelerateButton.png", 90f);
            return buttonAccelSprite;
        }

        private static Sprite buttonDecelSprite;
        public static Sprite getDecelButtonSprite() {
            if (buttonDecelSprite) return buttonDecelSprite;
            buttonDecelSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.EngineerSlowButton.png", 90f);
            return buttonDecelSprite;
        }

        private static Sprite buttonPositionSprite;
        public static Sprite getPositionButtonSprite() {
            if (buttonPositionSprite) return buttonPositionSprite;
            buttonPositionSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.EngineerDetectButton.png", 90f);
            return buttonPositionSprite;
        }

        public static void clearAndReload() {
            engineer = null;
            currentTrapNumber = 0;
            cooldown = CustomOptionHolder.engineerCooldown.getFloat();
            numberOfTraps = CustomOptionHolder.engineerNumberOfTraps.getFloat();
            trapsDuration = CustomOptionHolder.engineerTrapsDuration.getFloat();
            accelTrapIncrease = CustomOptionHolder.engineerAccelTrapIncrease.getFloat();
            decelTrapDecrease = CustomOptionHolder.engineerDecelTrapDecrease.getFloat();
            trapType = 1;
            savedOldSpeed = false;
            oldSpeed = 0;
            messageTimer = 0f;
        }
    }

    public static class Locksmith
    {

        public static PlayerControl locksmith;
        public static Color color = new Color32(242, 190, 255, byte.MaxValue);

        public static float cooldown = 30f;

        private static Sprite locksmithButtonSprite;
        public static Sprite getlocksmithButtonSprite() {
            if (locksmithButtonSprite) return locksmithButtonSprite;
            locksmithButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.LocksmithOpenButton.png", 90f);
            return locksmithButtonSprite;
        }

        public static void clearAndReload() {
            locksmith = null;
            cooldown = CustomOptionHolder.locksmithCooldown.getFloat();            
        }
    }

    public static class TaskMaster
    {
        public static PlayerControl taskMaster = null;
        public static Color color = new Color32(153, 153, 255, byte.MaxValue);
        public static bool clearedInitialTasks = false;
        public static byte oldTaskMasterPlayerId = byte.MaxValue;
        public static bool triggerTaskMasterCrewWin = false;
        public static float taskMasterAddCommonTasks = 0;
        public static float taskMasterAddLongTasks = 0;
        public static float taskMasterAddShortTasks = 0;
        public static float cooldown = 30f;
        public static float duration = 10f;
        public static float taskTimer = 0f;
        public static float backUpduration = 10f;
        public static byte rewardType = 0;
        public static bool hasKillButton = false;
        public static PlayerControl currentTarget;
        public static float killCooldown;

        private static Sprite taskMasterButtonSprite;
        public static Sprite gettaskMasterButtonSprite() {
            if (taskMasterButtonSprite) return taskMasterButtonSprite;
            taskMasterButtonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.TaskMasterSpeedButton.png", 90f);
            return taskMasterButtonSprite;
        }
        
        public static void clearAndReload() {
            taskMaster = null;
            clearedInitialTasks = false;
            oldTaskMasterPlayerId = byte.MaxValue;
            triggerTaskMasterCrewWin = false;
            taskMasterAddCommonTasks = Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraCommonTasks.getFloat());
            taskMasterAddLongTasks = Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraLongTasks.getFloat());
            taskMasterAddShortTasks = Mathf.RoundToInt(CustomOptionHolder.taskMasterExtraShortTasks.getFloat());
            taskTimer = 0f;
            cooldown = CustomOptionHolder.taskMasterCooldown.getFloat();
            duration = CustomOptionHolder.taskMasterDuration.getFloat();
            backUpduration = duration;
            rewardType = (byte)CustomOptionHolder.taskMasterRewardType.getSelection();
            hasKillButton = false;
            currentTarget = null;
            killCooldown = GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
        }
    }
    public static class Jailer
    {

        public static PlayerControl jailer;
        public static Color color = new Color32(204, 255, 204, byte.MaxValue);

        public static float cooldown = 30;
        public static float prisonDuration = 10;
        public static PlayerControl currentTarget;
        public static PlayerControl jailedPlayer;
        public static PlayerControl prisonPlayer;
        public static bool usedJail = false;

        public static TMPro.TMP_Text jailButtonText;
        
        private static Sprite buttonSprite;

        public static Sprite getButtonSprite() {
            if (buttonSprite) return buttonSprite;
            buttonSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.JailerPromoteButton.png", 90f);
            return buttonSprite;
        }

        public static void clearAndReload() {
            jailer = null;
            cooldown = CustomOptionHolder.jailerCooldown.getFloat();
            currentTarget = null;
            jailedPlayer = null;
            prisonPlayer = null;
            usedJail = false;
            prisonDuration = CustomOptionHolder.jailerDuration.getFloat();
        }

        public static void resetJailed() {
            jailedPlayer = null;
            prisonPlayer = null;
            usedJail = false; 
            jailButtonText.text = $" ";
        }
    }

    public static class Modifiers
    {
        public static Color color = new Color32(240, 128, 72, byte.MaxValue);
        public static Color loverscolor = new Color32(255, 0, 209, byte.MaxValue);

        public static PlayerControl lover1;
        public static PlayerControl lover2;
        public static PlayerControl lighter;
        public static PlayerControl blind;
        public static PlayerControl flash;
        public static PlayerControl bigchungus;
        public static PlayerControl theChosenOne;
        public static float chosenOneReportDelay = 0f;
        public static bool chosenOneReported = false;
        public static PlayerControl performer;
        public static float performerDuration = 15f;
        public static List<Arrow> performerLocalPerformerArrows = new List<Arrow>();
        public static bool performerReported = false;
        public static bool performerMusicStop = false;
        public static PlayerControl pro;
        public static PlayerControl paintball;
        public static Dictionary<byte, float> active = new Dictionary<byte, float>();
        public static Dictionary<byte, byte> paintballKillerMap = new Dictionary<byte, byte>();
        public static float paintballDuration = 10f; 
        public static PlayerControl electrician;
        public static float electricianDuration = 5f;
        public static bool electricianShock = false;

        // Lovers save if next to be exiled is a lover, because RPC of ending game comes before RPC of exiled
        public static bool notAckedExiledIsLover = false;

        public static bool existing() {
            return lover1 != null && lover2 != null && !lover1.Data.Disconnected && !lover2.Data.Disconnected;
        }

        public static bool existingAndAlive() {
            return existing() && !lover1.Data.IsDead && !lover2.Data.IsDead && !notAckedExiledIsLover; // ADD NOT ACKED IS LOVER
        }

        public static bool existingWithKiller() {
            return existing() && (lover1 == Renegade.renegade || lover2 == Renegade.renegade
                               || lover1 == Minion.minion || lover2 == Minion.minion
                               || lover1.Data.Role.IsImpostor || lover2.Data.Role.IsImpostor);
        }

        public static bool hasAliveKillingLover(this PlayerControl player) {
            if (!Modifiers.existingAndAlive() || !existingWithKiller())
                return false;
            return (player != null && (player == lover1 || player == lover2));
        }

        public static void clearAndReload() {
            lover1 = null;
            lover2 = null;
            notAckedExiledIsLover = false;
            blind = null;
            flash = null;
            bigchungus = null; 
            theChosenOne = null;
            chosenOneReported = false;
            chosenOneReportDelay = CustomOptionHolder.theChosenOneReportDelay.getFloat();
            performer = null;
            performerDuration = CustomOptionHolder.performerDuration.getFloat();
            performerLocalPerformerArrows = new List<Arrow>();
            performerReported = false;
            performerMusicStop = false;
            pro = null;
            paintball = null;
            paintballDuration = CustomOptionHolder.paintballDuration.getFloat();
            active = new Dictionary<byte, float>();
            paintballKillerMap = new Dictionary<byte, byte>();
            electrician = null;
            electricianDuration = CustomOptionHolder.electricianDuration.getFloat();
            electricianShock = false;
        }

        public static void ClearLovers() {
            lover1 = null;
            lover2 = null;
            notAckedExiledIsLover = false;
        }

        public static PlayerControl getPartner(this PlayerControl player) {
            if (player == null)
                return null;
            if (lover1 == player)
                return lover2;
            if (lover2 == player)
                return lover1;
            return null;
        }

    }

    public static class CaptureTheFlag
    {
        public static List<PlayerControl> redteamFlag = new List<PlayerControl>();
        public static PlayerControl redplayer01 = null;
        public static bool redplayer01IsReviving = false;
        public static PlayerControl redplayer01currentTarget = null;
        public static PlayerControl redplayer02 = null;
        public static bool redplayer02IsReviving = false;
        public static PlayerControl redplayer02currentTarget = null;
        public static PlayerControl redplayer03 = null;
        public static bool redplayer03IsReviving = false;
        public static PlayerControl redplayer03currentTarget = null;
        public static PlayerControl redplayer04 = null;
        public static bool redplayer04IsReviving = false;
        public static PlayerControl redplayer04currentTarget = null;
        public static PlayerControl redplayer05 = null;
        public static bool redplayer05IsReviving = false;
        public static PlayerControl redplayer05currentTarget = null;
        public static PlayerControl redplayer06 = null;
        public static bool redplayer06IsReviving = false;
        public static PlayerControl redplayer06currentTarget = null;
        public static PlayerControl redplayer07 = null;
        public static bool redplayer07IsReviving = false;
        public static PlayerControl redplayer07currentTarget = null;

        public static List<PlayerControl> blueteamFlag = new List<PlayerControl>();
        public static PlayerControl blueplayer01 = null;
        public static bool blueplayer01IsReviving = false;
        public static PlayerControl blueplayer01currentTarget = null;
        public static PlayerControl blueplayer02 = null;
        public static bool blueplayer02IsReviving = false;
        public static PlayerControl blueplayer02currentTarget = null;
        public static PlayerControl blueplayer03 = null;
        public static bool blueplayer03IsReviving = false;
        public static PlayerControl blueplayer03currentTarget = null;
        public static PlayerControl blueplayer04 = null;
        public static bool blueplayer04IsReviving = false;
        public static PlayerControl blueplayer04currentTarget = null;
        public static PlayerControl blueplayer05 = null;
        public static bool blueplayer05IsReviving = false;
        public static PlayerControl blueplayer05currentTarget = null;
        public static PlayerControl blueplayer06 = null;
        public static bool blueplayer06IsReviving = false;
        public static PlayerControl blueplayer06currentTarget = null;
        public static PlayerControl blueplayer07 = null;
        public static bool blueplayer07IsReviving = false;
        public static PlayerControl blueplayer07currentTarget = null;
        public static PlayerControl stealerPlayer = null;
        public static bool stealerPlayerIsReviving = false;
        public static PlayerControl stealerPlayercurrentTarget = null;
        public static List<GameObject> stealerSpawns = new List<GameObject>();

        public static float requiredFlags = 3;

        public static GameObject redflag = null;
        public static GameObject redflagbase = null;
        public static bool redflagtaken = false;
        public static PlayerControl redPlayerWhoHasBlueFlag = null;
        public static float currentRedTeamPoints = 0;
        public static List<Arrow> localRedFlagArrow = new List<Arrow>();

        public static GameObject blueflag = null;
        public static GameObject blueflagbase = null;
        public static bool blueflagtaken = false;
        public static PlayerControl bluePlayerWhoHasRedFlag = null;
        public static float currentBlueTeamPoints = 0;
        public static List<Arrow> localBlueFlagArrow = new List<Arrow>();

        public static bool triggerRedTeamWin = false;
        public static bool triggerBlueTeamWin = false;
        public static bool triggerDrawWin = false;

        public static string flagpointCounter = Language.introTexts[2] + "<color=#FF0000FF>" + currentRedTeamPoints + "</color> - " + "<color=#0000FFFF>" + currentBlueTeamPoints + "</color>";

        private static Sprite buttonSpriteTakeRedFlag;

        public static Sprite getTakeRedFlagButtonSprite() {
            if (buttonSpriteTakeRedFlag) return buttonSpriteTakeRedFlag;
            buttonSpriteTakeRedFlag = Helpers.loadSpriteFromResources("LasMonjas.Images.CaptureTheFlagStealRedFlagButton.png", 90f);
            return buttonSpriteTakeRedFlag;
        }

        private static Sprite buttonSpriteTakeBlueFlag;

        public static Sprite getTakeBlueFlagButtonSprite() {
            if (buttonSpriteTakeBlueFlag) return buttonSpriteTakeBlueFlag;
            buttonSpriteTakeBlueFlag = Helpers.loadSpriteFromResources("LasMonjas.Images.CaptureTheFlagStealBlueFlagButton.png", 90f);
            return buttonSpriteTakeBlueFlag;
        }

        private static Sprite buttonSpriteDeliverRedFlag;
        public static Sprite getDeliverRedFlagButtonSprite() {
            if (buttonSpriteDeliverRedFlag) return buttonSpriteDeliverRedFlag;
            buttonSpriteDeliverRedFlag = Helpers.loadSpriteFromResources("LasMonjas.Images.CaptureTheFlagDeliverRedFlagButton.png", 90f);
            return buttonSpriteDeliverRedFlag;
        }

        private static Sprite buttonSpriteDeliverBlueFlag;
        public static Sprite getDeliverBlueFlagButtonSprite() {
            if (buttonSpriteDeliverBlueFlag) return buttonSpriteDeliverBlueFlag;
            buttonSpriteDeliverBlueFlag = Helpers.loadSpriteFromResources("LasMonjas.Images.CaptureTheFlagDeliverBlueFlagButton.png", 90f);
            return buttonSpriteDeliverBlueFlag;
        }

        public static void clearAndReload() {
            redteamFlag.Clear();
            redplayer01 = null;
            redplayer01currentTarget = null;
            redplayer01IsReviving = false;
            redplayer02 = null;
            redplayer02IsReviving = false;
            redplayer02currentTarget = null;
            redplayer03 = null;
            redplayer03IsReviving = false;
            redplayer03currentTarget = null;
            redplayer04 = null;
            redplayer04IsReviving = false;
            redplayer04currentTarget = null;
            redplayer05 = null;
            redplayer05IsReviving = false;
            redplayer05currentTarget = null;
            redplayer06 = null;
            redplayer06IsReviving = false;
            redplayer06currentTarget = null;
            redplayer07 = null;
            redplayer07IsReviving = false;
            redplayer07currentTarget = null;
            blueteamFlag.Clear();
            blueplayer01 = null;
            blueplayer01IsReviving = false;
            blueplayer01currentTarget = null;
            blueplayer02 = null;
            blueplayer02IsReviving = false;
            blueplayer02currentTarget = null;
            blueplayer03 = null;
            blueplayer03IsReviving = false;
            blueplayer03currentTarget = null;
            blueplayer04 = null;
            blueplayer04IsReviving = false;
            blueplayer04currentTarget = null;
            blueplayer05 = null;
            blueplayer05IsReviving = false;
            blueplayer05currentTarget = null;
            blueplayer06 = null;
            blueplayer06IsReviving = false;
            blueplayer06currentTarget = null;
            blueplayer07 = null;
            blueplayer07IsReviving = false;
            blueplayer07currentTarget = null;
            stealerPlayer = null;
            stealerPlayerIsReviving = false;
            stealerPlayercurrentTarget = null;
            stealerSpawns.Clear();

            requiredFlags = CustomOptionHolder.requiredFlags.getFloat();
            redflag = null;
            redflagbase = null;
            redflagtaken = false;
            redPlayerWhoHasBlueFlag = null;
            currentRedTeamPoints = 0;
            blueflag = null;
            blueflagbase = null;
            blueflagtaken = false;
            bluePlayerWhoHasRedFlag = null;
            triggerRedTeamWin = false;
            triggerBlueTeamWin = false;
            triggerDrawWin = false;
            currentBlueTeamPoints = 0;
            localRedFlagArrow = new List<Arrow>();
            localBlueFlagArrow = new List<Arrow>();
            flagpointCounter = Language.introTexts[2] + "<color=#FF0000FF>" + currentRedTeamPoints + "</color> - " + "<color=#0000FFFF>" + currentBlueTeamPoints + "</color>";
        }
    }

    public static class PoliceAndThief
    {
        public static List<PlayerControl> thiefTeam = new List<PlayerControl>();
        public static PlayerControl thiefplayer01 = null;
        public static PlayerControl thiefplayer01currentTarget = null;
        public static bool thiefplayer01IsStealing = false;
        public static byte thiefplayer01JewelId = 0;
        public static bool thiefplayer01IsReviving = false;
        public static PlayerControl thiefplayer02 = null;
        public static PlayerControl thiefplayer02currentTarget = null;
        public static bool thiefplayer02IsStealing = false;
        public static byte thiefplayer02JewelId = 0;
        public static bool thiefplayer02IsReviving = false;
        public static PlayerControl thiefplayer03 = null;
        public static PlayerControl thiefplayer03currentTarget = null;
        public static bool thiefplayer03IsStealing = false;
        public static byte thiefplayer03JewelId = 0;
        public static bool thiefplayer03IsReviving = false;
        public static PlayerControl thiefplayer04 = null;
        public static PlayerControl thiefplayer04currentTarget = null;
        public static bool thiefplayer04IsStealing = false;
        public static byte thiefplayer04JewelId = 0;
        public static bool thiefplayer04IsReviving = false;
        public static PlayerControl thiefplayer05 = null;
        public static PlayerControl thiefplayer05currentTarget = null;
        public static bool thiefplayer05IsStealing = false;
        public static byte thiefplayer05JewelId = 0;
        public static bool thiefplayer05IsReviving = false;
        public static PlayerControl thiefplayer06 = null;
        public static PlayerControl thiefplayer06currentTarget = null;
        public static bool thiefplayer06IsStealing = false;
        public static byte thiefplayer06JewelId = 0;
        public static bool thiefplayer06IsReviving = false;
        public static PlayerControl thiefplayer07 = null;
        public static PlayerControl thiefplayer07currentTarget = null;
        public static bool thiefplayer07IsStealing = false;
        public static byte thiefplayer07JewelId = 0;
        public static bool thiefplayer07IsReviving = false;
        public static PlayerControl thiefplayer08 = null;
        public static PlayerControl thiefplayer08currentTarget = null;
        public static bool thiefplayer08IsStealing = false;
        public static byte thiefplayer08JewelId = 0;
        public static bool thiefplayer08IsReviving = false;
        public static PlayerControl thiefplayer09 = null;
        public static PlayerControl thiefplayer09currentTarget = null;
        public static bool thiefplayer09IsStealing = false;
        public static byte thiefplayer09JewelId = 0;
        public static bool thiefplayer09IsReviving = false;

        public static List<PlayerControl> policeTeam = new List<PlayerControl>();
        public static PlayerControl policeplayer01 = null;
        public static PlayerControl policeplayer01currentTarget = null;
        public static PlayerControl policeplayer01targetedPlayer = null;
        public static float policeplayer01lightTimer = 0;
        public static bool policeplayer01IsReviving = false;
        public static PlayerControl policeplayer02 = null;
        public static PlayerControl policeplayer02currentTarget = null;
        public static float policeplayer02lightTimer = 0;
        public static bool policeplayer02IsReviving = false;
        public static GameObject policeplayer02Taser = null;
        public static float policeplayer02mouseAngle = 0f;
        public static PlayerControl policeplayer03 = null;
        public static PlayerControl policeplayer03currentTarget = null;
        public static PlayerControl policeplayer03targetedPlayer = null;
        public static float policeplayer03lightTimer = 0;
        public static bool policeplayer03IsReviving = false;
        public static PlayerControl policeplayer04 = null;
        public static PlayerControl policeplayer04currentTarget = null;
        public static float policeplayer04lightTimer = 0;
        public static bool policeplayer04IsReviving = false;
        public static GameObject policeplayer04Taser = null;
        public static float policeplayer04mouseAngle = 0f; 
        public static PlayerControl policeplayer05 = null;
        public static PlayerControl policeplayer05currentTarget = null;
        public static PlayerControl policeplayer05targetedPlayer = null;
        public static float policeplayer05lightTimer = 0;
        public static bool policeplayer05IsReviving = false; 
        public static PlayerControl policeplayer06 = null;
        public static PlayerControl policeplayer06currentTarget = null;
        public static PlayerControl policeplayer06targetedPlayer = null;
        public static float policeplayer06lightTimer = 0;
        public static bool policeplayer06IsReviving = false;

        public static List<PlayerControl> thiefArrested = new List<PlayerControl>();
        public static List<GameObject> thiefTreasures = new List<GameObject>();
        public static GameObject cell = null;
        public static GameObject cellbutton = null;
        public static GameObject jewelbutton = null;

        public static GameObject celltwo = null;
        public static GameObject cellbuttontwo = null;
        public static GameObject jewelbuttontwo = null; 

        public static GameObject jewel01 = null;
        public static PlayerControl jewel01BeingStealed = null;
        public static GameObject jewel02 = null;
        public static PlayerControl jewel02BeingStealed = null;
        public static GameObject jewel03 = null;
        public static PlayerControl jewel03BeingStealed = null;
        public static GameObject jewel04 = null;
        public static PlayerControl jewel04BeingStealed = null;
        public static GameObject jewel05 = null;
        public static PlayerControl jewel05BeingStealed = null;
        public static GameObject jewel06 = null;
        public static PlayerControl jewel06BeingStealed = null;
        public static GameObject jewel07 = null;
        public static PlayerControl jewel07BeingStealed = null;
        public static GameObject jewel08 = null;
        public static PlayerControl jewel08BeingStealed = null;
        public static GameObject jewel09 = null;
        public static PlayerControl jewel09BeingStealed = null;
        public static GameObject jewel10 = null;
        public static PlayerControl jewel10BeingStealed = null;
        public static GameObject jewel11 = null;
        public static PlayerControl jewel11BeingStealed = null;
        public static GameObject jewel12 = null;
        public static PlayerControl jewel12BeingStealed = null;
        public static GameObject jewel13 = null;
        public static PlayerControl jewel13BeingStealed = null;
        public static GameObject jewel14 = null;
        public static PlayerControl jewel14BeingStealed = null;
        public static GameObject jewel15 = null;
        public static PlayerControl jewel15BeingStealed = null;

        public static float requiredJewels = 10;
        public static float policeTaseCooldown = 20f;
        public static float policeTaseDuration = 5f;
        public static bool policeCanSeeJewels = false;
        public static float policeCatchCooldown = 10f;
        public static float captureThiefTime = 3f;
        public static int whoCanThiefsKill = 0;

        public static float currentJewelsStoled = 0;
        public static float currentThiefsCaptured = 0;

        public static bool triggerThiefWin = false;
        public static bool triggerPoliceWin = false;

        public static string thiefpointCounter = Language.introTexts[3] + "<color=#FF0000FF>" + currentJewelsStoled + " / " + requiredJewels + "</color> | " + Language.introTexts[4] + "<color=#0000FFFF>" + currentThiefsCaptured + " / 10</color>";

        public static List<Arrow> localThiefReleaseArrow = new List<Arrow>();
        public static List<Arrow> localThiefDeliverArrow = new List<Arrow>();


        private static Sprite buttonSpriteLight;

        public static Sprite getLightButtonSprite() {
            if (buttonSpriteLight) return buttonSpriteLight;
            buttonSpriteLight = Helpers.loadSpriteFromResources("LasMonjas.Images.PoliceAndThiefsLightButton.png", 90f);
            return buttonSpriteLight;
        }

        private static Sprite buttonSpriteCaptureThief;

        public static Sprite getCaptureThiefButtonSprite() {
            if (buttonSpriteCaptureThief) return buttonSpriteCaptureThief;
            buttonSpriteCaptureThief = Helpers.loadSpriteFromResources("LasMonjas.Images.PoliceAndThiefCaptureButton.png", 90f);
            return buttonSpriteCaptureThief;
        }

        private static Sprite buttonSpriteFreeThief;

        public static Sprite getFreeThiefButtonSprite() {
            if (buttonSpriteFreeThief) return buttonSpriteFreeThief;
            buttonSpriteFreeThief = Helpers.loadSpriteFromResources("LasMonjas.Images.PoliceAndThiefFreeButton.png", 90f);
            return buttonSpriteFreeThief;
        }


        private static Sprite buttonSpriteDeliverJewel;

        public static Sprite getDeliverJewelButtonSprite() {
            if (buttonSpriteDeliverJewel) return buttonSpriteDeliverJewel;
            buttonSpriteDeliverJewel = Helpers.loadSpriteFromResources("LasMonjas.Images.PoliceAndThiefDeliverJewelButton.png", 90f);
            return buttonSpriteDeliverJewel;
        }

        private static Sprite buttonSpriteTakeJewel;

        public static Sprite getTakeJewelButtonSprite() {
            if (buttonSpriteTakeJewel) return buttonSpriteTakeJewel;
            buttonSpriteTakeJewel = Helpers.loadSpriteFromResources("LasMonjas.Images.PoliceAndThiefTakeJewelButton.png", 90f);
            return buttonSpriteTakeJewel;
        }

        private static Sprite taserSprite;

        public static Sprite getTaserSprite() {
            if (taserSprite) return taserSprite;
            taserSprite = Helpers.loadSpriteFromResources("LasMonjas.Images.PoliceAndThiefsTaser.png", 100f);
            return taserSprite;
        }

        private static Sprite buttonSpriteTaserThief;
        public static Sprite getTaserThiefButtonSprite() {
            if (buttonSpriteTaserThief) return buttonSpriteTaserThief;
            buttonSpriteTaserThief = Helpers.loadSpriteFromResources("LasMonjas.Images.PoliceAndThiefsTaserButton.png", 90f);
            return buttonSpriteTaserThief;
        }

        public static void clearAndReload() {
            cell = null;
            cellbutton = null;
            jewelbutton = null;
            thiefTreasures.Clear();
            thiefArrested.Clear();

            celltwo = null;
            cellbuttontwo = null;
            jewelbuttontwo = null;

            thiefTeam.Clear();
            thiefplayer01 = null;
            thiefplayer01currentTarget = null;
            thiefplayer01IsStealing = false;
            thiefplayer01JewelId = 0;
            thiefplayer01IsReviving = false;
            thiefplayer02 = null;
            thiefplayer02currentTarget = null;
            thiefplayer02IsStealing = false;
            thiefplayer02JewelId = 0;
            thiefplayer02IsReviving = false;
            thiefplayer03 = null;
            thiefplayer03currentTarget = null;
            thiefplayer03IsStealing = false;
            thiefplayer03JewelId = 0;
            thiefplayer03IsReviving = false;
            thiefplayer04 = null;
            thiefplayer04currentTarget = null;
            thiefplayer04IsStealing = false;
            thiefplayer04JewelId = 0;
            thiefplayer04IsReviving = false;
            thiefplayer05 = null;
            thiefplayer05currentTarget = null;
            thiefplayer05IsStealing = false;
            thiefplayer05JewelId = 0;
            thiefplayer05IsReviving = false;
            thiefplayer06 = null;
            thiefplayer06currentTarget = null;
            thiefplayer06IsStealing = false;
            thiefplayer06JewelId = 0;
            thiefplayer06IsReviving = false;
            thiefplayer07 = null;
            thiefplayer07currentTarget = null;
            thiefplayer07IsStealing = false;
            thiefplayer07JewelId = 0;
            thiefplayer07IsReviving = false;
            thiefplayer08 = null;
            thiefplayer08currentTarget = null;
            thiefplayer08IsStealing = false;
            thiefplayer08JewelId = 0;
            thiefplayer08IsReviving = false;
            thiefplayer09 = null;
            thiefplayer09currentTarget = null;
            thiefplayer09IsStealing = false;
            thiefplayer09JewelId = 0;
            thiefplayer09IsReviving = false;

            policeTeam.Clear();
            policeplayer01 = null;
            policeplayer01currentTarget = null;
            policeplayer01targetedPlayer = null;
            policeplayer01lightTimer = 0;
            policeplayer01IsReviving = false;
            policeplayer02 = null;
            policeplayer02currentTarget = null;
            policeplayer02lightTimer = 0;
            policeplayer02IsReviving = false;
            policeplayer02Taser = null;
            policeplayer02mouseAngle = 0f;
            policeplayer03 = null;
            policeplayer03currentTarget = null;
            policeplayer03targetedPlayer = null;
            policeplayer03lightTimer = 0;
            policeplayer03IsReviving = false;
            policeplayer04 = null;
            policeplayer04currentTarget = null;
            policeplayer04lightTimer = 0;
            policeplayer04IsReviving = false;
            policeplayer04Taser = null;
            policeplayer04mouseAngle = 0f; 
            policeplayer05 = null;
            policeplayer05currentTarget = null;
            policeplayer05targetedPlayer = null;
            policeplayer05lightTimer = 0;
            policeplayer05IsReviving = false;
            policeplayer06 = null;
            policeplayer06currentTarget = null;
            policeplayer06targetedPlayer = null;
            policeplayer06lightTimer = 0;
            policeplayer06IsReviving = false;

            jewel01 = null;
            jewel01BeingStealed = null;
            jewel02 = null;
            jewel02BeingStealed = null;
            jewel03 = null;
            jewel03BeingStealed = null;
            jewel04 = null;
            jewel04BeingStealed = null;
            jewel05 = null;
            jewel05BeingStealed = null;
            jewel06 = null;
            jewel06BeingStealed = null;
            jewel07 = null;
            jewel07BeingStealed = null;
            jewel08 = null;
            jewel08BeingStealed = null;
            jewel09 = null;
            jewel09BeingStealed = null;
            jewel10 = null;
            jewel10BeingStealed = null;
            jewel11 = null;
            jewel11BeingStealed = null;
            jewel12 = null;
            jewel12BeingStealed = null;
            jewel13 = null;
            jewel13BeingStealed = null;
            jewel14 = null;
            jewel14BeingStealed = null;
            jewel15 = null;
            jewel15BeingStealed = null;

            localThiefReleaseArrow = new List<Arrow>();
            localThiefDeliverArrow = new List<Arrow>();

            requiredJewels = CustomOptionHolder.thiefModerequiredJewels.getFloat();
            policeTaseCooldown = CustomOptionHolder.thiefModePoliceTaseCooldown.getFloat();
            policeTaseDuration = CustomOptionHolder.thiefModePoliceTaseDuration.getFloat();
            policeCanSeeJewels = CustomOptionHolder.thiefModePoliceCanSeeJewels.getBool();
            policeCatchCooldown = CustomOptionHolder.thiefModePoliceCatchCooldown.getFloat();
            captureThiefTime = CustomOptionHolder.thiefModecaptureThiefTime.getFloat();
            whoCanThiefsKill = CustomOptionHolder.thiefModeWhoCanThiefsKill.getSelection();
            currentJewelsStoled = 0;
            triggerThiefWin = false;
            triggerPoliceWin = false;
            currentThiefsCaptured = 0;
            thiefpointCounter = Language.introTexts[3] + "<color=#00F7FFFF>" + currentJewelsStoled + " / " + requiredJewels + "</color> | " + Language.introTexts[4] + " <color=#928B55FF>" + currentThiefsCaptured + " / " + thiefTeam.Count + "</color>";
        }
        public static PlayerControl GetTasedPlayer(float shotSize, float effectiveRange, bool policeTwo) {
            PlayerControl result = null;
            float num = effectiveRange;
            Vector3 pos;
            float mouseAngle;
            if (policeTwo) {
                mouseAngle = policeplayer02mouseAngle;
            } else {
                mouseAngle = policeplayer04mouseAngle;
            }
            foreach (PlayerControl player in thiefTeam) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead || player.inVent) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    result = player;
                }
            }
            return result;
        }
    }

    public static class KingOfTheHill
    {
        public static List<PlayerControl> greenTeam = new List<PlayerControl>();
        public static byte whichGreenKingplayerzone = 0;
        public static int totalGreenKingzonescaptured = 0;
        public static bool greenKinghaszoneone = false;
        public static bool greenKinghaszonetwo = false;
        public static bool greenKinghaszonethree = false;
        public static PlayerControl greenKingplayer = null;
        public static PlayerControl greenKingplayercurrentTarget = null;
        public static bool greenKingIsReviving = false;
        public static PlayerControl greenplayer01 = null;
        public static PlayerControl greenplayer01currentTarget = null;
        public static bool greenplayer01IsReviving = false;
        public static PlayerControl greenplayer02 = null;
        public static PlayerControl greenplayer02currentTarget = null;
        public static bool greenplayer02IsReviving = false;
        public static PlayerControl greenplayer03 = null;
        public static PlayerControl greenplayer03currentTarget = null;
        public static bool greenplayer03IsReviving = false;
        public static PlayerControl greenplayer04 = null;
        public static PlayerControl greenplayer04currentTarget = null;
        public static bool greenplayer04IsReviving = false;
        public static PlayerControl greenplayer05 = null;
        public static PlayerControl greenplayer05currentTarget = null;
        public static bool greenplayer05IsReviving = false;
        public static PlayerControl greenplayer06 = null;
        public static PlayerControl greenplayer06currentTarget = null;
        public static bool greenplayer06IsReviving = false;

        public static List<PlayerControl> yellowTeam = new List<PlayerControl>();
        public static byte whichYellowKingplayerzone = 0;
        public static int totalYellowKingzonescaptured = 0;
        public static bool yellowKinghaszoneone = false;
        public static bool yellowKinghaszonetwo = false;
        public static bool yellowKinghaszonethree = false;
        public static PlayerControl yellowKingplayer = null;
        public static PlayerControl yellowKingplayercurrentTarget = null;
        public static bool yellowKingIsReviving = false;
        public static PlayerControl yellowplayer01 = null;
        public static PlayerControl yellowplayer01currentTarget = null;
        public static bool yellowplayer01IsReviving = false;
        public static PlayerControl yellowplayer02 = null;
        public static PlayerControl yellowplayer02currentTarget = null;
        public static bool yellowplayer02IsReviving = false;
        public static PlayerControl yellowplayer03 = null;
        public static PlayerControl yellowplayer03currentTarget = null;
        public static bool yellowplayer03IsReviving = false;
        public static PlayerControl yellowplayer04 = null;
        public static PlayerControl yellowplayer04currentTarget = null;
        public static bool yellowplayer04IsReviving = false;
        public static PlayerControl yellowplayer05 = null;
        public static PlayerControl yellowplayer05currentTarget = null;
        public static bool yellowplayer05IsReviving = false;
        public static PlayerControl yellowplayer06 = null;
        public static PlayerControl yellowplayer06currentTarget = null;
        public static bool yellowplayer06IsReviving = false;
        public static PlayerControl usurperPlayer = null;
        public static PlayerControl usurperPlayercurrentTarget = null;
        public static bool usurperPlayerIsReviving = false;
        public static List<GameObject> usurperSpawns = new List<GameObject>();

        public static float requiredPoints = 150;
        public static float captureCooldown = 10f;

        public static GameObject greenflag = null;
        public static float currentGreenTeamPoints = 0;

        public static GameObject yellowflag = null;
        public static float currentYellowTeamPoints = 0;

        public static List<GameObject> kingZones = new List<GameObject>();
        public static GameObject flagzoneone = null;
        public static GameObject flagzonetwo = null;
        public static GameObject flagzonethree = null;
        public static GameObject zoneone = null;
        public static GameObject zonetwo = null;
        public static GameObject zonethree = null;
        public static Color zoneonecolor = Color.white;
        public static Color zonetwocolor = Color.white;
        public static Color zonethreecolor = Color.white;
        public static List<Arrow> localArrows = new List<Arrow>();
        public static GameObject greenkingaura = null;
        public static GameObject yellowkingaura = null;

        public static bool triggerGreenTeamWin = false;
        public static bool triggerYellowTeamWin = false;
        public static bool triggerDrawWin = false;

        public static string kingpointCounter = Language.introTexts[2] + "<color=#00FF00FF>" + currentGreenTeamPoints.ToString("F0") + "</color> - " + "<color=#FFFF00FF>" + currentYellowTeamPoints.ToString("F0") + "</color>";

        private static Sprite buttonSpritePlaceGreenFlag;

        public static Sprite getPlaceGreenFlagButtonSprite() {
            if (buttonSpritePlaceGreenFlag) return buttonSpritePlaceGreenFlag;
            buttonSpritePlaceGreenFlag = Helpers.loadSpriteFromResources("LasMonjas.Images.KingOfTheHillGreenCapture.png", 90f);
            return buttonSpritePlaceGreenFlag;
        }

        private static Sprite buttonSpritePlaceYellowFlag;

        public static Sprite getPlaceYellowFlagButtonSprite() {
            if (buttonSpritePlaceYellowFlag) return buttonSpritePlaceYellowFlag;
            buttonSpritePlaceYellowFlag = Helpers.loadSpriteFromResources("LasMonjas.Images.KingOfTheHillYellowCapture.png", 90f);
            return buttonSpritePlaceYellowFlag;
        }

        public static void clearAndReload() {
            greenTeam.Clear();
            whichGreenKingplayerzone = 0;
            totalGreenKingzonescaptured = 0;
            greenKinghaszoneone = false;
            greenKinghaszonetwo = false;
            greenKinghaszonethree = false;
            greenKingplayer = null;
            greenKingplayercurrentTarget = null;
            greenKingIsReviving = false;
            greenplayer01 = null;
            greenplayer01currentTarget = null;
            greenplayer01IsReviving = false;
            greenplayer02 = null;
            greenplayer02currentTarget = null;
            greenplayer02IsReviving = false;
            greenplayer03 = null;
            greenplayer03currentTarget = null;
            greenplayer03IsReviving = false;
            greenplayer04 = null;
            greenplayer04currentTarget = null;
            greenplayer04IsReviving = false;
            greenplayer05 = null;
            greenplayer05currentTarget = null;
            greenplayer05IsReviving = false;
            greenplayer06 = null;
            greenplayer06currentTarget = null;
            greenplayer06IsReviving = false;

            yellowTeam.Clear();
            whichYellowKingplayerzone = 0;
            totalYellowKingzonescaptured = 0;
            yellowKinghaszoneone = false;
            yellowKinghaszonetwo = false;
            yellowKinghaszonethree = false;
            yellowKingplayer = null;
            yellowKingplayercurrentTarget = null;
            yellowKingIsReviving = false;
            yellowplayer01 = null;
            yellowplayer01currentTarget = null;
            yellowplayer01IsReviving = false;
            yellowplayer02 = null;
            yellowplayer02currentTarget = null;
            yellowplayer02IsReviving = false;
            yellowplayer03 = null;
            yellowplayer03currentTarget = null;
            yellowplayer03IsReviving = false;
            yellowplayer04 = null;
            yellowplayer04currentTarget = null;
            yellowplayer04IsReviving = false;
            yellowplayer05 = null;
            yellowplayer05currentTarget = null;
            yellowplayer05IsReviving = false;
            yellowplayer06 = null;
            yellowplayer06currentTarget = null;
            yellowplayer06IsReviving = false;
            usurperPlayer = null;
            usurperPlayercurrentTarget = null;
            usurperPlayerIsReviving = false;
            usurperSpawns.Clear();

            requiredPoints = CustomOptionHolder.kingRequiredPoints.getFloat();
            captureCooldown = CustomOptionHolder.kingCaptureCooldown.getFloat();

            greenflag = null;
            currentGreenTeamPoints = 0;

            yellowflag = null;
            currentYellowTeamPoints = 0;

            kingZones.Clear();
            flagzoneone = null;
            flagzonetwo = null;
            flagzonethree = null;
            zoneone = null;
            zonetwo = null;
            zonethree = null;
            zoneonecolor = Color.white;
            zonetwocolor = Color.white;
            zonethreecolor = Color.white;
            greenkingaura = null;
            yellowkingaura = null;
            triggerGreenTeamWin = false;
            triggerYellowTeamWin = false;
            triggerDrawWin = false;

            localArrows = new List<Arrow>();
            kingpointCounter = Language.introTexts[2] + "<color=#00FF00FF>" + currentGreenTeamPoints.ToString("F0") + "</color> - " + "<color=#FFFF00FF>" + currentYellowTeamPoints.ToString("F0") + "</color>";
        }
    }

    public static class HotPotato
    {
        public static List<PlayerControl> notPotatoTeam = new List<PlayerControl>();
        public static List<PlayerControl> notPotatoTeamAlive = new List<PlayerControl>();
        public static List<PlayerControl> explodedPotatoTeam = new List<PlayerControl>();

        public static PlayerControl hotPotatoPlayer = null;
        public static PlayerControl hotPotatoPlayerCurrentTarget = null;
        public static PlayerControl notPotato01 = null;
        public static PlayerControl notPotato02 = null;
        public static PlayerControl notPotato03 = null;
        public static PlayerControl notPotato04 = null;
        public static PlayerControl notPotato05 = null;
        public static PlayerControl notPotato06 = null;
        public static PlayerControl notPotato07 = null;
        public static PlayerControl notPotato08 = null;
        public static PlayerControl notPotato09 = null;
        public static PlayerControl notPotato10 = null;
        public static PlayerControl notPotato11 = null;
        public static PlayerControl notPotato12 = null;
        public static PlayerControl notPotato13 = null;
        public static PlayerControl notPotato14 = null;

        public static PlayerControl explodedPotato01 = null;
        public static PlayerControl explodedPotato02 = null;
        public static PlayerControl explodedPotato03 = null;
        public static PlayerControl explodedPotato04 = null;
        public static PlayerControl explodedPotato05 = null;
        public static PlayerControl explodedPotato06 = null;
        public static PlayerControl explodedPotato07 = null;
        public static PlayerControl explodedPotato08 = null;
        public static PlayerControl explodedPotato09 = null;
        public static PlayerControl explodedPotato10 = null;
        public static PlayerControl explodedPotato11 = null;
        public static PlayerControl explodedPotato12 = null;
        public static PlayerControl explodedPotato13 = null;
        public static PlayerControl explodedPotato14 = null;

        public static GameObject hotPotato = null;

        public static float timeforTransfer = 15;
        public static float transferCooldown = 10f;
        public static float savedtimeforTransfer = 15;
        public static bool resetTimeForTransfer = true;
        public static float increaseTimeIfNoReset = 5f; 
        public static bool firstPotatoTransfered = false;

        public static bool notPotatoTeamAlerted = false;

        public static bool triggerHotPotatoEnd = false;

        public static string hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF></color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + notPotatoTeam.Count + "</color>";

        private static Sprite buttonPotato;

        public static Sprite getButtonSprite() {
            if (buttonPotato) return buttonPotato;
            buttonPotato = Helpers.loadSpriteFromResources("LasMonjas.Images.HotPotatoHotPotatusButton.png", 90f);
            return buttonPotato;
        }

        public static void clearAndReload() {
            notPotatoTeam.Clear();
            notPotatoTeamAlive.Clear();
            hotPotatoPlayer = null;
            hotPotatoPlayerCurrentTarget = null;
            notPotato01 = null;
            notPotato02 = null;
            notPotato03 = null;
            notPotato04 = null;
            notPotato05 = null;
            notPotato06 = null;
            notPotato07 = null;
            notPotato08 = null;
            notPotato09 = null;
            notPotato10 = null;
            notPotato11 = null;
            notPotato12 = null;
            notPotato13 = null;
            notPotato14 = null;

            explodedPotato01 = null;
            explodedPotato02 = null;
            explodedPotato03 = null;
            explodedPotato04 = null;
            explodedPotato05 = null;
            explodedPotato06 = null;
            explodedPotato07 = null;
            explodedPotato08 = null;
            explodedPotato09 = null;
            explodedPotato10 = null;
            explodedPotato11 = null;
            explodedPotato12 = null;
            explodedPotato13 = null;
            explodedPotato14 = null;

            timeforTransfer = CustomOptionHolder.hotPotatoTransferLimit.getFloat() + 10f;
            transferCooldown = CustomOptionHolder.hotPotatoCooldown.getFloat();
            resetTimeForTransfer = CustomOptionHolder.hotPotatoResetTimeForTransfer.getBool();
            increaseTimeIfNoReset = CustomOptionHolder.hotPotatoIncreaseTimeIfNoReset.getFloat(); 
            notPotatoTeamAlerted = false;
            triggerHotPotatoEnd = false;
            savedtimeforTransfer = timeforTransfer - 10f;
            firstPotatoTransfered = false;
            hotPotato = null;

            hotpotatopointCounter = Language.introTexts[5] + "<color=#00F7FFFF></color> | " + Language.introTexts[6] + "<color=#928B55FF>" + notPotatoTeam.Count + "</color>";
        }
    }

    public static class ZombieLaboratory
    {
        public static int currentKeyItems = 0;
        public static List<PlayerControl> survivorTeam = new List<PlayerControl>();
        public static List<PlayerControl> zombieTeam = new List<PlayerControl>();
        public static List<GameObject> groundItems = new List<GameObject>();
        public static List<PlayerControl> infectedTeam = new List<PlayerControl>();
        public static List<GameObject> nurseExits = new List<GameObject>();
        public static List<GameObject> nurseMedkits = new List<GameObject>();
        public static List<GameObject> laboratoryEntrances = new List<GameObject>();

        public static List<Vector3> susBoxPositions = new List<Vector3>();
        public static List<Arrow> localNurseArrows = new List<Arrow>();

        public static PlayerControl nursePlayer = null;
        public static PlayerControl nursePlayercurrentTarget = null;
        public static bool nursePlayerHasMedKit = false;
        public static bool nursePlayerInsideLaboratory = true;
        public static bool nursePlayerHasCureReady = false;
        public static byte nursePlayerCurrentExit = 0;
        public static PlayerControl survivorPlayer01 = null;
        public static PlayerControl survivorPlayer01currentTarget = null;
        public static bool survivorPlayer01IsReviving = false;
        public static bool survivorPlayer01IsInfected = false;
        public static bool survivorPlayer01CanKill = false;
        public static bool survivorPlayer01HasKeyItem = false;
        public static byte survivorPlayer01FoundBox = 0;
        public static float survivorPlayer01Timer = 60;
        public static GameObject survivorPlayer01SelectedBox = null;
        public static GameObject survivorPlayer01CurrentBox = null;
        public static PlayerControl survivorPlayer02 = null;
        public static PlayerControl survivorPlayer02currentTarget = null;
        public static bool survivorPlayer02IsReviving = false;
        public static bool survivorPlayer02IsInfected = false;
        public static bool survivorPlayer02CanKill = false;
        public static bool survivorPlayer02HasKeyItem = false;
        public static byte survivorPlayer02FoundBox = 0;
        public static float survivorPlayer02Timer = 60;
        public static GameObject survivorPlayer02SelectedBox = null;
        public static GameObject survivorPlayer02CurrentBox = null;
        public static PlayerControl survivorPlayer03 = null;
        public static PlayerControl survivorPlayer03currentTarget = null;
        public static bool survivorPlayer03IsReviving = false;
        public static bool survivorPlayer03IsInfected = false;
        public static bool survivorPlayer03CanKill = false;
        public static bool survivorPlayer03HasKeyItem = false;
        public static byte survivorPlayer03FoundBox = 0;
        public static float survivorPlayer03Timer = 60;
        public static GameObject survivorPlayer03SelectedBox = null;
        public static GameObject survivorPlayer03CurrentBox = null;
        public static PlayerControl survivorPlayer04 = null;
        public static PlayerControl survivorPlayer04currentTarget = null;
        public static bool survivorPlayer04IsReviving = false;
        public static bool survivorPlayer04IsInfected = false;
        public static bool survivorPlayer04CanKill = false;
        public static bool survivorPlayer04HasKeyItem = false;
        public static byte survivorPlayer04FoundBox = 0;
        public static float survivorPlayer04Timer = 60;
        public static GameObject survivorPlayer04SelectedBox = null;
        public static GameObject survivorPlayer04CurrentBox = null;
        public static PlayerControl survivorPlayer05 = null;
        public static PlayerControl survivorPlayer05currentTarget = null;
        public static bool survivorPlayer05IsReviving = false;
        public static bool survivorPlayer05IsInfected = false;
        public static bool survivorPlayer05CanKill = false;
        public static bool survivorPlayer05HasKeyItem = false;
        public static byte survivorPlayer05FoundBox = 0;
        public static float survivorPlayer05Timer = 60;
        public static GameObject survivorPlayer05SelectedBox = null;
        public static GameObject survivorPlayer05CurrentBox = null;
        public static PlayerControl survivorPlayer06 = null;
        public static PlayerControl survivorPlayer06currentTarget = null;
        public static bool survivorPlayer06IsReviving = false;
        public static bool survivorPlayer06IsInfected = false;
        public static bool survivorPlayer06CanKill = false;
        public static bool survivorPlayer06HasKeyItem = false;
        public static byte survivorPlayer06FoundBox = 0;
        public static float survivorPlayer06Timer = 60;
        public static GameObject survivorPlayer06SelectedBox = null;
        public static GameObject survivorPlayer06CurrentBox = null;
        public static PlayerControl survivorPlayer07 = null;
        public static PlayerControl survivorPlayer07currentTarget = null;
        public static bool survivorPlayer07IsReviving = false;
        public static bool survivorPlayer07IsInfected = false;
        public static bool survivorPlayer07CanKill = false;
        public static bool survivorPlayer07HasKeyItem = false;
        public static byte survivorPlayer07FoundBox = 0;
        public static float survivorPlayer07Timer = 60;
        public static GameObject survivorPlayer07SelectedBox = null;
        public static GameObject survivorPlayer07CurrentBox = null;
        public static PlayerControl survivorPlayer08 = null;
        public static PlayerControl survivorPlayer08currentTarget = null;
        public static bool survivorPlayer08IsReviving = false;
        public static bool survivorPlayer08IsInfected = false;
        public static bool survivorPlayer08CanKill = false;
        public static bool survivorPlayer08HasKeyItem = false;
        public static byte survivorPlayer08FoundBox = 0;
        public static float survivorPlayer08Timer = 60;
        public static GameObject survivorPlayer08SelectedBox = null;
        public static GameObject survivorPlayer08CurrentBox = null;
        public static PlayerControl survivorPlayer09 = null;
        public static PlayerControl survivorPlayer09currentTarget = null;
        public static bool survivorPlayer09IsReviving = false;
        public static bool survivorPlayer09IsInfected = false;
        public static bool survivorPlayer09CanKill = false;
        public static bool survivorPlayer09HasKeyItem = false;
        public static byte survivorPlayer09FoundBox = 0;
        public static float survivorPlayer09Timer = 60;
        public static GameObject survivorPlayer09SelectedBox = null;
        public static GameObject survivorPlayer09CurrentBox = null;
        public static PlayerControl survivorPlayer10 = null;
        public static PlayerControl survivorPlayer10currentTarget = null;
        public static bool survivorPlayer10IsReviving = false;
        public static bool survivorPlayer10IsInfected = false;
        public static bool survivorPlayer10CanKill = false;
        public static bool survivorPlayer10HasKeyItem = false;
        public static byte survivorPlayer10FoundBox = 0;
        public static float survivorPlayer10Timer = 60;
        public static GameObject survivorPlayer10SelectedBox = null;
        public static GameObject survivorPlayer10CurrentBox = null;
        public static PlayerControl survivorPlayer11 = null;
        public static PlayerControl survivorPlayer11currentTarget = null;
        public static bool survivorPlayer11IsReviving = false;
        public static bool survivorPlayer11IsInfected = false;
        public static bool survivorPlayer11CanKill = false;
        public static bool survivorPlayer11HasKeyItem = false;
        public static byte survivorPlayer11FoundBox = 0;
        public static float survivorPlayer11Timer = 60;
        public static GameObject survivorPlayer11SelectedBox = null;
        public static GameObject survivorPlayer11CurrentBox = null;
        public static PlayerControl survivorPlayer12 = null;
        public static PlayerControl survivorPlayer12currentTarget = null;
        public static bool survivorPlayer12IsReviving = false;
        public static bool survivorPlayer12IsInfected = false;
        public static bool survivorPlayer12CanKill = false;
        public static bool survivorPlayer12HasKeyItem = false;
        public static byte survivorPlayer12FoundBox = 0;
        public static float survivorPlayer12Timer = 60;
        public static GameObject survivorPlayer12SelectedBox = null;
        public static GameObject survivorPlayer12CurrentBox = null;
        public static PlayerControl survivorPlayer13 = null;
        public static PlayerControl survivorPlayer13currentTarget = null;
        public static bool survivorPlayer13IsReviving = false;
        public static bool survivorPlayer13IsInfected = false;
        public static bool survivorPlayer13CanKill = false;
        public static bool survivorPlayer13HasKeyItem = false;
        public static byte survivorPlayer13FoundBox = 0;
        public static float survivorPlayer13Timer = 60;
        public static GameObject survivorPlayer13SelectedBox = null;
        public static GameObject survivorPlayer13CurrentBox = null;
        public static PlayerControl zombiePlayer01 = null;
        public static PlayerControl zombiePlayer01currentTarget = null;
        public static PlayerControl zombiePlayer01infectedTarget = null;
        public static bool zombiePlayer01IsReviving = false;
        public static PlayerControl zombiePlayer02 = null;
        public static PlayerControl zombiePlayer02currentTarget = null;
        public static PlayerControl zombiePlayer02infectedTarget = null;
        public static bool zombiePlayer02IsReviving = false;
        public static PlayerControl zombiePlayer03 = null;
        public static PlayerControl zombiePlayer03currentTarget = null;
        public static PlayerControl zombiePlayer03infectedTarget = null;
        public static bool zombiePlayer03IsReviving = false;
        public static PlayerControl zombiePlayer04 = null;
        public static PlayerControl zombiePlayer04currentTarget = null;
        public static PlayerControl zombiePlayer04infectedTarget = null;
        public static bool zombiePlayer04IsReviving = false;
        public static PlayerControl zombiePlayer05 = null;
        public static PlayerControl zombiePlayer05currentTarget = null;
        public static PlayerControl zombiePlayer05infectedTarget = null;
        public static bool zombiePlayer05IsReviving = false;
        public static PlayerControl zombiePlayer06 = null;
        public static PlayerControl zombiePlayer06currentTarget = null;
        public static PlayerControl zombiePlayer06infectedTarget = null;
        public static bool zombiePlayer06IsReviving = false;
        public static PlayerControl zombiePlayer07 = null;
        public static PlayerControl zombiePlayer07currentTarget = null;
        public static PlayerControl zombiePlayer07infectedTarget = null;
        public static bool zombiePlayer07IsReviving = false;
        public static PlayerControl zombiePlayer08 = null;
        public static PlayerControl zombiePlayer08currentTarget = null;
        public static PlayerControl zombiePlayer08infectedTarget = null;
        public static bool zombiePlayer08IsReviving = false;
        public static PlayerControl zombiePlayer09 = null;
        public static PlayerControl zombiePlayer09currentTarget = null;
        public static PlayerControl zombiePlayer09infectedTarget = null;
        public static bool zombiePlayer09IsReviving = false;
        public static PlayerControl zombiePlayer10 = null;
        public static PlayerControl zombiePlayer10currentTarget = null;
        public static PlayerControl zombiePlayer10infectedTarget = null;
        public static bool zombiePlayer10IsReviving = false;
        public static PlayerControl zombiePlayer11 = null;
        public static PlayerControl zombiePlayer11currentTarget = null;
        public static PlayerControl zombiePlayer11infectedTarget = null;
        public static bool zombiePlayer11IsReviving = false;
        public static PlayerControl zombiePlayer12 = null;
        public static PlayerControl zombiePlayer12currentTarget = null;
        public static PlayerControl zombiePlayer12infectedTarget = null;
        public static bool zombiePlayer12IsReviving = false;
        public static PlayerControl zombiePlayer13 = null;
        public static PlayerControl zombiePlayer13currentTarget = null;
        public static PlayerControl zombiePlayer13infectedTarget = null;
        public static bool zombiePlayer13IsReviving = false;
        public static PlayerControl zombiePlayer14 = null;
        public static PlayerControl zombiePlayer14currentTarget = null;
        public static PlayerControl zombiePlayer14infectedTarget = null;
        public static bool zombiePlayer14IsReviving = false;

        public static bool zombieSenseiMapLaboratoryMode = false;
        public static bool zombieDleksMapLaboratoryMode = false;

        public static GameObject laboratory = null;
        public static GameObject laboratoryEnterButton = null;
        public static GameObject laboratoryExitButton = null;
        public static GameObject laboratoryCreateCureButton = null;
        public static GameObject laboratoryPutKeyItemButton = null;
        public static GameObject laboratoryKeyItem01 = null;
        public static GameObject laboratoryKeyItem02 = null;
        public static GameObject laboratoryKeyItem03 = null;
        public static GameObject laboratoryKeyItem04 = null;
        public static GameObject laboratoryKeyItem05 = null;
        public static GameObject laboratoryKeyItem06 = null;
        public static GameObject laboratoryNurseMedKit = null;
        public static GameObject laboratoryExitLeftButton = null;
        public static GameObject laboratoryExitRightButton = null;

        public static GameObject laboratorytwo = null;
        public static GameObject laboratorytwoEnterButton = null;
        public static GameObject laboratorytwoExitButton = null;
        public static GameObject laboratorytwoCreateCureButton = null;
        public static GameObject laboratorytwoPutKeyItemButton = null;
        public static GameObject laboratorytwoNurseMedKit = null;
        public static GameObject laboratorytwoExitLeftButton = null;
        public static GameObject laboratorytwoExitRightButton = null;

        public static bool keyItem01BeingHeld = false;
        public static bool keyItem02BeingHeld = false;
        public static bool keyItem03BeingHeld = false;
        public static bool keyItem04BeingHeld = false;
        public static bool keyItem05BeingHeld = false;
        public static bool keyItem06BeingHeld = false;

        public static float startZombies = 1f;
        public static float infectCooldown = 10f;
        public static float infectTime = 3f;
        public static float timeForHeal = 20f;
        public static float searchBoxTimer = 5f;

        public static bool triggerZombieWin = false;
        public static bool triggerSurvivorWin = false;

        public static string zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + zombieTeam.Count + "</color>";


        public static List<Arrow> localSurvivorsDeliverArrow = new List<Arrow>();

        private static Sprite buttonInfect;

        public static Sprite getInfectButtonSprite() {
            if (buttonInfect) return buttonInfect;
            buttonInfect = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratoryInfectButton.png", 90f);
            return buttonInfect;
        }

        private static Sprite buttonPickMedkit;

        public static Sprite getPickMedkitButtonSprite() {
            if (buttonPickMedkit) return buttonPickMedkit;
            buttonPickMedkit = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratoryPickMedkitButton.png", 90f);
            return buttonPickMedkit;
        }

        private static Sprite buttonDeliverMedkit;

        public static Sprite getDeliverMedkitButtonSprite() {
            if (buttonDeliverMedkit) return buttonDeliverMedkit;
            buttonDeliverMedkit = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratoryDeliverMedkitButton.png", 90f);
            return buttonDeliverMedkit;
        }

        private static Sprite buttonEnterLaboratory;

        public static Sprite getEnterLaboratoryButtonSprite() {
            if (buttonEnterLaboratory) return buttonEnterLaboratory;
            buttonEnterLaboratory = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratoryEnterLaboratoryButton.png", 90f);
            return buttonEnterLaboratory;
        }

        private static Sprite buttonExitLaboratory;

        public static Sprite getExitLaboratoryButtonSprite() {
            if (buttonExitLaboratory) return buttonExitLaboratory;
            buttonExitLaboratory = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratoryExitLaboratoryButton.png", 90f);
            return buttonExitLaboratory;
        }

        private static Sprite buttonCreateCure;

        public static Sprite getCreateCureButtonSprite() {
            if (buttonCreateCure) return buttonCreateCure;
            buttonCreateCure = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratoryCreateCureButton.png", 90f);
            return buttonCreateCure;
        }

        private static Sprite buttonSurvivorEnterLaboratory;

        public static Sprite getSurvivorEnterLaboratoryButtonSprite() {
            if (buttonSurvivorEnterLaboratory) return buttonSurvivorEnterLaboratory;
            buttonSurvivorEnterLaboratory = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratorySurvivorEnterLaboratoryButton.png", 90f);
            return buttonSurvivorEnterLaboratory;
        }

        private static Sprite buttonSurvivorExitLaboratory;

        public static Sprite getSurvivorExitLaboratoryButtonSprite() {
            if (buttonSurvivorExitLaboratory) return buttonSurvivorExitLaboratory;
            buttonSurvivorExitLaboratory = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratorySurvivorExitLaboratoryButton.png", 90f);
            return buttonSurvivorExitLaboratory;
        }

        private static Sprite buttonSurvivorEmptyShoot;

        public static Sprite getSurvivorEmptyShootButtonSprite() {
            if (buttonSurvivorEmptyShoot) return buttonSurvivorEmptyShoot;
            buttonSurvivorEmptyShoot = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratorySurvivorEmptyShootButton.png", 90f);
            return buttonSurvivorEmptyShoot;
        }

        private static Sprite buttonSurvivorFullShoot;

        public static Sprite getSurvivorFullShootButtonSprite() {
            if (buttonSurvivorFullShoot) return buttonSurvivorFullShoot;
            buttonSurvivorFullShoot = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratorySurvivorFullShootButton.png", 90f);
            return buttonSurvivorFullShoot;
        }

        private static Sprite buttonSurvivorTakeBox;

        public static Sprite getSurvivorTakeBoxButtonSprite() {
            if (buttonSurvivorTakeBox) return buttonSurvivorTakeBox;
            buttonSurvivorTakeBox = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratorySurvivorTakeBoxButton.png", 90f);
            return buttonSurvivorTakeBox;
        }

        private static Sprite buttonSurvivorDeliverBox;

        public static Sprite getSurvivorDeliverBoxButtonSprite() {
            if (buttonSurvivorDeliverBox) return buttonSurvivorDeliverBox;
            buttonSurvivorDeliverBox = Helpers.loadSpriteFromResources("LasMonjas.Images.ZombieLaboratorySurvivorDeliverBoxButton.png", 90f);
            return buttonSurvivorDeliverBox;
        }

        public static void clearAndReload() {
            survivorTeam.Clear();
            zombieTeam.Clear();
            groundItems.Clear();
            infectedTeam.Clear();
            susBoxPositions.Clear();
            nurseExits.Clear();
            nurseMedkits.Clear();
            localNurseArrows = new List<Arrow>();
            localSurvivorsDeliverArrow = new List<Arrow>();
            laboratoryEntrances.Clear();
            timeForHeal = CustomOptionHolder.zombieLaboratoryMaxTimeForHeal.getFloat();

            nursePlayer = null;
            nursePlayercurrentTarget = null;
            nursePlayerHasMedKit = false;
            nursePlayerInsideLaboratory = true;
            nursePlayerHasCureReady = false;
            nursePlayerCurrentExit = 0;
            laboratoryNurseMedKit = null;
            survivorPlayer01 = null;
            survivorPlayer01currentTarget = null;
            survivorPlayer01IsReviving = false;
            survivorPlayer01IsInfected = false;
            survivorPlayer01CanKill = false;
            survivorPlayer01HasKeyItem = false;
            survivorPlayer01FoundBox = 0;
            survivorPlayer01SelectedBox = null;
            survivorPlayer01CurrentBox = null;
            survivorPlayer01Timer = timeForHeal;
            survivorPlayer02 = null;
            survivorPlayer02currentTarget = null;
            survivorPlayer02IsReviving = false;
            survivorPlayer02IsInfected = false;
            survivorPlayer02CanKill = false;
            survivorPlayer02HasKeyItem = false;
            survivorPlayer02FoundBox = 0;
            survivorPlayer02SelectedBox = null;
            survivorPlayer02CurrentBox = null;
            survivorPlayer02Timer = timeForHeal;
            survivorPlayer03 = null;
            survivorPlayer03currentTarget = null;
            survivorPlayer03IsReviving = false;
            survivorPlayer03IsInfected = false;
            survivorPlayer03CanKill = false;
            survivorPlayer03HasKeyItem = false;
            survivorPlayer03FoundBox = 0;
            survivorPlayer03SelectedBox = null;
            survivorPlayer03CurrentBox = null;
            survivorPlayer03Timer = timeForHeal;
            survivorPlayer04 = null;
            survivorPlayer04currentTarget = null;
            survivorPlayer04IsReviving = false;
            survivorPlayer04IsInfected = false;
            survivorPlayer04CanKill = false;
            survivorPlayer04HasKeyItem = false;
            survivorPlayer04FoundBox = 0;
            survivorPlayer04SelectedBox = null;
            survivorPlayer04CurrentBox = null;
            survivorPlayer04Timer = timeForHeal;
            survivorPlayer05 = null;
            survivorPlayer05currentTarget = null;
            survivorPlayer05IsInfected = false;
            survivorPlayer05IsReviving = false;
            survivorPlayer05CanKill = false;
            survivorPlayer05HasKeyItem = false;
            survivorPlayer05FoundBox = 0;
            survivorPlayer05SelectedBox = null;
            survivorPlayer05CurrentBox = null;
            survivorPlayer05Timer = timeForHeal;
            survivorPlayer06 = null;
            survivorPlayer06currentTarget = null;
            survivorPlayer06IsReviving = false;
            survivorPlayer06IsInfected = false;
            survivorPlayer06CanKill = false;
            survivorPlayer06HasKeyItem = false;
            survivorPlayer06FoundBox = 0;
            survivorPlayer06SelectedBox = null;
            survivorPlayer06CurrentBox = null;
            survivorPlayer06Timer = timeForHeal;
            survivorPlayer07 = null;
            survivorPlayer07currentTarget = null;
            survivorPlayer07IsReviving = false;
            survivorPlayer07IsInfected = false;
            survivorPlayer07CanKill = false;
            survivorPlayer07HasKeyItem = false;
            survivorPlayer07FoundBox = 0;
            survivorPlayer07SelectedBox = null;
            survivorPlayer07CurrentBox = null;
            survivorPlayer07Timer = timeForHeal;
            survivorPlayer08 = null;
            survivorPlayer08currentTarget = null;
            survivorPlayer08IsInfected = false;
            survivorPlayer08IsReviving = false;
            survivorPlayer08CanKill = false;
            survivorPlayer08HasKeyItem = false;
            survivorPlayer08FoundBox = 0;
            survivorPlayer08SelectedBox = null;
            survivorPlayer08CurrentBox = null;
            survivorPlayer08Timer = timeForHeal;
            survivorPlayer09 = null;
            survivorPlayer09currentTarget = null;
            survivorPlayer09IsReviving = false;
            survivorPlayer09IsInfected = false;
            survivorPlayer09CanKill = false;
            survivorPlayer09HasKeyItem = false;
            survivorPlayer09FoundBox = 0;
            survivorPlayer09SelectedBox = null;
            survivorPlayer09CurrentBox = null;
            survivorPlayer09Timer = timeForHeal;
            survivorPlayer10 = null;
            survivorPlayer10currentTarget = null;
            survivorPlayer10IsReviving = false;
            survivorPlayer10IsInfected = false;
            survivorPlayer10CanKill = false;
            survivorPlayer10HasKeyItem = false;
            survivorPlayer10FoundBox = 0;
            survivorPlayer10SelectedBox = null;
            survivorPlayer10CurrentBox = null;
            survivorPlayer10Timer = timeForHeal;
            survivorPlayer11 = null;
            survivorPlayer11currentTarget = null;
            survivorPlayer11IsReviving = false;
            survivorPlayer11IsInfected = false;
            survivorPlayer11CanKill = false;
            survivorPlayer11HasKeyItem = false;
            survivorPlayer11FoundBox = 0;
            survivorPlayer11SelectedBox = null;
            survivorPlayer11CurrentBox = null;
            survivorPlayer11Timer = timeForHeal;
            survivorPlayer12 = null;
            survivorPlayer12currentTarget = null;
            survivorPlayer12IsReviving = false;
            survivorPlayer12IsInfected = false;
            survivorPlayer12CanKill = false;
            survivorPlayer12HasKeyItem = false;
            survivorPlayer12FoundBox = 0;
            survivorPlayer12SelectedBox = null;
            survivorPlayer12CurrentBox = null;
            survivorPlayer12Timer = timeForHeal;
            survivorPlayer13 = null;
            survivorPlayer13currentTarget = null;
            survivorPlayer13IsReviving = false;
            survivorPlayer13IsInfected = false;
            survivorPlayer13CanKill = false;
            survivorPlayer13HasKeyItem = false;
            survivorPlayer13FoundBox = 0;
            survivorPlayer13SelectedBox = null;
            survivorPlayer13CurrentBox = null;
            survivorPlayer13Timer = timeForHeal;
            zombiePlayer01 = null;
            zombiePlayer01currentTarget = null;
            zombiePlayer01infectedTarget = null;
            zombiePlayer01IsReviving = false;
            zombiePlayer02 = null;
            zombiePlayer02currentTarget = null;
            zombiePlayer02infectedTarget = null;
            zombiePlayer02IsReviving = false;
            zombiePlayer03 = null;
            zombiePlayer03currentTarget = null;
            zombiePlayer03infectedTarget = null;
            zombiePlayer03IsReviving = false;
            zombiePlayer04 = null;
            zombiePlayer04currentTarget = null;
            zombiePlayer04infectedTarget = null;
            zombiePlayer04IsReviving = false;
            zombiePlayer05 = null;
            zombiePlayer05currentTarget = null;
            zombiePlayer06infectedTarget = null;
            zombiePlayer05IsReviving = false;
            zombiePlayer06 = null;
            zombiePlayer06currentTarget = null;
            zombiePlayer06infectedTarget = null;
            zombiePlayer06IsReviving = false;
            zombiePlayer07 = null;
            zombiePlayer07currentTarget = null;
            zombiePlayer07infectedTarget = null;
            zombiePlayer07IsReviving = false;
            zombiePlayer08 = null;
            zombiePlayer08currentTarget = null;
            zombiePlayer08infectedTarget = null;
            zombiePlayer08IsReviving = false;
            zombiePlayer09 = null;
            zombiePlayer09currentTarget = null;
            zombiePlayer09infectedTarget = null;
            zombiePlayer09IsReviving = false;
            zombiePlayer10 = null;
            zombiePlayer10currentTarget = null;
            zombiePlayer10infectedTarget = null;
            zombiePlayer10IsReviving = false;
            zombiePlayer11 = null;
            zombiePlayer11currentTarget = null;
            zombiePlayer11infectedTarget = null;
            zombiePlayer11IsReviving = false;
            zombiePlayer12 = null;
            zombiePlayer12currentTarget = null;
            zombiePlayer12infectedTarget = null;
            zombiePlayer12IsReviving = false;
            zombiePlayer13 = null;
            zombiePlayer13currentTarget = null;
            zombiePlayer13infectedTarget = null;
            zombiePlayer13IsReviving = false;
            zombiePlayer14 = null;
            zombiePlayer14currentTarget = null;
            zombiePlayer14infectedTarget = null;
            zombiePlayer14IsReviving = false;

            laboratory = null;
            laboratoryEnterButton = null;
            laboratoryExitButton = null;
            laboratoryCreateCureButton = null;
            laboratoryPutKeyItemButton = null;
            laboratoryExitLeftButton = null;
            laboratoryExitRightButton = null;
            laboratoryKeyItem01 = null;
            laboratoryKeyItem02 = null;
            laboratoryKeyItem03 = null;
            laboratoryKeyItem04 = null;
            laboratoryKeyItem05 = null;
            laboratoryKeyItem06 = null;
            keyItem01BeingHeld = false;
            keyItem02BeingHeld = false;
            keyItem03BeingHeld = false;
            keyItem04BeingHeld = false;
            keyItem05BeingHeld = false;
            keyItem06BeingHeld = false;
            startZombies = CustomOptionHolder.zombieLaboratoryStartZombies.getFloat();
            infectCooldown = CustomOptionHolder.zombieLaboratoryInfectCooldown.getFloat();
            infectTime = CustomOptionHolder.zombieLaboratoryInfectTime.getFloat();
            searchBoxTimer = CustomOptionHolder.zombieLaboratorySearchBoxTimer.getFloat();
            zombieSenseiMapLaboratoryMode = CustomOptionHolder.activateSenseiMap.getBool();
            zombieDleksMapLaboratoryMode = CustomOptionHolder.activateDleksMap.getBool();
            currentKeyItems = 0;
            triggerZombieWin = false;
            triggerSurvivorWin = false;

            laboratorytwo = null;
            laboratorytwoEnterButton = null;
            laboratorytwoExitButton = null;
            laboratorytwoCreateCureButton = null;
            laboratorytwoPutKeyItemButton = null;
            laboratorytwoNurseMedKit = null;
            laboratorytwoExitLeftButton = null;
            laboratorytwoExitRightButton = null;

            zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + zombieTeam.Count + "</color>";

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    if (zombieSenseiMapLaboratoryMode) {
                        // sus[0-5] = key items
                        susBoxPositions.Add(new Vector3(-7.5f, -4, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.25f, -0.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-4.5f, 1.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.25f, 3, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.85f, 5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16.25f, -4.75f, 0.4f));
                        // sus[6-9] = ammo boxes
                        susBoxPositions.Add(new Vector3(-20, -1.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16f, -1.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18f, -1.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18f, 2f, 0.4f));
                        // sus[10-59] = nothing boxes
                        susBoxPositions.Add(new Vector3(-15.5f, 2.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-17.85f, 5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18.85f, -8f, 0.4f));
                        susBoxPositions.Add(new Vector3(-14.25f, -12.35f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16.35f, -10.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-13f, -7.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-10.75f, -9f, 0.4f));
                        susBoxPositions.Add(new Vector3(-10.75f, -11.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.5f, -12.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-7.75f, -11.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.75f, -9.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-1f, -9f, 0.4f));
                        susBoxPositions.Add(new Vector3(1f, -12f, 0.4f));
                        susBoxPositions.Add(new Vector3(3.75f, -14f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.5f, -14f, 0.4f));
                        susBoxPositions.Add(new Vector3(-8.5f, -0.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(12.5f, -0.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(12f, -2.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(10f, -2.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(5.75f, 2f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.75f, 5f, 0.4f));
                        susBoxPositions.Add(new Vector3(7.5f, 1.9f, 0.4f));
                        susBoxPositions.Add(new Vector3(5f, -9f, 0.4f));
                        susBoxPositions.Add(new Vector3(5f, -5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.35f, 8f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.75f, 6f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.75f, 3f, 0.4f));
                        susBoxPositions.Add(new Vector3(2.5f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(7.5f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(7.5f, -3f, 0.4f));
                        susBoxPositions.Add(new Vector3(7.5f, -1f, 0.4f));
                        susBoxPositions.Add(new Vector3(4.6f, -1f, 0.4f));
                        susBoxPositions.Add(new Vector3(3f, 1.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(4f, 3.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(1f, 3f, 0.4f));
                        susBoxPositions.Add(new Vector3(1f, -1.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-2.25f, -1.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.75f, -1.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.75f, -4f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.5f, -6f, 0.4f));
                        susBoxPositions.Add(new Vector3(-13.5f, -1.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.85f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-12f, -4.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-12f, 1.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-10.35f, 2.85f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.75f, 3.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.75f, 6f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.75f, 8f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.75f, 10.85f, 0.4f));
                        susBoxPositions.Add(new Vector3(-15.5f, -6.75f, 0.4f));
                    }
                    else if (zombieDleksMapLaboratoryMode) {
                        // sus[0-5] = key items
                        susBoxPositions.Add(new Vector3(15.75f, -0.9f, 0.4f));
                        susBoxPositions.Add(new Vector3(20, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(7.85f, -11.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-4.5f, -9.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.75f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-1.75f, -3.5f, 0.4f));
                        // sus[6-9] = ammo boxes
                        susBoxPositions.Add(new Vector3(18.5f, -9.65f, 0.4f));
                        susBoxPositions.Add(new Vector3(3.5f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16.75f, -6f, 0.4f));
                        susBoxPositions.Add(new Vector3(3f, -10f, 0.4f));
                        // sus[10-59] = nothing boxes
                        susBoxPositions.Add(new Vector3(21.5f, -2.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(20f, -4f, 0.4f));
                        susBoxPositions.Add(new Vector3(21f, -5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(21.5f, -8f, 0.4f));
                        susBoxPositions.Add(new Vector3(16.9f, -5.4f, 0.4f));
                        susBoxPositions.Add(new Vector3(13.9f, -5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(13.5f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(12.5f, -3.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(18f, 2.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(13.65f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(7.1f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.1f, -1.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.4f, -4.65f, 0.4f));
                        susBoxPositions.Add(new Vector3(5.75f, -5.3f, 0.4f));
                        susBoxPositions.Add(new Vector3(18.5f, -13.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(13.85f, -11.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(12.1f, -14.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.5f, -11f, 0.4f));
                        susBoxPositions.Add(new Vector3(9f, -8.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.5f, -8.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.5f, -14.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.35f, -9.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(0.65f, -15.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(3.55f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-1.05f, -7f, 0.4f));
                        susBoxPositions.Add(new Vector3(-2.5f, -8.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.3f, -7.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-2.5f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.15f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-2.15f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-4f, -16f, 0.4f));
                        susBoxPositions.Add(new Vector3(-8f, -14.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.4f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.4f, -8.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-11.75f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-14f, -4.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16.75f, -3f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.5f, -3.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.65f, -4.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.2f, -4.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.5f, 0f, 0.4f));
                        susBoxPositions.Add(new Vector3(-10.5f, 2.35f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-1.75f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-1.75f, 5.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(3.5f, 5.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(3.5f, -3.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(0.8f, -1.45f, 0.4f));
                        susBoxPositions.Add(new Vector3(0.8f, 3.85f, 0.4f));
                        susBoxPositions.Add(new Vector3(0.65f, -5.65f, 0.4f));
                    }
                    else {
                        // sus[0-5] = key items
                        susBoxPositions.Add(new Vector3(-15.75f, -0.9f, 0.4f));
                        susBoxPositions.Add(new Vector3(-20, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-7.85f, -11.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(4.5f, -9.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(5.75f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(1.75f, -3.5f, 0.4f));
                        // sus[6-9] = ammo boxes
                        susBoxPositions.Add(new Vector3(-18.5f, -9.65f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.5f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(16.75f, -6f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3f, -10f, 0.4f));
                        // sus[10-59] = nothing boxes
                        susBoxPositions.Add(new Vector3(-21.5f, -2.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-20f, -4f, 0.4f));
                        susBoxPositions.Add(new Vector3(-21f, -5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-21.5f, -8f, 0.4f));
                        susBoxPositions.Add(new Vector3(-16.9f, -5.4f, 0.4f));
                        susBoxPositions.Add(new Vector3(-13.9f, -5.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-13.5f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-12.5f, -3.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18f, 2.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-13.65f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-7.1f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.1f, -1.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.4f, -4.65f, 0.4f));
                        susBoxPositions.Add(new Vector3(-5.75f, -5.3f, 0.4f));
                        susBoxPositions.Add(new Vector3(-18.5f, -13.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(-13.85f, -11.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-12.1f, -14.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9.5f, -11f, 0.4f));
                        susBoxPositions.Add(new Vector3(-9f, -8.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.5f, -8.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(-6.5f, -14.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(0.35f, -9.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.65f, -15.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.55f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(1.05f, -7f, 0.4f));
                        susBoxPositions.Add(new Vector3(2.5f, -8.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.3f, -7.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(2.5f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(5.15f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(2.15f, -15f, 0.4f));
                        susBoxPositions.Add(new Vector3(4f, -16f, 0.4f));
                        susBoxPositions.Add(new Vector3(8f, -14.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.4f, -12.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.4f, -8.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(11.75f, -6.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(14f, -4.75f, 0.4f));
                        susBoxPositions.Add(new Vector3(16.75f, -3f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.5f, -3.25f, 0.4f));
                        susBoxPositions.Add(new Vector3(6.65f, -4.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(5.2f, -4.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(9.5f, 0f, 0.4f));
                        susBoxPositions.Add(new Vector3(10.5f, 2.35f, 0.4f));
                        susBoxPositions.Add(new Vector3(6f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(1.75f, 1.15f, 0.4f));
                        susBoxPositions.Add(new Vector3(1.75f, 5.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.5f, 5.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-3.5f, -3.5f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.8f, -1.45f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.8f, 3.85f, 0.4f));
                        susBoxPositions.Add(new Vector3(-0.65f, -5.65f, 0.4f));
                    }
                    break;
                case 1:
                    // sus[0-5] = key items
                    susBoxPositions.Add(new Vector3(-3.5f, 3.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.35f, 2.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(-3.55f, 1.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.4f, -1.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(-3.25f, -1.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(-0.55f, -1.35f, 0.4f));
                    // sus[6-9] = ammo boxes
                    susBoxPositions.Add(new Vector3(4.25f, -1.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.75f, -1.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.5f, -1.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.5f, -1.35f, 0.4f));
                    // sus[10-59] = nothing boxes
                    susBoxPositions.Add(new Vector3(14.5f, 0.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(15.4f, -0.85f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.5f, 2.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.5f, 7f, 0.4f));
                    susBoxPositions.Add(new Vector3(14.5f, 4f, 0.4f));
                    susBoxPositions.Add(new Vector3(16f, 4f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.25f, 4f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.25f, 1.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(6f, 1.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(6f, 4.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(6f, 8f, 0.4f));
                    susBoxPositions.Add(new Vector3(6f, 12f, 0.4f));
                    susBoxPositions.Add(new Vector3(8.6f, 13f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.8f, 10.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.7f, 12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.5f, 10.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.75f, 13.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.25f, 13.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.25f, 10.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(15.4f, 9.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.85f, 11.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.85f, 14f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.85f, 17.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.85f, 20.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.85f, 23.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(15.75f, 17.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(14.75f, 20.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.65f, 20.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, 20.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(21f, 17.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.5f, 23.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(22.15f, 23.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.15f, 24.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.25f, 24.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.25f, 9.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.5f, 4.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(18.5f, 3.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.5f, 2.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.5f, 0.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, 4.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, 0.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, -1.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.5f, 4.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.5f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.5f, 0.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.5f, -1.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(28.25f, 4.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(28.25f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(28.25f, 0.25f, 0.4f));
                    break;
                case 2:
                    // sus[0-5] = key items
                    susBoxPositions.Add(new Vector3(13.75f, -9.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.15f, -9.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.5f, -9.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(2.75f, -9.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(2.75f, -11.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(5.5f, -11.75f, 0.4f));
                    // sus[6-9] = ammo boxes
                    susBoxPositions.Add(new Vector3(7.5f, -12.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.75f, -12.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(16f, -12.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(24f, -12.15f, 0.4f));
                    // sus[10-59] = nothing boxes
                    susBoxPositions.Add(new Vector3(29f, -12.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(5.45f, -16.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.25f, -16.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.45f, -14.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(4.4f, -19f, 0.4f));
                    susBoxPositions.Add(new Vector3(8.5f, -19f, 0.4f));
                    susBoxPositions.Add(new Vector3(14.5f, -19f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.85f, -19f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.35f, -17.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.15f, -17.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.35f, -20f, 0.4f));
                    susBoxPositions.Add(new Vector3(2.25f, -23.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.15f, -21.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.25f, -21.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.25f, -24.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(8f, -24.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(11, -20.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.5f, -25f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, -25f, 0.4f));
                    susBoxPositions.Add(new Vector3(24f, -25f, 0.4f));
                    susBoxPositions.Add(new Vector3(18.5f, -21.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.75f, -21.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(22.5f, -21.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(22.25f, -18.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.25f, -18.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.65f, -14.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(26.5f, -14.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(24.5f, -17f, 0.4f));
                    susBoxPositions.Add(new Vector3(27.5f, -17f, 0.4f));
                    susBoxPositions.Add(new Vector3(27.85f, -22.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(31f, -20.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(38.35f, -20.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(36.5f, -21.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(35.5f, -19.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(32f, -15.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, -9f, 0.4f));
                    susBoxPositions.Add(new Vector3(24f, -7f, 0.4f));
                    susBoxPositions.Add(new Vector3(27.75f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(30f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(33.25f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(36.4f, -6.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(36.4f, -10f, 0.4f));
                    susBoxPositions.Add(new Vector3(39.25f, -10f, 0.4f));
                    susBoxPositions.Add(new Vector3(40.35f, -8f, 0.4f));
                    susBoxPositions.Add(new Vector3(39.1f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(31.5f, -9.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(28.75f, -9.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.5f, -9.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.5f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(15.75f, -21.75f, 0.4f));
                    break;
                case 3:
                    // sus[0-5] = key items
                    susBoxPositions.Add(new Vector3(15.75f, -0.9f, 0.4f));
                    susBoxPositions.Add(new Vector3(20, -6.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.85f, -11.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-4.5f, -9.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.75f, -15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-1.75f, -3.5f, 0.4f));
                    // sus[6-9] = ammo boxes
                    susBoxPositions.Add(new Vector3(18.5f, -9.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.5f, 1.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-16.75f, -6f, 0.4f));
                    susBoxPositions.Add(new Vector3(3f, -10f, 0.4f));
                    // sus[10-59] = nothing boxes
                    susBoxPositions.Add(new Vector3(21.5f, -2.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, -4f, 0.4f));
                    susBoxPositions.Add(new Vector3(21f, -5.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(21.5f, -8f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.9f, -5.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.9f, -5.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.5f, -6.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.5f, -3.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(18f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.65f, 1.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.1f, 1.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.1f, -1.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.4f, -4.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(5.75f, -5.3f, 0.4f));
                    susBoxPositions.Add(new Vector3(18.5f, -13.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.85f, -11.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.1f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.5f, -11f, 0.4f));
                    susBoxPositions.Add(new Vector3(9f, -8.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.5f, -8.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.5f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-0.35f, -9.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(0.65f, -15.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.55f, -15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-1.05f, -7f, 0.4f));
                    susBoxPositions.Add(new Vector3(-2.5f, -8.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-6.3f, -7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-2.5f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.15f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-2.15f, -15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-4f, -16f, 0.4f));
                    susBoxPositions.Add(new Vector3(-8f, -14.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-9.4f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-9.4f, -8.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-11.75f, -6.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14f, -4.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-16.75f, -3f, 0.4f));
                    susBoxPositions.Add(new Vector3(-9.5f, -3.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-6.65f, -4.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.2f, -4.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-9.5f, 0f, 0.4f));
                    susBoxPositions.Add(new Vector3(-10.5f, 2.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(-6f, 1.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-1.75f, 1.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-1.75f, 5.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.5f, 5.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.5f, -3.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(0.8f, -1.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(0.8f, 3.85f, 0.4f));
                    susBoxPositions.Add(new Vector3(0.65f, -5.65f, 0.4f));
                    break;
                case 4:
                    // sus[0-5] = key items
                    susBoxPositions.Add(new Vector3(3.5f, -0.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.5f, -2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.25f, -2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.25f, 2f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.25f, 2f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.25f, 2f, 0.4f));
                    // sus[6-9] = ammo boxes
                    susBoxPositions.Add(new Vector3(15.25f, 2f, 0.4f));
                    susBoxPositions.Add(new Vector3(24f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(25.75f, 0.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.15f, 8.5f, 0.4f));
                    // sus[10-59] = nothing boxes
                    susBoxPositions.Add(new Vector3(18.5f, -0.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(18.5f, 3f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, 5f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.5f, 6f, 0.4f));
                    susBoxPositions.Add(new Vector3(-23.25f, -0.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-19f, -2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-10.5f, -5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-7.5f, -7.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-0.75f, 5f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.25f, -1f, 0.4f));
                    susBoxPositions.Add(new Vector3(-7.5f, -1f, 0.4f));
                    susBoxPositions.Add(new Vector3(-15f, -1f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14.25f, 1.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14.25f, -4.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14.25f, -8.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-13.75f, -14.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-15.5f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-10.25f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.5f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(5.75f, -10.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(5.75f, -12.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(8.25f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.25f, -6.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.25f, -11f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.25f, -4f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.25f, -6.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.25f, -11f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.25f, -6.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.25f, -6.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.25f, -8.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.25f, -8.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.25f, -8.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(22.5f, -8.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(23f, -6f, 0.4f));
                    susBoxPositions.Add(new Vector3(29f, -6f, 0.4f));
                    susBoxPositions.Add(new Vector3(29f, -1.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(32f, 1.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(37.25f, -3.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(38.25f, 0.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(30.75f, 5.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(33.75f, 7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(29.25f, 7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(24.75f, 7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(27f, 9.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, 11.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(12f, 8.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(4.25f, 8.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(0.5f, 8.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-8.75f, 5.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-8.75f, 12.5f, 0.4f));
                    break;
                case 5:
                    // sus[0-5] = key items
                    susBoxPositions.Add(new Vector3(-16.5f, 7.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-20.4f, -0.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-16f, -0.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14f, -7.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(-21.5f, -7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-8.35f, -14.5f, 0.4f));
                    // sus[6-9] = ammo boxes
                    susBoxPositions.Add(new Vector3(-7.3f, -11f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.25f, -8.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(-3.33f, -10.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(6.45f, -14.5f, 0.4f));
                    // sus[10-59] = nothing boxes
                    susBoxPositions.Add(new Vector3(10.5f, -12f, 0.4f));
                    susBoxPositions.Add(new Vector3(7.65f, -10f, 0.4f));
                    susBoxPositions.Add(new Vector3(-1f, -6f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.5f, -1.65f, 0.4f));
                    susBoxPositions.Add(new Vector3(-0.45f, 1.4f, 0.4f));
                    susBoxPositions.Add(new Vector3(1.25f, 4.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(23.85f, 13.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, 11f, 0.4f));
                    susBoxPositions.Add(new Vector3(18.5f, 7.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(22f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.25f, 10f, 0.4f));
                    susBoxPositions.Add(new Vector3(7f, 4.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(8.15f, 1.2f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.25f, 1.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(19f, 0.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(20.75f, -6.85f, 0.4f));
                    susBoxPositions.Add(new Vector3(19.25f, -12f, 0.4f));
                    susBoxPositions.Add(new Vector3(11f, -7f, 0.4f));
                    susBoxPositions.Add(new Vector3(-8.85f, 5.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(-11.35f, -4.45f, 0.4f));
                    susBoxPositions.Add(new Vector3(-0.75f, -10.25f, 0.4f));                    
                    
                    susBoxPositions.Add(new Vector3(-18.3f, 5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-16.85f, 2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-21.5f, -2.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(-14.7f, -1.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-11.5f, 6.35f, 0.4f));
                    susBoxPositions.Add(new Vector3(-3f, 7f, 0.4f));
                    susBoxPositions.Add(new Vector3(-11.4f, 2f, 0.4f));
                    susBoxPositions.Add(new Vector3(-17.5f, -7.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(-17.25f, -9.3f, 0.4f));
                    susBoxPositions.Add(new Vector3(-13.75f, -9.3f, 0.4f));
                    susBoxPositions.Add(new Vector3(-11.15f, -8.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-4.5f, -14.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-7.4f, -7.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-3.65f, -8.8f, 0.4f));
                    susBoxPositions.Add(new Vector3(-5.15f, -10.15f, 0.4f));
                    susBoxPositions.Add(new Vector3(3.35f, -12.25f, 0.4f));
                    susBoxPositions.Add(new Vector3(11f, -15f, 0.4f));
                    susBoxPositions.Add(new Vector3(17.85f, -12.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(13.5f, -10.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(10.75f, -10, 0.4f));
                    susBoxPositions.Add(new Vector3(8f, -12f, 0.4f));
                    susBoxPositions.Add(new Vector3(16.25f, -6.5f, 0.4f));
                    susBoxPositions.Add(new Vector3(18f, 3.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(13f, 4f, 0.4f));
                    susBoxPositions.Add(new Vector3(9.15f, 4f, 0.4f));
                    susBoxPositions.Add(new Vector3(10f, 1f, 0.4f));
                    susBoxPositions.Add(new Vector3(12.35f, 7.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(20f, 13.75f, 0.4f));
                    susBoxPositions.Add(new Vector3(-0.25f, 6.35f, 0.4f));
                    break;
                case 6:
                    // sus[0-5] = key items
                    susBoxPositions.Add(new Vector3(0, -33.75f, -1f));
                    susBoxPositions.Add(new Vector3(2.5f, -33f, -1f));
                    susBoxPositions.Add(new Vector3(2.5f, -36f, -1f));
                    susBoxPositions.Add(new Vector3(5.25f, -33.5f, -1f));
                    susBoxPositions.Add(new Vector3(7.8f, -39.5f, -1f));
                    susBoxPositions.Add(new Vector3(5.3f, -39.5f, -1f));
                    // sus[6-9] = ammo boxes
                    susBoxPositions.Add(new Vector3(2.3f, -39.5f, -1f));
                    susBoxPositions.Add(new Vector3(-0.95f, -39.5f, -1f));
                    susBoxPositions.Add(new Vector3(-10.95f, -39f, -1f));
                    susBoxPositions.Add(new Vector3(-9.25f, -41f, -1f));
                    // sus[10-59] = nothing boxes
                    susBoxPositions.Add(new Vector3(-6.75f, -42.75f, -1f));
                    susBoxPositions.Add(new Vector3(-6f, -39.75f, -1f));
                    susBoxPositions.Add(new Vector3(-8.75f, -38.15f, -1f));
                    susBoxPositions.Add(new Vector3(-8.2f, -33.5f, -1f));
                    susBoxPositions.Add(new Vector3(-13.75f, -34.25f, -1f));
                    susBoxPositions.Add(new Vector3(-11.5f, -31.15f, -1f));
                    susBoxPositions.Add(new Vector3(-13.75f, -27.75f, -1f));
                    susBoxPositions.Add(new Vector3(-4.15f, -34f, -1f));
                    susBoxPositions.Add(new Vector3(-4.65f, -32f, -1f));
                    susBoxPositions.Add(new Vector3(-8.15f, -29f, -1f));
                    susBoxPositions.Add(new Vector3(-1.85f, -29f, -1f));
                    susBoxPositions.Add(new Vector3(13f, -25.25f, -1f));
                    susBoxPositions.Add(new Vector3(12.75f, -31.5f, -1f));
                    susBoxPositions.Add(new Vector3(10f, -31.5f, -1f));
                    susBoxPositions.Add(new Vector3(10f, -27.75f, -1f));
                    susBoxPositions.Add(new Vector3(1.25f, -30f, -1f));
                    susBoxPositions.Add(new Vector3(4.75f, -26.75f, -1f));
                    susBoxPositions.Add(new Vector3(4f, -20.25f, -1f));
                    susBoxPositions.Add(new Vector3(7.7f, -23.25f, -1f));
                    susBoxPositions.Add(new Vector3(9.5f, -20.5f, -1f));
                    susBoxPositions.Add(new Vector3(12f, 23.75f, -1f));
                    susBoxPositions.Add(new Vector3(14.25f, 24.65f, -1f));
                    susBoxPositions.Add(new Vector3(6f, 28.25f, -1f));
                    susBoxPositions.Add(new Vector3(5.75f, 31.5f, -1f));
                    susBoxPositions.Add(new Vector3(0f, 34f, -1f));
                    susBoxPositions.Add(new Vector3(2.75f, 30f, -1f));
                    susBoxPositions.Add(new Vector3(0.5f, 26.8f, -1f));
                    susBoxPositions.Add(new Vector3(4.5f, 8.4f, -1f));
                    susBoxPositions.Add(new Vector3(7.75f, 8.75f, -1f));
                    susBoxPositions.Add(new Vector3(10.45f, 17.25f, -1f));
                    susBoxPositions.Add(new Vector3(5.75f, 20.25f, -1f));
                    susBoxPositions.Add(new Vector3(-5.15f, 19.25f, -1f));
                    susBoxPositions.Add(new Vector3(1f, 19.25f, -1f));
                    susBoxPositions.Add(new Vector3(1f, 10f, -1f));
                    susBoxPositions.Add(new Vector3(1.35f, 14.5f, -1f));
                    susBoxPositions.Add(new Vector3(-3f, 14.5f, -1f));
                    susBoxPositions.Add(new Vector3(-6.4f, 14.15f, -1f));
                    susBoxPositions.Add(new Vector3(-6.7f, 10.25f, -1f));
                    susBoxPositions.Add(new Vector3(-10.25f, 10.25f, -1f));
                    susBoxPositions.Add(new Vector3(-9.5f, 12f, -1f));
                    susBoxPositions.Add(new Vector3(-15f, 18f, -1f));
                    susBoxPositions.Add(new Vector3(-12.45f, 15.75f, -1f));
                    susBoxPositions.Add(new Vector3(-12.15f, 20.25f, -1f));
                    susBoxPositions.Add(new Vector3(-9.45f, 19.15f, -1f));
                    susBoxPositions.Add(new Vector3(-10.25f, 25.5f, -1f));
                    susBoxPositions.Add(new Vector3(-4.75f, 25.5f, -1f));
                    susBoxPositions.Add(new Vector3(-4.75f, 28.45f, -1f));
                    susBoxPositions.Add(new Vector3(-8.45f, 28.45f, -1f));
                    susBoxPositions.Add(new Vector3(-12.25f, 28.45f, -1f));
                    susBoxPositions.Add(new Vector3(-12.25f, 31f, -1f));
                    break;
            }
            susBoxPositions.Shuffle();
        }
    }

    public static class BattleRoyale
    {
        public static List<PlayerControl> soloPlayerTeam = new List<PlayerControl>();
        public static PlayerControl soloPlayer01 = null;
        public static float soloPlayer01mouseAngle = 0f;
        public static GameObject soloPlayer01Wep = null;
        public static float soloPlayer01Lifes = 3;
        public static PlayerControl soloPlayer02 = null;
        public static float soloPlayer02mouseAngle = 0f;
        public static GameObject soloPlayer02Wep = null;
        public static float soloPlayer02Lifes = 3;
        public static PlayerControl soloPlayer03 = null;
        public static float soloPlayer03mouseAngle = 0f;
        public static GameObject soloPlayer03Wep = null;
        public static float soloPlayer03Lifes = 3;
        public static PlayerControl soloPlayer04 = null;
        public static float soloPlayer04mouseAngle = 0f;
        public static GameObject soloPlayer04Wep = null;
        public static float soloPlayer04Lifes = 3;
        public static PlayerControl soloPlayer05 = null;
        public static float soloPlayer05mouseAngle = 0f;
        public static GameObject soloPlayer05Wep = null;
        public static float soloPlayer05Lifes = 3;
        public static PlayerControl soloPlayer06 = null;
        public static float soloPlayer06mouseAngle = 0f;
        public static GameObject soloPlayer06Wep = null;
        public static float soloPlayer06Lifes = 3;
        public static PlayerControl soloPlayer07 = null;
        public static float soloPlayer07mouseAngle = 0f;
        public static GameObject soloPlayer07Wep = null;
        public static float soloPlayer07Lifes = 3;
        public static PlayerControl soloPlayer08 = null;
        public static float soloPlayer08mouseAngle = 0f;
        public static GameObject soloPlayer08Wep = null;
        public static float soloPlayer08Lifes = 3;
        public static PlayerControl soloPlayer09 = null;
        public static float soloPlayer09mouseAngle = 0f;
        public static GameObject soloPlayer09Wep = null;
        public static float soloPlayer09Lifes = 3;
        public static PlayerControl soloPlayer10 = null;
        public static float soloPlayer10mouseAngle = 0f;
        public static GameObject soloPlayer10Wep = null;
        public static float soloPlayer10Lifes = 3;
        public static PlayerControl soloPlayer11 = null;
        public static float soloPlayer11mouseAngle = 0f;
        public static GameObject soloPlayer11Wep = null;
        public static float soloPlayer11Lifes = 3;
        public static PlayerControl soloPlayer12 = null;
        public static float soloPlayer12mouseAngle = 0f;
        public static GameObject soloPlayer12Wep = null;
        public static float soloPlayer12Lifes = 3;
        public static PlayerControl soloPlayer13 = null;
        public static float soloPlayer13mouseAngle = 0f;
        public static GameObject soloPlayer13Wep = null;
        public static float soloPlayer13Lifes = 3;
        public static PlayerControl soloPlayer14 = null;
        public static float soloPlayer14mouseAngle = 0f;
        public static GameObject soloPlayer14Wep = null;
        public static float soloPlayer14Lifes = 3;
        public static PlayerControl soloPlayer15 = null;
        public static float soloPlayer15mouseAngle = 0f;
        public static GameObject soloPlayer15Wep = null;
        public static float soloPlayer15Lifes = 3;

        public static List<PlayerControl> limeTeam = new List<PlayerControl>();
        public static PlayerControl limePlayer01 = null;
        public static float limePlayer01mouseAngle = 0f;
        public static GameObject limePlayer01Wep = null;
        public static float limePlayer01Lifes = 3;
        public static bool limePlayer01IsReviving = false;
        public static PlayerControl limePlayer02 = null;
        public static float limePlayer02mouseAngle = 0f;
        public static GameObject limePlayer02Wep = null;
        public static float limePlayer02Lifes = 3;
        public static bool limePlayer02IsReviving = false;
        public static PlayerControl limePlayer03 = null;
        public static float limePlayer03mouseAngle = 0f;
        public static GameObject limePlayer03Wep = null;
        public static float limePlayer03Lifes = 3;
        public static bool limePlayer03IsReviving = false;
        public static PlayerControl limePlayer04 = null;
        public static float limePlayer04mouseAngle = 0f;
        public static GameObject limePlayer04Wep = null;
        public static float limePlayer04Lifes = 3;
        public static bool limePlayer04IsReviving = false;
        public static PlayerControl limePlayer05 = null;
        public static float limePlayer05mouseAngle = 0f;
        public static GameObject limePlayer05Wep = null;
        public static float limePlayer05Lifes = 3;
        public static bool limePlayer05IsReviving = false;
        public static PlayerControl limePlayer06 = null;
        public static float limePlayer06mouseAngle = 0f;
        public static GameObject limePlayer06Wep = null;
        public static float limePlayer06Lifes = 3;
        public static bool limePlayer06IsReviving = false;
        public static PlayerControl limePlayer07 = null;
        public static float limePlayer07mouseAngle = 0f;
        public static GameObject limePlayer07Wep = null;
        public static float limePlayer07Lifes = 3;
        public static bool limePlayer07IsReviving = false;

        public static List<PlayerControl> pinkTeam = new List<PlayerControl>();
        public static PlayerControl pinkPlayer01 = null;
        public static float pinkPlayer01mouseAngle = 0f;
        public static GameObject pinkPlayer01Wep = null;
        public static float pinkPlayer01Lifes = 3;
        public static bool pinkPlayer01IsReviving = false;
        public static PlayerControl pinkPlayer02 = null;
        public static float pinkPlayer02mouseAngle = 0f;
        public static GameObject pinkPlayer02Wep = null;
        public static float pinkPlayer02Lifes = 3;
        public static bool pinkPlayer02IsReviving = false;
        public static PlayerControl pinkPlayer03 = null;
        public static float pinkPlayer03mouseAngle = 0f;
        public static GameObject pinkPlayer03Wep = null;
        public static float pinkPlayer03Lifes = 3;
        public static bool pinkPlayer03IsReviving = false;
        public static PlayerControl pinkPlayer04 = null;
        public static float pinkPlayer04mouseAngle = 0f;
        public static GameObject pinkPlayer04Wep = null;
        public static float pinkPlayer04Lifes = 3;
        public static bool pinkPlayer04IsReviving = false;
        public static PlayerControl pinkPlayer05 = null;
        public static float pinkPlayer05mouseAngle = 0f;
        public static GameObject pinkPlayer05Wep = null;
        public static float pinkPlayer05Lifes = 3;
        public static bool pinkPlayer05IsReviving = false;
        public static PlayerControl pinkPlayer06 = null;
        public static float pinkPlayer06mouseAngle = 0f;
        public static GameObject pinkPlayer06Wep = null;
        public static float pinkPlayer06Lifes = 3;
        public static bool pinkPlayer06IsReviving = false;
        public static PlayerControl pinkPlayer07 = null;
        public static float pinkPlayer07mouseAngle = 0f;
        public static GameObject pinkPlayer07Wep = null;
        public static float pinkPlayer07Lifes = 3;
        public static bool pinkPlayer07IsReviving = false;

        public static List<PlayerControl> serialKillerTeam = new List<PlayerControl>();
        public static PlayerControl serialKiller = null;
        public static float serialKillermouseAngle = 0f;
        public static GameObject serialKillerWep = null;
        public static float serialKillerLifes = 3;
        public static float serialKillerCooldown = 3;
        public static bool serialKillerIsReviving = false;
        public static List<GameObject> serialKillerSpawns = new List<GameObject>();

        public static List<Vector3> soloPlayersSpawnPositions = new List<Vector3>();
        public static bool battleRoyaleSenseiMapMode = false;
        public static bool battleRoyaleDleksMap = false;

        public static float killCooldown = 1;
        public static float fighterLifes = 3f;
        public static int matchType = 0;
        public static float requiredScore = 0;

        public static int limePoints = 0;
        public static int pinkPoints = 0;
        public static int serialKillerPoints = 0;

        public static bool triggerSoloWin = false;
        public static bool triggerTimeWin = false;
        public static bool triggerLimeTeamWin = false;
        public static bool triggerPinkTeamWin = false;
        public static bool triggerSerialKillerWin = false;
        public static bool triggerDrawWin = false;

        public static string battleRoyalepointCounter = "";

        public static void clearAndReload() {
            fighterLifes = CustomOptionHolder.battleRoyaleLifes.getFloat();

            soloPlayersSpawnPositions.Clear();
            soloPlayerTeam.Clear();
            limeTeam.Clear();
            pinkTeam.Clear();
            serialKillerTeam.Clear();
            soloPlayer01 = null;
            soloPlayer01mouseAngle = 0;
            soloPlayer01Wep = null;
            soloPlayer01Lifes = fighterLifes;
            soloPlayer02 = null;
            soloPlayer02mouseAngle = 0;
            soloPlayer02Wep = null;
            soloPlayer02Lifes = fighterLifes;
            soloPlayer03 = null;
            soloPlayer03mouseAngle = 0;
            soloPlayer03Wep = null;
            soloPlayer03Lifes = fighterLifes;
            soloPlayer04 = null;
            soloPlayer04mouseAngle = 0;
            soloPlayer04Wep = null;
            soloPlayer04Lifes = fighterLifes;
            soloPlayer05 = null;
            soloPlayer05mouseAngle = 0;
            soloPlayer05Wep = null;
            soloPlayer05Lifes = fighterLifes;
            soloPlayer06 = null;
            soloPlayer06mouseAngle = 0;
            soloPlayer06Wep = null;
            soloPlayer06Lifes = fighterLifes;
            soloPlayer07 = null;
            soloPlayer07mouseAngle = 0;
            soloPlayer07Wep = null;
            soloPlayer07Lifes = fighterLifes;
            soloPlayer08 = null;
            soloPlayer08mouseAngle = 0;
            soloPlayer08Wep = null;
            soloPlayer08Lifes = fighterLifes;
            soloPlayer09 = null;
            soloPlayer09mouseAngle = 0;
            soloPlayer09Wep = null;
            soloPlayer09Lifes = fighterLifes;
            soloPlayer10 = null;
            soloPlayer10mouseAngle = 0;
            soloPlayer10Wep = null;
            soloPlayer10Lifes = fighterLifes;
            soloPlayer11 = null;
            soloPlayer11mouseAngle = 0;
            soloPlayer11Wep = null;
            soloPlayer11Lifes = fighterLifes;
            soloPlayer12 = null;
            soloPlayer12mouseAngle = 0;
            soloPlayer12Wep = null;
            soloPlayer12Lifes = fighterLifes;
            soloPlayer13 = null;
            soloPlayer13mouseAngle = 0;
            soloPlayer13Wep = null;
            soloPlayer13Lifes = fighterLifes;
            soloPlayer14 = null;
            soloPlayer14mouseAngle = 0;
            soloPlayer14Wep = null;
            soloPlayer14Lifes = fighterLifes;
            soloPlayer15 = null;
            soloPlayer15mouseAngle = 0;
            soloPlayer15Wep = null;
            soloPlayer15Lifes = fighterLifes;

            limePlayer01 = null;
            limePlayer01mouseAngle = 0;
            limePlayer01Wep = null;
            limePlayer01Lifes = fighterLifes;
            limePlayer01IsReviving = false;
            limePlayer02 = null;
            limePlayer02mouseAngle = 0;
            limePlayer02Wep = null;
            limePlayer02Lifes = fighterLifes;
            limePlayer02IsReviving = false;
            limePlayer03 = null;
            limePlayer03mouseAngle = 0;
            limePlayer03Wep = null;
            limePlayer03Lifes = fighterLifes;
            limePlayer03IsReviving = false;
            limePlayer04 = null;
            limePlayer04mouseAngle = 0;
            limePlayer04Wep = null;
            limePlayer04Lifes = fighterLifes;
            limePlayer04IsReviving = false;
            limePlayer05 = null;
            limePlayer05mouseAngle = 0;
            limePlayer05Wep = null;
            limePlayer05Lifes = fighterLifes;
            limePlayer05IsReviving = false;
            limePlayer06 = null;
            limePlayer06mouseAngle = 0;
            limePlayer06Wep = null;
            limePlayer06Lifes = fighterLifes;
            limePlayer06IsReviving = false;
            limePlayer07 = null;
            limePlayer07mouseAngle = 0;
            limePlayer07Wep = null;
            limePlayer07Lifes = fighterLifes;
            limePlayer07IsReviving = false;
            pinkPlayer01 = null;
            pinkPlayer01mouseAngle = 0;
            pinkPlayer01Wep = null;
            pinkPlayer01Lifes = fighterLifes;
            pinkPlayer01IsReviving = false;
            pinkPlayer02 = null;
            pinkPlayer02mouseAngle = 0;
            pinkPlayer02Wep = null;
            pinkPlayer02Lifes = fighterLifes;
            pinkPlayer02IsReviving = false;
            pinkPlayer03 = null;
            pinkPlayer03mouseAngle = 0;
            pinkPlayer03Wep = null;
            pinkPlayer03Lifes = fighterLifes;
            pinkPlayer03IsReviving = false;
            pinkPlayer04 = null;
            pinkPlayer04mouseAngle = 0;
            pinkPlayer04Wep = null;
            pinkPlayer04Lifes = fighterLifes;
            pinkPlayer04IsReviving = false;
            pinkPlayer05 = null;
            pinkPlayer05mouseAngle = 0;
            pinkPlayer05Wep = null;
            pinkPlayer05Lifes = fighterLifes;
            pinkPlayer05IsReviving = false;
            pinkPlayer06 = null;
            pinkPlayer06mouseAngle = 0;
            pinkPlayer06Wep = null;
            pinkPlayer06Lifes = fighterLifes;
            pinkPlayer06IsReviving = false;
            pinkPlayer07 = null;
            pinkPlayer07mouseAngle = 0;
            pinkPlayer07Wep = null;
            pinkPlayer07Lifes = fighterLifes;
            pinkPlayer07IsReviving = false;
            serialKiller = null;
            serialKillermouseAngle = 0;
            serialKillerWep = null;
            serialKillerIsReviving = false;
            serialKillerSpawns.Clear();

            killCooldown = CustomOptionHolder.battleRoyaleKillCooldown.getFloat();
            battleRoyaleSenseiMapMode = CustomOptionHolder.activateSenseiMap.getBool();
            battleRoyaleDleksMap = CustomOptionHolder.activateDleksMap.getBool();
            matchType = CustomOptionHolder.battleRoyaleMatchType.getSelection();
            if (PlayerInCache.AllPlayers.Count >= 11) {
                serialKillerCooldown = killCooldown / 3;
                serialKillerLifes = fighterLifes * 3;
            }
            else {
                serialKillerCooldown = killCooldown / 2;
                serialKillerLifes = fighterLifes * 2;
            }
            requiredScore = CustomOptionHolder.battleRoyaleScoreNeeded.getFloat();

            limePoints = 0;
            pinkPoints = 0;
            serialKillerPoints = 0; 

            triggerSoloWin = false;
            triggerTimeWin = false;
            triggerLimeTeamWin = false;
            triggerPinkTeamWin = false;
            triggerSerialKillerWin = false;
            triggerDrawWin = false;

            switch (matchType) {
                case 0:
                    battleRoyalepointCounter = Language.introTexts[11] + "<color=#009F57FF>" + soloPlayerTeam.Count + "</color>";
                    break;
                case 1:
                    if (serialKiller != null) {
                        battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + limeTeam.Count + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + pinkTeam.Count + "</color> | " + Language.introTexts[14] + "<color=#808080FF>" + serialKillerTeam.Count + "</color>";
                    }
                    else {
                        battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + limeTeam.Count + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + pinkTeam.Count + "</color>";
                    }
                    break;
                case 2:
                    if (serialKiller != null) {
                        battleRoyalepointCounter = Language.introTexts[15] + requiredScore + " | <color=#39FF14FF>" + Language.introTexts[12] + limePoints + "</color> | " + "<color=#F2BEFFFF>" + Language.introTexts[13] + pinkPoints + "</color> | " + "<color=#808080FF>" + Language.introTexts[16] + serialKillerPoints + "</color>";
                    }
                    else {
                        battleRoyalepointCounter = Language.introTexts[15] + requiredScore + " | <color=#39FF14FF>" + Language.introTexts[12] + limePoints + "</color> | " + "<color=#F2BEFFFF>" + Language.introTexts[13] + pinkPoints + "</color>";
                    }
                    break;
            }

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    if (battleRoyaleSenseiMapMode) {
                        soloPlayersSpawnPositions.Add(new Vector3(-6.8f, 11f, 0f)); // secutiry
                        soloPlayersSpawnPositions.Add(new Vector3(10f, -2.15f, 0f)); // wep shields
                        soloPlayersSpawnPositions.Add(new Vector3(-19f, 5.5f, 0f)); // upper engine
                        soloPlayersSpawnPositions.Add(new Vector3(7f, -14f, 0f)); // nav
                        soloPlayersSpawnPositions.Add(new Vector3(-6.25f, -1.5f, 0f)); // medbey
                        soloPlayersSpawnPositions.Add(new Vector3(-18.85f, -8f, 0f)); // lower engine
                        soloPlayersSpawnPositions.Add(new Vector3(4.75f, -8.5f, 0f)); // admin
                        soloPlayersSpawnPositions.Add(new Vector3(-0.75f, -1.5f, 0f)); // cafete
                        soloPlayersSpawnPositions.Add(new Vector3(-12f, 7f, 0f)); // empty room
                        soloPlayersSpawnPositions.Add(new Vector3(-5.5f, -13.15f, 0f)); // elect
                        soloPlayersSpawnPositions.Add(new Vector3(6.75f, 4.75f, 0f)); // comms
                        soloPlayersSpawnPositions.Add(new Vector3(-8.4f, -0.4f, 0f)); // o2
                        soloPlayersSpawnPositions.Add(new Vector3(-12f, -12.75f, 0f)); // storage
                        soloPlayersSpawnPositions.Add(new Vector3(-3.75f, 5f, 0f)); // hidden cafeteria room
                        soloPlayersSpawnPositions.Add(new Vector3(-19.5f, -1.5f, 0f)); // reactor
                    }
                    else if (battleRoyaleDleksMap) {
                        soloPlayersSpawnPositions.Add(new Vector3(8.75f, -8.5f, 0f)); // elec
                        soloPlayersSpawnPositions.Add(new Vector3(9.15f, -4.75f, 0f)); // medbey
                        soloPlayersSpawnPositions.Add(new Vector3(-6f, -3.5f, 0f)); // o2
                        soloPlayersSpawnPositions.Add(new Vector3(-6.25f, -8.5f, 0f)); // admin
                        soloPlayersSpawnPositions.Add(new Vector3(17.75f, 2.5f, 0f)); // upper engine
                        soloPlayersSpawnPositions.Add(new Vector3(-2.75f, -15.25f, 0f)); // comms
                        soloPlayersSpawnPositions.Add(new Vector3(17.75f, -13.25f, 0f)); // lower engine
                        soloPlayersSpawnPositions.Add(new Vector3(-9.75f, 2.75f, 0f)); // weapons
                        soloPlayersSpawnPositions.Add(new Vector3(13.5f, -6.75f, 0f)); // seguridad
                        soloPlayersSpawnPositions.Add(new Vector3(-9.5f, -12.25f, 0f)); // shields
                        soloPlayersSpawnPositions.Add(new Vector3(21.5f, -2.5f, 0f)); // reactor
                        soloPlayersSpawnPositions.Add(new Vector3(-16.5f, -3.5f, 0f)); // nav
                        soloPlayersSpawnPositions.Add(new Vector3(0.75f, 5.25f, 0f)); // caftereria upper
                        soloPlayersSpawnPositions.Add(new Vector3(1.75f, -16f, 0f)); // stoage
                        soloPlayersSpawnPositions.Add(new Vector3(0.75f, -2.75f, 0f)); // caftereria lower
                    }
                    else {
                        soloPlayersSpawnPositions.Add(new Vector3(-8.75f, -8.5f, 0f)); // elec
                        soloPlayersSpawnPositions.Add(new Vector3(-9.15f, -4.75f, 0f)); // medbey
                        soloPlayersSpawnPositions.Add(new Vector3(6f, -3.5f, 0f)); // o2
                        soloPlayersSpawnPositions.Add(new Vector3(6.25f, -8.5f, 0f)); // admin
                        soloPlayersSpawnPositions.Add(new Vector3(-17.75f, 2.5f, 0f)); // upper engine
                        soloPlayersSpawnPositions.Add(new Vector3(2.75f, -15.25f, 0f)); // comms
                        soloPlayersSpawnPositions.Add(new Vector3(-17.75f, -13.25f, 0f)); // lower engine
                        soloPlayersSpawnPositions.Add(new Vector3(9.75f, 2.75f, 0f)); // weapons
                        soloPlayersSpawnPositions.Add(new Vector3(-13.5f, -6.75f, 0f)); // seguridad
                        soloPlayersSpawnPositions.Add(new Vector3(9.5f, -12.25f, 0f)); // shields
                        soloPlayersSpawnPositions.Add(new Vector3(-21.5f, -2.5f, 0f)); // reactor
                        soloPlayersSpawnPositions.Add(new Vector3(16.5f, -3.5f, 0f)); // nav
                        soloPlayersSpawnPositions.Add(new Vector3(-0.75f, 5.25f, 0f)); // caftereria upper
                        soloPlayersSpawnPositions.Add(new Vector3(-1.75f, -16f, 0f)); // stoage
                        soloPlayersSpawnPositions.Add(new Vector3(-0.75f, -2.75f, 0f)); // caftereria lower
                    }
                    break;
                case 1:
                    soloPlayersSpawnPositions.Add(new Vector3(19.5f, 4.65f, 0f)); // storage
                    soloPlayersSpawnPositions.Add(new Vector3(11.25f, 10.5f, 0f)); // lab
                    soloPlayersSpawnPositions.Add(new Vector3(14.75f, 20.5f, 0f)); // office
                    soloPlayersSpawnPositions.Add(new Vector3(15.5f, -0.5f, 0f)); // medbey
                    soloPlayersSpawnPositions.Add(new Vector3(27.5f, -1.75f, 0f)); // balcony
                    soloPlayersSpawnPositions.Add(new Vector3(2.5f, 13.15f, 0f)); // reactor
                    soloPlayersSpawnPositions.Add(new Vector3(15.5f, 4f, 0f)); // comms
                    soloPlayersSpawnPositions.Add(new Vector3(21f, 20.5f, 0f)); // admin
                    soloPlayersSpawnPositions.Add(new Vector3(27, 4.75f, 0f)); // cafeteria
                    soloPlayersSpawnPositions.Add(new Vector3(6.15f, 6.5f, 0f)); // decom
                    soloPlayersSpawnPositions.Add(new Vector3(5f, -1.25f, 0f)); // long hallway
                    soloPlayersSpawnPositions.Add(new Vector3(16.15f, 24.25f, 0f)); // greenhouse
                    soloPlayersSpawnPositions.Add(new Vector3(-4.35f, 3.25f, 0f)); // launch pad
                    soloPlayersSpawnPositions.Add(new Vector3(9.5f, 1.25f, 0f)); // lockroom
                    soloPlayersSpawnPositions.Add(new Vector3(18f, 11.5f, 0f)); // midway
                    break;
                case 2:
                    soloPlayersSpawnPositions.Add(new Vector3(9.75f, -12.15f, 0f)); // elec
                    soloPlayersSpawnPositions.Add(new Vector3(40.5f, -7.75f, 0f)); // right lab
                    soloPlayersSpawnPositions.Add(new Vector3(11f, -23f, 0f)); // weapons
                    soloPlayersSpawnPositions.Add(new Vector3(36.5f, -21.5f, 0f)); // specifmen
                    soloPlayersSpawnPositions.Add(new Vector3(1f, -16.5f, 0f)); // up o2
                    soloPlayersSpawnPositions.Add(new Vector3(27.75f, -7.5f, 0f)); // left lab
                    soloPlayersSpawnPositions.Add(new Vector3(26.5f, -17f, 0f)); // right office
                    soloPlayersSpawnPositions.Add(new Vector3(12.5f, -16.5f, 0f)); // comms
                    soloPlayersSpawnPositions.Add(new Vector3(16.75f, -1f, 0f)); // launch pad
                    soloPlayersSpawnPositions.Add(new Vector3(22f, -25.15f, 0f)); // admin
                    soloPlayersSpawnPositions.Add(new Vector3(1.75f, -23.75f, 0f)); // low o2
                    soloPlayersSpawnPositions.Add(new Vector3(17.15f, -17f, 0f)); // left office
                    soloPlayersSpawnPositions.Add(new Vector3(3.75f, -12f, 0f)); // secutiry
                    soloPlayersSpawnPositions.Add(new Vector3(20.75f, -12f, 0f)); // storage
                    soloPlayersSpawnPositions.Add(new Vector3(1.5f, -20f, 0f)); // mid o2
                    break;
                case 3:
                    soloPlayersSpawnPositions.Add(new Vector3(8.75f, -8.5f, 0f)); // elec
                    soloPlayersSpawnPositions.Add(new Vector3(9.15f, -4.75f, 0f)); // medbey
                    soloPlayersSpawnPositions.Add(new Vector3(-6f, -3.5f, 0f)); // o2
                    soloPlayersSpawnPositions.Add(new Vector3(-6.25f, -8.5f, 0f)); // admin
                    soloPlayersSpawnPositions.Add(new Vector3(17.75f, 2.5f, 0f)); // upper engine
                    soloPlayersSpawnPositions.Add(new Vector3(-2.75f, -15.25f, 0f)); // comms
                    soloPlayersSpawnPositions.Add(new Vector3(17.75f, -13.25f, 0f)); // lower engine
                    soloPlayersSpawnPositions.Add(new Vector3(-9.75f, 2.75f, 0f)); // weapons
                    soloPlayersSpawnPositions.Add(new Vector3(13.5f, -6.75f, 0f)); // seguridad
                    soloPlayersSpawnPositions.Add(new Vector3(-9.5f, -12.25f, 0f)); // shields
                    soloPlayersSpawnPositions.Add(new Vector3(21.5f, -2.5f, 0f)); // reactor
                    soloPlayersSpawnPositions.Add(new Vector3(-16.5f, -3.5f, 0f)); // nav
                    soloPlayersSpawnPositions.Add(new Vector3(0.75f, 5.25f, 0f)); // caftereria upper
                    soloPlayersSpawnPositions.Add(new Vector3(1.75f, -16f, 0f)); // stoage
                    soloPlayersSpawnPositions.Add(new Vector3(0.75f, -2.75f, 0f)); // caftereria lower
                    break;
                case 4:
                    soloPlayersSpawnPositions.Add(new Vector3(-0.5f, -1, 0f));
                    break;
                case 5:
                    soloPlayersSpawnPositions.Add(new Vector3(-18.25f, 5f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(-22.65f, -7.15f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(2, 4.35f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(-3.15f, -10.5f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(23.7f, -7.8f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(-4.75f, -1.75f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(8f, -10f, 0f));
                    soloPlayersSpawnPositions.Add(new Vector3(7f, 1.75f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(13.25f, 10, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(22.3f, 3.3f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(20.5f, 7.35f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(24.15f, 14.45f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(-16.12f, 0.7f, 0f)); 
                    soloPlayersSpawnPositions.Add(new Vector3(1.65f, -1.5f, 0f));
                    soloPlayersSpawnPositions.Add(new Vector3(10.5f, -12, 0f)); 
                    break;
                case 6:
                    soloPlayersSpawnPositions.Add(new Vector3(3.75f, -26.5f, 0f));
                    break;
            }
        }

        public static PlayerControl GetShotPlayer(float shotSize, float effectiveRange, int whichPlayerShot) {
            float playerMouse = 0;
            Vector2 originPlayer = new Vector2(0, 0);
            switch (whichPlayerShot) {
                case 1:
                    playerMouse = soloPlayer01mouseAngle;
                    originPlayer = soloPlayer01.GetTruePosition();
                    break;
                case 2:
                    playerMouse = soloPlayer02mouseAngle;
                    originPlayer = soloPlayer02.GetTruePosition();
                    break;
                case 3:
                    playerMouse = soloPlayer03mouseAngle;
                    originPlayer = soloPlayer03.GetTruePosition();
                    break;
                case 4:
                    playerMouse = soloPlayer04mouseAngle;
                    originPlayer = soloPlayer04.GetTruePosition();
                    break;
                case 5:
                    playerMouse = soloPlayer05mouseAngle;
                    originPlayer = soloPlayer05.GetTruePosition();
                    break;
                case 6:
                    playerMouse = soloPlayer06mouseAngle;
                    originPlayer = soloPlayer06.GetTruePosition();
                    break;
                case 7:
                    playerMouse = soloPlayer07mouseAngle;
                    originPlayer = soloPlayer07.GetTruePosition();
                    break;
                case 8:
                    playerMouse = soloPlayer08mouseAngle;
                    originPlayer = soloPlayer08.GetTruePosition();
                    break;
                case 9:
                    playerMouse = soloPlayer09mouseAngle;
                    originPlayer = soloPlayer09.GetTruePosition();
                    break;
                case 10:
                    playerMouse = soloPlayer10mouseAngle;
                    originPlayer = soloPlayer10.GetTruePosition();
                    break;
                case 11:
                    playerMouse = soloPlayer11mouseAngle;
                    originPlayer = soloPlayer11.GetTruePosition();
                    break;
                case 12:
                    playerMouse = soloPlayer12mouseAngle;
                    originPlayer = soloPlayer12.GetTruePosition();
                    break;
                case 13:
                    playerMouse = soloPlayer13mouseAngle;
                    originPlayer = soloPlayer13.GetTruePosition();
                    break;
                case 14:
                    playerMouse = soloPlayer14mouseAngle;
                    originPlayer = soloPlayer14.GetTruePosition();
                    break;
                case 15:
                    playerMouse = soloPlayer15mouseAngle;
                    originPlayer = soloPlayer15.GetTruePosition();
                    break;
            }
            PlayerControl result = null;
            float num = effectiveRange;
            Vector3 pos;
            float mouseAngle = playerMouse;
            foreach (PlayerControl player in soloPlayerTeam) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead || player.inVent) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    if (!PhysicsHelpers.AnythingBetween(
                            originPlayer,
                            player.GetTruePosition(),
                            Constants.ShipOnlyMask,
                            false
                        )) {
                        result = player;
                    }
                }
            }
            return result;
        }
        public static PlayerControl GetLimeShotPlayer(float shotSize, float effectiveRange, int whichPlayerShot) {
            float playerMouse = 0;
            Vector2 originPlayer = new Vector2(0, 0);
            switch (whichPlayerShot) {
                case 1:
                    playerMouse = limePlayer01mouseAngle;
                    originPlayer = limePlayer01.GetTruePosition();
                    break;
                case 2:
                    playerMouse = limePlayer02mouseAngle;
                    originPlayer = limePlayer02.GetTruePosition();
                    break;
                case 3:
                    playerMouse = limePlayer03mouseAngle;
                    originPlayer = limePlayer03.GetTruePosition();
                    break;
                case 4:
                    playerMouse = limePlayer04mouseAngle;
                    originPlayer = limePlayer04.GetTruePosition();
                    break;
                case 5:
                    playerMouse = limePlayer05mouseAngle;
                    originPlayer = limePlayer05.GetTruePosition();
                    break;
                case 6:
                    playerMouse = limePlayer06mouseAngle;
                    originPlayer = limePlayer06.GetTruePosition();
                    break;
                case 7:
                    playerMouse = limePlayer07mouseAngle;
                    originPlayer = limePlayer07.GetTruePosition();
                    break;
            }
            PlayerControl result = null;
            float num = effectiveRange;
            Vector3 pos;
            float mouseAngle = playerMouse;
            foreach (PlayerControl player in pinkTeam) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead || player.inVent) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    if (!PhysicsHelpers.AnythingBetween(
                            originPlayer,
                            player.GetTruePosition(),
                            Constants.ShipOnlyMask,
                            false
                        )) {
                        result = player;
                    }
                }
            }
            foreach (PlayerControl player in serialKillerTeam) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead || player.inVent) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    if (!PhysicsHelpers.AnythingBetween(
                            originPlayer,
                            player.GetTruePosition(),
                            Constants.ShipOnlyMask,
                            false
                        )) {
                        result = player;
                    }
                }
            }
            return result;
        }
        public static PlayerControl GetPinkShotPlayer(float shotSize, float effectiveRange, int whichPlayerShot) {
            float playerMouse = 0;
            Vector2 originPlayer = new Vector2(0, 0);
            switch (whichPlayerShot) {
                case 1:
                    playerMouse = pinkPlayer01mouseAngle;
                    originPlayer = pinkPlayer01.GetTruePosition();
                    break;
                case 2:
                    playerMouse = pinkPlayer02mouseAngle;
                    originPlayer = pinkPlayer02.GetTruePosition();
                    break;
                case 3:
                    playerMouse = pinkPlayer03mouseAngle;
                    originPlayer = pinkPlayer03.GetTruePosition();
                    break;
                case 4:
                    playerMouse = pinkPlayer04mouseAngle;
                    originPlayer = pinkPlayer04.GetTruePosition();
                    break;
                case 5:
                    playerMouse = pinkPlayer05mouseAngle;
                    originPlayer = pinkPlayer05.GetTruePosition();
                    break;
                case 6:
                    playerMouse = pinkPlayer06mouseAngle;
                    originPlayer = pinkPlayer06.GetTruePosition();
                    break;
                case 7:
                    playerMouse = pinkPlayer07mouseAngle;
                    originPlayer = pinkPlayer07.GetTruePosition();
                    break;
            }
            PlayerControl result = null;
            float num = effectiveRange;
            Vector3 pos;
            float mouseAngle = playerMouse;
            foreach (PlayerControl player in limeTeam) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead || player.inVent) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    if (!PhysicsHelpers.AnythingBetween(
                            originPlayer,
                            player.GetTruePosition(),
                            Constants.ShipOnlyMask,
                            false
                        )) {
                        result = player;
                    }
                }
            }
            foreach (PlayerControl player in serialKillerTeam) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead || player.inVent) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    if (!PhysicsHelpers.AnythingBetween(
                            originPlayer,
                            player.GetTruePosition(),
                            Constants.ShipOnlyMask,
                            false
                        )) {
                        result = player;
                    }
                }
            }
            return result;
        }
        public static PlayerControl GetSerialShootPlayer(float shotSize, float effectiveRange) {
            PlayerControl result = null;
            float num = effectiveRange;
            Vector3 pos;
            float mouseAngle = serialKillermouseAngle;
            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    if (!PhysicsHelpers.AnythingBetween(
                            serialKiller.GetTruePosition(),
                            player.GetTruePosition(),
                            Constants.ShipOnlyMask,
                            false
                        )) {
                        result = player;
                    }
                }
            }
            return result;
        }
    }

    public static class MonjaFestival
    {
        public static List<PlayerControl> greenTeam = new List<PlayerControl>();
        public static GameObject greenTeamBase = null;
        public static PlayerControl greenPlayer01 = null;
        public static bool greenPlayer01IsReviving = false;
        public static int greenPlayer01Items = 0;
        public static PlayerControl greenPlayer01currentTarget = null;
        public static GameObject greenmonja01selectedSpawn = null;
        public static GameObject greenmonja01currentSpawn = null;
        public static byte greenmonja01foundspawn = 0;
        public static TMPro.TMP_Text greenmonja01DeliverCount;
        public static PlayerControl greenPlayer02 = null;
        public static bool greenPlayer02IsReviving = false;
        public static int greenPlayer02Items = 0;
        public static PlayerControl greenPlayer02currentTarget = null;
        public static GameObject greenmonja02selectedSpawn = null;
        public static GameObject greenmonja02currentSpawn = null;
        public static byte greenmonja02foundspawn = 0;
        public static TMPro.TMP_Text greenmonja02DeliverCount; 
        public static PlayerControl greenPlayer03 = null;
        public static bool greenPlayer03IsReviving = false;
        public static int greenPlayer03Items = 0;
        public static PlayerControl greenPlayer03currentTarget = null;
        public static GameObject greenmonja03selectedSpawn = null;
        public static GameObject greenmonja03currentSpawn = null;
        public static byte greenmonja03foundspawn = 0;
        public static TMPro.TMP_Text greenmonja03DeliverCount;
        public static PlayerControl greenPlayer04 = null;
        public static bool greenPlayer04IsReviving = false;
        public static int greenPlayer04Items = 0;
        public static PlayerControl greenPlayer04currentTarget = null;
        public static GameObject greenmonja04selectedSpawn = null;
        public static GameObject greenmonja04currentSpawn = null;
        public static byte greenmonja04foundspawn = 0;
        public static TMPro.TMP_Text greenmonja04DeliverCount;
        public static PlayerControl greenPlayer05 = null;
        public static bool greenPlayer05IsReviving = false;
        public static int greenPlayer05Items = 0;
        public static PlayerControl greenPlayer05currentTarget = null;
        public static GameObject greenmonja05selectedSpawn = null;
        public static GameObject greenmonja05currentSpawn = null;
        public static byte greenmonja05foundspawn = 0;
        public static TMPro.TMP_Text greenmonja05DeliverCount;
        public static PlayerControl greenPlayer06 = null;
        public static bool greenPlayer06IsReviving = false;
        public static int greenPlayer06Items = 0;
        public static PlayerControl greenPlayer06currentTarget = null;
        public static GameObject greenmonja06selectedSpawn = null;
        public static GameObject greenmonja06currentSpawn = null;
        public static byte greenmonja06foundspawn = 0;
        public static TMPro.TMP_Text greenmonja06DeliverCount;
        public static PlayerControl greenPlayer07 = null;
        public static bool greenPlayer07IsReviving = false;
        public static int greenPlayer07Items = 0;
        public static PlayerControl greenPlayer07currentTarget = null;
        public static GameObject greenmonja07selectedSpawn = null;
        public static GameObject greenmonja07currentSpawn = null;
        public static byte greenmonja07foundspawn = 0;
        public static TMPro.TMP_Text greenmonja07DeliverCount;

        public static List<PlayerControl> cyanTeam = new List<PlayerControl>();
        public static GameObject cyanTeamBase = null;
        public static PlayerControl cyanPlayer01 = null;
        public static bool cyanPlayer01IsReviving = false;
        public static int cyanPlayer01Items = 0;
        public static PlayerControl cyanPlayer01currentTarget = null;
        public static GameObject cyanPlayer01selectedSpawn = null;
        public static GameObject cyanPlayer01currentSpawn = null;
        public static byte cyanPlayer01foundspawn = 0;
        public static TMPro.TMP_Text cyanPlayer01DeliverCount; 
        public static PlayerControl cyanPlayer02 = null;
        public static bool cyanPlayer02IsReviving = false;
        public static int cyanPlayer02Items = 0;
        public static PlayerControl cyanPlayer02currentTarget = null;
        public static GameObject cyanPlayer02selectedSpawn = null;
        public static GameObject cyanPlayer02currentSpawn = null;
        public static byte cyanPlayer02foundspawn = 0;
        public static TMPro.TMP_Text cyanPlayer02DeliverCount; 
        public static PlayerControl cyanPlayer03 = null;
        public static bool cyanPlayer03IsReviving = false;
        public static int cyanPlayer03Items = 0;
        public static PlayerControl cyanPlayer03currentTarget = null;
        public static GameObject cyanPlayer03selectedSpawn = null;
        public static GameObject cyanPlayer03currentSpawn = null;
        public static byte cyanPlayer03foundspawn = 0;
        public static TMPro.TMP_Text cyanPlayer03DeliverCount;
        public static PlayerControl cyanPlayer04 = null;
        public static bool cyanPlayer04IsReviving = false;
        public static int cyanPlayer04Items = 0;
        public static PlayerControl cyanPlayer04currentTarget = null;
        public static GameObject cyanPlayer04selectedSpawn = null;
        public static GameObject cyanPlayer04currentSpawn = null;
        public static byte cyanPlayer04foundspawn = 0;
        public static TMPro.TMP_Text cyanPlayer04DeliverCount;
        public static PlayerControl cyanPlayer05 = null;
        public static bool cyanPlayer05IsReviving = false;
        public static int cyanPlayer05Items = 0;
        public static PlayerControl cyanPlayer05currentTarget = null;
        public static GameObject cyanPlayer05selectedSpawn = null;
        public static GameObject cyanPlayer05currentSpawn = null;
        public static byte cyanPlayer05foundspawn = 0;
        public static TMPro.TMP_Text cyanPlayer05DeliverCount;
        public static PlayerControl cyanPlayer06 = null;
        public static bool cyanPlayer06IsReviving = false;
        public static int cyanPlayer06Items = 0;
        public static PlayerControl cyanPlayer06currentTarget = null;
        public static GameObject cyanPlayer06selectedSpawn = null;
        public static GameObject cyanPlayer06currentSpawn = null;
        public static byte cyanPlayer06foundspawn = 0;
        public static TMPro.TMP_Text cyanPlayer06DeliverCount;
        public static PlayerControl cyanPlayer07 = null;
        public static bool cyanPlayer07IsReviving = false;
        public static int cyanPlayer07Items = 0;
        public static PlayerControl cyanPlayer07currentTarget = null;
        public static GameObject cyanPlayer07selectedSpawn = null;
        public static GameObject cyanPlayer07currentSpawn = null;
        public static byte cyanPlayer07foundspawn = 0;
        public static TMPro.TMP_Text cyanPlayer07DeliverCount;

        public static List<PlayerControl> bigMonjaTeam = new List<PlayerControl>();
        public static GameObject bigMonjaBase = null;
        public static GameObject bigMonjaBaseTwo = null;
        public static PlayerControl bigMonjaPlayer = null;
        public static bool bigMonjaIsReviving = false;
        public static int bigMonjaPlayerItems = 0;
        public static GameObject bigMonjaPlayerselectedSpawn = null;
        public static GameObject bigMonjaPlayercurrentSpawn = null;
        public static byte bigMonjaPlayerfoundspawn = 0;
        public static TMPro.TMP_Text bigMonjaPlayerDeliverCount; 
        public static float bigMonjaPlayerInvisibleTimer = 0;
        public static float bigMonjaPlayerKillCooldown = 10;
        public static float bigMonjaPlayerFindDeliverCooldown = 1;
        public static PlayerControl bigMonjaPlayercurrentTarget = null;
        public static List<GameObject> bigMonjaSpawns = new List<GameObject>();

        public static bool monjaFestivalSenseiMapMode = false;
        public static bool monjaFestivalDleksMap = false;

        public static float grabDeliverTime = 1f;

        public static GameObject bigSpawnOne = null;
        public static int bigSpawnOnePoints = 30;
        public static bool bigSpawnOneReloading = false;
        public static TMPro.TMP_Text bigSpawnOneCount;
        public static GameObject bigSpawnTwo = null;
        public static int bigSpawnTwoPoints = 30;
        public static bool bigSpawnTwoReloading = false;
        public static TMPro.TMP_Text bigSpawnTwoCount;
        public static GameObject littleSpawnOne = null;
        public static int littleSpawnOnePoints = 10;
        public static bool littleSpawnOneReloading = false;
        public static TMPro.TMP_Text littleSpawnOneCount;
        public static GameObject littleSpawnTwo = null;
        public static int littleSpawnTwoPoints = 10;
        public static bool littleSpawnTwoReloading = false;
        public static TMPro.TMP_Text littleSpawnTwoCount;
        public static GameObject littleSpawnThree = null;
        public static int littleSpawnThreePoints = 10;
        public static bool littleSpawnThreeReloading = false;
        public static TMPro.TMP_Text littleSpawnThreeCount;
        public static GameObject littleSpawnFour = null;
        public static int littleSpawnFourPoints = 10;
        public static bool littleSpawnFourReloading = false;
        public static TMPro.TMP_Text littleSpawnFourCount;
        public static int greenPoints = 0;
        public static int cyanPoints = 0;
        public static int bigMonjaPoints = 0;

        public static List<Arrow> localArrows = new List<Arrow>();
        public static List<Vector3> allulMonjaPositions = new List<Vector3>();

        public static GameObject allulMonja = null;

        public static int littleMonjasDroppedCount = 0;

        public static GameObject handsGreen01 = null;
        public static GameObject handsGreen02 = null;
        public static GameObject handsGreen03 = null;
        public static GameObject handsGreen04 = null;
        public static GameObject handsGreen05 = null;
        public static GameObject handsGreen06 = null;
        public static GameObject handsGreen07 = null;
        public static GameObject handsCyan01 = null;
        public static GameObject handsCyan02 = null;
        public static GameObject handsCyan03 = null;
        public static GameObject handsCyan04 = null;
        public static GameObject handsCyan05 = null;
        public static GameObject handsCyan06 = null;
        public static GameObject handsCyan07 = null;

        public static bool triggerGreenTeamWin = false;
        public static bool triggerCyanTeamWin = false;
        public static bool triggerBigMonjaWin = false;
        public static bool triggerDrawWin = false;

        public static string monjaFestivalCounter = "";

        public static void resetBigMonja() {
            bigMonjaPlayerInvisibleTimer = 0f;
            HudManagerStartPatch.bigmonjaInvisibleButton.Timer = HudManagerStartPatch.bigmonjaInvisibleButton.MaxTimer;
            HudManagerStartPatch.bigmonjaInvisibleButton.isEffectActive = false;
            HudManagerStartPatch.bigmonjaInvisibleButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            if (bigMonjaPlayer != null) {
                Helpers.alphaPlayer(false, bigMonjaPlayer.PlayerId);
                MonjaFestival.bigMonjaPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
            }
        }

        private static Sprite greenPickButton;
        public static Sprite getgreenPickButtonSprite() {
            if (greenPickButton) return greenPickButton;
            greenPickButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_GreenPick.png", 90f);
            return greenPickButton;
        }

        private static Sprite greenPickDeliverButton;
        public static Sprite getgreenPickDeliverButtonSprite() {
            if (greenPickDeliverButton) return greenPickDeliverButton;
            greenPickDeliverButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_GreenPickDeliver.png", 90f);
            return greenPickDeliverButton;
        }

        private static Sprite greenDeliverButton;
        public static Sprite getgreenDeliverButtonSprite() {
            if (greenDeliverButton) return greenDeliverButton;
            greenDeliverButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_GreenDeliver.png", 90f);
            return greenDeliverButton;
        }

        private static Sprite cyanPickButton;
        public static Sprite getcyanPickButtonSprite() {
            if (cyanPickButton) return cyanPickButton;
            cyanPickButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_CyanPick.png", 90f);
            return cyanPickButton;
        }

        private static Sprite cyanPickDeliverButton;
        public static Sprite getcyanPickDeliverButtonSprite() {
            if (cyanPickDeliverButton) return cyanPickDeliverButton;
            cyanPickDeliverButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_CyanPickDeliver.png", 90f);
            return cyanPickDeliverButton;
        }

        private static Sprite cyanDeliverButton;
        public static Sprite getcyanDeliverButtonSprite() {
            if (cyanDeliverButton) return cyanDeliverButton;
            cyanDeliverButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_CyanDeliver.png", 90f);
            return cyanDeliverButton;
        }

        private static Sprite greyPickButton;
        public static Sprite getgreyPickButtonSprite() {
            if (greyPickButton) return greyPickButton;
            greyPickButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_GreyPick.png", 90f);
            return greyPickButton;
        }

        private static Sprite greyPickDeliverButton;
        public static Sprite getgreyPickDeliverButtonSprite() {
            if (greyPickDeliverButton) return greyPickDeliverButton;
            greyPickDeliverButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_GreyPickDeliver.png", 90f);
            return greyPickDeliverButton;
        }

        private static Sprite greyDeliverButton;
        public static Sprite getgreyDeliverButtonSprite() {
            if (greyDeliverButton) return greyDeliverButton;
            greyDeliverButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_GreyDeliver.png", 90f);
            return greyDeliverButton;
        }

        private static Sprite greyInvisibleButton;
        public static Sprite getgreyInvisibleBButtonSprite() {
            if (greyInvisibleButton) return greyInvisibleButton;
            greyInvisibleButton = Helpers.loadSpriteFromResources("LasMonjas.Images.MonjaFestival_GreyInvisible.png", 90f);
            return greyInvisibleButton;
        }

        public static void clearAndReload() { 

            greenTeam.Clear();
            cyanTeam.Clear();
            bigMonjaTeam.Clear();
            allulMonjaPositions.Clear();

            greenTeamBase = null;
            greenPlayer01 = null;
            greenPlayer01IsReviving = false;
            greenPlayer01Items = 0;
            greenPlayer01currentTarget = null;
            greenmonja01selectedSpawn = null;
            greenmonja01currentSpawn = null;
            greenmonja01foundspawn = 0; 
            greenPlayer02IsReviving = false;
            greenPlayer02Items = 0;
            greenPlayer02currentTarget = null;
            greenmonja02selectedSpawn = null;
            greenmonja02currentSpawn = null;
            greenmonja02foundspawn = 0; 
            greenPlayer03 = null;
            greenPlayer03IsReviving = false;
            greenPlayer03Items = 0;
            greenPlayer03currentTarget = null;
            greenmonja03selectedSpawn = null;
            greenmonja03currentSpawn = null;
            greenmonja03foundspawn = 0;
            greenPlayer04 = null;
            greenPlayer04IsReviving = false;
            greenPlayer04Items = 0;
            greenPlayer04currentTarget = null;
            greenmonja04selectedSpawn = null;
            greenmonja04currentSpawn = null;
            greenmonja04foundspawn = 0;
            greenPlayer05 = null;
            greenPlayer05IsReviving = false;
            greenPlayer05Items = 0;
            greenPlayer05currentTarget = null;
            greenmonja05selectedSpawn = null;
            greenmonja05currentSpawn = null;
            greenmonja05foundspawn = 0;
            greenPlayer06 = null;
            greenPlayer06IsReviving = false;
            greenPlayer06Items = 0;
            greenPlayer06currentTarget = null;
            greenmonja06selectedSpawn = null;
            greenmonja06currentSpawn = null;
            greenmonja06foundspawn = 0;
            greenPlayer07 = null;
            greenPlayer07IsReviving = false;
            greenPlayer07Items = 0;
            greenPlayer07currentTarget = null;
            greenmonja07selectedSpawn = null;
            greenmonja07currentSpawn = null;
            greenmonja07foundspawn = 0;
            cyanTeamBase = null;
            cyanPlayer01 = null;
            cyanPlayer01IsReviving = false;
            cyanPlayer01Items = 0;
            cyanPlayer01currentTarget = null;
            cyanPlayer01selectedSpawn = null;
            cyanPlayer01currentSpawn = null;
            cyanPlayer01foundspawn = 0; 
            cyanPlayer02 = null;
            cyanPlayer02IsReviving = false;
            cyanPlayer02Items = 0;
            cyanPlayer02currentTarget = null;
            cyanPlayer02selectedSpawn = null;
            cyanPlayer02currentSpawn = null;
            cyanPlayer02foundspawn = 0; 
            cyanPlayer03 = null;
            cyanPlayer03IsReviving = false;
            cyanPlayer03Items = 0;
            cyanPlayer03currentTarget = null;
            cyanPlayer03selectedSpawn = null;
            cyanPlayer03currentSpawn = null;
            cyanPlayer03foundspawn = 0;
            cyanPlayer04 = null;
            cyanPlayer04IsReviving = false;
            cyanPlayer04Items = 0;
            cyanPlayer04currentTarget = null;
            cyanPlayer04selectedSpawn = null;
            cyanPlayer04currentSpawn = null;
            cyanPlayer04foundspawn = 0;
            cyanPlayer05 = null;
            cyanPlayer05IsReviving = false;
            cyanPlayer05Items = 0;
            cyanPlayer05currentTarget = null;
            cyanPlayer05selectedSpawn = null;
            cyanPlayer05currentSpawn = null;
            cyanPlayer05foundspawn = 0;
            cyanPlayer06 = null;
            cyanPlayer06IsReviving = false;
            cyanPlayer06Items = 0;
            cyanPlayer06currentTarget = null;
            cyanPlayer06selectedSpawn = null;
            cyanPlayer06currentSpawn = null;
            cyanPlayer06foundspawn = 0;
            cyanPlayer07 = null;
            cyanPlayer07IsReviving = false;
            cyanPlayer07Items = 0;
            cyanPlayer07currentTarget = null;
            cyanPlayer07selectedSpawn = null;
            cyanPlayer07currentSpawn = null;
            cyanPlayer07foundspawn = 0;
            bigMonjaBase = null;
            bigMonjaBaseTwo = null;
            bigMonjaPlayer = null;
            bigMonjaIsReviving = false;
            bigMonjaPlayerItems = 0;
            bigMonjaPlayercurrentTarget = null;
            bigMonjaPlayerselectedSpawn = null;
            bigMonjaPlayercurrentSpawn = null;
            bigMonjaPlayerfoundspawn = 0;
            bigMonjaPlayerInvisibleTimer = 0f;
            bigMonjaSpawns.Clear();

            grabDeliverTime = 1f;
            monjaFestivalSenseiMapMode = CustomOptionHolder.activateSenseiMap.getBool();
            monjaFestivalDleksMap = CustomOptionHolder.activateDleksMap.getBool();
            if (PlayerInCache.AllPlayers.Count >= 11) {
                bigMonjaPlayerKillCooldown = LasMonjas.gamemodeKillCooldown / 3;
                bigMonjaPlayerFindDeliverCooldown = grabDeliverTime / 3;
            }
            else {
                bigMonjaPlayerKillCooldown = LasMonjas.gamemodeKillCooldown / 2;
                bigMonjaPlayerFindDeliverCooldown = grabDeliverTime / 2;
            }
            greenPoints = 0;
            cyanPoints = 0;
            bigMonjaPoints = 0;

            bigSpawnOne = null;
            bigSpawnTwo = null;
            littleSpawnOne = null;
            littleSpawnTwo = null;
            littleSpawnThree = null;
            littleSpawnFour = null; 
            bigSpawnOnePoints = 30;
            bigSpawnOneReloading = false;
            bigSpawnTwoPoints = 30;
            bigSpawnTwoReloading = false;
            littleSpawnOnePoints = 10;
            littleSpawnOneReloading = false;
            littleSpawnTwoPoints = 10;
            littleSpawnTwoReloading = false;
            littleSpawnThreePoints = 10;
            littleSpawnThreeReloading = false;
            littleSpawnFourPoints = 10;
            littleSpawnFourReloading = false;
            triggerGreenTeamWin = false;
            triggerCyanTeamWin = false;
            triggerBigMonjaWin = false;
            triggerDrawWin = false;

            localArrows = new List<Arrow>();
            allulMonja = null;
            littleMonjasDroppedCount = 0;
            handsGreen01 = null;
            handsGreen02 = null;
            handsGreen03 = null;
            handsGreen04 = null;
            handsGreen05 = null;
            handsGreen06 = null;
            handsGreen07 = null;
            handsCyan01 = null;
            handsCyan02 = null;
            handsCyan03 = null;
            handsCyan04 = null;
            handsCyan05 = null;
            handsCyan06 = null;
            handsCyan07 = null;

            if (bigMonjaPlayer != null) {
                monjaFestivalCounter = Language.introTexts[2] + " | <color=#00FF00FF>" + Language.introTexts[17] + greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + cyanPoints + "</color> | " + "<color=#808080FF>" + Language.introTexts[19] + bigMonjaPoints + "</color>";
            }
            else {
                monjaFestivalCounter = Language.introTexts[2] + " | <color=#00FF00FF>" + Language.introTexts[17] + greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + cyanPoints + "</color>";
            }

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    if (monjaFestivalSenseiMapMode) {
                        allulMonjaPositions.Add(new Vector3(-3.65f, 5f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(-8.1f, -0.4f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(13.8f, -0.3f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(-19.8f, 5.4f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(-8.4f, -13.8f, 0.4f));
                    }
                    else if (monjaFestivalDleksMap) {
                        allulMonjaPositions.Add(new Vector3(9.8f, -8.9f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(-5.25f, -4.65f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(-7.75f, -14.25f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(12.5f, -3.75f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(-4.5f, -9.5f, 0.4f));
                    }
                    else {
                        allulMonjaPositions.Add(new Vector3(-9.8f, -8.9f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(5.25f, -4.65f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(7.75f, -14.25f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(-12.5f, -3.75f, 0.4f));
                        allulMonjaPositions.Add(new Vector3(4.5f, -9.5f, 0.4f));
                    }
                    break;
                case 1:
                    allulMonjaPositions.Add(new Vector3(-5.25f, -0.75f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(28f, -1.75f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(22.5f, 19f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(2.45f, 13.5f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(18.25f, -3.15f, 0.4f));
                    break;
                case 2:
                    allulMonjaPositions.Add(new Vector3(4.65f, -4.5f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(3.75f, -11.75f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(28f, -7.45f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(25, -21.5f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(24.15f, -3.25f, 0.4f));
                    break;
                case 3:
                    allulMonjaPositions.Add(new Vector3(9.8f, -8.9f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(-5.25f, -4.65f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(-7.75f, -14.25f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(12.5f, -3.75f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(-4.5f, -9.5f, 0.4f));
                    break;
                case 4:
                    allulMonjaPositions.Add(new Vector3(9.8f, -8.9f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(13.65f, 6f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(29.25f, 7f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(18, -4f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(-4f, 1.5f, 0.4f));
                    break;
                case 5:
                    allulMonjaPositions.Add(new Vector3(1.5f, -1.5f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(-22.75f, -7.15f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(-7.25f, -9, 0.4f));
                    allulMonjaPositions.Add(new Vector3(-14.25f, -16.5f, 0.4f));
                    allulMonjaPositions.Add(new Vector3(8.15f, 1.15f, 0.4f));
                    break;
                case 6:
                    allulMonjaPositions.Add(new Vector3(-14.5f, -34.25f, -0.01f));
                    allulMonjaPositions.Add(new Vector3(-6.45f, -42.75f, -0.01f));
                    allulMonjaPositions.Add(new Vector3(-11.25f, 30.5f, 0.03f));
                    allulMonjaPositions.Add(new Vector3(0, 33.75f, 0.03f));
                    allulMonjaPositions.Add(new Vector3(14.25f, 24.85f, 0.03f));
                    break;
            }
            allulMonjaPositions.Shuffle();
        }
    }

    public static class CustomMain
    {
        public static CustomAssets customAssets = new CustomAssets();
    }

    public class CustomAssets
    {
        // Custom Bundle Role Assets
        public GameObject bombermanBomb;
        public AudioClip bombermanPlaceBombClip;
        public AudioClip bombermanBombMusic;
        public AudioClip bombermanBombClip;
        public GameObject bombermanArea;
        public GameObject medusaPetrifyProp;
        public AudioClip medusaPetrify;
        public GameObject hypnotistReverse;
        public AudioClip archerBowClip;
        public AudioClip renegadeRecruitMinionClip;
        public GameObject trapperMine;
        public AudioClip trapperStepMineClip;
        public GameObject trapperTrap;
        public AudioClip trapperStepTrapClip;
        public GameObject yinyangerYinyang;
        public AudioClip yinyangerYinyangClip;
        public AudioClip yinyangerYinyangColisionClip;
        public GameObject challengerDuelArena;
        public AudioClip challengerDuelMusic;
        public GameObject challengerRock;
        public GameObject challengerPaper;
        public GameObject challengerScissors;
        public AudioClip challengerDuelKillClip;
        public GameObject monjaRitual;
        public GameObject monjaSprite;
        public GameObject monjaOneSprite;
        public GameObject monjaTwoSprite;
        public GameObject monjaThreeSprite;
        public GameObject monjaFourSprite;
        public GameObject monjaFiveSprite;
        public GameObject strandedVentBox;
        public GameObject strandedInviBox;
        public AudioClip monjaAwakeMusic;
        public AudioClip roleThiefStealRole;
        public AudioClip pyromaniacIgniteClip;
        public GameObject treasureHunterTreasure;
        public AudioClip treasureHunterPlaceTreasure;
        public AudioClip treasureHunterCollectTreasure;
        public GameObject devourerArena;
        public AudioClip devourerDingClip;
        public AudioClip devourerDevourClip;
        public AudioClip poisonerPoisonClip;
        public AudioClip puppeteerClip;
        public GameObject seekerArena;
        public GameObject monjashow;
        public GameObject culoshow;
        public GameObject dioshow;
        public AudioClip seekerMinigameMusic;
        public AudioClip timeTravelerTimeReverseClip;
        public AudioClip squireShieldClip;
        public AudioClip fortuneTellerRevealClip;
        public AudioClip spiritualistRevive;
        public GameObject performerDio;
        public AudioClip performerMusic;
        public AudioClip jinxQuack;
        public GameObject accelSprite;
        public GameObject decelSprite;
        public GameObject positionSprite;
        public AudioClip archerPickBow;
        public AudioClip bountyExilerTarget;
        public AudioClip chameleonInvisible;
        public AudioClip demonBite;
        public AudioClip illusionistHat;
        public AudioClip illusionistLightsOut;
        public AudioClip janitorClean;
        public AudioClip janitorDragBody;
        public AudioClip janitorDropBody_Fall;
        public AudioClip mimicPuppeteerTransform;
        public AudioClip ninjaTime;
        public AudioClip nunPlace;
        public AudioClip paintballDeath;
        public AudioClip painterPaint;
        public AudioClip sorcererHex;
        public AudioClip batEmit;
        public AudioClip detectiveCheck;
        public AudioClip finkSpy;
        public AudioClip forensicGhost;
        public AudioClip hackerHack;
        public AudioClip hunterTarget;
        public AudioClip jailerJail;
        public AudioClip jinxJinx;
        public AudioClip mechanicWelderAction;
        public AudioClip sleuthThere;
        public AudioClip sleuthBody;
        public AudioClip sleuthTarget;
        public AudioClip squireShield;
        public GameObject susBoxRed;
        public GameObject susBoxThreeColor;

        // Custom Bundle Capture the flag Assets
        public AudioClip captureTheFlagMusic;
        public GameObject redflag;
        public GameObject redflagbase;
        public GameObject blueflag;
        public GameObject blueflagbase;
        public GameObject redfloor;
        public GameObject bluefloor;

        // Custom Bundle Police and thief Assets
        public AudioClip policeAndThiefMusic;
        public GameObject cell;
        public GameObject jewelbutton;
        public GameObject freethiefbutton;
        public GameObject jeweldiamond;
        public GameObject jewelruby;
        public GameObject thiefspaceship;
        public GameObject thiefspaceshiphatch;
        public GameObject policeParalyze;
        public AudioClip policeTaser;

        // Custom Bundle King Of The Hill Assets
        public AudioClip kingOfTheHillMusic;
        public GameObject whiteflag;
        public GameObject greenflag;
        public GameObject yellowflag;
        public GameObject whitebase;
        public GameObject greenbase;
        public GameObject yellowbase;
        public GameObject greenaura;
        public GameObject yellowaura;
        public GameObject greenfloor;
        public GameObject yellowfloor;

        // Custom Bundle Hot Potato Assets
        public AudioClip hotPotatoMusic;
        public GameObject hotPotato;

        // Custom Bundle ZombieLaboratory Assets
        public AudioClip zombieLaboratoryMusic;
        public GameObject laboratory;
        public GameObject keyItem01;
        public GameObject keyItem02;
        public GameObject keyItem03;
        public GameObject keyItem04;
        public GameObject keyItem05;
        public GameObject keyItem06;
        public GameObject susBox;
        public GameObject emptyBox;
        public GameObject ammoBox;
        public AudioClip rechargeAmmoClip;
        public GameObject nurseMedKit;
        public GameObject mapMedKit;

        // Custom Bundle Battle Royale Assets
        public AudioClip battleRoyaleMusic;
        public AudioClip royaleGetHit;
        public AudioClip royaleHitPlayer;
        public GameObject royaleShoot;

        // Custom Bundle Monja Festival Assets
        public AudioClip monjaFestivalMusic;
        public GameObject greenBaseEmpty;
        public GameObject greenBaseFull;
        public GameObject cyanBaseEmpty;
        public GameObject cyanBaseFull;
        public GameObject greyBaseEmpty;
        public GameObject greyBaseFull;
        public GameObject bigSpawnOneEmpty;
        public GameObject bigSpawnOneFull;
        public GameObject littleSpawnOneEmpty;
        public GameObject littleSpawnOneFull;
        public GameObject pickOneGreenMonja;
        public GameObject pickTwoGreenMonja;
        public GameObject pickThreeGreenMonja;
        public GameObject pickOneCyanMonja;
        public GameObject pickTwoCyanMonja;
        public GameObject pickThreeCyanMonja;
        public GameObject floorGreenMonja;
        public GameObject floorCyanMonja;
        public GameObject floorGreyMonja;
        public GameObject floorAllulMonja;

        // Custom Map
        public GameObject customMap;
        public GameObject customMinimap;
        public GameObject customComms;

        // Custom Lobby
        public GameObject customLobby;
        public GameObject allulfitti;
        public GameObject allulbanner;

        // Custom Music
        public AudioClip lobbyMusic;
        public AudioClip tasksCalmMusic;
        public AudioClip tasksCoreMusic;
        public AudioClip tasksFinalMusic; 
        public AudioClip meetingCalmMusic;
        public AudioClip meetingCoreMusic;
        public AudioClip meetingFinalMusic;
        public AudioClip winCrewmatesMusic;
        public AudioClip winImpostorsMusic;
        public AudioClip winNeutralsMusic;
        public AudioClip winRebelsMusic;
    }
}