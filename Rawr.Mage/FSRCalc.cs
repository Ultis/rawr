using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Mage
{
    class FSRCalc
    {
        /*private static Dictionary<string, Dictionary<float, float>> Cache = new Dictionary<string, Dictionary<float, float>> ();

        public static bool TryGetCachedOO5SR(string spell, float casttimeHash, out float oo5sr)
        {
            oo5sr = 0;
            Dictionary<float, float> valueCache;
            if (Cache.TryGetValue(spell, out valueCache))
            {
                return valueCache.TryGetValue(casttimeHash, out oo5sr);
            }
            return false;
        }*/

        public List<float> ManaSpentTimestamp = new List<float> ();
        public List<float> ChannelDuration = new List<float> ();
        public float Duration;

        public void AddSpell(float duration, float lag, bool channel)
        {
            if (channel)
            {
                ManaSpentTimestamp.Add(Duration + lag);
                ChannelDuration.Add(duration);
            }
            else
            {
                ManaSpentTimestamp.Add(Duration + duration + lag);
                ChannelDuration.Add(0);
            }
            Duration += duration + lag;
        }

        public void AddPause(float duration)
        {
            Duration += duration;
        }

        public float TimeDiff(int i1, int i2)
        {
            float ret = 0;
            int N = ManaSpentTimestamp.Count;
            while (i1 < 0)
            {
                i1 += N;
                ret += Duration;
            }
            ret += ManaSpentTimestamp[i2] - ManaSpentTimestamp[i1];
            return ret;
        }

        public float CalculateOO5SR(float clearcastingChance/*, string spell, float casttimeHash*/)
        {
            int N = ManaSpentTimestamp.Count;
            float c;
            float ret = 0;
            for (int a = 0; a < N; a++)
            {
                c = 1 - clearcastingChance;
                int b = a - 1;
                float t;
                float t0 = TimeDiff(b, a) - ChannelDuration[((b % N) + N) % N];
                ret += ChannelDuration[(((a - 1) % N) + N) % N];
                do
	            {
	                t = TimeDiff(b, a) - 5;
                    if (t < 0)
                        ret += t0 * c;
                    else if (t < t0)
                        ret += (t0 - t) * c;
                    c *= clearcastingChance;
                    b -= 1;
                } while (t < t0);
            }
            ret = (Duration - ret) / Duration;

            // cache result
            /*Dictionary<float, float> valueCache;
            if (!Cache.TryGetValue(spell, out valueCache))
            {
                valueCache = new Dictionary<float, float>();
                Cache[spell] = valueCache;
            }
            valueCache[casttimeHash] = ret;*/

            return ret;
        }
    }
}
