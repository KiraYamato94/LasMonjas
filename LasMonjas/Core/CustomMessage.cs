using UnityEngine;
using System.Collections.Generic;
using System;

namespace LasMonjas.Core {

    public class CustomMessage
    {

        private TMPro.TMP_Text text;
        private static List<CustomMessage> customMessages = new List<CustomMessage>();
        private int localBombNumber = 0;

        public CustomMessage(string message, float duration, int bombNumber, float localPosition, int whichmessage) {
            RoomTracker roomTracker = HudManager.Instance?.roomTracker;
            localBombNumber = bombNumber;
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
                            // Bomberman bomb warning
                            if (Bomberman.activeBomb == true && localBombNumber == Bomberman.currentBombNumber) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + Bomberman.bombTimer.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 2:
                            // Challenger duel timer
                            if (Challenger.isDueling == true && Challenger.duelDuration >= 0 && Challenger.onlyOneFinishDuel) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#4F7D00FF>");
                                text.text = prefix + message + Challenger.duelDuration.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 3:
                            // Capture the flag timer
                            if (CaptureTheFlag.captureTheFlagMode && CaptureTheFlag.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + CaptureTheFlag.matchDuration.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 4:
                            // Capture the flag which team scores
                            if (CaptureTheFlag.captureTheFlagMode && CaptureTheFlag.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 5:
                            // Capture the flag point counter
                            if (CaptureTheFlag.captureTheFlagMode && CaptureTheFlag.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + CaptureTheFlag.flagpointCounter + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 6:
                            // Polic and thiefs timer
                            if (PoliceAndThief.policeAndThiefMode && PoliceAndThief.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + PoliceAndThief.matchDuration.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 7:
                            // Police and thiefs warnings
                            if (PoliceAndThief.policeAndThiefMode && PoliceAndThief.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 8:
                            // Police and thiefs point counter
                            if (PoliceAndThief.policeAndThiefMode && PoliceAndThief.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + PoliceAndThief.thiefpointCounter + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 9:
                            // Ilusionist light out ability timer for other impostors
                            if (Ilusionist.lightsOutTimer > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + Ilusionist.lightsOutTimer.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 10:
                            // King of the hill timer
                            if (KingOfTheHill.kingOfTheHillMode && KingOfTheHill.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + KingOfTheHill.matchDuration.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 11:
                            // King of the hill which team captures
                            if (KingOfTheHill.kingOfTheHillMode && KingOfTheHill.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 12:
                            // King of the hill point counter
                            if (KingOfTheHill.kingOfTheHillMode && KingOfTheHill.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + KingOfTheHill.kingpointCounter + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 13:
                            // Fink camera use for other impostors
                            if (Fink.finkTimer > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 14:
                            // Petrify text
                            if (Medusa.messageTimer > 0) {
                                string prefix = (even ? "<color=#FCBA03FF>" : "<color=#FF0000FF>");
                                text.text = prefix + message + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 15:
                            // Hot potato game timer
                            if (HotPotato.hotPotatoMode && HotPotato.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + HotPotato.matchDuration.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 16:
                            // Hot potato potato changed
                            if (HotPotato.hotPotatoMode && HotPotato.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 17:
                            // Hot potato counter
                            if (HotPotato.hotPotatoMode && HotPotato.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + HotPotato.hotpotatopointCounter + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 18:
                            // Hot potato timer
                            if (HotPotato.hotPotatoMode && HotPotato.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + HotPotato.timeforTransfer.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 19:
                            // ZombieLaboratory timer
                            if (ZombieLaboratory.zombieLaboratoryMode && ZombieLaboratory.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + ZombieLaboratory.matchDuration.ToString("F0") + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 20:
                            // ZombieLaboratory point counter
                            if (ZombieLaboratory.zombieLaboratoryMode && ZombieLaboratory.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + ZombieLaboratory.zombieLaboratoryCounter + "</color>";
                            }
                            else {
                                text.text = "";
                            }
                            break;
                        case 21:
                            // ZombieLaboratory warnings
                            if (ZombieLaboratory.zombieLaboratoryMode && ZombieLaboratory.matchDuration >= 0) {
                                string prefix = ("<color=#FF8000FF>");
                                text.text = prefix + message + "</color>";
                            }
                            else {
                                text.text = "";
                            }
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