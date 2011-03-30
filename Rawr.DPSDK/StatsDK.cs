using System;
using System.Collections.Generic;
using System.Text;


namespace Rawr.DK
{
    public enum StatType { Unbuffed, Buffed, Average, Maximum };

    public class StatsDK : Stats
    {
        private float _Mastery = 8;
        public float Mastery { 
            get{ return _Mastery;}
            set {_Mastery = value;} 
        }
        /// <summary>
        /// Effective value AP of just Vengence.
        /// TotalAP == AP + VengenceAttackPower
        /// </summary>
        public float VengenceAttackPower { get; set; }
        /// <summary>
        /// Effective parry - will be 0 if unarmed.
        /// </summary>
        public float EffectiveParry { get; set; }
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
