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
        public List<string> SpellsUsed;
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
            Spell sf = Solver.Starfire;
            Spell solarSS = Solver.Starsurge;
            Spell lunarSS = new Spell(Solver.Starsurge);
            Spell w = Solver.Wrath;
            Spell mf = Solver.Moonfire;
            Spell iSw = Solver.InsectSwarm;

            // Lunar-directed Starsurge generates lunar energy, thus changing the energy generation rate with Euphoria
            lunarSS.CriticalEnergy = 2 * talents.Euphoria;

            Spell eclipseSF = new Spell(Solver.Starfire);
            Spell eclipseW = new Spell(Solver.Wrath);
            Spell eclipseMF = new Spell(Solver.Moonfire);
            Spell eclipseIS = new Spell(Solver.InsectSwarm);
            Spell eclipseSolarSS = new Spell(solarSS);
            Spell eclipseLunarSS = new Spell(lunarSS);

            float eclipseBonus = 1 + calcs.EclipseBase + calcs.BasicStats.EclipseBonus;

            eclipseSF.AllDamageModifier *= eclipseBonus;
            eclipseW.AllDamageModifier *= eclipseBonus;
            eclipseSolarSS.AllDamageModifier *= eclipseBonus;
            eclipseLunarSS.AllDamageModifier *= eclipseBonus;
            eclipseMF.AllDamageModifier *= eclipseBonus;
            eclipseMF.DotEffect.AllDamageModifier *= eclipseBonus;
            eclipseIS.DotEffect.AllDamageModifier *= eclipseBonus;

            DoMainNuke(calcs, ref sf, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseSF, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref solarSS, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref lunarSS, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseSolarSS, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseLunarSS, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref w, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(calcs, ref eclipseW, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref mf, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseMF, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref iSw, spellPower, spellHit, spellCrit, spellHaste);
            DoDotSpell(calcs, ref eclipseIS, spellPower, spellHit, spellCrit, spellHaste);

            float barHalfSize = 100f;
            float starSurgeCD = 15.0f + solarSS.CastTime;
            float wrathEnergyRate = w.AverageEnergy / w.CastTime;
            float starfireEnergyRate = sf.AverageEnergy / sf.CastTime;
            float lunarStarsurgeEnergyRate = lunarSS.AverageEnergy / starSurgeCD;
            float solarStarsurgeEnergyRate = solarSS.AverageEnergy / starSurgeCD;

            float preLunarCasts = RotationData.LunarEclipseMode == EclipseMode.Unused ? 0 : (barHalfSize + (RotationData.SolarEclipseMode == EclipseMode.Unused ? sf.AverageEnergy / 2 : -w.AverageEnergy / 2f) - (RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse ? lunarSS.AverageEnergy : 0)) / w.AverageEnergy * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? lunarStarsurgeEnergyRate : 0) / wrathEnergyRate) + (RotationData.SolarEclipseMode == EclipseMode.Unused ? 1 : 0);
            float preLunarTime = preLunarCasts * w.CastTime;
            float preLunarDamage = preLunarCasts * w.DamagePerHit;

            float lunarCasts = RotationData.LunarEclipseMode == EclipseMode.Unused ? 0 : (barHalfSize + eclipseSF.AverageEnergy / 2f + (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse ? lunarSS.AverageEnergy : 0)) / eclipseSF.AverageEnergy * (1 + (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? lunarStarsurgeEnergyRate : 0) / starfireEnergyRate) + 1;
            float lunarTime = lunarCasts * eclipseSF.CastTime;
            float lunarDamage = lunarCasts * eclipseSF.DamagePerHit;

            float preSolarCasts = RotationData.SolarEclipseMode == EclipseMode.Unused ? 0 : (barHalfSize + (RotationData.LunarEclipseMode == EclipseMode.Unused ? w.AverageEnergy / 2f : -sf.AverageEnergy / 2f) - (RotationData.StarsurgeCastMode == StarsurgeMode.OutOfEclipse ? solarSS.AverageEnergy : 0)) / sf.AverageEnergy * (1 - (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? solarStarsurgeEnergyRate : 0) / starfireEnergyRate) - (RotationData.LunarEclipseMode == EclipseMode.Unused ? 0 : 1);
            float preSolarTime = preSolarCasts * sf.CastTime;
            float preSolarDamage = preSolarCasts * sf.DamagePerHit;

            float solarCasts = RotationData.SolarEclipseMode == EclipseMode.Unused ? 0 : (barHalfSize + eclipseW.AverageEnergy / 2f + (RotationData.StarsurgeCastMode == StarsurgeMode.InEclipse ? solarSS.AverageEnergy : 0)) / eclipseW.AverageEnergy * (1 + (RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ? solarStarsurgeEnergyRate : 0) / wrathEnergyRate);
            float solarTime = solarCasts * eclipseW.CastTime;
            float solarDamage = solarCasts * eclipseW.DamagePerHit;

            RotationData.WrathAvgCast = w.CastTime;
            RotationData.WrathAvgEnergy = w.AverageEnergy;
            RotationData.WrathCount = solarCasts + preLunarCasts;
            RotationData.WrathAvgHit = (solarDamage + preLunarDamage) / RotationData.WrathCount;

            RotationData.StarfireAvgCast = sf.CastTime;
            RotationData.StarfireAvgEnergy = sf.AverageEnergy;
            RotationData.StarfireCount = lunarCasts + preSolarCasts;
            RotationData.StarfireAvgHit = (lunarDamage + preSolarDamage) / RotationData.StarfireCount;

            float mainNukeTime = preLunarTime + preSolarTime + lunarTime + solarTime;
            float moonfireTime = 0, insectSwarmTime = 0;

            switch (RotationData.MoonfireRefreshMode)
            {
                case DotMode.Always:
                    float moonfireRatio = mf.CastTime / mf.DotEffect.Duration;
                    moonfireTime = moonfireRatio * mainNukeTime / (1 - moonfireRatio);
                    RotationData.MoonfireCasts = moonfireTime / mf.CastTime;
                    break;
                case DotMode.Once:
                    RotationData.MoonfireCasts = 1;
                    mainNukeTime += mf.CastTime;
                    moonfireTime = mf.CastTime;
                    break;
                case DotMode.Twice:
                    RotationData.MoonfireCasts = 2;
                    mainNukeTime += 2 * mf.CastTime;
                    moonfireTime = 2 * mf.CastTime;
                    break;
                case DotMode.Unused:
                    break;
            }
            RotationData.MoonfireAvgHit = (mf.DamagePerHit + mf.DotEffect.DamagePerHit) * RotationData.MoonfireCasts;
            RotationData.MoonfireTicks = RotationData.MoonfireCasts * mf.DotEffect.NumberOfTicks;
            RotationData.MoonfireDuration = mf.DotEffect.Duration;
            RotationData.AverageInstantCast = mf.CastTime;
            float moonfireDamage = RotationData.MoonfireAvgHit * RotationData.MoonfireCasts;

            switch (RotationData.InsectSwarmRefreshMode)
            {
                case DotMode.Always:
                    float insectSwarmRatio = iSw.CastTime / iSw.DotEffect.Duration;
                    insectSwarmTime = insectSwarmRatio * mainNukeTime / (1 - insectSwarmRatio);
                    RotationData.InsectSwarmCasts = insectSwarmTime / iSw.CastTime;
                    break;
                case DotMode.Once:
                    RotationData.InsectSwarmCasts = 1;
                    mainNukeTime += iSw.CastTime;
                    insectSwarmTime = iSw.CastTime;
                    break;
                case DotMode.Twice:
                    RotationData.InsectSwarmCasts = 2;
                    mainNukeTime += 2 * iSw.CastTime;
                    insectSwarmTime = 2 * iSw.CastTime;
                    break;
                case DotMode.Unused:
                    break;
            }
            RotationData.InsectSwarmAvgHit = iSw.DotEffect.DamagePerHit * RotationData.InsectSwarmCasts;
            RotationData.InsectSwarmTicks = RotationData.InsectSwarmCasts * iSw.DotEffect.NumberOfTicks;
            RotationData.InsectSwarmDuration = iSw.DotEffect.Duration;
            float insectSwarmDamage = RotationData.InsectSwarmAvgHit * RotationData.InsectSwarmCasts;

            float starSurgeRatio = solarSS.CastTime / starSurgeCD;
            float starSurgeTime = RotationData.StarsurgeCastMode == StarsurgeMode.OnCooldown ?
                starSurgeRatio * mainNukeTime / (1 - starSurgeRatio) :
                solarSS.CastTime * (RotationData.StarsurgeCastMode == StarsurgeMode.Unused ? 0 : 2);
            RotationData.StarSurgeCount = starSurgeTime / solarSS.CastTime;
            RotationData.StarSurgeAvgCast = solarSS.CastTime;
            RotationData.StarSurgeAvgEnergy = (lunarSS.AverageEnergy + solarSS.AverageEnergy) / 2f;
            RotationData.StarSurgeAvgHit = (eclipseSolarSS.DamagePerHit + eclipseLunarSS.DamagePerHit + solarSS.DamagePerHit + lunarSS.DamagePerHit) / 4f;
            float starSurgeDamage = RotationData.StarSurgeAvgHit * RotationData.StarSurgeCount;

            RotationData.CastCount = RotationData.WrathCount + RotationData.StarfireCount + RotationData.StarSurgeCount +
                RotationData.MoonfireCasts + RotationData.InsectSwarmCasts;
            RotationData.DotTicks = RotationData.InsectSwarmTicks + RotationData.MoonfireTicks;
            RotationData.Duration = mainNukeTime + moonfireTime + insectSwarmTime + starSurgeTime;
            RotationData.ManaUsed = RotationData.WrathCount * w.BaseManaCost +
                RotationData.StarfireCount * sf.BaseManaCost +
                RotationData.StarSurgeCount * solarSS.BaseManaCost +
                RotationData.MoonfireCasts * mf.BaseManaCost +
                RotationData.InsectSwarmCasts * iSw.BaseManaCost;

            RotationData.ManaGained = 2 * 0.06f * talents.Euphoria * calcs.BasicStats.Mana;

            return preSolarDamage + solarDamage + preLunarDamage + lunarDamage + moonfireDamage + insectSwarmDamage + starSurgeDamage;
        }
    }
}
