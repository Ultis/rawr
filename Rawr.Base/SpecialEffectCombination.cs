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
            public double[] d;
            public double[] p;
            public double[] c;
            public float[] triggerInterval;

            public Parameters(SpecialEffectCombination e, float[] triggerInterval, float[] triggerChance, float attackSpeed)
            {
                this.triggerInterval = triggerInterval;
                d = new double[e.effects.Count];
                p = new double[e.effects.Count];
                c = new double[e.effects.Count];

                bool discretizationCorrection = true;

                for (int i = 0; i < e.effects.Count; i++)
                {
                    d[i] = e.effects[i].Duration / triggerInterval[i];
                    p[i] = triggerChance[i] * e.effects[i].GetChance(attackSpeed);
                    c[i] = e.effects[i].Cooldown / triggerInterval[i];
                    if (discretizationCorrection)
                    {
                        c[i] += 0.5;
                    }
                    if (c[i] < 1.0) c[i] = 1.0;
                }
            }
        }

        public SpecialEffectCombination(List<SpecialEffect> effects)
        {
            this.effects = effects;
        }

        public float GetAverageCombinedUptime(float[] triggerInterval, float[] triggerChance, float attackSpeed, float fightDuration)
        {
            // CombinedAverageUptime = integrate_0..fightDuration prod_i Uptime[i](t) dt

            // initialize data, translate into interval time
            Parameters p = new Parameters(this, triggerInterval, triggerChance, attackSpeed);

            // integrate using adaptive Simspon's method
            double totalCombinedUptime = AdaptiveSimpsonsMethod(p, fightDuration, 0.000001, 20);

            return (float)(totalCombinedUptime / fightDuration);
        }

        private double AdaptiveSimpsonsAux(Parameters p, double a, double b, double epsilon, double S, double fa, double fb, double fc, int bottom)
        {
            double c = (a + b) / 2;
            double h = b - a;
            double d = (a + c) / 2;
            double e = (c + b) / 2;
            double fd = GetCombinedUptime(p, d);
            double fe = GetCombinedUptime(p, e);
            double Sleft = (h / 12) * (fa + 4 * fd + fc);
            double Sright = (h / 12) * (fc + 4 * fe + fb);
            double S2 = Sleft + Sright;
            if (bottom <= 0 || Math.Abs(S2 - S) <= 15 * epsilon)
            {
                return S2 + (S2 - S) / 15;
            }
            return AdaptiveSimpsonsAux(p, a, c, epsilon / 2, Sleft, fa, fc, fd, bottom - 1) +
                   AdaptiveSimpsonsAux(p, c, b, epsilon / 2, Sright, fc, fb, fe, bottom - 1);
        }

        private double AdaptiveSimpsonsMethod(Parameters p, double fightDuration, double epsilon, int maxRecursionDepth)
        {
            double a = 0.0;
            double b = fightDuration;
            double c = (a + b) / 2;
            double h = b - a;
            double fa = GetCombinedUptime(p, a);
            double fb = GetCombinedUptime(p, b);
            double fc = GetCombinedUptime(p, c);
            double S = (h / 6) * (fa + 4 * fc + fb);
            return AdaptiveSimpsonsAux(p, a, b, epsilon, S, fa, fb, fc, maxRecursionDepth); 
        }

        private double GetCombinedUptime(Parameters p, double t)
        {
            // Uptime(x) = sum_r=0..inf Ibeta(r+1, x - r * cooldown, p) - Ibeta(r+1, x - duration - r * cooldown, p)
            // t := x * interval
            // Uptime(t) = sum_r=0..inf Ibeta(r+1, t / interval - r * cooldown / interval, p) - Ibeta(r+1, t / interval - duration / interval - r * cooldown / interval, p)

            double combinedUptime = 1.0;

            for (int i = 0; i < effects.Count; i++)
            {
                double x = t / p.triggerInterval[i];

                double uptime = 0.0;
                int r = 1;
                while (x > 0)
                {
                    uptime += SpecialFunction.Ibeta(r, x, p.p[i]);
                    double xd = x - p.d[i];
                    if (xd > 0)
                    {
                        uptime -= SpecialFunction.Ibeta(r, xd, p.p[i]);
                    }
                    r++;
                    x -= p.c[i];
                }
                combinedUptime *= uptime;
            }

            return combinedUptime;
        }
    }
}
