using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    public enum DotMode { Unused, Once, Twice, Always };
    public enum StarsurgeMode { Unused, InEclipse, OutOfEclipse, OnCooldown };
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
        public float InsectSwarmDuration { get; set; }
        public float MoonfireCasts { get; set; }
        public float MoonfireTicks { get; set; }
        public float MoonfireAvgHit { get; set; }
        public float MoonfireDuration { get; set; }
        public float StarfallDamage { get; set; }
        public float StarfallStars { get; set; }
        public float MushroomDamage { get; set; }
        public float TreantDamage { get; set; }
        public float SolarUptime { get; set; }
        public float LunarUptime { get; set; }
        public float NaturesGraceUptime { get; set; }
        public DotMode MoonfireRefreshMode { get; set; }
        public DotMode InsectSwarmRefreshMode { get; set; }
        public StarsurgeMode StarsurgeCastMode { get; set; }
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
            mainNuke.AverageEnergy = mainNuke.BaseEnergy * spellHit;
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

            float baseTickRate = dotSpell.DotEffect.BaseTickLength / (1 + spellHaste);
            float ngTickRate = baseTickRate / (1 + naturesGraceBonusHaste);

            int baseTicks = (int)Math.Round(dotSpell.DotEffect.BaseDuration / baseTickRate, 0);
            int ngTicks = (int)Math.Round(dotSpell.DotEffect.BaseDuration / ngTickRate, 0);

            dotSpell.DotEffect.NumberOfTicks = naturesGraceUptime * ngTicks + (1 - naturesGraceUptime) * baseTicks;

            float baseDuration = baseTickRate * baseTicks;
            float ngDuration = ngTickRate * ngTicks;

            dotSpell.DotEffect.Duration = naturesGraceUptime * ngDuration + (1 - naturesGraceUptime) * baseDuration;

            dotSpell.DotEffect.TickLength = (1 - naturesGraceUptime) * dotSpell.DotEffect.BaseTickLength / (1 + spellHaste) +
                naturesGraceUptime * dotSpell.DotEffect.BaseTickLength / (1 + spellHaste) / (1 + naturesGraceBonusHaste);

            float mfDirectDamage = (dotSpell.BaseDamage + dotSpell.SpellDamageModifier * spellPower) * overallDamageModifier;
            float mfCritDamage = mfDirectDamage * dotSpell.CriticalDamageModifier;
            float totalCritChance = spellCrit + dotSpell.CriticalChanceModifier;
            dotSpell.DamagePerHit = (totalCritChance * mfCritDamage + (1 - totalCritChance) * mfDirectDamage) * spellHit;
            float normalDamagePerTick = dotSpell.DotEffect.TickDamage + dotSpell.DotEffect.SpellDamageModifierPerTick * spellPower;
            float critDamagePerTick = normalDamagePerTick * dotSpell.CriticalDamageModifier;
            float damagePerTick = (totalCritChance * critDamagePerTick + (1 - totalCritChance) * normalDamagePerTick) * dotEffectDamageModifier;
            dotSpell.DotEffect.DamagePerHit = dotSpell.DotEffect.NumberOfTicks * damagePerTick * spellHit;
        }

        // Perform damage and mana calculations for all spells in the given rotation.  Returns damage done over the total duration.
        public float DamageDone(DruidTalents talents, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste, float masteryPoints)
        {
            Spell sf = new Spell(Solver.Starfire);
            Spell ss = new Spell(Solver.Starsurge);
            Spell w = new Spell(Solver.Wrath);
            Spell mf = new Spell(Solver.Moonfire);
            Spell iSw = new Spell(Solver.InsectSwarm);

            float eclipseBonus = 1 + calcs.EclipseBase + calcs.BasicStats.EclipseBonus;

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesTorment, RotationData.NaturesGraceUptime);
            DoMainNuke(calcs, ref ss, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesTorment, RotationData.NaturesGraceUptime);
            DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesTorment, RotationData.NaturesGraceUptime);
            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesTorment, RotationData.NaturesGraceUptime);
            DoDotSpell(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesTorment, RotationData.NaturesGraceUptime);

            float barHalfSize = 100f;

            RotationData.AverageInstantCast = mf.CastTime;
            float moonfireRatio = RotationData.AverageInstantCast / mf.DotEffect.Duration;
            float insectSwarmRatio = RotationData.AverageInstantCast / iSw.DotEffect.Duration;

            float shootingStarsProcFrequency = (1 / iSw.DotEffect.TickLength + 1 / mf.DotEffect.TickLength) * 0.02f * talents.ShootingStars;
            float starsurgeCooldownBase = 15f + ss.CastTime + RotationData.AverageInstantCast;
            float starsurgeCooldownWithSSProcs = talents.ShootingStars > 0 ? 1 / shootingStarsProcFrequency * (1 - (float)Math.Exp(-starsurgeCooldownBase * shootingStarsProcFrequency)) : starsurgeCooldownBase;

            // Calculate the Starsurge average cast time, plus instants, to calculate the starsurge ratio
            float percentOfInstantStarsurges = RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse || RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse
                ? 1 - (float)Math.Exp(-8 * shootingStarsProcFrequency)
                : shootingStarsProcFrequency * starsurgeCooldownWithSSProcs;
            float starsurgeCastTimeWithInstants = percentOfInstantStarsurges * mf.CastTime + (1 - percentOfInstantStarsurges) * ss.CastTime;

            float starSurgeRatio = starsurgeCastTimeWithInstants / starsurgeCooldownWithSSProcs;

            float starSurgeFrequency = 1 / starsurgeCooldownWithSSProcs;
            float starfallReduction = RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? 1 / (1 + 5 * starSurgeFrequency) : 0;

            float totalNonNukeRatio = (RotationData.MoonfireRefreshMode == DotMode.Always ? moonfireRatio : 0) +
                (RotationData.InsectSwarmRefreshMode == DotMode.Always ? insectSwarmRatio : 0) +
                (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starSurgeRatio : 0) +
                (talents.Starfall == 1 ? RotationData.AverageInstantCast / ((90f - (talents.GlyphOfStarfall ? 30f : 0f) * (talents.GlyphOfStarsurge ? starfallReduction : 1)) + RotationData.AverageInstantCast) : 0);
                //(talents.ForceOfNature == 1 ? RotationData.AverageInstantCast / (180f + RotationData.AverageInstantCast) : 0) +
                //(3 * 0.5f / 10f);

            float starsurgeEnergyRate = ss.AverageEnergy / starsurgeCooldownWithSSProcs;
            float starsurgeEnergyRateOnlySSProcs = ss.AverageEnergy * shootingStarsProcFrequency;

            float eclipseWAverageEnergy = w.AverageEnergy;
            w.AverageEnergy *= 1 + 0.12f * talents.Euphoria;
            float wrathEnergyRate = w.AverageEnergy / w.CastTime;
            float wrathEclipseEnergyRate = eclipseWAverageEnergy / w.CastTime;
            float eclipseSFAverageEnergy = sf.AverageEnergy;
            sf.AverageEnergy *= 1 + 0.12f * talents.Euphoria;
            float starfireEnergyRate = sf.AverageEnergy / sf.CastTime;
            float starfireEclipseEnergyRate = eclipseSFAverageEnergy / sf.CastTime;
            
            float solarCasts = 0;
            float preLunarCasts = 0;
            if (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse || RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse)
            {
                solarCasts = (float)Math.Ceiling((barHalfSize - (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse ? ss.AverageEnergy : 0)) / eclipseWAverageEnergy);
                preLunarCasts = (2 * barHalfSize - eclipseWAverageEnergy * solarCasts - ss.AverageEnergy) * (1 - starsurgeEnergyRateOnlySSProcs / wrathEnergyRate) / w.AverageEnergy + 0.12f * talents.Euphoria;
            }
            else
            {
                solarCasts = (barHalfSize + eclipseWAverageEnergy / 2) / eclipseWAverageEnergy * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate / wrathEclipseEnergyRate : 0));
                preLunarCasts = (barHalfSize - wrathEclipseEnergyRate / 2) * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate : 0) / wrathEnergyRate) / w.AverageEnergy + 0.12f * talents.Euphoria;
            }
            float lunarCasts = 0;
            float preSolarCasts = 0;
            if (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse || RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse)
            {
                lunarCasts = (float)Math.Ceiling((barHalfSize - (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse ? ss.AverageEnergy : 0)) / eclipseSFAverageEnergy);
                preSolarCasts = (2 * barHalfSize - eclipseSFAverageEnergy * lunarCasts - ss.AverageEnergy) * (1 - starsurgeEnergyRateOnlySSProcs / starfireEnergyRate) / sf.AverageEnergy + 0.12f * talents.Euphoria;
            }
            else
            {
                lunarCasts = (barHalfSize + eclipseSFAverageEnergy / 2) / eclipseSFAverageEnergy * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate / starfireEclipseEnergyRate : 0));
                preSolarCasts = (barHalfSize - starfireEclipseEnergyRate / 2) * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate : 0) / starfireEnergyRate) / sf.AverageEnergy + 0.12f * talents.Euphoria;
            }

            float preLunarTime = preLunarCasts * w.CastTime;
            float lunarTime = lunarCasts * sf.CastTime;
            float preSolarTime = preSolarCasts * sf.CastTime;
            float solarTime = solarCasts * w.CastTime;

            float mainNukeDuration = preLunarTime + preSolarTime + lunarTime + solarTime;

            RotationData.WrathAvgCast = w.CastTime;
            RotationData.WrathAvgEnergy = w.AverageEnergy;

            RotationData.StarfireAvgCast = sf.CastTime;
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;

            float starSurgeTime = 0;
            if (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse || RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse)
            {
                starSurgeTime = starsurgeCastTimeWithInstants * (2 + shootingStarsProcFrequency * (preLunarTime + preSolarTime) * 1 / (1 - totalNonNukeRatio));
            }

            float nukesAndNotOnCDDuration = mainNukeDuration +
                (RotationData.InsectSwarmRefreshMode == DotMode.Once ? iSw.CastTime :
                    (RotationData.InsectSwarmRefreshMode == DotMode.Twice ? 2 * iSw.CastTime : 0)) +
                (RotationData.MoonfireRefreshMode == DotMode.Once ? mf.CastTime :
                    (RotationData.MoonfireRefreshMode == DotMode.Twice ? 2 * mf.CastTime : 0)) +
                    starSurgeTime;

            RotationData.Duration = nukesAndNotOnCDDuration / (1 - totalNonNukeRatio);

            if (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown)
            {
                starSurgeTime = RotationData.Duration * starSurgeRatio;
            }

            float moonfireTime = (RotationData.MoonfireRefreshMode == DotMode.Always) ? RotationData.Duration * moonfireRatio :
                (RotationData.MoonfireRefreshMode == DotMode.Once ? mf.CastTime :
                (RotationData.MoonfireRefreshMode == DotMode.Twice ? 2 * mf.CastTime : 0));
            RotationData.MoonfireCasts = moonfireTime / mf.CastTime;
            RotationData.MoonfireTicks = RotationData.MoonfireCasts * mf.DotEffect.NumberOfTicks;
            RotationData.MoonfireDuration = mf.DotEffect.Duration;

            float insectSwarmTime = (RotationData.InsectSwarmRefreshMode == DotMode.Always) ? RotationData.Duration * insectSwarmRatio :
                (RotationData.InsectSwarmRefreshMode == DotMode.Once ? iSw.CastTime :
                (RotationData.InsectSwarmRefreshMode == DotMode.Twice ? 2 * iSw.CastTime : 0));
            RotationData.InsectSwarmCasts = insectSwarmTime / iSw.CastTime;
            RotationData.InsectSwarmTicks = RotationData.InsectSwarmCasts * iSw.DotEffect.NumberOfTicks;
            RotationData.InsectSwarmDuration = iSw.DotEffect.Duration;

            float insectSwarmUptime = insectSwarmTime * iSw.DotEffect.Duration / iSw.CastTime / RotationData.Duration;

            RotationData.SolarUptime = solarTime / mainNukeDuration;
            RotationData.LunarUptime = lunarTime / mainNukeDuration;

            RotationData.StarSurgeAvgCast = starsurgeCastTimeWithInstants;
            RotationData.StarSurgeAvgEnergy = ss.AverageEnergy;
            RotationData.StarSurgeAvgHit = (RotationData.SolarUptime + RotationData.LunarUptime) * ss.DamagePerHit * eclipseBonus + (1 - RotationData.SolarUptime - RotationData.LunarUptime) * ss.DamagePerHit;
            RotationData.StarSurgeCount = starSurgeTime / starsurgeCastTimeWithInstants;
            float starSurgeDamage = RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount;

            float preLunarDamage = preLunarCasts * w.DamagePerHit * (1 + (talents.GlyphOfWrath ? insectSwarmUptime * 0.1f : 0f));
            float lunarDamage = lunarCasts * sf.DamagePerHit * eclipseBonus;
            float preSolarDamage = preSolarCasts * sf.DamagePerHit;
            float solarDamage = solarCasts * w.DamagePerHit * eclipseBonus * (1 + (talents.GlyphOfWrath ? insectSwarmUptime * 0.1f : 0f));

            RotationData.StarfireCount = lunarCasts + preSolarCasts;
            RotationData.StarfireAvgHit = (lunarDamage + preSolarDamage) / RotationData.StarfireCount;
            RotationData.WrathCount = solarCasts + preLunarCasts;
            RotationData.WrathAvgHit = (solarDamage + preLunarDamage) / RotationData.WrathCount;

            if (talents.GlyphOfStarfire)
            {
                Spell mfExtended = new Spell(mf);
                mfExtended.DotEffect.BaseDuration += 9.0f;
                DoDotSpell(calcs, ref mfExtended, spellPower, spellHit, spellCrit, spellHaste, 0.05f * talents.NaturesTorment, RotationData.NaturesGraceUptime);

                switch (RotationData.MoonfireRefreshMode)
                {
                    case DotMode.Once:
                        RotationData.MoonfireAvgHit = (mfExtended.DamagePerHit + mfExtended.DotEffect.DamagePerHit) * eclipseBonus;
                        break;
                    case DotMode.Twice:
                        RotationData.MoonfireAvgHit = ((mfExtended.DamagePerHit + mfExtended.DotEffect.DamagePerHit) * eclipseBonus + (mf.DamagePerHit + mf.DotEffect.DamagePerHit) * (talents.Sunfire > 0 ? eclipseBonus : 1f)) / 2f;
                        break;
                    case DotMode.Always:
                        RotationData.MoonfireAvgHit = RotationData.LunarUptime * (mfExtended.DamagePerHit + mfExtended.DotEffect.DamagePerHit) * eclipseBonus + (talents.Sunfire > 0 ? RotationData.SolarUptime * (mf.DamagePerHit + mf.DotEffect.DamagePerHit) * eclipseBonus : 0) + (preSolarTime / RotationData.Duration) * (mfExtended.DamagePerHit + mfExtended.DotEffect.DamagePerHit) + (1 - (talents.Sunfire > 0 ? RotationData.SolarUptime : 0) - RotationData.LunarUptime - (preSolarTime / RotationData.Duration)) * (mf.DamagePerHit + mf.DotEffect.DamagePerHit);
                        break;
                    case DotMode.Unused:
                    default:
                        break;
                }
            }
            else
            {
                RotationData.MoonfireAvgHit = RotationData.LunarUptime * (mf.DamagePerHit + mf.DotEffect.DamagePerHit) * eclipseBonus + (talents.Sunfire > 0 ? RotationData.SolarUptime * (mf.DamagePerHit + mf.DotEffect.DamagePerHit) * eclipseBonus : 0) + (1 - (talents.Sunfire > 0 ? RotationData.SolarUptime : 0) - RotationData.LunarUptime) * (mf.DamagePerHit + mf.DotEffect.DamagePerHit);
            }
            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;
            RotationData.InsectSwarmAvgHit = RotationData.SolarUptime * iSw.DotEffect.DamagePerHit * eclipseBonus + (1 - RotationData.SolarUptime) * iSw.DotEffect.DamagePerHit;
            float insectSwarmDamage = RotationData.InsectSwarmAvgHit * RotationData.InsectSwarmCasts;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.InsectSwarmCasts;
            RotationData.DotTicks = RotationData.InsectSwarmTicks + RotationData.MoonfireTicks;
            RotationData.ManaUsed = RotationData.WrathCount * w.BaseManaCost +
                RotationData.StarfireCount * sf.BaseManaCost +
                RotationData.StarSurgeCount * ss.BaseManaCost +
                RotationData.MoonfireCasts * mf.BaseManaCost +
                RotationData.InsectSwarmCasts * iSw.BaseManaCost;

            float manaSavingsFromOOC = 0.06f * (RotationData.MoonfireCasts / RotationData.CastCount * mf.BaseManaCost) +
                0.06f * (RotationData.InsectSwarmCasts / RotationData.CastCount * iSw.BaseManaCost) +
                0.06f * (RotationData.StarfireCount / RotationData.CastCount * sf.BaseManaCost) +
                0.06f * (RotationData.WrathCount / RotationData.CastCount * w.BaseManaCost) +
                0.06f * (RotationData.StarSurgeCount / RotationData.CastCount * ss.BaseManaCost);

            RotationData.ManaUsed -= manaSavingsFromOOC;

            RotationData.ManaGained = 2 * 0.08f * talents.Euphoria * calcs.BasicStats.Mana;

            return preSolarDamage + solarDamage + preLunarDamage + lunarDamage + moonfireDamage + insectSwarmDamage + starSurgeDamage;
        }
    }
}
