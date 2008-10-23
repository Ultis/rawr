using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ShadowPriest
{
    public class ComparisonCalculationShadowPriest : ComparisonCalculationBase
    {

        public ComparisonCalculationShadowPriest()
            : base()
        { }

        public ComparisonCalculationShadowPriest(string name)
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

        public float DpsPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SustainPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[1] = value; }
        }

        public float SurvivalPoints
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
