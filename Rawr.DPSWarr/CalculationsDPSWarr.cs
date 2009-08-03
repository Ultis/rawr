using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
#if SILVERLIGHT
using System.Windows.Media;
#else
using System.Drawing;
#endif

namespace Rawr.DPSWarr {
    [Rawr.Calculations.RawrModelInfo("DPSWarr", "Ability_Rogue_Ambush", CharacterClass.Warrior)]
    public class CalculationsDPSWarr : CalculationsBase {
        public override List<GemmingTemplate> DefaultGemmingTemplates {
            get {
				////Relevant Gem IDs for DPSWarrs
				//Red
				int[] bold = { 39900, 39996, 40111, 42142 };
                int[] fractured = { 39909, 40002, 40117, 42153 };
				//Purple
				int[] sovereign = { 39934, 40022, 40129 };

				//Orange
				int[] inscribed = { 39947, 40037, 40142 };
                int[] puissant = { 39933, 40033, 40140 };

				//Meta
				int chaotic = 41285;

				return new List<GemmingTemplate>() {
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon", //Max Strength
						RedId = bold[0], YellowId = bold[0], BlueId = bold[0], PrismaticId = bold[0], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Uncommon", //Strength
						RedId = bold[0], YellowId = inscribed[0], BlueId = sovereign[0], PrismaticId = bold[0], MetaId = chaotic },
                    
						
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare", Enabled = true, //Max Strength
						RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare", Enabled = true, //Strength
						RedId = bold[1], YellowId = inscribed[1], BlueId = sovereign[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Rare", Enabled = true, // ArPen
                        RedId = fractured[1], YellowId = inscribed[1], BlueId = puissant[1], PrismaticId = fractured[1], MetaId = chaotic },
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Rare", Enabled = true, // Max ArPen
                        RedId = fractured[1], YellowId = fractured[1], BlueId = fractured[1], PrismaticId = fractured[1], MetaId = chaotic },
	
					new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", //Max Strength
						RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Epic", //Strength
						RedId = bold[2], YellowId = inscribed[2], BlueId = sovereign[2], PrismaticId = bold[2], MetaId = chaotic },
						
					new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler", //Max Strength
						RedId = bold[3], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[3], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler", //Strength
						RedId = bold[1], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[1], MetaId = chaotic },
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler", // ArPen
                        RedId = fractured[1], YellowId = fractured[3], BlueId = fractured[3], PrismaticId = fractured[1], MetaId = chaotic },
                    new GemmingTemplate() { Model = "DPSWarr", Group = "Jeweler", // Max ArPen
                        RedId = fractured[3], YellowId = fractured[3], BlueId = fractured[3], PrismaticId = fractured[3], MetaId = chaotic },
				};
            }
        }

        #region Variables and Properties

        #if SILVERLIGHT
            public ICalculationOptionsPanel _calculationOptionsPanel = null;
            public override ICalculationOptionsPanel CalculationOptionsPanel
            {
                get
                {
                    if (_calculationOptionsPanel == null)
                    {
                        _calculationOptionsPanel = new CalculationOptionsPanelDPSWarr();
                    }
                    return _calculationOptionsPanel;
                }
            }
        #else
            public CalculationOptionsPanelBase _calculationOptionsPanel = null;
            public override CalculationOptionsPanelBase CalculationOptionsPanel {
                get {
                    if (_calculationOptionsPanel == null) {
                        _calculationOptionsPanel = new CalculationOptionsPanelDPSWarr();
                    }
                    return _calculationOptionsPanel;
                }
            }
        #endif

		private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null) {
                    _characterDisplayCalculationLabels = new string[] {
    					"Base Stats:Health and Stamina",
    					//"Base Stats:Stamina",
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
                        @"DPS Breakdown (General):Total DPS*1st number is total DPS
2nd number is total DMG over Duration",
                      
                        "Rage Details:Total Generated Rage",
                        "Rage Details:Needed Rage for Abilities",
                        "Rage Details:Available Free Rage",
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
                    "Resilience",
                    "Armor",
                    "Strength",
                    "Attack Power",
                    "Agility",
					"Crit Rating",
                    "Crit %",
                    "Haste Rating",
                    "Haste %",
					"Armor Penetration Rating",
					"Armor Penetration %",
                    "Hit Rating",
                    "Hit %",
                    "White Miss %",
                    "Yellow Miss %",
                    "Expertise Rating",
                    "Expertise",
                    "Dodge/Parry Reduction %",
                    "Dodge %",
                    "Parry %",
                    "Chance to be Avoided %",
                    //"Threat Reduction",
                    //"Threat Per Second",
					};
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        /*"Ability DPS Comparison"/*,
                        /*""/*,
                        /*""/*,
                        /*""*/
                    };
                }
                return _customChartNames;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors
        {
            get
            {
                
                if (_subPointNameColors == null){
#if SILVERLIGHT
                    _subPointNameColors = new Dictionary<string, System.Windows.Media.Color>();
                    _subPointNameColors.Add("DPS", System.Windows.Media.Color.FromArgb(255,255,0,0));
#else
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("DPS", Color.Red);
#endif
                }
                return _subPointNameColors;
            }
        }

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
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

        public override CharacterClass TargetClass { get { return CharacterClass.Warrior; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationsDPSWarr(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSWarr(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            XmlSerializer s = new XmlSerializer(typeof(CalculationOptionsDPSWarr));
            StringReader sr = new StringReader(xml);
            CalculationOptionsDPSWarr calcOpts = s.Deserialize(sr) as CalculationOptionsDPSWarr;
            return calcOpts;
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations) {
            CharacterCalculationsDPSWarr calculatedStats = new CharacterCalculationsDPSWarr();
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            Stats stats = GetCharacterStats(character, additionalItem);
            WarriorTalents talents = character.WarriorTalents;

            CombatFactors combatFactors = new CombatFactors(character, stats);
            Skills.WhiteAttacks whiteAttacks = new Skills.WhiteAttacks(character.WarriorTalents, stats, combatFactors, character);
            Skills skillAttacks = new Skills(character, character.WarriorTalents, stats, combatFactors, whiteAttacks);
            Rotation Rot = new Rotation(character, character.WarriorTalents, stats, combatFactors, whiteAttacks);
            Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race);// GetRaceStats(character);

            calculatedStats.Duration = calcOpts.Duration;
            calculatedStats.BasicStats = stats;
            calculatedStats.SkillAttacks = skillAttacks;
            calculatedStats.combatFactors = combatFactors;
            calculatedStats.Rot = Rot;
            calculatedStats.TargetLevel = calcOpts.TargetLevel;
            calculatedStats.BaseHealth = statsRace.Health;
            {// == Attack Table ==
                // Miss
                calculatedStats.Miss = stats.Miss;
                calculatedStats.HitRating = stats.HitRating;
                calculatedStats.HitPercent = StatConversion.GetHitFromRating(stats.HitRating);
                calculatedStats.HitPercBonus = stats.PhysicalHit;
                calculatedStats.HitPercentTtl = calculatedStats.HitPercent + calculatedStats.HitPercBonus;
                calculatedStats.HitCanFree =
                    StatConversion.GetRatingFromHit(
                        StatConversion.WHITE_MISS_CHANCE_CAP
                        - calculatedStats.HitPercBonus
                        - StatConversion.GetHitFromRating(calculatedStats.HitRating)
                    )
                    * -1f;
                calculatedStats.ExpertiseRating = stats.ExpertiseRating;
                calculatedStats.Expertise = StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, character.Class);
                calculatedStats.MhExpertise = combatFactors._c_mhexpertise;
                calculatedStats.OhExpertise = combatFactors._c_ohexpertise;
                calculatedStats.WeapMastPerc = character.WarriorTalents.WeaponMastery / 100f;
                calculatedStats.AgilityCritBonus = StatConversion.GetCritFromAgility(stats.Agility, character.Class);
                calculatedStats.CritRating = stats.CritRating;
                calculatedStats.CritPercent = StatConversion.GetCritFromRating(stats.CritRating) + stats.PhysicalCrit;
                calculatedStats.MhCrit = combatFactors._c_mhycrit;
                calculatedStats.OhCrit = combatFactors._c_ohycrit;
            }
            // Offensive
            float teethbonus = stats.Armor;
            teethbonus *= (float)character.WarriorTalents.ArmoredToTheTeeth;
            teethbonus /= 180.0f;
            calculatedStats.TeethBonus = (int)teethbonus;
            calculatedStats.ArmorPenetrationMaceSpec = ((character.MainHand != null && combatFactors._c_mhItemType == ItemType.TwoHandMace) ? character.WarriorTalents.MaceSpecialization * 0.03f : 0.00f);
            calculatedStats.ArmorPenetrationStance = ((!calcOpts.FuryStance) ? 0.10f : 0.00f);
            calculatedStats.ArmorPenetrationRating = stats.ArmorPenetrationRating;
            calculatedStats.ArmorPenetrationRating2Perc = StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating);
            calculatedStats.ArmorPenetration = calculatedStats.ArmorPenetrationMaceSpec
                + calculatedStats.ArmorPenetrationStance
                + calculatedStats.ArmorPenetrationRating2Perc;
            calculatedStats.HasteRating = stats.HasteRating;
            calculatedStats.HastePercent = talents.BloodFrenzy * (0.05f) + StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Warrior);
            // DPS

            Rot.Initialize(calculatedStats);
            if (calcOpts.MultipleTargets) { calculatedStats.Which = calculatedStats.HS; } else { calculatedStats.Which = calculatedStats.CL; }

            // Neutral
            // Defensive
            calculatedStats.Armor = (int)stats.Armor;

            calculatedStats.floorstring = calcOpts.AllowFlooring ? "000" : "000.00";

            if (calcOpts.FuryStance) {
                calculatedStats.TotalDPS = Rot.MakeRotationandDoDPS_Fury() + calculatedStats.Rot._DW_DPS;
                calculatedStats.WhiteDPS = calculatedStats.WhiteDPSMH + calculatedStats.WhiteDPSOH;
                //calculatedStats.TotalDPS += calculatedStats.WhiteDPS;
                /*calculatedStats.TotalDPS = calculatedStats.WhiteDPSMH + calculatedStats.WhiteDPSOH
                    + calculatedStats.BT.DPS + calculatedStats.WW.DPS + calculatedStats.BS.DPS
                    + calculatedStats.DW.DPS + calculatedStats.HS.DPS;*/
            }else{
                calculatedStats.TotalDPS = Rot.MakeRotationandDoDPS_Arms() + calculatedStats.Rot._DW_DPS;
                calculatedStats.WhiteDPS = Rot._WhiteDPS;
            }
            calculatedStats.OverallPoints = calculatedStats.TotalDPS;

            if (calcOpts.FuryStance) {
                calculatedStats.WhiteRage = whiteAttacks.whiteRageGenPerSec;
                calculatedStats.OtherRage = Rot.RageGenOtherPerSec;
                calculatedStats.NeedyRage = Rot.RageNeededPerSec;
                calculatedStats.FreeRage  = Rot.freeRage;
            }else{
                calculatedStats.WhiteRage = whiteAttacks.whiteRageGenPerSec;
                calculatedStats.OtherRage = Rot.RageGenOther;
                calculatedStats.NeedyRage = Rot.RageNeeded;
                calculatedStats.FreeRage  = Rot.RageGenWhite + Rot.RageGenOther - Rot.RageNeeded;
            }

            return calculatedStats;
        }

        public override Stats GetItemStats(Character character, Item additionalItem) {
            Stats statsItems = base.GetItemStats(character,additionalItem);
            return statsItems;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem) {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            WarriorTalents talents = character.WarriorTalents;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race);// GetRaceStats(character);
            Stats statsBuffs = GetBuffsStats(character);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsOptionsPanel = new Stats() {
                BonusStrengthMultiplier = (calcOpts.FuryStance ? talents.ImprovedBerserkerStance * 0.04f : 0f),
                PhysicalCrit = (calcOpts.FuryStance ? 0.03f : 0f)
                            // handle boss level difference
                            - 0.006f * (calcOpts.TargetLevel - 80f)
                            - (calcOpts.TargetLevel == 83f ? 0.03f : 0f),
            };
            Stats statsTalents = new Stats() {
                //Parry = talents.Deflection * 1.0f,
                PhysicalCrit = talents.Cruelty * 0.01f,
                //Dodge = talents.Anticipation * 1.0f,
                //Block = talents.ShieldSpecialization * 1.0f,
                //BonusBlockValueMultiplier = talents.ShieldMastery * 0.15f,
                BonusDamageMultiplier = /*(character.MainHand != null &&
                                         (character.MainHand.Type == ItemType.OneHandAxe ||
                                         character.MainHand.Type == ItemType.OneHandMace ||
                                         character.MainHand.Type == ItemType.OneHandSword ||
                                         character.MainHand.Type == ItemType.Dagger ||
                                         character.MainHand.Type == ItemType.FistWeapon)
                                         ? talents.OneHandedWeaponSpecialization * 0.02f: 0f)
                                         +*/ 
                                        1f *
                                        (character.MainHand != null &&
                                        (character.MainHand.Slot == ItemSlot.TwoHand)
                                         ? 1f + talents.TwoHandedWeaponSpecialization * 0.02f : 1f)
                                         *
                                         ((talents.TitansGrip == 1 && (character.MainHand != null && character.OffHand != null) &&
                                        (character.OffHand.Slot  == ItemSlot.TwoHand ||
                                         character.MainHand.Slot == ItemSlot.TwoHand)
                                         ? 0.90f : 1f)) -
                                         1f,
                BonusStaminaMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                BonusStrengthMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                Expertise = talents.Vitality * 2.0f + talents.StrengthOfArms * 2.0f,
                BonusArmorMultiplier = talents.Toughness * 0.02f,
                PhysicalHaste = talents.BloodFrenzy * 0.05f,
                // Removing this line, and instead adding it to CombatFactors as a buff, since statsTotal.ArmorPenetration includes arp from debuffs and not buffs, and they need to be separate
                //ArmorPenetration = ((character.MainHand != null && character.MainHand.Type == ItemType.TwoHandMace) ? talents.MaceSpecialization * 0.03f : 0.00f),
                PhysicalHit = talents.Precision * 0.01f,
            };
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents + statsOptionsPanel;

            // Stamina
            float totalBSTAM = statsTotal.BonusStaminaMultiplier;
            float staBase   = (float)Math.Floor((1f + totalBSTAM) * statsRace.Stamina             );
            float staBonus  = (float)Math.Floor((1f + totalBSTAM) * statsGearEnchantsBuffs.Stamina);
            statsTotal.Stamina = staBase + staBonus;
            
            // Health
            statsTotal.Health  += StatConversion.GetHealthFromStamina(statsTotal.Stamina);

            // Strength
            float totalBSM = statsTotal.BonusStrengthMultiplier;
            float strBase  = (float)Math.Floor((1f + totalBSM) * statsRace.Strength             );
            float strBonus = (float)Math.Floor((1f + totalBSM) * statsGearEnchantsBuffs.Strength);
            statsTotal.Strength = strBase + strBonus;

            // Agility
            statsTotal.Agility = (float)Math.Floor(statsTotal.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            
            // Armor
            statsTotal.Armor *= (1f + statsTotal.BonusArmorMultiplier);
            statsTotal.Armor += statsTotal.Agility * 2f;

            // Attack Power
            float totalBAPM = statsTotal.BonusAttackPowerMultiplier;
            float apBase        = (float)Math.Floor((1f + totalBAPM) * (statsRace.AttackPower                                ));
            float apBonusSTR    = (float)Math.Floor((1f + totalBAPM) * (statsTotal.Strength * 2f                             ));
            float apBonusAttT   = (float)Math.Floor((1f + totalBAPM) * ((statsTotal.Armor / (calcOpts._3pt2Mode ? 108f : 180f)) * talents.ArmoredToTheTeeth));
            float apBonusOther  = (float)Math.Floor((1f + totalBAPM) * (statsGearEnchantsBuffs.AttackPower                   ));
            statsTotal.AttackPower = apBase + apBonusSTR + apBonusAttT + apBonusOther;

            // Crit
            statsTotal.PhysicalCrit += StatConversion.GetCritFromAgility(statsTotal.Agility, character.Class);

            // SpecialEffects: Supposed to handle all procs such as Berserking, Mirror of Truth, Grim Toll, etc.
            CombatFactors combatFactors = new CombatFactors(character, statsTotal);
            Skills.WhiteAttacks whiteAttacks = new Skills.WhiteAttacks(talents, statsTotal, combatFactors, character);
            Rotation Rot = new Rotation(character, talents, statsTotal, combatFactors, whiteAttacks);
            Rot.Initialize();

            float fightDuration = calcOpts.Duration;

            float mhHitsPerSecond = 0f; float ohHitsPerSecond = 0f;
            if (calcOpts.FuryStance) {
                Skills.Ability bt = new Skills.BloodThirst(character, statsTotal, combatFactors, whiteAttacks);
                Skills.Ability ww = new Skills.WhirlWind(  character, statsTotal, combatFactors, whiteAttacks);
                mhHitsPerSecond = (bt.Activates + ww.Activates) / fightDuration * combatFactors.ProbMhYellowLand;
                ohHitsPerSecond = (               ww.Activates) / fightDuration * combatFactors.ProbOhYellowLand;
            }else{mhHitsPerSecond = 1f / (1.5f + calcOpts.GetLatency()) * 0.9f * combatFactors.ProbMhYellowLand;}
            if (combatFactors._c_mhItemSpeed > 0f) { mhHitsPerSecond += (1f / combatFactors.MH.Speed) * combatFactors.ProbMhWhiteLand; }
            if (combatFactors._c_ohItemSpeed > 0f) { ohHitsPerSecond += (1f / combatFactors.OH.Speed) * combatFactors.ProbOhWhiteLand; }
            
            float mhHitInterval    = 1f /  mhHitsPerSecond;
            float ohHitInterval    = 1f /  ohHitsPerSecond;
            float bothHitInterval  = 1f / (mhHitsPerSecond + ohHitsPerSecond);
            float bleedHitInterval = 1f / (calcOpts.FuryStance ? 1f : 4f / 3f); // 4/3 ticks per sec with deep wounds and rend both going, 1 tick/sec with just deep wounds
            float dmgDoneInterval  = 1f / (mhHitsPerSecond + ohHitsPerSecond + (calcOpts.FuryStance ? 1f : 4f / 3f));

            Stats statsProcs = new Stats();
            SpecialEffect bersMainHand = null;
            SpecialEffect bersOffHand = null;

            // special case for dual wielding w/ berserker enchant on one/both weapons, as they act independently
            if (character.MainHandEnchant != null && character.MainHandEnchant.Id == 3789){ // berserker enchant id
                Stats.SpecialEffectEnumerator mhEffects = character.MainHandEnchant.Stats.SpecialEffects();

                if (mhEffects.MoveNext()) {
                    bersMainHand = mhEffects.Current;
                    statsProcs += bersMainHand.GetAverageStats(mhHitInterval, 1f, combatFactors.MH.Speed, fightDuration);
                }
            }
            if (character.OffHandEnchant != null && character.OffHandEnchant.Id == 3789){
                Stats.SpecialEffectEnumerator ohEffects = character.OffHandEnchant.Stats.SpecialEffects();

                if (ohEffects.MoveNext()) {
                    bersOffHand = ohEffects.Current;
                    statsProcs += bersOffHand.GetAverageStats(ohHitInterval, 1f, combatFactors.OH.Speed, fightDuration);
                }
            }
            foreach (SpecialEffect effect in statsTotal.SpecialEffects()) {
                if (effect != bersMainHand && effect != bersOffHand) // bersStats is null if the char doesn't have berserking enchant
                {
                    switch (effect.Trigger)
                    {
                        case Trigger.Use: 
                            statsProcs += effect.GetAverageStats(0f, 1f, combatFactors._c_mhItemSpeed, fightDuration); 
                            break;
                        case Trigger.MeleeHit:
                        case Trigger.PhysicalHit: 
                            statsProcs += effect.GetAverageStats(bothHitInterval, 1f, combatFactors._c_mhItemSpeed, fightDuration);
                            break;
                        case Trigger.MeleeCrit:
                        case Trigger.PhysicalCrit:
                            statsProcs += effect.GetAverageStats(bothHitInterval, combatFactors._c_mhycrit, combatFactors._c_mhItemSpeed, fightDuration);
                            break;
                        case Trigger.DoTTick:
                            statsProcs += effect.GetAverageStats(bleedHitInterval, 1f, combatFactors._c_mhItemSpeed, fightDuration); // 1/sec DeepWounds, 1/3sec Rend
                            break;
                        case Trigger.DamageDone: // physical and dots
                            statsProcs += effect.GetAverageStats(dmgDoneInterval, 1f, combatFactors._c_mhItemSpeed, fightDuration); 
                            break;
                    }
                }
            }
            if (statsTotal.BonusWarrior2PT8Haste > 0f) {
                SpecialEffect hasteBonusEffect = new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { HasteRating = statsTotal.BonusWarrior2PT8Haste },
                    5f, // duration
                    0f // cooldown
                );
                statsProcs += hasteBonusEffect.GetAverageStats(1f / Rot.CritHsSlamPerSec, 1f, combatFactors.MH.Speed, fightDuration);
            }
            // Warrior Abilities as SpecialEffects
            Stats avgstats = new Stats() { AttackPower = 0f, };
            Skills.DeathWish       Death = new Skills.DeathWish(      character,statsTotal,combatFactors,whiteAttacks);
            avgstats += Death.AverageStats;
            //Recklessness is highly inaccurate right now
            //Skills.Recklessness  Reck  = new Skills.Recklessness(   character,statsTotal,combatFactors,whiteAttacks);
            //avgstats += Reck.AverageStats ;
            Skills.ShatteringThrow Shatt = new Skills.ShatteringThrow(character,statsTotal,combatFactors,whiteAttacks);
            avgstats += Shatt.AverageStats;
            Skills.SweepingStrikes Sweep = new Skills.SweepingStrikes(character,statsTotal,combatFactors,whiteAttacks);
            avgstats += Sweep.AverageStats;
            Skills.Bloodrage       Blood = new Skills.Bloodrage(      character,statsTotal,combatFactors,whiteAttacks);
            avgstats += Blood.AverageStats;
            //Skills.Hamstring     Hammy = new Skills.Hamstring(      character,statsTotal,combatFactors,whiteAttacks);
            //avgstats += Hammy.AverageStats;
            Skills.BattleShout     Battle = new Skills.BattleShout(   character,statsTotal,combatFactors,whiteAttacks);
            avgstats += Battle.AverageStats;
            Skills.CommandingShout CS = new Skills.CommandingShout( character,statsTotal,combatFactors,whiteAttacks);
            avgstats += CS.AverageStats;
            statsProcs += avgstats;

            statsProcs.Stamina      = (float)Math.Floor(statsProcs.Stamina     * (1f + statsTotal.BonusStaminaMultiplier));
            statsProcs.Strength     = (float)Math.Floor(statsProcs.Strength    * (1f + statsTotal.BonusStrengthMultiplier));
            statsProcs.Strength    += statsProcs.HighestStat;
            statsProcs.Agility      = (float)Math.Floor(statsProcs.Agility     * (1f + statsTotal.BonusAgilityMultiplier));
            statsProcs.AttackPower += statsProcs.Strength * 2f;
            statsProcs.AttackPower  = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsProcs.Health      += (float)Math.Floor(statsProcs.Stamina     * 10f);
            statsProcs.Armor       += 2f * statsProcs.Agility;
            statsProcs.Armor        = (float)Math.Floor(statsProcs.Armor       * (1f + statsTotal.BonusArmorMultiplier));

            statsTotal             += statsProcs;

            // Haste
            statsTotal.PhysicalHaste = (1f + statsRace.PhysicalHaste) *
                                       (1f + statsItems.PhysicalHaste) *
                                       (1f + statsBuffs.PhysicalHaste) *
                                       (1f + statsTalents.PhysicalHaste) *
                                       (1f + statsOptionsPanel.PhysicalHaste) *
                                       (1f + statsProcs.PhysicalHaste)
                                       - 1f;
            float ratingHasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, CharacterClass.Warrior);
            statsTotal.PhysicalHaste = (1f + statsTotal.PhysicalHaste) * (1f + ratingHasteBonus) - 1f;

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            /*List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            //CharacterCalculationsDPSWarr baseCalc, calc;
            //ComparisonCalculationBase comparison;
            //float[] subPoints;
            */
            switch (chartName) {
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot) {
            //Hide the ranged weapon enchants. None of them apply to melee damage at all.
            if (character != null && (character.WarriorTalents != null && enchant != null)) {
                return enchant.Slot == ItemSlot.Ranged ? false : base.EnchantFitsInSlot(enchant,character,slot) || (character.WarriorTalents.TitansGrip == 1 && enchant.Slot == ItemSlot.TwoHand && slot == ItemSlot.OffHand);
            }
            return enchant.Slot == ItemSlot.Ranged ? false : enchant.FitsInSlot(slot);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
			Stats s = new Stats() {
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                Health = stats.Health,
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                PhysicalHit = stats.PhysicalHit,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
				ArmorPenetration = stats.ArmorPenetration,
				ArmorPenetrationRating = stats.ArmorPenetrationRating,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusSlamDamage = stats.BonusSlamDamage,
                ArcaneDamage = stats.ArcaneDamage,
                DreadnaughtBonusRageProc = stats.DreadnaughtBonusRageProc,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone)
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }
        public override bool HasRelevantStats(Stats stats) {
            bool relevant = (
                stats.Stamina +
                stats.Health +
                stats.Agility +
                stats.Strength +
                stats.BonusAgilityMultiplier +
                stats.BonusStrengthMultiplier +
                stats.AttackPower +
                stats.BonusAttackPowerMultiplier +
                stats.CritRating +
                stats.HitRating +
                stats.PhysicalHit +
                stats.HasteRating +
                stats.ExpertiseRating +
                stats.ArmorPenetration +
                stats.ArmorPenetrationRating +
                stats.WeaponDamage +
                stats.BonusCritMultiplier +
                stats.BonusDamageMultiplier +
                stats.BonusBleedDamageMultiplier +
                stats.BonusCritChance +
                stats.Armor +
                stats.BonusArmor +
                stats.BonusArmorMultiplier + 
                stats.PhysicalHaste +
                stats.PhysicalCrit +
                stats.ArcaneDamage +
                stats.BonusPhysicalDamageMultiplier +
                stats.DreadnaughtBonusRageProc +
                stats.BonusWarrior2PT8Haste +
                stats.MortalstrikeBloodthirstCritIncrease +
                stats.BonusSlamDamage +
                //stats.BloodlustProc +
                stats.DarkmoonCardDeathProc + 
                stats.HighestStat
                ) != 0;
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit)
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            return relevant;
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique) {
            if (item == null || character == null) {
                return false;
            } else if (character.WarriorTalents.TitansGrip == 1 && item.Type == ItemType.Polearm && slot == CharacterSlot.OffHand) {
                return false;
            } else if (slot == CharacterSlot.OffHand && item.Slot == ItemSlot.TwoHand && character.WarriorTalents.TitansGrip == 1) {
                return true;
            } else if (slot == CharacterSlot.OffHand && character.MainHand != null && character.MainHand.Slot == ItemSlot.TwoHand) {
                return false;
            } else if (item.Type == ItemType.Polearm && slot == CharacterSlot.MainHand) {
                return true;
            } else {
                return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
            }
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

        public void GetTalents(Character character) {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            calcOpts.talents = character.WarriorTalents;
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs() {
            if (_relevantGlyphs == null) {
                _relevantGlyphs = new List<string>();
                // ===== MAJOR GLYPHS =====
                _relevantGlyphs.Add("Glyph of Bladestorm");
                _relevantGlyphs.Add("Glyph of Bloodthirst");
                _relevantGlyphs.Add("Glyph of Cleaving");
                _relevantGlyphs.Add("Glyph of Enraged Regeneration");
                _relevantGlyphs.Add("Glyph of Execution");
                _relevantGlyphs.Add("Glyph of Hamstring");
                _relevantGlyphs.Add("Glyph of Heroic Strike");
                _relevantGlyphs.Add("Glyph of Mortal Strike");
                _relevantGlyphs.Add("Glyph of Overpower");
                _relevantGlyphs.Add("Glyph of Rapid Charge");
                _relevantGlyphs.Add("Glyph of Rending");
                _relevantGlyphs.Add("Glyph of Resonating Power");
                _relevantGlyphs.Add("Glyph of Sweeping Strikes");
                _relevantGlyphs.Add("Glyph of Victory Rush");
                _relevantGlyphs.Add("Glyph of Vigilance");
                _relevantGlyphs.Add("Glyph of Whirlwind");
                /* The following Glyphs have been disabled as they are solely Defensive in nature.
                _relevantGlyphs.Add("Glyph of Barbaric Insults");
                _relevantGlyphs.Add("Glyph of Blocking");
                _relevantGlyphs.Add("Glyph of Devastate");
                _relevantGlyphs.Add("Glyph of Intervene");
                _relevantGlyphs.Add("Glyph of Last Stand");
                _relevantGlyphs.Add("Glyph of Revenge");
                _relevantGlyphs.Add("Glyph of Shield Wall");
                _relevantGlyphs.Add("Glyph of Shockwave");
                _relevantGlyphs.Add("Glyph of Spell Reflection");
                _relevantGlyphs.Add("Glyph of Sunder Armor");
                _relevantGlyphs.Add("Glyph of Taunt");*/
                // ===== MINOR GLYPHS =====
                _relevantGlyphs.Add("Glyph of Battle");
                _relevantGlyphs.Add("Glyph of Bloodrage");
                _relevantGlyphs.Add("Glyph of Charge");
                _relevantGlyphs.Add("Glyph of Enduring Victory");
                _relevantGlyphs.Add("Glyph of Thunder Clap");
                /* The following Glyphs have been disabled as they are solely Defensive in nature.
                //_relevantGlyphs.Add("Glyph of Mocking Blow");*/
            }
            return _relevantGlyphs;
        }
    
    }
}
