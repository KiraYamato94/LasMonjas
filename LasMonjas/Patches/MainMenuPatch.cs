using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using AmongUs.Data;
using Assets.InnerNet;

namespace LasMonjas.Patches
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public class MainMenuPatch    {
       
        private static AnnouncementPopUp popUp;

        private static void Postfix(MainMenuManager __instance) {
            // Check the music option after loading main menu screen, so when you join the Lobby it starts playing if enabled
            MapOptions.checkMusic();

            var template = GameObject.Find("ExitGameButton");
            var template2 = GameObject.Find("CreditsButton");
            if (template == null || template2 == null) return;
            template.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            template.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.625f, 0.5f);
            template.transform.FindChild("FontPlacer").transform.localScale = new Vector3(1.8f, 0.9f, 0.9f);
            template.transform.FindChild("FontPlacer").transform.localPosition = new Vector3(-0.8f, 0f, 0f);

            template2.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            template2.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.378f, 0.5f);
            template2.transform.FindChild("FontPlacer").transform.localScale = new Vector3(1.8f, 0.9f, 0.9f);
            template2.transform.FindChild("FontPlacer").transform.localPosition = new Vector3(-0.8f, 0f, 0f);

            // LMJ discord button
            var buttonDiscord = UnityEngine.Object.Instantiate(template, template.transform.parent);
            buttonDiscord.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            buttonDiscord.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.542f, 0.5f);

            var textDiscord = buttonDiscord.transform.GetComponentInChildren<TMPro.TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.5f, new System.Action<float>((p) => {
                textDiscord.SetText("LMJ Discord");
            })));
            PassiveButton passiveButtonDiscord = buttonDiscord.GetComponent<PassiveButton>();

            passiveButtonDiscord.OnClick = new Button.ButtonClickedEvent();
            passiveButtonDiscord.OnClick.AddListener((System.Action)(() => Application.OpenURL("https://discord.gg/UPCSqnD4NU")));

            // LMJ credits button
            if (template == null) return;
            var creditsButton = Object.Instantiate(template, template.transform.parent);

            creditsButton.transform.localScale = new Vector3(0.42f, 0.84f, 0.84f);
            creditsButton.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.462f, 0.5f);

            var textCreditsButton = creditsButton.transform.GetComponentInChildren<TMPro.TMP_Text>();
            __instance.StartCoroutine(Effects.Lerp(0.5f, new System.Action<float>((p) => {
                textCreditsButton.SetText("LMJ Credits");
            })));
            PassiveButton passiveCreditsButton = creditsButton.GetComponent<PassiveButton>();

            passiveCreditsButton.OnClick = new Button.ButtonClickedEvent();

            passiveCreditsButton.OnClick.AddListener((System.Action)delegate {

                if (popUp != null) Object.Destroy(popUp);
                var popUpTemplate = Object.FindObjectOfType<AnnouncementPopUp>(true);
                if (popUpTemplate == null) {
                    LasMonjasPlugin.Logger.LogError("couldnt show credits, popUp is null");
                    return;
                }
                popUp = Object.Instantiate(popUpTemplate);

                popUp.gameObject.SetActive(true);
                string creditsString = @$"<align=""center""><b>Beta Testers:</b>
Seira       Sensei      Belen       Colacao     Muaresito      
Xago        Palex       Piruneko    Olmaito     K1lucus
Nigure      Lumiro      Kirrody     Eur mom      GD

Thanks to Old Lady, Jesushi, Eur mom, GD and Lotty for our discord moderation!

Thanks to miniduikboot & GD for hosting modded servers!

";
                creditsString += $@"<size=60%> Credits and Code snips:
Reactor - The main framework used
BepInEx - Used to hook game functions
KevinMacLeod - For the music used
Makai Symphony - For the music used
Enterbrain Inc. - For some of the sound effects used
Essentials - Custom game options by TheOtherRoles team
tomozbot - Original idea for the Mayor (Captain) and Lighter
NotHunter101 - Original idea for the Medic (Squire and Forensic) and Engineer (Mechanic)
Woodi-dev - Original idea for the Sheriff, Lovers mod and the Jailer mod reference
Hardel-DW - Original idea came for the Investigator (Detective) and Time Master (Time Traveler)
TheOtherRoles - Original idea for Medium (Forensic), Time Master (Time Traveler), Seer (Fortune Teller), Hacker, Tracker (Sleuth), Mini (Kid), Security Guard (Welder and Vigilant), Bait (The Chosen One), Pursuer (Jinx), Vampire (Demon), Trickster (Illusionist), Guesser (Gambler), Witch (Sorcerer) and Paintball (Bloody). Also used Submerged's update button.
Town-Of-Us - Original idea for Swapper (Cheater), Altruist (Spiritualist), Flash, Giant (Big Chungus), Shifter (Role Thief), Arsonist (Pyromaniac), Undertaker (Janitor), Plumber (Miner), Librarian (Blackmailer), Exiler (Executioner) and Amnesiac
ottomated_ - Original idea for Morphling (Mimic), Camouflager (Painter) and Snitch (Fink)
Wunax - Original idea for Chameleon
dhalucard - Original idea for Jackal and Sidekick (Renegade and Minion)
Lunastellia - Original idea for Hunter
Maartii - Original idea for Jester (Joker)
Cheep - Original idea for Capture the Flag mod
Allul - Coded the gamemodes, custom map, Bomberman, Medusa, Hypnotist, Bounty Hunter, Trapper, Yinyanger, Challenger, Ninja, Berserker, Yandere, Stranded, Monja, Treasure Hunter, Devourer, Puppeteer, Seeker, Coward, Jailer, Performer, Blind, Electrician, and some more that weren't original ideas
Sensei - Made button sprites, custom map sprite and some hats
xxomega77 - Adapted Custom Hats System
AlexejheroYTB - Made Submerged compatible with Las Monjas
Eisbison - Original idea for Night Vision cameras
Pandraghon - Original idea for the Better Reactor, Comms and Oxygen sabotage
eDonnes124 - Original idea for Drunk (Pro)
Dolly1016 - Original idea for Sniper (Archer), Empiric (Poisoner), Alien (Bat), Necromancer and Trapper (Engineer)
Goose Goose Duck - Idea for Devourer (Pelican ability)
Town of Host - Idea for Welder (vent bomb) and Manipulator
Herysia - Original idea for the try hard vents
Twix - Downloader tool used to make Las Monjas Downloader
四个憨批汉化组 - Partial Chinese translation.
xiaojinna, Dawn66642, MC-AS-Huier - Partial Chinese translation.</size>";
                creditsString += "</align>";

                Assets.InnerNet.Announcement creditsAnnouncement = new() {
                    Id = "lmjrCredits",
                    Language = 0,
                    Number = 500,
                    Title = "Las Monjas\nSpecial Thanks",
                    ShortTitle = "LMJ Credits",
                    SubTitle = "",
                    PinState = false,
                    Date = "14.02.2022",
                    Text = creditsString,
                };
                __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => {
                    if (p == 1) {
                        var backup = DataManager.Player.Announcements.allAnnouncements;
                        DataManager.Player.Announcements.allAnnouncements = new();
                        popUp.Init(false);
                        DataManager.Player.Announcements.SetAnnouncements(new Announcement[] { creditsAnnouncement });
                        popUp.CreateAnnouncementList();
                        popUp.UpdateAnnouncementText(creditsAnnouncement.Number);
                        popUp.visibleAnnouncements[0].PassiveButton.OnClick.RemoveAllListeners();
                        DataManager.Player.Announcements.allAnnouncements = backup;
                    }
                })));
            });            
        }
    }
}