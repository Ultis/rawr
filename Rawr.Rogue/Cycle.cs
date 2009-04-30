using System;
using System.Collections.Generic;
using Rawr.Rogue.FinishingMoves;

namespace Rawr.Rogue
{
    [Serializable]
    public class Cycle
    {
        private readonly List<CycleComponent> components = new List<CycleComponent>();

        public int TotalComboPoints
        {
            get
            {
                var ret = 0;
                for (var i = 0; i < components.Count; i++)
                    ret += (components[i]).Rank;
                return ret;
            }
        }

        public override string ToString()
        {
            return string.Join("", components.ConvertAll(c => c.ToString()).ToArray());
        }

        public List<CycleComponent> Components
        {
            get { return components; }
        }
    }

    [Serializable]
    public class CycleComponent
    {
        private FinisherBase _finisher;
        private int _rank;

        public CycleComponent()
        {
            _rank = 0;
            _finisher = new NoFinisher();
        }

        public CycleComponent(int r, FinisherBase f)
        {
            _rank = r;
            _finisher = f;
        }

        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public FinisherBase Finisher
        {
            get { return _finisher; }
            set { _finisher = value; }
        }

        public float CalcFinisherDPS( RogueTalents talents, Stats stats, CombatFactors combatFactors, float cycleTime, WhiteAttacks whiteAttacks )
        {
            return _finisher.CalcFinisherDPS(stats, combatFactors, _rank, cycleTime, whiteAttacks);
        }

        public override string ToString()
        {
            return _rank == 0 || _finisher.Id == new NoFinisher().Id ? "" : _rank + _finisher.Id.ToString();
        }
    }
}