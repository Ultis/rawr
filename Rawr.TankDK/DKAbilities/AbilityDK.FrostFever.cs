using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Frost Fever Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_FrostFever : AbilityDK_Base
    {
        /// <summary>
        /// A disease dealing [0 + AP * 0.055 * 1.15] Frost damage every 3 sec and reducing the target's melee and ranged attack speed by 14% for 15 sec.  Caused by Icy Touch and other spells.
        /// Base damage 0
        /// Bonus from attack power [AP * 0.055 * 1.15]
        /// </summary>
        public AbilityDK_FrostFever()
        {
            this.szName = "Frost Fever";
            
        }
    }
}
