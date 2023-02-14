using LasMonjas.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Il2CppSystem.Xml.Schema.FacetsChecker.FacetsCompiler;
using static Rewired.Controller;

namespace LasMonjas.Core
{
    public class Language
    {
        public static string[] colorNames;

        public static string[] customOptionNames;

        public static string[] roleInfoNames;

        public static string[] exileControllerTexts;

        public static string[] introTexts;

        public static string[] playerControlTexts;

        public static string[] usablesTexts;

        public static string[] buttonsTexts;

        public static string[] helpersTexts;

        public static string[] statusRolesTexts;

        public static string[] statusCaptureTheFlagTexts;

        public static string[] statusPoliceAndThiefsTexts;

        public static string[] statusKingOfTheHillTexts;

        public static string[] statusHotPotatoTexts;

        public static string[] statusZombieLaboratoryTexts;

        public static string[] statusBattleRoyaleTexts;

        public static string[] statusMonjaFestivalTexts;

        public static void LoadLanguage() {
            switch (LasMonjasPlugin.modLanguage.Value) {
                // English
                case 1:
                    colorNames = new string[5] { "Lavender", "Petrol", "Mint", "Olive", "Ice" };
                    for (int i = 0; i < colorNames.Count(); i++) {
                        CustomColors.ColorStrings[i + 50000] = colorNames[i];
                    }
                    customOptionNames = new string[] {
                        CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Preset"),
                        CustomOptionHolder.cs(Jailer.color, "Global Settings"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Activate mod roles and gamemodes"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Activate Custom Skeld Map"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Hide Vent Anim on Shadows"),
                        CustomOptionHolder.cs(Detective.color, "Roles Settings"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Find a Role Mode"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Remove Swipe Card Task"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Remove Airship Doors"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Night vision for lights sabotage"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Screen shake for reactor sabotage"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Anonymous players for comms sabotage"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Decrease speed for oxygen sabotage"),
                        CustomOptionHolder.cs(Modifiers.color, "Modifiers"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Lovers"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Lighter"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Blind"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Flash"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Big Chungus"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "The Chosen One"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "The Chosen One") + ": Report Delay",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Performer"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Performer") + ": Alarm Duration",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Pro"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Paintball"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Paintball") + ": Paint Duration",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Electrician"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Electrician") + ": Discharge Duration",
                        CustomOptionHolder.cs(Sheriff.color, "Capture the Flag"),
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Match Duration",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Score Number",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Kill Cooldown",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Revive Wait Time",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Invincibility Time After Revive",
                        CustomOptionHolder.cs(Coward.color, "Police and Thieves"),
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Match Duration",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Jewel Number",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Police Kill Cooldown",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Police Arrest Cooldown",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Time to Arrest",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Police Tase Cooldown",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Police Tase Duration",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Police can see Jewels",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Police vision range",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Police Revive Wait Time",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Who Can Thieves Kill", // 45
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Thieves Kill Cooldown",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Thieves Revive Wait Time",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Invincibility Time After Revive",
                        CustomOptionHolder.cs(Squire.color, "King of the Hill"),
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Match Duration",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Score Number",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Capture Cooldown",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Kill Cooldown",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Kings can Kill",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Revive Wait Time",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Invincibility Time After Revive",
                        CustomOptionHolder.cs(Shy.color, "Hot Potato"),
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Match Duration",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Hot Potato Time Limit for Transfer",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Hot Potato Transfer Cooldown",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Cold Potatoes vision range",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Reset Hot Potato timer after Transfer",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Extra Time when timer doesn't reset",
                        CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory"),
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Match Duration",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Initial Zombies",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Time to Infect",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Infect Cooldown",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Search Box Timer",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Survivors vision range",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Time to use Medkit",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Kill Cooldown",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Revive Wait Time",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Invincibility Time After Revive",
                        CustomOptionHolder.cs(Sleuth.color, "Battle Royale"),
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Match Duration",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Match Type", // 77
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Kill Cooldown",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Fighter Lifes",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Score Number",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Revive Wait Time",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Invincibility Time After Revive",
                        CustomOptionHolder.cs(Monja.color, "Monja Festival"),
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Match Duration",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Kill Cooldown",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Revive Wait Time",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Invincibility Time After Revive",
                        CustomOptionHolder.cs(Mimic.color, "Mimic"),
                        "- " + CustomOptionHolder.cs(Mimic.color, "Mimic") + ": Duration",
                        CustomOptionHolder.cs(Painter.color, "Painter"),
                        "- " + CustomOptionHolder.cs(Painter.color, "Painter") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Painter.color, "Painter") + ": Duration",
                        CustomOptionHolder.cs(Demon.color, "Demon"),
                        "- " + CustomOptionHolder.cs(Demon.color, "Demon") + ": Delay Time",
                        "- " + CustomOptionHolder.cs(Demon.color, "Demon") + ": Can Kill near Nuns",
                        CustomOptionHolder.cs(Janitor.color, "Janitor"),
                        "- " + CustomOptionHolder.cs(Janitor.color, "Janitor") + ": Cooldown",
                        CustomOptionHolder.cs(Illusionist.color, "Illusionist"),
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": Hats Cooldown",
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": Lights Cooldown",
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": Blackout Duration",
                        CustomOptionHolder.cs(Manipulator.color, "Manipulator"),
                        CustomOptionHolder.cs(Bomberman.color, "Bomberman"),
                        "- " + CustomOptionHolder.cs(Bomberman.color, "Bomberman") + ": Cooldown",
                        CustomOptionHolder.cs(Chameleon.color, "Chameleon"),
                        "- " + CustomOptionHolder.cs(Chameleon.color, "Chameleon") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Chameleon.color, "Chameleon") + ": Duration",
                        CustomOptionHolder.cs(Gambler.color, "Gambler"),
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": Shoot Number",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": Can use emergency button",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": Can Shoot multiple times",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": Ignore shields",
                        CustomOptionHolder.cs(Sorcerer.color, "Sorcerer"),
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": Additional Cooldown per Spell",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": Spell Duration",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": Can use emergency button",
                        CustomOptionHolder.cs(Medusa.color, "Medusa"),
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": Petrify Delay",
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": Petrify Duration",
                        CustomOptionHolder.cs(Hypnotist.color, "Hypnotist"),
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": Spiral Number",
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": Spiral Effect Duration",
                        CustomOptionHolder.cs(Archer.color, "Archer"),
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": Arrow Size",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": Arrow Range",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": Notify Range",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": Aim Duration",
                        CustomOptionHolder.cs(Plumber.color, "Plumber"),
                        "- " + CustomOptionHolder.cs(Plumber.color, "Plumber") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Plumber.color, "Plumber") + ": Number of Vents",
                        CustomOptionHolder.cs(Librarian.color, "Librarian"),
                        "- " + CustomOptionHolder.cs(Librarian.color, "Librarian") + ": Cooldown",
                        CustomOptionHolder.cs(Renegade.color, "Renegade"),
                        "- " + CustomOptionHolder.cs(Renegade.color, "Renegade") + ": Can use Vents",
                        "- " + CustomOptionHolder.cs(Renegade.color, "Renegade") + ": Can Recruit a Minion",
                        CustomOptionHolder.cs(BountyHunter.color, "Bounty Hunter"),
                        CustomOptionHolder.cs(Trapper.color, "Trapper"),
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Mine Number",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Mine Duration",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Trap Number",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Trap Duration",
                        CustomOptionHolder.cs(Yinyanger.color, "Yinyanger"),
                        "- " + CustomOptionHolder.cs(Yinyanger.color, "Yinyanger") + ": Cooldown",
                        CustomOptionHolder.cs(Challenger.color, "Challenger"),
                        "- " + CustomOptionHolder.cs(Challenger.color, "Challenger") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Challenger.color, "Challenger") + ": Kills to Win",
                        CustomOptionHolder.cs(Ninja.color, "Ninja"),
                        CustomOptionHolder.cs(Berserker.color, "Berserker"),
                        "- " + CustomOptionHolder.cs(Berserker.color, "Berserker") + ": Kill Time Limit",
                        CustomOptionHolder.cs(Yandere.color, "Yandere"),
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": Stare Cooldown",
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": Stare Times",
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": Stare Duration",
                        CustomOptionHolder.cs(Stranded.color, "Stranded"),
                        CustomOptionHolder.cs(Monja.color, "Monja"),
                        CustomOptionHolder.cs(Joker.color, "Joker"),
                        "- " + CustomOptionHolder.cs(Joker.color, "Joker") + ": Can Sabotage",
                        CustomOptionHolder.cs(RoleThief.color, "Role Thief"),
                        "- " + CustomOptionHolder.cs(RoleThief.color, "Role Thief") + ": Cooldown",
                        CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac"),
                        "- " + CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac") + ": Ignite Duration",
                        CustomOptionHolder.cs(TreasureHunter.color, "Treasure Hunter"),
                        "- " + CustomOptionHolder.cs(TreasureHunter.color, "Treasure Hunter") + ": Treasures to Win",
                        CustomOptionHolder.cs(Devourer.color, "Devourer"),
                        "- " + CustomOptionHolder.cs(Devourer.color, "Devourer") + ": Devours to Win",
                        CustomOptionHolder.cs(Poisoner.color, "Poisoner"),
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": Time to Poison",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": Poison Infect Range",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": Time to fully Poison",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": Can Sabotage",
                        CustomOptionHolder.cs(Puppeteer.color, "Puppeteer"),
                        "- " + CustomOptionHolder.cs(Puppeteer.color, "Puppeteer") + ": Number of Kills",
                        CustomOptionHolder.cs(Exiler.color, "Exiler"),
                        CustomOptionHolder.cs(Amnesiac.color, "Amnesiac"),
                        CustomOptionHolder.cs(Seeker.color, "Seeker"),
                        "- " + CustomOptionHolder.cs(Seeker.color, "Seeker") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Seeker.color, "Seeker") + ": Points to Win",
                        CustomOptionHolder.cs(Captain.color, "Captain"),
                        "- " + CustomOptionHolder.cs(Captain.color, "Captain") + ": Can Special Vote one time",
                        CustomOptionHolder.cs(Mechanic.color, "Mechanic"),
                        "- " + CustomOptionHolder.cs(Mechanic.color, "Mechanic") + ": Repairs Number",
                        "- " + CustomOptionHolder.cs(Mechanic.color, "Mechanic") + ": Expert Repairs",
                        CustomOptionHolder.cs(Sheriff.color, "Sheriff"),
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Sheriff") + ": Can Kill Neutrals",
                        CustomOptionHolder.cs(Detective.color, "Detective"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Show Footprints", // 191
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Show Footprints Duration",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Anonymous Footprints",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Footprints Interval",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Footprints Duration",
                        CustomOptionHolder.cs(Forensic.color, "Forensic"),
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Time to know the name",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Time to know the color type",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Time to know if the killer has hat, outfit, pet or visor",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Question Duration",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": One question per Ghost",
                        CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler"),
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": Shield Duration",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": Rewind Duration",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": Revive player during Rewind",
                        CustomOptionHolder.cs(Squire.color, "Squire"),
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": Show Shielded Player to", // 209
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": Play murder attempt sound if shielded",
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": Can shield again after meeting",
                        CustomOptionHolder.cs(Cheater.color, "Cheater"),
                        "- " + CustomOptionHolder.cs(Cheater.color, "Cheater") + ": Can use emergency button",
                        "- " + CustomOptionHolder.cs(Cheater.color, "Cheater") + ": Can swap himself",
                        CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller"),
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Reveal Time",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Reveal Number",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Revealed Information", // 210
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Show Notification to", // 220
                        CustomOptionHolder.cs(Hacker.color, "Hacker"),
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": Duration",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": Battery Uses",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": Tasks for recharge batteries",
                        CustomOptionHolder.cs(Sleuth.color, "Sleuth"),
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": Track Interval",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": Can Track again after meeting",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": Track Corpses Cooldown",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": Track Corpses Duration",
                        CustomOptionHolder.cs(Fink.color, "Fink"),
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": Tasks remaining for being revealed to Impostors",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": Can reveal Renegade / Minion",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": Hawkeye Duration",
                        CustomOptionHolder.cs(Kid.color, "Kid"),
                        CustomOptionHolder.cs(Welder.color, "Welder"),
                        "- " + CustomOptionHolder.cs(Welder.color, "Welder") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Welder.color, "Welder") + ": Seal Number",
                        CustomOptionHolder.cs(Spiritualist.color, "Spiritualist"),
                        "- " + CustomOptionHolder.cs(Spiritualist.color, "Spiritualist") + ": Revive Player Time",
                        CustomOptionHolder.cs(Vigilant.color, "Vigilant"),
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": Remote Camera Duration",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": Battery Uses",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": Tasks for recharge batteries",
                        CustomOptionHolder.cs(Hunter.color, "Hunter"),
                        "- " + CustomOptionHolder.cs(Hunter.color, "Hunter") + ": Can mark again after meeting",
                        CustomOptionHolder.cs(Jinx.color, "Jinx"),
                        "- " + CustomOptionHolder.cs(Jinx.color, "Jinx") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Jinx.color, "Jinx") + ": Jinx Number",
                        CustomOptionHolder.cs(Coward.color, "Coward"),
                        "- " + CustomOptionHolder.cs(Coward.color, "Coward") + ": Number Of Meetings",
                        CustomOptionHolder.cs(Bat.color, "Bat"),
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": Emit Duration",
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": Emit Range",
                        CustomOptionHolder.cs(Necromancer.color, "Necromancer"),
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": Revive Duration",
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": Room Distance",
                        CustomOptionHolder.cs(Engineer.color, "Engineer"),
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Trap Number",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Trap Duration",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Speed Increase",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Speed Decrease",
                        CustomOptionHolder.cs(Shy.color, "Shy"),
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": Duration",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": Notify Range",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": Arrow color is player color",
                        CustomOptionHolder.cs(TaskMaster.color, "Task Master"),
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Extra Common Tasks",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Extra Long Tasks",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Extra Short Tasks",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Speed Cooldown",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Speed Duration",
                        CustomOptionHolder.cs(Jailer.color, "Jailer"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Jailer") + ": Cooldown",
                        "- " + CustomOptionHolder.cs(Jailer.color, "Jailer") + ": Jail Duration"
                    };
                    for (int o = 0; o < customOptionNames.Count(); o++) {
                        CustomOption.options[o].name = customOptionNames[o];
                        switch (o) {
                            case 45:
                                CustomOption.options[o].selections = new string[] { "Taser", "All", "Nobody" };
                                break;
                            case 77:
                                CustomOption.options[o].selections = new string[] { "All vs All", "Team Battle", "Score Battle" };
                                break;
                            case 191:
                                CustomOption.options[o].selections = new string[] { "Button Use", "Always" };
                                break;
                            case 209:
                                CustomOption.options[o].selections = new string[] { "Squire", "Both", "All" };
                                break;
                            case 219:
                                CustomOption.options[o].selections = new string[] { "Good / Bad", "Role Name" };
                                break;
                            case 220:
                                CustomOption.options[o].selections = new string[] { "Impostors", "Crewmates", "All", "Nobody" };
                                break;
                        }
                    }
                    roleInfoNames = new string[] {
                        "Steal <color=#0000FFFF>Blue Team</color> flag",
                        "Steal <color=#FF0000FF>Red Team</color> flag",
                        "Kill the player with a flag to switch teams with it",
                        "Kill the player with\na flag to switch teams with it",
                        "Capture all the <color=#D2B48CFF>Thieves</color>",
                        "Tase the <color=#D2B48CFF>Thieves</color>",//5
                        "Tase the <color=#D2B48CFF>Thieves\nwith right click</color>",
                        "Steal all the jewels without getting captured",
                        "Steal all the jewels\nwithout getting captured",
                        "Capture the zones",
                        "Protect your King", //10
                        "Kill a King to become one",
                        "Give the hot potato to other player",
                        "Give the hot potato\nto other player",
                        "Run from the <color=#808080FF>Hot Potato</color>", 
                        "You are burnt",//15
                        "Heal survivors and create the cure",
                        "Heal survivors\nand create the cure",
                        "Survive while looking for items to make the cure",
                        "Survive while looking\nfor items to make the cure",
                        "Infect all survivors",//20
                        "Be the last one alive",
                        "Kill <color=#F2BEFFFF>Pink</color> Team",
                        "Kill <color=#39FF14FF>Lime</color> Team",
                        "Mimic other player's look",
                        "Paint players with the same color",//25
                        "Bite players to delay their death",
                        "Remove and move bodies from the crime scene",
                        "Remove and move bodies\nfrom the crime scene",
                        "Create your own vent network and turn off the lights",
                        "Create your own vent\nnetwork and turn off the lights", //30
                        "Manipulate a player to kill his adjacent",
                        "Sabotage by putting bombs",
                        "Make yourself invisible\nbut you can't vent",
                        "Shoot a player choosing their role during the meeting",
                        "Shoot a player choosing\ntheir role during the meeting",//35
                        "Casts spells on players",
                        "Petrify players",
                        "Invert player movement controls",
                        "Make range kills",
                        "Pick bow with F\nand right click to shoot", //40
                        "Create new vents",
                        "Silence a player to prevent him from talking",
                        "Silence a player\nto prevent him from talking",
                        "Recruit a Minion and kill everyone",
                        "Help the Renegade killing everyone",//45
                        "Hunt down your target",
                        "Place landmines and root traps",
                        "Mark two players to die if they collide",
                        "Mark two players\nto die if they collide",
                        "Challenge a player to a rock-paper-scissors duel",//50
                        "Challenge a player to\na rock-paper-scissors duel",
                        "Mark and make double kills",
                        "You can't stop killing",
                        "Stalk and kill your target",
                        "Find ammo and kill 3 players",//55
                        "Recover your items and transform into Monja",
                        "Recover your items\nand transform into Monja",
                        "Get voted out to win",
                        "Get voted out to win\nOpen the map to activate the sabotage button",
                        "Steal other player's role",//60
                        "Ignite all survivors to win",
                        "Find treasures to win",
                        "Devour bodies to win",
                        "Poison all players to win",
                        "Poison all players to win\nOpen the map to activate the sabotage button",//65
                        "Make dummies and get them killed to win",
                        "Make dummies and\nget them killed to win",
                        "Get your target voted out to win",
                        "Remember your role reporting a body",
                        "Gain points playing hide and seek",//70
                        "Your vote counts twice",
                        "Repair sabotages on the ship",
                        "Kill the <color=#FF0000FF>Impostors</color>",
                        "Examine footprints",
                        "Find clues reporting bodies and asking their ghosts",//75
                        "Find clues reporting bodies\nand asking their ghosts",
                        "Rewind the time",
                        "Protect a player with your shield",
                        "Swap the votes of two players",
                        "Reveal who are the <color=#FF0000FF>Impostors</color>",//80
                        "Use Admin and Vitals from anywhere",
                        "Track down a player and corpses",
                        "Finish your tasks to reveal the <color=#FF0000FF>Impostors</color>",
                        "Everyone loses if you die or get voted out",
                        "Seal vents",//85
                        "Sacrifice yourself to revive a player",
                        "Call meetings from anywhere",
                        "Put additional cameras on the map",
                        "Activate remote Doorlog with Q key",
                        "Mark a player to die if you get killed",//90
                        "Jinx players abilities",
                        "Reduce button cooldown and increase impostor ones",
                        "Reduce button cooldown\nand increase impostor ones",
                        "Take a body to its room and revive it",
                        "Place speed and position traps",//95
                        "Place speed and position traps\nSwitch trap type with Q key",
                        "Check close players",
                        "Complete all your tasks and all your extra tasks to win",
                        "Complete all your tasks\nand all your extra tasks to win",
                        "Send killers to prison",//100
                        "Sabotage and kill everyone",
                        "Find and exile the <color=#FF0000FF>Impostors</color>",
                        "You have more vision",
                        "You have less vision",
                        "You're faster",//105
                        "You're bigger and slower",
                        "Your killer will report your body",
                        "Your death will trigger an alarm and reveal where your body is",
                        "Your death will trigger an alarm\nand reveal where your body is",
                        "Your movement controls are inverted",//110
                        "Your killer leaves a paint trail",
                        "Your killer will be paralyzed in place",
                        "♥Survive as a couple with your partner♥",
                        "♥Survive as a couple\nwith your partner♥",
                        "<color=#FF00D1FF>♥Survive as a couple with your partner♥. </color><color=#FF1919FF>Kill the rest</color>",//115
                        "<color=#FF00D1FF>♥Survive as a couple\nwith your partner♥. </color><color=#FF1919FF>Kill the rest</color>",
                        "Get more monjitas\nthan the other teams",
                    }; 
                    exileControllerTexts = new string[] {
                        " was the ",
                        "You thought I was the Impostor but it was me, Joker!",
                        "That's all folks!"
                    }; 
                    introTexts = new string[] {
                        "♥ Survive as a couple with ",
                        "Time Left: ",
                        "Score: ",
                        "Stolen Jewels: ",
                        "Captured Thieves: ",
                        "Hot Potato: ",
                        "Cold Potatoes: ",
                        "Key Items: ",
                        "Survivors: ",
                        "Infected: ",
                        "Zombies: ",
                        "Battle Royale Fighters: ",
                        "Lime Team: ",
                        "Pink Team: ",
                        "Serial Killer: ",
                        "Goal: ",
                        "Serial Killer Points: ",
                        "Green Team: ",
                        "Cyan Team: ",
                        "Big Monja: ",
                    };
                    playerControlTexts = new string[] {
                        "It appears to be a suicide!",
                        "The killer appears to be",
                        "lighter (L) color!",
                        "darker (D) color!",
                        "The killer appears to have a",
                        "The killer appears to wear a hat!",
                        "The killer doesn't wear a hat!",
                        "The killer appears to wear an outfit!",
                        "The killer doesn't wear an outfit!",
                        "The killer appears to have a pet!",
                        "The killer hasn't got a pet!",
                        "The killer appears to wear a visor!",
                        "The killer doesn't wear a visor!",
                        "The body is too old to gain information from!",
                        "Time Elapsed:"
                    }; 
                    usablesTexts = new string[] {
                        "Can't use the emergency button\non custom gamemodes!",
                        "The Cheater can't use the emergency button!",
                        "The Gambler can't use the emergency button!",
                        "The Sorcerer can't use the emergency button!",
                        "There's a Bomb, you can't use the emergency button!",
                        "There's a Blackout, emergency button doesn't work!",
                        "THE MONJA HAS AWAKENED, RUN YOU FOOLS!"
                    }; 
                    buttonsTexts = new string[] {
                        "Ghost of (",
                        "I was the ",
                        "My killer has a ",
                        "Seconds since death: ",
                        "My killer was the "
                    }; 
                    helpersTexts = new string[] {
                        " and recruit a Minion",
                        "Kill everyone"
                    };
                    statusRolesTexts = new string[] { 
                        "Speed Changed!",
                        "Hypnotized!",
                        "Target Died",
                        "Rampage Mode",
                        "Seeker's current points: ",
                        "The Illusionist turned off the lights: ",
                        "There's a Bomb on the map: ",
                        "Petrified!",
                        "THE MONJA HAS AWAKENED: ",
                        "Amnesiac Body Report: This role can't be remembered",
                        "Fink is using Hawkeye!",                     
                    }; 
                    statusCaptureTheFlagTexts = new string[] {                        
                        "You're the new <color=#FF0000FF>Red Team</color> player now!",
                        "You're the new <color=#0000FFFF>Blue Team</color> player now!",
                        "<color=#0000FFFF>Blue Flag</color> stolen by <color=#FF0000FF>",
                        "Your flag has been stolen!",
                        "<color=#FF0000FF>Red Flag</color> stolen by <color=#0000FFFF>",
                        "<color=#FF0000FF>Red Team</color> scored!",
                        "<color=#0000FFFF>Blue Team</color> scored!"
                    }; 
                    statusPoliceAndThiefsTexts = new string[] {                        
                        "A <color=#928B55FF>Thief</color> has been captured!",
                        "A <color=#928B55FF>Thief</color> has been released!",
                        "A <color=#00F7FFFF>Jewel</color> has been delivered!"
                    }; 
                    statusKingOfTheHillTexts = new string[] {                        
                        "You're the new <color=#00FF00FF>Green King</color>!",
                        "You're the new <color=#FFFF00FF>Yellow King</color>!",
                        "<color=#00FF00FF>Green King</color> has captured a zone!",
                        "<color=#FFFF00FF>Yellow King</color> has captured a zone!",
                        "Your King has been killed!"
                    }; 
                    statusHotPotatoTexts = new string[] {
                        " is the new Hot Potato!"
                    }; 
                    statusZombieLaboratoryTexts = new string[] {
                        "A <color=#FF00FFFF>Key Item</color> has been delivered!",
                        "A <color=#00CCFFFF>Survivor</color> has been <color=#FFFF00FF>Infected</color>!",
                        "A <color=#00CCFFFF>Survivor</color> turned into a <color=#996633FF>Zombie</color>!"
                    }; 
                    statusBattleRoyaleTexts = new string[] {
                        "One <color=#009F57FF>Fighter</color> down!",
                        "One <color=#39FF14FF>Lime Fighter</color> down!",
                        "One <color=#F2BEFFFF>Pink Fighter</color> down!",
                        "<color=#808080FF>Serial Killer</color> down!",
                        "Points for <color=#39FF14FF>Lime Team</color>!",
                        "Points for <color=#F2BEFFFF>Pink Team</color>!",
                        "Points for <color=#808080FF>Serial Killer</color>!",
                    }; 
                    statusMonjaFestivalTexts = new string[] {
                        "<color=#808080FF>Big Monja</color> is stealing from your basket!",
                        "<color=#FF00FFFF>Allul Monja</color> found by <color=#00FF00FF>Green Team</color>!",
                        "<color=#FF00FFFF>Allul Monja</color> found by <color=#00F7FFFF>Cyan Team</color>!",
                        "<color=#FF00FFFF>Allul Monja</color> found by <color=#808080FF>Big Monja</color>!",
                    };
                    break;
                // Spanish
                case 2:
                    colorNames = new string[5] { "Lavanda", "Petroleo", "Menta", "Aceituna", "Hielo" };
                    for (int i = 0; i < colorNames.Count(); i++) {
                        CustomColors.ColorStrings[i + 50000] = colorNames[i];
                    }
                    customOptionNames = new string[] {
                        CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "Plantilla"),
                        CustomOptionHolder.cs(Jailer.color, "Opciones Globales"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Activar roles y gamemodes del mod"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Activar Mapa Personalizado Skeld"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Ocultar animacion rejilla a distancia"),
                        CustomOptionHolder.cs(Detective.color, "Opciones de Roles"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Modo encuentra tu Rol"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Quitar Tarea de la Tarjeta"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Quitar puertas en Airship"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Vision Nocturna en sabotaje de luces"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Agitar Camara en sabotaje de reactor"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Jugadores anonimos en sabotaje de comunicaciones"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Menos velocidad en sabotaje de oxigeno"),
                        CustomOptionHolder.cs(Modifiers.color, "Modificadores"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Lovers"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Lighter"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Blind"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Flash"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Big Chungus"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "The Chosen One"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "The Chosen One") + ": Demora de Reporte",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Performer"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Performer") + ": Duracion de Alarma",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Pro"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Paintball"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Paintball") + ": Duration de Pintura",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Electrician"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Electrician") + ": Duracion de Paralisis",
                        CustomOptionHolder.cs(Sheriff.color, "Capture the Flag"),
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Duracion de Partida",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Puntuacion Necesaria",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Recarga de Matar",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Tiempo para Revivir",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Segundos invencibles tras Revivir",
                        CustomOptionHolder.cs(Coward.color, "Police and Thieves"),
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Duracion de Partida",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Joyas Necesarias",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Recarga de Matar Polis",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Recarga de Arrestar Polis",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Tiempo para Arrestar",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Recarga de Taser",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Duracion de Taser",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Joyas visibles para Polis",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Rango vision Polis",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Tiempo para Revivir Polis",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Ladrones pueden matar", // id 45
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Recarga de Matar Ladrones",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Tiempo para Revivir Ladrones",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": Segundos invencibles tras Revivir",
                        CustomOptionHolder.cs(Squire.color, "King of the Hill"),
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Duracion de Partida",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Puntuacion Necesaria",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Tiempo de Captura",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Recarga de Matar",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Los Reyes pueden matar",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Tiempo para Revivir",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Segundos invencibles tras Revivir",
                        CustomOptionHolder.cs(Shy.color, "Hot Potato"),
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Duracion de Partida",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Tiempo limite para transferir Patata",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Recarga de transferir Patata",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Rango vision Patatas Frias",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Reiniciar tiempo al transferir Patata",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": Tiempo extra si no se reinica al transferir",
                        CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory"),
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Duracion de Partida",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Zombies Iniciales",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Tiempo para Infectar",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Recarga de Infectar",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Tiempo para Buscar Cajas",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Rango vision Supervivientes",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Tiempo limite para curar Infeccion",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Recarga de Matar",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Tiempo para Revivir",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Segundos invencibles tras Revivir",
                        CustomOptionHolder.cs(Sleuth.color, "Battle Royale"),
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Duracion de Partida",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Tipo de Partida", // id 77
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Recarga de Matar",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Cantidad de vidas",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Puntuacion Necesaria",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Tiempo para Revivir",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Segundos invencibles tras Revivir",
                        CustomOptionHolder.cs(Monja.color, "Monja Festival"),
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Duracion de Partida",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Recarga de Matar",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Tiempo para Revivir",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Segundos invencibles tras Revivir",
                        CustomOptionHolder.cs(Mimic.color, "Mimic"),
                        "- " + CustomOptionHolder.cs(Mimic.color, "Mimic") + ": Duracion",
                        CustomOptionHolder.cs(Painter.color, "Painter"),
                        "- " + CustomOptionHolder.cs(Painter.color, "Painter") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Painter.color, "Painter") + ": Duracion",
                        CustomOptionHolder.cs(Demon.color, "Demon"),
                        "- " + CustomOptionHolder.cs(Demon.color, "Demon") + ": Demora de muerte",
                        "- " + CustomOptionHolder.cs(Demon.color, "Demon") + ": Puede matar cerca de las Nuns",
                        CustomOptionHolder.cs(Janitor.color, "Janitor"),
                        "- " + CustomOptionHolder.cs(Janitor.color, "Janitor") + ": Recarga",
                        CustomOptionHolder.cs(Illusionist.color, "Illusionist"),
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": Recarga de Sombreros",
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": Recarga de Apagar Luces",
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": Duracion de Apagon Duration",
                        CustomOptionHolder.cs(Manipulator.color, "Manipulator"),
                        CustomOptionHolder.cs(Bomberman.color, "Bomberman"),
                        "- " + CustomOptionHolder.cs(Bomberman.color, "Bomberman") + ": Recarga",
                        CustomOptionHolder.cs(Chameleon.color, "Chameleon"),
                        "- " + CustomOptionHolder.cs(Chameleon.color, "Chameleon") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Chameleon.color, "Chameleon") + ": Duracion",
                        CustomOptionHolder.cs(Gambler.color, "Gambler"),
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": Cantidad de Disparos",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": Puede convocar reuniones",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": Puede disparar varias veces",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": Ignora escudos",
                        CustomOptionHolder.cs(Sorcerer.color, "Sorcerer"),
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": Recarga adicional por hechizo",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": Duracion de Hechizar",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": Puede convocar reuniones",
                        CustomOptionHolder.cs(Medusa.color, "Medusa"),
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": Demora de petrificar",
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": Duracion de petrificar",
                        CustomOptionHolder.cs(Hypnotist.color, "Hypnotist"),
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": Cantidad de Espirales",
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": Duracion de Efecto Spiral",
                        CustomOptionHolder.cs(Archer.color, "Archer"),
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": Anchura de Flecha",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": Distancia de Flecha",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": Distancia de notifiacion",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": Duracion de mirilla",
                        CustomOptionHolder.cs(Plumber.color, "Plumber"),
                        "- " + CustomOptionHolder.cs(Plumber.color, "Plumber") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Plumber.color, "Plumber") + ": Cantidad de rejillas",
                        CustomOptionHolder.cs(Librarian.color, "Librarian"),
                        "- " + CustomOptionHolder.cs(Librarian.color, "Librarian") + ": Recarga",
                        CustomOptionHolder.cs(Renegade.color, "Renegade"),
                        "- " + CustomOptionHolder.cs(Renegade.color, "Renegade") + ": Puede usar rejillas",
                        "- " + CustomOptionHolder.cs(Renegade.color, "Renegade") + ": Puede reclutar un Minion",
                        CustomOptionHolder.cs(BountyHunter.color, "Bounty Hunter"),
                        CustomOptionHolder.cs(Trapper.color, "Trapper"),
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Cantidad de Minas",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Duracion de Minas",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Cantidad de Cepos",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": Duracion de Cepos",
                        CustomOptionHolder.cs(Yinyanger.color, "Yinyanger"),
                        "- " + CustomOptionHolder.cs(Yinyanger.color, "Yinyanger") + ": Recarga",
                        CustomOptionHolder.cs(Challenger.color, "Challenger"),
                        "- " + CustomOptionHolder.cs(Challenger.color, "Challenger") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Challenger.color, "Challenger") + ": Asesinatos para ganar",
                        CustomOptionHolder.cs(Ninja.color, "Ninja"),
                        CustomOptionHolder.cs(Berserker.color, "Berserker"),
                        "- " + CustomOptionHolder.cs(Berserker.color, "Berserker") + ": Tiempo limite para Matar",
                        CustomOptionHolder.cs(Yandere.color, "Yandere"),
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": Recarga de Acechar",
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": Cantidad de acechos",
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": Duracion de Acechar",
                        CustomOptionHolder.cs(Stranded.color, "Stranded"),
                        CustomOptionHolder.cs(Monja.color, "Monja"),
                        CustomOptionHolder.cs(Joker.color, "Joker"),
                        "- " + CustomOptionHolder.cs(Joker.color, "Joker") + ": Puede sabotear",
                        CustomOptionHolder.cs(RoleThief.color, "Role Thief"),
                        "- " + CustomOptionHolder.cs(RoleThief.color, "Role Thief") + ": Recarga",
                        CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac"),
                        "- " + CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac") + ": Duracion de Rociar",
                        CustomOptionHolder.cs(TreasureHunter.color, "Treasure Hunter"),
                        "- " + CustomOptionHolder.cs(TreasureHunter.color, "Treasure Hunter") + ": Cantidad de Tesoros",
                        CustomOptionHolder.cs(Devourer.color, "Devourer"),
                        "- " + CustomOptionHolder.cs(Devourer.color, "Devourer") + ": Cantidad de cuerpos",
                        CustomOptionHolder.cs(Poisoner.color, "Poisoner"),
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": Tiempo para Envenenar",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": Rango de infeccion",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": Tiempo para envenenar al 100%",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": Puede sabotear",
                        CustomOptionHolder.cs(Puppeteer.color, "Puppeteer"),
                        "- " + CustomOptionHolder.cs(Puppeteer.color, "Puppeteer") + ": Cantidad de muertes",
                        CustomOptionHolder.cs(Exiler.color, "Exiler"),
                        CustomOptionHolder.cs(Amnesiac.color, "Amnesiac"),
                        CustomOptionHolder.cs(Seeker.color, "Seeker"),
                        "- " + CustomOptionHolder.cs(Seeker.color, "Seeker") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Seeker.color, "Seeker") + ": Puntuacion necesaria",
                        CustomOptionHolder.cs(Captain.color, "Captain"),
                        "- " + CustomOptionHolder.cs(Captain.color, "Captain") + ": Puede forzar los votos una vez",
                        CustomOptionHolder.cs(Mechanic.color, "Mechanic"),
                        "- " + CustomOptionHolder.cs(Mechanic.color, "Mechanic") + ": Cantidad de Reparaciones",
                        "- " + CustomOptionHolder.cs(Mechanic.color, "Mechanic") + ": Reparaciones Expertas",
                        CustomOptionHolder.cs(Sheriff.color, "Sheriff"),
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Sheriff") + ": Puede matar Neutrales",
                        CustomOptionHolder.cs(Detective.color, "Detective"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Revelar huellas", // id 191
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Duracion de revelar Huellas",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Huellas Anonimas",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Intervalo de Huellas",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": Duracion de Huellas",
                        CustomOptionHolder.cs(Forensic.color, "Forensic"),
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Tiempo para obtener el nombre",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Tiempo para obtener el tipo de color",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Tiempo para saber si el asesino tiene gorro, traje, mascota o visor",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Duracion de Preguntar",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": Una pregunta por Fantasma",
                        CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler"),
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": Recarga",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": Duracion de Escudo",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": Segundos retrocedidos en el tiempo",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": Revivir jugadores al retroceder en el tiempo",
                        CustomOptionHolder.cs(Squire.color, "Squire"),
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": Mostrar jugador escudado", // id 209
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": Sonido de intento de asesinato del escudado",
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": Puede escudar de nuevo tras un reunion",
                        CustomOptionHolder.cs(Cheater.color, "Cheater"),
                        "- " + CustomOptionHolder.cs(Cheater.color, "Cheater") + ": Puede convocar reuniones",
                        "- " + CustomOptionHolder.cs(Cheater.color, "Cheater") + ": Puede intercambiar sus votos",
                        CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller"),
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Recarga",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Tiempo para Revelar",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Cantidad de Revelaciones",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Informacion Revelada", // id 219
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": Mostrar notifiacion", // id 220
                        CustomOptionHolder.cs(Hacker.color, "Hacker"),
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": Duracion",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": Cantidad de usos",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": Tareas para recargar usos",
                        CustomOptionHolder.cs(Sleuth.color, "Sleuth"),
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": Intervalo de rastreo",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": Puedes rastrear de nuevo tras una reunion",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": Recarga de rastrear cuerpos",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": Duracion de rastrear cuerpos",
                        CustomOptionHolder.cs(Fink.color, "Fink"),
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": Tareas restantes para ser reveleado a Impostores",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": Puede revelar a Renegade / Minion",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": Duracion de Espiar",
                        CustomOptionHolder.cs(Kid.color, "Kid"),
                        CustomOptionHolder.cs(Welder.color, "Welder"),
                        "- " + CustomOptionHolder.cs(Welder.color, "Welder") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Welder.color, "Welder") + ": Cantidad de Sellados",
                        CustomOptionHolder.cs(Spiritualist.color, "Spiritualist"),
                        "- " + CustomOptionHolder.cs(Spiritualist.color, "Spiritualist") + ": Duracion de Revivir",
                        CustomOptionHolder.cs(Vigilant.color, "Vigilant"),
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": Duracion de Camaras Remotas",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": Cantidad de usos",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": Tareas para recargar usos",
                        CustomOptionHolder.cs(Hunter.color, "Hunter"),
                        "- " + CustomOptionHolder.cs(Hunter.color, "Hunter") + ": Puede marcar de nuevo tras una reunion",
                        CustomOptionHolder.cs(Jinx.color, "Jinx"),
                        "- " + CustomOptionHolder.cs(Jinx.color, "Jinx") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Jinx.color, "Jinx") + ": Cantidad de gafadas",
                        CustomOptionHolder.cs(Coward.color, "Coward"),
                        "- " + CustomOptionHolder.cs(Coward.color, "Coward") + ": Cantidad de reuniones",
                        CustomOptionHolder.cs(Bat.color, "Bat"),
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": Duracion de Frecuencia",
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": Rango de Frecuencia",
                        CustomOptionHolder.cs(Necromancer.color, "Necromancer"),
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": Tiempo para Revivir",
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": Distancia de Habitaciones",
                        CustomOptionHolder.cs(Engineer.color, "Engineer"),
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Cantidad de Trampas",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Duracion de Trampas",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Aumento de Velocidad",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": Reduccion de Velocidad",
                        CustomOptionHolder.cs(Shy.color, "Shy"),
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": Duracion",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": Rando de Notificacion",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": Flecha del color del jugador",
                        CustomOptionHolder.cs(TaskMaster.color, "Task Master"),
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Tareas Comunes Extra",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Tareas Largas Extra",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Tareas Cortas Extra",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Recarga de Velocidad",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": Duracion de Velocidad",
                        CustomOptionHolder.cs(Jailer.color, "Jailer"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Jailer") + ": Recarga",
                        "- " + CustomOptionHolder.cs(Jailer.color, "Jailer") + ": Tiempo de carcel"
                    };
                    for (int o = 0; o < customOptionNames.Count(); o++) {
                        CustomOption.options[o].name = customOptionNames[o];
                        switch (o) {
                            case 45:
                                CustomOption.options[o].selections = new string[] { "Taser", "Todos", "Nadie" };
                                break;
                            case 77:
                                CustomOption.options[o].selections = new string[] { "Individual", "Por Equipos", "Por Puntuacion" };
                                break;
                            case 191:
                                CustomOption.options[o].selections = new string[] { "Uso de Boton", "Siempre" };
                                break;
                            case 209:
                                CustomOption.options[o].selections = new string[] { "Squire", "Ambos", "Todos" };
                                break;
                            case 219:
                                CustomOption.options[o].selections = new string[] { "Bueno / Malo", "Nombre Rol" };
                                break;
                            case 220:
                                CustomOption.options[o].selections = new string[] { "Impostores", "Tripulantes", "Todos", "Nadie" };
                                break;
                        }                        
                    }
                    roleInfoNames = new string[] {
                        "Roba la Bandera <color=#0000FFFF>Azul</color>",
                        "Roba la Bandera <color=#FF0000FF>Roja</color>",
                        "Matar al jugador con bandera para intercambiar equipos",
                        "Matar al jugador con\nbandera para intercambiar equipos",
                        "Captura todos los <color=#D2B48CFF>Ladrones</color>",
                        "Tasea a los <color=#D2B48CFF>Ladrones</color>",//5
                        "Tasea a los <color=#D2B48CFF>Ladrones\ncon clic derecho</color>",
                        "Roba las joyas sin ser capturado",
                        "Roba las joyas\nsin ser capturado",
                        "Captura las zonas",
                        "Protege a tu Rey", //10
                        "Mata al Rey para convertirte en uno",
                        "Dale la patata caliente a otro jugador",
                        "Dale la patata\ncaliente a otro jugador",
                        "Escapa de la <color=#808080FF>Patata Caliente</color>",
                        "Has explotado",//15
                        "Cura supervivientes y crea la cura",
                        "Cura supervivientes\ny crea la cura",
                        "Sobrevive mientras buscas objetos para la cura",
                        "Sobrevive mientras\nbuscas objetos para la cura",
                        "Infecta a los supervivientes",//20
                        "Sobrevive hasta el final",
                        "Mata al <color=#F2BEFFFF>Equipo Rosa</color>",
                        "Mata al <color=#39FF14FF>Equipo Lima</color>",
                        "Imita el aspecto de otro jugador",
                        "Colorea a todos del mismo color",//25
                        "Muerde jugadores para demorar su muerte",
                        "Quita y mueve cuerpos de la escena del crimen",
                        "Quita y mueve cuerpos\nde la escena del crimen",
                        "Crea tu propia red de rejillas y apaga las luces",
                        "Crea tu propia red de\nrejillas y apaga las luces", //30
                        "Manipula a un jugador para matar a su adyacente",
                        "Sabotea poniendo bombas",
                        "Hazte invisible pero\nno puedes usar rejillas",
                        "Dispara a un jugador durante una reunion adivinando su rol",
                        "Dispara a un jugador durante\nuna reunion adivinando su rol",//35
                        "Hechiza jugadores",
                        "Petrifica jugadores",
                        "Invierte los controles de los jugadores",
                        "Mata a distancia",
                        "Equipa el arco con la F\ny dispara con clic derecho", //40
                        "Crea nuevas rejillas",
                        "Silencia a un jugador para que no hable en la reunion",
                        "Silencia a un jugador\npara que no hable en la reunion",
                        "Recluta un Minion y mata a todos",
                        "Ayuda al Renegade a matar a todos",//45
                        "Mata a tu objetivo",
                        "Pon minas y cepos",
                        "Marca dos jugadores para que mueran si chocan",
                        "Marca dos jugadores\npara que mueran si chocan",
                        "Desafia jugadores a piedra, papel y tijeras",//50
                        "Desafia jugadores\na piedra, papel y tijeras",
                        "Marca y haz muertes dobles",
                        "No puedes parar de matar",
                        "Acecha y mata a tu objetivo",
                        "Encuentra municion y mata a tres jugadores",//55
                        "Lleva los objetos al ritual y despierta a la Monja",
                        "Lleva los objetos al ritual\ny despierta a la Monja",
                        "Consigue que te expulsen fuera para ganar",
                        "Consigue que te expulsen fuera para ganar\nAbre el mapa para activar el boton de sabotajes",
                        "Roba el rol a otro jugador",//60
                        "Rocia a todos para ganar",
                        "Encuentra tesoros para ganar",
                        "Devora cuerpos para ganar",
                        "Envenena a todos para ganar",
                        "Envenena a todos para ganar\nAbre el mapa para activar el boton de sabotajes",//65
                        "Muere haciendo de señuelo para ganar",
                        "Muere haciendo\nde señuelo para ganar",
                        "Expulsa a tu objetivo para ganar",
                        "Recuerda tu rol reportando un cuerpo",
                        "Gana puntos jugando al escondite",//70
                        "Tu voto cuenta doble",
                        "Repara sabotajes a distancia",
                        "Mata a los <color=#FF0000FF>Impostores</color>",
                        "Examina huellas",
                        "Obten informacion reportando cuerpos y hablando con fantasmas",//75
                        "Obten informacion reportando\ncuerpos y hablando con fantasmas",
                        "Rebobina el tiempo",
                        "Protege a un jugador con tu escudo",
                        "Cambia los votos de dos jugadores",
                        "Revela quien es bueno o malo",//80
                        "Usa Admin y Vitales a distancia",
                        "Rastrea jugadores y cuerpos",
                        "Completa tus tareas para revelear a los <color=#FF0000FF>Impostores</color>",
                        "Todos pierden si mueres o te expulsan",
                        "Sella Rejillas",//85
                        "Sacrificate para revivir a otro jugador",
                        "Convoca reuniones a distancia",
                        "Pon camaras adicionales en el mapa",
                        "Activa el Doorlog a distancia con la Q",
                        "El jugador que marques morira si te matan",//90
                        "Gafa las habilidades de los jugadores",
                        "Reduce la recarga de botones y aumenta la de impostores",
                        "Reduce la recarga\nde botones y aumenta la de impostores",
                        "Lleva un cuerpo a su sala para revivirlo",
                        "Pon trampas de velocidad y posicion",//95
                        "Pon trampas de velocidad y posicion\nAlterna el tipo de trampa con la Q",
                        "Comprueba quien esta cerca",
                        "Completa tus tareas y las extra para ganar",
                        "Completa tus tareas\ny las extra para ganar",
                        "Encarcela jugadores",//100
                        "Sabotea y mata a todos",
                        "Encuentra y expulsa a los <color=#FF0000FF>Impostores</color>",
                        "Tienes mas vision",
                        "Tienes menos vision",
                        "Eres mas rapido",//105
                        "Eres mas grande y lento",
                        "Tu asesino reportara tu cuerpo",
                        "Tu muerte activa una alarma y una flecha revela tu posicion",
                        "Tu muerte activa una alarma\ny una flecha revela tu posicion",
                        "Tus controles estan al reves",//110
                        "Tu asesino dejara un rastro de pintura",
                        "Tu asesino quedara paralizado",
                        "♥Sobrevive en pareja♥",
                        "♥Sobrevive en pareja♥",
                        "<color=#FF00D1FF>♥Sobrevive en pareja♥. </color><color=#FF1919FF>Mata al resto</color>",//115
                        "<color=#FF00D1FF>♥Sobrevive en pareja♥. </color><color=#FF1919FF>Mata al resto</color>",
                        "Consigue mas monjitas\nque el rival",
                    };
                    exileControllerTexts = new string[] {
                        " era el ",
                        "¡Pensabais que era un Impostor pero era yo, Joker!",
                        "¡Y eso es todo amigos!"
                    };
                    introTexts = new string[] {
                        "♥ Sobrevive en pareja con ",
                        "Tiempo Restante: ",
                        "Puntuacion: ",
                        "Joyas Robadas: ",
                        "Ladrones Capturados: ",
                        "Patata Caliente: ",
                        "Patatas Frias: ",
                        "Objetos Clave: ",
                        "Supervivientes: ",
                        "Infectados: ",
                        "Zombies: ",
                        "Luchadores Battle Royale: ",
                        "Equipo Lima: ",
                        "Equipo Rosa: ",
                        "Serial Killer: ",
                        "Objetivo: ",
                        "Puntuacion Serial Killer: ",
                        "Equipo Verde: ",
                        "Equipo Cian: ",
                        "Big Monja: ",
                    };
                    playerControlTexts = new string[] {
                        "¡Parece que ha sido un suicidio!",
                        "¡El asesino parece ser",
                        "color claro (L)!",
                        "color oscuro (D)!",
                        "¡El asesino parece tener un ",
                        "¡El asesino parece llevar gorro!",
                        "¡El asesino parece que no lleva gorro!",
                        "¡El asesino parece llevar traje!",
                        "¡El asesino parece que no lleva traje!",
                        "¡El asesino parece llevar mascota!",
                        "¡El asesino parece que no lleva mascota!",
                        "¡El asesino parece llevar visor!",
                        "¡El asesino parece que no lleva visor!",
                        "¡Ha pasado demasiado tiempo para obtener informacion!",
                        "Tiempo Transcurrido:"
                    };
                    usablesTexts = new string[] {
                        "¡No se puede usar el boton de\nemergencia en los modos de juego!",
                        "¡El Cheater no puede usar\nel boton de emergencia!",
                        "¡El Gambler no puede usar\nel boton de emergencia!",
                        "¡El Sorcerer no puede usar\nel boton de emergencia!",
                        "¡Hay una Bomba, no se puede usar\nel boton de emergencia!",
                        "¡Hay un Apagon, el boton de emergencia no funciona!",
                        "!LA MONJA HA DESPERTADO, CORRED INSENSATOS!"
                    };
                    buttonsTexts = new string[] {
                        "Fantasma de (",
                        "Mi rol era ",
                        "Mi asesino tiene un ",
                        "Segundos desde muerte: ",
                        "Me asesino fue el "
                    };
                    helpersTexts = new string[] {
                        " y recluta un Minion",
                        "Mata a todos"
                    };
                    statusRolesTexts = new string[] {
                        "¡Velocidad Cambiada!",
                        "¡Hipnotizado!",
                        "Objetivo Muerto",
                        "Modo Asesina",
                        "Puntuacion actual del Seeker: ",
                        "El Illusionist ha apagado las luces: ",
                        "Hay una Bomba en el mapa: ",
                        "¡Petrificado!",
                        "LA MONJA HA DESPERTADO: ",
                        "Amnesiac Body Report: Este rol no puede ser recordado",
                        "!El Fink esta espiando!",
                    };
                    statusCaptureTheFlagTexts = new string[] {
                        "¡Ahora eres del <color=#FF0000FF>Equipo Rojo</color>!",
                        "¡Ahora eres del <color=#0000FFFF>Equipo Azul</color>!",
                        "¡<color=#0000FFFF>Bandera Azul</color> robada por <color=#FF0000FF>",
                        "¡Han robado tu bandera!",
                        "¡<color=#FF0000FF>Bandera Roja</color> robada por <color=#0000FFFF>",
                        "¡Punto para el <color=#FF0000FF>Equipo Rojo</color>!",
                        "¡Punto para el <color=#0000FFFF>Equipo Azul</color>!"
                    };
                    statusPoliceAndThiefsTexts = new string[] {
                        "¡Un <color=#928B55FF>Ladron</color> ha sido capturado!",
                        "¡Un <color=#928B55FF>Ladron</color> ha sido liberado!",
                        "¡Se ha entregado una <color=#00F7FFFF>Joya</color>!"
                    };
                    statusKingOfTheHillTexts = new string[] {
                        "¡Eres el nuevo <color=#00FF00FF>Rey Verde</color>!",
                        "¡Eres el nuevo <color=#FFFF00FF>Rey Amarillo</color>!",
                        "¡El <color=#00FF00FF>Rey Verde</color> ha capturado una zona!",
                        "¡El <color=#FFFF00FF>Rey Amarillo</color> ha capturado una zona!",
                        "¡Tu Rey ha sido asesinado!"
                    };
                    statusHotPotatoTexts = new string[] {
                        " es la nueva Patata Caliente!"
                    };
                    statusZombieLaboratoryTexts = new string[] {
                        "¡Se ha entregado un <color=#FF00FFFF>Objeto Clave</color>!",
                        "¡Un <color=#00CCFFFF>Superviviente</color> ha sido <color=#FFFF00FF>Infectado</color>!",
                        "¡Un <color=#00CCFFFF>Superviviente</color> se ha convertido en <color=#996633FF>Zombie</color>!"
                    };
                    statusBattleRoyaleTexts = new string[] {
                        "¡Ha caido un <color=#009F57FF>Luchador</color>!",
                        "¡Ha caido un <color=#39FF14FF>Luchador Lima</color>!",
                        "¡Ha caido un <color=#F2BEFFFF>Luchador Rosa</color>!",
                        "¡Ha caido el <color=#808080FF>Serial Killer</color>!",
                        "¡Puntos para el <color=#39FF14FF>Equipo Lima</color>!",
                        "¡Puntos para el <color=#F2BEFFFF>Equipo Rosa</color>!",
                        "¡Puntos para el <color=#808080FF>Serial Killer</color>!",
                    };
                    statusMonjaFestivalTexts = new string[] {
                        "<color=#808080FF>Big Monja</color> esta robando de tu cesta!",
                        "<color=#FF00FFFF>Allul Monja</color> encontrado por el <color=#00FF00FF>Equipo Verde</color>!",
                        "<color=#FF00FFFF>Allul Monja</color> encontrado por el <color=#00F7FFFF>Equipo Cian</color>!",
                        "<color=#FF00FFFF>Allul Monja</color> encontrado por <color=#808080FF>Big Monja</color>!",
                    };
                    break;
                // Japanese
                case 3:
                    colorNames = new string[5] { "ラベンダー", "ペトロリウム", "ミント", "オリーブ", "アイス" };
                    for (int i = 0; i < colorNames.Count(); i++) {
                        CustomColors.ColorStrings[i + 50000] = colorNames[i];
                    }
                    customOptionNames = new string[] {
                        CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "プリセット"),
                        CustomOptionHolder.cs(Jailer.color, "全体設定"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Mod のロールとゲームモードを有効にします"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "カスタム Skeld マップを有効にします。"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "影のベントのアニメーションを非表示にします"),
                        CustomOptionHolder.cs(Detective.color, "ロールの設定"),
                        "- " + CustomOptionHolder.cs(Detective.color, "ロールモードを探します"),
                        "- " + CustomOptionHolder.cs(Detective.color, "カードをスワイプするタスクを削除します"),
                        "- " + CustomOptionHolder.cs(Detective.color, "飛行船のドアのタスクを削除します。"),
                        "- " + CustomOptionHolder.cs(Detective.color, "照明妨害時の暗視"),
                        "- " + CustomOptionHolder.cs(Detective.color, "原子炉妨害時のスクリーンシェイク"),
                        "- " + CustomOptionHolder.cs(Detective.color, "通信妨害の匿名プレイヤー"),
                        "- " + CustomOptionHolder.cs(Detective.color, "酸素妨害時の速度低下"),
                        CustomOptionHolder.cs(Modifiers.color, "修飾子"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Lovers"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Lighter"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Blind"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Flash"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Big Chungus"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "The Chosen One"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "The Chosen One") + ": レポートの遅れ",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Performer"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Performer") + ": アラーム機関",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Pro"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Paintball"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Paintball") + ": ペイント期間",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Electrician"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Electrician") + ": 放電期間",
                        CustomOptionHolder.cs(Sheriff.color, "Capture the Flag"),
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": 一致期間",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": スコア番号",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": Kill のクールダウン",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": 復活の待ち時間",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": 復活後の無敵時間",
                        CustomOptionHolder.cs(Coward.color, "Police and Thieves"),
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 一致期間",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 宝石番号",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察 Kill のクールダウン",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察のクールダウン逮捕",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 逮捕時間",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察のスタンガンのクールダウン",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察のスタンガンの期間",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察は宝石をみることができる",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察の視界の距離",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察復活の待ち時間",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 泥棒は誰を殺すことができますか", // 45
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 泥棒 Kill のクールダウン",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 泥棒復活の待ち時間",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 復活後の無敵時間",
                        CustomOptionHolder.cs(Squire.color, "King of the Hill"),
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 一致期間",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": スコア番号",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": クールダウンのキャプチャ",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": Kill のクールダウン",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 王は Kill 可能",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 復活の待ち時間",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 復活後の無敵時間",
                        CustomOptionHolder.cs(Shy.color, "Hot Potato"),
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": 一致期間",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": ホットポテトの転送時間制限",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": ホットポテトの転送クールダウン",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": コールドポテトの視界の距離",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": 転送後のホットポテトの時間のリセット",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": タイマーがリセットされない場合の追加時間",
                        CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory"),
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 一致期間",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 最初のゾンビ",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 感染までの時間",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 感染のクールダウン",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": ボックスタイマーの探索",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 生存者の視野の範囲",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 応急処置キットの使用時間",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": Kill のクールダウン",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 復活の待ち時間",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 復活後の無敵時間",
                        CustomOptionHolder.cs(Sleuth.color, "Battle Royale"),
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 一致期間",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 試合の種類", // 77
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": Kill のクールダウン",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 戦士のライフ",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": スコア番号",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 復活の待ち時間",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 復活後の無敵時間",
                        CustomOptionHolder.cs(Monja.color, "Monja Festival"),
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": 一致期間",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": Kill のクールダウン",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": 復活の待ち時間",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": 復活後の無敵時間",
                        CustomOptionHolder.cs(Mimic.color, "Mimic"),
                        "- " + CustomOptionHolder.cs(Mimic.color, "Mimic") + ": 期間",
                        CustomOptionHolder.cs(Painter.color, "Painter"),
                        "- " + CustomOptionHolder.cs(Painter.color, "Painter") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Painter.color, "Painter") + ": 期間",
                        CustomOptionHolder.cs(Demon.color, "Demon"),
                        "- " + CustomOptionHolder.cs(Demon.color, "Demon") + ": 遅延時間",
                        "- " + CustomOptionHolder.cs(Demon.color, "Demon") + ": 修道女の近くでの Kill",
                        CustomOptionHolder.cs(Janitor.color, "Janitor"),
                        "- " + CustomOptionHolder.cs(Janitor.color, "Janitor") + ": クールダウン",
                        CustomOptionHolder.cs(Illusionist.color, "Illusionist"),
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": 防止の来るダウン",
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": 照明のクールダウン",
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": 停電の期間",
                        CustomOptionHolder.cs(Manipulator.color, "Manipulator"),
                        CustomOptionHolder.cs(Bomberman.color, "Bomberman"),
                        "- " + CustomOptionHolder.cs(Bomberman.color, "Bomberman") + ": クールダウン",
                        CustomOptionHolder.cs(Chameleon.color, "Chameleon"),
                        "- " + CustomOptionHolder.cs(Chameleon.color, "Chameleon") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Chameleon.color, "Chameleon") + ": 期間",
                        CustomOptionHolder.cs(Gambler.color, "Gambler"),
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": 撮影番号",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": 緊急ボタンの使用",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": 複数回の撮影",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": シールドの無視",
                        CustomOptionHolder.cs(Sorcerer.color, "Sorcerer"),
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": スペル毎の追加のクールダウン",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": スペルの期間",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": 緊急ボタンの使用",
                        CustomOptionHolder.cs(Medusa.color, "Medusa"),
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": 石化の遅延",
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": 石化期間",
                        CustomOptionHolder.cs(Hypnotist.color, "Hypnotist"),
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": スパイラル番号",
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": スパイラル効果期間",
                        CustomOptionHolder.cs(Archer.color, "Archer"),
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": 矢のサイズ",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": 矢の範囲",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": 通知範囲",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": 標的の範囲",
                        CustomOptionHolder.cs(Plumber.color, "Plumber"),
                        "- " + CustomOptionHolder.cs(Plumber.color, "Plumber") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Plumber.color, "Plumber") + ": ベントの回数",
                        CustomOptionHolder.cs(Librarian.color, "Librarian"),
                        "- " + CustomOptionHolder.cs(Librarian.color, "Librarian") + ": クールダウン",
                        CustomOptionHolder.cs(Renegade.color, "Renegade"),
                        "- " + CustomOptionHolder.cs(Renegade.color, "Renegade") + ": ベントの使用可",
                        "- " + CustomOptionHolder.cs(Renegade.color, "Renegade") + ": ミニオンのリクルート",
                        CustomOptionHolder.cs(BountyHunter.color, "Bounty Hunter"),
                        CustomOptionHolder.cs(Trapper.color, "Trapper"),
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": 鉱山番号",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": 鉱山の期間",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": トラップ番号",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": トラップ期間",
                        CustomOptionHolder.cs(Yinyanger.color, "Yinyanger"),
                        "- " + CustomOptionHolder.cs(Yinyanger.color, "Yinyanger") + ": クールダウン",
                        CustomOptionHolder.cs(Challenger.color, "Challenger"),
                        "- " + CustomOptionHolder.cs(Challenger.color, "Challenger") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Challenger.color, "Challenger") + ": 勝つために殺す",
                        CustomOptionHolder.cs(Ninja.color, "Ninja"),
                        CustomOptionHolder.cs(Berserker.color, "Berserker"),
                        "- " + CustomOptionHolder.cs(Berserker.color, "Berserker") + ": Kill の時間制限",
                        CustomOptionHolder.cs(Yandere.color, "Yandere"),
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": 見つめているクールダウン",
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": 凝視時間",
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": 凝視期間",
                        CustomOptionHolder.cs(Stranded.color, "Stranded"),
                        CustomOptionHolder.cs(Monja.color, "Monja"),
                        CustomOptionHolder.cs(Joker.color, "Joker"),
                        "- " + CustomOptionHolder.cs(Joker.color, "Joker") + ": 妨害可能",
                        CustomOptionHolder.cs(RoleThief.color, "Role Thief"),
                        "- " + CustomOptionHolder.cs(RoleThief.color, "Role Thief") + ": クールダウン",
                        CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac"),
                        "- " + CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac") + ": 点火期間",
                        CustomOptionHolder.cs(TreasureHunter.color, "Treasure Hunter"),
                        "- " + CustomOptionHolder.cs(TreasureHunter.color, "Treasure Hunter") + ": 勝つための宝物",
                        CustomOptionHolder.cs(Devourer.color, "Devourer"),
                        "- " + CustomOptionHolder.cs(Devourer.color, "Devourer") + ": 勝つために貪ります",
                        CustomOptionHolder.cs(Poisoner.color, "Poisoner"),
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": 毒する時間",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": 毒感染範囲",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": 完全に毒する時間",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": 妨害可能",
                        CustomOptionHolder.cs(Puppeteer.color, "Puppeteer"),
                        "- " + CustomOptionHolder.cs(Puppeteer.color, "Puppeteer") + ": キルの数",
                        CustomOptionHolder.cs(Exiler.color, "Exiler"),
                        CustomOptionHolder.cs(Amnesiac.color, "Amnesiac"),
                        CustomOptionHolder.cs(Seeker.color, "Seeker"),
                        "- " + CustomOptionHolder.cs(Seeker.color, "Seeker") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Seeker.color, "Seeker") + ": 勝つためのポイント",
                        CustomOptionHolder.cs(Captain.color, "Captain"),
                        "- " + CustomOptionHolder.cs(Captain.color, "Captain") + ": 一度特別投票できます",
                        CustomOptionHolder.cs(Mechanic.color, "Mechanic"),
                        "- " + CustomOptionHolder.cs(Mechanic.color, "Mechanic") + ": 修理番号",
                        "- " + CustomOptionHolder.cs(Mechanic.color, "Mechanic") + ": 専門家の修理",
                        CustomOptionHolder.cs(Sheriff.color, "Sheriff"),
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Sheriff") + ": ニュートラルを殺すことができます",
                        CustomOptionHolder.cs(Detective.color, "Detective"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": フットプリント", // 191
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": フットプリントの期間を表示します",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": 匿名のフットプリント",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": フットプリント間隔",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": フットプリントの期間",
                        CustomOptionHolder.cs(Forensic.color, "Forensic"),
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 名前を知る時間",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 色のカラーを知る時間",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 殺人者が帽子、服、ペット、またはバイザーを持っているかどうかを知る時間",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 質問期間",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": ゴーストごとに1つの質問",
                        CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler"),
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": シールド期間",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": 巻き戻し期間",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": 巻き戻し中にプレーヤーを復活させます",
                        CustomOptionHolder.cs(Squire.color, "Squire"),
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": シールドプレーヤーを", // 209
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": シールドされた場合、殺人試みの音を再生します",
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": 会った後、再びシールドできます",
                        CustomOptionHolder.cs(Cheater.color, "Cheater"),
                        "- " + CustomOptionHolder.cs(Cheater.color, "Cheater") + ": 緊急ボタンの使用",
                        "- " + CustomOptionHolder.cs(Cheater.color, "Cheater") + ": 自分を交換できます",
                        CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller"),
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 時間を明らかにします",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 数字を明らかにします",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 明らかにされた情報", // 219
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 通知を示す", // 220
                        CustomOptionHolder.cs(Hacker.color, "Hacker"),
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": 期間",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": バッテリーの使用",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": 充電バッテリーのタスク",
                        CustomOptionHolder.cs(Sleuth.color, "Sleuth"),
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": トラック間隔",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": 会議後にもう一度追跡できます",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": 死体のクールダウンを追跡します",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": 死体の期間を追跡します",
                        CustomOptionHolder.cs(Fink.color, "Fink"),
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": 詐欺師に啓示されるためのタスク",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": 明らかにすることができます Renegade / Minion",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": タカの目の期間",
                        CustomOptionHolder.cs(Kid.color, "Kid"),
                        CustomOptionHolder.cs(Welder.color, "Welder"),
                        "- " + CustomOptionHolder.cs(Welder.color, "Welder") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Welder.color, "Welder") + ": シール番号",
                        CustomOptionHolder.cs(Spiritualist.color, "Spiritualist"),
                        "- " + CustomOptionHolder.cs(Spiritualist.color, "Spiritualist") + ": プレーヤーの時間を復活させます",
                        CustomOptionHolder.cs(Vigilant.color, "Vigilant"),
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": リモートカメラの期間",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": バッテリーの使用",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": 充電バッテリーのタスク",
                        CustomOptionHolder.cs(Hunter.color, "Hunter"),
                        "- " + CustomOptionHolder.cs(Hunter.color, "Hunter") + ": 会った後にもう一度マークできます",
                        CustomOptionHolder.cs(Jinx.color, "Jinx"),
                        "- " + CustomOptionHolder.cs(Jinx.color, "Jinx") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Jinx.color, "Jinx") + ": ジンクスナンバー",
                        CustomOptionHolder.cs(Coward.color, "Coward"),
                        "- " + CustomOptionHolder.cs(Coward.color, "Coward") + ": 会議の数",
                        CustomOptionHolder.cs(Bat.color, "Bat"),
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": エミット期間",
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": エミット範囲",
                        CustomOptionHolder.cs(Necromancer.color, "Necromancer"),
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": 復活します",
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": 部屋の距離",
                        CustomOptionHolder.cs(Engineer.color, "Engineer"),
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": トラップ番号",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": トラップ期間",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": 速度の上昇",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": 速度が低下します",
                        CustomOptionHolder.cs(Shy.color, "Shy"),
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": 期間",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": 通知範囲",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": 矢印の色はプレイヤーの色",
                        CustomOptionHolder.cs(TaskMaster.color, "Task Master"),
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 追加の一般的なタスク",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 余分な長いタスク",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 余分な短いタスク",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 速度のクールダウン",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 速度の期間",
                        CustomOptionHolder.cs(Jailer.color, "Jailer"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Jailer") + ": クールダウン",
                        "- " + CustomOptionHolder.cs(Jailer.color, "Jailer") + ": 刑務所の期間"
                    };
                    for (int o = 0; o < customOptionNames.Count(); o++) {
                        CustomOption.options[o].name = customOptionNames[o];
                        switch (o) {
                            case 45:
                                CustomOption.options[o].selections = new string[] { "Taser", "全て", "誰でもない" };
                                break;
                            case 77:
                                CustomOption.options[o].selections = new string[] { "全て vs 全て", "チームバトル", "スコアバトル" };
                                break;
                            case 191:
                                CustomOption.options[o].selections = new string[] { "ボタンの使用", "常に" };
                                break;
                            case 209:
                                CustomOption.options[o].selections = new string[] { "Squire", " 双方", "全て" };
                                break;
                            case 219:
                                CustomOption.options[o].selections = new string[] { " 善い / 悪い", "ロール名" };
                                break;
                            case 220:
                                CustomOption.options[o].selections = new string[] { "詐欺師", "クルー", "全て", "誰でもない" };
                                break;
                        }
                    }
                    roleInfoNames = new string[] {
                        "<color=#0000FFFF>ブルー</color>チームフラグを盗みます",
                        "<color=#FF0000FF>レッド</color>チームフラグを盗みます",
                        "旗でプレイヤーを殺すためにチームを切り替える",
                        "旗でプレイヤーを殺すためにチームを切り替える",
                        "すべての<color=#D2B48CFF>泥棒</color>を捕まえます",
                        "<color=#D2B48CFF>泥棒</color>をターセします",//5
                        "右クリックで<color=#D2B48CFF>Thieves</color>をターセします",
                        "捕らえられることなく、すべての宝石を盗みます",
                        "捕らえられることなく、すべての宝石を盗みます",
                        "ゾーンをキャプチャします",
                        "あなたの王を守ってください", //10
                        "王を殺して1人になります",
                        "ポットポテトを他のプレイヤーに渡します",
                        "ポットポテトを他のプレイヤーに渡します",
                        "<color=#808080FF>ホットポテト</color>から走ります",
                        "あなたは燃えています",//15
                        "生存者を癒し、治療を作成します",
                        "生存者を癒し、治療を作成します",
                        "治療法を作るためのアイテムを探している間生き残り​​ます",
                        "治療法を作るためのアイテムを探している間生き残り​​ます",
                        "すべての生存者に感染します",//20
                        "生きている最後のものになりなさい",
                        "<color=#F2BEFFFF>ピンクのチーム</color>を殺します",
                        "<color=#39FF14FF>ライムチーム</color>を殺します",
                        "他のプレイヤーの外観を模倣します",
                        "同じ色のプレイヤーをペイントします",//25
                        "彼らの死を遅らせるためにプレイヤーを噛みます",
                        "犯罪現場から体を取り除き、移動します",
                        "犯罪現場から体を取り除き、移動します",
                        "独自のベントネットワークを作成し、ライトをオフにします",
                        "独自のベントネットワークを作成し\nライトをオフにします", //30
                        "プレイヤーを操作して隣接する",
                        "爆弾を置くことによる妨害",
                        "自分を見えないようにしてください",
                        "会議中に自分の役割を選択するプレイヤーを撃ちます",
                        "会議中に自分の役割を選択するプレイヤーを撃ちます",//35
                        "プレイヤーに呪文をキャストします",
                        "プレイヤーを石化します",
                        "プレーヤーの動きを反転させます",
                        "範囲キルをする",
                        "Fと右クリックで弓を選んで撃ちます", //40
                        "新しい通気口を作成します",
                        "彼が話すのを防ぐためにプレイヤーを沈黙させます",
                        "彼が話すのを防ぐためにプレイヤーを沈黙させます",
                        "ミニオンを募集し、みんなを殺します",
                        "反逆者がみんなを殺すのを手伝ってください",//45
                        "ターゲットを追い詰めます",
                        "プランスランドマインとルートトラップ",
                        "2人のプレイヤーが衝突した場合に死ぬようにマークします",
                        "2人のプレイヤーが衝突した場合に死ぬようにマークします",
                        "ロックペーパーシッサーの決闘にプレイヤーに挑戦します",//50
                        "ロックペーパーシッサーの決闘にプレイヤーに挑戦します",
                        "マークとダブルキルを行います",
                        "殺すのを止めることはできません",
                        "茎とターゲットを殺します",
                        "弾薬を見つけて、3人のプレイヤーを殺します",//55
                        "アイテムを回復し、モンジャに変身します",
                        "アイテムを回復し、モンジャに変身します",
                        "投票して勝ち",
                        "投票して勝ち\nマップを開いて妨害ボタンをアクティブにします",
                        "他のプレイヤーの役割を盗みます",//60
                        "すべての生存者に勝つために発火します",
                        "勝つための宝物を見つけます",
                        "勝つために体をむさぼり食う",
                        "すべてのプレイヤーを毒して勝ち",
                        "すべてのプレイヤーを毒して勝ち\nマップを開いて妨害ボタンをアクティブにする",//65
                        "ダミーを作り、彼らを殺して勝ちます",
                        "ダミーを作り\n彼らを殺して勝ちます",
                        "あなたのターゲットを勝ち取るために投票してください",
                        "身体を報告する役割を覚えておいてください",
                        "かくれんぼをするポイントを獲得します",//70
                        "あなたの投票は2回カウントされます",
                        "船の妨害行為を修理します",
                        "詐欺師を殺します",
                        "フットプリントを調べます",
                        "身体を報告し、幽霊に尋ねる手がかりを見つけてください",//75
                        "身体を報告し\n幽霊に尋ねる手がかりを見つけてください",
                        "時間を巻き戻します",
                        "シールドでプレーヤーを保護します",
                        "2人のプレーヤーの投票を交換します",
                        "誰が詐欺師であるかを明らかにします",//80
                        "どこからでも管理者とバイタルを使用します",
                        "プレーヤーと死体を追跡します",
                        "詐欺師を明らかにするためにタスクを完了します",
                        "あなたが死んだり投票したりすると、誰もが負けます",
                        "ベントシール",//85
                        "自分を犠牲にしてプレイヤーを復活させる",
                        "どこからでも会議に電話します",
                        "追加のカメラをマップに置きます",
                        "Qキーを使用してリモートドアログをアクティブにします",
                        "あなたが殺されたら死ぬようにプレーヤーをマークしてください",//90
                        "ジンクスプレーヤーの能力",
                        "ボタンのクールダウンを削減し、詐欺師のボタンを増やします",
                        "ボタンのクールダウンを削減し\n詐欺師のボタンを増やします",
                        "体をその部屋に持って行き、それを復活させてください",
                        "速度と位置トラップを配置し",//95
                        "速度と位置トラップを配置し\nQキーでトラップタイプを切り替えます",
                        "クローズプレイヤーを確認してください",
                        "すべてのタスクを完了し、勝つための余分なタスクを作成します",
                        "すべてのタスクを完了し\n勝つための余分なタスクを作成します",
                        "殺人者を刑務所に送ります",//100
                        "妨害し、みんなを殺します",
                        "詐欺師を見つけて亡命します",
                        "あなたはより多くのビジョンを持っています",
                        "あなたは視力が少ないです",
                        "あなたはより速いです",//105
                        "あなたは大きくて遅くなります",
                        "あなたの殺害はあなたの体を報告します",
                        "あなたの死はアラームを引き起こし、あなたの体がどこにあるかを明らかにします",
                        "あなたの死はアラームを引き起こし\nあなたの体がどこにあるかを明らかにします",
                        "あなたの動きのコントロールは反転します",//110
                        "あなたのキラーはペイントトレイルを残します",
                        "あなたの殺人者は所定の位置に麻痺します",
                        "♥あなたのパートナーと一緒にカップルとして生き残ります。♥",
                        "♥あなたのパートナーと一緒にカップルとして生き残ります。♥",
                        "<color=#FF00D1FF>♥あなたのパートナーと一緒にカップルとして生き残ります。♥. </color><color=#FF1919FF>残りを殺します</color>",//115
                        "<color=#FF00D1FF>♥あなたのパートナーと一緒にカップルとして生き残ります。♥. </color><color=#FF1919FF>残りを殺します</color>",
                        "より多くのアイテムを取得\n他のチームより",
                    };
                    exileControllerTexts = new string[] {
                        " だった ",
                        "インポスターと思ったようですが私は Joker！",
                        "以上！"
                    };
                    introTexts = new string[] {
                        "♥ とカップルとして生き残る ",
                        "残り時間: ",
                        "スコア: ",
                        "盗まれた宝石: ",
                        "捕まえられた盗賊: ",
                        "ホットポテト: ",
                        "コールドポテト: ",
                        "キーアイテム: ",
                        "生存者: ",
                        "感染者: ",
                        "ゾンビ: ",
                        "バトルロイヤルファイターズ: ",
                        "ライムチーム: ",
                        "ピンクチーム: ",
                        "連続殺人犯: ",
                        "ゴール: ",
                        "連続殺人犯ポイント: ",
                        "グリーンチーム: ",
                        "シアンチーム: ",
                        "ビッグもんじゃ: ",
                    };
                    playerControlTexts = new string[] {
                        "自殺のようです!",
                        "殺人者はそうあるように見えます",
                        "明るい (L) 色です！",
                        "暗い (D) 色です！",
                        "私を殺した人は",
                        "キラーは帽子を着用している！",
                        "キラーは帽子着ていないようです！",
                        "キラーは衣装を着用している！",
                        "キラーは衣装着ていないようです！",
                        "キラーはペットを着用している！",
                        "キラーはペット着ていないようです！",
                        "キラーはバイザーを着用している！",
                        "キラーはバイザー着ていないようです！",
                        "体は古すぎて情報を得られません！",
                        "時間が経過した:"
                    };
                    usablesTexts = new string[] {
                        "カスタムモードでは緊急招集ボタンを使用できません！",
                        "Cheater 非常ボタンが使えない！",
                        "Gambler 非常ボタンが使えない！",
                        "Sorcerer 非常ボタンが使えない！",
                        "爆弾があります、緊急招集ボタンは使用できません！",
                        "停電中です。緊急招集ボタンは使用できません！",
                        "MONJA 目覚めた, 走る！"
                    };
                    buttonsTexts = new string[] {
                        "Player のゴースト (",
                        "私はでした ",
                        "私を殺した人は ",
                        "亡くなってからの時間 ",
                        "私を殺した人の役割はです "
                    };
                    helpersTexts = new string[] {
                        " ミニオンを募集します。",
                        "みんなを殺します"
                    };
                    statusRolesTexts = new string[] {
                        "速度の変更！",
                        "眠らされた！",
                        "ターゲットは死にました！",
                        "ランページモード",
                        "Seeker 現在のポイント: ",
                        "Illusionist は明かりを消した: ",
                        "地図に爆弾があります: ",
                        "石化！",
                        "MONJA 目覚めた: ",
                        "Amnesiac Body Report: この役割を思い出させることはできません",
                        "Fink はホークアイを使用しています！",
                    };
                    statusCaptureTheFlagTexts = new string[] {
                        "あなたは今新しい<color=#FF0000FF>レッドチームプレーヤーです！</color>",
                        "あなたは今新しい<color=#0000FFFF>ブルーチームプレーヤーです！</color>",
                        "によって盗まれ<color=#0000FFFF>た青い旗</color> <color=#FF0000FF>",
                        "あなたの旗は盗まれました！",
                        "によって盗まれ<color=#FF0000FF>た赤い旗</color> <color=#0000FFFF>",
                        "<color=#FF0000FF>レッドチーム</color>が得点しました！",
                        "<color=#0000FFFF>ブルーチーム</color>が得点しました！"
                    };
                    statusPoliceAndThiefsTexts = new string[] {
                        "<color=#928B55FF>泥棒</color>が捕らえられました！",
                        "<color=#928B55FF>泥棒</color>が釈放されました！",
                        "<color=#00F7FFFF>宝石</color>が配達されました！"
                    };
                    statusKingOfTheHillTexts = new string[] {
                        "あなたは新しい<color=#00FF00FF>緑王です</color>！",
                        "あなたは新しい<color=#FFFF00FF>黄色の王です</color>！",
                        "<color=#00FF00FF>緑王です</color>キングがゾーンを獲得しました！",
                        "<color=#FFFF00FF>黄色の王です</color>キングがゾーンを獲得しました！",
                        "あなたの王は殺されました！"
                    };
                    statusHotPotatoTexts = new string[] {
                        " は新しいホットポテトです！"
                    };
                    statusZombieLaboratoryTexts = new string[] {
                        "<color=#FF00FFFF>重要なアイテ</color>ムが配信されました！",
                        "<color=#00CCFFFF>生存者</color><color=#FFFF00FF>が感染しています</color>！",
                        "<color=#00CCFFFF>生存者</color><color=#996633FF>はゾンビになりました</color>！"
                    };
                    statusBattleRoyaleTexts = new string[] {
                        "一人の <color=#009F57FF>戦闘機</color>ダウン！",
                        "<color=#39FF14FF>ライム戦闘機</color>ダウン！",
                        "一人の <color=#F2BEFFFF>ピンクの戦闘機</color>ダウン！",
                        "<color=#808080FF>シリアルキラーダウン</color>ダウン！",
                        "<color=#39FF14FF>ライムのチーム</color>ポイント！",
                        "<color=#F2BEFFFF>ピンクのチーム</color>ポイント！",
                        "<color=#808080FF>シリアルキラーの</color>ポイント！",
                    };
                    statusMonjaFestivalTexts = new string[] {
                        "大きなもんじゃがあなたのかごから盗んでいます！",
                        "<color=#FF00FFFF>Allul Monja</color> 被绿队发现！",
                        "<color=#FF00FFFF>Allul Monja</color> 由青色团队发现！",
                        "<color=#FF00FFFF>Allul Monja</color> 发现者 <color=#808080FF>Big Monja</color>！",
                    };
                    break;
                // Chinese
                case 4:
                    colorNames = new string[5] { "薰衣紫", "深绿色", "薄荷色", "橄榄绿", "冰蓝色" };
                    for (int i = 0; i < colorNames.Count(); i++) {
                        CustomColors.ColorStrings[i + 50000] = colorNames[i];
                    }
                    customOptionNames = new string[] {
                        CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), "预设"),
                        CustomOptionHolder.cs(Jailer.color, "全局设置"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "激活修道院模组专属职业和模式"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "激活千年隼地图"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "隐藏关灯时使用管道动画"),
                        CustomOptionHolder.cs(Detective.color, "职业设置"),
                        "- " + CustomOptionHolder.cs(Detective.color, "启用模组职业"),
                        "- " + CustomOptionHolder.cs(Detective.color, "移除刷卡任务"),
                        "- " + CustomOptionHolder.cs(Detective.color, "移除飞艇地图的门"),
                        "- " + CustomOptionHolder.cs(Detective.color, "破坏灯光时监控不显示"),
                        "- " + CustomOptionHolder.cs(Detective.color, "破坏反应堆时屏幕晃动"),
                        "- " + CustomOptionHolder.cs(Detective.color, "破坏通讯进入隐蔽状态"),
                        "- " + CustomOptionHolder.cs(Detective.color, "破坏氧气时降低速度"),
                        CustomOptionHolder.cs(Modifiers.color, "Modifiers"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Lovers"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Lighter"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Blind"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Flash"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Big Chungus"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "The Chosen One"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "The Chosen One") + ": 报告延迟",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Performer"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Performer") + ": 警报持续时间",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Pro"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Paintball"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Paintball") + ": 隐蔽持续时间",
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Electrician"),
                        "- " + CustomOptionHolder.cs(Modifiers.color, "Electrician") + ": 释放持续时间",
                        CustomOptionHolder.cs(Sheriff.color, "Capture the Flag"),
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": 比赛时长",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": 分数",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": 击杀冷却时间",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": 复活等待时间",
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Capture the Flag") + ": 复活无敌时间",
                        CustomOptionHolder.cs(Coward.color, "Police and Thieves"),
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 比赛时长",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 宝石数量",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察击杀冷却时间",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察抓捕冷却时间",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 抓捕所需时间",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察电击枪冷却时间",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察电击效果持续时间",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察可以看见宝石",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察视野范围",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 警察复活等待时间",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 小偷可以击杀", // 45
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 小偷击杀冷却时间",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 小偷复活等待时间",
                        "- " + CustomOptionHolder.cs(Coward.color, "Police and Thieves") + ": 复活无敌时间",
                        CustomOptionHolder.cs(Squire.color, "King of the Hill"),
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 比赛时长",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 分数",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 偷取冷却时间",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 击杀冷却时间",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 国王可以击杀",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 复活等待时间",
                        "- " + CustomOptionHolder.cs(Squire.color, "King of the Hill") + ": 复活无敌时间",
                        CustomOptionHolder.cs(Shy.color, "Hot Potato"),
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": 比赛时长",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": 烫山芋爆炸倒计时",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": 传递烫山芋冷却时间",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": 冷山芋视野范围",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": 传递烫山芋重置爆炸时间",
                        "- " + CustomOptionHolder.cs(Shy.color, "Hot Potato") + ": 不重置倒计时时追加时间",
                        CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory"),
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 比赛时长",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 初始丧尸",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 丧尸感染所需时间",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 丧尸感染冷却时间",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 幸存者打开箱子所需时间",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 幸存者视野",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 护士治愈所需时间",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 击杀冷却时间",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 复活等待时间",
                        "- " + CustomOptionHolder.cs(Hunter.color, "Zombie Laboratory") + ": 复活无敌时间",
                        CustomOptionHolder.cs(Sleuth.color, "Battle Royale"),
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 比赛时长",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 游戏类型", // 77
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 击杀冷却时间",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 战士的生命",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 分数",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 复活等待时间",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Battle Royale") + ": 复活无敌时间",
                        CustomOptionHolder.cs(Monja.color, "Monja Festival"),
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": 比赛时长",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": 击杀冷却时间",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": 复活等待时间",
                        "- " + CustomOptionHolder.cs(Monja.color, "Monja Festival") + ": 复活无敌时间",
                        CustomOptionHolder.cs(Mimic.color, "Mimic"),
                        "- " + CustomOptionHolder.cs(Mimic.color, "Mimic") + ": 持续时间",
                        CustomOptionHolder.cs(Painter.color, "Painter"),
                        "- " + CustomOptionHolder.cs(Painter.color, "Painter") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Painter.color, "Painter") + ": 持续时间",
                        CustomOptionHolder.cs(Demon.color, "Demon"),
                        "- " + CustomOptionHolder.cs(Demon.color, "Demon") + ": 延迟时间",
                        "- " + CustomOptionHolder.cs(Demon.color, "Demon") + ": 可以在修女附近击杀",
                        CustomOptionHolder.cs(Janitor.color, "Janitor"),
                        "- " + CustomOptionHolder.cs(Janitor.color, "Janitor") + ": 冷却时间",
                        CustomOptionHolder.cs(Illusionist.color, "Illusionist"),
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": 帽子冷却时间",
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": 关灯冷却时间",
                        "- " + CustomOptionHolder.cs(Illusionist.color, "Illusionist") + ": 关灯持续时间",
                        CustomOptionHolder.cs(Manipulator.color, "Manipulator"),
                        CustomOptionHolder.cs(Bomberman.color, "Bomberman"),
                        "- " + CustomOptionHolder.cs(Bomberman.color, "Bomberman") + ": 冷却时间",
                        CustomOptionHolder.cs(Chameleon.color, "Chameleon"),
                        "- " + CustomOptionHolder.cs(Chameleon.color, "Chameleon") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Chameleon.color, "Chameleon") + ": 持续时间",
                        CustomOptionHolder.cs(Gambler.color, "Gambler"),
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": 猜测次数",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": 可以使用紧急会议",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": 可以多次猜测",
                        "- " + CustomOptionHolder.cs(Gambler.color, "Gambler") + ": 无视护盾",
                        CustomOptionHolder.cs(Sorcerer.color, "Sorcerer"),
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": 每次诅咒追加冷却时间",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": 诅咒所需时间",
                        "- " + CustomOptionHolder.cs(Sorcerer.color, "Sorcerer") + ": 可以使用紧急会议",
                        CustomOptionHolder.cs(Medusa.color, "Medusa"),
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": 石化延迟",
                        "- " + CustomOptionHolder.cs(Medusa.color, "Medusa") + ": 石化持续时间",
                        CustomOptionHolder.cs(Hypnotist.color, "Hypnotist"),
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": 技能次数",
                        "- " + CustomOptionHolder.cs(Hypnotist.color, "Hypnotist") + ": 技能持续时间",
                        CustomOptionHolder.cs(Archer.color, "Archer"),
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": 箭矢大小",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": 攻击范围",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": 攻击警报范围",
                        "- " + CustomOptionHolder.cs(Archer.color, "Archer") + ": 目标时限",
                        CustomOptionHolder.cs(Plumber.color, "Plumber"),
                        "- " + CustomOptionHolder.cs(Plumber.color, "Plumber") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Plumber.color, "Plumber") + ": 管道数量",
                        CustomOptionHolder.cs(Librarian.color, "Librarian"),
                        "- " + CustomOptionHolder.cs(Librarian.color, "Librarian") + ": 冷却时间",
                        CustomOptionHolder.cs(Renegade.color, "Renegade"),
                        "- " + CustomOptionHolder.cs(Renegade.color, "Renegade") + ": 可以使用管道",
                        "- " + CustomOptionHolder.cs(Renegade.color, "Renegade") + ": 可以招募小弟",
                        CustomOptionHolder.cs(BountyHunter.color, "Bounty Hunter"),
                        CustomOptionHolder.cs(Trapper.color, "Trapper"),
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": 地雷放置数量",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": 地雷持续时间",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": 陷阱数量",
                        "- " + CustomOptionHolder.cs(Trapper.color, "Trapper") + ": 陷阱持续时间",
                        CustomOptionHolder.cs(Yinyanger.color, "Yinyanger"),
                        "- " + CustomOptionHolder.cs(Yinyanger.color, "Yinyanger") + ": 冷却时间",
                        CustomOptionHolder.cs(Challenger.color, "Challenger"),
                        "- " + CustomOptionHolder.cs(Challenger.color, "Challenger") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Challenger.color, "Challenger") + ": 殺來贏",
                        CustomOptionHolder.cs(Ninja.color, "Ninja"),
                        CustomOptionHolder.cs(Berserker.color, "Berserker"),
                        "- " + CustomOptionHolder.cs(Berserker.color, "Berserker") + ": 击杀时限",
                        CustomOptionHolder.cs(Yandere.color, "Yandere"),
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": 追踪冷却时间",
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": 追踪时间",
                        "- " + CustomOptionHolder.cs(Yandere.color, "Yandere") + ": 追踪持续时间",
                        CustomOptionHolder.cs(Stranded.color, "Stranded"),
                        CustomOptionHolder.cs(Monja.color, "Monja"),
                        CustomOptionHolder.cs(Joker.color, "Joker"),
                        "- " + CustomOptionHolder.cs(Joker.color, "Joker") + ": 可以破坏",
                        CustomOptionHolder.cs(RoleThief.color, "Role Thief"),
                        "- " + CustomOptionHolder.cs(RoleThief.color, "Role Thief") + ": 冷却时间",
                        CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac"),
                        "- " + CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Pyromaniac.color, "Pyromaniac") + ": 无视持续时间",
                        CustomOptionHolder.cs(TreasureHunter.color, "Treasure Hunter"),
                        "- " + CustomOptionHolder.cs(TreasureHunter.color, "Treasure Hunter") + ": 寻宝成功",
                        CustomOptionHolder.cs(Devourer.color, "Devourer"),
                        "- " + CustomOptionHolder.cs(Devourer.color, "Devourer") + ": 吞噬尸体获得胜利",
                        CustomOptionHolder.cs(Poisoner.color, "Poisoner"),
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": 下毒所需时间",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": 病毒感染范围",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": 病毒感染时间",
                        "- " + CustomOptionHolder.cs(Poisoner.color, "Poisoner") + ": 可以破坏",
                        CustomOptionHolder.cs(Puppeteer.color, "Puppeteer"),
                        "- " + CustomOptionHolder.cs(Puppeteer.color, "Puppeteer") + ": 击杀次数",
                        CustomOptionHolder.cs(Exiler.color, "Exiler"),
                        CustomOptionHolder.cs(Amnesiac.color, "Amnesiac"),
                        CustomOptionHolder.cs(Seeker.color, "Seeker"),
                        "- " + CustomOptionHolder.cs(Seeker.color, "Seeker") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Seeker.color, "Seeker") + ": 分数达标",
                        CustomOptionHolder.cs(Captain.color, "Captain"),
                        "- " + CustomOptionHolder.cs(Captain.color, "Captain") + ": 可以投出特别的一票",
                        CustomOptionHolder.cs(Mechanic.color, "Mechanic"),
                        "- " + CustomOptionHolder.cs(Mechanic.color, "Mechanic") + ": 维修次数",
                        "- " + CustomOptionHolder.cs(Mechanic.color, "Mechanic") + ": 专业维修",
                        CustomOptionHolder.cs(Sheriff.color, "Sheriff"),
                        "- " + CustomOptionHolder.cs(Sheriff.color, "Sheriff") + ": 可以击杀中立阵营",
                        CustomOptionHolder.cs(Detective.color, "Detective"),
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": 显示脚印", // 191
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": 脚印显示时间",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": 匿名脚印",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": 脚印生成间隔",
                        "- " + CustomOptionHolder.cs(Detective.color, "Detective") + ": 脚印持续时间",
                        CustomOptionHolder.cs(Forensic.color, "Forensic"),
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 显示凶手名字报告时间",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 显示凶手颜色类型报告时间",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 显示凶手是否有帽子、衣服、宠物或面罩报告时间",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 询问持续时间",
                        "- " + CustomOptionHolder.cs(Forensic.color, "Forensic") + ": 每个灵魂只能询问一次",
                        CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler"),
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": 时之盾持续时间",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": 回溯持续时间",
                        "- " + CustomOptionHolder.cs(TimeTraveler.color, "Time Traveler") + ": 回溯过程中复活玩家",
                        CustomOptionHolder.cs(Squire.color, "Squire"),
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": 展示被庇护的玩家", // 209
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": 被庇护玩家被尝试击杀时播放音效",
                        "- " + CustomOptionHolder.cs(Squire.color, "Squire") + ": 会议结束后重置护盾",
                        CustomOptionHolder.cs(Cheater.color, "Cheater"),
                        "- " + CustomOptionHolder.cs(Cheater.color, "Cheater") + ": 可以使用紧急会议",
                        "- " + CustomOptionHolder.cs(Cheater.color, "Cheater") + ": 可以交换自己",
                        CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller"),
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 预言所需时间",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 预言次数",
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 预言信息", // 219
                        "- " + CustomOptionHolder.cs(FortuneTeller.color, "Fortune Teller") + ": 显示通知给", // 220
                        CustomOptionHolder.cs(Hacker.color, "Hacker"),
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": 持续时间",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": 电池用途",
                        "- " + CustomOptionHolder.cs(Hacker.color, "Hacker") + ": 充能需要完成的任务数",
                        CustomOptionHolder.cs(Sleuth.color, "Sleuth"),
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": 追踪箭头更新时间",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": 会议后可重置追踪目标",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": 追踪尸体冷却时间",
                        "- " + CustomOptionHolder.cs(Sleuth.color, "Sleuth") + ": 追踪尸体持续时间",
                        CustomOptionHolder.cs(Fink.color, "Fink"),
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": 揭发内鬼的剩余任务",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": 与叛乱者阵营相互发现 Renegade / Minion",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Fink.color, "Fink") + ": 鹰眼持续时间",
                        CustomOptionHolder.cs(Kid.color, "Kid"),
                        CustomOptionHolder.cs(Welder.color, "Welder"),
                        "- " + CustomOptionHolder.cs(Welder.color, "Welder") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Welder.color, "Welder") + ": 密封管道次数",
                        CustomOptionHolder.cs(Spiritualist.color, "Spiritualist"),
                        "- " + CustomOptionHolder.cs(Spiritualist.color, "Spiritualist") + ": 复活玩家所需时间",
                        CustomOptionHolder.cs(Vigilant.color, "Vigilant"),
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": 远程查看监控持续时间",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": 电池用途",
                        "- " + CustomOptionHolder.cs(Vigilant.color, "Vigilant") + ": 充能需要完成的任务数",
                        CustomOptionHolder.cs(Hunter.color, "Hunter"),
                        "- " + CustomOptionHolder.cs(Hunter.color, "Hunter") + ": 会议后重置标记",
                        CustomOptionHolder.cs(Jinx.color, "Jinx"),
                        "- " + CustomOptionHolder.cs(Jinx.color, "Jinx") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Jinx.color, "Jinx") + ": 把霉运分给其他人",
                        CustomOptionHolder.cs(Coward.color, "Coward"),
                        "- " + CustomOptionHolder.cs(Coward.color, "Coward") + ": 会议次数",
                        CustomOptionHolder.cs(Bat.color, "Bat"),
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": 技能冷却时间",
                        "- " + CustomOptionHolder.cs(Bat.color, "Bat") + ": 发出范围",
                        CustomOptionHolder.cs(Necromancer.color, "Necromancer"),
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": 复活时间",
                        "- " + CustomOptionHolder.cs(Necromancer.color, "Necromancer") + ": 房间距离",
                        CustomOptionHolder.cs(Engineer.color, "Engineer"),
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": 陷阱数量",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": 陷阱持续时间",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": 速度加快",
                        "- " + CustomOptionHolder.cs(Engineer.color, "Engineer") + ": 速度减慢",
                        CustomOptionHolder.cs(Shy.color, "Shy"),
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": 持续时间",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": 攻击警报范围",
                        "- " + CustomOptionHolder.cs(Shy.color, "Shy") + ": 箭头颜色是玩家颜色",
                        CustomOptionHolder.cs(TaskMaster.color, "Task Master"),
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 额外的普通任务",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 额外的长任务",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 额外的短任务",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 速度冷却时间",
                        "- " + CustomOptionHolder.cs(TaskMaster.color, "Task Master") + ": 速度持续时间",
                        CustomOptionHolder.cs(Jailer.color, "Jailer"),
                        "- " + CustomOptionHolder.cs(Jailer.color, "Jailer") + ": 冷却时间",
                        "- " + CustomOptionHolder.cs(Jailer.color, "Jailer") + ": 监禁时间"
                    };
                    for (int o = 0; o < customOptionNames.Count(); o++) {
                        CustomOption.options[o].name = customOptionNames[o];
                        switch (o) {
                            case 45:
                                CustomOption.options[o].selections = new string[] { "Taser", "所有", "无" };
                                break;
                            case 77:
                                CustomOption.options[o].selections = new string[] { "所有 VS 所有", "团队", "分数" };
                                break;
                            case 191:
                                CustomOption.options[o].selections = new string[] { "技能按钮", "始终" };
                                break;
                            case 209:
                                CustomOption.options[o].selections = new string[] { "Squire", "两者", "所有" };
                                break;
                            case 219:
                                CustomOption.options[o].selections = new string[] { "好的 / 坏的", "角色名称" };
                                break;
                            case 220:
                                CustomOption.options[o].selections = new string[] { "内鬼", "船员", "所有", "无" };
                                break;
                        }
                    }
                    roleInfoNames = new string[] {
                        "窃取<color=#0000FFFF>蓝队</color>旗帜",
                        "窃取<color=#FF0000FF>红队</color>旗帜",
                        "杀死拿着旗子的玩家，与它交换队伍",
                        "杀死拿着旗子的玩家, 与它交换队伍",
                        "捕获所有的<color=#D2B48CFF>盗贼</color>",
                        "用右键击打<color=#D2B48CFF>盗贼</color>",//5
                        "用右键击打<color=#D2B48CFF>盗贼</color>",
                        "偷取所有珠宝而不被抓获",
                        "偷取所有珠宝而不被抓获",
                        "占领区域",
                        "保护你的国王", //10
                        "杀死一个国王并替代他成为国王",
                        "把热的山芋送给其他玩家",
                        "把热的山芋送给其他玩家",
                        "远离<color=#808080FF>热山芋</color>",
                        "你被烫伤了",//15
                        "寻找解药并治疗正在被感染的幸存者",
                        "寻找解药并治疗正在被感染的幸存者",
                        "生存下去，同时寻找物品来获得",
                        "生存下去，同时寻找物品来获得",
                        "感染所有幸存者",//20
                        "成为最后一个活着的人",
                        "杀死<color=#F2BEFFFF>粉队</color>",
                        "杀死<color=#39FF14FF>灰队</color>",
                        "模仿其他玩家的造型",
                        "将玩家变成相同颜色和外形",//25
                        "咬住玩家以延迟他们的死亡",
                        "从犯罪现场移走尸体",
                        "从犯罪现场移走尸体",
                        "创建你自己的通风网络并关上灯",
                        "创建你自己的通风网络并关上灯", //30
                        "操纵玩家杀死他附近的玩家",
                        "放置炸弹破坏",
                        "让自己隐身",
                        "会议期间猜测玩家的职业",
                        "会议期间猜测玩家的职业",//35
                        "对玩家施放长矛",
                        "到处巡逻",
                        "反转玩家的移动方向",
                        "用F键选取弓箭，然后点击右键进行射击",
                        "用F键选取弓箭，然后点击右键进行射击", //40
                        "建造新的通风口",
                        "嘘，不要说话~",
                        "嘘，不要说话~",
                        "招募一个小弟，杀光所有人",
                        "帮助叛徒杀死所有人",//45
                        "猎取你的目标",
                        "埋设地雷和尖刺陷阱",
                        "你们只是阴阳之人罢了",
                        "你们只是阴阳之人罢了",
                        "猜拳来决一胜负吧",//50
                        "猜拳来决一胜负吧",
                        "标记并进行双杀",
                        "你越杀越兴奋，根本停不下来！！",
                        "追踪并杀死你的目标",
                        "寻找弹药并杀死3名玩家",//55
                        "找回你的物品，变身为 Monja",
                        "找回你的物品，变身为 Monja",
                        "想办法被放逐",
                        "想办法被放逐，打开地图，激活破坏按钮",
                        "窃取其他玩家的职业",//60
                        "燃烧吧，多么美妙的火焰！",
                        "宝藏！",
                        "吃尸体，吃更多尸体",
                        "带给船员们瘟疫吧",
                        "带给船员们瘟疫吧，打开地图，激活破坏的按钮",//65
                        "制作傀儡，让傀儡被杀以获得胜利",
                        "制作傀儡，让傀儡被杀以获得胜利",
                        "驱逐你的目标",
                        "记住你职业的作用报告尸体",
                        "捉迷藏获得积分",//70
                        "你的票数是其他人的两倍",
                        "快速修理船上的破坏",
                        "执法<color=#FF0000FF>内鬼</color>！",
                        "通过脚印痕迹找出内鬼",
                        "通过报告尸体和询问灵魂得到线索",//75
                        "通过报告尸体和询问灵魂得到线索",
                        "时光倒流",
                        "用你的护盾保护一名玩家",
                        "互换两名玩家的票",
                        "找出谁是真正的<color=#FF0000FF>内鬼</color>",//80
                        "从任何地方使用管理面板和生命检测仪",
                        "追踪玩家和寻找尸体",
                        "完成你的任务，找出<color=#FF0000FF>内鬼</color>",
                        "你是个孩子，没人会伤害你",
                        "把通风口堵死",//85
                        "牺牲自己来复活玩家",
                        "从任何地方发起会议",
                        "安装更多的监控在地图上",
                        "用Q键激活遥控门锁",
                        "如果你被杀，标记一位玩家同归于尽",//90
                        "把霉运分给其他人",
                        "减少技能的冷却时间，并增加内鬼技能的冷却时间。",
                        "减少技能的冷却时间，并增加内鬼技能的冷却时间。",
                        "将尸体拖到指定位置并等待",
                        "设置速度和位置机关",//95
                        "设置速度和位置机关，用Q键切换陷阱类型",
                        "察觉附近的玩家",
                        "完成所有的任务，并完成额外的任务来赢得胜利。",
                        "完成所有的任务，并完成额外的任务来赢得胜利。",
                        "把凶手送进监狱",//100
                        "破坏设备并杀光所有人",
                        "找出并驱逐<color=#FF0000FF>内鬼</color>",
                        "你有更广阔的视野",
                        "你的视野变窄了",
                        "你的速度更快了",//105
                        "你更大了，但也更慢了",
                        "杀害你的人会报告你的尸体",
                        "你的死亡会触发警报，并显示你的尸体在哪里。",
                        "你的死亡会触发警报，并显示你的尸体在哪里。",
                        "你移动的方向颠倒了",//110
                        "杀害你的凶手留下血液痕迹",
                        "杀害你的凶手会短暂的无法移动",
                        "♥与你的恋人一起活到最后♥",
                        "♥与你的恋人一起活到最后♥",
                        "<color=#FF00D1FF>♥与你的恋人一起活到最后♥. </color><color=#FF1919FF>并杀掉其他人</color>",//115
                        "<color=#FF00D1FF>♥与你的恋人一起活到最后♥. </color><color=#FF1919FF>并杀掉其他人</color>",
                        "获取更多项目\n比其他球队",
                    };
                    exileControllerTexts = new string[] {
                        " 是 ",
                        "没想到吧, Joker!",
                        "这就是所有的人!"
                    };
                    introTexts = new string[] {
                        "♥ 一起活到最后 ",
                        "剩余时间: ",
                        "分数: ",
                        "被盗的珠宝: ",
                        "被盗的珠宝: ",
                        "烫手山芋: ",
                        "冷山芋: ",
                        "关键线索: ",
                        "幸存者: ",
                        "你被感染了: ",
                        "丧尸: ",
                        "丛林大逃杀: ",
                        "灰色阵营: ",
                        "粉色阵营: ",
                        "连环杀手: ",
                        "目标: ",
                        "连环杀手分数: ",
                        "绿队: ",
                        "青色队: ",
                        "大文字烧: ",
                    };
                    playerControlTexts = new string[] {
                        "疑似自杀！",
                        "凶手疑似是",
                        "浅 (L)!",
                        "深 色 (D)!",
                        "凶手的颜色似乎是 ",
                        "凶手似乎戴着帽子!",
                        "凶手似乎不戴着帽子!",
                        "凶手似乎戴着衣服!",
                        "凶手似乎不戴着衣服!",
                        "凶手似乎戴着宠物!",
                        "凶手似乎不戴着宠物!",
                        "凶手似乎戴着面罩!",
                        "凶手似乎不戴着面罩!",
                        "尸体报告时间过久，没有信息！",
                        "时间流逝:"
                    };
                    usablesTexts = new string[] {
                        "在自定义游戏模式中不能发起会议！",
                        "Cheater 不能发起会议！",
                        "Gambler 不能发起会议！",
                        "Sorcerer 不能发起会议！",
                        "有炸弹！你不能发起会议！",
                        "停电了，紧急会议按钮断电了!",
                        "MONJA 已经苏醒，你们这群小笨蛋快逃吧！！！"
                    };
                    buttonsTexts = new string[] {
                        "的灵魂 (",
                        "我的职业 ",
                        "击杀我的凶手是 ",
                        "死后的时间 ",
                        "杀死我的凶手职业 "
                    };
                    helpersTexts = new string[] {
                        " 并招募一个小弟",
                        "杀死所有人"
                    };
                    statusRolesTexts = new string[] {
                        "速度变化！",
                        "你被催眠了！",
                        "目标死亡",
                        "暴走模式",
                        "Seeker 分数: ",
                        "Illusionist 关掉了灯光: ",
                        "地图上有一颗炸弹: ",
                        "受到了惊吓！",
                        "MONJA 醒了: ",
                        "Amnesiac Body Report: 这个身份不能被记住",
                        "Fink 使用了鹰眼！",
                    };
                    statusCaptureTheFlagTexts = new string[] {
                        "你成为了<color=#FF0000FF>红队</color>成员！",
                        "你成为了<color=#0000FFFF>蓝队</color>成员！",
                        "<color=#0000FFFF>蓝队</color>旗帜被夺走了<color=#FF0000FF>",
                        "你们家旗帜被夺走了！",
                        "<color=#FF0000FF>红队</color>旗帜被夺走了<color=#0000FFFF>",
                        "<color=#FF0000FF>红队</color>得分！",
                        "<color=#0000FFFF>蓝队</color>得分！"
                    };
                    statusPoliceAndThiefsTexts = new string[] {
                        "捕获一名小偷！",
                        "一名小偷越狱了！",
                        "偷到一颗宝石！"
                    };
                    statusKingOfTheHillTexts = new string[] {
                        "你成为<color=#00FF00FF>绿队新国王</color>!",
                        "你成为<color=#FFFF00FF>黄队新国王</color>!",
                        "<color=#00FF00FF>绿队</color>国王占领了一块领地！",
                        "<color=#FFFF00FF>黄队</color>国王占领了一块领地！",
                        "你的国王被击杀了！"
                    };
                    statusHotPotatoTexts = new string[] {
                        " 是新的烫手山芋!"
                    };
                    statusZombieLaboratoryTexts = new string[] {
                        "一个关键物品已送达！",
                        "感染了一名幸存者！",
                        "一位幸存者被感染！"
                    };
                    statusBattleRoyaleTexts = new string[] {
                        "一名战士倒下！",
                        "一位<color=#39FF14FF>灰队玩家</color>倒下！",
                        "一位<color=#F2BEFFFF>粉队玩家</color>倒下！",
                        "<color=#808080FF>连环杀手</color>倒下！",
                        "<color=#39FF14FF>灰色阵营</color>的分数！",
                        "<color=#F2BEFFFF>粉色阵营</color>的分数！",
                        "<color=#808080FF>连环杀手</color>的分数！",
                    };
                    statusMonjaFestivalTexts = new string[] {
                        "她从你的篮子里偷东西！",
                        "<color=#FF00FFFF>Allul Monja</color> 緑のチームによって発見された！",
                        "<color=#FF00FFFF>Allul Monja</color> シアンチームが発見！",
                        "<color=#FF00FFFF>Allul Monja</color> 大きなもんじゃが見つけた！",
                    };
                    break;
            }
        }
    }
}