using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", Character.CharacterClass.Priest)]
    public class CalculationsShadowPriest : CalculationsBase 
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Priest; } }

        private string _currentChartName = null;
        private float _currentChartTotal = 0;

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                switch (_currentChartName)
                {
                    case "Mana Sources":
                        _subPointNameColors.Add(string.Format("MP5 Sources ({0} Total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    case "DPS Sources":
                        _subPointNameColors.Add(string.Format("DPS Sources ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Red);
                        break;
                    case "Mana Usage":
                        _subPointNameColors.Add(string.Format("Mana Usage ({0} total)", _currentChartTotal.ToString("0")), System.Drawing.Color.Blue);
                        break;
                    default:
                        _subPointNameColors.Add("DPS-Burst", System.Drawing.Color.Red);
                        _subPointNameColors.Add("DPS-Sustained", System.Drawing.Color.Blue);
                        _subPointNameColors.Add("Survivability", System.Drawing.Color.Green);
                        break;
                }
                _currentChartName = null;
                return _subPointNameColors;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Spell Power",
					"Basic Stats:Regen",
					"Basic Stats:Crit",
					"Basic Stats:Hit",
					"Basic Stats:Haste",
                    "Simulation:Rotation",
                    "Simulation:DPS",
                    "Simulation:SustainDPS",
                    "Shadow:Vampiric Touch",
                    "Shadow:SW Pain",
                    "Shadow:Devouring Plague",
				    "Shadow:SW Death",
                    "Shadow:Mind Blast",
                    "Shadow:Mind Flay",
                    "Shadow:Vampiric Embrace",
                    "Holy:PW Shield",
                    "Holy:Smite",
                    "Holy:Holy Fire",
                    "Holy:Penance"
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelShadowPriest();
                }
                return _calculationOptionsPanel;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Stat Values", "Mana Sources", "DPS Sources", "Mana Usage" };
                return _customChartNames;
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationShadowPriest(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsShadowPriest(); }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Dagger,
                        Item.ItemType.Wand,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;

            _currentChartTotal = 0;
            _currentChartName = chartName;

            switch (chartName)
            {
                case "Mana Sources":
                    CharacterCalculationsShadowPriest mscalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    SolverBase mssolver = mscalcs.GetSolver(character, mscalcs.BasicStats);
                    mssolver.Calculate(mscalcs);
                    foreach (SolverBase.ManaSource Source in mssolver.ManaSources)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = Source.Name;
                        comparison.SubPoints[0] = Source.Value * 5;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "DPS Sources":
                    CharacterCalculationsShadowPriest dpscalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    SolverBase dpssolver = dpscalcs.GetSolver(character, dpscalcs.BasicStats);
                    dpssolver.Calculate(dpscalcs);
                    foreach (Spell spell in dpssolver.SpellPriority)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.SubPoints[0] = spell.SpellStatistics.DamageDone;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Mana Usage":
                    CharacterCalculationsShadowPriest mucalcs = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    SolverBase musolver = mucalcs.GetSolver(character, mucalcs.BasicStats);
                    musolver.Calculate(mucalcs);
                    foreach (Spell spell in musolver.SpellPriority)
                    {
                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = spell.Name;
                        comparison.SubPoints[0] = spell.SpellStatistics.ManaUsed * 5;
                        _currentChartTotal += comparison.SubPoints[0];
                        comparison.OverallPoints = comparison.SubPoints[0];
                        comparison.Equipped = false;
                        comparisonList.Add(comparison);
                    }
                    return comparisonList.ToArray();
                case "Stat Values":
                    CharacterCalculationsShadowPriest calcsBase = GetCharacterCalculations(character) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsIntellect = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSpirit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsMP5 = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSpellPower = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellPower = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsHaste = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HasteRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsCrit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { CritRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsHit = GetCharacterCalculations(character, new Item() { Stats = new Stats() { HitRating = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsSta = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Stamina = 50 } }) as CharacterCalculationsShadowPriest;
                    CharacterCalculationsShadowPriest calcsRes = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Resilience = 50 } }) as CharacterCalculationsShadowPriest;

                    return new ComparisonCalculationBase[] {
                        new ComparisonCalculationShadowPriest() { Name = "1 Intellect",
                            OverallPoints = (calcsIntellect.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsIntellect.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsIntellect.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsIntellect.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Spirit",
                            OverallPoints = (calcsSpirit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSpirit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSpirit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSpirit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 MP5",
                            OverallPoints = (calcsMP5.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsMP5.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsMP5.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsMP5.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Spell Power",
                            OverallPoints = (calcsSpellPower.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSpellPower.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSpellPower.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSpellPower.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Haste",
                            OverallPoints = (calcsHaste.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsHaste.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsHaste.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsHaste.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Crit",
                            OverallPoints = (calcsCrit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsCrit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsCrit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsCrit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Hit",
                            OverallPoints = (calcsHit.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsHit.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsHit.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsHit.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Stamina",
                            OverallPoints = (calcsSta.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsSta.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsSta.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsSta.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        },
                        new ComparisonCalculationShadowPriest() { Name = "1 Resilience",
                            OverallPoints = (calcsRes.OverallPoints - calcsBase.OverallPoints) / 50,
                            DpsPoints = (calcsRes.SubPoints[0] - calcsBase.SubPoints[0]) / 50,
                            SustainPoints = (calcsRes.SubPoints[1] - calcsBase.SubPoints[1]) / 50,
                            SurvivalPoints = (calcsRes.SubPoints[2] - calcsBase.SubPoints[2]) / 50
                        }};
                default:
                    //_customChartNames = null;
                    _currentChartName = null;
                    return new ComparisonCalculationBase[0];
            }
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            Stats stats = GetCharacterStats(character, additionalItem);
            Stats statsRace = GetRaceStats(character);
            CharacterCalculationsShadowPriest calculatedStats = new CharacterCalculationsShadowPriest();
            CalculationOptionsShadowPriest calculationOptions = character.CalculationOptions as CalculationOptionsShadowPriest;
            StatConversion sc = new StatConversion(character);

            calculatedStats.Race = character.Race;
            calculatedStats.BasicStats = stats;
            calculatedStats.Character = character;
            
            calculatedStats.SpiritRegen = (float)Math.Floor(5f * sc.GetSpiritRegenSec(calculatedStats.BasicStats.Spirit, calculatedStats.BasicStats.Intellect));
            calculatedStats.RegenInFSR = calculatedStats.SpiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5;
            calculatedStats.RegenOutFSR = calculatedStats.SpiritRegen + calculatedStats.BasicStats.Mp5;

            SolverBase solver = calculatedStats.GetSolver(character, stats);
            solver.Calculate(calculatedStats);

            calculatedStats.DpsPoints = solver.DPS;
            calculatedStats.SustainPoints = solver.SustainDPS;
            // If opponent has 25% crit, each 39.42308044 resilience gives -1% damage from dots and -1% chance to be crit. Also reduces crits by 2%.
            // This effectively means you gain 12.5% extra health from removing 12.5% dot and 12.5% crits at resilience cap (492.5 (39.42308044*12.5))
            // In addition, the remaining 12.5% crits are reduced by 25% (12.5%*200%damage*75% = 18.75%)
            // At resilience cap I'd say that your hp's are scaled by 1.125*1.1875 = ~30%. Probably wrong but good enough.
            calculatedStats.SurvivalPoints = calculatedStats.BasicStats.Health * calculationOptions.Survivability / 100f * (1 + 0.3f * calculatedStats.BasicStats.Resilience / 492.7885f);
            calculatedStats.OverallPoints = calculatedStats.DpsPoints + calculatedStats.SustainPoints + calculatedStats.SurvivalPoints;

            return calculatedStats;
        }

        public class StatConversion
        {
            // Numbers reverse engineered by Whitetooth (hotdogee [at] gmail [dot] com)
            private int _Level = 80;

            #region Combat Rating
            public enum RatingType
            {
                ArmorPenetration = 0,
                Block,                
                Crit,
                Defense,
                Dodge,
                Expertise,
                Haste,
                Hit,
                Parry,
                Resilience,
                SpellCrit,
                SpellHaste,
                SpellHit
            }

            private Dictionary<RatingType, float> RatingConversionBase60 = new Dictionary<RatingType, float>();
            private Dictionary<RatingType, List<float>> RatingConversionTable = new Dictionary<RatingType, List<float>>();

            public float GetArmorPenetrationFromRating(float ARRating, int Level) { return ARRating * RatingConversionTable[RatingType.ArmorPenetration][Level]; }
            public float GetArmorPenetrationFromRating(float ARRating) { return GetArmorPenetrationFromRating(ARRating, _Level); }

            public float GetBlockFromRating(float BlockRating, int Level) { return BlockRating * RatingConversionTable[RatingType.Block][Level]; }
            public float GetBlockFromRating(float BlockRating) { return GetBlockFromRating(BlockRating, _Level); }

            public float GetCritFromRating(float CritRating, int Level) { return CritRating * RatingConversionTable[RatingType.Crit][Level]; }
            public float GetCritFromRating(float CritRating) { return GetCritFromRating(CritRating, _Level); }

            public float GetDefenseFromRating(float DefenseRating, int Level) { return DefenseRating * RatingConversionTable[RatingType.Defense][Level]; }
            public float GetDefenseFromRating(float DefenseRating) { return GetDefenseFromRating(DefenseRating, _Level); }

            public float GetDodgeFromRating(float DodgeRating, int Level) { return DodgeRating * RatingConversionTable[RatingType.Dodge][Level]; }
            public float GetDodgeFromRating(float DodgeRating) { return GetDodgeFromRating(DodgeRating, _Level); }

            public float GetExpertiseFromRating(float ExpertiseRating, int Level) { return ExpertiseRating * RatingConversionTable[RatingType.Expertise][Level]; }
            public float GetExpertiseFromRating(float ExpertiseRating) { return GetExpertiseFromRating(ExpertiseRating, _Level); }

            public float GetHasteFromRating(float HasteRating, int Level) { return HasteRating * RatingConversionTable[RatingType.Haste][Level]; }
            public float GetHasteFromRating(float HasteRating) { return GetHasteFromRating(HasteRating, _Level); }

            public float GetHitFromRating(float HitRating, int Level) { return HitRating * RatingConversionTable[RatingType.Hit][Level]; }
            public float GetHitFromRating(float HitRating) { return GetHitFromRating(HitRating, _Level); }

            public float GetParryFromRating(float ParryRating, int Level) { return ParryRating * RatingConversionTable[RatingType.Parry][Level]; }
            public float GetParryFromRating(float ParryRating) { return GetParryFromRating(ParryRating, _Level); }

            public float GetResilienceFromRating(float ResilienceRating, int Level) { return ResilienceRating * RatingConversionTable[RatingType.Resilience][Level]; }
            public float GetResilienceFromRating(float ResilienceRating) { return GetResilienceFromRating(ResilienceRating, _Level); }

            public float GetSpellCritFromRating(float SpellCritRating, int Level) { return SpellCritRating * RatingConversionTable[RatingType.SpellCrit][Level]; }
            public float GetSpellCritFromRating(float SpellCritRating) { return GetSpellHasteFromRating(SpellCritRating, _Level); }

            public float GetSpellHasteFromRating(float SpellHasteRating, int Level) { return SpellHasteRating * RatingConversionTable[RatingType.SpellHaste][Level]; }
            public float GetSpellHasteFromRating(float SpellHasteRating) { return GetSpellHasteFromRating(SpellHasteRating, _Level); }

            public float GetSpellHitFromRating(float SpellHitRating, int Level) { return SpellHitRating * RatingConversionTable[RatingType.SpellHit][Level]; }
            public float GetSpellHitFromRating(float SpellHitRating) { return GetSpellCritFromRating(SpellHitRating, _Level); }
            #endregion

            #region Stat Rating
            public enum StatType
            {
                AgilityToCrit = 0,
                AgilityToDodge,
                IntellectToCrit,
            }
            private Dictionary<StatType, List<float>> StatConversionTable = new Dictionary<StatType, List<float>>();

            public float GetCritFromIntellect(float Intellect, int Level) { return Intellect * StatConversionTable[StatType.IntellectToCrit][Level]; }
            public float GetCritFromIntellect(float Intellect) { return GetCritFromIntellect(Intellect, _Level); }

            public float GetCritFromAgility(float Agility, int Level) { return Agility * StatConversionTable[StatType.AgilityToCrit][Level]; }
            public float GetCritFromAgility(float Agility) { return GetCritFromAgility(Agility, _Level); }

            public float GetDodgeFromAgility(float Agility, int Level) { return Agility * StatConversionTable[StatType.AgilityToDodge][Level]; }
            public float GetDodgeFromAgility(float Agility) { return GetDodgeFromAgility(Agility, _Level); }

            #endregion

            #region Spirit Based Mana Regen
            private List<float> SpiritRegenConstant = new List<float> { 0.000000000f,    // 00
                0.034965001f, 0.034191001f, 0.033465002f, 0.032527f,    0.031661f,    0.031076999f, 0.030523f,    0.029995f, 0.029307f,    0.028662f,    // 01-10
                0.027585f,    0.026215f,    0.025381001f, 0.024301f,    0.023345999f, 0.022748999f, 0.021958999f, 0.021387f, 0.020791f,    0.020121001f, // 11-20
                0.019733001f, 0.019156f,    0.018819001f, 0.018316999f, 0.017936001f, 0.017577f,    0.017201001f, 0.016919f, 0.016581999f, 0.016233999f, // 21-30
                0.015995f,    0.015707999f, 0.015464f,    0.015204f,    0.014957f,    0.014745f,    0.014496f,    0.014302f, 0.014095f,    0.013896f,    // 31-40
                0.013724f,    0.013522f,    0.013363f,    0.013176f,    0.012996f,    0.012854f,    0.012687f,    0.01254f,  0.012384f,    0.012233f,    // 41-50
                0.012114f,    0.011973f,    0.01186f,     0.011715f,    0.011576f,    0.011473f,    0.011342f,    0.011245f, 0.011111f,    0.011f,       // 51-60
                0.010701f,    0.010523f,    0.010291f,    0.01012f,     0.009969f,    0.009808f,    0.009652f,    0.009553f, 0.009446f,    0.009327f,    // 61-70
                0.008859f,    0.008415f,    0.007993f,    0.007592f,    0.007211f,    0.006849f,    0.006506f,    0.006179f, 0.005869f,    0.005575f     // 71-80
            };
            public float GetSpiritRegenSec(float Spirit, float Intellect, int Level) { return 0.001f + Spirit * SpiritRegenConstant[Level] * (float)Math.Sqrt(Intellect); }
            public float GetSpiritRegenSec(float Spirit, float Intellect) { return GetSpiritRegenSec(Spirit, Intellect, _Level); }
            #endregion

            public void SetLevel(int Level) { _Level = Level; }

            public StatConversion(Character _character)
            {
                SetLevel(_character.Level);

                #region Combat Rating Calculations
                // Level 60 values, rest is extrapolated           
                RatingConversionBase60[RatingType.ArmorPenetration] = 4.69512177f;
                RatingConversionBase60[RatingType.Block] = 5f;
                RatingConversionBase60[RatingType.Crit] = 14f;
                RatingConversionBase60[RatingType.Defense] = 1.5f;
                RatingConversionBase60[RatingType.Dodge] = 12f;
                RatingConversionBase60[RatingType.Expertise] = 10f;
                RatingConversionBase60[RatingType.Haste] = 10f;
                RatingConversionBase60[RatingType.Hit] = 10f;
                RatingConversionBase60[RatingType.Parry] = 15f;
                RatingConversionBase60[RatingType.Resilience] = 25f;
                RatingConversionBase60[RatingType.SpellCrit] = 14f;
                RatingConversionBase60[RatingType.SpellHaste] = 10f;
                RatingConversionBase60[RatingType.SpellHit] = 8f;

                foreach (RatingType rt in Enum.GetValues(typeof(RatingType)))
                    RatingConversionTable.Add(rt, new List<float>());

                for (int x = 0; x < 81; x++)
                {
                    float RatingConversion = 0;
                    if (x <= 10)
                        RatingConversion = 2f / 52f;
                    else if (x <= 35)
                        RatingConversion = (35f - 8f) / 52f;
                    else if (x <= 60)
                        RatingConversion = (x - 8f) / 52f;
                    else if (x <= 70)
                        RatingConversion = 82f / (262f - 3f * x);
                    else
                        RatingConversion = (float)Math.Pow((82f / 52f) * (131f / 63f), (x - 70f) / 10f);
                    foreach (RatingType rt in Enum.GetValues(typeof(RatingType)))
                        RatingConversionTable[rt].Add(1f / RatingConversionBase60[rt] * RatingConversion);
                }
                #endregion

                #region Stat Calculations
                // This is the Mage Int->Crit table, but will do nicely for all other classes between 60-80
                StatConversionTable[StatType.IntellectToCrit] = new List<float> { 0.0f,   // 00
                     6.11f,  6.35f,  6.60f,  7.09f,  7.33f,  7.58f,  7.82f,  8.06f,  8.55f,  8.80f, // 01-10
                     9.53f, 10.75f, 11.48f, 13.68f, 14.90f, 15.65f, 16.61f, 17.61f, 18.59f, 19.80f, // 11-20
                    20.53f, 21.74f, 22.47f, 23.70f, 24.69f, 25.64f, 26.88f, 29.59f, 30.77f, 32.05f, // 21-30
                    32.79f, 34.01f, 34.97f, 35.97f, 37.17f, 38.17f, 39.37f, 40.32f, 41.49f, 42.55f, // 31-40
                    43.48f, 46.51f, 47.39f, 48.54f, 49.75f, 50.76f, 52.08f, 53.19f, 54.35f, 55.87f, // 41-50
                    56.82f, 57.80f, 58.82f, 60.24f, 61.73f, 64.94f, 66.23f, 67.11f, 68.49f, 69.93f, // 51-60
                    69.93f, 69.93f, 69.93f, 70.42f, 70.42f, 72.46f, 74.63f, 76.34f, 78.13f, 80.00f, // 61-70
                    86.2068944f, 92.59259244f, 99.00990313f, 107.526879f, 114.9425283f, 123.4567927f, 133.333327f, 142.857139f, 153.8461534f, 166.6666709f // 71-80
                };

                switch (_character.Class)
                {
                    case Character.CharacterClass.DeathKnight:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.258700f, 0.226400f, 0.226400f, 0.226400f, 0.226400f, 0.201200f, 0.201200f, 0.201200f, 0.201200f, 0.201200f, // 01-10
                            0.181100f, 0.181100f, 0.164600f, 0.164600f, 0.150900f, 0.150900f, 0.150900f, 0.139300f, 0.139300f, 0.129300f, // 11-20
                            0.129300f, 0.129300f, 0.120700f, 0.113200f, 0.113200f, 0.106500f, 0.106500f, 0.100600f, 0.100600f, 0.095300f, // 21-30
                            0.095300f, 0.090500f, 0.090500f, 0.086200f, 0.086200f, 0.082300f, 0.082300f, 0.078700f, 0.078700f, 0.075500f, // 31-40
                            0.072400f, 0.072400f, 0.069600f, 0.069600f, 0.067100f, 0.067100f, 0.064700f, 0.062400f, 0.062400f, 0.060400f, // 41-50
                            0.060400f, 0.058400f, 0.056600f, 0.056600f, 0.054900f, 0.054900f, 0.053300f, 0.051700f, 0.051700f, 0.050300f, // 51-60
                            0.047700f, 0.045300f, 0.043100f, 0.042100f, 0.040200f, 0.038500f, 0.037000f, 0.035500f, 0.034200f, 0.033500f, // 61-70
                            0.031200f, 0.028700f, 0.026600f, 0.024800f, 0.023200f, 0.021600f, 0.019900f, 0.018500f, 0.017200f, 0.016000f // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                        break;
                    case Character.CharacterClass.Druid:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.126200f, 0.126200f, 0.120200f, 0.120200f, 0.114800f, 0.114800f, 0.109800f, 0.109800f, 0.105200f, 0.097100f, // 01-10
                            0.093500f, 0.093500f, 0.090200f, 0.090200f, 0.084200f, 0.084200f, 0.081400f, 0.078900f, 0.078900f, 0.070100f, // 11-20
                            0.070100f, 0.068200f, 0.066400f, 0.066400f, 0.063100f, 0.063100f, 0.061600f, 0.060100f, 0.060100f, 0.054900f, // 21-30
                            0.053700f, 0.053700f, 0.052600f, 0.051500f, 0.050500f, 0.049500f, 0.048500f, 0.048500f, 0.047600f, 0.044300f, // 31-40
                            0.043500f, 0.043500f, 0.042800f, 0.042100f, 0.040700f, 0.040100f, 0.040100f, 0.039400f, 0.038800f, 0.036600f, // 41-50
                            0.036100f, 0.035600f, 0.035100f, 0.035100f, 0.034100f, 0.033700f, 0.033200f, 0.032800f, 0.032400f, 0.030800f, // 51-60
                            0.029900f, 0.029500f, 0.028500f, 0.027900f, 0.027400f, 0.026900f, 0.026500f, 0.025800f, 0.025400f, 0.025000f, // 61-70
                            0.023200f, 0.021600f, 0.020100f, 0.018700f, 0.017300f, 0.016100f, 0.015000f, 0.013900f, 0.012900f, 0.012000f  // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.05f, // 61-70
                            0.047222f, 0.044444f, 0.041667f, 0.038889f, 0.036111f, 0.033333f, 0.030556f, 0.027778f, 0.025000f, 0.022222f, // 71-80
                        };
                        break;
                    case Character.CharacterClass.Hunter:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.284000f, 0.283400f, 0.271100f, 0.253000f, 0.243000f, 0.233700f, 0.225100f, 0.217100f, 0.205100f, 0.198400f, // 01-10
                            0.184800f, 0.167000f, 0.154700f, 0.144100f, 0.133000f, 0.126700f, 0.119400f, 0.111700f, 0.106000f, 0.099800f, // 11-20
                            0.096200f, 0.091000f, 0.087200f, 0.082900f, 0.079700f, 0.076700f, 0.073400f, 0.070900f, 0.068000f, 0.065400f, // 21-30
                            0.063700f, 0.061400f, 0.059200f, 0.057500f, 0.055600f, 0.054100f, 0.052400f, 0.050800f, 0.049300f, 0.048100f, // 31-40
                            0.047000f, 0.045700f, 0.044400f, 0.043300f, 0.042100f, 0.041300f, 0.040200f, 0.039100f, 0.038200f, 0.037300f, // 41-50
                            0.036600f, 0.035800f, 0.035000f, 0.034100f, 0.033400f, 0.032800f, 0.032100f, 0.031400f, 0.030700f, 0.030100f, // 51-60
                            0.029700f, 0.029000f, 0.028400f, 0.027900f, 0.027300f, 0.027000f, 0.026400f, 0.025900f, 0.025400f, 0.025000f, // 61-70
                            0.023200f, 0.021600f, 0.020100f, 0.018700f, 0.017300f, 0.016100f, 0.015000f, 0.013900f, 0.012900f, 0.012000f  // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                        break;
                    case Character.CharacterClass.Mage:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.077300f, 0.077300f, 0.077300f, 0.073600f, 0.073600f, 0.073600f, 0.073600f, 0.073600f, 0.073600f, 0.070300f, // 01-10
                            0.070300f, 0.070300f, 0.070300f, 0.070300f, 0.067200f, 0.067200f, 0.067200f, 0.067200f, 0.067200f, 0.064400f, // 11-20
                            0.064400f, 0.064400f, 0.064400f, 0.061800f, 0.061800f, 0.061800f, 0.061800f, 0.061800f, 0.059500f, 0.059500f, // 21-30
                            0.059500f, 0.059500f, 0.057300f, 0.057300f, 0.057300f, 0.055200f, 0.055200f, 0.055200f, 0.055200f, 0.053300f, // 31-40
                            0.053300f, 0.053300f, 0.053300f, 0.051500f, 0.051500f, 0.051500f, 0.049900f, 0.049900f, 0.049900f, 0.048300f, // 41-50
                            0.048300f, 0.048300f, 0.046800f, 0.046800f, 0.046800f, 0.045500f, 0.045500f, 0.045500f, 0.044200f, 0.044200f, // 51-60
                            0.044200f, 0.044200f, 0.042900f, 0.042900f, 0.042900f, 0.041800f, 0.041800f, 0.041800f, 0.040700f, 0.040700f, // 61-70
                            0.037700f, 0.035100f, 0.032900f, 0.030300f, 0.028100f, 0.026200f, 0.024200f, 0.022700f, 0.020900f, 0.019600f  // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                        break;
                    case Character.CharacterClass.Paladin:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.216400f, 0.216400f, 0.216400f, 0.192400f, 0.192400f, 0.192400f, 0.192400f, 0.173200f, 0.173200f, 0.173200f, // 01-10
                            0.173200f, 0.173200f, 0.157400f, 0.157400f, 0.144300f, 0.144300f, 0.144300f, 0.133200f, 0.133200f, 0.123700f, // 11-20
                            0.123700f, 0.123700f, 0.115400f, 0.108200f, 0.108200f, 0.108200f, 0.101900f, 0.101900f, 0.096200f, 0.096200f, // 21-30
                            0.091100f, 0.091100f, 0.086600f, 0.086600f, 0.082500f, 0.082500f, 0.082500f, 0.078700f, 0.078700f, 0.075300f, // 31-40
                            0.075300f, 0.075300f, 0.072100f, 0.069300f, 0.069300f, 0.066600f, 0.066600f, 0.064100f, 0.064100f, 0.061800f, // 41-50
                            0.059700f, 0.059700f, 0.057700f, 0.057700f, 0.055900f, 0.055900f, 0.054100f, 0.052500f, 0.052500f, 0.050900f, // 51-60
                            0.049500f, 0.048100f, 0.046800f, 0.045600f, 0.044400f, 0.044400f, 0.042200f, 0.042200f, 0.041200f, 0.040300f, // 61-70
                            0.036800f, 0.034600f, 0.032100f, 0.029900f, 0.027500f, 0.025800f, 0.024000f, 0.022200f, 0.020600f, 0.019200f  // 71-80
                        };
                        // Lifted from Tankadin
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.000403f, // 61-70
                            .000372f, .000345f, .000322f, .000298f, .000277f, .000257f, .000239f, .000223f, .0002207f, .000192f // 71-80
                        };
                        break;
                    case Character.CharacterClass.Priest:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.091200f, 0.091200f, 0.091200f, 0.086800f, 0.086800f, 0.086800f, 0.086800f, 0.082900f, 0.082900f, 0.082900f, // 01-10
                            0.082900f, 0.079300f, 0.079300f, 0.079300f, 0.079300f, 0.076000f, 0.076000f, 0.076000f, 0.072900f, 0.072900f, // 11-20
                            0.072900f, 0.072900f, 0.070100f, 0.070100f, 0.070100f, 0.067500f, 0.067500f, 0.067500f, 0.065100f, 0.065100f, // 21-30
                            0.065100f, 0.062900f, 0.062900f, 0.062900f, 0.060800f, 0.060800f, 0.060800f, 0.058800f, 0.058800f, 0.058800f, // 31-40
                            0.057000f, 0.057000f, 0.055300f, 0.055300f, 0.055300f, 0.053600f, 0.053600f, 0.052100f, 0.052100f, 0.052100f, // 41-50
                            0.050700f, 0.050700f, 0.049300f, 0.049300f, 0.048000f, 0.048000f, 0.046800f, 0.046800f, 0.045600f, 0.045600f, // 51-60
                            0.044500f, 0.044600f, 0.044300f, 0.043400f, 0.042700f, 0.042100f, 0.041500f, 0.041300f, 0.041200f, 0.040100f, // 61-70
                            0.037200f, 0.034400f, 0.032000f, 0.029900f, 0.027600f, 0.025700f, 0.024000f, 0.022200f, 0.020700f, 0.019200f  // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                        break;
                    case Character.CharacterClass.Rogue:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.447600f, 0.429000f, 0.411800f, 0.381300f, 0.367700f, 0.355000f, 0.332100f, 0.321700f, 0.312000f, 0.294100f, // 01-10
                            0.264000f, 0.239400f, 0.214500f, 0.198000f, 0.177500f, 0.166000f, 0.156000f, 0.145000f, 0.135500f, 0.127100f, // 11-20
                            0.119700f, 0.114400f, 0.108400f, 0.104000f, 0.098000f, 0.093600f, 0.090300f, 0.086500f, 0.083000f, 0.079200f, // 21-30
                            0.076800f, 0.074100f, 0.071500f, 0.069100f, 0.066400f, 0.064300f, 0.062800f, 0.060900f, 0.059200f, 0.057200f, // 31-40
                            0.055600f, 0.054200f, 0.052800f, 0.051200f, 0.049700f, 0.048600f, 0.047400f, 0.046400f, 0.045400f, 0.044000f, // 41-50
                            0.043100f, 0.042200f, 0.041200f, 0.040400f, 0.039400f, 0.038600f, 0.037800f, 0.037000f, 0.036400f, 0.035500f, // 51-60
                            0.033400f, 0.032200f, 0.030700f, 0.029600f, 0.028600f, 0.027600f, 0.026800f, 0.026200f, 0.025600f, 0.025000f, // 61-70 
                            0.023200f, 0.021600f, 0.020100f, 0.018700f, 0.017300f, 0.016100f, 0.015000f, 0.013900f, 0.012900f, 0.012000f, // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                        break;
                    case Character.CharacterClass.Shaman:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.103900f, 0.103900f, 0.099000f, 0.099000f, 0.094500f, 0.094500f, 0.094500f, 0.090300f, 0.090300f, 0.086600f, // 01-10
                            0.086600f, 0.083100f, 0.083100f, 0.079900f, 0.077000f, 0.074200f, 0.074200f, 0.071700f, 0.071700f, 0.067000f, // 11-20
                            0.067000f, 0.064900f, 0.064900f, 0.063000f, 0.061100f, 0.059400f, 0.059400f, 0.057700f, 0.057700f, 0.054700f, // 21-30
                            0.054700f, 0.053300f, 0.052000f, 0.052000f, 0.049500f, 0.048300f, 0.048300f, 0.047200f, 0.047200f, 0.045200f, // 31-40
                            0.044200f, 0.044200f, 0.043300f, 0.042400f, 0.041600f, 0.040700f, 0.040000f, 0.039200f, 0.039200f, 0.037800f, // 41-50
                            0.037100f, 0.036500f, 0.036500f, 0.035800f, 0.034600f, 0.034100f, 0.033500f, 0.033500f, 0.033000f, 0.032000f, // 51-60
                            0.031000f, 0.030400f, 0.029400f, 0.028500f, 0.028100f, 0.027300f, 0.026700f, 0.026100f, 0.025500f, 0.025000f, // 61-70
                            0.023200f, 0.021600f, 0.020100f, 0.018700f, 0.017300f, 0.016100f, 0.015000f, 0.013900f, 0.012900f, 0.012000f  // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                        break;
                    case Character.CharacterClass.Warlock:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.118900f, 0.118900f, 0.113200f, 0.113200f, 0.113200f, 0.108100f, 0.108100f, 0.108100f, 0.103400f, 0.103400f, // 01-10
                            0.099100f, 0.099100f, 0.099100f, 0.095900f, 0.094400f, 0.092800f, 0.091400f, 0.089900f, 0.088500f, 0.087100f, // 11-20
                            0.085700f, 0.084400f, 0.083100f, 0.081800f, 0.080500f, 0.079200f, 0.078000f, 0.076800f, 0.075600f, 0.074500f, // 21-30
                            0.073300f, 0.072200f, 0.071100f, 0.070000f, 0.069000f, 0.067900f, 0.066900f, 0.065900f, 0.064900f, 0.063900f, // 31-40
                            0.063000f, 0.062000f, 0.061100f, 0.060200f, 0.059300f, 0.058400f, 0.057600f, 0.056700f, 0.055900f, 0.055100f, // 41-50
                            0.054300f, 0.053500f, 0.052700f, 0.051900f, 0.051200f, 0.050400f, 0.049700f, 0.049000f, 0.048300f, 0.047600f, // 51-60
                            0.046900f, 0.046200f, 0.045500f, 0.044900f, 0.044200f, 0.043600f, 0.043000f, 0.042400f, 0.041800f, 0.041200f, // 61-70
                            0.038400f, 0.035500f, 0.033000f, 0.030900f, 0.028700f, 0.026400f, 0.024500f, 0.022900f, 0.021200f, 0.019800f  // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                        break;
                    case Character.CharacterClass.Warrior:
                        StatConversionTable[StatType.AgilityToCrit] = new List<float> { 0.0f,   // 00
                            0.258700f, 0.226400f, 0.226400f, 0.226400f, 0.226400f, 0.201200f, 0.201200f, 0.201200f, 0.201200f, 0.201200f, // 01-10
                            0.181100f, 0.181100f, 0.164600f, 0.164600f, 0.150900f, 0.150900f, 0.150900f, 0.139300f, 0.139300f, 0.129300f, // 11-20
                            0.129300f, 0.129300f, 0.120700f, 0.113200f, 0.113200f, 0.106500f, 0.106500f, 0.100600f, 0.100600f, 0.095300f, // 21-30
                            0.095300f, 0.090500f, 0.090500f, 0.086200f, 0.086200f, 0.082300f, 0.082300f, 0.078700f, 0.078700f, 0.075500f, // 31-40
                            0.072400f, 0.072400f, 0.069600f, 0.069600f, 0.067100f, 0.067100f, 0.064700f, 0.062400f, 0.062400f, 0.060400f, // 41-50
                            0.060400f, 0.058400f, 0.056600f, 0.056600f, 0.054900f, 0.054900f, 0.053300f, 0.051700f, 0.051700f, 0.050300f, // 51-60
                            0.047700f, 0.045300f, 0.043100f, 0.042100f, 0.040200f, 0.038500f, 0.037000f, 0.035500f, 0.034200f, 0.033500f, // 61-70
                            0.031200f, 0.028700f, 0.026600f, 0.024800f, 0.023200f, 0.021600f, 0.019900f, 0.018500f, 0.017200f, 0.016000f  // 71-80
                        };
                        StatConversionTable[StatType.AgilityToDodge] = new List<float> { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                        break;              
                    default:
                        new Exception("Unknown class!");
                        break;

                }
                // Reverse Intellect table for faster math later.
                for (int x = 0; x < StatConversionTable[StatType.IntellectToCrit].Count; x++)
                    if (StatConversionTable[StatType.IntellectToCrit][x] > 0)
                        StatConversionTable[StatType.IntellectToCrit][x] = 1f / StatConversionTable[StatType.IntellectToCrit][x];
                #endregion
            }
        }

        public Stats GetRaceStats(Character character)
        {
            switch (character.Race)
            {
                case Character.CharacterRace.NightElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 57f,
                        Intellect = 145f,
                        Spirit = 151f
                    };
                case Character.CharacterRace.Dwarf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 61f,
                        Intellect = 144f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Draenei:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 57f,
                        Intellect = 146f,
                        Spirit = 153f
                    };
                case Character.CharacterRace.Human:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 58f,
                        Intellect = 145f,
                        Spirit = 152f,
                        BonusSpiritMultiplier = 0.03f
                    };
                case Character.CharacterRace.BloodElf:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 56f,
                        Intellect = 149f,
                        Spirit = 150f
                    };
                case Character.CharacterRace.Troll:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Intellect = 141f,
                        Spirit = 152f
                    };
                case Character.CharacterRace.Undead:
                    return new Stats()
                    {
                        Health = 3211f,
                        Mana = 2620f,
                        Stamina = 59f,
                        Intellect = 143f,
                        Spirit = 156f,
                    };
            }
            return null;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            Stats statsRace = GetRaceStats(character);
            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTalents = new Stats()
            {
                BonusStaminaMultiplier = character.PriestTalents.Enlightenment * 0.01f,
                BonusSpiritMultiplier = (1 + character.PriestTalents.Enlightenment * 0.01f) * (1f + character.PriestTalents.SpiritOfRedemption * 0.05f) - 1f,
                BonusIntellectMultiplier = character.PriestTalents.MentalStrength * 0.03f,
                SpellDamageFromSpiritPercentage = character.PriestTalents.SpiritualGuidance * 0.05f + character.PriestTalents.TwistedFaith * 0.02f,
                SpellHaste = character.PriestTalents.Enlightenment * 0.01f,
                SpellCombatManaRegeneration = character.PriestTalents.Meditation * 0.1f
            };

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace + statsTalents;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor(statsTotal.Intellect * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.SpellPower = (float)Math.Round(statsTotal.SpellPower + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana += (statsTotal.Intellect - 20f) * 15f + 20f;
            statsTotal.Health += statsTotal.Stamina * 10f;
            statsTotal.SpellCrit += (statsTotal.Intellect / 80f / 100f) + (statsTotal.CritRating / 22.07692337F / 100f) + 0.0124f;
            statsTotal.SpellHaste += (statsTotal.HasteRating / 15.76923275f / 100f);
            statsTotal.SpellHit += (statsTotal.HitRating / 12.61538506f / 100f);

            return statsTotal;
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Health = stats.Health,
                Resilience = stats.Resilience,
                Intellect = stats.Intellect,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                Mp5 = stats.Mp5,
                SpellPower = stats.SpellPower,
                SpellShadowDamageRating = stats.SpellShadowDamageRating,
                SpellCritRating = stats.SpellCritRating,
                CritRating = stats.CritRating,
                SpellCrit = stats.SpellCrit,
                SpellHitRating = stats.SpellHitRating,
                HitRating = stats.HitRating,
                SpellHit = stats.SpellHit,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHaste = stats.SpellHaste,
                HasteRating = stats.HasteRating,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaRestoreFromMaxManaPerHit = stats.ManaRestoreFromMaxManaPerHit,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                TimbalsProc = stats.TimbalsProc,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (
                  stats.Stamina
                + stats.Health
                + stats.Resilience
                + stats.Intellect
                + stats.Mana
                + stats.Spirit
                + stats.Mp5
                + stats.SpellPower
                + stats.SpellShadowDamageRating
                + stats.SpellCritRating
                + stats.CritRating
                + stats.SpellCrit
                + stats.SpellHitRating
                + stats.HitRating
                + stats.SpellHit
                + stats.SpellHasteRating
                + stats.SpellHaste
                + stats.HasteRating
                + stats.BonusSpiritMultiplier
                + stats.SpellDamageFromSpiritPercentage
                + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion
                + stats.ThreatReductionMultiplier
                + stats.BonusShadowDamageMultiplier
                + stats.BonusHolyDamageMultiplier
                + stats.ManaRestoreOnCast_5_15
                + stats.ManaRestoreFromMaxManaPerHit
                + stats.SpellPowerFor15SecOnUse90Sec
                + stats.SpellPowerFor15SecOnUse2Min
                + stats.SpellPowerFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse2Min
                + stats.HasteRatingFor20SecOnUse5Min
                + stats.TimbalsProc
                + stats.BonusSpellCritMultiplier
                ) > 0;
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsShadowPriest));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsShadowPriest calcOpts = serializer.Deserialize(reader) as CalculationOptionsShadowPriest;
            return calcOpts;
        }
    }
}
