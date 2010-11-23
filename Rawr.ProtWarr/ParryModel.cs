using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    /* REMOVED DUE TO LACK OF PARRY HASTE IN CATACLYSM
     * KEEPING CODE FOR REFERENCE IF IT RETURNS!
     
    public class ParryModel
    {
        private Character Character;
        private CalculationOptionsProtWarr CalcOpts;
        private BossOptions BossOpts;
        private Stats Stats;

        private AttackTable AttackTable;
        private DefendTable DefendTable;

        public float BossAttackSpeed { get; private set; }
        public float WeaponSpeed { get; private set; }

        private void Calculate()
        {
            float globalCooldownSpeed   = Lookup.GlobalCooldownSpeed(Character, true);
            float baseBossAttackSpeed   = CalcOpts.BossAttackSpeed * (1.0f - Stats.BossAttackSpeedMultiplier);
            float baseWeaponSpeed       = Lookup.WeaponSpeed(Character, Stats);
            float bossAttackHaste       = 0.0f;
            float weaponHaste           = 0.0f;
            
            BossAttackSpeed             = baseBossAttackSpeed;
            WeaponSpeed                 = baseWeaponSpeed;

            if (CalcOpts.UseParryHaste)
            {
                // Iterate on this a few times to get a 'stable' result
                for (int j = 0; j < 4; j++)
                {
                    weaponHaste = DefendTable.Parry * 0.24f * (WeaponSpeed / BossAttackSpeed);
                    bossAttackHaste = AttackTable.Parry * 0.24f * ((BossAttackSpeed / WeaponSpeed) + (BossAttackSpeed / globalCooldownSpeed));
                    
                    WeaponSpeed     = baseWeaponSpeed / (1.0f + weaponHaste);
                    BossAttackSpeed = baseBossAttackSpeed / (1.0f + bossAttackHaste);
                }
            }
            else
            {
                // Simple adjust to the defender's speed if the attacker isn't parry hasted
                WeaponSpeed /= (1.0f + (DefendTable.Parry * 0.24f * (WeaponSpeed / BossAttackSpeed)));
            }
        }

        public ParryModel(Character character, Stats stats, CalculationOptionsProtWarr calcOpts, BossOptions bossOpts)
        {
            Character   = character;
            Stats       = stats;
            CalcOpts     = calcOpts;
            BossOpts    = bossOpts; 
            AttackTable = new AttackTable(character, stats, calcOpts, bossOpts);
            DefendTable = new DefendTable(character, stats, calcOpts, bossOpts);

            Calculate();
        }
    }
    */
}
