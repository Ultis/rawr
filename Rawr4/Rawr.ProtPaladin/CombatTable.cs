using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public abstract class CombatTable
    {
        protected Character Character;
        protected CalculationOptionsProtPaladin CalcOpts;
        protected BossOptions BossOpts;
        protected Stats Stats;
        protected Ability Ability;//TODO: expand the Ability Class to include DamageType(School) and AttackType

        public float Miss { get; protected set; }
        public float Dodge { get; protected set; }
        public float Parry { get; protected set; }
        public float Block { get; protected set; }
        public float Glance { get; protected set; }
        public float Resist { get; protected set; }
        public float Critical { get; protected set; }
        public float Hit { get; protected set; }

        public float AnyHit
        {
            get { return (1.0f - (Miss + Dodge + Parry)); }
        }

        public float AnyMiss
        {
            get { return (Miss + Dodge + Parry); }
        }

        protected virtual void Calculate() {}

        protected void Initialize(Character character, Stats stats, Ability ability, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
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
        private bool UseHolyShield { get; set; }

        protected override void Calculate()
        {
            // Hack to not count holy shield when we are trying to calculate crit chance without it
            if (!UseHolyShield && CalcOpts.UseHolyShield)
            {
                Stats.Accumulate(new Stats() { Block = -0.3f });
            }

            float tableSize = 0.0f;

            int targetLevel = BossOpts.Level;

            // Miss
            Miss = Math.Min(1.0f - tableSize,  Lookup.AvoidanceChance(Character, Stats, HitResult.Miss, targetLevel));
            tableSize += Miss;
            // Dodge
            Dodge = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Dodge, targetLevel));
            tableSize += Dodge;
            // Parry
            Parry = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Parry, targetLevel));
            tableSize += Parry;
            // Block
            if (Character.OffHand != null && Character.OffHand.Type == ItemType.Shield)
            {
                Block = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Block, targetLevel));
                tableSize += Block;
            }
            // Critical Hit
            Critical = Math.Min(1.0f - tableSize, Lookup.TargetCritChance(Character, Stats, targetLevel));
            tableSize += Critical;
            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);
            // Partial Resists don't belong in the combat table
            Resist = 1.0f - StatConversion.GetResistanceTable(targetLevel, Character.Level, Stats.FrostResistance, 0.0f)[0];

            // Hack to put back holy shield when we are trying to calculate crit chance without it
            if (!UseHolyShield && CalcOpts.UseHolyShield)
            {
                Stats.Accumulate(new Stats() { Block = 0.3f });
            }
        }

        public DefendTable(Character character, Stats stats, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts, bool useHolyShield)
        {
            UseHolyShield = useHolyShield;
            Initialize(character, stats, Ability.MeleeSwing, calcOpts, bossOpts);
        }
    }
/*
    public class ResistTable : CombatTable
    {
        protected override void Calculate()
        {
            float tableSize = 0.0f;

            // Miss
            Miss = Math.Min(1.0f - tableSize,  Lookup.AvoidanceChance(Character, Stats, HitResult.Miss));
            tableSize += Miss;
            // Dodge
            Dodge = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Dodge));
            tableSize += Dodge;
            // Parry
            Parry = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Parry));
            tableSize += Parry;
            // Block
            if (Character.OffHand != null && Character.OffHand.Type == ItemType.Shield)
            {
                Block = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Block));
                tableSize += Block;
            }
            // Critical Hit
            Critical = Math.Min(1.0f - tableSize, Lookup.TargetCritChance(Character, Stats));
            tableSize += Critical;
            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);
        }

        public ResistTable(Character character, Stats stats)
        {
            Initialize(character, stats, Ability.None);
        }
    }
*/
    public class AttackTable : CombatTable
    {
        protected override void Calculate()
        {
            float tableSize = 0.0f;

            int targetLevel = BossOpts.Level;

            float SpellHitChance = Lookup.SpellHitChance(Character, Stats, targetLevel);
            float bonusExpertise = Lookup.BonusExpertisePercentage(Character, Stats);

            if (Lookup.IsSpell(Ability))
            {
                // Miss
                Miss = Math.Min(1.0f - tableSize, 1.0f - SpellHitChance);// - bonusHit));
                tableSize += Miss;
                // Crit
                Critical = Lookup.SpellCritChance(Character, Stats, targetLevel);
                tableSize += Critical;
            }
            else
            {
                float bonusHit = Lookup.HitChance(Character, Stats, targetLevel);
                
                // Miss
                // Miss = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Miss) - bonusHit));
                Miss = Math.Min(1.0f - tableSize, Math.Max(0.0f, 1.0f - bonusHit));
                tableSize += Miss;
                // Avoidance
                if (Lookup.IsAvoidable(Ability))
                {
                    // Dodge
                    Dodge = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Dodge, targetLevel) - bonusExpertise));
                    tableSize += Dodge;
                    // Parry
                    Parry = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Parry, targetLevel) - bonusExpertise));
                    tableSize += Parry;
                }
                // Glancing Blow
                if (Ability == Ability.MeleeSwing)
                {
                    Glance = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Glance, targetLevel)));
                    tableSize += Glance;
                }
                // Block
                if (Ability == Ability.MeleeSwing || Ability == Ability.HammerOfTheRighteous)
                {
                    Block = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Block, targetLevel)));
                    tableSize += Block;
                }
                // Critical Hit
                Critical = Math.Min(1.0f - tableSize, Lookup.BonusCritPercentage(Character, Stats, Ability, targetLevel, CalcOpts.TargetType));
                tableSize += Critical;//FIXME: Tablesize must not change when critchance is negative (boss)
            }

            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);
        }

        public AttackTable(Character character, Stats stats, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            Initialize(character, stats, Ability.MeleeSwing, calcOpts, bossOpts);
        }

        public AttackTable(Character character, Stats stats, Ability ability, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            Initialize(character, stats, ability, calcOpts, bossOpts);
        }
    }
}
