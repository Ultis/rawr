namespace Rawr.Warlock 
{
    public sealed class ComparisonCalculationWarlock : ComparisonCalculationBase 
    {
        public ComparisonCalculationWarlock()
        {
        }

        public ComparisonCalculationWarlock(string name)
        {
            Name = name;
        }

        public override string Name { get; set; }

        public override string Description { get; set; }

        public override float OverallPoints { get; set; }

        private float[] _subPoints = new[] { 0f, 0f };
        public override float[] SubPoints 
        {
            get 
            {
                for (int x = 0; x < _subPoints.Length; x++) 
                { 
                    if (_subPoints[x] < 0f) 
                    { 
                        _subPoints[x] = 0f; 
                    }
                }
                return _subPoints;
            }
            set 
            {
                for (int x = 0; x < _subPoints.Length; x++) 
                { 
                    if (value[x] < 0f) 
                    { 
                        value[x] = 0f; 
                    } 
                }
                _subPoints = value;
            }
        }

        public float DpsPoints 
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = (value < 0f) ? 0f : value; }
        }

        public float PetDPSPoints 
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = (value < 0f) ? 0f : value; }
        }

        public override Item Item { get; set; }

        public override ItemInstance ItemInstance { get; set; }

        public override bool Equipped { get; set; }
    }
}