using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    public enum DotMode { Twice, Always };
    public enum StarsurgeMode { OnCooldown };
    public enum StarfallMode { Unused, LunarOnly, OnCooldown };
    public enum MushroomMode { Unused, SolarOnly, OnCooldown };
    // Rotation information for display to the user.
    public class RotationData
    {
        public float SustainedDPS = 0.0f;
        public float BurstDPS = 0.0f;
        public float DPM = 0.0f;
        public TimeSpan TimeToOOM = new TimeSpan(0, 0, 0);
        public string Name { get; set; }
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
        public DotMode MoonfireRefreshMode { get; set; }
        public DotMode InsectSwarmRefreshMode { get; set; }
        public StarsurgeMode StarsurgeCastMode { get; set; }
        public StarfallMode StarfallCastMode { get; set; }
        public MushroomMode WildMushroomCastMode { get; set; }
    }

    // Our old friend the spell rotation.
    public class SpellRotation
    {
        public MoonkinSolver Solver { get; set; }
        public RotationData RotationData = new RotationData();

        // Variables used for special calculations regarding Nature's Grace and Moonfire
        public float BaselineDuration { get; set; }
        public float NaturesGraceShortening { get; set; }

        // Multiple targets present for X % of single-target DPS
        public float MultipleTargetsPercentage { get; set; }

        public override string ToString()
        {
            return RotationData.Name;
        }

        // Calculate damage and casting time for a single, direct-damage spell.
        public void DoMainNuke(CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste, float naturesGraceBonusHaste, float naturesGraceUptime)
        {
            float latency = calcs.Latency;

            float overallDamageModifier = mainNuke.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
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
            mainNuke.CastTime = naturesGraceUptime * ngCastTime + (1 - naturesGraceUptime) * baseCastTime;
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * spellPower) * overallDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier;
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
            mainNuke.AverageEnergy = mainNuke.BaseEnergy;
        }

        // Calculate damage and casting time for a damage-over-time effect.
        public void DoDotSpell(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste, float naturesGraceBonusHaste, float naturesGraceUptime)
        {
            float latency = calcs.Latency;
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
        private float DoTreeCalcs(CharacterCalculationsMoonkin calcs, float effectiveNatureDamage, float treantLifespan)
        {
            int bossLevel = calcs.TargetLevel;
            int playerLevel = calcs.PlayerLevel;
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

            float numberOfStarHits = MultipleTargetsPercentage * 20f + (1 - MultipleTargetsPercentage) * 10f;

            float avgNumBigStarsHit = spellHit * numberOfStarHits;

            return avgNumBigStarsHit * averageDamagePerBigStar;
        }

        // Perform damage and mana calculations for all spells in the given rotation.  Returns damage done over the total duration.
        public float DamageDone(DruidTalents talents, CharacterCalculationsMoonkin calcs, float treantLifespan, float spellPower, float spellHit, float spellCrit, float spellHaste, float masteryPoints)
        {
            Spell sf = Solver.Starfire;
            Spell ss = Solver.Starsurge;
            Spell w = Solver.Wrath;
            Spell mf = Solver.Moonfire;
            Spell iSw = Solver.InsectSwarm;

            Spell mfExtended = new Spell(mf);
            mfExtended.DotEffect.BaseDuration += 9.0f;

            // 4.1: The bug causing the Eclipse buff to be rounded down to the nearest percent has been fixed
            float eclipseBonus = 1 + calcs.EclipseBase + masteryPoints * 0.02f;

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesGrace, RotationData.NaturesGraceUptime);
            DoMainNuke(calcs, ref ss, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesGrace, RotationData.NaturesGraceUptime);
			DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesGrace, RotationData.NaturesGraceUptime);

            RotationData.AverageInstantCast = 1.5f / (1 + spellHaste) * (1 - RotationData.NaturesGraceUptime) + (RotationData.NaturesGraceUptime * 1.5f / (1 + spellHaste) / (1 + 0.05f * talents.NaturesGrace));

            // Moonfire and Nature's Grace:
            // * 100% uptime if no glyph of SF and cast twice.
            // * 100% uptime on Sunfire and CEILING(15 / sfNGCast) * sfNGCast / mfExtendedDuration % on GoSF Moonfire if cast twice
            // * MIN(2 * IF(GoSF, mfExtendedDuration, mfDuration) / rotationLengthDifference, 100) % if cast on CD
            float mfOnCDNGUptime = (float)Math.Min(2 * (talents.GlyphOfStarfire ? mfExtended.DotEffect.BaseDuration : mf.DotEffect.BaseDuration) / (BaselineDuration - NaturesGraceShortening), 1f);

            float normalMFNGUptime = RotationData.MoonfireRefreshMode == DotMode.Twice ? 1f
                : (BaselineDuration > 0 && NaturesGraceShortening > 0 ? mfOnCDNGUptime : 1f);
            float extendedMFNGUptime = RotationData.MoonfireRefreshMode == DotMode.Twice ? (float)Math.Ceiling((15 - RotationData.AverageInstantCast) / sf.CastTime) * sf.CastTime / mfExtended.DotEffect.BaseDuration
                : (BaselineDuration > 0 && NaturesGraceShortening > 0 ? mfOnCDNGUptime : 1f);

            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesGrace, normalMFNGUptime);
            // Insect swarm never benefits from Nature's Grace
            DoDotSpell(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesGrace, 0);
            if (talents.GlyphOfStarfire)
				DoDotSpell(calcs, ref mfExtended, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesGrace, extendedMFNGUptime);

            RotationData.MoonfireAvgCast = mf.CastTime;
            RotationData.InsectSwarmAvgCast = iSw.CastTime;

            float starfallBaseDamage = (talents.Starfall > 0 && RotationData.StarfallCastMode == StarfallMode.Unused) ? 0 : DoStarfallCalcs(calcs, spellPower, spellHit, spellCrit);
            starfallBaseDamage *= 1 + (talents.GlyphOfFocus ? 0.1f : 0f);
            float starfallEclipseDamage = starfallBaseDamage * eclipseBonus;
            RotationData.TreantDamage = talents.ForceOfNature == 0 ? 0 : DoTreeCalcs(calcs, spellPower, treantLifespan);
            // T12 2-piece: 2-sec cast, 5192-6035 damage, affected by hit, 15-sec duration
            // For now, hard-code the 8% multiplier from Earth and Moon
            // Hard-code 3.5 casts/proc based on EJ testing
            float T122PieceHitDamage = (5192 + 6035) / 2f * spellHit * 1.08f;
            // I'm going to assume a 150% crit modifier on the 2T12 proc until I'm told otherwise
            float T122PieceCritDamage = T122PieceHitDamage * 1.5f;
            // Use 2.5% crit rate based on EJ testing
            float T122PieceBaseDamage = (0.975f * T122PieceHitDamage + 0.025f * T122PieceCritDamage) * 3.5f;
            float mushroomBaseDamage = RotationData.WildMushroomCastMode == MushroomMode.Unused ? 0 : DoMushroomCalcs(calcs, spellPower, spellHit, spellCrit);
            float mushroomEclipseDamage = mushroomBaseDamage * eclipseBonus;

            float barHalfSize = 100f;

            float insectSwarmRatio = RotationData.AverageInstantCast / iSw.DotEffect.Duration;

            float shootingStarsProcFrequency = (1 / iSw.DotEffect.TickLength + 1 / mf.DotEffect.TickLength) * 0.02f * talents.ShootingStars;
            float starsurgeCooldownBase = 15f + ss.CastTime + RotationData.AverageInstantCast;
            float starsurgeCooldownWithSSProcs = talents.ShootingStars > 0 ? 1 / shootingStarsProcFrequency * (1 - (float)Math.Exp(-starsurgeCooldownBase * shootingStarsProcFrequency)) : starsurgeCooldownBase;

            // Calculate the Starsurge average cast time, plus instants, to calculate the starsurge ratio
            float percentOfInstantStarsurges = shootingStarsProcFrequency * starsurgeCooldownWithSSProcs;
            float starsurgeCastTimeWithInstants = percentOfInstantStarsurges * RotationData.AverageInstantCast + (1 - percentOfInstantStarsurges) * ss.CastTime;

            float starSurgeRatio = starsurgeCastTimeWithInstants / starsurgeCooldownWithSSProcs;

            float starSurgeFrequency = 1 / starsurgeCooldownWithSSProcs;
            // This is a % cast time reduction
            float starfallReduction = RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? 1 / (1 + 5 * starSurgeFrequency) : 0f;

            float starsurgeEnergyRate = ss.AverageEnergy / starsurgeCooldownWithSSProcs;
            float starsurgeEnergyRateOnlySSProcs = ss.AverageEnergy * shootingStarsProcFrequency;

            float eclipseWAverageEnergy = w.AverageEnergy;
            // T12 4-piece set bonus
            w.AverageEnergy += calcs.BasicStats.BonusWrathEnergy;
            float wrathEnergyRate = w.AverageEnergy / w.CastTime;
            float wrathEclipseEnergyRate = eclipseWAverageEnergy / w.CastTime;
            float eclipseSFAverageEnergy = sf.AverageEnergy;
            // T12 4-piece set bonus
            sf.AverageEnergy += calcs.BasicStats.BonusStarfireEnergy;
            float starfireEnergyRate = sf.AverageEnergy / sf.CastTime;
            float starfireEclipseEnergyRate = eclipseSFAverageEnergy / sf.CastTime;

            float preLunarCasts = (barHalfSize - eclipseWAverageEnergy / 2 - w.BaseEnergy * talents.Euphoria * 0.12f * 2) * (1 - (starsurgeEnergyRate) / wrathEnergyRate) / w.AverageEnergy;
            float lunarCasts = (barHalfSize + eclipseSFAverageEnergy / 2) / eclipseSFAverageEnergy * (1 - (starsurgeEnergyRate) / starfireEclipseEnergyRate);
            float preSolarCasts = (barHalfSize - eclipseSFAverageEnergy / 2 - sf.BaseEnergy * (1 - (float)Math.Pow(1 - 0.12f * talents.Euphoria, 2))) * (1 - (starsurgeEnergyRate) / starfireEnergyRate) / sf.AverageEnergy;
            float solarCasts = (barHalfSize + eclipseWAverageEnergy / 2) / eclipseWAverageEnergy * (1 - (starsurgeEnergyRate) / wrathEclipseEnergyRate);

            float preLunarTime = preLunarCasts * w.CastTime;
            float lunarTime = lunarCasts * sf.CastTime;
            float preSolarTime = preSolarCasts * sf.CastTime;
            float solarTime = solarCasts * w.CastTime;

            float mainNukeDuration = preLunarTime + preSolarTime + lunarTime + solarTime;

            RotationData.SolarUptime = solarTime / mainNukeDuration;
            RotationData.LunarUptime = lunarTime / mainNukeDuration;

            // Wild Mushroom has a 1 sec GCD on placing mushrooms, no GCD on exploding
            float mushroomPlantTime = 3 * 1f;
            float detonateCooldown = 10f;

            float starfallCooldown = (90f - (talents.GlyphOfStarfall ? 30f : 0f)) * (talents.GlyphOfStarsurge ? starfallReduction : 1);
            float starfallRatio = talents.Starfall == 1 ? RotationData.AverageInstantCast / (starfallCooldown + RotationData.AverageInstantCast) : 0;
            float treantRatio = talents.ForceOfNature == 1 ? RotationData.AverageInstantCast / (180f + RotationData.AverageInstantCast) : 0;
            float mushroomRatio = RotationData.WildMushroomCastMode == MushroomMode.OnCooldown ? mushroomPlantTime / detonateCooldown : 0f;

            float percentOfMoonfiresExtended = 0f;
            if (talents.GlyphOfStarfire)
            {
                percentOfMoonfiresExtended = RotationData.MoonfireRefreshMode == DotMode.Twice ? 0.5f :
                    GetGlyphOfStarfireProbability(insectSwarmRatio, starSurgeRatio, starfallRatio, treantRatio, mushroomRatio,
                    lunarTime, preSolarTime, preLunarTime, solarTime,
                    mf.DotEffect.Duration, mfExtended.DotEffect.Duration, RotationData.AverageInstantCast);
            }
            float moonfireRatio = RotationData.AverageInstantCast / (percentOfMoonfiresExtended * mfExtended.DotEffect.Duration + (1 - percentOfMoonfiresExtended) * mf.DotEffect.Duration);

            float totalNonNukeRatio = (RotationData.MoonfireRefreshMode == DotMode.Always ? moonfireRatio : 0) +
                (RotationData.InsectSwarmRefreshMode == DotMode.Always ? insectSwarmRatio : 0) +
                (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starSurgeRatio : 0) +
                (RotationData.StarfallCastMode == StarfallMode.OnCooldown ? starfallRatio : 0) + treantRatio +
                (RotationData.WildMushroomCastMode == MushroomMode.OnCooldown ? mushroomRatio : 0);

            RotationData.WrathAvgCast = w.CastTime;
            RotationData.WrathAvgEnergy = w.AverageEnergy;

            RotationData.StarfireAvgCast = sf.CastTime;
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;

            float starSurgeTime = 0;
            // Starsurge is currently only on CD, but if it wasn't, this is where the time calculation would go

            float nukesAndNotOnCDDuration = mainNukeDuration +
                (RotationData.InsectSwarmRefreshMode == DotMode.Twice ? 2 * iSw.CastTime : 0) +
                (RotationData.MoonfireRefreshMode == DotMode.Twice ? 2 * mf.CastTime : 0) +
                starSurgeTime +
                (RotationData.StarfallCastMode == StarfallMode.LunarOnly ? RotationData.AverageInstantCast : 0) +
                (RotationData.WildMushroomCastMode == MushroomMode.SolarOnly ? mushroomPlantTime / detonateCooldown * RotationData.SolarUptime : 0);

            RotationData.Duration = nukesAndNotOnCDDuration / (1 - totalNonNukeRatio);

            if (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown)
            {
                starSurgeTime = RotationData.Duration * starSurgeRatio;
            }

            // Without glyph of Starsurge, you cannot fit a Starfall in every Lunar eclipse.
            // The actual result will be better than 1/2, because you will be able to cast SFall later in each Eclipse as the fight goes on,
            // but you will miss a Lunar proc entirely eventually.
            float starfallCooldownOverlap = starfallCooldown - RotationData.Duration;
            float rotationsToMiss = starfallCooldownOverlap > 0 ? lunarTime / starfallCooldownOverlap : 0f;
            float starfallFraction = rotationsToMiss > 0 ? (float)(Math.Ceiling(rotationsToMiss) / (Math.Ceiling(rotationsToMiss) + 1)) : 1f;
            RotationData.StarfallCasts = RotationData.StarfallCastMode == StarfallMode.OnCooldown ? starfallRatio * RotationData.Duration / RotationData.AverageInstantCast
                : (RotationData.StarfallCastMode == StarfallMode.LunarOnly ? starfallFraction : 0f);
            RotationData.TreantCasts = treantRatio * RotationData.Duration / RotationData.AverageInstantCast;
            RotationData.MushroomCasts = RotationData.WildMushroomCastMode == MushroomMode.OnCooldown ? mushroomRatio * RotationData.Duration / mushroomPlantTime
                : (RotationData.WildMushroomCastMode == MushroomMode.SolarOnly ? RotationData.SolarUptime * RotationData.Duration / detonateCooldown : 0f);
            RotationData.StarfallStars = MultipleTargetsPercentage * 20f + (1 - MultipleTargetsPercentage) * 10f;

            float moonfireTime = (RotationData.MoonfireRefreshMode == DotMode.Always) ? RotationData.Duration * moonfireRatio :
                (RotationData.MoonfireRefreshMode == DotMode.Twice ? 2 * mf.CastTime : 0);
            RotationData.MoonfireCasts = moonfireTime / mf.CastTime;
            RotationData.MoonfireTicks = RotationData.MoonfireCasts * mf.DotEffect.NumberOfTicks;
            RotationData.MoonfireDuration = mf.DotEffect.Duration;

            float insectSwarmTime = (RotationData.InsectSwarmRefreshMode == DotMode.Always) ? RotationData.Duration * insectSwarmRatio :
                (RotationData.InsectSwarmRefreshMode == DotMode.Twice ? 2 * iSw.CastTime : 0);
            RotationData.InsectSwarmCasts = insectSwarmTime / iSw.CastTime;
            RotationData.InsectSwarmTicks = RotationData.InsectSwarmCasts * iSw.DotEffect.NumberOfTicks;
            RotationData.InsectSwarmDuration = iSw.DotEffect.Duration;

            RotationData.StarfallDamage = RotationData.StarfallCastMode == StarfallMode.OnCooldown ?
                RotationData.LunarUptime * starfallEclipseDamage + (1 - RotationData.LunarUptime) * starfallBaseDamage :
                starfallEclipseDamage;

            RotationData.MushroomDamage = RotationData.WildMushroomCastMode == MushroomMode.OnCooldown ?
                RotationData.SolarUptime * mushroomEclipseDamage + (1 - RotationData.SolarUptime) * mushroomBaseDamage :
                mushroomEclipseDamage;

            RotationData.StarSurgeAvgCast = starsurgeCastTimeWithInstants;
            RotationData.StarSurgeAvgEnergy = ss.AverageEnergy;
            RotationData.StarSurgeAvgHit = (RotationData.SolarUptime + RotationData.LunarUptime) * ss.DamagePerHit * eclipseBonus + (1 - RotationData.SolarUptime - RotationData.LunarUptime) * ss.DamagePerHit;
            RotationData.StarSurgeCount = starSurgeTime / starsurgeCastTimeWithInstants;
            float starSurgeDamage = RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount;

            float preLunarDamage = preLunarCasts * w.DamagePerHit;
            float lunarDamage = lunarCasts * sf.DamagePerHit * eclipseBonus;
            float preSolarDamage = preSolarCasts * sf.DamagePerHit;
            float solarDamage = solarCasts * w.DamagePerHit * eclipseBonus;

            RotationData.StarfireCount = lunarCasts + preSolarCasts;
            RotationData.StarfireAvgHit = (lunarDamage + preSolarDamage) / RotationData.StarfireCount;
            RotationData.WrathCount = solarCasts + preLunarCasts;
            RotationData.WrathAvgHit = (solarDamage + preLunarDamage) / RotationData.WrathCount;

            float moonfireEclipseMultiplier = 1 + (eclipseBonus - 1) * (RotationData.MoonfireRefreshMode == DotMode.Always ? RotationData.LunarUptime + (talents.Sunfire > 0 ? RotationData.SolarUptime : 0) :
                (RotationData.MoonfireRefreshMode == DotMode.Twice ? (talents.Sunfire > 0 ? 1f : 0.5f) : 0));

            float insectSwarmEclipseMultiplier = 1 + (eclipseBonus - 1) * (RotationData.InsectSwarmRefreshMode == DotMode.Always ? RotationData.SolarUptime :
                (RotationData.InsectSwarmRefreshMode == DotMode.Twice ? 0.5f : 0));

            float unextendedAvgHit = (mf.DamagePerHit + mf.DotEffect.DamagePerHit) * moonfireEclipseMultiplier * (1 - RotationData.SolarUptime) +
                RotationData.SolarUptime * eclipseBonus * (mf.DamagePerHit + mf.DotEffect.DamagePerHit);
            float extendedAvgHit = (mfExtended.DamagePerHit + mfExtended.DotEffect.DamagePerHit) * moonfireEclipseMultiplier * (1 - RotationData.SolarUptime) +
                RotationData.SolarUptime * eclipseBonus * (mfExtended.DamagePerHit + mfExtended.DotEffect.DamagePerHit);
            RotationData.MoonfireAvgHit = percentOfMoonfiresExtended * extendedAvgHit + (1 - percentOfMoonfiresExtended) * unextendedAvgHit;
            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;
            RotationData.InsectSwarmAvgHit = iSw.DotEffect.DamagePerHit * insectSwarmEclipseMultiplier;
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

            return preSolarDamage + solarDamage + preLunarDamage + lunarDamage + moonfireDamage + insectSwarmDamage + starSurgeDamage + treantDamage + starfallDamage + mushroomDamage + T122PieceDamage;
        }

        /// <summary>
        /// One whale of a complicated expression generated from Mathematica.
        /// </summary>
        /// <param name="a">IS cast time ratio</param>
        /// <param name="b">SS cast time ratio</param>
        /// <param name="c">SFall cast time ratio</param>
        /// <param name="d">FoN cast time ratio</param>
        /// <param name="e">WM cast time ratio</param>
        /// <param name="F">Lunar time</param>
        /// <param name="G">Pre-solar time</param>
        /// <param name="H">Pre-lunar time</param>
        /// <param name="J">Solar time</param>
        /// <param name="r">Unextended MF duration</param>
        /// <param name="R">Extended MF duration</param>
        /// <param name="t">MF cast time</param>
        /// <returns>The % of Moonfire casts that will be extended by Glyph of Starfire</returns>
        private float GetGlyphOfStarfireProbability(float a, float b, float c, float d, float e, float F, float G, float H, float J, float r, float R, float t)
        {
            return (float)(Math.Sqrt(Math.Pow(a * F * r - a * F * R + a * G * r - a * G * R + a * Math.Pow(r, 2) - a * r * R + b * F * r - b * F * R + b * G * r - b * G * R + b * Math.Pow(r, 2) - b * r * R + c * F * r - c * F * R + c * G * r - c * G * R + c * Math.Pow(r, 2) - c * r * R + d * F * r - d * F * R + d * G * r - d * G * R + d * Math.Pow(r, 2) - d * r * R + e * F * r - e * F * R + e * G * r - e * G * R + e * Math.Pow(r, 2) - e * r * R - F * r + 2 * F * R - G * r + 2 * G * R + H * R + J * R - Math.Pow(r, 2) + r * R, 2) - 4 * (F * r - F * R + G * r - G * R + H * r - H * R + J * r - J * R) * (a * F * R + a * G * R + a * r * R + b * F * R + b * G * R + b * r * R + c * F * R + c * G * R + c * r * R + d * F * R + d * G * R + d * r * R + e * F * R + e * G * R + e * r * R - F * R + F * t - G * R + G * t - r * R + r * t)) - a * F * r + a * F * R - a * G * r + a * G * R - a * Math.Pow(r, 2) + a * r * R - b * F * r + b * F * R - b * G * r + b * G * R - b * Math.Pow(r, 2) + b * r * R - c * F * r + c * F * R - c * G * r + c * G * R - c * Math.Pow(r, 2) + c * r * R - d * F * r + d * F * R - d * G * r + d * G * R - d * Math.Pow(r, 2) + d * r * R - e * F * r + e * F * R - e * G * r + e * G * R - e * Math.Pow(r, 2) + e * r * R + F * r - 2 * F * R + G * r - 2 * G * R - H * R - J * R + Math.Pow(r, 2) - r * R) / (2 * (F * r - F * R + G * r - G * R + H * r - H * R + J * r - J * R));
        }
    }
}
