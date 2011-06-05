using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.RestoSham
{
    // Calculations, do not edit.
    class NumericField
    {
        public NumericField(string szName, float min, float max, bool bzero)
        {
            PropertyName = szName;
            MinValue = min;
            MaxValue = max;
            CanBeZero = bzero;
        }

        public string PropertyName = string.Empty;
        public float MinValue = float.MinValue;
        public float MaxValue = float.MaxValue;
        public bool CanBeZero = false;
    }
}
