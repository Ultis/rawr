using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK 
{
    class AbilityDK_WhiteSiwng : AbilityDK_Base
    {
        public AbilityDK_WhiteSiwng(CombatState CS)
        {
            this.CState = CS;
            this.szName = "White Swing";
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1;
            this.bTriggersGCD = false;
            this.AbilityIndex = (int)DKability.White;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.RunicPower = (int)((10f * CS.m_Talents.MightOfTheFrozenWastes * .15f) + ((CS.m_Talents.ScentOfBlood * .15f) * (CS.m_Stats.Parry + CS.m_Stats.Dodge))); // Should be 1.5 per point.
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }

        public override float DamageMultiplierModifer
        {
            get
            {
                float BCBChance = (CState.m_Talents.BloodCakedBlade * .1f);
                float BCBDamMult = .25f + (.125f * CState.m_uDiseaseCount);
                float DMM = ((1 + BCBChance) * (1 + BCBDamMult) - 1);
                return base.DamageMultiplierModifer + DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }
    }
}
