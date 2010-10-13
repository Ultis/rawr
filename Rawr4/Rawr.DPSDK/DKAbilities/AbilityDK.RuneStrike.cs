using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Rune Strike Ability based on the AbilityDK_Base class.
    /// Strike the target for 150% weapon damage plus [150 * AP * 10 / 10000].  Only usable after the Death Knight dodges or parries.  Can't be dodged, blocked, or parried.  This attack causes a high amount of threat.
    /// </summary>
    class AbilityDK_RuneStrike : AbilityDK_Base
    {
        public AbilityDK_RuneStrike(CombatState CS)
        {
            this.CState = CS;
            this.wMH = CS.MH;
            this.wOH = CS.OH;
            this.szName = "Rune Strike";
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 20;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.5f;
            this.bTriggersGCD = true;
            m_iToT = CState.m_Talents.ThreatOfThassarian;
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
                uint WDam = (uint)(this.wMH.damage * this.fWeaponDamageModifier);
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

        private int _DamageAdditiveModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public int DamageAdditiveModifer
        {
            get
            {
                //this.DamageAdditiveModifer = //(2 *  AP * 10 / 100)
                return (int)(this.CState.m_Stats.AttackPower * 2.1);
            }
            set
            {
                _DamageAdditiveModifer = value;
            }
        }

        public override float CritChance
        {
            get
            {
                return base.CritChance + (CState.m_Talents.GlyphofRuneStrike ? .1f : 0);
            }
        }
    }
}
