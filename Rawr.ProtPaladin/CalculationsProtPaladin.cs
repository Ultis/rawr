using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    [Rawr.Calculations.RawrModelInfo("ProtPaladin", "Ability_Paladin_JudgementsOfTheJust", Character.CharacterClass.Paladin)]
    public class CalculationsProtPaladin : CalculationsBase
    {
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for ProtPaladin
                // Red
                int[] bold = { 39900, 39996, 40111, 42142 };        // Strength
                int[] delicate = { 39905, 39997, 40112, 42143 };    // Agility
                int[] subtle = { 39907, 40000, 40115, 42151 };      // Dodge
                int[] flashing = { 39908, 40001, 40116, 42152 };    // Parry
                int[] precise = { 39910, 40003, 40118, 42154 };     // Expertise

                // Purple
                int[] shifting = { 39935, 40023, 40130, 40130 };    // Agility + Stamina
                int[] sovereign = { 39934, 40022, 40129, 40129 };   // Strength + Stamina
                int[] regal = { 39938, 40031, 40138, 40138 };       // Dodge + Stamina
                int[] defender = { 39939, 40032, 40139, 40139 };    // Parry + Stamina
                int[] guardian = { 39940, 40034, 40141, 40141 };    // Expertise + Stamina

                // Blue
                int[] solid = { 39919, 40008, 40119, 36767 };       // Stamina

                // Green
                int[] enduring = { 39976, 40089, 40167, 40167 };    // Defense + Stamina

                // Yellow
                int[] thick = { 39916, 40015, 40126, 42157 };       // Defense

                // Orange
                int[] etched = { 39948, 40038, 40143, 40143 };      // Strength + Hit
                int[] champion = { 39949, 40039, 40144, 40144 };    // Strength + Defense
                int[] stalwart = { 39964, 40056, 40160, 40160 };    // Dodge + Defense
                int[] glimmering = { 39965, 40057, 40161, 40161 };  // Parry + Defense
                int[] accurate = { 39966, 40058, 40162, 40162 };    // Expertise + Hit
                int[] resolute = { 39967, 40059, 40163, 40163 };    // Expertise + Defense

                // Meta
                int[] austere = { 41380 };
                int[] eternal = { 41396 };

                string[] groupNames = new string[] { "Uncommon", "Rare", "Epic", "Jeweler" };
                int[,][] gemmingTemplates = new int[,][]
                {
                    //Red       Yellow      Blue        Prismatic   Meta
                    { sovereign,    enduring,   solid,      solid,      austere },  // Balanced Threat + Avoidance, Austere
                    { sovereign,    enduring,   solid,      solid,      eternal },  // Balanced Threat + Avoidance, Eternal
                    { solid,        solid,      solid,      solid,      austere },  // Max Stam, Austere
                    { solid,        solid,      solid,      solid,      eternal },  // Max Stam, Eternal
                };

                // Generate List of Gemming Templates
                List<GemmingTemplate> gemmingTemplate = new List<GemmingTemplate>();
                for (int j = 0; j <= groupNames.GetUpperBound(0); j++)
                {
                    for (int i = 0; i <= gemmingTemplates.GetUpperBound(0); i++)
                    {
                        gemmingTemplate.Add(new GemmingTemplate()
                        {
                            Model = "ProtPaladin",
                            Group = groupNames[j],
                            RedId = gemmingTemplates[i, 0][j],
                            YellowId = gemmingTemplates[i, 1][j],
                            BlueId = gemmingTemplates[i, 2][j],
                            PrismaticId = gemmingTemplates[i, 3][j],
                            MetaId = gemmingTemplates[i, 4][0],
                            Enabled = j == 1
                        });
                    }
                }
                return gemmingTemplate;
            }
        }

        private static List<string> _relevantGlyphs;
        public override List<string> GetRelevantGlyphs()
        {
            if (_relevantGlyphs == null)
            {
                _relevantGlyphs = new List<string>();
                _relevantGlyphs.Add("Glyph of Judgement");
                _relevantGlyphs.Add("Glyph of Exorcism");
                _relevantGlyphs.Add("Glyph of Sense Undead");
                _relevantGlyphs.Add("Glyph of Consecration");
                _relevantGlyphs.Add("Glyph of Seal of Vengeance");
                _relevantGlyphs.Add("Glyph of Seal of Righteousness");
                _relevantGlyphs.Add("Glyph of Divine Plea");
            }
            return _relevantGlyphs;
        }

        #region Variables and Properties
        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelProtPaladin();
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
                    _characterDisplayCalculationLabels = new string[] {
                    "Base Stats:Health",
                    "Base Stats:Mana",
                    "Base Stats:Strength",
                    "Base Stats:Agility",
                    "Base Stats:Stamina",
                    "Base Stats:Intellect",

                    "Defensive Stats:Armor",
                    "Defensive Stats:Defense",
                    "Defensive Stats:Miss",
                    "Defensive Stats:Dodge",
                    "Defensive Stats:Parry",
                    "Defensive Stats:Block",
                    "Defensive Stats:Chance to be Crit",
                    "Defensive Stats:Resilience",
                    "Defensive Stats:Block Value",                      
                    "Defensive Stats:Avoidance",
                    "Defensive Stats:Avoidance + Block",
                    "Defensive Stats:Guaranteed Reduction",
                    "Defensive Stats:Total Mitigation",
                    "Defensive Stats:Attacker Speed",
                    "Defensive Stats:Damage Taken",

                    "Offensive Stats:Weapon Speed",
                    "Offensive Stats:Attack Power",
                    "Offensive Stats:Spell Power",
                    "Offensive Stats:Hit",
                    "Offensive Stats:Spell Hit",
                    "Offensive Stats:Physical Haste",
                    "Offensive Stats:Effective Target Armor",
                    "Offensive Stats:Effective Armor Penetration",
                    "Offensive Stats:Crit",
                    "Offensive Stats:Spell Crit",
                    "Offensive Stats:Expertise",
                    "Offensive Stats:Weapon Damage",
                    "Offensive Stats:Missed Attacks",
                    "Offensive Stats:Glancing Attacks",
                    "Offensive Stats:Total Damage/sec",
                    "Offensive Stats:Threat/sec",
                    //"Offensive Stats:Unlimited Threat/sec*All white damage converted to Heroic Strikes.",

                    "Resistances:Nature Resist",
                    "Resistances:Fire Resist",
                    "Resistances:Frost Resist",
                    "Resistances:Shadow Resist",
                    "Resistances:Arcane Resist",
                    "Complex Stats:Ranking Mode*The currently selected ranking mode. Ranking modes can be changed in the Options tab.",
                    @"Complex Stats:Overall Points*Overall Points are a sum of Mitigation, Threat and Survival Points. 
Overall is typically, but not always, the best way to rate gear. 
For specific encounters, closer attention to Mitigation and 
Survival Points individually may be important.
Remember to set your threat scale as needed.",
                    @"Complex Stats:Mitigation Points*Mitigation Points represent the amount of damage you mitigate, 
on average, through armor mitigation and avoidance. It is directly 
relational to your Damage Taken. Ideally, you want to maximize 
Mitigation Points, while maintaining 'enough' Survival Points 
(see Survival Points). If you find yourself dying due to healers 
running OOM, or being too busy healing you and letting other 
raid members die, then focus on Mitigation Points.",
                    @"Complex Stats:Survival Points*Survival Points represents the total raw physical damage 
(pre-avoidance/block) you can take before dying. Unlike 
Mitigation Points, you should not attempt to maximize this, 
but rather get 'enough' of it, and then focus on Mitigation. 
'Enough' can vary greatly by fight and by your healers.
If you find that you are being killed by burst damage,
focus on Survival Points.",
                    @"Complex Stats:Threat Points*Threat Points represents the average threat per second accumulated scaled by the threat scale.",
                    "Complex Stats:Nature Survival",
                    "Complex Stats:Fire Survival",
                    "Complex Stats:Frost Survival",
                    "Complex Stats:Shadow Survival",
                    "Complex Stats:Arcane Survival",
                    };
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
                    "Threat Per Second",
                    "% Total Mitigation",
                    "% Guaranteed Reduction",
                    "% Chance to Avoid Attacks",
                    "% chance to Avoid + Block Attacks",
                    "% Chance to be Crit",
                    "Defense Skill",
                    "Block Value",
                    "Burst Time", 
                    "TankPoints", 
                    "Nature Survival",
                    "Fire Survival",
                    "Frost Survival",
                    "Shadow Survival",
                    "Arcane Survival",
                    "% Hit Chance",
                    "% Spell Hit Chance",
                    "% Chance to be Avoided",
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
                    _customChartNames = new string[] {
                    "Ability Damage",
                    "Ability Threat",
                    "Combat Table: Defensive Stats",
                    "Combat Table: Offensive Stats",
                    "Combat Table: Spell Resistance",
                    "Item Budget",
                    };
                return _customChartNames;
            }
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Mitigation", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Threat", System.Drawing.Color.Yellow);
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
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
                    {
                        Item.ItemType.Plate,
                        Item.ItemType.None,
                        Item.ItemType.Shield,
                        Item.ItemType.Libram,
                        Item.ItemType.OneHandAxe,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.OneHandSword,
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Paladin; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationProtPaladin(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsProtPaladin(); }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsProtPaladin));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsProtPaladin calcOpts = serializer.Deserialize(reader) as CalculationOptionsProtPaladin;
            return calcOpts;
        }
        #endregion

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CharacterCalculationsProtPaladin calculatedStats = new CharacterCalculationsProtPaladin();
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            Stats stats = GetCharacterStats(character, additionalItem);

            AttackModelMode amm = AttackModelMode.BasicSoV;            
            if (calcOpts.SealChoice == "Seal of Righteousness")
                amm = AttackModelMode.BasicSoR;

            DefendModel dm = new DefendModel(character, stats);
            AttackModel am = new AttackModel(character, stats, amm);

            calculatedStats.BasicStats = stats;

            // Target Info
            calculatedStats.TargetLevel = calcOpts.TargetLevel;
            calculatedStats.TargetArmor = calcOpts.TargetArmor;
            calculatedStats.EffectiveTargetArmor = Lookup.GetEffectiveTargetArmor(character.Level, calcOpts.TargetArmor, stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating);
            calculatedStats.TargetArmorDamageReduction = Lookup.TargetArmorReduction(character, stats);
            calculatedStats.EffectiveTargetArmorDamageReduction = Lookup.EffectiveTargetArmorReduction(character ,stats);
            calculatedStats.ArmorPenetrationCap = Lookup.GetArmorPenetrationCap(calcOpts.TargetLevel, calcOpts.TargetArmor, 0.0f, stats.ArmorPenetration, stats.ArmorPenetrationRating);
            
            calculatedStats.ActiveBuffs = new List<Buff>(character.ActiveBuffs);
            calculatedStats.Abilities = am.Abilities;

            // Defensive stats
            calculatedStats.Miss = dm.DefendTable.Miss;
            calculatedStats.Dodge = dm.DefendTable.Dodge;
            calculatedStats.Parry = dm.DefendTable.Parry;
            calculatedStats.Block = dm.DefendTable.Block;

            calculatedStats.Defense = 400.0f + stats.Defense + (float)Math.Floor(stats.DefenseRating * ProtPaladin.DefenseRatingToDefense);
            calculatedStats.StaticBlockValue = stats.BlockValue;
            calculatedStats.ActiveBlockValue = stats.BlockValue + stats.JudgementBlockValue + stats.ShieldOfRighteousnessBlockValue;

            calculatedStats.DodgePlusMissPlusParry = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry;
            calculatedStats.DodgePlusMissPlusParryPlusBlock = calculatedStats.Dodge + calculatedStats.Miss + calculatedStats.Parry + calculatedStats.Block;
            calculatedStats.CritReduction = Lookup.AvoidanceChance(character, stats, HitResult.Crit);
            calculatedStats.CritVulnerability = dm.DefendTable.Critical;

            calculatedStats.ArmorReduction = Lookup.ArmorReduction(character, stats);
            calculatedStats.GuaranteedReduction = dm.GuaranteedReduction;
            calculatedStats.TotalMitigation = dm.Mitigation;
            calculatedStats.BaseAttackerSpeed = calcOpts.BossAttackSpeed;
            calculatedStats.AttackerSpeed = dm.ParryModel.BossAttackSpeed;
            calculatedStats.DamageTaken = dm.DamageTaken;
            calculatedStats.DPSTaken = dm.DamagePerSecond;
            calculatedStats.DamageTakenPerHit = dm.DamagePerHit;
            calculatedStats.DamageTakenPerBlock = dm.DamagePerBlock;
            calculatedStats.DamageTakenPerCrit = dm.DamagePerCrit;

            calculatedStats.ResistanceTable = StatConversion.GetResistanceTable(calcOpts.TargetLevel, character.Level, stats.AllResist + stats.FrostResistance, 0.0f);
            calculatedStats.ArcaneReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Arcane));
            calculatedStats.FireReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Fire));
            calculatedStats.FrostReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Frost));
            calculatedStats.NatureReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Nature));
            calculatedStats.ShadowReduction = (1.0f - Lookup.MagicReduction(character, stats, DamageType.Shadow));
            calculatedStats.ArcaneSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Arcane);
            calculatedStats.FireSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Fire);
            calculatedStats.FrostSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Frost);
            calculatedStats.NatureSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Nature);
            calculatedStats.ShadowSurvivalPoints = stats.Health / Lookup.MagicReduction(character, stats, DamageType.Shadow);

            // Offensive Stats
            calculatedStats.Hit = Lookup.HitChance(character, stats);
            calculatedStats.SpellHit = Lookup.SpellHitChance(character, stats);
            calculatedStats.Crit = Lookup.CritChance(character, stats);
            calculatedStats.SpellCrit = Lookup.SpellCritChance(character, stats);
            calculatedStats.Expertise = Lookup.BonusExpertisePercentage(character, stats);
            calculatedStats.PhysicalHaste = Lookup.BonusPhysicalHastePercentage(character, stats);
            calculatedStats.SpellHaste = Lookup.BonusSpellHastePercentage(character, stats);
            calculatedStats.ArmorPenetration = 1.0f - (1.0f - stats.ArmorPenetration) * (1.0f - (float)Math.Min(1.0f, StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating)));
            calculatedStats.ArmorPenetrationFromRating = (float)Math.Min(1.0f, Lookup.BonusArmorPenetrationPercentage(character, stats));
            calculatedStats.EffectiveArmorPenetration = (calcOpts.TargetArmor == 0) ? 0.0f : 1.0f - calculatedStats.EffectiveTargetArmor / calculatedStats.TargetArmor;
            if ((calculatedStats.ArmorPenetrationFromRating * calcOpts.TargetArmor) == 0.0f)
                calculatedStats.EffectiveArmorPenetrationRating = 0.0f;
            else
                calculatedStats.EffectiveArmorPenetrationRating = (float)Math.Max(0.0f, 
                    (calculatedStats.ArmorPenetrationFromRating * calculatedStats.ArmorPenetrationCap) / (calculatedStats.ArmorPenetrationFromRating * calcOpts.TargetArmor));
            float test = calculatedStats.EffectiveTargetArmorDamageReduction / calculatedStats.TargetArmorDamageReduction;
            calculatedStats.AvoidedAttacks = am.Abilities[Ability.None].AttackTable.AnyMiss;
            calculatedStats.MissedAttacks = am.Abilities[Ability.None].AttackTable.Miss;
            calculatedStats.DodgedAttacks = am.Abilities[Ability.None].AttackTable.Dodge;
            calculatedStats.ParriedAttacks = am.Abilities[Ability.None].AttackTable.Parry;
            calculatedStats.GlancingAttacks = am.Abilities[Ability.None].AttackTable.Glance;
            calculatedStats.GlancingReduction = Lookup.GlancingReduction(character);
            calculatedStats.BlockedAttacks = am.Abilities[Ability.None].AttackTable.Block;
            calculatedStats.WeaponSpeed = Lookup.WeaponSpeed(character, stats);
            calculatedStats.TotalDamagePerSecond = am.DamagePerSecond;

            // Ranking Points
            //calculatedStats.UnlimitedThreat = am.ThreatPerSecond;
            //am.RageModelMode = RageModelMode.Limited;
            calculatedStats.ThreatPerSecond = am.ThreatPerSecond;
            calculatedStats.ThreatModel = am.Name + "\n" + am.Description;

            calculatedStats.TankPoints = dm.TankPoints;
            calculatedStats.BurstTime = dm.BurstTime;
            calculatedStats.RankingMode = calcOpts.RankingMode;
            calculatedStats.ThreatPoints = calcOpts.ThreatScale * calculatedStats.ThreatPerSecond;
            switch (calcOpts.RankingMode)
            {
                #region Alternative Ranking Modes
                case 2:
                    // Tank Points Mode
                    calculatedStats.SurvivalPoints = (dm.EffectiveHealth);
                    calculatedStats.MitigationPoints = (dm.TankPoints - dm.EffectiveHealth);
                    calculatedStats.ThreatPoints *= 3.0f;
                    calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
                    break;
                case 3:
                    // Burst Time Mode
                    float threatScale = Convert.ToSingle(Math.Pow(Convert.ToDouble(calcOpts.BossAttackValue) / 25000.0d, 4));
                    calculatedStats.SurvivalPoints = (dm.BurstTime * 100.0f);
                    calculatedStats.MitigationPoints = 0.0f;
                    calculatedStats.ThreatPoints = (calculatedStats.ThreatPoints / threatScale) * 2.0f;
                    calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
                    break;
                case 4:
                    // Damage Output Mode
                    calculatedStats.SurvivalPoints = 0.0f;
                    calculatedStats.MitigationPoints = 0.0f;
                    calculatedStats.ThreatPoints = calculatedStats.TotalDamagePerSecond;
                    calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
                    break;
                case 5:
                    // ProtWarr Model (Average damage mitigated - EvanM Model)
                    calculatedStats.SurvivalPoints = (dm.EffectiveHealth);
                    calculatedStats.MitigationPoints = dm.Mitigation * calcOpts.BossAttackValue * 17000.0f / calcOpts.MitigationScale;
                    calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
                    break;
                case 6:
                    // Damage Taken of Boss Attack Value Mode
                    calculatedStats.SurvivalPoints = (dm.EffectiveHealth);
                    calculatedStats.MitigationPoints = dm.DamageTaken * calcOpts.BossAttackValue * (float)Math.Pow(10f, 17000.0f / calcOpts.MitigationScale);
                    calculatedStats.OverallPoints = -calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
                    break;
                case 7:
                    // Damage Taken of Boss Attack Value Mode
                    // Note: Will crash Rawr when you optimize for Mitigation Points
                    calculatedStats.SurvivalPoints = (dm.EffectiveHealth);
                    calculatedStats.MitigationPoints = -dm.DamageTaken * calcOpts.BossAttackValue * (float)Math.Pow(10f, calcOpts.MitigationScale);
                    calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
                    break;
                case 8:
                    // Model 8 Placeholder
                    calculatedStats.SurvivalPoints = 0.0f;
                    calculatedStats.MitigationPoints = 0.0f;
                    calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints;
                    break;
                #endregion
                default:
                    // Mitigation Scale Mode (Bear Model)
                    calculatedStats.SurvivalPoints = dm.EffectiveHealth;
                    calculatedStats.MitigationPoints = dm.DamageTaken == 0.0f ? 0.0f : (calcOpts.MitigationScale / dm.DamageTaken);
                    calculatedStats.OverallPoints = calculatedStats.MitigationPoints + calculatedStats.SurvivalPoints + calculatedStats.ThreatPoints;
                    break;
            }

            return calculatedStats;
        }

        #region Paladin Base Stats

        /* These values are well-defined by the sum of base value for Race, Class and gains from level.
        * For information about Race and Class values see http://www.wowwiki.com/Race
        * Infomation about Paladin stat gains from level and the formula for base mana is Negarines own research.
        *
        * BaseMana and BaseHealth coefficients change at level 25 and 60.
        *
        * BaseMana from level 1 - 25:
        * Let S_N = a_0 + a_1 + ... + a_N = SUM(a_n, n=0, N) be the partial sum to N of the sequence (a_n)
        * BaseMana_0 = 42f, constant.
        * BaseMana_1 = BaseMana_0 + BaseManaGain_1
        * BaseMana_2 = BaseMana_0 + BaseManaGain_1 + BaseManaGain_2
        * ...
        * BaseMana_LVL = BaseMana_0 + BaseManaGain_1 + BaseManaGain_2 + ... + BaseManaGain_LVL
        * BaseManaGain_n = -15f * IntLvlGain_n + 17 + n and finally
        * BaseMana_LVL = BaseMana_0 + SUM(BaseManaGain_n, n=1, LVL) with
        * IntLvlGain = {0,1,0,1,0,1,1,0,1,0,1,1,0,1,1,0,1,1,0,1,1,1,0,1} for level 1-25.
        * If I ever find the algorithm behind their StatLvlGain...(Moebius Triangle is pretty darn close: Floor(n * PI) mod2)
        * BaseMana from level 25 - 60: work in progess
        * BaseMana from level 60 - 80: BaseMana_LVL ~ 144,1 * LVL - 7134,3
        * You get the idea what it's like to model this, all you need is the DING! data ;o
        */

        private static float[] BaseHealth = new float[] // one-indexed
        {
            /*Paladin*/     0f, 18f, 26f, 34f, 42f, 50f, 58f, 66f, 84f, 92f, 100f, 108f, 116f, 124f, 132f, 131f,
                            141f, 152f, 164f, 177f, 191f, 206f, 222f, 239f, 247f, 266f, 286f, 307f, 329f,
                            342f, 366f, 391f, 407f, 434f, 462f, 481f, 511f, 542f, 564f, 597f, 621f, 656f,
                            682f, 719f, 747f, 786f, 816f, 857f, 889f, 922f, 966f, 1001f, 1037f, 1084f, 1122f,
                            1161f, 1201f, 1252f, 1294f, 1337f, 1381f, 1540f, 1708f, 1884f, 2068f, 2262f, 2466f,
                            2679f, 2901f, 3134f, 3377f, 3629f, 3900f, 4191f, 4503f, 4839f, 5200f, 5588f, 6005f,
                            6453f, 6934f,
        };

        private static float[] BaseMana = new float[] // one-indexed
        {
            /*Paladin*/     0f, 60f, 64f, 84f, 90f, 112f, 120f, 129f, 154f, 165f, 192f, 205f, 219f, 249f, 265f, 282f, 
                            315f, 334f, 354f, 390f, 412f, 435f, 459f, 499f, 525f, 552f, 579f, 621f, 648f, 675f, 
                            702f, 729f, 756f, 798f, 825f, 852f, 879f, 906f, 933f, 960f, 987f, 1014f, 1041f, 1068f, 
                            1110f, 1137f, 1164f, 1176f, 1203f, 1230f, 1257f, 1284f, 1311f, 1338f, 1365f, 1392f, 
                            1419f, 1446f, 1458f, 1485f, 1512f, 1656f, 1800f, 1944f, 2088f, 2232f, 2377f, 2521f, 
                            2665f, 2809f, 2953f, 3097f, 3241f, 3385f, 3529f, 3673f, 3817f, 3962f, 4106f, 4250f, 4394f,
        };

        private static float[,] ClassStats = new float[,]
        {
                            //  Strength,   Agility,    Stamina,    Intellect,  Spirit,
            /*Druid*/       {   1f,         0f,         0f,         2f,         2f     },
            /*Hunter*/      {   0f,         3f,         1f,         0f,         1f     },
            /*Mage*/        {   0f,         0f,         0f,         3f,         2f     },
            /*Paladin*/     {   2f,         0f,         2f,         0f,         1f     },
            /*Priest*/      {   0f,         0f,         0f,         2f,         3f     },
            /*Rogue*/       {   1f,         3f,         1f,         0f,         0f     },
            /*Shaman*/      {   1f,         0f,         1f,         1f,         2f     },
            /*Warlock*/     {   0f,         0f,         1f,         2f,         2f     },
            /*Warrior*/     {   3f,         0f,         2f,         0f,         0f     },
        };

            /*              //  Strength,   Agility,    Stamina,    Intellect,  Spirit,     Health,     Mana
            *  GainLvl1*    {   0f,         0f,         0f,         0f,         0f,         18f,        18f,   }, //paladin lvl 1 values 
            *  GainLvl2*    {   1f,         1f,         1f,         1f,         1f,         8f,         4f,    }, //paladin lvl 2 values
            *    ***
            *  GainLvl79*   {   3f,         1f,         3f,         1f,         1f,         478f,       159f,  }, //paladin lvl 79 values 
            *  GainLvl80*   {   3f,         2f,         2f,         2f,         2f,         501f,       174f,  }, //paladin lvl 80 values 
            *
            * The idea is to have an array over gained values from level 1-80 (the stat gain as seen in the yellow DING! text in game),
            * To get the values for a certain level, simply sum down through the array from your level.
            * The values are constant, but different for each class. I have all the values from 1-80 for Paladins, but only 1 AND 80 for the other classes. 
            * To not write too much redundant example code, LevelStats listed here is for level = 80 only.
            */

        private static float[,] LevelStats = new float[,]
        {
                            //  Strength,   Agility,    Stamina,    Intellect,  Spirit,
            /*Druid*/       {   1f,         0f,         0f,         2f,         2f,    },
            /*Hunter*/      {   0f,         3f,         1f,         0f,         1f,    },
            /*Mage*/        {   0f,         0f,         0f,         3f,         2f,    },
            /*Paladin*/     {   129f,       70f,        121f,       78f,        84f,   }, 
            /*Priest*/      {   0f,         0f,         0f,         2f,         3f,    },
            /*Rogue*/       {   1f,         3f,         1f,         0f,         0f,    },
            /*Shaman*/      {   1f,         0f,         1f,         1f,         2f,    },
            /*Warlock*/     {   0f,         0f,         1f,         2f,         2f,    },
            /*Warrior*/     {   3f,         0f,         2f,         0f,         0f,    },
        };

        private static float[,] RaceStats = new float[,]
        {
                            //  Strength,   Agility,    Stamina,    Intellect,  Spirit,
            /*Human*/       {   20f,        20f,        20f,        20f,        21f,   },
            /*Orc =*/       {   23f,        17f,        22f,        17f,        23f,   },
            /*Dwarf*/       {   22f,        16f,        23f,        19f,        19f,   },
            /*NightElf*/    {   17f,        25f,        19f,        20f,        20f,   },
            /*Undead*/      {   19f,        18f,        21f,        18f,        25f,   },
            /*Tauren*/      {   25f,        15f,        22f,        15f,        22f,   },
            /*Gnome*/       {   15f,        23f,        19f,        24f,        20f,   },
            /*Troll*/       {   21f,        22f,        21f,        16f,        21f,   },
            /*Draenei*/     {   21f,        17f,        19f,        21f,        22f,   },
            /*BloodElf*/    {   17f,        22f,        18f,        24f,        19f,   },
        };
            /*
            Stats uniform to all races of paladins:
            I plan to pull those variables out of the switch case to make the Race gains more distinct from Class attributes
            something like:
            ClassStats = new Stats()
            {
                Health = 6934f,             // Base Health for a level 80 paladin. (tauren get 5% bonus on this value)
                Mana = 4394f,               // Base Mana for a level 80 paladin
                AttackPower = 220f,         // Base Attack Power for level 80  (AP = (LVL * 3f) - 20f)
                PhysicalCrit = 0.032685f,   // Base PhysicalCrit for paladins TODO: Check if this is different for other classes
                SpellCrit = 0.03336f,       // Base SpellCrit for paladins
                Miss = 0.05f,               // Base Miss for a character with maxed out defense skill
                Dodge = 3.2685f,            // Base Dodge for a paladin
                Parry = 5f,                 // Base Parry for a chracter with maxed out defense skill
                Block = 5f,                 // Base Block for character with maxed out defense skill
            };
            Stats statsBase = RaceStats + StatsClass + StatsLevel;
            
            later in the code, the following arrays of stats are used,
            
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsBase + statsItems + statsBuffs + statsTalents;
            */

        private Stats GetRaceStats(Character character)
        {
            int Lvl = character.Level; 
            Stats statsBase = new Stats()
            {
                Health = BaseHealth[Lvl],   // Base Health for a level 80 paladin. (tauren get 5% bonus on this value)
                Mana = BaseMana[Lvl],       // Base Mana for a level 80 paladin
                AttackPower = 240f,         // Base Attack Power for level 80  AP = (Lvl * 3f)
                PhysicalCrit = 0.032685f,   // Base PhysicalCrit for paladins
                SpellCrit = 0.03336f,       // Base SpellCrit for paladins
                Miss = 0.05f,               // Base Miss for a character with maxed out defense skill
                Dodge = 3.2685f,            // Base Dodge for a paladin
                Parry = 5f,                 // Base Parry for a chracter with maxed out defense skill
                Block = 5f,                 // Base Block for character with maxed out defense skill
            };
            statsBase.Strength   = ClassStats[3, 0] + LevelStats[3, 0];
            statsBase.Agility    = ClassStats[3, 1] + LevelStats[3, 1];
            statsBase.Stamina    = ClassStats[3, 2] + LevelStats[3, 2];
            statsBase.Intellect  = ClassStats[3, 3] + LevelStats[3, 3];
            statsBase.Spirit     = ClassStats[3, 4] + LevelStats[3, 4];
            switch (character.Race)
//              foreach (int Race = 0; in RaceStats)
//              {
//                  statsBase.Attribute += RaceStats[Race, Value];
//              };
                {
                case Character.CharacterRace.Human:

                    statsBase.Strength  += RaceStats[0, 0];
                    statsBase.Agility   += RaceStats[0, 1];
                    statsBase.Stamina   += RaceStats[0, 2];
                    statsBase.Intellect += RaceStats[0, 3];
                    statsBase.Spirit    += RaceStats[0, 4];

                    //The Human Spirit: Spirit increased by 3%.
                    statsBase.BonusSpiritMultiplier *= 1.0f + 0.03f;//TODO: need to research if wowwiki race spirit value already includes this. (20*1.03 = 21)
                    if ((character.MainHand != null) &&
                        ((character.MainHand.Item.Type == Item.ItemType.OneHandSword) ||
                         (character.MainHand.Item.Type == Item.ItemType.OneHandMace)))
                    {
                        statsBase.Expertise += 3f;
                    }
                    break;
                case Character.CharacterRace.Orc:

                    statsBase.Strength  += RaceStats[1, 0];
                    statsBase.Agility   += RaceStats[1, 1];
                    statsBase.Stamina   += RaceStats[1, 2];
                    statsBase.Intellect += RaceStats[1, 3];
                    statsBase.Spirit    += RaceStats[1, 4];

                    //Command : Damage done by Death Knight, Hunter and Warlock pets increased by 5%
                    //Axe Specialization : Expertise with One- and Two-handed Axes increased by 5.
                    if ((character.MainHand != null) &&
                        (character.MainHand.Item.Type == Item.ItemType.OneHandAxe))
                    {
                        statsBase.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.Dwarf:

                    statsBase.Strength  += RaceStats[2, 0];
                    statsBase.Agility   += RaceStats[2, 1];
                    statsBase.Stamina   += RaceStats[2, 2];
                    statsBase.Intellect += RaceStats[2, 3];
                    statsBase.Spirit    += RaceStats[2, 4];

                    //Frost Resistance : Reduces the chance you will be hit by Frost spells by 2%
                    //Gun Specialization : increases chance to critically hit with Guns by 1%
                    //Stoneform : Activate to gain immunity to poison, disease, and bleed (will also remove these types of debuffs); 
                    //            +10% Armor; Lasts 8 seconds. 3 minute cooldown.
                    if ((character.MainHand != null) &&
                        (character.MainHand.Item.Type == Item.ItemType.OneHandMace))
                    {
                        statsBase.Expertise += 5f;
                    }
                    break;
                case Character.CharacterRace.NightElf:

                    statsBase.Strength  += RaceStats[3, 0];
                    statsBase.Agility   += RaceStats[3, 1];
                    statsBase.Stamina   += RaceStats[3, 2];
                    statsBase.Intellect += RaceStats[3, 3];
                    statsBase.Spirit    += RaceStats[3, 4];

                    //Quickness : Reduces the chance that melee and ranged attackers will hit you by 2%.
                    statsBase.Miss += 0.02f;
                    //Nature Resistance : Reduces the chance you will be hit by Nature spells by 2%.
                    break;
                case Character.CharacterRace.Undead:

                    statsBase.Strength  += RaceStats[4, 0];
                    statsBase.Agility   += RaceStats[4, 1];
                    statsBase.Stamina   += RaceStats[4, 2];
                    statsBase.Intellect += RaceStats[4, 3];
                    statsBase.Spirit    += RaceStats[4, 4];

                    //Shadow Resistance : Reduces the chance you will be hit by Shadow spells by 2%.
                    break;
                case Character.CharacterRace.Tauren:

                    statsBase.Strength  += RaceStats[5, 0];
                    statsBase.Agility   += RaceStats[5, 1];
                    statsBase.Stamina   += RaceStats[5, 2];
                    statsBase.Intellect += RaceStats[5, 3];
                    statsBase.Spirit    += RaceStats[5, 4];

                    //Endurance : Base Health increased by 5%
                    statsBase.Health = (float)Math.Floor(statsBase.Health * 1.05f);// = 7280f
                    //Nature Resistance : Reduces the chance you will be hit by Nature spells by 2%.
                    break;
                case Character.CharacterRace.Gnome:

                    statsBase.Strength  += RaceStats[6, 0];
                    statsBase.Agility   += RaceStats[6, 1];
                    statsBase.Stamina   += RaceStats[6, 2];
                    statsBase.Intellect += RaceStats[6, 3];
                    statsBase.Spirit    += RaceStats[6, 4];

                    //Expansive Mind : Increase Intellect by 5%
                    statsBase.BonusIntellectMultiplier *= 1f + 0.05f;
                    //Arcane Resistance : Reduces the chance you will be hit by Arcane spells by 2%
                    break;
                case Character.CharacterRace.Troll:

                    statsBase.Strength  += RaceStats[7, 0];
                    statsBase.Agility   += RaceStats[7, 1];
                    statsBase.Stamina   += RaceStats[7, 2];
                    statsBase.Intellect += RaceStats[7, 3];
                    statsBase.Spirit    += RaceStats[7, 4];

                    //Regeneration : Increase health regeneration bonus by 10%. Also allows 10% of normal health regen during combat.
                    //Beast Slaying : 5% damage bonus when fighting against Beasts.
                    //Throwing Specialization : Increases chance to critically hit with Throwing Weapon by 1%. 
                    //Bow Specialization : Increase Bow critical strike chance by 1%.
                    break;
                case Character.CharacterRace.Draenei:

                    statsBase.Strength  += RaceStats[8, 0];
                    statsBase.Agility   += RaceStats[8, 1];
                    statsBase.Stamina   += RaceStats[8, 2];
                    statsBase.Intellect += RaceStats[8, 3];
                    statsBase.Spirit    += RaceStats[8, 4];

                    //Shadow Resistance : Reduces the chance you will be hit by Shadow spells by 2%.
                    //Heroic Presence : Increases chance to hit with all spells and attacks by 1% for you and all party members within 30 yards.
                    statsBase.PhysicalHit = 0.01f;
                    statsBase.SpellHit = 0.01f;
                    break;
                case Character.CharacterRace.BloodElf:

                    statsBase.Strength  += RaceStats[9, 0];
                    statsBase.Agility   += RaceStats[9, 1];
                    statsBase.Stamina   += RaceStats[9, 2];
                    statsBase.Intellect += RaceStats[9, 3];
                    statsBase.Spirit    += RaceStats[9, 4];

                    //TODO Magic Resistance : Reduces the chance you will be hit by spells by 2%. There's no definition for this in Stats.cs
                    break;
                default:
                    statsBase = new Stats();
                    break;
            }
            return statsBase;
        }
        #endregion

        public override Stats GetItemStats(Character character, Item additionalItem)
        {

            Stats statsItems = base.GetItemStats(character, additionalItem);
            AttackTable attackTable= new AttackTable(character, statsItems);
/*
            float abilityPerSecond = 1.0f / 6.0f; // one HotR every 6seconds
            float hitRate = 0.85f;
            

            //Mongoose
            if (character.MainHand != null && statsItems.MongooseProc > 0)
            {
                float procRate = 1.0f; // PPM
                float procDuration = 15.0f;
                float procPerSecond = (((procRate / 60.0f) * character.MainHand.Item.Speed) + ((procRate / 60.0f) * abilityPerSecond)) * hitRate;
                float procUptime = procDuration * procPerSecond;

                statsItems.Agility += 120.0f * procUptime;
                statsItems.HasteRating += (2.0f / ProtPaladin.HasteRatingToPhysicalHaste) * procUptime;
            }

            //Executioner
            if (character.MainHand != null && statsItems.ExecutionerProc > 0)
            {
                float procRate = 1.2f; // PPM
                float procDuration = 15.0f;
                float procPerSecond = (((procRate / 60.0f) * character.MainHand.Item.Speed) + ((procRate / 60.0f) * abilityPerSecond)) * hitRate;
                float procUptime = procDuration * procPerSecond;

                statsItems.ArmorPenetrationRating += 120.0f * procUptime;
            }
*/
/*
            // Libram of the Sacred Shield
            if (character.Ranged != null && character.Ranged.Item.Id == 45145)
                statsItems.ShieldOfRighteousnessBlockValue += 272.0f;
            // Tome of the Lightbringer
            if (character.Ranged != null && character.Ranged.Item.Id == 32368)
                statsItems.JudgementBlockValue += 186.0f;
            // Libram of Obstruction
            if (character.Ranged != null && character.Ranged.Item.Id == 40707)
                statsItems.JudgementBlockValue += 352.0f;
*/
            return statsItems;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            PaladinTalents talents = character.PaladinTalents;

            Stats statsBase = GetRaceStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);
            Stats statsItems = GetItemStats(character, additionalItem);
            Stats statsTalents = new Stats()
            {
                Parry = talents.Deflection * 1.0f,
                Dodge = talents.Anticipation * 1.0f,
                Block = (calcOpts.UseHolyShield ? talents.HolyShield * 30.0f : 0.0f),
                BonusBlockValueMultiplier = talents.Redoubt * 0.1f,
                BonusDamageMultiplier = (1.0f + talents.Crusade * 0.01f) *
                    (talents.OneHandedWeaponSpecialization > 0 ? 1.0f + talents.OneHandedWeaponSpecialization * .03f + .01f : 1.0f) - 1.0f,
                BonusStaminaMultiplier = (1.0f + talents.SacredDuty * 0.04f) * (1.0f + talents.CombatExpertise * 0.02f) - 1.0f,
                BonusIntellectMultiplier = talents.DivineIntellect * 0.03f,
                Expertise = talents.CombatExpertise * 2.0f,
                BaseArmorMultiplier = talents.Toughness * 0.02f,
                PhysicalCrit = (talents.CombatExpertise * 0.02f) + (talents.Conviction * 0.01f) + (talents.SanctityOfBattle * 0.01f),
                SpellCrit = (talents.CombatExpertise * 0.02f) + (talents.Conviction * 0.01f) + (talents.SanctityOfBattle * 0.01f),
                BonusStrengthMultiplier = talents.DivineStrength * 0.03f,
            };
            Stats statsGearEnchantsBuffs = statsItems + statsBuffs;
            Stats statsTotal = statsBase + statsItems + statsBuffs + statsTalents;

            statsTotal.Intellect = (float)Math.Floor(statsBase.Intellect * (1.0f + statsTalents.BonusIntellectMultiplier));
            statsTotal.Intellect += (float)Math.Floor((statsItems.Intellect + statsBuffs.Intellect) * (1.0f + statsTalents.BonusIntellectMultiplier));
            statsTotal.BaseAgility = statsBase.Agility + statsTalents.Agility;
            statsTotal.Stamina = (float)Math.Floor(statsBase.Stamina * (1.0f + statsTalents.BonusStaminaMultiplier));
            statsTotal.Stamina += (float)Math.Floor((statsItems.Stamina + statsBuffs.Stamina) * (1.0f + statsTalents.BonusStaminaMultiplier));
            statsTotal.Stamina = (float)Math.Floor(statsTotal.Stamina * (1.0f + statsBuffs.BonusStaminaMultiplier) * (1.0f + statsItems.BonusStaminaMultiplier));
            statsTotal.Strength = (float)Math.Floor((statsBase.Strength + statsTalents.Strength) * (1.0f + statsTotal.BonusStrengthMultiplier));
            statsTotal.Strength += (float)Math.Floor((statsItems.Strength + statsBuffs.Strength) * (1.0f + statsTotal.BonusStrengthMultiplier));
            if (statsTotal.GreatnessProc > 0)
            {
                statsTotal.Strength += (float)Math.Floor(statsTotal.GreatnessProc * 15.0f / 48.0f);
            }
            if (talents.GlyphOfSealOfVengeance && calcOpts.SealChoice == "Seal of Vengeance") 
            {
                statsTotal.Expertise += 10.0f;
            }
            statsTotal.Agility = (float)Math.Floor((statsBase.Agility + statsTalents.Agility) * (1.0f + statsTotal.BonusAgilityMultiplier));
            statsTotal.Agility += (float)Math.Floor((statsItems.Agility + statsBuffs.Agility) * (1.0f + statsTotal.BonusAgilityMultiplier));
            statsTotal.Health += (statsTotal.Stamina - 18f) * 10f;
            if (character.ActiveBuffsContains("Commanding Shout"))
                statsBuffs.Health += statsBuffs.BonusCommandingShoutHP;
            statsTotal.Health *= 1f + statsTotal.BonusHealthMultiplier;
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Mana *= 1f + statsTotal.BonusManaMultiplier;
            statsTotal.Armor *= 1f + statsTotal.BaseArmorMultiplier;
            statsTotal.Armor += 2f * (float)Math.Floor(statsTotal.Agility) + statsTotal.BonusArmor;
            statsTotal.Armor = (float)Math.Floor(statsTotal.Armor * (1f + statsTotal.BonusArmorMultiplier));
            statsTotal.AttackPower += (statsTotal.Strength - 20f) * 2f + 20f;
            statsTotal.AttackPower = (float)Math.Floor(statsTotal.AttackPower * (1f + statsTotal.BonusAttackPowerMultiplier));
            statsTotal.SpellPower += (float)Math.Floor(statsTotal.Stamina * talents.TouchedByTheLight * 0.1f);
            //statsTotal.SpellPower = (float)Math.Floor(statsTotal.SpellPower * (1f + statsTotal.BonusSpellPowerMultiplier));// not sure there's such a thing
            statsTotal.NatureResistance += statsTotal.NatureResistanceBuff + statsTotal.AllResist;
            statsTotal.FireResistance += statsTotal.FireResistanceBuff + statsTotal.AllResist;
            statsTotal.FrostResistance += statsTotal.FrostResistanceBuff + statsTotal.AllResist;
            statsTotal.ShadowResistance += statsTotal.ShadowResistanceBuff + statsTotal.AllResist;
            statsTotal.ArcaneResistance += statsTotal.ArcaneResistanceBuff + statsTotal.AllResist;
            statsTotal.BlockValue += (float)Math.Floor(statsTotal.Strength * ProtPaladin.StrengthToBlockValue - 10f);
            statsTotal.BlockValue = (float)Math.Floor(statsTotal.BlockValue * (1f + statsTotal.BonusBlockValueMultiplier));
            statsTotal.ArmorPenetration = statsBase.ArmorPenetration + statsGearEnchantsBuffs.ArmorPenetration;
            statsTotal.BonusCritMultiplier = statsBase.BonusCritMultiplier + statsGearEnchantsBuffs.BonusCritMultiplier;
            statsTotal.CritRating = statsBase.CritRating + statsGearEnchantsBuffs.CritRating;
            statsTotal.ExpertiseRating = statsBase.ExpertiseRating + statsGearEnchantsBuffs.ExpertiseRating;
            statsTotal.HasteRating = statsBase.HasteRating + statsGearEnchantsBuffs.HasteRating;
            statsTotal.HasteRating += statsGearEnchantsBuffs.HasteRatingOnPhysicalAttack * 10f / 45f;
            statsTotal.HitRating = statsBase.HitRating + statsGearEnchantsBuffs.HitRating;
            statsTotal.WeaponDamage += Lookup.WeaponDamage(character, statsTotal, false);
            //statsTotal.ExposeWeakness = statsBase.ExposeWeakness + statsGearEnchantsBuffs.ExposeWeakness; // Nerfed in 3.1

            float hitBonus = StatConversion.GetPhysicalHitFromRating(statsTotal.HitRating);
            float expertiseBonus = (StatConversion.GetExpertiseFromRating(statsTotal.ExpertiseRating) + statsTotal.Expertise) * 0.0025f;
            float chanceMiss = Math.Max(0f, 0.08f - hitBonus);;
            float chanceDodge = Math.Max(0f, 0.065f + .005f * (calcOpts.TargetLevel - 83) - expertiseBonus);
            float chanceParry = Math.Max(0f, 0.1375f - expertiseBonus);
            if (calcOpts.TargetLevel < 83) chanceMiss = Math.Max(0f, 0.05f + 0.005f * (calcOpts.TargetLevel - 80f) - hitBonus);
            float anyMiss = chanceMiss + chanceDodge + chanceParry;
            float chanceCrit = StatConversion.GetPhysicalCritFromRating(statsTotal.CritRating) +
                                        StatConversion.GetPhysicalCritFromAgility(statsTotal.Agility, Character.CharacterClass.Paladin) +
                                        statsTotal.PhysicalCrit - (0.006f * (calcOpts.TargetLevel - character.Level) + 
                                        (calcOpts.TargetLevel == 83 ? 0.03f : 0.0f));
            float triggerPhysicalCrit = chanceCrit * (1.0f - anyMiss);
            float triggerMeleeInterval = Lookup.WeaponSpeed(character, statsTotal);
            
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        statsTotal += effect.GetAverageStats(0f, 1f, 2.5f);
                        break;
                    case Trigger.MeleeHit:
                    case Trigger.PhysicalHit:
                        statsTotal += effect.GetAverageStats(triggerMeleeInterval, 1f, 2.5f);
                        break;
                    case Trigger.MeleeCrit:
                    case Trigger.PhysicalCrit:
                        statsTotal += effect.GetAverageStats(triggerMeleeInterval, statsTotal.PhysicalCrit, 2.5f);
                        break;
                    case Trigger.DoTTick:
                        statsTotal += effect.GetAverageStats(3f, 1f, 2.5f);
                        break;
                    case Trigger.DamageDone:
                        statsTotal += effect.GetAverageStats(triggerMeleeInterval / 2f, 1f, 2.5f);
                        break;
                    case Trigger.Judgement:
                        Stats test = new Stats();
                        statsTotal += effect.GetAverageStats((-talents.ImprovedJudgements *1.0f), 1.0f);
                        break;
                }
            }

            statsTotal.JudgementBlockValue *= (1f + statsTotal.BonusBlockValueMultiplier);
            statsTotal.JudgementBlockValue = (float)Math.Floor(statsTotal.JudgementBlockValue);
            if (calcOpts.UseHolyShield)
            {
                statsTotal.ShieldOfRighteousnessBlockValue *= (1f + statsTotal.BonusBlockValueMultiplier);
                statsTotal.ShieldOfRighteousnessBlockValue = (float)Math.Floor(statsTotal.ShieldOfRighteousnessBlockValue);
            }
            else
                statsTotal.ShieldOfRighteousnessBlockValue = 0.0f;

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            CharacterCalculationsProtPaladin calculations = GetCharacterCalculations(character) as CharacterCalculationsProtPaladin;

            switch (chartName)
            {
                #region Ability Damage/Threat
                case "Ability Damage":
                case "Ability Threat":
                    {
                        ComparisonCalculationBase[] comparisons = new ComparisonCalculationBase[calculations.Abilities.Count];
                        int j = 0;
                        foreach (System.Collections.DictionaryEntry abilities in calculations.Abilities)
                        {
                            AbilityModel ability = (AbilityModel)abilities.Value;
                            ComparisonCalculationProtPaladin comparison = new ComparisonCalculationProtPaladin();

                            comparison.Name = ability.Name;
                            if (chartName == "Ability Damage")
                                comparison.MitigationPoints = ability.Damage;
                            if (chartName == "Ability Threat")
                                comparison.ThreatPoints = ability.Threat;

                            comparison.OverallPoints = comparison.SurvivalPoints + comparison.ThreatPoints + comparison.MitigationPoints;
                            comparisons[j] = comparison;
                            j++;
                        }
                        return comparisons;
                    }
                #endregion
                #region Combat Table: Defensive Stats
                case "Combat Table: Defensive Stats":
                    {
                        ComparisonCalculationProtPaladin calcMiss = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcDodge = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcParry = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcBlock = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcCrit = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcCrush = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcHit = new ComparisonCalculationProtPaladin();
                        if (calculations != null)
                        {
                            calcMiss.Name  = "1 Miss";
                            calcDodge.Name = "2 Dodge";
                            calcParry.Name = "3 Parry";
                            calcBlock.Name = "4 Block";
                            calcCrit.Name  = "5 Crit";
                            calcCrush.Name = "6 Crush";
                            calcHit.Name   = "7 Hit";

                            calcMiss.OverallPoints = calcMiss.MitigationPoints = calculations.Miss * 100.0f;
                            calcDodge.OverallPoints = calcDodge.MitigationPoints = calculations.Dodge * 100.0f;
                            calcParry.OverallPoints = calcParry.MitigationPoints = calculations.Parry * 100.0f;
                            calcBlock.OverallPoints = calcBlock.MitigationPoints = calculations.Block * 100.0f;
                            calcCrit.OverallPoints = calcCrit.SurvivalPoints = calculations.CritVulnerability * 100.0f;
                            calcHit.OverallPoints = calcHit.SurvivalPoints = (1.0f - (calculations.DodgePlusMissPlusParryPlusBlock + calculations.CritVulnerability)) * 100.0f;
                        }
                        return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcBlock, calcCrit, calcCrush, calcHit };
                    }
                #endregion
                #region Combat Table: Offensive Stats
                case "Combat Table: Offensive Stats":
                    {
                        ComparisonCalculationProtPaladin calcMiss = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcDodge = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcParry = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcGlance = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcBlock = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcCrit = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcHit = new ComparisonCalculationProtPaladin();
                        if (calculations != null)
                        {
                            calcMiss.Name   = "1 Miss";
                            calcDodge.Name  = "2 Dodge";
                            calcParry.Name  = "3 Parry";
                            calcGlance.Name = "4 Glancing";
                            calcBlock.Name  = "5 Block";
                            calcCrit.Name   = "6 Crit";
                            calcHit.Name    = "7 Hit";

                            calcMiss.OverallPoints = calcMiss.MitigationPoints = calculations.MissedAttacks * 100.0f;
                            calcDodge.OverallPoints = calcDodge.MitigationPoints = calculations.DodgedAttacks * 100.0f;
                            calcParry.OverallPoints = calcParry.MitigationPoints = calculations.ParriedAttacks * 100.0f;
                            calcGlance.OverallPoints = calcGlance.MitigationPoints = calculations.GlancingAttacks * 100.0f;
                            calcBlock.OverallPoints = calcBlock.MitigationPoints = calculations.BlockedAttacks * 100.0f;
                            calcCrit.OverallPoints = calcCrit.SurvivalPoints = calculations.Crit * 100.0f;
                            calcHit.OverallPoints = calcHit.SurvivalPoints = (1.0f - (calculations.AvoidedAttacks + calculations.GlancingAttacks + calculations.BlockedAttacks + calculations.Crit)) * 100.0f;
                        }
                        return new ComparisonCalculationBase[] { calcMiss, calcDodge, calcParry, calcGlance, calcBlock, calcCrit, calcHit };
                    }
                #endregion
                #region Item Budget
                case "Item Budget":
                    CharacterCalculationsProtPaladin calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcDodgeValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DodgeRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcParryValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ParryRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcBlockValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcExpertiseValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { ExpertiseRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHitValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 10f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcBlockValueValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { BlockValue = 10f / 0.65f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcHealthValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Health = (10f * 10f) / 0.667f } }) as CharacterCalculationsProtPaladin;
                    CharacterCalculationsProtPaladin calcResilValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 10f } }) as CharacterCalculationsProtPaladin;

                    //Differential Calculations for Agi
                    CharacterCalculationsProtPaladin calcAtAdd = calcBaseValue;
                    float agiToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && agiToAdd < 2)
                    {
                        agiToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    CharacterCalculationsProtPaladin calcAtSubtract = calcBaseValue;
                    float agiToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && agiToSubtract > -2)
                    {
                        agiToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Agility = agiToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    agiToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonAgi = new ComparisonCalculationProtPaladin()
                    {
                        Name = "10 Agility",
                        OverallPoints = 10 * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (agiToAdd - agiToSubtract),
                        MitigationPoints = 10 * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (agiToAdd - agiToSubtract),
                        SurvivalPoints = 10 * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (agiToAdd - agiToSubtract),
                        ThreatPoints = 10 * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (agiToAdd - agiToSubtract)
                    };

                    //Differential Calculations for Str
                    calcAtAdd = calcBaseValue;
                    float strToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && strToAdd <= 22)
                    {
                        strToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float strToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && strToSubtract >= -22)
                    {
                        strToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Strength = strToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    strToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonStr = new ComparisonCalculationProtPaladin()
                    {
                        Name = "10 Strength",
                        OverallPoints = 10 * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (strToAdd - strToSubtract),
                        MitigationPoints = 10 * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (strToAdd - strToSubtract),
                        SurvivalPoints = 10 * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (strToAdd - strToSubtract),
                        ThreatPoints = 10 * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (strToAdd - strToSubtract)
                    };


                    //Differential Calculations for Def
                    calcAtAdd = calcBaseValue;
                    float defToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && defToAdd < 20)
                    {
                        defToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float defToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && defToSubtract > -20)
                    {
                        defToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { DefenseRating = defToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    defToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonDef = new ComparisonCalculationProtPaladin()
                    {
                        Name = "10 Defense Rating",
                        OverallPoints = 10 * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (defToAdd - defToSubtract),
                        MitigationPoints = 10 * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (defToAdd - defToSubtract),
                        SurvivalPoints = 10 * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (defToAdd - defToSubtract),
                        ThreatPoints = 10 * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (defToAdd - defToSubtract)
                    };


                    //Differential Calculations for AC
                    calcAtAdd = calcBaseValue;
                    float acToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && acToAdd < 2)
                    {
                        acToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float acToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && acToSubtract > -2)
                    {
                        acToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Armor = acToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    acToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonAC = new ComparisonCalculationProtPaladin()
                    {
                        Name = "100 Armor",
                        OverallPoints = 100f * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (acToAdd - acToSubtract),
                        MitigationPoints = 100f * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (acToAdd - acToSubtract),
                        SurvivalPoints = 100f * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (acToAdd - acToSubtract),
                        ThreatPoints = 100f * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (acToAdd - acToSubtract),
                    };


                    //Differential Calculations for Sta
                    calcAtAdd = calcBaseValue;
                    float staToAdd = 0f;
                    while (calcBaseValue.OverallPoints == calcAtAdd.OverallPoints && staToAdd < 2)
                    {
                        staToAdd += 0.01f;
                        calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToAdd } }) as CharacterCalculationsProtPaladin;
                    }

                    calcAtSubtract = calcBaseValue;
                    float staToSubtract = 0f;
                    while (calcBaseValue.OverallPoints == calcAtSubtract.OverallPoints && staToSubtract > -2)
                    {
                        staToSubtract -= 0.01f;
                        calcAtSubtract = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = staToSubtract } }) as CharacterCalculationsProtPaladin;
                    }
                    staToSubtract += 0.01f;

                    ComparisonCalculationProtPaladin comparisonSta = new ComparisonCalculationProtPaladin()
                    {
                        Name = "15 Stamina",
                        OverallPoints = (10f / 0.667f) * (calcAtAdd.OverallPoints - calcBaseValue.OverallPoints) / (staToAdd - staToSubtract),
                        MitigationPoints = (10f / 0.667f) * (calcAtAdd.MitigationPoints - calcBaseValue.MitigationPoints) / (staToAdd - staToSubtract),
                        SurvivalPoints = (10f / 0.667f) * (calcAtAdd.SurvivalPoints - calcBaseValue.SurvivalPoints) / (staToAdd - staToSubtract),
                        ThreatPoints = (10f / 0.667f) * (calcAtAdd.ThreatPoints - calcBaseValue.ThreatPoints) / (staToAdd - staToSubtract)
                    };

                    return new ComparisonCalculationBase[] { 
                        comparisonStr,
						comparisonAgi,
						comparisonAC,
						comparisonSta,
						comparisonDef,
						new ComparisonCalculationProtPaladin() { Name = "10 Dodge Rating",
                            OverallPoints = (calcDodgeValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcDodgeValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcDodgeValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcDodgeValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Parry Rating",
                            OverallPoints = (calcParryValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcParryValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcParryValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcParryValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Block Rating",
                            OverallPoints = (calcBlockValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcBlockValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Haste Rating",
                            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHasteValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHasteValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHasteValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Expertise Rating",
                            OverallPoints = (calcExpertiseValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcExpertiseValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcExpertiseValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcExpertiseValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "10 Hit Rating",
                            OverallPoints = (calcHitValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHitValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHitValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHitValue.ThreatPoints - calcBaseValue.ThreatPoints)},
                        new ComparisonCalculationProtPaladin() { Name = "15.38 Block Value",
                            OverallPoints = (calcBlockValueValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcBlockValueValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcBlockValueValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcBlockValueValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationProtPaladin() { Name = "150 Health",
                            OverallPoints = (calcHealthValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcHealthValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcHealthValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcHealthValue.ThreatPoints - calcBaseValue.ThreatPoints)},
						new ComparisonCalculationProtPaladin() { Name = "10 Resilience",
                            OverallPoints = (calcResilValue.OverallPoints - calcBaseValue.OverallPoints), 
							MitigationPoints = (calcResilValue.MitigationPoints - calcBaseValue.MitigationPoints),
                            SurvivalPoints = (calcResilValue.SurvivalPoints - calcBaseValue.SurvivalPoints),
                            ThreatPoints = (calcResilValue.ThreatPoints - calcBaseValue.ThreatPoints)},
					};
                #endregion 
                #region Spell Resistance
                case "Combat Table: Spell Resistance":
                    {
                        ComparisonCalculationProtPaladin calcSpellRes0   = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes10  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes20  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes30  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes40  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes50  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes60  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes70  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes80  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes90  = new ComparisonCalculationProtPaladin();
                        ComparisonCalculationProtPaladin calcSpellRes100 = new ComparisonCalculationProtPaladin();
                        
                        if (calculations != null)
                        {
                            calcSpellRes0.Name   = "0.00";
                            calcSpellRes10.Name  = "0.10";
                            calcSpellRes20.Name  = "0.20";
                            calcSpellRes30.Name  = "0.30";
                            calcSpellRes40.Name  = "0.40";
                            calcSpellRes50.Name  = "0.50";
                            calcSpellRes60.Name  = "0.60";
                            calcSpellRes70.Name  = "0.70";
                            calcSpellRes80.Name  = "0.80";
                            calcSpellRes90.Name  = "0.90";
                            calcSpellRes100.Name = "1.00";

                            calcSpellRes0.OverallPoints   = calcSpellRes0.MitigationPoints = calculations.ResistanceTable[0] * 100.0f;
                            calcSpellRes10.OverallPoints  = calcSpellRes10.MitigationPoints = calculations.ResistanceTable[1] * 100.0f;
                            calcSpellRes20.OverallPoints  = calcSpellRes20.MitigationPoints = calculations.ResistanceTable[2] * 100.0f;
                            calcSpellRes30.OverallPoints  = calcSpellRes30.MitigationPoints = calculations.ResistanceTable[3] * 100.0f;
                            calcSpellRes40.OverallPoints  = calcSpellRes40.MitigationPoints = calculations.ResistanceTable[4] * 100.0f;
                            calcSpellRes50.OverallPoints  = calcSpellRes50.MitigationPoints = calculations.ResistanceTable[5] * 100.0f;
                            calcSpellRes60.OverallPoints  = calcSpellRes60.MitigationPoints = calculations.ResistanceTable[6] * 100.0f;
                            calcSpellRes70.OverallPoints  = calcSpellRes70.MitigationPoints = calculations.ResistanceTable[7] * 100.0f;
                            calcSpellRes80.OverallPoints  = calcSpellRes80.MitigationPoints = calculations.ResistanceTable[8] * 100.0f;
                            calcSpellRes90.OverallPoints  = calcSpellRes90.MitigationPoints = calculations.ResistanceTable[9] * 100.0f;
                            calcSpellRes100.OverallPoints = calcSpellRes100.MitigationPoints = calculations.ResistanceTable[10] * 100.0f;
                        }
                        return new ComparisonCalculationBase[] { calcSpellRes0, calcSpellRes10, calcSpellRes20, calcSpellRes30, calcSpellRes40, calcSpellRes50, calcSpellRes60, calcSpellRes70, calcSpellRes80, calcSpellRes90, calcSpellRes100 };
                    }
                #endregion
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        #region Relevancy Methods
        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        {
            // Filters out Non-Shield Offhand Enchants and Ranged Enchants
            if ((slot == Item.ItemSlot.OffHand && enchant.Slot != Item.ItemSlot.OffHand) || slot == Item.ItemSlot.Ranged) return false;
            // Filters out Death Knight and Two-Hander Enchants
            if (enchant.Name.StartsWith("Rune of the") || enchant.Slot == Item.ItemSlot.TwoHand) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
            {
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                AverageArmor = stats.AverageArmor,
                Stamina = stats.Stamina,
                Agility = stats.Agility,
                DodgeRating = stats.DodgeRating,
                ParryRating = stats.ParryRating,
                BlockRating = stats.BlockRating,
                BlockValue = stats.BlockValue,
                DefenseRating = stats.DefenseRating,
                Resilience = stats.Resilience,
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                Health = stats.Health,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                DamageTakenMultiplier = stats.DamageTakenMultiplier,
                Miss = stats.Miss,
                CritChanceReduction = stats.CritChanceReduction,
                AllResist = stats.AllResist,
                ArcaneResistance = stats.ArcaneResistance,
                NatureResistance = stats.NatureResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                ShadowResistance = stats.ShadowResistance,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,

                Strength = stats.Strength,
                AttackPower = stats.AttackPower,
                SpellPower = stats.SpellPower,
                CritRating = stats.CritRating,
                PhysicalCrit = stats.PhysicalCrit,
                SpellCrit = stats.SpellCrit,
                HitRating = stats.HitRating,
                PhysicalHit = stats.PhysicalHit,
                SpellHit = stats.SpellHit,
                HasteRating = stats.HasteRating,
                PhysicalHaste = stats.PhysicalHaste,
                ExpertiseRating = stats.ExpertiseRating,
                ArmorPenetration = stats.ArmorPenetration,
                ArmorPenetrationRating = stats.ArmorPenetrationRating,
                WeaponDamage = stats.WeaponDamage,
                BonusCritMultiplier = stats.BonusCritMultiplier,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusBlockValueMultiplier = stats.BonusBlockValueMultiplier,

                MongooseProc = stats.MongooseProc,
                MongooseProcAverage = stats.MongooseProcAverage,
                MongooseProcConstant = stats.MongooseProcConstant,

                ExecutionerProc = stats.ExecutionerProc,

                JudgementBlockValue = stats.JudgementBlockValue,
                BonusHammerOfTheRighteousMultiplier = stats.BonusHammerOfTheRighteousMultiplier,
                ShieldOfRighteousnessBlockValue = stats.ShieldOfRighteousnessBlockValue,
                BonusSealOfCorruptionDamageMultiplier = stats.BonusSealOfCorruptionDamageMultiplier,
                BonusSealOfRighteousnessDamageMultiplier = stats.BonusSealOfRighteousnessDamageMultiplier,
                BonusSealOfVengeanceDamageMultiplier = stats.BonusSealOfVengeanceDamageMultiplier,
                ConsecrationSpellPower = stats.ConsecrationSpellPower,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.DoTTick
                    || effect.Trigger == Trigger.DamageDone || effect.Trigger == Trigger.Judgement)
                {
                    if (HasRelevantStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                    }
                }
            }
            return s;
        }

        public override bool IsItemRelevant(Item item)
        {
            try
            {
                return (string.IsNullOrEmpty(item.RequiredClasses) || item.RequiredClasses.Replace(" ", "").Contains(TargetClass.ToString())) &&
                    (RelevantItemTypes.Contains(item.Type)) && (HasRelevantStats(item.Stats) ||
                    (((item.Slot == Item.ItemSlot.Trinket) || (item.IsGem)) && (hasRelevantTrinketGemStats(item.Stats)))) ||
                    (item.Slot == Item.ItemSlot.Ranged) && (item.Type == Item.ItemType.Libram);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool hasRelevantTrinketGemStats(Stats stats)
        {
            return (
                //Trinket+Gem Stats
                stats.Strength +
                stats.Stamina +
                //Trinket Stats
                stats.BlockValue +
                //Gem Stats
                stats.BonusArmorMultiplier +
                stats.BonusBlockValueMultiplier +
                //Tanking Stats
                stats.Resilience +  
                stats.ParryRating + 
                stats.BlockRating + 
                stats.BlockValue + 
                stats.DefenseRating + 
                stats.DodgeRating +
                stats.Resilience +
                //Threat Stats
                stats.CritRating +
                stats.HitRating +
                stats.ExpertiseRating
                ) != 0;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool relevant = (
                stats.DefenseRating +
                stats.DodgeRating +
                stats.ParryRating +
                stats.BlockRating +

                stats.BonusArmor +
                stats.Health +
                stats.ArmorPenetrationRating +
                stats.ArmorPenetration +

                stats.JudgementBlockValue +
                stats.ShieldOfRighteousnessBlockValue +
                stats.ConsecrationSpellPower +
 
                //// Resistances
                stats.AllResist +
                stats.ArcaneResistance + 
                stats.NatureResistance + 
                stats.FireResistance +
                stats.FrostResistance + 
                stats.ShadowResistance + 
                stats.ArcaneResistanceBuff +
                stats.NatureResistanceBuff + 
                stats.FireResistanceBuff +
                stats.FrostResistanceBuff + 
                stats.ShadowResistanceBuff 
                ) != 0;

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.Judgement)
                {
                    relevant |= HasRelevantStats(effect.Stats);
                    if (relevant) break;
                }
            }
            return relevant;
        }

        public override bool IsBuffRelevant(Buff buff)
        {
            Stats stats = buff.Stats;
            bool hasRelevantBuffStats = (
                stats.Stamina +
                stats.Strength +
                stats.Agility +
                stats.BonusArmor +
                stats.Health +

                stats.PhysicalCrit +
                stats.SpellCrit +
                stats.PhysicalHaste +
                stats.SpellHaste +
                stats.AttackPower +
                stats.SpellPower +
                stats.CritRating +
                stats.HitRating +
                stats.PhysicalHit +
                stats.SpellHit +
                stats.Miss +
                stats.ArmorPenetration +

                stats.BonusStrengthMultiplier +
                stats.BonusAgilityMultiplier +
                stats.BonusStaminaMultiplier +
                stats.BonusHealthMultiplier +

                stats.ThreatIncreaseMultiplier +
                stats.BonusArmorMultiplier +
                stats.BonusAttackPowerMultiplier +
                stats.BonusDamageMultiplier +
                stats.BonusHolyDamageMultiplier +
                stats.BaseArmorMultiplier +
                stats.DamageTakenMultiplier +

                stats.AllResist +
                stats.ArcaneResistance +
                stats.NatureResistance +
                stats.FireResistance +
                stats.FrostResistance +
                stats.ShadowResistance +
                stats.ArcaneResistanceBuff +
                stats.NatureResistanceBuff +
                stats.FireResistanceBuff +
                stats.FrostResistanceBuff +
                stats.ShadowResistanceBuff +

                stats.BonusHammerOfTheRighteousMultiplier +
                stats.ConsecrationSpellPower +
                stats.ShieldOfRighteousnessBlockValue +
                stats.BonusSealOfCorruptionDamageMultiplier +
                stats.BonusSealOfRighteousnessDamageMultiplier +
                stats.BonusSealOfVengeanceDamageMultiplier +

                stats.BossAttackSpeedMultiplier
                ) != 0;

            bool notClassSetBonus = ((buff.Group == "Set Bonuses") && !((buff.Name.Contains("Aegis")) || buff.Name.Contains("Redemption")));

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.MeleeCrit || effect.Trigger == Trigger.MeleeHit
                    || effect.Trigger == Trigger.PhysicalCrit || effect.Trigger == Trigger.PhysicalHit || effect.Trigger == Trigger.Judgement)
                {
                    hasRelevantBuffStats |= HasRelevantStats(effect.Stats);
                    if (hasRelevantBuffStats && !notClassSetBonus) break;
                }
            }

            return hasRelevantBuffStats && !notClassSetBonus;
        }

        public override bool IsEnchantRelevant(Enchant enchant)
        {
            Stats stats = enchant.Stats;

            bool hasRelevantStats = (

                stats.Strength +
                stats.Stamina +
                stats.Agility + 
				stats.Health +

                stats.BlockValue +

                stats.BonusArmor +
                stats.ThreatIncreaseMultiplier +

                //Tanking Stats
                stats.Resilience +
                stats.ParryRating +
                stats.BlockRating +
                stats.BlockValue +
                stats.DefenseRating +
                stats.DodgeRating +
                stats.Resilience +

                //Threat Stats
                stats.CritRating +
                stats.HitRating +
                stats.ExpertiseRating +

                //Enchant Procs
                stats.MongooseProc +
                stats.ExecutionerProc
                ) != 0;

            return hasRelevantStats;
        }
        #endregion

 //       public bool debugMode(Character character)
 //       {
 //           CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
 //           return calcOpts.debugMode;
 //       }

        /// <summary>
        /// Saves the talents for the character
        /// </summary>
        /// <param name="character">The character for whom the talents should be saved</param>
        public void GetTalents(Character character)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            calcOpts.talents = character.PaladinTalents;
        }
    }


    public class ProtPaladin
    {
        public static readonly float AgilityToDodge = 1.0f / 52.08333333f;
        public static readonly float DodgeRatingToDodge = 1.0f / 39.34798813f;
        public static readonly float StrengthToAP = 2.0f;
        public static readonly float StrengthToBlockValue = 1.0f / 2.0f;
        public static readonly float AgilityToArmor = 2.0f;
        public static readonly float AgilityToCrit = 1.0f / 52.08333333f;
        public static readonly float IntellectToCrit = 1.0f / 166.6666709f;
        public static readonly float StaminaToHP = 10.0f;
        public static readonly float HitRatingToHit = 1.0f / 32.78998947f;
        public static readonly float HitRatingToSpellHit = 1.0f / 26.23199272f;
        public static readonly float CritRatingToCrit = 1.0f / 45.90598679f;
        public static readonly float HasteRatingToSpellHaste = 1.0f / 32.78998947f;
        public static readonly float HasteRatingToPhysicalHaste = 1.0f / 25.22306882f;    // 3.1 Hybrid haste buff
        public static readonly float ExpertiseRatingToExpertise = 1.0f / (32.78998947f / 4f);
        public static readonly float ExpertiseToDodgeParryReduction = 0.25f;
        public static readonly float DefenseRatingToDefense = 1.0f / 4.918498039f;
        public static readonly float DefenseToDodge = 0.04f;
        public static readonly float DefenseToBlock = 0.04f;
        public static readonly float DefenseToParry = 0.04f;
        public static readonly float DefenseToMiss = 0.04f;
        public static readonly float DefenseToCritReduction = 0.04f;
        public static readonly float DefenseToDazeReduction = 0.04f;
        public static readonly float ParryRatingToParry = 1.0f / 49.18498611f;
        public static readonly float BlockRatingToBlock = 1.0f / 16.39499474f;
        public static readonly float ResilienceRatingToCritReduction = 1.0f / 81.97497559f;
        public static readonly float ArPToArmorPenetration = 1.0f / 12.3162384f;
    }
}