using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    class CharacterCalculationsWarlock : CharacterCalculationsBase
    {

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

      
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            throw new NotImplementedException();
      
        }
    }
}
