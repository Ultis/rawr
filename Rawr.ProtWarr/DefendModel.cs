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

        public readonly DefendTable DefendTable;

        public float DamagePerSecond { get; set; }
        public float AverageDamagePerAttack { get; set; }
        public float DamagePerHit { get; set; }
        public float DamagePerBlockedHit { get; set; }
        public float GuaranteedReduction { get; set; }
        public float Mitigation { get; set; }
        public float TankPoints { get; set; }
        public float EffectiveHealth { get; set; }

        public void Calculate()
        {
            float attackSpeed = Options.BossAttackSpeed;
            float armorReduction = (1.0f - Lookup.ArmorReduction(Character, Stats));
            float guaranteedReduction = (Lookup.StanceDamageReduction(Character, Stats) * armorReduction);
            
            DamagePerHit        = Options.BossAttackValue * guaranteedReduction;
            DamagePerBlockedHit = Math.Max(0.0f, DamagePerHit - Lookup.BlockReduction(Character, Stats));
            AverageDamagePerAttack = 
                ((DamagePerHit * DefendTable.Hit) + 
                (DamagePerBlockedHit * DefendTable.Block) + 
                (2.0f * DamagePerHit * DefendTable.Critical));
            DamagePerSecond     = AverageDamagePerAttack / attackSpeed;

            GuaranteedReduction = (1.0f - guaranteedReduction);
            Mitigation          = (1.0f - (AverageDamagePerAttack / Options.BossAttackValue));
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
