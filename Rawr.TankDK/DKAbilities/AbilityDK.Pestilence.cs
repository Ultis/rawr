using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Howling Blast Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_Pestilence : AbilityDK_Base
    {
        public AbilityDK_Pestilence(Stats s)
        {
            this.sStats = s;
            this.szName = "Pestilence";
            this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uMinDamage = 0;
            this.uMaxDamage = 0;
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 0;
            this.bTriggersGCD = true;
            this.uRange = 0;
            this.uArea = 10;
            // TODO: AOE - need to have target numbers.
            // Glyph - Refreshes disease
            // Glyph - extends range

        }
    }
}
