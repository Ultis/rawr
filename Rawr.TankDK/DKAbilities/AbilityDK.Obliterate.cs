using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Obliterate Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_Obliterate : AbilityDK_Base
    {
        public AbilityDK_Obliterate(Stats s, Weapon MH, Weapon OH)
        {
            this.sStats = s;
            this.wMH = MH;
            this.wOH = OH;
            this.szName = "Obliterate";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -15;
            this.uBaseDamage = (uint)(584 * .8f);
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = .8f;
            this.bTriggersGCD = true;
            // Physical Damage * .125 * # diseases on target may consume the diseases.
        }

    }
}
