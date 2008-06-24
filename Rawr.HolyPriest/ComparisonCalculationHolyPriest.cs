using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.HolyPriest
{
    public class ComparisonCalculationHolyPriest : ComparisonCalculationBase
    {

        public ComparisonCalculationHolyPriest()
            : base()
        { }

        public ComparisonCalculationHolyPriest(string name)
            : base()
        {
            _name = name;
        }
        private string _name = string.Empty;
        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float HealPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float RegenPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float HastePoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
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
    }
}
