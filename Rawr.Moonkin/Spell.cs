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
        public float ManaCost { get; set; }
    }
    class DotEffect
    {
        public DotEffect() { AllDamageModifier = 1.0f; }
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

    // Our old friend the spell rotation.
    class SpellRotation
    {
        public static float NGReduction { get; set; }
        private Spell LocateSpell(string name)
        {
            return Array.Find<Spell>(MoonkinSolver.SpellData, delegate(Spell sp) { return sp.Name == name; });
        }
        public List<string> SpellsUsed;
        public RotationData RotationData = new RotationData();
        public string Name { get; set; }
        public float Duration { get; set; }
        public float ManaUsed { get; set; }
        public float ManaGained { get; set; }
        public float CastCount { get; set; }
        public float DotTicks { get; set; }

        // Calculate damage and casting time for a single, direct-damage spell.
        private void DoMainNuke(Character character, CharacterCalculationsMoonkin calcs, ref Spell mainNuke, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;

            mainNuke.CastTime = mainNuke.BaseCastTime - 0.1f * character.DruidTalents.StarlightWrath;
            float totalCritChance = spellCrit + mainNuke.CriticalChanceModifier;
            float baseCastTime = (float)Math.Max(1.0f, mainNuke.CastTime / (1 + spellHaste)) + latency;
            float NGCastTime = (float)Math.Max(1.0f, (mainNuke.CastTime - NGReduction) / (1 + spellHaste)) + latency;
            if (mainNuke.Name == "W")
                NGCastTime += latency;    // Additional penalty for clipping GCD
            mainNuke.CastTime = totalCritChance * NGCastTime + (1 - totalCritChance) * baseCastTime;
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
            dotSpell.CastTime = Math.Max(dotSpell.BaseCastTime / (1 + spellHaste), 1.0f) + 2 * latency;
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
            dotSpell.CastTime = Math.Max(dotSpell.BaseCastTime / (1 + spellHaste), 1.0f) + 2 * latency;
            float damagePerTick = (dotSpell.DotEffect.TickDamage + dotSpell.DotEffect.SpellDamageModifierPerTick * spellPower) * dotSpell.DotEffect.AllDamageModifier;
            dotSpell.DotEffect.DamagePerHit = dotSpell.DotEffect.NumberOfTicks * damagePerTick * spellHit;
        }

        // Perform damage and mana calculations for all spells in the given rotation.  Returns damage done over the total duration.
        public float DamageDone(Character character, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            // No support for 1 dot + Eclipse rotations
            if (character.DruidTalents.Eclipse > 0 && (SpellsUsed.Count == 1 || SpellsUsed.Count == 3))
            {
                return DoEclipseCalcs(character, calcs, spellPower, spellHit, spellCrit, spellHaste);
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
                    Spell mainNuke = LocateSpell(SpellsUsed[0]);
                    DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

                    float omenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * mainNuke.BaseCastTime : 0;
                    mainNuke.ManaCost = mainNuke.BaseManaCost - 0.25f * JoWProc * spellHit - (spellCrit + mainNuke.CriticalChanceModifier) * moonkinFormProc - mainNuke.BaseManaCost * omenProcChance * spellHit;
                    Duration = mainNuke.CastTime;
                    RotationData.ManaUsed = ManaUsed = mainNuke.ManaCost;
                    RotationData.ManaGained = ManaGained = mainNuke.BaseManaCost - mainNuke.ManaCost;
                    RotationData.DPM = mainNuke.DamagePerHit / mainNuke.ManaCost;
                    CastCount = 1.0f;
                    DotTicks = 0.0f;

                    return mainNuke.DamagePerHit;
                // Nuke + 1 DotEffect
                case 2:
                    // Find the spells
                    Spell DotEffectSpell = LocateSpell(SpellsUsed[0]);
                    mainNuke = LocateSpell(SpellsUsed[1]);
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
                    DotTicks = DotEffectSpell.DotEffect.NumberOfTicks;

                    float manaFromJoW = (numNukeCasts + 1) / 4 * JoWProc * spellHit;
                    float manaFromOoC = ((3.5f / 60.0f * 1.5f) * mainNuke.BaseManaCost
                        + (numNukeCasts - 1) * (3.5f / 60.0f * mainNuke.BaseCastTime) * mainNuke.BaseManaCost
                        + (3.5f / 60.0f * 3.5f) * DotEffectSpell.BaseManaCost) * spellHit;
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
                    Spell moonFire = LocateSpell(SpellsUsed[0]);
                    Spell insectSwarm = LocateSpell(SpellsUsed[1]);
                    mainNuke = LocateSpell(SpellsUsed[2]);
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
                    DotTicks = moonFire.DotEffect.NumberOfTicks + numISCasts * insectSwarm.DotEffect.NumberOfTicks;

                    manaFromJoW = (numNukeCasts + 1 + numISCasts) / 4 * JoWProc * spellHit;
                    manaFromOoC = ((3.5f / 60.0f * 1.5f) * mainNuke.BaseManaCost
                        + (numNukeCasts - 1 - numISCasts) * (3.5f / 60.0f * mainNuke.BaseCastTime) * mainNuke.BaseManaCost
                        + (3.5f / 60.0f * mainNuke.BaseCastTime) * moonFire.BaseManaCost
                        + (3.5f / 60.0f * mainNuke.BaseCastTime) * numISCasts * insectSwarm.BaseManaCost
                        + (3.5f / 60.0f * 1.5f) * numISCasts * mainNuke.BaseManaCost) * spellHit;
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

        private float DoEclipseCalcs(Character character, CharacterCalculationsMoonkin calcs, float spellPower, float spellHit, float spellCrit, float spellHaste)
        {
            float latency = calcs.Latency;

            float JoWProc = character.ActiveBuffsContains("Judgement of Wisdom") ? 0.02f * CalculationsMoonkin.BaseMana : 0.0f;
            float moonkinFormProc = (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm == 1) ? 0.02f * calcs.BasicStats.Mana : 0.0f;
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            bool starfireGlyph = calcOpts.glyph1 == "Starfire" || calcOpts.glyph2 == "Starfire" || calcOpts.glyph3 == "Starfire";

            switch (SpellsUsed.Count)
            {
                // Nuke only
                case 1:
                    Spell mainNuke = LocateSpell(SpellsUsed[0]);
                    Spell offNuke = LocateSpell(SpellsUsed[0] == "SF" ? "W" : "SF");
                    DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);
                    DoMainNuke(character, calcs, ref offNuke, spellPower, spellHit, spellCrit, spellHaste);
                    float preEclipseTime = 0.0f;
                    float preEclipseDPS = 0.0f;
                    float preEclipseManaUsed = 0.0f;
                    float preEclipseManaGained = 0.0f;
                    float eclipseTime = 0.0f;
                    float eclipseDPS = 0.0f;
                    float eclipseManaUsed = 0.0f;
                    float eclipseManaGained = 0.0f;
                    float postEclipseTime = 0.0f;
                    float postEclipseDPS = 0.0f;
                    float postEclipseManaUsed = 0.0f;
                    float postEclipseManaGained = 0.0f;
                    float timeToProc = 0.0f;
                    if (calcOpts.SmartSwitching)
                    {
                        float preEclipseOmenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * offNuke.BaseCastTime : 0;
                        float eclipseOmenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * mainNuke.BaseCastTime : 0;

                        float eclipseProcChance = (spellCrit + offNuke.CriticalChanceModifier) * spellHit * (offNuke.Name == "W" ? 0.2f * character.DruidTalents.Eclipse : character.DruidTalents.Eclipse / 3.0f);
                        float effectiveCritRate = (1.0f - (offNuke.Name == "W" ? 0.2f * character.DruidTalents.Eclipse : character.DruidTalents.Eclipse / 3.0f)) * (spellCrit + offNuke.CriticalChanceModifier) * spellHit;
                        float castsToProc = 1.0f / eclipseProcChance;
                        float unhastedCastTime = offNuke.BaseCastTime - 0.1f * character.DruidTalents.StarlightWrath;
                        float baseCastTime = Math.Max(1.0f, unhastedCastTime / (1 + spellHaste)) + latency;
                        float NGCastTime = Math.Max(1.0f, (unhastedCastTime - NGReduction) / (1 + spellHaste)) + latency + (offNuke.Name == "W" ? latency : 0.0f);
                        float effectiveCastTime = baseCastTime * (1 - effectiveCritRate) + NGCastTime * effectiveCritRate;
                        timeToProc = effectiveCastTime * (castsToProc - 0.5f);
                        float rotationLength = 30.0f + timeToProc;

                        if (mainNuke.Name == "W") mainNuke.AllDamageModifier *= 1.2f;
                        DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit + (mainNuke.Name == "SF" ? 0.3f : 0.0f), spellHaste);
                        if (mainNuke.Name == "W") mainNuke.AllDamageModifier /= 1.2f;
                        float eclipseCastTime = mainNuke.CastTime;

                        preEclipseTime = timeToProc + NGCastTime;
                        preEclipseDPS = offNuke.DamagePerHit / effectiveCastTime;
                        preEclipseManaUsed = offNuke.BaseManaCost / effectiveCastTime * preEclipseTime;
                        preEclipseManaGained = (offNuke.BaseManaCost * preEclipseOmenProcChance) + ((spellCrit + offNuke.CriticalChanceModifier) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        eclipseTime = 15.0f - NGCastTime - 0.5f * mainNuke.CastTime;
                        eclipseDPS = mainNuke.DamagePerHit / mainNuke.CastTime;
                        eclipseManaUsed = mainNuke.BaseManaCost / mainNuke.CastTime * eclipseTime;
                        eclipseManaGained = (mainNuke.BaseManaCost * eclipseOmenProcChance) + ((spellCrit + mainNuke.CriticalChanceModifier + (mainNuke.Name == "SF" ? 0.3f : 0.0f)) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        postEclipseTime = 30.0f - 15.0f + 0.5f * eclipseCastTime;

                        DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

                        postEclipseDPS = mainNuke.DamagePerHit / mainNuke.CastTime;
                        postEclipseManaUsed = mainNuke.BaseManaCost / mainNuke.CastTime * postEclipseTime;
                        postEclipseManaGained = (mainNuke.BaseManaCost * eclipseOmenProcChance) + ((spellCrit + mainNuke.CriticalChanceModifier) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        Duration = preEclipseTime + eclipseTime + postEclipseTime;
                        DotTicks = 0.0f;
                        CastCount = castsToProc + (eclipseTime / eclipseCastTime) + (postEclipseTime / mainNuke.CastTime);
                        ManaUsed = RotationData.ManaUsed = preEclipseManaUsed + eclipseManaUsed + postEclipseManaUsed;
                        ManaGained = RotationData.ManaGained = (preEclipseManaGained / offNuke.CastTime * preEclipseTime) +
                            (eclipseManaGained / eclipseCastTime * eclipseTime) +
                            (postEclipseManaGained / mainNuke.CastTime * postEclipseTime);
                    }
                    else
                    {
                        float preEclipseOmenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * mainNuke.BaseCastTime : 0;
                        float eclipseOmenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * offNuke.BaseCastTime : 0;

                        float eclipseProcChance = (spellCrit + mainNuke.CriticalChanceModifier) * spellHit * (mainNuke.Name == "W" ? 0.2f * character.DruidTalents.Eclipse : character.DruidTalents.Eclipse / 3.0f);
                        float effectiveCritRate = (1.0f - (mainNuke.Name == "W" ? 0.2f * character.DruidTalents.Eclipse : character.DruidTalents.Eclipse / 3.0f)) * (spellCrit + mainNuke.CriticalChanceModifier) * spellHit;
                        float castsToProc = 1.0f / eclipseProcChance;
                        float unhastedCastTime = mainNuke.BaseCastTime - 0.1f * character.DruidTalents.StarlightWrath;
                        float baseCastTime = Math.Max(1.0f, unhastedCastTime / (1 + spellHaste)) + latency;
                        float NGCastTime = Math.Max(1.0f, (unhastedCastTime - NGReduction) / (1 + spellHaste)) + latency + (mainNuke.Name == "W" ? latency : 0.0f);
                        float effectiveCastTime = baseCastTime * (1 - effectiveCritRate) + NGCastTime * effectiveCritRate;
                        timeToProc = effectiveCastTime * (castsToProc - 0.5f);
                        float rotationLength = 30.0f + timeToProc;

                        if (offNuke.Name == "W") offNuke.AllDamageModifier *= 1.2f;
                        DoMainNuke(character, calcs, ref offNuke, spellPower, spellHit, spellCrit + (offNuke.Name == "SF" ? 0.3f : 0.0f), spellHaste);
                        if (offNuke.Name == "W") offNuke.AllDamageModifier /= 1.2f;

                        preEclipseTime = timeToProc + NGCastTime;
                        preEclipseDPS = mainNuke.DamagePerHit / effectiveCastTime;
                        preEclipseManaUsed = mainNuke.BaseManaCost / effectiveCastTime * preEclipseTime;
                        preEclipseManaGained = (mainNuke.BaseManaCost * preEclipseOmenProcChance) + ((spellCrit + mainNuke.CriticalChanceModifier) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        eclipseTime = 15.0f - NGCastTime - 0.5f * offNuke.CastTime;
                        eclipseDPS = offNuke.DamagePerHit / offNuke.CastTime;
                        eclipseManaUsed = offNuke.BaseManaCost / offNuke.CastTime * eclipseTime;
                        eclipseManaGained = (offNuke.BaseManaCost * eclipseOmenProcChance) + ((spellCrit + offNuke.CriticalChanceModifier + (offNuke.Name == "SF" ? 0.3f : 0.0f)) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        postEclipseTime = 30.0f - 15.0f + 0.5f * offNuke.CastTime;
                        postEclipseDPS = mainNuke.DamagePerHit / mainNuke.CastTime;
                        postEclipseManaUsed = mainNuke.BaseManaCost / mainNuke.CastTime * postEclipseTime;
                        postEclipseManaGained = (mainNuke.BaseManaCost * preEclipseOmenProcChance) + ((spellCrit + mainNuke.CriticalChanceModifier) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        Duration = preEclipseTime + eclipseTime + postEclipseTime;
                        DotTicks = 0.0f;
                        CastCount = castsToProc + (eclipseTime / offNuke.CastTime) + (postEclipseTime / mainNuke.CastTime);
                        ManaUsed = RotationData.ManaUsed = preEclipseManaUsed + eclipseManaUsed + postEclipseManaUsed;
                        ManaGained = RotationData.ManaGained = (preEclipseManaGained / mainNuke.CastTime * preEclipseTime) +
                            (eclipseManaGained / offNuke.CastTime * eclipseTime) +
                            (postEclipseManaGained / mainNuke.CastTime * postEclipseTime);

                        DoMainNuke(character, calcs, ref offNuke, spellPower, spellHit, spellCrit, spellHaste);
                    }
                    float damageDone = preEclipseDPS * preEclipseTime + eclipseDPS * eclipseTime + postEclipseDPS * postEclipseTime;
                    RotationData.DPM = damageDone / ManaUsed;
                    ManaUsed -= ManaGained;
                    RotationData.ManaUsed -= ManaGained;
                    return damageDone;
                // Nuke + both DotEffects
                case 3:
                    mainNuke = LocateSpell(SpellsUsed[2]);
                    offNuke = LocateSpell(SpellsUsed[2] == "SF" ? "W" : "SF");
                    Spell moonFire = LocateSpell("MF");
                    Spell insectSwarm = LocateSpell("IS");
                    // Do Starfire glyph calculations, if applicable; then do DoT spell calculations
                    if (starfireGlyph) moonFire.DotEffect.Duration += 9.0f;
                    // Do iIS calculations, if applicable
                    if (character.DruidTalents.ImprovedInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF")
                        {
                            mainNuke.CriticalChanceModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                            offNuke.AllDamageModifier *= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                        }
                        else if (mainNuke.Name == "W")
                        {
                            mainNuke.AllDamageModifier *= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                            offNuke.CriticalChanceModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                        }
                    }
                    DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);
                    DoMainNuke(character, calcs, ref offNuke, spellPower, spellHit, spellCrit, spellHaste);
                    DoDotSpell(character, calcs, ref moonFire, spellPower, spellHit, spellCrit, spellHaste);
                    DoDotSpell(character, calcs, ref insectSwarm, spellPower, spellHit, spellCrit, spellHaste);
                    float starfireOmenProcChance, wrathOmenProcChance;
                    preEclipseTime = 0.0f;
                    preEclipseDPS = 0.0f;
                    preEclipseManaUsed = 0.0f;
                    preEclipseManaGained = 0.0f;
                    eclipseTime = 0.0f;
                    eclipseDPS = 0.0f;
                    eclipseManaUsed = 0.0f;
                    eclipseManaGained = 0.0f;
                    postEclipseTime = 0.0f;
                    postEclipseDPS = 0.0f;
                    postEclipseManaUsed = 0.0f;
                    postEclipseManaGained = 0.0f;
                    timeToProc = 0.0f;
                    float moonfireCasts = 0.0f;
                    float insectSwarmCasts = 0.0f;
                    float ratioSFtoW = 0.0f;
                    if (calcOpts.SmartSwitching)
                    {
                        float preEclipseOmenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * offNuke.BaseCastTime : 0;
                        float eclipseOmenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * mainNuke.BaseCastTime : 0;

                        // Casting main-nuke/off-nuke into SF/W terms for calculation of MF/IS Omen of Clarity savings
                        if (mainNuke.Name == "SF")
                        {
                            starfireOmenProcChance = eclipseOmenProcChance;
                            wrathOmenProcChance = preEclipseOmenProcChance;
                        }
                        else
                        {
                            starfireOmenProcChance = preEclipseOmenProcChance;
                            wrathOmenProcChance = eclipseOmenProcChance;
                        }

                        float eclipseProcChance = (spellCrit + offNuke.CriticalChanceModifier) * spellHit * (offNuke.Name == "W" ? 0.2f * character.DruidTalents.Eclipse : character.DruidTalents.Eclipse / 3.0f);
                        float effectiveCritRate = (1.0f - (offNuke.Name == "W" ? 0.2f * character.DruidTalents.Eclipse : character.DruidTalents.Eclipse / 3.0f)) * (spellCrit + offNuke.CriticalChanceModifier) * spellHit;
                        float castsToProc = 1.0f / eclipseProcChance;
                        float unhastedCastTime = offNuke.BaseCastTime - 0.1f * character.DruidTalents.StarlightWrath;
                        float baseCastTime = Math.Max(1.0f, unhastedCastTime / (1 + spellHaste)) + latency;
                        float NGCastTime = Math.Max(1.0f, (unhastedCastTime - NGReduction) / (1 + spellHaste)) + latency + (offNuke.Name == "W" ? latency : 0.0f);
                        float effectiveCastTime = baseCastTime * (1 - effectiveCritRate) + NGCastTime * effectiveCritRate;
                        timeToProc = effectiveCastTime * (castsToProc - 0.5f);

                        moonfireCasts = (30.0f + timeToProc) / moonFire.DotEffect.Duration;
                        insectSwarmCasts = (30.0f + timeToProc) / insectSwarm.DotEffect.Duration;
                        float timeCastingDots = (moonfireCasts * moonFire.CastTime) + (insectSwarmCasts * insectSwarm.CastTime);

                        float rotationLength = 30.0f + timeToProc + (timeCastingDots * (timeToProc / (timeToProc + 30.0f)));

                        if (mainNuke.Name == "W") mainNuke.AllDamageModifier *= 1.2f;
                        DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit + (mainNuke.Name == "SF" ? 0.3f : 0.0f), spellHaste);
                        if (mainNuke.Name == "W") mainNuke.AllDamageModifier /= 1.2f;
                        float eclipseCastTime = mainNuke.CastTime;

                        preEclipseTime = timeToProc + NGCastTime;
                        preEclipseDPS = offNuke.DamagePerHit / effectiveCastTime;
                        preEclipseManaUsed = offNuke.BaseManaCost / effectiveCastTime * preEclipseTime;
                        preEclipseManaGained = (offNuke.BaseManaCost * preEclipseOmenProcChance) + ((spellCrit + offNuke.CriticalChanceModifier) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        eclipseTime = 15.0f - NGCastTime - 0.5f * mainNuke.CastTime - (timeCastingDots * (15.0f / (30.0f + timeToProc)));
                        eclipseDPS = mainNuke.DamagePerHit / mainNuke.CastTime;
                        eclipseManaUsed = mainNuke.BaseManaCost / mainNuke.CastTime * eclipseTime;
                        eclipseManaGained = (mainNuke.BaseManaCost * eclipseOmenProcChance) + ((spellCrit + mainNuke.CriticalChanceModifier + (mainNuke.Name == "SF" ? 0.3f : 0.0f)) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        DoMainNuke(character, calcs, ref mainNuke, spellPower, spellHit, spellCrit, spellHaste);

                        postEclipseTime = 30.0f - 15.0f + 0.5f * eclipseCastTime - (timeCastingDots * ((30.0f - 15.0f) / (30.0f + timeToProc)));
                        postEclipseDPS = mainNuke.DamagePerHit / mainNuke.CastTime;
                        postEclipseManaUsed = mainNuke.BaseManaCost / mainNuke.CastTime * postEclipseTime;
                        postEclipseManaGained = (mainNuke.BaseManaCost * eclipseOmenProcChance) + ((spellCrit + mainNuke.CriticalChanceModifier) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        Duration = preEclipseTime + eclipseTime + postEclipseTime + timeCastingDots;
                        CastCount = castsToProc + (eclipseTime / eclipseCastTime) + (postEclipseTime / mainNuke.CastTime);
                        ManaUsed = RotationData.ManaUsed = preEclipseManaUsed + eclipseManaUsed + postEclipseManaUsed;
                        ManaGained = RotationData.ManaGained = (preEclipseManaGained / offNuke.CastTime * preEclipseTime) +
                            (eclipseManaGained / eclipseCastTime * eclipseTime) +
                            (postEclipseManaGained / mainNuke.CastTime * postEclipseTime);

                        // Calculation of SF/W ratio changes depending on which is main nuke
                        if (mainNuke.Name == "SF")
                        {
                            ratioSFtoW = (eclipseTime / eclipseCastTime + postEclipseTime / mainNuke.CastTime) / (eclipseTime / eclipseCastTime + castsToProc + postEclipseTime / mainNuke.CastTime);
                        }
                        else
                        {
                            ratioSFtoW = castsToProc / (eclipseTime / eclipseCastTime + castsToProc + postEclipseTime / mainNuke.CastTime);
                        }
                    }
                    else
                    {
                        float preEclipseOmenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * mainNuke.BaseCastTime : 0;
                        float eclipseOmenProcChance = character.DruidTalents.OmenOfClarity == 1 ? 3.5f / 60f * offNuke.BaseCastTime : 0;

                        // Casting main-nuke/off-nuke into SF/W terms for calculation of MF/IS Omen of Clarity savings
                        if (mainNuke.Name == "SF")
                        {
                            starfireOmenProcChance = preEclipseOmenProcChance;
                            wrathOmenProcChance = eclipseOmenProcChance;
                        }
                        else
                        {
                            starfireOmenProcChance = eclipseOmenProcChance;
                            wrathOmenProcChance = preEclipseOmenProcChance;
                        }

                        float eclipseProcChance = (spellCrit + mainNuke.CriticalChanceModifier) * spellHit * (mainNuke.Name == "W" ? 0.2f * character.DruidTalents.Eclipse : character.DruidTalents.Eclipse / 3.0f);
                        float effectiveCritRate = (1.0f - (mainNuke.Name == "W" ? 0.2f * character.DruidTalents.Eclipse : character.DruidTalents.Eclipse / 3.0f)) * (spellCrit + mainNuke.CriticalChanceModifier) * spellHit;
                        float castsToProc = 1.0f / eclipseProcChance;
                        float unhastedCastTime = mainNuke.BaseCastTime - 0.1f * character.DruidTalents.StarlightWrath;
                        float baseCastTime = Math.Max(1.0f, unhastedCastTime / (1 + spellHaste)) + latency;
                        float NGCastTime = Math.Max(1.0f, (unhastedCastTime - NGReduction) / (1 + spellHaste)) + latency + (mainNuke.Name == "W" ? latency : 0.0f);
                        float effectiveCastTime = baseCastTime * (1 - effectiveCritRate) + NGCastTime * effectiveCritRate;
                        timeToProc = effectiveCastTime * (castsToProc - 0.5f);

                        moonfireCasts = (30.0f + timeToProc) / moonFire.DotEffect.Duration;
                        insectSwarmCasts = (30.0f + timeToProc) / insectSwarm.DotEffect.Duration;
                        float timeCastingDots = (moonfireCasts * moonFire.CastTime) + (insectSwarmCasts * insectSwarm.CastTime);

                        float rotationLength = 30.0f + timeToProc + (timeCastingDots * (timeToProc / (timeToProc + 30.0f)));

                        if (offNuke.Name == "W") offNuke.AllDamageModifier *= 1.2f;
                        DoMainNuke(character, calcs, ref offNuke, spellPower, spellHit, spellCrit + (offNuke.Name == "SF" ? 0.3f : 0.0f), spellHaste);
                        if (offNuke.Name == "W") offNuke.AllDamageModifier /= 1.2f;
                        float eclipseCastTime = offNuke.CastTime;

                        preEclipseTime = timeToProc + NGCastTime;
                        preEclipseDPS = mainNuke.DamagePerHit / effectiveCastTime;
                        preEclipseManaUsed = mainNuke.BaseManaCost / effectiveCastTime * preEclipseTime;
                        preEclipseManaGained = (mainNuke.BaseManaCost * preEclipseOmenProcChance) + ((spellCrit + mainNuke.CriticalChanceModifier) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        eclipseTime = 15.0f - NGCastTime - 0.5f * offNuke.CastTime - (timeCastingDots * (15.0f / (30.0f + timeToProc)));
                        eclipseDPS = offNuke.DamagePerHit / offNuke.CastTime;
                        eclipseManaUsed = offNuke.BaseManaCost / offNuke.CastTime * eclipseTime;
                        eclipseManaGained = (offNuke.BaseManaCost * eclipseOmenProcChance) + ((spellCrit + offNuke.CriticalChanceModifier + (offNuke.Name == "SF" ? 0.3f : 0.0f)) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        postEclipseTime = 30.0f - 15.0f + 0.5f * offNuke.CastTime - (timeCastingDots * ((30.0f - 15.0f) / (30.0f + timeToProc)));
                        postEclipseDPS = mainNuke.DamagePerHit / mainNuke.CastTime;
                        postEclipseManaUsed = mainNuke.BaseManaCost / mainNuke.CastTime * postEclipseTime;
                        postEclipseManaGained = (mainNuke.BaseManaCost * preEclipseOmenProcChance) + ((spellCrit + mainNuke.CriticalChanceModifier) * spellHit * moonkinFormProc) + (JoWProc / 4 * spellHit);

                        Duration = preEclipseTime + eclipseTime + postEclipseTime + timeCastingDots;
                        CastCount = castsToProc + (eclipseTime / offNuke.CastTime) + (postEclipseTime / mainNuke.CastTime);
                        ManaUsed = RotationData.ManaUsed = preEclipseManaUsed + eclipseManaUsed + postEclipseManaUsed;
                        ManaGained = RotationData.ManaGained = (preEclipseManaGained / mainNuke.CastTime * preEclipseTime) +
                            (eclipseManaGained / offNuke.CastTime * eclipseTime) +
                            (postEclipseManaGained / mainNuke.CastTime * postEclipseTime);

                        // Calculation of SF/W ratio changes depending on which is main nuke
                        if (mainNuke.Name == "SF")
                        {
                            ratioSFtoW = (castsToProc + postEclipseTime / mainNuke.CastTime) / (eclipseTime / offNuke.CastTime + castsToProc + postEclipseTime / mainNuke.CastTime);
                        }
                        else
                        {
                            ratioSFtoW = (eclipseTime / eclipseCastTime) / (eclipseTime / eclipseCastTime + castsToProc + postEclipseTime / mainNuke.CastTime);
                        }

                        DoMainNuke(character, calcs, ref offNuke, spellPower, spellHit, spellCrit, spellHaste);
                    }

                    DotTicks = moonFire.DotEffect.NumberOfTicks * moonfireCasts + insectSwarm.DotEffect.NumberOfTicks * insectSwarmCasts;
                    CastCount += moonfireCasts + insectSwarmCasts;
                    ManaUsed += moonfireCasts * moonFire.BaseManaCost + insectSwarmCasts * insectSwarm.BaseManaCost;
                    float mfSavingsFromOoC = moonFire.BaseManaCost - (moonFire.BaseManaCost *
                        (1 - ratioSFtoW * starfireOmenProcChance - (1 - ratioSFtoW) * wrathOmenProcChance));
                    float isSavingsFromOoC = insectSwarm.BaseManaCost - (insectSwarm.BaseManaCost *
                        (1 - ratioSFtoW * starfireOmenProcChance - (1 - ratioSFtoW) * wrathOmenProcChance));
                    ManaGained += moonfireCasts * (mfSavingsFromOoC + ((spellCrit + moonFire.CriticalChanceModifier) * moonkinFormProc * spellHit) + JoWProc * spellHit / 4.0f);
                    ManaGained += insectSwarmCasts * (isSavingsFromOoC + JoWProc * spellHit / 4.0f);
                    // Undo SF glyph
                    if (starfireGlyph) moonFire.DotEffect.Duration -= 9.0f;
                    // Undo iIS calculations
                    if (character.DruidTalents.ImprovedInsectSwarm > 0)
                    {
                        if (mainNuke.Name == "SF")
                        {
                            mainNuke.CriticalChanceModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                            offNuke.AllDamageModifier /= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                        }
                        else if (mainNuke.Name == "W")
                        {
                            mainNuke.AllDamageModifier /= 1 + 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                            offNuke.CriticalChanceModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                        }
                    }
                    damageDone = preEclipseDPS * preEclipseTime + eclipseDPS * eclipseTime + postEclipseDPS * postEclipseTime +
                        moonfireCasts * (moonFire.DamagePerHit + moonFire.DotEffect.DamagePerHit) + insectSwarmCasts * insectSwarm.DotEffect.DamagePerHit;
                    RotationData.DPM = damageDone / ManaUsed;

                    ManaUsed -= ManaGained;

                    RotationData.ManaUsed = ManaUsed;
                    RotationData.ManaGained = ManaGained;
                    return damageDone;
                default:
                    throw new Exception("Invalid rotation specified in rotation solver.");
            }
        }
    }

    // The interface class to the rest of Rawr.  Provides a single Solve method that runs all the calculations.
    static class MoonkinSolver
    {
        // A list of all currently active proc effects.
        public static List<ProcEffect> procEffects;
        // A list of all the damage spells
        private static Spell[] _spellData = null;
        public static Spell[] SpellData
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
                                SpellDamageModifier = 0.76f
                            },
                            School = SpellSchool.Nature
                        }
                    };
                }
                return _spellData;
            }
        }
        public static Spell Starfire
        {
            get
            {
                return SpellData[0];
            }
        }
        public static Spell Moonfire
        {
            get
            {
                return SpellData[1];
            }
        }
        public static Spell Wrath
        {
            get
            {
                return SpellData[2];
            }
        }
        public static Spell InsectSwarm
        {
            get
            {
                return SpellData[3];
            }
        }
        public static void ResetSpellList()
        {
            // Since the property rebuilding the array is based on this variable being null, this effectively forces a refresh
            _spellData = null;
        }

        // The spell rotations themselves.
        public static List<SpellRotation> rotations = null;

        // Results data from the calculations, which will be sent to the UI.
        static Dictionary<string, RotationData> cachedResults = new Dictionary<string, RotationData>();

        public static void Solve(Character character, ref CharacterCalculationsMoonkin calcs)
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
            float treeDamage = (character.DruidTalents.ForceOfNature == 1) ? DoTreeCalcs(baseSpellPower, 0.0f, calcs.BasicStats.Bloodlust, calcOpts.TreantLifespan, character.DruidTalents.Brambles) : 0.0f;
            // Extend that to number of casts per fight.  Round down and ensure that only complete tree casts are counted.
            treeDamage *= (int)Math.Floor(calcOpts.FightLength / 3.5f) + 1;
            // Multiply by raid-wide damage increases.
            treeDamage *= 1 + calcs.BasicStats.BonusDamageMultiplier;
            // Calculate the DPS averaged over the fight length.
            float treeDPS = treeDamage / (calcOpts.FightLength * 60.0f);

			// Do Starfall calculations.
			float starfallDamage = (character.DruidTalents.Starfall == 1) ? DoStarfallCalcs(baseSpellPower, baseHit, baseCrit, Wrath.CriticalDamageModifier) : 0.0f;
			starfallDamage *= (int)Math.Floor(calcOpts.FightLength / (3 + 1.0f/6.0f)) + 1;
			starfallDamage *= (1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusDamageMultiplier);
			float starfallDPS = starfallDamage / (calcOpts.FightLength * 60.0f);

            foreach (SpellRotation rot in rotations)
            {

                float accumulatedDamage = 0.0f;
                float totalUpTime = 0.0f;
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
                        proc.Activate(calcs, ref baseSpellPower, ref baseHit, ref baseCrit, ref baseHaste);
                        float tempDamage = rot.DamageDone(character, calcs, baseSpellPower, baseHit, baseCrit, baseHaste);
                        float tempUpTime = proc.UpTime(rot, calcs);
                        proc.Deactivate(calcs, ref baseSpellPower, ref baseHit, ref baseCrit, ref baseHaste);
                        accumulatedDamage += tempDamage * tempUpTime;
                        totalUpTime += tempUpTime;
                    }
                    if (proc.CalculateMP5 != null)
                    {
                        manaGained += proc.CalculateMP5(rot, calcs, baseSpellPower, baseHit, baseCrit, baseHaste) / 5.0f * calcs.FightLength * 60.0f;
                    }
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
        private static void BuildProcList(CharacterCalculationsMoonkin calcs)
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
        }

        // Non-rotation-specific mana calculations
        private static float GetEffectiveManaPool(Character character, CharacterCalculationsMoonkin calcs)
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
                    float userIntellect = calcs.BasicStats.Intellect - (character.MainHand == null ? 0 : character.MainHand.Stats.Intellect) - (character.OffHand == null ? 0 : character.OffHand.Stats.Intellect)
                        + calcOpts.InnervateWeaponInt;
                    // Do the same with spirit
                    float userSpirit = calcs.BasicStats.Spirit - (character.MainHand == null ? 0 : character.MainHand.Stats.Spirit) - (character.OffHand == null ? 0 : character.OffHand.Stats.Spirit)
                        + calcOpts.InnervateWeaponSpi;
                    // The new spirit regen for innervate periods uses the new weapon stats
                    spiritRegen = baseRegenConstant * (float)Math.Sqrt(userIntellect) * userSpirit;
                }
                float innervateManaRate = spiritRegen * 4 + calcs.BasicStats.Mp5 / 5f;
                float innervateTime = numInnervates * 20.0f;
                totalInnervateMana = innervateManaRate * innervateTime - (numInnervates * CalculationsMoonkin.BaseMana * 0.04f);
            }
            // Replenishment calculations
            float replenishmentPerTick = calcs.BasicStats.Mana * calcs.BasicStats.ManaRestoreFromMaxManaPerSecond;
            float replenishmentMana = calcOpts.ReplenishmentUptime * replenishmentPerTick * calcOpts.FightLength * 60;

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen + manaRestoredByPots + replenishmentMana;
        }

        // Now returns damage per cast to allow adjustments for fight length
        private static float DoTreeCalcs(float effectiveNatureDamage, float meleeHaste, float bloodLust, float treantLifespan, int bramblesLevel)
        {
            // 642 = base AP, 5.7% spell power scaling
            float attackPower = 642.0f + (float)Math.Floor(0.057f * effectiveNatureDamage);
            // 238.0 = base DPS, 1.7 = best observed swing speed
            float damagePerHit = (238.0f + attackPower / 14.0f) * 1.7f;
            float attackSpeed = 1.7f / (1 + meleeHaste) / (1 + bloodLust);
            float damagePerTree = (treantLifespan * 30.0f / attackSpeed) * damagePerHit * (1 + 0.05f * bramblesLevel);
            return 3 * damagePerTree;
        }

		// Starfall
		private static float DoStarfallCalcs(float effectiveArcaneDamage, float spellHit, float spellCrit, float critDamageModifier)
		{
			float baseDamage = 5460.0f;

			// Spell coefficient = 60%
			float damagePerNormalHit = baseDamage + 0.6f * effectiveArcaneDamage;
			float damagePerCrit = damagePerNormalHit * critDamageModifier;
			return (spellCrit * damagePerCrit + (1 - spellCrit) * damagePerNormalHit) * spellHit;
		}

        // Clicky trinket calculations
        private static void DoOnUseTrinketCalcs(CharacterCalculationsMoonkin calcs, float hitRate, ref float spellPower, ref float effectiveSpellCrit, ref float effectiveSpellHaste, ref float trinketExtraDPS)
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
                effectiveSpellHaste += calcs.BasicStats.HasteRatingFor20SecOnUse2Min * 20.0f / 120.0f;
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
        private static void RecreateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
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
        private static void UpdateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
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
            SpellRotation.NGReduction = character.DruidTalents.NaturesGrace / 3.0f * 0.5f;
        }

    }
}
