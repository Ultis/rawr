using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Death Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_DeathStrike : AbilityDK_Base
    {
        public AbilityDK_DeathStrike(Stats s, Weapon MH, Weapon OH)
        {
            this.sStats = s;
            this.wMH = MH;
            this.wOH = OH;
            this.szName = "Death Strike";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -15;
            this.uBaseDamage = 223;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = .75f;
            this.bTriggersGCD = true;
            this.sStats.HealthRestoreFromMaxHealth += (.05f /* * # of diseases on target */);
        }
    }
}
