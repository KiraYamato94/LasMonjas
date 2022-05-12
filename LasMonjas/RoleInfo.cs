using HarmonyLib;
using System.Linq;
using System;
using System.Collections.Generic;
using static LasMonjas.LasMonjas;
using UnityEngine;

namespace LasMonjas
{
    class RoleInfo
    {
        public Color color;
        public string name;
        public string introDescription;
        public string shortDescription;
        public RoleId roleId;
        public bool isNeutral;
        public bool isRebel;

        RoleInfo(string name, Color color, string introDescription, string shortDescription, RoleId roleId, bool isNeutral = false, bool isRebel = false) {
            this.color = color;
            this.name = name;
            this.introDescription = introDescription;
            this.shortDescription = shortDescription;
            this.roleId = roleId;
            this.isNeutral = isNeutral;
            this.isRebel = isRebel;
        }

        // Capture the Flag Teams
        public static RoleInfo redplayer01 = new RoleInfo("Red Team", Color.red, "Steal <color=#0000FFFF>Blue Team</color> flag", "Steal <color=#0000FFFF>Blue Team</color> flag", RoleId.RedPlayer01);
        public static RoleInfo redplayer02 = new RoleInfo("Red Team", Color.red, "Steal <color=#0000FFFF>Blue Team</color> flag", "Steal <color=#0000FFFF>Blue Team</color> flag", RoleId.RedPlayer02);
        public static RoleInfo redplayer03 = new RoleInfo("Red Team", Color.red, "Steal <color=#0000FFFF>Blue Team</color> flag", "Steal <color=#0000FFFF>Blue Team</color> flag", RoleId.RedPlayer03);
        public static RoleInfo redplayer04 = new RoleInfo("Red Team", Color.red, "Steal <color=#0000FFFF>Blue Team</color> flag", "Steal <color=#0000FFFF>Blue Team</color> flag", RoleId.RedPlayer04);
        public static RoleInfo redplayer05 = new RoleInfo("Red Team", Color.red, "Steal <color=#0000FFFF>Blue Team</color> flag", "Steal <color=#0000FFFF>Blue Team</color> flag", RoleId.RedPlayer05);
        public static RoleInfo redplayer06 = new RoleInfo("Red Team", Color.red, "Steal <color=#0000FFFF>Blue Team</color> flag", "Steal <color=#0000FFFF>Blue Team</color> flag", RoleId.RedPlayer06);
        public static RoleInfo redplayer07 = new RoleInfo("Red Team", Color.red, "Steal <color=#0000FFFF>Blue Team</color> flag", "Steal <color=#0000FFFF>Blue Team</color> flag", RoleId.RedPlayer07);
        public static RoleInfo blueplayer01 = new RoleInfo("Blue Team", Color.blue, "Steal <color=#FF0000FF>Red Team</color> flag", "Steal <color=#FF0000FF>Red Team</color> flag", RoleId.BluePlayer01);
        public static RoleInfo blueplayer02 = new RoleInfo("Blue Team", Color.blue, "Steal <color=#FF0000FF>Red Team</color> flag", "Steal <color=#FF0000FF>Red Team</color> flag", RoleId.BluePlayer02);
        public static RoleInfo blueplayer03 = new RoleInfo("Blue Team", Color.blue, "Steal <color=#FF0000FF>Red Team</color> flag", "Steal <color=#FF0000FF>Red Team</color> flag", RoleId.BluePlayer03);
        public static RoleInfo blueplayer04 = new RoleInfo("Blue Team", Color.blue, "Steal <color=#FF0000FF>Red Team</color> flag", "Steal <color=#FF0000FF>Red Team</color> flag", RoleId.BluePlayer04);
        public static RoleInfo blueplayer05 = new RoleInfo("Blue Team", Color.blue, "Steal <color=#FF0000FF>Red Team</color> flag", "Steal <color=#FF0000FF>Red Team</color> flag", RoleId.BluePlayer05);
        public static RoleInfo blueplayer06 = new RoleInfo("Blue Team", Color.blue, "Steal <color=#FF0000FF>Red Team</color> flag", "Steal <color=#FF0000FF>Red Team</color> flag", RoleId.BluePlayer06);
        public static RoleInfo blueplayer07 = new RoleInfo("Blue Team", Color.blue, "Steal <color=#FF0000FF>Red Team</color> flag", "Steal <color=#FF0000FF>Red Team</color> flag", RoleId.BluePlayer07);
        public static RoleInfo stealerplayer = new RoleInfo("Flag Stealer", Color.grey, "Kill the player with a flag to switch teams with it", "Kill the player with a flag \nto switch teams with it", RoleId.StealerPlayer);

        // Police and Thief Teams
        public static RoleInfo policeplayer01 = new RoleInfo("Police Officer", Color.cyan, "Capture all the <color=#D2B48CFF>Thiefs</color>", "Capture all the <color=#D2B48CFF>Thiefs</color>", RoleId.PolicePlayer01);
        public static RoleInfo policeplayer02 = new RoleInfo("Police Officer", Color.cyan, "Capture all the <color=#D2B48CFF>Thiefs</color>", "Capture all the <color=#D2B48CFF>Thiefs</color>", RoleId.PolicePlayer02);
        public static RoleInfo policeplayer03 = new RoleInfo("Police Officer", Color.cyan, "Capture all the <color=#D2B48CFF>Thiefs</color>", "Capture all the <color=#D2B48CFF>Thiefs</color>", RoleId.PolicePlayer03);
        public static RoleInfo policeplayer04 = new RoleInfo("Police Officer", Color.cyan, "Capture all the <color=#D2B48CFF>Thiefs</color>", "Capture all the <color=#D2B48CFF>Thiefs</color>", RoleId.PolicePlayer04);
        public static RoleInfo policeplayer05 = new RoleInfo("Police Officer", Color.cyan, "Capture all the <color=#D2B48CFF>Thiefs</color>", "Capture all the <color=#D2B48CFF>Thiefs</color>", RoleId.PolicePlayer05);
        public static RoleInfo thiefplayer01 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer01);
        public static RoleInfo thiefplayer02 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer02);
        public static RoleInfo thiefplayer03 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer03);
        public static RoleInfo thiefplayer04 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer04);
        public static RoleInfo thiefplayer05 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer05);
        public static RoleInfo thiefplayer06 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer06);
        public static RoleInfo thiefplayer07 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer07);
        public static RoleInfo thiefplayer08 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer08);
        public static RoleInfo thiefplayer09 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer09);
        public static RoleInfo thiefplayer10 = new RoleInfo("Thief", Mechanic.color, "Steal all the jewels without getting captured", "Steal all the jewels \nwithout getting captured", RoleId.ThiefPlayer10);

        // King of the hill Teams
        public static RoleInfo greenKing = new RoleInfo("Green King", Color.green, "Capture the zones", "Capture the zones", RoleId.GreenKing);
        public static RoleInfo greenplayer01 = new RoleInfo("Green Team", Color.green, "Protect your King", "Protect your King", RoleId.GreenPlayer01);
        public static RoleInfo greenplayer02 = new RoleInfo("Green Team", Color.green, "Protect your King", "Protect your King", RoleId.GreenPlayer02);
        public static RoleInfo greenplayer03 = new RoleInfo("Green Team", Color.green, "Protect your King", "Protect your King", RoleId.GreenPlayer03);
        public static RoleInfo greenplayer04 = new RoleInfo("Green Team", Color.green, "Protect your King", "Protect your King", RoleId.GreenPlayer04);
        public static RoleInfo greenplayer05 = new RoleInfo("Green Team", Color.green, "Protect your King", "Protect your King", RoleId.GreenPlayer05);
        public static RoleInfo greenplayer06 = new RoleInfo("Green Team", Color.green, "Protect your King", "Protect your King", RoleId.GreenPlayer06);
        public static RoleInfo yellowKing = new RoleInfo("Yellow King", Color.yellow, "Capture the zones", "Capture the zones", RoleId.YellowKing);
        public static RoleInfo yellowplayer01 = new RoleInfo("Yellow Team", Color.yellow, "Protect your King", "Protect your King", RoleId.YellowPlayer01);
        public static RoleInfo yellowplayer02 = new RoleInfo("Yellow Team", Color.yellow, "Protect your King", "Protect your King", RoleId.YellowPlayer02);
        public static RoleInfo yellowplayer03 = new RoleInfo("Yellow Team", Color.yellow, "Protect your King", "Protect your King", RoleId.YellowPlayer03);
        public static RoleInfo yellowplayer04 = new RoleInfo("Yellow Team", Color.yellow, "Protect your King", "Protect your King", RoleId.YellowPlayer04);
        public static RoleInfo yellowplayer05 = new RoleInfo("Yellow Team", Color.yellow, "Protect your King", "Protect your King", RoleId.YellowPlayer05);
        public static RoleInfo yellowplayer06 = new RoleInfo("Yellow Team", Color.yellow, "Protect your King", "Protect your King", RoleId.YellowPlayer06);
        public static RoleInfo usurperplayer = new RoleInfo("Usurper", Color.grey, "Kill a King to become one", "Kill a King to become one", RoleId.UsurperPlayer);

        // Hot Potato Teams
        public static RoleInfo hotPotatoPlayer = new RoleInfo("Hot Potato", Color.grey, "Give the hot potato to other player", "Give the hot potato \nto other player", RoleId.HotPotato);
        public static RoleInfo notPotato01 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato01);
        public static RoleInfo notPotato02 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato02);
        public static RoleInfo notPotato03 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato03);
        public static RoleInfo notPotato04 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato04);
        public static RoleInfo notPotato05 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato05);
        public static RoleInfo notPotato06 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato06);
        public static RoleInfo notPotato07 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato07);
        public static RoleInfo notPotato08 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato08);
        public static RoleInfo notPotato09 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato09);
        public static RoleInfo notPotato10 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato10);
        public static RoleInfo notPotato11 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato11);
        public static RoleInfo notPotato12 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato12);
        public static RoleInfo notPotato13 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato13);
        public static RoleInfo notPotato14 = new RoleInfo("Cold Potato", Color.cyan, "Run from the Hot Potato", "Run from the Hot Potato", RoleId.NotPotato14);
        public static RoleInfo explodedPotato01 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato01);
        public static RoleInfo explodedPotato02 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato02);
        public static RoleInfo explodedPotato03 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato03);
        public static RoleInfo explodedPotato04 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato04);
        public static RoleInfo explodedPotato05 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato05);
        public static RoleInfo explodedPotato06 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato06);
        public static RoleInfo explodedPotato07 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato07);
        public static RoleInfo explodedPotato08 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato08);
        public static RoleInfo explodedPotato09 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato09);
        public static RoleInfo explodedPotato10 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato10);
        public static RoleInfo explodedPotato11 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato11);
        public static RoleInfo explodedPotato12 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato12);
        public static RoleInfo explodedPotato13 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato13);
        public static RoleInfo explodedPotato14 = new RoleInfo("Burnt Potato", Mechanic.color, "You are burnt", "You are burnt", RoleId.ExplodedPotato14);

        // ZombieLaboratory Teams
        public static RoleInfo nursePlayer = new RoleInfo("Nurse", Medusa.color, "Heal survivors and create the cure", "Heal survivors and create the cure", RoleId.NursePlayer);
        public static RoleInfo survivorPlayer01 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer01);
        public static RoleInfo survivorPlayer02 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer02);
        public static RoleInfo survivorPlayer03 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer03);
        public static RoleInfo survivorPlayer04 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer04);
        public static RoleInfo survivorPlayer05 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer05);
        public static RoleInfo survivorPlayer06 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer06);
        public static RoleInfo survivorPlayer07 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer07);
        public static RoleInfo survivorPlayer08 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer08);
        public static RoleInfo survivorPlayer09 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer09);
        public static RoleInfo survivorPlayer10 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer10);
        public static RoleInfo survivorPlayer11 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer11);
        public static RoleInfo survivorPlayer12 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer12);
        public static RoleInfo survivorPlayer13 = new RoleInfo("Survivor", Color.cyan, "Survive while looking for items to make the cure", "Survive while looking \nfor items to make the cure", RoleId.SurvivorPlayer13);
        public static RoleInfo zombiePlayer01 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer01);
        public static RoleInfo zombiePlayer02 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer02);
        public static RoleInfo zombiePlayer03 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer03);
        public static RoleInfo zombiePlayer04 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer04);
        public static RoleInfo zombiePlayer05 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer05);
        public static RoleInfo zombiePlayer06 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer06);
        public static RoleInfo zombiePlayer07 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer07);
        public static RoleInfo zombiePlayer08 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer08);
        public static RoleInfo zombiePlayer09 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer09);
        public static RoleInfo zombiePlayer10 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer10);
        public static RoleInfo zombiePlayer11 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer11);
        public static RoleInfo zombiePlayer12 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer12);
        public static RoleInfo zombiePlayer13 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer13);
        public static RoleInfo zombiePlayer14 = new RoleInfo("Zombie", Mechanic.color, "Infect all survivors", "Infect all survivors", RoleId.ZombiePlayer14);
        
        // Impostor roles
        public static RoleInfo mimic = new RoleInfo("Mimic", Mimic.color, "Mimic other player's look", "Mimic other player's look", RoleId.Mimic);
        public static RoleInfo painter = new RoleInfo("Painter", Painter.color, "Paint players with the same color", "Paint players with the same color", RoleId.Painter);
        public static RoleInfo demon = new RoleInfo("Demon", Demon.color, "Bite players to delay their death", "Bite players to delay their death", RoleId.Demon);
        public static RoleInfo janitor = new RoleInfo("Janitor", Janitor.color, "Remove and move bodies from the crime scene", "Remove and move bodies from the crime scene", RoleId.Janitor);
        public static RoleInfo ilusionist = new RoleInfo("Ilusionist", Ilusionist.color, "Create your own vent network and turn off the lights", "Create your own vent network \nand turn off the lights", RoleId.Ilusionist);
        public static RoleInfo manipulator = new RoleInfo("Manipulator", Manipulator.color, "Manipulate a player to kill his adjacent", "Manipulate a player to kill his adjacent", RoleId.Manipulator);
        public static RoleInfo bomberman = new RoleInfo("Bomberman", Bomberman.color, "Sabotage by putting bombs", "Sabotage by putting bombs", RoleId.Bomberman);
        public static RoleInfo chameleon = new RoleInfo("Chameleon", Chameleon.color, "Make yourself invisible", "Make yourself invisible", RoleId.Chameleon);
        public static RoleInfo gambler = new RoleInfo("Gambler", Gambler.color, "Shoot a player choosing their role during the meeting", "Shoot a player choosing \ntheir role during the meeting", RoleId.Gambler);
        public static RoleInfo sorcerer = new RoleInfo("Sorcerer", Sorcerer.color, "Casts spells on players", "Casts spells on players", RoleId.Sorcerer);

        // Rebelde roles
        public static RoleInfo renegade = new RoleInfo("Renegade", Renegade.color, "Recruit a Minion and kill everyone", "Recruit a Minion and kill everyone", RoleId.Renegade, false, true);
        public static RoleInfo minion = new RoleInfo("Minion", Minion.color, "Help the Renegade killing everyone", "Help the Renegade killing everyone", RoleId.Minion, false, true);
        public static RoleInfo bountyHunter = new RoleInfo("Bounty Hunter", BountyHunter.color, "Hunt down your target" + BountyHunter.rolName, "Hunt down your target" + BountyHunter.rolName, RoleId.BountyHunter, false, true);
        public static RoleInfo trapper = new RoleInfo("Trapper", Trapper.color, "Place landmines and root traps", "Place landmines and root traps", RoleId.Trapper, false, true);
        public static RoleInfo yinyanger = new RoleInfo("Yinyanger", Yinyanger.color, "Mark two players to die if they collide", "Mark two players to die if they collide", RoleId.Yinyanger, false, true);
        public static RoleInfo challenger = new RoleInfo("Challenger", Challenger.color, "Challenge a player to a rock-paper-scissors duel", "Challenge a player to \na rock-paper-scissors duel", RoleId.Challenger, false, true);

        // Neutral roles
        public static RoleInfo joker = new RoleInfo("Joker", Joker.color, "Get voted out to win", "Get voted out to win \nOpen the map to activate the sabotage button", RoleId.Joker, true, false);
        public static RoleInfo rolethief = new RoleInfo("Role Thief", RoleThief.color, "Steal other player role", "Steal other player role", RoleId.RoleThief, true, false);
        public static RoleInfo pyromaniac = new RoleInfo("Pyromaniac", Pyromaniac.color, "Ignite all survivors to win", "Ignite all survivors to win", RoleId.Pyromaniac, true, false);
        public static RoleInfo treasureHunter = new RoleInfo("Treasure Hunter", TreasureHunter.color, "Find treasures to win", "Find treasures to win", RoleId.TreasureHunter, true, false);
        public static RoleInfo devourer = new RoleInfo("Devourer", Devourer.color, "Devour bodies to win", "Devour bodies to win", RoleId.Devourer, true, false);

        // Crewmate roles
        public static RoleInfo captain = new RoleInfo("Captain", Captain.color, "Your vote counts twice", "Your vote counts twice", RoleId.Captain);
        public static RoleInfo mechanic = new RoleInfo("Mechanic", Mechanic.color, "Repair sabotages on the ship", "Repair sabotages on the ship", RoleId.Mechanic);
        public static RoleInfo sheriff = new RoleInfo("Sheriff", Sheriff.color, "Kill the <color=#FF0000FF>Impostors</color>", "Kill the <color=#FF0000FF>Impostors</color>", RoleId.Sheriff);
        public static RoleInfo detective = new RoleInfo("Detective", Detective.color, "Examine footprints to find the <color=#FF0000FF>Impostors</color>", "Examine footprints to find the <color=#FF0000FF>Impostors</color>", RoleId.Detective);
        public static RoleInfo forensic = new RoleInfo("Forensic", Forensic.color, "Find clues reporting bodies and asking their ghosts", "Find clues reporting bodies \nand asking their ghosts", RoleId.Forensic);
        public static RoleInfo timeTraveler = new RoleInfo("Time Traveler", TimeTraveler.color, "Rewind the time", "Rewind the time", RoleId.TimeTraveler);
        public static RoleInfo squire = new RoleInfo("Squire", Squire.color, "Protect a player with your shield", "Protect a player with your shield", RoleId.Squire);
        public static RoleInfo cheater = new RoleInfo("Cheater", Cheater.color, "Swap the votes of two players", "Swap the votes of two players", RoleId.Cheater);
        public static RoleInfo fortuneTeller = new RoleInfo("Fortune Teller", FortuneTeller.color, "Reveal who are the <color=#FF0000FF>Impostors</color>", "Reveal who are the <color=#FF0000FF>Impostors</color>", RoleId.FortuneTeller);
        public static RoleInfo hacker = new RoleInfo("Hacker", Hacker.color, "Use Admin and Vitals from anywhere", "Use Admin and Vitals from anywhere", RoleId.Hacker);
        public static RoleInfo sleuth = new RoleInfo("Sleuth", Sleuth.color, "Track down a player and corpses", "Track down a player and corpses", RoleId.Sleuth);
        public static RoleInfo fink = new RoleInfo("Fink", Fink.color, "Finish your tasks to reveal the <color=#FF0000FF>Impostors</color>", "Finish your tasks to reveal the <color=#FF0000FF>Impostors</color>", RoleId.Fink);
        public static RoleInfo kid = new RoleInfo("Kid", Kid.color, "Everyone lose if you die or get voted out", "Everyone lose if you die or get voted out", RoleId.Kid);
        public static RoleInfo welder = new RoleInfo("Welder", Welder.color, "Seal vents", "Seal vents", RoleId.Welder);
        public static RoleInfo spiritualist = new RoleInfo("Spiritualist", Spiritualist.color, "Sacrifice yourself to revive a player", "Sacrifice yourself to revive a player", RoleId.Spiritualist);
        public static RoleInfo coward = new RoleInfo("Coward", Coward.color, "Call meetings from anywhere", "Call meetings from anywhere", RoleId.Coward);
        public static RoleInfo vigilant = new RoleInfo("Vigilant", Vigilant.color, "Put additional cameras on the map", "Put additional cameras on the map", RoleId.Vigilant);
        public static RoleInfo vigilantMira = new RoleInfo("Vigilant", Vigilant.color, "Activate remote Doorlog with Q key", "Activate remote Doorlog with Q key", RoleId.VigilantMira);
        public static RoleInfo medusa = new RoleInfo("Medusa", Medusa.color, "Petrify suspicious players", "Petrify suspicious players", RoleId.Medusa);
        public static RoleInfo hunter = new RoleInfo("Hunter", Hunter.color, "Mark a player to die if you get killed", "Mark a player to die if you get killed", RoleId.Hunter);
        public static RoleInfo jinx = new RoleInfo("Jinx", Jinx.color, "Jinx players abilities", "Jinx players abilities", RoleId.Jinx);
        public static RoleInfo impostor = new RoleInfo("Impostor", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "Sabotage and kill everyone"), "Sabotage and kill everyone", RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo("Crewmate", Kid.color, "Find and exile the <color=#FF0000FF>Impostors</color>", "Find and exile the <color=#FF0000FF>Impostors</color>", RoleId.Crewmate);
        public static RoleInfo lighter = new RoleInfo("Lighter", Modifiers.color, "You have more vision", "You have more vision", RoleId.Lighter);
        public static RoleInfo blind = new RoleInfo("Blind", Modifiers.color, "You have less vision", "You have less vision", RoleId.Blind);
        public static RoleInfo flash = new RoleInfo("Flash", Modifiers.color, "You're faster", "You're faster", RoleId.Flash);
        public static RoleInfo bigchungus = new RoleInfo("Big Chungus", Modifiers.color, "You're bigger and slower", "You're bigger and slower", RoleId.BigChungus);
        public static RoleInfo theChosenOne = new RoleInfo("The Chosen One", Modifiers.color, "Force your killer to report your body", "Force your killer to report your body", RoleId.TheChosenOne);
        public static RoleInfo performer = new RoleInfo("Performer", Modifiers.color, "Your death will trigger an alarm and reveal where your body is", "Your death will trigger an alarm \nand reveal where your body is", RoleId.Performer);
        public static RoleInfo lover = new RoleInfo("Lover", Modifiers.loverscolor, $"♥Survive as a couple with your partner♥", $"♥Survive as a couple with your partner♥", RoleId.Lover);
        public static RoleInfo badlover = new RoleInfo("Loverstor", Palette.ImpostorRed, $"<color=#FF00D1FF>♥Survive as a couple with your partner♥. </color><color=#FF1919FF>Kill the rest</color>", $"<color=#FF00D1FF>♥Survive as a couple with your partner♥. \n</color><color=#FF1919FF>Kill the rest</color>", RoleId.Lover);


        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>() {
            impostor,
            mimic,
            painter,
            demon,
            janitor,
            ilusionist,
            manipulator,
            bomberman,
            chameleon,
            gambler,
            sorcerer,
            renegade,
            minion,
            bountyHunter,
            trapper,
            yinyanger,
            challenger,
            joker,
            rolethief,
            pyromaniac,
            treasureHunter,
            devourer,
            crewmate,
            captain,
            mechanic,
            sheriff,
            detective,
            forensic,
            timeTraveler,
            squire,
            cheater,
            fortuneTeller,
            hacker,
            sleuth,
            fink,
            welder,
            spiritualist,
            coward,         
            vigilant,
            vigilantMira,
            kid,
            medusa,
            hunter,
            jinx,
            lighter,
            blind,
            flash,
            bigchungus,
            theChosenOne,
            performer,
            lover,
            badlover,
            redplayer01,
            redplayer02,
            redplayer03,
            redplayer04,
            redplayer05,
            redplayer06,
            redplayer07,
            blueplayer01,
            blueplayer02,
            blueplayer03,
            blueplayer04,
            blueplayer05,
            blueplayer06,
            blueplayer07,
            stealerplayer,
            policeplayer01,
            policeplayer02,
            policeplayer03,
            policeplayer04,
            policeplayer05,
            thiefplayer01,
            thiefplayer02,
            thiefplayer03,
            thiefplayer04,
            thiefplayer05,
            thiefplayer06,
            thiefplayer07,
            thiefplayer08,
            thiefplayer09,
            thiefplayer10,
            greenKing,
            greenplayer01,
            greenplayer02,
            greenplayer03,
            greenplayer04,
            greenplayer05,
            greenplayer06,
            yellowKing,
            yellowplayer01,
            yellowplayer02,
            yellowplayer03,
            yellowplayer04,
            yellowplayer05,
            yellowplayer06,
            usurperplayer,
            hotPotatoPlayer,
            notPotato01,
            notPotato02,
            notPotato03,
            notPotato04,
            notPotato05,
            notPotato06,
            notPotato07,
            notPotato08,
            notPotato09,
            notPotato10,
            notPotato11,
            notPotato12,
            notPotato13,
            notPotato14,
            explodedPotato01,
            explodedPotato02,
            explodedPotato03,
            explodedPotato04,
            explodedPotato05,
            explodedPotato06,
            explodedPotato07,
            explodedPotato08,
            explodedPotato09,
            explodedPotato10,
            explodedPotato11,
            explodedPotato12,
            explodedPotato13,
            explodedPotato14,
            nursePlayer,
            survivorPlayer01,
            survivorPlayer02,
            survivorPlayer03,
            survivorPlayer04,
            survivorPlayer05,
            survivorPlayer06,
            survivorPlayer07,
            survivorPlayer08,
            survivorPlayer09,
            survivorPlayer10,
            survivorPlayer11,
            survivorPlayer12,
            survivorPlayer13,
            zombiePlayer01,
            zombiePlayer02,
            zombiePlayer03,
            zombiePlayer04,
            zombiePlayer05,
            zombiePlayer06,
            zombiePlayer07,
            zombiePlayer08,
            zombiePlayer09,
            zombiePlayer10,
            zombiePlayer11,
            zombiePlayer12,
            zombiePlayer13,
            zombiePlayer14
        };

        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p) {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            // Capture the Flag
            if (p == CaptureTheFlag.redplayer01) infos.Add(redplayer01);
            if (p == CaptureTheFlag.redplayer02) infos.Add(redplayer02);
            if (p == CaptureTheFlag.redplayer03) infos.Add(redplayer03);
            if (p == CaptureTheFlag.redplayer04) infos.Add(redplayer04);
            if (p == CaptureTheFlag.redplayer05) infos.Add(redplayer05);
            if (p == CaptureTheFlag.redplayer06) infos.Add(redplayer06);
            if (p == CaptureTheFlag.redplayer07) infos.Add(redplayer07);
            if (p == CaptureTheFlag.blueplayer01) infos.Add(blueplayer01);
            if (p == CaptureTheFlag.blueplayer02) infos.Add(blueplayer02);
            if (p == CaptureTheFlag.blueplayer03) infos.Add(blueplayer03);
            if (p == CaptureTheFlag.blueplayer04) infos.Add(blueplayer04);
            if (p == CaptureTheFlag.blueplayer05) infos.Add(blueplayer05);
            if (p == CaptureTheFlag.blueplayer06) infos.Add(blueplayer06);
            if (p == CaptureTheFlag.blueplayer07) infos.Add(blueplayer07);
            if (p == CaptureTheFlag.stealerPlayer) infos.Add(stealerplayer);

            // Police and Thief
            if (p == PoliceAndThief.policeplayer01) infos.Add(policeplayer01);
            if (p == PoliceAndThief.policeplayer02) infos.Add(policeplayer02);
            if (p == PoliceAndThief.policeplayer03) infos.Add(policeplayer03);
            if (p == PoliceAndThief.policeplayer04) infos.Add(policeplayer04);
            if (p == PoliceAndThief.policeplayer05) infos.Add(policeplayer05);
            if (p == PoliceAndThief.thiefplayer01) infos.Add(thiefplayer01);
            if (p == PoliceAndThief.thiefplayer02) infos.Add(thiefplayer02);
            if (p == PoliceAndThief.thiefplayer03) infos.Add(thiefplayer03);
            if (p == PoliceAndThief.thiefplayer04) infos.Add(thiefplayer04);
            if (p == PoliceAndThief.thiefplayer05) infos.Add(thiefplayer05);
            if (p == PoliceAndThief.thiefplayer06) infos.Add(thiefplayer06);
            if (p == PoliceAndThief.thiefplayer07) infos.Add(thiefplayer07);
            if (p == PoliceAndThief.thiefplayer08) infos.Add(thiefplayer08);
            if (p == PoliceAndThief.thiefplayer09) infos.Add(thiefplayer09);
            if (p == PoliceAndThief.thiefplayer10) infos.Add(thiefplayer10);

            // King of the hill
            if (p == KingOfTheHill.greenKingplayer) infos.Add(greenKing);
            if (p == KingOfTheHill.greenplayer01) infos.Add(greenplayer01);
            if (p == KingOfTheHill.greenplayer02) infos.Add(greenplayer02);
            if (p == KingOfTheHill.greenplayer03) infos.Add(greenplayer03);
            if (p == KingOfTheHill.greenplayer04) infos.Add(greenplayer04);
            if (p == KingOfTheHill.greenplayer05) infos.Add(greenplayer05);
            if (p == KingOfTheHill.greenplayer06) infos.Add(greenplayer06);
            if (p == KingOfTheHill.yellowKingplayer) infos.Add(yellowKing);
            if (p == KingOfTheHill.yellowplayer01) infos.Add(yellowplayer01);
            if (p == KingOfTheHill.yellowplayer02) infos.Add(yellowplayer02);
            if (p == KingOfTheHill.yellowplayer03) infos.Add(yellowplayer03);
            if (p == KingOfTheHill.yellowplayer04) infos.Add(yellowplayer04);
            if (p == KingOfTheHill.yellowplayer05) infos.Add(yellowplayer05);
            if (p == KingOfTheHill.yellowplayer06) infos.Add(yellowplayer06);
            if (p == KingOfTheHill.usurperPlayer) infos.Add(usurperplayer);

            // Hot Potato
            if (p == HotPotato.hotPotatoPlayer) infos.Add(hotPotatoPlayer);
            if (p == HotPotato.notPotato01) infos.Add(notPotato01);
            if (p == HotPotato.notPotato02) infos.Add(notPotato02);
            if (p == HotPotato.notPotato03) infos.Add(notPotato03);
            if (p == HotPotato.notPotato04) infos.Add(notPotato04);
            if (p == HotPotato.notPotato05) infos.Add(notPotato05);
            if (p == HotPotato.notPotato06) infos.Add(notPotato06);
            if (p == HotPotato.notPotato07) infos.Add(notPotato07);
            if (p == HotPotato.notPotato08) infos.Add(notPotato08);
            if (p == HotPotato.notPotato09) infos.Add(notPotato09);
            if (p == HotPotato.notPotato10) infos.Add(notPotato10);
            if (p == HotPotato.notPotato11) infos.Add(notPotato11);
            if (p == HotPotato.notPotato12) infos.Add(notPotato12);
            if (p == HotPotato.notPotato13) infos.Add(notPotato13);
            if (p == HotPotato.notPotato14) infos.Add(notPotato14);

            if (p == HotPotato.explodedPotato01) infos.Add(explodedPotato01);
            if (p == HotPotato.explodedPotato02) infos.Add(explodedPotato02);
            if (p == HotPotato.explodedPotato03) infos.Add(explodedPotato03);
            if (p == HotPotato.explodedPotato04) infos.Add(explodedPotato04);
            if (p == HotPotato.explodedPotato05) infos.Add(explodedPotato05);
            if (p == HotPotato.explodedPotato06) infos.Add(explodedPotato06);
            if (p == HotPotato.explodedPotato07) infos.Add(explodedPotato07);
            if (p == HotPotato.explodedPotato08) infos.Add(explodedPotato08);
            if (p == HotPotato.explodedPotato09) infos.Add(explodedPotato09);
            if (p == HotPotato.explodedPotato10) infos.Add(explodedPotato10);
            if (p == HotPotato.explodedPotato11) infos.Add(explodedPotato11);
            if (p == HotPotato.explodedPotato12) infos.Add(explodedPotato12);
            if (p == HotPotato.explodedPotato13) infos.Add(explodedPotato13);
            if (p == HotPotato.explodedPotato14) infos.Add(explodedPotato14);

            // ZombieLaboratory
            if (p == ZombieLaboratory.nursePlayer) infos.Add(nursePlayer);
            if (p == ZombieLaboratory.survivorPlayer01) infos.Add(survivorPlayer01);
            if (p == ZombieLaboratory.survivorPlayer02) infos.Add(survivorPlayer02);
            if (p == ZombieLaboratory.survivorPlayer03) infos.Add(survivorPlayer03);
            if (p == ZombieLaboratory.survivorPlayer04) infos.Add(survivorPlayer04);
            if (p == ZombieLaboratory.survivorPlayer05) infos.Add(survivorPlayer05);
            if (p == ZombieLaboratory.survivorPlayer06) infos.Add(survivorPlayer06);
            if (p == ZombieLaboratory.survivorPlayer07) infos.Add(survivorPlayer07);
            if (p == ZombieLaboratory.survivorPlayer08) infos.Add(survivorPlayer08);
            if (p == ZombieLaboratory.survivorPlayer09) infos.Add(survivorPlayer09);
            if (p == ZombieLaboratory.survivorPlayer10) infos.Add(survivorPlayer10);
            if (p == ZombieLaboratory.survivorPlayer11) infos.Add(survivorPlayer11);
            if (p == ZombieLaboratory.survivorPlayer12) infos.Add(survivorPlayer12);
            if (p == ZombieLaboratory.survivorPlayer13) infos.Add(survivorPlayer13);

            if (p == ZombieLaboratory.zombiePlayer01) infos.Add(zombiePlayer01);
            if (p == ZombieLaboratory.zombiePlayer02) infos.Add(zombiePlayer02);
            if (p == ZombieLaboratory.zombiePlayer03) infos.Add(zombiePlayer03);
            if (p == ZombieLaboratory.zombiePlayer04) infos.Add(zombiePlayer04);
            if (p == ZombieLaboratory.zombiePlayer05) infos.Add(zombiePlayer05);
            if (p == ZombieLaboratory.zombiePlayer06) infos.Add(zombiePlayer06);
            if (p == ZombieLaboratory.zombiePlayer07) infos.Add(zombiePlayer07);
            if (p == ZombieLaboratory.zombiePlayer08) infos.Add(zombiePlayer08);
            if (p == ZombieLaboratory.zombiePlayer09) infos.Add(zombiePlayer09);
            if (p == ZombieLaboratory.zombiePlayer10) infos.Add(zombiePlayer10);
            if (p == ZombieLaboratory.zombiePlayer11) infos.Add(zombiePlayer11);
            if (p == ZombieLaboratory.zombiePlayer12) infos.Add(zombiePlayer12);
            if (p == ZombieLaboratory.zombiePlayer13) infos.Add(zombiePlayer13);
            if (p == ZombieLaboratory.zombiePlayer14) infos.Add(zombiePlayer14);


            // Impostor roles
            if (p == Mimic.mimic) infos.Add(mimic);
            if (p == Painter.painter) infos.Add(painter);
            if (p == Demon.demon) infos.Add(demon);
            if (p == Ilusionist.ilusionist) infos.Add(ilusionist);
            if (p == Janitor.janitor) infos.Add(janitor);
            if (p == Manipulator.manipulator) infos.Add(manipulator);
            if (p == Bomberman.bomberman) infos.Add(bomberman);
            if (p == Chameleon.chameleon) infos.Add(chameleon);
            if (p == Gambler.gambler) infos.Add(gambler);
            if (p == Sorcerer.sorcerer) infos.Add(sorcerer);

            // Rebels roles
            if (p == Renegade.renegade || (Renegade.formerRenegades != null && Renegade.formerRenegades.Any(x => x.PlayerId == p.PlayerId))) infos.Add(renegade);
            if (p == Minion.minion) infos.Add(minion);
            if (p == BountyHunter.bountyhunter) infos.Add(bountyHunter);
            if (p == Trapper.trapper) infos.Add(trapper);
            if (p == Yinyanger.yinyanger) infos.Add(yinyanger);
            if (p == Challenger.challenger) infos.Add(challenger);

            // Neutral roles
            if (p == Joker.joker) infos.Add(joker);
            if (p == RoleThief.rolethief) infos.Add(rolethief);
            if (p == Pyromaniac.pyromaniac) infos.Add(pyromaniac);
            if (p == TreasureHunter.treasureHunter) infos.Add(treasureHunter);
            if (p == Devourer.devourer) infos.Add(devourer);

            // Crewmate roles
            if (p == Captain.captain) infos.Add(captain);
            if (p == Mechanic.mechanic) infos.Add(mechanic);
            if (p == Sheriff.sheriff) infos.Add(sheriff);
            if (p == Detective.detective) infos.Add(detective);
            if (p == Forensic.forensic) infos.Add(forensic);
            if (p == TimeTraveler.timeTraveler) infos.Add(timeTraveler);
            if (p == Squire.squire) infos.Add(squire);
            if (p == Cheater.cheater) infos.Add(cheater);
            if (p == FortuneTeller.fortuneTeller) infos.Add(fortuneTeller);
            if (p == Hacker.hacker) infos.Add(hacker);
            if (p == Sleuth.sleuth) infos.Add(sleuth);
            if (p == Fink.fink) infos.Add(fink);
            if (p == Kid.kid) infos.Add(kid);
            if (p == Welder.welder) infos.Add(welder);
            if (p == Spiritualist.spiritualist) infos.Add(spiritualist);
            if (p == Coward.coward) infos.Add(coward);
            if (p == Vigilant.vigilant) infos.Add(vigilant);
            if (p == Vigilant.vigilantMira) infos.Add(vigilantMira);
            if (p == Medusa.medusa) infos.Add(medusa);
            if (p == Hunter.hunter) infos.Add(hunter);
            if (p == Jinx.jinx) infos.Add(jinx);

            // Modifier
            if (p == Modifiers.lighter) infos.Add(lighter);
            if (p == Modifiers.blind) infos.Add(blind);
            if (p == Modifiers.flash) infos.Add(flash);
            if (p == Modifiers.bigchungus) infos.Add(bigchungus);
            if (p == Modifiers.theChosenOne) infos.Add(theChosenOne);
            if (p == Modifiers.performer) infos.Add(performer);
            if (p == Modifiers.lover1 || p == Modifiers.lover2) infos.Add(p.Data.Role.IsImpostor ? badlover : lover);

            // Default roles
            if (infos.Count == 0 && p.Data.Role.IsImpostor) infos.Add(impostor); // Just Impostor
            if (infos.Count == 0 && !p.Data.Role.IsImpostor) infos.Add(crewmate); // Just Crewmate

            return infos;
        }

        public static String GetRolesString(PlayerControl p, bool useColors) {
            string roleName;
            roleName = String.Join(" ", getRoleInfoForPlayer(p).Select(x => useColors ? Helpers.cs(x.color, x.name) : x.name).ToArray());
            if (roleName.Contains("Lover")) roleName.Replace("Lover", "");
            return roleName;
        }

        public class RoleFortuneTellerInfo
        {
            public Color color;
            public string name;
            public bool isGood;

            RoleFortuneTellerInfo(Color color, string name, bool isGood) {
                this.color = color;
                this.name = name;
                this.isGood = isGood;
            }

            public static RoleFortuneTellerInfo getFortuneTellerRoleInfoForPlayer(PlayerControl p) {
                string name = "";
                bool isGood = true;
                Color color = Color.white;

                if (Captain.captain != null && p == Captain.captain) {
                    name = "Captain";
                    color = Captain.color;
                }
                else if (Mechanic.mechanic != null && p == Mechanic.mechanic) {
                    name = "Mechanic";
                    color = Mechanic.color;
                }
                else if (Sheriff.sheriff != null && p == Sheriff.sheriff) {
                    name = "Sheriff";
                    color = Sheriff.color;
                }
                else if (Detective.detective != null && p == Detective.detective) {
                    name = "Detective";
                    color = Detective.color;
                }
                else if (Forensic.forensic != null && p == Forensic.forensic) {
                    name = "Forensic";
                    color = Forensic.color;
                }
                else if (TimeTraveler.timeTraveler != null && p == TimeTraveler.timeTraveler) {
                    name = "Time Traveler";
                    color = TimeTraveler.color;
                }
                else if (Squire.squire != null && p == Squire.squire) {
                    name = "Squire";
                    color = Squire.color;
                }
                else if (Cheater.cheater != null && p == Cheater.cheater) {
                    name = "Cheater";
                    color = Cheater.color;
                }
                else if (FortuneTeller.fortuneTeller != null && p == FortuneTeller.fortuneTeller) {
                    name = "Fortune Teller";
                    color = FortuneTeller.color;
                }
                else if (Hacker.hacker != null && p == Hacker.hacker) {
                    name = "Hacker";
                    color = Hacker.color;
                }
                else if (Sleuth.sleuth != null && p == Sleuth.sleuth) {
                    name = "Sleuth";
                    color = Sleuth.color;
                }
                else if (Fink.fink != null && p == Fink.fink) {
                    name = "Fink";
                    color = Fink.color;
                }
                else if (Kid.kid != null && p == Kid.kid) {
                    name = "Kid";
                    color = Kid.color;
                }
                else if (Welder.welder != null && p == Welder.welder) {
                    name = "Welder";
                    color = Welder.color;
                }
                else if (Spiritualist.spiritualist != null && p == Spiritualist.spiritualist) {
                    name = "Spiritualist";
                    color = Spiritualist.color;
                }
                else if (Coward.coward != null && p == Coward.coward) {
                    name = "Coward";
                    color = Coward.color;
                }
                else if (Vigilant.vigilant != null && p == Vigilant.vigilant) {
                    name = "Vigilant";
                    color = Vigilant.color;
                }
                else if (Vigilant.vigilantMira != null && p == Vigilant.vigilantMira) {
                    name = "Vigilant";
                    color = Vigilant.color;
                }
                else if (Medusa.medusa != null && p == Medusa.medusa) {
                    name = "Medusa";
                    color = Medusa.color;
                }
                else if (Hunter.hunter != null && p == Hunter.hunter) {
                    name = "Hunter";
                    color = Hunter.color;
                }
                else if (Jinx.jinx != null && p == Jinx.jinx) {
                    name = "Jinx";
                    color = Jinx.color;
                }
                else if (Mimic.mimic != null && p == Mimic.mimic) {
                    name = "Mimic";
                    color = Mimic.color;
                    isGood = false;
                }
                else if (Painter.painter != null && p == Painter.painter) {
                    name = "Painter";
                    color = Painter.color;
                    isGood = false;
                }
                else if (Demon.demon != null && p == Demon.demon) {
                    name = "Demon";
                    color = Demon.color;
                    isGood = false;
                }
                else if (Ilusionist.ilusionist != null && p == Ilusionist.ilusionist) {
                    name = "Ilusionist";
                    color = Ilusionist.color;
                    isGood = false;
                }
                else if (Janitor.janitor != null && p == Janitor.janitor) {
                    name = "Janitor";
                    color = Janitor.color;
                    isGood = false;
                }
                else if (Manipulator.manipulator != null && p == Manipulator.manipulator) {
                    name = "Manipulator";
                    color = Manipulator.color;
                    isGood = false;
                }
                else if (Bomberman.bomberman != null && p == Bomberman.bomberman) {
                    name = "Bomberman";
                    color = Bomberman.color;
                    isGood = false;
                }
                else if (Chameleon.chameleon != null && p == Chameleon.chameleon) {
                    name = "Chameleon";
                    color = Palette.ImpostorRed;
                    isGood = false;
                }
                else if (Gambler.gambler != null && p == Gambler.gambler) {
                    name = "Gambler";
                    color = Palette.ImpostorRed;
                    isGood = false;
                }
                else if (Sorcerer.sorcerer != null && p == Sorcerer.sorcerer) {
                    name = "Sorcerer";
                    color = Palette.ImpostorRed;
                    isGood = false;
                }
                else if (Renegade.renegade != null && p == Renegade.renegade) {
                    name = "Renegade";
                    color = Renegade.color;
                    isGood = false;
                }
                else if (Minion.minion != null && p == Minion.minion) {
                    name = "Minion";
                    color = Minion.color;
                    isGood = false;
                }
                else if (BountyHunter.bountyhunter != null && p == BountyHunter.bountyhunter) {
                    name = "Bounty Hunter";
                    color = BountyHunter.color;
                    isGood = false;
                }
                else if (Trapper.trapper != null && p == Trapper.trapper) {
                    name = "Trapper";
                    color = Trapper.color;
                    isGood = false;
                }
                else if (Yinyanger.yinyanger != null && p == Yinyanger.yinyanger) {
                    name = "Yinyanger";
                    color = Yinyanger.color;
                    isGood = false;
                }
                else if (Challenger.challenger != null && p == Challenger.challenger) {
                    name = "Challenger";
                    color = Challenger.color;
                    isGood = false;
                }
                else if (Joker.joker != null && p == Joker.joker) {
                    name = "Joker";
                    color = Joker.color;
                    isGood = false;
                }
                else if (RoleThief.rolethief != null && p == RoleThief.rolethief) {
                    name = "Role Thief";
                    color = RoleThief.color;
                    isGood = false;
                }
                else if (Pyromaniac.pyromaniac != null && p == Pyromaniac.pyromaniac) {
                    name = "Pyromaniac";
                    color = Pyromaniac.color;
                    isGood = false;
                }
                else if (TreasureHunter.treasureHunter != null && p == TreasureHunter.treasureHunter) {
                    name = "Treasure Hunter";
                    color = TreasureHunter.color;
                    isGood = false;
                }
                else if (Devourer.devourer != null && p == Devourer.devourer) {
                    name = "Devourer";
                    color = Devourer.color;
                    isGood = false;
                }
                else if (p.Data.Role.IsImpostor) { // Just Impostor
                    name = "Impostor";
                    color = Palette.ImpostorRed;
                    isGood = false;
                }
                else { // Just Crewmate
                    name = "Crewmate";
                    color = Kid.color;
                }

                return new RoleFortuneTellerInfo(
                    color,
                    name,
                    isGood
                );
            }
        }
    }
}
