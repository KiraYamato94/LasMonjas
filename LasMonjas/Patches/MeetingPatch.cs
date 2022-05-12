using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnhollowerBaseLib;
using static LasMonjas.LasMonjas;
using static LasMonjas.MapOptions;
using System.Collections;
using System;
using System.Text;
using UnityEngine;
using System.Reflection;

namespace LasMonjas.Patches {
    [HarmonyPatch]
    class MeetingHudPatch {
        static bool[] selections;
        static SpriteRenderer[] renderers;
        private static GameData.PlayerInfo target = null;
        private const float scale = 0.65f;
        
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CheckForEndVoting))]
        class MeetingCalculateVotesPatch {
            private static Dictionary<byte, int> CalculateVotes(MeetingHud __instance) {
                Dictionary<byte, int> dictionary = new Dictionary<byte, int>();
                for (int i = 0; i < __instance.playerStates.Length; i++) {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.VotedFor != 252 && playerVoteArea.VotedFor != 255 && playerVoteArea.VotedFor != 254) {
                        PlayerControl player = Helpers.playerById((byte)playerVoteArea.TargetPlayerId);
                        if (player == null || player.Data == null || player.Data.IsDead || player.Data.Disconnected) continue;

                        int currentVotes;
                        int additionalVotes = (Captain.captain != null && Captain.captain.PlayerId == playerVoteArea.TargetPlayerId) ? 2 : 1; // Captain additional vote
                        if (dictionary.TryGetValue(playerVoteArea.VotedFor, out currentVotes))
                            dictionary[playerVoteArea.VotedFor] = currentVotes + additionalVotes;
                        else
                            dictionary[playerVoteArea.VotedFor] = additionalVotes;
                    }
                }
                // Cheater cheat votes
                if (Cheater.cheater != null && !Cheater.cheater.Data.IsDead) {
                    PlayerVoteArea cheated1 = null;
                    PlayerVoteArea cheated2 = null;
                    foreach (PlayerVoteArea playerVoteArea in __instance.playerStates) {
                        if (playerVoteArea.TargetPlayerId == Cheater.playerId1) cheated1 = playerVoteArea;
                        if (playerVoteArea.TargetPlayerId == Cheater.playerId2) cheated2 = playerVoteArea;
                    }

                    if (cheated1 != null && cheated2 != null) {
                        if (!dictionary.ContainsKey(cheated1.TargetPlayerId)) dictionary[cheated1.TargetPlayerId] = 0;
                        if (!dictionary.ContainsKey(cheated2.TargetPlayerId)) dictionary[cheated2.TargetPlayerId] = 0;
                        int tmp = dictionary[cheated1.TargetPlayerId];
                        dictionary[cheated1.TargetPlayerId] = dictionary[cheated2.TargetPlayerId];
                        dictionary[cheated2.TargetPlayerId] = tmp;
                    }
                }

                return dictionary;
            }


            static bool Prefix(MeetingHud __instance) {
                if (__instance.playerStates.All((PlayerVoteArea ps) => ps.AmDead || ps.DidVote)) {
                    
			        Dictionary<byte, int> self = CalculateVotes(__instance);
                    bool tie;
			        KeyValuePair<byte, int> max = self.MaxPair(out tie);
                    GameData.PlayerInfo exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => !tie && v.PlayerId == max.Key && !v.IsDead);

                    MeetingHud.VoterState[] array = new MeetingHud.VoterState[__instance.playerStates.Length];
                    for (int i = 0; i < __instance.playerStates.Length; i++)
                    {
                        PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                        array[i] = new MeetingHud.VoterState {
                            VoterId = playerVoteArea.TargetPlayerId,
                            VotedForId = playerVoteArea.VotedFor
                        };
                    }

                    __instance.RpcVotingComplete(array, exiled, tie);
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.BloopAVoteIcon))]
        class MeetingHudBloopAVoteIconPatch {
            public static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)]GameData.PlayerInfo voterPlayer, [HarmonyArgument(1)]int index, [HarmonyArgument(2)]Transform parent) {
                SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate<SpriteRenderer>(__instance.PlayerVotePrefab);
                if (!PlayerControl.GameOptions.AnonymousVotes)
                    PlayerControl.SetPlayerMaterialColors(voterPlayer.DefaultOutfit.ColorId, spriteRenderer);
                else
                    PlayerControl.SetPlayerMaterialColors(Palette.DisabledGrey, spriteRenderer);
                spriteRenderer.transform.SetParent(parent);
                spriteRenderer.transform.localScale = Vector3.zero;
                __instance.StartCoroutine(Effects.Bloop((float)index * 0.3f, spriteRenderer.transform, 1f, 0.5f));
                parent.GetComponent<VoteSpreader>().AddVote(spriteRenderer);
                return false;
            }
        } 

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.PopulateResults))]
        class MeetingHudPopulateVotesPatch {
            
            static bool Prefix(MeetingHud __instance, Il2CppStructArray<MeetingHud.VoterState> states) {
                
                // Cheater cheat
                PlayerVoteArea cheated1 = null;
                PlayerVoteArea cheated2 = null;
                foreach (PlayerVoteArea playerVoteArea in __instance.playerStates) {
                    if (playerVoteArea.TargetPlayerId == Cheater.playerId1) cheated1 = playerVoteArea;
                    if (playerVoteArea.TargetPlayerId == Cheater.playerId2) cheated2 = playerVoteArea;
                }
                bool doCheat = cheated1 != null && cheated2 != null && Cheater.cheater != null && !Cheater.cheater.Data.IsDead;
                if (doCheat) {
                    __instance.StartCoroutine(Effects.Slide3D(cheated1.transform, cheated1.transform.localPosition, cheated2.transform.localPosition, 1.5f));
                    __instance.StartCoroutine(Effects.Slide3D(cheated2.transform, cheated2.transform.localPosition, cheated1.transform.localPosition, 1.5f));
                }


                __instance.TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingVotingResults, new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                int num = 0;
                for (int i = 0; i < __instance.playerStates.Length; i++) {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    byte targetPlayerId = playerVoteArea.TargetPlayerId;
                    // Cheater change playerVoteArea that gets the votes
                    if (doCheat && playerVoteArea.TargetPlayerId == cheated1.TargetPlayerId) playerVoteArea = cheated2;
                    else if (doCheat && playerVoteArea.TargetPlayerId == cheated2.TargetPlayerId) playerVoteArea = cheated1;

                    playerVoteArea.ClearForResults();
                    int num2 = 0;
                    bool captainFirstVoteDisplayed = false;
                    for (int j = 0; j < states.Length; j++) {
                        MeetingHud.VoterState voterState = states[j];
                        GameData.PlayerInfo playerById = GameData.Instance.GetPlayerById(voterState.VoterId);
                        if (playerById == null) {
                            Debug.LogError(string.Format("Couldn't find player info for voter: {0}", voterState.VoterId));
                        } else if (i == 0 && voterState.SkippedVote && !playerById.IsDead) {
                            __instance.BloopAVoteIcon(playerById, num, __instance.SkippedVoting.transform);
                            num++;
                        }
                        else if (voterState.VotedForId == targetPlayerId && !playerById.IsDead) {
                            __instance.BloopAVoteIcon(playerById, num2, playerVoteArea.transform);
                            num2++;
                        }

                        // Captain vote, redo this iteration to place a second vote
                        if (Captain.captain != null && voterState.VoterId == (sbyte)Captain.captain.PlayerId && !captainFirstVoteDisplayed) {
                            captainFirstVoteDisplayed = true;
                            j--;    
                        }
                    }
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        class MeetingHudVotingCompletedPatch {
            static void Postfix(MeetingHud __instance, [HarmonyArgument(0)]byte[] states, [HarmonyArgument(1)]GameData.PlayerInfo exiled, [HarmonyArgument(2)]bool tie)
            {
                // Reset cheater values
                Cheater.playerId1 = Byte.MaxValue;
                Cheater.playerId2 = Byte.MaxValue;

                // Lovers save next to be exiled, because RPC of ending game comes before RPC of exiled
                Modifiers.notAckedExiledIsLover = false;
                if (exiled != null)
                    Modifiers.notAckedExiledIsLover = ((Modifiers.lover1 != null && Modifiers.lover1.PlayerId == exiled.PlayerId) || (Modifiers.lover2 != null && Modifiers.lover2.PlayerId == exiled.PlayerId));
            }
        }


        static void cheaterOnClick(int i, MeetingHud __instance) {
            if (__instance.state == MeetingHud.VoteStates.Results) return;
            if (__instance.playerStates[i].AmDead) return;

            int selectedCount = selections.Where(b => b).Count();
            SpriteRenderer renderer = renderers[i];

            if (selectedCount == 0) {
                renderer.color = Color.green;
                selections[i] = true;
            } else if (selectedCount == 1) {
                if (selections[i]) {
                    renderer.color = Color.red;
                    selections[i] = false;
                } else {
                    selections[i] = true;
                    renderer.color = Color.green;   
                    
                    PlayerVoteArea firstPlayer = null;
                    PlayerVoteArea secondPlayer = null;
                    for (int A = 0; A < selections.Length; A++) {
                        if (selections[A]) {
                            if (firstPlayer != null) {
                                secondPlayer = __instance.playerStates[A];
                                break;
                            } else {
                                firstPlayer = __instance.playerStates[A];
                            }
                        }
                    }

                    if (firstPlayer != null && secondPlayer != null) {
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.CheaterCheat, Hazel.SendOption.Reliable, -1);
                        writer.Write((byte)firstPlayer.TargetPlayerId);
                        writer.Write((byte)secondPlayer.TargetPlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);

                        RPCProcedure.cheaterCheat((byte)firstPlayer.TargetPlayerId, (byte)secondPlayer.TargetPlayerId);
                    }
                }
            }
        }

        private static GameObject gamblerUI;
        static void gamblerOnClick(int buttonTarget, MeetingHud __instance) {
            if (gamblerUI != null || !(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted)) return;
            __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(false));

            Transform container = UnityEngine.Object.Instantiate(__instance.transform.FindChild("PhoneUI"), __instance.transform);
            container.transform.localPosition = new Vector3(0, 0, -5f);
            gamblerUI = container.gameObject;

            int i = 0;
            var buttonTemplate = __instance.playerStates[0].transform.FindChild("votePlayerBase");
            var maskTemplate = __instance.playerStates[0].transform.FindChild("MaskArea");
            var smallButtonTemplate = __instance.playerStates[0].Buttons.transform.Find("CancelButton");
            var textTemplate = __instance.playerStates[0].NameText;

            Transform exitButtonParent = (new GameObject()).transform;
            exitButtonParent.SetParent(container);
            Transform exitButton = UnityEngine.Object.Instantiate(buttonTemplate.transform, exitButtonParent);
            Transform exitButtonMask = UnityEngine.Object.Instantiate(maskTemplate, exitButtonParent);
            exitButton.gameObject.GetComponent<SpriteRenderer>().sprite = smallButtonTemplate.GetComponent<SpriteRenderer>().sprite;
            exitButtonParent.transform.localPosition = new Vector3(2.725f, 2.1f, -5);
            exitButtonParent.transform.localScale = new Vector3(0.217f, 0.9f, 1);
            exitButton.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
            exitButton.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() => {
                __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                UnityEngine.Object.Destroy(container.gameObject);
            }));

            List<Transform> buttons = new List<Transform>();
            Transform selectedButton = null;

            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                foreach (RoleInfo roleInfo in RoleInfo.allRoleInfos) {
                    // Not gambled roles
                    if (/* Special roles */ roleInfo.roleId == RoleId.Lover || roleInfo.roleId == RoleId.Kid || (roleInfo == RoleInfo.vigilantMira && PlayerControl.GameOptions.MapId != 1) || (roleInfo == RoleInfo.vigilant && PlayerControl.GameOptions.MapId == 1)
                        /* Impostor roles */ || roleInfo.roleId == RoleId.Mimic || roleInfo.roleId == RoleId.Painter || roleInfo.roleId == RoleId.Demon || roleInfo.roleId == RoleId.Janitor || roleInfo.roleId == RoleId.Ilusionist || roleInfo.roleId == RoleId.Manipulator || roleInfo.roleId == RoleId.Bomberman || roleInfo.roleId == RoleId.Chameleon || roleInfo.roleId == RoleId.Gambler || roleInfo.roleId == RoleId.Sorcerer || roleInfo.roleId == RoleId.Impostor
                        /* Modifiers*/ || roleInfo.roleId == RoleId.BigChungus
                        /* Capture the flag players*/ || roleInfo.roleId == RoleId.RedPlayer01 || roleInfo.roleId == RoleId.RedPlayer02 || roleInfo.roleId == RoleId.RedPlayer03 || roleInfo.roleId == RoleId.RedPlayer04 || roleInfo.roleId == RoleId.RedPlayer05 || roleInfo.roleId == RoleId.RedPlayer06 || roleInfo.roleId == RoleId.RedPlayer07 || roleInfo.roleId == RoleId.BluePlayer01 || roleInfo.roleId == RoleId.BluePlayer02 || roleInfo.roleId == RoleId.BluePlayer03 || roleInfo.roleId == RoleId.BluePlayer04 || roleInfo.roleId == RoleId.BluePlayer05 || roleInfo.roleId == RoleId.BluePlayer06 || roleInfo.roleId == RoleId.BluePlayer07 || roleInfo.roleId == RoleId.StealerPlayer
                        /* Police and Thief players*/ || roleInfo.roleId == RoleId.PolicePlayer01 || roleInfo.roleId == RoleId.PolicePlayer02 || roleInfo.roleId == RoleId.PolicePlayer03 || roleInfo.roleId == RoleId.PolicePlayer04 || roleInfo.roleId == RoleId.PolicePlayer05 || roleInfo.roleId == RoleId.ThiefPlayer01 || roleInfo.roleId == RoleId.ThiefPlayer02 || roleInfo.roleId == RoleId.ThiefPlayer03 || roleInfo.roleId == RoleId.ThiefPlayer04 || roleInfo.roleId == RoleId.ThiefPlayer05 || roleInfo.roleId == RoleId.ThiefPlayer06 || roleInfo.roleId == RoleId.ThiefPlayer07 || roleInfo.roleId == RoleId.ThiefPlayer08 || roleInfo.roleId == RoleId.ThiefPlayer09 || roleInfo.roleId == RoleId.ThiefPlayer10
                        /* King of the hill*/ || roleInfo.roleId == RoleId.GreenKing || roleInfo.roleId == RoleId.YellowKing || roleInfo.roleId == RoleId.GreenPlayer01 || roleInfo.roleId == RoleId.GreenPlayer02 || roleInfo.roleId == RoleId.GreenPlayer03 || roleInfo.roleId == RoleId.GreenPlayer04 || roleInfo.roleId == RoleId.GreenPlayer05 || roleInfo.roleId == RoleId.GreenPlayer06 || roleInfo.roleId == RoleId.YellowPlayer01 || roleInfo.roleId == RoleId.YellowPlayer02 || roleInfo.roleId == RoleId.YellowPlayer03 || roleInfo.roleId == RoleId.YellowPlayer04 || roleInfo.roleId == RoleId.YellowPlayer05 || roleInfo.roleId == RoleId.YellowPlayer06 || roleInfo.roleId == RoleId.UsurperPlayer
                        /* Hot Potato */ || roleInfo.roleId == RoleId.HotPotato || roleInfo.roleId == RoleId.NotPotato01 || roleInfo.roleId == RoleId.NotPotato02 || roleInfo.roleId == RoleId.NotPotato03 || roleInfo.roleId == RoleId.NotPotato04 || roleInfo.roleId == RoleId.NotPotato05 || roleInfo.roleId == RoleId.NotPotato06 || roleInfo.roleId == RoleId.NotPotato07 || roleInfo.roleId == RoleId.NotPotato08 || roleInfo.roleId == RoleId.NotPotato09 || roleInfo.roleId == RoleId.NotPotato10 || roleInfo.roleId == RoleId.NotPotato11 || roleInfo.roleId == RoleId.NotPotato12 || roleInfo.roleId == RoleId.NotPotato13 || roleInfo.roleId == RoleId.NotPotato14
                        /* Exploded Potato */ || roleInfo.roleId == RoleId.ExplodedPotato01 || roleInfo.roleId == RoleId.ExplodedPotato02 || roleInfo.roleId == RoleId.ExplodedPotato03 || roleInfo.roleId == RoleId.ExplodedPotato04 || roleInfo.roleId == RoleId.ExplodedPotato05 || roleInfo.roleId == RoleId.ExplodedPotato06 || roleInfo.roleId == RoleId.ExplodedPotato07 || roleInfo.roleId == RoleId.ExplodedPotato08 || roleInfo.roleId == RoleId.ExplodedPotato09 || roleInfo.roleId == RoleId.ExplodedPotato10 || roleInfo.roleId == RoleId.ExplodedPotato11 || roleInfo.roleId == RoleId.ExplodedPotato12 || roleInfo.roleId == RoleId.ExplodedPotato13 || roleInfo.roleId == RoleId.ExplodedPotato14
                        /* Zombie Laboratory */ || roleInfo.roleId == RoleId.NursePlayer || roleInfo.roleId == RoleId.ZombiePlayer01 || roleInfo.roleId == RoleId.ZombiePlayer02 || roleInfo.roleId == RoleId.ZombiePlayer03 || roleInfo.roleId == RoleId.ZombiePlayer04 || roleInfo.roleId == RoleId.ZombiePlayer05 || roleInfo.roleId == RoleId.ZombiePlayer06 || roleInfo.roleId == RoleId.ZombiePlayer07 || roleInfo.roleId == RoleId.ZombiePlayer08 || roleInfo.roleId == RoleId.ZombiePlayer09 || roleInfo.roleId == RoleId.ZombiePlayer10 || roleInfo.roleId == RoleId.ZombiePlayer11 || roleInfo.roleId == RoleId.ZombiePlayer12 || roleInfo.roleId == RoleId.ZombiePlayer13 || roleInfo.roleId == RoleId.ZombiePlayer14 || roleInfo.roleId == RoleId.SurvivorPlayer01 || roleInfo.roleId == RoleId.SurvivorPlayer02 || roleInfo.roleId == RoleId.SurvivorPlayer03 || roleInfo.roleId == RoleId.SurvivorPlayer04 || roleInfo.roleId == RoleId.SurvivorPlayer05 || roleInfo.roleId == RoleId.SurvivorPlayer06 || roleInfo.roleId == RoleId.SurvivorPlayer07 || roleInfo.roleId == RoleId.SurvivorPlayer08 || roleInfo.roleId == RoleId.SurvivorPlayer09 || roleInfo.roleId == RoleId.SurvivorPlayer10 || roleInfo.roleId == RoleId.SurvivorPlayer11 || roleInfo.roleId == RoleId.SurvivorPlayer12 || roleInfo.roleId == RoleId.SurvivorPlayer13)
                        continue;

                    // Only add current game roles
                    if (roleInfo.roleId != RoleInfo.getRoleInfoForPlayer(player).FirstOrDefault().roleId) {
                        continue;
                    }

                    Transform buttonParent = (new GameObject()).transform;
                    buttonParent.SetParent(container);
                    Transform button = UnityEngine.Object.Instantiate(buttonTemplate, buttonParent);
                    Transform buttonMask = UnityEngine.Object.Instantiate(maskTemplate, buttonParent);
                    TMPro.TextMeshPro label = UnityEngine.Object.Instantiate(textTemplate, button);
                    button.GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.GamblerPlate.png", 90f);
                    buttons.Add(button);
                    int row = i / 5, col = i % 5;
                    buttonParent.localPosition = new Vector3(-3.47f + 1.75f * col, 1.5f - 0.45f * row, -5);
                    buttonParent.localScale = new Vector3(0.55f, 0.55f, 1f);
                    label.text = Helpers.cs(roleInfo.color, roleInfo.name);
                    label.alignment = TMPro.TextAlignmentOptions.Center;
                    label.transform.localPosition = new Vector3(0, 0, label.transform.localPosition.z);
                    label.transform.localScale *= 1.7f;
                    int copiedIndex = i;

                    button.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
                    if (!PlayerControl.LocalPlayer.Data.IsDead) button.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() => {
                        if (selectedButton != button) {
                            selectedButton = button;
                            buttons.ForEach(x => x.GetComponent<SpriteRenderer>().color = x == selectedButton ? Color.red : Color.white);
                        }
                        else {
                            PlayerControl target = Helpers.playerById((byte)__instance.playerStates[buttonTarget].TargetPlayerId);
                            if (!(__instance.state == MeetingHud.VoteStates.Voted || __instance.state == MeetingHud.VoteStates.NotVoted) || target == null || Gambler.numberOfShots <= 0) return;

                            if (!Gambler.canKillThroughShield && target == Squire.shielded) { // If can't bypass shields, notifiy everyone about the kill attempt and close the window without lossing a shoot opportunity
                                __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                                UnityEngine.Object.Destroy(container.gameObject);

                                MessageWriter murderAttemptWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ShieldedMurderAttempt, Hazel.SendOption.Reliable, -1);
                                AmongUsClient.Instance.FinishRpcImmediately(murderAttemptWriter);
                                RPCProcedure.shieldedMurderAttempt();
                                return;
                            }

                            var mainRoleInfo = RoleInfo.getRoleInfoForPlayer(target).FirstOrDefault();
                            if (mainRoleInfo == null) return;

                            if (Gambler.gambler.Data.Role.IsImpostor && target.Data.Role.IsImpostor) {
                                target = PlayerControl.LocalPlayer;
                            }
                            else {
                                target = (mainRoleInfo == roleInfo) ? target : PlayerControl.LocalPlayer;

                                // Reset the GUI
                                __instance.playerStates.ToList().ForEach(x => x.gameObject.SetActive(true));
                                UnityEngine.Object.Destroy(container.gameObject);
                                if (Gambler.canShootMultipleTimes && Gambler.numberOfShots > 1 && target != PlayerControl.LocalPlayer)
                                    __instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == target.PlayerId && x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject); });
                                else
                                    __instance.playerStates.ToList().ForEach(x => { if (x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject); });
                            }

                            // Shoot player and send chat info if activated
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.GamblerShoot, Hazel.SendOption.Reliable, -1);
                            writer.Write(target.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.gamblerShoot(target.PlayerId);
                        }
                    }));

                    i++;
                }
            }
            container.transform.localScale *= 0.75f;
        }

        [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.Select))]
        class PlayerVoteAreaSelectPatch {
            static bool Prefix(MeetingHud __instance) {
                return !(PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer == Gambler.gambler && gamblerUI != null);
            }
        }


        static void populateButtonsPostfix(MeetingHud __instance) {
            // Add Cheater Buttons
            if (Cheater.cheater != null && PlayerControl.LocalPlayer == Cheater.cheater && !Cheater.cheater.Data.IsDead) {
                selections = new bool[__instance.playerStates.Length];
                renderers = new SpriteRenderer[__instance.playerStates.Length];

                for (int i = 0; i < __instance.playerStates.Length; i++) {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.AmDead || (playerVoteArea.TargetPlayerId == Cheater.cheater.PlayerId && !Cheater.canOnlyCheatOthers)) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject checkbox = UnityEngine.Object.Instantiate(template);
                    checkbox.transform.SetParent(playerVoteArea.transform);
                    checkbox.transform.position = template.transform.position;
                    checkbox.transform.localPosition = new Vector3(-0.95f, 0.03f, -1.3f);
                    SpriteRenderer renderer = checkbox.GetComponent<SpriteRenderer>();
                    renderer.sprite = Cheater.getCheckSprite();
                    renderer.color = Color.red;

                    PassiveButton button = checkbox.GetComponent<PassiveButton>();
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((System.Action)(() => cheaterOnClick(copiedIndex, __instance)));
                    selections[i] = false;
                    renderers[i] = renderer;
                }
            }

            //Fix visor in Meetings 
            foreach (PlayerVoteArea pva in __instance.playerStates) {
                if(pva.PlayerIcon != null && pva.PlayerIcon.VisorSlot != null){
                    pva.PlayerIcon.VisorSlot.transform.position += new Vector3(0, 0, -1f);
                }
            }

            // Add overlay for spelled players
            if (Sorcerer.sorcerer != null && Sorcerer.spelledPlayers != null) {
                foreach (PlayerVoteArea pva in __instance.playerStates) {
                    if (Sorcerer.spelledPlayers.Any(x => x.PlayerId == pva.TargetPlayerId)) {
                        SpriteRenderer rend = (new GameObject()).AddComponent<SpriteRenderer>();
                        rend.transform.SetParent(pva.transform);
                        rend.gameObject.layer = pva.Megaphone.gameObject.layer;
                        rend.transform.localPosition = new Vector3(-0.5f, -0.03f, -1f);
                        rend.sprite = Sorcerer.getSpelledMeetingSprite();
                    }
                }
            }

            // Add Gambler Buttons
            if (Gambler.gambler != null && PlayerControl.LocalPlayer == Gambler.gambler && !Gambler.gambler.Data.IsDead && Gambler.numberOfShots > 0) {
                for (int i = 0; i < __instance.playerStates.Length; i++) {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    if (playerVoteArea.AmDead || playerVoteArea.TargetPlayerId == PlayerControl.LocalPlayer.PlayerId) continue;

                    GameObject template = playerVoteArea.Buttons.transform.Find("CancelButton").gameObject;
                    GameObject targetBox = UnityEngine.Object.Instantiate(template, playerVoteArea.transform);
                    targetBox.name = "ShootButton";
                    targetBox.transform.localPosition = new Vector3(-0.95f, 0.03f, -1.3f);
                    SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
                    renderer.sprite = Gambler.getTargetSprite();
                    PassiveButton button = targetBox.GetComponent<PassiveButton>();
                    button.OnClick.RemoveAllListeners();
                    int copiedIndex = i;
                    button.OnClick.AddListener((System.Action)(() => gamblerOnClick(copiedIndex, __instance)));
                }
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.ServerStart))]
        class MeetingServerStartPatch {
            static void Postfix(MeetingHud __instance)
            {
                populateButtonsPostfix(__instance);
            }
        }

        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Deserialize))]
        class MeetingDeserializePatch {
            static void Postfix(MeetingHud __instance, [HarmonyArgument(0)]MessageReader reader, [HarmonyArgument(1)]bool initialState)
            {
                // Add Cheater buttons
                if (initialState) {
                    populateButtonsPostfix(__instance);
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CoStartMeeting))]
        class StartMeetingPatch {
            public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)]GameData.PlayerInfo meetingTarget) {
                // Forensic meeting start time
                Forensic.meetingStartTime = DateTime.UtcNow;
                // Reset demon bitten
                Demon.bitten = null;                
                // Save the meeting target
                target = meetingTarget;

                // Reset camera zoom for Fink when meeting start
                if (Fink.fink != null) {
                    Fink.resetCamera();
                }

                // Reset medusa target
                Medusa.petrified = null;
            }
        }
    }
}
