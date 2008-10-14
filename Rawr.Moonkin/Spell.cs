using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Moonkin
{
    enum SpellSchool
    {
        Arcane,
        Nature
    }
    abstract class Spell
    {
        private static float gcd = 1.5f;
        public static float GlobalCooldown
        {
            get
            {
                return gcd;
            }
            set
            {
                if (value < 1.0f)
                    gcd = 1.0f;
                else if (value > 1.5f)
                    gcd = 1.5f;
                else
                    gcd = value;
            }
        }

        protected string name = "";
        public string Name
        {
            get
            {
                return name;
            }
        }

        protected SpellSchool school = SpellSchool.Arcane;
        public SpellSchool School
        {
            get
            {
                return school;
            }
        }

        protected float baseDamage = 0.0f;
        public float DamagePerHit
        {
            get
            {
                return baseDamage;
            }
            set
            {
                baseDamage = value;
            }
        }
        protected float spellDamageMultiplier = 1.0f;
        public float SpellDamageModifier
        {
            get
            {
                return spellDamageMultiplier;
            }
            set
            {
                spellDamageMultiplier = value;
            }
        }
        protected float specialDamageModifier = 1.0f;
        public float SpecialDamageModifier
        {
            get
            {
                return specialDamageModifier;
            }
            set
            {
                specialDamageModifier = value;
            }
        }
        protected float baseCriticalMultiplier = 0.5f;
        public float CriticalHitMultiplier
        {
            get
            {
                return baseCriticalMultiplier;
            }
            set
            {
                baseCriticalMultiplier = value;
            }
        }
        protected float baseCriticalChance = 0.0f;
        public float SpecialCriticalModifier
        {
            get
            {
                return baseCriticalChance;
            }
            set
            {
                baseCriticalChance = value;
            }
        }

        protected float baseCastTime = Spell.GlobalCooldown;
        protected float unhastedCastTime = 0.0f;
        public float CastTime
        {
            get
            {
                return baseCastTime;
            }
            set
            {
                if (value < Spell.GlobalCooldown)
                    baseCastTime = Spell.GlobalCooldown;
                else
                    baseCastTime = value;
            }
        }

        protected float manaCost = 0.0f;
        public float ManaCost
        {
            get
            {
                return manaCost;
            }
            set
            {
                if (value > 0)
                    manaCost = value;
            }
        }

        public abstract float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, int naturesGrace, float latency);
        protected DotEffect dotEffect = null;
        public DotEffect DoT
        {
            get
            {
                return dotEffect;
            }
            set
            {
                dotEffect = value;
            }
        }
    }
    class DotEffect
    {
        private float totalDuration = 0.0f;
        public float Duration
        {
            get
            {
                return totalDuration;
            }
            set
            {
                totalDuration = value;
            }
        }
        private float tickDuration = 0.0f;
        public float TickDuration
        {
            get
            {
                return tickDuration;
            }
            set
            {
                tickDuration = value;
            }
        }
        private float damagePerTick = 0.0f;
        public float DamagePerTick
        {
            get
            {
                return damagePerTick;
            }
            set
            {
                damagePerTick = value;
            }
        }
        private float spellDamageMultiplier = 0.0f;
        public float SpellDamageMultiplier
        {
            get
            {
                return spellDamageMultiplier;
            }
            set
            {
                spellDamageMultiplier = value;
            }
        }
        private float specialDamageMultiplier = 1.0f;
        public float SpecialDamageMultiplier
        {
            get
            {
                return specialDamageMultiplier;
            }
            set
            {
                spellDamageMultiplier = value;
            }
        }
        public float NumberOfTicks
        {
            get
            {
                return totalDuration / tickDuration;
            }
        }
        public float SpellDamageMultiplierPerTick
        {
            get
            {
                return SpellDamageMultiplier / NumberOfTicks;
            }
            set
            {
                SpellDamageMultiplier += value * NumberOfTicks;
            }
        }
    }

    // Starfire
    class Starfire : Spell
    {
        public Starfire()
        {
            name = "SF";
            baseDamage = (818.0f + 964.0f) / 2.0f;
            spellDamageMultiplier = 1.0f;
            baseCastTime = 3.5f;
            manaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.16f);
            dotEffect = null;
            school = SpellSchool.Arcane;
        }
        public override float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, int naturesGrace, float latency)
        {
            // Save the unhasted cast time
            if (unhastedCastTime == 0.0f)
                unhastedCastTime = baseCastTime;

            float damageCoefficient = (baseDamage + spellDamageMultiplier * spellDamage) * specialDamageModifier;
            float critDamageCoefficient = baseCriticalMultiplier;
            float critChanceCoefficient = baseCriticalChance + critRate;
            float naturesGraceTime = 0.5f * (naturesGrace / 3.0f);
            float hitCoefficient = hitRate;
            float hasteCoefficient = 1 + hasteRating;

            // New cast time calculations to deal with NG and clipping
            double castTimeNoNG = Math.Max(unhastedCastTime / hasteCoefficient, 1.0f);
            double castTimeWithNG = Math.Max((unhastedCastTime - naturesGraceTime) / hasteCoefficient, 1.0f);
            double NGChance = critChanceCoefficient * hitCoefficient;
            baseCastTime = (float)((1 - NGChance) * castTimeNoNG + NGChance * castTimeWithNG);
            baseCastTime += latency;

            return (damageCoefficient * (1 + critDamageCoefficient * critChanceCoefficient) * hitCoefficient) / baseCastTime;
        }
    }

    // Moonfire
    class Moonfire : Spell
    {
        public Moonfire()
        {
            name = "MF";
            baseDamage = (305.0f + 357.0f) / 2.0f;
            spellDamageMultiplier = (1.5f/3.5f) * (baseDamage / (baseDamage + 150.0f*4.0f));
            baseCastTime = Spell.GlobalCooldown;
            manaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.21f);
            dotEffect = new DotEffect()
                {
                    Duration = 12.0f,
                    TickDuration = 3.0f,
                    DamagePerTick = 150.0f,
                    SpellDamageMultiplier = (12.0f/15.0f) * (150.0f*4.0f / (baseDamage + 150.0f*4.0f))
                };
            school = SpellSchool.Arcane;
        }
        public override float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, int naturesGrace, float latency)
        {
            float damageCoefficient = (baseDamage + spellDamageMultiplier * spellDamage) * specialDamageModifier;
            float critDamageCoefficient = baseCriticalMultiplier;
            float critChanceCoefficient = baseCriticalChance + critRate;
            // Nature's Grace is ignored for Moonfire, because it is an instant cast
            float hitCoefficient = hitRate;
            // Haste rating is ignored for Moonfire, because it is an instant cast
            // Latency
            CastTime += latency;
            // Calculate DoT component
            float dotEffectDPS = (dotEffect.NumberOfTicks * (dotEffect.DamagePerTick + dotEffect.SpellDamageMultiplierPerTick * spellDamage) * specialDamageModifier* dotEffect.SpecialDamageMultiplier) / dotEffect.Duration;
            // Moonfire DPS is calculated over the duration of the DoT
            return (damageCoefficient * (1 + critDamageCoefficient * critChanceCoefficient) * hitCoefficient) / dotEffect.Duration + dotEffectDPS;
        }
    }

    // Wrath
    class Wrath : Spell
    {
        public Wrath()
        {
            name = "W";
            baseDamage = (431.0f + 485.0f) / 2.0f;
            spellDamageMultiplier = 2.0f/3.5f;
            baseCastTime = 2.0f;
            manaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.11f);
            dotEffect = null;
            school = SpellSchool.Nature;
        }
        public override float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, int naturesGrace, float latency)
        {
            // Save the unhasted cast time
            if (unhastedCastTime == 0.0f)
                unhastedCastTime = baseCastTime;

            float damageCoefficient = (baseDamage + spellDamageMultiplier * spellDamage) * specialDamageModifier;
            float critDamageCoefficient = baseCriticalMultiplier;
            float critChanceCoefficient = baseCriticalChance + critRate;
            float naturesGraceTime = 0.5f * (naturesGrace / 3.0f);
            float hitCoefficient = hitRate;
            float hasteCoefficient = 1 + hasteRating;

            // New cast time calculations to deal with NG and clipping
            double castTimeNoNG = Math.Max(unhastedCastTime / hasteCoefficient, 1.0f);
            double castTimeWithNG = Math.Max((unhastedCastTime - naturesGraceTime) / hasteCoefficient, 1.0f);
            double NGChance = critChanceCoefficient * hitCoefficient;
            baseCastTime = (float)((1 - NGChance) * castTimeNoNG + NGChance * castTimeWithNG);
            baseCastTime += latency;

            return (damageCoefficient * (1 + critDamageCoefficient * critChanceCoefficient) * hitCoefficient) / baseCastTime;
        }
    }

    // Insect Swarm
    class InsectSwarm : Spell
    {
        public InsectSwarm()
        {
            name = "IS";
            baseDamage = 0.0f;
            spellDamageMultiplier = 0.0f;
            baseCastTime = Spell.GlobalCooldown;
            manaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.08f);
            dotEffect = new DotEffect()
            {
                Duration = 12.0f,
                TickDuration = 2.0f,
                DamagePerTick = 172.0f,
                SpellDamageMultiplier = 0.76f
            };
            school = SpellSchool.Nature;
        }
        public override float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, int naturesGrace, float latency)
        {
            // Latency
            CastTime += latency;
            // Insect Swarm is a pure DoT, therefore the calculations are relatively uncomplicated
            float dotEffectDPS = (dotEffect.NumberOfTicks * (dotEffect.DamagePerTick + dotEffect.SpellDamageMultiplierPerTick * spellDamage) * specialDamageModifier * dotEffect.SpecialDamageMultiplier) / dotEffect.Duration;
            return dotEffectDPS;
        }
    }

    class RotationData
    {
        public float RawDPS = 0.0f;
        public float DPS = 0.0f;
        public float DPM = 0.0f;
        public float ManaUsed = 0.0f;
        public float ManaGained = 0.0f;
        public TimeSpan TimeToOOM = new TimeSpan(0, 0, 0);
    }

    class SpellRotation
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        private List<Spell> spells = new List<Spell>();
        public List<Spell> Spells
        {
            get
            {
                return spells;
            }
            set
            {
                spells = value;
            }
        }
        public float DPS(float arcaneDamage, float natureDamage, float spellHit, float spellCrit, float spellHaste, float manaPool, float fightLength, int naturesGrace, float T54pcBonus, float latency)
        {
            float accumulatedDamage = 0.0f;
            float accumulatedManaUsed = 0.0f;
            float accumulatedDuration = 0.0f;
            Dictionary<float, float> activeDots = new Dictionary<float,float>();
            // Handle case where two non-DoT spells are cast (DoTs should not fall off before last spell cast)
            bool has1NonDot = false;
            float lastSpellCastTime = 0.0f;
            foreach (Spell sp in spells)
            {
                if (sp.DoT == null && has1NonDot)
                {
                    lastSpellCastTime = (sp.CastTime / (1 + spellHaste)) + latency;
                }
                if (sp.DoT == null && !has1NonDot)
                    has1NonDot = true;
            }
            foreach (Spell sp in spells)
            {
                float currentSpellDPS = 0.0f;
                float timeSpentCasting = 0.0f;
                int numberOfCasts = 0;
                float timeSpentCastingIn4T5 = 0.0f;
                if (sp.School == SpellSchool.Arcane)
                {
                    currentSpellDPS = sp.DPS(arcaneDamage, spellHit, spellCrit, spellHaste, naturesGrace, latency);
                }
                else
                {
                    currentSpellDPS = sp.DPS(natureDamage, spellHit, spellCrit, spellHaste, naturesGrace, latency);
                }
                if (sp.DoT == null)
                {
                    float dotDuration = sp.CastTime;
                    foreach (KeyValuePair<float, float> pair in activeDots)
                    {
                        if (pair.Value > dotDuration)
                            dotDuration = pair.Value;
                    }
                    while (timeSpentCasting < dotDuration - lastSpellCastTime - latency)
                    {
                        // Nordrassil Regalia (4T5 bonus)
                        // This should handle the case where a DoT tick falls off before the last cast completes (I hope)
                        if (T54pcBonus > 0.0f && sp.Name == "SF" && activeDots.Count != 0 && dotDuration - timeSpentCasting - lastSpellCastTime >= sp.CastTime)
                        {
                            timeSpentCastingIn4T5 += sp.CastTime;
                        }
                        timeSpentCasting += sp.CastTime;
                        ++numberOfCasts;
                    }
                    lastSpellCastTime = 0.0f;
                }
                else
                {
                    ++numberOfCasts;
                    timeSpentCasting = sp.CastTime;
                    activeDots.Add(currentSpellDPS, sp.DoT.Duration);
                }
                List<float> dotsToDecrement = new List<float>();
                foreach (KeyValuePair<float, float> pair in activeDots)
                {
                    if (pair.Value > 0)
                    {
                        // Handle the case where the DoT tick may fall off
                        accumulatedDamage += pair.Key * (timeSpentCasting > pair.Value ? pair.Value : timeSpentCasting);
                        dotsToDecrement.Add(pair.Key);
                    }
                }
                foreach (float key in dotsToDecrement)
                {
                    activeDots[key] -= timeSpentCasting;
                }
                // Prevent double-counting of DoT spells
                if (sp.DoT == null)
                {
                    accumulatedDamage += (currentSpellDPS * timeSpentCastingIn4T5 * (1+T54pcBonus)) + (currentSpellDPS * (timeSpentCasting - timeSpentCastingIn4T5));
                }
                accumulatedManaUsed += sp.ManaCost * numberOfCasts;
                accumulatedDuration += sp.CastTime * numberOfCasts;
            }
            RawDPS = accumulatedDamage / accumulatedDuration;
            DPM = accumulatedDamage / accumulatedManaUsed;
            float secsToOOM = manaPool / (accumulatedManaUsed / accumulatedDuration);
            if (secsToOOM > fightLength)
                TimeToOOM = new TimeSpan(0, 0, 0);
            else
                TimeToOOM = new TimeSpan(0, (int)secsToOOM / 60, (int)secsToOOM % 60);
            return accumulatedDamage / accumulatedDuration * (secsToOOM >= fightLength ? fightLength : secsToOOM) / fightLength;
        }
        private void CalculateRotationalVariables()
        {
            Dictionary<float, float> activeDots = new Dictionary<float, float>();
            // Handle case where two non-DoT spells are cast (DoTs should not fall off before last spell cast)
            bool has1NonDot = false;
            float lastSpellCastTime = 0.0f;
            foreach (Spell sp in spells)
            {
                if (sp.DoT == null && has1NonDot)
                {
                    lastSpellCastTime = sp.CastTime;
                }
                if (sp.DoT == null && !has1NonDot)
                    has1NonDot = true;
            }
            foreach (Spell sp in spells)
            {
                float timeSpentCasting = 0.0f;
                if (sp.DoT == null)
                {
                    float dotDuration = sp.CastTime;
                    float minimumDot = 20.0f;
                    foreach (KeyValuePair<float, float> pair in activeDots)
                    {
                        if (pair.Value < minimumDot)
                        {
                            minimumDot = pair.Value;
                        }
                    }
                    if (minimumDot > dotDuration) dotDuration = minimumDot;
                    while (timeSpentCasting < dotDuration - lastSpellCastTime)
                    {
                        if (sp.Name == "SF")
                            ++_starfireCount;
                        else if (sp.Name == "W")
                            ++_wrathCount;
                        timeSpentCasting += sp.CastTime;
                        ++_castCount;
                    }
                }
                else
                {
                    _dotTicks += (int)sp.DoT.NumberOfTicks;
                    ++_castCount;
                    timeSpentCasting = sp.CastTime;
                    activeDots.Add(sp.DoT.DamagePerTick, sp.DoT.Duration);
                }
                List<float> dotsToDecrement = new List<float>();
                foreach (KeyValuePair<float, float> pair in activeDots)
                {
                    if (pair.Value > 0)
                    {
                        dotsToDecrement.Add(pair.Key);
                    }
                }
                foreach (float key in dotsToDecrement)
                {
                    activeDots[key] -= timeSpentCasting;
                }
                _manaUsed += sp.ManaCost * _castCount;
                _avgCritChance += sp.SpecialCriticalModifier * _castCount;
                _duration += timeSpentCasting;
            }
            _avgCritChance /= _castCount;
        }
        private float _avgCritChance = 0.0f;
        private float _castCount = 0.0f;
        private float _duration = 0.0f;
        private int _dotTicks = 0;
        private float _manaUsed = 0.0f;
        public float AverageCritChance
        {
            get
            {
                if (_duration == 0.0f)
                    CalculateRotationalVariables();
                return _avgCritChance;
            }
        }
        public float ManaUsed
        {
            get
            {
                if (_duration == 0.0f)
                    CalculateRotationalVariables();
                return _manaUsed;
            }
        }
        public float ManaGained { get; set; }
        public float Duration
        {
            get
            {
                if (_duration == 0.0f)
                    CalculateRotationalVariables();
                return _duration;
            }
            set
            {
                // Add an accessor for this property for the calculation of NG+Moonfire
                _duration = value;
            }
        }
        public float CastCount
        {
            get
            {
                if (_duration == 0.0f)
                    CalculateRotationalVariables();
                return _castCount;
            }
        }
        public int TotalDotTicks
        {
            get
            {
                if (_duration == 0)
                    CalculateRotationalVariables();
                return _dotTicks;
            }
        }
        public bool HasMoonfire
        {
            get
            {
                foreach (Spell sp in spells)
                {
                    if (sp.Name == "MF")
                        return true;
                }
                return false;
            }
        }
        public bool HasInsectSwarm
        {
            get
            {
                foreach (Spell sp in spells)
                {
                    if (sp.Name == "IS")
                        return true;
                }
                return false;
            }
        }
        private int _starfireCount = 0;
        public int StarfireCount
        {
            get
            {
                if (_duration == 0)
                    CalculateRotationalVariables();
                return _starfireCount;
            }
        }
        private int _wrathCount = 0;
        public int WrathCount
        {
            get
            {
                if (_duration == 0)
                    CalculateRotationalVariables();
                return _wrathCount;
            }
        }
        // These fields get filled in only after the DPS calculations are done
        public float DPM { get; set; }
        public TimeSpan TimeToOOM { get; set; }
        public float RawDPS { get; set; }

        public void ResetRotationalVariables()
        {
            _avgCritChance = 0.0f;
            _castCount = 0.0f;
            _dotTicks = 0;
            _duration = 0.0f;
            _manaUsed = 0.0f;
            _starfireCount = 0;
        }
    }

    static class MoonkinSolver
    {
        static Starfire starfire = null;
        static Moonfire moonfire = null;
        static Wrath wrath = null;
        static InsectSwarm insectSwarm = null;
        static Dictionary<string, RotationData> cachedResults = new Dictionary<string, RotationData>();
        static List<SpellRotation> SpellRotations = null;
        static float trinketExtraDPS = 0.0f;

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
                if (calcOpts.ManaPotType == "Super Mana Potion")
                    manaPerPot = 2400.0f;
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
            int numInnervates = calcOpts.Innervate && fightLength - innervateDelay > 0 ? ((int)(fightLength - innervateDelay) / (int)innervateCooldown + 1) : 0;
            float totalInnervateMana = 0.0f;
            if (numInnervates > 0)
            {
                // Innervate mana rate increases only spirit-based regen
                float spiritRegen = (calcs.ManaRegen - calcs.BasicStats.Mp5 / 5f);
                // Add in calculations for an innervate weapon
                if (calcOpts.InnervateWeapon)
                {
                    float baseRegenConstant = 0.00932715221261f;
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

        private static void UpdateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            Stats stats = calcs.BasicStats;
			CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // Add (possibly talented) +spelldmg
            // Starfire: Damage +(0.04 * Wrath of Cenarius)
            // Wrath: Damage +(0.02 * Wrath of Cenarius)
            wrath.SpellDamageModifier += 0.02f * character.DruidTalents.WrathOfCenarius;
            starfire.SpellDamageModifier += 0.04f * character.DruidTalents.WrathOfCenarius;

            // Add spell damage from idols
            starfire.DamagePerHit += stats.StarfireDmg;
            moonfire.DamagePerHit += stats.MoonfireDmg;
            wrath.DamagePerHit += stats.WrathDmg;

            // Add spell-specific damage
            // Starfire, Moonfire, Wrath: Damage +(0.02 * Moonfury)
            wrath.SpecialDamageModifier += 0.02f * character.DruidTalents.Moonfury;
            moonfire.SpecialDamageModifier += 0.02f * character.DruidTalents.Moonfury;
            starfire.SpecialDamageModifier += 0.02f * character.DruidTalents.Moonfury;
            // Moonfire, Insect Swarm: One extra tick (Nature's Splendor)
            moonfire.DoT.Duration += 3.0f * character.DruidTalents.NaturesSplendor;
            insectSwarm.DoT.Duration += 2.0f * character.DruidTalents.NaturesSplendor;
            // Moonfire: Damage, Crit chance +(0.05 * Imp Moonfire)
            moonfire.SpecialDamageModifier += 0.05f * character.DruidTalents.ImprovedMoonfire;
            moonfire.SpecialCriticalModifier += 0.05f * character.DruidTalents.ImprovedMoonfire;

            // Wrath, Insect Swarm: Nature spell damage multipliers
            wrath.SpecialDamageModifier *= ((1 + calcs.BasicStats.BonusNatureSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            insectSwarm.SpecialDamageModifier *= ((1 + calcs.BasicStats.BonusNatureSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            // Starfire, Moonfire: Arcane damage multipliers
            starfire.SpecialDamageModifier *= ((1 + calcs.BasicStats.BonusArcaneSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            moonfire.SpecialDamageModifier *= ((1 + calcs.BasicStats.BonusArcaneSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            // Moonfire, Insect Swarm: Damage +(0.01 * Genesis)
            moonfire.DoT.SpecialDamageMultiplier += 0.01f * character.DruidTalents.Genesis;
            insectSwarm.DoT.SpecialDamageMultiplier += 0.01f * character.DruidTalents.Genesis;

            // Level-based partial resistances
            wrath.SpecialDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 70);
            starfire.SpecialDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 70);
            moonfire.SpecialDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 70);
            // Insect Swarm is a binary spell

            // Add spell-specific crit chance
            // Wrath, Starfire: Crit chance +(0.02 * Nature's Majesty)
            wrath.SpecialCriticalModifier += 0.02f * character.DruidTalents.NaturesMastery;
            starfire.SpecialCriticalModifier += 0.02f * character.DruidTalents.NaturesMastery;

            // Add spell-specific critical strike damage
            // Starfire, Moonfire, Wrath: Crit damage +(0.2 * Vengeance)
            starfire.CriticalHitMultiplier *= 1 + 0.2f * character.DruidTalents.Vengeance;
            moonfire.CriticalHitMultiplier *= 1 + 0.2f * character.DruidTalents.Vengeance;
            wrath.CriticalHitMultiplier *= 1 + 0.2f * character.DruidTalents.Vengeance;
            // Chaotic Skyfire Diamond
            starfire.CriticalHitMultiplier *= 1.0f + 1.5f / 0.5f * stats.BonusSpellCritMultiplier;
            moonfire.CriticalHitMultiplier *= 1.0f + 1.5f / 0.5f * stats.BonusSpellCritMultiplier;
            wrath.CriticalHitMultiplier *= 1.0f + 1.5f / 0.5f * stats.BonusSpellCritMultiplier;

            // Reduce spell-specific mana costs
            // Starfire, Moonfire, Wrath: Mana cost -(0.03 * Moonglow)
            starfire.ManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);
            moonfire.ManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);
            wrath.ManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);

            // Reduce spell-specific cast times
            // Wrath, Starfire: Cast time -(0.1 * Starlight Wrath)
            wrath.CastTime -= 0.1f * character.DruidTalents.StarlightWrath;
            starfire.CastTime -= 0.1f * character.DruidTalents.StarlightWrath;

            // Add set bonuses
            moonfire.DoT.Duration += stats.MoonfireExtension;
            starfire.SpecialCriticalModifier += stats.StarfireCritChance;
        }

        public static void Solve(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            // Try to reset the cached results dictionary on each call
            cachedResults = new Dictionary<string, RotationData>();
            float effectiveSpellHit = calcs.BasicStats.HitRating;
			CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            int naturesGrace = character.DruidTalents.NaturesGrace;
            float fightLength = calcs.FightLength * 60.0f;

            float baseHitRate = 0.83f;

            switch (calcs.TargetLevel)
            {
                case 70:
                    baseHitRate = 0.96f;
                    break;
                case 71:
                    baseHitRate = 0.95f;
                    break;
                case 72:
                    baseHitRate = 0.94f;
                    break;
                case 73:
                    baseHitRate = 0.83f;
                    break;
                default:
                    baseHitRate = 0.83f;
                    break;
            }

            if (baseHitRate + effectiveSpellHit / CalculationsMoonkin.hitRatingConversionFactor > 1.0f)
            {
                effectiveSpellHit = CalculationsMoonkin.hitRatingConversionFactor * (1.0f - baseHitRate);
            }
            float spellHitRate = baseHitRate + effectiveSpellHit / CalculationsMoonkin.hitRatingConversionFactor;

            RecreateSpells(character, ref calcs);

            // Spell-specific mana cost reductions
            // Moonkin Form
            if (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm > 0)
            {
                starfire.ManaCost -= (calcs.SpellCrit + starfire.SpecialCriticalModifier) * 0.02f * calcs.BasicStats.Mana * spellHitRate;
                moonfire.ManaCost -= (calcs.SpellCrit + moonfire.SpecialCriticalModifier) * 0.02f * calcs.BasicStats.Mana * spellHitRate;
                wrath.ManaCost -= (calcs.SpellCrit + wrath.SpecialCriticalModifier) * 0.02f * calcs.BasicStats.Mana * spellHitRate;
            }
            // Generic mana restore per cast
            starfire.ManaCost -= calcs.BasicStats.ManaRestorePerCast;
            moonfire.ManaCost -= calcs.BasicStats.ManaRestorePerCast;
            wrath.ManaCost -= calcs.BasicStats.ManaRestorePerCast;
            insectSwarm.ManaCost -= calcs.BasicStats.ManaRestorePerCast;

            float maxDPS = 0.0f;
            float maxRawDPS = 0.0f;
            foreach (SpellRotation rotation in SpellRotations)
            {
                // Reset all parameters to defaults
                Spell.GlobalCooldown = 1.5f;
                float effectiveArcaneDamage = calcs.ArcaneDamage;
                float effectiveNatureDamage = calcs.NatureDamage;
                float effectiveSpellCrit = calcs.BasicStats.CritRating;
                float effectiveSpellHaste = calcs.BasicStats.HasteRating;
                float effectiveMana = GetEffectiveManaPool(character, calcs);

                // Improved Insect Swarm
                if (rotation.HasInsectSwarm && rotation.WrathCount > 0)
                {
                    wrath.SpecialDamageModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                }
                else if (rotation.HasMoonfire && rotation.StarfireCount > 0)
                {
                    starfire.SpecialCriticalModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                }

                // Trinkets
                trinketExtraDPS = 0.0f;
                // Do a pre-emptive call to rotation.DPS to get corrected cast times for spells
                rotation.DPS(effectiveArcaneDamage, effectiveNatureDamage, spellHitRate, effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor, effectiveSpellHaste / CalculationsMoonkin.hasteRatingConversionFactor, effectiveMana, fightLength, naturesGrace, calcs.BasicStats.StarfireBonusWithDot, calcs.Latency);
                rotation.ResetRotationalVariables();
                DoTrinketCalcs(calcs, rotation, spellHitRate, ref effectiveArcaneDamage, ref effectiveNatureDamage, ref effectiveSpellCrit, ref effectiveSpellHaste);

                // JoW/mana restore procs
                // Judgement of Wisdom
                starfire.ManaCost -= spellHitRate * calcs.BasicStats.ManaRestorePerHit * calcs.BasicStats.Mana / ((float)Math.Floor(4.0f / starfire.CastTime) + 1);
                wrath.ManaCost -= spellHitRate * calcs.BasicStats.ManaRestorePerHit * calcs.BasicStats.Mana / ((float)Math.Floor(4.0f / wrath.CastTime) + 1);

                // Omen of Clarity
                if (character.DruidTalents.OmenOfClarity > 0)
                {
                    // Starfire
                    float castsDuringCooldown = 10.0f / starfire.CastTime;
                    float expectedCastsToProc = (1 / (0.06f * spellHitRate)) + castsDuringCooldown;
                    float expectedProcChance = 1 / expectedCastsToProc;
                    starfire.ManaCost -= starfire.ManaCost * expectedProcChance;
                    // Wrath
                    castsDuringCooldown = 10.0f / wrath.CastTime;
                    expectedCastsToProc = (1 / (0.06f * spellHitRate)) + castsDuringCooldown;
                    expectedProcChance = 1 / expectedCastsToProc;
                    wrath.ManaCost -= wrath.ManaCost * expectedProcChance;
                }
                rotation.ResetRotationalVariables();
                effectiveMana += DoManaRestoreCalcs(calcs, rotation, spellHitRate, effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor, character.DruidTalents.OmenOfClarity > 0) * (fightLength / rotation.Duration);
                // Casting trees?  Remove from effective mana
                if (character.DruidTalents.ForceOfNature > 0)
                {
                    int numTreeCasts = ((int)fightLength / 120) + 1;
                    effectiveMana -= numTreeCasts * CalculationsMoonkin.BaseMana * 0.12f;
                }
                rotation.ManaGained = effectiveMana;
                // Calculate average global cooldown based on effective haste rating (includes trinkets)
                Spell.GlobalCooldown /= 1 + effectiveSpellHaste * (1 / CalculationsMoonkin.hasteRatingConversionFactor);
                // Reset the cast time on Insect Swarm and Moonfire, since this is affected by haste
                insectSwarm.CastTime = Spell.GlobalCooldown;
                moonfire.CastTime = Spell.GlobalCooldown;
                // Incorporate Nature's Grace with Moonfire into the rotational calculations
                if (naturesGrace > 0 && rotation.HasMoonfire)
                {
                    float critFromGear = effectiveSpellCrit * (1 / CalculationsMoonkin.critRatingConversionFactor);
                    if (rotation.StarfireCount > 0)
                    {
                        starfire.CastTime -= ((1 - (rotation.AverageCritChance + critFromGear)) * (moonfire.SpecialCriticalModifier + critFromGear) * 0.5f * (naturesGrace/3)) / rotation.StarfireCount;
                    }
                    else if (rotation.WrathCount > 0)
                    {
                        wrath.CastTime -= ((1 - (rotation.AverageCritChance + critFromGear)) * (moonfire.SpecialCriticalModifier + critFromGear) * 0.5f * (naturesGrace / 3)) / rotation.WrathCount;
                    }
                }
                float treeDPS = (character.DruidTalents.ForceOfNature > 0) ? DoTreeCalcs(effectiveNatureDamage, calcOpts.TreantLifespan, character.DruidTalents.Brambles) : 0;
                float currentDPS = rotation.DPS(effectiveArcaneDamage, effectiveNatureDamage, spellHitRate, effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor, effectiveSpellHaste / CalculationsMoonkin.hasteRatingConversionFactor, effectiveMana, fightLength, naturesGrace, calcs.BasicStats.StarfireBonusWithDot, calcs.Latency) + trinketExtraDPS + treeDPS;
                float currentRawDPS = rotation.RawDPS + trinketExtraDPS + treeDPS;

                // After the DPS calculations are done, we can make a stab at Eclipse calculations
                // Please note that the numbers on this are going to be EXTREMELY rough
                // Only one main nuke per rotation, thankfully
                if (rotation.StarfireCount > 0 && character.DruidTalents.Eclipse > 0)
                {
                    float eclipseProcChance = effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor + starfire.SpecialCriticalModifier;
                    eclipseProcChance *= spellHitRate;
                    eclipseProcChance *= character.DruidTalents.Eclipse / 3.0f;
                    eclipseProcChance *= rotation.StarfireCount / rotation.CastCount;   // Percentage of casts that are Starfire

                    float expectedCastsToProc = 1 / eclipseProcChance;

                    float timeToProc = expectedCastsToProc * starfire.CastTime;

                    float rotationLength = 40.0f + timeToProc;
                    float normalLength = rotationLength - 10.0f + starfire.CastTime;

                    float timeInEclipse = 10.0f - starfire.CastTime;
                    float eclipseDPS = wrath.DPS(effectiveNatureDamage, spellHitRate, effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor, effectiveSpellHaste / CalculationsMoonkin.hasteRatingConversionFactor, character.DruidTalents.NaturesGrace, calcs.Latency) * 1.2f;

                    float totalDamageDone = eclipseDPS * timeInEclipse + normalLength * currentDPS;
                    float totalMaxDamageDone = eclipseDPS * timeInEclipse + normalLength * currentRawDPS;

                    currentDPS = totalDamageDone / rotationLength;
                    currentRawDPS = totalMaxDamageDone / rotationLength;
                }
                else if (rotation.WrathCount > 0 && character.DruidTalents.Eclipse > 0)
                {
                    float eclipseProcChance = effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor + wrath.SpecialCriticalModifier;
                    eclipseProcChance *= spellHitRate;
                    eclipseProcChance *= 0.2f * character.DruidTalents.Eclipse / 3.0f;
                    eclipseProcChance *= rotation.WrathCount / rotation.CastCount;   // Percentage of casts that are Starfire

                    float expectedCastsToProc = 1 / eclipseProcChance;

                    float timeToProc = expectedCastsToProc * wrath.CastTime;

                    float rotationLength = 40.0f + timeToProc;
                    float normalLength = rotationLength - 10.0f + wrath.CastTime;

                    float timeInEclipse = 10.0f - wrath.CastTime;
                    float eclipseDPS = starfire.DPS(effectiveArcaneDamage, spellHitRate, effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor + 0.3f, effectiveSpellHaste / CalculationsMoonkin.hasteRatingConversionFactor, character.DruidTalents.NaturesGrace, calcs.Latency);

                    float totalDamageDone = eclipseDPS * timeInEclipse + normalLength * currentDPS;
                    float totalMaxDamageDone = eclipseDPS * timeInEclipse + normalLength * currentRawDPS;

                    currentDPS = totalDamageDone / rotationLength;
                    currentRawDPS = totalMaxDamageDone / rotationLength;
                }

                // Restore Starfire/Wrath's cast time because the objects are reused
                if (naturesGrace > 0 && rotation.HasMoonfire)
                {
                    float critFromGear = effectiveSpellCrit * (1 / CalculationsMoonkin.critRatingConversionFactor);
                    if (rotation.StarfireCount > 0)
                    {
                        starfire.CastTime += ((1 - (rotation.AverageCritChance + critFromGear)) * (moonfire.SpecialCriticalModifier + critFromGear) * 0.5f * (naturesGrace / 3)) / rotation.StarfireCount;
                    }
                    else if (rotation.WrathCount > 0)
                    {
                        wrath.CastTime += ((1 - (rotation.AverageCritChance + critFromGear)) * (moonfire.SpecialCriticalModifier + critFromGear) * 0.5f * (naturesGrace / 3)) / rotation.WrathCount;
                    }
                }
                // Undo Improved Insect Swarm
                if (rotation.HasInsectSwarm && rotation.WrathCount > 0)
                {
                    wrath.SpecialDamageModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                }
                else if (rotation.HasMoonfire && rotation.StarfireCount > 0)
                {
                    starfire.SpecialCriticalModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                }
                // Undo Omen of Clarity
                if (character.DruidTalents.OmenOfClarity > 0)
                {
                    // Starfire
                    float castsDuringCooldown = 10.0f / starfire.CastTime;
                    float expectedCastsToProc = (1 / (0.06f * spellHitRate)) + castsDuringCooldown;
                    float expectedProcChance = 1 / expectedCastsToProc;
                    starfire.ManaCost += starfire.ManaCost * expectedProcChance;
                    // Wrath
                    castsDuringCooldown = 10.0f / wrath.CastTime;
                    expectedCastsToProc = (1 / (0.06f * spellHitRate)) + castsDuringCooldown;
                    expectedProcChance = 1 / expectedCastsToProc;
                    wrath.ManaCost += wrath.ManaCost * expectedProcChance;
                }
                // Undo Judgement of Wisdom
                starfire.ManaCost += spellHitRate * calcs.BasicStats.ManaRestorePerHit * calcs.BasicStats.Mana / ((float)Math.Floor(4.0f / starfire.CastTime) + 1);
                wrath.ManaCost += spellHitRate * calcs.BasicStats.ManaRestorePerHit * calcs.BasicStats.Mana / ((float)Math.Floor(4.0f / wrath.CastTime) + 1);

                // All damage multiplier
                currentDPS *= 1 + calcs.BasicStats.BonusDamageMultiplier;
                currentRawDPS *= 1 + calcs.BasicStats.BonusDamageMultiplier;

                if (currentDPS > maxDPS)
                {
                    calcs.SelectedRotation = rotation;
                    maxDPS = currentDPS;
                }
                if (currentRawDPS > maxRawDPS)
                {
                    calcs.MaxDPSRotation = rotation;
                    maxRawDPS = currentRawDPS;
                }
                cachedResults[rotation.Name] = new RotationData()
                {
                    RawDPS = currentRawDPS,
                    DPS = currentDPS,
                    DPM = rotation.DPM,
                    ManaUsed = rotation.ManaUsed,
                    ManaGained = rotation.ManaGained,
                    TimeToOOM = rotation.TimeToOOM
                };
            }
            calcs.SubPoints = new float[] { maxDPS, maxRawDPS };
            calcs.OverallPoints = calcs.SubPoints[0] + calcs.SubPoints[1];
            calcs.Rotations = cachedResults;
        }

        // Let there be TREES.
        private static float DoTreeCalcs(float effectiveNatureDamage, float treantLifespan, int bramblesLevel)
        {
            float damagePerHit = 176.0f + 0.075f * effectiveNatureDamage;
            float attackSpeed = 1.6f;
            float damagePerTree = (treantLifespan * 30.0f / attackSpeed) * damagePerHit * (1 + 0.05f * bramblesLevel);
            return 3 * damagePerTree / 120.0f;
        }

        private static float DoManaRestoreCalcs(CharacterCalculationsMoonkin calcs, SpellRotation rotation, float hitRate, float critRate, bool omenOfClarity)
        {
            float manaFromTrinket = 0.0f;
            // Pendant of the Violet Eye - stacking mp5 buff for 20 sec
            if (calcs.BasicStats.Mp5OnCastFor20SecOnUse2Min > 0)
            {
                float currentTime = 0.0f;
                float currentMp5 = 21.0f;
                float timeSinceLastCast = 0.0f;
                while (currentTime < 20.0f)
                {
                    manaFromTrinket += currentMp5 / 5.0f * 2.0f;
                    currentTime += 2.0f;
                    timeSinceLastCast += 2.0f;
                    if (timeSinceLastCast >= rotation.Duration / rotation.CastCount)
                    {
                        timeSinceLastCast -= rotation.Duration / rotation.CastCount;
                        currentMp5 += 21.0f;
                    }
                }
                manaFromTrinket /= 120.0f;
                manaFromTrinket *= rotation.Duration;
            }
            return manaFromTrinket;
        }

        private static void DoTrinketCalcs(CharacterCalculationsMoonkin calcs, SpellRotation rotation, float hitRate, ref float effectiveArcaneDamage, ref float effectiveNatureDamage, ref float effectiveSpellCrit, ref float effectiveSpellHaste)
        {
            // Unseen Moon proc
            if (rotation.HasMoonfire && calcs.BasicStats.UnseenMoonDamageBonus > 0)
            {
                float numberOfProcs = 0.5f;    // 50% proc chance
                float timeBetweenProcs = rotation.Duration / numberOfProcs;
                effectiveArcaneDamage += calcs.BasicStats.UnseenMoonDamageBonus * 10.0f / timeBetweenProcs;
                effectiveNatureDamage += calcs.BasicStats.UnseenMoonDamageBonus * 10.0f / timeBetweenProcs;
            }
            // Ashtongue Talisman of Equilibrium (Moonkin version)
            if (rotation.StarfireCount > 0 && calcs.BasicStats.DruidAshtongueTrinket > 0)
            {
                double starfireCastsIn8Sec = 8.0 / starfire.CastTime;
                double uptime = 1 - Math.Pow((1 - 0.25), starfireCastsIn8Sec);
                effectiveArcaneDamage += calcs.BasicStats.DruidAshtongueTrinket * (float)uptime;
                effectiveNatureDamage += calcs.BasicStats.DruidAshtongueTrinket * (float)uptime;
            }
            // The Lightning Capacitor
            if (calcs.BasicStats.LightningCapacitorProc > 0)
            {
                float specialDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusNatureSpellPowerMultiplier);
                float baseDamage = (694 + 806) / 2.0f;
                float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
                float timeBetweenProcs = rotation.Duration / (hitRate * (calcs.SpellCrit + rotation.AverageCritChance) * rotation.CastCount);
                if (timeBetweenProcs < 2.5f) timeBetweenProcs = timeBetweenProcs * 3.0f + 2.5f;
                else timeBetweenProcs *= 3.0f;
                trinketExtraDPS += averageDamage / timeBetweenProcs;
            }
            // Shatterered Sun Pendant (45s internal CD)
            if (calcs.BasicStats.ShatteredSunAcumenProc > 0)
            {
                if (calcs.Scryer)
                {
                    float specialDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusArcaneSpellPowerMultiplier);
                    float baseDamage = (333 + 367) / 2.0f;
                    float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
                    trinketExtraDPS += averageDamage / 45.0f;
                }
                else
                {
                    effectiveArcaneDamage += 120.0f * 10.0f / 45.0f;
                    effectiveNatureDamage += 120.0f * 10.0f / 45.0f;
                }
            }
            // Timbal's Focusing Crystal (10% proc on a DoT tick, 15s internal cooldown)
            if (calcs.BasicStats.TimbalsProc > 0 && rotation.TotalDotTicks > 0)
            {
                float specialDamageModifier = (1 + calcs.BasicStats.BonusShadowSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier);
                float baseDamage = (285 + 475) / 2.0f;
                float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
                float timeBetweenProcs = 1 / (rotation.TotalDotTicks / rotation.Duration * 0.1f) + 15.0f;
                trinketExtraDPS += averageDamage / timeBetweenProcs;
            }
            // Spell damage for 10 seconds on resist
            if (calcs.BasicStats.SpellDamageFor10SecOnResist > 0)
            {
                float procsPerRotation = (1 - hitRate) * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveArcaneDamage += calcs.BasicStats.SpellDamageFor10SecOnResist * 10.0f / timeBetweenProcs;
                effectiveNatureDamage += calcs.BasicStats.SpellDamageFor10SecOnResist * 10.0f / timeBetweenProcs;
            }
            // 5% chance of spell damage on hit, no cooldown.
            if (calcs.BasicStats.SpellDamageFor10SecOnHit_5 > 0)
            {
                float procsPerRotation = 0.05f * hitRate * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveArcaneDamage += calcs.BasicStats.SpellDamageFor10SecOnHit_5  * 10.0f / timeBetweenProcs;
                effectiveNatureDamage += calcs.BasicStats.SpellDamageFor10SecOnHit_5  * 10.0f / timeBetweenProcs;
            }
            // 10% chance of spell damage on hit, 45 second cooldown.
            if (calcs.BasicStats.SpellDamageFor10SecOnHit_10_45 > 0)
            {
                float procsPerRotation = 0.1f * hitRate * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveArcaneDamage += calcs.BasicStats.SpellDamageFor10SecOnHit_10_45 * 10.0f / (45.0f + timeBetweenProcs);
                effectiveNatureDamage += calcs.BasicStats.SpellDamageFor10SecOnHit_10_45 * 10.0f / (45.0f + timeBetweenProcs);
            }
            // 20% chance of spell damage on crit, 45 second cooldown.
            if (calcs.BasicStats.SpellDamageFor10SecOnCrit_20_45 > 0)
            {
                float procsPerRotation = 0.2f * hitRate * (effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor) * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveArcaneDamage += calcs.BasicStats.SpellDamageFor10SecOnHit_5 * 10.0f / (45.0f + timeBetweenProcs);
                effectiveNatureDamage += calcs.BasicStats.SpellDamageFor10SecOnHit_5 * 10.0f / (45.0f + timeBetweenProcs);
            }
            // 15% chance of spell haste on cast, 45-second cooldown (Mystical Skyfire Diamond)
            if (calcs.BasicStats.SpellHasteFor6SecOnCast_15_45 > 0)
            {
                float procsPerRotation = 0.15f * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveSpellHaste += calcs.BasicStats.SpellHasteFor6SecOnCast_15_45 * 6.0f / (45.0f + timeBetweenProcs);
            }
            // 10% chance of spell haste on hit, 45-second cooldown (Quagmirran's Eye)
            if (calcs.BasicStats.SpellHasteFor6SecOnHit_10_45 > 0)
            {
                float procsPerRotation = 0.1f * hitRate * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveSpellHaste += calcs.BasicStats.SpellHasteFor6SecOnHit_10_45 * 6.0f / (45.0f + timeBetweenProcs);
            }
            // Haste trinkets
            if (calcs.BasicStats.HasteRatingFor20SecOnUse2Min > 0)
            {
                effectiveSpellHaste += calcs.BasicStats.HasteRatingFor20SecOnUse2Min * 20.0f / 120.0f;
            }
            // Spell damage trinkets
            if (calcs.BasicStats.SpellPowerFor15SecOnUse90Sec > 0)
            {
                effectiveArcaneDamage += calcs.BasicStats.SpellPowerFor15SecOnUse90Sec * 15.0f / 90.0f;
                effectiveNatureDamage += calcs.BasicStats.SpellPowerFor15SecOnUse90Sec * 15.0f / 90.0f;
            }
            if (calcs.BasicStats.SpellPowerFor20SecOnUse2Min > 0)
            {
                effectiveArcaneDamage += calcs.BasicStats.SpellPowerFor20SecOnUse2Min * 20.0f / 120.0f;
                effectiveNatureDamage += calcs.BasicStats.SpellPowerFor20SecOnUse2Min * 20.0f / 120.0f;
            }
        }

        private static void RecreateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            starfire = new Starfire();
            wrath = new Wrath();
            insectSwarm = new InsectSwarm();
            moonfire = new Moonfire();
            SpellRotations = new List<SpellRotation>(new SpellRotation[]
            {
            new SpellRotation()
            {
                Name = "MF/SF",
                Spells = new List<Spell>(new Spell[]
                {
                    moonfire,
                    starfire
                })
            },
            new SpellRotation()
            {
                Name = "MF/W",
                Spells = new List<Spell>(new Spell[]
                {
                    moonfire,
                    wrath
                })
            },
            new SpellRotation()
            {
                Name = "IS/SF",
                Spells = new List<Spell>(new Spell[]
                {
                    insectSwarm,
                    starfire
                })
            },
            new SpellRotation()
            {
                Name = "IS/W",
                Spells = new List<Spell>(new Spell[]
                {
                    insectSwarm,
                    wrath
                })
            },
            new SpellRotation()
            {
                Name = "IS/MF/SF",
                Spells = new List<Spell>(new Spell[]
                {
                    insectSwarm,
                    moonfire,
                    starfire
                })
            },
            new SpellRotation()
            {
                Name = "IS/MF/W",
                Spells = new List<Spell>(new Spell[]
                {
                    insectSwarm,
                    moonfire,
                    wrath
                })
            },
            new SpellRotation()
            {
                Name = "SF Spam",
                Spells = new List<Spell>(new Spell[]
                {
                    starfire
                })
            },
            new SpellRotation()
            {
                Name = "W Spam",
                Spells = new List<Spell>(new Spell[]
                {
                    wrath
                })
            }
            });

            UpdateSpells(character, ref calcs);
        }
    }
}
