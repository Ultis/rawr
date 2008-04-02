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

    class Rotation
    {
        public string Name = "";
        public float DPS = 0.0f;
        public float DPM = 0.0f;
        public TimeSpan TimeToOOM = new TimeSpan(0, 0, 0);
    }

    class DotSpell
    {
        public float spellDamageTickCoefficient = 1.0f;
        public float numTicks = 0.0f;
        public float damagePerTick = 0.0f;
        public float tickLength = 0.0f;
        public void AddSpellDamage(float spellDamageAdded)
        {
            damagePerTick += spellDamageTickCoefficient / numTicks * spellDamageAdded;
        }

        internal void RemoveSpellDamage(float spellDamageAdded)
        {
            damagePerTick += spellDamageTickCoefficient / numTicks * spellDamageAdded;
        }
    }
    class Spell
    {
        private static float globalCooldown = 1.5f;
        public static float GlobalCooldown
        {
            get
            {
                return globalCooldown;
            }
            set
            {
                if (value < 1.0f)
                    globalCooldown = 1.0f;
                else if (value > 1.5f)
                    globalCooldown = 1.5f;
                else
                    globalCooldown = value;
            }
        }
        public SpellSchool school = SpellSchool.Nature;
        public float manaCost = 0.0f;
        public float trueCastTime = 0.0f;
        public float castTime
        {
            get
            {
                return trueCastTime;
            }
            set
            {
                if (value < GlobalCooldown)
                    trueCastTime = GlobalCooldown;
                else
                    trueCastTime = value;
            }
        }
        public float spellDamageCoefficient = 1.0f;
        public void AddSpellDamage(float spellDamageAdded)
        {
            damagePerHit += spellDamageCoefficient * spellDamageAdded;
        }
        public float damagePerHit = 0.0f;
        public DotSpell dotEffect = null;
        public float critBonus = 150.0f;
        public float extraCritChance = 0.0f;

        internal void RemoveSpellDamage(float spellDamageAdded)
        {
            damagePerHit -= spellDamageCoefficient * spellDamageAdded;
        }
    }

    class MoonkinSpells : IEnumerable<KeyValuePair<string, Spell>>
    {
        private Dictionary<string, Spell> spellList = null;
        public string lastRotationName = "";
        public Spell this[string spellName]
        {
            get
            {
                if (spellList == null)
                {
                    spellList = new Dictionary<string, Spell>();
                    spellList.Add("Wrath", new Spell()
                    {
                        manaCost = 255.0f,
                        school = SpellSchool.Nature,
                        castTime = 2.0f,
                        damagePerHit = (381.0f + 429.0f) / 2.0f,
                        dotEffect = null,
                        critBonus = 1.50f,
                        extraCritChance = 0.0f,
                        spellDamageCoefficient = 0.571f
                    });
                    spellList.Add("Starfire", new Spell()
                    {
                        manaCost = 370.0f,
                        school = SpellSchool.Arcane,
                        castTime = 3.5f,
                        damagePerHit = (540.0f + 636.0f) / 2.0f,
                        dotEffect = null,
                        critBonus = 1.50f,
                        extraCritChance = 0.0f,
                        spellDamageCoefficient = 1.0f
                    });
                    spellList.Add("Moonfire", new Spell()
                    {
                        manaCost = 495.0f,
                        school = SpellSchool.Arcane,
                        // Instant cast, but GCD is limiting factor
                        castTime = Spell.GlobalCooldown,
                        damagePerHit = (305.0f + 357.0f) / 2.0f,
                        dotEffect = new DotSpell()
                        {
                            tickLength = 3.0f,
                            numTicks = 4.0f,
                            damagePerTick = 150.0f,
                            spellDamageTickCoefficient = 0.52f
                        },
                        critBonus = 1.50f,
                        extraCritChance = 0.0f,
                        spellDamageCoefficient = 0.15f
                    });
                    spellList.Add("Insect Swarm", new Spell()
                    {
                        manaCost = 175.0f,
                        school = SpellSchool.Nature,
                        // Instant cast, but GCD is limiting factor
                        castTime = Spell.GlobalCooldown,
                        // Using a 0% damage per hit should ensure that a "critical" insect swarm doesn't do any extra damage
                        damagePerHit = 0.0f,
                        dotEffect = new DotSpell()
                        {
                            tickLength = 2.0f,
                            numTicks = 6.0f,
                            damagePerTick = 132.0f,
                            spellDamageTickCoefficient = 0.76f
                        },
                        critBonus = 0.0f,
                        extraCritChance = 0.0f
                    });
                }
                return spellList[spellName];
            }
            set
            {
                spellList[spellName] = value;
            }
        }

        private Dictionary<string, List<Spell>> BuildSpellRotations()
        {
            // Build each spell rotation
            List<Spell> MFSFx3W = new List<Spell>(new Spell[] {
                spellList["Moonfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Wrath"]
            });
            List<Spell> ISMFSFx3 = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Moonfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"]
            });
            List<Spell> SFSpam = new List<Spell>(new Spell[] {
                spellList["Starfire"]
            });
            List<Spell> MFWx8 = new List<Spell>(new Spell[] {
                spellList["Moonfire"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"]
            });
            List<Spell> ISSFx4 = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"]
            });
            List<Spell> ISMFWx7 = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Moonfire"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"]
            });
            List<Spell> ISSFx3W = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Wrath"]
            });
            List<Spell> WrathSpam = new List<Spell>(new Spell[] {
                spellList["Wrath"]
            });
            List<Spell> ISWx8 = new List<Spell>(new Spell[] {
                spellList["Insect Swarm"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"],
                spellList["Wrath"]
            });
            List<Spell> MFSFx4 = new List<Spell>(new Spell[] {
                spellList["Moonfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"],
                spellList["Starfire"]
            });

            // Create a "master list" of spell rotations
            Dictionary<string, List<Spell>> spellRotations = new Dictionary<string, List<Spell>>();
            spellRotations.Add("MF/SFx4", MFSFx4);
            spellRotations.Add("MF/SFx3/W", MFSFx3W);
            spellRotations.Add("MF/Wx8", MFWx8);
            spellRotations.Add("IS/MF/SFx3", ISMFSFx3);
            spellRotations.Add("IS/MF/Wx7", ISMFWx7);
            spellRotations.Add("IS/SFx3/W", ISSFx3W);
            spellRotations.Add("IS/SFx4", ISSFx4);
            spellRotations.Add("IS/Wx8", ISWx8);
            spellRotations.Add("SF Spam", SFSpam);
            spellRotations.Add("W Spam", WrathSpam);
            return spellRotations;
        }

        private float GetEffectiveManaPool(Character character, CharacterCalculationsMoonkin calcs)
        {
            float fightLength = calcs.FightLength * 60.0f;

            float innervateCooldown = 360 - calcs.BasicStats.InnervateCooldownReduction;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen5SR * fightLength;

            // Mana pot calculations
            float manaPotDelay = float.Parse(character.CalculationOptions["ManaPotDelay"], System.Globalization.CultureInfo.InvariantCulture) * 60.0f;
            int numPots = character.CalculationOptions["ManaPots"] == "Yes" && fightLength - manaPotDelay > 0 ? ((int)(fightLength-manaPotDelay) / 120 + 1) : 0;
            float manaRestoredByPots = 0.0f;
            if (numPots > 0)
            {
                float manaPerPot = 0.0f;
                if (character.CalculationOptions["ManaPotType"] == "Super Mana Potion")
                    manaPerPot = 2400.0f;
                if (character.CalculationOptions["ManaPotType"] == "Fel Mana Potion")
                    manaPerPot = 3200.0f;

                manaRestoredByPots = numPots * manaPerPot;
            }

            // Innervate calculations
            float innervateDelay = float.Parse(character.CalculationOptions["InnervateDelay"], System.Globalization.CultureInfo.InvariantCulture) * 60.0f;
            int numInnervates = character.CalculationOptions["Innervate"] == "Yes" && fightLength - innervateDelay > 0 ? ((int)(fightLength-innervateDelay) / (int)innervateCooldown + 1) : 0;
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
                    float userIntellect = calcs.BasicStats.Intellect - character.MainHand.Stats.Intellect
                        + int.Parse(character.CalculationOptions["InnervateWeaponInt"], System.Globalization.CultureInfo.InvariantCulture);
                    if (character.OffHand != null)
                        userIntellect -= character.OffHand.Stats.Intellect;
                    // Do the same with spirit
                    float userSpirit = calcs.BasicStats.Spirit - character.MainHand.Stats.Spirit
                        + int.Parse(character.CalculationOptions["InnervateWeaponSpi"], System.Globalization.CultureInfo.InvariantCulture);
                    if (character.OffHand != null)
                        userIntellect -= character.OffHand.Stats.Spirit;
                    // The new spirit regen for innervate periods uses the new weapon stats
                    spiritRegen = baseRegenConstant * (float)Math.Sqrt(userIntellect) * userSpirit;
                }
                float innervateManaRate = spiritRegen * 4 + calcs.BasicStats.Mp5 / 5f;
                float innervateTime = numInnervates * 20.0f;
                totalInnervateMana = innervateManaRate * innervateTime - (numInnervates * calcs.BasicStats.Mana * 0.04f);
            }

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen + manaRestoredByPots;
        }

        public void GetRotation(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            // Nature's Grace haste time
            bool naturesGrace = int.Parse(character.CalculationOptions["NaturesGrace"]) > 0 ? true : false;

            // Calculate spell resists due to level
            int targetLevel = int.Parse(character.CalculationOptions["TargetLevel"]);
            float missRate = 0.05f;
            switch (targetLevel)
            {
                case 70:
                    missRate = 0.04f;
                    break;
                case 71:
                    missRate = 0.05f;
                    break;
                case 72:
                    missRate = 0.06f;
                    break;
                case 73:
                    missRate = 0.17f;
                    break;
            }
            missRate -= calcs.SpellHit;
            if (missRate < 0.01f) missRate = 0.01f;

            // Get total effective mana pool and total effective dps time
            float totalMana = GetEffectiveManaPool(character, calcs);
            float fightLength = calcs.FightLength * 60;

            Dictionary<string, List<Spell>> spellRotations = BuildSpellRotations();

            foreach (KeyValuePair<string, List<Spell>> rotation in spellRotations)
            {
                Rotation currentRotation = null;
                foreach (Rotation r in calcs.Rotations)
                {
                    if (r.Name == rotation.Key)
                    {
                        currentRotation = r;
                        break;
                    }
                }
                float averageCritChance = 0.0f;
                int spellCount = 0;
                foreach (Spell sp in rotation.Value)
                {
                    // Spells that do 0 damage are considered uncrittable in this simulation
                    if (sp.damagePerHit > 0.0f)
                        averageCritChance += calcs.SpellCrit + sp.extraCritChance;
                    ++spellCount;
                }
                averageCritChance /= spellCount;

                // Add trinket effects
                DoTrinkets(character, calcs, rotation.Value, averageCritChance, missRate);

                float damageDone = 0.0f;
                float manaUsed = 0.0f;
                float duration = 0.0f;
                float dotDuration = 0.0f;
                foreach (Spell sp in rotation.Value)
                {
                    DoSpellCalculations(sp, naturesGrace, averageCritChance, missRate, calcs, ref damageDone, ref manaUsed, ref duration, ref dotDuration);
                }
                // Handle the case where there is sufficient haste to add another spell cast in the DoT time
                // Right now, just automagically casts the last spell in the rotation
                while (dotDuration > rotation.Value[rotation.Value.Count - 1].castTime)
                {
                    DoSpellCalculations(rotation.Value[rotation.Value.Count - 1], naturesGrace, averageCritChance, missRate, calcs, ref damageDone, ref manaUsed, ref duration, ref dotDuration);
                }
                // Handle the case where DoTs overflow the cast times
                if (dotDuration > 0)
                    duration += dotDuration;

                // Add in JoW mana restore
                float manaFromJoW = 0.0f, manaFromOther = 0.0f, manaFromTrinket = 0.0f;
                if (calcs.BasicStats.ManaRestorePerCast > 0 || calcs.BasicStats.ManaRestorePerHit > 0 || calcs.BasicStats.Mp5OnCastFor20SecOnUse2Min > 0)
                {
                    int numCasts = (int)((fightLength / duration) * spellCount);
                    float numHits = numCasts * (1 - missRate);
                    manaFromOther = numCasts * calcs.BasicStats.ManaRestorePerCast;
                    manaFromJoW = numHits * calcs.BasicStats.ManaRestorePerHit;
                    // 20-second mp5 trinket with a 2-minute cooldown.
                    manaFromTrinket = calcs.BasicStats.Mp5OnCastFor20SecOnUse2Min * numCasts * 20 / 240;
                    totalMana += manaFromJoW + manaFromOther + manaFromTrinket;
                }
                // Calculate how long we will burn through all our mana
                float secsToOom = totalMana / (manaUsed / duration);
                // This dps calc takes into account time spent not doing dps due to OOM issues
                float dps = damageDone / duration * (secsToOom >= fightLength ? fightLength : secsToOom) / fightLength;
                float dpm = damageDone / manaUsed;
                calcs.DamageDone = damageDone;
                currentRotation.DPS = dps;
                currentRotation.DPM = dpm;
                if (secsToOom >= fightLength)
                    currentRotation.TimeToOOM = new TimeSpan(0, 0, 0);
                else
                    currentRotation.TimeToOOM = new TimeSpan(0, (int)Math.Floor(secsToOom) / 60, (int)Math.Floor(secsToOom) % 60);
                if (calcs.SelectedRotation == null || dps > calcs.SelectedRotation.DPS)
                {
                    calcs.SelectedRotation = currentRotation;
                }
                if (calcs.BasicStats.ManaRestorePerCast > 0 || calcs.BasicStats.ManaRestorePerHit > 0 || calcs.BasicStats.Mp5OnCastFor20SecOnUse2Min > 0)
                {
                    // Remove the mana added from JoW, it will change at the next cycle
                    totalMana -= manaFromJoW + manaFromOther + manaFromTrinket;
                }

                // Remove trinket effects
                UndoTrinkets(character, calcs, rotation.Value, averageCritChance, missRate);
            }
            calcs.SubPoints = new float[] { calcs.SelectedRotation.DPS * fightLength };
            calcs.OverallPoints = calcs.SubPoints[0];
        }

        private void UndoTrinkets(Character character, CharacterCalculationsMoonkin calcs, List<Spell> rotation, float averageCritChance, float missRate)
        {
            float hasteDivisor = 1560.0f;
            List<Spell> addedDamage = new List<Spell>();
            foreach (Spell sp in rotation)
            {
                // Again, only remove the bonus spell damage once
                if (!addedDamage.Contains(sp))
                {
                    // Remove moonfire proc bonus
                    if (rotation.Contains(this["Moonfire"]) && calcs.BasicStats.UnseenMoonDamageBonus > 0)
                    {
                        sp.RemoveSpellDamage(calcs.BasicStats.UnseenMoonDamageBonus * 0.5f);   // 50% chance to proc, NO COOLDOWN!
                        if (sp.dotEffect != null) sp.dotEffect.RemoveSpellDamage(calcs.BasicStats.UnseenMoonDamageBonus * 0.5f);
                    }

                    // Increased damage bonus from debuffs
                    if (sp.school == SpellSchool.Arcane)
                        sp.damagePerHit /= ((1 + calcs.BasicStats.BonusArcaneSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
                    else if (sp.school == SpellSchool.Nature)
                        sp.damagePerHit /= ((1 + calcs.BasicStats.BonusNatureSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));

                    // Spell damage for 10 seconds on resist
                    if (calcs.BasicStats.SpellDamageFor10SecOnResist > 0)
                    {
                        sp.RemoveSpellDamage(calcs.BasicStats.SpellDamageFor10SecOnResist * missRate);
                    }
                    // 15% chance of spell haste on cast, 45-second cooldown (Mystical Skyfire Diamond)
                    if (calcs.BasicStats.SpellHasteFor6SecOnCast_15_45 > 0)
                    {
                        float hasteRating = calcs.BasicStats.SpellHasteFor6SecOnCast_15_45 * 0.15f * 6 / 45;
                        Spell.GlobalCooldown *= 1 + hasteRating / hasteDivisor;
                        sp.castTime *= 1 + hasteRating / hasteDivisor;
                    }
                    // 10% chance of spell haste on hit, 45-second cooldown (Quagmirran's Eye)
                    if (calcs.BasicStats.SpellHasteFor6SecOnHit_10_45 > 0)
                    {
                        float hasteRating = calcs.BasicStats.SpellHasteFor6SecOnHit_10_45 * (1 - missRate) * 0.1f * 6 / 45;
                        Spell.GlobalCooldown *= 1 + hasteRating / hasteDivisor;
                        sp.castTime *= 1 + hasteRating / hasteDivisor;
                    }
                    // 5% chance of spell damage on hit, no cooldown.
                    if (calcs.BasicStats.SpellDamageFor10SecOnHit_5 > 0)
                    {
                        sp.RemoveSpellDamage(calcs.BasicStats.SpellDamageFor10SecOnHit_5 * (0.05f * (1 - missRate)));
                    }
                    // 10% chance of spell damage on hit, 45 second cooldown.
                    if (calcs.BasicStats.SpellDamageFor10SecOnHit_10_45 > 0)
                    {
                        sp.RemoveSpellDamage(calcs.BasicStats.SpellDamageFor10SecOnHit_10_45 * (0.1f * (1 - missRate)) / 4);
                    }
                    // 20% chance of spell damage on crit, 45 second cooldown.
                    if (calcs.BasicStats.SpellDamageFor10SecOnCrit_20_45 > 0)
                    {
                        sp.RemoveSpellDamage(calcs.BasicStats.SpellDamageFor10SecOnCrit_20_45 * (0.2f * (averageCritChance * (1 - missRate)) / 4));
                    }
                    addedDamage.Add(sp);
                }
            }
        }

        private void DoTrinkets(Character character, CharacterCalculationsMoonkin calcs, List<Spell> rotation, float averageCritChance, float missRate)
        {
            float hasteDivisor = 1560.0f;
            List<Spell> addedDamage = new List<Spell>();
            // Separate loop for doing trinket procs (yeah, I know, seems inefficient, but the average crit chance is needed)
            foreach (Spell sp in rotation)
            {
                if (!addedDamage.Contains(sp))    // Trinket procs, only add spell damage to spells that haven't been touched
                {
                    // Unseen Moon proc
                    if (rotation.Contains(this["Moonfire"]) && calcs.BasicStats.UnseenMoonDamageBonus > 0)
                    {
                        sp.AddSpellDamage(calcs.BasicStats.UnseenMoonDamageBonus * 0.5f);   // 50% chance to proc, NO COOLDOWN!
                        if (sp.dotEffect != null) sp.dotEffect.AddSpellDamage(calcs.BasicStats.UnseenMoonDamageBonus * 0.5f);
                    }

                    // Increased damage bonus from debuffs
                    if (sp.school == SpellSchool.Arcane)
                        sp.damagePerHit *= ((1 + calcs.BasicStats.BonusArcaneSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));
                    else if (sp.school == SpellSchool.Nature)
                        sp.damagePerHit *= ((1 + calcs.BasicStats.BonusNatureSpellPowerMultiplier) * (1 + calcs.BasicStats.BonusSpellPowerMultiplier));

                    // Spell damage for 10 seconds on resist
                    if (calcs.BasicStats.SpellDamageFor10SecOnResist > 0)
                    {
                        sp.AddSpellDamage(calcs.BasicStats.SpellDamageFor10SecOnResist * missRate);
                    }
                    // 15% spell haste on cast, 45-second cooldown (Mystical Skyfire Diamond)
                    if (calcs.BasicStats.SpellHasteFor6SecOnCast_15_45 > 0)
                    {
                        float hasteRating = calcs.BasicStats.SpellHasteFor6SecOnCast_15_45 * 0.15f * 6 / 45;
                        Spell.GlobalCooldown *= 1 + hasteRating / hasteDivisor;
                        sp.castTime *= 1 + hasteRating / hasteDivisor;
                    }
                    // 10% chance of spell haste on hit, 45-second cooldown (Quagmirran's Eye)
                    if (calcs.BasicStats.SpellHasteFor6SecOnHit_10_45 > 0)
                    {
                        float hasteRating = calcs.BasicStats.SpellHasteFor6SecOnHit_10_45 * (1 - missRate) * 0.1f * 6 / 45;
                        Spell.GlobalCooldown *= 1 + hasteRating / hasteDivisor;
                        sp.castTime *= 1 + hasteRating / hasteDivisor;
                    }
                    // 5% chance of spell damage on hit, no cooldown.
                    if (calcs.BasicStats.SpellDamageFor10SecOnHit_5 > 0)
                    {
                        sp.AddSpellDamage(calcs.BasicStats.SpellDamageFor10SecOnHit_5 * (0.05f * (1 - missRate)));
                    }
                    // 10% chance of spell damage on hit, 45 second cooldown.
                    if (calcs.BasicStats.SpellDamageFor10SecOnHit_10_45 > 0)
                    {
                        sp.AddSpellDamage(calcs.BasicStats.SpellDamageFor10SecOnHit_5 * (0.05f * (1 - missRate)));
                    }
                    // 20% chance of spell damage on crit, 45 second cooldown.
                    if (calcs.BasicStats.SpellDamageFor10SecOnCrit_20_45 > 0)
                    {
                        sp.AddSpellDamage(calcs.BasicStats.SpellDamageFor10SecOnCrit_20_45 * (0.2f * (averageCritChance * (1 - missRate)) / 4));
                    }
                    addedDamage.Add(sp);
                }
            }
        }

        private void DoSpellCalculations(Spell sp, bool naturesGrace, float averageCritChance, float missRate, CharacterCalculationsMoonkin calcs, ref float damageDone, ref float manaUsed, ref float duration, ref float dotDuration)
        {
            // Save/restore casting time because we only want to apply the effect once
            float oldCastTime = sp.castTime;
            if (naturesGrace)
            {
                sp.castTime -= 0.5f * averageCritChance;
            }
            float oldSpellDamage = 0.0f;
            if (dotDuration > 0 && sp.school == SpellSchool.Arcane && sp.dotEffect == null)
            {
                oldSpellDamage = sp.damagePerHit;
                // Add 4pc T5 bonus to Starfire spell
                sp.damagePerHit *= 1.1f;
            }
            // Calculate hits/crits/misses
            // Use a 2-roll system; crits only count if the spell hits, it's either a hit or a crit (not both)
            // Note: sp.DamagePerHit makes allowance for a DoT not being able to crit.
            float normalHitDamage = sp.damagePerHit * (1 - missRate - sp.extraCritChance - calcs.SpellCrit);
            float critHitDamage = sp.damagePerHit * sp.critBonus * ((1 - missRate) * (sp.extraCritChance + calcs.SpellCrit));

            damageDone += normalHitDamage + critHitDamage;
            manaUsed += sp.manaCost;
            if (sp.dotEffect != null)
            {
                damageDone += sp.dotEffect.damagePerTick * sp.dotEffect.numTicks;
                dotDuration = sp.dotEffect.numTicks * sp.dotEffect.tickLength;
            }
            if (dotDuration > 0)
            {
                dotDuration -= sp.castTime;
            }
            duration += sp.castTime;

            if (naturesGrace)
            {
                sp.castTime = oldCastTime;
            }
            if (oldSpellDamage > 0)
            {
                sp.damagePerHit = oldSpellDamage;
                oldSpellDamage = 0.0f;
            }
        }

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return spellList.GetEnumerator();
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,Spell>> Members

        IEnumerator<KeyValuePair<string, Spell>> IEnumerable<KeyValuePair<string, Spell>>.GetEnumerator()
        {
            return spellList.GetEnumerator();
        }

        #endregion

    }
}