using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Blood Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_BloodStrike : AbilityDK_Base
    {
        public AbilityDK_BloodStrike(CombatState CS)
        {
            this.CState = CS;
            this.wMH = CS.MH;
            this.wOH = CS.OH;
            this.szName = "Blood Strike";
            this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = (uint)Math.Floor(850 * .8);
            this.fWeaponDamageModifier = 0.8f;
            this.bWeaponRequired = true;
            this.bTriggersGCD = true;
            m_iToT = CState.m_Talents.ThreatOfThassarian;

        }

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public float DamageMultiplierModifer
        {
            get
            {
                return (CState.m_uDiseaseCount * .1f) + _DamageMultiplierModifer + base.DamageMultiplierModifer;
            }
            set
            {
                _DamageMultiplierModifer = value;
            }
        }

        private int m_iToT = 0;

        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        override public uint uBaseDamage
        {
            get
            {
                uint WDam = (uint)((850 + this.wMH.damage) * this.fWeaponDamageModifier);
                // Off-hand damage is only effective if we have Threat of Thassaurian
                // And only for specific strikes as defined by the talent.
                if (m_iToT > 0 && null != this.wOH) // DW
                {
                    float iToTMultiplier = 0;
                    if (m_iToT == 1)
                        iToTMultiplier = .30f;
                    if (m_iToT == 2)
                        iToTMultiplier = .60f;
                    if (m_iToT == 3)
                        iToTMultiplier = 1f;
                    WDam += (uint)(this.wOH.damage * iToTMultiplier * this.fWeaponDamageModifier);
                }
                return WDam;
            }
        }
    }
}
