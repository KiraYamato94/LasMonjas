using HarmonyLib;
using static LasMonjas.LasMonjas;
using UnityEngine;
using AmongUs.GameOptions;

namespace LasMonjas.Patches
{

    [HarmonyPatch(typeof(ShipStatus))]
    public class ShipStatusPatch
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
        public static bool Prefix(ref float __result, ShipStatus __instance, [HarmonyArgument(0)] GameData.PlayerInfo player) {
            if (!__instance.Systems.ContainsKey(SystemTypes.Electrical) || GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return true;

            switch (gameType) {
                case 0:
                case 1:
                default:
                    if (player.Role.IsImpostor
                        || (Renegade.renegade != null && Renegade.renegade.PlayerId == player.PlayerId && Illusionist.lightsOutTimer <= 0f)
                        || (Minion.minion != null && Minion.minion.PlayerId == player.PlayerId && Illusionist.lightsOutTimer <= 0f)) {// Impostor, Renegade/Minion
                        __result = GetNeutralLightRadius(__instance, true);
                        return false;
                    }

                    if (Modifiers.blind != null && Modifiers.blind.PlayerId == player.PlayerId && Illusionist.lightsOutTimer <= 0f) {// if player is Blind
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.CrewLightMod) * 0.75f;
                        return false;
                    }

                    if (Modifiers.lighter != null && Modifiers.lighter.PlayerId == player.PlayerId && Illusionist.lightsOutTimer <= 0f) {// if player is Lighter
                        float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                        __result = Mathf.Lerp(__instance.MaxLightRadius * 0.75f, __instance.MaxLightRadius * 2, unlerped);
                        return false;
                    }

                    if (Illusionist.illusionist != null && Illusionist.lightsOutTimer > 0f) {
                        float lerpValue = 1f;
                        if (Illusionist.lightsOutDuration - Illusionist.lightsOutTimer < 0.5f) {
                            lerpValue = Mathf.Clamp01((Illusionist.lightsOutDuration - Illusionist.lightsOutTimer) * 2);
                        }
                        else if (Illusionist.lightsOutTimer < 0.5) {
                            lerpValue = Mathf.Clamp01(Illusionist.lightsOutTimer * 2);
                        }

                        __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, 1 - lerpValue) * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.CrewLightMod);
                        return false;
                    }

                    // Default light radius
                    __result = GetNeutralLightRadius(__instance, false);
                    return false;
                case 2:
                case 4:
                case 7:
                case 8:
                    if (player == null || player.IsDead) // IsDead
                        __result = __instance.MaxLightRadius;
                    else {
                        foreach (PlayerControl gamemodePlayer in PlayerControl.AllPlayerControls) {
                            if (gamemodePlayer != null && gamemodePlayer.PlayerId == player.PlayerId) {
                                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.CrewLightMod);
                            }
                        }
                    }
                    return false;
                case 3:
                    if (player == null || player.IsDead) // IsDead
                        __result = __instance.MaxLightRadius;
                    else {
                        foreach (PlayerControl gamemodePlayer in PlayerControl.AllPlayerControls) {
                            if (gamemodePlayer != null && PoliceAndThief.policeplayer01 != null && gamemodePlayer == PoliceAndThief.policeplayer01 && PoliceAndThief.policeplayer01.PlayerId == player.PlayerId && PlayerControl.LocalPlayer == PoliceAndThief.policeplayer01) {
                                if (PoliceAndThief.policeplayer01lightTimer > 0f) {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                                }
                                else {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                                }
                                return false;
                            }
                            else if (gamemodePlayer != null && PoliceAndThief.policeplayer03 != null && gamemodePlayer == PoliceAndThief.policeplayer03 && PoliceAndThief.policeplayer03.PlayerId == player.PlayerId && PlayerControl.LocalPlayer == PoliceAndThief.policeplayer03) {
                                if (PoliceAndThief.policeplayer03lightTimer > 0f) {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                                }
                                else {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                                }
                                return false;
                            }
                            else if (gamemodePlayer != null && PoliceAndThief.policeplayer02 != null && gamemodePlayer == PoliceAndThief.policeplayer02 && PoliceAndThief.policeplayer02.PlayerId == player.PlayerId && PlayerControl.LocalPlayer == PoliceAndThief.policeplayer02) {
                                if (PoliceAndThief.policeplayer02lightTimer > 0f) {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                                }
                                else {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                                }
                                return false;
                            }
                            else if (gamemodePlayer != null && PoliceAndThief.policeplayer05 != null && gamemodePlayer == PoliceAndThief.policeplayer05 && PoliceAndThief.policeplayer05.PlayerId == player.PlayerId && PlayerControl.LocalPlayer == PoliceAndThief.policeplayer05) {
                                if (PoliceAndThief.policeplayer05lightTimer > 0f) {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                                }
                                else {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                                }
                                return false;
                            }
                            else if (gamemodePlayer != null && PoliceAndThief.policeplayer04 != null && gamemodePlayer == PoliceAndThief.policeplayer04 && PoliceAndThief.policeplayer04.PlayerId == player.PlayerId && PlayerControl.LocalPlayer == PoliceAndThief.policeplayer04) {
                                if (PoliceAndThief.policeplayer04lightTimer > 0f) {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                                }
                                else {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                                }
                                return false;
                            }
                            else if (gamemodePlayer != null && PoliceAndThief.policeplayer06 != null && gamemodePlayer == PoliceAndThief.policeplayer06 && PoliceAndThief.policeplayer06.PlayerId == player.PlayerId && PlayerControl.LocalPlayer == PoliceAndThief.policeplayer06) {
                                if (PoliceAndThief.policeplayer06lightTimer > 0f) {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * PoliceAndThief.policeVision, unlerped);
                                }
                                else {
                                    float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                    __result = Mathf.Lerp(__instance.MinLightRadius * (PoliceAndThief.policeVision / 2), __instance.MaxLightRadius * (PoliceAndThief.policeVision / 2), unlerped);
                                }
                                return false;
                            }
                            else {
                                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.CrewLightMod);
                            }
                        }
                    }
                    return false;
                case 5:
                    if (player == null || player.IsDead) // IsDead
                        __result = __instance.MaxLightRadius;
                    else {
                        foreach (PlayerControl gamemodePlayer in PlayerControl.AllPlayerControls) {
                            if (gamemodePlayer != null && HotPotato.hotPotatoPlayer != null && gamemodePlayer == HotPotato.hotPotatoPlayer && HotPotato.hotPotatoPlayer.PlayerId == player.PlayerId && PlayerControl.LocalPlayer == HotPotato.hotPotatoPlayer) {
                                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                __result = Mathf.Lerp(__instance.MinLightRadius, __instance.MaxLightRadius, unlerped) * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.CrewLightMod);
                            }
                            else
                                if (gamemodePlayer != null && gamemodePlayer.PlayerId == player.PlayerId) {
                                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                __result = Mathf.Lerp(__instance.MinLightRadius * (HotPotato.notPotatoVision / 2), __instance.MaxLightRadius * (HotPotato.notPotatoVision / 2), unlerped);
                            }
                        }
                    }
                    return false;
                case 6:
                    if (player == null || player.IsDead) // IsDead
                        __result = __instance.MaxLightRadius;
                    else {
                        foreach (PlayerControl gamemodePlayer in PlayerControl.AllPlayerControls) {
                            if (gamemodePlayer != null && ZombieLaboratory.nursePlayer != null && gamemodePlayer == ZombieLaboratory.nursePlayer && ZombieLaboratory.nursePlayer.PlayerId == player.PlayerId && PlayerControl.LocalPlayer == ZombieLaboratory.nursePlayer && ZombieLaboratory.currentKeyItems >= 3) {
                                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                __result = Mathf.Lerp(__instance.MinLightRadius * (ZombieLaboratory.survivorsVision / 1.5f), __instance.MaxLightRadius * (ZombieLaboratory.survivorsVision / 1.5f), unlerped);
                            }
                            else {
                                float unlerped = Mathf.InverseLerp(__instance.MinLightRadius, __instance.MaxLightRadius, GetNeutralLightRadius(__instance, false));
                                __result = Mathf.Lerp(__instance.MinLightRadius * (ZombieLaboratory.survivorsVision / 2), __instance.MaxLightRadius * (ZombieLaboratory.survivorsVision / 2), unlerped);
                            }
                        }
                    }
                    return false;
            }
        }

        public static float GetNeutralLightRadius(ShipStatus shipStatus, bool isImpostor) {
            if (SubmergedCompatibility.Loaded && shipStatus.Type == SubmergedCompatibility.SUBMERGED_MAP_TYPE) {
                return SubmergedCompatibility.GetSubmergedNeutralLightRadius(isImpostor);
            }

            if (isImpostor) return shipStatus.MaxLightRadius * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.ImpostorLightMod);

            SwitchSystem switchSystem = shipStatus.Systems[SystemTypes.Electrical].TryCast<SwitchSystem>();
            float lerpValue = switchSystem.Value / 255f;

            return Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, lerpValue) * GameOptionsManager.Instance.CurrentGameOptions.GetFloat(FloatOptionNames.CrewLightMod);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.IsGameOverDueToDeath))]
        public static void Postfix2(LogicGameFlowNormal __instance, ref bool __result) {
            __result = false;
        }

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.RepairSystem))]
        class RepairSystemPatch {
            public static bool Prefix(ShipStatus __instance, [HarmonyArgument(0)] SystemTypes systemType, [HarmonyArgument(1)] PlayerControl player, [HarmonyArgument(2)] byte amount) {

                // Mechanic expert repairs
                if (Mechanic.mechanic != null && Mechanic.mechanic == player && Mechanic.expertRepairs) {
                    switch (systemType) {
                        case SystemTypes.Reactor:
                            if (amount == 64 || amount == 65) {
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 67);
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 66);
                            }
                            if (amount == 16 || amount == 17) {
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 19);
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Reactor, 18);
                            }
                            break;
                        case SystemTypes.Laboratory:
                            if (amount == 64 || amount == 65) {
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Laboratory, 67);
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Laboratory, 66);
                            }
                            break;
                        case SystemTypes.LifeSupp:
                            if (amount == 64 || amount == 65) {
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 67);
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.LifeSupp, 66);
                            }
                            break;
                        case SystemTypes.Comms:
                            if (amount == 16 || amount == 17) {
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 19);
                                ShipStatus.Instance.RpcRepairSystem(SystemTypes.Comms, 18);
                            }
                            break;
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(SwitchSystem), nameof(SwitchSystem.RepairDamage))]
        class SwitchSystemRepairPatch
        {
            public static void Postfix(SwitchSystem __instance, [HarmonyArgument(0)] PlayerControl player, [HarmonyArgument(1)] byte amount) {

                // Mechanic expert lights repairs
                if (Mechanic.mechanic != null && Mechanic.mechanic == player && Mechanic.expertRepairs) {

                    if (amount >= 0 && amount <= 4) {
                        __instance.ActualSwitches = 0;
                        __instance.ExpectedSwitches = 0;
                    }

                }
            }
        }
    }
}