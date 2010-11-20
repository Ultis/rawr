using System;

namespace Rawr.ShadowPriest.Spells
{
    public class ShadowFiend : DoTSpell
    {
        public ShadowFiend()
            : base()
        {
        }
        protected override void SetDotValues()
        {
            base.SetDotValues();

            tickHasteCoEf = 0f;
            debuffDurationBase = 10f;
            tickPeriodBase = 1f;

        }
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseScaling = 0.212126922078004f;
            manaCost = 0f;
            shortName = "SF";
            name = "Shadow Fiend";
            cooldown = 180f;
        }
        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }

        public override float AverageDamage
        {
            get
            {
                float ShadowCrawl = 0.15f * 13f / 15f;
                float nonCritDamage = ((1f - CritChance) * TickDamage) * (1 +ShadowCrawl);
                float critDamage = ((CritChance * TickCritDamage) * (1 +ShadowCrawl));
                // ShadowCrawl has a 5 second duration every 6 seconds, meaning you can keep it up 13 seconds of the 15 seconds.
                return (nonCritDamage + critDamage) * TickNumber; 
            } 
        }
        
        #region hide
        public ShadowFiend(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static ShadowFiend operator +(ShadowFiend A, ShadowFiend B)
        {
            ShadowFiend C = (ShadowFiend)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ShadowFiend operator *(ShadowFiend A, float b)
        {
            ShadowFiend C = (ShadowFiend)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion

    }
}
