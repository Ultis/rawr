using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the IcyTouch Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_IcyTouch : AbilityDK_Base
    {
        /// <summary>
        /// Chills the target for 227 to 245 Frost damage and  infects 
        /// them with Frost Fever, a disease that deals periodic damage 
        /// and reduces melee and ranged attack speed by 14% for 15 sec.
        /// </summary>
        public AbilityDK_IcyTouch(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Icy Touch";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.uMaxDamage = 505 / 2;
            this.uMinDamage = 547 / 2;
            this.tDamageType = ItemDamageType.Frost;
            this.bTriggersGCD = true;
            this.ml_TriggeredAbility = new AbilityDK_Base[1];
            this.AbilityIndex = (int)DKability.IcyTouch;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10 + (CS.m_Talents.ChillOfTheGrave > 0 ? -5 : 0);
            this.ml_TriggeredAbility[0] = new AbilityDK_FrostFever(CS);
            this.uRange = 20 + (uint)(CState.m_Talents.IcyReach * 5);
        }

        private int _DamageAdditiveModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public int DamageAdditiveModifer
        {
            get
            {
                return (int)(this.CState.m_Stats.AttackPower * .2);
            }
            set
            {
                _DamageAdditiveModifer = value;
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
    }
}
