using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Sequencer
{
    abstract class Event
    {
        public abstract void process();
    }

    class FlameshockDotEvent : Event
    {
        public float BonusPower { get; set; }

        public override void process()
        {
     	    throw new NotImplementedException();
        }
    }

    class Timeline
    {
        private SortedList<float, CastingState> _timeLine;

        public CastingState this[float t]
        {
            get
            {
                int index = findActiveIndex(t);
                if (index == -1) return null;
                return _timeLine.Values[index];
            }
        }

        private int findActiveIndex(float time)
        {
            if (time <= 0) return -1;
            if (time == 0) return 0;

            IList<float> keys = _timeLine.Keys;
            int first = 0, last = _timeLine.Keys.Count - 1;
            while (first < last)
            {
                int mid = (first + last) / 2;
                int comp = time.CompareTo(keys[mid]);
                if (comp == 0)
                {
                    return mid;
                }
                else if (comp < 0)
                {
                    last = mid;
                }
                else
                {
                    first = mid;
                }
            }

            return last;
        }

        private void split(float time)
        {
            int index = findActiveIndex(time);
            if (index == -1) return;

            CastingState clone = _timeLine.Values[index].Clone();
            _timeLine[time] = clone;
        }

        public void ApplyBuff(CastingState buff, float time, float duration)
        {
            split(time);
            if (duration != 0) split(time + duration);

            int first = _timeLine.IndexOfKey(time);
            int last = _timeLine.Count - 1;
            if (duration != 0) last = _timeLine.IndexOfKey(time + duration) - 1;

            for (int k = first; k <= last; k++)
            {
                _timeLine.Values[k] = _timeLine.Values[k] + buff;
            }
        }
    }

    class Record
    {
        public float LightningBoltCasts { get; set; }
        public float LightningBoltHits { get; set; }
        public float LightningBoltCrits { get; set; }
        public float LightningBoltMisses { get; set; }
        public float LightningBoltBonusPower { get; set; }

        public float LavaBurstCasts { get; set; }
        public float LavaBurstHits { get; set; }
        public float LavaBurstCrits { get; set; }
        public float LavaBurstMisses { get; set; }
        public float LavaBurstBonusPower { get; set; }

        public float FlameShockCasts { get; set; }
        public float FlameShockHits { get; set; }
        public float FlameShockCrits { get; set; }
        public float FlameShockTicks { get; set; }
        public float FlameShockBonusPower { get; set; }
        public float FlameShockTickBonusPower { get; set; }
        
        public float Mana { get; set; }
    }

    public class Sequencer
    {
        private void solve(bool up)
        {
        }

        public void solve(Stats stats, ShamanTalents talents, CalculationOptionsElemental calcOpts)
        {
            // Solve UP and solve DOWN
        }
    }
}
