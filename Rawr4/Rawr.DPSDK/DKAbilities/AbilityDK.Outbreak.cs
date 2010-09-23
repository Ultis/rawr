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
        }

    }
}
