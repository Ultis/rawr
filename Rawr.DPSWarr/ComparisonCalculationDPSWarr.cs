using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    class ComparisonCalculationDPSWarr : ComparisonCalculationBase
    {
        private string name = String.Empty;
        public override string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        private float overallPoints = 0f;
        public override float OverallPoints
        {
            get
            {
                return overallPoints;
            }
            set
            {
                overallPoints = value;
            }
        }

        private float[] subPoints = new float[] { 0f, 0f };

        public override float[] SubPoints
        {
            get
            {
                return subPoints;
            }
            set
            {
                subPoints = value;
            }
        }

        public float DamagePoints
        {
            get
            {
                return subPoints[0];
            }
            set
            {
                subPoints[0] = value;
            }
        }

        public float EfficiencyPoints
        {
            get
            {
                return subPoints[1];
            }
            set
            {
                subPoints[1] = value;
            }
        }

        private Item item = null;
        public override Item Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }

        bool equipped = false;
        public override bool Equipped
        {
            get
            {
                return equipped;
            }
            set
            {
                equipped = value;
            }
        }
    }
}
