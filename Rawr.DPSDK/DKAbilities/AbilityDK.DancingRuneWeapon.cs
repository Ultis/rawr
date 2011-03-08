using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Death Coil Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_DancingRuneWeapon : AbilityDK_Base
    {
        public AbilityDK_DancingRuneWeapon(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Dancing Rune Weapon";
            this.bWeaponRequired = true;
            this.bTriggersGCD = true;
            this.AbilityIndex = (int)DKability.DRW;
            this.Cooldown = (int)(1.5 * 60 * 1000);
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 60;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
        }
    }    
}
