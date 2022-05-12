using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using Hazel;
using System;
using UnhollowerBaseLib;

namespace LasMonjas.Patches
{
    public static class LobbyBehaviour_Patch
    {
        private static GameObject prefab = null;
        private static GameObject getOldLobby(string name = "Lobby(Clone)") {

            GameObject lobby = GameObject.Find(name);
            if (lobby == null) {
                LasMonjasPlugin.Logger.LogFatal("Lobby error");
                throw new Exception("Lobby not found");
            }

            return lobby;

        }

        private static void LoadPrefab() {

            prefab = CustomMain.customAssets.customLobby;
            if (prefab == null) {
                LasMonjasPlugin.Logger.LogFatal("gameObject error");
                throw new Exception("Loading asset error");
            }

            Material shadowShader = null;
            GameObject background = GameObject.Find("Lobby(Clone)/Background");
            {
                SpriteRenderer sp = background.GetComponent<SpriteRenderer>();
                if (sp != null) {
                    shadowShader = sp.material;
                }
            }
            {
                SpriteRenderer sp = prefab.GetComponent<SpriteRenderer>();
                if (sp != null && shadowShader != null) {
                    sp.material = shadowShader;
                }
                else {
                    LasMonjasPlugin.Logger.LogFatal("material error");
                    throw new Exception("shadowShader not found");
                }
            }

        }
        
        private static string lobbyCodeText = "";

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public class GameStartManagerStartPatch
        {
            public static void Postfix(GameStartManager __instance) {
                // Copy lobby code before hide room code
                string code = InnerNet.GameCode.IntToGameName(AmongUsClient.Instance.GameId);
                GUIUtility.systemCopyBuffer = code;
                lobbyCodeText = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoomCode, new Il2CppReferenceArray<Il2CppSystem.Object>(0)) + "\r\n" + code;
            }
        }
        
        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
        public class GameStartManagerUpdatePatch
        {

            public static void Postfix(GameStartManager __instance) {               
                // Lobby code replacement if hide room code is enabled
                __instance.GameRoomName.text = LasMonjasPlugin.StreamerMode.Value ? $"<color={LasMonjasPlugin.StreamerModeReplacementColor.Value}>{LasMonjasPlugin.StreamerModeReplacementText.Value}</color>" : lobbyCodeText;
            }
        }
        
        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]

        class LobbyBehavour_Start_Patch
        {
            public static void Postfix(LobbyBehaviour __instance) {
                if (MapOptions.activateMusic) {
                    SoundManager.Instance.PlaySound(CustomMain.customAssets.lobbyMusic, true, 5f);
                }
                CaptureTheFlag.captureTheFlagMode = false;
                PoliceAndThief.policeAndThiefMode = false;
                KingOfTheHill.kingOfTheHillMode = false;
                HotPotato.hotPotatoMode = false;
                ZombieLaboratory.zombieLaboratoryMode = false;

                if (prefab == null) {
                    LoadPrefab();
                }

                GameObject instance = GameObject.Instantiate(prefab);
                instance.transform.position = new Vector3(0f, 0.85f, 0f);

                GameObject optionBox = GameObject.Find("Lobby(Clone)/SmallBox");
                if (optionBox == null) {
                    LasMonjasPlugin.Logger.LogFatal("PC Options error");
                    throw new Exception("PC Options not found");
                }

                GameObject wardrobeBox = GameObject.Find("Lobby(Clone)/panel_Wardrobe");
                if (wardrobeBox == null) {
                    LasMonjasPlugin.Logger.LogFatal("wardrobe error");
                    throw new Exception("wardrobe not found");
                }

                GameObject myOptionBox = GameObject.Instantiate(optionBox, new Vector3(8.47f, 2.75f, 0f), Quaternion.identity);
                myOptionBox.GetComponent<Collider2D>().enabled = false;
                myOptionBox.GetComponent<SpriteRenderer>().enabled = false;
                myOptionBox.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

                GameObject mywardrobeBox = GameObject.Instantiate(wardrobeBox, new Vector3(9, 0.37f, 0.0023f), Quaternion.identity);

                GameObject oldLobby = getOldLobby();
                oldLobby.GetComponent<Collider2D>().enabled = false;
                GameObject.Find("Lobby(Clone)/Background").SetActive(false);

                GameObject.Find("Lobby(Clone)/RightEngine").transform.position = new Vector3(12.45f, -0.3f, 0.5f);
                GameObject.Find("Lobby(Clone)/LeftEngine").transform.position = new Vector3(-4.775f, -3f, 0.5f);
                GameObject myLeftEngine = GameObject.Find("Lobby(Clone)/LeftEngine");
                GameObject.Instantiate(myLeftEngine, new Vector3(-4.775f, 1.75f, 0.5f), Quaternion.identity);
            }
        }

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
        class GameStartPatch
        {
            // Deactivate custom lobby items on game start
            public static void Prefix(ShipStatus __instance) {
                if (!DestroyableSingleton<TutorialManager>.InstanceExists) {
                    GameObject allulbackground = GameObject.Find("allul_customLobby(Clone)");
                    allulbackground.SetActive(false);
                    GameObject alluloptionbox = GameObject.Find("SmallBox(Clone)");
                    alluloptionbox.SetActive(false);
                    GameObject myLeftEngine = GameObject.Find("LeftEngine(Clone)");
                    myLeftEngine.SetActive(false);
                    GameObject myWardrobe = GameObject.Find("panel_Wardrobe(Clone)");
                    myWardrobe.SetActive(false);
                }
            }
        }
    }
}
