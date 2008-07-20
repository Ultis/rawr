using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public sealed partial class Solver
    {
        private const int cooldownCount = 10;
        private const double segmentDuration = 30;
        private int segments;

        private List<CastingState> stateList;
        private List<SpellId> spellList;

        private SolverLP lp;
        private Heap<SolverLP> heap;
        private double[] solution;
        private int[] segmentColumn;
        private CharacterCalculationsMage calculationResult;
        private Character character;
        private CalculationOptionsMage calculationOptions;
        private string armor;

        private bool segmentCooldowns;
        private bool segmentNonCooldowns;
        private bool integralMana;
        private bool requiresMIP;

        private bool minimizeTime;
        private bool restrictManaUse;

        private bool heroismAvailable;
        private bool arcanePowerAvailable;
        private bool icyVeinsAvailable;
        private bool combustionAvailable;
        private bool moltenFuryAvailable;
        private bool trinket1Available;
        private bool trinket2Available;
        private bool coldsnapAvailable;
        private bool destructionPotionAvailable;
        private bool drumsOfBattleAvailable;
        private bool flameCapAvailable;
        
        private bool trinket1OnManaGem;
        private bool trinket2OnManaGem;
        private double trinket1Cooldown;
        private double trinket1Duration;
        private double trinket2Cooldown;
        private double trinket2Duration;

        private double coldsnapCooldown;

        #region LP rows
        private int rowManaRegen = -1;
        private int rowFightDuration = -1;
        private int rowEvocation = -1;
        private int rowPotion = -1;
        private int rowManaGem = -1;
        private int rowHeroism = -1;
        private int rowArcanePower = -1;
        private int rowHeroismArcanePower = -1;
        private int rowIcyVeins = -1;
        private int rowMoltenFury = -1;
        private int rowMoltenFuryDestructionPotion = -1;
        private int rowMoltenFuryIcyVeins = -1;
        private int rowHeroismDestructionPotion = -1;
        private int rowIcyVeinsDestructionPotion = -1;
        private int rowManaGemFlameCap = -1;
        private int rowMoltenFuryFlameCap = -1;
        private int rowFlameCapDestructionPotion = -1;
        private int rowTrinket1 = -1;
        private int rowTrinket2 = -1;
        private int rowMoltenFuryTrinket1 = -1;
        private int rowMoltenFuryTrinket2 = -1;
        private int rowHeroismTrinket1 = -1;
        private int rowHeroismTrinket2 = -1;
        private int rowTrinketManaGem = -1;
        private int rowDpsTime = -1;
        private int rowAoe = -1;
        private int rowFlamestrike = -1;
        private int rowConeOfCold = -1;
        private int rowBlastWave = -1;
        private int rowDragonsBreath = -1;
        private int rowCombustion = -1;
        private int rowMoltenFuryCombustion = -1;
        private int rowHeroismCombustion = -1;
        private int rowHeroismIcyVeins = -1;
        private int rowDrumsOfBattleActivation = -1;
        private int rowMoltenFuryDrumsOfBattle = -1;
        private int rowHeroismDrumsOfBattle = -1;
        private int rowIcyVeinsDrumsOfBattle = -1;
        private int rowArcanePowerDrumsOfBattle = -1;
        private int rowThreat = -1;
        private int rowManaPotionManaGem = -1;
        private int rowDrumsOfBattle = -1;
        private int rowDrinking = -1;
        private int rowTimeExtension = -1;
        private int rowAfterFightRegenMana = -1;
        private int rowAfterFightRegenHealth = -1;
        private int rowTargetDamage = -1;
        private int rowSegmentMoltenFury = -1;
        private int rowSegmentHeroism = -1;
        private int rowSegmentArcanePower = -1;
        private int rowSegmentIcyVeins = -1;
        private int rowSegmentCombustion = -1;
        private int rowSegmentDrumsOfBattle = -1;
        private int rowSegmentDrumsOfBattleActivation = -1;
        private int rowSegmentFlameCap = -1;
        private int rowSegmentManaGem = -1;
        private int rowSegmentPotion = -1;
        private int rowSegmentTrinket1 = -1;
        private int rowSegmentTrinket2 = -1;
        private int rowSegment = -1;
        private int rowSegmentManaOverflow = -1;
        private int rowSegmentManaUnderflow = -1;
        #endregion

        private Solver(Character character, CalculationOptionsMage calculationOptions, bool segmentCooldowns, bool integralMana, string armor)
        {
            this.character = character;
            this.calculationOptions = calculationOptions;
            this.segmentCooldowns = segmentCooldowns;
            this.integralMana = integralMana;
            this.armor = armor;
            requiresMIP = segmentCooldowns || integralMana;
        }

        private static bool IsItemActivatable(Item item)
        {
            if (item == null) return false;
            return (item.Stats.SpellDamageFor15SecOnUse2Min + item.Stats.SpellDamageFor20SecOnUse2Min + item.Stats.SpellHasteFor20SecOnUse2Min + item.Stats.Mp5OnCastFor20SecOnUse2Min + item.Stats.SpellDamageFor15SecOnManaGem + item.Stats.SpellDamageFor15SecOnUse90Sec + item.Stats.SpellHasteFor20SecOnUse5Min > 0);
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

        private static object calculationLock = new object();

        public static CharacterCalculationsMage GetCharacterCalculations(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, CalculationsMage calculations, string armor, bool segmentCooldowns, bool integralMana)
        {
            Solver solver = new Solver(character, calculationOptions, segmentCooldowns, integralMana, armor);
            return solver.PrivateGetCharacterCalculations(additionalItem, calculations);
        }

        private CharacterCalculationsMage PrivateGetCharacterCalculations(Item additionalItem, CalculationsMage calculations)
        {
            lock (calculationLock)
            {
                List<Buff> autoActivatedBuffs = new List<Buff>();
                Stats rawStats = calculations.GetRawStats(character, additionalItem, calculationOptions, autoActivatedBuffs, armor);
                Stats characterStats = calculations.GetCharacterStats(character, additionalItem, rawStats, calculationOptions);

                bool savedSmartOptimization = calculationOptions.SmartOptimization;
                bool savedABCycles = calculationOptions.ABCycles;
                bool savedDestructionPotion = calculationOptions.DestructionPotion;
                bool savedFlameCap = calculationOptions.FlameCap;

                //if (useSMP) calculationOptions.SmartOptimization = true;
                segments = (segmentCooldowns) ? (int)Math.Ceiling(calculationOptions.FightDuration / segmentDuration) : 1;
                segmentColumn = new int[segments + 1];

                heroismAvailable = calculationOptions.HeroismAvailable;
                arcanePowerAvailable = !calculationOptions.DisableCooldowns && (calculationOptions.ArcanePower == 1);
                icyVeinsAvailable = !calculationOptions.DisableCooldowns && (calculationOptions.IcyVeins == 1);
                combustionAvailable = !calculationOptions.DisableCooldowns && (calculationOptions.Combustion == 1);
                moltenFuryAvailable = calculationOptions.MoltenFury > 0;
                trinket1Available = !calculationOptions.DisableCooldowns && IsItemActivatable(character.Trinket1);
                trinket2Available = !calculationOptions.DisableCooldowns && IsItemActivatable(character.Trinket2);
                coldsnapAvailable = !calculationOptions.DisableCooldowns && (calculationOptions.ColdSnap == 1);
                destructionPotionAvailable = !calculationOptions.DisableCooldowns && calculationOptions.DestructionPotion;
                flameCapAvailable = !calculationOptions.DisableCooldowns && calculationOptions.FlameCap;
                drumsOfBattleAvailable = !calculationOptions.DisableCooldowns && calculationOptions.DrumsOfBattle;
                coldsnapCooldown = 8 * 60 * (1 - 0.1f * calculationOptions.IceFloes);

                trinket1OnManaGem = false;
                trinket2OnManaGem = false;

                if (calculationOptions.SmartOptimization)
                {
                    if (calculationOptions.SpellPower == 0)
                    {
                        calculationOptions.ABCycles = false;
                    }
                    else
                    {
                        calculationOptions.DestructionPotion = false;
                        calculationOptions.FlameCap = false;
                    }
                }

                calculationResult = new CharacterCalculationsMage();
                calculationResult.Calculations = calculations;
                calculationResult.BaseStats = characterStats;
                calculationResult.Character = character;
                calculationResult.CalculationOptions = calculationOptions;

                #region Setup Trinkets
                if (trinket1Available)
                {
                    Stats s = character.Trinket1.Stats;
                    if (s.SpellDamageFor20SecOnUse2Min + s.SpellHasteFor20SecOnUse2Min + s.Mp5OnCastFor20SecOnUse2Min > 0)
                    {
                        trinket1Duration = 20;
                        trinket1Cooldown = 120;
                    }
                    if (s.SpellDamageFor15SecOnManaGem > 0)
                    {
                        trinket1Duration = 15;
                        trinket1Cooldown = 120;
                        trinket1OnManaGem = true;
                    }
                    if (s.SpellDamageFor15SecOnUse90Sec > 0)
                    {
                        trinket1Duration = 15;
                        trinket1Cooldown = 90;
                    }
                    if (s.SpellHasteFor20SecOnUse5Min > 0)
                    {
                        trinket1Duration = 20;
                        trinket1Cooldown = 300;
                    }
                    if (s.SpellDamageFor15SecOnUse2Min > 0)
                    {
                        trinket1Duration = 15;
                        trinket1Cooldown = 120;
                    }
                    calculationResult.Trinket1Duration = trinket1Duration;
                    calculationResult.Trinket1Cooldown = trinket1Cooldown;
                    calculationResult.Trinket1Name = character.Trinket1.Name;
                }
                if (trinket2Available)
                {
                    Stats s = character.Trinket2.Stats;
                    if (s.SpellDamageFor20SecOnUse2Min + s.SpellHasteFor20SecOnUse2Min + s.Mp5OnCastFor20SecOnUse2Min > 0)
                    {
                        trinket2Duration = 20;
                        trinket2Cooldown = 120;
                    }
                    if (s.SpellDamageFor15SecOnManaGem > 0)
                    {
                        trinket2Duration = 15;
                        trinket2Cooldown = 120;
                        trinket2OnManaGem = true;
                    }
                    if (s.SpellDamageFor15SecOnUse90Sec > 0)
                    {
                        trinket2Duration = 15;
                        trinket2Cooldown = 90;
                    }
                    if (s.SpellHasteFor20SecOnUse5Min > 0)
                    {
                        trinket2Duration = 20;
                        trinket2Cooldown = 300;
                    }
                    if (s.SpellDamageFor15SecOnUse2Min > 0)
                    {
                        trinket2Duration = 15;
                        trinket2Cooldown = 120;
                    }
                    calculationResult.Trinket2Duration = trinket2Duration;
                    calculationResult.Trinket2Cooldown = trinket2Cooldown;
                    calculationResult.Trinket2Name = character.Trinket2.Name;
                }
                #endregion

                if (armor == null)
                {
                    if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Mage Armor"))) armor = "Mage Armor";
                    else if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Molten Armor"))) armor = "Molten Armor";
                    else if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Ice Armor"))) armor = "Ice Armor";
                }

                stateList = GetStateList(characterStats);
                spellList = GetSpellList();

                calculationResult.AutoActivatedBuffs.AddRange(autoActivatedBuffs);
                calculationResult.MageArmor = armor;

                List<double> tpsList;

                ConstructProblem(additionalItem, calculations, rawStats, characterStats, out tpsList);

                if (requiresMIP)
                {
                    RestrictSolution();
                }

                calculationResult.Solution = lp.Solve();

                if (minimizeTime)
                {
                    calculationResult.SubPoints[0] = -(float)(calculationOptions.TargetDamage / calculationResult.Solution[calculationResult.Solution.Length - 1]);
                }
                else
                {
                    calculationResult.SubPoints[0] = ((float)calculationResult.Solution[calculationResult.Solution.Length - 1] + calculationResult.WaterElementalDamage) / calculationOptions.FightDuration;
                }
                calculationResult.SubPoints[1] = EvaluateSurvivability(characterStats);
                calculationResult.OverallPoints = calculationResult.SubPoints[0] + calculationResult.SubPoints[1];

                float threat = 0;
                for (int i = 0; i < tpsList.Count; i++)
                {
                    threat += (float)(tpsList[i] * calculationResult.Solution[i]);
                }
                calculationResult.Tps = threat / calculationOptions.FightDuration;

                calculationOptions.SmartOptimization = savedSmartOptimization;
                calculationOptions.ABCycles = savedABCycles;
                calculationOptions.DestructionPotion = savedDestructionPotion;
                calculationOptions.FlameCap = savedFlameCap;

                return calculationResult;
            }
        }

        private float EvaluateSurvivability(Stats characterStats)
        {
            double ampMelee = (1 - 0.02 * calculationOptions.PrismaticCloak) * (1 - 0.01 * calculationOptions.ArcticWinds) * (1 - calculationResult.BaseState.MeleeMitigation) * (1 - calculationResult.BaseState.Dodge);
            double ampPhysical = (1 - 0.02 * calculationOptions.PrismaticCloak) * (1 - 0.01 * calculationOptions.ArcticWinds) * (1 - calculationResult.BaseState.MeleeMitigation);
            double ampArcane = (1 - 0.02 * calculationOptions.PrismaticCloak) * (1 + 0.01 * calculationOptions.PlayingWithFire) * Math.Max(1 - characterStats.ArcaneResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampFire = (1 - 0.02 * calculationOptions.PrismaticCloak) * (1 + 0.01 * calculationOptions.PlayingWithFire) * (1 - 0.02 * calculationOptions.FrozenCore) * Math.Max(1 - characterStats.FireResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampFrost = (1 - 0.02 * calculationOptions.PrismaticCloak) * (1 + 0.01 * calculationOptions.PlayingWithFire) * (1 - 0.02 * calculationOptions.FrozenCore) * Math.Max(1 - characterStats.FrostResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampNature = (1 - 0.02 * calculationOptions.PrismaticCloak) * (1 + 0.01 * calculationOptions.PlayingWithFire) * Math.Max(1 - characterStats.NatureResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampShadow = (1 - 0.02 * calculationOptions.PrismaticCloak) * (1 + 0.01 * calculationOptions.PlayingWithFire) * Math.Max(1 - characterStats.ShadowResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampHoly = (1 - 0.02 * calculationOptions.PrismaticCloak) * (1 + 0.01 * calculationOptions.PlayingWithFire);

            double melee = ampMelee * (calculationOptions.MeleeDps * (1 + Math.Max(0, calculationOptions.MeleeCrit / 100.0 - calculationResult.BaseState.PhysicalCritReduction) * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1)) + calculationOptions.MeleeDot * (1 - 0.5f * calculationResult.BaseState.CritDamageReduction));
            double physical = ampPhysical * (calculationOptions.PhysicalDps * (1 + Math.Max(0, calculationOptions.PhysicalCrit / 100.0 - calculationResult.BaseState.PhysicalCritReduction) * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1)) + calculationOptions.PhysicalDot * (1 - 0.5f * calculationResult.BaseState.CritDamageReduction));
            double arcane = ampArcane * (calculationOptions.ArcaneDps * (1 + Math.Max(0, calculationOptions.ArcaneCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (1.75 * (1 - calculationResult.BaseState.CritDamageReduction) - 1)) + calculationOptions.ArcaneDot * (1 - 0.5f * calculationResult.BaseState.CritDamageReduction));
            double fire = ampFire * (calculationOptions.FireDps * (1 + Math.Max(0, calculationOptions.FireCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (2.1 * (1 - calculationResult.BaseState.CritDamageReduction) - 1)) + calculationOptions.FireDot * (1 - 0.5f * calculationResult.BaseState.CritDamageReduction));
            double frost = ampFrost * (calculationOptions.FrostDps * (1 + Math.Max(0, calculationOptions.FrostCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1)) + calculationOptions.FrostDot * (1 - 0.5f * calculationResult.BaseState.CritDamageReduction));
            double holy = ampHoly * (calculationOptions.HolyDps * (1 + Math.Max(0, calculationOptions.HolyCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (1.5 * (1 - calculationResult.BaseState.CritDamageReduction) - 1)) + calculationOptions.HolyDot * (1 - 0.5f * calculationResult.BaseState.CritDamageReduction));
            double nature = ampNature * (calculationOptions.NatureDps * (1 + Math.Max(0, calculationOptions.NatureCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1)) + calculationOptions.NatureDot * (1 - 0.5f * calculationResult.BaseState.CritDamageReduction));
            double shadow = ampShadow * (calculationOptions.ShadowDps * (1 + Math.Max(0, calculationOptions.ShadowCrit / 100.0 - calculationResult.BaseState.SpellCritReduction) * (2 * (1 - calculationResult.BaseState.CritDamageReduction) - 1)) + calculationOptions.ShadowDot * (1 - 0.5f * calculationResult.BaseState.CritDamageReduction));

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

            calculationResult.ChanceToDie = (float)(0.5f * (1f - XMath.Erf((characterStats.Health / burstWindow + characterStats.Hp5 / 5 - Xmean) / Math.Sqrt(2 * (Xvar * (1 + Tk) + Xmean * Xmean * Tk)))));
            calculationResult.MeanIncomingDps = (float)Xmean;

            //double maxTimeToDie = 1.0 / (1 - calculationOptions.ChanceToLiveLimit / 100.0) - 1;
            //double timeToDie = Math.Min(1.0 / calculatedStats.ChanceToDie - 1, maxTimeToDie);

            //calculatedStats.SubPoints[1] = calculatedStats.BasicStats.Health * calculationOptions.SurvivabilityRating + (float)(calculationOptions.ChanceToLiveScore * timeToDie / maxTimeToDie);
            float ret = calculationResult.BaseStats.Health * calculationOptions.SurvivabilityRating + (float)(calculationOptions.ChanceToLiveScore * Math.Pow(1 - calculationResult.ChanceToDie, 0.1));
            if (float.IsNaN(ret)) ret = 0f;
            return ret;
        }

        private unsafe void ConstructProblem(Item additionalItem, CalculationsMage calculations, Stats rawStats, Stats characterStats, out List<double> tpsList)
        {
            calculationResult.StartingMana = Math.Min(characterStats.Mana, calculationResult.BaseState.ManaRegenDrinking * calculationOptions.DrinkingTime);
            double maxDrinkingTime = Math.Min(30, (characterStats.Mana - calculationResult.StartingMana) / calculationResult.BaseState.ManaRegenDrinking);
            bool drinkingEnabled = (maxDrinkingTime > 0.000001);

            bool needsTimeExtension = false;
            bool afterFightRegen = calculationOptions.FarmingMode;

            minimizeTime = false;
            if (calculationOptions.TargetDamage > 0)
            {
                minimizeTime = true;
            }
            if (minimizeTime) needsTimeExtension = true;

            if (segmentCooldowns && (flameCapAvailable || destructionPotionAvailable)) restrictManaUse = true;
            if (restrictManaUse) segmentNonCooldowns = true;

            int rowCount = ConstructRows(minimizeTime, drinkingEnabled, needsTimeExtension, afterFightRegen);

            lp = new SolverLP(rowCount, 9 + (2 + spellList.Count * stateList.Count) * segments, calculationResult, segments);
            tpsList = new List<double>();
            double tps;
            calculationResult.SolutionVariable = new List<SolutionVariable>();

            fixed (double* pRowScale = SolverLP.rowScale, pColumnScale = SolverLP.columnScale, pCost = LP._cost, pB = LP._b, pData = SparseMatrix.data, pValue = SparseMatrix.value)
            fixed (int* pRow = SparseMatrix.row, pCol = SparseMatrix.col)
            {
                lp.BeginUnsafe(pRowScale, pColumnScale, pCost, pB, pData, pValue, pRow, pCol);

                #region Set LP Scaling
                lp.SetRowScaleUnsafe(rowManaRegen, 0.1);
                lp.SetRowScaleUnsafe(rowManaGem, 40.0);
                lp.SetRowScaleUnsafe(rowPotion, 40.0);
                lp.SetRowScaleUnsafe(rowManaGemFlameCap, 40.0);
                lp.SetRowScaleUnsafe(rowCombustion, 10.0);
                lp.SetRowScaleUnsafe(rowHeroismCombustion, 10.0);
                lp.SetRowScaleUnsafe(rowMoltenFuryCombustion, 10.0);
                lp.SetRowScaleUnsafe(rowDrumsOfBattleActivation, 30.0);
                lp.SetRowScaleUnsafe(rowThreat, 0.05);
                lp.SetRowScaleUnsafe(rowCount, 0.05);
                if (restrictManaUse)
                {
                    for (int ss = 0; ss < segments - 1; ss++)
                    {
                        lp.SetRowScaleUnsafe(rowSegmentManaOverflow + ss, 0.1);
                        lp.SetRowScaleUnsafe(rowSegmentManaUnderflow + ss, 0.1);
                    }
                }
                #endregion

                float threatFactor = (1 + characterStats.ThreatIncreaseMultiplier) * (1 - characterStats.ThreatReductionMultiplier);
                float dpsTime = calculationOptions.DpsTime;
                float silenceTime = calculationOptions.EffectShadowSilenceFrequency * calculationOptions.EffectShadowSilenceDuration * Math.Max(1 - characterStats.ShadowResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
                if (1 - silenceTime < dpsTime) dpsTime = 1 - silenceTime;

                #region Formulate LP
                // idle regen
                int column = -1;
                double manaRegen;
                int idleRegenSegments = (restrictManaUse) ? segments : 1;
                manaRegen = -(calculationResult.BaseState.ManaRegen * (1 - calculationOptions.Fragmentation) + calculationResult.BaseState.ManaRegen5SR * calculationOptions.Fragmentation);
                for (int segment = 0; segment < idleRegenSegments; segment++)
                {
                    column = lp.AddColumnUnsafe();
                    if (segment == 0) calculationResult.ColumnIdleRegen = column;
                    calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.IdleRegen, Segment = segment });
                    tpsList.Add(0.0);
                    lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                    lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetElementUnsafe(rowDpsTime, column, -1.0);
                    lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                    if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                    if (restrictManaUse)
                    {
                        for (int ss = segment; ss < segments - 1; ss++)
                        {
                            lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                            lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                        }
                    }
                }
                // wand
                if (character.Ranged != null && character.Ranged.Type == Item.ItemType.Wand)
                {
                    int wandSegments = (restrictManaUse) ? segments : 1;
                    Spell wand = new Wand(calculationResult.BaseState, (MagicSchool)character.Ranged.DamageType, character.Ranged.MinDamage, character.Ranged.MaxDamage, character.Ranged.Speed);
                    calculationResult.BaseState.SetSpell(SpellId.Wand, wand);
                    manaRegen = wand.CostPerSecond - wand.ManaRegenPerSecond;
                    for (int segment = 0; segment < wandSegments; segment++)
                    {
                        column = lp.AddColumnUnsafe();
                        if (segment == 0) calculationResult.ColumnWand = column;
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.Wand, Spell = wand, Segment = segment, State = calculationResult.BaseState });
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                        lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                        lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = wand.ThreatPerSecond);
                        lp.SetElementUnsafe(rowTargetDamage, column, -wand.DamagePerSecond);
                        lp.SetCostUnsafe(column, minimizeTime ? -1 : wand.DamagePerSecond);
                        tpsList.Add(tps);
                        if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                            }
                        }
                    }
                }
                // evocation
                if (calculationOptions.EvocationEnabled)
                {
                    int evocationSegments = (restrictManaUse) ? segments : 1;
                    double evocationDuration = (8f + characterStats.EvocationExtension) / calculationResult.BaseState.CastingSpeed;
                    calculationResult.EvocationDuration = evocationDuration;
                    float evocationMana = characterStats.Mana;
                    calculationResult.EvocationRegen = calculationResult.BaseState.ManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed;
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
                        float evocationRegen = ((0.001f + evocationStats.Spirit * 0.009327f * (float)Math.Sqrt(evocationStats.Intellect)) * evocationStats.SpellCombatManaRegeneration + evocationStats.Mp5 / 5f + calculationResult.BaseState.SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration) + 0.15f * evocationStats.Mana / 2f * calculationResult.BaseState.CastingSpeed;
                        if (evocationRegen > calculationResult.EvocationRegen)
                        {
                            evocationMana = evocationStats.Mana;
                            calculationResult.EvocationRegen = evocationRegen;
                        }
                    }
                    if (calculationResult.EvocationRegen * evocationDuration > characterStats.Mana)
                    {
                        evocationDuration = characterStats.Mana / calculationResult.EvocationRegen;
                        calculationResult.EvocationDuration = evocationDuration;
                    }
                    for (int segment = 0; segment < evocationSegments; segment++)
                    {
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.Evocation, Segment = segment });
                        column = lp.AddColumnUnsafe();
                        if (segment == 0) calculationResult.ColumnEvocation = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.EvocationRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, -calculationResult.EvocationRegen);
                        lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                        lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                        lp.SetElementUnsafe(rowEvocation, column, 1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 0.5f * threatFactor); // should split among all targets if more than one, assume one only
                        lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                        tpsList.Add(tps);
                        if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, -calculationResult.EvocationRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, calculationResult.EvocationRegen);
                            }
                        }
                    }
                }
                // mana potion
                if (calculationOptions.ManaPotionEnabled)
                {
                    int manaPotionSegments = (segmentCooldowns && (destructionPotionAvailable || restrictManaUse)) ? segments : 1;
                    calculationResult.MaxManaPotion = 1 + (int)((calculationOptions.FightDuration - 30f) / 120f);
                    manaRegen = -(1 + characterStats.BonusManaPotion) * 2400f;
                    for (int segment = 0; segment < manaPotionSegments; segment++)
                    {
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.ManaPotion, Segment = segment });
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnScaleUnsafe(column, 1.0 / 40.0);
                        if (segment == 0) calculationResult.ColumnManaPotion = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                        lp.SetElementUnsafe(rowPotion, column, 1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = (1 + characterStats.BonusManaPotion) * 2400f * 0.5f * threatFactor);
                        lp.SetElementUnsafe(rowManaPotionManaGem, column, 40.0);
                        lp.SetCostUnsafe(column, 0.0);
                        tpsList.Add(tps);
                        if (segmentCooldowns && destructionPotionAvailable)
                        {
                            for (int ss = 0; ss < segments; ss++)
                            {
                                double cool = 120;
                                int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentPotion + ss, column, 15.0);
                                if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                            }
                        }
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                            }
                        }
                    }
                }
                // mana gem
                if (calculationOptions.ManaGemEnabled)
                {
                    int manaGemSegments = (segmentCooldowns && (flameCapAvailable || restrictManaUse)) ? segments : 1;
                    calculationResult.MaxManaGem = Math.Min(5, 1 + (int)((calculationOptions.FightDuration - 30f) / 120f));
                    double manaGemRegenAvg = (1 + characterStats.BonusManaGem) * (-Math.Min(3, 1 + (int)((calculationOptions.FightDuration - 30f) / 120f)) * 2400f - ((calculationOptions.FightDuration >= 390) ? 1100f : 0f) - ((calculationOptions.FightDuration >= 510) ? 850 : 0)) / (calculationResult.MaxManaGem);
                    for (int segment = 0; segment < manaGemSegments; segment++)
                    {
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.ManaGem, Segment = segment });
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnScaleUnsafe(column, 1.0 / 40.0);
                        if (segment == 0) calculationResult.ColumnManaGem = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaGemRegenAvg);
                        lp.SetElementUnsafe(rowManaRegen, column, manaGemRegenAvg);
                        lp.SetElementUnsafe(rowManaGem, column, 1.0);
                        lp.SetElementUnsafe(rowManaGemFlameCap, column, 1.0);
                        lp.SetElementUnsafe(rowTrinketManaGem, column, -1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = -manaGemRegenAvg * 0.5f * threatFactor);
                        lp.SetElementUnsafe(rowManaPotionManaGem, column, 40.0);
                        lp.SetCostUnsafe(column, 0.0);
                        tpsList.Add(tps);
                        if (segmentCooldowns && flameCapAvailable)
                        {
                            for (int ss = 0; ss < segments; ss++)
                            {
                                double cool = 120;
                                int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentManaGem + ss, column, 60.0);
                                if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                            }
                        }
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaGemRegenAvg);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaGemRegenAvg);
                            }
                        }
                    }
                }
                // drums
                if (drumsOfBattleAvailable)
                {
                    List<CastingState> drumsStates = new List<CastingState>();
                    //int drums = 0x4;
                    //int drumsAndFC = 0x6;
                    bool found = false;
                    for (int i = 0; i < stateList.Count; i++)
                    {
                        if (stateList[i].IncrementalSetIndex == 4 && !stateList[i].Trinket1 && !stateList[i].Trinket2)
                        {                            
                            drumsStates.Add(stateList[i]);
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        drumsStates.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, false, false, false, false, false, false, false, false, true, 4));
                    }
                    if (flameCapAvailable)
                    {
                        found = false;
                        for (int i = 0; i < stateList.Count; i++)
                        {
                            if (stateList[i].IncrementalSetIndex == 6 && !stateList[i].Trinket1 && !stateList[i].Trinket2)
                            {
                                drumsStates.Add(stateList[i]);
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            drumsStates.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, false, false, false, false, true, false, false, false, true, 6));
                        }
                    }

                    int drumsOfBattleSegments = segments; // always segment, we need it to guarantee each block has activation
                    manaRegen = -calculationResult.BaseState.ManaRegen5SR;
                    foreach (CastingState state in drumsStates)
                    {
                        for (int segment = 0; segment < drumsOfBattleSegments; segment++)
                        {
                            calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.DrumsOfBattle, Segment = segment, State = state });
                            column = lp.AddColumnUnsafe();
                            if (segment == 0) calculationResult.ColumnDrumsOfBattle = column;
                            lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                            lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                            lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                            lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                            lp.SetElementUnsafe(rowDrumsOfBattleActivation, column, -1 / calculationResult.BaseState.GlobalCooldown);
                            lp.SetElementUnsafe(rowDrumsOfBattle, column, 1.0);
                            if (state.FlameCap) lp.SetElementUnsafe(rowManaGemFlameCap, column, 1.0 / 40.0);
                            lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                            tpsList.Add(0.0);
                            if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                            if (restrictManaUse)
                            {
                                for (int ss = segment; ss < segments - 1; ss++)
                                {
                                    lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                    lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                                }
                            }
                            if (segmentCooldowns)
                            {
                                for (int ss = 0; ss < segments; ss++)
                                {
                                    double cool = 120;
                                    int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                    if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                    if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentDrumsOfBattle + ss, column, 1.0);
                                    if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                                }
                                for (int ss = 0; ss < segments; ss++)
                                {
                                    double cool = 120;
                                    int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                    if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                    if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentDrumsOfBattleActivation + ss, column, 1.0);
                                    if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                                }
                                if (state.FlameCap)
                                {
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = 180;
                                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentFlameCap + ss, column, 1.0);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                                    }
                                    for (int ss = 0; ss < segments; ss++)
                                    {
                                        double cool = 120;
                                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentManaGem + ss, column, 1.0);
                                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                                    }
                                }
                            }
                        }
                    }
                }
                // drinking
                if (drinkingEnabled)
                {
                    calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.Drinking });
                    calculationResult.ColumnDrinking = column = lp.AddColumnUnsafe();
                    manaRegen = -calculationResult.BaseState.ManaRegenDrinking;
                    lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                    lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetElementUnsafe(rowDrinking, column, 1.0);
                    lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                    tpsList.Add(0.0);
                    if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + 0, column, 1.0);
                    if (restrictManaUse)
                    {
                        for (int ss = 0; ss < segments - 1; ss++)
                        {
                            lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                            lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                        }
                    }
                }
                // time extension
                if (needsTimeExtension)
                {
                    calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.TimeExtension });
                    calculationResult.ColumnTimeExtension = column = lp.AddColumnUnsafe();
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetElementUnsafe(rowEvocation, column, calculationResult.EvocationDuration / 480.0);
                    lp.SetElementUnsafe(rowPotion, column, 1.0 / 120.0);
                    lp.SetElementUnsafe(rowManaGem, column, 1.0 / 120.0);
                    lp.SetElementUnsafe(rowArcanePower, column, 15.0 / 180.0);
                    lp.SetElementUnsafe(rowIcyVeins, column, 20.0 / 180.0 + (coldsnapAvailable ? 20.0 / coldsnapCooldown : 0.0));
                    lp.SetElementUnsafe(rowMoltenFury, column, calculationOptions.MoltenFuryPercentage);
                    lp.SetElementUnsafe(rowManaGemFlameCap, column, 1f / 120f);
                    lp.SetElementUnsafe(rowTrinket1, column, trinket1Duration / trinket1Cooldown);
                    lp.SetElementUnsafe(rowTrinket2, column, trinket2Duration / trinket2Cooldown);
                    lp.SetElementUnsafe(rowDpsTime, column, -(1 - dpsTime));
                    lp.SetElementUnsafe(rowAoe, column, calculationOptions.AoeDuration);
                    lp.SetElementUnsafe(rowCombustion, column, 1.0 / 180.0);
                    lp.SetElementUnsafe(rowDrumsOfBattle, column, 30.0 / 120.0);
                    tpsList.Add(0.0);
                }
                // after fight regen
                if (afterFightRegen)
                {
                    calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.AfterFightRegen });
                    calculationResult.ColumnAfterFightRegen = column = lp.AddColumnUnsafe();
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetElementUnsafe(rowAfterFightRegenMana, column, -calculationResult.BaseState.ManaRegenDrinking);
                    lp.SetCostUnsafe(column, minimizeTime ? -1 : 0);
                    tpsList.Add(0.0);
                }
                // mana overflow
                if (restrictManaUse)
                {
                    for (int segment = 0; segment < segments; segment++)
                    {
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.ManaOverflow, Segment = segment });
                        column = lp.AddColumnUnsafe();
                        if (segment == 0) calculationResult.ColumnManaOverflow = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, 1.0);
                        lp.SetElementUnsafe(rowManaRegen, column, 1.0);
                        tpsList.Add(0.0);
                        for (int ss = segment; ss < segments - 1; ss++)
                        {
                            lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, 1.0);
                            lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -1.0);
                        }
                    }
                }
                // spells
                if (calculationOptions.IncrementalOptimizations)
                {
                    int lastSegment = -1;
                    for (int index = 0; index < calculationOptions.IncrementalSetStateIndexes.Length; index++)
                    {
                        for (int buffset = 0; buffset < stateList.Count; buffset++)
                        {
                            if (calculationOptions.IncrementalSetStateIndexes[index] == stateList[buffset].IncrementalSetIndex)
                            {
                                column = lp.AddColumnUnsafe();
                                Spell s = stateList[buffset].GetSpell(calculationOptions.IncrementalSetSpells[index]);
                                int seg = calculationOptions.IncrementalSetSegments[index];
                                if (seg != lastSegment)
                                {
                                    for (; lastSegment < seg; )
                                    {
                                        segmentColumn[++lastSegment] = column;
                                    }
                                }
                                calculationResult.SolutionVariable.Add(new SolutionVariable() { State = stateList[buffset], Spell = s, Segment = seg, Type = VariableType.Spell });
                                SetSpellColumn(minimizeTime, tpsList, seg, stateList[buffset], column, s);
                            }
                        }
                    }
                    for (; lastSegment < segments; )
                    {
                        segmentColumn[++lastSegment] = column + 1;
                    }
                }
                else
                {
                    int firstMoltenFurySegment = (int)((calculationOptions.FightDuration - calculationOptions.MoltenFuryPercentage * calculationOptions.FightDuration) / segmentDuration);

                    for (int seg = 0; seg < segments; seg++)
                    {
                        segmentColumn[seg] = column + 1;
                        for (int buffset = 0; buffset < stateList.Count; buffset++)
                        {
                            for (int spell = 0; spell < spellList.Count; spell++)
                            {
                                if (segmentCooldowns && stateList[buffset].MoltenFury && seg < firstMoltenFurySegment) continue;
                                if (!segmentNonCooldowns && stateList[buffset] == calculationResult.BaseState && seg != 0) continue;
                                Spell s = stateList[buffset].GetSpell(spellList[spell]);
                                if (s.AffectedByFlameCap || !stateList[buffset].FlameCap)
                                {
                                    column = lp.AddColumnUnsafe();
                                    calculationResult.SolutionVariable.Add(new SolutionVariable() { State = stateList[buffset], Spell = s, Segment = seg, Type = VariableType.Spell });
                                    SetSpellColumn(minimizeTime, tpsList, seg, stateList[buffset], column, s);
                                }
                            }
                        }
                    }
                    segmentColumn[segments] = column + 1;
                }

                lp.lp.EndColumnConstruction();
                SetProblemRHS(maxDrinkingTime);
                #endregion

                lp.EndUnsafe();
            }
        }

        private void SetProblemRHS(double maxDrinkingTime)
        {
            #region Water Elemental
            int coldsnapCount = coldsnapAvailable ? (1 + (int)((calculationOptions.FightDuration - 45f) / coldsnapCooldown)) : 0;

            // water elemental
            if (calculationOptions.SummonWaterElemental == 1)
            {
                int targetLevel = calculationOptions.TargetLevel;
                calculationResult.WaterElemental = true;
                // 45 sec, 3 min cooldown + cold snap
                // 2.5 sec Waterbolt, affected by heroism, totems, 0.4x frost damage from character
                // TODO consider adding water elemental as part of optimization for stacking with cooldowns
                // TODO add GCD for summoning and mana cost
                float spellHit = 0;
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Totem of Wrath"))) spellHit += 0.03f;
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Inspiring Presence"))) spellHit += 0.01f;
                float hitRate = Math.Min(0.99f, ((targetLevel <= 72) ? (0.96f - (targetLevel - 70) * 0.01f) : (0.94f - (targetLevel - 72) * 0.11f)) + spellHit);
                float spellCrit = 0.05f;
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Winter's Chill")) || calculationOptions.WintersChill == 1) spellHit += 0.1f;
                float multiplier = hitRate;
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Curse of the Elements"))) multiplier *= 1.1f;
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Improved Curse of the Elements"))) multiplier *= 1.13f / 1.1f;
                if (character.ActiveBuffs.Contains(Buff.GetBuffByName("Misery"))) multiplier *= 1.05f;
                float realResistance = calculationOptions.FrostResist;
                float partialResistFactor = (realResistance == 1) ? 0 : (1 - realResistance - ((targetLevel > 70) ? ((targetLevel - 70) * 0.02f) : 0f));
                multiplier *= partialResistFactor;
                calculationResult.WaterElementalDps = (521.5f + (0.4f * calculationResult.BaseState.FrostDamage + (character.ActiveBuffs.Contains(Buff.GetBuffByName("Wrath of Air")) ? 101 : 0)) * 2.5f / 3.5f) * multiplier * (1 + 0.5f * spellCrit) / 2.5f;
                calculationResult.WaterElementalDuration = (float)(1 + (int)((calculationOptions.FightDuration - 45f) / 180f)) * 45;
                if (coldsnapAvailable) calculationResult.WaterElementalDuration = (float)MaximizeColdsnapDuration(calculationOptions.FightDuration, coldsnapCooldown, 45.0, 180.0, out coldsnapCount);
                /*calculatedStats.WaterElementalDuration = (float)(1 + coldsnapCount + (int)((calculatedStats.FightDuration - coldsnapCount * coldsnapDelay - 45f) / 180f)) * 45;
                float nextElementalEnd = (float)((calculatedStats.WaterElementalDuration / 45f - coldsnapCount) * 180f + coldsnapCount * coldsnapDelay + 45f);
                if (nextElementalEnd - 45.0f < calculationOptions.FightDuration) calculatedStats.WaterElementalDuration += calculationOptions.FightDuration - nextElementalEnd + 45.0f;
                calculatedStats.WaterElementalDuration = Math.Min(calculatedStats.WaterElementalDuration, calculationOptions.FightDuration);*/
                if (heroismAvailable)
                {
                    float heroTime = Math.Min(40.0f, calculationResult.WaterElementalDuration);
                    calculationResult.WaterElementalDamage = calculationResult.WaterElementalDps * ((calculationResult.WaterElementalDuration - heroTime) + heroTime * 1.3f);
                }
                else
                    calculationResult.WaterElementalDamage = calculationResult.WaterElementalDuration * calculationResult.WaterElementalDps;
            }
            #endregion

            float combustionCount = combustionAvailable ? (1 + (int)((calculationOptions.FightDuration - 15f) / 195f)) : 0;

            double ivlength = 0.0;
            if (calculationOptions.SummonWaterElemental == 0 && coldsnapAvailable)
            {
                ivlength = Math.Floor(MaximizeColdsnapDuration(calculationOptions.FightDuration, coldsnapCooldown, 20.0, 180.0, out coldsnapCount));
            }
            else if (calculationOptions.SummonWaterElemental == 1 && coldsnapAvailable)
            {
                double wecount = (calculationResult.WaterElementalDuration / 45.0);
                if (wecount >= Math.Floor(wecount) + 20.0 / 45.0)
                    ivlength = Math.Ceiling(wecount) * 20.0;
                else
                    ivlength = Math.Floor(wecount) * 20.0;
            }
            else
            {
                ivlength = (1 + (int)((calculationOptions.FightDuration - 20f) / 180f)) * 20;
            }

            double aplength = (1 + (int)((calculationOptions.FightDuration - 30f) / 180f)) * 15;
            double mflength = calculationOptions.MoltenFuryPercentage * calculationOptions.FightDuration;
            double dpivstackArea = calculationOptions.FightDuration;
            //if (mfAvailable && heroismAvailable) dpivstackArea -= 120; // only applies if heroism and iv cannot stack
            double dpivlength = 15 * (int)(dpivstackArea / 360f);
            if (dpivstackArea % 360f < 195)
            {
                dpivlength += 15;
            }
            else
            {
                dpivlength += 30;
            }
            double dpflamelength = 15 * (int)(calculationOptions.FightDuration / 360f);
            if (calculationOptions.FightDuration % 360f < 195)
            {
                dpflamelength += 15;
            }
            else
            {
                dpflamelength += 30;
            }
            double drumsivlength = 20 * (int)(calculationOptions.FightDuration / 360f);
            if (calculationOptions.FightDuration % 360f < 195)
            {
                drumsivlength += 20;
            }
            else
            {
                drumsivlength += 40;
            }
            double drumsaplength = 15 * (int)(calculationOptions.FightDuration / 360f);
            if (calculationOptions.FightDuration % 360f < 195)
            {
                drumsaplength += 15;
            }
            else
            {
                drumsaplength += 30;
            }

            // mana burn estimate
            float manaBurn = 80;
            if (calculationOptions.AoeDuration > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ArcaneExplosion);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (calculationOptions.EmpoweredFireball > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.Fireball);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (calculationOptions.EmpoweredFrostbolt > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.Frostbolt);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (calculationOptions.SpellPower > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ArcaneBlast33);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            if (icyVeinsAvailable)
            {
                manaBurn *= 1.1f;
            }
            if (arcanePowerAvailable)
            {
                manaBurn *= 1.1f;
            }

            if (calculationOptions.FightDuration - 7800 / manaBurn < 0) // fix for maximum pot+gem constraint
            {
                manaBurn = 7800 / calculationOptions.FightDuration;
            }

            lp.SetRHSUnsafe(rowManaRegen, calculationResult.StartingMana);
            lp.SetRHSUnsafe(rowFightDuration, calculationOptions.FightDuration);
            lp.SetRHSUnsafe(rowTimeExtension, -calculationOptions.FightDuration);
            lp.SetRHSUnsafe(rowEvocation, calculationResult.EvocationDuration * Math.Max(1, (1 + Math.Floor((calculationOptions.FightDuration - 200f) / 480f))));
            lp.SetRHSUnsafe(rowPotion, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaPotion);
            lp.SetRHSUnsafe(rowManaGem, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaGem);
            if (heroismAvailable) lp.SetRHSUnsafe(rowHeroism, 40);
            if (arcanePowerAvailable) lp.SetRHSUnsafe(rowArcanePower, calculationOptions.AverageCooldowns ? 15.0 / 180.0 * calculationOptions.FightDuration : aplength);
            if (heroismAvailable && arcanePowerAvailable) lp.SetRHSUnsafe(rowHeroismArcanePower, 15);
            if (icyVeinsAvailable) lp.SetRHSUnsafe(rowIcyVeins, calculationOptions.AverageCooldowns ? (20.0 / 180.0 + (coldsnapAvailable ? 20.0 / coldsnapCooldown : 0.0)) * calculationOptions.FightDuration : ivlength);
            if (moltenFuryAvailable) lp.SetRHSUnsafe(rowMoltenFury, mflength);
            if (moltenFuryAvailable) lp.SetRHSUnsafe(rowMoltenFuryDestructionPotion, 15);
            if (moltenFuryAvailable && icyVeinsAvailable) lp.SetRHSUnsafe(rowMoltenFuryIcyVeins, coldsnapAvailable ? 40 : 20);
            if (heroismAvailable) lp.SetRHSUnsafe(rowHeroismDestructionPotion, 15);
            if (icyVeinsAvailable) lp.SetRHSUnsafe(rowIcyVeinsDestructionPotion, dpivlength);
            if (segmentCooldowns)
            {
                lp.SetRHSUnsafe(rowManaGemFlameCap, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : Math.Max(((int)((calculationOptions.FightDuration - 60.0) / 60.0)) * 0.5 + 1.5, calculationResult.MaxManaGem));
            }
            else if (calculationOptions.FlameCap && !(!calculationOptions.SmartOptimization && calculationOptions.SpellPower > 0))
            {
                lp.SetRHSUnsafe(rowManaGemFlameCap, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : ((int)(calculationOptions.FightDuration / 180.0 + 2.0 / 3.0)) * 3.0 / 2.0);
            }
            else
            {
                lp.SetRHSUnsafe(rowManaGemFlameCap, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaGem);
            }
            if (moltenFuryAvailable) lp.SetRHSUnsafe(rowMoltenFuryFlameCap, 60);
            lp.SetRHSUnsafe(rowFlameCapDestructionPotion, dpflamelength);
            if (trinket1Available) lp.SetRHSUnsafe(rowTrinket1, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * trinket1Duration / trinket1Cooldown : ((1 + (int)((calculationOptions.FightDuration - trinket1Duration) / trinket1Cooldown)) * trinket1Duration));
            if (trinket2Available) lp.SetRHSUnsafe(rowTrinket2, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * trinket2Duration / trinket2Cooldown : ((1 + (int)((calculationOptions.FightDuration - trinket2Duration) / trinket2Cooldown)) * trinket2Duration));
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
            lp.SetRHSUnsafe(rowMoltenFuryDrumsOfBattle, 30 - calculationResult.BaseState.GlobalCooldown);
            lp.SetRHSUnsafe(rowHeroismDrumsOfBattle, 30 - calculationResult.BaseState.GlobalCooldown);
            lp.SetRHSUnsafe(rowIcyVeinsDrumsOfBattle, drumsivlength);
            lp.SetRHSUnsafe(rowArcanePowerDrumsOfBattle, drumsaplength);
            lp.SetRHSUnsafe(rowThreat, calculationOptions.TpsLimit * calculationOptions.FightDuration);
            double manaConsum;
            if (integralMana)
            {
                manaConsum = Math.Ceiling((calculationOptions.FightDuration - 7800 / manaBurn) / 60f + 2);
            }
            else
            {
                manaConsum = ((calculationOptions.FightDuration - 7800 / manaBurn) / 60f + 2);
            }
            if ((trinket1OnManaGem || trinket2OnManaGem) && manaConsum < calculationResult.MaxManaGem) manaConsum = calculationResult.MaxManaGem;
            lp.SetRHSUnsafe(rowManaPotionManaGem, manaConsum * 40.0);
            lp.SetRHSUnsafe(rowDrumsOfBattle, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * 30.0 / 120.0 : 30.0 * (1 + (int)((calculationOptions.FightDuration - 30) / 120)));
            lp.SetRHSUnsafe(rowDrinking, maxDrinkingTime);
            lp.SetRHSUnsafe(rowTargetDamage, -calculationOptions.TargetDamage);

            if (segmentCooldowns)
            {
                if (moltenFuryAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        if ((seg + 1) * segmentDuration > calculationOptions.FightDuration - mflength)
                        {
                            if (calculationOptions.FightDuration - mflength < seg * segmentDuration) lp.SetRHSUnsafe(rowSegmentMoltenFury + seg, segmentDuration);
                            else lp.SetRHSUnsafe(rowSegmentMoltenFury + seg, Math.Max(0, segmentDuration - (calculationOptions.FightDuration - mflength - seg * segmentDuration)));
                        }
                    }
                }
                // heroism
                // ap
                if (arcanePowerAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentArcanePower + seg, 15.0);
                        double cool = 180;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // iv
                if (icyVeinsAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentIcyVeins + seg, 20 + (coldsnapAvailable ? 20 : 0));
                        double cool = 180 + (coldsnapAvailable ? 20 : 0);
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // combustion
                if (combustionAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentCombustion + seg, 1.0);
                        double cool = 180 + 15;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // drums
                if (drumsOfBattleAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentDrumsOfBattle + seg, 30.0);
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentDrumsOfBattleActivation + seg, calculationResult.BaseState.GlobalCooldown);
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // flamecap
                if (flameCapAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentFlameCap + seg, 60.0);
                        double cool = 180;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentManaGem + seg, 60.0);
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // destruction
                if (destructionPotionAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentPotion + seg, 15.0);
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // t1
                if (trinket1Available)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentTrinket1 + seg, trinket1Duration);
                        double cool = trinket1Cooldown;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // t2
                if (trinket2Available)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentTrinket2 + seg, trinket2Duration);
                        double cool = trinket2Cooldown;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // timing
                for (int seg = 0; seg < segments; seg++)
                {
                    lp.SetRHSUnsafe(rowSegment + seg, segmentDuration);
                }
            }
            if (restrictManaUse)
            {
                for (int ss = 0; ss < segments - 1; ss++)
                {
                    lp.SetRHSUnsafe(rowSegmentManaUnderflow + ss, calculationResult.StartingMana);
                    lp.SetRHSUnsafe(rowSegmentManaOverflow + ss, calculationResult.BaseStats.Mana - calculationResult.StartingMana);
                }
            }
        }

        private int ConstructRows(bool minimizeTime, bool drinkingEnabled, bool needsTimeExtension, bool afterFightRegen)
        {
            int rowCount = 0;

            rowManaRegen = rowCount++;
            rowFightDuration = rowCount++;
            if (calculationOptions.EvocationEnabled) rowEvocation = rowCount++;
            if (calculationOptions.ManaPotionEnabled) rowPotion = rowCount++;
            if (calculationOptions.ManaGemEnabled) rowManaGem = rowCount++;
            if (heroismAvailable) rowHeroism = rowCount++;
            if (arcanePowerAvailable) rowArcanePower = rowCount++;
            if (heroismAvailable && arcanePowerAvailable) rowHeroismArcanePower = rowCount++;
            if (icyVeinsAvailable) rowIcyVeins = rowCount++;
            if (moltenFuryAvailable) rowMoltenFury = rowCount++;
            if (moltenFuryAvailable && destructionPotionAvailable) rowMoltenFuryDestructionPotion = rowCount++;
            if (moltenFuryAvailable && icyVeinsAvailable) rowMoltenFuryIcyVeins = rowCount++;
            if (heroismAvailable && destructionPotionAvailable) rowHeroismDestructionPotion = rowCount++;
            if (icyVeinsAvailable && destructionPotionAvailable) rowIcyVeinsDestructionPotion = rowCount++;
            if (flameCapAvailable) rowManaGemFlameCap = rowCount++;
            if (moltenFuryAvailable && flameCapAvailable) rowMoltenFuryFlameCap = rowCount++;
            if (flameCapAvailable && destructionPotionAvailable) rowFlameCapDestructionPotion = rowCount++;
            if (trinket1Available) rowTrinket1 = rowCount++;
            if (trinket2Available) rowTrinket2 = rowCount++;
            if (moltenFuryAvailable && trinket1Available) rowMoltenFuryTrinket1 = rowCount++;
            if (moltenFuryAvailable && trinket2Available) rowMoltenFuryTrinket2 = rowCount++;
            if (heroismAvailable && trinket1Available) rowHeroismTrinket1 = rowCount++;
            if (heroismAvailable && trinket2Available) rowHeroismTrinket2 = rowCount++;
            if (trinket1OnManaGem || trinket2OnManaGem) rowTrinketManaGem = rowCount++;
            if (calculationOptions.AoeDuration > 0)
            {
                rowAoe = rowCount++;
                rowFlamestrike = rowCount++;
                rowConeOfCold = rowCount++;
                if (calculationOptions.BlastWave == 1) rowBlastWave = rowCount++;
                if (calculationOptions.DragonsBreath == 1) rowDragonsBreath = rowCount++;
            }
            if (combustionAvailable) rowCombustion = rowCount++;
            if (combustionAvailable && moltenFuryAvailable) rowMoltenFuryCombustion = rowCount++;
            if (combustionAvailable && heroismAvailable) rowHeroismCombustion = rowCount++;
            if (icyVeinsAvailable && heroismAvailable) rowHeroismIcyVeins = rowCount++;
            if (drumsOfBattleAvailable) rowDrumsOfBattleActivation = rowCount++;
            if (drumsOfBattleAvailable && moltenFuryAvailable) rowMoltenFuryDrumsOfBattle = rowCount++;
            if (drumsOfBattleAvailable && heroismAvailable) rowHeroismDrumsOfBattle = rowCount++;
            if (drumsOfBattleAvailable && icyVeinsAvailable) rowIcyVeinsDrumsOfBattle = rowCount++;
            if (drumsOfBattleAvailable && arcanePowerAvailable) rowArcanePowerDrumsOfBattle = rowCount++;
            if (calculationOptions.TpsLimit < 5000f && calculationOptions.TpsLimit > 0f) rowThreat = rowCount++;
            if (drumsOfBattleAvailable) rowDrumsOfBattle = rowCount++;
            if (drinkingEnabled) rowDrinking = rowCount++;
            if (needsTimeExtension) rowTimeExtension = rowCount++;
            if (afterFightRegen) rowAfterFightRegenMana = rowCount++;
            //if (afterFightRegen) rowAfterFightRegenHealth = rowCount++;
            if (minimizeTime) rowTargetDamage = rowCount++;
            rowDpsTime = rowCount++;
            rowManaPotionManaGem = rowCount++;
            if (segmentCooldowns)
            {
                // mf, heroism, ap, iv, combustion, drums, flamecap, destruction, t1, t2
                // mf
                if (moltenFuryAvailable)
                {
                    bool firstSet = false;
                    double mflength = calculationOptions.MoltenFuryPercentage * calculationOptions.FightDuration;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        if ((seg + 1) * segmentDuration > calculationOptions.FightDuration - mflength)
                        {
                            if (!firstSet)
                            {
                                rowSegmentMoltenFury = rowCount++ - seg;
                                firstSet = true;
                            }
                            else
                            {
                                rowCount++;
                            }
                        }
                    }
                }
                // heroism
                // ap
                if (arcanePowerAvailable)
                {
                    rowSegmentArcanePower = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 180;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // iv
                if (icyVeinsAvailable)
                {
                    rowSegmentIcyVeins = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 180 + (coldsnapAvailable ? 20 : 0);
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // combustion
                if (combustionAvailable)
                {
                    rowSegmentCombustion = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 180 + 15;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // drums
                if (drumsOfBattleAvailable)
                {
                    rowSegmentDrumsOfBattle = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    rowSegmentDrumsOfBattleActivation = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // flamecap & mana gem
                if (flameCapAvailable)
                {
                    rowSegmentFlameCap = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 180;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    rowSegmentManaGem = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // destruction
                if (destructionPotionAvailable)
                {
                    rowSegmentPotion = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 120;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // t1
                if (trinket1Available)
                {
                    rowSegmentTrinket1 = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = trinket1Cooldown;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // t2
                if (trinket2Available)
                {
                    rowSegmentTrinket2 = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = trinket2Cooldown;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // max segment time
                rowSegment = rowCount;
                rowCount += segments;
                // mana overflow & underflow (don't need over all segments, that is already verified)
                if (restrictManaUse)
                {
                    rowSegmentManaOverflow = rowCount;
                    rowCount += segments - 1;
                    rowSegmentManaUnderflow = rowCount;
                    rowCount += segments - 1;
                }
            }
            return rowCount;
        }

        private void SetSpellColumn(bool minimizeTime, List<double> tpsList, int segment, CastingState state, int column, Spell spell)
        {
            double manaRegen = spell.CostPerSecond - spell.ManaRegenPerSecond;
            lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
            lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
            lp.SetElementUnsafe(rowFightDuration, column, 1.0);
            lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
            if (state.DestructionPotion) lp.SetElementUnsafe(rowPotion, column, 1.0 / 15.0);
            if (state.Heroism) lp.SetElementUnsafe(rowHeroism, column, 1.0);
            if (state.ArcanePower) lp.SetElementUnsafe(rowArcanePower, column, 1.0);
            if (state.Heroism && state.ArcanePower) lp.SetElementUnsafe(rowHeroismArcanePower, column, 1.0);
            if (state.IcyVeins) lp.SetElementUnsafe(rowIcyVeins, column, 1.0);
            if (state.MoltenFury) lp.SetElementUnsafe(rowMoltenFury, column, 1.0);
            if (state.MoltenFury && state.DestructionPotion) lp.SetElementUnsafe(rowMoltenFuryDestructionPotion, column, 1.0);
            if (state.MoltenFury && state.IcyVeins) lp.SetElementUnsafe(rowMoltenFuryIcyVeins, column, 1.0);
            if (state.DestructionPotion && state.Heroism) lp.SetElementUnsafe(rowHeroismDestructionPotion, column, 1.0);
            if (state.DestructionPotion && state.IcyVeins) lp.SetElementUnsafe(rowIcyVeinsDestructionPotion, column, 1.0);
            if (state.FlameCap) lp.SetElementUnsafe(rowManaGemFlameCap, column, 1.0 / 40.0);
            if (state.MoltenFury && state.FlameCap) lp.SetElementUnsafe(rowMoltenFuryFlameCap, column, 1.0);
            if (state.DestructionPotion && state.FlameCap) lp.SetElementUnsafe(rowFlameCapDestructionPotion, column, 1.0);
            if (state.Trinket1) lp.SetElementUnsafe(rowTrinket1, column, 1.0);
            if (state.Trinket2) lp.SetElementUnsafe(rowTrinket2, column, 1.0);
            if (state.MoltenFury && state.Trinket1) lp.SetElementUnsafe(rowMoltenFuryTrinket1, column, 1.0);
            if (state.MoltenFury && state.Trinket2) lp.SetElementUnsafe(rowMoltenFuryTrinket2, column, 1.0);
            if (state.Heroism && state.Trinket1) lp.SetElementUnsafe(rowHeroismTrinket1, column, 1.0);
            if (state.Heroism && state.Trinket2) lp.SetElementUnsafe(rowHeroismTrinket2, column, 1.0);
            lp.SetElementUnsafe(rowTrinketManaGem, column, ((state.Trinket1 && trinket1OnManaGem) ? 1 / trinket1Duration : 0) + ((state.Trinket2 && trinket2OnManaGem) ? 1 / trinket2Duration : 0));
            if (spell.AreaEffect) lp.SetElementUnsafe(rowAoe, column, 1.0);
            if (spell.AreaEffect)
            {
                Flamestrike fs = spell as Flamestrike;
                if (fs != null)
                {
                    if (!fs.SpammedDot) lp.SetElementUnsafe(rowFlamestrike, column, fs.DotDuration / fs.CastTime);
                }
                else
                {
                    lp.SetElementUnsafe(rowFlamestrike, column, -1.0);
                }
                ConeOfCold coc = spell as ConeOfCold;
                if (coc != null)
                {
                    lp.SetElementUnsafe(rowConeOfCold, column, (coc.Cooldown / coc.CastTime - 1.0));
                }
                else
                {
                    lp.SetElementUnsafe(rowConeOfCold, column, -1.0);
                }
                BlastWave bw = spell as BlastWave;
                if (bw != null)
                {
                    lp.SetElementUnsafe(rowBlastWave, column, (bw.Cooldown / bw.CastTime - 1.0));
                }
                else
                {
                    lp.SetElementUnsafe(rowBlastWave, column, -1);
                }
                DragonsBreath db = spell as DragonsBreath;
                if (db != null)
                {
                    lp.SetElementUnsafe(rowDragonsBreath, column, (db.Cooldown / db.CastTime - 1.0));
                }
                else
                {
                    lp.SetElementUnsafe(rowDragonsBreath, column, -1.0);
                }
            }
            if (state.Combustion) lp.SetElementUnsafe(rowCombustion, column, (1 / (state.CombustionDuration * spell.CastTime / spell.CastProcs)));
            if (state.Combustion && state.MoltenFury) lp.SetElementUnsafe(rowMoltenFuryCombustion, column, (1 / (state.CombustionDuration * spell.CastTime / spell.CastProcs)));
            if (state.Combustion && state.Heroism) lp.SetElementUnsafe(rowHeroismCombustion, column, (1 / (state.CombustionDuration * spell.CastTime / spell.CastProcs)));
            if (state.IcyVeins && state.Heroism) lp.SetElementUnsafe(rowHeroismIcyVeins, column, 1.0);
            if (state.DrumsOfBattle) lp.SetElementUnsafe(rowDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle) lp.SetElementUnsafe(rowDrumsOfBattleActivation, column, 1 / (30 - calculationResult.BaseState.GlobalCooldown));
            if (state.DrumsOfBattle && state.MoltenFury) lp.SetElementUnsafe(rowMoltenFuryDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.Heroism) lp.SetElementUnsafe(rowHeroismDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.IcyVeins) lp.SetElementUnsafe(rowIcyVeinsDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.ArcanePower) lp.SetElementUnsafe(rowArcanePowerDrumsOfBattle, column, 1.0);
            lp.SetElementUnsafe(rowThreat, column, spell.ThreatPerSecond);
            tpsList.Add(spell.ThreatPerSecond);
            //lp[rowManaPotionManaGem, index] = (statsList[buffset].FlameCap ? 1 : 0) + (statsList[buffset].DestructionPotion ? 40.0 / 15.0 : 0);
            lp.SetElementUnsafe(rowTargetDamage, column, -spell.DamagePerSecond);
            lp.SetCostUnsafe(column, minimizeTime ? -1 : spell.DamagePerSecond);
            if (segmentCooldowns)
            {
                // mf, heroism, ap, iv, combustion, drums, flamecap, destro, t1, t2
                if (state.MoltenFury)
                {
                    lp.SetElementUnsafe(rowSegmentMoltenFury + segment, column, 1.0);
                }
                //lp[rowOffset + 1 * segments + seg, index] = 1;
                if (state.ArcanePower)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 180;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentArcanePower + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.IcyVeins)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 180 + (coldsnapAvailable ? 20 : 0);
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentIcyVeins + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.Combustion)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 180 + 15;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentCombustion + ss, column, 1.0 / (state.CombustionDuration * spell.CastTime / spell.CastProcs));
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.DrumsOfBattle)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 120;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentDrumsOfBattle + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.FlameCap)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 180;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentFlameCap + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 120;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentManaGem + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.DestructionPotion)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 120;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentPotion + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.Trinket1)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = trinket1Cooldown;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentTrinket1 + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.Trinket2)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = trinket2Cooldown;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentTrinket2 + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (segmentNonCooldowns || state != calculationResult.BaseState) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
            }
            if (restrictManaUse)
            {
                for (int ss = segment; ss < segments - 1; ss++)
                {
                    lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                    lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                }
            }
        }

        private List<SpellId> GetSpellList()
        {
            List<SpellId> list = new List<SpellId>();

            if (calculationOptions.CustomSpellMixEnabled || calculationOptions.CustomSpellMixOnly)
            {
                list.Add(SpellId.CustomSpellMix);
            }
            if (!calculationOptions.CustomSpellMixOnly)
            {
                if (calculationOptions.SmartOptimization)
                {
                    if (calculationOptions.EmpoweredFireball > 0)
                    {
                        list.Add(calculationOptions.MaintainScorch ? SpellId.FireballScorch : SpellId.Fireball);
                    }
                    else if (calculationOptions.EmpoweredFrostbolt > 0)
                    {
                        list.Add(SpellId.Frostbolt);
                    }
                    else if (calculationOptions.SpellPower > 0)
                    {
                        list.Add(SpellId.ArcaneBlast33);
                        if (calculationOptions.ImprovedFrostbolt > 0) list.Add(SpellId.Frostbolt);
                        if (calculationOptions.ImprovedFireball > 0) list.Add(calculationOptions.MaintainScorch ? SpellId.FireballScorch : SpellId.Fireball);
                        if (calculationOptions.ImprovedArcaneMissiles + calculationOptions.EmpoweredArcaneMissiles > 0) list.Add(SpellId.ArcaneMissiles);
                    }
                    else
                    {
                        list.Add(SpellId.ArcaneMissiles);
                        list.Add(SpellId.Scorch);
                        list.Add(calculationOptions.MaintainScorch ? SpellId.FireballScorch : SpellId.Fireball);
                        list.Add(SpellId.Frostbolt);
                        list.Add(SpellId.ArcaneBlast33);
                    }
                }
                else
                {
                    list.Add(SpellId.ArcaneMissiles);
                    list.Add(SpellId.Scorch);
                    list.Add(calculationOptions.MaintainScorch ? SpellId.FireballScorch : SpellId.Fireball);
                    list.Add(SpellId.FireballFireBlast);
                    list.Add(SpellId.Frostbolt);
                    list.Add(SpellId.ArcaneBlast33);
                }
                if (calculationOptions.ABCycles)
                {
                    if (calculationOptions.EmpoweredArcaneMissiles > 0)
                    {
                        list.Add(SpellId.ABAMP);
                        list.Add(SpellId.ABAM);
                        list.Add(SpellId.AB3AMSc);
                        list.Add(SpellId.ABAM3Sc);
                        list.Add(SpellId.ABAM3Sc2);
                        list.Add(SpellId.ABAM3FrB);
                        list.Add(SpellId.ABAM3FrB2);
                        list.Add(SpellId.ABAM3ScCCAM);
                        list.Add(SpellId.ABAM3Sc2CCAM);
                        list.Add(SpellId.ABAM3FrBCCAM);
                        list.Add(SpellId.ABAM3FrBScCCAM);
                        list.Add(SpellId.ABAMCCAM);
                        list.Add(SpellId.ABAM3CCAM);
                    }
                    if (calculationOptions.ImprovedFrostbolt > 0)
                    {
                        list.Add(SpellId.ABFrB3FrB);
                        list.Add(SpellId.ABFrB3FrBSc);
                    }
                    if (calculationOptions.ImprovedFireball > 0)
                    {
                        list.Add(SpellId.ABFB3FBSc);
                        //list.Add(SpellId.AB3Sc);
                    }
                }
                if (calculationOptions.AoeDuration > 0)
                {
                    list.Add(SpellId.ArcaneExplosion);
                    list.Add(SpellId.FlamestrikeSpammed);
                    list.Add(SpellId.FlamestrikeSingle);
                    list.Add(SpellId.Blizzard);
                    list.Add(SpellId.ConeOfCold);
                    if (calculationOptions.BlastWave == 1) list.Add(SpellId.BlastWave);
                    if (calculationOptions.DragonsBreath == 1) list.Add(SpellId.DragonsBreath);
                }
            }
            return list;
        }

        private List<CastingState> GetStateList(Stats characterStats)
        {
            List<CastingState> list = new List<CastingState>();

            int availableMask = 0;
            bool canDoubleTrinket = trinket1OnManaGem || trinket2OnManaGem;
            if (moltenFuryAvailable) availableMask |= 0x80;
            if (heroismAvailable) availableMask |= 0x40;
            if (arcanePowerAvailable) availableMask |= 0x20;
            if (icyVeinsAvailable) availableMask |= 0x10;
            if (combustionAvailable) availableMask |= 0x8;
            if (drumsOfBattleAvailable) availableMask |= 0x4;
            if (flameCapAvailable) availableMask |= 0x2;
            if (destructionPotionAvailable) availableMask |= 0x1;
            if (calculationOptions.IncrementalOptimizations)
            {
                for (int incrementalSortedIndex = 0; incrementalSortedIndex < calculationOptions.IncrementalSetSortedStates.Length; incrementalSortedIndex++)
                {
                    int incrementalSetIndex = calculationOptions.IncrementalSetSortedStates[incrementalSortedIndex];
                    bool mf = (incrementalSetIndex & 0x80) == 0;
                    bool heroism = (incrementalSetIndex & 0x40) == 0;
                    bool ap = (incrementalSetIndex & 0x20) == 0;
                    bool iv = (incrementalSetIndex & 0x10) == 0;
                    bool combustion = (incrementalSetIndex & 0x8) == 0;
                    bool drums = (incrementalSetIndex & 0x4) == 0;
                    bool flameCap = (incrementalSetIndex & 0x2) == 0;
                    bool destructionPotion = (incrementalSetIndex & 0x1) == 0;
                    for (int t1 = 0; t1 < 2; t1++)
                        for (int t2 = 0; t2 < 2; t2++)
                        {
                            bool trinket1 = t1 == 0;
                            bool trinket2 = t2 == 0;
                            if ((availableMask | incrementalSetIndex) == 0xFF && (trinket1Available || !trinket1) && (trinket2Available || !trinket2))
                            {
                                if (!trinket1 || !trinket2 || canDoubleTrinket) // only leave through trinkets that can stack
                                {
                                    if ((!trinket1 || !trinket1OnManaGem || !flameCap) && (!trinket2 || !trinket2OnManaGem || !flameCap)) // do not allow SCB together with flame cap
                                    {
                                        if ((calculationOptions.HeroismControl != 1 || !heroism || !mf) && (calculationOptions.HeroismControl != 2 || !heroism || (incrementalSetIndex == 0xFF && !trinket1 && !trinket2)))
                                        {
                                            list.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, ap, mf, iv, heroism, destructionPotion, flameCap, trinket1, trinket2, combustion, drums, incrementalSetIndex));
                                            if (incrementalSetIndex == 0xFF && !trinket1 && !trinket2)
                                            {
                                                calculationResult.BaseState = list[list.Count - 1];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                }
                if (calculationResult.BaseState == null) calculationResult.BaseState = new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, false, false, false, false, false, false, false, false, false, 0xFF);
            }
            else
            {
                for (int incrementalSetIndex = 0; incrementalSetIndex < 0x100; incrementalSetIndex++)
                {
                    bool mf = (incrementalSetIndex & 0x80) == 0;
                    bool heroism = (incrementalSetIndex & 0x40) == 0;
                    bool ap = (incrementalSetIndex & 0x20) == 0;
                    bool iv = (incrementalSetIndex & 0x10) == 0;
                    bool combustion = (incrementalSetIndex & 0x8) == 0;
                    bool drums = (incrementalSetIndex & 0x4) == 0;
                    bool flameCap = (incrementalSetIndex & 0x2) == 0;
                    bool destructionPotion = (incrementalSetIndex & 0x1) == 0;
                    for (int t1 = 0; t1 < 2; t1++)
                        for (int t2 = 0; t2 < 2; t2++)
                        {
                            bool trinket1 = t1 == 0;
                            bool trinket2 = t2 == 0;
                            if ((availableMask | incrementalSetIndex) == 0xFF && (trinket1Available || !trinket1) && (trinket2Available || !trinket2))
                            {
                                if (!trinket1 || !trinket2 || canDoubleTrinket) // only leave through trinkets that can stack
                                {
                                    if ((!trinket1 || !trinket1OnManaGem || !flameCap) && (!trinket2 || !trinket2OnManaGem || !flameCap)) // do not allow SCB together with flame cap
                                    {
                                        if ((calculationOptions.HeroismControl != 1 || !heroism || !mf) && (calculationOptions.HeroismControl != 2 || !heroism || (incrementalSetIndex == 0xFF && !trinket1 && !trinket2)))
                                        {
                                            list.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, ap, mf, iv, heroism, destructionPotion, flameCap, trinket1, trinket2, combustion, drums, incrementalSetIndex));
                                            if (incrementalSetIndex == 0xFF && !trinket1 && !trinket2)
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
