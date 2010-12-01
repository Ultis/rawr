using System;

namespace Rawr.RestoSham
{
    public sealed class ComparisonCalculationRestoSham : ComparisonCalculationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonCalculationRestoSham"/> class.
        /// </summary>
        public ComparisonCalculationRestoSham()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonCalculationRestoSham"/> class.
        /// </summary>
        /// <param name="szName">The name of the comparision.</param>
        public ComparisonCalculationRestoSham(String szName)
            : base()
        {
            this.Name = szName;
        }

        private string _name = string.Empty;
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

        private float _overallPoints = 0.0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }


        private float[] _SubPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _SubPoints; }
            set { _SubPoints = value; }
        }

        /// <summary>
        /// The Item, or other object, being rated. This property is used to build the tooltip for this
        /// object in the chart. If this is null, no tooltip will be displayed. If the object is not an
        /// Item, a new blank item may be created for this field, containing just a Name and Stats.
        /// </summary>
        public override Item Item
        {
            get;
            set;
		}

        /// <summary>
        /// The ItemInstance being rated. This property is used to build the tooltip for this
        /// object in the chart.
        /// </summary>
		public override ItemInstance ItemInstance
		{
            get;
            set;
		}

        /// <summary>
        /// Whether the object being rated is currently equipped by the character. This controls whether
        /// the item's label is highlighted in light green on the charts.
        /// </summary>
        public override bool Equipped
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the object being rated is currently partially equipped by the character. This controls whether
        /// the item's label is highlighted in a lighter green on the charts.
        /// </summary>
        public override bool PartEquipped { get; set; }
    }
}
