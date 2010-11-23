using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    class DefendModel
    {
        private Character Character;
        private CalculationOptionsProtWarr Options;
        private Stats Stats;

        public readonly ParryModel ParryModel;
        public readonly DefendTable DefendTable;

        public float AverageDamagePerAttack { get; set; }
        public float AverageDamagePerHit { get; set; }
        public float DamagePerHit { get; set; }
        public float DamagePerBlock { get; set; }
        public float DamagePerCritBlock { get; set; }
        public float DamagePerCrit { get; set; }
        public float DamagePerSecond { get; set; }
        public float GuaranteedReduction { get; set; }
        public float Mitigation { get; set; }
        public float EffectiveHealth { get; set; }
        public float BurstTime { get; set; }

        public float AttackerSwingsPerSecond
        {
            get { return (1.0f / ParryModel.BossAttackSpeed); }
        }
        public float AttackerHitsPerSecond
        {
            get { return (AttackerSwingsPerSecond * DefendTable.AnyHit); }
        }

        public void Calculate()
        {
            float attackSpeed           = ParryModel.BossAttackSpeed;
            float armorReduction        = (1.0f - Lookup.ArmorReduction(Character, Stats, Options.TargetLevel));
            float baseDamagePerSecond   = Options.BossAttackValue / Options.BossAttackSpeed;
            float guaranteedReduction   = (Lookup.StanceDamageReduction(Character, Stats) * armorReduction);
            float baseAttack            = Options.BossAttackValue * guaranteedReduction;

            DamagePerHit        = baseAttack * (1.0f + Stats.PhysicalDamageTakenMultiplier) * (1f + Stats.BossPhysicalDamageDealtMultiplier);
            DamagePerCrit       = DamagePerHit * 2.0f;
            DamagePerBlock      = DamagePerHit * (1.0f - 0.3f);
            DamagePerCritBlock  = DamagePerHit * (1.0f - 0.6f);

            AverageDamagePerHit =
                DamagePerHit * (DefendTable.Hit / DefendTable.AnyHit) +
                DamagePerCrit * (DefendTable.Critical / DefendTable.AnyHit) +
                DamagePerBlock * (DefendTable.Block / DefendTable.AnyHit) +
                DamagePerCritBlock * (DefendTable.CriticalBlock / DefendTable.AnyHit);
            AverageDamagePerAttack =
                DamagePerHit * DefendTable.Hit +
                DamagePerCrit * DefendTable.Critical +
                DamagePerBlock * DefendTable.Block +
                DamagePerCritBlock * DefendTable.CriticalBlock;

            DamagePerSecond         = AverageDamagePerAttack / attackSpeed;
            Mitigation              = (1.0f - (DamagePerSecond / baseDamagePerSecond));
            EffectiveHealth         = (Stats.Health / guaranteedReduction);
            GuaranteedReduction     = (1.0f - guaranteedReduction);

            double a = Convert.ToDouble(DefendTable.AnyMiss);
            double h = Convert.ToDouble(Stats.Health);
            double H = Convert.ToDouble(AverageDamagePerHit);
            double s = Convert.ToDouble(ParryModel.BossAttackSpeed / Options.BossAttackSpeed);
            BurstTime = Convert.ToSingle((1.0d / a) * ((1.0d / Math.Pow(1.0d - a, h / H)) - 1.0d) * s);
        }

        public DefendModel(Character character, Stats stats, CalculationOptionsProtWarr options)
        {
            Character   = character;
            Stats       = stats;
            Options     = options;
            ParryModel  = new ParryModel(character, stats, options);
            DefendTable = new DefendTable(character, stats, options); 
            Calculate();
        }
    }
}
