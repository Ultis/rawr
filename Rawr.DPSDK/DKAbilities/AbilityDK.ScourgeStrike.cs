using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Scourge Strike Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_ScourgeStrike : AbilityDK_Base
    {
        public AbilityDK_ScourgeStrike(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Scourge Strike";
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uBaseDamage = 624;
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1f;
            this.bTriggersGCD = true;
            this.tDamageType = ItemDamageType.Physical;
            this.AbilityIndex = (int)DKability.ScourgeStrike;
            UpdateCombatState(CS);
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.wMH = CS.MH;
            this.wOH = CS.OH;
        }

        public override float DamageMultiplierModifer
        {
            get
            {
                float DMM = base.DamageMultiplierModifer;
                DMM += (.12f * CState.m_Talents.RageOfRivendare);
                DMM += CState.m_uDiseaseCount * .18f + (CState.m_Talents.GlyphofScourgeStrike ? .3f : 0);
                return DMM;
            }
            set
            {
                base.DamageMultiplierModifer = value;
            }
        }

        public override float GetTotalDamage()
        {
            if (CState.m_Spec == Rotation.Type.Unholy)
                return base.GetTotalDamage();
            else
                return 0;
        }
    }
}
