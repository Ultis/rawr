using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    public enum Trigger : int
    {
        Use,
        SpellHit,
        SpellCrit,
        SpellCast,
        SpellMiss,
        DamageSpellHit,
        DamageSpellCrit,
        DamageSpellCast,
        HealingSpellHit,
        HealingSpellCrit,
        HealingSpellCast,
        MeleeHit,
        MeleeCrit,
        PhysicalHit,
        PhysicalCrit,
        ManaGem,
        DoTTick,
        DamageDone,
        MageNukeCast,
    }

    [Serializable]
    public class SpecialEffect
    {
        public Trigger Trigger { get; set; }
        public Stats Stats { get; set; }
        public float Duration { get; set; }
        public float Cooldown { get; set; }
        public float Chance { get; set; }
        public int MaxStack { get; set; }

        public SpecialEffect()
        {
            Chance = 1.0f;
            MaxStack = 1;
        }

        public SpecialEffect(Trigger trigger, Stats stats, float duration, float cooldown)
        {
            Chance = 1.0f;
            MaxStack = 1;
            Trigger = trigger;
            Duration = duration;
            Cooldown = cooldown;
            Stats = stats;
        }

        public SpecialEffect(Trigger trigger, Stats stats, float duration, float cooldown, float chance)
        {
            Chance = chance;
            MaxStack = 1;
            Trigger = trigger;
            Duration = duration;
            Cooldown = cooldown;
            Stats = stats;
        }

        public SpecialEffect(Trigger trigger, Stats stats, float duration, float cooldown, float chance, int maxStack)
        {
            Chance = chance;
            MaxStack = maxStack;
            Trigger = trigger;
            Duration = duration;
            Cooldown = cooldown;
            Stats = stats;
        }

        /// <summary>
        /// Computes average stats given the frequency of triggers.
        /// </summary>
        public Stats GetAverageStats()
        {
            return GetAverageStats(0.0f);
        }

        /// <summary>
        /// Computes average stats given the frequency of triggers.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds.</param>
        public Stats GetAverageStats(float triggerInterval)
        {
            return GetAverageStats(triggerInterval, 1.0f);
        }

        /// <summary>
        /// Computes average stats given the frequency of triggers.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced (for example for
        /// SpellCrit trigger you would set triggerInterval to average time between hits and set
        /// triggerChance to crit chance)</param>
        public Stats GetAverageStats(float triggerInterval, float triggerChance)
        {
            // Since no fightDuration given, assume stack building time  (if applicable) doesn't matter and only calculate Uptime
            return Stats * (MaxStack * GetAverageUptime(triggerInterval, triggerChance));
        }

        /// <summary>
        /// Computes average stats given the frequency of triggers.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced (for example for
        /// SpellCrit trigger you would set triggerInterval to average time between hits and set
        /// triggerChance to crit chance)</param>
        /// <param name="attackSpeed">Average unhasted attack speed, used in PPM calculations.</param>
        public Stats GetAverageStats(float triggerInterval, float triggerChance, float attackSpeed)
        {
            // Since no fightDuration given, assume stack building time (if applicable) doesn't matter and only calculate Uptime
            return Stats * (MaxStack * GetAverageUptime(triggerInterval, triggerChance, attackSpeed));
        }

        /// <summary>
        /// Computes average stats given the frequency of triggers.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced (for example for
        /// SpellCrit trigger you would set triggerInterval to average time between hits and set
        /// triggerChance to crit chance)</param>
        /// <param name="attackSpeed">Average unhasted attack speed, used in PPM calculations.</param>
        /// <param name="fightDuration">Duration of fight in seconds.</param>
        public Stats GetAverageStats(float triggerInterval, float triggerChance, float attackSpeed, float fightDuration)
        {
            if (MaxStack > 1)
            {
                return Stats * GetAverageStackSize(triggerInterval, triggerChance, attackSpeed, fightDuration);
            }
            else
              return Stats * GetAverageUptime(triggerInterval, triggerChance, attackSpeed, fightDuration);
        }

        /// <summary>
        /// Computes average stack size given the frequency of triggers.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced (for example for
        /// SpellCrit trigger you would set triggerInterval to average time between hits and set
        /// triggerChance to crit chance)</param>
        /// <param name="attackSpeed">Average unhasted attack speed, used in PPM calculations.</param>
        /// <param name="fightDuration">Duration of fight in seconds.</param>
        public float GetAverageStackSize(float triggerInterval, float triggerChance, float attackSpeed, float fightDuration)
        {
            float averageStack = 0;
            if ((MaxStack > 1) && (Cooldown == 0f))
            {
                // Simplified handling for stacking procs
                float probToStack = 1.0f - (float)Math.Pow(1f - triggerChance * GetChance(attackSpeed), Duration / triggerInterval);
                

                if (probToStack > 0)
                {
                    // For now, assume it stacks to max, if it can stack at all
                    float buildTime = triggerInterval / triggerChance * MaxStack;
                    float value;
                    if (fightDuration > buildTime)
                        value = buildTime * (MaxStack - 1) / 2 + (fightDuration - buildTime) * MaxStack;
                    else
                        value = buildTime * (MaxStack - 1) / 2;

                    averageStack = value / fightDuration;
                }
                else
                {
                    // Handle it like single stack only for now
                    // Since no cooldown, assume it isn't stacking because Duration <  ( triggerInterval / triggerChance )
                    averageStack = Duration / (triggerInterval / triggerChance);       // Average Uptime
                }
                return averageStack;
            }
            else
                return GetAverageUptime(triggerInterval, triggerChance, attackSpeed, fightDuration);
        }

        /// <summary>
        /// Computes average uptime of the effect given the frequency of triggers.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced (for example for
        /// SpellCrit trigger you would set triggerInterval to average time between spell ticks and set
        /// triggerChance to crit chance)</param>
        public float GetAverageUptime(float triggerInterval, float triggerChance)
        {
            return GetAverageUptime(triggerInterval, triggerChance, 3f, 0f);
        }

        /// <summary>
        /// Computes average uptime of the effect given the frequency of triggers.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced (for example for
        /// SpellCrit trigger you would set triggerInterval to average time between spell ticks and set
        /// triggerChance to crit chance)</param>
        /// <param name="attackSpeed">Average unhasted attack speed, used in PPM calculations.</param>
        public float GetAverageUptime(float triggerInterval, float triggerChance, float attackSpeed)
        {
            return GetAverageUptime(triggerInterval, triggerChance, attackSpeed, 0f);
        }

        /// <summary>
        /// Computes average uptime of (atleast 1 stack of) the effect given the frequency of triggers.
        /// </summary>
        /// <param name="triggerInterval">Average time interval between triggers in seconds.</param>
        /// <param name="triggerChance">Chance that trigger of correct type is produced (for example for
        /// SpellCrit trigger you would set triggerInterval to average time between spell ticks and set
        /// triggerChance to crit chance)</param>
        /// <param name="attackSpeed">Average unhasted attack speed, used in PPM calculations.</param>
        /// <param name="fightDuration">Duration of fight in seconds.</param>
        public float GetAverageUptime(float triggerInterval, float triggerChance, float attackSpeed, float fightDuration)
        {
            if (triggerChance == 0f)
            {
                return 0f;
            }

            if (Cooldown > Duration)
            {
                // fight duration is used only in this case, for the cooldown < duration case the other approximations are good enough
                if (fightDuration == 0f)
                {
                    return Duration / (Cooldown + triggerInterval / triggerChance / GetChance(attackSpeed));
                }
                else
                {
                    // activeTime(T) := average time the effect is up given we are out of cooldown and there is T time left
                    // activeTime(T) = 0 if T <= 0
                    // activeTime(T) = chance * (min(duration, T) + activeTime(T - cooldown)) + (1 - chance) * activeTime(T - interval)
                    // activeTime[T] = chance * (min(D, T) + activeTime[T - C]) + (1 - chance) * activeTime[T - 1]

                    // activeTime[0] = 0
                    // activeTime[1] = p
                    // activeTime[2] = p * 2 + (1 - p) * activeTime[1]
                    // activeTime[3] = p * 3 + (1 - p) * activeTime[2]
                    // ...
                    // activeTime[D] = p * D + (1 - p) * activeTime[D - 1]

                    // H[0] = 0
                    // H[n] = n * p + (1 - p) * H[n - 1]
                    // H[n] = 1 - 1/p + n + (1 - p)^n * (1/p - 1)
                    //      = n + (1 - p)/p * ((1 - p)^n - 1)
                    // activeTime[n] = n - (1 - p)/p * (1 - (1 - p)^n)
                    // activeTime[D] = D - (1 - p)/p * (1 - (1 - p)^D)

                    // activeTime[D + 1] = p * D + (1 - p) * activeTime[D]
                    // activeTime[D + 2] = p * D + (1 - p) * (p * D + (1 - p) * activeTime[D])
                    //                   = p * D + p * (1 - p) * D + (1 - p) ^ 2 * activeTime[D]
                    //                   = p * (2 - p) * D + (1 - p) ^ 2 * activeTime[D]
                    // activeTime[D + 3] = p * D + (1 - p) * [p * (2 - p) * D + (1 - p) ^ 2 * activeTime[D]]
                    //                   = p * D * (1 + (1 - p) * (2 - p)) + (1 - p) ^ 3 * activeTime[D]
                    // activeTime[D + 4] = p * D * (1 + (1 - p) * (1 + (1 - p) * (2 - p))) + (1 - p) ^ 4 * activeTime[D]

                    // K[1] = 1
                    // K[n] = 1 + (1 - p) * K[n - 1]
                    // K[n] = 1/p + (1-p)^(n-1) * (p - 1) / p
                    //      = (1 - (1-p) ^ n) / p

                    // activeTime[D + n] = p * D * (1 - (1-p) ^ n) / p + (1 - p) ^ n * activeTime[D]
                    //                   = D + (1 - p) ^ n * (activeTime[D] - D)
                    //                   = D - (1 - p) ^ n * (1 - p)/p * (1 - (1 - p)^D)

                    // activeTime[C] = activeTime[D + C - D] = D + (1 - p) ^ (C - D) * (activeTime[D] - D)
                    //               = D - (1 - p) ^ (C - D) * (1 - p)/p * (1 - (1 - p)^D)

                    // activeTime[C + 1] = p * (D + activeTime[1]) + (1 - p) * activeTime[C]
                    //                   = p * D + p * activeTime[1] + (1 - p) * activeTime[C]
                    // activeTime[C + 2] = p * D + p * activeTime[2] + (1 - p) * (p * D + p * activeTime[1] + (1 - p) * activeTime[C])
                    //                   = p * D * K[2] + p * activeTime[2] + (1 - p) * p * activeTime[1] + (1 - p) ^ 2 * activeTime[C]

                    // activeTime[2] - p * 2 = (1 - p) * activeTime[1]
                    // activeTime[C + 2] = p * D * K[2] + p * activeTime[2] + p * (activeTime[2] - p * 2) + (1 - p) ^ 2 * activeTime[C]
                    //                   = p * D * K[2] + 2 * p * activeTime[2] - 2 * p ^ 2 + (1 - p) ^ 2 * activeTime[C]
                    // activeTime[C + 3] = p * D * K[3] + p * activeTime[3] + (1 - p) * (2 * p * activeTime[2] - 2 * p ^ 2 + (1 - p) ^ 2 * activeTime[C])
                    //                   = p * D * K[3] + p * activeTime[3] + 2 * p * (activeTime[3] - 3 * p) - 2 * p ^ 2 * (1 - p) + (1 - p) ^ 3 * activeTime[C]
                    //                   = p * D * K[3] + 3 * p * activeTime[3] - 3 * 2 * p ^ 2 - 2 * p ^ 2 * (1 - p) + (1 - p) ^ 3 * activeTime[C]
                    //                   = p * D * K[3] + 3 * p * activeTime[3] - 2 * p ^ 2 * (4 - p) + (1 - p) ^ 3 * activeTime[C]

                    // activeTime[C + 1] = p * D + p * p + (1 - p) * activeTime[C]
                    // activeTime[C + 2] = p * D * K[2] + 2 * (p * (2 - p) - (1 - p) * (1 - (1 - p)^2)) + (1 - p) ^ 2 * activeTime[C]

                    // activeTime[C + n] = p * D * K[n] + p * H[n] + (1 - p) * (p * H[n - 1] + (1 - p) * (p * H[n - 2] + ...))
                    // p * H[n] = p * (n + (1 - p)/p * ((1 - p)^n - 1)) = p * n - (1 - p) * (1 - (1 - p)^n)
                    // sum_i=0..n p * (1 - p)^(i - 1) * H[i]
                    // p * (1 - p) ^ (i - 1) * H[i] = p * i * (1 - p) ^ (i - 1) - (1 - p) ^ i * (1 - (1 - p)^i)
                    // sum_i=0..n p * (1 - p)^(i - 1) * H[i] = p * n * (1 - p) ^ (n - 1) - (1 - p) ^ n * (1 - (1 - p)^n)    +    p * (n - 1) * (1 - p) ^ (n - 2) - (1 - p) ^ (n - 1) * (1 - (1 - p)^(n - 1)) + ...
                    // - (1 - p) ^ n * (1 - (1 - p)^n) + (1 - p) ^ (n - 1) * (p * n - 1 + (1 - p)^(n - 1)))

                    // activeTime[C + 4] = p * D * K[4] + p * activeTime[4] + (1 - p) * (3 * p * activeTime[3] - 2 * p ^ 2 * (4 - p)) + (1 - p) ^ 4 * activeTime[C]
                    // p * activeTime[4] + (1 - p) * (3 * p * activeTime[3] - 2 * p ^ 2 * (4 - p)) = 
                    // p * activeTime[4] + 3 * p * (1 - p) * activeTime[3] - 2 * p ^ 2 * (4 - p) * (1 - p) =
                    // p * activeTime[4] + 3 * p * (activeTime[4] - 4 * p) - 2 * p ^ 2 * (4 - p) * (1 - p) =
                    // 4 * p * activeTime[4] - p ^ 2 * (3 * 4 + (1 - p) * (3 * 2 + (1 - p) * (2 * 1 + (1 - p) * 1 * 0)))

                    // R[0] = 0
                    // R[n] = n * (n - 1) + (1 - p) * R[n - 1]
                    // R[n] = 2*p^(-2) * ((1 - p)^n - 1 - n)  +  2*p^(-3) * (1 - (1-p)^n)  +  n*(n + 1)/p
                    //      = [p^2*n*(n + 1) + 2*p*((1 - p)^n - 1 - n) + 2* (1 - (1-p)^n)] / p^3
                    //      = [p^2*n*(n + 1) - 2*p*n - 2*p*(1 - (1 - p)^n) + 2* (1 - (1-p)^n)] / p^3
                    //      = [p*n*(p*n + p - 2) + 2*(1 - (1-p)^n) * (1 - p)] / p^3

                    // S[n] := n * p * H[n] - p ^ 2 * R[n] =
                    // n * (p * n - (1 - p) * (1 - (1 - p)^n)) - [n*(p*n + p - 2) + 2/p*(1 - (1-p)^n) * (1 - p)]
                    // p * n^2 - n*(p*n + p - 2) - (2/p * (1 - p) + n * (1 - p)) * (1 - (1 - p)^n)
                    // n * (2 - p) - (2/p + n) * (1 - p) * (1 - (1 - p)^n)

                    // activeTime[C + n] = p * D * K[n] + n * (2 - p) - (2/p + n) * (1 - p) * (1 - (1 - p)^n) + (1 - p) ^ n * activeTime[C]
                    //                   = (D - (2/p + n) * (1 - p)) * (1 - (1-p) ^ n) + n * (2 - p) + (1 - p) ^ n * activeTime[C]
                    //                   = 
                    // activeTime[C + n] = p * D * K[n] + S[n] + (1 - p) ^ n * activeTime[C]

                    // activeTime[k*C+n] = p * (D + activeTime[(k-1)*C+n]) + (1 - p) * activeTime[k*C+n-1]

                    // p * T * D / (p * C + 1)

                    // activeTime[2*C+n] = p * (D + activeTime[C+n]) + (1 - p) * activeTime[2*C+n-1]
                    //                   = p * D + p * (p * D * K[n] + S[n] + (1 - p) ^ n * activeTime[C])
                    if (triggerInterval > 0 && fightDuration <= Duration)
                    {
                        double n = fightDuration / triggerInterval;
                        double p = triggerChance * GetChance(attackSpeed);
                        return (float)((n - (1 - p) / p * (1 - Math.Pow(1 - p, n))) / n);
                    }
                    else if (triggerInterval > 0 && fightDuration <= Cooldown)
                    {
                        double D = Duration / triggerInterval;
                        double n = fightDuration / triggerInterval - D;
                        double p = triggerChance * GetChance(attackSpeed);
                        return (float)((D - Math.Pow(1 - p, n) * (1 - p) / p * (1 - Math.Pow(1 - p, D))) / (fightDuration / triggerInterval));
                    }
                    else
                    {
                        float p = triggerChance * GetChance(attackSpeed);
                        // just an approximation for now, accurate symbolic solution on todo list
                        float t = fightDuration - triggerInterval / p;
                        float cc = Cooldown + triggerInterval / p;
                        float total = (float)Math.Floor(t / cc) * Duration;
                        t -= (float)Math.Floor(t / cc) * cc;
                        total += Math.Min(t, Duration);
                        return total / fightDuration;
                    }
                }
            }
            else if (Cooldown == 0.0f)
            {
                return 1.0f - (float)Math.Pow(1f - triggerChance * GetChance(attackSpeed), Duration / triggerInterval);
            }
            else
            {
                //P(E0) = P(E0|S0)*P(S0) + P(E0|!S0) * (1 - P(S0))
                //=1 - (1 - P(S0)) + P(E0|!S0) * (1 - P(S0))
                //=1 + P(!S0) * (1 - P(E0|!S0))
                //=1 + (1 - P0) * (1 - P(E0|!S0))

                //P(E0|!S0) = P(S1|!S0) + P(E0|!S0,!S1)*P(!S1|!S0)

                //P(S1|!S0) = P(!S0|S1)*P(S1)/P(!S0) = P0 / (1 - P0)
                //P(!S1|!S0) = P(!S0|!S1)*P(!S1)/P(!S0) = P(!S0|!S1)

                //P(E0|!S0) = P0 / (1 - P0) + P(E0|!S0,!S1)*P(!S0|!S1)

                //P(E0|!S0,!S1) = P(S2|!S0,!S1) + P(E0|!S0,!S1,!S2)*P(!S2|!S0,!S1)

                //P(S2|!S0,!S1) = P(!S0,S1)/P(!S0,!S1) = (P(!S0) - P(!S0,!S1))/P(!S0,!S1)

                //P(!S0,!S1) = P(!S0|!S1) * P(!S1) = P(!S0|!S1) * (1 - P0)



                //P(S0|!S1,!S2,...,!SC) = PT
                //P(S0|Sn) = 0, 0 < n <= C

                //P(S0) = P(S0|S1)*P(S1) + P(S0|!S1)*P(!S1)
                //=P(S0|!S1)*(1 - P(S0))

                //P(S0) * (1 + P(S0|!S1)) = P(S0|!S1)
                //P(S0) = P(S0|!S1) / (1 + P(S0|!S1))

                //P(S0|!S1) = P(S0|!S1,S2)*P(S2|!S1) + P(S0|!S1,!S2)*P(!S2|!S1)
                //=P(S0|!S1,S2)*P(S1|!S0) + P(S0|!S1,!S2)*P(!S1|!S0)
                //=P(S0|!S1,!S2)*(1 - P(S0|!S1))

                //P(S0|!S1) * (1 + P(S0|!S1,!S2)) = P(S0|!S1,!S2)
                //P(S0|!S1) = P(S0|!S1,!S2) / (1 + P(S0|!S1,!S2))


                //P(S0|!S1,!S2) = P(S0|!S1,!S2,S3)*P(S3|!S1,!S2) + P(S0|!S1,!S2,!S3)*P(!S3|!S1,!S2)
                //=P(S0|!S1,!S2,!S3)*P(!S3|!S1,!S2) = P(S0|!S1,!S2,!S3) * (1 - P(S0|!S1,!S2))

                //P(!S3|!S1,!S2) = P(!S2|!S0,!S1) = P(!S0|!S2,!S1)*P(!S1,!S0) / P(!S0,!S1) = P(!S0|!S2,!S1) = 1 - P(S0|!S1,!S2)

                //P(S0|!S1,!S2) = P(S0|!S1,!S2,!S3) / (1 + P(S0|!S1,!S2,!S3))

                //PC = PT / (1 + PT)
                //PC_1 = PC / (1 + PC) = PT / (1 + PT) / ((1 + 2 * PT) / (1 + PT)) = PT / (1 + 2 * PT)
                //PC_2 = PT / (1 + 2 * PT) / ((1 + 3 * PT) / (1 + 2 * PT)) = PT / (1 + 3 * PT)


                //P0 = PT / (1 + C * PT)



                //P0 / (1 - P0) = PT / (1 + C * PT) / ((1 + (C - 1) * PT) / (1 + C * PT)) = PT / (1 + (C - 1) * PT) = P1

                //Pk = PT / (1 + (C - k) * PT)

                //P(E0) = 1 + (1 - P0) * (1 - P(E0|!S0))

                //P(E0|!S0) = P0 / (1 - P0) + P(E0|!S0,!S1)*P(!S0|!S1)
                //=P1 + P(E0|!S0,!S1)*(1 - P1)


                //P(E0|!S0,!S1) = P(S2|!S0,!S1) + P(E0|!S0,!S1,!S2)*P(!S2|!S0,!S1)


                //P(S2|!S0,!S1) = P(!S0|S2,!S1) * P(S2,!S1) / P(!S0,!S1) = P(S1,!S0) / P(!S0,!S1) = P(!S0|S1) * P(S1) / [P(!S0|!S1) * P(!S1)]
                //=P0 / [(1 - P(S0|!S1)) * (1 - P0) = P0 / [(1 - P1) * (1 - P0)]
                //=P1 / (1 - P1) = P2


                //P(E0|!S0,!S1) = P2 + P(E0|!S0,!S1,!S2)*(1 - P2)


                //P(E0|!S0,!S1,!S2,...,!SD-1) = 0


                //PED = 0

                //PED_1 = PD_1

                //PED_2 = PD_2 + PD_1 * (1 - PD_2) = PD_2 + PD_1 - PD_1 * PD_2 =
                //[PT * (1 + (C - D + 1) * PT) + PT * (1 + (C - D + 2) * PT) - PT * PT] / [(1 + (C - D + 2) * PT) * (1 + (C - D + 1) * PT)]
                //PT * 2 * [(1 + (C - D + 1) * PT)] / [(1 + (C - D + 2) * PT) * (1 + (C - D + 1) * PT)]
                //2 * PT / (1 + (C - D + 2) * PT) = 2 * PD_2
                //PED_3 = 3 * PD_3
                //PE0 = D * P0, if D < C

                //P(E0|!S0,!S1,...,!SC-1) = PC + P(E0|!S0,!S1,!S2,...,!SC)*(1 - PC)


                //P(E0|!S0,!S1,!S2,...,!SC) = P(E0|!S0-C,SC+1)*P(SC+1|!S0-C) + P(E0|!S0-C,!SC+1)*P(!SC+1|!S0-C)
                //=P(SC+1|!S0-C) + P(E0|!S0-C,!SC+1)*P(!SC+1|!S0-C)


                //P(SC+1|!S0-C) = P(!S0-C,SC+1) / P(!S0-C) = P(!S0,!S1-C,SC+1) / P(!S0-C) = P(!S0|!S1-C,SC+1) * P(!S0-C-1,SC) / P(!S0-C) = 
                //(1 - PT) * P(SC|!S0-C-1) * P(!S0-C-1) / [P(!SC|!S0-C-1) * P(!S0-C-1)] = 
                //(1 - PT) * P(SC|!S0-C-1) / P(!SC|!S0-C-1)
                //= (1 - PT) * PT / (1 - PT) = PT


                //P(E0|!S0,!S1,!S2,...,!SC) = PT + P(E0|!S0-C,!SC+1) * (1 - PT)


                //PED = 0
                //PED_1 = PT

                //PED_2 = PT + PED_1 * (1 - PT) = PT + PT * (1 - PT) = 2 * PT - PT * PT = 1 - (1 - 2 * PT + PT * PT) = 1 - (1 - PT) ^ 2

                //PED_3 = PT + PED_2 * (1 - PT) = PT + [1 - (1 - PT) ^ 2] * (1 - PT) =
                //PT + [(1 - PT) - (1 - PT) ^ 3] = 1 - (1 - PT) ^ 3


                //PEC = 1 - (1 - PT) ^ (D - C)


                //PEC_1 = PC-1 + PEC*(1 - PC-1)

                //PEC_2 = PC-2 + PEC_1*(1 - PC-2) = PC-2 + [PC-1 + PEC*(1 - PC-1)]*(1 - PC-2) = PC_2 + PC_1 - PC_1 * PC_2 + PEC*(1 - PC_1)*(1 - PC_2)
                //= 1 - (1 - PC_1)*(1 - PC_2) + PEC * (1 - PC_1)*(1 - PC_2) = 1 - (1 - PC_1)*(1 - PC_2) * (1 - PEC)


                //(1 - PC_1)*(1 - PC_2) = 1 - [PC_2 + PC_1 - PC_1 * PC_2] = 1 - 2 * PC_2

                //PEC_2 = 1 - (1 - 2 * PC_2) * (1 - PEC)

                //PE0 = 1 - (1 - C * P0) * (1 - PEC)
                //= 1 - (1 - C * PT / (1 + C * PT)) * (1 - PEC)
                //= 1 - 1 / (1 + C * PT) * (1 - PEC)

                //= 1 - 1 / (1 + C * PT) * (1 - PT) ^ (D - C)
                return 1.0f - 1.0f / (1.0f + Cooldown / triggerInterval * triggerChance * GetChance(attackSpeed)) * (float)Math.Pow(1f - triggerChance * GetChance(attackSpeed), (Duration - Cooldown) / triggerInterval);
            }

        }

        public bool UsesPPM()
        {
            return Chance < 0;
        }

        public float GetChance(float attackspeed)
        {
            if (Chance < 0)
            {
                return -Chance / 60f * attackspeed;
            }
            else return Chance;
        }

        private string CooldownString
        {
            get
            {
                int cooldown = (int)Cooldown;
                if (cooldown % 60 == 0)
                {
                    return (cooldown / 60).ToString() + " Min";
                }
                else
                {
                    return cooldown.ToString() + " Sec";
                }
            }
        }

        private string DurationString
        {
            get
            {
                int duration = (int)Duration;
                return duration.ToString() + " Sec";
            }
        }

        private string ChanceString
        {
            get
            {
                if (Chance < 0) return (-Chance).ToString("N1") + " PPM";
                else return (Chance * 100).ToString("N0") + "%";
            }
        }

        private string TriggerString
        {
            get
            {
                switch (Trigger)
                {
                    case Trigger.DamageSpellCast:
                        return " on Damaging Spell Cast";
                    case Trigger.DamageSpellCrit:
                        return " on Damaging Spell Crit";
                    case Trigger.DamageSpellHit:
                        return " on Damaging Spell Hit";
                    case Trigger.HealingSpellCast:
                        return " on Healing Spell Cast";
                    case Trigger.HealingSpellCrit:
                        return " on Healing Spell Crit";
                    case Trigger.HealingSpellHit:
                        return " on Healing Spell Hit";
                    case Trigger.MeleeCrit:
                        return " on Melee Crit";
                    case Trigger.MeleeHit:
                        return " on Melee Hit";
                    case Trigger.SpellCast:
                        return " on Spell Cast";
                    case Trigger.SpellCrit:
                        return " on Spell Crit";
                    case Trigger.SpellHit:
                        return " on Spell Hit";
                    case Trigger.SpellMiss:
                        return " on Spell Miss";
                    case Trigger.Use:
                        return "";
                    case Trigger.PhysicalHit:
                        return " on Physical Hit";
                    case Trigger.PhysicalCrit:
                        return " on Physical Crit";
                    case Trigger.ManaGem:
                        return " on Mana Gem";
                    case Trigger.MageNukeCast:
                        return " on Mage Nuke Cast";
                    default:
                        return " " + Trigger.ToString();
                }
            }
        }

        public override string ToString()
        {
            if (MaxStack > 1)
            {
                if (Cooldown == 0.0f)
                {
                    if (Chance == 1.0f)
                    {
                        return string.Format("{4}x {0} ({1}{3})", Stats.ToString(), DurationString, ChanceString, TriggerString, MaxStack);
                    }
                    else
                    {
                        return string.Format("{4}x {0} ({1} {2}{3})", Stats.ToString(), DurationString, ChanceString, TriggerString, MaxStack);
                    }
                }
                else
                {
                    if (Chance == 1.0f)
                    {
                        return string.Format("{5}x {0} ({1}{3}/{4})", Stats.ToString(), DurationString, ChanceString, TriggerString, CooldownString, MaxStack);
                    }
                    else
                    {
                        return string.Format("{5}x {0} ({1} {2}{3}/{4})", Stats.ToString(), DurationString, ChanceString, TriggerString, CooldownString, MaxStack);
                    }
                }
            }
            else
            {
                if (Cooldown == 0.0f)
                {
                    if (Chance == 1.0f)
                    {
                        return string.Format("{0} ({1}{3})", Stats.ToString(), DurationString, ChanceString, TriggerString);
                    }
                    else
                    {
                        return string.Format("{0} ({1} {2}{3})", Stats.ToString(), DurationString, ChanceString, TriggerString);
                    }
                }
                else
                {
                    if (Chance == 1.0f)
                    {
                        return string.Format("{0} ({1}{3}/{4})", Stats.ToString(), DurationString, ChanceString, TriggerString, CooldownString);
                    }
                    else
                    {
                        return string.Format("{0} ({1} {2}{3}/{4})", Stats.ToString(), DurationString, ChanceString, TriggerString, CooldownString);
                    }
                }
            }
        }
    }
}
