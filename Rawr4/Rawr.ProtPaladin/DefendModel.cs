using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    class DefendModel
    {
        private Character Character;
        private CalculationOptionsProtPaladin CalcOpts;
        private BossOptions BossOpts;
        private Base.StatsPaladin Stats;

        public readonly ParryModel ParryModel;
        public readonly DefendTable DefendTable;

        public float AverageDamagePerAttack { get; set; }
        public float AverageDamagePerHit { get; set; }
        public float DamagePerHit { get; set; }
        public float DamagePerBlock { get; set; }
        public float DamagePerCrit { get; set; }
        public float DamagePerSecond { get; set; }
        public float GuaranteedReduction { get; set; }
        public float Mitigation { get; set; }
        public float DamageTaken { get; set; }
        public float[] ResistanceTable { get; set; }
        public float TankPoints { get; set; }
        public float EffectiveHealth { get; set; }
        public float BurstTime { get; set; }

        public void Calculate()
        {
            float attackSpeed           = BossOpts.DefaultMeleeAttack.AttackSpeed / (1f - Stats.BossAttackSpeedReductionMultiplier);
            float armorReduction        = (1.0f - Lookup.ArmorReduction(Stats.Armor, BossOpts.Level));
            float baseDamagePerSecond   = BossOpts.DefaultMeleeAttack.DamagePerHit / BossOpts.DefaultMeleeAttack.AttackSpeed;
            float guaranteedReduction   = (Lookup.DamageReduction(Stats) * armorReduction);
            float absorbed = Stats.DamageAbsorbed;

            DamagePerHit = (BossOpts.DefaultMeleeAttack.DamagePerHit * guaranteedReduction) - absorbed;
            DamagePerCrit   = (2.0f * DamagePerHit);
            DamagePerBlock  = Math.Max(0.0f, DamagePerHit * (1f - Lookup.ActiveBlockReduction(Stats.BonusBlockValueMultiplier, Character.PaladinTalents.HolyShield)));

            AverageDamagePerHit =
                DamagePerHit * (DefendTable.Hit / DefendTable.AnyHit) +
                DamagePerCrit * (DefendTable.Critical / DefendTable.AnyHit) +
                DamagePerBlock * (DefendTable.Block / DefendTable.AnyHit);
            AverageDamagePerAttack = 
                DamagePerHit * DefendTable.Hit + 
                DamagePerCrit * DefendTable.Critical +
                DamagePerBlock * DefendTable.Block;

            DamagePerSecond     = AverageDamagePerAttack / attackSpeed;
            DamageTaken         = DamagePerSecond / baseDamagePerSecond;
            Mitigation          = (1.0f - (DamagePerSecond / baseDamagePerSecond));
            TankPoints          = (Stats.Health / (1.0f - Mitigation));
            EffectiveHealth     = (Stats.Health / guaranteedReduction);
            GuaranteedReduction = (1.0f - guaranteedReduction);

            double a = Convert.ToDouble(DefendTable.AnyMiss);
            double h = Convert.ToDouble(Stats.Health);
            double H = Convert.ToDouble(AverageDamagePerHit);
            double s = Convert.ToDouble(BossOpts.DefaultMeleeAttack.AttackSpeed / BossOpts.DefaultMeleeAttack.AttackSpeed);
            BurstTime = Convert.ToSingle((1.0d / a) * ((1.0d / Math.Pow(1.0d - a, h / H)) - 1.0d) * s);
            /*
            // Attempt to make a different TTL:
            float damageTaken = Options.BossAttackValue; // for TTL(EH)
            float health = EffectiveHealth;
            float anyHit = 1.0f; // worst case, you get hit every swing
            float attacksToKill = (float)Math.Ceiling(health / DamageTaken); // So 10 health / 4 damage = 2.5 attacks = 3 attacks.
            float timeToDie = attacksToKill * attackSpeed; // time in seconds
            float chanceToDie = Convert.ToSingle((float)Math.Pow(anyHit, attacksToKill)); // (= 1^attacksToKill)
            float timeToLive = timeToDie * chanceToDie;
            BurstTime = timeToLive;
            */
        }

        public DefendModel(Character character, Base.StatsPaladin stats, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            Character   = character;
            Stats       = stats;
            CalcOpts    = calcOpts;
            BossOpts    = bossOpts;

            ParryModel = new ParryModel(character, stats, calcOpts, bossOpts);
            DefendTable = new DefendTable(character, stats, calcOpts, bossOpts);
            Calculate();
        }
    }
}
