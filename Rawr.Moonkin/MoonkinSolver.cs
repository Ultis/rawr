using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    // The interface public class to the rest of Rawr.  Provides a single Solve method that runs all the calculations.
    public class MoonkinSolver
    {
        private const int NUM_SPELL_DETAILS = 17;
        // A list of all currently active proc effects.
        public List<ProcEffect> procEffects;
        public static float BaseMana;
        public static float OOC_PROC_CHANCE = 0.02f;
        public static float EUPHORIA_PERCENT = 0.08f;
        // A list of all the damage spells
        private Spell[] _spellData = null;
        private Spell[] SpellData
        {
            get
            {
                if (_spellData == null)
                {
                    _spellData = new Spell[] {
                        new Spell()
                        {
                            Name = "SF",
                            BaseDamage = (987 + 1229) / 2.0f,
                            SpellDamageModifier = 1.0f,
                            BaseCastTime = 3.2f,
                            BaseManaCost = (float)(int)(BaseMana * 0.11f),
                            DotEffect = null,
                            School = SpellSchool.Arcane,
                            BaseEnergy = 20
                        },
                        new Spell()
                        {
                            Name = "MF",
                            BaseDamage = (197.0f + 239.0f) / 2.0f,
                            SpellDamageModifier = 0.18f,
                            BaseCastTime = 1.5f,
                            BaseManaCost = (float)(int)(BaseMana * 0.09f),
                            DotEffect = new DotEffect()
                                {
                                    BaseDuration = 12.0f,
                                    BaseTickLength = 2.0f,
                                    TickDamage = 93.0f,
                                    SpellDamageModifierPerTick = 0.18f
                                },
                            School = SpellSchool.Arcane
                        },
                        new Spell()
                        {
                            Name = "W",
                            BaseDamage = (675f + 761.0f) / 2.0f,
                            SpellDamageModifier = 2.5f/3.5f,
                            BaseCastTime = 2.5f,
                            BaseManaCost = (float)(int)(BaseMana * 0.09f),
                            DotEffect = null,
                            School = SpellSchool.Nature,
                            BaseEnergy = 40/3
                        },
                        new Spell()
                        {
                            Name = "IS",
                            BaseDamage = 0.0f,
                            SpellDamageModifier = 0.0f,
                            BaseCastTime = 1.5f,
                            BaseManaCost = (float)(int)(BaseMana * 0.08f),
                            DotEffect = new DotEffect()
                            {
                                BaseDuration = 12.0f,
                                BaseTickLength = 2.0f,
                                TickDamage = 136.0f,
                                SpellDamageModifierPerTick = 0.13f
                            },
                            School = SpellSchool.Nature
                        },
                        new Spell()
                        {
                            Name = "SS",
                            BaseDamage = (1272 + 1756) / 2f,
                            SpellDamageModifier = 1.535f,
                            BaseCastTime = 2.0f,
                            BaseManaCost = (float)(int)(BaseMana * 0.11f),
                            DotEffect = null,
                            School = SpellSchool.Spellstorm,
                            BaseEnergy = 15,
                            CriticalDamageModifier = 1.5f
                        }
                    };
                }
                return _spellData;
            }
        }
        public Spell Starfire
        {
            get
            {
                return SpellData[0];
            }
        }
        public Spell Moonfire
        {
            get
            {
                return SpellData[1];
            }
        }
        public Spell Wrath
        {
            get
            {
                return SpellData[2];
            }
        }
        public Spell InsectSwarm
        {
            get
            {
                return SpellData[3];
            }
        }
        public Spell Starsurge
        {
            get
            {
                return SpellData[4];
            }
        }
        public void ResetSpellList()
        {
            // Since the property rebuilding the array is based on this variable being null, this effectively forces a refresh
            _spellData = null;
        }
        public Spell FindSpell(string name)
        {
            switch (name)
            {
                case "SF":
                    return Starfire;
                case "MF":
                    return Moonfire;
                case "IS":
                    return InsectSwarm;
                case "W":
                    return Wrath;
                case "SS":
                    return Starsurge;
                default:
                    return null;
            }
        }

        // The spell rotations themselves.
        private List<SpellRotation> rotations = null;
        public List<SpellRotation> Rotations
        {
            get
            {
                if (rotations == null) RecreateRotations();
                return rotations;
            }
        }

        // Results data from the calculations, which will be sent to the UI.
        Dictionary<string, RotationData> cachedResults = new Dictionary<string, RotationData>();

        public float GetSpellHit(CharacterCalculationsMoonkin calcs)
        {
            float baseHit = 1.0f;
            switch (calcs.TargetLevel - calcs.PlayerLevel)
            {
                case 0:
                    baseHit -= 0.04f;
                    break;
                case 1:
                    baseHit -= 0.05f;
                    break;
                case 2:
                    baseHit -= 0.06f;
                    break;
                case 3:
                    baseHit -= 0.17f;
                    break;
                default:
                    baseHit -= 0.17f;
                    break;
            }
            baseHit = (float)Math.Min(1.0f, baseHit + calcs.SpellHit);
            return baseHit;
        }

        public void Solve(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            BaseMana = BaseStats.GetBaseStats(character).Mana;
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            DruidTalents talents = character.DruidTalents;
            procEffects = new List<ProcEffect>();
            RecreateSpells(character, ref calcs);
            cachedResults = new Dictionary<string, RotationData>();

            float trinketDPS = 0.0f;
            float baseSpellPower = calcs.SpellPower;
            float baseHit = GetSpellHit(calcs);
            float baseCrit = calcs.SpellCrit;
            float baseHaste = calcs.SpellHaste;
            float baseMastery = calcs.Mastery;

            BuildProcList(calcs);

            float maxDamageDone = 0.0f, maxBurstDamageDone = 0.0f;
            SpellRotation maxBurstRotation = rotations[0];
            SpellRotation maxRotation = rotations[0];

            float manaPool = GetEffectiveManaPool(character, calcOpts, calcs);

            float percentTimeInRotation = 1;
            float movementShare = 0f;

            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) bossOpts = new BossOptions();

            float fearShare = (bossOpts.FearingTargsDur / 1000) / bossOpts.FearingTargsFreq;
            float stunShare = (bossOpts.StunningTargsDur / 1000) / bossOpts.StunningTargsFreq;
            float silenceShare = (bossOpts.SilencingTargsDur / 1000) / bossOpts.SilencingTargsFreq;
            float invulnerableShare = bossOpts.TimeBossIsInvuln / bossOpts.BerserkTimer;

            List<Attack> attacks = bossOpts.GetFilteredAttackList(ATTACK_TYPES.AT_AOE);
            attacks.AddRange(bossOpts.GetFilteredAttackList(ATTACK_TYPES.AT_RANGED));
            int movementCount = attacks.Count;
            float assumedMovementDuration = 2.0f;   // Assume 2 seconds per move
            float accumulatedDurations = 0.0f;
            foreach (Attack a in attacks)
                accumulatedDurations += a.AttackSpeed;

            movementShare = (movementCount == 0 ? 0 : assumedMovementDuration / (accumulatedDurations / movementCount) / (1 + calcs.BasicStats.MovementSpeed));

            percentTimeInRotation -= movementShare + fearShare + stunShare + invulnerableShare + silenceShare;

            float manaGained = manaPool - calcs.BasicStats.Mana;

            float oldArcaneMultiplier = calcs.BasicStats.BonusArcaneDamageMultiplier;
            float oldNatureMultiplier = calcs.BasicStats.BonusNatureDamageMultiplier;

            foreach (SpellRotation rot in rotations)
            {
                if (rot.RotationData.Name == "None") continue;
                rot.Solver = this;

                // Reset variables modified in the pre-loop to base values
                float currentSpellPower = baseSpellPower;
                float currentCrit = baseCrit + StatConversion.NPC_LEVEL_SPELL_CRIT_MOD[calcs.TargetLevel - character.Level];
                float currentHaste = baseHaste;
                float currentMastery = baseMastery;
                calcs.BasicStats.BonusArcaneDamageMultiplier = oldArcaneMultiplier;
                calcs.BasicStats.BonusNatureDamageMultiplier = oldNatureMultiplier;
                float accumulatedDamage = 0.0f;
                float totalUpTime = 0.0f;
                float[] spellDetails = new float[NUM_SPELL_DETAILS];
                List<ProcEffect> activatedEffects = new List<ProcEffect>();
                List<ProcEffect> alwaysUpEffects = new List<ProcEffect>();

                // Pre-calculate rotational variables with base stats
                rot.RotationData.NaturesGraceUptime = 0f;
                float baselineDPS = rot.DamageDone(talents, calcs, calcOpts.TreantLifespan, baseSpellPower, baseHit, baseCrit, baseHaste, baseMastery) / (calcs.FightLength * 60.0f);
                rot.BaselineDuration = rot.RotationData.Duration;
                // Calculate Nature's Grace uptime in a separate loop
				if (talents.NaturesGrace > 0)
                {
                    float delta = 0;
                    do
                    {
                        rot.RotationData.NaturesGraceUptime = 30 / rot.RotationData.Duration;
                        float currentDPS = rot.DamageDone(talents, calcs, calcOpts.TreantLifespan, baseSpellPower, baseHit, currentCrit, currentHaste, currentMastery) / (calcs.FightLength * 60.0f);
                        delta = currentDPS - baselineDPS;
                        baselineDPS = currentDPS;
                        rot.NaturesGraceShortening = rot.BaselineDuration - rot.RotationData.Duration;
                    } while (delta > 1);
                }
                // Calculate spell power/spell damage modifying trinkets in a separate pre-loop
                // Add spell crit effects here as well, since they no longer affect timing
                foreach (ProcEffect proc in procEffects)
                {
                    if (proc.Effect.Stats.SpellPower > 0 || proc.Effect.Stats.CritRating > 0 || proc.Effect.Stats.MasteryRating > 0)
                    {
                        float procSpellPower = proc.Effect.Stats.SpellPower;
                        float procSpellCrit = StatConversion.GetSpellCritFromRating(proc.Effect.Stats.CritRating);
                        float procMastery = StatConversion.GetMasteryFromRating(proc.Effect.Stats.MasteryRating);

                        float triggerInterval = 0.0f, triggerChance = 1.0f;
                        switch (proc.Effect.Trigger)
                        {
                            case Trigger.DamageDone:
                            case Trigger.DamageOrHealingDone:
                                triggerInterval = ((rot.RotationData.Duration / rot.RotationData.CastCount) + (rot.RotationData.Duration / (rot.RotationData.MoonfireTicks + rot.RotationData.InsectSwarmTicks))) / 2.0f;
                                break;
                            case Trigger.Use:
                                break;
                            case Trigger.SpellHit:
                            case Trigger.DamageSpellHit:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.CastCount;
                                triggerChance = GetSpellHit(calcs);
                                break;
                            case Trigger.SpellCrit:
                            case Trigger.DamageSpellCrit:
                                triggerInterval = rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.InsectSwarmCasts);
                                triggerChance = baseCrit;
                                break;
                            case Trigger.SpellCast:
                            case Trigger.DamageSpellCast:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.CastCount;
                                break;
                            case Trigger.MoonfireCast:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.MoonfireCasts;
                                break;
                            case Trigger.DoTTick:
                                triggerInterval = rot.RotationData.Duration / (rot.RotationData.InsectSwarmTicks + rot.RotationData.MoonfireTicks);
                                break;
                            case Trigger.MoonfireTick:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.MoonfireTicks;
                                break;
                            case Trigger.InsectSwarmTick:
                                triggerInterval = rot.RotationData.Duration / rot.RotationData.InsectSwarmTicks;
                                break;
                            default:
                                triggerChance = 0.0f;
                                break;
                        }
                        if (triggerChance > 0)
                        {
                            float durationMultiplier = proc.Effect.LimitedToExecutePhase ? calcs.Sub35Percent : 1f;
                            currentSpellPower += (proc.Effect.MaxStack > 1 ? proc.Effect.GetAverageStackSize(triggerInterval, triggerChance, 3.0f, calcs.FightLength * 60.0f * durationMultiplier) : 1) *
                            proc.Effect.GetAverageUptime(triggerInterval, triggerChance, 3.0f, calcs.FightLength * 60.0f) * procSpellPower * durationMultiplier;
                            currentCrit += (proc.Effect.MaxStack > 1 ? proc.Effect.GetAverageStackSize(triggerInterval, triggerChance, 3.0f, calcs.FightLength * 60.0f * durationMultiplier) : 1) *
                                proc.Effect.GetAverageUptime(triggerInterval, triggerChance, 3.0f, calcs.FightLength * 60.0f) * procSpellCrit * durationMultiplier;
                            currentMastery += (proc.Effect.MaxStack > 1 ? proc.Effect.GetAverageStackSize(triggerInterval, triggerChance, 3.0f, calcs.FightLength * 60.0f * durationMultiplier) : 1) *
                                proc.Effect.GetAverageUptime(triggerInterval, triggerChance, 3.0f, calcs.FightLength * 60.0f) * procMastery * durationMultiplier;
                        }
                    }
                    // 2T10 (both if statements, which is why it isn't else-if)
                    if (proc.Effect.Stats.BonusArcaneDamageMultiplier > 0)
                    {
                        calcs.BasicStats.BonusArcaneDamageMultiplier += proc.Effect.GetAverageUptime(rot.RotationData.Duration / rot.RotationData.CastCount, 1f, 3.0f, calcs.FightLength * 60.0f) * proc.Effect.Stats.BonusArcaneDamageMultiplier;
                    }
                    if (proc.Effect.Stats.BonusNatureDamageMultiplier > 0)
                    {
                        calcs.BasicStats.BonusNatureDamageMultiplier += proc.Effect.GetAverageUptime(rot.RotationData.Duration / rot.RotationData.CastCount, 1f, 3.0f, calcs.FightLength * 60.0f) * proc.Effect.Stats.BonusNatureDamageMultiplier;
                    }
                    if (proc.Effect.Stats._rawSpecialEffectDataSize > 0)
                    {
                        SpecialEffect childEffect = proc.Effect.Stats._rawSpecialEffectData[0];
                        // Nevermelting Ice Crystal
                        if (childEffect.Stats.CritRating != 0)
                        {
                            float maxStack = proc.Effect.Stats.CritRating;
                            float numNegativeStacks = childEffect.GetAverageStackSize(rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.InsectSwarmCasts), Math.Min(1.0f, baseCrit + StatConversion.GetSpellCritFromRating(maxStack)), 3.0f, proc.Effect.Duration);
                            float averageNegativeValue = childEffect.Stats.CritRating * numNegativeStacks;
                            float averageCritRating = maxStack + averageNegativeValue;
                            currentCrit += StatConversion.GetSpellCritFromRating(averageCritRating * proc.Effect.GetAverageUptime(0f, 1f));
                        }
                        // Fetish of Volatile Power
                        if (childEffect.Stats.HasteRating != 0)
                        {
                            currentHaste += StatConversion.GetSpellHasteFromRating(childEffect.Stats.HasteRating * childEffect.GetAverageStackSize(rot.RotationData.Duration / rot.RotationData.CastCount, 1f, 3.0f, proc.Effect.Duration) * proc.Effect.GetAverageUptime(0f, 1f));
                        }
                        // 4T11
                        if (childEffect.Stats.SpellCrit != 0)
                        {
                            float maxStack = proc.Effect.Stats.SpellCrit;
                            float numNegativeStacks = childEffect.GetAverageStackSize(rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.InsectSwarmCasts), Math.Min(1.0f, baseCrit + maxStack), 3.0f, proc.Effect.Duration);
                            float averageNegativeValue = childEffect.Stats.SpellCrit * numNegativeStacks;
                            float averageCrit = maxStack + averageNegativeValue;
                            currentCrit += averageCrit * proc.Effect.GetAverageUptime(rot.RotationData.Duration / 2f, 1f, 3.0f, calcs.FightLength * 60.0f);
                        }
                    }
                }
                // Calculate damage and mana contributions for non-stat-boosting trinkets
                // Separate timing-altering proc trinkets into their own list
                foreach (ProcEffect proc in procEffects)
                {
                    if (proc.CalculateDPS != null)
                    {
                        accumulatedDamage += proc.CalculateDPS(rot, calcs, currentSpellPower, baseHit, currentCrit, currentHaste) * rot.RotationData.Duration;
                    }
                    if (proc.Activate != null)
                    {
                        float upTime = proc.UpTime(rot, calcs);
                        // Procs with 100% uptime should be activated and not put into the combination loop
                        if (upTime == 1)
                        {
                            alwaysUpEffects.Add(proc);
                            proc.Activate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste, ref currentMastery);
                        }
                        // Procs with uptime 0 < x < 100 should be activated
                        else if (upTime > 0)
                            activatedEffects.Add(proc);
                    }
                    if (proc.CalculateMP5 != null)
                    {
                        manaGained += proc.CalculateMP5(rot, calcs, currentSpellPower, baseHit, currentCrit, currentHaste) / 5.0f * calcs.FightLength * 60.0f;
                    }
                }
                // Calculate stat-boosting trinkets, taking into effect interactions with other stat-boosting procs
                int sign = 1;
                Dictionary<int, float> cachedDamages = new Dictionary<int, float>();
                Dictionary<int, float> cachedUptimes = new Dictionary<int, float>();
                Dictionary<int, float[]> cachedDetails = new Dictionary<int, float[]>();
                // Iterate over the entire set of trinket combinations (each trinket by itself, 2 at a time, ...)
                for (int i = 1; i <= activatedEffects.Count; ++i)
                {
                    // Create a new combination generator for this "level" of trinket interaction
                    CombinationGenerator gen = new CombinationGenerator(activatedEffects.Count, i);
                    // Iterate over all combinations
                    while (gen.HasNext())
                    {
                        float tempUpTime = 1.0f;
                        int[] vals = gen.GetNext();
                        int pairs = 0;
                        int lengthCounter = 0;
                        // Activate the trinkets, calculate the damage and uptime, then deactivate them
                        foreach (int idx in vals)
                        {
                            pairs |= 1 << idx;
                            ++lengthCounter;
                            activatedEffects[idx].Activate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste, ref currentMastery);
                        }
                        currentCrit = (float)Math.Min(1.0f, currentCrit);
                        float tempDPS = rot.DamageDone(talents, calcs, calcOpts.TreantLifespan, currentSpellPower, baseHit, currentCrit, currentHaste, currentMastery) / rot.RotationData.Duration;
                        spellDetails[0] = rot.RotationData.StarfireAvgHit;
                        spellDetails[1] = rot.RotationData.WrathAvgHit;
                        spellDetails[2] = rot.RotationData.MoonfireAvgHit;
                        spellDetails[3] = rot.RotationData.InsectSwarmAvgHit;
                        spellDetails[4] = rot.RotationData.StarSurgeAvgHit;
                        spellDetails[5] = rot.RotationData.StarfireAvgCast;
                        spellDetails[6] = rot.RotationData.WrathAvgCast;
                        spellDetails[7] = rot.RotationData.MoonfireAvgCast;
                        spellDetails[8] = rot.RotationData.InsectSwarmAvgCast;
                        spellDetails[9] = rot.RotationData.StarSurgeAvgCast;
                        spellDetails[10] = rot.RotationData.AverageInstantCast;
                        spellDetails[11] = rot.RotationData.StarfireAvgEnergy;
                        spellDetails[12] = rot.RotationData.WrathAvgEnergy;
                        spellDetails[13] = rot.RotationData.StarSurgeAvgEnergy;
                        spellDetails[14] = rot.RotationData.TreantDamage;
                        spellDetails[15] = rot.RotationData.StarfallDamage;
                        spellDetails[16] = rot.RotationData.MushroomDamage;
                        foreach (int idx in vals)
                        {
                            tempUpTime *= activatedEffects[idx].UpTime(rot, calcs);
                            activatedEffects[idx].Deactivate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste, ref currentMastery);
                        }
                        if (tempUpTime == 0) continue;
                        // Adjust previous probability tables by the current factor
                        // At the end of the algorithm, this ensures that the probability table will contain the individual
                        // probabilities of each effect or set of effects.
                        // These adjustments only need to be made for higher levels of the table, and if the current probability is > 0.
                        if (lengthCounter > 1)
                        {
                            List<int> keys = new List<int>(cachedUptimes.Keys);
                            foreach (int subset in keys)
                            {
                                // Truly a subset?
                                if ((pairs & subset) != subset)
                                {
                                    continue;
                                }

                                // Calculate the "layer" of the current subset by getting the set bit count.
                                int subsetLength = 0;
                                for (int j = subset; j > 0; ++subsetLength)
                                {
                                    j &= --j;
                                }

                                // Set the sign of the operation: Evenly separated layers are added, oddly separated layers are subtracted
                                int newSign = ((lengthCounter - subsetLength) % 2 == 0) ? 1 : -1;

                                // Adjust by current uptime * sign of operation.
                                cachedUptimes[subset] += newSign * tempUpTime;
                            }
                        }
                        // Cache the results to be calculated later
                        cachedUptimes[pairs] = tempUpTime;
                        cachedDamages[pairs] = tempDPS;
                        cachedDetails[pairs] = new float[NUM_SPELL_DETAILS];
                        Array.Copy(spellDetails, cachedDetails[pairs], NUM_SPELL_DETAILS);
                        totalUpTime += sign * tempUpTime;
                    }
                    sign = -sign;
                }
                float accumulatedDPS = 0.0f;
                Array.Clear(spellDetails, 0, spellDetails.Length);
                // Apply the above-calculated probabilities to the previously stored damage calculations and add to the result.
                foreach (KeyValuePair<int, float> kvp in cachedUptimes)
                {
                    if (kvp.Value == 0) continue;
                    accumulatedDPS += kvp.Value * cachedDamages[kvp.Key];
                    for (int i = 0; i < NUM_SPELL_DETAILS; ++i)
                    {
                        spellDetails[i] += kvp.Value * cachedDetails[kvp.Key][i];
                    }
                }
                float damageDone = rot.DamageDone(talents, calcs, calcOpts.TreantLifespan, currentSpellPower, baseHit, currentCrit, currentHaste, currentMastery);
                accumulatedDPS += (1 - totalUpTime) * damageDone / rot.RotationData.Duration;
                spellDetails[0] += (1 - totalUpTime) * rot.RotationData.StarfireAvgHit;
                spellDetails[1] += (1 - totalUpTime) * rot.RotationData.WrathAvgHit;
                spellDetails[2] += (1 - totalUpTime) * rot.RotationData.MoonfireAvgHit;
                spellDetails[3] += (1 - totalUpTime) * rot.RotationData.InsectSwarmAvgHit;
                spellDetails[4] += (1 - totalUpTime) * rot.RotationData.StarSurgeAvgHit;
                spellDetails[5] += (1 - totalUpTime) * rot.RotationData.StarfireAvgCast;
                spellDetails[6] += (1 - totalUpTime) * rot.RotationData.WrathAvgCast;
                spellDetails[7] += (1 - totalUpTime) * rot.RotationData.MoonfireAvgCast;
                spellDetails[8] += (1 - totalUpTime) * rot.RotationData.InsectSwarmAvgCast;
                spellDetails[9] += (1 - totalUpTime) * rot.RotationData.StarSurgeAvgCast;
                spellDetails[10] += (1 - totalUpTime) * rot.RotationData.AverageInstantCast;
                spellDetails[11] += (1 - totalUpTime) * rot.RotationData.StarfireAvgEnergy;
                spellDetails[12] += (1 - totalUpTime) * rot.RotationData.WrathAvgEnergy;
                spellDetails[13] += (1 - totalUpTime) * rot.RotationData.StarSurgeAvgEnergy;
                spellDetails[14] += (1 - totalUpTime) * rot.RotationData.TreantDamage;
                spellDetails[15] += (1 - totalUpTime) * rot.RotationData.StarfallDamage;
                spellDetails[16] += (1 - totalUpTime) * rot.RotationData.MushroomDamage;

                accumulatedDamage += accumulatedDPS * rot.RotationData.Duration;

                // Movement - Sunfire/IS/Sfall/WM/Shooting Stars/trees
                Spell lunarShower = new Spell(Moonfire);
                lunarShower.AllDamageModifier *= (1 + 0.15f * talents.LunarShower) * (1 + (float)Math.Floor(calcs.EclipseBase * 100 + currentMastery * 2.0f) / 100f);
                lunarShower.BaseManaCost *= 1 - (0.3f * talents.LunarShower);
                rot.DoDotSpell(calcs, ref lunarShower, currentSpellPower, baseHit, currentCrit, currentHaste, 0.05f * talents.NaturesGrace, rot.RotationData.NaturesGraceUptime);
                float movementDPS = lunarShower.DamagePerHit / lunarShower.CastTime +
                    lunarShower.DotEffect.DamagePerHit / lunarShower.DotEffect.Duration +
                    rot.RotationData.InsectSwarmAvgHit / (rot.RotationData.InsectSwarmDuration == 0 ? 1 : rot.RotationData.InsectSwarmDuration);
                float movementManaPerSec = lunarShower.BaseManaCost / lunarShower.CastTime + rot.Solver.InsectSwarm.BaseManaCost / rot.RotationData.InsectSwarmAvgCast;

                // Multi-target - IS/Sunfire spam, SFall, Wild Mushroom

                float burstDPS = accumulatedDamage / rot.RotationData.Duration * percentTimeInRotation + movementDPS * movementShare;
                float sustainedDPS = burstDPS;

                // Mana calcs:
                // Main rotation - all spells
                // Movement rotation - Lunar Shower MF, IS, Shooting Stars procs, and Starfall only
                rot.RotationData.ManaGained += manaGained / (calcs.FightLength * 60.0f) * rot.RotationData.Duration;
                float timeToOOM = manaPool / ((rot.RotationData.ManaUsed - rot.RotationData.ManaGained) / rot.RotationData.Duration) * percentTimeInRotation +
                    (manaPool / movementManaPerSec) * movementShare;
                if (timeToOOM <= 0) timeToOOM = calcs.FightLength * 60.0f;   // Happens when ManaUsed is less than 0
                if (timeToOOM < calcs.FightLength * 60.0f)
                {
                    rot.RotationData.TimeToOOM = new TimeSpan(0, (int)(timeToOOM / 60), (int)(timeToOOM % 60));
                    sustainedDPS = burstDPS * timeToOOM / (calcs.FightLength * 60.0f);
                }
                
                burstDPS += trinketDPS;
                sustainedDPS += trinketDPS;

                rot.RotationData.SustainedDPS = sustainedDPS;
                rot.RotationData.BurstDPS = burstDPS;
                rot.RotationData.StarfireAvgHit = spellDetails[0];
                rot.RotationData.WrathAvgHit = spellDetails[1];
                rot.RotationData.MoonfireAvgHit = spellDetails[2];
                rot.RotationData.InsectSwarmAvgHit = spellDetails[3];
                rot.RotationData.StarSurgeAvgHit = spellDetails[4];
                rot.RotationData.StarfireAvgCast = spellDetails[5];
                rot.RotationData.WrathAvgCast = spellDetails[6];
                rot.RotationData.MoonfireAvgCast = spellDetails[7];
                rot.RotationData.InsectSwarmAvgCast = spellDetails[8];
                rot.RotationData.StarSurgeAvgCast = spellDetails[9];
                rot.RotationData.AverageInstantCast = spellDetails[10];
                rot.RotationData.StarfireAvgEnergy = spellDetails[11];
                rot.RotationData.WrathAvgEnergy = spellDetails[12];
                rot.RotationData.StarSurgeAvgEnergy = spellDetails[13];
                rot.RotationData.TreantDamage = spellDetails[14];
                rot.RotationData.StarfallDamage = spellDetails[15];
                rot.RotationData.MushroomDamage = spellDetails[16];

                // Update the sustained DPS rotation if any one of the following three cases is true:
                // 1) No user rotation is selected and sustained DPS is maximum
                // 2) A user rotation is selected, Eclipse is not present, and the user rotation matches the current rotation
                // 3) A user rotation is selected, Eclipse is present, and the user rotation's dot spells matches this rotation's
                if ((calcOpts.UserRotation == "None" && sustainedDPS > maxDamageDone) || rot.RotationData.Name == calcOpts.UserRotation)
                {
                    maxDamageDone = sustainedDPS;
                    maxRotation = rot;
                }
                if (burstDPS > maxBurstDamageDone)
                {
                    maxBurstDamageDone = burstDPS;
                    maxBurstRotation = rot;
                }
                cachedResults[rot.RotationData.Name] = rot.RotationData;

                // Deactivate always-up procs
                foreach (ProcEffect proc in alwaysUpEffects)
                {
                    proc.Deactivate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste, ref currentMastery);
                }
            }
            // Present the findings to the user.
            calcs.SelectedRotation = maxRotation.RotationData;
            calcs.BurstRotation = maxBurstRotation.RotationData;
            calcs.SubPoints = new float[] { maxBurstDamageDone, maxDamageDone };
            calcs.OverallPoints = calcs.SubPoints[0] + calcs.SubPoints[1];
            calcs.Rotations = cachedResults;
        }

        // Create proc effect calculations for proc-based trinkets.
        private void BuildProcList(CharacterCalculationsMoonkin calcs)
        {
            // Implement a new handler for each special effect in the calculations stats
            foreach (SpecialEffect effect in calcs.BasicStats.SpecialEffects())
            {
                procEffects.Add(new ProcEffect(effect));
            }
        }

        // Non-rotation-specific mana calculations
        private float GetEffectiveManaPool(Character character, CalculationOptionsMoonkin calcOpts, CharacterCalculationsMoonkin calcs)
        {
            float fightLength = calcs.FightLength * 60.0f;

            float innervateCooldown = 180;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen * fightLength;

            // Mana pot calculations
            float manaRestoredByPots = 0.0f;
            foreach (Buff b in character.ActiveBuffs)
            {
                if (b.Stats.ManaRestore > 0)
                {
                    manaRestoredByPots = b.Stats.ManaRestore;
                    break;
                }
            }

            // Innervate calculations
            float innervateDelay = calcOpts.InnervateDelay * 60.0f;
            int numInnervates = (calcOpts.Innervate && fightLength - innervateDelay > 0) ? ((int)(fightLength - innervateDelay) / (int)innervateCooldown + 1) : 0;
            float totalInnervateMana = numInnervates * 0.2f * calcs.BasicStats.Mana;
            totalInnervateMana *= 1 + 0.15f * character.DruidTalents.Dreamstate;

            // Replenishment calculations
            float replenishmentPerTick = calcs.BasicStats.Mana * calcs.BasicStats.ManaRestoreFromMaxManaPerSecond;
            float replenishmentMana = calcOpts.ReplenishmentUptime * replenishmentPerTick * calcs.FightLength * 60;

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen + manaRestoredByPots + replenishmentMana;
        }

        // Redo the spell calculations
        private void RecreateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            ResetSpellList();
            RecreateRotations();
            UpdateSpells(character, ref calcs);
        }

        private void RecreateRotations()
        {
            rotations = new List<SpellRotation>();
            rotations.Add(new SpellRotation() { RotationData = new RotationData() { Name = "None" } });
            for (int mfMode = 0; mfMode < 2; ++mfMode)
            {
                for (int isMode = 0; isMode < 2; ++isMode)
                {
                    for (int sfMode = 0; sfMode < 3; ++sfMode)
                    {
                        for (int wmMode = 0; wmMode < 3; ++wmMode)
                        {
                            DotMode mfModeEnum = (DotMode)mfMode;
                            DotMode isModeEnum = (DotMode)isMode;
                            StarfallMode sfModeEnum = (StarfallMode)sfMode;
                            MushroomMode wmModeEnum = (MushroomMode)wmMode;
                            string name = String.Format("MF {0} IS {1} SF {2} WM {3}",
                                mfModeEnum.ToString(),
                                isModeEnum.ToString(),
                                sfModeEnum.ToString(),
                                wmModeEnum.ToString());
                            rotations.Add(new SpellRotation()
                            {
                                RotationData = new RotationData()
                                {
                                    Name = name,
                                    MoonfireRefreshMode = mfModeEnum,
                                    InsectSwarmRefreshMode = isModeEnum,
                                    StarsurgeCastMode = StarsurgeMode.OnCooldown,
                                    StarfallCastMode = sfModeEnum,
                                    WildMushroomCastMode = wmModeEnum
                                }
                            });
                        }
                    }
                }
            }
        }

        // Add talented effects to the spells
        private void UpdateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            DruidTalents talents = character.DruidTalents;
            Stats stats = calcs.BasicStats;

            switch (talents.StarlightWrath)
            {
                case 1:
                    Starfire.BaseCastTime -= 0.1f;
                    Wrath.BaseCastTime -= 0.1f;
                    break;
                case 2:
                    Starfire.BaseCastTime -= 0.2f;
                    Wrath.BaseCastTime -= 0.2f;
                    break;
                case 3:
                    Starfire.BaseCastTime -= 0.5f;
                    Wrath.BaseCastTime -= 0.5f;
                    break;
                default:
                    break;
            }

            float moonfireDotGlyph = talents.GlyphOfMoonfire ? 0.2f : 0.0f;
            float insectSwarmGlyph = talents.GlyphOfInsectSwarm ? 0.3f : 0.0f;
            // Add spell-specific damage
            // Moonfire, Insect Swarm: glyphs
            Moonfire.DotEffect.AllDamageModifier *= 1 + moonfireDotGlyph;
            InsectSwarm.DotEffect.AllDamageModifier *= 1 + insectSwarmGlyph;
            // Moonfire: Direct damage +(0.03 * Blessing of the Grove)
            Moonfire.AllDamageModifier *= 1 + 0.03f * talents.BlessingOfTheGrove;
            // Moonfire, Insect Swarm: +2/4/6 seconds for Genesis
            Moonfire.DotEffect.BaseDuration += 2f * talents.Genesis;
            InsectSwarm.DotEffect.BaseDuration += 2f * talents.Genesis;
            // Wrath: 10% for glyph
            Wrath.AllDamageModifier *= 1 + (talents.GlyphOfWrath ? 0.1f : 0f);

            // Add spell-specific critical strike damage
            // Burning Shadowspirit Diamond
            float baseCritMultiplier = 1.5f * (1 + stats.BonusCritMultiplier);
            float moonfuryMultiplier = baseCritMultiplier + (baseCritMultiplier - 1);
            Starfire.CriticalDamageModifier = Wrath.CriticalDamageModifier = Moonfire.CriticalDamageModifier = InsectSwarm.CriticalDamageModifier = moonfuryMultiplier;
            Starsurge.CriticalDamageModifier = moonfuryMultiplier;

            // Reduce spell-specific mana costs
            // Shard of Woe (Mana cost -405)
            Starfire.BaseManaCost -= calcs.BasicStats.SpellsManaReduction;
            Moonfire.BaseManaCost -= calcs.BasicStats.SpellsManaReduction;
            Wrath.BaseManaCost -= calcs.BasicStats.SpellsManaReduction;
            InsectSwarm.BaseManaCost -= calcs.BasicStats.SpellsManaReduction;
            Starsurge.BaseManaCost -= calcs.BasicStats.SpellsManaReduction;
            // All spells: Mana cost -(0.03 * Moonglow)
            Starfire.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Moonfire.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Wrath.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            InsectSwarm.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Starsurge.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);

            // Add set bonuses
            UpdateSpellsFromSetBonuses(character.ActiveBuffs.FindAll(buff => buff.Group == "Set Bonuses"));

            // PTR changes go here
            if (calcs.PtrMode)
            {
                // 4.1 PTR: Starsurge damage -20% (base and coefficient)
                Starsurge.BaseDamage *= 0.8f;
                Starsurge.SpellDamageModifier *= 0.8f;
            }
        }

        /// <summary>
        /// Handle set bonuses without adding new stats to the base stats class.
        /// </summary>
        /// <param name="activeSetBonuses">A list of set bonuses that are active on the character</param>
        private void UpdateSpellsFromSetBonuses(List<Buff> activeSetBonuses)
        {
            foreach (Buff buff in activeSetBonuses)
            {
                switch (buff.SetName)
                {
                    // Tier 10: Lasherweave
                    case "Lasherweave Regalia":
                        switch (buff.SetThreshold)
                        {
                            // 4-piece bonus: 7% additional damage from crits
                            case 4:
                                Starfire.CriticalDamageModifier += 0.07f;
                                Wrath.CriticalDamageModifier += 0.07f;
                                break;
                        }
                        break;
                    // Tier 11: Stormrider's
                    case "Stormrider's Regalia":
                        switch (buff.SetThreshold)
                        {
                            // 2-piece bonus: 5% additional crit chance on dots
                            case 2:
                                Moonfire.CriticalChanceModifier += 0.05f;
                                InsectSwarm.CriticalChanceModifier += 0.05f;
                                break;
                        }
                        break;
                }
            }
        }
    }
}
