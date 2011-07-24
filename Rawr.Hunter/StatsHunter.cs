using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    /// <summary>
    /// Hunter custom implementation of the Stats object to expand it with Hunter Specific variables
    /// </summary>
    public class StatsHunter : Stats
    {
        #region ===== Additive Stats ==================
        /// <summary>
        /// % Extra Pet Crit Chance
        /// </summary>
        public float BonusPetCritChance { get; set; }
        /// <summary>
        /// Pet Stamina
        /// </summary>
        public float PetStamina { get; set; }
        /// <summary>
        /// Pet Strength
        /// </summary>
        public float PetStrength { get; set; }
        /// <summary>
        /// Pet Spirit
        /// </summary>
        public float PetSpirit { get; set; }
        /// <summary>
        /// Pet Attack Power
        /// </summary>
        public float PetAttackPower { get; set; }
        #endregion
        #region ===== Multiplicative Stats ============
        public float BonusRangedAttackPowerMultiplier { get; set; }
        public float BonusPetAttackPowerMultiplier { get; set; }
        #endregion

        #region Set Bonuses
        public bool Gladiator_2pc { get; set; }
        public bool Gladiator_4pc { get; set; }

        public bool Tier_11_2pc { get; set; }
        public bool Tier_11_4pc { get; set; }

        public bool Tier_12_2pc { get; set; }
        public bool Tier_12_4pc { get; set; }
        #endregion

        public void SetSets(Character character)
        {
            int TCount;
            // Gladiator
            character.SetBonusCount.TryGetValue("Gladiator's Pursuit", out TCount);
            if (TCount >= 2) { Gladiator_2pc = true; }
            if (TCount >= 4) { Gladiator_4pc = true; }

            //T11
            character.SetBonusCount.TryGetValue("Lightning-Charged Battlegear", out TCount);
            if (TCount >= 2) { Tier_11_2pc = true; }
            if (TCount >= 4) { Tier_11_4pc = true; }

            //T12
            character.SetBonusCount.TryGetValue("Flamewaker's Battlegear", out TCount);
            if (TCount >= 2) { Tier_12_2pc = true; }
            if (TCount >= 4) { Tier_12_4pc = true; }
        }

        internal float[] _rawAdditiveData = new float[AdditiveStatCount];
        internal float[] _rawMultiplicativeData = new float[MultiplicativeStatCount];
        internal float[] _rawInverseMultiplicativeData = new float[InverseMultiplicativeStatCount];
        internal float[] _rawNoStackData = new float[NonStackingStatCount];

        public static StatsHunter operator *(StatsHunter a, float b)
        {
            StatsHunter c = new StatsHunter();

            int i = c._rawAdditiveData.Length;
            while (--i >= 0)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] * b;
            }
            i = c._rawMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawMultiplicativeData[i] = a._rawMultiplicativeData[i] * b;
            }
            i = c._rawInverseMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawInverseMultiplicativeData[i] = a._rawInverseMultiplicativeData[i] * b;
            }

            i = c._rawNoStackData.Length;
            while (--i >= 0)
            {
                c._rawNoStackData[i] = a._rawNoStackData[i] * b;
            }
            // undefined for special effects
            return c;
        }


        public virtual Stats GetItemStats(Character character, Item additionalItem)
        {
            StatsHunter stats = new StatsHunter();
            //AccumulateItemStats(stats, character, additionalItem);
            return stats;
        }
    }
}