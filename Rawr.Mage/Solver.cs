using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Mage
{
    public sealed partial class Solver
    {
        private const int cooldownCount = 12;
        private const double segmentDuration = 30;
        private int segments;

        private List<CastingState> stateList;
        private List<SpellId> spellList;

        private SolverLP lp;
        private double[] solution;
        private int[] segmentColumn;
        private CharacterCalculationsMage calculationResult;
        private Character character;
        private CalculationOptionsMage calculationOptions;
        private string armor;

        private bool restrictThreat;

        private bool segmentCooldowns;
        private bool segmentNonCooldowns;
        private bool integralMana;
        private bool requiresMIP;

        private bool minimizeTime;
        private bool restrictManaUse;
        private bool needsTimeExtension;
        private bool conjureManaGem;

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

        private double manaGemEffectDuration;
        private double trinket1Cooldown;
        private double trinket1Duration;
        private double trinket2Cooldown;
        private double trinket2Duration;

        #region LP rows
        private int rowManaRegen = -1;
        private int rowFightDuration = -1;
        private int rowEvocation = -1;
        private int rowPotion = -1;
        private int rowManaPotion = -1;
        private int rowConjureManaGem = -1;
        private int rowManaGem = -1;
        private int rowManaGemMax = -1;
        private int rowHeroism = -1;
        private int rowArcanePower = -1;
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
        private int rowManaPotionManaGem = -1;
        private int rowDrumsOfBattle = -1;
        private int rowTimeExtension = -1;
        private int rowAfterFightRegenMana = -1;
        private int rowAfterFightRegenHealth = -1;
        private int rowTargetDamage = -1;
        private int rowSegmentMoltenFury = -1;
        private int rowSegmentHeroism = -1;
        private int rowSegmentArcanePower = -1;
        private int rowSegmentIcyVeins = -1;
        private int rowSegmentWaterElemental = -1;
        private int rowSegmentSummonWaterElemental = -1;
        private int rowSegmentCombustion = -1;
        private int rowSegmentDrumsOfBattle = -1;
        private int rowSegmentDrumsOfBattleActivation = -1;
        private int rowSegmentFlameCap = -1;
        private int rowSegmentManaGem = -1;
        //private int rowSegmentPotion = -1;
        private int rowSegmentTrinket1 = -1;
        private int rowSegmentTrinket2 = -1;
        private int rowSegmentManaGemEffect = -1;
        private int rowSegment = -1;
        private int rowSegmentManaOverflow = -1;
        private int rowSegmentManaUnderflow = -1;
        private int rowSegmentThreat = -1;
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
            return (item.Stats.SpellPowerFor15SecOnUse2Min + item.Stats.SpellPowerFor20SecOnUse2Min + item.Stats.HasteRatingFor20SecOnUse2Min + item.Stats.Mp5OnCastFor20SecOnUse2Min + item.Stats.SpellPowerFor15SecOnUse90Sec + item.Stats.HasteRatingFor20SecOnUse5Min + item.Stats.SpellPowerFor20SecOnUse5Min > 0);
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

                //if (useSMP) calculationOptions.SmartOptimization = true;
                segments = (segmentCooldowns) ? (int)Math.Ceiling(calculationOptions.FightDuration / segmentDuration) : 1;
                segmentColumn = new int[segments + 1];

                calculationResult = new CharacterCalculationsMage();
                calculationResult.Calculations = calculations;
                calculationResult.BaseStats = characterStats;
                calculationResult.Character = character;
                calculationResult.CalculationOptions = calculationOptions;

                restrictThreat = segmentCooldowns && calculationOptions.TpsLimit != 5000f && calculationOptions.TpsLimit > 0f;
                heroismAvailable = calculationOptions.HeroismAvailable;
                arcanePowerAvailable = !calculationOptions.DisableCooldowns && (character.MageTalents.ArcanePower == 1);
                icyVeinsAvailable = !calculationOptions.DisableCooldowns && (character.MageTalents.IcyVeins == 1);
                combustionAvailable = !calculationOptions.DisableCooldowns && (character.MageTalents.Combustion == 1);
                moltenFuryAvailable = character.MageTalents.MoltenFury > 0;
                trinket1Available = !calculationOptions.DisableCooldowns && IsItemActivatable(character.Trinket1);
                trinket2Available = !calculationOptions.DisableCooldowns && IsItemActivatable(character.Trinket2);
                coldsnapAvailable = !calculationOptions.DisableCooldowns && (character.MageTalents.ColdSnap == 1);
                potionOfWildMagicAvailable = !calculationOptions.DisableCooldowns && calculationOptions.PotionOfWildMagic;
                potionOfSpeedAvailable = !calculationOptions.DisableCooldowns && calculationOptions.PotionOfSpeed;
                effectPotionAvailable = potionOfWildMagicAvailable || potionOfSpeedAvailable;
                flameCapAvailable = !calculationOptions.DisableCooldowns && calculationOptions.FlameCap;
                drumsOfBattleAvailable = !calculationOptions.DisableCooldowns && calculationOptions.DrumsOfBattle;
                waterElementalAvailable = !calculationOptions.DisableCooldowns && (character.MageTalents.SummonWaterElemental == 1);
                manaGemEffectAvailable = calculationOptions.ManaGemEnabled && characterStats.SpellPowerFor15SecOnManaGem > 0;
                calculationResult.EvocationCooldown = calculationOptions.Mode308 ? (240.0 - 60.0 * character.MageTalents.ArcaneFlows) : 300.0;
                calculationResult.ColdsnapCooldown = (8 * 60) * (1 - 0.1 * character.MageTalents.ColdAsIce);
                calculationResult.ArcanePowerCooldown = 180.0 - 30.0 * character.MageTalents.ArcaneFlows;
                calculationResult.ArcanePowerDuration = 15.0 + (calculationOptions.GlyphOfArcanePower ? 3.0 : 0.0);
                calculationResult.IcyVeinsCooldown = 180.0 * (1 - 0.07 * character.MageTalents.IceFloes + (character.MageTalents.IceFloes == 3 ? 0.01 : 0.00));
                calculationResult.WaterElementalCooldown = (180.0 - (calculationOptions.GlyphOfWaterElemental ? 30.0 : 0.0)) * (1 - 0.1 * character.MageTalents.ColdAsIce);
                calculationResult.WaterElementalDuration = 45.0 + 5.0 * character.MageTalents.ImprovedWaterElemental;
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

                #region Setup Trinkets
                if (trinket1Available)
                {
                    Stats s = character.Trinket1.Stats;
                    if (s.SpellPowerFor20SecOnUse2Min + s.HasteRatingFor20SecOnUse2Min + s.Mp5OnCastFor20SecOnUse2Min > 0)
                    {
                        trinket1Duration = 20;
                        trinket1Cooldown = 120;
                    }
                    if (s.SpellPowerFor15SecOnUse90Sec > 0)
                    {
                        trinket1Duration = 15;
                        trinket1Cooldown = 90;
                    }
                    if (s.HasteRatingFor20SecOnUse5Min + s.SpellPowerFor20SecOnUse5Min > 0)
                    {
                        trinket1Duration = 20;
                        trinket1Cooldown = 300;
                    }
                    if (s.SpellPowerFor15SecOnUse2Min > 0)
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
                    if (s.SpellPowerFor20SecOnUse2Min + s.HasteRatingFor20SecOnUse2Min + s.Mp5OnCastFor20SecOnUse2Min > 0)
                    {
                        trinket2Duration = 20;
                        trinket2Cooldown = 120;
                    }
                    if (s.SpellPowerFor15SecOnUse90Sec > 0)
                    {
                        trinket2Duration = 15;
                        trinket2Cooldown = 90;
                    }
                    if (s.HasteRatingFor20SecOnUse5Min + s.SpellPowerFor20SecOnUse5Min > 0)
                    {
                        trinket2Duration = 20;
                        trinket2Cooldown = 300;
                    }
                    if (s.SpellPowerFor15SecOnUse2Min > 0)
                    {
                        trinket2Duration = 15;
                        trinket2Cooldown = 120;
                    }
                    calculationResult.Trinket2Duration = trinket2Duration;
                    calculationResult.Trinket2Cooldown = trinket2Cooldown;
                    calculationResult.Trinket2Name = character.Trinket2.Name;
                }
                if (manaGemEffectAvailable)
                {
                    if (characterStats.SpellPowerFor15SecOnManaGem > 0)
                    {
                        manaGemEffectDuration = 15;
                    }
                    calculationResult.ManaGemEffectDuration = manaGemEffectDuration;
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
                    calculationResult.SubPoints[0] = ((float)calculationResult.Solution[calculationResult.Solution.Length - 1] /*+ calculationResult.WaterElementalDamage*/) / calculationOptions.FightDuration;
                }
                calculationResult.SubPoints[1] = EvaluateSurvivability(characterStats);
                calculationResult.OverallPoints = calculationResult.SubPoints[0] + calculationResult.SubPoints[1];

                float threat = 0;
                for (int i = 0; i < tpsList.Count; i++)
                {
                    threat += (float)(tpsList[i] * calculationResult.Solution[i]);
                }
                calculationResult.Tps = threat / calculationOptions.FightDuration;

                return calculationResult;
            }
        }

        private float EvaluateSurvivability(Stats characterStats)
        {
            double ampMelee = (1 - 0.02 * character.MageTalents.PrismaticCloak) * (1 - 0.01 * character.MageTalents.ArcticWinds) * (1 - calculationResult.BaseState.MeleeMitigation) * (1 - calculationResult.BaseState.Dodge);
            double ampPhysical = (1 - 0.02 * character.MageTalents.PrismaticCloak) * (1 - 0.01 * character.MageTalents.ArcticWinds) * (1 - calculationResult.BaseState.MeleeMitigation);
            double ampArcane = (1 - 0.02 * character.MageTalents.PrismaticCloak) * (1 + 0.01 * character.MageTalents.PlayingWithFire) * (1 - 0.02 * character.MageTalents.FrozenCore) * Math.Max(1 - characterStats.ArcaneResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampFire = (1 - 0.02 * character.MageTalents.PrismaticCloak) * (1 + 0.01 * character.MageTalents.PlayingWithFire) * (1 - 0.02 * character.MageTalents.FrozenCore) * Math.Max(1 - characterStats.FireResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampFrost = (1 - 0.02 * character.MageTalents.PrismaticCloak) * (1 + 0.01 * character.MageTalents.PlayingWithFire) * (1 - 0.02 * character.MageTalents.FrozenCore) * Math.Max(1 - characterStats.FrostResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampNature = (1 - 0.02 * character.MageTalents.PrismaticCloak) * (1 + 0.01 * character.MageTalents.PlayingWithFire) * (1 - 0.02 * character.MageTalents.FrozenCore) * Math.Max(1 - characterStats.NatureResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampShadow = (1 - 0.02 * character.MageTalents.PrismaticCloak) * (1 + 0.01 * character.MageTalents.PlayingWithFire) * (1 - 0.02 * character.MageTalents.FrozenCore) * Math.Max(1 - characterStats.ShadowResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);
            double ampHoly = (1 - 0.02 * character.MageTalents.PrismaticCloak) * (1 + 0.01 * character.MageTalents.PlayingWithFire) * (1 - 0.02 * character.MageTalents.FrozenCore);

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

            needsTimeExtension = false;
            bool afterFightRegen = calculationOptions.FarmingMode;
            conjureManaGem = calculationOptions.ManaGemEnabled && calculationOptions.FightDuration > 500.0f;

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

            lp = new SolverLP(rowCount, 9 + (10 + spellList.Count * stateList.Count) * segments, calculationResult, segments);
            tpsList = new List<double>();
            double tps;
            calculationResult.SolutionVariable = new List<SolutionVariable>();

            /*if (segmentCooldowns)
            {
                double maxdps = 0;
                double mindpm = double.PositiveInfinity;
                double maxdpm = 0;
                double[] spellControl = new double[34];
                for (int control0 = 0; control0 < 4; control0++)
                for (int control1 = 0; control1 < 4; control1++)
                for (int control2 = 0; control2 < 3; control2++)
                for (int control3 = 0; control3 < 3; control3++)
                for (int control4 = 0; control4 < 4; control4++)
                for (int control5 = 0; control5 < 4; control5++)
                for (int control6 = 0; control6 < 4; control6++)
                for (int control7 = 0; control7 < 4; control7++)
                for (int control8 = 0; control8 < 4; control8++)
                {
                    for (int i = 0; i < 34; i++) spellControl[i] = 0;
                    spellControl[0 + control0] = 1;
                    spellControl[4 + control1] = 1;
                    spellControl[8 + control2] = 1;
                    spellControl[11 + control3] = 1;
                    spellControl[14 + control4] = 1;
                    spellControl[18 + control5] = 1;
                    spellControl[22 + control6] = 1;
                    spellControl[26 + control7] = 1;
                    spellControl[30 + control8] = 1;
                    Spell generic = new GenericArcane(calculationResult.BaseState, spellControl[0], spellControl[1], spellControl[2], spellControl[3], spellControl[4], spellControl[5], spellControl[6], spellControl[7], spellControl[8], spellControl[9], spellControl[10], spellControl[11], spellControl[12], spellControl[13], spellControl[14], spellControl[15], spellControl[16], spellControl[17], spellControl[18], spellControl[19], spellControl[20], spellControl[21], spellControl[22], spellControl[23], spellControl[24], spellControl[25], spellControl[26], spellControl[27], spellControl[28], spellControl[29], spellControl[30], spellControl[31], spellControl[32], spellControl[33]);
                    // (cycledps - abdps) / (cyclemps - abmps)
                    double dpm = (generic.DamagePerSecond - 4852.069) / (generic.ManaPerSecond - 250.269913);
                    if (generic.DamagePerSecond > maxdps)
                    {
                        maxdps = generic.DamagePerSecond;
                    }
                    if (dpm > 0 && dpm < mindpm && generic.ManaPerSecond < 300)
                    {
                        mindpm = dpm;
                    }
                    if (generic.DamagePerSecond > 4852.069 && dpm > maxdpm)
                    {
                        maxdpm = dpm;
                    }
                }
            }*/

            fixed (double* pRowScale = SolverLP.rowScale, pColumnScale = SolverLP.columnScale, pCost = LP._cost, pData = SparseMatrix.data, pValue = SparseMatrix.value)
            fixed (int* pRow = SparseMatrix.row, pCol = SparseMatrix.col)
            {
                lp.BeginUnsafe(pRowScale, pColumnScale, pCost, pData, pValue, pRow, pCol);

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
                    for (int ss = 0; ss < segments - 1; ss++)
                    {
                        lp.SetRowScaleUnsafe(rowSegmentManaOverflow + ss, 0.1);
                        lp.SetRowScaleUnsafe(rowSegmentManaUnderflow + ss, 0.1);
                    }
                }
                if (restrictThreat)
                {
                    for (int ss = 0; ss < segments - 1; ss++)
                    {
                        lp.SetRowScaleUnsafe(rowSegmentThreat + ss, 0.001);
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
                    lp.SetColumnUpperBound(column, (idleRegenSegments > 1) ? segmentDuration : calculationOptions.FightDuration);
                    if (segment == 0) calculationResult.ColumnIdleRegen = column;
                    calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.IdleRegen, Segment = segment, State = calculationResult.BaseState });
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
                        lp.SetColumnUpperBound(column, (wandSegments > 1) ? segmentDuration : calculationOptions.FightDuration);
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
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
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
                    calculationResult.MaxEvocation = (int)Math.Max(1, (1 + Math.Floor((calculationOptions.FightDuration - 120f) / calculationResult.EvocationCooldown)));
                    for (int segment = 0; segment < evocationSegments; segment++)
                    {
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.Evocation, Segment = segment, State = calculationResult.BaseState });
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
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                    }
                }
                // mana potion
                calculationResult.MaxManaPotion = 1;
                if (calculationOptions.ManaPotionEnabled)
                {
                    int manaPotionSegments = (segmentCooldowns && (potionOfWildMagicAvailable || restrictManaUse)) ? segments : 1;
                    manaRegen = -(1 + characterStats.BonusManaPotion) * calculationResult.ManaPotionValue;
                    for (int segment = 0; segment < manaPotionSegments; segment++)
                    {
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.ManaPotion, Segment = segment });
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnScaleUnsafe(column, 1.0 / 40.0);
                        lp.SetColumnUpperBound(column, (manaPotionSegments > 1) ? 1.0 : calculationResult.MaxManaPotion);
                        if (segment == 0) calculationResult.ColumnManaPotion = column;
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                        lp.SetElementUnsafe(rowPotion, column, 1.0);
                        lp.SetElementUnsafe(rowManaPotion, column, 1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = (1 + characterStats.BonusManaPotion) * calculationResult.ManaPotionValue * 0.5f * threatFactor);
                        calculationResult.ManaPotionTps = tps;
                        lp.SetElementUnsafe(rowManaPotionManaGem, column, 40.0);
                        lp.SetCostUnsafe(column, 0.0);
                        tpsList.Add(tps);
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
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                    }
                }
                // mana gem
                if (calculationOptions.ManaGemEnabled)
                {
                    int manaGemSegments = (segmentCooldowns && (flameCapAvailable || restrictManaUse)) ? segments : 1;
                    calculationResult.MaxManaGem = 1 + (int)((calculationOptions.FightDuration - 30f) / 120f);
                    double manaGemRegen = -(1 + characterStats.BonusManaGem) * calculationResult.ManaGemValue;
                    for (int segment = 0; segment < manaGemSegments; segment++)
                    {
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.ManaGem, Segment = segment });
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
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaGemRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaGemRegen);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
                        }
                    }
                }
                // drums
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
                        drumsStates.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, false, false, false, false, false, false, false, false, false, true, false, false));
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
                            drumsStates.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, false, false, false, false, false, true, false, false, false, true, false, false));
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
                            mfDrumsStates.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, true, false, false, false, false, false, false, false, false, true, false, false));
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
                                mfDrumsStates.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, true, false, false, false, false, true, false, false, false, true, false, false));
                            }
                        }
                    }

                    int drumsOfBattleSegments = segments; // always segment, we need it to guarantee each block has activation
                    manaRegen = -calculationResult.BaseState.ManaRegen5SR;
                    for (int segment = 0; segment < drumsOfBattleSegments; segment++)
                    {
                        List<CastingState> states = (calculationOptions.FightDuration - calculationOptions.MoltenFuryPercentage * calculationOptions.FightDuration < segment * segmentDuration) ? mfDrumsStates : drumsStates;
                        foreach (CastingState state in states)
                        {
                            calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.DrumsOfBattle, Segment = segment, State = state });
                            column = lp.AddColumnUnsafe();
                            lp.SetColumnUpperBound(column, calculationResult.BaseState.GlobalCooldown * ((drumsOfBattleSegments > 1) ? 1 : (1 + (int)((calculationOptions.FightDuration - 30) / 120))));
                            if (segment == 0 && state == states[0]) calculationResult.ColumnDrumsOfBattle = column;
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
                // summon water elemental
                if (waterElementalAvailable)
                {
                    int waterElementalSegments = segments; // always segment, we need it to guarantee each block has activation
                    manaRegen = (int)(0.16 * BaseSpell.BaseMana[calculationOptions.PlayerLevel]) / calculationResult.BaseState.GlobalCooldown - calculationResult.BaseState.ManaRegen5SR;
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
                        states.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, false, false, false, false, false, false, false, false, false, false, true, false));
                    }
                    for (int segment = 0; segment < waterElementalSegments; segment++)
                    {
                        foreach (CastingState state in states)
                        {
                            Waterbolt waterbolt = new Waterbolt(state, state.FrostDamage);
                            calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.SummonWaterElemental, Segment = segment, State = state, Spell = waterbolt });
                            column = lp.AddColumnUnsafe();
                            if (waterElementalSegments > 1) lp.SetColumnUpperBound(column, calculationResult.BaseState.GlobalCooldown);
                            if (segment == 0 && state == states[0]) calculationResult.ColumnSummonWaterElemental = column;
                            lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                            lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                            lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                            lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                            lp.SetElementUnsafe(rowSummonWaterElemental, column, -1 / calculationResult.BaseState.GlobalCooldown);
                            lp.SetElementUnsafe(rowSummonWaterElementalCount, column, 1.0);
                            lp.SetElementUnsafe(rowWaterElemental, column, 1.0);
                            lp.SetCostUnsafe(column, minimizeTime ? -1 : waterbolt.DamagePerSecond);
                            tpsList.Add(0.0);
                            lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
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
                                    double cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
                                    int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                    if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                    if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentWaterElemental + ss, column, 1.0);
                                    if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                                }
                                for (int ss = 0; ss < segments; ss++)
                                {
                                    double cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
                                    int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                                    if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                                    if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentSummonWaterElemental + ss, column, 1.0);
                                    if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
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
                    lp.SetColumnUpperBound(column, maxDrinkingTime);
                    manaRegen = -calculationResult.BaseState.ManaRegenDrinking;
                    lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                    lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
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
                    lp.SetColumnUpperBound(column, calculationOptions.FightDuration);
                    lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                    lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                    lp.SetElementUnsafe(rowEvocation, column, calculationResult.EvocationDuration / calculationResult.EvocationCooldown);
                    //lp.SetElementUnsafe(rowPotion, column, 1.0 / 120.0);
                    lp.SetElementUnsafe(rowManaGem, column, 1.0 / 120.0);
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
                    lp.SetElementUnsafe(rowDrumsOfBattle, column, calculationResult.BaseState.GlobalCooldown / 120.0);
                    tpsList.Add(0.0);
                }
                // after fight regen
                if (afterFightRegen)
                {
                    calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.AfterFightRegen });
                    calculationResult.ColumnAfterFightRegen = column = lp.AddColumnUnsafe();
                    lp.SetColumnUpperBound(column, calculationOptions.FightDuration);
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
                // conjure mana gem
                if (conjureManaGem)
                {
                    int conjureSegments = (restrictManaUse) ? segments : 1;
                    Spell spell = new ConjureManaGem(calculationResult.BaseState);
                    calculationResult.ConjureManaGem = spell;
                    calculationResult.MaxConjureManaGem = (int)((calculationOptions.FightDuration - 300.0f) / 360.0f) + 1;
                    manaRegen = spell.CostPerSecond - spell.ManaRegenPerSecond;
                    for (int segment = 0; segment < conjureSegments; segment++)
                    {
                        column = lp.AddColumnUnsafe();
                        lp.SetColumnUpperBound(column, spell.CastTime * ((conjureSegments > 1) ? 1 : calculationResult.MaxConjureManaGem));                        
                        calculationResult.SolutionVariable.Add(new SolutionVariable() { Type = VariableType.ConjureManaGem, Spell = spell, Segment = segment, State = calculationResult.BaseState });
                        lp.SetElementUnsafe(rowAfterFightRegenMana, column, manaRegen);
                        lp.SetElementUnsafe(rowManaRegen, column, manaRegen);
                        lp.SetElementUnsafe(rowConjureManaGem, column, 1.0);
                        lp.SetElementUnsafe(rowFightDuration, column, 1.0);
                        lp.SetElementUnsafe(rowTimeExtension, column, -1.0);
                        lp.SetElementUnsafe(rowThreat, column, tps = spell.ThreatPerSecond);
                        lp.SetElementUnsafe(rowTargetDamage, column, -spell.DamagePerSecond);
                        lp.SetCostUnsafe(column, minimizeTime ? -1 : spell.DamagePerSecond);
                        tpsList.Add(tps);
                        lp.SetElementUnsafe(rowManaGem, column, -3.0 / spell.CastTime); // one cast time gives 3 new gem uses
                        if (segmentNonCooldowns) lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
                        if (restrictManaUse)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentManaUnderflow + ss, column, manaRegen);
                                lp.SetElementUnsafe(rowSegmentManaOverflow + ss, column, -manaRegen);
                            }
                        }
                        if (restrictThreat)
                        {
                            for (int ss = segment; ss < segments - 1; ss++)
                            {
                                lp.SetElementUnsafe(rowSegmentThreat + ss, column, tps);
                            }
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
                            if ((calculationOptions.IncrementalSetStateIndexes[index] & stateList[buffset].Cooldown) == calculationOptions.IncrementalSetStateIndexes[index])
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

                    List<Spell> placed = new List<Spell>();
                    for (int seg = 0; seg < segments; seg++)
                    {
                        segmentColumn[seg] = column + 1;
                        for (int buffset = 0; buffset < stateList.Count; buffset++)
                        {
                            placed.Clear();
                            for (int spell = 0; spell < spellList.Count; spell++)
                            {
                                if (segmentCooldowns && stateList[buffset].MoltenFury && seg < firstMoltenFurySegment) continue;
                                if (!segmentNonCooldowns && stateList[buffset] == calculationResult.BaseState && seg != 0) continue;
                                if (segmentCooldowns && calculationOptions.HeroismControl == 3 && stateList[buffset].Heroism && seg < firstMoltenFurySegment) continue;
                                Spell s = stateList[buffset].GetSpell(spellList[spell]);
                                bool skip = false;
                                foreach (Spell s2 in placed)
                                {
                                    // TODO verify it this is ok, it assumes that spells placed under same casting state are independent except for aoe spells
                                    // assuming there are no constraints that depend on properties of particular spell cycle instead of properties of casting state
                                    if (!s.AreaEffect && s2.DamagePerSecond >= s.DamagePerSecond - 0.00001 && s2.ManaPerSecond <= s.ManaPerSecond + 0.00001)
                                    {
                                        skip = true;
                                        break;
                                    }
                                }
                                if (!skip && (s.AffectedByFlameCap || !stateList[buffset].FlameCap))
                                {
                                    placed.Add(s);
                                    column = lp.AddColumnUnsafe();
                                    calculationResult.SolutionVariable.Add(new SolutionVariable() { State = stateList[buffset], Spell = s, Segment = seg, Type = VariableType.Spell });
                                    SetSpellColumn(minimizeTime, tpsList, seg, stateList[buffset], column, s);
                                }
                            }
                        }
                    }
                    segmentColumn[segments] = column + 1;
                }

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
                if (waterElementalAvailable) effectiveDuration -= calculationResult.BaseState.GlobalCooldown; // EXPERIMENTAL
                ivlength = MaximizeEffectDuration(effectiveDuration, 20.0, calculationResult.IcyVeinsCooldown);
            }

            double aplength = MaximizeEffectDuration(calculationOptions.FightDuration, calculationResult.ArcanePowerDuration, calculationResult.ArcanePowerCooldown);
            double mflength = calculationOptions.MoltenFuryPercentage * calculationOptions.FightDuration;
            
            // these between-two cooldowns limits are now a lot more questionable since a lot of these
            // have varying cooldown durations based on talents/glyphs
            /*double dpivstackArea = calculationOptions.FightDuration;
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
            }*/

            // mana burn estimate
            float manaBurn = 80;
            if (calculationOptions.AoeDuration > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ArcaneExplosion);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.EmpoweredFire > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.Fireball);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.EmpoweredFrostbolt > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.FrostboltFOF);
                manaBurn = s.CostPerSecond - s.ManaRegenPerSecond;
            }
            else if (character.MageTalents.SpellPower > 0)
            {
                Spell s = calculationResult.BaseState.GetSpell(SpellId.ABABar);
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
            lp.SetRHSUnsafe(rowEvocation, calculationResult.EvocationDuration * calculationResult.MaxEvocation);
            lp.SetRHSUnsafe(rowPotion, calculationResult.MaxManaPotion);
            lp.SetRHSUnsafe(rowManaPotion, calculationResult.MaxManaPotion);
            lp.SetRHSUnsafe(rowManaGem, Math.Min(3, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaGem));
            lp.SetRHSUnsafe(rowManaGemMax, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaGem);
            if (conjureManaGem) lp.SetRHSUnsafe(rowConjureManaGem, calculationResult.MaxConjureManaGem * calculationResult.ConjureManaGem.CastTime);
            if (heroismAvailable) lp.SetRHSUnsafe(rowHeroism, 40);
            if (arcanePowerAvailable) lp.SetRHSUnsafe(rowArcanePower, calculationOptions.AverageCooldowns ? calculationResult.ArcanePowerDuration / calculationResult.ArcanePowerCooldown * calculationOptions.FightDuration : aplength);
            if (heroismAvailable && arcanePowerAvailable) lp.SetRHSUnsafe(rowHeroismArcanePower, calculationResult.ArcanePowerDuration);
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
            else if (flameCapAvailable && !(!calculationOptions.SmartOptimization && character.MageTalents.SpellPower > 0))
            {
                lp.SetRHSUnsafe(rowManaGemFlameCap, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : ((int)(calculationOptions.FightDuration / 180.0 + 2.0 / 3.0)) * 3.0 / 2.0);
            }
            else
            {
                lp.SetRHSUnsafe(rowManaGemFlameCap, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration / 120.0 : calculationResult.MaxManaGem);
            }
            if (moltenFuryAvailable) lp.SetRHSUnsafe(rowMoltenFuryFlameCap, 60);
            //lp.SetRHSUnsafe(rowFlameCapDestructionPotion, dpflamelength);
            if (trinket1Available) lp.SetRHSUnsafe(rowTrinket1, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * trinket1Duration / trinket1Cooldown : ((1 + (int)((calculationOptions.FightDuration - trinket1Duration) / trinket1Cooldown)) * trinket1Duration));
            if (trinket2Available) lp.SetRHSUnsafe(rowTrinket2, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * trinket2Duration / trinket2Cooldown : ((1 + (int)((calculationOptions.FightDuration - trinket2Duration) / trinket2Cooldown)) * trinket2Duration));
            if (manaGemEffectAvailable) lp.SetRHSUnsafe(rowManaGemEffect, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * manaGemEffectDuration / 120f : ((1 + (int)((calculationOptions.FightDuration - manaGemEffectDuration) / 120f)) * manaGemEffectDuration));
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
            //lp.SetRHSUnsafe(rowIcyVeinsDrumsOfBattle, drumsivlength);
            //lp.SetRHSUnsafe(rowArcanePowerDrumsOfBattle, drumsaplength);
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
            if (manaGemEffectAvailable && manaConsum < calculationResult.MaxManaGem) manaConsum = calculationResult.MaxManaGem;
            lp.SetRHSUnsafe(rowManaPotionManaGem, manaConsum * 40.0);
            lp.SetRHSUnsafe(rowDrumsOfBattle, calculationOptions.AverageCooldowns ? calculationOptions.FightDuration * calculationResult.BaseState.GlobalCooldown / 120.0 : calculationResult.BaseState.GlobalCooldown * (1 + (int)((calculationOptions.FightDuration - 30) / 120)));
            if (waterElementalAvailable)
            {
                double duration = calculationOptions.AverageCooldowns ? (calculationResult.WaterElementalDuration / calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration / calculationResult.ColdsnapCooldown : 0.0)) * calculationOptions.FightDuration : weDuration;
                lp.SetRHSUnsafe(rowWaterElemental, duration);
                lp.SetRHSUnsafe(rowSummonWaterElementalCount, calculationResult.BaseState.GlobalCooldown * Math.Ceiling(duration / calculationResult.WaterElementalDuration));
            }
            lp.SetRHSUnsafe(rowTargetDamage, -calculationOptions.TargetDamage);

            if (segmentCooldowns)
            {
                if (moltenFuryAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        if ((seg + 1) * segmentDuration > calculationOptions.FightDuration - mflength)
                        {
                            if (calculationOptions.FightDuration - mflength < seg * segmentDuration)
                            {
                                double dur = (seg < segments - 1) ? segmentDuration : (calculationOptions.FightDuration - (segments - 1) * segmentDuration);
                                lp.SetRHSUnsafe(rowSegmentMoltenFury + seg, dur);
                                lp.SetLHSUnsafe(rowSegmentMoltenFury + seg, dur);
                            }
                            else
                            {
                                double dur = Math.Max(0, segmentDuration - (calculationOptions.FightDuration - mflength - seg * segmentDuration));
                                lp.SetRHSUnsafe(rowSegmentMoltenFury + seg, dur);
                                lp.SetLHSUnsafe(rowSegmentMoltenFury + seg, dur);
                            }
                        }
                    }
                }
                // heroism
                // ap
                if (arcanePowerAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentArcanePower + seg, calculationResult.ArcanePowerDuration);
                        double cool = calculationResult.ArcanePowerCooldown;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // iv
                if (icyVeinsAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentIcyVeins + seg, 20 + (coldsnapAvailable ? 20 : 0));
                        double cool = calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20 : 0);
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
                // water elemental
                if (waterElementalAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentWaterElemental + seg, calculationResult.WaterElementalDuration + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0));
                        double cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentSummonWaterElemental + seg, calculationResult.BaseState.GlobalCooldown + (coldsnapAvailable ? calculationResult.BaseState.GlobalCooldown : 0.0));
                        double cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
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
                if (manaGemEffectAvailable)
                {
                    for (int seg = 0; seg < segments; seg++)
                    {
                        lp.SetRHSUnsafe(rowSegmentManaGemEffect + seg, manaGemEffectDuration);
                        double cool = 120f;
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                // timing
                for (int seg = 0; seg < segments; seg++)
                {
                    lp.SetRHSUnsafe(rowSegment + seg, (seg < segments - 1) ? segmentDuration : (calculationOptions.FightDuration - (segments - 1) * segmentDuration));
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
            if (restrictThreat)
            {
                for (int ss = 0; ss < segments - 1; ss++)
                {
                    lp.SetRHSUnsafe(rowSegmentThreat + ss, calculationOptions.TpsLimit * segmentDuration * (ss + 1));
                }
            }
        }

        private int ConstructRows(bool minimizeTime, bool drinkingEnabled, bool needsTimeExtension, bool afterFightRegen)
        {
            int rowCount = 0;

            if (!calculationOptions.UnlimitedMana) rowManaRegen = rowCount++;
            rowFightDuration = rowCount++;
            if (calculationOptions.EvocationEnabled && (needsTimeExtension || restrictManaUse || integralMana)) rowEvocation = rowCount++;
            if (calculationOptions.ManaPotionEnabled || effectPotionAvailable) rowPotion = rowCount++;
            if (calculationOptions.ManaPotionEnabled && integralMana) rowManaPotion = rowCount++;
            if (calculationOptions.ManaGemEnabled)
            {
                rowManaGem = rowCount++;
                rowManaGemMax = rowCount++;
            }
            if (conjureManaGem)
            {
                rowConjureManaGem = rowCount++;
            }
            if (heroismAvailable) rowHeroism = rowCount++;
            if (arcanePowerAvailable) rowArcanePower = rowCount++;
            if (heroismAvailable && arcanePowerAvailable) rowHeroismArcanePower = rowCount++;
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
                if (character.MageTalents.BlastWave == 1) rowBlastWave = rowCount++;
                if (character.MageTalents.DragonsBreath == 1) rowDragonsBreath = rowCount++;
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
            if (calculationOptions.TpsLimit != 5000f && calculationOptions.TpsLimit > 0f) rowThreat = rowCount++;
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
                        double cool = calculationResult.ArcanePowerCooldown;
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
                        double cool = calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20 : 0);
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
                if (waterElementalAvailable)
                {
                    rowSegmentWaterElemental = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
                        if (seg * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                    rowSegmentSummonWaterElemental = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
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
                // mana gem effect
                if (manaGemEffectAvailable)
                {
                    rowSegmentManaGemEffect = rowCount;
                    for (int seg = 0; seg < segments; seg++)
                    {
                        rowCount++;
                        double cool = 120f;
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
                if (restrictThreat)
                {
                    rowSegmentThreat = rowCount;
                    rowCount += segments - 1;
                }
            }
            return rowCount;
        }

        private void SetSpellColumn(bool minimizeTime, List<double> tpsList, int segment, CastingState state, int column, Spell spell)
        {
            double bound = calculationOptions.FightDuration;
            double manaRegen = spell.CostPerSecond - spell.ManaRegenPerSecond;
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
            if (state.Heroism && state.ArcanePower) lp.SetElementUnsafe(rowHeroismArcanePower, column, 1.0);
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
            //if (state.DrumsOfBattle) lp.SetElementUnsafe(rowDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle) lp.SetElementUnsafe(rowDrumsOfBattleActivation, column, 1 / (30 - calculationResult.BaseState.GlobalCooldown));
            if (state.DrumsOfBattle && state.MoltenFury) lp.SetElementUnsafe(rowMoltenFuryDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.Heroism) lp.SetElementUnsafe(rowHeroismDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.IcyVeins) lp.SetElementUnsafe(rowIcyVeinsDrumsOfBattle, column, 1.0);
            if (state.DrumsOfBattle && state.ArcanePower) lp.SetElementUnsafe(rowArcanePowerDrumsOfBattle, column, 1.0);
            if (state.WaterElemental) lp.SetElementUnsafe(rowSummonWaterElemental, column, 1 / (calculationResult.WaterElementalDuration - calculationResult.BaseState.GlobalCooldown));
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
                    bound = Math.Min(bound, calculationResult.ArcanePowerDuration);
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = calculationResult.ArcanePowerCooldown;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentArcanePower + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.IcyVeins)
                {
                    bound = Math.Min(bound, (coldsnapAvailable) ? 40.0 : 20.0);
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = calculationResult.IcyVeinsCooldown + (coldsnapAvailable ? 20 : 0);
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentIcyVeins + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.WaterElemental)
                {
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = calculationResult.WaterElementalCooldown + (coldsnapAvailable ? calculationResult.WaterElementalDuration : 0.0);
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentWaterElemental + ss, column, 1.0);
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
                    bound = Math.Min(bound, trinket2Duration);
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = trinket2Cooldown;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentTrinket2 + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (state.ManaGemEffect)
                {
                    bound = Math.Min(bound, manaGemEffectDuration);
                    for (int ss = 0; ss < segments; ss++)
                    {
                        double cool = 120f;
                        int maxs = (int)Math.Floor(ss + cool / segmentDuration) - 1;
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) maxs = segments - 1;
                        if (segment >= ss && segment <= maxs) lp.SetElementUnsafe(rowSegmentManaGemEffect + ss, column, 1.0);
                        if (ss * segmentDuration + cool >= calculationOptions.FightDuration) break;
                    }
                }
                if (segmentNonCooldowns || state != calculationResult.BaseState)
                {
                    bound = Math.Min(bound, segmentDuration);
                    lp.SetElementUnsafe(rowSegment + segment, column, 1.0);
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
            if (restrictThreat)
            {
                for (int ss = segment; ss < segments - 1; ss++)
                {
                    lp.SetElementUnsafe(rowSegmentThreat + ss, column, spell.ThreatPerSecond);
                }
            }
            lp.SetColumnUpperBound(column, bound);
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
                if (calculationOptions.MaintainScorch && calculationOptions.MaintainSnare && character.MageTalents.ImprovedScorch > 0 && character.MageTalents.Slow > 0)
                {
                    // no cycles right now that provide scorch and snare
                }
                if (calculationOptions.MaintainScorch && character.MageTalents.ImprovedScorch > 0)
                {
                    if (calculationOptions.SmartOptimization)
                    {
                        if (character.MageTalents.PiercingIce == 3 && character.MageTalents.IceShards == 3 && calculationOptions.PlayerLevel >= 75)
                        {
                            if (character.MageTalents.LivingBomb > 0)
                            {
                                list.Add(SpellId.FFBScLBPyro);
                            }
                            list.Add(SpellId.FFBScPyro);
                        }
                        else
                        {
                            if (character.MageTalents.LivingBomb > 0)
                            {
                                list.Add(SpellId.FBScLBPyro);
                            }
                            if (character.MageTalents.HotStreak > 0)
                            {
                                list.Add(SpellId.FBScPyro);
                            }
                            else
                            {
                                list.Add(SpellId.FBSc);
                            }
                        }
                    }
                    else
                    {
                        if (character.MageTalents.LivingBomb > 0)
                        {
                            list.Add(SpellId.FBScLBPyro);
                            list.Add(SpellId.ScLBPyro);
                        }
                        else if (character.MageTalents.HotStreak > 0)
                        {
                            list.Add(SpellId.FBScPyro);
                        }
                        else
                        {
                            list.Add(SpellId.FBSc);
                        }
                        if (calculationOptions.PlayerLevel >= 75)
                        {
                            list.Add(SpellId.FFBScPyro);
                            if (character.MageTalents.LivingBomb > 0)
                            {
                                list.Add(SpellId.FFBScLBPyro);
                            }
                        }
                        list.Add(SpellId.FBFBlast);
                    }
                }
                else if (calculationOptions.MaintainSnare && character.MageTalents.Slow > 0)
                {
                    if (calculationOptions.SmartOptimization)
                    {
                        if (character.MageTalents.ArcaneBarrage > 0)
                        {
                            if (character.MageTalents.ImprovedFrostbolt > 0)
                            {
                                list.Add(SpellId.FrBABarSlow);
                            }
                            if (character.MageTalents.ImprovedFireball > 0)
                            {
                                list.Add(SpellId.FBABarSlow);
                            }
                            if (character.MageTalents.ArcaneEmpowerment > 0)
                            {
                                list.Add(SpellId.ABABarSlow);
                            }
                            if (character.MageTalents.ImprovedFrostbolt == 0 && character.MageTalents.ImprovedFireball == 0 && character.MageTalents.ArcaneEmpowerment == 0)
                            {
                                list.Add(SpellId.FrBABarSlow);
                                list.Add(SpellId.FBABarSlow);
                                list.Add(SpellId.ABABarSlow);
                            }
                        }
                    }
                    else
                    {
                        list.Add(SpellId.FrBABarSlow);
                        list.Add(SpellId.FBABarSlow);
                        list.Add(SpellId.ABABarSlow);
                    }
                }
                else
                {
                    if (calculationOptions.SmartOptimization)
                    {
                        if (character.MageTalents.EmpoweredFire > 0)
                        {
                            if (character.MageTalents.PiercingIce == 3 && character.MageTalents.IceShards == 3 && calculationOptions.PlayerLevel >= 75)
                            {
                                list.Add(SpellId.FFBPyro);
                                if (character.MageTalents.LivingBomb > 0) list.Add(SpellId.FFBLBPyro);
                            }
                            else
                            {
                                if (character.MageTalents.HotStreak > 0 && character.MageTalents.Pyroblast > 0)
                                {
                                    list.Add(SpellId.FBPyro);
                                }
                                else
                                {
                                    list.Add(SpellId.Fireball);
                                }
                                if (character.MageTalents.LivingBomb > 0) list.Add(SpellId.FBLBPyro);
                            }
                        }
                        else if (character.MageTalents.EmpoweredFrostbolt > 0)
                        {
                            if (character.MageTalents.BrainFreeze > 0)
                            {
                                list.Add(SpellId.FrBFB);
                            }
                            else
                            {
                                list.Add(SpellId.FrostboltFOF);
                            }
                        }
                        else if (character.MageTalents.ArcaneBarrage > 0)
                        {
                            if (character.MageTalents.ImprovedFrostbolt > 0)
                            {
                                list.Add(SpellId.Frostbolt);
                                list.Add(SpellId.FrBABar);
                                list.Add(SpellId.FrB2ABar);
                            }
                            if (character.MageTalents.ImprovedFireball > 0)
                            {
                                list.Add(SpellId.Fireball);
                                list.Add(SpellId.FBABar);
                                list.Add(SpellId.FB2ABar);
                            }
                            if (character.MageTalents.ArcaneEmpowerment > 0)
                            {
                                if (!calculationOptions.Mode308) list.Add(SpellId.ABP);
                                list.Add(SpellId.ABABar);
                                list.Add(SpellId.AB2ABar);
                            }
                            if (character.MageTalents.ImprovedFrostbolt == 0 && character.MageTalents.ImprovedFireball == 0 && character.MageTalents.ArcaneEmpowerment == 0)
                            {
                                list.Add(SpellId.FrBABar);
                                list.Add(SpellId.FBABar);
                                if (!calculationOptions.Mode308) list.Add(SpellId.ABP);
                                list.Add(SpellId.ABABar);
                                list.Add(SpellId.AB2ABar);
                                list.Add(SpellId.FB2ABar);
                                list.Add(SpellId.FrB2ABar);
                            }
                        }
                        else
                        {
                            list.Add(SpellId.ArcaneMissiles);
                            list.Add(SpellId.Fireball);
                            list.Add(SpellId.FrostboltFOF);
                            if (calculationOptions.PlayerLevel >= 75) list.Add(SpellId.FrostfireBoltFOF);
                            if (!calculationOptions.Mode308) list.Add(SpellId.ABP);
                        }
                    }
                    else
                    {
                        list.Add(SpellId.ArcaneMissiles);
                        list.Add(SpellId.Scorch);
                        if (character.MageTalents.LivingBomb > 0) list.Add(SpellId.ScLBPyro);
                        if (character.MageTalents.HotStreak > 0 && character.MageTalents.Pyroblast > 0)
                        {
                            list.Add(SpellId.FBPyro);
                        }
                        else
                        {
                            list.Add(SpellId.Fireball);
                        }
                        if (calculationOptions.PlayerLevel >= 75)
                        {
                            list.Add(SpellId.FrostfireBoltFOF);
                            list.Add(SpellId.FFBPyro);
                            if (character.MageTalents.LivingBomb > 0) list.Add(SpellId.FFBLBPyro);
                        }
                        list.Add(SpellId.FBFBlast);
                        if (character.MageTalents.LivingBomb > 0) list.Add(SpellId.FBLBPyro);
                        list.Add(SpellId.FrostboltFOF);
                        if (character.MageTalents.BrainFreeze > 0) list.Add(SpellId.FrBFB);
                        list.Add(SpellId.ArcaneBlastSpam);
                        list.Add(SpellId.ABAM);
                        if (!calculationOptions.Mode308) list.Add(SpellId.ABP);
                        if (character.MageTalents.ArcaneBarrage > 0) list.Add(SpellId.ABABar);
                        if (character.MageTalents.ArcaneBarrage > 0) list.Add(SpellId.AB2ABar);
                        if (character.MageTalents.ArcaneBarrage > 0) list.Add(SpellId.ABarAM);
                        if (character.MageTalents.MissileBarrage > 0) list.Add(SpellId.ABMBAM);
                        if (character.MageTalents.ArcaneBarrage > 0) list.Add(SpellId.FBABar);
                        if (character.MageTalents.ArcaneBarrage > 0) list.Add(SpellId.FB2ABar);
                        if (character.MageTalents.ArcaneBarrage > 0) list.Add(SpellId.FrBABar);
                        if (character.MageTalents.ArcaneBarrage > 0) list.Add(SpellId.FrB2ABar);
                        if (calculationOptions.PlayerLevel >= 75 && character.MageTalents.ArcaneBarrage > 0) list.Add(SpellId.FFBABar);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.ABABarX);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.ABABarY);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.AB3ABar);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.AB3ABarC);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.AB3ABarX);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.ABAMABar);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.AB2AMABar);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.AB3AMABar);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.AB32AMABar);
                        if (character.MageTalents.ArcaneBarrage > 0 && character.MageTalents.MissileBarrage > 0 && calculationOptions.Mode308) list.Add(SpellId.AB3MBAMABar);
                    }
                }
                if (calculationOptions.AoeDuration > 0)
                {
                    list.Add(SpellId.ArcaneExplosion);
                    list.Add(SpellId.FlamestrikeSpammed);
                    list.Add(SpellId.FlamestrikeSingle);
                    list.Add(SpellId.Blizzard);
                    list.Add(SpellId.ConeOfCold);
                    if (character.MageTalents.BlastWave == 1) list.Add(SpellId.BlastWave);
                    if (character.MageTalents.DragonsBreath == 1) list.Add(SpellId.DragonsBreath);
                }
            }
            return list;
        }

        private List<CastingState> GetStateList(Stats characterStats)
        {
            List<CastingState> list = new List<CastingState>();

            Cooldown availableMask = 0;
            bool canDoubleTrinket = false;
            if (waterElementalAvailable) availableMask |= Cooldown.WaterElemental;
            if (moltenFuryAvailable) availableMask |= Cooldown.MoltenFury;
            if (heroismAvailable) availableMask |= Cooldown.Heroism;
            if (arcanePowerAvailable) availableMask |= Cooldown.ArcanePower;
            if (icyVeinsAvailable) availableMask |= Cooldown.IcyVeins;
            if (combustionAvailable) availableMask |= Cooldown.Combustion;
            if (drumsOfBattleAvailable) availableMask |= Cooldown.DrumsOfBattle;
            if (flameCapAvailable) availableMask |= Cooldown.FlameCap;
            if (potionOfWildMagicAvailable) availableMask |= Cooldown.PotionOfWildMagic;
            if (potionOfSpeedAvailable) availableMask |= Cooldown.PotionOfSpeed;
            if (trinket1Available) availableMask |= Cooldown.Trinket1;
            if (trinket2Available) availableMask |= Cooldown.Trinket2;
            if (manaGemEffectAvailable) availableMask |= Cooldown.ManaGemEffect;
            if (calculationOptions.IncrementalOptimizations)
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
                    for (Cooldown index = 0; index <= Cooldown.ItemBasedMask; index++)
                    {
                        bool trinket1 = (index & Cooldown.Trinket1) != 0;
                        bool trinket2 = (index & Cooldown.Trinket2) != 0;
                        bool mg = (index & Cooldown.ManaGemEffect) != 0;
                        if (((incrementalSetIndex | index) & availableMask) == (incrementalSetIndex | index)) // make sure all are available
                        {
                            if (!trinket1 || !trinket2 || canDoubleTrinket) // only leave through trinkets that can stack
                            {
                                if (!mg || !flameCap) // do not allow mana gem together with flame cap
                                {
                                    if (!pos || !powm) // do not allow different potions at the same time
                                    {
                                        if ((calculationOptions.HeroismControl != 1 || !heroism || !mf) && (calculationOptions.HeroismControl != 2 || !heroism || (incrementalSetIndex == 0 && index == 0)) || (calculationOptions.HeroismControl != 3 || !moltenFuryAvailable || !heroism || mf))
                                        {
                                            list.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, ap, mf, iv, heroism, powm, pos, flameCap, trinket1, trinket2, combustion, drums, we, mg));
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
                if (calculationResult.BaseState == null) calculationResult.BaseState = new CastingState(calculationResult, characterStats, calculationOptions, armor, character, false, false, false, false, false, false, false, false, false, false, false, false, false);
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
                    if (((incrementalSetIndex) & availableMask) == (incrementalSetIndex)) // make sure all are available
                    {
                        if (!trinket1 || !trinket2 || canDoubleTrinket) // only leave through trinkets that can stack
                        {
                            if (!mg || !flameCap) // do not allow mana gem together with flame cap
                            {
                                if (!pos || !powm) // do not allow different potions at the same time
                                {
                                    if ((calculationOptions.HeroismControl != 1 || !heroism || !mf) && (calculationOptions.HeroismControl != 2 || !heroism || (incrementalSetIndex == 0)) || (calculationOptions.HeroismControl != 3 || !moltenFuryAvailable || !heroism || mf))
                                    {
                                        list.Add(new CastingState(calculationResult, characterStats, calculationOptions, armor, character, ap, mf, iv, heroism, powm, pos, flameCap, trinket1, trinket2, combustion, drums, we, mg));
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
