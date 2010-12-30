using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Necrotic Strike Ability based on the AbilityDK_Base class.
    /// absorbs the next (0.75 *  AP) healing received by the target.  
    /// For 15 sec, or until the full amount of healing is absorbed, the target's casting time is increased by 30%.
    /// </summary>
    class AbilityDK_NecroticStrike : AbilityDK_Base
    {
        public AbilityDK_NecroticStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Necrotic Strike";
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1f;
            // Absorbs healing on a target.
            this.bTriggersGCD = true;
            this.AbilityIndex = (int)DKability.NecroticStrike;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }
    }
}
