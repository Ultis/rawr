using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    class DefendModel
    {
        private Character Character;
        private Stats Stats;
        private CalculationOptionsProtWarr Options;

        public readonly DefendTable DefendTable;

        private float _damagePerSecond = 0.0f;
        public float DamagePerSecond
        {
            get { return _damagePerSecond; }
            private set { _damagePerSecond = value; }
        }

        private float _damagePerHit = 0.0f;
        public float DamagePerHit
        {
            get { return _damagePerHit; }
            private set { _damagePerHit = value; }
        }

        public float EffectiveHealth
        {
            get { return (Stats.Health / ((1.0f - Lookup.ArmorReduction(Character, Stats)) * 0.9f)); }
        }

        public float TankPoints
        {
            get { return (Stats.Health / (DamagePerHit / Options.BossAttackValue)); }
        }

        public void Calculate()
        {
            float attackSpeed = 2.0f;
            float hitDamage = Options.BossAttackValue * Lookup.StanceDamageReduction(Character, Stats) * Lookup.ArmorReduction(Character, Stats);
            float blockDamage = (Math.Max(0.0f, hitDamage - Lookup.BlockReduction(Character, Stats)) * DefendTable.Block);

            DamagePerHit = ((hitDamage * DefendTable.Hit) + (blockDamage * DefendTable.Block) + (2.0f * hitDamage * DefendTable.Critical));
            DamagePerSecond = DamagePerHit / attackSpeed;
        }

        public DefendModel(Character character, Stats stats)
        {
            Character   = character;
            Stats       = stats;
            Options     = Character.CalculationOptions as CalculationOptionsProtWarr;
            DefendTable = new DefendTable(character, stats); 
            Calculate();
        }
    }
}
