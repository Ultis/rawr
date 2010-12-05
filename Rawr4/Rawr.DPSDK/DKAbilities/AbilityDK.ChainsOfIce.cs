using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the IcyTouch Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_ChainsOfIce : AbilityDK_Base
    {
        /// <summary>
        /// Chills the target for 227 to 245 Frost damage and  infects 
        /// them with Frost Fever, a disease that deals periodic damage 
        /// and reduces melee and ranged attack speed by 14% for 15 sec.
        /// </summary>
        public AbilityDK_ChainsOfIce(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Chains of Ice";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10 + (CS.m_Talents.ChillOfTheGrave > 0 ? -5 : 0);
            this.uMaxDamage = (CS.m_Talents.GlyphofChainsofIce ? 156u : 0u);
            this.uMinDamage = (CS.m_Talents.GlyphofChainsofIce ? 144u : 0u);
            this.uRange = 20;
            this.tDamageType = ItemDamageType.Frost;
            this.bTriggersGCD = true;
            this.ml_TriggeredAbility = new AbilityDK_Base[1];
            this.ml_TriggeredAbility[0] = new AbilityDK_FrostFever(CS);
        }

        private int _DamageAdditiveModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public int DamageAdditiveModifer
        {
            get
            {
                //this.DamageAdditiveModifer = //[AP * 0.055 * 1.15]
                if (CState.m_Talents.GlyphofChainsofIce)
                    return (int)(this.CState.m_Stats.AttackPower * .2) + base.DamageAdditiveModifer;
                else
                    return 0;
            }
            set
            {
                _DamageAdditiveModifer = value;
            }
        }
    }
}
