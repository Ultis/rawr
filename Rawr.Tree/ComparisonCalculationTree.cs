
namespace Rawr.Tree
{
    public class ComparisonCalculationTree : ComparisonCalculationBase
    {
        public override string Name { get; set;}

        private string _desc = string.Empty;
        public override string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public override float OverallPoints { get; set; }

        private float[] subPoints = new float[] { 0f, 0f, 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
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
        
        public override bool Equipped { get; set; }
        public override bool PartEquipped { get; set; }

        public override string ToString()
        {
            if (subPoints.Length == 1)
                return string.Format("{0}: ({1:0.0}O )", Name, OverallPoints);
            else
        return string.Format("{0}: (Tank ({1:0.0} -> {2:0.0}) Raid ({3:0.0} -> {4:0.0}))", Name, OverallPoints, SubPoints[(int)PointsTree.TankSustained], SubPoints[(int)PointsTree.TankBurst], SubPoints[(int)PointsTree.RaidSustained], SubPoints[(int)PointsTree.RaidBurst]);
        }
    }
}
