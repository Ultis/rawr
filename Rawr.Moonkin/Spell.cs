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

        // Critical hit damage = baseDamage * (baseCriticalOffset + baseCriticalMultiplier);
        protected float baseCriticalOffset = 1.0f;        // Chaotic Skyfire Diamond adds 0.03f to this value
        public float CriticalHitIncrement
        {
            get
            {
                return baseCriticalOffset;
            }
            set
            {
                baseCriticalOffset = value;
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

        public abstract float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, bool naturesGrace);
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
            baseDamage = (540.0f + 636.0f) / 2.0f;
            spellDamageMultiplier = 1.0f;
            baseCastTime = 3.5f;
            manaCost = 370.0f;
            dotEffect = null;
            school = SpellSchool.Arcane;
        }
        public override float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, bool naturesGrace)
        {
            // Reset base cast time
            if (unhastedCastTime > 0.0f)
                baseCastTime = unhastedCastTime;
            else
                unhastedCastTime = baseCastTime;

            float damageCoefficient = baseDamage + spellDamageMultiplier * spellDamage;
            // Adoriele's DPS calculations assume a 200% damage crit, we need to modify that
            float critDamageCoefficient = (baseCriticalOffset + baseCriticalMultiplier) / 2.0f;
            float critCoefficient = baseCriticalChance + critRate;
            float naturesGraceTime = naturesGrace ? 0.5f : 0.0f;
            float hitCoefficient = hitRate;
            float hasteCoefficient = 1 + hasteRating;

            // Use the property so that haste over the haste cap will clip at the current GCD, if possible to achieve for Starfire
            CastTime = (unhastedCastTime - naturesGraceTime * critCoefficient * hitCoefficient) / hasteCoefficient;

            return (damageCoefficient * (critDamageCoefficient + critCoefficient) * hitCoefficient) / ((baseCastTime - naturesGraceTime * critCoefficient * hitCoefficient) / hasteCoefficient);
        }
    }

    // Moonfire
    class Moonfire : Spell
    {
        public Moonfire()
        {
            name = "MF";
            baseDamage = (305.0f + 357.0f) / 2.0f;
            spellDamageMultiplier = 0.15f;
            baseCastTime = Spell.GlobalCooldown;
            manaCost = 495.0f;
            dotEffect = new DotEffect()
                {
                    Duration = 12.0f,
                    TickDuration = 3.0f,
                    DamagePerTick = 150.0f,
                    SpellDamageMultiplier = 0.52f
                };
            school = SpellSchool.Arcane;
        }
        public override float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, bool naturesGrace)
        {
            float damageCoefficient = baseDamage + spellDamageMultiplier * spellDamage;
            // Adoriele's DPS calculations assume a 200% damage crit, we need to modify that
            float critDamageCoefficient = (baseCriticalOffset + baseCriticalMultiplier) / 2.0f;
            float critCoefficient = baseCriticalChance + critRate;
            // Nature's Grace is ignored for Moonfire, because it is an instant cast
            float hitCoefficient = hitRate;
            // Haste rating is ignored for Moonfire, because it is an instant cast
            // Calculate DoT component
            float dotEffectDPS = dotEffect.NumberOfTicks * (dotEffect.DamagePerTick + dotEffect.SpellDamageMultiplierPerTick * spellDamage) / dotEffect.Duration;
            // Moonfire DPS is calculated over the duration of the DoT
            return (damageCoefficient * (critDamageCoefficient + critCoefficient) * hitCoefficient) / dotEffect.Duration + dotEffectDPS;
        }
    }

    // Wrath
    class Wrath : Spell
    {
        public Wrath()
        {
            name = "W";
            baseDamage = (381.0f + 429.0f) / 2.0f;
            spellDamageMultiplier = 0.571f;
            baseCastTime = 2.0f;
            manaCost = 255.0f;
            dotEffect = null;
            school = SpellSchool.Nature;
        }
        public override float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, bool naturesGrace)
        {
            // Reset base cast time
            if (unhastedCastTime > 0.0f)
                baseCastTime = unhastedCastTime;
            else
                unhastedCastTime = baseCastTime;

            float damageCoefficient = baseDamage + spellDamageMultiplier * spellDamage;
            // Adoriele's DPS calculations assume a 200% damage crit, we need to modify that
            float critDamageCoefficient = (baseCriticalOffset + baseCriticalMultiplier) / 2.0f;
            float critCoefficient = baseCriticalChance + critRate;
            // Nature's Grace is ignored for Wrath, because it does not reduce the GCD
            float hitCoefficient = hitRate;
            float hasteCoefficient = 1 + hasteRating;

            // Use the property so that haste over the haste cap will clip at the current GCD
            CastTime /= hasteCoefficient;

            return (damageCoefficient * (critDamageCoefficient + critCoefficient) * hitCoefficient) / baseCastTime;
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
            manaCost = 175.0f;
            dotEffect = new DotEffect()
            {
                Duration = 12.0f,
                TickDuration = 2.0f,
                DamagePerTick = 132.0f,
                SpellDamageMultiplier = 0.76f
            };
            school = SpellSchool.Nature;
        }
        public override float DPS(float spellDamage, float hitRate, float critRate, float hasteRating, bool naturesGrace)
        {
            // Insect Swarm is a pure DoT, therefore the calculations are relatively uncomplicated
            float dotEffectDPS = dotEffect.NumberOfTicks * (dotEffect.DamagePerTick + dotEffect.SpellDamageMultiplierPerTick * spellDamage) / dotEffect.Duration;
            return dotEffectDPS;
        }
    }

    class RotationData
    {
        public float DPS = 0.0f;
        public float DPM = 0.0f;
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
        public float DPS(float arcaneDamage, float natureDamage, float spellHit, float spellCrit, float spellHaste, float manaPool, float fightLength, bool naturesGrace)
        {
            float accumulatedDamage = 0.0f;
            float accumulatedManaUsed = 0.0f;
            float accumulatedDuration = 0.0f;
            Dictionary<float, float> activeDots = new Dictionary<float,float>();
            foreach (Spell sp in spells)
            {
                float currentSpellDPS = 0.0f;
                float timeSpentCasting = 0.0f;
                int numberOfCasts = 0;
                if (sp.School == SpellSchool.Arcane)
                {
                    currentSpellDPS = sp.DPS(arcaneDamage, spellHit, spellCrit, spellHaste, naturesGrace);
                }
                else
                {
                    currentSpellDPS = sp.DPS(natureDamage, spellHit, spellCrit, spellHaste, naturesGrace);
                }
                if (sp.DoT == null)
                {
                    float dotDuration = sp.CastTime;
                    foreach (KeyValuePair<float, float> pair in activeDots)
                    {
                        if (pair.Value > dotDuration)
                            dotDuration = pair.Value;
                    }
                    while (timeSpentCasting < dotDuration)
                    {
                        timeSpentCasting += sp.CastTime;
                        ++numberOfCasts;
                    }
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
                accumulatedDamage += currentSpellDPS * timeSpentCasting;
                accumulatedManaUsed += sp.ManaCost * numberOfCasts;
                accumulatedDuration += sp.CastTime * numberOfCasts;
            }
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
            foreach (Spell sp in spells)
            {
                float timeSpentCasting = 0.0f;
                if (sp.DoT == null)
                {
                    float dotDuration = sp.CastTime;
                    foreach (KeyValuePair<float, float> pair in activeDots)
                    {
                        if (pair.Value > dotDuration)
                            dotDuration = pair.Value;
                    }
                    while (timeSpentCasting < dotDuration)
                    {
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
                _avgCritChance += sp.SpecialCriticalModifier * _castCount;
                _duration += timeSpentCasting;
            }
            _avgCritChance /= _castCount;
        }
        private float _avgCritChance = 0.0f;
        private float _castCount = 0.0f;
        private float _duration = 0.0f;
        private int _dotTicks = 0;
        public float AverageCritChance
        {
            get
            {
                if (_avgCritChance == 0.0f)
                    CalculateRotationalVariables();
                return _avgCritChance;
            }
        }
        public float Duration
        {
            get
            {
                if (_duration == 0.0f)
                    CalculateRotationalVariables();
                return _duration;
            }
        }
        public float CastCount
        {
            get
            {
                if (_castCount == 0.0f)
                    CalculateRotationalVariables();
                return _castCount;
            }
        }
        public int TotalDotTicks
        {
            get
            {
                if (_dotTicks == 0)
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
        // These fields get filled in only after the DPS calculations are done
        public float DPM { get; set; }
        public TimeSpan TimeToOOM { get; set; }
    }

    static class MoonkinSolver
    {
        static Starfire starfire = null;
        static Moonfire moonfire = null;
        static Wrath wrath = null;
        static InsectSwarm insectSwarm = null;
        static Dictionary<string, RotationData> cachedResults = new Dictionary<string, RotationData>();
        static List<SpellRotation> SpellRotations = null;

        private static float GetEffectiveManaPool(Character character, CharacterCalculationsMoonkin calcs)
        {
            float fightLength = calcs.FightLength * 60.0f;

            float innervateCooldown = 360 - calcs.BasicStats.InnervateCooldownReduction;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen5SR * fightLength;

            // Mana pot calculations
            float manaPotDelay = float.Parse(character.CalculationOptions["ManaPotDelay"], System.Globalization.CultureInfo.InvariantCulture) * 60.0f;
            int numPots = character.CalculationOptions["ManaPots"] == "Yes" && fightLength - manaPotDelay > 0 ? ((int)(fightLength - manaPotDelay) / 120 + 1) : 0;
            float manaRestoredByPots = 0.0f;
            if (numPots > 0)
            {
                float manaPerPot = 0.0f;
                if (character.CalculationOptions["ManaPotType"] == "Super Mana Potion")
                    manaPerPot = 2400.0f;
                if (character.CalculationOptions["ManaPotType"] == "Fel Mana Potion")
                    manaPerPot = 3200.0f;
                // Bonus from Alchemist's Stone
                if (calcs.BasicStats.BonusManaPotion > 0)
                {
                    manaPerPot *= 1 + calcs.BasicStats.BonusManaPotion;
                }

                manaRestoredByPots = numPots * manaPerPot;
            }

            // Innervate calculations
            float innervateDelay = float.Parse(character.CalculationOptions["InnervateDelay"], System.Globalization.CultureInfo.InvariantCulture) * 60.0f;
            int numInnervates = character.CalculationOptions["Innervate"] == "Yes" && fightLength - innervateDelay > 0 ? ((int)(fightLength - innervateDelay) / (int)innervateCooldown + 1) : 0;
            float totalInnervateMana = 0.0f;
            if (numInnervates > 0)
            {
                // Innervate mana rate increases only spirit-based regen
                float spiritRegen = (calcs.ManaRegen - calcs.BasicStats.Mp5 / 5f);
                // Add in calculations for an innervate weapon
                if (character.CalculationOptions["InnervateWeapon"] == "Yes")
                {
                    float baseRegenConstant = 0.00932715221261f;
                    // Calculate the intellect from a weapon swap
                    float userIntellect = calcs.BasicStats.Intellect - (character.MainHand == null ? 0 : character.MainHand.Stats.Intellect) - (character.OffHand == null ? 0 : character.OffHand.Stats.Intellect)
                        + int.Parse(character.CalculationOptions["InnervateWeaponInt"], System.Globalization.CultureInfo.InvariantCulture);
                    // Do the same with spirit
                    float userSpirit = calcs.BasicStats.Spirit - (character.MainHand == null ? 0 : character.MainHand.Stats.Spirit) - (character.OffHand == null ? 0 : character.OffHand.Stats.Spirit)
                        + int.Parse(character.CalculationOptions["InnervateWeaponSpi"], System.Globalization.CultureInfo.InvariantCulture);
                    // The new spirit regen for innervate periods uses the new weapon stats
                    spiritRegen = baseRegenConstant * (float)Math.Sqrt(userIntellect) * userSpirit;
                }
                float innervateManaRate = spiritRegen * 4 + calcs.BasicStats.Mp5 / 5f;
                float innervateTime = numInnervates * 20.0f;
                totalInnervateMana = innervateManaRate * innervateTime - (numInnervates * calcs.BasicStats.Mana * 0.04f);
            }
            // Shadow priest calculations
            float sPriestMp5 = float.Parse(character.CalculationOptions["ShadowPriest"], System.Globalization.CultureInfo.InvariantCulture);
            float sPriestMana = sPriestMp5 / 5 * fightLength;

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen + manaRestoredByPots + sPriestMana;
        }

        private static void UpdateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            Stats stats = calcs.BasicStats;
            // Add (possibly talented) +spelldmg
            // Starfire: Damage +(0.04 * Wrath of Cenarius)
            // Wrath: Damage +(0.02 * Wrath of Cenarius)
            wrath.SpellDamageModifier += 0.02f * int.Parse(character.CalculationOptions["WrathofCenarius"]);
            starfire.SpellDamageModifier += 0.04f * int.Parse(character.CalculationOptions["WrathofCenarius"]);

            // Add spell damage from idols
            starfire.DamagePerHit += stats.StarfireDmg;
            moonfire.DamagePerHit += stats.MoonfireDmg;
            wrath.DamagePerHit += stats.WrathDmg;

            // Add spell-specific damage
            // Starfire, Moonfire, Wrath: Damage +(0.02 * Moonfury)
            wrath.DamagePerHit *= 1.0f + (0.02f * int.Parse(character.CalculationOptions["Moonfury"]));
            moonfire.DamagePerHit *= 1.0f + (0.02f * int.Parse(character.CalculationOptions["Moonfury"]));
            moonfire.DoT.DamagePerTick *= 1.0f + (0.02f * int.Parse(character.CalculationOptions["Moonfury"]));
            starfire.DamagePerHit *= 1.0f + (0.02f * int.Parse(character.CalculationOptions["Moonfury"]));

            // Wrath, Insect Swarm: Nature spell damage multipliers
            wrath.DamagePerHit *= ((1 + calcs.BasicStats.BonusNatureSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            insectSwarm.DoT.DamagePerTick *= ((1 + calcs.BasicStats.BonusNatureSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            // Starfire, Moonfire: Arcane damage multipliers
            starfire.DamagePerHit *= ((1 + calcs.BasicStats.BonusArcaneSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            moonfire.DamagePerHit *= ((1 + calcs.BasicStats.BonusArcaneSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            moonfire.DoT.DamagePerTick *= ((1 + calcs.BasicStats.BonusArcaneSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));

            // Add spell-specific crit chance
            // Wrath, Starfire: Crit chance +(0.02 * Focused Starlight)
            wrath.SpecialCriticalModifier += 0.02f * int.Parse(character.CalculationOptions["FocusedStarlight"]);
            starfire.SpecialCriticalModifier += 0.02f * int.Parse(character.CalculationOptions["FocusedStarlight"]);
            // Moonfire: Damage, Crit chance +(0.05 * Imp Moonfire)
            moonfire.DamagePerHit *= 1.0f + (0.05f * int.Parse(character.CalculationOptions["ImpMoonfire"]));
            moonfire.DoT.DamagePerTick *= 1.0f + (0.05f * int.Parse(character.CalculationOptions["ImpMoonfire"]));
            moonfire.SpecialCriticalModifier += 0.05f * int.Parse(character.CalculationOptions["ImpMoonfire"]);

            // Add spell-specific critical strike damage
            // Chaotic Skyfire Diamond
            starfire.CriticalHitIncrement += calcs.BasicStats.BonusSpellCritMultiplier;
            starfire.CriticalHitMultiplier += calcs.BasicStats.BonusSpellCritMultiplier;
            moonfire.CriticalHitIncrement += calcs.BasicStats.BonusSpellCritMultiplier;
            moonfire.CriticalHitMultiplier += calcs.BasicStats.BonusSpellCritMultiplier;
            wrath.CriticalHitIncrement += calcs.BasicStats.BonusSpellCritMultiplier;
            wrath.CriticalHitMultiplier += calcs.BasicStats.BonusSpellCritMultiplier;
            // Starfire, Moonfire, Wrath: Crit damage +(0.2 * Vengeance)
            starfire.CriticalHitMultiplier *= 1 + 0.2f * int.Parse(character.CalculationOptions["Vengeance"]);
            moonfire.CriticalHitMultiplier *= 1 + 0.2f * int.Parse(character.CalculationOptions["Vengeance"]);
            wrath.CriticalHitMultiplier *= 1 + 0.2f * int.Parse(character.CalculationOptions["Vengeance"]);

            // Reduce spell-specific mana costs
            // Starfire, Moonfire, Wrath: Mana cost -(0.03 * Moonglow)
            starfire.ManaCost *= 1.0f - (0.03f * int.Parse(character.CalculationOptions["Moonglow"]));
            moonfire.ManaCost *= 1.0f - (0.03f * int.Parse(character.CalculationOptions["Moonglow"]));
            wrath.ManaCost *= 1.0f - (0.03f * int.Parse(character.CalculationOptions["Moonglow"]));

            // Reduce spell-specific cast times
            // Wrath, Starfire: Cast time -(0.1 * Starlight Wrath)
            wrath.CastTime -= 0.1f * int.Parse(character.CalculationOptions["StarlightWrath"]);
            starfire.CastTime -= 0.1f * int.Parse(character.CalculationOptions["StarlightWrath"]);

            // Add set bonuses
            moonfire.DoT.Duration += stats.MoonfireExtension;
            starfire.SpecialCriticalModifier += stats.StarfireCritChance;

            // Latency calculations
            wrath.CastTime += calcs.Latency;
            starfire.CastTime += calcs.Latency;
            moonfire.CastTime += calcs.Latency;
            insectSwarm.CastTime += calcs.Latency;
        }

        public static void Solve(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            float effectiveSpellHit = calcs.BasicStats.SpellHitRating;
            bool naturesGrace = int.Parse(character.CalculationOptions["NaturesGrace"]) > 0 ? true : false;
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

            if (baseHitRate + effectiveSpellHit / 1262.0f > 0.99f)
            {
                effectiveSpellHit = 1262.0f * (0.99f - baseHitRate);
            }

            RecreateSpells(character, ref calcs);

            float maxDPS = 0.0f;
            foreach (SpellRotation rotation in SpellRotations)
            {
                // Reset all parameters to defaults
                Spell.GlobalCooldown = 1.5f;
                float effectiveArcaneDamage = calcs.ArcaneDamage;
                float effectiveNatureDamage = calcs.NatureDamage;
                float effectiveSpellCrit = calcs.BasicStats.SpellCritRating;
                float effectiveSpellHaste = calcs.BasicStats.SpellHasteRating;
                float effectiveMana = GetEffectiveManaPool(character, calcs);

                // Trinkets
                DoTrinketCalcs(calcs, rotation, baseHitRate + effectiveSpellHit / 1262.0f, ref effectiveArcaneDamage, ref effectiveNatureDamage, ref effectiveSpellCrit, ref effectiveSpellHaste);

                // JoW/mana restore procs
                effectiveMana += DoManaRestoreCalcs(calcs, rotation, baseHitRate + effectiveSpellHit / 1262.0f) * (fightLength / rotation.Duration);

                // Calculate average global cooldown based on effective haste rating (includes trinkets)
                Spell.GlobalCooldown /= 1 + effectiveSpellHaste * (1 / 1576.0f);
                // Reset the cast time on Insect Swarm and Moonfire, since this is affected by haste
                insectSwarm.CastTime = Spell.GlobalCooldown;
                moonfire.CastTime = Spell.GlobalCooldown;

                float currentDPS = rotation.DPS(effectiveArcaneDamage, effectiveNatureDamage, baseHitRate + effectiveSpellHit / 1262.0f, effectiveSpellCrit / 2208.0f, effectiveSpellHaste / 1576.0f, effectiveMana, fightLength, naturesGrace);
                if (currentDPS > maxDPS)
                {
                    calcs.SelectedRotation = rotation;
                    maxDPS = currentDPS;
                }
                cachedResults[rotation.Name] = new RotationData()
                {
                    DPS = currentDPS,
                    DPM = rotation.DPM,
                    TimeToOOM = rotation.TimeToOOM
                };
            }
            calcs.SubPoints = new float[] { maxDPS * fightLength };
            calcs.OverallPoints = calcs.SubPoints[0];
            calcs.Rotations = cachedResults;
        }

        private static float DoManaRestoreCalcs(CharacterCalculationsMoonkin calcs, SpellRotation rotation, float hitRate)
        {
            float manaFromOther = calcs.BasicStats.ManaRestorePerCast * rotation.CastCount;
            float manaFromJoW = calcs.BasicStats.ManaRestorePerHit * (hitRate * rotation.CastCount);
            float manaFromTrinket = 0.0f;
            // Pendant of the Violet Eye - stacking mp5 buff for 20 sec
            if (calcs.BasicStats.Mp5OnCastFor20SecOnUse2Min > 0)
            {
                float averageBuffMp5 = 0.0f;
                float averageCastTime = rotation.CastCount / rotation.Duration;
                int currentStep = 21;
                float currentTime = 0.0f;
                while (currentTime < 20.0f)
                {
                    averageBuffMp5 += (float)currentStep * (20.0f - currentTime > averageCastTime ? averageCastTime : 20.0f - currentTime);
                    currentTime += averageCastTime;
                    currentStep += 21;
                }
                averageBuffMp5 /= (currentStep - 21) / 21;
                manaFromTrinket = averageBuffMp5 * 4 / 240.0f;
            }
            return manaFromJoW + manaFromOther + manaFromTrinket;
        }

        private static void DoTrinketCalcs(CharacterCalculationsMoonkin calcs, SpellRotation rotation, float hitRate, ref float effectiveArcaneDamage, ref float effectiveNatureDamage, ref float effectiveSpellCrit, ref float effectiveSpellHaste)
        {
            // Unseen Moon proc
            if (rotation.HasMoonfire && calcs.BasicStats.UnseenMoonDamageBonus > 0)
            {
                float numberOfProcs = 0.5f / rotation.CastCount;    // 50% proc chance on one spell in the whole rotation
                float timeBetweenProcs = rotation.Duration / numberOfProcs;
                effectiveArcaneDamage += calcs.BasicStats.UnseenMoonDamageBonus * 10.0f / timeBetweenProcs;
                effectiveNatureDamage += calcs.BasicStats.UnseenMoonDamageBonus * 10.0f / timeBetweenProcs;
            }
            // The Lightning Capacitor
            // Calculate the average passive spell damage given the proc rate
            if (calcs.BasicStats.LightningCapacitorProc > 0)
            {
                float baseDamage = (694 + 806) / 2.0f;
                float averageDamage = baseDamage + (baseDamage * 1.5f * calcs.SpellCrit);
                float castsBetweenProcs = 1 / (rotation.AverageCritChance + calcs.SpellCrit) * 3.0f;
                effectiveArcaneDamage += averageDamage / castsBetweenProcs;
                effectiveNatureDamage += averageDamage / castsBetweenProcs;
            }
            // Shatterered Sun Pendant (45s internal CD)
            if (calcs.BasicStats.ShatteredSunAcumenProc > 0)
            {
                if (calcs.Scryer)
                {
                    float baseDamage = (333 + 367) / 2.0f;
                    float averageDamage = baseDamage + (baseDamage * 1.5f * calcs.SpellCrit);
                    float castsBetweenProcs = rotation.CastCount * (45.0f / rotation.Duration);
                    effectiveArcaneDamage += averageDamage / castsBetweenProcs;
                    effectiveNatureDamage += averageDamage / castsBetweenProcs;
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
                float baseDamage = (285 + 475) / 2.0f;
                float averageDamage = baseDamage + (baseDamage * 1.5f * calcs.SpellCrit);
                float timeBetweenProcs = 1 / (rotation.TotalDotTicks / rotation.Duration * 0.1f) + 15.0f;
                float castsBetweenProcs = rotation.CastCount * (timeBetweenProcs / rotation.Duration);
                effectiveArcaneDamage += averageDamage / castsBetweenProcs;
                effectiveNatureDamage += averageDamage / castsBetweenProcs;
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
                effectiveArcaneDamage += calcs.BasicStats.SpellDamageFor10SecOnHit_5 * 10.0f / (45.0f + timeBetweenProcs);
                effectiveNatureDamage += calcs.BasicStats.SpellDamageFor10SecOnHit_5 * 10.0f / (45.0f + timeBetweenProcs);
            }
            // 20% chance of spell damage on crit, 45 second cooldown.
            if (calcs.BasicStats.SpellDamageFor10SecOnCrit_20_45 > 0)
            {
                float procsPerRotation = 0.2f * hitRate * (effectiveSpellCrit / 2208.0f) * rotation.CastCount;
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
            if (calcs.BasicStats.SpellHasteFor20SecOnUse2Min > 0)
            {
                effectiveSpellHaste += calcs.BasicStats.SpellHasteFor20SecOnUse2Min * 20.0f / 120.0f;
            }
            // Spell damage trinkets
            if (calcs.BasicStats.SpellDamageFor15SecOnUse90Sec > 0)
            {
                effectiveArcaneDamage += calcs.BasicStats.SpellDamageFor15SecOnUse90Sec * 15.0f / 90.0f;
                effectiveNatureDamage += calcs.BasicStats.SpellDamageFor15SecOnUse90Sec * 15.0f / 90.0f;
            }
            if (calcs.BasicStats.SpellDamageFor20SecOnUse2Min > 0)
            {
                effectiveArcaneDamage += calcs.BasicStats.SpellDamageFor20SecOnUse2Min * 20.0f / 120.0f;
                effectiveNatureDamage += calcs.BasicStats.SpellDamageFor20SecOnUse2Min * 20.0f / 120.0f;
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
                Name = "MF/SFx4",
                Spells = new List<Spell>(new Spell[]
                {
                    moonfire,
                    starfire
                })
            },
            new SpellRotation()
            {
                Name = "MF/Wx8",
                Spells = new List<Spell>(new Spell[]
                {
                    moonfire,
                    wrath
                })
            },
            new SpellRotation()
            {
                Name = "IS/SFx4",
                Spells = new List<Spell>(new Spell[]
                {
                    insectSwarm,
                    starfire
                })
            },
            new SpellRotation()
            {
                Name = "IS/Wx8",
                Spells = new List<Spell>(new Spell[]
                {
                    insectSwarm,
                    wrath
                })
            },
            new SpellRotation()
            {
                Name = "MF/SFx3/W",
                Spells = new List<Spell>(new Spell[]
                {
                    moonfire,
                    starfire,
                    wrath
                })
            },
            new SpellRotation()
            {
                Name = "IS/SFx3/W",
                Spells = new List<Spell>(new Spell[]
                {
                    insectSwarm,
                    starfire,
                    wrath
                })
            },
            new SpellRotation()
            {
                Name = "IS/MF/SFx3",
                Spells = new List<Spell>(new Spell[]
                {
                    insectSwarm,
                    moonfire,
                    starfire
                })
            },
            new SpellRotation()
            {
                Name = "IS/MF/Wx7",
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
