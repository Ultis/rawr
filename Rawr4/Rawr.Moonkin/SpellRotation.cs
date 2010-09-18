using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    public enum EclipseMode { Unused, Standard, Stretched };
    public enum DotMode { Unused, Once, Twice, Always };
    public enum StarsurgeMode { Unused, InEclipse, OutOfEclipse, OnCooldown };
    // Rotation information for display to the user.
    public class RotationData
    {
        public float DPS = 0.0f;
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
        public float TreantDamage { get; set; }
        public DotMode MoonfireRefreshMode { get; set; }
        public DotMode InsectSwarmRefreshMode { get; set; }
        public StarsurgeMode StarsurgeCastMode { get; set; }
        public EclipseMode LunarEclipseMode { get; set; }
        public EclipseMode SolarEclipseMode { get; set; }
    }

    // Our old friend the spell rotation.
    public class SpellRotation
    {
        public MoonkinSolver Solver { get; set; }
        public RotationData RotationData = new RotationData();

        // Calculate damage and casting time for a single, direct-damage spell.
        public void DoMainNuke(CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;

            float overallDamageModifier = mainNuke.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
            overallDamageModifier *= mainNuke.School == SpellSchool.Arcane ? (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) : (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
            overallDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);

            float gcd = 1.5f / (1.0f + spellHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;

            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            mainNuke.CastTime = (float)Math.Max(mainNuke.BaseCastTime / (1 + spellHaste), instantCast);
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * spellPower) * overallDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier * (1 + calcs.BasicStats.MoonkinT10CritDot);
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
            mainNuke.AverageEnergy = totalCritChance * (mainNuke.BaseEnergy + mainNuke.CriticalEnergy) + (1 - totalCritChance) * mainNuke.BaseEnergy;
        }

        // Calculate damage and casting time for a damage-over-time effect.
        private void DoDotSpell(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;
            float schoolMultiplier = dotSpell.School == SpellSchool.Arcane ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier;

            float overallDamageModifier = dotSpell.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier);
            overallDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);

            float dotEffectDamageModifier = dotSpell.DotEffect.AllDamageModifier * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + schoolMultiplier);
            dotEffectDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);

            float gcd = 1.5f / (1.0f + spellHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;
            dotSpell.CastTime = (float)Math.Max(dotSpell.CastTime / (1 + spellHaste), instantCast);

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
            Spell sUf = new Spell(Solver.Moonfire);
            sUf.School = SpellSchool.Nature;

            Spell eclipseSF = new Spell(Solver.Starfire);
            Spell eclipseW = new Spell(Solver.Wrath);
            Spell eclipseMF = new Spell(Solver.Moonfire);
            Spell eclipseIS = new Spell(Solver.InsectSwarm);
            Spell eclipseSS = new Spell(Solver.Starsurge);
            Spell eclipseSuF = new Spell(sUf);

            float eclipseBonus = 1 + calcs.EclipseBase + calcs.BasicStats.EclipseBonus;

            eclipseSF.AllDamageModifier *= eclipseBonus;
            eclipseW.AllDamageModifier *= eclipseBonus;
            eclipseSS.AllDamageModifier *= eclipseBonus;
            eclipseMF.AllDamageModifier *= eclipseBonus;
            eclipseMF.DotEffect.AllDamageModifier *= eclipseBonus;
            eclipseIS.DotEffect.AllDamageModifier *= eclipseBonus;
            eclipseSuF.AllDamageModifier *= eclipseBonus;
            eclipseSuF.DotEffect.AllDamageModifier *= eclipseBonus;

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseSF, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref ss, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseSS, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseW, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseMF, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref sUf, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseSuF, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseIS, spellPower, spellHit, spellCrit, spellHaste);

            float barHalfSize = 100f;
            float starSurgeCD = 15.0f + ss.CastTime;
            float wrathEnergyRate = w.AverageEnergy / w.CastTime;
            float starfireEnergyRate = sf.AverageEnergy / sf.CastTime;
            float starsurgeEnergyRate = ss.AverageEnergy / starSurgeCD;

            float moonfireRatio = mf.CastTime / mf.DotEffect.Duration;
            float insectSwarmRatio = iSw.CastTime / iSw.DotEffect.Duration;
            float starSurgeRatio = ss.CastTime / starSurgeCD;
            float mainNukeRatio = (1 - moonfireRatio) * (1 - insectSwarmRatio) * (1 - starSurgeRatio);
            float sfRatioAtZeroEnergy = wrathEnergyRate / (wrathEnergyRate + starfireEnergyRate);

            float preLunarTime = RotationData.LunarEclipseMode == EclipseMode.Unused ? 0 : (RotationData.LunarEclipseMode == EclipseMode.Stretched ? 2 : (barHalfSize + (RotationData.SolarEclipseMode == EclipseMode.Unused ? sf.AverageEnergy / 2 : -w.AverageEnergy / 2f) - (RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse ? ss.AverageEnergy : 0)) / w.AverageEnergy * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate : 0) / wrathEnergyRate) + (RotationData.SolarEclipseMode == EclipseMode.Unused ? 1 : 0)) * w.CastTime;

            float lunarTime = RotationData.LunarEclipseMode == EclipseMode.Unused ? 0 : (barHalfSize + eclipseSF.AverageEnergy / 2f + (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse ? ss.AverageEnergy : 0)) / eclipseSF.AverageEnergy * (1 + (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate : 0) / starfireEnergyRate) + 1 * eclipseSF.CastTime;

            float remainingLunarTime = 45f * mainNukeRatio - lunarTime - (RotationData.SolarEclipseMode == EclipseMode.Unused ? preLunarTime : 0);
            float addedLunarTime = remainingLunarTime * (sfRatioAtZeroEnergy + starsurgeEnergyRate / (wrathEnergyRate + starfireEnergyRate));
            float addedPreLunarTime = remainingLunarTime - addedLunarTime;
            if (RotationData.LunarEclipseMode == EclipseMode.Stretched)
            {
                preLunarTime += addedPreLunarTime;
                lunarTime += addedLunarTime;
            }

            float preSolarTime = RotationData.SolarEclipseMode == EclipseMode.Unused ? 0 : (RotationData.LunarEclipseMode == EclipseMode.Stretched ? 1 : (barHalfSize + (RotationData.LunarEclipseMode == EclipseMode.Unused ? w.AverageEnergy / 2f : -sf.AverageEnergy / 2f) - (RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse ? ss.AverageEnergy : 0)) / sf.AverageEnergy * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate : 0) / starfireEnergyRate) - (RotationData.LunarEclipseMode == EclipseMode.Unused ? 0 : 1)) * sf.CastTime;

            float solarTime = RotationData.SolarEclipseMode == EclipseMode.Unused ? 0 : (barHalfSize + eclipseW.AverageEnergy / 2f + (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse ? ss.AverageEnergy : 0)) / eclipseW.AverageEnergy * (1 + (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate : 0) / wrathEnergyRate) * eclipseW.CastTime;

            float remainingSolarTime = 45f * mainNukeRatio - solarTime - (RotationData.LunarEclipseMode == EclipseMode.Unused ? preSolarTime : 0);
            float addedSolarTime = remainingSolarTime * (1 - sfRatioAtZeroEnergy + starsurgeEnergyRate / (wrathEnergyRate + starfireEnergyRate));
            float addedPreSolarTime = remainingSolarTime - addedSolarTime;

            if (RotationData.SolarEclipseMode == EclipseMode.Stretched)
            {
                preSolarTime += addedPreSolarTime;
                solarTime += addedSolarTime;
            }

            RotationData.WrathAvgCast = w.CastTime;
            RotationData.WrathAvgEnergy = w.AverageEnergy;

            RotationData.StarfireAvgCast = sf.CastTime;
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;

            float accumulatedDuration = preLunarTime + preSolarTime + lunarTime + solarTime;

            float moonfireTime = 0f;
            switch (RotationData.MoonfireRefreshMode)
            {
                case DotMode.Always:
                    moonfireTime = moonfireRatio * accumulatedDuration / (1 - moonfireRatio);
                    RotationData.MoonfireCasts = moonfireTime / mf.CastTime;
                    break;
                case DotMode.Once:
                    accumulatedDuration += mf.CastTime;
                    RotationData.MoonfireCasts = 1;
                    break;
                case DotMode.Twice:
                    accumulatedDuration += 2 * mf.CastTime;
                    RotationData.MoonfireCasts = 2;
                    break;
                case DotMode.Unused:
                    break;
            }
            RotationData.MoonfireTicks = RotationData.MoonfireCasts * mf.DotEffect.NumberOfTicks;
            RotationData.MoonfireDuration = mf.DotEffect.Duration;
            RotationData.AverageInstantCast = mf.CastTime;

            float insectSwarmTime = 0f;
            switch (RotationData.InsectSwarmRefreshMode)
            {
                case DotMode.Always:
                    insectSwarmTime = insectSwarmRatio * accumulatedDuration / (1 - insectSwarmRatio);
                    RotationData.InsectSwarmCasts = insectSwarmTime / iSw.CastTime;
                    break;
                case DotMode.Once:
                    accumulatedDuration += iSw.CastTime;
                    RotationData.InsectSwarmCasts = 1;
                    break;
                case DotMode.Twice:
                    accumulatedDuration += 2 * iSw.CastTime;
                    RotationData.InsectSwarmCasts = 2;
                    break;
                case DotMode.Unused:
                    break;
            }
            RotationData.InsectSwarmTicks = RotationData.InsectSwarmCasts * iSw.DotEffect.NumberOfTicks;
            RotationData.InsectSwarmDuration = iSw.DotEffect.Duration;
            float insectSwarmUptime = RotationData.InsectSwarmCasts * iSw.DotEffect.Duration / accumulatedDuration;

            float starSurgeTime = 0f;
            switch (RotationData.StarsurgeCastMode)
            {
                case StarsurgeMode.OnCooldown:
                    starSurgeTime = starSurgeRatio * accumulatedDuration / (1 - starSurgeRatio);
                    RotationData.StarSurgeCount = starSurgeTime / ss.CastTime;
                    break;
                case StarsurgeMode.InEclipse:
                case StarsurgeMode.OutOfEclipse:
                    accumulatedDuration += ss.CastTime * 2;
                    RotationData.StarSurgeCount = 2;
                    break;
                case StarsurgeMode.Unused:
                    break;
            }

            float solarUptime = (solarTime + (RotationData.SolarEclipseMode == EclipseMode.Stretched ? preSolarTime : 0)) / accumulatedDuration;
            float lunarUptime = (lunarTime + (RotationData.LunarEclipseMode == EclipseMode.Stretched ? preLunarTime : 0)) / accumulatedDuration;

            float timeInSolar = (preSolarTime + solarTime) / accumulatedDuration;
            float timeInLunar = (preLunarTime + lunarTime) / accumulatedDuration;

            RotationData.StarSurgeAvgCast = ss.CastTime;
            RotationData.StarSurgeAvgEnergy = ss.AverageEnergy * timeInSolar + ss.AverageEnergy * timeInLunar;
            RotationData.StarSurgeAvgHit = (solarTime + lunarUptime) * eclipseSS.DamagePerHit + (1 - solarUptime - lunarUptime) * ss.DamagePerHit;
            float starSurgeDamage = RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount;

            float preLunarCasts = preLunarTime / w.CastTime;
            float lunarCasts = lunarTime / eclipseSF.CastTime;
            float preSolarCasts = preSolarTime / sf.CastTime;
            float solarCasts = solarTime / eclipseW.CastTime;
            float preLunarDamage = preLunarCasts * w.DamagePerHit * (1 + (talents.GlyphOfWrath ? insectSwarmUptime * 0.1f : 0f));
            float lunarDamage = lunarCasts * eclipseSF.DamagePerHit;
            float preSolarDamage = preSolarCasts * sf.DamagePerHit;
            float solarDamage = solarCasts * eclipseW.DamagePerHit * (1 + (talents.GlyphOfWrath ? insectSwarmUptime * 0.1f : 0f));

            RotationData.StarfireCount = lunarCasts + preSolarCasts;
            RotationData.StarfireAvgHit = (lunarDamage + preSolarDamage) / RotationData.StarfireCount;
            RotationData.WrathCount = solarCasts + preLunarCasts;
            RotationData.WrathAvgHit = (solarDamage + preLunarDamage) / RotationData.WrathCount;

            RotationData.MoonfireAvgHit = lunarUptime * (eclipseMF.DamagePerHit + eclipseMF.DotEffect.DamagePerHit) + (1 - lunarUptime) * (mf.DamagePerHit + mf.DotEffect.DamagePerHit);
            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;
            RotationData.InsectSwarmAvgHit = solarUptime * eclipseIS.DotEffect.DamagePerHit + (1 - solarUptime) * iSw.DotEffect.DamagePerHit;
            float insectSwarmDamage = RotationData.InsectSwarmAvgHit * RotationData.InsectSwarmCasts;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.InsectSwarmCasts;
            RotationData.DotTicks = RotationData.InsectSwarmTicks + RotationData.MoonfireTicks;
            RotationData.Duration = accumulatedDuration + insectSwarmTime + moonfireTime + starSurgeTime;
            RotationData.ManaUsed = RotationData.WrathCount * w.BaseManaCost +
                RotationData.StarfireCount * sf.BaseManaCost +
                RotationData.StarSurgeCount * ss.BaseManaCost +
                RotationData.MoonfireCasts * mf.BaseManaCost +
                RotationData.InsectSwarmCasts * iSw.BaseManaCost;

            RotationData.ManaGained = 2 * 0.06f * talents.Euphoria * calcs.BasicStats.Mana;

            return preSolarDamage + solarDamage + preLunarDamage + lunarDamage + moonfireDamage + insectSwarmDamage + starSurgeDamage;
        }
    }
}
