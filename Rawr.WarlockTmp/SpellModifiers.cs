using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {

    public class SpellModifiers {

        public float AdditiveMultiplier { get; private set; }
        public float AdditiveDirectMultiplier { get; private set; }
        public float AdditiveTickMultiplier { get; private set; }
        public float MultiplicativeMultiplier { get; private set; }
        public float MultiplicativeDirectMultiplier { get; private set; }
        public float MultiplicativeTickMultiplier { get; private set; }
        public float CritChance { get; private set; }
        public float CritOverallMultiplier { get; private set; }
        public float CritBonusMultiplier { get; private set; }

        public SpellModifiers() { }

        public void Accumulate(SpellModifiers other) {

            AddAdditiveMultiplier(other.AdditiveMultiplier);
            AddAdditiveDirectMultiplier(other.AdditiveDirectMultiplier);
            AddAdditiveTickMultiplier(other.AdditiveTickMultiplier);
            AddMultiplicativeMultiplier(other.MultiplicativeMultiplier);
            AddMultiplicativeDirectMultiplier(
                other.MultiplicativeDirectMultiplier);
            AddMultiplicativeTickMultiplier(
                other.MultiplicativeTickMultiplier);
            AddCritChance(other.CritChance);
            AddCritOverallMultiplier(other.CritOverallMultiplier);
            AddCritBonusMultiplier(other.CritBonusMultiplier);
        }

        public float GetFinalDirectMultiplier() {

            return (1 + AdditiveMultiplier + AdditiveDirectMultiplier)
                * (1 + MultiplicativeMultiplier)
                * (1 + MultiplicativeDirectMultiplier);
        }

        public float GetFinalTickMultiplier() {

            return (1 + AdditiveMultiplier + AdditiveTickMultiplier)
                * (1 + MultiplicativeMultiplier)
                * (1 + MultiplicativeTickMultiplier);
        }

        public float GetFinalCritMultiplier() {

            float bonus = 1.5f * (1 + CritOverallMultiplier) - 1;
            return 1f + bonus * (1 + CritBonusMultiplier);
        }

        public void AddAdditiveMultiplier(float val) {

            AdditiveMultiplier += val;
        }

        public void AddAdditiveDirectMultiplier(float val) {

            AdditiveDirectMultiplier += val;
        }

        public void AddAdditiveTickMultiplier(float val) {

            AdditiveTickMultiplier += val;
        }

        public void AddMultiplicativeMultiplier(float val) {

            MultiplicativeMultiplier
                = (1 + MultiplicativeMultiplier) * (1 + val) - 1;
        }

        public void AddMultiplicativeDirectMultiplier(float val) {

            MultiplicativeDirectMultiplier
                = (1 + MultiplicativeDirectMultiplier) * (1 + val) - 1;
        }

        public void AddMultiplicativeTickMultiplier(float val) {

            MultiplicativeTickMultiplier
                = (1 + MultiplicativeTickMultiplier) * (1 + val) - 1;
        }

        public void AddCritChance(float val) {

            CritChance += val;
        }

        public void AddCritOverallMultiplier(float val) {

            CritOverallMultiplier = (1 + CritOverallMultiplier) * (1 + val) - 1;
        }

        public void AddCritBonusMultiplier(float val) {

            CritBonusMultiplier = (1 + CritBonusMultiplier) * (1 + val) - 1;
        }
    }
}
