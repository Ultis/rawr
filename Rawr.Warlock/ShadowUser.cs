using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Warlock
{
    [Serializable]
    public abstract class ShadowUser
    {
        public float EffectiveCritRate { get; set; }
        public float DirectShadowHitsPerSecond { get; set; }
        public float ShadowDps { get; set; }

        public abstract void Calculate(CharacterCalculationsWarlock calculations);

        public static void CalculateRaidIsbUptime(CharacterCalculationsWarlock calculations)
        {
            if (calculations.CalculationOptions.IsbMethod == IsbMethod.Raid)
            {
                float chanceToHit = CalculationsWarlock.ChanceToHit(calculations.CalculationOptions.TargetLevel, calculations.HitPercent);
                float myDirectShadowHitsPerSecond = calculations.SpellRotation.ShadowBoltCastRatio / calculations.SpellRotation.ShadowBoltCastTime * chanceToHit;
                float myEffectiveCritRate = myDirectShadowHitsPerSecond * calculations.CritPercent;

                float raidDirectShadowHitsPerSecond = 0, raidEffectiveCritRate = 0, raidShadowDps = 0;
                for (int i = 0; i < calculations.CalculationOptions.NumRaidWarlocks; i++)
                {
                    SUWarlock currentWarlock = calculations.CalculationOptions.RaidWarlocks[i];
                    currentWarlock.Calculate(calculations);
                    raidDirectShadowHitsPerSecond += currentWarlock.DirectShadowHitsPerSecond;
                    raidEffectiveCritRate += currentWarlock.EffectiveCritRate;
                    raidShadowDps += currentWarlock.ShadowDps;
                }
                for (int i = 0; i < calculations.CalculationOptions.NumRaidShadowPriests; i++)
                {
                    SUShadowPriest currentSp = calculations.CalculationOptions.RaidShadowPriests[i];
                    currentSp.Calculate(calculations);
                    raidDirectShadowHitsPerSecond += currentSp.DirectShadowHitsPerSecond;
                    raidShadowDps += currentSp.ShadowDps;
                }

                myEffectiveCritRate /= 100;
                raidEffectiveCritRate /= 100;
                float raidIsbUptime = 1 - (float)Math.Pow(1 - (raidEffectiveCritRate / raidDirectShadowHitsPerSecond), 4);
                float totalIsbUptime = 1 - (float)Math.Pow(1 - ((raidEffectiveCritRate + myEffectiveCritRate) / (raidDirectShadowHitsPerSecond + myDirectShadowHitsPerSecond)), 4);

                calculations.RaidDpsFromIsb = raidShadowDps * 0.2f * (totalIsbUptime - raidIsbUptime);
                calculations.IsbUptime = totalIsbUptime;
            }
            else
            {
                calculations.RaidDpsFromIsb = 0;
                calculations.IsbUptime = calculations.CalculationOptions.CustomIsbUptime;
            }
        }
    }

    [Serializable]
    public class SUWarlock : ShadowUser
    {
        public float HitPercent { get; set; }
        public float CritPercent { get; set; }
        public float SbCastTime { get; set; }
        public float SbCastRatio { get; set; }

        public SUWarlock()
        {
            HitPercent = 16;
            CritPercent = 20;
            SbCastTime = 2.6f;
            SbCastRatio = 0.95f;
            ShadowDps = 1600;
        }

        public SUWarlock(float hitPercent, float critPercent, float sbCastTime, float sbCastRatio, float shadowDps)
        {
            HitPercent = hitPercent;
            CritPercent = critPercent;
            SbCastTime = sbCastTime;
            SbCastRatio = sbCastRatio;
            ShadowDps = shadowDps;
        }

        public override void Calculate(CharacterCalculationsWarlock calculations)
        {
            if (HitPercent != 0 && CritPercent != 0 && SbCastTime != 0 && SbCastRatio != 0)
            {
                float chanceToHit = CalculationsWarlock.ChanceToHit(calculations.CalculationOptions.TargetLevel, HitPercent);
                DirectShadowHitsPerSecond = SbCastRatio / SbCastTime * chanceToHit;
                EffectiveCritRate = CritPercent * DirectShadowHitsPerSecond;
            }   
        }
    }

    [Serializable]
    public class SUShadowPriest : ShadowUser
    {
        public float HitPercent { get; set; }
        public float MbFrequency { get; set; }

        public SUShadowPriest()
        {
            HitPercent = 16;
            MbFrequency = 7.5f;
            ShadowDps = 1100;
        }

        public SUShadowPriest(float hitPercent, float mbFrequency, float shadowDps)
        {
            HitPercent = hitPercent;
            MbFrequency = mbFrequency;
            ShadowDps = shadowDps;
        }

        public override void Calculate(CharacterCalculationsWarlock calculations)
        {
            float chanceToHit = CalculationsWarlock.ChanceToHit(calculations.CalculationOptions.TargetLevel, HitPercent);
            DirectShadowHitsPerSecond = (1 / MbFrequency + 1f / 12f) * chanceToHit;
            EffectiveCritRate = 0;
        }
    }
}
