using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Rogue {
    class ComparisonCalculationsRogue : ComparisonCalculationBase {
        private string _name = string.Empty;
        public override string Name {
            get { return _name; }
            set { _name = value; }
        }

        private float _overallPoints = 0f;
        public override float OverallPoints {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DPSPoints {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        private Item _item = null;
        public override Item Item {
            get { return _item; }
            set { _item = value; }
        }

        private bool _equipped = false;
        public override bool Equipped {
            get { return _equipped; }
            set { _equipped = value; }
        }

        public override string ToString() {
            return string.Format("{0}: ({1}O {2}D)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
        }
    }
}
