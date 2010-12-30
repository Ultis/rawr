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
            this.uBaseDamage = 985;
            this.bWeaponRequired = false;
            this.bTriggersGCD = true;
            this.uRange = 30;
            this.tDamageType = ItemDamageType.Shadow;
            this.AbilityIndex = (int)DKability.DeathCoil;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 40 - (CS.m_Talents.RunicCorruption * 3);
            if (CS.m_Talents.UnholyBlight > 0)
            {
                ml_TriggeredAbility = new AbilityDK_Base[1];
                ml_TriggeredAbility[0] = new AbilityDK_UnholyBlight(CS);
            }

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

        override public float DamageMultiplierModifer
        {
            get
            {
                float DMM = base.DamageMultiplierModifer;
                DMM += CState.m_Talents.Morbidity * .05f;
                if (CState.m_Talents.GlyphofDeathCoil)
                    DMM += .15f;
                return DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }
    }

    class AbilityDK_UnholyBlight : AbilityDK_Base
    {
        public AbilityDK_UnholyBlight(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Unholy Blight";
            this.bWeaponRequired = false;
            this.tDamageType = ItemDamageType.Shadow;
            this.uDuration = 10 * 1000;
        }

        public override uint uBaseDamage
        {
            get
            {
                AbilityDK_DeathCoil DC = new AbilityDK_DeathCoil(CState);
                return (uint)((float)DC.TotalDamage * .01f);
            }
            set
            {
                base.uBaseDamage = value;
            }
        }
    }
    
}
