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


    class DotSpell
    {
        public float numTicks = 0.0f;
        public float damagePerTick = 0.0f;
        public float tickLength = 0.0f;
    }
    class Spell
    {
        // This field will become modifiable via a property in 2.4, according to spell haste
        public static float GlobalCooldown = 1.5f;
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
        public float damagePerHit = 0.0f;
        public DotSpell dotEffect = null;
        public float critBonus = 150.0f;
        public float extraCritChance = 0.0f;
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
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Starfire", new Spell()
                    {
                        manaCost = 370.0f,
                        school = SpellSchool.Arcane,
                        castTime = 3.5f,
                        damagePerHit = (540.0f + 636.0f) / 2.0f,
                        dotEffect = null,
                        critBonus = 1.50f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Moonfire", new Spell()
                    {
                        manaCost = 495.0f,
                        school = SpellSchool.Arcane,
                        // Instant cast, but GCD is limiting factor
                        castTime = 1.5f,
                        damagePerHit = (305.0f + 357.0f) / 2.0f,
                        dotEffect = new DotSpell()
                        {
                            tickLength = 3.0f,
                            numTicks = 4.0f,
                            damagePerTick = 150.0f
                        },
                        critBonus = 1.50f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Insect Swarm", new Spell()
                    {
                        manaCost = 175.0f,
                        school = SpellSchool.Nature,
                        // Instant cast, but GCD is limiting factor
                        castTime = 1.5f,
                        // Using a 0% damage per hit should ensure that a "critical" insect swarm doesn't do any extra damage
                        damagePerHit = 0.0f,
                        dotEffect = new DotSpell()
                        {
                            tickLength = 2.0f,
                            numTicks = 6.0f,
                            damagePerTick = 132.0f
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

            // Create a "master list" of spell rotations
            Dictionary<string, List<Spell>> spellRotations = new Dictionary<string, List<Spell>>();
            spellRotations.Add("MF/SFx3/W", MFSFx3W);
            spellRotations.Add("MF/Wx8", MFWx8);
            spellRotations.Add("IS/MF/SFx3", ISMFSFx3);
            spellRotations.Add("IS/MF/Wx7", ISMFWx7);
            spellRotations.Add("IS/SFx3/W", ISSFx3W);
            spellRotations.Add("IS/SFx4", ISSFx4);
            spellRotations.Add("IS/Wx8", ISWx8);
            spellRotations.Add("Starfire Spam", SFSpam);
            spellRotations.Add("Wrath Spam", WrathSpam);
            return spellRotations;
        }

        private float GetEffectiveManaPool(Character character, CharacterCalculationsMoonkin calcs)
        {
            float fightLength = calcs.FightLength * 60.0f;

            // Mana/5 calculations
            float totalManaRegen = calcs.ManaRegen5SR * fightLength;

            // Innervate calculations
            float innervateManaRate = calcs.ManaRegen * 5;
            float innervateTime = character.CalculationOptions["Innervate"] == "Yes" ? ((int)calcs.FightLength / 6 + 1) * 20.0f : 0.0f;
            float totalInnervateMana = innervateManaRate * innervateTime;

            return calcs.BasicStats.Mana + totalInnervateMana + totalManaRegen;
        }

        public void GetRotation(Character character, ref CharacterCalculationsMoonkin calcs)
        {
            // Nature's Swiftness procs
            bool naturesSwiftness = int.Parse(character.CalculationOptions["NaturesSwiftness"]) > 0 ? true : false;
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
            float innervateTime = character.CalculationOptions["Innervate"] == "Yes" ? ((int)calcs.FightLength / 6 + 1) * 20.0f : 0.0f;
            float dpsTime = calcs.FightLength * 60 - innervateTime;

            float totalMana = GetEffectiveManaPool(character, calcs);

            Dictionary<string, List<Spell>> spellRotations = BuildSpellRotations();

            int instantCasts = naturesSwiftness ? (int)Math.Floor(dpsTime) % 3 : 0;

            foreach (KeyValuePair<string, List<Spell>> rotation in spellRotations)
            {
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
                if (naturesGrace)
                {
                    foreach (Spell sp in rotation.Value)
                    {
                        // Should properly reduce the average cast time of a cycle based on average
                        // expected Nature's Grace uptime
                        sp.castTime -= 0.5f * averageCritChance;
                    }
                }

                float damageDone = 0.0f;
                float manaUsed = 0.0f;
                float duration = 0.0f;
                foreach (Spell sp in rotation.Value)
                {
                    // Calculate hits/crits
                    damageDone += sp.damagePerHit + sp.damagePerHit * sp.critBonus * (sp.extraCritChance + calcs.SpellCrit);
                    manaUsed += sp.manaCost;
                    if (sp.dotEffect != null)
                    {
                        damageDone += sp.dotEffect.damagePerTick * sp.dotEffect.numTicks;
                    }
                    duration += sp.castTime;
                }
                // Reduce damage by number of spells that missed
                damageDone *= 1 - missRate;
                // Calculate how long we will burn through all our mana
                float secsToOom = totalMana / (manaUsed / duration);
                // This dps calc takes into account time spent not doing dps due to OOM issues
                float dps = damageDone / duration * (secsToOom >= dpsTime ? dpsTime : secsToOom) / dpsTime;
                float mps = manaUsed / duration;
                if (dps > calcs.DPS)
                {
                    calcs.DPS = dps;
                    calcs.MPS = mps;
                    calcs.RotationName = rotation.Key;
                    if (secsToOom >= dpsTime)
                        calcs.TimeToOOM = new TimeSpan(0, 0, 0);
                    else
                        calcs.TimeToOOM = new TimeSpan(0, (int)Math.Floor(secsToOom) / 60, (int)Math.Floor(secsToOom) % 60);
                }
            }

            calcs.SubPoints = new float[] { calcs.DPS, calcs.MPS };
            calcs.OverallPoints = calcs.SubPoints[0] + calcs.SubPoints[1];
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