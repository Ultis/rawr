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
            this.uMinDamage = 1152 / 2;
            this.uMaxDamage = 1250 / 2;
            this.tDamageType = ItemDamageType.Frost;
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 0;
            this.bTriggersGCD = true;
            this.uRange = 20;
            this.uArea = 10;
            this.bAOE = true;
            this.AbilityIndex = (int)DKability.HowlingBlast;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            if (CS.m_Talents.GlyphofHowlingBlast && CS.m_uDiseaseCount < 2)
            {
                this.ml_TriggeredAbility = new AbilityDK_Base[1];
                this.ml_TriggeredAbility[0] = new AbilityDK_FrostFever(CS);
            }
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10 + (CS.m_Talents.ChillOfTheGrave > 0 ? -5 : 0);
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
            if (CState.m_Talents.HowlingBlast > 0)
            {
                // Start w/ getting the base damage values.
                int iDamage = GetTickDamage();
                // Assuming full duration, or standard impact.
                // But I want this in whole numbers.
                // Also need to decide if I want this to be whole ticks, or if partial ticks will be allowed.
                float fDamageCount = (float)(uDuration / Math.Max(1, uTickRate));

                iDamage = (int)((float)iDamage * fDamageCount * (1 + CritChance) * Math.Min(1, HitChance));
                if (bAOE == true)
                {
                    // Need to ensure this value is reasonable for all abilities.
                    iDamage = (int)(
                        (float)iDamage // Damage to main target
                        + (iDamage * .4f) * (Math.Max(1, this.CState.m_NumberOfTargets)-1 ) // damage to secondary targets @ 40%
                        );
                }
                return iDamage;
            }
            else
                return 0;
        }
    }
}
