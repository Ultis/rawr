using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Corpse Explosion Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_CorpseExplosion : AbilityDK_Base
    {
        public AbilityDK_CorpseExplosion(Stats s)
        {
            this.sStats = s;
            this.szName = "Corpse Explosion";
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 40;
            this.uBaseDamage = 443;
            this.bWeaponRequired = false;
            this.bTriggersGCD = true;
            this.uRange = 30;
            this.Cooldown = 5000;
            // TODO: AOE - multiple Targets
        }
    }
}
