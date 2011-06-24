using System;

namespace Rawr.Retribution
{
    class ComparisonCalculationRetribution : ComparisonCalculationBase
    {
        private string _name = String.Empty;
        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _desc = string.Empty;
        public override string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public override float OverallPoints { get; set; }

        public override float[] SubPoints { get; set; }

        public float DPSPoints
        {
            get { return SubPoints[0]; }
            set { SubPoints[0] = value; }
        }

        public override Item Item { get; set; }

        public override ItemInstance ItemInstance { get; set; }

        public ComparisonCalculationRetribution()
        {
            Equipped = false;
            ItemInstance = null;
            Item = null;
            SubPoints = new float[] { 0f };
            OverallPoints = 0f;
        }

        public override bool Equipped { get; set; }

        public override bool PartEquipped { get; set; }
    }
}
