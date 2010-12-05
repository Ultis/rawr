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
            this.wMH = CS.MH;
            this.wOH = CS.OH;
            this.szName = "Frost Strike";
            this.AbilityCost[(int)DKCostTypes.RunicPower] = (40 - (CState.m_Talents.GlyphofFrostStrike ? 8 : 0));
            this.uBaseDamage = 0;
            this.tDamageType = ItemDamageType.Frost;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.1f;
            this.bTriggersGCD = true;
            m_iToT = CState.m_Talents.ThreatOfThassarian;
            this.AbilityIndex = (int)DKability.FrostStrike;

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
                uint WDam = (uint)((277 + this.wMH.damage) * this.fWeaponDamageModifier);
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

        public override float DamageMultiplierModifer
        {
            get
            {
                float DMM = base.DamageMultiplierModifer;
                if (CState.m_Talents.MercilessCombat > 0)
                {
                    DMM = DMM * (1 + ((CState.m_Talents.MercilessCombat * .06f) * .35f));
                }
                return DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }

        
        public override int GetTotalDamage()
        {
            if (CState.m_Spec == Rotation.Type.Frost)
                return base.GetTotalDamage();
            else
                return 0;
        }
    }
}
