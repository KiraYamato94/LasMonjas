using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using static LasMonjas.LasMonjas;
using static LasMonjas.GameHistory;
using LasMonjas.Objects;
using UnityEngine;
using LasMonjas.Core;
using AmongUs.GameOptions;

namespace LasMonjas.Patches {
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class PlayerControlFixedUpdatePatch {

        static PlayerControl setTarget(bool onlyCrewmates = false, bool targetPlayersInVents = false, List<PlayerControl> untargetablePlayers = null, PlayerControl targetingPlayer = null) {
            PlayerControl result = null;
            float num = LegacyGameOptions.KillDistances[Mathf.Clamp(GameOptionsManager.Instance.currentGameOptions.GetInt(Int32OptionNames.KillDistance), 0, 2)];
            if (!ShipStatus.Instance) return result;
            if (targetingPlayer == null) targetingPlayer = PlayerInCache.LocalPlayer.PlayerControl;
            if (targetingPlayer.Data.IsDead) return result;

            Vector2 truePosition = targetingPlayer.GetTruePosition();
            Il2CppSystem.Collections.Generic.List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++) {
                NetworkedPlayerInfo playerInfo = allPlayers[i];
                if (!playerInfo.Disconnected && playerInfo.PlayerId != targetingPlayer.PlayerId && !playerInfo.IsDead && (!onlyCrewmates || !playerInfo.Role.IsImpostor)) {
                    PlayerControl @object = playerInfo.Object;
                    if (untargetablePlayers != null && untargetablePlayers.Any(x => x == @object)) {
                        // if that player is not targetable: skip check
                        continue;
                    }

                    if (@object && (!@object.inVent || targetPlayersInVents)) {
                        Vector2 vector = @object.GetTruePosition() - truePosition;
                        float magnitude = vector.magnitude;
                        if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask)) {
                            result = @object;
                            num = magnitude;
                        }
                    }
                }
            }
            return result;
        }
        static void setPlayerOutline(PlayerControl target, Color color) {
            if (target == null || target.cosmetics.currentBodySprite.BodySprite == null) return;

            target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
            target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", color);
        }
        static void setBasePlayerOutlines() {
            foreach (PlayerControl target in PlayerInCache.AllPlayers) {
                if (target == null || target.cosmetics.currentBodySprite.BodySprite == null) continue;

                bool isTransformedMimic = target == Mimic.mimic && Mimic.transformTarget != null && Mimic.transformTimer > 0f;
                bool isTransformedPuppeteer = target == Puppeteer.puppeteer && Puppeteer.transformTarget != null && Puppeteer.morphed;
                bool hasVisibleShield = false;
                if (Painter.painterTimer <= 0f && !Helpers.MushroomSabotageActive() && Squire.shielded != null && !Challenger.isDueling && !Seeker.isMinigaming && ((target == Squire.shielded && !isTransformedMimic) || (isTransformedMimic && Mimic.transformTarget == Squire.shielded) || (isTransformedPuppeteer && Puppeteer.transformTarget == Squire.shielded))) {
                    hasVisibleShield = Squire.showShielded == 0 && PlayerInCache.LocalPlayer.PlayerControl == Squire.squire // Squire only
                        || (Squire.showShielded == 1 && (PlayerInCache.LocalPlayer.PlayerControl == Squire.shielded || PlayerInCache.LocalPlayer.PlayerControl == Squire.squire)) // Shielded + Squire
                        || (Squire.showShielded == 2); // Everyone
                }

                if (hasVisibleShield) {
                    target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
                    target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", Squire.color);
                }
                else {
                    target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 0f);
                }
            }
        }
        // Show player roles on meeting for dead players
        public static void ghostsSeePlayerRoles() {
            if (gameType <= 1) {
                foreach (PlayerControl p in PlayerInCache.AllPlayers) {
                    if (p == PlayerInCache.LocalPlayer.PlayerControl || PlayerInCache.LocalPlayer.PlayerControl.Data.IsDead) {

                        PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.PlayerId == p.PlayerId);
                        Transform meetingInfoTransform = playerVoteArea != null ? playerVoteArea.NameText.transform.parent.FindChild("Info") : null;
                        TMPro.TextMeshPro meetingInfo = meetingInfoTransform != null ? meetingInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                        if (meetingInfo == null && playerVoteArea != null) {
                            meetingInfo = UnityEngine.Object.Instantiate(playerVoteArea.NameText, playerVoteArea.NameText.transform.parent);
                            meetingInfo.transform.localPosition += Vector3.down * 0.10f;
                            meetingInfo.fontSize *= 0.60f;
                            meetingInfo.gameObject.name = "Info";
                        }

                        // Set player name higher to align in middle
                        if (meetingInfo != null && playerVoteArea != null) {
                            var playerName = playerVoteArea.NameText;
                            playerName.transform.localPosition = new Vector3(0.3384f, (0.0311f + 0.0683f), -0.1f);
                        }

                        string roleNames = RoleInfo.GetRolesString(p, true);

                        string playerInfoText = "";
                        string meetingInfoText = "";
                        if (MapOptions.ghostsSeeRoles && PlayerInCache.LocalPlayer.PlayerControl.Data.IsDead) {
                            playerInfoText = $"{roleNames}";
                            meetingInfoText = playerInfoText;
                        }

                        if (meetingInfo != null) meetingInfo.text = MeetingHud.Instance.state == MeetingHud.MeetingStates.Results ? "" : meetingInfoText;
                    }
                }
            }
        }
        static void ventColorUpdate() {
            if (PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor && ShipStatus.Instance?.AllVents != null) {
                foreach (Vent vent in ShipStatus.Instance.AllVents) {
                    try {
                        if (vent?.myRend?.material != null) {
                            if (Renegade.renegade != null && Renegade.renegade.inVent || Minion.minion != null && Minion.minion.inVent || Stranded.stranded != null && Stranded.stranded.inVent) {
                                vent.myRend.material.SetFloat("_Outline", 1f);
                                vent.myRend.material.SetColor("_OutlineColor", Renegade.color);
                            }
                            else if (vent.myRend.material.GetColor("_AddColor") != Color.red) {
                                vent.myRend.material.SetFloat("_Outline", 0);
                            }
                        }
                    }
                    catch { }
                }
            }
        }
        static void impostorSetTarget() {
            if (!PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor || Archer.archer != null && PlayerInCache.LocalPlayer.PlayerControl == Archer.archer || Demon.demon != null && PlayerInCache.LocalPlayer.PlayerControl == Demon.demon || !PlayerInCache.LocalPlayer.PlayerControl.CanMove || PlayerInCache.LocalPlayer.PlayerControl.Data.IsDead || gameType >= 2) { // !isImpostor || !canMove || isDead
                HudManager.Instance.KillButton.SetTarget(null);
                return;
            }

            PlayerControl target = null;
            target = setTarget(true, false);

            HudManager.Instance.KillButton.SetTarget(target);
        }
        static void mimicSetTarget() {
            if (Mimic.mimic == null || Mimic.mimic != PlayerInCache.LocalPlayer.PlayerControl) return;
            Mimic.currentTarget = setTarget();
            setPlayerOutline(Mimic.currentTarget, Mimic.color);
        }

        static void mimicAndPainterUpdate() {
            float oldPaintTimer = Painter.painterTimer;
            float oldMimicTimer = Mimic.transformTimer;
            Painter.painterTimer = Mathf.Max(0f, Painter.painterTimer - Time.fixedDeltaTime);
            Mimic.transformTimer = Mathf.Max(0f, Mimic.transformTimer - Time.fixedDeltaTime);

            // Paint reset and set Mimic look if necessary
            if (oldPaintTimer > 0f && Painter.painterTimer <= 0f) {
                Painter.resetPaint();
                if (Mimic.transformTimer > 0f && Mimic.mimic != null && Mimic.transformTarget != null && !Helpers.MushroomSabotageActive()) {
                    PlayerControl target = Mimic.transformTarget;
                    Mimic.mimic.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }
                if (Puppeteer.puppeteer != null && Puppeteer.morphed && !Helpers.MushroomSabotageActive()) {
                    PlayerControl target = Puppeteer.transformTarget;
                    Puppeteer.puppeteer.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }                
            }

            // Mimic reset (only if paint is inactive)
            if (Painter.painterTimer <= 0f && oldMimicTimer > 0f && Mimic.transformTimer <= 0f && Mimic.mimic != null)
                Mimic.resetMimic();
        }
        static void demonSetTarget() {
            if (Demon.demon == null || Demon.demon != PlayerInCache.LocalPlayer.PlayerControl) return;

            PlayerControl target = null;
            target = setTarget(true, true);

            bool targetNearNun = false;
            if (target != null) {
                foreach (Nun nun in Nun.nuns) {
                    if (Vector2.Distance(nun.nun.transform.position, target.transform.position) <= 1.91f) {
                        targetNearNun = true;
                    }
                }
            }
            Demon.targetNearNun = targetNearNun;
            Demon.currentTarget = target;
            setPlayerOutline(Demon.currentTarget, Demon.color);
        }
        static void manipulatorSetTarget() {
            if (Manipulator.manipulator == null || Manipulator.manipulator != PlayerInCache.LocalPlayer.PlayerControl) return;
            if (Manipulator.manipulatedVictim != null && (Manipulator.manipulatedVictim.Data.Disconnected || Manipulator.manipulatedVictim.Data.IsDead)) {
                // If the manipulated victim is disconnected or dead reset the manipulate so a new manipulate can be applied
                Manipulator.resetManipulate();
            }
            if (Manipulator.manipulatedVictim == null) {
                PlayerControl target;

                target = setTarget(true, false); 
                Manipulator.currentTarget = target;
                setPlayerOutline(Manipulator.currentTarget, Manipulator.color);
            }
        }

        static void manipulatedVictimSetTarget() {
            if (Manipulator.manipulatedVictim == null || Manipulator.manipulatedVictim != PlayerInCache.LocalPlayer.PlayerControl) return;
            Manipulator.manipulatedVictimTarget = setTarget();
            setPlayerOutline(Manipulator.manipulatedVictimTarget, Manipulator.color);
        }

        static void sorcererSetTarget() {
            if (Sorcerer.sorcerer == null || Sorcerer.sorcerer != PlayerInCache.LocalPlayer.PlayerControl) return;
            List<PlayerControl> untargetables;
            if (Sorcerer.spellTarget != null)
                untargetables = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId != Sorcerer.spellTarget.PlayerId).ToList(); // Don't switch the target from the the one you're currently casting a spell on
            else {
                untargetables = Sorcerer.spelledPlayers; 
            }
            Sorcerer.currentTarget = setTarget(true, untargetablePlayers: untargetables);
            setPlayerOutline(Sorcerer.currentTarget, Sorcerer.color);
        }
        static void medusaSetTarget() {
            if (Medusa.medusa == null || Medusa.medusa != PlayerInCache.LocalPlayer.PlayerControl) return;
            PlayerControl target;

            target = setTarget(true, false);
            Medusa.currentTarget = target;
            setPlayerOutline(Medusa.currentTarget, Medusa.color);
        }
        static void librarianSetTarget() {
            if (Librarian.librarian == null || Librarian.librarian != PlayerInCache.LocalPlayer.PlayerControl) return;
            Librarian.currentTarget = setTarget(true, false);
            setPlayerOutline(Librarian.currentTarget, Librarian.color);
        }
        static void renegadeSetTarget() {
            if (Renegade.renegade == null || Renegade.renegade != PlayerInCache.LocalPlayer.PlayerControl) return;
            var untargetablePlayers = new List<PlayerControl>();
            if (Minion.minion != null) untargetablePlayers.Add(Minion.minion);
            Renegade.currentTarget = setTarget(untargetablePlayers: untargetablePlayers);
            setPlayerOutline(Renegade.currentTarget, Palette.ImpostorRed);
        }
        static void minionSetTarget() {
            if (Minion.minion == null || Minion.minion != PlayerInCache.LocalPlayer.PlayerControl) return;
            var untargetablePlayers = new List<PlayerControl>();
            if (Renegade.renegade != null) untargetablePlayers.Add(Renegade.renegade);
            Minion.currentTarget = setTarget(untargetablePlayers: untargetablePlayers);
            setPlayerOutline(Minion.currentTarget, Palette.ImpostorRed);
        }
        static void bountyHunterSetTarget() {
            if (BountyHunter.bountyhunter == null || BountyHunter.bountyhunter != PlayerInCache.LocalPlayer.PlayerControl) return;
            BountyHunter.currentTarget = setTarget();
            setPlayerOutline(BountyHunter.currentTarget, BountyHunter.color);
        }
        static void trapperSetTarget() {
            if (Trapper.trapper == null || Trapper.trapper != PlayerInCache.LocalPlayer.PlayerControl) return;
            Trapper.currentTarget = setTarget();
            setPlayerOutline(Trapper.currentTarget, Trapper.color);
        }
        static void yinyangerSetTarget() {
            if (Yinyanger.yinyanger == null || Yinyanger.yinyanger != PlayerInCache.LocalPlayer.PlayerControl) return;
            Yinyanger.currentTarget = setTarget();
            setPlayerOutline(Yinyanger.currentTarget, Yinyanger.color);
        }
        static void challengerSetTarget() {
            if (Challenger.challenger == null || Challenger.challenger != PlayerInCache.LocalPlayer.PlayerControl) return;
            Challenger.currentTarget = setTarget();
            setPlayerOutline(Challenger.currentTarget, Challenger.color);
        }
        static void ninjaSetTarget() {
            if (Ninja.ninja == null || Ninja.ninja != PlayerInCache.LocalPlayer.PlayerControl) return;
            Ninja.currentTarget = setTarget();
            setPlayerOutline(Ninja.currentTarget, Ninja.color);
        }
        static void berserkerSetTarget() {
            if (Berserker.berserker == null || Berserker.berserker != PlayerInCache.LocalPlayer.PlayerControl) return;
            Berserker.currentTarget = setTarget();
            setPlayerOutline(Berserker.currentTarget, Berserker.color);
        }
        static void yandereSetTarget() {
            if (Yandere.yandere == null || Yandere.yandere != PlayerInCache.LocalPlayer.PlayerControl) return;

            if (!Yandere.rampageMode) {
                if (Yandere.target != null) {
                    var untargetables = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId != Yandere.target.PlayerId).ToList();
                    Yandere.currentTarget = setTarget(untargetablePlayers: untargetables);
                }
            } else {
                Yandere.currentTarget = setTarget();
            }
            setPlayerOutline(Yandere.currentTarget, Yandere.color);
        }
        static void strandedSetTarget() {
            if (Stranded.stranded == null || Stranded.stranded != PlayerInCache.LocalPlayer.PlayerControl) return;
            Stranded.currentTarget = setTarget();
            setPlayerOutline(Stranded.currentTarget, Stranded.color);
        }
        static void monjaSetTarget() {
            if (Monja.monja == null || Monja.monja != PlayerInCache.LocalPlayer.PlayerControl) return;
            Monja.currentTarget = setTarget();
            setPlayerOutline(Monja.currentTarget, Monja.color);
        }
        static void roleThiefSetTarget() {
            if (RoleThief.rolethief == null || RoleThief.rolethief != PlayerInCache.LocalPlayer.PlayerControl) return;
            RoleThief.currentTarget = setTarget();
            setPlayerOutline(RoleThief.currentTarget, RoleThief.color);
        }
        public static void pyromaniacSetTarget() {
            if (Pyromaniac.pyromaniac == null || Pyromaniac.pyromaniac != PlayerInCache.LocalPlayer.PlayerControl) return;
            List<PlayerControl> untargetables;
            if (Pyromaniac.sprayTarget != null)
                untargetables = PlayerControl.AllPlayerControls.ToArray().ToArray().Where(x => x.PlayerId != Pyromaniac.sprayTarget.PlayerId).ToList();
            else
                untargetables = Pyromaniac.sprayedPlayers;
            Pyromaniac.currentTarget = setTarget(untargetablePlayers: untargetables);
            if (Pyromaniac.currentTarget != null) setPlayerOutline(Pyromaniac.currentTarget, Pyromaniac.color);
        }
        static void devourerSetTarget() {
            if (Devourer.devourer == null || Devourer.devourer != PlayerInCache.LocalPlayer.PlayerControl) return;
            Devourer.currentTarget = setTarget();
            setPlayerOutline(Devourer.currentTarget, Devourer.color);
        }

        public static void poisonerSetTarget() {
            if (Poisoner.poisoner == null || Poisoner.poisoner != PlayerInCache.LocalPlayer.PlayerControl) return;
            List<PlayerControl> untargetables;
            if (Poisoner.poisonTarget != null)
                untargetables = PlayerControl.AllPlayerControls.ToArray().ToArray().Where(x => x.PlayerId != Poisoner.poisonTarget.PlayerId).ToList();
            else
                untargetables = Poisoner.poisonedPlayers;
            Poisoner.currentTarget = setTarget(untargetablePlayers: untargetables);
            if (Poisoner.currentTarget != null) setPlayerOutline(Poisoner.currentTarget, Poisoner.color);
        }
        static void puppeteerSetTarget() {
            if (Puppeteer.puppeteer == null || Puppeteer.puppeteer != PlayerInCache.LocalPlayer.PlayerControl) return;
            Puppeteer.currentTarget = setTarget();
            setPlayerOutline(Puppeteer.currentTarget, Puppeteer.color);
        }
        static void seekerSetTarget() {
            if (Seeker.seeker == null || Seeker.seeker != PlayerInCache.LocalPlayer.PlayerControl) return;
            Seeker.currentTarget = setTarget();
            setPlayerOutline(Seeker.currentTarget, Seeker.color);
        }
        public static void mechanicUpdate() {
            if (Mechanic.mechanic == null || PlayerInCache.LocalPlayer.PlayerControl != Mechanic.mechanic || Mechanic.mechanic.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.taskInfo(Mechanic.mechanic.Data);
            if (playerCompleted == Mechanic.rechargedTasks) {
                MessageWriter usedRechargeWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.MechanicUsedRepair, Hazel.SendOption.Reliable, -1);
                usedRechargeWriter.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(usedRechargeWriter);
                RPCProcedure.mechanicUsedRepair(2);
            }
        }
        static void sheriffSetTarget() {
            if (Sheriff.sheriff == null || Sheriff.sheriff != PlayerInCache.LocalPlayer.PlayerControl) return;
            Sheriff.currentTarget = setTarget();
            setPlayerOutline(Sheriff.currentTarget, Sheriff.color);
        }
        static void detectiveUpdateFootPrints() {
            if (Detective.detective == null || Detective.detective != PlayerInCache.LocalPlayer.PlayerControl) return;

            Detective.timer -= Time.fixedDeltaTime;
            if (Detective.timer <= 0f) {
                Detective.timer = Detective.footprintIntervall;
                foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                    if (player != null && player != PlayerInCache.LocalPlayer.PlayerControl && !player.Data.IsDead && !player.inVent && !PlayerInCache.LocalPlayer.PlayerControl.Data.IsDead) {
                        new Footprint(Detective.footprintDuration, Detective.anonymousFootprints, player);
                    }
                }
            }
        }
        public static void forensicSetTarget() {
            if (Forensic.forensic == null || Forensic.forensic != PlayerInCache.LocalPlayer.PlayerControl || Forensic.forensic.Data.IsDead || Forensic.deadBodies == null || ShipStatus.Instance?.AllVents == null) return;

            DeadPlayer target = null;
            Vector2 truePosition = PlayerInCache.LocalPlayer.PlayerControl.GetTruePosition();
            float closestDistance = float.MaxValue;
            float usableDistance = ShipStatus.Instance.AllVents.FirstOrDefault().UsableDistance;
            foreach ((DeadPlayer dp, Vector3 ps) in Forensic.deadBodies) {
                float distance = Vector2.Distance(ps, truePosition);
                if (distance <= usableDistance && distance < closestDistance) {
                    closestDistance = distance;
                    target = dp;
                }
            }
            Forensic.target = target;
        }
        
        static void squireSetTarget() {
            if (Squire.squire == null || Squire.squire != PlayerInCache.LocalPlayer.PlayerControl) return;
            Squire.currentTarget = setTarget();
            if (!Squire.usedShield) setPlayerOutline(Squire.currentTarget, Squire.color);
        }
        static void fortuneTellerSetTarget() {
            if (FortuneTeller.fortuneTeller == null || FortuneTeller.fortuneTeller != PlayerInCache.LocalPlayer.PlayerControl) return;
            FortuneTeller.currentTarget = setTarget();
            setPlayerOutline(FortuneTeller.currentTarget, FortuneTeller.color);
            if (FortuneTeller.currentTarget != null && FortuneTeller.revealedPlayers.Any(p => p.Data.PlayerId == FortuneTeller.currentTarget.Data.PlayerId)) FortuneTeller.currentTarget = null; // Remove target if already revealed
        }

        public static void fortuneTellerUpdate() {
            if (FortuneTeller.fortuneTeller == null || PlayerInCache.LocalPlayer.PlayerControl != FortuneTeller.fortuneTeller || FortuneTeller.fortuneTeller.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.taskInfo(FortuneTeller.fortuneTeller.Data);
            if (playerCompleted == FortuneTeller.rechargedTasks) {
                MessageWriter usedRechargeWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.FortuneTellerAbilityUses, Hazel.SendOption.Reliable, -1);
                usedRechargeWriter.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(usedRechargeWriter);
                RPCProcedure.fortuneTellerAbilityUses(2);
            }
        }
        public static void hackerUpdate() {
            if (Hacker.hacker == null || PlayerInCache.LocalPlayer.PlayerControl != Hacker.hacker || Hacker.hacker.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.taskInfo(Hacker.hacker.Data);
            if (playerCompleted == Hacker.rechargedTasks) {
                MessageWriter usedRechargeWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.HackerAbilityUses, Hazel.SendOption.Reliable, -1);
                usedRechargeWriter.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(usedRechargeWriter);
                RPCProcedure.hackerAbilityUses(2);
            }
        }
        static void sleuthSetTarget() {
            if (Sleuth.sleuth == null || Sleuth.sleuth != PlayerInCache.LocalPlayer.PlayerControl) return;
            Sleuth.currentTarget = setTarget();
            if (!Sleuth.usedLocate) setPlayerOutline(Sleuth.currentTarget, Sleuth.color);
        }
        static void sleuthUpdate() {
            // Handle player locate
            if (Sleuth.arrow?.arrow != null) {
                if (Sleuth.sleuth == null || PlayerInCache.LocalPlayer.PlayerControl != Sleuth.sleuth || Challenger.isDueling || Seeker.isMinigaming || isHappeningAnonymousComms) {
                    Sleuth.arrow.arrow.SetActive(false);
                    return;
                }

                if (Sleuth.sleuth != null && Sleuth.located != null && PlayerInCache.LocalPlayer.PlayerControl == Sleuth.sleuth && !Sleuth.sleuth.Data.IsDead) {
                    Sleuth.timeUntilUpdate -= Time.fixedDeltaTime;

                    if (Sleuth.timeUntilUpdate <= 0f) {
                        bool locatedOnMap = !Sleuth.sleuth.Data.IsDead;
                        Vector3 position = Sleuth.located.transform.position;
                        if (!locatedOnMap) { // Check for dead body
                            DeadBody body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Sleuth.located.PlayerId);
                            if (body != null) {
                                locatedOnMap = true;
                                position = body.transform.position;
                            }
                        }

                        Sleuth.arrow.Update(position);
                        Sleuth.arrow.arrow.SetActive(locatedOnMap);
                        Sleuth.timeUntilUpdate = Sleuth.updateIntervall;
                    }
                    else {
                        Sleuth.arrow.Update();
                    }
                }
            }

            // Handle corpses locate
            if (Sleuth.sleuth != null && Sleuth.sleuth == PlayerInCache.LocalPlayer.PlayerControl && Sleuth.corpsesPathfindTimer >= 0f && !Sleuth.sleuth.Data.IsDead) {
                bool arrowsCountChanged = Sleuth.localArrows.Count != Sleuth.deadBodyPositions.Count();
                int index = 0;

                if (arrowsCountChanged) {
                    foreach (Arrow arrow in Sleuth.localArrows) UnityEngine.Object.Destroy(arrow.arrow);
                    Sleuth.localArrows = new List<Arrow>();
                }
                foreach (Vector3 position in Sleuth.deadBodyPositions) {
                    if (arrowsCountChanged) {
                        Sleuth.localArrows.Add(new Arrow(Sleuth.color));
                        Sleuth.localArrows[index].arrow.SetActive(true);
                    }
                    if (Sleuth.localArrows[index] != null) Sleuth.localArrows[index].Update(position);
                    index++;
                }
            }
            else if (Sleuth.localArrows.Count > 0) {
                foreach (Arrow arrow in Sleuth.localArrows) UnityEngine.Object.Destroy(arrow.arrow);
                Sleuth.localArrows = new List<Arrow>();
            }

            // Handle closest player locate
            if (Sleuth.sleuth == PlayerInCache.LocalPlayer.PlayerControl && !Sleuth.sleuth.Data.IsDead) {
                if (Sleuth.timer >= 0f) {

                    PlayerControl result = null;
                    float num = 10f;

                    Vector2 truePosition = Sleuth.sleuth.GetTruePosition();
                    Il2CppSystem.Collections.Generic.List<NetworkedPlayerInfo> allPlayers = GameData.Instance.AllPlayers;
                    for (int i = 0; i < allPlayers.Count; i++) {
                        NetworkedPlayerInfo playerInfo = allPlayers[i];
                        if (!playerInfo.Disconnected && playerInfo.PlayerId != Sleuth.sleuth.PlayerId && !playerInfo.IsDead) {
                            PlayerControl @object = playerInfo.Object;
                            if (@object && @object.Collider.enabled) {
                                Vector2 vector = @object.GetTruePosition() - truePosition;
                                float magnitude = vector.magnitude;
                                if (magnitude <= num) {
                                    result = @object;
                                    num = magnitude;
                                }
                            }
                        }
                    }

                    if (result != null && Vector2.Distance(result.transform.position, Sleuth.sleuth.transform.position) < 10f) {
                        Sleuth.arrowWho.Update(result.transform.position, Palette.PlayerColors[result.Data.DefaultOutfit.ColorId]);
                        Sleuth.arrowWho.arrow.SetActive(true);
                    }
                    else {
                        Sleuth.arrowWho.arrow.SetActive(false);
                    }
                }
                else {
                    Sleuth.arrowWho.arrow.SetActive(false);
                }
            }
        }
        static void finkUpdate() {

            if (Fink.fink == null || Fink.fink.Data.IsDead || Fink.localArrows == null) return;

            foreach (Arrow arrow in Fink.localArrows) arrow.arrow.SetActive(false);

            var (playerCompleted, playerTotal) = TasksHandler.taskInfo(Fink.fink.Data);
            int numberOfTasks = playerTotal - playerCompleted;
            if (numberOfTasks <= Fink.taskCountForImpostors) {
                if (numberOfTasks <= Fink.taskCountForImpostors && PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor) {
                    if (Fink.localArrows.Count == 0) {
                        Fink.localArrows.Add(new Arrow(Fink.color));
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.bountyExilerTarget, false, 5f);
                    }
                    if (Fink.localArrows.Count != 0 && Fink.localArrows[0] != null) {
                        Fink.localArrows[0].arrow.SetActive(true);
                        Fink.localArrows[0].Update(Fink.fink.transform.position);
                    }
                }
            }
            
            if (PlayerInCache.LocalPlayer.PlayerControl == Fink.fink && numberOfTasks == 0 && !Challenger.isDueling && !Seeker.isMinigaming && !isHappeningAnonymousComms) {
                int arrowIndex = 0;
                foreach (PlayerControl p in PlayerInCache.AllPlayers) {
                    bool arrowForImp = p.Data.Role.IsImpostor;

                    if (!p.Data.IsDead && arrowForImp) {
                        if (arrowIndex >= Fink.localArrows.Count) {
                            Fink.localArrows.Add(new Arrow(Color.red));
                        }
                        if (arrowIndex < Fink.localArrows.Count && Fink.localArrows[arrowIndex] != null) {
                            Fink.localArrows[arrowIndex].arrow.SetActive(true);
                            Fink.localArrows[arrowIndex].Update(p.transform.position);
                        }
                        arrowIndex++;
                    }
                }
            }
        }
        public static void welderSetTarget() {
            if (Welder.welder == null || Welder.welder != PlayerInCache.LocalPlayer.PlayerControl || ShipStatus.Instance == null || ShipStatus.Instance.AllVents == null) return;

            Vent target = null;
            Vector2 truePosition = PlayerInCache.LocalPlayer.PlayerControl.GetTruePosition();
            float closestDistance = float.MaxValue;
            for (int i = 0; i < ShipStatus.Instance.AllVents.Length; i++) {
                Vent vent = ShipStatus.Instance.AllVents[i];
                if (vent.gameObject.name.StartsWith("Hat_") || vent.gameObject.name.StartsWith("SealedVent_") || vent.gameObject.name.StartsWith("FutureSealedVent_")) continue;
                float distance = Vector2.Distance(vent.transform.position, truePosition);
                if (distance <= vent.UsableDistance && distance < closestDistance) {
                    closestDistance = distance;
                    target = vent;
                }
            }
            Welder.ventTarget = target;
        }
        static void spiritualistRevivedPlayerSetTarget() {
            if (Spiritualist.revivedPlayer == null || Spiritualist.revivedPlayer != PlayerInCache.LocalPlayer.PlayerControl) return;
            var untargetables = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId != Spiritualist.revivedPlayerKiller.PlayerId).ToList();
            Spiritualist.revivedPlayerTarget = setTarget(untargetablePlayers: untargetables); 
            setPlayerOutline(Spiritualist.revivedPlayerTarget, Spiritualist.color);
        }
        static void necromancerUpdate() {            
            if (Necromancer.revivedPlayer != null) {
                if (!Necromancer.revivedPlayer.Data.IsDead && (PlayerInCache.LocalPlayer.PlayerControl.Data.Role.IsImpostor || PlayerInCache.LocalPlayer.PlayerControl == Renegade.renegade || PlayerInCache.LocalPlayer.PlayerControl == Minion.minion || PlayerInCache.LocalPlayer.PlayerControl == BountyHunter.bountyhunter || PlayerInCache.LocalPlayer.PlayerControl == Trapper.trapper || PlayerInCache.LocalPlayer.PlayerControl == Yinyanger.yinyanger || PlayerInCache.LocalPlayer.PlayerControl == Challenger.challenger || PlayerInCache.LocalPlayer.PlayerControl == Ninja.ninja || PlayerInCache.LocalPlayer.PlayerControl == Berserker.berserker || PlayerInCache.LocalPlayer.PlayerControl == Yandere.yandere || PlayerInCache.LocalPlayer.PlayerControl == Stranded.stranded || PlayerInCache.LocalPlayer.PlayerControl == Monja.monja)) {
                    if (Necromancer.localNecromancerArrows.Count == 0) Necromancer.localNecromancerArrows.Add(new Arrow(Color.green));
                    if (Necromancer.localNecromancerArrows.Count != 0 && Necromancer.localNecromancerArrows[0] != null) {
                        Necromancer.localNecromancerArrows[0].arrow.SetActive(true);
                        Necromancer.localNecromancerArrows[0].Update(Necromancer.revivedPlayer.transform.position);
                    }
                }
                else {
                    if (Necromancer.localNecromancerArrows.Count != 0) {
                        Necromancer.localNecromancerArrows[0].arrow.SetActive(false);
                    }
                }
            }
        }
        public static void cowardUpdate() {
            if (Coward.coward == null || PlayerInCache.LocalPlayer.PlayerControl != Coward.coward || Coward.coward.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.taskInfo(Coward.coward.Data);
            if (playerCompleted == Coward.rechargedTasks) {
                MessageWriter usedRechargeWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.CowardUsedCall, Hazel.SendOption.Reliable, -1);
                usedRechargeWriter.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(usedRechargeWriter);
                RPCProcedure.cowardUsedCall(2);
            }
        }
        public static void vigilantUpdate() {
            if (Vigilant.vigilant == null || PlayerInCache.LocalPlayer.PlayerControl != Vigilant.vigilant || Vigilant.vigilant.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.taskInfo(Vigilant.vigilant.Data);
            if (playerCompleted == Vigilant.rechargedTasks) {
                MessageWriter usedRechargeWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.VigilantAbilityUses, Hazel.SendOption.Reliable, -1);
                usedRechargeWriter.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(usedRechargeWriter);
                RPCProcedure.vigilantAbilityUses(2);
            }
        }
        static void hunterSetTarget() {
            if (Hunter.hunter == null || Hunter.hunter != PlayerInCache.LocalPlayer.PlayerControl) return;
            Hunter.currentTarget = setTarget();
            if (!Hunter.usedHunted) setPlayerOutline(Hunter.currentTarget, Hunter.color);
        }
        static void jinxSetTarget() {
            if (Jinx.jinx == null || Jinx.jinx != PlayerInCache.LocalPlayer.PlayerControl) return;
            Jinx.target = setTarget();
            setPlayerOutline(Jinx.target, Jinx.color);
            if (Jinx.target != null && Jinx.jinxedList.Any(p => p.Data.PlayerId == Jinx.target.Data.PlayerId)) Jinx.target = null; // Remove target if already Jinxed and didn't trigger the jinx
        }
        static void taskMasterSetTarget() {
            if (TaskMaster.taskMaster == null || TaskMaster.taskMaster != PlayerInCache.LocalPlayer.PlayerControl || TaskMaster.rewardType != 1) return;
            TaskMaster.currentTarget = setTarget();
            setPlayerOutline(TaskMaster.currentTarget, TaskMaster.color);
        }       
        static void jailerSetTarget() {
            if (Jailer.jailer == null || Jailer.jailer != PlayerInCache.LocalPlayer.PlayerControl) return;
            Jailer.currentTarget = setTarget();
            if (!Jailer.usedJail) setPlayerOutline(Jailer.currentTarget, Jailer.color);
        }
        static void theChosenOneUpdate() {
            if (Modifiers.theChosenOne == null || Modifiers.theChosenOne != PlayerInCache.LocalPlayer.PlayerControl) return;

            // TheChosenOne report
            if (Modifiers.theChosenOne.Data.IsDead && !Modifiers.chosenOneReported) {
                Modifiers.chosenOneReportDelay -= Time.fixedDeltaTime;
                DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == Modifiers.theChosenOne.PlayerId)?.FirstOrDefault();
                if (deadPlayer.killerIfExisting != null && Modifiers.chosenOneReportDelay <= 0f) {
                    Modifiers.chosenOneReported = true;

                    if (!Monja.awakened) {
                        // Bomberman bomb reset when report the chosen one
                        if (Bomberman.bomberman != null && Bomberman.activeBomb == true) {
                            MessageWriter bombwriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.FixBomb, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(bombwriter);
                            RPCProcedure.fixBomb();
                        }

                        // Remove manipulated
                        if (Manipulator.manipulatedVictim != null) {
                            Manipulator.manipulatedVictim = null;
                            Manipulator.manipulatedVictimTimer = 21f;
                        }

                        // Stop Performer Music
                        if (Modifiers.performer != null && Modifiers.performer.Data.IsDead && !Modifiers.performerReported) {
                            MessageWriter performerwriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PerformerIsReported, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(performerwriter); 
                            RPCProcedure.performerIsReported(0);
                        }

                        // Manually murder the Spiritualist's revived player
                        if (Spiritualist.revivedPlayer != null && !Spiritualist.revivedPlayer.Data.IsDead) {
                            MessageWriter murderRevivedPlayer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.MurderSpiritualistRevivedPlayer, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(murderRevivedPlayer);
                            RPCProcedure.murderSpiritualistRevivedPlayer();
                        }

                        Helpers.handleDemonBiteOnBodyReport(); // Manually call Demon handling, since the CmdReportDeadBody Prefix won't be called
                        Helpers.handleMedusaPetrifyOnBodyReport(); // Manually call Medusa handling, since the CmdReportDeadBody Prefix won't be called
                        if (Devourer.devourer != null && Devourer.devourer.PlayerId != Modifiers.theChosenOne.PlayerId) {
                            Helpers.handleEatenPlayersOnBodyReport(); // Manually call Devourer devour if he isnt the chosenone, since the CmdReportDeadBody Prefix won't be called
                        }
                        RPCProcedure.uncheckedCmdReportDeadBody(deadPlayer.killerIfExisting.PlayerId, Modifiers.theChosenOne.PlayerId);

                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedCmdReportDeadBody, Hazel.SendOption.Reliable, -1);
                        writer.Write(deadPlayer.killerIfExisting.PlayerId);
                        writer.Write(Modifiers.theChosenOne.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);

                        MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                        writermusic.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                        RPCProcedure.changeMusic(1);
                    }
                }
            }
        }
        static void performerUpdate() {
            if (Modifiers.performer != null) {
                if (Modifiers.performerDuration > 0 && Modifiers.performer.Data.IsDead && !Modifiers.performerReported && (PlayerInCache.LocalPlayer.PlayerControl != Modifiers.performer && PlayerInCache.LocalPlayer.PlayerControl != Spiritualist.spiritualist)) {
                    if (Modifiers.performerLocalPerformerArrows.Count == 0) Modifiers.performerLocalPerformerArrows.Add(new Arrow(Modifiers.color));
                    if (Modifiers.performerLocalPerformerArrows.Count != 0 && Modifiers.performerLocalPerformerArrows[0] != null) {
                        var bodyPerformer = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Modifiers.performer.PlayerId);
                        if (bodyPerformer != null) {
                            Modifiers.performerLocalPerformerArrows[0].arrow.SetActive(true);
                            Modifiers.performerLocalPerformerArrows[0].Update(bodyPerformer.transform.position);
                        }
                    }
                }
                else {
                    if (Modifiers.performerLocalPerformerArrows.Count != 0) {
                        Modifiers.performerLocalPerformerArrows[0].arrow.SetActive(false);
                    }
                }

                // Upon performer duration, stop the music and play bomb music if there's a bomb or normal task music
                if (Modifiers.performer.Data.IsDead && Modifiers.performerDuration <= 0 && !Modifiers.performerMusicStop && !Modifiers.performerReported && (PlayerInCache.LocalPlayer.PlayerControl != Spiritualist.spiritualist)) {
                    Modifiers.performerMusicStop = true;
                    SoundManager.Instance.StopSound(CustomMain.customAssets.performerMusic);
                    if (Bomberman.activeBomb) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.bombermanBombMusic, true, 75f);
                    }
                    else {
                        RPCProcedure.changeMusic(2);
                    }
                }
            }
        }
        static void paintballTrail() {
            if (!Modifiers.active.Any()) return;
            foreach (KeyValuePair<byte, float> entry in new Dictionary<byte, float>(Modifiers.active)) {
                PlayerControl player = Helpers.playerById(entry.Key);
                PlayerControl killerPlayer = Helpers.playerById(Modifiers.paintballKillerMap[player.PlayerId]);

                Modifiers.active[entry.Key] = entry.Value - Time.fixedDeltaTime;
                if (entry.Value <= 0 || player.Data.IsDead) {
                    Modifiers.active.Remove(entry.Key);
                    continue;  // Stop creating paint if timer reaches 0, the killer died or is in vent
                }
                // Don't create paint inside vents
                if (!player.inVent) {
                    new PaintballTrail(player, killerPlayer);
                }
            }
        }

        private static void gamemodeIndividualRevivingTargetStatus(List<PlayerControl> untargetablePlayers, PlayerControl player, bool isReviving) {
            if (player == null) return;

            if (isReviving) {
                if (!untargetablePlayers.Contains(player)) {
                    untargetablePlayers.Add(player);
                }
            }
            else {
                untargetablePlayers.Remove(player);
            }
        }
        
        private static void gamemodeTeamsRevivingTargetStatus(PlayerControl player, bool isReviving, params List<PlayerControl>[] playerLists) {
            if (player == null) return;

            foreach (var list in playerLists) {
                if (isReviving) {
                    if (!list.Contains(player)) {
                        list.Add(player);
                    }
                }
                else {
                    list.Remove(player);
                }
            }
        }
        static void captureTheFlagSetTarget() {

            if (gameType != 2)
                return;

            var untargetableAllPlayers = new List<PlayerControl>();

            var untargetableRedPlayers = new List<PlayerControl>();
            foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                untargetableRedPlayers.Add(player);
            }

            var untargetableBluePlayers = new List<PlayerControl>();
            foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                untargetableBluePlayers.Add(player);
            }

            // Prevent killing reviving players
            foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                gamemodeTeamsRevivingTargetStatus(player, CaptureTheFlag.revivingPlayers.Contains(player), untargetableBluePlayers, untargetableAllPlayers);
            }
            foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                gamemodeTeamsRevivingTargetStatus(player, CaptureTheFlag.revivingPlayers.Contains(player), untargetableRedPlayers, untargetableAllPlayers);
            }
            gamemodeTeamsRevivingTargetStatus(CaptureTheFlag.stealerPlayer, CaptureTheFlag.revivingPlayers.Contains(CaptureTheFlag.stealerPlayer), untargetableRedPlayers, untargetableBluePlayers);

            for (int i = 0; i < CaptureTheFlag.redteamFlag.Count; i++) {
                if (CaptureTheFlag.redteamFlag[i] != null && CaptureTheFlag.redteamFlag[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                CaptureTheFlag.redTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableRedPlayers);
                setPlayerOutline(CaptureTheFlag.redTeamCurrentargets[i], Palette.ImpostorRed);

                break;
            }

            for (int i = 0; i < CaptureTheFlag.blueteamFlag.Count; i++) {
                if (CaptureTheFlag.blueteamFlag[i] != null && CaptureTheFlag.blueteamFlag[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                CaptureTheFlag.blueTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableBluePlayers);
                setPlayerOutline(CaptureTheFlag.blueTeamCurrentargets[i], Color.blue);

                break;
            }

            if (CaptureTheFlag.stealerPlayer != null && CaptureTheFlag.stealerPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                CaptureTheFlag.stealerPlayercurrentTarget = setTarget(untargetablePlayers: untargetableAllPlayers);
                setPlayerOutline(CaptureTheFlag.stealerPlayercurrentTarget, Color.grey);
            }
        }

        static void policeandThiefSetTarget() {

            if (gameType != 3)
                return;

            var untargetablePolice = new List<PlayerControl>();
            foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                untargetablePolice.Add(player);
            }

            // Prevent killing reviving players
            foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                gamemodeIndividualRevivingTargetStatus(untargetablePolice, player, PoliceAndThief.revivingPlayers.Contains(player));
            }

            for (int i = 0; i < PoliceAndThief.policeTeam.Count; i++) {
                if (PoliceAndThief.policeTeam[i] != null && PoliceAndThief.policeTeam[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                PoliceAndThief.policeTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetablePolice);
                setPlayerOutline(PoliceAndThief.policeTeamCurrentargets[i], Cheater.color);

                break;
            }

            var untargetableThiefs = new List<PlayerControl>();
            foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                untargetableThiefs.Add(player);
            }

            foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                gamemodeIndividualRevivingTargetStatus(untargetableThiefs, player, PoliceAndThief.revivingPlayers.Contains(player));
            }

            for (int i = 0; i < PoliceAndThief.thiefTeam.Count; i++) {
                if (PoliceAndThief.thiefTeam[i] != null && PoliceAndThief.thiefTeam[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                PoliceAndThief.thiefTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefTeamCurrentargets[i], Mechanic.color);

                break;
            }
        }

        static void kingOfTheHillSetTarget() {

            if (gameType != 4)
                return;

            var untargetableAllPlayers = new List<PlayerControl>();

            var untargetableGreenPlayers = new List<PlayerControl>();
            foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                untargetableGreenPlayers.Add(player);
            }

            var untargetableYellowPlayers = new List<PlayerControl>();
            foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                untargetableYellowPlayers.Add(player);
            }

            // Prevent killing reviving players
            foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                gamemodeTeamsRevivingTargetStatus(player, KingOfTheHill.revivingPlayers.Contains(player), untargetableYellowPlayers, untargetableAllPlayers);
            }
            foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                gamemodeTeamsRevivingTargetStatus(player, KingOfTheHill.revivingPlayers.Contains(player), untargetableGreenPlayers, untargetableAllPlayers);
            }
            gamemodeTeamsRevivingTargetStatus(KingOfTheHill.usurperPlayer, KingOfTheHill.revivingPlayers.Contains(KingOfTheHill.usurperPlayer), untargetableGreenPlayers, untargetableYellowPlayers);

            for (int i = 0; i < KingOfTheHill.greenTeam.Count; i++) {
                if (KingOfTheHill.greenTeam[i] != null && KingOfTheHill.greenTeam[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                KingOfTheHill.greenTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(KingOfTheHill.greenTeamCurrentargets[i], Color.green);

                break;
            }

            for (int i = 0; i < KingOfTheHill.yellowTeam.Count; i++) {
                if (KingOfTheHill.yellowTeam[i] != null && KingOfTheHill.yellowTeam[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                KingOfTheHill.yellowTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableYellowPlayers);
                setPlayerOutline(KingOfTheHill.yellowTeamCurrentargets[i], Color.yellow);

                break;
            }

            if (KingOfTheHill.usurperPlayer != null && KingOfTheHill.usurperPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                KingOfTheHill.usurperPlayercurrentTarget = setTarget(untargetablePlayers: untargetableAllPlayers);
                setPlayerOutline(KingOfTheHill.usurperPlayercurrentTarget, Color.grey);
            }
        }

        static void hotPotatoSetTarget() {

            if (gameType != 5)
                return;

            if (HotPotato.hotPotatoPlayer != null && HotPotato.hotPotatoPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                HotPotato.hotPotatoPlayerCurrentTarget = setTarget();
                setPlayerOutline(HotPotato.hotPotatoPlayerCurrentTarget, Color.grey);
            }
        }

        static void zombieLaboratorySetTarget() {

            if (gameType != 6)
                return;

            var untargetableSurvivorsPlayers = new List<PlayerControl>();

            foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                untargetableSurvivorsPlayers.Add(player);
            }

            var untargetableZombiePlayers = new List<PlayerControl>();

            foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                untargetableZombiePlayers.Add(player);
            }

            if (ZombieLaboratory.nursePlayer != null) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.nursePlayer);
                untargetableZombiePlayers.Add(ZombieLaboratory.nursePlayer);
            }

            // Prevent killing reviving players
            foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                if (player == ZombieLaboratory.nursePlayer) continue; // skip Nurse
                gamemodeIndividualRevivingTargetStatus(untargetableZombiePlayers, player, ZombieLaboratory.revivingPlayers.Contains(player));
            }
            foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                gamemodeIndividualRevivingTargetStatus(untargetableSurvivorsPlayers, player, ZombieLaboratory.revivingPlayers.Contains(player));
            }

            if (ZombieLaboratory.nursePlayer != null && ZombieLaboratory.nursePlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                ZombieLaboratory.nursePlayercurrentTarget = setTarget();
                setPlayerOutline(ZombieLaboratory.nursePlayercurrentTarget, Locksmith.color);
            }

            for (int i = 0; i < ZombieLaboratory.survivorTeam.Count; i++) {
                if (ZombieLaboratory.survivorTeam[i] != null && ZombieLaboratory.survivorTeam[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                ZombieLaboratory.survivorTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorTeamCurrentargets[i], Color.green);

                break;
            }

            for (int i = 0; i < ZombieLaboratory.zombieTeam.Count; i++) {
                if (ZombieLaboratory.zombieTeam[i] != null && ZombieLaboratory.zombieTeam[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                ZombieLaboratory.zombieTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombieTeamCurrentargets[i], Sheriff.color);

                break;
            }
        }

        static void monjaFestivalSetTarget() {

            if (gameType != 8)
                return;

            var untargetableAllPlayers = new List<PlayerControl>();

            var untargetableGreenPlayers = new List<PlayerControl>();
            foreach (PlayerControl player in MonjaFestival.greenTeam) {
                untargetableGreenPlayers.Add(player);
            }

            var untargetableCyanPlayers = new List<PlayerControl>();
            foreach (PlayerControl player in MonjaFestival.cyanTeam) {
                untargetableCyanPlayers.Add(player);
            }

            // Prevent killing reviving players
            foreach (PlayerControl player in MonjaFestival.cyanTeam) {
                gamemodeIndividualRevivingTargetStatus(untargetableGreenPlayers, player, MonjaFestival.revivingPlayers.Contains(player));
            }
            foreach (PlayerControl player in MonjaFestival.greenTeam) {
                gamemodeIndividualRevivingTargetStatus(untargetableCyanPlayers, player, MonjaFestival.revivingPlayers.Contains(player));
            }
            gamemodeTeamsRevivingTargetStatus(MonjaFestival.bigMonjaPlayer, MonjaFestival.revivingPlayers.Contains(MonjaFestival.bigMonjaPlayer), untargetableGreenPlayers, untargetableCyanPlayers);

            for (int i = 0; i < MonjaFestival.greenTeam.Count; i++) {
                if (MonjaFestival.greenTeam[i] != null && MonjaFestival.greenTeam[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                MonjaFestival.greenTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(MonjaFestival.greenTeamCurrentargets[i], Color.green);

                break;
            }

            for (int i = 0; i < MonjaFestival.cyanTeam.Count; i++) {
                if (MonjaFestival.cyanTeam[i] != null && MonjaFestival.cyanTeam[i] != PlayerInCache.LocalPlayer.PlayerControl) continue; // skip if not the local player

                MonjaFestival.cyanTeamCurrentargets[i] = setTarget(untargetablePlayers: untargetableCyanPlayers);
                setPlayerOutline(MonjaFestival.cyanTeamCurrentargets[i], Color.cyan);

                break;
            }

            if (MonjaFestival.bigMonjaPlayer != null && MonjaFestival.bigMonjaPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                MonjaFestival.bigMonjaPlayercurrentTarget = setTarget(untargetablePlayers: untargetableAllPlayers);
                setPlayerOutline(MonjaFestival.bigMonjaPlayercurrentTarget, Color.grey);
            }
        }

        public static void Postfix(PlayerControl __instance) {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started || GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return;

            if (PlayerInCache.LocalPlayer.PlayerControl == __instance) {
                // Update player outlines
                setBasePlayerOutlines();

                // Update Role Description
                Helpers.refreshRoleDescription(__instance);

                // Show roles for dead players on meeting
                ghostsSeePlayerRoles();

                // Ventcolor
                ventColorUpdate();

                // Impostor
                impostorSetTarget();

                // Mimic and Painter
                mimicSetTarget();
                mimicAndPainterUpdate();

                // Demon
                demonSetTarget();
                Nun.UpdateAll();

                // Manipulator
                manipulatorSetTarget();
                manipulatedVictimSetTarget();

                // Sorcerer
                sorcererSetTarget();

                // Medusa
                medusaSetTarget();

                // Librarian
                librarianSetTarget();

                // Renegade
                renegadeSetTarget();

                // Minion
                minionSetTarget();

                // BountyHunter
                bountyHunterSetTarget();

                // Trapper
                trapperSetTarget();

                // Yinyanger
                yinyangerSetTarget();

                // Challenger
                challengerSetTarget();

                // Ninja
                ninjaSetTarget();

                // Berserker
                berserkerSetTarget();

                // Yandere
                yandereSetTarget();

                // Stranded
                strandedSetTarget();

                // Monja
                monjaSetTarget();

                // RoleThief
                roleThiefSetTarget();

                // Pyromaniac
                pyromaniacSetTarget();

                // Devourer
                devourerSetTarget();

                // Poisoner
                poisonerSetTarget();

                // Puppeteer
                puppeteerSetTarget();

                // Seeker
                seekerSetTarget();

                // Mechanic
                mechanicUpdate();
                
                // Sheriff
                sheriffSetTarget();

                // Detective
                detectiveUpdateFootPrints();

                // Forensic
                forensicSetTarget();

                // Squire
                squireSetTarget();

                // FortuneTeller
                fortuneTellerSetTarget();
                fortuneTellerUpdate();

                // Hacker
                hackerUpdate();

                // Sleuth
                sleuthSetTarget();
                sleuthUpdate();

                // Fink
                finkUpdate();

                // Welder
                welderSetTarget();

                // Spiritualist Update
                spiritualistRevivedPlayerSetTarget();
                
                // Necromancer Update
                necromancerUpdate();

                // Coward
                cowardUpdate();
                
                // Vigilant
                vigilantUpdate();

                // Hunter
                hunterSetTarget();

                // Jinx
                jinxSetTarget();

                // Task Master
                taskMasterSetTarget();
                
                // Jailer
                jailerSetTarget();

                // TheChosenOne
                theChosenOneUpdate();

                // Performer Update
                performerUpdate();

                // Paintball
                paintballTrail();

                // Capture the flag update
                captureTheFlagSetTarget();

                // Police and Thief update
                policeandThiefSetTarget();

                // King of the hill update
                kingOfTheHillSetTarget();

                // Hot Potato update
                hotPotatoSetTarget();

                // ZombieLaboratory
                zombieLaboratorySetTarget();

                // Monja Festival
                monjaFestivalSetTarget();
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ReportDeadBody))]
    class PlayerControlCmdReportDeadBodyPatch {
        public static void Prefix(PlayerControl __instance) {
            // Bomberman bomb reset when report body
            if (Bomberman.bomberman != null && Bomberman.activeBomb == true) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.FixBomb, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.fixBomb();
            }

            //If music option is enabled and the player who used emergency button was not a bitten player
            if (PlayerInCache.LocalPlayer.PlayerControl != Demon.bitten || Demon.bitten == null) {
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(1);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(1);
            }

            // Murder the bitten player before the meeting starts or reset the bitten player
            Helpers.handleDemonBiteOnBodyReport();

            // Murder the petrify players before the meeting starts
            Helpers.handleMedusaPetrifyOnBodyReport();

            // Murder the eaten players before the meating starts
            Helpers.handleEatenPlayersOnBodyReport();

            // Remove manipulated
            if (Manipulator.manipulatedVictim != null) {
                Manipulator.manipulatedVictim = null;
                Manipulator.manipulatedVictimTimer = 21f;
            }

            // Murder the Spiritualist's revived player if someone reports a body or call emergency
            if (Spiritualist.revivedPlayer != null && !Spiritualist.revivedPlayer.Data.IsDead) {
                MessageWriter murderRevivedPlayer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.MurderSpiritualistRevivedPlayer, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(murderRevivedPlayer);
                RPCProcedure.murderSpiritualistRevivedPlayer();
            }

            // Performer isreported
            if (Modifiers.performer != null && Modifiers.performer.Data.IsDead && !Modifiers.performerReported) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PerformerIsReported, Hazel.SendOption.Reliable, -1);
                writer.Write(0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.performerIsReported(0);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerInCache.LocalPlayer.PlayerControl.ReportDeadBody))]
    class BodyReportPatch
    {
        static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] NetworkedPlayerInfo target) {
            // Forensic report
            bool isForensicReport = Forensic.forensic != null && Forensic.forensic == PlayerInCache.LocalPlayer.PlayerControl && __instance.PlayerId == Forensic.forensic.PlayerId;
            if (isForensicReport) {
                
                DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();

                if (deadPlayer != null && deadPlayer.killerIfExisting != null) {
                    float timeSinceDeath = ((float)(DateTime.UtcNow - deadPlayer.timeOfDeath).TotalMilliseconds);
                    string msg = "";


                    if (deadPlayer.player == RoleThief.rolethief && deadPlayer.killerIfExisting.Data.PlayerName == RoleThief.rolethief.Data.PlayerName) {
                        msg = $"{Language.helpersTexts[2]} ({Language.roleInfoRoleNames[27]}): {Language.playerControlTexts[0]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                    }
                    else if (deadPlayer.player == BountyHunter.bountyhunter && deadPlayer.killerIfExisting.Data.PlayerName == BountyHunter.bountyhunter.Data.PlayerName) {
                        msg = $"{Language.helpersTexts[2]} ({Language.roleInfoRoleNames[17]}): {Language.playerControlTexts[0]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                    }
                    else if (deadPlayer.player == Modifiers.lover1 && deadPlayer.killerIfExisting.Data.PlayerName == Modifiers.lover1.Data.PlayerName || deadPlayer.player == Modifiers.lover2 && deadPlayer.killerIfExisting.Data.PlayerName == Modifiers.lover2.Data.PlayerName) {
                        msg = $"{Language.helpersTexts[2]} ({Language.roleInfoRoleNames[72]}): {Language.playerControlTexts[0]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                    }
                    else if (deadPlayer.player == Sheriff.sheriff && deadPlayer.killerIfExisting.Data.PlayerName == Sheriff.sheriff.Data.PlayerName) {
                        msg = $"{Language.helpersTexts[2]} ({Language.roleInfoRoleNames[38]}): {Language.playerControlTexts[0]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                    }
                    else if (timeSinceDeath < Forensic.reportNameDuration * 1000) {
                        msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[1]} {deadPlayer.killerIfExisting.Data.PlayerName}! ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                    }
                    else if (timeSinceDeath < Forensic.reportColorDuration * 1000) {
                        var typeOfColor = Helpers.isLighterColor(deadPlayer.killerIfExisting.Data.DefaultOutfit.ColorId) ? Language.playerControlTexts[2] : Language.playerControlTexts[3];
                        msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[4]} {typeOfColor} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                    }
                    else if (timeSinceDeath < Forensic.reportClueDuration * 1000) {
                        int randomClue = rnd.Next(1, 5);
                        switch (randomClue) {
                            case 1:
                                if (deadPlayer.killerIfExisting.Data.DefaultOutfit.HatId != null) {
                                    msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[5]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                }
                                else {
                                    msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[6]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                }
                                break;
                            case 2:
                                if (deadPlayer.killerIfExisting.Data.DefaultOutfit.SkinId != null) {
                                    msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[7]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                }
                                else {
                                    msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[8]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                }
                                break;
                            case 3:
                                if (deadPlayer.killerIfExisting.Data.DefaultOutfit.PetId != null) {
                                    msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[9]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                }
                                else {
                                    msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[10]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                }
                                break;
                            case 4:
                                if (deadPlayer.killerIfExisting.Data.DefaultOutfit.VisorId != null) {
                                    msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[11]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                }
                                else {
                                    msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[12]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                }
                                break;
                        }
                    }
                    else {
                        msg = $"{Language.helpersTexts[2]}: {Language.playerControlTexts[13]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                    }
                    

                    if (!string.IsNullOrWhiteSpace(msg)) {
                        if (AmongUsClient.Instance.AmClient && FastDestroyableSingleton<HudManager>.Instance) {
                            FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerInCache.LocalPlayer.PlayerControl, msg);
                        }
                        if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0) {
                            FastDestroyableSingleton<Assets.CoreScripts.UnityTelemetry>.Instance.SendWho();
                        }
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class MurderPlayerPatch
    {
        public static bool resetToCrewmate = false;
        public static bool resetToDead = false;

        public static void Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target) {
            // Allow everyone to murder players
            resetToCrewmate = !__instance.Data.Role.IsImpostor;
            resetToDead = __instance.Data.IsDead;
            __instance.Data.Role.TeamType = RoleTeamTypes.Impostor;
            __instance.Data.IsDead = false;
        }

        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target) {
            // Collect dead player info
            DeadPlayer deadPlayer = new DeadPlayer(target, DateTime.UtcNow, DeathReason.Kill, __instance);
            GameHistory.deadPlayers.Add(deadPlayer);

            // Reset killer to crewmate if resetToCrewmate
            if (resetToCrewmate) __instance.Data.Role.TeamType = RoleTeamTypes.Crewmate;
            if (resetToDead) __instance.Data.IsDead = true;

            switch (gameType) {
                case 0:
                case 1:
                    if (PlayerInCache.LocalPlayer.PlayerControl == target && GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
                        HudManager.Instance.StartCoroutine(Effects.Lerp(0.2f, new Action<float>((p) => { // Delayed action
                            if (p == 1f) {
                                if (HauntMenuMinigame.Instance) {
                                    HauntMenuMinigame.Instance.ForceClose();
                                    HauntMenuMinigame.Instance.amClosing = HauntMenuMinigame.CloseState.Closing;
                                }
                                HudManager.Instance.AbilityButton.Hide();
                                target.NetTransform.Halt();
                            }
                        })));
                    }

                    // Remove fake tasks when player dies
                    if (target.hasFakeTasks()) {
                        if (Puppeteer.puppeteer != null && target == Puppeteer.puppeteer) {
                            if (!Puppeteer.morphed) {
                                target.clearAllTasks();
                            }
                        }
                        else {
                            target.clearAllTasks();
                        }
                    }

                    // Teleport body if killed while Monja Awakened
                    if (Monja.awakened) {
                        var monjaBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                        monjaBody.transform.position = new Vector3(50, 50, 1);
                        if (target == Monja.monja) {
                            RPCProcedure.monjaReset();
                        }
                    }

                    // Lover suicide trigger on murder
                    if ((Modifiers.lover1 != null && target == Modifiers.lover1) || (Modifiers.lover2 != null && target == Modifiers.lover2)) {
                        PlayerControl otherLover = target == Modifiers.lover1 ? Modifiers.lover2 : Modifiers.lover1;
                        if (otherLover != null && !otherLover.Data.IsDead) {
                            otherLover.MurderPlayer(otherLover, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            if (Monja.awakened) {
                                var loverBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == otherLover.PlayerId);
                                loverBody.transform.position = new Vector3(50, 50, 1);
                            }
                        }
                    }

                    // Kid trigger win on murder
                    if (Kid.kid != null && target == Kid.kid) {
                        Kid.triggerKidLose = true;
                        GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.KidLose, false);
                    }

                    // Janitor Button Sync
                    if (Janitor.janitor != null && PlayerInCache.LocalPlayer.PlayerControl == Janitor.janitor && __instance == Janitor.janitor && HudManagerStartPatch.janitorCleanButton != null)
                        HudManagerStartPatch.janitorCleanButton.Timer = Janitor.janitor.killTimer;

                    if (Janitor.janitor != null && target == Janitor.janitor && Janitor.dragginBody) {
                        Janitor.janitorResetValuesAtDead();
                    }

                    // Manipulator reset manipulated if killed
                    if (Manipulator.manipulatedVictim != null && target == Manipulator.manipulatedVictim) {
                        Manipulator.manipulatedVictim = null;
                        Manipulator.manipulatedVictimTimer = 21f;
                    }

                    // Chameleon reset invisibility
                    if (Chameleon.chameleon != null && target == Chameleon.chameleon) {
                        Chameleon.resetChameleon();
                    }

                    // Sorcerer Button Sync
                    if (Sorcerer.sorcerer != null && PlayerInCache.LocalPlayer.PlayerControl == Sorcerer.sorcerer && __instance == Sorcerer.sorcerer && HudManagerStartPatch.sorcererSpellButton != null)
                        HudManagerStartPatch.sorcererSpellButton.Timer = HudManagerStartPatch.sorcererSpellButton.MaxTimer;

                    // Medusa remove petrify from list
                    if (Medusa.medusa != null && Medusa.petrifiedPlayers.Count != 0) {
                        if (target == Medusa.medusa) {
                            Medusa.ResetMedusa();
                        }
                        else {
                            Helpers.unpetrifyForMinigames(target);
                        }
                    }

                    // Devourer clear List if killed
                    if (Devourer.devourer != null && target == Devourer.devourer) {
                        if (Devourer.eatenPlayers.Count != 0) {
                            foreach (PlayerControl devouredPlayer in Devourer.eatenPlayers) {
                                if (devouredPlayer == PlayerInCache.LocalPlayer.PlayerControl) {
                                    devouredPlayer.transform.position = MapOptions.positionBeforeAte;
                                }
                            }
                        }
                        Devourer.eatenPlayers.Clear();
                        Devourer.devourEatCounterButtonText.text = $"{Devourer.eatenPlayers.Count}";
                    }

                    // Archer dead
                    if (Archer.archer != null && target == Archer.archer) {
                        if (Archer.Guides.Count != 0) {
                            foreach (var guide in Archer.Guides) {
                                guide.Value.color = Color.clear;
                            }
                        }
                        Archer.weaponEquiped = false;
                        if (Archer.bow != null) {
                            Archer.bow.gameObject.SetActive(Archer.weaponEquiped);
                        }
                    }

                    // Librarian restore abilty use if silenced target died
                    if (Librarian.librarian != null && Librarian.targetLibrary != null && target == Librarian.targetLibrary) {
                        Librarian.targetLibrary = null;
                        Librarian.targetNameButtonText.text = "";
                    }

                    // BountyHunter suicide trigger if his target is murdered
                    if (BountyHunter.bountyhunter != null && target == BountyHunter.hasToKill && BountyHunter.bountyhunter != __instance) {
                        if (!BountyHunter.bountyhunter.Data.IsDead) {
                            BountyHunter.bountyhunter.MurderPlayer(BountyHunter.bountyhunter, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                        }
                    }

                    // Yinyanger, reset both targets if he gets killed
                    if (Yinyanger.yinyanger != null && target == Yinyanger.yinyanger) {
                        Yinyanger.yinyedplayer = null;
                        Yinyanger.yangyedplayer = null;
                    }

                    // Yinyanger reset the selected target if one of them gets killed
                    if (Yinyanger.yinyanger != null && Yinyanger.yinyanger != __instance) {
                        if (Yinyanger.yinyedplayer != null && target == Yinyanger.yinyedplayer) {
                            Yinyanger.resetYined();
                        }
                        if (Yinyanger.yangyedplayer != null && target == Yinyanger.yangyedplayer) {
                            Yinyanger.resetYanged();
                        }
                    }

                    // Ninja reset marked if killed
                    if (Ninja.ninja != null && Ninja.markedTarget != null && target == Ninja.markedTarget) {
                        Ninja.markedTarget = null;
                        Ninja.targetNameButtonText.text = "";
                    }

                    // Berserker reset if revived later
                    if (Berserker.berserker != null && target == Berserker.berserker) {
                        Berserker.killedFirstTime = false;
                        Berserker.timeToKill = Berserker.backupTimeToKill;
                    }

                    // Yandere rampage mode
                    if (Yandere.yandere != null && Yandere.yandere != __instance && Yandere.target != null && target == Yandere.target && !Yandere.rampageMode) {
                        Yandere.rampageMode = true;
                        Yandere.yandereTargetButtonText.text = Language.statusRolesTexts[2];
                        Yandere.yandereKillButtonText.text = Language.statusRolesTexts[3];
                        if (PlayerInCache.LocalPlayer.PlayerControl == Yandere.yandere) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.hunterTarget, false, 100f);
                        }
                    }

                    // Stranded reset invisibility
                    if (Stranded.stranded != null && (target == Stranded.stranded || __instance == Stranded.stranded && Stranded.isInvisible)) {
                        Stranded.resetStranded();
                    }

                    // Monja revert item if killed
                    if (Monja.monja != null && target.PlayerId == Monja.monja.PlayerId) {
                        if (Monja.isDeliveringItem) {
                            RPCProcedure.monjaRevertItemPosition(Monja.itemId);
                        }
                    }

                    // Devourer play sound when someone dies
                    if (Devourer.devourer != null && Devourer.devourer == PlayerInCache.LocalPlayer.PlayerControl && !Devourer.devourer.Data.IsDead) {
                        SoundManager.Instance.PlaySound(CustomMain.customAssets.devourerDingClip, false, 100f);
                    }

                    // Poisoner restore Poison ability if poisonTarget died
                    if (Poisoner.poisoner != null && Poisoner.poisonedTarget != null && target == Poisoner.poisonedTarget) {
                        Poisoner.poisonedTarget = null;
                    }

                    // Puppeteer trigger counter or win if its was morphed
                    if (Puppeteer.puppeteer != null && target == Puppeteer.puppeteer && Puppeteer.morphed) {
                        // remove puppeteer corpse and dead entry
                        DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
                        for (int i = 0; i < array.Length; i++) {
                            if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == target.PlayerId) {
                                array[i].gameObject.active = false;
                            }
                        }
                        HudManager.Instance.StartCoroutine(Effects.Lerp(0.25f, new Action<float>((p) => { // Delayed action
                            if (p == 1f) {
                                // revive puppeteer
                                target.Revive();
                                if (Helpers.isSubmergedMap()) {
                                    if (Puppeteer.puppeteer.transform.position.y > 0) {
                                        Puppeteer.puppeteer.transform.position = new Vector3(5.5f, 31.5f, -5);
                                    }
                                    else {
                                        Puppeteer.puppeteer.transform.position = new Vector3(-4.75f, -33.25f, -5);
                                    }
                                }
                                else {
                                    Puppeteer.puppeteer.transform.position = Puppeteer.positionPreMorphed;
                                }
                                for (int i = 0; i < array.Length; i++) {
                                    if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == target.PlayerId) {
                                        UnityEngine.Object.Destroy(array[i].gameObject);
                                    }
                                }
                            }
                        })));

                        DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                        if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                        HudManagerStartPatch.puppeteerTransformButton.Timer = HudManagerStartPatch.puppeteerTransformButton.MaxTimer;
                        HudManagerStartPatch.puppeteerSampleButton.Timer = HudManagerStartPatch.puppeteerSampleButton.MaxTimer;
                        Puppeteer.morphed = false;
                        Puppeteer.puppeteer.setDefaultLook();
                        Puppeteer.counter += 1;
                        Puppeteer.transformTarget = null;
                        Puppeteer.pickTarget = null;
                        Puppeteer.currentTarget = null;
                        Puppeteer.puppeteerText.text = $"{Puppeteer.counter}/{Puppeteer.numberOfKills}";

                        __instance.SetKillTimer(0f);
                        if (PlayerInCache.LocalPlayer.PlayerControl == __instance) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.puppeteerClip, false, 75f);
                        }

                        if (PlayerInCache.LocalPlayer.PlayerControl != Puppeteer.puppeteer) return;

                        if (Puppeteer.counter >= Puppeteer.numberOfKills) {
                            MessageWriter winWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerInCache.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.PuppeteerWin, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(winWriter);
                            RPCProcedure.puppeteerWin();
                        }
                    }

                    // Kill Exiler if the target is killed
                    if (Exiler.exiler != null && Exiler.target != null && target == Exiler.target && !Exiler.exiler.Data.IsDead) {
                        Exiler.exiler.MurderPlayer(Exiler.exiler, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                    }

                    // Reset hided player if killed
                    if (Seeker.seeker != null) {
                        if (Seeker.seeker == target) {
                            Seeker.ResetValues(false);
                        }
                        if (Seeker.hidedPlayerOne != null && target == Seeker.hidedPlayerOne) {
                            Seeker.ResetOnePlayer(1);
                        }
                        if (Seeker.hidedPlayerTwo != null && target == Seeker.hidedPlayerTwo) {
                            Seeker.ResetOnePlayer(2);
                        }
                        if (Seeker.hidedPlayerThree != null && target == Seeker.hidedPlayerThree) {
                            Seeker.ResetOnePlayer(3);
                        }
                    }

                    // If Squire killed, remove the shield
                    if (Squire.squire != null && Squire.shielded != null && target == Squire.squire) {
                        Squire.shielded = null;
                    }

                    // Forensic add body
                    if (Forensic.deadBodies != null) {
                        Forensic.featureDeadBodies.Add(new Tuple<DeadPlayer, Vector3>(deadPlayer, target.transform.position));
                    }

                    // Sleuth store body positions
                    if (Sleuth.deadBodyPositions != null) Sleuth.deadBodyPositions.Add(target.transform.position);

                    // Fink reset camera on dead
                    if (Fink.fink != null && target == Fink.fink) {
                        Fink.resetCamera();
                        if (Fink.localArrows != null) {
                            foreach (Arrow arrow in Fink.localArrows) arrow.arrow.SetActive(false);
                        }
                    }

                    // Vigilant delete doorlog item when killed
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 1 && Vigilant.vigilant != null && target == Vigilant.vigilant) {
                        GameObject vigilantdoorlog = GameObject.Find("VigilantDoorLog");
                        if (vigilantdoorlog != null) {
                            vigilantdoorlog.SetActive(false);
                        }
                    }

                    // Hunter target suicide trigger on Hunter murder
                    if (Hunter.hunter != null && target == Hunter.hunter) {
                        if (Hunter.hunted != null && !Hunter.hunted.Data.IsDead) {
                            Hunter.hunted.MurderPlayer(Hunter.hunted, MurderResultFlags.Succeeded | MurderResultFlags.DecisionByHost);
                            Hunter.targetButtonText.text = $" ";
                        }
                    }

                    // Spiritualist revived player killer
                    if (Spiritualist.revivedPlayerKiller != null && target == Spiritualist.revivedPlayerKiller) {
                        RPCProcedure.murderSpiritualistRevivedPlayer();
                    }

                    // Necromancer dead
                    if (Necromancer.necromancer != null && target == Necromancer.necromancer && Necromancer.dragginBody) {
                        Necromancer.necromancerResetValuesAtDead();
                    }

                    // Task Master clear extra tasks if killed while doing them
                    if (TaskMaster.taskMaster != null && target == TaskMaster.taskMaster && TaskMaster.clearedInitialTasks) {
                        target.clearAllTasks();
                    }

                    // If Jailer killed, remove the jailed
                    if (Jailer.jailer != null && Jailer.jailedPlayer != null && target == Jailer.jailer) {
                        Jailer.jailedPlayer = null;
                    }

                    // Performer timer upon death
                    if (Modifiers.performer != null && target == Modifiers.performer) {
                        Modifiers.performerDuration = CustomOptionHolder.performerDuration.getFloat();
                        // music Stop and play theater music
                        if (PlayerInCache.LocalPlayer.PlayerControl != Spiritualist.spiritualist) {
                            if (!Monja.awakened) {
                                RPCProcedure.changeMusic(7);
                                SoundManager.Instance.PlaySound(CustomMain.customAssets.performerMusic, false, 5f);
                            }
                            new DIO(Modifiers.performerDuration, Modifiers.performer);
                        }
                        Modifiers.performerMusicStop = false;
                    }

                    // Paintball trigger on death
                    if (Modifiers.paintball != null && target == Modifiers.paintball) {
                        if (PlayerInCache.LocalPlayer.PlayerControl == __instance) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.paintballDeath, false, 100f);
                        }
                        Modifiers.active = new Dictionary<byte, float>();
                        Modifiers.paintballKillerMap = new Dictionary<byte, byte>();
                        Modifiers.active.Add(__instance.PlayerId, Modifiers.paintballDuration);
                        Modifiers.paintballKillerMap.Add(__instance.PlayerId, target.PlayerId);
                    }

                    // Electrician discharge trigger on death
                    if (Modifiers.electrician != null && target == Modifiers.electrician) {
                        if (PlayerInCache.LocalPlayer.PlayerControl == __instance) {
                            SoundManager.Instance.PlaySound(CustomMain.customAssets.policeTaser, false, 100f);
                        }
                        new Tased(Modifiers.electricianDuration, __instance);
                    }

                    // Check alive players for disable sabotage button if game result in 1vs1 special condition (impostor + rebel / impostor + captain / rebel + captain)
                    alivePlayers = 0;
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        if (!player.Data.IsDead) {
                            alivePlayers += 1;
                        }
                    }
                    break;
                case 2:
                    // CTF revive
                    var ctfBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                    Helpers.GamemodesDisableDeadBodyReportOnClic(ctfBody);

                    // Capture the flag reset flag position if killed while having it
                    if (CaptureTheFlag.redPlayerWhoHasBlueFlag != null && target == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CaptureTheFlag.blueflagtaken = false;
                        Helpers.showGamemodesPopUp(0, Helpers.playerById(CaptureTheFlag.redPlayerWhoHasBlueFlag.PlayerId));
                        CaptureTheFlag.redPlayerWhoHasBlueFlag = null;
                        CaptureTheFlag.blueflag.transform.parent = CaptureTheFlag.blueflagbase.transform.parent;
                        CaptureTheFlag.blueflag.transform.position = Helpers.CTFblueFlagPos;
                    }

                    if (CaptureTheFlag.bluePlayerWhoHasRedFlag != null && target == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CaptureTheFlag.redflagtaken = false;
                        Helpers.showGamemodesPopUp(0, Helpers.playerById(CaptureTheFlag.bluePlayerWhoHasRedFlag.PlayerId));
                        CaptureTheFlag.bluePlayerWhoHasRedFlag = null;
                        CaptureTheFlag.redflag.transform.parent = CaptureTheFlag.redflagbase.transform.parent;
                        CaptureTheFlag.redflag.transform.position = Helpers.CTFredFlagPos;
                    }

                    // Capture the flag revive player
                    if (CaptureTheFlag.stealerPlayer != null && CaptureTheFlag.stealerPlayer.PlayerId == target.PlayerId) {
                        CaptureTheFlag.revivingPlayers.Add(CaptureTheFlag.stealerPlayer);
                        CaptureTheFlag.stealerPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Normal);
                        Helpers.alphaPlayer(CaptureTheFlag.stealerPlayer.PlayerId, 0.5f);
                        Helpers.GamemodesGenericBecomeAliveAndTargetable(CaptureTheFlag.stealerPlayer, CaptureTheFlag.revivingPlayers, LasMonjas.gamemodeReviveTime, true);
                        Helpers.GamemodesGenericRevive(CaptureTheFlag.stealerPlayer, ctfBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.CTFstealerPlayerPos, Helpers.CTFstealerPlayerPos, new Vector3(0f, -33.5f, CaptureTheFlag.stealerPlayer.transform.position.z));
                    }

                    foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                        if (player.PlayerId == target.PlayerId) {
                            CaptureTheFlag.revivingPlayers.Add(player);
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, CaptureTheFlag.revivingPlayers, LasMonjas.gamemodeReviveTime);
                            Helpers.GamemodesGenericRevive(player, ctfBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.CTFredTeamPos, Helpers.CTFredTeamPos, new Vector3(-14f, -27.5f, player.transform.position.z));
                        }
                    }
                    foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                        if (player.PlayerId == target.PlayerId) {
                            CaptureTheFlag.revivingPlayers.Add(player);
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, CaptureTheFlag.revivingPlayers, LasMonjas.gamemodeReviveTime);
                            Helpers.GamemodesGenericRevive(player, ctfBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.CTFblueTeamPos, new Vector3(14.25f, 24.25f, player.transform.position.z), Helpers.CTFblueTeamPos);
                        }
                    }
                    break;
                case 3:
                    // PT
                    var ptBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                    Helpers.GamemodesDisableDeadBodyReportOnClic(ptBody);

                    foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            PoliceAndThief.revivingPlayers.Add(player);
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, PoliceAndThief.revivingPlayers, LasMonjas.gamemodeInvincibilityTime);
                            Helpers.GamemodesGenericRevive(player, ptBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.PATpoliceTeamPos, Helpers.PATpoliceTeamPos, new Vector3(-9.25f, -41.25f, player.transform.position.z));
                        }
                    }
                    foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            PoliceAndThief.revivingPlayers.Add(player);
                            if (PoliceAndThief.stealingPlayers.Contains(player)) {
                                RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, Helpers.PoliceAndThiefsGetJewelId(target));
                            }
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, PoliceAndThief.revivingPlayers, LasMonjas.gamemodeReviveTime * 1.25f);
                            Helpers.GamemodesGenericRevive(player, ptBody, (LasMonjas.gamemodeReviveTime * 1.25f) - LasMonjas.gamemodeInvincibilityTime, Helpers.PATthiefTeamPos, Helpers.PATthiefTeamPos, new Vector3(12.5f, -31.75f, player.transform.position.z));
                        }
                    }
                    break;
                case 4:
                    // KOTH
                    var kothBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                    Helpers.GamemodesDisableDeadBodyReportOnClic(kothBody);

                    if (KingOfTheHill.usurperPlayer != null && KingOfTheHill.usurperPlayer.PlayerId == target.PlayerId) {
                        KingOfTheHill.revivingPlayers.Add(KingOfTheHill.usurperPlayer);
                        KingOfTheHill.usurperPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Normal);
                        Helpers.alphaPlayer(KingOfTheHill.usurperPlayer.PlayerId, 0.5f);
                        Helpers.GamemodesGenericBecomeAliveAndTargetable(KingOfTheHill.usurperPlayer, KingOfTheHill.revivingPlayers, LasMonjas.gamemodeReviveTime, true);
                        Helpers.GamemodesGenericRevive(KingOfTheHill.usurperPlayer, kothBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.KOTHusurperPlayerPos, Helpers.KOTHusurperPlayerPos, new Vector3(-4.25f, -33.5f, KingOfTheHill.usurperPlayer.transform.position.z));
                    }

                    foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            // Restore zones
                            if (KingOfTheHill.greenKingplayer != null && target.PlayerId == KingOfTheHill.greenKingplayer.PlayerId) {
                                KingOfTheHill.resetCapturedZones(ref KingOfTheHill.greenKinghaszoneone, KingOfTheHill.flagzoneone, KingOfTheHill.zoneone, ref KingOfTheHill.zoneonecolor);
                                KingOfTheHill.resetCapturedZones(ref KingOfTheHill.greenKinghaszonetwo, KingOfTheHill.flagzonetwo, KingOfTheHill.zonetwo, ref KingOfTheHill.zonetwocolor);
                                KingOfTheHill.resetCapturedZones(ref KingOfTheHill.greenKinghaszonethree, KingOfTheHill.flagzonethree, KingOfTheHill.zonethree, ref KingOfTheHill.zonethreecolor);
                                Helpers.showGamemodesPopUp(1, Helpers.playerById(KingOfTheHill.greenKingplayer.PlayerId));
                                KingOfTheHill.totalGreenKingzonescaptured = 0;
                                // Hide aura while dead
                                DeadPlayer kinggreenPlayer = deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();
                                if (kinggreenPlayer != null && kinggreenPlayer.killerIfExisting != null) {
                                    if (kinggreenPlayer.player == KingOfTheHill.greenKingplayer && kinggreenPlayer.killerIfExisting != KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.greenkingaura.SetActive(false);
                                        HudManager.Instance.StartCoroutine(Effects.Lerp(LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, new Action<float>((p) => {
                                            if (p == 1f) {
                                                KingOfTheHill.greenkingaura.SetActive(true);
                                            }
                                        })));
                                    }
                                }
                            }
                            KingOfTheHill.revivingPlayers.Add(player);
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, KingOfTheHill.revivingPlayers, LasMonjas.gamemodeReviveTime);
                            Helpers.GamemodesGenericRevive(player, kothBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.KOTHgreenTeamPos, Helpers.KOTHgreenTeamPos, new Vector3(-14.5f, -34.35f, player.transform.position.z));
                        }
                    }
                    foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            // Restore zones
                            if (KingOfTheHill.yellowKingplayer != null && target.PlayerId == KingOfTheHill.yellowKingplayer.PlayerId) {
                                KingOfTheHill.resetCapturedZones(ref KingOfTheHill.yellowKinghaszoneone, KingOfTheHill.flagzoneone, KingOfTheHill.zoneone, ref KingOfTheHill.zoneonecolor);
                                KingOfTheHill.resetCapturedZones(ref KingOfTheHill.yellowKinghaszonetwo, KingOfTheHill.flagzonetwo, KingOfTheHill.zonetwo, ref KingOfTheHill.zonetwocolor);
                                KingOfTheHill.resetCapturedZones(ref KingOfTheHill.yellowKinghaszonethree, KingOfTheHill.flagzonethree, KingOfTheHill.zonethree, ref KingOfTheHill.zonethreecolor);
                                Helpers.showGamemodesPopUp(2, Helpers.playerById(KingOfTheHill.yellowKingplayer.PlayerId));
                                KingOfTheHill.totalYellowKingzonescaptured = 0;
                                // Hide aura while dead
                                DeadPlayer kingyellowPlayer = deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();
                                if (kingyellowPlayer != null && kingyellowPlayer.killerIfExisting != null) {
                                    if (kingyellowPlayer.player == KingOfTheHill.yellowKingplayer && kingyellowPlayer.killerIfExisting != KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.yellowkingaura.SetActive(false);
                                        HudManager.Instance.StartCoroutine(Effects.Lerp(LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, new Action<float>((p) => {
                                            if (p == 1f) {
                                                KingOfTheHill.yellowkingaura.SetActive(true);
                                            }
                                        })));
                                    }
                                }
                            }
                            KingOfTheHill.revivingPlayers.Add(player);
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, KingOfTheHill.revivingPlayers, LasMonjas.gamemodeReviveTime);
                            Helpers.GamemodesGenericRevive(player, kothBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.KOTHyellowTeamPos, new Vector3(0f, 33.5f, player.transform.position.z), Helpers.KOTHyellowTeamPos);
                        }
                    }
                    break;
                case 5:
                    // HP
                    var hpBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                    Helpers.GamemodesDisableDeadBodyReportOnClic(hpBody);

                    if (HotPotato.hotPotatoPlayer != null && HotPotato.hotPotatoPlayer.PlayerId == target.PlayerId) {

                        HotPotato.timeforTransfer = HotPotato.savedtimeforTransfer + 4f;

                        HudManager.Instance.StartCoroutine(Effects.Lerp(1, new Action<float>((p) => { // Delayed action
                            if (p == 1f) {

                                Helpers.AddExplodedPotato(HotPotato.hotPotatoPlayer);

                                int notPotatosAlives = -1;
                                HotPotato.notPotatoTeamAlive.Clear();
                                foreach (PlayerControl notPotato in HotPotato.notPotatoTeam) {
                                    if (!notPotato.Data.IsDead) {
                                        notPotatosAlives += 1;
                                        HotPotato.notPotatoTeamAlive.Add(notPotato);
                                    }
                                }

                                if (notPotatosAlives < 1) {
                                    HotPotato.triggerHotPotatoEnd = true;
                                    GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.HotPotatoEnd, false);
                                    return;
                                }

                                PlayerControl nextPotato = HotPotato.notPotatoTeam[0];
                                Helpers.RemoveNotPotato(nextPotato);
                                HotPotato.hotPotatoPlayer = nextPotato;

                                HotPotato.notPotatoTeam.RemoveAt(0);

                                HotPotato.hotPotatoPlayer.NetTransform.Halt();
                                HotPotato.hotPotatoPlayer.moveable = false;
                                Helpers.RestoreBodyTypeWithDelay(HotPotato.hotPotatoPlayer);
                                HotPotato.hotPotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                HotPotato.hotPotato.transform.parent = HotPotato.hotPotatoPlayer.transform;

                                HudManager.Instance.StartCoroutine(Effects.Lerp(3, new Action<float>((p) => { // Delayed action
                                    if (p == 1f) {
                                        HotPotato.hotPotatoPlayer.moveable = true;
                                    }
                                })));

                                Helpers.showGamemodesPopUp(1, Helpers.playerById(HotPotato.hotPotatoPlayer.PlayerId));
                                HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + notPotatosAlives + "</color>";
                            }
                        })));
                    }
                    break;
                case 6:
                    // ZL
                    var zlBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                    Helpers.GamemodesDisableDeadBodyReportOnClic(zlBody);

                    foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            ZombieLaboratory.revivingPlayers.Add(player);
                            ZombieLaboratory.hasAmmoPlayers.Remove(player);
                            if (ZombieLaboratory.infectedPlayers.Contains(player)) {
                                ZombieLaboratory.infectedPlayers.Remove(player);
                            }
                            if (ZombieLaboratory.hasKeyItemPlayers.Contains(player)) {
                                ZombieLaboratory.hasKeyItemPlayers.Remove(player);
                                RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, Helpers.ZombieLaboratoryGetFoundBox(target));
                            }
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, ZombieLaboratory.revivingPlayers, LasMonjas.gamemodeReviveTime);
                            Helpers.GamemodesGenericRevive(player, zlBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.ZLsurvivorTeamPos, Helpers.ZLsurvivorTeamPos, new Vector3(9.75f, -31.35f, player.transform.position.z));
                        }
                    }
                    foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            ZombieLaboratory.revivingPlayers.Add(player);
                            player.MyPhysics.SetBodyType(PlayerBodyTypes.Normal);
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, ZombieLaboratory.revivingPlayers, LasMonjas.gamemodeReviveTime, true);
                            Helpers.GamemodesGenericRevive(player, zlBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.ZLzombieTeamPos, Helpers.ZLzombieTeamPos, new Vector3(-4.15f, -33.5f, player.transform.position.z));
                        }
                    }
                    ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedPlayers.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                    break;
                case 7:
                    // BR
                    var brBody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                    Helpers.GamemodesDisableDeadBodyReportOnClic(brBody);

                    if (BattleRoyale.matchType == 2) {
                        if (BattleRoyale.serialKiller != null && BattleRoyale.serialKiller.PlayerId == target.PlayerId) {
                            BattleRoyale.revivingPlayers.Add(BattleRoyale.serialKiller);
                            //BattleRoyale.serialKiller.MyPhysics.SetBodyType(PlayerBodyTypes.Normal);
                            Helpers.alphaPlayer(BattleRoyale.serialKiller.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(BattleRoyale.serialKiller, BattleRoyale.revivingPlayers, LasMonjas.gamemodeReviveTime, true, true);
                            Helpers.GamemodesGenericRevive(BattleRoyale.serialKiller, brBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.BRserialKillerPos, Helpers.BRserialKillerPos, new Vector3(-4.25f, -33.5f, BattleRoyale.serialKiller.transform.position.z));
                        }

                        foreach (PlayerControl player in BattleRoyale.limeTeam) {
                            if (player.PlayerId == target.PlayerId) {
                                BattleRoyale.revivingPlayers.Add(player);
                                Helpers.alphaPlayer(player.PlayerId, 0.5f);
                                Helpers.GamemodesGenericBecomeAliveAndTargetable(player, BattleRoyale.revivingPlayers, LasMonjas.gamemodeReviveTime, false, true);
                                Helpers.GamemodesGenericRevive(player, brBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.BRlimeTeamPos, Helpers.BRlimeTeamPos, new Vector3(-14.5f, -34.35f, player.transform.position.z));
                            }
                        }
                        foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                            if (player.PlayerId == target.PlayerId) {
                                BattleRoyale.revivingPlayers.Add(player);
                                Helpers.alphaPlayer(player.PlayerId, 0.5f);
                                Helpers.GamemodesGenericBecomeAliveAndTargetable(player, BattleRoyale.revivingPlayers, LasMonjas.gamemodeReviveTime, false, true);
                                Helpers.GamemodesGenericRevive(player, brBody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.BRpinkTeamPos, new Vector3(0f, 33.5f, player.transform.position.z), Helpers.BRpinkTeamPos);
                            }
                        }
                    }
                    break;
                case 8:
                    // MF
                    var mfbody = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                    Helpers.GamemodesDisableDeadBodyReportOnClic(mfbody);

                    float AngleStep = 360.0f;
                    float Offset = 0.15f;
                    if (MonjaFestival.bigMonjaPlayer != null && MonjaFestival.bigMonjaPlayer.PlayerId == target.PlayerId) {
                        MonjaFestival.resetBigMonja();
                        MonjaFestival.revivingPlayers.Add(MonjaFestival.bigMonjaPlayer);
                        Helpers.MonjaFestivalResetPlayer(MonjaFestival.bigMonjaPlayer, ref MonjaFestival.bigMonjaPlayerItems, MonjaFestival.bigMonjaPlayerDeliverCount, null, AngleStep, Offset);
                        MonjaFestival.bigMonjaPlayerItems = 0;
                        MonjaFestival.bigMonjaPlayerDeliverCount.text = $"{MonjaFestival.bigMonjaPlayerItems} / 10";
                        MonjaFestival.bigMonjaPlayer.MyPhysics.SetBodyType(PlayerBodyTypes.Normal);
                        Helpers.alphaPlayer(MonjaFestival.bigMonjaPlayer.PlayerId, 0.5f);
                        Helpers.GamemodesGenericBecomeAliveAndTargetable(MonjaFestival.bigMonjaPlayer, MonjaFestival.revivingPlayers, LasMonjas.gamemodeReviveTime, true);
                        Helpers.GamemodesGenericRevive(MonjaFestival.bigMonjaPlayer, mfbody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.MFbigMonjaPos, Helpers.MFbigMonjaPos, new Vector3(7.15f, -20.5f, MonjaFestival.bigMonjaPlayer.transform.position.z));
                    }

                    foreach (PlayerControl player in MonjaFestival.greenTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            MonjaFestival.revivingPlayers.Add(player);
                            if (MonjaFestival.greenPlayer01 != null && target.PlayerId == MonjaFestival.greenPlayer01.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.greenPlayer01, ref MonjaFestival.greenPlayer01Items, MonjaFestival.greenmonja01DeliverCount, MonjaFestival.handsGreen01, AngleStep, Offset);
                            }
                            else if (MonjaFestival.greenPlayer02 != null && target.PlayerId == MonjaFestival.greenPlayer02.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.greenPlayer02, ref MonjaFestival.greenPlayer02Items, MonjaFestival.greenmonja02DeliverCount, MonjaFestival.handsGreen02, AngleStep, Offset);
                            }
                            else if (MonjaFestival.greenPlayer03 != null && target.PlayerId == MonjaFestival.greenPlayer03.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.greenPlayer03, ref MonjaFestival.greenPlayer03Items, MonjaFestival.greenmonja03DeliverCount, MonjaFestival.handsGreen03, AngleStep, Offset);
                            }
                            else if (MonjaFestival.greenPlayer04 != null && target.PlayerId == MonjaFestival.greenPlayer04.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.greenPlayer04, ref MonjaFestival.greenPlayer04Items, MonjaFestival.greenmonja04DeliverCount, MonjaFestival.handsGreen04, AngleStep, Offset);
                            }
                            else if (MonjaFestival.greenPlayer05 != null && target.PlayerId == MonjaFestival.greenPlayer05.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.greenPlayer05, ref MonjaFestival.greenPlayer05Items, MonjaFestival.greenmonja05DeliverCount, MonjaFestival.handsGreen05, AngleStep, Offset);
                            }
                            else if (MonjaFestival.greenPlayer06 != null && target.PlayerId == MonjaFestival.greenPlayer06.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.greenPlayer06, ref MonjaFestival.greenPlayer06Items, MonjaFestival.greenmonja06DeliverCount, MonjaFestival.handsGreen06, AngleStep, Offset);
                            }
                            else if (MonjaFestival.greenPlayer07 != null && target.PlayerId == MonjaFestival.greenPlayer07.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.greenPlayer07, ref MonjaFestival.greenPlayer07Items, MonjaFestival.greenmonja07DeliverCount, MonjaFestival.handsGreen07, AngleStep, Offset);
                            }
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, MonjaFestival.revivingPlayers, LasMonjas.gamemodeReviveTime);
                            Helpers.GamemodesGenericRevive(player, mfbody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.MFgreenTeamPos, Helpers.MFgreenTeamPos, new Vector3(-4.35f, -33.5f, player.transform.position.z));
                        }
                    }
                    foreach (PlayerControl player in MonjaFestival.cyanTeam) {
                        if (player.PlayerId == target.PlayerId) {

                            MonjaFestival.revivingPlayers.Add(player);
                            if (MonjaFestival.cyanPlayer01 != null && target.PlayerId == MonjaFestival.cyanPlayer01.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.cyanPlayer01, ref MonjaFestival.cyanPlayer01Items, MonjaFestival.cyanPlayer01DeliverCount, MonjaFestival.handsCyan01, AngleStep, Offset);
                            }
                            else if (MonjaFestival.cyanPlayer02 != null && target.PlayerId == MonjaFestival.cyanPlayer02.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.cyanPlayer02, ref MonjaFestival.cyanPlayer02Items, MonjaFestival.cyanPlayer02DeliverCount, MonjaFestival.handsCyan02, AngleStep, Offset);
                            }
                            else if (MonjaFestival.cyanPlayer03 != null && target.PlayerId == MonjaFestival.cyanPlayer03.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.cyanPlayer03, ref MonjaFestival.cyanPlayer03Items, MonjaFestival.cyanPlayer03DeliverCount, MonjaFestival.handsCyan03, AngleStep, Offset);
                            }
                            else if (MonjaFestival.cyanPlayer04 != null && target.PlayerId == MonjaFestival.cyanPlayer04.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.cyanPlayer04, ref MonjaFestival.cyanPlayer04Items, MonjaFestival.cyanPlayer04DeliverCount, MonjaFestival.handsCyan04, AngleStep, Offset);
                            }
                            else if (MonjaFestival.cyanPlayer05 != null && target.PlayerId == MonjaFestival.cyanPlayer05.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.cyanPlayer05, ref MonjaFestival.cyanPlayer05Items, MonjaFestival.cyanPlayer05DeliverCount, MonjaFestival.handsCyan05, AngleStep, Offset);
                            }
                            else if (MonjaFestival.cyanPlayer06 != null && target.PlayerId == MonjaFestival.cyanPlayer06.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.cyanPlayer06, ref MonjaFestival.cyanPlayer06Items, MonjaFestival.cyanPlayer06DeliverCount, MonjaFestival.handsCyan06, AngleStep, Offset);
                            }
                            else if (MonjaFestival.cyanPlayer07 != null && target.PlayerId == MonjaFestival.cyanPlayer07.PlayerId) {
                                Helpers.MonjaFestivalResetPlayer(MonjaFestival.cyanPlayer07, ref MonjaFestival.cyanPlayer07Items, MonjaFestival.cyanPlayer07DeliverCount, MonjaFestival.handsCyan07, AngleStep, Offset);
                            }
                            Helpers.alphaPlayer(player.PlayerId, 0.5f);
                            Helpers.GamemodesGenericBecomeAliveAndTargetable(player, MonjaFestival.revivingPlayers, LasMonjas.gamemodeReviveTime);
                            Helpers.GamemodesGenericRevive(player, mfbody, LasMonjas.gamemodeReviveTime - LasMonjas.gamemodeInvincibilityTime, Helpers.MFcyanTeamPos, new Vector3(-10.25f, 10.15f, player.transform.position.z), Helpers.MFcyanTeamPos);
                        }
                    }
                    break;
            }
            // Reset RolesSummaryUI on target if is open when it gets killed
            if (PlayerInCache.LocalPlayer.PlayerControl == target) {
                Helpers.ResetRoleSummaryUI();
            }
        }
    }

    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.CoPerformKill))]
    class KillAnimationCoPerformKillPatch {
        public static bool hideNextAnimation = true;
        public static void Prefix(KillAnimation __instance, [HarmonyArgument(0)]ref PlayerControl source, [HarmonyArgument(1)]ref PlayerControl target) {
            if (hideNextAnimation)
                source = target;
            hideNextAnimation = false;
        }
    }

    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.SetMovement))]
    class KillAnimationSetMovementPatch {
        private static int? colorId = null;
        public static void Prefix(PlayerControl source, bool canMove) {
            Color color = source.cosmetics.currentBodySprite.BodySprite.material.GetColor("_BodyColor");
            if (Mimic.mimic != null && source.Data.PlayerId == Mimic.mimic.PlayerId) {
                var index = Palette.PlayerColors.IndexOf(color);
                if (index != -1) colorId = index;
            }
        }

        public static void Postfix(PlayerControl source, bool canMove) {
            if (colorId.HasValue) source.RawSetColor(colorId.Value);
            colorId = null;
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RemoveTask))]
    class PlayerControlRemoveTaskPatch
    {
        static void Postfix(PlayerTask task) {
            switch (task.TaskType) {
                case TaskTypes.FixComms:
                    isHappeningAnonymousComms = false;
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        player.setDefaultLook();
                        if (player.cosmetics.currentPet) player.cosmetics.currentPet.gameObject.SetActive(true);
                    }
                    break;
                case TaskTypes.ResetReactor:
                    HudManager.Instance.PlayerCam.shakeAmount = 0f;
                    HudManager.Instance.PlayerCam.shakePeriod = 0;
                    break;
                case TaskTypes.ResetSeismic:
                    HudManager.Instance.PlayerCam.shakeAmount = 0f;
                    HudManager.Instance.PlayerCam.shakePeriod = 0;
                    break;
                case TaskTypes.StopCharles:
                    HudManager.Instance.PlayerCam.shakeAmount = 0f;
                    HudManager.Instance.PlayerCam.shakePeriod = 0;
                    break;
                case TaskTypes.RestoreOxy:
                    foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                        player.MyPhysics.Speed = 2.5f;
                    }
                    break;
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
    public static class ExilePlayerPatch
    {
        public static void Prefix(PlayerControl __instance) {
            // Kid exile lose condition
            if (Kid.kid != null && Kid.kid == __instance) {
                Kid.triggerKidLose = true;
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.KidLose, false);
            }
            // Joker win condition
            else if (Joker.joker != null && Joker.joker == __instance) {
                Joker.triggerJokerWin = true;
            }
        }

        public static void Postfix(PlayerControl __instance) {
            // Collect dead player info
            DeadPlayer deadPlayer = new DeadPlayer(__instance, DateTime.UtcNow, DeathReason.Exile, null);
            GameHistory.deadPlayers.Add(deadPlayer);

            // Remove fake tasks when player dies
            if (__instance.hasFakeTasks())
                __instance.clearAllTasks();

            // Lover suicide trigger on exile
            if ((Modifiers.lover1 != null && __instance == Modifiers.lover1) || (Modifiers.lover2 != null && __instance == Modifiers.lover2)) {
                PlayerControl otherLover = __instance == Modifiers.lover1 ? Modifiers.lover2 : Modifiers.lover1;
                if (otherLover != null && !otherLover.Data.IsDead)
                    otherLover.Exiled();
            }

            // Check alive players for disable sabotage button if game result in 1vs1 special condition (impostor + rebel / impostor + captain / rebel + captain)
            alivePlayers = 0;
            foreach (PlayerControl player in PlayerInCache.AllPlayers) {
                if (!player.Data.IsDead) {
                    alivePlayers += 1;
                }
            }
        }
    }
}