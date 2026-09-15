using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using static LasMonjas.LasMonjas;
using AmongUs.GameOptions;
using LasMonjas.Core;

namespace LasMonjas.Patches
{
    [HarmonyPatch(typeof(RoleOptionsCollectionV08), nameof(RoleOptionsCollectionV08.GetNumPerGame))]
    class RoleOptionsDataGetNumPerGamePatch
    {
        public static void Postfix(ref int __result) {
            if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.Normal) __result = 0; // Deactivate Vanilla Roles
        }
    }

    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    class RoleManagerSelectRolesPatch
    {

        private static List<int> myGamemodeList = new List<int>();

        public static void Postfix() {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ResetVaribles, Hazel.SendOption.Reliable, -1);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.resetVariables();

            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {                
                getRoleAssignmentData();
            }
        }

        private static readonly RoleId[][] ZombieLaboratoryLayouts = {

            null, // case 0 doesn't do anything

            new RoleId[] // start zombies = 1
                {
                    RoleId.ZombiePlayer01,
                    RoleId.NursePlayer,
                    RoleId.SurvivorPlayer01,
                    RoleId.SurvivorPlayer02,
                    RoleId.SurvivorPlayer03,
                    RoleId.SurvivorPlayer04,
                    RoleId.SurvivorPlayer05,
                    RoleId.SurvivorPlayer06,
                    RoleId.SurvivorPlayer07,
                    RoleId.SurvivorPlayer08,
                    RoleId.SurvivorPlayer09,
                    RoleId.SurvivorPlayer10,
                    RoleId.SurvivorPlayer11,
                    RoleId.SurvivorPlayer12,
                    RoleId.SurvivorPlayer13
                },

                new RoleId[] // start zombies = 2
                {
                    RoleId.ZombiePlayer01,
                    RoleId.NursePlayer,
                    RoleId.SurvivorPlayer01,
                    RoleId.ZombiePlayer02,
                    RoleId.SurvivorPlayer02,
                    RoleId.SurvivorPlayer03,
                    RoleId.SurvivorPlayer04,
                    RoleId.SurvivorPlayer05,
                    RoleId.SurvivorPlayer06,
                    RoleId.SurvivorPlayer07,
                    RoleId.SurvivorPlayer08,
                    RoleId.SurvivorPlayer09,
                    RoleId.SurvivorPlayer10,
                    RoleId.SurvivorPlayer11,
                    RoleId.SurvivorPlayer12
                },

            new RoleId[] // start zombies = 3
                {
                    RoleId.ZombiePlayer01,
                    RoleId.NursePlayer,
                    RoleId.SurvivorPlayer01,
                    RoleId.ZombiePlayer02,
                    RoleId.ZombiePlayer03,
                    RoleId.SurvivorPlayer02,
                    RoleId.SurvivorPlayer03,
                    RoleId.SurvivorPlayer04,
                    RoleId.SurvivorPlayer05,
                    RoleId.SurvivorPlayer06,
                    RoleId.SurvivorPlayer07,
                    RoleId.SurvivorPlayer08,
                    RoleId.SurvivorPlayer09,
                    RoleId.SurvivorPlayer10,
                    RoleId.SurvivorPlayer11
                },

            new RoleId[] // start zombies = 4
                {
                    RoleId.ZombiePlayer01,
                    RoleId.NursePlayer,
                    RoleId.SurvivorPlayer01,
                    RoleId.ZombiePlayer02,
                    RoleId.ZombiePlayer03,
                    RoleId.ZombiePlayer04,
                    RoleId.SurvivorPlayer02,
                    RoleId.SurvivorPlayer03,
                    RoleId.SurvivorPlayer04,
                    RoleId.SurvivorPlayer05,
                    RoleId.SurvivorPlayer06,
                    RoleId.SurvivorPlayer07,
                    RoleId.SurvivorPlayer08,
                    RoleId.SurvivorPlayer09,
                    RoleId.SurvivorPlayer10
                },

            new RoleId[] // start zombies = 5
                {
                    RoleId.ZombiePlayer01,
                    RoleId.NursePlayer,
                    RoleId.SurvivorPlayer01,
                    RoleId.ZombiePlayer02,
                    RoleId.ZombiePlayer03,
                    RoleId.ZombiePlayer04,
                    RoleId.ZombiePlayer05,
                    RoleId.SurvivorPlayer02,
                    RoleId.SurvivorPlayer03,
                    RoleId.SurvivorPlayer04,
                    RoleId.SurvivorPlayer05,
                    RoleId.SurvivorPlayer06,
                    RoleId.SurvivorPlayer07,
                    RoleId.SurvivorPlayer08,
                    RoleId.SurvivorPlayer09
                }
        };

        private static void getRoleAssignmentData() {
            // Get 3 player lists, one for crewmates/neutrals/rebels, one for impostor and a global one for gamemodes.
            List<PlayerControl> crewmates = PlayerControl.AllPlayerControls.ToArray().ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            crewmates.RemoveAll(x => x.Data.Role.IsImpostor);
            List<PlayerControl> impostors = PlayerControl.AllPlayerControls.ToArray().ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
            impostors.RemoveAll(x => !x.Data.Role.IsImpostor);
            List<PlayerControl> modifiers = PlayerControl.AllPlayerControls.ToArray().ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();

            myGamemodeList.Clear();
            bool oddNumber = false;
            int playerNumber = 1;

            // Assign roles only if the game won't be a custom gamemode
            switch (gameType) {
                case 0:
                    // Roles
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
                    crewSettings.Add((byte)RoleId.Vigilant, CustomOptionHolder.vigilantSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Hunter, CustomOptionHolder.hunterSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Jinx, CustomOptionHolder.jinxSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Coward, CustomOptionHolder.cowardSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Bat, CustomOptionHolder.batSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Necromancer, CustomOptionHolder.necromancerSpawnRate.getSelection());
                    crewSettings.Add((byte)RoleId.Engineer, CustomOptionHolder.engineerSpawnRate.getSelection());
                    if ((GameOptionsManager.Instance.currentGameOptions.MapId == 0 && !CustomOptionHolder.activateSenseiMap.getBool()) || GameOptionsManager.Instance.currentGameOptions.MapId >= 1) {
                        crewSettings.Add((byte)RoleId.Locksmith, CustomOptionHolder.locksmithSpawnRate.getSelection());
                    }
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

                    // Add modifiers after selecting the roles
                    assignModifiers();
                    break;
                case 1:
                    // Find a Role
                    break;
                case 2:
                    // CTF
                    if (Mathf.Ceil(PlayerInCache.AllPlayers.Count) % 2 != 0) {
                        oddNumber = true;
                        setRoleToRandomPlayer((byte)RoleId.StealerPlayer, modifiers);
                    }
                    while (myGamemodeList.Count < (Mathf.Round(PlayerInCache.AllPlayers.Count / 2))) {
                        setRoleToRandomPlayer((byte)((int)RoleId.RedPlayer01 + playerNumber - 1), modifiers);
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    playerNumber = 9;
                    while (!oddNumber && myGamemodeList.Count < PlayerInCache.AllPlayers.Count || oddNumber && myGamemodeList.Count < PlayerInCache.AllPlayers.Count - 1) {
                        setRoleToRandomPlayer((byte)((int)RoleId.BluePlayer01 + playerNumber - 9), modifiers); 
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    break;
                case 3:
                    // PT
                    while (myGamemodeList.Count < (Mathf.Round(PlayerInCache.AllPlayers.Count / 2.39f))) {
                        setRoleToRandomPlayer((byte)((int)RoleId.PolicePlayer01 + playerNumber - 1), modifiers);
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    playerNumber = 7;
                    while (myGamemodeList.Count < PlayerInCache.AllPlayers.Count) {
                        setRoleToRandomPlayer((byte)((int)RoleId.ThiefPlayer01 + playerNumber - 7), modifiers);
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    break;
                case 4:
                    // KOTH
                    if (Mathf.Ceil(PlayerInCache.AllPlayers.Count) % 2 != 0) {
                        oddNumber = true;
                        setRoleToRandomPlayer((byte)RoleId.UsurperPlayer, modifiers);
                    }
                    while (myGamemodeList.Count < (Mathf.Round(PlayerInCache.AllPlayers.Count / 2))) {
                        setRoleToRandomPlayer((byte)((int)RoleId.GreenKing + playerNumber - 1), modifiers);
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    playerNumber = 9;
                    while (!oddNumber && myGamemodeList.Count < PlayerInCache.AllPlayers.Count || oddNumber && myGamemodeList.Count < PlayerInCache.AllPlayers.Count - 1) {
                        setRoleToRandomPlayer((byte)((int)RoleId.YellowKing + playerNumber - 9), modifiers); 
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    break;
                case 5:
                    // HP
                    while (myGamemodeList.Count < PlayerInCache.AllPlayers.Count) {
                        setRoleToRandomPlayer((byte)((int)RoleId.HotPotato + playerNumber - 1), modifiers); 
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    break;
                case 6:
                    // ZL  
                    var layout = ZombieLaboratoryLayouts[(int)ZombieLaboratory.startZombies];

                    for (int i = 0; i < PlayerInCache.AllPlayers.Count; i++) {
                        setRoleToRandomPlayer((byte)layout[i], modifiers);
                    }                        
                    myGamemodeList.Add(playerNumber);
                    playerNumber += 1;
                    break;
                case 7:
                    // BR
                    if (BattleRoyale.matchType == 0) {
                        while (myGamemodeList.Count < PlayerInCache.AllPlayers.Count) {
                            setRoleToRandomPlayer((byte)((int)RoleId.SoloPlayer01 + playerNumber - 1), modifiers);
                            myGamemodeList.Add(playerNumber);
                            playerNumber += 1;
                        }
                    }
                    else {
                        // Battle Royale Teams
                        if (Mathf.Ceil(PlayerInCache.AllPlayers.Count) % 2 != 0) {
                            oddNumber = true;
                            setRoleToRandomPlayer((byte)RoleId.SerialKiller, modifiers);
                        }
                        while (myGamemodeList.Count < (Mathf.Round(PlayerInCache.AllPlayers.Count / 2))) {
                            setRoleToRandomPlayer((byte)((int)RoleId.LimePlayer01 + playerNumber - 1), modifiers);
                            myGamemodeList.Add(playerNumber);
                            playerNumber += 1;
                        }
                        playerNumber = 9;
                        while (!oddNumber && myGamemodeList.Count < PlayerInCache.AllPlayers.Count || oddNumber && myGamemodeList.Count < PlayerInCache.AllPlayers.Count - 1) {
                            setRoleToRandomPlayer((byte)((int)RoleId.PinkPlayer01 + playerNumber - 9), modifiers); 
                            myGamemodeList.Add(playerNumber);
                            playerNumber += 1;
                        }
                    }
                    break;
                case 8:
                    // MF
                    if (Mathf.Ceil(PlayerInCache.AllPlayers.Count) % 2 != 0) {
                        oddNumber = true;
                        setRoleToRandomPlayer((byte)RoleId.BigMonja, modifiers);
                    }
                    while (myGamemodeList.Count < (Mathf.Round(PlayerInCache.AllPlayers.Count / 2))) {
                        setRoleToRandomPlayer((byte)((int)RoleId.GreenMonjaPlayer01 + playerNumber - 1), modifiers); 
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    playerNumber = 9;
                    while (!oddNumber && myGamemodeList.Count < PlayerInCache.AllPlayers.Count || oddNumber && myGamemodeList.Count < PlayerInCache.AllPlayers.Count - 1) {
                        setRoleToRandomPlayer((byte)((int)RoleId.CyanPlayer01 + playerNumber - 9), modifiers);
                        myGamemodeList.Add(playerNumber);
                        playerNumber += 1;
                    }
                    break;
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

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetRole, Hazel.SendOption.Reliable, -1);
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

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetModifier, Hazel.SendOption.Reliable, -1);
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