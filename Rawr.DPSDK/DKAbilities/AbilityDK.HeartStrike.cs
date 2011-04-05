using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Heart Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_HeartStrike : AbilityDK_Base
    {
        public AbilityDK_HeartStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Heart Strike";
            this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.DamageAdditiveModifer = 736;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.75f;
            this.bTriggersGCD = true;
            this.bAOE = true;
            this.AbilityIndex = (int)DKability.HeartStrike;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }

        private float _HSDamageMultiplierModifer = -1f;
        /// <summary>Setup the modifier formula for a given ability</summary>
        public override float DamageMultiplierModifer {
            get {
                if (_HSDamageMultiplierModifer == -1f) {
                    _HSDamageMultiplierModifer = base.DamageMultiplierModifer * (1f + (this.CState.m_Talents.GlyphofHeartStrike ? 0.30f : 0f));
                }
                //
                float multiplier = _HSDamageMultiplierModifer * (1f + (CState.m_uDiseaseCount * .15f));
                float targetsToProcess = CState.m_NumberOfTargets;
                while (targetsToProcess > 0) {
                    if (targetsToProcess > 1) {
                        // Handles Full
                        multiplier *= (1f + (1f - 0.25f));
                    } else {
                        // Handles Partial
                        multiplier *= (1f + (1f - 0.25f) * (1f - targetsToProcess));
                    }
                    targetsToProcess--;
                }
                //
                return multiplier;
            }
        }

        public override float GetTotalDamage()
        {
            if (CState.m_Spec == Rotation.Type.Blood)
                return base.GetTotalDamage();
            else
                return 0;
        }
    }
}
