using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Heart Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_FrostStrike : AbilityDK_Base
    {
        public AbilityDK_FrostStrike(Stats s, Weapon MH, Weapon OH)
        {
            this.sStats = s;
            this.wMH = MH;
            this.wOH = OH;
            this.szName = "Frost Strike";
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 40;
            this.uMinDamage = 137;
            this.uMaxDamage = 138;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = .55f;
            this.bTriggersGCD = true;
            // TODO: Multi Target spell - need to add in the damage for the 2nd target.
        }
    }
}
