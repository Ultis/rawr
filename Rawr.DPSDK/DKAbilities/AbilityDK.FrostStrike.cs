using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Frost Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_FrostStrike : AbilityDK_Base
    {
        public AbilityDK_FrostStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Frost Strike";
            this.uBaseDamage = 0;
            this.tDamageType = ItemDamageType.Frost;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.3f;
            this.bTriggersGCD = true;
            this.AbilityIndex = (int)DKability.FrostStrike;
            UpdateCombatState(CS);
        }

        private int m_iToT = 0;

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.wMH = CS.MH;
            this.wOH = CS.OH;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = (40 - (CState.m_Talents.GlyphofFrostStrike ? 8 : 0));
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
                uint WDam = (uint)((277 + this.wMH.damage) * this.fWeaponDamageModifier);
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
                if (CState.m_Talents.MercilessCombat > 0)
                    DMM = DMM * (1 + ((CState.m_Talents.MercilessCombat * .06f) * .35f));
                return DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }

        
        public override float GetTotalDamage()
        {
            if (CState.m_Spec == Rotation.Type.Frost)
                return base.GetTotalDamage();
            else
                return 0;
        }

        private float _BonusCritChance;
        public override float CritChance
        {
            get
            {
                return Math.Min(1, base.CritChance + CState.m_Stats.BonusCritChanceFrostStrike + _BonusCritChance + (CState.m_Stats.b2T11_DPS ? .05f : 0));
            }

        }
        public void SetKMCritChance(float value)
        {
            _BonusCritChance = value;
        }
    }
}
