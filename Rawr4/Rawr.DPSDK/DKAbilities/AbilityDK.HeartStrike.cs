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
            this.wMH = CS.MH;
            this.wOH = CS.OH;
            this.szName = "Heart Strike";
            this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.DamageAdditiveModifer = 819;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1f;
            this.bTriggersGCD = true;
            this.bAOE = true;
        }

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public float DamageMultiplierModifer
        {
            get
            {
                _DamageMultiplierModifer += base.DamageMultiplierModifer + (this.CState.m_Talents.GlyphofHeartStrike ? .3f : 0);
                float multiplier = (CState.m_uDiseaseCount * .1f) + _DamageMultiplierModifer;
                if (CState.m_NumberOfTargets > 1)
                { multiplier *= 1.75f; }
                if (CState.m_NumberOfTargets > 2)
                { multiplier *= 1.75f; }
                return multiplier;
            }
        }
    }
}
