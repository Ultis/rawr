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
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10 + (CS.m_Talents.ChillOfTheGrave > 0 ? -5 : 0);
            this.uMinDamage = 1152 / 2;
            this.uMaxDamage = 1250 / 2;
            this.tDamageType = ItemDamageType.Frost;
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 0;
            this.bTriggersGCD = true;
            this.uRange = 20;
            this.uArea = 10;
            this.bAOE = true;
            if (CS.m_Talents.GlyphofHowlingBlast)
            {
                this.ml_TriggeredAbility = new AbilityDK_Base[1];
                this.ml_TriggeredAbility[0] = new AbilityDK_FrostFever(CS);
            }
            this.AbilityIndex = (int)DKability.HowlingBlast;

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
                return base.GetTotalDamage();
            else
                return 0;
        }
    }
}
