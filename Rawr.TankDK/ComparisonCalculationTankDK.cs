using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK {
    // Reminder: this is for an individual item.  Do not apply weighting to this class.  
    class ComparisonCalculationTankDK : ComparisonCalculationBase {
        private string _name = string.Empty;
        public override string Name {
            get { return _name; }
            set { _name = value; }
        }

        private string _desc = string.Empty;
        public override string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public float Survival
        {
            get { return _subPoints[(int)SMTSubPoints.Survival]; }
            set { _subPoints[(int)SMTSubPoints.Survival] = value; _overallPoints = Survival + Burst + Mitigation + Threat; }
        }

        public float Mitigation {
            get { return _subPoints[(int)SMTSubPoints.Mitigation]; }
            set { _subPoints[(int)SMTSubPoints.Mitigation] = value; _overallPoints = Survival + Burst + Mitigation + Threat; }
        }
        
        public float Burst
        {
            get { return _subPoints[(int)SMTSubPoints.Burst]; }
            set { _subPoints[(int)SMTSubPoints.Burst] = value; _overallPoints = Survival + Burst + Mitigation + Threat; }
        }

        public float Threat {
            get { return _subPoints[(int)SMTSubPoints.Threat]; }
            set { _subPoints[(int)SMTSubPoints.Threat] = value; _overallPoints = Survival + Burst + Mitigation + Threat; }
        }

        public float BurstTime
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float ReactionTime
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        private float _overallPoints = 0f;
        public override float OverallPoints {
            // Reminder: this is for an individual item.  Do not apply weighting to this class.  
            get {
                if (_overallPoints == 0f) { _overallPoints = Survival + Burst + Mitigation + Threat; }
                return _overallPoints;
            }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] {0f, 0f, 0f, 0f};
        public override float[] SubPoints {
            get { return _subPoints; }
            set { _subPoints = value; _overallPoints = Survival + Burst + Mitigation + Threat; }
        }

        private Item _item = null;
        public override Item Item {
            get { return _item; }
            set { _item = value; }
        }

        private ItemInstance _itemInstance = null;
        public override ItemInstance ItemInstance {
            get { return _itemInstance; }
            set { _itemInstance = value; }
        }

        private bool _equipped = false;
        public override bool Equipped {
            get { return _equipped; }
            set { _equipped = value; }
        }
        public override bool PartEquipped { get; set; }

        /// <summary>
        /// Name of the Stat to set to 1.00 for relative stats calcs
        /// This basically handles the EH base-line
        /// </summary>
        public override String BaseStat { get { return " Stamina"; } }

        /// <summary>
        /// User Option whether to use the Base Stat feature for relative stats calcs
        /// </summary>
        public override bool getBaseStatOption(Character character) { return true; }

        public override string ToString() {
            // So that the ToString() function can be used as a base-line comparison if all else fails.
            return string.Format("{0}: ({1}O = {2}S {3}M {4}B {5}T)", Name, OverallPoints, Survival, Mitigation, Burst, Threat);
        }
    }
}
