using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using static LasMonjas.LasMonjas;

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

        [HarmonyPatch]
        public static class ChatCommandsInfo
        {
            [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
            private static class SendChatPatch
            {
                private static void ReloadLanguage(int language) {
                    switch (language) {
                        // English
                        case 1:
                            LasMonjasPlugin.modLanguage.Value = 1;
                            break;
                        // Spanish
                        case 2:
                            LasMonjasPlugin.modLanguage.Value = 2;
                            break;
                        // Japanese
                        case 3:
                            LasMonjasPlugin.modLanguage.Value = 3;
                            break;
                        // Chinese
                        case 4:
                            LasMonjasPlugin.modLanguage.Value = 4;
                            break;
                    }
                    Language.LoadLanguage();
                }

                static bool Prefix(ChatController __instance) {
                    string text = __instance.freeChatField.textArea.text;
                    string subText = text.Split().Last();
                    string infoText = "";
                    bool handled = false;
                    if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) {
                        if (text.ToLower().StartsWith("/language ") || text.ToLower().StartsWith("/l ")) {
                            switch (subText.ToLower()) {
                                // Impostor roles
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
                            //CachedPlayer.LocalPlayer.PlayerControl.RpcSendChat(infoText);
                        }

                        if (text.ToLower().StartsWith("/help ") || text.ToLower().StartsWith("/h ")) {
                            switch (subText.ToLower()) {
                                // Impostor roles
                                case "mimic":
                                case "imitador":
                                case "ミミック":
                                case "化形者":
                                    infoText = Language.impSummaryTexts[0];
                                    break;
                                case "painter":
                                case "pintor":
                                case "画家":
                                case "隐蔽者":
                                    infoText = Language.impSummaryTexts[1];
                                    break;
                                case "demon":
                                case "demonio":
                                case "悪魔":
                                case "吸血鬼":
                                    infoText = Language.impSummaryTexts[2];
                                    break;
                                case "janitor":
                                case "bedel":
                                case "管理人":
                                case "清理者":
                                    infoText = Language.impSummaryTexts[3];
                                    break;
                                case "illusionist":
                                case "ilusionista":
                                case "イリュージョニスト":
                                case "骗术师":
                                    infoText = Language.impSummaryTexts[4];
                                    break;
                                case "manipulator":
                                case "manipulador":
                                case "マニピュレータ":
                                case "术士":
                                    infoText = Language.impSummaryTexts[5];
                                    break;
                                case "bomberman":
                                case "ボンバーマン":
                                case "爆破者":
                                    infoText = Language.impSummaryTexts[6];
                                    break;
                                case "chameleon":
                                case "camaleon":
                                case "カメレオン":
                                case "隐身人":
                                    infoText = Language.impSummaryTexts[7];
                                    break;
                                case "gambler":
                                case "apostador":
                                case "ギャンブラー":
                                case "赌徒":
                                    infoText = Language.impSummaryTexts[8];
                                    break;
                                case "sorcerer":
                                case "hechicero":
                                case "ソーサラー":
                                case "女巫":
                                    infoText = Language.impSummaryTexts[9];
                                    break;
                                case "medusa":
                                case "メデューサ":
                                case "美杜莎":
                                    infoText = Language.impSummaryTexts[10];
                                    break;
                                case "hypnotist":
                                case "hipnotizador":
                                case "催眠術師":
                                case "催眠师":
                                    infoText = Language.impSummaryTexts[11];
                                    break;
                                case "archer":
                                case "arquero":
                                case "弓箭手":
                                    infoText = Language.impSummaryTexts[12];
                                    break;
                                case "plumber":
                                case "fontanero":
                                case "管道工":
                                    infoText = Language.impSummaryTexts[13];
                                    break;
                                case "librarian":
                                case "bibliotecario":
                                case "勒索者":
                                    infoText = Language.impSummaryTexts[14];
                                    break;
                                case "impostor":
                                case "うそつき":
                                case "内鬼":
                                    infoText = Language.impSummaryTexts[15];
                                    break;

                                // Rebel roles
                                case "renegade":
                                case "renegado":
                                case "レネゲード":
                                case "变节者":
                                    infoText = Language.rebelSummaryTexts[0];
                                    break;
                                case "bountyhunter":
                                case "cazarrecompensas":
                                case "賞金稼ぎ":
                                case "赏金猎人":
                                    infoText = Language.rebelSummaryTexts[1];
                                    break;
                                case "trapper":
                                case "trampero":
                                case "トラッパー":
                                case "陷阱师":
                                    infoText = Language.rebelSummaryTexts[2];
                                    break;
                                case "yinyanger":
                                case "yinyanero":
                                case "インヤンガー":
                                case "阴阳师":
                                    infoText = Language.rebelSummaryTexts[3];
                                    break;
                                case "challenger":
                                case "desafiador":
                                case "チャレンジャー":
                                case "决斗者":
                                    infoText = Language.rebelSummaryTexts[4];
                                    break;
                                case "ninja":
                                case "忍者":
                                    infoText = Language.rebelSummaryTexts[5];
                                    break;
                                case "berserker":
                                case "バーサーカー":
                                case "狂战士":
                                    infoText = Language.rebelSummaryTexts[6];
                                    break;
                                case "yandere":
                                case "ヤンデレ":
                                case "病娇":
                                    infoText = Language.rebelSummaryTexts[7];
                                    break;
                                case "stranded":
                                case "naufrago":
                                case "座礁した":
                                case "探险家":
                                    infoText = Language.rebelSummaryTexts[8];
                                    break;
                                case "monja":
                                case "もんじゃ":
                                case "古神":
                                    infoText = Language.rebelSummaryTexts[9];
                                    break;

                                // Neutral roles
                                case "joker":
                                case "ジョーカー":
                                case "小丑":
                                    infoText = Language.neutralSummaryTexts[0];
                                    break;
                                case "rolethief":
                                case "ladronderoles":
                                case "ロール泥棒":
                                case "身份窃贼":
                                    infoText = Language.neutralSummaryTexts[1];
                                    break;
                                case "pyromaniac":
                                case "piromano":
                                case "放火魔":
                                case "纵火犯":
                                    infoText = Language.neutralSummaryTexts[2];
                                    break;
                                case "treasurehunter":
                                case "cazatesoros":
                                case "トレジャーハンター":
                                case "寻宝猎人":
                                    infoText = Language.neutralSummaryTexts[3];
                                    break;
                                case "devourer":
                                case "devorador":
                                case "むさぼり食う者":
                                case "秃鹫":
                                    infoText = Language.neutralSummaryTexts[4];
                                    break;
                                case "poisoner":
                                case "envenenador":
                                case "毒殺者":
                                case "毒师":
                                    infoText = Language.neutralSummaryTexts[5];
                                    break;
                                case "puppeteer":
                                case "titiritero":
                                case "操り人形師":
                                case "傀儡师":
                                    infoText = Language.neutralSummaryTexts[6];
                                    break;
                                case "exiler":
                                case "exiliado":
                                case "亡命者":
                                case "处刑者":
                                    infoText = Language.neutralSummaryTexts[7];
                                    break;
                                case "amnesiac":
                                case "amnesico":
                                case "健忘症":
                                case "失忆者":
                                    infoText = Language.neutralSummaryTexts[8];
                                    break;
                                case "seeker":
                                case "buscador":
                                case "シーカー":
                                case "捉人鬼":
                                    infoText = Language.neutralSummaryTexts[9];
                                    break;

                                // Crewmate roles
                                case "captain":
                                case "capitan":
                                case "キャプテン":
                                case "船长":
                                    infoText = Language.crewSummaryTexts[0];
                                    break;
                                case "mechanic":
                                case "mecanico":
                                case "メカニック":
                                case "修理工":
                                    infoText = Language.crewSummaryTexts[1];
                                    break;
                                case "sheriff":
                                case "シェリフ":
                                case "警长":
                                    infoText = Language.crewSummaryTexts[2];
                                    break;
                                case "detective":
                                case "隠密":
                                case "侦探":
                                    infoText = Language.crewSummaryTexts[3];
                                    break;
                                case "forensic":
                                case "forense":
                                case "フォレンジック":
                                case "法医":
                                    infoText = Language.crewSummaryTexts[4];
                                    break;
                                case "timetraveler":
                                case "viajerotemporal":
                                case "タイムトラベラー":
                                case "时间之主":
                                    infoText = Language.crewSummaryTexts[5];
                                    break;
                                case "squire":
                                case "defensor":
                                case "スクワイア":
                                case "卫兵":
                                    infoText = Language.crewSummaryTexts[6];
                                    break;
                                case "cheater":
                                case "tramposo":
                                case "詐欺師":
                                case "换票师":
                                    infoText = Language.crewSummaryTexts[7];
                                    break;
                                case "fortuneteller":
                                case "adivino":
                                case "占い師":
                                case "预言家":
                                    infoText = Language.crewSummaryTexts[8];
                                    break;
                                case "hacker":
                                case "ハッカー":
                                case "黑客":
                                    infoText = Language.crewSummaryTexts[9];
                                    break;
                                case "sleuth":
                                case "sabueso":
                                case "探偵":
                                case "追踪者":
                                    infoText = Language.crewSummaryTexts[10];
                                    break;
                                case "fink":
                                case "soplon":
                                case "フィンク":
                                case "告密者":
                                    infoText = Language.crewSummaryTexts[11];
                                    break;
                                case "kid":
                                case "niño":
                                case "子供":
                                case "小孩":
                                    infoText = Language.crewSummaryTexts[12];
                                    break;
                                case "welder":
                                case "soldador":
                                case "溶接機":
                                case "焊工":
                                    infoText = Language.crewSummaryTexts[13];
                                    break;
                                case "spiritualist":
                                case "espiritista":
                                case "スピリチュアリスト":
                                case "殉道者":
                                    infoText = Language.crewSummaryTexts[14];
                                    break;
                                case "vigilant":
                                case "vigilante":
                                case "警戒":
                                case "哨兵":
                                    infoText = Language.crewSummaryTexts[15];
                                    break;
                                case "hunter":
                                case "cazador":
                                case "猟師":
                                case "猎人":
                                    infoText = Language.crewSummaryTexts[16];
                                    break;
                                case "jinx":
                                case "gafe":
                                case "ジンクス":
                                case "扫把星":
                                    infoText = Language.crewSummaryTexts[17];
                                    break;
                                case "coward":
                                case "cobarde":
                                case "腰抜け":
                                case "怯懦者":
                                    infoText = Language.crewSummaryTexts[18];
                                    break;
                                case "bat":
                                case "murcielago":
                                case "コウモリ":
                                case "蝙蝠侠":
                                    infoText = Language.crewSummaryTexts[19];
                                    break;
                                case "necromancer":
                                case "nigromante":
                                case "ネクロマンサー":
                                case "死灵法师":
                                    infoText = Language.crewSummaryTexts[20];
                                    break;
                                case "engineer":
                                case "ingeniero":
                                case "エンジニア":
                                case "机关师":
                                    infoText = Language.crewSummaryTexts[21];
                                    break;
                                case "shy":
                                case "timido":
                                case "シャイ":
                                case "内敛者":
                                    infoText = Language.crewSummaryTexts[22];
                                    break;
                                case "taskmaster":
                                case "maestrodetareas":
                                case "タスクマスター":
                                case "工作达人":
                                    infoText = Language.crewSummaryTexts[23];
                                    break;
                                case "jailer":
                                case "carcelero":
                                case "看守":
                                case "狱警":
                                    infoText = Language.crewSummaryTexts[24];
                                    break;
                                case "crewmate":
                                case "tripulante":
                                case "乗組員":
                                case "船员":
                                    infoText = Language.crewSummaryTexts[25];
                                    break;

                                // Modifiers:
                                case "lover":
                                case "amante":
                                case "愛人":
                                case "恋人":
                                    infoText = Language.modifierSummaryTexts[1];
                                    break;
                                case "lighter":
                                case "iluminador":
                                case "ライター":
                                case "火炬":
                                    infoText = Language.modifierSummaryTexts[2];
                                    break;
                                case "blind":
                                case "ciego":
                                case "盲目":
                                case "失明者":
                                    infoText = Language.modifierSummaryTexts[3];
                                    break;
                                case "flash":
                                case "閃光":
                                case "闪电侠":
                                    infoText = Language.modifierSummaryTexts[4];
                                    break;
                                case "bigchungus":
                                case "ビッグチャンガス":
                                case "巨人":
                                    infoText = Language.modifierSummaryTexts[5];
                                    break;
                                case "thechosenone":
                                case "elelegido":
                                case "選ばれし者":
                                case "诱饵":
                                    infoText = Language.modifierSummaryTexts[6];
                                    break;
                                case "performer":
                                case "teatrero":
                                case "パフォーマー":
                                case "广播员":
                                    infoText = Language.modifierSummaryTexts[7];
                                    break;
                                case "pro":
                                case "プロ":
                                case "醉鬼":
                                    infoText = Language.modifierSummaryTexts[8];
                                    break;
                                case "paintball":
                                case "ペイントボール":
                                case "溅血者":
                                    infoText = Language.modifierSummaryTexts[9];
                                    break;
                                case "electrician":
                                case "electricista":
                                case "電気技師":
                                case "电工":
                                    infoText = Language.modifierSummaryTexts[10];
                                    break;

                                // Gamemodes:
                                case "capturetheflag":
                                case "ctf":
                                case "capturalabandera":
                                case "旗を取れ":
                                case "夺旗赛":
                                    infoText = Language.gamemodeSummaryTexts[0];
                                    break;
                                case "policeandthieves":
                                case "pat":
                                case "polisycacos":
                                case "警察と泥棒":
                                case "警察抓小偷":
                                    infoText = Language.gamemodeSummaryTexts[1];
                                    break;
                                case "kingofthehill":
                                case "koth":
                                case "reydelacolina":
                                case "キングオブザヒル":
                                case "山丘之王":
                                    infoText = Language.gamemodeSummaryTexts[2];
                                    break;
                                case "hotpotato":
                                case "hp":
                                case "patatacaliente":
                                case "焼き芋":
                                case "烫手山芋":
                                    infoText = Language.gamemodeSummaryTexts[3];
                                    break;
                                case "zombielaboratory":
                                case "zl":
                                case "laboratoriozombie":
                                case "ゾンビ研究所":
                                case "生化危机":
                                    infoText = Language.gamemodeSummaryTexts[4];
                                    break;
                                case "battleroyale":
                                case "br":
                                case "batallacampal":
                                case "バトルロワイアル":
                                case "大逃杀":
                                    infoText = Language.gamemodeSummaryTexts[5];
                                    break;
                                case "monjafestival":
                                case "mf":
                                case "もんじゃ祭り":
                                case "玩偶狂欢":
                                    infoText = Language.gamemodeSummaryTexts[6];
                                    break;

                                // not defined or wrong write
                                default:
                                    infoText = Language.helpersTexts[4];
                                    break;
                            }
                            handled = true;
                            __instance.AddChat(PlayerInCache.LocalPlayer.PlayerControl, infoText);
                            //CachedPlayer.LocalPlayer.PlayerControl.RpcSendChat(infoText);
                        }
                    }

                    if (MeetingHud.Instance != null && gameType <= 1) {
                        List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(PlayerInCache.LocalPlayer.PlayerControl);
                        RoleInfo roleInfo = infos.Where(info => !info.isModifier).FirstOrDefault();
                        if (text.ToLower().StartsWith("/myrole")) {
                            if (roleInfo == null) {
                                infoText = Language.helpersTexts[3];
                            }
                            else {
                                switch (roleInfo.roleId.ToString().ToLower()) {
                                    // Impostor roles
                                    case "mimic":
                                    case "imitador":
                                    case "ミミック":
                                    case "化形者":
                                        infoText = Language.impSummaryTexts[0];
                                        break;
                                    case "painter":
                                    case "pintor":
                                    case "画家":
                                    case "隐蔽者":
                                        infoText = Language.impSummaryTexts[1];
                                        break;
                                    case "demon":
                                    case "demonio":
                                    case "悪魔":
                                    case "吸血鬼":
                                        infoText = Language.impSummaryTexts[2];
                                        break;
                                    case "janitor":
                                    case "bedel":
                                    case "管理人":
                                    case "清理者":
                                        infoText = Language.impSummaryTexts[3];
                                        break;
                                    case "illusionist":
                                    case "ilusionista":
                                    case "イリュージョニスト":
                                    case "骗术师":
                                        infoText = Language.impSummaryTexts[4];
                                        break;
                                    case "manipulator":
                                    case "manipulador":
                                    case "マニピュレータ":
                                    case "术士":
                                        infoText = Language.impSummaryTexts[5];
                                        break;
                                    case "bomberman":
                                    case "ボンバーマン":
                                    case "爆破者":
                                        infoText = Language.impSummaryTexts[6];
                                        break;
                                    case "chameleon":
                                    case "camaleon":
                                    case "カメレオン":
                                    case "隐身人":
                                        infoText = Language.impSummaryTexts[7];
                                        break;
                                    case "gambler":
                                    case "apostador":
                                    case "ギャンブラー":
                                    case "赌徒":
                                        infoText = Language.impSummaryTexts[8];
                                        break;
                                    case "sorcerer":
                                    case "hechicero":
                                    case "ソーサラー":
                                    case "女巫":
                                        infoText = Language.impSummaryTexts[9];
                                        break;
                                    case "medusa":
                                    case "メデューサ":
                                    case "美杜莎":
                                        infoText = Language.impSummaryTexts[10];
                                        break;
                                    case "hypnotist":
                                    case "hipnotizador":
                                    case "催眠術師":
                                    case "催眠师":
                                        infoText = Language.impSummaryTexts[11];
                                        break;
                                    case "archer":
                                    case "arquero":
                                    case "弓箭手":
                                        infoText = Language.impSummaryTexts[12];
                                        break;
                                    case "plumber":
                                    case "fontanero":
                                    case "管道工":
                                        infoText = Language.impSummaryTexts[13];
                                        break;
                                    case "librarian":
                                    case "bibliotecario":
                                    case "勒索者":
                                        infoText = Language.impSummaryTexts[14];
                                        break;
                                    case "impostor":
                                    case "うそつき":
                                    case "内鬼":
                                        infoText = Language.impSummaryTexts[15];
                                        break;

                                    // Rebel roles
                                    case "renegade":
                                    case "renegado":
                                    case "レネゲード":
                                    case "变节者":
                                        infoText = Language.rebelSummaryTexts[0];
                                        break;
                                    case "bountyhunter":
                                    case "cazarrecompensas":
                                    case "賞金稼ぎ":
                                    case "赏金猎人":
                                        infoText = Language.rebelSummaryTexts[1];
                                        break;
                                    case "trapper":
                                    case "trampero":
                                    case "トラッパー":
                                    case "陷阱师":
                                        infoText = Language.rebelSummaryTexts[2];
                                        break;
                                    case "yinyanger":
                                    case "yinyanero":
                                    case "インヤンガー":
                                    case "阴阳师":
                                        infoText = Language.rebelSummaryTexts[3];
                                        break;
                                    case "challenger":
                                    case "desafiador":
                                    case "チャレンジャー":
                                    case "决斗者":
                                        infoText = Language.rebelSummaryTexts[4];
                                        break;
                                    case "ninja":
                                    case "忍者":
                                        infoText = Language.rebelSummaryTexts[5];
                                        break;
                                    case "berserker":
                                    case "バーサーカー":
                                    case "狂战士":
                                        infoText = Language.rebelSummaryTexts[6];
                                        break;
                                    case "yandere":
                                    case "ヤンデレ":
                                    case "病娇":
                                        infoText = Language.rebelSummaryTexts[7];
                                        break;
                                    case "stranded":
                                    case "naufrago":
                                    case "座礁した":
                                    case "探险家":
                                        infoText = Language.rebelSummaryTexts[8];
                                        break;
                                    case "monja":
                                    case "もんじゃ":
                                    case "古神":
                                        infoText = Language.rebelSummaryTexts[9];
                                        break;

                                    // Neutral roles
                                    case "joker":
                                    case "ジョーカー":
                                    case "小丑":
                                        infoText = Language.neutralSummaryTexts[0];
                                        break;
                                    case "rolethief":
                                    case "ladronderoles":
                                    case "ロール泥棒":
                                    case "身份窃贼":
                                        infoText = Language.neutralSummaryTexts[1];
                                        break;
                                    case "pyromaniac":
                                    case "piromano":
                                    case "放火魔":
                                    case "纵火犯":
                                        infoText = Language.neutralSummaryTexts[2];
                                        break;
                                    case "treasurehunter":
                                    case "cazatesoros":
                                    case "トレジャーハンター":
                                    case "寻宝猎人":
                                        infoText = Language.neutralSummaryTexts[3];
                                        break;
                                    case "devourer":
                                    case "devorador":
                                    case "むさぼり食う者":
                                    case "秃鹫":
                                        infoText = Language.neutralSummaryTexts[4];
                                        break;
                                    case "poisoner":
                                    case "envenenador":
                                    case "毒殺者":
                                    case "毒师":
                                        infoText = Language.neutralSummaryTexts[5];
                                        break;
                                    case "puppeteer":
                                    case "titiritero":
                                    case "操り人形師":
                                    case "傀儡师":
                                        infoText = Language.neutralSummaryTexts[6];
                                        break;
                                    case "exiler":
                                    case "exiliado":
                                    case "亡命者":
                                    case "处刑者":
                                        infoText = Language.neutralSummaryTexts[7];
                                        break;
                                    case "amnesiac":
                                    case "amnesico":
                                    case "健忘症":
                                    case "失忆者":
                                        infoText = Language.neutralSummaryTexts[8];
                                        break;
                                    case "seeker":
                                    case "buscador":
                                    case "シーカー":
                                    case "捉人鬼":
                                        infoText = Language.neutralSummaryTexts[9];
                                        break;

                                    // Crewmate roles
                                    case "captain":
                                    case "capitan":
                                    case "キャプテン":
                                    case "船长":
                                        infoText = Language.crewSummaryTexts[0];
                                        break;
                                    case "mechanic":
                                    case "mecanico":
                                    case "メカニック":
                                    case "修理工":
                                        infoText = Language.crewSummaryTexts[1];
                                        break;
                                    case "sheriff":
                                    case "シェリフ":
                                    case "警长":
                                        infoText = Language.crewSummaryTexts[2];
                                        break;
                                    case "detective":
                                    case "隠密":
                                    case "侦探":
                                        infoText = Language.crewSummaryTexts[3];
                                        break;
                                    case "forensic":
                                    case "forense":
                                    case "フォレンジック":
                                    case "法医":
                                        infoText = Language.crewSummaryTexts[4];
                                        break;
                                    case "timetraveler":
                                    case "viajerotemporal":
                                    case "タイムトラベラー":
                                    case "时间之主":
                                        infoText = Language.crewSummaryTexts[5];
                                        break;
                                    case "squire":
                                    case "defensor":
                                    case "スクワイア":
                                    case "卫兵":
                                        infoText = Language.crewSummaryTexts[6];
                                        break;
                                    case "cheater":
                                    case "tramposo":
                                    case "詐欺師":
                                    case "换票师":
                                        infoText = Language.crewSummaryTexts[7];
                                        break;
                                    case "fortuneteller":
                                    case "adivino":
                                    case "占い師":
                                    case "预言家":
                                        infoText = Language.crewSummaryTexts[8];
                                        break;
                                    case "hacker":
                                    case "ハッカー":
                                    case "黑客":
                                        infoText = Language.crewSummaryTexts[9];
                                        break;
                                    case "sleuth":
                                    case "sabueso":
                                    case "探偵":
                                    case "追踪者":
                                        infoText = Language.crewSummaryTexts[10];
                                        break;
                                    case "fink":
                                    case "soplon":
                                    case "フィンク":
                                    case "告密者":
                                        infoText = Language.crewSummaryTexts[11];
                                        break;
                                    case "kid":
                                    case "niño":
                                    case "子供":
                                    case "小孩":
                                        infoText = Language.crewSummaryTexts[12];
                                        break;
                                    case "welder":
                                    case "soldador":
                                    case "溶接機":
                                    case "焊工":
                                        infoText = Language.crewSummaryTexts[13];
                                        break;
                                    case "spiritualist":
                                    case "espiritista":
                                    case "スピリチュアリスト":
                                    case "殉道者":
                                        infoText = Language.crewSummaryTexts[14];
                                        break;
                                    case "vigilant":
                                    case "vigilante":
                                    case "警戒":
                                    case "哨兵":
                                        infoText = Language.crewSummaryTexts[15];
                                        break;
                                    case "hunter":
                                    case "cazador":
                                    case "猟師":
                                    case "猎人":
                                        infoText = Language.crewSummaryTexts[16];
                                        break;
                                    case "jinx":
                                    case "gafe":
                                    case "ジンクス":
                                    case "扫把星":
                                        infoText = Language.crewSummaryTexts[17];
                                        break;
                                    case "coward":
                                    case "cobarde":
                                    case "腰抜け":
                                    case "怯懦者":
                                        infoText = Language.crewSummaryTexts[18];
                                        break;
                                    case "bat":
                                    case "murcielago":
                                    case "コウモリ":
                                    case "蝙蝠侠":
                                        infoText = Language.crewSummaryTexts[19];
                                        break;
                                    case "necromancer":
                                    case "nigromante":
                                    case "ネクロマンサー":
                                    case "死灵法师":
                                        infoText = Language.crewSummaryTexts[20];
                                        break;
                                    case "engineer":
                                    case "ingeniero":
                                    case "エンジニア":
                                    case "机关师":
                                        infoText = Language.crewSummaryTexts[21];
                                        break;
                                    case "shy":
                                    case "timido":
                                    case "シャイ":
                                    case "内敛者":
                                        infoText = Language.crewSummaryTexts[22];
                                        break;
                                    case "taskmaster":
                                    case "maestrodetareas":
                                    case "タスクマスター":
                                    case "工作达人":
                                        infoText = Language.crewSummaryTexts[23];
                                        break;
                                    case "jailer":
                                    case "carcelero":
                                    case "看守":
                                    case "狱警":
                                        infoText = Language.crewSummaryTexts[24];
                                        break;
                                    case "crewmate":
                                    case "tripulante":
                                    case "乗組員":
                                    case "船员":
                                        infoText = Language.crewSummaryTexts[25];
                                        break;
                                }
                            }
                            handled = true;
                            __instance.AddChat(PlayerInCache.LocalPlayer.PlayerControl, infoText);
                            //CachedPlayer.LocalPlayer.PlayerControl.RpcSendChat(infoText);
                        }

                        RoleInfo roleInfoModifier = infos.Where(info => info.isModifier).FirstOrDefault();
                        if (text.ToLower().StartsWith("/mymodifier") || text.ToLower().StartsWith("/mymod")) {
                            if (roleInfoModifier == null) {
                                infoText = Language.modifierSummaryTexts[0];
                            }
                            else {
                                switch (roleInfoModifier.roleId.ToString().ToLower()) {
                                    // Modifiers:
                                    case "lover":
                                    case "amante":
                                    case "愛人":
                                    case "恋人":
                                        infoText = Language.modifierSummaryTexts[1];
                                        break;
                                    case "lighter":
                                    case "iluminador":
                                    case "ライター":
                                    case "火炬":
                                        infoText = Language.modifierSummaryTexts[2];
                                        break;
                                    case "blind":
                                    case "ciego":
                                    case "盲目":
                                    case "失明者":
                                        infoText = Language.modifierSummaryTexts[3];
                                        break;
                                    case "flash":
                                    case "閃光":
                                    case "闪电侠":
                                        infoText = Language.modifierSummaryTexts[4];
                                        break;
                                    case "bigchungus":
                                    case "ビッグチャンガス":
                                    case "巨人":
                                        infoText = Language.modifierSummaryTexts[5];
                                        break;
                                    case "thechosenone":
                                    case "elelegido":
                                    case "選ばれし者":
                                    case "诱饵":
                                        infoText = Language.modifierSummaryTexts[6];
                                        break;
                                    case "performer":
                                    case "teatrero":
                                    case "パフォーマー":
                                    case "广播员":
                                        infoText = Language.modifierSummaryTexts[7];
                                        break;
                                    case "pro":
                                    case "プロ":
                                    case "醉鬼":
                                        infoText = Language.modifierSummaryTexts[8];
                                        break;
                                    case "paintball":
                                    case "ペイントボール":
                                    case "溅血者":
                                        infoText = Language.modifierSummaryTexts[9];
                                        break;
                                    case "electrician":
                                    case "electricista":
                                    case "電気技師":
                                    case "电工":
                                        infoText = Language.modifierSummaryTexts[10];
                                        break;
                                }
                            }
                            handled = true;
                            __instance.AddChat(PlayerInCache.LocalPlayer.PlayerControl, infoText);
                            //CachedPlayer.LocalPlayer.PlayerControl.RpcSendChat(infoText);
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