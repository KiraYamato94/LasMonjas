using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using static LasMonjas.LasMonjas;
using static LasMonjas.GameHistory;
using LasMonjas.Core;
using HarmonyLib;
using Hazel;
using LasMonjas.Objects;
using LasMonjas.Patches;
using AmongUs.GameOptions;
using System.Collections;
using TMPro;
using System.Text.RegularExpressions;
using static UnityEngine.GraphicsBuffer;


namespace LasMonjas
{

    public enum MurderAttemptResult {
        PerformKill,
        SuppressKill,
        JinxKill
    }
    public static class Helpers {

        private static readonly Dictionary<string, Sprite> _CachedSprites = new();
        public static Sprite loadSpriteFromResources(string path, float pixelsPerUnit) {
            try {
                if (_CachedSprites.TryGetValue(path + pixelsPerUnit, out Sprite sprite))
                    return sprite;
                Texture2D texture = loadTextureFromResources(path);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
                return _CachedSprites[path + pixelsPerUnit] = sprite;
            }
            catch {
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
            foreach (PlayerControl player in PlayerInCache.AllPlayers)
                if (player.PlayerId == id)
                    return player;
            return null;
        }

        public static Dictionary<byte, PlayerControl> allPlayersById()
        {
            Dictionary<byte, PlayerControl> res = new Dictionary<byte, PlayerControl>();
            foreach (PlayerControl player in PlayerInCache.AllPlayers)
                res.Add(player.PlayerId, player);
            return res;
        }
        public static int availableVentId() {
            var id = 0;
            while (true) {
                if (ShipStatus.Instance.AllVents.All(v => v.Id != id)) return id;
                id++;
            }
        }
        public static void handleDemonBiteOnBodyReport() {
            // Murder the bitten player and reset bitten (regardless whether the kill was successful or not)
            Helpers.checkMurderAttemptAndKill(Demon.demon, Demon.bitten, false);
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.DemonSetBitten, Hazel.SendOption.Reliable, -1);
            writer.Write(byte.MaxValue);
            writer.Write(byte.MaxValue);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.demonSetBitten(byte.MaxValue, byte.MaxValue);
        }

        public static void handleMedusaPetrifyOnBodyReport() {
            // Murder the petrified players (regardless whether the kill was successful or not)
            foreach (PlayerControl player in Medusa.petrifiedPlayers) {
                if (Medusa.medusa != null) {
                    Helpers.checkMurderAttemptAndKill(Medusa.medusa, player, false);
                }
            }
        }
        public static PlayerControl GetShootPlayer(float shotSize, float effectiveRange, float angle, List<PlayerControl> team, Vector2 originPlayer, bool checkWalls = false) {
            PlayerControl result = null;
            float num = effectiveRange;
            Vector3 pos;
            float mouseAngle = angle;
            foreach (PlayerControl player in team) {
                if (player.PlayerId == PlayerInCache.LocalPlayer.PlayerControl.PlayerId) continue;

                if (player.Data.IsDead) continue;

                pos = player.transform.position - PlayerInCache.LocalPlayer.PlayerControl.transform.position;
                pos = new Vector3(
                    pos.x * MathF.Cos(mouseAngle) + pos.y * MathF.Sin(mouseAngle),
                    pos.y * MathF.Cos(mouseAngle) - pos.x * MathF.Sin(mouseAngle));
                if (Math.Abs(pos.y) < shotSize && (!(pos.x < 0)) && pos.x < num) {
                    num = pos.x;
                    if (checkWalls) {
                        if (!PhysicsHelpers.AnythingBetween(
                            originPlayer,
                            player.GetTruePosition(),
                            Constants.ShipOnlyMask,
                            false
                        )) {
                            result = player;
                        }
                    }
                    else {
                        result = player;
                    }
                }
            }
            return result;
        }

        public static void handleEatenPlayersOnBodyReport() {
            // Murder the eaten players (regardless whether the kill was successful or not)
            foreach (PlayerControl player in Devourer.eatenPlayers) {
                Helpers.checkMurderAttemptAndKill(Devourer.devourer, player, false);                             
            }
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
            return isNeutral(player) || isRebel(player);
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
        public static int lineCount(string text) {
            return text.Count(c => c == '\n');
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
            if ((source == Renegade.renegade || source == Minion.minion) && (target == Renegade.renegade || target == Minion.minion)) return false; // Members of team Renegade see the names of each other
            return true;
        }

        public static void setDefaultLook(this PlayerControl target) {
            if (!MushroomSabotageActive()) {
                target.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
            }
        }

        public static void setLook(this PlayerControl target, String playerName, int colorId, string hatId, string visorId, string skinId, string petId) {
            target.RawSetColor(colorId);
            target.RawSetVisor(visorId, colorId);
            target.RawSetHat(hatId, colorId);
            target.RawSetName(hidePlayerName(PlayerInCache.LocalPlayer.PlayerControl, target) ? "" : playerName);

            SkinViewData nextSkin = null;
            try { nextSkin = ShipStatus.Instance.CosmeticsCache.GetSkin(skinId); } catch { return; };

            PlayerPhysics playerPhysics = target.MyPhysics;
            AnimationClip clip = null;
            var spriteAnim = playerPhysics.myPlayer.cosmetics.skin.animator;
            var currentPhysicsAnim = playerPhysics.Animations.Animator.GetCurrentAnimation();


            if (currentPhysicsAnim == playerPhysics.Animations.group.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;
            float progress = playerPhysics.Animations.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playerPhysics.myPlayer.cosmetics.skin.skin = nextSkin;
            playerPhysics.myPlayer.cosmetics.skin.UpdateMaterial();

            spriteAnim.Play(clip, 1f);
            spriteAnim.m_animator.Play("a", 0, progress % 1);
            spriteAnim.m_animator.Update(0f);

            target.RawSetPet(petId, colorId);           
        }

        public static bool roleCanUseVents(this PlayerControl player) {
            bool roleCouldUse = false;
            if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek) {
                if (!PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor) {
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
                        if (PlayerInCache.LocalPlayer.PlayerControl != CaptureTheFlag.bluePlayerWhoHasRedFlag && PlayerInCache.LocalPlayer.PlayerControl != CaptureTheFlag.redPlayerWhoHasBlueFlag
                                && (CaptureTheFlag.redteamFlag.Contains(player) && !CaptureTheFlag.revivingPlayers.Contains(player)
                                || CaptureTheFlag.blueteamFlag.Contains(player) && !CaptureTheFlag.revivingPlayers.Contains(player)
                                || PlayerInCache.LocalPlayer.PlayerControl == CaptureTheFlag.stealerPlayer && !CaptureTheFlag.revivingPlayers.Contains(CaptureTheFlag.stealerPlayer))) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 3:
                        // PT:
                        if (PoliceAndThief.thiefTeam.Contains(player) && !PoliceAndThief.revivingPlayers.Contains(player) && !PoliceAndThief.stealingPlayers.Contains(player)) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 4:
                        // KOTH:
                        if (PlayerInCache.LocalPlayer.PlayerControl != KingOfTheHill.greenKingplayer && PlayerInCache.LocalPlayer.PlayerControl != KingOfTheHill.yellowKingplayer
                                && (KingOfTheHill.greenTeam.Contains(player) && !KingOfTheHill.revivingPlayers.Contains(player)
                                || KingOfTheHill.yellowTeam.Contains(player) && !KingOfTheHill.revivingPlayers.Contains(player)
                                || PlayerInCache.LocalPlayer.PlayerControl == KingOfTheHill.usurperPlayer && !KingOfTheHill.revivingPlayers.Contains(KingOfTheHill.usurperPlayer))) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 5:
                        // HP:
                        if (PlayerInCache.LocalPlayer.PlayerControl == HotPotato.hotPotatoPlayer) {
                            roleCouldUse = true;
                        }
                        else {
                            roleCouldUse = false;
                        }
                        break;
                    case 6:
                        // ZL:
                        if (ZombieLaboratory.zombieTeam.Contains(player) && !ZombieLaboratory.revivingPlayers.Contains(player)) { 
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
                        if (PlayerInCache.LocalPlayer.PlayerControl == MonjaFestival.bigMonjaPlayer) {
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

        public static MurderAttemptResult checkMurderAttempt(PlayerControl killer, PlayerControl target) {
            // Modified vanilla checks
            if (AmongUsClient.Instance.IsGameOver) return MurderAttemptResult.SuppressKill;
            if (killer == null || killer.Data == null || killer.Data.IsDead || killer.Data.Disconnected) return MurderAttemptResult.SuppressKill; // Allow non Impostor kills compared to vanilla code
            if (target == null || target.Data == null || target.Data.IsDead || target.Data.Disconnected) return MurderAttemptResult.SuppressKill; // Allow killing players in vents compared to vanilla code

            // Handle jinx shot
            if (checkIfJinxed(playerById(killer.PlayerId))) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
                writer.Write(killer.PlayerId);
                writer.Write((byte)0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setJinxed(killer.PlayerId, 0);

                return MurderAttemptResult.JinxKill;
            }

            // Check eatenPlayer
            else if (checkIfEaten(playerById(killer.PlayerId))) {
                return MurderAttemptResult.JinxKill;
            }

            // Block impostor shielded kill
            else if (Squire.shielded != null && Squire.shielded == target) {
                killer.MurderPlayer(Squire.shielded, MurderResultFlags.FailedProtected);
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.ShieldedMurderAttempt, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.shieldedMurderAttempt();
                return MurderAttemptResult.SuppressKill;
            }

            // Block impostor jailer kill and teleport the killer to prison
            else if (Jailer.jailedPlayer != null && Jailer.jailedPlayer == target) {
                List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(killer);
                RoleInfo roleInfo = infos.FirstOrDefault();
                if (killer.Data.Role.IsImpostor || roleInfo.TeamId == Team.Rebel) {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.PrisonPlayer, Hazel.SendOption.Reliable, -1);
                    writer.Write(killer.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.prisonPlayer(killer.PlayerId);
                }
                return MurderAttemptResult.SuppressKill;
            }

            // Block TimeTraveler with shield kill
            else if (TimeTraveler.shielded && TimeTraveler.timeTraveler != null && TimeTraveler.timeTraveler == target) {
                killer.MurderPlayer(TimeTraveler.timeTraveler, MurderResultFlags.FailedProtected);
                return MurderAttemptResult.SuppressKill;
            }

            // Demon try nun kill
            else if (Demon.bitten != null && !Demon.bitten.Data.IsDead) {
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

        public static MurderAttemptResult checkMurderAttemptAndKill(PlayerControl killer, PlayerControl target, bool showAnimation = true) {
            // The local player checks for the validity of the kill and performs it afterwards (different to vanilla, where the host performs all the checks)
            // The kill attempt will be shared using a custom RPC, hence combining modded and unmodded versions is impossible

            MurderAttemptResult murder = checkMurderAttempt(killer, target);
            if (murder == MurderAttemptResult.PerformKill) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
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

        public static bool AnySabotageActive() {           

            foreach (PlayerTask task in PlayerInCache.LocalPlayer.PlayerControl.myTasks) {
                if (PlayerTask.TaskIsEmergency(task) || task.TaskType == TaskTypes.MushroomMixupSabotage) {
                    return true;
                }
            }

            return false;
        }

        public static bool MushroomSabotageActive() {
            foreach (PlayerTask task in PlayerInCache.LocalPlayer.PlayerControl.myTasks) {
                if (task.TaskType == TaskTypes.MushroomMixupSabotage) {
                    return true;
                }
            }

            return false;
        }

        public static void enableCursor(string mode) {
            if (mode == "start") {
                Texture2D sprite = Helpers.loadTextureFromResources("LasMonjas.Images.Cursor.png");
                Cursor.SetCursor(sprite, Vector2.zero, CursorMode.Auto);
                return;
            }
            if (LasMonjasPlugin.MonjaCursor.Value) {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            else {
                Texture2D sprite = Helpers.loadTextureFromResources("LasMonjas.Images.Cursor.png");
                Cursor.SetCursor(sprite, Vector2.zero, CursorMode.Auto);
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
            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, false); // This will move button positions to the correct position.
        }

        public static AudioClip GetIntroSound(RoleTypes roleType) {
            return RoleManager.Instance.AllRoles.ToArray().Where((role) => role.Role == roleType).FirstOrDefault().IntroSound;
        }

        public static void playEndMusic(int whichTeamMusic) {
            MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
            writermusic.Write(whichTeamMusic);
            AmongUsClient.Instance.FinishRpcImmediately(writermusic);
            RPCProcedure.changeMusic((byte)whichTeamMusic);
        }

        public static void alphaPlayer(byte playerId, float alpha) {
            PlayerControl player = playerById(playerId);

            player.cosmetics.nameText.color = new Color(player.cosmetics.nameText.color.r, player.cosmetics.nameText.color.g, player.cosmetics.nameText.color.b, alpha);
            player.cosmetics.colorBlindText.color = new Color(player.cosmetics.colorBlindText.color.r, player.cosmetics.colorBlindText.color.g, player.cosmetics.colorBlindText.color.b, alpha);
            if (!player.cosmetics.currentPet.data.IsEmpty) {
                if (player.cosmetics.currentPet.renderers[0] != null && player.cosmetics.currentPet.shadows[0] != null) {
                    player.cosmetics.currentPet.renderers[0].color = new Color(player.cosmetics.currentPet.renderers[0].color.r, player.cosmetics.currentPet.renderers[0].color.g, player.cosmetics.currentPet.renderers[0].color.b, alpha);
                    player.cosmetics.currentPet.shadows[0].color = new Color(player.cosmetics.currentPet.shadows[0].color.r, player.cosmetics.currentPet.shadows[0].color.g, player.cosmetics.currentPet.shadows[0].color.b, alpha);
                }
            }
            if (player.cosmetics.hat != null) {
                player.cosmetics.hat.Parent.color = new Color(player.cosmetics.hat.Parent.color.r, player.cosmetics.hat.Parent.color.g, player.cosmetics.hat.Parent.color.b, alpha);
                player.cosmetics.hat.BackLayer.color = new Color(player.cosmetics.hat.BackLayer.color.r, player.cosmetics.hat.BackLayer.color.g, player.cosmetics.hat.BackLayer.color.b, alpha);
                player.cosmetics.hat.FrontLayer.color = new Color(player.cosmetics.hat.FrontLayer.color.r, player.cosmetics.hat.FrontLayer.color.g, player.cosmetics.hat.FrontLayer.color.b, alpha);
            }
            if (player.cosmetics.visor != null) {
                player.cosmetics.visor.Image.color = new Color(player.cosmetics.visor.Image.color.r, player.cosmetics.visor.Image.color.g, player.cosmetics.visor.Image.color.b, alpha);
            }
            player.MyPhysics.myPlayer.cosmetics.skin.layer.color = new Color(player.MyPhysics.myPlayer.cosmetics.skin.layer.color.r, player.MyPhysics.myPlayer.cosmetics.skin.layer.color.g, player.MyPhysics.myPlayer.cosmetics.skin.layer.color.b, alpha);
        }

        public static void turnIntoImpostor(PlayerControl player) {
            player.Data.Role.TeamType = RoleTeamTypes.Impostor;
            RoleManager.Instance.SetRole(player, RoleTypes.Impostor);
            player.SetKillTimer(GameOptionsManager.Instance.currentGameOptions.GetFloat(FloatOptionNames.KillCooldown));

            foreach (var localPlayer in PlayerInCache.AllPlayers) {
                if (localPlayer.PlayerControl.Data.Role.IsImpostor && PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor) {
                    player.cosmetics.nameText.color = Palette.ImpostorRed;
                }
            }
        }
        public static void turnIntoCrewmate(PlayerControl player) {
            player.Data.Role.TeamType = RoleTeamTypes.Crewmate;
            RoleManager.Instance.SetRole(player, RoleTypes.Crewmate);

            foreach (var localPlayer in PlayerInCache.AllPlayers) {
                if (!localPlayer.PlayerControl.Data.Role.IsImpostor && !PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor) {
                    player.cosmetics.nameText.color = Palette.CrewmateBlue;
                }
            }
        }

        public static void activateDleksMap() {
            if (activatedDleks == false && !CustomOptionHolder.activateSenseiMap.getBool() && CustomOptionHolder.activateDleksMap.getBool() && GameOptionsManager.Instance.currentGameOptions.MapId == 0) {
                GameObject dleksMap = GameObject.Find("SkeldShip(Clone)");
                dleksMap.transform.localScale = new Vector3(dleksMap.transform.localScale.x * -1, dleksMap.transform.localScale.y, dleksMap.transform.localScale.z);
                activatedDleks = true;
            }
        }
        public static void activateSenseiMap () {
            bool activeSensei = CustomOptionHolder.activateSenseiMap.getBool();

            int senseiMapHide = customSkeldHS; 

            if (GameOptionsManager.Instance.currentGameOptions.MapId == 0 && activatedSensei == false) {
                if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek && senseiMapHide >= 50 || GameOptionsManager.Instance.currentGameMode == GameModes.Normal && activeSensei) {
                    // Spawn map + assign shadow and materials layers
                    GameObject senseiMap = GameObject.Instantiate(CustomMain.customAssets.customMap, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
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
        public static bool isNeutral(PlayerControl player) {
            RoleInfo roleInfo = RoleInfo.getRoleInfoForPlayer(player).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.TeamId == Team.Neutral;
            return false;
        }
        public static bool isRebel(PlayerControl player) {
            RoleInfo roleInfo = RoleInfo.getRoleInfoForPlayer(player).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.TeamId == Team.Rebel;
            return false;
        }
        public static void ClearRivalPlayer() {
            // Notify players about clearing rivalplayer
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ChallengerSetRival, Hazel.SendOption.Reliable, -1);
            writer.Write(byte.MaxValue);
            writer.Write(byte.MaxValue);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.challengerSetRival(byte.MaxValue, byte.MaxValue);
        }

        private static Sprite menuBackground;

        public static Sprite getMenuBackground() {
            if (menuBackground != null) return menuBackground;
            menuBackground = loadSpriteFromResources("LasMonjas.Images.RoleListScreen.png", 110f);
            return menuBackground;
        }
        
        private static Sprite roleSummaryBackground;

        public static Sprite getRoleSummaryBackground() {
            if (roleSummaryBackground != null) return roleSummaryBackground;
            roleSummaryBackground = loadSpriteFromResources("LasMonjas.Images.TeamScreen.png", 110f);
            return roleSummaryBackground;
        }
        
        private static readonly Regex MatchLengthRegex = new("<color=#[A-Z0-9]{8}>(.+)<\\/color>");
        public static string WrapText(string text, int width = 52) {
            List<string> newLines = new();
            string[] lines = text.Split("\n");

            foreach (string line in lines) {
                if (line.Length <= width) {
                    newLines.Add(line);
                    continue;
                }

                List<List<string>> splitLines = new()
                {
                    new List<string>()
                };
                int length = 0;
                foreach (string word in line.Split(" ")) {
                    Match matches = MatchLengthRegex.Match(word);
                    int trueLength = matches.Groups.Count >= 2 ? matches.Groups[1].Value.Length : word.Length;
                    if (length + trueLength > width) {
                        splitLines.Add(new List<string>());
                        length = 0;
                    }
                    splitLines[^1].Add(word);
                    length += trueLength;
                }

                foreach (List<string> splitLine in splitLines)
                    newLines.Add(string.Join(' ', splitLine));
            }

            return string.Join('\n', newLines);
        }

        public static void UpdateLanguageForRoleSummary() {
            RoleInfo.mimic.name = Language.roleInfoRoleNames[0];
            RoleInfo.mimic.SettingsDescription = Language.impSummaryTexts[0];
            RoleInfo.painter.name = Language.roleInfoRoleNames[1];
            RoleInfo.painter.SettingsDescription = Language.impSummaryTexts[1];
            RoleInfo.demon.name = Language.roleInfoRoleNames[2];
            RoleInfo.demon.SettingsDescription = Language.impSummaryTexts[2];
            RoleInfo.janitor.name = Language.roleInfoRoleNames[3];
            RoleInfo.janitor.SettingsDescription = Language.impSummaryTexts[3];
            RoleInfo.illusionist.name = Language.roleInfoRoleNames[4];
            RoleInfo.illusionist.SettingsDescription = Language.impSummaryTexts[4];
            RoleInfo.manipulator.name = Language.roleInfoRoleNames[5];
            RoleInfo.manipulator.SettingsDescription = Language.impSummaryTexts[5];
            RoleInfo.bomberman.name = Language.roleInfoRoleNames[6];
            RoleInfo.bomberman.SettingsDescription = Language.impSummaryTexts[6];
            RoleInfo.chameleon.name = Language.roleInfoRoleNames[7];
            RoleInfo.chameleon.SettingsDescription = Language.impSummaryTexts[7];
            RoleInfo.gambler.name = Language.roleInfoRoleNames[8];
            RoleInfo.gambler.SettingsDescription = Language.impSummaryTexts[8];
            RoleInfo.sorcerer.name = Language.roleInfoRoleNames[9];
            RoleInfo.sorcerer.SettingsDescription = Language.impSummaryTexts[9];
            RoleInfo.medusa.name = Language.roleInfoRoleNames[10];
            RoleInfo.medusa.SettingsDescription = Language.impSummaryTexts[10];
            RoleInfo.hypnotist.name = Language.roleInfoRoleNames[11];
            RoleInfo.hypnotist.SettingsDescription = Language.impSummaryTexts[11];
            RoleInfo.archer.name = Language.roleInfoRoleNames[12];
            RoleInfo.archer.SettingsDescription = Language.impSummaryTexts[12];
            RoleInfo.plumber.name = Language.roleInfoRoleNames[13];
            RoleInfo.plumber.SettingsDescription = Language.impSummaryTexts[13];
            RoleInfo.librarian.name = Language.roleInfoRoleNames[14];
            RoleInfo.librarian.SettingsDescription = Language.impSummaryTexts[14];

            RoleInfo.renegade.name = Language.roleInfoRoleNames[15];
            RoleInfo.renegade.SettingsDescription = Language.rebelSummaryTexts[0];
            RoleInfo.bountyHunter.name = Language.roleInfoRoleNames[17];
            RoleInfo.bountyHunter.SettingsDescription = Language.rebelSummaryTexts[1];
            RoleInfo.trapper.name = Language.roleInfoRoleNames[18];
            RoleInfo.trapper.SettingsDescription = Language.rebelSummaryTexts[2];
            RoleInfo.yinyanger.name = Language.roleInfoRoleNames[19];
            RoleInfo.yinyanger.SettingsDescription = Language.rebelSummaryTexts[3];
            RoleInfo.challenger.name = Language.roleInfoRoleNames[20];
            RoleInfo.challenger.SettingsDescription = Language.rebelSummaryTexts[4];
            RoleInfo.ninja.name = Language.roleInfoRoleNames[21];
            RoleInfo.ninja.SettingsDescription = Language.rebelSummaryTexts[5];
            RoleInfo.berserker.name = Language.roleInfoRoleNames[22];
            RoleInfo.berserker.SettingsDescription = Language.rebelSummaryTexts[6];
            RoleInfo.yandere.name = Language.roleInfoRoleNames[23];
            RoleInfo.yandere.SettingsDescription = Language.rebelSummaryTexts[7];
            RoleInfo.stranded.name = Language.roleInfoRoleNames[24];
            RoleInfo.stranded.SettingsDescription = Language.rebelSummaryTexts[8];
            RoleInfo.monja.name = Language.roleInfoRoleNames[25];
            RoleInfo.monja.SettingsDescription = Language.rebelSummaryTexts[9];

            RoleInfo.joker.name = Language.roleInfoRoleNames[26];
            RoleInfo.joker.SettingsDescription = Language.neutralSummaryTexts[0];
            RoleInfo.rolethief.name = Language.roleInfoRoleNames[27];
            RoleInfo.rolethief.SettingsDescription = Language.neutralSummaryTexts[1];
            RoleInfo.pyromaniac.name = Language.roleInfoRoleNames[28];
            RoleInfo.pyromaniac.SettingsDescription = Language.neutralSummaryTexts[2];
            RoleInfo.treasureHunter.name = Language.roleInfoRoleNames[29];
            RoleInfo.treasureHunter.SettingsDescription = Language.neutralSummaryTexts[3];
            RoleInfo.devourer.name = Language.roleInfoRoleNames[30];
            RoleInfo.devourer.SettingsDescription = Language.neutralSummaryTexts[4];
            RoleInfo.poisoner.name = Language.roleInfoRoleNames[31];
            RoleInfo.poisoner.SettingsDescription = Language.neutralSummaryTexts[5];
            RoleInfo.puppeteer.name = Language.roleInfoRoleNames[32];
            RoleInfo.puppeteer.SettingsDescription = Language.neutralSummaryTexts[6];
            RoleInfo.exiler.name = Language.roleInfoRoleNames[33];
            RoleInfo.exiler.SettingsDescription = Language.neutralSummaryTexts[7];
            RoleInfo.amnesiac.name = Language.roleInfoRoleNames[34];
            RoleInfo.amnesiac.SettingsDescription = Language.neutralSummaryTexts[8];
            RoleInfo.seeker.name = Language.roleInfoRoleNames[35]; 
            RoleInfo.seeker.SettingsDescription = Language.neutralSummaryTexts[9];

            RoleInfo.captain.name = Language.roleInfoRoleNames[36];
            RoleInfo.captain.SettingsDescription = Language.crewSummaryTexts[0];
            RoleInfo.mechanic.name = Language.roleInfoRoleNames[37];
            RoleInfo.mechanic.SettingsDescription = Language.crewSummaryTexts[1];
            RoleInfo.sheriff.name = Language.roleInfoRoleNames[38];
            RoleInfo.sheriff.SettingsDescription = Language.crewSummaryTexts[2];
            RoleInfo.detective.name = Language.roleInfoRoleNames[39];
            RoleInfo.detective.SettingsDescription = Language.crewSummaryTexts[3];
            RoleInfo.forensic.name = Language.roleInfoRoleNames[40];
            RoleInfo.forensic.SettingsDescription = Language.crewSummaryTexts[4];
            RoleInfo.timeTraveler.name = Language.roleInfoRoleNames[41];
            RoleInfo.timeTraveler.SettingsDescription = Language.crewSummaryTexts[5];
            RoleInfo.squire.name = Language.roleInfoRoleNames[42];
            RoleInfo.squire.SettingsDescription = Language.crewSummaryTexts[6];
            RoleInfo.cheater.name = Language.roleInfoRoleNames[43];
            RoleInfo.cheater.SettingsDescription = Language.crewSummaryTexts[7];
            RoleInfo.fortuneTeller.name = Language.roleInfoRoleNames[44];
            RoleInfo.fortuneTeller.SettingsDescription = Language.crewSummaryTexts[8];
            RoleInfo.hacker.name = Language.roleInfoRoleNames[45];
            RoleInfo.hacker.SettingsDescription = Language.crewSummaryTexts[9];
            RoleInfo.sleuth.name = Language.roleInfoRoleNames[46];
            RoleInfo.sleuth.SettingsDescription = Language.crewSummaryTexts[10];
            RoleInfo.fink.name = Language.roleInfoRoleNames[47];
            RoleInfo.fink.SettingsDescription = Language.crewSummaryTexts[11];
            RoleInfo.kid.name = Language.roleInfoRoleNames[48];
            RoleInfo.kid.SettingsDescription = Language.crewSummaryTexts[12];
            RoleInfo.welder.name = Language.roleInfoRoleNames[49];
            RoleInfo.welder.SettingsDescription = Language.crewSummaryTexts[13];
            RoleInfo.spiritualist.name = Language.roleInfoRoleNames[50];
            RoleInfo.spiritualist.SettingsDescription = Language.crewSummaryTexts[14];
            RoleInfo.coward.name = Language.roleInfoRoleNames[51];
            RoleInfo.coward.SettingsDescription = Language.crewSummaryTexts[18];
            RoleInfo.vigilant.name = Language.roleInfoRoleNames[52];
            RoleInfo.vigilant.SettingsDescription = Language.crewSummaryTexts[15];
            RoleInfo.hunter.name = Language.roleInfoRoleNames[53];
            RoleInfo.hunter.SettingsDescription = Language.crewSummaryTexts[16];
            RoleInfo.jinx.name = Language.roleInfoRoleNames[54];
            RoleInfo.jinx.SettingsDescription = Language.crewSummaryTexts[17];
            RoleInfo.bat.name = Language.roleInfoRoleNames[55];
            RoleInfo.bat.SettingsDescription = Language.crewSummaryTexts[19];
            RoleInfo.necromancer.name = Language.roleInfoRoleNames[56];
            RoleInfo.necromancer.SettingsDescription = Language.crewSummaryTexts[20];
            RoleInfo.engineer.name = Language.roleInfoRoleNames[57];
            RoleInfo.engineer.SettingsDescription = Language.crewSummaryTexts[21];
            RoleInfo.locksmith.name = Language.roleInfoRoleNames[58];
            RoleInfo.locksmith.SettingsDescription = Language.crewSummaryTexts[22];
            RoleInfo.taskMaster.name = Language.roleInfoRoleNames[59];
            RoleInfo.taskMaster.SettingsDescription = Language.crewSummaryTexts[23];
            RoleInfo.jailer.name = Language.roleInfoRoleNames[60];
            RoleInfo.jailer.SettingsDescription = Language.crewSummaryTexts[24];

            RoleInfo.lighter.name = Language.roleInfoRoleNames[63];
            RoleInfo.lighter.SettingsDescription = Language.modifierSummaryTexts[1];
            RoleInfo.blind.name = Language.roleInfoRoleNames[64];
            RoleInfo.blind.SettingsDescription = Language.modifierSummaryTexts[2];
            RoleInfo.flash.name = Language.roleInfoRoleNames[65];
            RoleInfo.flash.SettingsDescription = Language.modifierSummaryTexts[3];
            RoleInfo.bigchungus.name = Language.roleInfoRoleNames[66];
            RoleInfo.bigchungus.SettingsDescription = Language.modifierSummaryTexts[4];
            RoleInfo.theChosenOne.name = Language.roleInfoRoleNames[67];
            RoleInfo.theChosenOne.SettingsDescription = Language.modifierSummaryTexts[5];
            RoleInfo.performer.name = Language.roleInfoRoleNames[68];
            RoleInfo.performer.SettingsDescription = Language.modifierSummaryTexts[6];
            RoleInfo.pro.name = Language.roleInfoRoleNames[69];
            RoleInfo.pro.SettingsDescription = Language.modifierSummaryTexts[7];
            RoleInfo.paintball.name = Language.roleInfoRoleNames[70];
            RoleInfo.paintball.SettingsDescription = Language.modifierSummaryTexts[8];
            RoleInfo.electrician.name = Language.roleInfoRoleNames[71];
            RoleInfo.electrician.SettingsDescription = Language.modifierSummaryTexts[9];
            RoleInfo.lover.name = Language.roleInfoRoleNames[72];
            RoleInfo.lover.SettingsDescription = Language.modifierSummaryTexts[0];

            RoleInfo.captureTheFlag.name = Language.teamNames[2];
            RoleInfo.captureTheFlag.SettingsDescription = Language.gamemodeSummaryTexts[0];
            RoleInfo.policeandThiefs.name = Language.teamNames[3];
            RoleInfo.policeandThiefs.SettingsDescription = Language.gamemodeSummaryTexts[1];
            RoleInfo.kingOfTheHill.name = Language.teamNames[4];
            RoleInfo.kingOfTheHill.SettingsDescription = Language.gamemodeSummaryTexts[2];
            RoleInfo.hotPotatoMode.name = Language.teamNames[5];
            RoleInfo.hotPotatoMode.SettingsDescription = Language.gamemodeSummaryTexts[3];
            RoleInfo.zombieLaboratory.name = Language.teamNames[6];
            RoleInfo.zombieLaboratory.SettingsDescription = Language.gamemodeSummaryTexts[4];
            RoleInfo.battleRoyale.name = Language.teamNames[7];
            RoleInfo.battleRoyale.SettingsDescription = Language.gamemodeSummaryTexts[5];
            RoleInfo.monjaFestival.name = Language.teamNames[8];
            RoleInfo.monjaFestival.SettingsDescription = Language.gamemodeSummaryTexts[6];
        }

        public static Vector3 CTFstealerPlayerPos;
        public static Vector3 CTFredTeamPos;
        public static Vector3 CTFblueTeamPos;
        public static Vector3 CTFredFlagPos;
        public static Vector3 CTFredFlagBasePos;
        public static Vector3 CTFblueFlagPos;
        public static Vector3 CTFblueFlagBasePos;

        public static void CreateCTF() {
            
            CTFstealerPlayerPos = new Vector3();
            CTFredTeamPos = new Vector3();
            CTFblueTeamPos = new Vector3();
            CTFredFlagPos = new Vector3();
            CTFredFlagBasePos = new Vector3();
            CTFblueFlagPos = new Vector3();
            CTFblueFlagBasePos = new Vector3();

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                // Skeld / Custom Skeld
                case 0:
                    if (activatedSensei) {
                        CTFstealerPlayerPos = new Vector3(-3.65f, 5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFredTeamPos = new Vector3(-17.5f, -1.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFblueTeamPos = new Vector3(7.7f, -0.95f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFredFlagPos = new Vector3(-17.5f, -1.35f, 0.5f);
                        CTFredFlagBasePos = new Vector3(-17.5f, -1.4f, 1f);
                        CTFblueFlagPos = new Vector3(7.7f, -1.15f, 0.5f);
                        CTFblueFlagBasePos = new Vector3(7.7f, -1.2f, 1f);
                    }
                    else if (activatedDleks) {
                        CTFstealerPlayerPos = new Vector3(-6.35f, -7.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFredTeamPos = new Vector3(20.5f, -5.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFblueTeamPos = new Vector3(-16.5f, -4.45f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFredFlagPos = new Vector3(20.5f, -5.35f, 0.5f);
                        CTFredFlagBasePos = new Vector3(20.5f, -5.4f, 1f);
                        CTFblueFlagPos = new Vector3(-16.5f, -4.65f, 0.5f);
                        CTFblueFlagBasePos = new Vector3(-16.5f, -4.7f, 1f);
                    }
                    else {
                        CTFstealerPlayerPos = new Vector3(6.35f, -7.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFredTeamPos = new Vector3(-20.5f, -5.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFblueTeamPos = new Vector3(16.5f, -4.45f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        CTFredFlagPos = new Vector3(-20.5f, -5.35f, 0.5f);
                        CTFredFlagBasePos = new Vector3(-20.5f, -5.4f, 1f);
                        CTFblueFlagPos = new Vector3(16.5f, -4.65f, 0.5f);
                        CTFblueFlagBasePos = new Vector3(16.5f, -4.7f, 1f);
                    }
                    break;
                // Mira HQ
                case 1:
                    CTFstealerPlayerPos = new Vector3(17.75f, 24f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredTeamPos = new Vector3(2.53f, 10.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFblueTeamPos = new Vector3(23.25f, 5.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredFlagPos = new Vector3(2.525f, 10.55f, 0.5f);
                    CTFredFlagBasePos = new Vector3(2.53f, 10.5f, 1f);
                    CTFblueFlagPos = new Vector3(23.25f, 5.05f, 0.5f);
                    CTFblueFlagBasePos = new Vector3(23.25f, 5f, 1f);
                    break;
                    // Polus
                case 2:
                    CTFstealerPlayerPos = new Vector3(31.75f, -13f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredTeamPos = new Vector3(36.4f, -21.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFblueTeamPos = new Vector3(5.4f, -9.45f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredFlagPos = new Vector3(36.4f, -21.7f, 0.5f);
                    CTFredFlagBasePos = new Vector3(36.4f, -21.75f, 1f);
                    CTFblueFlagPos = new Vector3(5.4f, -9.65f, 0.5f);
                    CTFblueFlagBasePos = new Vector3(5.4f, -9.7f, 1f);
                    break;
                // Dleks
                case 3:
                    CTFstealerPlayerPos = new Vector3(-6.35f, -7.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredTeamPos = new Vector3(20.5f, -5.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFblueTeamPos = new Vector3(-16.5f, -4.45f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredFlagPos = new Vector3(20.5f, -5.35f, 0.5f);
                    CTFredFlagBasePos = new Vector3(20.5f, -5.4f, 1f);
                    CTFblueFlagPos = new Vector3(-16.5f, -4.65f, 0.5f);
                    CTFblueFlagBasePos = new Vector3(-16.5f, -4.7f, 1f);
                    break;
                // Airship
                case 4:
                    CTFstealerPlayerPos = new Vector3(10.25f, -15.35f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredTeamPos = new Vector3(-17.5f, -1f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFblueTeamPos = new Vector3(33.6f, 1.45f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredFlagPos = new Vector3(-17.5f, -1.2f, 0.5f);
                    CTFredFlagBasePos = new Vector3(-17.5f, -1.25f, 1f);
                    CTFblueFlagPos = new Vector3(33.6f, 1.25f, 0.5f);
                    CTFblueFlagBasePos = new Vector3(33.6f, 1.2f, 1f);
                    break;
                // Fungle
                case 5:
                    CTFstealerPlayerPos = new Vector3(2.85f, -5.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredTeamPos = new Vector3(-23f, -0.45f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFblueTeamPos = new Vector3(19.25f, 2.35f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredFlagPos = new Vector3(-23f, -0.65f, 0.5f);
                    CTFredFlagBasePos = new Vector3(-23, -0.7f, 1f);
                    CTFblueFlagPos = new Vector3(19.25f, 2.15f, 0.5f);
                    CTFblueFlagBasePos = new Vector3(19.25f, 2.1f, 1f);
                    break;
                // Submerged
                case 6:
                    CTFstealerPlayerPos = new Vector3(1f, 10f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredTeamPos = new Vector3(-8.35f, 28.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFblueTeamPos = new Vector3(12.5f, -31.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    CTFredFlagPos = new Vector3(-8.35f, 28.05f, 0.03f);
                    CTFredFlagBasePos = new Vector3(-8.35f, 28, 0.031f);
                    CTFblueFlagPos = new Vector3(12.5f, -31.45f, -0.011f);
                    CTFblueFlagBasePos = new Vector3(12.5f, -31.5f, -0.01f); 
                    
                    // Add another respawn on each floor
                    GameObject redteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    redteamfloor.name = "redteamfloor";
                    redteamfloor.transform.position = new Vector3(-14f, -27.5f, -0.01f);
                    GameObject blueteamfloor = GameObject.Instantiate(CustomMain.customAssets.bluefloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    blueteamfloor.name = "blueteamfloor";
                    blueteamfloor.transform.position = new Vector3(14.25f, 24.25f, 0.03f);
                    break;
            }

            foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                player.transform.position = CTFredTeamPos;
            }

            foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                player.transform.position = CTFblueTeamPos;
            }

            if (PlayerInCache.LocalPlayer.PlayerControl != null && !createdcapturetheflag) {
                clearAllTasks(PlayerInCache.LocalPlayer.PlayerControl);
                
                GameObject redflag = GameObject.Instantiate(CustomMain.customAssets.redflag, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                redflag.name = "redflag";
                redflag.transform.position = CTFredFlagPos;
                CaptureTheFlag.redflag = redflag;
                GameObject redflagbase = GameObject.Instantiate(CustomMain.customAssets.redflagbase, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                redflagbase.name = "redflagbase";
                redflagbase.transform.position = CTFredFlagBasePos;
                CaptureTheFlag.redflagbase = redflagbase;
                GameObject blueflag = GameObject.Instantiate(CustomMain.customAssets.blueflag, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                blueflag.name = "blueflag";
                blueflag.transform.position = CTFblueFlagPos;
                CaptureTheFlag.blueflag = blueflag;
                GameObject blueflagbase = GameObject.Instantiate(CustomMain.customAssets.blueflagbase, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                blueflagbase.name = "blueflagbase";
                blueflagbase.transform.position = CTFblueFlagBasePos;
                CaptureTheFlag.blueflagbase = blueflagbase;               

                if (CaptureTheFlag.stealerPlayer != null) {
                    CaptureTheFlag.stealerPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                    CaptureTheFlag.stealerPlayer.transform.position = CTFstealerPlayerPos;
                    CaptureTheFlag.stealerSpawns.Add(redflagbase);
                    CaptureTheFlag.stealerSpawns.Add(blueflagbase);
                }
                
                createdcapturetheflag = true;
            }
        }

        public static Vector3 PATpoliceTeamPos;
        public static Vector3 PATthiefTeamPos;
        public static Vector3 PATcellPos;
        public static Vector3 PATcellButtonPos;
        public static Vector3 PATjewelButtonPos;
        public static Vector3 PATthiefSpaceShipPos;
        public static Vector3 PATjewel01Pos;
        public static Vector3 PATjewel02Pos;
        public static Vector3 PATjewel03Pos;
        public static Vector3 PATjewel04Pos;
        public static Vector3 PATjewel05Pos;
        public static Vector3 PATjewel06Pos;
        public static Vector3 PATjewel07Pos;
        public static Vector3 PATjewel08Pos;
        public static Vector3 PATjewel09Pos;
        public static Vector3 PATjewel10Pos;
        public static Vector3 PATjewel11Pos;
        public static Vector3 PATjewel12Pos;
        public static Vector3 PATjewel13Pos;
        public static Vector3 PATjewel14Pos;
        public static Vector3 PATjewel15Pos;

        public static void CreatePAT() {

            PATpoliceTeamPos = new Vector3();
            PATthiefTeamPos = new Vector3();
            PATcellPos = new Vector3();
            PATcellButtonPos = new Vector3();
            PATjewelButtonPos = new Vector3();
            PATthiefSpaceShipPos = new Vector3();
            PATjewel01Pos = new Vector3();
            PATjewel02Pos = new Vector3();
            PATjewel03Pos = new Vector3();
            PATjewel04Pos = new Vector3();
            PATjewel05Pos = new Vector3();
            PATjewel06Pos = new Vector3();
            PATjewel07Pos = new Vector3();
            PATjewel08Pos = new Vector3();
            PATjewel09Pos = new Vector3();
            PATjewel10Pos = new Vector3();
            PATjewel11Pos = new Vector3();
            PATjewel12Pos = new Vector3();
            PATjewel13Pos = new Vector3();
            PATjewel14Pos = new Vector3();
            PATjewel15Pos = new Vector3();

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                // Skeld / Custom Skeld
                case 0:
                    if (activatedSensei) {
                        PATpoliceTeamPos = new Vector3(-12f, 5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        PATthiefTeamPos = new Vector3(13.75f, -0.2f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        PATcellPos = new Vector3(-12f, 7.2f, 0.5f);
                        PATcellButtonPos = new Vector3(-12f, 4.7f, 0.5f);
                        PATjewelButtonPos = new Vector3(13.75f, -0.42f, 0.5f);
                        PATthiefSpaceShipPos = new Vector3(17f, 0f, 0.6f);
                        PATjewel01Pos = new Vector3(6.95f, 4.95f, 1f);
                        PATjewel02Pos = new Vector3(-3.75f, 5.35f, 1f);
                        PATjewel03Pos = new Vector3(-7.7f, 11.3f, 1f);
                        PATjewel04Pos = new Vector3(-19.65f, 5.3f, 1f);
                        PATjewel05Pos = new Vector3(-19.65f, -8, 1f);
                        PATjewel06Pos = new Vector3(-5.45f, -13f, 1f);
                        PATjewel07Pos = new Vector3(-7.65f, -4.2f, 1f);
                        PATjewel08Pos = new Vector3(2f, -6.75f, 1f);
                        PATjewel09Pos = new Vector3(8.9f, 1.45f, 1f);
                        PATjewel10Pos = new Vector3(4.6f, -2.25f, 1f);
                        PATjewel11Pos = new Vector3(-5.05f, -0.88f, 1f);
                        PATjewel12Pos = new Vector3(-8.25f, -0.45f, 1f);
                        PATjewel13Pos = new Vector3(-19.75f, -1.55f, 1f);
                        PATjewel14Pos = new Vector3(-12.1f, -13.15f, 1f);
                        PATjewel15Pos = new Vector3(7.15f, -14.45f, 1f);
                    }
                    else if (activatedDleks) {
                        PATpoliceTeamPos = new Vector3(10.2f, 1.18f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        PATthiefTeamPos = new Vector3(1.31f, -16.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        PATcellPos = new Vector3(10.25f, 3.38f, 0.5f);
                        PATcellButtonPos = new Vector3(10.2f, 0.93f, 0.5f);
                        PATjewelButtonPos = new Vector3(-0.20f, -17.15f, 0.5f);
                        PATthiefSpaceShipPos = new Vector3(1.345f, -19.16f, 0.6f);
                        GameObject thiefspaceshiphatchDleks = GameObject.Instantiate(CustomMain.customAssets.thiefspaceshiphatch, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                        thiefspaceshiphatchDleks.name = "thiefspaceshiphatch";
                        thiefspaceshiphatchDleks.transform.position = new Vector3(1.345f, -19.16f, 0.6f);
                        PATjewel01Pos = new Vector3(18.65f, -9.9f, 1f);
                        PATjewel02Pos = new Vector3(21.5f, -2, 1f);
                        PATjewel03Pos = new Vector3(5.9f, -8.25f, 1f);
                        PATjewel04Pos = new Vector3(-4.5f, -7.5f, 1f);
                        PATjewel05Pos = new Vector3(-7.85f, -14.45f, 1f);
                        PATjewel06Pos = new Vector3(-6.65f, -4.8f, 1f);
                        PATjewel07Pos = new Vector3(-10.5f, 2.15f, 1f);
                        PATjewel08Pos = new Vector3(5.5f, 3.5f, 1f);
                        PATjewel09Pos = new Vector3(19, -1.2f, 1f);
                        PATjewel10Pos = new Vector3(21.5f, -8.35f, 1f);
                        PATjewel11Pos = new Vector3(12.5f, -3.75f, 1f);
                        PATjewel12Pos = new Vector3(5.9f, -5.25f, 1f);
                        PATjewel13Pos = new Vector3(-2.65f, -16.5f, 1f);
                        PATjewel14Pos = new Vector3(-16.75f, -4.75f, 1f);
                        PATjewel15Pos = new Vector3(-3.8f, 3.5f, 1f);
                    }
                    else {
                        PATpoliceTeamPos = new Vector3(-10.2f, 1.18f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        PATthiefTeamPos = new Vector3(-1.31f, -16.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        PATcellPos = new Vector3(-10.25f, 3.38f, 0.5f);
                        PATcellButtonPos = new Vector3(-10.2f, 0.93f, 0.5f);
                        PATjewelButtonPos = new Vector3(0.20f, -17.15f, 0.5f);
                        PATthiefSpaceShipPos = new Vector3(1.765f, -19.16f, 0.6f);
                        GameObject thiefspaceshiphatch = GameObject.Instantiate(CustomMain.customAssets.thiefspaceshiphatch, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                        thiefspaceshiphatch.name = "thiefspaceshiphatch";
                        thiefspaceshiphatch.transform.position = new Vector3(1.765f, -19.16f, 0.6f);
                        PATjewel01Pos = new Vector3(-18.65f, -9.9f, 1f);
                        PATjewel02Pos = new Vector3(-21.5f, -2, 1f);
                        PATjewel03Pos = new Vector3(-5.9f, -8.25f, 1f);
                        PATjewel04Pos = new Vector3(4.5f, -7.5f, 1f);
                        PATjewel05Pos = new Vector3(7.85f, -14.45f, 1f);
                        PATjewel06Pos = new Vector3(6.65f, -4.8f, 1f);
                        PATjewel07Pos = new Vector3(10.5f, 2.15f, 1f);
                        PATjewel08Pos = new Vector3(-5.5f, 3.5f, 1f);
                        PATjewel09Pos = new Vector3(-19, -1.2f, 1f);
                        PATjewel10Pos = new Vector3(-21.5f, -8.35f, 1f);
                        PATjewel11Pos = new Vector3(-12.5f, -3.75f, 1f);
                        PATjewel12Pos = new Vector3(-5.9f, -5.25f, 1f);
                        PATjewel13Pos = new Vector3(2.65f, -16.5f, 1f);
                        PATjewel14Pos = new Vector3(16.75f, -4.75f, 1f);
                        PATjewel15Pos = new Vector3(3.8f, 3.5f, 1f);
                    }
                    break;
                // Mira HQ
                case 1:
                    PATpoliceTeamPos = new Vector3(1.8f, -1f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATthiefTeamPos = new Vector3(17.75f, 11.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATcellPos = new Vector3(1.75f, 1.125f, 0.5f);
                    PATcellButtonPos = new Vector3(1.8f, -1.25f, 0.5f);
                    PATjewelButtonPos = new Vector3(18.5f, 13.85f, 0.5f);
                    PATthiefSpaceShipPos = new Vector3(21.4f, 14.2f, 0.6f);
                    PATjewel01Pos = new Vector3(-4.5f, 2.5f, 1f);
                    PATjewel02Pos = new Vector3(6.25f, 14f, 1f);
                    PATjewel03Pos = new Vector3(9.15f, 4.75f, 1f);
                    PATjewel04Pos = new Vector3(14.75f, 20.5f, 1f);
                    PATjewel05Pos = new Vector3(19.5f, 17.5f, 1f);
                    PATjewel06Pos = new Vector3(21, 24.1f, 1f);
                    PATjewel07Pos = new Vector3(19.5f, 4.75f, 1f);
                    PATjewel08Pos = new Vector3(28.25f, 0, 1f);
                    PATjewel09Pos = new Vector3(2.45f, 11.25f, 1f);
                    PATjewel10Pos = new Vector3(4.4f, 1.75f, 1f);
                    PATjewel11Pos = new Vector3(9.25f, 13f, 1f);
                    PATjewel12Pos = new Vector3(13.75f, 23.5f, 1f);
                    PATjewel13Pos = new Vector3(16, 4, 1f);
                    PATjewel14Pos = new Vector3(15.35f, -0.9f, 1f);
                    PATjewel15Pos = new Vector3(19.5f, -1.75f, 1f);
                    break;
                // Polus
                case 2:
                    PATpoliceTeamPos = new Vector3(8.18f, -7.4f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATthiefTeamPos = new Vector3(30f, -15.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATcellPos = new Vector3(8.25f, -5.15f, 0.5f);
                    PATcellButtonPos = new Vector3(8.2f, -7.5f, 0.5f);
                    PATjewelButtonPos = new Vector3(32.25f, -15.9f, 0.5f);
                    PATthiefSpaceShipPos = new Vector3(35.35f, -15.55f, 0.8f);
                    PATjewel01Pos = new Vector3(16.7f, -2.65f, 0.75f);
                    PATjewel02Pos = new Vector3(25.35f, -7.35f, 0.75f);
                    PATjewel03Pos = new Vector3(34.9f, -9.75f, 0.75f);
                    PATjewel04Pos = new Vector3(36.5f, -21.75f, 0.75f);
                    PATjewel05Pos = new Vector3(17.25f, -17.5f, 0.75f);
                    PATjewel06Pos = new Vector3(10.9f, -20.5f, -0.75f);
                    PATjewel07Pos = new Vector3(1.5f, -20.25f, 0.75f);
                    PATjewel08Pos = new Vector3(3f, -12f, 0.75f);
                    PATjewel09Pos = new Vector3(30f, -7.35f, 0.75f);
                    PATjewel10Pos = new Vector3(40.25f, -8f, 0.75f);
                    PATjewel11Pos = new Vector3(26f, -17.15f, 0.75f);
                    PATjewel12Pos = new Vector3(22f, -25.25f, 0.75f);
                    PATjewel13Pos = new Vector3(20.65f, -12f, 0.75f);
                    PATjewel14Pos = new Vector3(9.75f, -12.25f, 0.75f);
                    PATjewel15Pos = new Vector3(2.25f, -24f, 0.75f);
                    break;
                // Dleks
                case 3:
                    PATpoliceTeamPos = new Vector3(10.2f, 1.18f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATthiefTeamPos = new Vector3(1.31f, -16.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATcellPos = new Vector3(10.25f, 3.38f, 0.5f);
                    PATcellButtonPos = new Vector3(10.2f, 0.93f, 0.5f);
                    PATjewelButtonPos = new Vector3(-0.20f, -17.15f, 0.5f);
                    PATthiefSpaceShipPos = new Vector3(1.345f, -19.16f, 0.6f);
                    GameObject thiefspaceshiphatchdleks = GameObject.Instantiate(CustomMain.customAssets.thiefspaceshiphatch, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    thiefspaceshiphatchdleks.name = "thiefspaceshiphatch";
                    thiefspaceshiphatchdleks.transform.position = new Vector3(1.345f, -19.16f, 0.6f);
                    PATjewel01Pos = new Vector3(18.65f, -9.9f, 1f);
                    PATjewel02Pos = new Vector3(21.5f, -2, 1f);
                    PATjewel03Pos = new Vector3(5.9f, -8.25f, 1f);
                    PATjewel04Pos = new Vector3(-4.5f, -7.5f, 1f);
                    PATjewel05Pos = new Vector3(-7.85f, -14.45f, 1f);
                    PATjewel06Pos = new Vector3(-6.65f, -4.8f, 1f);
                    PATjewel07Pos = new Vector3(-10.5f, 2.15f, 1f);
                    PATjewel08Pos = new Vector3(5.5f, 3.5f, 1f);
                    PATjewel09Pos = new Vector3(19, -1.2f, 1f);
                    PATjewel10Pos = new Vector3(21.5f, -8.35f, 1f);
                    PATjewel11Pos = new Vector3(12.5f, -3.75f, 1f);
                    PATjewel12Pos = new Vector3(5.9f, -5.25f, 1f);
                    PATjewel13Pos = new Vector3(-2.65f, -16.5f, 1f);
                    PATjewel14Pos = new Vector3(-16.75f, -4.75f, 1f);
                    PATjewel15Pos = new Vector3(-3.8f, 3.5f, 1f);
                    break;
                // Airship
                case 4:
                    PATpoliceTeamPos = new Vector3(-18.5f, 0.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATthiefTeamPos = new Vector3(7.15f, -14.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATcellPos = new Vector3(-18.45f, 3.55f, 0.5f);
                    PATcellButtonPos = new Vector3(-18.5f, 0.5f, 0.5f);
                    PATjewelButtonPos = new Vector3(10.275f, -16.3f, -0.01f);
                    PATthiefSpaceShipPos = new Vector3(13.5f, -16f, 0.6f);
                    PATjewel01Pos = new Vector3(-23.5f, -1.5f, 1f);
                    PATjewel02Pos = new Vector3(-14.15f, -4.85f, 1f);
                    PATjewel03Pos = new Vector3(-13.9f, -16.25f, 1f);
                    PATjewel04Pos = new Vector3(-0.85f, -2.5f, 1f);
                    PATjewel05Pos = new Vector3(-5, 8.5f, 1f);
                    PATjewel06Pos = new Vector3(19.3f, -4.15f, 1f);
                    PATjewel07Pos = new Vector3(19.85f, 8, 1f);
                    PATjewel08Pos = new Vector3(28.85f, -1.75f, 1f);
                    PATjewel09Pos = new Vector3(-14.5f, -8.5f, 1f);
                    PATjewel10Pos = new Vector3(6.3f, -2.75f, 1f);
                    PATjewel11Pos = new Vector3(20.75f, 2.5f, 1f);
                    PATjewel12Pos = new Vector3(29.25f, 7, 1f);
                    PATjewel13Pos = new Vector3(37.5f, -3.5f, 1f);
                    PATjewel14Pos = new Vector3(25.2f, -8.75f, 1f);
                    PATjewel15Pos = new Vector3(16.3f, -11, 1f);
                    break;
                // Fungle
                case 5:
                    PATpoliceTeamPos = new Vector3(-22.5f, -0.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATthiefTeamPos = new Vector3(20f, 11f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATcellPos = new Vector3(-26.75f, -0.65f, 0.5f);
                    PATcellButtonPos = new Vector3(-24f, -0.5f, 0.5f);
                    PATjewelButtonPos = new Vector3(18f, 11.75f, 0.05f);
                    PATthiefSpaceShipPos = new Vector3(19f, 9.25f, 0.6f);
                    PATjewel01Pos = new Vector3(-18.25f, 5f, 1f);
                    PATjewel02Pos = new Vector3(-22.65f, -7.15f, 1f);
                    PATjewel03Pos = new Vector3(2, 4.35f, 1f);
                    PATjewel04Pos = new Vector3(-3.15f, -10.5f, 0.9f);
                    PATjewel05Pos = new Vector3(23.7f, -7.8f, 1f);
                    PATjewel06Pos = new Vector3(-4.75f, -1.75f, 1f);
                    PATjewel07Pos = new Vector3(8f, -10f, 1f);
                    PATjewel08Pos = new Vector3(7f, 1.75f, 1f);
                    PATjewel09Pos = new Vector3(13.25f, 10, 1f);
                    PATjewel10Pos = new Vector3(22.3f, 3.3f, 1f);
                    PATjewel11Pos = new Vector3(20.5f, 7.35f, 1f);
                    PATjewel12Pos = new Vector3(24.15f, 14.45f, 1f);
                    PATjewel13Pos = new Vector3(-16.12f, 0.7f, 1f);
                    PATjewel14Pos = new Vector3(1.65f, -1.5f, 1f);
                    PATjewel15Pos = new Vector3(10.5f, -12, 1f);
                    break;
                // Submerged
                case 6:
                    PATpoliceTeamPos = new Vector3(-8.45f, 27f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATthiefTeamPos = new Vector3(1f, 10f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    PATcellPos = new Vector3(-5.9f, 31.85f, 0.5f);
                    PATcellButtonPos = new Vector3(-6f, 28.5f, 0.03f);
                    PATjewelButtonPos = new Vector3(1f, 10f, 0.03f);
                    PATthiefSpaceShipPos = new Vector3(14.5f, -35f, -0.011f);
                    PATjewel01Pos = new Vector3(-15f, 17.5f, -1f);
                    PATjewel02Pos = new Vector3(8f, 32f, -1f);
                    PATjewel03Pos = new Vector3(-6.75f, 10f, -1f);
                    PATjewel04Pos = new Vector3(5.15f, 8f, -1f);
                    PATjewel05Pos = new Vector3(5f, -33.5f, -1f);
                    PATjewel06Pos = new Vector3(-4.15f, -33.5f, -1f);
                    PATjewel07Pos = new Vector3(-14f, -27.75f, -1f);
                    PATjewel08Pos = new Vector3(7.8f, -23.75f, -1f);
                    PATjewel09Pos = new Vector3(-6.75f, -42.75f, -1f);
                    PATjewel10Pos = new Vector3(13f, -25.25f, -1f);
                    PATjewel11Pos = new Vector3(-14f, -34.25f, -1f);
                    PATjewel12Pos = new Vector3(0f, -33.5f, -1f);
                    PATjewel13Pos = new Vector3(-6.5f, 14f, -1f);
                    PATjewel14Pos = new Vector3(14.25f, 24.5f, -1f);
                    PATjewel15Pos = new Vector3(-12.25f, 31f, -1f);

                    // Add another cell and deliver point on each floor
                    GameObject celltwo = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    celltwo.name = "celltwo";
                    celltwo.transform.position = new Vector3(-14.1f, -39f, -0.01f);
                    celltwo.gameObject.layer = 9;
                    celltwo.transform.GetChild(0).gameObject.layer = 9;
                    PoliceAndThief.celltwo = celltwo;
                    GameObject cellbuttontwo = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    cellbuttontwo.name = "cellbuttontwo";
                    cellbuttontwo.transform.position = new Vector3(-11f, -39.35f, -0.01f);
                    PoliceAndThief.cellbuttontwo = cellbuttontwo;
                    GameObject jewelbuttontwo = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    jewelbuttontwo.name = "jewelbuttontwo";
                    jewelbuttontwo.transform.position = new Vector3(13f, -32.5f, -0.01f);
                    PoliceAndThief.jewelbuttontwo = jewelbuttontwo;
                    GameObject thiefspaceshiptwo = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    thiefspaceshiptwo.name = "thiefspaceshiptwo";
                    thiefspaceshiptwo.transform.position = new Vector3(-2.75f, 9f, 0.031f);
                    thiefspaceshiptwo.transform.localScale = new Vector3(-1f, 1f, 1f); 
                    break;
            }

            foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                player.transform.position = PATpoliceTeamPos;
            }

            foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                player.transform.position = PATthiefTeamPos;
                if (player == PlayerInCache.LocalPlayer.PlayerControl) {
                    // Add Arrows pointing the release and deliver point
                    if (PoliceAndThief.localThiefReleaseArrow.Count == 0) {
                        PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                        PoliceAndThief.localThiefReleaseArrow[0].arrow.SetActive(true);
                        if (Helpers.isSubmergedMap()) {
                            PoliceAndThief.localThiefReleaseArrow.Add(new Arrow(Palette.PlayerColors[10]));
                            PoliceAndThief.localThiefReleaseArrow[1].arrow.SetActive(true);
                        }
                    }
                    if (PoliceAndThief.localThiefDeliverArrow.Count == 0) {
                        PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                        PoliceAndThief.localThiefDeliverArrow[0].arrow.SetActive(true);
                        if (Helpers.isSubmergedMap()) {
                            PoliceAndThief.localThiefDeliverArrow.Add(new Arrow(Palette.PlayerColors[16]));
                            PoliceAndThief.localThiefDeliverArrow[1].arrow.SetActive(true);
                        }
                    }
                }
            }            

            if (PlayerInCache.LocalPlayer.PlayerControl != null && !createdpoliceandthief) {
                clearAllTasks(PlayerInCache.LocalPlayer.PlayerControl);
                
                GameObject cell = GameObject.Instantiate(CustomMain.customAssets.cell, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                cell.name = "cell";
                cell.transform.position = PATcellPos;
                cell.gameObject.layer = 9;
                cell.transform.GetChild(0).gameObject.layer = 9;
                PoliceAndThief.cell = cell;
                GameObject cellbutton = GameObject.Instantiate(CustomMain.customAssets.freethiefbutton, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                cellbutton.name = "cellbutton";
                cellbutton.transform.position = PATcellButtonPos;
                PoliceAndThief.cellbutton = cellbutton;
                GameObject jewelbutton = GameObject.Instantiate(CustomMain.customAssets.jewelbutton, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewelbutton.name = "jewelbutton";
                jewelbutton.transform.position = PATjewelButtonPos;
                PoliceAndThief.jewelbutton = jewelbutton;
                GameObject thiefspaceship = GameObject.Instantiate(CustomMain.customAssets.thiefspaceship, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                thiefspaceship.name = "thiefspaceship";
                thiefspaceship.transform.position = PATthiefSpaceShipPos;

                // Spawn jewels
                GameObject jewel01 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel01.transform.position = PATjewel01Pos;
                jewel01.name = "jewel01";
                PoliceAndThief.jewel01 = jewel01;
                GameObject jewel02 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel02.transform.position = PATjewel02Pos;
                jewel02.name = "jewel02";
                PoliceAndThief.jewel02 = jewel02;
                GameObject jewel03 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel03.transform.position = PATjewel03Pos;
                jewel03.name = "jewel03";
                PoliceAndThief.jewel03 = jewel03;
                GameObject jewel04 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel04.transform.position = PATjewel04Pos;
                jewel04.name = "jewel04";
                PoliceAndThief.jewel04 = jewel04;
                GameObject jewel05 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel05.transform.position = PATjewel05Pos;
                jewel05.name = "jewel05";
                PoliceAndThief.jewel05 = jewel05;
                GameObject jewel06 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel06.transform.position = PATjewel06Pos;
                jewel06.name = "jewel06";
                PoliceAndThief.jewel06 = jewel06;
                GameObject jewel07 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel07.transform.position = PATjewel07Pos;
                jewel07.name = "jewel07";
                PoliceAndThief.jewel07 = jewel07;
                GameObject jewel08 = GameObject.Instantiate(CustomMain.customAssets.jeweldiamond, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel08.transform.position = PATjewel08Pos;
                jewel08.name = "jewel08";
                PoliceAndThief.jewel08 = jewel08;
                GameObject jewel09 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel09.transform.position = PATjewel09Pos;
                jewel09.name = "jewel09";
                PoliceAndThief.jewel09 = jewel09;
                GameObject jewel10 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel10.transform.position = PATjewel10Pos;
                jewel10.name = "jewel10";
                PoliceAndThief.jewel10 = jewel10;
                GameObject jewel11 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel11.transform.position = PATjewel11Pos;
                jewel11.name = "jewel11";
                PoliceAndThief.jewel11 = jewel11;
                GameObject jewel12 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel12.transform.position = PATjewel12Pos;
                jewel12.name = "jewel12";
                PoliceAndThief.jewel12 = jewel12;
                GameObject jewel13 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel13.transform.position = PATjewel13Pos;
                jewel13.name = "jewel13";
                PoliceAndThief.jewel13 = jewel13;
                GameObject jewel14 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel14.transform.position = PATjewel14Pos;
                jewel14.name = "jewel14";
                PoliceAndThief.jewel14 = jewel14;
                GameObject jewel15 = GameObject.Instantiate(CustomMain.customAssets.jewelruby, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                jewel15.transform.position = PATjewel15Pos;
                jewel15.name = "jewel15";
                PoliceAndThief.jewel15 = jewel15;
                PoliceAndThief.thiefTreasures.Add(jewel01);
                PoliceAndThief.thiefTreasures.Add(jewel02);
                PoliceAndThief.thiefTreasures.Add(jewel03);
                PoliceAndThief.thiefTreasures.Add(jewel04);
                PoliceAndThief.thiefTreasures.Add(jewel05);
                PoliceAndThief.thiefTreasures.Add(jewel06);
                PoliceAndThief.thiefTreasures.Add(jewel07);
                PoliceAndThief.thiefTreasures.Add(jewel08);
                PoliceAndThief.thiefTreasures.Add(jewel09);
                PoliceAndThief.thiefTreasures.Add(jewel10);
                PoliceAndThief.thiefTreasures.Add(jewel11);
                PoliceAndThief.thiefTreasures.Add(jewel12);
                PoliceAndThief.thiefTreasures.Add(jewel13);
                PoliceAndThief.thiefTreasures.Add(jewel14);
                PoliceAndThief.thiefTreasures.Add(jewel15);

                createdpoliceandthief = true;
            }
        }

        public static Vector3 KOTHusurperPlayerPos;
        public static Vector3 KOTHgreenTeamPos;
        public static Vector3 KOTHyellowTeamPos;
        public static Vector3 KOTHgreenTeamFloorPos;
        public static Vector3 KOTHyellowTeamFloorPos;
        public static Vector3 KOTHflagZoneOnePos;
        public static Vector3 KOTHzoneOnePos;
        public static Vector3 KOTHflagZoneTwoPos;
        public static Vector3 KOTHzoneTwoPos;
        public static Vector3 KOTHflagZoneThreePos;
        public static Vector3 KOTHzoneThreePos;

        public static void CreateKOTH() {

            KOTHusurperPlayerPos = new Vector3();
            KOTHgreenTeamPos = new Vector3();
            KOTHyellowTeamPos = new Vector3();
            KOTHgreenTeamFloorPos = new Vector3();
            KOTHyellowTeamFloorPos = new Vector3();
            KOTHflagZoneOnePos = new Vector3();
            KOTHzoneOnePos = new Vector3();
            KOTHflagZoneTwoPos = new Vector3();
            KOTHzoneTwoPos = new Vector3();
            KOTHflagZoneThreePos = new Vector3();
            KOTHzoneThreePos = new Vector3();

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                // Skeld / Custom Skeld
                case 0:
                    if (activatedSensei) {
                        KOTHusurperPlayerPos = new Vector3(-6.8f, 10.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHgreenTeamPos = new Vector3(-16.4f, -10.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHyellowTeamPos = new Vector3(7f, -14.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHgreenTeamFloorPos = new Vector3(-16.4f, -10.5f, 0.5f);
                        KOTHyellowTeamFloorPos = new Vector3(7f, -14.4f, 0.5f);
                        KOTHflagZoneOnePos = new Vector3(7.85f, -1.5f, 0.4f);
                        KOTHzoneOnePos = new Vector3(7.85f, -1.5f, 0.5f);
                        KOTHflagZoneTwoPos = new Vector3(-6.35f, -1.1f, 0.4f);
                        KOTHzoneTwoPos = new Vector3(-6.35f, -1.1f, 0.5f);
                        KOTHflagZoneThreePos = new Vector3(-12.15f, 7.35f, 0.4f);
                        KOTHzoneThreePos = new Vector3(-12.15f, 7.35f, 0.5f);
                    }
                    else if (activatedDleks) {
                        KOTHusurperPlayerPos = new Vector3(1f, 5.35f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHgreenTeamPos = new Vector3(7f, -8.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHyellowTeamPos = new Vector3(-6.25f, -3.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHgreenTeamFloorPos = new Vector3(7f, -8.5f, 0.5f);
                        KOTHyellowTeamFloorPos = new Vector3(-6.25f, -3.75f, 0.5f);
                        KOTHflagZoneOnePos = new Vector3(9.1f, -2.25f, 0.4f);
                        KOTHzoneOnePos = new Vector3(9.1f, -2.25f, 0.5f);
                        KOTHflagZoneTwoPos = new Vector3(-4.5f, -7.5f, 0.4f);
                        KOTHzoneTwoPos = new Vector3(-4.5f, -7.5f, 0.5f);
                        KOTHflagZoneThreePos = new Vector3(-3.25f, -15.5f, 0.4f);
                        KOTHzoneThreePos = new Vector3(-3.25f, -15.5f, 0.5f);
                    }
                    else {
                        KOTHusurperPlayerPos = new Vector3(-1f, 5.35f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHgreenTeamPos = new Vector3(-7f, -8.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHyellowTeamPos = new Vector3(6.25f, -3.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        KOTHgreenTeamFloorPos = new Vector3(-7f, -8.5f, 0.5f);
                        KOTHyellowTeamFloorPos = new Vector3(6.25f, -3.75f, 0.5f);
                        KOTHflagZoneOnePos = new Vector3(-9.1f, -2.25f, 0.4f);
                        KOTHzoneOnePos = new Vector3(-9.1f, -2.25f, 0.5f);
                        KOTHflagZoneTwoPos = new Vector3(4.5f, -7.5f, 0.4f);
                        KOTHzoneTwoPos = new Vector3(4.5f, -7.5f, 0.5f);
                        KOTHflagZoneThreePos = new Vector3(3.25f, -15.5f, 0.4f);
                        KOTHzoneThreePos = new Vector3(3.25f, -15.5f, 0.5f);
                    }
                    break;
                // Mira HQ
                case 1:
                    KOTHusurperPlayerPos = new Vector3(2.5f, 11f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamPos = new Vector3(-4.45f, 1.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHyellowTeamPos = new Vector3(19.5f, 4.7f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamFloorPos = new Vector3(-4.45f, 1.5f, 0.5f);
                    KOTHyellowTeamFloorPos = new Vector3(19.5f, 4.45f, 0.5f);
                    KOTHflagZoneOnePos = new Vector3(15.25f, 4f, 0.4f);
                    KOTHzoneOnePos = new Vector3(15.25f, 4f, 0.5f);
                    KOTHflagZoneTwoPos = new Vector3(17.85f, 19.5f, 0.4f);
                    KOTHzoneTwoPos = new Vector3(17.85f, 19.5f, 0.5f);
                    KOTHflagZoneThreePos = new Vector3(6.15f, 12.5f, 0.4f);
                    KOTHzoneThreePos = new Vector3(6.15f, 12.5f, 0.5f);
                    break;
                // Polus
                case 2:
                    KOTHusurperPlayerPos = new Vector3(20.5f, -12f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamPos = new Vector3(2.25f, -23.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHyellowTeamPos = new Vector3(36.35f, -6.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamFloorPos = new Vector3(2.25f, -24f, 0.5f);
                    KOTHyellowTeamFloorPos = new Vector3(36.35f, -6.4f, 0.5f);
                    KOTHflagZoneOnePos = new Vector3(15f, -13.5f, 0.4f);
                    KOTHzoneOnePos = new Vector3(15f, -13.5f, 0.5f);
                    KOTHflagZoneTwoPos = new Vector3(20.75f, -22.75f, 0.4f);
                    KOTHzoneTwoPos = new Vector3(20.75f, -22.75f, 0.5f);
                    KOTHflagZoneThreePos = new Vector3(16.65f, -1.5f, 0.4f);
                    KOTHzoneThreePos = new Vector3(16.65f, -1.5f, 0.5f);
                    break;
                // Dleks
                case 3:
                    KOTHusurperPlayerPos = new Vector3(1f, 5.35f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamPos = new Vector3(7f, -8.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHyellowTeamPos = new Vector3(-6.25f, -3.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamFloorPos = new Vector3(7f, -8.5f, 0.5f);
                    KOTHyellowTeamFloorPos = new Vector3(-6.25f, -3.75f, 0.5f);
                    KOTHflagZoneOnePos = new Vector3(9.1f, -2.25f, 0.4f);
                    KOTHzoneOnePos = new Vector3(9.1f, -2.25f, 0.5f);
                    KOTHflagZoneTwoPos = new Vector3(-4.5f, -7.5f, 0.4f);
                    KOTHzoneTwoPos = new Vector3(-4.5f, -7.5f, 0.5f);
                    KOTHflagZoneThreePos = new Vector3(-3.25f, -15.5f, 0.4f);
                    KOTHzoneThreePos = new Vector3(-3.25f, -15.5f, 0.5f);
                    break;
                // Airship
                case 4:
                    KOTHusurperPlayerPos = new Vector3(12.25f, 2f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamPos = new Vector3(-13.9f, -14.45f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHyellowTeamPos = new Vector3(37.35f, -3.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamFloorPos = new Vector3(-13.9f, -14.7f, 0.5f);
                    KOTHyellowTeamFloorPos = new Vector3(37.35f, -3.5f, 0.5f);
                    KOTHflagZoneOnePos = new Vector3(-8.75f, 5.1f, 0.4f);
                    KOTHzoneOnePos = new Vector3(-8.75f, 5.1f, 0.5f);
                    KOTHflagZoneTwoPos = new Vector3(19.9f, 11.25f, 0.4f);
                    KOTHzoneTwoPos = new Vector3(19.9f, 11.25f, 0.5f);
                    KOTHflagZoneThreePos = new Vector3(16.3f, -8.6f, 0.4f);
                    KOTHzoneThreePos = new Vector3(16.3f, -8.6f, 0.5f);
                    break;
                // Fungle
                case 5:
                    KOTHusurperPlayerPos = new Vector3(-3.25f, -10.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamPos = new Vector3(-17.5f, 7f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHyellowTeamPos = new Vector3(21.5f, -6.85f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamFloorPos = new Vector3(-17.5f, 7.25f, 0.5f);
                    KOTHyellowTeamFloorPos = new Vector3(21.5f, -7.1f, 0.5f);
                    KOTHflagZoneOnePos = new Vector3(-17.5f, -7.25f, 0.4f);
                    KOTHzoneOnePos = new Vector3(-17.5f, -7.25f, 0.5f);
                    KOTHflagZoneTwoPos = new Vector3(10.7f, -12f, 0.4f);
                    KOTHzoneTwoPos = new Vector3(10.7f, -12f, 0.5f);
                    KOTHflagZoneThreePos = new Vector3(13.15f, 10f, 0.4f);
                    KOTHzoneThreePos = new Vector3(13.15f, 10f, 0.5f);
                    break;
                // Submerged
                case 6:
                    KOTHusurperPlayerPos = new Vector3(5.75f, 31.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamPos = new Vector3(-12.25f, 18.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHyellowTeamPos = new Vector3(-8.5f, -39.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    KOTHgreenTeamFloorPos = new Vector3(-12.25f, 18.25f, 0.03f);
                    GameObject greenteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    greenteamfloortwo.name = "greenteamfloortwo";
                    greenteamfloortwo.transform.position = new Vector3(-14.5f, -34.35f, -0.01f);
                    KOTHyellowTeamFloorPos = new Vector3(-8.5f, -39.5f, -0.01f);
                    GameObject yellowteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    yellowteamfloortwo.name = "yellowteamfloortwo";
                    yellowteamfloortwo.transform.position = new Vector3(0f, 33.5f, 0.03f);
                    KOTHflagZoneOnePos = new Vector3(1f, 10f, 0.029f);
                    KOTHzoneOnePos = new Vector3(1f, 10f, 0.03f);
                    KOTHflagZoneTwoPos = new Vector3(2.5f, -35.5f, -0.01f);
                    KOTHzoneTwoPos = new Vector3(2.5f, -35.5f, -0.011f);
                    KOTHflagZoneThreePos = new Vector3(10f, -31.5f, -0.01f);
                    KOTHzoneThreePos = new Vector3(10f, -31.5f, -0.011f);
                    KingOfTheHill.usurperSpawns.Add(greenteamfloortwo);
                    KingOfTheHill.usurperSpawns.Add(yellowteamfloortwo);
                    break;
            }

            foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                player.transform.position = KOTHgreenTeamPos;
            }

            foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                player.transform.position = KOTHyellowTeamPos;
            }

            if (PlayerInCache.LocalPlayer.PlayerControl != null && !createdkingofthehill) {
                clearAllTasks(PlayerInCache.LocalPlayer.PlayerControl);

                GameObject greenteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                greenteamfloor.name = "greenteamfloor";
                greenteamfloor.transform.position = KOTHgreenTeamFloorPos;
                GameObject yellowteamfloor = GameObject.Instantiate(CustomMain.customAssets.yellowfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                yellowteamfloor.name = "yellowteamfloor";
                yellowteamfloor.transform.position = KOTHyellowTeamFloorPos;
                GameObject greenkingaura = GameObject.Instantiate(CustomMain.customAssets.greenaura, KingOfTheHill.greenKingplayer.transform);
                greenkingaura.name = "greenkingaura";
                greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, 0.4f);
                KingOfTheHill.greenkingaura = greenkingaura;
                GameObject yellowkingaura = GameObject.Instantiate(CustomMain.customAssets.yellowaura, KingOfTheHill.yellowKingplayer.transform);
                yellowkingaura.name = "yellowkingaura";
                yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, 0.4f);
                KingOfTheHill.yellowkingaura = yellowkingaura;
                GameObject flagzoneone = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                flagzoneone.name = "flagzoneone";
                flagzoneone.transform.position = KOTHflagZoneOnePos;
                KingOfTheHill.flagzoneone = flagzoneone;
                GameObject zoneone = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                zoneone.name = "zoneone";
                zoneone.transform.position = KOTHzoneOnePos;
                KingOfTheHill.zoneone = zoneone;
                GameObject flagzonetwo = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                flagzonetwo.name = "flagzonetwo";
                flagzonetwo.transform.position = KOTHflagZoneTwoPos;
                KingOfTheHill.flagzonetwo = flagzonetwo;
                GameObject zonetwo = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                zonetwo.name = "zonetwo";
                zonetwo.transform.position = KOTHzoneTwoPos;
                KingOfTheHill.zonetwo = zonetwo;
                GameObject flagzonethree = GameObject.Instantiate(CustomMain.customAssets.whiteflag, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                flagzonethree.name = "flagzonethree";
                flagzonethree.transform.position = KOTHflagZoneThreePos;
                KingOfTheHill.flagzonethree = flagzonethree;
                GameObject zonethree = GameObject.Instantiate(CustomMain.customAssets.whitebase, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                zonethree.name = "zonethree";
                zonethree.transform.position = KOTHzoneThreePos;
                KingOfTheHill.zonethree = zonethree;
                KingOfTheHill.kingZones.Add(zoneone);
                KingOfTheHill.kingZones.Add(zonetwo);
                KingOfTheHill.kingZones.Add(zonethree);

                if (KingOfTheHill.usurperPlayer != null) {
                    KingOfTheHill.usurperPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                    KingOfTheHill.usurperPlayer.transform.position = KOTHusurperPlayerPos;
                    KingOfTheHill.usurperSpawns.Add(greenteamfloor);
                    KingOfTheHill.usurperSpawns.Add(yellowteamfloor);                    
                }

                if (Helpers.isSubmergedMap()) {
                    greenkingaura.transform.position = new Vector3(KingOfTheHill.greenKingplayer.transform.position.x, KingOfTheHill.greenKingplayer.transform.position.y, -0.5f);
                    yellowkingaura.transform.position = new Vector3(KingOfTheHill.yellowKingplayer.transform.position.x, KingOfTheHill.yellowKingplayer.transform.position.y, -0.5f);
                }

                createdkingofthehill = true;
            }
        }

        public static Vector3 HPhotPotatoPlayerPos;
        public static Vector3 HPnotPotatoTeamPos;

        public static void CreateHP() {

            HPhotPotatoPlayerPos = new Vector3();
            HPnotPotatoTeamPos = new Vector3();

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                // Skeld / Custom Skeld
                case 0:
                    if (activatedSensei) {
                        HPhotPotatoPlayerPos = new Vector3(-6.5f, -2.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        HPnotPotatoTeamPos = new Vector3(12.5f, -0.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    }
                    else if (activatedDleks) {
                        HPhotPotatoPlayerPos = new Vector3(0.75f, -7f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        HPnotPotatoTeamPos = new Vector3(-6.25f, -3.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    }
                    else {
                        HPhotPotatoPlayerPos = new Vector3(-0.75f, -7f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        HPnotPotatoTeamPos = new Vector3(6.25f, -3.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    }
                    break;
                // Mira HQ
                case 1:
                    HPhotPotatoPlayerPos = new Vector3(6.15f, 6.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    HPnotPotatoTeamPos = new Vector3(17.75f, 11.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    break;
                // Polus
                case 2:
                    HPhotPotatoPlayerPos = new Vector3(20.5f, -11.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    HPnotPotatoTeamPos = new Vector3(12.25f, -16f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    break;
                // Dleks
                case 3:
                    HPhotPotatoPlayerPos = new Vector3(0.75f, -7f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    HPnotPotatoTeamPos = new Vector3(-6.25f, -3.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    break;
                // Airship
                case 4:
                    HPhotPotatoPlayerPos = new Vector3(12.25f, 2f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    HPnotPotatoTeamPos = new Vector3(6.25f, 2.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    break;
                // Fungle
                case 5:
                    HPhotPotatoPlayerPos = new Vector3(-10.75f, 12.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    HPnotPotatoTeamPos = new Vector3(-3.25f, -10.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    break;
                // Submerged
                case 6:
                    HPhotPotatoPlayerPos = new Vector3(-4.25f, -33.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    HPnotPotatoTeamPos = new Vector3(13f, -25.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    break;
            }

            HotPotato.hotPotatoPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
            HotPotato.hotPotatoPlayer.transform.position = HPhotPotatoPlayerPos;
            
            foreach (PlayerControl player in HotPotato.notPotatoTeam) {
                player.transform.position = HPnotPotatoTeamPos;
            }

            if (PlayerInCache.LocalPlayer.PlayerControl != null && !createdhotpotato) {
                clearAllTasks(PlayerInCache.LocalPlayer.PlayerControl);

                GameObject hotpotato = GameObject.Instantiate(CustomMain.customAssets.hotPotato, HotPotato.hotPotatoPlayer.transform);
                hotpotato.name = "hotpotato";
                hotpotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                HotPotato.hotPotato = hotpotato;

                HudManager.Instance.DangerMeter.gameObject.SetActive(true);

                createdhotpotato = true;
            }
        }

        public static Vector3 ZLzombieTeamPos;
        public static Vector3 ZLsurvivorTeamPos;
        public static Vector3 ZLnursePos;
        public static Vector3 ZLmedkitOnePos;
        public static Vector3 ZLmedkitTwoPos;
        public static Vector3 ZLmedkitThreePos;
        public static Vector3 ZLlaboratoryPos;

        public static void CreateZL() {

            ZLzombieTeamPos = new Vector3();
            ZLsurvivorTeamPos = new Vector3();
            ZLnursePos = new Vector3();
            ZLmedkitOnePos = new Vector3();
            ZLmedkitTwoPos = new Vector3();
            ZLmedkitThreePos = new Vector3();
            ZLlaboratoryPos = new Vector3();

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                // Skeld / Custom Skeld
                case 0:
                    if (activatedSensei) {
                        ZLzombieTeamPos = new Vector3(-4.85f, 6, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLsurvivorTeamPos = new Vector3(4.75f, -8.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLnursePos = new Vector3(-12f, 7.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLmedkitOnePos = new Vector3(-6.5f, -0.85f, -0.1f);
                        ZLmedkitTwoPos = new Vector3(-18.85f, 2f, -0.1f);
                        ZLmedkitThreePos = new Vector3(-5.75f, 11.75f, -0.1f);
                        ZLlaboratoryPos = new Vector3(-12f, 7.2f, 0.5f);
                    }
                    else if (activatedDleks) {
                        ZLzombieTeamPos = new Vector3(17.25f, -13.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLsurvivorTeamPos = new Vector3(-11.75f, -4.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLnursePos = new Vector3(10.2f, 3.6f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLmedkitOnePos = new Vector3(7.25f, -5f, -0.1f);
                        ZLmedkitTwoPos = new Vector3(-3.75f, 3.5f, -0.1f);
                        ZLmedkitThreePos = new Vector3(13.75f, -3.75f, -0.1f);
                        ZLlaboratoryPos = new Vector3(10.25f, 3.38f, 0.5f);
                    }
                    else {
                        ZLzombieTeamPos = new Vector3(-17.25f, -13.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLsurvivorTeamPos = new Vector3(11.75f, -4.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLnursePos = new Vector3(-10.2f, 3.6f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        ZLmedkitOnePos = new Vector3(-7.25f, -5f, -0.1f);
                        ZLmedkitTwoPos = new Vector3(3.75f, 3.5f, -0.1f);
                        ZLmedkitThreePos = new Vector3(-13.75f, -3.75f, -0.1f);
                        ZLlaboratoryPos = new Vector3(-10.25f, 3.38f, 0.5f);
                    }
                    break;
                // Mira HQ
                case 1:
                    ZLzombieTeamPos = new Vector3(18.5f, -1.85f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLsurvivorTeamPos = new Vector3(6.1f, 5.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLnursePos = new Vector3(1.8f, 1.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLmedkitOnePos = new Vector3(16.25f, 0.25f, -0.1f);
                    ZLmedkitTwoPos = new Vector3(8.5f, 13.75f, -0.1f);
                    ZLmedkitThreePos = new Vector3(-4.5f, 3.5f, -0.1f);
                    ZLlaboratoryPos = new Vector3(1.75f, 1.125f, 0.5f);
                    break;
                // Polus
                case 2:
                    ZLzombieTeamPos = new Vector3(17.15f, -17.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLsurvivorTeamPos = new Vector3(40.4f, -6.8f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLnursePos = new Vector3(16.65f, -2.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLmedkitOnePos = new Vector3(20.75f, -12f, -0.1f);
                    ZLmedkitTwoPos = new Vector3(3.5f, -11.75f, -0.1f);
                    ZLmedkitThreePos = new Vector3(31.5f, -7.5f, -0.1f);
                    ZLlaboratoryPos = new Vector3(16.68f, -2.52f, 0.5f);
                    break;
                // Dleks
                case 3:
                    ZLzombieTeamPos = new Vector3(17.25f, -13.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLsurvivorTeamPos = new Vector3(-11.75f, -4.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLnursePos = new Vector3(10.2f, 3.6f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLmedkitOnePos = new Vector3(7.25f, -5f, -0.1f);
                    ZLmedkitTwoPos = new Vector3(-3.75f, 3.5f, -0.1f);
                    ZLmedkitThreePos = new Vector3(13.75f, -3.75f, -0.1f);
                    ZLlaboratoryPos = new Vector3(10.25f, 3.38f, 0.5f);
                    break;
                // Airship
                case 4:
                    ZLzombieTeamPos = new Vector3(32.35f, 7.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLsurvivorTeamPos = new Vector3(25.25f, -8.65f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLnursePos = new Vector3(-18.5f, 2.9f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLmedkitOnePos = new Vector3(-12f, 2.5f, -0.1f);
                    ZLmedkitTwoPos = new Vector3(-13.5f, -9.75f, -0.1f);
                    ZLmedkitThreePos = new Vector3(-8.85f, 7.5f, -0.1f);
                    ZLlaboratoryPos = new Vector3(-18.45f, 3f, 0.5f);
                    ZombieLaboratory.nursePlayerInsideLaboratory = false;
                    break;
                // Fungle
                case 5:
                    ZLzombieTeamPos = new Vector3(-4.25f, -10.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLsurvivorTeamPos = new Vector3(6.5f, 2.85f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLnursePos = new Vector3(-26.75f, -0.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLmedkitOnePos = new Vector3(-2.75f, -0.15f, -0.1f);
                    ZLmedkitTwoPos = new Vector3(-10, -12.25f, -0.1f);
                    ZLmedkitThreePos = new Vector3(-9.25f, 6.4f, -0.1f);
                    ZLlaboratoryPos = new Vector3(-27f, -0.65f, 0.5f);
                    break;
                // Submerged
                case 6:
                    ZLzombieTeamPos = new Vector3(1f, 10f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLsurvivorTeamPos = new Vector3(5.5f, 31.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLnursePos = new Vector3(-6f, 31.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    ZLmedkitOnePos = new Vector3(0f, 32f, -1f);
                    ZLmedkitTwoPos = new Vector3(6f, -34f, -1f);
                    ZLmedkitThreePos = new Vector3(-11.25f, -27.75f, -1f);
                    ZLlaboratoryPos = new Vector3(-5.9f, 31.85f, 0.5f);
                    GameObject laboratorytwo = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    laboratorytwo.name = "laboratorytwo";
                    laboratorytwo.transform.position = new Vector3(-14.1f, -39f, -0.01f);
                    laboratorytwo.gameObject.layer = 9;
                    laboratorytwo.transform.GetChild(0).gameObject.layer = 9;
                    ZombieLaboratory.laboratorytwo = laboratorytwo;
                    ZombieLaboratory.laboratorytwoEnterButton = laboratorytwo.transform.GetChild(1).gameObject;
                    ZombieLaboratory.laboratorytwoEnterButton.transform.position = new Vector3(-10.08f, -39.5f, -0.11f);
                    ZombieLaboratory.laboratorytwoExitButton = laboratorytwo.transform.GetChild(2).gameObject;
                    ZombieLaboratory.laboratorytwoCreateCureButton = laboratorytwo.transform.GetChild(3).gameObject;
                    ZombieLaboratory.laboratorytwoPutKeyItemButton = laboratorytwo.transform.GetChild(4).gameObject;
                    ZombieLaboratory.laboratorytwoExitLeftButton = laboratorytwo.transform.GetChild(5).gameObject;
                    ZombieLaboratory.laboratorytwoExitRightButton = laboratorytwo.transform.GetChild(6).gameObject;
                    ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratorytwoEnterButton);
                    ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratorytwoExitButton);
                    ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratorytwoExitLeftButton);
                    ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratorytwoExitRightButton);
                    ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratorytwoEnterButton);
                    ZombieLaboratory.nursePlayerInsideLaboratory = false;
                    break;
            }

            foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                player.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                player.transform.position = ZLzombieTeamPos;
            }

            foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                if (player == PlayerInCache.LocalPlayer.PlayerControl && PlayerInCache.LocalPlayer.PlayerControl != ZombieLaboratory.nursePlayer) {
                    player.transform.position = ZLsurvivorTeamPos;
                    // Add Arrows pointing the deliver point
                    if (ZombieLaboratory.localSurvivorsDeliverArrow.Count == 0) {
                        ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                        ZombieLaboratory.localSurvivorsDeliverArrow[0].arrow.SetActive(true);
                        if (Helpers.isSubmergedMap()) {
                            ZombieLaboratory.localSurvivorsDeliverArrow.Add(new Arrow(Palette.PlayerColors[3]));
                            ZombieLaboratory.localSurvivorsDeliverArrow[1].arrow.SetActive(true);
                        }
                    }
                }
            }

            if (PlayerInCache.LocalPlayer.PlayerControl == ZombieLaboratory.nursePlayer) {
                ZombieLaboratory.nursePlayer.transform.position = ZLnursePos; ;
                GameObject mapMedKit = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                mapMedKit.name = "mapMedKit";
                mapMedKit.transform.position = ZLmedkitOnePos;
                GameObject mapMedKittwo = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                mapMedKittwo.name = "mapMedKittwo";
                mapMedKittwo.transform.position = ZLmedkitTwoPos;
                GameObject mapMedKitthree = GameObject.Instantiate(CustomMain.customAssets.mapMedKit, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                mapMedKitthree.name = "mapMedKitthree";
                mapMedKitthree.transform.position = ZLmedkitThreePos;
                ZombieLaboratory.nurseMedkits.Add(mapMedKit);
                ZombieLaboratory.nurseMedkits.Add(mapMedKittwo);
                ZombieLaboratory.nurseMedkits.Add(mapMedKitthree);
                // Add Arrows pointing the medkit only for nurse
                if (ZombieLaboratory.localNurseArrows.Count == 0 && ZombieLaboratory.localNurseArrows.Count < 3) {
                    ZombieLaboratory.localNurseArrows.Add(new Arrow(Locksmith.color));
                    ZombieLaboratory.localNurseArrows.Add(new Arrow(Locksmith.color));
                    ZombieLaboratory.localNurseArrows.Add(new Arrow(Locksmith.color));
                }
                ZombieLaboratory.localNurseArrows[0].arrow.SetActive(true);
                ZombieLaboratory.localNurseArrows[1].arrow.SetActive(true);
                ZombieLaboratory.localNurseArrows[2].arrow.SetActive(true);
            }

            if (PlayerInCache.LocalPlayer.PlayerControl != null && !createdzombielaboratory) {
                clearAllTasks(PlayerInCache.LocalPlayer.PlayerControl);

                GameObject laboratory = GameObject.Instantiate(CustomMain.customAssets.laboratory, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                laboratory.name = "laboratory";
                laboratory.transform.position = ZLlaboratoryPos;
                laboratory.gameObject.layer = 9;
                laboratory.transform.GetChild(0).gameObject.layer = 9;
                ZombieLaboratory.laboratory = laboratory;
                ZombieLaboratory.laboratoryEnterButton = laboratory.transform.GetChild(1).gameObject;
                if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                    ZombieLaboratory.laboratoryEnterButton.transform.position = new Vector3(-23.6f, -0.73f, 0.04f);
                }
                else if (Helpers.isSubmergedMap()) {
                    ZombieLaboratory.laboratoryEnterButton.transform.position = new Vector3(-5.7f, 29.47f, -0.01f);
                }
                ZombieLaboratory.laboratoryExitButton = laboratory.transform.GetChild(2).gameObject;
                ZombieLaboratory.laboratoryCreateCureButton = laboratory.transform.GetChild(3).gameObject;
                ZombieLaboratory.laboratoryPutKeyItemButton = laboratory.transform.GetChild(4).gameObject;
                ZombieLaboratory.laboratoryExitLeftButton = laboratory.transform.GetChild(5).gameObject;
                ZombieLaboratory.laboratoryExitRightButton = laboratory.transform.GetChild(6).gameObject;
                ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryEnterButton);
                ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitButton);
                ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitLeftButton);
                ZombieLaboratory.nurseExits.Add(ZombieLaboratory.laboratoryExitRightButton);

                GameObject nurseMedKit = GameObject.Instantiate(CustomMain.customAssets.nurseMedKit, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                nurseMedKit.name = "nurseMedKit";
                nurseMedKit.transform.parent = ZombieLaboratory.nursePlayer.transform;
                nurseMedKit.transform.localPosition = new Vector3(0f, 0.7f, -0.1f);
                ZombieLaboratory.laboratoryNurseMedKit = nurseMedKit;
                ZombieLaboratory.laboratoryNurseMedKit.SetActive(false);
                ZombieLaboratory.laboratoryEntrances.Add(ZombieLaboratory.laboratoryEnterButton);

                createdzombielaboratory = true;
            }
        }

        public static Vector3 BRserialKillerPos;
        public static Vector3 BRlimeTeamPos;
        public static Vector3 BRpinkTeamPos;
        public static Vector3 BRlimeTeamFloorPos;
        public static Vector3 BRpinkTeamFloorPos;

        public static void CreateBR() {

            BRserialKillerPos = new Vector3();
            BRlimeTeamPos = new Vector3();
            BRpinkTeamPos = new Vector3();
            BRlimeTeamFloorPos = new Vector3();
            BRpinkTeamFloorPos = new Vector3();

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                // Skeld / Custom Skeld
                case 0:
                    if (activatedSensei) {
                        BRserialKillerPos = new Vector3(-3.65f, 5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRlimeTeamPos = new Vector3(-17.5f, -1.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRpinkTeamPos = new Vector3(7.7f, -0.95f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRlimeTeamFloorPos = new Vector3(-17.5f, -1.15f, 0.5f);
                        BRpinkTeamFloorPos = new Vector3(7.7f, -0.95f, 0.5f);
                    }
                    else if (activatedDleks) {
                        BRserialKillerPos = new Vector3(-6.35f, -7.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRlimeTeamPos = new Vector3(17f, -5.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRpinkTeamPos = new Vector3(-12f, -4.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRlimeTeamFloorPos = new Vector3(17f, -5.5f, 0.5f);
                        BRpinkTeamFloorPos = new Vector3(-12f, -4.75f, 0.5f);
                    }
                    else {
                        BRserialKillerPos = new Vector3(6.35f, -7.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRlimeTeamPos = new Vector3(-17f, -5.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRpinkTeamPos = new Vector3(12f, -4.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        BRlimeTeamFloorPos = new Vector3(-17f, -5.5f, 0.5f);
                        BRpinkTeamFloorPos = new Vector3(12f, -4.75f, 0.5f);
                    }
                    break;
                // Mira HQ
                case 1:
                    BRserialKillerPos = new Vector3(16.25f, 24.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamPos = new Vector3(6.15f, 13.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRpinkTeamPos = new Vector3(22.25f, 3f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamFloorPos = new Vector3(6.15f, 13.25f, 0.5f);
                    BRpinkTeamFloorPos = new Vector3(22.25f, 3f, 0.5f);                    
                    break;
                // Polus
                case 2:
                    BRserialKillerPos = new Vector3(22.3f, -19.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamPos = new Vector3(2.35f, -23.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRpinkTeamPos = new Vector3(36.35f, -8f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamFloorPos = new Vector3(2.35f, -23.75f, 0.5f);
                    BRpinkTeamFloorPos = new Vector3(36.35f, -8f, 0.5f);                    
                    break;
                // Dleks
                case 3:
                    BRserialKillerPos = new Vector3(-6.35f, -7.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamPos = new Vector3(17f, -5.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRpinkTeamPos = new Vector3(-12f, -4.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamFloorPos = new Vector3(17f, -5.5f, 0.5f);
                    BRpinkTeamFloorPos = new Vector3(-12f, -4.75f, 0.5f);
                    break;
                // Airship
                case 4:
                    BRserialKillerPos = new Vector3(12.25f, 2f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamPos = new Vector3(-13.9f, -14.45f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRpinkTeamPos = new Vector3(37.35f, -3.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamFloorPos = new Vector3(-13.9f, -14.45f, 0.5f);
                    BRpinkTeamFloorPos = new Vector3(37.35f, -3.25f, 0.5f);                    
                    break;
                // Fungle
                case 5:
                    BRserialKillerPos = new Vector3(9.35f, -9.85f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamPos = new Vector3(1.6f, -1.65f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRpinkTeamPos = new Vector3(6.75f, 2f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamFloorPos = new Vector3(1.6f, -1.65f, 0.5f);
                    BRpinkTeamFloorPos = new Vector3(6.75f, 2, 0.5f);                    
                    break;
                // Submerged
                case 6:
                    BRserialKillerPos = new Vector3(5.75f, 31.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamPos = new Vector3(-12.25f, 18.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRpinkTeamPos = new Vector3(-8.5f, -39.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    BRlimeTeamFloorPos = new Vector3(-12.25f, 18.5f, 0.03f);
                    BRpinkTeamFloorPos = new Vector3(-8.5f, -39.5f, -0.01f);
                    GameObject limeteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    limeteamfloortwo.name = "limeteamfloortwo";
                    limeteamfloortwo.transform.position = new Vector3(-14.5f, -34.35f, -0.01f);
                    GameObject pinkteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    pinkteamfloortwo.name = "pinkteamfloortwo";
                    pinkteamfloortwo.transform.position = new Vector3(0f, 33.5f, 0.03f);
                    BattleRoyale.serialKillerSpawns.Add(limeteamfloortwo);
                    BattleRoyale.serialKillerSpawns.Add(pinkteamfloortwo);
                    break;
            }

            if (BattleRoyale.matchType == 0) {
                foreach (PlayerControl soloPlayer in BattleRoyale.soloPlayerTeam) {
                    soloPlayer.transform.position = new Vector3(BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].x, BattleRoyale.soloPlayersSpawnPositions[howmanyBattleRoyaleplayers].y, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    howmanyBattleRoyaleplayers += 1;
                }
            }
            else {
                if (BattleRoyale.serialKiller != null) {
                    BattleRoyale.serialKiller.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                    BattleRoyale.serialKiller.transform.position = BRserialKillerPos;
                }

                foreach (PlayerControl player in BattleRoyale.limeTeam) {
                    player.transform.position = BRlimeTeamPos;
                }

                foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                    player.transform.position = BRpinkTeamPos;
                }
            }

            if (PlayerInCache.LocalPlayer.PlayerControl != null && !createdbattleroyale) {
                clearAllTasks(PlayerInCache.LocalPlayer.PlayerControl);

                if (BattleRoyale.matchType != 0) {
                    GameObject limeteamfloor = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    limeteamfloor.name = "limeteamfloor";
                    limeteamfloor.transform.position = BRlimeTeamFloorPos;
                    GameObject pinkteamfloor = GameObject.Instantiate(CustomMain.customAssets.redfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    pinkteamfloor.name = "pinkteamfloor";
                    pinkteamfloor.transform.position = BRpinkTeamFloorPos;
                    BattleRoyale.serialKillerSpawns.Add(limeteamfloor);
                    BattleRoyale.serialKillerSpawns.Add(pinkteamfloor);
                }

                createdbattleroyale = true;
            }
        }

        public static Vector3 MFbigMonjaPos;
        public static Vector3 MFgreenTeamPos;
        public static Vector3 MFcyanTeamPos;
        public static Vector3 MFbigSpawnOnePos;
        public static Vector3 MFbigSpawnTwoPos;
        public static Vector3 MFlittleSpawnOnePos;
        public static Vector3 MFlittleSpawnTwoPos;
        public static Vector3 MFlittleSpawnThreePos;
        public static Vector3 MFlittleSpawnFourPos;
        public static Vector3 MFgreenBasePos;
        public static Vector3 MFcyanBasePos;
        public static Vector3 MFgreyBasePos;
        public static Vector3 MFallulMonjaPos;

        public static void CreateMF() {

            MFbigMonjaPos = new Vector3();
            MFgreenTeamPos = new Vector3();
            MFcyanTeamPos = new Vector3();
            MFbigSpawnOnePos = new Vector3();
            MFbigSpawnTwoPos = new Vector3();
            MFlittleSpawnOnePos = new Vector3();
            MFlittleSpawnTwoPos = new Vector3();
            MFlittleSpawnThreePos = new Vector3();
            MFlittleSpawnFourPos = new Vector3();
            MFgreenBasePos = new Vector3();
            MFcyanBasePos = new Vector3();
            MFgreyBasePos = new Vector3();
            MFallulMonjaPos = new Vector3();

            switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                // Skeld / Custom Skeld
                case 0:
                    if (activatedSensei) {
                        MFbigMonjaPos = new Vector3(-12f, 7f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFgreenTeamPos = new Vector3(-10.5f, -10, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFcyanTeamPos = new Vector3(7.4f, -5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFbigSpawnOnePos = new Vector3(-6.2f, -1.4f, 1f);
                        MFbigSpawnTwoPos = new Vector3(-0.5f, 3f, 1f);
                        MFlittleSpawnOnePos = new Vector3(-6.75f, 10.5f, 0.5f);
                        MFlittleSpawnTwoPos = new Vector3(-17.5f, -1.5f, 0.5f);
                        MFlittleSpawnThreePos = new Vector3(4.5f, -14f, 0.5f);
                        MFlittleSpawnFourPos = new Vector3(-11.5f, -4f, 0.5f);
                        MFgreenBasePos = new Vector3(-10.5f, -10, 0.5f);
                        MFcyanBasePos = new Vector3(7.4f, -5f, 0.5f);
                        MFgreyBasePos = new Vector3(-12f, 7f, 0.5f);
                        MFallulMonjaPos = new Vector3(9.2f, 5f, 0.5f);
                    }
                    else if (activatedDleks) {
                        MFbigMonjaPos = new Vector3(-4.5f, -7.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFgreenTeamPos = new Vector3(9f, -2.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFcyanTeamPos = new Vector3(-5f, -15.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFbigSpawnOnePos = new Vector3(-9.5f, 0.9f, 1f);
                        MFbigSpawnTwoPos = new Vector3(17.15f, -13.25f, 1f);
                        MFlittleSpawnOnePos = new Vector3(20.5f, -5.5f, 0.5f);
                        MFlittleSpawnTwoPos = new Vector3(0.75f, 5.25f, 0.5f);
                        MFlittleSpawnThreePos = new Vector3(2.15f, -9.75f, 0.5f);
                        MFlittleSpawnFourPos = new Vector3(-16.5f, -4.7f, 0.5f);
                        MFgreenBasePos = new Vector3(9f, -2.5f, 0.5f);
                        MFcyanBasePos = new Vector3(-5f, -15.5f, 0.5f);
                        MFgreyBasePos = new Vector3(-4.5f, -7.25f, 0.5f);
                        MFallulMonjaPos = new Vector3(9.8f, -8.9f, 0.5f);
                        GameObject skeldBigYVentDleks = GameObject.Find("AdminVent");
                        skeldBigYVentDleks.transform.position = new Vector3(-2.25f, -15.25f, skeldBigYVentDleks.transform.position.z);
                    }
                    else {
                        MFbigMonjaPos = new Vector3(4.5f, -7.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFgreenTeamPos = new Vector3(-9f, -2.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFcyanTeamPos = new Vector3(5f, -15.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                        MFbigSpawnOnePos = new Vector3(9.5f, 0.9f, 1f);
                        MFbigSpawnTwoPos = new Vector3(-17.15f, -13.25f, 1f);
                        MFlittleSpawnOnePos = new Vector3(-20.5f, -5.5f, 0.5f);
                        MFlittleSpawnTwoPos = new Vector3(-0.75f, 5.25f, 0.5f);
                        MFlittleSpawnThreePos = new Vector3(-2.15f, -9.75f, 0.5f);
                        MFlittleSpawnFourPos = new Vector3(16.5f, -4.7f, 0.5f);
                        MFgreenBasePos = new Vector3(-9f, -2.5f, 0.5f);
                        MFcyanBasePos = new Vector3(5f, -15.5f, 0.5f);
                        MFgreyBasePos = new Vector3(4.5f, -7.25f, 0.5f);
                        MFallulMonjaPos = new Vector3(-9.8f, -8.9f, 0.5f);
                        GameObject skeldBigYVentDleks = GameObject.Find("AdminVent");
                        skeldBigYVentDleks.transform.position = new Vector3(2.25f, -15.25f, skeldBigYVentDleks.transform.position.z);
                    }
                    break;
                // Mira HQ
                case 1:
                    MFbigMonjaPos = new Vector3(-4.45f, 2f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFgreenTeamPos = new Vector3(23f, 4.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFcyanTeamPos = new Vector3(8.5f, 13f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFbigSpawnOnePos = new Vector3(15.25f, 4f, 1f);
                    MFbigSpawnTwoPos = new Vector3(17.85f, 23.25f, 1f);
                    MFlittleSpawnOnePos = new Vector3(19.5f, 4.55f, 0.5f);
                    MFlittleSpawnTwoPos = new Vector3(15f, 19.25f, 0.5f);
                    MFlittleSpawnThreePos = new Vector3(14.5f, 0.25f, 0.5f);
                    MFlittleSpawnFourPos = new Vector3(2.35f, 11.15f, 0.5f);
                    MFgreenBasePos = new Vector3(23f, 4.75f, 0.5f);
                    MFcyanBasePos = new Vector3(8.5f, 13f, 0.5f);
                    MFgreyBasePos = new Vector3(-4.45f, 2f, 0.5f);
                    MFallulMonjaPos = new Vector3(9.2f, 5f, 0.5f);
                    break;
                // Polus
                case 2:
                    MFbigMonjaPos = new Vector3(21.75f, -25.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFgreenTeamPos = new Vector3(31.5f, -7.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFcyanTeamPos = new Vector3(2.35f, -23.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFbigSpawnOnePos = new Vector3(26.2f, -17f, 1f);
                    MFbigSpawnTwoPos = new Vector3(7.45f, -9.5f, 1f);
                    MFlittleSpawnOnePos = new Vector3(36.5f, -21.5f, 0.5f);
                    MFlittleSpawnTwoPos = new Vector3(1.35f, -17f, 0.5f);
                    MFlittleSpawnThreePos = new Vector3(19.75f, -11.5f, 0.5f);
                    MFlittleSpawnFourPos = new Vector3(20.75f, -21.35f, 0.5f);
                    MFgreenBasePos = new Vector3(31.5f, -7.75f, 0.5f);
                    MFcyanBasePos = new Vector3(2.35f, -23.75f, 0.5f);
                    MFgreyBasePos = new Vector3(21.75f, -25.15f, 0.5f);
                    MFallulMonjaPos = new Vector3(4.65f, -4.5f, 0.5f);
                    break;
                // Dleks
                case 3:
                    MFbigMonjaPos = new Vector3(4.5f, -7.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFgreenTeamPos = new Vector3(-9f, -2.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFcyanTeamPos = new Vector3(5f, -15.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFbigSpawnOnePos = new Vector3(9.5f, 0.9f, 1f);
                    MFbigSpawnTwoPos = new Vector3(-17.15f, -13.25f, 1f);
                    MFlittleSpawnOnePos = new Vector3(-20.5f, -5.5f, 0.5f);
                    MFlittleSpawnTwoPos = new Vector3(-0.75f, 5.25f, 0.5f);
                    MFlittleSpawnThreePos = new Vector3(-2.15f, -9.75f, 0.5f);
                    MFlittleSpawnFourPos = new Vector3(16.5f, -4.7f, 0.5f);
                    MFgreenBasePos = new Vector3(-9f, -2.5f, 0.5f);
                    MFcyanBasePos = new Vector3(5f, -15.5f, 0.5f);
                    MFgreyBasePos = new Vector3(4.5f, -7.25f, 0.5f);
                    MFallulMonjaPos = new Vector3(-9.8f, -8.9f, 0.5f);
                    GameObject skeldBigYVent = GameObject.Find("AdminVent");
                    skeldBigYVent.transform.position = new Vector3(-2.25f, -15.25f, skeldBigYVent.transform.position.z);
                    break;
                // Airship
                case 4:
                    MFbigMonjaPos = new Vector3(6.35f, 2.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFgreenTeamPos = new Vector3(-10.15f, -6.75f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFcyanTeamPos = new Vector3(38.25f, 0f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFbigSpawnOnePos = new Vector3(-8.75f, 12.35f, 1f);
                    MFbigSpawnTwoPos = new Vector3(16.25f, -8.85f, 1f);
                    MFlittleSpawnOnePos = new Vector3(-23.5f, -1.35f, 0.5f);
                    MFlittleSpawnTwoPos = new Vector3(7f, -12.5f, 0.5f);
                    MFlittleSpawnThreePos = new Vector3(20f, 7.75f, 0.5f);
                    MFlittleSpawnFourPos = new Vector3(15.45f, 0f, 0.5f);
                    MFgreenBasePos = new Vector3(-10.15f, -6.75f, 0.5f);
                    MFcyanBasePos = new Vector3(38.25f, 0f, 0.5f);
                    MFgreyBasePos = new Vector3(6.35f, 2.5f, 0.5f);
                    MFallulMonjaPos = new Vector3(20.75f, 2.5f, 0.5f);
                    break;
                // Fungle
                case 5:
                    MFbigMonjaPos = new Vector3(-4.25f, -8.5f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFgreenTeamPos = new Vector3(-17.5f, 7.2f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFcyanTeamPos = new Vector3(12.5f, 10, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFbigSpawnOnePos = new Vector3(-0.65f, 4.25f, 1f);
                    MFbigSpawnTwoPos = new Vector3(23.25f, 13.5f, 1f);
                    MFlittleSpawnOnePos = new Vector3(-17.45f, -7.35f, 0.5f);
                    MFlittleSpawnTwoPos = new Vector3(10.85f, -15, 0.5f);
                    MFlittleSpawnThreePos = new Vector3(21.85f, -7.5f, 0.5f);
                    MFlittleSpawnFourPos = new Vector3(21.45f, 3, 0.5f);
                    MFgreenBasePos = new Vector3(-17.5f, 7.2f, 0.5f);
                    MFcyanBasePos = new Vector3(12.5f, 10, 0.5f);
                    MFgreyBasePos = new Vector3(-4.25f, -8.5f, 0.5f);
                    MFallulMonjaPos = new Vector3(1.5f, -1.5f, 0.5f);
                    break;
                // Submerged
                case 6:
                    MFbigMonjaPos = new Vector3(-12.2f, 19.15f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFgreenTeamPos = new Vector3(-1.8f, 12.25f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFcyanTeamPos = new Vector3(2.65f, -35.65f, PlayerInCache.LocalPlayer.PlayerControl.transform.position.z);
                    MFbigSpawnOnePos = new Vector3(-8.45f, 28.55f, 0.03f);
                    MFbigSpawnTwoPos = new Vector3(-8.45f, -39.65f, -0.01f);
                    MFlittleSpawnOnePos = new Vector3(5.35f, 31.35f, 0.03f);
                    MFlittleSpawnTwoPos = new Vector3(5.10f, 10.85f, 0.03f);
                    MFlittleSpawnThreePos = new Vector3(-11.45f, -31.15f, -0.01f);
                    MFlittleSpawnFourPos = new Vector3(12.65f, -31.85f, -0.01f);
                    MFgreenBasePos = new Vector3(-1.8f, 12.25f, 0.03f);
                    MFcyanBasePos = new Vector3(2.65f, -35.65f, -0.01f);
                    MFgreyBasePos = new Vector3(-12.2f, 19.15f, 0.03f);
                    MFallulMonjaPos = new Vector3(-14.5f, -34.25f, -0.01f);
                    GameObject greenteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.greenfloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    greenteamfloortwo.name = "greenteamfloortwo";
                    greenteamfloortwo.transform.position = new Vector3(-4.35f, -33.5f, -0.01f);
                    GameObject cyanteamfloortwo = GameObject.Instantiate(CustomMain.customAssets.bluefloor, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    cyanteamfloortwo.name = "cyanteamfloortwo";
                    cyanteamfloortwo.transform.position = new Vector3(-10.25f, 10.15f, 0.03f);
                    GameObject greyBasetwo = GameObject.Instantiate(CustomMain.customAssets.greyBaseEmpty, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                    greyBasetwo.name = "greyBase";
                    greyBasetwo.transform.position = new Vector3(7.15f, -20.5f, -0.01f);
                    MonjaFestival.bigMonjaBaseTwo = greyBasetwo;
                    MonjaFestival.bigMonjaSpawns.Add(greyBasetwo);
                    break;
            }

            if (MonjaFestival.bigMonjaPlayer != null) {
                MonjaFestival.bigMonjaPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                MonjaFestival.bigMonjaPlayer.transform.position = MFbigMonjaPos;
                GameObject greyBase = GameObject.Instantiate(CustomMain.customAssets.greyBaseEmpty, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                greyBase.name = "greyBase";
                greyBase.transform.position = MFgreyBasePos;
                MonjaFestival.bigMonjaBase = greyBase;
                MonjaFestival.bigMonjaSpawns.Add(greyBase);
            }

            foreach (PlayerControl player in MonjaFestival.greenTeam) {
                player.transform.position = MFgreenTeamPos;
            }

            foreach (PlayerControl player in MonjaFestival.cyanTeam) {
                player.transform.position = MFcyanTeamPos;
            }

            if (PlayerInCache.LocalPlayer.PlayerControl != null && !createdmonjafestival) {
                clearAllTasks(PlayerInCache.LocalPlayer.PlayerControl);

                GameObject bigSpawnOne = GameObject.Instantiate(CustomMain.customAssets.bigSpawnOneFull, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                bigSpawnOne.name = "bigSpawnOne";
                bigSpawnOne.transform.position = MFbigSpawnOnePos;
                MonjaFestival.bigSpawnOne = bigSpawnOne;
                MonjaFestival.bigSpawnOneCount = GameObject.Instantiate(HudManagerStartPatch.greenmonja01PickDeliverButton.actionButton.cooldownTimerText, MonjaFestival.bigSpawnOne.transform);
                MonjaFestival.bigSpawnOneCount.text = $"{MonjaFestival.bigSpawnOnePoints} / 30";
                MonjaFestival.bigSpawnOneCount.enableWordWrapping = false;
                MonjaFestival.bigSpawnOneCount.transform.localScale = Vector3.one * 0.5f;
                MonjaFestival.bigSpawnOneCount.transform.localPosition += new Vector3(0f, 0.75f, 0);

                GameObject bigSpawnTwo = GameObject.Instantiate(CustomMain.customAssets.bigSpawnOneFull, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                bigSpawnTwo.name = "bigSpawnTwo";
                bigSpawnTwo.transform.position = MFbigSpawnTwoPos;
                MonjaFestival.bigSpawnTwo = bigSpawnTwo;
                MonjaFestival.bigSpawnTwoCount = GameObject.Instantiate(HudManagerStartPatch.greenmonja01PickDeliverButton.actionButton.cooldownTimerText, MonjaFestival.bigSpawnTwo.transform);
                MonjaFestival.bigSpawnTwoCount.text = $"{MonjaFestival.bigSpawnTwoPoints} / 30";
                MonjaFestival.bigSpawnTwoCount.enableWordWrapping = false;
                MonjaFestival.bigSpawnTwoCount.transform.localScale = Vector3.one * 0.5f;
                MonjaFestival.bigSpawnTwoCount.transform.localPosition += new Vector3(0f, 0.75f, 0);

                GameObject littleSpawnOne = GameObject.Instantiate(CustomMain.customAssets.littleSpawnOneFull, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                littleSpawnOne.name = "littleSpawnOne";
                littleSpawnOne.transform.position = MFlittleSpawnOnePos;
                MonjaFestival.littleSpawnOne = littleSpawnOne;
                MonjaFestival.littleSpawnOneCount = GameObject.Instantiate(HudManagerStartPatch.greenmonja01PickDeliverButton.actionButton.cooldownTimerText, MonjaFestival.littleSpawnOne.transform);
                MonjaFestival.littleSpawnOneCount.text = $"{MonjaFestival.littleSpawnOnePoints} / 10";
                MonjaFestival.littleSpawnOneCount.enableWordWrapping = false;
                MonjaFestival.littleSpawnOneCount.transform.localScale = Vector3.one * 0.5f;
                MonjaFestival.littleSpawnOneCount.transform.localPosition += new Vector3(0f, 0.75f, 0);

                GameObject littleSpawnTwo = GameObject.Instantiate(CustomMain.customAssets.littleSpawnOneFull, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                littleSpawnTwo.name = "littleSpawnTwo";
                littleSpawnTwo.transform.position = MFlittleSpawnTwoPos;
                MonjaFestival.littleSpawnTwo = littleSpawnTwo;
                MonjaFestival.littleSpawnTwoCount = GameObject.Instantiate(HudManagerStartPatch.greenmonja01PickDeliverButton.actionButton.cooldownTimerText, MonjaFestival.littleSpawnTwo.transform);
                MonjaFestival.littleSpawnTwoCount.text = $"{MonjaFestival.littleSpawnTwoPoints} / 10";
                MonjaFestival.littleSpawnTwoCount.enableWordWrapping = false;
                MonjaFestival.littleSpawnTwoCount.transform.localScale = Vector3.one * 0.5f;
                MonjaFestival.littleSpawnTwoCount.transform.localPosition += new Vector3(0f, 0.75f, 0);

                GameObject littleSpawnThree = GameObject.Instantiate(CustomMain.customAssets.littleSpawnOneFull, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                littleSpawnThree.name = "littleSpawnThree";
                littleSpawnThree.transform.position = MFlittleSpawnThreePos;
                MonjaFestival.littleSpawnThree = littleSpawnThree;
                MonjaFestival.littleSpawnThreeCount = GameObject.Instantiate(HudManagerStartPatch.greenmonja01PickDeliverButton.actionButton.cooldownTimerText, MonjaFestival.littleSpawnThree.transform);
                MonjaFestival.littleSpawnThreeCount.text = $"{MonjaFestival.littleSpawnThreePoints} / 10";
                MonjaFestival.littleSpawnThreeCount.enableWordWrapping = false;
                MonjaFestival.littleSpawnThreeCount.transform.localScale = Vector3.one * 0.5f;
                MonjaFestival.littleSpawnThreeCount.transform.localPosition += new Vector3(0f, 0.75f, 0);

                GameObject littleSpawnFour = GameObject.Instantiate(CustomMain.customAssets.littleSpawnOneFull, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                littleSpawnFour.name = "littleSpawnFour";
                littleSpawnFour.transform.position = MFlittleSpawnFourPos;
                MonjaFestival.littleSpawnFour = littleSpawnFour;
                MonjaFestival.littleSpawnFourCount = GameObject.Instantiate(HudManagerStartPatch.greenmonja01PickDeliverButton.actionButton.cooldownTimerText, MonjaFestival.littleSpawnFour.transform);
                MonjaFestival.littleSpawnFourCount.text = $"{MonjaFestival.littleSpawnThreePoints} / 10";
                MonjaFestival.littleSpawnFourCount.enableWordWrapping = false;
                MonjaFestival.littleSpawnFourCount.transform.localScale = Vector3.one * 0.5f;
                MonjaFestival.littleSpawnFourCount.transform.localPosition += new Vector3(0f, 0.75f, 0);

                GameObject greenBase = GameObject.Instantiate(CustomMain.customAssets.greenBaseEmpty, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                greenBase.name = "greenBase";
                greenBase.transform.position = MFgreenBasePos;
                MonjaFestival.greenTeamBase = greenBase;
                GameObject cyanBase = GameObject.Instantiate(CustomMain.customAssets.cyanBaseEmpty, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                cyanBase.name = "cyanBase";
                cyanBase.transform.position = MFcyanBasePos;
                MonjaFestival.cyanTeamBase = cyanBase;

                GameObject allulMonja = GameObject.Instantiate(CustomMain.customAssets.floorAllulMonja, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                allulMonja.transform.position = MFallulMonjaPos;
                allulMonja.name = "allulMonja";
                MonjaFestival.allulMonja = allulMonja;
                Reactor.Utilities.Coroutines.Start(HudManagerUpdatePatch.allulMonjaReload());

                MonjaFestival.bigMonjaSpawns.Add(bigSpawnOne);
                MonjaFestival.bigMonjaSpawns.Add(bigSpawnTwo);
                MonjaFestival.bigMonjaSpawns.Add(littleSpawnOne);
                MonjaFestival.bigMonjaSpawns.Add(littleSpawnTwo);
                MonjaFestival.bigMonjaSpawns.Add(littleSpawnThree);
                MonjaFestival.bigMonjaSpawns.Add(littleSpawnFour);
                MonjaFestival.bigMonjaSpawns.Add(greenBase);
                MonjaFestival.bigMonjaSpawns.Add(cyanBase);
                MonjaFestival.bigMonjaSpawns.Add(allulMonja);

                createdmonjafestival = true;
            }
        }

        public static void RemoveObjectsOnGamemodes (int mapId) {
            switch (mapId) {
                case 0:
                case 3:
                    // Remove camera use and admin table on Skeld / Custom Skeld / Dleks
                    GameObject cameraStand = GameObject.Find("SurvConsole");
                    cameraStand.GetComponent<PolygonCollider2D>().enabled = false;
                    GameObject admin = GameObject.Find("MapRoomConsole");
                    admin.GetComponent<CircleCollider2D>().enabled = false;
                    break;
                case 1:
                    // Remove Doorlog use, Decontamination doors and admin table on MiraHQ
                    GameObject DoorLog = GameObject.Find("SurvLogConsole");
                    DoorLog.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject deconUpperDoor = GameObject.Find("UpperDoor");
                    deconUpperDoor.SetActive(false);
                    GameObject deconLowerDoor = GameObject.Find("LowerDoor");
                    deconLowerDoor.SetActive(false);
                    GameObject deconUpperDoorPanelTop = GameObject.Find("DeconDoorPanel-Top");
                    deconUpperDoorPanelTop.SetActive(false);
                    GameObject deconUpperDoorPanelHigh = GameObject.Find("DeconDoorPanel-High");
                    deconUpperDoorPanelHigh.SetActive(false);
                    GameObject deconUpperDoorPanelBottom = GameObject.Find("DeconDoorPanel-Bottom");
                    deconUpperDoorPanelBottom.SetActive(false);
                    GameObject deconUpperDoorPanelLow = GameObject.Find("DeconDoorPanel-Low");
                    deconUpperDoorPanelLow.SetActive(false);
                    GameObject miraAdmin = GameObject.Find("AdminMapConsole");
                    miraAdmin.GetComponent<CircleCollider2D>().enabled = false;
                    break;
                case 2:
                    // Remove Decon doors, camera use, vitals, admin tables on Polus
                    GameObject lowerdecon = GameObject.Find("LowerDecon");
                    lowerdecon.SetActive(false);
                    GameObject upperdecon = GameObject.Find("UpperDecon");
                    upperdecon.SetActive(false);
                    GameObject survCameras = GameObject.Find("Surv_Panel");
                    survCameras.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject vitals = GameObject.Find("panel_vitals");
                    vitals.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject adminone = GameObject.Find("panel_map");
                    adminone.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject admintwo = GameObject.Find("panel_map (1)");
                    admintwo.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject ramp = GameObject.Find("ramp");
                    ramp.transform.position = new Vector3(ramp.transform.position.x, ramp.transform.position.y, 0.75f);
                    break;
                case 4:
                    // Remove camera use, admin table, vitals, electrical doors on Airship
                    GameObject cameras = GameObject.Find("task_cams");
                    cameras.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject airshipadmin = GameObject.Find("panel_cockpit_map");
                    airshipadmin.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject airshipvitals = GameObject.Find("panel_vitals");
                    airshipvitals.GetComponent<CircleCollider2D>().enabled = false;

                    Helpers.GetStaticDoor("TopLeftVert").SetOpen(true);
                    Helpers.GetStaticDoor("TopLeftHort").SetOpen(true);
                    Helpers.GetStaticDoor("BottomHort").SetOpen(true);
                    Helpers.GetStaticDoor("TopCenterHort").SetOpen(true);
                    Helpers.GetStaticDoor("LeftVert").SetOpen(true);
                    Helpers.GetStaticDoor("RightVert").SetOpen(true);
                    Helpers.GetStaticDoor("TopRightVert").SetOpen(true);
                    Helpers.GetStaticDoor("TopRightHort").SetOpen(true);
                    Helpers.GetStaticDoor("BottomRightHort").SetOpen(true);
                    Helpers.GetStaticDoor("BottomRightVert").SetOpen(true);
                    Helpers.GetStaticDoor("LeftDoorTop").SetOpen(true);
                    Helpers.GetStaticDoor("LeftDoorBottom").SetOpen(true);

                    GameObject laddermeeting = GameObject.Find("ladder_meeting");
                    laddermeeting.SetActive(false);
                    GameObject platform = GameObject.Find("Platform");
                    platform.SetActive(false);
                    GameObject platformleft = GameObject.Find("PlatformLeft");
                    platformleft.SetActive(false);
                    GameObject platformright = GameObject.Find("PlatformRight");
                    platformright.SetActive(false);
                    GameObject recordsadmin = GameObject.Find("records_admin_map");
                    recordsadmin.GetComponent<BoxCollider2D>().enabled = false;
                    break;
                case 5:
                    // Remove Decon doors, camera use, vitals, admin tables on Fungle
                    GameObject binoculars = GameObject.Find("BinocularsSecurityConsole");
                    binoculars.GetComponent<PolygonCollider2D>().enabled = false;
                    GameObject mushrooms = GameObject.Find("FungleShip(Clone)/Outside/OutsideJungle/Mushrooms");
                    mushrooms.SetActive(false);               
                    GameObject labvitals = GameObject.Find("FungleShip(Clone)/Rooms/Laboratory/VitalsConsole");
                    labvitals.GetComponent<BoxCollider2D>().enabled = false;                    
                    break;
                case 6:
                    // Remove camera use, admin table, vitals, on Submerged
                    GameObject upperCentralVent = GameObject.Find("UpperCentralVent");
                    upperCentralVent.GetComponent<CircleCollider2D>().enabled = false;
                    upperCentralVent.GetComponent<PolygonCollider2D>().enabled = false;
                    GameObject lowerCentralVent = GameObject.Find("LowerCentralVent");
                    lowerCentralVent.GetComponent<BoxCollider2D>().enabled = false;
                    GameObject securityCams = GameObject.Find("SecurityConsole");
                    securityCams.GetComponent<PolygonCollider2D>().enabled = false;
                    GameObject submergedvitals = GameObject.Find("panel_vitals(Clone)");
                    submergedvitals.GetComponent<CircleCollider2D>().enabled = false;
                    GameObject submergedadminone = GameObject.Find("console-adm-admintable");
                    submergedadminone.GetComponent<CircleCollider2D>().enabled = false;
                    GameObject submergedadmintwo = GameObject.Find("console-adm-admintable (1)");
                    submergedadmintwo.GetComponent<CircleCollider2D>().enabled = false;
                    GameObject deconVLower = GameObject.Find("DeconDoorVLower");
                    deconVLower.SetActive(false);
                    GameObject deconVUpper = GameObject.Find("DeconDoorVUpper");
                    deconVUpper.SetActive(false);
                    GameObject deconHLower = GameObject.Find("DeconDoorHLower");
                    deconHLower.SetActive(false);
                    GameObject deconHUpper = GameObject.Find("DeconDoorHUpper");
                    deconHUpper.SetActive(false);
                    GameObject camsone = GameObject.Find("Submerged(Clone)/Cameras/LowerDeck/Electrical/FixConsole");
                    camsone.GetComponent<PolygonCollider2D>().enabled = false;
                    GameObject camstwo = GameObject.Find("Submerged(Clone)/Cameras/LowerDeck/Lobby/FixConsole");
                    camstwo.GetComponent<BoxCollider2D>().enabled = false;
                    camstwo.GetComponent<CircleCollider2D>().enabled = false;
                    GameObject camsthree = GameObject.Find("Submerged(Clone)/Cameras/UpperDeck/Comms/FixConsole");
                    camsthree.GetComponent<PolygonCollider2D>().enabled = false;
                    GameObject camsfour = GameObject.Find("Submerged(Clone)/Cameras/UpperDeck/Lobby/FixConsole");
                    camsfour.GetComponent<BoxCollider2D>().enabled = false;
                    camsfour.GetComponent<CircleCollider2D>().enabled = false;
                    GameObject camsfive = GameObject.Find("Submerged(Clone)/Cameras/UpperDeck/WestHallway/FixConsole");
                    camsfive.GetComponent<BoxCollider2D>().enabled = false;
                    camsfive.GetComponent<CircleCollider2D>().enabled = false;
                    GameObject camssix = GameObject.Find("Submerged(Clone)/Cameras/UpperDeck/YHallway/FixConsole");
                    camssix.GetComponent<BoxCollider2D>().enabled = false;
                    camssix.GetComponent<CircleCollider2D>().enabled = false;
                    GameObject camsseven = GameObject.Find("Submerged(Clone)/Cameras/LowerDeck/WestHallway/FixConsole");
                    camsseven.GetComponent<BoxCollider2D>().enabled = false;
                    break;
            }
        }
        public static bool checkIfJinxed(PlayerControl player) {
            return Jinx.jinxedList.Any(p => p.Data.PlayerId == player.Data.PlayerId);
        }

        public static void jinxedAction(PlayerControl player) {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(player.NetId, (byte)CustomRPC.SetJinxed, Hazel.SendOption.Reliable, -1);
            writer.Write(player.PlayerId);
            writer.Write((byte)0);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.setJinxed(player.PlayerId, 0);
        }

        public static void unpetrifyForMinigames(PlayerControl player) {
            var unPetrify = Medusa.petrifiedPlayers.FirstOrDefault(x => x.PlayerId == player.PlayerId);

            if (unPetrify == null)
                return;

            unPetrify.moveable = true;
            Medusa.petrifiedPlayers.Remove(unPetrify);
            GameObject petrifyZone = GameObject.Find(unPetrify.name + "petrifyZone");
            if (petrifyZone != null) {
                UnityEngine.Object.Destroy(petrifyZone);
            }

        }

        public static bool checkIfEaten(PlayerControl player) {
            return Devourer.eatenPlayers.Any(p => p.Data.PlayerId == player.Data.PlayerId);
        }

        public static void showGamemodesPopUp(int flag, PlayerControl player) {
            var popup = GameManagerCreator.Instance.HideAndSeekManagerPrefab.DeathPopupPrefab;

            var newPopUp = UnityEngine.Object.Instantiate(popup, HudManager.Instance.transform.parent);
            if (flag != 0) {
                newPopUp.gameObject.transform.GetChild(0).GetComponent<TextTranslatorTMP>().enabled = false;
            }
            switch (gameType) {
                case 2:
                    // CTF:
                    switch (flag) {
                        case 0: // kill flag player
                            if (player == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                                newPopUp.gameObject.transform.position += new Vector3(-3, -0.25f, 0);
                            }
                            else if (player == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                                newPopUp.gameObject.transform.position += new Vector3(3, -0.25f, 0);
                            }
                            break;
                        case 1: // new red player
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusCaptureTheFlagTexts[0];
                            newPopUp.gameObject.transform.position += new Vector3 (0, -0.25f, 0);
                            break;
                        case 2: // new blue player
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusCaptureTheFlagTexts[1];
                            newPopUp.gameObject.transform.position += new Vector3(0, -0.25f, 0);
                            break;
                        case 3: // steal blue flag
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusCaptureTheFlagTexts[2];
                            newPopUp.gameObject.transform.position += new Vector3(-3, -0.25f, 0);
                            break;
                        case 4: // steal red flag
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusCaptureTheFlagTexts[4];
                            newPopUp.gameObject.transform.position += new Vector3(3, -0.25f, 0);
                            break;
                        case 5: // score red
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusCaptureTheFlagTexts[5];
                            newPopUp.gameObject.transform.position += new Vector3(-3, -0.25f, 0);
                            break;
                        case 6: // score blue
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusCaptureTheFlagTexts[6];
                            newPopUp.gameObject.transform.position += new Vector3(3, -0.25f, 0);
                            break;
                    }
                    break;
                case 3:
                    // PAT:
                    switch (flag) {
                        case 1: // captured thief
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusPoliceAndThiefsTexts[0];
                            newPopUp.gameObject.transform.position += new Vector3(-3, -0.25f, 0);
                            break;
                        case 2: // release thief
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusPoliceAndThiefsTexts[1];
                            newPopUp.gameObject.transform.position += new Vector3(0, -0.25f, 0);
                            break;
                        case 3: // deliver jewel
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusPoliceAndThiefsTexts[2];
                            newPopUp.gameObject.transform.position += new Vector3(3, -0.25f, 0);
                            break;
                    }
                    break;
                case 4:
                    // KOTH:
                    switch (flag) {
                        case 1: // green king killed
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusKingOfTheHillTexts[4];
                            newPopUp.gameObject.transform.position += new Vector3(-2, -0.25f, 0);
                            break;
                        case 2: // yellow king killed
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusKingOfTheHillTexts[5];
                            newPopUp.gameObject.transform.position += new Vector3(2, -0.25f, 0);
                            break;
                        case 3: // new green king
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusKingOfTheHillTexts[0];
                            newPopUp.gameObject.transform.position += new Vector3(-2, -0.25f, 0);
                            break;
                        case 4: // new yellow king
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusKingOfTheHillTexts[1];
                            newPopUp.gameObject.transform.position += new Vector3(2, -0.25f, 0);
                            break;
                        case 5: // green zone
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusKingOfTheHillTexts[2];
                            newPopUp.gameObject.transform.position += new Vector3(-2, -0.25f, 0);
                            break;
                        case 6: // yellow zone
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusKingOfTheHillTexts[3];
                            newPopUp.gameObject.transform.position += new Vector3(2, -0.25f, 0);
                            break;
                    }
                    break;
                case 5:
                    // HP:
                    newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusHotPotatoTexts[0];
                    newPopUp.gameObject.transform.position += new Vector3(0, -0.25f, 0);
                    break;
                case 6:
                    // ZL:
                    switch (flag) {
                        case 1: // key item delivered
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusZombieLaboratoryTexts[0];
                            newPopUp.gameObject.transform.position += new Vector3(3, -0.25f, 0);
                            break;
                        case 2: // survivor infected
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusZombieLaboratoryTexts[1];
                            newPopUp.gameObject.transform.position += new Vector3(-3, -0.25f, 0);
                            break;
                        case 3: // survivor zombie
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusZombieLaboratoryTexts[2];
                            newPopUp.gameObject.transform.position += new Vector3(0, -0.25f, 0);
                            break;
                    }
                    break;
                case 7:
                    // BR:
                    switch (BattleRoyale.matchType) {
                        case 0: // someone died
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusBattleRoyaleTexts[0];
                            break;
                        case 1:
                            switch (flag) {
                                case 1: // someone from lime team died
                                    newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusBattleRoyaleTexts[1];
                                    newPopUp.gameObject.transform.position += new Vector3(-3, -0.25f, 0);
                                    break;
                                case 2: // someone from pink team died
                                    newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusBattleRoyaleTexts[2];
                                    newPopUp.gameObject.transform.position += new Vector3(3, -0.25f, 0);
                                    break;
                                case 3: // serial killer died
                                    newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusBattleRoyaleTexts[3];
                                    newPopUp.gameObject.transform.position += new Vector3(0, -0.25f, 0);
                                    break;
                            }
                            break;
                        case 2:
                            switch (flag) {
                                case 1: // points for lime team
                                    newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusBattleRoyaleTexts[4];
                                    newPopUp.gameObject.transform.position += new Vector3(-3, -0.25f, 0);
                                    break;
                                case 2: // points for pink team
                                    newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusBattleRoyaleTexts[5];
                                    newPopUp.gameObject.transform.position += new Vector3(3, -0.25f, 0);
                                    break;
                                case 3: // points for serial killer
                                    newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusBattleRoyaleTexts[6];
                                    newPopUp.gameObject.transform.position += new Vector3(0, -0.25f, 0);
                                    break;
                            }
                            break;
                    }
                    break;
                case 8:
                    // MF:
                    switch (flag) {
                        case 1: // steal from green
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusMonjaFestivalTexts[0];
                            newPopUp.gameObject.transform.position += new Vector3(0, -0.25f, 0);
                            break;
                        case 2: // steal from cyan
                            newPopUp.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = Language.statusMonjaFestivalTexts[0];
                            newPopUp.gameObject.transform.position += new Vector3(0, -0.25f, 0);
                            break;
                    }
                    break;
            }
            newPopUp.Show(player, 0);
        }

        public static IEnumerator PerformTimedAction(float duration, Action<float> action) {
            for (var t = 0f; t < duration; t += Time.deltaTime) {
                action(t / duration);
                yield return EndFrame();
            }

            action(1f);
            yield break;
        }
        public static IEnumerator EndFrame() {
            yield return new WaitForEndOfFrame();
        }
        public static void OverrideOnClickListeners(this PassiveButton passive, Action action, bool enabled = true) {
            passive.OnClick?.RemoveAllListeners();
            passive.OnClick = new();
            passive.OnClick.AddListener(action);
            passive.enabled = enabled;
        }

        public static void OverrideOnMouseOverListeners(this PassiveButton passive, Action action, bool enabled = true) {
            passive.OnMouseOver?.RemoveAllListeners();
            passive.OnMouseOver = new();
            passive.OnMouseOver.AddListener(action);
            passive.enabled = enabled;
        }

        public static void OverrideOnMouseOutListeners(this PassiveButton passive, Action action, bool enabled = true) {
            passive.OnMouseOut?.RemoveAllListeners();
            passive.OnMouseOut = new();
            passive.OnMouseOut.AddListener(action);
            passive.enabled = enabled;
        }
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source)
                action(item);
        }
        public static void AddModSettingsChangeMessage(this NotificationPopper popper, StringNames key, string value, string option, bool playSound = true) {
            string str = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.LobbyChangeSettingNotification, "<font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + option + "</font>", "<font=\"Barlow-Black SDF\" material=\"Barlow-Black Outline\">" + value + "</font>");
            popper.SettingsChangeMessageLogic(key, str, playSound);
        }

        public static void ResetRoleSummaryUI() {
            if (LobbyRoleInfo.RolesSummaryUI != null) {
                UnityEngine.Object.Destroy(LobbyRoleInfo.RolesSummaryUI);
                LobbyRoleInfo.RolesSummaryUI = null;
            }
        }

        public static void UpdateSenseiMap() {
            GameObject mymap = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/Background");
            mymap.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.customMinimap.GetComponent<SpriteRenderer>().sprite;
            GameObject hereindicator = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/HereIndicatorParent");
            hereindicator.transform.position = hereindicator.transform.position + new Vector3(0.23f, -0.8f, 0);

            // Map room names
            GameObject minimapNames = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/RoomNames (1)");
            minimapNames.transform.GetChild(0).transform.position = minimapNames.transform.GetChild(0).transform.position + new Vector3(0f, -0.5f, 0); // Upper engine
            minimapNames.transform.GetChild(2).transform.position = minimapNames.transform.GetChild(2).transform.position + new Vector3(0.7f, -0.55f, 0); // Reactor
            minimapNames.transform.GetChild(3).transform.position = minimapNames.transform.GetChild(3).transform.position + new Vector3(1.75f, 2.37f, 0); // security
            minimapNames.transform.GetChild(4).transform.position = minimapNames.transform.GetChild(4).transform.position + new Vector3(0.89f, -1.18f, 0); // medbey
            minimapNames.transform.GetChild(5).transform.position = minimapNames.transform.GetChild(5).transform.position + new Vector3(0.52f, -1.32f, 0); // Cafetería
            minimapNames.transform.GetChild(6).transform.position = minimapNames.transform.GetChild(6).transform.position + new Vector3(1f, -1.59f, 0); // weapons
            minimapNames.transform.GetChild(7).transform.position = minimapNames.transform.GetChild(7).transform.position + new Vector3(-1.72f, -3.03f, 0); // nav
            minimapNames.transform.GetChild(8).transform.position = minimapNames.transform.GetChild(8).transform.position + new Vector3(-0.08f, 1.45f, 0); // shields
            minimapNames.transform.GetChild(9).transform.position = minimapNames.transform.GetChild(9).transform.position + new Vector3(1.1f, 2.88f, 0); // cooms
            minimapNames.transform.GetChild(10).transform.position = minimapNames.transform.GetChild(10).transform.position + new Vector3(-2.2f, -0.82f, 0); // storage
            minimapNames.transform.GetChild(11).transform.position = minimapNames.transform.GetChild(11).transform.position + new Vector3(0.32f, -1.02f, 0); // Admin
            minimapNames.transform.GetChild(12).transform.position = minimapNames.transform.GetChild(12).transform.position + new Vector3(0.53f, -2.1f, 0); // electrical
            minimapNames.transform.GetChild(13).transform.position = minimapNames.transform.GetChild(13).transform.position + new Vector3(-3.5f, -0.5f, 0); // o2

            // Map sabotage
            GameObject minimapSabotage = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/InfectedOverlay");
            minimapSabotage.transform.GetChild(0).gameObject.SetActive(false); // cafeteria doors
            minimapSabotage.transform.GetChild(2).gameObject.SetActive(false); // medbey doors
            minimapSabotage.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(false); // electrical doors
            minimapSabotage.transform.GetChild(5).gameObject.SetActive(false); // upper engine doors
            minimapSabotage.transform.GetChild(6).gameObject.SetActive(false); // lower engine doors
            minimapSabotage.transform.GetChild(7).gameObject.SetActive(false); // storage doors
            minimapSabotage.transform.GetChild(9).gameObject.SetActive(false); // security doors

            minimapSabotage.transform.GetChild(1).transform.position = minimapSabotage.transform.GetChild(1).transform.position + new Vector3(0.95f, 3.3f, 0); // Sabotage cooms
            minimapSabotage.transform.GetChild(3).transform.GetChild(1).transform.position = minimapSabotage.transform.GetChild(3).transform.GetChild(1).transform.position + new Vector3(0.165f, -1.2f, 0); // Sabotage electrical
            minimapSabotage.transform.GetChild(4).transform.position = minimapSabotage.transform.GetChild(4).transform.position + new Vector3(-3f, 0.05f, 0); // Sabotage o2
            minimapSabotage.transform.GetChild(8).transform.position = minimapSabotage.transform.GetChild(8).transform.position + new Vector3(0.6f, 0.1f, 0); // Sabotage reactor

            updatedSenseiMinimap = true;
        }

        public static bool isSubmergedMap() {
            return GameOptionsManager.Instance.currentGameOptions.MapId == 6;
        }

        public static void AssingRoleToTeam(ref PlayerControl slot, PlayerControl player, List<PlayerControl> team) {
            slot = player;
            team.Add(player);
        }

        public static void UpdateDraggedBody(PlayerControl player, bool draggingBody, byte bodyId) {
            if (player == null || !draggingBody)
                return;

            DeadBody[] bodies = UnityEngine.Object.FindObjectsOfType<DeadBody>();

            foreach (DeadBody body in bodies) {
                if (GameData.Instance.GetPlayerById(body.ParentId).PlayerId != bodyId)
                    continue;

                var currentPosition = player.GetTruePosition();
                var velocity = player.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;

                var newPos = (Vector2)player.GetTruePosition() - (velocity / 3) + new Vector2(0.15f, 0.25f) + body.myCollider.offset;

                if (PhysicsHelpers.AnythingBetween(
                        currentPosition,
                        newPos,
                        Constants.ShipAndObjectsMask,
                        false))
                    continue;

                body.transform.position = newPos;

                if (Helpers.isSubmergedMap()) {
                    body.transform.position += new Vector3(0, 0, -0.5f);
                }

                break;
            }
        }

        public static void RestoreBodyTypeWithDelay(PlayerControl player) {
            HudManager.Instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) => { // Delayed action
                if (p == 1f && player != null) {
                    player.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                }
            })));
        }
        
        public static void GamemodesDisableDeadBodyReportOnClic(DeadBody gamemodeBody) {
            gamemodeBody.GetComponent<PassiveButton>().enabled = false;
        }

        public static void GamemodesGenericBecomeAliveAndTargetable(PlayerControl player, HashSet<PlayerControl> revivingList, float reviveTime, bool isNeutral = false, bool isBattleRoyale = false) {
            HudManager.Instance.StartCoroutine(Effects.Lerp(reviveTime, new Action<float>((p) => {
                if (p == 1f && player != null) {
                    revivingList.Remove(player);
                    Helpers.alphaPlayer(player.PlayerId, 1f);
                    if (isNeutral) {
                        player.MyPhysics.SetBodyType(PlayerBodyTypes.Seeker);
                    }
                    if (isBattleRoyale) {
                        if (isNeutral) {
                            if (PlayerInCache.AllPlayers.Count >= 11) {
                                BattleRoyale.serialKillerLifes = BattleRoyale.fighterLifes * 3;
                            }
                            else {
                                BattleRoyale.serialKillerLifes = BattleRoyale.fighterLifes * 2;
                            }
                        }
                        else {
                            Helpers.BattleRoyaleRestoreTeamLifes(player);
                        }
                    }
                }
            })));
        }

        public static void GamemodesGenericRevive(PlayerControl player, DeadBody playerBody, float reviveTime, Vector3 playerPos, Vector3 submergedUpperPos, Vector3 submergedLowerPos) {
            HudManager.Instance.StartCoroutine(Effects.Lerp(reviveTime, new Action<float>((p) => {
                if (p == 1f && player != null) {
                    player.Revive();
                    if (Helpers.isSubmergedMap()) {
                        if (player.transform.position.y > 0) {
                            player.transform.position = submergedUpperPos;
                        }
                        else {
                            player.transform.position = submergedLowerPos;
                        }
                    }
                    else {
                        player.transform.position = playerPos;
                    }
                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == player.PlayerId).FirstOrDefault();
                    if (playerBody != null) UnityEngine.Object.Destroy(playerBody.gameObject);
                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                }

            })));
        }

        public static void CaptureTheFlagSwapStealerRole(ref PlayerControl player, PlayerControl murdered, List<PlayerControl> team) {
            int index = team.IndexOf(player);

            if (index != -1) {
                team[index] = CaptureTheFlag.stealerPlayer;
            }
            player = CaptureTheFlag.stealerPlayer;
            CaptureTheFlag.stealerPlayer = murdered;
            player.MyPhysics.SetBodyType(PlayerBodyTypes.Normal);
            player.MurderPlayer(CaptureTheFlag.stealerPlayer, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
        }

        public static byte PoliceAndThiefsGetJewelId(PlayerControl player) {
            if (player == PoliceAndThief.thiefplayer01) return PoliceAndThief.thiefplayer01JewelId;
            if (player == PoliceAndThief.thiefplayer02) return PoliceAndThief.thiefplayer02JewelId;
            if (player == PoliceAndThief.thiefplayer03) return PoliceAndThief.thiefplayer03JewelId;
            if (player == PoliceAndThief.thiefplayer04) return PoliceAndThief.thiefplayer04JewelId;
            if (player == PoliceAndThief.thiefplayer05) return PoliceAndThief.thiefplayer05JewelId;
            if (player == PoliceAndThief.thiefplayer06) return PoliceAndThief.thiefplayer06JewelId;
            if (player == PoliceAndThief.thiefplayer07) return PoliceAndThief.thiefplayer07JewelId;
            if (player == PoliceAndThief.thiefplayer08) return PoliceAndThief.thiefplayer08JewelId;
            if (player == PoliceAndThief.thiefplayer09) return PoliceAndThief.thiefplayer09JewelId;

            return 0;
        }
        public static void PoliceAndThiefsSetJewelId(PlayerControl player, byte jewelId) {
            if (player == PoliceAndThief.thiefplayer01) PoliceAndThief.thiefplayer01JewelId = jewelId;
            else if (player == PoliceAndThief.thiefplayer02) PoliceAndThief.thiefplayer02JewelId = jewelId;
            else if (player == PoliceAndThief.thiefplayer03) PoliceAndThief.thiefplayer03JewelId = jewelId;
            else if (player == PoliceAndThief.thiefplayer04) PoliceAndThief.thiefplayer04JewelId = jewelId;
            else if (player == PoliceAndThief.thiefplayer05) PoliceAndThief.thiefplayer05JewelId = jewelId;
            else if (player == PoliceAndThief.thiefplayer06) PoliceAndThief.thiefplayer06JewelId = jewelId;
            else if (player == PoliceAndThief.thiefplayer07) PoliceAndThief.thiefplayer07JewelId = jewelId;
            else if (player == PoliceAndThief.thiefplayer08) PoliceAndThief.thiefplayer08JewelId = jewelId;
            else if (player == PoliceAndThief.thiefplayer09) PoliceAndThief.thiefplayer09JewelId = jewelId;
        }
        public static void KOTHRemoveMinionPlayer(PlayerControl player, bool greenTeam) {
            if (greenTeam) {
                if (KingOfTheHill.greenplayer01 == player) KingOfTheHill.greenplayer01 = null;
                else if (KingOfTheHill.greenplayer02 == player) KingOfTheHill.greenplayer02 = null;
                else if (KingOfTheHill.greenplayer03 == player) KingOfTheHill.greenplayer03 = null;
                else if (KingOfTheHill.greenplayer04 == player) KingOfTheHill.greenplayer04 = null;
                else if (KingOfTheHill.greenplayer05 == player) KingOfTheHill.greenplayer05 = null;
                else if (KingOfTheHill.greenplayer06 == player) KingOfTheHill.greenplayer06 = null;
            }
            else {
                if (KingOfTheHill.yellowplayer01 == player) KingOfTheHill.yellowplayer01 = null;
                else if (KingOfTheHill.yellowplayer02 == player) KingOfTheHill.yellowplayer02 = null;
                else if (KingOfTheHill.yellowplayer03 == player) KingOfTheHill.yellowplayer03 = null;
                else if (KingOfTheHill.yellowplayer04 == player) KingOfTheHill.yellowplayer04 = null;
                else if (KingOfTheHill.yellowplayer05 == player) KingOfTheHill.yellowplayer05 = null;
                else if (KingOfTheHill.yellowplayer06 == player) KingOfTheHill.yellowplayer06 = null;
            }
        }
        public static void RemoveNotPotato(PlayerControl potato) {
            if (HotPotato.notPotato01 == potato) HotPotato.notPotato01 = null;
            else if (HotPotato.notPotato02 == potato) HotPotato.notPotato02 = null;
            else if (HotPotato.notPotato03 == potato) HotPotato.notPotato03 = null;
            else if (HotPotato.notPotato04 == potato) HotPotato.notPotato04 = null;
            else if (HotPotato.notPotato05 == potato) HotPotato.notPotato05 = null;
            else if (HotPotato.notPotato06 == potato) HotPotato.notPotato06 = null;
            else if (HotPotato.notPotato07 == potato) HotPotato.notPotato07 = null;
            else if (HotPotato.notPotato08 == potato) HotPotato.notPotato08 = null;
            else if (HotPotato.notPotato09 == potato) HotPotato.notPotato09 = null;
            else if (HotPotato.notPotato10 == potato) HotPotato.notPotato10 = null;
            else if (HotPotato.notPotato11 == potato) HotPotato.notPotato11 = null;
            else if (HotPotato.notPotato12 == potato) HotPotato.notPotato12 = null;
            else if (HotPotato.notPotato13 == potato) HotPotato.notPotato13 = null;
            else if (HotPotato.notPotato14 == potato) HotPotato.notPotato14 = null;
        }

        public static void AddExplodedPotato(PlayerControl potato) {
            if (HotPotato.explodedPotato01 == null) HotPotato.explodedPotato01 = potato;
            else if (HotPotato.explodedPotato02 == null) HotPotato.explodedPotato02 = potato;
            else if (HotPotato.explodedPotato03 == null) HotPotato.explodedPotato03 = potato;
            else if (HotPotato.explodedPotato04 == null) HotPotato.explodedPotato04 = potato;
            else if (HotPotato.explodedPotato05 == null) HotPotato.explodedPotato05 = potato;
            else if (HotPotato.explodedPotato06 == null) HotPotato.explodedPotato06 = potato;
            else if (HotPotato.explodedPotato07 == null) HotPotato.explodedPotato07 = potato;
            else if (HotPotato.explodedPotato08 == null) HotPotato.explodedPotato08 = potato;
            else if (HotPotato.explodedPotato09 == null) HotPotato.explodedPotato09 = potato;
            else if (HotPotato.explodedPotato10 == null) HotPotato.explodedPotato10 = potato;
            else if (HotPotato.explodedPotato11 == null) HotPotato.explodedPotato11 = potato;
            else if (HotPotato.explodedPotato12 == null) HotPotato.explodedPotato12 = potato;
            else if (HotPotato.explodedPotato13 == null) HotPotato.explodedPotato13 = potato;
            else if (HotPotato.explodedPotato14 == null) HotPotato.explodedPotato14 = potato;

            HotPotato.explodedPotatoTeam.Add(potato);
        }

        public static void HotPotatoReplaceNotPotato(PlayerControl currentPotato, PlayerControl replacementPotato) {
            if (HotPotato.notPotato01 == currentPotato) HotPotato.notPotato01 = replacementPotato;
            else if (HotPotato.notPotato02 == currentPotato) HotPotato.notPotato02 = replacementPotato;
            else if (HotPotato.notPotato03 == currentPotato) HotPotato.notPotato03 = replacementPotato;
            else if (HotPotato.notPotato04 == currentPotato) HotPotato.notPotato04 = replacementPotato;
            else if (HotPotato.notPotato05 == currentPotato) HotPotato.notPotato05 = replacementPotato;
            else if (HotPotato.notPotato06 == currentPotato) HotPotato.notPotato06 = replacementPotato;
            else if (HotPotato.notPotato07 == currentPotato) HotPotato.notPotato07 = replacementPotato;
            else if (HotPotato.notPotato08 == currentPotato) HotPotato.notPotato08 = replacementPotato;
            else if (HotPotato.notPotato09 == currentPotato) HotPotato.notPotato09 = replacementPotato;
            else if (HotPotato.notPotato10 == currentPotato) HotPotato.notPotato10 = replacementPotato;
            else if (HotPotato.notPotato11 == currentPotato) HotPotato.notPotato11 = replacementPotato;
            else if (HotPotato.notPotato12 == currentPotato) HotPotato.notPotato12 = replacementPotato;
            else if (HotPotato.notPotato13 == currentPotato) HotPotato.notPotato13 = replacementPotato;
            else if (HotPotato.notPotato14 == currentPotato) HotPotato.notPotato14 = replacementPotato;
        }

        public static byte ZombieLaboratoryGetFoundBox(PlayerControl player) {
            if (player == ZombieLaboratory.survivorPlayer01) return ZombieLaboratory.survivorPlayer01FoundBox;
            if (player == ZombieLaboratory.survivorPlayer02) return ZombieLaboratory.survivorPlayer02FoundBox;
            if (player == ZombieLaboratory.survivorPlayer03) return ZombieLaboratory.survivorPlayer03FoundBox;
            if (player == ZombieLaboratory.survivorPlayer04) return ZombieLaboratory.survivorPlayer04FoundBox;
            if (player == ZombieLaboratory.survivorPlayer05) return ZombieLaboratory.survivorPlayer05FoundBox;
            if (player == ZombieLaboratory.survivorPlayer06) return ZombieLaboratory.survivorPlayer06FoundBox;
            if (player == ZombieLaboratory.survivorPlayer07) return ZombieLaboratory.survivorPlayer07FoundBox;
            if (player == ZombieLaboratory.survivorPlayer08) return ZombieLaboratory.survivorPlayer08FoundBox;
            if (player == ZombieLaboratory.survivorPlayer09) return ZombieLaboratory.survivorPlayer09FoundBox;
            if (player == ZombieLaboratory.survivorPlayer10) return ZombieLaboratory.survivorPlayer10FoundBox;
            if (player == ZombieLaboratory.survivorPlayer11) return ZombieLaboratory.survivorPlayer11FoundBox;
            if (player == ZombieLaboratory.survivorPlayer12) return ZombieLaboratory.survivorPlayer12FoundBox;
            if (player == ZombieLaboratory.survivorPlayer13) return ZombieLaboratory.survivorPlayer13FoundBox;

            return 0;
        }
        public static void ZombieLaboratorySetFoundBox(PlayerControl player, byte foundBox) {
            if (player == ZombieLaboratory.survivorPlayer01) ZombieLaboratory.survivorPlayer01FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer02) ZombieLaboratory.survivorPlayer02FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer03) ZombieLaboratory.survivorPlayer03FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer04) ZombieLaboratory.survivorPlayer04FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer05) ZombieLaboratory.survivorPlayer05FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer06) ZombieLaboratory.survivorPlayer06FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer07) ZombieLaboratory.survivorPlayer07FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer08) ZombieLaboratory.survivorPlayer08FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer09) ZombieLaboratory.survivorPlayer09FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer10) ZombieLaboratory.survivorPlayer10FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer11) ZombieLaboratory.survivorPlayer11FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer12) ZombieLaboratory.survivorPlayer12FoundBox = foundBox;
            else if (player == ZombieLaboratory.survivorPlayer13) ZombieLaboratory.survivorPlayer13FoundBox = foundBox;
        }

        public static void BattleRoyaleHandleTeamLifes(ref float lifes, PlayerControl murdered, byte sourceId, byte targetId, int footprintColor, int scoredTeam) {
            lifes -= 1;
            new BattleRoyaleFootprint(murdered, footprintColor);
            if (lifes > 0) return;
            RPCProcedure.uncheckedMurderPlayer(sourceId, targetId, 0);
            switch (BattleRoyale.matchType) {
                case 1: 
                    // lime 1 - pink 2, same value used for creating footprint
                    RPCProcedure.battleRoyaleCheckWin(footprintColor);
                    Helpers.showGamemodesPopUp(footprintColor, Helpers.playerById(targetId));
                    break;
                case 2:
                    if (BattleRoyale.serialKiller != null && sourceId == BattleRoyale.serialKiller.PlayerId) {
                        RPCProcedure.battleRoyaleScoreCheck(3, 1);
                        Helpers.showGamemodesPopUp(3, Helpers.playerById(targetId));
                    }
                    else {
                        // on teamscore check, points for lime = 1, pink = 2
                        RPCProcedure.battleRoyaleScoreCheck(scoredTeam, 1);
                        Helpers.showGamemodesPopUp(scoredTeam, Helpers.playerById(targetId));
                    }
                    break;
            }
        }

        public static void BattleRoyaleRestoreTeamLifes(PlayerControl battler) {
            // Lime Team
            if (BattleRoyale.limePlayer01?.PlayerId == battler.PlayerId) BattleRoyale.limePlayer01Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.limePlayer02?.PlayerId == battler.PlayerId) BattleRoyale.limePlayer02Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.limePlayer03?.PlayerId == battler.PlayerId) BattleRoyale.limePlayer03Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.limePlayer04?.PlayerId == battler.PlayerId) BattleRoyale.limePlayer04Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.limePlayer05?.PlayerId == battler.PlayerId) BattleRoyale.limePlayer05Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.limePlayer06?.PlayerId == battler.PlayerId) BattleRoyale.limePlayer06Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.limePlayer07?.PlayerId == battler.PlayerId) BattleRoyale.limePlayer07Lifes = BattleRoyale.fighterLifes;

            // Pink Team
            else if (BattleRoyale.pinkPlayer01?.PlayerId == battler.PlayerId) BattleRoyale.pinkPlayer01Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.pinkPlayer02?.PlayerId == battler.PlayerId) BattleRoyale.pinkPlayer02Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.pinkPlayer03?.PlayerId == battler.PlayerId) BattleRoyale.pinkPlayer03Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.pinkPlayer04?.PlayerId == battler.PlayerId) BattleRoyale.pinkPlayer04Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.pinkPlayer05?.PlayerId == battler.PlayerId) BattleRoyale.pinkPlayer05Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.pinkPlayer06?.PlayerId == battler.PlayerId) BattleRoyale.pinkPlayer06Lifes = BattleRoyale.fighterLifes;
            else if (BattleRoyale.pinkPlayer07?.PlayerId == battler.PlayerId) BattleRoyale.pinkPlayer07Lifes = BattleRoyale.fighterLifes;
        }

        public static void MonjaFestivalResetPlayer(PlayerControl player, ref int monjitas, TMPro.TMP_Text deliverCount, GameObject handsObject, float angleStep, float offset) {
            MonjaFestivalDropMonjitas(player, monjitas, angleStep, offset);
            if (MonjaFestival.bigMonjaPlayer != null && MonjaFestival.bigMonjaPlayer == player) return;
            monjitas = 0;
            deliverCount.text = $"{monjitas} / 3";
            handsObject.GetComponent<SpriteRenderer>().sprite = null;
        }

        public static void MonjaFestivalDropMonjitas(PlayerControl player, int monjitasCount, float angleStep, float offset) {
            if (player == null || monjitasCount <= 0) return;

            float currentAngleStep = angleStep / monjitasCount;
            for (int i = 0; i < monjitasCount; i++) {
                GameObject littleMonja = GameObject.Instantiate(CustomMain.customAssets.floorGreenMonja, PlayerInCache.LocalPlayer.PlayerControl.transform.parent);
                littleMonja.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
                littleMonja.transform.position = new Vector3(player.transform.position.x + offset, player.transform.position.y + offset, 0.5f);
                littleMonja.transform.RotateAround(player.transform.position, Vector3.forward, currentAngleStep * i);
                littleMonja.name = "littleMonja" + MonjaFestival.littleMonjasDroppedCount.ToString();
                MonjaFestival.littleMonjasDroppedCount += 1;
                MonjaFestival.bigMonjaSpawns.Add(littleMonja);
            }
        }
        public static IEnumerator MonjaSpawnReload(Func<int> getCurrentPoints, Action<int> setCurrentPoints, int maximumPoints, float reloadSeconds, Action<bool> setReloadingState, GameObject spawnObject, TMPro.TMP_Text counterText, Sprite fullSpawnSprite) {
            setReloadingState(true);

            while (getCurrentPoints() < maximumPoints) {
                yield return new WaitForSeconds(reloadSeconds);

                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) {
                    setReloadingState(false);
                    yield break;
                }

                int newPoints = getCurrentPoints() + 1;

                setCurrentPoints(newPoints);

                spawnObject.GetComponent<SpriteRenderer>().sprite = fullSpawnSprite;

                counterText.text = $"{newPoints} / {maximumPoints}";

                if (newPoints >= maximumPoints) {
                    setReloadingState(false);
                }
            }
        }
    }
}