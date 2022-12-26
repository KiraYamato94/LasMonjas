using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using Il2CppInterop;
using UnityEngine;
using System;
using static LasMonjas.LasMonjas;
using AmongUs.GameOptions;

namespace LasMonjas.Patches
{
    [HarmonyPatch(typeof(RoleOptionsData), nameof(RoleOptionsData.GetNumPerGame))]
    class RoleOptionsDataGetNumPerGamePatch
    {
        public static void Postfix(ref int __result) {
            if (CustomOptionHolder.activateRoles.getBool()) __result = 0; // Deactivate Vanilla Roles if the mod roles are active
        }
    }

    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    class RoleManagerSelectRolesPatch
    {

        private static List<int> myGamemodeList = new List<int>();

        public static void Postfix() {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ResetVaribles, Hazel.SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.resetVariables();

            if (CustomOptionHolder.activateRoles.getBool() && GameOptionsManager.Instance.currentGameMode == GameModes.Normal) // Don't assign Roles if deactivated
                getRoleAssignmentData();
        }

        private static void getRoleAssignmentData() {
            // Get 3 player lists, one for crewmates/neutrals/rebels, one for impostor and a global one for gamemodes.
            List<PlayerControl> crewmates = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            crewmates.RemoveAll(x => x.Data.Role.IsImpostor);
            List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            impostors.RemoveAll(x => !x.Data.Role.IsImpostor);
            List<PlayerControl> modifiers = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();

            if (CaptureTheFlag.captureTheFlagMode) {
                howmanygamemodesareon += 1;
            }
            if (PoliceAndThief.policeAndThiefMode) {
                howmanygamemodesareon += 1;
            }
            if (KingOfTheHill.kingOfTheHillMode) {
                howmanygamemodesareon += 1;
            }
            if (HotPotato.hotPotatoMode) {
                howmanygamemodesareon += 1;
            }
            if (ZombieLaboratory.zombieLaboratoryMode) {
                howmanygamemodesareon += 1;
            }
            if (BattleRoyale.battleRoyaleMode) {
                howmanygamemodesareon += 1;
            }

            // Assign roles only if the game won't be a custom gamemode
            if (howmanygamemodesareon != 1) {

                if (!whoAmIMode) {

                    int crewmateMax = 15;
                    int neutralMax = 1;
                    int impostorMax = 3;
                    int rebelMax = 1;

                    // Fill in the lists with the roles that are active in the settings
                    Dictionary<byte, int> impSettings = new Dictionary<byte, int>();
                    Dictionary<byte, int> rebelSettings = new Dictionary<byte, int>();
                    Dictionary<byte, int> neutralSettings = new Dictionary<byte, int>();
                    Dictionary<byte, int> crewSettings = new Dictionary<byte, int>();

                    impSettings.Add((byte)RoleId.Mimic, CustomOptionHolder.mimicSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Painter, CustomOptionHolder.painterSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Demon, CustomOptionHolder.demonSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Illusionist, CustomOptionHolder.illusionistSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Janitor, CustomOptionHolder.janitorSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Manipulator, CustomOptionHolder.manipulatorSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Bomberman, CustomOptionHolder.bombermanSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Chameleon, CustomOptionHolder.chameleonSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Gambler, CustomOptionHolder.gamblerSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Sorcerer, CustomOptionHolder.sorcererSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Medusa, CustomOptionHolder.medusaSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Hypnotist, CustomOptionHolder.hypnotistSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Archer, CustomOptionHolder.archerSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Plumber, CustomOptionHolder.plumberSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Librarian, CustomOptionHolder.librarianSpawnRate.getSelection());

                    rebelSettings.Add((byte)RoleId.Renegade, CustomOptionHolder.renegadeSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.BountyHunter, CustomOptionHolder.bountyHunterSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Trapper, CustomOptionHolder.trapperSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Yinyanger, CustomOptionHolder.yinyangerSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Challenger, CustomOptionHolder.challengerSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Ninja, CustomOptionHolder.ninjaSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Berserker, CustomOptionHolder.berserkerSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Yandere, CustomOptionHolder.yandereSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Stranded, CustomOptionHolder.strandedSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Monja, CustomOptionHolder.monjaSpawnRate.getSelection());

                    neutralSettings.Add((byte)RoleId.Joker, CustomOptionHolder.jokerSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Pyromaniac, CustomOptionHolder.pyromaniacSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.RoleThief, CustomOptionHolder.rolethiefSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.TreasureHunter, CustomOptionHolder.treasureHunterSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Devourer, CustomOptionHolder.devourerSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Poisoner, CustomOptionHolder.poisonerSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Puppeteer, CustomOptionHolder.puppeteerSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Exiler, CustomOptionHolder.exilerSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Amnesiac, CustomOptionHolder.amnesiacSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Seeker, CustomOptionHolder.seekerSpawnRate.getSelection());

                    crewSettings.Add((byte)RoleId.Captain, CustomOptionHolder.captainSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Mechanic, CustomOptionHolder.mechanicSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Sheriff, CustomOptionHolder.sheriffSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Detective, CustomOptionHolder.detectiveSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Forensic, CustomOptionHolder.forensicSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.TimeTraveler, CustomOptionHolder.timeTravelerSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Squire, CustomOptionHolder.squireSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Cheater, CustomOptionHolder.cheaterSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.FortuneTeller, CustomOptionHolder.fortuneTellerSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Hacker, CustomOptionHolder.hackerSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Sleuth, CustomOptionHolder.sleuthSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Fink, CustomOptionHolder.finkSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Kid, CustomOptionHolder.kidSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Welder, CustomOptionHolder.welderSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Spiritualist, CustomOptionHolder.spiritualistSpawnRate.getSelection());
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 1) {
                        crewSettings.Add((byte)RoleId.Vigilant, CustomOptionHolder.vigilantSpawnRate.getSelection());
                    }
                    else {
                        crewSettings.Add((byte)RoleId.VigilantMira, CustomOptionHolder.vigilantSpawnRate.getSelection());
                    }
                    crewSettings.Add((byte)RoleId.Hunter, CustomOptionHolder.hunterSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Jinx, CustomOptionHolder.jinxSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Coward, CustomOptionHolder.cowardSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Bat, CustomOptionHolder.batSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Necromancer, CustomOptionHolder.necromancerSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Engineer, CustomOptionHolder.engineerSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Shy, CustomOptionHolder.shySpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.TaskMaster, CustomOptionHolder.taskMasterSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Jailer, CustomOptionHolder.jailerSpawnRate.getSelection());

                    // Get all roles where the chance to occur is set to 100%
                    List<byte> ensuredImpostorRoles = impSettings.Where(x => x.Value == 1).Select(x => x.Key).ToList();
                    List<byte> ensuredRebelRoles = rebelSettings.Where(x => x.Value == 1).Select(x => x.Key).ToList();
                    List<byte> ensuredNeutralRoles = neutralSettings.Where(x => x.Value == 1).Select(x => x.Key).ToList();
                    List<byte> ensuredCrewmateRoles = crewSettings.Where(x => x.Value == 1).Select(x => x.Key).ToList();

                    // Assign roles until we run out of either players we can assign roles to or run out of roles we can assign to players
                    while (
                        (impostors.Count > 0 && impostorMax > 0 && ensuredImpostorRoles.Count > 0) ||
                        (crewmates.Count > 0 && (
                            (rebelMax > 0 && ensuredRebelRoles.Count > 0) ||
                            (neutralMax > 0 && ensuredNeutralRoles.Count > 0) ||
                            (crewmateMax > 0 && ensuredCrewmateRoles.Count > 0)
                        ))) {

                        Dictionary<RoleType, List<byte>> rolesToAssign = new Dictionary<RoleType, List<byte>>();
                        if (impostors.Count > 0 && impostorMax > 0 && ensuredImpostorRoles.Count > 0) rolesToAssign.Add(RoleType.Impostor, ensuredImpostorRoles);
                        if (crewmates.Count > 0 && rebelMax > 0 && ensuredRebelRoles.Count > 0) rolesToAssign.Add(RoleType.Rebel, ensuredRebelRoles);
                        if (crewmates.Count > 0 && neutralMax > 0 && ensuredNeutralRoles.Count > 0) rolesToAssign.Add(RoleType.Neutral, ensuredNeutralRoles);
                        if (crewmates.Count > 0 && crewmateMax > 0 && ensuredCrewmateRoles.Count > 0) rolesToAssign.Add(RoleType.Crewmate, ensuredCrewmateRoles);

                        // Randomly select a pool of roles to assign a role from (Crewmate role, Neutral role, Impostor role or Rebel role) then select one of the roles from the selected pool to a player and remove the rol from the pool
                        var roleType = rolesToAssign.Keys.ElementAt(rnd.Next(0, rolesToAssign.Keys.Count()));
                        var players = roleType == RoleType.Crewmate || roleType == RoleType.Neutral || roleType == RoleType.Rebel ? crewmates : impostors;
                        var index = rnd.Next(0, rolesToAssign[roleType].Count);
                        var roleId = rolesToAssign[roleType][index];
                        setRoleToRandomPlayer(rolesToAssign[roleType][index], players);
                        rolesToAssign[roleType].RemoveAt(index);

                        // Adjust the role limit
                        switch (roleType) {
                            case RoleType.Impostor: impostorMax--; break;
                            case RoleType.Rebel: rebelMax--; break;
                            case RoleType.Neutral: neutralMax--; break;
                            case RoleType.Crewmate: crewmateMax--; break;
                        }
                    }
                }
                // Add modifiers after selecting the roles
                if (CustomOptionHolder.activateModifiers.getSelection() == 1) {
                    assignModifiers();
                }               
            }
            else {
                if (CaptureTheFlag.captureTheFlagMode) {
                    // Capture the flag    
                    myGamemodeList.Clear();
                    bool oddNumber = false;
                    if (Mathf.Ceil(PlayerControl.AllPlayerControls.Count) % 2 != 0) {
                        oddNumber = true;
                        setRoleToRandomPlayer((byte)RoleId.StealerPlayer, modifiers);
                    }
                    int myflag = 1;
                    while (myGamemodeList.Count < (Mathf.Round(PlayerControl.AllPlayerControls.Count / 2))) {
                        switch (myflag) {
                            case 1:
                                setRoleToRandomPlayer((byte)RoleId.RedPlayer01, modifiers);
                                break;
                            case 2:
                                setRoleToRandomPlayer((byte)RoleId.RedPlayer02, modifiers);
                                break;
                            case 3:
                                setRoleToRandomPlayer((byte)RoleId.RedPlayer03, modifiers);
                                break;
                            case 4:
                                setRoleToRandomPlayer((byte)RoleId.RedPlayer04, modifiers);
                                break;
                            case 5:
                                setRoleToRandomPlayer((byte)RoleId.RedPlayer05, modifiers);
                                break;
                            case 6:
                                setRoleToRandomPlayer((byte)RoleId.RedPlayer06, modifiers);
                                break;
                            case 7:
                                setRoleToRandomPlayer((byte)RoleId.RedPlayer07, modifiers);
                                break;
                        }
                        myGamemodeList.Add(myflag);
                        myflag += 1;
                    }
                    int myblueflag = 9;
                    while (!oddNumber && myGamemodeList.Count < PlayerControl.AllPlayerControls.Count || oddNumber && myGamemodeList.Count < PlayerControl.AllPlayerControls.Count - 1) {
                        switch (myblueflag) {
                            case 9:
                                setRoleToRandomPlayer((byte)RoleId.BluePlayer01, modifiers);
                                break;
                            case 10:
                                setRoleToRandomPlayer((byte)RoleId.BluePlayer02, modifiers);
                                break;
                            case 11:
                                setRoleToRandomPlayer((byte)RoleId.BluePlayer03, modifiers);
                                break;
                            case 12:
                                setRoleToRandomPlayer((byte)RoleId.BluePlayer04, modifiers);
                                break;
                            case 13:
                                setRoleToRandomPlayer((byte)RoleId.BluePlayer05, modifiers);
                                break;
                            case 14:
                                setRoleToRandomPlayer((byte)RoleId.BluePlayer06, modifiers);
                                break;
                            case 15:
                                setRoleToRandomPlayer((byte)RoleId.BluePlayer07, modifiers);
                                break;
                        }
                        myGamemodeList.Add(myblueflag);
                        myblueflag += 1;
                    }
                }
                else if (PoliceAndThief.policeAndThiefMode) {
                    // Police and Thief    
                    myGamemodeList.Clear();
                    int mypolice = 1;
                    while (myGamemodeList.Count < (Mathf.Round(PlayerControl.AllPlayerControls.Count / 2.39f))) {
                        switch (mypolice) {
                            case 1:
                                setRoleToRandomPlayer((byte)RoleId.PolicePlayer01, modifiers);
                                break;
                            case 2:
                                setRoleToRandomPlayer((byte)RoleId.PolicePlayer03, modifiers);
                                break;
                            case 3:
                                setRoleToRandomPlayer((byte)RoleId.PolicePlayer02, modifiers);
                                break;
                            case 4:
                                setRoleToRandomPlayer((byte)RoleId.PolicePlayer05, modifiers);
                                break;
                            case 5:
                                setRoleToRandomPlayer((byte)RoleId.PolicePlayer04, modifiers);
                                break;
                            case 6:
                                setRoleToRandomPlayer((byte)RoleId.PolicePlayer06, modifiers);
                                break;
                        }
                        myGamemodeList.Add(mypolice);
                        mypolice += 1;
                    }
                    int mythief = 7;
                    while (myGamemodeList.Count < PlayerControl.AllPlayerControls.Count) {
                        switch (mythief) {
                            case 7:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer01, modifiers);
                                break;
                            case 8:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer02, modifiers);
                                break;
                            case 9:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer03, modifiers);
                                break;
                            case 10:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer04, modifiers);
                                break;
                            case 11:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer05, modifiers);
                                break;
                            case 12:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer06, modifiers);
                                break;
                            case 13:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer07, modifiers);
                                break;
                            case 14:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer08, modifiers);
                                break;
                            case 15:
                                setRoleToRandomPlayer((byte)RoleId.ThiefPlayer09, modifiers);
                                break;
                        }
                        myGamemodeList.Add(mythief);
                        mythief += 1;
                    }
                }
                else if (KingOfTheHill.kingOfTheHillMode) {
                    // King of the hill    
                    myGamemodeList.Clear();
                    bool oddNumber = false;
                    if (Mathf.Ceil(PlayerControl.AllPlayerControls.Count) % 2 != 0) {
                        oddNumber = true;
                        setRoleToRandomPlayer((byte)RoleId.UsurperPlayer, modifiers);
                    }
                    int myking = 1;
                    while (myGamemodeList.Count < (Mathf.Round(PlayerControl.AllPlayerControls.Count / 2))) {
                        switch (myking) {
                            case 1:
                                setRoleToRandomPlayer((byte)RoleId.GreenKing, modifiers);
                                break;
                            case 2:
                                setRoleToRandomPlayer((byte)RoleId.GreenPlayer01, modifiers);
                                break;
                            case 3:
                                setRoleToRandomPlayer((byte)RoleId.GreenPlayer02, modifiers);
                                break;
                            case 4:
                                setRoleToRandomPlayer((byte)RoleId.GreenPlayer03, modifiers);
                                break;
                            case 5:
                                setRoleToRandomPlayer((byte)RoleId.GreenPlayer04, modifiers);
                                break;
                            case 6:
                                setRoleToRandomPlayer((byte)RoleId.GreenPlayer05, modifiers);
                                break;
                            case 7:
                                setRoleToRandomPlayer((byte)RoleId.GreenPlayer06, modifiers);
                                break;
                        }
                        myGamemodeList.Add(myking);
                        myking += 1;
                    }
                    int myyellowking = 9;
                    while (!oddNumber && myGamemodeList.Count < PlayerControl.AllPlayerControls.Count || oddNumber && myGamemodeList.Count < PlayerControl.AllPlayerControls.Count - 1) {
                        switch (myyellowking) {
                            case 9:
                                setRoleToRandomPlayer((byte)RoleId.YellowKing, modifiers);
                                break;
                            case 10:
                                setRoleToRandomPlayer((byte)RoleId.YellowPlayer01, modifiers);
                                break;
                            case 11:
                                setRoleToRandomPlayer((byte)RoleId.YellowPlayer02, modifiers);
                                break;
                            case 12:
                                setRoleToRandomPlayer((byte)RoleId.YellowPlayer03, modifiers);
                                break;
                            case 13:
                                setRoleToRandomPlayer((byte)RoleId.YellowPlayer04, modifiers);
                                break;
                            case 14:
                                setRoleToRandomPlayer((byte)RoleId.YellowPlayer05, modifiers);
                                break;
                            case 15:
                                setRoleToRandomPlayer((byte)RoleId.YellowPlayer06, modifiers);
                                break;
                        }
                        myGamemodeList.Add(myyellowking);
                        myyellowking += 1;
                    }
                }
                else if (HotPotato.hotPotatoMode) {
                    // Hot Potato   
                    myGamemodeList.Clear();
                    int mypotato = 1;
                    while (myGamemodeList.Count < PlayerControl.AllPlayerControls.Count) {
                        switch (mypotato) {
                            case 1:
                                setRoleToRandomPlayer((byte)RoleId.HotPotato, modifiers);
                                break;
                            case 2:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato01, modifiers);
                                break;
                            case 3:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato02, modifiers);
                                break;
                            case 4:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato03, modifiers);
                                break;
                            case 5:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato04, modifiers);
                                break;
                            case 6:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato05, modifiers);
                                break;
                            case 7:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato06, modifiers);
                                break;
                            case 8:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato07, modifiers);
                                break;
                            case 9:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato08, modifiers);
                                break;
                            case 10:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato09, modifiers);
                                break;
                            case 11:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato10, modifiers);
                                break;
                            case 12:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato11, modifiers);
                                break;
                            case 13:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato12, modifiers);
                                break;
                            case 14:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato13, modifiers);
                                break;
                            case 15:
                                setRoleToRandomPlayer((byte)RoleId.NotPotato14, modifiers);
                                break;
                        }
                        myGamemodeList.Add(mypotato);
                        mypotato += 1;
                    }
                }
                else if (ZombieLaboratory.zombieLaboratoryMode) {
                    // ZombieLaboratory
                    myGamemodeList.Clear();
                    int myzombie = 1;
                    while (myGamemodeList.Count < PlayerControl.AllPlayerControls.Count) {
                        switch (ZombieLaboratory.startZombies) {
                            case 1:
                                switch (myzombie) {
                                    case 1:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer01, modifiers);
                                        break;
                                    case 2:
                                        setRoleToRandomPlayer((byte)RoleId.NursePlayer, modifiers);
                                        break;
                                    case 3:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer01, modifiers);
                                        break;
                                    case 4:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer02, modifiers);
                                        break;
                                    case 5:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer03, modifiers);
                                        break;
                                    case 6:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer04, modifiers);
                                        break;
                                    case 7:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer05, modifiers);
                                        break;
                                    case 8:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer06, modifiers);
                                        break;
                                    case 9:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer07, modifiers);
                                        break;
                                    case 10:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer08, modifiers);
                                        break;
                                    case 11:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer09, modifiers);
                                        break;
                                    case 12:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer10, modifiers);
                                        break;
                                    case 13:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer11, modifiers);
                                        break;
                                    case 14:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer12, modifiers);
                                        break;
                                    case 15:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer13, modifiers);
                                        break;
                                }
                                break;
                            case 2:
                                switch (myzombie) {
                                    case 1:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer01, modifiers);
                                        break;
                                    case 2:
                                        setRoleToRandomPlayer((byte)RoleId.NursePlayer, modifiers);
                                        break;
                                    case 3:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer01, modifiers);
                                        break;
                                    case 4:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer02, modifiers);
                                        break;
                                    case 5:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer02, modifiers);
                                        break;
                                    case 6:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer03, modifiers);
                                        break;
                                    case 7:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer04, modifiers);
                                        break;
                                    case 8:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer05, modifiers);
                                        break;
                                    case 9:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer06, modifiers);
                                        break;
                                    case 10:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer07, modifiers);
                                        break;
                                    case 11:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer08, modifiers);
                                        break;
                                    case 12:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer09, modifiers);
                                        break;
                                    case 13:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer10, modifiers);
                                        break;
                                    case 14:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer11, modifiers);
                                        break;
                                    case 15:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer12, modifiers);
                                        break;
                                }
                                break;
                            case 3:
                                switch (myzombie) {
                                    case 1:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer01, modifiers);
                                        break;
                                    case 2:
                                        setRoleToRandomPlayer((byte)RoleId.NursePlayer, modifiers);
                                        break;
                                    case 3:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer01, modifiers);
                                        break;
                                    case 4:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer02, modifiers);
                                        break;
                                    case 5:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer03, modifiers);
                                        break;
                                    case 6:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer02, modifiers);
                                        break;
                                    case 7:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer03, modifiers);
                                        break;
                                    case 8:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer04, modifiers);
                                        break;
                                    case 9:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer05, modifiers);
                                        break;
                                    case 10:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer06, modifiers);
                                        break;
                                    case 11:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer07, modifiers);
                                        break;
                                    case 12:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer08, modifiers);
                                        break;
                                    case 13:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer09, modifiers);
                                        break;
                                    case 14:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer10, modifiers);
                                        break;
                                    case 15:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer11, modifiers);
                                        break;
                                }
                                break;
                            case 4:
                                switch (myzombie) {
                                    case 1:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer01, modifiers);
                                        break;
                                    case 2:
                                        setRoleToRandomPlayer((byte)RoleId.NursePlayer, modifiers);
                                        break;
                                    case 3:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer01, modifiers);
                                        break;
                                    case 4:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer02, modifiers);
                                        break;
                                    case 5:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer03, modifiers);
                                        break;
                                    case 6:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer04, modifiers);
                                        break;
                                    case 7:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer02, modifiers);
                                        break;
                                    case 8:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer03, modifiers);
                                        break;
                                    case 9:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer04, modifiers);
                                        break;
                                    case 10:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer05, modifiers);
                                        break;
                                    case 11:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer06, modifiers);
                                        break;
                                    case 12:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer07, modifiers);
                                        break;
                                    case 13:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer08, modifiers);
                                        break;
                                    case 14:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer09, modifiers);
                                        break;
                                    case 15:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer10, modifiers);
                                        break;
                                }
                                break;
                            case 5:
                                switch (myzombie) {
                                    case 1:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer01, modifiers);
                                        break;
                                    case 2:
                                        setRoleToRandomPlayer((byte)RoleId.NursePlayer, modifiers);
                                        break;
                                    case 3:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer01, modifiers);
                                        break;
                                    case 4:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer02, modifiers);
                                        break;
                                    case 5:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer03, modifiers);
                                        break;
                                    case 6:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer04, modifiers);
                                        break;
                                    case 7:
                                        setRoleToRandomPlayer((byte)RoleId.ZombiePlayer05, modifiers);
                                        break;
                                    case 8:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer02, modifiers);
                                        break;
                                    case 9:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer03, modifiers);
                                        break;
                                    case 10:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer04, modifiers);
                                        break;
                                    case 11:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer05, modifiers);
                                        break;
                                    case 12:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer06, modifiers);
                                        break;
                                    case 13:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer07, modifiers);
                                        break;
                                    case 14:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer08, modifiers);
                                        break;
                                    case 15:
                                        setRoleToRandomPlayer((byte)RoleId.SurvivorPlayer09, modifiers);
                                        break;
                                }
                                break;
                        }
                        myGamemodeList.Add(myzombie);
                        myzombie += 1;
                    }
                }
                else if (BattleRoyale.battleRoyaleMode) {
                    // Battle Royale   
                    myGamemodeList.Clear();
                    if (BattleRoyale.matchType == 0) {
                        int myBattle = 1;
                        while (myGamemodeList.Count < PlayerControl.AllPlayerControls.Count) {
                            switch (myBattle) {
                                case 1:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer01, modifiers);
                                    break;
                                case 2:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer02, modifiers);
                                    break;
                                case 3:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer03, modifiers);
                                    break;
                                case 4:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer04, modifiers);
                                    break;
                                case 5:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer05, modifiers);
                                    break;
                                case 6:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer06, modifiers);
                                    break;
                                case 7:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer07, modifiers);
                                    break;
                                case 8:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer08, modifiers);
                                    break;
                                case 9:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer09, modifiers);
                                    break;
                                case 10:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer10, modifiers);
                                    break;
                                case 11:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer11, modifiers);
                                    break;
                                case 12:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer12, modifiers);
                                    break;
                                case 13:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer13, modifiers);
                                    break;
                                case 14:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer14, modifiers);
                                    break;
                                case 15:
                                    setRoleToRandomPlayer((byte)RoleId.SoloPlayer15, modifiers);
                                    break;
                            }
                            myGamemodeList.Add(myBattle);
                            myBattle += 1;
                        }
                    }
                    else {
                        // Battle Royale Teams   
                        myGamemodeList.Clear();
                        bool oddNumber = false;
                        if (Mathf.Ceil(PlayerControl.AllPlayerControls.Count) % 2 != 0) {
                            oddNumber = true;
                            setRoleToRandomPlayer((byte)RoleId.SerialKiller, modifiers);
                        }
                        int myBattleLime = 1;
                        while (myGamemodeList.Count < (Mathf.Round(PlayerControl.AllPlayerControls.Count / 2))) {
                            switch (myBattleLime) {
                                case 1:
                                    setRoleToRandomPlayer((byte)RoleId.LimePlayer01, modifiers);
                                    break;
                                case 2:
                                    setRoleToRandomPlayer((byte)RoleId.LimePlayer02, modifiers);
                                    break;
                                case 3:
                                    setRoleToRandomPlayer((byte)RoleId.LimePlayer03, modifiers);
                                    break;
                                case 4:
                                    setRoleToRandomPlayer((byte)RoleId.LimePlayer04, modifiers);
                                    break;
                                case 5:
                                    setRoleToRandomPlayer((byte)RoleId.LimePlayer05, modifiers);
                                    break;
                                case 6:
                                    setRoleToRandomPlayer((byte)RoleId.LimePlayer06, modifiers);
                                    break;
                                case 7:
                                    setRoleToRandomPlayer((byte)RoleId.LimePlayer07, modifiers);
                                    break;
                            }
                            myGamemodeList.Add(myBattleLime);
                            myBattleLime += 1;
                        }
                        int myBattlePink = 9;
                        while (!oddNumber && myGamemodeList.Count < PlayerControl.AllPlayerControls.Count || oddNumber && myGamemodeList.Count < PlayerControl.AllPlayerControls.Count - 1) {
                            switch (myBattlePink) {
                                case 9:
                                    setRoleToRandomPlayer((byte)RoleId.PinkPlayer01, modifiers);
                                    break;
                                case 10:
                                    setRoleToRandomPlayer((byte)RoleId.PinkPlayer02, modifiers);
                                    break;
                                case 11:
                                    setRoleToRandomPlayer((byte)RoleId.PinkPlayer03, modifiers);
                                    break;
                                case 12:
                                    setRoleToRandomPlayer((byte)RoleId.PinkPlayer04, modifiers);
                                    break;
                                case 13:
                                    setRoleToRandomPlayer((byte)RoleId.PinkPlayer05, modifiers);
                                    break;
                                case 14:
                                    setRoleToRandomPlayer((byte)RoleId.PinkPlayer06, modifiers);
                                    break;
                                case 15:
                                    setRoleToRandomPlayer((byte)RoleId.PinkPlayer07, modifiers);
                                    break;
                            }
                            myGamemodeList.Add(myBattlePink);
                            myBattlePink += 1;
                        }
                    }
                }
            }
        }

        private static void assignModifiers() {
            var modifierMax = 8;
            int modifierCountSettings = modifierMax;
            List<PlayerControl> players = PlayerControl.AllPlayerControls.ToArray().ToList();
            int modifierCount = Mathf.Min(players.Count, modifierCountSettings);

            if (modifierCount == 0) return;

            List<RoleId> allModifiers = new List<RoleId>();
            List<RoleId> ensuredModifiers = new List<RoleId>();
            allModifiers.AddRange(new List<RoleId> {
                RoleId.Lighter,
                RoleId.Blind,
                RoleId.Flash,
                RoleId.BigChungus,
                RoleId.TheChosenOne,
                RoleId.Performer,
                RoleId.Pro,
                RoleId.Paintball,
                RoleId.Electrician
            });

            if (Kid.kid == null && rnd.Next(1, 2) <= CustomOptionHolder.loverPlayer.getSelection()) { // Assign lover
                bool isEvilLover = rnd.Next(1, 101) <= 50;
                byte firstLoverId;
                List<PlayerControl> impPlayer = new List<PlayerControl>(players);
                List<PlayerControl> crewPlayer = new List<PlayerControl>(players);
                impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
                crewPlayer.RemoveAll(x => x.Data.Role.IsImpostor);

                if (isEvilLover) firstLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, impPlayer);
                else firstLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, crewPlayer);
                byte secondLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, crewPlayer, 1);

                players.RemoveAll(x => x.PlayerId == firstLoverId || x.PlayerId == secondLoverId);
                modifierCount--;
            }

            foreach (RoleId m in allModifiers) {
                if (getSelectionForRoleId(m) == 1) ensuredModifiers.AddRange(Enumerable.Repeat(m, getSelectionForRoleId(m) / 1));
            }

            assignModifiersToPlayers(ensuredModifiers, players, modifierCount); // Assign ensured modifier

            modifierCount -= ensuredModifiers.Count;
            if (modifierCount <= 0) return;            
        }
        
        private static byte setRoleToRandomPlayer(byte roleId, List<PlayerControl> playerList, bool removePlayer = true) {
            var index = rnd.Next(0, playerList.Count);
            byte playerId = playerList[index].PlayerId;
            if (removePlayer) playerList.RemoveAt(index);

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetRole, Hazel.SendOption.Reliable, -1);
            writer.Write(roleId);
            writer.Write(playerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.setRole(roleId, playerId);
            return playerId;
        }

        private static int getSelectionForRoleId(RoleId roleId) {
            int selection = 0;
            switch (roleId) {
                case RoleId.Lover:
                    selection = CustomOptionHolder.loverPlayer.getSelection(); 
                    break;
                case RoleId.Lighter:
                    selection = CustomOptionHolder.lighterPlayer.getSelection(); 
                    break;
                case RoleId.Blind:
                    selection = CustomOptionHolder.blindPlayer.getSelection(); 
                    break;
                case RoleId.Flash:
                    selection = CustomOptionHolder.flashPlayer.getSelection();
                    break;
                case RoleId.BigChungus:
                    selection = CustomOptionHolder.bigchungusPlayer.getSelection();
                    break;
                case RoleId.TheChosenOne:
                    selection = CustomOptionHolder.theChosenOnePlayer.getSelection();
                    break;
                case RoleId.Performer:
                    selection = CustomOptionHolder.performerPlayer.getSelection();
                    break;
                case RoleId.Pro:
                    selection = CustomOptionHolder.proPlayer.getSelection();
                    break;
                case RoleId.Paintball:
                    selection = CustomOptionHolder.paintballPlayer.getSelection();
                    break;
                case RoleId.Electrician:
                    selection = CustomOptionHolder.electricianPlayer.getSelection();
                    break;
            }
            return selection;
        }

        private static void assignModifiersToPlayers(List<RoleId> modifiers, List<PlayerControl> playerList, int modifierCount) {
            modifiers = modifiers.OrderBy(x => rnd.Next()).ToList(); // randomize list

            while (modifierCount < modifiers.Count) {
                var index = rnd.Next(0, modifiers.Count);
                modifiers.RemoveAt(index);
            }

            byte playerId;

            foreach (RoleId modifier in modifiers) {
                if (playerList.Count == 0) break;
                playerId = setModifierToRandomPlayer((byte)modifier, playerList);
                playerList.RemoveAll(x => x.PlayerId == playerId);
            }
        }
        
        private static byte setModifierToRandomPlayer(byte modifierId, List<PlayerControl> playerList, byte flag = 0) {
            var index = rnd.Next(0, playerList.Count);
            byte playerId = playerList[index].PlayerId;
            playerList.RemoveAt(index);

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetModifier, Hazel.SendOption.Reliable, -1);
            writer.Write(modifierId);
            writer.Write(playerId);
            writer.Write(flag);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.setModifier(modifierId, playerId, flag);
            return playerId;
        }

        private enum RoleType
        {
            Crewmate = 0,
            Neutral = 1,
            Impostor = 2,
            Rebel = 3
        }
    }
}