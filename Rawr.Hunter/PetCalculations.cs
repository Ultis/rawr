using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter
{
    class PetCalculations
    {
    
        Character character;
        CharacterCalculationsHunter calculatedStats;
        CalculationOptionsHunter options;
        Stats statsBuffs;

        double armorReduction;

        double specialAttackSpeed;
        double whiteAttackSpeed;


        public double ferociousInspirationUptime;

        public PetCalculations(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter options, Stats statsBuffs)
        {
            this.character = character;
            this.calculatedStats = calculatedStats;
            this.options = options;
            this.statsBuffs = statsBuffs;
            

            double targetArmor = options.TargetArmor - statsBuffs.ArmorPenetration;
            this.armorReduction = 1.0 - (targetArmor / (467.5 * options.TargetLevel + targetArmor - 22167.5));

            calculatePetStats();

        }

        private void calculatePetStats()
        {
            Stats petStats = new Stats()
            {
                Agility = 128,
                Strength = 162,
                Stamina = 307,
                Intellect = 33,
                Spirit = 99
                //TODO: Level80
            };


            petStats.AttackPower = (petStats.Strength - 10f) * 2f;
            petStats.AttackPower += (calculatedStats.BasicStats.RangedAttackPower * .22f);

            #region Hit
            double petHitChance = calculatedStats.BasicStats.PhysicalHit;
            petHitChance += (.02 * character.HunterTalents.AnimalHandler);

            /*
            if (character.Race == Character.CharacterRace.Draenei)
            {
                petHitChance += .01;
            }
            if (options.TargetLevel == 73) // TODO: Level80
            {
                petHitChance -= (5 * .004 + .02);
            }
            else
            {
                petHitChance -= ((options.TargetLevel * 5 - 350) * .001);
            }
             * */
            if (petHitChance > 1.0)
            {
                petHitChance = 1.0;
            }
            petStats.PhysicalHit = (float)petHitChance;
            #endregion

            //petStats.PhysicalCrit = 0.0320f;
            petStats.PhysicalCrit += petStats.Agility / 2560f;

            petStats.PhysicalCrit += (.02f * character.HunterTalents.Ferocity);

            petStats.PhysicalCrit -= ((options.TargetLevel * 5f - 350f) * .0004f);

            petStats.PhysicalCrit += calculatedStats.BasicStats.BonusPetCritChance;

            calculatedStats.PetStats = petStats;

        }

        private float getSpecialDPS()
        {

            double focus = (24.0 + 12.0 * character.HunterTalents.BestialDiscipline) / 4.0;

            double shotsPerSecond = 1.0 / calculatedStats.BaseAttackSpeed + 1.0 / 1.5;

            focus += shotsPerSecond * calculatedStats.BasicStats.PhysicalCrit * character.HunterTalents.GoForTheThroat * 25.0;

            double abDmg = 0.07 * calculatedStats.PetStats.AttackPower;

            double clawDmg = (118.0 + 168.0) / 2.0 + abDmg;

            double petHitCrit = (1.0 + calculatedStats.PetStats.PhysicalCrit) * calculatedStats.PetStats.PhysicalHit;

            clawDmg *= petHitCrit;

            clawDmg *= 1.0 + character.HunterTalents.UnleashedFury * 0.04;
            clawDmg *= 1.0 + character.HunterTalents.KindredSpirits * 0.04;

            double clawsPerSecond = focus / 25.0;
            if (clawsPerSecond > 1.0 / 1.5)
            {
                clawsPerSecond = 1.0 / 1.5;
            }

            specialAttackSpeed = 1.0 / clawsPerSecond;

            return (float)(clawDmg * clawsPerSecond);
        }

        private void getFerociousInspirationUptime()
        {
            ferociousInspirationUptime = 1.0 - Math.Pow(1.0 - calculatedStats.PetStats.PhysicalCrit, 10.0/whiteAttackSpeed + 10.0/specialAttackSpeed);
        }

        public float getDPS()
        {
            double petDmg = 60;
            double petBaseAttackSpeed = 2.0;

            // TODO: Level80

            double petAttackSpeed = petBaseAttackSpeed / (1.0 + character.HunterTalents.SerpentsSwiftness * 0.02);
            petAttackSpeed /= 1.0 + 0.3; // TODO: Cobra Reflexes

            petDmg += (calculatedStats.PetStats.AttackPower / 14.0) * petBaseAttackSpeed;
            double petHitCrit = (1.0 + calculatedStats.PetStats.PhysicalCrit) * calculatedStats.PetStats.PhysicalHit;

            petDmg *= petHitCrit;

            petDmg *= 1.0 + character.HunterTalents.UnleashedFury * 0.04;
            petDmg *= 1.0 + character.HunterTalents.KindredSpirits * 0.04;

            calculatedStats.PetSpecialDPS = getSpecialDPS() * this.armorReduction;


            whiteAttackSpeed = petAttackSpeed;
            whiteAttackSpeed /= 1.30;
            double frenzyUptime = 1.0 - Math.Pow(1.0 - character.HunterTalents.Frenzy / 5.0 * calculatedStats.PetStats.PhysicalCrit, 8.0 / whiteAttackSpeed + 8.0 / specialAttackSpeed);
            whiteAttackSpeed = whiteAttackSpeed * frenzyUptime + petAttackSpeed * (1.0 - frenzyUptime);


            calculatedStats.PetBaseDPS = (petDmg * this.armorReduction) / whiteAttackSpeed;

            getFerociousInspirationUptime();
            double fi = 1.0 + character.HunterTalents.FerociousInspiration * 0.01 * ferociousInspirationUptime;
            calculatedStats.PetBaseDPS *= fi;
            calculatedStats.PetSpecialDPS *= fi;

            return (float)(calculatedStats.PetBaseDPS + calculatedStats.PetSpecialDPS);
        }
    }
}
