using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Death Strike Ability based on the AbilityDK_Base class.
    /// A deadly attack that deals 150% weapon damage plus (((330  * 150 / 100))), healing you for 30% 
    /// of the damage you have sustained during the preceding 5 sec (minimum of at least 10% of your maximum health).
    /// </summary>
    class AbilityDK_DeathStrike : AbilityDK_Base
    {
        private int m_iToT = 0;
        public AbilityDK_DeathStrike(CombatState CS)
        {
            this.CState = CS;
            this.wMH = CS.MH;
            this.wOH = CS.OH;
            this.szName = "Death Strike";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -15;
            this.DamageAdditiveModifer = (330 * 150 / 100);
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.5f;
            this.bTriggersGCD = true;
//            this.CState.m_Stats.HealthRestoreFromMaxHealth += (.05f /* * # of diseases on target */);
            m_iToT = CState.m_Talents.ThreatOfThassarian;
        }

        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        override public uint uBaseDamage
        {
            get
            {
                uint WDam = base.uBaseDamage;
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
            set
            {
                // Setup so that we can just set a base damage w/o having to 
                // manually set Min & Max all the time.
                uMaxDamage = uMinDamage = value;
            }
        }
    }
}
