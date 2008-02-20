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
    // Starfire: Damage +(0.04 * Wrath of Cenarius)
    // Moonfire: Damage +(0.02 * Wrath of Cenarius)
    // All spells: If Nature's Swiftness, instant cast every 3 min.
    // Spell rotation calculator

    class Spell
    {
        private int baseMana;
        private float baseCastTime;
        private int baseDamage;
    }

}