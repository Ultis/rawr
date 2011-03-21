using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Festering Strike Ability based on the AbilityDK_Base class.
    /// Increases the duration of your Blood Plague, Frost Fever, and Chains of Ice effects on the target by up to 6 sec.
    /// </summary>
    class AbilityDK_FesteringStrike : AbilityDK_Base
    {
        public AbilityDK_FesteringStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Festering Strike";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.DamageAdditiveModifer = 560 * 150 / 100;
            this.fWeaponDamageModifier = 1.5f;
            this.bWeaponRequired = true;
            this.bTriggersGCD = true;
            this.uRange = MELEE_RANGE;
            this.AbilityIndex = (int)DKability.FesteringStrike;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            if (CS.m_Spec == Rotation.Type.Unholy)
                this.AbilityCost[(int)DKCostTypes.Death] = -2; 
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }

        public override float DamageMultiplierModifer
        {
            get
            {
                float DMM = base.DamageMultiplierModifer;
                DMM += (.12f * CState.m_Talents.RageOfRivendare);
                return DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }
    }
}
