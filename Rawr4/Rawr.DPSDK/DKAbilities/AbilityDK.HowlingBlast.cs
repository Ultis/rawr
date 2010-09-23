using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Howling Blast Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_HowlingBlast : AbilityDK_Base
    {
        public AbilityDK_HowlingBlast(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Howling Blast";
            this.AbilityCost[(int)DKCostTypes.Frost] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uMinDamage = 1152 / 2;
            this.uMaxDamage = 1250 / 2;
            this.tDamageType = ItemDamageType.Frost;
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 0;
            this.bTriggersGCD = true;
            this.uRange = 20;
            this.uArea = 10;
            this.bAOE = true;
            
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
                return (int)(this.CState.m_Stats.AttackPower * .4 );
            }
            set
            {
                _DamageAdditiveModifer = value;
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
                return CState.m_NumberOfTargets;
            }
            set
            {
                _DamageMultiplierModifer = value;
            }
        }
    }
}
