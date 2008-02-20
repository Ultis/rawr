using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{

    // TODO:
    // All spells: If Nature's Grace, cast time -(0.5 * average crit chance of spell rotation)
    // All spells: If Nature's Swiftness, instant cast every 3 min.
    // Spell rotation calculator

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
        public SpellSchool school = SpellSchool.Nature;
        public float manaCost = 0.0f;
        public float castTime = 0.0f;
        public float damagePerHit = 0.0f;
        public DotSpell dotEffect = null;
        public float critBonus = 150.0f;
        public float extraCritChance = 0.0f;
    }

    class MoonkinSpells
    {
        private Dictionary<string, Spell> spellList = null;
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
                        critBonus = 150.0f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Starfire", new Spell()
                    {
                        manaCost = 370.0f,
                        school = SpellSchool.Arcane,
                        castTime = 3.5f,
                        damagePerHit = (540.0f + 636.0f) / 2.0f,
                        dotEffect = null,
                        critBonus = 150.0f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Moonfire", new Spell()
                    {
                        manaCost = 495.0f,
                        school = SpellSchool.Arcane,
                        // Instant cast, but GCD is limiting factor
                        // This will change in 2.4 when haste affects GCD
                        castTime = 1.5f,
                        damagePerHit = (305.0f + 357.0f) / 2.0f,
                        dotEffect = new DotSpell()
                        {
                            tickLength = 3.0f,
                            numTicks = 4.0f,
                            damagePerTick = 150.0f
                        },
                        critBonus = 0.0f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Insect Swarm", new Spell()
                    {
                        manaCost = 175.0f,
                        school = SpellSchool.Nature,
                        // Instant cast, but GCD is limiting factor
                        // This will change in 2.4 when haste affects GCD
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

        public float[] GetDPSAndDPMRotations()
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

            // Create a "master list" of each spell rotation
            List<List<Spell>> spellRotations = new List<List<Spell>>();
            spellRotations.Add(MFSFx3W);
            spellRotations.Add(MFWx8);
            spellRotations.Add(ISMFSFx3);
            spellRotations.Add(ISMFWx7);
            spellRotations.Add(ISSFx3W);
            spellRotations.Add(ISSFx4);
            spellRotations.Add(ISWx8);
            spellRotations.Add(SFSpam);
            spellRotations.Add(WrathSpam);

            float[] results = new float[] { 0.0f, 0.0f };
            // Iterate through the spell rotation lists, finding the DPS and DPM of each rotation
            foreach (List<Spell> rotation in spellRotations)
            {
                float damagePerSecond = 0.0f;
                float damagePerMana = 0.0f;
                foreach (Spell sp in rotation)
                {
                    // Direct damage
                    damagePerSecond += sp.damagePerHit / sp.castTime;
                    damagePerMana += sp.damagePerHit / sp.manaCost;
                    // DoT damage
                    if (sp.dotEffect != null)
                    {
                        damagePerSecond += sp.dotEffect.damagePerTick / sp.dotEffect.tickLength;
                        damagePerMana += sp.dotEffect.damagePerTick * sp.dotEffect.numTicks / sp.manaCost;
                    }
                }
                if (damagePerSecond > results[0])
                {
                    results[0] = damagePerSecond;
                    results[1] = damagePerMana;
                }
            }

            return results;
        }
    }
}