using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public abstract class CombatTable
    {
        protected Character Character;
        protected CalculationOptionsProtPaladin Options;
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

        protected void Initialize(Character character, Stats stats, Ability ability, CalculationOptionsProtPaladin options)
        {
            Character   = character;
            Options     = options;
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
            Miss = Math.Min(1.0f - tableSize,  Lookup.AvoidanceChance(Character, Stats, HitResult.Miss, Options.TargetLevel));
            tableSize += Miss;
            // Dodge
            Dodge = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Dodge, Options.TargetLevel));
            tableSize += Dodge;
            // Parry
            Parry = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Parry, Options.TargetLevel));
            tableSize += Parry;
            // Block
            if (Character.OffHand != null && Character.OffHand.Type == ItemType.Shield)
            {
                Block = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Character, Stats, HitResult.Block, Options.TargetLevel));
                tableSize += Block;
            }
            // Critical Hit
            Critical = Math.Min(1.0f - tableSize, Lookup.TargetCritChance(Character, Stats, Options.TargetLevel));
            tableSize += Critical;
            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);
            // Partial Resists don't belong in the combat table
            Resist = 1.0f - StatConversion.GetResistanceTable(Options.TargetLevel, Character.Level, Stats.FrostResistance, 0.0f)[0];
        }

        public DefendTable(Character character, Stats stats, CalculationOptionsProtPaladin options)
        {
            Initialize(character, stats, Ability.None, options);
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
            float SpellHitChance = Lookup.SpellHitChance(Character, Stats, Options.TargetLevel);
            float bonusExpertise = Lookup.BonusExpertisePercentage(Character, Stats);

            if (Lookup.IsSpell(Ability))
            {
                // Miss
                Miss = Math.Min(1.0f - tableSize, 1.0f - SpellHitChance);// - bonusHit));
                tableSize += Miss;
                // Crit
                Critical = Lookup.SpellCritChance(Character, Stats, Options.TargetLevel);
                tableSize += Critical;
            }
            else
            {
                float bonusHit = Lookup.HitChance(Character, Stats, Options.TargetLevel);
                
                // Miss
                // Miss = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Miss) - bonusHit));
                Miss = Math.Min(1.0f - tableSize, Math.Max(0.0f, 1.0f - bonusHit));
                tableSize += Miss;
                // Avoidance
                if (Lookup.IsAvoidable(Ability))
                {
                    // Dodge
                    Dodge = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Dodge, Options.TargetLevel) - bonusExpertise));
                    tableSize += Dodge;
                    // Parry
                    Parry = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Parry, Options.TargetLevel) - bonusExpertise));
                    tableSize += Parry;
                }
                // Glancing Blow
                if (Ability == Ability.None)
                {
                    Glance = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Glance, Options.TargetLevel)));
                    tableSize += Glance;
                }
                // Block
                if (Ability == Ability.None || Ability == Ability.HammerOfTheRighteous)
                {
                    Block = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Block, Options.TargetLevel)));
                    tableSize += Block;
                }
                // Critical Hit
                Critical = Math.Min(1.0f - tableSize, Lookup.BonusCritPercentage(Character, Stats, Ability, Options.TargetLevel, Options.TargetType));
                tableSize += Critical;//FIXME: Tablesize must not change when critchance is negative (boss)
            }

            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);

            // Partial Resist TODO: Partial Resists don't belong in the combat table, they're not an avoidance type but a damage multiplier.
            if (Lookup.HasPartials(Ability))
            {
                Resist = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Resist, Options.TargetLevel)));
            }
        }

        public AttackTable(Character character, Stats stats, CalculationOptionsProtPaladin options)
        {
            Initialize(character, stats, Ability.None, options);
        }

        public AttackTable(Character character, Stats stats, Ability ability, CalculationOptionsProtPaladin options)
        {
            Initialize(character, stats, ability, options);
        }
    }
}
