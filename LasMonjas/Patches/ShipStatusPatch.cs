using System;
using System.Linq;
using HarmonyLib;
using UnhollowerRuntimeLib;
using static LasMonjas.LasMonjas;
using UnityEngine;

namespace LasMonjas.Patches
{

    [HarmonyPatch(typeof(ShipStatus))]
    public class ShipStatusPatch
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
        public static bool Prefix(ref float __result, ShipStatus __instance, [HarmonyArgument(0)] GameData.PlayerInfo player) {
            if (!__instance.Systems.ContainsKey(SystemTypes.Electrical)) return true;

            // Same crewmate vision for everyone on gamemodes with no vision change
            if (CaptureTheFlag.captureTheFlagMode || KingOfTheHill.kingOfTheHillMode) {
                if (player == null || player.IsDead) // IsDead
                    __result = __instance.MaxLightRadius;
                else {
                    foreach (PlayerControl gamemodePlayer in PlayerControl.AllPlayerControls) {
                        if (gamemodePlayer != null && gamemodePlayer.PlayerId == player.PlayerId) {
                            float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                            __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * PlayerControl.GameOptions.CrewLightMod;
                        }
                    }
                }
                return false;
            }
            else if (PoliceAndThief.policeAndThiefMode) {
                if (player == null || player.IsDead) // IsDead
                    __result = __instance.MaxLightRadius;

                else if (PoliceAndThief.policeplayer01 != null && PoliceAndThief.policeplayer01.PlayerId == player.PlayerId) {
                    if (PoliceAndThief.policeplayer01lightTimer > 0f) {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                    }
                    else {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                    }
                }
                else if (PoliceAndThief.policeplayer02 != null && PoliceAndThief.policeplayer02.PlayerId == player.PlayerId) {
                    if (PoliceAndThief.policeplayer02lightTimer > 0f) {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                    }
                    else {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                    }
                }
                else if (PoliceAndThief.policeplayer03 != null && PoliceAndThief.policeplayer03.PlayerId == player.PlayerId) {
                    if (PoliceAndThief.policeplayer03lightTimer > 0f) {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                    }
                    else {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                    }
                }
                else if (PoliceAndThief.policeplayer04 != null && PoliceAndThief.policeplayer04.PlayerId == player.PlayerId) {
                    if (PoliceAndThief.policeplayer04lightTimer > 0f) {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                    }
                    else {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                    }
                }
                else if (PoliceAndThief.policeplayer05 != null && PoliceAndThief.policeplayer05.PlayerId == player.PlayerId) {
                    if (PoliceAndThief.policeplayer05lightTimer > 0f) {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                    }
                    else {
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                    }
                }
                else {
                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                    __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * PlayerControl.GameOptions.CrewLightMod;
                }
                return false;
            }
            else if (HotPotato.hotPotatoMode) {
                if (player == null || player.IsDead) // IsDead
                    __result = __instance.MaxLightRadius;
                else if (HotPotato.hotPotatoPlayer != null && HotPotato.hotPotatoPlayer.PlayerId == player.PlayerId) {
                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                    __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * PlayerControl.GameOptions.CrewLightMod;
                }
                else {
                    foreach (PlayerControl notPotato in HotPotato.notPotatoTeam) {
                        if (notPotato != null && notPotato.PlayerId == player.PlayerId) {
                            float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                            __result = Mathf.Lerp(__instance.MinLightRadius * (HotPotato.notPotatoVision / 2), __instance.MaxLightRadius * (HotPotato.notPotatoVision / 2), unlerped);
                        }
                    }
                }
                return false;
            }
            else if (ZombieLaboratory.zombieLaboratoryMode) {
                if (player == null || player.IsDead) // IsDead
                    __result = __instance.MaxLightRadius;
                else {
                    foreach (PlayerControl survivor in ZombieLaboratory.survivorTeam) {
                        if (survivor != null && survivor.PlayerId == player.PlayerId) {
                            float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                            __result = Mathf.Lerp(__instance.MinLightRadius * (ZombieLaboratory.survivorsVision / 2), __instance.MaxLightRadius * (ZombieLaboratory.survivorsVision / 2), unlerped);
                        }
                    }
                    foreach (PlayerControl zombie in ZombieLaboratory.zombieTeam) {
                        if (zombie != null && zombie.PlayerId == player.PlayerId) {
                            float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                            __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * PlayerControl.GameOptions.CrewLightMod;
                        }
                    }
                }
                return false;
            }
            else {
                /*if (player == null || player.IsDead) // IsDead
                    __result = __instance.MaxLightRadius;
                else */

                if (player.Role.IsImpostor
                        || (Renegade.renegade != null && Renegade.renegade.PlayerId == player.PlayerId && Ilusionist.lightsOutTimer <= 0f)
                        || (Minion.minion != null && Minion.minion.PlayerId == player.PlayerId && Ilusionist.lightsOutTimer <= 0f)) {// Impostor, Renegade/Minion
                    __result = GetNeutralLightRadius(__instance, true);
                    return false;
                }

                if (Modifiers.blind != null && Modifiers.blind.PlayerId == player.PlayerId && Ilusionist.lightsOutTimer <= 0f) {// if player is Blind
                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                    __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * PlayerControl.GameOptions.CrewLightMod * 0.75f;
                    return false;
                }

                if (Modifiers.lighter != null && Modifiers.lighter.PlayerId == player.PlayerId && Ilusionist.lightsOutTimer <= 0f) {// if player is Lighter
                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                    __result = Mathf.Lerp(__instance.MaxLightRadius * 0.75f, __instance.MaxLightRadius * 2, unlerped);
                    return false;
                }

                if (Ilusionist.ilusionist != null && Ilusionist.lightsOutTimer > 0f) {
                    float lerpValue = 1f;
                    if (Ilusionist.lightsOutDuration - Ilusionist.lightsOutTimer < 0.5f) {
                        lerpValue = Mathf.Clamp01((Ilusionist.lightsOutDuration - Ilusionist.lightsOutTimer) * 2);
                    }
                    else if (Ilusionist.lightsOutTimer < 0.5) {
                        lerpValue = Mathf.Clamp01(Ilusionist.lightsOutTimer * 2);
                    }

                    __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, 1 - lerpValue) * PlayerControl.GameOptions.CrewLightMod;
                    return false;
                }

                // Default light radius
                __result = GetNeutralLightRadius(__instance, false);
                return false;
            }
        }

        public static float GetNeutralLightRadius(ShipStatus shipStatus, bool isImpostor) {
            if (SubmergedCompatibility.Loaded && shipStatus.Type == SubmergedCompatibility.SUBMERGED_MAP_TYPE) {
                return SubmergedCompatibility.GetSubmergedNeutralLightRadius(isImpostor);
            }

            if (isImpostor) return shipStatus.MaxLightRadius * PlayerControl.GameOptions.ImpostorLightMod;

            SwitchSystem switchSystem = shipStatus.Systems[SystemTypes.Electrical].TryCast<SwitchSystem>();
            float lerpValue = switchSystem.Value / 255f;

            return Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, lerpValue) * PlayerControl.GameOptions.CrewLightMod;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.IsGameOverDueToDeath))]
        public static void Postfix2(ShipStatus __instance, ref bool __result) {
            __result = false;
        }

    }

}