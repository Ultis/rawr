using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    public enum DotMode { Twice, Always };
    public enum StarfallMode { Unused, LunarOnly, OnCooldown };
    public enum MushroomMode { Unused, SolarOnly, OnCooldown };
    // Rotation information for display to the user.
    public class RotationData
    {
        #region Inputs
        public string Name { get; set; }
        public DotMode MoonfireRefreshMode { get; set; }
        public DotMode InsectSwarmRefreshMode { get; set; }
        public StarfallMode StarfallCastMode { get; set; }
        public MushroomMode WildMushroomCastMode { get; set; }
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
        public void DoMainNuke(CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste, float latency)
        {
            float overallDamageModifier = mainNuke.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
            // Add a check for the higher of the two spell schools, as Starsurge always chooses the higher one
            overallDamageModifier *= mainNuke.School == SpellSchool.Arcane ? (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) :
                (mainNuke.School == SpellSchool.Nature ? (1 + calcs.BasicStats.BonusNatureDamageMultiplier) :
                (1 + (calcs.BasicStats.BonusArcaneDamageMultiplier > calcs.BasicStats.BonusNatureDamageMultiplier ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier)));

            float gcd = 1.5f / (1.0f + spellHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;

            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            float baseCastTime = (float)Math.Max(mainNuke.BaseCastTime / (1 + spellHaste), instantCast);
            mainNuke.CastTime = baseCastTime;
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * spellPower) * overallDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier;
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
            mainNuke.AverageEnergy = mainNuke.BaseEnergy;
        }

        // Calculate damage and casting time for a damage-over-time effect.
        public void DoDotSpell(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste, float naturesGraceBonusHaste, float naturesGraceUptime, float latency)
        {
            float schoolMultiplier = dotSpell.School == SpellSchool.Arcane ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier;

            float overallDamageModifier = dotSpell.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier);

            float dotEffectDamageModifier = dotSpell.DotEffect.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier);

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
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
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
            float hitDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier);
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

        private double GetInterpolatedCastTime(float actualHaste, bool use4T12Table)
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
                return MoonkinSolver.T12RotationDurations[i] + r * (MoonkinSolver.T12RotationDurations[i + 1] - MoonkinSolver.T12RotationDurations[i]);
            }
            else
            {
                return MoonkinSolver.BaseRotationDurations[i] + r * (MoonkinSolver.BaseRotationDurations[i + 1] - MoonkinSolver.BaseRotationDurations[i]);
            }
        }

        private double[] GetInterpolatedCastTable(float actualHaste, bool use4T12Table)
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
            if (use4T12Table)
            {
                for (int index = 0; index < MoonkinSolver.CastDistributionSpells.Length; ++index)
                {
                    retval[index] = MoonkinSolver.T12CastDistribution[i, index] + r * (MoonkinSolver.T12CastDistribution[i + 1, index] - MoonkinSolver.T12CastDistribution[i, index]);
                }
            }
            else
            {
                for (int index = 0; index < MoonkinSolver.CastDistributionSpells.Length; ++index)
                {
                    retval[index] = MoonkinSolver.CastDistribution[i, index] + r * (MoonkinSolver.CastDistribution[i + 1, index] - MoonkinSolver.CastDistribution[i, index]);
                }
            }

            return retval;
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

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste, latency);
            DoMainNuke(calcs, ref ss, spellPower, spellHit, spellCrit, spellHaste, latency);
            DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste, latency);
            double gcd = Math.Max(1, 1.5 / (1 + spellHaste)) + latency;

            double baselineNukesDuration = GetInterpolatedCastTime(calcs.SpellHaste, calcs.BasicStats.BonusWrathEnergy > 0);

            double[] castDistribution = GetInterpolatedCastTable(calcs.SpellHaste, calcs.BasicStats.BonusWrathEnergy > 0);

            // Break the cast distribution down into its component cast counts
            double wrathCastsBase = castDistribution[1] * baselineNukesDuration / w.CastTime;
            double eclipseWrathCastsBase = castDistribution[5] * baselineNukesDuration / w.CastTime;
            double nonEclipsedWrathPercentage = castDistribution[1] / (castDistribution[1] + castDistribution[5]);
            double eclipsedWrathPercentage = castDistribution[5] / (castDistribution[1] + castDistribution[5]);
            RotationData.WrathAvgHit = (float)(nonEclipsedWrathPercentage * w.DamagePerHit + eclipsedWrathPercentage * w.DamagePerHit * eclipseBonus);
            RotationData.WrathAvgEnergy = w.AverageEnergy;
            RotationData.WrathCount = (float)(wrathCastsBase + eclipseWrathCastsBase);
            double starfireCastsBase = castDistribution[0] * baselineNukesDuration / sf.CastTime;
            double eclipseStarfireCastsBase = castDistribution[4] * baselineNukesDuration / sf.CastTime;
            double nonEclipsedStarfirePercentage = castDistribution[0] / (castDistribution[0] + castDistribution[4]);
            double eclipsedStarfirePercentage = castDistribution[4] / (castDistribution[0] + castDistribution[4]);
            RotationData.StarfireAvgHit = (float)(nonEclipsedStarfirePercentage * sf.DamagePerHit + eclipsedStarfirePercentage * sf.DamagePerHit * eclipseBonus);
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;
            RotationData.StarfireCount = (float)(starfireCastsBase + eclipseStarfireCastsBase);
            double starsurgeCastsBase = castDistribution[2] * baselineNukesDuration / ss.CastTime;
            double eclipseStarsurgeCastsBase = castDistribution[6] * baselineNukesDuration / ss.CastTime;
            double shootingStarsProcsBase = castDistribution[3] * baselineNukesDuration / gcd;
            double eclipseShootingStarsProcsBase = castDistribution[7] * baselineNukesDuration / gcd;
            double allStarsurgePercentage = castDistribution[2] + castDistribution[6] + castDistribution[3] + castDistribution[7];
            double nonEclipsedStarsurgePercentage = (castDistribution[2] + castDistribution[3]) / allStarsurgePercentage;
            double eclipsedStarsurgePercentage = (castDistribution[6] + castDistribution[7]) / allStarsurgePercentage;
            RotationData.StarSurgeAvgHit = (float)(nonEclipsedStarsurgePercentage * ss.DamagePerHit + eclipsedStarsurgePercentage * ss.DamagePerHit * eclipseBonus);
            RotationData.StarSurgeAvgEnergy = ss.AverageEnergy;
            RotationData.StarSurgeCount = (float)(starsurgeCastsBase + eclipseStarsurgeCastsBase + shootingStarsProcsBase + eclipseShootingStarsProcsBase);

            double wrathNGCast = Math.Max(1, (w.CastTime - latency) / (1 + 0.05 * talents.NaturesGrace)) + latency;
            double sfNGCast = Math.Max(1, (sf.CastTime - latency) / (1 + 0.05 * talents.NaturesGrace)) + latency;
            double ssNGCast = Math.Max(1, (ss.CastTime - latency) / (1 + 0.05 * talents.NaturesGrace)) + latency;
            double ngGcd = Math.Max(1, 1.5 / (1 + spellHaste) / (1 + 0.05 * talents.NaturesGrace)) + latency;

            double wrathCastsUnderNG = Math.Ceiling(15 - gcd - ngGcd / wrathNGCast);
            double starfireCastsUnderNG = Math.Ceiling(15 - gcd - ngGcd / sfNGCast);
            double wrathNGReduction = wrathCastsUnderNG * (w.CastTime - wrathNGCast);
            double starfireNGReduction = starfireCastsUnderNG * (sf.CastTime - sfNGCast);

            w.CastTime = (float)((wrathCastsUnderNG / (wrathCastsBase + eclipseWrathCastsBase)) * wrathNGCast +
                         (1 - wrathCastsUnderNG / (wrathCastsBase + eclipseWrathCastsBase)) * w.CastTime);
            sf.CastTime = (float)((starfireCastsUnderNG / (starfireCastsBase + eclipseStarfireCastsBase)) * sfNGCast +
                         (1 - starfireCastsUnderNG / (starfireCastsBase + eclipseStarfireCastsBase)) * sf.CastTime);

            // First-order estimate of NG uptime for Starsurge
            double starsurgeCastsUnderNG = (30 / (baselineNukesDuration - wrathNGReduction - starfireNGReduction)) * (eclipseStarsurgeCastsBase + starsurgeCastsBase);
            double shootingStarsProcsUnderNG = (30 / (baselineNukesDuration - wrathNGReduction - starfireNGReduction)) * (shootingStarsProcsBase + eclipseShootingStarsProcsBase);
            double starsurgeNGReduction = starsurgeCastsUnderNG * (ss.CastTime - ssNGCast);
            double shootingStarsNGReduction = shootingStarsProcsUnderNG * (gcd - ngGcd);
            float ssCastTime = (float)((starsurgeCastsUnderNG / (starsurgeCastsBase + shootingStarsProcsBase + eclipseStarsurgeCastsBase + eclipseShootingStarsProcsBase)) * ssNGCast +
                (shootingStarsProcsUnderNG / (starsurgeCastsBase + shootingStarsProcsBase + eclipseStarsurgeCastsBase + eclipseShootingStarsProcsBase)) * ngGcd +
                ((starsurgeCastsBase + eclipseStarsurgeCastsBase - starsurgeCastsUnderNG) / (starsurgeCastsBase + shootingStarsProcsBase + eclipseStarsurgeCastsBase + eclipseShootingStarsProcsBase)) * ss.CastTime +
                ((shootingStarsProcsBase + eclipseShootingStarsProcsBase - shootingStarsProcsUnderNG) / (starsurgeCastsBase + shootingStarsProcsBase + eclipseStarsurgeCastsBase + eclipseShootingStarsProcsBase)) * gcd);

            ss.CastTime = ssCastTime;

            RotationData.StarfireAvgCast = sf.CastTime;
            RotationData.WrathAvgCast = w.CastTime;
            RotationData.StarSurgeAvgCast = ss.CastTime;

            double NaturesGraceShortening = wrathNGReduction + starfireNGReduction + starsurgeNGReduction + shootingStarsNGReduction;

            double mainNukeDuration = baselineNukesDuration - NaturesGraceShortening;

            if (calcs.BasicStats.DragonwrathProc > 0) mainNukeDuration /= (1 + MoonkinSolver.DRAGONWRATH_PROC_RATE);

            RotationData.NaturesGraceUptime = (float)((30 - 2 * gcd) / (mainNukeDuration + 2 * ngGcd));

            RotationData.AverageInstantCast = (float)(gcd * (1 - RotationData.NaturesGraceUptime) + ngGcd * RotationData.NaturesGraceUptime);

            RotationData.LunarUptime = (float)(castDistribution[4] + 0.5 * castDistribution[6] + 0.5 * castDistribution[7]);
            RotationData.SolarUptime = (float)(castDistribution[5] + 0.5 * castDistribution[6] + 0.5 * castDistribution[7]);

            // Moonfire related local variables
            float mfBaseDur, mfMeanDur, mfMaxDur, mfMeanMaxDur, mfTicks, mfMaxTicks;
            mfBaseDur = mf.DotEffect.BaseDuration;
            mfMaxDur = mfBaseDur + (talents.GlyphOfStarfire ? 9f : 0f);

            // Determine Nature's Grace uptime against Moonfire
            // The expression for DotMode.Always is approximate
            float mfNGUptime = RotationData.MoonfireRefreshMode == DotMode.Always ? (float)Math.Min(2 * mfMaxDur / (mainNukeDuration + 2 * gcd), 1) : (RotationData.MoonfireRefreshMode == DotMode.Twice ? (talents.GlyphOfStarfire ? RotationData.NaturesGraceUptime : 1f) : 0f);
            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesGrace, mfNGUptime, latency);
            // Insect Swarm never benefits from Nature's Grace
            DoDotSpell(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesGrace, 0, latency);

            mfTicks = mf.DotEffect.NumberOfTicks;
            mfMaxTicks = mfTicks + (talents.GlyphOfStarfire ? 6 : 0);
            mfMeanDur = mf.DotEffect.Duration;
            mfMeanMaxDur = mf.DotEffect.Duration + (talents.GlyphOfStarfire ? 6 * mf.DotEffect.TickLength : 0f);

            RotationData.MoonfireAvgCast = mf.CastTime;
            RotationData.InsectSwarmAvgCast = iSw.CastTime;

            float insectSwarmRatio = RotationData.InsectSwarmRefreshMode == DotMode.Always ? iSw.CastTime / iSw.DotEffect.Duration : 0f;
            float insectSwarmTime = RotationData.InsectSwarmRefreshMode == DotMode.Twice ? 2 * iSw.CastTime : 0;

            float moonfireTime = RotationData.MoonfireRefreshMode == DotMode.Twice ? 2 * mf.CastTime : 0f;
            float moonfireRatio = RotationData.MoonfireRefreshMode == DotMode.Always ? mf.CastTime / mfMeanMaxDur : 0f;

            float mushroomPlantTime = 3f;
            float detonateCooldown = 10f;
            float starfallReduction = (float)(starsurgeCastsBase + shootingStarsProcsBase + eclipseStarsurgeCastsBase + eclipseShootingStarsProcsBase) * 5f;
            float starfallCooldown = (90f - (talents.GlyphOfStarfall ? 30f : 0f)) - (talents.GlyphOfStarsurge ? starfallReduction : 0);
            float starfallRatio = talents.Starfall == 1 ?
                (RotationData.StarfallCastMode == StarfallMode.OnCooldown ? RotationData.AverageInstantCast / (starfallCooldown + RotationData.AverageInstantCast) : 0f)
                : 0f;
            float starfallTime = RotationData.StarfallCastMode == StarfallMode.LunarOnly ? RotationData.AverageInstantCast : 0f;
            float treantRatio = talents.ForceOfNature == 1 ? RotationData.AverageInstantCast / (180f + RotationData.AverageInstantCast) : 0;
            float mushroomRatio = RotationData.WildMushroomCastMode == MushroomMode.OnCooldown ? mushroomPlantTime / detonateCooldown : 0f;
            float mushroomTime = RotationData.WildMushroomCastMode == MushroomMode.SolarOnly ? (float)mainNukeDuration * RotationData.SolarUptime / detonateCooldown * mushroomPlantTime : 0f;
            float totalOnCooldownRatio = moonfireRatio + insectSwarmRatio + starfallRatio + treantRatio + mushroomRatio;

            RotationData.Duration = (float)(mainNukeDuration + (RotationData.MoonfireRefreshMode == DotMode.Twice ? moonfireTime : 0f) +
                (RotationData.InsectSwarmRefreshMode == DotMode.Twice ? insectSwarmTime : 0f) +
                (RotationData.StarfallCastMode == StarfallMode.LunarOnly ? starfallTime : 0f) +
                (RotationData.WildMushroomCastMode == MushroomMode.SolarOnly ? mushroomTime : 0f)) / (1 - totalOnCooldownRatio);

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
            float mushroomBaseDamage = RotationData.WildMushroomCastMode == MushroomMode.Unused ? 0 : DoMushroomCalcs(calcs, spellPower, spellHit, spellCrit);
            // Dragonwrath
            mushroomBaseDamage *= 1 + (calcs.BasicStats.DragonwrathProc > 0 ? MoonkinSolver.DRAGONWRATH_PROC_RATE : 0f);
            float mushroomEclipseDamage = mushroomBaseDamage * eclipseBonus;

            // Without glyph of Starsurge, you cannot fit a Starfall in every Lunar eclipse.
            // The actual result will be better than 1/2, because you will be able to cast SFall later in each Eclipse as the fight goes on,
            // but you will miss a Lunar proc entirely eventually.
            float starfallCooldownOverlap = starfallCooldown - RotationData.Duration;
            float rotationsToMiss = starfallCooldownOverlap > 0 ? RotationData.Duration * RotationData.LunarUptime / starfallCooldownOverlap : 0f;
            float starfallFraction = rotationsToMiss > 0 ? (float)(Math.Ceiling(rotationsToMiss) / (Math.Ceiling(rotationsToMiss) + 1)) : 1f;
            RotationData.StarfallCasts = RotationData.StarfallCastMode == StarfallMode.OnCooldown ? starfallRatio * RotationData.Duration / RotationData.AverageInstantCast
                : (RotationData.StarfallCastMode == StarfallMode.LunarOnly ? starfallFraction : 0f);
            RotationData.TreantCasts = treantRatio * RotationData.Duration / RotationData.AverageInstantCast;
            RotationData.MushroomCasts = RotationData.WildMushroomCastMode == MushroomMode.OnCooldown ? mushroomRatio * RotationData.Duration / mushroomPlantTime
                : (RotationData.WildMushroomCastMode == MushroomMode.SolarOnly ? RotationData.SolarUptime * RotationData.Duration / detonateCooldown : 0f);
            RotationData.StarfallStars = 10f;

            float percentOfMoonfiresExtended = 0f;
            if (talents.GlyphOfStarfire)
            {
                percentOfMoonfiresExtended = RotationData.MoonfireRefreshMode == DotMode.Twice ?
                    (RotationData.Duration / 2 - mfMeanDur) / (mfMeanMaxDur - mfMeanDur) :
                (float)(castDistribution[4] + castDistribution[0] +
                0.5 * castDistribution[2] + 0.5 * castDistribution[3] +
                0.5 * castDistribution[6] + 0.5 * castDistribution[7] +
                mfMeanDur / RotationData.Duration);
            }
            float unextendedMoonfireAverage = mf.DamagePerHit + mf.DotEffect.DamagePerHit;
            float extendedMoonfireAverage = mf.DamagePerHit + (mfMaxTicks * (mf.DotEffect.DamagePerHit / mf.DotEffect.NumberOfTicks));
            float moonfireEclipseMultiplier = 1 + (eclipseBonus - 1) * (RotationData.MoonfireRefreshMode == DotMode.Always ?
                RotationData.LunarUptime + (talents.Sunfire > 0 ? RotationData.SolarUptime : 0f) :
                (RotationData.MoonfireRefreshMode == DotMode.Twice ? (talents.Sunfire > 0 ? 1 : 0.5f) : 0f));

            RotationData.MoonfireCasts = RotationData.MoonfireRefreshMode == DotMode.Twice ? moonfireTime / mf.CastTime : RotationData.Duration * moonfireRatio;
            RotationData.MoonfireTicks = percentOfMoonfiresExtended * mfMaxTicks + (1 - percentOfMoonfiresExtended) * mfTicks;
            RotationData.MoonfireDuration = percentOfMoonfiresExtended * mfMeanDur + (1 - percentOfMoonfiresExtended) * mfMeanMaxDur;
            RotationData.MoonfireAvgHit = (percentOfMoonfiresExtended * extendedMoonfireAverage + (1 - percentOfMoonfiresExtended) * unextendedMoonfireAverage) * moonfireEclipseMultiplier;

            RotationData.InsectSwarmCasts = RotationData.InsectSwarmRefreshMode == DotMode.Twice ? insectSwarmTime / iSw.CastTime : RotationData.Duration * insectSwarmRatio;
            RotationData.InsectSwarmTicks = RotationData.InsectSwarmCasts * iSw.DotEffect.NumberOfTicks;
            RotationData.InsectSwarmDuration = iSw.DotEffect.Duration;
            RotationData.InsectSwarmAvgHit = iSw.DotEffect.DamagePerHit * (1 + (eclipseBonus - 1) * (RotationData.InsectSwarmRefreshMode == DotMode.Twice ? 0.5f : RotationData.SolarUptime));

            RotationData.StarfallDamage = RotationData.StarfallCastMode == StarfallMode.OnCooldown ?
                RotationData.LunarUptime * starfallEclipseDamage + (1 - RotationData.LunarUptime) * starfallBaseDamage :
                starfallEclipseDamage;

            RotationData.MushroomDamage = RotationData.WildMushroomCastMode == MushroomMode.OnCooldown ?
                RotationData.SolarUptime * mushroomEclipseDamage + (1 - RotationData.SolarUptime) * mushroomBaseDamage :
                mushroomEclipseDamage;

            float starSurgeDamage = RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount;

            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;
            float insectSwarmDamage = RotationData.InsectSwarmAvgHit * RotationData.InsectSwarmCasts;

            // Calculate total damage done for external cooldowns per rotation
            float starfallDamage = RotationData.StarfallDamage * RotationData.StarfallCasts;
            float treantDamage = RotationData.TreantDamage * RotationData.TreantCasts;
            float mushroomDamage = RotationData.MushroomDamage * RotationData.MushroomCasts;
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
            // Wild Mushroom - 3x 11% of base mana
            float mushroomManaCost = (int)(0.33f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;
            // Force of Nature - 12% of base mana
            float treantManaCost = (int)(0.12f * MoonkinSolver.BaseMana) - calcs.BasicStats.SpellsManaCostReduction - calcs.BasicStats.NatureSpellsManaCostReduction;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.InsectSwarmCasts + RotationData.StarfallCasts + RotationData.MushroomCasts + RotationData.TreantCasts;
            RotationData.DotTicks = RotationData.InsectSwarmTicks + RotationData.MoonfireTicks;
            RotationData.ManaUsed = RotationData.WrathCount * w.BaseManaCost +
                RotationData.StarfireCount * sf.BaseManaCost +
                RotationData.StarSurgeCount * ss.BaseManaCost +
                RotationData.MoonfireCasts * mf.BaseManaCost +
                RotationData.InsectSwarmCasts * iSw.BaseManaCost +
                RotationData.StarfallCasts * starfallManaCost +
                RotationData.MushroomCasts * mushroomManaCost +
                RotationData.TreantCasts * treantManaCost;

            float manaSavingsFromOOC = MoonkinSolver.OOC_PROC_CHANCE * (RotationData.MoonfireCasts / RotationData.CastCount * mf.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.InsectSwarmCasts / RotationData.CastCount * iSw.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarfireCount / RotationData.CastCount * sf.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.WrathCount / RotationData.CastCount * w.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarSurgeCount / RotationData.CastCount * ss.BaseManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.StarfallCasts / RotationData.CastCount * starfallManaCost) +
                MoonkinSolver.OOC_PROC_CHANCE * (RotationData.MushroomCasts / RotationData.CastCount * mushroomManaCost);

            RotationData.ManaUsed -= manaSavingsFromOOC;

            RotationData.ManaGained = 2 * MoonkinSolver.EUPHORIA_PERCENT * talents.Euphoria * calcs.BasicStats.Mana;

            return RotationData.WrathAvgHit * RotationData.WrathCount +
                RotationData.StarfireAvgHit * RotationData.StarfireCount +
                RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount +
                moonfireDamage + insectSwarmDamage + starSurgeDamage + treantDamage + starfallDamage + mushroomDamage + T122PieceDamage;
        }
    }
}
