using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class ComparisonCalculationTree : ComparisonCalculationBase
    {

        public ComparisonCalculationTree()
            : base()
        { }

        public ComparisonCalculationTree(string name)
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

        private float[] _subPoints = new float[] { 0f, 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float HpSPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float Mp5Points
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float SurvivalPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }

        public float ToLPoints
        {
            get { return _subPoints[3]; }
            set { _subPoints[3] = value; }
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
            return string.Format("{0}: ({1:0.0}O {2:0.0}H {3:0.0}M {4:0.0}S {5:0.0}T)", Name,
                OverallPoints, HpSPoints, Mp5Points, SurvivalPoints, ToLPoints);
        }
    }
}
