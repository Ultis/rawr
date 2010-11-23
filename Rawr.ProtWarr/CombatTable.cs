using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public abstract class CombatTable
    {
        protected Player Player;
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

        protected void Initialize(Player player, Ability ability)
        {
            Player  = player;
            Ability = ability;
            Calculate();
        }
    }

    public class DefendTable : CombatTable
    {
        protected override void Calculate()
        {
            float tableSize = 0.0f;

            // Miss
            Miss = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Player, HitResult.Miss));
            tableSize += Miss;
            // Dodge
            Dodge = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Player, HitResult.Dodge));
            tableSize += Dodge;
            // Parry
            Parry = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Player, HitResult.Parry));
            tableSize += Parry;
            // Block
            if (Player.Character.OffHand != null && Player.Character.OffHand.Type == ItemType.Shield)
            {
                Block = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Player, HitResult.Block));
                tableSize += Block;

                // Critical Block, two-roll system but fake the combat table entry
                CriticalBlock = Block * Lookup.AvoidanceChance(Player, HitResult.CritBlock);
                Block -= CriticalBlock;
            }
            // Critical Hit
            Critical = Math.Min(1.0f - tableSize, Lookup.TargetCritChance(Player));
            tableSize += Critical;
            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);
        }

        public DefendTable(Player player)
        {
            Initialize(player, Ability.None);
        }
    }

    public class AttackTable : CombatTable
    {
        protected override void Calculate()
        {
            float tableSize = 0.0f;
            float bonusHit = Lookup.BonusHitPercentage(Player);
            float bonusExpertise = Lookup.BonusExpertisePercentage(Player);

            // Miss
            Miss = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Player, HitResult.Miss) - bonusHit));
            tableSize += Miss;
            // Avoidance
            if (Lookup.IsAvoidable(Ability))
            {
                // Dodge
                Dodge = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Player, HitResult.Dodge) - bonusExpertise));
                tableSize += Dodge;
                // Parry
                Parry = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Player, HitResult.Parry) - bonusExpertise));
                tableSize += Parry;
            }
            if (Ability == Ability.None)
            {
                // White Glancing Blow
                Glance = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.TargetAvoidanceChance(Player, HitResult.Glance)));
                tableSize += Glance;

                // White Critical Hits
                Critical = Math.Max(0.0f, Math.Min(1.0f - tableSize, Lookup.BonusCritPercentage(Player, Ability))
                            - Lookup.TargetAvoidanceChance(Player, HitResult.Crit));
                tableSize += Critical;               
            }
            else
            {
                // Yellow Critical Hits
                Critical = Math.Min(1.0f - tableSize, Math.Max(0.0f, Lookup.BonusCritPercentage(Player, Ability)
                            - Lookup.TargetAvoidanceChance(Player, HitResult.Crit)));
                tableSize += Critical;
            }
            // Normal Hit
            Hit = Math.Max(0.0f, 1.0f - tableSize);
        }

        public AttackTable(Player player)
        {
            Initialize(player, Ability.None);
        }

        public AttackTable(Player player, Ability ability)
        {
            Initialize(player, ability);
        }
    }
}
