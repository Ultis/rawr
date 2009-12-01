using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    /// <summary>
    /// This class is the implmentation of the Blood Plague Ability based on the AbilityDK_Base class.
    /// </summary>
    class AbilityDK_BloodPlague : AbilityDK_Base
    {
        /// <summary>
        /// A disease dealing [0 + AP * 0.055 * 1.15] Shadow damage every 3 sec for 15 sec.  Caused by Plague Strike and other abilities.
        /// Base damage 0
        /// Bonus from attack power [AP * 0.055 * 1.15]
        /// </summary>
        public AbilityDK_BloodPlague()
        {
            this.szName = "Blood Plague";
        }
    }
}
