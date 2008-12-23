using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    class ComparisonCalculationTankDK : ComparisonCalculationBase
    {
        private string _name = string.Empty;
        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public float Mitigation 
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float Avoidance
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float Threat
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }


        public override float OverallPoints
        {
            get { return Mitigation + Avoidance + Threat; }
            set { }
        }

        private float[] _subPoints = {0.0f, 0.0f, 0.0f};

        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        private Item _item = null;
        public override Item Item
        {
            get { return _item; }
            set { _item = value; }
        }

        private bool _equipped = false;
        public override bool Equipped
        {
            get { return _equipped; }
            set { _equipped = value; }
        }

        public override string ToString()
        {
            return "";
            //return string.Format("{0}: ({1}O {2}M {3}S)", Name, Math.Round(OverallPoints), Math.Round(MitigationPoints), Math.Round(SurvivalPoints));
        }
    }
}
