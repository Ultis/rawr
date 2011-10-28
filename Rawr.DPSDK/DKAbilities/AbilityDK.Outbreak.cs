using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Outbreak Ability based on the AbilityDK_Base class.
    /// Instantly applies Blood Plague and Frost Fever to the target enemy.
    /// </summary>
    class AbilityDK_Outbreak : AbilityDK_Base
    {
        public AbilityDK_Outbreak(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Outbreak";
            this.bWeaponRequired = false;
            this.uRange = 30;
            this.bTriggersGCD = true;
            this.Cooldown = 60 * 1000; // 1 min CD.
            if (Rawr.Properties.GeneralSettings.Default.PTRMode && CS.m_Talents.HighestTree == 0)
                this.Cooldown = 30 * 1000;
            this.ml_TriggeredAbility = new AbilityDK_Base[2];
            UpdateCombatState(CS);
            AbilityIndex = (int)DKability.Outbreak;
        }

        public override void UpdateCombatState(CombatState CS)
        {
            base.UpdateCombatState(CS);
            this.ml_TriggeredAbility[0] = new AbilityDK_BloodPlague(CS);
            this.ml_TriggeredAbility[1] = new AbilityDK_FrostFever(CS);
        }
    }
}
