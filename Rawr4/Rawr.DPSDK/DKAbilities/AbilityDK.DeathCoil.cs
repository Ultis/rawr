using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Death Coil Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_DeathCoil : AbilityDK_Base
    {
        public AbilityDK_DeathCoil(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Death Coil";
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 40;
            this.uBaseDamage = 985;
            this.bWeaponRequired = false;
            this.bTriggersGCD = true;
            this.uRange = 30;
            this.tDamageType = ItemDamageType.Shadow;
            this.AbilityIndex = (int)DKability.DeathCoil;
        }

        private int _DamageAdditiveModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public int DamageAdditiveModifer
        {
            get
            {
                //this.DamageAdditiveModifer = [AP * 0.3]
                return (int)(this.CState.m_Stats.AttackPower * .3) + this._DamageAdditiveModifer;
            }
            set
            {
                _DamageAdditiveModifer = value;
            }
        }

        private float _DamageMultiplierModifier = 0;
        override public float DamageMultiplierModifer
        {
            get
            {
                //this.DamageAdditiveModifer = [AP * 0.3]
                if (CState.m_Talents.GlyphofDeathCoil)
                    _DamageMultiplierModifier += .15f;
                return this._DamageMultiplierModifier + base.DamageMultiplierModifer;
            }
            set
            {
                _DamageMultiplierModifier = value;
            }
        }
    }
}
