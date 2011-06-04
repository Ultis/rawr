/* Action framework (currently used by Rawr.Tree)
 * 
 * This file contains code to model fights by dividing them in several divisions,
 * and dividing the fight time among several "actions"
 * 
 * Division is generally performed depending on which on-use effects are up:
 * the FightDivision class will automatically compute such a division.
 * 
 * Actions are defined as sequences of spell which are used together as a unit:
 * "per-second" here refers to cast-time seconds: E stands for "effect" and usually means damage or healing.
 * 
 * An ActionDistribution represents the distribution of available time and resources
 * (usually mana) among several possible actions.
 * 
 * In addition, "passive" entries are provided to record effects that do not need any cast time allocation to happen.
 * 
 * ActionDistributionsByDivision represents a set of ActionDistributions, one for each division.
 * 
 * ActionOptimization allows to automatically assign time and mana to actions in a set of ActionDistributions.
 * 
 * A model would normally manually add actions to be used on cooldown, or representing HoTs or DoT to be always kept up.
 * After that, it would use ActionOptimization to add spammable filler actions optimally.
 * 
 * ActionOptimization uses a greedy algorithm, which allows to more quickly solve the special linear program denoting action optimization.
 * 
 * The linear program is this:
 * maximize sum eps_{action,division} * x_{action,division}
 * sum_action t_{action,division} <= divtime_division
 * sum_action mps_{action,division} * x_{action,division} = manausage_division
 * sum divtime_division * manausage_division <= manaregen
 * x_{action,division} >= 0
 * 
 * where x_{action,division} is the amount of time in division devoted to using action
 * 
 * Restricting to a single division we have:
 * maximize sum eps_{action,division} * x_{action,division}
 * sum_action t_{action,division} <= divtime_division
 * sum_action mps_{action,division} * x_{action,division} < manabudget_division
 * x_{action,division} >= 0
 * 
 * Since the solution space is compact, all but 2 of these constraints must be tight, which means that at most 2 actions are going to be used.
 * We thus have three cases:
 * - Mana bound: spamming the most efficient action, with some idle time
 * - Time bound: spamming the highest HPS/DPS action, with some unused mana
 * - Mana/time bound: splitting time between two actions, to use both all available time and all available mana
 * 
 * We can represent the HPS and MPS of each action as points in 2D space.
 * In this case, each solution is either one of the points, or an interpolation of two points, thus lying on the segment connecting them.
 * 
 * The solution is then easily determined depending on the mana budget as the point on the convex hull where the MPS coordinate matches the chosen mana budget, and which maximizes the HPS/DPS.
 * Obviously, this point is the unique point which lies on the border of the convex hull, in the upper part relative to EPS, and the "ascending part" relative to MPS.
 * Note that all these points need to be scaled by the uptime of the division in question.
 * 
 * Considering the whole problem, all except #divisions + 1 constraints must be tight; since for each division, we can have at most two used actions,
 * this means that we use a single action in all divisions, except at most one where we use two.
 * 
 * For a solution to be optimal, it must be impossible to transfer mana from a division to another, producing an HPS improvement.
 * This means that all convex points must have "the same tangent" on each selected point.
 * 
 * More precisely, all the "left tangents" must be less than equal than the "right tangents" at the selected solution points on the convex hull for each division.
 * This means that, for each a slope, we have can easily find an optimal solution (or more solutions if it exactly matches the slope of multiple segments).
 * As the slope is decreased from infinity to 0, the mana usage will increase from 0 to the maximum possible.
 * 
 * Hence, this suggests a simple greedy algorithm, which is the one we use.
 * 
 * After precomputing the convex hull for each division, we store the "current point" for each division.
 * Then, we simply pick the point with the highest right tangent slope, and move on that segment until we run out of mana or reach the endpoint.
 * We then continue until all mana has been allocated, or the highest DPS/HPS spells are being used in all divisions.
 * 
 * Intuitively, we are first spending mana on the most efficient action overall, and later spending it where the relative efficiency ((EPS_newspell - EPS_oldspell) / (EPM_newspell - EPM_oldspell)) is maximized, until we run out.
 * 
 * Note that this algorithm only works for actions that are usable without restriction.
 * Actions that are limited by a cooldown cannot be directly modelled in this way.
 * The simplest way is to manually add those on cooldowns.
 * It is however possible to create a combination action for each basic action and combinations of cooldowns
 * (with the cooldowns used for an appropriate fraction of the time, and the rest spent on the action).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Base
{
    public struct DiscreteAction
    {
        public double Time;
        public double Mana;
        public double Direct;
        public double Periodic;
        public double Casts;
        public double Ticks;
    }

    public struct ContinuousAction
    {
        public double Time;
        public double MPS;
        public double EPS;
        public double CPS;
        public double TPS;

        public string EPSText { get { return String.Format("{0:F0} = {1:F0}/{2:F2}", EPS, EPS * Time, Time); }}
        public string MPSText { get { return String.Format("{0:F0} = {1:F0}/{2:F2}", MPS, MPS * Time, Time); } }
        public string EPMText { get { return String.Format("{0:F2} = {1:F0}/{2:F0}", EPS / MPS, EPS * Time, MPS * Time); } }

        public ContinuousAction(DiscreteAction pa)
        {
            Time = pa.Time;
            MPS = pa.Mana;
            EPS = pa.Direct + pa.Periodic;
            CPS = pa.Casts;
            TPS = pa.Ticks;
            if (Time != 0)
            {
                MPS /= Time;
                EPS /= Time;
                CPS /= Time;
                TPS /= Time;
            }
        }

        public static ContinuousAction[] AverageActionSets(ContinuousAction[][] actionSets, double[] weights)
        {
            int n = actionSets[0].Length;
            ContinuousAction[] actions = new ContinuousAction[n];
            double[] totalWeight = new double[n];
            for (int i = 0; i < weights.Length; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (actionSets[i][j].Time > 0)
                    {
                        totalWeight[j] += weights[i];
                        actions[j].EPS += weights[i] * actionSets[i][j].EPS;
                        actions[j].MPS += weights[i] * actionSets[i][j].MPS;
                        actions[j].Time += weights[i] * actionSets[i][j].Time;
                    }
                }
            }

            for (int j = 0; j < n; ++j)
            {
                if (totalWeight[j] > 0)
                {
                    actions[j].EPS /= totalWeight[j];
                    actions[j].MPS /= totalWeight[j];
                    actions[j].Time /= totalWeight[j];
                }
            }
            return actions;
        }
    };

    public class ActionDistribution
    {
        private ContinuousAction[] actions;
        private double[] fraction;
        private double[] mps;
        private double[] eps;
        private double[] cps;
        private double[] tps;
        private double totalFraction = 0.0f;
        private double totalMPS = 0.0f;
        private double maxFraction = 1.0f;
        private double maxMPS = double.PositiveInfinity;
        private int passives;
        private double[] passiveEPS;
        private double[] passiveMPS;
        private double[] passiveTPS;

        public ContinuousAction[] Actions { get { return actions; } }
        public int Passives { get { return passives; } }

        public double TotalMPS { get { return totalMPS; } }
        public double MaxMPS { get { return maxMPS; } set { maxMPS = value; } }
        public double AvailMPS { get { return maxMPS - totalMPS; } }

        public double TotalFraction { get { return totalFraction; } }
        public double MaxFraction { get { return maxFraction; } }
        public double AvailFraction { get { return maxFraction - totalFraction; } }
        
        public double PassiveEPS(int i)
        {
            return passiveEPS[i];
        }

        public double PassiveMPS(int i)
        {
            return passiveMPS[i];
        }

        public double Fraction(int i)
        {
            return fraction[i];
        }

        public double EPS(int i)
        {
            return eps[i];
        }

        public double MPS(int i)
        {
            return mps[i];
        }

        public double CPS(int i)
        {
            return cps[i];
        }

        public double TPS(int i)
        {
            return tps[i];
        }

        public double TotalEPS()
        {
            double res = 0;
            for (int i = 0; i < eps.Length; ++i)
                res += eps[i];
            for (int i = 0; i < passives; ++i)
                res += passiveEPS[i];
            return res;
        }

        public double TotalCPS()
        {
            double res = 0;
            for (int i = 0; i < eps.Length; ++i)
                res += cps[i];
            return res;
        }

        public double TotalTPS()
        {
            double res = 0;
            for (int i = 0; i < tps.Length; ++i)
                res += tps[i];
            for (int i = 0; i < passives; ++i)
                res += passiveTPS[i];
            return res;
        }

        public ActionDistribution Clone()
        {
            ActionDistribution clone = (ActionDistribution)this.MemberwiseClone();
            clone.fraction = (double[])clone.fraction.Clone();
            clone.eps = (double[])clone.eps.Clone();
            clone.mps = (double[])clone.mps.Clone();
            clone.cps = (double[])clone.cps.Clone();
            clone.tps = (double[])clone.tps.Clone();
            clone.passiveEPS = (double[])clone.passiveEPS.Clone();
            clone.passiveMPS = (double[])clone.passiveMPS.Clone();
            clone.passiveTPS = (double[])clone.passiveTPS.Clone();
            return clone;
        }


        public ActionDistribution(ContinuousAction[] actions, int passives)
        {
            this.passives = passives;
            this.actions = actions;
            fraction = new double[actions.Length];
            mps = new double[actions.Length];
            eps = new double[actions.Length];
            cps = new double[actions.Length];
            tps = new double[actions.Length];
            passiveEPS = new double[passives];
            passiveMPS = new double[passives];
            passiveTPS = new double[passives];
        }

        public void AddPassiveTPS(int passive, double tps)
        {
            passiveTPS[passive] += tps;
        }

        public void AddPassive(int passive, double eps)
        {
            passiveEPS[passive] += eps;
        }

        public void AddPassive(int passive, double eps, double mps)
        {
            passiveEPS[passive] += eps;
            passiveMPS[passive] += mps;
            totalMPS += mps;
        }

        public void AddAction(int action, double t)
        {
            t = Math.Min(t, maxFraction - totalFraction);
            if(actions[action].MPS > 0)
                t = Math.Min(t, (maxMPS - totalMPS) / actions[action].MPS);
            if (t <= 0.0)
                return;
            
            fraction[(int)action] += t;
            totalFraction += t;
            eps[action] += actions[action].EPS * t;
            tps[action] += actions[action].TPS * t;
            cps[action] += actions[action].CPS * t;
            double cmps = actions[action].MPS * t;
            mps[action] += cmps;
            totalMPS += cmps;
        }

        public void AddActionPeriodically(int action, double period)
        {
            AddAction(action, actions[(int)action].Time / period);
        }

        public void AddBestActions(int[] actions)
        {
            ActionOptimization.AddBestActions(new ActionDistribution[] { this }, new double[] { 1 }, actions);
        }

        public static ActionDistribution Combine(ActionDistribution[] dists, double[] coeffs)
        {
            ActionDistribution dist = new ActionDistribution(dists[0].actions, dists[0].passives);
            int e = dist.fraction.Length;
            int p = dist.passives;
            int n = dists.Length;
            dist.maxFraction = 0;
            dist.maxMPS = 0;

            for(int i = 0; i < n; ++i)
            {
                dist.totalMPS += dists[i].totalMPS * coeffs[i];
                dist.totalFraction += dists[i].totalFraction * coeffs[i];
                dist.maxMPS += dists[i].maxMPS * coeffs[i];
                dist.maxFraction += dists[i].maxFraction * coeffs[i];
                for (int j = 0; j < e; ++j)
                {
                    dist.fraction[j] += dists[i].fraction[j] * coeffs[i];
                    dist.eps[j] += dists[i].eps[j] * coeffs[i];
                    dist.mps[j] += dists[i].mps[j] * coeffs[i];
                    dist.tps[j] += dists[i].tps[j] * coeffs[i];
                    dist.cps[j] += dists[i].cps[j] * coeffs[i];
                }
                for (int j = 0; j < p; ++j)
                    dist.passiveEPS[j] += dists[i].passiveEPS[j] * coeffs[i];
                for (int j = 0; j < p; ++j)
                    dist.passiveMPS[j] += dists[i].passiveMPS[j] * coeffs[i];
                for (int j = 0; j < p; ++j)
                    dist.passiveTPS[j] += dists[i].passiveTPS[j] * coeffs[i];
            }
            return dist;
        }

        public void GetProperties(IDictionary<string, string> dict, string prefix, string[] actionNames, string[] passiveNames)
        {
            new ActionDistributionsByDivision(new FightDivision(), new ActionDistribution[] { this }).GetProperties(dict, prefix, actionNames, passiveNames);
        }
    }

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
                effectsAndBuckets[i] = new KeyValuePair<SpecialEffect,int>(effects[i], (buckets != null) ? buckets[i] : -1);

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
                if(lastBucket >= 0)
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

            List<KeyValuePair<int, double>> effectDurationsList = new List<KeyValuePair<int,double>>();
            effectDurationsList.AddRange(effectDurations);
            effectDurationsList.Sort((a,b) => a.Key - b.Key);

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

    public class ActionDistributionsByDivision
    {
        public FightDivision Division;
        public ActionDistribution Distribution;
        public ActionDistribution[] Distributions;

        public ActionDistributionsByDivision(FightDivision division, ActionDistribution[] dists)
        {
            Division = division;
            Distributions = dists;
            Distribution = ActionDistribution.Combine(dists, Division.Fractions);
        }

        public void GetProperties(IDictionary<string, string> dict, string prefix, string[] actionNames, string[] passiveNames)
        {
            double totalEPS = Distribution.TotalEPS();
            double[] divTotalEPS = new double[Division.Count];
            for (int div = 0; div < Division.Count; ++div)
                divTotalEPS[div] = Distributions[div].TotalEPS();

            for (int j = 0; j < Distribution.Actions.Length; ++j)
            {
                if (Distribution.Fraction(j) > 0)
                {
                    string s = String.Format("{0:F0} = {1:F1}% ({2:F1}t, {3:F0}m)", Distribution.EPS(j), Distribution.EPS(j) / totalEPS * 100.0f, Distribution.Fraction(j) * 100.0f, Distribution.MPS(j));
                    if (Division.Count > 1)
                        s += "*" + Division.GetDivisionDetailTooltip(div => String.Format("{0:F0} = {1:F1}% ({2:F1}t, {3:F0}m)", Distributions[div].EPS(j), Distributions[div].EPS(j) / divTotalEPS[div] * 100.0f, Distributions[div].Fraction(j) * 100.0f, Distributions[div].MPS(j)));
                    dict.Add(prefix + actionNames[j], s);
                }
                else
                    dict.Add(prefix + actionNames[j], "");
            }

            for (int j = 0; j < Distribution.Passives; ++j)
            {
                if (Distribution.PassiveEPS(j) > 0 || Distribution.PassiveMPS(j) > 0)
                {
                    string s = String.Format("{0:F0} = {1:F1}% ({2:F0}m)", Distribution.PassiveEPS(j), Distribution.PassiveEPS(j) / totalEPS * 100.0f, Distribution.PassiveMPS(j));
                    if(Division.Count > 1)
                        s += "*" + Division.GetDivisionDetailTooltip(div => String.Format("{0:F0} = {1:F1}% ({2:F0}m)", Distributions[div].PassiveEPS(j), Distributions[div].PassiveEPS(j) / divTotalEPS[div] * 100.0f, Distributions[div].PassiveMPS(j)));
                    dict.Add(prefix + passiveNames[j], s);
                }
                else
                    dict.Add(prefix + passiveNames[j], "");
            }

            {
                string s = String.Format("{0:F0}t, {1:F0}m", (1 - Distribution.TotalFraction) * 100.0f, Distribution.MaxMPS - Distribution.TotalMPS);
                if(Division.Count > 1)
                    s += "*" + Division.GetDivisionDetailTooltip(div => String.Format("{0:F0}%, {1:F0}m", (Distributions[div].MaxFraction - Distributions[div].TotalFraction) * 100.0f, Distributions[div].MaxMPS - Distributions[div].TotalMPS));
                dict.Add(prefix + "Idle", s);
            }
        }
    }

    // based on Rawr.Mage.Heap
    public class DoubleMaxHeap<T>
    {
        private int cap;
        private double[] keys;
        private T[] values;
        private int mCount;

        public DoubleMaxHeap(int capacity = 16)
        {
            this.cap = capacity;
            this.keys = new double[this.cap + 1];
            this.values = new T[this.cap + 1];
        }

        public void Clear()
        {
            this.mCount = 0;
        }

        public void Pop()
        {
            if (mCount == 0)
                return;
            double insertKey = keys[mCount];
            T insertValue = values[mCount];
            --mCount;
            int i = 1;
            while ((i << 1) <= mCount)
            {
                int maxChild = i << 1;
                if (((maxChild + 1) <= mCount) && keys[maxChild + 1] > keys[maxChild])
                    ++maxChild;
                if (insertKey >= keys[maxChild])
                    break;
                keys[i] = keys[maxChild];
                values[i] = values[maxChild];
                i = maxChild;
            }
            keys[i] = insertKey;
            values[i] = insertValue;
        }

        public void Push(double key, T value)
        {
            if (this.mCount == cap)
            {
                cap *= 2;
                double[] newKeys = new double[cap + 1];
                T[] newValues = new T[cap + 1];
                Array.Copy(values, newValues, values.Length);
                Array.Copy(keys, newKeys, keys.Length);
                keys = newKeys;
                values = newValues;
            }
            this.mCount++;
            int i = this.mCount;
            while ((i > 1) && keys[i >> 1] < key)
            {
                keys[i] = keys[i >> 1];
                values[i] = values[i >> 1];
                i >>= 1;
            }
            keys[i] = key;
            values[i] = value;
        }

        public int Count
        {
            get
            {
                return this.mCount;
            }
        }

        public double TopKey
        {
            get
            {
                return keys[1];
            }
        }

        public T TopValue
        {
            get
            {
                return values[1];
            }
        }
    }

    // we use the same array for all divisions to reduce memory allocation overhead and improve cache behavior
    public class ActionOptimization
    {
        private static double ccw(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            return (x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1);
        }

        // clockwise left-to-right excluding left vertical collinear points if any
        // INPUT: coordinates in x/y[start .. start + n], scratch is a scratch array of size >=n
        // OUTPUT: returns outsize (size of convex hull), filtered and reordered coordinates in x/y[start .. start + outsize], original offsets in points[start .. start + outsize]    
        //
         // NOTE: x and y will ALSO be resorted in convex hull order
        // Output placed in points, starting at start (must have x.Length space)
        // Output is in the start ... start + n range
        public static int FindUpperIncreasingConvexHullPoints(double[] x, double[] y, int[] points, int start, int n, double[] scratch)
        {
            if (n == 0)
                return 0;
            int end = start + n;
            
            int npoints = start;

            for (int i = 0; i < n; ++i)
                points[start + i] = start + i;

            Array.Sort(x, points, start, n);

            for (int i = 0; i < n; ++i)
                scratch[i] = y[points[start + i]];

            for (int i = 0; i < n; ++i)
                y[start + i] = scratch[i];

            {
                int i;
                double minX = x[start];
                double lastY = y[start];
                for (i = start + 1; i < end; ++i)
                {
                    double cx = x[i];
                    if(cx != minX)
                        break;

                    double cy = y[i];
                    if (cy > lastY)
                    {
                        points[start] = points[i];
                        y[start] = lastY = cy;
                    }
                }
                ++npoints;
                
                for (; i < end; ++i)
                {
                    double cx = x[i];
                    double cy = y[i];

                    if (cy <= lastY)
                        continue;

                    while (npoints > (start + 1))
                    {
                        int a = npoints - 2;
                        int b = npoints - 1;
                        if (ccw(x[a], y[a], x[b], y[b], cx, cy) >= 0)
                            --npoints;
                        else
                            break;
                    }
                    lastY = cy;
                    x[npoints] = cx;
                    y[npoints] = cy;
                    points[npoints] = points[i];
                    ++npoints;
                }
            }

            return npoints - start;
        }

        /* PROBLEM DESCRIPTION:
         * We have a set budget, that we need to split between agents who can divide their time between a set of actions, to maximize the resulting gain.
         * Must contain a 0 cost action in each set
         * 
         * OUTPUT:
         * selections[] is the selected action for each agent
         * if interpSet is >= 0, then it denotes the only agent that splits its time between actions: (1 - interpT) time doing selections[interpSet] and interpT time doing interpTarget
         * 
         * NOTE: will clobber costs and gains arrays
         */
        public static void FindBestBudgetSplit(double[] costs, double[] gains, int l, int n, double budget, out int[] selections, out int interpSet, out double interpT, out int interpTarget)
        {
            int[] hulls = new int[n * l];
            int[] hullends = new int[n];

            double[] scratch = new double[l];
            for (int i = 0; i < n; ++i)
                hullends[i] = i * l + FindUpperIncreasingConvexHullPoints(costs, gains, hulls, i * l, l, scratch);

            DoubleMaxHeap<int> pq = new DoubleMaxHeap<int>();
            int[] cursors = new int[n];

            for (int i = 0; i < n; ++i)
            {
                int c = i * l;
                cursors[i] = c;
                if ((c + 1) < hullends[i])
                    pq.Push((gains[c + 1] - gains[c]) / (costs[c + 1] - costs[c]), i);
            }

            interpSet = -1;
            interpT = 0;
            interpTarget = -1;

            while (pq.Count > 0 && budget > 0)
            {
                int i = pq.TopValue;
                pq.Pop();

                int c = cursors[i];
                double deltacost = costs[c + 1] - costs[c];
                if (deltacost < budget)
                {
                    budget -= deltacost;
                    
                    cursors[i] = ++c;
                    if ((c + 1) < hullends[i])
                        pq.Push((gains[c + 1] - gains[c]) / (costs[c + 1] - costs[c]), i);
                }
                else
                {
                    interpSet = i;
                    interpT = budget / deltacost;
                    interpTarget = hulls[c + 1] - i * l;
                    break;
                }
            }

            selections = new int[n];
            for (int i = 0; i < n; ++i)
                selections[i] = hulls[cursors[i]] - i * l;
        }

        public static void FindBestActions(ContinuousAction[][] actionSets, double[] factors, int[] actions, double mpsLeft, out int[] selectedActions, out int interpSet, out double interpT, out int interpTargetAction)
        {
            int l = actions.Length + 1;
            int n = actionSets.Length;
            double[] mps = new double[n * l];
            double[] eps = new double[n * l];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < actions.Length; ++j)
                {
                    mps[i * l + j] = actionSets[i][actions[j]].MPS * factors[i];
                    eps[i * l + j] = actionSets[i][actions[j]].EPS * factors[i];
                }
                mps[i * l + actions.Length] = 0;
                eps[i * l + actions.Length] = 0;
            }
            int[] selections;
            int interpTarget;

            FindBestBudgetSplit(mps, eps, l, n, mpsLeft, out selections, out interpSet, out interpT, out interpTarget);

            selectedActions = new int[n];
            for (int i = 0; i < n; ++i)
            {
                if (selections[i] >= actions.Length || actionSets[i][actions[selections[i]]].EPS == 0)
                    selectedActions[i] = -1;
                else
                    selectedActions[i] = actions[selections[i]];
            }

            if (interpSet < 0 || interpTarget >= actions.Length || actionSets[interpSet][actions[interpTarget]].EPS == 0)
                interpTargetAction = -1;
            else
                interpTargetAction = actions[interpTarget];
        }

        public static void FindBestActions(ActionDistribution[] dists, double[] factors, int[] actions, out int[] selectedActions, out int interpSet, out double interpT, out int interpTargetAction)
        {
            int n = dists.Length;
            ContinuousAction[][] actionSets = new ContinuousAction[n][];
            double mpsLeft = 0.0;
            double[] newFactors = new double[n];
            for (int i = 0; i < n; ++i)
            {
                actionSets[i] = dists[i].Actions;
                newFactors[i] = factors[i] * (dists[i].MaxFraction - dists[i].TotalFraction);
                mpsLeft += (dists[i].MaxMPS - dists[i].TotalMPS) * factors[i];
            }

            FindBestActions(actionSets, newFactors, actions, mpsLeft, out selectedActions, out interpSet, out interpT, out interpTargetAction);
        }

        public static void AddBestActions(ActionDistribution[] dists, double[] factors, int[] actions)
        {
            int[] selectedActions;
            int interpSet;
            double interpT;
            int interpTargetAction;
            
            FindBestActions(dists, factors, actions, out selectedActions, out interpSet, out interpT, out interpTargetAction);

            for (int i = 0; i < dists.Length; ++i)
            {
                double maxMPS = dists[i].MaxMPS;
                dists[i].MaxMPS = double.PositiveInfinity;
                double fractionLeft = dists[i].MaxFraction - dists[i].TotalFraction;
                if (i == interpSet)
                {
                    if (selectedActions[i] >= 0)
                        dists[i].AddAction(selectedActions[i], (1 - interpT) * fractionLeft);
                    if (interpTargetAction >= 0)
                        dists[i].AddAction(interpTargetAction, interpT * fractionLeft);
                }
                else if (selectedActions[i] >= 0)
                    dists[i].AddAction(selectedActions[i], fractionLeft);
                
                dists[i].MaxMPS = maxMPS;
            }
        }

        public static void AddBestActions(ActionDistribution[] dists, double[] factors, int[] actions, double unevenPart)
        {
            if (unevenPart < 1)
            {
                for (int i = 0; i < dists.Length; ++i)
                {
                    double maxMPS = dists[i].MaxMPS;
                    dists[i].MaxMPS = dists[i].TotalMPS * unevenPart + maxMPS * (1 - unevenPart);
                    dists[i].AddBestActions(actions);
                }
            }

            if(unevenPart > 0)
                AddBestActions(dists, factors, actions);
        }
    }
}
