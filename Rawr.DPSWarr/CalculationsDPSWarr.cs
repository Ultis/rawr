using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
#if RAWR3
using System.Windows.Media;
#else
using System.Drawing;
#endif

namespace Rawr.DPSWarr {
    public class ErrorBoxDPSWarr {
        /// <summary>
        /// Generates a pop-up message box with error info. This constructor does not show the box automatically, it must be called.
        /// </summary>
        public ErrorBoxDPSWarr() {
            Title       = "There was an Error";
            Message     = "No Message";
            Function    = "No Function Name";
            StackTrace  = "No Stack Trace";
            Info        = "No Additional Info";
            Line        = 0;
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        /// <param name="stacktrace">The Stack Trace leading to this point</param>
        /// <param name="line">The line of the Function throwing the Error</param>
        public ErrorBoxDPSWarr(string title, string message, string function, string info, string stacktrace, int line) {
            Title = title;
            Message = message;
            Function = function;
            StackTrace = stacktrace;
            Info = info;
            Line = line;
            Show();
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="info">Additional info pertaining to the current action</param>
        /// <param name="line">The line of the Function throwing the Error</param>
        public ErrorBoxDPSWarr(string title, string message, string function,string info, int line) {
            Title = title;
            Message = message;
            Function = function;
            StackTrace = "No Stack Trace";
            Info = info;
            Line = line;
            Show();
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        /// <param name="line">The line of the Function throwing the Error</param>
        public ErrorBoxDPSWarr(string title, string message, string function, int line) {
            Title = title;
            Message = message;
            Function = function;
            StackTrace = "No Stack Trace";
            Info = "No Additional Info";
            Line = line;
            Show();
        }
        /// <summary>
        /// Generates a pop-up message box with error info.
        /// </summary>
        /// <param name="title">The Title, which appears on the Title bar</param>
        /// <param name="message">The Error Message itself</param>
        /// <param name="function">The Function throwing this Error</param>
        public ErrorBoxDPSWarr(string title, string message, string function) {
            Title = title;
            Message = message;
            Function = function;
            StackTrace = "No Stack Trace";
            Info = "No Additional Info";
            Line = 0;
            Show();
        }
        public string Title;
        public string Message;
        public string Function;
        public string Info;
        public string StackTrace;
        public int Line;
        private string buildFullMessage() {
            string retVal = "";
            retVal += Function + " Line: " + Line.ToString();
            retVal += "\r\n\r\n";
            retVal += "Error Message: " + Message;
            retVal += "\r\n\r\n";
            retVal += "Info: " + Info;
            retVal += "\r\n\r\n";
            retVal += "Stack Trace:\r\n" + StackTrace;
            return retVal;
        }
        public void Show(){
            #if RAWR3
                //new ErrorWindow() { Message = Title + "\r\n\r\n" + buildFullMessage() }.Show();
            #else
                System.Windows.Forms.MessageBox.Show(buildFullMessage(),Title);
            #endif
            Console.WriteLine(Title + "\n" + buildFullMessage());
        }
    }
    [Rawr.Calculations.RawrModelInfo("DPSWarr", "Ability_Rogue_Ambush", CharacterClass.Warrior)]
    public class CalculationsDPSWarr : CalculationsBase {
        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
				///Relevant Gem IDs for DPSWarrs
                //                  uncom  rare    epic  jewel
				//Red
				int[] bold      = { 39900, 39996, 40111, 42142 };
                int[] frac      = { 39909, 40002, 40117, 42153 };
				//Purple
				int[] svrn      = { 39934, 40022, 40129 };
                int[] pusn      = { 39933, 40033, 40140 };
				//Orange
				int[] insc      = { 39947, 40037, 40142 };
				//Meta
				int chaotic     = 41285;
                int relent      = 41398;

				return new List<GemmingTemplate>() {
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon",              RedId = bold[0], YellowId = insc[0], BlueId = svrn[0], PrismaticId = bold[0], MetaId = chaotic },// STR
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon",              RedId = bold[0], YellowId = bold[0], BlueId = bold[0], PrismaticId = bold[0], MetaId = chaotic },// Max STR
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon",              RedId = bold[0], YellowId = bold[0], BlueId = bold[0], PrismaticId = bold[0], MetaId = relent  },// Max STR with All red Meta
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon",              RedId = frac[0], YellowId = insc[0], BlueId = pusn[0], PrismaticId = frac[0], MetaId = chaotic },// ArP
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon",              RedId = frac[0], YellowId = frac[0], BlueId = frac[0], PrismaticId = frac[0], MetaId = chaotic },// Max ArP
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon",              RedId = frac[0], YellowId = frac[0], BlueId = frac[0], PrismaticId = frac[0], MetaId = relent  },// Max ArP with All red Meta
                    
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
				};
            }
        }

        #region Variables and Properties

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
1232-None
1109-Arms(123)
0924-Arms(123)+Mace(185)",
                        @"Base Stats:Hit*8.00% chance to miss base for Yellow Attacks
Precision 0- 8%-0%=8%=262 Rating soft cap
Precision 1- 8%-1%=7%=230 Rating soft cap
Precision 2- 8%-2%=6%=197 Rating soft cap
Precision 3- 8%-3%=5%=164 Rating soft cap",
                        @"Base Stats:Expertise*6.50% chance to dodge/parry for attacks
Weapon Mastery 0- 6.50%-0%=6.50%=214 Rating Cap
Weapon Mastery 1- 6.50%-1%=6.50%=181 Rating Cap
Weapon Mastery 2- 6.50%-2%=6.50%=148 Rating Cap

Don't forget your weapons used matched with races can affect these numbers.",
                        
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
                        "DPS Breakdown (Arms):Taste for Blood*Does an Overpower",
                        "DPS Breakdown (Arms):Sudden Death*Does a limited Execute",
                        "DPS Breakdown (Arms):Slam*If this number is zero, it most likely means that your other abilities are proc'g often enough that you are rarely, if ever, having to resort to Slamming your target.",
                        "DPS Breakdown (Arms):Sword Spec",

                        "DPS Breakdown (Maintenance):Thunder Clap",
                        "DPS Breakdown (Maintenance):Shattering Throw",

                        "DPS Breakdown (General):Deep Wounds",
                        "DPS Breakdown (General):Heroic Strike",
                        "DPS Breakdown (General):Cleave",
                        "DPS Breakdown (General):White DPS",
                        "DPS Breakdown (General):Execute*<20% Spamming only",
                        @"DPS Breakdown (General):Total DPS*1st number is total DPS
2nd number is total DMG over Duration",
                      
                        "Rage Details:Total Generated Rage",
                        "Rage Details:Needed Rage for Abilities",
                        "Rage Details:Available Free Rage*For Heroic Strikes and Cleaves",
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
					};
                return _optimizableCalculationLabels;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("DPS", Color.FromArgb(255,255,0,0));
#if RAWR3
                    _subPointNameColors.Add("Survivability", Color.FromArgb(255, 64, 128, 32));
#else
                    _subPointNameColors.Add("Survivability", Color.FromArgb(255, 0, 128, 0));
#endif
                }
                return _subPointNameColors;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationsDPSWarr(); }
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
                && slot == ItemSlot.OffHand)
            {
                    return true;
            }else if (character != null
                && character.WarriorTalents.TitansGrip == 0
                && (enchant.Slot == ItemSlot.TwoHand || enchant.Slot == ItemSlot.OneHand)
                && slot == ItemSlot.OffHand)
            {
                return false;
            }
            // If all the above is ok, return base version
            return enchant.FitsInSlot(slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique) {
            // We need specilized handling due to Titan's Grip
            if (item == null || character == null) {
                return false;
            } else if (character.WarriorTalents.TitansGrip > 0 && item.Type == ItemType.Polearm && slot == CharacterSlot.OffHand) {
                return false;
            } else if (character.WarriorTalents.TitansGrip > 0 && item.Slot == ItemSlot.TwoHand && slot == CharacterSlot.OffHand) {
                return true;
            } else if (slot == CharacterSlot.OffHand && character.MainHand != null && character.MainHand.Slot == ItemSlot.TwoHand) {
                return false;
            } else if (item.Type == ItemType.Polearm && slot == CharacterSlot.MainHand) {
                return true;
            } else {
                return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
            }
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
                    "Glyph of Vigilance",
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
                    "Glyph of Taunt",*/
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

        private static bool _HidingBadStuff = true;
        internal static bool HidingBadStuff { get { return _HidingBadStuff; } set { _HidingBadStuff = value; } }

        public override Stats GetRelevantStats(Stats stats) {
            Stats relevantStats = new Stats() {
                // Base Stats
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                Health = stats.Health,
                Armor = stats.Armor,
                // Ratings
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
				ArmorPenetrationRating = stats.ArmorPenetrationRating,
                ArcaneDamage = stats.ArcaneDamage,
                // Bonuses
                BonusArmor = stats.BonusArmor,
                WeaponDamage = stats.WeaponDamage,
                ArmorPenetration = stats.ArmorPenetration,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                MovementSpeed = stats.MovementSpeed,
                StunDurReduc = stats.StunDurReduc,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                // Procs
                DarkmoonCardDeathProc = stats.DarkmoonCardDeathProc,
                HighestStat = stats.HighestStat,
                Paragon = stats.Paragon,
                ManaorEquivRestore = stats.ManaorEquivRestore,
                // Multipliers
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
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
            };
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if ((effect.Trigger == Trigger.Use ||
                     effect.Trigger == Trigger.MeleeCrit ||
                     effect.Trigger == Trigger.MeleeHit ||
                     effect.Trigger == Trigger.PhysicalCrit ||
                     effect.Trigger == Trigger.PhysicalHit ||
                     effect.Trigger == Trigger.DoTTick ||
                     effect.Trigger == Trigger.DamageDone ||
                     effect.Trigger == Trigger.DamageTaken ||
                     effect.Trigger == Trigger.HSorSLHit)
                    && HasRelevantStats(effect.Stats))
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
                stats.ArcaneDamage +
                stats.SpellHit + // used for TClap/Demo Shout maintenance
                // Bonuses
                stats.BonusArmor +
                stats.WeaponDamage +
                stats.ArmorPenetration +
                stats.PhysicalCrit +
                stats.PhysicalHaste +
                stats.PhysicalHit +
                stats.MovementSpeed +
                stats.StunDurReduc +
                stats.SnareRootDurReduc +
                stats.FearDurReduc +
                // Procs
                stats.DarkmoonCardDeathProc +
                stats.HighestStat +
                stats.Paragon +
                stats.ManaorEquivRestore +
                // Multipliers
                stats.BonusAgilityMultiplier +
                stats.BonusStrengthMultiplier +
                stats.BonusAttackPowerMultiplier +
                stats.BonusBleedDamageMultiplier +
                stats.BonusDamageMultiplier +
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
                stats.BonusWarrior_T9_4P_SLHSCritIncrease
                ) != 0;
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (effect.Trigger == Trigger.Use ||
                    effect.Trigger == Trigger.MeleeCrit ||
                    effect.Trigger == Trigger.MeleeHit ||
                    effect.Trigger == Trigger.PhysicalCrit ||
                    effect.Trigger == Trigger.PhysicalHit ||
                    effect.Trigger == Trigger.DoTTick ||
                    effect.Trigger == Trigger.DamageDone ||
                    effect.Trigger == Trigger.DamageTaken ||
                    effect.Trigger == Trigger.DamageAvoided ||
                    effect.Trigger == Trigger.HSorSLHit)
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            return relevant;
        }

        private bool HasSurvivabilityStats(Stats stats) {
            bool retVal = false;
            if (( stats.Health
                + stats.Stamina
                + stats.BonusHealthMultiplier
                + stats.BonusStaminaMultiplier
                ) > 0) {
                retVal = true;
            }
            return retVal;
        }

        private bool HasIgnoreStats(Stats stats) {
            if (!HidingBadStuff) { return false; }
            return (
                // Remove Spellcasting Stuff
                stats.Mp5 + stats.SpellPower + stats.Mana + stats.Spirit + stats.Intellect
                + stats.BonusSpiritMultiplier + stats.BonusIntellectMultiplier
                + stats.SpellPenetration + stats.BonusManaMultiplier
                // Remove Defensive Stuff (until we do that special modelling)
                + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry
                + stats.DodgeRating + stats.ParryRating + stats.BlockRating + stats.Block +
                // Remove PvP Items
                stats.Resilience
                ) > 0;
        }

        public override bool IsItemRelevant(Item item) {
            if ( // Manual override for +X to all Stats gems
                   item.Name == "Nightmare Tear"
                || item.Name == "Enchanted Tear"
                || item.Name == "Enchanted Pearl"
                || item.Name == "Chromatic Sphere"
                ){
                return true;
            }else{
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
            if (name.Contains("Potion of Wild Magic")) {
                return true;
            }
            // Force some buffs to go away
            else if(name.Contains("Malorne")
                 || name.Contains("Duskweaver")
                 || name.Contains("Primalstrike")
                 || name.Contains("Clefthoof")
                 || name.Contains("Dreamwalker")
                 || name.Contains("Skyshatter")
            ) {
                return false;
            }
            bool haswantedStats = HasWantedStats(buff.Stats);
            bool hassurvStats = HasSurvivabilityStats(buff.Stats);
            bool hasbadstats = HasIgnoreStats(buff.Stats);
            bool retVal = haswantedStats || (hassurvStats && !hasbadstats);
            return retVal;
        }

        public override bool IsEnchantRelevant(Enchant enchant) { return HasWantedStats(enchant.Stats) || (HasSurvivabilityStats(enchant.Stats) && !HasIgnoreStats(enchant.Stats)); }

        public Stats GetBuffsStats(Character character) {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;

            // Removes the Battle Shout & Commanding Presence Buffs if you are maintaining it yourself
            // Also removes their equivalent of Blessing of Might (+Improved)
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.BattleShout_]) {
                float hasRelevantBuff = character.WarriorTalents.BoomingVoice +
                                        character.WarriorTalents.CommandingPresence;
                Buff a = Buff.GetBuffByName("Commanding Presence (Attack Power)");
                Buff b = Buff.GetBuffByName("Battle Shout");
                Buff c = Buff.GetBuffByName("Improved Blessing of Might");
                Buff d = Buff.GetBuffByName("Blessing of Might");
                if(hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); }
                    if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); }
                }
            }

            // Removes the Commanding Shout & Commanding Presence Buffs if you are maintaining it yourself
            // Also removes their equivalent of Blood Pact (+Improved Imp)
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.CommandingShout_]) {
                float hasRelevantBuff = character.WarriorTalents.BoomingVoice +
                                        character.WarriorTalents.CommandingPresence;
                Buff a = Buff.GetBuffByName("Commanding Presence (Health)");
                Buff b = Buff.GetBuffByName("Commanding Shout");
                Buff c = Buff.GetBuffByName("Improved Imp");
                Buff d = Buff.GetBuffByName("Blood Pact");
                if(hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); }
                    if (character.ActiveBuffs.Contains(c)) { character.ActiveBuffs.Remove(c); }
                    if (character.ActiveBuffs.Contains(d)) { character.ActiveBuffs.Remove(d); }
                }
            }

            // Removes the Trauma Buff and it's equivalent Mangle if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            {
                float hasRelevantBuff = character.WarriorTalents.Trauma;
                Buff a = Buff.GetBuffByName("Trauma");
                Buff b = Buff.GetBuffByName("Mangle");
                if(hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); }
                }
            }

            // Removes the Blood Frenzy Buff and it's equivalent of Savage Combat if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            {
                float hasRelevantBuff = character.WarriorTalents.BloodFrenzy;
                Buff a = Buff.GetBuffByName("Blood Frenzy");
                Buff b = Buff.GetBuffByName("Savage Combat");
                if (hasRelevantBuff > 0) {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); }
                }
            }

            // Removes the Rampage Buff and it's equivalent of Leader of the Pack if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if(calcOpts.FuryStance){
                float hasRelevantBuff = character.WarriorTalents.Rampage;
                Buff a = Buff.GetBuffByName("Rampage");
                Buff b = Buff.GetBuffByName("Leader of the Pack");
                if (hasRelevantBuff == 1) {
                    if (character.ActiveBuffs.Contains(a)) { character.ActiveBuffs.Remove(a); }
                    if (character.ActiveBuffs.Contains(b)) { character.ActiveBuffs.Remove(b); }
                }
            }

            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            return statsBuffs;
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
            if (doit) { character.ActiveBuffs.Add(Buff.GetBuffByName("Sunder Armor")); }*/
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
                    };
                }
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            CharacterCalculationsDPSWarr calculations = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr;

            switch (chartName) {
                #region Ability DPS+Damage
                case "Ability DPS+Damage":
                    {
                        List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                        if(calculations.Rot.GetType() == typeof(FuryRotation)) {
                            #region Fury
                            FuryRotation fr = (FuryRotation)calculations.Rot;
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Bloodsurge";
                                comparison.DPSPoints = fr._BS_DPS;
                                comparison.SurvPoints = fr.BS.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Bloodthirst";
                                comparison.DPSPoints = fr._BT_DPS;
                                comparison.SurvPoints = fr.BT.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Whirlwind";
                                comparison.DPSPoints = fr._WW_DPS;
                                comparison.SurvPoints = fr.WW.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            #endregion
                        } else if (calculations.Rot.GetType() == typeof(ArmsRotation)) {
                            #region Arms
                            ArmsRotation ar = (ArmsRotation)calculations.Rot;
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Bladestorm";
                                comparison.DPSPoints = ar._BLS_DPS;
                                comparison.SurvPoints = ar.BLS.DamageOnUse / 6f;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Mortal Strike";
                                comparison.DPSPoints = ar._MS_DPS;
                                comparison.SurvPoints = ar.MS.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Rend";
                                comparison.DPSPoints = ar._RD_DPS;
                                comparison.SurvPoints = ar.RD.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Overpower";
                                comparison.DPSPoints = ar._OP_DPS;
                                comparison.SurvPoints = ar.OP.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Taste for Blood";
                                comparison.DPSPoints = ar._TB_DPS;
                                comparison.SurvPoints = ar.TB.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Sudden Death";
                                comparison.DPSPoints = ar._SD_DPS;
                                comparison.SurvPoints = ar.SD.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Slam";
                                comparison.DPSPoints = ar._SL_DPS;
                                comparison.SurvPoints = ar.SL.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            {
                                ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                                comparison.Name = "Sword Spec";
                                comparison.DPSPoints = ar._SS_DPS;
                                comparison.SurvPoints = ar.SS.DamageOnUse;
                                comparisons.Add(comparison);
                            }
                            #endregion
                        }
                        #region Maintenance
                        {
                            ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                            comparison.Name = "Thunder Clap";
                            comparison.DPSPoints = calculations.Rot._TH_DPS;
                            comparison.SurvPoints = calculations.Rot.TH.DamageOnUse;
                            comparisons.Add(comparison);
                        }
                        {
                            ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                            comparison.Name = "Shattering Throw";
                            comparison.DPSPoints = calculations.Rot._Shatt_DPS;
                            comparison.SurvPoints = calculations.Rot.ST.DamageOnUse;
                            comparisons.Add(comparison);
                        }
                        #endregion
                        #region Generics
                        {
                            ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                            comparison.Name = "Deep Wounds";
                            comparison.DPSPoints = calculations.Rot._DW_DPS;
                            comparison.SurvPoints = calculations.Rot.DW.TickSize;
                            comparisons.Add(comparison);
                        }
                        {
                            ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                            comparison.Name = "Heroic Strike";
                            comparison.DPSPoints = calculations.HS.DPS;
                            comparison.SurvPoints = calculations.HS.DamageOnUse;
                            comparisons.Add(comparison);
                        }
                        {
                            ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                            comparison.Name = "Cleave";
                            comparison.DPSPoints = calculations.CL.DPS;
                            comparison.SurvPoints = calculations.CL.DamageOnUse;
                            comparisons.Add(comparison);
                        }
                        {
                            ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                            comparison.Name = "White Attacks";
                            comparison.DPSPoints = calculations.WhiteDPS;
                            comparison.SurvPoints = calculations.Whites.MhDamageOnUse
                                                  + calculations.Whites.OhDamageOnUse;
                            comparisons.Add(comparison);
                        }
                        {
                            ComparisonCalculationsDPSWarr comparison = new ComparisonCalculationsDPSWarr();
                            comparison.Name = "Execute";
                            comparison.DPSPoints = calculations.Rot._EX_DPS;
                            comparison.SurvPoints = calculations.Rot.EX.DamageOnUse;
                            comparisons.Add(comparison);
                        }
                        #endregion
                        foreach(ComparisonCalculationsDPSWarr comp in comparisons){
                            comp.OverallPoints = comp.DPSPoints + comp.SurvPoints;
                        }
                        return comparisons.ToArray();
                    }
                #endregion
                default: { return new ComparisonCalculationBase[0]; }
            }
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) {
            int line = 0;
            CharacterCalculationsDPSWarr calculatedStats = new CharacterCalculationsDPSWarr();
            try {
                CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr; line++;
                Stats stats = GetCharacterStats(character, additionalItem); line++;
                WarriorTalents talents = character.WarriorTalents; line++;

                CombatFactors combatFactors = new CombatFactors(character, stats); line++;
                Skills.WhiteAttacks whiteAttacks = new Skills.WhiteAttacks(character, stats, combatFactors); line++;
                Rotation Rot; line++;
                if (calcOpts.FuryStance) Rot = new FuryRotation(character, stats, combatFactors, whiteAttacks);
                else Rot = new ArmsRotation(character, stats, combatFactors, whiteAttacks); line++;
                Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race); line++;

                calculatedStats.Duration = calcOpts.Duration; line++;
                calculatedStats.BasicStats = stats; line++;
                calculatedStats.combatFactors = combatFactors; line++;
                calculatedStats.Rot = Rot; line++;
                calculatedStats.TargetLevel = calcOpts.TargetLevel; line++;
                calculatedStats.BaseHealth = statsRace.Health; line++;
                {// == Attack Table ==
                    // Miss
                    calculatedStats.Miss = stats.Miss;
                    calculatedStats.HitRating = stats.HitRating;
                    calculatedStats.ExpertiseRating = stats.ExpertiseRating;
                    calculatedStats.Expertise = StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Warrior);
                    calculatedStats.MhExpertise = combatFactors._c_mhexpertise;
                    calculatedStats.OhExpertise = combatFactors._c_ohexpertise;
                    calculatedStats.WeapMastPerc = character.WarriorTalents.WeaponMastery / 100f;
                    calculatedStats.AgilityCritBonus = StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Warrior);
                    calculatedStats.CritRating = stats.CritRating;
                    calculatedStats.CritPercent = StatConversion.GetCritFromRating(stats.CritRating) + stats.PhysicalCrit;
                    calculatedStats.MhCrit = combatFactors._c_mhycrit;
                    calculatedStats.OhCrit = combatFactors._c_ohycrit;
                } line++;
                // Offensive
                float teethbonus = stats.Armor;
                teethbonus *= (float)character.WarriorTalents.ArmoredToTheTeeth;
                teethbonus /= 108f;
                calculatedStats.TeethBonus = (int)teethbonus;
                calculatedStats.ArmorPenetrationMaceSpec = ((character.MainHand != null && combatFactors._c_mhItemType == ItemType.TwoHandMace) ? character.WarriorTalents.MaceSpecialization * 0.03f : 0.00f);
                calculatedStats.ArmorPenetrationStance = ((!calcOpts.FuryStance) ? (0.10f + stats.BonusWarrior_T9_2P_ArP) : 0.00f);
                calculatedStats.ArmorPenetrationRating = stats.ArmorPenetrationRating;
                calculatedStats.ArmorPenetrationRating2Perc = StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating);
                calculatedStats.ArmorPenetration = calculatedStats.ArmorPenetrationMaceSpec
                    + calculatedStats.ArmorPenetrationStance
                    + calculatedStats.ArmorPenetrationRating2Perc;
                calculatedStats.HasteRating = stats.HasteRating;
                calculatedStats.HastePercent = stats.PhysicalHaste; //talents.BloodFrenzy * (0.05f) + StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Warrior);
                line++;
                // DPS

                Rot.Initialize(calculatedStats);
                line++;
                // Neutral
                // Defensive
                calculatedStats.Armor = (int)stats.Armor; line++;

                calculatedStats.floorstring = calcOpts.AllowFlooring ? "000" : "000.00"; line++;

                Rot.MakeRotationandDoDPS(true); line++;

                float Health2Surv = stats.Health / 100f; line++;
                calculatedStats.TotalHPS = Rot._HPS_TTL; line++;
                calculatedStats.Survivability = (calculatedStats.TotalHPS + Health2Surv) * calcOpts.SurvScale; line++;
                calculatedStats.OverallPoints = calculatedStats.TotalDPS + calculatedStats.Survivability; line++;
            }catch (Exception ex){
                new ErrorBoxDPSWarr("Error in creating Stat Pane Calculations", ex.Message, "GetCharacterCalculations()", line);
            }
            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem) {
            try {
                CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
                WarriorTalents talents = character.WarriorTalents;

                Stats statsRace  = BaseStats.GetBaseStats(character.Level, CharacterClass.Warrior, character.Race);
                Stats statsBuffs = GetBuffsStats(character);
                Stats statsItems = GetItemStats(character, additionalItem);
                Stats statsOptionsPanel = new Stats() {
                    BonusStrengthMultiplier = (calcOpts.FuryStance ? talents.ImprovedBerserkerStance * 0.04f : 0f),
                    PhysicalCrit = (calcOpts.FuryStance ? 0.03f + statsBuffs.BonusWarrior_T9_2P_Crit : 0f)
                                // handle boss level difference
                                + StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel - character.Level],
                };
                Stats statsTalents = new Stats() {
                    // Offensive
                    BonusDamageMultiplier = (character.MainHand == null ? 0f :
                                                /* One Handed Weapon Spec  Not using this to prevent any misconceptions
                                                ((character.MainHand.Slot == ItemSlot.OneHand) ? 1f + talents.OneHandedWeaponSpecialization * 0.02f : 1f)
                                                * */
                                                // Two Handed Weapon Spec
                                                ((character.MainHand.Slot == ItemSlot.TwoHand) ? 1f + talents.TwoHandedWeaponSpecialization * 0.02f : 1f)
                                                *
                                                // Titan's Grip Penalty
                                                ((talents.TitansGrip > 0 && character.OffHand != null && (character.OffHand.Slot  == ItemSlot.TwoHand || character.MainHand.Slot == ItemSlot.TwoHand) ? 0.90f : 1f))
                                                // Convert it back a simple mod number
                                                - 1f
                                             ),
                    BonusPhysicalDamageMultiplier = (
                                                        calcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Rend_] // Have Rend up
                                                        || talents.DeepWounds > 0 // Have Deep Wounds
                                                    ? talents.BloodFrenzy * 0.02f : 0f),
                    PhysicalCrit = talents.Cruelty * 0.01f,
                    BonusStaminaMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                    BonusStrengthMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                    Expertise = talents.Vitality * 2.0f + talents.StrengthOfArms * 2.0f,
                    PhysicalHit = talents.Precision * 0.01f,
                    PhysicalHaste = talents.BloodFrenzy * 0.05f,
                    StunDurReduc = (float)Math.Ceiling(20f / 3f * talents.IronWill) / 100f,
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
                if (talents.Rampage > 0) {
                    /*SpecialEffect rampage = new SpecialEffect(Trigger.MeleeCrit, new Stats() { PhysicalCrit = 0.05f, }, 10, 0);
                    statsTalents.AddSpecialEffect(rampage);*/
                    statsTalents.PhysicalCrit += 0.05f;
                }
                if (talents.WreckingCrew > 0) {
                    float value = talents.WreckingCrew * 0.02f;
                    SpecialEffect wrecking = new SpecialEffect(Trigger.MeleeCrit, new Stats() { BonusDamageMultiplier = value, }, 12, 0);
                    statsTalents.AddSpecialEffect(wrecking);
                }
                if (talents.Trauma > 0 && character.MainHand != null) {
                    float value = talents.Trauma * 0.15f;
                    SpecialEffect trauma = new SpecialEffect(Trigger.MeleeCrit, new Stats() { BonusBleedDamageMultiplier = value, }, 15, 0);
                    statsTalents.AddSpecialEffect(trauma);
                }
                if (talents.DeathWish > 0 && calcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DeathWish_]) {
                    SpecialEffect death = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusDamageMultiplier = 0.20f, DamageTakenMultiplier = 0.05f, },
                        30f, 3f * 60f * (1f - 1f / 9f * talents.IntensifyRage));
                    statsTalents.AddSpecialEffect(death);
                }

                Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
                Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents + statsOptionsPanel;
                Stats statsProcs = new Stats();

                // Stamina
                float totalBSTAM = statsTotal.BonusStaminaMultiplier;
                float staBase    = (float)Math.Floor((1f + totalBSTAM) * statsRace.Stamina             );
                float staBonus   = (float)Math.Floor((1f + totalBSTAM) * statsGearEnchantsBuffs.Stamina);
                statsTotal.Stamina = staBase + staBonus;
                
                // Health
                statsTotal.Health += StatConversion.GetHealthFromStamina(statsTotal.Stamina);
                statsTotal.Health *= 1f + statsTotal.BonusHealthMultiplier;

                // Strength
                float totalBSM = statsTotal.BonusStrengthMultiplier;
                float strBase  = (float)Math.Floor((1f + totalBSM) * statsRace.Strength             );
                float strBonus = (float)Math.Floor((1f + totalBSM) * statsGearEnchantsBuffs.Strength);
                statsTotal.Strength = strBase + strBonus;

                // Agility
                float totalBAM = statsTotal.BonusAgilityMultiplier;
                float agiBase  = (float)Math.Floor((1f + totalBAM) * statsRace.Agility);
                float agiBonus = (float)Math.Floor((1f + totalBAM) * statsGearEnchantsBuffs.Agility);
                statsTotal.Agility = agiBase + agiBonus;
                
                // Armor
                statsTotal.Armor       = (float)Math.Floor(statsTotal.Armor      * (1f + statsTotal.BaseArmorMultiplier ));
                statsTotal.BonusArmor += statsTotal.Agility * 2f;
                statsTotal.BonusArmor  = (float)Math.Floor(statsTotal.BonusArmor * (1f + statsTotal.BonusArmorMultiplier));
                statsTotal.Armor      += statsTotal.BonusArmor;

                // Attack Power
                float totalBAPM = statsTotal.BonusAttackPowerMultiplier;
                float apBase        = (1f + totalBAPM) * (statsRace.AttackPower                                );
                float apBonusSTR    = (1f + totalBAPM) * (statsTotal.Strength * 2f                             );
                float apBonusAttT   = (1f + totalBAPM) * ((statsTotal.Armor / 108f) * talents.ArmoredToTheTeeth);
                float apBonusOther  = (1f + totalBAPM) * (statsGearEnchantsBuffs.AttackPower                   );
                statsTotal.AttackPower = (float)Math.Floor(apBase + apBonusSTR + apBonusAttT + apBonusOther);

                // Crit
                statsTotal.PhysicalCrit += StatConversion.GetCritFromAgility(statsTotal.Agility, character.Class);

                // Haste
                statsTotal.HasteRating = (float)Math.Floor(statsTotal.HasteRating);
                float ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, character.Class);
                statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                           (1f + statsItems.PhysicalHaste) *
                                           (1f + statsBuffs.PhysicalHaste) *
                                           (1f + statsTalents.PhysicalHaste) *
                                           (1f + statsOptionsPanel.PhysicalHaste) *
                                           //(1f + statsProcs.PhysicalHaste) *
                                           (1f + ratingHasteBonus)
                                           - 1f;

                // SpecialEffects: Supposed to handle all procs such as Berserking, Mirror of Truth, Grim Toll, etc.
                CombatFactors combatFactors = new CombatFactors(character, statsTotal);
                Skills.WhiteAttacks whiteAttacks = new Skills.WhiteAttacks(character, statsTotal, combatFactors);
                Rotation Rot;
                if (calcOpts.FuryStance) Rot = new FuryRotation(character, statsTotal, combatFactors, whiteAttacks);
                else Rot = new ArmsRotation(character, statsTotal, combatFactors, whiteAttacks);
                Rot.Initialize();
                Rot.MakeRotationandDoDPS(false);

                // Add some last minute SpecialEffects
                if (Rot.ST.Validated) {
                    SpecialEffect shatt = new SpecialEffect(Trigger.Use,
                        new Stats() { ArmorPenetration = 0.20f, },
                        Rot.ST.Duration, Rot.ST.Cd,
                        Rot.ST.MHAtkTable.AnyLand);
                    statsTotal.AddSpecialEffect(shatt);
                }
                if (Rot.BR.Validated) {
                    SpecialEffect blood = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusRageGen = 1f * (1f + talents.ImprovedBloodrage * 0.25f), },
                        Rot.BR.Duration, Rot.BR.Cd);
                    statsTotal.AddSpecialEffect(blood);
                }
                /*if (Rot.Hammy.Validated) {
                    SpecialEffect hammy = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusTargets = 1f * calcOpts.MultipleTargetsPerc / 100f, },
                        Rot.SW.Duration, Rot.SW.Cd);
                    statsTotal.AddSpecialEffect(hammy);
                }*/
                if (Rot.BTS.Validated) {
                    SpecialEffect bs = new SpecialEffect(Trigger.Use,
                        new Stats() { AttackPower = (548f * (1f + talents.CommandingPresence * 0.05f)), },
                        Rot.BTS.Duration, Rot.BTS.Cd+0.01f);
                    statsTotal.AddSpecialEffect(bs);
                }
                if (Rot.CS.Validated) {
                    float value = (2255f * (1f + talents.CommandingPresence * 0.05f));
                    SpecialEffect cs = new SpecialEffect(Trigger.Use,
                        new Stats() { Health = value, },
                        Rot.CS.Duration, Rot.CS.Cd+0.01f);
                    statsTotal.AddSpecialEffect(cs);
                }
                
                float fightDuration = calcOpts.Duration;

                bool useOH = combatFactors.useOH;

                float bleedHitInterval = 1f / (calcOpts.FuryStance ? 1f : 4f / 3f); // 4/3 ticks per sec with deep wounds and rend both going, 1 tick/sec with just deep wounds
                float attemptedAtksInterval = fightDuration / Rot.GetAttemptedAtksOverDur();
                float landedAtksInterval = fightDuration / Rot.GetLandedAtksOverDur();
                float dmgDoneInterval = fightDuration / (Rot.GetLandedAtksOverDur() + (calcOpts.FuryStance ? 1f : 4f / 3f));

                float attempted = Rot.GetAttemptedAtksOverDur();
                float land = Rot.GetLandedAtksOverDur();
                float crit = Rot.GetCriticalAtksOverDur();

                float hitRate  = attempted > 0 ? (float)Math.Min(1f, Math.Max(0f, land / attempted)) : 0f;
                float critRate = attempted > 0 ? (float)Math.Min(1f, Math.Max(0f, crit / attempted)) : 0f;
                
                if (Rot.SW.Validated) {
                    SpecialEffect sweep = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusTargets = 1f, },
                        (float)Math.Min(Rot.SW.Duration, landedAtksInterval * 5f), Rot.SW.Cd);
                    statsTotal.AddSpecialEffect(sweep);
                }
                if (Rot.RK.Validated) {
                    SpecialEffect reck = new SpecialEffect(Trigger.Use,
                        new Stats() { PhysicalCrit = 1f - Rot.RK.MHAtkTable.Crit, },
                        (float)Math.Min(Rot.RK.Duration, landedAtksInterval * 3f), Rot.RK.Cd);
                    statsTotal.AddSpecialEffect(reck);
                }
                if (talents.Flurry > 0)
                {
                    float value = talents.Flurry * 0.05f;
                    SpecialEffect flurry = new SpecialEffect(Trigger.MeleeCrit, new Stats() { PhysicalHaste = value, }, 
                        landedAtksInterval * 3f, 0f);
                    statsTotal.AddSpecialEffect(flurry);
                }

                SpecialEffect bersMainHand = null;
                SpecialEffect bersOffHand = null;

                if (character.MainHandEnchant != null && character.MainHandEnchant.Id == 3789)
                { // berserker enchant id
                    Stats.SpecialEffectEnumerator mhEffects = character.MainHandEnchant.Stats.SpecialEffects();

                    if (mhEffects.MoveNext())
                    {
                        bersMainHand = mhEffects.Current;
                    }
                }
                if (combatFactors.useOH && character.OffHandEnchant != null && character.OffHandEnchant.Id == 3789)
                {
                    Stats.SpecialEffectEnumerator ohEffects = character.OffHandEnchant.Stats.SpecialEffects();

                    if (ohEffects.MoveNext())
                    {
                        bersOffHand = ohEffects.Current;
                    }
                }

                statsProcs += GetSpecialEffectsStats(character, Rot, combatFactors, bersMainHand, bersOffHand, attemptedAtksInterval, hitRate, critRate, bleedHitInterval, dmgDoneInterval, statsTotal, null);

                // Base Stats
                statsProcs.Stamina      = (float)Math.Floor(statsProcs.Stamina     * (1f + totalBSTAM) * (1f + statsProcs.BonusStaminaMultiplier    ));
                statsProcs.Strength     = (float)Math.Floor(statsProcs.Strength    * (1f + totalBSM)   * (1f + statsProcs.BonusStrengthMultiplier   ));
                statsProcs.Strength    += (float)Math.Floor(statsProcs.HighestStat * (1f + totalBSM)   * (1f + statsProcs.BonusStrengthMultiplier   ));
                statsProcs.Strength    += (float)Math.Floor(statsProcs.Paragon     * (1f + totalBSM)   * (1f + statsProcs.BonusStrengthMultiplier   ));
                statsProcs.Agility      = (float)Math.Floor(statsProcs.Agility     * (1f + totalBAM)   * (1f + statsProcs.BonusAgilityMultiplier    ));
                statsProcs.Health      += (float)Math.Floor(statsProcs.Stamina     * 10f);
                
                // Armor
                statsProcs.Armor        = (float)Math.Floor(statsProcs.Armor      * (1f + statsTotal.BaseArmorMultiplier  + statsProcs.BaseArmorMultiplier ));
                statsProcs.BonusArmor  += statsProcs.Agility * 2f;
                statsProcs.BonusArmor   = (float)Math.Floor(statsProcs.BonusArmor * (1f + statsTotal.BonusArmorMultiplier + statsProcs.BonusArmorMultiplier));
                statsProcs.Armor       += statsProcs.BonusArmor;
                statsProcs.BonusArmor  = 0; //it's been added to Armor so kill it

                // Attack Power
                float totalBAPMProcs    = (1f + statsTotal.BonusAttackPowerMultiplier) * (1f + statsProcs.BonusAttackPowerMultiplier) - 1f;
                float apBonusSTRProcs   = (1f + totalBAPM) * (statsProcs.Strength * 2f);
                float apBonusAttTProcs  = (1f + totalBAPM) * ((statsProcs.Armor / 108f) * talents.ArmoredToTheTeeth);
                float apBonusOtherProcs = (1f + totalBAPM) * (statsProcs.AttackPower);
                statsProcs.AttackPower  = (float)Math.Floor(apBonusSTRProcs + apBonusAttTProcs + apBonusOtherProcs);

                // Crit
                statsProcs.PhysicalCrit += StatConversion.GetCritFromAgility(statsProcs.Agility, character.Class);

                // Haste
                statsProcs.PhysicalHaste = (1f + statsProcs.PhysicalHaste)
                                         * (1f + StatConversion.GetPhysicalHasteFromRating(statsProcs.HasteRating, character.Class))
                                         - 1f;

                statsTotal             += statsProcs;

                // Haste
                statsTotal.HasteRating   = (float)Math.Floor(statsTotal.HasteRating);
                ratingHasteBonus         = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, character.Class);
                statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                           (1f + statsItems.PhysicalHaste) *
                                           (1f + statsBuffs.PhysicalHaste) *
                                           (1f + statsTalents.PhysicalHaste) *
                                           (1f + statsOptionsPanel.PhysicalHaste) *
                                           (1f + statsProcs.PhysicalHaste) *
                                           (1f + ratingHasteBonus)
                                           - 1f;
                // special case for dual wielding w/ berserker enchant on one/both weapons, as they act independently
                combatFactors.StatS = statsTotal;
                Stats bersStats = new Stats();
                if (bersMainHand != null)
                { // berserker enchant id
                    bersStats += bersMainHand.GetAverageStats(fightDuration/Rot.GetAttemptedAtksOverDurMH(), Rot.GetLandedAtksOverDurMH() / Rot.GetAttemptedAtksOverDurMH(), combatFactors._c_mhItemSpeed, fightDuration);
                    float f = bersMainHand.GetAverageUptime(fightDuration / Rot.GetAttemptedAtksOverDurMH(), Rot.GetLandedAtksOverDurMH() / Rot.GetAttemptedAtksOverDurMH(), combatFactors._c_mhItemSpeed, fightDuration);
                }
                if (bersOffHand != null)
                {
                    bersStats += bersOffHand.GetAverageStats(fightDuration / Rot.GetAttemptedAtksOverDurOH(), Rot.GetLandedAtksOverDurOH() / Rot.GetAttemptedAtksOverDurOH(), combatFactors._c_mhItemSpeed, fightDuration);
                    float f = bersOffHand.GetAverageUptime(fightDuration / Rot.GetAttemptedAtksOverDurOH(), Rot.GetLandedAtksOverDurOH() / Rot.GetAttemptedAtksOverDurOH(), combatFactors._c_mhItemSpeed, fightDuration);
                }
                apBonusOtherProcs = (1f + totalBAPM) * (bersStats.AttackPower);
                bersStats.AttackPower = (float)Math.Floor(apBonusOtherProcs);
                statsTotal += bersStats;

                return statsTotal;
            }catch (Exception ex){
                new ErrorBoxDPSWarr("Error in creating Character Stats", ex.Message, "GetCharacterStats()");
            }
            return new Stats();
        }
        private Stats GetSpecialEffectsStats(Character Char, Rotation Rot, CombatFactors combatFactors,
            SpecialEffect bersMainHand, SpecialEffect bersOffHand,
            float attemptedAtkInterval, float hitRate, float critRate, float bleedHitInterval, float dmgDoneInterval,
            Stats statsTotal, Stats statsToProcess)
        {
            try {
                CalculationOptionsDPSWarr calcOpts = Char.CalculationOptions as CalculationOptionsDPSWarr;
                WarriorTalents talents = Char.WarriorTalents;
                Stats statsProcs = new Stats();
                float fightDuration = calcOpts.Duration;
                //
                foreach (SpecialEffect effect in (statsToProcess != null ? statsToProcess.SpecialEffects() : statsTotal.SpecialEffects())) {
                    if (effect != bersMainHand && effect != bersOffHand) // bersStats is null if the char doesn't have berserking enchant
                    {
                        float oldArp = effect.Stats.ArmorPenetrationRating;
                        if (effect.Stats.ArmorPenetrationRating > 0) {
                            float arpenBuffs =
                                ((combatFactors._c_mhItemType == ItemType.TwoHandMace) ? talents.MaceSpecialization * 0.03f : 0.00f) +
                                (!calcOpts.FuryStance ? (0.10f + statsTotal.BonusWarrior_T9_2P_ArP) : 0.0f);
                            float currentArp = arpenBuffs + StatConversion.GetArmorPenetrationFromRating(statsTotal.ArmorPenetrationRating
                                + (statsToProcess != null ? statsToProcess.ArmorPenetrationRating : 0f));
                            float arpToHardCap = (1f - currentArp) * StatConversion.RATING_PER_ARMORPENETRATION;
                            if (arpToHardCap < effect.Stats.ArmorPenetrationRating) effect.Stats.ArmorPenetrationRating = arpToHardCap;
                        }
                        switch (effect.Trigger) {
                            case Trigger.Use:
                                Stats _stats = new Stats();
                                if (effect.Stats._rawSpecialEffectDataSize == 1 && statsToProcess == null) {
                                    float uptime = effect.GetAverageUptime(0f, 1f, combatFactors._c_mhItemSpeed, fightDuration);
                                    //float uptime =  (effect.Cooldown / fightDuration);
                                    _stats.AddSpecialEffect(effect.Stats._rawSpecialEffectData[0]);
                                    Stats _stats2 = GetSpecialEffectsStats(Char, Rot, combatFactors, bersMainHand, bersOffHand,
                                        attemptedAtkInterval, hitRate, critRate, bleedHitInterval, dmgDoneInterval, statsTotal, _stats);
                                    _stats = _stats2 * uptime;
                                } else {
                                    _stats = effect.GetAverageStats(0f, 1f, combatFactors._c_mhItemSpeed, fightDuration);
                                }
                                statsProcs += _stats;
                                break;
                            case Trigger.MeleeHit:
                            case Trigger.PhysicalHit:
                                if (attemptedAtkInterval > 0f) statsProcs += effect.GetAverageStats(attemptedAtkInterval, hitRate, combatFactors._c_mhItemSpeed, fightDuration);
                                //float i = effect.GetAverageUptime(attemptedAtkInterval, hitRate, combatFactors._c_mhItemSpeed, fightDuration);
                                break;
                            case Trigger.MeleeCrit:
                            case Trigger.PhysicalCrit:
                                if (attemptedAtkInterval > 0f) statsProcs += effect.GetAverageStats(attemptedAtkInterval, critRate, combatFactors._c_mhItemSpeed, fightDuration);
                                //float j = effect.GetAverageUptime(attemptedAtkInterval, critRate, combatFactors._c_mhItemSpeed, fightDuration);
                                break;
                            case Trigger.DoTTick:
                                if (bleedHitInterval > 0f) statsProcs += effect.GetAverageStats(bleedHitInterval, 1f, combatFactors._c_mhItemSpeed, fightDuration); // 1/sec DeepWounds, 1/3sec Rend
                                break;
                            case Trigger.DamageDone: // physical and dots
                                if (dmgDoneInterval > 0f) statsProcs += effect.GetAverageStats(dmgDoneInterval, 1f, combatFactors._c_mhItemSpeed, fightDuration); 
                                break;
                            case Trigger.HSorSLHit: // Set bonus handler
                                //Rot._SL_GCDs = Rot._SL_GCDs;
                                //Rot._HS_Acts = Rot._HS_Acts;
                                if (Rot.CritHsSlamOverDur > 0f) {
                                    Stats addme = effect.GetAverageStats(fightDuration / Rot.CritHsSlamOverDur, 0.4f, combatFactors._c_mhItemSpeed, fightDuration);
                                    statsProcs += addme;
                                }
                                break;
                        }
                        effect.Stats.ArmorPenetrationRating = oldArp;
                    }
                }

                return statsProcs;
            } catch (Exception ex) {
                new ErrorBoxDPSWarr("Error in creating SpecialEffects Stats", ex.Message, "GetSpecialEffectsStats()");
                return new Stats();
            }
        }
    }
}
