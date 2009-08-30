using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public class SpecialEffectCombination
    {
        private Dictionary<float, Interpolator> interpolator = new Dictionary<float, Interpolator>();
        private List<SpecialEffect> effects;

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
            public float[] d;
            public float[] p;
            public float[] c;
            public float[] o;
            public bool[] a;
            public float[] triggerInterval;

            public Parameters(SpecialEffectCombination e, float[] triggerInterval, float[] triggerChance, float[] offset, float attackSpeed) : this(e, triggerInterval, triggerChance, offset, null, attackSpeed)
            {
                a = new bool[e.effects.Count];

                for (int i = 0; i < e.effects.Count; i++)
                {
                    a[i] = true;
                }
            }

            public Parameters(SpecialEffectCombination e, float[] triggerInterval, float[] triggerChance, float[] offset, bool[] active, float attackSpeed)
            {
                this.triggerInterval = triggerInterval;
                d = new float[e.effects.Count];
                p = new float[e.effects.Count];
                c = new float[e.effects.Count];
                o = new float[e.effects.Count];
                a = active;

                bool discretizationCorrection = true;

                for (int i = 0; i < e.effects.Count; i++)
                {
                    d[i] = e.effects[i].Duration / triggerInterval[i];
                    p[i] = triggerChance[i] * e.effects[i].GetChance(attackSpeed);
                    c[i] = e.effects[i].Cooldown / triggerInterval[i];
                    if (discretizationCorrection)
                    {
                        c[i] += 0.5f;
                    }
                    if (c[i] < 1.0f) c[i] = 1.0f;
                    o[i] = offset[i] / triggerInterval[i];
                }
            }
        }

        public SpecialEffectCombination(List<SpecialEffect> effects)
        {
            this.effects = effects;
        }

        /// <summary>
        /// Computes the average uptime of all effects being active.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds for each effect.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced for each effect.</param>
        /// <param name="offset">Initial cooldown for each effect.</param>
        /// <param name="attackSpeed">Average unhasted attack speed, used in PPM calculations.</param>
        /// <param name="fightDuration">Duration of fight in seconds.</param>
        public float GetAverageCombinedUptime(float[] triggerInterval, float[] triggerChance, float[] offset, float attackSpeed, float fightDuration)
        {
            // CombinedAverageUptime = integrate_0..fightDuration prod_i Uptime[i](t) dt

            // initialize data, translate into interval time
            Parameters p = new Parameters(this, triggerInterval, triggerChance, offset, attackSpeed);

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
        public float GetAverageCombinedUptime(float[] triggerInterval, float[] triggerChance, float[] offset, bool[] active, float attackSpeed, float fightDuration)
        {
            // CombinedAverageUptime = integrate_0..fightDuration prod_i Uptime[i](t) dt

            // initialize data, translate into interval time
            Parameters p = new Parameters(this, triggerInterval, triggerChance, offset, active, attackSpeed);

            // integrate using adaptive Simspon's method
            float totalCombinedUptime = AdaptiveSimpsonsMethod(p, fightDuration, 0.001f, 20);

            return (totalCombinedUptime / fightDuration);
        }

        private float AdaptiveSimpsonsAux(Parameters p, float a, float b, float epsilon, float S, float fa, float fb, float fc, int bottom)
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

        private float AdaptiveSimpsonsMethod(Parameters p, float fightDuration, float epsilon, int maxRecursionDepth)
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

        private float GetCombinedUptime(Parameters p, float t)
        {
            // Uptime(x) = sum_r=0..inf Ibeta(r+1, x - r * cooldown, p) - Ibeta(r+1, x - duration - r * cooldown, p)
            // t := x * interval
            // Uptime(t) = sum_r=0..inf Ibeta(r+1, t / interval - r * cooldown / interval, p) - Ibeta(r+1, t / interval - duration / interval - r * cooldown / interval, p)

            float combinedUptime = 1.0f;

            for (int i = 0; i < effects.Count; i++)
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
                if (p.a[i])
                {
                    combinedUptime *= uptime;
                }
                else
                {
                    combinedUptime *= (1.0f - uptime);
                }
            }

            return combinedUptime;
        }
    }
}
