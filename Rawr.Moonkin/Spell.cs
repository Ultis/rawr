using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Moonkin
{

    // TODO:
    // Implement Wrath, Starfire, Moonfire, Insect Swarm
    // Wrath, Starfire: Cast time -(0.1 * Starlight Wrath)
    // Wrath, Starfire: Crit chance +(0.02 * Focused Starlight)
    // Moonfire: Damage, Crit chance +(0.05 * Imp Moonfire)
    // Starfire, Moonfire, Wrath: Crit damage +(0.2 * Vengeance)
    // All spells: If Nature's Grace, cast time -(0.5 * average crit chance of spell rotation)
    // Starfire, Moonfire, Wrath: Mana cost -(0.03 * Moonglow)
    // Starfire, Moonfire, Wrath: Damage +(0.02 * Moonfury)
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
        public int manaCost = 0;
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
                        manaCost = 255,
                        school = SpellSchool.Nature,
                        castTime = 2.0f,
                        damagePerHit = (381.0f + 429.0f) / 2.0f,
                        dotEffect = null,
                        critBonus = 150.0f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Starfire", new Spell()
                    {
                        manaCost = 370,
                        school = SpellSchool.Arcane,
                        castTime = 3.5f,
                        damagePerHit = (540.0f + 636.0f) / 2.0f,
                        dotEffect = null,
                        critBonus = 150.0f,
                        extraCritChance = 0.0f
                    });
                    spellList.Add("Moonfire", new Spell()
                    {
                        manaCost = 495,
                        school = SpellSchool.Arcane,
                        castTime = 0.0f,
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
                        manaCost = 175,
                        school = SpellSchool.Nature,
                        castTime = 0.0f,
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
    }

    class SpellRotation
    {
        private int damagePerMana = 0;
        private int damagePerSecond = 0;
    }
}