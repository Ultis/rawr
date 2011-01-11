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
            ServerNames["TW"] = new List<string>()
            {
                "暴風祭壇",
                "亞雷戈斯",
                "巴納札爾",
                "眾星之子",
                "聖光之願",
                "奧妮克希亞",
                "天空之牆",
                "暗影之月",
                "語風",
                "世界之樹",
                "阿薩斯",
                "黑龍軍團",
                "血之谷",
                "冰風崗哨",
                "水晶之刺",
                "死亡之翼",
                "屠魔山谷",
                "惡魔之魂",
                "巨龍之喉",
                "鬼霧峰",
                "狂心",
                "冰霜之刺",
                "諾姆瑞根",
                "地獄吼",
                "凜風峽灣",
                "寒冰皇冠",
                "克爾蘇加德",
                "米奈希爾",
                "奈辛瓦里",
                "夜空之歌",
                "撒爾薩里安",
                "銀翼要塞",
                "尖石",
                "雷鱗",
                "遠祖灘頭",
                "日落沼澤",
                "戰歌",
                "憤怒使者",
                "狂熱之刃"
            };
            ServerNames["TW"].Sort();
            ServerNames["CN"] = new List<string>()
            {
                "奥蕾莉亚",
                "回音山",
                "玛多兰",
                "莫德古得",
                "普瑞斯托",
                "白银之手",
                "图拉扬",
                "伊瑟拉",
                "阿格拉玛",
                "暴风祭坛",
                "安威玛尔",
                "艾苏恩",
                "黑龙军团",
                "黑石尖塔",
                "蓝龙军团",
                "藏宝海湾",
                "铜龙军团",
                "燃烧平原",
                "达纳斯",
                "死亡之翼",
                "迪托马斯",
                "尘风峡谷",
                "烈焰峰",
                "诺莫瑞根",
                "卡扎克",
                "卡德罗斯",
                "基尔罗格",
                "库德兰",
                "洛萨",
                "玛瑟里顿",
                "山丘之王",
                "耐萨里奥",
                "红龙军团",
                "罗宁",
                "萨格拉斯",
                "索瑞森",
                "索拉丁",
                "雷霆之王",
                "奥达曼",
                "国王之谷",
                "艾森娜",
                "塞纳里奥",
                "塞纳留斯",
                "众星之子",
                "梦境之树",
                "艾露恩",
                "月光林地",
                "夜空之歌",
                "诺达希尔",
                "神谕林地",
                "月神殿",
                "泰兰德",
                "迷雾之海",
                "轻风之语",
                "冬泉谷",
                "阿迦玛甘",
                "奥拉基尔",
                "阿克蒙德",
                "爱斯特纳",
                "埃加洛尔",
                "艾萨拉",
                "埃苏雷格",
                "达斯雷玛",
                "屠魔山谷",
                "毁灭之锤",
                "火焰之树",
                "冰霜之刃",
                "地狱咆哮",
                "海加尔",
                "伊利丹",
                "卡德加",
                "闪电之刃",
                "麦维影歌",
                "梅尔加尼",
                "玛法里奥",
                "主宰之剑",
                "耐普图隆",
                "拉文凯斯",
                "暗影之月",
                "石爪峰",
                "风暴之怒",
                "战歌",
                "风行者",
                "夏维安",
                "吉安娜",
                "米莎",
                "灵魂石地",
                "布莱克摩",
                "黑暗之矛",
                "鬼雾峰",
                "杜隆坦",
                "回音群岛",
                "埃德萨拉",
                "迦罗娜",
                "玛里苟斯",
                "红云台地",
                "雷克萨",
                "符文图腾",
                "奥丹姆",
                "布瑞尔",
                "达拉然",
                "遗忘海岸",
                "霜之哀伤",
                "圣光之愿",
                "麦迪文",
                "纳斯雷兹姆",
                "银月",
                "银松森林",
                "泰瑞纳斯",
                "乌瑟尔",
                "耳语海岸",
                "鹰巢山",
                "奥特兰克",
                "安东尼达斯",
                "阿拉索",
                "阿尔萨斯",
                "达隆米尔",
                "艾欧纳尔",
                "霜狼",
                "玛诺洛斯",
                "耐奥祖",
                "匹瑞诺德",
                "拉格纳罗斯",
                "莱斯霜语",
                "瑞文戴尔",
                "血色十字军",
                "通灵学院",
                "激流堡",
                "塔伦米尔",
                "金色平原",
                "海达希亚",
                "瓦里玛萨斯",
                "安其拉",
                "阿纳克洛斯",
                "安纳塞隆",
                "阿努巴拉克",
                "阿拉希",
                "巴纳扎尔",
                "黑手军团",
                "翼之巢",
                "血羽",
                "燃烧军团",
                "克洛玛古斯",
                "破碎岭",
                "克苏恩",
                "德拉诺",
                "龙骨平原",
                "范达尔鹿盔",
                "无尽之海",
                "格瑞姆巴托",
                "古拉巴什",
                "哈卡",
                "海克泰尔",
                "卡拉赞",
                "库尔提拉斯",
                "莱索恩",
                "洛丹伦",
                "熔火之心",
                "纳克萨玛斯",
                "奈法利安",
                "奎尔萨拉斯",
                "拉贾克斯",
                "拉文霍德",
                "萨菲隆",
                "森金",
                "泰拉尔",
                "桑德兰",
                "雷霆之怒",
                "瓦拉斯塔兹",
                "永恒之井",
            };
            ServerNames["CN"].Sort();
#endregion
        }

        public Character Character { get; private set; }
        private Rawr.Rawr4ArmoryService _armoryService = new Rawr4ArmoryService();

        public void ShowReload()
        {
            this.Show();
            OKButton_Click(null, null);
            BT_OK.IsEnabled = false;
        }

        public BNetLoadDialog()
        {
            InitializeComponent();

            RegionCombo.ItemsSource = new List<string>() { "US", "EU", "KR", "TW", "CN" };

            _armoryService.ProgressChanged += new EventHandler<EventArgs<string>>(_armoryService_ProgressChanged);
            _armoryService.GetCharacterCompleted += new EventHandler<EventArgs<Character>>(_armoryService_GetCharacterCompleted);

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
            this.DialogResult = true;
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

