using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Collections;
using UnhollowerBaseLib;
using UnityEngine;
using System.Linq;
using static LasMonjas.LasMonjas;
using LasMonjas.Core;
using HarmonyLib;
using Hazel;
using LasMonjas.Objects;
using LasMonjas.Patches;

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
                System.Console.WriteLine("Error loading sprite from path: " + path);
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
                System.Console.WriteLine("Error loading texture from resources: " + path);
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
                System.Console.WriteLine("Error loading texture from disk: " + path);
            }
            return null;
        }

        internal delegate bool d_LoadImage(IntPtr tex, IntPtr data, bool markNonReadable);
        internal static d_LoadImage iCall_LoadImage;
        private static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable) {
            if (iCall_LoadImage == null)
                iCall_LoadImage = IL2CPP.ResolveICall<d_LoadImage>("UnityEngine.ImageConversion::LoadImage");
            var il2cppArray = (Il2CppStructArray<byte>) data;
            return iCall_LoadImage.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
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

                if (roleInfo.name == "Renegade") {
                    var getMinionText = Renegade.canRecruitMinion ? " and recruit a Minion" : "";
                    task.Text = cs(roleInfo.color, $"{roleInfo.name}: Kill everyone{getMinionText}");  
                } else {
                    task.Text = cs(roleInfo.color, $"{roleInfo.name}: {roleInfo.shortDescription}");  
                }

                player.myTasks.Insert(0, task);
            }
        }

        public static bool isLighterColor(int colorId) {
            return CustomColors.lighterColors.Contains(colorId);
        }

        public static bool isCustomServer() {
            if (DestroyableSingleton<ServerManager>.Instance == null) return false;
            StringNames n = DestroyableSingleton<ServerManager>.Instance.CurrentRegion.TranslateName;
            return n != StringNames.ServerNA && n != StringNames.ServerEU && n != StringNames.ServerAS;
        }

        //Fake tasks for neutral and rebel team
        public static bool hasFakeTasks(this PlayerControl player) {
            return (player == Joker.joker || player == RoleThief.rolethief || player == Pyromaniac.pyromaniac || player == TreasureHunter.treasureHunter || player == Devourer.devourer || player == Renegade.renegade || player == Minion.minion || player == BountyHunter.bountyhunter  || player == Trapper.trapper || player == Yinyanger.yinyanger || player == Challenger.challenger || Renegade.formerRenegades.Contains(player));
        }

        public static bool canBeErased(this PlayerControl player) {
            return (player != Renegade.renegade && player != Minion.minion && !Renegade.formerRenegades.Contains(player));
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
            player.NameText.color = new Color(player.NameText.color.r, player.NameText.color.g, player.NameText.color.b, alpha);
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
            if (Painter.painterTimer > 0f) return true; // No names are visible
            else if (!MapOptions.hidePlayerNames) return false; // All names are visible
            else if (source == null || target == null) return true;
            else if (source == target) return false; // Player sees his own name
            else if (source.Data.Role.IsImpostor && target.Data.Role.IsImpostor) return false; // Members of team Impostors see the names of Impostors
            else if ((source == Modifiers.lover1 || source == Modifiers.lover2) && (target == Modifiers.lover1 || target == Modifiers.lover2)) return false; // Members of team Lovers see the names of each other
            else if ((source == Renegade.renegade || source == Minion.minion) && (target == Renegade.renegade || target == Minion.minion || target == Renegade.fakeMinion)) return false; // Members of team Renegade see the names of each other
            return true;
        }

        public static void setDefaultLook(this PlayerControl target) {
            target.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void setLook(this PlayerControl target, String playerName, int colorId, string hatId, string visorId, string skinId, string petId) {
            target.RawSetColor(colorId);
            target.RawSetVisor(visorId);
            target.RawSetHat(hatId, colorId);
            target.RawSetName(hidePlayerName(PlayerControl.LocalPlayer, target) ? "" : playerName);

            SkinViewData nextSkin = DestroyableSingleton<HatManager>.Instance.GetSkinById(skinId).viewData.viewData;
            PlayerPhysics playerPhysics = target.MyPhysics;
            AnimationClip clip = null;
            var spriteAnim = playerPhysics.Skin.animator;
            var currentPhysicsAnim = playerPhysics.Animator.GetCurrentAnimation();
            if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.CurrentAnimationGroup.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;

            float progress = playerPhysics.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playerPhysics.Skin.skin = nextSkin;
            spriteAnim.Play(clip, 1f);
            spriteAnim.m_animator.Play("a", 0, progress % 1);
            spriteAnim.m_animator.Update(0f);

            if (target.CurrentPet) UnityEngine.Object.Destroy(target.CurrentPet.gameObject);
            target.CurrentPet = UnityEngine.Object.Instantiate<PetBehaviour>(DestroyableSingleton<HatManager>.Instance.GetPetById(petId).viewData.viewData);
            target.CurrentPet.transform.position = target.transform.position;
            target.CurrentPet.Source = target;
            target.CurrentPet.Visible = target.Visible;
            PlayerControl.SetPlayerMaterialColors(colorId, target.CurrentPet.rend);
        }

        public static bool roleCanUseVents(this PlayerControl player) {
            bool roleCouldUse = false;
            if (CaptureTheFlag.captureTheFlagMode && PlayerControl.LocalPlayer == player && howmanygamemodesareon == 1) {
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
            }
            else if (PoliceAndThief.policeAndThiefMode && PlayerControl.LocalPlayer == player && howmanygamemodesareon == 1) {
                if (PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer01 && !PoliceAndThief.thiefplayer01IsStealing && !PoliceAndThief.thiefplayer01IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer02 && !PoliceAndThief.thiefplayer02IsStealing && !PoliceAndThief.thiefplayer02IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer03 && !PoliceAndThief.thiefplayer03IsStealing && !PoliceAndThief.thiefplayer03IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer04 && !PoliceAndThief.thiefplayer04IsStealing && !PoliceAndThief.thiefplayer04IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer05 && !PoliceAndThief.thiefplayer05IsStealing && !PoliceAndThief.thiefplayer05IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer06 && !PoliceAndThief.thiefplayer06IsStealing && !PoliceAndThief.thiefplayer06IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer07 && !PoliceAndThief.thiefplayer07IsStealing && !PoliceAndThief.thiefplayer07IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer08 && !PoliceAndThief.thiefplayer08IsStealing && !PoliceAndThief.thiefplayer08IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer09 && !PoliceAndThief.thiefplayer09IsStealing && !PoliceAndThief.thiefplayer09IsReviving 
                    || PlayerControl.LocalPlayer == PoliceAndThief.thiefplayer10 && !PoliceAndThief.thiefplayer10IsStealing && !PoliceAndThief.thiefplayer10IsReviving) {
                    roleCouldUse = true;
                }
                else {
                    roleCouldUse = false;
                }
            }
            else if (KingOfTheHill.kingOfTheHillMode && PlayerControl.LocalPlayer == player && howmanygamemodesareon == 1) {
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
            }
            else if (HotPotato.hotPotatoMode && PlayerControl.LocalPlayer == player && howmanygamemodesareon == 1) {
                if (PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                    roleCouldUse = true;
                }
                else {
                    roleCouldUse = false;
                }
            }
            else if (ZombieLaboratory.zombieLaboratoryMode && PlayerControl.LocalPlayer == player && howmanygamemodesareon == 1) {
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
                else if (player.Data.Role.IsImpostor) {
                    roleCouldUse = true;
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

        public static MurderAttemptResult checkMurderAttemptAndKill(PlayerControl killer, PlayerControl target, bool isMeetingStart = false, bool showAnimation = true)  {
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

        public static List<PlayerControl> getKillerTeamMembers(PlayerControl player) {
            List<PlayerControl> team = new List<PlayerControl>();
            foreach(PlayerControl p in PlayerControl.AllPlayerControls) {
                if (player.Data.Role.IsImpostor && p.Data.Role.IsImpostor && player.PlayerId != p.PlayerId && team.All(x => x.PlayerId != p.PlayerId)) team.Add(p);
                else if (player == Renegade.renegade && p == Minion.minion) team.Add(p); 
                else if (player == Minion.minion && p == Renegade.renegade) team.Add(p);
            }
            
            return team;
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
    }
}
