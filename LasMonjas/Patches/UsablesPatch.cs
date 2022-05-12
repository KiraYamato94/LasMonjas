using HarmonyLib;
using System;
using Hazel;
using UnityEngine;
using System.Linq;
using static LasMonjas.LasMonjas;
using static LasMonjas.GameHistory;
using static LasMonjas.MapOptions;
using System.Collections.Generic;


namespace LasMonjas.Patches {

    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        public static bool Prefix(bool __runOriginal, Vent __instance, ref float __result, [HarmonyArgument(0)] GameData.PlayerInfo pc, [HarmonyArgument(1)] ref bool canUse, [HarmonyArgument(2)] ref bool couldUse)
        {
            if (!__runOriginal) {
                return false;
            }
			
			float num = float.MaxValue;
            PlayerControl @object = pc.Object;

            bool roleCouldUse = @object.roleCanUseVents();

            var usableDistance = __instance.UsableDistance;
            if (__instance.name.StartsWith("Hat_")) {
                if (Ilusionist.ilusionist != PlayerControl.LocalPlayer) {
                    // Only the Ilusionist can use the Hats
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
            else if (PlayerControl.GameOptions.MapId == 5) {
                if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor && (__instance.name.StartsWith("LowerCentralVent") || __instance.name.StartsWith("UpperCentralVent"))) {
                    canUse = couldUse = false;
                    __result = num;
                    return false;
                }
            }

            couldUse = (@object.inVent || roleCouldUse) && !pc.IsDead && (@object.CanMove || @object.inVent);
            canUse = couldUse;
            if (canUse)
            {
                Vector2 truePosition = @object.GetTruePosition();
                Vector3 position = __instance.transform.position;
                num = Vector2.Distance(truePosition, position);
                
                canUse &= (num <= usableDistance && !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShipOnlyMask, false));
            }
            __result = num;
            return false;
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.DoClick))]
    class VentButtonDoClickPatch {
        static  bool Prefix(VentButton __instance) {
            // Manually modifying the VentButton to use Vent.Use again in order to trigger the Vent.Use prefix patch
		    if (__instance.currentTarget != null) __instance.currentTarget.Use();
            return false;
        }
    }

    [HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
    public static class VentUsePatch {
        public static bool Prefix(Vent __instance) {
            bool canUse;
            bool couldUse;
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
            bool canMoveInVents = true;
            if (!canUse) return false; 

            bool isEnter = !PlayerControl.LocalPlayer.inVent;
            
            if (__instance.name.StartsWith("Hat_")) {
                __instance.SetButtons(isEnter && canMoveInVents);
                MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UseUncheckedVent, Hazel.SendOption.Reliable);
                writer.WritePacked(__instance.Id);
                writer.Write(PlayerControl.LocalPlayer.PlayerId);
                writer.Write(isEnter ? byte.MaxValue : (byte)0);
                writer.EndMessage();
                RPCProcedure.useUncheckedVent(__instance.Id, PlayerControl.LocalPlayer.PlayerId, isEnter ? byte.MaxValue : (byte)0);
                return false;
            }

            if(isEnter) {
                PlayerControl.LocalPlayer.MyPhysics.RpcEnterVent(__instance.Id);
            } else {
                PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(__instance.Id);
            }
            __instance.SetButtons(isEnter && canMoveInVents);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    class VentButtonVisibilityPatch {
        static void Postfix(PlayerControl __instance) {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            if (__instance.AmOwner && __instance.roleCanUseVents() && howmanygamemodesareon == 1) {
                HudManager.Instance.ImpostorVentButton.Show();
            }
            else if (__instance.AmOwner && __instance.roleCanUseVents() && HudManager.Instance.ReportButton.isActiveAndEnabled || __instance.AmOwner && __instance.roleCanUseVents() && HudManager.Instance.ReportButton.isActiveAndEnabled && howmanygamemodesareon != 1) {
                HudManager.Instance.ImpostorVentButton.Show();
            }
        }
    }

    [HarmonyPatch(typeof(VentButton), nameof(VentButton.SetTarget))]
    class VentButtonSetTargetPatch {
        static Sprite defaultVentSprite = null;
        static void Postfix(VentButton __instance) {
            // Ilusionist render special vent button
            if (Ilusionist.ilusionist != null && Ilusionist.ilusionist == PlayerControl.LocalPlayer) {
                if (defaultVentSprite == null) defaultVentSprite = __instance.graphic.sprite;
                bool isSpecialVent = __instance.currentTarget != null && __instance.currentTarget.gameObject != null && __instance.currentTarget.gameObject.name.StartsWith("Hat_");
                __instance.graphic.sprite = isSpecialVent ? Ilusionist.getIlusionistVentButtonSprite() : defaultVentSprite;
                __instance.buttonLabelText.enabled = !isSpecialVent;
            }
        }
    }

    [HarmonyPatch(typeof(KillButton), nameof(KillButton.DoClick))]
    class KillButtonDoClickPatch {
        public static bool Prefix(KillButton __instance) {
            if (__instance.isActiveAndEnabled && __instance.currentTarget && !__instance.isCoolingDown && !PlayerControl.LocalPlayer.Data.IsDead && PlayerControl.LocalPlayer.CanMove) {
                // Use an unchecked kill command, to allow shorter kill cooldowns etc. without getting kicked
                MurderAttemptResult res = Helpers.checkMurderAttemptAndKill(PlayerControl.LocalPlayer, __instance.currentTarget);
                // Handle Jinx kill
                if (res == MurderAttemptResult.JinxKill) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.jinxQuack, false, 5f);
                    PlayerControl.LocalPlayer.killTimer = PlayerControl.GameOptions.KillCooldown;
                    if (PlayerControl.LocalPlayer == Janitor.janitor)
                        Janitor.janitor.killTimer = HudManagerStartPatch.janitorCleanButton.Timer = HudManagerStartPatch.janitorCleanButton.MaxTimer;
                    else if (PlayerControl.LocalPlayer == Manipulator.manipulator)
                        Manipulator.manipulator.killTimer = HudManagerStartPatch.manipulatorManipulateButton.Timer = HudManagerStartPatch.manipulatorManipulateButton.MaxTimer;
                    else if (PlayerControl.LocalPlayer == Sorcerer.sorcerer)
                        Sorcerer.sorcerer.killTimer = HudManagerStartPatch.sorcererSpellButton.Timer = HudManagerStartPatch.sorcererSpellButton.MaxTimer;
                }
                __instance.SetTarget(null);
            }
            return false;
        }
    }


    [HarmonyPatch(typeof(SabotageButton), nameof(SabotageButton.DoClick))]
    class SabotageButtonDoClickPatch
    {
        static bool Prefix(SabotageButton __instance) {

            // Block sabotage button on custom gamemodes
            if (howmanygamemodesareon == 1) {
                return false;
            }
            else {
                
                // Block sabotage button if Bomberman bomb, lights out, duel or special condition 1vs1 is active
                bool blockSabotage = (PlayerControl.LocalPlayer.Data.Role.IsImpostor || (Joker.canSabotage && Joker.joker != null && Joker.joker == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead)) && (alivePlayers <= 2 || Bomberman.activeBomb || Challenger.isDueling || Ilusionist.lightsOutTimer > 0);
                if (blockSabotage) return false;

                // Joker sabotage
                if (Joker.canSabotage && Joker.joker != null && Joker.joker == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && !Bomberman.activeBomb && !Challenger.isDueling && Ilusionist.lightsOutTimer <= 0) {
                    MapBehaviour.Instance.ShowSabotageMap();
                    return false;
                }
            }

            return true;
        }
    }
    
    [HarmonyPatch(typeof(SabotageButton), nameof(SabotageButton.Refresh))]
    class SabotageButtonRefreshPatch {
        static void Postfix() {

            // Change sabotage button image and block sabotages if capture the flag mode or police and thief mode
            if (howmanygamemodesareon == 1) {
                HudManager.Instance.SabotageButton.Hide();
            }

            else {

                // Block sabotage button if Bomberman bomb, lights out, duel or special condition 1vs1 is active
                bool blockSabotage = (PlayerControl.LocalPlayer.Data.Role.IsImpostor || (Joker.canSabotage && Joker.joker != null && Joker.joker == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead)) && (alivePlayers <= 2 || Bomberman.activeBomb == true || Challenger.isDueling || Ilusionist.lightsOutTimer > 0);
                if (blockSabotage) {
                    HudManager.Instance.SabotageButton.SetDisabled();
                }

                if (Joker.canSabotage && Joker.joker != null && Joker.joker == PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.Data.IsDead && !Bomberman.activeBomb && !Challenger.isDueling && Ilusionist.lightsOutTimer <= 0) {
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
            bool blockReport = Challenger.isDueling || Spiritualist.preventReport || howmanygamemodesareon == 1;
            if (blockReport) return false;

            return true;
        }
    }
    
    [HarmonyPatch(typeof(EmergencyMinigame), nameof(EmergencyMinigame.Update))]
    class EmergencyMinigameUpdatePatch {
        static void Postfix(EmergencyMinigame __instance) {
            var roleCanCallEmergency = true;
            var statusText = "";

            // Deactivate emergency button for custom gamemodes
            if (howmanygamemodesareon == 1) {
                roleCanCallEmergency = false;
                statusText = "Can't use the emergency button \non custom gamemodes!";
            }
            // Deactivate emergency button for Cheater
            if (Cheater.cheater != null && Cheater.cheater == PlayerControl.LocalPlayer && !Cheater.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = "The Cheater can't use the emergency button!";
            }

            // Deactivate emergency button for Gambler
            if (Gambler.gambler != null && Gambler.gambler == PlayerControl.LocalPlayer && !Gambler.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = "The Gambler can't use the emergency button!";
            }

            // Deactivate emergency button for Sorcerer
            if (Sorcerer.sorcerer != null && Sorcerer.sorcerer == PlayerControl.LocalPlayer && !Sorcerer.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = "The Sorcerer can't use the emergency button!";
            }

            // Deactivate emergency button for TreasureHunter
            if (TreasureHunter.treasureHunter != null && TreasureHunter.treasureHunter == PlayerControl.LocalPlayer && !TreasureHunter.canCallEmergency) {
                roleCanCallEmergency = false;
                statusText = "The Treasure Hunter can't use the emergency button!";
            }

            // Deactivate emergency button if there's a bomb
            if (Bomberman.bomberman != null && Bomberman.activeBomb == true) {
                roleCanCallEmergency = false;
                statusText = "There's a Bomb, you can't use the emergency button!";
            }

            // Deactivate emergency button if there's lights out
            if (Ilusionist.ilusionist != null && Ilusionist.lightsOutTimer > 0) {
                roleCanCallEmergency = false;
                statusText = "There's a Blackout, emergency button doesn't work!";
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

    [HarmonyPatch]
    class VitalsMinigamePatch
    {
        private static List<TMPro.TextMeshPro> hackerTexts = new List<TMPro.TextMeshPro>();

        [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Begin))]
        class VitalsMinigameStartPatch
        {

            static void Postfix(VitalsMinigame __instance) {

                if (Hacker.hacker != null && PlayerControl.LocalPlayer == Hacker.hacker) {
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
                    if (panel.PlayerIcon != null && panel.PlayerIcon.Skin != null) {
                        panel.PlayerIcon.Skin.transform.position = new Vector3(0, 0, 0f);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
        class VitalsMinigameUpdatePatch
        {

            static void Postfix(VitalsMinigame __instance) {
                // Hacker show time since death

                if (Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer && Hacker.hackerTimer > 0) {
                    for (int k = 0; k < __instance.vitals.Length; k++) {
                        VitalsPanel vitalsPanel = __instance.vitals[k];
                        GameData.PlayerInfo player = vitalsPanel.PlayerInfo;
                        // Hacker update
                        if (vitalsPanel.IsDead) {
                            DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == player?.PlayerId)?.FirstOrDefault();
                            if (deadPlayer != null && deadPlayer.timeOfDeath != null && k < hackerTexts.Count && hackerTexts[k] != null) {
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
        //static Dictionary<SystemTypes, List<Color>> players = new Dictionary<SystemTypes, List<Color>>();
        static Dictionary<SystemTypes, List<Color>> players = new Dictionary<SystemTypes, System.Collections.Generic.List<Color>>();

        [HarmonyPatch(typeof(MapCountOverlay), nameof(MapCountOverlay.Update))]
        class MapCountOverlayUpdatePatch
        {
            static bool Prefix(MapCountOverlay __instance) {
                // Update new positions for sensei Map
                if (activatedSensei && PlayerControl.GameOptions.MapId == 0 && !updatedSenseiAdminmap) {
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
                foreach (PlayerTask task in PlayerControl.LocalPlayer.myTasks)
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
                            int num = plainShipRoom.roomArea.OverlapCollider(__instance.filter, __instance.buffer);
                            int num2 = num;
                            for (int j = 0; j < num; j++) {
                                Collider2D collider2D = __instance.buffer[j];
                                if (!(collider2D.tag == "DeadBody")) {
                                    PlayerControl component = collider2D.GetComponent<PlayerControl>();
                                    if (!component || component.Data == null || component.Data.Disconnected || component.Data.IsDead) {
                                        num2--;
                                    }
                                    else if (component?.MyRend?.material != null) {
                                        Color color = component.MyRend.material.GetColor("_BodyColor");
                                        roomColors.Add(color);
                                    }
                                }
                                else {
                                    DeadBody component = collider2D.GetComponent<DeadBody>();
                                    if (component) {
                                        GameData.PlayerInfo playerInfo = GameData.Instance.GetPlayerById(component.ParentId);
                                        if (playerInfo != null) {
                                            var color = Palette.PlayerColors[playerInfo.DefaultOutfit.ColorId];
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
                bool showHackerInfo = Hacker.hacker != null && Hacker.hacker == PlayerControl.LocalPlayer && Hacker.hackerTimer > 0;
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
                if (PlayerControl.GameOptions.MapId == 0 && activatedSensei) {
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

                if ((__instance.isStatic || update) && !PlayerTask.PlayerHasTaskOfType<IHudOverrideTask>(PlayerControl.LocalPlayer)) {
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
                else if (!__instance.isStatic && PlayerTask.PlayerHasTaskOfType<HudOverrideTask>(PlayerControl.LocalPlayer)) {
                    __instance.isStatic = true;
                    for (int j = 0; j < __instance.ViewPorts.Length; j++) {
                        __instance.ViewPorts[j].sharedMaterial = __instance.StaticMaterial;
                        __instance.SabText[j].gameObject.SetActive(true);
                    }
                }
                return false;
            }
        }
    }
}