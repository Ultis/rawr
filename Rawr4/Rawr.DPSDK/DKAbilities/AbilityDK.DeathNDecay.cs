using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Howling Blast Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_DeathNDecay : AbilityDK_Base
    {
        public AbilityDK_DeathNDecay(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Death N Decay";
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = 46;
            this.tDamageType = ItemDamageType.Shadow;
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 0;
            this.bTriggersGCD = true;
            this.uRange = 30;
            this.uArea = 10;
            this.uTickRate = 1 * 1000;
            this.uDuration = (uint)(10 * 1000 * (CS.m_Talents.GlyphofDeathandDecay ? 1.5 : 1));
            this.Cooldown = 30 * 1000;
            this.bAOE = true;
            this.AbilityIndex = (int)DKability.DeathNDecay;

        }

        private int _DamageAdditiveModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public int DamageAdditiveModifer
        {
            get
            {
                //this.DamageAdditiveModifer = [AP * 0.064]
                return (int)(this.CState.m_Stats.AttackPower * .064) + this._DamageAdditiveModifer;
            }
            set
            {
                _DamageAdditiveModifer = value;
            }
        }
    
    }
}
