using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public class Segment
    {
        public int Index { get; set; }
        public double Duration { get; set; }
        public double TimeStart { get; set; }
        public double TimeEnd { get { return TimeStart + Duration; } }
        //public int FirstSpellColumn { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1} - {2})", Index, CalculationsMage.TimeFormat(TimeStart), CalculationsMage.TimeFormat(TimeEnd));
        }
    }

    public sealed partial class Solver
    {
        private const int cooldownCount = 15;
        private List<Segment> segmentList;
        private List<SolutionVariable> solutionVariable;

        private List<CastingState> stateList;
        private List<CycleId> spellList;

        private SolverLP lp;
        private double[] solution;
        private double lowerBound;
        private double upperBound;
        private int[] segmentColumn;
        private CharacterCalculationsMage calculationResult;
        private Character character;
        private MageTalents talents;
        private CalculationOptionsMage calculationOptions;
        private string armor;

        private bool restrictThreat;

        private int advancedConstraintsLevel;
        private bool segmentCooldowns;
        private bool segmentNonCooldowns;
        private bool integralMana;
        private bool requiresMIP;

        private bool minimizeTime;
        private bool restrictManaUse;
        private bool needsTimeExtension;
        private bool conjureManaGem;
        private bool wardsAvailable;

        private bool heroismAvailable;
        private bool arcanePowerAvailable;
        private bool icyVeinsAvailable;
        private bool combustionAvailable;
        private bool moltenFuryAvailable;
        private bool trinket1Available;
        private bool trinket2Available;
        private bool coldsnapAvailable;
        private bool potionOfWildMagicAvailable;
        private bool potionOfSpeedAvailable;
        private bool effectPotionAvailable;
        private bool drumsOfBattleAvailable;
        private bool flameCapAvailable;
        private bool waterElementalAvailable;
        private bool manaGemEffectAvailable;
        private bool powerInfusionAvailable;
        private bool evocationAvailable;
        private bool manaPotionAvailable;

        private Cooldown availableCooldownMask = 0;

        private double manaGemEffectDuration;
        private double trinket1Cooldown;
        private double trinket1Duration;
        private double trinket2Cooldown;
        private double trinket2Duration;

        private struct SegmentConstraint
        {
            public int Row;
            public int MinSegment;
            public int MaxSegment;
        }

        #region LP rows
        private int rowManaRegen = -1;
        private int rowFightDuration = -1;
        private int rowEvocation = -1;
        private int rowEvocationIV = -1;
        private int rowEvocationHero = -1;
        private int rowEvocationIVHero = -1;
        //private int rowEvocationIVActivation = -1;
        //private int rowEvocationHeroActivation = -1;
        //private int rowEvocationIVHeroActivation = -1;
        private int rowPotion = -1;
        private int rowManaPotion = -1;
        private int rowConjureManaGem = -1;
        private int rowWard = -1;
        private int rowManaGem = -1;
        private int rowManaGemMax = -1;
        private int rowHeroism = -1;
        private int rowArcanePower = -1;
        private int rowHeroismManaGemEffect = -1;
        private int rowHeroismArcanePower = -1;
        private int rowIcyVeins = -1;
        private int rowWaterElemental = -1;
        private int rowMoltenFury = -1;
        //private int rowMoltenFuryDestructionPotion = -1;
        private int rowMoltenFuryIcyVeins = -1;
        private int rowMoltenFuryManaGemEffect = -1;
        //private int rowHeroismDestructionPotion = -1;
        //private int rowIcyVeinsDestructionPotion = -1;
        private int rowManaGemFlameCap = -1;
        private int rowMoltenFuryFlameCap = -1;
        //private int rowFlameCapDestructionPotion = -1;
        private int rowTrinket1 = -1;
        private int rowTrinket2 = -1;
        private int rowManaGemEffect = -1;
        private int rowMoltenFuryTrinket1 = -1;
        private int rowMoltenFuryTrinket2 = -1;
        private int rowHeroismTrinket1 = -1;
        private int rowHeroismTrinket2 = -1;
        private int rowManaGemEffectActivation = -1;
        private int rowDpsTime = -1;
        private int rowAoe = -1;
        private int rowFlamestrike = -1;
        private int rowConeOfCold = -1;
        private int rowBlastWave = -1;
        private int rowDragonsBreath = -1;
        private int rowCombustion = -1;
        private int rowPowerInfusion = -1;
        private int rowMoltenFuryCombustion = -1;
        private int rowHeroismCombustion = -1;
        private int rowHeroismIcyVeins = -1;
        private int rowSummonWaterElemental = -1;
        private int rowSummonWaterElementalCount = -1;
        private int rowDrumsOfBattleActivation = -1;
        private int rowMoltenFuryDrumsOfBattle = -1;
        private int rowHeroismDrumsOfBattle = -1;
        private int rowIcyVeinsDrumsOfBattle = -1;
        private int rowArcanePowerDrumsOfBattle = -1;
        private int rowThreat = -1;
        //private int rowManaPotionManaGem = -1;
        private int rowDrumsOfBattle = -1;
        private int rowTimeExtension = -1;
        private int rowAfterFightRegenMana = -1;
        //private int rowAfterFightRegenHealth = -1;
        private int rowTargetDamage = -1;
        //private int rowSegmentMoltenFury = -1;
        //private int rowSegmentHeroism = -1;
        private List<SegmentConstraint> rowSegmentArcanePower = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentPowerInfusion = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentIcyVeins = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentWaterElemental = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentSummonWaterElemental = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentCombustion = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentDrumsOfBattle = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentDrumsOfBattleActivation = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentFlameCap = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentManaGem = new List<SegmentConstraint>();
        //private int rowSegmentPotion = -1;
        private List<SegmentConstraint> rowSegmentTrinket1 = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentTrinket2 = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentManaGemEffect = new List<SegmentConstraint>();
        private List<SegmentConstraint> rowSegmentEvocation = new List<SegmentConstraint>();
        private int rowSegment = -1;
        private int rowSegmentManaOverflow = -1;
        private int rowSegmentManaUnderflow = -1;
        private int rowSegmentThreat = -1;
        #endregion

        private bool useIncrementalOptimizations;
        private bool useGlobalOptimizations;
        private bool needsDisplayCalculations;
        private bool needsSolutionVariables;

        private bool cancellationPending;

        internal bool CancellationPending
        {
            get
            {
                return cancellationPending;
            }
        }

        public void CancelAsync()
        {
            cancellationPending = true;
        }

        public Solver(Character character, CalculationOptionsMage calculationOptions, bool segmentCooldowns, bool integralMana, int advancedConstraintsLevel, string armor, bool useIncrementalOptimizations, bool useGlobalOptimizations, bool needsDisplayCalculations, bool needsSolutionVariables)
        {
            this.character = character;
            this.talents = character.MageTalents;
            this.calculationOptions = calculationOptions;
            this.segmentCooldowns = segmentCooldowns;
            this.advancedConstraintsLevel = advancedConstraintsLevel;
            this.integralMana = integralMana;
            this.armor = armor;
            this.useIncrementalOptimizations = useIncrementalOptimizations;
            this.useGlobalOptimizations = useGlobalOptimizations;
            this.needsDisplayCalculations = needsDisplayCalculations;
            this.requiresMIP = segmentCooldowns || integralMana;
            if (needsDisplayCalculations || requiresMIP) needsSolutionVariables = true;
            this.needsSolutionVariables = needsSolutionVariables;
        }

        private static bool IsItemActivatable(ItemInstance item)
        {
            if (item == null || item.Item == null) return false;
            return (item.Item.Stats.ContainsSpecialEffect(effect => effect.Trigger == Trigger.Use));
        }

        private double MaximizeColdsnapDuration(double fightDuration, double coldsnapCooldown, double effectDuration, double effectCooldown, out int coldsnapCount)
        {
            int bestColdsnap = 0;
            double bestEffect = 0.0;
            List<int> coldsnap = new List<int>();
            List<double> startTime = new List<double>();
            List<double> coldsnapTime = new List<double>();
            int index = 0;
            coldsnap.Add(2);
            startTime.Add(0.0);
            coldsnapTime.Add(0.0);
            do
            {
                if (index > 0 && startTime[index - 1] + effectDuration >= fightDuration)
                {
                    double effect = (index - 1) * effectDuration + Math.Max(fightDuration - startTime[index - 1], 0.0);
                    if (effect > bestEffect)
                    {
                        bestEffect = effect;
                        bestColdsnap = 0;
                        for (int i = 0; i < index; i++)
                        {
                            if (startTime[i] < fightDuration - 20.0) bestColdsnap += coldsnap[i]; // if it is a coldsnap for a very short elemental, don't count it for IV
                        }
                    }
                    index--;
                }
                coldsnap[index]--;
                if (coldsnap[index] < 0)
                {
                    index--;
                }
                else
                {
                    double time = 0.0;
                    if (index > 0)
                    {
                        time = startTime[index - 1] + effectDuration;
                        int lastColdsnap = -1;
                        for (int j = 0; j < index; j++)
                        {
                            if (coldsnap[j] == 1) lastColdsnap = j;
                        }
                        if (coldsnap[index] == 1)
                        {
                            // use coldsnap
                            double normalTime = Math.Max(time, startTime[index - 1] + effectCooldown);
                            double coldsnapReady = 0.0;
                            if (lastColdsnap >= 0) coldsnapReady = coldsnapTime[lastColdsnap] + coldsnapCooldown;
                            if (coldsnapReady >= normalTime)
                            {
                                // coldsnap won't be ready until effect will be back anyway, so we don't actually need it
                                coldsnap[index] = 0;
                                time = normalTime;
                            }
                            else
                            {
                                // go now or when coldsnap is ready
                                time = Math.Max(coldsnapReady, time);
                                coldsnapTime[index] = Math.Max(coldsnapReady, startTime[index - 1]);
                            }
                        }
                        else
                        {
                            // we are not allowed to use coldsnap even if we could
                            // make sure to adjust by coldsnap constraints
                            time = Math.Max(time, startTime[index - 1] + effectCooldown);
                        }
                    }
                    else
                    {
                        coldsnap[index] = 0;
                    }
                    startTime[index] = time;
                    index++;
                    if (index >= coldsnap.Count)
                    {
                        coldsnap.Add(0);
                        coldsnapTime.Add(0.0);
                        startTime.Add(0.0);
                    }
                    coldsnap[index] = 2;
                }
            } while (index >= 0);
            coldsnapCount = bestColdsnap;
            return bestEffect;
        }

        private double MaximizeEffectDuration(double fightDuration, double effectDuration, double effectCooldown)
        {
            if (fightDuration < effectDuration) return fightDuration;
            double total = effectDuration;
            fightDuration -= effectDuration;
            int count = (int)(fightDuration / effectCooldown);
            total += effectDuration * count;
            fightDuration -= effectCooldown * count;
            fightDuration -= effectCooldown - effectDuration;
            if (fightDuration > 0) total += fightDuration;
            return total;
        }

        public static CharacterCalculationsMage GetCharacterCalculations(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, CalculationsMage calculations, string armor, bool segmentCooldowns, bool integralMana, int advancedConstraintsLevel, bool useIncrementalOptimizations, bool useGlobalOptimizations, bool needsDisplayCalculations, bool needsSolutionVariables)
        {
            Solver solver = new Solver(character, calculationOptions, segmentCooldowns, integralMana, advancedConstraintsLevel, armor, useIncrementalOptimizations, useGlobalOptimizations, needsDisplayCalculations, needsSolutionVariables);
            return solver.GetCharacterCalculations(additionalItem, calculations);
        }

        public CharacterCalculationsMage GetCharacterCalculations(Item additionalItem, CalculationsMage calculations)
        {
            Stats rawStats;
            Stats baseStats;
            InitializeCalculationResult(additionalItem, calculations, out rawStats, out baseStats);

            stateList = GetStateList(baseStats);
            spellList = GetSpellList();

            List<double> tpsList;

            ConstructProblem(additionalItem, calculations, rawStats, baseStats, out tpsList);

            if (requiresMIP)
            {
                RestrictSolution();
            }

            calculationResult.Solution = lp.Solve();
            ArrayPool.ReleaseArraySet(lp.ArraySet);

            if (!requiresMIP)
            {
                calculationResult.UpperBound = lp.Value;
                calculationResult.LowerBound = 0.0;
            }
            else
            {
                calculationResult.UpperBound = upperBound;
                if (integralMana && segmentCooldowns && advancedConstraintsLevel >= 5) calculationResult.LowerBound = lowerBound;
            }

            if (minimizeTime)
            {
                calculationResult.SubPoints[0] = -(float)(calculationOptions.TargetDamage / calculationResult.Solution[calculationResult.Solution.Length - 1]);
            }
            else
            {
                calculationResult.SubPoints[0] = ((float)calculationResult.Solution[calculationResult.Solution.Length - 1] /*+ calculationResult.WaterElementalDamage*/) / calculationOptions.FightDuration;
            }
            calculationResult.SubPoints[1] = EvaluateSurvivability(baseStats);
            calculationResult.OverallPoints = calculationResult.SubPoints[0] + calculationResult.SubPoints[1];

            if (needsDisplayCalculations)
            {
                float threat = 0;
                for (int i = 0; i < tpsList.Count; i++)
                {
                    threat += (float)(tpsList[i] * calculationResult.Solution[i]);
                }
                calculationResult.Tps = threat / calculationOptions.FightDuration;
            }

            return calculationResult;
        }

        public CharacterCalculationsMage InitializeCalculationResult(Item additionalItem, CalculationsMage calculations, out Stats rawStats, out Stats baseStats)
        {
            List<Buff> autoActivatedBuffs = new List<Buff>();
            List<Buff> activeBuffs;
            rawStats = calculations.GetRawStats(character, additionalItem, calculationOptions, autoActivatedBuffs, armor, out activeBuffs);
            baseStats = calculations.GetCharacterStats(character, additionalItem, rawStats, calculationOptions);

            calculationResult = new CharacterCalculationsMage();
            calculationResult.Calculations = calculations;
            calculationResult.BaseStats = baseStats;
            calculationResult.Character = character;
            calculationResult.CalculationOptions = calculationOptions;
            calculationResult.MageTalents = talents;
            calculationResult.ActiveBuffs = activeBuffs;
            calculationResult.NeedsDisplayCalculations = needsDisplayCalculations;

            evocationAvailable = calculationOptions.EvocationEnabled && !calculationOptions.EffectDisableManaSources;
            manaPotionAvailable = calculationOptions.ManaPotionEnabled && !calculationOptions.EffectDisableManaSources;
            restrictThreat = segmentCooldowns && calculationOptions.TpsLimit > 0f;
            powerInfusionAvailable = !calculationOptions.DisableCooldowns && calculationOptions.PowerInfusionAvailable;
            heroismAvailable = !calculationOptions.DisableCooldowns && calculationOptions.HeroismAvailable;
            arcanePowerAvailable = !calculationOptions.DisableCooldowns && (talents.ArcanePower == 1);
            icyVeinsAvailable = !calculationOptions.DisableCooldowns && (talents.IcyVeins == 1);
            combustionAvailable = !calculationOptions.DisableCooldowns && (talents.Combustion == 1);
            moltenFuryAvailable = talents.MoltenFury > 0;
            trinket1Available = !calculationOptions.DisableCooldowns && IsItemActivatable(character.Trinket1);
            trinket2Available = !calculationOptions.DisableCooldowns && IsItemActivatable(character.Trinket2);
            coldsnapAvailable = !calculationOptions.DisableCooldowns && (talents.ColdSnap == 1);
            potionOfWildMagicAvailable = !calculationOptions.DisableCooldowns && calculationOptions.PotionOfWildMagic;
            potionOfSpeedAvailable = !calculationOptions.DisableCooldowns && calculationOptions.PotionOfSpeed;
            effectPotionAvailable = potionOfWildMagicAvailable || potionOfSpeedAvailable;
            flameCapAvailable = !calculationOptions.DisableCooldowns && calculationOptions.FlameCap;
            drumsOfBattleAvailable = !calculationOptions.DisableCooldowns && calculationOptions.DrumsOfBattle;
            waterElementalAvailable = !calculationOptions.DisableCooldowns && (talents.SummonWaterElemental == 1);
            calculationResult.ManaGemEffect = manaGemEffectAvailable = calculationOptions.ManaGemEnabled && baseStats.ContainsSpecialEffect(effect => effect.Trigger == Trigger.ManaGem);
            calculationResult.EvocationCooldown = (240.0 - 60.0 * talents.ArcaneFlows);
            calculationResult.ColdsnapCooldown = (8 * 60) * (1 - 0.1 * talents.ColdAsIce);
            calculationResult.ArcanePowerCooldown = 120.0 * (1 - 0.15 * talents.ArcaneFlows);
            calculationResult.ArcanePowerDuration = 15.0 + (talents.GlyphOfArcanePower ? 3.0 : 0.0);
            calculationResult.IcyVeinsCooldown = 180.0 * (1 - 0.07 * talents.IceFloes + (talents.IceFloes == 3 ? 0.01 : 0.00));
            calculationResult.WaterElementalCooldown = (180.0 - (talents.GlyphOfWaterElemental ? 30.0 : 0.0)) * (1 - 0.1 * talents.ColdAsIce);
            calculationResult.WaterElementalDuration = 45.0 + 5.0 * talents.EnduringWinter;
            calculationResult.PowerInfusionDuration = 15.0;
            calculationResult.PowerInfusionCooldown = 120.0;
            if (!calculationOptions.EffectDisableManaSources)
            {
                if (calculationOptions.PlayerLevel < 77)
                {
                    calculationResult.ManaGemValue = 2400.0;
                    calculationResult.MaxManaGemValue = 2460.0;
                }
                else
                {
                    calculationResult.ManaGemValue = 3415.0;
                    calculationResult.MaxManaGemValue = 3500.0;
                }
                if (calculationOptions.PlayerLevel <= 70)
                {
                    calculationResult.ManaPotionValue = 2400.0;
                    calculationResult.MaxManaPotionValue = 3000.0;
                }
                else
                {
                    calculationResult.ManaPotionValue = 4300.0;
                    calculationResult.MaxManaPotionValue = 4400.0;
                }
            }

            #region Effects Setup
            if (trinket1Available)
            {
                Stats s = character.Trinket1.Item.Stats;
                foreach (SpecialEffect effect in s.SpecialEffects(e => e.Trigger == Trigger.Use))
                {
                    trinket1Duration = effect.Duration;
                    trinket1Cooldown = effect.Cooldown;
                    calculationResult.Trinket1SpellPower += effect.Stats.SpellPower;
                    calculationResult.Trinket1HasteRating += effect.Stats.HasteRating;
                }
                calculationResult.Trinket1Duration = trinket1Duration;
                calculationResult.Trinket1Cooldown = trinket1Cooldown;
                if (needsDisplayCalculations) calculationResult.Trinket1Name = character.Trinket1.Item.Name;
            }
            if (trinket2Available)
            {
                Stats s = character.Trinket2.Item.Stats;
                foreach (SpecialEffect effect in s.SpecialEffects(e => e.Trigger == Trigger.Use))
                {
                    trinket2Duration = effect.Duration;
                    trinket2Cooldown = effect.Cooldown;
                    calculationResult.Trinket2SpellPower += effect.Stats.SpellPower;
                    calculationResult.Trinket2HasteRating += effect.Stats.HasteRating;
                }
                calculationResult.Trinket2Duration = trinket2Duration;
                calculationResult.Trinket2Cooldown = trinket2Cooldown;
                if (needsDisplayCalculations) calculationResult.Trinket2Name = character.Trinket2.Item.Name;
            }
            if (manaGemEffectAvailable)
            {
                foreach (SpecialEffect effect in baseStats.SpecialEffects(e => e.Trigger == Trigger.ManaGem))
                {
                    manaGemEffectDuration = effect.Duration;
                    calculationResult.ManaGemEffectSpellPower += effect.Stats.SpellPower;
                }
                calculationResult.ManaGemEffectDuration = manaGemEffectDuration;
            }
            List<SpecialEffect> list = new List<SpecialEffect>();
            foreach (SpecialEffect effect in baseStats.SpecialEffects())
            {
                if (effect.Stats.HasteRating > 0 && effect.MaxStack == 1)
                {
                    if (effect.Cooldown >= effect.Duration && (effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.DamageSpellCast))
                    {
                        list.Add(effect);
                    }
                    else if (effect.Cooldown == 0 && (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellCrit))
                    {
                        list.Add(effect);
                    }
                }
            }
            calculationResult.HasteRatingEffects = list.ToArray();
            list.Clear();
            foreach (SpecialEffect effect in baseStats.SpecialEffects())
            {
                if (effect.Stats.SpellPower > 0 && effect.MaxStack == 1)
                {
                    if (effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.SpellMiss || effect.Trigger == Trigger.MageNukeCast)
                    {
                        list.Add(effect);
                    }
                }
            }
            calculationResult.SpellPowerEffects = list.ToArray();
            list.Clear();
            foreach (SpecialEffect effect in baseStats.SpecialEffects())
            {
                if (effect.Stats.ArcaneDamage + effect.Stats.FireDamage /*+ effect.Stats.FrostDamage*/ + effect.Stats.NatureDamage + effect.Stats.ShadowDamage/* + effect.Stats.HolyDamage*/ > 0 && effect.MaxStack == 1)
                {
                    if (effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellHit)
                    {
                        list.Add(effect);
                    }
                }
            }
            calculationResult.DamageProcEffects = list.ToArray();
            list.Clear();
            foreach (SpecialEffect effect in baseStats.SpecialEffects())
            {
                if (effect.Stats.ManaRestore > 0 && effect.MaxStack == 1)
                {
                    if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit)
                    {
                        list.Add(effect);
                    }
                }
            }
            calculationResult.ManaRestoreEffects = list.ToArray();
            list.Clear();
            foreach (SpecialEffect effect in baseStats.SpecialEffects())
            {
                if (effect.Stats.Mp5 > 0 && effect.MaxStack == 1)
                {
                    if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit)
                    {
                        list.Add(effect);
                    }
                }
            }
            calculationResult.Mp5Effects = list.ToArray();
            #endregion

            if (armor == null)
            {
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Mage Armor"))) armor = "Mage Armor";
                else if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Molten Armor"))) armor = "Molten Armor";
                else if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Ice Armor"))) armor = "Ice Armor";
            }

            CalculateBaseStateStats(baseStats);

            calculationResult.AutoActivatedBuffs.AddRange(autoActivatedBuffs);
            calculationResult.MageArmor = armor;

            return calculationResult;
        }

        private void CalculateBaseStateStats(Stats baseStats)
        {
            calculationResult.BaseSpellHit = baseStats.HitRating * calculationOptions.LevelScalingFactor / 800f + baseStats.SpellHit + 0.01f * talents.Precision;

            int targetLevel = calculationOptions.TargetLevel;
            int playerLevel = calculationOptions.PlayerLevel;

            calculationResult.BaseArcaneHitRate = Math.Min(Spell.MaxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculationResult.BaseSpellHit + 0.01f * talents.ArcaneFocus);
            calculationResult.BaseFireHitRate = Math.Min(Spell.MaxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculationResult.BaseSpellHit);
            calculationResult.BaseFireHitRate = Math.Min(Spell.MaxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculationResult.BaseSpellHit);
            calculationResult.BaseFrostHitRate = Math.Min(Spell.MaxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculationResult.BaseSpellHit);
            calculationResult.BaseNatureHitRate = Math.Min(Spell.MaxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculationResult.BaseSpellHit);
            calculationResult.BaseShadowHitRate = Math.Min(Spell.MaxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculationResult.BaseSpellHit);
            calculationResult.BaseFrostFireHitRate = Math.Min(Spell.MaxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculationResult.BaseSpellHit);
            calculationResult.BaseHolyHitRate = Math.Min(Spell.MaxHitRate, ((targetLevel <= playerLevel + 2) ? (0.96f - (targetLevel - playerLevel) * 0.01f) : (0.94f - (targetLevel - playerLevel - 2) * 0.11f)) + calculationResult.BaseSpellHit);

            float threatFactor = (1 + baseStats.ThreatIncreaseMultiplier) * (1 - baseStats.ThreatReductionMultiplier);

            calculationResult.ArcaneThreatMultiplier = threatFactor * (1 - character.MageTalents.ArcaneSubtlety * 0.2f);
            calculationResult.FireThreatMultiplier = threatFactor * (1 - character.MageTalents.BurningSoul * 0.05f);
            calculationResult.FrostThreatMultiplier = threatFactor * (1 - ((character.MageTalents.FrostChanneling > 0) ? (0.01f + 0.03f * character.MageTalents.FrostChanneling) : 0f));
            calculationResult.FrostFireThreatMultiplier = threatFactor * Math.Min(1 - character.MageTalents.BurningSoul * 0.05f, 1 - ((character.MageTalents.FrostChanneling > 0) ? (0.01f + 0.03f * character.MageTalents.FrostChanneling) : 0f));
            calculationResult.NatureThreatMultiplier = threatFactor;
            calculationResult.ShadowThreatMultiplier = threatFactor;
            calculationResult.HolyThreatMultiplier = threatFactor;

            float baseSpellModifier = (1 + 0.01f * talents.ArcaneInstability) * (1 + 0.01f * talents.PlayingWithFire) * (1 + baseStats.BonusDamageMultiplier) * calculationOptions.EffectDamageMultiplier;
            calculationResult.BaseArcaneSpellModifier = baseSpellModifier * (1 + baseStats.BonusArcaneDamageMultiplier);
            calculationResult.BaseFireSpellModifier = baseSpellModifier * (1 + 0.02f * talents.FirePower) * (1 + baseStats.BonusFireDamageMultiplier);
            calculationResult.BaseFrostSpellModifier = baseSpellModifier * (1 + 0.02f * talents.PiercingIce) * (1 + 0.01f * talents.ArcticWinds) * (1 + baseStats.BonusFrostDamageMultiplier);
            calculationResult.BaseNatureSpellModifier = baseSpellModifier * (1 + baseStats.BonusNatureDamageMultiplier);
            calculationResult.BaseShadowSpellModifier = baseSpellModifier * (1 + baseStats.BonusShadowDamageMultiplier);
            calculationResult.BaseHolySpellModifier = baseSpellModifier * (1 + baseStats.BonusHolyDamageMultiplier);
            calculationResult.BaseFrostFireSpellModifier = baseSpellModifier * (1 + 0.02f * talents.FirePower) * (1 + 0.02f * talents.PiercingIce) * (1 + 0.01f * talents.ArcticWinds) * Math.Max(1 + baseStats.BonusFireDamageMultiplier, 1 + baseStats.BonusFrostDamageMultiplier);

            float spellCritPerInt = 0f;
            float spellCritBase = 0f;
            float baseRegen = 0f;
            switch (playerLevel)
            {
                case 70:
                    spellCritPerInt = 0.0125f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.005596f;
                    break;
                case 71:
                    spellCritPerInt = 0.0116f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.005316f;
                    break;
                case 72:
                    spellCritPerInt = 0.0108f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.005049f;
                    break;
                case 73:
                    spellCritPerInt = 0.0101f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.004796f;
                    break;
                case 74:
                    spellCritPerInt = 0.0093f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.004555f;
                    break;
                case 75:
                    spellCritPerInt = 0.0087f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.004327f;
                    break;
                case 76:
                    spellCritPerInt = 0.0081f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.004110f;
                    break;
                case 77:
                    spellCritPerInt = 0.0075f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.003903f;
                    break;
                case 78:
                    spellCritPerInt = 0.007f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.003708f;
                    break;
                case 79:
                    spellCritPerInt = 0.0065f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.003522f;
                    break;
                case 80:
                    spellCritPerInt = 0.006f;
                    spellCritBase = 0.9075f;
                    baseRegen = 0.003345f;
                    break;
            }
            float spellCrit = 0.01f * (baseStats.Intellect * spellCritPerInt + spellCritBase) + 0.01f * talents.ArcaneInstability + 0.15f * 0.02f * talents.ArcaneConcentration * talents.ArcanePotency + baseStats.CritRating / 1400f * calculationOptions.LevelScalingFactor + baseStats.SpellCrit + talents.FocusMagic * 0.03f * (1 - (float)Math.Pow(1 - calculationOptions.FocusMagicTargetCritRate, 10.0)) + 0.01f * talents.Pyromaniac;

            calculationResult.BaseCritRate = spellCrit;
            calculationResult.BaseArcaneCritRate = spellCrit;
            calculationResult.BaseFireCritRate = spellCrit + 0.02f * talents.CriticalMass;
            calculationResult.BaseFrostFireCritRate = spellCrit + 0.02f * talents.CriticalMass;
            calculationResult.BaseFrostCritRate = spellCrit;
            calculationResult.BaseNatureCritRate = spellCrit;
            calculationResult.BaseShadowCritRate = spellCrit;
            calculationResult.BaseHolyCritRate = spellCrit;

            float levelScalingFactor = calculationOptions.LevelScalingFactor;
            if (!calculationOptions.EffectDisableManaSources)
            {
                calculationResult.SpiritRegen = (0.001f + baseStats.Spirit * baseRegen * (float)Math.Sqrt(baseStats.Intellect)) * calculationOptions.EffectRegenMultiplier;
                calculationResult.ManaRegen = calculationResult.SpiritRegen + baseStats.Mp5 / 5f + 15732 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * baseStats.Mana / calculationOptions.FightDuration + baseStats.ManaRestoreFromMaxManaPerSecond * baseStats.Mana;
                calculationResult.ManaRegen5SR = calculationResult.SpiritRegen * baseStats.SpellCombatManaRegeneration + baseStats.Mp5 / 5f + 15732 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * baseStats.Mana / calculationOptions.FightDuration + baseStats.ManaRestoreFromMaxManaPerSecond * baseStats.Mana;
            }
            calculationResult.HealthRegen = 0.0312f * baseStats.Spirit + baseStats.Hp5 / 5f;
            calculationResult.HealthRegenCombat = baseStats.Hp5 / 5f;
            if (playerLevel < 75)
            {
                calculationResult.ManaRegenDrinking = calculationResult.ManaRegen + 240f;
                calculationResult.HealthRegenEating = calculationResult.HealthRegen + 250f;
            }
            else if (playerLevel < 80)
            {
                calculationResult.ManaRegenDrinking = calculationResult.ManaRegen + 306f;
                calculationResult.HealthRegenEating = calculationResult.HealthRegen + 440f;
            }
            else
            {
                calculationResult.ManaRegenDrinking = calculationResult.ManaRegen + 640f;
                calculationResult.HealthRegenEating = calculationResult.HealthRegen + 750f;
            }
            calculationResult.MeleeMitigation = (1 - 1 / (1 + 0.1f * baseStats.Armor / (8.5f * (targetLevel + 4.5f * (targetLevel - 59)) + 40)));
            calculationResult.Defense = 5 * playerLevel + baseStats.DefenseRating / 4.918498039f; // this is for level 80 only
            int molten = (armor == "Molten Armor") ? 1 : 0;
            float resilienceFactor = 2875f;
            calculationResult.PhysicalCritReduction = (0.04f * (calculationResult.Defense - 5 * calculationOptions.PlayerLevel) / 100 + baseStats.Resilience / resilienceFactor * levelScalingFactor + molten * 0.05f);
            calculationResult.SpellCritReduction = (baseStats.Resilience / resilienceFactor * levelScalingFactor + molten * 0.05f);
            calculationResult.CritDamageReduction = (baseStats.Resilience / resilienceFactor * 2.2f * levelScalingFactor);
            calculationResult.Dodge = 0.043545f + 0.01f / (0.006650f + 0.953f / ((0.04f * (calculationResult.Defense - 5 * playerLevel)) / 100f + baseStats.DodgeRating / 1200 * levelScalingFactor + (baseStats.Agility - 46f) * 0.0195f));

            calculationResult.BaseArcaneCritBonus = (1 + (1.5f * (1 + baseStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * talents.SpellPower + 0.1f * talents.Burnout + baseStats.CritBonusDamage));
            calculationResult.BaseFireCritBonus = (1 + (1.5f * (1 + baseStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * talents.SpellPower + 0.1f * talents.Burnout + baseStats.CritBonusDamage)) * (1 + 0.08f * talents.Ignite);
            calculationResult.BaseFrostCritBonus = (1 + (1.5f * (1 + baseStats.BonusSpellCritMultiplier) - 1) * (1 + talents.IceShards / 3.0f + 0.25f * talents.SpellPower + 0.1f * talents.Burnout + baseStats.CritBonusDamage));
            calculationResult.BaseFrostFireCritBonus = (1 + (1.5f * (1 + baseStats.BonusSpellCritMultiplier) - 1) * (1 + talents.IceShards / 3.0f + 0.25f * talents.SpellPower + 0.1f * talents.Burnout + baseStats.CritBonusDamage)) * (1 + 0.08f * talents.Ignite);
            calculationResult.BaseNatureCritBonus = (1 + (1.5f * (1 + baseStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * talents.SpellPower + baseStats.CritBonusDamage)); // unknown if affected by burnout
            calculationResult.BaseShadowCritBonus = (1 + (1.5f * (1 + baseStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * talents.SpellPower + baseStats.CritBonusDamage));
            calculationResult.BaseHolyCritBonus = (1 + (1.5f * (1 + baseStats.BonusSpellCritMultiplier) - 1) * (1 + 0.25f * talents.SpellPower + baseStats.CritBonusDamage));

            calculationResult.BaseCastingSpeed = (1 + baseStats.HasteRating / 995f * levelScalingFactor) * (1f + baseStats.SpellHaste) * (1f + 0.02f * character.MageTalents.NetherwindPresence) * calculationOptions.EffectHasteMultiplier;
            calculationResult.BaseGlobalCooldown = Math.Max(Spell.GlobalCooldownLimit, 1.5f / calculationResult.BaseCastingSpeed);

            calculationResult.IncomingDamageAmpMelee = (1 - 0.02 * talents.PrismaticCloak) * (1 - 0.01 * talents.ArcticWinds) * (1 - calculationResult.MeleeMitigation) * (1 - calculationResult.Dodge) * (1 - 0.5f * calculationResult.CritDamageReduction);
            calculationResult.IncomingDamageAmpPhysical = (1 - 0.02 * talents.PrismaticCloak) * (1 - 0.01 * talents.ArcticWinds) * (1 - calculationResult.MeleeMitigation) * (1 - 0.5f * calculationResult.CritDamageReduction);
            calculationResult.IncomingDamageAmpArcane = (1 - 0.02 * talents.PrismaticCloak) * (1 + 0.01 * talents.PlayingWithFire) * (1 - 0.02 * talents.FrozenCore) * (1 - StatConversion.GetAverageResistance(targetLevel, playerLevel, baseStats.ArcaneResistance, 0)) * (1 - 0.5f * calculationResult.CritDamageReduction);
            calculationResult.IncomingDamageAmpFire = (1 - 0.02 * talents.PrismaticCloak) * (1 + 0.01 * talents.PlayingWithFire) * (1 - 0.02 * talents.FrozenCore) * (1 - StatConversion.GetAverageResistance(targetLevel, playerLevel, baseStats.FireResistance, 0)) * (1 - 0.5f * calculationResult.CritDamageReduction);
            calculationResult.IncomingDamageAmpFrost = (1 - 0.02 * talents.PrismaticCloak) * (1 + 0.01 * talents.PlayingWithFire) * (1 - 0.02 * talents.FrozenCore) * (1 - StatConversion.GetAverageResistance(targetLevel, playerLevel, baseStats.FrostResistance, 0)) * (1 - 0.5f * calculationResult.CritDamageReduction);
            calculationResult.IncomingDamageAmpNature = (1 - 0.02 * talents.PrismaticCloak) * (1 + 0.01 * talents.PlayingWithFire) * (1 - 0.02 * talents.FrozenCore) * (1 - StatConversion.GetAverageResistance(targetLevel, playerLevel, baseStats.NatureResistance, 0)) * (1 - 0.5f * calculationResult.CritDamageReduction);
            calculationResult.IncomingDamageAmpShadow = (1 - 0.02 * talents.PrismaticCloak) * (1 + 0.01 * talents.PlayingWithFire) * (1 - 0.02 * talents.FrozenCore) * (1 - StatConversion.GetAverageResistance(targetLevel, playerLevel, baseStats.ShadowResistance, 0)) * (1 - 0.5f * calculationResult.CritDamageReduction);
            calculationResult.IncomingDamageAmpHoly = (1 - 0.02 * talents.PrismaticCloak) * (1 + 0.01 * talents.PlayingWithFire) * (1 - 0.02 * talents.FrozenCore) * (1 - 0.5f * calculationResult.CritDamageReduction);

            calculationResult.IncomingDamageDpsMelee = calculationResult.IncomingDamageAmpMelee * (calculationOptions.MeleeDps * (1 + Math.Max(0, calculationOptions.MeleeCrit / 100.0 - calculationResult.PhysicalCritReduction) * (2 * (1 - calculationResult.CritDamageReduction) - 1)) + calculationOptions.MeleeDot);
            calculationResult.IncomingDamageDpsPhysical = calculationResult.IncomingDamageAmpPhysical * (calculationOptions.PhysicalDps * (1 + Math.Max(0, calculationOptions.PhysicalCrit / 100.0 - calculationResult.PhysicalCritReduction) * (2 * (1 - calculationResult.CritDamageReduction) - 1)) + calculationOptions.PhysicalDot);
            calculationResult.IncomingDamageDpsArcane = calculationResult.IncomingDamageAmpArcane * (calculationOptions.ArcaneDps * (1 + Math.Max(0, calculationOptions.ArcaneCrit / 100.0 - calculationResult.SpellCritReduction) * (2 * (1 - calculationResult.CritDamageReduction) - 1)) + calculationOptions.ArcaneDot);
            calculationResult.IncomingDamageDpsFire = calculationResult.IncomingDamageAmpFire * (calculationOptions.FireDps * (1 + Math.Max(0, calculationOptions.FireCrit / 100.0 - calculationResult.SpellCritReduction) * (2 * (1 - calculationResult.CritDamageReduction) - 1)) + calculationOptions.FireDot);
            calculationResult.IncomingDamageDpsFrost = calculationResult.IncomingDamageAmpFrost * (calculationOptions.FrostDps * (1 + Math.Max(0, calculationOptions.FrostCrit / 100.0 - calculationResult.SpellCritReduction) * (2 * (1 - calculationResult.CritDamageReduction) - 1)) + calculationOptions.FrostDot);
            calculationResult.IncomingDamageDpsNature = calculationResult.IncomingDamageAmpNature * (calculationOptions.NatureDps * (1 + Math.Max(0, calculationOptions.NatureCrit / 100.0 - calculationResult.SpellCritReduction) * (2 * (1 - calculationResult.CritDamageReduction) - 1)) + calculationOptions.NatureDot);
            calculationResult.IncomingDamageDpsShadow = calculationResult.IncomingDamageAmpShadow * (calculationOptions.ShadowDps * (1 + Math.Max(0, calculationOptions.ShadowCrit / 100.0 - calculationResult.SpellCritReduction) * (2 * (1 - calculationResult.CritDamageReduction) - 1)) + calculationOptions.ShadowDot);
            calculationResult.IncomingDamageDpsHoly = calculationResult.IncomingDamageAmpHoly * (calculationOptions.HolyDps * (1 + Math.Max(0, calculationOptions.HolyCrit / 100.0 - calculationResult.SpellCritReduction) * (2 * (1 - calculationResult.CritDamageReduction) - 1)) + calculationOptions.HolyDot);

            calculationResult.IncomingDamageDps = calculationResult.IncomingDamageDpsMelee + calculationResult.IncomingDamageDpsPhysical + calculationResult.IncomingDamageDpsArcane + calculationResult.IncomingDamageDpsFire + calculationResult.IncomingDamageDpsFrost + calculationResult.IncomingDamageDpsShadow + calculationResult.IncomingDamageDpsNature + calculationResult.IncomingDamageDpsHoly;
            float incanterSpellPower = Math.Min((float)Math.Min(calculationOptions.AbsorptionPerSecond, calculationResult.IncomingDamageDps) * 0.05f * talents.IncantersAbsorption * 10, 0.05f * baseStats.Health);
            if (calculationOptions.AbsorptionPerSecond > calculationResult.IncomingDamageDps)
            {
                calculationResult.IncomingDamageDps = 0.0f;
            }
            else
            {
                calculationResult.IncomingDamageDps -= calculationOptions.AbsorptionPerSecond;
            }

            calculationResult.BaseArcaneSpellPower = baseStats.SpellArcaneDamageRating + baseStats.SpellPower + incanterSpellPower;
            calculationResult.BaseFireSpellPower = baseStats.SpellFireDamageRating + baseStats.SpellPower + incanterSpellPower;
            calculationResult.BaseFrostSpellPower = baseStats.SpellFrostDamageRating + baseStats.SpellPower + incanterSpellPower;
            calculationResult.BaseNatureSpellPower = baseStats.SpellNatureDamageRating + baseStats.SpellPower + incanterSpellPower;
            calculationResult.BaseShadowSpellPower = baseStats.SpellShadowDamageRating + baseStats.SpellPower + incanterSpellPower;
            calculationResult.BaseHolySpellPower = /* baseStats.SpellHolyDamageRating + */ baseStats.SpellPower + incanterSpellPower;
        }

        private float EvaluateSurvivability(Stats characterStats)
        {
            double ampMelee = calculationResult.IncomingDamageAmpMelee;
            double ampPhysical = calculationResult.IncomingDamageAmpPhysical;
            double ampArcane = calculationResult.IncomingDamageAmpArcane;
            double ampFire = calculationResult.IncomingDamageAmpFire;
            double ampFrost = calculationResult.IncomingDamageAmpFrost;
            double ampNature = calculationResult.IncomingDamageAmpNature;
            double ampShadow = calculationResult.IncomingDamageAmpShadow;
            double ampHoly = calculationResult.IncomingDamageAmpHoly;

            double melee = calculationResult.IncomingDamageDpsMelee;
            double physical = calculationResult.IncomingDamageDpsPhysical;
            double arcane = calculationResult.IncomingDamageDpsArcane;
            double fire = calculationResult.IncomingDamageDpsFire;
            double frost = calculationResult.IncomingDamageDpsFrost;
            double nature = calculationResult.IncomingDamageDpsNature;
            double shadow = calculationResult.IncomingDamageDpsShadow;
            double holy = calculationResult.IncomingDamageDpsHoly;

            double burstWindow = calculationOptions.BurstWindow;
            double burstImpacts = calculationOptions.BurstImpacts;

            // B(n, p) ~ N(np, np(1-p))
            // n = burstImpacts
            // Xi ~ ampi * (dpsi * (1 + B(n, criti) / n * critMulti) + doti)
            //    ~ ampi * (dpsi * (1 + N(n * criti, n * criti * (1 - criti)) / n * critMulti) + doti)
            //    ~ N(ampi * (doti + dpsi * (1 + critMulti * criti)), ampi^2 * dpsi^2 * critMulti^2 / n * criti * (1 - criti))
            // X = sum Xi ~ N(sum ampi * (doti + dpsi * (1 + critMulti * criti)), sum ampi^2 * dpsi^2 * critMulti^2 / n * criti * (1 - criti))
            // H = Health + hp5 / 5 * burstWindow
            // P(burstWindow * sum Xi >= H) = 1 - P(burstWindow * sum Xi <= H) = 1 / 2 * (1 - Erf((H - mu) / (sigma * sqrt(2)))) =
            //                = 1 / 2 * (1 - Erf((H / burstWindow - [sum ampi * (doti + dpsi * (1 + critMulti * criti))]) / sqrt(2 * [sum ampi^2 * dpsi^2 * critMulti^2 / n * criti * (1 - criti)])))

            double meleeVar = Math.Pow(ampMelee * calculationOptions.MeleeDps * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1), 2) / burstImpacts * Math.Max(0, calculationOptions.MeleeCrit / 100.0 - calculationResult.BaseState.PhysicalCritReduction) * (1 - Math.Max(0, calculationOptions.MeleeCrit / 100.0 - calculationResult.BaseState.PhysicalCritReduction));
            double physicalVar = Math.Pow(ampPhysical * calculationOptions.PhysicalDps * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1), 2) / burstImpacts * Math.Max(0, calculationOptions.PhysicalCrit / 100.0 - calculationResult.BaseState.PhysicalCritReduction) * (1 - Math.Max(0, calculationOptions.PhysicalCrit / 100.0 - calculationResult.BaseState.PhysicalCritReduction));
            double arcaneVar = Math.Pow(ampArcane * calculationOptions.ArcaneDps * (1.75 * (1 - calculationResult.BaseState.CritDamageReduction) - 1), 2) / burstImpacts * Math.Max(0, calculationOptions.ArcaneCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (1 - Math.Max(0, calculationOptions.ArcaneCrit / 100.0 - calculationResult.BaseState.SpellCritReduction));
            double fireVar = Math.Pow(ampFire * calculationOptions.FireDps * (2.1 * (1 - calculationResult.BaseState.CritDamageReduction) - 1), 2) / burstImpacts * Math.Max(0, calculationOptions.FireCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (1 - Math.Max(0, calculationOptions.FireCrit / 100.0 - calculationResult.BaseState.SpellCritReduction));
            double frostVar = Math.Pow(ampFrost * calculationOptions.FrostDps * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1), 2) / burstImpacts * Math.Max(0, calculationOptions.FrostCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (1 - Math.Max(0, calculationOptions.FrostCrit / 100.0 - calculationResult.BaseState.SpellCritReduction));
            double holyVar = Math.Pow(ampHoly * calculationOptions.HolyDps * (1.5 * (1 - calculationResult.BaseState.CritDamageReduction) - 1), 2) / burstImpacts * Math.Max(0, calculationOptions.HolyCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (1 - Math.Max(0, calculationOptions.HolyCrit / 100.0 - calculationResult.BaseState.SpellCritReduction));
            double natureVar = Math.Pow(ampNature * calculationOptions.NatureDps * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1), 2) / burstImpacts * Math.Max(0, calculationOptions.NatureCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (1 - Math.Max(0, calculationOptions.NatureCrit / 100.0 - calculationResult.BaseState.SpellCritReduction));
            double shadowVar = Math.Pow(ampShadow * calculationOptions.ShadowDps * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1), 2) / burstImpacts * Math.Max(0, calculationOptions.ShadowCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (1 - Math.Max(0, calculationOptions.ShadowCrit / 100.0 - calculationResult.BaseState.SpellCritReduction));

            double Xmean = melee + physical + arcane + fire + frost + holy + nature + shadow;
            double Xvar = meleeVar + physicalVar + arcaneVar + fireVar + frostVar + holyVar + natureVar + shadowVar;

            // T = healing response time ~ N(Tmean, Tvar)
            // T * X ~ N(Tmean * Xmean, Tvar * Xvar + Tmean^2 * Xvar + Xmean^2 * Tvar)   // approximation reasonable for high Tmean / sqrt(Tvar)
            // P(T * X >= H) = 1 / 2 * (1 - Erf((H - mean) / (sigma * sqrt(2)))) =
            //               = 1 / 2 * (1 - Erf((H - mean) / sqrt(2 * var)))
            //               = 1 / 2 * (1 - Erf((H - Tmean * Xmean) / sqrt(2 * (Tvar * Xvar + Tmean^2 * Xvar + Xmean^2 * Tvar))))

            // Tvar := Tk * Tmean^2,   Tk <<< 1

            // P(T * X >= H) = 1 / 2 * (1 - Erf((H / Tmean - Xmean) / sqrt(2 * (Xvar * (Tk + 1) + Xmean^2 * Tk))))

            double Tk = 0.01;

            calculationResult.ChanceToDie = (float)(0.5f * (1f - SpecialFunction.Erf((characterStats.Health / burstWindow + characterStats.Hp5 / 5 - Xmean) / Math.Sqrt(2 * (Xvar * (1 + Tk) + Xmean * Xmean * Tk)))));
            calculationResult.MeanIncomingDps = (float)Xmean;

            //double maxTimeToDie = 1.0 / (1 - calculationOptions.ChanceToLiveLimit / 100.0) - 1;
            //double timeToDie = Math.Min(1.0 / calculatedStats.ChanceToDie - 1, maxTimeToDie);

            //calculatedStats.SubPoints[1] = calculatedStats.BasicStats.Health * calculationOptions.SurvivabilityRating + (float)(calculationOptions.ChanceToLiveScore * timeToDie / maxTimeToDie);
            float ret = calculationResult.BaseStats.Health * calculationOptions.SurvivabilityRating + (float)(calculationOptions.ChanceToLiveScore * Math.Pow(1 - calculationResult.ChanceToDie, 0.1));
            if (float.IsNaN(ret)) ret = 0f;
            return ret;
        }

        private void AddSegmentTicks(List<double> ticks, double cooldownDuration)
        {
            for (int i = 0; i * 0.5 * cooldownDuration < calculationOptions.FightDuration; i++)
            {
                ticks.Add(i * 0.5 * cooldownDuration);
            }
        }

        private void AddEffectTicks(List<double> ticks, double cooldownDuration, double effectDuration)
        {
            for (int i = 0; i * cooldownDuration + effectDuration < calculationOptions.FightDuration; i++)
            {
                ticks.Add(i * cooldownDuration + effectDuration);
                if (i * cooldownDuration + effectDuration > calculationOptions.FightDuration - effectDuration)
                {
                    ticks.Add(calculationOptions.FightDuration - effectDuration);
                }
            }
        }

        // rawStats is only valid for calculationOptions.EvocationWeapon + calculationOptions.EvocationSpirit > 0, otherwise it is the same as baseStats
#if SILVERLIGHT
        private void ConstructProblem(Item additionalItem, CalculationsMage calculations, Stats rawStats, Stats baseStats, out List<double> tpsList)
#else
        private unsafe void ConstructProblem(Item additionalItem, CalculationsMage calculations, Stats rawStats, Stats baseStats, out List<double> tpsList)
#endif
        {
            segmentList = new List<Segment>();
            if (segmentCooldowns)
            {
                List<double> ticks = new List<double>();
                if (calculationOptions.VariableSegmentDuration)
                {
                    // variable segment durations to get a better grasp on varied cooldown durations
                    // create ticks in intervals of half cooldown duration
                    if (potionOfSpeedAvailable || potionOfWildMagicAvailable || manaPotionAvailable)
                    {
                        AddSegmentTicks(ticks, 120.0);
                    }
                    if (arcanePowerAvailable)
                    {
                        AddSegmentTicks(ticks, calculationResult.ArcanePowerCooldown);
                        //AddEffectTicks(ticks, calculationResult.ArcanePowerCooldown, calculationResult.ArcanePowerDuration);
                    }
                    if (combustionAvailable) AddSegmentTicks(ticks, 300.0);
                    if (drumsOfBattleAvailable) AddSegmentTicks(ticks, 120.0);
                    if (flameCapAvailable || calculationOptions.ManaGemEnabled || manaGemEffectAvailable)
                    {
                        ticks.Add(15.0); // get a better grasp on mana overflow
                        AddSegmentTicks(ticks, 60.0);
                    }
                    if (icyVeinsAvailable)
                    {
                        AddSegmentTicks(ticks, calculationResult.IcyVeinsCooldown);
                        //if (!coldsnapAvailable) AddEffectTicks(ticks, calculationResult.IcyVeinsCooldown, 20.0);
                    }
                    if (waterElementalAvailable) AddSegmentTicks(ticks, calculationResult.WaterElementalCooldown);
                    if (trinket1Available) AddSegmentTicks(ticks, trinket1Cooldown);
                    if (trinket2Available) AddSegmentTicks(ticks, trinket2Cooldown);
                }
                else
                {
                    for (int i = 0; calculationOptions.FixedSegmentDuration * i < calculationOptions.FightDuration - 0.00001; i++)
                    {
                        //segmentList.Add(new Segment() { TimeStart = calculationOptions.FixedSegmentDuration * i, Duration = Math.Min(calculationOptions.FixedSegmentDuration, calculationOptions.FightDuration - calculationOptions.FixedSegmentDuration * i) });
                        ticks.Add(calculationOptions.FixedSegmentDuration * i);
                    }
                }
                if (!string.IsNullOrEmpty(calculationOptions.AdditionalSegmentSplits))
                {
                    string[] splits = calculationOptions.AdditionalSegmentSplits.Split(',');
                    foreach (string split in splits)
                    {
                        double tick;
                        if (double.TryParse(split.Trim(), out tick))
                        {
                            ticks.Add(tick);
                        }
                    }
                }
                if (moltenFuryAvailable)
                {
                    ticks.Add((1 - calculationOptions.MoltenFuryPercentage) * calculationOptions.FightDuration);
                }
                if (!string.IsNullOrEmpty(calculationOptions.CooldownRestrictions) && calculationOptions.CooldownRestrictionList == null)
                {
                    StateDescription.Scanner scanner = new StateDescription.Scanner();
                    StateDescription.Parser parser = new StateDescription.Parser(scanner);
                    calculationOptions.CooldownRestrictionList = new List<CooldownRestriction>();
                    string[] lines = calculationOptions.CooldownRestrictions.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
#if SILVERLIGHT
                        string[] tokens = line.Split(new char[] { '-', ':' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            tokens[i] = tokens[i].Trim();
                        }
#else
                        string[] tokens = line.Split(new char[] { '-', ':', ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
#endif
                        if (tokens.Length == 3)
                        {
                            CooldownRestriction restriction = new CooldownRestriction();
                            double value;
                            if (!double.TryParse(tokens[0], out value)) continue;
                            restriction.TimeStart = value;
                            if (!double.TryParse(tokens[1], out value)) continue;
                            restriction.TimeEnd = value;
                            StateDescription.ParseTree parseTree = parser.Parse(tokens[2]);
                            if (parseTree != null && parseTree.Errors.Count == 0)
                            {
                                try
                                {
                                    restriction.IsMatch = parseTree.Compile();
                                    calculationOptions.CooldownRestrictionList.Add(restriction);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
                if (calculationOptions.CooldownRestrictionList != null)
                {
                    foreach (CooldownRestriction restriction in calculationOptions.CooldownRestrictionList)
                    {
                        ticks.Add(restriction.TimeStart);
                        ticks.Add(restriction.TimeEnd);
                    }
                }
                ticks.Sort();
                for (int i = 0; i < ticks.Count; i++)
                {
                    if ((i == 0 || ticks[i] > ticks[i - 1] + 0.00001) && ticks[i] < calculationOptions.FightDuration - 0.00001)
                    {
                        if (segmentList.Count > 0)
                        {
                            segmentList[segmentList.Count - 1].Duration = ticks[i] - ticks[i - 1];
                        }
                        segmentList.Add(new Segment() { TimeStart = ticks[i] });
                    }
                }
                segmentList[segmentList.Count - 1].Duration = calculationOptions.FightDuration - segmentList[segmentList.Count - 1].TimeStart;
            }
            else
            {
                segmentList.Add(new Segment() { TimeStart = 0, Duration = calculationOptions.FightDuration });
            }
            for (int i = 0; i < segmentList.Count; i++)
            {
                segmentList[i].Index = i;
            }
            //segments = (segmentCooldowns) ? (int)Math.Ceiling(calculationOptions.FightDuration / segmentDuration) : 1;
            segmentColumn = new int[segmentList.Count + 1];

            calculationResult.StartingMana = Math.Min(baseStats.Mana, calculationResult.BaseState.ManaRegenDrinking * calculationOptions.DrinkingTime);
            double maxDrinkingTime = Math.Min(30, (baseStats.Mana - calculationResult.StartingMana) / calculationResult.BaseState.ManaRegenDrinking);
            bool drinkingEnabled = (maxDrinkingTime > 0.000001);

            needsTimeExtension = false;
            bool afterFightRegen = calculationOptions.FarmingMode;
            conjureManaGem = calculationOptions.ManaGemEnabled && calculationOptions.FightDuration > 500.0f;
            wardsAvailable = calculationResult.IncomingDamageDpsFire + calculationResult.IncomingDamageDpsFrost > 0.0 && talents.FrostWarding > 0;

            minimizeTime = false;
            if (calculationOptions.TargetDamage > 0)
            {
                minimizeTime = true;
            }
            if (minimizeTime) needsTimeExtension = true;

            if (segmentCooldowns && (flameCapAvailable || manaGemEffectAvailable)) restrictManaUse = true;
            if (calculationOptions.UnlimitedMana)
            {
                restrictManaUse = false;
                integralMana = false;
            }
            if (restrictManaUse) segmentNonCooldowns = true;
            if (restrictThreat) segmentNonCooldowns = true;

            int rowCount = ConstructRows(minimizeTime, drinkingEnabled, needsTimeExtension, afterFightRegen);

            lp = new SolverLP(rowCount, 9 + (12 + (calculationOptions.EnableHastedEvocation ? 6 : 0) + spellList.Count * stateList.Count) * segmentList.Count, calculationResult, segmentList.Count);
            if (needsDisplayCalculations)
            {
                tpsList = new List<double>();
            }
            else
            {
                tpsList = null;
            }
            double tps;
            if (needsSolutionVariables)
            {
                calculationResult.SolutionVariable = solutionVariable = new List<SolutionVariable>();
            }
            calculationResult.SegmentList = segmentList;

#if !SILVERLIGHT
            fixed (double* pRowScale = lp.ArraySet.rowScale, pColumnScale = lp.ArraySet.columnScale, pCost = lp.ArraySet._cost, pData = lp.ArraySet.SparseMatrixData, pValue = lp.ArraySet.SparseMatrixValue)
            fixed (int* pRow = lp.ArraySet.SparseMatrixRow, pCol = lp.ArraySet.SparseMatrixCol)
#endif
            {
#if SILVERLIGHT
                lp.BeginSafe(lp.ArraySet.rowScale, lp.ArraySet.columnScale, lp.ArraySet._cost, lp.ArraySet.SparseMatrixData, lp.ArraySet.SparseMatrixValue, lp.ArraySet.SparseMatrixRow, lp.ArraySet.SparseMatrixCol);
#else
                lp.BeginUnsafe(pRowScale, pColumnScale, pCost, pData, pValue, pRow, pCol);
#endif

                #region Set LP Scaling
                lp.SetRowScaleUnsafe(rowManaRegen, 0.1);
                lp.SetRowScaleUnsafe(rowManaGem, 40.0);
                lp.SetRowScaleUnsafe(rowPotion, 40.0);
                lp.SetRowScaleUnsafe(rowManaGemMax, 40.0);
                lp.SetRowScaleUnsafe(rowManaPotion, 40.0);
                lp.SetRowScaleUnsafe(rowManaGemFlameCap, 40.0);
                lp.SetRowScaleUnsafe(rowCombustion, 10.0);
                lp.SetRowScaleUnsafe(rowHeroismCombustion, 10.0);
                lp.SetRowScaleUnsafe(rowMoltenFuryCombustion, 10.0);
                lp.SetRowScaleUnsafe(rowDrumsOfBattleActivation, 30.0);
                lp.SetRowScaleUnsafe(rowThreat, 0.001);
                lp.SetRowScaleUnsafe(rowCount, 0.05);
                if (restrictManaUse)
                {
                    for (int ss = 0; ss < segmentList.Count - 1; ss++)
                    {
                        lp.SetRowScaleUnsafe(rowSegmentManaOverflow + ss, 0.1);
                        lp.SetRowScaleUnsafe(rowSegmentManaUnderflow + ss, 0.1);
                    }
                }
                if (restrictThreat)
                {
                    for (int ss = 0; ss < segmentList.Count - 1; ss++)
                    {
                        lp.SetRowScaleUnsafe(rowSegmentThreat + ss, 0.001);
                    }
                }
                #endregion

                float threatFactor = (1 + baseStats.ThreatIncreaseMultiplier) * (1 - baseStats.ThreatReductionMultiplier);
                float dpsTime = calculationOptions.DpsTime;
                float silenceTime = calculationOptions.EffectShadowSilenceFrequency * calculationOptions.EffectShadowSilenceDuration * Math.Max(1 - baseStats.ShadowResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
                if (1 - silenceTime < dpsTime) dpsTime = 1 - silenceTime;

                #region Formulate LP
                #region Idle Regen
                int column = -1;
                double manaRegen;
                int idleRegenSegments = (restrictManaUse) ? segmentList.Count : 1;
                manaRegen = -(calculationResult.BaseState.ManaRegen * (1 - calculationOptions.Fragmentation) + calculationResult.BaseState.ManaRegen5SR * calculationOptions.Fragmentation);
                for (int segment = 0; segment < idleRegenSegments; segment++)
                {
                    column = lp.AddColumnUnsafe();
                    lp.SetColumnUpperBound(column, (idleRegenSegments > 1) ? segmentList[segment].Duration : calculationOptions.FightDuration);
                    if (segment == 0) calculationResult.ColumnIdleRegen = column;
                    if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.IdleRegen, Segment = segment, State = calculationResult.BaseState });
                    if (needsDisplayCalculations) tpsList.Add(0.0);
                    lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                    lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetElementUnsafe(rowDpsTime, column, -1.0);
                    lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                    if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                    if (restrictManaUse)
                    {
                        for (int ss = segment; ss < segmentList.Count - 1; ss++)
                        {
                            lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                            lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                        }
                    }
                }
                #endregion
                #region Wand
                if (character.Ranged != null && character.Ranged.Item.Type == ItemType.Wand)
                {
                    int wandSegments = (restrictManaUse) ? segmentList.Count : 1;
                    Spell w = new WandTemplate(calculationResult, (MagicSchool)character.Ranged.Item.DamageType, character.Ranged.Item.MinDamage, character.Ranged.Item.MaxDamage, character.Ranged.Item.Speed).GetSpell(calculationResult.BaseState);
                    calculationResult.Wand = w;
                    Cycle wand = w;
                    manaRegen = wand.ManaPerSecond;
                    for (int segment = 0; segment < wandSegments; segment++)
                    {
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnUpperBound(column, (wandSegments > 1) ? segmentList[segment].Duration : calculationOptions.FightDuration);
                        if (segment == 0) calculationResult.ColumnWand = column;
                        if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.Wand, Cycle = wand, Segment = segment, State = calculationResult.BaseState });
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                        lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                        lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = wand.ThreatPerSecond);
                        lp.SetElementUnsafe(rowTargetDamage, column, -wand.DamagePerSecond);
                        lp.SetCostUnsafe(column, minimizeTime ? -1 : wand.DamagePerSecond);
                        if (needsDisplayCalculations) tpsList.Add(tps);
                        if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                    }
                }
                #endregion
                #region Evocation
                calculationResult.EvocationStats = calculationResult.BaseStats;
                if (evocationAvailable)
                {
                    int evocationSegments = (restrictManaUse) ? segmentList.Count : 1;
                    double evocationDuration = (8f + baseStats.EvocationExtension) / calculationResult.BaseState.CastingSpeed;
                    calculationResult.EvocationDuration = evocationDuration;
                    calculationResult.EvocationDurationIV = evocationDuration / 1.2;
                    calculationResult.EvocationDurationHero = evocationDuration / 1.3;
                    calculationResult.EvocationDurationIVHero = evocationDuration / 1.2 / 1.3;
                    float evocationMana = baseStats.Mana;
                    calculationResult.EvocationRegen = calculationResult.BaseState.ManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed;
                    calculationResult.EvocationRegenIV = calculationResult.BaseState.ManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2;
                    calculationResult.EvocationRegenHero = calculationResult.BaseState.ManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.3;
                    calculationResult.EvocationRegenIVHero = calculationResult.BaseState.ManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2 * 1.3;
                    if (calculationOptions.EvocationWeapon + calculationOptions.EvocationSpirit > 0)
                    {
                        Stats evocationRawStats = rawStats.Clone();
                        if (character.MainHand != null)
                        {
                            evocationRawStats.Intellect -= character.MainHand.GetTotalStats().Intellect;
                            evocationRawStats.Spirit -= character.MainHand.GetTotalStats().Spirit;
                        }
                        if (character.OffHand != null)
                        {
                            evocationRawStats.Intellect -= character.OffHand.GetTotalStats().Intellect;
                            evocationRawStats.Spirit -= character.OffHand.GetTotalStats().Spirit;
                        }
                        if (character.Ranged != null)
                        {
                            evocationRawStats.Intellect -= character.Ranged.GetTotalStats().Intellect;
                            evocationRawStats.Spirit -= character.Ranged.GetTotalStats().Spirit;
                        }
                        if (character.MainHandEnchant != null)
                        {
                            evocationRawStats.Intellect -= character.MainHandEnchant.Stats.Intellect;
                            evocationRawStats.Spirit -= character.MainHandEnchant.Stats.Spirit;
                        }
                        evocationRawStats.Intellect += calculationOptions.EvocationWeapon;
                        evocationRawStats.Spirit += calculationOptions.EvocationSpirit;
                        Stats evocationStats = calculations.GetCharacterStats(character, additionalItem, evocationRawStats, calculationOptions);
                        float spiritFactor = 0.003345f;
                        double evoManaRegen5SR = ((0.001f + evocationStats.Spirit * spiritFactor * (float)Math.Sqrt(evocationStats.Intellect)) * evocationStats.SpellCombatManaRegeneration + evocationStats.Mp5 / 5f + 15732 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * baseStats.Mana / calculationOptions.FightDuration + evocationStats.ManaRestoreFromMaxManaPerSecond * evocationStats.Mana);
                        double evocationRegen = evoManaRegen5SR + 0.15f * evocationStats.Mana / 2f * calculationResult.BaseState.CastingSpeed;
                        if (evocationRegen > calculationResult.EvocationRegen)
                        {
                            calculationResult.EvocationStats = evocationStats;
                            evocationMana = evocationStats.Mana;
                            calculationResult.EvocationRegen = evocationRegen;
                            calculationResult.EvocationRegenIV = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2;
                            calculationResult.EvocationRegenHero = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.3;
                            calculationResult.EvocationRegenIVHero = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2 * 1.3;
                        }
                    }
                    if (calculationResult.EvocationRegen * evocationDuration > baseStats.Mana)
                    {
                        evocationDuration = baseStats.Mana / calculationResult.EvocationRegen;
                        calculationResult.EvocationDuration = evocationDuration;
                        calculationResult.EvocationDurationIV = baseStats.Mana / calculationResult.EvocationRegenIV;
                        calculationResult.EvocationDurationHero = baseStats.Mana / calculationResult.EvocationRegenHero;
                        calculationResult.EvocationDurationIVHero = baseStats.Mana / calculationResult.EvocationRegenIVHero;
                    }
                    if (segmentCooldowns && advancedConstraintsLevel >= 3)
                    {
                        calculationResult.MaxEvocation = Math.Max(1, 1 + Math.Floor((calculationOptions.FightDuration - evocationDuration) / calculationResult.EvocationCooldown));
                    }
                    else
                    {
                        calculationResult.MaxEvocation = Math.Max(1, 1 + Math.Floor((calculationOptions.FightDuration - 90f) / calculationResult.EvocationCooldown));
                    }
                    CastingState evoState = calculationResult.BaseState.Clone();
                    evoState.Cooldown |= Cooldown.Evocation;
                    CastingState evoStateIV = calculationResult.BaseState.Clone();
                    evoStateIV.Cooldown |= Cooldown.Evocation | Cooldown.IcyVeins;
                    CastingState evoStateHero = calculationResult.BaseState.Clone();
                    evoStateHero.Cooldown |= Cooldown.Evocation | Cooldown.Heroism;
                    CastingState evoStateIVHero = calculationResult.BaseState.Clone();
                    evoStateIVHero.Cooldown |= Cooldown.Evocation | Cooldown.IcyVeins | Cooldown.Heroism;
                    for (int segment = 0; segment < evocationSegments; segment++)
                    {
                        // base evocation
                        if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.Evocation, Segment = segment, State = evoState });
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnUpperBound(column, (evocationSegments > 1) ? evocationDuration : evocationDuration * calculationResult.MaxEvocation);
                        if (segment == 0) calculationResult.ColumnEvocation = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.EvocationRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, -calculationResult.EvocationRegen);
                        lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                        lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                        lp.SetElementUnsafe(rowEvocation, column, 1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 0.5f * threatFactor); // should split among all targets if more than one, assume one only
                        calculationResult.EvocationTps = tps;
                        lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                        if (needsDisplayCalculations) tpsList.Add(tps);
                        if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, -calculationResult.EvocationRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, calculationResult.EvocationRegen);
                            }
                            foreach (SegmentConstraint constraint in rowSegmentEvocation)
                            {
                                if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                        if (calculationOptions.EnableHastedEvocation)
                        {
                            if (icyVeinsAvailable)
                            {
                                // last tick of icy veins
                                if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.EvocationIV, Segment = segment, State = evoStateIV });
                                column = lp.AddColumnUnsafe();
                                lp.SetColumnUpperBound(column, (evocationSegments > 1) ? calculationResult.EvocationDurationIV : calculationResult.EvocationDurationIV * calculationResult.MaxEvocation);
                                lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.EvocationRegenIV);
                                lp.SetElementUnsafe(rowManaRegen, column, -calculationResult.EvocationRegenIV);
                                lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                                lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                                lp.SetElementUnsafe(rowIcyVeins, column, 1.0);
                                lp.SetElementUnsafe(rowEvocation, column, 1.2);
                                lp.SetElementUnsafe(rowEvocationIV, column, 1.0);
                                //lp.SetElementUnsafe(rowEvocationIVActivation, column, 1.0 - calculationResult.EvocationDurationIV / 0.1);
                                lp.SetElementUnsafe(rowThreat, column, tps = 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2 * 0.5f * threatFactor); // should split among all targets if more than one, assume one only
                                lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                                if (needsDisplayCalculations) tpsList.Add(tps);
                                if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                                if (segmentCooldowns)
                                {
                                    foreach (SegmentConstraint constraint in rowSegmentIcyVeins)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                                    }
                                }
                                if (restrictManaUse)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, -calculationResult.EvocationRegenIV);
                                        lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, calculationResult.EvocationRegenIV);
                                    }
                                    foreach (SegmentConstraint constraint in rowSegmentEvocation)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.2);
                                    }
                                }
                                if (restrictThreat)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                                    }
                                }
                                // remainder
                                if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.EvocationIV, Segment = segment, State = evoState });
                                column = lp.AddColumnUnsafe();
                                lp.SetColumnUpperBound(column, (evocationSegments > 1) ? calculationResult.EvocationDurationIV : calculationResult.EvocationDurationIV * calculationResult.MaxEvocation);
                                lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.EvocationRegenIV);
                                lp.SetElementUnsafe(rowManaRegen, column, -calculationResult.EvocationRegenIV);
                                lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                                lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                                lp.SetElementUnsafe(rowEvocation, column, 1.2);
                                lp.SetElementUnsafe(rowEvocationIV, column, 1.0);
                                //lp.SetElementUnsafe(rowEvocationIVActivation, column, 1.0);
                                lp.SetElementUnsafe(rowThreat, column, tps = 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2 * 0.5f * threatFactor); // should split among all targets if more than one, assume one only
                                lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                                if (needsDisplayCalculations) tpsList.Add(tps);
                                if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                                if (restrictManaUse)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, -calculationResult.EvocationRegenIV);
                                        lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, calculationResult.EvocationRegenIV);
                                    }
                                    foreach (SegmentConstraint constraint in rowSegmentEvocation)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.2);
                                    }
                                }
                                if (restrictThreat)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                                    }
                                }
                            }
                            if (heroismAvailable)
                            {
                                // last tick of heroism
                                if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.EvocationHero, Segment = segment, State = evoStateHero });
                                column = lp.AddColumnUnsafe();
                                lp.SetColumnUpperBound(column, (evocationSegments > 1) ? calculationResult.EvocationDurationHero : calculationResult.EvocationDurationHero);
                                lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.EvocationRegenHero);
                                lp.SetElementUnsafe(rowManaRegen, column, -calculationResult.EvocationRegenHero);
                                lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                                lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                                lp.SetElementUnsafe(rowHeroism, column, 1.0);
                                lp.SetElementUnsafe(rowEvocation, column, 1.3);
                                lp.SetElementUnsafe(rowEvocationHero, column, 1.0);
                                //lp.SetElementUnsafe(rowEvocationHeroActivation, column, 1.0 - calculationResult.EvocationDurationHero / 0.1);
                                lp.SetElementUnsafe(rowThreat, column, tps = 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.3 * 0.5f * threatFactor); // should split among all targets if more than one, assume one only
                                lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                                if (needsDisplayCalculations) tpsList.Add(tps);
                                if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                                if (restrictManaUse)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, -calculationResult.EvocationRegenHero);
                                        lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, calculationResult.EvocationRegenHero);
                                    }
                                    foreach (SegmentConstraint constraint in rowSegmentEvocation)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.3);
                                    }
                                }
                                if (restrictThreat)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                                    }
                                }
                                // remainder
                                if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.EvocationHero, Segment = segment, State = evoState });
                                column = lp.AddColumnUnsafe();
                                lp.SetColumnUpperBound(column, (evocationSegments > 1) ? calculationResult.EvocationDurationHero : calculationResult.EvocationDurationHero);
                                lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.EvocationRegenHero);
                                lp.SetElementUnsafe(rowManaRegen, column, -calculationResult.EvocationRegenHero);
                                lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                                lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                                lp.SetElementUnsafe(rowEvocation, column, 1.3);
                                lp.SetElementUnsafe(rowEvocationHero, column, 1.0);
                                //lp.SetElementUnsafe(rowEvocationHeroActivation, column, 1.0);
                                lp.SetElementUnsafe(rowThreat, column, tps = 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.3 * 0.5f * threatFactor); // should split among all targets if more than one, assume one only
                                lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                                if (needsDisplayCalculations) tpsList.Add(tps);
                                if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                                if (restrictManaUse)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, -calculationResult.EvocationRegenHero);
                                        lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, calculationResult.EvocationRegenHero);
                                    }
                                    foreach (SegmentConstraint constraint in rowSegmentEvocation)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.3);
                                    }
                                }
                                if (restrictThreat)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                                    }
                                }
                            }
                            if (icyVeinsAvailable && heroismAvailable)
                            {
                                // last tick of icy veins+heroism
                                if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.EvocationIVHero, Segment = segment, State = evoStateIVHero });
                                column = lp.AddColumnUnsafe();
                                lp.SetColumnUpperBound(column, (evocationSegments > 1) ? calculationResult.EvocationDurationIVHero : calculationResult.EvocationDurationIVHero);
                                lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.EvocationRegenIVHero);
                                lp.SetElementUnsafe(rowManaRegen, column, -calculationResult.EvocationRegenIVHero);
                                lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                                lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                                lp.SetElementUnsafe(rowHeroism, column, 1.0);
                                lp.SetElementUnsafe(rowIcyVeins, column, 1.0);
                                lp.SetElementUnsafe(rowHeroismIcyVeins, column, 1.0);
                                lp.SetElementUnsafe(rowEvocation, column, 1.2 * 1.3);
                                lp.SetElementUnsafe(rowEvocationHero, column, 1.2);
                                lp.SetElementUnsafe(rowEvocationIVHero, column, 1.0);
                                //lp.SetElementUnsafe(rowEvocationIVHeroActivation, column, 1.0 - calculationResult.EvocationDurationIVHero / 0.1);
                                lp.SetElementUnsafe(rowThreat, column, tps = 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2 * 1.3 * 0.5f * threatFactor); // should split among all targets if more than one, assume one only
                                lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                                if (needsDisplayCalculations) tpsList.Add(tps);
                                if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                                if (segmentCooldowns)
                                {
                                    foreach (SegmentConstraint constraint in rowSegmentIcyVeins)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                                    }
                                }
                                if (restrictManaUse)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, -calculationResult.EvocationRegenIVHero);
                                        lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, calculationResult.EvocationRegenIVHero);
                                    }
                                    foreach (SegmentConstraint constraint in rowSegmentEvocation)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.2 * 1.3);
                                    }
                                }
                                if (restrictThreat)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                                    }
                                }
                                // remainder
                                if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.EvocationIVHero, Segment = segment, State = evoState });
                                column = lp.AddColumnUnsafe();
                                lp.SetColumnUpperBound(column, (evocationSegments > 1) ? calculationResult.EvocationDurationIVHero : calculationResult.EvocationDurationIVHero);
                                lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.EvocationRegenIVHero);
                                lp.SetElementUnsafe(rowManaRegen, column, -calculationResult.EvocationRegenIVHero);
                                lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                                lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                                lp.SetElementUnsafe(rowEvocation, column, 1.2 * 1.3);
                                lp.SetElementUnsafe(rowEvocationHero, column, 1.2);
                                lp.SetElementUnsafe(rowEvocationIVHero, column, 1.0);
                                //lp.SetElementUnsafe(rowEvocationIVHeroActivation, column, 1.0);
                                lp.SetElementUnsafe(rowThreat, column, tps = 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2 * 1.3 * 0.5f * threatFactor); // should split among all targets if more than one, assume one only
                                lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                                if (needsDisplayCalculations) tpsList.Add(tps);
                                if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                                if (restrictManaUse)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, -calculationResult.EvocationRegenIVHero);
                                        lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, calculationResult.EvocationRegenIVHero);
                                    }
                                    foreach (SegmentConstraint constraint in rowSegmentEvocation)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.2 * 1.3);
                                    }
                                }
                                if (restrictThreat)
                                {
                                    for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                    {
                                        lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Mana Potion
                calculationResult.MaxManaPotion = 1;
                if (manaPotionAvailable)
                {
                    int manaPotionSegments = (segmentCooldowns && (potionOfWildMagicAvailable || restrictManaUse)) ? segmentList.Count : 1;
                    manaRegen = -(1 + baseStats.BonusManaPotion) * calculationResult.ManaPotionValue;
                    for (int segment = 0; segment < manaPotionSegments; segment++)
                    {
                        if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.ManaPotion, Segment = segment });
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnScaleUnsafe(column, 1.0 / 40.0);
                        lp.SetColumnUpperBound(column, (manaPotionSegments > 1) ? 1.0 : calculationResult.MaxManaPotion);
                        if (segment == 0) calculationResult.ColumnManaPotion = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                        lp.SetElementUnsafe(rowPotion, column, 1.0);
                        lp.SetElementUnsafe(rowManaPotion, column, 1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = (1 + baseStats.BonusManaPotion) * calculationResult.ManaPotionValue * 0.5f * threatFactor);
                        calculationResult.ManaPotionTps = tps;
                        //lp.SetElementUnsafe(rowManaPotionManaGem, column, 40.0);
                        lp.SetCostUnsafe(column, 0.0);
                        if (needsDisplayCalculations) tpsList.Add(tps);
                        /*if (segmentCooldowns && effectPotionAvailable)
                        {
                            for (int ss = 0; ss < segments; ss++)
                            {
                                double cool = 120;
                                int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentPotion + ss, column, 15.0);
                                if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                            }
                        }*/
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                    }
                }
                #endregion
                #region Mana Gem
                if (calculationOptions.ManaGemEnabled)
                {
                    int manaGemSegments = (segmentCooldowns && (flameCapAvailable || restrictManaUse)) ? segmentList.Count : 1;
                    if (segmentCooldowns && advancedConstraintsLevel >= 3)
                    {
                        calculationResult.MaxManaGem = 1 + (int)((calculationOptions.FightDuration - 1f) / 120f);
                    }
                    else
                    {
                        calculationResult.MaxManaGem = 1 + (int)((calculationOptions.FightDuration - 30f) / 120f);
                    }
                    double manaGemRegen = -(1 + baseStats.BonusManaGem) * calculationResult.ManaGemValue;
                    for (int segment = 0; segment < manaGemSegments; segment++)
                    {
                        if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.ManaGem, Segment = segment });
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnScaleUnsafe(column, 1.0 / 40.0);
                        lp.SetColumnUpperBound(column, (manaGemSegments > 1) ? 1.0 : calculationResult.MaxManaGem);
                        if (segment == 0) calculationResult.ColumnManaGem = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaGemRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaGemRegen);
                        lp.SetElementUnsafe(rowManaGem, column, 1.0);
                        lp.SetElementUnsafe(rowManaGemMax, column, 1.0);
                        lp.SetElementUnsafe(rowManaGemFlameCap, column, 1.0);
                        lp.SetElementUnsafe(rowManaGemEffectActivation, column, -1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = -manaGemRegen * 0.5f * threatFactor);
                        calculationResult.ManaGemTps = tps;
                        //lp.SetElementUnsafe(rowManaPotionManaGem, column, 40.0);
                        lp.SetCostUnsafe(column, 0.0);
                        if (needsDisplayCalculations) tpsList.Add(tps);
                        if (segmentCooldowns && flameCapAvailable)
                        {
                            foreach (SegmentConstraint constraint in rowSegmentManaGem)
                            {
                                if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 60.0);
                            }
                        }
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaGemRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaGemRegen);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                    }
                }
                #endregion
                #region Drums of Battle
                if (drumsOfBattleAvailable)
                {
                    List<CastingState> drumsStates = new List<CastingState>();
                    List<CastingState> mfDrumsStates = new List<CastingState>();
                    //int drums = 0x4;
                    //int drumsAndFC = 0x6;
                    // mf and drums = 132
                    // mf and drums and fc = 134
                    bool found = false;
                    for (int i = 0; i < stateList.Count; i++)
                    {
                        if (stateList[i].Cooldown == Cooldown.DrumsOfBattle)
                        {                            
                            drumsStates.Add(stateList[i]);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        drumsStates.Add(new CastingState(calculationResult, Cooldown.DrumsOfBattle, false));
                    }
                    if (flameCapAvailable)
                    {
                        found = false;
                        for (int i = 0; i < stateList.Count; i++)
                        {
                            if (stateList[i].Cooldown == (Cooldown.DrumsOfBattle | Cooldown.FlameCap))
                            {
                                drumsStates.Add(stateList[i]);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            drumsStates.Add(new CastingState(calculationResult, Cooldown.DrumsOfBattle | Cooldown.FlameCap, false));
                        }
                    }
                    if (moltenFuryAvailable)
                    {
                        found = false;
                        for (int i = 0; i < stateList.Count; i++)
                        {
                            if (stateList[i].Cooldown == (Cooldown.MoltenFury | Cooldown.DrumsOfBattle))
                            {
                                mfDrumsStates.Add(stateList[i]);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            mfDrumsStates.Add(new CastingState(calculationResult, Cooldown.MoltenFury | Cooldown.DrumsOfBattle, false));
                        }
                        if (flameCapAvailable)
                        {
                            found = false;
                            for (int i = 0; i < stateList.Count; i++)
                            {
                                if (stateList[i].Cooldown == (Cooldown.MoltenFury | Cooldown.DrumsOfBattle | Cooldown.FlameCap))
                                {
                                    mfDrumsStates.Add(stateList[i]);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                mfDrumsStates.Add(new CastingState(calculationResult, Cooldown.MoltenFury | Cooldown.DrumsOfBattle | Cooldown.FlameCap, false));
                            }
                        }
                    }

                    int drumsOfBattleSegments = segmentList.Count; // always segment, we need it to guarantee each block has activation
                    manaRegen = -calculationResult.BaseState.ManaRegen5SR;
                    for (int segment = 0; segment < drumsOfBattleSegments; segment++)
                    {
                        List<CastingState> states = (calculationOptions.FightDuration - calculationOptions.MoltenFuryPercentage * calculationOptions.FightDuration < segmentList[segment].TimeStart) ? mfDrumsStates : drumsStates;
                        foreach (CastingState state in states)
                        {
                            if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.DrumsOfBattle, Segment = segment, State = state });
                            column = lp.AddColumnUnsafe();
                            lp.SetColumnUpperBound(column, calculationResult.BaseGlobalCooldown * ((drumsOfBattleSegments > 1) ? 1 : (1 + (int)((calculationOptions.FightDuration - 30) / 120))));
                            if (segment == 0 && state == states[0]) calculationResult.ColumnDrumsOfBattle = column;
                            lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                            lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                            lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                            lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                            lp.SetElementUnsafe(rowDrumsOfBattleActivation, column, -1 / calculationResult.BaseGlobalCooldown);
                            lp.SetElementUnsafe(rowDrumsOfBattle, column, 1.0);
                            if (state.FlameCap) lp.SetElementUnsafe(rowManaGemFlameCap, column, 1.0 / 40.0);
                            lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                            if (needsDisplayCalculations) tpsList.Add(0.0);
                            if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                            if (restrictManaUse)
                            {
                                for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                {
                                    lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                    lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                                }
                            }
                            if (segmentCooldowns)
                            {
                                foreach (SegmentConstraint constraint in rowSegmentDrumsOfBattle)
                                {
                                    if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                                }
                                foreach (SegmentConstraint constraint in rowSegmentDrumsOfBattleActivation)
                                {
                                    if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                                }
                                if (state.FlameCap)
                                {
                                    foreach (SegmentConstraint constraint in rowSegmentFlameCap)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                                    }
                                    foreach (SegmentConstraint constraint in rowSegmentManaGem)
                                    {
                                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Summon Water Elemental
                if (waterElementalAvailable)
                {
                    int waterElementalSegments = segmentList.Count; // always segment, we need it to guarantee each block has activation
                    manaRegen = (int)(0.16 * SpellTemplate.BaseMana[calculationOptions.PlayerLevel]) / calculationResult.BaseGlobalCooldown - calculationResult.BaseState.ManaRegen5SR;
                    List<CastingState> states = new List<CastingState>();
                    bool found = false;
                    // WE = 0x100
                    for (int i = 0; i < stateList.Count; i++)
                    {
                        if (stateList[i].Cooldown == Cooldown.WaterElemental)
                        {
                            states.Add(stateList[i]);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        states.Add(new CastingState(calculationResult, Cooldown.WaterElemental, false));
                    }
                    for (int segment = 0; segment < waterElementalSegments; segment++)
                    {
                        foreach (CastingState state in states)
                        {
                            Spell waterbolt = calculationResult.WaterboltTemplate.GetSpell(state);
                            if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.SummonWaterElemental, Segment = segment, State = state });
                            column = lp.AddColumnUnsafe();
                            if (waterElementalSegments > 1) lp.SetColumnUpperBound(column, calculationResult.BaseGlobalCooldown);
                            if (segment == 0 && state == states[0]) calculationResult.ColumnSummonWaterElemental = column;
                            lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                            lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                            lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                            lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                            lp.SetElementUnsafe(rowSummonWaterElemental, column, -1 / calculationResult.BaseGlobalCooldown);
                            lp.SetElementUnsafe(rowSummonWaterElementalCount, column, 1.0);
                            lp.SetElementUnsafe(rowWaterElemental, column, 1.0);
                            lp.SetCostUnsafe(column, minimizeTime ? -1 : waterbolt.DamagePerSecond);
                            if (needsDisplayCalculations) tpsList.Add(0.0);
                            lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                            if (restrictManaUse)
                            {
                                for (int ss = segment; ss < segmentList.Count - 1; ss++)
                                {
                                    lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                    lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                                }
                            }
                            if (segmentCooldowns)
                            {
                                foreach (SegmentConstraint constraint in rowSegmentWaterElemental)
                                {
                                    if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                                }
                                foreach (SegmentConstraint constraint in rowSegmentSummonWaterElemental)
                                {
                                    if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Drinking
                if (drinkingEnabled)
                {
                    if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.Drinking });
                    calculationResult.ColumnDrinking = column = lp.AddColumnUnsafe();
                    lp.SetColumnUpperBound(column, maxDrinkingTime);
                    manaRegen = -calculationResult.BaseState.ManaRegenDrinking;
                    lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                    lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                    if (needsDisplayCalculations) tpsList.Add(0.0);
                    if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + 0, column, 1.0);
                    if (restrictManaUse)
                    {
                        for (int ss = 0; ss < segmentList.Count - 1; ss++)
                        {
                            lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                            lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                        }
                    }
                }
                #endregion
                #region Time Extension
                if (needsTimeExtension)
                {
                    if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.TimeExtension });
                    calculationResult.ColumnTimeExtension = column = lp.AddColumnUnsafe();
                    lp.SetColumnUpperBound(column, calculationOptions.FightDuration);
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetElementUnsafe(rowEvocation, column, calculationResult.EvocationDuration / calculationResult.EvocationCooldown);
                    //lp.SetElementUnsafe(rowPotion, column, 1.0 / 120.0);
                    lp.SetElementUnsafe(rowManaGem, column, 1.0 / 120.0);
                    lp.SetElementUnsafe(rowPowerInfusion, column, calculationResult.PowerInfusionDuration / calculationResult.PowerInfusionCooldown);
                    lp.SetElementUnsafe(rowArcanePower, column, calculationResult.ArcanePowerDuration / calculationResult.ArcanePowerCooldown);
                    lp.SetElementUnsafe(rowIcyVeins, column, 20.0 / calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20.0 / calculationResult.ColdsnapCooldown : 0.0));
                    lp.SetElementUnsafe(rowMoltenFury, column, calculationOptions.MoltenFuryPercentage);
                    lp.SetElementUnsafe(rowManaGemFlameCap, column, 1f / 120f);
                    lp.SetElementUnsafe(rowTrinket1, column, trinket1Duration / trinket1Cooldown);
                    lp.SetElementUnsafe(rowTrinket2, column, trinket2Duration / trinket2Cooldown);
                    lp.SetElementUnsafe(rowManaGemEffect, column, manaGemEffectDuration / 120f);
                    lp.SetElementUnsafe(rowDpsTime, column, -(1 - dpsTime));
                    lp.SetElementUnsafe(rowAoe, column, calculationOptions.AoeDuration);
                    lp.SetElementUnsafe(rowCombustion, column, 1.0 / 180.0);
                    lp.SetElementUnsafe(rowDrumsOfBattle, column, calculationResult.BaseGlobalCooldown / 120.0);
                    if (needsDisplayCalculations) tpsList.Add(0.0);
                }
                #endregion
                #region After Fight Regen
                if (afterFightRegen)
                {
                    if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.AfterFightRegen });
                    calculationResult.ColumnAfterFightRegen = column = lp.AddColumnUnsafe();
                    lp.SetColumnUpperBound(column, calculationOptions.FightDuration);
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.BaseState.ManaRegenDrinking);
                    lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                    if (needsDisplayCalculations) tpsList.Add(0.0);
                }
                #endregion
                #region Mana Overflow
                if (restrictManaUse)
                {
                    for (int segment = 0; segment < segmentList.Count; segment++)
                    {
                        if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.ManaOverflow, Segment = segment });
                        column = lp.AddColumnUnsafe();
                        if (segment == 0) calculationResult.ColumnManaOverflow = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, 1.0);
                        lp.SetElementUnsafe(rowManaRegen, column, 1.0);
                        if (needsDisplayCalculations) tpsList.Add(0.0);
                        for (int ss = segment; ss < segmentList.Count - 1; ss++)
                        {
                            lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, 1.0);
                            lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -1.0);
                        }
                    }
                }
                #endregion
                #region Conjure Mana Gem
                if (conjureManaGem)
                {
                    int conjureSegments = (restrictManaUse) ? segmentList.Count : 1;
                    Cycle spell = calculationResult.ConjureManaGemTemplate.GetSpell(calculationResult.BaseState);
                    calculationResult.ConjureManaGem = spell;
                    calculationResult.MaxConjureManaGem = (int)((calculationOptions.FightDuration - 300.0f) / 360.0f) + 1;
                    manaRegen = spell.ManaPerSecond;
                    for (int segment = 0; segment < conjureSegments; segment++)
                    {
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnUpperBound(column, spell.CastTime * ((conjureSegments > 1) ? 1 : calculationResult.MaxConjureManaGem));
                        if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.ConjureManaGem, Cycle = spell, Segment = segment, State = calculationResult.BaseState });
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                        lp.SetElementUnsafe(rowConjureManaGem, column, 1.0);
                        lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                        lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = spell.ThreatPerSecond);
                        lp.SetElementUnsafe(rowTargetDamage, column, -spell.DamagePerSecond);
                        lp.SetCostUnsafe(column, minimizeTime ? -1 : spell.DamagePerSecond);
                        if (needsDisplayCalculations) tpsList.Add(tps);
                        lp.SetElementUnsafe(rowManaGem, column, -3.0 / spell.CastTime); // one cast time gives 3 new gem uses
                        if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                    }
                }
                #endregion
                #region Fire/Frost Ward
                if (wardsAvailable)
                {

                    int wardSegments = (restrictManaUse) ? segmentList.Count : 1;
                    Cycle fireWard = calculationResult.FireWardTemplate.GetSpell(calculationResult.BaseState);
                    Cycle frostWard = calculationResult.FrostWardTemplate.GetSpell(calculationResult.BaseState);
                    Cycle spell = fireWard.CostPerSecond < frostWard.CostPerSecond ? fireWard : frostWard;
                    calculationResult.Ward = spell;
                    calculationResult.MaxWards = (int)((calculationOptions.FightDuration - 15.0f) / 30.0f) + 1;
                    manaRegen = spell.ManaPerSecond;
                    for (int segment = 0; segment < wardSegments; segment++)
                    {
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnUpperBound(column, spell.CastTime * ((wardSegments > 1) ? 1 : calculationResult.MaxWards));
                        if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { Type = VariableType.Ward, Cycle = spell, Segment = segment, State = calculationResult.BaseState });
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                        lp.SetElementUnsafe(rowWard, column, 1.0);
                        lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                        lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = spell.ThreatPerSecond);
                        lp.SetElementUnsafe(rowTargetDamage, column, -spell.DamagePerSecond);
                        lp.SetCostUnsafe(column, minimizeTime ? -1 : spell.DamagePerSecond);
                        if (needsDisplayCalculations) tpsList.Add(tps);
                        if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segmentList.Count - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                    }
                }
                #endregion
                #region Spells
                if (useIncrementalOptimizations)
                {
                    int lastSegment = -1;
                    for (int index = 0; index < calculationOptions.IncrementalSetStateIndexes.Length; index++)
                    {
                        for (int buffset = 0; buffset < stateList.Count; buffset++)
                        {
                            if ((calculationOptions.IncrementalSetStateIndexes[index] & stateList[buffset].Cooldown) == calculationOptions.IncrementalSetStateIndexes[index])
                            {
                                if (calculationOptions.CooldownRestrictionsValid(segmentList[calculationOptions.IncrementalSetSegments[index]], stateList[buffset]))
                                {
                                    column = lp.AddColumnUnsafe();
                                    Cycle c = stateList[buffset].GetCycle(calculationOptions.IncrementalSetSpells[index]);
                                    int seg = calculationOptions.IncrementalSetSegments[index];
                                    if (seg != lastSegment)
                                    {
                                        for (; lastSegment < seg; )
                                        {
                                            segmentColumn[++lastSegment] = column;
                                        }
                                    }
                                    if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { State = stateList[buffset], Cycle = c, Segment = seg, Type = VariableType.Spell });
                                    SetSpellColumn(minimizeTime, tpsList, seg, stateList[buffset], column, c);
                                }
                            }
                        }
                    }
                    for (; lastSegment < segmentList.Count; )
                    {
                        segmentColumn[++lastSegment] = column + 1;
                    }
                }
                else
                {
                    int firstMoltenFurySegment = segmentList.FindIndex(s => s.TimeEnd > calculationOptions.FightDuration * (1 - calculationOptions.MoltenFuryPercentage) + 0.00001);

                    List<Cycle> placed = new List<Cycle>();
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        segmentColumn[seg] = column + 1;
                        for (int buffset = 0; buffset < stateList.Count; buffset++)
                        {
                            if (calculationOptions.CooldownRestrictionsValid(segmentList[seg], stateList[buffset]))
                            {
                                placed.Clear();
                                for (int spell = 0; spell < spellList.Count; spell++)
                                {
                                    if (segmentCooldowns && moltenFuryAvailable && stateList[buffset].MoltenFury && seg < firstMoltenFurySegment) continue;
                                    if (segmentCooldowns && moltenFuryAvailable && !stateList[buffset].MoltenFury && seg >= firstMoltenFurySegment) continue;
                                    if (!segmentNonCooldowns && stateList[buffset] == calculationResult.BaseState && seg != 0) continue;
                                    if (segmentCooldowns && calculationOptions.HeroismControl == 3 && stateList[buffset].Heroism && seg < firstMoltenFurySegment) continue;
                                    Cycle c = stateList[buffset].GetCycle(spellList[spell]);
                                    bool skip = false;
                                    foreach (Cycle s2 in placed)
                                    {
                                        // TODO verify it this is ok, it assumes that spells placed under same casting state are independent except for aoe spells
                                        // assuming there are no constraints that depend on properties of particular spell cycle instead of properties of casting state
                                        if (!c.AreaEffect && s2.DamagePerSecond >= c.DamagePerSecond - 0.00001 && s2.ManaPerSecond <= c.ManaPerSecond + 0.00001)
                                        {
                                            skip = true;
                                            break;
                                        }
                                    }
                                    if (!skip && (c.AffectedByFlameCap || !stateList[buffset].FlameCap))
                                    {
                                        placed.Add(c);
                                        column = lp.AddColumnUnsafe();
                                        if (needsSolutionVariables) solutionVariable.Add(new SolutionVariable() { State = stateList[buffset], Cycle = c, Segment = seg, Type = VariableType.Spell });
                                        SetSpellColumn(minimizeTime, tpsList, seg, stateList[buffset], column, c);
                                    }
                                }
                            }
                        }
                    }
                    segmentColumn[segmentList.Count] = column + 1;
                }
                #endregion

                lp.EndColumnConstruction();
                SetProblemRHS();
                #endregion

                lp.EndUnsafe();
            }
        }

        private void SetProblemRHS()
        {
            int coldsnapCount = coldsnapAvailable ? (1 + (int)((calculationOptions.FightDuration - calculationResult.WaterElementalDuration) / calculationResult.ColdsnapCooldown)) : 0;

            // water elemental
            double weDuration = 0.0;
            if (waterElementalAvailable)
            {
                weDuration = MaximizeEffectDuration(calculationOptions.FightDuration, calculationResult.WaterElementalDuration, calculationResult.WaterElementalCooldown);
                if (coldsnapAvailable) weDuration = MaximizeColdsnapDuration(calculationOptions.FightDuration, calculationResult.ColdsnapCooldown, calculationResult.WaterElementalDuration, calculationResult.WaterElementalCooldown, out coldsnapCount);
            }

            double combustionCount = combustionAvailable ? (1 + (int)((calculationOptions.FightDuration - 15f) / 195f)) : 0;

            double ivlength = 0.0;
            if (!waterElementalAvailable && coldsnapAvailable)
            {
                ivlength = Math.Floor(MaximizeColdsnapDuration(calculationOptions.FightDuration, calculationResult.ColdsnapCooldown, 20.0, calculationResult.IcyVeinsCooldown, out coldsnapCount));
            }
            else if (waterElementalAvailable && coldsnapAvailable)
            {
                // TODO recheck this logic
                double wecount = (weDuration / calculationResult.WaterElementalDuration);
                if (wecount >= Math.Floor(wecount) + 20.0 / calculationResult.WaterElementalDuration)
                    ivlength = Math.Ceiling(wecount) * 20.0;
                else
                    ivlength = Math.Floor(wecount) * 20.0;
            }
            else
            {
                double effectiveDuration = calculationOptions.FightDuration;
                if (waterElementalAvailable) effectiveDuration -= calculationResult.BaseGlobalCooldown; // EXPERIMENTAL
                ivlength = MaximizeEffectDuration(effectiveDuration, 20.0, calculationResult.IcyVeinsCooldown);
            }

            double aplength = MaximizeEffectDuration(calculationOptions.FightDuration, calculationResult.ArcanePowerDuration, calculationResult.ArcanePowerCooldown);
            double pilength = MaximizeEffectDuration(calculationOptions.FightDuration, calculationResult.PowerInfusionDuration, calculationResult.PowerInfusionCooldown);
            double mflength = calculationOptions.MoltenFuryPercentage * calculationOptions.FightDuration;

            lp.SetRHSUnsafe(rowManaRegen, calculationResult.StartingMana);
            lp.SetRHSUnsafe(rowFightDuration, calculationOptions.FightDuration);
            lp.SetRHSUnsafe(rowTimeExtension, -calculationOptions.FightDuration);
            lp.SetRHSUnsafe(rowEvocation, calculationResult.EvocationDuration * calculationResult.MaxEvocation);
            if (icyVeinsAvailable) lp.SetRHSUnsafe(rowEvocationIV, calculationResult.EvocationDurationIV * calculationResult.MaxEvocation);
            if (heroismAvailable) lp.SetRHSUnsafe(rowEvocationHero, calculationResult.EvocationDurationHero);
            if (icyVeinsAvailable && heroismAvailable) lp.SetRHSUnsafe(rowEvocationIVHero, calculationResult.EvocationDurationIVHero);
            lp.SetRHSUnsafe(rowPotion, calculationResult.MaxManaPotion);
            lp.SetRHSUnsafe(rowManaPotion, calculationResult.MaxManaPotion);
            lp.SetRHSUnsafe(rowManaGem, Math.Min(3, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaGem));
            lp.SetRHSUnsafe(rowManaGemMax, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaGem);
            if (conjureManaGem) lp.SetRHSUnsafe(rowConjureManaGem, calculationResult.MaxConjureManaGem * calculationResult.ConjureManaGem.CastTime);
            if (wardsAvailable) lp.SetRHSUnsafe(rowWard, calculationResult.MaxWards * calculationResult.Ward.CastTime);
            if (heroismAvailable)
            {
                lp.SetRHSUnsafe(rowHeroism, 40.0);
                double minDuration = Math.Min(0.99 * calculationOptions.FightDuration * calculationOptions.DpsTime, 40.0);
                if (moltenFuryAvailable && calculationOptions.HeroismControl == 3 && mflength < minDuration)
                {
                    minDuration = 0.99 * mflength;
                }
                lp.SetLHSUnsafe(rowHeroism, minDuration); // if heroism is marked as available then this implies that it has to be used, not only that it can be used
            }
            if (powerInfusionAvailable) lp.SetRHSUnsafe(rowPowerInfusion, calculationOptions.AverageCooldowns ? calculationResult.PowerInfusionDuration / calculationResult.PowerInfusionCooldown * calculationOptions.FightDuration : pilength);
            if (arcanePowerAvailable) lp.SetRHSUnsafe(rowArcanePower, calculationOptions.AverageCooldowns ? calculationResult.ArcanePowerDuration / calculationResult.ArcanePowerCooldown * calculationOptions.FightDuration : aplength);
            if (heroismAvailable && arcanePowerAvailable) lp.SetRHSUnsafe(rowHeroismArcanePower, calculationResult.ArcanePowerDuration);
            if (heroismAvailable && manaGemEffectAvailable) lp.SetRHSUnsafe(rowHeroismManaGemEffect, calculationResult.ManaGemEffectDuration);
            if (icyVeinsAvailable) lp.SetRHSUnsafe(rowIcyVeins, calculationOptions.AverageCooldowns ? (20.0 / calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20.0 / calculationResult.ColdsnapCooldown : 0.0)) * calculationOptions.FightDuration : ivlength);
            if (moltenFuryAvailable) lp.SetRHSUnsafe(rowMoltenFury, mflength);
            //if (moltenFuryAvailable) lp.SetRHSUnsafe(rowMoltenFuryDestructionPotion, 15);
            if (moltenFuryAvailable && icyVeinsAvailable) lp.SetRHSUnsafe(rowMoltenFuryIcyVeins, coldsnapAvailable ? 40 : 20);
            if (moltenFuryAvailable && manaGemEffectAvailable) lp.SetRHSUnsafe(rowMoltenFuryManaGemEffect, manaGemEffectDuration);
            //if (heroismAvailable) lp.SetRHSUnsafe(rowHeroismDestructionPotion, 15);
            //if (icyVeinsAvailable) lp.SetRHSUnsafe(rowIcyVeinsDestructionPotion, dpivlength);
            if (segmentCooldowns)
            {
                lp.SetRHSUnsafe(rowManaGemFlameCap, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : Math.Max(((int)((calculationOptions.FightDuration - 60.0) / 60.0)) * 0.5 + 1.5, calculationResult.MaxManaGem));
            }
            else if (flameCapAvailable && !(!useGlobalOptimizations && talents.SpellPower > 0))
            {
                lp.SetRHSUnsafe(rowManaGemFlameCap, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : ((int)(calculationOptions.FightDuration / 180.0 + 2.0 / 3.0)) * 3.0 / 2.0);
            }
            else
            {
                lp.SetRHSUnsafe(rowManaGemFlameCap, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaGem);
            }
            if (moltenFuryAvailable) lp.SetRHSUnsafe(rowMoltenFuryFlameCap, 60);
            //lp.SetRHSUnsafe(rowFlameCapDestructionPotion, dpflamelength);
            if (trinket1Available) lp.SetRHSUnsafe(rowTrinket1, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * trinket1Duration / trinket1Cooldown : MaximizeEffectDuration(calculationOptions.FightDuration, trinket1Duration, trinket1Cooldown));
            if (trinket2Available) lp.SetRHSUnsafe(rowTrinket2, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * trinket2Duration / trinket2Cooldown : MaximizeEffectDuration(calculationOptions.FightDuration, trinket2Duration, trinket2Cooldown));
            if (manaGemEffectAvailable) lp.SetRHSUnsafe(rowManaGemEffect, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * manaGemEffectDuration / 120f : MaximizeEffectDuration(calculationOptions.FightDuration, manaGemEffectDuration, 120.0));
            if (moltenFuryAvailable && trinket1Available) lp.SetRHSUnsafe(rowMoltenFuryTrinket1, trinket1Duration);
            if (moltenFuryAvailable && trinket2Available) lp.SetRHSUnsafe(rowMoltenFuryTrinket2, trinket2Duration);
            if (heroismAvailable && trinket1Available) lp.SetRHSUnsafe(rowHeroismTrinket1, trinket1Duration);
            if (heroismAvailable && trinket2Available) lp.SetRHSUnsafe(rowHeroismTrinket2, trinket2Duration);
            lp.SetRHSUnsafe(rowDpsTime, -(1 - calculationOptions.DpsTime) * calculationOptions.FightDuration);
            lp.SetRHSUnsafe(rowAoe, calculationOptions.AoeDuration * calculationOptions.FightDuration);
            lp.SetRHSUnsafe(rowCombustion, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 180.0 : combustionCount);
            lp.SetRHSUnsafe(rowMoltenFuryCombustion, 1);
            lp.SetRHSUnsafe(rowHeroismCombustion, 1);
            lp.SetRHSUnsafe(rowHeroismIcyVeins, coldsnapAvailable ? 40 : 20);
            lp.SetRHSUnsafe(rowMoltenFuryDrumsOfBattle, 30 - calculationResult.BaseGlobalCooldown);
            lp.SetRHSUnsafe(rowHeroismDrumsOfBattle, 30 - calculationResult.BaseGlobalCooldown);
            //lp.SetRHSUnsafe(rowIcyVeinsDrumsOfBattle, drumsivlength);
            //lp.SetRHSUnsafe(rowArcanePowerDrumsOfBattle, drumsaplength);
            lp.SetRHSUnsafe(rowThreat, calculationOptions.TpsLimit * calculationOptions.FightDuration);
            double manaConsum;
            /*if (integralMana)
            {
                manaConsum = Math.Ceiling((calculationOptions.FightDuration - 7800 / manaBurn) / 60f + 2);
            }
            else
            {
                manaConsum = ((calculationOptions.FightDuration - 7800 / manaBurn) / 60f + 2);
            }
            if (manaGemEffectAvailable && manaConsum < calculationResult.MaxManaGem)*/ manaConsum = calculationResult.MaxManaGem;
            //lp.SetRHSUnsafe(rowManaPotionManaGem, manaConsum * 40.0);
            lp.SetRHSUnsafe(rowDrumsOfBattle, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * calculationResult.BaseGlobalCooldown / 120.0 : calculationResult.BaseGlobalCooldown * (1 + (int)((calculationOptions.FightDuration - 30) / 120)));
            if (waterElementalAvailable)
            {
                double duration = calculationOptions.AverageCooldowns ? (calculationResult.WaterElementalDuration / calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration / calculationResult.ColdsnapCooldown : 0.0)) * calculationOptions.FightDuration : weDuration;
                lp.SetRHSUnsafe(rowWaterElemental, duration);
                lp.SetRHSUnsafe(rowSummonWaterElementalCount, calculationResult.BaseGlobalCooldown * Math.Ceiling(duration / calculationResult.WaterElementalDuration));
            }
            lp.SetRHSUnsafe(rowTargetDamage, -calculationOptions.TargetDamage);

            if (segmentCooldowns)
            {
                // heroism
                // ap
                if (arcanePowerAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentArcanePower)
                    {
                        lp.SetRHSUnsafe(constraint.Row, calculationResult.ArcanePowerDuration);
                    }
                }
                // pi
                if (powerInfusionAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentPowerInfusion)
                    {
                        lp.SetRHSUnsafe(constraint.Row, calculationResult.PowerInfusionDuration);
                    }
                }
                // iv
                if (icyVeinsAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentIcyVeins)
                    {
                        lp.SetRHSUnsafe(constraint.Row, 20 + (coldsnapAvailable ? 20 : 0));
                    }
                }
                // combustion
                if (combustionAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentCombustion)
                    {
                        lp.SetRHSUnsafe(constraint.Row, 1.0);
                    }
                }
                // drums
                if (drumsOfBattleAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentDrumsOfBattle)
                    {
                        lp.SetRHSUnsafe(constraint.Row, 30.0);
                    }
                    foreach (SegmentConstraint constraint in rowSegmentDrumsOfBattleActivation)
                    {
                        lp.SetRHSUnsafe(constraint.Row, calculationResult.BaseGlobalCooldown);
                    }
                }
                // water elemental
                if (waterElementalAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentWaterElemental)
                    {
                        lp.SetRHSUnsafe(constraint.Row, calculationResult.WaterElementalDuration + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0));
                    }
                    foreach (SegmentConstraint constraint in rowSegmentSummonWaterElemental)
                    {
                        lp.SetRHSUnsafe(constraint.Row, calculationResult.BaseGlobalCooldown + (coldsnapAvailable ? calculationResult.BaseGlobalCooldown : 0.0));
                    }
                }
                // flamecap
                if (flameCapAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentFlameCap)
                    {
                        lp.SetRHSUnsafe(constraint.Row, 60.0);
                    }
                    foreach (SegmentConstraint constraint in rowSegmentManaGem)
                    {
                        lp.SetRHSUnsafe(constraint.Row, 60.0);
                    }
                }
                // effect potion
                /*if (effectPotionAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentPotion + seg, 15.0);
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }*/
                // t1
                if (trinket1Available)
                {
                    foreach (SegmentConstraint constraint in rowSegmentTrinket1)
                    {
                        lp.SetRHSUnsafe(constraint.Row, trinket1Duration);
                    }
                }
                // t2
                if (trinket2Available)
                {
                    foreach (SegmentConstraint constraint in rowSegmentTrinket2)
                    {
                        lp.SetRHSUnsafe(constraint.Row, trinket2Duration);
                    }
                }
                if (manaGemEffectAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentManaGemEffect)
                    {
                        lp.SetRHSUnsafe(constraint.Row, manaGemEffectDuration);
                    }
                }
                if (evocationAvailable)
                {
                    foreach (SegmentConstraint constraint in rowSegmentEvocation)
                    {
                        lp.SetRHSUnsafe(constraint.Row, calculationResult.EvocationDuration);
                    }
                }
                // timing
                for (int seg = 0; seg < segmentList.Count; seg++)
                {
                    lp.SetRHSUnsafe(rowSegment + seg, segmentList[seg].Duration);
                }
            }
            if (restrictManaUse)
            {
                for (int ss = 0; ss < segmentList.Count - 1; ss++)
                {
                    lp.SetRHSUnsafe(rowSegmentManaUnderflow + ss, calculationResult.StartingMana);
                    lp.SetRHSUnsafe(rowSegmentManaOverflow + ss, calculationResult.BaseStats.Mana - calculationResult.StartingMana);
                }
            }
            if (restrictThreat)
            {
                for (int ss = 0; ss < segmentList.Count - 1; ss++)
                {
                    lp.SetRHSUnsafe(rowSegmentThreat + ss, calculationOptions.TpsLimit * segmentList[ss].TimeEnd);
                }
            }
        }

        private int ConstructRows(bool minimizeTime, bool drinkingEnabled, bool needsTimeExtension, bool afterFightRegen)
        {
            int rowCount = 0;

            if (!calculationOptions.UnlimitedMana) rowManaRegen = rowCount++;
            rowFightDuration = rowCount++;
            if (evocationAvailable && (needsTimeExtension || restrictManaUse || integralMana || calculationOptions.EnableHastedEvocation)) rowEvocation = rowCount++;
            if (calculationOptions.EnableHastedEvocation)
            {
                if (evocationAvailable && icyVeinsAvailable)
                {
                    if (needsTimeExtension || restrictManaUse || integralMana) rowEvocationIV = rowCount++;
                    //rowEvocationIVActivation = rowCount++;
                }
                if (evocationAvailable && heroismAvailable)
                {
                    if (needsTimeExtension || restrictManaUse || integralMana) rowEvocationHero = rowCount++;
                    //rowEvocationHeroActivation = rowCount++;
                }
                if (evocationAvailable && icyVeinsAvailable && heroismAvailable)
                {
                    if (needsTimeExtension || restrictManaUse || integralMana) rowEvocationIVHero = rowCount++;
                    //rowEvocationIVHeroActivation = rowCount++;
                }
            }
            if (manaPotionAvailable || effectPotionAvailable) rowPotion = rowCount++;
            if (manaPotionAvailable && integralMana) rowManaPotion = rowCount++;
            if (calculationOptions.ManaGemEnabled)
            {
                rowManaGem = rowCount++;
                rowManaGemMax = rowCount++;
            }
            if (conjureManaGem)
            {
                rowConjureManaGem = rowCount++;
            }
            if (wardsAvailable)
            {
                rowWard = rowCount++;
            }
            if (heroismAvailable) rowHeroism = rowCount++;
            if (arcanePowerAvailable) rowArcanePower = rowCount++;
            if (powerInfusionAvailable) rowPowerInfusion = rowCount++;
            if (heroismAvailable && arcanePowerAvailable) rowHeroismArcanePower = rowCount++;
            if (heroismAvailable && manaGemEffectAvailable) rowHeroismManaGemEffect = rowCount++;
            if (icyVeinsAvailable) rowIcyVeins = rowCount++;
            if (moltenFuryAvailable) rowMoltenFury = rowCount++;
            //if (moltenFuryAvailable && potionOfWildMagicAvailable) rowMoltenFuryDestructionPotion = rowCount++;
            if (moltenFuryAvailable && icyVeinsAvailable) rowMoltenFuryIcyVeins = rowCount++;
            if (moltenFuryAvailable && manaGemEffectAvailable) rowMoltenFuryManaGemEffect = rowCount++;
            //if (heroismAvailable && effectPotionAvailable) rowHeroismDestructionPotion = rowCount++;
            //if (icyVeinsAvailable && effectPotionAvailable) rowIcyVeinsDestructionPotion = rowCount++;
            if (flameCapAvailable) rowManaGemFlameCap = rowCount++;
            if (moltenFuryAvailable && flameCapAvailable) rowMoltenFuryFlameCap = rowCount++;
            //if (flameCapAvailable && destructionPotionAvailable) rowFlameCapDestructionPotion = rowCount++;
            if (trinket1Available) rowTrinket1 = rowCount++;
            if (trinket2Available) rowTrinket2 = rowCount++;
            if (moltenFuryAvailable && trinket1Available) rowMoltenFuryTrinket1 = rowCount++;
            if (moltenFuryAvailable && trinket2Available) rowMoltenFuryTrinket2 = rowCount++;
            if (heroismAvailable && trinket1Available) rowHeroismTrinket1 = rowCount++;
            if (heroismAvailable && trinket2Available) rowHeroismTrinket2 = rowCount++;
            if (manaGemEffectAvailable) rowManaGemEffectActivation = rowCount++;
            if (calculationOptions.AoeDuration > 0)
            {
                rowAoe = rowCount++;
                rowFlamestrike = rowCount++;
                rowConeOfCold = rowCount++;
                if (talents.BlastWave == 1) rowBlastWave = rowCount++;
                if (talents.DragonsBreath == 1) rowDragonsBreath = rowCount++;
            }
            if (combustionAvailable) rowCombustion = rowCount++;
            if (combustionAvailable && moltenFuryAvailable) rowMoltenFuryCombustion = rowCount++;
            if (combustionAvailable && heroismAvailable) rowHeroismCombustion = rowCount++;
            if (icyVeinsAvailable && heroismAvailable) rowHeroismIcyVeins = rowCount++;
            if (drumsOfBattleAvailable) rowDrumsOfBattleActivation = rowCount++;
            if (drumsOfBattleAvailable && moltenFuryAvailable) rowMoltenFuryDrumsOfBattle = rowCount++;
            if (drumsOfBattleAvailable && heroismAvailable) rowHeroismDrumsOfBattle = rowCount++;
            //if (drumsOfBattleAvailable && icyVeinsAvailable) rowIcyVeinsDrumsOfBattle = rowCount++;
            //if (drumsOfBattleAvailable && arcanePowerAvailable) rowArcanePowerDrumsOfBattle = rowCount++;
            if (calculationOptions.TpsLimit > 0f) rowThreat = rowCount++;
            if (drumsOfBattleAvailable) rowDrumsOfBattle = rowCount++;
            if (needsTimeExtension) rowTimeExtension = rowCount++;
            if (afterFightRegen) rowAfterFightRegenMana = rowCount++;
            //if (afterFightRegen) rowAfterFightRegenHealth = rowCount++;
            if (minimizeTime) rowTargetDamage = rowCount++;
            if (waterElementalAvailable)
            {
                rowWaterElemental = rowCount++;
                rowSummonWaterElemental = rowCount++;
                rowSummonWaterElementalCount = rowCount++;
            }
            rowDpsTime = rowCount++;
            //rowManaPotionManaGem = rowCount++;
            if (segmentCooldowns)
            {
                // mf, heroism, ap, iv, combustion, drums, flamecap, destruction, t1, t2
                // mf
                // heroism
                // ap
                if (arcanePowerAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentArcanePower;
                    double cool = calculationResult.ArcanePowerCooldown;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // pi
                if (powerInfusionAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentPowerInfusion;
                    double cool = calculationResult.PowerInfusionCooldown;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // iv
                if (icyVeinsAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentIcyVeins;
                    double cool = calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20 : 0);
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // combustion
                if (combustionAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentCombustion;
                    double cool = 180 + 15;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // drums
                if (drumsOfBattleAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentDrumsOfBattle;
                    double cool = 120;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                    list = rowSegmentDrumsOfBattleActivation;
                    cool = 120;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                if (waterElementalAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentWaterElemental;
                    double cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                    list = rowSegmentSummonWaterElemental;
                    cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // flamecap & mana gem
                if (flameCapAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentFlameCap;
                    double cool = 180;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                    list = rowSegmentManaGem;
                    cool = 120;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // effect potion
                /*if (effectPotionAvailable)
                {
                    rowSegmentPotion = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }*/
                // t1
                if (trinket1Available)
                {
                    List<SegmentConstraint> list = rowSegmentTrinket1;
                    double cool = trinket1Cooldown;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // t2
                if (trinket2Available)
                {
                    List<SegmentConstraint> list = rowSegmentTrinket2;
                    double cool = trinket2Cooldown;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // mana gem effect
                if (manaGemEffectAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentManaGemEffect;
                    double cool = 120.0;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                if (evocationAvailable)
                {
                    List<SegmentConstraint> list = rowSegmentEvocation;
                    double cool = calculationResult.EvocationCooldown;
                    for (int seg = 0; seg < segmentList.Count; seg++)
                    {
                        int maxs = segmentList.FindIndex(s => s.TimeEnd > segmentList[seg].TimeStart + cool + 0.00001) - 1;
                        if (maxs == -2) maxs = segmentList.Count - 1;
                        if (list.Count == 0 || maxs > list[list.Count - 1].MaxSegment)
                        {
                            list.Add(new SegmentConstraint() { Row = rowCount++, MinSegment = seg, MaxSegment = maxs });
                        }
                    }
                }
                // max segment time
                rowSegment = rowCount;
                rowCount += segmentList.Count;
                // mana overflow & underflow (don't need over all segments, that is already verified)
                if (restrictManaUse)
                {
                    rowSegmentManaOverflow = rowCount;
                    rowCount += segmentList.Count - 1;
                    rowSegmentManaUnderflow = rowCount;
                    rowCount += segmentList.Count - 1;
                }
                if (restrictThreat)
                {
                    rowSegmentThreat = rowCount;
                    rowCount += segmentList.Count - 1;
                }
            }
            return rowCount;
        }

        private void SetSpellColumn(bool minimizeTime, List<double> tpsList, int segment, CastingState state, int column, Cycle cycle)
        {
            double bound = calculationOptions.FightDuration;
            double manaRegen = cycle.ManaPerSecond;
            lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
            lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
            lp.SetElementUnsafe(rowFightDuration, column, 1.0);
            lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
            if (state.PotionOfWildMagic || state.PotionOfSpeed)
            {
                lp.SetElementUnsafe(rowPotion, column, 1.0 / 15.0);
            }
            if (state.WaterElemental) lp.SetElementUnsafe(rowWaterElemental, column, 1.0);
            if (state.Heroism) lp.SetElementUnsafe(rowHeroism, column, 1.0);
            if (state.ArcanePower) lp.SetElementUnsafe(rowArcanePower, column, 1.0);
            if (state.PowerInfusion) lp.SetElementUnsafe(rowPowerInfusion, column, 1.0);
            if (state.Heroism && state.ArcanePower) lp.SetElementUnsafe(rowHeroismArcanePower, column, 1.0);
            if (state.Heroism && state.ManaGemEffect) lp.SetElementUnsafe(rowHeroismManaGemEffect, column, 1.0);
            if (state.IcyVeins) lp.SetElementUnsafe(rowIcyVeins, column, 1.0);
            if (state.MoltenFury) lp.SetElementUnsafe(rowMoltenFury, column, 1.0);
            //if (state.MoltenFury && state.PotionOfWildMagic) lp.SetElementUnsafe(rowMoltenFuryDestructionPotion, column, 1.0);
            if (state.MoltenFury && state.IcyVeins) lp.SetElementUnsafe(rowMoltenFuryIcyVeins, column, 1.0);
            if (state.MoltenFury && state.ManaGemEffect) lp.SetElementUnsafe(rowMoltenFuryManaGemEffect, column, 1.0);
            //if (state.PotionOfWildMagic && state.Heroism) lp.SetElementUnsafe(rowHeroismDestructionPotion, column, 1.0);
            //if (state.PotionOfWildMagic && state.IcyVeins) lp.SetElementUnsafe(rowIcyVeinsDestructionPotion, column, 1.0);
            if (state.FlameCap) lp.SetElementUnsafe(rowManaGemFlameCap, column, 1.0 / 40.0);
            if (state.MoltenFury && state.FlameCap) lp.SetElementUnsafe(rowMoltenFuryFlameCap, column, 1.0);
            //if (state.PotionOfWildMagic && state.FlameCap) lp.SetElementUnsafe(rowFlameCapDestructionPotion, column, 1.0);
            if (state.Trinket1) lp.SetElementUnsafe(rowTrinket1, column, 1.0);
            if (state.Trinket2) lp.SetElementUnsafe(rowTrinket2, column, 1.0);
            if (state.ManaGemEffect) lp.SetElementUnsafe(rowManaGemEffect, column, 1.0);
            if (state.MoltenFury && state.Trinket1) lp.SetElementUnsafe(rowMoltenFuryTrinket1, column, 1.0);
            if (state.MoltenFury && state.Trinket2) lp.SetElementUnsafe(rowMoltenFuryTrinket2, column, 1.0);
            if (state.Heroism && state.Trinket1) lp.SetElementUnsafe(rowHeroismTrinket1, column, 1.0);
            if (state.Heroism && state.Trinket2) lp.SetElementUnsafe(rowHeroismTrinket2, column, 1.0);
            lp.SetElementUnsafe(rowManaGemEffectActivation, column, ((state.ManaGemEffect) ? 1 / manaGemEffectDuration : 0));
            if (cycle.AreaEffect) lp.SetElementUnsafe(rowAoe, column, 1.0);
            if (cycle.AreaEffect)
            {
                Spell fs = cycle.AoeSpell;
                if (fs.SpellTemplate is FlamestrikeTemplate)
                {
                    if (!fs.SpammedDot) lp.SetElementUnsafe(rowFlamestrike, column, fs.DotDuration / fs.CastTime);
                }
                else
                {
                    lp.SetElementUnsafe(rowFlamestrike, column, -1.0);
                }
                Spell coc = cycle.AoeSpell;
                if (coc.SpellTemplate is ConeOfColdTemplate)
                {
                    lp.SetElementUnsafe(rowConeOfCold, column, (coc.Cooldown / coc.CastTime - 1.0));
                }
                else
                {
                    lp.SetElementUnsafe(rowConeOfCold, column, -1.0);
                }
                Spell bw = cycle.AoeSpell;
                if (bw.SpellTemplate is BlastWaveTemplate)
                {
                    lp.SetElementUnsafe(rowBlastWave, column, (bw.Cooldown / bw.CastTime - 1.0));
                }
                else
                {
                    lp.SetElementUnsafe(rowBlastWave, column, -1);
                }
                Spell db = cycle.AoeSpell;
                if (db.SpellTemplate is DragonsBreathTemplate)
                {
                    lp.SetElementUnsafe(rowDragonsBreath, column, (db.Cooldown / db.CastTime - 1.0));
                }
                else
                {
                    lp.SetElementUnsafe(rowDragonsBreath, column, -1.0);
                }
            }
            if (state.Combustion) lp.SetElementUnsafe(rowCombustion, column, (1 / (state.CombustionDuration * cycle.CastTime / cycle.CastProcs)));
            if (state.Combustion && state.MoltenFury) lp.SetElementUnsafe(rowMoltenFuryCombustion, column, (1 / (state.CombustionDuration * cycle.CastTime / cycle.CastProcs)));
            if (state.Combustion && state.Heroism) lp.SetElementUnsafe(rowHeroismCombustion, column, (1 / (state.CombustionDuration * cycle.CastTime / cycle.CastProcs)));
            if (state.IcyVeins && state.Heroism) lp.SetElementUnsafe(rowHeroismIcyVeins, column, 1.0);
            //if (state.DrumsOfBattle) lp.SetElementUnsafe(rowDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle) lp.SetElementUnsafe(rowDrumsOfBattleActivation, column, 1 / (30 - calculationResult.BaseGlobalCooldown));
            if (state.DrumsOfBattle && state.MoltenFury) lp.SetElementUnsafe(rowMoltenFuryDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.Heroism) lp.SetElementUnsafe(rowHeroismDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.IcyVeins) lp.SetElementUnsafe(rowIcyVeinsDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.ArcanePower) lp.SetElementUnsafe(rowArcanePowerDrumsOfBattle, column, 1.0);
            if (state.WaterElemental) lp.SetElementUnsafe(rowSummonWaterElemental, column, 1 / (calculationResult.WaterElementalDuration - calculationResult.BaseGlobalCooldown));
            lp.SetElementUnsafe(rowThreat, column, cycle.ThreatPerSecond);
            if (needsDisplayCalculations) tpsList.Add(cycle.ThreatPerSecond);
            //lp[rowManaPotionManaGem, index] = (statsList[buffset].FlameCap ? 1 : 0) + (statsList[buffset].DestructionPotion ? 40.0 / 15.0 : 0);
            lp.SetElementUnsafe(rowTargetDamage, column, -cycle.DamagePerSecond);
            lp.SetCostUnsafe(column, minimizeTime ? -1 : cycle.DamagePerSecond);
            if (segmentCooldowns)
            {
                // mf, heroism, ap, iv, combustion, drums, flamecap, destro, t1, t2
                //lp[rowOffset + 1 * segments + seg, index] = 1;
                if (state.ArcanePower)
                {
                    bound = Math.Min(bound, calculationResult.ArcanePowerDuration);
                    foreach (SegmentConstraint constraint in rowSegmentArcanePower)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.PowerInfusion)
                {
                    bound = Math.Min(bound, calculationResult.PowerInfusionDuration);
                    foreach (SegmentConstraint constraint in rowSegmentPowerInfusion)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.IcyVeins)
                {
                    bound = Math.Min(bound, (coldsnapAvailable) ? 40.0 : 20.0);
                    foreach (SegmentConstraint constraint in rowSegmentIcyVeins)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.WaterElemental)
                {
                    foreach (SegmentConstraint constraint in rowSegmentWaterElemental)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.Combustion)
                {
                    foreach (SegmentConstraint constraint in rowSegmentCombustion)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.DrumsOfBattle)
                {
                    foreach (SegmentConstraint constraint in rowSegmentDrumsOfBattle)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.FlameCap)
                {
                    foreach (SegmentConstraint constraint in rowSegmentFlameCap)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                    foreach (SegmentConstraint constraint in rowSegmentManaGem)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.PotionOfWildMagic || state.PotionOfSpeed)
                {
                    bound = Math.Min(bound, 15.0); 
                    /*for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 120;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentPotion + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }*/
                }
                if (state.Trinket1)
                {
                    bound = Math.Min(bound, trinket1Duration);
                    foreach (SegmentConstraint constraint in rowSegmentTrinket1)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.Trinket2)
                {
                    bound = Math.Min(bound, trinket2Duration);
                    foreach (SegmentConstraint constraint in rowSegmentTrinket2)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (state.ManaGemEffect)
                {
                    bound = Math.Min(bound, manaGemEffectDuration);
                    foreach (SegmentConstraint constraint in rowSegmentManaGemEffect)
                    {
                        if (segment >= constraint.MinSegment && segment <= constraint.MaxSegment) lp.SetElementUnsafe(constraint.Row, column, 1.0);
                    }
                }
                if (segmentNonCooldowns || state != calculationResult.BaseState)
                {
                    bound = Math.Min(bound, segmentList[segment].Duration);
                    lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                }
            }
            if (restrictManaUse)
            {
                for (int ss = segment; ss < segmentList.Count - 1; ss++)
                {
                    lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                    lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                }
            }
            if (restrictThreat)
            {
                for (int ss = segment; ss < segmentList.Count - 1; ss++)
                {
                    lp.SetElementUnsafe(rowSegmentThreat + ss, column, cycle.ThreatPerSecond);
                }
            }
            lp.SetColumnUpperBound(column, bound);
        }

        private List<CycleId> GetSpellList()
        {
            List<CycleId> list = new List<CycleId>();

            if (calculationOptions.CustomSpellMixEnabled || calculationOptions.CustomSpellMixOnly)
            {
                list.Add(CycleId.CustomSpellMix);
            }
            if (!calculationOptions.CustomSpellMixOnly)
            {
                if (calculationOptions.MaintainScorch && calculationOptions.MaintainSnare && talents.ImprovedScorch > 0 && talents.Slow > 0)
                {
                    // no cycles right now that provide scorch and snare
                }
                if (calculationOptions.MaintainScorch && talents.ImprovedScorch > 0)
                {
                    if (useGlobalOptimizations)
                    {
                        if (talents.ArcaneBarrage > 0 && talents.MissileBarrage > 0)
                        {
                            list.Add(CycleId.ABABarSc);
                            list.Add(CycleId.ABABarCSc);
                            list.Add(CycleId.ABAMABarSc);
                            list.Add(CycleId.AB3AMABarSc);
                            list.Add(CycleId.AB3ABarCSc);
                            list.Add(CycleId.AB3MBAMABarSc);
                        }
                        else if (talents.PiercingIce == 3 && talents.IceShards == 3 && calculationOptions.PlayerLevel >= 75)
                        {
                            if (talents.LivingBomb > 0)
                            {
                                list.Add(CycleId.FFBScLBPyro);
                            }
                            list.Add(CycleId.FFBScPyro);
                        }
                        else
                        {
                            if (talents.LivingBomb > 0)
                            {
                                list.Add(CycleId.FBScLBPyro);
                            }
                            if (talents.HotStreak > 0)
                            {
                                list.Add(CycleId.FBScPyro);
                            }
                            else
                            {
                                list.Add(CycleId.FBSc);
                            }
                        }
                    }
                    else
                    {
                        if (talents.LivingBomb > 0)
                        {
                            list.Add(CycleId.FBScLBPyro);
                            list.Add(CycleId.ScLBPyro);
                        }
                        if (talents.HotStreak > 0)
                        {
                            list.Add(CycleId.FBScPyro);
                        }
                        else
                        {
                            list.Add(CycleId.FBSc);
                        }
                        if (calculationOptions.PlayerLevel >= 75)
                        {
                            list.Add(CycleId.FFBScPyro);
                            if (talents.LivingBomb > 0)
                            {
                                list.Add(CycleId.FFBScLBPyro);
                            }
                        }
                        list.Add(CycleId.FBFBlast);
                        if (talents.ArcaneBarrage > 0 && talents.MissileBarrage > 0)
                        {
                            list.Add(CycleId.ABABarSc);
                            list.Add(CycleId.ABABarCSc);
                            list.Add(CycleId.ABAMABarSc);
                            list.Add(CycleId.AB3AMABarSc);
                            list.Add(CycleId.AB3ABarCSc);
                            list.Add(CycleId.AB3MBAMABarSc);
                        }
                    }
                }
                // deprecated, if there is demand for this we have to create completely new cycles
                /*else if (calculationOptions.MaintainSnare && talents.Slow > 0)
                {
                    if (useGlobalOptimizations)
                    {
                        if (talents.ArcaneBarrage > 0)
                        {
                            if (talents.ImprovedFrostbolt > 0)
                            {
                                list.Add(CycleId.FrBABarSlow);
                            }
                            if (talents.ImprovedFireball > 0)
                            {
                                list.Add(CycleId.FBABarSlow);
                            }
                            if (talents.ArcaneEmpowerment > 0)
                            {
                                list.Add(CycleId.ABABarSlow);
                            }
                            if (talents.ImprovedFrostbolt == 0 && talents.ImprovedFireball == 0 && talents.ArcaneEmpowerment == 0)
                            {
                                list.Add(CycleId.FrBABarSlow);
                                list.Add(CycleId.FBABarSlow);
                                list.Add(CycleId.ABABarSlow);
                            }
                        }
                    }
                    else
                    {
                        list.Add(CycleId.FrBABarSlow);
                        list.Add(CycleId.FBABarSlow);
                        list.Add(CycleId.ABABarSlow);
                    }
                }*/
                else
                {
                    if (useGlobalOptimizations)
                    {
                        if (talents.EmpoweredFire > 0)
                        {
                            if (talents.PiercingIce == 3 && talents.IceShards == 3 && calculationOptions.PlayerLevel >= 75)
                            {
                                list.Add(CycleId.FFBPyro);
                                if (talents.LivingBomb > 0) list.Add(CycleId.FFBLBPyro);
                            }
                            else
                            {
                                if (talents.HotStreak > 0 && talents.Pyroblast > 0)
                                {
                                    list.Add(CycleId.FBPyro);
                                }
                                else
                                {
                                    list.Add(CycleId.Fireball);
                                }
                                if (talents.LivingBomb > 0) list.Add(CycleId.FBLBPyro);
                            }
                        }
                        else if (talents.EmpoweredFrostbolt > 0)
                        {
                            if (talents.BrainFreeze > 0)
                            {
                                list.Add(CycleId.FrBFB);
                                list.Add(CycleId.FrBFBIL);
                                list.Add(CycleId.FrBILFB);
                            }
                            list.Add(CycleId.FrBIL);
                            list.Add(CycleId.FrostboltFOF);
                        }
                        else if (talents.ArcaneEmpowerment > 0)
                        {
                            if (calculationOptions.Mode322)
                            {
                                list.Add(CycleId.AB2AM);
                                list.Add(CycleId.AB3AM23MBAM);
                                list.Add(CycleId.AB4AM234MBAM);
                                list.Add(CycleId.ABSpam234MBAM);
                                list.Add(CycleId.ABSpam24MBAM);
                                list.Add(CycleId.ABSpam4MBAM);
                                list.Add(CycleId.ArcaneBlastSpam);
                            }
                            else
                            {
                                list.Add(CycleId.ABAM);
                                list.Add(CycleId.AB2AM);
                                list.Add(CycleId.AB3AM);
                                if (talents.MissileBarrage > 0)
                                {
                                    list.Add(CycleId.AB3AM2MBAM);
                                    list.Add(CycleId.ABSpam03MBAM);
                                    list.Add(CycleId.ABSpam3MBAM);
                                    if (talents.ArcaneBarrage > 0)
                                    {
                                        list.Add(CycleId.ABABar1MBAM);
                                        list.Add(CycleId.ABABar2MBAM);
                                        list.Add(CycleId.AB3ABar3MBAM);
                                    }
                                }
                            }
                        }
                        else
                        {
                            list.Add(CycleId.ArcaneMissiles);
                            list.Add(CycleId.Fireball);
                            list.Add(CycleId.FrostboltFOF);
                            if (calculationOptions.PlayerLevel >= 75) list.Add(CycleId.FrostfireBoltFOF);
                        }
                    }
                    else
                    {
                        list.Add(CycleId.ArcaneMissiles);
                        list.Add(CycleId.Scorch);
                        if (talents.LivingBomb > 0) list.Add(CycleId.ScLBPyro);
                        if (talents.HotStreak > 0 && talents.Pyroblast > 0)
                        {
                            list.Add(CycleId.FBPyro);
                        }
                        else
                        {
                            list.Add(CycleId.Fireball);
                        }
                        if (calculationOptions.PlayerLevel >= 75)
                        {
                            list.Add(CycleId.FrostfireBoltFOF);
                            list.Add(CycleId.FFBPyro);
                            if (talents.LivingBomb > 0) list.Add(CycleId.FFBLBPyro);
                        }
                        list.Add(CycleId.FBFBlast);
                        if (talents.LivingBomb > 0) list.Add(CycleId.FBLBPyro);
                        list.Add(CycleId.FrostboltFOF);
                        if (talents.BrainFreeze > 0) list.Add(CycleId.FrBFB);
                        if (talents.FingersOfFrost > 0)
                        {
                            if (talents.BrainFreeze > 0)
                            {
                                list.Add(CycleId.FrBFBIL);
                                list.Add(CycleId.FrBILFB);
                            }
                            list.Add(CycleId.FrBIL);
                        }
                        if (calculationOptions.Mode322)
                        {
                            list.Add(CycleId.AB2AM);
                            list.Add(CycleId.AB3AM23MBAM);
                            list.Add(CycleId.AB4AM234MBAM);
                            list.Add(CycleId.ABSpam234MBAM);
                            list.Add(CycleId.ABSpam24MBAM);
                            list.Add(CycleId.ABSpam4MBAM);
                            list.Add(CycleId.ArcaneBlastSpam);
                        }
                        else
                        {
                            list.Add(CycleId.ArcaneBlastSpam);
                            list.Add(CycleId.ABAM);
                            list.Add(CycleId.AB2AM);
                            list.Add(CycleId.AB3AM);
                            if (talents.MissileBarrage > 0)
                            {
                                list.Add(CycleId.AB3AM2MBAM);
                                list.Add(CycleId.ABSpam03MBAM);
                                list.Add(CycleId.ABSpam3MBAM);
                            }
                            if (talents.ArcaneBarrage > 0 && talents.MissileBarrage > 0)
                            {
                                list.Add(CycleId.ABABar0C);
                                list.Add(CycleId.ABABar1C);
                                list.Add(CycleId.ABABar0MBAM);
                                list.Add(CycleId.AB2ABar2MBAM);
                                list.Add(CycleId.AB2ABar2C);
                                list.Add(CycleId.AB2ABar3C);
                                list.Add(CycleId.AB3ABar3C);
                                list.Add(CycleId.ABSpam3C);
                                list.Add(CycleId.ABSpam03C);
                                list.Add(CycleId.ABABar3C);
                                list.Add(CycleId.ABABar2C);
                                list.Add(CycleId.ABABar2MBAM);
                                list.Add(CycleId.ABABar1MBAM);
                                list.Add(CycleId.AB3ABar3MBAM);
                                list.Add(CycleId.AB3AMABar);
                                list.Add(CycleId.AB3AMABar2C);
                            }
                        }
                    }
                }
                if (calculationOptions.AoeDuration > 0)
                {
                    list.Add(CycleId.ArcaneExplosion);
                    list.Add(CycleId.FlamestrikeSpammed);
                    list.Add(CycleId.FlamestrikeSingle);
                    list.Add(CycleId.Blizzard);
                    list.Add(CycleId.ConeOfCold);
                    if (talents.BlastWave == 1) list.Add(CycleId.BlastWave);
                    if (talents.DragonsBreath == 1) list.Add(CycleId.DragonsBreath);
                }
            }
            return list;
        }

        private List<CastingState> GetStateList(Stats characterStats)
        {
            List<CastingState> list = new List<CastingState>();

            bool canDoubleTrinket = false;
            if (waterElementalAvailable) availableCooldownMask |= Cooldown.WaterElemental;
            if (moltenFuryAvailable) availableCooldownMask |= Cooldown.MoltenFury;
            if (heroismAvailable) availableCooldownMask |= Cooldown.Heroism;
            if (arcanePowerAvailable) availableCooldownMask |= Cooldown.ArcanePower;
            if (icyVeinsAvailable) availableCooldownMask |= Cooldown.IcyVeins;
            if (combustionAvailable) availableCooldownMask |= Cooldown.Combustion;
            if (drumsOfBattleAvailable) availableCooldownMask |= Cooldown.DrumsOfBattle;
            if (flameCapAvailable) availableCooldownMask |= Cooldown.FlameCap;
            if (potionOfWildMagicAvailable) availableCooldownMask |= Cooldown.PotionOfWildMagic;
            if (potionOfSpeedAvailable) availableCooldownMask |= Cooldown.PotionOfSpeed;
            if (trinket1Available) availableCooldownMask |= Cooldown.Trinket1;
            if (trinket2Available) availableCooldownMask |= Cooldown.Trinket2;
            if (manaGemEffectAvailable) availableCooldownMask |= Cooldown.ManaGemEffect;
            if (powerInfusionAvailable) availableCooldownMask |= Cooldown.PowerInfusion;
            if (useIncrementalOptimizations)
            {
                for (int incrementalSortedIndex = 0; incrementalSortedIndex < calculationOptions.IncrementalSetSortedStates.Length; incrementalSortedIndex++)
                {
                    // incremental index is filtered by non-item based cooldowns
                    Cooldown incrementalSetIndex = calculationOptions.IncrementalSetSortedStates[incrementalSortedIndex];                    
                    bool pos = (incrementalSetIndex & Cooldown.PotionOfSpeed) != 0;
                    bool we = (incrementalSetIndex & Cooldown.WaterElemental) != 0;
                    bool mf = (incrementalSetIndex & Cooldown.MoltenFury) != 0;
                    bool heroism = (incrementalSetIndex & Cooldown.Heroism) != 0;
                    bool ap = (incrementalSetIndex & Cooldown.ArcanePower) != 0;
                    bool iv = (incrementalSetIndex & Cooldown.IcyVeins) != 0;
                    bool combustion = (incrementalSetIndex & Cooldown.Combustion) != 0;
                    bool drums = (incrementalSetIndex & Cooldown.DrumsOfBattle) != 0;
                    bool flameCap = (incrementalSetIndex & Cooldown.FlameCap) != 0;
                    bool powm = (incrementalSetIndex & Cooldown.PotionOfWildMagic) != 0;
                    bool pi = (incrementalSetIndex & Cooldown.PowerInfusion) != 0;
                    for (Cooldown index = 0; index <= Cooldown.ItemBasedMask; index++)
                    {
                        bool trinket1 = (index & Cooldown.Trinket1) != 0;
                        bool trinket2 = (index & Cooldown.Trinket2) != 0;
                        bool mg = (index & Cooldown.ManaGemEffect) != 0;
                        if (((incrementalSetIndex | index) & availableCooldownMask) == (incrementalSetIndex | index)) // make sure all are available
                        {
                            if (!trinket1 || !trinket2 || canDoubleTrinket) // only leave through trinkets that can stack
                            {
                                if (!ap || !pi) // can't have ap and pi at the same time
                                if (!mg || !flameCap) // do not allow mana gem together with flame cap
                                {
                                    if (!pos || !powm) // do not allow different potions at the same time
                                    {
                                        if ((calculationOptions.HeroismControl != 1 || !heroism || !mf) && (calculationOptions.HeroismControl != 2 || !heroism || (incrementalSetIndex == Cooldown.Heroism && index == 0)) && (calculationOptions.HeroismControl != 3 || !moltenFuryAvailable || !heroism || mf))
                                        {
                                            list.Add(new CastingState(calculationResult, incrementalSetIndex | index, false));
                                            if (incrementalSetIndex == 0 && index == 0)
                                            {
                                                calculationResult.BaseState = list[list.Count - 1];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (calculationResult.BaseState == null) calculationResult.BaseState = new CastingState(calculationResult, Cooldown.None, false);
            }
            else
            {
                for (Cooldown incrementalSetIndex = 0; incrementalSetIndex <= Cooldown.Mask; incrementalSetIndex++)
                {
                    bool pos = (incrementalSetIndex & Cooldown.PotionOfSpeed) != 0;
                    bool we = (incrementalSetIndex & Cooldown.WaterElemental) != 0;
                    bool mf = (incrementalSetIndex & Cooldown.MoltenFury) != 0;
                    bool heroism = (incrementalSetIndex & Cooldown.Heroism) != 0;
                    bool ap = (incrementalSetIndex & Cooldown.ArcanePower) != 0;
                    bool iv = (incrementalSetIndex & Cooldown.IcyVeins) != 0;
                    bool combustion = (incrementalSetIndex & Cooldown.Combustion) != 0;
                    bool drums = (incrementalSetIndex & Cooldown.DrumsOfBattle) != 0;
                    bool flameCap = (incrementalSetIndex & Cooldown.FlameCap) != 0;
                    bool powm = (incrementalSetIndex & Cooldown.PotionOfWildMagic) != 0;
                    bool trinket1 = (incrementalSetIndex & Cooldown.Trinket1) != 0;
                    bool trinket2 = (incrementalSetIndex & Cooldown.Trinket2) != 0;
                    bool mg = (incrementalSetIndex & Cooldown.ManaGemEffect) != 0;
                    bool pi = (incrementalSetIndex & Cooldown.PowerInfusion) != 0;
                    if (((incrementalSetIndex) & availableCooldownMask) == (incrementalSetIndex)) // make sure all are available
                    {
                        if (!trinket1 || !trinket2 || canDoubleTrinket) // only leave through trinkets that can stack
                        {
                            if (!ap || !pi) // can't have ap and pi at the same time
                            if (!mg || !flameCap) // do not allow mana gem together with flame cap
                            {
                                if (!pos || !powm) // do not allow different potions at the same time
                                {
                                    if ((calculationOptions.HeroismControl != 1 || !heroism || !mf) && (calculationOptions.HeroismControl != 2 || !heroism || (incrementalSetIndex == Cooldown.Heroism)) && (calculationOptions.HeroismControl != 3 || !moltenFuryAvailable || !heroism || mf))
                                    {
                                        list.Add(new CastingState(calculationResult, incrementalSetIndex, false));
                                        if (incrementalSetIndex == 0)
                                        {
                                            calculationResult.BaseState = list[list.Count - 1];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }
    }
}
