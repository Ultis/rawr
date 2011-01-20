using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Death Strike Ability based on the AbilityDK_Base class.
    /// A deadly attack that deals 150% weapon damage plus (((330  * 150 / 100))), healing you for 15% 
    /// of the damage you have sustained during the preceding 5 sec (minimum of at least 7% of your maximum health).
    /// </summary>
    class AbilityDK_DeathStrike : AbilityDK_Base
    {
        private int m_iToT = 0;
        public AbilityDK_DeathStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Death Strike";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -15;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1.5f;
            this.bTriggersGCD = true;
            this.AbilityIndex = (int)DKability.DeathStrike;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            if (CS.m_Spec == Rotation.Type.Blood)
                this.AbilityCost[(int)DKCostTypes.Death] = -2;
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
                this.m_iToT = CState.m_Talents.ThreatOfThassarian;

                uint WDam = (uint)((330 + this.wMH.damage) * this.fWeaponDamageModifier);
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
                WDam += (uint)(this.wOH.damage * iToTMultiplier * this.fWeaponDamageModifier * (1 + (CState.m_Talents.NervesOfColdSteel * .25 / 3)));
                return WDam;
            }
        }

        private float _DamageMultiplierModifier;
        public override float DamageMultiplierModifer
        {
            get
            {
                _DamageMultiplierModifier = base.DamageMultiplierModifer;
                _DamageMultiplierModifier += CState.m_Stats.BonusDeathStrikeDamage;
                if (CState.m_Stats.b2T11_Tank)
                    _DamageMultiplierModifier += .05f;

                _DamageMultiplierModifier += (this.CState.m_Talents.GlyphofDeathStrike ? Math.Max(.02f * CState.m_CurrentRP, .4f) : 0);
                return _DamageMultiplierModifier;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }

        /// <summary>
        /// The Crit Chance for the ability.  
        /// </summary>
        [Percentage]
        public override float CritChance
        {
            get
            {
                return Math.Max(1, .0065f + CState.m_Stats.PhysicalCrit + CState.m_Stats.BonusDeathStrikeCrit + StatConversion.NPC_LEVEL_CRIT_MOD[3]);
            }
        }
    }
}
