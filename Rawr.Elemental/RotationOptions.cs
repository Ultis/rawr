using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental
{
    public class RotationOptions : IRotationOptions
    {
        public RotationOptions(bool useFN, bool useCL, bool useDpsFireTotem)
        {
            UseFireNova = useFN;
            UseChainLightning = useCL;
            UseDpsFireTotem = useDpsFireTotem;
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

        public bool UseDpsFireTotem
        {
            get;
            set;
        }

        #endregion
    }
}
