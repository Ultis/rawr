using System;
using System.Collections.Generic;
#if RAWR3
using System.Windows.Media;
#else
using System.Drawing;
#endif
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Rawr.DPSWarr.Markov;
using Rawr.Base;
using Rawr.Bosses;

namespace Rawr.DPSWarr {
    public struct DPSWarrCharacter
    {
        public Character Char;
        public Rotation Rot;
        public CombatFactors combatFactors;
        public CalculationOptionsDPSWarr calcOpts;
    }
    [Rawr.Calculations.RawrModelInfo("DPSWarr", "Ability_Rogue_Ambush", CharacterClass.Warrior)]
    public class CalculationsDPSWarr : CalculationsBase {
        #region Variables and Properties

        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ///Relevant Gem IDs for DPSWarrs
                //                rare    epic  jewel
                //Red slots
                int[] red_str = { 39996, 40111, 42142 };
                int[] red_arp = { 40002, 40117, 42153 };
                int[] red_exp = { 40003, 40118, 42154 };
                //Blue slots -- All the stat+sta, No haste because str or arp should always be better
                int[] blu_str = { 40022, 40129, 40129 };
                int[] blu_arp = { 40033, 40140, 40140 };
                int[] blu_exp = { 40034, 40141, 40141 };
                int[] blu_hit = { 40088, 40166, 40166 };
                //Yellow slots
                int[] ylw_str = { 40037, 40142, 40142 }; // 10str/10crit
                int[][] ylw_hit =  { new int[] { 40014, 40125, 42156 }, // 20hit
                                     new int[] { 40038, 40143, 40143 }, // 10hit/10str
                                     new int[] { 40058, 40162, 40162 } }; // 10hit/10exp
                int[] ylw_has = { 40041, 40146, 40146 }; // 10haste/10str

                string group; bool enabled;
                List<GemmingTemplate> templates = new List<GemmingTemplate>();

                #region Strength
                enabled = true;
                group = "Strength";
                // Straight
                AddTemplates(templates, red_str, red_str, red_str, red_str, group, enabled);
                // Socket Bonus
                AddTemplates(templates, red_str, blu_str, ylw_str, red_str, group, enabled);
                #endregion

                #region Armor Pen
                enabled = true;
                group = "Armor Pen";
                // Straight
                AddTemplates(templates, red_arp, red_arp, red_arp, red_arp, group, enabled);
                // Socket Bonus
                AddTemplates(templates, red_arp, blu_arp, ylw_str, red_arp, group, enabled);
                #endregion

                #region Hit/Exp-gemming
                group = "Hit";
                enabled = false;
                // Hit
                for (int k = 0; k < ylw_hit.Length - 1; k++) // not doing hit/exp here
                {
                    // Straight
                    AddTemplates(templates, ylw_hit[k], ylw_hit[k], ylw_hit[k], ylw_hit[k], group, enabled);
                    // Socket Bonus w/Str
                    AddTemplates(templates, red_str, blu_hit, ylw_hit[k], red_str, group, enabled);
                    // Socket Bonus w/Arp
                    AddTemplates(templates, red_arp, blu_hit, ylw_hit[k], red_arp, group, enabled);
                }
                // Exp
                group = "Exp";
                enabled = false;
                // Straight
                AddTemplates(templates, red_exp, red_exp, red_exp, red_exp, group, enabled);
                // Socket Bonus
                AddTemplates(templates, red_exp, blu_exp, ylw_hit[2], red_exp, group, enabled);
                #endregion

                #region Crit-capped
                group = "Crit-capped";
                enabled = false;                
                // Strength
                AddTemplates(templates, red_str, blu_str, ylw_has, red_str, group, enabled);
                #endregion

                /*foreach (GemmingTemplate gt in templates)
                {
                    List<GemmingTemplate> foo = templates.FindAll(t => (t.BlueId == gt.BlueId && t.RedId == gt.RedId && t.YellowId == gt.YellowId && t.MetaId == gt.MetaId));
                    if (foo.Count > 1)
                    {
                        int j = 0; // break
                    }
                }*/
                
                templates.Sort(new Comparison<GemmingTemplate>(
                    delegate(GemmingTemplate first, GemmingTemplate second) {
                        string[] group1 = first.Group.Split(new char[] {' '},2);
                        string[] group2 = second.Group.Split(new char[] {' '}, 2);
                        int temp = group1[0].CompareTo(group2[0]);
                        if (temp != 0) // they're not the same
                        {
                            if (group1[0] == "Rare") return -1; // r|e or r|j
                            if (group2[0] == "Rare") return 1;  // e|r or j|r
                            if (group1[0] == "Jewelcrafter") return 1; // e|j
                            return -1; // j|e
                        }
                        else // they're the same
                        {
                            temp = group1[1].CompareTo(group2[1]);
                            if (temp != 0)
                            {
                                // str > arp > hit > exp > crit-capped
                                switch (group1[1])
                                {
                                    case "Strength": return -1;
                                    case "Armor Pen": return (group2[1] == "Strength" ? 1 : -1);
                                    case "Hit": return (group2[1] == "Strength" || group2[1] == "Armor Pen" ? 1 : -1);
                                    case "Expertise": return (group2[1] != "Crit-capped" ? 1 : -1);
                                    default: return 1;
                                }
                            }
                            else
                            {
                                return 0; // Not exact, but i'll take it for now 
                            }
                        }
                        
                }));

                

                return templates;
                /*return new List<GemmingTemplate>() {
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare",                  RedId = bold[1], YellowId = insc[1], BlueId = svrn[1], PrismaticId = bold[1], MetaId = chaotic },// STR
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare",                  RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = chaotic },// Max STR
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare",                  RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = relent  },// Max STR with All red Meta
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare",                  RedId = frac[1], YellowId = insc[1], BlueId = pusn[1], PrismaticId = frac[1], MetaId = chaotic },// ArP
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Rare",                  RedId = frac[1], YellowId = frac[1], BlueId = frac[1], PrismaticId = frac[1], MetaId = chaotic },// Max ArP
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Rare",                  RedId = frac[1], YellowId = frac[1], BlueId = frac[1], PrismaticId = frac[1], MetaId = relent  },// Max ArP with All red Meta
	
					new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", Enabled = true,  RedId = bold[2], YellowId = insc[2], BlueId = svrn[2], PrismaticId = bold[2], MetaId = chaotic },// STR
					new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", Enabled = true,  RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = chaotic },// Max STR
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", Enabled = true,  RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = relent  },// Max STR with All red Meta
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", Enabled = true,  RedId = frac[2], YellowId = insc[2], BlueId = pusn[2], PrismaticId = frac[2], MetaId = chaotic },// ArP
					new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", Enabled = true,  RedId = frac[2], YellowId = frac[2], BlueId = frac[2], PrismaticId = frac[2], MetaId = chaotic },// Max ArP
					new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", Enabled = true,  RedId = frac[2], YellowId = frac[2], BlueId = frac[2], PrismaticId = frac[2], MetaId = relent  },// Max ArP with All red Meta
						
					new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler",               RedId = bold[3], YellowId = insc[2], BlueId = svrn[2], PrismaticId = bold[3], MetaId = chaotic },// STR
					new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler",               RedId = bold[3], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[3], MetaId = chaotic },// Max STR
					new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler",               RedId = bold[3], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[3], MetaId = relent  },// Max STR with All red Meta
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler",               RedId = frac[3], YellowId = insc[2], BlueId = pusn[2], PrismaticId = frac[3], MetaId = chaotic },// ArP
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler",               RedId = frac[3], YellowId = frac[3], BlueId = frac[3], PrismaticId = frac[3], MetaId = chaotic },// Max ArP
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler",               RedId = frac[3], YellowId = frac[3], BlueId = frac[3], PrismaticId = frac[3], MetaId = relent  },// Max ArP with All red Meta
				};*/
            }
        }

        private static void AddTemplates(List<GemmingTemplate> templates, int[] red, int[] blu, int[] ylw, int[] pris, string group, bool enabled)
        {
            //Meta
            const int chaotic = 41285;
            const int relent = 41398;
            const int nightmare = 49110;
            const string groupFormat = "{0} {1}";
            string[] quality = new string[] { "Rare", "Epic", "Jewelcrafter" };
            for (int j = 0; j < 3; j++)
            {
                // Check to make sure we're not adding the same gem template twice due to repeating JC gems
                if (j != 2 || !(red[j] == red[j - 1] && blu[j] == blu[j - 1] && ylw[j] == ylw[j - 1]))
                {
                    string groupStr = String.Format(groupFormat, quality[j], group);
                    templates.Add(new GemmingTemplate()
                    {
                        Model = "DPSWarr",
                        Group = groupStr,
                        RedId = red[j],
                        BlueId = blu[j],
                        YellowId = ylw[j],
                        PrismaticId = pris[j],
                        MetaId = chaotic,
                        Enabled = (enabled && j == 1)
                    });
                    templates.Add(new GemmingTemplate()
                    {
                        Model = "DPSWarr",
                        Group = groupStr,
                        RedId = red[j],
                        BlueId = blu[j],
                        YellowId = ylw[j],
                        PrismaticId = pris[j],
                        MetaId = relent,
                        Enabled = (enabled && j == 1)
                    });
                    // Nightmare tear, only when not going for the socket bonus
                    if (red[j] != blu[j] && blu[j] != ylw[j])
                    {
                        templates.Add(new GemmingTemplate()
                        {
                            Model = "DPSWarr",
                            Group = groupStr,
                            RedId = red[j],
                            BlueId = nightmare,
                            YellowId = ylw[j],
                            PrismaticId = pris[j],
                            MetaId = chaotic,
                            Enabled = (enabled && j == 1)
                        });
                        templates.Add(new GemmingTemplate()
                        {
                            Model = "DPSWarr",
                            Group = groupStr,
                            RedId = red[j],
                            BlueId = nightmare,
                            YellowId = ylw[j],
                            PrismaticId = pris[j],
                            MetaId = relent,
                            Enabled = (enabled && j == 1)
                        });
                    }
                }
            }
        }

        
        #if RAWR3
            public ICalculationOptionsPanel _calculationOptionsPanel = null;
            public override ICalculationOptionsPanel CalculationOptionsPanel
        #else
            public CalculationOptionsPanelBase _calculationOptionsPanel = null;
            public override CalculationOptionsPanelBase CalculationOptionsPanel
        #endif
            {
                get {
                    if (_calculationOptionsPanel == null) {
                        _calculationOptionsPanel = new CalculationOptionsPanelDPSWarr();
                    }
                    return _calculationOptionsPanel;
                }
            }

        private string[] _characterDisplayCalculationLabels = null;

        public override string GetCharacterStatsString(Character character)
        {
            StringBuilder stats = new StringBuilder();
            stats.AppendFormat("Character:\t\t{0}@{1}-{2}\r\nRace:\t\t{3}",
                character.Name, character.Region, character.Realm, character.Race);

            char[] splits = {':','*'};
            Dictionary<string,string> dict = GetCharacterCalculations(character, null, false, false, true).GetAsynchronousCharacterDisplayCalculationValues();
            foreach (string s in CharacterDisplayCalculationLabels)
            {
                string[] label = s.Split(splits);
                if (dict.ContainsKey(label[1]))
                {
                    stats.AppendFormat("\r\n{0}:\t\t{1}", label[1], dict[label[1]].Split('*')[0]);
                }
            }
            
            return stats.ToString();
        }
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    _characterDisplayCalculationLabels = new string[] {
    					"Base Stats:Health and Stamina",
                        "Base Stats:Armor",
                        "Base Stats:Strength",
                        "Base Stats:Attack Power",
                        "Base Stats:Agility",
                        "Base Stats:Crit",
                        "Base Stats:Haste",
                        @"Base Stats:Armor Penetration*Rating to Cap with bonuses applied
(but not trinkets)
1400-None
1261-Battle(140)
1177-Battle(140)+T92P(084)
1051-Battle(140)+Mace(210)
0967-Battle(140)+T92P(084)+Mace(210)",
                        @"Base Stats:Hit*8.00% chance to miss base for Yellow Attacks (LVL 83 Targ)
Precision 0- 8%-0%=8%=264 Rating soft cap
Precision 1- 8%-1%=7%=230 Rating soft cap
Precision 2- 8%-2%=6%=197 Rating soft cap
Precision 3- 8%-3%=5%=164 Rating soft cap",
                        @"Base Stats:Expertise*Base 6.50% chance to be Dodged (LVL 83 Targ)
X Axis is Weapon Mastery
Y Axis is Strength of Arms
x>| 0  |  1  |  2
0 |213|180|147
1 |197|164|131
2 |180|147|115

0/2 in each the cap is 213 Rating
2/2 in each the cap is 115 Rating

Base 13.75% chance to be Parried (LVL 83 Targ)
Strength of Arms
0 |459
1 |443
2 |426

These numbers to do not include racial bonuses.",
                        
                        @"DPS Breakdown (Fury):Description*1st Number is per second or per tick
2nd Number is the average damage (factoring mitigation, hit/miss ratio and crits) per hit
3rd Number is number of times activated over fight duration",
                        "DPS Breakdown (Fury):Bloodsurge",
                        "DPS Breakdown (Fury):Bloodthirst",
                        "DPS Breakdown (Fury):Whirlwind",

                        "DPS Breakdown (Arms):Bladestorm*Bladestorm only uses 1 GCD to activate but it is channeled for a total of 4 GCD's",
                        "DPS Breakdown (Arms):Mortal Strike",
                        "DPS Breakdown (Arms):Rend",
                        "DPS Breakdown (Arms):Overpower",
                        "DPS Breakdown (Arms):Taste for Blood*Perform an Overpower",
                        "DPS Breakdown (Arms):Sudden Death*Perform an Execute",
                        "DPS Breakdown (Arms):Slam*If this number is zero, it most likely means that your other abilities are proc'g often enough that you are rarely, if ever, having to resort to Slamming your target.",
                        "DPS Breakdown (Arms):Sword Specialization",

                        "DPS Breakdown (Maintenance):Thunder Clap",
                        "DPS Breakdown (Maintenance):Shattering Throw",

                        "DPS Breakdown (General):Deep Wounds",
                        "DPS Breakdown (General):Heroic Strike",
                        "DPS Breakdown (General):Cleave",
                        "DPS Breakdown (General):White DPS",
                        "DPS Breakdown (General):Execute*<20% Spamming only",
                        "DPS Breakdown (General):Special DMG Procs*Such as Bandit's Insignia or Hand Mounted Pyro Rocket",
                        @"DPS Breakdown (General):Total DPS*1st number is total DPS
2nd number is total DMG over Duration",
                      
                        "Rage Details:Total Generated Rage",
                        "Rage Details:Needed Rage for Abilities",
                        "Rage Details:Available Free Rage*For Heroic Strikes and Cleaves",
#if (!RAWR3 && DEBUG)                        
                        "Debug:Calculation Time"
#endif
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels {
            get {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Armor",
                        "Strength",
                        "Attack Power",
                        "Agility",
                        "Crit %",
                        "Haste %",
					    "Armor Penetration %",
                        "% Chance to Miss (White)",
                        "% Chance to Miss (Yellow)",
                        "% Chance to be Dodged",
                        "% Chance to be Parried",
                        "% Chance to be Avoided (Yellow/Dodge)",
                        "Respect Highest ArP Proc Cap",
					};
                return _optimizableCalculationLabels;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        private Dictionary<string, Color> _subPointNameColors_Normal = null;
        private Dictionary<string, Color> _subPointNameColors_DPSDMG = null;
        private Dictionary<string, Color> _subPointNameColors_DPSDPR = null;
        public override Dictionary<string, Color> SubPointNameColors {
            get {
                if (_subPointNameColors_Normal == null) {
                    _subPointNameColors_Normal = new Dictionary<string, Color>();
                    _subPointNameColors_Normal.Add("DPS", Color.FromArgb(255, 255, 0, 0));
                    _subPointNameColors_Normal.Add("Survivability", Color.FromArgb(255, 64, 128, 32));
                }
                if (_subPointNameColors == null) { _subPointNameColors = _subPointNameColors_Normal; }
                Dictionary<string, Color> ret = _subPointNameColors;
                _subPointNameColors = _subPointNameColors_Normal;
                return ret;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationDPSWarr(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSWarr(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer s = new XmlSerializer(typeof(CalculationOptionsDPSWarr));
            StringReader sr = new StringReader(xml);
            CalculationOptionsDPSWarr calcOpts = s.Deserialize(sr) as CalculationOptionsDPSWarr;
            return calcOpts;
        }

        #endregion

        #region Relavancy

        public override CharacterClass TargetClass { get { return CharacterClass.Warrior; } }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(new[] {
                        ItemType.None,
                        ItemType.Leather,
                        ItemType.Mail,
                        ItemType.Plate,
                        ItemType.Bow,
                        ItemType.Crossbow,
                        ItemType.Gun,
                        ItemType.Thrown,
                        ItemType.Dagger,
                        ItemType.FistWeapon,
                        ItemType.OneHandMace,
                        ItemType.OneHandSword,
                        ItemType.OneHandAxe,
                        ItemType.Polearm,
                        ItemType.TwoHandMace,
                        ItemType.TwoHandSword,
                        ItemType.TwoHandAxe
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot) {
            // Hide the ranged weapon enchants. None of them apply to melee damage at all.
            if (enchant.Slot == ItemSlot.Ranged) { return false; }
            // Disallow Shield enchants, all shield enchants are ItemSlot.OffHand and nothing else is according to Astry
            if (enchant.Slot == ItemSlot.OffHand) { return false; }
            // Allow offhand Enchants for two-handers if toon has Titan's Grip
            // If not, turn off all enchants for the offhand
            if (character != null
                && character.WarriorTalents.TitansGrip > 0
                && enchant.Slot == ItemSlot.TwoHand
                && slot == ItemSlot.OffHand) {
                return true;
            } else if (character != null
                && character.WarriorTalents.TitansGrip == 0
                && (enchant.Slot == ItemSlot.TwoHand || enchant.Slot == ItemSlot.OneHand)
                && slot == ItemSlot.OffHand) {
                return false;
            }
            // If all the above is ok, return base version
            return enchant.FitsInSlot(slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique) {
            // We need specilized handling due to Titan's Grip
            if (item == null || character == null) {
                return false;
            }

            // Covers all TG weapons
            if (character.WarriorTalents.TitansGrip > 0) {
                // Polearm can't go in OH, can't go in MH if there's an OH, but can go in MH if there's no OH
                if (item.Type == ItemType.Polearm) {
                    if (slot == CharacterSlot.OffHand || character.OffHand != null) return false;
                    if (slot == CharacterSlot.MainHand) return true;
                    return false;
                }
                // If there's a polearm in the MH, nothing can go in OH
                if (slot == CharacterSlot.OffHand && character.MainHand != null && character.MainHand.Type == ItemType.Polearm) {
                    return false;
                }
                // Else, if it's a 2h weapon it can go in OH or MH
                if (item.Slot == ItemSlot.TwoHand && (slot == CharacterSlot.OffHand || slot == CharacterSlot.MainHand)) return true;
            }

            // Not TG, so can't dual-wield with a 2H in the MH
            if (slot == CharacterSlot.OffHand && character.MainHand != null && character.MainHand.Slot == ItemSlot.TwoHand) {
                return false;
            }

            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        private static List<string> _relevantGlyphs = null;
        public override List<string> GetRelevantGlyphs() {
            if (_relevantGlyphs == null) {
                _relevantGlyphs = new List<string>() {
                    // ===== MAJOR GLYPHS =====
                    "Glyph of Bladestorm",
                    "Glyph of Bloodthirst",
                    "Glyph of Cleaving",
                    "Glyph of Enraged Regeneration",
                    "Glyph of Execution",
                    "Glyph of Hamstring",
                    "Glyph of Heroic Strike",
                    "Glyph of Mortal Strike",
                    "Glyph of Overpower",
                    "Glyph of Rapid Charge",
                    "Glyph of Rending",
                    "Glyph of Resonating Power",
                    "Glyph of Sweeping Strikes",
                    "Glyph of Victory Rush",
                    "Glyph of Whirlwind",
                    /* The following Glyphs have been disabled as they are solely Defensive in nature.
                    "Glyph of Barbaric Insults",
                    "Glyph of Blocking",
                    "Glyph of Devastate",
                    "Glyph of Intervene",
                    "Glyph of Last Stand",
                    "Glyph of Revenge",
                    "Glyph of Shield Wall",
                    "Glyph of Shockwave",
                    "Glyph of Spell Reflection",
                    "Glyph of Sunder Armor",
                    "Glyph of Taunt",
                    "Glyph of Vigilance",*/
                    // ===== MINOR GLYPHS =====
                    "Glyph of Battle",
                    "Glyph of Bloodrage",
                    "Glyph of Charge",
                    "Glyph of Enduring Victory",
                    "Glyph of Thunder Clap",
                    "Glyph of Command",
                    /* The following Glyphs have been disabled as they are solely Defensive in nature.
                    //"Glyph of Mocking Blow",*/
                };
            }
            return _relevantGlyphs;
        }

        private bool HidingBadStuff { get { return HidingBadStuff_Def || HidingBadStuff_Spl || HidingBadStuff_PvP; } }
        internal static bool HidingBadStuff_Def { get; set; }
        internal static bool HidingBadStuff_Spl { get; set; }
        internal static bool HidingBadStuff_PvP { get; set; }

        internal static List<Trigger> _RelevantTriggers = null;
        internal static List<Trigger> RelevantTriggers {
            get {
                return _RelevantTriggers ?? (_RelevantTriggers = new List<Trigger>() {
                    Trigger.Use,
                    Trigger.MeleeCrit,
                    Trigger.MeleeHit,
                    Trigger.PhysicalCrit,
                    Trigger.PhysicalHit,
                    Trigger.DoTTick,
                    Trigger.DamageDone,
                    Trigger.DamageTaken,
                    Trigger.DamageAvoided,
                    Trigger.HSorSLHit,
                });
            }
            set { _RelevantTriggers = value; }
        }

        public override Stats GetRelevantStats(Stats stats) {
            Stats relevantStats = new Stats() {
                // Base Stats
                Stamina = stats.Stamina,
                Health = stats.Health,
                Agility = stats.Agility,
                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                Armor = stats.Armor,
                // Ratings
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                SpellHitRating = stats.SpellHitRating,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
                // Bonuses
                BonusArmor = stats.BonusArmor,
                WeaponDamage = stats.WeaponDamage,
                ArmorPenetration = stats.ArmorPenetration,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                SpellHit = stats.SpellHit,
                MovementSpeed = stats.MovementSpeed,
                StunDurReduc = stats.StunDurReduc,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                // Target Debuffs
                BossAttackPower = stats.BossAttackPower,
                BossAttackSpeedMultiplier = stats.BossAttackSpeedMultiplier,
                // Procs
                DarkmoonCardDeathProc = stats.DarkmoonCardDeathProc,
                HighestStat = stats.HighestStat,
                Paragon = stats.Paragon,
                DeathbringerProc = stats.DeathbringerProc,
                ManaorEquivRestore = stats.ManaorEquivRestore,
                // Damage Procs
                ShadowDamage = stats.ShadowDamage,
                ArcaneDamage = stats.ArcaneDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,
                FrostDamage = stats.FrostDamage,
                FireDamage = stats.FireDamage,
                ProcdShadowDamageMin = stats.ProcdShadowDamageMin,
                ProcdArcaneDamageMin = stats.ProcdArcaneDamageMin,
                ProcdHolyDamageMin = stats.ProcdHolyDamageMin,
                ProcdNatureDamageMin = stats.ProcdNatureDamageMin,
                ProcdFrostDamageMin = stats.ProcdFrostDamageMin,
                ProcdFireDamageMin = stats.ProcdFireDamageMin,
                ProcdShadowDamageMax = stats.ProcdShadowDamageMax,
                ProcdArcaneDamageMax = stats.ProcdArcaneDamageMax,
                ProcdHolyDamageMax = stats.ProcdHolyDamageMax,
                ProcdNatureDamageMax = stats.ProcdNatureDamageMax,
                ProcdFrostDamageMax = stats.ProcdFrostDamageMax,
                ProcdFireDamageMax = stats.ProcdFireDamageMax,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                    // Multipliers
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                DamageTakenMultiplier = stats.DamageTakenMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusCritChance = stats.BonusCritChance,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                // Set Bonuses
                BonusWarrior_T7_2P_SlamDamage = stats.BonusWarrior_T7_2P_SlamDamage,
                BonusWarrior_T7_4P_RageProc = stats.BonusWarrior_T7_4P_RageProc,
                BonusWarrior_T8_2P_HasteProc = stats.BonusWarrior_T8_2P_HasteProc,
                BonusWarrior_T8_4P_MSBTCritIncrease = stats.BonusWarrior_T8_4P_MSBTCritIncrease,
                BonusWarrior_T9_2P_Crit = stats.BonusWarrior_T9_2P_Crit,
                BonusWarrior_T9_2P_ArP = stats.BonusWarrior_T9_2P_ArP,
                BonusWarrior_T9_4P_SLHSCritIncrease = stats.BonusWarrior_T9_4P_SLHSCritIncrease,
                BonusWarrior_T10_2P_DWAPProc = stats.BonusWarrior_T10_2P_DWAPProc,
                BonusWarrior_T10_4P_BSSDProcChange = stats.BonusWarrior_T10_4P_BSSDProcChange,
                BonusWarrior_PvP_4P_InterceptCDReduc = stats.BonusWarrior_PvP_4P_InterceptCDReduc,
                // Special
                BonusRageGen = stats.BonusRageGen,
                BonusRageOnCrit = stats.BonusRageOnCrit,
                HealthRestore = stats.HealthRestore,
                HealthRestoreFromMaxHealth = stats.HealthRestoreFromMaxHealth,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (RelevantTriggers.Contains(effect.Trigger) && HasRelevantStats(effect.Stats))
                {
                    relevantStats.AddSpecialEffect(effect);
                }
            }
            return relevantStats;
        }
        public override bool HasRelevantStats(Stats stats) {
            bool relevant = HasWantedStats(stats) && !HasIgnoreStats(stats);
            return relevant;
        }

        private bool HasWantedStats(Stats stats) {
            bool relevant = (
                // Base Stats
                stats.Agility +
                stats.Strength +
                stats.AttackPower +
                stats.Armor +
                // Ratings
                stats.CritRating +
                stats.HitRating +
                stats.HasteRating +
                stats.ExpertiseRating +
                stats.ArmorPenetrationRating +
                // Bonuses
                stats.BonusArmor +
                stats.WeaponDamage +
                stats.ArmorPenetration +
                stats.PhysicalCrit +
                stats.PhysicalHaste +
                stats.PhysicalHit +
                stats.SpellHit + // used for TClap/Demo Shout maintenance
                stats.MovementSpeed +
                stats.StunDurReduc +
                stats.SnareRootDurReduc +
                stats.FearDurReduc +
                // Target Debuffs
                stats.BossAttackPower +
                stats.BossAttackSpeedMultiplier +
                // Procs
                stats.DarkmoonCardDeathProc +
                stats.HighestStat +
                stats.Paragon +
                stats.DeathbringerProc +
                stats.ManaorEquivRestore +
                // Damage Procs
                stats.ShadowDamage +
                stats.ArcaneDamage +
                stats.HolyDamage +
                stats.NatureDamage +
                stats.FrostDamage +
                stats.FireDamage +
                stats.ProcdShadowDamageMin +
                stats.ProcdArcaneDamageMin +
                stats.ProcdHolyDamageMin +
                stats.ProcdNatureDamageMin +
                stats.ProcdFrostDamageMin +
                stats.ProcdFireDamageMin +
                stats.ProcdShadowDamageMax +
                stats.ProcdArcaneDamageMax +
                stats.ProcdHolyDamageMax +
                stats.ProcdNatureDamageMax +
                stats.ProcdFrostDamageMax +
                stats.ProcdFireDamageMax +
                stats.BonusShadowDamageMultiplier +
                stats.BonusArcaneDamageMultiplier +
                stats.BonusHolyDamageMultiplier +
                stats.BonusNatureDamageMultiplier +
                stats.BonusFrostDamageMultiplier +
                stats.BonusFireDamageMultiplier +
                // Multipliers
                stats.BonusAgilityMultiplier +
                stats.BonusStrengthMultiplier +
                stats.BonusAttackPowerMultiplier +
                stats.BonusBleedDamageMultiplier +
                stats.BonusDamageMultiplier +
                stats.DamageTakenMultiplier +
                stats.BonusPhysicalDamageMultiplier +
                stats.BonusCritMultiplier +
                stats.BonusCritChance +
                stats.BaseArmorMultiplier +
                stats.BonusArmorMultiplier +
                // Set Bonuses
                stats.BonusWarrior_T7_2P_SlamDamage +
                stats.BonusWarrior_T7_4P_RageProc +
                stats.BonusWarrior_T8_2P_HasteProc +
                stats.BonusWarrior_T8_4P_MSBTCritIncrease +
                stats.BonusWarrior_T9_2P_Crit +
                stats.BonusWarrior_T9_2P_ArP +
                stats.BonusWarrior_T9_4P_SLHSCritIncrease +
                stats.BonusWarrior_T10_2P_DWAPProc +
                stats.BonusWarrior_T10_4P_BSSDProcChange +
                stats.BonusWarrior_PvP_4P_InterceptCDReduc +
                // Special
                stats.BonusRageGen +
                stats.BonusRageOnCrit
                ) != 0;
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (RelevantTriggers.Contains(effect.Trigger))
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            return relevant;
        }

        private bool HasSurvivabilityStats(Stats stats) {
            bool retVal = false;
            if ((stats.Health
                + stats.Stamina
                + stats.BonusHealthMultiplier
                + stats.BonusStaminaMultiplier
                + stats.HealthRestore
                + stats.HealthRestoreFromMaxHealth
                ) > 0) {
                retVal = true;
            }
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (RelevantTriggers.Contains(effect.Trigger))
                {
                    retVal |= HasSurvivabilityStats(effect.Stats);
                    if (retVal) break;
                }
            }
            return retVal;
        }

        private bool HasIgnoreStats(Stats stats) {
            if (!HidingBadStuff) { return false; }
            bool retVal = false;
            retVal = (
                // Remove Spellcasting Stuff
                (HidingBadStuff_Spl ? stats.Mp5 + stats.SpellPower + stats.Mana + stats.ManaRestore + stats.Spirit
                                    + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier
                                    + stats.SpellPenetration + stats.BonusManaMultiplier
                                    : 0f)
                // Remove Defensive Stuff (until we do that special modelling)
                + (HidingBadStuff_Def ? stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry
                                      + stats.DodgeRating + stats.ParryRating + stats.BlockRating + stats.Block
                                      : 0f)
                // Remove PvP Items
                + (HidingBadStuff_PvP ? stats.Resilience : 0f)
                ) > 0;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                //if (RelevantTriggers.Contains(effect.Trigger))
                //{
                    retVal |= !RelevantTriggers.Contains(effect.Trigger);
                    retVal |= HasIgnoreStats(effect.Stats);
                    if (retVal) break;
                //}
            }

            return retVal;
        }

        public override bool IsItemRelevant(Item item) {
            if ( // Manual override for +X to all Stats gems
                   item.Name == "Nightmare Tear"
                || item.Name == "Enchanted Tear"
                || item.Name == "Enchanted Pearl"
                || item.Name == "Chromatic Sphere"
                ) {
                return true;
                //}else if (item.Type == ItemType.Polearm && 
            } else {
                Stats stats = item.Stats;
                bool wantedStats = HasWantedStats(stats);
                bool survstats = HasSurvivabilityStats(stats);
                bool ignoreStats = HasIgnoreStats(stats);
                return (wantedStats || survstats) && !ignoreStats && base.IsItemRelevant(item);
            }
        }

        public override bool IsBuffRelevant(Buff buff) {
            string name = buff.Name;
            // Force some buffs to active
            if (name.Contains("Potion of Wild Magic")
                || name.Contains("Insane Strength Potion")
            ) {
                return true;
            }
            // Force some buffs to go away
            else if (!buff.AllowedClasses.Contains(CharacterClass.Warrior)) {
                return false;
            }
            bool haswantedStats = HasWantedStats(buff.Stats);
            bool hassurvStats = HasSurvivabilityStats(buff.Stats);
            bool hasbadstats = HasIgnoreStats(buff.Stats);
            bool retVal = haswantedStats || (hassurvStats && !hasbadstats);
            return retVal;
        }

        public override bool IsEnchantRelevant(Enchant enchant) {
            string name = enchant.Name;
            if (name.Contains("Rune of")) {
                return false; // Bad DK Enchant, Bad!
            }
            return IsProfEnchantRelevant(enchant) && (HasWantedStats(enchant.Stats) || (HasSurvivabilityStats(enchant.Stats) && !HasIgnoreStats(enchant.Stats)));
        }

        public Stats GetBuffsStats(Character character, CalculationOptionsDPSWarr calcOpts) {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            #region Racials to Force Enable
            // Draenei should always have this buff activated
            // NOTE: for other races we don't wanna take it off if the user has it active, so not adding code for that
            if (character.Race == CharacterRace.Draenei
                && !character.ActiveBuffsContains("Heroic Presence"))
            {
                character.ActiveBuffsAdd(("Heroic Presence"));
            }
            #endregion

            List<Buff> buffGroup = new List<Buff>();
            #region Maintenance Auto-Fixing
            // Removes the Sunder Armor if you are maintaining it yourself
            // Also removes Acid Spit and Expose Armor
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SunderArmor_]) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Sunder Armor"));
                buffGroup.Add(Buff.GetBuffByName("Acid Spit"));
                buffGroup.Add(Buff.GetBuffByName("Expose Armor"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }

            // Removes the Thunder Clap & Improved Buffs if you are maintaining it yourself
            // Also removes Judgements of the Just, Infected Wounds, Frost Fever, Improved Icy Touch
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ThunderClap_]) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Thunder Clap"));
                buffGroup.Add(Buff.GetBuffByName("Improved Thunder Clap"));
                buffGroup.Add(Buff.GetBuffByName("Judgements of the Just"));
                buffGroup.Add(Buff.GetBuffByName("Infected Wounds"));
                buffGroup.Add(Buff.GetBuffByName("Frost Fever"));
                buffGroup.Add(Buff.GetBuffByName("Improved Icy Touch"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }

            // Removes the Demoralizing Shout & Improved Buffs if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.DemoralizingShout_]) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Demoralizing Shout"));
                buffGroup.Add(Buff.GetBuffByName("Improved Demoralizing Shout"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }

            // Removes the Battle Shout & Commanding Presence Buffs if you are maintaining it yourself
            // Also removes their equivalent of Blessing of Might (+Improved)
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.BattleShout_]) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Commanding Presence (Attack Power)"));
                buffGroup.Add(Buff.GetBuffByName("Battle Shout"));
                buffGroup.Add(Buff.GetBuffByName("Improved Blessing of Might"));
                buffGroup.Add(Buff.GetBuffByName("Blessing of Might"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }

            // Removes the Commanding Shout & Commanding Presence Buffs if you are maintaining it yourself
            // Also removes their equivalent of Blood Pact (+Improved Imp)
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.CommandingShout_]) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Commanding Presence (Health)"));
                buffGroup.Add(Buff.GetBuffByName("Commanding Shout"));
                buffGroup.Add(Buff.GetBuffByName("Improved Imp"));
                buffGroup.Add(Buff.GetBuffByName("Blood Pact"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }
            #endregion

            #region Passive Ability Auto-Fixing
            // Removes the Trauma Buff and it's equivalent Mangle if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (character.WarriorTalents.Trauma > 0)
            {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Trauma"));
                buffGroup.Add(Buff.GetBuffByName("Mangle"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }

            // Removes the Blood Frenzy Buff and it's equivalent of Savage Combat if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (character.WarriorTalents.BloodFrenzy > 0)
            {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Blood Frenzy"));
                buffGroup.Add(Buff.GetBuffByName("Savage Combat"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }

            // Removes the Rampage Buff and it's equivalent of Leader of the Pack if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (character.WarriorTalents.Rampage > 0)
            {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Rampage"));
                buffGroup.Add(Buff.GetBuffByName("Leader of the Pack"));
                MaintBuffHelper(buffGroup, character, removedBuffs);
            }
            #endregion

            #region Special Pot Handling
            foreach (Buff potionBuff in character.ActiveBuffs.FindAll(b => b.Name.Contains("Potion")))
            {
                if (potionBuff.Stats._rawSpecialEffectData != null
                    && potionBuff.Stats._rawSpecialEffectData[0] != null)
                {
                    Stats newStats = new Stats();
                    newStats.AddSpecialEffect(new SpecialEffect(potionBuff.Stats._rawSpecialEffectData[0].Trigger,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Stats,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Duration,
                                                                calcOpts.Duration,
                                                                potionBuff.Stats._rawSpecialEffectData[0].Chance,
                                                                potionBuff.Stats._rawSpecialEffectData[0].MaxStack));

                    Buff newBuff = new Buff() { Stats = newStats };
                    character.ActiveBuffs.Remove(potionBuff);
                    character.ActiveBuffsAdd(newBuff);
                    removedBuffs.Add(potionBuff);
                    addedBuffs.Add(newBuff);
                }
            }
            #endregion

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
           
            foreach (Buff b in removedBuffs) {
                character.ActiveBuffsAdd(b);
            }
            foreach (Buff b in addedBuffs){
                character.ActiveBuffs.Remove(b);
            }

            return statsBuffs;
        }

        private void MaintBuffHelper(List<Buff> buffGroup, Character character, List<Buff> removedBuffs)
        {
            foreach (Buff b in buffGroup)
            {
                if (character.ActiveBuffs.Remove(b)) removedBuffs.Add(b);
            }
        }
        public override void SetDefaults(Character character) {
            //CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            //WarriorTalents  talents = character.WarriorTalents;

            //if (calcOpts == null) { calcOpts = new CalculationOptionsDPSWarr(); }
            //calcOpts.FuryStance = talents.TitansGrip == 1; // automatically set arms stance if you don't have TG talent by default
            //bool doit = false;
            //bool removeother = false;

            // == SUNDER ARMOR ==
            // The benefits from both Sunder Armor, Acid Spit and Expose Armor are identical
            // But the other buffs don't stay up like Sunder
            // If we are maintaining Sunder Armor ourselves, then we should reap the benefits
            /*doit = calcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SunderArmor_] && !character.ActiveBuffs.Contains(Buff.GetBuffByName("Sunder Armor"));
            removeother = doit;
            if (removeother) {
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Acid Spit"))) {
                    character.ActiveBuffs.Remove(Buff.GetBuffByName("Acid Spit"));
                }
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Expose Armor"))) {
                    character.ActiveBuffs.Remove(Buff.GetBuffByName("Expose Armor"));
                }
            }
            if (doit) { character.ActiveBuffsAdd(("Sunder Armor")); }*/
        }

        public override bool IncludeOffHandInCalculations(Character character) {
            if (character.OffHand == null) { return false; }
            if (character.CurrentTalents is WarriorTalents) {
                WarriorTalents talents = (WarriorTalents)character.CurrentTalents;
                if (talents.TitansGrip > 0) {
                    return true;
                } else {// if (character.MainHand.Slot != ItemSlot.TwoHand)
                    return base.IncludeOffHandInCalculations(character);
                }
            }
            return false;
        }

        #endregion

        #region Special Comparison Charts
        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        "Ability DPS+Damage",
                        //"Ability Maintenance Changes",
                        "Rage Cost per Damage",
                    };
                }
                return _customChartNames;
            }
        }

        float getDPS(Character character, int Iter, bool with) {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.Maintenance[Iter] = with;
            CharacterCalculationsDPSWarr calculations = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr;
            return calculations.TotalDPS;
        }
        float getSurv(Character character, int Iter, bool with) {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.Maintenance[Iter] = with;
            CharacterCalculationsDPSWarr calculations = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr;
            return calculations.TotalHPS;
        }

        ComparisonCalculationDPSWarr getComp(Character character, string Name, int Iter) {
            ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
            comparison.Name = Name;
            float with = getDPS(character.Clone(), Iter, true);
            float without = getDPS(character.Clone(), Iter, false);
            comparison.DPSPoints = with - without;
            with = getSurv(character.Clone(), Iter, true);
            without = getSurv(character.Clone(), Iter, false);
            comparison.SurvPoints = with - without;
            return comparison;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            CharacterCalculationsDPSWarr calculations = GetCharacterCalculations(character.Clone()) as CharacterCalculationsDPSWarr;
            CalculationOptionsDPSWarr calcOpts = character.Clone().CalculationOptions as CalculationOptionsDPSWarr;
            bool[] origMaints = (bool[])calcOpts.Maintenance.Clone();

            switch (chartName) {
                #region Ability DPS+Damage
                case "Ability DPS+Damage": {
                    if(_subPointNameColors_DPSDMG == null){
                        _subPointNameColors_DPSDMG = new Dictionary<string, Color>();
                        _subPointNameColors_DPSDMG.Add("DPS", Color.FromArgb(255, 255, 0, 0));
                        _subPointNameColors_DPSDMG.Add("Damage Per Hit", Color.FromArgb(255, 0, 0, 255));
                    }
                    _subPointNameColors = _subPointNameColors_DPSDMG;
                    List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                    foreach (Rawr.DPSWarr.Rotation.AbilWrapper aw in calculations.Rot.GetAbilityList())
                    {
                        if (aw.ability.DamageOnUse == 0) { continue; }
                        ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                        comparison.Name = aw.ability.Name;
                        comparison.Description = aw.ability.Description;
                        comparison.DPSPoints = aw.DPS;
                        comparison.SurvPoints = aw.ability.DamageOnUse;
                        comparisons.Add(comparison);
                    }
                    foreach (ComparisonCalculationDPSWarr comp in comparisons) {
                        comp.OverallPoints = comp.DPSPoints + comp.SurvPoints;
                    }
                    calcOpts.Maintenance = origMaints;
                    return comparisons.ToArray();
                }
                #endregion
                #region Ability Maintenance Changes
                case "Ability Maintenance Changes": {
                    List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                    #region Rage Generators
                    comparisons.Add(getComp(character, "Berserker Rage", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BerserkerRage_));
                    comparisons.Add(getComp(character, "Bloodrage", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bloodrage_));
                    #endregion
                    #region Maintenance
                    comparisons.Add(getComp(character, "Battle Shout", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BattleShout_));
                    comparisons.Add(getComp(character, "Commanding Shout", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.CommandingShout_));
                    comparisons.Add(getComp(character, "Demoralizing Shout", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DemoralizingShout_));
                    comparisons.Add(getComp(character, "Sunder Armor", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SunderArmor_));
                    comparisons.Add(getComp(character, "Thunder Clap", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ThunderClap_));
                    comparisons.Add(getComp(character, "Hamstring", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Hamstring_));
                    #endregion
                    #region Periodics
                    comparisons.Add(getComp(character, "Shattering Throw", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ShatteringThrow_));
                    comparisons.Add(getComp(character, "Sweeping Strikes", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_));
                    comparisons.Add(getComp(character, "Death Wish", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DeathWish_));
                    comparisons.Add(getComp(character, "Recklessness", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Recklessness_));
                    comparisons.Add(getComp(character, "Enraged Regeneration", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.EnragedRegeneration_));
                    #endregion
                    #region Damage Dealers
                    if (calculations.Rot.GetType() == typeof(FuryRotation)) {
                        #region Fury
                        comparisons.Add(getComp(character, "Bloodsurge", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bloodsurge_));
                        comparisons.Add(getComp(character, "Bloodthirst", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bloodthirst_));
                        comparisons.Add(getComp(character, "Whirlwind", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Whirlwind_));
                        #endregion
                    } else if (calculations.Rot.GetType() == typeof(ArmsRotation)) {
                        #region Arms
                        comparisons.Add(getComp(character, "Bladestorm", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bladestorm_));
                        comparisons.Add(getComp(character, "Mortal Strike", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.MortalStrike_));
                        comparisons.Add(getComp(character, "Rend", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Rend_));
                        comparisons.Add(getComp(character, "Overpower", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Overpower_));
                        comparisons.Add(getComp(character, "Taste for Blood", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.TasteForBlood_));
                        comparisons.Add(getComp(character, "Sudden Death", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SuddenDeath_));
                        comparisons.Add(getComp(character, "Slam", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Slam_));
                        #endregion
                    }
                    comparisons.Add(getComp(character, "<20% Execute Spamming", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_));
                    #endregion
                    #region Rage Dumps
                    comparisons.Add(getComp(character, "Heroic Strike", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_));
                    comparisons.Add(getComp(character, "Cleave", (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_));
                    #endregion
                    foreach (ComparisonCalculationDPSWarr comp in comparisons) {
                        comp.OverallPoints = comp.DPSPoints + comp.SurvPoints;
                    }
                    calcOpts.Maintenance = origMaints;
                    return comparisons.ToArray();
                }
                #endregion
                #region Rage Cost per Damage
                case "Rage Cost per Damage": {
                    if (_subPointNameColors_DPSDPR == null) {
                        _subPointNameColors_DPSDPR = new Dictionary<string, Color>();
                        _subPointNameColors_DPSDPR.Add("Damage Per Rage Point", Color.FromArgb(255, 255, 0, 0));
                        _subPointNameColors_DPSDPR.Add("Deep Wounds Bonus", Color.FromArgb(255, 0, 0, 255));
                    }
                    _subPointNameColors = _subPointNameColors_DPSDPR;
                    List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                    float DeepWoundsDamage = calculations.Rot.DW.TickSize * 6f;

                    foreach (Rotation.AbilWrapper aw in calculations.Rot.GetAbilityList())
                    {
                        if (aw.ability.DamageOnUse == 0) { continue; }
                        ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                        comparison.Name = aw.ability.Name;
                        comparison.Description = string.Format("Costs {0} Rage\r\n{1}",aw.ability.RageCost,aw.ability.Description);
                        comparison.SubPoints[0] = (aw.ability.DamageOnUse * aw.ability.AvgTargets) / aw.ability.RageCost;
                        comparison.SubPoints[1] = (aw.ability.MHAtkTable.Crit * DeepWoundsDamage) / aw.ability.RageCost;
                        comparisons.Add(comparison);
                    }
                    foreach (ComparisonCalculationDPSWarr comp in comparisons) {
                        comp.OverallPoints = comp.SubPoints[0] + comp.SubPoints[1];
                    }
                    calcOpts.Maintenance = origMaints;
                    return comparisons.ToArray();
                }
                #endregion
                default: { calcOpts.Maintenance = origMaints; return new ComparisonCalculationBase[0]; }
            }
        }
        #endregion

        #region Character Calcs

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) {
#if (!RAWR3 && DEBUG)
            if (character.Name == "") {
                DateTime dtEnd = DateTime.Now.AddSeconds(10);
                int count = 0;
                while (dtEnd > DateTime.Now) {
                    Calculations.GetCharacterCalculations(character);
                    count++;
                }
                float calcsPerSec = count / 10f;
            }
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
#endif
            CharacterCalculationsDPSWarr calculatedStats = new CharacterCalculationsDPSWarr();
            cacheChar = character;
            try {
                CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
                if (calcOpts == null) calcOpts = new CalculationOptionsDPSWarr();
                
                CombatFactors combatFactors;
                Skills.WhiteAttacks whiteAttacks;
                Rotation Rot;

                Stats stats = GetCharacterStats(character, additionalItem, StatType.Average, calcOpts, out combatFactors, out whiteAttacks, out Rot);

                DPSWarrCharacter charStruct;
                charStruct.calcOpts = calcOpts;
                charStruct.Rot = Rot;
                charStruct.combatFactors = combatFactors;
                charStruct.Char = character;

                WarriorTalents talents = character.WarriorTalents;
                //CombatFactors combatFactors = new CombatFactors(character, stats, calcOpts);
                 
                if (calcOpts.UseMarkov)
                {
                    Markov.StateSpaceGeneratorArmsTest b = new Markov.StateSpaceGeneratorArmsTest();
                    b.StateSpaceGeneratorArmsTest1(character, stats, combatFactors, whiteAttacks, calcOpts);
                }

                Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race);

                /*Rotation Rot;
                if (calcOpts.FuryStance) Rot = new FuryRotation(character, stats, combatFactors, whiteAttacks, calcOpts);
                else Rot = new ArmsRotation(character, stats, combatFactors, whiteAttacks, calcOpts);*/

                calculatedStats.Duration = calcOpts.Duration;
                
                calculatedStats.AverageStats = stats;
                if (needsDisplayCalculations)
                {
                    calculatedStats.UnbuffedStats = GetCharacterStats(character, additionalItem, StatType.Unbuffed, calcOpts);
                    calculatedStats.BuffedStats = GetCharacterStats(character, additionalItem, StatType.Buffed, calcOpts);
                    calculatedStats.BuffsStats = GetBuffsStats(character, calcOpts);
                    calculatedStats.MaximumStats = GetCharacterStats(character, additionalItem, StatType.Maximum, calcOpts);
                }
                
                calculatedStats.combatFactors = combatFactors; 
                calculatedStats.Rot = Rot; 
                calculatedStats.TargetLevel = calcOpts.TargetLevel; 
                calculatedStats.BaseHealth = statsRace.Health; 
                {// == Attack Table ==
                    // Miss
                    calculatedStats.Miss = stats.Miss;
                    calculatedStats.HitRating = stats.HitRating;
                    calculatedStats.ExpertiseRating = stats.ExpertiseRating;
                    calculatedStats.Expertise = StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Warrior) + stats.Expertise;
                    calculatedStats.MhExpertise = combatFactors._c_mhexpertise;
                    calculatedStats.OhExpertise = combatFactors._c_ohexpertise;
                    calculatedStats.WeapMastPerc = character.WarriorTalents.WeaponMastery / 100f;
                    calculatedStats.AgilityCritBonus = StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Warrior);
                    calculatedStats.CritRating = stats.CritRating;
                    calculatedStats.CritPercent = stats.PhysicalCrit;
                    calculatedStats.MhCrit = combatFactors._c_mhycrit;
                    calculatedStats.OhCrit = combatFactors._c_ohycrit;
                } 
                // Offensive
                calculatedStats.TeethBonus = (stats.Armor * talents.ArmoredToTheTeeth / 108f);
                calculatedStats.BonusCritPercPoleAxeSpec = ((character.MainHand != null && (combatFactors._c_mhItemType == ItemType.TwoHandAxe || combatFactors._c_mhItemType == ItemType.Polearm)) ? character.WarriorTalents.PoleaxeSpecialization * 0.01f : 0.00f);
                calculatedStats.ArmorPenetrationMaceSpec = ((character.MainHand != null && combatFactors._c_mhItemType == ItemType.TwoHandMace) ? character.WarriorTalents.MaceSpecialization * 0.03f : 0.00f);
                calculatedStats.ArmorPenetrationStance = ((!calcOpts.FuryStance) ? (0.10f + stats.BonusWarrior_T9_2P_ArP) : 0.00f);
                calculatedStats.ArmorPenetrationRating = stats.ArmorPenetrationRating;
                calculatedStats.ArmorPenetrationRating2Perc = StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating);
                calculatedStats.ArmorPenetration = calculatedStats.ArmorPenetrationMaceSpec
                    + calculatedStats.ArmorPenetrationStance
                    + calculatedStats.ArmorPenetrationRating2Perc;
                calculatedStats.HasteRating = stats.HasteRating;
                calculatedStats.HastePercent = stats.PhysicalHaste; //talents.BloodFrenzy * (0.05f) + StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Warrior);
                
                // DPS
                Rot.Initialize(calculatedStats);
                
                // Neutral
                // Defensive
                calculatedStats.Armor = stats.Armor; 

                calculatedStats.floorstring = calcOpts.AllowFlooring ? "000" : "000.00"; 

                Rot.MakeRotationandDoDPS(true, needsDisplayCalculations);

                // Special Damage Procs, like Bandit's Insignia or Hand-mounted Pyro Rockets
                Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
                Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();
                CalculateTriggers(charStruct, triggerIntervals, triggerChances);
                DamageProcs.SpecialDamageProcs SDP;
                calculatedStats.SpecProcDPS = 0f;
                if (stats._rawSpecialEffectData != null)
                {
                    SDP = new Rawr.DamageProcs.SpecialDamageProcs(character, stats,
                        calcOpts.TargetLevel - character.Level, new List<SpecialEffect>(stats.SpecialEffects()),
                        triggerIntervals, triggerChances, calcOpts.Duration, combatFactors.DamageReduction);

                    calculatedStats.SpecProcDPS = SDP.CalculateAll();
                    /*calculatedStats.SpecProcDPS += SDP.Calculate(ItemDamageType.Physical);
                    calculatedStats.SpecProcDPS += SDP.Calculate(ItemDamageType.Shadow);
                    calculatedStats.SpecProcDPS += SDP.Calculate(ItemDamageType.Holy);
                    calculatedStats.SpecProcDPS += SDP.Calculate(ItemDamageType.Arcane);
                    calculatedStats.SpecProcDPS += SDP.Calculate(ItemDamageType.Nature);
                    calculatedStats.SpecProcDPS += SDP.Calculate(ItemDamageType.Fire);
                    calculatedStats.SpecProcDPS += SDP.Calculate(ItemDamageType.Frost);*/
                }
                calculatedStats.TotalDPS += calculatedStats.SpecProcDPS;

                // Survivability
                if (stats.HealthRestoreFromMaxHealth > 0) {
                    stats.HealthRestore += stats.HealthRestoreFromMaxHealth * stats.Health * calcOpts.Duration;
                }

                float Health2Surv  = (stats.Health) / 100f; 
                      Health2Surv += (stats.HealthRestore) / 1000f; 
                float DmgTakenMods2Surv = (1f - stats.DamageTakenMultiplier) * 100f;
                float BossAttackPower2Surv = stats.BossAttackPower / 14f * -1f;
                float BossAttackSpeedMods2Surv = (1f - stats.BossAttackSpeedMultiplier) * 100f;
                calculatedStats.TotalHPS = Rot._HPS_TTL; 
                calculatedStats.Survivability = calcOpts.SurvScale * (calculatedStats.TotalHPS
                                                                      + Health2Surv
                                                                      + DmgTakenMods2Surv
                                                                      + BossAttackPower2Surv
                                                                      + BossAttackSpeedMods2Surv);
                calculatedStats.OverallPoints = calculatedStats.TotalDPS + calculatedStats.Survivability; 

                //calculatedStats.UnbuffedStats = GetCharacterStats(character, additionalItem, StatType.Unbuffed, calcOpts);
                if (needsDisplayCalculations)
                {
                    calculatedStats.BuffedStats = GetCharacterStats(character, additionalItem, StatType.Buffed, calcOpts);
                    //calculatedStats.MaximumStats = GetCharacterStats(character, additionalItem, StatType.Maximum, calcOpts);

                    float maxArp = calculatedStats.BuffedStats.ArmorPenetrationRating;
                    foreach (SpecialEffect effect in calculatedStats.BuffedStats.SpecialEffects(s => s.Stats.ArmorPenetrationRating > 0f))
                    {
                        maxArp += effect.Stats.ArmorPenetrationRating;
                    }
                    calculatedStats.MaxArmorPenetration = calculatedStats.ArmorPenetrationMaceSpec
                        + calculatedStats.ArmorPenetrationStance
                        + StatConversion.GetArmorPenetrationFromRating(maxArp);
                }

            } catch (Exception ex) {
                ErrorBox eb = new ErrorBox("Error in creating Stat Pane Calculations",
                    ex.Message, "GetCharacterCalculations()", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
#if (!RAWR3 && DEBUG)
            if (needsDisplayCalculations)
            {
                sw.Stop();
                long elapsedTime = sw.Elapsed.Ticks;
                calculatedStats.calculationTime = elapsedTime;
            }
#endif
            return calculatedStats;
        }

        private enum StatType { Unbuffed, Buffed, Average, Maximum };
        
        public override Stats GetCharacterStats(Character character, Item additionalItem) {
            try {
                return GetCharacterStats(character, additionalItem, StatType.Average, (CalculationOptionsDPSWarr)character.CalculationOptions);
            } catch (Exception ex) {
                ErrorBox eb = new ErrorBox("Error in getting Character Stats",
                    ex.Message, "GetCharacterStats()", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
            return new Stats() { };
        }

        private Stats GetCharacterStats_Buffed(Character character, Item additionalItem, CalculationOptionsDPSWarr calcOpts, bool isBuffed) {
            cacheChar = character;
            if (calcOpts == null) { calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr; }
            WarriorTalents talents = character.WarriorTalents;

            #region From Race
            Stats statsRace = BaseStats.GetBaseStats(character.Level, CharacterClass.Warrior, character.Race);
            #endregion
            #region From Gear/Buffs
            Stats statsBuffs = (isBuffed ? GetBuffsStats(character, calcOpts) : new Stats());
            Stats statsItems = GetItemStats(character, additionalItem);
            /*if (statsItems._rawSpecialEffectData != null) {
                foreach (SpecialEffect effect in statsItems._rawSpecialEffectData) {
                    if (effect != null && effect.Stats != null && effect.Stats.DeathbringerProc > 0)
                    {
                        statsItems.RemoveSpecialEffect(effect);
                        List<SpecialEffect> new2add = SpecialEffects.GetDeathBringerEffects(character.Class, effect.Stats.DeathbringerProc);
                        foreach (SpecialEffect e in new2add) {
                            e.Stats.DeathbringerProc = 1;
                            statsItems.AddSpecialEffect(e);
                        }
                    }
                }
            }*/
            #endregion
            #region From Options
            Stats statsOptionsPanel = new Stats()
            {
                BonusStrengthMultiplier = (calcOpts.FuryStance ? talents.ImprovedBerserkerStance * 0.04f : 0f),
                PhysicalCrit = (calcOpts.FuryStance ? 0.03f + statsBuffs.BonusWarrior_T9_2P_Crit : 0f),
                    
                DamageTakenMultiplier = (calcOpts.FuryStance ? 0.05f : 0f),
            };
            #endregion
            #region From Talents
            Stats statsTalents = new Stats() {
                // Offensive
                BonusDamageMultiplier = (character.MainHand == null ? 0f :
                    /* One Handed Weapon Spec  Not using this to prevent any misconceptions
                    ((character.MainHand.Slot == ItemSlot.OneHand) ? 1f + talents.OneHandedWeaponSpecialization * 0.02f : 1f)
                      */
                    // Two Handed Weapon Spec
                                            ((character.MainHand.Slot == ItemSlot.TwoHand) ? 1f + talents.TwoHandedWeaponSpecialization * 0.02f : 1f)
                                            *
                    // Titan's Grip Penalty
                                            ((talents.TitansGrip > 0 && character.OffHand != null && (character.OffHand.Slot == ItemSlot.TwoHand || character.MainHand.Slot == ItemSlot.TwoHand) ? 0.90f : 1f))
                    // Convert it back a simple mod number
                                            - 1f
                                         ),
                BonusPhysicalDamageMultiplier = (calcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Rend_] // Have Rend up
                                                 || talents.DeepWounds > 0 // Have Deep Wounds
                                                ? talents.BloodFrenzy * 0.02f : 0f),
                PhysicalCrit = talents.Cruelty * 0.01f,
                BonusStaminaMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                BonusStrengthMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                Expertise = talents.Vitality * 2.0f + talents.StrengthOfArms * 2.0f,
                PhysicalHit = talents.Precision * 0.01f,
                PhysicalHaste = talents.BloodFrenzy * 0.05f,
                StunDurReduc = talents.IronWill / 15f,
                // Defensive
                Parry = talents.Deflection * 0.01f,
                Dodge = talents.Anticipation * 0.01f,
                Block = talents.ShieldSpecialization * 0.01f,
                BonusBlockValueMultiplier = talents.ShieldMastery * 0.15f,
                BonusShieldSlamDamage = talents.GagOrder * 0.05f,
                DevastateCritIncrease = talents.SwordAndBoard * 0.05f,
                BaseArmorMultiplier = talents.Toughness * 0.02f,
            };
            // Add Talents that give SpecialEffects
            if (talents.Rampage > 0 && isBuffed) {
                statsTalents.PhysicalCrit += 0.05f;
            }
            if (talents.WreckingCrew > 0) {
                //float value = talents.WreckingCrew * 0.02f;
                SpecialEffect wrecking = new SpecialEffect(Trigger.MeleeCrit, new Stats() { BonusDamageMultiplier = talents.WreckingCrew * 0.02f, }, 12, 0);
                statsTalents.AddSpecialEffect(wrecking);
            }
            if (talents.Trauma > 0 && character.MainHand != null) {
                //float value = talents.Trauma * 0.15f;
                SpecialEffect trauma = new SpecialEffect(Trigger.MeleeCrit, new Stats() { BonusBleedDamageMultiplier = talents.Trauma * 0.15f, }, 15, 0);
                statsTalents.AddSpecialEffect(trauma);
            }
            if (talents.DeathWish > 0 && calcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DeathWish_]) {
                SpecialEffect death = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusDamageMultiplier = 0.20f, DamageTakenMultiplier = 0.05f, },
                    30f, 3f * 60f * (1f - 1f / 9f * talents.IntensifyRage));
                statsTalents.AddSpecialEffect(death);
            }
            #endregion

            /*Stats statsGearEnchantsBuffs = new Stats();
            statsGearEnchantsBuffs.Accumulate(statsItems);
            statsGearEnchantsBuffs.Accumulate(statsBuffs);*/
            Stats statsTotal = new Stats();
            statsTotal.Accumulate(statsRace);
            statsTotal.Accumulate(statsItems);
            statsTotal.Accumulate(statsBuffs);
            statsTotal.Accumulate(statsTalents);
            statsTotal.Accumulate(statsOptionsPanel);
            statsTotal = UpdateStatsAndAdd(statsTotal, null, character);
            //Stats statsProcs = new Stats();

           
            // Dodge (your dodging incoming attacks)
            statsTotal.Dodge += StatConversion.GetDodgeFromAgility(statsTotal.Agility, character.Class);
            statsTotal.Dodge += StatConversion.GetDodgeFromRating(statsTotal.DodgeRating, character.Class);

            // Parry (your parrying incoming attacks)
            statsTotal.Parry += StatConversion.GetParryFromRating(statsTotal.ParryRating, character.Class);

            return statsTotal;
        }

        private Stats GetCharacterStats(Character character, Item additionalItem, StatType statType, CalculationOptionsDPSWarr calcOpts)
        {
            CombatFactors temp; Skills.WhiteAttacks temp2; Rotation temp3;
            return GetCharacterStats(character, additionalItem, statType, calcOpts, out temp, out temp2, out temp3);
        }
        private Stats GetCharacterStats(Character character, Item additionalItem, StatType statType, CalculationOptionsDPSWarr calcOpts, out CombatFactors combatFactors, out Skills.WhiteAttacks whiteAttacks, out Rotation Rot)
        {
            cacheChar = character;
            Stats statsTotal = GetCharacterStats_Buffed(character, additionalItem, calcOpts, statType != StatType.Unbuffed);
            combatFactors = new CombatFactors(character, statsTotal, calcOpts);
            whiteAttacks = new Skills.WhiteAttacks(character, statsTotal, combatFactors, calcOpts);
            if (calcOpts.FuryStance) Rot = new FuryRotation(character, statsTotal, combatFactors, whiteAttacks, calcOpts);
            else Rot = new ArmsRotation(character, statsTotal, combatFactors, whiteAttacks, calcOpts);
            
            if (statType == (StatType.Buffed | StatType.Unbuffed))
            {
                return statsTotal;
            }
            // SpecialEffects: Supposed to handle all procs such as Berserking, Mirror of Truth, Grim Toll, etc.
            WarriorTalents talents = character.WarriorTalents;
            Rot.Initialize();
            Rot.MakeRotationandDoDPS(false, false);
            Rot.AddValidatedSpecialEffects(statsTotal, talents);

            DPSWarrCharacter charStruct;
            charStruct.calcOpts = calcOpts;
            charStruct.Char = character;
            charStruct.combatFactors = combatFactors;
            charStruct.Rot = Rot;

            float fightDuration = calcOpts.Duration;

            List<SpecialEffect> bersMainHand = new List<SpecialEffect>();
            List<SpecialEffect> bersOffHand = new List<SpecialEffect>();

            if (character.MainHandEnchant != null/* && character.MainHandEnchant.Id == 3789*/) { // 3789 = Berserker Enchant ID, but now supporting other proc effects as well
                Stats.SpecialEffectEnumerator mhEffects = character.MainHandEnchant.Stats.SpecialEffects();
                if (mhEffects.MoveNext()) {
                    bersMainHand.Add(mhEffects.Current); 
                }
            }
            if (character.MainHand != null && character.MainHand.Item.Stats._rawSpecialEffectData != null)
            {
                Stats.SpecialEffectEnumerator mhEffects = character.MainHand.Item.Stats.SpecialEffects();
                if (mhEffects.MoveNext()) { bersMainHand.Add(mhEffects.Current); }
            }
            if (combatFactors.useOH && character.OffHandEnchant != null /*&& character.OffHandEnchant.Id == 3789*/) {
                Stats.SpecialEffectEnumerator ohEffects = character.OffHandEnchant.Stats.SpecialEffects();
                if (ohEffects.MoveNext()) { bersOffHand.Add(ohEffects.Current); }
            }
            if (character.OffHand != null && character.OffHand.Item.Stats._rawSpecialEffectData != null)
            {
                Stats.SpecialEffectEnumerator ohEffects = character.OffHand.Item.Stats.SpecialEffects();
                if (ohEffects.MoveNext()) { bersOffHand.Add(ohEffects.Current); }
            }
            if (statType == StatType.Average)
            {
                DoSpecialEffects(charStruct, bersMainHand, bersOffHand, statsTotal);
            }
            else // if (statType == StatType.Maximum)
            {
                Stats maxSpecEffects = new Stats();
                foreach (SpecialEffect effect in statsTotal.SpecialEffects()) maxSpecEffects.Accumulate(effect.Stats);
                return UpdateStatsAndAdd(maxSpecEffects, combatFactors.StatS, character);
            }
            //UpdateStatsAndAdd(statsProcs, statsTotal, character); // Already done in GetSpecialEffectStats

            // special case for dual wielding w/ berserker enchant on one/both weapons, as they act independently
            //combatFactors.StatS = statsTotal;
            Stats bersStats = new Stats();
            foreach (SpecialEffect e in bersMainHand) {
                if (e.Duration == 0) {
                    bersStats.ShadowDamage = e.GetAverageProcsPerSecond(fightDuration / Rot.AttemptedAtksOverDurMH, Rot.LandedAtksOverDurMH / Rot.AttemptedAtksOverDurMH, combatFactors._c_mhItemSpeed, calcOpts.SE_UseDur ? fightDuration : 0);
                } else {
                    // berserker enchant id
                    float f = e.GetAverageUptime(fightDuration / Rot.AttemptedAtksOverDurMH, Rot.LandedAtksOverDurMH / Rot.AttemptedAtksOverDurMH, combatFactors._c_mhItemSpeed, calcOpts.SE_UseDur ? fightDuration : 0);
                    bersStats.Accumulate(e.Stats, f);
                }
            }
            foreach (SpecialEffect e in bersOffHand) {
                if (e.Duration == 0) {
                    bersStats.ShadowDamage += e.GetAverageProcsPerSecond(fightDuration / Rot.AttemptedAtksOverDurOH, Rot.LandedAtksOverDurOH / Rot.AttemptedAtksOverDurOH, combatFactors._c_ohItemSpeed, calcOpts.SE_UseDur ? fightDuration : 0);
                } else {
                    float f = e.GetAverageUptime(fightDuration / Rot.AttemptedAtksOverDurOH, Rot.LandedAtksOverDurOH / Rot.AttemptedAtksOverDurOH, combatFactors._c_ohItemSpeed, calcOpts.SE_UseDur ? fightDuration : 0);
                    bersStats.Accumulate(e.Stats, f);
                }
            }
            combatFactors.StatS = UpdateStatsAndAdd(bersStats, combatFactors.StatS, character);
            combatFactors.InvalidateCache();
            return combatFactors.StatS;
        }

        private void DoSpecialEffects(DPSWarrCharacter charStruct,
            List<SpecialEffect> bersMainHand, List<SpecialEffect> bersOffHand,
            Stats statsTotal)
        {
            
            #region Initialize Triggers
            Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
            Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();

            CalculateTriggers(charStruct, triggerIntervals, triggerChances);
            #endregion

            #region ArPen Lists
            List<float> tempArPenRatings = new List<float>();
            List<float> tempArPenRatingUptimes = new List<float>();
            List<SpecialEffect> tempArPenEffects = new List<SpecialEffect>();
            List<float> tempArPenEffectIntervals = new List<float>();
            List<float> tempArPenEffectChances = new List<float>();
            List<float> tempArPenEffectScales = new List<float>();

            List<SpecialEffect> critEffects = new List<SpecialEffect>(); 
            #endregion

            List<SpecialEffect> firstPass = new List<SpecialEffect>();
            List<SpecialEffect> secondPass = new List<SpecialEffect>();
            bool doubleExecutioner = false;
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                effect.Stats.GenerateSparseData();

                if (!triggerIntervals.ContainsKey(effect.Trigger)) continue;
                else if (effect.Stats.CritRating + effect.Stats.DeathbringerProc > 0f)
                {
                    critEffects.Add(effect);
                    if (effect.Stats.DeathbringerProc > 0f) secondPass.Add(effect); // for strength only
                }
                else if (effect.Stats.ArmorPenetrationRating > 0f)
                {
                    if (doubleExecutioner) continue;
                    Trigger realTrigger;
                    if (bersMainHand.Contains(effect))
                    {
                        bersMainHand.Remove(effect);
                        doubleExecutioner = true;
                        if (bersOffHand.Contains(effect))
                        {
                            bersOffHand.Remove(effect);
                            realTrigger = Trigger.MeleeHit;
                        }
                        else realTrigger = Trigger.MainHandHit;
                    }
                    else if (bersOffHand.Contains(effect))
                    {
                        bersOffHand.Remove(effect);
                        realTrigger = Trigger.OffHandHit;
                    }
                    else realTrigger = effect.Trigger;
                    tempArPenEffects.Add(effect);
                    tempArPenEffectIntervals.Add(triggerIntervals[realTrigger]);
                    tempArPenEffectChances.Add(triggerChances[realTrigger]);
                    tempArPenEffectScales.Add(1f);
                }
                else if (!bersMainHand.Contains(effect) && !bersOffHand.Contains(effect) &&
                   (effect.Stats.DeathbringerProc > 0f ||
                    effect.Stats.Agility > 0f ||
                    effect.Stats.HasteRating > 0f ||
                    effect.Stats.HitRating > 0f ||
                    effect.Stats.CritRating > 0f ||
                    effect.Stats.PhysicalHaste > 0f ||
                    effect.Stats.PhysicalCrit > 0f ||
                    effect.Stats.PhysicalHit > 0f))
                {
                    firstPass.Add(effect);
                }
                else if (!bersMainHand.Contains(effect) && !bersOffHand.Contains(effect))
                {
                    secondPass.Add(effect);
                }
            }

            if (tempArPenEffects.Count == 0)
            {
                tempArPenRatings.Add(0.0f);
                tempArPenRatingUptimes.Add(1.0f);
            }
            else if (tempArPenEffects.Count == 1)
            { //Only one, add it to
                SpecialEffect effect = tempArPenEffects[0];
                float uptime = effect.GetAverageStackSize(tempArPenEffectIntervals[0], tempArPenEffectChances[0], charStruct.combatFactors._c_mhItemSpeed, (charStruct.calcOpts.SE_UseDur ? charStruct.calcOpts.Duration : 0f)) * tempArPenEffectScales[0];
                tempArPenRatings.Add(effect.Stats.ArmorPenetrationRating);
                tempArPenRatingUptimes.Add(uptime);
                tempArPenRatings.Add(0.0f);
                tempArPenRatingUptimes.Add(1.0f - uptime);
            }
            else if (tempArPenEffects.Count > 1)
            {
                //if (tempArPenEffects.Count >= 2)
                //{
                //    offset[0] = calcOpts.TrinketOffset;
                //}
                WeightedStat[] arPenWeights = SpecialEffect.GetAverageCombinedUptimeCombinations(tempArPenEffects.ToArray(), tempArPenEffectIntervals.ToArray(), tempArPenEffectChances.ToArray(), new float[tempArPenEffectChances.Count], tempArPenEffectScales.ToArray(), charStruct.combatFactors._c_mhItemSpeed, charStruct.calcOpts.Duration, AdditiveStat.ArmorPenetrationRating);
                for (int i = 0; i < arPenWeights.Length; i++)
                {
                    tempArPenRatings.Add(arPenWeights[i].Value);
                    tempArPenRatingUptimes.Add(arPenWeights[i].Chance);
                }
            }
            // Get the average Armor Pen Rating across all procs
            if (tempArPenRatings.Count > 0f)
            {
                Stats originalStats = charStruct.combatFactors.StatS;
                int LevelDif = charStruct.calcOpts.TargetLevel - charStruct.Char.Level;

                float arpenBuffs =
                                ((charStruct.combatFactors._c_mhItemType == ItemType.TwoHandMace) ? charStruct.Char.WarriorTalents.MaceSpecialization * 0.03f : 0.00f) +
                                (!charStruct.calcOpts.FuryStance ? (0.10f + originalStats.BonusWarrior_T9_2P_ArP) : 0.0f);

                float OriginalArmorReduction = StatConversion.GetArmorDamageReduction(charStruct.Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif],
                    originalStats.ArmorPenetration, arpenBuffs, originalStats.ArmorPenetrationRating);
                float ProccedArmorReduction = 0f;
                for (int i = 0; i < tempArPenRatings.Count; i++)
                {
                    ProccedArmorReduction += tempArPenRatingUptimes[i] *
                                StatConversion.GetArmorDamageReduction(charStruct.Char.Level,
                                (int)StatConversion.NPC_ARMOR[LevelDif],
                                originalStats.ArmorPenetration, arpenBuffs,
                                originalStats.ArmorPenetrationRating + tempArPenRatings[i]);
                }
                Stats dummyStats = new Stats();
                
                float procArp = StatConversion.GetRatingFromArmorReduction(charStruct.Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif],
                    originalStats.ArmorPenetration, arpenBuffs, ProccedArmorReduction);
                originalStats.ArmorPenetrationRating += (procArp - originalStats.ArmorPenetrationRating);                
            }

            IterativeSpecialEffectsStats(charStruct, firstPass, critEffects, triggerIntervals, triggerChances, true, new Stats(), charStruct.combatFactors.StatS);
            IterativeSpecialEffectsStats(charStruct, secondPass, critEffects, triggerIntervals, triggerChances, false, null, charStruct.combatFactors.StatS);
        }

        private static void CalculateTriggers(DPSWarrCharacter charStruct, Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances)
        {
            float fightDuration = charStruct.calcOpts.Duration;
            float fightDuration2Pass = charStruct.calcOpts.SE_UseDur ? fightDuration : 0;

            float attemptedMH = charStruct.Rot.AttemptedAtksOverDurMH;
            float attemptedOH = charStruct.Rot.AttemptedAtksOverDurOH;
            float attempted = attemptedMH + attemptedOH;

            float landMH = charStruct.Rot.LandedAtksOverDurMH;
            float landOH = charStruct.Rot.LandedAtksOverDurOH;
            float land = landMH + landOH;

            float crit = charStruct.Rot.CriticalAtksOverDur;

            float avoidedAttacks = charStruct.combatFactors.StatS.Dodge + charStruct.combatFactors.StatS.Parry;

            float dwbleed = 0;
            if (charStruct.Char.WarriorTalents.DeepWounds > 0 && crit != 0f) dwbleed = fightDuration * (float)Math.Floor(fightDuration / crit) / (fightDuration / crit);

            float bleed = dwbleed + fightDuration * (charStruct.calcOpts.FuryStance || !charStruct.calcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Rend_] ? 0f : 1f / 3f);

            float bleedHitInterval = fightDuration / bleed;
            float dwbleedHitInterval = fightDuration / dwbleed;
            float attemptedAtkInterval = fightDuration / attempted;
            float attemptedAtksIntervalMH = fightDuration / attemptedMH;
            float attemptedAtksIntervalOH = fightDuration / attemptedOH;
            float landedAtksInterval = fightDuration / land;
            float dmgDoneInterval = fightDuration / (land + bleed);
            float dmgTakenInterval = fightDuration / charStruct.calcOpts.AoETargetsFreq;

            float hitRate = 1, hitRateMH = 1, hitRateOH = 1, critRate = 1;
            if (attempted != 0f)
            {
                hitRate = land / attempted;
                critRate = crit / attempted;
            }
            if (attemptedMH != 0f)
                hitRateMH = landMH / attemptedMH;
            if (attemptedOH != 0f)
                hitRateOH = landOH / attemptedOH;

            triggerIntervals[Trigger.Use] = 0f;
            triggerChances[Trigger.Use] = 1f;

            triggerIntervals[Trigger.MeleeHit] = triggerIntervals[Trigger.PhysicalHit] = attemptedAtkInterval;
            triggerChances[Trigger.MeleeHit] = triggerChances[Trigger.PhysicalHit] = hitRate;

            triggerIntervals[Trigger.PhysicalCrit] = triggerIntervals[Trigger.MeleeCrit] = attemptedAtkInterval;
            triggerChances[Trigger.PhysicalCrit] = triggerChances[Trigger.MeleeCrit] = critRate;

            triggerIntervals[Trigger.MainHandHit] = attemptedAtksIntervalMH;
            triggerChances[Trigger.MainHandHit] = hitRateMH;
            triggerIntervals[Trigger.OffHandHit] = attemptedAtksIntervalOH;
            triggerChances[Trigger.OffHandHit] = hitRateOH;

            triggerIntervals[Trigger.DoTTick] = bleedHitInterval;
            triggerChances[Trigger.DoTTick] = 1f;

            triggerIntervals[Trigger.DamageDone] = dmgDoneInterval;
            triggerChances[Trigger.DamageDone] = 1f;

            triggerIntervals[Trigger.DamageTaken] = dmgTakenInterval;
            triggerChances[Trigger.DamageTaken] = 1f;

            triggerIntervals[Trigger.DamageAvoided] = dmgTakenInterval;
            triggerChances[Trigger.DamageAvoided] = avoidedAttacks;

            triggerIntervals[Trigger.HSorSLHit] = fightDuration / charStruct.Rot.CritHsSlamOverDur;
            triggerChances[Trigger.HSorSLHit] = 1f;

            triggerIntervals[Trigger.DeepWoundsTick] = dwbleedHitInterval;
            triggerChances[Trigger.DeepWoundsTick] = 1f;
        }

        private Stats IterativeSpecialEffectsStats(DPSWarrCharacter charStruct, List<SpecialEffect> specialEffects, List<SpecialEffect> critEffects,
            Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances,
            bool iterate, Stats iterateOld, Stats originalStats) {

                WarriorTalents talents = charStruct.Char.WarriorTalents;
                float fightDuration = charStruct.calcOpts.Duration;
            Stats statsProcs = new Stats();
            try {
                //float bleedHitInterval = 1f / (calcOpts.FuryStance ? 1f : 4f / 3f); // 4/3 ticks per sec with deep wounds and rend both going, 1 tick/sec with just deep wounds
                //float attemptedAtkInterval = fightDuration / Rot.AttemptedAtksOverDur;
                //float landedAtksInterval = fightDuration / Rot.LandedAtksOverDur;
                //float dmgDoneInterval = fightDuration / (Rot.LandedAtksOverDur + (calcOpts.FuryStance ? 1f : 4f / 3f));
                float dmgTakenInterval = fightDuration / charStruct.calcOpts.AoETargetsFreq;

                float attempted = charStruct.Rot.AttemptedAtksOverDur;
                float land = charStruct.Rot.LandedAtksOverDur;
                float crit = charStruct.Rot.CriticalAtksOverDur;

                int LevelDif = charStruct.calcOpts.TargetLevel - charStruct.Char.Level;
                List<Trigger> critTriggers = new List<Trigger>();
                List<float> critWeights = new List<float>();
                bool needsHitTableReset = false;
                foreach (SpecialEffect effect in critEffects)
                {
                    needsHitTableReset = true;

                    critTriggers.Add(effect.Trigger);
                    critWeights.Add(1f / (effect.Stats.DeathbringerProc > 0f ? 3f : 1f));
                        
                }
                foreach (SpecialEffect effect in specialEffects) {
                    /*if (effect.Stats.ArmorPenetrationRating > 0) {
                        float arpenBuffs =
                            ((combatFactors._c_mhItemType == ItemType.TwoHandMace) ? talents.MaceSpecialization * 0.03f : 0.00f) +
                            (!calcOpts.FuryStance ? (0.10f + originalStats.BonusWarrior_T9_2P_ArP) : 0.0f);

                        float OriginalArmorReduction = StatConversion.GetArmorDamageReduction(Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif],
                            originalStats.ArmorPenetration, arpenBuffs, originalStats.ArmorPenetrationRating);
                        float ProccedArmorReduction = StatConversion.GetArmorDamageReduction(Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif],
                            originalStats.ArmorPenetration + effect.Stats.ArmorPenetration, arpenBuffs, originalStats.ArmorPenetrationRating + effect.Stats.ArmorPenetrationRating);

                        Stats dummyStats = new Stats();
                        float procUptime = ApplySpecialEffect(effect, Char, Rot, combatFactors, calcOpts, originalStats.Dodge + originalStats.Parry, ref dummyStats);

                        float targetReduction = ProccedArmorReduction * procUptime + OriginalArmorReduction * (1f - procUptime);
                        //float arpDiff = OriginalArmorReduction - targetReduction;
                        float procArp = StatConversion.GetRatingFromArmorReduction(Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif],
                            originalStats.ArmorPenetration, arpenBuffs, targetReduction);
                        statsProcs.ArmorPenetrationRating += (procArp - originalStats.ArmorPenetrationRating);
                    } 
                    else */
                    
                    if (effect.Stats.ManaorEquivRestore > 0f) {
                        // effect.Duration = 0, so GetAverageStats won't work
                        float numProcs = effect.GetAverageProcsPerSecond(dmgTakenInterval, originalStats.Dodge + originalStats.Parry, 0f, 0f) * fightDuration;
                        statsProcs.ManaorEquivRestore += effect.Stats.ManaorEquivRestore * numProcs;
                    } else {
                        ApplySpecialEffect(effect, charStruct, triggerIntervals, triggerChances, ref statsProcs);
                    }
                }

                WeightedStat[] critProcs;
                if (critEffects.Count == 0)
                {
                    critProcs = new WeightedStat[] { new WeightedStat() { Value = 0f, Chance = 1f } };
                }
                else if (critEffects.Count == 1)
                {
                    float interval = triggerIntervals[critEffects[0].Trigger];
                    float chance = triggerChances[critEffects[0].Trigger];
                    float upTime = critEffects[0].GetAverageStackSize(interval, chance, charStruct.combatFactors._c_mhItemSpeed, (charStruct.calcOpts.SE_UseDur ? charStruct.calcOpts.Duration : 0f));
                    upTime *= critWeights[0];
                    critProcs = new WeightedStat[] { new WeightedStat() { Value = critEffects[0].Stats.CritRating + critEffects[0].Stats.DeathbringerProc, Chance = upTime },
                                                     new WeightedStat() { Value = 0f, Chance = 1f - upTime } };
                }
                else
                {
                    float[] intervals = new float[critEffects.Count];
                    float[] chances = new float[critEffects.Count];
                    float[] offset = new float[critEffects.Count];
                    for (int i = 0; i < critEffects.Count; i++)
                    {
                        intervals[i] = triggerIntervals[critEffects[i].Trigger];
                        chances[i] = triggerChances[critEffects[i].Trigger];
                        if (critEffects[i].Stats.DeathbringerProc > 0f) critEffects[i].Stats.CritRating = critEffects[i].Stats.DeathbringerProc;
                    }

                    critProcs = SpecialEffect.GetAverageCombinedUptimeCombinations(critEffects.ToArray(), intervals, chances, offset, critWeights.ToArray(), charStruct.combatFactors._c_mhItemSpeed, charStruct.calcOpts.Duration, AdditiveStat.CritRating);
                    foreach (SpecialEffect se in critEffects)
                    {
                        if (se.Stats.DeathbringerProc > 0f) se.Stats.CritRating = 0f;
                    }
                }
                charStruct.combatFactors.critProcs = critProcs;
                charStruct.combatFactors.StatS = UpdateStatsAndAdd(statsProcs, originalStats, charStruct.Char);
                charStruct.combatFactors.InvalidateCache();
                //Rot.InvalidateCache();
                if (iterate) {
                    const float precisionWhole = 0.01f;
                    const float precisionDec = 0.0001f;
                    if (statsProcs.Agility - iterateOld.Agility > precisionWhole ||
                        statsProcs.HasteRating - iterateOld.HasteRating > precisionWhole ||
                        statsProcs.HitRating - iterateOld.HitRating > precisionWhole ||
                        statsProcs.CritRating - iterateOld.CritRating > precisionWhole ||
                        statsProcs.PhysicalHaste - iterateOld.PhysicalHaste > precisionDec ||
                        statsProcs.PhysicalCrit - iterateOld.PhysicalCrit > precisionDec ||
                        statsProcs.PhysicalHit - iterateOld.PhysicalHit > precisionDec)
                    {
                        if (needsHitTableReset) charStruct.Rot.ResetHitTables();
                        charStruct.Rot.doIterations();
                        CalculateTriggers(charStruct, triggerIntervals, triggerChances);
                        return IterativeSpecialEffectsStats(charStruct,
                            specialEffects, critEffects, triggerIntervals, triggerChances, true, statsProcs, originalStats);
                    }
                    else
                    {
                        //int j = 0;
                    }
                }

                return statsProcs;
            } catch (Exception ex) {
                ErrorBox box = new ErrorBox("Error in creating SpecialEffects Stats", ex.Message, "GetSpecialEffectsStats()");
                box.Show();
                return new Stats();
            }
        }

        private enum SpecialEffectDataType { AverageStats, UpTime };
        private float ApplySpecialEffect(SpecialEffect effect, DPSWarrCharacter charStruct, Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances, ref Stats applyTo) {
            float fightDuration = charStruct.calcOpts.Duration;
            float fightDuration2Pass = charStruct.calcOpts.SE_UseDur ? fightDuration : 0;

            Stats effectStats = effect.Stats;

            float upTime = 0f;
            //float avgStack = 1f;
            if (effect.Stats.ArmorPenetration > 0f || effect.Stats.ArmorPenetrationRating > 0f) {
                //int j = 0;
            }
            if (effect.Trigger == Trigger.Use)
            {
                    if (effect.Stats._rawSpecialEffectDataSize == 1) {
                        upTime = effect.GetAverageUptime(0f, 1f, charStruct.combatFactors._c_mhItemSpeed, fightDuration2Pass);
                        //float uptime =  (effect.Cooldown / fightDuration);
                        List<SpecialEffect> nestedEffect = new List<SpecialEffect>();
                        nestedEffect.Add(effect.Stats._rawSpecialEffectData[0]);
                        Stats _stats2 = new Stats();
                        ApplySpecialEffect(effect.Stats._rawSpecialEffectData[0], charStruct, triggerIntervals, triggerChances, ref _stats2);
                        effectStats = _stats2;
                    } else {
                        upTime = effect.GetAverageStackSize(0f, 1f, charStruct.combatFactors._c_mhItemSpeed, fightDuration2Pass); 
                    }
            }
            else if (effect.Duration == 0f)
            {
                upTime = effect.GetAverageProcsPerSecond(triggerIntervals[effect.Trigger], 
                                                         triggerChances[effect.Trigger],
                                                         charStruct.combatFactors._c_mhItemSpeed,
                                                         fightDuration2Pass);
            }
            else if (triggerIntervals.ContainsKey(effect.Trigger))
            {
                upTime = effect.GetAverageStackSize(triggerIntervals[effect.Trigger], 
                                                         triggerChances[effect.Trigger],
                                                         charStruct.combatFactors._c_mhItemSpeed,
                                                         fightDuration2Pass);
            }

            if (effect.Stats.DeathbringerProc > 0) 
                upTime /= 3;
            if (upTime > 0f) {
                if (effect.Duration == 0f)
                    applyTo.ShadowDamage = upTime;
                else if (upTime <= effect.MaxStack)
                    applyTo.Accumulate(effectStats, upTime);

                return upTime;
            }
            return 0f;
        }

        private static Stats UpdateStatsAndAdd(Stats statsToAdd, Stats baseStats, Character character)
        {
            Stats retVal;
            float newStaMult = 1f + statsToAdd.BonusStaminaMultiplier;
            float newStrMult = 1f + statsToAdd.BonusStrengthMultiplier;
            float newAgiMult = 1f + statsToAdd.BonusAgilityMultiplier;
            float newArmMult = 1f + statsToAdd.BonusArmorMultiplier;
            float newBaseArmMult = 1f + statsToAdd.BaseArmorMultiplier;
            float newAtkMult = 1f + statsToAdd.BonusAttackPowerMultiplier;
            float newHealthMult = 1f + statsToAdd.BonusHealthMultiplier;
            if (baseStats != null)
            {
                retVal = baseStats.Clone();
                
                newStaMult *= (1f + retVal.BonusStaminaMultiplier);
                newStrMult *= (1f + retVal.BonusStrengthMultiplier);
                newAgiMult *= (1f + retVal.BonusAgilityMultiplier);
                newArmMult *= (1f + retVal.BonusArmorMultiplier);
                newBaseArmMult *= (1f + retVal.BaseArmorMultiplier);
                newAtkMult *= (1f + retVal.BonusAttackPowerMultiplier);
                newHealthMult *= (1f + retVal.BonusHealthMultiplier);

                // normalize retVal with its old base stat values, since we're updating them below
                // This essentially reverses what gets done to statsToAdd, but only things that
                // are affected by multipliers (like base stats, armor, AP, etc)
                
                retVal.Health -= retVal.Stamina * 10f; // Stamina is affected by a multiplier

                // Since AP is set to (RawAP + 2*STR + A2T + BonusAP)*APMult, and Str/A2T are affected by mults too,
                // we need to rewind the Str and Armor components out.  We will add them after we've updated Str/Armor, below
                retVal.AttackPower /= 1f + retVal.BonusAttackPowerMultiplier;
                retVal.AttackPower -= (retVal.Strength * 2f) +
                                      (retVal.Armor / 108f * character.WarriorTalents.ArmoredToTheTeeth);

                // This is reversing the Armor = (Armor*BaseMult + Bonus)*BonusMult
                retVal.Armor /= 1f + retVal.BonusArmorMultiplier;
                retVal.Armor -= retVal.BonusArmor;
                retVal.Armor /= 1f + retVal.BaseArmorMultiplier;
                retVal.BonusArmor -= retVal.Agility * 2f;
                
                // Agi is multed, remove it from PhysicalCrit for now
                retVal.PhysicalCrit -= StatConversion.GetCritFromAgility(retVal.Agility, character.Class);
            }
            else
            {
                retVal = null;
            }

            // Deal with the deathbringer proc before doing anything with mults -- Crit and Arp are handled separately due to being capped
            statsToAdd.Strength += statsToAdd.DeathbringerProc;
            statsToAdd.HasteRating += statsToAdd.DeathbringerProc;

            //statsToAdd.DeathbringerProc = 0f;

            #region Base Stats
            statsToAdd.Stamina  *= newStaMult;
            statsToAdd.Strength *= newStrMult;
            statsToAdd.Agility *= newAgiMult;

            if (retVal != null)
            {
                // change retvals to use the new mults.  Combines Stat/=oldMult; Stat*=newMult
                retVal.Stamina *= newStaMult / (1f + retVal.BonusStaminaMultiplier);
                retVal.Strength *= newStrMult / (1f + retVal.BonusStrengthMultiplier);
                retVal.Agility *= newAgiMult / (1f + retVal.BonusAgilityMultiplier);
            }
            #endregion

            #region Health
            statsToAdd.Health *= newHealthMult;
            statsToAdd.Health += (statsToAdd.Stamina * 10f);
            if (retVal != null)
            {
                // Combines rollback of oldmult and addition of newmult
                retVal.Health *= newHealthMult / (1f + retVal.BonusHealthMultiplier);
                retVal.Health += retVal.Stamina * 10f;
            }
            #endregion

            #region Armor
            statsToAdd.BonusArmor += statsToAdd.Agility * 2f;
            statsToAdd.Armor = (statsToAdd.Armor * newBaseArmMult + statsToAdd.BonusArmor) * newArmMult;
            if (retVal != null)
            {
                retVal.BonusArmor += retVal.Agility * 2f;
                retVal.Armor = (retVal.Armor * newBaseArmMult + retVal.BonusArmor) * newArmMult;
            }
            #endregion

            #region Attack Power
            // stats to add
            statsToAdd.AttackPower += (statsToAdd.Strength * 2f) +
                                  (statsToAdd.Armor / 108f * character.WarriorTalents.ArmoredToTheTeeth) +
                                  statsToAdd.BonusAttackPower;
            statsToAdd.AttackPower *= newAtkMult;
            // reset retval
            if (retVal != null)
            {
                // already rolled back AP's oldmult, so not combining
                retVal.AttackPower += (retVal.Strength * 2f) +
                                  (retVal.Armor / 108f * character.WarriorTalents.ArmoredToTheTeeth);
                retVal.AttackPower *= newAtkMult;
            }
            #endregion

            // Crit
            statsToAdd.PhysicalCrit += StatConversion.GetCritFromAgility(statsToAdd.Agility, character.Class);
            statsToAdd.PhysicalCrit += StatConversion.GetCritFromRating(statsToAdd.CritRating, character.Class);
            if (retVal != null)
            {
                retVal.PhysicalCrit += StatConversion.GetCritFromAgility(retVal.Agility, character.Class);
            }
            // Haste
            statsToAdd.PhysicalHaste = (1f + statsToAdd.PhysicalHaste)
                                     * (1f + StatConversion.GetPhysicalHasteFromRating(Math.Max(0, statsToAdd.HasteRating), character.Class))
                                     - 1f;

            // If we're adding two, then return the .Accumulate
            if (retVal != null) {
                retVal.Accumulate(statsToAdd);

                // Paragon and its friends
                if (retVal.Paragon > 0f || retVal.HighestStat > 0f) {
                    float paragonValue = retVal.Paragon + retVal.HighestStat; // how much paragon to add
                    retVal.Paragon = retVal.HighestStat = 0f; // remove Paragon stat, since it's not needed
                    if (retVal.Strength > retVal.Agility) // Now that we've added the two stats, we run UpdateStatsAndAdd again for paragon
                    {
                        return UpdateStatsAndAdd(new Stats { Strength = paragonValue }, retVal, character);
                    } else {
                        return UpdateStatsAndAdd(new Stats { Agility = paragonValue }, retVal, character);
                    }
                } else {
                    return retVal;
                }
            } else { // Just processing one, not adding two
                return statsToAdd;
            }
        }

        #endregion
    }
}
