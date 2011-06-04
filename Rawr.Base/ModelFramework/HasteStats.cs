using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Base
{
    public class HasteStats
    {
        public double HasteRating;
        public double Haste;

        public double[] HasteProcUptimes;
        public int[] HasteProcRatings;
        public double[] HasteProcMultipliers;

        public double HastedSecond;
        public double HastedGCD;

        public HasteStats(Stats stats, KeyValuePair<double, SpecialEffect>[] hasteProcs)
            : this(stats.SpellHaste, stats.HasteRating, hasteProcs)
        {}

        public HasteStats(double haste, double hasteRating, KeyValuePair<double, SpecialEffect>[] hasteProcs)
        {
            Haste = haste;
            HasteRating = hasteRating;

            HasteProcUptimes = new double[hasteProcs.Length];
            HasteProcRatings = new int[hasteProcs.Length];
            HasteProcMultipliers = new double[hasteProcs.Length];

            for (int i = 0; i < hasteProcs.Length; ++i)
            {
                HasteProcUptimes[i] = hasteProcs[i].Key;
                HasteProcRatings[i] = (int)hasteProcs[i].Value.Stats.HasteRating;
                HasteProcMultipliers[i] = 1 + hasteProcs[i].Value.Stats.SpellHaste;
            }

            // avoid GCD cap
            HastedSecond = computeHastedCastTimeRaw(1 << 16) / 65.536;

            HastedGCD = computeHastedCastTimeRaw(1500);
        }

        public double ComputeTicks(double baseTickRateMS, double baseDurationMS)
        {
            double duration, tps;
            return ComputeTicks(baseTickRateMS, baseDurationMS, out duration, out tps);
        }

        public double ComputeTicks(double baseTickRateMS, double baseDurationMS, out double duration)
        {
            double tps;
            return ComputeTicks(baseTickRateMS, baseDurationMS, out duration, out tps);
        }

        public double ComputeTicks(double baseTickRateMS, double baseDurationMS, out double duration, out double tps)
        {
            //tickRateMS = 0;
            double ticks = 0;
            tps = 0;
            duration = 0;

            int n = HasteProcUptimes.Length;
            int maxmask = 1 << n;
            for (int i = 0; i < maxmask; ++i)
            {
                int hasteRating = 0;
                double hasteFactor = (1 + Haste);
                double fraction = 1.0;
                for (int j = 0; j < n; ++j)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        hasteRating += HasteProcRatings[j];
                        hasteFactor *= HasteProcMultipliers[j];
                        fraction *= HasteProcUptimes[j];
                    }
                    else
                        fraction *= 1 - HasteProcUptimes[j];
                }

                // spell haste and physical haste are actually identically computed in Cataclysm
                double hasteRatingMult = 1.0f + StatConversion.GetSpellHasteFromRating((float)(HasteRating + hasteRating));
                hasteFactor *= hasteRatingMult;
                double curTickRateMS = baseTickRateMS / hasteFactor;
                double curTicks = (double)Math.Ceiling(baseDurationMS / (double)Math.Round(curTickRateMS) - 0.5f);

                //tickRateMS += fraction * curTickRateMS;
                ticks += fraction * curTicks;
                tps += fraction * (1.0 / curTickRateMS);
                duration += fraction * curTicks * curTickRateMS;
            }
            duration /= 1000.0;
            tps *= 1000.0;
            return ticks;
        }

        public double ComputeHastedCastTime(double timeMS)
        {
            if (timeMS <= 1000)
                return 1.0;
            if (timeMS == 1500)
                return HastedGCD;
            return computeHastedCastTimeRaw(timeMS);
        }

        private double computeHastedCastTimeRaw(double timeMS)
        {
            double hastedTimeMS = 0;

            int n = HasteProcUptimes.Length;
            int maxmask = 1 << n;
            for (int i = 0; i < maxmask; ++i)
            {
                int hasteRating = 0;
                double hasteFactor = (1 + Haste);
                double fraction = 1.0;
                for (int j = 0; j < n; ++j)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        hasteRating += HasteProcRatings[j];
                        hasteFactor *= HasteProcMultipliers[j];
                        fraction *= HasteProcUptimes[j];
                    }
                    else
                        fraction *= 1 - HasteProcUptimes[j];
                }

                double hasteRatingMult = 1.0f + StatConversion.GetSpellHasteFromRating((float)(HasteRating + hasteRating));
                hasteFactor *= hasteRatingMult;

                hastedTimeMS += fraction * Math.Max(timeMS / hasteFactor, 1000);
            }
            return hastedTimeMS / 1000.0;
        }
    }
}
