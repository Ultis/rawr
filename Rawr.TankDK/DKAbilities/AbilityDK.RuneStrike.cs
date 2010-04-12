using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Rune Strike Ability based on the AbilityDK_Base class.
    /// Strike the target for 150% weapon damage plus [150 * AP * 10 / 10000].  Only usable after the Death Knight dodges or parries.  Can't be dodged, blocked, or parried.  This attack causes a high amount of threat.
    /// </summary>
    class AbilityDK_RuneStrike : AbilityDK_Base
    {
        public AbilityDK_RuneStrike(Stats s, Weapon MH, Weapon OH)
        {
            this.sStats = s;
            this.wMH = MH;
            this.wOH = OH;
            this.szName = "Rune Strike";
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 20;
            this.uBaseDamage = 736; // May need to adjust this.
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.5f;
            this.DamageAdditiveModifer = 150 * (int)sStats.AttackPower * 10 / 10000;
            this.bTriggersGCD = false;
//            this.Cooldown = (uint)(wMH.baseSpeed * 1000);
            // TODO: Multi Target spell - need to add in the damage for the 2nd target.
        }
    }
}
