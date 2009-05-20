using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    public class ComparisonCalculationHunter : ComparisonCalculationBase
    {
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

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

		public float HunterDpsPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float PetDpsPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }

		}

        private Item _item = null;
        public override Item Item
        {
            get { return _item; }
            set { _item = value; }
		}

		private ItemInstance _itemInstance = null;
		public override ItemInstance ItemInstance
		{
			get { return _itemInstance; }
			set { _itemInstance = value; }
		}

        private bool _equipped = false;
        public override bool Equipped
        {
            get { return _equipped; }
            set { _equipped = value; }
        }

        /// <summary>
        /// Name of the Stat to set to 1.00 for relative stats calcs
        /// </summary>
        public override String BaseStat { get { return " Agility"; } }

        /// <summary>
        /// User Option whether to use the Base Stat feature for relative stats calcs
        /// </summary>
        public override bool getBaseStatOption(Character character) { return true; }
    }
}
