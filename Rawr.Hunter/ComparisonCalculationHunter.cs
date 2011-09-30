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

        private string _desc = string.Empty;
        public override string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        private float _overallPoints = 0f;
        private float[] _subPoints = new float[] { 0f, 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }
        public float HunterDpsPoints
        {
            get { return _subPoints[(int)subpointINDEX.HUNTERDPS]; }
            set { _subPoints[(int)subpointINDEX.HUNTERDPS] = value; }
        }
        public float HunterSurvPoints
        {
            get { return _subPoints[(int)subpointINDEX.HUNTERSURVIVAL]; }
            set { _subPoints[(int)subpointINDEX.HUNTERSURVIVAL] = value; }
        }
        public float PetDpsPoints
        {
            get { return _subPoints[(int)subpointINDEX.PETDPS]; }
            set { _subPoints[(int)subpointINDEX.PETDPS] = value; }
        }
        public float PetSurvPoints
        {
            get { return _subPoints[(int)subpointINDEX.PETSURVIVAL]; }
            set { _subPoints[(int)subpointINDEX.PETSURVIVAL] = value; }
        }
        public override float OverallPoints
        {
            get
            {
                _overallPoints = 0;
                _overallPoints += HunterDpsPoints;
                _overallPoints += HunterSurvPoints;
                _overallPoints += PetDpsPoints;
                _overallPoints += PetSurvPoints;
                return _overallPoints;
            }
            set { _overallPoints = value; }
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

        public override bool PartEquipped { get; set; }

        public override string ToString() {
            return string.Format("{0}: ({1}O {2}HD {3}PD {4}HS {5}PS)",
                Name, Math.Round(OverallPoints), Math.Round(HunterDpsPoints ), Math.Round(PetDpsPoints ),
                                                 Math.Round(HunterSurvPoints), Math.Round(PetSurvPoints));
        }
    }
}
