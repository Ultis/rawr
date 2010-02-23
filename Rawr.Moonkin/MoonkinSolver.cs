using System;
using System.Collections.Generic;

namespace Rawr.Moonkin
{
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
            float treeDamage = (talents.ForceOfNature == 1) ? DoTreeCalcs(baseSpellPower, calcs.BasicStats.PhysicalHaste, calcs.BasicStats.ArmorPenetration, calcs.BasicStats.PhysicalCrit, calcOpts.TreantLifespan, character.DruidTalents.Brambles) : 0.0f;
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
                if (timeToOOM <= 0) timeToOOM = calcs.FightLength * 60.0f;   // Happens when ManaUsed is less than 0
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
        private float DoTreeCalcs(float effectiveNatureDamage, float meleeHaste, float armorPen, float meleeCrit, float treantLifespan, int bramblesLevel)
        {
            // 642 = base AP, 57% spell power scaling
            float attackPower = 642.0f + (float)Math.Floor(0.57f * effectiveNatureDamage);
            // 398.8 = base DPS, 1.7 = best observed swing speed
            float damagePerHit = (398.8f + attackPower / 14.0f) * 1.7f;
            float critRate = 0.05f + meleeCrit;
            float glancingRate = 0.2f;
            float bossArmor = StatConversion.NPC_ARMOR[83 - 80] * (1.0f - armorPen);
            float damageReduction = bossArmor / (bossArmor + 15232.5f);
            damagePerHit *= 1.0f - damageReduction;
            damagePerHit = (critRate * damagePerHit * 2.0f) + (glancingRate * damagePerHit * 0.75f) + ((1 - critRate - glancingRate) * damagePerHit);
            float attackSpeed = 1.7f / (1 + meleeHaste);
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
            Starfire.CriticalDamageModifier = stats.BonusCritMultiplier > 0 ? 1.5f * (1 + stats.BonusCritMultiplier) : 1.5f;
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