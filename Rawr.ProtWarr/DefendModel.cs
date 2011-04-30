using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class DefendModel
    {
        private Player Player;
        private float AttackSpeed;

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
            get { return (1.0f / AttackSpeed); }
        }
        public float AttackerHitsPerSecond
        {
            get { return (AttackerSwingsPerSecond * DefendTable.AnyHit); }
        }

        public void Calculate()
        {
            float armorReduction        = (1.0f - Lookup.ArmorReduction(Player));
            float guaranteedReduction   = (1.0f - Lookup.StanceDamageReduction(Player)) * armorReduction;
            float baseDamagePerSecond   = Player.Options.BossAttackValue / Player.Options.BossAttackSpeed;
            float baseAttack            = Player.Options.BossAttackValue * guaranteedReduction;

            DamagePerHit        = baseAttack   * (1.0f - Player.Stats.PhysicalDamageTakenReductionMultiplier) * (1.0f - Player.Stats.BossPhysicalDamageDealtReductionMultiplier);
            DamagePerCrit       = DamagePerHit * 2.0f;
            DamagePerBlock      = DamagePerHit * (1.0f - (0.3f + Player.Stats.BonusBlockValueMultiplier));
            DamagePerCritBlock  = DamagePerHit * (1.0f - ((0.3f + Player.Stats.BonusBlockValueMultiplier) * 2.0f));

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

            DamagePerSecond         = AverageDamagePerAttack / AttackSpeed;
            Mitigation              = (1.0f - (DamagePerSecond / baseDamagePerSecond));
            EffectiveHealth         = (Player.Stats.Health / guaranteedReduction);
            GuaranteedReduction     = (1.0f - guaranteedReduction);

            double a = Convert.ToDouble(DefendTable.AnyMiss);
            double h = Convert.ToDouble(Player.Stats.Health);
            double H = Convert.ToDouble(AverageDamagePerHit);
            double s = Convert.ToDouble(AttackSpeed);
            BurstTime = Convert.ToSingle((1.0d / a) * ((1.0d / Math.Pow(1.0d - a, h / H)) - 1.0d) * s);
        }

        public DefendModel(Player player)
        {
            Player      = player;
            AttackSpeed = Lookup.TargetWeaponSpeed(Player);
            DefendTable = new DefendTable(Player);
            Calculate();
        }
    }
}
