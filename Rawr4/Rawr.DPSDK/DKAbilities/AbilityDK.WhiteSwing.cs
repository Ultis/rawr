using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DK 
{
    class AbilityDK_WhiteSiwng : AbilityDK_Base
    {
        public AbilityDK_WhiteSiwng(Stats s, Weapon MH, Weapon OH)
        {
//            this.sStats = s;
            this.wMH = MH;
            this.wOH = OH;
            this.szName = "White Swing";
            this.bWeaponRequired = true;
            this.fWeaponDamageModifier = 1;
            this.bTriggersGCD = false;
            this.AbilityIndex = (int)DKability.White;

        }
    }
}
