using UnityEngine;
using System.Collections.Generic;
using System;

namespace LasMonjas.Core {

    public class CustomMessage
    {

        private TMPro.TMP_Text text;
        private static List<CustomMessage> customMessages = new List<CustomMessage>();

        public CustomMessage(string message, float duration, float localPosition, int whichmessage) {
            RoomTracker roomTracker = HudManager.Instance?.roomTracker;
            if (roomTracker != null) {
                GameObject gameObject = UnityEngine.Object.Instantiate(roomTracker.gameObject);

                gameObject.transform.SetParent(HudManager.Instance.transform);
                UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<RoomTracker>());
                text = gameObject.GetComponent<TMPro.TMP_Text>();
                text.text = message;

                gameObject.transform.localPosition = new Vector3(0, localPosition, gameObject.transform.localPosition.z);
                customMessages.Add(this);

                HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {
                    bool even = ((int)(p * duration / 0.25f)) % 2 == 0;
                    switch (whichmessage) {
                        case 1:
                            // Illusionist light out ability timer for other impostors
                            while (Illusionist.lightsOutDuration > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + Illusionist.lightsOutTimer.ToString("F0") + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 2:
                            int localBombNumber = Bomberman.currentBombNumber;
                            // Bomberman bomb warning
                            while (Bomberman.activeBomb && localBombNumber == Bomberman.currentBombNumber) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + Bomberman.bombTimer.ToString("F0") + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 3:
                            // Petrify text
                            while (Medusa.messageTimer > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 4:
                            // Hypnotized text
                            while (Hypnotist.messageTimer > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 5:
                            // Challenger duel timer
                            while (Challenger.isDueling && Challenger.duelDuration >= 0 && Challenger.onlyOneFinishDuel) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#4F7D00FF>");
                                text.text = prefix + message + Challenger.duelDuration.ToString("F0") + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 6:
                            // Monja text
                            while (Monja.awakened && Monja.awakenTimer > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + Monja.awakenTimer.ToString("F0") + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 7:
                            // Seeker minigame timer
                            while (Seeker.isMinigaming && Seeker.minigameDuration >= 0 && Seeker.onlyOneFinishMinigame) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#808080FF>");
                                text.text = prefix + message + Seeker.minigameDuration.ToString("F0") + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 8:
                            // Seeker points warning text
                            while (Seeker.isMinigaming) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#808080FF>");
                                text.text = prefix + message + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 9:
                            // Fink camera use for other impostors
                            while (Fink.finkTimer > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#B80032FF>");
                                text.text = prefix + message + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 10:
                            // Speed text
                            while (Engineer.messageTimer > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#7F4C32FF>");
                                text.text = prefix + message + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 15:
                            // Gamemode match duration
                            while (LasMonjas.gameType >= 2 && LasMonjas.gamemodeMatchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + LasMonjas.gamemodeMatchDuration.ToString("F0") + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                        case 16:
                            // Gamemode warnings
                            if (LasMonjas.gameType >= 2 && LasMonjas.gamemodeMatchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + "</color>";
                            } else {
                                text.text = "";
                            }
                            break;
                        case 17:
                            // Gamemode progress counter
                            while (LasMonjas.gameType >= 2 && LasMonjas.gamemodeMatchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                switch (LasMonjas.gameType) {
                                    case 2:
                                        text.text = prefix + CaptureTheFlag.flagpointCounter + "</color>";
                                        break;
                                    case 3:
                                        text.text = prefix + PoliceAndThief.thiefpointCounter + "</color>";
                                        break;
                                    case 4:
                                        text.text = prefix + KingOfTheHill.kingpointCounter + "</color>";
                                        break;
                                    case 5:
                                        text.text = prefix + HotPotato.hotpotatopointCounter + "</color>";
                                        break;
                                    case 6:
                                        text.text = prefix + ZombieLaboratory.zombieLaboratoryCounter + "</color>";
                                        break;
                                    case 7:
                                        text.text = prefix + BattleRoyale.battleRoyalepointCounter + "</color>";
                                        break;
                                    case 8:
                                        text.text = prefix + MonjaFestival.monjaFestivalCounter + "</color>";
                                        break;
                                }
                                return;
                            }
                            text.text = "";
                            break;
                        case 18:
                            // Hot potato timer
                            while (LasMonjas.gameType == 5 && LasMonjas.gamemodeMatchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + HotPotato.timeforTransfer.ToString("F0") + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                    }
                    if (text != null) text.color = even ? Color.yellow : Color.red;
                    if (p == 1f && text != null && text.gameObject != null) {
                        UnityEngine.Object.Destroy(text.gameObject);
                        customMessages.Remove(this);
                    }
                })));
            }
        }
    }
}