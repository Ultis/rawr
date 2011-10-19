using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Obliterate Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_Obliterate : AbilityDK_Base
    {
        public AbilityDK_Obliterate(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Obliterate";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.5f;
            this.bTriggersGCD = true;
            // Physical Damage * .125 * # diseases on target may consume the diseases.
            this.AbilityIndex = (int)DKability.Obliterate;
            UpdateCombatState(CS);
        }

        private int m_iToT = 0;
        
        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -20 + (CState.m_Talents.ChillOfTheGrave * -5);
            this.wMH = CState.MH;
            this.wOH = CState.OH;
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
                uint WDam = (uint)((650 + this.wMH.damage) * this.fWeaponDamageModifier);
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

        private float _DamageMultiplierModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public float DamageMultiplierModifer
        {
            get
            {
                float multiplier = 1 + (CState.m_uDiseaseCount * .125f); 
                multiplier *= 1 + _DamageMultiplierModifer;
                multiplier *= 1 + base.DamageMultiplierModifer;
                multiplier *= 1 + (CState.m_Talents.GlyphofObliterate ? .20f : 0);
                multiplier *= 1 + (CState.m_Talents.Annihilation * 0.15f);
                multiplier *= (1 + ((CState.m_Talents.MercilessCombat * .06f) * .35f));
                return multiplier - 1 ;
            }
        }

        private float _BonusCritChance = 0;
        public override float CritChance
        {
            get
            {
                return Math.Min(1, base.CritChance * (1 + CState.m_Stats.BonusCritChanceObliterate) * (1 + _BonusCritChance));
            }
        }
        public void SetKMCritChance(float value)
        {
            _BonusCritChance = value;
        }

        public override float GetTickDamage()
        {
            float baseTickDamage = base.GetTickDamage();
            float FireBonusDamage = CState.m_Stats.b4T12_DPS ? baseTickDamage * .06f : 0;
            return baseTickDamage + (FireBonusDamage * (1 + CState.m_Stats.BonusFireDamageMultiplier));
        }
    }
}
