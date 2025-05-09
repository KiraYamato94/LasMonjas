using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using LasMonjas.Core;
using LasMonjas.Patches;

namespace LasMonjas
{
    class RoleInfo
    {
        public Color color;
        public string name;
        public string introDescription;
        public string shortDescription;
        public RoleId roleId;
        public readonly Team TeamId;
        public string SettingsDescription;

        RoleInfo(string name, Color color, string introDescription, string shortDescription, RoleId roleId, Team teamId = Team.Null, string settingsDescription = "") {
            this.color = color;
            this.name = name;
            this.introDescription = introDescription;
            this.shortDescription = shortDescription;
            this.roleId = roleId;
            TeamId = teamId;
            SettingsDescription = settingsDescription;
        }

        // Capture the Flag Teams
        public static RoleInfo redplayer01 = new RoleInfo(Language.roleInfoRoleNames[74], Color.red, Language.roleInfoNames[0], Language.roleInfoNames[0], RoleId.RedPlayer01);
        public static RoleInfo redplayer02 = new RoleInfo(Language.roleInfoRoleNames[74], Color.red, Language.roleInfoNames[0], Language.roleInfoNames[0], RoleId.RedPlayer02);
        public static RoleInfo redplayer03 = new RoleInfo(Language.roleInfoRoleNames[74], Color.red, Language.roleInfoNames[0], Language.roleInfoNames[0], RoleId.RedPlayer03);
        public static RoleInfo redplayer04 = new RoleInfo(Language.roleInfoRoleNames[74], Color.red, Language.roleInfoNames[0], Language.roleInfoNames[0], RoleId.RedPlayer04);
        public static RoleInfo redplayer05 = new RoleInfo(Language.roleInfoRoleNames[74], Color.red, Language.roleInfoNames[0], Language.roleInfoNames[0], RoleId.RedPlayer05);
        public static RoleInfo redplayer06 = new RoleInfo(Language.roleInfoRoleNames[74], Color.red, Language.roleInfoNames[0], Language.roleInfoNames[0], RoleId.RedPlayer06);
        public static RoleInfo redplayer07 = new RoleInfo(Language.roleInfoRoleNames[74], Color.red, Language.roleInfoNames[0], Language.roleInfoNames[0], RoleId.RedPlayer07);
        public static RoleInfo blueplayer01 = new RoleInfo(Language.roleInfoRoleNames[75], Color.blue, Language.roleInfoNames[1], Language.roleInfoNames[1], RoleId.BluePlayer01);
        public static RoleInfo blueplayer02 = new RoleInfo(Language.roleInfoRoleNames[75], Color.blue, Language.roleInfoNames[1], Language.roleInfoNames[1], RoleId.BluePlayer02);
        public static RoleInfo blueplayer03 = new RoleInfo(Language.roleInfoRoleNames[75], Color.blue, Language.roleInfoNames[1], Language.roleInfoNames[1], RoleId.BluePlayer03);
        public static RoleInfo blueplayer04 = new RoleInfo(Language.roleInfoRoleNames[75], Color.blue, Language.roleInfoNames[1], Language.roleInfoNames[1], RoleId.BluePlayer04);
        public static RoleInfo blueplayer05 = new RoleInfo(Language.roleInfoRoleNames[75], Color.blue, Language.roleInfoNames[1], Language.roleInfoNames[1], RoleId.BluePlayer05);
        public static RoleInfo blueplayer06 = new RoleInfo(Language.roleInfoRoleNames[75], Color.blue, Language.roleInfoNames[1], Language.roleInfoNames[1], RoleId.BluePlayer06);
        public static RoleInfo blueplayer07 = new RoleInfo(Language.roleInfoRoleNames[75], Color.blue, Language.roleInfoNames[1], Language.roleInfoNames[1], RoleId.BluePlayer07);
        public static RoleInfo stealerplayer = new RoleInfo(Language.roleInfoRoleNames[76], Color.grey, Language.roleInfoNames[2], Language.roleInfoNames[3], RoleId.StealerPlayer);
        public static RoleInfo captureTheFlag = new RoleInfo(Language.teamNames[2], Sheriff.color, Language.teamNames[2], Language.teamNames[2], RoleId.CaptureTheFlag, Team.Gamemode, Language.gamemodeSummaryTexts[0]);

        // Police and Thief Teams
        public static RoleInfo policeplayer01 = new RoleInfo(Language.roleInfoRoleNames[77], Color.cyan, Language.roleInfoNames[4], Language.roleInfoNames[4], RoleId.PolicePlayer01);
        public static RoleInfo policeplayer02 = new RoleInfo(Language.roleInfoRoleNames[78], Color.yellow, Language.roleInfoNames[5], Language.roleInfoNames[6], RoleId.PolicePlayer02);
        public static RoleInfo policeplayer03 = new RoleInfo(Language.roleInfoRoleNames[77], Color.cyan, Language.roleInfoNames[4], Language.roleInfoNames[4], RoleId.PolicePlayer03);
        public static RoleInfo policeplayer04 = new RoleInfo(Language.roleInfoRoleNames[78], Color.yellow, Language.roleInfoNames[5], Language.roleInfoNames[6], RoleId.PolicePlayer04);
        public static RoleInfo policeplayer05 = new RoleInfo(Language.roleInfoRoleNames[77], Color.cyan, Language.roleInfoNames[4], Language.roleInfoNames[4], RoleId.PolicePlayer05);
        public static RoleInfo policeplayer06 = new RoleInfo(Language.roleInfoRoleNames[77], Color.cyan, Language.roleInfoNames[4], Language.roleInfoNames[4], RoleId.PolicePlayer06);
        public static RoleInfo thiefplayer01 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer01);
        public static RoleInfo thiefplayer02 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer02);
        public static RoleInfo thiefplayer03 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer03);
        public static RoleInfo thiefplayer04 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer04);
        public static RoleInfo thiefplayer05 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer05);
        public static RoleInfo thiefplayer06 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer06);
        public static RoleInfo thiefplayer07 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer07);
        public static RoleInfo thiefplayer08 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer08);
        public static RoleInfo thiefplayer09 = new RoleInfo(Language.roleInfoRoleNames[79], Mechanic.color, Language.roleInfoNames[7], Language.roleInfoNames[8], RoleId.ThiefPlayer09);
        public static RoleInfo policeandThiefs = new RoleInfo(Language.teamNames[3], Coward.color, Language.teamNames[3], Language.teamNames[3], RoleId.PoliceAndThiefs, Team.Gamemode, Language.gamemodeSummaryTexts[1]);

        // King of the hill Teams
        public static RoleInfo greenKing = new RoleInfo(Language.roleInfoRoleNames[80], Color.green, Language.roleInfoNames[9], Language.roleInfoNames[9], RoleId.GreenKing);
        public static RoleInfo greenplayer01 = new RoleInfo(Language.roleInfoRoleNames[81], Color.green, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.GreenPlayer01);
        public static RoleInfo greenplayer02 = new RoleInfo(Language.roleInfoRoleNames[81], Color.green, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.GreenPlayer02);
        public static RoleInfo greenplayer03 = new RoleInfo(Language.roleInfoRoleNames[81], Color.green, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.GreenPlayer03);
        public static RoleInfo greenplayer04 = new RoleInfo(Language.roleInfoRoleNames[81], Color.green, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.GreenPlayer04);
        public static RoleInfo greenplayer05 = new RoleInfo(Language.roleInfoRoleNames[81], Color.green, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.GreenPlayer05);
        public static RoleInfo greenplayer06 = new RoleInfo(Language.roleInfoRoleNames[81], Color.green, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.GreenPlayer06);
        public static RoleInfo yellowKing = new RoleInfo(Language.roleInfoRoleNames[82], Color.yellow, Language.roleInfoNames[9], Language.roleInfoNames[9], RoleId.YellowKing);
        public static RoleInfo yellowplayer01 = new RoleInfo(Language.roleInfoRoleNames[83], Color.yellow, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.YellowPlayer01);
        public static RoleInfo yellowplayer02 = new RoleInfo(Language.roleInfoRoleNames[83], Color.yellow, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.YellowPlayer02);
        public static RoleInfo yellowplayer03 = new RoleInfo(Language.roleInfoRoleNames[83], Color.yellow, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.YellowPlayer03);
        public static RoleInfo yellowplayer04 = new RoleInfo(Language.roleInfoRoleNames[83], Color.yellow, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.YellowPlayer04);
        public static RoleInfo yellowplayer05 = new RoleInfo(Language.roleInfoRoleNames[83], Color.yellow, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.YellowPlayer05);
        public static RoleInfo yellowplayer06 = new RoleInfo(Language.roleInfoRoleNames[83], Color.yellow, Language.roleInfoNames[10], Language.roleInfoNames[10], RoleId.YellowPlayer06);
        public static RoleInfo usurperplayer = new RoleInfo(Language.roleInfoRoleNames[84], Color.grey, Language.roleInfoNames[11], Language.roleInfoNames[11], RoleId.UsurperPlayer);
        public static RoleInfo kingOfTheHill = new RoleInfo(Language.teamNames[4], Squire.color, Language.teamNames[4], Language.teamNames[4], RoleId.KingOfTheHill, Team.Gamemode, Language.gamemodeSummaryTexts[2]);

        // Hot Potato Teams
        public static RoleInfo hotPotatoPlayer = new RoleInfo(Language.roleInfoRoleNames[85], Color.grey, Language.roleInfoNames[12], Language.roleInfoNames[13], RoleId.HotPotato);
        public static RoleInfo notPotato01 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato01);
        public static RoleInfo notPotato02 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato02);
        public static RoleInfo notPotato03 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato03);
        public static RoleInfo notPotato04 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato04);
        public static RoleInfo notPotato05 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato05);
        public static RoleInfo notPotato06 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato06);
        public static RoleInfo notPotato07 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato07);
        public static RoleInfo notPotato08 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato08);
        public static RoleInfo notPotato09 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato09);
        public static RoleInfo notPotato10 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato10);
        public static RoleInfo notPotato11 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato11);
        public static RoleInfo notPotato12 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato12);
        public static RoleInfo notPotato13 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato13);
        public static RoleInfo notPotato14 = new RoleInfo(Language.roleInfoRoleNames[86], Color.cyan, Language.roleInfoNames[14], Language.roleInfoNames[14], RoleId.NotPotato14);
        public static RoleInfo explodedPotato01 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato01);
        public static RoleInfo explodedPotato02 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato02);
        public static RoleInfo explodedPotato03 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato03);
        public static RoleInfo explodedPotato04 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato04);
        public static RoleInfo explodedPotato05 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato05);
        public static RoleInfo explodedPotato06 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato06);
        public static RoleInfo explodedPotato07 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato07);
        public static RoleInfo explodedPotato08 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato08);
        public static RoleInfo explodedPotato09 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato09);
        public static RoleInfo explodedPotato10 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato10);
        public static RoleInfo explodedPotato11 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato11);
        public static RoleInfo explodedPotato12 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato12);
        public static RoleInfo explodedPotato13 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato13);
        public static RoleInfo explodedPotato14 = new RoleInfo(Language.roleInfoRoleNames[87], Mechanic.color, Language.roleInfoNames[15], Language.roleInfoNames[15], RoleId.ExplodedPotato14);
        public static RoleInfo hotPotatoMode = new RoleInfo(Language.teamNames[5], Locksmith.color, Language.teamNames[5], Language.teamNames[5], RoleId.HotPotatoMode, Team.Gamemode, Language.gamemodeSummaryTexts[3]);

        // ZombieLaboratory Teams
        public static RoleInfo nursePlayer = new RoleInfo(Language.roleInfoRoleNames[88], Locksmith.color, Language.roleInfoNames[16], Language.roleInfoNames[17], RoleId.NursePlayer);
        public static RoleInfo survivorPlayer01 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer01);
        public static RoleInfo survivorPlayer02 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer02);
        public static RoleInfo survivorPlayer03 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer03);
        public static RoleInfo survivorPlayer04 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer04);
        public static RoleInfo survivorPlayer05 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer05);
        public static RoleInfo survivorPlayer06 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer06);
        public static RoleInfo survivorPlayer07 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer07);
        public static RoleInfo survivorPlayer08 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer08);
        public static RoleInfo survivorPlayer09 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer09);
        public static RoleInfo survivorPlayer10 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer10);
        public static RoleInfo survivorPlayer11 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer11);
        public static RoleInfo survivorPlayer12 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer12);
        public static RoleInfo survivorPlayer13 = new RoleInfo(Language.roleInfoRoleNames[89], Color.cyan, Language.roleInfoNames[18], Language.roleInfoNames[19], RoleId.SurvivorPlayer13);
        public static RoleInfo zombiePlayer01 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer01);
        public static RoleInfo zombiePlayer02 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer02);
        public static RoleInfo zombiePlayer03 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer03);
        public static RoleInfo zombiePlayer04 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer04);
        public static RoleInfo zombiePlayer05 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer05);
        public static RoleInfo zombiePlayer06 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer06);
        public static RoleInfo zombiePlayer07 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer07);
        public static RoleInfo zombiePlayer08 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer08);
        public static RoleInfo zombiePlayer09 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer09);
        public static RoleInfo zombiePlayer10 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer10);
        public static RoleInfo zombiePlayer11 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer11);
        public static RoleInfo zombiePlayer12 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer12);
        public static RoleInfo zombiePlayer13 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer13);
        public static RoleInfo zombiePlayer14 = new RoleInfo(Language.roleInfoRoleNames[90], Mechanic.color, Language.roleInfoNames[20], Language.roleInfoNames[20], RoleId.ZombiePlayer14);
        public static RoleInfo zombieLaboratory = new RoleInfo(Language.teamNames[6], Hunter.color, Language.teamNames[6], Language.teamNames[6], RoleId.ZombieLaboratory, Team.Gamemode, Language.gamemodeSummaryTexts[4]);

        // Battle Royale
        public static RoleInfo soloPlayer01 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer01);
        public static RoleInfo soloPlayer02 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer02);
        public static RoleInfo soloPlayer03 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer03);
        public static RoleInfo soloPlayer04 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer04);
        public static RoleInfo soloPlayer05 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer05);
        public static RoleInfo soloPlayer06 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer06);
        public static RoleInfo soloPlayer07 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer07);
        public static RoleInfo soloPlayer08 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer08);
        public static RoleInfo soloPlayer09 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer09);
        public static RoleInfo soloPlayer10 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer10);
        public static RoleInfo soloPlayer11 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer11);
        public static RoleInfo soloPlayer12 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer12);
        public static RoleInfo soloPlayer13 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer13);
        public static RoleInfo soloPlayer14 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer14);
        public static RoleInfo soloPlayer15 = new RoleInfo(Language.roleInfoRoleNames[91], Sleuth.color, Language.roleInfoNames[21], Language.roleInfoNames[21], RoleId.SoloPlayer15);

        public static RoleInfo limePlayer01 = new RoleInfo(Language.roleInfoRoleNames[92], FortuneTeller.color, Language.roleInfoNames[22], Language.roleInfoNames[22], RoleId.LimePlayer01);
        public static RoleInfo limePlayer02 = new RoleInfo(Language.roleInfoRoleNames[92], FortuneTeller.color, Language.roleInfoNames[22], Language.roleInfoNames[22], RoleId.LimePlayer02);
        public static RoleInfo limePlayer03 = new RoleInfo(Language.roleInfoRoleNames[92], FortuneTeller.color, Language.roleInfoNames[22], Language.roleInfoNames[22], RoleId.LimePlayer03);
        public static RoleInfo limePlayer04 = new RoleInfo(Language.roleInfoRoleNames[92], FortuneTeller.color, Language.roleInfoNames[22], Language.roleInfoNames[22], RoleId.LimePlayer04);
        public static RoleInfo limePlayer05 = new RoleInfo(Language.roleInfoRoleNames[92], FortuneTeller.color, Language.roleInfoNames[22], Language.roleInfoNames[22], RoleId.LimePlayer05);
        public static RoleInfo limePlayer06 = new RoleInfo(Language.roleInfoRoleNames[92], FortuneTeller.color, Language.roleInfoNames[22], Language.roleInfoNames[22], RoleId.LimePlayer06);
        public static RoleInfo limePlayer07 = new RoleInfo(Language.roleInfoRoleNames[92], FortuneTeller.color, Language.roleInfoNames[22], Language.roleInfoNames[22], RoleId.LimePlayer07);
        public static RoleInfo pinkPlayer01 = new RoleInfo(Language.roleInfoRoleNames[93], Locksmith.color, Language.roleInfoNames[23], Language.roleInfoNames[23], RoleId.PinkPlayer01);
        public static RoleInfo pinkPlayer02 = new RoleInfo(Language.roleInfoRoleNames[93], Locksmith.color, Language.roleInfoNames[23], Language.roleInfoNames[23], RoleId.PinkPlayer02);
        public static RoleInfo pinkPlayer03 = new RoleInfo(Language.roleInfoRoleNames[93], Locksmith.color, Language.roleInfoNames[23], Language.roleInfoNames[23], RoleId.PinkPlayer03);
        public static RoleInfo pinkPlayer04 = new RoleInfo(Language.roleInfoRoleNames[93], Locksmith.color, Language.roleInfoNames[23], Language.roleInfoNames[23], RoleId.PinkPlayer04);
        public static RoleInfo pinkPlayer05 = new RoleInfo(Language.roleInfoRoleNames[93], Locksmith.color, Language.roleInfoNames[23], Language.roleInfoNames[23], RoleId.PinkPlayer05);
        public static RoleInfo pinkPlayer06 = new RoleInfo(Language.roleInfoRoleNames[93], Locksmith.color, Language.roleInfoNames[23], Language.roleInfoNames[23], RoleId.PinkPlayer06);
        public static RoleInfo pinkPlayer07 = new RoleInfo(Language.roleInfoRoleNames[93], Locksmith.color, Language.roleInfoNames[23], Language.roleInfoNames[23], RoleId.PinkPlayer07);
        public static RoleInfo serialKiller = new RoleInfo(Language.roleInfoRoleNames[94], Joker.color, Language.helpersTexts[1], Language.helpersTexts[1], RoleId.SerialKiller);
        public static RoleInfo battleRoyale = new RoleInfo(Language.teamNames[7], Sleuth.color, Language.teamNames[7], Language.teamNames[7], RoleId.BattleRoyale, Team.Gamemode, Language.gamemodeSummaryTexts[5]);

        // Monja Festival
        public static RoleInfo greenMonjaPlayer01 = new RoleInfo(Language.roleInfoRoleNames[95], Color.green, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.GreenMonjaPlayer01);
        public static RoleInfo greenMonjaPlayer02 = new RoleInfo(Language.roleInfoRoleNames[95], Color.green, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.GreenMonjaPlayer02);
        public static RoleInfo greenMonjaPlayer03 = new RoleInfo(Language.roleInfoRoleNames[95], Color.green, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.GreenMonjaPlayer03);
        public static RoleInfo greenMonjaPlayer04 = new RoleInfo(Language.roleInfoRoleNames[95], Color.green, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.GreenMonjaPlayer04);
        public static RoleInfo greenMonjaPlayer05 = new RoleInfo(Language.roleInfoRoleNames[95], Color.green, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.GreenMonjaPlayer05);
        public static RoleInfo greenMonjaPlayer06 = new RoleInfo(Language.roleInfoRoleNames[95], Color.green, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.GreenMonjaPlayer06);
        public static RoleInfo greenMonjaPlayer07 = new RoleInfo(Language.roleInfoRoleNames[95], Color.green, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.GreenMonjaPlayer07);
        public static RoleInfo cyanPlayer01 = new RoleInfo(Language.roleInfoRoleNames[95], Color.cyan, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.CyanPlayer01);
        public static RoleInfo cyanPlayer02 = new RoleInfo(Language.roleInfoRoleNames[95], Color.cyan, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.CyanPlayer02);
        public static RoleInfo cyanPlayer03 = new RoleInfo(Language.roleInfoRoleNames[95], Color.cyan, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.CyanPlayer03);
        public static RoleInfo cyanPlayer04 = new RoleInfo(Language.roleInfoRoleNames[95], Color.cyan, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.CyanPlayer04);
        public static RoleInfo cyanPlayer05 = new RoleInfo(Language.roleInfoRoleNames[95], Color.cyan, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.CyanPlayer05);
        public static RoleInfo cyanPlayer06 = new RoleInfo(Language.roleInfoRoleNames[95], Color.cyan, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.CyanPlayer06);
        public static RoleInfo cyanPlayer07 = new RoleInfo(Language.roleInfoRoleNames[95], Color.cyan, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.CyanPlayer07);
        public static RoleInfo bigMonja = new RoleInfo(Language.roleInfoRoleNames[96], Joker.color, Language.roleInfoNames[117], Language.roleInfoNames[117], RoleId.BigMonja);
        public static RoleInfo monjaFestival = new RoleInfo(Language.teamNames[8], Monja.color, Language.teamNames[8], Language.teamNames[8], RoleId.MonjaFestival, Team.Gamemode, Language.gamemodeSummaryTexts[6]);


        // Impostor roles
        public static RoleInfo mimic = new RoleInfo(Language.roleInfoRoleNames[0], Mimic.color, Language.roleInfoNames[24], Language.roleInfoNames[24], RoleId.Mimic, Team.Impostor, Language.impSummaryTexts[0]);
        public static RoleInfo painter = new RoleInfo(Language.roleInfoRoleNames[1], Painter.color, Language.roleInfoNames[25], Language.roleInfoNames[25], RoleId.Painter, Team.Impostor, Language.impSummaryTexts[1]);
        public static RoleInfo demon = new RoleInfo(Language.roleInfoRoleNames[2], Demon.color, Language.roleInfoNames[26], Language.roleInfoNames[26], RoleId.Demon, Team.Impostor, Language.impSummaryTexts[2]);
        public static RoleInfo janitor = new RoleInfo(Language.roleInfoRoleNames[3], Janitor.color, Language.roleInfoNames[27], Language.roleInfoNames[28], RoleId.Janitor, Team.Impostor, Language.impSummaryTexts[3]);
        public static RoleInfo illusionist = new RoleInfo(Language.roleInfoRoleNames[4], Illusionist.color, Language.roleInfoNames[29], Language.roleInfoNames[30], RoleId.Illusionist, Team.Impostor, Language.impSummaryTexts[4]);
        public static RoleInfo manipulator = new RoleInfo(Language.roleInfoRoleNames[5], Manipulator.color, Language.roleInfoNames[31], Language.roleInfoNames[31], RoleId.Manipulator, Team.Impostor, Language.impSummaryTexts[5]);
        public static RoleInfo bomberman = new RoleInfo(Language.roleInfoRoleNames[6], Bomberman.color, Language.roleInfoNames[32], Language.roleInfoNames[32], RoleId.Bomberman, Team.Impostor, Language.impSummaryTexts[6]);
        public static RoleInfo chameleon = new RoleInfo(Language.roleInfoRoleNames[7], Chameleon.color, Language.roleInfoNames[33], Language.roleInfoNames[33], RoleId.Chameleon, Team.Impostor, Language.impSummaryTexts[7]);
        public static RoleInfo gambler = new RoleInfo(Language.roleInfoRoleNames[8], Gambler.color, Language.roleInfoNames[34], Language.roleInfoNames[35], RoleId.Gambler, Team.Impostor, Language.impSummaryTexts[8]);
        public static RoleInfo sorcerer = new RoleInfo(Language.roleInfoRoleNames[9], Sorcerer.color, Language.roleInfoNames[36], Language.roleInfoNames[36], RoleId.Sorcerer, Team.Impostor, Language.impSummaryTexts[9]);
        public static RoleInfo medusa = new RoleInfo(Language.roleInfoRoleNames[10], Medusa.color, Language.roleInfoNames[37], Language.roleInfoNames[37], RoleId.Medusa, Team.Impostor, Language.impSummaryTexts[10]);
        public static RoleInfo hypnotist = new RoleInfo(Language.roleInfoRoleNames[11], Hypnotist.color, Language.roleInfoNames[38], Language.roleInfoNames[38], RoleId.Hypnotist, Team.Impostor, Language.impSummaryTexts[11]);
        public static RoleInfo archer = new RoleInfo(Language.roleInfoRoleNames[12], Archer.color, Language.roleInfoNames[39], Language.roleInfoNames[40], RoleId.Archer, Team.Impostor, Language.impSummaryTexts[12]);
        public static RoleInfo plumber = new RoleInfo(Language.roleInfoRoleNames[13], Plumber.color, Language.roleInfoNames[41], Language.roleInfoNames[41], RoleId.Plumber, Team.Impostor, Language.impSummaryTexts[13]);
        public static RoleInfo librarian = new RoleInfo(Language.roleInfoRoleNames[14], Librarian.color, Language.roleInfoNames[42], Language.roleInfoNames[43], RoleId.Librarian, Team.Impostor, Language.impSummaryTexts[14]);

        // Rebelde roles
        public static RoleInfo renegade = new RoleInfo(Language.roleInfoRoleNames[15], Renegade.color, Language.roleInfoNames[44], Language.roleInfoNames[44], RoleId.Renegade, Team.Rebel, Language.rebelSummaryTexts[0]);
        public static RoleInfo minion = new RoleInfo(Language.roleInfoRoleNames[16], Minion.color, Language.roleInfoNames[45], Language.roleInfoNames[45], RoleId.Minion);
        public static RoleInfo bountyHunter = new RoleInfo(Language.roleInfoRoleNames[17], BountyHunter.color, Language.roleInfoNames[46], Language.roleInfoNames[46], RoleId.BountyHunter, Team.Rebel, Language.rebelSummaryTexts[1]);
        public static RoleInfo trapper = new RoleInfo(Language.roleInfoRoleNames[18], Trapper.color, Language.roleInfoNames[47], Language.roleInfoNames[47], RoleId.Trapper, Team.Rebel, Language.rebelSummaryTexts[2]);
        public static RoleInfo yinyanger = new RoleInfo(Language.roleInfoRoleNames[19], Yinyanger.color, Language.roleInfoNames[48], Language.roleInfoNames[49], RoleId.Yinyanger, Team.Rebel, Language.rebelSummaryTexts[3]);
        public static RoleInfo challenger = new RoleInfo(Language.roleInfoRoleNames[20], Challenger.color, Language.roleInfoNames[50], Language.roleInfoNames[51], RoleId.Challenger, Team.Rebel, Language.rebelSummaryTexts[4]);
        public static RoleInfo ninja = new RoleInfo(Language.roleInfoRoleNames[21], Ninja.color, Language.roleInfoNames[52], Language.roleInfoNames[52], RoleId.Ninja, Team.Rebel, Language.rebelSummaryTexts[5]);
        public static RoleInfo berserker = new RoleInfo(Language.roleInfoRoleNames[22], Berserker.color, Language.roleInfoNames[53], Language.roleInfoNames[53], RoleId.Berserker, Team.Rebel, Language.rebelSummaryTexts[6]);
        public static RoleInfo yandere = new RoleInfo(Language.roleInfoRoleNames[23], Yandere.color, Language.roleInfoNames[54], Language.roleInfoNames[54], RoleId.Yandere, Team.Rebel, Language.rebelSummaryTexts[7]);
        public static RoleInfo stranded = new RoleInfo(Language.roleInfoRoleNames[24], Stranded.color, Language.roleInfoNames[55], Language.roleInfoNames[55], RoleId.Stranded, Team.Rebel, Language.rebelSummaryTexts[8]);
        public static RoleInfo monja = new RoleInfo(Language.roleInfoRoleNames[25], Monja.color, Language.roleInfoNames[56], Language.roleInfoNames[57], RoleId.Monja, Team.Rebel, Language.rebelSummaryTexts[9]);

        // Neutral roles
        public static RoleInfo joker = new RoleInfo(Language.roleInfoRoleNames[26], Joker.color, Language.roleInfoNames[58], Language.roleInfoNames[59], RoleId.Joker, Team.Neutral, Language.neutralSummaryTexts[0]);
        public static RoleInfo rolethief = new RoleInfo(Language.roleInfoRoleNames[27], RoleThief.color, Language.roleInfoNames[60], Language.roleInfoNames[60], RoleId.RoleThief, Team.Neutral, Language.neutralSummaryTexts[1]);
        public static RoleInfo pyromaniac = new RoleInfo(Language.roleInfoRoleNames[28], Pyromaniac.color, Language.roleInfoNames[61], Language.roleInfoNames[61], RoleId.Pyromaniac, Team.Neutral, Language.neutralSummaryTexts[2]);
        public static RoleInfo treasureHunter = new RoleInfo(Language.roleInfoRoleNames[29], TreasureHunter.color, Language.roleInfoNames[62], Language.roleInfoNames[62], RoleId.TreasureHunter, Team.Neutral, Language.neutralSummaryTexts[3]);
        public static RoleInfo devourer = new RoleInfo(Language.roleInfoRoleNames[30], Devourer.color, Language.roleInfoNames[63], Language.roleInfoNames[63], RoleId.Devourer, Team.Neutral, Language.neutralSummaryTexts[4]);
        public static RoleInfo poisoner = new RoleInfo(Language.roleInfoRoleNames[31], Poisoner.color, Language.roleInfoNames[64], Language.roleInfoNames[65], RoleId.Poisoner, Team.Neutral, Language.neutralSummaryTexts[5]);
        public static RoleInfo puppeteer = new RoleInfo(Language.roleInfoRoleNames[32], Puppeteer.color, Language.roleInfoNames[66], Language.roleInfoNames[67], RoleId.Puppeteer, Team.Neutral, Language.neutralSummaryTexts[6]);
        public static RoleInfo exiler = new RoleInfo(Language.roleInfoRoleNames[33], Exiler.color, Language.roleInfoNames[68], Language.roleInfoNames[68], RoleId.Exiler, Team.Neutral, Language.neutralSummaryTexts[7]);
        public static RoleInfo amnesiac = new RoleInfo(Language.roleInfoRoleNames[34], Amnesiac.color, Language.roleInfoNames[69], Language.roleInfoNames[69], RoleId.Amnesiac, Team.Neutral, Language.neutralSummaryTexts[8]);
        public static RoleInfo seeker = new RoleInfo(Language.roleInfoRoleNames[35], Seeker.color, Language.roleInfoNames[70], Language.roleInfoNames[70], RoleId.Seeker, Team.Neutral, Language.neutralSummaryTexts[9]);

        // Crewmate roles
        public static RoleInfo captain = new RoleInfo(Language.roleInfoRoleNames[36], Captain.color, Language.roleInfoNames[71], Language.roleInfoNames[71], RoleId.Captain, Team.Crewmate, Language.crewSummaryTexts[0]);
        public static RoleInfo mechanic = new RoleInfo(Language.roleInfoRoleNames[37], Mechanic.color, Language.roleInfoNames[72], Language.roleInfoNames[72], RoleId.Mechanic, Team.Crewmate, Language.crewSummaryTexts[1]);
        public static RoleInfo sheriff = new RoleInfo(Language.roleInfoRoleNames[38], Sheriff.color, Language.roleInfoNames[73], Language.roleInfoNames[73], RoleId.Sheriff, Team.Crewmate, Language.crewSummaryTexts[2]);
        public static RoleInfo detective = new RoleInfo(Language.roleInfoRoleNames[39], Detective.color, Language.roleInfoNames[74], Language.roleInfoNames[74], RoleId.Detective, Team.Crewmate, Language.crewSummaryTexts[3]);
        public static RoleInfo forensic = new RoleInfo(Language.roleInfoRoleNames[40], Forensic.color, Language.roleInfoNames[75], Language.roleInfoNames[76], RoleId.Forensic, Team.Crewmate, Language.crewSummaryTexts[4]);
        public static RoleInfo timeTraveler = new RoleInfo(Language.roleInfoRoleNames[41], TimeTraveler.color, Language.roleInfoNames[77], Language.roleInfoNames[77], RoleId.TimeTraveler, Team.Crewmate, Language.crewSummaryTexts[5]);
        public static RoleInfo squire = new RoleInfo(Language.roleInfoRoleNames[42], Squire.color, Language.roleInfoNames[78], Language.roleInfoNames[78], RoleId.Squire, Team.Crewmate, Language.crewSummaryTexts[6]);
        public static RoleInfo cheater = new RoleInfo(Language.roleInfoRoleNames[43], Cheater.color, Language.roleInfoNames[79], Language.roleInfoNames[79], RoleId.Cheater, Team.Crewmate, Language.crewSummaryTexts[7]);
        public static RoleInfo fortuneTeller = new RoleInfo(Language.roleInfoRoleNames[44], FortuneTeller.color, Language.roleInfoNames[80], Language.roleInfoNames[80], RoleId.FortuneTeller, Team.Crewmate, Language.crewSummaryTexts[8]);
        public static RoleInfo hacker = new RoleInfo(Language.roleInfoRoleNames[45], Hacker.color, Language.roleInfoNames[81], Language.roleInfoNames[81], RoleId.Hacker, Team.Crewmate, Language.crewSummaryTexts[9]);
        public static RoleInfo sleuth = new RoleInfo(Language.roleInfoRoleNames[46], Sleuth.color, Language.roleInfoNames[82], Language.roleInfoNames[82], RoleId.Sleuth, Team.Crewmate, Language.crewSummaryTexts[10]);
        public static RoleInfo fink = new RoleInfo(Language.roleInfoRoleNames[47], Fink.color, Language.roleInfoNames[83], Language.roleInfoNames[83], RoleId.Fink, Team.Crewmate, Language.crewSummaryTexts[11]);
        public static RoleInfo kid = new RoleInfo(Language.roleInfoRoleNames[48], Kid.color, Language.roleInfoNames[84], Language.roleInfoNames[84], RoleId.Kid, Team.Crewmate, Language.crewSummaryTexts[12]);
        public static RoleInfo welder = new RoleInfo(Language.roleInfoRoleNames[49], Welder.color, Language.roleInfoNames[85], Language.roleInfoNames[85], RoleId.Welder, Team.Crewmate, Language.crewSummaryTexts[13]);
        public static RoleInfo spiritualist = new RoleInfo(Language.roleInfoRoleNames[50], Spiritualist.color, Language.roleInfoNames[86], Language.roleInfoNames[86], RoleId.Spiritualist, Team.Crewmate, Language.crewSummaryTexts[14]);
        public static RoleInfo coward = new RoleInfo(Language.roleInfoRoleNames[51], Coward.color, Language.roleInfoNames[87], Language.roleInfoNames[87], RoleId.Coward, Team.Crewmate, Language.crewSummaryTexts[18]);
        public static RoleInfo vigilant = new RoleInfo(Language.roleInfoRoleNames[52], Vigilant.color, Language.roleInfoNames[88], Language.roleInfoNames[88], RoleId.Vigilant, Team.Crewmate, Language.crewSummaryTexts[15]);
        public static RoleInfo vigilantMira = new RoleInfo(Language.roleInfoRoleNames[52], Vigilant.color, Language.roleInfoNames[89], Language.roleInfoNames[89], RoleId.VigilantMira);
        public static RoleInfo hunter = new RoleInfo(Language.roleInfoRoleNames[53], Hunter.color, Language.roleInfoNames[90], Language.roleInfoNames[90], RoleId.Hunter, Team.Crewmate, Language.crewSummaryTexts[16]);
        public static RoleInfo jinx = new RoleInfo(Language.roleInfoRoleNames[54], Jinx.color, Language.roleInfoNames[91], Language.roleInfoNames[91], RoleId.Jinx, Team.Crewmate, Language.crewSummaryTexts[17]);
        public static RoleInfo bat = new RoleInfo(Language.roleInfoRoleNames[55], Bat.color, Language.roleInfoNames[92], Language.roleInfoNames[93], RoleId.Bat, Team.Crewmate, Language.crewSummaryTexts[19]);
        public static RoleInfo necromancer = new RoleInfo(Language.roleInfoRoleNames[56], Necromancer.color, Language.roleInfoNames[94], Language.roleInfoNames[94], RoleId.Necromancer, Team.Crewmate, Language.crewSummaryTexts[20]);
        public static RoleInfo engineer = new RoleInfo(Language.roleInfoRoleNames[57], Engineer.color, Language.roleInfoNames[95], Language.roleInfoNames[96], RoleId.Engineer, Team.Crewmate, Language.crewSummaryTexts[21]);
        public static RoleInfo locksmith = new RoleInfo(Language.roleInfoRoleNames[58], Locksmith.color, Language.roleInfoNames[97], Language.roleInfoNames[97], RoleId.Locksmith, Team.Crewmate, Language.crewSummaryTexts[22]);
        public static RoleInfo taskMaster = new RoleInfo(Language.roleInfoRoleNames[59], TaskMaster.color, Language.roleInfoNames[98], Language.roleInfoNames[99], RoleId.TaskMaster, Team.Crewmate, Language.crewSummaryTexts[23]);
        public static RoleInfo jailer = new RoleInfo(Language.roleInfoRoleNames[60], Jailer.color, Language.roleInfoNames[100], Language.roleInfoNames[100], RoleId.Jailer, Team.Crewmate, Language.crewSummaryTexts[24]);
        public static RoleInfo impostor = new RoleInfo(Language.roleInfoRoleNames[61], Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, Language.roleInfoNames[101]), Language.roleInfoNames[101], RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo(Language.roleInfoRoleNames[62], Kid.color, Language.roleInfoNames[102], Language.roleInfoNames[102], RoleId.Crewmate);
        public static RoleInfo lighter = new RoleInfo(Language.roleInfoRoleNames[63], Modifiers.color, Language.roleInfoNames[103], Language.roleInfoNames[103], RoleId.Lighter, Team.Modifier, Language.modifierSummaryTexts[2]);
        public static RoleInfo blind = new RoleInfo(Language.roleInfoRoleNames[64], Modifiers.color, Language.roleInfoNames[104], Language.roleInfoNames[104], RoleId.Blind, Team.Modifier, Language.modifierSummaryTexts[3]);
        public static RoleInfo flash = new RoleInfo(Language.roleInfoRoleNames[65], Modifiers.color, Language.roleInfoNames[105], Language.roleInfoNames[105], RoleId.Flash, Team.Modifier, Language.modifierSummaryTexts[4]);
        public static RoleInfo bigchungus = new RoleInfo(Language.roleInfoRoleNames[66], Modifiers.color, Language.roleInfoNames[106], Language.roleInfoNames[106], RoleId.BigChungus, Team.Modifier, Language.modifierSummaryTexts[5]);
        public static RoleInfo theChosenOne = new RoleInfo(Language.roleInfoRoleNames[67], Modifiers.color, Language.roleInfoNames[107], Language.roleInfoNames[107], RoleId.TheChosenOne, Team.Modifier, Language.modifierSummaryTexts[6]);
        public static RoleInfo performer = new RoleInfo(Language.roleInfoRoleNames[68], Modifiers.color, Language.roleInfoNames[108], Language.roleInfoNames[109], RoleId.Performer, Team.Modifier, Language.modifierSummaryTexts[7]);
        public static RoleInfo pro = new RoleInfo(Language.roleInfoRoleNames[69], Modifiers.color, Language.roleInfoNames[110], Language.roleInfoNames[110], RoleId.Pro, Team.Modifier, Language.modifierSummaryTexts[8]);
        public static RoleInfo paintball = new RoleInfo(Language.roleInfoRoleNames[70], Modifiers.color, Language.roleInfoNames[111], Language.roleInfoNames[111], RoleId.Paintball, Team.Modifier, Language.modifierSummaryTexts[9]);
        public static RoleInfo electrician = new RoleInfo(Language.roleInfoRoleNames[71], Modifiers.color, Language.roleInfoNames[112], Language.roleInfoNames[112], RoleId.Electrician, Team.Modifier, Language.modifierSummaryTexts[10]);
        public static RoleInfo lover = new RoleInfo(Language.roleInfoRoleNames[72], Modifiers.loverscolor, $"{Language.roleInfoNames[113]}", $"{Language.roleInfoNames[114]}", RoleId.Lover, Team.Modifier, Language.modifierSummaryTexts[1]);
        public static RoleInfo badlover = new RoleInfo(Language.roleInfoRoleNames[73], Palette.ImpostorRed, $"{Language.roleInfoNames[115]}", $"{Language.roleInfoNames[116]}", RoleId.Lover);


        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>() {
            impostor,
            mimic,
            painter,
            demon,
            janitor,
            illusionist,
            manipulator,
            bomberman,
            chameleon,
            gambler,
            sorcerer,
            medusa,
            hypnotist,
            archer,
            plumber,
            librarian,
            renegade,
            minion,
            bountyHunter,
            trapper,
            yinyanger,
            challenger,
            ninja,
            berserker,
            yandere,
            stranded,
            monja,
            joker,
            rolethief,
            pyromaniac,
            treasureHunter,
            devourer,
            poisoner,
            puppeteer,
            exiler,
            amnesiac,
            seeker,
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
            hunter,
            jinx,
            bat,
            necromancer,
            engineer,
            locksmith,
            taskMaster,
            jailer,
            lighter,
            blind,
            flash,
            bigchungus,
            theChosenOne,
            performer,
            paintball,
            electrician,
            pro,
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
            policeplayer06,
            thiefplayer01,
            thiefplayer02,
            thiefplayer03,
            thiefplayer04,
            thiefplayer05,
            thiefplayer06,
            thiefplayer07,
            thiefplayer08,
            thiefplayer09,
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
            zombiePlayer14,
            soloPlayer01,
            soloPlayer02,
            soloPlayer03,
            soloPlayer04,
            soloPlayer05,
            soloPlayer06,
            soloPlayer07,
            soloPlayer08,
            soloPlayer09,
            soloPlayer10,
            soloPlayer11,
            soloPlayer12,
            soloPlayer13,
            soloPlayer14,
            soloPlayer15,
            limePlayer01,
            limePlayer02,
            limePlayer03,
            limePlayer04,
            limePlayer05,
            limePlayer06,
            limePlayer07,
            pinkPlayer01,
            pinkPlayer02,
            pinkPlayer03,
            pinkPlayer04,
            pinkPlayer05,
            pinkPlayer06,
            pinkPlayer07,
            serialKiller,
            greenMonjaPlayer01,
            greenMonjaPlayer02,
            greenMonjaPlayer03,
            greenMonjaPlayer04,
            greenMonjaPlayer05,
            greenMonjaPlayer06,
            greenMonjaPlayer07,
            cyanPlayer01,
            cyanPlayer02,
            cyanPlayer03,
            cyanPlayer04,
            cyanPlayer05,
            cyanPlayer06,
            cyanPlayer07,
            bigMonja,
            captureTheFlag,
            policeandThiefs,
            kingOfTheHill,
            hotPotatoMode,
            zombieLaboratory,
            battleRoyale,
            monjaFestival
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
            if (p == PoliceAndThief.policeplayer06) infos.Add(policeplayer06);
            if (p == PoliceAndThief.thiefplayer01) infos.Add(thiefplayer01);
            if (p == PoliceAndThief.thiefplayer02) infos.Add(thiefplayer02);
            if (p == PoliceAndThief.thiefplayer03) infos.Add(thiefplayer03);
            if (p == PoliceAndThief.thiefplayer04) infos.Add(thiefplayer04);
            if (p == PoliceAndThief.thiefplayer05) infos.Add(thiefplayer05);
            if (p == PoliceAndThief.thiefplayer06) infos.Add(thiefplayer06);
            if (p == PoliceAndThief.thiefplayer07) infos.Add(thiefplayer07);
            if (p == PoliceAndThief.thiefplayer08) infos.Add(thiefplayer08);
            if (p == PoliceAndThief.thiefplayer09) infos.Add(thiefplayer09);

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

            // Battle Royale
            if (p == BattleRoyale.soloPlayer01) infos.Add(soloPlayer01);
            if (p == BattleRoyale.soloPlayer02) infos.Add(soloPlayer02);
            if (p == BattleRoyale.soloPlayer03) infos.Add(soloPlayer03);
            if (p == BattleRoyale.soloPlayer04) infos.Add(soloPlayer04);
            if (p == BattleRoyale.soloPlayer05) infos.Add(soloPlayer05);
            if (p == BattleRoyale.soloPlayer06) infos.Add(soloPlayer06);
            if (p == BattleRoyale.soloPlayer07) infos.Add(soloPlayer07);
            if (p == BattleRoyale.soloPlayer08) infos.Add(soloPlayer08);
            if (p == BattleRoyale.soloPlayer09) infos.Add(soloPlayer09);
            if (p == BattleRoyale.soloPlayer10) infos.Add(soloPlayer10);
            if (p == BattleRoyale.soloPlayer11) infos.Add(soloPlayer11);
            if (p == BattleRoyale.soloPlayer12) infos.Add(soloPlayer12);
            if (p == BattleRoyale.soloPlayer13) infos.Add(soloPlayer13);
            if (p == BattleRoyale.soloPlayer14) infos.Add(soloPlayer14);
            if (p == BattleRoyale.soloPlayer15) infos.Add(soloPlayer15);

            if (p == BattleRoyale.limePlayer01) infos.Add(limePlayer01);
            if (p == BattleRoyale.limePlayer02) infos.Add(limePlayer02);
            if (p == BattleRoyale.limePlayer03) infos.Add(limePlayer03);
            if (p == BattleRoyale.limePlayer04) infos.Add(limePlayer04);
            if (p == BattleRoyale.limePlayer05) infos.Add(limePlayer05);
            if (p == BattleRoyale.limePlayer06) infos.Add(limePlayer06);
            if (p == BattleRoyale.limePlayer07) infos.Add(limePlayer07);
            if (p == BattleRoyale.pinkPlayer01) infos.Add(pinkPlayer01);
            if (p == BattleRoyale.pinkPlayer02) infos.Add(pinkPlayer02);
            if (p == BattleRoyale.pinkPlayer03) infos.Add(pinkPlayer03);
            if (p == BattleRoyale.pinkPlayer04) infos.Add(pinkPlayer04);
            if (p == BattleRoyale.pinkPlayer05) infos.Add(pinkPlayer05);
            if (p == BattleRoyale.pinkPlayer06) infos.Add(pinkPlayer06);
            if (p == BattleRoyale.pinkPlayer07) infos.Add(pinkPlayer07);
            if (p == BattleRoyale.serialKiller) infos.Add(serialKiller);

            // Monja Festival
            if (p == MonjaFestival.greenPlayer01) infos.Add(greenMonjaPlayer01);
            if (p == MonjaFestival.greenPlayer02) infos.Add(greenMonjaPlayer02);
            if (p == MonjaFestival.greenPlayer03) infos.Add(greenMonjaPlayer03);
            if (p == MonjaFestival.greenPlayer04) infos.Add(greenMonjaPlayer04);
            if (p == MonjaFestival.greenPlayer05) infos.Add(greenMonjaPlayer05);
            if (p == MonjaFestival.greenPlayer06) infos.Add(greenMonjaPlayer06);
            if (p == MonjaFestival.greenPlayer07) infos.Add(greenMonjaPlayer07);
            if (p == MonjaFestival.cyanPlayer01) infos.Add(cyanPlayer01);
            if (p == MonjaFestival.cyanPlayer02) infos.Add(cyanPlayer02);
            if (p == MonjaFestival.cyanPlayer03) infos.Add(cyanPlayer03);
            if (p == MonjaFestival.cyanPlayer04) infos.Add(cyanPlayer04);
            if (p == MonjaFestival.cyanPlayer05) infos.Add(cyanPlayer05);
            if (p == MonjaFestival.cyanPlayer06) infos.Add(cyanPlayer06);
            if (p == MonjaFestival.cyanPlayer07) infos.Add(cyanPlayer07);
            if (p == MonjaFestival.bigMonjaPlayer) infos.Add(bigMonja);


            // Impostor roles
            if (p == Mimic.mimic) infos.Add(mimic);
            if (p == Painter.painter) infos.Add(painter);
            if (p == Demon.demon) infos.Add(demon);
            if (p == Illusionist.illusionist) infos.Add(illusionist);
            if (p == Janitor.janitor) infos.Add(janitor);
            if (p == Manipulator.manipulator) infos.Add(manipulator);
            if (p == Bomberman.bomberman) infos.Add(bomberman);
            if (p == Chameleon.chameleon) infos.Add(chameleon);
            if (p == Gambler.gambler) infos.Add(gambler);
            if (p == Sorcerer.sorcerer) infos.Add(sorcerer);
            if (p == Medusa.medusa) infos.Add(medusa);
            if (p == Hypnotist.hypnotist) infos.Add(hypnotist);
            if (p == Archer.archer) infos.Add(archer);
            if (p == Plumber.plumber) infos.Add(plumber);
            if (p == Librarian.librarian) infos.Add(librarian);

            // Rebels roles
            if (p == Renegade.renegade || (Renegade.formerRenegades != null && Renegade.formerRenegades.Any(x => x.PlayerId == p.PlayerId))) infos.Add(renegade);
            if (p == Minion.minion) infos.Add(minion);
            if (p == BountyHunter.bountyhunter) infos.Add(bountyHunter);
            if (p == Trapper.trapper) infos.Add(trapper);
            if (p == Yinyanger.yinyanger) infos.Add(yinyanger);
            if (p == Challenger.challenger) infos.Add(challenger);
            if (p == Ninja.ninja) infos.Add(ninja);
            if (p == Berserker.berserker) infos.Add(berserker);
            if (p == Yandere.yandere) infos.Add(yandere);
            if (p == Stranded.stranded) infos.Add(stranded);
            if (p == Monja.monja) infos.Add(monja);

            // Neutral roles
            if (p == Joker.joker) infos.Add(joker);
            if (p == RoleThief.rolethief) infos.Add(rolethief);
            if (p == Pyromaniac.pyromaniac) infos.Add(pyromaniac);
            if (p == TreasureHunter.treasureHunter) infos.Add(treasureHunter);
            if (p == Devourer.devourer) infos.Add(devourer);
            if (p == Poisoner.poisoner) infos.Add(poisoner);
            if (p == Puppeteer.puppeteer) infos.Add(puppeteer);
            if (p == Exiler.exiler) infos.Add(exiler);
            if (p == Amnesiac.amnesiac) infos.Add(amnesiac);
            if (p == Seeker.seeker) infos.Add(seeker);

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
            if (p == Hunter.hunter) infos.Add(hunter);
            if (p == Jinx.jinx) infos.Add(jinx);
            if (p == Bat.bat) infos.Add(bat);
            if (p == Necromancer.necromancer) infos.Add(necromancer);
            if (p == Engineer.engineer) infos.Add(engineer);
            if (p == Locksmith.locksmith) infos.Add(locksmith);
            if (p == TaskMaster.taskMaster) infos.Add(taskMaster);
            if (p == Jailer.jailer) infos.Add(jailer);

            // Modifier
            if (p == Modifiers.lighter) infos.Add(lighter);
            if (p == Modifiers.blind) infos.Add(blind);
            if (p == Modifiers.flash) infos.Add(flash);
            if (p == Modifiers.bigchungus) infos.Add(bigchungus);
            if (p == Modifiers.theChosenOne) infos.Add(theChosenOne);
            if (p == Modifiers.performer) infos.Add(performer);
            if (p == Modifiers.pro) infos.Add(pro);
            if (p == Modifiers.paintball) infos.Add(paintball);
            if (p == Modifiers.electrician) infos.Add(electrician);
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
                else if (Hunter.hunter != null && p == Hunter.hunter) {
                    name = "Hunter";
                    color = Hunter.color;
                }
                else if (Jinx.jinx != null && p == Jinx.jinx) {
                    name = "Jinx";
                    color = Jinx.color;
                }
                else if (Bat.bat != null && p == Bat.bat) {
                    name = "Bat";
                    color = Bat.color;
                }
                else if (Necromancer.necromancer != null && p == Necromancer.necromancer) {
                    name = "Necromancer";
                    color = Necromancer.color;
                }
                else if (Engineer.engineer != null && p == Engineer.engineer) {
                    name = "Engineer";
                    color = Engineer.color;
                }
                else if (Locksmith.locksmith != null && p == Locksmith.locksmith) {
                    name = "Locksmith";
                    color = Locksmith.color;
                }
                else if (TaskMaster.taskMaster != null && p == TaskMaster.taskMaster) {
                    name = "Task Master";
                    color = TaskMaster.color;
                }
                else if (Jailer.jailer != null && p == Jailer.jailer) {
                    name = "Jailer";
                    color = Jailer.color;
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
                else if (Illusionist.illusionist != null && p == Illusionist.illusionist) {
                    name = "Illusionist";
                    color = Illusionist.color;
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
                else if (Medusa.medusa != null && p == Medusa.medusa) {
                    name = "Medusa";
                    color = Medusa.color;
                    isGood = false;
                }
                else if (Hypnotist.hypnotist != null && p == Hypnotist.hypnotist) {
                    name = "Hypnotist";
                    color = Palette.ImpostorRed;
                    isGood = false;
                }
                else if (Archer.archer != null && p == Archer.archer) {
                    name = "Archer";
                    color = Palette.ImpostorRed;
                    isGood = false;
                }
                else if (Plumber.plumber != null && p == Plumber.plumber) {
                    name = "Plumber";
                    color = Palette.ImpostorRed;
                    isGood = false;
                }
                else if (Librarian.librarian != null && p == Librarian.librarian) {
                    name = "Librarian";
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
                else if (Ninja.ninja != null && p == Ninja.ninja) {
                    name = "Ninja";
                    color = Ninja.color;
                    isGood = false;
                }
                else if (Berserker.berserker != null && p == Berserker.berserker) {
                    name = "Berserker";
                    color = Berserker.color;
                    isGood = false;
                }
                else if (Yandere.yandere != null && p == Yandere.yandere) {
                    name = "Yandere";
                    color = Yandere.color;
                    isGood = false;
                }
                else if (Stranded.stranded != null && p == Stranded.stranded) {
                    name = "Stranded";
                    color = Stranded.color;
                    isGood = false;
                }
                else if (Monja.monja != null && p == Monja.monja) {
                    name = "Monja";
                    color = Monja.color;
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
                else if (Poisoner.poisoner != null && p == Poisoner.poisoner) {
                    name = "Poisoner";
                    color = Poisoner.color;
                    isGood = false;
                }
                else if (Puppeteer.puppeteer != null && p == Puppeteer.puppeteer) {
                    name = "Puppeteer";
                    color = Puppeteer.color;
                    isGood = false;
                }
                else if (Exiler.exiler != null && p == Exiler.exiler) {
                    name = "Exiler";
                    color = Exiler.color;
                    isGood = false;
                }
                else if (Amnesiac.amnesiac != null && p == Amnesiac.amnesiac) {
                    name = "Amnesiac";
                    color = Amnesiac.color;
                    isGood = false;
                }
                else if (Seeker.seeker != null && p == Seeker.seeker) {
                    name = "Seeker";
                    color = Seeker.color;
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