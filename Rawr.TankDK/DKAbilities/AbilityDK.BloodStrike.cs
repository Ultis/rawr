using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Blood Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_BloodStrike : AbilityDK_Base
    {
        public AbilityDK_BloodStrike(Stats s, Weapon w)
        {
            this.sStats = s;
            this.wWeapon = w;
            this.szName = "Blood Strike";
            this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = 306;
            this.fWeaponDamageModifier = 0.4f;
            this.bWeaponRequired = true;
            this.bTriggersGCD = true;
            // Damage increased by 12.5% per disease on target.
        }
    }
}
