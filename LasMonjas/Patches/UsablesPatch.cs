using HarmonyLib;
using System;
using Hazel;
using UnityEngine;
using System.Linq;
using static LasMonjas.LasMonjas;
using static LasMonjas.GameHistory;
using System.Collections.Generic;
using PowerTools;
using LasMonjas.Core;
using AmongUs.GameOptions;

namespace LasMonjas.Patches
{

    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        public static bool Prefix(bool __runOriginal, Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse) {
            if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek) {
                __runOriginal = true;
                return __runOriginal;
            }

            if (!__runOriginal) {
                return false;
            }

            float num = float.MaxValue;
            PlayerControl @object = pc.Object;

            bool roleCouldUse = @object.roleCanUseVents();

            var usableDistance = __instance.UsableDistance;
            if (__instance.name.StartsWith("Hat_")) {
                if (Illusionist.illusionist != PlayerInCache.LocalPlayer.PlayerControl) {
                    // Only the Illusionist can use the Hats
                    canUse = false;
                    couldUse = false;
                    __result = num;
                    return false;
                }
                else {
                    // Reduce the usable distance to reduce the risk of gettings stuck while trying to jump into the hat if it's placed near objects
                    usableDistance = 0.4f;
                }
            }
            else if (__instance.name.StartsWith("SealedVent_")) {
                canUse = couldUse = false;
                __result = num;
                return false;
            }

            // Submerged check
            if (GameOptionsManager.Instance.currentGameOptions.MapId == 6) {
                // as submerged does, only change stuff for vents 9 and 14 of submerged. Code partially provided by AlexejheroYTB
                if (SubmergedCompatibility.getInTransition()) {
                    __result = float.MaxValue;
                    return canUse = couldUse = false;
                }
                switch (__instance.Id) {
                    case 9:  // Cannot enter vent 9 (Engine Room Exit Only Vent)!
                        if (PlayerInCache.LocalPlayer.PlayerControl.inVent) break;
                        __result = float.MaxValue;
                        return canUse = couldUse = false;
                    case 14: // Lower Central
                        __result = float.MaxValue;
                        couldUse = roleCouldUse && !pc.IsDead && (@object.CanMove || @object.inVent);
                        canUse = couldUse;
                        if (canUse) {
                            Vector3 center = @object.Collider.bounds.center;
                            Vector3 position = __instance.transform.position;
                            __result = Vector2.Distance(center, position);
                            canUse &= __result <= __instance.UsableDistance;
                        }
                        return false;
                }
            }

            couldUse = (@object.inVent || roleCouldUse) && !pc.IsDead && (@object.CanMove || @object.inVent);
            canUse = couldUse;
            if (canUse) {
                Vector3 center = @object.Collider.bounds.center;
                Vector3 position = __instance.transform.position;
                num = Vector2.Distance(center, position);

                canUse &= (num <= usableDistance && !PhysicsHelpers.AnythingBetween(@object.Collider, center, position, Constants.ShipOnlyMask, false));
            }
            __result = num;
            return false;
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.DoClick))]
    class VentButtonDoClickPatch
    {
        static bool Prefix(VentButton __instance) {
            // Manually modifying the VentButton to use Vent.Use again in order to trigger the Vent.Use prefix patch
            if (__instance.currentTarget != null) __instance.currentTarget.Use();
            return false;
        }
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
    public static class VentUsePatch
    {
        public static bool Prefix(Vent __instance) {
            bool canUse;
            bool couldUse;
            __instance.CanUse(PlayerInCache.LocalPlayer.Data, out canUse, out couldUse);
            bool canMoveInVents = true;
            if (!canUse) return false;

            bool isEnter = !PlayerInCache.LocalPlayer.PlayerControl.inVent;

            if (__instance.name.StartsWith("Hat_")) {
                __instance.SetButtons(isEnter && canMoveInVents);
                MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UseUncheckedVent, Hazel.SendOption.Reliable);
                writer.WritePacked(__instance.Id);
                writer.Write(PlayerInCache.LocalPlayer.PlayerControl.PlayerId);
                writer.Write(isEnter ? byte.MaxValue : (byte)0);
                writer.EndMessage();
                RPCProcedure.useUncheckedVent(__instance.Id, PlayerInCache.LocalPlayer.PlayerControl.PlayerId, isEnter ? byte.MaxValue : (byte)0);
                return false;
            }

            if (isEnter) {
                PlayerInCache.LocalPlayer.PlayerControl.MyPhysics.RpcEnterVent(__instance.Id);
            }
            else {
                PlayerInCache.LocalPlayer.PlayerControl.MyPhysics.RpcExitVent(__instance.Id);
            }
            __instance.SetButtons(isEnter && canMoveInVents);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    class VentButtonVisibilityPatch
    {
        static void Postfix(PlayerControl __instance) {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                if (__instance.AmOwner && __instance.roleCanUseVents() && gameType >= 2) {
                    HudManager.Instance.ImpostorVentButton.Show();
                }
                else if (__instance.AmOwner && __instance.roleCanUseVents() && HudManager.Instance.ReportButton.isActiveAndEnabled || __instance.AmOwner && __instance.roleCanUseVents() && HudManager.Instance.ReportButton.isActiveAndEnabled && gameType <= 1) {
                    HudManager.Instance.ImpostorVentButton.Show();
                }
            }
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.SetTarget))]
    class VentButtonSetTargetPatch
    {
        static Sprite defaultVentSprite = null;
        static void Postfix(VentButton __instance) {
            // Illusionist render special vent button
            if (Illusionist.illusionist != null && Illusionist.illusionist == PlayerInCache.LocalPlayer.PlayerControl) {
                if (defaultVentSprite == null) defaultVentSprite = __instance.graphic.sprite;
                bool isSpecialVent = __instance.currentTarget != null && __instance.currentTarget.gameObject != null && __instance.currentTarget.gameObject.name.StartsWith("Hat_");
                __instance.graphic.sprite = isSpecialVent ? Illusionist.getIllusionistVentButtonSprite() : defaultVentSprite;
                __instance.buttonLabelText.enabled = !isSpecialVent;
            }
        }
    }

    internal class VisibleVentPatches
    {
        public static int ShipAndObjectsMask = LayerMask.GetMask(new string[]
        {
            "Ship",
            "Objects"
        });

        [HarmonyPatch(typeof(Vent), nameof(Vent.EnterVent))] //EnterVent
        public static class EnterVentPatch
        {
            public static bool Prefix(Vent __instance, PlayerControl pc) {

                if (!__instance.EnterVentAnim) {
                    return false;
                }

                var truePosition = PlayerInCache.LocalPlayer.PlayerControl.GetTruePosition();

                Vector2 vector = pc.GetTruePosition() - truePosition;
                var magnitude = vector.magnitude;
                if (pc != null && !hideVentAnim || hideVentAnim && magnitude < PlayerInCache.LocalPlayer.PlayerControl.lightSource.viewDistance &&
                !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude,
                    ShipAndObjectsMask)) {
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5) {
                        __instance.GetComponent<SpriteAnim>().Play(__instance.EnterVentAnim, 1f);
                    }
                    else {
                        __instance.transform.GetChild(3).GetComponent<SpriteAnim>().Play(__instance.EnterVentAnim, 1f);
                    }
                }

                if (pc.AmOwner && Constants.ShouldPlaySfx()) //ShouldPlaySfx
                {
                    SoundManager.Instance.StopSound(ShipStatus.Instance.VentEnterSound);
                    SoundManager.Instance.PlaySound(ShipStatus.Instance.VentEnterSound, false, 1f).pitch =
                        UnityEngine.Random.Range(0.8f, 1.2f);
                }

                if (Welder.bombedVent != null && __instance.Id == Welder.bombedVent.Id && !Welder.welder.Data.IsDead) {
                    if (Welder.bombedVent == __instance) {
                        RPCProcedure.uncheckedMurderPlayer(Welder.welder.PlayerId, pc.PlayerId, 0);
                    }
                    return false;
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(Vent), nameof(Vent.ExitVent))] //ExitVent
        public static class ExitVentPatch
        {
            public static bool Prefix(Vent __instance, PlayerControl pc) {

                if (!__instance.ExitVentAnim) {
                    return false;
                }

                var truePosition = PlayerInCache.LocalPlayer.PlayerControl.GetTruePosition();

                Vector2 vector = pc.GetTruePosition() - truePosition;
                var magnitude = vector.magnitude;
                if (pc != null && !hideVentAnim || hideVentAnim && magnitude < PlayerInCache.LocalPlayer.PlayerControl.lightSource.viewDistance &&
                !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude,
                    ShipAndObjectsMask)) {
                    if (GameOptionsManager.Instance.currentGameOptions.MapId != 5) {
                        __instance.GetComponent<SpriteAnim>().Play(__instance.ExitVentAnim, 1f);
                    }
                    else {
                        __instance.transform.GetChild(3).GetComponent<SpriteAnim>().Play(__instance.ExitVentAnim, 1f);
                    }
                }

                if (pc.AmOwner && Constants.ShouldPlaySfx()) //ShouldPlaySfx
                {
                    SoundManager.Instance.StopSound(ShipStatus.Instance.VentEnterSound);
                    SoundManager.Instance.PlaySound(ShipStatus.Instance.VentEnterSound, false, 1f).pitch =
                        UnityEngine.Random.Range(0.8f, 1.2f);
                }

                if (Welder.bombedVent != null && __instance.Id == Welder.bombedVent.Id && !Welder.welder.Data.IsDead) {
                    if (Welder.bombedVent == __instance) {
                        RPCProcedure.uncheckedMurderPlayer(Welder.welder.PlayerId, pc.PlayerId, 0);
                    }
                    return false;
                }

                return false;
            }
        }
    }

    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    class KillButtonDoClickPatch
    {
        public static bool Prefix(KillButton __instance) {
            if (__instance.isActiveAndEnabled && __instance.currentTarget && !__instance.isCoolingDown && !PlayerInCache.LocalPlayer.Data.IsDead && PlayerInCache.LocalPlayer.PlayerControl.CanMove) {
                // Use an unchecked kill command, to allow shorter kill cooldowns etc. without getting kicked
                MurderAttemptResult res = Helpers.checkMurderAttemptAndKill(PlayerInCache.LocalPlayer.PlayerControl, __instance.currentTarget);
                // Handle Jinx kill
                if (res == MurderAttemptResult.JinxKill) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                    PlayerInCache.LocalPlayer.PlayerControl.killTimer = GameOptionsManager.Instance.currentGameOptions.GetFloat(FloatOptionNames.KillCooldown);
                    if (PlayerInCache.LocalPlayer.PlayerControl == Janitor.janitor)
                        Janitor.janitor.killTimer = HudManagerStartPatch.janitorCleanButton.Timer = HudManagerStartPatch.janitorCleanButton.MaxTimer;
                    else if (PlayerInCache.LocalPlayer.PlayerControl == Manipulator.manipulator)
                        Manipulator.manipulator.killTimer = HudManagerStartPatch.manipulatorManipulateButton.Timer = HudManagerStartPatch.manipulatorManipulateButton.MaxTimer;
                    else if (PlayerInCache.LocalPlayer.PlayerControl == Sorcerer.sorcerer)
                        Sorcerer.sorcerer.killTimer = HudManagerStartPatch.sorcererSpellButton.Timer = HudManagerStartPatch.sorcererSpellButton.MaxTimer;
                }
                __instance.SetTarget(null);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.Close))]
    class MapBehaviourCloseHauntButton
    {
        static void Postfix() {
            if (GameOptionsManager.Instance.currentGameMode == GameModes.Normal && gameType <= 1) {
                foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                    if (player == PlayerInCache.LocalPlayer.PlayerControl) {
                        HudManager.Instance.AbilityButton.Hide();
                        var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == player.PlayerId);
                        DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == player.PlayerId).FirstOrDefault();
                        if (body == null && deadPlayerEntry != null && player.Data.IsDead) {
                            HudManager.Instance.AbilityButton.Show();
                        }
                    }
                }
            }
        }
    }


    [HarmonyPatch(typeof(SabotageButton), nameof(SabotageButton.DoClick))]
    class SabotageButtonDoClickPatch
    {
        static bool Prefix(SabotageButton __instance) {

            // Block sabotage button on custom gamemodes
            if (gameType >= 2 || Monja.awakened) {
                return false;
            }
            else {

                // Block sabotage button if Bomberman bomb, lights out, duel or special condition 1vs1 is active
                bool blockSabotage = (PlayerInCache.LocalPlayer.Data.Role.IsImpostor || (Joker.canSabotage && Joker.joker != null && Joker.joker == PlayerInCache.LocalPlayer.PlayerControl && !PlayerInCache.LocalPlayer.Data.IsDead) || (Poisoner.canSabotage && Poisoner.poisoner != null && Poisoner.poisoner == PlayerInCache.LocalPlayer.PlayerControl && !PlayerInCache.LocalPlayer.Data.IsDead)) && (alivePlayers <= 2 || Bomberman.activeBomb || Challenger.isDueling || Seeker.isMinigaming || Illusionist.lightsOutTimer > 0);
                if (blockSabotage) return false;

                // Joker sabotage
                if (Joker.canSabotage && Joker.joker != null && Joker.joker == PlayerInCache.LocalPlayer.PlayerControl && !PlayerInCache.LocalPlayer.Data.IsDead && !Bomberman.activeBomb && !Challenger.isDueling && !Seeker.isMinigaming && Illusionist.lightsOutTimer <= 0) {
                    MapBehaviour.Instance.ShowSabotageMap();
                    return false;
                }

                // Poisoner sabotage
                if (Poisoner.canSabotage && Poisoner.poisoner != null && Poisoner.poisoner == PlayerInCache.LocalPlayer.PlayerControl && !PlayerInCache.LocalPlayer.Data.IsDead && !Bomberman.activeBomb && !Challenger.isDueling && !Seeker.isMinigaming && Illusionist.lightsOutTimer <= 0) {
                    MapBehaviour.Instance.ShowSabotageMap();
                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(SabotageButton), nameof(SabotageButton.Refresh))]
    class SabotageButtonRefreshPatch
    {
        static void Postfix() {

            // Change sabotage button image and block sabotages for gamemodes
            if (gameType >= 2 || Monja.awakened) {
                HudManager.Instance.SabotageButton.Hide();
            }

            else {

                // Block sabotage button if Bomberman bomb, lights out, duel or special condition 1vs1 is active
                bool blockSabotage = (PlayerInCache.LocalPlayer.Data.Role.IsImpostor || (Joker.canSabotage && Joker.joker != null && Joker.joker == PlayerInCache.LocalPlayer.PlayerControl && !PlayerInCache.LocalPlayer.Data.IsDead) || (Poisoner.canSabotage && Poisoner.poisoner != null && Poisoner.poisoner == PlayerInCache.LocalPlayer.PlayerControl && !PlayerInCache.LocalPlayer.Data.IsDead)) && (alivePlayers <= 2 || Bomberman.activeBomb || Challenger.isDueling || Seeker.isMinigaming || Illusionist.lightsOutTimer > 0);
                if (blockSabotage) {
                    HudManager.Instance.SabotageButton.SetDisabled();
                }

                if (Joker.canSabotage && Joker.joker != null && Joker.joker == PlayerInCache.LocalPlayer.PlayerControl && !PlayerInCache.LocalPlayer.Data.IsDead && !Bomberman.activeBomb && !Challenger.isDueling && !Seeker.isMinigaming && Illusionist.lightsOutTimer <= 0) {
                    if (MapBehaviour.Instance != null && !MapBehaviour.Instance.IsOpen && MeetingHud.Instance == null) {
                        HudManager.Instance.SabotageButton.Show();
                    }
                    else if (MapBehaviour.Instance != null && MapBehaviour.Instance.IsOpen) {
                        HudManager.Instance.SabotageButton.Hide();
                    }
                }

                if (Poisoner.canSabotage && Poisoner.poisoner != null && Poisoner.poisoner == PlayerInCache.LocalPlayer.PlayerControl && !PlayerInCache.LocalPlayer.Data.IsDead && !Bomberman.activeBomb && !Challenger.isDueling && !Seeker.isMinigaming && Illusionist.lightsOutTimer <= 0) {
                    if (MapBehaviour.Instance != null && !MapBehaviour.Instance.IsOpen && MeetingHud.Instance == null) {
                        HudManager.Instance.SabotageButton.Show();
                    }
                    else if (MapBehaviour.Instance != null && MapBehaviour.Instance.IsOpen) {
                        HudManager.Instance.SabotageButton.Hide();
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(ReportButton), nameof(ReportButton.DoClick))]
    class ReportButtonUpdatePatch
    {
        static bool Prefix(ReportButton __instance) {

            // Block report button if dueling or gamemodes)
            bool blockReport = Challenger.isDueling || gameType >= 2 || Monja.awakened || Seeker.isMinigaming;
            if (blockReport) return false;

            return true;
        }
    }

    [HarmonyPatch(typeof(EmergencyMinigame), nameof(EmergencyMinigame.Update))]
    class EmergencyMinigameUpdatePatch
    {
        static void Postfix(EmergencyMinigame __instance) {
            var roleCanCallEmergency = true;
            var statusText = "";

            // Deactivate emergency button for custom gamemodes
            if (gameType >= 2) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[0];
            }

            // Deactivate emergency button for Cheater
            if (Cheater.cheater != null && Cheater.cheater == PlayerInCache.LocalPlayer.PlayerControl && !Cheater.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[1];
            }

            // Deactivate emergency button for Gambler
            if (Gambler.gambler != null && Gambler.gambler == PlayerInCache.LocalPlayer.PlayerControl && !Gambler.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[2];
            }

            // Deactivate emergency button for Sorcerer
            if (Sorcerer.sorcerer != null && Sorcerer.sorcerer == PlayerInCache.LocalPlayer.PlayerControl && !Sorcerer.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[3];
            }

            // Deactivate emergency button if there's a bomb
            if (Bomberman.bomberman != null && Bomberman.activeBomb == true) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[4];
            }

            // Deactivate emergency button if there's lights out
            if (Illusionist.illusionist != null && Illusionist.lightsOutTimer > 0) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[5];
            }

            // Deactivate emergency button for Medusa
            if (Medusa.medusa != null && Medusa.medusa == PlayerInCache.LocalPlayer.PlayerControl) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[8];
            }

            // Deactivate emergency button if Monja awakened
            if (Monja.monja != null && Monja.awakened) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[6];
            }

            // Deactivate emergency button for Fortune teller
            if (FortuneTeller.fortuneTeller != null && FortuneTeller.fortuneTeller == PlayerInCache.LocalPlayer.PlayerControl && !FortuneTeller.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[7];
            }

            // Deactivate emergency button for Devourer
            if (Devourer.devourer != null && Devourer.devourer == PlayerInCache.LocalPlayer.PlayerControl) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[9];
            }

            // Deactivate emergency button for Spiritualist revived player
            if (Spiritualist.revivedPlayer != null && Spiritualist.revivedPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                roleCanCallEmergency = false;
                statusText = Language.usablesTexts[0];
            }

            if (!roleCanCallEmergency) {
                __instance.StatusText.text = statusText;
                __instance.NumberText.text = string.Empty;
                __instance.ClosedLid.gameObject.SetActive(true);
                __instance.OpenLid.gameObject.SetActive(false);
                __instance.ButtonActive = false;
                return;
            }
        }
    }

    [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.DoClick))]
    class AbilityButtonDoClickPatch
    {
        static bool Prefix(AbilityButton __instance) {

            bool blockAbility = gameType >= 2 && !HotPotato.hotPotato;
            if (blockAbility) return false;

            return true;
        }
    }

    [HarmonyPatch(typeof(AbilityButton), nameof(AbilityButton.Update))]
    class AbilityButtonUpdatePatch
    {
        static void Postfix() {

            if (gameType >= 2 && !HotPotato.hotPotato) {
                HudManager.Instance.AbilityButton.Hide();
            }
        }
    }

    [HarmonyPatch]
    class VitalsMinigamePatch
    {
        private static List<TMPro.TextMeshPro> hackerTexts = new List<TMPro.TextMeshPro>();

        [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
        class VitalsMinigameStartPatch
        {

            static void Postfix(VitalsMinigame __instance) {

                if (Hacker.hacker != null && PlayerInCache.LocalPlayer.PlayerControl == Hacker.hacker) {
                    hackerTexts = new List<TMPro.TextMeshPro>();
                    foreach (VitalsPanel panel in __instance.vitals) {
                        TMPro.TextMeshPro text = UnityEngine.Object.Instantiate(__instance.SabText, panel.transform);
                        hackerTexts.Add(text);
                        UnityEngine.Object.DestroyImmediate(text.GetComponent<AlphaBlink>());
                        text.gameObject.SetActive(false);
                        text.transform.localScale = Vector3.one * 0.75f;
                        text.transform.localPosition = new Vector3(-0.75f, -0.23f, 0f);

                    }
                }

                //Fix Visor in Vitals
                foreach (VitalsPanel panel in __instance.vitals) {
                    if (panel.PlayerIcon != null && panel.PlayerIcon.cosmetics.skin != null) {
                        panel.PlayerIcon.cosmetics.skin.transform.position = new Vector3(0, 0, 0f);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
        class VitalsMinigameUpdatePatch
        {

            static void Postfix(VitalsMinigame __instance) {
                // Hacker show time since death

                if (Hacker.hacker != null && Hacker.hacker == PlayerInCache.LocalPlayer.PlayerControl && Hacker.hackerTimer > 0) {
                    for (int k = 0; k < __instance.vitals.Length; k++) {
                        VitalsPanel vitalsPanel = __instance.vitals[k];
                        GameData.PlayerInfo player = vitalsPanel.PlayerInfo;
                        // Hacker update
                        if (vitalsPanel.IsDead) {
                            DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == player?.PlayerId)?.FirstOrDefault();
                            if (deadPlayer != null && k < hackerTexts.Count && hackerTexts[k] != null) {
                                float timeSinceDeath = ((float)(DateTime.UtcNow - deadPlayer.timeOfDeath).TotalMilliseconds);
                                hackerTexts[k].gameObject.SetActive(true);
                                hackerTexts[k].text = Math.Round(timeSinceDeath / 1000) + "s";
                            }
                        }
                    }
                }
                else {
                    foreach (TMPro.TextMeshPro text in hackerTexts)
                        if (text != null && text.gameObject != null)
                            text.gameObject.SetActive(false);
                }
            }
        }
    }

    [HarmonyPatch]
    class AdminPanelPatch
    {
        static Dictionary<SystemTypes, List<Color>> players = new Dictionary<SystemTypes, System.Collections.Generic.List<Color>>();

        [HarmonyPatch(typeof(MapCountOverlay), nameof(MapCountOverlay.Update))]
        class MapCountOverlayUpdatePatch
        {
            static bool Prefix(MapCountOverlay __instance) {
                // Update new positions for sensei Map
                if (activatedSensei && GameOptionsManager.Instance.currentGameOptions.MapId == 0 && !updatedSenseiAdminmap) {
                    GameObject myAdminIcons = GameObject.Find("Main Camera/Hud/ShipMap(Clone)/CountOverlay");
                    myAdminIcons.transform.GetChild(0).transform.position = myAdminIcons.transform.GetChild(0).transform.position + new Vector3(0, -0.2f, 0); // upper engine
                    myAdminIcons.transform.GetChild(1).transform.position = myAdminIcons.transform.GetChild(1).transform.position + new Vector3(0, 0.3f, 0); // lower engine
                    myAdminIcons.transform.GetChild(2).transform.position = myAdminIcons.transform.GetChild(2).transform.position + new Vector3(0.5f, 0, 0); // Reactor
                    myAdminIcons.transform.GetChild(3).transform.position = myAdminIcons.transform.GetChild(3).transform.position + new Vector3(1.6f, 2.3f, 0); // security
                    myAdminIcons.transform.GetChild(4).transform.position = myAdminIcons.transform.GetChild(4).transform.position + new Vector3(0.7f, -0.95f, 0); // medbey
                    myAdminIcons.transform.GetChild(5).transform.position = myAdminIcons.transform.GetChild(5).transform.position + new Vector3(0.5f, -1f, 0); // Cafetería
                    myAdminIcons.transform.GetChild(6).transform.position = myAdminIcons.transform.GetChild(6).transform.position + new Vector3(0.80f, -1, 0); // weapons
                    myAdminIcons.transform.GetChild(7).transform.position = myAdminIcons.transform.GetChild(7).transform.position + new Vector3(-1.5f, -2.6f, 0); // nav
                    myAdminIcons.transform.GetChild(8).transform.position = myAdminIcons.transform.GetChild(8).transform.position + new Vector3(0f, 1.5f, 0); // shields
                    myAdminIcons.transform.GetChild(9).transform.position = myAdminIcons.transform.GetChild(9).transform.position + new Vector3(0.9f, 3f, 0); // cooms
                    myAdminIcons.transform.GetChild(10).transform.position = myAdminIcons.transform.GetChild(10).transform.position + new Vector3(-1.7f, -0.3f, 0); // storage
                    myAdminIcons.transform.GetChild(11).transform.position = myAdminIcons.transform.GetChild(11).transform.position + new Vector3(0.20f, -0.5f, 0); // Admin
                    myAdminIcons.transform.GetChild(12).transform.position = myAdminIcons.transform.GetChild(12).transform.position + new Vector3(0.5f, -1.2f, 0); // elec
                    myAdminIcons.transform.GetChild(13).transform.position = myAdminIcons.transform.GetChild(13).transform.position + new Vector3(-2.9f, 0, 0); // o2
                    updatedSenseiAdminmap = true;
                }

                // Save colors for the Hacker
                __instance.timer += Time.deltaTime;
                if (__instance.timer < 0.1f) {
                    return false;
                }
                __instance.timer = 0f;
                players = new Dictionary<SystemTypes, List<Color>>();
                bool commsActive = false;
                foreach (PlayerTask task in PlayerInCache.LocalPlayer.PlayerControl.myTasks)
                    if (task.TaskType == TaskTypes.FixComms) commsActive = true;


                if (!__instance.isSab && commsActive) {
                    __instance.isSab = true;
                    __instance.BackgroundColor.SetColor(Palette.DisabledGrey);
                    __instance.SabotageText.gameObject.SetActive(true);
                    return false;
                }
                if (__instance.isSab && !commsActive) {
                    __instance.isSab = false;
                    __instance.BackgroundColor.SetColor(Color.green);
                    __instance.SabotageText.gameObject.SetActive(false);
                }

                for (int i = 0; i < __instance.CountAreas.Length; i++) {
                    CounterArea counterArea = __instance.CountAreas[i];
                    List<Color> roomColors = new List<Color>();
                    players.Add(counterArea.RoomType, roomColors);

                    if (!commsActive) {
                        PlainShipRoom plainShipRoom = ShipStatus.Instance.FastRooms[counterArea.RoomType];

                        if (plainShipRoom != null && plainShipRoom.roomArea) {
                            HashSet<int> hashSet = new HashSet<int>();
                            int num = plainShipRoom.roomArea.OverlapCollider(__instance.filter, __instance.buffer);
                            int num2 = 0;
                            for (int j = 0; j < num; j++) {
                                Collider2D collider2D = __instance.buffer[j];
                                if (collider2D.CompareTag("DeadBody") && __instance.includeDeadBodies) {
                                    num2++;
                                    DeadBody bodyComponent = collider2D.GetComponent<DeadBody>();
                                    if (bodyComponent) {
                                        GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(bodyComponent.ParentId);
                                        if (playerInfo != null) {
                                            var color = Palette.PlayerColors[playerInfo.DefaultOutfit.ColorId];
                                            roomColors.Add(color);
                                        }
                                    }
                                }
                                else {
                                    PlayerControl component = collider2D.GetComponent<PlayerControl>();
                                    if (component && component.Data != null && !component.Data.Disconnected && !component.Data.IsDead && (__instance.showLivePlayerPosition || !component.AmOwner) && hashSet.Add((int)component.PlayerId)) {
                                        num2++;
                                        if (component?.cosmetics?.currentBodySprite?.BodySprite?.material != null) {
                                            Color color = component.cosmetics.currentBodySprite.BodySprite.material.GetColor("_BodyColor");
                                            if (Painter.painterTimer > 0) {
                                                color = Palette.PlayerColors[Detective.footprintcolor];
                                            }
                                            roomColors.Add(color);
                                        }
                                    }
                                }
                            }
                            counterArea.UpdateCount(num2);
                        }
                        else {
                            Debug.LogWarning("Couldn't find counter for:" + counterArea.RoomType);
                        }
                    }
                    else {
                        counterArea.UpdateCount(0);
                    }
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(CounterArea), nameof(CounterArea.UpdateCount))]
        class CounterAreaUpdateCountPatch
        {
            private static Material defaultMat;
            private static Material newMat;
            static void Postfix(CounterArea __instance) {
                // Hacker display saved colors on the admin panel
                bool showHackerInfo = Hacker.hacker != null && Hacker.hacker == PlayerInCache.LocalPlayer.PlayerControl && Hacker.hackerTimer > 0;
                if (players.ContainsKey(__instance.RoomType)) {
                    List<Color> colors = players[__instance.RoomType];

                    for (int i = 0; i < __instance.myIcons.Count; i++) {
                        PoolableBehavior icon = __instance.myIcons[i];
                        SpriteRenderer renderer = icon.GetComponent<SpriteRenderer>();

                        if (renderer != null) {
                            if (defaultMat == null) defaultMat = renderer.material;
                            if (newMat == null) newMat = UnityEngine.Object.Instantiate<Material>(defaultMat);
                            if (showHackerInfo && colors.Count > i) {
                                renderer.material = newMat;
                                var color = colors[i];
                                renderer.material.SetColor("_BodyColor", color);
                                var id = Palette.PlayerColors.IndexOf(color);
                                if (id < 0) {
                                    renderer.material.SetColor("_BackColor", color);
                                }
                                else {
                                    renderer.material.SetColor("_BackColor", Palette.ShadowColors[id]);
                                }
                                renderer.material.SetColor("_VisorColor", Palette.VisorColor);
                            }
                            else {
                                renderer.material = defaultMat;
                            }
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch]
    class SurveillanceMinigamePatch
    {
        private static int page = 0;
        private static float timer = 0f;

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Begin))]
        class SurveillanceMinigameBeginPatch
        {
            public static void Postfix(SurveillanceMinigame __instance) {

                if (nightVision) {
                    GameObject gameObject = __instance.Viewables.transform.Find("CloseButton").gameObject;
                    nightOverlay = new List<GameObject>();
                    foreach (MeshRenderer meshRenderer in __instance.ViewPorts) {
                        GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, __instance.Viewables.transform);
                        gameObject2.name = "NightVisionOverlay";
                        gameObject2.transform.position = new Vector3(meshRenderer.transform.position.x, meshRenderer.transform.position.y, gameObject2.transform.position.z);
                        gameObject2.transform.localScale = new Vector3(0.91f, 0.612f, 1f);
                        gameObject2.GetComponent<SpriteRenderer>().sprite = null;
                        UnityEngine.Object.Destroy(gameObject2.GetComponent<ButtonBehavior>());
                        UnityEngine.Object.Destroy(gameObject2.GetComponent<CloseButtonConsoleBehaviour>());
                        UnityEngine.Object.Destroy(gameObject2.GetComponent<CircleCollider2D>());
                        nightOverlay.Add(gameObject2);
                    }
                    canNightOverlay = true;
                    removeNightOverlay = true;
                }

                // Add Vigilant cameras
                page = 0;
                timer = 0;
                if (ShipStatus.Instance.AllCameras.Length > 4 && __instance.FilteredRooms.Length > 0) {
                    __instance.textures = __instance.textures.ToList().Concat(new RenderTexture[ShipStatus.Instance.AllCameras.Length - 4]).ToArray();
                    for (int i = 4; i < ShipStatus.Instance.AllCameras.Length; i++) {
                        SurvCamera surv = ShipStatus.Instance.AllCameras[i];
                        Camera camera = UnityEngine.Object.Instantiate<Camera>(__instance.CameraPrefab);
                        camera.transform.SetParent(__instance.transform);
                        camera.transform.position = new Vector3(surv.transform.position.x, surv.transform.position.y, 8f);
                        camera.orthographicSize = 2.35f;
                        RenderTexture temporary = RenderTexture.GetTemporary(256, 256, 16, (RenderTextureFormat)0);
                        __instance.textures[i] = temporary;
                        camera.targetTexture = temporary;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Update))]
        class SurveillanceMinigameUpdatePatch
        {

            public static bool Prefix(SurveillanceMinigame __instance) {

                // Change camera position on SenseiMap
                if (GameOptionsManager.Instance.currentGameOptions.MapId == 0 && activatedSensei) {
                    GameObject myCameras = GameObject.Find("Main Camera/SurvMinigame(Clone)");
                    myCameras.transform.GetChild(3).transform.position = new Vector3(9.45f, -1.48f, myCameras.transform.GetChild(3).transform.position.z);
                    myCameras.transform.GetChild(4).transform.position = new Vector3(-8.5f, 2.29f, myCameras.transform.GetChild(4).transform.position.z);
                    myCameras.transform.GetChild(5).transform.position = new Vector3(-16.5f, -6.215f, myCameras.transform.GetChild(5).transform.position.z);
                    myCameras.transform.GetChild(6).transform.position = new Vector3(5f, -13.85f, myCameras.transform.GetChild(6).transform.position.z);
                }

                timer += Time.deltaTime;
                int numberOfPages = Mathf.CeilToInt(ShipStatus.Instance.AllCameras.Length / 4f);

                bool update = false;

                if (timer > 3f || Input.GetKeyDown(KeyCode.RightArrow)) {
                    update = true;
                    timer = 0f;
                    page = (page + 1) % numberOfPages;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    page = (page + numberOfPages - 1) % numberOfPages;
                    update = true;
                    timer = 0f;
                }

                if ((__instance.isStatic || update) && !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerInCache.LocalPlayer.PlayerControl)) {
                    __instance.isStatic = false;
                    for (int i = 0; i < __instance.ViewPorts.Length; i++) {
                        __instance.ViewPorts[i].sharedMaterial = __instance.DefaultMaterial;
                        __instance.SabText[i].gameObject.SetActive(false);
                        if (page * 4 + i < __instance.textures.Length)
                            __instance.ViewPorts[i].material.SetTexture("_MainTex", __instance.textures[page * 4 + i]);
                        else
                            __instance.ViewPorts[i].sharedMaterial = __instance.StaticMaterial;
                    }
                }
                else if (!__instance.isStatic && PlayerTask.PlayerHasTaskOfType<HudOverrideTask>(PlayerInCache.LocalPlayer.PlayerControl)) {
                    __instance.isStatic = true;
                    for (int j = 0; j < __instance.ViewPorts.Length; j++) {
                        __instance.ViewPorts[j].sharedMaterial = __instance.StaticMaterial;
                        __instance.SabText[j].gameObject.SetActive(true);
                    }
                }

                if (nightVision && !PlayerInCache.LocalPlayer.Data.Role.IsImpostor) {
                    if (Modifiers.lighter != null && PlayerInCache.LocalPlayer.PlayerControl == Modifiers.lighter) return false;
                    foreach (PlayerTask task in PlayerInCache.LocalPlayer.PlayerControl.myTasks) {
                        isLightsOut = false;
                        if (task.TaskType == TaskTypes.FixLights) {
                            isLightsOut = true;
                            if (canNightOverlay) {
                                foreach (GameObject gameObjecttwo in nightOverlay) {
                                    gameObjecttwo.GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.NightVisionCamera.png", 350f);
                                }
                                canNightOverlay = false;
                                removeNightOverlay = true;
                            }
                            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                                player.setLook("", 11, "", "", "", "");
                                if (player.cosmetics.currentPet) player.cosmetics.currentPet.gameObject.SetActive(false);
                            }
                            return false;
                        }
                    }

                    if (removeNightOverlay && !isLightsOut) {
                        foreach (GameObject gameObjecttwo in nightOverlay) {
                            gameObjecttwo.GetComponent<SpriteRenderer>().sprite = null;
                        }
                        foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                            player.setDefaultLook();
                            if (player.cosmetics.currentPet) player.cosmetics.currentPet.gameObject.SetActive(true);
                        }
                        canNightOverlay = true;
                        removeNightOverlay = false;
                    }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.Close))]
        class SurveillanceMinigameClosePatch
        {

            public static bool Prefix(SurveillanceMinigame __instance) {

                if (nightVision) {
                    foreach (GameObject gameObjecttwo in nightOverlay) {
                        gameObjecttwo.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        player.setDefaultLook();
                    }
                    canNightOverlay = true;
                    removeNightOverlay = true;
                }
                return true;
            }

        }

        [HarmonyPatch(typeof(SurveillanceMinigame), nameof(SurveillanceMinigame.OnDestroy))]
        class SurveillanceMinigameOnDestroyPatch
        {

            public static bool Prefix(SurveillanceMinigame __instance) {

                if (nightVision) {
                    foreach (GameObject gameObjecttwo in nightOverlay) {
                        gameObjecttwo.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        player.setDefaultLook();
                    }
                    canNightOverlay = true;
                    removeNightOverlay = true;
                }
                return true;
            }

        }

        [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.Begin))]
        class PlanetSurveillanceMinigameBeginPatch
        {
            public static void Postfix(PlanetSurveillanceMinigame __instance) {

                if (nightVision) {
                    GameObject gameObject = __instance.Viewables.transform.Find("CloseButton").gameObject;
                    nightOverlay = new List<GameObject>();
                    MeshRenderer meshRenderer = __instance.ViewPort;
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject, __instance.Viewables.transform);
                    gameObject2.name = "NightVisionOverlay";
                    gameObject2.transform.position = new Vector3(meshRenderer.transform.position.x, meshRenderer.transform.position.y, gameObject2.transform.position.z);
                    gameObject2.transform.localScale = new Vector3(0.915f, 0.585f, 1f);
                    gameObject2.GetComponent<SpriteRenderer>().sprite = null;
                    UnityEngine.Object.Destroy(gameObject2.GetComponent<ButtonBehavior>());
                    UnityEngine.Object.Destroy(gameObject2.GetComponent<CloseButtonConsoleBehaviour>());
                    UnityEngine.Object.Destroy(gameObject2.GetComponent<CircleCollider2D>());
                    nightOverlay.Add(gameObject2);

                    canNightOverlay = true;
                    removeNightOverlay = true;
                }
            }
        }

        [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.Update))]
        class PlanetSurveillanceMinigameUpdatePatch
        {

            public static bool Prefix(PlanetSurveillanceMinigame __instance) {

                if (nightVision && !PlayerInCache.LocalPlayer.Data.Role.IsImpostor) {
                    if (Modifiers.lighter != null && PlayerInCache.LocalPlayer.PlayerControl == Modifiers.lighter) return false;
                    foreach (PlayerTask task in PlayerInCache.LocalPlayer.PlayerControl.myTasks) {
                        isLightsOut = false;
                        if (task.TaskType == TaskTypes.FixLights) {
                            isLightsOut = true;
                            if (canNightOverlay) {
                                foreach (GameObject gameObjecttwo in nightOverlay) {
                                    gameObjecttwo.GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.NightVisionCamera.png", 150f);
                                }
                                canNightOverlay = false;
                                removeNightOverlay = true;
                            }
                            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                                player.setLook("", 11, "", "", "", "");
                            }
                            return false;
                        }
                    }

                    if (removeNightOverlay && !isLightsOut) {
                        foreach (GameObject gameObjecttwo in nightOverlay) {
                            gameObjecttwo.GetComponent<SpriteRenderer>().sprite = null;
                        }
                        foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                            player.setDefaultLook();
                        }
                        canNightOverlay = true;
                        removeNightOverlay = false;
                    }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.Close))]
        class PlanetSurveillanceMinigameClosePatch
        {

            public static bool Prefix(PlanetSurveillanceMinigame __instance) {

                if (nightVision) {
                    foreach (GameObject gameObjecttwo in nightOverlay) {
                        gameObjecttwo.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        player.setDefaultLook();
                    }
                    canNightOverlay = true;
                    removeNightOverlay = true;
                }
                return true;
            }

        }

        [HarmonyPatch(typeof(PlanetSurveillanceMinigame), nameof(PlanetSurveillanceMinigame.OnDestroy))]
        class PlanetSurveillanceMinigameOnDestroyPatch
        {

            public static bool Prefix(PlanetSurveillanceMinigame __instance) {

                if (nightVision) {
                    foreach (GameObject gameObjecttwo in nightOverlay) {
                        gameObjecttwo.GetComponent<SpriteRenderer>().sprite = null;
                    }
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        player.setDefaultLook();
                    }
                    canNightOverlay = true;
                    removeNightOverlay = true;
                }
                return true;
            }

        }

        // This class is partially taken from The Other Roles to fix outfits on ziplines https://github.com/TheOtherRolesAU/TheOtherRoles/blob/main/TheOtherRoles/Patches/TransportationToolPatches.cs Licensed under GPLv3
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZiplineBehaviour), nameof(ZiplineBehaviour.Use), new Type[] { typeof(PlayerControl), typeof(bool) })]
        public static void postfix(ZiplineBehaviour __instance, PlayerControl player, bool fromTop) {
            __instance.StartCoroutine(Effects.Lerp(fromTop ? __instance.downTravelTime : __instance.upTravelTime, new System.Action<float>((p) => {
                HandZiplinePoolable hand;
                __instance.playerIdHands.TryGetValue(player.PlayerId, out hand);
                if (hand != null) {
                    if (Painter.painterTimer <= 0 && !Helpers.MushroomSabotageActive()) {
                        if (player == Mimic.mimic && Mimic.transformTimer > 0) {
                            hand.SetPlayerColor(Mimic.transformTarget.CurrentOutfit, PlayerMaterial.MaskType.None);
                        }
                        else if (player == Puppeteer.puppeteer && Puppeteer.morphed) {
                            hand.SetPlayerColor(Puppeteer.transformTarget.CurrentOutfit, PlayerMaterial.MaskType.None);
                        }
                        else {
                            hand.SetPlayerColor(player.CurrentOutfit, PlayerMaterial.MaskType.None);
                        }
                    }
                    else {
                        PlayerMaterial.SetColors(6, hand.handRenderer);
                    }
                }
            })));
        }
    }
}