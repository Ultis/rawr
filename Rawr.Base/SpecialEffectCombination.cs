using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public struct WeightedStat
    {
        public float Chance;
        public float Value;
    }

    public partial class SpecialEffect
    {
        //private Dictionary<float, Interpolator> interpolator = new Dictionary<float, Interpolator>();
        //private List<SpecialEffect> effects;

        /*private class UptimeInterpolator : Interpolator
        {
            protected List<SpecialEffect> effect;

            public UptimeInterpolator(SpecialEffectCombination effect, float fightDuration)
                : base(fightDuration, true)
            {
                this.effect = effect.effects;
            }

            protected override float Evaluate(float[] procChance, float[] interval)
            {
                double d = effect.Duration / interval;
                double d2 = d * 0.5;
                double n = fightDuration / interval;
                double p = procChance;

                double c = effect.Cooldown / interval;
                if (discretizationCorrection)
                {
                    c += 0.5;
                }
                if (c < 1.0) c = 1.0;
                double x = n;

                const double w1 = 5.0 / 9.0;
                const double w2 = 8.0 / 9.0;
                const double k = 0.77459666924148337703585307995648;  //Math.Sqrt(3.0 / 5.0);
                double dx = k * d2;

                double averageUptime = 0.0;
                int r = 1;
                while (x > 0)
                {
                    // integrate_t=(x-duration)..x Ibeta(r, t, p) dt
                    if (x - d > 0)
                    {
                        double tmid = x - d2;
                        averageUptime += (w1 * SpecialFunction.Ibeta(r, tmid - dx, p) + w2 * SpecialFunction.Ibeta(r, tmid, p) + w1 * SpecialFunction.Ibeta(r, tmid + dx, p)) * d2;
                    }
                    else //if (x > 0)
                    {
                        double tmid = x * 0.5;
                        double dt = k * tmid;
                        averageUptime += (w1 * SpecialFunction.Ibeta(r, tmid - dt, p) + w2 * SpecialFunction.Ibeta(r, tmid, p) + w1 * SpecialFunction.Ibeta(r, tmid + dt, p)) * tmid;
                    }
                    r++;
                    x -= c;
                }
                return (float)(averageUptime / n);
            }
        }*/

        private class Parameters
        {
            public SpecialEffect[] effects; /*in*/
            public float[] d; /*in*/
            public float[] p; /*in*/
            public float[] c; /*in*/
            public float[] o; /*in*/
            public float[] triggerInterval; /*in*/
            public float[] uptime; /*out*/
            public float[,] combinedUptime; /*out*/
            public float[,] partialIntegral; /*out*/

            public Parameters(SpecialEffect[] effects, float[] triggerInterval, float[] triggerChance, float[] offset, float attackSpeed)
            {
                this.effects = effects;
                this.triggerInterval = triggerInterval;
                d = new float[effects.Length];
                p = new float[effects.Length];
                c = new float[effects.Length];
                o = new float[effects.Length];

                bool discretizationCorrection = true;

                for (int i = 0; i < effects.Length; i++)
                {
                    d[i] = effects[i].Duration / triggerInterval[i];
                    p[i] = triggerChance[i] * effects[i].GetChance(attackSpeed);
                    c[i] = effects[i].Cooldown / triggerInterval[i];
                    if (discretizationCorrection)
                    {
                        c[i] += 0.5f;
                    }
                    if (c[i] < 1.0f) c[i] = 1.0f;
                    o[i] = offset[i] / triggerInterval[i];
                }
            }
        }

        /// <summary>
        /// Computes the average uptime of all effects being active.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds for each effect.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced for each effect.</param>
        /// <param name="offset">Initial cooldown for each effect.</param>
        /// <param name="attackSpeed">Average unhasted attack speed, used in PPM calculations.</param>
        /// <param name="fightDuration">Duration of fight in seconds.</param>
        public static float GetAverageCombinedUptime(SpecialEffect[] effects, float[] triggerInterval, float[] triggerChance, float[] offset, float attackSpeed, float fightDuration)
        {
            // CombinedAverageUptime = integrate_0..fightDuration prod_i Uptime[i](t) dt

            // initialize data, translate into interval time
            Parameters p = new Parameters(effects, triggerInterval, triggerChance, offset, attackSpeed);

            // integrate using adaptive Simspon's method
            double totalCombinedUptime = AdaptiveSimpsonsMethod(p, fightDuration, 0.001f, 20);

            return (float)(totalCombinedUptime / fightDuration);
        }

        /// <summary>
        /// Computes the average uptime of specific effects being active/inactive.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds for each effect.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced for each effect.</param>
        /// <param name="active">Determines if specific effects are being active/inactive for the uptime calculation.</param>
        /// <param name="offset">Initial cooldown for each effect.</param>
        /// <param name="attackSpeed">Average unhasted attack speed, used in PPM calculations.</param>
        /// <param name="fightDuration">Duration of fight in seconds.</param>
        public static WeightedStat[] GetAverageCombinedUptimeCombinations(SpecialEffect[] effects, float[] triggerInterval, float[] triggerChance, float[] offset, float attackSpeed, float fightDuration, AdditiveStat stat)
        {
            // CombinedAverageUptime = integrate_0..fightDuration prod_i Uptime[i](t) dt

            // initialize data, translate into interval time
            Parameters p = new Parameters(effects, triggerInterval, triggerChance, offset, attackSpeed);
            p.uptime = new float[effects.Length];
            const int maxRecursionDepth = 20;
            p.combinedUptime = new float[1 + 2 * maxRecursionDepth, 1 << effects.Length];
            p.partialIntegral = new float[1 + 2 * maxRecursionDepth, 1 << effects.Length];

            // integrate using adaptive Simspon's method
            AdaptiveSimpsonsMethodCombinations(p, fightDuration, 0.001f, 20);

            WeightedStat[] result = new WeightedStat[1 << effects.Length];

            for (int i = 0; i < (1 << effects.Length); i++)
            {
                result[i].Chance = p.partialIntegral[0, i] / fightDuration;
                for (int j = 0; j < effects.Length; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        result[i].Value += effects[j].Stats._rawAdditiveData[(int)stat];
                    }
                }
            }

            return result;
        }

        private static float AdaptiveSimpsonsAux(Parameters p, float a, float b, float epsilon, float S, float fa, float fb, float fc, int bottom)
        {
            float c = (a + b) / 2;
            float h = b - a;
            float d = (a + c) / 2;
            float e = (c + b) / 2;
            float fd = GetCombinedUptime(p, d);
            float fe = GetCombinedUptime(p, e);
            float Sleft = (h / 12) * (fa + 4 * fd + fc);
            float Sright = (h / 12) * (fc + 4 * fe + fb);
            float S2 = Sleft + Sright;
			if (bottom <= 0 || (h < 10 && Math.Abs(S2 - S) <= 15 * epsilon))
            {
                return S2 + (S2 - S) / 15;
            }
            return AdaptiveSimpsonsAux(p, a, c, epsilon / 2, Sleft, fa, fc, fd, bottom - 1) +
                   AdaptiveSimpsonsAux(p, c, b, epsilon / 2, Sright, fc, fb, fe, bottom - 1);
        }

        private static void AdaptiveSimpsonsAuxCombinations(Parameters p, float a, float b, float epsilon, int S, int fa, int fb, int fc, int bottom, int level)
        {
            float c = (a + b) / 2;
            float h = b - a;
            float d = (a + c) / 2;
            float e = (c + b) / 2;
            int fd = 1 + 2 * level;
            GetCombinedUptimeCombinations(p, d, fd);
            int fe = 2 + 2 * level;
            GetCombinedUptimeCombinations(p, e, fe);
            int Sleft = 2 * level - 1;
            int Sright = 2 * level;
            bool terminate = true;
            for (int i = 0; i < (1 << p.effects.Length); i++)
            {
                p.partialIntegral[Sleft, i] = (h / 12) * (p.combinedUptime[fa, i] + 4 * p.combinedUptime[fd, i] + p.combinedUptime[fc, i]);
                p.partialIntegral[Sright, i] = (h / 12) * (p.combinedUptime[fc, i] + 4 * p.combinedUptime[fe, i] + p.combinedUptime[fb, i]);
                if (!(bottom <= 0 || (h < 10 && Math.Abs(p.partialIntegral[Sleft, i] + p.partialIntegral[Sright, i] - p.partialIntegral[S, i]) <= 15 * epsilon)))
                {
                    terminate = false;
                }
            }
            float S2 = Sleft + Sright;
            if (terminate)
            {
                for (int i = 0; i < (1 << p.effects.Length); i++)
                {
                    p.partialIntegral[S, i] = (p.partialIntegral[Sleft, i] + p.partialIntegral[Sright, i]) * 16.0f / 15.0f - p.partialIntegral[S, i] / 15.0f;
                }
            }
            else
            {
                AdaptiveSimpsonsAuxCombinations(p, a, c, epsilon / 2, Sleft, fa, fc, fd, bottom - 1, level + 1);
                AdaptiveSimpsonsAuxCombinations(p, c, b, epsilon / 2, Sright, fc, fb, fe, bottom - 1, level + 1);
                for (int i = 0; i < (1 << p.effects.Length); i++)
                {
                    p.partialIntegral[S, i] = p.partialIntegral[Sleft, i] + p.partialIntegral[Sright, i];
                }
            }
        }

        private static float AdaptiveSimpsonsMethod(Parameters p, float fightDuration, float epsilon, int maxRecursionDepth)
        {
            float a = 0.0f;
            float b = fightDuration;
            float c = (a + b) / 2;
            float h = b - a;
            float fa = GetCombinedUptime(p, a);
            float fb = GetCombinedUptime(p, b);
            float fc = GetCombinedUptime(p, c);
            float S = (h / 6) * (fa + 4 * fc + fb);
            return AdaptiveSimpsonsAux(p, a, b, epsilon, S, fa, fb, fc, maxRecursionDepth); 
        }

        private static void AdaptiveSimpsonsMethodCombinations(Parameters p, float fightDuration, float epsilon, int maxRecursionDepth)
        {
            float a = 0.0f;
            float b = fightDuration;
            float c = (a + b) / 2;
            float h = b - a;
            int fa = 0;
            GetCombinedUptimeCombinations(p, a, fa);
            int fb = 1;
            GetCombinedUptimeCombinations(p, b, fb);
            int fc = 2;
            GetCombinedUptimeCombinations(p, c, fc);
            int S = 0;
            for (int i = 0; i < (1 << p.effects.Length); i++)
            {
                p.partialIntegral[0, i] = (h / 6) * (p.combinedUptime[fa, i] + 4 * p.combinedUptime[fc, i] + p.combinedUptime[fb, i]);
            }
            AdaptiveSimpsonsAuxCombinations(p, a, b, epsilon, S, fa, fb, fc, maxRecursionDepth, 1);
        }

        private static float GetCombinedUptime(Parameters p, float t)
        {
            // Uptime(x) = sum_r=0..inf Ibeta(r+1, x - r * cooldown, p) - Ibeta(r+1, x - duration - r * cooldown, p)
            // t := x * interval
            // Uptime(t) = sum_r=0..inf Ibeta(r+1, t / interval - r * cooldown / interval, p) - Ibeta(r+1, t / interval - duration / interval - r * cooldown / interval, p)

            float combinedUptime = 1.0f;

            for (int i = 0; i < p.effects.Length; i++)
            {
                float x = t / p.triggerInterval[i] - p.o[i];

                float uptime = 0.0f;
                int r = 1;
                while (x > 0)
                {
                    uptime += SpecialFunction.IbetaInterpolated(r, x, p.p[i]);
                    float xd = x - p.d[i];
                    if (xd > 0)
                    {
                        uptime -= SpecialFunction.IbetaInterpolated(r, xd, p.p[i]);
                    }
                    r++;
                    x -= p.c[i];
                }
                combinedUptime *= uptime;
            }

            return combinedUptime;
        }

        private static void GetCombinedUptimeCombinations(Parameters p, float t, int index)
        {
            // Up time(x) = sum_r=0..inf Ibeta(r+1, x - r * cooldown, p) - Ibeta(r+1, x - duration - r * cooldown, p)
            // t := x * interval
            // Uptime(t) = sum_r=0..inf Ibeta(r+1, t / interval - r * cooldown / interval, p) - Ibeta(r+1, t / interval - duration / interval - r * cooldown / interval, p)

            for (int i = 0; i < p.effects.Length; i++)
            {
                float x = t / p.triggerInterval[i] - p.o[i];

                p.uptime[i] = 0.0f;
                int r = 1;
                while (x > 0)
                {
                    p.uptime[i] += SpecialFunction.IbetaInterpolated(r, x, p.p[i]);
                    float xd = x - p.d[i];
                    if (xd > 0)
                    {
                        p.uptime[i] -= SpecialFunction.IbetaInterpolated(r, xd, p.p[i]);
                    }
                    r++;
                    x -= p.c[i];
                }
            }

            for (int i = 0; i < (1 << p.effects.Length); i++)
            {
                p.combinedUptime[index, i] = 1.0f;
                for (int j = 0; j < p.effects.Length; j++)
                {
					if ((i & (1 << j)) == 0)
					{
						p.combinedUptime[index, i] *= (1.0f - p.uptime[j]);
					}
					else
					{
						p.combinedUptime[index, i] *= p.uptime[j];
					}
				}
            }
        }
    }
}
