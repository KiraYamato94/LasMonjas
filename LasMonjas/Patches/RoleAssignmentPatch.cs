using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using System;
using static LasMonjas.LasMonjas;

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
        public static List<int> myroles = new List<int>();

        private static List<int> myimpostorroles = new List<int>();

        private static List<int> mymodifiers = new List<int>();

        private static List<int> myCapturetheflag = new List<int>();
        private static List<int> myPoliceandthief = new List<int>();
        private static List<int> myKingoftheHill = new List<int>();
        private static List<int> myHotPotato = new List<int>();
        private static List<int> myZombie = new List<int>();

        public static void Postfix() {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ResetVaribles, Hazel.SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.resetVariables();

            if (!DestroyableSingleton<TutorialManager>.InstanceExists && CustomOptionHolder.activateRoles.getBool()) // Don't assign Roles in Tutorial or if deactivated
                getRoleAssignmentData();
        }

        private static void getRoleAssignmentData() {
            // Get 3 player lists, one for crewmates/neutrals/rebels, one for impostor and a global one for modifiers.
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

            // Assign roles only if the game won't be a custom gamemode
            if (howmanygamemodesareon != 1) {

                // If randomRoles setting isn't random, assign roles by list order
                if (CustomOptionHolder.randomRoles.getSelection() != 0) {

                    // Impostors role asignment by list order
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.mimicSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Mimic, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.painterSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Painter, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.demonSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Demon, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.janitorSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Janitor, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.ilusionistSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Ilusionist, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.manipulatorSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Manipulator, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.bombermanSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Bomberman, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.chameleonSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Chameleon, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.gamblerSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Gambler, impostors);
                    if (impostors.Count >= 1 && (rnd.Next(1, 2) <= CustomOptionHolder.sorcererSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Sorcerer, impostors);
                    // Cewmate Role asignment by list order                               
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.captainSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Captain, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.mechanicSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Mechanic, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.sheriffSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Sheriff, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.detectiveSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Detective, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.forensicSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Forensic, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.timeTravelerSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.TimeTraveler, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.squireSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Squire, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.cheaterSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Cheater, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.fortuneTellerSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.FortuneTeller, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.hackerSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Hacker, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.sleuthSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Sleuth, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.finkSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Fink, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.kidSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Kid, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.welderSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Welder, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.spiritualistSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Spiritualist, crewmates);

                    if (PlayerControl.GameOptions.MapId != 1) {
                        if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.vigilantSpawnRate.getSelection()))
                            setRoleToRandomPlayer((byte)RoleId.Vigilant, crewmates);
                    }
                    else {
                        if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.vigilantSpawnRate.getSelection()))
                            setRoleToRandomPlayer((byte)RoleId.VigilantMira, crewmates);
                    }

                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.hunterSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.Hunter, crewmates);
                    }
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.jinxSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Jinx, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.cowardSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Coward, crewmates);
                    if (crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.medusaSpawnRate.getSelection()))
                        setRoleToRandomPlayer((byte)RoleId.Medusa, crewmates);

                    // Even assing by list order, limit rebel and neutrals roles to only 1 per game
                    int rebelds = 1;
                    if (rebelds > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.renegadeSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.Renegade, crewmates);
                        rebelds -= 1;
                    }
                    if (rebelds > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.bountyHunterSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.BountyHunter, crewmates);
                        rebelds -= 1;
                    }
                    if (rebelds > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.trapperSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.Trapper, crewmates);
                        rebelds -= 1;
                    }
                    if (rebelds > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.yinyangerSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.Yinyanger, crewmates);
                        rebelds -= 1;
                    }
                    if (rebelds > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.challengerSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.Challenger, crewmates);
                        rebelds -= 1;
                    }
                    int neutrals = 1;
                    if (neutrals > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.jokerSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.Joker, crewmates);
                        neutrals -= 1;
                    }
                    if (neutrals > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.rolethiefSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.RoleThief, crewmates);
                        neutrals -= 1;
                    }
                    if (neutrals > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.pyromaniacSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.Pyromaniac, crewmates);
                        neutrals -= 1;
                    }
                    if (neutrals > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.treasureHunterSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.TreasureHunter, crewmates);
                        neutrals -= 1;
                    }
                    if (neutrals > 0 && crewmates.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.devourerSpawnRate.getSelection())) {
                        setRoleToRandomPlayer((byte)RoleId.Devourer, crewmates);
                        neutrals -= 1;
                    }

                    // Modifiers get the modifier number and assing by list order if the option is active            
                    mymodifiers.Clear();
                    bool activeModifiers = CustomOptionHolder.activateModifiers.getBool();
                    //if (rnd.Next(1, 2) <= CustomOptionHolder.activateModifiers.getSelection()) {
                    if (activeModifiers) {
                        //for (int i = 0; i < CustomOptionHolder.numberOfModifiers.getFloat(); i++) {
                        for (int i = 0; i < modifiers.Count(); i++) {
                            mymodifiers.Add(i);
                        }
                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.loverPlayer.getSelection())) {
                            setRoleToRandomPlayer((byte)RoleId.Lover, modifiers, 0);
                            setRoleToRandomPlayer((byte)RoleId.Lover, modifiers, 1);
                        }
                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.lighterPlayer.getSelection())) {
                            setRoleToRandomPlayer((byte)RoleId.Lighter, modifiers);
                        }
                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.blindPlayer.getSelection())) {
                            setRoleToRandomPlayer((byte)RoleId.Blind, modifiers);
                        }
                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.flashPlayer.getSelection())) {
                            setRoleToRandomPlayer((byte)RoleId.Flash, modifiers);
                        }
                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.bigchungusPlayer.getSelection())) {
                            setRoleToRandomPlayer((byte)RoleId.BigChungus, modifiers);
                        }
                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.theChosenOnePlayer.getSelection())) {
                            setRoleToRandomPlayer((byte)RoleId.TheChosenOne, modifiers);
                        }
                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.performerPlayer.getSelection())) {
                            setRoleToRandomPlayer((byte)RoleId.Performer, modifiers);
                        }
                    }
                }
                else {
                    var crewmateMin = 1;
                    var crewmateMax = 15;
                    var neutralMin = 1;
                    var neutralMax = 1;
                    var impostorMin = 1;
                    var impostorMax = 3;
                    var rebelMin = 1;
                    var rebelMax = 1;

                    // Make sure min is less or equal to max
                    if (crewmateMin > crewmateMax) crewmateMin = crewmateMax;
                    if (neutralMin > neutralMax) neutralMin = neutralMax;
                    if (impostorMin > impostorMax) impostorMin = impostorMax;
                    if (rebelMin > rebelMax) rebelMin = rebelMax;

                    // Get the maximum allowed count of each role type based on the minimum and maximum option
                    int crewCountSettings = rnd.Next(crewmateMin, crewmateMax + 1);
                    int neutralCountSettings = rnd.Next(neutralMin, neutralMax + 1);
                    int impCountSettings = rnd.Next(impostorMin, impostorMax + 1);
                    int rebelCountSettings = rnd.Next(rebelMin, rebelMax + 1);

                    // Potentially lower the actual maximum to the assignable players
                    int maxCrewmateRoles = Mathf.Min(crewmates.Count, crewCountSettings);
                    int maxNeutralRoles = Mathf.Min(crewmates.Count, neutralCountSettings);
                    int maxImpostorRoles = Mathf.Min(impostors.Count, impCountSettings);
                    int maxRebelRoles = Mathf.Min(crewmates.Count, rebelCountSettings);

                    // Fill in the lists with the roles that are active in the settings
                    Dictionary<byte, int> impSettings = new Dictionary<byte, int>();
                    Dictionary<byte, int> neutralSettings = new Dictionary<byte, int>();
                    Dictionary<byte, int> crewSettings = new Dictionary<byte, int>();
                    Dictionary<byte, int> rebelSettings = new Dictionary<byte, int>();

                    impSettings.Add((byte)RoleId.Mimic, CustomOptionHolder.mimicSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Painter, CustomOptionHolder.painterSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Demon, CustomOptionHolder.demonSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Ilusionist, CustomOptionHolder.ilusionistSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Janitor, CustomOptionHolder.janitorSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Manipulator, CustomOptionHolder.manipulatorSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Bomberman, CustomOptionHolder.bombermanSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Chameleon, CustomOptionHolder.chameleonSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Gambler, CustomOptionHolder.gamblerSpawnRate.getSelection());
                    impSettings.Add((byte)RoleId.Sorcerer, CustomOptionHolder.sorcererSpawnRate.getSelection());

                    neutralSettings.Add((byte)RoleId.Joker, CustomOptionHolder.jokerSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Pyromaniac, CustomOptionHolder.pyromaniacSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.RoleThief, CustomOptionHolder.rolethiefSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.TreasureHunter, CustomOptionHolder.treasureHunterSpawnRate.getSelection());
                    neutralSettings.Add((byte)RoleId.Devourer, CustomOptionHolder.devourerSpawnRate.getSelection());

                    rebelSettings.Add((byte)RoleId.Renegade, CustomOptionHolder.renegadeSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.BountyHunter, CustomOptionHolder.bountyHunterSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Trapper, CustomOptionHolder.trapperSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Yinyanger, CustomOptionHolder.yinyangerSpawnRate.getSelection());
                    rebelSettings.Add((byte)RoleId.Challenger, CustomOptionHolder.challengerSpawnRate.getSelection());

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
                    crewSettings.Add((byte)RoleId.Coward, CustomOptionHolder.cowardSpawnRate.getSelection());
                    if (PlayerControl.GameOptions.MapId != 1) {
                        crewSettings.Add((byte)RoleId.Vigilant, CustomOptionHolder.vigilantSpawnRate.getSelection());
                    }
                    else {
                        crewSettings.Add((byte)RoleId.VigilantMira, CustomOptionHolder.vigilantSpawnRate.getSelection());
                    }
                    crewSettings.Add((byte)RoleId.Medusa, CustomOptionHolder.medusaSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Hunter, CustomOptionHolder.hunterSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Jinx, CustomOptionHolder.jinxSpawnRate.getSelection());

                    // Get all roles where the chance to occur is set to 100%
                    List<byte> ensuredCrewmateRoles = crewSettings.Where(x => x.Value == 1).Select(x => x.Key).ToList();
                    List<byte> ensuredNeutralRoles = neutralSettings.Where(x => x.Value == 1).Select(x => x.Key).ToList();
                    List<byte> ensuredImpostorRoles = impSettings.Where(x => x.Value == 1).Select(x => x.Key).ToList();
                    List<byte> ensuredRebelRoles = rebelSettings.Where(x => x.Value == 1).Select(x => x.Key).ToList();

                    // Assign roles until we run out of either players we can assign roles to or run out of roles we can assign to players
                    while (
                        (impostors.Count > 0 && maxImpostorRoles > 0 && ensuredImpostorRoles.Count > 0) ||
                        (crewmates.Count > 0 && (
                            (maxRebelRoles > 0 && ensuredRebelRoles.Count > 0) ||
                            (maxNeutralRoles > 0 && ensuredNeutralRoles.Count > 0) ||
                            (maxCrewmateRoles > 0 && ensuredCrewmateRoles.Count > 0)
                        ))) {

                        Dictionary<RoleType, List<byte>> rolesToAssign = new Dictionary<RoleType, List<byte>>();
                        if (impostors.Count > 0 && maxImpostorRoles > 0 && ensuredImpostorRoles.Count > 0) rolesToAssign.Add(RoleType.Impostor, ensuredImpostorRoles);
                        if (crewmates.Count > 0 && maxRebelRoles > 0 && ensuredRebelRoles.Count > 0) rolesToAssign.Add(RoleType.Rebel, ensuredRebelRoles);
                        if (crewmates.Count > 0 && maxNeutralRoles > 0 && ensuredNeutralRoles.Count > 0) rolesToAssign.Add(RoleType.Neutral, ensuredNeutralRoles);
                        if (crewmates.Count > 0 && maxCrewmateRoles > 0 && ensuredCrewmateRoles.Count > 0) rolesToAssign.Add(RoleType.Crewmate, ensuredCrewmateRoles);

                        // Randomly select a pool of roles to assign a role from (Crewmate role, Neutral role, Impostor role or Rebel role) then select one of the roles from the selected pool to a player and remove the rol from the pool
                        var roleType = rolesToAssign.Keys.ElementAt(rnd.Next(0, rolesToAssign.Keys.Count()));
                        var players = roleType == RoleType.Rebel || roleType == RoleType.Neutral || roleType == RoleType.Crewmate ? crewmates : impostors;
                        var index = rnd.Next(0, rolesToAssign[roleType].Count);
                        var roleId = rolesToAssign[roleType][index];
                        setRoleToRandomPlayer(rolesToAssign[roleType][index], players);
                        rolesToAssign[roleType].RemoveAt(index);

                        // Adjust the role limit
                        switch (roleType) {
                            case RoleType.Impostor: maxImpostorRoles--; break;
                            case RoleType.Rebel: maxRebelRoles--; break;
                            case RoleType.Neutral: maxNeutralRoles--; break;
                            case RoleType.Crewmate: maxCrewmateRoles--; break;
                        }
                    }

                    // Add modifiers after selecting the roles
                    mymodifiers.Clear();
                    bool activeModifiers = CustomOptionHolder.activateModifiers.getBool();
                    //if (rnd.Next(1, 2) <= CustomOptionHolder.activateModifiers.getSelection()) {
                        if (activeModifiers) {
                        foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                            int num = rnd.Next(1, 8);
                            //while (mymodifiers.Contains(num) && mymodifiers.Count < CustomOptionHolder.numberOfModifiers.getFloat()) {
                            while (mymodifiers.Contains(num) && mymodifiers.Count < modifiers.Count()) {
                                num = rnd.Next(1, 8);
                            }

                            //if (mymodifiers.Count < CustomOptionHolder.numberOfModifiers.getFloat()) {
                            if (mymodifiers.Count < modifiers.Count()) {
                                switch (num) {
                                    case 1:
                                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.loverPlayer.getSelection())) {
                                            setRoleToRandomPlayer((byte)RoleId.Lover, modifiers, 0);
                                            setRoleToRandomPlayer((byte)RoleId.Lover, modifiers, 1);
                                        }
                                        break;
                                    case 2:
                                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.lighterPlayer.getSelection())) {
                                            setRoleToRandomPlayer((byte)RoleId.Lighter, modifiers);
                                        }
                                        break;
                                    case 3:
                                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.blindPlayer.getSelection())) {
                                            setRoleToRandomPlayer((byte)RoleId.Blind, modifiers);
                                        }
                                        break;
                                    case 4:
                                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.flashPlayer.getSelection())) {
                                            setRoleToRandomPlayer((byte)RoleId.Flash, modifiers);
                                        }
                                        break;
                                    case 5:
                                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.bigchungusPlayer.getSelection())) {
                                            setRoleToRandomPlayer((byte)RoleId.BigChungus, modifiers);
                                        }
                                        break;
                                    case 6:
                                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.theChosenOnePlayer.getSelection())) {
                                            setRoleToRandomPlayer((byte)RoleId.TheChosenOne, modifiers);
                                        }
                                        break;
                                    case 7:
                                        if (mymodifiers.Count > 0 && (rnd.Next(1, 2) <= CustomOptionHolder.performerPlayer.getSelection())) {
                                            setRoleToRandomPlayer((byte)RoleId.Performer, modifiers);
                                        }
                                        break;
                                }
                                mymodifiers.Add(num);
                            }
                        }
                    }
                }
            }
            else if (CaptureTheFlag.captureTheFlagMode && howmanygamemodesareon == 1) {
                // Capture the flag    
                myCapturetheflag.Clear();
                bool oddNumber = false;
                if (Mathf.Ceil(PlayerControl.AllPlayerControls.Count) % 2 != 0) {
                    oddNumber = true;
                    setRoleToRandomPlayer((byte)RoleId.StealerPlayer, modifiers);
                }
                int myflag = 1;
                while (myCapturetheflag.Count < (Mathf.Round(PlayerControl.AllPlayerControls.Count / 2))) {
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
                    myCapturetheflag.Add(myflag);
                    myflag += 1;
                }
                int myblueflag = 9;
                while (!oddNumber && myCapturetheflag.Count < PlayerControl.AllPlayerControls.Count || oddNumber && myCapturetheflag.Count < PlayerControl.AllPlayerControls.Count - 1) {
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
                    myCapturetheflag.Add(myblueflag);
                    myblueflag += 1;
                }
            }
            else if (PoliceAndThief.policeAndThiefMode && howmanygamemodesareon == 1) {
                // Police and Thief    
                myPoliceandthief.Clear();
                int mypolice = 1;
                while (myPoliceandthief.Count < (Mathf.Round(PlayerControl.AllPlayerControls.Count / 2.75f))) {
                    switch (mypolice) {
                        case 1:
                            setRoleToRandomPlayer((byte)RoleId.PolicePlayer01, modifiers);
                            break;
                        case 2:
                            setRoleToRandomPlayer((byte)RoleId.PolicePlayer02, modifiers);
                            break;
                        case 3:
                            setRoleToRandomPlayer((byte)RoleId.PolicePlayer03, modifiers);
                            break;
                        case 4:
                            setRoleToRandomPlayer((byte)RoleId.PolicePlayer04, modifiers);
                            break;
                        case 5:
                            setRoleToRandomPlayer((byte)RoleId.PolicePlayer05, modifiers);
                            break;
                    }
                    myPoliceandthief.Add(mypolice);
                    mypolice += 1;
                }
                int mythief = 6;
                while (myPoliceandthief.Count < PlayerControl.AllPlayerControls.Count) {
                    switch (mythief) {
                        case 6:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer01, modifiers);
                            break;
                        case 7:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer02, modifiers);
                            break;
                        case 8:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer03, modifiers);
                            break;
                        case 9:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer04, modifiers);
                            break;
                        case 10:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer05, modifiers);
                            break;
                        case 11:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer06, modifiers);
                            break;
                        case 12:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer07, modifiers);
                            break;
                        case 13:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer08, modifiers);
                            break;
                        case 14:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer09, modifiers);
                            break;
                        case 15:
                            setRoleToRandomPlayer((byte)RoleId.ThiefPlayer10, modifiers);
                            break;
                    }
                    myPoliceandthief.Add(mythief);
                    mythief += 1;
                }
            }
            else if (KingOfTheHill.kingOfTheHillMode && howmanygamemodesareon == 1) {
                // King of the hill    
                myKingoftheHill.Clear();
                bool oddNumber = false;
                if (Mathf.Ceil(PlayerControl.AllPlayerControls.Count) % 2 != 0) {
                    oddNumber = true;
                    setRoleToRandomPlayer((byte)RoleId.UsurperPlayer, modifiers);
                }
                int myking = 1;
                while (myKingoftheHill.Count < (Mathf.Round(PlayerControl.AllPlayerControls.Count / 2))) {
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
                    myKingoftheHill.Add(myking);
                    myking += 1;
                }
                int myyellowking = 9;
                while (!oddNumber && myKingoftheHill.Count < PlayerControl.AllPlayerControls.Count || oddNumber && myKingoftheHill.Count < PlayerControl.AllPlayerControls.Count - 1) {
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
                    myKingoftheHill.Add(myyellowking);
                    myyellowking += 1;
                }
            }
            else if (HotPotato.hotPotatoMode && howmanygamemodesareon == 1) {
                // Hot Potato   
                myHotPotato.Clear();
                int mypotato = 1;
                while (myHotPotato.Count < PlayerControl.AllPlayerControls.Count) {
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
                    myHotPotato.Add(mypotato);
                    mypotato += 1;
                }
            }
            else if (ZombieLaboratory.zombieLaboratoryMode && howmanygamemodesareon == 1) {
                // ZombieLaboratory
                myZombie.Clear();
                int myzombie = 1;
                while (myZombie.Count < PlayerControl.AllPlayerControls.Count) {
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
                    myZombie.Add(myzombie);
                    myzombie += 1;
                }
            }
        }

        private static byte setRoleToRandomPlayer(byte roleId, List<PlayerControl> playerList, byte flag = 0, bool removePlayer = true) {
            var index = rnd.Next(0, playerList.Count);
            byte playerId = playerList[index].PlayerId;
            if (removePlayer) playerList.RemoveAt(index);

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetRole, Hazel.SendOption.Reliable, -1);
            writer.Write(roleId);
            writer.Write(playerId);
            writer.Write(flag);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.setRole(roleId, playerId, flag);
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