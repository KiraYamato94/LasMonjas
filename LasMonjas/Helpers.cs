using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using static LasMonjas.LasMonjas;
using LasMonjas.Core;
using HarmonyLib;
using Hazel;
using LasMonjas.Objects;
using LasMonjas.Patches;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using AmongUs.GameOptions;
using Il2CppInterop.Runtime;

namespace LasMonjas
{

    public enum MurderAttemptResult {
        PerformKill,
        SuppressKill,
        JinxKill
    }
    public static class Helpers {

        public static Sprite loadSpriteFromResources(string path, float pixelsPerUnit) {
            try {
                Texture2D texture = loadTextureFromResources(path);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            } catch {
                //System.Console.WriteLine("Error loading sprite from path: " + path);
            }
            return null;
        }

        public static unsafe Texture2D loadTextureFromResources(string path) {
            try {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var length = stream.Length;
                var byteTexture = new Il2CppStructArray<byte>(length);
                stream.Read(new Span<byte>(IntPtr.Add(byteTexture.Pointer, IntPtr.Size * 4).ToPointer(), (int)length));
                ImageConversion.LoadImage(texture, byteTexture, false);
                return texture;
            } catch {
                //System.Console.WriteLine("Error loading texture from resources: " + path);
            }
            return null;
        }

        public static Texture2D loadTextureFromDisk(string path) {
            try {          
                if (File.Exists(path))     {
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                    var byteTexture = Il2CppSystem.IO.File.ReadAllBytes(path); 
                    ImageConversion.LoadImage(texture, byteTexture, false);
                    return texture;
                }
            } catch {
                //System.Console.WriteLine("Error loading texture from disk: " + path);
            }
            return null;
        }

        public static PlayerControl playerById(byte id)
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                if (player.PlayerId == id)
                    return player;
            return null;
        }

        public static Dictionary<byte, PlayerControl> allPlayersById()
        {
            Dictionary<byte, PlayerControl> res = new Dictionary<byte, PlayerControl>();
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                res.Add(player.PlayerId, player);
            return res;
        }

        public static void handleDemonBiteOnBodyReport() {
            // Murder the bitten player and reset bitten (regardless whether the kill was successful or not)
            Helpers.checkMurderAttemptAndKill(Demon.demon, Demon.bitten, true, false);
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.DemonSetBitten, Hazel.SendOption.Reliable, -1);
            writer.Write(byte.MaxValue);
            writer.Write(byte.MaxValue);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.demonSetBitten(byte.MaxValue, byte.MaxValue);
        }

        public static void refreshRoleDescription(PlayerControl player) {
            if (player == null) return;

            List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(player); 

            var toRemove = new List<PlayerTask>();
            foreach (PlayerTask t in player.myTasks) {
                var textTask = t.gameObject.GetComponent<ImportantTextTask>();
                if (textTask != null) {
                    var info = infos.FirstOrDefault(x => textTask.Text.StartsWith(x.name));
                    if (info != null)
                        infos.Remove(info); // TextTask for this RoleInfo does not have to be added, as it already exists
                    else
                        toRemove.Add(t); // TextTask does not have a corresponding RoleInfo and will hence be deleted
                }
            }

            foreach (PlayerTask t in toRemove) {
                t.OnRemove();
                player.myTasks.Remove(t);
                UnityEngine.Object.Destroy(t.gameObject);
            }

            // Add TextTask for remaining RoleInfos
            foreach (RoleInfo roleInfo in infos) {
                var task = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                task.transform.SetParent(player.transform, false);

                if (roleInfo.name == Language.roleInfoRoleNames[15]) {
                    var getMinionText = Renegade.canRecruitMinion ? Language.helpersTexts[0] : "";
                    task.Text = cs(roleInfo.color, $"{roleInfo.name}: {Language.helpersTexts[1]}{getMinionText}");
                } else {
                    task.Text = cs(roleInfo.color, $"{roleInfo.name}: {roleInfo.shortDescription}");
                }

                player.myTasks.Insert(0, task);
            }
        }

        public static bool isLighterColor(int colorId) {
            return CustomColors.lighterColors.Contains(colorId);
        }

        //Fake tasks for neutral and rebel team
        public static bool hasFakeTasks(this PlayerControl player) {
            return (player == Joker.joker || player == RoleThief.rolethief || player == Pyromaniac.pyromaniac || player == TreasureHunter.treasureHunter || player == Devourer.devourer || player == Poisoner.poisoner || player == Puppeteer.puppeteer || player == Exiler.exiler || player == Amnesiac.amnesiac || player == Seeker.seeker || player == Renegade.renegade || player == Minion.minion || player == BountyHunter.bountyhunter || player == Trapper.trapper || player == Yinyanger.yinyanger || player == Challenger.challenger || player == Ninja.ninja || player == Berserker.berserker || player == Yandere.yandere || player == Stranded.stranded || player == Monja.monja || Renegade.formerRenegades.Any(x => x == player));
        }

        public static void clearAllTasks(this PlayerControl player) {
            if (player == null) return;
            foreach (var playerTask in player.myTasks) {
                playerTask.OnRemove();
                UnityEngine.Object.Destroy(playerTask.gameObject);
            }
            player.myTasks.Clear();

            if (player.Data != null && player.Data.Tasks != null)
                player.Data.Tasks.Clear();
        }

        public static void setSemiTransparent(this PoolablePlayer player, bool value) {
            float alpha = value ? 0.25f : 1f;
            foreach (SpriteRenderer r in player.gameObject.GetComponentsInChildren<SpriteRenderer>())
                r.color = new Color(r.color.r, r.color.g, r.color.b, alpha);
            player.cosmetics.nameText.color = new Color(player.cosmetics.nameText.color.r, player.cosmetics.nameText.color.g, player.cosmetics.nameText.color.b, alpha);
        }

        public static string GetString(this TranslationController t, StringNames key, params Il2CppSystem.Object[] parts) {
            return t.GetString(key, parts);
        }

        public static string cs(Color c, string s) {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }
 
        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static KeyValuePair<byte, int> MaxPair(this Dictionary<byte, int> self, out bool tie) {
            tie = true;
            KeyValuePair<byte, int> result = new KeyValuePair<byte, int>(byte.MaxValue, int.MinValue);
            foreach (KeyValuePair<byte, int> keyValuePair in self)
            {
                if (keyValuePair.Value > result.Value)
                {
                    result = keyValuePair;
                    tie = false;
                }
                else if (keyValuePair.Value == result.Value)
                {
                    tie = true;
                }
            }
            return result;
        }
        public static bool hidePlayerName(PlayerControl source, PlayerControl target) {
            if (source == target) return false;
            if (source == null || target == null) return true;
            if (source.Data.IsDead) return false;
            if (target.Data.IsDead) return true;
            if (Painter.painterTimer > 0f) return true; // No names are visible          
            if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started && ShipStatus.Instance != null && source.transform != null && target.transform != null) {
                float distMod = 1.025f;
                float distance = Vector3.Distance(source.transform.position, target.transform.position);
                bool anythingBetween = PhysicsHelpers.AnythingBetween(source.transform.position, target.transform.position, Constants.ShadowMask, false);

                if (distance > ShipStatus.Instance.CalculateLightRadius(source.Data) * distMod || anythingBetween) return true;
            }
            if (!MapOptions.hidePlayerNames) return false; // All names are visible
            if (source.Data.Role.IsImpostor && target.Data.Role.IsImpostor) return false; // Members of team Impostors see the names of Impostors
            if (source.getPartner() == target) return false; // Members of team Lovers see the names of each other
            if ((source == Renegade.renegade || source == Minion.minion) && (target == Renegade.renegade || target == Minion.minion || target == Renegade.fakeMinion)) return false; // Members of team Renegade see the names of each other
            return true;
        }

        public static void setDefaultLook(this PlayerControl target) {
            target.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void setLook(this PlayerControl target, String playerName, int colorId, string hatId, string visorId, string skinId, string petId) {
            target.RawSetColor(colorId);
            target.RawSetVisor(visorId, colorId);
            target.RawSetHat(hatId, colorId);
            target.RawSetName(hidePlayerName(PlayerControl.LocalPlayer, target) ? "" : playerName);
            target.RawSetPet(petId, colorId);

            SkinViewData nextSkin = DestroyableSingleton<HatManager>.Instance.GetSkinById(skinId).viewData.viewData;
            PlayerPhysics playerPhysics = target.MyPhysics;
            AnimationClip clip = null;
            var spriteAnim = playerPhysics.myPlayer.cosmetics.skin.animator;
            var currentPhysicsAnim = playerPhysics.Animations.Animator;
            if (currentPhysicsAnim == playerPhysics.Animations.group.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;

            float progress = playerPhysics.Animations.Animator.GetNormalisedTime();
            playerPhysics.myPlayer.cosmetics.skin.skin = nextSkin;
            playerPhysics.myPlayer.cosmetics.skin.UpdateMaterial();
            spriteAnim.Play(clip, 1f);
            spriteAnim.m_animator.Play("a", 0, progress % 1);
            spriteAnim.m_animator.Update(0f);

            if (target.cosmetics.currentPet) UnityEngine.Object.Destroy(target.cosmetics.currentPet.gameObject);
            target.cosmetics.currentPet = UnityEngine.Object.Instantiate<PetBehaviour>(DestroyableSingleton<HatManager>.Instance.GetPetById(petId).viewData.viewData);
            target.cosmetics.currentPet.transform.position = target.transform.position;
            target.cosmetics.currentPet.Source = target;
            target.cosmetics.currentPet.Visible = target.Visible;
            target.SetPlayerMaterialColors(target.cosmetics.currentPet.rend);
        }

        public static bool roleCanUseVents(this PlayerControl player) {
            bool roleCouldUse = false;
            if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek) {
                if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor) {
                    roleCouldUse = true;
                }
            } else if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                switch (gameType) {
                    case 0:
                    case 1:
                        if (Monja.awakened) {
                            roleCouldUse = false;
                        }
                        else {
                            if (Chameleon.chameleon != null && Chameleon.chameleon == player)
                                roleCouldUse = false;
                            else if (Janitor.janitor != null && Janitor.janitor == player && Janitor.dragginBody)
                                roleCouldUse = false;
                            else if (Renegade.canUseVents && Renegade.renegade != null && Renegade.renegade == player)
                                roleCouldUse = true;
                            else if (Renegade.canUseVents && Minion.minion != null && Minion.minion == player)
                                roleCouldUse = true;
                            else if (Stranded.canVent && Stranded.invisibleTimer <= 0f && Stranded.stranded != null && Stranded.stranded == player)
                                roleCouldUse = true;
                            else if (player.Data.Role.IsImpostor) {
                                roleCouldUse = true;
                            }
                        }
                        break;
                    case 2:
                        // CTF:
                        if (PlayerControl.LocalPlayer != CaptureTheFlag.bluePlayerWhoHasRedFlag && PlayerControl.LocalPlayer != CaptureTheFlag.redPlayerWhoHasBlueFlag
                                && (PlayerControl.LocalPlayer == CaptureTheFlag.redplayer01 && !CaptureTheFlag.redplayer01IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer02 && !CaptureTheFlag.redplayer02IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer03 && !CaptureTheFlag.redplayer03IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer04 && !CaptureTheFlag.redplayer04IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer05 && !CaptureTheFlag.redplayer05IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer06 && !CaptureTheFlag.redplayer06IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.redplayer07 && !CaptureTheFlag.redplayer07IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer01 && !CaptureTheFlag.blueplayer01IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer02 && !CaptureTheFlag.blueplayer02IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer03 && !CaptureTheFlag.blueplayer03IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer04 && !CaptureTheFlag.blueplayer04IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer05 && !CaptureTheFlag.blueplayer05IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer06 && !CaptureTheFlag.blueplayer06IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.blueplayer07 && !CaptureTheFlag.blueplayer07IsReviving
                                || PlayerControl.LocalPlayer == CaptureTheFlag.stealerPlayer && !CaptureTheFlag.stealerPlayerIsReviving)) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 3:
                        // PT:
                        if (PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer01 && !PoliceAndThief.thiefplayer01IsStealing && !PoliceAndThief.thiefplayer01IsReviving
                                || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer02 && !PoliceAndThief.thiefplayer02IsStealing && !PoliceAndThief.thiefplayer02IsReviving
                                || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer03 && !PoliceAndThief.thiefplayer03IsStealing && !PoliceAndThief.thiefplayer03IsReviving
                                || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer04 && !PoliceAndThief.thiefplayer04IsStealing && !PoliceAndThief.thiefplayer04IsReviving
                                || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer05 && !PoliceAndThief.thiefplayer05IsStealing && !PoliceAndThief.thiefplayer05IsReviving
                                || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer06 && !PoliceAndThief.thiefplayer06IsStealing && !PoliceAndThief.thiefplayer06IsReviving
                                || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer07 && !PoliceAndThief.thiefplayer07IsStealing && !PoliceAndThief.thiefplayer07IsReviving
                                || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer08 && !PoliceAndThief.thiefplayer08IsStealing && !PoliceAndThief.thiefplayer08IsReviving
                                || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer09 && !PoliceAndThief.thiefplayer09IsStealing && !PoliceAndThief.thiefplayer09IsReviving) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 4:
                        // KOTH:
                        if (PlayerControl.LocalPlayer != KingOfTheHill.greenKingplayer && PlayerControl.LocalPlayer != KingOfTheHill.yellowKingplayer
                                && (PlayerControl.LocalPlayer == KingOfTheHill.greenplayer01 && !KingOfTheHill.greenplayer01IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer02 && !KingOfTheHill.greenplayer02IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer03 && !KingOfTheHill.greenplayer03IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer04 && !KingOfTheHill.greenplayer04IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer05 && !KingOfTheHill.greenplayer05IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.greenplayer06 && !KingOfTheHill.greenplayer06IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer01 && !KingOfTheHill.yellowplayer01IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer02 && !KingOfTheHill.yellowplayer02IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer03 && !KingOfTheHill.yellowplayer03IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer04 && !KingOfTheHill.yellowplayer04IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer05 && !KingOfTheHill.yellowplayer05IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.yellowplayer06 && !KingOfTheHill.yellowplayer06IsReviving
                                || PlayerControl.LocalPlayer == KingOfTheHill.usurperPlayer && !KingOfTheHill.usurperPlayerIsReviving)) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 5:
                        // HP:
                        if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 6:
                        // ZL:
                        if (PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer01 && !ZombieLaboratory.zombiePlayer01IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer02 && !ZombieLaboratory.zombiePlayer02IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer03 && !ZombieLaboratory.zombiePlayer03IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer04 && !ZombieLaboratory.zombiePlayer04IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer05 && !ZombieLaboratory.zombiePlayer05IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer06 && !ZombieLaboratory.zombiePlayer06IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer07 && !ZombieLaboratory.zombiePlayer07IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer08 && !ZombieLaboratory.zombiePlayer08IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer09 && !ZombieLaboratory.zombiePlayer09IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer10 && !ZombieLaboratory.zombiePlayer10IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer11 && !ZombieLaboratory.zombiePlayer11IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer12 && !ZombieLaboratory.zombiePlayer12IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer13 && !ZombieLaboratory.zombiePlayer13IsReviving
                                || PlayerControl.LocalPlayer == ZombieLaboratory.zombiePlayer14 && !ZombieLaboratory.zombiePlayer14IsReviving) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 7:
                        // BR:
                        roleCouldUse = false;
                        break;
                    case 8:
                        // MF:
                        if (PlayerControl.LocalPlayer == MonjaFestival.bigMonjaPlayer) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                }
            }
            return roleCouldUse;
        }

        public static MurderAttemptResult checkMurderAttempt(PlayerControl killer, PlayerControl target, bool blockRewind = false) {
            // Modified vanilla checks
            if (AmongUsClient.Instance.IsGameOver) return MurderAttemptResult.SuppressKill;
            if (killer == null || killer.Data == null || killer.Data.IsDead || killer.Data.Disconnected) return MurderAttemptResult.SuppressKill; // Allow non Impostor kills compared to vanilla code
            if (target == null || target.Data == null || target.Data.IsDead || target.Data.Disconnected) return MurderAttemptResult.SuppressKill; // Allow killing players in vents compared to vanilla code

            // Handle jinx shot
            if (Jinx.jinxedList.Any(x => x.PlayerId == killer.PlayerId)) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                writer.Write(killer.PlayerId);
                writer.Write((byte)0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setJinxed(killer.PlayerId, 0);

                return MurderAttemptResult.JinxKill;
            }

            // Block impostor shielded kill
            else if (Squire.shielded != null && Squire.shielded == target) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.ShieldedMurderAttempt, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.shieldedMurderAttempt();
                return MurderAttemptResult.SuppressKill;
            }

            // Block impostor jailer kill and teleport the killer to prison
            else if (Jailer.jailedPlayer != null && Jailer.jailedPlayer == target) {
                List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(killer);
                RoleInfo roleInfo = infos.FirstOrDefault();
                if (killer.Data.Role.IsImpostor || roleInfo.isRebel) {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.PrisonPlayer, Hazel.SendOption.Reliable, -1);
                    writer.Write(killer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.prisonPlayer(killer.PlayerId);
                }
                return MurderAttemptResult.SuppressKill;
            }

            // Block TimeTraveler with time shield kill
            else if (TimeTraveler.shieldActive && TimeTraveler.timeTraveler != null && TimeTraveler.timeTraveler == target) {
                if (!blockRewind) { // Only rewind the attempt was not called because a meeting startet 
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.TimeTravelerRewindTime, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.timeTravelerRewindTime();
                }
                return MurderAttemptResult.SuppressKill;
            }

            else // Demon try nun kill
            if (Demon.bitten != null && !Demon.bitten.Data.IsDead) {
                if (Nun.nuns.Count == 0) {
                    return MurderAttemptResult.PerformKill;
                }
                else {
                    foreach (Nun nun in Nun.nuns) {
                        if (Vector2.Distance(nun.nun.transform.position, Demon.bitten.transform.position) < 2f) {
                            return MurderAttemptResult.SuppressKill;
                        }
                    }
                    return MurderAttemptResult.PerformKill;
                }
            }

            return MurderAttemptResult.PerformKill;
        }

        public static MurderAttemptResult checkMurderAttemptAndKill(PlayerControl killer, PlayerControl target, bool isMeetingStart = false, bool showAnimation = true) {
            // The local player checks for the validity of the kill and performs it afterwards (different to vanilla, where the host performs all the checks)
            // The kill attempt will be shared using a custom RPC, hence combining modded and unmodded versions is impossible

            MurderAttemptResult murder = checkMurderAttempt(killer, target, isMeetingStart);
            if (murder == MurderAttemptResult.PerformKill) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                writer.Write(killer.PlayerId);
                writer.Write(target.PlayerId);
                writer.Write(showAnimation ? Byte.MaxValue : 0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.uncheckedMurderPlayer(killer.PlayerId, target.PlayerId, showAnimation ? Byte.MaxValue : (byte)0);
            }
            return murder;
        }

        public static void Shuffle<T>(this IList<T> list) {
            System.Random rnd = new System.Random();
            for (var i = 0; i < list.Count; i++)
                list.Swap(i, rnd.Next(i, list.Count));
        }

        public static void Swap<T>(this IList<T> list, int i, int j) {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        public static object TryCast(this Il2CppObjectBase self, Type type) {
            return AccessTools.Method(self.GetType(), nameof(Il2CppObjectBase.TryCast)).MakeGenericMethod(type).Invoke(self, Array.Empty<object>());
        }

        public static bool AnySabotageActive(bool disableSubmergedMaskCheck = false) {
            if (disableSubmergedMaskCheck) {
                SubmergedCompatibility.DisableO2MaskCheckForEmergency = true;
            }

            foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks) {
                if (PlayerTask.TaskIsEmergency(task)) {
                    SubmergedCompatibility.DisableO2MaskCheckForEmergency = false;
                    return true;
                }
            }

            SubmergedCompatibility.DisableO2MaskCheckForEmergency = false;
            return false;
        }

        public static void enableCursor(string mode) {
            if (mode == "start") {
                Sprite sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Cursor.png", 115f);
                Cursor.SetCursor(sprite.texture, Vector2.zero, CursorMode.Auto);
                return;
            }
            if (LasMonjasPlugin.MonjaCursor.Value) {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            else {
                Sprite sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.Cursor.png", 115f);
                Cursor.SetCursor(sprite.texture, Vector2.zero, CursorMode.Auto);
            }
        }

        public static bool zoomOutStatus = false;
        public static void toggleZoom(bool reset = false) {

            float orthographicSize = reset || zoomOutStatus ? 3f : 12f;

            zoomOutStatus = !zoomOutStatus && !reset;
            Camera.main.orthographicSize = orthographicSize;
            foreach (var cam in Camera.allCameras) {
                if (cam != null && cam.gameObject.name == "UI Camera") cam.orthographicSize = orthographicSize;  // The UI is scaled too, else we cant click the buttons. Downside: map is super small.
            }
            if (HudManagerStartPatch.zoomOutButton != null) {
                HudManagerStartPatch.zoomOutButton.Sprite = zoomOutStatus ? Helpers.loadSpriteFromResources("LasMonjas.Images.PlusButton.png", 75f) : Helpers.loadSpriteFromResources("LasMonjas.Images.MinusButton.png", 150f);
                HudManagerStartPatch.zoomOutButton.PositionOffset = zoomOutStatus ? new Vector3(0f, 3f, 0) : new Vector3(0.4f, 2.8f, 0);
            }
            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height); // This will move button positions to the correct position.
        }

        public static AudioClip GetIntroSound(RoleTypes roleType) {
            return RoleManager.Instance.AllRoles.Where((role) => role.Role == roleType).FirstOrDefault().IntroSound;
        }

        public static void playEndMusic(int whichTeamMusic) {
            MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
            switch (whichTeamMusic) {
                case 3: // Neutrals
                    writermusic.Write(3);
                    AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                    RPCProcedure.changeMusic(3);
                    break;
                case 4: // Rebels
                    writermusic.Write(4);
                    AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                    RPCProcedure.changeMusic(4);
                    break;
                case 5: // Crewmates
                    writermusic.Write(5);
                    AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                    RPCProcedure.changeMusic(5);
                    break;
                case 6: // Impostors
                    writermusic.Write(6);
                    AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                    RPCProcedure.changeMusic(6);
                    break;
            }
        }

        public static void alphaPlayer(bool invisible, byte playerId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == playerId) {
                    if (invisible) {
                        player.cosmetics.nameText.color = new Color(player.cosmetics.nameText.color.r, player.cosmetics.nameText.color.g, player.cosmetics.nameText.color.b, 0.5f);
                        player.cosmetics.colorBlindText.color = new Color(player.cosmetics.colorBlindText.color.r, player.cosmetics.colorBlindText.color.g, player.cosmetics.colorBlindText.color.b, 0.5f);
                        if (player.cosmetics.currentPet != null && player.cosmetics.currentPet.rend != null && player.cosmetics.currentPet.shadowRend != null) {
                            player.cosmetics.currentPet.rend.color = new Color(player.cosmetics.currentPet.rend.color.r, player.cosmetics.currentPet.rend.color.g, player.cosmetics.currentPet.rend.color.b, 0.5f);
                            player.cosmetics.currentPet.shadowRend.color = new Color(player.cosmetics.currentPet.shadowRend.color.r, player.cosmetics.currentPet.shadowRend.color.g, player.cosmetics.currentPet.shadowRend.color.b, 0.5f);
                        }
                        if (player.cosmetics.hat != null) {
                            player.cosmetics.hat.Parent.color = new Color(player.cosmetics.hat.Parent.color.r, player.cosmetics.hat.Parent.color.g, player.cosmetics.hat.Parent.color.b, 0.5f);
                            player.cosmetics.hat.BackLayer.color = new Color(player.cosmetics.hat.BackLayer.color.r, player.cosmetics.hat.BackLayer.color.g, player.cosmetics.hat.BackLayer.color.b, 0.5f);
                            player.cosmetics.hat.FrontLayer.color = new Color(player.cosmetics.hat.FrontLayer.color.r, player.cosmetics.hat.FrontLayer.color.g, player.cosmetics.hat.FrontLayer.color.b, 0.5f);
                        }
                        if (player.cosmetics.visor != null) {
                            player.cosmetics.visor.Image.color = new Color(player.cosmetics.visor.Image.color.r, player.cosmetics.visor.Image.color.g, player.cosmetics.visor.Image.color.b, 0.5f);
                        }
                        player.MyPhysics.myPlayer.cosmetics.skin.layer.color = new Color(player.MyPhysics.myPlayer.cosmetics.skin.layer.color.r, player.MyPhysics.myPlayer.cosmetics.skin.layer.color.g, player.MyPhysics.myPlayer.cosmetics.skin.layer.color.b, 0.5f);
                    }
                    else {
                        player.cosmetics.nameText.color = new Color(player.cosmetics.nameText.color.r, player.cosmetics.nameText.color.g, player.cosmetics.nameText.color.b, 1f);
                        player.cosmetics.colorBlindText.color = new Color(player.cosmetics.colorBlindText.color.r, player.cosmetics.colorBlindText.color.g, player.cosmetics.colorBlindText.color.b, 1);
                        if (player.cosmetics.currentPet != null && player.cosmetics.currentPet.rend != null && player.cosmetics.currentPet.shadowRend != null) {
                            player.cosmetics.currentPet.rend.color = new Color(player.cosmetics.currentPet.rend.color.r, player.cosmetics.currentPet.rend.color.g, player.cosmetics.currentPet.rend.color.b, 1f);
                            player.cosmetics.currentPet.shadowRend.color = new Color(player.cosmetics.currentPet.shadowRend.color.r, player.cosmetics.currentPet.shadowRend.color.g, player.cosmetics.currentPet.shadowRend.color.b, 1f);
                        }
                        if (player.cosmetics.hat != null) {
                            player.cosmetics.hat.Parent.color = new Color(player.cosmetics.hat.Parent.color.r, player.cosmetics.hat.Parent.color.g, player.cosmetics.hat.Parent.color.b, 1f);
                            player.cosmetics.hat.BackLayer.color = new Color(player.cosmetics.hat.BackLayer.color.r, player.cosmetics.hat.BackLayer.color.g, player.cosmetics.hat.BackLayer.color.b, 1f);
                            player.cosmetics.hat.FrontLayer.color = new Color(player.cosmetics.hat.FrontLayer.color.r, player.cosmetics.hat.FrontLayer.color.g, player.cosmetics.hat.FrontLayer.color.b, 1f);
                        }
                        if (player.cosmetics.visor != null) {
                            player.cosmetics.visor.Image.color = new Color(player.cosmetics.visor.Image.color.r, player.cosmetics.visor.Image.color.g, player.cosmetics.visor.Image.color.b, 1f);
                        }
                        player.MyPhysics.myPlayer.cosmetics.skin.layer.color = new Color(player.MyPhysics.myPlayer.cosmetics.skin.layer.color.r, player.MyPhysics.myPlayer.cosmetics.skin.layer.color.g, player.MyPhysics.myPlayer.cosmetics.skin.layer.color.b, 1f);
                    }
                }
            }
        }

        public static void invisiblePlayer(byte playerId) {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (player.PlayerId == playerId) {
                    player.cosmetics.nameText.color = new Color(player.cosmetics.nameText.color.r, player.cosmetics.nameText.color.g, player.cosmetics.nameText.color.b, 0f);
                    player.cosmetics.colorBlindText.color = new Color(player.cosmetics.colorBlindText.color.r, player.cosmetics.colorBlindText.color.g, player.cosmetics.colorBlindText.color.b, 0);
                    if (player.cosmetics.currentPet != null && player.cosmetics.currentPet.rend != null && player.cosmetics.currentPet.shadowRend != null) {
                        player.cosmetics.currentPet.rend.color = new Color(player.cosmetics.currentPet.rend.color.r, player.cosmetics.currentPet.rend.color.g, player.cosmetics.currentPet.rend.color.b, 0f);
                        player.cosmetics.currentPet.shadowRend.color = new Color(player.cosmetics.currentPet.shadowRend.color.r, player.cosmetics.currentPet.shadowRend.color.g, player.cosmetics.currentPet.shadowRend.color.b, 0f);
                    }
                    if (player.cosmetics.hat != null) {
                        player.cosmetics.hat.Parent.color = new Color(player.cosmetics.hat.Parent.color.r, player.cosmetics.hat.Parent.color.g, player.cosmetics.hat.Parent.color.b, 0f);
                        player.cosmetics.hat.BackLayer.color = new Color(player.cosmetics.hat.BackLayer.color.r, player.cosmetics.hat.BackLayer.color.g, player.cosmetics.hat.BackLayer.color.b, 0f);
                        player.cosmetics.hat.FrontLayer.color = new Color(player.cosmetics.hat.FrontLayer.color.r, player.cosmetics.hat.FrontLayer.color.g, player.cosmetics.hat.FrontLayer.color.b, 0f);
                    }
                    if (player.cosmetics.visor != null) {
                        player.cosmetics.visor.Image.color = new Color(player.cosmetics.visor.Image.color.r, player.cosmetics.visor.Image.color.g, player.cosmetics.visor.Image.color.b, 0f);
                    }
                    player.MyPhysics.myPlayer.cosmetics.skin.layer.color = new Color(player.MyPhysics.myPlayer.cosmetics.skin.layer.color.r, player.MyPhysics.myPlayer.cosmetics.skin.layer.color.g, player.MyPhysics.myPlayer.cosmetics.skin.layer.color.b, 0f);
                }
            }
        }
        public static void turnIntoImpostor(PlayerControl player) {
            player.Data.Role.TeamType = RoleTeamTypes.Impostor;
            RoleManager.Instance.SetRole(player, RoleTypes.Impostor);
            player.SetKillTimer(GameOptionsManager.Instance.currentGameOptions.GetFloat(FloatOptionNames.KillCooldown));

            foreach (var localPlayer in PlayerControl.AllPlayerControls) {
                if (localPlayer.Data.Role.IsImpostor && PlayerControl.LocalPlayer.Data.Role.IsImpostor) {
                    player.cosmetics.nameText.color = Palette.ImpostorRed;
                }
            }
        }
        public static void turnIntoCrewmate(PlayerControl player) {
            player.Data.Role.TeamType = RoleTeamTypes.Crewmate;
            RoleManager.Instance.SetRole(player, RoleTypes.Crewmate);

            foreach (var localPlayer in PlayerControl.AllPlayerControls) {
                if (!localPlayer.Data.Role.IsImpostor && !PlayerControl.LocalPlayer.Data.Role.IsImpostor) {
                    player.cosmetics.nameText.color = Palette.CrewmateBlue;
                }
            }
        }

        public static void activateSenseiMap () {
            bool activeSensei = CustomOptionHolder.activateSenseiMap.getBool();

            int senseiMapHide = customSkeldHS; 

            if (GameOptionsManager.Instance.currentGameOptions.MapId == 0 && activatedSensei == false) {
                if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek && senseiMapHide >= 50 || GameOptionsManager.Instance.currentGameMode == GameModes.Normal && activeSensei) {
                    // Spawn map + assign shadow and materials layers
                    GameObject senseiMap = GameObject.Instantiate(CustomMain.customAssets.customMap, PlayerControl.LocalPlayer.transform.parent);
                    senseiMap.name = "HalconUI";
                    senseiMap.transform.position = new Vector3(-1.5f, -1.4f, 15.05f);
                    senseiMap.transform.GetChild(0).gameObject.layer = 9; // Ship Layer for HalconColisions
                    senseiMap.transform.GetChild(0).transform.GetChild(0).gameObject.layer = 11; // Object Layer for HalconShadows
                    senseiMap.transform.GetChild(0).transform.GetChild(1).gameObject.layer = 9; // Ship Layer for HalconAboveItems
                    Material shadowShader = null;
                    GameObject background = GameObject.Find("SkeldShip(Clone)/AdminHallway");
                    {
                        SpriteRenderer sp = background.GetComponent<SpriteRenderer>();
                        if (sp != null) {
                            shadowShader = sp.material;
                        }
                    }
                    {
                        SpriteRenderer sp = senseiMap.GetComponent<SpriteRenderer>();
                        if (sp != null && shadowShader != null) {
                            sp.material = shadowShader;
                            senseiMap.transform.GetChild(0).transform.GetChild(0).transform.GetComponent<SpriteRenderer>().material = shadowShader;
                        }
                    }

                    // Assign colliders objets, find halconCollisions to be the main parent
                    GameObject halconCollisions = senseiMap.transform.GetChild(0).transform.gameObject;

                    // Area colliders rebuilded for showing map names
                    GameObject colliderAdmin = GameObject.Find("SkeldShip(Clone)/Admin/Room");
                    colliderAdmin.transform.SetParent(halconCollisions.transform);
                    colliderAdmin.name = "RoomAdmin";
                    foreach (Collider2D c in colliderAdmin.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderAdmin.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myAdminpoints = { new Vector2(10.09f, -3.65f), new Vector2(1.96f, -3.65f), new Vector2(0.28f, -6.09f), new Vector2(3.97f, -10.45f), new Vector2(7.12f, -10.43f) };
                    colliderAdmin.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myAdminpoints;

                    GameObject colliderCafeteria = GameObject.Find("SkeldShip(Clone)/Cafeteria/Room");
                    colliderCafeteria.transform.SetParent(halconCollisions.transform);
                    colliderCafeteria.name = "RoomCafeteria";
                    foreach (Collider2D c in colliderCafeteria.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderCafeteria.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myCafeteriapoints = { new Vector2(4f, 3.35f), new Vector2(-2f, 3.35f), new Vector2(-2f, 4f), new Vector2(-4.5f, 6f), new Vector2(-4.5f, 0.55f), new Vector2(-2.8f, 0f), new Vector2(-2.8f, -2.64f), new Vector2(4, -2.64f) };
                    colliderCafeteria.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myCafeteriapoints;

                    GameObject colliderCockpit = GameObject.Find("SkeldShip(Clone)/Cockpit/Room");
                    colliderCockpit.transform.SetParent(halconCollisions.transform);
                    colliderCockpit.name = "RoomCookpit";
                    foreach (Collider2D c in colliderCockpit.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderCockpit.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myCockpitpoints = { new Vector2(5f, -10f), new Vector2(5f, -13f), new Vector2(8.5f, -13f), new Vector2(8.5f, -10f) };
                    colliderCockpit.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myCockpitpoints;

                    GameObject colliderWeapons = GameObject.Find("SkeldShip(Clone)/Weapons/Room");
                    colliderWeapons.transform.SetParent(halconCollisions.transform);
                    colliderWeapons.name = "RoomWeapons";
                    foreach (Collider2D c in colliderWeapons.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderWeapons.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myWeaponspoints = { new Vector2(12.5f, 0.5f), new Vector2(8.5f, 1.35f), new Vector2(8.5f, -3.5f), new Vector2(12.5f, -3.5f) };
                    colliderWeapons.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myWeaponspoints;

                    GameObject colliderLifeSupport = GameObject.Find("SkeldShip(Clone)/LifeSupport/Room");
                    colliderLifeSupport.transform.SetParent(halconCollisions.transform);
                    colliderLifeSupport.name = "RoomLifeSupport";
                    foreach (Collider2D c in colliderLifeSupport.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderLifeSupport.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myLifeSupportpoints = { new Vector2(-6.66f, 1.8f), new Vector2(-8.56f, 0.75f), new Vector2(-9.1f, 0.5f), new Vector2(-9.1f, -0.6f), new Vector2(-6.3f, -0.6f), new Vector2(-6.3f, 1.8f) };
                    colliderLifeSupport.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myLifeSupportpoints;

                    GameObject colliderShields = GameObject.Find("SkeldShip(Clone)/Shields/Room");
                    colliderShields.transform.SetParent(halconCollisions.transform);
                    colliderShields.name = "RoomShields";
                    foreach (Collider2D c in colliderShields.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderShields.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myShieldspoints = { new Vector2(4.3f, 0.3f), new Vector2(4.3f, -3.1f), new Vector2(8f, -3.1f), new Vector2(8f, 0.3f) };
                    colliderShields.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myShieldspoints;

                    GameObject colliderElectrical = GameObject.Find("SkeldShip(Clone)/Electrical/Room");
                    colliderElectrical.transform.SetParent(halconCollisions.transform);
                    colliderElectrical.name = "RoomElectrical";
                    foreach (Collider2D c in colliderElectrical.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderElectrical.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myElectricalpoints = { new Vector2(-3.9f, -9.54f), new Vector2(-3.9f, -6.69f), new Vector2(-6.7f, -6.69f), new Vector2(-6.7f, -9.54f), new Vector2(-7.3f, -9.54f), new Vector2(-7.3f, -12.9f), new Vector2(-3.39f, -12.9f), new Vector2(-3.39f, -9.54f) };
                    colliderElectrical.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myElectricalpoints;


                    GameObject colliderReactor = GameObject.Find("SkeldShip(Clone)/Reactor/Room");
                    colliderReactor.transform.SetParent(halconCollisions.transform);
                    colliderReactor.name = "RoomReactor";
                    foreach (Collider2D c in colliderReactor.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderReactor.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myReactorpoints = { new Vector2(-21, 2f), new Vector2(-21.5f, 0f), new Vector2(-21f, -4.2f), new Vector2(-12.6f, -2.79f), new Vector2(-12.85f, -1.25f), new Vector2(-12.6f, -0.1f) };
                    colliderReactor.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myReactorpoints;

                    GameObject colliderStorage = GameObject.Find("SkeldShip(Clone)/Storage/Room");
                    colliderStorage.transform.SetParent(halconCollisions.transform);
                    colliderStorage.name = "RoomStorage";
                    foreach (Collider2D c in colliderStorage.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderStorage.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myStoragepoints = { new Vector2(-11.2f, -5.7f), new Vector2(-17.4f, -9f), new Vector2(-14.91f, -11.23f), new Vector2(-15.19f, -11.61f), new Vector2(-12.46f, -13.07f), new Vector2(-9.13f, -14.07f), new Vector2(-8.78f, -13.24f), new Vector2(-7.38f, -13.24f), new Vector2(-7.4f, -9.52f), new Vector2(-7.2f, -9.52f), new Vector2(-7.2f, -7.2f) };
                    colliderStorage.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myStoragepoints;

                    GameObject colliderRightEngine = GameObject.Find("SkeldShip(Clone)/RightEngine/Room");
                    colliderRightEngine.transform.SetParent(halconCollisions.transform);
                    colliderRightEngine.name = "RoomRightEngine";
                    foreach (Collider2D c in colliderRightEngine.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderRightEngine.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myRightEnginepoints = { new Vector2(-20f, -4.5f), new Vector2(-19.15f, -6.95f), new Vector2(-16.8f, -8.9f), new Vector2(-11f, -5.1f), new Vector2(-11.75f, -4.75f), new Vector2(-12.65f, -3.25f) };
                    colliderRightEngine.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myRightEnginepoints;

                    GameObject colliderLeftEngine = GameObject.Find("SkeldShip(Clone)/LeftEngine/Room");
                    colliderLeftEngine.transform.SetParent(halconCollisions.transform);
                    colliderLeftEngine.name = "RoomLeftEngine";
                    foreach (Collider2D c in colliderLeftEngine.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderLeftEngine.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myLeftEnginepoints = { new Vector2(-16.68f, 7.17f), new Vector2(-18.86f, 4.95f), new Vector2(-20.28f, 2.03f), new Vector2(-12.84f, 0.3f), new Vector2(-11.93f, 1.85f), new Vector2(-10.87f, 2.85f) };
                    colliderLeftEngine.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myLeftEnginepoints;

                    GameObject colliderComms = GameObject.Find("SkeldShip(Clone)/Comms/Room");
                    colliderComms.transform.SetParent(halconCollisions.transform);
                    colliderComms.name = "RoomComms";
                    foreach (Collider2D c in colliderComms.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderComms.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myCommspoints = { new Vector2(4.3f, 4.5f), new Vector2(4.3f, 0.7f), new Vector2(8f, 0.7f), new Vector2(8f, 4.5f) };
                    colliderComms.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myCommspoints;

                    GameObject colliderSecurity = GameObject.Find("SkeldShip(Clone)/Security/Room");
                    colliderSecurity.transform.SetParent(halconCollisions.transform);
                    colliderSecurity.name = "RoomSecurity";
                    foreach (Collider2D c in colliderSecurity.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderSecurity.transform.position = new Vector3(0, 0, 0);
                    Vector2[] mySecuritypoints = { new Vector2(-7.9f, 10.3f), new Vector2(-7.9f, 8.25f), new Vector2(-3.75f, 8.25f), new Vector2(-3.75f, 10.3f) };
                    colliderSecurity.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = mySecuritypoints;

                    GameObject colliderMedical = GameObject.Find("SkeldShip(Clone)/Medical/Room");
                    colliderMedical.transform.SetParent(halconCollisions.transform);
                    colliderMedical.name = "RoomMedical";
                    foreach (Collider2D c in colliderMedical.GetComponents<Collider2D>()) {
                        c.enabled = false;
                    }
                    colliderMedical.transform.position = new Vector3(0, 0, 0);
                    Vector2[] myMedicalpoints = { new Vector2(-4.8f, 1.3f), new Vector2(-5.99f, 1.3f), new Vector2(-5.99f, -1.75f), new Vector2(-8.31f, -2.5f), new Vector2(-7.5f, -2.5f), new Vector2(-7.5f, -3.9f), new Vector2(-3.23f, -3.9f), new Vector2(-3.23f, -1.8f), new Vector2(-3.23f, -0.18f), new Vector2(-4.8f, -0.18f) };
                    colliderMedical.transform.GetChild(0).GetComponent<PolygonCollider2D>().points = myMedicalpoints;

                    // HullItems objects
                    GameObject halconHullItems = senseiMap.transform.GetChild(1).transform.gameObject; // find halconHullItems to the parent
                    GameObject skeldhatch0001 = GameObject.Find("hatch0001");
                    skeldhatch0001.transform.SetParent(halconHullItems.transform);
                    skeldhatch0001.transform.position = new Vector3(-10.33f, -14.025f, skeldhatch0001.transform.position.z);
                    GameObject skeldshieldborder_off = GameObject.Find("shieldborder_off");
                    skeldshieldborder_off.transform.SetParent(halconHullItems.transform);
                    skeldshieldborder_off.transform.position = new Vector3(10.85f, -6.2f, skeldshieldborder_off.transform.position.z);
                    GameObject skeldthruster0001lowestone = GameObject.Find("thruster0001 (1)");
                    skeldthruster0001lowestone.transform.SetParent(halconHullItems.transform);
                    skeldthruster0001lowestone.transform.position = new Vector3(-24.4f, -9.25f, skeldthruster0001lowestone.transform.position.z);
                    GameObject skeldthruster0001lowerone = GameObject.Find("thruster0001 (2)");
                    skeldthruster0001lowerone.transform.SetParent(halconHullItems.transform);
                    skeldthruster0001lowerone.transform.position = new Vector3(-25.75f, -6, skeldthruster0001lowerone.transform.position.z);
                    GameObject skeldthruster0001upperone = GameObject.Find("thruster0001");
                    skeldthruster0001upperone.transform.SetParent(halconHullItems.transform);
                    skeldthruster0001upperone.transform.position = new Vector3(-25.75f, 3.275f, skeldthruster0001upperone.transform.position.z);
                    GameObject skeldthruster0001higherone = GameObject.Find("thruster0001 (3)");
                    skeldthruster0001higherone.transform.SetParent(halconHullItems.transform);
                    skeldthruster0001higherone.transform.position = new Vector3(-24.4f, 5.9f, skeldthruster0001higherone.transform.position.z);
                    GameObject skeldthruster0001middleone = GameObject.Find("thrusterbig0001");
                    skeldthruster0001middleone.transform.SetParent(halconHullItems.transform);
                    skeldthruster0001middleone.transform.position = new Vector3(-28.15f, -2, skeldthruster0001middleone.transform.position.z);
                    GameObject skeldweapongun = GameObject.Find("WeaponGun");
                    skeldweapongun.transform.SetParent(halconHullItems.transform);
                    skeldweapongun.transform.position = new Vector3(16.5f, -1.865f, skeldweapongun.transform.position.z);
                    GameObject skeldlowershield = GameObject.Find("shield_off");
                    skeldlowershield.transform.SetParent(halconHullItems.transform);
                    skeldlowershield.transform.position = new Vector3(10.9f, -6.65f, skeldlowershield.transform.position.z);
                    GameObject skelduppershield = GameObject.Find("shield_off (1)");
                    skelduppershield.transform.SetParent(halconHullItems.transform);
                    skelduppershield.transform.position = new Vector3(10.8f, -5.85f, skelduppershield.transform.position.z);
                    GameObject skeldstarfield = GameObject.Find("starfield");
                    skeldstarfield.transform.SetParent(halconHullItems.transform);
                    skeldstarfield.transform.position = new Vector3(3, -4.5f, skeldstarfield.transform.position.z);

                    // Admin objects
                    GameObject halconAdmin = senseiMap.transform.GetChild(2).transform.gameObject; // find halconAdmin to be the parent
                    GameObject skeldAdminVent = GameObject.Find("AdminVent");
                    skeldAdminVent.transform.SetParent(halconAdmin.transform);
                    skeldAdminVent.transform.position = new Vector3(4.17f, -10.5f, skeldAdminVent.transform.position.z);
                    GameObject skeldadmintable = GameObject.Find("admin_bridge");
                    skeldadmintable.transform.SetParent(halconAdmin.transform);
                    skeldadmintable.transform.position = new Vector3(5.01f, -6.675f, skeldadmintable.transform.position.z);
                    GameObject skeldSwipeCardConsole = GameObject.Find("SwipeCardConsole");
                    skeldSwipeCardConsole.transform.SetParent(halconAdmin.transform);
                    skeldSwipeCardConsole.transform.position = new Vector3(6.07f, -6.575f, skeldSwipeCardConsole.transform.position.z);
                    GameObject skeldMapRoomConsole = GameObject.Find("MapRoomConsole");
                    skeldMapRoomConsole.transform.SetParent(halconAdmin.transform);
                    skeldMapRoomConsole.transform.position = new Vector3(3.95f, -6.575f, skeldMapRoomConsole.transform.position.z);
                    GameObject skeldLeftScreen = GameObject.Find("LeftScreen");
                    skeldLeftScreen.transform.SetParent(halconAdmin.transform);
                    skeldLeftScreen.transform.position = new Vector3(3.56f, -3.85f, skeldLeftScreen.transform.position.z);
                    GameObject skeldRightScreen = GameObject.Find("RightScreen");
                    skeldRightScreen.transform.SetParent(halconAdmin.transform);
                    skeldRightScreen.transform.position = new Vector3(5.55f, -3.85f, skeldRightScreen.transform.position.z);
                    GameObject skeldAdminUploadDataConsole = GameObject.Find("SkeldShip(Clone)/Admin/Ground/admin_walls/UploadDataConsole");
                    skeldAdminUploadDataConsole.transform.SetParent(halconAdmin.transform);
                    skeldAdminUploadDataConsole.transform.position = new Vector3(8.975f, -3.86f, skeldAdminUploadDataConsole.transform.position.z);
                    GameObject skeldAdminNoOxyConsole = GameObject.Find("SkeldShip(Clone)/Admin/Ground/admin_walls/NoOxyConsole");
                    skeldAdminNoOxyConsole.transform.SetParent(halconAdmin.transform);
                    skeldAdminNoOxyConsole.transform.position = new Vector3(2.65f, -4f, skeldAdminNoOxyConsole.transform.position.z);
                    GameObject skeldAdminFixWiringConsole = GameObject.Find("SkeldShip(Clone)/Admin/Ground/admin_walls/FixWiringConsole");
                    skeldAdminFixWiringConsole.transform.SetParent(halconAdmin.transform);
                    skeldAdminFixWiringConsole.transform.position = new Vector3(6.47f, -3.87f, skeldAdminFixWiringConsole.transform.position.z);
                    GameObject skeldmapComsChairs = GameObject.Find("map_ComsChairs");
                    skeldmapComsChairs.transform.SetParent(halconAdmin.transform);
                    skeldmapComsChairs.transform.position = new Vector3(4.585f, -4.38f, skeldmapComsChairs.transform.position.z);
                    skeldadmintable.transform.GetChild(0).gameObject.SetActive(false); // Deactivate map animation

                    // Cafeteria objects
                    GameObject halconCafeteria = senseiMap.transform.GetChild(3).transform.gameObject; // find halconCafeteria to be the parent
                    GameObject skeldCafeVent = GameObject.Find("CafeVent");
                    skeldCafeVent.transform.SetParent(halconCafeteria.transform);
                    skeldCafeVent.transform.position = new Vector3(-4.7f, 4, skeldCafeVent.transform.position.z);
                    GameObject skeldCafeGarbageConsole = GameObject.Find("SkeldShip(Clone)/Cafeteria/Ground/GarbageConsole");
                    skeldCafeGarbageConsole.transform.SetParent(halconCafeteria.transform);
                    skeldCafeGarbageConsole.transform.position = new Vector3(4.69f, 4, skeldCafeGarbageConsole.transform.position.z);
                    GameObject skeldCafeFixWiringConsole = GameObject.Find("SkeldShip(Clone)/Cafeteria/Ground/FixWiringConsole");
                    skeldCafeFixWiringConsole.transform.SetParent(halconCafeteria.transform);
                    skeldCafeFixWiringConsole.transform.position = new Vector3(-4.15f, 2.62f, skeldCafeFixWiringConsole.transform.position.z);
                    GameObject skeldCafeDataConsole = GameObject.Find("SkeldShip(Clone)/Cafeteria/Ground/DataConsole");
                    skeldCafeDataConsole.transform.SetParent(halconCafeteria.transform);
                    skeldCafeDataConsole.transform.position = new Vector3(-3.75f, 6.05f, skeldCafeDataConsole.transform.position.z);
                    GameObject skeldCafeEmergencyConsole = GameObject.Find("EmergencyConsole");
                    skeldCafeEmergencyConsole.transform.SetParent(halconCafeteria.transform);
                    skeldCafeEmergencyConsole.transform.position = new Vector3(-0.65f, 1, skeldCafeEmergencyConsole.transform.position.z);

                    // nav objects
                    GameObject halconCockpit = senseiMap.transform.GetChild(4).transform.gameObject; // find halconCockpit to be the parent
                    GameObject skeldNavVentNorth = GameObject.Find("NavVentNorth");
                    skeldNavVentNorth.transform.SetParent(halconCockpit.transform);
                    skeldNavVentNorth.transform.position = new Vector3(6.5f, -13.15f, skeldNavVentNorth.transform.position.z);
                    GameObject skeldNavVentSouth = GameObject.Find("NavVentSouth");
                    skeldNavVentSouth.transform.SetParent(halconCockpit.transform);
                    skeldNavVentSouth.transform.position = new Vector3(6.5f, -15.05f, skeldNavVentSouth.transform.position.z);
                    GameObject skeldNavDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/Cockpit/DivertPowerConsole");
                    skeldNavDivertPowerConsole.transform.SetParent(halconCockpit.transform);
                    skeldNavDivertPowerConsole.transform.position = new Vector3(6.07f, -12.55f, skeldNavDivertPowerConsole.transform.position.z);
                    GameObject skeldNavStabilizeSteeringConsole = GameObject.Find("StabilizeSteeringConsole");
                    skeldNavStabilizeSteeringConsole.transform.SetParent(halconCockpit.transform);
                    skeldNavStabilizeSteeringConsole.transform.position = new Vector3(9.21f, -14.17f, skeldNavStabilizeSteeringConsole.transform.position.z);
                    GameObject skeldNavChartCourseConsole = GameObject.Find("ChartCourseConsole");
                    skeldNavChartCourseConsole.transform.SetParent(halconCockpit.transform);
                    skeldNavChartCourseConsole.transform.position = new Vector3(8.01f, -13.1f, skeldNavChartCourseConsole.transform.position.z);
                    GameObject skeldNavUploadDataConsole = GameObject.Find("SkeldShip(Clone)/Cockpit/Ground/UploadDataConsole");
                    skeldNavUploadDataConsole.transform.SetParent(halconCockpit.transform);
                    skeldNavUploadDataConsole.transform.position = new Vector3(6.59f, -12.55f, skeldNavUploadDataConsole.transform.position.z);
                    GameObject skeldNavnav_chairmid = GameObject.Find("nav_chairmid");
                    skeldNavnav_chairmid.transform.SetParent(halconCockpit.transform);
                    skeldNavnav_chairmid.transform.position = new Vector3(8.5f, -14.1f, skeldNavnav_chairmid.transform.position.z);
                    GameObject skeldNavnav_chairback = GameObject.Find("nav_chairback");
                    skeldNavnav_chairback.transform.SetParent(halconCockpit.transform);
                    skeldNavnav_chairback.transform.position = new Vector3(7.7f, -13.4f, skeldNavnav_chairback.transform.position.z);

                    // Weapons objects
                    GameObject halconWeapons = senseiMap.transform.GetChild(5).transform.gameObject; // find halconWeapons to be the parent
                    GameObject skeldWeaponsVent = GameObject.Find("WeaponsVent");
                    skeldWeaponsVent.transform.SetParent(halconWeapons.transform);
                    skeldWeaponsVent.transform.position = new Vector3(12.25f, -2.85f, skeldWeaponsVent.transform.position.z);
                    GameObject skeldWeaponsUploadDataConsole = GameObject.Find("SkeldShip(Clone)/Weapons/Ground/UploadDataConsole");
                    skeldWeaponsUploadDataConsole.transform.SetParent(halconWeapons.transform);
                    skeldWeaponsUploadDataConsole.transform.position = new Vector3(11.33f, 0.3f, skeldWeaponsUploadDataConsole.transform.position.z);
                    GameObject skeldWeaponsDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/Weapons/Ground/weap_wall/DivertPowerConsole");
                    skeldWeaponsDivertPowerConsole.transform.SetParent(halconWeapons.transform);
                    skeldWeaponsDivertPowerConsole.transform.position = new Vector3(14.24f, 0.075f, skeldWeaponsDivertPowerConsole.transform.position.z);
                    GameObject skeldWeaponsHeadAnim = GameObject.Find("bullettop-capglo0001");
                    skeldWeaponsHeadAnim.transform.SetParent(halconWeapons.transform);
                    skeldWeaponsHeadAnim.transform.position = new Vector3(10.14f, 0.525f, skeldWeaponsHeadAnim.transform.position.z);
                    GameObject skeldWeaponsConsole = GameObject.Find("WeaponConsole");
                    skeldWeaponsConsole.transform.SetParent(halconWeapons.transform);
                    skeldWeaponsConsole.transform.position = new Vector3(11.84f, -1.25f, skeldWeaponsConsole.transform.position.z);

                    // LifeSupport objects
                    GameObject halconLifeSupport = senseiMap.transform.GetChild(6).transform.gameObject; // find halconLifeSupport to be the parent
                    GameObject skeldLifeSupportGarbageConsole = GameObject.Find("SkeldShip(Clone)/LifeSupport/Ground/GarbageConsole");
                    skeldLifeSupportGarbageConsole.transform.SetParent(halconLifeSupport.transform);
                    skeldLifeSupportGarbageConsole.transform.position = new Vector3(-10.665f, 0.37f, skeldLifeSupportGarbageConsole.transform.position.z);
                    GameObject skeldLifeSupportDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/LifeSupport/Ground/DivertPowerConsole");
                    skeldLifeSupportDivertPowerConsole.transform.SetParent(halconLifeSupport.transform);
                    skeldLifeSupportDivertPowerConsole.transform.position = new Vector3(-7.808f, 2.07f, skeldLifeSupportDivertPowerConsole.transform.position.z);
                    GameObject skeldLifeSupportCleanFilterConsole = GameObject.Find("SkeldShip(Clone)/LifeSupport/Ground/CleanFilterConsole");
                    skeldLifeSupportCleanFilterConsole.transform.SetParent(halconLifeSupport.transform);
                    skeldLifeSupportCleanFilterConsole.transform.position = new Vector3(-9.8f, 0.82f, skeldLifeSupportCleanFilterConsole.transform.position.z);
                    GameObject skeldLifeSupportLifeSuppTank = GameObject.Find("SkeldShip(Clone)/LifeSupport/Ground/LifeSuppTank");
                    skeldLifeSupportLifeSuppTank.transform.SetParent(halconLifeSupport.transform);
                    skeldLifeSupportLifeSuppTank.transform.position = new Vector3(-8.45f, 0.6f, skeldLifeSupportLifeSuppTank.transform.position.z);
                    GameObject skeldBigYVent = GameObject.Find("BigYVent");
                    skeldBigYVent.transform.SetParent(halconLifeSupport.transform);
                    skeldBigYVent.transform.position = new Vector3(-9.65f, -0.4f, skeldBigYVent.transform.position.z);

                    // Shields objects
                    GameObject halconShields = senseiMap.transform.GetChild(7).transform.gameObject; // find halconShields to be the parent
                    GameObject skeldShieldsVent = GameObject.Find("ShieldsVent");
                    skeldShieldsVent.transform.SetParent(halconShields.transform);
                    skeldShieldsVent.transform.position = new Vector3(5.575f, -1f, skeldShieldsVent.transform.position.z);
                    GameObject skeldShieldsDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/Shields/Ground/shields_floor/shields_wallside/DivertPowerConsole");
                    skeldShieldsDivertPowerConsole.transform.SetParent(halconShields.transform);
                    skeldShieldsDivertPowerConsole.transform.position = new Vector3(8.962f, 0.7f, skeldShieldsDivertPowerConsole.transform.position.z);
                    GameObject skeldShieldLowerLeft = GameObject.Find("ShieldLowerLeft");
                    skeldShieldLowerLeft.transform.SetParent(halconShields.transform);
                    skeldShieldLowerLeft.transform.position = new Vector3(5.99f, -2.98f, skeldShieldLowerLeft.transform.position.z);
                    GameObject skeldShieldsbulb = GameObject.Find("bulb");
                    skeldShieldsbulb.transform.SetParent(halconShields.transform);
                    skeldShieldsbulb.transform.position = new Vector3(9.55f, -1.05f, skeldShieldsbulb.transform.position.z);
                    GameObject skeldShieldsbulbone = GameObject.Find("bulb (1)");
                    skeldShieldsbulbone.transform.SetParent(halconShields.transform);
                    skeldShieldsbulbone.transform.position = new Vector3(9.55f, -0.7f, skeldShieldsbulbone.transform.position.z);
                    GameObject skeldShieldsbulbtwo = GameObject.Find("bulb (2)");
                    skeldShieldsbulbtwo.transform.SetParent(halconShields.transform);
                    skeldShieldsbulbtwo.transform.position = new Vector3(9.55f, -0.35f, skeldShieldsbulbtwo.transform.position.z);
                    GameObject skeldShieldsbulbthree = GameObject.Find("bulb (3)");
                    skeldShieldsbulbthree.transform.SetParent(halconShields.transform);
                    skeldShieldsbulbthree.transform.position = new Vector3(5.45f, 0.15f, skeldShieldsbulbthree.transform.position.z);
                    GameObject skeldShieldsbulbfour = GameObject.Find("bulb (4)");
                    skeldShieldsbulbfour.transform.SetParent(halconShields.transform);
                    skeldShieldsbulbfour.transform.position = new Vector3(5.75f, 0.3f, skeldShieldsbulbfour.transform.position.z);
                    GameObject skeldShieldsbulbfive = GameObject.Find("bulb (5)");
                    skeldShieldsbulbfive.transform.SetParent(halconShields.transform);
                    skeldShieldsbulbfive.transform.position = new Vector3(6.05f, 0.45f, skeldShieldsbulbfive.transform.position.z);
                    GameObject skeldShieldsbulbsix = GameObject.Find("bulb (6)");
                    skeldShieldsbulbsix.transform.SetParent(halconShields.transform);
                    skeldShieldsbulbsix.transform.position = new Vector3(6.35f, 0.6f, skeldShieldsbulbsix.transform.position.z);

                    // Hallway objects
                    GameObject halconHallway = senseiMap.transform.GetChild(8).transform.gameObject; // find halconBigHallway to be the parent
                    GameObject skeldCrossHallwayFixWiringConsole = GameObject.Find("SkeldShip(Clone)/CrossHallway/FixWiringConsole");
                    skeldCrossHallwayFixWiringConsole.transform.SetParent(halconHallway.transform);
                    skeldCrossHallwayFixWiringConsole.transform.position = new Vector3(-8.9F, 4.93F, skeldCrossHallwayFixWiringConsole.transform.position.z);
                    GameObject skeldBigYHallwayFixWiringConsole = GameObject.Find("SkeldShip(Clone)/BigYHallway/FixWiringConsole");
                    skeldBigYHallwayFixWiringConsole.transform.SetParent(halconHallway.transform);
                    skeldBigYHallwayFixWiringConsole.transform.position = new Vector3(4.685f, -12.53f, skeldBigYHallwayFixWiringConsole.transform.position.z);
                    GameObject skeldAdminSurvCamera = GameObject.Find("SkeldShip(Clone)/AdminHallway/SurvCamera");
                    skeldAdminSurvCamera.transform.SetParent(halconHallway.transform);
                    skeldAdminSurvCamera.transform.position = new Vector3(5.345f, -12.45f, skeldAdminSurvCamera.transform.position.z);
                    GameObject skeldBigHallwaySurvCamera = GameObject.Find("SkeldShip(Clone)/BigYHallway/SurvCamera");
                    skeldBigHallwaySurvCamera.transform.SetParent(halconHallway.transform);
                    skeldBigHallwaySurvCamera.transform.position = new Vector3(9.33f, 0.8f, skeldBigHallwaySurvCamera.transform.position.z);
                    GameObject skeldNorthHallwaySurvCamera = GameObject.Find("SkeldShip(Clone)/NorthHallway/SurvCamera");
                    skeldNorthHallwaySurvCamera.transform.SetParent(halconHallway.transform);
                    skeldNorthHallwaySurvCamera.transform.position = new Vector3(-14.53f, -4.5f, skeldNorthHallwaySurvCamera.transform.position.z);
                    GameObject skeldCrossHallwaySurvCamera = GameObject.Find("SkeldShip(Clone)/CrossHallway/SurvCamera");
                    skeldCrossHallwaySurvCamera.transform.SetParent(halconHallway.transform);
                    skeldCrossHallwaySurvCamera.transform.position = new Vector3(-9.85f, 4.75f, skeldCrossHallwaySurvCamera.transform.position.z);

                    // Electrical objects
                    GameObject halconElectrical = senseiMap.transform.GetChild(9).transform.gameObject; // find halconElectrical to be the parent
                    GameObject skeldElecVent = GameObject.Find("ElecVent");
                    skeldElecVent.transform.SetParent(halconElectrical.transform);
                    skeldElecVent.transform.position = new Vector3(-5.22f, -13.95f, skeldElecVent.transform.position.z);
                    GameObject skeldElecCalibrateConsole = GameObject.Find("CalibrateConsole");
                    skeldElecCalibrateConsole.transform.SetParent(halconElectrical.transform);
                    skeldElecCalibrateConsole.transform.position = new Vector3(-5.48f, -11.55f, skeldElecCalibrateConsole.transform.position.z);
                    GameObject skeldelectric_frontset = GameObject.Find("electric_frontset");
                    skeldelectric_frontset.transform.SetParent(halconElectrical.transform);
                    skeldelectric_frontset.transform.position = new Vector3(-7.6f, -12.75f, skeldelectric_frontset.transform.position.z);
                    GameObject skeldElecUploadDataConsole = GameObject.Find("SkeldShip(Clone)/Electrical/Ground/UploadDataConsole");
                    skeldElecUploadDataConsole.transform.SetParent(halconElectrical.transform);
                    skeldElecUploadDataConsole.transform.position = new Vector3(-7.75f, -8.25f, skeldElecUploadDataConsole.transform.position.z);
                    GameObject skeldElecFixWiringConsole = GameObject.Find("SkeldShip(Clone)/Electrical/Ground/FixWiringConsole");
                    skeldElecFixWiringConsole.transform.SetParent(halconElectrical.transform);
                    skeldElecFixWiringConsole.transform.position = new Vector3(-6.37f, -8.725f, skeldElecFixWiringConsole.transform.position.z);
                    GameObject skeldElectDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/Electrical/Ground/DivertPowerConsole");
                    skeldElectDivertPowerConsole.transform.SetParent(halconElectrical.transform);
                    skeldElectDivertPowerConsole.transform.position = new Vector3(-8.55f, -11.25f, skeldElectDivertPowerConsole.transform.position.z);

                    // Reactor objects
                    GameObject halconReactor = senseiMap.transform.GetChild(10).transform.gameObject; // find halconReactor to be the parent
                    GameObject skeldReactorVent = GameObject.Find("ReactorVent");
                    skeldReactorVent.transform.SetParent(halconReactor.transform);
                    skeldReactorVent.transform.position = new Vector3(-19.75f, -3.1f, skeldReactorVent.transform.position.z);
                    GameObject skeldUpperReactorVent = GameObject.Find("UpperReactorVent");
                    skeldUpperReactorVent.transform.SetParent(halconReactor.transform);
                    skeldUpperReactorVent.transform.position = new Vector3(-19.75f, 0f, skeldUpperReactorVent.transform.position.z);
                    GameObject skeldDivertPowerFalsePanel = GameObject.Find("DivertPowerFalsePanel");
                    skeldDivertPowerFalsePanel.transform.SetParent(halconReactor.transform);
                    skeldDivertPowerFalsePanel.transform.position = new Vector3(-18.6f, 1, skeldDivertPowerFalsePanel.transform.position.z);
                    GameObject skeldreactor_toppipe = GameObject.Find("reactor_toppipe");
                    skeldreactor_toppipe.transform.SetParent(halconReactor.transform);
                    skeldreactor_toppipe.transform.position = new Vector3(-22.08f, 0.8f, skeldreactor_toppipe.transform.position.z);
                    GameObject skeldreactor_base = GameObject.Find("reactor_base");
                    skeldreactor_base.transform.SetParent(halconReactor.transform);
                    skeldreactor_base.transform.position = new Vector3(-22.12f, -2.6f, skeldreactor_base.transform.position.z);
                    GameObject skeldreactor_wireTop = GameObject.Find("reactor_wireTop");
                    skeldreactor_wireTop.transform.SetParent(halconReactor.transform);
                    skeldreactor_wireTop.transform.position = new Vector3(-21.21f, 0.175f, 6.7f);
                    GameObject skeldreactor_wireBot = GameObject.Find("reactor_wireBot");
                    skeldreactor_wireBot.transform.SetParent(halconReactor.transform);
                    skeldreactor_wireBot.transform.position = new Vector3(-21.21f, -2.7f, 6.9f);
                    skeldreactor_wireBot.transform.rotation = Quaternion.Euler(0f, 0f, 12.5f);

                    // Storage objects
                    GameObject halconStorage = senseiMap.transform.GetChild(11).transform.gameObject; // find halconStorage to be the parent
                    GameObject skeldAirlockConsole = GameObject.Find("AirlockConsole");
                    skeldAirlockConsole.transform.SetParent(halconStorage.transform);
                    skeldAirlockConsole.transform.position = new Vector3(-9.725f, -12.6f, skeldAirlockConsole.transform.position.z);
                    GameObject skeldstorage_Boxes = GameObject.Find("storage_Boxes");
                    skeldstorage_Boxes.transform.SetParent(halconStorage.transform);
                    skeldstorage_Boxes.transform.position = new Vector3(-13.55f, -10.4f, skeldstorage_Boxes.transform.position.z);
                    GameObject skeldStorageFixWiringConsole = GameObject.Find("SkeldShip(Clone)/Storage/Ground/FixWiringConsole");
                    skeldStorageFixWiringConsole.transform.SetParent(halconStorage.transform);
                    skeldStorageFixWiringConsole.transform.position = new Vector3(-17.77f, -9.74f, skeldStorageFixWiringConsole.transform.position.z);

                    // RightEngine objects
                    GameObject halconRightEngine = senseiMap.transform.GetChild(12).transform.gameObject; // find halconRightEngine to be the parent
                    GameObject skeldREngineVent = GameObject.Find("REngineVent");
                    skeldREngineVent.transform.SetParent(halconRightEngine.transform);
                    skeldREngineVent.transform.position = new Vector3(-18.9f, -8.7f, skeldREngineVent.transform.position.z);
                    GameObject skeldRchain01 = GameObject.Find("SkeldShip(Clone)/RightEngine/Ground/engineRight/chain01");
                    skeldRchain01.transform.SetParent(halconRightEngine.transform);
                    skeldRchain01.transform.position = new Vector3(-17.75f, -3.65f, skeldRchain01.transform.position.z);
                    GameObject skeldRchain02 = GameObject.Find("SkeldShip(Clone)/RightEngine/Ground/engineRight/chain02");
                    skeldRchain02.transform.SetParent(halconRightEngine.transform);
                    skeldRchain02.transform.position = new Vector3(-18.025f, -3.7f, skeldRchain02.transform.position.z);
                    GameObject skeldRchain011 = GameObject.Find("chain01 (1)");
                    skeldRchain011.transform.SetParent(halconRightEngine.transform);
                    skeldRchain011.transform.position = new Vector3(-18.765f, -3.85f, skeldRchain011.transform.position.z);
                    GameObject skeldREngineDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/RightEngine/Ground/DivertPowerConsole");
                    skeldREngineDivertPowerConsole.transform.SetParent(halconRightEngine.transform);
                    skeldREngineDivertPowerConsole.transform.position = new Vector3(-16.875f, -3.7f, skeldREngineDivertPowerConsole.transform.position.z);
                    GameObject skeldREngineFuelEngineConsole = GameObject.Find("SkeldShip(Clone)/RightEngine/Ground/engineRight/FuelEngineConsole");
                    skeldREngineFuelEngineConsole.transform.SetParent(halconRightEngine.transform);
                    skeldREngineFuelEngineConsole.transform.position = new Vector3(-19.65f, -7.12f, skeldREngineFuelEngineConsole.transform.position.z);
                    GameObject skeldREngineAlignEngineConsole = GameObject.Find("SkeldShip(Clone)/RightEngine/Ground/engineRight/AlignEngineConsole");
                    skeldREngineAlignEngineConsole.transform.SetParent(halconRightEngine.transform);
                    skeldREngineAlignEngineConsole.transform.position = new Vector3(-20.475f, -7.12f, skeldREngineAlignEngineConsole.transform.position.z);
                    GameObject skeldREngineElectric = GameObject.Find("SkeldShip(Clone)/RightEngine/Ground/engineRight/Electric");
                    skeldREngineElectric.transform.SetParent(halconRightEngine.transform);
                    skeldREngineElectric.transform.position = new Vector3(-19.2f, -5.475f, skeldREngineElectric.transform.position.z);
                    GameObject skeldREngineSteam = GameObject.Find("SkeldShip(Clone)/RightEngine/Ground/engineRight/Steam");
                    skeldREngineSteam.transform.SetParent(halconRightEngine.transform);
                    skeldREngineSteam.transform.position = new Vector3(-17.6f, -4.4f, skeldREngineSteam.transform.position.z);
                    GameObject skeldREngineSteam1 = GameObject.Find("SkeldShip(Clone)/RightEngine/Ground/engineRight/Steam (1)");
                    skeldREngineSteam1.transform.SetParent(halconRightEngine.transform);
                    skeldREngineSteam1.transform.position = new Vector3(-17.6f, -7.4f, skeldREngineSteam1.transform.position.z);
                    GameObject skeldengineRight = GameObject.Find("engineRight");
                    skeldengineRight.transform.SetParent(halconRightEngine.transform);
                    skeldengineRight.transform.position = new Vector3(-19.02f, -5.982f, skeldengineRight.transform.position.z);

                    // LeftEngine objects
                    GameObject halconLeftEngine = senseiMap.transform.GetChild(13).transform.gameObject; // find halconLeftEngine to be the parent
                    GameObject skeldLEngineVent = GameObject.Find("LEngineVent");
                    skeldLEngineVent.transform.SetParent(halconLeftEngine.transform);
                    skeldLEngineVent.transform.position = new Vector3(-18.92f, 5.8f, skeldLEngineVent.transform.position.z);
                    GameObject skeldLchain01 = GameObject.Find("SkeldShip(Clone)/LeftEngine/Ground/chain01");
                    skeldLchain01.transform.SetParent(halconLeftEngine.transform);
                    skeldLchain01.transform.position = new Vector3(-17.1f, 6.1f, skeldLchain01.transform.position.z);
                    GameObject skeldLchain02 = GameObject.Find("SkeldShip(Clone)/LeftEngine/Ground/chain02");
                    skeldLchain02.transform.SetParent(halconLeftEngine.transform);
                    skeldLchain02.transform.position = new Vector3(-16.9f, 5.95f, skeldLchain02.transform.position.z);
                    GameObject skeldLEngineDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/LeftEngine/Ground/DivertPowerConsole");
                    skeldLEngineDivertPowerConsole.transform.SetParent(halconLeftEngine.transform);
                    skeldLEngineDivertPowerConsole.transform.position = new Vector3(-18.92f, 6.95f, skeldLEngineDivertPowerConsole.transform.position.z);
                    GameObject skeldLEngineFuelEngineConsole = GameObject.Find("SkeldShip(Clone)/LeftEngine/Ground/engineLeft/FuelEngineConsole");
                    skeldLEngineFuelEngineConsole.transform.SetParent(halconLeftEngine.transform);
                    skeldLEngineFuelEngineConsole.transform.position = new Vector3(-19.65f, 2.48f, skeldLEngineFuelEngineConsole.transform.position.z);
                    GameObject skeldLEngineAlignEngineConsole = GameObject.Find("SkeldShip(Clone)/LeftEngine/Ground/engineLeft/AlignEngineConsole");
                    skeldLEngineAlignEngineConsole.transform.SetParent(halconLeftEngine.transform);
                    skeldLEngineAlignEngineConsole.transform.position = new Vector3(-20.375f, 2.56f, skeldLEngineAlignEngineConsole.transform.position.z);
                    GameObject skeldLEngineElectric = GameObject.Find("SkeldShip(Clone)/LeftEngine/Ground/engineLeft/Electric");
                    skeldLEngineElectric.transform.SetParent(halconLeftEngine.transform);
                    skeldLEngineElectric.transform.position = new Vector3(-19.2f, 4.15f, skeldLEngineElectric.transform.position.z);
                    GameObject skeldLEngineSteam = GameObject.Find("SkeldShip(Clone)/LeftEngine/Ground/engineLeft/Steam");
                    skeldLEngineSteam.transform.SetParent(halconLeftEngine.transform);
                    skeldLEngineSteam.transform.position = new Vector3(-17.6f, 5.1f, skeldLEngineSteam.transform.position.z);
                    GameObject skeldLEngineSteam1 = GameObject.Find("SkeldShip(Clone)/LeftEngine/Ground/engineLeft/Steam (1)");
                    skeldLEngineSteam1.transform.SetParent(halconLeftEngine.transform);
                    skeldLEngineSteam1.transform.position = new Vector3(-17.7f, 3.8f, skeldLEngineSteam1.transform.position.z);
                    GameObject skeldengineLeft = GameObject.Find("engineLeft");
                    skeldengineLeft.transform.SetParent(halconLeftEngine.transform);
                    skeldengineLeft.transform.position = new Vector3(-19.02f, 3.63f, skeldengineLeft.transform.position.z);

                    // Comms objects
                    GameObject halconComms = senseiMap.transform.GetChild(14).transform.gameObject; // find halconComms to be the parent
                    GameObject skeldFixCommsConsole = GameObject.Find("FixCommsConsole");
                    skeldFixCommsConsole.transform.SetParent(halconComms.transform);
                    skeldFixCommsConsole.transform.position = new Vector3(7.555f, 3.34f, skeldFixCommsConsole.transform.position.z);
                    skeldFixCommsConsole.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.customComms.GetComponent<SpriteRenderer>().sprite;
                    GameObject skeldcommsDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/Comms/Ground/comms_wallstuff/DivertPowerConsole");
                    skeldcommsDivertPowerConsole.transform.SetParent(halconComms.transform);
                    skeldcommsDivertPowerConsole.transform.position = new Vector3(6.95f, 5.775f, skeldcommsDivertPowerConsole.transform.position.z);
                    GameObject skeldcommsUploadDataConsole = GameObject.Find("SkeldShip(Clone)/Comms/Ground/comms_wallstuff/UploadDataConsole");
                    skeldcommsUploadDataConsole.transform.SetParent(halconComms.transform);
                    skeldcommsUploadDataConsole.transform.position = new Vector3(8.85f, 1.87f, skeldcommsUploadDataConsole.transform.position.z);
                    GameObject skeldtapescomms_tapes0001 = GameObject.Find("tapes-comms_tapes0001");
                    skeldtapescomms_tapes0001.transform.SetParent(halconComms.transform);
                    skeldtapescomms_tapes0001.transform.position = new Vector3(6.047f, 5.8f, skeldtapescomms_tapes0001.transform.position.z);

                    // Security objects
                    GameObject halconSecurity = senseiMap.transform.GetChild(15).transform.gameObject; // find halconSecurity to be the parent
                    GameObject skeldSecurityVent = GameObject.Find("SecurityVent");
                    skeldSecurityVent.transform.SetParent(halconSecurity.transform);
                    skeldSecurityVent.transform.position = new Vector3(-8.25f, 10.7f, skeldSecurityVent.transform.position.z);
                    GameObject skeldmap_surveillance = GameObject.Find("map_surveillance");
                    skeldmap_surveillance.transform.SetParent(halconSecurity.transform);
                    skeldmap_surveillance.transform.position = new Vector3(-6.8f, 12.26f, skeldmap_surveillance.transform.position.z);
                    GameObject skeldServers = GameObject.Find("Servers");
                    skeldServers.transform.SetParent(halconSecurity.transform);
                    skeldServers.transform.position = new Vector3(-8.5f, 11.72f, skeldServers.transform.position.z);
                    GameObject skeldsecurityDivertPowerConsole = GameObject.Find("SkeldShip(Clone)/Security/Ground/DivertPowerConsole");
                    skeldsecurityDivertPowerConsole.transform.SetParent(halconSecurity.transform);
                    skeldsecurityDivertPowerConsole.transform.position = new Vector3(-5.3f, 12.025f, skeldsecurityDivertPowerConsole.transform.position.z);

                    // Medical objects
                    GameObject halconMedical = senseiMap.transform.GetChild(16).transform.gameObject; // find halconMedical to be the parent
                    GameObject skeldMedVent = GameObject.Find("MedVent");
                    skeldMedVent.transform.SetParent(halconMedical.transform);
                    skeldMedVent.transform.position = new Vector3(-4.35f, -1.8f, skeldMedVent.transform.position.z);
                    GameObject skeldMedScanner = GameObject.Find("MedScanner");
                    skeldMedScanner.transform.SetParent(halconMedical.transform);
                    skeldMedScanner.transform.position = new Vector3(-8.4f, -2.915f, skeldMedScanner.transform.position.z);
                    GameObject skeldMedBayConsole = GameObject.Find("MedBayConsole");
                    skeldMedBayConsole.transform.SetParent(halconMedical.transform);
                    skeldMedBayConsole.transform.position = new Vector3(-4.315f, -0.595f, skeldMedBayConsole.transform.position.z);

                    var objList = GameObject.FindObjectsOfType<Console>().ToList();
                    foreach (var obj in objList) {
                        if (obj.name != "AlignEngineConsole") {
                            obj.checkWalls = true;
                        }
                    }

                    // Change original skeld map parent and hide the innecesary vanilla objects (don't destroy them, the game won't work otherwise)
                    GameObject skeldship = GameObject.Find("SkeldShip(Clone)");
                    Transform[] allChildren = skeldship.transform.GetComponentsInChildren<Transform>(true);
                    for (int i = 1; i < allChildren.Length - 1; i++) {
                        allChildren[i].gameObject.SetActive(false);
                    }
                    skeldship.transform.SetParent(halconCollisions.transform);
                    activatedSensei = true;
                }
            }
        }

        public static StaticDoor GetStaticDoor(string name)
        {
            foreach (var doors in UnityEngine.Object.FindObjectsOfType(Il2CppType.Of<StaticDoor>()))
            {
                if (doors.name != name) continue;

                return doors.Cast<StaticDoor>();
            }
            return null;
        }
    }
}