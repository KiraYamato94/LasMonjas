using System;
using System.Security.Cryptography;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using UnityEngine;
using System.Linq;
using Il2CppInterop;
using System.Collections.Generic;
using static LasMonjas.LasMonjas;
using TMPro;
using static UnityEngine.ParticleSystem.PlaybackState;
using Epic.OnlineServices;

namespace LasMonjas.Core
{


    [HarmonyPatch]
    public static class ChatCommands
    {
        public static bool isLover(this PlayerControl player) => !(player == null) && (player == Modifiers.lover1 || player == Modifiers.lover2);

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class EnableChat
        {
            public static void Postfix(HudManager __instance) {
                if (!__instance.Chat.isActiveAndEnabled && PlayerControl.LocalPlayer.isLover())
                    __instance.Chat.SetVisible(true);
            }
        }

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        public static class AddChat
        {
            public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer) {
                if (__instance != DestroyableSingleton<HudManager>.Instance.Chat)
                    return true;
                PlayerControl localPlayer = PlayerControl.LocalPlayer;
                return localPlayer == null || (MeetingHud.Instance != null || LobbyBehaviour.Instance != null || (localPlayer.Data.IsDead || localPlayer.isLover() || (int)sourcePlayer.PlayerId == (int)PlayerControl.LocalPlayer.PlayerId));

            }
        }

        [HarmonyPatch]
        public static class ChatCommandsInfo
        {
            [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
            private static class SendChatPatch
            {
                private static void ReloadLanguage(int language) {
                    switch (language) {
                        // English
                        case 1:
                            LasMonjasPlugin.modLanguage.Value = 1;
                            break;
                        // Spanish
                        case 2:
                            LasMonjasPlugin.modLanguage.Value = 2;
                            break;
                        // Japanese
                        case 3:
                            LasMonjasPlugin.modLanguage.Value = 3;
                            break;
                        // Chinese
                        case 4:
                            LasMonjasPlugin.modLanguage.Value = 4;
                            break;
                    }
                    Language.LoadLanguage();
                }

                static bool Prefix(ChatController __instance) {
                    string text = __instance.TextArea.text;
                    string subText = text.Split().Last();
                    string infoText = "";
                    bool handled = false;
                    if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) {
                        if (text.ToLower().StartsWith("/language ") || text.ToLower().StartsWith("/l ")) {
                            switch (subText.ToLower()) {
                                // Impostor roles
                                case "english":
                                    ReloadLanguage(1);
                                    infoText = "Las Monjas: language changed to English";
                                    break;
                                case "spanish":
                                    infoText = "Las Monjas: idioma cambiado a Español";
                                    ReloadLanguage(2);
                                    break;
                                case "japanese":
                                    infoText = "Las Monjas: 言語を日本語に変更";
                                    ReloadLanguage(3);
                                    break;
                                case "chinese":
                                    infoText = "Las Monjas: 语言改为中文";
                                    ReloadLanguage(4);
                                    break;
                                default:
                                    infoText = "Las Monjas: language not supported";
                                    break;
                            }
                            handled = true;
                            __instance.AddChat(PlayerControl.LocalPlayer, infoText);
                            //PlayerControl.LocalPlayer.RpcSendChat(infoText);
                        }

                        if (text.ToLower().StartsWith("/help ") || text.ToLower().StartsWith("/h ")) {
                            switch (LasMonjasPlugin.modLanguage.Value) {
                                // English
                                case 1:
                                    switch (subText.ToLower()) {
                                        // Impostor roles
                                        case "mimic":
                                            infoText = "Mimic: impostor who can mimic the appearance of other player.";
                                            break;
                                        case "painter":
                                            infoText = "Painter: impostor who can hide every player's outfit and paint them with a random color.";
                                            break;
                                        case "demon":
                                            infoText = "Demon: impostor who can bite a player to delay his death." +
                                                "\nIf there's a Demon, all players will have a Nun button to place one Nun per game on the map." +
                                                "\nStaying next to the Nun nullifies the bite.";
                                            break;
                                        case "janitor":
                                            infoText = "Janitor: impostor who can remove and move bodies." +
                                                "\nHe can't remove and move bodies at the same time and can't use vents while carrying a body.";
                                            break;
                                        case "illusionist":
                                            infoText = "Illusionist: impostor who can make his own vent network and turn off the lights from anywhere." +
                                                "\nHis 3-vents network can only be used by himself and becomes visible for everyone right after placing the third vent." +
                                                "\nOnce the vent network is done, he gains the ability to turn off the lights.";
                                            break;
                                        case "manipulator":
                                            infoText = "Manipulator: impostor who can manipulate a player to kill his adjacent from anywhere." +
                                                "\nCan kill anyone, himself included, with his ability.";
                                            break;
                                        case "bomberman":
                                            infoText = "Bomberman: impostor who can place a bomb on the map." +
                                                "\nThe bomb can't be placed if a sabotage is active or a player is too close to him." +
                                                "\nPlayers can defuse the bomb by touching it, impostors win if the bomb isn't defused.";
                                            break;
                                        case "chameleon":
                                            infoText = "Chameleon: impostor who can become invisible." +
                                                "\nHe can't use vents. While invisible, he can be killed by roles who have a kill button.";
                                            break;
                                        case "gambler":
                                            infoText = "Gambler: impostor who can shoot a player choosing their role during the meeting." +
                                                "\nHe has to guess the player's role to kill him, only the current ingame roles appear on his screen. Choosing the wrong one kills himself.";
                                            break;
                                        case "sorcerer":
                                            infoText = "Sorcerer: impostor who can cast spells on players." +
                                                "\nSpelled players will have a purple pumpkin icon next to their names during the meeting and will die afterwards unless the Sorcerer is voted out.";
                                            break;
                                        case "medusa":
                                            infoText = "Medusa: impostor who can petrify other players." +
                                                "\nA petrified player can't move for a set amount of time.";
                                            break;
                                        case "hypnotist":
                                            infoText = "Hypnotist: impostor who can place traps which inverts player's movement controls." +
                                                "\nTraps become active after a meeting and don't affect impostors.";
                                            break;
                                        case "archer":
                                            infoText = "Archer: impostor who can make long distance kills but can't make normal ones." +
                                                "\nHe needs to pick the bow (invisible to other players), aim with the mouse and right click to shoot." +
                                                "\nA warning image appears on his position if he misses the shoot or above the player's body if he kills someone.";
                                            break;
                                        case "plumber":
                                            infoText = "Plumber: impostor who can create usable vents for any vent role." +
                                                "\nVents become available after a meeting only when all extra vents had been placed.";
                                            break;
                                        case "librarian":
                                            infoText = "Librarian: impostor who can prevent a player from talking on a meeting." +
                                                "\nEveryone knows who is silenced during the meeting.";
                                            break;


                                        // Rebel roles
                                        case "renegade":
                                            infoText = "Renegade: rebel who has to kill everyone." +
                                                "\nHe can recruit a Minion to help him killing, both have impostor vision, can vent and their names will be green.";
                                            break;
                                        case "bountyhunter":
                                            infoText = "Bounty Hunter: rebel who has to kill a specific player." +
                                                "\nHis target button assigns a random player's role to be his target but if the target is already dead, he also dies." +
                                                "\nIf the target gets exiled, Bounty hunter also does, and if the target disconnects, Bounty Hunter dies.";
                                            break;
                                        case "trapper":
                                            infoText = "Trapper: rebel who has to kill everyone with mines." +
                                                "\nHe can put traps that root the player who touched it and mines that kill whoever steps on it.";
                                            break;
                                        case "yinyanger":
                                            infoText = "Yinyanger: rebel who has to kill everyone marking two players each time." +
                                                "\nHe can mark a player with the Yin and another one with the Yang, if they collide both die ignoring any shields they could have." +
                                                "\nAfter marking one player, he can't mark the other one if the marked one is too close to the target.";
                                            break;
                                        case "challenger":
                                            infoText = "Challenger: rebel who has to kill everyone with rock-paper-scissors duels." +
                                                "\nSelecting a player will teleport all players to the duel arena after 10 seconds if no sabotage is active." +
                                                "\nIf one of them doesn't select an attack, the other wins automatically. If no one selects an attack both die. Nobody dies on draw.";
                                            break;
                                        case "ninja":
                                            infoText = "Ninja: rebel who has to kill everyone making double kills." +
                                                "\nSelect a player to teleport to his position afterwards killing him in the process. He can use the normal kill button right after to make a double kill.";
                                            break;
                                        case "berserker":
                                            infoText = "Berserker: rebel who has to kill everyone but can't stop killing." +
                                                "\nAfter killing for first time, his kill button gets a permanent 10 second cooldown but he dies if he doesn't kill for a set amount of time.";
                                            break;
                                        case "yandere":
                                            infoText = "Yandere: rebel who has to stalk a target a few times and then kill it to win." +
                                                "\nIf the target gets exiled or killed by another player, she enters rampage mode and has to kill everyone to win instead.";
                                            break;
                                        case "stranded":
                                            infoText = "Stranded: rebel who has to find ammo in boxes around the map and kill 3 players." +
                                                "\nHe can also find an item to become invisible for a while and the vent ability.";
                                            break;
                                        case "monja":
                                            infoText = "Monja: rebel who has to find little monjas and bring them to the ritual spot." +
                                                "\nOnce all the monjas are delivered, she can transform into Monja to kill everyone within 60 seconds, otherwise she'll die." +
                                                "\nIf there's a Kid ingame, the Monja must kill everyone except the Kid to win. Also during the transformation players can only do tasks or run away.";
                                            break;

                                        // Neutral roles
                                        case "joker":
                                            infoText = "Joker: neutral who has to be voted out to win." +
                                                "\nHe can sabotage but only if he's alive.";
                                            break;
                                        case "rolethief":
                                            infoText = "Role Thief: neutral with no win condition." +
                                                "\nHe can steal the role of other players but if he tries to steal an impostor or rebel role, he dies.";
                                            break;
                                        case "pyromaniac":
                                            infoText = "Pyromaniac: neutral who has to ignite everyone to win." +
                                                "\nHe can spray players by standing next to them, once he sprays everyone he wins.";
                                            break;
                                        case "treasurehunter":
                                            infoText = "Treasure Hunter: neutral who has to look for treasures to win." +
                                                "\nHis button spawns one treasure randomly on the map and can use it again after finding the first treasure, after getting the needed amount he wins.";
                                            break;
                                        case "devourer":
                                            infoText = "Devourer: neutral who has to eat bodies to win." +
                                                "\nHe hears a sound when a player dies.";
                                            break;
                                        case "poisoner":
                                            infoText = "Poisoner: neutral who has to poison everyone to win." +
                                                "\nHe selects a player to be the poisoned one, players standing next to the poisoned increase their poison meter." +
                                                "\nA player who reached 100% meter counts towards poisoning other players. Once every player reaches 100% poison meter, he wins.";
                                            break;
                                        case "puppeteer":
                                            infoText = "Puppeteer: neutral who can morph into other players and has to get killed while morphed a few times to win." +
                                                "\nHe can pick a sample from a player and morph into it all the time he wants or until a meeting is called, he gets killed or he decides to uncover himself." +
                                                "\nIf he gets killed while morphed, he gains one point and revives on the spot where he started the morph, he wins after reaching the needed points.";
                                            break;
                                        case "exiler":
                                            infoText = "Exiler: neutral who has to vote out a specific player to win." +
                                                "\nHis target button assigns a random player to be his target but if the target is already dead, he also dies. But if the target disconnects, he wins.";
                                            break;
                                        case "amnesiac":
                                            infoText = "Amnesiac: neutral who remembers his role reporting a body." +
                                                "\nThe role resets after remembers it.";
                                            break;
                                        case "seeker":
                                            infoText = "Seeker: neutral who has to find people on hide and seek minigame." +
                                                "\nUpon selecting from 1 to 3 people, he can start the minigame teleporting all players to a new zone." +
                                                "\nHe gains 1 point per player found on the minigame, after reaching the needed points he wins.";
                                            break;

                                        // Crewmate roles
                                        case "captain":
                                            infoText = "Captain: crewmate whose vote counts double." +
                                                "\nHe can redirect all the votes to another player one time per game, but if he redirects them to a crewmate he gets exiled." +
                                                "\nBeing alone with an impostor or rebel allows him to call an emergency meeting to exile the remaining player.";
                                            break;
                                        case "mechanic":
                                            infoText = "Mechanic: crewmate who can fix sabotages a certain number of times from anywhere." +
                                                "\nHe also fixes the whole sabotage by doing only one of the emergency tasks without the needing of a second player.";
                                            break;
                                        case "sheriff":
                                            infoText = "Sheriff: crewmate who can kill players." +
                                                "\nHe dies if he tries to kill a crewmate.";
                                            break;
                                        case "detective":
                                            infoText = "Detective: crewmate who can see player's footprints." +
                                                "\nFootprints don't spawn close to vents and only spawn if Detective is alive.";
                                            break;
                                        case "forensic":
                                            infoText = "Forensic: crewmate who gets clues by reporting bodies and asking player's ghosts." +
                                                "\nThe clues that he can get by reporting a body are the killer's name, color type or something about his appearance." +
                                                "\nGhosts appear on the next round where a body was and their clues are which role killed that player, killer's color type or how much time has passed before reporting the body.";
                                            break;
                                        case "timetraveler":
                                            infoText = "Time Traveler: crewmate who can rewind the time two times per game, reviving players that were killed during the rewind." +
                                                "\nHe can rewind the time if there's no active sabotage, the time rewinds also if someone tries to kill him while he's shielded by time shield." +
                                                "\nThis role can't use Vitals.";
                                            break;
                                        case "squire":
                                            infoText = "Squire: crewmate who can put a shield on a player." +
                                                "\nThe shield last until he gets exiled or killed. Trying to kill the shielded player will trigger a sound heard by Impostors, Rebels, Sheriff, Squire and the shielded player.";
                                            break;
                                        case "cheater":
                                            infoText = "Cheater: crewmate who can swap the votes of two players." +
                                                "\nAfter swapping votes, he gets exiled if one of those two players turned out to be a crewmate.";
                                            break;
                                        case "fortuneteller":
                                            infoText = "Fortune Teller: crewmate who can reveal if a player is good or bad." +
                                                "\nRevealing triggers a sound and turns the screen blue for both players." +
                                                "\nThe name turns red for bad roles and cyan for good ones.";
                                            break;
                                        case "hacker":
                                            infoText = "Hacker: crewmate who can use Admin and Vitals from anywhere and gets more information from them." +
                                                "\nWhile his hack ability is active, he can see player colors on Admin and how much time has passed since someone died on Vitals.";
                                            break;
                                        case "sleuth":
                                            infoText = "Sleuth: crewmate who can track bodies and one player's position." +
                                                "\nHe sees a blue arrow pointing to the tracked player and green ones pointing to the bodies.";
                                            break;
                                        case "fink":
                                            infoText = "Fink: crewmate who reveals who the impostors are after finishing his tasks and can zoom out the camera." +
                                                "\nHe can't move while the camera is zoomed out." +
                                                "\nImpostors also know who the Fink is when a few tasks remain or when he's zooming out the camera.";
                                            break;
                                        case "kid":
                                            infoText = "Kid: crewmate who shouldn't be killed or exiled, otherwise everyone loses." +
                                                "\nHe's smaller than the other players.";
                                            break;
                                        case "welder":
                                            infoText = "Welder: crewmate who can disable vents." +
                                                "\nThose vents become unavailable after the next meeting and can't be entered or exited, but still can be used as a tunnel.";
                                            break;
                                        case "spiritualist":
                                            infoText = "Spiritualist: crewmate who can revive another player at the cost of his own life." +
                                                "\nHe needs to stay next to a body to revive it, but if someone calls a meeting while he tries to revive, he dies instead." +
                                                "\nImpostors and Rebels get a pink arrow pointing the revived player.";
                                            break;
                                        case "vigilant":
                                            infoText = "Vigilant: crewmate who can place four extra cameras on the map." +
                                                "\nThe cameras become available after a meeting and when he places all the cameras he gets the ability to remote check cameras." +
                                                "\nOn MiraHQ he can remote check doorlog instead.";
                                            break;
                                        case "hunter":
                                            infoText = "Hunter: crewmate who can mark another player who will die if he gets killed." +
                                                "\nExiling him won't exile the marked player.";
                                            break;
                                        case "jinx":
                                            infoText = "Jinx: crewmate who can block other player's buttons." +
                                                "\nUsing a button while being jinxed makes that button enter cooldown.";
                                            break;
                                        case "coward":
                                            infoText = "Coward: crewmate who can call meetings from anywhere." +
                                                "\nHe can't call meetings if a sabotage is active.";
                                            break;
                                        case "bat":
                                            infoText = "Bat: crewmate who can emit a frequency that alters button cooldown." +
                                                "\nCrewmates, Rebels and Neutrals button cooldown goes 2x faster." +
                                                "\nImpostors button cooldown increase by 1 each second.";
                                            break;
                                        case "necromancer":
                                            infoText = "Necromancer: crewmate who can drag and drop bodies and revive them by dragging them to a specific room." +
                                                "\nThe specific room is pointed to by a blue arrow." +
                                                "\nImpostors and Rebels get a green arrow pointing to the revived player.";
                                            break;
                                        case "engineer":
                                            infoText = "Engineer: crewmate who can place increase or decrease speed and position traps." +
                                                "\nHe can switch trap type with the F key." +
                                                "\nTraps become active after a meeting and have a 5 seconds effect duration.";
                                            break;
                                        case "shy":
                                            infoText = "Shy: crewmate who can reveal the position of the closest player." +
                                                "\nAn arrow reveals the direction of the closest player.";
                                            break;
                                        case "taskmaster":
                                            infoText = "Task Master: crewmate who has extra tasks after doing the initial ones." +
                                                "\nCompleting the extra tasks before getting killed achieves a crewmate win.";
                                            break;
                                        case "jailer":
                                            infoText = "Jailer: crewmate who can mark a player to be his assistant." +
                                                "\nTrying to kill the assistant denies the kill and teleports the killer to the jail for a few seconds, this only works one time and the Jailer needs to mark another player after this.";
                                            break;

                                        // Modifiers:
                                        case "lovers":
                                            infoText = "Lovers: modifier who links two players." +
                                                "\nBoth die if one get killed or exiled.";
                                            break;
                                        case "lighter":
                                            infoText = "Lighter: modifier who gives more vision to a player." +
                                                "\nAlso it makes that player immune to night vision.";
                                            break;
                                        case "blind":
                                            infoText = "Blind: modifier who reduces a player's vision." +
                                                "\nIt doesn't affect impostors.";
                                            break;
                                        case "flash":
                                            infoText = "Flash: modifier who increases a player's speed." +
                                                "\nDoesn't affect during Challenger's duel and anonymous comms.";
                                            break;
                                        case "bigchungus":
                                            infoText = "Big Chungus: modifier who increases a player's size and reduces his speed." +
                                                "\nDoesn't affect during Challenger's duel, Seeker's minigame and anonymous comms.";
                                            break;
                                        case "thechosenone":
                                            infoText = "The Chosen One: modifier who forces his killer to report his body." +
                                                "\nA report delay can be configured.";
                                            break;
                                        case "performer":
                                            infoText = "Performer: modifier whose death triggers music and an arrow reveals his position." +
                                                "\nMusic's duration can be configured.";
                                            break;
                                        case "pro":
                                            infoText = "Pro: modifier who inverts a player's movement controls." +
                                                "\nIt also affects the player in ghost form.";
                                            break;
                                        case "paintball":
                                            infoText = "Paintball: modifier who splashes his killer on death, the killer leaves a trail with the player's color for a few seconds.";
                                            break;
                                        case "electrician":
                                            infoText = "Electrician: modifier who paralyzes his killer for a few seconds.";
                                            break;

                                        // Gamemodes:
                                        case "capturetheflag":
                                        case "ctf":
                                            infoText = "Capture The Flag: gamemode between red and blue teams where each team has to steal the other's team flag and take it to their base." +
                                                "\nEveryone can vent and kill but you can't while having the flag." +
                                                "\nOn odd player number games a special role called Flag Stealer will appear, if he kills a player with the flag, he switches teams with him.";
                                            break;
                                        case "policeandthieves":
                                        case "pat":
                                            infoText = "Police and Thieves: gamemode between cyan and brown teams where Police team has to capture all the Thieves while Thief team has to steal all the jewels on the map without being captured." +
                                                "\nThis gamemode adds a new map room, the Prison, a place where captured Thieves get teleported, to release them another Thief has to press the button outside the Prison." +
                                                "\nThe player assignment is max 9 Thieves and 6 Police, where 2 will be Tasers who can paralyze Thieves for a few seconds." +
                                                "\nThieves can vent but can't while having a jewel.";
                                            break;
                                        case "kingofthehill":
                                        case "koth":
                                            infoText = "King of The Hill: gamemode between green and yellow team. There are 3 capturable zones in the map and each team has a King who can capture them." +
                                                "\nEach captured zone gives 1 point per second to the team who has it. Everyone can vent and kill except the Kings." +
                                                "\nOn odd player number games a special role called Usurper will appear, if he kills a king, he swiches teams with him.";
                                            break;
                                        case "hotpotato":
                                        case "hp":
                                            infoText = "Hot Potato: gamemode where a random player gets the Hot Potato role and has to give the hot potato to another player before the time runs out, otherwise the hot potato player will die." +
                                                "\nThere can only be 1 Hot Potato on the game, the rest will be Cold Potatoes or Burnt Potatoes if they die." +
                                                "\nThis gamemode also works as Hide and Seek.";
                                            break;
                                        case "zombielaboratory":
                                        case "zl":
                                            infoText = "Zombie Laboratory: gamemode where a random player gets the Zombie role, another player the Nurse role and the rest will be Survivors." +
                                                "\nThe Zombie must infect everyone to win, while the Survivors have to look for the key items hidden in boxes around the map to deliver them to the new map zone, the Infirmary, where the Nurse can make the cure and win the game." +
                                                "\nOnly Zombies can vent, the Nurse can't be infected and can pick a medkit to heal an infected play.";
                                            break;
                                        case "battleroyale":
                                        case "br":
                                            infoText = "Battle Royale: gamemode where everyone has a ranged kill button with the Archer's mechanic (usable with right mouse click) and a very low cooldown." +
                                                "\nNobody can vent and players have lives and when they reach 0 lives they die for the rest of the match, the last one alive wins." +
                                                "\nAlso there are 3 ways of playing this gamemode, All VS All, Team Battle or Score Battle." +
                                                "\nOn odd player number games for Team and Score Battle there will be a powerful neutral role called Serial Killer that can also win killing everyone or reaching the needed score. This role has 3x lives and half kill cooldown.";
                                            break;
                                    }
                                    break;
                                // Spanish
                                case 2:
                                    switch (subText.ToLower()) {
                                        // Impostor roles
                                        case "mimic":
                                            infoText = "Mimic: impostor que puede copiar el aspecto de otro jugador durante un tiempo determinado.";
                                            break;
                                        case "painter":
                                            infoText = "Painter: impostor que hace que todos los jugadores se vean del mismo color sin cosmeticos.";
                                            break;
                                        case "demon":
                                            infoText = "Demon: impostor que muerde jugadores para demorar su muerte." +
                                                "\nCuando hay un Demon, todos los jugadores tienen un boton para poner una Nun en el mapa que dura toda la partida." +
                                                "\nEl area de la Nun te protege de la mordedura.";
                                            break;
                                        case "janitor":
                                            infoText = "Janitor: impostor que puede retirar y mover cuerpos." +
                                                "\nNo puede usar ambas habilidades al mismo tiempo ni usar rejillas si esta moviendo un cuerpo.";
                                            break;
                                        case "illusionist":
                                            infoText = "Illusionist: impostor que puede crear su propia red de tres rejillas y apagar las luces a distancia." +
                                                "\nEsa red de rejilla solo puede usarla el y se vuelve visible para todos al poner la tercera." +
                                                "\nAl terminar la red de rejillas obtiene la habilidad de apagar las luces.";
                                            break;
                                        case "manipulator":
                                            infoText = "Manipulator: impostor que puede matar al jugador situado al lado del manipulado." +
                                                "\nPuede matar a cualquiera, incluido a si mismo como a otros impostores con su habilidad.";
                                            break;
                                        case "bomberman":
                                            infoText = "Bomberman: impostor que puede poner una bomba en el mapa." +
                                                "\nNo puede ponerla si hay un sabotaje activo o esta muy cerca de un jugador." +
                                                "\nLos jugadores pueden desactivar la bomba tocandola, si no lo consiguen ganan los Impostores.";
                                            break;
                                        case "chameleon":
                                            infoText = "Chameleon: impostor que puede volverse invisible." +
                                                "\nNo puede usar rejillas y pueden matarlo mientras esta en invisible.";
                                            break;
                                        case "gambler":
                                            infoText = "Gambler: impostor que puede disparar a jugadores en la reunion si adivina su rol." +
                                                "\nSolo aparecen los roles presentes en esa partida para seleccionar, si se equivoca muere el mismo.";
                                            break;
                                        case "sorcerer":
                                            infoText = "Sorcerer: impostor que puede hechizar jugadores." +
                                                "\nLos jugadores hechizados tendran una calabaza purpura al lado de su nombre en la reunion y seran expulsados de la nave si en esa ronda no se expulsa al Sorcerer.";
                                            break;
                                        case "medusa":
                                            infoText = "Medusa: impostor que puede petrificar jugadores." +
                                                "\nEl jugador petrificado no podra hacer nada durante el tiempo de petrificacion.";
                                            break;
                                        case "hypnotist":
                                            infoText = "Hypnotist: impostor que puede poner trampas que invierten los controles de movimiento de los jugadors." +
                                                "\nLas trampas duran toda la partida, se vuelven visibles y activas tras una reunion y no afectan a otros Impostores.";
                                            break;
                                        case "archer":
                                            infoText = "Archer: impostor que puede matar a distancia y a traves de las paredes, pero no puede matar de forma normal." +
                                                "\nTiene que equiparse el arco (visible solo para el), apuntar con el raton y hacer clic derecho para disparar." +
                                                "\nUn aviso aparecera en la posicion del Archer si falla el tiro, de lo contrario el aviso aparecera encima del cuerpo del jugador que ha muerto.";
                                            break;
                                        case "plumber":
                                            infoText = "Plumber: impostor que puede crear rejillas adicionales utilizables por cualquier rol que pueda usar rejillas." +
                                                "\nLas rejillas se vuelven visibles y utilizables tras una reunion una vez el maximo de rejillas haya sido alcanzado.";
                                            break;
                                        case "librarian":
                                            infoText = "Librarian: impostor que puede silenciar a un jugador para evitar que hable durante la reunion." +
                                                "\nTodos saben quien esta silenciado durante la reunion.";
                                            break;


                                        // Rebel roles
                                        case "renegade":
                                            infoText = "Renegade: rebelde que tiene que matar a todos para ganar." +
                                                "\nPuede reclutar un Minion para que le ayude, ambos tiene vision de impostor y pueden usar rejillas.";
                                            break;
                                        case "bountyhunter":
                                            infoText = "Bounty Hunter: rebelde que tiene que matar a un jugador especifico para ganar." +
                                                "\nEse jugador estara definido por su rol, pero si ya esta muerto entonces el Bounty Hunter." +
                                                "\nSi el objetivo es expulsado, el Bounty Hunter tambien, si el objetivo se desconecta el Bounty Hunter muere.";
                                            break;
                                        case "trapper":
                                            infoText = "Trapper: rebelde que tiene que matar a todos poniendo trampas." +
                                                "\nPuede poner minas que mataran a cualquier que las pise y cepos que inmovilizaran a quien lo pise.";
                                            break;
                                        case "yinyanger":
                                            infoText = "Yinyanger: rebelde que tiene que matar a todos marcando a dos jugadores de cada vez." +
                                                "\nSi esos jugadores chocan entre ellos, mueren ignorando cualquier tipo de escudo que pudieran tener." +
                                                "\nSi uno de esos jugadores muere por otro jugador, podra selecionar otro objetivo en la misma ronda." +
                                                "\nNo puede seleccionar jugadores que esten cerca del ya marcado.";
                                            break;
                                        case "challenger":
                                            infoText = "Challenger: rebelde que tiene que matar a todos mediante duelos de piedra, papel y tijeras." +
                                                "\nTras seleccionar un jugador, seran teletransportados a la arena del duelo pasados 10 segundos si no hay un sabotaje activo o el Challenger o su objetivo murieron en ese periodo." +
                                                "\nSi uno de ellos no escoge ataque, el otro gana automaticamente. Si nadie escoge ataque mueren ambos, pero nadie muere en caso de empate.";
                                            break;
                                        case "ninja":
                                            infoText = "Ninja: rebelde que tiene que matar a todos haciendo muertes dobles." +
                                                "\nPuede seleccionar a un jugador para posteriormente teletranportarse a el matandolo en el acto, con la posibilidad de usar su boton de matar normal acto seguido para realizar una muerte doble.";
                                            break;
                                        case "berserker":
                                            infoText = "Berserker: rebelde que tiene que matar a todos pero se muere si no mata durante cierto tiempo." +
                                                "\nUna vez mata por primera vez, su boton de matar obtiene un tiempo de recarga de 10 segundos pero el contador de tiempo limite empieza a bajar, si no mata a nadie antes de que ese contador llegue a 0, se muere el.";
                                            break;
                                        case "yandere":
                                            infoText = "Yandere: rebelde que tiene que acechar varias veces a un jugador determinado y luego matarlo para ganar." +
                                                "\nSi ese jugador es expulsado o lo matan, la Yandere entra en modo furia, donde tendra que matar a todos para ganar.";
                                            break;
                                        case "stranded":
                                            infoText = "Stranded: rebelde que tiene que encontrar municion en las cajas y matar a tres jugadores." +
                                                "\nPuede encontrar la habilidad de usar rejillas y de volverse invisible una vez por partida.";
                                            break;
                                        case "monja":
                                            infoText = "Monja: rebelde que tiene que reunir los objetos para realizar el ritual y luego matar a todos." +
                                                "\nCuando reune todos los objetos, se puede transformar en la Monja para matar a todos antes de 60 segundos, sino se muere." +
                                                "\nNo puede matar al niño y durante la transformacion los jugadores solo pueden hacer tareas y huir.";
                                            break;

                                        // Neutral roles
                                        case "joker":
                                            infoText = "Joker: neutral que tiene que ser expulsado de la nave para ganar." +
                                                "\nPuede sabotear pero solo mientras esta vivo.";
                                            break;
                                        case "rolethief":
                                            infoText = "Role Thief: neutral que no tiene condicion de victoria." +
                                                "\nPuede robar el rol de otro jugador pero si intenta robarlo a un Impostor o Rebelde se muere.";
                                            break;
                                        case "pyromaniac":
                                            infoText = "Pyromaniac: neutral que tiene que quemar a todos para ganar." +
                                                "\nPuede rociar a los jugadores poniendose a su lado, cuando rocia a todos gana.";
                                            break;
                                        case "treasurehunter":
                                            infoText = "Treasure Hunter: neutral que tiene que encontrar tesoros para ganar." +
                                                "\nSu boton invoca un cofre en un sitio aleatorio del mapa, puede invocar otro tras encontrarlo.";
                                            break;
                                        case "devourer":
                                            infoText = "Devourer: neutral que tiene que devorar cuerpos para ganar." +
                                                "\nCuando muere alguien escucha un sonido.";
                                            break;
                                        case "poisoner":
                                            infoText = "Poisoner: neutral que tiene que envenenar a todos para ganar." +
                                                "\nSelecciona a un jugador para ser el envenenado y los jugadores cercanos a el se iran envenenando poco a poco." +
                                                "\nAl llegar a 100% de veneno, ese jugador contara para envenenar a los demas y una vez todos esten al 100% ganara.";
                                            break;
                                        case "puppeteer":
                                            infoText = "Puppeteer: neutral que puede transformarse en otro jugador a modo de marioneta y deben matarlo varias veces para ganar." +
                                                "\nLa transformacion dura lo que quiera el jugador hasta que se haga una reunion o lo maten." +
                                                "\nSi lo matan transformado, gana un punto y vuelve al lugar donde se transformo inicialmente.";
                                            break;
                                        case "exiler":
                                            infoText = "Exiler: neutral que tiene que expulsar de la nave a un jugador especifico." +
                                                "\nSi su objetivo muere o ya estaba muerto al seleccionarlo, tambien muere, pero si su objetivo se desconecta entonces gana automaticamente.";
                                            break;
                                        case "amnesiac":
                                            infoText = "Amnesiac: neutral que puede obtener un rol reportando un cuerpo." +
                                                "\nEse rol se reinicia al obtenerlo, en caso de ser un rol no obtenible saldra un aviso en el chat solo para el.";
                                            break;
                                        case "seeker":
                                            infoText = "Seeker: neutral que tiene que ganar puntos jugando al escondite." +
                                                "\nTras seleccionar entre uno a tres jugadores, puede empezar el minijuego que teletransportara a todos los jugadores a una nueva zona." +
                                                "\nGana un punto por jugador encontrado en el minijuego.";
                                            break;

                                        // Crewmate roles
                                        case "captain":
                                            infoText = "Captain: tripulante cuyo voto cuenta doble." +
                                                "\nPuede forzar la votacion una vez por partida para que todos los votos vayan a un jugador concreto, pero si ese jugador es inocente, entonces el Captain tambien es expulsado." +
                                                "\nSi se queda solo contra un Impostor o Rebelde, puede convocar una reunion a distancia cuando quiera para echar al jugador restante.";
                                            break;
                                        case "mechanic":
                                            infoText = "Mechanic: tripulante que puede reparar sabotajes a distancia." +
                                                "\nPuede reparar los sabotajes dobles el solo.";
                                            break;
                                        case "sheriff":
                                            infoText = "Sheriff: tripulante que puede matar a otros jugadores." +
                                                "\nSi intenta matar a un tripulante se muere.";
                                            break;
                                        case "detective":
                                            infoText = "Detective: tripulante que puede ver las huellas de los jugadores." +
                                                "\nLas huellas no aparecen cerca de las rejillas y solo aparecen mientras el Detective esta vivo.";
                                            break;
                                        case "forensic":
                                            infoText = "Forensic: tripulante que puede obtener pistas reportando cuerpos y hablando con fantasmas." +
                                                "\nLas pistas al reportar cuerpos pueden ser el nombre del asesino, su tono de color o algo sobre su apariencia." +
                                                "\nLos fantasmas de los jugadores que murieron aparecen tras la siguiente ruenion y sus pistas pueden ser que rol tiene el asesino, su tono de color y cuanto tiempo paso hasta que reportaron el cuerpo.";
                                            break;
                                        case "timetraveler":
                                            infoText = "Time Traveler: tripulante que puede rebobinar el tiempo dos veces por partida, reviviendo a los jugadores que murieron durante ese tiempo." +
                                                "\nSolo puede rebobinar el tiempo si no hay un sabotaje activo o si lo intenta matar mientras tiene el escudo temporal activado." +
                                                "\nNo puede usar Vitales.";
                                            break;
                                        case "squire":
                                            infoText = "Squire: tripulante que puede proteger a otro jugador con un escudo." +
                                                "\nEl escudo dura hasta que el Squire es expulsado, asesinado o se convoca una reunion. Si intentas matar al escudado los Impostores, Rebeles, Sheriff, Squire y el escudado escucharan un sonido.";
                                            break;
                                        case "cheater":
                                            infoText = "Cheater: tripulante que puede intercambiar los votos de dos jugadores." +
                                                "\nSi cambia los votos y el expulsado era inocente, el Cheater tambien es expulsado.";
                                            break;
                                        case "fortuneteller":
                                            infoText = "Fortune Teller: tripulante que puede revelear quien es bueno o malo." +
                                                "\nTras revelar a un jugador se escuchara un sonido y la pantalla se volvera azul para ambos jugadores dependiendo de la opcion de a quien notificar que ha sido revelado." +
                                                "\nEl nombre de los jugadores revelados se volvera rojo si es malo y azul si es bueno.";
                                            break;
                                        case "hacker":
                                            infoText = "Hacker: tripulante que puede usar Admin y Vitales a distancia y obtener mas informacion que los demas." +
                                                "\nPuede ver el color de los jugadores en Admin y cuando tiempo lleva muerto un jugador en Vitales.";
                                            break;
                                        case "sleuth":
                                            infoText = "Sleuth: tripulante que puede rastrear a un jugador y cuerpos." +
                                                "\nLa flecha rastreadora de jugadores es azul mientras que la de cuerpo es verde.";
                                            break;
                                        case "fink":
                                            infoText = "Fink: tripulante que puede alejar la camara del mapa y revelar quienes son los Impostores si finaliza todas sus tareas antes de morir." +
                                                "\nNo puede moverse cuando aleja la camara." +
                                                "\nLos Impostores sabran quien es el Fink cuando le quede cierto numero de tareas por hacer y tendran un aviso si esta usando su habilidad de alejar la camara.";
                                            break;
                                        case "kid":
                                            infoText = "Kid: tripulante que no debe ser expulsado ni matado, de lo contrario todos perderan." +
                                                "\nEs mas pequeño que otros jugadores.";
                                            break;
                                        case "welder":
                                            infoText = "Welder: tripulante que puede sellar rejillas." +
                                                "\nEl sellado se produce tras una reunion y provoca que esa rejilla no pueda ser usada para entrar ni salir, pero si es posible usarla de tunel.";
                                            break;
                                        case "spiritualist":
                                            infoText = "Spiritualist: tripulante que puede revivir a otro jugador a costa de su propia vida." +
                                                "\nDebe permanecer un tiempo cerca del cuerpo para revivirlo, pero si alguien convoca una reunion mientras intenta revivirlo, se muere." +
                                                "\nLos Impostores y Rebeles obtiene una flecha rosa apuntando al jugador revivido.";
                                            break;
                                        case "vigilant":
                                            infoText = "Vigilant: tripulante que puede poner camaras adicionales en el mapa." +
                                                "\nLas camaras se vuelven utilizables tras una reunion y cuando se ha alcanzado el maximo numero de camaras obtiene la habilidad de ver las camaras a distancia basada en usos." +
                                                "\nEn el mapa MiraHQ puede usar el DoorLog a distancia en vez de poner camaras.";
                                            break;
                                        case "hunter":
                                            infoText = "Hunter: tripulante que puede marcar a un jugador para que muera cuando se muere el." +
                                                "\nSi es expulsado de la nave, al jugador marcado no le pasa nada.";
                                            break;
                                        case "jinx":
                                            infoText = "Jinx: tripulante que puede gafar las habilidades de los demas." +
                                                "\nSi estas gafado, ese jugador escuchara un sonido y su boton entrara en tiempo de recarga.";
                                            break;
                                        case "coward":
                                            infoText = "Coward: tripulante que puede convocar reuniones a distancia pero no puede hacerlo si hay un sabotaje activo.";
                                            break;
                                        case "bat":
                                            infoText = "Bat: tripulante que puede emitir una frecuencia que alterara los tiempos de recarga de los botones." +
                                                "\nLos Tripulantes, Rebeldes y Neutrales cercanos a el veran sus tiempos de recarga acelerados el doble de rapido." +
                                                "\nLos Impostores en su lugar veran el tiempo de recarga en aumento.";
                                            break;
                                        case "necromancer":
                                            infoText = "Necromancer: tripulante que puede mover cuerpos y revivirlos tras llevarlos a una habitacion concreta pero aleatoria." +
                                                "\nLa habitacion esta marcada por una flecha azul." +
                                                "\nLos Impostores y Rebeles obtiene una flecha verde apuntando al jugador revivido.";
                                            break;
                                        case "engineer":
                                            infoText = "Engineer: tripulante que puede poner trampas que alteran la velocidad de desplazamiento y detectan la posicion de jugadores." +
                                                "\nPuede cambiar el tipo de trampa a colocar con la F." +
                                                "\nLas trampas se vuelven activas tras una reunion y su efecto dura 5 segundos.";
                                            break;
                                        case "shy":
                                            infoText = "Shy: tripulante que puede revelar si hay jugadores cercanos a el." +
                                                "\nUna flecha apuntara al jugador mas cercado a el.";
                                            break;
                                        case "taskmaster":
                                            infoText = "Task Master: tripulante que tiene tareas extra para hacer al completar las iniciales." +
                                                "\nSi completa las extra sin morir, obtiene una victoria para los Tripulantes.";
                                            break;
                                        case "jailer":
                                            infoText = "Jailer: tripulante que puede marcar a un jugador para que sea su asistente." +
                                                "\nSi intentas matar al asistente, seras teletransportado a la prision durante un tiempo, esto solo funciona una vez y el Jailer debera seleccionar a otro jugador.";
                                            break;

                                        // Modifiers:
                                        case "lovers":
                                            infoText = "Lovers: modificador que vincula a dos jugadores." +
                                                "\nSi uno muere o es expulsado, el otro tambien.";
                                            break;
                                        case "lighter":
                                            infoText = "Lighter: modificador que incrementa el rango de vision de un jugador." +
                                                "\nAdemas es inmune a la vision nocturna.";
                                            break;
                                        case "blind":
                                            infoText = "Blind: modificador que reduce el rango de vision de un jugador." +
                                                "\nNo afecta a Impostores.";
                                            break;
                                        case "flash":
                                            infoText = "Flash: modificador que incrementa la velocidad de movimiento de un jugador." +
                                                "\nEste efecto no se aplica durante el duelo del Challenger, escondite del Seeker o comunicaciones anonimas.";
                                            break;
                                        case "bigchungus":
                                            infoText = "Big Chungus: modificador que reduce la velocidad de movimiento de un jugador y lo hace mas grande." +
                                                "\nEste efecto no se aplica durante el duelo del Challenger, escondite del Seeker o comunicaciones anonimas.";
                                            break;
                                        case "thechosenone":
                                            infoText = "The Chosen One: modificador que forzara a su asesino a reportar el cuerpo de ese jugador." +
                                                "\nUna demora de reporte de hasta 5 segundos puede ser configurada.";
                                            break;
                                        case "performer":
                                            infoText = "Performer: modificador cuya muerte activa una alarma y una flecha revela la posicion del cuerpo de ese jugador." +
                                                "\nLa duracion de la Alarma puede ser configurada.";
                                            break;
                                        case "pro":
                                            infoText = "Pro: modificador que invierte los controles de movimiento de un jugador." +
                                                "\nTambien afecta si eres fantasma.";
                                            break;
                                        case "paintball":
                                            infoText = "Paintball: modificador que salpica al asesino de ese jugador, haciendo que el asesino deje un rastro del color del jugador asesinado durante un tiempo.";
                                            break;
                                        case "electrician":
                                            infoText = "Electrician: modificador que paraliza durante unos segundos a su asesino.";
                                            break;

                                        // Gamemodes:
                                        case "capturetheflag":
                                        case "ctf":
                                            infoText = "Capture The Flag: modo de juego que enfrenta al equipo rojo y azul donde tendran que robar la bandera del equipo rival y llevarla a su base." +
                                                "\nTodos pueden matar y usar rejillas, pero no puedes hacerlo si llevas la bandera." +
                                                "\nEn partidas cuyo numero de jugadores es impar, aparecera el Roba Banderas, un rol que si mata al jugador que lleva la bandera intercambiara equipos con el.";
                                            break;
                                        case "policeandthieves":
                                        case "pat":
                                            infoText = "Police and Thieves: modo de juego que enfrenta al equipo Policias (cian) y Ladrones (marron) en el que los Policias deben capturar a todos los Ladrones antes de que ellos roben todas las joyas." +
                                                "\nEs modo de juego añade una nueva zona en el mapa, la Prision, lugar al que los Ladrones capturados son teletransportados. Para liberar a un Ladron debe pulsarse el boton que esta delante de la puerta de la Prision." +
                                                "\nPuede haber un maximo de 9 Ladrones y 6 Policias, de los cuales 2 seran Tasers, que pueden paralizar a un Ladron unos segundos." +
                                                "\nLos Ladrones pueden usar rejillas pero si llevan una joya no pueden hacerlo.";
                                            break;
                                        case "kingofthehill":
                                        case "koth":
                                            infoText = "King of The Hill: modo de juego que enfrenta al equipo verde y amarillo. Cada equipo tiene un Rey que puede capturar las 3 zonas del mapa." +
                                                "\nCada zona capturada da un punto por segundo al equipo que posee. Todos pueden usar rejillas y matar excepto los Reyes." +
                                                "\nEn partidas cuyo numero de jugadores es impar, aparecera el Usurpador, un rol que si mata a un Rey se convierte en el nuevo Rey de ese equipo.";
                                            break;
                                        case "hotpotato":
                                        case "hp":
                                            infoText = "Hot Potato: modo de juego en el que un jugador es nombrado Patata Caliente y debe pasarsela a otro jugador antes de que explote." +
                                                "\nCuando una Patata Caliente explota, se convierte en Patata Quemada y un jugador vivo obtiene el rol de Patata Caliente de forma aleatoria.";
                                            break;
                                        case "zombielaboratory":
                                        case "zl":
                                            infoText = "Zombie Laboratory: modo de juego que enfrenta al equipo Zombies y Supervivientes." +
                                                "\nLos Zombies deben infectar a todos para ganar, mientras que los Supervivientes deben buscar los objetos clave para hacer la cura dentro de las cajas del mapa y llevarlos a la Enfermeria, una nueva zona del mapa en la que aparece el rol Enfermera, que puede recoger botiques en el mapa para o bien matar un Zombie o curar la infeccion de un Superviviente." +
                                                "\nLos Zombies pueden usar rejillas, la Enfermera no puede ser infectada.";
                                            break;
                                        case "battleroyale":
                                        case "br":
                                            infoText = "Battle Royale: modo de juego en el que todos pueden matar a distancia con la mecanica del rol Archer (apuntar con el raton y clic derecho para disparar)." +
                                                "\nNo se pueden usar rejillas en este modo, los jugadores tendran vidas al lado de su nombre y cuando llega a 0 vidas mueren para el resto de la partida." +
                                                "\nHay 3 formas de jugar, Todos Contra Todos, Por Equipos y Por Puntuacion." +
                                                "\nEn partidas cuyo numero de jugadores es impar, aparecera el rol Serial Killer, un rol muy poderoso que tiene 3 veces mas vidas que los demas jugadores y dispara mas rapido.";
                                            break;
                                    }
                                    break;
                                // Japanese
                                case 3:
                                    switch (subText.ToLower()) {
                                        // Impostor roles
                                        case "mimic":
                                            infoText = "Mimic: 他のプレイヤーになりすますことができるインポスター。";
                                            break;
                                        case "painter":
                                            infoText = "Painter: 他のプレイヤーをランダムな色で装飾できるインポスター。";
                                            break;
                                        case "demon":
                                            infoText = "Demon: 噛みつくことでプレイヤーを時間をかけて殺せるインポスター。" +
                                                "\nもし悪魔が存在する場合、1ゲーム、1マップごとに修道女を配置するボタンがあります。" +
                                                "\n修道女のとなりに滞在することで、噛みつきを無効にできます。";
                                            break;
                                        case "janitor":
                                            infoText = "Janitor: 死体を取り除いたり、移動できるインポスター。" +
                                                "\n同時に死体を取り除き、移動することはできません。";
                                            break;
                                        case "illusionist":
                                            infoText = "Illusionist: 専用のベントネットワークを構築し、どこからでも照明をおとせるインポスター。" +
                                                "\n3つのベントネットワークは自分でしか使用できず、3番目のベントを配置した直後に。" +
                                                "\nすべての人にみえるようになります。ベントネットワーク構築後、照明をオフにすることができます。";
                                            break;
                                        case "manipulator":
                                            infoText = "Manipulator: 隣接するプレイヤーを殺せるインポスター。" +
                                                "\nの能力は自身を含む誰でも殺害可能。";
                                            break;
                                        case "bomberman":
                                            infoText = "Bomberman: 爆弾をマップに配置できるインポスター。" +
                                                "\n妨害が実行されている場合、またはプレイヤーがインポスターの近くにいるときは爆弾は配置できません。" +
                                                "\nプレイヤーは爆弾に触れることで、信管を取り除くことができます。爆弾の信管が取り除かれない場合、インポスターが勝利します。";
                                            break;
                                        case "chameleon":
                                            infoText = "Chameleon: 姿を消せるインポスター。" +
                                                "\nベントを使用することはできません。姿はみえない状態でも、killボタンを持っている他の役職により殺される可能性はあります。";
                                            break;
                                        case "gambler":
                                            infoText = "Gambler: 会議中に選択した役割のプレイヤーを撃つことができるインポスター。" +
                                                "\nプレイヤーを撃つには役割を推測して充てる必要があります。もし、間違った場合、自分自身を撃つことになります。";
                                            break;
                                        case "sorcerer":
                                            infoText = "Sorcerer: 他のプレイヤーに呪文を唱えることができるインポスター。" +
                                                "\n呪文を唱えられたプレイヤーはミーティング中、名前の横に紫のかぼちゃのアイコンが表示され、魔術師が投票されない場合、ミーティング後に死亡します。";
                                            break;
                                        case "medusa":
                                            infoText = "Medusa: 他のプレイヤーを石化できるインポスター。" +
                                                "\n石化したプレイヤーは一定の間、移動できなくなります。";
                                            break;
                                        case "hypnotist":
                                            infoText = "Hypnotist: プレイヤーの動きの制御を反対にさせる罠を配置できるインポスター。" +
                                                "\n罠は会議の後に有効になり、インポスターには影響を与えません。";
                                            break;
                                        case "archer":
                                            infoText = "Archer: 長距離kill ができるインポスター。ただし、通常のkill はできません。" +
                                                "\n他のプレイヤーからは見えない弓を使用して、マウスで標準をあわせ、右クリックにて射ぬく必要があります。" +
                                                "\n標的を逃した場合、インポスターの位置に警告が表示され、標的を殺した場合は、標的の上に警告が表示されます。";
                                            break;
                                        case "plumber":
                                            infoText = "Plumber: ベント可能な役割のために、ベントを作ることができるインポスター。" +
                                                "\nベントはミーティング後にすべての追加ベントが配置された場合に有効になります。";
                                            break;
                                        case "librarian":
                                            infoText = "Librarian: プレイヤーが会議で話すのを妨げることができるインポスター。" +
                                                "\n誰もが会議中に誰が妨げられているかを知ることができます。";
                                            break;


                                        // Rebel roles
                                        case "renegade":
                                            infoText = "Renegade: みんなを殺さなければいけない反乱軍" +
                                                "\n彼はミニオンを募集して殺害を助けることができます。";
                                            break;
                                        case "bountyhunter":
                                            infoText = "Bounty Hunter: 特定のプレイヤーを殺害しなければいけない反乱軍。" +
                                                "\nターゲットボタンはランダムにターゲットにすべきプレイヤーの役割を設定しますが、\r\nもし、そのターゲットが既に死んでいる場合はその物も死ぬことになります。";
                                            break;
                                        case "trapper":
                                            infoText = "Trapper: 地雷で全員を殺害する反乱者。" +
                                                "\n触れたプレイヤーを根こそぎ殺す罠や、踏んだ者を殺す地雷を設置することができます。";
                                            break;
                                        case "yinyanger":
                                            infoText = "Yinyanger: 毎回2人のプレイヤーをマークする全員を殺さなければならない反乱者。" +
                                                "\n彼は、彼らが持っている可能性のあるシールドを無視して両方を衝突させるならば、彼は陰でプレーヤーとヤンと一緒に別のプレイヤーをマークすることができます。" +
                                                "\n1人のプレーヤーをマークした後、マークされた1つがターゲットに近すぎる場合、もう1つのプレーヤーをマークすることはできません。";
                                            break;
                                        case "challenger":
                                            infoText = "Challenger: じゃんけんデュエルで全員を殺さねばならない反乱軍。" +
                                                "\n妨害は実行されていない場合、10秒後に選ばれたプレイヤーがデュエルアリーナへテレポートします。" +
                                                "\n誰も攻撃を選ばなかった場合、自動的に他の人が勝利します。攻撃を選択しない場合は両方とも死にます。引き分けでは誰も死にません。";
                                            break;
                                        case "ninja":
                                            infoText = "Ninja: 誰もがダブルキルをしている人を殺さなければならない反逆者。" +
                                                "\nプレーヤーを選択して、その後、その過程で彼を殺した後、自分の立場にテレポートします。彼はすぐに通常のキルボタンを使用してダブルキルを行うことができます。";
                                            break;
                                        case "berserker":
                                            infoText = "Berserker:全員を殺さなければならないが、殺すのを止めることはできない反乱者。" +
                                                "\n初めて殺した後、彼のキルボタンは永久に10秒のクールダウンを受け取りますが、彼が一定の時間を殺さないと彼は死にます。";
                                            break;
                                        case "yandere":
                                            infoText = "Yandere: ターゲットを数回ストーキングし、それを殺して勝つ必要がある反乱軍。" +
                                                "\nターゲットが別のプレーヤーに追放または殺された場合、彼は大暴れモードに入り、代わりに勝つために全員を殺さなければなりません。";
                                            break;
                                        case "stranded":
                                            infoText = "Stranded: 地図の周りの箱で弾薬を見つけて3人のプレイヤーを殺さなければならない反逆者。" +
                                                "\n彼はまた、しばらく目に見えないアイテムとベント能力を見つけることができます。";
                                            break;
                                        case "monja":
                                            infoText = "Monja: 小さなモンジャを見つけて儀式の場所に連れて行かなければならない反乱者。" +
                                                "\nすべてのモンジャが配達されると、彼女は60秒以内にすべての人を殺すためにモンジャに変身することができます。そうでなければ彼女は死ぬでしょう。" +
                                                "\n子供が登場した場合、モンジャは子供を除くすべての人を殺して勝つために殺さなければなりません。";
                                            break;

                                        // Neutral roles
                                        case "joker":
                                            infoText = "Joker: 勝つために投票されなければならないニュートラル。" +
                                                "\n彼は生きている場合にのみ妨害することができます。";
                                            break;
                                        case "rolethief":
                                            infoText = "Role Thief: 勝利状態のないニュートラル。" +
                                                "\n彼は他のプレイヤーの役割を盗むことができますが、彼が詐欺師や反逆者の役割を盗もうとすると死ぬ。";
                                            break;
                                        case "pyromaniac":
                                            infoText = "Pyromaniac: 勝つために全員を点火しなければならないニュートラル。" +
                                                "\n彼は彼らの隣に立って、彼が勝ったすべての人をスプレーしたら、プレーヤーをスプレーすることができます。";
                                            break;
                                        case "treasurehunter":
                                            infoText = "Treasure Hunter: 勝つために宝物を探さなければならないニュートラル。" +
                                                "\n彼のボタンは、必要な量を見つけた後、マップ上でラウンドごとに1つの宝物をランダムに発生させます。";
                                            break;
                                        case "devourer":
                                            infoText = "Devourer: 勝つために体を食べなければならない中立。" +
                                                "\nプレーヤーが死ぬと彼は音を聞きます。";
                                            break;
                                        case "poisoner":
                                            infoText = "Poisoner: 勝つために皆を毒しなければならないニュートラル。" +
                                                "\n彼は毒されたプレーヤーを選択し、毒されたプレーヤーは毒の隣に立って毒計を増やします。" +
                                                "\nすべてのプレイヤーが100％の毒メーターに達すると、彼は勝ちます。";
                                            break;
                                        case "puppeteer":
                                            infoText = "Puppeteer: 他のプレイヤーにモーフィングできるニュートラルで、勝つために数回変化している間に殺さなければなりません。" +
                                                "\n彼はプレイヤーからサンプルを選んで、望んでいる間、または会議が呼ばれるまで、彼は殺されるか、自分自身を明らかにすることにしたことに決めます。" +
                                                "\nモーフィング中に殺された場合、彼は1つのポイントを獲得し、モーフを始めた場所で復活し、必要なポイントに到達した後に勝ちます。";
                                            break;
                                        case "exiler":
                                            infoText = "Exiler: 勝つために特定のプレーヤーに投票しなければならないニュートラル。" +
                                                "\n彼のターゲットボタンは、ランダムなプレーヤーを自分のターゲットに割り当てますが、ターゲットがすでに死んでいる場合、彼は死にます。";
                                            break;
                                        case "amnesiac":
                                            infoText = "Amnesiac: 身体を報告している彼の役割を覚えているニュートラル。" +
                                                "\nロールはそれを覚えてからリセットします。";
                                            break;
                                        case "seeker":
                                            infoText = "Seeker: 中立的な人は、hide＆seek minigameで人々を見つけなければなりません。" +
                                                "\n3人を選択すると、彼はすべてのプレーヤーを新しいゾーンにテレポートするミニゲームを開始できます。" +
                                                "\n彼は、彼が勝つ必要なポイントに達した後、ミニゲームで見つかったプレーヤーごとに1ポイントを獲得します。";
                                            break;

                                        // Crewmate roles
                                        case "captain":
                                            infoText = "Captain: 投票が2倍になる乗組員。" +
                                                "\n彼はすべての票をゲームごとに1回別のプレーヤーにリダイレクトできますが、彼がそれらを乗組員にリダイレクトすると追放されます。" +
                                                "\n詐欺師または反逆者と一人でいることで、彼は残りのプレーヤーを亡命させるために緊急会議に電話することができます。";
                                            break;
                                        case "mechanic":
                                            infoText = "Mechanic: どこからでも一定数を妨害することができる乗組員。" +
                                                "\n彼はまた、2番目のプレーヤーを必要とせずに緊急タスクの1つだけを行うことにより、妨害行為全体を修正します。";
                                            break;
                                        case "sheriff":
                                            infoText = "Sheriff: 選手を殺すことができるクルーメイト。" +
                                                "\nしかし、彼が乗組員を殺そうとすると彼は死にます。";
                                            break;
                                        case "detective":
                                            infoText = "Detective: プレイヤーの足跡を見ることができるクルーメイト。" +
                                                "\nフットプリントはベントを閉じないで、探偵が生きている場合にのみスポーンしません。";
                                            break;
                                        case "forensic":
                                            infoText = "Forensic: 身体を報告し、プレイヤーの幽霊に尋ねることで手がかりを得る乗組員。" +
                                                "\n身体を報告することで彼が得ることができる手がかりは、キラーの名前、色の種類、または彼の外観に関する何かです。" +
                                                "\nゴーストは次のラウンドに登場し、身体が存在し、その手がかりがそのプレーヤー、キラーの色のタイプ、または体を報告する前にどれだけの時間が経過したかを殺しました。";
                                            break;
                                        case "timetraveler":
                                            infoText = "Time Traveler: ゲームごとに2回時間を巻き戻すことができ、巻き戻し中に殺されたプレイヤーを復活させることができるクルーメイト。" +
                                                "\n彼は、積極的な妨害行為がなければ時間を巻き戻すことができます。時間は、彼がタイムシールドで保護されている間に誰かが彼を殺そうとする場合にも巻き戻します。" +
                                                "\nこの役割はバイタルを使用できません。";
                                            break;
                                        case "squire":
                                            infoText = "Squire: プレーヤーにシールドを置くことができるクルーメイト。" +
                                                "\n彼が追放されたり殺されたりするまで、盾は続きます。シールドプレーヤーを殺そうとすると、詐欺師、反乱軍、保安官、スクワイア、シールドプレイヤーが聞いた音が引き起こされます。";
                                            break;
                                        case "cheater":
                                            infoText = "Cheater: 2人のプレーヤーの票を交換できるクルーメイト。" +
                                                "\n投票を交換した後、これら2人のプレーヤーのうちの1人が乗組員で​​あることが判明した場合、彼は追放されます。";
                                            break;
                                        case "fortuneteller":
                                            infoText = "Fortune Teller: プレーヤーが良いか悪いかを明らかにできるクルーメイト。" +
                                                "\n明らかにすると、音がトリガーされ、両方のプレイヤーの画面が青くなります。" +
                                                "\n名前は悪い役割では赤くなり、シアンは良い役割については赤くなります。";
                                            break;
                                        case "hacker":
                                            infoText = "Hacker: どこからでも管理者とバイタルを使用し、彼らからより多くの情報を得ることができる乗組員。" +
                                                "\n彼のハック能力はアクティブですが、彼は誰かがバイタルで亡くなって以来、管理者が管理者の色の色とどれくらいの時間が経過したかを見ることができます。";
                                            break;
                                        case "sleuth":
                                            infoText = "Sleuth: 身体と1人のプレイヤーの位置を追跡できるクルーメイト。" +
                                                "\n彼は、追跡されたプレーヤーと緑のプレーヤーを指している青い矢印が体を指しているのを見ます。";
                                            break;
                                        case "fink":
                                            infoText = "Fink: 詐欺師がタスクを終えた後に誰であるかを明らかにし、カメラをズームアウトできる乗組員。" +
                                                "\nカメラがズームアウトされている間、彼は動くことができません。" +
                                                "\n詐欺師はまた、いくつかのタスクが残っているとき、またはカメラをズームアウトしているときにフィンクが誰であるかを知っています。";
                                            break;
                                        case "kid":
                                            infoText = "Kid: 殺されたり追放されるべきではない乗組員、さもなければ全員が損失します。" +
                                                "\n彼は他のプレイヤーよりも小さいです。";
                                            break;
                                        case "welder":
                                            infoText = "Welder: 通気口を無効にできる乗組員。" +
                                                "\nこれらの通気孔は次の会議の後に利用できなくなり、入場または退出することはできませんが、それでもトンネルとして使用できます。";
                                            break;
                                        case "spiritualist":
                                            infoText = "Spiritualist: 自分の人生を犠牲にして別のプレーヤーを復活させることができる乗組員。" +
                                                "\n彼はそれを復活させるために体の隣にとどまる必要がありますが、誰かが復活しようとする間に会議に電話した場合、代わりに死にます。" +
                                                "\n詐欺師と反乱軍は、復活したプレーヤーを指すピンクの矢を手に入れます。";
                                            break;
                                        case "vigilant":
                                            infoText = "Vigilant: 地図上に4つの余分なカメラを配置できる乗組員。" +
                                                "\nカメラは会議の後に利用可能になり、彼がすべてのカメラを配置すると、カメラをリモートすることができます。" +
                                                "\nMiraHQでは、代わりにドアログをリモートすることができます。";
                                            break;
                                        case "hunter":
                                            infoText = "Hunter: 彼が殺された場合に死ぬ別のプレーヤーをマークすることができるクルーメイト。" +
                                                "\n彼を追放することは、マークされたプレーヤーを亡命しません。";
                                            break;
                                        case "jinx":
                                            infoText = "Jinx: 他のプレイヤーのボタンをブロックできるクルーメイト。" +
                                                "\nジンクス中にボタンを使用すると、そのボタンはクールダウンに入ります。";
                                            break;
                                        case "coward":
                                            infoText = "Coward: どこからでも会議に電話できる乗組員。" +
                                                "\n妨害行為がアクティブであれば、彼は会議に電話することができません。";
                                            break;
                                        case "bat":
                                            infoText = "Bat: crewmate who can emit a frequency that alters button cooldown." +
                                                "\nクルーメイト、反乱軍、ニュートラルのボタンのクールダウンは、x2をより速くします。" +
                                                "\n詐欺師のボタンのクールダウンは毎秒 1 ずつ増加します。";
                                            break;
                                        case "necromancer":
                                            infoText = "Necromancer: 身体をドラッグアンドドロップし、それらを復活させることができる乗組員は、特定の部屋にドラッグします。" +
                                                "\n特定の部屋は青い矢印で尖っています。" +
                                                "\n詐欺師と反乱軍は、復活したプレーヤーを指すピンクの矢を手に入れます。";
                                            break;
                                        case "engineer":
                                            infoText = "Engineer: 速度を上げたり減少させたり、トラップを配置したりできる乗組員。" +
                                                "\n彼はFキーでトラップタイプを切り替えることができます。" +
                                                "\nトラップは会議の後にアクティブになり、5秒の効果の期間があります。";
                                            break;
                                        case "shy":
                                            infoText = "Shy: 最も近いプレーヤーの地位を明らかにすることができるクルーメイト。" +
                                                "\n矢印は、最も近いプレーヤーの方向を明らかにします。";
                                            break;
                                        case "taskmaster":
                                            infoText = "Task Master: 最初のタスクを行った後に余分なタスクを持っている乗組員。" +
                                                "\n殺される前に余分なタスクを完了すると、乗組員の勝利が得られます。";
                                            break;
                                        case "jailer":
                                            infoText = "Jailer: プレーヤーをアシスタントにマークできるクルーメイト。" +
                                                "\nアシスタントを殺そうとすると、殺害を否定し、殺人者を数秒間刑務所にテレポートしますが、これは1回だけ機能し、看守はこの後に別のプレーヤーをマークする必要があります。";
                                            break;

                                        // Modifiers:
                                        case "lovers":
                                            infoText = "Lovers: 2人のプレーヤーをリンクする修飾子。" +
                                                "\n殺されたり追放されたりすると、両方とも死にます。";
                                            break;
                                        case "lighter":
                                            infoText = "Lighter: プレーヤーにより多くのビジョンを与える修飾子。" +
                                                "\nまた、そのプレイヤーは暗視に免疫があります。";
                                            break;
                                        case "blind":
                                            infoText = "Blind: プレーヤーのビジョンを減らす修飾子。" +
                                                "\n詐欺師には影響しません。";
                                            break;
                                        case "flash":
                                            infoText = "Flash: プレーヤーの速度を上げる修飾子。" +
                                                "\nチャレンジャーの決闘と匿名の通信中には影響しません。";
                                            break;
                                        case "bigchungus":
                                            infoText = "Big Chungus: プレーヤーのサイズを大きくして速度を下げる修飾子。" +
                                                "\nチャレンジャーの決闘と匿名の通信中には影響しません。";
                                            break;
                                        case "thechosenone":
                                            infoText = "The Chosen One: 彼の殺人者に彼の体を報告させる修飾子。" +
                                                "\nレポート遅延を構成できます。";
                                            break;
                                        case "performer":
                                            infoText = "Performer: 死が音楽と矢がトリガーされる修飾子が彼の立場を楽しんでいます。" +
                                                "\n音楽の期間を構成できます。";
                                            break;
                                        case "pro":
                                            infoText = "Pro: プレーヤーの動きを反転させる修飾子。" +
                                                "\nまた、ゴーストフォームのプレーヤーにも影響します。";
                                            break;
                                        case "paintball":
                                            infoText = "Paintball: 死亡時に殺人者をはねかける修飾子、殺人者は数秒間プレーヤーの色でトレイルを残します。";
                                            break;
                                        case "electrician":
                                            infoText = "Electrician: 数秒間彼の殺人者を麻痺させる修飾子。";
                                            break;

                                        // Gamemodes:
                                        case "capturetheflag":
                                        case "ctf":
                                            infoText = "Capture The Flag: 各チームが相手のチームフラグを盗み、それをベースに持ち込む必要がある赤と青のチームの間のゲームモード。" +
                                                "\n誰もが通気して殺すことができますが、旗を持っている間はできません。" +
                                                "\nOdd Player Number Gamesでは、Flag Stealerと呼ばれる特別な役割が表示されます。彼がフラグでプレイヤーを殺すと、彼はチームを切り替えます。";
                                            break;
                                        case "policeandthieves":
                                        case "pat":
                                            infoText = "Police and Thieves: シアンチームとブラウンチームの間のゲームモードは、警察チームがすべての泥棒を捕らえなければなりませんが、泥棒チームは捕らえられることなく地図上のすべての宝石を盗まなければなりません。" +
                                                "\nこのゲームモードは、新しいマップルーム、刑務所を追加します。" +
                                                "\nプレイヤーの割り当ては、最大9泥棒と6つのポリシーです。" +
                                                "\n泥棒は通気することができますが、宝石を持っている間はできません。";
                                            break;
                                        case "kingofthehill":
                                        case "koth":
                                            infoText = "King of The Hill: 緑と黄色のチームの間のゲームモード。マップには3つの捕虜ゾーンがあり、各チームにはそれらをキャプチャできる王がいます。" +
                                                "\n誰もが王を除いて排出して殺すことができます。" +
                                                "\n奇妙なプレイヤー番号のゲームでは、王を殺すと、王と呼ばれる特別な役割が登場します。彼は彼とチームをスイッチします。";
                                            break;
                                        case "hotpotato":
                                        case "hp":
                                            infoText = "Hot Potato: ランダムなプレーヤーがホットポテトの役割を獲得し、時間がなくなる前にホットポテトを別のプレイヤーに与える必要があるゲームモード、そうでなければホットポテトプレーヤーは死にます。" +
                                                "\nゲームでは1匹のホットポテトしかありません。残りは冷たいジャガイモや燃えたジャガイモが亡くなった場合に焦げたジャガイモになります。" +
                                                "\nこのゲームモードは、かくれんぼとしても機能します。";
                                            break;
                                        case "zombielaboratory":
                                        case "zl":
                                            infoText = "Zombie Laboratory: ランダムプレーヤーがゾンビの役割を獲得し、別のプレーヤーが看護師の役割を担当し、残りは生存者になるゲームモード。" +
                                                "\nゾンビはすべての人に勝つために感染しなければなりませんが、生存者は地図の周りの箱に隠れた重要なアイテムを探して、看護師が治療を行い、ゲームに勝つことができる新しい地図ゾーン、診療所にそれらを届けなければなりません。" +
                                                "\nゾンビのみが通気することができ、看護師は感染することができず、感染したプレイを癒すためにメドキットを選ぶことができます。";
                                            break;
                                        case "battleroyale":
                                        case "br":
                                            infoText = "Battle Royale: 誰もがアーチャーのメカニック（右マウスのクリックで使用可能）と非常に低いクールダウンで遠隔キルボタンを持っているゲームモード。" +
                                                "\n誰もベントできず、プレイヤーは生命を持つことができず、彼らが0人の生命に達すると、彼らは試合の残りの部分で死ぬ、最後の生きたものが勝ちます。" +
                                                "\nまた、このゲームモードをプレイする方法は3つあります。すべてのVS、チームバトルまたはスコアバトルがあります。" +
                                                "\nチームとスコアバトルの奇妙なプレイヤー番号ゲームでは、シリアルキラーと呼ばれる強力なニュートラルな役割があり、これもすべての人を殺したり、必要なスコアに達したりすることができます。この役割にはX3寿命があり、半分のクールダウンがあります";
                                            break;
                                    }
                                    break;
                                // Chinese
                                case 4:
                                    switch (subText.ToLower()) {
                                        // Impostor roles
                                        case "mimic":
                                            infoText = "Mimic: 内鬼阵营：可以模仿其他玩家的外表。";
                                            break;
                                        case "painter":
                                            infoText = "Painter: 内鬼阵营：可以让其他玩家变为同一个外形并隐藏名字。";
                                            break;
                                        case "demon":
                                            infoText = "Demon: 内鬼阵营：咬伤玩家后，那位玩家会慢慢失血而亡。" +
                                                "\n如果有吸血鬼在场，所以所有船员会获得一个修女。" +
                                                "\n在修女旁边会使咬人无效果。";
                                            break;
                                        case "janitor":
                                            infoText = "Janitor: 内鬼阵营：可以移动尸体。" +
                                                "\n但是不可以在同一时间重新移动尸体。";
                                            break;
                                        case "illusionist":
                                            infoText = "Illusionist: 内鬼阵营：可以放置3个帽子，帽子之间相互连通，并且可以随时随地控制灯光。" +
                                                "\n这3个帽子只能供自己使用，当全部放置完毕后，所有人都可以看到帽子，且获得控制灯光的能力。";
                                            break;
                                        case "manipulator":
                                            infoText = "Manipulator: 内鬼阵营：可以在任何地方操纵一个玩家击杀被操控者附近的一个玩家。" +
                                                "\n可以用他的能力杀死任何人，包括他自己。";
                                            break;
                                        case "bomberman":
                                            infoText = "Bomberman: 内鬼阵营：可以在地图上放置炸弹。" +
                                                "\n如果处于破坏状态或附近有人时，炸弹就不能被放置。" +
                                                "\n其他玩家可以通过接触来拆除炸弹，如果炸弹没有被及时拆除，内鬼将会获胜。";
                                            break;
                                        case "chameleon":
                                            infoText = "Chameleon: 内鬼阵营：可以隐身。" +
                                                "\n不能使用管道。在隐身期间，仍然可以被击杀。";
                                            break;
                                        case "gambler":
                                            infoText = "Gambler: 内鬼阵营：在会议期间猜测其他玩家的身份，猜对可以杀死对方，猜错会死亡。" +
                                                "\n猜测界面只会显示本局存在身份。";
                                            break;
                                        case "sorcerer":
                                            infoText = "Sorcerer: 内鬼阵营：可以向玩家施加诅咒。" +
                                                "\n在会议期间，被诅咒的玩家名字旁边会有一个紫色的南瓜图标。会议结束后被诅咒玩家将会死亡，除非巫师被放逐。";
                                            break;
                                        case "medusa":
                                            infoText = "Medusa: 内鬼阵营：能将其他玩家石化。" +
                                                "\n被石化的玩家在一定时间内不能移动。";
                                            break;
                                        case "hypnotist":
                                            infoText = "Hypnotist: 内鬼阵营：通过放置陷阱，让被踩中玩家迷失方向。" +
                                                "\n陷阱在会议后生效，陷阱不影响内鬼阵营玩家。";
                                            break;
                                        case "archer":
                                            infoText = "Archer: 内鬼阵营：只能使用弓进行击杀，不能进行普通击杀。" +
                                                "\n需要使用弓箭（其他玩家看不到），用鼠标瞄准，然后点击鼠标右键进行射击。" +
                                                "\n如果没有射中，就会在自己的位置上会出现一个警告图像，如果射中了，就会在尸体上出现一个警告图像。";
                                            break;
                                        case "plumber":
                                            infoText = "Plumber: 内鬼阵营：可以建造管道的职业。" +
                                                "\n当所有额外的管道都建造完成后，等到会议结束后这些管道就会生效。";
                                            break;
                                        case "librarian":
                                            infoText = "Librarian: 内鬼阵营：可以阻止玩家在会议上发言。" +
                                                "\n所有人都可以知道谁在会议上被禁言。";
                                            break;


                                        // Rebel roles
                                        case "renegade":
                                            infoText = "Renegade: 叛乱者阵营：杀死所有人。" +
                                                "\n他可以招募一个玩家来帮助他击杀，两人都有内鬼的视野，可以使用管道，他们的名字以及管道将显示为绿色的。";
                                            break;
                                        case "bountyhunter":
                                            infoText = "Bounty Hunter: 叛乱者阵营：杀死他的目标。" +
                                                "\n随机一位玩家作为他的目标，如果目标被非自己杀死，他也会死亡。";
                                            break;
                                        case "trapper":
                                            infoText = "Trapper: 叛乱者阵营：用地雷炸死所有人。" +
                                                "\n可以设置陷阱，将触碰它的玩家困住，也可以放置地雷，炸死其他玩家。";
                                            break;
                                        case "yinyanger":
                                            infoText = "Yinyanger: 叛乱者阵营：杀死所有人，每次都要标记两个玩家。" +
                                                "\n需要用 '阴 '标记一名玩家，然后用· '阳 '标记另一名玩家，如果他们相遇，两个人都会死亡，无视任何护盾。" +
                                                "\n不可以标记两个相近的玩家，只能标记两个相近玩家中的其中一个。";
                                            break;
                                        case "challenger":
                                            infoText = "Challenger: 叛乱者阵营：与其他玩家进行石头剪刀布以决斗的方式杀死所有人。" +
                                                "\n选择一名玩家并在10秒后传送所有玩家到决斗竞技场。" +
                                                "\n如果其中一个没有选择攻击，另一个就会自动获胜。如果没有人选择攻击，他们都会死。";
                                            break;
                                        case "ninja":
                                            infoText = "Ninja: 叛乱者阵营：杀光所有人并制造双杀。" +
                                                "\n选择一个玩家传送到他的位置，然后杀死他，在此之后使用普通的击杀按钮进行双杀。";
                                            break;
                                        case "berserker":
                                            infoText = "Berserker: 叛乱者阵营：杀死所有人，并且无法停止杀戮。" +
                                                "\n在第一次击杀后，击杀冷却永远只有10秒，如果在一定时间内没有杀人，便会死亡。";
                                            break;
                                        case "yandere":
                                            infoText = "Yandere: 叛乱者阵营：跟踪目标，并杀死它赢得胜利。" +
                                                "\n如果目标被放逐或被杀死，那么必须杀死所有人才能获胜。";
                                            break;
                                        case "stranded":
                                            infoText = "Stranded: 叛乱者阵营：需要在地图周围的盒子中找到弹药并杀死3名玩家。" +
                                                "\n也可以在盒子中找到让自己隐形以及使用管道能力的物品。";
                                            break;
                                        case "monja":
                                            infoText = "Monja: 叛乱者阵营：找到所有的小修女，并把他们带到仪式现场。" +
                                                "\n当收集到所有小修女时，就会在60秒后变成修女并杀死所有人，否则就会死亡。" +
                                                "\n如果有小孩在游戏中，那么只要杀死除小孩以为的玩家即可胜利。";
                                            break;

                                        // Neutral roles
                                        case "joker":
                                            infoText = "Joker: 中立阵营：在会议中被放逐才能获胜。" +
                                                "\n可以使用破坏，但前提是必须活着。";
                                            break;
                                        case "rolethief":
                                            infoText = "Role Thief: 中立阵营：在交换身份前无法获胜。" +
                                                "\n可以窃取其他玩家的角色，但如果试图窃取内鬼或叛乱者的职业，他就会死亡。";
                                            break;
                                        case "pyromaniac":
                                            infoText = "Pyromaniac: 中立阵营：把所有人浇油并点燃才能获胜。" +
                                                "\n可以站在其他玩家旁边浇油，浇完所有人就可以获得点燃按钮。";
                                            break;
                                        case "treasurehunter":
                                            infoText = "Treasure Hunter: 中立阵营:寻找所有宝藏才能获胜。" +
                                                "\n使用按钮，每轮在地图上随机产生一个宝藏，找到所需数量后就赢了。";
                                            break;
                                        case "devourer":
                                            infoText = "Devourer: 中立阵营：吞噬足够的尸体即可胜利。" +
                                                "\n当玩家死亡时，会听到一种声音。";
                                            break;
                                        case "poisoner":
                                            infoText = "Poisoner: 中立阵营：感染所有人才能胜利。" +
                                                "\n可以选择一名玩家作为感染源，感染源旁边的玩家会增加他们的感染值。" +
                                                "\n一旦所有玩家达到100%感染指数，他便获胜。";
                                            break;
                                        case "puppeteer":
                                            infoText = "Puppeteer: 中立阵营：变身成其他玩家，变身状态下被击杀指定次数才能获胜。" +
                                                "\n可以从玩家身上选择一个样本，然后随时变形，直到会议召开，就会变回原形。" +
                                                "\n如果在变行状态时被杀，将获得1分，并在他开始变形的地方复活，在达到所需的分数后他将获胜。";
                                            break;
                                        case "exiler":
                                            infoText = "Exiler: 中立阵营：在会议中放逐目标玩家才能获胜。" +
                                                "\n开局将随机分配一个玩家作为目标，但如果目标非投票死亡，他也会死亡。";
                                            break;
                                        case "amnesiac":
                                            infoText = "Amnesiac: 中立阵营：报告尸体来获得死者的职业。" +
                                                "\n会议结束后会获得尸体职业。";
                                            break;
                                        case "seeker":
                                            infoText = "Seeker: 中立阵营：在捉迷藏小游戏中找到其他玩家。" +
                                                "\n在选择3个人后，就可以开始游戏，将所有玩家传送到一个新的区域。" +
                                                "\n在游戏中每找到一个玩家便获得1分，在达到所需点数后便获胜。";
                                            break;

                                        // Crewmate roles
                                        case "captain":
                                            infoText = "Captain: 船员阵营：拥有双倍的票数。" +
                                                "\n可以在每局游戏中把所有的票数转给另一个玩家，但如果把票数转给一个船员，就会被放逐。" +
                                                "\n场上只剩内鬼阵或营反叛者阵营时，可以召集紧急会议放逐剩下的玩家。";
                                            break;
                                        case "mechanic":
                                            infoText = "Mechanic: 船员阵营：可以随时随地修复一定次数破坏。" +
                                                "\n可以一个人修复所有的破坏。";
                                            break;
                                        case "sheriff":
                                            infoText = "Sheriff: 船员阵营：可以击杀其他玩家。" +
                                                "\n如果试图击杀船员，就会死亡。";
                                            break;
                                        case "detective":
                                            infoText = "Detective: 船员阵营：可以看到所有的玩家脚印。" +
                                                "\n脚印不会在管道内产生，只有侦探活着的时候才会生成。";
                                            break;
                                        case "forensic":
                                            infoText = "Forensic: 船员阵营：通过报告尸体和询问玩家的灵魂来获得线索。" +
                                                "\n通过报告尸体可以得到的线索是凶手的名字、颜色类型或关于他的外表的东西。" +
                                                "\n灵魂会在下一轮出现在尸体所在的地方，它们的线索有可能是杀手的颜色类型，或在报告尸体前经过了多长时间。";
                                            break;
                                        case "timetraveler":
                                            infoText = "Time Traveler: 船员阵营：每场比赛可以将时间倒退两次，使在倒退过程中被杀死的玩家复活。。" +
                                                "\n如果没有处于破坏状态下，他可以倒退时间。如果有人试图在他开启时间盾庇护的情况下杀死他，时间也会倒退。" +
                                                "\n该职业不能使用生命体征仪。";
                                            break;
                                        case "squire":
                                            infoText = "Squire: 船员阵营：可以给一个玩家戴上一个护盾。" +
                                                "\n直到他死亡前，试图杀死被保护的玩家将触发一个声音，Impostors，Rebels，Sheriff，Squire 和被庇护的玩家可以听见这个声音。";
                                            break;
                                        case "cheater":
                                            infoText = "Cheater: 船员阵营：可以调换两个玩家的票。" +
                                                "\n在交换选票后，如果这两个玩家中的一个是船员，就会被放逐。";
                                            break;
                                        case "fortuneteller":
                                            infoText = "Fortune Teller: 船员阵营：可以预言一个玩家是好是坏。" +
                                                "\n预言会触发一个声音，并使两个玩家的屏幕变成蓝色。" +
                                                "\n坏职业的名字变成红色，好职业的名字变成青色。";
                                            break;
                                        case "hacker":
                                            infoText = "Hacker: 船员阵营：可以在任何地方使用管理和生命体征，并从中获得更多信息。" +
                                                "\n当黑客能力被激活时，可以在管理地图上看到玩家的大致位置及颜色，以及从生命体征仪上查看玩家是否死亡。";
                                            break;
                                        case "sleuth":
                                            infoText = "Sleuth: 船员阵营：可以追踪尸体和一名玩家的位置。" +
                                                "\n蓝色箭头指向被追踪的玩家，绿色箭头指向尸体。";
                                            break;
                                        case "fink":
                                            infoText = "Fink: 船员阵营：完成任务后，就可以知道谁是内鬼。" +
                                                "\n可以扩大视野，当扩大视野时，不能移动。" +
                                                "\n当剩下几个任务或扩大视野时，会有箭头提示内鬼或叛乱者。";
                                            break;
                                        case "kid":
                                            infoText = "Kid: 船员阵营：不应能被杀死或放逐，否则大家都会损失。" +
                                                "\n他比其他玩家都小。";
                                            break;
                                        case "welder":
                                            infoText = "Welder: 船员阵营：可以堵住通风口。" +
                                                "\n这些管道在下次会议后就不可用了，不能进入或退出，但仍然可以停留在管道内。";
                                            break;
                                        case "spiritualist":
                                            infoText = "Spiritualist: 船员阵营：能以自己的生命救活另一个玩家。" +
                                                "\n他可以复活玩家，如果有人在他复活的时候召集会议，他就会死。" +
                                                "\n内鬼阵营和叛乱者阵营会看见一个粉色箭头指向复活的玩家。";
                                            break;
                                        case "vigilant":
                                            infoText = "Vigilant: 船员阵营：能在地图上多放四个摄像头。" +
                                                "\n摄像头在会议结束后可用，当放置所有摄像头时，就可以随时随地使用监控。" +
                                                "\n在米拉总部地图上，他可以远程检查门的情况。";
                                            break;
                                        case "hunter":
                                            infoText = "Hunter: 船员阵营：可以标记一名玩家，如果被杀死，被标记玩家也会死亡。" +
                                                "\n当被放逐时，并不会放逐被标记的玩家。";
                                            break;
                                        case "jinx":
                                            infoText = "Jinx: 船员阵营：可以使玩家技能失效。" +
                                                "\n在被诅咒时使用技能会失效并进入冷却。";
                                            break;
                                        case "coward":
                                            infoText = "Coward: 船员阵营：可以随时随地召集会议。" +
                                                "\n如果处于破坏状态下，就不能远程召集会议。";
                                            break;
                                        case "bat":
                                            infoText = "Bat: 船员阵营：可以通过技能使冷却时间发生改变。" +
                                                "\nCrewmates, Rebels 和 Neutrals 时间加快了2倍" +
                                                "\nImpostors 冷却时间增加一定的秒数";
                                            break;
                                        case "necromancer":
                                            infoText = "Necromancer: 船员阵营：可以拖放尸体并将其拖到特定房间复活。" +
                                                "\n房间由蓝色箭头指示。" +
                                                "\n内鬼阵营和叛乱者阵营会得到一个粉色箭头指向复活的玩家。";
                                            break;
                                        case "engineer":
                                            infoText = "Engineer: 船员阵营：可以放置加速，减速，位置踏板。" +
                                                "\n按F键切换放置类型。" +
                                                "\n在会议结束后生效并有5秒持续时间。";
                                            break;
                                        case "shy":
                                            infoText = "Shy: 船员阵营：箭头指向距离他最近的玩家。";
                                            break;
                                        case "taskmaster":
                                            infoText = "Task Master: 船员阵营：在做完任务后有额外的任务。" +
                                                "\n在临死之前做完额外任务将获得胜利。";
                                            break;
                                        case "jailer":
                                            infoText = "Jailer: 船员阵营：可以标记一位玩家进行保护。" +
                                                "\n如果有玩家试图击杀被庇护的玩家将被送进监狱一段时间成为狱卒，狱卒需标记另一位玩家。";
                                            break;

                                        // Modifiers:
                                        case "lovers":
                                            infoText = "Lovers: 附加职业：两个人恋爱了。" +
                                                "\n其中一个被击杀或驱逐，另一位也会为爱赴死。";
                                            break;
                                        case "lighter":
                                            infoText = "Lighter: 附加职业：拥有更大的视野。" +
                                                "\n且黑灯时也能看清。";
                                            break;
                                        case "blind":
                                            infoText = "Blind: 附加职业：视野降低。" +
                                                "\n但对内鬼阵营无效。";
                                            break;
                                        case "flash":
                                            infoText = "Flash: 附加职业：速度变快。" +
                                                "\n在和挑战者猜拳和匿名通讯中无效。";
                                            break;
                                        case "bigchungus":
                                            infoText = "Big Chungus: 附加职业：体型变大，走不动路。" +
                                                "\n在和挑战者猜拳和匿名通讯中无效。";
                                            break;
                                        case "thechosenone":
                                            infoText = "The Chosen One: 附加职业：强迫凶手报告。" +
                                                "\n可设置报告延迟。";
                                            break;
                                        case "performer":
                                            infoText = "Performer: 附加职业：当死亡时会有JOJO音乐并有箭头指向尸体位置。" +
                                                "\n可设置音乐时间。";
                                            break;
                                        case "pro":
                                            infoText = "Pro: 让玩家失去方向感反方向移动。" +
                                                "\n变成灵魂也一样。";
                                            break;
                                        case "paintball":
                                            infoText = "Paintball: 加职业：死亡时血液会溅到凶手身上，凶手会留下带有玩家颜色的痕迹，可设置持续时间。";
                                            break;
                                        case "electrician":
                                            infoText = "Electrician: 附加职业：药水溅到凶手身上，使凶手瘫痪。";
                                            break;

                                        // Gamemodes:
                                        case "capturetheflag":
                                        case "ctf":
                                            infoText = "Capture The Flag: 游戏模式：红队和蓝队之间的对决，双方必须偷取对方的旗帜，并将其带到自己的基地。" +
                                                "\n每个人都可以使用通风管道和击杀，但不能在拿旗帜时使用通风管道和击杀。" +
                                                "\n在总玩家数为单数时，游戏内会出现一个叫做 \"窃旗者 \"的特殊角色，如果他击杀一位拿着旗子的玩家时，他就会和对方交换队伍。";
                                            break;
                                        case "policeandthieves":
                                        case "pat":
                                            infoText = "Police and Thieves: 游戏模式：青色和棕色队伍之间追逐，警察必须抓住所有小偷，而小偷必须在不被抓住的情况下偷走地图上所有的珠宝。" +
                                                "\n这个游戏模式增加了一个新的地图房间，监狱。" +
                                                "\n每局游戏最多9名小偷和6名警察。" +
                                                "\n小偷可以使用通风管道，但在拿珠宝时不能使用通风管道。";
                                            break;
                                        case "kingofthehill":
                                        case "koth":
                                            infoText = "King of The Hill: 游戏模式：绿队和黄队之间对决。地图上有3个可占领的区域，每队都有一个国王可以占领它们。" +
                                                "\n除了国王，每个人都可以击杀和使用管道。" +
                                                "\n总玩家数为单数时，游戏会出现一个名为篡夺者的特殊角色，如果他杀了一个国王，他就会和国王交换队伍。";
                                            break;
                                        case "hotpotato":
                                        case "hp":
                                            infoText = "Hot Potato: 游戏模式：随机一位玩家获得了 '热山芋'，并且必须在规定时间结束前将 '热山芋' 交给另一位玩家，否则玩家将会死亡。" +
                                                "\n游戏中只能有一个烫手山芋，其他的人如果死了，就会变成冷山芋或烧焦的山芋。" +
                                                "\n这个游戏模式也可以作为捉迷藏。";
                                            break;
                                        case "zombielaboratory":
                                        case "zl":
                                            infoText = "Zombie Laboratory: 游戏模式：随机一位玩家成为丧尸，随机一位玩家成为护士，其余玩家则是幸存者。" +
                                                "\n僵尸必须感染幸存者才能获胜，而幸存者必须寻找藏在地图周围箱子里的关键物品，将它们送到新的地图区域--医务室，在那里护士可以做出治疗并赢得游戏。" +
                                                "\n只有僵尸可以使用通风管道，护士有血清不会被感染，可以找一个医疗箱来治疗被感染的幸存者。";
                                            break;
                                        case "battleroyale":
                                        case "br":
                                            infoText = "Battle Royale: 游戏模式：每位玩家有一个带有弓箭手机制的远程杀戮按钮（可通过鼠标右键使用），冷却时间非常短。" +
                                                "\n不可以使用通风管道，所有玩家都有生命值，当他们的生命值归零时，他们将死亡，活到最后的玩家获得胜利。" +
                                                "\n此外，这个游戏模式有3种玩法，所单人模式，团队战或分数战。" +
                                                "\n在团队战和分数战总玩家数量为单数时，游戏内会有一个强大的中立角色，称为 \"连环杀手\"，它也可以赢得杀死所有人或达到所需的分数。这个角色有3点生命值和一半的击杀冷却时间。";
                                            break;
                                    }
                                    break;
                            }
                            
                            handled = true;
                            __instance.AddChat(PlayerControl.LocalPlayer, infoText);
                            //PlayerControl.LocalPlayer.RpcSendChat(infoText);
                        }
                    }
                    if (MeetingHud.Instance != null && howmanygamemodesareon != 1) {
                        List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerControl.LocalPlayer);
                        RoleInfo roleInfo = infos.Where(info => !info.isModifier).FirstOrDefault();
                        if (text.ToLower().StartsWith("/myrole") || text.ToLower().StartsWith("/m")) {
                            switch (LasMonjasPlugin.modLanguage.Value) {
                                // English
                                case 1:
                                    if (roleInfo == null) {
                                        infoText = "You don't have a Role.";
                                    }
                                    else {
                                        switch (roleInfo.roleId.ToString().ToLower()) {
                                            // Impostor roles
                                            case "mimic":
                                                infoText = "Mimic: impostor who can mimic the appearance of other player.";
                                                break;
                                            case "painter":
                                                infoText = "Painter: impostor who can hide every player's outfit and paint them with a random color.";
                                                break;
                                            case "demon":
                                                infoText = "Demon: impostor who can bite a player to delay his death." +
                                                    "\nIf there's a Demon, all players will have a Nun button to place one Nun per game on the map." +
                                                    "\nStaying next to the Nun nullifies the bite.";
                                                break;
                                            case "janitor":
                                                infoText = "Janitor: impostor who can remove and move bodies." +
                                                    "\nHe can't remove and move bodies at the same time.";
                                                break;
                                            case "illusionist":
                                                infoText = "Illusionist: impostor who can make his own vent network and turn off the lights from anywhere." +
                                                    "\nHis 3-vents network can only be used by himself and becomes visible for everyone right after placing the third vent." +
                                                    "\nOnce the vent network is done, he gains the ability to turn off the lights.";
                                                break;
                                            case "manipulator":
                                                infoText = "Manipulator: impostor who can manipulate a player to kill his adjacent from anywhere." +
                                                    "\nCan kill anyone, himself included, with his ability.";
                                                break;
                                            case "bomberman":
                                                infoText = "Bomberman: impostor who can place a bomb on the map." +
                                                    "\nThe bomb can't be placed if a sabotage is active or a player is too close to him." +
                                                    "\nPlayers can defuse the bomb by touching it, impostors win if the bomb isn't defused.";
                                                break;
                                            case "chameleon":
                                                infoText = "Chameleon: impostor who can become invisible." +
                                                    "\nHe can't use vents. While invisible, he can be killed by roles who has a kill button.";
                                                break;
                                            case "gambler":
                                                infoText = "Gambler: impostor who can shoot a player choosing their role during the meeting." +
                                                    "\nHe has to guess the player's role to kill him, only the current ingame roles appears on his screen. Choosing the wrong one kills himself.";
                                                break;
                                            case "sorcerer":
                                                infoText = "Sorcerer: impostor who can cast spells on players." +
                                                    "\nSpelled players will have a purple pumpkin icon next to their names during the meeting and will die afterwards unless the Sorcerer is voted out.";
                                                break;
                                            case "medusa":
                                                infoText = "Medusa: impostor who can petrify other players." +
                                                    "\nA petrified player can't move for a set amount of time.";
                                                break;
                                            case "hypnotist":
                                                infoText = "Hypnotist: impostor who can place traps which inverts player's movement controls." +
                                                    "\nTraps become active after a meeting and don't affect impostors.";
                                                break;
                                            case "archer":
                                                infoText = "Archer: impostor who can make long distance kills but can't make normal ones." +
                                                    "\nHe needs to pick the bow (invisible to other players), aim with the mouse and right click to shoot." +
                                                    "\nA warning image appears on his position if he miss the shoot or above the player's body if he kills someone.";
                                                break;
                                            case "plumber":
                                                infoText = "Plumber: impostor who can create usable vents for any vent role." +
                                                    "\nVents become available after a meeting only when all extra vents had been placed.";
                                                break;
                                            case "librarian":
                                                infoText = "Librarian: impostor who can prevent a player from talking on a meeting." +
                                                    "\nEveryone knows who is silenced during the meeting.";
                                                break;


                                            // Rebel roles
                                            case "renegade":
                                                infoText = "Renegade: rebel who has to kill everyone." +
                                                    "\nHe can recruit a Minion to help him killing, both have impostor vision, can vent and their names will be green.";
                                                break;
                                            case "bountyhunter":
                                                infoText = "Bounty Hunter: rebel who has to kill a specific player." +
                                                    "\nHis target button assigns a random player's role to be his target but if the target is already dead, he also dies.";
                                                break;
                                            case "trapper":
                                                infoText = "Trapper: rebel who has to kill everyone with mines." +
                                                    "\nHe can put traps that root the player who touched it and mines that kill whoever steps on it.";
                                                break;
                                            case "yinyanger":
                                                infoText = "Yinyanger: rebel who has to kill everyone marking two players each time." +
                                                    "\nHe can mark a player with the Yin and another one with the Yang, if they collide both die ignoring any shields they could have." +
                                                    "\nAfter marking one player, he can't mark the other one if the marked one is too close to the target.";
                                                break;
                                            case "challenger":
                                                infoText = "Challenger: rebel who has to kill everyone with rock-paper-scissors duels." +
                                                    "\nSelecting a player will teleport all players to the duel arena after 10 seconds if no sabotage is active." +
                                                    "\nIf one of them doesn't select an attack, the other wins automatically. If noone select an attack both die. Nobody die on draw.";
                                                break;
                                            case "ninja":
                                                infoText = "Ninja: rebel who has to kill everyone making double kills." +
                                                    "\nSelect a player to teleport to his position afterwards killing him in the process. He can use the normal kill button right after to make a double kill.";
                                                break;
                                            case "berserker":
                                                infoText = "Berserker: rebel who has to kill everyone but can't stop killing." +
                                                    "\nAfter killing for first time, his kill button gets a permanent 10 seconds cooldown but he dies if he doesn't kill for a set amount of time.";
                                                break;
                                            case "yandere":
                                                infoText = "Yandere: rebel who has to stalk a target a few times and then kill it to win." +
                                                    "\nIf the target gets exiled or killed by another player, he enters rampage mode and have to kill everyone to win instead.";
                                                break;
                                            case "stranded":
                                                infoText = "Stranded: rebel who has to find ammo on boxes around the map and kill 3 players." +
                                                    "\nHe can also find a item to become invisible for a while and the vent ability.";
                                                break;
                                            case "monja":
                                                infoText = "Monja: rebel who has to find little monjas and bring them to the ritual spot." +
                                                    "\nOnce all the monjas are delivered, she can transform into Monja to kill everyone within 60 seconds, otherwise she'll die." +
                                                    "\nIf there's a Kid ingame, the Monja must kill everyone except the Kid to win.";
                                                break;

                                            // Neutral roles
                                            case "joker":
                                                infoText = "Joker: neutral who has to be voted out to win." +
                                                    "\nHe can sabotage but only if he's alive.";
                                                break;
                                            case "rolethief":
                                                infoText = "Role Thief: neutral with no win condition." +
                                                    "\nHe can steal the role of other players but if he tries to steal an impostor or rebel role, he dies.";
                                                break;
                                            case "pyromaniac":
                                                infoText = "Pyromaniac: neutral who has to ignite everyone to win." +
                                                    "\nHe can spray players by standing next to them, once he sprayed everyone he wins.";
                                                break;
                                            case "treasurehunter":
                                                infoText = "Treasure Hunter: neutral who has to look for treasures to win." +
                                                    "\nHis button spawns one treasure per round randomly on the map, after finding the needed amount he wins.";
                                                break;
                                            case "devourer":
                                                infoText = "Devourer: neutral who has to eat bodies to win." +
                                                    "\nHe hears a sound when a player dies.";
                                                break;
                                            case "poisoner":
                                                infoText = "Poisoner: neutral who has to poison everyone to win." +
                                                    "\nHe selects a player to be the poisoned one, players standing next to the poisoned increase their poison meter." +
                                                    "\nOnce every player reached 100% poison meter, he wins.";
                                                break;
                                            case "puppeteer":
                                                infoText = "Puppeteer: neutral who can morph into other players and has to get killed while morphed a few times to win." +
                                                    "\nHe can pick a sample from a player and morph into it all the time he wants or until a meeting is called, he gets killed or he decides to uncover himself." +
                                                    "\nIf he gets killed while morphed, he gains one point and revives on the spot where he started the morph, he wins after reaching the needed points.";
                                                break;
                                            case "exiler":
                                                infoText = "Exiler: neutral who has to vote out a specific player to win." +
                                                    "\nHis target button assigns a random player to be his target but if the target is already dead, he also dies.";
                                                break;
                                            case "amnesiac":
                                                infoText = "Amnesiac: neutral who remember his role reporting a body." +
                                                    "\nThe role resets after remember it.";
                                                break;
                                            case "seeker":
                                                infoText = "Seeker: neutral who has to find people on hide and seek minigame." +
                                                    "\nUpon selecting from 1 to 3 people, he can start the minigame teleporting all players to a new zone." +
                                                    "\nHe gains 1 point per player found on the minigame, after reaching the needed points he wins.";
                                                break;

                                            // Crewmate roles
                                            case "captain":
                                                infoText = "Captain: crewmate whose vote counts double." +
                                                    "\nHe can redirect all the votes to another player one time per game, but if he redirects them to a crewmate he gets exiled." +
                                                    "\nBeing alone with an impostor or rebel allows him to call an emergency meeting to exile the remaining player.";
                                                break;
                                            case "mechanic":
                                                infoText = "Mechanic: crewmate who can fix sabotages a certain number of times from anywhere." +
                                                    "\nHe also fixes the whole sabotage by doing only one of the emergency tasks without the needing of a second player.";
                                                break;
                                            case "sheriff":
                                                infoText = "Sheriff: crewmate who can kill players," +
                                                    "\nbut he dies if he tries to kill a crewmate.";
                                                break;
                                            case "detective":
                                                infoText = "Detective: crewmate who can see player's footprints." +
                                                    "\n Footprints don't spawn close no vents and only spawn if Detective is alive.";
                                                break;
                                            case "forensic":
                                                infoText = "Forensic: crewmate who gets clues by reporting bodies and asking player's ghosts." +
                                                    "\nThe clues that he can get by reporting a body are the killer's name, color type or something about his appearance." +
                                                    "\nGhosts appear on the next round where a body was and their clues are which role killed that player, killer's color type or how much time has passed before reporting the body.";
                                                break;
                                            case "timetraveler":
                                                infoText = "Time Traveler: crewmate who can rewind the time two times per game, reviving players that were killed during the rewind." +
                                                    "\nHe can rewind the time if there's no active sabotage, the time rewinds also if someone tries to kill him while he's shielded by time shield." +
                                                    "\nThis role can't use Vitals.";
                                                break;
                                            case "squire":
                                                infoText = "Squire: crewmate who can put a shield on a player." +
                                                    "\nThe shield last until he gets exiled or killed. Trying to kill the shielded player will trigger a sound heard by Impostors, Rebels, Sheriff, Squire and the shielded player.";
                                                break;
                                            case "cheater":
                                                infoText = "Cheater: crewmate who can swap the votes of two players." +
                                                    "\nAfter swapping votes, he gets exiled if one of those two players turned out to be a crewmate.";
                                                break;
                                            case "fortuneteller":
                                                infoText = "Fortune Teller: crewmate who can reveal if a player is good or bad." +
                                                    "\nRevealing triggers a sound and turns the screen blue for both players." +
                                                    "\nThe name turns red for bad roles and cyan for good ones.";
                                                break;
                                            case "hacker":
                                                infoText = "Hacker: crewmate who can use Admin and Vitals from anywhere and gets more information from them." +
                                                    "\nWhile his hack ability is active, he can see players color on Admin and how much time has passed since someone died on Vitals.";
                                                break;
                                            case "sleuth":
                                                infoText = "Sleuth: crewmate who can track bodies and one player's position." +
                                                    "\nHe sees a blue arrow pointing the tracked player and green ones pointing the bodies.";
                                                break;
                                            case "fink":
                                                infoText = "Fink: crewmate who reveals who the impostors are after finishing his tasks and can zoom out the camera." +
                                                    "\nHe can't move while the camera is zoomed out." +
                                                    "\nImpostors also know who the Fink is when a few tasks remains or when he's zooming out the camera.";
                                                break;
                                            case "kid":
                                                infoText = "Kid: crewmate who shouldn't be killed or exiled, otherwise everyone loss." +
                                                    "\nHe's smaller than the other players.";
                                                break;
                                            case "welder":
                                                infoText = "Welder: crewmate who can disable vents." +
                                                    "\nThose vents become unavailable after the next meeting and can't be entered or exited, but still can be used as a tunnel.";
                                                break;
                                            case "spiritualist":
                                                infoText = "Spiritualist: crewmate who can revive another player at the cost of his own life." +
                                                    "\nHe needs to stay next to a body to revive it, but if someone calls a meeting while he tries to revive, he dies instead." +
                                                    "\nImpostors and Rebels get a pink arrow pointing the revived player.";
                                                break;
                                            case "vigilant":
                                                infoText = "Vigilant: crewmate who can place four extra cameras on the map." +
                                                    "\nThe cameras become available after a meeting and when he place all the cameras he gets the ability to remote check cameras." +
                                                    "\nOn MiraHQ he can remote check doorlog instead.";
                                                break;
                                            case "hunter":
                                                infoText = "Hunter: crewmate who can mark another player who will die if he gets killed." +
                                                    "\nExiling him won't exile the marked player.";
                                                break;
                                            case "jinx":
                                                infoText = "Jinx: crewmate who can block other players buttons." +
                                                    "\nUsing a button while being jinxed makes that button to enter cooldown.";
                                                break;
                                            case "coward":
                                                infoText = "Coward: crewmate who can call meetings from anywhere." +
                                                    "\nHe can't call meetings if a sabotage is active.";
                                                break;
                                            case "bat":
                                                infoText = "Bat: crewmate who can emit a frequency that alters buttons cooldown." +
                                                    "\nCrewmates, Rebels and Neutrals buttons cooldown goes x2 faster." +
                                                    "\nImpostors buttons cooldown increase by 1 each second.";
                                                break;
                                            case "necromancer":
                                                infoText = "Necromancer: crewmate who can drag and drop bodies and revive them dragging them to a specific room." +
                                                    "\nThe specific room is pointed by a blue arrow." +
                                                    "\nImpostors and Rebels get a pink arrow pointing the revived player.";
                                                break;
                                            case "engineer":
                                                infoText = "Engineer: crewmate who can place increase or decrease speed and position traps." +
                                                    "\nHe can switch trap type with the F key." +
                                                    "\nTraps become active after a meeting and have a 5 seconds effect duration.";
                                                break;
                                            case "shy":
                                                infoText = "Shy: crewmate who can reveal the position of the closest player." +
                                                    "\nAn arrow reveals the direction of the closest player.";
                                                break;
                                            case "taskmaster":
                                                infoText = "Task Master: crewmate who has extra tasks after doing the initial ones." +
                                                    "\nCompleting the extra tasks before getting killed achieves a crewmate win.";
                                                break;
                                            case "jailer":
                                                infoText = "Jailer: crewmate who can mark a player to be his assistant." +
                                                    "\nTrying to kill the assistant denies the kill and teleports the killer to the jail for a few seconds, this only works one time and the Jailer needs to mark another player after this.";
                                                break;
                                        }
                                    }
                                    break;
                                // Spanish
                                case 2:
                                    if (roleInfo == null) {
                                        infoText = "No tienes un Rol.";
                                    }
                                    else {
                                        switch (roleInfo.roleId.ToString().ToLower()) {
                                            // Impostor roles
                                            case "mimic":
                                                infoText = "Mimic: impostor que puede copiar el aspecto de otro jugador durante un tiempo determinado.";
                                                break;
                                            case "painter":
                                                infoText = "Painter: impostor que hace que todos los jugadores se vean del mismo color sin cosmeticos.";
                                                break;
                                            case "demon":
                                                infoText = "Demon: impostor que muerde jugadores para demorar su muerte." +
                                                    "\nCuando hay un Demon, todos los jugadores tienen un boton para poner una Nun en el mapa que dura toda la partida." +
                                                    "\nEl area de la Nun te protege de la mordedura.";
                                                break;
                                            case "janitor":
                                                infoText = "Janitor: impostor que puede retirar y mover cuerpos." +
                                                    "\nNo puede usar ambas habilidades al mismo tiempo ni usar rejillas si esta moviendo un cuerpo.";
                                                break;
                                            case "illusionist":
                                                infoText = "Illusionist: impostor que puede crear su propia red de tres rejillas y apagar las luces a distancia." +
                                                    "\nEsa red de rejilla solo puede usarla el y se vuelve visible para todos al poner la tercera." +
                                                    "\nAl terminar la red de rejillas obtiene la habilidad de apagar las luces.";
                                                break;
                                            case "manipulator":
                                                infoText = "Manipulator: impostor que puede matar al jugador situado al lado del manipulado." +
                                                    "\nPuede matar a cualquiera, incluido a si mismo como a otros impostores con su habilidad.";
                                                break;
                                            case "bomberman":
                                                infoText = "Bomberman: impostor que puede poner una bomba en el mapa." +
                                                    "\nNo puede ponerla si hay un sabotaje activo o esta muy cerca de un jugador." +
                                                    "\nLos jugadores pueden desactivar la bomba tocandola, si no lo consiguen ganan los Impostores.";
                                                break;
                                            case "chameleon":
                                                infoText = "Chameleon: impostor que puede volverse invisible." +
                                                    "\nNo puede usar rejillas y pueden matarlo mientras esta en invisible.";
                                                break;
                                            case "gambler":
                                                infoText = "Gambler: impostor que puede disparar a jugadores en la reunion si adivina su rol." +
                                                    "\nSolo aparecen los roles presentes en esa partida para seleccionar, si se equivoca muere el mismo.";
                                                break;
                                            case "sorcerer":
                                                infoText = "Sorcerer: impostor que puede hechizar jugadores." +
                                                    "\nLos jugadores hechizados tendran una calabaza purpura al lado de su nombre en la reunion y seran expulsados de la nave si en esa ronda no se expulsa al Sorcerer.";
                                                break;
                                            case "medusa":
                                                infoText = "Medusa: impostor que puede petrificar jugadores." +
                                                    "\nEl jugador petrificado no podra hacer nada durante el tiempo de petrificacion.";
                                                break;
                                            case "hypnotist":
                                                infoText = "Hypnotist: impostor que puede poner trampas que invierten los controles de movimiento de los jugadors." +
                                                    "\nLas trampas duran toda la partida, se vuelven visibles y activas tras una reunion y no afectan a otros Impostores.";
                                                break;
                                            case "archer":
                                                infoText = "Archer: impostor que puede matar a distancia y a traves de las paredes, pero no puede matar de forma normal." +
                                                    "\nTiene que equiparse el arco (visible solo para el), apuntar con el raton y hacer clic derecho para disparar." +
                                                    "\nUn aviso aparecera en la posicion del Archer si falla el tiro, de lo contrario el aviso aparecera encima del cuerpo del jugador que ha muerto.";
                                                break;
                                            case "plumber":
                                                infoText = "Plumber: impostor que puede crear rejillas adicionales utilizables por cualquier rol que pueda usar rejillas." +
                                                    "\nLas rejillas se vuelven visibles y utilizables tras una reunion una vez el maximo de rejillas haya sido alcanzado.";
                                                break;
                                            case "librarian":
                                                infoText = "Librarian: impostor que puede silenciar a un jugador para evitar que hable durante la reunion." +
                                                    "\nTodos saben quien esta silenciado durante la reunion.";
                                                break;


                                            // Rebel roles
                                            case "renegade":
                                                infoText = "Renegade: rebelde que tiene que matar a todos para ganar." +
                                                    "\nPuede reclutar un Minion para que le ayude, ambos tiene vision de impostor y pueden usar rejillas.";
                                                break;
                                            case "bountyhunter":
                                                infoText = "Bounty Hunter: rebelde que tiene que matar a un jugador especifico para ganar." +
                                                    "\nEse jugador estara definido por su rol, pero si ya esta muerto entonces el Bounty Hunter." +
                                                    "\nSi el objetivo es expulsado, el Bounty Hunter tambien, si el objetivo se desconecta el Bounty Hunter muere.";
                                                break;
                                            case "trapper":
                                                infoText = "Trapper: rebelde que tiene que matar a todos poniendo trampas." +
                                                    "\nPuede poner minas que mataran a cualquier que las pise y cepos que inmovilizaran a quien lo pise.";
                                                break;
                                            case "yinyanger":
                                                infoText = "Yinyanger: rebelde que tiene que matar a todos marcando a dos jugadores de cada vez." +
                                                    "\nSi esos jugadores chocan entre ellos, mueren ignorando cualquier tipo de escudo que pudieran tener." +
                                                    "\nSi uno de esos jugadores muere por otro jugador, podra selecionar otro objetivo en la misma ronda." +
                                                    "\nNo puede seleccionar jugadores que esten cerca del ya marcado.";
                                                break;
                                            case "challenger":
                                                infoText = "Challenger: rebelde que tiene que matar a todos mediante duelos de piedra, papel y tijeras." +
                                                    "\nTras seleccionar un jugador, seran teletransportados a la arena del duelo pasados 10 segundos si no hay un sabotaje activo o el Challenger o su objetivo murieron en ese periodo." +
                                                    "\nSi uno de ellos no escoge ataque, el otro gana automaticamente. Si nadie escoge ataque mueren ambos, pero nadie muere en caso de empate.";
                                                break;
                                            case "ninja":
                                                infoText = "Ninja: rebelde que tiene que matar a todos haciendo muertes dobles." +
                                                    "\nPuede seleccionar a un jugador para posteriormente teletranportarse a el matandolo en el acto, con la posibilidad de usar su boton de matar normal acto seguido para realizar una muerte doble.";
                                                break;
                                            case "berserker":
                                                infoText = "Berserker: rebelde que tiene que matar a todos pero se muere si no mata durante cierto tiempo." +
                                                    "\nUna vez mata por primera vez, su boton de matar obtiene un tiempo de recarga de 10 segundos pero el contador de tiempo limite empieza a bajar, si no mata a nadie antes de que ese contador llegue a 0, se muere el.";
                                                break;
                                            case "yandere":
                                                infoText = "Yandere: rebelde que tiene que acechar varias veces a un jugador determinado y luego matarlo para ganar." +
                                                    "\nSi ese jugador es expulsado o lo matan, la Yandere entra en modo furia, donde tendra que matar a todos para ganar.";
                                                break;
                                            case "stranded":
                                                infoText = "Stranded: rebelde que tiene que encontrar municion en las cajas y matar a tres jugadores." +
                                                    "\nPuede encontrar la habilidad de usar rejillas y de volverse invisible una vez por partida.";
                                                break;
                                            case "monja":
                                                infoText = "Monja: rebelde que tiene que reunir los objetos para realizar el ritual y luego matar a todos." +
                                                    "\nCuando reune todos los objetos, se puede transformar en la Monja para matar a todos antes de 60 segundos, sino se muere." +
                                                    "\nNo puede matar al niño y durante la transformacion los jugadores solo pueden hacer tareas y huir.";
                                                break;

                                            // Neutral roles
                                            case "joker":
                                                infoText = "Joker: neutral que tiene que ser expulsado de la nave para ganar." +
                                                    "\nPuede sabotear pero solo mientras esta vivo.";
                                                break;
                                            case "rolethief":
                                                infoText = "Role Thief: neutral que no tiene condicion de victoria." +
                                                    "\nPuede robar el rol de otro jugador pero si intenta robarlo a un Impostor o Rebelde se muere.";
                                                break;
                                            case "pyromaniac":
                                                infoText = "Pyromaniac: neutral que tiene que quemar a todos para ganar." +
                                                    "\nPuede rociar a los jugadores poniendose a su lado, cuando rocia a todos gana.";
                                                break;
                                            case "treasurehunter":
                                                infoText = "Treasure Hunter: neutral que tiene que encontrar tesoros para ganar." +
                                                    "\nSu boton invoca un cofre en un sitio aleatorio del mapa, puede invocar otro tras encontrarlo.";
                                                break;
                                            case "devourer":
                                                infoText = "Devourer: neutral que tiene que devorar cuerpos para ganar." +
                                                    "\nCuando muere alguien escucha un sonido.";
                                                break;
                                            case "poisoner":
                                                infoText = "Poisoner: neutral que tiene que envenenar a todos para ganar." +
                                                    "\nSelecciona a un jugador para ser el envenenado y los jugadores cercanos a el se iran envenenando poco a poco." +
                                                    "\nAl llegar a 100% de veneno, ese jugador contara para envenenar a los demas y una vez todos esten al 100% ganara.";
                                                break;
                                            case "puppeteer":
                                                infoText = "Puppeteer: neutral que puede transformarse en otro jugador a modo de marioneta y deben matarlo varias veces para ganar." +
                                                    "\nLa transformacion dura lo que quiera el jugador hasta que se haga una reunion o lo maten." +
                                                    "\nSi lo matan transformado, gana un punto y vuelve al lugar donde se transformo inicialmente.";
                                                break;
                                            case "exiler":
                                                infoText = "Exiler: neutral que tiene que expulsar de la nave a un jugador especifico." +
                                                    "\nSi su objetivo muere o ya estaba muerto al seleccionarlo, tambien muere, pero si su objetivo se desconecta entonces gana automaticamente.";
                                                break;
                                            case "amnesiac":
                                                infoText = "Amnesiac: neutral que puede obtener un rol reportando un cuerpo." +
                                                    "\nEse rol se reinicia al obtenerlo, en caso de ser un rol no obtenible saldra un aviso en el chat solo para el.";
                                                break;
                                            case "seeker":
                                                infoText = "Seeker: neutral que tiene que ganar puntos jugando al escondite." +
                                                    "\nTras seleccionar de uno a tres jugadores, puede empezar el minijuego que teletransportara a todos los jugadores a una nueva zona." +
                                                    "\nGana un punto por jugador encontrado en el minijuego.";
                                                break;

                                            // Crewmate roles
                                            case "captain":
                                                infoText = "Captain: tripulante cuyo voto cuenta doble." +
                                                    "\nPuede forzar la votacion una vez por partida para que todos los votos vayan a un jugador concreto, pero si ese jugador es inocente, entonces el Captain tambien es expulsado." +
                                                    "\nSi se queda solo contra un Impostor o Rebelde, puede convocar una reunion a distancia cuando quiera para echar al jugador restante.";
                                                break;
                                            case "mechanic":
                                                infoText = "Mechanic: tripulante que puede reparar sabotajes a distancia." +
                                                    "\nPuede reparar los sabotajes dobles el solo.";
                                                break;
                                            case "sheriff":
                                                infoText = "Sheriff: tripulante que puede matar a otros jugadores." +
                                                    "\nSi intenta matar a un tripulante se muere.";
                                                break;
                                            case "detective":
                                                infoText = "Detective: tripulante que puede ver las huellas de los jugadores." +
                                                    "\nLas huellas no aparecen cerca de las rejillas y solo aparecen mientras el Detective esta vivo.";
                                                break;
                                            case "forensic":
                                                infoText = "Forensic: tripulante que puede obtener pistas reportando cuerpos y hablando con fantasmas." +
                                                    "\nLas pistas al reportar cuerpos pueden ser el nombre del asesino, su tono de color o algo sobre su apariencia." +
                                                    "\nLos fantasmas de los jugadores que murieron aparecen tras la siguiente ruenion y sus pistas pueden ser que rol tiene el asesino, su tono de color y cuanto tiempo paso hasta que reportaron el cuerpo.";
                                                break;
                                            case "timetraveler":
                                                infoText = "Time Traveler: tripulante que puede rebobinar el tiempo dos veces por partida, reviviendo a los jugadores que murieron durante ese tiempo." +
                                                    "\nSolo puede rebobinar el tiempo si no hay un sabotaje activo o si lo intenta matar mientras tiene el escudo temporal activado." +
                                                    "\nNo puede usar Vitales.";
                                                break;
                                            case "squire":
                                                infoText = "Squire: tripulante que puede proteger a otro jugador con un escudo." +
                                                    "\nEl escudo dura hasta que el Squire es expulsado, asesinado o se convoca una reunion. Si intentas matar al escudado los Impostores, Rebeles, Sheriff, Squire y el escudado escucharan un sonido.";
                                                break;
                                            case "cheater":
                                                infoText = "Cheater: tripulante que puede intercambiar los votos de dos jugadores." +
                                                    "\nSi cambia los votos y el expulsado era inocente, el Cheater tambien es expulsado.";
                                                break;
                                            case "fortuneteller":
                                                infoText = "Fortune Teller: tripulante que puede revelear quien es bueno o malo." +
                                                    "\nTras revelar a un jugador se escuchara un sonido y la pantalla se volvera azul para ambos jugadores dependiendo de la opcion de a quien notificar que ha sido revelado." +
                                                    "\nEl nombre de los jugadores revelados se volvera rojo si es malo y azul si es bueno.";
                                                break;
                                            case "hacker":
                                                infoText = "Hacker: tripulante que puede usar Admin y Vitales a distancia y obtener mas informacion que los demas." +
                                                    "\nPuede ver el color de los jugadores en Admin y cuando tiempo lleva muerto un jugador en Vitales.";
                                                break;
                                            case "sleuth":
                                                infoText = "Sleuth: tripulante que puede rastrear a un jugador y cuerpos." +
                                                    "\nLa flecha rastreadora de jugadores es azul mientras que la de cuerpo es verde.";
                                                break;
                                            case "fink":
                                                infoText = "Fink: tripulante que puede alejar la camara del mapa y revelar quienes son los Impostores si finaliza todas sus tareas antes de morir." +
                                                    "\nNo puede moverse cuando aleja la camara." +
                                                    "\nLos Impostores sabran quien es el Fink cuando le quede cierto numero de tareas por hacer y tendran un aviso si esta usando su habilidad de alejar la camara.";
                                                break;
                                            case "kid":
                                                infoText = "Kid: tripulante que no debe ser expulsado ni matado, de lo contrario todos perderan." +
                                                    "\nEs mas pequeño que otros jugadores.";
                                                break;
                                            case "welder":
                                                infoText = "Welder: tripulante que puede sellar rejillas." +
                                                    "\nEl sellado se produce tras una reunion y provoca que esa rejilla no pueda ser usada para entrar ni salir, pero si es posible usarla de tunel.";
                                                break;
                                            case "spiritualist":
                                                infoText = "Spiritualist: tripulante que puede revivir a otro jugador a costa de su propia vida." +
                                                    "\nDebe permanecer un tiempo cerca del cuerpo para revivirlo, pero si alguien convoca una reunion mientras intenta revivirlo, se muere." +
                                                    "\nLos Impostores y Rebeles obtiene una flecha rosa apuntando al jugador revivido.";
                                                break;
                                            case "vigilant":
                                                infoText = "Vigilant: tripulante que puede poner camaras adicionales en el mapa." +
                                                    "\nLas camaras se vuelven utilizables tras una reunion y cuando se ha alcanzado el maximo numero de camaras obtiene la habilidad de ver las camaras a distancia basada en usos." +
                                                    "\nEn el mapa MiraHQ puede usar el DoorLog a distancia en vez de poner camaras.";
                                                break;
                                            case "hunter":
                                                infoText = "Hunter: tripulante que puede marcar a un jugador para que muera cuando se muere el." +
                                                    "\nSi es expulsado de la nave, al jugador marcado no le pasa nada.";
                                                break;
                                            case "jinx":
                                                infoText = "Jinx: tripulante que puede gafar las habilidades de los demas." +
                                                    "\nSi estas gafado, ese jugador escuchara un sonido y su boton entrara en tiempo de recarga.";
                                                break;
                                            case "coward":
                                                infoText = "Coward: tripulante que puede convocar reuniones a distancia pero no puede hacerlo si hay un sabotaje activo.";
                                                break;
                                            case "bat":
                                                infoText = "Bat: tripulante que puede emitir una frecuencia que alterara los tiempos de recarga de los botones." +
                                                    "\nLos Tripulantes, Rebeldes y Neutrales cercanos a el veran sus tiempos de recarga acelerados el doble de rapido." +
                                                    "\nLos Impostores en su lugar veran el tiempo de recarga en aumento.";
                                                break;
                                            case "necromancer":
                                                infoText = "Necromancer: tripulante que puede mover cuerpos y revivirlos tras llevarlos a una habitacion concreta pero aleatoria." +
                                                    "\nLa habitacion esta marcada por una flecha azul." +
                                                    "\nLos Impostores y Rebeles obtiene una flecha verde apuntando al jugador revivido.";
                                                break;
                                            case "engineer":
                                                infoText = "Engineer: tripulante que puede poner trampas que alteran la velocidad de desplazamiento y detectan la posicion de jugadores." +
                                                    "\nPuede cambiar el tipo de trampa a colocar con la F." +
                                                    "\nLas trampas se vuelven activas tras una reunion y su efecto dura 5 segundos.";
                                                break;
                                            case "shy":
                                                infoText = "Shy: tripulante que puede revelar si hay jugadores cercanos a el." +
                                                    "\nUna flecha apuntara al jugador mas cercado a el.";
                                                break;
                                            case "taskmaster":
                                                infoText = "Task Master: tripulante que tiene tareas extra para hacer al completar las iniciales." +
                                                    "\nSi completa las extra sin morir, obtiene una victoria para los Tripulantes.";
                                                break;
                                            case "jailer":
                                                infoText = "Jailer: tripulante que puede marcar a un jugador para que sea su asistente." +
                                                    "\nSi intentas matar al asistente, seras teletransportado a la prision durante un tiempo, esto solo funciona una vez y el Jailer debera seleccionar a otro jugador.";
                                                break;
                                        }
                                    }
                                    break;
                                // Japanese
                                case 3:
                                    if (roleInfo == null) {
                                        infoText = "あなたには役割がありません。";
                                    }
                                    else {
                                        switch (roleInfo.roleId.ToString().ToLower()) {
                                            // Impostor roles
                                            case "mimic":
                                                infoText = "Mimic: 他のプレイヤーになりすますことができるインポスター。";
                                                break;
                                            case "painter":
                                                infoText = "Painter: 他のプレイヤーをランダムな色で装飾できるインポスター。";
                                                break;
                                            case "demon":
                                                infoText = "Demon: 噛みつくことでプレイヤーを時間をかけて殺せるインポスター。" +
                                                    "\nもし悪魔が存在する場合、1ゲーム、1マップごとに修道女を配置するボタンがあります。" +
                                                    "\n修道女のとなりに滞在することで、噛みつきを無効にできます。";
                                                break;
                                            case "janitor":
                                                infoText = "Janitor: 死体を取り除いたり、移動できるインポスター。" +
                                                    "\n同時に死体を取り除き、移動することはできません。";
                                                break;
                                            case "illusionist":
                                                infoText = "Illusionist: 専用のベントネットワークを構築し、どこからでも照明をおとせるインポスター。" +
                                                    "\n3つのベントネットワークは自分でしか使用できず、3番目のベントを配置した直後に。" +
                                                    "\nすべての人にみえるようになります。ベントネットワーク構築後、照明をオフにすることができます。";
                                                break;
                                            case "manipulator":
                                                infoText = "Manipulator: 隣接するプレイヤーを殺せるインポスター。" +
                                                    "\nの能力は自身を含む誰でも殺害可能。";
                                                break;
                                            case "bomberman":
                                                infoText = "Bomberman: 爆弾をマップに配置できるインポスター。" +
                                                    "\n妨害が実行されている場合、またはプレイヤーがインポスターの近くにいるときは爆弾は配置できません。" +
                                                    "\nプレイヤーは爆弾に触れることで、信管を取り除くことができます。爆弾の信管が取り除かれない場合、インポスターが勝利します。";
                                                break;
                                            case "chameleon":
                                                infoText = "Chameleon: 姿を消せるインポスター。" +
                                                    "\nベントを使用することはできません。姿はみえない状態でも、killボタンを持っている他の役職により殺される可能性はあります。";
                                                break;
                                            case "gambler":
                                                infoText = "Gambler: 会議中に選択した役割のプレイヤーを撃つことができるインポスター。" +
                                                    "\nプレイヤーを撃つには役割を推測して充てる必要があります。もし、間違った場合、自分自身を撃つことになります。";
                                                break;
                                            case "sorcerer":
                                                infoText = "Sorcerer: 他のプレイヤーに呪文を唱えることができるインポスター。" +
                                                    "\n呪文を唱えられたプレイヤーはミーティング中、名前の横に紫のかぼちゃのアイコンが表示され、魔術師が投票されない場合、ミーティング後に死亡します。";
                                                break;
                                            case "medusa":
                                                infoText = "Medusa: 他のプレイヤーを石化できるインポスター。" +
                                                    "\n石化したプレイヤーは一定の間、移動できなくなります。";
                                                break;
                                            case "hypnotist":
                                                infoText = "Hypnotist: プレイヤーの動きの制御を反対にさせる罠を配置できるインポスター。" +
                                                    "\n罠は会議の後に有効になり、インポスターには影響を与えません。";
                                                break;
                                            case "archer":
                                                infoText = "Archer: 長距離kill ができるインポスター。ただし、通常のkill はできません。" +
                                                    "\n他のプレイヤーからは見えない弓を使用して、マウスで標準をあわせ、右クリックにて射ぬく必要があります。" +
                                                    "\n標的を逃した場合、インポスターの位置に警告が表示され、標的を殺した場合は、標的の上に警告が表示されます。";
                                                break;
                                            case "plumber":
                                                infoText = "Plumber: ベント可能な役割のために、ベントを作ることができるインポスター。" +
                                                    "\nベントはミーティング後にすべての追加ベントが配置された場合に有効になります。";
                                                break;
                                            case "librarian":
                                                infoText = "Librarian: プレイヤーが会議で話すのを妨げることができるインポスター。" +
                                                    "\n誰もが会議中に誰が妨げられているかを知ることができます。";
                                                break;


                                            // Rebel roles
                                            case "renegade":
                                                infoText = "Renegade: みんなを殺さなければいけない反乱軍" +
                                                    "\n彼はミニオンを募集して殺害を助けることができます。";
                                                break;
                                            case "bountyhunter":
                                                infoText = "Bounty Hunter: 特定のプレイヤーを殺害しなければいけない反乱軍。" +
                                                    "\nターゲットボタンはランダムにターゲットにすべきプレイヤーの役割を設定しますが、\r\nもし、そのターゲットが既に死んでいる場合はその物も死ぬことになります。";
                                                break;
                                            case "trapper":
                                                infoText = "Trapper: 地雷で全員を殺害する反乱者。" +
                                                    "\n触れたプレイヤーを根こそぎ殺す罠や、踏んだ者を殺す地雷を設置することができます。";
                                                break;
                                            case "yinyanger":
                                                infoText = "Yinyanger: 毎回2人のプレイヤーをマークする全員を殺さなければならない反乱者。" +
                                                    "\n彼は、彼らが持っている可能性のあるシールドを無視して両方を衝突させるならば、彼は陰でプレーヤーとヤンと一緒に別のプレイヤーをマークすることができます。" +
                                                    "\n1人のプレーヤーをマークした後、マークされた1つがターゲットに近すぎる場合、もう1つのプレーヤーをマークすることはできません。";
                                                break;
                                            case "challenger":
                                                infoText = "Challenger: じゃんけんデュエルで全員を殺さねばならない反乱軍。" +
                                                    "\n妨害は実行されていない場合、10秒後に選ばれたプレイヤーがデュエルアリーナへテレポートします。" +
                                                    "\n誰も攻撃を選ばなかった場合、自動的に他の人が勝利します。攻撃を選択しない場合は両方とも死にます。引き分けでは誰も死にません。";
                                                break;
                                            case "ninja":
                                                infoText = "Ninja: 誰もがダブルキルをしている人を殺さなければならない反逆者。" +
                                                    "\nプレーヤーを選択して、その後、その過程で彼を殺した後、自分の立場にテレポートします。彼はすぐに通常のキルボタンを使用してダブルキルを行うことができます。";
                                                break;
                                            case "berserker":
                                                infoText = "Berserker:全員を殺さなければならないが、殺すのを止めることはできない反乱者。" +
                                                    "\n初めて殺した後、彼のキルボタンは永久に10秒のクールダウンを受け取りますが、彼が一定の時間を殺さないと彼は死にます。";
                                                break;
                                            case "yandere":
                                                infoText = "Yandere: ターゲットを数回ストーキングし、それを殺して勝つ必要がある反乱軍。" +
                                                    "\nターゲットが別のプレーヤーに追放または殺された場合、彼は大暴れモードに入り、代わりに勝つために全員を殺さなければなりません。";
                                                break;
                                            case "stranded":
                                                infoText = "Stranded: 地図の周りの箱で弾薬を見つけて3人のプレイヤーを殺さなければならない反逆者。" +
                                                    "\n彼はまた、しばらく目に見えないアイテムとベント能力を見つけることができます。";
                                                break;
                                            case "monja":
                                                infoText = "Monja: 小さなモンジャを見つけて儀式の場所に連れて行かなければならない反乱者。" +
                                                    "\nすべてのモンジャが配達されると、彼女は60秒以内にすべての人を殺すためにモンジャに変身することができます。そうでなければ彼女は死ぬでしょう。" +
                                                    "\n子供が登場した場合、モンジャは子供を除くすべての人を殺して勝つために殺さなければなりません。";
                                                break;

                                            // Neutral roles
                                            case "joker":
                                                infoText = "Joker: 勝つために投票されなければならないニュートラル。" +
                                                    "\n彼は生きている場合にのみ妨害することができます。";
                                                break;
                                            case "rolethief":
                                                infoText = "Role Thief: 勝利状態のないニュートラル。" +
                                                    "\n彼は他のプレイヤーの役割を盗むことができますが、彼が詐欺師や反逆者の役割を盗もうとすると死ぬ。";
                                                break;
                                            case "pyromaniac":
                                                infoText = "Pyromaniac: 勝つために全員を点火しなければならないニュートラル。" +
                                                    "\n彼は彼らの隣に立って、彼が勝ったすべての人をスプレーしたら、プレーヤーをスプレーすることができます。";
                                                break;
                                            case "treasurehunter":
                                                infoText = "Treasure Hunter: 勝つために宝物を探さなければならないニュートラル。" +
                                                    "\n彼のボタンは、必要な量を見つけた後、マップ上でラウンドごとに1つの宝物をランダムに発生させます。";
                                                break;
                                            case "devourer":
                                                infoText = "Devourer: 勝つために体を食べなければならない中立。" +
                                                    "\nプレーヤーが死ぬと彼は音を聞きます。";
                                                break;
                                            case "poisoner":
                                                infoText = "Poisoner: 勝つために皆を毒しなければならないニュートラル。" +
                                                    "\n彼は毒されたプレーヤーを選択し、毒されたプレーヤーは毒の隣に立って毒計を増やします。" +
                                                    "\nすべてのプレイヤーが100％の毒メーターに達すると、彼は勝ちます。";
                                                break;
                                            case "puppeteer":
                                                infoText = "Puppeteer: 他のプレイヤーにモーフィングできるニュートラルで、勝つために数回変化している間に殺さなければなりません。" +
                                                    "\n彼はプレイヤーからサンプルを選んで、望んでいる間、または会議が呼ばれるまで、彼は殺されるか、自分自身を明らかにすることにしたことに決めます。" +
                                                    "\nモーフィング中に殺された場合、彼は1つのポイントを獲得し、モーフを始めた場所で復活し、必要なポイントに到達した後に勝ちます。";
                                                break;
                                            case "exiler":
                                                infoText = "Exiler: 勝つために特定のプレーヤーに投票しなければならないニュートラル。" +
                                                    "\n彼のターゲットボタンは、ランダムなプレーヤーを自分のターゲットに割り当てますが、ターゲットがすでに死んでいる場合、彼は死にます。";
                                                break;
                                            case "amnesiac":
                                                infoText = "Amnesiac: 身体を報告している彼の役割を覚えているニュートラル。" +
                                                    "\nロールはそれを覚えてからリセットします。";
                                                break;
                                            case "seeker":
                                                infoText = "Seeker: 中立的な人は、hide＆seek minigameで人々を見つけなければなりません。" +
                                                    "\n3人を選択すると、彼はすべてのプレーヤーを新しいゾーンにテレポートするミニゲームを開始できます。" +
                                                    "\n彼は、彼が勝つ必要なポイントに達した後、ミニゲームで見つかったプレーヤーごとに1ポイントを獲得します。";
                                                break;

                                            // Crewmate roles
                                            case "captain":
                                                infoText = "Captain: 投票が2倍になる乗組員。" +
                                                    "\n彼はすべての票をゲームごとに1回別のプレーヤーにリダイレクトできますが、彼がそれらを乗組員にリダイレクトすると追放されます。" +
                                                    "\n詐欺師または反逆者と一人でいることで、彼は残りのプレーヤーを亡命させるために緊急会議に電話することができます。";
                                                break;
                                            case "mechanic":
                                                infoText = "Mechanic: どこからでも一定数を妨害することができる乗組員。" +
                                                    "\n彼はまた、2番目のプレーヤーを必要とせずに緊急タスクの1つだけを行うことにより、妨害行為全体を修正します。";
                                                break;
                                            case "sheriff":
                                                infoText = "Sheriff: 選手を殺すことができるクルーメイト。" +
                                                    "\nしかし、彼が乗組員を殺そうとすると彼は死にます。";
                                                break;
                                            case "detective":
                                                infoText = "Detective: プレイヤーの足跡を見ることができるクルーメイト。" +
                                                    "\nフットプリントはベントを閉じないで、探偵が生きている場合にのみスポーンしません。";
                                                break;
                                            case "forensic":
                                                infoText = "Forensic: 身体を報告し、プレイヤーの幽霊に尋ねることで手がかりを得る乗組員。" +
                                                    "\n身体を報告することで彼が得ることができる手がかりは、キラーの名前、色の種類、または彼の外観に関する何かです。" +
                                                    "\nゴーストは次のラウンドに登場し、身体が存在し、その手がかりがそのプレーヤー、キラーの色のタイプ、または体を報告する前にどれだけの時間が経過したかを殺しました。";
                                                break;
                                            case "timetraveler":
                                                infoText = "Time Traveler: ゲームごとに2回時間を巻き戻すことができ、巻き戻し中に殺されたプレイヤーを復活させることができるクルーメイト。" +
                                                    "\n彼は、積極的な妨害行為がなければ時間を巻き戻すことができます。時間は、彼がタイムシールドで保護されている間に誰かが彼を殺そうとする場合にも巻き戻します。" +
                                                    "\nこの役割はバイタルを使用できません。";
                                                break;
                                            case "squire":
                                                infoText = "Squire: プレーヤーにシールドを置くことができるクルーメイト。" +
                                                    "\n彼が追放されたり殺されたりするまで、盾は続きます。シールドプレーヤーを殺そうとすると、詐欺師、反乱軍、保安官、スクワイア、シールドプレイヤーが聞いた音が引き起こされます。";
                                                break;
                                            case "cheater":
                                                infoText = "Cheater: 2人のプレーヤーの票を交換できるクルーメイト。" +
                                                    "\n投票を交換した後、これら2人のプレーヤーのうちの1人が乗組員で​​あることが判明した場合、彼は追放されます。";
                                                break;
                                            case "fortuneteller":
                                                infoText = "Fortune Teller: プレーヤーが良いか悪いかを明らかにできるクルーメイト。" +
                                                    "\n明らかにすると、音がトリガーされ、両方のプレイヤーの画面が青くなります。" +
                                                    "\n名前は悪い役割では赤くなり、シアンは良い役割については赤くなります。";
                                                break;
                                            case "hacker":
                                                infoText = "Hacker: どこからでも管理者とバイタルを使用し、彼らからより多くの情報を得ることができる乗組員。" +
                                                    "\n彼のハック能力はアクティブですが、彼は誰かがバイタルで亡くなって以来、管理者が管理者の色の色とどれくらいの時間が経過したかを見ることができます。";
                                                break;
                                            case "sleuth":
                                                infoText = "Sleuth: 身体と1人のプレイヤーの位置を追跡できるクルーメイト。" +
                                                    "\n彼は、追跡されたプレーヤーと緑のプレーヤーを指している青い矢印が体を指しているのを見ます。";
                                                break;
                                            case "fink":
                                                infoText = "Fink: 詐欺師がタスクを終えた後に誰であるかを明らかにし、カメラをズームアウトできる乗組員。" +
                                                    "\nカメラがズームアウトされている間、彼は動くことができません。" +
                                                    "\n詐欺師はまた、いくつかのタスクが残っているとき、またはカメラをズームアウトしているときにフィンクが誰であるかを知っています。";
                                                break;
                                            case "kid":
                                                infoText = "Kid: 殺されたり追放されるべきではない乗組員、さもなければ全員が損失します。" +
                                                    "\n彼は他のプレイヤーよりも小さいです。";
                                                break;
                                            case "welder":
                                                infoText = "Welder: 通気口を無効にできる乗組員。" +
                                                    "\nこれらの通気孔は次の会議の後に利用できなくなり、入場または退出することはできませんが、それでもトンネルとして使用できます。";
                                                break;
                                            case "spiritualist":
                                                infoText = "Spiritualist: 自分の人生を犠牲にして別のプレーヤーを復活させることができる乗組員。" +
                                                    "\n彼はそれを復活させるために体の隣にとどまる必要がありますが、誰かが復活しようとする間に会議に電話した場合、代わりに死にます。" +
                                                    "\n詐欺師と反乱軍は、復活したプレーヤーを指すピンクの矢を手に入れます。";
                                                break;
                                            case "vigilant":
                                                infoText = "Vigilant: 地図上に4つの余分なカメラを配置できる乗組員。" +
                                                    "\nカメラは会議の後に利用可能になり、彼がすべてのカメラを配置すると、カメラをリモートすることができます。" +
                                                    "\nMiraHQでは、代わりにドアログをリモートすることができます。";
                                                break;
                                            case "hunter":
                                                infoText = "Hunter: 彼が殺された場合に死ぬ別のプレーヤーをマークすることができるクルーメイト。" +
                                                    "\n彼を追放することは、マークされたプレーヤーを亡命しません。";
                                                break;
                                            case "jinx":
                                                infoText = "Jinx: 他のプレイヤーのボタンをブロックできるクルーメイト。" +
                                                    "\nジンクス中にボタンを使用すると、そのボタンはクールダウンに入ります。";
                                                break;
                                            case "coward":
                                                infoText = "Coward: どこからでも会議に電話できる乗組員。" +
                                                    "\n妨害行為がアクティブであれば、彼は会議に電話することができません。";
                                                break;
                                            case "bat":
                                                infoText = "Bat: crewmate who can emit a frequency that alters button cooldown." +
                                                    "\nクルーメイト、反乱軍、ニュートラルのボタンのクールダウンは、x2をより速くします。" +
                                                    "\n詐欺師のボタンのクールダウンは毎秒 1 ずつ増加します。";
                                                break;
                                            case "necromancer":
                                                infoText = "Necromancer: 身体をドラッグアンドドロップし、それらを復活させることができる乗組員は、特定の部屋にドラッグします。" +
                                                    "\n特定の部屋は青い矢印で尖っています。" +
                                                    "\n詐欺師と反乱軍は、復活したプレーヤーを指すピンクの矢を手に入れます。";
                                                break;
                                            case "engineer":
                                                infoText = "Engineer: 速度を上げたり減少させたり、トラップを配置したりできる乗組員。" +
                                                    "\n彼はFキーでトラップタイプを切り替えることができます。" +
                                                    "\nトラップは会議の後にアクティブになり、5秒の効果の期間があります。";
                                                break;
                                            case "shy":
                                                infoText = "Shy: 最も近いプレーヤーの地位を明らかにすることができるクルーメイト。" +
                                                    "\n矢印は、最も近いプレーヤーの方向を明らかにします。";
                                                break;
                                            case "taskmaster":
                                                infoText = "Task Master: 最初のタスクを行った後に余分なタスクを持っている乗組員。" +
                                                    "\n殺される前に余分なタスクを完了すると、乗組員の勝利が得られます。";
                                                break;
                                            case "jailer":
                                                infoText = "Jailer: プレーヤーをアシスタントにマークできるクルーメイト。" +
                                                    "\nアシスタントを殺そうとすると、殺害を否定し、殺人者を数秒間刑務所にテレポートしますが、これは1回だけ機能し、看守はこの後に別のプレーヤーをマークする必要があります。";
                                                break;
                                        }
                                    }
                                    break;
                                // Chinese
                                case 4:
                                    if (roleInfo == null) {
                                        infoText = "你没有职业。";
                                    }
                                    else {
                                        switch (roleInfo.roleId.ToString().ToLower()) {
                                            // Impostor roles
                                            case "mimic":
                                                infoText = "Mimic: 内鬼阵营：可以模仿其他玩家的外表。";
                                                break;
                                            case "painter":
                                                infoText = "Painter: 内鬼阵营：可以让其他玩家变为同一个外形并隐藏名字。";
                                                break;
                                            case "demon":
                                                infoText = "Demon: 内鬼阵营：咬伤玩家后，那位玩家会慢慢失血而亡。" +
                                                    "\n如果有吸血鬼在场，所以所有船员会获得一个修女。" +
                                                    "\n在修女旁边会使咬人无效果。";
                                                break;
                                            case "janitor":
                                                infoText = "Janitor: 内鬼阵营：可以移动尸体。" +
                                                    "\n但是不可以在同一时间重新移动尸体。";
                                                break;
                                            case "illusionist":
                                                infoText = "Illusionist: 内鬼阵营：可以放置3个帽子，帽子之间相互连通，并且可以随时随地控制灯光。" +
                                                    "\n这3个帽子只能供自己使用，当全部放置完毕后，所有人都可以看到帽子，且获得控制灯光的能力。";
                                                break;
                                            case "manipulator":
                                                infoText = "Manipulator: 内鬼阵营：可以在任何地方操纵一个玩家击杀被操控者附近的一个玩家。" +
                                                    "\n可以用他的能力杀死任何人，包括他自己。";
                                                break;
                                            case "bomberman":
                                                infoText = "Bomberman: 内鬼阵营：可以在地图上放置炸弹。" +
                                                    "\n如果处于破坏状态或附近有人时，炸弹就不能被放置。" +
                                                    "\n其他玩家可以通过接触来拆除炸弹，如果炸弹没有被及时拆除，内鬼将会获胜。";
                                                break;
                                            case "chameleon":
                                                infoText = "Chameleon: 内鬼阵营：可以隐身。" +
                                                    "\n不能使用管道。在隐身期间，仍然可以被击杀。";
                                                break;
                                            case "gambler":
                                                infoText = "Gambler: 内鬼阵营：在会议期间猜测其他玩家的身份，猜对可以杀死对方，猜错会死亡。" +
                                                    "\n猜测界面只会显示本局存在身份。";
                                                break;
                                            case "sorcerer":
                                                infoText = "Sorcerer: 内鬼阵营：可以向玩家施加诅咒。" +
                                                    "\n在会议期间，被诅咒的玩家名字旁边会有一个紫色的南瓜图标。会议结束后被诅咒玩家将会死亡，除非巫师被放逐。";
                                                break;
                                            case "medusa":
                                                infoText = "Medusa: 内鬼阵营：能将其他玩家石化。" +
                                                    "\n被石化的玩家在一定时间内不能移动。";
                                                break;
                                            case "hypnotist":
                                                infoText = "Hypnotist: 内鬼阵营：通过放置陷阱，让被踩中玩家迷失方向。" +
                                                    "\n陷阱在会议后生效，陷阱不影响内鬼阵营玩家。";
                                                break;
                                            case "archer":
                                                infoText = "Archer: 内鬼阵营：只能使用弓进行击杀，不能进行普通击杀。" +
                                                    "\n需要使用弓箭（其他玩家看不到），用鼠标瞄准，然后点击鼠标右键进行射击。" +
                                                    "\n如果没有射中，就会在自己的位置上会出现一个警告图像，如果射中了，就会在尸体上出现一个警告图像。";
                                                break;
                                            case "plumber":
                                                infoText = "Plumber: 内鬼阵营：可以建造管道的职业。" +
                                                    "\n当所有额外的管道都建造完成后，等到会议结束后这些管道就会生效。";
                                                break;
                                            case "librarian":
                                                infoText = "Librarian: 内鬼阵营：可以阻止玩家在会议上发言。" +
                                                    "\n所有人都可以知道谁在会议上被禁言。";
                                                break;


                                            // Rebel roles
                                            case "renegade":
                                                infoText = "Renegade: 叛乱者阵营：杀死所有人。" +
                                                    "\n他可以招募一个玩家来帮助他击杀，两人都有内鬼的视野，可以使用管道，他们的名字以及管道将显示为绿色的。";
                                                break;
                                            case "bountyhunter":
                                                infoText = "Bounty Hunter: 叛乱者阵营：杀死他的目标。" +
                                                    "\n随机一位玩家作为他的目标，如果目标被非自己杀死，他也会死亡。";
                                                break;
                                            case "trapper":
                                                infoText = "Trapper: 叛乱者阵营：用地雷炸死所有人。" +
                                                    "\n可以设置陷阱，将触碰它的玩家困住，也可以放置地雷，炸死其他玩家。";
                                                break;
                                            case "yinyanger":
                                                infoText = "Yinyanger: 叛乱者阵营：杀死所有人，每次都要标记两个玩家。" +
                                                    "\n需要用 '阴 '标记一名玩家，然后用· '阳 '标记另一名玩家，如果他们相遇，两个人都会死亡，无视任何护盾。" +
                                                    "\n不可以标记两个相近的玩家，只能标记两个相近玩家中的其中一个。";
                                                break;
                                            case "challenger":
                                                infoText = "Challenger: 叛乱者阵营：与其他玩家进行石头剪刀布以决斗的方式杀死所有人。" +
                                                    "\n选择一名玩家并在10秒后传送所有玩家到决斗竞技场。" +
                                                    "\n如果其中一个没有选择攻击，另一个就会自动获胜。如果没有人选择攻击，他们都会死。";
                                                break;
                                            case "ninja":
                                                infoText = "Ninja: 叛乱者阵营：杀光所有人并制造双杀。" +
                                                    "\n选择一个玩家传送到他的位置，然后杀死他，在此之后使用普通的击杀按钮进行双杀。";
                                                break;
                                            case "berserker":
                                                infoText = "Berserker: 叛乱者阵营：杀死所有人，并且无法停止杀戮。" +
                                                    "\n在第一次击杀后，击杀冷却永远只有10秒，如果在一定时间内没有杀人，便会死亡。";
                                                break;
                                            case "yandere":
                                                infoText = "Yandere: 叛乱者阵营：跟踪目标，并杀死它赢得胜利。" +
                                                    "\n如果目标被放逐或被杀死，那么必须杀死所有人才能获胜。";
                                                break;
                                            case "stranded":
                                                infoText = "Stranded: 叛乱者阵营：需要在地图周围的盒子中找到弹药并杀死3名玩家。" +
                                                    "\n也可以在盒子中找到让自己隐形以及使用管道能力的物品。";
                                                break;
                                            case "monja":
                                                infoText = "Monja: 叛乱者阵营：找到所有的小修女，并把他们带到仪式现场。" +
                                                    "\n当收集到所有小修女时，就会在60秒后变成修女并杀死所有人，否则就会死亡。" +
                                                    "\n如果有小孩在游戏中，那么只要杀死除小孩以为的玩家即可胜利。";
                                                break;

                                            // Neutral roles
                                            case "joker":
                                                infoText = "Joker: 中立阵营：在会议中被放逐才能获胜。" +
                                                    "\n可以使用破坏，但前提是必须活着。";
                                                break;
                                            case "rolethief":
                                                infoText = "Role Thief: 中立阵营：在交换身份前无法获胜。" +
                                                    "\n可以窃取其他玩家的角色，但如果试图窃取内鬼或叛乱者的职业，他就会死亡。";
                                                break;
                                            case "pyromaniac":
                                                infoText = "Pyromaniac: 中立阵营：把所有人浇油并点燃才能获胜。" +
                                                    "\n可以站在其他玩家旁边浇油，浇完所有人就可以获得点燃按钮。";
                                                break;
                                            case "treasurehunter":
                                                infoText = "Treasure Hunter: 中立阵营:寻找所有宝藏才能获胜。" +
                                                    "\n使用按钮，每轮在地图上随机产生一个宝藏，找到所需数量后就赢了。";
                                                break;
                                            case "devourer":
                                                infoText = "Devourer: 中立阵营：吞噬足够的尸体即可胜利。" +
                                                    "\n当玩家死亡时，会听到一种声音。";
                                                break;
                                            case "poisoner":
                                                infoText = "Poisoner: 中立阵营：感染所有人才能胜利。" +
                                                    "\n可以选择一名玩家作为感染源，感染源旁边的玩家会增加他们的感染值。" +
                                                    "\n一旦所有玩家达到100%感染指数，他便获胜。";
                                                break;
                                            case "puppeteer":
                                                infoText = "Puppeteer: 中立阵营：变身成其他玩家，变身状态下被击杀指定次数才能获胜。" +
                                                    "\n可以从玩家身上选择一个样本，然后随时变形，直到会议召开，就会变回原形。" +
                                                    "\n如果在变行状态时被杀，将获得1分，并在他开始变形的地方复活，在达到所需的分数后他将获胜。";
                                                break;
                                            case "exiler":
                                                infoText = "Exiler: 中立阵营：在会议中放逐目标玩家才能获胜。" +
                                                    "\n开局将随机分配一个玩家作为目标，但如果目标非投票死亡，他也会死亡。";
                                                break;
                                            case "amnesiac":
                                                infoText = "Amnesiac: 中立阵营：报告尸体来获得死者的职业。" +
                                                    "\n会议结束后会获得尸体职业。";
                                                break;
                                            case "seeker":
                                                infoText = "Seeker: 中立阵营：在捉迷藏小游戏中找到其他玩家。" +
                                                    "\n在选择3个人后，就可以开始游戏，将所有玩家传送到一个新的区域。" +
                                                    "\n在游戏中每找到一个玩家便获得1分，在达到所需点数后便获胜。";
                                                break;

                                            // Crewmate roles
                                            case "captain":
                                                infoText = "Captain: 船员阵营：拥有双倍的票数。" +
                                                    "\n可以在每局游戏中把所有的票数转给另一个玩家，但如果把票数转给一个船员，就会被放逐。" +
                                                    "\n场上只剩内鬼阵或营反叛者阵营时，可以召集紧急会议放逐剩下的玩家。";
                                                break;
                                            case "mechanic":
                                                infoText = "Mechanic: 船员阵营：可以随时随地修复一定次数破坏。" +
                                                    "\n可以一个人修复所有的破坏。";
                                                break;
                                            case "sheriff":
                                                infoText = "Sheriff: 船员阵营：可以击杀其他玩家。" +
                                                    "\n如果试图击杀船员，就会死亡。";
                                                break;
                                            case "detective":
                                                infoText = "Detective: 船员阵营：可以看到所有的玩家脚印。" +
                                                    "\n脚印不会在管道内产生，只有侦探活着的时候才会生成。";
                                                break;
                                            case "forensic":
                                                infoText = "Forensic: 船员阵营：通过报告尸体和询问玩家的灵魂来获得线索。" +
                                                    "\n通过报告尸体可以得到的线索是凶手的名字、颜色类型或关于他的外表的东西。" +
                                                    "\n灵魂会在下一轮出现在尸体所在的地方，它们的线索有可能是杀手的颜色类型，或在报告尸体前经过了多长时间。";
                                                break;
                                            case "timetraveler":
                                                infoText = "Time Traveler: 船员阵营：每场比赛可以将时间倒退两次，使在倒退过程中被杀死的玩家复活。。" +
                                                    "\n如果没有处于破坏状态下，他可以倒退时间。如果有人试图在他开启时间盾庇护的情况下杀死他，时间也会倒退。" +
                                                    "\n该职业不能使用生命体征仪。";
                                                break;
                                            case "squire":
                                                infoText = "Squire: 船员阵营：可以给一个玩家戴上一个护盾。" +
                                                    "\n直到他死亡前，试图杀死被保护的玩家将触发一个声音，Impostors，Rebels，Sheriff，Squire 和被庇护的玩家可以听见这个声音。";
                                                break;
                                            case "cheater":
                                                infoText = "Cheater: 船员阵营：可以调换两个玩家的票。" +
                                                    "\n在交换选票后，如果这两个玩家中的一个是船员，就会被放逐。";
                                                break;
                                            case "fortuneteller":
                                                infoText = "Fortune Teller: 船员阵营：可以预言一个玩家是好是坏。" +
                                                    "\n预言会触发一个声音，并使两个玩家的屏幕变成蓝色。" +
                                                    "\n坏职业的名字变成红色，好职业的名字变成青色。";
                                                break;
                                            case "hacker":
                                                infoText = "Hacker: 船员阵营：可以在任何地方使用管理和生命体征，并从中获得更多信息。" +
                                                    "\n当黑客能力被激活时，可以在管理地图上看到玩家的大致位置及颜色，以及从生命体征仪上查看玩家是否死亡。";
                                                break;
                                            case "sleuth":
                                                infoText = "Sleuth: 船员阵营：可以追踪尸体和一名玩家的位置。" +
                                                    "\n蓝色箭头指向被追踪的玩家，绿色箭头指向尸体。";
                                                break;
                                            case "fink":
                                                infoText = "Fink: 船员阵营：完成任务后，就可以知道谁是内鬼。" +
                                                    "\n可以扩大视野，当扩大视野时，不能移动。" +
                                                    "\n当剩下几个任务或扩大视野时，会有箭头提示内鬼或叛乱者。";
                                                break;
                                            case "kid":
                                                infoText = "Kid: 船员阵营：不应能被杀死或放逐，否则大家都会损失。" +
                                                    "\n他比其他玩家都小。";
                                                break;
                                            case "welder":
                                                infoText = "Welder: 船员阵营：可以堵住通风口。" +
                                                    "\n这些管道在下次会议后就不可用了，不能进入或退出，但仍然可以停留在管道内。";
                                                break;
                                            case "spiritualist":
                                                infoText = "Spiritualist: 船员阵营：能以自己的生命救活另一个玩家。" +
                                                    "\n他可以复活玩家，如果有人在他复活的时候召集会议，他就会死。" +
                                                    "\n内鬼阵营和叛乱者阵营会看见一个粉色箭头指向复活的玩家。";
                                                break;
                                            case "vigilant":
                                                infoText = "Vigilant: 船员阵营：能在地图上多放四个摄像头。" +
                                                    "\n摄像头在会议结束后可用，当放置所有摄像头时，就可以随时随地使用监控。" +
                                                    "\n在米拉总部地图上，他可以远程检查门的情况。";
                                                break;
                                            case "hunter":
                                                infoText = "Hunter: 船员阵营：可以标记一名玩家，如果被杀死，被标记玩家也会死亡。" +
                                                    "\n当被放逐时，并不会放逐被标记的玩家。";
                                                break;
                                            case "jinx":
                                                infoText = "Jinx: 船员阵营：可以使玩家技能失效。" +
                                                    "\n在被诅咒时使用技能会失效并进入冷却。";
                                                break;
                                            case "coward":
                                                infoText = "Coward: 船员阵营：可以随时随地召集会议。" +
                                                    "\n如果处于破坏状态下，就不能远程召集会议。";
                                                break;
                                            case "bat":
                                                infoText = "Bat: 船员阵营：可以通过技能使冷却时间发生改变。" +
                                                    "\nCrewmates, Rebels 和 Neutrals 时间加快了2倍" +
                                                    "\nImpostors 冷却时间增加一定的秒数";
                                                break;
                                            case "necromancer":
                                                infoText = "Necromancer: 船员阵营：可以拖放尸体并将其拖到特定房间复活。" +
                                                    "\n房间由蓝色箭头指示。" +
                                                    "\n内鬼阵营和叛乱者阵营会得到一个粉色箭头指向复活的玩家。";
                                                break;
                                            case "engineer":
                                                infoText = "Engineer: 船员阵营：可以放置加速，减速，位置踏板。" +
                                                    "\n按F键切换放置类型。" +
                                                    "\n在会议结束后生效并有5秒持续时间。";
                                                break;
                                            case "shy":
                                                infoText = "Shy: 船员阵营：箭头指向距离他最近的玩家。";
                                                break;
                                            case "taskmaster":
                                                infoText = "Task Master: 船员阵营：在做完任务后有额外的任务。" +
                                                    "\n在临死之前做完额外任务将获得胜利。";
                                                break;
                                            case "jailer":
                                                infoText = "Jailer: 船员阵营：可以标记一位玩家进行保护。" +
                                                    "\n如果有玩家试图击杀被庇护的玩家将被送进监狱一段时间成为狱卒，狱卒需标记另一位玩家。";
                                                break;
                                        }
                                    }
                                    break;
                            }
                            handled = true;
                            __instance.AddChat(PlayerControl.LocalPlayer, infoText);
                            //PlayerControl.LocalPlayer.RpcSendChat(infoText);                          
                        }
                        RoleInfo roleInfoModifier = infos.Where(info => info.isModifier).FirstOrDefault();
                        if (text.ToLower().StartsWith("/mymodifier") || text.ToLower().StartsWith("/mymod")) {
                            switch (LasMonjasPlugin.modLanguage.Value) {
                                // English
                                case 1:
                                    if (roleInfoModifier == null) {
                                        infoText = "You don't have a Modifier.";
                                    }
                                    else {
                                        switch (roleInfoModifier.roleId.ToString().ToLower()) {
                                            // Modifiers:
                                            case "lover":
                                                infoText = "Lovers: modifier who links two players." +
                                                    "\nBoth die if one get killed or exiled.";
                                                break;
                                            case "lighter":
                                                infoText = "Lighter: modifier who gives more vision to a player." +
                                                    "\nAlso it makes that player immune to night vision.";
                                                break;
                                            case "blind":
                                                infoText = "Blind: modifier who reduces a player's vision." +
                                                    "\nIt doesn't affect impostors.";
                                                break;
                                            case "flash":
                                                infoText = "Flash: modifier who increases a player's speed." +
                                                    "\nDoesn't affect during Challenger's duel and anonymous comms.";
                                                break;
                                            case "bigchungus":
                                                infoText = "Big Chungus: modifier who increases a player's size and reduces his speed." +
                                                    "\nDoesn't affect during Challenger's duel and anonymous comms.";
                                                break;
                                            case "thechosenone":
                                                infoText = "The Chosen One: modifier who force his killer to report his body." +
                                                    "\nA report delay can be configured.";
                                                break;
                                            case "performer":
                                                infoText = "Performer: modifier whose death triggers music and an arrow revels his position." +
                                                    "\nMusic's duration can be configured.";
                                                break;
                                            case "pro":
                                                infoText = "Pro: modifier who inverts a player's movement controls." +
                                                    "\nIt also affects the player on ghost form.";
                                                break;
                                            case "paintball":
                                                infoText = "Paintball: modifier who splashes his killer on death, the killer leaves a trail with the player's color for a few seconds.";
                                                break;
                                            case "electrician":
                                                infoText = "Electrician: modifier who paralyzes his killer for a few seconds.";
                                                break;
                                        }
                                    }
                                    break;
                                // Spanish
                                case 2:
                                    if (roleInfoModifier == null) {
                                        infoText = "No tienes un Modificador.";
                                    }
                                    else {
                                        switch (roleInfoModifier.roleId.ToString().ToLower()) {
                                            // Modifiers:
                                            case "lovers":
                                                infoText = "Lovers: modificador que vincula a dos jugadores." +
                                                    "\nSi uno muere o es expulsado, el otro tambien.";
                                                break;
                                            case "lighter":
                                                infoText = "Lighter: modificador que incrementa el rango de vision de un jugador." +
                                                    "\nAdemas es inmune a la vision nocturna.";
                                                break;
                                            case "blind":
                                                infoText = "Blind: modificador que reduce el rango de vision de un jugador." +
                                                    "\nNo afecta a Impostores.";
                                                break;
                                            case "flash":
                                                infoText = "Flash: modificador que incrementa la velocidad de movimiento de un jugador." +
                                                    "\nEste efecto no se aplica durante el duelo del Challenger, escondite del Seeker o comunicaciones anonimas.";
                                                break;
                                            case "bigchungus":
                                                infoText = "Big Chungus: modificador que reduce la velocidad de movimiento de un jugador y lo hace mas grande." +
                                                    "\nEste efecto no se aplica durante el duelo del Challenger, escondite del Seeker o comunicaciones anonimas.";
                                                break;
                                            case "thechosenone":
                                                infoText = "The Chosen One: modificador que forzara a su asesino a reportar el cuerpo de ese jugador." +
                                                    "\nUna demora de reporte de hasta 5 segundos puede ser configurada.";
                                                break;
                                            case "performer":
                                                infoText = "Performer: modificador cuya muerte activa una alarma y una flecha revela la posicion del cuerpo de ese jugador." +
                                                    "\nLa duracion de la Alarma puede ser configurada.";
                                                break;
                                            case "pro":
                                                infoText = "Pro: modificador que invierte los controles de movimiento de un jugador." +
                                                    "\nTambien afecta si eres fantasma.";
                                                break;
                                            case "paintball":
                                                infoText = "Paintball: modificador que salpica al asesino de ese jugador, haciendo que el asesino deje un rastro del color del jugador asesinado durante un tiempo.";
                                                break;
                                            case "electrician":
                                                infoText = "Electrician: modificador que paraliza durante unos segundos a su asesino.";
                                                break;
                                        }
                                    }
                                    break;
                                // Japanese
                                case 3:
                                    if (roleInfoModifier == null) {
                                        infoText = "修飾子はありません。";
                                    }
                                    else {
                                        switch (roleInfoModifier.roleId.ToString().ToLower()) {
                                            // Modifiers:
                                            case "lovers":
                                                infoText = "Lovers: 2人のプレーヤーをリンクする修飾子。" +
                                                    "\n殺されたり追放されたりすると、両方とも死にます。";
                                                break;
                                            case "lighter":
                                                infoText = "Lighter: プレーヤーにより多くのビジョンを与える修飾子。" +
                                                    "\nまた、そのプレイヤーは暗視に免疫があります。";
                                                break;
                                            case "blind":
                                                infoText = "Blind: プレーヤーのビジョンを減らす修飾子。" +
                                                    "\n詐欺師には影響しません。";
                                                break;
                                            case "flash":
                                                infoText = "Flash: プレーヤーの速度を上げる修飾子。" +
                                                    "\nチャレンジャーの決闘と匿名の通信中には影響しません。";
                                                break;
                                            case "bigchungus":
                                                infoText = "Big Chungus: プレーヤーのサイズを大きくして速度を下げる修飾子。" +
                                                    "\nチャレンジャーの決闘と匿名の通信中には影響しません。";
                                                break;
                                            case "thechosenone":
                                                infoText = "The Chosen One: 彼の殺人者に彼の体を報告させる修飾子。" +
                                                    "\nレポート遅延を構成できます。";
                                                break;
                                            case "performer":
                                                infoText = "Performer: 死が音楽と矢がトリガーされる修飾子が彼の立場を楽しんでいます。" +
                                                    "\n音楽の期間を構成できます。";
                                                break;
                                            case "pro":
                                                infoText = "Pro: プレーヤーの動きを反転させる修飾子。" +
                                                    "\nまた、ゴーストフォームのプレーヤーにも影響します。";
                                                break;
                                            case "paintball":
                                                infoText = "Paintball: 死亡時に殺人者をはねかける修飾子、殺人者は数秒間プレーヤーの色でトレイルを残します。";
                                                break;
                                            case "electrician":
                                                infoText = "Electrician: 数秒間彼の殺人者を麻痺させる修飾子。";
                                                break;
                                        }
                                    }
                                    break;
                                // Chinese
                                case 4:
                                    if (roleInfoModifier == null) {
                                        infoText = "你没有附加职业";
                                    }
                                    else {
                                        switch (roleInfoModifier.roleId.ToString().ToLower()) {
                                            // Modifiers:
                                            case "lovers":
                                                infoText = "Lovers: 附加职业：两个人恋爱了。" +
                                                    "\n其中一个被击杀或驱逐，另一位也会为爱赴死。";
                                                break;
                                            case "lighter":
                                                infoText = "Lighter: 附加职业：拥有更大的视野。" +
                                                    "\n且黑灯时也能看清。";
                                                break;
                                            case "blind":
                                                infoText = "Blind: 附加职业：视野降低。" +
                                                    "\n但对内鬼阵营无效。";
                                                break;
                                            case "flash":
                                                infoText = "Flash: 附加职业：速度变快。" +
                                                    "\n在和挑战者猜拳和匿名通讯中无效。";
                                                break;
                                            case "bigchungus":
                                                infoText = "Big Chungus: 附加职业：体型变大，走不动路。" +
                                                    "\n在和挑战者猜拳和匿名通讯中无效。";
                                                break;
                                            case "thechosenone":
                                                infoText = "The Chosen One: 附加职业：强迫凶手报告。" +
                                                    "\n可设置报告延迟。";
                                                break;
                                            case "performer":
                                                infoText = "Performer: 附加职业：当死亡时会有JOJO音乐并有箭头指向尸体位置。" +
                                                    "\n可设置音乐时间。";
                                                break;
                                            case "pro":
                                                infoText = "Pro: 让玩家失去方向感反方向移动。" +
                                                    "\n变成灵魂也一样。";
                                                break;
                                            case "paintball":
                                                infoText = "Paintball: 加职业：死亡时血液会溅到凶手身上，凶手会留下带有玩家颜色的痕迹，可设置持续时间。";
                                                break;
                                            case "electrician":
                                                infoText = "Electrician: 附加职业：药水溅到凶手身上，使凶手瘫痪。";
                                                break;
                                        }
                                    }
                                    break;
                            }
                            handled = true;
                            __instance.AddChat(PlayerControl.LocalPlayer, infoText);
                            //PlayerControl.LocalPlayer.RpcSendChat(infoText);
                        }
                    }

                    if (handled) {
                        __instance.TextArea.Clear();
                        __instance.quickChatMenu.ResetGlyphs();
                    }
                    return !handled;
                }
            }
        }
    }
}