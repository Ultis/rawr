using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    public class ComparisonCalculationWarlock : ComparisonCalculationBase
    {

        public ComparisonCalculationWarlock()
            : base()
        { }

        public ComparisonCalculationWarlock(string name)
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

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get
            {
                for (int x = 0; x < _subPoints.Length; x++)
                    if (_subPoints[x] < 0f)
                        _subPoints[x] = 0f;
                return _subPoints;
            }
            set
            {
                for (int x = 0; x < _subPoints.Length; x++)
                    if (value[x] < 0f)
                        value[x] = 0f;
                _subPoints = value;
            }
        }

        public float DpsPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = (value < 0f)?0f:value; }
        }

        public float PetDPSPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = (value < 0f)?0f:value; }
        }

        /*public float SurvivalPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = (value < 0f)?0f:value; }
        }*/

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
    }
}