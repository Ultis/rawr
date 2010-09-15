using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    // The interface public class to the rest of Rawr.  Provides a single Solve method that runs all the calculations.
    public class MoonkinSolver
    {
        private const int NUM_SPELL_DETAILS = 13;
        // A list of all currently active proc effects.
        public List<ProcEffect> procEffects;
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
                            BaseDamage = (885 + 1103) / 2.0f,
                            SpellDamageModifier = 1.0f,
                            BaseCastTime = 3.5f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.16f),
                            DotEffect = null,
                            School = SpellSchool.Arcane,
                            BaseEnergy = 20
                        },
                        new Spell()
                        {
                            Name = "MF",
                            BaseDamage = (176.0f + 214.0f) / 2.0f,
                            SpellDamageModifier = 0.18f,
                            BaseCastTime = 1.5f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.18f),
                            DotEffect = new DotEffect()
                                {
                                    Duration = 12.0f,
                                    TickLength = 3.0f,
                                    TickDamage = 84.0f,
                                    SpellDamageModifierPerTick = 0.18f
                                },
                            School = SpellSchool.Arcane
                        },
                        new Spell()
                        {
                            Name = "W",
                            BaseDamage = (606f + 682.0f) / 2.0f,
                            SpellDamageModifier = 2.5f/3.5f,
                            BaseCastTime = 2.0f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.14f),
                            DotEffect = null,
                            School = SpellSchool.Nature,
                            BaseEnergy = 13
                        },
                        new Spell()
                        {
                            Name = "IS",
                            BaseDamage = 0.0f,
                            SpellDamageModifier = 0.0f,
                            BaseCastTime = 1.5f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.08f),
                            DotEffect = new DotEffect()
                            {
                                Duration = 12.0f,
                                TickLength = 2.0f,
                                TickDamage = 244.0f,
                                SpellDamageModifierPerTick = 0.26f
                            },
                            School = SpellSchool.Nature
                        },
                        new Spell()
                        {
                            Name = "SS",
                            BaseDamage = (1140 + 1574) / 2f,
                            SpellDamageModifier = 1.535f,
                            BaseCastTime = 2.0f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.11f),
                            DotEffect = null,
                            School = SpellSchool.Spellstorm,
                            BaseEnergy = 10,
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
        public List<SpellRotation> rotations = null;

        // Results data from the calculations, which will be sent to the UI.
        Dictionary<string, RotationData> cachedResults = new Dictionary<string, RotationData>();

        public float GetSpellHit(CharacterCalculationsMoonkin calcs)
        {
            float baseHit = 1.0f;
            switch (calcs.TargetLevel)
            {
                case 80:
                    baseHit -= 0.04f;
                    break;
                case 81:
                    baseHit -= 0.05f;
                    break;
                case 82:
                    baseHit -= 0.06f;
                    break;
                case 83:
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
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            DruidTalents talents = character.DruidTalents;
            procEffects = new List<ProcEffect>();
            RecreateSpells(talents, ref calcs);
            cachedResults = new Dictionary<string, RotationData>();

            float trinketDPS = 0.0f;
            float baseSpellPower = calcs.SpellPower;
            float baseHit = GetSpellHit(calcs);
            float baseCrit = calcs.SpellCrit;
            float baseHaste = calcs.SpellHaste;

            BuildProcList(calcs);

            float maxDamageDone = 0.0f;
            SpellRotation maxBurstRotation = rotations[0];
            SpellRotation maxRotation = rotations[0];

            float manaPool = GetEffectiveManaPool(character, calcOpts, calcs);

            // Do tree calculations: Calculate damage per cast.
            float treeDamage = (talents.ForceOfNature == 1) ? DoTreeCalcs(baseSpellPower, calcs.BasicStats.PhysicalHaste, calcs.BasicStats.PhysicalCrit, calcOpts.TreantLifespan) : 0.0f;
            // Extend that to number of casts per fight.
            float treeCasts = (float)Math.Floor(calcs.FightLength / 3) + 1.0f;
            // Partial cast: If the fight lasts 3.x minutes and x is less than 0.5 (30 sec tree duration), calculate a partial cast
            if ((int)calcs.FightLength % 3 == 0 && calcs.FightLength - (int)calcs.FightLength < 0.5)
                treeCasts += (calcs.FightLength - (int)calcs.FightLength) / 0.5f - 1.0f;
            treeDamage *= treeCasts;
            // Multiply by raid-wide damage increases.
            treeDamage *= (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusPhysicalDamageMultiplier);
            // Calculate the DPS averaged over the fight length.
            float treeDPS = treeDamage / (calcs.FightLength * 60.0f);
            // Calculate mana usage for trees.
            float treeManaUsage = (float)Math.Ceiling(treeCasts) * CalculationsMoonkin.BaseMana * 0.12f;
            manaPool -= talents.ForceOfNature == 1 ? treeManaUsage : 0.0f;

            // Do Starfall calculations.
            bool starfallGlyph = talents.GlyphOfStarfall;
            Buff tier102PieceBuff = character.ActiveBuffs.Find(theBuff => theBuff.Name == "Lasherweave Regalia (T10) 2 Piece Bonus");
            float numberOfStarHits = 10.0f;
            float starfallDamage = (talents.Starfall == 1) ? DoStarfallCalcs(baseSpellPower, baseHit, baseCrit,
                (1 + calcs.BasicStats.BonusDamageMultiplier) *
                (1 + calcs.BasicStats.BonusSpellPowerMultiplier) *
                (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) *
                (1 + (talents.GlyphOfFocus ? 0.1f : 0.0f)), Wrath.CriticalDamageModifier, out numberOfStarHits) : 0.0f;
            float starfallCD = 1.5f - (starfallGlyph ? 0.5f : 0.0f);
            float numStarfallCasts = (float)Math.Floor(calcs.FightLength / starfallCD) + 1.0f;
            // Partial cast: If the difference between fight length and total starfall CD time is less than 10 seconds (duration),
            // calculate a partial cast
            float starfallDiff = calcs.FightLength * 60.0f - (numStarfallCasts - 1) * starfallCD * 60.0f;
            if (starfallDiff > 0 && starfallDiff < 10)
                numStarfallCasts += starfallDiff / 60.0f / (1.0f / 6.0f) - 1.0f;
            starfallDamage *= numStarfallCasts;
            float starfallManaUsage = (float)Math.Ceiling(numStarfallCasts) * CalculationsMoonkin.BaseMana * 0.39f * (1 - 0.03f * talents.Moonglow);
            manaPool -= talents.Starfall == 1 ? starfallManaUsage : 0.0f;

            // Calculate effect of casting Starfall/Treants
            float globalCooldown = 1.5f / (1 + baseHaste) + calcs.Latency;
            float treantTime = (talents.ForceOfNature == 1) ? globalCooldown * (float)Math.Ceiling(treeCasts) : 0.0f;
            float starfallTime = (talents.Starfall == 1) ? globalCooldown * (float)Math.Ceiling(numStarfallCasts) : 0.0f;

            float totalTimeInRotation = calcs.FightLength * 60.0f - (treantTime + starfallTime);
            float percentTimeInRotation = totalTimeInRotation / (calcs.FightLength * 60.0f);
#if RAWR3 || RAWR4
            BossOptions bossOpts = character.BossOptions;
            if (bossOpts == null) bossOpts = new BossOptions();

            float fearShare = (bossOpts.FearingTargsDur / 1000) / bossOpts.FearingTargsFreq;
            float stunShare = (bossOpts.StunningTargsDur / 1000) / bossOpts.StunningTargsFreq;
            float invulnerableShare = bossOpts.TimeBossIsInvuln / bossOpts.BerserkTimer;

            List<Attack> attacks = bossOpts.GetFilteredAttackList(ATTACK_TYPES.AT_AOE);
            attacks.AddRange(bossOpts.GetFilteredAttackList(ATTACK_TYPES.AT_RANGED));
            int movementCount = attacks.Count;
            float assumedMovementDuration = 2.0f;   // Assume 2 seconds per move
            float accumulatedDurations = 0.0f;
            foreach (Attack a in attacks)
                accumulatedDurations += a.AttackSpeed;

            float movementShare = (movementCount == 0 ? 0 : assumedMovementDuration / (accumulatedDurations / movementCount) / (1 + calcs.BasicStats.MovementSpeed));

            percentTimeInRotation -= movementShare + fearShare + stunShare + invulnerableShare;
#endif

            float manaGained = manaPool - calcs.BasicStats.Mana;

            float oldArcaneMultiplier = calcs.BasicStats.BonusArcaneDamageMultiplier;
            float oldNatureMultiplier = calcs.BasicStats.BonusNatureDamageMultiplier;

            foreach (SpellRotation rot in rotations)
            {
                rot.Solver = this;
                float accumulatedDamage = 0.0f;
                float totalUpTime = 0.0f;
                float[] spellDetails = new float[NUM_SPELL_DETAILS];
                List<ProcEffect> activatedEffects = new List<ProcEffect>();
                List<ProcEffect> alwaysUpEffects = new List<ProcEffect>();

                // Pre-calculate rotational variables with base stats
                rot.DamageDone(talents, calcs, baseSpellPower, baseHit, baseCrit, baseHaste);

                // Reset variables modified in the pre-loop to base values
                float currentSpellPower = baseSpellPower;
                float currentCrit = baseCrit;
                float currentHaste = baseHaste;
                calcs.BasicStats.BonusArcaneDamageMultiplier = oldArcaneMultiplier;
                calcs.BasicStats.BonusNatureDamageMultiplier = oldNatureMultiplier;
                // Calculate spell power/spell damage modifying trinkets in a separate pre-loop
                foreach (ProcEffect proc in procEffects)
                {
                    if (proc.Effect.Stats.SpellPower > 0)
                    {
                        float procSpellPower = proc.Effect.Stats.SpellPower;

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
                            case Trigger.InsectSwarmOrMoonfireTick:
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
                            currentSpellPower += (proc.Effect.MaxStack > 1 ? proc.Effect.GetAverageStackSize(triggerInterval, triggerChance, 3.0f, calcs.FightLength * 60.0f) : 1) *
                            proc.Effect.GetAverageUptime(triggerInterval, triggerChance) * procSpellPower;
                    }
                    // 2T10 (both if statements, which is why it isn't else-if)
                    if (proc.Effect.Stats.BonusArcaneDamageMultiplier > 0)
                    {
                        calcs.BasicStats.BonusArcaneDamageMultiplier += proc.Effect.GetAverageUptime(rot.RotationData.Duration / rot.RotationData.CastCount, 1f) * proc.Effect.Stats.BonusArcaneDamageMultiplier;
                    }
                    if (proc.Effect.Stats.BonusNatureDamageMultiplier > 0)
                    {
                        calcs.BasicStats.BonusNatureDamageMultiplier += proc.Effect.GetAverageUptime(rot.RotationData.Duration / rot.RotationData.CastCount, 1f) * proc.Effect.Stats.BonusNatureDamageMultiplier;
                    }
                    if (proc.Effect.Stats._rawSpecialEffectDataSize > 0)
                    {
                        SpecialEffect childEffect = proc.Effect.Stats._rawSpecialEffectData[0];
                        // Nevermelting Ice Crystal
                        if (childEffect.Stats.CritRating != 0)
                        {
                            float maxStack = proc.Effect.Stats.CritRating;
                            float numNegativeStacks = childEffect.GetAverageStackSize(rot.RotationData.Duration / (rot.RotationData.CastCount - rot.RotationData.InsectSwarmCasts), baseCrit, 3.0f, proc.Effect.Duration);
                            float averageNegativeValue = childEffect.Stats.CritRating * numNegativeStacks;
                            float averageCritRating = maxStack + averageNegativeValue;
                            currentCrit += StatConversion.GetSpellCritFromRating(averageCritRating * proc.Effect.GetAverageUptime(0f, 1f));
                        }
                        // Fetish of Volatile Power
                        else if (childEffect.Stats.HasteRating != 0)
                        {
                            currentHaste += StatConversion.GetSpellHasteFromRating(childEffect.Stats.HasteRating * childEffect.GetAverageStackSize(rot.RotationData.Duration / rot.RotationData.CastCount, 1f, 3.0f, proc.Effect.Duration) * proc.Effect.GetAverageUptime(0f, 1f));
                        }
                    }
                }
                // Calculate damage and mana contributions for non-stat-boosting trinkets
                // Separate stat-boosting proc trinkets into their own list
                foreach (ProcEffect proc in procEffects)
                {
                    if (proc.CalculateDPS != null)
                    {
                        accumulatedDamage += proc.CalculateDPS(rot, calcs, currentSpellPower, baseHit, currentCrit, currentHaste) * rot.RotationData.Duration;
                    }
                    else if (proc.Activate != null)
                    {
                        float upTime = proc.UpTime(rot, calcs);
                        // Procs with 100% uptime should be activated and not put into the combination loop
                        if (upTime == 1)
                        {
                            alwaysUpEffects.Add(proc);
                            proc.Activate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste);
                        }
                        // Procs with uptime 0 < x < 100 should be activated
                        else if (upTime > 0)
                            activatedEffects.Add(proc);
                    }
                    else if (proc.CalculateMP5 != null)
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
                            activatedEffects[idx].Activate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste);
                        }
                        float tempDPS = rot.DamageDone(talents, calcs, currentSpellPower, baseHit, currentCrit, currentHaste) / rot.RotationData.Duration;
                        spellDetails[0] = Starfire.DamagePerHit;
                        spellDetails[1] = Wrath.DamagePerHit;
                        spellDetails[2] = Moonfire.DamagePerHit + Moonfire.DotEffect.DamagePerHit;
                        spellDetails[3] = InsectSwarm.DotEffect.DamagePerHit;
                        spellDetails[4] = Starsurge.DamagePerHit;
                        spellDetails[5] = Starfire.CastTime;
                        spellDetails[6] = Wrath.CastTime;
                        spellDetails[7] = Starsurge.CastTime;
                        spellDetails[8] = Moonfire.CastTime;
                        spellDetails[9] = Starfire.AverageEnergy;
                        spellDetails[10] = Wrath.AverageEnergy;
                        spellDetails[11] = Starsurge.AverageEnergy;
                        foreach (int idx in vals)
                        {
                            tempUpTime *= activatedEffects[idx].UpTime(rot, calcs);
                            activatedEffects[idx].Deactivate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste);
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
                float damageDone = rot.DamageDone(talents, calcs, currentSpellPower, baseHit, currentCrit, currentHaste);
                accumulatedDPS += (1 - totalUpTime) * damageDone / rot.RotationData.Duration;
                spellDetails[0] += (1 - totalUpTime) * Starfire.DamagePerHit;
                spellDetails[1] += (1 - totalUpTime) * Wrath.DamagePerHit;
                spellDetails[2] += (1 - totalUpTime) * Moonfire.DamagePerHit + Moonfire.DotEffect.DamagePerHit;
                spellDetails[3] += (1 - totalUpTime) * InsectSwarm.DotEffect.DamagePerHit;
                spellDetails[4] += (1 - totalUpTime) * Starsurge.DamagePerHit;
                spellDetails[5] += (1 - totalUpTime) * Starfire.CastTime;
                spellDetails[6] += (1 - totalUpTime) * Wrath.CastTime;
                spellDetails[7] += (1 - totalUpTime) * Starsurge.CastTime;
                spellDetails[8] += (1 - totalUpTime) * Moonfire.CastTime;
                spellDetails[9] += (1 - totalUpTime) * Starfire.AverageEnergy;
                spellDetails[10] += (1 - totalUpTime) * Wrath.AverageEnergy;
                spellDetails[11] += (1 - totalUpTime) * Starsurge.AverageEnergy;

                accumulatedDamage += accumulatedDPS * rot.RotationData.Duration;

                float burstDPS = accumulatedDamage / rot.RotationData.Duration * percentTimeInRotation;
                float sustainedDPS = burstDPS;
                float timeToOOM = (manaPool / rot.RotationData.ManaUsed) * rot.RotationData.Duration;
                if (timeToOOM <= 0) timeToOOM = calcs.FightLength * 60.0f;   // Happens when ManaUsed is less than 0
                if (timeToOOM < calcs.FightLength * 60.0f)
                {
                    rot.RotationData.TimeToOOM = new TimeSpan(0, (int)(timeToOOM / 60), (int)(timeToOOM % 60));
                    sustainedDPS = burstDPS * timeToOOM / (calcs.FightLength * 60.0f);
                }
                float t10StarfallDamage = starfallDamage;
                // Approximate the effect of the 2T10 set bonus
                if (tier102PieceBuff != null)
                {
                    Stats.SpecialEffectEnumerator enumerator = tier102PieceBuff.Stats.SpecialEffects();
                    enumerator.MoveNext();
                    SpecialEffect effect = enumerator.Current;
                    float upTime = effect.GetAverageUptime(rot.RotationData.Duration / rot.RotationData.CastCount, 1f);
                    t10StarfallDamage = upTime * (starfallDamage * (1 + effect.Stats.BonusArcaneDamageMultiplier)) + (1 - upTime) * starfallDamage;
                }
                float starfallDPS = t10StarfallDamage / (calcs.FightLength * 60.0f);
                burstDPS += trinketDPS + treeDPS + starfallDPS;
                sustainedDPS += trinketDPS + treeDPS + starfallDPS;

                rot.RotationData.StarfallDamage = t10StarfallDamage / numStarfallCasts;
                rot.RotationData.StarfallStars = numberOfStarHits;
                rot.RotationData.DPS = sustainedDPS;
                rot.RotationData.StarfireAvgHit = spellDetails[0];
                rot.RotationData.WrathAvgHit = spellDetails[1];
                rot.RotationData.MoonfireAvgHit = spellDetails[2];
                rot.RotationData.InsectSwarmAvgHit = spellDetails[3];
                rot.RotationData.StarSurgeAvgHit = spellDetails[4];
                rot.RotationData.StarfireAvgCast = spellDetails[5];
                rot.RotationData.WrathAvgCast = spellDetails[6];
                rot.RotationData.StarSurgeAvgCast = spellDetails[7];
                rot.RotationData.AverageInstantCast = spellDetails[8];
                rot.RotationData.StarfireAvgEnergy = spellDetails[9];
                rot.RotationData.WrathAvgEnergy = spellDetails[10];
                rot.RotationData.StarSurgeAvgEnergy = spellDetails[11];

                // Update the sustained DPS rotation if any one of the following three cases is true:
                // 1) No user rotation is selected and sustained DPS is maximum
                // 2) A user rotation is selected, Eclipse is not present, and the user rotation matches the current rotation
                // 3) A user rotation is selected, Eclipse is present, and the user rotation's dot spells matches this rotation's
                if (sustainedDPS > maxDamageDone)
                {
                    maxDamageDone = sustainedDPS;
                    maxRotation = rot;
                }
                rot.RotationData.ManaGained += manaGained / (calcs.FightLength * 60.0f) * rot.RotationData.Duration;
                cachedResults[rot.RotationData.Name] = rot.RotationData;

                // Deactivate always-up procs
                foreach (ProcEffect proc in alwaysUpEffects)
                {
                    proc.Deactivate(character, calcs, ref currentSpellPower, ref baseHit, ref currentCrit, ref currentHaste);
                }
                rot.RotationData.TreantDamage = treeDamage / treeCasts;
            }
            // Present the findings to the user.
            calcs.SelectedRotation = maxRotation.RotationData;
            calcs.SubPoints = new float[] { maxDamageDone };
            calcs.OverallPoints = calcs.SubPoints[0];
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

            float innervateCooldown = 360 - calcs.BasicStats.InnervateCooldownReduction;

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
            float totalInnervateMana = numInnervates * calcs.BasicStats.Mana * (0.2f * (character.DruidTalents.GlyphOfInnervate ? 1.5f : 1.0f));

            // Replenishment calculations
            float replenishmentPerTick = calcs.BasicStats.Mana * calcs.BasicStats.ManaRestoreFromMaxManaPerSecond;
            float replenishmentMana = calcOpts.ReplenishmentUptime * replenishmentPerTick * calcs.FightLength * 60;

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen + manaRestoredByPots + replenishmentMana;
        }

        // Now returns damage per cast to allow adjustments for fight length
        private float DoTreeCalcs(float effectiveNatureDamage, float meleeHaste, float meleeCrit, float treantLifespan)
        {
            // 642 = base AP, 57% spell power scaling
            float attackPower = 642.0f + (float)Math.Floor(0.57f * effectiveNatureDamage);
            // 398.8 = base DPS, 1.7 = best observed swing speed
            float damagePerHit = (398.8f + attackPower / 14.0f) * 1.7f;
            float critRate = 0.05f + meleeCrit;
            float glancingRate = 0.2f;
            float bossArmor = StatConversion.NPC_ARMOR[83 - 80];
            float damageReduction = bossArmor / (bossArmor + 15232.5f);
            damagePerHit *= 1.0f - damageReduction;
            damagePerHit = (critRate * damagePerHit * 2.0f) + (glancingRate * damagePerHit * 0.75f) + ((1 - critRate - glancingRate) * damagePerHit);
            float attackSpeed = 1.7f / (1 + meleeHaste);
            float damagePerTree = (treantLifespan * 30.0f / attackSpeed) * damagePerHit;
            return 3 * damagePerTree;
        }

        // Starfall
        private float DoStarfallCalcs(float effectiveArcaneDamage, float spellHit, float spellCrit, float hitDamageModifier, float critDamageModifier, out float numberOfStarHits)
        {
            float baseDamagePerStar = (331.0f + 383.0f) / 2.0f;
            float mainStarCoefficient = 0.247f;
            float baseSplashDamage = 101.0f;
            float splashCoefficient = 0.127f;

            // TODO: Right now, calculating single-target only, which is 10 stars with splash damage.
            // AOE situations gives 20 stars.
            // CORRECTION 2010/06/12: single-target damage DOES do splash, if the star hits.

            float damagePerBigStarHit = (baseDamagePerStar + effectiveArcaneDamage * mainStarCoefficient) * hitDamageModifier;
            float damagePerSmallStarHit = (baseSplashDamage + effectiveArcaneDamage * splashCoefficient) * hitDamageModifier;

            float critDamagePerBigStarHit = damagePerBigStarHit * critDamageModifier;
            float critDamagePerSmallStarHit = damagePerSmallStarHit * critDamageModifier;

            float averageDamagePerBigStar = spellCrit * critDamagePerBigStarHit + (1 - spellCrit) * damagePerBigStarHit;
            float averageDamagePerSmallStar = spellCrit * critDamagePerSmallStarHit + (1 - spellCrit) * damagePerSmallStarHit;

            numberOfStarHits = 10.0f;

            float avgNumBigStarsHit = spellHit * numberOfStarHits;
            float avgNumSmallStarsHit = avgNumBigStarsHit * spellHit;

            return avgNumBigStarsHit * averageDamagePerBigStar + avgNumSmallStarsHit * averageDamagePerSmallStar;
        }

        // Redo the spell calculations
        private void RecreateSpells(DruidTalents talents, ref CharacterCalculationsMoonkin calcs)
        {
            ResetSpellList();

            rotations = new List<SpellRotation>()
            {
                new SpellRotation()
                {
                    RotationData = new RotationData()
                    {
                        Name = "Maintain Solar"
                    }
                },
                new SpellRotation()
                {
                    RotationData = new RotationData()
                    {
                        Name = "Proc Alternating"
                    }
                }
            };

            UpdateSpells(talents, ref calcs);
        }

        // Add talented effects to the spells
        private void UpdateSpells(DruidTalents talents, ref CharacterCalculationsMoonkin calcs)
        {
            Stats stats = calcs.BasicStats;

            switch (talents.StarlightWrath)
            {
                case 1:
                    Starfire.BaseCastTime -= 0.15f;
                    Wrath.BaseCastTime -= 0.15f;
                    Starsurge.BaseCastTime -= 0.15f;
                    break;
                case 2:
                    Starfire.BaseCastTime -= 0.25f;
                    Wrath.BaseCastTime -= 0.25f;
                    Starsurge.BaseCastTime -= 0.25f;
                    break;
                case 3:
                    Starfire.BaseCastTime -= 0.5f;
                    Wrath.BaseCastTime -= 0.5f;
                    Starsurge.BaseCastTime -= 0.5f;
                    break;
                default:
                    break;
            }

            float moonfireDotGlyph = talents.GlyphOfMoonfire ? 0.2f : 0.0f;
            float insectSwarmGlyph = talents.GlyphOfInsectSwarm ? 0.3f : 0.0f;
            // Add spell-specific damage
            // Moonfire, Insect Swarm: Dot Damage +(0.02 * Genesis) (Additive with Genesis/Glyph/Set bonus)
            Moonfire.DotEffect.AllDamageModifier *= 1 + 0.02f * talents.Genesis
                                                      + moonfireDotGlyph;
            InsectSwarm.DotEffect.AllDamageModifier *= 1 + 0.01f * talents.Genesis
                                                         + insectSwarmGlyph;

            // Add spell-specific critical strike damage
            // Chaotic Skyflare Diamond
            float baseCritMultiplier = 1.5f * (1 + stats.BonusCritMultiplier);
            float moonfuryMultiplier = baseCritMultiplier + (baseCritMultiplier - 1);   // TODO: Only active when Moonkin specialization
            Starfire.CriticalDamageModifier = Wrath.CriticalDamageModifier = Moonfire.CriticalDamageModifier = InsectSwarm.CriticalDamageModifier = moonfuryMultiplier;
            Starsurge.CriticalDamageModifier = baseCritMultiplier;

            // Reduce spell-specific mana costs
            // All spells: Mana cost -(0.03 * Moonglow)
            Starfire.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Moonfire.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Wrath.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            InsectSwarm.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Starsurge.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);

            // Energy bonuses
            Starfire.CriticalEnergy += 4 * talents.Euphoria;
            Wrath.CriticalEnergy += 2 * talents.Euphoria;

            Starsurge.BaseEnergy += 3 * talents.LunarGuidance;

            // Add set bonuses
            // 2T6
            Moonfire.DotEffect.Duration += stats.MoonfireExtension;
            // 4T6
            Starfire.CriticalChanceModifier += stats.StarfireCritChance;
            // 4T7
            Starfire.CriticalChanceModifier += stats.BonusNukeCritChance;
            Wrath.CriticalChanceModifier += stats.BonusNukeCritChance;

        }

    }
}