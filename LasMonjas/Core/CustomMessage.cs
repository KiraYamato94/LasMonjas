using UnityEngine;
using System.Collections.Generic;
using System;

namespace LasMonjas.Core {

    public class CustomMessage
    {
        private TMPro.TMP_Text text;
        private static List<CustomMessage> customMessages = new List<CustomMessage>();

        private const string WarningColor = "<color=#FCBA03FF>";
        private const string ImpostorColor = "<color=#FF0000FF>";
        private const string RebelColor = "<color=#4F7D00FF>";
        private const string NeutralColor = "<color=#808080FF>";
        private const string CrewmateColor = "<color=#00FFFF>";
        private const string GamemodeColor = "<color=#FF8000FF>";

        private void SetMessage(bool condition, string colorA, string colorB, string content, bool even) {
            if (!condition) {
                text.text = "";
                return;
            }

            text.text = (even ? colorA : colorB) + content + "</color>";
        }

        private static string GetGamemodeCounter() {
            return LasMonjas.gameType switch {
                2 => CaptureTheFlag.flagpointCounter,
                3 => PoliceAndThief.thiefpointCounter,
                4 => KingOfTheHill.kingpointCounter,
                5 => HotPotato.hotpotatopointCounter,
                6 => ZombieLaboratory.zombieLaboratoryCounter,
                7 => BattleRoyale.battleRoyalepointCounter,
                8 => MonjaFestival.monjaFestivalCounter,
                _ => ""
            };
        }

        public CustomMessage(string message, float duration, Vector2 localPosition, int whichmessage) {
            RoomTracker roomTracker = HudManager.Instance?.roomTracker;
            if (roomTracker != null) {
                GameObject gameObject = UnityEngine.Object.Instantiate(roomTracker.gameObject);

                gameObject.transform.SetParent(HudManager.Instance.transform);
                UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<RoomTracker>());
                text = gameObject.GetComponent<TMPro.TMP_Text>();
                text.text = message;

                gameObject.transform.localPosition = new Vector3(localPosition.x, localPosition.y, gameObject.transform.localPosition.z);
                customMessages.Add(this);

                HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {
                    bool even = ((int)(p * duration / 0.25f)) % 2 == 0;
                    switch (whichmessage) {
                        case 1: // Illusionist light out ability timer for other impostors
                            SetMessage(Illusionist.lightsOutTimer > 0, WarningColor, ImpostorColor, message + Illusionist.lightsOutTimer.ToString("F0"), even);
                            return;
                        case 2: // Bomberman bomb warning
                            int localBombNumber = Bomberman.currentBombNumber;                            
                            SetMessage(Bomberman.activeBomb && localBombNumber == Bomberman.currentBombNumber, WarningColor, ImpostorColor, message + Bomberman.bombTimer.ToString("F0"), even);
                            return;
                        case 4: // Hypnotized text
                            SetMessage(Hypnotist.messageTimer > 0, WarningColor, ImpostorColor, message, even);
                            return;
                        case 5: // Challenger duel timer
                            SetMessage(Challenger.isDueling && Challenger.duelDuration >= 0 && Challenger.onlyOneFinishDuel, WarningColor, RebelColor, message + Challenger.duelDuration.ToString("F0"), even);
                            return;
                        case 6: // Monja text
                            SetMessage(Monja.awakened && Monja.awakenTimer > 0, WarningColor, RebelColor, message + Monja.awakenTimer.ToString("F0"), even);
                            return;
                        case 7: // Seeker minigame timer
                            SetMessage(Seeker.isMinigaming && Seeker.minigameDuration >= 0 && Seeker.onlyOneFinishMinigame, WarningColor, NeutralColor, message + Seeker.minigameDuration.ToString("F0"), even);
                            return; 
                        case 8: // Seeker points warning text
                            SetMessage(Seeker.isMinigaming && Seeker.minigameDuration >= 0 && Seeker.onlyOneFinishMinigame, WarningColor, NeutralColor, message, even);
                            return;
                        case 9: // Fink camera use for other impostors
                            SetMessage(Fink.finkTimer > 0, WarningColor, CrewmateColor, message, even);
                            return;
                        case 10: // Speed text
                            SetMessage(Engineer.messageTimer > 0, WarningColor, CrewmateColor, message, even);
                            return;
                        case 15: // Gamemode progress counter
                            text.alignment = TMPro.TextAlignmentOptions.Left;
                            while (LasMonjas.gameType >= 2 && LasMonjas.gamemodeMatchDuration >= 0) {
                                text.text = GamemodeColor + GetGamemodeCounter() + "</color>";
                                return;
                            }
                            text.text = "";
                            break;
                    }
                    if (p == 1f && text != null && text.gameObject != null) {
                        UnityEngine.Object.Destroy(text.gameObject);
                        customMessages.Remove(this);
                    }
                })));
            }
        }
    }
}