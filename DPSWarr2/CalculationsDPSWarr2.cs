using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Rawr.DPSWarr;

namespace Rawr.DPSWarr2
{
    [Rawr.Calculations.RawrModelInfo("DPSWarr2", "Ability_Rogue_Ambush", Character.CharacterClass.Warrior)]
    public class CalculationsDPSWarr2 : CalculationsBase
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                ////Relevant Gem IDs for DPSWarrs
                //Red
                int[] bold = { 39900, 39996, 40111, 42142 };
                int[] fractured = { 39909, 40002, 40117, 42153 };

                //Purple
                int[] sovereign = { 39934, 40022, 40129 };
                int[] puissant = { 39933, 40033, 40140 };

                //Orange
                int[] inscribed = { 39947, 40037, 40142 };

                //Meta
                int chaotic = 41285;

                return new List<GemmingTemplate>() {
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Uncommon", //Max Strength
						RedId = bold[0], YellowId = bold[0], BlueId = bold[0], PrismaticId = bold[0], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Uncommon", //Strength
						RedId = bold[0], YellowId = inscribed[0], BlueId = sovereign[0], PrismaticId = bold[0], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Uncommon", //Armor Pen
                        RedId = fractured[0], YellowId = inscribed[0], BlueId = puissant[0], PrismaticId = fractured[0], MetaId = chaotic },
	

					new GemmingTemplate() { Model = "DPSWarr2", Group = "Rare", Enabled = true, //Max Strength
						RedId = bold[1], YellowId = bold[1], BlueId = bold[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Rare", Enabled = true, //Strength
						RedId = bold[1], YellowId = inscribed[1], BlueId = sovereign[1], PrismaticId = bold[1], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Rare", //Armor Pen
                        RedId = fractured[1], YellowId = inscribed[1], BlueId = puissant[1], PrismaticId = fractured[1], MetaId = chaotic },	

					new GemmingTemplate() { Model = "DPSWarr2", Group = "Epic", //Max Strength
						RedId = bold[2], YellowId = bold[2], BlueId = bold[2], PrismaticId = bold[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Epic", //Strength
						RedId = bold[2], YellowId = inscribed[2], BlueId = sovereign[2], PrismaticId = bold[2], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Epic", //Armor Pen
                        RedId = fractured[2], YellowId = inscribed[2], BlueId = puissant[2], PrismaticId = fractured[2], MetaId = chaotic },
	
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Jeweler", //Max Strength
						RedId = bold[3], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[3], MetaId = chaotic },
					new GemmingTemplate() { Model = "DPSWarr2", Group = "Jeweler", //Strength
						RedId = bold[1], YellowId = bold[3], BlueId = bold[3], PrismaticId = bold[1], MetaId = chaotic },
                    new GemmingTemplate() { Model = "DPSWarr2", Group = "Uncommon", //Max Armor Pen
                        RedId = fractured[3], YellowId = fractured[3], BlueId = fractured[3], PrismaticId = fractured[3], MetaId = chaotic },
                    new GemmingTemplate() { Model = "DPSWarr2", Group = "Jeweler", //Armor Pen
                        RedId = fractured[1], YellowId = fractured[3], BlueId = fractured[3], PrismaticId = fractured[1], MetaId = chaotic },
				};
            }
        }

        #region Variables and Properties
        public CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
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

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                {
                    _characterDisplayCalculationLabels = new string[] {
    					"Base Stats:Health",
    					"Base Stats:Stamina",
                        "Base Stats:Armor",
                        "Base Stats:Strength*1 STR = 2 AP x Kings Buff x Talents",
                        "Base Stats:Attack Power*2 AP is just 2 AP, go for STR",
                        "Base Stats:Agility",
                        "Base Stats:Haste",
                        "Base Stats:Crit",
                        @"Base Stats:Armor Penetration*Rating to Cap with buffs/debuffs applied
1232-None
1074-Sndr
1039-Sndr+FF
0916-Sndr+FF+Arms
0731-Sndr+FF+Arms+Mace",
                        "Base Stats:Damage Reduction*Should be exact opposite of ArP in Percentage (%)",
                        @"Base Stats:Hit Rating*8.00% chance to miss base for Yellow Attacks
Precision 0- 8%-0%=8%=263 Rating soft cap
Precision 1- 8%-1%=7%=230 Rating soft cap
Precision 2- 8%-2%=6%=197 Rating soft cap
Precision 3- 8%-3%=5%=164 Rating soft cap",
                        @"Base Stats:Expertise*6.50% chance to dodge/parry for attacks
Weapon Mastery 0- 6.50%-0%=6.50%=214 Rating Cap
Weapon Mastery 1- 6.50%-1%=6.50%=181 Rating Cap
Weapon Mastery 2- 6.50%-2%=6.50%=148 Rating Cap

Don't forget your weapons used matched with races can affect these numbers.",
                        
                        "DPS Breakdown (Fury):Bloodsurge",
                        "DPS Breakdown (Fury):Bloodthirst",
                        "DPS Breakdown (Fury):Whirlwind",
                        "DPS Breakdown (Arms):Mortal Strike",
                        "DPS Breakdown (Arms):Slam",
                        "DPS Breakdown (Arms):Rend",
                        "DPS Breakdown (Arms):Sudden Death",
                        "DPS Breakdown (Arms):Overpower",
                        "DPS Breakdown (Arms):Bladestorm",
                        "DPS Breakdown (Arms):Sword Spec",
                        "DPS Breakdown (General):Heroic Strike",
                        "DPS Breakdown (General):Deep Wounds",
                        "DPS Breakdown (General):White DPS",
                        "DPS Breakdown (General):Total DPS",
                      
                        "Rage Details:White DPS Rage",
                        "Rage Details:Other Rage",
                        "Rage Details:Free Rage",
                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                    "Health",
                    "Haste Rating",
                    "Expertise Rating",
                    "Hit Rating",
					"Crit Rating",
					"Agility",
					"Attack Power",
					"Armor Penetration",
					"Armor Penetration Rating",
					};
                return _optimizableCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                {
                    _customChartNames = new string[] {
                        "SEP",
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
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("DPS", Color.Red);
                }
                return _subPointNameColors;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new[] {
                        Item.ItemType.None,
                        Item.ItemType.Leather,
                        Item.ItemType.Mail,
                        Item.ItemType.Plate,
                        Item.ItemType.Bow,
                        Item.ItemType.Crossbow,
                        Item.ItemType.Gun,
                        Item.ItemType.Thrown,
                        Item.ItemType.Dagger,
                        Item.ItemType.FistWeapon,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.OneHandSword,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.Staff,
                        Item.ItemType.Polearm,
                        Item.ItemType.TwoHandMace,
                        Item.ItemType.TwoHandSword,
                        Item.ItemType.TwoHandAxe
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Warrior; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationsDPSWarr2(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSWarr2(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            XmlSerializer s = new XmlSerializer(typeof(CalculationOptionsDPSWarr));
            StringReader sr = new StringReader(xml);
            CalculationOptionsDPSWarr calcOpts = s.Deserialize(sr) as CalculationOptionsDPSWarr;
            return calcOpts;
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            /** TODO (ebs)
             * Migrate
             **/
            CharacterCalculationsDPSWarr2 calculatedStats = new CharacterCalculationsDPSWarr2();
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;

            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race);
            calculatedStats.BasicStats = stats;
            /*CombatFactors combatFactors = new CombatFactors(character, stats);
            WhiteAttacks whiteAttacks = new WhiteAttacks(character.WarriorTalents, stats, combatFactors);
            Skills skillAttacks = new Skills(character, character.WarriorTalents, stats, combatFactors, whiteAttacks);
            
            calculatedStats.BasicStats = stats;
            calculatedStats.SkillAttacks = skillAttacks;
            calculatedStats.TargetLevel = calcOpts.TargetLevel;
            calculatedStats.BaseHealth = statsRace.Health;
            // Attack Table
            calculatedStats.Miss = stats.Miss;
            calculatedStats.HitRating = stats.HitRating;
            calculatedStats.HitPercent = StatConversion.GetHitFromRating(stats.HitRating);
            calculatedStats.ExpertiseRating = stats.ExpertiseRating;
            calculatedStats.Expertise = StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, character.Class);
            calculatedStats.MhExpertise = combatFactors.MhExpertise;
            calculatedStats.OhExpertise = combatFactors.OhExpertise;
            calculatedStats.AgilityCritBonus = StatConversion.GetCritFromAgility(stats.Agility, character.Class);
            calculatedStats.CritRating = stats.CritRating;
            float stancecritbonus = 0.0f;
            if (calcOpts.FuryStance) { stancecritbonus = 3.0f; }
            calculatedStats.CritPercent = StatConversion.GetCritFromRating(stats.CritRating) + (character.WarriorTalents.Cruelty + stancecritbonus) / 100.0f
                + calculatedStats.AgilityCritBonus + statsRace.PhysicalCrit;
            calculatedStats.MhCrit = combatFactors.MhCrit;// +stancecritbonus / 100.0f;
            calculatedStats.OhCrit = combatFactors.OhCrit;// +stancecritbonus / 100.0f;
            // Offensive
            float teethbonus = stats.Armor;
            teethbonus *= (float)character.WarriorTalents.ArmoredToTheTeeth;
            teethbonus /= 180.0f;
            calculatedStats.TeethBonus = (int)teethbonus;
            calculatedStats.ArmorPenetrationMaceSpec = ((character.MainHand != null && character.MainHand.Type == Item.ItemType.TwoHandMace) ? character.WarriorTalents.MaceSpecialization * 0.03f : 0.00f);
            calculatedStats.ArmorPenetrationStance = ((!calcOpts.FuryStance) ? 0.10f : 0.00f);
            calculatedStats.ArmorPenetrationRating = stats.ArmorPenetrationRating;
            calculatedStats.ArmorPenetrationRating2Perc = StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating);
            calculatedStats.ArmorPenetration = calculatedStats.ArmorPenetrationMaceSpec
                + calculatedStats.ArmorPenetrationStance
                + calculatedStats.ArmorPenetrationRating2Perc;
            calculatedStats.HasteRating = stats.HasteRating;
            calculatedStats.HastePercent = StatConversion.GetHasteFromRating(stats.HasteRating, Character.CharacterClass.Warrior);
            // DPS
            calculatedStats.WhiteDPSMH = whiteAttacks.CalcMhWhiteDPS();
            calculatedStats.WhiteDPSOH = whiteAttacks.CalcOhWhiteDPS();
            calculatedStats.WhiteDPS = calculatedStats.WhiteDPSMH + calculatedStats.WhiteDPSOH;
            calculatedStats.DeepWoundsDPS = skillAttacks.Deepwounds();
            calculatedStats.HeroicStrikeDPS = skillAttacks.HeroicStrike();
            calculatedStats.SlamDPS = skillAttacks.Slam();
            calculatedStats.RendDPS = skillAttacks.Rend();
            calculatedStats.MortalStrikeDPS = skillAttacks.MortalStrike();
            calculatedStats.OverpowerDPS = skillAttacks.Overpower();
            calculatedStats.SwordSpecDPS = skillAttacks.SwordSpec();
            calculatedStats.BladestormDPS = skillAttacks.BladeStorm();
            calculatedStats.BloodsurgeDPS = skillAttacks.Bloodsurge();
            calculatedStats.SuddenDeathDPS = skillAttacks.SuddenDeath();
            calculatedStats.BloodthirstDPS = skillAttacks.Bloodthirst();
            calculatedStats.WhirlwindDPS = skillAttacks.Whirlwind();
            // Neutral
            // Defensive
            calculatedStats.Armor = (int)stats.Armor;
            calculatedStats.damageReduc = combatFactors.DamageReduction;

            calculatedStats.WhiteRage = whiteAttacks.whiteRageGen();
            calculatedStats.OtherRage = skillAttacks.OtherRage();
            calculatedStats.FreeRage = skillAttacks.freeRage();

            calculatedStats.TotalDPS = whiteAttacks.CalcMhWhiteDPS() + whiteAttacks.CalcOhWhiteDPS() + skillAttacks.Bloodthirst() + skillAttacks.Whirlwind() +
                                       skillAttacks.HeroicStrike() + skillAttacks.Bloodsurge() + skillAttacks.Deepwounds() +
                                       skillAttacks.MortalStrike() + skillAttacks.SuddenDeath() + skillAttacks.Slam() + skillAttacks.Overpower() + skillAttacks.Rend() + skillAttacks.SwordSpec() + skillAttacks.BladeStorm();
            calculatedStats.OverallPoints = calculatedStats.TotalDPS;
            */
            calculatedStats.OverallPoints = stats.AttackPower;
            return calculatedStats;
        }

        #region Charts and Relevent Stats
        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats
            {
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
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                MongooseProc = stats.MongooseProc,
                MongooseProcAverage = stats.MongooseProcAverage,
                MongooseProcConstant = stats.MongooseProcConstant,
                ExecutionerProc = stats.ExecutionerProc,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                Armor = stats.Armor,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusSlamDamage = stats.BonusSlamDamage,
                DreadnaughtBonusRageProc = stats.DreadnaughtBonusRageProc,
                BerserkingProc = stats.BerserkingProc
            };
        }
        public override bool HasRelevantStats(Stats stats)
        {
            return (
                stats.Agility +
                stats.Strength +
                stats.BonusAgilityMultiplier +
                stats.BonusStrengthMultiplier +
                stats.AttackPower +
                stats.BonusAttackPowerMultiplier +
                stats.CritRating + stats.HitRating +
                stats.HasteRating +
                stats.ExpertiseRating +
                stats.ArmorPenetration +
                stats.ArmorPenetrationRating +
                stats.WeaponDamage +
                stats.BonusCritMultiplier +
                stats.BonusDamageMultiplier +
                stats.MongooseProc +
                stats.MongooseProcAverage +
                stats.MongooseProcConstant +
                stats.ExecutionerProc +
                stats.BonusBleedDamageMultiplier +
                stats.BonusCritChance +
                stats.Armor +
                stats.PhysicalHaste +
                stats.PhysicalCrit +
                stats.BonusPhysicalDamageMultiplier +
                stats.DreadnaughtBonusRageProc +
                stats.BonusSlamDamage +
                stats.BerserkingProc) != 0;
        }
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsDPSWarr2 baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { Strength = 10 } },
                        new Item() { Stats = new Stats() { Agility = 10 } },
                        new Item() { Stats = new Stats() { AttackPower = 20 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                        new Item() { Stats = new Stats() { ExpertiseRating = 8.19f } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { ArmorPenetrationRating = 10 } },
                    };
                    string[] statList = new string[] {
                        "Strength",
                        "Agility",
                        "Attack Power",
                        "Crit Rating",
                        "Hit Rating",
                        "Expertise Rating",
                        "Haste Rating",
                        "Armor Penetration Rating",
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsDPSWarr2;

                    float mod = 1;
                    for (int index = 0; index < itemList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsDPSWarr2;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = (calc.OverallPoints - baseCalc.OverallPoints) / 10;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            if (comparison.Name == "Strength")
                            {
                                subPoints[i] = 1;
                            }
                            else
                            {
                                subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                                subPoints[i] /= 10;
                                subPoints[i] /= mod;
                            }
                        }
                        comparison.SubPoints = subPoints;
                        if (comparison.Name == "Strength")
                        {
                            mod = comparison.OverallPoints;
                            comparison.OverallPoints = 1;
                        }
                        else if (comparison.Name == "Expertise Rating")
                        {
                            comparison.OverallPoints /= mod;
                            comparison.OverallPoints /= 8.197f;
                        }
                        else
                        {
                            comparison.OverallPoints /= mod;
                        }

                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }
        #endregion

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
            WarriorTalents talents = character.WarriorTalents;

            Stats statsRace = BaseStats.GetBaseStats(character.Level, character.Class, character.Race);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsTalents = new Stats()
            {
                // Arms
                Parry = talents.Deflection * 0.01f,
                CritBonusDamage = talents.Impale * 0.10f,
                PhysicalHaste = talents.BloodFrenzy * 0.03f,
                BonusBleedDamageMultiplier = talents.BloodFrenzy * 0.02f,

                // Fury
                PhysicalCrit = talents.Cruelty * 0.01f,
                PhysicalHit = talents.Precision * 0.01f,
                
                // Prot
                Dodge = talents.Anticipation * 0.01f,
                Block = talents.ShieldSpecialization * 0.01f,
                BonusBlockValueMultiplier = talents.ShieldMastery * 0.15f,
                BonusShieldSlamDamage = talents.GagOrder * 0.05f,
                DevastateCritIncrease = talents.SwordAndBoard * 0.05f,
                BaseArmorMultiplier = talents.Toughness * 0.02f,

                // Multiples
                BonusDamageMultiplier = (1f + ((character.MainHand != null && character.MainHand.Item.Slot == Item.ItemSlot.OneHand) ?
                    talents.OneHandedWeaponSpecialization * 0.02f : talents.TwoHandedWeaponSpecialization * 0.02f)) *
                    (1 - talents.TitansGrip*0.1f) - 1f,
                BonusStaminaMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f,
                BonusStrengthMultiplier = talents.Vitality * 0.02f + talents.StrengthOfArms * 0.02f + talents.ImprovedBerserkerStance * 0.04f,
                Expertise = talents.Vitality * 2.0f + talents.StrengthOfArms * 2.0f,
                
                
            };
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsRace + statsItems + statsBuffs + statsTalents;

            float agiBase = (float)Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier));
            statsTotal.BaseAgility = agiBase;
            float agiBonus = (float)Math.Floor(statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier));
            float strBase = (float)Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float strBonus = (float)Math.Floor(statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier));
            float staBase = (float)Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier));
            float staBonus = (float)Math.Floor((statsGearEnchantsBuffs.Stamina + (float)calcOpts.ToughnessLvl) * (1 + statsRace.BonusStaminaMultiplier));
            statsTotal.Stamina = (float)Math.Floor((statsRace.Stamina + statsTalents.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Stamina += (float)Math.Floor((statsItems.Stamina + statsBuffs.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));

            statsTotal.BonusAttackPowerMultiplier = ((1 + statsRace.BonusAttackPowerMultiplier) * (1 + statsGearEnchantsBuffs.BonusAttackPowerMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier) * (1 + 0.02f * character.WarriorTalents.StrengthOfArms) * (1 + character.WarriorTalents.ImprovedBerserkerStance * ((calcOpts.FuryStance) ? 0.04f : 0.00f))) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;

            statsTotal.Agility = (float)Math.Floor(agiBase * (1f + statsTotal.BonusAgilityMultiplier)) + (float)Math.Floor(agiBonus * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Strength = (float)Math.Floor(strBase * (1f + statsTotal.BonusStrengthMultiplier)) + (float)Math.Floor(strBonus * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Stamina = (float)Math.Floor((staBase) * (1f + statsTotal.BonusStaminaMultiplier)) + (float)Math.Floor(staBonus * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f))));

            statsTotal.Armor = statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2;

            statsTotal.AttackPower = (statsTotal.Strength * 2 + statsRace.AttackPower) + statsGearEnchantsBuffs.AttackPower;
            statsTotal.AttackPower += (statsTotal.Armor / 180) * character.WarriorTalents.ArmoredToTheTeeth;
            statsTotal.AttackPower += statsTotal.AttackPower * statsTotal.BonusAttackPowerMultiplier;

            statsTotal.PhysicalHit = character.WarriorTalents.Precision;
            statsTotal.HitRating = statsGearEnchantsBuffs.HitRating;

            statsTotal.ExpertiseRating = statsGearEnchantsBuffs.ExpertiseRating;
            //statsTotal.Expertise += 2 * character.WarriorTalents.StrengthOfArms;
            //statsTotal.Expertise += (character.MainHand != null ? CombatFactors.GetRacialExpertiseFromWeapon(character.Race, character.MainHand.Item) : 0f);

            statsTotal.HasteRating = statsGearEnchantsBuffs.HasteRating;
            statsTotal.PhysicalHaste = statsGearEnchantsBuffs.PhysicalHaste;
            statsTotal.PhysicalHaste += statsTalents.PhysicalHaste;
            //statsTotal.PhysicalHaste *= (1 + 0.03f * character.WarriorTalents.BloodFrenzy);

            statsTotal.ArmorPenetration = statsGearEnchantsBuffs.ArmorPenetration
                + ((character.MainHand != null && character.MainHand.Type == Item.ItemType.TwoHandMace) ? talents.MaceSpecialization * 0.03f : 0.00f)
                + ((!calcOpts.FuryStance) ? 0.10f : 0.00f);

            statsTotal.CritRating = statsRace.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.PhysicalCrit = statsRace.PhysicalCrit;
            statsTotal.PhysicalCrit += StatConversion.GetCritFromAgility(statsTotal.Agility, character.Class);
            statsTotal.PhysicalCrit += character.WarriorTalents.Cruelty * 0.01f;
            statsTotal.PhysicalCrit += statsGearEnchantsBuffs.PhysicalCrit;

            statsTotal.BonusCritMultiplier = statsGearEnchantsBuffs.BonusCritMultiplier;

            statsTotal.BonusDamageMultiplier = statsGearEnchantsBuffs.BonusDamageMultiplier;
            statsTotal.BonusPhysicalDamageMultiplier = statsGearEnchantsBuffs.BonusPhysicalDamageMultiplier;

            statsTotal.WeaponDamage = statsGearEnchantsBuffs.WeaponDamage;

            statsTotal.BonusBleedDamageMultiplier = statsGearEnchantsBuffs.BonusBleedDamageMultiplier;
            statsTotal.BonusSlamDamage = statsGearEnchantsBuffs.BonusSlamDamage;
            statsTotal.DreadnaughtBonusRageProc = statsGearEnchantsBuffs.DreadnaughtBonusRageProc;




            // TODO: This is new and stolen from the Cat model per Astrylian and is supposed to handle all procs
            // such as Berserking, Mirror of Truth, Grim Toll, etc.
            float hasteBonus = StatConversion.GetPhysicalHasteFromRating(statsTotal.HasteRating, Character.CharacterClass.Warrior);//stats.HasteRating * 1.3f / 32.78998947f / 100f;
            hasteBonus = (1f + hasteBonus) * (1f + statsTotal.Bloodlust * 40f / Math.Max(calcOpts.Duration, 40f)) - 1f;
            float meleeHitInterval = 1f / ((1f + hasteBonus) * (1f + statsTotal.PhysicalHaste) + 1f / 3.5f);
            float chanceCrit = StatConversion.GetCritFromRating(statsTotal.CritRating) + statsTotal.PhysicalCrit +
                StatConversion.GetCritFromAgility(statsTotal.Agility, Character.CharacterClass.Warrior) //(stats.CritRating / 45.90598679f + stats.Agility * 0.012f) / 100f + stats.PhysicalCrit 
                - (0.006f * (calcOpts.TargetLevel - character.Level) + (calcOpts.TargetLevel == 83 ? 0.03f : 0f));
            Stats statsProcs = new Stats();
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        statsProcs += effect.GetAverageStats(0f, 1f, 1f, calcOpts.Duration);
                        break;
                    case Trigger.MeleeHit:
                    case Trigger.PhysicalHit:
                        statsProcs += effect.GetAverageStats(meleeHitInterval, 1f, 1f, calcOpts.Duration);
                        break;
                    case Trigger.MeleeCrit:
                    case Trigger.PhysicalCrit:
                        statsProcs += effect.GetAverageStats(meleeHitInterval, chanceCrit, 1f, calcOpts.Duration);
                        break;
                    case Trigger.DoTTick:
                        statsProcs += effect.GetAverageStats(1.5f, 1f, 1f, calcOpts.Duration);
                        break;
                    case Trigger.DamageDone:
                        statsProcs += effect.GetAverageStats(meleeHitInterval / 2f, 1f, 1f, calcOpts.Duration);
                        break;
                }
            }

            statsProcs.Stamina = (float)Math.Floor(statsProcs.Stamina * (1f + statsTotal.BonusStaminaMultiplier));
            statsProcs.Strength = (float)Math.Floor(statsProcs.Strength * (1f + statsTotal.BonusStrengthMultiplier));
            statsProcs.Agility = (float)Math.Floor(statsProcs.Agility * (1f + statsTotal.BonusAgilityMultiplier));
            statsProcs.AttackPower += statsProcs.Strength * 2f + statsProcs.Agility;
            statsProcs.AttackPower = (float)Math.Floor(statsProcs.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsProcs.Health += (float)Math.Floor(statsProcs.Stamina * 10f);
            statsProcs.Armor += 2f * statsProcs.Agility;
            statsProcs.Armor = (float)Math.Floor(statsProcs.Armor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal += statsProcs;



            return statsTotal;
        }
    }
}
