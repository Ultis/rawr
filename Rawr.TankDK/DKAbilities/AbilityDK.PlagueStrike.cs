using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Plague Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_PlagueStrike : AbilityDK_Base
    {
        public AbilityDK_PlagueStrike(Stats s, Weapon MH, Weapon OH)
        {
            this.sStats = s;
            this.wMH = MH;
            this.wOH = OH;
            this.szName = "Plague Strike";
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = 189;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = .5f;
            this.bTriggersGCD = true;
        }

    }
}
