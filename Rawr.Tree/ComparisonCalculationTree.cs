using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class ComparisonCalculationTree : ComparisonCalculationBase
    {
        public override string Name { get; set;}
        
        public override float OverallPoints { get; set;}

        private float[] subPoints = new float[] { 0f, 0f, 0f, 0f };
        public override float[] SubPoints { get; set;}

        public float HpSPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float Mp5Points
        {
            get { return subPoints[1]; }
            set { subPoints[1] = value; }
        }

        public float SurvivalPoints
        {
            get { return subPoints[2]; }
            set { subPoints[2] = value; }
        }

        private Item _item = null;
        public override Item Item 
        {
            get { return _item; }
            set { _item = value; }
        }
        
        public override bool Equipped { get; set;}

        public override string ToString()
        {
            return string.Format("{0}: ({1:0.0}O {2:0.0}H {3:0.0}M {4:0.0}S)", Name, OverallPoints, HpSPoints, Mp5Points, SurvivalPoints);
        }
    }
}
