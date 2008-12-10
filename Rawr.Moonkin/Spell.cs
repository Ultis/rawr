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
            set
            {
                name = value;
            }
        }

        protected SpellSchool school = SpellSchool.Arcane;
        public SpellSchool School
        {
            get
            {
                return school;
            }
            set
            {
                school = value;
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
        protected float idolExtraSpellPower = 0.0f;
        public float IdolExtraSpellPower
        {
            get
            {
                return idolExtraSpellPower;
            }
            set
            {
                idolExtraSpellPower = value;
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
        public float CastTime
        {
            get
            {
                return baseCastTime;
            }
            set
            {
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
                manaCost = value;
            }
        }

        public float DPS(float spellDamage, float hitRate, float critRate)
        {
            float damageCoefficient = (baseDamage + spellDamageMultiplier * (spellDamage + idolExtraSpellPower)) * specialDamageModifier;
            float critDamageCoefficient = baseCriticalMultiplier;
            float critChanceCoefficient = baseCriticalChance + critRate;
            float hitCoefficient = hitRate;

            return (damageCoefficient * (1 + critDamageCoefficient * critChanceCoefficient) * hitCoefficient) / (dotEffect == null ? baseCastTime : dotEffect.Duration) + (dotEffect == null ? 0 : dotEffect.DPS(spellDamage + idolExtraSpellPower, hitRate));
        }
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
                specialDamageMultiplier = value;
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
        public float DPS(float spellDamage, float hitRate)
        {
            return NumberOfTicks * (DamagePerTick + SpellDamageMultiplierPerTick * spellDamage) * SpecialDamageMultiplier / Duration;
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
        private static Spell[] _spellData = null;
        public static Spell[] SpellData
        {
            get
            {
                if (_spellData == null)
                {
                    // For Moonfire calculations
                    float baseDamage = (406.0f + 476.0f) / 2.0f;

                    _spellData = new Spell[] {
                        new Spell()
                        {
                            Name = "SF",
                            DamagePerHit = (1028.0f + 1212.0f) / 2.0f,
                            SpellDamageModifier = 1.0f,
                            CastTime = 3.5f,
                            ManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.16f),
                            DoT = null,
                            School = SpellSchool.Arcane
                        },
                        new Spell()
                        {
                            Name = "MF",
                            DamagePerHit = (406.0f + 476.0f) / 2.0f,
                            SpellDamageModifier = (1.5f/3.5f) * (baseDamage / (baseDamage + 200.0f*4.0f)),
                            CastTime = Spell.GlobalCooldown,
                            ManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.21f),
                            DoT = new DotEffect()
                                {
                                    Duration = 12.0f,
                                    TickDuration = 3.0f,
                                    DamagePerTick = 200.0f,
                                    SpellDamageMultiplier = (12.0f/15.0f) * (200.0f*4.0f / (baseDamage + 200.0f*4.0f))
                                },
                            School = SpellSchool.Arcane
                        },
                        new Spell()
                        {
                            Name = "W",
                            DamagePerHit = (553.0f + 623.0f) / 2.0f,
                            SpellDamageModifier = 2.0f/3.5f,
                            CastTime = 2.0f,
                            ManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.11f),
                            DoT = null,
                            School = SpellSchool.Nature
                        },
                        new Spell()
                        {
                            Name = "IS",
                            DamagePerHit = 0.0f,
                            SpellDamageModifier = 0.0f,
                            CastTime = Spell.GlobalCooldown,
                            ManaCost = (float)(int)(CalculationsMoonkin.BaseMana * 0.08f),
                            DoT = new DotEffect()
                            {
                                Duration = 12.0f,
                                TickDuration = 2.0f,
                                DamagePerTick = 1290.0f / 6.0f,
                                SpellDamageMultiplier = 0.76f
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
        public static void RecreateSpells()
        {
            // Since the property rebuilding the array is based on this variable being null, this effectively forces a refresh
            _spellData = null;
        }

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
        public float DPS(float arcaneDamage, float natureDamage, float spellHit, float spellCrit, float manaPool, float fightLength, float T54pcBonus)
        {
            float accumulatedDamage = 0.0f;
            float accumulatedManaUsed = 0.0f;
            float accumulatedDuration = 0.0f;
            Dictionary<string, KeyValuePair<float, float>> activeDots = new Dictionary<string, KeyValuePair<float, float>>();
            foreach (Spell sp in spells)
            {
                float currentSpellDPS = 0.0f;
                float timeSpentCasting = 0.0f;
                int numberOfCasts = 0;
                float timeSpentCastingIn4T5 = 0.0f;
                if (sp.School == SpellSchool.Arcane)
                {
                    currentSpellDPS = sp.DPS(arcaneDamage, spellHit, spellCrit);
                }
                else
                {
                    currentSpellDPS = sp.DPS(natureDamage, spellHit, spellCrit);
                }
                if (sp.DoT == null)
                {
                    float dotDuration = sp.CastTime;
                    foreach (KeyValuePair<string, KeyValuePair<float, float>> pair in activeDots)
                    {
                        if (pair.Value.Value > dotDuration)
                            dotDuration = pair.Value.Value;
                    }
                    while (timeSpentCasting < dotDuration)
                    {
                        // Nordrassil Regalia (4T5 bonus)
                        // This should handle the case where a DoT tick falls off before the last cast completes (I hope)
                        if (T54pcBonus > 0.0f && sp.Name == "SF" && activeDots.Count != 0 && dotDuration - timeSpentCasting >= sp.CastTime)
                        {
                            timeSpentCastingIn4T5 += sp.CastTime;
                        }
                        timeSpentCasting += sp.CastTime;
                        ++numberOfCasts;
                    }
                }
                else
                {
                    ++numberOfCasts;
                    timeSpentCasting = sp.CastTime;
                    activeDots.Add(sp.Name, new KeyValuePair<float,float>(currentSpellDPS, sp.DoT.Duration));
                }
                Dictionary<string, float> dotsToDecrement = new Dictionary<string,float>();
                foreach (KeyValuePair<string, KeyValuePair<float, float>> pair in activeDots)
                {
                    if (pair.Value.Value > 0)
                    {
                        // Handle the case where the DoT tick may fall off
                        accumulatedDamage += pair.Value.Key * (timeSpentCasting > pair.Value.Value ? pair.Value.Value : timeSpentCasting);
                        dotsToDecrement.Add(pair.Key, pair.Value.Key);
                    }
                }
                foreach (KeyValuePair<string, float> pair in dotsToDecrement)
                {
                    activeDots[pair.Key] = new KeyValuePair<float,float>(activeDots[pair.Key].Key, activeDots[pair.Key].Value - timeSpentCasting);
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
        public void CalculateRotationalVariables()
        {
            ResetRotationalVariables();
            _castCount = (HasMoonfire ? 1 : 0);
            _castCount += (HasInsectSwarm ? 1 : 0);
            Spell iSw = spells.Find(delegate(Spell sp)
            {
                return sp.Name == "IS";
            });
            Spell mf = spells.Find(delegate(Spell sp)
            {
                return sp.Name == "MF";
            });
            _duration = 0.0f;
            // Figure out which one lasts longer, MF or IS
            // This will be used in calculations for nukes
            if (iSw != null)
            {
                _duration = iSw.DoT.Duration;
                _dotTicks += (int)iSw.DoT.NumberOfTicks;
                _manaUsed += iSw.ManaCost;
            }
            if (mf != null)
            {
                if (mf.DoT.Duration >= _duration)
                    _duration = mf.DoT.Duration;
                _dotTicks += (int)mf.DoT.NumberOfTicks;
                _avgCritChance += mf.SpecialCriticalModifier;
                _manaUsed += mf.ManaCost;
            }
            foreach (Spell sp in spells)
            {
                // We found our main nuke, do calculations
                if (sp.DoT == null)
                {
                    if (iSw == null && mf == null)
                    {
                        _duration = sp.CastTime;
                    }
                    float numCasts = _duration / sp.CastTime;
                    _avgCritChance = (_avgCritChance + numCasts * sp.SpecialCriticalModifier) / (numCasts + _castCount);
                    _castCount += numCasts;
                    if (sp.Name == "SF")
                        _starfireCount = numCasts;
                    else
                        _wrathCount = numCasts;
                    _manaUsed += numCasts * sp.ManaCost;
                }
            }
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
            set
            {
                // Add an accessor for this property for the calculation of the Starfire glyph
                _manaUsed = value;
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
        private float _starfireCount = 0;
        public float StarfireCount
        {
            get
            {
                if (_duration == 0)
                    CalculateRotationalVariables();
                return _starfireCount;
            }
        }
        private float _wrathCount = 0;
        public float WrathCount
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
            _wrathCount = 0;
        }
    }

    static class MoonkinSolver
    {
        static Dictionary<string, RotationData> cachedResults = new Dictionary<string, RotationData>();
        static List<SpellRotation> SpellRotations = null;

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
            int numInnervates = calcOpts.Innervate && fightLength - innervateDelay > 0 ? ((int)(fightLength - innervateDelay) / (int)innervateCooldown + 1) : 0;
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

        private static void UpdateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            Stats stats = calcs.BasicStats;
			CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // Add (possibly talented) +spelldmg
            // Starfire: Damage +(0.04 * Wrath of Cenarius)
            // Wrath: Damage +(0.02 * Wrath of Cenarius)
            SpellRotation.Wrath.SpellDamageModifier += 0.02f * character.DruidTalents.WrathOfCenarius;
            SpellRotation.Starfire.SpellDamageModifier += 0.04f * character.DruidTalents.WrathOfCenarius;

            // Add spell damage from idols
            SpellRotation.Starfire.IdolExtraSpellPower += stats.StarfireDmg;
            SpellRotation.Moonfire.IdolExtraSpellPower += stats.MoonfireDmg;
            SpellRotation.Wrath.DamagePerHit += stats.WrathDmg;

            // Add spell-specific damage
            // Starfire, Moonfire, Wrath: Damage +(0.03 * Moonfury)
            SpellRotation.Wrath.SpecialDamageModifier += (float)Math.Floor(character.DruidTalents.Moonfury * 10 / 3.0f) / 100.0f;
            SpellRotation.Moonfire.SpecialDamageModifier += (float)Math.Floor(character.DruidTalents.Moonfury * 10 / 3.0f) / 100.0f;
            SpellRotation.Moonfire.DoT.SpecialDamageMultiplier += (float)Math.Floor(character.DruidTalents.Moonfury * 10 / 3.0f) / 100.0f;
            SpellRotation.Starfire.SpecialDamageModifier += (float)Math.Floor(character.DruidTalents.Moonfury * 10 / 3.0f) / 100.0f;
            // Moonfire, Insect Swarm: One extra tick (Nature's Splendor)
            SpellRotation.Moonfire.DoT.Duration += 3.0f * character.DruidTalents.NaturesSplendor;
            SpellRotation.InsectSwarm.DoT.Duration += 2.0f * character.DruidTalents.NaturesSplendor;
            // Moonfire: Damage, Crit chance +(0.05 * Imp Moonfire)
            SpellRotation.Moonfire.SpecialDamageModifier += 0.05f * character.DruidTalents.ImprovedMoonfire;
            SpellRotation.Moonfire.DoT.SpecialDamageMultiplier += 0.05f * character.DruidTalents.ImprovedMoonfire;
            SpellRotation.Moonfire.SpecialCriticalModifier += 0.05f * character.DruidTalents.ImprovedMoonfire;
            // Moonfire, Insect Swarm: Damage +(0.01 * Genesis)
            SpellRotation.Moonfire.DoT.SpecialDamageMultiplier += 0.01f * character.DruidTalents.Genesis;
            SpellRotation.InsectSwarm.DoT.SpecialDamageMultiplier += 0.01f * character.DruidTalents.Genesis;

            // Wrath, Insect Swarm: Nature spell damage multipliers
            SpellRotation.Wrath.SpecialDamageModifier *= ((1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            SpellRotation.InsectSwarm.DoT.SpecialDamageMultiplier *= ((1 + calcs.BasicStats.BonusNatureDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            // Starfire, Moonfire: Arcane damage multipliers
            SpellRotation.Starfire.SpecialDamageModifier *= ((1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            SpellRotation.Moonfire.SpecialDamageModifier *= ((1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
            SpellRotation.Moonfire.DoT.SpecialDamageMultiplier *= ((1 + calcs.BasicStats.BonusArcaneDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));

            // Level-based partial resistances
            SpellRotation.Wrath.SpecialDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);
            SpellRotation.Starfire.SpecialDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);
            SpellRotation.Moonfire.SpecialDamageModifier *= 1 - 0.02f * (calcs.TargetLevel - 80);
            SpellRotation.Moonfire.DoT.SpecialDamageMultiplier *= 1 - 0.02f * (calcs.TargetLevel - 80);
            // Insect Swarm is a binary spell

            // Add spell-specific crit chance
            // Wrath, Starfire: Crit chance +(0.02 * Nature's Majesty)
            SpellRotation.Wrath.SpecialCriticalModifier += 0.02f * character.DruidTalents.NaturesMastery;
            SpellRotation.Starfire.SpecialCriticalModifier += 0.02f * character.DruidTalents.NaturesMastery;

            // Add spell-specific critical strike damage
            // Starfire, Moonfire, Wrath: Crit damage +(0.2 * Vengeance)
            SpellRotation.Starfire.CriticalHitMultiplier *= 1 + 0.2f * character.DruidTalents.Vengeance;
            SpellRotation.Moonfire.CriticalHitMultiplier *= 1 + 0.2f * character.DruidTalents.Vengeance;
            SpellRotation.Wrath.CriticalHitMultiplier *= 1 + 0.2f * character.DruidTalents.Vengeance;
            // Chaotic Skyfire Diamond
            SpellRotation.Starfire.CriticalHitMultiplier *= 1.0f + 1.5f / 0.5f * stats.BonusSpellCritMultiplier;
            SpellRotation.Moonfire.CriticalHitMultiplier *= 1.0f + 1.5f / 0.5f * stats.BonusSpellCritMultiplier;
            SpellRotation.Wrath.CriticalHitMultiplier *= 1.0f + 1.5f / 0.5f * stats.BonusSpellCritMultiplier;

            // Reduce spell-specific mana costs
            // Starfire, Moonfire, Wrath: Mana cost -(0.03 * Moonglow)
            SpellRotation.Starfire.ManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);
            SpellRotation.Moonfire.ManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);
            SpellRotation.Wrath.ManaCost *= 1.0f - (0.03f * character.DruidTalents.Moonglow);

            // Reduce spell-specific cast times
            // Wrath, Starfire: Cast time -(0.1 * Starlight Wrath)
            SpellRotation.Wrath.CastTime -= 0.1f * character.DruidTalents.StarlightWrath;
            SpellRotation.Starfire.CastTime -= 0.1f * character.DruidTalents.StarlightWrath;

            // Add set bonuses
            SpellRotation.Moonfire.DoT.Duration += stats.MoonfireExtension;
            SpellRotation.Starfire.SpecialCriticalModifier += stats.StarfireCritChance;
			SpellRotation.InsectSwarm.SpecialDamageModifier *= 1.0f + stats.BonusInsectSwarmDamage;
			SpellRotation.Starfire.SpecialCriticalModifier += stats.BonusNukeCritChance;
			SpellRotation.Wrath.SpecialCriticalModifier += stats.BonusNukeCritChance;
        }

        public static void Solve(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            // Reset all spells to their base status
            RecreateSpells(character, ref calcs);
            Spell.GlobalCooldown = 1.5f;
            // Pull out the calculation options (may not be necessary?)
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // Try to reset the cached results dictionary on each call
            cachedResults = new Dictionary<string, RotationData>();

            // Spell ratings once the trinket calculator is done with them
            float effectiveSpellHit = calcs.SpellHit * CalculationsMoonkin.hitRatingConversionFactor;
            float effectiveArcaneDamage = calcs.ArcaneDamage;
            float effectiveNatureDamage = calcs.NatureDamage;
            float effectiveSpellCrit = calcs.SpellCrit * CalculationsMoonkin.critRatingConversionFactor;
            float effectiveSpellHaste = calcs.SpellHaste * CalculationsMoonkin.hasteRatingConversionFactor;
            // Mana pool without Pendant of the Violet Eye
            float effectiveMana = GetEffectiveManaPool(character, calcs);

            // Utility variables
            int naturesGrace = character.DruidTalents.NaturesGrace;
            float fightLength = calcs.FightLength * 60.0f;
            float trinketExtraDPS = 0.0f;

            // Casting trees?  Remove from effective mana
            if (character.DruidTalents.ForceOfNature > 0)
            {
                int numTreeCasts = ((int)fightLength / 180) + 1;
                effectiveMana -= numTreeCasts * CalculationsMoonkin.BaseMana * 0.12f;
            }

            float baseHitRate = 0.83f;

            switch (calcs.TargetLevel)
            {
                case 80:
                    baseHitRate = 0.96f;
                    break;
                case 81:
                    baseHitRate = 0.95f;
                    break;
                case 82:
                    baseHitRate = 0.94f;
                    break;
                case 83:
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
            DoOnUseTrinketCalcs(calcs, spellHitRate, ref effectiveArcaneDamage, ref effectiveNatureDamage, ref effectiveSpellCrit,
                ref effectiveSpellHaste, ref trinketExtraDPS);

            // Convenience calculations
            float spellCritRate = effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor;
            float spellHaste = effectiveSpellHaste / CalculationsMoonkin.hasteRatingConversionFactor;

            // Calculate average global cooldown based on effective haste rating (includes trinkets)
            Spell.GlobalCooldown = 1.5f / (1 + spellHaste) + calcs.Latency;
            // Update spell cast times with new haste
            foreach (Spell sp in SpellRotation.SpellData)
            {
                // Direct nukes, subject to Nature's Grace
                if (sp.DoT == null)
                {
                    float castTimeNoNG = Math.Max(sp.CastTime / (1 + spellHaste), 1.0f) + calcs.Latency;
                    float castTimeNG = Math.Max((sp.CastTime - 0.5f) / (1 + spellHaste), 1.0f) + calcs.Latency;
                    if (castTimeNG == 1 + calcs.Latency) castTimeNG += calcs.Latency;
                    float NGProcChance = (spellCritRate + sp.SpecialCriticalModifier) * spellHitRate * naturesGrace / 3.0f;
                    sp.CastTime = castTimeNG * NGProcChance + castTimeNoNG * (1 - NGProcChance);
                }
                else
                {
                    sp.CastTime = Math.Max(sp.CastTime / (1 + spellHaste), 1.0f) + calcs.Latency;
                }
            }

            // Spell-specific mana cost reductions
            // Moonkin Form
            if (character.ActiveBuffsContains("Moonkin Form") && character.DruidTalents.MoonkinForm > 0)
            {
                SpellRotation.Starfire.ManaCost -= (spellCritRate + SpellRotation.Starfire.SpecialCriticalModifier) * 0.02f * calcs.BasicStats.Mana * spellHitRate;
                SpellRotation.Moonfire.ManaCost -= (spellCritRate + SpellRotation.Moonfire.SpecialCriticalModifier) * 0.02f * calcs.BasicStats.Mana * spellHitRate;
                SpellRotation.Wrath.ManaCost -= (spellCritRate + SpellRotation.Wrath.SpecialCriticalModifier) * 0.02f * calcs.BasicStats.Mana * spellHitRate;
            }
			// Mana restore per crit trinket
			SpellRotation.Starfire.ManaCost -= (spellCritRate + SpellRotation.Starfire.SpecialCriticalModifier) * calcs.BasicStats.ManaRestorePerCrit * spellHitRate;
			SpellRotation.Moonfire.ManaCost -= (spellCritRate + SpellRotation.Moonfire.SpecialCriticalModifier) * calcs.BasicStats.ManaRestorePerCrit * spellHitRate;
			SpellRotation.Wrath.ManaCost -= (spellCritRate + SpellRotation.Wrath.SpecialCriticalModifier) * calcs.BasicStats.ManaRestorePerCrit * spellHitRate;
            // Generic mana restore per cast
            SpellRotation.Starfire.ManaCost -= calcs.BasicStats.ManaRestorePerCast;
            SpellRotation.Moonfire.ManaCost -= calcs.BasicStats.ManaRestorePerCast;
            SpellRotation.Wrath.ManaCost -= calcs.BasicStats.ManaRestorePerCast;
            SpellRotation.InsectSwarm.ManaCost -= calcs.BasicStats.ManaRestorePerCast;
            // Judgement of Wisdom (now generic!)
            if (calcs.BasicStats.ManaRestoreFromMaxManaPerHit > 0)
            {
                SpellRotation.Moonfire.ManaCost -= spellHitRate * calcs.BasicStats.ManaRestoreFromMaxManaPerHit * calcs.BasicStats.Mana;
                SpellRotation.Starfire.ManaCost -= spellHitRate * calcs.BasicStats.ManaRestoreFromMaxManaPerHit * calcs.BasicStats.Mana;
                SpellRotation.Wrath.ManaCost -= spellHitRate * calcs.BasicStats.ManaRestoreFromMaxManaPerHit * calcs.BasicStats.Mana;
            }

            float maxDPS = 0.0f;
            float maxRawDPS = 0.0f;
            // Save the on-use trinket DPS to be restored every new rotation
            float baselineTrinketDPS = trinketExtraDPS;
            foreach (SpellRotation rotation in SpellRotations)
            {
                // Temporary variables that will get reset with every rotation
                // These are the variables that change with procs and such
                float tempArcaneDamage = effectiveArcaneDamage;
                float tempNatureDamage = effectiveNatureDamage;
                float tempHit = spellHitRate;
                float tempCritRating = effectiveSpellCrit;
                float tempCrit = tempCritRating / CalculationsMoonkin.critRatingConversionFactor;
                float tempHasteRating = effectiveSpellHaste;
                // Reset trinket DPS to the on-use baseline
                trinketExtraDPS = baselineTrinketDPS;

                // Do additional spell stuff here, so that CalculateRotationalVariables() grabs all the correct parameters
                // Improved Insect Swarm
                if (rotation.HasInsectSwarm && rotation.WrathCount > 0)
                {
                    SpellRotation.Wrath.SpecialDamageModifier *= 1 + (0.01f * character.DruidTalents.ImprovedInsectSwarm);
                }
                else if (rotation.HasMoonfire && rotation.StarfireCount > 0)
                {
                    SpellRotation.Starfire.SpecialCriticalModifier += 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                }
                // Haste trinket calculations
                DoHasteProcCalcs(calcs, rotation, spellHitRate, ref tempHasteRating);
                float tempHaste = tempHasteRating / CalculationsMoonkin.hasteRatingConversionFactor;
				// Recalculate spell cast times (!)
                float hasteDiff = tempHaste - spellHaste;
                Dictionary<string, float> oldCastTimes = new Dictionary<string, float>();
                if (hasteDiff > 0)
                {
                    oldCastTimes.Add("GlobalCooldown", Spell.GlobalCooldown);
                    Spell.GlobalCooldown = Spell.GlobalCooldown / (1 + hasteDiff);
                    // Update spell cast times with new haste
                    foreach (Spell sp in SpellRotation.SpellData)
                    {
                        oldCastTimes.Add(sp.Name, sp.CastTime);
                        sp.CastTime = Math.Max((sp.CastTime - calcs.Latency) / (1 + hasteDiff), 1.0f) + calcs.Latency;
                    }
                }
                // Recalculate all variables
                rotation.CalculateRotationalVariables();
                
                // Incorporate Nature's Grace with Moonfire into the rotational calculations
                if (naturesGrace > 0 && rotation.HasMoonfire)
                {
                    if (rotation.StarfireCount > 0)
                    {
                        SpellRotation.Starfire.CastTime -= ((1 - (rotation.AverageCritChance + spellCritRate)) * (SpellRotation.Moonfire.SpecialCriticalModifier + spellCritRate) * 0.5f * (naturesGrace / 3)) / rotation.StarfireCount;
                    }
                    else if (rotation.WrathCount > 0)
                    {
                        SpellRotation.Wrath.CastTime -= ((1 - (rotation.AverageCritChance + spellCritRate)) * (SpellRotation.Moonfire.SpecialCriticalModifier + spellCritRate) * 0.5f * (naturesGrace / 3)) / rotation.WrathCount;
                    }
                    rotation.CalculateRotationalVariables();
                }

                DoProcTrinketCalcs(calcs, rotation, tempHit, tempCritRating, ref tempArcaneDamage, ref tempNatureDamage, ref trinketExtraDPS);

                // Average cast time for main nuke, including non-nuke spells
                float starfireAverageCastTime = 0.0f;
                if (rotation.StarfireCount > 0)
                    starfireAverageCastTime = (rotation.CastCount / rotation.StarfireCount) * SpellRotation.Starfire.CastTime;
                float wrathAverageCastTime = 0.0f;
                if (rotation.WrathCount > 0)
                    wrathAverageCastTime = (rotation.CastCount / rotation.WrathCount) * SpellRotation.Wrath.CastTime;

                // Omen of Clarity
                if (character.DruidTalents.OmenOfClarity > 0)
                {
                    // Starfire
                    if (starfireAverageCastTime > 0)
                    {
                        float expectedProcChance = (3.5f / 60.0f) * starfireAverageCastTime;
                        SpellRotation.Starfire.ManaCost -= SpellRotation.Starfire.ManaCost * expectedProcChance;
                    }
                    // Wrath
                    if (wrathAverageCastTime > 0)
                    {
                        float expectedProcChance = (3.5f / 60.0f) * wrathAverageCastTime;
                        SpellRotation.Wrath.ManaCost -= SpellRotation.Wrath.ManaCost * expectedProcChance;
                    }
                    rotation.CalculateRotationalVariables();
                }
                // Pendant of the Violet Eye
                effectiveMana += DoTrinketManaRestoreCalcs(calcs, rotation) * (fightLength / rotation.Duration);
                rotation.ManaGained = effectiveMana;

                // The heart of the matter.  Calculate rotation DPS based on previously calculated stats.
                float treeDPS = (character.DruidTalents.ForceOfNature > 0) ? DoTreeCalcs(tempNatureDamage, calcOpts.TreantLifespan, character.DruidTalents.Brambles) : 0;
                float currentDPS = rotation.DPS(tempArcaneDamage, tempNatureDamage, spellHitRate, spellCritRate, effectiveMana, fightLength, calcs.BasicStats.StarfireBonusWithDot) + trinketExtraDPS + treeDPS;
                float currentRawDPS = rotation.RawDPS + trinketExtraDPS + treeDPS;

                // Handle the Starfire glyph
                if ((calcOpts.glyph1 == "Starfire" || calcOpts.glyph2 == "Starfire" || calcOpts.glyph3 == "Starfire") && rotation.HasMoonfire)
                {
                    if (rotation.StarfireCount > 0)
                    {
                        Spell newMoonfire = new Spell()
                        {
                            Name = "MF",
                            School = SpellSchool.Arcane,
                            CastTime = SpellRotation.Moonfire.CastTime,
                            CriticalHitMultiplier = SpellRotation.Moonfire.CriticalHitMultiplier,
                            DamagePerHit = SpellRotation.Moonfire.DamagePerHit,
                            ManaCost = SpellRotation.Moonfire.ManaCost,
                            SpecialCriticalModifier = SpellRotation.Moonfire.SpecialCriticalModifier,
                            SpecialDamageModifier = SpellRotation.Moonfire.SpecialDamageModifier,
                            SpellDamageModifier = SpellRotation.Moonfire.SpellDamageModifier,
                            DoT = new DotEffect()
                            {
                                Duration = SpellRotation.Moonfire.DoT.Duration + 3.0f * Math.Min(rotation.StarfireCount, 3),
                                TickDuration = SpellRotation.Moonfire.DoT.TickDuration,
                                DamagePerTick = SpellRotation.Moonfire.DoT.DamagePerTick,
                                SpellDamageMultiplier = SpellRotation.Moonfire.DoT.SpellDamageMultiplier,
                                SpecialDamageMultiplier = SpellRotation.Moonfire.DoT.SpecialDamageMultiplier
                            }
                        };
                        SpellRotation newRotation = null;
                        if (rotation.HasInsectSwarm)
                        {
                            newRotation = new SpellRotation()
                            {
                                Spells = new List<Spell>()
                                {
                                    newMoonfire,
                                    SpellRotation.InsectSwarm,
                                    SpellRotation.Starfire
                                }
                            };
                        }
                        else
                        {
                            newRotation = new SpellRotation()
                            {
                                Spells = new List<Spell>()
                                {
                                    newMoonfire,
                                    SpellRotation.Starfire
                                }
                            };
                        }
                        newRotation.CalculateRotationalVariables();
                        currentDPS = newRotation.DPS(tempArcaneDamage, tempNatureDamage, spellHitRate, spellCritRate, effectiveMana, fightLength, calcs.BasicStats.StarfireBonusWithDot) + trinketExtraDPS + treeDPS;
                        currentRawDPS = newRotation.RawDPS + trinketExtraDPS + treeDPS;
                        rotation.ManaUsed = newRotation.ManaUsed;
                        rotation.TimeToOOM = newRotation.TimeToOOM;
                        rotation.RawDPS = newRotation.RawDPS;
                        rotation.DPM = newRotation.DPM;
                    }
                    else
                    {
                        Spell newMoonfireFirst = new Spell()
                        {
                            Name = "MF1",
                            School = SpellSchool.Arcane,
                            CastTime = SpellRotation.Moonfire.CastTime,
                            CriticalHitMultiplier = SpellRotation.Moonfire.CriticalHitMultiplier,
                            DamagePerHit = SpellRotation.Moonfire.DamagePerHit,
                            ManaCost = 0.0f,
                            SpecialCriticalModifier = SpellRotation.Moonfire.SpecialCriticalModifier,
                            SpecialDamageModifier = SpellRotation.Moonfire.SpecialDamageModifier,
                            SpellDamageModifier = SpellRotation.Moonfire.SpellDamageModifier,
                            DoT = new DotEffect()
                            {
                                Duration = 3 * SpellRotation.Starfire.CastTime + SpellRotation.Moonfire.CastTime,
                                TickDuration = SpellRotation.Moonfire.DoT.TickDuration,
                                DamagePerTick = SpellRotation.Moonfire.DoT.DamagePerTick,
                                SpellDamageMultiplier = SpellRotation.Moonfire.DoT.SpellDamageMultiplier,
                                SpecialDamageMultiplier = SpellRotation.Moonfire.DoT.SpecialDamageMultiplier
                            },
                            IdolExtraSpellPower = SpellRotation.Moonfire.IdolExtraSpellPower
                        };
                        Spell newMoonfireSecond = new Spell()
                        {
                            Name = "MF2",
                            School = SpellSchool.Arcane,
                            CastTime = 0.0f,
                            CriticalHitMultiplier = 0.0f,
                            DamagePerHit = 0.0f,
                            ManaCost = 0.0f,
                            SpecialCriticalModifier = 0.0f,
                            SpecialDamageModifier = 0.0f,
                            SpellDamageModifier = 0.0f,
                            DoT = new DotEffect()
                            {
                                Duration = SpellRotation.Moonfire.DoT.Duration + 9.0f - 3 * SpellRotation.Starfire.CastTime - SpellRotation.Moonfire.CastTime,
                                TickDuration = SpellRotation.Moonfire.DoT.TickDuration,
                                DamagePerTick = SpellRotation.Moonfire.DoT.DamagePerTick,
                                SpellDamageMultiplier = SpellRotation.Moonfire.DoT.SpellDamageMultiplier,
                                SpecialDamageMultiplier = SpellRotation.Moonfire.DoT.SpecialDamageMultiplier
                            },
                            IdolExtraSpellPower = SpellRotation.Moonfire.IdolExtraSpellPower
                        };
                        // The above two spells actually do damage.  This spell is a placeholder for calculating rotation duration and the like.
                        Spell newMoonfire = new Spell()
                        {
                            Name = "MF",
                            School = SpellSchool.Arcane,
                            CastTime = 0.0f,
                            CriticalHitMultiplier = 0.0f,
                            DamagePerHit = 0.0f,
                            ManaCost = SpellRotation.Moonfire.ManaCost,
                            SpecialCriticalModifier = 0.0f,
                            SpecialDamageModifier = 0.0f,
                            SpellDamageModifier = 0.0f,
                            DoT = new DotEffect()
                            {
                                Duration = SpellRotation.Moonfire.DoT.Duration + 9.0f,
                                TickDuration = SpellRotation.Moonfire.DoT.TickDuration,
                                DamagePerTick = 0.0f,
                                SpellDamageMultiplier = 0.0f,
                                SpecialDamageMultiplier = 0.0f
                            },
                            IdolExtraSpellPower = SpellRotation.Moonfire.IdolExtraSpellPower
                        };
                        SpellRotation newRotation = null;
                        if (rotation.HasInsectSwarm)
                        {
                            newRotation = new SpellRotation()
                            {
                                Spells = new List<Spell>()
                                {
                                    newMoonfire,
                                    newMoonfireFirst,
                                    SpellRotation.Starfire,
                                    newMoonfireSecond,
                                    SpellRotation.InsectSwarm,
                                    SpellRotation.Wrath
                                }
                            };
                        }
                        else
                        {
                            newRotation = new SpellRotation()
                            {
                                Spells = new List<Spell>()
                                {
                                    newMoonfire,
                                    newMoonfireFirst,
                                    SpellRotation.Starfire,
                                    newMoonfireSecond,
                                    SpellRotation.Wrath
                                }
                            };
                        }
                        newRotation.CalculateRotationalVariables();
                        currentDPS = newRotation.DPS(tempArcaneDamage, tempNatureDamage, spellHitRate, spellCritRate, effectiveMana, fightLength, calcs.BasicStats.StarfireBonusWithDot) + trinketExtraDPS + treeDPS;
                        currentRawDPS = newRotation.RawDPS + trinketExtraDPS + treeDPS;
                        rotation.ManaUsed = newRotation.ManaUsed;
                        rotation.TimeToOOM = newRotation.TimeToOOM;
                        rotation.RawDPS = newRotation.RawDPS;
                        rotation.DPM = newRotation.DPM;
                    }
                }

                // After the DPS calculations are done, we can make a stab at Eclipse calculations
                // Please note that the numbers on this are going to be EXTREMELY rough
                // Only one main nuke per rotation, thankfully
                if (character.DruidTalents.Eclipse > 0)
                {
                    if (calcOpts.SmartSwitching)
                    {
                        if (rotation.StarfireCount > 0)
                        {
                            float eclipseProcChance = spellCritRate + SpellRotation.Wrath.SpecialCriticalModifier;
                            eclipseProcChance *= spellHitRate;
                            eclipseProcChance *= 0.2f * character.DruidTalents.Eclipse / 3.0f;
                            float expectedCastsToProc = 1 / eclipseProcChance;

                            // This will be time spent trying to proc Eclipse
                            float timeToProc = expectedCastsToProc * SpellRotation.Wrath.CastTime;
                            float procPeriodDPS = SpellRotation.Wrath.DPS(tempNatureDamage, spellHitRate, spellCritRate) + trinketExtraDPS + treeDPS;

                            float timeInEclipse = 15.0f - SpellRotation.Wrath.CastTime;
                            float eclipseDPS = SpellRotation.Starfire.DPS(tempArcaneDamage, spellHitRate, spellCritRate + 0.3f) + trinketExtraDPS + treeDPS;

                            float rotationLength = 30.0f;
                            float nonProcTime = rotationLength - timeInEclipse;

                            float totalDamageDone = timeToProc * procPeriodDPS + timeInEclipse * eclipseDPS + nonProcTime * currentDPS;
                            float totalMaxDamageDone = timeToProc * procPeriodDPS + timeInEclipse * eclipseDPS + nonProcTime * currentRawDPS;

                            currentDPS = totalDamageDone / (rotationLength + timeToProc);
                            currentRawDPS = totalMaxDamageDone / (rotationLength + timeToProc);
                        }
                        else
                        {
                            float eclipseProcChance = spellCritRate + SpellRotation.Starfire.SpecialCriticalModifier;
                            eclipseProcChance *= spellHitRate;
                            eclipseProcChance *= character.DruidTalents.Eclipse / 3.0f;
                            float expectedCastsToProc = 1 / eclipseProcChance;

                            // This will be time spent trying to proc Eclipse
                            float timeToProc = expectedCastsToProc * SpellRotation.Starfire.CastTime;
                            float procPeriodDPS = SpellRotation.Starfire.DPS(tempArcaneDamage, spellHitRate, spellCritRate) + trinketExtraDPS + treeDPS;

                            float timeInEclipse = 15.0f - SpellRotation.Starfire.CastTime;
                            float eclipseDPS = SpellRotation.Wrath.DPS(tempNatureDamage, spellHitRate, spellCritRate) * 1.2f + trinketExtraDPS + treeDPS;

                            float rotationLength = 30.0f;
                            float nonProcTime = rotationLength - timeInEclipse;

                            float totalDamageDone = timeToProc * procPeriodDPS + timeInEclipse * eclipseDPS + nonProcTime * currentDPS;
                            float totalMaxDamageDone = timeToProc * procPeriodDPS + timeInEclipse * eclipseDPS + nonProcTime * currentRawDPS;

                            currentDPS = totalDamageDone / (rotationLength + timeToProc);
                            currentRawDPS = totalMaxDamageDone / (rotationLength + timeToProc);
                        }
                    }
                    else
                    {
                        if (rotation.StarfireCount > 0)
                        {
                            float eclipseProcChance = spellCritRate + SpellRotation.Starfire.SpecialCriticalModifier;
                            eclipseProcChance *= spellHitRate;
                            eclipseProcChance *= character.DruidTalents.Eclipse / 3.0f;
                            eclipseProcChance *= rotation.StarfireCount / rotation.CastCount;   // Percentage of casts that are Starfire

                            float expectedCastsToProc = 1 / eclipseProcChance;

                            float timeToProc = expectedCastsToProc * starfireAverageCastTime;

                            float rotationLength = 30.0f + timeToProc;
                            float normalLength = rotationLength - 15.0f + starfireAverageCastTime;

                            float timeInEclipse = 15.0f - starfireAverageCastTime;
                            float eclipseDPS = SpellRotation.Wrath.DPS(tempNatureDamage, spellHitRate, spellCritRate) * 1.2f + trinketExtraDPS + treeDPS;

                            float totalDamageDone = eclipseDPS * timeInEclipse + normalLength * currentDPS;
                            float totalMaxDamageDone = eclipseDPS * timeInEclipse + normalLength * currentRawDPS;

                            currentDPS = totalDamageDone / rotationLength;
                            currentRawDPS = totalMaxDamageDone / rotationLength;
                        }
                        else
                        {
                            float eclipseProcChance = spellCritRate + SpellRotation.Wrath.SpecialCriticalModifier;
                            eclipseProcChance *= spellHitRate;
                            eclipseProcChance *= 0.2f * character.DruidTalents.Eclipse / 3.0f;
                            eclipseProcChance *= rotation.WrathCount / rotation.CastCount;   // Percentage of casts that are Starfire

                            float expectedCastsToProc = 1 / eclipseProcChance;

                            float timeToProc = expectedCastsToProc * wrathAverageCastTime;

                            float rotationLength = 30.0f + timeToProc;
                            float normalLength = rotationLength - 15.0f + wrathAverageCastTime;

                            float timeInEclipse = 15.0f - wrathAverageCastTime;
                            float eclipseDPS = SpellRotation.Starfire.DPS(tempArcaneDamage, spellHitRate, spellCritRate + 0.3f) + trinketExtraDPS + treeDPS;

                            float totalDamageDone = eclipseDPS * timeInEclipse + normalLength * currentDPS;
                            float totalMaxDamageDone = eclipseDPS * timeInEclipse + normalLength * currentRawDPS;

                            currentDPS = totalDamageDone / rotationLength;
                            currentRawDPS = totalMaxDamageDone / rotationLength;
                        }
                    }
                }

                // Undo Improved Insect Swarm
                if (rotation.HasInsectSwarm && rotation.WrathCount > 0)
                {
                    SpellRotation.Wrath.SpecialDamageModifier /= 1 + (0.01f * character.DruidTalents.ImprovedInsectSwarm);
                }
                else if (rotation.HasMoonfire && rotation.StarfireCount > 0)
                {
                    SpellRotation.Starfire.SpecialCriticalModifier -= 0.01f * character.DruidTalents.ImprovedInsectSwarm;
                }
                // Undo Omen of Clarity
                if (character.DruidTalents.OmenOfClarity > 0)
                {
                    // Starfire
                    if (starfireAverageCastTime > 0)
                    {
                        float expectedProcChance = (3.5f / 60.0f) * starfireAverageCastTime;
                        SpellRotation.Starfire.ManaCost += SpellRotation.Starfire.ManaCost * expectedProcChance;
                    }
                    // Wrath
                    if (wrathAverageCastTime > 0)
                    {
                        float expectedProcChance = (3.5f / 60.0f) * wrathAverageCastTime;
                        SpellRotation.Wrath.ManaCost += SpellRotation.Wrath.ManaCost * expectedProcChance;
                    }
                }

                // Restore Starfire/Wrath's cast time because the objects are reused
                if (naturesGrace > 0 && rotation.HasMoonfire)
                {
                    if (rotation.StarfireCount > 0)
                    {
                        SpellRotation.Starfire.CastTime += ((1 - (rotation.AverageCritChance + spellCritRate)) * (SpellRotation.Moonfire.SpecialCriticalModifier + spellCritRate) * 0.5f * (naturesGrace / 3)) / rotation.StarfireCount;
                    }
                    else if (rotation.WrathCount > 0)
                    {
                        SpellRotation.Wrath.CastTime += ((1 - (rotation.AverageCritChance + spellCritRate)) * (SpellRotation.Moonfire.SpecialCriticalModifier + spellCritRate) * 0.5f * (naturesGrace / 3)) / rotation.WrathCount;
                    }
                }
                // Undo haste trinkets (!)
                if (hasteDiff > 0)
                {
                    Spell.GlobalCooldown = oldCastTimes["GlobalCooldown"];
                    // Update spell cast times with new haste
                    foreach (Spell sp in SpellRotation.SpellData)
                    {
                        sp.CastTime = oldCastTimes[sp.Name];
                    }
                }
                // All damage multiplier
                currentDPS *= 1 + calcs.BasicStats.BonusDamageMultiplier;
                currentRawDPS *= 1 + calcs.BasicStats.BonusDamageMultiplier;

                // Save maximum mana-limited and non-mana limited DPS rotations
                if (calcOpts.userRotation == "None" && currentDPS > maxDPS)
                {
                    calcs.SelectedRotation = rotation;
                    maxDPS = currentDPS;
                }
                else if (calcOpts.userRotation != "None" && rotation.Name == calcOpts.userRotation)
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
            float damagePerHit = (450.0f + 0.052f * effectiveNatureDamage) * (2.0f * 1.02f) * (1.0f);
            float attackSpeed = 1.8f / (1 + 0f);
            float damagePerTree = (treantLifespan * 30.0f / attackSpeed) * damagePerHit * (1 + 0.05f * bramblesLevel);
            return 3 * damagePerTree / 180.0f;
        }

        private static float DoTrinketManaRestoreCalcs(CharacterCalculationsMoonkin calcs, SpellRotation rotation)
        {
            float manaFromTrinket = 0.0f;
            // Pendant of the Violet Eye - stacking mp5 buff for 20 sec
            if (calcs.BasicStats.Mp5OnCastFor20SecOnUse2Min > 0)
            {
                float averageCastsIn20Sec = 20.0f / (rotation.CastCount / rotation.Duration);
                float averageTrinketMp5 = calcs.BasicStats.Mp5OnCastFor20SecOnUse2Min * (rotation.Duration / rotation.CastCount) / 20.0f;
                manaFromTrinket += averageTrinketMp5 * 4.0f / 120.0f * rotation.Duration;
            }
			// Mp5 on cast for 10 sec, 45 sec cooldown
			if (calcs.BasicStats.ManaRestoreOnCast_10_45 > 0)
			{
				float procsPerRotation = 0.1f * rotation.CastCount;
				float timeBetweenProcs = rotation.Duration / procsPerRotation;
				manaFromTrinket += calcs.BasicStats.ManaRestoreOnCast_10_45 / (45.0f + timeBetweenProcs) * rotation.Duration;
			}
            return manaFromTrinket;
        }
        private static void DoHasteProcCalcs(CharacterCalculationsMoonkin calcs, SpellRotation rotation, float hitRate, ref float effectiveSpellHaste)
        {
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
			// 10% chance of spell haste on cast, 45s cooldown
			if (calcs.BasicStats.SpellHasteFor10SecOnCast_10_45 > 0)
			{
                float procsPerRotation = 0.1f * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveSpellHaste += calcs.BasicStats.SpellHasteFor10SecOnCast_10_45 * 10.0f / (45.0f + timeBetweenProcs);
			}
        }
        private static void DoProcTrinketCalcs(CharacterCalculationsMoonkin calcs, SpellRotation rotation, float hitRate, float effectiveSpellCrit, ref float effectiveArcaneDamage, ref float effectiveNatureDamage, ref float trinketExtraDPS)
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
                double starfireCastsIn8Sec = 8.0 / SpellRotation.Starfire.CastTime;
                double uptime = 1 - Math.Pow((1 - 0.25), starfireCastsIn8Sec);
                effectiveArcaneDamage += calcs.BasicStats.DruidAshtongueTrinket * (float)uptime;
                effectiveNatureDamage += calcs.BasicStats.DruidAshtongueTrinket * (float)uptime;
            }
            // The Lightning Capacitor
            if (calcs.BasicStats.LightningCapacitorProc > 0)
            {
                float specialDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
                float baseDamage = (694 + 806) / 2.0f;
                float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
                float timeBetweenProcs = rotation.Duration / (hitRate * (calcs.SpellCrit + rotation.AverageCritChance) * rotation.CastCount);
                if (timeBetweenProcs < 2.5f) timeBetweenProcs = timeBetweenProcs * 3.0f + 2.5f;
                else timeBetweenProcs *= 3.0f;
                trinketExtraDPS += averageDamage / timeBetweenProcs;
            }
            // The Thunder Capacitor
            if (calcs.BasicStats.ThunderCapacitorProc > 0)
            {
                float specialDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusNatureDamageMultiplier);
                float baseDamage = (1181 + 1371) / 2.0f;
                float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
                float timeBetweenProcs = rotation.Duration / (hitRate * (calcs.SpellCrit + rotation.AverageCritChance) * rotation.CastCount);
                if (timeBetweenProcs < 2.5f) timeBetweenProcs = timeBetweenProcs * 4.0f + 2.5f;
                else timeBetweenProcs *= 4.0f;
                trinketExtraDPS += averageDamage / timeBetweenProcs;
            }
			// Pendulum of Telluric Currents (15% proc, 45s internal cooldown)
			if (calcs.BasicStats.PendulumOfTelluricCurrentsProc > 0)
			{
				float specialDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusShadowDamageMultiplier);
				float baseDamage = (1168 + 1752) / 2.0f;
				float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
				float timeBetweenProcs = rotation.Duration / (hitRate * rotation.CastCount * 0.15f) + 45f;
				trinketExtraDPS += averageDamage / timeBetweenProcs;
			}
            // Timbal's Focusing Crystal (10% proc on a DoT tick, 15s internal cooldown)
            if (calcs.BasicStats.TimbalsProc > 0 && rotation.TotalDotTicks > 0)
            {
                float specialDamageModifier = (1 + calcs.BasicStats.BonusShadowDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier);
                float baseDamage = (285 + 475) / 2.0f;
                float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
                float timeBetweenProcs = 1 / (rotation.TotalDotTicks / rotation.Duration * 0.1f) + 15.0f;
                trinketExtraDPS += averageDamage / timeBetweenProcs;
            }
            // Extract of Necromantic Power (10% proc on a DoT tick, 15s internal cooldown)
            if (calcs.BasicStats.ExtractOfNecromanticPowerProc > 0 && rotation.TotalDotTicks > 0)
            {
                float specialDamageModifier = (1 + calcs.BasicStats.BonusShadowDamageMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier);
                float baseDamage = (788 + 1312) / 2.0f;
                float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
                float timeBetweenProcs = 1 / (rotation.TotalDotTicks / rotation.Duration * 0.1f) + 15.0f;
                trinketExtraDPS += averageDamage / timeBetweenProcs;
            }
            // Darkmoon Card: Death (35% proc, 45s internal cooldown)
            if (calcs.BasicStats.DarkmoonCardDeathProc > 0)
            {
                float specialDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusShadowDamageMultiplier);
                float baseDamage = (744 + 956) / 2.0f;
                float averageDamage = hitRate * baseDamage * (1 + 0.5f * calcs.SpellCrit) * specialDamageModifier;
                float timeBetweenProcs = rotation.Duration / (hitRate * rotation.CastCount * 0.35f) + 45f;
                trinketExtraDPS += averageDamage / timeBetweenProcs;
            }
            // Spell damage for 10 seconds on resist
            if (calcs.BasicStats.SpellPowerFor10SecOnResist > 0)
            {
                float procsPerRotation = (1 - hitRate) * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveArcaneDamage += calcs.BasicStats.SpellPowerFor10SecOnResist * 10.0f / timeBetweenProcs;
                effectiveNatureDamage += calcs.BasicStats.SpellPowerFor10SecOnResist * 10.0f / timeBetweenProcs;
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
            if (calcs.BasicStats.SpellPowerFor10SecOnHit_10_45 > 0)
            {
                float procsPerRotation = 0.1f * hitRate * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveArcaneDamage += calcs.BasicStats.SpellPowerFor10SecOnHit_10_45 * 10.0f / (45.0f + timeBetweenProcs);
                effectiveNatureDamage += calcs.BasicStats.SpellPowerFor10SecOnHit_10_45 * 10.0f / (45.0f + timeBetweenProcs);
            }
            // 20% chance of spell damage on crit, 45 second cooldown.
            if (calcs.BasicStats.SpellPowerFor10SecOnCrit_20_45 > 0)
            {
                float procsPerRotation = 0.2f * hitRate * (effectiveSpellCrit / CalculationsMoonkin.critRatingConversionFactor) * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveArcaneDamage += calcs.BasicStats.SpellPowerFor10SecOnCrit_20_45 * 10.0f / (45.0f + timeBetweenProcs);
                effectiveNatureDamage += calcs.BasicStats.SpellPowerFor10SecOnCrit_20_45 * 10.0f / (45.0f + timeBetweenProcs);
            }
			// 15% chance of spell power for 10 seconds on cast, 45s cooldown.
			if (calcs.BasicStats.SpellPowerFor10SecOnCast_15_45 > 0)
			{
				float procsPerRotation = 0.15f * rotation.CastCount;
                float timeBetweenProcs = rotation.Duration / procsPerRotation;
                effectiveArcaneDamage += calcs.BasicStats.SpellPowerFor10SecOnCast_15_45 * 10.0f / (45.0f + timeBetweenProcs);
                effectiveNatureDamage += calcs.BasicStats.SpellPowerFor10SecOnCast_15_45 * 10.0f / (45.0f + timeBetweenProcs);
			}
        }
        private static void DoOnUseTrinketCalcs(CharacterCalculationsMoonkin calcs, float hitRate, ref float effectiveArcaneDamage, ref float effectiveNatureDamage, ref float effectiveSpellCrit, ref float effectiveSpellHaste, ref float trinketExtraDPS)
        {
            // Shatterered Sun Pendant (45s internal CD)
            if (calcs.BasicStats.ShatteredSunAcumenProc > 0)
            {
                if (calcs.Scryer)
                {
                    float specialDamageModifier = (1 + calcs.BasicStats.BonusSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusArcaneDamageMultiplier);
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
            if (calcs.BasicStats.SpellPowerFor20SecOnUse5Min > 0)
            {
                effectiveArcaneDamage += calcs.BasicStats.SpellPowerFor20SecOnUse5Min * 20.0f / 300.0f;
                effectiveNatureDamage += calcs.BasicStats.SpellPowerFor20SecOnUse5Min * 20.0f / 300.0f;
            }
        }

        private static void RecreateSpells(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            SpellRotation.RecreateSpells();
            CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
            // Moonfire glyph
            if (calcOpts.glyph1 == "Moonfire" || calcOpts.glyph2 == "Moonfire" || calcOpts.glyph3 == "Moonfire")
            {
                SpellRotation.Moonfire.SpecialDamageModifier -= 0.9f;
                SpellRotation.Moonfire.DoT.SpecialDamageMultiplier += 0.75f;
            }
            // Insect swarm glyph
            if (calcOpts.glyph1 == "Insect Swarm" || calcOpts.glyph2 == "Insect Swarm" || calcOpts.glyph3 == "Insect Swarm")
            {
                SpellRotation.InsectSwarm.DoT.SpecialDamageMultiplier += 0.3f;
            }
            SpellRotations = new List<SpellRotation>(new SpellRotation[]
            {
            new SpellRotation()
            {
                Name = "MF/SF",
                Spells = new List<Spell>(new Spell[]
                {
                    SpellRotation.Moonfire,
                    SpellRotation.Starfire
                })
            },
            new SpellRotation()
            {
                Name = "MF/W",
                Spells = new List<Spell>(new Spell[]
                {
                    SpellRotation.Moonfire,
                    SpellRotation.Wrath
                })
            },
            new SpellRotation()
            {
                Name = "IS/SF",
                Spells = new List<Spell>(new Spell[]
                {
                    SpellRotation.InsectSwarm,
                    SpellRotation.Starfire
                })
            },
            new SpellRotation()
            {
                Name = "IS/W",
                Spells = new List<Spell>(new Spell[]
                {
                    SpellRotation.InsectSwarm,
                    SpellRotation.Wrath
                })
            },
            new SpellRotation()
            {
                Name = "IS/MF/SF",
                Spells = new List<Spell>(new Spell[]
                {
                    SpellRotation.InsectSwarm,
                    SpellRotation.Moonfire,
                    SpellRotation.Starfire
                })
            },
            new SpellRotation()
            {
                Name = "IS/MF/W",
                Spells = new List<Spell>(new Spell[]
                {
                    SpellRotation.InsectSwarm,
                    SpellRotation.Moonfire,
                    SpellRotation.Wrath
                })
            },
            new SpellRotation()
            {
                Name = "SF Spam",
                Spells = new List<Spell>(new Spell[]
                {
                    SpellRotation.Starfire
                })
            },
            new SpellRotation()
            {
                Name = "W Spam",
                Spells = new List<Spell>(new Spell[]
                {
                    SpellRotation.Wrath
                })
            }
            });

            UpdateSpells(character, ref calcs);
        }
    }
}
