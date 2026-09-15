using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using static LasMonjas.LasMonjas;
using LasMonjas.Patches;
using System;

namespace LasMonjas.Core
{
    [HarmonyPatch]
    public static class ChatCommands
    {
        public static bool isLover(this PlayerControl player) => !(player == null) && (player == Modifiers.lover1 || player == Modifiers.lover2);

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class EnableChat
        {
            public static void Postfix(HudManager __instance) {
                if (!__instance.Chat.isActiveAndEnabled && (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay || PlayerInCache.LocalPlayer.PlayerControl.isLover()))
                    __instance.Chat.SetVisible(true);
            }
        }

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
        public static class AddChat
        {
            public static bool Prefix(ChatController __instance, [HarmonyArgument(0)] PlayerControl sourcePlayer) {
                if (__instance != FastDestroyableSingleton<HudManager>.Instance.Chat)
                    return true;
                PlayerControl localPlayer = PlayerInCache.LocalPlayer.PlayerControl;
                return localPlayer == null || (MeetingHud.Instance != null || LobbyBehaviour.Instance != null || (localPlayer.Data.IsDead || localPlayer.isLover() || (int)sourcePlayer.PlayerId == (int)PlayerInCache.LocalPlayer.PlayerControl.PlayerId));

            }
        }

        private static void Add(Dictionary<string, Func<string>> dict, Func<string> summary, params string[] aliases) {
            foreach (var alias in aliases)
                dict[alias] = summary;
        }

        private static Dictionary<string, Func<string>> CreateSummaries() {
            var summaries = new Dictionary<string, Func<string>>();

            // Impostor Roles
            Add(summaries, () => Language.impSummaryTexts[0], "mimic", "imitador", "ミミック", "化形者");
            Add(summaries, () => Language.impSummaryTexts[1], "painter", "pintor", "画家", "隐蔽者");
            Add(summaries, () => Language.impSummaryTexts[2], "demon", "demonio", "悪魔", "吸血鬼");
            Add(summaries, () => Language.impSummaryTexts[3], "janitor", "bedel", "管理人", "清理者");
            Add(summaries, () => Language.impSummaryTexts[4], "illusionist", "ilusionista", "イリュージョニスト", "骗术师");
            Add(summaries, () => Language.impSummaryTexts[5], "manipulator", "manipulador", "マニピュレータ", "术士");
            Add(summaries, () => Language.impSummaryTexts[6], "bomberman", "bomberman", "ボンバーマン", "爆破者");
            Add(summaries, () => Language.impSummaryTexts[7], "chameleon", "camaleon", "カメレオン", "隐身人");
            Add(summaries, () => Language.impSummaryTexts[8], "gambler", "apostador", "ギャンブラー", "赌徒");
            Add(summaries, () => Language.impSummaryTexts[9], "sorcerer", "hechicero", "ソーサラー", "女巫");
            Add(summaries, () => Language.impSummaryTexts[10], "medusa", "medusa", "メデューサ", "美杜莎");
            Add(summaries, () => Language.impSummaryTexts[11], "hypnotist", "hipnotizador", "催眠術師", "催眠师");
            Add(summaries, () => Language.impSummaryTexts[12], "archer", "arquero", "射手", "弓箭手");
            Add(summaries, () => Language.impSummaryTexts[13], "plumber", "fontanero", "配管工", "管道工");
            Add(summaries, () => Language.impSummaryTexts[14], "librarian", "bibliotecario", "司書", "勒索者");
            Add(summaries, () => Language.impSummaryTexts[15], "impostor", "impostor", "うそつき", "内鬼");

            // Rebel roles
            Add(summaries, () => Language.rebelSummaryTexts[0], "renegade", "renegado", "レネゲード", "变节者");
            Add(summaries, () => Language.rebelSummaryTexts[1], "bountyhunter", "cazarrecompensas", "賞金稼ぎ", "赏金猎人");
            Add(summaries, () => Language.rebelSummaryTexts[2], "trapper", "trampero", "トラッパー", "陷阱师");
            Add(summaries, () => Language.rebelSummaryTexts[3], "yinyanger", "yinyanero", "インヤンガー", "阴阳师");
            Add(summaries, () => Language.rebelSummaryTexts[4], "challenger", "desafiador", "チャレンジャー", "决斗者");
            Add(summaries, () => Language.rebelSummaryTexts[5], "ninja", "ninja", "忍者", "忍者");
            Add(summaries, () => Language.rebelSummaryTexts[6], "berserker", "berserker", "バーサーカー", "狂战士");
            Add(summaries, () => Language.rebelSummaryTexts[7], "yandere", "yandere", "ヤンデレ", "病娇");
            Add(summaries, () => Language.rebelSummaryTexts[8], "stranded", "naufrago", "座礁した", "探险家");
            Add(summaries, () => Language.rebelSummaryTexts[9], "monja", "monja", "もんじゃ", "古神");
            Add(summaries, () => Language.rebelSummaryTexts[10], "minion", "subdito", "ミニオン", "爪牙");

            // Neutral roles
            Add(summaries, () => Language.neutralSummaryTexts[0], "joker", "joker", "ジョーカー", "小丑");
            Add(summaries, () => Language.neutralSummaryTexts[1], "rolethief", "ladronderoles", "ロール泥棒", "身份窃贼");
            Add(summaries, () => Language.neutralSummaryTexts[2], "pyromaniac", "piromano", "放火魔", "纵火犯");
            Add(summaries, () => Language.neutralSummaryTexts[3], "treasurehunter", "cazatesoros", "トレジャーハンター", "寻宝猎人");
            Add(summaries, () => Language.neutralSummaryTexts[4], "devourer", "devorador", "むさぼり食う者", "秃鹫");
            Add(summaries, () => Language.neutralSummaryTexts[5], "poisoner", "envenenador", "毒殺者", "毒师");
            Add(summaries, () => Language.neutralSummaryTexts[6], "puppeteer", "titiritero", "操り人形師", "傀儡师");
            Add(summaries, () => Language.neutralSummaryTexts[7], "exiler", "exiliado", "亡命者", "处刑者");
            Add(summaries, () => Language.neutralSummaryTexts[8], "amnesiac", "amnesico", "健忘症", "失忆者");
            Add(summaries, () => Language.neutralSummaryTexts[9], "seeker", "buscador", "シーカー", "捉人鬼");

            // Crewmate roles
            Add(summaries, () => Language.crewSummaryTexts[0], "captain", "capitan", "キャプテン", "船长");
            Add(summaries, () => Language.crewSummaryTexts[1], "mechanic", "mecanico", "メカニック", "修理工");
            Add(summaries, () => Language.crewSummaryTexts[2], "sheriff", "sheriff", "シェリフ", "警长");
            Add(summaries, () => Language.crewSummaryTexts[3], "detective", "detective", "隠密", "侦探");
            Add(summaries, () => Language.crewSummaryTexts[4], "forensic", "forense", "フォレンジック", "法医");
            Add(summaries, () => Language.crewSummaryTexts[5], "timetraveler", "viajerotemporal", "タイムトラベラー", "时间之主");
            Add(summaries, () => Language.crewSummaryTexts[6], "squire", "defensor", "スクワイア", "卫兵");
            Add(summaries, () => Language.crewSummaryTexts[7], "cheater", "tramposo", "詐欺師", "换票师");
            Add(summaries, () => Language.crewSummaryTexts[8], "fortuneteller", "adivino", "占い師", "预言家");
            Add(summaries, () => Language.crewSummaryTexts[9], "hacker", "hacker", "ハッカー", "黑客");
            Add(summaries, () => Language.crewSummaryTexts[10], "sleuth", "sabueso", "探偵", "追踪者");
            Add(summaries, () => Language.crewSummaryTexts[11], "fink", "soplon", "フィンク", "告密者");
            Add(summaries, () => Language.crewSummaryTexts[12], "kid", "niño", "子供", "小孩");
            Add(summaries, () => Language.crewSummaryTexts[13], "welder", "soldador", "溶接機", "焊工");
            Add(summaries, () => Language.crewSummaryTexts[14], "spiritualist", "espiritista", "スピリチュアリスト", "殉道者");
            Add(summaries, () => Language.crewSummaryTexts[15], "vigilant", "vigilante", "警戒", "哨兵");
            Add(summaries, () => Language.crewSummaryTexts[16], "hunter", "cazador", "猟師", "猎人");
            Add(summaries, () => Language.crewSummaryTexts[17], "jinx", "gafe", "ジンクス", "扫把星");
            Add(summaries, () => Language.crewSummaryTexts[18], "coward", "cobarde", "腰抜け", "怯懦者");
            Add(summaries, () => Language.crewSummaryTexts[19], "bat", "murcielago", "コウモリ", "蝙蝠侠");
            Add(summaries, () => Language.crewSummaryTexts[20], "necromancer", "nigromante", "ネクロマンサー", "死灵法师");
            Add(summaries, () => Language.crewSummaryTexts[21], "engineer", "ingeniero", "エンジニア", "机关师");
            Add(summaries, () => Language.crewSummaryTexts[22], "locksmith", "cerrajero", "錠前屋", "锁匠");
            Add(summaries, () => Language.crewSummaryTexts[23], "taskmaster", "maestrodetareas", "タスクマスター", "工作达人");
            Add(summaries, () => Language.crewSummaryTexts[24], "jailer", "carcelero", "看守", "狱警");
            Add(summaries, () => Language.crewSummaryTexts[25], "crewmate", "tripulante", "乗組員", "船员");

            // Modifiers
            Add(summaries, () => Language.modifierSummaryTexts[0], "lover", "amante", "愛人", "恋人");
            Add(summaries, () => Language.modifierSummaryTexts[1], "lighter", "iluminador", "ライター", "火炬");
            Add(summaries, () => Language.modifierSummaryTexts[2], "blind", "ciego", "盲目", "失明者");
            Add(summaries, () => Language.modifierSummaryTexts[3], "flash", "flash", "閃光", "闪电侠");
            Add(summaries, () => Language.modifierSummaryTexts[4], "bigchungus", "bigchungus", "ビッグチャンガス", "巨人");
            Add(summaries, () => Language.modifierSummaryTexts[5], "thechosenone", "elelegido", "選ばれし者", "诱饵");
            Add(summaries, () => Language.modifierSummaryTexts[6], "performer", "teatrero", "パフォーマー", "广播员");
            Add(summaries, () => Language.modifierSummaryTexts[7], "pro", "pro", "プロ", "醉鬼");
            Add(summaries, () => Language.modifierSummaryTexts[8], "paintball", "boladepintura", "ペイントボール", "溅血者");
            Add(summaries, () => Language.modifierSummaryTexts[9], "electrician", "electricista", "電気技師", "电工");

            // Gamemodes
            Add(summaries, () => Language.gamemodeSummaryTexts[0], "capturetheflag", "capturalabandera", "旗を取れ", "夺旗赛");
            Add(summaries, () => Language.gamemodeSummaryTexts[1], "policeandthieves", "polisycacos", "警察と泥棒", "警察抓小偷");
            Add(summaries, () => Language.gamemodeSummaryTexts[2], "kingofthehill", "reydelacolina", "キングオブザヒル", "山丘之王");
            Add(summaries, () => Language.gamemodeSummaryTexts[3], "hotpotato", "patatacaliente", "焼き芋", "烫手山芋");
            Add(summaries, () => Language.gamemodeSummaryTexts[4], "zombielaboratory", "laboratoriozombie", "ゾンビ研究所", "生化危机");
            Add(summaries, () => Language.gamemodeSummaryTexts[5], "battleroyale", "batallacampal", "バトルロワイアル", "大逃杀");
            Add(summaries, () => Language.gamemodeSummaryTexts[6], "monjafestival", "monjafestival", "もんじゃ祭り", "玩偶狂欢");

            return summaries;
        }

        private static readonly Dictionary<string, Func<string>> Summaries = CreateSummaries();

        private static string GetSummary(string alias) {
            if (Summaries.TryGetValue(alias.ToLower(), out var summary)) {
                return summary();
            }

            return Language.helpersTexts[4];
        }

        [HarmonyPatch]
        public static class ChatCommandsInfo
        {
            [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
            private static class SendChatPatch
            {
                private static void ReloadLanguage(int language) {
                    // 1 English - 2 Spanish - 3 Japanese - 4 Chinese
                    LasMonjasPlugin.modLanguage.Value = language;                    
                    Language.LoadLanguage();
                    Helpers.UpdateLanguageForRoleSummary();
                }

                static bool Prefix(ChatController __instance) {
                    string text = __instance.freeChatField.textArea.text;
                    string subText = text.Split().Last();
                    string infoText = "";
                    bool handled = false;
                    if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) {
                        if (text.ToLower().StartsWith("/language ") || text.ToLower().StartsWith("/l ")) {
                            switch (subText.ToLower()) {
                                // Language change
                                case "english":
                                    ReloadLanguage(1);
                                    infoText = "Las Monjas: language changed to English";
                                    break;
                                case "spanish":
                                    infoText = "Las Monjas: idioma cambiado a Español";
                                    ReloadLanguage(2);
                                    break;
                                case "japanese":
                                    infoText = "Las Monjas: 言語を日本語に変更";
                                    ReloadLanguage(3);
                                    break;
                                case "chinese":
                                    infoText = "Las Monjas: 语言改为中文";
                                    ReloadLanguage(4);
                                    break;
                                default:
                                    infoText = "Las Monjas: language not supported";
                                    break;
                            }
                            handled = true;
                            __instance.AddChat(PlayerInCache.LocalPlayer.PlayerControl, infoText);
                        }

                        if (text.ToLower().StartsWith("/help ") || text.ToLower().StartsWith("/h ")) {
                            infoText = GetSummary(subText);
                            handled = true;
                            __instance.AddChat(PlayerInCache.LocalPlayer.PlayerControl, infoText);
                        }
                    }

                    if (MeetingHud.Instance != null && gameType <= 1) {
                        List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerInCache.LocalPlayer.PlayerControl);
                        RoleInfo roleInfo = infos.Where(info => info.TeamId != Team.Modifier).FirstOrDefault();
                        if (text.ToLower().StartsWith("/myrole")) {
                            if (roleInfo == null) {
                                infoText = Language.helpersTexts[3];
                            }
                            else {
                                infoText = GetSummary(roleInfo.roleId.ToString());
                            }
                            handled = true;
                            __instance.AddChat(PlayerInCache.LocalPlayer.PlayerControl, infoText);
                        }

                        RoleInfo roleInfoModifier = infos.Where(info => info.TeamId == Team.Modifier).FirstOrDefault();
                        if (text.ToLower().StartsWith("/mymodifier") || text.ToLower().StartsWith("/mymod")) {
                            if (roleInfoModifier == null) {
                                infoText = Language.modifierSummaryTexts[10];
                            }
                            else {
                                infoText = GetSummary(roleInfoModifier.roleId.ToString());
                            }
                            handled = true;
                            __instance.AddChat(PlayerInCache.LocalPlayer.PlayerControl, infoText);
                        }
                    }

                    if (handled) {
                        __instance.freeChatField.Clear();
                        __instance.quickChatMenu.Clear();
                    }
                    return !handled;
                }
            }
        }
    }
}