using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK
{
    /// <summary>
    /// This class is the implmentation of the Howling Blast Ability based on the AbilityDK_Base class.
    /// Spreads existing Blood Plague and Frost Fever infections from your target to all other enemies within 10 yards.  
    /// Diseases spread this way deal 50% of normal damage.
    /// </summary>
    class AbilityDK_Pestilence : AbilityDK_Base
    {
        public AbilityDK_Pestilence(CombatState CS)
        {
            this.CState = CS;
            this.szName = "Pestilence";
            this.AbilityCost[(int)DKCostTypes.Blood] = 1;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = -10;
            this.uMinDamage = 0;
            this.uMaxDamage = 0;
            this.bWeaponRequired = false;
            this.fWeaponDamageModifier = 0;
            this.bTriggersGCD = true;
            this.uRange = 0;
            this.uArea = 10 + (CS.m_Talents.GlyphofPestilence ? 5u : 0u);
            this.bAOE = true;
            // TODO: Glyph - Refreshes disease

        }
    }
}
