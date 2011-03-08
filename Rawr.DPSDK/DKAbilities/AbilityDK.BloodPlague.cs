using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Blood Plague Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_BloodPlague : AbilityDK_Base
    {
        /// <summary>
        /// A disease dealing [0 + AP * 0.055 * 1.15] Shadow damage every 3 sec for 21 sec.  Caused by Plague Strike and other abilities.
        /// Base damage 0
        /// Bonus from attack power [AP * 0.055 * 1.15]
        /// </summary>
        public AbilityDK_BloodPlague(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Blood Plague";
            this.tDamageType = ItemDamageType.Shadow;
            this.uTickRate = 3 * 1000;
            this.uBaseDamage = 0;
            this.bTriggersGCD = false;
            this.CastTime = 0;
            this.Cooldown = 0;
            this.AbilityIndex = (int)DKability.BloodPlague;
            uRange = 0;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            if (CState.m_Talents.Epidemic > 3)
                // error
                this.uDuration = 21000;
            else
                this.uDuration = (21 * 1000) + ((uint)CState.m_Talents.Epidemic * 4000);
            if (CState.m_uDiseaseCount < (2 + CS.m_Talents.EbonPlaguebringer))
                CState.m_uDiseaseCount++;

        }

        private int _DamageAdditiveModifer = 0;
        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public int DamageAdditiveModifer
        {
            get
            {
                if (CState.m_Stats != null)
                    return (int)(this.CState.m_Stats.AttackPower * .055 * 1.15) + this._DamageAdditiveModifer + base.DamageAdditiveModifer;
                return base.DamageAdditiveModifer;
            }
            set
            {
                _DamageAdditiveModifer = value;
            }
        }

        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        override public float DamageMultiplierModifer
        {
            get
            {
                if (CState.m_Stats != null)
                    return CState.m_Stats.BonusDiseaseDamageMultiplier + base.DamageMultiplierModifer;
                return base.DamageMultiplierModifer;
            }
        }
    }
}
