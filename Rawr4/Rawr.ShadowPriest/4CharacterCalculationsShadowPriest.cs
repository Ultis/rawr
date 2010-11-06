using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
    public class CharacterCalculationsShadowPriest : CharacterCalculationsBase
    {
        #region Variable Declarations and Definitions

        public Stats BasicStats { get; set; }
        public Stats CombatStats { get; set; }
        public int TargetLevel { get; set; }

        public Spell LightningBolt;

        public float ManaRegen;
        public float ReplenishMP5;

        public float TimeToOOM;
        public float CastRegenFraction;
        public float RotationDPS;
        public float TotalDPS;
        public float RotationMPS;
        public float CastsPerSecond;
        public float CritsPerSecond;
        public float MissesPerSecond;

        public float LatencyPerSecond;

        public string Rotation;
        public string RotationDetails;

        public Character LocalCharacter { get; set; }

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        #endregion

        #region the overridden method (GetCharacterDisplayCalculationValues)
        /// <summary>
        /// Builds a dictionary containing the values to display for each of the
        /// calculations defined in CharacterDisplayCalculationLabels. The key
        /// should be the Label of each display calculation, and the value
        /// should be the value to display, optionally appended with '*'
        /// followed by any string you'd like displayed as a tooltip on the
        /// value.
        /// </summary>
        /// <returns>
        /// A Dictionary<string, string> containing the values to display for
        /// each of the calculations defined in
        /// CharacterDisplayCalculationLabels.
        /// </returns>
        public override Dictionary<string, string>
            GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues
                = new Dictionary<string, string>();
            dictValues.Add("Test Value", "0.01*A Value");
            return dictValues;
        }
        #endregion


    }

    

}
