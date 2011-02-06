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

        public float AnyAvoid
        {
            get { return (AnyMiss + AnyBlock); }
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
        public float BaseBlock { get; protected set; }
        public float BuffedBlock { get; protected set; }
        public float BaseCriticalBlock { get; protected set; }
        public float BuffedCriticalBlock { get; protected set; }

        public float BaseAnyAvoid
        {
            get { return (AnyMiss + BaseBlock + BaseCriticalBlock); }
        }

        public float BuffedAnyAvoid
        {
            get { return (AnyMiss + BuffedBlock + BuffedCriticalBlock); }
        }

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
                Block               = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Player, HitResult.Block));
                BaseBlock           = Block;
                BuffedBlock         = Block;

                CriticalBlock       = Block * Math.Min(1.0f, Lookup.AvoidanceChance(Player, HitResult.CritBlock));
                BaseCriticalBlock   = CriticalBlock;
                BuffedCriticalBlock = CriticalBlock;

                // Average in Shield Block if Enabled
                if (Player.Options.UseShieldBlock)
                {
                    float shieldBlockUptime = 10.0f / Player.Options.ShieldBlockInterval;

                    BuffedBlock         = Math.Min(1.0f - tableSize, Lookup.AvoidanceChance(Player, HitResult.Block) + 0.25f);
                    BuffedCriticalBlock = BuffedBlock * Math.Min(1.0f, Lookup.AvoidanceChance(Player, HitResult.CritBlock) + (0.25f - (BuffedBlock - Block)));

                    Block               = (Block * (1.0f - shieldBlockUptime)) + (BuffedBlock * shieldBlockUptime);
                    CriticalBlock       = (CriticalBlock * (1.0f - shieldBlockUptime)) + (BuffedCriticalBlock * shieldBlockUptime);
                }
               
                tableSize   += Block;
                Block       -= CriticalBlock;
                BaseBlock   -= BaseCriticalBlock;
                BuffedBlock -= BuffedCriticalBlock;
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
                // Yellow Critical Hits, Two-Roll
                Critical = Math.Min(1.0f - tableSize, this.AnyHit * (Math.Max(0.0f, Lookup.BonusCritPercentage(Player, Ability)
                            - Lookup.TargetAvoidanceChance(Player, HitResult.Crit))));
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
