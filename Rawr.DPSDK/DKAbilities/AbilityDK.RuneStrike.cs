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
            this.szName = "Rune Strike";
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 20;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.8f;
            this.bTriggersGCD = true;
            this.AbilityIndex = (int)DKability.RuneStrike;
            UpdateCombatState(CS);
        }

        private int m_iToT = 0;

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }

        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        override public uint uBaseDamage
        {
            get
            {
                m_iToT = CState.m_Talents.ThreatOfThassarian;
                uint WDam = (uint)(this.wMH.damage * this.fWeaponDamageModifier);
                // Off-hand damage is only effective if we have Threat of Thassaurian
                // And only for specific strikes as defined by the talent.
                float iToTMultiplier = 0;
                if (m_iToT > 0 && null != this.wOH) // DW
                {
                    if (m_iToT == 1)
                        iToTMultiplier = .30f;
                    if (m_iToT == 2)
                        iToTMultiplier = .60f;
                    if (m_iToT == 3)
                        iToTMultiplier = 1f;
                }
                if (this.wOH != null)
                    WDam += (uint)(this.wOH.damage * iToTMultiplier * this.fWeaponDamageModifier * (1 + (CState.m_Talents.NervesOfColdSteel * .25 / 3)));
                return WDam;
            }
        }

        private int _DamageAdditiveModifer = 0;
        /// <summary>Set up the modifier formula for a given ability</summary>
        /// (1.5 *  AP * 10 / 100)
        public override int DamageAdditiveModifer { get { return (int)(this.CState.m_Stats.AttackPower * 0.15f); } set { _DamageAdditiveModifer = value; } }

        public override float CritChance { get { return base.CritChance + (CState.m_Talents.GlyphofRuneStrike ? 0.10f : 0); }
        }
    }
}
