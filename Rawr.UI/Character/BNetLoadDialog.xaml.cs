using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class BNetLoadDialog : ChildWindow
    {
        private static Dictionary<string, List<string>> ServerNames;

        static BNetLoadDialog()
        {
            #region Server Names
            ServerNames = new Dictionary<string, List<string>>();
            #region United States Servers
            ServerNames["US"] = new List<string>() {
                "Aegwynn",
                "Aerie Peak",
                "Agamaggan",
                "Aggramar",
                "Akama",
                "Alexstrasza",
                "Alleria",
                "Altar of Storms",
                "Alterac Mountains",
                "Aman'Thul",
                "Andorhal",
                "Anetheron",
                "Antonidas",
                "Anub'arak",
                "Anvilmar",
                "Arathor",
                "Archimonde",
                "Area 52",
                "Argent Dawn",
                "Arthas",
                "Arygos",
                "Auchindoun",
                "Azgalor",
                "Azjol-Nerub",
                "Azshara",
                "Azuremyst",
                "Baelgun",
                "Balnazzar",
                "Barthilas",
                "Black Dragonflight",
                "Blackhand",
                "Blackrock",
                "Blackwater Raiders",
                "Blackwing Lair",
                "Bladefist",
                "Blade's Edge",
                "Bleeding Hollow",
                "Blood Furnace",
                "Bloodhoof",
                "Bloodscalp",
                "Bonechewer",
                "Borean Tundra",
                "Boulderfist",
                "Bronzebeard",
                "Burning Blade",
                "Burning Legion",
                "Caelestrasz",
                "Cairne",
                "Cenarion Circle",
                "Cenarius",
                "Cho'gall",
                "Chromaggus",
                "Coilfang",
                "Crushridge",
                "Daggerspine",
                "Dalaran",
                "Dalvengyr",
                "Dark Iron",
                "Darkspear",
                "Darrowmere",
                "Dath'Remar",
                "Dawnbringer",
                "Deathwing",
                "Demon Soul",
                "Dentarg",
                "Destromath",
                "Dethecus",
                "Detheroc",
                "Doomhammer",
                "Draenor",
                "Dragonblight",
                "Dragonmaw",
                "Draka",
                "Drakkari",
                "Drak'tharon",
                "Drak'thul",
                "Dreadmaul",
                "Drenden",
                "Dunemaul",
                "Durotan",
                "Duskwood",
                "Earthen Ring",
                "Echo Isles",
                "Eitrigg",
                "Eldre'Thalas",
                "Elune",
                "Emerald Dream",
                "Eonar",
                "Eredar",
                "Executus",
                "Exodar",
                "Farstriders",
                "Feathermoon",
                "Fenris",
                "Firetree",
                "Fizzcrank",
                "Frostmane",
                "Frostmourne",
                "Frostwolf",
                "Galakrond",
                "Garithos",
                "Garona",
                "Garrosh",
                "Ghostlands",
                "Gilneas",
                "Gnomeregan",
                "Gorefiend",
                "Gorgonnash",
                "Greymane",
                "Grizzly Hills",
                "Gul'dan",
                "Gundrak",
                "Gurubashi",
                "Hakkar",
                "Haomarush",
                "Hellscream",
                "Hydraxis",
                "Hyjal",
                "Icecrown",
                "Illidan",
                "Jaedenar",
                "Jubei'Thos",
                "Kael'thas",
                "Kalecgos",
                "Kargath",
                "Kel'Thuzad",
                "Khadgar",
                "Khaz Modan",
                "Khaz'goroth",
                "Kil'Jaeden",
                "Kilrogg",
                "Kirin Tor",
                "Korgath",
                "Korialstrasz",
                "Kul Tiras",
                "Laughing Skull",
                "Lethon",
                "Lightbringer",
                "Lightninghoof",
                "Lightning's Blade",
                "Llane",
                "Lothar",
                "Madoran",
                "Maelstrom",
                "Magtheridon",
                "Maiev",
                "Malfurion",
                "Mal'Ganis",
                "Malorne",
                "Malygos",
                "Mannoroth",
                "Medivh",
                "Misha",
                "Mok'Nathal",
                "Moon Guard",
                "Moonrunner",
                "Mug'thol",
                "Muradin",
                "Nagrand",
                "Nathrezim",
                "Nazgrel",
                "Nazjatar",
                "Ner'zhul",
                "Nesingwary",
                "Nordrassil",
                "Norgannon",
                "Onyxia",
                "Perenolde",
                "Proudmoore",
                "Quel'Dorei",
                "Quel'Thalas",
                "Ragnaros",
                "Ravencrest",
                "Ravenholdt",
                "Rexxar",
                "Rivendare",
                "Runetotem",
                "Sargeras",
                "Saurfang",
                "Scarlet Crusade",
                "Scilla",
                "Sen'Jin",
                "Sentinels",
                "Shadow Council",
                "Shadowmoon",
                "Shadowsong",
                "Shandris",
                "Shattered Halls",
                "Shattered Hand",
                "Shu'Halo",
                "Silver Hand",
                "Silvermoon",
                "Sisters of Elune",
                "Skullcrusher",
                "Skywall",
                "Smolderthorn",
                "Spinebreaker",
                "Spirestone",
                "Staghelm",
                "Steamwheedle Cartel",
                "Stonemaul",
                "Stormrage",
                "Stormreaver",
                "Stormscale",
                "Suramar",
                "Tanaris",
                "Terenas",
                "Terokkar",
                "Thaurissan",
                "The Forgotten Coast",
                "The Scryers",
                "The Underbog",
                "The Venture Co",
                "Thorium Brotherhood",
                "Thrall",
                "Thunderhorn",
                "Thunderlord",
                "Tichondrius",
                "Tortheldrin",
                "Trollbane",
                "Turalyon",
                "Twisting Nether",
                "Uldaman",
                "Uldum",
                "Undermine",
                "Ursin",
                "Uther",
                "Vashj",
                "Vek'nilash",
                "Velen",
                "Warsong",
                "Whisperwind",
                "Wildhammer",
                "Windrunner",
                "Winterhoof",
                "Wyrmrest Accord",
                "Ysera",
                "Ysondre",
                "Zangarmarsh",
                "Zul'jin",
                "Zuluhed",
            };
            ServerNames["US"].Sort();
            #endregion
            #region European Servers
            ServerNames["EU"] = new List<string>() {
                #region English Servers
                "Aerie Peak",
                "Agamaggan",
                "Aggramar",
                "Ahn'Qiraj",
                "Al'Akir",
                "Alonsus",
                "Anachronos",
                "Arathor",
                "Argent Dawn",
                "Aszune",
                "Auchindoun",
                "Azjol-Nerub",
                "Azuremyst",
                "Balnazzar",
                "Blade's Edge",
                "Bladefist",
                "Bloodhoof",
                "Bloodscalp",
                "Boulderfist",
                "Bloodfeather",
                "Bronze Dragonflight",
                "Bronzebeard",
                "Burning Blade",
                "Burning Legion",
                "Burning Steppes",
                "Chamber of Aspects",
                "Chromaggus",
                "Crushridge",
                "Daggerspine",
                "Darkmoon Faire",
                "Darksorrow",
                "Darkspear",
                "Deathwing",
                "Defias Brotherhood",
                "Dentarg",
                "Doomhammer",
                "Draenor",
                "Dragonblight",
                "Dragonmaw",
                "Drak'thul",
                "Dunemaul",
                "Earthen Ring",
                "Emerald Dream",
                "Emeriss",
                "Eonar",
                "Executus",
                "Frostmane",
                "Frostwhisper",
                "Genjuros",
                "Ghostlands",
                "Grim Batol",
                "Hakkar",
                "Haomarush",
                "Hellfire",
                "Hellscream",
                "Jaedenar",
                "Karazhan",
                "Kazzak",
                "Khadgar",
                "Kilrogg",
                "Kor'gall",
                "Kul Tiras",
                "Laughing Skull",
                "Lightbringer",
                "Lightning's Blade",
                "Magtheridon",
                "Mazrigos",
                "Moonglade",
                "Nagrand",
                "Neptulon",
                "Nordrassil",
                "Outland",
                "Quel'Thalas",
                "Ragnaros",
                "Ravencrest",
                "Ravenholdt",
                "Runetotem",
                "Saurfang",
                "Scarshield Legion",
                "Shadowsong",
                "Shattered Halls",
                "Shattered Hand",
                "Silvermoon",
                "Skullcrusher",
                "Spinebreaker",
                "Sporeggar",
                "Steamwheedle Cartel",
                "Stormrage",
                "Stormreaver",
                "Stormscale",
                "Sunstrider",
                "Sylvanas",
                "Talnivarr",
                "Tarren Mill",
                "Terenas",
                "Terokkar",
                "The Maelstrom",
                "The Sha'tar",
                "The Venture Co",
                "Thunderhorn",
                "Trollbane",
                "Turalyon",
                "Twilight's Hammer",
                "Twisting Nether",
                "Vashj",
                "Vek'nilash",
                "Wildhammer",
                "Xavius",
                "Zenedar",
                #endregion
                #region French Servers
                "Arak-arahm",
                "Arathi",
                "Archimonde",
                "Chants éternels",
                "Cho'gall",
                "Confrérie du Thorium",
                "Conseil des Ombres",
                "Culte de la Rive Noire",
                "Dalaran",
                "Drek'Thar",
                "Eitrigg",
                "Eldre'Thalas",
                "Elune",
                "Garona",
                "Hyjal",
                "Illidan",
                "Kael'Thas",
                "Khaz Modan",
                "Kirin Tor",
                "Krasus",
                "La Croisade écarlate",
                "Les Clairvoyants",
                "Les Sentinelles",
                "Marécage de Zangar",
                "Medivh",
                "Naxxramas",
                "Ner'zhul",
                "Rashgarroth",
                "Sargeras",
                "Sinstralis",
                "Suramar",
                "Temple noir",
                "Throk'Feroth",
                "Uldaman",
                "Varimathras",
                "Vol'jin",
                "Ysondre",
                #endregion
                #region German Servers
                "Aegwynn",
                "Alexstrasza",
                "Alleria",
                "Aman'Thul",
                "Ambossar",
                "Anetheron",
                "Antonidas",
                "Anub'arak",
                "Area 52",
                "Arthas",
                "Arygos",
                "Azshara",
                "Baelgun",
                "Blackhand",
                "Blackmoore",
                "Blackrock",
                "Blutkessel",
                "Dalvengyr",
                "Das Konsortium",
                "Das Syndikat",
                "Der abyssische Rat",
                "Der Mithrilorden",
                "Der Rat von Dalaran",
                "Destromath",
                "Dethecus",
                "Die Aldor",
                "Die Arguswacht",
                "Die ewige Wacht",
                "Die Nachtwache",
                "Die Silberne Hand",
                "Die Todeskrallen",
                "Dun Morogh",
                "Durotan",
                "Echsenkessel",
                "Eredar",
                "Festung der Stürme",
                "Forscherliga",
                "Frostmourne",
                "Frostwolf",
                "Garrosh",
                "Gilneas",
                "Gorgonnash",
                "Gul'dan",
                "Kargath",
                "Kel'Thuzad",
                "Khaz'goroth",
                "Kil'Jaeden",
                "Krag'jin",
                "Kult der Verdammten",
                "Lordaeron",
                "Lothar",
                "Madmortem",
                "Mal'Ganis",
                "Malfurion",
                "Malorne",
                "Malygos",
                "Mannoroth",
                "Mug'thol",
                "Nathrezim",
                "Nazjatar",
                "Nefarian",
                "Nera'thor",
                "Nethersturm",
                "Norgannon",
                "Nozdormu",
                "Onyxia",
                "Perenolde",
                "Proudmoore",
                "Rajaxx",
                "Rexxar",
                "Sen'jin",
                "Shattrath",
                "Taerar",
                "Teldrassil",
                "Terrordar",
                "Theradras",
                "Thrall",
                "Tichondrius",
                "Tirion",
                "Todeswache",
                "Ulduar",
                "Un'Goro",
                "Vek'lor",
                "Wrathbringer",
                "Ysera",
                "Zirkel des Cenarius",
                "Zuluhed",
                #endregion
                #region Russian Servers
                "Азурегос",
                "Борейская тундра",
                "Вечная Песня",
                "Галакронд",
                "Гордунни",
                "Голдринн",
                "Гром",
                "Дракономор",
                "Король-лич",
                "Пиратская бухта",
                "Подземье",
                "Разувий",
                "Ревущий фьорд",
                "Свежеватель Душ",
                "Седогрив",
                "Страж Смерти",
                "Термоштепсель",
                "Ткач Смерти",
                "Черный Шрам",
                "Ясеневый лес",
                #endregion
                #region Spanish Servers
                "C'Thun",
                "Colinas Pardas",
                "Dun Modr",
                "Exodar",
                "Los Errantes",
                "Minahonda",
                "Sanguino",
                "Shen'dralar",
                "Tyrande",
                "Uldum",
                "Zul'jin",
                #endregion
                // Following are possible removals
                // Unable to locate server names at http://eu.battle.net/wow/en/status
                "Molten Core",
                "Shadowmoon",
                "Stonemaul",
                "Warsong",
            };
            ServerNames["EU"].Sort();
            #endregion
            #region Korean Servers
            ServerNames["KR"] = new List<string>()
            {
                // http://kr.battle.net/wow/ko/status
                "가로나",
                "굴단",
                "노르간논",
                "달라란",
                "데스윙",
                "듀로탄",
                "라그나로스",
                "레인",
                "렉사르",
                "말리고스",
                "말퓨리온",
                "메디브",
                "불타는 군단",
                "블랙무어",
                "살타리온",
                "세나리우스",
                "스톰레이지",
                "아즈샤라",
                "알레리아",
                "알렉스트라자",
                "에이그윈",
                "엘룬",
                "와일드해머",
                "우서",
                "윈드러너",
                "이오나",
                "줄진",
                "카라잔",
                "카르가스",
                "쿨 티라스",
                "티리온",
                "하이잘",
                "헬스크림",
                // Possible removals, unable to locate following in link above
                "아키몬드",
                "아서스",
                "아즈갈로",
                "검은용군단",
                "청동용군단",
                "진홍십자군",
                "일리단",
                "켈투자드",
                "킬제덴",
                "마그테리돈",
                "말가니스",
                "만노로스",
                "나스레짐",
                "넬'쥴",
                "붉은용군단",
                "살게라스",
                "티콘드리우스",
                "에그윈",
                "아스준",
                "둠해머",
                "드레노어",
                "캘타스",
                "킬로그",
                "레엔",
                "실버문",
                "실바나스",
            };
            ServerNames["KR"].Sort();
            #endregion
            #region Tiawan Servers
            ServerNames["TW"] = new List<string>()
            {
                // http://tw.battle.net/wow/zh/status
                "世界之樹",
                "亞雷戈斯",
                "全球爭霸戰2",
                "冬握湖",
                "冰霜之刺",
                "冰風崗哨",
                "凜風峽灣",
                "地獄吼",
                "夜空之歌",
                "天空之牆",
                "奎爾達納斯",
                "奧妮克希亞",
                "寒冰皇冠",
                "尖石",
                "屠魔山谷",
                "巨龍之喉",
                "巴納札爾",
                "惡魔之魂",
                "憤怒使者",
                "戰歌",
                "提克迪奧斯",
                "撒爾薩里安",
                "日落沼澤",
                "暗影之月",
                "暴風祭壇",
                "死亡之翼",
                "水晶之刺",
                "火焰之樹",
                "狂心",
                "狂熱之刃",
                "狼狂索斯",
                "眾星之子",
                "米奈希爾",
                "聖光之願",
                "艾奧那斯",
                "血之谷",
                "語風",
                "諾姆瑞根",
                "遠祖灘頭",
                "銀翼要塞",
                "阿薩斯",
                "雷鱗",
                "風暴群山",
                "鬼霧峰",
                "黑龍軍團"

                //"克爾蘇加德",
                //"奈辛瓦里",
            };
            ServerNames["TW"].Sort();
            #endregion
            #region Chinese Servers
            ServerNames["CN"] = new List<string>()
            {
                // http://www.battlenet.com.cn/wow/zh/status
                "万色星辰",
                "世界之树",
                "丹莫德",
                "主宰之剑",
                "亚雷戈斯",
                "亡语者",
                "伊兰尼库斯",
                "伊利丹",
                "伊森利恩",
                "伊森德雷",
                "伊瑟拉",
                "伊莫塔尔",
                "伊萨里奥斯",
                "元素之力",
                "克尔苏加德",
                "克洛玛古斯",
                "克苏恩",
                "军团要塞",
                "冬拥湖",
                "冬泉谷",
                "冰川之拳",
                "冰霜之刃",
                "冰风岗",
                "凤凰之神",
                "凯尔萨斯",
                "凯恩血蹄",
                "刀塔",
                "利刃之拳",
                "刺骨利刃",
                "加兹鲁维",
                "加基森",
                "加尔",
                "加德纳尔",
                "加里索斯",
                "勇士岛",
                "千针石林",
                "午夜之镰",
                "卡德加",
                "卡德罗斯",
                "卡扎克",
                "卡拉赞",
                "卡珊德拉",
                "古加尔",
                "古尔丹",
                "古拉巴什",
                "古达克",
                "哈兰",
                "哈卡",
                "噬灵沼泽",
                "嚎风峡湾",
                "回音山",
                "国王之谷",
                "图拉扬",
                "圣火神殿",
                "地狱之石",
                "地狱咆哮",
                "埃克索图斯",
                "埃加洛尔",
                "埃德萨拉",
                "埃苏雷格",
                "埃雷达尔",
                "基尔加丹",
                "基尔罗格",
                "塔纳利斯",
                "塞拉摩",
                "塞拉赞恩",
                "塞泰克",
                "塞纳里奥",
                "壁炉谷",
                "夏维安",
                "外域",
                "大地之怒",
                "大漩涡",
                "天空之墙",
                "太阳之井",
                "夺灵者",
                "奈法利安",
                "奎尔丹纳斯",
                "奎尔萨拉斯",
                "奥妮克希亚",
                "奥尔加隆",
                "奥拉基尔",
                "奥斯里安",
                "奥特兰克",
                "奥蕾莉亚",
                "奥达曼",
                "守护之剑",
                "安其拉",
                "安加萨",
                "安多哈尔",
                "安威玛尔",
                "安戈洛",
                "安格博达",
                "安纳塞隆",
                "安苏",
                "密林游侠",
                "寒冰皇冠",
                "尘风峡谷",
                "屠魔山谷",
                "山丘之王",
                "岩石巨塔",
                "巨龙之吼",
                "巫妖之王",
                "巴尔古恩",
                "巴瑟拉斯",
                "巴纳扎尔",
                "布兰卡德",
                "布莱克摩",
                "布莱恩",
                "布鲁塔卢斯",
                "希尔瓦娜斯",
                "希雷诺斯",
                "幽暗沼泽",
                "库尔提拉斯",
                "库德兰",
                "弗塞雷迦",
                "影之哀伤",
                "影牙要塞",
                "德拉诺",
                "恐怖图腾",
                "恶魔之翼",
                "恶魔之魂",
                "戈古纳斯",
                "戈提克",
                "戈杜尼",
                "战歌",
                "扎拉赞恩",
                "托塞德林",
                "托尔巴拉德",
                "拉文凯斯",
                "拉文霍德",
                "拉格纳洛斯",
                "拉贾克斯",
                "提尔之手",
                "提瑞斯法",
                "摩摩尔",
                "斩魔者",
                "斯坦索姆",
                "无尽之海",
                "无底海渊",
                "日落沼泽",
                "普瑞斯托",
                "普罗德摩",
                "暗影之月",
                "暗影议会",
                "暗影迷宫",
                "暮色森林",
                "暴风祭坛",
                "月光林地",
                "月神殿",
                "末日行者",
                "朵丹尼尔",
                "杜隆坦",
                "格瑞姆巴托",
                "格雷迈恩",
                "格鲁尔",
                "桑德兰",
                "梅尔加尼",
                "梦境之树",
                "森金",
                "死亡之翼",
                "死亡之门",
                "死亡熔炉",
                "毁灭之锤",
                "水晶之刺",
                "永夜港",
                "永恒之井",
                "法拉希姆",
                "泰兰德",
                "泰拉尔",
                "洛丹伦",
                "洛肯",
                "洛萨",
                "海克泰尔",
                "海加尔",
                "海达希亚",
                "浸毒之骨",
                "深渊之喉",
                "深渊之巢",
                "激流之傲",
                "激流堡",
                "火喉",
                "火烟之谷",
                "火焰之树",
                "火羽山",
                "灰烬使者",
                "灰谷",
                "烈焰峰",
                "烈焰荆棘",
                "熊猫酒仙",
                "熔火之心",
                "熵魔",
                "燃烧之刃",
                "燃烧军团",
                "燃烧平原",
                "爱斯特纳",
                "狂热之刃",
                "狂风峭壁",
                "玛克扎尔",
                "玛多兰",
                "玛格曼达",
                "玛格索尔",
                "玛法里奥",
                "玛洛加尔",
                "玛瑟里顿",
                "玛诺洛斯",
                "玛里苟斯",
                "瑞文戴尔",
                "瑟玛普拉格",
                "瑟莱德丝",
                "瓦丝琪",
                "瓦拉斯塔兹",
                "瓦里玛萨斯",
                "甜水绿洲",
                "生态船",
                "白银之手",
                "白骨荒野",
                "盖斯",
                "石爪峰",
                "石锤",
                "破碎大厅",
                "破碎岭",
                "祖尔金",
                "祖阿曼",
                "神圣之歌",
                "禁魔监狱",
                "穆戈尔",
                "符文图腾",
                "米奈希尔",
                "索拉丁",
                "索瑞森",
                "红云台地",
                "红龙军团",
                "红龙女王",
                "纳克萨玛斯",
                "纳沙塔尔",
                "织亡者",
                "维克尼拉斯",
                "罗宁",
                "羽月",
                "翡翠梦境",
                "耐克鲁斯",
                "耐奥祖",
                "耐普图隆",
                "耐萨里奥",
                "耳语海岸",
                "能源舰",
                "自由之风",
                "艾森娜",
                "艾欧纳尔",
                "艾维娜",
                "艾苏恩",
                "艾莫莉丝",
                "艾萨拉",
                "艾露恩",
                "芬里斯",
                "苏塔恩",
                "范克里夫",
                "范达尔鹿盔",
                "荆棘谷",
                "莉亚德琳",
                "莫什奥格",
                "莫加尔",
                "莫德雷萨",
                "莫格莱尼",
                "莱索恩",
                "菲拉斯",
                "菲米丝",
                "萨塔里奥",
                "萨尔",
                "萨格拉斯",
                "萨菲隆",
                "萨贝里安",
                "蒸汽地窟",
                "蓝龙军团",
                "藏宝海湾",
                "蜘蛛王国",
                "血吼",
                "血槌",
                "血牙魔王",
                "血环",
                "血羽",
                "血色十字军",
                "血顶",
                "试炼之环",
                "诺兹多姆",
                "诺森德",
                "诺莫瑞根",
                "贫瘠之地",
                "踏梦者",
                "轻风之语",
                "达克萨隆",
                "达基萨斯",
                "达尔坎",
                "达文格尔",
                "达斯雷玛",
                "达纳斯",
                "达隆米尔",
                "迅捷微风",
                "远古海滩",
                "迦拉克隆",
                "迦玛兰",
                "迦罗娜",
                "迦顿",
                "迪托马斯",
                "迪瑟洛克",
                "迪门修斯",
                "逐日者",
                "通灵学院",
                "遗忘海岸",
                "邪恶颅壳",
                "金度",
                "金色平原",
                "铜龙军团",
                "银月",
                "银松森林",
                "闪电之刃",
                "阿克蒙德",
                "阿努巴拉克",
                "阿卡玛",
                "阿古斯",
                "阿尔萨斯",
                "阿扎达斯",
                "阿拉希",
                "阿拉索",
                "阿斯塔洛",
                "阿曼尼",
                "阿格拉玛",
                "阿比迪斯",
                "阿纳克洛斯",
                "阿迦玛甘",
                "雏龙之翼",
                "雷克萨",
                "雷德",
                "雷斧堡垒",
                "雷霆之怒",
                "雷霆之王",
                "雷霆号角",
                "霍格",
                "霜之哀伤",
                "霜狼",
                "风暴之怒",
                "风暴之眼",
                "风暴之鳞",
                "风暴峭壁",
                "风暴裂隙",
                "风行者",
                "鬼雾峰",
                "鲜血熔炉",
                "鹰巢山",
                "麦姆",
                "麦维影歌",
                "麦迪文",
                "黄金之路",
                "黑手军团",
                "黑暗之矛",
                "黑暗之门",
                "黑暗虚空",
                "黑暗魅影",
                "黑石尖塔",
                "黑翼之巢",
                "黑铁",
                "黑锋哨站",
                "黑龙军团",
                "龙骨平原",
            };
            ServerNames["CN"].Sort();
            #endregion
            #endregion
        }

        public Character Character { get; private set; }
        private Rawr.Rawr4ArmoryService _armoryService = new Rawr4ArmoryService();

        private bool isReload = false;
        public void ShowReload()
        {
            isReload = true;
            Character = null;
            this.Show();
            OKButton_Click(null, null);
        }

        public BNetLoadDialog()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
#endif

            RegionCombo.ItemsSource = new List<string>() { "US", "EU", "KR", "TW", "CN" };

            _armoryService.ProgressChanged += new EventHandler<EventArgs<string>>(_armoryService_ProgressChanged);
            _armoryService.GetCharacterCompleted += new EventHandler<EventArgs<Character>>(_armoryService_GetCharacterCompleted);
            _armoryService.GetCharacterErrored += new EventHandler<EventArgs<String>>(_armoryService_GetCharacterErrored);

            if (Rawr.Properties.RecentSettings.Default.RecentRegion == null)
            {
                Rawr.Properties.RecentSettings.Default.RecentRegion = "US";
            }
            RegionCombo.SelectedItem = Rawr.Properties.RecentSettings.Default.RecentRegion;

            if (Rawr.Properties.RecentSettings.Default.RecentChars != null) {
                int count = Rawr.Properties.RecentSettings.Default.RecentChars.Count;
                if (count > 0) {
                    string[] autocomplete = new string[count];
                    Rawr.Properties.RecentSettings.Default.RecentChars.CopyTo(autocomplete, 0);
                    NameText.IsTextCompletionEnabled = true;
                    NameText.ItemsSource = autocomplete;
                    NameText.Text = Rawr.Properties.RecentSettings.Default.RecentChars[count - 1];
                }
            } else { Rawr.Properties.RecentSettings.Default.RecentChars = new List<string>() { }; }
            if (Rawr.Properties.RecentSettings.Default.RecentServers == null)
            {
                Rawr.Properties.RecentSettings.Default.RecentServers = new List<string>() { }; 
            }
            List<string> autocompletelist = new List<string>(ServerNames[(string)RegionCombo.SelectedItem]);
            bool dirty = false;
            foreach (var server in Rawr.Properties.RecentSettings.Default.RecentServers)
            {
                if (!autocompletelist.Contains(server))
                {
                    autocompletelist.Add(server);
                    dirty = true;
                }
            }
            if (dirty)
            {
                autocompletelist.Sort();
            }
            RealmText.IsTextCompletionEnabled = true;
            RealmText.ItemsSource = autocompletelist;
            int c = Rawr.Properties.RecentSettings.Default.RecentServers.Count;
            if (c > 0)
            {
                RealmText.Text = Rawr.Properties.RecentSettings.Default.RecentServers[c - 1];
            }
            RealmText_TextChanged(null, null);
            BT_CancelProcessing.IsEnabled = false;
        }

        private bool OKButtonIsEnabled {
            get
            {
                bool ok = true;
                // Validate both are not Empty
                if (String.IsNullOrEmpty(RealmText.Text)) { ok = false; }
                else if (String.IsNullOrEmpty(NameText.Text)) { ok = false; }
                // Validate Server is in the Server Name List, allow for ' to be missing
                else
                {
                    ok = false;
                    foreach (String sn in ServerNames[RegionCombo.SelectedItem as String])
                    {
                        if (sn.Replace("\'", "").ToLower().Contains(RealmText.Text.Replace("\'", "").ToLower())) { ok = true; }
                    }
                }
                //
                return ok;
            }
        }

        private void RegionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RegionCombo == null) return; // it's null when still loading xaml
            List<string> autocompletelist = new List<string>(ServerNames[(string)RegionCombo.SelectedItem]);
            bool dirty = false;
            foreach (var server in Rawr.Properties.RecentSettings.Default.RecentServers)
            {
                if (!autocompletelist.Contains(server))
                {
                    autocompletelist.Add(server);
                    dirty = true;
                }
            }
            if (dirty)
            {
                autocompletelist.Sort();
            }
            RealmText.IsTextCompletionEnabled = true;
            RealmText.ItemsSource = autocompletelist;
        }

        private void RealmText_TextChanged(object sender, RoutedEventArgs e)
        {
            BT_OK.IsEnabled = OKButtonIsEnabled;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            _armoryService.GetCharacterAsync((CharacterRegion)Enum.Parse(typeof(CharacterRegion),
                (string)RegionCombo.SelectedItem, false), 
                RealmText.Text, NameText.Text, ForceRefreshCheckBox.IsChecked.Value);

            ProgressBarStatus.IsIndeterminate = true;
            BT_OK.IsEnabled = RegionCombo.IsEnabled = RealmText.IsEnabled = NameText.IsEnabled = false;
            BT_CancelProcessing.IsEnabled = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            if (BT_CancelProcessing.IsEnabled) {
                BT_OK.IsEnabled = RegionCombo.IsEnabled = RealmText.IsEnabled = NameText.IsEnabled = true;
                BT_CancelProcessing.IsEnabled = false;
                ProgressBarStatus.IsIndeterminate = false;
                TextBlockStatus.Text = string.Empty;
            }
            this.DialogResult = false;
        }

        private void BT_CancelProcessing_Click(object sender, RoutedEventArgs e)
        {
            _armoryService.CancelAsync();
            BT_OK.IsEnabled = RegionCombo.IsEnabled = RealmText.IsEnabled = NameText.IsEnabled = true;
            BT_CancelProcessing.IsEnabled = false;
            ProgressBarStatus.IsIndeterminate = false;
            TextBlockStatus.Text = string.Empty;
        }

#if !SILVERLIGHT
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
#endif

        void _armoryService_ProgressChanged(object sender, EventArgs<string> e)
        {
            string[] progress = e.Value.Split('|');
            TextBlockStatus.Text = progress[0];
            if (progress.Length > 1)
            {
                ToolTipStatus.Visibility = Visibility.Visible;
                ToolTipStatus.Content = progress[1];
            }
            else
                ToolTipStatus.Visibility = Visibility.Collapsed;
        }

        void _armoryService_GetCharacterCompleted(object sender, EventArgs<Character> e)
        {
            ProgressBarStatus.IsIndeterminate = true;
            ProgressBarStatus.Value = ProgressBarStatus.Maximum;
            Character = e.Value;
            if (!isReload) { this.DialogResult = true; }
            else 
            {
                BT_CancelProcessing.IsEnabled = false;
                BT_Cancel.Content = "Done"; 
            }
        }

        void _armoryService_GetCharacterErrored(object sender, EventArgs<String> e)
        {
            ProgressBarStatus.IsIndeterminate = true;
            ProgressBarStatus.Value = ProgressBarStatus.Maximum;
            Character = null;
            if (isReload) {
                this.DialogResult = false;
            } else {
                BT_CancelProcessing_Click(null, null);
            }
        }

        public void Load(string characterName, CharacterRegion region, string realm)
        {
            NameText.Text = characterName;
            RealmText.Text = realm;
            RegionCombo.SelectedItem = region.ToString();
            OKButton_Click(null, null);
        }

    }
}

