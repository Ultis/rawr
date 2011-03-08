using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Frost Fever Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_FrostFever : AbilityDK_Base
    {
        /// <summary>
        /// A disease dealing [0 + AP * 0.055 * 1.15] Frost damage every 3 sec and reducing the target's melee and ranged attack speed by 20% for 21 sec.  Caused by Icy Touch and other spells.
        /// Base damage 0
        /// Bonus from attack power [AP * 0.055 * 1.15]
        /// </summary>
        /// <param name="Epidemic">How many points into Epidemic?</param>
        public AbilityDK_FrostFever(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Frost Fever";
            this.tDamageType = ItemDamageType.Frost;
            this.uTickRate = 3 * 1000;
            this.uBaseDamage = 0;
            this.bTriggersGCD = false;
            this.Cooldown = 0;
            this.CastTime = 0;
            this.AbilityIndex = (int)DKability.FrostFever;
            uRange = 0;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            if (CState.m_uDiseaseCount < (2 + CS.m_Talents.EbonPlaguebringer))
                CState.m_uDiseaseCount++;
            if (CS.m_Talents.Epidemic > 3)
                // error
                this.uDuration = 21000;
            else
                this.uDuration = 21 * 1000 + ((uint)CS.m_Talents.Epidemic * 4000);
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
                if (CState.m_Stats != null)
                    return (int)(this.CState.m_Stats.AttackPower * .055 * 1.15) + this._DamageAdditiveModifer + base.DamageAdditiveModifer;
                return base.DamageAdditiveModifer;
            }
            set
            {
                _DamageAdditiveModifer = value;
            }
        }

        private float _DamageMultiplierModifier;
        public override float DamageMultiplierModifer
        {
            get
            {
                if (CState.m_Stats != null)
                    _DamageMultiplierModifier = CState.m_Stats.BonusDiseaseDamageMultiplier + base.DamageMultiplierModifer + (CState.m_Talents.GlyphofIcyTouch ? .3f : 0);
                return _DamageMultiplierModifier;
            }
        }
    }
}
