using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Spells
{
    // Thanks to Levva for help with Fire Elemental modelling
    // Elemental version currently only about 20% complete
    // This will likely go through many iterations, hopefully live 
    // by 2.3.23 at the latest.

    class FireElemental : Totem
    {
        private float ap = 0f;
        private float sp = 0f;
        private float petMeleeMissRate = 0f;
        private float petMeleeMultipliers = 0f;
        private float petSpellMissRate = 0f;
        private float petSpellMultipliers = 0f;

        private float meleeTotalDps;
        private float novaTotalDps;
        private float blastTotalDps;
        private float shieldTotalDps;
        private float totalDps;
        private float totalMana;
        private int novaUses;
        private int blastUses;
        private int meleeUses;
        private int additionalTargets;

        public FireElemental()
            : base()
        {
        }

        public FireElemental(ISpellArgs args)
            : base()
        {
            Initialize(args);
        }

        protected override void  SetBaseValues()
        {
            base.SetBaseValues();

            cooldown = 600;
        }

        public override void  Initialize(ISpellArgs args)
        {
            additionalTargets = args.AdditionalTargets;
            if(additionalTargets < 0)
                additionalTargets = 0;
            else if(additionalTargets > 9)
                additionalTargets = 9;

            if (args.Talents.GlyphofFireElementalTotem)
                cooldown = 300;
            
            ap = args.Stats.AttackPower;
            sp = args.Stats.SpellPower;
            petMeleeMissRate = args.Stats.HitRating;
            petMeleeMultipliers = args.Stats.BonusPetDamageMultiplier;
            petSpellMissRate = args.Stats.SpellHit;
            petSpellMultipliers = args.Stats.BonusPetDamageMultiplier;

            meleeTotalDps = 0f;
            novaTotalDps = 0f;
            blastTotalDps = 0f;
            shieldTotalDps = 0f;
            totalDps = 0f;
            totalMana = 4000 + (15 * (0.3f * args.Stats.Intellect));
            CalculateUses();
            CalculateDamage();
        }

        public float getDPS()
        {
            return totalDps;
        }

        private float getAverageUptime()
        {
            return 0f;
        }

        private void CalculateDamage()
        {
            float meleeBaseDps = (180 + (.6f * sp) + (2f / 14f) * ap) * meleeUses / 120f; // Subject to glancing blows
            float blastBaseDps = (775f + (.2f * sp)) * blastUses / 120f;
            float novaBaseDps = (987.5f + (.5f * sp)) * novaUses / 120f;
            float shieldBaseDps = (96 + (.015f * sp)) / 2f;

            float meleeNormal = meleeBaseDps * (1 - petMeleeMissRate - crit - .24f);
            float meleeCrits = meleeBaseDps * CritChance * 1.5f;
            float meleeGlances = meleeBaseDps * .75f;
            meleeTotalDps = (meleeNormal + meleeNormal + meleeGlances) * petMeleeMultipliers * getAverageUptime();

            float blastNormal = blastBaseDps * (1 - petSpellMissRate - CritChance);
            float blastCrits = blastBaseDps * 2f * CritChance;
            blastTotalDps = (blastNormal + blastCrits) * petSpellMultipliers * getAverageUptime(); 

            float novaNormal = novaBaseDps * (1 - petSpellMissRate - CritChance);
            float novaCrits = novaBaseDps * CritChance * 2f;
            novaTotalDps = (novaNormal + novaCrits) * petSpellMultipliers * (additionalTargets + 1) * getAverageUptime();

            float shieldNormal = shieldBaseDps * (1 - petSpellMissRate - CritChance);
            float shieldCrits = shieldBaseDps * CritChance * 2f;
            shieldTotalDps = (shieldNormal + shieldCrits) * petSpellMultipliers * (additionalTargets + 1) * getAverageUptime();

            totalDps = meleeTotalDps + novaTotalDps + blastTotalDps + shieldTotalDps;
        }

        private void CalculateUses()
        {
            float time = 2f;
            int sequence = 1;
            novaUses = 0;
            blastUses = 0;
            meleeUses = 0;

            while (totalMana > 0 && time < 120f)
            {
                switch (sequence)
                {
                    case (int)AbilitySequence.nova:
                        if (totalMana > 207)
                        {
                            novaUses++;
                            totalMana -= 207;
                            time += 2.5f;
                        }
                        break;
                    case (int)AbilitySequence.blast:
                        if (totalMana > 276)
                        {
                            blastUses++;
                            totalMana -= 276;
                            time += 4f;
                        }
                        break;
                    case (int)AbilitySequence.melee:
                        meleeUses++;
                        time += 2f;
                        break;
                }
                sequence++;
                if (sequence == 4) sequence = 1;
            }
        }

        private enum AbilitySequence
        {
            nova = 1,
            blast = 2,
            melee = 3
        }
    }
}

// All numbers below are curteousy of the EnhSim project (thank you, Levva)
//const f32 FIRE_ELEMENTAL_MANA = 0.23f;
//const int FIRE_ELEMENTAL_COOLDOWN = 60000;
//const int FIRE_ELEMENTAL_DURATION = 12000;
//const int FIRE_ELEMENTAL_SWING_BASE = 180;
//const f32 FIRE_ELEMENTAL_SWING_AP_COEFFICIENT = 2.0f / 14;
//const f32 FIRE_ELEMENTAL_SWING_SP_COEFFICIENT = 0.6f;
//const int FIRE_ELEMENTAL_SWING_SPEED = 200;
//const int FIRE_ELEMENTAL_FIRE_SHIELD_BASE = 96;
//const f32 FIRE_ELEMENTAL_FIRE_SHIELD_COEFFICIENT = 0.015f;
//const int FIRE_ELEMENTAL_FIRE_SHIELD_COOLDOWN = 300;
//const int FIRE_ELEMENTAL_FIRE_NOVA_BASE_LOW = 925;
//const int FIRE_ELEMENTAL_FIRE_NOVA_BASE_HIGH = 1050;
//const f32 FIRE_ELEMENTAL_FIRE_NOVA_COEFFICIENT = 0.5f;
//const int FIRE_ELEMENTAL_FIRE_NOVA_COOLDOWN = 500;
//const int FIRE_ELEMENTAL_FIRE_NOVA_CAST_TIME = 250;
//const f32 FIRE_ELEMENTAL_FIRE_NOVA_MANA = 207.0f;
//const int FIRE_ELEMENTAL_FIRE_BLAST_BASE_LOW = 725;
//const int FIRE_ELEMENTAL_FIRE_BLAST_BASE_HIGH = 825;
//const f32 FIRE_ELEMENTAL_FIRE_BLAST_COEFFICIENT = 0.2f;
//const int FIRE_ELEMENTAL_FIRE_BLAST_COOLDOWN = 500;
//const f32 FIRE_ELEMENTAL_FIRE_BLAST_MANA = 276.0f;
//const int FIRE_ELEMENTAL_MANA_BASE = 4000;
//const f32 FIRE_ELEMENTAL_MANA_COEFFICIENT = 0.3f;
//const f32 FIRE_ELEMENTAL_MANA_REGEN_PER_TICK = 10.66f;
//const int FIRE_ELEMENTAL_MELEE_CRIT_CHANCE_BASE = 0;
//const int FIRE_ELEMENTAL_SPELL_CRIT_CHANCE_BASE = 0;
//const f32 FIRE_ELEMENTAL_MELEE_CRIT_MODIFIER = 2.0f;
//const f32 FIRE_ELEMENTAL_SPELL_CRIT_MODIFIER = 1.5f;
