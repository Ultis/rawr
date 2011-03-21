using System;
using System.Collections.Generic;
using System.Text;


namespace Rawr.DK
{
    public class StatsDK : Stats
    {
        public float Mastery { get; set; }
        /// <summary>
        /// Increased RP gained from ability usage
        /// Percentage
        /// </summary>
        [Percentage]
        public float BonusRPMultiplier { get; set; }
        /// <summary>
        /// Increased Rune regen rate.  
        /// Percentage - use like haste.
        /// </summary>
        [Percentage]
        public float BonusRuneRegeneration { get; set; }
        /// <summary>
        /// Increased raw max RP.
        /// </summary>
        public float BonusMaxRunicPower { get; set; }
        
        public bool b2T11_Tank { get; set; }
        public bool b4T11_Tank { get; set ;}
        //public bool b2T11_DPS { get; set; }
        public bool b4T11_DPS { get; set; } // TODO: Still need to implement this.
    }
}
