using HarmonyLib;
using System;
using static LasMonjas.LasMonjas;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LasMonjas.Objects;
using LasMonjas.Core;
using AmongUs.GameOptions;
using TMPro;

namespace LasMonjas.Patches
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.OnDestroy))]
    class IntroCutsceneOnDestroyPatch
    {
        public static void Prefix(IntroCutscene __instance) {

            Helpers.activateSenseiMap();

            Helpers.activateDleksMap();

            GameObject allulfitti = GameObject.Instantiate(CustomMain.customAssets.allulfitti, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
            GameObject allulbanner = GameObject.Instantiate(CustomMain.customAssets.allulbanner, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                case 0:
                    if (activatedSensei) {
                        allulfitti.transform.position = new Vector3(-6.8f, -5.2f, 0.5f);
                        allulbanner.SetActive(false);
                    }
                    else if (activatedDleks) {
                        allulfitti.transform.position = new Vector3(13.75f, 2, 0.5f);
                        allulbanner.transform.position = new Vector3(0.75f, -3.5f, 0.5f);
                    }
                    else {
                        allulfitti.transform.position = new Vector3(-13.75f, 2, 0.5f);
                        allulbanner.transform.position = new Vector3(-0.75f, -3.5f, 0.5f);
                    }
                    break;
                case 1:
                    allulfitti.transform.position = new Vector3(6.2f, -0.25f, 0.5f);
                    allulbanner.transform.position = new Vector3(25.5f, 4.75f, 0.5f);
                    break;
                case 2:
                    allulfitti.transform.position = new Vector3(19.3f, -13.3f, 0.5f);
                    allulbanner.transform.position = new Vector3(19.8f, -19.25f, 0.5f);
                    break;
                case 3:
                    allulfitti.transform.position = new Vector3(13.75f, 2, 0.5f);
                    allulbanner.transform.position = new Vector3(0.75f, -3.5f, 0.5f);
                    break;
                case 4:
                    allulfitti.transform.position = new Vector3(7.75f, 7.5f, 0.5f);
                    allulbanner.transform.position = new Vector3(11f, 16.75f, 0.5f);
                    break;
                case 5:
                    allulfitti.transform.position = new Vector3(15f, 12.5f, 0.5f);
                    allulbanner.transform.position = new Vector3(-5f, 4.25f, 0.5f);
                    break;
                case 6:
                    allulfitti.transform.position = new Vector3(6f, 22.65f, -0.5f);
                    allulbanner.transform.position = new Vector3(3.85f, -19.35f, -0.5f);
                    break;
            }

            // Generate alive player icons for Pyromaniac and Poisoner
            int playerCounter = 0;
            if (PlayerInCache.LocalPlayer.PlayerControl != null && HudManager.Instance != null && gameType <= 1) {
                Vector3 bottomLeft = new Vector3(-HudManager.Instance.UseButton.transform.parent.localPosition.x, HudManager.Instance.UseButton.transform.parent.localPosition.y, HudManager.Instance.UseButton.transform.parent.localPosition.z);
                foreach (PlayerControl p in PlayerInCache.AllPlayers) {
                    NetworkedPlayerInfo data = p.Data;
                    PoolablePlayer player = UnityEngine.Object.Instantiate<PoolablePlayer>(__instance.PlayerPrefab, HudManager.Instance.transform);
                    p.SetPlayerMaterialColors(player.cosmetics.currentBodySprite.BodySprite);
                    player.SetSkin(data.DefaultOutfit.SkinId, data.DefaultOutfit.ColorId);
                    player.cosmetics.hat.SetHat(data.DefaultOutfit.HatId, data.DefaultOutfit.ColorId);
                    player.cosmetics.visor.SetVisor(data.DefaultOutfit.VisorId, data.DefaultOutfit.ColorId);
                    player.cosmetics.nameText.text = data.PlayerName;
                    player.SetFlipX(true);
                    MapOptions.playerIcons[p.PlayerId] = player;

                    if (PlayerInCache.LocalPlayer.PlayerControl == Pyromaniac.pyromaniac && p != Pyromaniac.pyromaniac || PlayerInCache.LocalPlayer.PlayerControl == Poisoner.poisoner && p != Poisoner.poisoner) {
                        player.transform.localPosition = bottomLeft + new Vector3(-0.25f, -0.25f, 0) + Vector3.right * playerCounter++ * 0.35f;
                        player.transform.localScale = Vector3.one * 0.2f;
                        player.setSemiTransparent(true);
                        player.gameObject.SetActive(true);
                    }
                    else {
                        player.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    [HarmonyPatch]
    class IntroPatch
    {
        public static void setupIntroTeamIcons(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {

            //SoundManager.Instance.StopSound(CustomMain.customAssets.lobbyMusic);            

            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                switch (gameType) {
                    case 0:
                    case 1:
                        // Intro solo teams (rebels and neutrals)
                        if (Helpers.isNeutral(PlayerInCache.LocalPlayer.PlayerControl)) {
                            var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            soloTeam.Add(PlayerInCache.LocalPlayer.PlayerControl);
                            yourTeam = soloTeam;
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Engineer);
                        }

                        if (Helpers.isRebel(PlayerInCache.LocalPlayer.PlayerControl)) {
                            var soloTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            soloTeam.Add(PlayerInCache.LocalPlayer.PlayerControl);
                            yourTeam = soloTeam;
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                        }                        

                        if (MapOptions.activateMusic) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.tasksCalmMusic, true, 25f);
                        }
                        break;
                    case 2:
                        // CTF
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.captureTheFlagMusic, true, 25f);
                        // Intro capture the flag teams                        
                        if (PlayerInCache.LocalPlayer.PlayerControl == CaptureTheFlag.stealerPlayer) {
                            var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            greyTeam.Add(PlayerInCache.LocalPlayer.PlayerControl);
                            yourTeam = greyTeam;
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                        } else {
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        break;
                    case 3:
                        // PT
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.policeAndThiefMusic, true, 25f);
                        // Intro police and thiefs teams
                        PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);                        
                        break;
                    case 4:
                        // KOTH
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.kingOfTheHillMusic, true, 25f);
                        // Intro king of the hill teams                        
                        if (PlayerInCache.LocalPlayer.PlayerControl == KingOfTheHill.usurperPlayer) {
                            var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            greyTeam.Add(PlayerInCache.LocalPlayer.PlayerControl);
                            yourTeam = greyTeam;
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                        }
                        else {
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        break;
                    case 5:
                        // HP
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.hotPotatoMusic, true, 25f);
                        // Intro hot potato teams
                        if (PlayerInCache.LocalPlayer.PlayerControl == HotPotato.hotPotatoPlayer) {
                            var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            greyTeam.Add(PlayerInCache.LocalPlayer.PlayerControl);
                            yourTeam = greyTeam;
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Impostor);
                        } else {
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        break;
                    case 6:
                        // ZL
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.zombieLaboratoryMusic, true, 25f);
                        // Intro zombie teams                        
                        if (PlayerInCache.LocalPlayer.PlayerControl == ZombieLaboratory.nursePlayer) {                            
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Scientist);
                        }
                        else {
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        break;
                    case 7:
                        // BR
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.battleRoyaleMusic, true, 25f);
                        // Intro Battle Royale
                        if (BattleRoyale.matchType == 0) {
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        else {
                            if (PlayerInCache.LocalPlayer.PlayerControl == BattleRoyale.serialKiller) {
                                var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                                greyTeam.Add(PlayerInCache.LocalPlayer.PlayerControl);
                                yourTeam = greyTeam;
                                PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                            }
                            else {
                                PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                            }
                        }
                        break;
                    case 8:
                        // MF
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.monjaFestivalMusic, true, 25f);                        
                        if (PlayerInCache.LocalPlayer.PlayerControl == MonjaFestival.bigMonjaPlayer) {
                            var greyTeam = new Il2CppSystem.Collections.Generic.List<PlayerControl>();
                            greyTeam.Add(PlayerInCache.LocalPlayer.PlayerControl);
                            yourTeam = greyTeam;
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Shapeshifter);
                        }
                        else {
                            PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IntroSound = Helpers.GetIntroSound(RoleTypes.Crewmate);
                        }
                        break;
                }
            }
        }

        public static void setupIntroTeam(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {

            List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerInCache.LocalPlayer.PlayerControl);
            RoleInfo roleInfo = infos.Where(info => info.roleId != RoleId.Lover).FirstOrDefault();
            if (roleInfo == null) return;
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                switch (gameType) {
                    case 0:
                    case 1:
                        if (roleInfo.TeamId == Team.Neutral) {
                            var neutralColor = new Color32(128, 128, 128, 255);
                            __instance.BackgroundBar.material.color = neutralColor;
                            __instance.TeamTitle.text = Language.teamNames[0];
                            __instance.TeamTitle.color = neutralColor;
                        }
                        else if (roleInfo.TeamId == Team.Rebel) {
                            var rebelColor = new Color32(79, 125, 0, 255);
                            __instance.BackgroundBar.material.color = rebelColor;
                            __instance.TeamTitle.text = Language.teamNames[1];
                            __instance.TeamTitle.color = rebelColor;
                        }
                        break;
                    case 2:
                        // CTF
                        __instance.ImpostorText.text = "";
                        __instance.BackgroundBar.material.color = Sheriff.color;
                        __instance.TeamTitle.text = Language.teamNames[2];
                        __instance.TeamTitle.color = Sheriff.color;
                        break;
                    case 3:
                        // PT
                        __instance.ImpostorText.text = "";
                        __instance.BackgroundBar.material.color = Coward.color;
                        __instance.TeamTitle.text = Language.teamNames[3];
                        __instance.TeamTitle.color = Coward.color;
                        break;
                    case 4:
                        // KOTH
                        __instance.ImpostorText.text = "";
                        __instance.BackgroundBar.material.color = Squire.color;
                        __instance.TeamTitle.text = Language.teamNames[4];
                        __instance.TeamTitle.color = Squire.color;
                        break;
                    case 5:
                        // HP
                        __instance.ImpostorText.text = "";
                        __instance.BackgroundBar.material.color = Locksmith.color;
                        __instance.TeamTitle.text = Language.teamNames[5];
                        __instance.TeamTitle.color = Locksmith.color;
                        break;
                    case 6:
                        // ZL
                        __instance.ImpostorText.text = "";
                        __instance.BackgroundBar.material.color = Hunter.color;
                        __instance.TeamTitle.text = Language.teamNames[6];
                        __instance.TeamTitle.color = Hunter.color;
                        break;
                    case 7:
                        // BR
                        __instance.ImpostorText.text = "";
                        __instance.BackgroundBar.material.color = Sleuth.color;
                        __instance.TeamTitle.text = Language.teamNames[7];
                        __instance.TeamTitle.color = Sleuth.color;
                        break;
                    case 8:
                        // MF
                        __instance.ImpostorText.text = "";
                        __instance.BackgroundBar.material.color = Monja.color;
                        __instance.TeamTitle.text = Language.teamNames[8];
                        __instance.TeamTitle.color = Monja.color;
                        break;
                }
            }
        }

        //[HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
        [HarmonyPatch(typeof(IntroCutscene._ShowRole_d__41), nameof(IntroCutscene._ShowRole_d__41.MoveNext))]
        class ShowRolePatch
        {
            //public static void Postfix(IntroCutscene __instance) {
            private static int last;
            public static void Postfix(IntroCutscene._ShowRole_d__41 __instance) {
                if (__instance.__4__this.GetInstanceID() == last)
                    return;

                last = __instance.__4__this.GetInstanceID(); 
                
                List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerInCache.LocalPlayer.PlayerControl);
                RoleInfo roleInfo = infos.Where(info => info.roleId != RoleId.Lover).FirstOrDefault();

                Color color = new Color(__instance.__4__this.YouAreText.color.r, __instance.__4__this.YouAreText.color.g, __instance.__4__this.YouAreText.color.b, 0f);
                __instance.__4__this.StartCoroutine(Effects.Lerp(0.5f, new Action<float>((t) => {

                    if (roleInfo != null) {
                        __instance.__4__this.RoleText.text = roleInfo.name;
                        __instance.__4__this.RoleBlurbText.text = roleInfo.introDescription;
                        color = roleInfo.color;
                    }

                    if (infos.Any(info => info.roleId == RoleId.Lover)) {
                        PlayerControl otherLover = PlayerInCache.LocalPlayer.PlayerControl == Modifiers.lover1 ? Modifiers.lover2 : Modifiers.lover1;
                        __instance.__4__this.RoleBlurbText.text = PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor ? "<color=#FF00D1FF>Lover</color><color=#FF0000FF>stor</color>" : "<color=#FF00D1FF>Lover</color>";
                        __instance.__4__this.RoleBlurbText.color = PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor ? Color.white : Modifiers.loverscolor;
                        __instance.__4__this.ImpostorText.text = Helpers.cs(Modifiers.loverscolor, $"{Language.introTexts[0]} + {otherLover?.Data?.PlayerName ?? ""} ♥");
                        __instance.__4__this.ImpostorText.gameObject.SetActive(true);
                        __instance.__4__this.BackgroundBar.material.color = Modifiers.loverscolor;
                    }

                    color.a = t;
                    __instance.__4__this.YouAreText.color = color;
                    __instance.__4__this.RoleText.color = color;
                    __instance.__4__this.RoleBlurbText.color = color;
                })));

                // MiraHQ special roles
                if (GameOptionsManager.Instance.currentGameOptions.MapId == 1) {

                    // Create the doorlog access from anywhere to the Vigilant
                    if (Vigilant.vigilantMira != null && Vigilant.vigilantMira == PlayerInCache.LocalPlayer.PlayerControl && !Vigilant.createdDoorLog) {
                        GameObject vigilantDoorLog = GameObject.Find("SurvLogConsole");
                        Vigilant.doorLog = GameObject.Instantiate(vigilantDoorLog, Vigilant.vigilantMira.transform);
                        Vigilant.doorLog.name = "VigilantDoorLog";
                        Vigilant.doorLog.layer = 8; // Assign player layer to ignore collisions
                        Vigilant.doorLog.GetComponent<SpriteRenderer>().enabled = false;
                        Vigilant.doorLog.transform.localPosition = new Vector2(0, -0.5f);
                        Vigilant.createdDoorLog = true;
                    }

                    // Remove decon doors if locksmith spawns
                    if (Locksmith.locksmith != null) {
                        GameObject deconUpperDoor = GameObject.Find("UpperDoor");
                        deconUpperDoor.SetActive(false);
                        GameObject deconLowerDoor = GameObject.Find("LowerDoor");
                        deconLowerDoor.SetActive(false);
                        GameObject deconUpperDoorPanelTop = GameObject.Find("DeconDoorPanel-Top");
                        deconUpperDoorPanelTop.SetActive(false);
                        GameObject deconUpperDoorPanelHigh = GameObject.Find("DeconDoorPanel-High");
                        deconUpperDoorPanelHigh.SetActive(false);
                        GameObject deconUpperDoorPanelBottom = GameObject.Find("DeconDoorPanel-Bottom");
                        deconUpperDoorPanelBottom.SetActive(false);
                        GameObject deconUpperDoorPanelLow = GameObject.Find("DeconDoorPanel-Low");
                        deconUpperDoorPanelLow.SetActive(false);
                    }
                }

                // Create the duel arena if there's a Challenger
                if (Challenger.challenger != null && PlayerInCache.LocalPlayer.PlayerControl != null && !createdduelarena) {
                    GameObject duelArena = GameObject.Instantiate(CustomMain.customAssets.challengerDuelArena, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    duelArena.name = "duelArena";
                    duelArena.transform.position = new Vector3(40, 0f, 1f);
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) { // Create another duel arena on submerged lower floor
                        GameObject lowerduelArena = GameObject.Instantiate(CustomMain.customAssets.challengerDuelArena, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                        lowerduelArena.name = "lowerduelArena";
                        lowerduelArena.transform.position = new Vector3(40, -48.119f, 1f);
                    }
                    createdduelarena = true;
                }

                // Create the seaker arena if there's a Seeker
                if (Seeker.seeker != null && PlayerInCache.LocalPlayer.PlayerControl != null && !createdseekerarena) {
                    GameObject seekerArena = GameObject.Instantiate(CustomMain.customAssets.seekerArena, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
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
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) { // Create another duel arena on submerged lower floor
                        GameObject lowerseekerArena = GameObject.Instantiate(CustomMain.customAssets.seekerArena, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
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
                    createdseekerarena = true;
                }

                // Create the Devourer arena if there's a Devourer
                if (Devourer.devourer != null && PlayerInCache.LocalPlayer.PlayerControl != null && !createddevourerarena) {
                    GameObject devourerArena = GameObject.Instantiate(CustomMain.customAssets.devourerArena, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    devourerArena.name = "devourerArena";
                    devourerArena.transform.position = new Vector3(-40, 0f, 1f);
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) { // Create another devourer arena on submerged lower floor
                        GameObject lowerdevourerArena = GameObject.Instantiate(CustomMain.customAssets.devourerArena, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                        lowerdevourerArena.name = "lowerdevourerArena";
                        lowerdevourerArena.transform.position = new Vector3(-40, -48.119f, 1f);
                    }
                    createddevourerarena = true;
                }
                
                // Bomberman area
                if (Bomberman.bomberman != null && PlayerInCache.LocalPlayer.PlayerControl == Bomberman.bomberman) {
                    GameObject bombArea = GameObject.Instantiate(CustomMain.customAssets.bombermanArea, PlayerInCache.LocalPlayer.PlayerControl.transform);
                    bombArea.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                        bombArea.transform.localPosition = new Vector3(0, 0f, -0.5f);
                    }
                    else {
                        bombArea.transform.localPosition = new Vector3(0, 0f, 1f);
                    }
                    bombArea.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                    Bomberman.bombArea = bombArea;
                    Bomberman.bombArea.SetActive(false);
                }
                
                // Submerged remove Chameleon special vent
                if (Chameleon.chameleon != null && PlayerInCache.LocalPlayer.PlayerControl == Chameleon.chameleon && GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                    GameObject vent = GameObject.Find("LowerCentralVent");
                    vent.GetComponent<BoxCollider2D>().enabled = false;
                }

                // Make object list for Hypnotist and Trapper for traps
                if ((Hypnotist.hypnotist != null && PlayerInCache.LocalPlayer.PlayerControl == Hypnotist.hypnotist) || (Trapper.trapper != null && PlayerInCache.LocalPlayer.PlayerControl == Trapper.trapper)) {
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
                            GameObject fungleZiplineBottom = GameObject.Find("FungleShip(Clone)/Zipline/ZiplineBottomPost");
                            GameObject fungleZiplineTop = GameObject.Find("FungleShip(Clone)/Zipline/ZiplineTopPost");
                            GameObject fungleBottomLeftLadder = GameObject.Find("FungleShip(Clone)/Outside/OutsideHighlands/Ladders/BottomLeftLadder");
                            GameObject fungleBottomRightLadder = GameObject.Find("FungleShip(Clone)/Outside/OutsideHighlands/Ladders/BottomRightLadder");
                            GameObject fungleBottomLedgeLadder = GameObject.Find("FungleShip(Clone)/Outside/OutsideHighlands/Ladders/BottomLedgeLadder");
                            GameObject fungleTopLedgeLadder = GameObject.Find("FungleShip(Clone)/Outside/OutsideHighlands/Ladders/TopLedgeLadder");
                            Hypnotist.objectsCantPlaceTraps.Add(fungleZiplineBottom);
                            Hypnotist.objectsCantPlaceTraps.Add(fungleZiplineTop);
                            Hypnotist.objectsCantPlaceTraps.Add(fungleBottomLeftLadder);
                            Hypnotist.objectsCantPlaceTraps.Add(fungleBottomRightLadder);
                            Hypnotist.objectsCantPlaceTraps.Add(fungleBottomLedgeLadder);
                            Hypnotist.objectsCantPlaceTraps.Add(fungleTopLedgeLadder);
                            break;
                        case 6:
                            GameObject submergedMedScanner = GameObject.Find("console_medscan");
                            Hypnotist.objectsCantPlaceTraps.Add(submergedMedScanner);
                            break;
                    }
                }

                // Make elevator list for Time Traveler
                if ((TimeTraveler.timeTraveler != null || Plumber.plumber != null) && GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                    GameObject westLeftElevatorLower = GameObject.Find("Submerged(Clone)/Elevators/WestLeftElevator/LowerElevator");
                    GameObject westLeftElevatorUpper = GameObject.Find("Submerged(Clone)/Elevators/WestLeftElevator/UpperElevator");
                    GameObject westRightElevatorLower = GameObject.Find("Submerged(Clone)/Elevators/WestRightElevator/LowerElevator");
                    GameObject westRightElevatorUpper = GameObject.Find("Submerged(Clone)/Elevators/WestRightElevator/UpperElevator");
                    GameObject eastLeftElevatorLower = GameObject.Find("Submerged(Clone)/Elevators/EastLeftElevator/LowerElevator");
                    GameObject eastLeftElevatorUpper = GameObject.Find("Submerged(Clone)/Elevators/EastLeftElevator/UpperElevator");
                    GameObject eastRightElevatorLower = GameObject.Find("Submerged(Clone)/Elevators/EastRightElevator/LowerElevator");
                    GameObject eastRightElevatorUpper = GameObject.Find("Submerged(Clone)/Elevators/EastRightElevator/UpperElevator");
                    GameObject serviceElevatorLower = GameObject.Find("Submerged(Clone)/Elevators/ServiceElevator/LowerElevator");
                    GameObject serviceElevatorUpper = GameObject.Find("Submerged(Clone)/Elevators/ServiceElevator/UpperElevator");
                    TimeTraveler.objectsCantPlaceTeleport.Add(westLeftElevatorLower);
                    TimeTraveler.objectsCantPlaceTeleport.Add(westLeftElevatorUpper);
                    TimeTraveler.objectsCantPlaceTeleport.Add(westRightElevatorLower);
                    TimeTraveler.objectsCantPlaceTeleport.Add(westRightElevatorUpper);
                    TimeTraveler.objectsCantPlaceTeleport.Add(eastLeftElevatorLower);
                    TimeTraveler.objectsCantPlaceTeleport.Add(eastLeftElevatorUpper);
                    TimeTraveler.objectsCantPlaceTeleport.Add(eastRightElevatorLower);
                    TimeTraveler.objectsCantPlaceTeleport.Add(eastRightElevatorUpper);
                    TimeTraveler.objectsCantPlaceTeleport.Add(serviceElevatorLower);
                    TimeTraveler.objectsCantPlaceTeleport.Add(serviceElevatorUpper);
                }

                // Remove the swipe card task
                clearSwipeCardTask();

                // Remove airship doors
                //removeAirshipDoors();

                // Activate sensei map
                Helpers.activateSenseiMap();

                // Activate Dleks map
                Helpers.activateDleksMap();

                // Create the jail if there's a Jailer
                if (Jailer.jailer != null && PlayerInCache.LocalPlayer.PlayerControl != null && !createdjail) {
                    GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    cell.name = "cell";
                    cell.gameObject.layer = 9;
                    cell.transform.GetChild(0).gameObject.layer = 9;

                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        case 0:
                            if (activatedSensei) {
                                cell.transform.position = new Vector3(-12f, 7.2f, 0.5f);
                            }
                            else if (activatedDleks) {
                                cell.transform.position = new Vector3(10.25f, 3.38f, 0.5f);
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
                            cell.transform.position = new Vector3(-26.75f, -0.65f, 0.5f);
                            break;
                        case 6:
                            cell.transform.position = new Vector3(-15.25f, 28.4f, 0.5f);
                            // Create another jail on submerged lower floor
                            GameObject celltwo = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                            celltwo.name = "cell_lower";
                            celltwo.transform.position = new Vector3(-1.15f, -21f, -0.01f);
                            celltwo.gameObject.layer = 9;
                            celltwo.transform.GetChild(0).gameObject.layer = 9;
                            break;
                    }
                    createdjail = true;
                }

                // Create susBoxes for Stranded
                if (Stranded.stranded != null && Stranded.stranded == PlayerInCache.LocalPlayer.PlayerControl && !createdStrandedBoxes) {
                    GameObject ammoBox01 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    ammoBox01.transform.position = Stranded.susBoxPositions[0];
                    ammoBox01.name = "ammoBox";
                    GameObject ammoBox02 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    ammoBox02.transform.position = Stranded.susBoxPositions[1];
                    ammoBox02.name = "ammoBox";
                    GameObject ammoBox03 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    ammoBox03.transform.position = Stranded.susBoxPositions[2];
                    ammoBox03.name = "ammoBox";
                    GameObject ventBox = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    ventBox.transform.position = Stranded.susBoxPositions[3];
                    ventBox.name = "ventBox";
                    GameObject invisibleBox = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    invisibleBox.transform.position = Stranded.susBoxPositions[4];
                    invisibleBox.name = "invisibleBox";
                    Stranded.groundItems.Add(ammoBox01);
                    Stranded.groundItems.Add(ammoBox02);
                    Stranded.groundItems.Add(ammoBox03);
                    Stranded.groundItems.Add(ventBox);
                    Stranded.groundItems.Add(invisibleBox);
                    // Nothing boxes
                    for (int i = 0; i < Stranded.susBoxPositions.Count - 5; i++) {
                        GameObject nothingBox = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                        nothingBox.transform.position = Stranded.susBoxPositions[i + 5];
                        nothingBox.name = "nothingBox";
                        Stranded.groundItems.Add(nothingBox);
                    }
                    createdStrandedBoxes = true;
                }

                // Create items for Monja
                if (Monja.monja != null && !createdMonjaItems) {
                    GameObject monjaRitual = GameObject.Instantiate(CustomMain.customAssets.monjaRitual, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    Monja.ritualObject = monjaRitual;
                    Monja.ritualObject.layer = 9;
                    GameObject monjaSprite = GameObject.Instantiate(CustomMain.customAssets.monjaSprite, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    Monja.monjaSprite = monjaSprite;
                    Monja.monjaSprite.transform.parent = Monja.monja.transform;
                    Monja.monjaSprite.transform.position = new Vector3 (0 ,0, -1);
                    Monja.monjaSprite.transform.localPosition = new Vector3 (0 ,0, -1);
                    Monja.monjaSprite.SetActive(false);
                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                        case 0:
                            if (activatedSensei) {
                                Monja.ritualObject.transform.position = new Vector3(3f, 2.25f, 0.5f);
                            }
                            else if (activatedDleks) {                                
                                Monja.ritualObject.transform.position = new Vector3(0.9f, 5f, 0.5f);
                            }
                            else {
                                Monja.ritualObject.transform.position = new Vector3(-0.9f, 5f, 0.5f);
                            }
                            break;
                        case 1:
                            Monja.ritualObject.transform.position = new Vector3(17.85f, 11.35f, 0.5f);
                            break;
                        case 2:
                            Monja.ritualObject.transform.position = new Vector3(13.75f, -9.75f, 0.5f);
                            break;
                        case 3:
                            Monja.ritualObject.transform.position = new Vector3(0.9f, 5f, 0.5f);
                            break;
                        case 4:
                            Monja.ritualObject.transform.position = new Vector3(10.75f, -0.25f, 0.5f);
                            break;
                        case 5:
                            Monja.ritualObject.transform.position = new Vector3(-10.5f, 5.15f, 0.5f);
                            break;
                        case 6:
                            Monja.ritualObject.transform.position = new Vector3(-6.35f, 13.85f, -0.005f);
                            break;
                    }
                    GameObject keyitem01 = GameObject.Instantiate(CustomMain.customAssets.monjaOneSprite, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    keyitem01.transform.position = Monja.itemListPositions[0];
                    keyitem01.name = "item01";
                    Monja.item01 = keyitem01;
                    GameObject keyitem02 = GameObject.Instantiate(CustomMain.customAssets.monjaTwoSprite, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    keyitem02.transform.position = Monja.itemListPositions[1];
                    keyitem02.name = "item02";
                    Monja.item02 = keyitem02;
                    GameObject keyitem03 = GameObject.Instantiate(CustomMain.customAssets.monjaThreeSprite, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    keyitem03.transform.position = Monja.itemListPositions[2];
                    keyitem03.name = "item03";
                    Monja.item03 = keyitem03;
                    GameObject keyitem04 = GameObject.Instantiate(CustomMain.customAssets.monjaFourSprite, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    keyitem04.transform.position = Monja.itemListPositions[3];
                    keyitem04.name = "item04";
                    Monja.item04 = keyitem04;
                    GameObject keyitem05 = GameObject.Instantiate(CustomMain.customAssets.monjaFiveSprite, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    keyitem05.transform.position = Monja.itemListPositions[4];
                    keyitem05.name = "item05";
                    Monja.item05 = keyitem05;
                    Monja.objectList.Add(keyitem01);
                    Monja.objectList.Add(keyitem02);
                    Monja.objectList.Add(keyitem03);
                    Monja.objectList.Add(keyitem04);
                    Monja.objectList.Add(keyitem05);
                    if (Monja.monja != PlayerInCache.LocalPlayer.PlayerControl) {
                        foreach (GameObject keyItem in Monja.objectList) {
                            keyItem.SetActive(false);
                        }
                    }
                    createdMonjaItems = true;
                }

                if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {

                    if (gameType == 1 && !createdWhoAmI) {
                        // Create who Am I Mode
                        string[] crewRoleNames = new string[25] { "captainRole", "mechanicRole", "sheriffRole", "detectiveRole", "forensicRole", "timetravelerRole", "squireRole", "cheaterRole", "fortunetellerRole", "hackerRole", "sleuthRole", "finkRole", "kidRole", "welderRole", "spiritualistRole", "vigilantRole", "hunterRole", "jinxRole", "cowardRole", "batRole", "necromancerRole", "engineerRole", "locksmithRole", "taskmasterRole", "jailerRole" };
                        // Crew boxes
                        for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 35; i++) {
                            GameObject whoAmICrewBox = GameObject.Instantiate(CustomMain.customAssets.susBoxThreeColor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                            whoAmICrewBox.transform.position = ZombieLaboratory.susBoxPositions[i];
                            whoAmICrewBox.name = crewRoleNames[i];
                            whoAmIModeCrewItems.Add(whoAmICrewBox);
                            whoAmIModeGlobalItems.Add(whoAmICrewBox);
                        }

                        string[] impostorRoleNames = new string[15] { "mimicRole", "painterRole", "demonRole", "janitorRole", "illusionistRole", "manipulatorRole", "bombermanRole", "chameleonRole", "gamblerRole", "sorcererRole", "medusaRole", "hypnotistRole", "archerRole", "plumberRole", "librarianRole" };
                        // Impostor boxes
                        for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 45; i++) {
                            GameObject whoAmIImpostorBox = GameObject.Instantiate(CustomMain.customAssets.susBoxRed, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                            whoAmIImpostorBox.transform.position = ZombieLaboratory.susBoxPositions[i + 25];
                            whoAmIImpostorBox.name = impostorRoleNames[i];
                            whoAmIModeImpostorItems.Add(whoAmIImpostorBox);
                            whoAmIModeGlobalItems.Add(whoAmIImpostorBox);
                        }

                        string[] rebelsRoleNames = new string[9] { "renegadeRole", "trapperRole", "yinyangerRole", "challengerRole", "ninjaRole", "berserkerRole", "yandereRole", "strandedRole", "monjaRole" };
                        // Rebel boxes
                        for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 51; i++) {
                            GameObject whoAmIRebelBox = GameObject.Instantiate(CustomMain.customAssets.susBoxThreeColor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                            whoAmIRebelBox.transform.position = ZombieLaboratory.susBoxPositions[i + 40];
                            whoAmIRebelBox.name = rebelsRoleNames[i];
                            whoAmIModeRebelsItems.Add(whoAmIRebelBox);
                            whoAmIModeGlobalItems.Add(whoAmIRebelBox);
                        }

                        string[] neutralsRoleNames = new string[8] { "jokerRole", "pyromaniacRole", "treasurehunterRole", "devourerRole", "poisonerRole", "puppeteerRole", "exilerRole", "seekerRole" };
                        // Neutral boxes
                        for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 52; i++) {
                            GameObject whoAmINeutralBox = GameObject.Instantiate(CustomMain.customAssets.susBoxThreeColor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                            whoAmINeutralBox.transform.position = ZombieLaboratory.susBoxPositions[i + 50];
                            whoAmINeutralBox.name = neutralsRoleNames[i];
                            whoAmIModeNeutralsItems.Add(whoAmINeutralBox);
                            whoAmIModeGlobalItems.Add(whoAmINeutralBox);
                            if (whoAmINeutralBox.name == "pyromaniacRole") {
                                whoAmINeutralBox.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.susBoxRed.GetComponent<SpriteRenderer>().sprite;
                            }
                        }

                        if (PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor) {
                            foreach (GameObject item in whoAmIModeCrewItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                            foreach (GameObject item in whoAmIModeRebelsItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                            foreach (GameObject item in whoAmIModeNeutralsItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                        }
                        else {
                            foreach (GameObject item in whoAmIModeImpostorItems) {
                                item.transform.position = new Vector3(100, 100, 0);
                            }
                        }
                        createdWhoAmI = true;
                    }

                    if (gameType >= 2) {
                        progress = GameObject.Find("ProgressTracker");
                        progress.GetComponentInChildren<TextTranslatorTMP>().enabled = false;
                        progress.GetComponentInChildren<TextMeshPro>().alignment = TextAlignmentOptions.Right;
                        switch (gameType) {
                            case 2:
                                // CTF:
                                Helpers.CreateCTF();
                                progress.GetComponentInChildren<TextMeshPro>().text = Language.introTexts[1] + LasMonjas.gamemodeMatchDuration.ToString("F0");
                                new CustomMessage(CaptureTheFlag.flagpointCounter, LasMonjas.gamemodeMatchDuration, new Vector2(-2.5f, 2.35f), 15);
                                // Add Arrows pointing the flags
                                if (CaptureTheFlag.localRedFlagArrow.Count == 0) CaptureTheFlag.localRedFlagArrow.Add(new Arrow(Color.red));
                                CaptureTheFlag.localRedFlagArrow[0].arrow.SetActive(true);
                                if (CaptureTheFlag.localBlueFlagArrow.Count == 0) CaptureTheFlag.localBlueFlagArrow.Add(new Arrow(Color.blue));
                                CaptureTheFlag.localBlueFlagArrow[0].arrow.SetActive(true); 
                                break;
                            case 3:
                                // PAT:
                                Helpers.CreatePAT();
                                if (!PoliceAndThief.policeCanSeeJewels) {
                                    foreach (PlayerControl police in PoliceAndThief.policeTeam) {
                                        if (police == PlayerInCache.LocalPlayer.PlayerControl) {
                                            foreach (GameObject jewel in PoliceAndThief.thiefTreasures) {
                                                jewel.SetActive(false);
                                            }
                                        }
                                    }
                                }
                                progress.GetComponentInChildren<TextMeshPro>().text = Language.introTexts[1] + LasMonjas.gamemodeMatchDuration.ToString("F0");
                                PoliceAndThief.thiefpointCounter = Language.introTexts[3] + "<color=#00F7FFFF>" + PoliceAndThief.currentJewelsStoled + " / " + PoliceAndThief.requiredJewels + "</color> | " + Language.introTexts[4] + "<color=#928B55FF>" + PoliceAndThief.currentThiefsCaptured + " / " + PoliceAndThief.thiefTeam.Count + "</color>";
                                new CustomMessage(PoliceAndThief.thiefpointCounter, LasMonjas.gamemodeMatchDuration, new Vector2(-2.5f, 2.35f), 15); 
                                break;
                            case 4:
                                // KOTH:
                                Helpers.CreateKOTH();
                                progress.GetComponentInChildren<TextMeshPro>().text = Language.introTexts[1] + LasMonjas.gamemodeMatchDuration.ToString("F0");
                                new CustomMessage(KingOfTheHill.kingpointCounter, LasMonjas.gamemodeMatchDuration, new Vector2(-2.5f, 2.35f), 15);
                                // Add Arrows pointing the zones
                                if (KingOfTheHill.localArrows.Count == 0 && KingOfTheHill.localArrows.Count < 4) {
                                    KingOfTheHill.localArrows.Add(new Arrow(KingOfTheHill.zoneonecolor));
                                    KingOfTheHill.localArrows.Add(new Arrow(KingOfTheHill.zonetwocolor));
                                    KingOfTheHill.localArrows.Add(new Arrow(KingOfTheHill.zonethreecolor));
                                    KingOfTheHill.localArrows.Add(new Arrow(Color.cyan));
                                    KingOfTheHill.localArrows.Add(new Arrow(Color.cyan));
                                    KingOfTheHill.localArrows.Add(new Arrow(Color.cyan));
                                }
                                KingOfTheHill.localArrows[0].arrow.SetActive(true);
                                KingOfTheHill.localArrows[1].arrow.SetActive(true);
                                KingOfTheHill.localArrows[2].arrow.SetActive(true);
                                if (PlayerInCache.LocalPlayer.PlayerControl == KingOfTheHill.greenKingplayer || PlayerInCache.LocalPlayer.PlayerControl == KingOfTheHill.yellowKingplayer) {
                                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                                }
                                else {
                                    KingOfTheHill.localArrows[3].arrow.SetActive(true);
                                }
                                if (KingOfTheHill.usurperPlayer != null && PlayerInCache.LocalPlayer.PlayerControl == KingOfTheHill.usurperPlayer) {
                                    KingOfTheHill.localArrows[3].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[4].arrow.SetActive(true);
                                    KingOfTheHill.localArrows[5].arrow.SetActive(true);
                                }
                                else {
                                    KingOfTheHill.localArrows[4].arrow.SetActive(false);
                                    KingOfTheHill.localArrows[5].arrow.SetActive(false);
                                }
                                break;
                            case 5:
                                // HP:
                                Helpers.CreateHP();                                
                                progress.GetComponentInChildren<TextMeshPro>().text = "<color=#FF8000FF>" + Language.introTexts[1] + "</color>" + gamemodeMatchDuration.ToString("F0") + " | <color=#FF8000FF>" + Language.introTexts[5] + "</color>" + HotPotato.timeforTransfer.ToString("F0");
                                HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + HotPotato.notPotatoTeam.Count + "</color>";
                                new CustomMessage(HotPotato.hotpotatopointCounter, LasMonjas.gamemodeMatchDuration, new Vector2(-2.5f, 2.35f), 15);
                                break;
                            case 6:
                                // ZL:
                                Helpers.CreateZL();
                                // Spawn key items
                                GameObject keyitem01 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                keyitem01.transform.position = ZombieLaboratory.susBoxPositions[0];
                                keyitem01.name = "keyItem01";
                                ZombieLaboratory.laboratoryKeyItem01 = keyitem01;
                                GameObject keyitem02 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                keyitem02.transform.position = ZombieLaboratory.susBoxPositions[1];
                                keyitem02.name = "keyItem02";
                                ZombieLaboratory.laboratoryKeyItem02 = keyitem02;
                                GameObject keyitem03 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                keyitem03.transform.position = ZombieLaboratory.susBoxPositions[2];
                                keyitem03.name = "keyItem03";
                                ZombieLaboratory.laboratoryKeyItem03 = keyitem03;
                                GameObject keyitem04 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                keyitem04.transform.position = ZombieLaboratory.susBoxPositions[3];
                                keyitem04.name = "keyItem04";
                                ZombieLaboratory.laboratoryKeyItem04 = keyitem04;
                                GameObject keyitem05 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                keyitem05.transform.position = ZombieLaboratory.susBoxPositions[4];
                                keyitem05.name = "keyItem05";
                                ZombieLaboratory.laboratoryKeyItem05 = keyitem05;
                                GameObject keyitem06 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                keyitem06.transform.position = ZombieLaboratory.susBoxPositions[5];
                                keyitem06.name = "keyItem06";
                                ZombieLaboratory.laboratoryKeyItem06 = keyitem06;
                                // Ammoboxes
                                GameObject ammoBox01 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                ammoBox01.transform.position = ZombieLaboratory.susBoxPositions[6];
                                ammoBox01.name = "ammoBox";
                                GameObject ammoBox02 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                ammoBox02.transform.position = ZombieLaboratory.susBoxPositions[7];
                                ammoBox02.name = "ammoBox";
                                GameObject ammoBox03 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                ammoBox03.transform.position = ZombieLaboratory.susBoxPositions[8];
                                ammoBox03.name = "ammoBox";
                                GameObject ammoBox04 = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                ammoBox04.transform.position = ZombieLaboratory.susBoxPositions[9];
                                ammoBox04.name = "ammoBox";
                                ZombieLaboratory.groundItems.Add(keyitem01);
                                ZombieLaboratory.groundItems.Add(keyitem02);
                                ZombieLaboratory.groundItems.Add(keyitem03);
                                ZombieLaboratory.groundItems.Add(keyitem04);
                                ZombieLaboratory.groundItems.Add(keyitem05);
                                ZombieLaboratory.groundItems.Add(keyitem06);
                                ZombieLaboratory.groundItems.Add(ammoBox01);
                                ZombieLaboratory.groundItems.Add(ammoBox02);
                                ZombieLaboratory.groundItems.Add(ammoBox03);
                                ZombieLaboratory.groundItems.Add(ammoBox04);
                                // Nothing boxes
                                for (int i = 0; i < ZombieLaboratory.susBoxPositions.Count - 10; i++) {
                                    GameObject nothingBox = GameObject.Instantiate(CustomMain.customAssets.susBox, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                    nothingBox.transform.position = ZombieLaboratory.susBoxPositions[i + 10];
                                    nothingBox.name = "nothingBox";
                                    ZombieLaboratory.groundItems.Add(nothingBox);
                                }
                                progress.GetComponentInChildren<TextMeshPro>().text = Language.introTexts[1] + LasMonjas.gamemodeMatchDuration.ToString("F0");
                                ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                                new CustomMessage(ZombieLaboratory.zombieLaboratoryCounter, LasMonjas.gamemodeMatchDuration, new Vector2(-2.5f, 2.35f), 15); 
                                break;
                            case 7:
                                // BR:
                                Helpers.CreateBR();
                                progress.GetComponentInChildren<TextMeshPro>().text = Language.introTexts[1] + LasMonjas.gamemodeMatchDuration.ToString("F0");
                                switch (BattleRoyale.matchType) {
                                    case 0:
                                        BattleRoyale.battleRoyalepointCounter = Language.introTexts[11] + "<color=#009F57FF>" + BattleRoyale.soloPlayerTeam.Count + "</color>";
                                        break;
                                    case 1:
                                        if (BattleRoyale.serialKiller != null) {
                                            BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + BattleRoyale.limeTeam.Count + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + BattleRoyale.pinkTeam.Count + "</color> | " + Language.introTexts[14] + "<color=#808080FF>" + BattleRoyale.serialKillerTeam.Count + "</color>";
                                        }
                                        else {
                                            BattleRoyale.battleRoyalepointCounter = Language.introTexts[12] + "<color=#39FF14FF>" + BattleRoyale.limeTeam.Count + "</color> | " + Language.introTexts[13] + "<color=#F2BEFFFF>" + BattleRoyale.pinkTeam.Count + "</color>";
                                        }
                                        break;
                                    case 2:
                                        if (BattleRoyale.serialKiller != null) {
                                            BattleRoyale.battleRoyalepointCounter = Language.introTexts[15] + BattleRoyale.requiredScore + " | <color=#39FF14FF>" + Language.introTexts[12] + BattleRoyale.limePoints + "</color> | " + "<color=#F2BEFFFF>" + Language.introTexts[13] + BattleRoyale.pinkPoints + "</color> | " + "<color=#808080FF>" + Language.introTexts[16] + BattleRoyale.serialKillerPoints + "</color>";
                                        }
                                        else {
                                            BattleRoyale.battleRoyalepointCounter = Language.introTexts[15] + BattleRoyale.requiredScore + " | <color=#39FF14FF>" + Language.introTexts[12] + BattleRoyale.limePoints + "</color> | " + "<color=#F2BEFFFF>" + Language.introTexts[13] + BattleRoyale.pinkPoints + "</color>";
                                        }
                                        break;
                                }
                                new CustomMessage(BattleRoyale.battleRoyalepointCounter, LasMonjas.gamemodeMatchDuration, new Vector2(-2.5f, 2.35f), 15);
                                break;
                            case 8:
                                // MF:
                                Helpers.CreateMF();
                                progress.GetComponentInChildren<TextMeshPro>().text = Language.introTexts[1] + LasMonjas.gamemodeMatchDuration.ToString("F0");
                                if (MonjaFestival.bigMonjaPlayer != null) {
                                    MonjaFestival.monjaFestivalCounter = "<color=#00FF00FF>" + Language.introTexts[17] + MonjaFestival.greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + MonjaFestival.cyanPoints + "</color> | " + "<color=#808080FF>" + Language.introTexts[19] + MonjaFestival.bigMonjaPoints + "</color>";
                                }
                                else {
                                    MonjaFestival.monjaFestivalCounter = "<color=#00FF00FF>" + Language.introTexts[17] + MonjaFestival.greenPoints + "</color> | " + "<color=#00F7FFFF>" + Language.introTexts[18] + MonjaFestival.cyanPoints + "</color>";
                                }
                                new CustomMessage(MonjaFestival.monjaFestivalCounter, LasMonjas.gamemodeMatchDuration, new Vector2(-2.5f, 2.35f), 15);
                                if (MonjaFestival.localArrows.Count == 0) {
                                    MonjaFestival.localArrows.Add(new Arrow(Color.green));
                                    MonjaFestival.localArrows.Add(new Arrow(Color.cyan));
                                    MonjaFestival.localArrows.Add(new Arrow(Color.grey));
                                    MonjaFestival.localArrows.Add(new Arrow(Color.grey));
                                    MonjaFestival.localArrows[0].arrow.SetActive(false);
                                    MonjaFestival.localArrows[1].arrow.SetActive(false);
                                    MonjaFestival.localArrows[2].arrow.SetActive(false);
                                    MonjaFestival.localArrows[3].arrow.SetActive(false);

                                    if (PlayerInCache.LocalPlayer.PlayerControl == MonjaFestival.bigMonjaPlayer) {
                                        MonjaFestival.localArrows[2].arrow.SetActive(true);
                                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                                            MonjaFestival.localArrows[3].arrow.SetActive(true);
                                        }
                                    }

                                    foreach (PlayerControl player in MonjaFestival.greenTeam) {
                                        if (player == PlayerInCache.LocalPlayer.PlayerControl)
                                            MonjaFestival.localArrows[0].arrow.SetActive(true);
                                    }
                                    foreach (PlayerControl player in MonjaFestival.cyanTeam) {
                                        if (player == PlayerInCache.LocalPlayer.PlayerControl)
                                            MonjaFestival.localArrows[1].arrow.SetActive(true);
                                    }
                                }
                                foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                                    if (player != null) {
                                        GameObject hands = GameObject.Instantiate(CustomMain.customAssets.pickOneGreenMonja, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                                        hands.GetComponent<SpriteRenderer>().sprite = null;
                                        hands.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.1f, -0.5f);
                                        hands.transform.parent = player.transform;
                                        hands.name = "hands" + player.name;
                                    }
                                }
                                if (MonjaFestival.greenPlayer01 != null) {
                                    MonjaFestival.handsGreen01 = GameObject.Find("hands" + MonjaFestival.greenPlayer01.name);
                                }
                                if (MonjaFestival.greenPlayer02 != null) {
                                    MonjaFestival.handsGreen02 = GameObject.Find("hands" + MonjaFestival.greenPlayer02.name);
                                }
                                if (MonjaFestival.greenPlayer03 != null) {
                                    MonjaFestival.handsGreen03 = GameObject.Find("hands" + MonjaFestival.greenPlayer03.name);
                                }
                                if (MonjaFestival.greenPlayer04 != null) {
                                    MonjaFestival.handsGreen04 = GameObject.Find("hands" + MonjaFestival.greenPlayer04.name);
                                }
                                if (MonjaFestival.greenPlayer05 != null) {
                                    MonjaFestival.handsGreen05 = GameObject.Find("hands" + MonjaFestival.greenPlayer05.name);
                                }
                                if (MonjaFestival.greenPlayer06 != null) {
                                    MonjaFestival.handsGreen06 = GameObject.Find("hands" + MonjaFestival.greenPlayer06.name);
                                }
                                if (MonjaFestival.greenPlayer07 != null) {
                                    MonjaFestival.handsGreen07 = GameObject.Find("hands" + MonjaFestival.greenPlayer07.name);
                                }
                                if (MonjaFestival.cyanPlayer01 != null) {
                                    MonjaFestival.handsCyan01 = GameObject.Find("hands" + MonjaFestival.cyanPlayer01.name);
                                }
                                if (MonjaFestival.cyanPlayer02 != null) {
                                    MonjaFestival.handsCyan02 = GameObject.Find("hands" + MonjaFestival.cyanPlayer02.name);
                                }
                                if (MonjaFestival.cyanPlayer03 != null) {
                                    MonjaFestival.handsCyan03 = GameObject.Find("hands" + MonjaFestival.cyanPlayer03.name);
                                }
                                if (MonjaFestival.cyanPlayer04 != null) {
                                    MonjaFestival.handsCyan04 = GameObject.Find("hands" + MonjaFestival.cyanPlayer04.name);
                                }
                                if (MonjaFestival.cyanPlayer05 != null) {
                                    MonjaFestival.handsCyan05 = GameObject.Find("hands" + MonjaFestival.cyanPlayer05.name);
                                }
                                if (MonjaFestival.cyanPlayer06 != null) {
                                    MonjaFestival.handsCyan06 = GameObject.Find("hands" + MonjaFestival.cyanPlayer06.name);
                                }
                                if (MonjaFestival.cyanPlayer07 != null) {
                                    MonjaFestival.handsCyan07 = GameObject.Find("hands" + MonjaFestival.cyanPlayer07.name);
                                }
                                break;
                        }
                        Helpers.RemoveObjectsOnGamemodes(GameOptionsManager.Instance.currentGameOptions.MapId);
                    }                    
                }
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch
        {
            public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                setupIntroTeamIcons(__instance, ref teamToDisplay);
            }

            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay) {
                setupIntroTeam(__instance, ref teamToDisplay);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch
        {
            public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                setupIntroTeamIcons(__instance, ref yourTeam);
            }

            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam) {
                setupIntroTeam(__instance, ref yourTeam);
            }
        }

        public static void clearSwipeCardTask() {

            bool removeSwipeCard = CustomOptionHolder.removeSwipeCard.getBool();

            if (removeSwipeCard && removedSwipe == false && GameOptionsManager.Instance.currentGameOptions.MapId != 1 && GameOptionsManager.Instance.currentGameOptions.MapId != 4) {
                foreach (PlayerControl myplayer in PlayerInCache.AllPlayers) {
                    if (myplayer != Joker.joker && myplayer != RoleThief.rolethief && myplayer != Pyromaniac.pyromaniac && myplayer != TreasureHunter.treasureHunter && myplayer != Devourer.devourer && myplayer != Poisoner.poisoner && myplayer != Puppeteer.puppeteer && myplayer != Exiler.exiler && myplayer != Amnesiac.amnesiac && myplayer != Seeker.seeker && myplayer != Renegade.renegade && myplayer != Minion.minion && myplayer != BountyHunter.bountyhunter && myplayer != Trapper.trapper && myplayer != Yinyanger.yinyanger && myplayer != Challenger.challenger && myplayer != Ninja.ninja && myplayer != Berserker.berserker && myplayer != Yandere.yandere && myplayer != Stranded.stranded && myplayer != Monja.monja && !myplayer.Data.Role.IsImpostor) {
                        var toRemove = new List<PlayerTask>();
                        foreach (PlayerTask task in myplayer.myTasks)
                            if (task.TaskType == TaskTypes.SwipeCard)
                                toRemove.Add(task);
                        foreach (PlayerTask task in toRemove)
                            if (task.TaskType == TaskTypes.SwipeCard)
                                myplayer.RemoveTask(task);
                    }
                }
                removedSwipe = true;
            }
        }

        /*public static void removeAirshipDoors() {

            bool removeAirshipDoors = CustomOptionHolder.removeAirshipDoors.getBool();

            if (removeAirshipDoors && removedAirshipDoors == false && GameOptionsManager.Instance.currentGameOptions.MapId == 4) {
                List<GameObject> doors = new List<GameObject>();

                GameObject celldoor01 = GameObject.Find("doorsideOpen (2)");
                doors.Add(celldoor01);
                GameObject celldoor02 = GameObject.Find("door_vault");
                doors.Add(celldoor02);
                GameObject celldoor04 = GameObject.Find("door_gap");
                doors.Add(celldoor04);

                GameObject bighallwaydoor01 = GameObject.Find("Door_VertOpen");
                doors.Add(bighallwaydoor01);
                GameObject bighallwaydoor02 = GameObject.Find("Door_VertOpen (4)");
                doors.Add(bighallwaydoor02);
                GameObject bighallwaydoor03 = GameObject.Find("Door_HortOpen");
                doors.Add(bighallwaydoor03);
                GameObject bighallwaydoor04 = GameObject.Find("Door_HortOpen (1)");
                doors.Add(bighallwaydoor04);

                GameObject kitchendoor01 = GameObject.Find("Door_VertOpen (1)");
                doors.Add(kitchendoor01);
                GameObject kitchendoor02 = GameObject.Find("Door_VertOpen (2)");
                doors.Add(kitchendoor02);
                GameObject kitchendoor03 = GameObject.Find("Door_VertOpen (3)");
                doors.Add(kitchendoor03);

                GameObject medbeydoor01 = GameObject.Find("Door_VertOpen (10)");
                doors.Add(medbeydoor01);
                GameObject medbeydoor02 = GameObject.Find("Door_HortOpen (3)");
                doors.Add(medbeydoor02);

                GameObject recorddoor01 = GameObject.Find("Door_VertOpen (11)");
                doors.Add(recorddoor01);
                GameObject recorddoor02 = GameObject.Find("Door_VertOpen (12)");
                doors.Add(recorddoor02);
                GameObject recorddoor03 = GameObject.Find("Door_HortOpen (2)");
                doors.Add(recorddoor03);

                GameObject hallway01 = GameObject.Find("Door_VertOpen (5)");
                doors.Add(hallway01);
                GameObject hallway02 = GameObject.Find("Door_VertOpen (6)");
                doors.Add(hallway02);

                foreach (GameObject door in doors) {
                    door.GetComponent<BoxCollider2D>().enabled = false;
                    door.GetComponent<SpriteRenderer>().enabled = false;
                }

                removedAirshipDoors = true;
            }
        }*/
    }
}