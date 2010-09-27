using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
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
        public float SolarUptime { get; set; }
        public float LunarUptime { get; set; }
        public DotMode MoonfireRefreshMode { get; set; }
        public DotMode InsectSwarmRefreshMode { get; set; }
        public StarsurgeMode StarsurgeCastMode { get; set; }
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
            // Add a check for the higher of the two spell schools, as Starsurge always chooses the higher one
            overallDamageModifier *= mainNuke.School == SpellSchool.Arcane ? (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) :
                (mainNuke.School == SpellSchool.Nature ? (1 + calcs.BasicStats.BonusNatureDamageMultiplier) :
                (1 + (calcs.BasicStats.BonusArcaneDamageMultiplier > calcs.BasicStats.BonusNatureDamageMultiplier ? calcs.BasicStats.BonusArcaneDamageMultiplier : calcs.BasicStats.BonusNatureDamageMultiplier)));
            overallDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);

            float gcd = 1.5f / (1.0f + spellHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;

            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            mainNuke.CastTime = (float)Math.Max(mainNuke.BaseCastTime / (1 + spellHaste), instantCast);
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * spellPower) * overallDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier * (1 + calcs.BasicStats.MoonkinT10CritDot);
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
            mainNuke.AverageEnergy = mainNuke.BaseEnergy;
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
            dotSpell.CastTime = instantCast;

            dotSpell.DotEffect.TickLength = dotSpell.DotEffect.BaseTickLength / (1 + spellHaste);

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
            Spell mfExtended = new Spell(mf);
            mfExtended.DotEffect.Duration += talents.GlyphOfStarfire ? 9.0f : 0.0f;

            Spell eclipseSF = new Spell(Solver.Starfire);
            Spell eclipseW = new Spell(Solver.Wrath);
            Spell eclipseMF = new Spell(Solver.Moonfire);
            Spell eclipseIS = new Spell(Solver.InsectSwarm);
            Spell eclipseSS = new Spell(Solver.Starsurge);
            Spell eclipseSuF = new Spell(eclipseMF);
            Spell eclipseMFExtended = new Spell(mfExtended);
            Spell eclipseSuFExtended = new Spell(mfExtended);
            eclipseSuF.School = SpellSchool.Nature;
            eclipseSuFExtended.School = SpellSchool.Nature;

            float eclipseBonus = 1 + calcs.EclipseBase + calcs.BasicStats.EclipseBonus;

            eclipseSF.AllDamageModifier *= eclipseBonus;
            eclipseW.AllDamageModifier *= eclipseBonus;
            eclipseSS.AllDamageModifier *= eclipseBonus;
            eclipseMF.AllDamageModifier *= eclipseBonus;
            eclipseMF.DotEffect.AllDamageModifier *= eclipseBonus;
            eclipseMFExtended.AllDamageModifier *= eclipseBonus;
            eclipseMFExtended.DotEffect.AllDamageModifier *= eclipseBonus;
            eclipseIS.DotEffect.AllDamageModifier *= eclipseBonus;
            eclipseSuF.AllDamageModifier *= eclipseBonus;
            eclipseSuF.DotEffect.AllDamageModifier *= eclipseBonus;
            eclipseSuFExtended.AllDamageModifier *= eclipseBonus;
            eclipseSuFExtended.DotEffect.AllDamageModifier *= eclipseBonus;

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseSF, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref ss, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseSS, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseW, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref mfExtended, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseMF, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseSuF, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseMFExtended, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseSuFExtended, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseIS, spellPower, spellHit, spellCrit, spellHaste);

            float barHalfSize = 100f;

            RotationData.AverageInstantCast = mf.CastTime;
            float moonfireRatio = mf.CastTime / mf.DotEffect.Duration;
            float insectSwarmRatio = iSw.CastTime / iSw.DotEffect.Duration;
            float totalNonNukeRatio = (RotationData.MoonfireRefreshMode == DotMode.Always ? moonfireRatio : 0) +
                (RotationData.InsectSwarmRefreshMode == DotMode.Always ? insectSwarmRatio : 0) +
                (talents.Starfall == 1 ? RotationData.AverageInstantCast / (90f - (talents.GlyphOfStarfall ? 30f : 0f) + RotationData.AverageInstantCast) : 0) +
                (talents.ForceOfNature == 1 ? RotationData.AverageInstantCast / (360f + RotationData.AverageInstantCast) : 0);

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

            float shootingStarsProcFrequency = (RotationData.InsectSwarmTicks + RotationData.MoonfireTicks) * 0.02f * talents.ShootingStars;
            float starsurgeCooldownBase = 15f + ss.CastTime;
            float starsurgeCooldownWithSSProcs = 1 / (shootingStarsProcFrequency + 1 / (starsurgeCooldownBase * (1 + starsurgeCooldownBase * shootingStarsProcFrequency / 2)));

            float starsurgeEnergyRate = ss.AverageEnergy / starsurgeCooldownWithSSProcs;
            float starsurgeEnergyRateOnlySSProcs = ss.AverageEnergy * shootingStarsProcFrequency;

            float starSurgeCD = 15.0f + ss.CastTime;
            w.AverageEnergy *= 1 + 0.12f * talents.Euphoria;
            float wrathEnergyRate = w.AverageEnergy / w.CastTime;
            float wrathEclipseEnergyRate = eclipseW.AverageEnergy / eclipseW.CastTime;
            sf.AverageEnergy *= 1 + 0.12f * talents.Euphoria;
            float starfireEnergyRate = sf.AverageEnergy / sf.CastTime;
            float starfireEclipseEnergyRate = eclipseSF.AverageEnergy / eclipseSF.CastTime;
            
            float solarCasts = 0;
            float preLunarCasts = 0;
            if (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse || RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse)
            {
                solarCasts = (float)Math.Ceiling((barHalfSize - (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse ? ss.AverageEnergy : 0)) / eclipseW.AverageEnergy);
                preLunarCasts = (2 * barHalfSize - eclipseW.AverageEnergy * solarCasts - ss.AverageEnergy) * (1 - starsurgeEnergyRateOnlySSProcs / wrathEnergyRate) / w.AverageEnergy + 0.12f * talents.Euphoria;
            }
            else
            {
                solarCasts = (barHalfSize + eclipseW.AverageEnergy / 2) / eclipseW.AverageEnergy * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate / wrathEclipseEnergyRate : 0));
                preLunarCasts = (barHalfSize - wrathEclipseEnergyRate / 2) * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate : 0) / wrathEnergyRate) / w.AverageEnergy + 0.12f * talents.Euphoria;
            }
            float lunarCasts = 0;
            float preSolarCasts = 0;
            if (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse || RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse)
            {
                lunarCasts = (float)Math.Ceiling((barHalfSize - (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse ? ss.AverageEnergy : 0)) / eclipseSF.AverageEnergy);
                preSolarCasts = (2 * barHalfSize - eclipseSF.AverageEnergy * lunarCasts - ss.AverageEnergy) * (1 - starsurgeEnergyRateOnlySSProcs / starfireEnergyRate) / sf.AverageEnergy + 0.12f * talents.Euphoria;
            }
            else
            {
                lunarCasts = (barHalfSize + eclipseSF.AverageEnergy / 2) / eclipseSF.AverageEnergy * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate / starfireEclipseEnergyRate : 0));
                preLunarCasts = (barHalfSize - starfireEclipseEnergyRate / 2) * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? starsurgeEnergyRate : 0) / starfireEnergyRate) / sf.AverageEnergy + 0.12f * talents.Euphoria;
            }

            float preLunarTime = preLunarCasts * w.CastTime;
            float lunarTime = lunarCasts * eclipseSF.CastTime;
            float preSolarTime = preSolarCasts * sf.CastTime;
            float solarTime = solarCasts * eclipseW.CastTime;

            RotationData.WrathAvgCast = w.CastTime;
            RotationData.WrathAvgEnergy = w.AverageEnergy;

            RotationData.StarfireAvgCast = sf.CastTime;
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;

            float mainNukeDuration = preLunarTime + preSolarTime + lunarTime + solarTime;
            float nukesAndNotOnCDDuration = mainNukeDuration +
                (RotationData.InsectSwarmRefreshMode == DotMode.Once ? iSw.CastTime :
                    (RotationData.InsectSwarmRefreshMode == DotMode.Twice ? 2 * iSw.CastTime : 0)) +
                (RotationData.MoonfireRefreshMode == DotMode.Once ? mf.CastTime :
                    (RotationData.MoonfireRefreshMode == DotMode.Twice ? 2 * mf.CastTime : 0));

            RotationData.Duration = nukesAndNotOnCDDuration / (1 - totalNonNukeRatio);

            float starSurgeRatio = ss.CastTime / (starsurgeCooldownWithSSProcs + RotationData.AverageInstantCast);
            float starSurgeTime = 0;
            if (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown)
            {
                starSurgeTime = RotationData.Duration * starSurgeRatio;
            }
            else if (RotationData.StarsurgeCastMode != StarsurgeMode.Unused)
            {
                starSurgeTime = ss.CastTime * (2 + shootingStarsProcFrequency * (preLunarTime + preSolarTime) * 1 / (1 - totalNonNukeRatio));
            }

            RotationData.SolarUptime = solarTime / mainNukeDuration;
            RotationData.LunarUptime = lunarTime / mainNukeDuration;

            RotationData.StarSurgeAvgCast = ss.CastTime;
            RotationData.StarSurgeAvgEnergy = ss.AverageEnergy;
            RotationData.StarSurgeAvgHit = (RotationData.SolarUptime + RotationData.LunarUptime) * eclipseSS.DamagePerHit + (1 - RotationData.SolarUptime - RotationData.LunarUptime) * ss.DamagePerHit;
            RotationData.StarSurgeCount = starSurgeTime / ss.CastTime;
            float starSurgeDamage = RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount;

            float preLunarDamage = preLunarCasts * w.DamagePerHit * (1 + (talents.GlyphOfWrath ? insectSwarmUptime * 0.1f : 0f));
            float lunarDamage = lunarCasts * eclipseSF.DamagePerHit;
            float preSolarDamage = preSolarCasts * sf.DamagePerHit;
            float solarDamage = solarCasts * eclipseW.DamagePerHit * (1 + (talents.GlyphOfWrath ? insectSwarmUptime * 0.1f : 0f));

            RotationData.StarfireCount = lunarCasts + preSolarCasts;
            RotationData.StarfireAvgHit = (lunarDamage + preSolarDamage) / RotationData.StarfireCount;
            RotationData.WrathCount = solarCasts + preLunarCasts;
            RotationData.WrathAvgHit = (solarDamage + preLunarDamage) / RotationData.WrathCount;

            float moonfireExtendedPercent = (RotationData.MoonfireRefreshMode == DotMode.Once ? 1 :
                (RotationData.MoonfireRefreshMode == DotMode.Twice ? 0.5f :
                (RotationData.MoonfireRefreshMode == DotMode.Unused ? 0 :
                (lunarTime + preSolarTime) / mainNukeDuration + mf.DotEffect.Duration / RotationData.Duration)));
            float averageUnextendedHit = RotationData.LunarUptime * (eclipseMF.DamagePerHit + eclipseMF.DotEffect.DamagePerHit) + RotationData.SolarUptime * (eclipseSuF.DamagePerHit + eclipseSuF.DotEffect.DamagePerHit) + (1 - RotationData.SolarUptime - RotationData.LunarUptime) * (mf.DamagePerHit + mf.DotEffect.DamagePerHit);
            float averageExtendedHit = RotationData.LunarUptime * (eclipseMFExtended.DamagePerHit + eclipseMFExtended.DotEffect.DamagePerHit) + RotationData.SolarUptime * (eclipseSuFExtended.DamagePerHit + eclipseSuFExtended.DotEffect.DamagePerHit) + (1 - RotationData.SolarUptime - RotationData.LunarUptime) * (mfExtended.DamagePerHit + mfExtended.DotEffect.DamagePerHit);
            RotationData.MoonfireAvgHit = moonfireExtendedPercent * averageExtendedHit + (1 - moonfireExtendedPercent) * averageUnextendedHit;
            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;
            RotationData.InsectSwarmAvgHit = RotationData.SolarUptime * eclipseIS.DotEffect.DamagePerHit + (1 - RotationData.SolarUptime) * iSw.DotEffect.DamagePerHit;
            float insectSwarmDamage = RotationData.InsectSwarmAvgHit * RotationData.InsectSwarmCasts;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.InsectSwarmCasts;
            RotationData.DotTicks = RotationData.InsectSwarmTicks + RotationData.MoonfireTicks;
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
