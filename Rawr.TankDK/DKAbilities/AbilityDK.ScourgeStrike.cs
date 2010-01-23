using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Scourge Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_ScourgeStrike : AbilityDK_Base
    {
        public AbilityDK_ScourgeStrike(Stats s, Weapon w)
        {
            this.sStats = s;
            this.wWeapon = w;
            this.szName = "Scourge Strike";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -15;
            this.uBaseDamage = 400;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = .5f;
            this.bTriggersGCD = true;
            // Physical Damage * .25 * # diseases on target as shadow.
        }

    }
}
