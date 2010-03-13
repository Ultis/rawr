using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Heart Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_HeartStrike : AbilityDK_Base
    {
        public AbilityDK_HeartStrike(Stats s, Weapon MH, Weapon OH)
        {
            this.sStats = s;
            this.wMH = MH;
            this.wOH = OH;
            this.szName = "Heart Strike";
            this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = 736; // May need to adjust this.
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = .5f;
            this.bTriggersGCD = true;
            // TODO: Multi Target spell - need to add in the damage for the 2nd target.
        }
    }
}
