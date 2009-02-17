using System;

namespace Rawr.Rogue
{
    internal class ComparisonCalculationsRogue : ComparisonCalculationBase
    {
        private string _name = string.Empty;
        private float[] _subPoints = new[] {0f};

        public override string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override float OverallPoints { get; set; }

        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DPSPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

		public override Item Item { get; set; }

		private ItemInstance _itemInstance = null;
		public override ItemInstance ItemInstance
		{
			get { return _itemInstance; }
			set { _itemInstance = value; }
		}

        public override bool Equipped { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: ({1}O {2}D)", Name, Math.Round(OverallPoints), Math.Round(DPSPoints));
        }
    }
}