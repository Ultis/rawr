using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {

    /// <summary>
    /// Data container class for the results of calculations about a Character
    /// </summary>
    public class CharacterCalculationsWarlock : CharacterCalculationsBase {

        #region Get/Set Scores

        private float _overallPoints = 0f;
        public override float OverallPoints {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DpsPoints {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float PetDPSPoints {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        #endregion

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
            GetCharacterDisplayCalculationValues() {
            
            Dictionary<string, string> dictValues
                = new Dictionary<string, string>();
            dictValues.Add("Bonus Damage", "Not implemented, so sad.");
            dictValues.Add("Hit Rating", "Also not implemented :(");
            return dictValues;
        }
    }
}
//3456789 223456789 323456789 423456789 523456789 623456789 723456789 8234567890
