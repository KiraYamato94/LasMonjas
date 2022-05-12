using HarmonyLib;

namespace LasMonjas.Patches {
    [Harmony]
    public class AccountManagerPatch {
        [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.RandomizeName))]
        public static class RandomizeNamePatch {
            static bool Prefix(AccountManager __instance) {  
                if (SaveManager.lastPlayerName == null)
                    return true;
                SaveManager.PlayerName = SaveManager.lastPlayerName;
		        __instance.accountTab.UpdateNameDisplay();
                return false; 
            }
        }
    }
}