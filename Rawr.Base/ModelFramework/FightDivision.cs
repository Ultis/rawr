using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.ModelFramework
{
    public class FightDivision
    {
        public SpecialEffect[] Effects;

        public int Count;
        public double[] Fractions;
        public int[] EffectMasks;

        public FightDivision()
        {
            Effects = new SpecialEffect[0];

            Count = 1;
            Fractions = new double[] { 1 };
            EffectMasks = new int[] { 0 };
        }

        public FightDivision(SpecialEffect[] effects, double[] fractions, int[] effectMasks)
        {
            this.Effects = effects;

            this.Count = fractions.Length;
            this.Fractions = fractions;
            this.EffectMasks = effectMasks;
        }


        private class SpecialEffectComparer : IComparer<KeyValuePair<SpecialEffect, int>>
        {
            public int Compare(KeyValuePair<SpecialEffect, int> a, KeyValuePair<SpecialEffect, int> b)
            {
                if (a.Key.Cooldown > b.Key.Cooldown)
                    return -1;
                if (b.Key.Cooldown > a.Key.Cooldown)
                    return 1;
                if (a.Value < b.Value)
                    return -1;
                if (b.Value < a.Value)
                    return 1;
                if (a.Key.Duration < b.Key.Duration)
                    return -1;
                if (b.Key.Duration < a.Key.Duration)
                    return 1;
                return 0;
            }
        }

        // effects with the same cooldown and the same nonnegative bucket number will not be triggered simultaneously
        // otherwise, we naively engage each cooldown ASAP
        public static FightDivision ComputeNaive(SpecialEffect[] effects, double fightLength, int[] buckets = null)
        {
            KeyValuePair<SpecialEffect, int>[] effectsAndBuckets = new KeyValuePair<SpecialEffect, int>[effects.Length];
            for (int i = 0; i < effects.Length; ++i)
                effectsAndBuckets[i] = new KeyValuePair<SpecialEffect, int>(effects[i], (buckets != null) ? buckets[i] : -1);

            Array.Sort(effectsAndBuckets, new SpecialEffectComparer());

            double lastCooldown = double.PositiveInfinity;
            int lastBucket = -1;
            double bucketOffset = 0;

            DoubleMaxHeap<int> pq = new DoubleMaxHeap<int>();

            for (int i = 0; i < effectsAndBuckets.Length; ++i)
            {
                effects[i] = effectsAndBuckets[i].Key;

                if (effects[i].Cooldown != lastCooldown)
                {
                    lastCooldown = effects[i].Cooldown;
                    lastBucket = -1;
                    bucketOffset = 0;
                }

                if (effectsAndBuckets[i].Value != lastBucket)
                {
                    lastBucket = effectsAndBuckets[i].Value;
                    bucketOffset = 0;
                }

                double offset = bucketOffset;
                if (lastBucket >= 0)
                    bucketOffset += effects[i].Duration;

                pq.Push(-offset, i << 1);
            }

            pq.Push(-fightLength, -1);

            Dictionary<int, double> effectDurations = new Dictionary<int, double>();

            int effectMask = 0;
            double time = 0;
            for (; ; )
            {
                double nextTime = -pq.TopKey;
                int id = pq.TopValue;
                pq.Pop();

                if (nextTime != time)
                {
                    double v = 0;
                    effectDurations.TryGetValue(effectMask, out v);
                    effectDurations[effectMask] = v + nextTime - time;
                }

                time = nextTime;

                if (id < 0)
                    break;

                int i = id >> 1;

                if ((id & 1) == 0)
                {
                    effectMask |= 1 << i;
                    pq.Push(-(time + effects[i].Duration), (i << 1) | 1);
                }
                else
                {
                    effectMask &= ~(1 << i);
                    pq.Push(-(time + effects[i].Cooldown - effects[i].Duration), i << 1);
                }
            }

            List<KeyValuePair<int, double>> effectDurationsList = new List<KeyValuePair<int, double>>();
            effectDurationsList.AddRange(effectDurations);
            effectDurationsList.Sort((a, b) => a.Key - b.Key);

            int[] effectMasks = new int[effectDurations.Count];
            double[] fractions = new double[effectDurations.Count];
            {
                int i = 0;
                foreach (KeyValuePair<int, double> effectDuration in effectDurationsList)
                {
                    effectMasks[i] = effectDuration.Key;
                    fractions[i] = effectDuration.Value / fightLength;
                    ++i;
                }
            }

            return new FightDivision(effects, fractions, effectMasks);
        }

        public delegate string DivisionTextDelegate(int div);

        public String GetDivisionDetailTooltip(DivisionTextDelegate del)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Effects.Length; ++i)
            {
                sb.Append((char)((int)'A' + i));
                sb.Append(" = ");
                sb.AppendLine(Effects[i].ToString());
            }

            for (int i = 0; i < EffectMasks.Length; ++i)
            {
                sb.AppendLine();
                for (int j = 0; j < Effects.Length; ++j)
                {
                    if ((EffectMasks[i] & (1 << j)) != 0)
                        sb.Append((char)((int)'A' + j));
                }
                sb.Append(": ");
                sb.Append(del(i));
            }
            return sb.ToString();
        }
    }
}
