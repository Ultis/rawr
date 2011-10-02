using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    public enum StarfallMode { Unused, LunarOnly, OnCooldown };
    // Rotation information for display to the user.
    public class RotationData
    {
        #region Inputs
        public string Name { get; set; }
        public StarfallMode StarfallCastMode { get; set; }
        #endregion
        #region Outputs
        public float SustainedDPS = 0.0f;
        public float BurstDPS = 0.0f;
        public float DPM = 0.0f;
        public TimeSpan TimeToOOM = new TimeSpan(0, 0, 0);
        public float AverageInstantCast { get; set; }
        public float Duration { get; set; }
        public float ManaUsed { get; set; }
        public float ManaGained { get; set; }
        public float CastCount { get; set; }
        public float DotTicks { get; set; }
        public float WrathCount { get; set; }
        public float WrathAvgHit { get; set; }
        public float WrathAvgCast { get; set; }
        public float WrathAvgEnergy { get; set; }
        public float StarfireCount { get; set; }
        public float StarfireAvgHit { get; set; }
        public float StarfireAvgCast { get; set; }
        public float StarfireAvgEnergy { get; set; }
        public float StarSurgeCount { get; set; }
        public float StarSurgeAvgHit { get; set; }
        public float StarSurgeAvgCast { get; set; }
        public float StarSurgeAvgEnergy { get; set; }
        public float InsectSwarmTicks { get; set; }
        public float InsectSwarmCasts { get; set; }
        public float InsectSwarmAvgHit { get; set; }
        public float InsectSwarmAvgCast { get; set; }
        public float InsectSwarmDuration { get; set; }
        public float MoonfireCasts { get; set; }
        public float MoonfireTicks { get; set; }
        public float MoonfireAvgHit { get; set; }
        public float MoonfireAvgCast { get; set; }
        public float MoonfireDuration { get; set; }
        public float StarfallCasts { get; set; }
        public float StarfallDamage { get; set; }
        public float StarfallStars { get; set; }
        public float MushroomCasts { get; set; }
        public float MushroomDamage { get; set; }
        public float TreantCasts { get; set; }
        public float TreantDamage { get; set; }
        public float SolarUptime { get; set; }
        public float LunarUptime { get; set; }
        public float NaturesGraceUptime { get; set; }
        #endregion
    }

    // Our old friend the spell rotation.
    public class SpellRotation
    {
        public MoonkinSolver Solver { get; set; }
        public RotationData RotationData = new RotationData();

        public override string ToString()
        {
            return RotationData.Name;
        }

        // Calculate damage and casting time for a single, direct-damage spell.
        public void DoMainNuke(CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste, float naturesGraceUptime, float latency)
        {
            float naturesGraceBonusHaste = 0.15f;

            float overallDamageModifier = mainNuke.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
            // Add a check for the higher of the two spell schools, as Starsurge always chooses the higher one
            overallDamageModifier *= mainNuke.School == SpellSchool.Arcane ? (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) :
                (mainNuke.School == SpellSchool.Nature ? (1 + calcs.BasicStats.BonusNatureDamageMultiplier) :
                (1 + (calcs.BasicStats.BonusArcaneDamageMultiplier > calcs.BasicStats.BonusNatureDamageMultiplier ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier)));

            float gcd = 1.5f / (1.0f + spellHaste);
            float ngGcd = gcd / (1 + naturesGraceBonusHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;
            float instantCastNG = (float)Math.Max(ngGcd, 1.0f) + latency;

            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            float baseCastTime = (float)Math.Max(mainNuke.BaseCastTime / (1 + spellHaste), instantCast);
            float ngCastTime = (float)Math.Max(mainNuke.BaseCastTime / (1 + spellHaste) / (1 + naturesGraceBonusHaste), instantCastNG);
            mainNuke.CastTime = (1 - naturesGraceUptime) * baseCastTime + naturesGraceUptime * ngCastTime;
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * spellPower) * overallDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier;
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
            mainNuke.AverageEnergy = mainNuke.BaseEnergy;
        }

        // Calculate damage and casting time for a damage-over-time effect.
        public void DoDotSpell(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste, float naturesGraceUptime, float latency)
        {
            float naturesGraceBonusHaste = 0.15f;

            float schoolMultiplier = dotSpell.School == SpellSchool.Arcane ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier;

            float overallDamageModifier = dotSpell.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier);

            float dotEffectDamageModifier = dotSpell.DotEffect.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier);

            float gcd = 1.5f / (1.0f + spellHaste);
            float ngGcd = gcd / (1 + naturesGraceBonusHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;
            float instantCastNG = (float)Math.Max(ngGcd, 1.0f) + latency;
            dotSpell.CastTime = naturesGraceUptime * instantCastNG + (1 - naturesGraceUptime) * instantCast;

            // Flatly calculated tick rate
            float baseTickRate = dotSpell.DotEffect.BaseTickLength / (1 + spellHaste);
            float ngTickRate = baseTickRate / (1 + naturesGraceBonusHaste);

            // Round the tick rate to the nearest millisecond
            baseTickRate = ((int)Math.Floor(baseTickRate * 1000 + 0.5f)) / 1000f;
            ngTickRate = ((int)Math.Floor(ngTickRate * 1000 + 0.5f)) / 1000f;

            // Round the number of ticks up if > .5, down if <= .5
            int baseTicks = (int)Math.Ceiling((dotSpell.DotEffect.BaseDuration / baseTickRate) - 0.5f);
            int ngTicks = (int)Math.Ceiling((dotSpell.DotEffect.BaseDuration / ngTickRate) - 0.5f);

            dotSpell.DotEffect.NumberOfTicks = naturesGraceUptime * ngTicks + (1 - naturesGraceUptime) * baseTicks;

            float baseDuration = baseTickRate * baseTicks;
            float ngDuration = ngTickRate * ngTicks;

            dotSpell.DotEffect.Duration = naturesGraceUptime * ngDuration + (1 - naturesGraceUptime) * baseDuration;

            dotSpell.DotEffect.TickLength = (1 - naturesGraceUptime) * baseTickRate + naturesGraceUptime * ngTickRate;

            float mfDirectDamage = (dotSpell.BaseDamage + dotSpell.SpellDamageModifier * spellPower) * overallDamageModifier;
            float mfCritDamage = mfDirectDamage * dotSpell.CriticalDamageModifier;
            float totalCritChance = spellCrit + dotSpell.CriticalChanceModifier;
            dotSpell.DamagePerHit = (totalCritChance * mfCritDamage + (1 - totalCritChance) * mfDirectDamage) * spellHit;
            float normalDamagePerTick = dotSpell.DotEffect.TickDamage + dotSpell.DotEffect.SpellDamageModifierPerTick * spellPower;
            float critDamagePerTick = normalDamagePerTick * dotSpell.CriticalDamageModifier;
            float damagePerTick = (totalCritChance * critDamagePerTick + (1 - totalCritChance) * normalDamagePerTick) * dotEffectDamageModifier;
            dotSpell.DotEffect.DamagePerHit = dotSpell.DotEffect.NumberOfTicks * damagePerTick * spellHit;
            dotSpell.AverageEnergy = dotSpell.BaseEnergy;
        }

        private float DoMushroomCalcs(CharacterCalculationsMoonkin calcs, float effectiveNatureDamage, float spellHit, float spellCrit)
        {
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
            float critDamageModifier = 1.5f * (1 + calcs.BasicStats.BonusCritDamageMultiplier);
            // 845-1022 damage
            float baseDamage = (845 + 1022) / 2;
            float damagePerHit = (baseDamage + effectiveNatureDamage * 0.6032f) * hitDamageModifier;
            float damagePerCrit = damagePerHit * critDamageModifier;
            return spellHit * (damagePerHit * (1 - spellCrit) + damagePerCrit * spellCrit);
        }

        // Now returns damage per cast to allow adjustments for fight length
        private float DoTreeCalcs(CharacterCalculationsMoonkin calcs, int playerLevel, int bossLevel, float effectiveNatureDamage, float treantLifespan)
        {
            float sunderPercent = calcs.BasicStats.TargetArmorReduction;
            float meleeHit = calcs.SpellHit * (StatConversion.WHITE_MISS_CHANCE_CAP[bossLevel - playerLevel] / StatConversion.GetSpellMiss(playerLevel - bossLevel, false));
            float physicalDamageMultiplierBonus = (1f + calcs.BasicStats.BonusDamageMultiplier) * (1f + calcs.BasicStats.BonusPhysicalDamageMultiplier);
            float physicalDamageMultiplierReduc = (1f - calcs.BasicStats.DamageTakenReductionMultiplier) * (1f - calcs.BasicStats.PhysicalDamageTakenReductionMultiplier);
            // 932 = base AP, 57% spell power scaling
            float attackPower = 932.0f + (float)Math.Floor(0.57f * effectiveNatureDamage);
            // 1.65 s base swing speed
            float baseAttackSpeed = 1.65f;
            float attackSpeed = baseAttackSpeed / (1 + calcs.BasicStats.PhysicalHaste);
            // 580 = base DPS
            float damagePerHit = (580f + attackPower / 14.0f) * baseAttackSpeed;
            // 5% base crit rate, inherit crit debuffs
            // Remove crit depression, as it doesn't appear to have an effect (unless it's base ~10% crit rate)
            float critRate = 0.05f;
            // White hit glancing rate
            float glancingRate = StatConversion.WHITE_GLANCE_CHANCE_CAP[bossLevel - playerLevel];
            // Hit rate determined by the amount of melee hit, not by spell hit
            float missRate = Math.Max(0f, StatConversion.WHITE_MISS_CHANCE_CAP[bossLevel - playerLevel] - meleeHit);
            // Since the trees inherit expertise from their hit, scale their hit rate such that when they are hit capped, they are expertise capped
            float dodgeRate = Math.Max(0f, StatConversion.WHITE_DODGE_CHANCE_CAP[bossLevel - playerLevel] * (missRate / StatConversion.WHITE_MISS_CHANCE_CAP[bossLevel - playerLevel]));
            // Armor damage reduction, including Sunder
            float damageReduction = StatConversion.GetArmorDamageReduction(playerLevel, StatConversion.NPC_ARMOR[bossLevel - playerLevel] * (1f - sunderPercent), 0, 0);
            // Final normal damage per swing
            damagePerHit *= 1.0f - damageReduction;
            damagePerHit *= physicalDamageMultiplierReduc;
            damagePerHit *= physicalDamageMultiplierBonus;
            // Damage per swing, including crits/glances/misses
            // This is a cheesy approximation of a true combat table, but because crit/miss/dodge rates will all be fairly low, I don't need to do the whole thing
            damagePerHit = (critRate * damagePerHit * 2.0f) + (glancingRate * damagePerHit * 0.75f) + ((1 - critRate - glancingRate - missRate - dodgeRate) * damagePerHit);
            // Total damage done in their estimated lifespan
            float damagePerTree = (treantLifespan * 30.0f / attackSpeed) * damagePerHit;
            return 3 * damagePerTree;
        }

        // Starfall
        private float DoStarfallCalcs(CharacterCalculationsMoonkin calcs, float effectiveArcaneDamage, float spellHit, float spellCrit)
        {
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier);
            // Starfall is affected by Moonfury
            float critDamageModifier = 1.5f * (1 + calcs.BasicStats.BonusCritDamageMultiplier) + (1.5f * (1 + calcs.BasicStats.BonusCritDamageMultiplier) - 1);
            float baseDamagePerStar = (370.0f + 428.0f) / 2.0f;
            float mainStarCoefficient = 0.247f;

            float damagePerBigStarHit = (baseDamagePerStar + effectiveArcaneDamage * mainStarCoefficient) * hitDamageModifier;

            float critDamagePerBigStarHit = damagePerBigStarHit * critDamageModifier;

            float averageDamagePerBigStar = spellCrit * critDamagePerBigStarHit + (1 - spellCrit) * damagePerBigStarHit;

            float numberOfStarHits = 10f;

            float avgNumBigStarsHit = spellHit * numberOfStarHits;

            return avgNumBigStarsHit * averageDamagePerBigStar;
        }

        private double GetInterpolatedCastTime(float actualHaste, bool use4T12Table, bool use4T13Table, bool useGoSFTable)
        {
            // Get index and remainder for interpolation
            double r = actualHaste / 0.05;
            int i = (int)r;
            r -= i;

            // If we're out of bounds, clip to the edge of the table
            if (i + 1 >= 21)
            {
                i = 19;
                r = 1;
            }

            // Index the table and interpolate the remainder
            if (use4T12Table)
            {
                if (useGoSFTable)
                    return MoonkinSolver.T12RotationDurationsGoSF[i] + r * (MoonkinSolver.T12RotationDurationsGoSF[i + 1] - MoonkinSolver.T12RotationDurationsGoSF[i]);
                else
                    return MoonkinSolver.T12RotationDurations[i] + r * (MoonkinSolver.T12RotationDurations[i + 1] - MoonkinSolver.T12RotationDurations[i]);
            }
            else if (use4T13Table)
            {
                if (useGoSFTable)
                    return MoonkinSolver.T13RotationDurationsGoSF[i] + r * (MoonkinSolver.T13RotationDurationsGoSF[i + 1] - MoonkinSolver.T13RotationDurationsGoSF[i]);
                else
                    return MoonkinSolver.T13RotationDurations[i] + r * (MoonkinSolver.T13RotationDurations[i + 1] - MoonkinSolver.T13RotationDurations[i]);
            }
            else
            {
                if (useGoSFTable)
                    return MoonkinSolver.BaseRotationDurationsGoSF[i] + r * (MoonkinSolver.BaseRotationDurationsGoSF[i + 1] - MoonkinSolver.BaseRotationDurationsGoSF[i]);
                else
                    return MoonkinSolver.BaseRotationDurations[i] + r * (MoonkinSolver.BaseRotationDurations[i + 1] - MoonkinSolver.BaseRotationDurations[i]);
            }
        }

        private double GetInterpolatedNGUpime(float actualHaste, bool use4T12Table, bool use4T13Table, bool useGoSFTable)
        {
            // Get index and remainder for interpolation
            double r = actualHaste / 0.05;
            int i = (int)r;
            r -= i;

            // If we're out of bounds, clip to the edge of the table
            if (i + 1 >= 21)
            {
                i = 19;
                r = 1;
            }

            // Index the table and interpolate the remainder
            if (use4T12Table)
            {
                if (useGoSFTable)
                    return MoonkinSolver.T12NGUptimesGoSF[i] + r * (MoonkinSolver.T12NGUptimesGoSF[i + 1] - MoonkinSolver.T12NGUptimesGoSF[i]);
                else
                    return MoonkinSolver.T12NGUptimes[i] + r * (MoonkinSolver.T12NGUptimes[i + 1] - MoonkinSolver.T12NGUptimes[i]);
            }
            else if (use4T13Table)
            {
                if (useGoSFTable)
                    return MoonkinSolver.T13NGUptimesGoSF[i] + r * (MoonkinSolver.T13NGUptimesGoSF[i + 1] - MoonkinSolver.T13NGUptimesGoSF[i]);
                else
                    return MoonkinSolver.T13NGUptimes[i] + r * (MoonkinSolver.T13NGUptimes[i + 1] - MoonkinSolver.T13NGUptimes[i]);
            }
            else
            {
                if (useGoSFTable)
                    return MoonkinSolver.BaseNGUptimesGoSF[i] + r * (MoonkinSolver.BaseNGUptimesGoSF[i + 1] - MoonkinSolver.BaseNGUptimesGoSF[i]);
                else
                    return MoonkinSolver.BaseNGUptimes[i] + r * (MoonkinSolver.BaseNGUptimes[i + 1] - MoonkinSolver.BaseNGUptimes[i]);
            }
        }

        private double[] GetInterpolatedCastTable(float actualHaste, bool use4T12Table, bool use4T13Table, bool useGoSFTable)
        {
            // Get index and remainder for interpolation
            double r = actualHaste / 0.05;
            int i = (int)r;
            r -= i;

            // If we're out of bounds, clip to the edge of the table
            if (i + 1 >= 21)
            {
                i = 19;
                r = 1;
            }

            double[] retval = new double[MoonkinSolver.CastDistributionSpells.Length];

            // Index the table and interpolate the remainder
            for (int index = 0; index < MoonkinSolver.CastDistributionSpells.Length; ++index)
            {
                if (use4T12Table)
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.T12CastDistributionGoSF[i, index] + r * (MoonkinSolver.T12CastDistributionGoSF[i + 1, index] - MoonkinSolver.T12CastDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.T12CastDistribution[i, index] + r * (MoonkinSolver.T12CastDistribution[i + 1, index] - MoonkinSolver.T12CastDistribution[i, index]);
                }
                else if (use4T13Table)
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.T13CastDistributionGoSF[i, index] + r * (MoonkinSolver.T13CastDistributionGoSF[i + 1, index] - MoonkinSolver.T13CastDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.T13CastDistribution[i, index] + r * (MoonkinSolver.T13CastDistribution[i + 1, index] - MoonkinSolver.T13CastDistribution[i, index]);
                }
                else
                {
                    if (useGoSFTable)
                        retval[index] = MoonkinSolver.CastDistributionGoSF[i, index] + r * (MoonkinSolver.CastDistributionGoSF[i + 1, index] - MoonkinSolver.CastDistributionGoSF[i, index]);
                    else
                        retval[index] = MoonkinSolver.CastDistribution[i, index] + r * (MoonkinSolver.CastDistribution[i + 1, index] - MoonkinSolver.CastDistribution[i, index]);
                }
            }

            return retval;
        }

        double GetPercentOfMoonfiresExtended(float actualHaste, bool use4T12Table, bool use4T13Table)
        {
            // Get index and remainder for interpolation
            double r = actualHaste / 0.05;
            int i = (int)r;
            r -= i;

            // If we're out of bounds, clip to the edge of the table
            if (i + 1 >= 21)
            {
                i = 19;
                r = 1;
            }

            // Index the table and interpolate the remainder
            if (use4T12Table)
            {
                return MoonkinSolver.T12PercentMoonfiresExtended[i] + r * (MoonkinSolver.T12PercentMoonfiresExtended[i + 1] - MoonkinSolver.T12PercentMoonfiresExtended[i]);
            }
            else if (use4T13Table)
            {
                return MoonkinSolver.T13PercentMoonfiresExtended[i] + r * (MoonkinSolver.T13PercentMoonfiresExtended[i + 1] - MoonkinSolver.T13PercentMoonfiresExtended[i]);
            }
            else
            {
                return MoonkinSolver.BasePercentMoonfiresExtended[i] + r * (MoonkinSolver.BasePercentMoonfiresExtended[i + 1] - MoonkinSolver.BasePercentMoonfiresExtended[i]);
            }
        }

        // Perform damage and mana calculations for all spells in the given rotation.  Returns damage done over the total duration.
        public float DamageDone(Character character, CharacterCalculationsMoonkin calcs, float treantLifespan, float spellPower, float spellHit, float spellCrit, float spellHaste, float masteryPoints, float latency)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            DruidTalents talents = character.DruidTalents;
            Spell sf = Solver.Starfire;
            Spell ss = Solver.Starsurge;
            Spell w = Solver.Wrath;
            Spell mf = Solver.Moonfire;
            Spell iSw = Solver.InsectSwarm;

            // 4.1: The bug causing the Eclipse buff to be rounded down to the nearest percent has been fixed
            float eclipseBonus = 1 + MoonkinSolver.ECLIPSE_BASE + masteryPoints * 0.02f;

            RotationData.NaturesGraceUptime = (float)GetInterpolatedNGUpime(spellHaste, calcs.BasicStats.BonusWrathEnergy > 0, calcs.BasicStats.T13FourPieceActive, talents.GlyphOfStarfire);

            RotationData.Duration = (float)GetInterpolatedCastTime(calcs.SpellHaste, calcs.BasicStats.BonusWrathEnergy > 0, calcs.BasicStats.T13FourPieceActive, talents.GlyphOfStarfire);

            double[] castDistribution = GetInterpolatedCastTable(calcs.SpellHaste, calcs.BasicStats.BonusWrathEnergy > 0, calcs.BasicStats.T13FourPieceActive, talents.GlyphOfStarfire);

            double percentOfMoonfiresExtended = talents.GlyphOfStarfire ? GetPercentOfMoonfiresExtended(calcs.SpellHaste, calcs.BasicStats.BonusWrathEnergy > 0, calcs.BasicStats.T13FourPieceActive) : 0;

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste, RotationData.NaturesGraceUptime, latency);
            DoMainNuke(calcs, ref ss, spellPower, spellHit, spellCrit, spellHaste, RotationData.NaturesGraceUptime, latency);
            DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste, RotationData.NaturesGraceUptime, latency);
            double gcd = Math.Max(1, 1.5 / (1 + spellHaste)) + latency;
            double ngGcd = Math.Max(1, 1.5 / (1 + spellHaste) / (1 + 0.05 * talents.NaturesGrace)) + latency;

            // Moonfire related local variables
            float mfBaseDur, mfMeanDur, mfMaxDur, mfMeanMaxDur, mfTicks, mfMaxTicks;
            mfBaseDur = mf.DotEffect.BaseDuration;
            mfMaxDur = mfBaseDur + (talents.GlyphOfStarfire ? 9f : 0f);

            // Determine Nature's Grace uptime against Moonfire
            float mfNGUptime = (float)Math.Min(2 * mfMaxDur / RotationData.Duration, 1);
            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste, mfNGUptime, latency);
            // Insect Swarm never benefits from Nature's Grace
            DoDotSpell(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste, 0, latency);

            mfTicks = mf.DotEffect.NumberOfTicks;
            mfMaxTicks = mfTicks + (talents.GlyphOfStarfire ? 6 : 0);
            mfMeanDur = mf.DotEffect.Duration;
            mfMeanMaxDur = mf.DotEffect.Duration + (talents.GlyphOfStarfire ? 6 * mf.DotEffect.TickLength : 0f);

            RotationData.MoonfireAvgCast = mf.CastTime;
            RotationData.InsectSwarmAvgCast = iSw.CastTime;

            // Break the cast distribution down into its component cast counts
            double wrathCasts = castDistribution[1] * RotationData.Duration / w.CastTime;
            double eclipseWrathCasts = castDistribution[5] * RotationData.Duration / w.CastTime;
            double nonEclipsedWrathPercentage = castDistribution[1] / (castDistribution[1] + castDistribution[5]);
            double eclipsedWrathPercentage = castDistribution[5] / (castDistribution[1] + castDistribution[5]);
            RotationData.WrathAvgHit = (float)(nonEclipsedWrathPercentage * w.DamagePerHit + eclipsedWrathPercentage * w.DamagePerHit * eclipseBonus);
            RotationData.WrathAvgEnergy = w.AverageEnergy;
            RotationData.WrathCount = (float)(wrathCasts + eclipseWrathCasts);
            double starfireCasts = castDistribution[0] * RotationData.Duration / sf.CastTime;
            double eclipseStarfireCasts = castDistribution[4] * RotationData.Duration / sf.CastTime;
            double nonEclipsedStarfirePercentage = castDistribution[0] / (castDistribution[0] + castDistribution[4]);
            double eclipsedStarfirePercentage = castDistribution[4] / (castDistribution[0] + castDistribution[4]);
            RotationData.StarfireAvgHit = (float)(nonEclipsedStarfirePercentage * sf.DamagePerHit + eclipsedStarfirePercentage * sf.DamagePerHit * eclipseBonus);
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;
            RotationData.StarfireCount = (float)(starfireCasts + eclipseStarfireCasts);
            double starsurgeCasts = castDistribution[2] * RotationData.Duration / ss.CastTime;
            double eclipseStarsurgeCasts = castDistribution[6] * RotationData.Duration / ss.CastTime;
            double shootingStarsProcs = castDistribution[3] * RotationData.Duration / gcd;
            double eclipseShootingStarsProcs = castDistribution[7] * RotationData.Duration / gcd;
            double allStarsurgePercentage = castDistribution[2] + castDistribution[6] + castDistribution[3] + castDistribution[7];
            double nonEclipsedStarsurgePercentage = (castDistribution[2] + castDistribution[3]) / allStarsurgePercentage;
            double eclipsedStarsurgePercentage = (castDistribution[6] + castDistribution[7]) / allStarsurgePercentage;
            double starsurgePercentage = (castDistribution[2] + castDistribution[6]) / allStarsurgePercentage;
            double shootingStarsPercentage = (castDistribution[3] + castDistribution[7]) / allStarsurgePercentage;
            RotationData.StarSurgeAvgHit = (float)(nonEclipsedStarsurgePercentage * ss.DamagePerHit + eclipsedStarsurgePercentage * ss.DamagePerHit * eclipseBonus);
            RotationData.StarSurgeAvgEnergy = ss.AverageEnergy;
            RotationData.StarSurgeCount = (float)(starsurgeCasts + eclipseStarsurgeCasts + shootingStarsProcs + eclipseShootingStarsProcs);
            double moonfireCasts = castDistribution[8] * RotationData.Duration / mf.CastTime;
            double eclipsedMoonfireCasts = castDistribution[10] * RotationData.Duration / mf.CastTime;
            double nonEclipsedMoonfirePercentage = castDistribution[8] / (castDistribution[8] + castDistribution[10]);
            double eclipsedMoonfirePercentage = castDistribution[10] / (castDistribution[8] + castDistribution[10]);
            RotationData.MoonfireCasts = (float)(moonfireCasts + eclipsedMoonfireCasts);
            double insectSwarmCasts = castDistribution[9] * RotationData.Duration / iSw.CastTime;
            double eclipsedInsectSwarmCasts = castDistribution[11] * RotationData.Duration / iSw.CastTime;
            double nonEclipsedInsectSwarmPercentage = castDistribution[9] / (castDistribution[9] + castDistribution[11]);
            double eclipsedInsectSwarmPercentage = castDistribution[11] / (castDistribution[9] + castDistribution[11]);
            RotationData.InsectSwarmCasts = (float)(insectSwarmCasts + eclipsedInsectSwarmCasts);

            double unextendedMoonfireAverage = nonEclipsedMoonfirePercentage * (mf.DamagePerHit + mf.DotEffect.DamagePerHit) +
                eclipsedMoonfirePercentage * (mf.DamagePerHit + mf.DotEffect.DamagePerHit) * eclipseBonus;
            double mfExtendedDotDamage = mfMaxTicks * (mf.DotEffect.DamagePerHit / mf.DotEffect.NumberOfTicks);
            double extendedMoonfireAverage = nonEclipsedMoonfirePercentage * (mf.DamagePerHit + mfExtendedDotDamage) +
                eclipsedMoonfirePercentage * (mf.DamagePerHit + mfExtendedDotDamage) * eclipseBonus;

            RotationData.MoonfireTicks = (float)(percentOfMoonfiresExtended * mfMaxTicks + (1 - percentOfMoonfiresExtended) * mfTicks);
            RotationData.MoonfireDuration = (float)(percentOfMoonfiresExtended * mfMeanDur + (1 - percentOfMoonfiresExtended) * mfMeanMaxDur);
            RotationData.MoonfireAvgHit = (float)(percentOfMoonfiresExtended * extendedMoonfireAverage + (1 - percentOfMoonfiresExtended) * unextendedMoonfireAverage);

            RotationData.InsectSwarmTicks = RotationData.InsectSwarmCasts * iSw.DotEffect.NumberOfTicks;
            RotationData.InsectSwarmDuration = iSw.DotEffect.Duration;
            RotationData.InsectSwarmAvgHit = (float)(nonEclipsedInsectSwarmPercentage * iSw.DotEffect.DamagePerHit +
                eclipsedInsectSwarmPercentage * iSw.DotEffect.DamagePerHit * eclipseBonus);

            RotationData.StarfireAvgCast = sf.CastTime;
            RotationData.WrathAvgCast = w.CastTime;

            RotationData.AverageInstantCast = (float)(gcd * (1 - RotationData.NaturesGraceUptime) + ngGcd * RotationData.NaturesGraceUptime);

            RotationData.StarSurgeAvgCast = (float)(starsurgePercentage * ss.CastTime + shootingStarsPercentage * RotationData.AverageInstantCast);

            // Modify the rotation duration to simulate the energy bonus from Dragonwrath procs
            if (calcs.BasicStats.DragonwrathProc > 0)
            {
                float baselineNukeDuration = RotationData.StarfireCount * RotationData.StarfireAvgCast +
                    RotationData.WrathCount * RotationData.WrathAvgCast +
                    RotationData.StarSurgeCount * RotationData.StarSurgeAvgCast;
                float dragonwrathNukeDuration = baselineNukeDuration / (1 + MoonkinSolver.DRAGONWRATH_PROC_RATE);
                RotationData.Duration -= (baselineNukeDuration - dragonwrathNukeDuration);
            }

            RotationData.LunarUptime = (float)(castDistribution[4] + 0.5 * castDistribution[6] + 0.5 * castDistribution[7] + 0.5 * castDistribution[10]);
            RotationData.SolarUptime = (float)(castDistribution[5] + 0.5 * castDistribution[6] + 0.5 * castDistribution[7] + 0.5 * castDistribution[10] + castDistribution[11]);

            float starfallReduction = (float)(starsurgeCasts + shootingStarsProcs + eclipseStarsurgeCasts + eclipseShootingStarsProcs) * 5f;
            float starfallCooldown = (90f - (talents.GlyphOfStarfall ? 30f : 0f)) - (talents.GlyphOfStarsurge ? starfallReduction : 0);
            float starfallRatio = talents.Starfall == 1 ?
                (RotationData.StarfallCastMode == StarfallMode.OnCooldown ? RotationData.AverageInstantCast / (starfallCooldown + RotationData.AverageInstantCast) : 0f)
                : 0f;
            float starfallTime = RotationData.StarfallCastMode == StarfallMode.LunarOnly ? RotationData.AverageInstantCast : 0f;
            float treantRatio = talents.ForceOfNature == 1 ? RotationData.AverageInstantCast / (180f + RotationData.AverageInstantCast) : 0;

            float starfallBaseDamage = (talents.Starfall > 0 && RotationData.StarfallCastMode == StarfallMode.Unused) ? 0 : DoStarfallCalcs(calcs, spellPower, spellHit, spellCrit);
            starfallBaseDamage *= 1 + (talents.GlyphOfFocus ? 0.1f : 0f);
            // Dragonwrath
            starfallBaseDamage *= 1 + (calcs.BasicStats.DragonwrathProc > 0 ? MoonkinSolver.DRAGONWRATH_PROC_RATE : 0f);
            float starfallEclipseDamage = starfallBaseDamage * eclipseBonus;
            RotationData.TreantDamage = talents.ForceOfNature == 0 ? 0 : DoTreeCalcs(calcs, character.Level, character.BossOptions.Level, spellPower, treantLifespan);
            // T12 2-piece: 2-sec cast, 5192-6035 damage, affected by hit, 15-sec duration
            float T122PieceHitDamage = (5192 + 6035) / 2f * spellHit * (1 + calcs.BasicStats.BonusFireDamageMultiplier);
            // I'm going to assume a 150% crit modifier on the 2T12 proc until I'm told otherwise
            float T122PieceCritDamage = T122PieceHitDamage * 1.5f;
            // Use 2.5% crit rate based on EJ testing
            // Hard-code 4.5 casts/proc based on EJ testing
            float T122PieceBaseDamage = (0.975f * T122PieceHitDamage + 0.025f * T122PieceCritDamage) * 4.5f;

            // Without glyph of Starsurge, you cannot fit a Starfall in every Lunar eclipse.
            // The actual result will be better than 1/2, because you will be able to cast SFall later in each Eclipse as the fight goes on,
            // but you will miss a Lunar proc entirely eventually.
            float starfallCooldownOverlap = starfallCooldown - RotationData.Duration;
            float rotationsToMiss = starfallCooldownOverlap > 0 ? RotationData.Duration * RotationData.LunarUptime / starfallCooldownOverlap : 0f;
            float starfallFraction = rotationsToMiss > 0 ? (float)(Math.Ceiling(rotationsToMiss) / (Math.Ceiling(rotationsToMiss) + 1)) : 1f;
            RotationData.StarfallCasts = RotationData.StarfallCastMode == StarfallMode.OnCooldown ? starfallRatio * RotationData.Duration / RotationData.AverageInstantCast
                : (RotationData.StarfallCastMode == StarfallMode.LunarOnly ? starfallFraction : 0f);
            RotationData.TreantCasts = treantRatio * RotationData.Duration / RotationData.AverageInstantCast;
            RotationData.StarfallStars = 10f;

            RotationData.StarfallDamage = RotationData.StarfallCastMode == StarfallMode.OnCooldown ?
                RotationData.LunarUptime * starfallEclipseDamage + (1 - RotationData.LunarUptime) * starfallBaseDamage :
                starfallEclipseDamage;

            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;
            float insectSwarmDamage = RotationData.InsectSwarmAvgHit * RotationData.InsectSwarmCasts;

            // Calculate total damage done for external cooldowns per rotation
            float starfallDamage = RotationData.StarfallDamage * RotationData.StarfallCasts;
            float treantDamage = RotationData.TreantDamage * RotationData.TreantCasts;
            float T122PieceDamage = 0f;
            if (calcs.BasicStats.ContainsSpecialEffect(se => se.Trigger == Trigger.MageNukeCast))
            {
                foreach (SpecialEffect effect in calcs.BasicStats.SpecialEffects(se => se.Trigger == Trigger.MageNukeCast))
                {
                    T122PieceDamage = T122PieceBaseDamage * effect.GetAverageUptime(RotationData.Duration / (RotationData.WrathCount + RotationData.StarfireCount), 1f);
                }
            }

            // Calculate mana cost per cast.
            // Starfall - 35% of base mana
            float starfallManaCost = (int)(0.35f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;
            // Force of Nature - 12% of base mana
            float treantManaCost = (int)(0.12f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.InsectSwarmCasts + RotationData.StarfallCasts + RotationData.TreantCasts;
            RotationData.DotTicks = RotationData.InsectSwarmTicks + RotationData.MoonfireTicks;
            RotationData.ManaUsed = RotationData.WrathCount * w.BaseManaCost +
                RotationData.StarfireCount * sf.BaseManaCost +
                RotationData.StarSurgeCount * ss.BaseManaCost +
                RotationData.MoonfireCasts * mf.BaseManaCost +
                RotationData.InsectSwarmCasts * iSw.BaseManaCost +
                RotationData.StarfallCasts * starfallManaCost +
                RotationData.TreantCasts * treantManaCost;

            float manaSavingsFromOOC = MoonkinSolver.OOC_PROC_CHANCE * (RotationData.MoonfireCasts / RotationData.CastCount * mf.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.InsectSwarmCasts / RotationData.CastCount * iSw.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarfireCount / RotationData.CastCount * sf.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.WrathCount / RotationData.CastCount * w.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarSurgeCount / RotationData.CastCount * ss.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarfallCasts / RotationData.CastCount * starfallManaCost);

            RotationData.ManaUsed -= manaSavingsFromOOC;

            RotationData.ManaGained = 2 * MoonkinSolver.EUPHORIA_PERCENT * talents.Euphoria * calcs.BasicStats.Mana;

            return RotationData.WrathAvgHit * RotationData.WrathCount +
                RotationData.StarfireAvgHit * RotationData.StarfireCount +
                RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount +
                moonfireDamage + insectSwarmDamage + treantDamage + starfallDamage + T122PieceDamage;
        }
    }
}
