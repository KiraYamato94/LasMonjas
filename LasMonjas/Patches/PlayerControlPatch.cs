using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            float num = GameOptionsData.KillDistances[Mathf.Clamp(GameOptionsManager.Instance.currentGameOptions.GetInt(Int32OptionNames.KillDistance), 0, 2)];
            if (!ShipStatus.Instance) return result;
            if (targetingPlayer == null) targetingPlayer = PlayerControl.LocalPlayer;
            if (targetingPlayer.Data.IsDead) return result;

            Vector2 truePosition = targetingPlayer.GetTruePosition();
            Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
            for (int i = 0; i < allPlayers.Count; i++) {
                GameData.PlayerInfo playerInfo = allPlayers[i];
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
            foreach (PlayerControl target in PlayerControl.AllPlayerControls) {
                if (target == null || target.cosmetics.currentBodySprite.BodySprite == null) continue;

                bool isTransformedMimic = target == Mimic.mimic && Mimic.transformTarget != null && Mimic.transformTimer > 0f;
                bool isTransformedPuppeteer = target == Puppeteer.puppeteer && Puppeteer.transformTarget != null && Puppeteer.morphed;
                bool hasVisibleShield = false;
                if (Painter.painterTimer <= 0f && Squire.shielded != null && !Challenger.isDueling && !Seeker.isMinigaming && ((target == Squire.shielded && !isTransformedMimic) || (isTransformedMimic && Mimic.transformTarget == Squire.shielded) || (isTransformedPuppeteer && Puppeteer.transformTarget == Squire.shielded))) {
                    hasVisibleShield = Squire.showShielded == 0 && PlayerControl.LocalPlayer == Squire.squire // Squire only
                        || (Squire.showShielded == 1 && (PlayerControl.LocalPlayer == Squire.shielded || PlayerControl.LocalPlayer == Squire.squire)) // Shielded + Squire
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
            if (howmanygamemodesareon != 1) {
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                    if (p == PlayerControl.LocalPlayer || PlayerControl.LocalPlayer.Data.IsDead) {

                        PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.TargetPlayerId == p.PlayerId);
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
                        if (MapOptions.ghostsSeeRoles && PlayerControl.LocalPlayer.Data.IsDead) {
                            playerInfoText = $"{roleNames}";
                            meetingInfoText = playerInfoText;
                        }

                        if (meetingInfo != null) meetingInfo.text = MeetingHud.Instance.state == MeetingHud.VoteStates.Results ? "" : meetingInfoText;
                    }
                }
            }
        }
        static void ventColorUpdate() {
            if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && ShipStatus.Instance?.AllVents != null) {
                foreach (Vent vent in ShipStatus.Instance.AllVents) {
                    try {
                        if (vent?.myRend?.material != null) {
                            if (Renegade.renegade != null && Renegade.renegade.inVent || Minion.minion != null && Minion.minion.inVent) {
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
            if (!PlayerControl.LocalPlayer.Data.Role.IsImpostor || Archer.archer != null && PlayerControl.LocalPlayer == Archer.archer || Demon.demon != null && PlayerControl.LocalPlayer == Demon.demon || !PlayerControl.LocalPlayer.CanMove || PlayerControl.LocalPlayer.Data.IsDead || howmanygamemodesareon == 1) { // !isImpostor || !canMove || isDead
                HudManager.Instance.KillButton.SetTarget(null);
                return;
            }

            PlayerControl target = null;
            target = setTarget(true, false);

            HudManager.Instance.KillButton.SetTarget(target);
        }
        static void mimicSetTarget() {
            if (Mimic.mimic == null || Mimic.mimic != PlayerControl.LocalPlayer) return;
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
                if (Mimic.transformTimer > 0f && Mimic.mimic != null && Mimic.transformTarget != null) {
                    PlayerControl target = Mimic.transformTarget;
                    Mimic.mimic.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }
                if (Puppeteer.puppeteer != null && Puppeteer.morphed) {
                    PlayerControl target = Puppeteer.transformTarget;
                    Puppeteer.puppeteer.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
                }                
            }

            // Mimic reset (only if paint is inactive)
            if (Painter.painterTimer <= 0f && oldMimicTimer > 0f && Mimic.transformTimer <= 0f && Mimic.mimic != null)
                Mimic.resetMimic();
        }
        static void demonSetTarget() {
            if (Demon.demon == null || Demon.demon != PlayerControl.LocalPlayer) return;

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
            if (Manipulator.manipulator == null || Manipulator.manipulator != PlayerControl.LocalPlayer) return;
            if (Manipulator.manipulatedVictim != null && (Manipulator.manipulatedVictim.Data.Disconnected || Manipulator.manipulatedVictim.Data.IsDead)) {
                // If the manipulated victim is disconnected or dead reset the manipulate so a new manipulate can be applied
                Manipulator.resetManipulate();
            }
            if (Manipulator.manipulatedVictim == null) {
                Manipulator.currentTarget = setTarget();
                setPlayerOutline(Manipulator.currentTarget, Manipulator.color);
            }
            else {
                Manipulator.manipulatedVictimTarget = setTarget(targetingPlayer: Manipulator.manipulatedVictim);
                setPlayerOutline(Manipulator.manipulatedVictimTarget, Manipulator.color);
            }
        }
        static void sorcererSetTarget() {
            if (Sorcerer.sorcerer == null || Sorcerer.sorcerer != PlayerControl.LocalPlayer) return;
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
            if (Medusa.medusa == null || Medusa.medusa != PlayerControl.LocalPlayer) return;
            PlayerControl target = null;

            target = setTarget(true, false);
            Medusa.currentTarget = target;
            setPlayerOutline(Medusa.currentTarget, Medusa.color);
        }
        static void librarianSetTarget() {
            if (Librarian.librarian == null || Librarian.librarian != PlayerControl.LocalPlayer) return;
            Librarian.currentTarget = setTarget(true, false);
            setPlayerOutline(Librarian.currentTarget, Librarian.color);
        }
        static void renegadeSetTarget() {
            if (Renegade.renegade == null || Renegade.renegade != PlayerControl.LocalPlayer) return;
            var untargetablePlayers = new List<PlayerControl>();
            if (Minion.minion != null) untargetablePlayers.Add(Minion.minion);
            Renegade.currentTarget = setTarget(untargetablePlayers: untargetablePlayers);
            setPlayerOutline(Renegade.currentTarget, Palette.ImpostorRed);
        }
        static void minionSetTarget() {
            if (Minion.minion == null || Minion.minion != PlayerControl.LocalPlayer) return;
            var untargetablePlayers = new List<PlayerControl>();
            if (Renegade.renegade != null) untargetablePlayers.Add(Renegade.renegade);
            Minion.currentTarget = setTarget(untargetablePlayers: untargetablePlayers);
            setPlayerOutline(Minion.currentTarget, Palette.ImpostorRed);
        }
        static void bountyHunterSetTarget() {
            if (BountyHunter.bountyhunter == null || BountyHunter.bountyhunter != PlayerControl.LocalPlayer) return;
            BountyHunter.currentTarget = setTarget();
            setPlayerOutline(BountyHunter.currentTarget, BountyHunter.color);
        }
        static void trapperSetTarget() {
            if (Trapper.trapper == null || Trapper.trapper != PlayerControl.LocalPlayer) return;
            Trapper.currentTarget = setTarget();
            setPlayerOutline(Trapper.currentTarget, Trapper.color);
        }
        static void yinyangerSetTarget() {
            if (Yinyanger.yinyanger == null || Yinyanger.yinyanger != PlayerControl.LocalPlayer) return;
            Yinyanger.currentTarget = setTarget();
            setPlayerOutline(Yinyanger.currentTarget, Yinyanger.color);
        }
        static void challengerSetTarget() {
            if (Challenger.challenger == null || Challenger.challenger != PlayerControl.LocalPlayer) return;
            Challenger.currentTarget = setTarget();
            setPlayerOutline(Challenger.currentTarget, Challenger.color);
        }
        static void ninjaSetTarget() {
            if (Ninja.ninja == null || Ninja.ninja != PlayerControl.LocalPlayer) return;
            Ninja.currentTarget = setTarget();
            setPlayerOutline(Ninja.currentTarget, Ninja.color);
        }
        static void berserkerSetTarget() {
            if (Berserker.berserker == null || Berserker.berserker != PlayerControl.LocalPlayer) return;
            Berserker.currentTarget = setTarget();
            setPlayerOutline(Berserker.currentTarget, Berserker.color);
        }
        static void yandereSetTarget() {
            if (Yandere.yandere == null || Yandere.yandere != PlayerControl.LocalPlayer) return;
            if (Yandere.target == null) return;

            if (!Yandere.rampageMode) {
                var untargetables = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId != Yandere.target.PlayerId).ToList();
                Yandere.currentTarget = setTarget(untargetablePlayers: untargetables);
            } else {
                Yandere.currentTarget = setTarget();
            }
            setPlayerOutline(Yandere.currentTarget, Yandere.color);
        }
        static void strandedSetTarget() {
            if (Stranded.stranded == null || Stranded.stranded != PlayerControl.LocalPlayer) return;
            Stranded.currentTarget = setTarget();
            setPlayerOutline(Stranded.currentTarget, Stranded.color);
        }
        static void monjaSetTarget() {
            if (Monja.monja == null || Monja.monja != PlayerControl.LocalPlayer) return;
            Monja.currentTarget = setTarget();
            setPlayerOutline(Monja.currentTarget, Monja.color);
        }
        static void roleThiefSetTarget() {
            if (RoleThief.rolethief == null || RoleThief.rolethief != PlayerControl.LocalPlayer) return;
            RoleThief.currentTarget = setTarget();
            setPlayerOutline(RoleThief.currentTarget, RoleThief.color);
        }
        public static void pyromaniacSetTarget() {
            if (Pyromaniac.pyromaniac == null || Pyromaniac.pyromaniac != PlayerControl.LocalPlayer) return;
            List<PlayerControl> untargetables;
            if (Pyromaniac.sprayTarget != null)
                untargetables = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId != Pyromaniac.sprayTarget.PlayerId).ToList();
            else
                untargetables = Pyromaniac.sprayedPlayers;
            Pyromaniac.currentTarget = setTarget(untargetablePlayers: untargetables);
            if (Pyromaniac.currentTarget != null) setPlayerOutline(Pyromaniac.currentTarget, Pyromaniac.color);
        }
        public static void poisonerSetTarget() {
            if (Poisoner.poisoner == null || Poisoner.poisoner != PlayerControl.LocalPlayer) return;
            List<PlayerControl> untargetables;
            if (Poisoner.poisonTarget != null)
                untargetables = PlayerControl.AllPlayerControls.ToArray().Where(x => x.PlayerId != Poisoner.poisonTarget.PlayerId).ToList();
            else
                untargetables = Poisoner.poisonedPlayers;
            Poisoner.currentTarget = setTarget(untargetablePlayers: untargetables);
            if (Poisoner.currentTarget != null) setPlayerOutline(Poisoner.currentTarget, Poisoner.color);
        }
        static void puppeteerSetTarget() {
            if (Puppeteer.puppeteer == null || Puppeteer.puppeteer != PlayerControl.LocalPlayer) return;
            Puppeteer.currentTarget = setTarget();
            setPlayerOutline(Puppeteer.currentTarget, Puppeteer.color);
        }
        static void seekerSetTarget() {
            if (Seeker.seeker == null || Seeker.seeker != PlayerControl.LocalPlayer) return;
            Seeker.currentTarget = setTarget();
            setPlayerOutline(Seeker.currentTarget, Seeker.color);
        }
        static void sheriffSetTarget() {
            if (Sheriff.sheriff == null || Sheriff.sheriff != PlayerControl.LocalPlayer) return;
            Sheriff.currentTarget = setTarget();
            setPlayerOutline(Sheriff.currentTarget, Sheriff.color);
        }
        static void detectiveUpdateFootPrints() {
            if (Detective.detective == null || Detective.detective != PlayerControl.LocalPlayer) return;

            Detective.timer -= Time.fixedDeltaTime;
            if (Detective.timer <= 0f) {
                Detective.timer = Detective.footprintIntervall;
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (player != null && player != PlayerControl.LocalPlayer && !player.Data.IsDead && !player.inVent && !PlayerControl.LocalPlayer.Data.IsDead) {
                        new Footprint(Detective.footprintDuration, Detective.anonymousFootprints, player);
                    }
                }
            }
        }
        public static void forensicSetTarget() {
            if (Forensic.forensic == null || Forensic.forensic != PlayerControl.LocalPlayer || Forensic.forensic.Data.IsDead || Forensic.deadBodies == null || ShipStatus.Instance?.AllVents == null) return;

            DeadPlayer target = null;
            Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
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
        public static void bendTimeUpdate() {
            if (TimeTraveler.isRewinding) {
                if (localPlayerPositions.Count > 0) {
                    // Set position
                    var next = localPlayerPositions[0];
                    // Exit current vent if necessary
                    if (PlayerControl.LocalPlayer.inVent) {
                        foreach (Vent vent in ShipStatus.Instance.AllVents) {
                            bool canUse;
                            bool couldUse;
                            vent.CanUse(PlayerControl.LocalPlayer.Data, out canUse, out couldUse);
                            if (canUse) {
                                PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(vent.Id);
                                vent.SetButtons(false);
                            }
                        }
                    }

                    // Set position
                    if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
                        localPlayerPositions.RemoveAt(0);

                        if (localPlayerPositions.Count > 1) localPlayerPositions.RemoveAt(0); // Skip every second position to rewind twice as fast, but never skip the last position
                        if (PlayerControl.LocalPlayer.transform.position.y > 0) {
                            PlayerControl.LocalPlayer.transform.position = new Vector3(5f, 19.5f, PlayerControl.LocalPlayer.transform.position.z);
                        }
                        else {
                            PlayerControl.LocalPlayer.transform.position = new Vector3(1.35f, -28.25f, PlayerControl.LocalPlayer.transform.position.z);
                        }
                    }
                    else {
                        PlayerControl.LocalPlayer.transform.position = next.Item1;

                        localPlayerPositions.RemoveAt(0);

                        if (localPlayerPositions.Count > 1) localPlayerPositions.RemoveAt(0); // Skip every second position to rewind twice as fast, but never skip the last position
                    }

                    // Try reviving LOCAL player 
                    if (TimeTraveler.reviveDuringRewind && PlayerControl.LocalPlayer.Data.IsDead) {
                        DeadPlayer deadPlayer = deadPlayers.Where(x => x.player == PlayerControl.LocalPlayer).FirstOrDefault();
                        if (deadPlayer != null && next.Item2 < deadPlayer.timeOfDeath) {
                            MessageWriter write = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TimeTravelerRevive, Hazel.SendOption.Reliable, -1);
                            write.Write(PlayerControl.LocalPlayer.PlayerId);
                            AmongUsClient.Instance.FinishRpcImmediately(write);
                            RPCProcedure.timeTravelerRevive(PlayerControl.LocalPlayer.PlayerId);
                        }
                    }
                }
                else {
                    TimeTraveler.isRewinding = false;
                    PlayerControl.LocalPlayer.moveable = true;
                }
            }
            else {
                while (localPlayerPositions.Count >= Mathf.Round(TimeTraveler.rewindTime / Time.fixedDeltaTime)) localPlayerPositions.RemoveAt(localPlayerPositions.Count - 1);
                localPlayerPositions.Insert(0, new Tuple<Vector3, DateTime>(PlayerControl.LocalPlayer.transform.position, DateTime.UtcNow)); // CanMove = CanMove
            }
        }
        static void squireSetTarget() {
            if (Squire.squire == null || Squire.squire != PlayerControl.LocalPlayer) return;
            Squire.currentTarget = setTarget();
            if (!Squire.usedShield) setPlayerOutline(Squire.currentTarget, Squire.color);
        }
        static void fortuneTellerSetTarget() {
            if (FortuneTeller.fortuneTeller == null || FortuneTeller.fortuneTeller != PlayerControl.LocalPlayer) return;
            FortuneTeller.currentTarget = setTarget();
            setPlayerOutline(FortuneTeller.currentTarget, FortuneTeller.color);
            if (FortuneTeller.currentTarget != null && FortuneTeller.revealedPlayers.Any(p => p.Data.PlayerId == FortuneTeller.currentTarget.Data.PlayerId)) FortuneTeller.currentTarget = null; // Remove target if already revealed
        }
        public static void hackerUpdate() {
            if (Hacker.hacker == null || PlayerControl.LocalPlayer != Hacker.hacker || Hacker.hacker.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.taskInfo(Hacker.hacker.Data);
            if (playerCompleted == Hacker.rechargedTasks) {
                MessageWriter usedRechargeWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.HackerAbilityUses, Hazel.SendOption.Reliable, -1);
                usedRechargeWriter.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(usedRechargeWriter);
                RPCProcedure.hackerAbilityUses(2);
            }
        }
        static void sleuthSetTarget() {
            if (Sleuth.sleuth == null || Sleuth.sleuth != PlayerControl.LocalPlayer) return;
            Sleuth.currentTarget = setTarget();
            if (!Sleuth.usedLocate) setPlayerOutline(Sleuth.currentTarget, Sleuth.color);
        }
        static void sleuthUpdate() {
            // Handle player locate
            if (Sleuth.arrow?.arrow != null) {
                if (Sleuth.sleuth == null || PlayerControl.LocalPlayer != Sleuth.sleuth || Challenger.isDueling || Seeker.isMinigaming || isHappeningAnonymousComms) {
                    Sleuth.arrow.arrow.SetActive(false);
                    return;
                }

                if (Sleuth.sleuth != null && Sleuth.located != null && PlayerControl.LocalPlayer == Sleuth.sleuth && !Sleuth.sleuth.Data.IsDead) {
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
            if (Sleuth.sleuth != null && Sleuth.sleuth == PlayerControl.LocalPlayer && Sleuth.corpsesPathfindTimer >= 0f && !Sleuth.sleuth.Data.IsDead) {
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
        }
        static void finkUpdate() {

            if (Fink.fink == null || Fink.fink.Data.IsDead) return;

            if (Fink.localArrows == null) return;

            foreach (Arrow arrow in Fink.localArrows) arrow.arrow.SetActive(false);

            var (playerCompleted, playerTotal) = TasksHandler.taskInfo(Fink.fink.Data);
            int numberOfTasks = playerTotal - playerCompleted;

            if (numberOfTasks <= Fink.taskCountForImpostors && (PlayerControl.LocalPlayer.Data.Role.IsImpostor || (Fink.includeTeamRenegade && (PlayerControl.LocalPlayer == Renegade.renegade || PlayerControl.LocalPlayer == Minion.minion)))) {
                if (Fink.localArrows.Count == 0) Fink.localArrows.Add(new Arrow(Fink.color));
                if (Fink.localArrows.Count != 0 && Fink.localArrows[0] != null) {
                    Fink.localArrows[0].arrow.SetActive(true);
                    Fink.localArrows[0].Update(Fink.fink.transform.position);
                }
            }
            else if (PlayerControl.LocalPlayer == Fink.fink && numberOfTasks == 0 && !Challenger.isDueling && !Seeker.isMinigaming && !isHappeningAnonymousComms) {
                int arrowIndex = 0;
                foreach (PlayerControl p in PlayerControl.AllPlayerControls) {
                    bool arrowForImp = p.Data.Role.IsImpostor;
                    bool arrowForTeamRenegade = Fink.includeTeamRenegade && (p == Renegade.renegade || p == Minion.minion);

                    if (!p.Data.IsDead && (arrowForImp || arrowForTeamRenegade)) {
                        if (arrowIndex >= Fink.localArrows.Count) {
                            Fink.localArrows.Add(new Arrow(Color.red));
                            if (p == Renegade.renegade || p == Minion.minion) Fink.localArrows.Add(new Arrow(Renegade.color));
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
            if (Welder.welder == null || Welder.welder != PlayerControl.LocalPlayer || ShipStatus.Instance == null || ShipStatus.Instance.AllVents == null) return;

            Vent target = null;
            Vector2 truePosition = PlayerControl.LocalPlayer.GetTruePosition();
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
        static void spiritualistAndNecromancerUpdate() {
            if (Spiritualist.revivedPlayer != null) {
                if (!Spiritualist.revivedPlayer.Data.IsDead && (PlayerControl.LocalPlayer.Data.Role.IsImpostor || PlayerControl.LocalPlayer == Renegade.renegade || PlayerControl.LocalPlayer == Minion.minion || PlayerControl.LocalPlayer == BountyHunter.bountyhunter || PlayerControl.LocalPlayer == Trapper.trapper || PlayerControl.LocalPlayer == Yinyanger.yinyanger || PlayerControl.LocalPlayer == Challenger.challenger || PlayerControl.LocalPlayer == Ninja.ninja || PlayerControl.LocalPlayer == Berserker.berserker || PlayerControl.LocalPlayer == Yandere.yandere || PlayerControl.LocalPlayer == Stranded.stranded || PlayerControl.LocalPlayer == Monja.monja)) {
                    if (Spiritualist.localSpiritArrows.Count == 0) Spiritualist.localSpiritArrows.Add(new Arrow(Spiritualist.color));
                    if (Spiritualist.localSpiritArrows.Count != 0 && Spiritualist.localSpiritArrows[0] != null) {
                        Spiritualist.localSpiritArrows[0].arrow.SetActive(true);
                        Spiritualist.localSpiritArrows[0].Update(Spiritualist.revivedPlayer.transform.position);
                    }
                }
                else {
                    if (Spiritualist.localSpiritArrows.Count != 0) {
                        Spiritualist.localSpiritArrows[0].arrow.SetActive(false);
                    }
                }
            }
            if (Necromancer.revivedPlayer != null) {
                if (!Necromancer.revivedPlayer.Data.IsDead && (PlayerControl.LocalPlayer.Data.Role.IsImpostor || PlayerControl.LocalPlayer == Renegade.renegade || PlayerControl.LocalPlayer == Minion.minion || PlayerControl.LocalPlayer == BountyHunter.bountyhunter || PlayerControl.LocalPlayer == Trapper.trapper || PlayerControl.LocalPlayer == Yinyanger.yinyanger || PlayerControl.LocalPlayer == Challenger.challenger || PlayerControl.LocalPlayer == Ninja.ninja || PlayerControl.LocalPlayer == Berserker.berserker || PlayerControl.LocalPlayer == Yandere.yandere || PlayerControl.LocalPlayer == Stranded.stranded || PlayerControl.LocalPlayer == Monja.monja)) {
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
        public static void vigilantUpdate() {
            if (Vigilant.vigilant == null || PlayerControl.LocalPlayer != Vigilant.vigilant || Vigilant.vigilant.Data.IsDead) return;
            var (playerCompleted, _) = TasksHandler.taskInfo(Vigilant.vigilant.Data);
            if (playerCompleted == Vigilant.rechargedTasks) {
                MessageWriter usedRechargeWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.VigilantAbilityUses, Hazel.SendOption.Reliable, -1);
                usedRechargeWriter.Write(2);
                AmongUsClient.Instance.FinishRpcImmediately(usedRechargeWriter);
                RPCProcedure.vigilantAbilityUses(2);
            }
        }
        static void hunterSetTarget() {
            if (Hunter.hunter == null || Hunter.hunter != PlayerControl.LocalPlayer) return;
            Hunter.currentTarget = setTarget();
            if (!Hunter.usedHunted) setPlayerOutline(Hunter.currentTarget, Hunter.color);
        }
        static void jinxSetTarget() {
            if (Jinx.jinx == null || Jinx.jinx != PlayerControl.LocalPlayer) return;
            Jinx.target = setTarget();
            setPlayerOutline(Jinx.target, Jinx.color);
            if (Jinx.target != null && Jinx.jinxedList.Any(p => p.Data.PlayerId == Jinx.target.Data.PlayerId)) Jinx.target = null; // Remove target if already Jinxed and didn't trigger the jinx
        }
        static void necromancerUpdate() {
            
        }        
        static void shyUpdate() {

            if (Shy.shy == null)
                return;

            // Handle closest player locate
            if (Shy.shy == PlayerControl.LocalPlayer && !Shy.shy.Data.IsDead) {
                if (Shy.timer >= 0f) {

                    PlayerControl result = null;
                    float num = Shy.shyArrowRange;

                    Vector2 truePosition = Shy.shy.GetTruePosition();
                    Il2CppSystem.Collections.Generic.List<GameData.PlayerInfo> allPlayers = GameData.Instance.AllPlayers;
                    for (int i = 0; i < allPlayers.Count; i++) {
                        GameData.PlayerInfo playerInfo = allPlayers[i];
                        if (!playerInfo.Disconnected && playerInfo.PlayerId != Shy.shy.PlayerId && !playerInfo.IsDead) {
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

                    if (result != null && Vector2.Distance(result.transform.position, Shy.shy.transform.position) < Shy.shyArrowRange) {
                        if (Shy.playerColor) {
                            Shy.arrow.Update(result.transform.position, Palette.PlayerColors[result.Data.DefaultOutfit.ColorId]);
                        }
                        else {
                            Shy.arrow.Update(result.transform.position, Shy.color);
                        }
                        Shy.arrow.arrow.SetActive(true);
                    }
                    else {
                        Shy.arrow.arrow.SetActive(false);
                    }
                }
                else {
                    Shy.arrow.arrow.SetActive(false);
                }
            }
        }
        static void jailerSetTarget() {
            if (Jailer.jailer == null || Jailer.jailer != PlayerControl.LocalPlayer) return;
            Jailer.currentTarget = setTarget();
            if (!Jailer.usedJail) setPlayerOutline(Jailer.currentTarget, Jailer.color);
        }        
        static void theChosenOneUpdate() {
            if (Modifiers.theChosenOne == null || Modifiers.theChosenOne != PlayerControl.LocalPlayer) return;

            // TheChosenOne report
            if (Modifiers.theChosenOne.Data.IsDead && !Modifiers.chosenOneReported) {
                Modifiers.chosenOneReportDelay -= Time.fixedDeltaTime;
                DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == Modifiers.theChosenOne.PlayerId)?.FirstOrDefault();
                if (deadPlayer.killerIfExisting != null && Modifiers.chosenOneReportDelay <= 0f) {
                    Modifiers.chosenOneReported = true;

                    if (!Monja.awakened) {
                        // Bomberman bomb reset when report the chosen one
                        if (Bomberman.bomberman != null && Bomberman.activeBomb == true) {
                            MessageWriter bombwriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FixBomb, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(bombwriter);
                            RPCProcedure.fixBomb();
                        }

                        Helpers.handleDemonBiteOnBodyReport(); // Manually call Demon handling, since the CmdReportDeadBody Prefix won't be called
                        RPCProcedure.uncheckedCmdReportDeadBody(deadPlayer.killerIfExisting.PlayerId, Modifiers.theChosenOne.PlayerId);

                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedCmdReportDeadBody, Hazel.SendOption.Reliable, -1);
                        writer.Write(deadPlayer.killerIfExisting.PlayerId);
                        writer.Write(Modifiers.theChosenOne.PlayerId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);

                        MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                        writermusic.Write(1);
                        AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                        RPCProcedure.changeMusic(1);
                    }
                }
            }
        }
        static void performerUpdate() {
            if (Modifiers.performer != null) {
                if (Modifiers.performerDuration > 0 && Modifiers.performer.Data.IsDead && !Modifiers.performerReported && (PlayerControl.LocalPlayer != Modifiers.performer && PlayerControl.LocalPlayer != Spiritualist.spiritualist && PlayerControl.LocalPlayer != TimeTraveler.timeTraveler)) {
                    if (Modifiers.performerLocalPerformerArrows.Count == 0) Modifiers.performerLocalPerformerArrows.Add(new Arrow(Modifiers.color));
                    if (Modifiers.performerLocalPerformerArrows.Count != 0 && Modifiers.performerLocalPerformerArrows[0] != null) {
                        Modifiers.performerLocalPerformerArrows[0].arrow.SetActive(true);
                        var bodyPerformer = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == Modifiers.performer.PlayerId);
                        Modifiers.performerLocalPerformerArrows[0].Update(bodyPerformer.transform.position);
                    }
                }
                else {
                    if (Modifiers.performerLocalPerformerArrows.Count != 0) {
                        Modifiers.performerLocalPerformerArrows[0].arrow.SetActive(false);

                    }
                }

                // Upon performer duration, stop the music and play bomb music if there's a bomb or normal task music
                if (Modifiers.performer.Data.IsDead && Modifiers.performerDuration <= 0 && !Modifiers.performerMusicStop && !Modifiers.performerReported && (PlayerControl.LocalPlayer != Spiritualist.spiritualist && PlayerControl.LocalPlayer != TimeTraveler.timeTraveler)) {
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
        
        static void captureTheFlagSetTarget() {

            if (!CaptureTheFlag.captureTheFlagMode || CaptureTheFlag.captureTheFlagMode && howmanygamemodesareon != 1)
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
            if (CaptureTheFlag.blueplayer01IsReviving) {
                untargetableRedPlayers.Add(CaptureTheFlag.blueplayer01);
                untargetableAllPlayers.Add(CaptureTheFlag.blueplayer01);
            }
            else {
                untargetableRedPlayers.Remove(CaptureTheFlag.blueplayer01);
                untargetableAllPlayers.Remove(CaptureTheFlag.blueplayer01);
            }
            if (CaptureTheFlag.blueplayer02IsReviving) {
                untargetableRedPlayers.Add(CaptureTheFlag.blueplayer02);
                untargetableAllPlayers.Add(CaptureTheFlag.blueplayer02);
            }
            else {
                untargetableRedPlayers.Remove(CaptureTheFlag.blueplayer02);
                untargetableAllPlayers.Remove(CaptureTheFlag.blueplayer02);
            }
            if (CaptureTheFlag.blueplayer03IsReviving) {
                untargetableRedPlayers.Add(CaptureTheFlag.blueplayer03);
                untargetableAllPlayers.Add(CaptureTheFlag.blueplayer03);
            }
            else {
                untargetableRedPlayers.Remove(CaptureTheFlag.blueplayer03);
                untargetableAllPlayers.Remove(CaptureTheFlag.blueplayer03);
            }
            if (CaptureTheFlag.blueplayer04IsReviving) {
                untargetableRedPlayers.Add(CaptureTheFlag.blueplayer04);
                untargetableAllPlayers.Add(CaptureTheFlag.blueplayer04);
            }
            else {
                untargetableRedPlayers.Remove(CaptureTheFlag.blueplayer04);
                untargetableAllPlayers.Remove(CaptureTheFlag.blueplayer04);
            }
            if (CaptureTheFlag.blueplayer05IsReviving) {
                untargetableRedPlayers.Add(CaptureTheFlag.blueplayer05);
                untargetableAllPlayers.Add(CaptureTheFlag.blueplayer05);
            }
            else {
                untargetableRedPlayers.Remove(CaptureTheFlag.blueplayer05);
                untargetableAllPlayers.Remove(CaptureTheFlag.blueplayer05);
            }
            if (CaptureTheFlag.blueplayer06IsReviving) {
                untargetableRedPlayers.Add(CaptureTheFlag.blueplayer06);
                untargetableAllPlayers.Add(CaptureTheFlag.blueplayer06);
            }
            else {
                untargetableRedPlayers.Remove(CaptureTheFlag.blueplayer06);
                untargetableAllPlayers.Remove(CaptureTheFlag.blueplayer06);
            }
            if (CaptureTheFlag.blueplayer07IsReviving) {
                untargetableRedPlayers.Add(CaptureTheFlag.blueplayer07);
                untargetableAllPlayers.Add(CaptureTheFlag.blueplayer07);
            }
            else {
                untargetableRedPlayers.Remove(CaptureTheFlag.blueplayer07);
                untargetableAllPlayers.Remove(CaptureTheFlag.blueplayer07);
            }
            if (CaptureTheFlag.stealerPlayerIsReviving) {
                untargetableRedPlayers.Add(CaptureTheFlag.stealerPlayer);
                untargetableBluePlayers.Add(CaptureTheFlag.stealerPlayer);
            }
            else {
                untargetableRedPlayers.Remove(CaptureTheFlag.stealerPlayer);
                untargetableBluePlayers.Remove(CaptureTheFlag.stealerPlayer);
            }

            if (CaptureTheFlag.redplayer01 != null && CaptureTheFlag.redplayer01 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.redplayer01currentTarget = setTarget(untargetablePlayers: untargetableRedPlayers);
                setPlayerOutline(CaptureTheFlag.redplayer01currentTarget, Palette.ImpostorRed);
            }
            if (CaptureTheFlag.redplayer02 != null && CaptureTheFlag.redplayer02 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.redplayer02currentTarget = setTarget(untargetablePlayers: untargetableRedPlayers);
                setPlayerOutline(CaptureTheFlag.redplayer02currentTarget, Palette.ImpostorRed);
            }
            if (CaptureTheFlag.redplayer03 != null && CaptureTheFlag.redplayer03 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.redplayer03currentTarget = setTarget(untargetablePlayers: untargetableRedPlayers);
                setPlayerOutline(CaptureTheFlag.redplayer03currentTarget, Palette.ImpostorRed);
            }
            if (CaptureTheFlag.redplayer04 != null && CaptureTheFlag.redplayer04 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.redplayer04currentTarget = setTarget(untargetablePlayers: untargetableRedPlayers);
                setPlayerOutline(CaptureTheFlag.redplayer04currentTarget, Palette.ImpostorRed);
            }
            if (CaptureTheFlag.redplayer05 != null && CaptureTheFlag.redplayer05 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.redplayer05currentTarget = setTarget(untargetablePlayers: untargetableRedPlayers);
                setPlayerOutline(CaptureTheFlag.redplayer05currentTarget, Palette.ImpostorRed);
            }
            if (CaptureTheFlag.redplayer06 != null && CaptureTheFlag.redplayer06 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.redplayer06currentTarget = setTarget(untargetablePlayers: untargetableRedPlayers);
                setPlayerOutline(CaptureTheFlag.redplayer06currentTarget, Palette.ImpostorRed);
            }
            if (CaptureTheFlag.redplayer07 != null && CaptureTheFlag.redplayer07 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.redplayer07currentTarget = setTarget(untargetablePlayers: untargetableRedPlayers);
                setPlayerOutline(CaptureTheFlag.redplayer07currentTarget, Palette.ImpostorRed);
            }

            // Prevent killing reviving players
            if (CaptureTheFlag.redplayer01IsReviving) {
                untargetableBluePlayers.Add(CaptureTheFlag.redplayer01);
                untargetableAllPlayers.Add(CaptureTheFlag.redplayer01);
            }
            else {
                untargetableBluePlayers.Remove(CaptureTheFlag.redplayer01);
                untargetableAllPlayers.Remove(CaptureTheFlag.redplayer01);
            }
            if (CaptureTheFlag.redplayer02IsReviving) {
                untargetableBluePlayers.Add(CaptureTheFlag.redplayer02);
                untargetableAllPlayers.Add(CaptureTheFlag.redplayer02);
            }
            else {
                untargetableBluePlayers.Remove(CaptureTheFlag.redplayer02);
                untargetableAllPlayers.Remove(CaptureTheFlag.redplayer02);
            }
            if (CaptureTheFlag.redplayer03IsReviving) {
                untargetableBluePlayers.Add(CaptureTheFlag.redplayer03);
                untargetableAllPlayers.Add(CaptureTheFlag.redplayer03);
            }
            else {
                untargetableBluePlayers.Remove(CaptureTheFlag.redplayer03);
                untargetableAllPlayers.Remove(CaptureTheFlag.redplayer03);
            }
            if (CaptureTheFlag.redplayer04IsReviving) {
                untargetableBluePlayers.Add(CaptureTheFlag.redplayer04);
                untargetableAllPlayers.Add(CaptureTheFlag.redplayer04);
            }
            else {
                untargetableBluePlayers.Remove(CaptureTheFlag.redplayer04);
                untargetableAllPlayers.Remove(CaptureTheFlag.redplayer04);
            }
            if (CaptureTheFlag.redplayer05IsReviving) {
                untargetableBluePlayers.Add(CaptureTheFlag.redplayer05);
                untargetableAllPlayers.Add(CaptureTheFlag.redplayer05);
            }
            else {
                untargetableBluePlayers.Remove(CaptureTheFlag.redplayer05);
                untargetableAllPlayers.Remove(CaptureTheFlag.redplayer05);
            }
            if (CaptureTheFlag.redplayer06IsReviving) {
                untargetableBluePlayers.Add(CaptureTheFlag.redplayer06);
                untargetableAllPlayers.Add(CaptureTheFlag.redplayer06);
            }
            else {
                untargetableBluePlayers.Remove(CaptureTheFlag.redplayer06);
                untargetableAllPlayers.Remove(CaptureTheFlag.redplayer06);
            }
            if (CaptureTheFlag.redplayer07IsReviving) {
                untargetableBluePlayers.Add(CaptureTheFlag.redplayer07);
                untargetableAllPlayers.Add(CaptureTheFlag.redplayer07);
            }
            else {
                untargetableBluePlayers.Remove(CaptureTheFlag.redplayer07);
                untargetableAllPlayers.Remove(CaptureTheFlag.redplayer07);
            }

            if (CaptureTheFlag.blueplayer01 != null && CaptureTheFlag.blueplayer01 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.blueplayer01currentTarget = setTarget(untargetablePlayers: untargetableBluePlayers);
                setPlayerOutline(CaptureTheFlag.blueplayer01currentTarget, Color.blue);
            }
            if (CaptureTheFlag.blueplayer02 != null && CaptureTheFlag.blueplayer02 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.blueplayer02currentTarget = setTarget(untargetablePlayers: untargetableBluePlayers);
                setPlayerOutline(CaptureTheFlag.blueplayer02currentTarget, Color.blue);
            }
            if (CaptureTheFlag.blueplayer03 != null && CaptureTheFlag.blueplayer03 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.blueplayer03currentTarget = setTarget(untargetablePlayers: untargetableBluePlayers);
                setPlayerOutline(CaptureTheFlag.blueplayer03currentTarget, Color.blue);
            }
            if (CaptureTheFlag.blueplayer04 != null && CaptureTheFlag.blueplayer04 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.blueplayer04currentTarget = setTarget(untargetablePlayers: untargetableBluePlayers);
                setPlayerOutline(CaptureTheFlag.blueplayer04currentTarget, Color.blue);
            }
            if (CaptureTheFlag.blueplayer05 != null && CaptureTheFlag.blueplayer05 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.blueplayer05currentTarget = setTarget(untargetablePlayers: untargetableBluePlayers);
                setPlayerOutline(CaptureTheFlag.blueplayer05currentTarget, Color.blue);
            }
            if (CaptureTheFlag.blueplayer06 != null && CaptureTheFlag.blueplayer06 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.blueplayer06currentTarget = setTarget(untargetablePlayers: untargetableBluePlayers);
                setPlayerOutline(CaptureTheFlag.blueplayer06currentTarget, Color.blue);
            }
            if (CaptureTheFlag.blueplayer07 != null && CaptureTheFlag.blueplayer07 == PlayerControl.LocalPlayer) {
                CaptureTheFlag.blueplayer07currentTarget = setTarget(untargetablePlayers: untargetableBluePlayers);
                setPlayerOutline(CaptureTheFlag.blueplayer07currentTarget, Color.blue);
            }
            if (CaptureTheFlag.stealerPlayer != null && CaptureTheFlag.stealerPlayer == PlayerControl.LocalPlayer) {
                CaptureTheFlag.stealerPlayercurrentTarget = setTarget(untargetablePlayers: untargetableAllPlayers);
                setPlayerOutline(CaptureTheFlag.stealerPlayercurrentTarget, Color.grey);
            }
        }

        static void policeandThiefSetTarget() {

            if (!PoliceAndThief.policeAndThiefMode || PoliceAndThief.policeAndThiefMode && howmanygamemodesareon != 1)
                return;

            var untargetablePolice = new List<PlayerControl>();
            foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                untargetablePolice.Add(player);
            }

            // Prevent killing reviving players
            if (PoliceAndThief.thiefplayer01IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer01);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer01);
            }
            if (PoliceAndThief.thiefplayer02IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer02);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer02);
            }
            if (PoliceAndThief.thiefplayer03IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer03);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer03);
            }
            if (PoliceAndThief.thiefplayer04IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer04);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer04);
            }
            if (PoliceAndThief.thiefplayer05IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer05);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer05);
            }
            if (PoliceAndThief.thiefplayer06IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer06);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer06);
            }
            if (PoliceAndThief.thiefplayer07IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer07);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer07);
            }
            if (PoliceAndThief.thiefplayer08IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer08);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer08);
            }
            if (PoliceAndThief.thiefplayer09IsReviving) {
                untargetablePolice.Add(PoliceAndThief.thiefplayer09);
            }
            else {
                untargetablePolice.Remove(PoliceAndThief.thiefplayer09);
            }

            if (PoliceAndThief.policeplayer01 != null && PoliceAndThief.policeplayer01 == PlayerControl.LocalPlayer) {
                PoliceAndThief.policeplayer01currentTarget = setTarget(untargetablePlayers: untargetablePolice);
                setPlayerOutline(PoliceAndThief.policeplayer01currentTarget, Cheater.color);
            }
            if (PoliceAndThief.policeplayer02 != null && PoliceAndThief.policeplayer02 == PlayerControl.LocalPlayer) {
                PoliceAndThief.policeplayer02currentTarget = setTarget(untargetablePlayers: untargetablePolice);
                setPlayerOutline(PoliceAndThief.policeplayer02currentTarget, Cheater.color);
            }
            if (PoliceAndThief.policeplayer03 != null && PoliceAndThief.policeplayer03 == PlayerControl.LocalPlayer) {
                PoliceAndThief.policeplayer03currentTarget = setTarget(untargetablePlayers: untargetablePolice);
                setPlayerOutline(PoliceAndThief.policeplayer03currentTarget, Cheater.color);
            }
            if (PoliceAndThief.policeplayer04 != null && PoliceAndThief.policeplayer04 == PlayerControl.LocalPlayer) {
                PoliceAndThief.policeplayer04currentTarget = setTarget(untargetablePlayers: untargetablePolice);
                setPlayerOutline(PoliceAndThief.policeplayer04currentTarget, Cheater.color);
            }
            if (PoliceAndThief.policeplayer05 != null && PoliceAndThief.policeplayer05 == PlayerControl.LocalPlayer) {
                PoliceAndThief.policeplayer05currentTarget = setTarget(untargetablePlayers: untargetablePolice);
                setPlayerOutline(PoliceAndThief.policeplayer05currentTarget, Cheater.color);
            }
            if (PoliceAndThief.policeplayer06 != null && PoliceAndThief.policeplayer06 == PlayerControl.LocalPlayer) {
                PoliceAndThief.policeplayer06currentTarget = setTarget(untargetablePlayers: untargetablePolice);
                setPlayerOutline(PoliceAndThief.policeplayer06currentTarget, Cheater.color);
            }

            var untargetableThiefs = new List<PlayerControl>();
            foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                untargetableThiefs.Add(player);
            }

            // Prevent killing reviving players
            if (PoliceAndThief.policeplayer01IsReviving) {
                untargetableThiefs.Add(PoliceAndThief.policeplayer01);
            }
            else {
                untargetableThiefs.Remove(PoliceAndThief.policeplayer01);
            }
            if (PoliceAndThief.policeplayer02IsReviving) {
                untargetableThiefs.Add(PoliceAndThief.policeplayer02);
            }
            else {
                untargetableThiefs.Remove(PoliceAndThief.policeplayer02);
            }
            if (PoliceAndThief.policeplayer03IsReviving) {
                untargetableThiefs.Add(PoliceAndThief.policeplayer03);
            }
            else {
                untargetableThiefs.Remove(PoliceAndThief.policeplayer03);
            }
            if (PoliceAndThief.policeplayer04IsReviving) {
                untargetableThiefs.Add(PoliceAndThief.policeplayer04);
            }
            else {
                untargetableThiefs.Remove(PoliceAndThief.policeplayer04);
            }
            if (PoliceAndThief.policeplayer05IsReviving) {
                untargetableThiefs.Add(PoliceAndThief.policeplayer05);
            }
            else {
                untargetableThiefs.Remove(PoliceAndThief.policeplayer05);
            }
            if (PoliceAndThief.policeplayer06IsReviving) {
                untargetableThiefs.Add(PoliceAndThief.policeplayer06);
            }
            else {
                untargetableThiefs.Remove(PoliceAndThief.policeplayer06);
            }

            if (PoliceAndThief.thiefplayer01 != null && PoliceAndThief.thiefplayer01 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer01currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer01currentTarget, Mechanic.color);
            }
            if (PoliceAndThief.thiefplayer02 != null && PoliceAndThief.thiefplayer02 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer02currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer02currentTarget, Mechanic.color);
            }
            if (PoliceAndThief.thiefplayer03 != null && PoliceAndThief.thiefplayer03 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer03currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer03currentTarget, Mechanic.color);
            }
            if (PoliceAndThief.thiefplayer04 != null && PoliceAndThief.thiefplayer04 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer04currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer04currentTarget, Mechanic.color);
            }
            if (PoliceAndThief.thiefplayer05 != null && PoliceAndThief.thiefplayer05 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer05currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer05currentTarget, Mechanic.color);
            }
            if (PoliceAndThief.thiefplayer06 != null && PoliceAndThief.thiefplayer06 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer06currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer06currentTarget, Mechanic.color);
            }
            if (PoliceAndThief.thiefplayer07 != null && PoliceAndThief.thiefplayer07 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer07currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer07currentTarget, Mechanic.color);
            }
            if (PoliceAndThief.thiefplayer08 != null && PoliceAndThief.thiefplayer08 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer08currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer08currentTarget, Mechanic.color);
            }
            if (PoliceAndThief.thiefplayer09 != null && PoliceAndThief.thiefplayer09 == PlayerControl.LocalPlayer) {
                PoliceAndThief.thiefplayer09currentTarget = setTarget(untargetablePlayers: untargetableThiefs);
                setPlayerOutline(PoliceAndThief.thiefplayer09currentTarget, Mechanic.color);
            }
        }

        static void kingOfTheHillSetTarget() {

            if (!KingOfTheHill.kingOfTheHillMode || KingOfTheHill.kingOfTheHillMode && howmanygamemodesareon != 1)
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
            if (KingOfTheHill.yellowplayer01IsReviving) {
                untargetableGreenPlayers.Add(KingOfTheHill.yellowplayer01);
                untargetableAllPlayers.Add(KingOfTheHill.yellowplayer01);
            }
            else {
                untargetableGreenPlayers.Remove(KingOfTheHill.yellowplayer01);
                untargetableAllPlayers.Remove(KingOfTheHill.yellowplayer01);
            }
            if (KingOfTheHill.yellowplayer02IsReviving) {
                untargetableGreenPlayers.Add(KingOfTheHill.yellowplayer02);
                untargetableAllPlayers.Add(KingOfTheHill.yellowplayer02);
            }
            else {
                untargetableGreenPlayers.Remove(KingOfTheHill.yellowplayer02);
                untargetableAllPlayers.Remove(KingOfTheHill.yellowplayer02);
            }
            if (KingOfTheHill.yellowplayer03IsReviving) {
                untargetableGreenPlayers.Add(KingOfTheHill.yellowplayer03);
                untargetableAllPlayers.Add(KingOfTheHill.yellowplayer03);
            }
            else {
                untargetableGreenPlayers.Remove(KingOfTheHill.yellowplayer03);
                untargetableAllPlayers.Remove(KingOfTheHill.yellowplayer03);
            }
            if (KingOfTheHill.yellowplayer04IsReviving) {
                untargetableGreenPlayers.Add(KingOfTheHill.yellowplayer04);
                untargetableAllPlayers.Add(KingOfTheHill.yellowplayer04);
            }
            else {
                untargetableGreenPlayers.Remove(KingOfTheHill.yellowplayer04);
                untargetableAllPlayers.Remove(KingOfTheHill.yellowplayer04);
            }
            if (KingOfTheHill.yellowplayer05IsReviving) {
                untargetableGreenPlayers.Add(KingOfTheHill.yellowplayer05);
                untargetableAllPlayers.Add(KingOfTheHill.yellowplayer05);
            }
            else {
                untargetableGreenPlayers.Remove(KingOfTheHill.yellowplayer05);
                untargetableAllPlayers.Remove(KingOfTheHill.yellowplayer05);
            }
            if (KingOfTheHill.yellowplayer06IsReviving) {
                untargetableGreenPlayers.Add(KingOfTheHill.yellowplayer06);
                untargetableAllPlayers.Add(KingOfTheHill.yellowplayer06);
            }
            else {
                untargetableGreenPlayers.Remove(KingOfTheHill.yellowplayer06);
                untargetableAllPlayers.Remove(KingOfTheHill.yellowplayer06);
            }
            if (KingOfTheHill.yellowKingIsReviving) {
                untargetableGreenPlayers.Add(KingOfTheHill.yellowKingplayer);
                untargetableAllPlayers.Add(KingOfTheHill.yellowKingplayer);
            }
            else {
                untargetableGreenPlayers.Remove(KingOfTheHill.yellowKingplayer);
                untargetableAllPlayers.Remove(KingOfTheHill.yellowKingplayer);
            }
            if (KingOfTheHill.usurperPlayerIsReviving) {
                untargetableGreenPlayers.Add(KingOfTheHill.usurperPlayer);
                untargetableYellowPlayers.Add(KingOfTheHill.usurperPlayer);
            }
            else {
                untargetableGreenPlayers.Remove(KingOfTheHill.usurperPlayer);
                untargetableYellowPlayers.Remove(KingOfTheHill.usurperPlayer);
            }

            if (KingOfTheHill.greenKingplayer != null && KingOfTheHill.greenKingplayer == PlayerControl.LocalPlayer) {
                KingOfTheHill.greenKingplayercurrentTarget = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(KingOfTheHill.greenKingplayercurrentTarget, Color.green);
            }
            if (KingOfTheHill.greenplayer01 != null && KingOfTheHill.greenplayer01 == PlayerControl.LocalPlayer) {
                KingOfTheHill.greenplayer01currentTarget = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(KingOfTheHill.greenplayer01currentTarget, Color.green);
            }
            if (KingOfTheHill.greenplayer02 != null && KingOfTheHill.greenplayer02 == PlayerControl.LocalPlayer) {
                KingOfTheHill.greenplayer02currentTarget = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(KingOfTheHill.greenplayer02currentTarget, Color.green);
            }
            if (KingOfTheHill.greenplayer03 != null && KingOfTheHill.greenplayer03 == PlayerControl.LocalPlayer) {
                KingOfTheHill.greenplayer03currentTarget = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(KingOfTheHill.greenplayer03currentTarget, Color.green);
            }
            if (KingOfTheHill.greenplayer04 != null && KingOfTheHill.greenplayer04 == PlayerControl.LocalPlayer) {
                KingOfTheHill.greenplayer04currentTarget = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(KingOfTheHill.greenplayer04currentTarget, Color.green);
            }
            if (KingOfTheHill.greenplayer05 != null && KingOfTheHill.greenplayer05 == PlayerControl.LocalPlayer) {
                KingOfTheHill.greenplayer05currentTarget = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(KingOfTheHill.greenplayer05currentTarget, Color.green);
            }
            if (KingOfTheHill.greenplayer06 != null && KingOfTheHill.greenplayer06 == PlayerControl.LocalPlayer) {
                KingOfTheHill.greenplayer06currentTarget = setTarget(untargetablePlayers: untargetableGreenPlayers);
                setPlayerOutline(KingOfTheHill.greenplayer06currentTarget, Color.green);
            }

            // Prevent killing reviving players
            if (KingOfTheHill.greenplayer01IsReviving) {
                untargetableYellowPlayers.Add(KingOfTheHill.greenplayer01);
                untargetableAllPlayers.Add(KingOfTheHill.greenplayer01);
            }
            else {
                untargetableYellowPlayers.Remove(KingOfTheHill.greenplayer01);
                untargetableAllPlayers.Remove(KingOfTheHill.greenplayer01);
            }
            if (KingOfTheHill.greenplayer02IsReviving) {
                untargetableYellowPlayers.Add(KingOfTheHill.greenplayer02);
                untargetableAllPlayers.Add(KingOfTheHill.greenplayer02);
            }
            else {
                untargetableYellowPlayers.Remove(KingOfTheHill.greenplayer02);
                untargetableAllPlayers.Remove(KingOfTheHill.greenplayer02);
            }
            if (KingOfTheHill.greenplayer03IsReviving) {
                untargetableYellowPlayers.Add(KingOfTheHill.greenplayer03);
                untargetableAllPlayers.Add(KingOfTheHill.greenplayer03);
            }
            else {
                untargetableYellowPlayers.Remove(KingOfTheHill.greenplayer03);
                untargetableAllPlayers.Remove(KingOfTheHill.greenplayer03);
            }
            if (KingOfTheHill.greenplayer04IsReviving) {
                untargetableYellowPlayers.Add(KingOfTheHill.greenplayer04);
                untargetableAllPlayers.Add(KingOfTheHill.greenplayer04);
            }
            else {
                untargetableYellowPlayers.Remove(KingOfTheHill.greenplayer04);
                untargetableAllPlayers.Remove(KingOfTheHill.greenplayer04);
            }
            if (KingOfTheHill.greenplayer05IsReviving) {
                untargetableYellowPlayers.Add(KingOfTheHill.greenplayer05);
                untargetableAllPlayers.Add(KingOfTheHill.greenplayer05);
            }
            else {
                untargetableYellowPlayers.Remove(KingOfTheHill.greenplayer05);
                untargetableAllPlayers.Remove(KingOfTheHill.greenplayer05);
            }
            if (KingOfTheHill.greenplayer06IsReviving) {
                untargetableYellowPlayers.Add(KingOfTheHill.greenplayer06);
                untargetableAllPlayers.Add(KingOfTheHill.greenplayer06);
            }
            else {
                untargetableYellowPlayers.Remove(KingOfTheHill.greenplayer06);
                untargetableAllPlayers.Remove(KingOfTheHill.greenplayer06);
            }
            if (KingOfTheHill.greenKingIsReviving) {
                untargetableYellowPlayers.Add(KingOfTheHill.greenKingplayer);
                untargetableAllPlayers.Add(KingOfTheHill.greenKingplayer);
            }
            else {
                untargetableYellowPlayers.Remove(KingOfTheHill.greenKingplayer);
                untargetableAllPlayers.Remove(KingOfTheHill.greenKingplayer);
            }

            if (KingOfTheHill.yellowKingplayer != null && KingOfTheHill.yellowKingplayer == PlayerControl.LocalPlayer) {
                KingOfTheHill.yellowKingplayercurrentTarget = setTarget(untargetablePlayers: untargetableYellowPlayers);
                setPlayerOutline(KingOfTheHill.yellowKingplayercurrentTarget, Color.yellow);
            }
            if (KingOfTheHill.yellowplayer01 != null && KingOfTheHill.yellowplayer01 == PlayerControl.LocalPlayer) {
                KingOfTheHill.yellowplayer01currentTarget = setTarget(untargetablePlayers: untargetableYellowPlayers);
                setPlayerOutline(KingOfTheHill.yellowplayer01currentTarget, Color.yellow);
            }
            if (KingOfTheHill.yellowplayer02 != null && KingOfTheHill.yellowplayer02 == PlayerControl.LocalPlayer) {
                KingOfTheHill.yellowplayer02currentTarget = setTarget(untargetablePlayers: untargetableYellowPlayers);
                setPlayerOutline(KingOfTheHill.yellowplayer02currentTarget, Color.yellow);
            }
            if (KingOfTheHill.yellowplayer03 != null && KingOfTheHill.yellowplayer03 == PlayerControl.LocalPlayer) {
                KingOfTheHill.yellowplayer03currentTarget = setTarget(untargetablePlayers: untargetableYellowPlayers);
                setPlayerOutline(KingOfTheHill.yellowplayer03currentTarget, Color.yellow);
            }
            if (KingOfTheHill.yellowplayer04 != null && KingOfTheHill.yellowplayer04 == PlayerControl.LocalPlayer) {
                KingOfTheHill.yellowplayer04currentTarget = setTarget(untargetablePlayers: untargetableYellowPlayers);
                setPlayerOutline(KingOfTheHill.yellowplayer04currentTarget, Color.yellow);
            }
            if (KingOfTheHill.yellowplayer05 != null && KingOfTheHill.yellowplayer05 == PlayerControl.LocalPlayer) {
                KingOfTheHill.yellowplayer05currentTarget = setTarget(untargetablePlayers: untargetableYellowPlayers);
                setPlayerOutline(KingOfTheHill.yellowplayer05currentTarget, Color.yellow);
            }
            if (KingOfTheHill.yellowplayer06 != null && KingOfTheHill.yellowplayer06 == PlayerControl.LocalPlayer) {
                KingOfTheHill.yellowplayer06currentTarget = setTarget(untargetablePlayers: untargetableYellowPlayers);
                setPlayerOutline(KingOfTheHill.yellowplayer06currentTarget, Color.yellow);
            }

            if (KingOfTheHill.usurperPlayer != null && KingOfTheHill.usurperPlayer == PlayerControl.LocalPlayer) {
                KingOfTheHill.usurperPlayercurrentTarget = setTarget(untargetablePlayers: untargetableAllPlayers);
                setPlayerOutline(KingOfTheHill.usurperPlayercurrentTarget, Color.grey);
            }
        }

        static void hotPotatoSetTarget() {

            if (!HotPotato.hotPotatoMode || HotPotato.hotPotatoMode && howmanygamemodesareon != 1)
                return;

            if (HotPotato.hotPotatoPlayer != null && HotPotato.hotPotatoPlayer == PlayerControl.LocalPlayer) {
                HotPotato.hotPotatoPlayerCurrentTarget = setTarget();
                setPlayerOutline(HotPotato.hotPotatoPlayerCurrentTarget, Color.grey);
            }
        }

        static void zombieLaboratorySetTarget() {

            if (!ZombieLaboratory.zombieLaboratoryMode || ZombieLaboratory.zombieLaboratoryMode && howmanygamemodesareon != 1)
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
            if (ZombieLaboratory.survivorPlayer01IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer01);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer01);
            }
            if (ZombieLaboratory.survivorPlayer02IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer02);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer02);
            }
            if (ZombieLaboratory.survivorPlayer03IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer03);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer03);
            }
            if (ZombieLaboratory.survivorPlayer04IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer04);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer04);
            }
            if (ZombieLaboratory.survivorPlayer05IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer05);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer05);
            }
            if (ZombieLaboratory.survivorPlayer06IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer06);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer06);
            }
            if (ZombieLaboratory.survivorPlayer07IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer07);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer07);
            }
            if (ZombieLaboratory.survivorPlayer08IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer08);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer08);
            }
            if (ZombieLaboratory.survivorPlayer09IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer09);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer09);
            }
            if (ZombieLaboratory.survivorPlayer10IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer10);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer10);
            }
            if (ZombieLaboratory.survivorPlayer11IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer11);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer11);
            }
            if (ZombieLaboratory.survivorPlayer12IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer12);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer12);
            }
            if (ZombieLaboratory.survivorPlayer13IsReviving) {
                untargetableZombiePlayers.Add(ZombieLaboratory.survivorPlayer13);
            }
            else {
                untargetableZombiePlayers.Remove(ZombieLaboratory.survivorPlayer13);
            }

            if (ZombieLaboratory.nursePlayer != null && ZombieLaboratory.nursePlayer == PlayerControl.LocalPlayer) {
                ZombieLaboratory.nursePlayercurrentTarget = setTarget();
                setPlayerOutline(ZombieLaboratory.nursePlayercurrentTarget, Shy.color);
            }
            if (ZombieLaboratory.survivorPlayer01 != null && ZombieLaboratory.survivorPlayer01 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer01currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer01currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer02 != null && ZombieLaboratory.survivorPlayer02 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer02currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer02currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer03 != null && ZombieLaboratory.survivorPlayer03 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer03currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer03currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer04 != null && ZombieLaboratory.survivorPlayer04 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer04currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer04currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer05 != null && ZombieLaboratory.survivorPlayer05 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer05currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer05currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer06 != null && ZombieLaboratory.survivorPlayer06 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer06currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer06currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer07 != null && ZombieLaboratory.survivorPlayer07 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer07currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer07currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer08 != null && ZombieLaboratory.survivorPlayer08 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer08currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer08currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer09 != null && ZombieLaboratory.survivorPlayer09 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer09currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer09currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer10 != null && ZombieLaboratory.survivorPlayer10 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer10currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer10currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer11 != null && ZombieLaboratory.survivorPlayer11 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer11currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer11currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer12 != null && ZombieLaboratory.survivorPlayer12 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer12currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer12currentTarget, Color.cyan);
            }
            if (ZombieLaboratory.survivorPlayer13 != null && ZombieLaboratory.survivorPlayer13 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.survivorPlayer13currentTarget = setTarget(untargetablePlayers: untargetableSurvivorsPlayers);
                setPlayerOutline(ZombieLaboratory.survivorPlayer13currentTarget, Color.cyan);
            }

            // Prevent killing reviving players
            if (ZombieLaboratory.zombiePlayer01IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer01);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer01);
            }
            if (ZombieLaboratory.zombiePlayer02IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer02);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer02);
            }
            if (ZombieLaboratory.zombiePlayer03IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer03);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer03);
            }
            if (ZombieLaboratory.zombiePlayer04IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer04);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer04);
            }
            if (ZombieLaboratory.zombiePlayer05IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer05);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer05);
            }
            if (ZombieLaboratory.zombiePlayer06IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer06);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer06);
            }
            if (ZombieLaboratory.zombiePlayer07IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer07);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer07);
            }
            if (ZombieLaboratory.zombiePlayer08IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer08);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer08);
            }
            if (ZombieLaboratory.zombiePlayer09IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer09);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer09);
            }
            if (ZombieLaboratory.zombiePlayer10IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer10);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer10);
            }
            if (ZombieLaboratory.zombiePlayer11IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer11);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer11);
            }
            if (ZombieLaboratory.zombiePlayer12IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer12);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer12);
            }
            if (ZombieLaboratory.zombiePlayer13IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer13);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer13);
            }
            if (ZombieLaboratory.zombiePlayer14IsReviving) {
                untargetableSurvivorsPlayers.Add(ZombieLaboratory.zombiePlayer14);
            }
            else {
                untargetableSurvivorsPlayers.Remove(ZombieLaboratory.zombiePlayer14);
            }

            if (ZombieLaboratory.zombiePlayer01 != null && ZombieLaboratory.zombiePlayer01 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer01currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer01currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer02 != null && ZombieLaboratory.zombiePlayer02 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer02currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer02currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer03 != null && ZombieLaboratory.zombiePlayer03 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer03currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer03currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer04 != null && ZombieLaboratory.zombiePlayer04 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer04currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer04currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer05 != null && ZombieLaboratory.zombiePlayer05 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer05currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer05currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer06 != null && ZombieLaboratory.zombiePlayer06 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer06currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer06currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer07 != null && ZombieLaboratory.zombiePlayer07 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer07currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer07currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer08 != null && ZombieLaboratory.zombiePlayer08 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer08currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer08currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer09 != null && ZombieLaboratory.zombiePlayer09 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer09currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer09currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer10 != null && ZombieLaboratory.zombiePlayer10 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer10currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer10currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer11 != null && ZombieLaboratory.zombiePlayer11 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer11currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer11currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer12 != null && ZombieLaboratory.zombiePlayer12 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer12currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer12currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer13 != null && ZombieLaboratory.zombiePlayer13 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer13currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer13currentTarget, Sheriff.color);
            }
            if (ZombieLaboratory.zombiePlayer14 != null && ZombieLaboratory.zombiePlayer14 == PlayerControl.LocalPlayer) {
                ZombieLaboratory.zombiePlayer14currentTarget = setTarget(untargetablePlayers: untargetableZombiePlayers);
                setPlayerOutline(ZombieLaboratory.zombiePlayer14currentTarget, Sheriff.color);
            }

        }

        public static void Postfix(PlayerControl __instance) {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;

            if (PlayerControl.LocalPlayer == __instance) {
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

                // Poisoner
                poisonerSetTarget();

                // Puppeteer
                puppeteerSetTarget();

                // Seeker
                seekerSetTarget();

                // Sheriff
                sheriffSetTarget();

                // Detective
                detectiveUpdateFootPrints();

                // Forensic
                forensicSetTarget();

                // TimeTraveler
                bendTimeUpdate();

                // Squire
                squireSetTarget();

                // FortuneTeller
                fortuneTellerSetTarget();

                // Hacker
                hackerUpdate();

                // Sleuth
                sleuthSetTarget();
                sleuthUpdate();

                // Fink
                finkUpdate();       

                // Welder
                welderSetTarget();

                // Spiritualist and Necromancer Update
                spiritualistAndNecromancerUpdate();

                // Vigilant
                vigilantUpdate();

                // Hunter
                hunterSetTarget();

                // Jinx
                jinxSetTarget();

                // Shy
                shyUpdate();

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
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdReportDeadBody))]
    class PlayerControlCmdReportDeadBodyPatch {
        public static void Prefix(PlayerControl __instance) {
            // Bomberman bomb reset when report body
            if (Bomberman.bomberman != null && Bomberman.activeBomb == true) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.FixBomb, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.fixBomb();
            }

            //If music option is enabled and the player who used emergency button was not a bitten player
            if (PlayerControl.LocalPlayer != Demon.bitten || Demon.bitten == null) {
                MessageWriter writermusic = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.ChangeMusic, Hazel.SendOption.Reliable, -1);
                writermusic.Write(1);
                AmongUsClient.Instance.FinishRpcImmediately(writermusic);
                RPCProcedure.changeMusic(1);
            }

            // Murder the bitten player before the meeting starts or reset the bitten player
            Helpers.handleDemonBiteOnBodyReport();        
            
            // Murder the Spiritualist if someone reports a body or call emergency while trying to revive another player
            if (Spiritualist.spiritualist != null && Spiritualist.isReviving && Spiritualist.canRevive) {
                MessageWriter murderSpiritualist = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.MurderSpiritualistIfReportWhileReviving, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(murderSpiritualist);
                RPCProcedure.murderSpiritualistIfReportWhileReviving();
            }
            
            // Performer isreported
            if (Modifiers.performer != null && Modifiers.performer.Data.IsDead && !Modifiers.performerReported) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PerformerIsReported, Hazel.SendOption.Reliable, -1);
                writer.Write(0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.performerIsReported(0);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.LocalPlayer.CmdReportDeadBody))]
    class BodyReportPatch
    {
        static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] GameData.PlayerInfo target) {
            // Forensic report
            bool isForensicReport = Forensic.forensic != null && Forensic.forensic == PlayerControl.LocalPlayer && __instance.PlayerId == Forensic.forensic.PlayerId;
            if (isForensicReport) {
                DeadPlayer deadPlayer = deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();

                if (deadPlayer != null && deadPlayer.killerIfExisting != null) {
                    float timeSinceDeath = ((float)(DateTime.UtcNow - deadPlayer.timeOfDeath).TotalMilliseconds);
                    string msg = "";

                    if (isForensicReport) {
                        if (deadPlayer.player == RoleThief.rolethief && deadPlayer.killerIfExisting.Data.PlayerName == RoleThief.rolethief.Data.PlayerName) {
                            msg = $"Body Report (Role Thief): {Language.playerControlTexts[0]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                        }
                        else if (deadPlayer.player == BountyHunter.bountyhunter && deadPlayer.killerIfExisting.Data.PlayerName == BountyHunter.bountyhunter.Data.PlayerName) {
                            msg = $"Body Report (Bounty Hunter): {Language.playerControlTexts[0]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                        }
                        else if (deadPlayer.player == Modifiers.lover1 && deadPlayer.killerIfExisting.Data.PlayerName == Modifiers.lover1.Data.PlayerName || deadPlayer.player == Modifiers.lover2 && deadPlayer.killerIfExisting.Data.PlayerName == Modifiers.lover2.Data.PlayerName) {
                            msg = $"Body Report (Lover): {Language.playerControlTexts[0]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                        }
                        else if (deadPlayer.player == Sheriff.sheriff && deadPlayer.killerIfExisting.Data.PlayerName == Sheriff.sheriff.Data.PlayerName) {
                            msg = $"Body Report (Sheriff): {Language.playerControlTexts[0]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                        }
                        else if (timeSinceDeath < Forensic.reportNameDuration * 1000) {
                            msg = $"Body Report: {Language.playerControlTexts[1]} {deadPlayer.killerIfExisting.Data.PlayerName}! ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                        }
                        else if (timeSinceDeath < Forensic.reportColorDuration * 1000) {
                            var typeOfColor = Helpers.isLighterColor(deadPlayer.killerIfExisting.Data.DefaultOutfit.ColorId) ? Language.playerControlTexts[2] : Language.playerControlTexts[3];
                            msg = $"Body Report: {Language.playerControlTexts[4]} {typeOfColor} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                        }
                        else if (timeSinceDeath < Forensic.reportClueDuration * 1000) {
                            int randomClue = rnd.Next(1, 5);
                            switch (randomClue) {
                                case 1:
                                    if (deadPlayer.killerIfExisting.Data.DefaultOutfit.HatId != null) {
                                        msg = $"Body Report: {Language.playerControlTexts[5]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                    }
                                    else {
                                        msg = $"Body Report: {Language.playerControlTexts[6]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                    }
                                    break;
                                case 2:
                                    if (deadPlayer.killerIfExisting.Data.DefaultOutfit.SkinId != null) {
                                        msg = $"Body Report: {Language.playerControlTexts[7]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                    }
                                    else {
                                        msg = $"Body Report: {Language.playerControlTexts[8]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                    }
                                    break;
                                case 3:
                                    if (deadPlayer.killerIfExisting.Data.DefaultOutfit.PetId != null) {
                                        msg = $"Body Report: {Language.playerControlTexts[9]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                    }
                                    else {
                                        msg = $"Body Report: {Language.playerControlTexts[10]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                    }
                                    break;
                                case 4:
                                    if (deadPlayer.killerIfExisting.Data.DefaultOutfit.VisorId != null) {
                                        msg = $"Body Report: {Language.playerControlTexts[11]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                    }
                                    else {
                                        msg = $"Body Report: {Language.playerControlTexts[12]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                                    }
                                    break;
                            }
                        }
                        else {
                            msg = $"Body Report: {Language.playerControlTexts[13]} ({Language.playerControlTexts[14]} {Math.Round(timeSinceDeath / 1000)})";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(msg)) {
                        if (AmongUsClient.Instance.AmClient && DestroyableSingleton<HudManager>.Instance) {
                            DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, msg);
                        }
                        if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0) {
                            DestroyableSingleton<Assets.CoreScripts.Telemetry>.Instance.SendWho();
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

            if (PlayerControl.LocalPlayer == target && GameOptionsManager.Instance.currentGameMode == GameModes.Normal) {
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
                var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                body.transform.position = new Vector3(50, 50, 1);
                if (target == Monja.monja) {
                    RPCProcedure.monjaReset();
                }
            }

            // Lover suicide trigger on murder
            if ((Modifiers.lover1 != null && target == Modifiers.lover1) || (Modifiers.lover2 != null && target == Modifiers.lover2)) {
                PlayerControl otherLover = target == Modifiers.lover1 ? Modifiers.lover2 : Modifiers.lover1;
                if (otherLover != null && !otherLover.Data.IsDead) {
                    otherLover.MurderPlayer(otherLover);
                    if (Monja.awakened) {
                        var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == otherLover.PlayerId);
                        body.transform.position = new Vector3(50, 50, 1);
                    }
                }
            }

            // Kid trigger win on murder
            if (Kid.kid != null && target == Kid.kid) {
                Kid.triggerKidLose = true;
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.KidLose, false);
            }
            
            // Janitor Button Sync
            if (Janitor.janitor != null && PlayerControl.LocalPlayer == Janitor.janitor && __instance == Janitor.janitor && HudManagerStartPatch.janitorCleanButton != null)
                HudManagerStartPatch.janitorCleanButton.Timer = Janitor.janitor.killTimer;

            if (Janitor.janitor != null && target == Janitor.janitor && Janitor.dragginBody) {
                Janitor.janitorResetValuesAtDead();
            }

            // Manipulator Button Sync
            if (Manipulator.manipulator != null && PlayerControl.LocalPlayer == Manipulator.manipulator && __instance == Manipulator.manipulator && HudManagerStartPatch.manipulatorManipulateButton != null) {
                if (Manipulator.manipulator.killTimer > HudManagerStartPatch.manipulatorManipulateButton.Timer) {
                    HudManagerStartPatch.manipulatorManipulateButton.Timer = Manipulator.manipulator.killTimer;
                }
            }

            // Chameleon reset invisibility
            if (Chameleon.chameleon != null && target == Chameleon.chameleon) {
                Chameleon.resetChameleon();
            }
            
            // Sorcerer Button Sync
            if (Sorcerer.sorcerer != null && PlayerControl.LocalPlayer == Sorcerer.sorcerer && __instance == Sorcerer.sorcerer && HudManagerStartPatch.sorcererSpellButton != null)
                HudManagerStartPatch.sorcererSpellButton.Timer = HudManagerStartPatch.sorcererSpellButton.MaxTimer;

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
                    BountyHunter.bountyhunter.MurderPlayer(BountyHunter.bountyhunter);
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
                if (PlayerControl.LocalPlayer == Yandere.yandere) {
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
            if (Devourer.devourer != null && Devourer.devourer == PlayerControl.LocalPlayer && !Devourer.devourer.Data.IsDead) {
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
                        if (GameOptionsManager.Instance.currentGameOptions.MapId == 5) {
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
                if (PlayerControl.LocalPlayer == __instance) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.puppeteerClip, false, 75f);
                }

                if (PlayerControl.LocalPlayer != Puppeteer.puppeteer) return;

                if (Puppeteer.counter >= Puppeteer.numberOfKills) {
                    MessageWriter winWriter = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.PuppeteerWin, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(winWriter);
                    RPCProcedure.puppeteerWin();
                }
            }

            // Kill Exiler if the target is killed
            if (Exiler.exiler != null && Exiler.target != null && target == Exiler.target && !Exiler.exiler.Data.IsDead) {
                Exiler.exiler.MurderPlayer(Exiler.exiler);
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
            if (Vigilant.vigilantMira != null && target == Vigilant.vigilantMira) {
                GameObject vigilantdoorlog = GameObject.Find("VigilantDoorLog");
                if (vigilantdoorlog != null) {
                    vigilantdoorlog.SetActive(false);
                }
            }           

            // Hunter target suicide trigger on Hunter murder
            if (Hunter.hunter != null && target == Hunter.hunter) {
                if (Hunter.hunted != null && !Hunter.hunted.Data.IsDead) {
                    Hunter.hunted.MurderPlayer(Hunter.hunted);
                    Hunter.targetButtonText.text = $" ";
                }
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
                // Ace Attorney Music Stop and play theater music
                if (PlayerControl.LocalPlayer != Spiritualist.spiritualist && PlayerControl.LocalPlayer != TimeTraveler.timeTraveler) {
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
                if (PlayerControl.LocalPlayer == __instance) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.paintballDeath, false, 100f);
                }
                Modifiers.active = new Dictionary<byte, float>();
                Modifiers.paintballKillerMap = new Dictionary<byte, byte>();
                Modifiers.active.Add(__instance.PlayerId, Modifiers.paintballDuration);
                Modifiers.paintballKillerMap.Add(__instance.PlayerId, target.PlayerId);
            }

            // Electrician discharge trigger on death
            if (Modifiers.electrician != null && target == Modifiers.electrician) {
                if (PlayerControl.LocalPlayer == __instance) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.policeTaser, false, 100f);
                }
                new Tased(Modifiers.electricianDuration, __instance);
            }
            
            if (howmanygamemodesareon == 1) {
                // Capture the flag revive player
                if (CaptureTheFlag.captureTheFlagMode) {
                    // Capture the flag reset flag position if killed while having it
                    if (CaptureTheFlag.redPlayerWhoHasBlueFlag != null && target == CaptureTheFlag.redPlayerWhoHasBlueFlag) {
                        CaptureTheFlag.blueflagtaken = false;
                        CaptureTheFlag.blueteamAlerted = false;
                        CaptureTheFlag.redPlayerWhoHasBlueFlag = null;
                        CaptureTheFlag.blueflag.transform.parent = CaptureTheFlag.blueflagbase.transform.parent;
                        switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                            // Skeld
                            case 0:
                                if (activatedSensei) {
                                    CaptureTheFlag.blueflag.transform.position = new Vector3(7.7f, -1.15f, 0.5f);
                                }
                                else {
                                    CaptureTheFlag.blueflag.transform.position = new Vector3(16.5f, -4.65f, 0.5f);
                                }
                                break;
                            // MiraHQ
                            case 1:
                                CaptureTheFlag.blueflag.transform.position = new Vector3(23.25f, 5.05f, 0.5f);
                                break;
                            // Polus
                            case 2:
                                CaptureTheFlag.blueflag.transform.position = new Vector3(5.4f, -9.65f, 0.5f);
                                break;
                            // Dleks
                            case 3:
                                CaptureTheFlag.blueflag.transform.position = new Vector3(-16.5f, -4.65f, 0.5f);
                                break;
                            // Airship
                            case 4:
                                CaptureTheFlag.blueflag.transform.position = new Vector3(33.6f, 1.25f, 0.5f);
                                break;
                            // Submerged
                            case 5:
                                CaptureTheFlag.blueflag.transform.position = new Vector3(12.5f, -31.45f, -0.011f);
                                break;
                        }
                    }

                    if (CaptureTheFlag.bluePlayerWhoHasRedFlag != null && target == CaptureTheFlag.bluePlayerWhoHasRedFlag) {
                        CaptureTheFlag.redflagtaken = false;
                        CaptureTheFlag.redteamAlerted = false;
                        CaptureTheFlag.bluePlayerWhoHasRedFlag = null;
                        CaptureTheFlag.redflag.transform.parent = CaptureTheFlag.redflagbase.transform.parent;
                        switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                            // Skeld
                            case 0:
                                if (activatedSensei) {
                                    CaptureTheFlag.redflag.transform.position = new Vector3(-17.5f, -1.35f, 0.5f);
                                }
                                else {
                                    CaptureTheFlag.redflag.transform.position = new Vector3(-20.5f, -5.35f, 0.5f);
                                }
                                break;
                            // MiraHQ
                            case 1:
                                CaptureTheFlag.redflag.transform.position = new Vector3(2.525f, 10.55f, 0.5f);
                                break;
                            // Polus
                            case 2:
                                CaptureTheFlag.redflag.transform.position = new Vector3(36.4f, -21.7f, 0.5f);
                                break;
                            // Dlesk
                            case 3:
                                CaptureTheFlag.redflag.transform.position = new Vector3(20.5f, -5.35f, 0.5f);
                                break;
                            // Airship
                            case 4:
                                CaptureTheFlag.redflag.transform.position = new Vector3(-17.5f, -1.2f, 0.5f);
                                break;
                            // Submerged
                            case 5:
                                CaptureTheFlag.redflag.transform.position = new Vector3(-8.35f, 28.05f, 0.03f);
                                break;
                        }
                    }

                    // Capture the flag revive player
                    if (CaptureTheFlag.stealerPlayer != null && CaptureTheFlag.stealerPlayer.PlayerId == target.PlayerId) {
                        var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                        body.transform.position = new Vector3(50, 50, 1);
                        CaptureTheFlag.stealerPlayerIsReviving = true;
                        Helpers.alphaPlayer(true, CaptureTheFlag.stealerPlayer.PlayerId);
                        HudManager.Instance.StartCoroutine(Effects.Lerp(CaptureTheFlag.reviveTime, new Action<float>((p) => {
                            if (p == 1f && CaptureTheFlag.stealerPlayer != null) {
                                CaptureTheFlag.stealerPlayerIsReviving = false;
                                Helpers.alphaPlayer(false, CaptureTheFlag.stealerPlayer.PlayerId);
                            }
                        })));
                        HudManager.Instance.StartCoroutine(Effects.Lerp(CaptureTheFlag.reviveTime - CaptureTheFlag.invincibilityTimeAfterRevive, new Action<float>((p) => {
                            if (p == 1f && CaptureTheFlag.stealerPlayer != null) {
                                CaptureTheFlag.stealerPlayer.Revive();
                                switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                    // Skeld
                                    case 0:
                                        if (activatedSensei) {
                                            CaptureTheFlag.stealerPlayer.transform.position = new Vector3(-3.65f, 5f, CaptureTheFlag.stealerPlayer.transform.position.z);
                                        }
                                        else {
                                            CaptureTheFlag.stealerPlayer.transform.position = new Vector3(6.35f, -7.5f, CaptureTheFlag.stealerPlayer.transform.position.z);
                                        }
                                        break;
                                    // MiraHQ
                                    case 1:
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(17.75f, 24f, CaptureTheFlag.stealerPlayer.transform.position.z);
                                        break;
                                    // Polus
                                    case 2:
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(31.75f, -13f, CaptureTheFlag.stealerPlayer.transform.position.z);
                                        break;
                                    // Dleks
                                    case 3:
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(-6.35f, -7.5f, CaptureTheFlag.stealerPlayer.transform.position.z);
                                        break;
                                    // Airship
                                    case 4:
                                        CaptureTheFlag.stealerPlayer.transform.position = new Vector3(10.25f, -15.35f, CaptureTheFlag.stealerPlayer.transform.position.z);
                                        break;
                                    // Submerged
                                    case 5:
                                        if (CaptureTheFlag.stealerPlayer.transform.position.y > 0) {
                                            CaptureTheFlag.stealerPlayer.transform.position = new Vector3(1f, 10f, CaptureTheFlag.stealerPlayer.transform.position.z);
                                        }
                                        else {
                                            CaptureTheFlag.stealerPlayer.transform.position = new Vector3(0f, -33.5f, CaptureTheFlag.stealerPlayer.transform.position.z);
                                        }
                                        break;
                                }
                                DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                            }

                        })));

                    }

                    foreach (PlayerControl player in CaptureTheFlag.redteamFlag) {
                        if (player.PlayerId == target.PlayerId) {
                            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                            body.transform.position = new Vector3(50, 50, 1);
                            if (CaptureTheFlag.redplayer01 != null && target.PlayerId == CaptureTheFlag.redplayer01.PlayerId) {
                                CaptureTheFlag.redplayer01IsReviving = true;
                            }
                            else if (CaptureTheFlag.redplayer02 != null && target.PlayerId == CaptureTheFlag.redplayer02.PlayerId) {
                                CaptureTheFlag.redplayer02IsReviving = true;
                            }
                            else if (CaptureTheFlag.redplayer03 != null && target.PlayerId == CaptureTheFlag.redplayer03.PlayerId) {
                                CaptureTheFlag.redplayer03IsReviving = true;
                            }
                            else if (CaptureTheFlag.redplayer04 != null && target.PlayerId == CaptureTheFlag.redplayer04.PlayerId) {
                                CaptureTheFlag.redplayer04IsReviving = true;
                            }
                            else if (CaptureTheFlag.redplayer05 != null && target.PlayerId == CaptureTheFlag.redplayer05.PlayerId) {
                                CaptureTheFlag.redplayer05IsReviving = true;
                            }
                            else if (CaptureTheFlag.redplayer06 != null && target.PlayerId == CaptureTheFlag.redplayer06.PlayerId) {
                                CaptureTheFlag.redplayer06IsReviving = true;
                            }
                            else if (CaptureTheFlag.redplayer07 != null && target.PlayerId == CaptureTheFlag.redplayer07.PlayerId) {
                                CaptureTheFlag.redplayer07IsReviving = true;
                            }
                            Helpers.alphaPlayer(true, player.PlayerId);                            
                            HudManager.Instance.StartCoroutine(Effects.Lerp(CaptureTheFlag.reviveTime, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    if (CaptureTheFlag.redplayer01 != null && target.PlayerId == CaptureTheFlag.redplayer01.PlayerId) {
                                        CaptureTheFlag.redplayer01IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.redplayer02 != null && target.PlayerId == CaptureTheFlag.redplayer02.PlayerId) {
                                        CaptureTheFlag.redplayer02IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.redplayer03 != null && target.PlayerId == CaptureTheFlag.redplayer03.PlayerId) {
                                        CaptureTheFlag.redplayer03IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.redplayer04 != null && target.PlayerId == CaptureTheFlag.redplayer04.PlayerId) {
                                        CaptureTheFlag.redplayer04IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.redplayer05 != null && target.PlayerId == CaptureTheFlag.redplayer05.PlayerId) {
                                        CaptureTheFlag.redplayer05IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.redplayer06 != null && target.PlayerId == CaptureTheFlag.redplayer06.PlayerId) {
                                        CaptureTheFlag.redplayer06IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.redplayer07 != null && target.PlayerId == CaptureTheFlag.redplayer07.PlayerId) {
                                        CaptureTheFlag.redplayer07IsReviving = false;
                                    }
                                    Helpers.alphaPlayer(false, player.PlayerId);                                   
                                }
                            })));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(CaptureTheFlag.reviveTime - CaptureTheFlag.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    player.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                player.transform.position = new Vector3(-17.5f, -1.15f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-20.5f, -5.15f, player.transform.position.z);
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            player.transform.position = new Vector3(2.53f, 10.75f, player.transform.position.z);
                                            break;
                                        // Polus
                                        case 2:
                                            player.transform.position = new Vector3(36.4f, -21.5f, player.transform.position.z);
                                            break;
                                        // Dleks
                                        case 3:
                                            player.transform.position = new Vector3(20.5f, -5.15f, player.transform.position.z);
                                            break;
                                        // Airship
                                        case 4:
                                            player.transform.position = new Vector3(-17.5f, -1.1f, player.transform.position.z);
                                            break;
                                        // Submerged
                                        case 5:
                                            if (player.transform.position.y > 0) {
                                                player.transform.position = new Vector3(-8.35f, 28.25f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-14f, -27.5f, player.transform.position.z);
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));

                        }
                    }
                    foreach (PlayerControl player in CaptureTheFlag.blueteamFlag) {
                        if (player.PlayerId == target.PlayerId) {
                            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                            body.transform.position = new Vector3(50, 50, 1);
                            if (CaptureTheFlag.blueplayer01 != null && target.PlayerId == CaptureTheFlag.blueplayer01.PlayerId) {
                                CaptureTheFlag.blueplayer01IsReviving = true;
                            }
                            else if (CaptureTheFlag.blueplayer02 != null && target.PlayerId == CaptureTheFlag.blueplayer02.PlayerId) {
                                CaptureTheFlag.blueplayer02IsReviving = true;
                            }
                            else if (CaptureTheFlag.blueplayer03 != null && target.PlayerId == CaptureTheFlag.blueplayer03.PlayerId) {
                                CaptureTheFlag.blueplayer03IsReviving = true;
                            }
                            else if (CaptureTheFlag.blueplayer04 != null && target.PlayerId == CaptureTheFlag.blueplayer04.PlayerId) {
                                CaptureTheFlag.blueplayer04IsReviving = true;
                            }
                            else if (CaptureTheFlag.blueplayer05 != null && target.PlayerId == CaptureTheFlag.blueplayer05.PlayerId) {
                                CaptureTheFlag.blueplayer05IsReviving = true;
                            }
                            else if (CaptureTheFlag.blueplayer06 != null && target.PlayerId == CaptureTheFlag.blueplayer06.PlayerId) {
                                CaptureTheFlag.blueplayer06IsReviving = true;
                            }
                            else if (CaptureTheFlag.blueplayer07 != null && target.PlayerId == CaptureTheFlag.blueplayer07.PlayerId) {
                                CaptureTheFlag.blueplayer07IsReviving = true;
                            }
                            Helpers.alphaPlayer(true, player.PlayerId);                           
                            HudManager.Instance.StartCoroutine(Effects.Lerp(CaptureTheFlag.reviveTime, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    if (CaptureTheFlag.blueplayer01 != null && target.PlayerId == CaptureTheFlag.blueplayer01.PlayerId) {
                                        CaptureTheFlag.blueplayer01IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.blueplayer02 != null && target.PlayerId == CaptureTheFlag.blueplayer02.PlayerId) {
                                        CaptureTheFlag.blueplayer02IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.blueplayer03 != null && target.PlayerId == CaptureTheFlag.blueplayer03.PlayerId) {
                                        CaptureTheFlag.blueplayer03IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.blueplayer04 != null && target.PlayerId == CaptureTheFlag.blueplayer04.PlayerId) {
                                        CaptureTheFlag.blueplayer04IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.blueplayer05 != null && target.PlayerId == CaptureTheFlag.blueplayer05.PlayerId) {
                                        CaptureTheFlag.blueplayer05IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.blueplayer06 != null && target.PlayerId == CaptureTheFlag.blueplayer06.PlayerId) {
                                        CaptureTheFlag.blueplayer06IsReviving = false;
                                    }
                                    else if (CaptureTheFlag.blueplayer07 != null && target.PlayerId == CaptureTheFlag.blueplayer07.PlayerId) {
                                        CaptureTheFlag.blueplayer07IsReviving = false;
                                    }
                                    Helpers.alphaPlayer(false, player.PlayerId);                                    
                                }
                            })));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(CaptureTheFlag.reviveTime - CaptureTheFlag.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    player.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                player.transform.position = new Vector3(7.7f, -0.95f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(16.5f, -4.45f, player.transform.position.z);
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            player.transform.position = new Vector3(23.25f, 5.25f, player.transform.position.z);
                                            break;
                                        // Polus
                                        case 2:
                                            player.transform.position = new Vector3(5.4f, -9.45f, player.transform.position.z);
                                            break;
                                        // Dleks
                                        case 3:
                                            player.transform.position = new Vector3(-16.5f, -4.45f, player.transform.position.z);
                                            break;
                                        // Airship
                                        case 4:
                                            player.transform.position = new Vector3(33.6f, 1.45f, player.transform.position.z);
                                            break;
                                        // Submerged
                                        case 5:
                                            if (player.transform.position.y > 0) {
                                                player.transform.position = new Vector3(14.25f, 24.25f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(12.5f, -31.25f, player.transform.position.z);
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));

                        }
                    }
                }

                // Police and Thief revive player
                if (PoliceAndThief.policeAndThiefMode) {
                    foreach (PlayerControl player in PoliceAndThief.policeTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                            body.transform.position = new Vector3(50, 50, 1);
                            if (PoliceAndThief.policeplayer01 != null && target.PlayerId == PoliceAndThief.policeplayer01.PlayerId) {
                                PoliceAndThief.policeplayer01IsReviving = true;
                            }
                            else if (PoliceAndThief.policeplayer02 != null && target.PlayerId == PoliceAndThief.policeplayer02.PlayerId) {
                                PoliceAndThief.policeplayer02IsReviving = true;
                            }
                            else if (PoliceAndThief.policeplayer03 != null && target.PlayerId == PoliceAndThief.policeplayer03.PlayerId) {
                                PoliceAndThief.policeplayer03IsReviving = true;
                            }
                            else if (PoliceAndThief.policeplayer04 != null && target.PlayerId == PoliceAndThief.policeplayer04.PlayerId) {
                                PoliceAndThief.policeplayer04IsReviving = true;
                            }
                            else if (PoliceAndThief.policeplayer05 != null && target.PlayerId == PoliceAndThief.policeplayer05.PlayerId) {
                                PoliceAndThief.policeplayer05IsReviving = true;
                            }
                            else if (PoliceAndThief.policeplayer06 != null && target.PlayerId == PoliceAndThief.policeplayer06.PlayerId) {
                                PoliceAndThief.policeplayer06IsReviving = true;
                            }
                            Helpers.alphaPlayer(true, player.PlayerId);                            
                            HudManager.Instance.StartCoroutine(Effects.Lerp(PoliceAndThief.policeReviveTime, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    if (PoliceAndThief.policeplayer01 != null && target.PlayerId == PoliceAndThief.policeplayer01.PlayerId) {
                                        PoliceAndThief.policeplayer01IsReviving = false;
                                    }
                                    else if (PoliceAndThief.policeplayer02 != null && target.PlayerId == PoliceAndThief.policeplayer02.PlayerId) {
                                        PoliceAndThief.policeplayer02IsReviving = false;
                                    }
                                    else if (PoliceAndThief.policeplayer03 != null && target.PlayerId == PoliceAndThief.policeplayer03.PlayerId) {
                                        PoliceAndThief.policeplayer03IsReviving = false;
                                    }
                                    else if (PoliceAndThief.policeplayer04 != null && target.PlayerId == PoliceAndThief.policeplayer04.PlayerId) {
                                        PoliceAndThief.policeplayer04IsReviving = false;
                                    }
                                    else if (PoliceAndThief.policeplayer05 != null && target.PlayerId == PoliceAndThief.policeplayer05.PlayerId) {
                                        PoliceAndThief.policeplayer05IsReviving = false;
                                    }
                                    else if (PoliceAndThief.policeplayer06 != null && target.PlayerId == PoliceAndThief.policeplayer06.PlayerId) {
                                        PoliceAndThief.policeplayer06IsReviving = false;
                                    }
                                    Helpers.alphaPlayer(false, player.PlayerId);                                    
                                }
                            })));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(PoliceAndThief.policeReviveTime - PoliceAndThief.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    player.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                player.transform.position = new Vector3(-12f, 5f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-10.2f, 1.18f, player.transform.position.z);
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            player.transform.position = new Vector3(1.8f, -1f, player.transform.position.z);
                                            break;
                                        // Polus
                                        case 2:
                                            player.transform.position = new Vector3(8.18f, -7.4f, player.transform.position.z);
                                            break;
                                        // Dleks
                                        case 3:
                                            player.transform.position = new Vector3(10.2f, 1.18f, player.transform.position.z);
                                            break;
                                        // Airship
                                        case 4:
                                            player.transform.position = new Vector3(-18.5f, 0.75f, player.transform.position.z);
                                            break;
                                        // Submerged
                                        case 5:
                                            if (player.transform.position.y > 0) {
                                                player.transform.position = new Vector3(-8.45f, 27f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-9.25f, -41.25f, player.transform.position.z);
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));

                        }
                    }
                    foreach (PlayerControl player in PoliceAndThief.thiefTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                            body.transform.position = new Vector3(50, 50, 1);
                            if (PoliceAndThief.thiefplayer01 != null && target.PlayerId == PoliceAndThief.thiefplayer01.PlayerId) {
                                if (PoliceAndThief.thiefplayer01IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer01JewelId);
                                }
                                PoliceAndThief.thiefplayer01IsReviving = true;
                            }
                            else if (PoliceAndThief.thiefplayer02 != null && target.PlayerId == PoliceAndThief.thiefplayer02.PlayerId) {
                                if (PoliceAndThief.thiefplayer02IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer02JewelId);
                                }
                                PoliceAndThief.thiefplayer02IsReviving = true;
                            }
                            else if (PoliceAndThief.thiefplayer03 != null && target.PlayerId == PoliceAndThief.thiefplayer03.PlayerId) {
                                if (PoliceAndThief.thiefplayer03IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer03JewelId);
                                }
                                PoliceAndThief.thiefplayer03IsReviving = true;
                            }
                            else if (PoliceAndThief.thiefplayer04 != null && target.PlayerId == PoliceAndThief.thiefplayer04.PlayerId) {
                                if (PoliceAndThief.thiefplayer04IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer04JewelId);
                                }
                                PoliceAndThief.thiefplayer04IsReviving = true;
                            }
                            else if (PoliceAndThief.thiefplayer05 != null && target.PlayerId == PoliceAndThief.thiefplayer05.PlayerId) {
                                if (PoliceAndThief.thiefplayer05IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer05JewelId);
                                }
                                PoliceAndThief.thiefplayer05IsReviving = true;
                            }
                            else if (PoliceAndThief.thiefplayer06 != null && target.PlayerId == PoliceAndThief.thiefplayer06.PlayerId) {
                                if (PoliceAndThief.thiefplayer06IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer06JewelId);
                                }
                                PoliceAndThief.thiefplayer06IsReviving = true;
                            }
                            else if (PoliceAndThief.thiefplayer07 != null && target.PlayerId == PoliceAndThief.thiefplayer07.PlayerId) {
                                if (PoliceAndThief.thiefplayer07IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer07JewelId);
                                }
                                PoliceAndThief.thiefplayer07IsReviving = true;
                            }
                            else if (PoliceAndThief.thiefplayer08 != null && target.PlayerId == PoliceAndThief.thiefplayer08.PlayerId) {
                                if (PoliceAndThief.thiefplayer08IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer08JewelId);
                                }
                                PoliceAndThief.thiefplayer08IsReviving = true;
                            }
                            else if (PoliceAndThief.thiefplayer09 != null && target.PlayerId == PoliceAndThief.thiefplayer09.PlayerId) {
                                if (PoliceAndThief.thiefplayer09IsStealing) {
                                    RPCProcedure.policeandThiefRevertedJewelPosition(target.PlayerId, PoliceAndThief.thiefplayer09JewelId);
                                }
                                PoliceAndThief.thiefplayer09IsReviving = true;
                            }
                            Helpers.alphaPlayer(true, player.PlayerId);
                            HudManager.Instance.StartCoroutine(Effects.Lerp(PoliceAndThief.thiefReviveTime, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    if (PoliceAndThief.thiefplayer01 != null && target.PlayerId == PoliceAndThief.thiefplayer01.PlayerId) {
                                        PoliceAndThief.thiefplayer01IsReviving = false;
                                    }
                                    else if (PoliceAndThief.thiefplayer02 != null && target.PlayerId == PoliceAndThief.thiefplayer02.PlayerId) {
                                        PoliceAndThief.thiefplayer02IsReviving = false;
                                    }
                                    else if (PoliceAndThief.thiefplayer03 != null && target.PlayerId == PoliceAndThief.thiefplayer03.PlayerId) {
                                        PoliceAndThief.thiefplayer03IsReviving = false;
                                    }
                                    else if (PoliceAndThief.thiefplayer04 != null && target.PlayerId == PoliceAndThief.thiefplayer04.PlayerId) {
                                        PoliceAndThief.thiefplayer04IsReviving = false;
                                    }
                                    else if (PoliceAndThief.thiefplayer05 != null && target.PlayerId == PoliceAndThief.thiefplayer05.PlayerId) {
                                        PoliceAndThief.thiefplayer05IsReviving = false;
                                    }
                                    else if (PoliceAndThief.thiefplayer06 != null && target.PlayerId == PoliceAndThief.thiefplayer06.PlayerId) {
                                        PoliceAndThief.thiefplayer06IsReviving = false;
                                    }
                                    else if (PoliceAndThief.thiefplayer07 != null && target.PlayerId == PoliceAndThief.thiefplayer07.PlayerId) {
                                        PoliceAndThief.thiefplayer07IsReviving = false;
                                    }
                                    else if (PoliceAndThief.thiefplayer08 != null && target.PlayerId == PoliceAndThief.thiefplayer08.PlayerId) {
                                        PoliceAndThief.thiefplayer08IsReviving = false;
                                    }
                                    else if (PoliceAndThief.thiefplayer09 != null && target.PlayerId == PoliceAndThief.thiefplayer09.PlayerId) {
                                        PoliceAndThief.thiefplayer09IsReviving = false;
                                    }
                                    Helpers.alphaPlayer(false, player.PlayerId);
                                }
                            })));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(PoliceAndThief.thiefReviveTime - PoliceAndThief.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    player.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                player.transform.position = new Vector3(13.75f, -0.2f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-1.31f, -16.25f, player.transform.position.z);
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            player.transform.position = new Vector3(17.75f, 11.5f, player.transform.position.z);
                                            break;
                                        // Polus
                                        case 2:
                                            player.transform.position = new Vector3(30f, -15.75f, player.transform.position.z);
                                            break;
                                        // Dleks
                                        case 3:
                                            player.transform.position = new Vector3(1.31f, -16.25f, player.transform.position.z);
                                            break;
                                        // Airship
                                        case 4:
                                            player.transform.position = new Vector3(7.15f, -14.5f, player.transform.position.z);
                                            break;
                                        // Submerged
                                        case 5:
                                            if (player.transform.position.y > 0) {
                                                player.transform.position = new Vector3(1f, 10f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(12.5f, -31.75f, player.transform.position.z);
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));

                        }
                    }
                }

                // King of the hill revive player
                if (KingOfTheHill.kingOfTheHillMode) {
                    if (KingOfTheHill.usurperPlayer != null && KingOfTheHill.usurperPlayer.PlayerId == target.PlayerId) {
                        var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                        body.transform.position = new Vector3(50, 50, 1);
                        KingOfTheHill.usurperPlayerIsReviving = true;
                        Helpers.alphaPlayer(true, KingOfTheHill.usurperPlayer.PlayerId);
                        HudManager.Instance.StartCoroutine(Effects.Lerp(KingOfTheHill.reviveTime, new Action<float>((p) => {
                            if (p == 1f && KingOfTheHill.usurperPlayer != null) {
                                KingOfTheHill.usurperPlayerIsReviving = false;
                                KingOfTheHill.usurperPlayer.cosmetics.nameText.color = new Color(KingOfTheHill.usurperPlayer.cosmetics.nameText.color.r, KingOfTheHill.usurperPlayer.cosmetics.nameText.color.g, KingOfTheHill.usurperPlayer.cosmetics.nameText.color.b, 1f);
                                Helpers.alphaPlayer(false, KingOfTheHill.usurperPlayer.PlayerId);
                            }
                        })));
                        HudManager.Instance.StartCoroutine(Effects.Lerp(KingOfTheHill.reviveTime - KingOfTheHill.kingInvincibilityTimeAfterRevive, new Action<float>((p) => {
                            if (p == 1f && KingOfTheHill.usurperPlayer != null) {
                                KingOfTheHill.usurperPlayer.Revive();
                                switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                    // Skeld
                                    case 0:
                                        if (activatedSensei) {
                                            KingOfTheHill.usurperPlayer.transform.position = new Vector3(-6.8f, 10.75f, KingOfTheHill.usurperPlayer.transform.position.z);
                                        }
                                        else {
                                            KingOfTheHill.usurperPlayer.transform.position = new Vector3(-1f, 5.35f, KingOfTheHill.usurperPlayer.transform.position.z);
                                        }
                                        break;
                                    // MiraHQ
                                    case 1:
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(2.5f, 11f, KingOfTheHill.usurperPlayer.transform.position.z);
                                        break;
                                    // Polus
                                    case 2:
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(20.5f, -12f, KingOfTheHill.usurperPlayer.transform.position.z);
                                        break;
                                    // Dleks
                                    case 3:
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(1f, 5.35f, KingOfTheHill.usurperPlayer.transform.position.z);
                                        break;
                                    // Airship
                                    case 4:
                                        KingOfTheHill.usurperPlayer.transform.position = new Vector3(12.25f, 2f, KingOfTheHill.usurperPlayer.transform.position.z);
                                        break;
                                    // Submerged
                                    case 5:
                                        if (KingOfTheHill.usurperPlayer.transform.position.y > 0) {
                                            KingOfTheHill.usurperPlayer.transform.position = new Vector3(5.75f, 31.25f, KingOfTheHill.usurperPlayer.transform.position.z);
                                        }
                                        else {
                                            KingOfTheHill.usurperPlayer.transform.position = new Vector3(-4.25f, -33.5f, KingOfTheHill.usurperPlayer.transform.position.z);
                                        }
                                        break;
                                }
                                DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                            }

                        })));

                    }

                    foreach (PlayerControl player in KingOfTheHill.greenTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                            body.transform.position = new Vector3(50, 50, 1);

                            // Restore zones
                            if (KingOfTheHill.greenKingplayer != null && target.PlayerId == KingOfTheHill.greenKingplayer.PlayerId) {
                                KingOfTheHill.greenteamAlerted = false;
                                if (KingOfTheHill.greenKinghaszoneone) {
                                    KingOfTheHill.greenKinghaszoneone = false;
                                    KingOfTheHill.flagzoneone.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whiteflag.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zoneone.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whitebase.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zoneonecolor = Color.white;
                                }
                                if (KingOfTheHill.greenKinghaszonetwo) {
                                    KingOfTheHill.greenKinghaszonetwo = false;
                                    KingOfTheHill.flagzonetwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whiteflag.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zonetwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whitebase.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zonetwocolor = Color.white;
                                }
                                if (KingOfTheHill.greenKinghaszonethree) {
                                    KingOfTheHill.greenKinghaszonethree = false;
                                    KingOfTheHill.flagzonethree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whiteflag.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zonethree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whitebase.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zonethreecolor = Color.white;
                                }
                                // Alert green team players
                                if (!KingOfTheHill.greenteamAlerted) {
                                    KingOfTheHill.greenteamAlerted = true;
                                    foreach (PlayerControl greenplayer in KingOfTheHill.greenTeam) {
                                        if (greenplayer == PlayerControl.LocalPlayer && greenplayer != null) {
                                            new CustomMessage(Language.statusKingOfTheHillTexts[4], 5, -1, 1f, 11);
                                        }
                                    }
                                }
                                KingOfTheHill.totalGreenKingzonescaptured = 0;
                                KingOfTheHill.greenKingIsReviving = true;
                                // Hide aura while dead
                                DeadPlayer kinggreenPlayer = deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();
                                if (kinggreenPlayer != null && kinggreenPlayer.killerIfExisting != null) {
                                    if (kinggreenPlayer.player == KingOfTheHill.greenKingplayer && kinggreenPlayer.killerIfExisting != KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.greenkingaura.SetActive(false);
                                        HudManager.Instance.StartCoroutine(Effects.Lerp(KingOfTheHill.reviveTime - KingOfTheHill.kingInvincibilityTimeAfterRevive, new Action<float>((p) => {
                                            if (p == 1f) {
                                                KingOfTheHill.greenkingaura.SetActive(true);
                                            }
                                        })));
                                    }
                                }
                            }
                            else if (KingOfTheHill.greenplayer01 != null && target.PlayerId == KingOfTheHill.greenplayer01.PlayerId) {
                                KingOfTheHill.greenplayer01IsReviving = true;
                            }
                            else if (KingOfTheHill.greenplayer02 != null && target.PlayerId == KingOfTheHill.greenplayer02.PlayerId) {
                                KingOfTheHill.greenplayer02IsReviving = true;
                            }
                            else if (KingOfTheHill.greenplayer03 != null && target.PlayerId == KingOfTheHill.greenplayer03.PlayerId) {
                                KingOfTheHill.greenplayer03IsReviving = true;
                            }
                            else if (KingOfTheHill.greenplayer04 != null && target.PlayerId == KingOfTheHill.greenplayer04.PlayerId) {
                                KingOfTheHill.greenplayer04IsReviving = true;
                            }
                            else if (KingOfTheHill.greenplayer05 != null && target.PlayerId == KingOfTheHill.greenplayer05.PlayerId) {
                                KingOfTheHill.greenplayer05IsReviving = true;
                            }
                            else if (KingOfTheHill.greenplayer06 != null && target.PlayerId == KingOfTheHill.greenplayer06.PlayerId) {
                                KingOfTheHill.greenplayer06IsReviving = true;
                            }
                            Helpers.alphaPlayer(true, player.PlayerId);
                            HudManager.Instance.StartCoroutine(Effects.Lerp(KingOfTheHill.reviveTime, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    if (KingOfTheHill.greenKingplayer != null && target.PlayerId == KingOfTheHill.greenKingplayer.PlayerId) {
                                        KingOfTheHill.greenKingIsReviving = false;
                                    }
                                    else if (KingOfTheHill.greenplayer01 != null && target.PlayerId == KingOfTheHill.greenplayer01.PlayerId) {
                                        KingOfTheHill.greenplayer01IsReviving = false;
                                    }
                                    else if (KingOfTheHill.greenplayer02 != null && target.PlayerId == KingOfTheHill.greenplayer02.PlayerId) {
                                        KingOfTheHill.greenplayer02IsReviving = false;
                                    }
                                    else if (KingOfTheHill.greenplayer03 != null && target.PlayerId == KingOfTheHill.greenplayer03.PlayerId) {
                                        KingOfTheHill.greenplayer03IsReviving = false;
                                    }
                                    else if (KingOfTheHill.greenplayer04 != null && target.PlayerId == KingOfTheHill.greenplayer04.PlayerId) {
                                        KingOfTheHill.greenplayer04IsReviving = false;
                                    }
                                    else if (KingOfTheHill.greenplayer05 != null && target.PlayerId == KingOfTheHill.greenplayer05.PlayerId) {
                                        KingOfTheHill.greenplayer05IsReviving = false;
                                    }
                                    else if (KingOfTheHill.greenplayer06 != null && target.PlayerId == KingOfTheHill.greenplayer06.PlayerId) {
                                        KingOfTheHill.greenplayer06IsReviving = false;
                                    }
                                    Helpers.alphaPlayer(false, player.PlayerId);
                                }
                            })));
                            HudManager.Instance.StartCoroutine(Effects.Lerp(KingOfTheHill.reviveTime - KingOfTheHill.kingInvincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    player.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                player.transform.position = new Vector3(-16.4f, -10.25f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-7f, -8.25f, player.transform.position.z);
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            player.transform.position = new Vector3(-4.45f, 1.75f, player.transform.position.z);
                                            break;
                                        // Polus
                                        case 2:
                                            player.transform.position = new Vector3(2.25f, -23.75f, player.transform.position.z);
                                            break;
                                        // Dleks
                                        case 3:
                                            player.transform.position = new Vector3(7f, -8.25f, player.transform.position.z);
                                            break;
                                        // Airship
                                        case 4:
                                            player.transform.position = new Vector3(-13.9f, -14.45f, player.transform.position.z);
                                            break;
                                        // Submerged
                                        case 5:
                                            if (player.transform.position.y > 0) {
                                                player.transform.position = new Vector3(-12.25f, 18.5f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-14.5f, -34.35f, player.transform.position.z);
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));

                        }
                    }
                    foreach (PlayerControl player in KingOfTheHill.yellowTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                            body.transform.position = new Vector3(50, 50, 1);

                            // Restore zones
                            if (KingOfTheHill.yellowKingplayer != null && target.PlayerId == KingOfTheHill.yellowKingplayer.PlayerId) {
                                KingOfTheHill.yellowteamAlerted = false;
                                if (KingOfTheHill.yellowKinghaszoneone) {
                                    KingOfTheHill.yellowKinghaszoneone = false;
                                    KingOfTheHill.flagzoneone.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whiteflag.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zoneone.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whitebase.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zoneonecolor = Color.white;
                                }
                                if (KingOfTheHill.yellowKinghaszonetwo) {
                                    KingOfTheHill.yellowKinghaszonetwo = false;
                                    KingOfTheHill.flagzonetwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whiteflag.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zonetwo.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whitebase.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zonetwocolor = Color.white;
                                }
                                if (KingOfTheHill.yellowKinghaszonethree) {
                                    KingOfTheHill.yellowKinghaszonethree = false;
                                    KingOfTheHill.flagzonethree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whiteflag.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zonethree.GetComponent<SpriteRenderer>().sprite = CustomMain.customAssets.whitebase.GetComponent<SpriteRenderer>().sprite;
                                    KingOfTheHill.zonethreecolor = Color.white;
                                }
                                // Alert yellow team players
                                if (!KingOfTheHill.yellowteamAlerted) {
                                    KingOfTheHill.yellowteamAlerted = true;
                                    foreach (PlayerControl yellowplayer in KingOfTheHill.yellowTeam) {
                                        if (yellowplayer == PlayerControl.LocalPlayer && yellowplayer != null) {
                                            new CustomMessage(Language.statusKingOfTheHillTexts[4], 5, -1, 1f, 11);
                                        }
                                    }
                                }
                                KingOfTheHill.totalYellowKingzonescaptured = 0;
                                KingOfTheHill.yellowKingIsReviving = true;
                                // Hide aura while dead
                                DeadPlayer kingyellowPlayer = deadPlayers?.Where(x => x.player?.PlayerId == target?.PlayerId)?.FirstOrDefault();
                                if (kingyellowPlayer != null && kingyellowPlayer.killerIfExisting != null) {
                                    if (kingyellowPlayer.player == KingOfTheHill.yellowKingplayer && kingyellowPlayer.killerIfExisting != KingOfTheHill.usurperPlayer) {
                                        KingOfTheHill.yellowkingaura.SetActive(false);
                                        HudManager.Instance.StartCoroutine(Effects.Lerp(KingOfTheHill.reviveTime - KingOfTheHill.kingInvincibilityTimeAfterRevive, new Action<float>((p) => {
                                            if (p == 1f) {
                                                KingOfTheHill.yellowkingaura.SetActive(true);
                                            }
                                        })));
                                    }
                                }
                            }
                            else if (KingOfTheHill.yellowplayer01 != null && target.PlayerId == KingOfTheHill.yellowplayer01.PlayerId) {
                                KingOfTheHill.yellowplayer01IsReviving = true;
                            }
                            else if (KingOfTheHill.yellowplayer02 != null && target.PlayerId == KingOfTheHill.yellowplayer02.PlayerId) {
                                KingOfTheHill.yellowplayer02IsReviving = true;
                            }
                            else if (KingOfTheHill.yellowplayer03 != null && target.PlayerId == KingOfTheHill.yellowplayer03.PlayerId) {
                                KingOfTheHill.yellowplayer03IsReviving = true;
                            }
                            else if (KingOfTheHill.yellowplayer04 != null && target.PlayerId == KingOfTheHill.yellowplayer04.PlayerId) {
                                KingOfTheHill.yellowplayer04IsReviving = true;
                            }
                            else if (KingOfTheHill.yellowplayer05 != null && target.PlayerId == KingOfTheHill.yellowplayer05.PlayerId) {
                                KingOfTheHill.yellowplayer05IsReviving = true;
                            }
                            else if (KingOfTheHill.yellowplayer06 != null && target.PlayerId == KingOfTheHill.yellowplayer06.PlayerId) {
                                KingOfTheHill.yellowplayer06IsReviving = true;
                            }
                            Helpers.alphaPlayer(true, player.PlayerId);
                            HudManager.Instance.StartCoroutine(Effects.Lerp(KingOfTheHill.reviveTime, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    if (KingOfTheHill.yellowKingplayer != null && target.PlayerId == KingOfTheHill.yellowKingplayer.PlayerId) {
                                        KingOfTheHill.yellowKingIsReviving = false;
                                    }
                                    else if (KingOfTheHill.yellowplayer01 != null && target.PlayerId == KingOfTheHill.yellowplayer01.PlayerId) {
                                        KingOfTheHill.yellowplayer01IsReviving = false;
                                    }
                                    else if (KingOfTheHill.yellowplayer02 != null && target.PlayerId == KingOfTheHill.yellowplayer02.PlayerId) {
                                        KingOfTheHill.yellowplayer02IsReviving = false;
                                    }
                                    else if (KingOfTheHill.yellowplayer03 != null && target.PlayerId == KingOfTheHill.yellowplayer03.PlayerId) {
                                        KingOfTheHill.yellowplayer03IsReviving = false;
                                    }
                                    else if (KingOfTheHill.yellowplayer04 != null && target.PlayerId == KingOfTheHill.yellowplayer04.PlayerId) {
                                        KingOfTheHill.yellowplayer04IsReviving = false;
                                    }
                                    else if (KingOfTheHill.yellowplayer05 != null && target.PlayerId == KingOfTheHill.yellowplayer05.PlayerId) {
                                        KingOfTheHill.yellowplayer05IsReviving = false;
                                    }
                                    else if (KingOfTheHill.yellowplayer06 != null && target.PlayerId == KingOfTheHill.yellowplayer06.PlayerId) {
                                        KingOfTheHill.yellowplayer06IsReviving = false;
                                    }
                                    Helpers.alphaPlayer(false, player.PlayerId);
                                }
                            })));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(KingOfTheHill.reviveTime - KingOfTheHill.kingInvincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    player.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                player.transform.position = new Vector3(7f, -14.15f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(6.25f, -3.5f, player.transform.position.z);
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            player.transform.position = new Vector3(19.5f, 4.7f, player.transform.position.z);
                                            break;
                                        // Polus
                                        case 2:
                                            player.transform.position = new Vector3(36.35f, -6.15f, player.transform.position.z);
                                            break;
                                        // Dleks
                                        case 3:
                                            player.transform.position = new Vector3(-6.25f, -3.5f, player.transform.position.z);
                                            break;
                                        // Airship
                                        case 4:
                                            player.transform.position = new Vector3(37.35f, -3.25f, player.transform.position.z);
                                            break;
                                        // Submerged
                                        case 5:
                                            if (player.transform.position.y > 0) {
                                                player.transform.position = new Vector3(0f, 33.5f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-8.5f, -39.5f, player.transform.position.z);
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));

                        }
                    }
                }

                // Hot Potato new potato on murder
                if (HotPotato.hotPotatoMode) {
                    if (HotPotato.hotPotatoPlayer != null && HotPotato.hotPotatoPlayer.PlayerId == target.PlayerId) {

                        var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                        body.transform.position = new Vector3(50, 50, 1);

                        HotPotato.timeforTransfer = HotPotato.savedtimeforTransfer + 4f;

                        HudManager.Instance.StartCoroutine(Effects.Lerp(1, new Action<float>((p) => { // Delayed action
                            if (p == 1f) {

                                if (HotPotato.explodedPotato01 == null) {
                                    HotPotato.explodedPotato01 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato01);
                                }
                                else if (HotPotato.explodedPotato02 == null) {
                                    HotPotato.explodedPotato02 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato02);
                                }
                                else if (HotPotato.explodedPotato03 == null) {
                                    HotPotato.explodedPotato03 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato03);
                                }
                                else if (HotPotato.explodedPotato04 == null) {
                                    HotPotato.explodedPotato04 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato04);
                                }
                                else if (HotPotato.explodedPotato05 == null) {
                                    HotPotato.explodedPotato05 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato05);
                                }
                                else if (HotPotato.explodedPotato06 == null) {
                                    HotPotato.explodedPotato06 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato06);
                                }
                                else if (HotPotato.explodedPotato07 == null) {
                                    HotPotato.explodedPotato07 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato07);
                                }
                                else if (HotPotato.explodedPotato08 == null) {
                                    HotPotato.explodedPotato08 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato08);
                                }
                                else if (HotPotato.explodedPotato09 == null) {
                                    HotPotato.explodedPotato09 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato09);
                                }
                                else if (HotPotato.explodedPotato10 == null) {
                                    HotPotato.explodedPotato10 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato10);
                                }
                                else if (HotPotato.explodedPotato11 == null) {
                                    HotPotato.explodedPotato11 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato11);
                                }
                                else if (HotPotato.explodedPotato12 == null) {
                                    HotPotato.explodedPotato12 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato12);
                                }
                                else if (HotPotato.explodedPotato13 == null) {
                                    HotPotato.explodedPotato13 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato13);
                                }
                                else if (HotPotato.explodedPotato14 == null) {
                                    HotPotato.explodedPotato14 = HotPotato.hotPotatoPlayer;
                                    HotPotato.explodedPotatoTeam.Add(HotPotato.explodedPotato14);
                                }

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

                                HotPotato.hotPotatoPlayer = HotPotato.notPotatoTeam[0];

                                // If hot potato timed out, assing new potato
                                if (HotPotato.notPotato01 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato01) {
                                    HotPotato.notPotato01 = null;
                                }
                                else if (HotPotato.notPotato02 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato02) {
                                    HotPotato.notPotato02 = null;
                                }
                                else if (HotPotato.notPotato03 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato03) {
                                    HotPotato.notPotato03 = null;
                                }
                                else if (HotPotato.notPotato04 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato04) {
                                    HotPotato.notPotato04 = null;
                                }
                                else if (HotPotato.notPotato05 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato05) {
                                    HotPotato.notPotato05 = null;
                                }
                                else if (HotPotato.notPotato06 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato06) {
                                    HotPotato.notPotato06 = null;
                                }
                                else if (HotPotato.notPotato07 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato07) {
                                    HotPotato.notPotato07 = null;
                                }
                                else if (HotPotato.notPotato08 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato08) {
                                    HotPotato.notPotato08 = null;
                                }
                                else if (HotPotato.notPotato09 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato09) {
                                    HotPotato.notPotato09 = null;
                                }
                                else if (HotPotato.notPotato10 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato10) {
                                    HotPotato.notPotato10 = null;
                                }
                                else if (HotPotato.notPotato11 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato11) {
                                    HotPotato.notPotato11 = null;
                                }
                                else if (HotPotato.notPotato12 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato12) {
                                    HotPotato.notPotato12 = null;
                                }
                                else if (HotPotato.notPotato13 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato13) {
                                    HotPotato.notPotato13 = null;
                                }
                                else if (HotPotato.notPotato14 != null && HotPotato.notPotatoTeam[0] == HotPotato.notPotato14) {
                                    HotPotato.notPotato14 = null;
                                }

                                HotPotato.notPotatoTeam.RemoveAt(0);

                                HotPotato.hotPotatoPlayer.NetTransform.Halt();
                                HotPotato.hotPotatoPlayer.moveable = false;
                                HotPotato.hotPotato.transform.position = HotPotato.hotPotatoPlayer.transform.position + new Vector3(0, 0.5f, -0.25f);
                                HotPotato.hotPotato.transform.parent = HotPotato.hotPotatoPlayer.transform;

                                HudManager.Instance.StartCoroutine(Effects.Lerp(3, new Action<float>((p) => { // Delayed action
                                    if (p == 1f) {
                                        HotPotato.hotPotatoPlayer.moveable = true;
                                    }
                                })));

                                new CustomMessage("<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color>" + Language.statusHotPotatoTexts[0], 5, -1, 1f, 16);
                                HotPotato.hotpotatopointCounter = Language.introTexts[5] + "<color=#808080FF>" + HotPotato.hotPotatoPlayer.name + "</color> | " + Language.introTexts[6] + "<color=#00F7FFFF>" + notPotatosAlives + "</color>";
                            }
                        })));
                    }
                }

                // ZombieLaboratory revive players
                if (ZombieLaboratory.zombieLaboratoryMode) {

                    // ZombieLaboratory revive players
                    foreach (PlayerControl player in ZombieLaboratory.survivorTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                            body.transform.position = new Vector3(50, 50, 1);
                            if (ZombieLaboratory.survivorPlayer01 != null && target.PlayerId == ZombieLaboratory.survivorPlayer01.PlayerId) {
                                ZombieLaboratory.survivorPlayer01IsReviving = true;
                                ZombieLaboratory.survivorPlayer01CanKill = false;
                                if (ZombieLaboratory.survivorPlayer01IsInfected) {
                                    ZombieLaboratory.survivorPlayer01IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer01);
                                }
                                if (ZombieLaboratory.survivorPlayer01HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer01HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer01FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer02 != null && target.PlayerId == ZombieLaboratory.survivorPlayer02.PlayerId) {
                                ZombieLaboratory.survivorPlayer02IsReviving = true;
                                ZombieLaboratory.survivorPlayer02CanKill = false;
                                if (ZombieLaboratory.survivorPlayer02IsInfected) {
                                    ZombieLaboratory.survivorPlayer02IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer02);
                                }
                                if (ZombieLaboratory.survivorPlayer02HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer02HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer02FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer03 != null && target.PlayerId == ZombieLaboratory.survivorPlayer03.PlayerId) {
                                ZombieLaboratory.survivorPlayer03IsReviving = true;
                                ZombieLaboratory.survivorPlayer03CanKill = false;
                                if (ZombieLaboratory.survivorPlayer03IsInfected) {
                                    ZombieLaboratory.survivorPlayer03IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer03);
                                }
                                if (ZombieLaboratory.survivorPlayer03HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer03HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer03FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer04 != null && target.PlayerId == ZombieLaboratory.survivorPlayer04.PlayerId) {
                                ZombieLaboratory.survivorPlayer04IsReviving = true;
                                ZombieLaboratory.survivorPlayer04CanKill = false;
                                if (ZombieLaboratory.survivorPlayer04IsInfected) {
                                    ZombieLaboratory.survivorPlayer04IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer04);
                                }
                                if (ZombieLaboratory.survivorPlayer04HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer04HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer04FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer05 != null && target.PlayerId == ZombieLaboratory.survivorPlayer05.PlayerId) {
                                ZombieLaboratory.survivorPlayer05IsReviving = true;
                                ZombieLaboratory.survivorPlayer05CanKill = false;
                                if (ZombieLaboratory.survivorPlayer05IsInfected) {
                                    ZombieLaboratory.survivorPlayer05IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer05);
                                }
                                if (ZombieLaboratory.survivorPlayer05HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer05HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer05FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer06 != null && target.PlayerId == ZombieLaboratory.survivorPlayer06.PlayerId) {
                                ZombieLaboratory.survivorPlayer06IsReviving = true;
                                ZombieLaboratory.survivorPlayer06CanKill = false;
                                if (ZombieLaboratory.survivorPlayer06IsInfected) {
                                    ZombieLaboratory.survivorPlayer06IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer06);
                                }
                                if (ZombieLaboratory.survivorPlayer06HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer06HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer06FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer07 != null && target.PlayerId == ZombieLaboratory.survivorPlayer07.PlayerId) {
                                ZombieLaboratory.survivorPlayer07IsReviving = true;
                                ZombieLaboratory.survivorPlayer07CanKill = false;
                                if (ZombieLaboratory.survivorPlayer07IsInfected) {
                                    ZombieLaboratory.survivorPlayer07IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer07);
                                }
                                if (ZombieLaboratory.survivorPlayer07HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer07HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer07FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer08 != null && target.PlayerId == ZombieLaboratory.survivorPlayer08.PlayerId) {
                                ZombieLaboratory.survivorPlayer08IsReviving = true;
                                ZombieLaboratory.survivorPlayer08CanKill = false;
                                if (ZombieLaboratory.survivorPlayer08IsInfected) {
                                    ZombieLaboratory.survivorPlayer08IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer08);
                                }
                                if (ZombieLaboratory.survivorPlayer08HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer08HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer08FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer09 != null && target.PlayerId == ZombieLaboratory.survivorPlayer09.PlayerId) {
                                ZombieLaboratory.survivorPlayer09IsReviving = true;
                                ZombieLaboratory.survivorPlayer09CanKill = false;
                                if (ZombieLaboratory.survivorPlayer09IsInfected) {
                                    ZombieLaboratory.survivorPlayer09IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer09);
                                }
                                if (ZombieLaboratory.survivorPlayer09HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer09HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer09FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer10 != null && target.PlayerId == ZombieLaboratory.survivorPlayer10.PlayerId) {
                                ZombieLaboratory.survivorPlayer10IsReviving = true;
                                ZombieLaboratory.survivorPlayer10CanKill = false;
                                if (ZombieLaboratory.survivorPlayer10IsInfected) {
                                    ZombieLaboratory.survivorPlayer10IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer10);
                                }
                                if (ZombieLaboratory.survivorPlayer10HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer10HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer10FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer11 != null && target.PlayerId == ZombieLaboratory.survivorPlayer11.PlayerId) {
                                ZombieLaboratory.survivorPlayer11IsReviving = true;
                                ZombieLaboratory.survivorPlayer11CanKill = false;
                                if (ZombieLaboratory.survivorPlayer11IsInfected) {
                                    ZombieLaboratory.survivorPlayer11IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer11);
                                }
                                if (ZombieLaboratory.survivorPlayer11HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer11HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer11FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer12 != null && target.PlayerId == ZombieLaboratory.survivorPlayer12.PlayerId) {
                                ZombieLaboratory.survivorPlayer12IsReviving = true;
                                ZombieLaboratory.survivorPlayer12CanKill = false;
                                if (ZombieLaboratory.survivorPlayer12IsInfected) {
                                    ZombieLaboratory.survivorPlayer12IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer12);
                                }
                                if (ZombieLaboratory.survivorPlayer12HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer12HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer12FoundBox);
                                }
                            }
                            else if (ZombieLaboratory.survivorPlayer13 != null && target.PlayerId == ZombieLaboratory.survivorPlayer13.PlayerId) {
                                ZombieLaboratory.survivorPlayer13IsReviving = true;
                                ZombieLaboratory.survivorPlayer13CanKill = false;
                                if (ZombieLaboratory.survivorPlayer13IsInfected) {
                                    ZombieLaboratory.survivorPlayer13IsInfected = false;
                                    ZombieLaboratory.infectedTeam.Remove(ZombieLaboratory.survivorPlayer13);
                                }
                                if (ZombieLaboratory.survivorPlayer13HasKeyItem) {
                                    ZombieLaboratory.survivorPlayer13HasKeyItem = false;
                                    RPCProcedure.zombieLaboratoryRevertedKeyPosition(target.PlayerId, ZombieLaboratory.survivorPlayer13FoundBox);
                                }
                            }
                            Helpers.alphaPlayer(true, player.PlayerId);
                            HudManager.Instance.StartCoroutine(Effects.Lerp(ZombieLaboratory.reviveTime, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    if (ZombieLaboratory.survivorPlayer01 != null && target.PlayerId == ZombieLaboratory.survivorPlayer01.PlayerId) {
                                        ZombieLaboratory.survivorPlayer01IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer02 != null && target.PlayerId == ZombieLaboratory.survivorPlayer02.PlayerId) {
                                        ZombieLaboratory.survivorPlayer02IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer03 != null && target.PlayerId == ZombieLaboratory.survivorPlayer03.PlayerId) {
                                        ZombieLaboratory.survivorPlayer03IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer04 != null && target.PlayerId == ZombieLaboratory.survivorPlayer04.PlayerId) {
                                        ZombieLaboratory.survivorPlayer04IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer05 != null && target.PlayerId == ZombieLaboratory.survivorPlayer05.PlayerId) {
                                        ZombieLaboratory.survivorPlayer05IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer06 != null && target.PlayerId == ZombieLaboratory.survivorPlayer06.PlayerId) {
                                        ZombieLaboratory.survivorPlayer06IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer07 != null && target.PlayerId == ZombieLaboratory.survivorPlayer07.PlayerId) {
                                        ZombieLaboratory.survivorPlayer07IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer08 != null && target.PlayerId == ZombieLaboratory.survivorPlayer08.PlayerId) {
                                        ZombieLaboratory.survivorPlayer08IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer09 != null && target.PlayerId == ZombieLaboratory.survivorPlayer09.PlayerId) {
                                        ZombieLaboratory.survivorPlayer09IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer10 != null && target.PlayerId == ZombieLaboratory.survivorPlayer10.PlayerId) {
                                        ZombieLaboratory.survivorPlayer10IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer11 != null && target.PlayerId == ZombieLaboratory.survivorPlayer11.PlayerId) {
                                        ZombieLaboratory.survivorPlayer11IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer12 != null && target.PlayerId == ZombieLaboratory.survivorPlayer12.PlayerId) {
                                        ZombieLaboratory.survivorPlayer12IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.survivorPlayer13 != null && target.PlayerId == ZombieLaboratory.survivorPlayer13.PlayerId) {
                                        ZombieLaboratory.survivorPlayer13IsReviving = false;
                                    }
                                    Helpers.alphaPlayer(false, player.PlayerId);
                                }
                            })));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(ZombieLaboratory.reviveTime - ZombieLaboratory.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    player.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                if (player == ZombieLaboratory.nursePlayer) {
                                                    player.transform.position = new Vector3(-12f, 7.15f, player.transform.position.z);
                                                }
                                                else {
                                                    player.transform.position = new Vector3(4.75f, -8.5f, player.transform.position.z);
                                                }
                                            }
                                            else {
                                                if (player == ZombieLaboratory.nursePlayer) {
                                                    player.transform.position = new Vector3(-10.2f, 3.6f, player.transform.position.z);
                                                }
                                                else {
                                                    player.transform.position = new Vector3(11.75f, -4.75f, player.transform.position.z);
                                                }
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            if (player == ZombieLaboratory.nursePlayer) {
                                                player.transform.position = new Vector3(1.8f, 1.25f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(6.1f, 5.75f, player.transform.position.z);
                                            }
                                            break;
                                        // Polus
                                        case 2:
                                            if (player == ZombieLaboratory.nursePlayer) {
                                                player.transform.position = new Vector3(16.65f, -2.5f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(40.4f, -6.8f, player.transform.position.z);
                                            }
                                            break;
                                        // Dleks
                                        case 3:
                                            if (player == ZombieLaboratory.nursePlayer) {
                                                player.transform.position = new Vector3(10.2f, 3.6f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-11.75f, -4.75f, player.transform.position.z);
                                            }
                                            break;
                                        // Airship
                                        case 4:
                                            if (player == ZombieLaboratory.nursePlayer) {
                                                player.transform.position = new Vector3(-18.5f, 2.9f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(25.25f, -8.65f, player.transform.position.z);
                                            }
                                            break;
                                        // Submerged
                                        case 5:
                                            if (player.transform.position.y > 0) {
                                                if (player == ZombieLaboratory.nursePlayer) {
                                                    player.transform.position = new Vector3(-6f, 31.85f, player.transform.position.z);
                                                }
                                                else {
                                                    player.transform.position = new Vector3(5.5f, 31.5f, player.transform.position.z);
                                                }
                                            }
                                            else {
                                                if (player == ZombieLaboratory.nursePlayer) {
                                                    player.transform.position = new Vector3(-14f, -39.25f, player.transform.position.z);
                                                }
                                                else {
                                                    player.transform.position = new Vector3(9.75f, -31.35f, player.transform.position.z);
                                                }
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));
                        }
                    }
                    foreach (PlayerControl player in ZombieLaboratory.zombieTeam) {
                        if (player.PlayerId == target.PlayerId) {
                            var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                            body.transform.position = new Vector3(50, 50, 1);
                            if (ZombieLaboratory.zombiePlayer01 != null && target.PlayerId == ZombieLaboratory.zombiePlayer01.PlayerId) {
                                ZombieLaboratory.zombiePlayer01IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer02 != null && target.PlayerId == ZombieLaboratory.zombiePlayer02.PlayerId) {
                                ZombieLaboratory.zombiePlayer02IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer03 != null && target.PlayerId == ZombieLaboratory.zombiePlayer03.PlayerId) {
                                ZombieLaboratory.zombiePlayer03IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer04 != null && target.PlayerId == ZombieLaboratory.zombiePlayer04.PlayerId) {
                                ZombieLaboratory.zombiePlayer04IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer05 != null && target.PlayerId == ZombieLaboratory.zombiePlayer05.PlayerId) {
                                ZombieLaboratory.zombiePlayer05IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer06 != null && target.PlayerId == ZombieLaboratory.zombiePlayer06.PlayerId) {
                                ZombieLaboratory.zombiePlayer06IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer07 != null && target.PlayerId == ZombieLaboratory.zombiePlayer07.PlayerId) {
                                ZombieLaboratory.zombiePlayer07IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer08 != null && target.PlayerId == ZombieLaboratory.zombiePlayer08.PlayerId) {
                                ZombieLaboratory.zombiePlayer08IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer09 != null && target.PlayerId == ZombieLaboratory.zombiePlayer09.PlayerId) {
                                ZombieLaboratory.zombiePlayer09IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer10 != null && target.PlayerId == ZombieLaboratory.zombiePlayer10.PlayerId) {
                                ZombieLaboratory.zombiePlayer10IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer11 != null && target.PlayerId == ZombieLaboratory.zombiePlayer11.PlayerId) {
                                ZombieLaboratory.zombiePlayer11IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer12 != null && target.PlayerId == ZombieLaboratory.zombiePlayer12.PlayerId) {
                                ZombieLaboratory.zombiePlayer12IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer13 != null && target.PlayerId == ZombieLaboratory.zombiePlayer13.PlayerId) {
                                ZombieLaboratory.zombiePlayer13IsReviving = true;
                            }
                            else if (ZombieLaboratory.zombiePlayer14 != null && target.PlayerId == ZombieLaboratory.zombiePlayer14.PlayerId) {
                                ZombieLaboratory.zombiePlayer14IsReviving = true;
                            }
                            Helpers.alphaPlayer(true, player.PlayerId);
                            HudManager.Instance.StartCoroutine(Effects.Lerp(ZombieLaboratory.reviveTime, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    if (ZombieLaboratory.zombiePlayer01 != null && target.PlayerId == ZombieLaboratory.zombiePlayer01.PlayerId) {
                                        ZombieLaboratory.zombiePlayer01IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer02 != null && target.PlayerId == ZombieLaboratory.zombiePlayer02.PlayerId) {
                                        ZombieLaboratory.zombiePlayer02IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer03 != null && target.PlayerId == ZombieLaboratory.zombiePlayer03.PlayerId) {
                                        ZombieLaboratory.zombiePlayer03IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer04 != null && target.PlayerId == ZombieLaboratory.zombiePlayer04.PlayerId) {
                                        ZombieLaboratory.zombiePlayer04IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer05 != null && target.PlayerId == ZombieLaboratory.zombiePlayer05.PlayerId) {
                                        ZombieLaboratory.zombiePlayer05IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer06 != null && target.PlayerId == ZombieLaboratory.zombiePlayer06.PlayerId) {
                                        ZombieLaboratory.zombiePlayer06IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer07 != null && target.PlayerId == ZombieLaboratory.zombiePlayer07.PlayerId) {
                                        ZombieLaboratory.zombiePlayer07IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer08 != null && target.PlayerId == ZombieLaboratory.zombiePlayer08.PlayerId) {
                                        ZombieLaboratory.zombiePlayer08IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer09 != null && target.PlayerId == ZombieLaboratory.zombiePlayer09.PlayerId) {
                                        ZombieLaboratory.zombiePlayer09IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer10 != null && target.PlayerId == ZombieLaboratory.zombiePlayer10.PlayerId) {
                                        ZombieLaboratory.zombiePlayer10IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer11 != null && target.PlayerId == ZombieLaboratory.zombiePlayer11.PlayerId) {
                                        ZombieLaboratory.zombiePlayer11IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer12 != null && target.PlayerId == ZombieLaboratory.zombiePlayer12.PlayerId) {
                                        ZombieLaboratory.zombiePlayer12IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer13 != null && target.PlayerId == ZombieLaboratory.zombiePlayer13.PlayerId) {
                                        ZombieLaboratory.zombiePlayer13IsReviving = false;
                                    }
                                    else if (ZombieLaboratory.zombiePlayer14 != null && target.PlayerId == ZombieLaboratory.zombiePlayer14.PlayerId) {
                                        ZombieLaboratory.zombiePlayer14IsReviving = false;
                                    }
                                    Helpers.alphaPlayer(false, player.PlayerId);
                                }
                            })));

                            HudManager.Instance.StartCoroutine(Effects.Lerp(ZombieLaboratory.reviveTime - ZombieLaboratory.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && player != null) {
                                    player.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                player.transform.position = new Vector3(-4.85f, 6, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-17.25f, -13.25f, player.transform.position.z);
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            player.transform.position = new Vector3(18.5f, -1.85f, player.transform.position.z);
                                            break;
                                        // Polus
                                        case 2:
                                            player.transform.position = new Vector3(17.15f, -17.15f, player.transform.position.z);
                                            break;
                                        // Dleks
                                        case 3:
                                            player.transform.position = new Vector3(17.25f, -13.25f, player.transform.position.z);
                                            break;
                                        // Airship
                                        case 4:
                                            player.transform.position = new Vector3(32.35f, 7.25f, player.transform.position.z);
                                            break;
                                        // Submerged
                                        case 5:
                                            if (player.transform.position.y > 0) {
                                                player.transform.position = new Vector3(1f, 10f, player.transform.position.z);
                                            }
                                            else {
                                                player.transform.position = new Vector3(-4.15f, -33.5f, player.transform.position.z);
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));

                        }
                    }
                    ZombieLaboratory.zombieLaboratoryCounter = Language.introTexts[7] + "<color=#FF00FFFF>" + ZombieLaboratory.currentKeyItems + " / 6</color> | " + Language.introTexts[8] + "<color=#00CCFFFF>" + ZombieLaboratory.survivorTeam.Count + "</color> | " + Language.introTexts[9] + "<color=#FFFF00FF>" + ZombieLaboratory.infectedTeam.Count + "</color> | " + Language.introTexts[10] + "<color=#996633FF>" + ZombieLaboratory.zombieTeam.Count + "</color>";
                }

                // Battle Royale
                if (BattleRoyale.battleRoyaleMode) {
                    var body = UnityEngine.Object.FindObjectsOfType<DeadBody>().FirstOrDefault(b => b.ParentId == target.PlayerId);
                    body.transform.position = new Vector3(50, 50, 1);

                    if (BattleRoyale.matchType == 2) {
                        if (BattleRoyale.serialKiller != null && BattleRoyale.serialKiller.PlayerId == target.PlayerId) {
                            BattleRoyale.serialKillerIsReviving = true;
                            Helpers.alphaPlayer(true, BattleRoyale.serialKiller.PlayerId);
                            HudManager.Instance.StartCoroutine(Effects.Lerp(BattleRoyale.reviveTime, new Action<float>((p) => {
                                if (p == 1f && BattleRoyale.serialKiller != null) {
                                    BattleRoyale.serialKillerIsReviving = false;
                                    BattleRoyale.serialKillerLifes = BattleRoyale.fighterLifes * 3;
                                    Helpers.alphaPlayer(false, BattleRoyale.serialKiller.PlayerId);
                                }
                            })));
                            HudManager.Instance.StartCoroutine(Effects.Lerp(BattleRoyale.reviveTime - BattleRoyale.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                if (p == 1f && BattleRoyale.serialKiller != null) {
                                    BattleRoyale.serialKiller.Revive();
                                    switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                        // Skeld
                                        case 0:
                                            if (activatedSensei) {
                                                BattleRoyale.serialKiller.transform.position = new Vector3(-3.65f, 5f, PlayerControl.LocalPlayer.transform.position.z);
                                            }
                                            else {
                                                BattleRoyale.serialKiller.transform.position = new Vector3(6.35f, -7.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            }
                                            break;
                                        // MiraHQ
                                        case 1:
                                            BattleRoyale.serialKiller.transform.position = new Vector3(16.25f, 24.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            break;
                                        // Polus
                                        case 2:
                                            BattleRoyale.serialKiller.transform.position = new Vector3(22.3f, -19.15f, PlayerControl.LocalPlayer.transform.position.z);
                                            break;
                                        // Dleks
                                        case 3:
                                            BattleRoyale.serialKiller.transform.position = new Vector3(-6.35f, -7.5f, PlayerControl.LocalPlayer.transform.position.z);
                                            break;
                                        // Airship
                                        case 4:
                                            BattleRoyale.serialKiller.transform.position = new Vector3(12.25f, 2f, PlayerControl.LocalPlayer.transform.position.z);
                                            break;
                                        // Submerged
                                        case 5:
                                            if (BattleRoyale.serialKiller.transform.position.y > 0) {
                                                BattleRoyale.serialKiller.transform.position = new Vector3(5.75f, 31.25f, BattleRoyale.serialKiller.transform.position.z);
                                            }
                                            else {
                                                BattleRoyale.serialKiller.transform.position = new Vector3(-4.25f, -33.5f, BattleRoyale.serialKiller.transform.position.z);
                                            }
                                            break;
                                    }
                                    DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                    if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                    if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                }

                            })));

                        }

                        foreach (PlayerControl player in BattleRoyale.limeTeam) {
                            if (player.PlayerId == target.PlayerId) {

                                if (BattleRoyale.limePlayer01 != null && target.PlayerId == BattleRoyale.limePlayer01.PlayerId) {
                                    BattleRoyale.limePlayer01IsReviving = true;
                                }
                                else if (BattleRoyale.limePlayer02 != null && target.PlayerId == BattleRoyale.limePlayer02.PlayerId) {
                                    BattleRoyale.limePlayer02IsReviving = true;
                                }
                                else if (BattleRoyale.limePlayer03 != null && target.PlayerId == BattleRoyale.limePlayer03.PlayerId) {
                                    BattleRoyale.limePlayer03IsReviving = true;
                                }
                                else if (BattleRoyale.limePlayer04 != null && target.PlayerId == BattleRoyale.limePlayer04.PlayerId) {
                                    BattleRoyale.limePlayer04IsReviving = true;
                                }
                                else if (BattleRoyale.limePlayer05 != null && target.PlayerId == BattleRoyale.limePlayer05.PlayerId) {
                                    BattleRoyale.limePlayer05IsReviving = true;
                                }
                                else if (BattleRoyale.limePlayer06 != null && target.PlayerId == BattleRoyale.limePlayer06.PlayerId) {
                                    BattleRoyale.limePlayer06IsReviving = true;
                                }
                                else if (BattleRoyale.limePlayer07 != null && target.PlayerId == BattleRoyale.limePlayer07.PlayerId) {
                                    BattleRoyale.limePlayer07IsReviving = true;
                                }
                                Helpers.alphaPlayer(true, player.PlayerId);
                                HudManager.Instance.StartCoroutine(Effects.Lerp(BattleRoyale.reviveTime, new Action<float>((p) => {
                                    if (p == 1f && player != null) {
                                        if (BattleRoyale.limePlayer01 != null && target.PlayerId == BattleRoyale.limePlayer01.PlayerId) {
                                            BattleRoyale.limePlayer01IsReviving = false;
                                            BattleRoyale.limePlayer01Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.limePlayer02 != null && target.PlayerId == BattleRoyale.limePlayer02.PlayerId) {
                                            BattleRoyale.limePlayer02IsReviving = false;
                                            BattleRoyale.limePlayer02Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.limePlayer03 != null && target.PlayerId == BattleRoyale.limePlayer03.PlayerId) {
                                            BattleRoyale.limePlayer03IsReviving = false;
                                            BattleRoyale.limePlayer03Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.limePlayer04 != null && target.PlayerId == BattleRoyale.limePlayer04.PlayerId) {
                                            BattleRoyale.limePlayer04IsReviving = false;
                                            BattleRoyale.limePlayer04Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.limePlayer05 != null && target.PlayerId == BattleRoyale.limePlayer05.PlayerId) {
                                            BattleRoyale.limePlayer05IsReviving = false;
                                            BattleRoyale.limePlayer05Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.limePlayer06 != null && target.PlayerId == BattleRoyale.limePlayer06.PlayerId) {
                                            BattleRoyale.limePlayer06IsReviving = false;
                                            BattleRoyale.limePlayer06Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.limePlayer07 != null && target.PlayerId == BattleRoyale.limePlayer07.PlayerId) {
                                            BattleRoyale.limePlayer07IsReviving = false;
                                            BattleRoyale.limePlayer07Lifes = BattleRoyale.fighterLifes;
                                        }
                                        Helpers.alphaPlayer(false, player.PlayerId);
                                    }
                                })));
                                HudManager.Instance.StartCoroutine(Effects.Lerp(BattleRoyale.reviveTime - BattleRoyale.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                    if (p == 1f && player != null) {
                                        player.Revive();
                                        switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                            // Skeld
                                            case 0:
                                                if (activatedSensei) {
                                                    player.transform.position = new Vector3(-17.5f, -1.15f, player.transform.position.z);
                                                }
                                                else {
                                                    player.transform.position = new Vector3(-17f, -5.5f, player.transform.position.z);
                                                }
                                                break;
                                            // MiraHQ
                                            case 1:
                                                player.transform.position = new Vector3(6.15f, 13.25f, player.transform.position.z);
                                                break;
                                            // Polus
                                            case 2:
                                                player.transform.position = new Vector3(2.35f, -23.75f, player.transform.position.z);
                                                break;
                                            // Dleks
                                            case 3:
                                                player.transform.position = new Vector3(17f, -5.5f, player.transform.position.z);
                                                break;
                                            // Airship
                                            case 4:
                                                player.transform.position = new Vector3(-13.9f, -14.45f, player.transform.position.z);
                                                break;
                                            // Submerged
                                            case 5:
                                                if (player.transform.position.y > 0) {
                                                    player.transform.position = new Vector3(-12.25f, 18.5f, player.transform.position.z);
                                                }
                                                else {
                                                    player.transform.position = new Vector3(-14.5f, -34.35f, player.transform.position.z);
                                                }
                                                break;
                                        }
                                        DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                        if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                        if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                    }

                                })));

                            }
                        }
                        foreach (PlayerControl player in BattleRoyale.pinkTeam) {
                            if (player.PlayerId == target.PlayerId) {

                                if (BattleRoyale.pinkPlayer01 != null && target.PlayerId == BattleRoyale.pinkPlayer01.PlayerId) {
                                    BattleRoyale.pinkPlayer01IsReviving = true;
                                }
                                else if (BattleRoyale.pinkPlayer02 != null && target.PlayerId == BattleRoyale.pinkPlayer02.PlayerId) {
                                    BattleRoyale.pinkPlayer02IsReviving = true;
                                }
                                else if (BattleRoyale.pinkPlayer03 != null && target.PlayerId == BattleRoyale.pinkPlayer03.PlayerId) {
                                    BattleRoyale.pinkPlayer03IsReviving = true;
                                }
                                else if (BattleRoyale.pinkPlayer04 != null && target.PlayerId == BattleRoyale.pinkPlayer04.PlayerId) {
                                    BattleRoyale.pinkPlayer04IsReviving = true;
                                }
                                else if (BattleRoyale.pinkPlayer05 != null && target.PlayerId == BattleRoyale.pinkPlayer05.PlayerId) {
                                    BattleRoyale.pinkPlayer05IsReviving = true;
                                }
                                else if (BattleRoyale.pinkPlayer06 != null && target.PlayerId == BattleRoyale.pinkPlayer06.PlayerId) {
                                    BattleRoyale.pinkPlayer06IsReviving = true;
                                }
                                else if (BattleRoyale.pinkPlayer01 != null && target.PlayerId == BattleRoyale.pinkPlayer07.PlayerId) {
                                    BattleRoyale.pinkPlayer07IsReviving = true;
                                }
                                Helpers.alphaPlayer(true, player.PlayerId);
                                HudManager.Instance.StartCoroutine(Effects.Lerp(BattleRoyale.reviveTime, new Action<float>((p) => {
                                    if (p == 1f && player != null) {
                                        if (BattleRoyale.pinkPlayer01 != null && target.PlayerId == BattleRoyale.pinkPlayer01.PlayerId) {
                                            BattleRoyale.pinkPlayer01IsReviving = false;
                                            BattleRoyale.pinkPlayer01Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.pinkPlayer02 != null && target.PlayerId == BattleRoyale.pinkPlayer02.PlayerId) {
                                            BattleRoyale.pinkPlayer02IsReviving = false;
                                            BattleRoyale.pinkPlayer02Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.pinkPlayer03 != null && target.PlayerId == BattleRoyale.pinkPlayer03.PlayerId) {
                                            BattleRoyale.pinkPlayer03IsReviving = false;
                                            BattleRoyale.pinkPlayer03Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.pinkPlayer04 != null && target.PlayerId == BattleRoyale.pinkPlayer04.PlayerId) {
                                            BattleRoyale.pinkPlayer04IsReviving = false;
                                            BattleRoyale.pinkPlayer04Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.pinkPlayer05 != null && target.PlayerId == BattleRoyale.pinkPlayer05.PlayerId) {
                                            BattleRoyale.pinkPlayer05IsReviving = false;
                                            BattleRoyale.pinkPlayer05Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.pinkPlayer06 != null && target.PlayerId == BattleRoyale.pinkPlayer06.PlayerId) {
                                            BattleRoyale.pinkPlayer06IsReviving = false;
                                            BattleRoyale.pinkPlayer06Lifes = BattleRoyale.fighterLifes;
                                        }
                                        else if (BattleRoyale.pinkPlayer01 != null && target.PlayerId == BattleRoyale.pinkPlayer07.PlayerId) {
                                            BattleRoyale.pinkPlayer07IsReviving = false;
                                            BattleRoyale.pinkPlayer07Lifes = BattleRoyale.fighterLifes;
                                        }
                                        Helpers.alphaPlayer(false, player.PlayerId);
                                    }
                                })));

                                HudManager.Instance.StartCoroutine(Effects.Lerp(BattleRoyale.reviveTime - BattleRoyale.invincibilityTimeAfterRevive, new Action<float>((p) => {
                                    if (p == 1f && player != null) {
                                        player.Revive();
                                        switch (GameOptionsManager.Instance.currentGameOptions.MapId) {
                                            // Skeld
                                            case 0:
                                                if (activatedSensei) {
                                                    player.transform.position = new Vector3(7.7f, -0.95f, player.transform.position.z);
                                                }
                                                else {
                                                    player.transform.position = new Vector3(12f, -4.75f, player.transform.position.z);
                                                }
                                                break;
                                            // MiraHQ
                                            case 1:
                                                player.transform.position = new Vector3(22.25f, 3f, player.transform.position.z);
                                                break;
                                            // Polus
                                            case 2:
                                                player.transform.position = new Vector3(36.35f, -8f, player.transform.position.z);
                                                break;
                                            // Dleks
                                            case 3:
                                                player.transform.position = new Vector3(-12f, -4.75f, player.transform.position.z);
                                                break;
                                            // Airship
                                            case 4:
                                                player.transform.position = new Vector3(37.35f, -3.25f, player.transform.position.z);
                                                break;
                                            // Submerged
                                            case 5:
                                                if (player.transform.position.y > 0) {
                                                    player.transform.position = new Vector3(0f, 33.5f, player.transform.position.z);
                                                }
                                                else {
                                                    player.transform.position = new Vector3(-8.5f, -39.5f, player.transform.position.z);
                                                }
                                                break;
                                        }
                                        DeadPlayer deadPlayerEntry = deadPlayers.Where(x => x.player.PlayerId == target.PlayerId).FirstOrDefault();
                                        if (body != null) UnityEngine.Object.Destroy(body.gameObject);
                                        if (deadPlayerEntry != null) deadPlayers.Remove(deadPlayerEntry);
                                    }

                                })));

                            }
                        }
                    }
                }
            } else {
                // Check alive players for disable sabotage button if game result in 1vs1 special condition (impostor + rebel / impostor + captain / rebel + captain)
                alivePlayers = 0;
                foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                    if (!player.Data.IsDead) {
                        alivePlayers += 1;
                    }
                }               
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
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                        player.setDefaultLook();
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
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
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
            foreach (PlayerControl player in PlayerControl.AllPlayerControls) {
                if (!player.Data.IsDead) {
                    alivePlayers += 1;
                }
            }
        }
    }
}
