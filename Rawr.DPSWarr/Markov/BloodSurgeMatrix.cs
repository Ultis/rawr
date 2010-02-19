using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Rawr.DPSWarr.Markov;
using Rawr.Base;
using Rawr.Bosses;

namespace Rawr.DPSWarr.Markov
{
    class BloodSurgeMatrix
    {
        private double[] _procChances;
        public double ProcChances(int index)
        {
            if (index < 0 || index > 2) return 0;
            return _procChances[index];
        }
        
        public BloodSurgeMatrix()
        {
            _procChances = new double[] {1, 0, 0};
        }

        public void AddAbility(double chance1, double chance2, double numActivates)
        {
            double[] newChances = new double[3];
            if (numActivates != 1)
            {
                newChances[2] = 1 - Math.Pow(1 - chance2, numActivates);
                newChances[1] = 1 - Math.Pow(1 - chance1, numActivates);
                newChances[0] = 1 - newChances[2] - newChances[1];
            }
            ResetMatrix(newChances);
        }

        public void AddAbility(double chance1, double chance2)
        {
            double[] newChances = new double[] { 1 - chance2 - chance1, chance1, chance2 };
            ResetMatrix(newChances);
        }

        private void ResetMatrix(double[] chances)
        {
            double[] ret = new double[3];
            
            ret[0] = _procChances[0] * chances[0];

            ret[1] = _procChances[1] * chances[0] +
                     _procChances[0] * chances[1] +
                     _procChances[1] * chances[1];

            ret[2] = 1 - ret[1] - ret[0];

            _procChances = ret;
        }
    }
}
