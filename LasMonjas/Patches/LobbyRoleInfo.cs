using HarmonyLib;
using LasMonjas.Core;
using Steamworks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LasMonjas.Patches
{
    public enum Team
    {
        Impostor,
        Rebel,
        Neutral,
        Crewmate,
        Modifier,
        Gamemode,
        Null
    }

    /*public static class LobbyRoleInfo
    {
        public static GameObject RolesSummaryUI { get; set; }
        public static readonly List<string> Teams = new() { "Impostors", "Rebels", "Neutrals", "Crewmates", "Modifiers", "Gamemodes" };
        private static TextMeshPro infoButtonText;
        private static TextMeshPro infoTitleText;

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class RoleSummaryButtonHudUpdate
        {
            public static void Postfix() {
                if (!LobbyBehaviour.Instance || AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) return;
                try {
                    HudManagerStartPatch.roleSummaryButton.Update();
                }
                catch { }
            }
        }

        public static void RoleSummaryOnClick() {
            if (RolesSummaryUI != null) return;

            SpriteRenderer container = new GameObject("RoleSummaryMenuContainer").AddComponent<SpriteRenderer>();
            container.sprite = Helpers.getRoleSummaryBackground();
            container.transform.SetParent(HudManager.Instance.transform);
            container.gameObject.transform.SetLocalZ(-200);
            container.transform.localPosition = new Vector3(0, -0.2f, -50f);
            container.transform.localScale = new Vector3(.75f, .7f, 1f);
            container.gameObject.layer = 5;

            RolesSummaryUI = container.gameObject;

            Transform buttonTemplate = HudManager.Instance.SettingsButton.transform;
            TextMeshPro textTemplate = HudManager.Instance.TaskPanel.taskText;

            TextMeshPro newtitle = Object.Instantiate(textTemplate, container.transform);
            newtitle.text = Language.helpersTexts[6];
            newtitle.color = Color.white;
            newtitle.outlineWidth = .25f;
            newtitle.transform.localPosition = new Vector3(1f, 0.1f, -2f);
            newtitle.transform.localScale = Vector3.one * 2.5f;

            List<Transform> buttons = new();

            for (int i = 0; i < Teams.Count; i++) {
                string team = "";
                Team teamid = Team.Null;
                switch (Teams[i]) {
                    case "Impostors":
                        team = Helpers.cs(Palette.ImpostorRed, Language.roleInfoRoleNames[61]);
                        teamid = Team.Impostor;
                        break;
                    case "Rebels":
                        team = Helpers.cs(Renegade.color, Language.teamNames[1]);
                        teamid = Team.Rebel;
                        break;
                    case "Neutrals":
                        team = Helpers.cs(Color.gray, Language.teamNames[0]);
                        teamid = Team.Neutral;
                        break;
                    case "Crewmates":
                        team = Helpers.cs(Palette.CrewmateBlue, Language.roleInfoRoleNames[62]);
                        teamid = Team.Crewmate;
                        break;
                    case "Modifiers":
                        team = Helpers.cs(Modifiers.color, Language.helpersTexts[7]);
                        teamid = Team.Modifier;
                        break;
                    case "Gamemodes":
                        team = Helpers.cs(Sheriff.color, Language.helpersTexts[8]);
                        teamid = Team.Gamemode;
                        break;
                }

                Transform buttonTransform = Object.Instantiate(buttonTemplate, container.transform);
                buttonTransform.name = team + " Button";
                buttonTransform.GetComponent<BoxCollider2D>().size = new Vector2(2.5f, 0.55f);
                buttonTransform.GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.RolePlate.png", 215f);
                buttons.Add(buttonTransform);
                buttonTransform.localPosition = new Vector3(0, 2.2f - i * 1f, -5);
                buttonTransform.localScale = new Vector3(2f, 1.5f, 1f);

                TextMeshPro label = Object.Instantiate(textTemplate, buttonTransform);
                label.text = team;
                label.alignment = TextAlignmentOptions.Center;
                label.transform.localPosition = new Vector3(0, 0, label.transform.localPosition.z);
                label.transform.localScale = new Vector3(1.6f, 2.3f, 1f);

                PassiveButton button = buttonTransform.GetComponent<PassiveButton>();
                button.OnClick.RemoveAllListeners();
                Button.ButtonClickedEvent onClick = button.OnClick = new Button.ButtonClickedEvent();
                onClick.AddListener((Action)(() => {
                    Object.Destroy(container.gameObject);
                    roleInfosOnclick(team, teamid);
                }));

                button.OnMouseOver.RemoveAllListeners();
                button.OnMouseOver.AddListener((Action)(() => {
                    buttonTransform.GetComponent<SpriteRenderer>().color = Color.yellow;
                }));

                button.OnMouseOut.RemoveAllListeners();
                button.OnMouseOut.AddListener((Action)(() => {
                    buttonTransform.GetComponent<SpriteRenderer>().color = Color.white;
                }));
            }
        }

        public static void roleInfosOnclick(string team, Team teamId) {
            SpriteRenderer container = new GameObject("RoleListMenuContainer").AddComponent<SpriteRenderer>();
            container.sprite = Helpers.getMenuBackground(); 
            container.transform.SetParent(HudManager.Instance.transform);
            container.transform.localPosition = new Vector3(0, -0.2f, -75f);
            container.transform.localScale = new Vector3(.7f, .7f, 1f);
            container.gameObject.layer = 5;
            RolesSummaryUI = container.gameObject;

            Transform buttonTemplate = HudManager.Instance.SettingsButton.transform;
            TextMeshPro textTemplate = HudManager.Instance.TaskPanel.taskText;

            TextMeshPro newtitle = Object.Instantiate(textTemplate, container.transform);
            newtitle.text = team;
            newtitle.outlineWidth = .25f;
            newtitle.transform.localPosition = new Vector3(0f, 2.7f, -2f);
            newtitle.transform.localScale = Vector3.one * 2.5f;

            List<Transform> buttons = new();
            int count = 0;
            bool gameStarted = AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started;
            foreach (RoleInfo roleInfo in RoleInfo.allRoleInfos) {
                if (roleInfo.TeamId != teamId) continue;

                Transform buttonTransform = Object.Instantiate(buttonTemplate, container.transform);
                buttonTransform.name = Helpers.cs(roleInfo.color, roleInfo.name) + " Button";
                buttonTransform.GetComponent<BoxCollider2D>().size = new Vector2(2.5f, 0.55f);
                TextMeshPro label = Object.Instantiate(textTemplate, buttonTransform);
                buttonTransform.GetComponent<SpriteRenderer>().sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.RolePlate2.png", 215f);
                buttons.Add(buttonTransform);
                int row = count / 3, col = count % 3;
                buttonTransform.localPosition = new Vector3(-3.205f + col * 3.2f, 2.9f - row * 0.75f, -5);
                buttonTransform.localScale = new Vector3(1.125f, 1.125f, 1f);
                label.text = Helpers.cs(roleInfo.color, roleInfo.name);
                label.alignment = TextAlignmentOptions.Center;
                label.transform.localPosition = new Vector3(0, 0, label.transform.localPosition.z);
                label.transform.localScale *= 1.55f;
                PassiveButton button = buttonTransform.GetComponent<PassiveButton>();
                button.OnClick.RemoveAllListeners();
                Button.ButtonClickedEvent onClick = button.OnClick = new Button.ButtonClickedEvent();
                onClick.AddListener((Action)(() => {
                    Object.Destroy(container.gameObject);
                    AddInfoCard(roleInfo);
                }));
                button.OnMouseOut.RemoveAllListeners();
                button.OnMouseOver.AddListener((Action)(() => {
                    buttonTransform.GetComponent<SpriteRenderer>().color = Color.yellow;
                }));
                button.OnMouseOut.RemoveAllListeners();
                button.OnMouseOut.AddListener((Action)(() => {
                    buttonTransform.GetComponent<SpriteRenderer>().color = Color.white;
                }));
                count++;
            }
        }
        static void AddInfoCard(RoleInfo roleInfo) {
            string roleSettingDescription = Helpers.WrapText(roleInfo.SettingsDescription);
            string coloredHelp = Helpers.cs(Color.white, roleSettingDescription);

            GameObject roleCard = Object.Instantiate(new GameObject("RoleCard"), HudManager.Instance.transform);
            SpriteRenderer roleCardRend = roleCard.AddComponent<SpriteRenderer>();
            roleCard.layer = 5;
            roleCard.transform.localPosition = new Vector3(0f, 0f, -150f);
            roleCard.transform.localScale = new Vector3(0.68f, 0.68f, 1f);
            RolesSummaryUI = roleCard.gameObject;

            roleCardRend.sprite = Helpers.loadSpriteFromResources("LasMonjas.Images.SummaryScreen.png", 110f);

            infoButtonText = Object.Instantiate(HudManager.Instance.TaskPanel.taskText, roleCard.transform);
            infoButtonText.color = Color.white;
            infoButtonText.text = coloredHelp;
            infoButtonText.enableWordWrapping = false;
            infoButtonText.transform.localScale = Vector3.one * 1.25f;
            infoButtonText.transform.localPosition = new Vector3(-2.7f, 0f, -50f);
            infoButtonText.alignment = TextAlignmentOptions.TopLeft;
            infoButtonText.fontStyle = FontStyles.Bold; 
            
            infoTitleText = Object.Instantiate(HudManager.Instance.TaskPanel.taskText, roleCard.transform);
            infoTitleText.color = Color.white;
            infoTitleText.text = Helpers.cs(roleInfo.color, roleInfo.name);
            infoTitleText.enableWordWrapping = false;
            infoTitleText.transform.localScale = Vector3.one * 3f;
            infoTitleText.transform.localPosition = new Vector3(0f, 2.4f, -50f);
            infoTitleText.alignment = TextAlignmentOptions.Center;
            infoTitleText.fontStyle = FontStyles.Bold;
        }
    }*/
}