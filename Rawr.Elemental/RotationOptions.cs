using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
    public class RotationOptions : IRotationOptions
    {
        public RotationOptions(bool useFN, bool useCL)
        {
            UseFireNova = useFN;
            UseChainLightning = useCL;
        }
        #region IRotationOptions Member

        public bool UseFireNova
        {
            get;
            set;
        }

        public bool UseChainLightning
        {
            get;
            set;
        }

        #endregion
    }
}
