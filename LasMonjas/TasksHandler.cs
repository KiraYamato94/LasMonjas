using HarmonyLib;
using static LasMonjas.LasMonjas;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Hazel;

namespace LasMonjas
{
    [HarmonyPatch]
    public static class TasksHandler {

        public static Tuple<int, int> taskInfo(GameData.PlayerInfo playerInfo) {
            int TotalTasks = 0;
            int CompletedTasks = 0;
            if (!playerInfo.Disconnected && playerInfo.Tasks != null &&
                playerInfo.Object &&
                (PlayerControl.GameOptions.GhostsDoTasks || !playerInfo.IsDead) &&
                playerInfo.Role && playerInfo.Role.TasksCountTowardProgress &&
                !playerInfo.Object.hasFakeTasks()
                ) {
                for (int j = 0; j < playerInfo.Tasks.Count; j++) {
                    TotalTasks++;
                    if (playerInfo.Tasks[j].Complete) {
                        CompletedTasks++;
                    }
                }
            }
            return Tuple.Create(CompletedTasks, TotalTasks);
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
        private static class GameDataRecomputeTaskCountsPatch {
            private static bool Prefix(GameData __instance) {
                __instance.TotalTasks = 0;
                __instance.CompletedTasks = 0;
                for (int i = 0; i < __instance.AllPlayers.Count; i++) {
                    GameData.PlayerInfo playerInfo = __instance.AllPlayers[i];
                    if (playerInfo.Object
                    && playerInfo.Object.hasAliveKillingLover()) // Tasks do not count if a Crewmate has an alive killing Lover                                        
                        continue;
                    var (playerCompleted, playerTotal) = taskInfo(playerInfo);
                    __instance.TotalTasks += playerTotal;
                    __instance.CompletedTasks += playerCompleted;
                }
                return false;
            }
        }

        public static byte[] GetTaskMasterTasks(PlayerControl pc) {
            if (pc != TaskMaster.taskMaster)
                return null;

            List<byte> list = new List<byte>(10);

            TaskMaster.taskMasterAddCommonTasks = SetTasksToList(ref list, ShipStatus.Instance.CommonTasks.ToList(), Mathf.RoundToInt(TaskMaster.taskMasterAddCommonTasks));
            TaskMaster.taskMasterAddLongTasks = SetTasksToList(ref list, ShipStatus.Instance.LongTasks.ToList(), Mathf.RoundToInt(TaskMaster.taskMasterAddLongTasks));
            TaskMaster.taskMasterAddShortTasks = SetTasksToList(ref list, ShipStatus.Instance.NormalTasks.ToList(), Mathf.RoundToInt(TaskMaster.taskMasterAddShortTasks));

            return list.ToArray();
        }

        private static int SetTasksToList(ref List<byte> list, List<NormalPlayerTask> playerTasks, int numConfiguredTasks) {
            if (numConfiguredTasks == 0)
                return 0;
            List<TaskTypes> taskTypesList = new List<TaskTypes>();
            playerTasks.Shuffle();
            int count = 0;
            int numTasks = Math.Min(playerTasks.Count, numConfiguredTasks);
            for (int i = 0; i < playerTasks.Count; i++) {
                if (taskTypesList.Contains(playerTasks[i].TaskType))
                    continue;
                taskTypesList.Add(playerTasks[i].TaskType);
                ++count;
                list.Add((byte)playerTasks[i].Index);
                if (count >= numTasks)
                    break;
            }
            return count;
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.CompleteTask))]
        private static class GameDataCompleteTaskPatch
        {
            private static void Postfix(GameData __instance, [HarmonyArgument(0)] PlayerControl pc, [HarmonyArgument(1)] uint taskId) {
                
                if (TaskMaster.taskMaster != null && TaskMaster.taskMaster == PlayerControl.LocalPlayer && !TaskMaster.taskMaster.Data.IsDead) {
                    
                    var (playerCompleted, playerTotal) = taskInfo(TaskMaster.taskMaster.Data);
                    int numberOfLeftTasks = playerTotal - playerCompleted;

                    if (numberOfLeftTasks <= 0) {

                        if (TaskMaster.clearedInitialTasks) {

                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TaskMasterTriggerCrewWin, Hazel.SendOption.Reliable, -1);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.taskMasterTriggerCrewWin();
                        }
                        else {
                            byte[] taskTypeIds = GetTaskMasterTasks(TaskMaster.taskMaster);
                            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.TaskMasterSetExtraTasks, Hazel.SendOption.Reliable, -1);
                            writer.Write(TaskMaster.taskMaster.PlayerId);
                            writer.Write(taskTypeIds);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                            RPCProcedure.taskMasterSetExTasks(TaskMaster.taskMaster.PlayerId, byte.MaxValue, taskTypeIds);
                        }                       
                    }
                }
            }
        }
    }
}