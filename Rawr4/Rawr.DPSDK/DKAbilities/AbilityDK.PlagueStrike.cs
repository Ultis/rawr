using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Plague Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_PlagueStrike : AbilityDK_Base
    {
        public AbilityDK_PlagueStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Plague Strike";
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1f;
            this.bTriggersGCD = true;
            this.ml_TriggeredAbility = new AbilityDK_Base[1];
            this.AbilityIndex = (int)DKability.PlagueStrike;
            UpdateCombatState(CS);
        }

        private int m_iToT = 0;

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.ml_TriggeredAbility[0] = new AbilityDK_BloodPlague(CS);
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
                uint WDam = (uint)((420 + this.wMH.damage) * this.fWeaponDamageModifier);
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
