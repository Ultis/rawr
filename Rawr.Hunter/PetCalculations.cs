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
        
        public PetCalculations(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter options)
        {
            this.character = character;
            this.calculatedStats = calculatedStats;
            this.options = options;

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
            petStats.AttackPower += (calculatedStats.RAP * .22f);

            #region Hit
            double petHitChance = calculatedStats.BasicStats.Hit;
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
            petStats.Hit = (float)petHitChance;
            #endregion

            petStats.Crit += petStats.Agility / 2560f;

            petStats.Crit += (.02f * character.HunterTalents.Ferocity);

            petStats.Crit -= ((options.TargetLevel * 5f - 350f) * .0004f);

            petStats.Crit += calculatedStats.BasicStats.BonusPetCritChance;

            calculatedStats.PetStats = petStats;

        }


        public float getDPS()
        {
            double petDmg = 60;
            double petBaseAttackSpeed = 2.0;

            // TODO: Level80

            double petAttackSpeed = petBaseAttackSpeed / (1.0 + character.HunterTalents.SerpentsSwiftness * 0.02);
            petAttackSpeed /= 1.0 + 0.3; // TODO: Cobra Reflexes

            petDmg += (calculatedStats.PetStats.AttackPower / 14.0) * petBaseAttackSpeed;
            double petHitCrit = (1.0 + calculatedStats.PetStats.Crit) * calculatedStats.PetStats.Hit;

            petDmg *= petHitCrit;

            petDmg *= 1.0 + character.HunterTalents.UnleashedFury * 0.04;
            petDmg *= 1.0 + character.HunterTalents.KindredSpirits * 0.04;

            calculatedStats.PetBaseDPS = petDmg / petAttackSpeed;

            return (float)calculatedStats.PetBaseDPS;
        }
    }
}
