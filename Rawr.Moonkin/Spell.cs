using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
    public enum SpellSchool
    {
        Arcane,
        Nature
    }
    public class Spell
    {
        public Spell() { AllDamageModifier = 1.0f; CriticalDamageModifier = 1.0f; }
        public Spell(Spell copy)
        {
            this.Name = copy.Name;
            this.School = copy.School;
            this.BaseCastTime = copy.BaseCastTime;
            this.BaseDamage = copy.BaseDamage;
            this.AllDamageModifier = copy.AllDamageModifier;
            this.BaseManaCost = copy.BaseManaCost;
            this.CriticalChanceModifier = copy.CriticalChanceModifier;
            this.CriticalDamageModifier = copy.CriticalDamageModifier;
            this.DotEffect = copy.DotEffect == null ? null : new DotEffect(copy.DotEffect);
            this.IdolExtraSpellPower = copy.IdolExtraSpellPower;
            this.SpellDamageModifier = copy.SpellDamageModifier;
        }
        public string Name { get; set; }
        public float BaseDamage { get; set; }
        public SpellSchool School { get; set; }
        public float SpellDamageModifier { get; set; }
        public float AllDamageModifier { get; set; }
        public float IdolExtraSpellPower { get; set; }
        public float CriticalDamageModifier { get; set; }
        public float CriticalChanceModifier { get; set; }
        public float BaseCastTime { get; set; }
        public float BaseManaCost { get; set; }
        public DotEffect DotEffect { get; set; }
        // Section for variables which get filled in during rotation calcs
        public float DamagePerHit { get; set; }
        public float CastTime { get; set; }
        public float NGCastTime { get; set; }
        public float ManaCost { get; set; }
    }
    public class DotEffect
    {
        public DotEffect() { AllDamageModifier = 1.0f;  }
        public DotEffect(DotEffect copy)
        {
            this.AllDamageModifier = copy.AllDamageModifier;
            this.BaseDamage = copy.BaseDamage;
            this.Duration = copy.Duration;
            this.TickDamage = copy.TickDamage;
            this.TickLength = copy.TickLength;

            this.SpellDamageModifierPerTick = copy.SpellDamageModifierPerTick;
        }
        public float Duration { get; set; }
        public float TickLength { get; set; }
        public float TickDamage { get; set; }
        public bool CanCrit { get; set; }
        public float SpellDamageModifier
        {
            get
            {
                return SpellDamageModifierPerTick * NumberOfTicks;
            }
            set
            {
                SpellDamageModifierPerTick += value / NumberOfTicks;
            }
        }
        public float AllDamageModifier { get; set; }
        public float NumberOfTicks
        {
            get
            {
                return Duration / TickLength;
            }
        }
        public float SpellDamageModifierPerTick { get; set; }
        public float BaseDamage
        {
            get
            {
                return NumberOfTicks * TickDamage;
            }
            set
            {
                TickDamage += value / NumberOfTicks;
            }
        }
        // Section for variables which get filled in during rotation calcs
        public float DamagePerHit { get; set; }
    }
    public class RotationData
    {
        public float BurstDPS = 0.0f;
        public float DPS = 0.0f;
        public float DPM = 0.0f;
        public float ManaUsed = 0.0f;
        public float ManaGained = 0.0f;
        public TimeSpan TimeToOOM = new TimeSpan(0, 0, 0);
    }

    // Define delegate types for proc effect public class
    // Enable and disable the effect of the proc.  These two delegates should perform exact opposite operations.
    public delegate void Activate(Character theChar, CharacterCalculationsMoonkin calcs, ref float spellPower, ref float spellHit, ref float spellCrit, ref float spellHaste);
    public delegate void Deactivate(Character theChar, CharacterCalculationsMoonkin calcs, ref float spellPower, ref float spellHit, ref float spellCrit, ref float spellHaste);
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
            // Shadow damage procs - most widely varied at the current moment
            if (effect.Stats.ShadowDamage > 0)
            {
                CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                {
                    SpecialEffect e = Effect;
                    float specialDamageModifier = (1 + c.BasicStats.BonusSpellPowerMultiplier) * (1 + c.BasicStats.BonusShadowDamageMultiplier) * (1 + c.BasicStats.BonusDamageMultiplier);
                    float triggerInterval = 0.0f;
                    switch (e.Trigger)
                    {
                        case Trigger.DoTTick:       // Extract
                            triggerInterval = r.Duration / r.DotTicks;
                            break;
                        case Trigger.SpellHit:      // Pendulum
                            triggerInterval = r.Duration / r.CastCount;
                            break;
                        case Trigger.DamageDone:    // DMC: Death
                            triggerInterval = r.Duration / (r.CastCount + r.DotTicks);
                            break;
                        default:
                            return 0.0f;
                    }
                    float procsPerSecond = e.GetAverageProcsPerSecond(triggerInterval, 1.0f, 3.0f, c.FightLength * 60.0f);
                    return e.Stats.ShadowDamage * specialDamageModifier * procsPerSecond;
                };
            }
			// Lightning Capacitor, Thunder Capacitor, Reign of the Unliving/Undead
			else if (effect.Stats.NatureDamage > 0 || effect.Stats.FireDamage > 0)
			{
				if (effect.Stats.NatureDamage > 0)
				{
					CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
					{
						float specialDamageModifier = (1 + c.BasicStats.BonusSpellPowerMultiplier) * (1 + c.BasicStats.BonusNatureDamageMultiplier) * (1 + c.BasicStats.BonusDamageMultiplier);
                        float procsPerSecond = Effect.GetAverageProcsPerSecond(r.Duration / (r.CastCount * sc), 1.0f, 3.0f, c.FightLength * 60.0f);
                        return Effect.Stats.NatureDamage * specialDamageModifier * procsPerSecond;
					};
				}
				else
				{
					CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
					{
                        float specialDamageModifier = (1 + c.BasicStats.BonusSpellPowerMultiplier) * (1 + c.BasicStats.BonusFireDamageMultiplier) * (1 + c.BasicStats.BonusDamageMultiplier);
                        float procsPerSecond = Effect.GetAverageProcsPerSecond(r.Duration / (r.CastCount * sc), 1.0f, 3.0f, c.FightLength * 60.0f);
                        return Effect.Stats.FireDamage * specialDamageModifier * procsPerSecond;
					};
				}
			}
            else if (effect.Stats.Mp5 > 0)
            {
                CalculateMP5 = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                {
                    SpecialEffect e = Effect;
                    float procsPerSecond = e.GetAverageProcsPerSecond(r.Duration / r.CastCount, 1.0f, 3.0f, c.FightLength * 60f);
                    return (e.Stats.Mp5 / 5.0f * e.Duration) * procsPerSecond * 5.0f;
                };
            }
            // Moonkin 4T8 set bonus (15% chance on IS tick to proc an instant-cast Starfire)
            else if (effect.Stats.StarfireProc == 1)
            {
                CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                {
                    if (r.InsectSwarmTicks == 0) return 0.0f;
                    Spell newSF = new Spell()
                    {
                        AllDamageModifier = r.Solver.Starfire.AllDamageModifier,
                        BaseCastTime = 1.5f,
                        BaseDamage = r.Solver.Starfire.BaseDamage,
                        BaseManaCost = r.Solver.Starfire.BaseManaCost,
                        CriticalChanceModifier = r.Solver.Starfire.CriticalChanceModifier,
                        CriticalDamageModifier = r.Solver.Starfire.CriticalDamageModifier,
                        DotEffect = null,
                        IdolExtraSpellPower = r.Solver.Starfire.IdolExtraSpellPower,
                        Name = r.Solver.Starfire.Name,
                        School = r.Solver.Starfire.School,
                        SpellDamageModifier = r.Solver.Starfire.SpellDamageModifier
                    };
                    r.DoSpecialStarfire(c, ref newSF, sp, sHi, sc, sHa);
                    float timeBetweenProcs = r.Solver.InsectSwarm.DotEffect.TickLength / Effect.Chance;
                    float replaceWrathWithSFDPS = (newSF.DamagePerHit / newSF.CastTime) - (r.Solver.Wrath.DamagePerHit / r.Solver.Wrath.CastTime);
                    float replaceSFWithSFDPS = (newSF.DamagePerHit / newSF.CastTime) - (r.Solver.Starfire.DamagePerHit / r.Solver.Starfire.CastTime);
                    return (replaceWrathWithSFDPS * (r.WrathCount / (r.WrathCount + r.StarfireCount)) +
                        replaceSFWithSFDPS * (r.StarfireCount / (r.WrathCount + r.StarfireCount)))
                        / timeBetweenProcs;
                };
            }
            else if (Effect.Trigger == Trigger.DamageDone ||
                Effect.Trigger == Trigger.DamageSpellCast ||
                Effect.Trigger == Trigger.DamageSpellCrit ||
                Effect.Trigger == Trigger.DamageSpellHit ||
                Effect.Trigger == Trigger.SpellCast ||
                Effect.Trigger == Trigger.SpellCrit ||
                Effect.Trigger == Trigger.SpellHit ||
                Effect.Trigger == Trigger.SpellMiss ||
                Effect.Trigger == Trigger.Use ||
                Effect.Trigger == Trigger.MoonfireCast ||
                Effect.Trigger == Trigger.MoonfireTick ||
				Effect.Trigger == Trigger.InsectSwarmTick)
            {
                Activate = delegate(Character ch, CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                {
                    SpecialEffect e = Effect;
                    int maxStack = e.MaxStack;
                    Stats st = e.Stats;
                    float spellPower = st.SpellPower;
                    float critRating = st.CritRating;
                    float spellCrit = StatConversion.GetSpellCritFromRating(critRating * maxStack);
                    float hasteRating = st.HasteRating;
                    float spellHaste = StatConversion.GetSpellHasteFromRating(hasteRating * maxStack);
                    spellHaste += st.SpellHaste;
                    float spirit = st.Spirit;
                    float highestStat = st.HighestStat;
                    float arcaneModifier = st.BonusArcaneDamageMultiplier;
                    float natureModifier = st.BonusNatureDamageMultiplier;

                    if (spellPower > 0)
                        sp += spellPower * maxStack;
                    if (critRating > 0)
                        sc += spellCrit;
                    if (spellHaste > 0)
                        sHa += spellHaste;
                    if (arcaneModifier > 0)
                        c.BasicStats.BonusArcaneDamageMultiplier += arcaneModifier;
                    if (natureModifier > 0)
                        c.BasicStats.BonusNatureDamageMultiplier += natureModifier;
                    if (spirit > 0)
                    {
                        Stats s = c.BasicStats.Clone();
                        s.Spirit += spirit;
                        CharacterCalculationsMoonkin cNew = CalculationsMoonkin.GetInnerCharacterCalculations(ch, s, null);
                        storedStats.SpellPower = cNew.SpellPower - c.SpellPower;
                        sp += storedStats.SpellPower;
                    }
                    if (highestStat > 0)
                    {
                        if (c.BasicStats.Spirit > c.BasicStats.Intellect)
                        {
                            Stats s = c.BasicStats.Clone();
                            s.Spirit += highestStat;
                            CharacterCalculationsMoonkin cNew = CalculationsMoonkin.GetInnerCharacterCalculations(ch, s, null);
                            storedStats.SpellPower = cNew.SpellPower - c.SpellPower;
                            sp += storedStats.SpellPower;
                        }
                        else
                        {
                            Stats s = c.BasicStats.Clone();
                            s.Intellect += highestStat;
                            CharacterCalculationsMoonkin cNew = CalculationsMoonkin.GetInnerCharacterCalculations(ch, s, null);
                            storedStats.SpellPower = cNew.SpellPower - c.SpellPower;
                            storedStats.SpellCrit = cNew.SpellCrit - c.SpellCrit;
                            sp += storedStats.SpellPower;
                            sc += storedStats.SpellCrit;
                        }
                    }
                };
                Deactivate = delegate(Character ch, CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                {
                    SpecialEffect e = Effect;
                    int maxStack = e.MaxStack;
                    Stats st = e.Stats;
                    float spellPower = st.SpellPower;
                    float critRating = st.CritRating;
                    float spellCrit = StatConversion.GetSpellCritFromRating(critRating * maxStack);
                    float hasteRating = st.HasteRating;
                    float spellHaste = StatConversion.GetSpellHasteFromRating(hasteRating * maxStack);
                    spellHaste += st.SpellHaste;
                    float spirit = st.Spirit;
                    float highestStat = st.HighestStat;
                    float arcaneModifier = st.BonusArcaneDamageMultiplier;
                    float natureModifier = st.BonusNatureDamageMultiplier;

                    if (arcaneModifier > 0)
                        c.BasicStats.BonusArcaneDamageMultiplier -= arcaneModifier;
                    if (natureModifier > 0)
                        c.BasicStats.BonusNatureDamageMultiplier -= natureModifier;
                    if (spellPower > 0)
                        sp -= spellPower * maxStack;
                    if (critRating > 0)
                        sc -= spellCrit;
                    if (spellHaste > 0)
                        sHa -= spellHaste;
                    if (spirit > 0)
                    {
                        sp -= storedStats.SpellPower;
                    }
                    if (highestStat > 0)
                    {
                        sp -= storedStats.SpellPower;
                        if (c.BasicStats.Intellect >= c.BasicStats.Spirit)
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
                            upTime = Effect.GetAverageUptime(r.Duration / r.CastCount, r.Solver.GetSpellHit(c));
                            break;
                        case Trigger.DamageSpellCrit:
                        case Trigger.SpellCrit:
                            upTime = Effect.GetAverageUptime(r.Duration / (r.CastCount - (r.InsectSwarmTicks / r.Solver.InsectSwarm.DotEffect.NumberOfTicks)), c.SpellCrit);
                            break;
                        case Trigger.SpellCast:
                        case Trigger.DamageSpellCast:
                            upTime = Effect.GetAverageUptime(r.Duration / r.CastCount, 1f);
                            break;
                        case Trigger.MoonfireCast:
                            upTime = Effect.GetAverageUptime(r.Duration / r.MoonfireCasts, r.Solver.GetSpellHit(c));
                            break;
                        case Trigger.MoonfireTick:
                            upTime = Effect.GetAverageUptime(r.Duration / r.MoonfireTicks, 1f);
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

    // Our old friend the spell rotation.
    public class SpellRotation
    {
        public MoonkinSolver Solver { get; set; }
        /*private Spell LocateSpell(Spell[] SpellData, string name)
        {
            foreach (Spell sp in SpellData)
            {
                if (sp.Name == name)
                    return sp;
            }
            return null;
            //return Array.Find<Spell>(SpellData, delegate(Spell sp) { return sp.Name == name; });
        }*/
        public List<string> SpellsUsed;
        public RotationData RotationData = new RotationData();
        public string Name { get; set; }
        public float Duration { get; set; }
        public float ManaUsed { get; set; }
        public float ManaGained { get; set; }
        public float CastCount { get; set; }
        public float DotTicks { get; set; }
        public float WrathCount { get; set; }
        public float StarfireCount { get; set; }
        public float InsectSwarmTicks { get; set; }
        public float MoonfireCasts { get; set; }
        public float MoonfireTicks { get; set; }

        // Calculate damage and casting time for a single, direct-damage spell.
        private void DoMainNuke(DruidTalents talents, CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;
            int naturesGrace = talents.NaturesGrace;
            int starlightWrath = talents.StarlightWrath;

            float gcd = 1.5f / (1.0f + spellHaste);
            float instantCast = (float)Math.Max(gcd, 1.0f) + latency;
            float ngGCD = (float)Math.Max(gcd / 1.2f, 1.0f);
            float instantCastNG = ngGCD + latency;

            mainNuke.CastTime = mainNuke.BaseCastTime - 0.1f * starlightWrath;
            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            float normalCastTime = (float)Math.Max(mainNuke.CastTime / (1 + spellHaste), instantCast);
            mainNuke.NGCastTime = (float)Math.Max(mainNuke.CastTime / (1 + spellHaste) / (1 + 0.2f * naturesGrace / 3.0f), instantCastNG);
            float NGProcChance = totalCritChance * naturesGrace / 3.0f;
            float NGUptime = 1.0f - (float)Math.Pow(1.0f - NGProcChance, Math.Floor(3.0f / normalCastTime) + 1.0f);
            mainNuke.CastTime = (1 - NGUptime) * normalCastTime + NGUptime * mainNuke.NGCastTime;
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * (spellPower + mainNuke.IdolExtraSpellPower)) * mainNuke.AllDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier * (1 + calcs.BasicStats.MoonkinT10CritDot);
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
        }
        // Modified version of above function specifically for use in calculating Moonkin 4T8 proc.
        public void DoSpecialStarfire(CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;

            mainNuke.CastTime = mainNuke.BaseCastTime;
            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            float normalCastTime = (float)Math.Max(1.0f, mainNuke.CastTime / (1 + spellHaste)) + latency;
            mainNuke.NGCastTime = (float)Math.Max(1.0f, mainNuke.CastTime / (1 + spellHaste) / (1 + 0.2f * Solver.NaturesGrace / 3.0f)) + latency;
            float NGProcChance = totalCritChance * Solver.NaturesGrace / 3.0f;
            float NGUptime = 1.0f - (float)Math.Pow(1.0f - NGProcChance, Math.Floor(3.0f / normalCastTime) + 1.0f);
            mainNuke.CastTime = (1 - NGUptime) * normalCastTime + NGUptime * mainNuke.NGCastTime;
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * (spellPower + mainNuke.IdolExtraSpellPower)) * mainNuke.AllDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier * (1 + calcs.BasicStats.MoonkinT10CritDot);
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
        }

        // Calculate damage and casting time for a damage-over-time effect.
        private void DoDotSpell(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            if (dotSpell.Name == "MF")
                DoMoonfire(calcs, ref dotSpell, spellPower, spellHit, spellCrit, spellHaste);
            else
                DoInsectSwarm(calcs, ref dotSpell, spellPower, spellHit, spellCrit, spellHaste);
        }

        // Calculate damage and casting time for the Moonfire effect.
        private void DoMoonfire(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;
            dotSpell.CastTime = Math.Max(dotSpell.BaseCastTime / (1 + spellHaste), 1.0f + latency) + latency;
            float mfDirectDamage = (dotSpell.BaseDamage + dotSpell.SpellDamageModifier * (spellPower + dotSpell.IdolExtraSpellPower)) * dotSpell.AllDamageModifier;
            float mfCritDamage = mfDirectDamage * dotSpell.CriticalDamageModifier;
            float totalCritChance = spellCrit + dotSpell.CriticalChanceModifier;
            dotSpell.DamagePerHit = (totalCritChance * mfCritDamage + (1 - totalCritChance) * mfDirectDamage) * spellHit;
            float normalDamagePerTick = dotSpell.DotEffect.TickDamage + dotSpell.DotEffect.SpellDamageModifierPerTick * spellPower;
            float damagePerTick = 0.0f;
            if (dotSpell.DotEffect.CanCrit)
            {
                float critDamagePerTick = normalDamagePerTick * dotSpell.CriticalDamageModifier;
                damagePerTick = (totalCritChance * critDamagePerTick + (1 - totalCritChance) * normalDamagePerTick) * dotSpell.DotEffect.AllDamageModifier;
            }
            else
            {
                damagePerTick = normalDamagePerTick * dotSpell.DotEffect.AllDamageModifier;
            }
            dotSpell.DotEffect.DamagePerHit = dotSpell.DotEffect.NumberOfTicks * damagePerTick * spellHit;
        }

        // Calculate damage and casting time for the Insect Swarm effect.
        private void DoInsectSwarm(CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;
            dotSpell.CastTime = Math.Max(dotSpell.BaseCastTime / (1 + spellHaste), 1.0f + latency) + latency;
            float damagePerTick = (dotSpell.DotEffect.TickDamage + dotSpell.DotEffect.SpellDamageModifierPerTick * (spellPower + dotSpell.IdolExtraSpellPower)) * dotSpell.DotEffect.AllDamageModifier;
            dotSpell.DotEffect.DamagePerHit = dotSpell.DotEffect.NumberOfTicks * damagePerTick * spellHit;
        }

        // Perform damage and mana calculations for all spells in the given rotation.  Returns damage done over the total duration.
        public float DamageDone(DruidTalents talents, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            if (talents.Eclipse > 0)
            {
                return DoEclipseCalcs(talents, calcs, Solver, spellPower, spellHit, spellCrit, spellHaste);
            }
            float latency = calcs.Latency;

            float moonkinFormProc = (talents.MoonkinForm == 1) ? 0.02f * calcs.BasicStats.Mana : 0.0f;
            bool starfireGlyph = talents.GlyphOfStarfire;
            int impInsectSwarm = talents.ImprovedInsectSwarm;

            switch (SpellsUsed.Count)
            {
                // Nuke only
                case 1:
                    Spell mainNuke = Solver.FindSpell(SpellsUsed[0]);
                    DoMainNuke(talents, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

                    float omenProcChance = talents.OmenOfClarity == 1 ? 0.06f : 0;
                    mainNuke.ManaCost = mainNuke.BaseManaCost - mainNuke.BaseCastTime / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana * spellHit - (spellCrit + mainNuke.CriticalChanceModifier) * moonkinFormProc - mainNuke.BaseManaCost * omenProcChance * spellHit;
                    Duration = mainNuke.CastTime;
                    RotationData.ManaUsed = ManaUsed = mainNuke.ManaCost;
                    RotationData.ManaGained = ManaGained = mainNuke.BaseManaCost - mainNuke.ManaCost;
                    RotationData.DPM = mainNuke.DamagePerHit / mainNuke.ManaCost;
                    CastCount = 1.0f;
                    DotTicks = 0.0f;
                    WrathCount = mainNuke.Name == "W" ? 1.0f : 0.0f;
                    StarfireCount = mainNuke.Name == "SF" ? 1.0f : 0.0f;

                    return mainNuke.DamagePerHit;
                // Nuke + 1 DotEffect
                case 2:
                    // Find the spells
                    Spell DotEffectSpell = Solver.FindSpell(SpellsUsed[0]);
                    mainNuke = Solver.FindSpell(SpellsUsed[1]);
                    // Do Starfire glyph calculations, if applicable; then do DoT spell calculations
                    if (starfireGlyph && mainNuke.Name == "SF" && DotEffectSpell.Name == "MF") DotEffectSpell.DotEffect.Duration += 9.0f;
                    DoDotSpell(calcs, ref DotEffectSpell, spellPower, spellHit, spellCrit, spellHaste);

                    // Do iIS calculations, if applicable
                    if (impInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF" && DotEffectSpell.Name == "MF")
                            mainNuke.CriticalChanceModifier += 0.01f * impInsectSwarm;
                        else if (mainNuke.Name == "W" && DotEffectSpell.Name == "IS")
                            mainNuke.AllDamageModifier *= 1 + 0.01f * impInsectSwarm;
                    }
                    // Calculate main nuke damage
                    DoMainNuke(talents, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

                    // Set rotation duration
                    Duration = DotEffectSpell.DotEffect.Duration;

                    // Calculate mana usage and damage done for this rotation
                    float timeSpentCastingNuke = Duration - DotEffectSpell.CastTime;
                    float nukeDamageDone = mainNuke.DamagePerHit / mainNuke.CastTime * timeSpentCastingNuke;
                    
                    float numNukeCasts = timeSpentCastingNuke / mainNuke.CastTime;
                    float nukeManaSpent = mainNuke.BaseManaCost * numNukeCasts;
                    float totalManaSpent = nukeManaSpent + DotEffectSpell.BaseManaCost;
                    CastCount = numNukeCasts + 1.0f;
                    WrathCount = mainNuke.Name == "W" ? numNukeCasts : 0.0f;
                    StarfireCount = mainNuke.Name == "SF" ? numNukeCasts : 0.0f;
                    DotTicks = DotEffectSpell.DotEffect.NumberOfTicks;
                    if (DotEffectSpell.Name == "IS")
                        InsectSwarmTicks = DotTicks;
                    else if (DotEffectSpell.Name == "MF")
                    {
                        MoonfireTicks = DotTicks;
                        MoonfireCasts = 1f;
                    }

                    float manaFromJoW = (mainNuke.ManaCost - mainNuke.BaseCastTime / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana) * numNukeCasts;
                    manaFromJoW += DotEffectSpell.ManaCost - 1.5f / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana;
                    float manaFromOoC = ((0.06f) * mainNuke.BaseManaCost
                        + (numNukeCasts - 1) * (0.06f) * mainNuke.BaseManaCost
                        + (0.06f) * DotEffectSpell.BaseManaCost) * spellHit;
                    float manaFromMoonkin = moonkinFormProc * spellHit * ((spellCrit + mainNuke.CriticalChanceModifier) * numNukeCasts + DotEffectSpell.CriticalChanceModifier);

                    float actualManaSpent = totalManaSpent - manaFromJoW - manaFromMoonkin - (talents.OmenOfClarity == 1 ? manaFromOoC : 0.0f);

                    RotationData.ManaUsed = ManaUsed = actualManaSpent;
                    RotationData.ManaGained = ManaGained = totalManaSpent - actualManaSpent;
                    RotationData.DPM = (nukeDamageDone + DotEffectSpell.DamagePerHit + DotEffectSpell.DotEffect.DamagePerHit) / totalManaSpent;
                    
                    // Undo iIS, if applicable
                    if (impInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF" && DotEffectSpell.Name == "MF")
                            mainNuke.CriticalChanceModifier -= 0.01f * impInsectSwarm;
                        else if (mainNuke.Name == "W" && DotEffectSpell.Name == "IS")
                            mainNuke.AllDamageModifier /= 1 + 0.01f * impInsectSwarm;
                    }
                    // Undo SF glyph, if applicable
                    if (starfireGlyph && mainNuke.Name == "SF" && DotEffectSpell.Name == "MF") DotEffectSpell.DotEffect.Duration -= 9.0f;
                    // Return the damage done per rotation
                    return nukeDamageDone + DotEffectSpell.DamagePerHit + DotEffectSpell.DotEffect.DamagePerHit;
                // Nuke + both DotEffects
                case 3:
                    // Find the spells
                    Spell moonFire = Solver.FindSpell(SpellsUsed[0]);
                    Spell insectSwarm = Solver.FindSpell(SpellsUsed[1]);
                    mainNuke = Solver.FindSpell(SpellsUsed[2]);
                    // Do Starfire glyph calculations, if applicable; then do DoT spell calculations
                    if (starfireGlyph && mainNuke.Name == "SF") moonFire.DotEffect.Duration += 9.0f;
                    DoDotSpell(calcs, ref moonFire, spellPower, spellHit, spellCrit, spellHaste);
                    DoDotSpell(calcs, ref insectSwarm, spellPower, spellHit, spellCrit, spellHaste);

                    // Do iIS calculations, if applicable
                    if (impInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF")
                            mainNuke.CriticalChanceModifier += 0.01f * impInsectSwarm;
                        else if (mainNuke.Name == "W")
                            mainNuke.AllDamageModifier *= 1 + 0.01f * impInsectSwarm;
                    }
                    // Calculate main nuke damage
                    DoMainNuke(talents, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

                    // Set rotation duration
                    Duration = moonFire.DotEffect.Duration;

                    // Calculate mana usage and damage done for this rotation
                    float timeSpentCastingIS = insectSwarm.CastTime * moonFire.DotEffect.Duration / insectSwarm.DotEffect.Duration;
                    float insectSwarmDamage = insectSwarm.DotEffect.DamagePerHit * moonFire.DotEffect.Duration / insectSwarm.DotEffect.Duration;
                    timeSpentCastingNuke = Duration - timeSpentCastingIS - moonFire.CastTime;
                    nukeDamageDone = mainNuke.DamagePerHit / mainNuke.CastTime * timeSpentCastingNuke;

                    numNukeCasts = timeSpentCastingNuke / mainNuke.CastTime;
                    float numISCasts = timeSpentCastingIS / insectSwarm.CastTime;
                    nukeManaSpent = mainNuke.BaseManaCost * numNukeCasts;
                    totalManaSpent = nukeManaSpent + moonFire.BaseManaCost + numISCasts * insectSwarm.BaseManaCost;
                    CastCount = numNukeCasts + numISCasts + 1.0f;
                    WrathCount = mainNuke.Name == "W" ? numNukeCasts : 0.0f;
                    StarfireCount = mainNuke.Name == "SF" ? numNukeCasts : 0.0f;
                    DotTicks = moonFire.DotEffect.NumberOfTicks + numISCasts * insectSwarm.DotEffect.NumberOfTicks;
                    InsectSwarmTicks = numISCasts * insectSwarm.DotEffect.NumberOfTicks;
                    MoonfireTicks = moonFire.DotEffect.NumberOfTicks;
                    MoonfireCasts = 1.0f;

                    manaFromJoW = numNukeCasts * (mainNuke.ManaCost - mainNuke.BaseCastTime / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana) +
                        (moonFire.ManaCost - 1.5f / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana) +
                        numISCasts * (insectSwarm.ManaCost - 1.5f / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana);
                    manaFromOoC = ((0.06f) * mainNuke.BaseManaCost
                        + (numNukeCasts - 1 - numISCasts) * (0.06f) * mainNuke.BaseManaCost
                        + (0.06f) * moonFire.BaseManaCost
                        + (0.06f) * numISCasts * insectSwarm.BaseManaCost
                        + (0.06f) * numISCasts * mainNuke.BaseManaCost) * spellHit;
                    manaFromMoonkin = moonkinFormProc * spellHit * ((spellCrit + mainNuke.CriticalChanceModifier) * numNukeCasts + moonFire.CriticalChanceModifier);

                    actualManaSpent = totalManaSpent - manaFromJoW - manaFromMoonkin - (talents.OmenOfClarity == 1 ? manaFromOoC : 0.0f);

                    RotationData.ManaUsed = ManaUsed = actualManaSpent;
                    RotationData.ManaGained = ManaGained = totalManaSpent - actualManaSpent;
                    RotationData.DPM = (nukeDamageDone + moonFire.DamagePerHit + moonFire.DotEffect.DamagePerHit + insectSwarm.DotEffect.DamagePerHit) / totalManaSpent;

                    // Undo iIS, if applicable
                    if (impInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF")
                            mainNuke.CriticalChanceModifier -= 0.01f * impInsectSwarm;
                        else if (mainNuke.Name == "W")
                            mainNuke.AllDamageModifier /= 1 + 0.01f * impInsectSwarm;
                    }
                    // Undo SF glyph
                    if (starfireGlyph && mainNuke.Name == "SF") moonFire.DotEffect.Duration -= 9.0f;
                    // Return the damage done per rotation
                    return nukeDamageDone + moonFire.DamagePerHit + moonFire.DotEffect.DamagePerHit + insectSwarmDamage;
                default:
                    throw new Exception("Invalid rotation specified in rotation solver.");
            }
        }

        private float DoEclipseCalcs(DruidTalents talents, CharacterCalculationsMoonkin calcs, MoonkinSolver solver, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;

            float omenOfClarityProcChance = talents.OmenOfClarity * 0.06f;
            float moonkinFormProc = (talents.MoonkinForm == 1) ? 0.02f * calcs.BasicStats.Mana : 0.0f;
            bool starfireGlyph = talents.GlyphOfStarfire;
            int impInsectSwarm = talents.ImprovedInsectSwarm;

            float moonfireCasts = SpellsUsed.Contains("MF") ? 2.0f : 0.0f;
            float insectSwarmCasts = SpellsUsed.Contains("IS") ? 2.0f : 0.0f;

            Spell moonfire = moonfireCasts > 0 ? solver.FindSpell("MF") : null;
            Spell insectSwarm = insectSwarmCasts > 0 ? solver.FindSpell("IS") : null;

            // Do SF glyph
            if (starfireGlyph && moonfire != null) moonfire.DotEffect.Duration += 9.0f;

            float eclipseMultiplier = 0.3f + calcs.BasicStats.EclipseBonus;

            float eclipseDuration = 15.0f;
            //float eclipseCooldown = 30.0f;

            Spell preLunarCast = solver.FindSpell("W");

            // Do improved Insect Swarm
            if (insectSwarm != null)
                preLunarCast.AllDamageModifier *= 1 + 0.01f * impInsectSwarm;

            Spell solarEclipseCast = new Spell(solver.FindSpell("W"));

            // Eclipse bonus and improved Insect Swarm
			// NOTE: Eclipse bonus additive with Moonfury and 4T9; multiplicative with everything else
            solarEclipseCast.AllDamageModifier = 1 + (float)Math.Floor(talents.Moonfury * 10 / 3.0f) / 100.0f + calcs.BasicStats.BonusMoonkinNukeDamage + eclipseMultiplier;
            solarEclipseCast.AllDamageModifier *= ((1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier));
            if (insectSwarm != null)
                solarEclipseCast.AllDamageModifier *= 1 + 0.01f * impInsectSwarm;
            solarEclipseCast.AllDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);

            Spell preSolarCast = solver.FindSpell("SF");
            if (moonfire != null)
                preSolarCast.CriticalChanceModifier += 0.01f * impInsectSwarm;

            Spell lunarEclipseCast = new Spell(solver.FindSpell("SF"));
            lunarEclipseCast.CriticalChanceModifier += eclipseMultiplier;
            if (moonfire != null)
                lunarEclipseCast.CriticalChanceModifier += 0.01f * impInsectSwarm;

            DoMainNuke(talents, calcs, ref preSolarCast, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(talents, calcs, ref solarEclipseCast, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(talents, calcs, ref preLunarCast, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(talents, calcs, ref lunarEclipseCast, spellPower, spellHit, spellCrit, spellHaste);

            if (moonfire != null)
                DoDotSpell(calcs, ref moonfire, spellPower, spellHit, spellCrit, spellHaste);
            if (insectSwarm != null)
                DoDotSpell(calcs, ref insectSwarm, spellPower, spellHit, spellCrit, spellHaste);

            float lunarProcChance = (spellCrit + preLunarCast.CriticalChanceModifier) * spellHit * talents.Eclipse / 3.0f * 0.6f;
            float castsToProcLunar = 1.0f / lunarProcChance;
            float timeToProcLunar = preLunarCast.CastTime * (castsToProcLunar - 0.5f);

            float solarProcChance = (spellCrit + preSolarCast.CriticalChanceModifier) * spellHit * talents.Eclipse / 3.0f;
            float castsToProcSolar = 1.0f / solarProcChance;
            float timeToProcSolar = preSolarCast.CastTime * (castsToProcSolar - 0.5f);

            float rotationLength = 2 * eclipseDuration + timeToProcLunar + timeToProcSolar + 2 * insectSwarmCasts + 2 * moonfireCasts;

            float preLunarTime = timeToProcLunar + (preLunarCast.CastTime * 0.5f) + preLunarCast.NGCastTime * 1.5f;
            float preLunarDPS = preLunarCast.DamagePerHit / preLunarCast.CastTime;
            float preLunarManaUsed = preLunarCast.BaseManaCost / preLunarCast.CastTime * preLunarTime;
            float preLunarManaGained = (preLunarCast.BaseManaCost * omenOfClarityProcChance) +
                ((spellCrit + preLunarCast.CriticalChanceModifier) * spellHit * moonkinFormProc) +
                (preLunarCast.BaseCastTime / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana);

            float lunarTime = eclipseDuration - (preLunarCast.NGCastTime * 1.5f) -  lunarEclipseCast.CastTime * 0.5f;
            float lunarDPS = lunarEclipseCast.DamagePerHit / lunarEclipseCast.CastTime;
            float lunarManaUsed = lunarEclipseCast.BaseManaCost / lunarEclipseCast.CastTime * lunarTime;
            float lunarManaGained = (lunarEclipseCast.BaseManaCost * omenOfClarityProcChance) +
                ((spellCrit + lunarEclipseCast.CriticalChanceModifier) * spellHit * moonkinFormProc) +
                (lunarEclipseCast.BaseCastTime / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana);

            float preSolarTime = timeToProcSolar + (lunarEclipseCast.CastTime * 0.5f) + preSolarCast.NGCastTime;
            float preSolarDPS = preSolarCast.DamagePerHit / preSolarCast.CastTime;
            float preSolarManaUsed = preSolarCast.BaseManaCost / preSolarCast.CastTime * preSolarTime;
            float preSolarManaGained = (preSolarCast.BaseManaCost * omenOfClarityProcChance) +
                ((spellCrit + preSolarCast.CriticalChanceModifier) * spellHit * moonkinFormProc) +
                (preSolarCast.BaseCastTime / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana);

            float solarTime = eclipseDuration - (preSolarCast.NGCastTime) - (preLunarCast.CastTime * 0.5f);
            float solarDPS = solarEclipseCast.DamagePerHit / solarEclipseCast.CastTime;
            float solarManaUsed = solarEclipseCast.BaseManaCost / solarEclipseCast.CastTime * solarTime;
            float solarManaGained = (solarEclipseCast.BaseManaCost * omenOfClarityProcChance) +
                ((spellCrit + solarEclipseCast.CriticalChanceModifier) * spellHit * moonkinFormProc) +
                (solarEclipseCast.BaseCastTime / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana);

            // Moonfire tick calculation:
            // Min(rotationLength, SFglyph + regMF + regMF) / tickLength if 100% uptime specified
            float preSolarMfTicks = moonfire != null ? (float)Math.Min(moonfire.CastTime + ((insectSwarm != null ? insectSwarm.CastTime : 0.0f) + preSolarTime + solarTime) / 3, moonfire.DotEffect.NumberOfTicks) : 0.0f;
            float preLunarMfTicks = moonfire != null ? (float)Math.Min(moonfire.CastTime + ((insectSwarm != null ? insectSwarm.CastTime : 0.0f) + preLunarTime + lunarTime) / 3, moonfire.DotEffect.NumberOfTicks) : 0.0f;
            float moonfireTicks = preSolarMfTicks + preLunarMfTicks;
            float insectSwarmTicks = insectSwarm != null ? insectSwarmCasts * insectSwarm.DotEffect.NumberOfTicks : 0.0f;

            float moonfireDamage = moonfire != null ? moonfireCasts * moonfire.DamagePerHit + (moonfire.DotEffect.DamagePerHit / moonfire.DotEffect.NumberOfTicks) * moonfireTicks : 0.0f;
            float insectSwarmDamage = insectSwarm != null ? insectSwarmCasts * insectSwarm.DotEffect.DamagePerHit : 0.0f;

            float moonfireTime = moonfire != null ? moonfireCasts * moonfire.CastTime : 0.0f;
            float insectSwarmTime = insectSwarm != null ? insectSwarmCasts * insectSwarm.CastTime : 0.0f;

            float moonfireManaUsed = moonfire != null ? moonfireCasts * moonfire.BaseManaCost : 0.0f;
            float insectSwarmManaUsed = insectSwarm != null ? insectSwarmCasts * insectSwarm.BaseManaCost : 0.0f;

            float damageDone = preSolarTime * preSolarDPS + solarTime * solarDPS + preLunarTime * preLunarDPS + lunarTime * lunarDPS +
                moonfireDamage + insectSwarmDamage;

            Duration = rotationLength;
            DotTicks = moonfireTicks + insectSwarmTicks;
            InsectSwarmTicks = insectSwarmTicks;
            MoonfireTicks = moonfireTicks;
            MoonfireCasts = moonfireCasts;
            CastCount = castsToProcLunar + (lunarTime / lunarEclipseCast.CastTime) + castsToProcSolar + (solarTime / solarEclipseCast.CastTime) + moonfireCasts + insectSwarmCasts;

            WrathCount = castsToProcLunar + (solarTime / solarEclipseCast.CastTime);
            StarfireCount = castsToProcSolar + (lunarTime / lunarEclipseCast.CastTime);

            ManaUsed = preSolarManaUsed + solarManaUsed + preLunarManaUsed + lunarManaUsed + moonfireManaUsed + insectSwarmManaUsed;
            ManaGained = castsToProcSolar * preSolarManaGained + (solarTime / solarEclipseCast.CastTime) * solarManaGained + castsToProcLunar * preLunarManaGained + (lunarTime / lunarEclipseCast.CastTime) * lunarManaGained;

            float mfSavingsFromOoC = moonfire != null ? (moonfire.BaseManaCost - (moonfire.BaseManaCost *
                (1 - StarfireCount / WrathCount * 0.06f - (1 - StarfireCount / WrathCount) * 0.06f))) : 0.0f;
            float isSavingsFromOoC = insectSwarm != null ? (insectSwarm.BaseManaCost - (insectSwarm.BaseManaCost *
                (1 - StarfireCount / WrathCount * 0.06f - (1 - StarfireCount / WrathCount) * 0.06f))) : 0.0f;
            ManaGained += moonfire != null ? (moonfireCasts * (mfSavingsFromOoC +
                ((spellCrit + moonfire.CriticalChanceModifier) * moonkinFormProc * spellHit)
                + 1.5f / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana)) : 0.0f;
            ManaGained += insectSwarm != null ? (insectSwarmCasts * (isSavingsFromOoC + 1.5f / 60.0f * calcs.BasicStats.ManaRestoreFromBaseManaPPM * CalculationsMoonkin.BaseMana)) : 0.0f;

            RotationData.ManaGained = ManaGained;
            RotationData.DPM = damageDone / ManaUsed;
            ManaUsed -= ManaGained;
            RotationData.ManaUsed = ManaUsed;

            // Undo SF glyph
            if (starfireGlyph && moonfire != null) moonfire.DotEffect.Duration -= 9.0f;

            // Undo improved Insect Swarm
            if (insectSwarm != null)
            {
                preLunarCast.AllDamageModifier /= 1 + 0.01f * impInsectSwarm;
            }
            if (moonfire != null)
            {
                preSolarCast.CriticalChanceModifier -= 0.01f * impInsectSwarm;
            }

            return damageDone;
        }
    }

    // The interface public class to the rest of Rawr.  Provides a single Solve method that runs all the calculations.
    public class MoonkinSolver
    {
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
                            BaseDamage = (1038.0f + 1222.0f) / 2.0f,
                            SpellDamageModifier = 1.0f,
                            BaseCastTime = 3.5f,
                            CastTime = 3.5f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.16f),
                            DotEffect = null,
                            School = SpellSchool.Arcane
                        },
                        new Spell()
                        {
                            Name = "MF",
                            BaseDamage = (406.0f + 476.0f) / 2.0f,
                            SpellDamageModifier = (1.5f / 3.5f) * (1.5f / 3.5f) / (1.5f / 3.5f + 12f / 15f),
                            BaseCastTime = 1.5f,
                            CastTime = 1.5f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.21f),
                            DotEffect = new DotEffect()
                                {
                                    Duration = 12.0f,
                                    TickLength = 3.0f,
                                    TickDamage = 200.0f,
                                    SpellDamageModifierPerTick = (12f / 15f) * (12f / 15f) / (1.5f / 3.5f + 12f / 15f) / (12.0f / 3.0f)
                                },
                            School = SpellSchool.Arcane
                        },
                        new Spell()
                        {
                            Name = "W",
                            BaseDamage = (558.0f + 628.0f) / 2.0f,
                            SpellDamageModifier = 2.0f/3.5f,
                            BaseCastTime = 2.0f,
                            CastTime = 1.5f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.11f),
                            DotEffect = null,
                            School = SpellSchool.Nature
                        },
                        new Spell()
                        {
                            Name = "IS",
                            BaseDamage = 0.0f,
                            SpellDamageModifier = 0.0f,
                            BaseCastTime = 1.5f,
                            CastTime = 1.5f,
                            BaseManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.08f),
                            DotEffect = new DotEffect()
                            {
                                Duration = 12.0f,
                                TickLength = 2.0f,
                                TickDamage = 1290.0f / 6.0f,
                                SpellDamageModifierPerTick = 0.2f
                            },
                            School = SpellSchool.Nature
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
                default:
                    return null;
            }
        }

        public float NaturesGrace = 0.0f;

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
            float maxBurstDamageDone = 0.0f;
            SpellRotation maxBurstRotation = rotations[0];
            SpellRotation maxRotation = rotations[0];

            float manaPool = GetEffectiveManaPool(character, calcOpts, calcs);

            // Do tree calculations: Calculate damage per cast.
            float treeDamage = (talents.ForceOfNature == 1) ? DoTreeCalcs(baseSpellPower, calcs.BasicStats.PhysicalHaste, calcs.BasicStats.ArmorPenetration, calcs.BasicStats.PhysicalCrit, calcs.BasicStats.Bloodlust, calcOpts.TreantLifespan, character.DruidTalents.Brambles) : 0.0f;
            // Extend that to number of casts per fight.
            float treeCasts = (float)Math.Floor(calcOpts.FightLength / 3) + 1.0f;
            // Partial cast: If the fight lasts 3.x minutes and x is less than 0.5 (30 sec tree duration), calculate a partial cast
            if ((int)calcOpts.FightLength % 3 == 0 && calcOpts.FightLength - (int)calcOpts.FightLength < 0.5)
                treeCasts += (calcOpts.FightLength - (int)calcOpts.FightLength) / 0.5f - 1.0f;
            treeDamage *= treeCasts;
            // Multiply by raid-wide damage increases.
            treeDamage *= (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusPhysicalDamageMultiplier);
            // Calculate the DPS averaged over the fight length.
            float treeDPS = treeDamage / (calcOpts.FightLength * 60.0f);
            // Calculate mana usage for trees.
            float treeManaUsage = (float)Math.Ceiling(treeCasts) * CalculationsMoonkin.BaseMana * 0.12f;
            manaPool -= talents.ForceOfNature == 1 ? treeManaUsage : 0.0f;

			// Do Starfall calculations.
            bool starfallGlyph = talents.GlyphOfStarfall;
            float starfallDamage = (talents.Starfall == 1) ? DoStarfallCalcs(baseSpellPower, baseHit, baseCrit, Wrath.CriticalDamageModifier) : 0.0f;
            float starfallCD = 1.5f - (starfallGlyph ? 0.5f : 0.0f);
            float numStarfallCasts = (float)Math.Floor(calcOpts.FightLength / starfallCD) + 1.0f;
            // Partial cast: If the difference between fight length and total starfall CD time is less than 10 seconds (duration),
            // calculate a partial cast
            float starfallDiff = calcOpts.FightLength * 60.0f - (numStarfallCasts - 1) * starfallCD * 60.0f;
            if (starfallDiff > 0 && starfallDiff < 10)
                numStarfallCasts += starfallDiff / 60.0f / (1.0f / 6.0f) - 1.0f;
            starfallDamage *= numStarfallCasts;
			starfallDamage *= (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
			float starfallDPS = starfallDamage / (calcOpts.FightLength * 60.0f);
            float starfallManaUsage = (float)Math.Ceiling(numStarfallCasts) * CalculationsMoonkin.BaseMana * 0.39f;
            manaPool -= talents.Starfall == 1 ? starfallManaUsage : 0.0f;

            // Simple faerie fire mana calc
            float faerieFireCasts = (float)Math.Floor(calcOpts.FightLength / 5) + (calcOpts.FightLength % 5 != 0 ? 1.0f : 0.0f);
            float faerieFireMana = faerieFireCasts * CalculationsMoonkin.BaseMana * 0.08f;
            if (talents.ImprovedFaerieFire > 0)
                manaPool -= faerieFireMana;

            // Calculate effect of casting Starfall/Treants/ImpFF (regular FF is assumed to be provided by a feral)
            float globalCooldown = 1.5f / (1 + baseHaste) + calcs.Latency;
            float treantTime = (talents.ForceOfNature == 1) ? globalCooldown * (float)Math.Ceiling(treeCasts) : 0.0f;
            float starfallTime = (talents.Starfall == 1) ? globalCooldown * (float)Math.Ceiling(numStarfallCasts) : 0.0f;
            float faerieFireTime = (talents.ImprovedFaerieFire > 0) ? globalCooldown * faerieFireCasts : 0.0f;

            float totalTimeInRotation = calcs.FightLength * 60.0f - (treantTime + starfallTime + faerieFireTime);
            float percentTimeInRotation = totalTimeInRotation / (calcs.FightLength * 60.0f);
            
            float manaGained = manaPool - calcs.BasicStats.Mana;

            foreach (SpellRotation rot in rotations)
            {
                rot.Solver = this;
                float accumulatedDamage = 0.0f;
                float totalUpTime = 0.0f;
                List<ProcEffect> activatedEffects = new List<ProcEffect>();
                // Calculate damage and mana contributions for non-stat-boosting trinkets
                // Separate stat-boosting proc trinkets into their own list
                foreach (ProcEffect proc in procEffects)
                {
                    if (proc.CalculateDPS != null)
                    {
                        if (rot.Duration == 0.0f)
                            rot.DamageDone(talents, calcs, baseSpellPower, baseHit, baseCrit, baseHaste);
                        accumulatedDamage += proc.CalculateDPS(rot, calcs, baseSpellPower, baseHit, baseCrit, baseHaste) * rot.Duration;
                    }
                    else if (proc.Activate != null)
                    {
                        activatedEffects.Add(proc);
                    }
                    if (proc.CalculateMP5 != null)
                    {
                        if (rot.Duration == 0.0f)
                            rot.DamageDone(talents, calcs, baseSpellPower, baseHit, baseCrit, baseHaste);
                        manaGained += proc.CalculateMP5(rot, calcs, baseSpellPower, baseHit, baseCrit, baseHaste) / 5.0f * calcs.FightLength * 60.0f;
                    }
                }
                // Calculate stat-boosting trinkets, taking into effect interactions with other stat-boosting procs
                int sign = 1;
                Dictionary<int, float> cachedDamages = new Dictionary<int, float>();
                Dictionary<int, float> cachedUptimes = new Dictionary<int, float>();
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
                            activatedEffects[idx].Activate(character, calcs, ref baseSpellPower, ref baseHit, ref baseCrit, ref baseHaste);
                        }
                        float tempDPS = rot.DamageDone(talents, calcs, baseSpellPower, baseHit, baseCrit, baseHaste) / rot.Duration;
                        foreach (int idx in vals)
                        {
                            tempUpTime *= activatedEffects[idx].UpTime(rot, calcs);
                            activatedEffects[idx].Deactivate(character, calcs, ref baseSpellPower, ref baseHit, ref baseCrit, ref baseHaste);
                        }
                        // Adjust previous probability tables by the current factor
                        // At the end of the algorithm, this ensures that the probability table will contain the individual
                        // probabilities of each effect or set of effects.
                        // These adjustments only need to be made for higher levels of the table, and if the current probability is > 0.
                        if (tempUpTime > 0 && lengthCounter > 1)
                        {
                            List<int> keys = new List<int>(cachedUptimes.Keys);
                            foreach (int subset in keys)
                            {
                                // Calculate the "layer" of the current subset by getting the set bit count.
                                int subsetLength = 0;
                                for (int j = 0; j < 32; ++j)
                                    if ((subset & (1 << j)) > 0)
                                        ++subsetLength;
                                // Entries that are in the current "layer" or higher in the table are not subsets, by definition
                                if (subsetLength >= lengthCounter) break;
                                // Set the sign of the operation: Evenly separated layers are added, oddly separated layers are subtracted
                                int newSign = ((lengthCounter - subsetLength) % 2 == 0) ? 1 : -1;
                                // Check for subset.
                                // If it is a subset, adjust by current uptime * sign of operation.
                                if ((pairs & subset) == subset)
                                {
                                    cachedUptimes[subset] += newSign * tempUpTime;
                                }
                            }
                        }
                        // Cache the results to be calculated later
                        cachedUptimes[pairs] = tempUpTime;
                        cachedDamages[pairs] = tempDPS;
                        totalUpTime += sign * tempUpTime;
                    }
                    sign = -sign;
                }
                float accumulatedDPS = 0.0f;
                // Apply the above-calculated probabilities to the previously stored damage calculations and add to the result.
                foreach (KeyValuePair<int, float> kvp in cachedUptimes)
                {
                    accumulatedDPS += kvp.Value * cachedDamages[kvp.Key];
                }
                float damageDone = rot.DamageDone(talents, calcs, baseSpellPower, baseHit, baseCrit, baseHaste);
                accumulatedDPS += (1 - totalUpTime) * damageDone / rot.Duration;

                accumulatedDamage += accumulatedDPS * rot.Duration;

                float burstDPS = accumulatedDamage / rot.Duration * percentTimeInRotation;
                float sustainedDPS = burstDPS;
                float timeToOOM = (manaPool / rot.RotationData.ManaUsed) * rot.Duration;
                if (timeToOOM < calcs.FightLength * 60.0f)
                {
                    rot.RotationData.TimeToOOM = new TimeSpan(0, (int)(timeToOOM / 60), (int)(timeToOOM % 60));
                    sustainedDPS = burstDPS * timeToOOM / (calcs.FightLength * 60.0f);
                }
                burstDPS += trinketDPS + treeDPS + starfallDPS;
                sustainedDPS += trinketDPS + treeDPS + starfallDPS;
                rot.RotationData.BurstDPS = burstDPS;
                rot.RotationData.DPS = sustainedDPS;

                // Update the sustained DPS rotation if any one of the following three cases is true:
                // 1) No user rotation is selected and sustained DPS is maximum
                // 2) A user rotation is selected, Eclipse is not present, and the user rotation matches the current rotation
                // 3) A user rotation is selected, Eclipse is present, and the user rotation's dot spells matches this rotation's
                if ((calcOpts.UserRotation == "None" && sustainedDPS > maxDamageDone) ||
                    (character.DruidTalents.Eclipse == 0 && calcOpts.UserRotation == rot.Name) ||
                    (character.DruidTalents.Eclipse > 0 && (calcOpts.UserRotation == rot.Name.Replace("Filler", "SF") ||
                    calcOpts.UserRotation == rot.Name.Replace("Filler", "W"))))
                {
                    maxDamageDone = sustainedDPS;
                    maxRotation = rot;
                }
                if (burstDPS > maxBurstDamageDone)
                {
                    maxBurstDamageDone = burstDPS;
                    maxBurstRotation = rot;
                }
                rot.ManaGained += manaGained / (calcs.FightLength * 60.0f) * rot.Duration;
                rot.RotationData.ManaGained += manaGained / (calcs.FightLength * 60.0f) * rot.Duration;
                if (rot.Name.Contains("Filler"))
                {
                    cachedResults[rot.Name.Replace("Filler", "SF")] = rot.RotationData;
                    cachedResults[rot.Name.Replace("Filler", "W")] = rot.RotationData;
                }
                else
                    cachedResults[rot.Name] = rot.RotationData;
            }
            // Present the findings to the user.
            calcs.SelectedRotation = maxRotation;
            calcs.BurstDPSRotation = maxBurstRotation;
            calcs.SubPoints = new float[] { maxDamageDone, maxBurstDamageDone };
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

            float innervateCooldown = 360 - calcs.BasicStats.InnervateCooldownReduction;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen5SR * fightLength;

            // Mana pot calculations
            int numPots = calcOpts.ManaPots ? 1 : 0;
            float manaRestoredByPots = 0.0f;
            if (numPots > 0)
            {
                float manaPerPot = 0.0f;
                if (calcOpts.ManaPotType == "Runic Mana Potion")
                    manaPerPot = 4320.0f;
                if (calcOpts.ManaPotType == "Fel Mana Potion")
                    manaPerPot = 3200.0f;
                // Bonus from Alchemist's Stone
                if (calcs.BasicStats.BonusManaPotion > 0)
                {
                    manaPerPot *= 1 + calcs.BasicStats.BonusManaPotion;
                }

                manaRestoredByPots = numPots * manaPerPot;
            }

            // Innervate calculations
            float innervateDelay = calcOpts.InnervateDelay * 60.0f;
            int numInnervates = (calcOpts.Innervate && fightLength - innervateDelay > 0) ? ((int)(fightLength - innervateDelay) / (int)innervateCooldown + 1) : 0;
            float totalInnervateMana = numInnervates * CalculationsMoonkin.BaseMana * (4.5f + (character.DruidTalents.GlyphOfInnervate ? 0.9f : 0.0f));

            // Replenishment calculations
            float replenishmentPerTick = calcs.BasicStats.Mana * calcs.BasicStats.ManaRestoreFromMaxManaPerSecond;
            float replenishmentMana = calcOpts.ReplenishmentUptime * replenishmentPerTick * calcOpts.FightLength * 60;

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen + manaRestoredByPots + replenishmentMana;
        }

        // Now returns damage per cast to allow adjustments for fight length
        private float DoTreeCalcs(float effectiveNatureDamage, float meleeHaste, float armorPen, float meleeCrit, float bloodLust, float treantLifespan, int bramblesLevel)
        {
            // 642 = base AP, 57% spell power scaling
            float attackPower = 642.0f + (float)Math.Floor(0.57f * effectiveNatureDamage);
            // 398.8 = base DPS, 1.7 = best observed swing speed
            float damagePerHit = (398.8f + attackPower / 14.0f) * 1.7f;
            float critRate = 0.05f + meleeCrit;
            float glancingRate = 0.2f;
            float bossArmor = StatConversion.NPC_ARMOR[83-80] * (1.0f - armorPen);
            float damageReduction = bossArmor / (bossArmor + 15232.5f);
            damagePerHit *= 1.0f - damageReduction;
            damagePerHit = (critRate * damagePerHit * 2.0f) + (glancingRate * damagePerHit * 0.75f) + ((1 - critRate - glancingRate) * damagePerHit);
            float attackSpeed = 1.7f / (1 + meleeHaste) / (1 + bloodLust);
            float damagePerTree = (treantLifespan * 30.0f / attackSpeed) * damagePerHit * (1 + 0.05f * bramblesLevel);
            return 3 * damagePerTree;
        }

		// Starfall
		private float DoStarfallCalcs(float effectiveArcaneDamage, float spellHit, float spellCrit, float critDamageModifier)
		{
			float baseDamage = 5460.0f;

			// Spell coefficient = 60%
			float damagePerNormalHit = baseDamage + 0.6f * effectiveArcaneDamage;
			float damagePerCrit = damagePerNormalHit * critDamageModifier;
			return (spellCrit * damagePerCrit + (1 - spellCrit) * damagePerNormalHit) * spellHit;
		}

        // Redo the spell calculations
        private void RecreateSpells(DruidTalents talents, ref CharacterCalculationsMoonkin calcs)
        {
            ResetSpellList();
            if (talents.Eclipse == 0)
            {
                rotations = new List<SpellRotation>(new SpellRotation[]
                {
                    new SpellRotation()
                    {
                        Name = "MF/SF",
                        SpellsUsed = new List<string>(new string[]{ "MF", "SF" })
                    },
                    new SpellRotation()
                    {
                        Name = "MF/W",
                        SpellsUsed = new List<string>(new string[]{ "MF", "W" })
                    },
                    new SpellRotation()
                    {
                        Name = "IS/SF",
                        SpellsUsed = new List<string>(new string[]{ "IS", "SF" })
                    },
                    new SpellRotation()
                    {
                        Name = "IS/W",
                        SpellsUsed = new List<string>(new string[]{ "IS", "W" })
                    },
                    new SpellRotation()
                    {
                        Name = "IS/MF/SF",
                        SpellsUsed = new List<string>(new string[]{ "IS", "MF", "SF" })
                    },
                    new SpellRotation()
                    {
                        Name = "IS/MF/W",
                        SpellsUsed = new List<string>(new string[]{ "IS", "MF", "W" })
                    },
                    new SpellRotation()
                    {
                        Name = "SF Spam",
                        SpellsUsed = new List<string>(new string[]{ "SF" })
                    },
                    new SpellRotation()
                    {
                        Name = "W Spam",
                        SpellsUsed = new List<string>(new string[]{ "W" })
                    }
                });
            }
            else
            {
                rotations = new List<SpellRotation>(new SpellRotation[] {
                    new SpellRotation()
                    {
                        Name = "W Spam",
                        SpellsUsed = new List<string>(new string[] { "W" })
                    },
                    new SpellRotation()
                    {
                        Name = "SF Spam",
                        SpellsUsed = new List<string>(new string[] { "SF" })
                    },
                    new SpellRotation()
                    {
                        Name = "MF/Filler",
                        SpellsUsed = new List<string>(new String[] { "MF", "SF" })
                    },
                    new SpellRotation()
                    {
                        Name = "IS/Filler",
                        SpellsUsed = new List<string>(new string[] { "IS", "W" })
                    },
                    new SpellRotation()
                    {
                        Name = "IS/MF/Filler",
                        SpellsUsed = new List<string>(new string[] { "IS", "MF", "SF" })
                    }
                });
            }

            UpdateSpells(talents, ref calcs);
        }

        // Add talented effects to the spells
        private void UpdateSpells(DruidTalents talents, ref CharacterCalculationsMoonkin calcs)
        {
            Stats stats = calcs.BasicStats;
            // Add (possibly talented) +spelldmg
            // Starfire: Damage +(0.04 * Wrath of Cenarius)
            // Wrath: Damage +(0.02 * Wrath of Cenarius)
            Wrath.SpellDamageModifier += 0.02f * talents.WrathOfCenarius;
            Starfire.SpellDamageModifier += 0.04f * talents.WrathOfCenarius;

            // Add spell damage from idols
            //Starfire.IdolExtraSpellPower += stats.StarfireDmg;
            Starfire.BaseDamage += stats.StarfireDmg;
            //Moonfire.IdolExtraSpellPower += stats.MoonfireDmg;
            Moonfire.BaseDamage += stats.MoonfireDmg;
            Wrath.BaseDamage += stats.WrathDmg;
            //InsectSwarm.IdolExtraSpellPower += stats.InsectSwarmDmg;
            InsectSwarm.DotEffect.TickDamage += stats.InsectSwarmDmg / InsectSwarm.DotEffect.NumberOfTicks;

            float moonfireDDGlyph = talents.GlyphOfMoonfire ? -0.9f : 0.0f;
            float moonfireDotGlyph = talents.GlyphOfMoonfire ? 0.75f : 0.0f;
            float insectSwarmGlyph = talents.GlyphOfInsectSwarm ? 0.3f : 0.0f;
            // Add spell-specific damage
            // Starfire, Moonfire, Wrath: Damage +(0.03 * Moonfury) (Additive with 4T9?)
            // Moonfire: Damage +(0.05 * Imp Moonfire) (Additive with Moonfury/Genesis/Glyph)
            // Moonfire, Insect Swarm: Dot Damage +(0.01 * Genesis) (Additive with Moonfury/Imp. Moonfire/Glyph/Set bonus)
            Wrath.AllDamageModifier *= 1 + (float)Math.Floor(talents.Moonfury * 10 / 3.0f) / 100.0f + stats.BonusMoonkinNukeDamage;
            Moonfire.AllDamageModifier *= 1 + (float)Math.Floor(talents.Moonfury * 10 / 3.0f) / 100.0f
                                            + 0.05f * talents.ImprovedMoonfire
                                            + moonfireDDGlyph;
            Moonfire.DotEffect.AllDamageModifier *= 1 + (float)Math.Floor(talents.Moonfury * 10 / 3.0f) / 100.0f
                                                      + 0.01f * talents.Genesis
                                                      + 0.05f * talents.ImprovedMoonfire
                                                      + moonfireDotGlyph;
            Starfire.AllDamageModifier *= 1 + (float)Math.Floor(talents.Moonfury * 10 / 3.0f) / 100.0f + stats.BonusMoonkinNukeDamage;
            InsectSwarm.DotEffect.AllDamageModifier *= 1 + 0.01f * talents.Genesis
                                                         + insectSwarmGlyph
                                                         + stats.BonusInsectSwarmDamage;

            // Moonfire, Insect Swarm: One extra tick (Nature's Splendor)
            Moonfire.DotEffect.Duration += 3.0f * talents.NaturesSplendor;
            InsectSwarm.DotEffect.Duration += 2.0f * talents.NaturesSplendor;
            // Moonfire: Crit chance +(0.05 * Imp Moonfire)
            Moonfire.CriticalChanceModifier += 0.05f * talents.ImprovedMoonfire;

            // Wrath, Insect Swarm: Nature spell damage multipliers
            Wrath.AllDamageModifier *= ((1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier));
            InsectSwarm.DotEffect.AllDamageModifier *= ((1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier));
            // Starfire, Moonfire: Arcane damage multipliers
            Starfire.AllDamageModifier *= ((1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier));
            Moonfire.AllDamageModifier *= ((1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier));
            Moonfire.DotEffect.AllDamageModifier *= ((1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier));

            // Level-based partial resistances
            Wrath.AllDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);
            Starfire.AllDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);
            Moonfire.AllDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);
            Moonfire.DotEffect.AllDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);
            // Insect Swarm is a binary spell

            // Add spell-specific crit chance
            // Wrath, Starfire: Crit chance +(0.02 * Nature's Majesty)
            Wrath.CriticalChanceModifier += 0.02f * talents.NaturesMajesty;
            Starfire.CriticalChanceModifier += 0.02f * talents.NaturesMajesty;

            // Add spell-specific critical strike damage
            // Chaotic Skyfire Diamond
            Starfire.CriticalDamageModifier = stats.BonusCritMultiplier > 0 ? 1.5f * (1+stats.BonusCritMultiplier) : 1.5f;
            Wrath.CriticalDamageModifier = stats.BonusCritMultiplier > 0 ? 1.5f * (1 + stats.BonusCritMultiplier) : 1.5f;
            Moonfire.CriticalDamageModifier = stats.BonusCritMultiplier > 0 ? 1.5f * (1 + stats.BonusCritMultiplier) : 1.5f;
            // Starfire, Moonfire, Wrath: Crit damage +(0.2 * Vengeance)
            Starfire.CriticalDamageModifier = (Starfire.CriticalDamageModifier - 1.0f) * (1 + 0.2f * talents.Vengeance) + 1.0f;
            Wrath.CriticalDamageModifier = (Wrath.CriticalDamageModifier - 1.0f) * (1 + 0.2f * talents.Vengeance) + 1.0f;
            Moonfire.CriticalDamageModifier = (Moonfire.CriticalDamageModifier - 1.0f) * (1 + 0.2f * talents.Vengeance) + 1.0f;

            // Reduce spell-specific mana costs
            // Starfire, Moonfire, Wrath: Mana cost -(0.03 * Moonglow)
            Starfire.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Moonfire.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);
            Wrath.BaseManaCost *= 1.0f - (0.03f * talents.Moonglow);

            // Add set bonuses
            // 2T6
            Moonfire.DotEffect.Duration += stats.MoonfireExtension;
            // 4T6
            Starfire.CriticalChanceModifier += stats.StarfireCritChance;
            // 4T7
            Starfire.CriticalChanceModifier += stats.BonusNukeCritChance;
            Wrath.CriticalChanceModifier += stats.BonusNukeCritChance;
            // 2T9
            Moonfire.DotEffect.CanCrit = stats.MoonfireDotCrit == 1;

            // Nature's Grace
            NaturesGrace = talents.NaturesGrace;
        }

    }
}
