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

        private float _guaranteedReduction = 0.0f;
        public float GuaranteedReduction
        {
            get { return _guaranteedReduction; }
            private set { _guaranteedReduction = value; }
        }

        private float _mitigation = 0.0f;
        public float Mitigation
        {
            get { return _mitigation; }
            private set { _mitigation = value; }
        }

        private float _tankPoints = 0.0f;
        public float TankPoints
        {
            get { return _tankPoints; }
            private set { _tankPoints = value; }
        }

        private float _effectiveHealth = 0.0f;
        public float EffectiveHealth
        {
            get { return _effectiveHealth; }
            private set { _effectiveHealth = value; }
        }

        public void Calculate()
        {
            float attackSpeed = 2.0f;
            float armorReduction = (1.0f - Lookup.ArmorReduction(Character, Stats));
            float guaranteedReduction = (Lookup.StanceDamageReduction(Character, Stats) * armorReduction);
            float hitDamage = Options.BossAttackValue * guaranteedReduction;
            float blockDamage = Math.Max(0.0f, hitDamage - Lookup.BlockReduction(Character, Stats));

            DamagePerHit        = ((hitDamage * DefendTable.Hit) + (blockDamage * DefendTable.Block) + (2.0f * hitDamage * DefendTable.Critical));
            DamagePerSecond     = DamagePerHit / attackSpeed;

            GuaranteedReduction = (1.0f - guaranteedReduction);
            Mitigation          = (1.0f - (DamagePerHit / Options.BossAttackValue));
            TankPoints          = (Stats.Health / (1.0f - Mitigation));
            EffectiveHealth     = (Stats.Health / guaranteedReduction);
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
