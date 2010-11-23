using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public abstract class CombatTable
    {
        protected Character Character;
        protected CalculationOptionsProtWarr CalcOpts;
        protected BossOptions BossOpts;
        protected Stats Stats;
        protected Ability Ability;

        public float Miss { get; protected set; }
        public float Dodge { get; protected set; }
        public float Parry { get; protected set; }
        public float Block { get; protected set; }
        public float CriticalBlock { get; protected set; }
        public float Glance { get; protected set; }
        public float Critical { get; protected set; }
        public float Hit { get; protected set; }

        public float AnyMiss
        {
            get { return (Miss + Dodge + Parry); }
        }

        public float AnyBlock
        {
            get { return (Block + CriticalBlock); }
        }

        public float DodgeParryBlock
        {
            get { return (Dodge + Parry + AnyBlock); }
        }

        public float AnyHit
        {
            get { return (1.0f - AnyMiss); }
        }

        protected virtual void Calculate()
        {
        }

        protected void Initialize(Character character, Stats stats, CalculationOptionsProtWarr calcOpts, BossOptions bossOpts, Ability ability)
        {
            Character   = character;
            CalcOpts    = calcOpts;
            BossOpts    = bossOpts;
            Stats       = stats;
            Ability     = ability;
            Calculate();
        }
    }

    public class DefendTable : CombatTable
    {
        protected override void Calculate()
        {
            float tableSize = 0.0f;

            // Miss
            Miss = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Miss, BossOpts.Level));
            tableSize += Miss;
            // Dodge
            Dodge = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Dodge, BossOpts.Level));
            tableSize += Dodge;
            // Parry
            Parry = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Parry, BossOpts.Level));
            tableSize += Parry;
            // Block
            if (Character.OffHand != null && Character.OffHand.Type == ItemType.Shield)
            {
                Block = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Block, BossOpts.Level));
                tableSize += Block;

                // Critical Block, two-roll system but fake the combat table entry
                CriticalBlock = Block * Lookup.AvoidanceChance(Character, Stats, HitResult.CritBlock, BossOpts.Level);
                Block -= CriticalBlock;
            }
            // Critical Hit
            Critical = Math.Min(1.0f - tableSize, Lookup.TargetCritChance(Character, Stats, BossOpts.Level));
            tableSize += Critical;
            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);
        }

        public DefendTable(Character character, Stats stats, CalculationOptionsProtWarr calcOpts, BossOptions bossOpts)
        {
            Initialize(character, stats, calcOpts, bossOpts, Ability.None);
        }
    }

    public class AttackTable : CombatTable
    {
        protected override void Calculate()
        {
            float tableSize = 0.0f;
            float bonusHit = Lookup.BonusHitPercentage(Character, Stats);
            float bonusExpertise = Lookup.BonusExpertisePercentage(Character, Stats);

            // Miss
            Miss = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Miss, BossOpts.Level) - bonusHit));
            tableSize += Miss;
            // Avoidance
            if (Lookup.IsAvoidable(Ability))
            {
                // Dodge
                Dodge = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Dodge, BossOpts.Level) - bonusExpertise));
                tableSize += Dodge;
                // Parry
                Parry = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Parry, BossOpts.Level) - bonusExpertise));
                tableSize += Parry;
            }
            if (Ability == Ability.None)
            {
                // White Glancing Blow
                Glance = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Glance, BossOpts.Level)));
                tableSize += Glance;

                // White Critical Hits
                Critical = Math.Max(0.0f, Math.Min(1.0f - tableSize, Lookup.BonusCritPercentage(Character, Stats, Ability, BossOpts.Level))
                            - Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Crit, BossOpts.Level));
                tableSize += Critical;               
            }
            else
            {
                // Yellow Critical Hits
                Critical = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.BonusCritPercentage(Character, Stats, Ability, BossOpts.Level) 
                            - Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Crit, BossOpts.Level)));
                tableSize += Critical;
            }
            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);
        }

        public AttackTable(Character character, Stats stats, CalculationOptionsProtWarr calcOpts, BossOptions bossOpts)
        {
            Initialize(character, stats, calcOpts, bossOpts, Ability.None);
        }

        public AttackTable(Character character, Stats stats, CalculationOptionsProtWarr calcOpts, BossOptions bossOpts, Ability ability)
        {
            Initialize(character, stats, calcOpts, bossOpts, ability);
        }
    }
}
