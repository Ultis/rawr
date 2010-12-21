using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
    // Define delegate types for proc effect public class
    // Enable and disable the effect of the proc.  These two delegates should perform exact opposite operations.
    public delegate void Activate(Character theChar, CharacterCalculationsMoonkin calcs, ref float spellPower, ref float spellHit, ref float spellCrit, ref float spellHaste, ref float mastery);
    public delegate void Deactivate(Character theChar, CharacterCalculationsMoonkin calcs, ref float spellPower, ref float spellHit, ref float spellCrit, ref float spellHaste, ref float mastery);
    // Calculate the uptime of the effect.  This will be used to weight the proc when calculating the rotational DPS.
    public delegate float UpTime(SpellRotation rotation, CharacterCalculationsMoonkin calcs);
    // Optional calculations for complicated proc effects like Eclipse or trinkets that proc additional damage.
    // The return value of this calculation will be ADDED to the rotational DPS.
    public delegate float CalculateDPS(SpellRotation rotation, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste);
    // The return value of this calculation will be used to adjust the mana statistics of the rotation.
    public delegate float CalculateMP5(SpellRotation rotation, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste);

    // The proc effect public class itself.
    // NOTE: Adding constructor with special effect to allow efficient construction of the proc list in a loop.
    public class ProcEffect
    {
        public ProcEffect() { }
        public ProcEffect(SpecialEffect effect)
        {
            this.Effect = effect;
            // Damage procs - unified code handler
            if (effect.Stats.ShadowDamage > 0 || effect.Stats.FireDamage > 0 || effect.Stats.FrostDamage > 0 ||
                effect.Stats.NatureDamage > 0 || effect.Stats.HolyDamage > 0 || effect.Stats.ArcaneDamage > 0 ||
                effect.Stats.ValkyrDamage > 0)
            {
                CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                {
                    SpecialEffect e = Effect;
                    float schoolModifier = 1 +
                        (effect.Stats.ShadowDamage > 0 ? c.BasicStats.BonusShadowDamageMultiplier : 0) +
                        (effect.Stats.FireDamage > 0 ? c.BasicStats.BonusFireDamageMultiplier : 0) +
                        (effect.Stats.FrostDamage > 0 ? c.BasicStats.BonusFrostDamageMultiplier : 0) +
                        (effect.Stats.NatureDamage > 0 ? c.BasicStats.BonusNatureDamageMultiplier : 0) +
                        (effect.Stats.HolyDamage > 0 ? c.BasicStats.BonusHolyDamageMultiplier : 0) +
                        (effect.Stats.ArcaneDamage > 0 ? c.BasicStats.BonusArcaneDamageMultiplier : 0);
                    float specialDamageModifier = (1 + c.BasicStats.BonusSpellPowerMultiplier) * (1 + c.BasicStats.BonusDamageMultiplier) * schoolModifier;
                    float baseValue = e.Stats.ShadowDamage + e.Stats.FireDamage + e.Stats.FrostDamage + e.Stats.NatureDamage + e.Stats.HolyDamage + e.Stats.ArcaneDamage + e.Stats.ValkyrDamage;
                    float triggerInterval = 0.0f, triggerChance = 1.0f;
                    switch (e.Trigger)
                    {
                        case Trigger.DoTTick:
                            triggerInterval = r.RotationData.Duration / r.RotationData.DotTicks;
                            break;
                        case Trigger.DamageSpellHit:
                        case Trigger.SpellHit:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            triggerChance = sHi;
                            break;
                        case Trigger.SpellCast:
                        case Trigger.DamageSpellCast:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            break;
                        case Trigger.SpellCrit:
                        case Trigger.DamageSpellCrit:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            triggerChance = sc * sHi;
                            break;
                        case Trigger.DamageDone:
                        case Trigger.DamageOrHealingDone:
                            triggerInterval = r.RotationData.Duration / (r.RotationData.CastCount + r.RotationData.DotTicks);
                            break;
                        case Trigger.Use:
                            break;
                        default:
                            return 0.0f;
                    }
                    float procsPerSecond = e.GetAverageProcsPerSecond(triggerInterval, triggerChance, 3.0f, c.FightLength * 60.0f);
                    return baseValue * (e.Duration == 0 ? 1 : e.Duration) * specialDamageModifier * procsPerSecond;
                };
            }
            if (effect.Stats.Mp5 > 0)
            {
                CalculateMP5 = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                {
                    SpecialEffect e = Effect;
                    float triggerInterval = 0.0f, triggerChance = 1.0f;
                    switch (e.Trigger)
                    {
                        case Trigger.DoTTick:
                            triggerInterval = r.RotationData.Duration / r.RotationData.DotTicks;
                            break;
                        case Trigger.DamageSpellHit:
                        case Trigger.SpellHit:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            triggerChance = sHi;
                            break;
                        case Trigger.SpellCast:
                        case Trigger.DamageSpellCast:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            break;
                        case Trigger.SpellCrit:
                        case Trigger.DamageSpellCrit:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            triggerChance = sc * sHi;
                            break;
                        case Trigger.DamageDone:
                        case Trigger.DamageOrHealingDone:
                            triggerInterval = r.RotationData.Duration / (r.RotationData.CastCount + r.RotationData.DotTicks);
                            break;
                        case Trigger.Use:
                            break;
                        default:
                            return 0.0f;
                    }
                    float procsPerSecond = e.GetAverageProcsPerSecond(triggerInterval, triggerChance, 3.0f, c.FightLength * 60f);
                    return (e.Stats.Mp5 / 5.0f * e.Duration) * procsPerSecond * 5.0f;
                };
            }
            if (effect.Stats.ManaRestoreFromMaxManaPerSecond > 0)
            {
                CalculateMP5 = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                {
                    SpecialEffect e = Effect;
                    float triggerInterval = 0.0f, triggerChance = 1.0f;
                    switch (e.Trigger)
                    {
                        case Trigger.DoTTick:
                            triggerInterval = r.RotationData.Duration / r.RotationData.DotTicks;
                            break;
                        case Trigger.DamageSpellHit:
                        case Trigger.SpellHit:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            triggerChance = sHi;
                            break;
                        case Trigger.SpellCast:
                        case Trigger.DamageSpellCast:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            break;
                        case Trigger.SpellCrit:
                        case Trigger.DamageSpellCrit:
                            triggerInterval = r.RotationData.Duration / r.RotationData.CastCount;
                            triggerChance = sc * sHi;
                            break;
                        case Trigger.DamageDone:
                        case Trigger.DamageOrHealingDone:
                            triggerInterval = r.RotationData.Duration / (r.RotationData.CastCount + r.RotationData.DotTicks);
                            break;
                        case Trigger.Use:
                            break;
                        default:
                            return 0.0f;
                    }
                    float procsPerSecond = e.GetAverageProcsPerSecond(triggerInterval, triggerChance, 3.0f, c.FightLength * 60f);
                    return e.Stats.ManaRestoreFromMaxManaPerSecond * c.BasicStats.Mana * e.Duration * procsPerSecond * 5.0f;
                };
            }
            if (Effect.Stats._rawSpecialEffectDataSize == 0 && 
                (Effect.Trigger == Trigger.DamageDone ||
                Effect.Trigger == Trigger.DamageOrHealingDone ||
                Effect.Trigger == Trigger.DamageSpellCast ||
                Effect.Trigger == Trigger.DamageSpellCrit ||
                Effect.Trigger == Trigger.DamageSpellHit ||
                Effect.Trigger == Trigger.SpellCast ||
                Effect.Trigger == Trigger.SpellCrit ||
                Effect.Trigger == Trigger.SpellHit ||
                Effect.Trigger == Trigger.SpellMiss ||
                Effect.Trigger == Trigger.Use ||
                Effect.Trigger == Trigger.MoonfireCast ||
                Effect.Trigger == Trigger.InsectSwarmCast ||
                Effect.Trigger == Trigger.InsectSwarmOrMoonfireCast ||
                Effect.Trigger == Trigger.MoonfireTick ||
                Effect.Trigger == Trigger.InsectSwarmTick ||
				Effect.Trigger == Trigger.DoTTick) &&
                (Effect.Stats.HasteRating > 0 ||
                Effect.Stats.SpellHaste > 0  ||
                Effect.Stats.HighestStat > 0 ||
                Effect.Stats.Intellect > 0))
            {
                Activate = delegate(Character ch, CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa, ref float m)
                {
                    SpecialEffect e = Effect;
                    int maxStack = e.MaxStack;
                    Stats st = e.Stats;
                    float hasteRating = st.HasteRating;
                    float spellHaste = StatConversion.GetSpellHasteFromRating(hasteRating * maxStack);
                    spellHaste += st.SpellHaste;
                    float highestStat = st.HighestStat;

                    if (spellHaste > 0)
                        sHa += spellHaste;
                    if (st.Intellect > 0 || highestStat > 0)
                    {
                        StatsMoonkin s = c.BasicStats.Clone() as StatsMoonkin;
                        s.Intellect += (st.Intellect > 0 ? st.Intellect : highestStat);
                        CharacterCalculationsMoonkin cNew = CalculationsMoonkin.GetInnerCharacterCalculations(ch, s, null);
                        storedStats.SpellPower = cNew.SpellPower - c.SpellPower;
                        storedStats.SpellCrit = cNew.SpellCrit - c.SpellCrit;
                        sp += storedStats.SpellPower;
                        sc += storedStats.SpellCrit;
                    }
                };
                Deactivate = delegate(Character ch, CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa, ref float m)
                {
                    SpecialEffect e = Effect;
                    int maxStack = e.MaxStack;
                    Stats st = e.Stats;
                    float hasteRating = st.HasteRating;
                    float spellHaste = StatConversion.GetSpellHasteFromRating(hasteRating * maxStack);
                    spellHaste += st.SpellHaste;
                    float highestStat = st.HighestStat;

                    if (spellHaste > 0)
                        sHa -= spellHaste;
                    if (st.Intellect > 0 || highestStat > 0)
                    {
                        sp -= storedStats.SpellPower;
                        sc -= storedStats.SpellCrit;
                    }
                };
                UpTime = delegate(SpellRotation r, CharacterCalculationsMoonkin c)
                {
                    float upTime = 0.0f;
                    switch (Effect.Trigger)
                    {
                        case Trigger.Use:
                            upTime = Effect.GetAverageUptime(0f, 1f);
                            break;
                        case Trigger.SpellHit:
                        case Trigger.DamageSpellHit:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / r.RotationData.CastCount, r.Solver.GetSpellHit(c));
                            break;
                        case Trigger.DamageSpellCrit:
                        case Trigger.SpellCrit:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / (r.RotationData.CastCount - (r.RotationData.InsectSwarmTicks / r.Solver.InsectSwarm.DotEffect.NumberOfTicks)), c.SpellCrit);
                            break;
                        case Trigger.SpellCast:
                        case Trigger.DamageSpellCast:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / r.RotationData.CastCount, 1f);
                            break;
                        case Trigger.MoonfireCast:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / r.RotationData.MoonfireCasts, 1f);
                            break;
                        case Trigger.InsectSwarmCast:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / r.RotationData.InsectSwarmCasts, 1f);
                            break;
                        case Trigger.InsectSwarmOrMoonfireCast:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / (r.RotationData.MoonfireCasts + r.RotationData.InsectSwarmCasts), 1f);
                            break;
                        case Trigger.EclipseProc:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / 2f, 1f);
                            break;
                        case Trigger.MoonfireTick:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / r.RotationData.MoonfireTicks, 1f);
                            break;
                        case Trigger.InsectSwarmTick:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / r.RotationData.InsectSwarmTicks, 1f);
                            break;
                        case Trigger.DoTTick:
                            upTime = Effect.GetAverageUptime(r.RotationData.Duration / (r.RotationData.MoonfireTicks + r.RotationData.InsectSwarmTicks), 1f);
                            break;
                        case Trigger.DamageDone:
                        case Trigger.DamageOrHealingDone:
                            upTime = Effect.GetAverageUptime(((r.RotationData.Duration / r.RotationData.CastCount) + (r.RotationData.Duration / (r.RotationData.MoonfireTicks + r.RotationData.InsectSwarmTicks))) / 2.0f, 1f);
                            break;
                        default:
                            break;
                    }
                    return upTime;
                };
            }
        }
        private Stats storedStats = new Stats();
        public SpecialEffect Effect { get; set; }
        public Activate Activate { get; set; }
        public Deactivate Deactivate { get; set; }
        public UpTime UpTime { get; set; }
        public CalculateDPS CalculateDPS { get; set; }
        public CalculateMP5 CalculateMP5 { get; set; }
    }
    // Combination generator public class
    // Helper public class to generate all possible combinations of proc effects to mathematically account for interactions.
    public class CombinationGenerator
    {
        int[] a;
        int n;
        int r;
        long numLeft;
        long total;
        public CombinationGenerator(int n, int r)
        {
            if (r > n) throw new ArgumentException("r must be less than or equal to n");
            if (n < 1) throw new ArgumentException("n must be greater than or equal to 1");

            this.n = n;
            this.r = r;
            a = new int[r];
            long nFact = GetFactorial(n);
            long rFact = GetFactorial(r);
            long nMinusRFact = GetFactorial(n - r);
            total = nFact / (rFact * nMinusRFact);
            Reset();
        }

        public bool HasNext()
        {
            return numLeft > 0;
        }

        public int[] GetNext()
        {
            if (numLeft == total)
            {
                --numLeft;
                return a;
            }

            int i = r - 1;
            while (a[i] == n - r + i)
            {
                --i;
            }
            a[i]++;
            for (int j = i + 1; j < r; ++j)
            {
                a[j] = a[i] + j - i;
            }
            --numLeft;
            return a;
        }

        private void Reset()
        {
            for (int i = 0; i < a.Length; ++i)
            {
                a[i] = i;
            }
            numLeft = total;
        }

        private long GetFactorial(int p)
        {
            long fact = 1;
            for (int i = p; i > 1; --i)
            {
                fact *= i;
            }
            return fact;
        }
    }
}
