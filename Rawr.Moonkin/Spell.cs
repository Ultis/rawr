using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{
    enum SpellSchool
    {
        Arcane,
        Nature
    }
    class Spell
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
    class DotEffect
    {
        public DotEffect() { AllDamageModifier = 1.0f; }
        public DotEffect(DotEffect copy)
        {
            this.AllDamageModifier = copy.AllDamageModifier;
            this.BaseDamage = copy.BaseDamage;
            this.Duration = copy.Duration;
            this.TickDamage = copy.TickDamage;
            this.TickLength = copy.TickLength;
            this.SpellDamageModifier = copy.SpellDamageModifier;
        }
        public float Duration { get; set; }
        public float TickLength { get; set; }
        public float TickDamage { get; set; }
        public float SpellDamageModifier { get; set; }
        public float AllDamageModifier { get; set; }
        public float NumberOfTicks
        {
            get
            {
                return Duration / TickLength;
            }
        }
        public float SpellDamageModifierPerTick
        {
            get
            {
                return SpellDamageModifier / NumberOfTicks;
            }
            set
            {
                SpellDamageModifier += value * NumberOfTicks;
            }
        }
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
    class RotationData
    {
        public float BurstDPS = 0.0f;
        public float DPS = 0.0f;
        public float DPM = 0.0f;
        public float ManaUsed = 0.0f;
        public float ManaGained = 0.0f;
        public TimeSpan TimeToOOM = new TimeSpan(0, 0, 0);
    }

    // Define delegate types for proc effect class
    // Enable and disable the effect of the proc.  These two delegates should perform exact opposite operations.
    delegate void Activate(CharacterCalculationsMoonkin calcs, ref float spellPower, ref float spellHit, ref float spellCrit, ref float spellHaste);
    delegate void Deactivate(CharacterCalculationsMoonkin calcs, ref float spellPower, ref float spellHit, ref float spellCrit, ref float spellHaste);
    // Calculate the uptime of the effect.  This will be used to weight the proc when calculating the rotational DPS.
    delegate float UpTime(SpellRotation rotation, CharacterCalculationsMoonkin calcs);
    // Optional calculations for complicated proc effects like Eclipse or trinkets that proc additional damage.
    // The return value of this calculation will be ADDED to the rotational DPS.
    delegate float CalculateDPS(SpellRotation rotation, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste);
    // The return value of this calculation will be used to adjust the mana statistics of the rotation.
    delegate float CalculateMP5(SpellRotation rotation, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste);

    // The proc effect class itself.
    class ProcEffect
    {
        public Activate Activate { get; set; }
        public Deactivate Deactivate { get; set; }
        public UpTime UpTime { get; set; }
        public CalculateDPS CalculateDPS { get; set; }
        public CalculateMP5 CalculateMP5 { get; set; }
    }

    // Combination generator class
    // Helper class to generate all possible combinations of proc effects to mathematically account for interactions.
    class CombinationGenerator
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
    class SpellRotation
    {
        public MoonkinSolver Solver { get; set; }
        private Spell LocateSpell(Spell[] SpellData, string name)
        {
            return Array.Find<Spell>(SpellData, delegate(Spell sp) { return sp.Name == name; });
        }
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

        // Calculate damage and casting time for a single, direct-damage spell.
        private void DoMainNuke(Character character, CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;

            mainNuke.CastTime = mainNuke.BaseCastTime - 0.1f * character.DruidTalents.StarlightWrath;
            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            float normalCastTime = (float)Math.Max(1.0f, mainNuke.CastTime / (1 + spellHaste)) + latency;
            mainNuke.NGCastTime = (float)Math.Max(1.0f, mainNuke.CastTime / (1 + spellHaste) / (1 + 0.2f * character.DruidTalents.NaturesGrace / 3.0f)) + latency;
            float NGProcChance = totalCritChance * character.DruidTalents.NaturesGrace / 3.0f;
            float NGUptime = 1.0f - (float)Math.Pow(1.0f - NGProcChance, Math.Floor(3.0f / normalCastTime) + 1.0f);
            mainNuke.CastTime = (1 - NGUptime) * normalCastTime + NGUptime * mainNuke.NGCastTime;
            // Damage calculations
            float damagePerNormalHit = (mainNuke.BaseDamage + mainNuke.SpellDamageModifier * (spellPower + mainNuke.IdolExtraSpellPower)) * mainNuke.AllDamageModifier;
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier;
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
            float damagePerCrit = damagePerNormalHit * mainNuke.CriticalDamageModifier;
            mainNuke.DamagePerHit = (totalCritChance * damagePerCrit + (1 - totalCritChance) * damagePerNormalHit) * spellHit;
        }

        // Calculate damage and casting time for a damage-over-time effect.
        private void DoDotSpell(Character character, CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            if (dotSpell.Name == "MF")
                DoMoonfire(character, calcs, ref dotSpell, spellPower, spellHit, spellCrit, spellHaste);
            else
                DoInsectSwarm(character, calcs, ref dotSpell, spellPower, spellHit, spellCrit, spellHaste);
        }

        // Calculate damage and casting time for the Moonfire effect.
        private void DoMoonfire(Character character, CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;
            dotSpell.CastTime = Math.Max(dotSpell.BaseCastTime / (1 + spellHaste), 1.0f + latency) + latency;
            float mfDirectDamage = (dotSpell.BaseDamage + dotSpell.SpellDamageModifier * (spellPower + dotSpell.IdolExtraSpellPower)) * dotSpell.AllDamageModifier;
            float mfCritDamage = mfDirectDamage * dotSpell.CriticalDamageModifier;
            float totalCritChance = spellCrit + dotSpell.CriticalChanceModifier;
            dotSpell.DamagePerHit = (totalCritChance * mfCritDamage + (1 - totalCritChance) * mfDirectDamage) * spellHit;
            float damagePerTick = (dotSpell.DotEffect.TickDamage + dotSpell.DotEffect.SpellDamageModifierPerTick * spellPower) * dotSpell.DotEffect.AllDamageModifier;
            dotSpell.DotEffect.DamagePerHit = dotSpell.DotEffect.NumberOfTicks * damagePerTick * spellHit;
        }

        // Calculate damage and casting time for the Insect Swarm effect.
        private void DoInsectSwarm(Character character, CharacterCalculationsMoonkin calcs, ref Spell dotSpell, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;
            dotSpell.CastTime = Math.Max(dotSpell.BaseCastTime / (1 + spellHaste), 1.0f + latency) + latency;
            float damagePerTick = (dotSpell.DotEffect.TickDamage + dotSpell.DotEffect.SpellDamageModifierPerTick * (spellPower + dotSpell.IdolExtraSpellPower)) * dotSpell.DotEffect.AllDamageModifier;
            dotSpell.DotEffect.DamagePerHit = dotSpell.DotEffect.NumberOfTicks * damagePerTick * spellHit;
        }

        // Perform damage and mana calculations for all spells in the given rotation.  Returns damage done over the total duration.
        public float DamageDone(Character character, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            if (character.DruidTalents.Eclipse > 0)
            {
                return DoEclipseCalcs(character, calcs, Solver, spellPower, spellHit, spellCrit, spellHaste);
            }
            float latency = calcs.Latency;

            float JoWProc = character.ActiveBuffsContains("Judgement of Wisdom") ? 0.02f * CalculationsMoonkin.BaseMana : 0.0f;
            float moonkinFormProc = (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm == 1) ? 0.02f * calcs.BasicStats.Mana : 0.0f;
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            bool starfireGlyph = calcOpts.glyph1 == "Starfire" || calcOpts.glyph2 == "Starfire" || calcOpts.glyph3 == "Starfire";

            switch (SpellsUsed.Count)
            {
                // Nuke only
                case 1:
                    Spell mainNuke = LocateSpell(Solver.SpellData, SpellsUsed[0]);
                    DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

                    float omenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 0.06f : 0;
                    mainNuke.ManaCost = mainNuke.BaseManaCost - 0.25f * JoWProc * spellHit - (spellCrit + mainNuke.CriticalChanceModifier) * moonkinFormProc - mainNuke.BaseManaCost * omenProcChance * spellHit;
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
                    Spell DotEffectSpell = LocateSpell(Solver.SpellData, SpellsUsed[0]);
                    mainNuke = LocateSpell(Solver.SpellData, SpellsUsed[1]);
                    // Do Starfire glyph calculations, if applicable; then do DoT spell calculations
                    if (starfireGlyph && mainNuke.Name == "SF" && DotEffectSpell.Name == "MF") DotEffectSpell.DotEffect.Duration += 9.0f;
                    DoDotSpell(character, calcs, ref DotEffectSpell, spellPower, spellHit, spellCrit, spellHaste);

                    // Do iIS calculations, if applicable
                    if (character.DruidTalents.ImprovedInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF" && DotEffectSpell.Name == "MF")
                            mainNuke.CriticalChanceModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                        else if (mainNuke.Name == "W" && DotEffectSpell.Name == "IS")
                            mainNuke.AllDamageModifier *= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                    }
                    // Calculate main nuke damage
                    DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

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

                    float manaFromJoW = (numNukeCasts + 1) / 4 * JoWProc * spellHit;
                    float manaFromOoC = ((0.06f) * mainNuke.BaseManaCost
                        + (numNukeCasts - 1) * (0.06f) * mainNuke.BaseManaCost
                        + (0.06f) * DotEffectSpell.BaseManaCost) * spellHit;
                    float manaFromMoonkin = moonkinFormProc * spellHit * ((spellCrit + mainNuke.CriticalChanceModifier) * numNukeCasts + DotEffectSpell.CriticalChanceModifier);

                    float actualManaSpent = totalManaSpent - manaFromJoW - manaFromMoonkin - (character.DruidTalents.OmenOfClarity == 1 ? manaFromOoC : 0.0f);

                    RotationData.ManaUsed = ManaUsed = actualManaSpent;
                    RotationData.ManaGained = ManaGained = totalManaSpent - actualManaSpent;
                    RotationData.DPM = (nukeDamageDone + DotEffectSpell.DamagePerHit + DotEffectSpell.DotEffect.DamagePerHit) / totalManaSpent;
                    
                    // Undo iIS, if applicable
                    if (character.DruidTalents.ImprovedInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF" && DotEffectSpell.Name == "MF")
                            mainNuke.CriticalChanceModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                        else if (mainNuke.Name == "W" && DotEffectSpell.Name == "IS")
                            mainNuke.AllDamageModifier /= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                    }
                    // Undo SF glyph, if applicable
                    if (starfireGlyph && mainNuke.Name == "SF" && DotEffectSpell.Name == "MF") DotEffectSpell.DotEffect.Duration -= 9.0f;
                    // Return the damage done per rotation
                    return nukeDamageDone + DotEffectSpell.DamagePerHit + DotEffectSpell.DotEffect.DamagePerHit;
                // Nuke + both DotEffects
                case 3:
                    // Find the spells
                    Spell moonFire = LocateSpell(Solver.SpellData, SpellsUsed[0]);
                    Spell insectSwarm = LocateSpell(Solver.SpellData, SpellsUsed[1]);
                    mainNuke = LocateSpell(Solver.SpellData, SpellsUsed[2]);
                    // Do Starfire glyph calculations, if applicable; then do DoT spell calculations
                    if (starfireGlyph && mainNuke.Name == "SF") moonFire.DotEffect.Duration += 9.0f;
                    DoDotSpell(character, calcs, ref moonFire, spellPower, spellHit, spellCrit, spellHaste);
                    DoDotSpell(character, calcs, ref insectSwarm, spellPower, spellHit, spellCrit, spellHaste);

                    // Do iIS calculations, if applicable
                    if (character.DruidTalents.ImprovedInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF")
                            mainNuke.CriticalChanceModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                        else if (mainNuke.Name == "W")
                            mainNuke.AllDamageModifier *= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                    }
                    // Calculate main nuke damage
                    DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

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

                    manaFromJoW = (numNukeCasts + 1 + numISCasts) / 4 * JoWProc * spellHit;
                    manaFromOoC = ((0.06f) * mainNuke.BaseManaCost
                        + (numNukeCasts - 1 - numISCasts) * (0.06f) * mainNuke.BaseManaCost
                        + (0.06f) * moonFire.BaseManaCost
                        + (0.06f) * numISCasts * insectSwarm.BaseManaCost
                        + (0.06f) * numISCasts * mainNuke.BaseManaCost) * spellHit;
                    manaFromMoonkin = moonkinFormProc * spellHit * ((spellCrit + mainNuke.CriticalChanceModifier) * numNukeCasts + moonFire.CriticalChanceModifier);

                    actualManaSpent = totalManaSpent - manaFromJoW - manaFromMoonkin - (character.DruidTalents.OmenOfClarity == 1 ? manaFromOoC : 0.0f);

                    RotationData.ManaUsed = ManaUsed = actualManaSpent;
                    RotationData.ManaGained = ManaGained = totalManaSpent - actualManaSpent;
                    RotationData.DPM = (nukeDamageDone + moonFire.DamagePerHit + moonFire.DotEffect.DamagePerHit + insectSwarm.DotEffect.DamagePerHit) / totalManaSpent;

                    // Undo iIS, if applicable
                    if (character.DruidTalents.ImprovedInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF")
                            mainNuke.CriticalChanceModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                        else if (mainNuke.Name == "W")
                            mainNuke.AllDamageModifier /= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                    }
                    // Undo SF glyph
                    if (starfireGlyph && mainNuke.Name == "SF") moonFire.DotEffect.Duration -= 9.0f;
                    // Return the damage done per rotation
                    return nukeDamageDone + moonFire.DamagePerHit + moonFire.DotEffect.DamagePerHit + insectSwarmDamage;
                default:
                    throw new Exception("Invalid rotation specified in rotation solver.");
            }
        }

        private float DoEclipseCalcs(Character character, CharacterCalculationsMoonkin calcs, MoonkinSolver solver, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;

            float JoWProc = character.ActiveBuffsContains("Judgement of Wisdom") ? 0.02f * CalculationsMoonkin.BaseMana : 0.0f;
            float omenOfClarityProcChance = character.DruidTalents.OmenOfClarity * 0.06f;
            float moonkinFormProc = (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm == 1) ? 0.02f * calcs.BasicStats.Mana : 0.0f;
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            bool starfireGlyph = calcOpts.glyph1 == "Starfire" || calcOpts.glyph2 == "Starfire" || calcOpts.glyph3 == "Starfire";

            float moonfireCasts = SpellsUsed.Contains("MF") ? 1.0f + (calcOpts.MoonfireAlways ? 1.0f : 0.0f) : 0.0f;
            float insectSwarmCasts = SpellsUsed.Contains("IS") ? 2.0f : 0.0f;

            Spell moonfire = moonfireCasts > 0 ? LocateSpell(solver.SpellData, "MF") : null;
            Spell insectSwarm = insectSwarmCasts > 0 ? LocateSpell(solver.SpellData, "IS") : null;

            // Do SF glyph
            if (starfireGlyph && moonfire != null) moonfire.DotEffect.Duration += 9.0f;

            float eclipseMultiplier = 0.3f + calcs.BasicStats.EclipseBonus;

            float eclipseDuration = 15.0f;
            float eclipseCooldown = 30.0f;

            Spell preEclipseCast = LocateSpell(solver.SpellData, calcOpts.LunarEclipse ? "W" : "SF");

            // Do improved Insect Swarm
            if (preEclipseCast.Name == "W" && insectSwarm != null)
                preEclipseCast.AllDamageModifier *= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
            else if (preEclipseCast.Name == "SF" && moonfire != null)
                preEclipseCast.CriticalChanceModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;

            Spell eclipseCast = new Spell(LocateSpell(solver.SpellData, calcOpts.LunarEclipse ? "SF" : "W"));

            // Eclipse bonus and improved Insect Swarm
            if (eclipseCast.Name == "W")
            {
                eclipseCast.AllDamageModifier *= 1.0f + eclipseMultiplier;
                if (insectSwarm != null)
                    eclipseCast.AllDamageModifier *= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
            }
            else
            {
                eclipseCast.CriticalChanceModifier += eclipseMultiplier;
                if (moonfire != null)
                    eclipseCast.CriticalChanceModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
            }

            Spell postEclipseCast = LocateSpell(solver.SpellData, SpellsUsed[SpellsUsed.Count - 1]);

            DoMainNuke(character, calcs, ref preEclipseCast, spellPower, spellHit, spellCrit, spellHaste);
            DoMainNuke(character, calcs, ref eclipseCast, spellPower, spellHit, spellCrit, spellHaste);
            if (preEclipseCast != postEclipseCast)
            {
                // Do improved Insect Swarm
                if (postEclipseCast.Name == "W" && insectSwarm != null)
                    postEclipseCast.AllDamageModifier *= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                else if (postEclipseCast.Name == "SF" && moonfire != null)
                    postEclipseCast.CriticalChanceModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                DoMainNuke(character, calcs, ref postEclipseCast, spellPower, spellHit, spellCrit, spellHaste);
            }

            if (moonfire != null)
                DoDotSpell(character, calcs, ref moonfire, spellPower, spellHit, spellCrit, spellHaste);
            if (insectSwarm != null)
                DoDotSpell(character, calcs, ref insectSwarm, spellPower, spellHit, spellCrit, spellHaste);

            float eclipseProcChance = (spellCrit + preEclipseCast.CriticalChanceModifier) * spellHit * character.DruidTalents.Eclipse / 3.0f * (calcOpts.LunarEclipse ? 0.6f : 1.0f);
            float preEclipseCritRate = ((1 - character.DruidTalents.Eclipse / 3.0f * (calcOpts.LunarEclipse ? 0.6f : 1.0f)) * (spellCrit + preEclipseCast.CriticalChanceModifier) * spellHit) / (1 - eclipseProcChance);

            float expectedCastsToProc = 1.0f / eclipseProcChance;

            float timeToProc = preEclipseCast.CastTime * (expectedCastsToProc - 0.5f);

            float rotationLength = timeToProc + eclipseCooldown;

            float preEclipseTime = timeToProc + preEclipseCast.NGCastTime;
            float preEclipseDPS = preEclipseCast.DamagePerHit / preEclipseCast.CastTime;
            float preEclipseManaUsed = preEclipseCast.BaseManaCost / preEclipseCast.CastTime * preEclipseTime;
            float preEclipseManaGained = (preEclipseCast.BaseManaCost * omenOfClarityProcChance) + 
                ((spellCrit + preEclipseCast.CriticalChanceModifier) * spellHit * moonkinFormProc) + 
                (JoWProc / 4 * spellHit);

            // Subtract the instant cast time in the following situations:
            // 1) If insect swarm is ever cast during a rotation
            // 2) If the rotation is MF/X AND Moonfire is specified to have 100% uptime
            float eclipseTime = eclipseDuration - preEclipseCast.NGCastTime - 
                (insectSwarm != null ? insectSwarm.CastTime : 0.0f) -
                (SpellsUsed.Count == 2 && moonfire != null && calcOpts.MoonfireAlways ? moonfire.CastTime : 0.0f);
            float eclipseDPS = eclipseCast.DamagePerHit / eclipseCast.CastTime;
            float eclipseManaUsed = eclipseCast.BaseManaCost / eclipseCast.CastTime * eclipseTime;
            float eclipseManaGained = (eclipseCast.BaseManaCost * omenOfClarityProcChance) +
                ((spellCrit + eclipseCast.CriticalChanceModifier) * spellHit * moonkinFormProc) +
                (JoWProc / 4 * spellHit);

            // Subtract the instant cast time in the following situations:
            // 1) If insect swarm is ever cast during a rotation
            // 2) Once if moonfire is used in an MF/X rotation
            // 3) Twice if moonfire is specified with 100% uptime in an IS/MF/X rotation
            float postEclipseTime = eclipseCooldown - eclipseDuration -
                (insectSwarm != null ? insectSwarm.CastTime : 0.0f) -
                (moonfire != null ? moonfire.CastTime : 0.0f) -
                (SpellsUsed.Count == 3 && calcOpts.MoonfireAlways ? moonfire.CastTime : 0.0f);
            float postEclipseDPS = postEclipseCast.DamagePerHit / postEclipseCast.CastTime;
            float postEclipseManaUsed = postEclipseCast.BaseManaCost / postEclipseCast.CastTime * postEclipseTime;
            float postEclipseManaGained = (postEclipseCast.BaseManaCost * omenOfClarityProcChance) +
                ((spellCrit + postEclipseCast.CriticalChanceModifier) * spellHit * moonkinFormProc) +
                (JoWProc / 4 * spellHit);

            // Moonfire tick calculation:
            // Min(rotationLength, SFglyph + regMF + regMF) / tickLength if 100% uptime specified
            float moonfireTicks = moonfire != null ? (calcOpts.MoonfireAlways ? 
                (float)Math.Min(rotationLength, moonfire.DotEffect.Duration * 2 - (starfireGlyph ? 9.0f : 0.0f)) / moonfire.DotEffect.TickLength :
                moonfire.DotEffect.NumberOfTicks) : 0.0f;
            float insectSwarmTicks = insectSwarm != null ? insectSwarmCasts * insectSwarm.DotEffect.NumberOfTicks : 0.0f;

            float moonfireDamage = moonfire != null ? moonfireCasts * moonfire.DamagePerHit + (moonfire.DotEffect.DamagePerHit / moonfire.DotEffect.NumberOfTicks) * moonfireTicks : 0.0f;
            float insectSwarmDamage = insectSwarm != null ? insectSwarmCasts * insectSwarm.DotEffect.DamagePerHit : 0.0f;

            float moonfireTime = moonfire != null ? moonfireCasts * moonfire.CastTime : 0.0f;
            float insectSwarmTime = insectSwarm != null ? insectSwarmCasts * insectSwarm.CastTime : 0.0f;

            float moonfireManaUsed = moonfire != null ? moonfireCasts * moonfire.BaseManaCost : 0.0f;
            float insectSwarmManaUsed = insectSwarm != null ? insectSwarmCasts * insectSwarm.BaseManaCost : 0.0f;

            float damageDone = preEclipseTime * preEclipseDPS + eclipseTime * eclipseDPS + postEclipseTime * postEclipseDPS +
                moonfireDamage + insectSwarmDamage;

            Duration = preEclipseTime + eclipseTime + postEclipseTime + moonfireTime + insectSwarmTime;
            DotTicks = moonfireTicks + insectSwarmTicks;
            InsectSwarmTicks = insectSwarmTicks;
            CastCount = expectedCastsToProc + (eclipseTime / eclipseCast.CastTime) + (postEclipseTime / postEclipseCast.CastTime);
            if (calcOpts.LunarEclipse)
            {
                StarfireCount = (eclipseTime / eclipseCast.CastTime) + (postEclipseCast.Name == "SF" ? postEclipseTime / postEclipseCast.CastTime : 0.0f);
                WrathCount = (preEclipseTime / preEclipseCast.CastTime) + (postEclipseCast.Name == "W" ? postEclipseTime / postEclipseCast.CastTime : 0.0f);
            }
            else
            {
                WrathCount = (eclipseTime / eclipseCast.CastTime) + (postEclipseCast.Name == "W" ? postEclipseTime / postEclipseCast.CastTime : 0.0f);
                StarfireCount = (preEclipseTime / preEclipseCast.CastTime) + (postEclipseCast.Name == "SF" ? postEclipseTime / postEclipseCast.CastTime : 0.0f);
            }
            ManaUsed = preEclipseManaUsed + eclipseManaUsed + postEclipseManaUsed + moonfireManaUsed + insectSwarmManaUsed;
            ManaGained = expectedCastsToProc * preEclipseManaGained + (eclipseTime / eclipseCast.CastTime) * eclipseManaGained + (postEclipseTime / postEclipseCast.CastTime) * postEclipseManaGained;

            float mfSavingsFromOoC = moonfire != null ? (moonfire.BaseManaCost - (moonfire.BaseManaCost *
                (1 - StarfireCount / WrathCount * 0.06f - (1 - StarfireCount / WrathCount) * 0.06f))) : 0.0f;
            float isSavingsFromOoC = insectSwarm != null ? (insectSwarm.BaseManaCost - (insectSwarm.BaseManaCost *
                (1 - StarfireCount / WrathCount * 0.06f - (1 - StarfireCount / WrathCount) * 0.06f))) : 0.0f;
            ManaGained += moonfire != null ? (moonfireCasts * (mfSavingsFromOoC + ((spellCrit + moonfire.CriticalChanceModifier) * moonkinFormProc * spellHit) + JoWProc * spellHit / 4.0f)) : 0.0f;
            ManaGained += insectSwarm != null ? (insectSwarmCasts * (isSavingsFromOoC + JoWProc * spellHit / 4.0f)) : 0.0f;

            RotationData.ManaGained = ManaGained;
            RotationData.DPM = damageDone / ManaUsed;
            ManaUsed -= ManaGained;
            RotationData.ManaUsed = ManaUsed;

            // Undo SF glyph
            if (starfireGlyph && moonfire != null) moonfire.DotEffect.Duration -= 9.0f;

            // Undo improved Insect Swarm
            if (preEclipseCast.Name == "W" && insectSwarm != null)
                preEclipseCast.AllDamageModifier /= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
            else if (preEclipseCast.Name == "SF" && moonfire != null)
                preEclipseCast.CriticalChanceModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
            if (preEclipseCast != postEclipseCast)
            {
                if (postEclipseCast.Name == "W" && insectSwarm != null)
                    postEclipseCast.AllDamageModifier /= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                else if (postEclipseCast.Name == "SF" && moonfire != null)
                    postEclipseCast.CriticalChanceModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
            }

            return damageDone;
        }
    }

    // The interface class to the rest of Rawr.  Provides a single Solve method that runs all the calculations.
    class MoonkinSolver
    {
        // A list of all currently active proc effects.
        public List<ProcEffect> procEffects;
        // A list of all the damage spells
        private Spell[] _spellData = null;
        public Spell[] SpellData
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
                                    SpellDamageModifier = (12f / 15f) * (12f / 15f) / (1.5f / 3.5f + 12f / 15f)
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
                                SpellDamageModifier = 1.2f
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

        public float NaturesGrace = 0.0f;

        // The spell rotations themselves.
        public List<SpellRotation> rotations = null;

        // Results data from the calculations, which will be sent to the UI.
        Dictionary<string, RotationData> cachedResults = new Dictionary<string, RotationData>();

        public void Solve(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            procEffects = new List<ProcEffect>();
            RecreateSpells(character, ref calcs);
            cachedResults = new Dictionary<string, RotationData>();

            float trinketDPS = 0.0f;
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
            float baseSpellPower = calcs.SpellPower;
            float baseCrit = calcs.SpellCrit;
            float baseHaste = calcs.SpellHaste;
            DoOnUseTrinketCalcs(calcs, baseHit, ref baseSpellPower, ref baseCrit, ref baseHaste, ref trinketDPS);
            BuildProcList(calcs);

            float maxDamageDone = 0.0f;
            float maxBurstDamageDone = 0.0f;
            SpellRotation maxBurstRotation = rotations[0];
            SpellRotation maxRotation = rotations[0];

            float manaPool = GetEffectiveManaPool(character, calcs);
            float manaGained = manaPool - calcs.BasicStats.Mana;

            // Do tree calculations: Calculate damage per cast.
            float treeDamage = (character.DruidTalents.ForceOfNature == 1) ? DoTreeCalcs(baseSpellPower, calcs.BasicStats.PhysicalHaste, calcs.BasicStats.ArmorPenetration, calcs.BasicStats.PhysicalCrit, calcs.BasicStats.Bloodlust, calcOpts.TreantLifespan, character.DruidTalents.Brambles) : 0.0f;
            // Extend that to number of casts per fight.  Round down and ensure that only complete tree casts are counted.
            treeDamage *= (int)Math.Floor(calcOpts.FightLength / 3.5f) + 1.0f;
            // Multiply by raid-wide damage increases.
            treeDamage *= (1 + calcs.BasicStats.BonusDamageMultiplier) * (1 + calcs.BasicStats.BonusPhysicalDamageMultiplier);
            // Calculate the DPS averaged over the fight length.
            float treeDPS = treeDamage / (calcOpts.FightLength * 60.0f);

			// Do Starfall calculations.
            bool starfallGlyph = calcOpts.glyph1 == "Starfall" || calcOpts.glyph2 == "Starfall" || calcOpts.glyph3 == "Starfall";
			float starfallDamage = (character.DruidTalents.Starfall == 1) ? DoStarfallCalcs(baseSpellPower, baseHit, baseCrit, Wrath.CriticalDamageModifier) : 0.0f;
            float starfallCD = 1.5f - (starfallGlyph ? 0.5f : 0.0f);
            float starfallDivisor = starfallCD + 1.0f / 6.0f;
            float numStarfallCasts = (float)Math.Floor(calcOpts.FightLength / starfallDivisor) + 1.0f;
            starfallDamage *= numStarfallCasts;
			starfallDamage *= (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
			float starfallDPS = starfallDamage / (calcOpts.FightLength * 60.0f);

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
                            rot.DamageDone(character, calcs, baseSpellPower, baseHit, baseCrit, baseHaste);
                        accumulatedDamage += proc.CalculateDPS(rot, calcs, baseSpellPower, baseHit, baseCrit, baseHaste) * rot.Duration;
                    }
                    else if (proc.Activate != null)
                    {
                        activatedEffects.Add(proc);
                    }
                    if (proc.CalculateMP5 != null)
                    {
                        manaGained += proc.CalculateMP5(rot, calcs, baseSpellPower, baseHit, baseCrit, baseHaste) / 5.0f * calcs.FightLength * 60.0f;
                    }
                }
                // Calculate stat-boosting trinkets, taking into effect interactions with other stat-boosting procs
                int sign = 1;
                Dictionary<List<int>, float> cachedDamages = new Dictionary<List<int>, float>();
                Dictionary<List<int>, float> cachedUptimes = new Dictionary<List<int>, float>();
                for (int i = 1; i <= activatedEffects.Count; ++i)
                {
                    CombinationGenerator gen = new CombinationGenerator(activatedEffects.Count, i);
                    while (gen.HasNext())
                    {
                        float tempUpTime = 1.0f;
                        int[] vals = gen.GetNext();
                        foreach (int idx in vals)
                        {
                            activatedEffects[idx].Activate(calcs, ref baseSpellPower, ref baseHit, ref baseCrit, ref baseHaste);
                        }
                        float tempDamage = rot.DamageDone(character, calcs, baseSpellPower, baseHit, baseCrit, baseHaste);
                        foreach (int idx in vals)
                        {
                            tempUpTime *= activatedEffects[idx].UpTime(rot, calcs);
                            activatedEffects[idx].Deactivate(calcs, ref baseSpellPower, ref baseHit, ref baseCrit, ref baseHaste);
                        }
                        List<int> pairs = new List<int>(vals);
                        cachedUptimes[pairs] = tempUpTime;
                        cachedDamages[pairs] = tempDamage;
                        totalUpTime += sign * tempUpTime;
                    }
                    sign = -sign;
                }
                // Iterate through the probability table and adjust probabilities relative to each other
                // This accomplishes the effect of finding the probability that any proc or combination of procs,
                // and ONLY that particular set of procs, will be active at a given time.
                List<List<int>> keys = new List<List<int>>(cachedUptimes.Keys);
                foreach (List<int> vals in keys)
                {
                    int newSign = 1;
                    int keyCount = vals.Count;
                    foreach (List<int> innerVals in keys)
                    {
                        if (innerVals == vals) continue;
                        if (innerVals.Count > keyCount)
                        {
                            keyCount = innerVals.Count;
                            newSign = -newSign;
                        }
                        if (vals.TrueForAll(delegate(int val)
                        {
                            return innerVals.Contains(val);
                        }))
                        {
                            cachedUptimes[vals] += newSign * cachedUptimes[innerVals];
                        }
                    }
                }
                // Apply the above-calculated probabilities to the previously stored damage calculations and add to the result.
                foreach (KeyValuePair<List<int>, float> kvp in cachedUptimes)
                {
                    accumulatedDamage += kvp.Value * cachedDamages[kvp.Key];
                }
                accumulatedDamage += (1 - totalUpTime) * rot.DamageDone(character, calcs, baseSpellPower, baseHit, baseCrit, baseHaste);
                float burstDPS = accumulatedDamage / rot.Duration;
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
                if ((calcOpts.userRotation == "None" && sustainedDPS > maxDamageDone) || calcOpts.userRotation == rot.Name)
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
            // Sundial of the Exiled, et al. (Spell power 10 sec on hit, 10% chance, 45s cooldown)
            if (calcs.BasicStats.SpellPowerFor10SecOnHit_10_45 > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    Activate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sp += c.BasicStats.SpellPowerFor10SecOnHit_10_45;
                    },
                    Deactivate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sp -= c.BasicStats.SpellPowerFor10SecOnHit_10_45;
                    },
                    UpTime = delegate(SpellRotation r, CharacterCalculationsMoonkin c)
                    {
                        float timeToProc = r.Duration / (r.CastCount * 0.1f) + 45.0f;
                        return 10.0f / (10.0f + timeToProc);
                    }
                });
            }
            // Dying Curse, et al. (Spell power for 10 sec on cast, 15% chance, 45s cooldown)
            if (calcs.BasicStats.SpellPowerFor10SecOnCast_15_45 > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    Activate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sp += c.BasicStats.SpellPowerFor10SecOnCast_15_45;
                    },
                    Deactivate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sp -= c.BasicStats.SpellPowerFor10SecOnCast_15_45;
                    },
                    UpTime = delegate(SpellRotation r, CharacterCalculationsMoonkin c)
                    {
                        float timeToProc = r.Duration / (r.CastCount * 0.15f) + 45.0f;
                        return 10.0f / (10.0f + timeToProc);
                    }
                });
            }
            // Flow of Knowledge, et al. (Spell power for 10 sec on cast, 10% chance, 45s cooldown)
            if (calcs.BasicStats.SpellPowerFor10SecOnCast_10_45 > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    Activate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sp += c.BasicStats.SpellPowerFor10SecOnCast_10_45;
                    },
                    Deactivate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sp -= c.BasicStats.SpellPowerFor10SecOnCast_10_45;
                    },
                    UpTime = delegate(SpellRotation r, CharacterCalculationsMoonkin c)
                    {
                        float timeToProc = r.Duration / (r.CastCount * 0.10f) + 45.0f;
                        return 10.0f / (10.0f + timeToProc);
                    }
                });
            }
            // Embrace of the Spider (505 haste/10 sec, 45s internal cooldown, 10% chance on cast)
            if (calcs.BasicStats.SpellHasteFor10SecOnCast_10_45 > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    Activate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sHa += c.BasicStats.SpellHasteFor10SecOnCast_10_45 / CalculationsMoonkin.hasteRatingConversionFactor;
                    },
                    Deactivate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sHa -= c.BasicStats.SpellHasteFor10SecOnCast_10_45 / CalculationsMoonkin.hasteRatingConversionFactor;
                    },
                    UpTime = delegate(SpellRotation r, CharacterCalculationsMoonkin c)
                    {
                        float timeToProc = (r.Duration / r.CastCount) / 0.1f + 45.0f;
                        return 10.0f / (10.0f + timeToProc);
                    }
                });
            }
            // Thunder Capacitor (2.5s cooldown after a proc, 4 charges/proc)
            if (calcs.BasicStats.ThunderCapacitorProc > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                    {
                        float specialDamageModifier = (1 + c.BasicStats.BonusSpellPowerMultiplier) * (1 + c.BasicStats.BonusNatureDamageMultiplier) * (1 + c.BasicStats.BonusDamageMultiplier);
                        float baseDamage = (1181 + 1371) / 2.0f;
                        float averageDamage = sHi * baseDamage * (1 + 0.5f * sc) * specialDamageModifier;
                        float timeBetweenProcs = r.Duration / (sHi * sc * r.CastCount);
                        if (timeBetweenProcs < 2.5f) timeBetweenProcs = timeBetweenProcs * 4.0f + 2.5f;
                        else timeBetweenProcs *= 4.0f;
                        return averageDamage / timeBetweenProcs;
                    }
                });
            }
            // Pendulum of Telluric Currents (15% chance on spell hit, 45s internal cooldown)
            if (calcs.BasicStats.PendulumOfTelluricCurrentsProc > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                    {
                        float specialDamageModifier = (1 + c.BasicStats.BonusSpellPowerMultiplier) * (1 + c.BasicStats.BonusShadowDamageMultiplier) * (1 + c.BasicStats.BonusDamageMultiplier);
                        float baseDamage = (1168 + 1752) / 2.0f;
                        float averageDamage = sHi * baseDamage * (1 + 0.5f * sc) * specialDamageModifier;
                        float timeBetweenProcs = r.Duration / (sHi * r.CastCount * 0.15f) + 45f;
                        return averageDamage / timeBetweenProcs;
                    }
                });
            }
            // Extract of Necromantic Power (10% proc on a DoT tick, 15s internal cooldown)
            if (calcs.BasicStats.ExtractOfNecromanticPowerProc > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                    {
                        if (r.DotTicks == 0) return 0.0f;
                        float specialDamageModifier = (1 + c.BasicStats.BonusShadowDamageMultiplier) * (1 + c.BasicStats.BonusSpellPowerMultiplier) * (1 + c.BasicStats.BonusDamageMultiplier);
                        float baseDamage = (788 + 1312) / 2.0f;
                        float averageDamage = sHi * baseDamage * (1 + 0.5f * sc) * specialDamageModifier;
                        float timeBetweenProcs = 1 / (r.DotTicks / r.Duration * 0.1f) + 15.0f;
                        return averageDamage / timeBetweenProcs;
                    }
                });
            }
            // Darkmoon Card: Death (35% proc from any damage source, 45s internal cooldown)
            if (calcs.BasicStats.DarkmoonCardDeathProc > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    CalculateDPS = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                    {
                        float specialDamageModifier = (1 + c.BasicStats.BonusSpellPowerMultiplier) * (1 + c.BasicStats.BonusShadowDamageMultiplier) * (1 + c.BasicStats.BonusDamageMultiplier);
                        float baseDamage = (744 + 956) / 2.0f;
                        float averageDamage = sHi * baseDamage * (1 + 0.5f * sc) * specialDamageModifier;
                        float timeBetweenProcs = r.Duration / ((r.CastCount + r.DotTicks) * 0.35f) + 45.0f;
                        return averageDamage / timeBetweenProcs;
                    }
                });
            }
            // Je'Tze's Bell (10% chance of 100 mp5 for 15 sec)
            if (calcs.BasicStats.ManaRestoreOnCast_10_45 > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    CalculateMP5 = delegate(SpellRotation r, CharacterCalculationsMoonkin c, float sp, float sHi, float sc, float sHa)
                    {
                        float procsPerRotation = 0.1f * r.CastCount;
                        float timeBetweenProcs = r.Duration / procsPerRotation + 45.0f;
                        return 300.0f / timeBetweenProcs * 5.0f;
                    }
                });
            }
            // Moonkin 4T8 set bonus (?? chance on IS tick to proc an instant-cast Starfire)
            if (calcs.BasicStats.StarfireProc > 0)
            {
                procEffects.Add(new ProcEffect()
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
                        float timeBetweenProcs = r.Solver.InsectSwarm.DotEffect.TickLength / 0.03f;
                        float replaceWrathWithSFDPS = (newSF.DamagePerHit / newSF.CastTime) - (r.Solver.Wrath.DamagePerHit / r.Solver.Wrath.CastTime);
                        float replaceSFWithSFDPS = (newSF.DamagePerHit / newSF.CastTime) - (r.Solver.Starfire.DamagePerHit / r.Solver.Starfire.CastTime);
                        return (replaceWrathWithSFDPS * (r.WrathCount / (r.WrathCount + r.StarfireCount)) + 
                            replaceSFWithSFDPS * (r.StarfireCount / (r.WrathCount + r.StarfireCount)))
                            / timeBetweenProcs;
                    }
                });
            }
            // Nature's Grace (33/66/100% chance of 20% haste for 3 sec on crit)
            /*if (NaturesGrace > 0)
            {
                procEffects.Add(new ProcEffect()
                {
                    Activate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sHa += 0.2f;
                    },
                    Deactivate = delegate(CharacterCalculationsMoonkin c, ref float sp, ref float sHi, ref float sc, ref float sHa)
                    {
                        sHa -= 0.2f;
                    },
                    UpTime = delegate(SpellRotation r, CharacterCalculationsMoonkin c)
                    {
                        return (1.0f - (float)Math.Pow(1.0f - c.SpellCrit, 3.0f / (r.Duration / r.CastCount))) * (r.Solver.NaturesGrace / 3.0f);
                    }
                });
            }*/
        }

        // Non-rotation-specific mana calculations
        private float GetEffectiveManaPool(Character character, CharacterCalculationsMoonkin calcs)
        {
            float fightLength = calcs.FightLength * 60.0f;

            float innervateCooldown = 360 - calcs.BasicStats.InnervateCooldownReduction;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen5SR * fightLength;

            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
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
            float totalInnervateMana = 0.0f;
            if (numInnervates > 0)
            {
                // Innervate mana rate increases only spirit-based regen
                float spiritRegen = (calcs.ManaRegen - calcs.BasicStats.Mp5 / 5f);
                // Add in calculations for an innervate weapon
                if (calcOpts.InnervateWeapon)
                {
                    float baseRegenConstant = CalculationsMoonkin.ManaRegenConstant;
                    // Calculate the intellect from a weapon swap
                    float userIntellect = calcs.BasicStats.Intellect - (character.MainHand == null ? 0 : character.MainHand.Item.Stats.Intellect) - (character.OffHand == null ? 0 : character.OffHand.Item.Stats.Intellect)
                        + calcOpts.InnervateWeaponInt;
                    // Do the same with spirit
                    float userSpirit = calcs.BasicStats.Spirit - (character.MainHand == null ? 0 : character.MainHand.Item.Stats.Spirit) - (character.OffHand == null ? 0 : character.OffHand.Item.Stats.Spirit)
                        + calcOpts.InnervateWeaponSpi;
                    // The new spirit regen for innervate periods uses the new weapon stats
                    spiritRegen = baseRegenConstant * (float)Math.Sqrt(userIntellect) * userSpirit;
                }
                float innervateManaRate = spiritRegen * 4 + calcs.BasicStats.Mp5 / 5f;
                float innervateTime = numInnervates * 20.0f;
                totalInnervateMana = innervateManaRate * innervateTime;
            }
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
            float bossArmor = 10645f * (1.0f - armorPen);
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

        // Clicky trinket calculations
        private void DoOnUseTrinketCalcs(CharacterCalculationsMoonkin calcs, float hitRate, ref float spellPower, ref float effectiveSpellCrit, ref float effectiveSpellHaste, ref float trinketExtraDPS)
        {
            // Shatterered Sun Pendant (45s internal CD)
            if (calcs.BasicStats.ShatteredSunAcumenProc > 0)
            {
                if (calcs.Scryer)
                {
                    float AllDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
                    float baseDamage = (333 + 367) / 2.0f;
                    float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * AllDamageModifier;
                    trinketExtraDPS += averageDamage / 45.0f;
                }
                else
                {
                    spellPower += 120.0f * 10.0f / 45.0f;
                }
            }
            // Haste trinkets
            if (calcs.BasicStats.HasteRatingFor20SecOnUse2Min > 0)
            {
                effectiveSpellHaste += calcs.BasicStats.HasteRatingFor20SecOnUse2Min * 20.0f / 120.0f / CalculationsMoonkin.hasteRatingConversionFactor;
            }
            // Spell damage trinkets
            if (calcs.BasicStats.SpellPowerFor15SecOnUse90Sec > 0)
            {
                spellPower += calcs.BasicStats.SpellPowerFor15SecOnUse90Sec * 15.0f / 90.0f;
            }
            if (calcs.BasicStats.SpellPowerFor20SecOnUse2Min > 0)
            {
                spellPower += calcs.BasicStats.SpellPowerFor20SecOnUse2Min * 20.0f / 120.0f;
            }
            if (calcs.BasicStats.SpellPowerFor20SecOnUse5Min > 0)
            {
                spellPower += calcs.BasicStats.SpellPowerFor20SecOnUse5Min * 20.0f / 300.0f;
            }
        }

        // Redo the spell calculations
        private void RecreateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            ResetSpellList();
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
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

            UpdateSpells(character, ref calcs);
        }

        // Add talented effects to the spells
        private void UpdateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            Stats stats = calcs.BasicStats;
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // Add (possibly talented) +spelldmg
            // Starfire: Damage +(0.04 * Wrath of Cenarius)
            // Wrath: Damage +(0.02 * Wrath of Cenarius)
            Wrath.SpellDamageModifier += 0.02f * character.DruidTalents.WrathOfCenarius;
            Starfire.SpellDamageModifier += 0.04f * character.DruidTalents.WrathOfCenarius;

            // Add spell damage from idols
            Starfire.IdolExtraSpellPower += stats.StarfireDmg;
            Moonfire.IdolExtraSpellPower += stats.MoonfireDmg;
            Wrath.BaseDamage += stats.WrathDmg;
            InsectSwarm.IdolExtraSpellPower += stats.InsectSwarmDmg;

            float moonfireDDGlyph = (calcOpts.glyph1 == "Moonfire" || calcOpts.glyph2 == "Moonfire" || calcOpts.glyph3 == "Moonfire") ? -0.9f : 0.0f;
            float moonfireDotGlyph = (calcOpts.glyph1 == "Moonfire" || calcOpts.glyph2 == "Moonfire" || calcOpts.glyph3 == "Moonfire") ? 0.75f : 0.0f;
            float insectSwarmGlyph = (calcOpts.glyph1 == "Insect Swarm" || calcOpts.glyph2 == "Insect Swarm" || calcOpts.glyph3 == "Insect Swarm") ? 0.3f : 0.0f;
            // Add spell-specific damage
            // Starfire, Moonfire, Wrath: Damage +(0.03 * Moonfury)
            // Moonfire: Damage +(0.05 * Imp Moonfire) (Additive with Moonfury/Genesis/Glyph)
            // Moonfire, Insect Swarm: Dot Damage +(0.01 * Genesis) (Additive with Moonfury/Imp. Moonfire/Glyph/Set bonus)
            Wrath.AllDamageModifier *= 1 + (float)Math.Floor(character.DruidTalents.Moonfury * 10 / 3.0f) / 100.0f;
            Moonfire.AllDamageModifier *= 1 + (float)Math.Floor(character.DruidTalents.Moonfury * 10 / 3.0f) / 100.0f
                                            + 0.05f * character.DruidTalents.ImprovedMoonfire
                                            + moonfireDDGlyph;
            Moonfire.DotEffect.AllDamageModifier *= 1 + (float)Math.Floor(character.DruidTalents.Moonfury * 10 / 3.0f) / 100.0f
                                                      + 0.01f * character.DruidTalents.Genesis
                                                      + 0.05f * character.DruidTalents.ImprovedMoonfire
                                                      + moonfireDotGlyph;
            Starfire.AllDamageModifier *= 1 + (float)Math.Floor(character.DruidTalents.Moonfury * 10 / 3.0f) / 100.0f;
            InsectSwarm.DotEffect.AllDamageModifier *= 1 + 0.01f * character.DruidTalents.Genesis
                                                         + insectSwarmGlyph
                                                         + stats.BonusInsectSwarmDamage;

            // Moonfire, Insect Swarm: One extra tick (Nature's Splendor)
            Moonfire.DotEffect.Duration += 3.0f * character.DruidTalents.NaturesSplendor;
            InsectSwarm.DotEffect.Duration += 2.0f * character.DruidTalents.NaturesSplendor;
            // Moonfire: Crit chance +(0.05 * Imp Moonfire)
            Moonfire.CriticalChanceModifier += 0.05f * character.DruidTalents.ImprovedMoonfire;

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
            Wrath.CriticalChanceModifier += 0.02f * character.DruidTalents.NaturesMajesty;
            Starfire.CriticalChanceModifier += 0.02f * character.DruidTalents.NaturesMajesty;

            // Add spell-specific critical strike damage
            // Chaotic Skyfire Diamond
            Starfire.CriticalDamageModifier = stats.BonusCritMultiplier > 0 ? 1.5f * (1+stats.BonusCritMultiplier) : 1.5f;
            Wrath.CriticalDamageModifier = stats.BonusCritMultiplier > 0 ? 1.5f * (1 + stats.BonusCritMultiplier) : 1.5f;
            Moonfire.CriticalDamageModifier = stats.BonusCritMultiplier > 0 ? 1.5f * (1 + stats.BonusCritMultiplier) : 1.5f;
            // Starfire, Moonfire, Wrath: Crit damage +(0.2 * Vengeance)
            Starfire.CriticalDamageModifier = (Starfire.CriticalDamageModifier - 1.0f) * (1 + 0.2f * character.DruidTalents.Vengeance) + 1.0f;
            Wrath.CriticalDamageModifier = (Wrath.CriticalDamageModifier - 1.0f) * (1 + 0.2f * character.DruidTalents.Vengeance) + 1.0f;
            Moonfire.CriticalDamageModifier = (Moonfire.CriticalDamageModifier - 1.0f) * (1 + 0.2f * character.DruidTalents.Vengeance) + 1.0f;

            // Reduce spell-specific mana costs
            // Starfire, Moonfire, Wrath: Mana cost -(0.03 * Moonglow)
            Starfire.BaseManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);
            Moonfire.BaseManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);
            Wrath.BaseManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);

            // Add set bonuses
            Moonfire.DotEffect.Duration += stats.MoonfireExtension;
            Starfire.CriticalChanceModifier += stats.StarfireCritChance;
            Starfire.CriticalChanceModifier += stats.BonusNukeCritChance;
            Wrath.CriticalChanceModifier += stats.BonusNukeCritChance;

            // Nature's Grace
            NaturesGrace = character.DruidTalents.NaturesGrace;
        }

    }
}
