using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    enum DKCostTypes : int
    {
        Blood = 0,
        Frost = 1,
        UnHoly = 2,
        RunicPower = 3,

        NumCostTypes
    }

    /// <summary>
    /// This class will be the base class for abilities.
    /// Each ability will inherit from this class and have to implement their own functions
    /// for the various spells/abilities.
    /// Then we will want to be able to aggregate all the data to have a solid picture of what's
    /// going on when the abilities are used.
    /// EG. IT * 2 should give us the values of 2 ITs in cost, damage, time, etc.
    /// </summary>
    class AbilityDK_Base
    {
        /// <summary>
        ///  Name of the ability.
        /// </summary>
        public string szName { get; set; }
        
        /// <summary>
        /// What is the cost of the ability?
        /// 4 INTs representing the 3 Rune Types & Runic Power.
        /// Use enum (int)DKCostTypes for placement.
        /// Negative costs mean they grant that item.
        /// TODO: Determine death rune functionality.
        /// </summary>
        public int[] AbilityCost = new int[(int)DKCostTypes.NumCostTypes];

        /// <summary>
        /// What min damage does the ability cause?
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        public uint uMinDamage { get; set; }
        /// <summary>
        /// What max damage does the ability cause?
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        public uint uMaxDamage { get; set; }
        /// <summary>
        /// Get the average value between Max and Min damage
        /// For DOTs damage is on a per-tick basis.
        /// </summary>
        public uint uBaseDamage { 
            get
            {
                uint AvgDam = (this.uMinDamage + this.uMaxDamage) / 2;
                uint WDam = 0;
                // Handle non-weapon based effects:
                if (null != this.wWeapon)
                    WDam = (uint)(this.wWeapon.damage * this.fWeaponDamageModifier);
                // Average out the min & max damage, then add in baseDamage from the weapon.
                return (AvgDam + WDam);
            }
            set 
            { 
                uMaxDamage = uMinDamage = value / 2;
            }
        }
        /// <summary>
        /// What's the range of a given ability?
        /// The idea is that we want to quantify the range buffing talents.
        /// </summary>
        public uint uRange { get; set; }
        /// <summary>
        /// What type of damage does this ability do?
        /// </summary>
        public ItemDamageType tDamageType  { get; set; }

        ///////////////////////////////////////////////////////
        // Time based items.  
        // These will all be effected by haste.
        // Haste effects GCD to a max hasted of 1.5 sec to 1 sec.
        /// <summary>
        /// How long does it take to cast?
        /// </summary>
        public float fCastTime { get; set; }
        /// <summary>
        /// Cooldown in seconds
        /// Default = 1.5 secs == Global Cooldown
        /// GCD min == 1.0 secs.
        /// </summary>
        private float _fCooldown;
        public float fCooldown 
        { 
            get 
            {
                return Math.Max(1f, _fCooldown);
            } 
            set
            { _fCooldown = value; }
        }

        /// <summary>
        /// Does this ability trigger the GCD?
        /// </summary>
        public bool bTriggersGCD { get; set; }
        /// <summary>
        /// How long does the effect last?
        /// </summary>
        public float fDuration { get; set; }
        /// <summary>
        /// How often does the effect proc for?
        /// </summary>
        public float fTickRate { get; set; }

        /////////////////////////////////////////////////
        // Weapon related items.

        public bool bWeaponRequired { get; set; }
        public Weapon wWeapon;
        public float fWeaponDamageModifier { get; set; }

        /// <summary>
        /// Default constructor for the DK's abilities.
        /// </summary>
        public AbilityDK_Base()
        {
            this.szName = "";
            this.AbilityCost[(int)DKCostTypes.Blood] = 0;
            this.AbilityCost[(int)DKCostTypes.Frost] = 0;
            this.AbilityCost[(int)DKCostTypes.UnHoly] = 0;
            this.AbilityCost[(int)DKCostTypes.RunicPower] = 0;
            this.uBaseDamage = 0;
            this.uRange = 5;
            this.tDamageType = ItemDamageType.Physical;
            this.fCooldown = 1.5f; // GCD

        }

        /// <summary>
        /// Get the single instance damage of this ability.
        /// </summary>
        /// <returns>float that represents a fully buffed single instance of this ability.</returns>
        public float GetTickDamage()
        {
            // Start w/ getting the base damage values.
            float fDamage = this.uBaseDamage;
            // TODO: Apply modifiers.
            fDamage += this.DamageAdditiveModifer;
            return fDamage;
        }

        /// <summary>
        /// Get the full effect over the lifetime of the ability.
        /// </summary>
        /// <returns>float that is TickDamage * duration</returns>
        public float GetTotalDamage()
        {
            // Start w/ getting the base damage values.
            float fDamage = this.GetTickDamage();
            // Assuming full duration, or standard impact.
            // But I want this in whole numbers.
            float fDamageCount = (float)Math.Floor(this.fDuration / this.fTickRate);
            // To prevent divide by 0 errors.
            if (float.IsNaN(fDamageCount))
            {
#if DEBUG
//                throw new Exception("fDamageCount NaN");
#endif
                fDamageCount = 1;
            }
            fDamage *= fDamageCount;
            return fDamage;
        }

        /// <summary>
        /// Setup the modifier formula for a given ability.
        /// </summary>
        private float _DamageAdditiveModifer;
        public float DamageAdditiveModifer
        {
            get 
            {
                return _DamageAdditiveModifer;
            }
            set 
            {
                _DamageAdditiveModifer = value;
            }
        }

        /// <summary>
        /// Setup Threat modifiers since this is what we're really interested in.
        /// </summary>
        private float _ThreatMultiplier;
        public float ThreatMultiplier
        {
            get
            {
                return _ThreatMultiplier;
            }
            set
            {
                _ThreatMultiplier = value;
            }
        }

        /// <summary>
        /// Get total threat values.
        /// </summary>
        private float _ThreatAdditiveModifier;
        public float TotalThreat
        {
            get
            {
                return StatConversion.ApplyMultiplier(GetTotalDamage(), ThreatMultiplier) + _ThreatAdditiveModifier;
            }
            set
            {
                _ThreatAdditiveModifier = value;
            }
        }
    }
}
