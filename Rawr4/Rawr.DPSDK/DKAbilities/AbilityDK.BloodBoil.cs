using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the BloodBoil Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_BloodBoil : AbilityDK_Base
    {
        public AbilityDK_BloodBoil(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Blood Boil";
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = 357;
            this.tDamageType = ItemDamageType.Shadow;
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 0;
            this.bTriggersGCD = true;
            this.uRange = 0;
            this.bAOE = true;
            this.AbilityIndex = (int)DKability.BloodBoil;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            // TODO: Refactor this out for rotation specific PS -> BB when BP already on target.
            this.uArea = (uint)(10 + (CState.m_Talents.GlyphofBloodBoil ? 5 : 0));
            if ((CState.m_Talents.CrimsonScourge == 2)
                && (CState.m_uDiseaseCount >= 2))
            {
                this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            }
            else
                this.AbilityCost[(int)DKCostTypes.Blood] = 1;

        }

        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public int DamageAdditiveModifer
        {
            get
            {
                // AP Scaling
                int AdditionalDamage = (int)(this.CState.m_Stats.AttackPower * 0.08);
                // Additional Disease Damage:
                if (CState.m_uDiseaseCount > 0)
                    AdditionalDamage += (int)(this.CState.m_Stats.AttackPower * 0.035);
                return AdditionalDamage + base.DamageAdditiveModifer;
            }
            set
            {
                base.DamageAdditiveModifer = value;
            }
        }

        override public float DamageMultiplierModifer
        {
            get
            {
                if (CState.m_Talents.CrimsonScourge > 0)
                {
                    return base.DamageMultiplierModifer + (CState.m_Talents.CrimsonScourge * .2f);
                }
                else
                    return base.DamageMultiplierModifer;
            }
        }
    }
}
