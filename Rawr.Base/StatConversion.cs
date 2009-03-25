using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    // To be done:
    // Physical Combat:
    // float GetPhysicalMiss(int LevelDelta, bool DualWield)
    // float GetPhysicalDodge(int LevelDelta)
    // float GetPhysicalParry(int LevelDelta)
    // float GetPhysicalGlancing(int LevelDelta)
    // float[??] GetPhysicalCombatTable(int LevelDelta, float Hit, float Expertise, float Block)
    // float GetPhysicalMitigation(int TargetLevel)
    // Magical Combat:
    // float GetSpellMiss(int LevelDelta, bool PvP)
    // float GetAverageSpellResist(int LevelDelta, float Resistance)
    // float[11] GetSpellResistTable(int LevelDelta, float Resistance)
    // float GetMinimumSpellResist(int LevelDelta, float Resistance)


	public static class ArmorCalculations
	{
		//Added as part of the change to ArPen functionality; should integrate with StatConversion or something,
		//don't know exactly how, quite yet.
		public static float GetDamageReduction(int attackerLevel, float targetArmor, 
			float attackerArmorPenetration, float attackerArmorPenetrationRating)
		{
			float armorReductionPercent = (1f - attackerArmorPenetration) * (1f - attackerArmorPenetrationRating * 1.25f / 1539.529991f);
			float reducedArmor = (float)targetArmor * (armorReductionPercent);
			float damageReduction = (reducedArmor / ((467.5f * attackerLevel) + reducedArmor - 22167.5f));
			return damageReduction;
		}
	}

    public static class StatConversion
    {   // Class only works for Level 80 Characters
        // Numbers reverse engineered by Whitetooth (hotdogee [at] gmail [dot] com)

        #region Constants

        public const float LEVEL_80_COMBATRATING_MODIFIER = 3.2789987789987789987789987789988f; // 82/52 * Math.Pow(131/63, ((80 - 70) / 10));
        public const float RATING_PER_ARMORPENETRATION = 1231.6239f; // 4.69512177f / 1.25f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_BLOCK = 1639.4994f; // 5f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_DEFENSE = 4.9184987f; // 1.5f * LEVEL_80_COMBATRATING_MODIFIER;
        public const float RATING_PER_DODGE = 3934.7985f; // 12f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_EXPERTISE = 8.1974969f; // 2.5f * LEVEL_80_COMBATRATING_MODIFIER;
        public const float RATING_PER_PARRY = 4918.4987f; // 15f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_PHYSICALCRIT = 4590.5983f; // 14f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_PHYSICALHASTE = 3278.9988f; // 10f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_PHYSICALHIT = 3278.9988f; // 10f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_RESILIENCE = 8197.4969f; // 25f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_SPELLCRIT = 4590.5983f; // 14f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_SPELLHASTE = 3278.9988f; // 10f * LEVEL_80_COMBATRATING_MODIFIER * 100f;
        public const float RATING_PER_SPELLHIT = 2623.1990f; //8f * LEVEL_80_COMBATRATING_MODIFIER * 100f;

        // Same for all classes
        public const float INT_PER_SPELLCRIT = 166.66667f;
        public const float REGEN_CONSTANT = 0.005575f;

        // Sigh, depends on class.
        public static float[] AGI_PER_PHYSICALCRIT = { 0.0f, // CharacterClass starts at 1
            62.50f, // Warrior 1
            52.08f, // Paladin 2
            83.33f, // Hunter 3
            83.33f, // Rogue 4
            52.08f, // Priest 5
            62.50f, // Death Knight 6
            83.33f, // Shaman 7
            51.02f, // Mage 8
            50.51f, // Warlock 9
            0.0f,   // Empty 10
            83.33f, // Druid 11
        };

        public static float[] AGI_PER_DODGE = { 0.0f, // Starts at 0
            73.52941176f, // Warrior 1
            52.08333333f, // Paladin 2
            75.18796992f, // Hunter 3
            41.49377593f, // Rogue 4
            52.08333333f, // Priest 5
            73.52941176f, // Death Knight 6
            52.08333333f, // Shaman 7
            51.28205128f, // Mage 8
            52.08333333f, // Warlock 9
            0.0f,         // Empty 10
            41.66666667f, // Druid 11
        };

        #endregion


        #region Functions that return something.

        public static float GetArmorPenetrationFromRating(float Rating, Character.CharacterClass Class) { return GetArmorPenetrationFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% Armor Ignored)
        /// </summary>
        /// <param name="Rating">Armor Penetration Rating</param>
        /// <returns>A Percentage (0.05 = 5% Armor Ignored)</returns>
        public static float GetArmorPenetrationFromRating(float Rating)
        {
            return Rating / RATING_PER_ARMORPENETRATION;
        }

        public static float GetBlockFromRating(float Rating, Character.CharacterClass Class) { return GetBlockFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% added chance to Block)
        /// </summary>
        /// <param name="Rating">Block Rating</param>
        /// <returns>A Percentage (0.05 = 5% added chance to Block)</returns>
        public static float GetBlockFromRating(float Rating)
        {
            return Rating / RATING_PER_BLOCK;
        }

        public static float GetDefenseFromRating(float Rating, Character.CharacterClass Class) { return GetDefenseFromRating(Rating); }
        /// <summary>
        /// Returns a Value (5.4 = 5 extra Defense)
        /// </summary>
        /// <param name="Rating">Defense Rating</param>
        /// <returns>A Value (5.4 = 5 extra Defense)</returns>
        public static float GetDefenseFromRating(float Rating)
        {
            return Rating / RATING_PER_DEFENSE;
        }

        public static float GetDodgeFromRating(float Rating, Character.CharacterClass Class) { return GetDodgeFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Dodge)
        /// </summary>
        /// <param name="Rating">Dodge Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Dodge)</returns>
        public static float GetDodgeFromRating(float Rating)
        {
            return Rating / RATING_PER_DODGE;
        }

        public static float GetExpertiseFromRating(float Rating, Character.CharacterClass Class) { return GetExpertiseFromRating(Rating); }
        /// <summary>
        /// Returns a Value (6.34 = 6.34 extra Expertise)
        /// </summary>
        /// <param name="Rating">Expertise Rating</param>
        /// <returns>A Value (6.34 = 6.34 extra Expertise)</returns>
        public static float GetExpertiseFromRating(float Rating)
        {
            return Rating / RATING_PER_EXPERTISE;
        }

        public static float GetParryFromRating(float Rating, Character.CharacterClass Class) { return GetParryFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Parry)
        /// </summary>
        /// <param name="Rating">Parry Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Parry)</returns>
        public static float GetParryFromRating(float Rating)
        {
            return Rating / RATING_PER_PARRY;
        }

        public static float GetCritFromRating(float Rating, Character.CharacterClass Class) { return GetPhysicalCritFromRating(Rating); }
        public static float GetCritFromRating(float Rating) { return GetPhysicalCritFromRating(Rating); }
        public static float GetPhysicalCritFromRating(float Rating, Character.CharacterClass Class) { return GetPhysicalCritFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Rating">Crit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetPhysicalCritFromRating(float Rating)
        {
            return Rating / RATING_PER_PHYSICALCRIT;
        }

        // Returns a Percentage
        public static float GetHasteFromRating(float Rating, Character.CharacterClass Class) { return GetPhysicalHasteFromRating(Rating, Class); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Haste)
        /// </summary>
        /// <param name="Rating">Haste Rating</param>
        /// <param name="Class">Character.CharacterClass</param>
        /// <returns>A Percentage (0.05 = 5% extra Haste)</returns>
        public static float GetPhysicalHasteFromRating(float Rating, Character.CharacterClass Class)
        {
            if (Class == Character.CharacterClass.DeathKnight
                || Class == Character.CharacterClass.Druid
                || Class == Character.CharacterClass.Paladin
                || Class == Character.CharacterClass.Shaman)
                return Rating / RATING_PER_PHYSICALHASTE * 1.3f;    // Patch 3.1: Hybrids gain 30% more Phyiscal Haste from Haste Rating.
            return Rating / RATING_PER_PHYSICALHASTE;
        }

        public static float GetHitFromRating(float Rating, Character.CharacterClass Class) { return GetPhysicalHitFromRating(Rating); }
        public static float GetHitFromRating(float Rating) { return GetPhysicalHitFromRating(Rating); }
        public static float GetPhysicalHitFromRating(float Rating, Character.CharacterClass Class) { return GetPhysicalHitFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Hit)</returns>
        public static float GetPhysicalHitFromRating(float Rating)
        {
            return Rating / RATING_PER_PHYSICALHIT;
        }

        // Returns a Percentage
        public static float GetResilienceFromRating(float Rating, Character.CharacterClass Class) { return GetResilienceFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Resilience)
        /// </summary>
        /// <param name="Rating">Resilience Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Resilience)</returns>
        public static float GetResilienceFromRating(float Rating)
        {
            return Rating / RATING_PER_RESILIENCE;
        }

        public static float GetSpellCritFromRating(float Rating, Character.CharacterClass Class) { return GetSpellCritFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Rating">Crit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetSpellCritFromRating(float Rating)
        {
            return Rating / RATING_PER_SPELLCRIT;
        }

        public static float GetSpellHasteFromRating(float Rating, Character.CharacterClass Class) { return GetSpellHasteFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Haste)
        /// </summary>
        /// <param name="Rating">Haste Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra Haste)</returns>
        public static float GetSpellHasteFromRating(float Rating)
        {
            return Rating / RATING_PER_SPELLHASTE;
        }

        public static float GetSpellHitFromRating(float Rating, Character.CharacterClass Class) { return GetSpellHitFromRating(Rating); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Hit)
        /// </summary>
        /// <param name="Rating">Hit Rating</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Hit)</returns>
        public static float GetSpellHitFromRating(float Rating)
        {
            return Rating / RATING_PER_SPELLHIT;
        }

        public static float GetSpellCritFromIntellect(float Intellect, Character.CharacterClass Class) { return GetSpellCritFromIntellect(Intellect); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Intellect">Intellect</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetSpellCritFromIntellect(float Intellect)
        {
            return Intellect / INT_PER_SPELLCRIT * 0.01f;
        }

        public static float GetCritFromAgility(float Agility, Character.CharacterClass Class) { return GetPhysicalCritFromAgility(Agility, Class); }
        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra chance to Crit)
        /// </summary>
        /// <param name="Agility">Agility</param>
        /// <param name="Class">Character.CharacterClass</param>
        /// <returns>A Percentage (0.05 = 5% extra chance to Crit)</returns>
        public static float GetPhysicalCritFromAgility(float Agility, Character.CharacterClass Class)
        {
            return Agility / AGI_PER_PHYSICALCRIT[(int)Class] * 0.01f;
        }

        /// <summary>
        /// Returns a Percentage (0.05 = 5% extra Dodge)
        /// </summary>
        /// <param name="Agility">Agility</param>
        /// <param name="Class">Character.CharacterClass</param>
        /// <returns>A Percentage (0.05 = 5% extra Dodge)</returns>
        public static float GetDodgeFromAgility(float Agility, Character.CharacterClass Class)
        {
            return Agility / AGI_PER_DODGE[(int)Class] * 0.01f;
        }

        public static float GetSpiritRegenSec(float Spirit, float Intellect, Character.CharacterClass Class) { return GetSpiritRegenSec(Spirit, Intellect); }
        /// <summary>
        /// Returns a Number, How much mana is gained each Second. (Multiply by 5 to get MP5)
        /// </summary>
        /// <param name="Spirit">Spirit</param>
        /// <param name="Intellect">Intellect</param>
        /// <returns>A Number, How much mana is gained each Second. (Multiply by 5 to get MP5)</returns>
        public static float GetSpiritRegenSec(float Spirit, float Intellect)
        {
            return 0.001f + Spirit * REGEN_CONSTANT * (float)Math.Sqrt(Intellect);
        }
        #endregion
    }

/* Old:
    // This class converts combat ratings from Rating to % stat.
    // It convertrs Intellect to Spell Crit (Reliable for lvl 70-80, below 70 only reliable for Mage)
    // It converts Agility to Dodge and Melee Crit as well (Not all classes implemented yet.)
    // It contains a conversion for Intellect & Spirit for Spirit Based mana regen for 1-80 (as MP1)
    public class StatConversion
    {
        // Numbers reverse engineered by Whitetooth (hotdogee [at] gmail [dot] com)
        static int MaxLevel = 80;
        private int _Level = MaxLevel;

        #region Combat Rating
        public enum RatingType
        {
            ArmorPenetration = 0,
            Block,
            Crit,
            Defense,
            Dodge,
            Expertise,
            Haste,
            Hit,
            Parry,
            Resilience,
            SpellCrit,
            SpellHaste,
            SpellHit
        }

        private float[] RatingConversionBase60 = new float[Enum.GetValues(typeof(RatingType)).Length];
        private float[,] RatingConversionTable = new float[Enum.GetValues(typeof(RatingType)).Length, MaxLevel + 1];

        public float GetArmorPenetrationFromRating(float ARRating, int Level) { return ARRating * RatingConversionTable[(int)RatingType.ArmorPenetration, Level]; }
        public float GetArmorPenetrationFromRating(float ARRating) { return GetArmorPenetrationFromRating(ARRating, _Level); }

        public float GetBlockFromRating(float BlockRating, int Level) { return BlockRating * RatingConversionTable[(int)RatingType.Block, Level]; }
        public float GetBlockFromRating(float BlockRating) { return GetBlockFromRating(BlockRating, _Level); }

        public float GetCritFromRating(float CritRating, int Level) { return CritRating * RatingConversionTable[(int)RatingType.Crit, Level]; }
        public float GetCritFromRating(float CritRating) { return GetCritFromRating(CritRating, _Level); }

        public float GetDefenseFromRating(float DefenseRating, int Level) { return DefenseRating * RatingConversionTable[(int)RatingType.Defense, Level]; }
        public float GetDefenseFromRating(float DefenseRating) { return GetDefenseFromRating(DefenseRating, _Level); }

        public float GetDodgeFromRating(float DodgeRating, int Level) { return DodgeRating * RatingConversionTable[(int)RatingType.Dodge, Level]; }
        public float GetDodgeFromRating(float DodgeRating) { return GetDodgeFromRating(DodgeRating, _Level); }

        public float GetExpertiseFromRating(float ExpertiseRating, int Level) { return ExpertiseRating * RatingConversionTable[(int)RatingType.Expertise, Level]; }
        public float GetExpertiseFromRating(float ExpertiseRating) { return GetExpertiseFromRating(ExpertiseRating, _Level); }

        public float GetHasteFromRating(float HasteRating, int Level) { return HasteRating * RatingConversionTable[(int)RatingType.Haste, Level]; }
        public float GetHasteFromRating(float HasteRating) { return GetHasteFromRating(HasteRating, _Level); }

        public float GetHitFromRating(float HitRating, int Level) { return HitRating * RatingConversionTable[(int)RatingType.Hit, Level]; }
        public float GetHitFromRating(float HitRating) { return GetHitFromRating(HitRating, _Level); }

        public float GetParryFromRating(float ParryRating, int Level) { return ParryRating * RatingConversionTable[(int)RatingType.Parry, Level]; }
        public float GetParryFromRating(float ParryRating) { return GetParryFromRating(ParryRating, _Level); }

        public float GetResilienceFromRating(float ResilienceRating, int Level) { return ResilienceRating * RatingConversionTable[(int)RatingType.Resilience, Level]; }
        public float GetResilienceFromRating(float ResilienceRating) { return GetResilienceFromRating(ResilienceRating, _Level); }

        public float GetSpellCritFromRating(float SpellCritRating, int Level) { return SpellCritRating * RatingConversionTable[(int)RatingType.SpellCrit, Level]; }
        public float GetSpellCritFromRating(float SpellCritRating) { return GetSpellCritFromRating(SpellCritRating, _Level); }

        public float GetSpellHasteFromRating(float SpellHasteRating, int Level) { return SpellHasteRating * RatingConversionTable[(int)RatingType.SpellHaste, Level]; }
        public float GetSpellHasteFromRating(float SpellHasteRating) { return GetSpellHasteFromRating(SpellHasteRating, _Level); }

        public float GetSpellHitFromRating(float SpellHitRating, int Level) { return SpellHitRating * RatingConversionTable[(int)RatingType.SpellHit, Level]; }
        public float GetSpellHitFromRating(float SpellHitRating) { return GetSpellHitFromRating(SpellHitRating, _Level); }
        #endregion

        #region Stat Rating
        public enum StatType
        {
            AgilityToCrit = 0,
            AgilityToDodge,
            IntellectToCrit,
        }
        private float[][] StatConversionTable = new float[Enum.GetValues(typeof(RatingType)).Length][];
//        private Dictionary<StatType, List<float>> StatConversionTable = new Dictionary<StatType, List<float>>();

        public float GetSpellCritFromIntellect(float Intellect, int Level) { return Intellect * StatConversionTable[(int)StatType.IntellectToCrit][Level]; }
        public float GetSpellCritFromIntellect(float Intellect) { return GetSpellCritFromIntellect(Intellect, _Level); }

        public float GetCritFromAgility(float Agility, int Level) { return Agility * StatConversionTable[(int)StatType.AgilityToCrit][Level]; }
        public float GetCritFromAgility(float Agility) { return GetCritFromAgility(Agility, _Level); }

        public float GetDodgeFromAgility(float Agility, int Level) { return Agility * StatConversionTable[(int)StatType.AgilityToDodge][Level]; }
        public float GetDodgeFromAgility(float Agility) { return GetDodgeFromAgility(Agility, _Level); }

        #endregion

        #region Spirit Based Mana Regen
        private float[] SpiritRegenConstant = { 0.000000000f,    // 00
                0.034965001f, 0.034191001f, 0.033465002f, 0.032527f,    0.031661f,    0.031076999f, 0.030523f,    0.029995f, 0.029307f,    0.028662f,    // 01-10
                0.027585f,    0.026215f,    0.025381001f, 0.024301f,    0.023345999f, 0.022748999f, 0.021958999f, 0.021387f, 0.020791f,    0.020121001f, // 11-20
                0.019733001f, 0.019156f,    0.018819001f, 0.018316999f, 0.017936001f, 0.017577f,    0.017201001f, 0.016919f, 0.016581999f, 0.016233999f, // 21-30
                0.015995f,    0.015707999f, 0.015464f,    0.015204f,    0.014957f,    0.014745f,    0.014496f,    0.014302f, 0.014095f,    0.013896f,    // 31-40
                0.013724f,    0.013522f,    0.013363f,    0.013176f,    0.012996f,    0.012854f,    0.012687f,    0.01254f,  0.012384f,    0.012233f,    // 41-50
                0.012114f,    0.011973f,    0.01186f,     0.011715f,    0.011576f,    0.011473f,    0.011342f,    0.011245f, 0.011111f,    0.011f,       // 51-60
                0.010701f,    0.010523f,    0.010291f,    0.01012f,     0.009969f,    0.009808f,    0.009652f,    0.009553f, 0.009446f,    0.009327f,    // 61-70
                0.008859f,    0.008415f,    0.007993f,    0.007592f,    0.007211f,    0.006849f,    0.006506f,    0.006179f, 0.005869f,    0.005575f     // 71-80
            };
        public float GetSpiritRegenSec(float Spirit, float Intellect, int Level) { return 0.001f + Spirit * SpiritRegenConstant[Level] * (float)Math.Sqrt(Intellect); }
        public float GetSpiritRegenSec(float Spirit, float Intellect) { return GetSpiritRegenSec(Spirit, Intellect, _Level); }
        #endregion

        public void SetLevel(int Level) { _Level = Level; }

        public StatConversion(Character _character)
        {
            SetLevel(_character.Level);

            #region Combat Rating Calculations
            // Level 60 values, rest is extrapolated           
            RatingConversionBase60[(int)RatingType.ArmorPenetration] = 4.69512177f;
            RatingConversionBase60[(int)RatingType.Block] = 5f;
            RatingConversionBase60[(int)RatingType.Crit] = 14f;
            RatingConversionBase60[(int)RatingType.Defense] = 1.5f;
            RatingConversionBase60[(int)RatingType.Dodge] = 12f;
            RatingConversionBase60[(int)RatingType.Expertise] = 10f;
            RatingConversionBase60[(int)RatingType.Haste] = 10f;
            RatingConversionBase60[(int)RatingType.Hit] = 10f;
            RatingConversionBase60[(int)RatingType.Parry] = 15f;
            RatingConversionBase60[(int)RatingType.Resilience] = 25f;
            RatingConversionBase60[(int)RatingType.SpellCrit] = 14f;
            RatingConversionBase60[(int)RatingType.SpellHaste] = 10f;
            RatingConversionBase60[(int)RatingType.SpellHit] = 8f;

            for (int x = 0; x < 81; x++)
            {
                float RatingConversion = 0;
                if (x <= 10)
                    RatingConversion = 2f / 52f;
                else if (x <= 35)
                    RatingConversion = (35f - 8f) / 52f;
                else if (x <= 60)
                    RatingConversion = (x - 8f) / 52f;
                else if (x <= 70)
                    RatingConversion = 82f / (262f - 3f * x);
                else
                    RatingConversion = (float)Math.Pow((82f / 52f) * (131f / 63f), (x - 70f) / 10f);
                foreach (RatingType rt in Enum.GetValues(typeof(RatingType)))
                    RatingConversionTable[(int)rt, x] = 1f / (RatingConversionBase60[(int)rt] * RatingConversion);
            }
            #endregion

            #region Stat Calculations
            // This is the Mage Int->Crit table, but will do nicely for all other classes between 60-80
            StatConversionTable[(int)StatType.IntellectToCrit] = new float[] { 0.0f,   // 00
                     6.11f,  6.35f,  6.60f,  7.09f,  7.33f,  7.58f,  7.82f,  8.06f,  8.55f,  8.80f, // 01-10
                     9.53f, 10.75f, 11.48f, 13.68f, 14.90f, 15.65f, 16.61f, 17.61f, 18.59f, 19.80f, // 11-20
                    20.53f, 21.74f, 22.47f, 23.70f, 24.69f, 25.64f, 26.88f, 29.59f, 30.77f, 32.05f, // 21-30
                    32.79f, 34.01f, 34.97f, 35.97f, 37.17f, 38.17f, 39.37f, 40.32f, 41.49f, 42.55f, // 31-40
                    43.48f, 46.51f, 47.39f, 48.54f, 49.75f, 50.76f, 52.08f, 53.19f, 54.35f, 55.87f, // 41-50
                    56.82f, 57.80f, 58.82f, 60.24f, 61.73f, 64.94f, 66.23f, 67.11f, 68.49f, 69.93f, // 51-60
                    69.93f, 69.93f, 69.93f, 70.42f, 70.42f, 72.46f, 74.63f, 76.34f, 78.13f, 80.00f, // 61-70
                    86.2068944f, 92.59259244f, 99.00990313f, 107.526879f, 114.9425283f, 123.4567927f, 133.333327f, 142.857139f, 153.8461534f, 166.6666709f // 71-80
                };

            switch (_character.Class)
            {
                case Character.CharacterClass.DeathKnight:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.258700f, 0.226400f, 0.226400f, 0.226400f, 0.226400f, 0.201200f, 0.201200f, 0.201200f, 0.201200f, 0.201200f, // 01-10
                            0.181100f, 0.181100f, 0.164600f, 0.164600f, 0.150900f, 0.150900f, 0.150900f, 0.139300f, 0.139300f, 0.129300f, // 11-20
                            0.129300f, 0.129300f, 0.120700f, 0.113200f, 0.113200f, 0.106500f, 0.106500f, 0.100600f, 0.100600f, 0.095300f, // 21-30
                            0.095300f, 0.090500f, 0.090500f, 0.086200f, 0.086200f, 0.082300f, 0.082300f, 0.078700f, 0.078700f, 0.075500f, // 31-40
                            0.072400f, 0.072400f, 0.069600f, 0.069600f, 0.067100f, 0.067100f, 0.064700f, 0.062400f, 0.062400f, 0.060400f, // 41-50
                            0.060400f, 0.058400f, 0.056600f, 0.056600f, 0.054900f, 0.054900f, 0.053300f, 0.051700f, 0.051700f, 0.050300f, // 51-60
                            0.047700f, 0.045300f, 0.043100f, 0.042100f, 0.040200f, 0.038500f, 0.037000f, 0.035500f, 0.034200f, 0.033500f, // 61-70
                            0.031200f, 0.028700f, 0.026600f, 0.024800f, 0.023200f, 0.021600f, 0.019900f, 0.018500f, 0.017200f, 0.016000f // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                    break;
                case Character.CharacterClass.Druid:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.126200f, 0.126200f, 0.120200f, 0.120200f, 0.114800f, 0.114800f, 0.109800f, 0.109800f, 0.105200f, 0.097100f, // 01-10
                            0.093500f, 0.093500f, 0.090200f, 0.090200f, 0.084200f, 0.084200f, 0.081400f, 0.078900f, 0.078900f, 0.070100f, // 11-20
                            0.070100f, 0.068200f, 0.066400f, 0.066400f, 0.063100f, 0.063100f, 0.061600f, 0.060100f, 0.060100f, 0.054900f, // 21-30
                            0.053700f, 0.053700f, 0.052600f, 0.051500f, 0.050500f, 0.049500f, 0.048500f, 0.048500f, 0.047600f, 0.044300f, // 31-40
                            0.043500f, 0.043500f, 0.042800f, 0.042100f, 0.040700f, 0.040100f, 0.040100f, 0.039400f, 0.038800f, 0.036600f, // 41-50
                            0.036100f, 0.035600f, 0.035100f, 0.035100f, 0.034100f, 0.033700f, 0.033200f, 0.032800f, 0.032400f, 0.030800f, // 51-60
                            0.029900f, 0.029500f, 0.028500f, 0.027900f, 0.027400f, 0.026900f, 0.026500f, 0.025800f, 0.025400f, 0.025000f, // 61-70
                            0.023200f, 0.021600f, 0.020100f, 0.018700f, 0.017300f, 0.016100f, 0.015000f, 0.013900f, 0.012900f, 0.012000f  // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.05f, // 61-70
                            0.047222f, 0.044444f, 0.041667f, 0.038889f, 0.036111f, 0.033333f, 0.030556f, 0.027778f, 0.025000f, 0.022222f, // 71-80
                        };
                    break;
                case Character.CharacterClass.Hunter:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.284000f, 0.283400f, 0.271100f, 0.253000f, 0.243000f, 0.233700f, 0.225100f, 0.217100f, 0.205100f, 0.198400f, // 01-10
                            0.184800f, 0.167000f, 0.154700f, 0.144100f, 0.133000f, 0.126700f, 0.119400f, 0.111700f, 0.106000f, 0.099800f, // 11-20
                            0.096200f, 0.091000f, 0.087200f, 0.082900f, 0.079700f, 0.076700f, 0.073400f, 0.070900f, 0.068000f, 0.065400f, // 21-30
                            0.063700f, 0.061400f, 0.059200f, 0.057500f, 0.055600f, 0.054100f, 0.052400f, 0.050800f, 0.049300f, 0.048100f, // 31-40
                            0.047000f, 0.045700f, 0.044400f, 0.043300f, 0.042100f, 0.041300f, 0.040200f, 0.039100f, 0.038200f, 0.037300f, // 41-50
                            0.036600f, 0.035800f, 0.035000f, 0.034100f, 0.033400f, 0.032800f, 0.032100f, 0.031400f, 0.030700f, 0.030100f, // 51-60
                            0.029700f, 0.029000f, 0.028400f, 0.027900f, 0.027300f, 0.027000f, 0.026400f, 0.025900f, 0.025400f, 0.025000f, // 61-70
                            0.023200f, 0.021600f, 0.020100f, 0.018700f, 0.017300f, 0.016100f, 0.015000f, 0.013900f, 0.012900f, 0.012000f  // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                    break;
                case Character.CharacterClass.Mage:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.077300f, 0.077300f, 0.077300f, 0.073600f, 0.073600f, 0.073600f, 0.073600f, 0.073600f, 0.073600f, 0.070300f, // 01-10
                            0.070300f, 0.070300f, 0.070300f, 0.070300f, 0.067200f, 0.067200f, 0.067200f, 0.067200f, 0.067200f, 0.064400f, // 11-20
                            0.064400f, 0.064400f, 0.064400f, 0.061800f, 0.061800f, 0.061800f, 0.061800f, 0.061800f, 0.059500f, 0.059500f, // 21-30
                            0.059500f, 0.059500f, 0.057300f, 0.057300f, 0.057300f, 0.055200f, 0.055200f, 0.055200f, 0.055200f, 0.053300f, // 31-40
                            0.053300f, 0.053300f, 0.053300f, 0.051500f, 0.051500f, 0.051500f, 0.049900f, 0.049900f, 0.049900f, 0.048300f, // 41-50
                            0.048300f, 0.048300f, 0.046800f, 0.046800f, 0.046800f, 0.045500f, 0.045500f, 0.045500f, 0.044200f, 0.044200f, // 51-60
                            0.044200f, 0.044200f, 0.042900f, 0.042900f, 0.042900f, 0.041800f, 0.041800f, 0.041800f, 0.040700f, 0.040700f, // 61-70
                            0.037700f, 0.035100f, 0.032900f, 0.030300f, 0.028100f, 0.026200f, 0.024200f, 0.022700f, 0.020900f, 0.019600f  // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                    break;
                case Character.CharacterClass.Paladin:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.216400f, 0.216400f, 0.216400f, 0.192400f, 0.192400f, 0.192400f, 0.192400f, 0.173200f, 0.173200f, 0.173200f, // 01-10
                            0.173200f, 0.173200f, 0.157400f, 0.157400f, 0.144300f, 0.144300f, 0.144300f, 0.133200f, 0.133200f, 0.123700f, // 11-20
                            0.123700f, 0.123700f, 0.115400f, 0.108200f, 0.108200f, 0.108200f, 0.101900f, 0.101900f, 0.096200f, 0.096200f, // 21-30
                            0.091100f, 0.091100f, 0.086600f, 0.086600f, 0.082500f, 0.082500f, 0.082500f, 0.078700f, 0.078700f, 0.075300f, // 31-40
                            0.075300f, 0.075300f, 0.072100f, 0.069300f, 0.069300f, 0.066600f, 0.066600f, 0.064100f, 0.064100f, 0.061800f, // 41-50
                            0.059700f, 0.059700f, 0.057700f, 0.057700f, 0.055900f, 0.055900f, 0.054100f, 0.052500f, 0.052500f, 0.050900f, // 51-60
                            0.049500f, 0.048100f, 0.046800f, 0.045600f, 0.044400f, 0.044400f, 0.042200f, 0.042200f, 0.041200f, 0.040300f, // 61-70
                            0.036800f, 0.034600f, 0.032100f, 0.029900f, 0.027500f, 0.025800f, 0.024000f, 0.022200f, 0.020600f, 0.019200f  // 71-80
                        };
                    // Lifted from Tankadin
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.000403f, // 61-70
                            .000372f, .000345f, .000322f, .000298f, .000277f, .000257f, .000239f, .000223f, .0002207f, .000192f // 71-80
                        };
                    break;
                case Character.CharacterClass.Priest:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.091200f, 0.091200f, 0.091200f, 0.086800f, 0.086800f, 0.086800f, 0.086800f, 0.082900f, 0.082900f, 0.082900f, // 01-10
                            0.082900f, 0.079300f, 0.079300f, 0.079300f, 0.079300f, 0.076000f, 0.076000f, 0.076000f, 0.072900f, 0.072900f, // 11-20
                            0.072900f, 0.072900f, 0.070100f, 0.070100f, 0.070100f, 0.067500f, 0.067500f, 0.067500f, 0.065100f, 0.065100f, // 21-30
                            0.065100f, 0.062900f, 0.062900f, 0.062900f, 0.060800f, 0.060800f, 0.060800f, 0.058800f, 0.058800f, 0.058800f, // 31-40
                            0.057000f, 0.057000f, 0.055300f, 0.055300f, 0.055300f, 0.053600f, 0.053600f, 0.052100f, 0.052100f, 0.052100f, // 41-50
                            0.050700f, 0.050700f, 0.049300f, 0.049300f, 0.048000f, 0.048000f, 0.046800f, 0.046800f, 0.045600f, 0.045600f, // 51-60
                            0.044500f, 0.044600f, 0.044300f, 0.043400f, 0.042700f, 0.042100f, 0.041500f, 0.041300f, 0.041200f, 0.040100f, // 61-70
                            0.037200f, 0.034400f, 0.032000f, 0.029900f, 0.027600f, 0.025700f, 0.024000f, 0.022200f, 0.020700f, 0.019200f  // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                    break;
                case Character.CharacterClass.Rogue:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.447600f, 0.429000f, 0.411800f, 0.381300f, 0.367700f, 0.355000f, 0.332100f, 0.321700f, 0.312000f, 0.294100f, // 01-10
                            0.264000f, 0.239400f, 0.214500f, 0.198000f, 0.177500f, 0.166000f, 0.156000f, 0.145000f, 0.135500f, 0.127100f, // 11-20
                            0.119700f, 0.114400f, 0.108400f, 0.104000f, 0.098000f, 0.093600f, 0.090300f, 0.086500f, 0.083000f, 0.079200f, // 21-30
                            0.076800f, 0.074100f, 0.071500f, 0.069100f, 0.066400f, 0.064300f, 0.062800f, 0.060900f, 0.059200f, 0.057200f, // 31-40
                            0.055600f, 0.054200f, 0.052800f, 0.051200f, 0.049700f, 0.048600f, 0.047400f, 0.046400f, 0.045400f, 0.044000f, // 41-50
                            0.043100f, 0.042200f, 0.041200f, 0.040400f, 0.039400f, 0.038600f, 0.037800f, 0.037000f, 0.036400f, 0.035500f, // 51-60
                            0.033400f, 0.032200f, 0.030700f, 0.029600f, 0.028600f, 0.027600f, 0.026800f, 0.026200f, 0.025600f, 0.025000f, // 61-70 
                            0.023200f, 0.021600f, 0.020100f, 0.018700f, 0.017300f, 0.016100f, 0.015000f, 0.013900f, 0.012900f, 0.012000f, // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                    break;
                case Character.CharacterClass.Shaman:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.103900f, 0.103900f, 0.099000f, 0.099000f, 0.094500f, 0.094500f, 0.094500f, 0.090300f, 0.090300f, 0.086600f, // 01-10
                            0.086600f, 0.083100f, 0.083100f, 0.079900f, 0.077000f, 0.074200f, 0.074200f, 0.071700f, 0.071700f, 0.067000f, // 11-20
                            0.067000f, 0.064900f, 0.064900f, 0.063000f, 0.061100f, 0.059400f, 0.059400f, 0.057700f, 0.057700f, 0.054700f, // 21-30
                            0.054700f, 0.053300f, 0.052000f, 0.052000f, 0.049500f, 0.048300f, 0.048300f, 0.047200f, 0.047200f, 0.045200f, // 31-40
                            0.044200f, 0.044200f, 0.043300f, 0.042400f, 0.041600f, 0.040700f, 0.040000f, 0.039200f, 0.039200f, 0.037800f, // 41-50
                            0.037100f, 0.036500f, 0.036500f, 0.035800f, 0.034600f, 0.034100f, 0.033500f, 0.033500f, 0.033000f, 0.032000f, // 51-60
                            0.031000f, 0.030400f, 0.029400f, 0.028500f, 0.028100f, 0.027300f, 0.026700f, 0.026100f, 0.025500f, 0.025000f, // 61-70
                            0.023200f, 0.021600f, 0.020100f, 0.018700f, 0.017300f, 0.016100f, 0.015000f, 0.013900f, 0.012900f, 0.012000f  // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                    break;
                case Character.CharacterClass.Warlock:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.118900f, 0.118900f, 0.113200f, 0.113200f, 0.113200f, 0.108100f, 0.108100f, 0.108100f, 0.103400f, 0.103400f, // 01-10
                            0.099100f, 0.099100f, 0.099100f, 0.095900f, 0.094400f, 0.092800f, 0.091400f, 0.089900f, 0.088500f, 0.087100f, // 11-20
                            0.085700f, 0.084400f, 0.083100f, 0.081800f, 0.080500f, 0.079200f, 0.078000f, 0.076800f, 0.075600f, 0.074500f, // 21-30
                            0.073300f, 0.072200f, 0.071100f, 0.070000f, 0.069000f, 0.067900f, 0.066900f, 0.065900f, 0.064900f, 0.063900f, // 31-40
                            0.063000f, 0.062000f, 0.061100f, 0.060200f, 0.059300f, 0.058400f, 0.057600f, 0.056700f, 0.055900f, 0.055100f, // 41-50
                            0.054300f, 0.053500f, 0.052700f, 0.051900f, 0.051200f, 0.050400f, 0.049700f, 0.049000f, 0.048300f, 0.047600f, // 51-60
                            0.046900f, 0.046200f, 0.045500f, 0.044900f, 0.044200f, 0.043600f, 0.043000f, 0.042400f, 0.041800f, 0.041200f, // 61-70
                            0.038400f, 0.035500f, 0.033000f, 0.030900f, 0.028700f, 0.026400f, 0.024500f, 0.022900f, 0.021200f, 0.019800f  // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                    break;
                case Character.CharacterClass.Warrior:
                    StatConversionTable[(int)StatType.AgilityToCrit] = new float[] { 0.0f,   // 00
                            0.258700f, 0.226400f, 0.226400f, 0.226400f, 0.226400f, 0.201200f, 0.201200f, 0.201200f, 0.201200f, 0.201200f, // 01-10
                            0.181100f, 0.181100f, 0.164600f, 0.164600f, 0.150900f, 0.150900f, 0.150900f, 0.139300f, 0.139300f, 0.129300f, // 11-20
                            0.129300f, 0.129300f, 0.120700f, 0.113200f, 0.113200f, 0.106500f, 0.106500f, 0.100600f, 0.100600f, 0.095300f, // 21-30
                            0.095300f, 0.090500f, 0.090500f, 0.086200f, 0.086200f, 0.082300f, 0.082300f, 0.078700f, 0.078700f, 0.075500f, // 31-40
                            0.072400f, 0.072400f, 0.069600f, 0.069600f, 0.067100f, 0.067100f, 0.064700f, 0.062400f, 0.062400f, 0.060400f, // 41-50
                            0.060400f, 0.058400f, 0.056600f, 0.056600f, 0.054900f, 0.054900f, 0.053300f, 0.051700f, 0.051700f, 0.050300f, // 51-60
                            0.047700f, 0.045300f, 0.043100f, 0.042100f, 0.040200f, 0.038500f, 0.037000f, 0.035500f, 0.034200f, 0.033500f, // 61-70
                            0.031200f, 0.028700f, 0.026600f, 0.024800f, 0.023200f, 0.021600f, 0.019900f, 0.018500f, 0.017200f, 0.016000f  // 71-80
                        };
                    StatConversionTable[(int)StatType.AgilityToDodge] = new float[] { 0.0f,  // 00
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 01-10
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 11-20
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 21-30
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 31-40
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 41-50
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 51-60
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 61-70
                            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, // 71-80
                        };
                    break;
                default:
                    new Exception("Unknown class!");
                    break;

            }
            // Reverse Intellect table for faster math later.
            for (int x = 0; x < StatConversionTable[(int)StatType.IntellectToCrit].Length; x++)
                if (StatConversionTable[(int)StatType.IntellectToCrit][x] > 0)
                    StatConversionTable[(int)StatType.IntellectToCrit][x] = 1f / StatConversionTable[(int)StatType.IntellectToCrit][x];
            #endregion
        }
    }
 */
}