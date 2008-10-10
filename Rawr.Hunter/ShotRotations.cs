using System;

namespace Rawr.Hunter
{

    public class RotationInfo
    {
        public double rotationDmg = 0;
        public double rotationTime = 0;

        public double DPS
        {
            get { return rotationDmg / rotationTime; }
        }
    }


    public class ShotRotationCalculator
    {
        Character character;
        CharacterCalculationsHunter calculatedStats;
        CalculationOptionsHunter options;
        double hawkRAPBonus;
        double totalStaticHaste;
        double effectiveRAPAgainstMob;
        double abilitiesCritDmgModifier;
        double yellowCritDmgModifier;
        double weaponDamageAverage;
        double ammoDamage;
        double talentModifiers;
        double armorReduction;
        double talentedArmorReduction;


        public ShotRotationCalculator(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter options, double totalStaticHaste, double effectiveRAPAgainstMob, double abilitiesCritDmgModifier, double yellowCritDmgModifier, double weaponDamageAverage, double ammoDamage, double talentModifiers)
        {
            this.character = character;
            this.calculatedStats = calculatedStats;
            this.options = options;
            this.hawkRAPBonus = 155 * (1.0 + 0.5 * character.HunterTalents.AspectMastery); // TODO: Level80
            this.totalStaticHaste = totalStaticHaste;
            this.effectiveRAPAgainstMob = effectiveRAPAgainstMob;
            this.abilitiesCritDmgModifier = abilitiesCritDmgModifier;
            this.yellowCritDmgModifier = yellowCritDmgModifier;
            this.weaponDamageAverage = weaponDamageAverage;
            this.ammoDamage = ammoDamage;
            this.talentModifiers = talentModifiers;
            double targetArmor = options.TargetArmor - calculatedStats.BasicStats.ArmorPenetration;
            this.armorReduction = 1.0 - (targetArmor / (467.5 * options.TargetLevel + targetArmor - 22167.5));

            targetArmor = options.TargetArmor * (1.0 - character.HunterTalents.PiercingShots * 0.02) - calculatedStats.BasicStats.ArmorPenetration;
            this.talentedArmorReduction = 1.0 - (targetArmor / (467.5 * options.TargetLevel + targetArmor - 22167.5));
        }


        public RotationInfo SteadyRotation()
        {
            RotationInfo info = new RotationInfo();
            ShotSteady(info);

            return info;
        }

        public RotationInfo ASSteadyRotation(int steadys)
        {
            RotationInfo info = new RotationInfo();

            ShotArcane(info, steadys);
            for (int i = 0; i < steadys; i++)
            {
                ShotSteady(info);
            }

            if (info.rotationTime < 6.0)
            {
                info.rotationTime = 6.0;
            }

            return info;
        }


        public RotationInfo ASSteadySerpentRotation()
        {
            RotationInfo info = new RotationInfo();

            ShotSerpentSting(info);
            ShotArcane(info, 3);
            ShotSteady(info);
            ShotSteady(info);
            ShotSteady(info);
            ShotArcane(info, 3);
            ShotSteady(info);
            ShotSteady(info);
            ShotSteady(info);

            return info;

        }

        public RotationInfo ExpSteadySerpRotation()
        {
            RotationInfo info = new RotationInfo();
            ShotSerpentSting(info);
            ShotExplosive(info);
            ShotSteady(info);
            ShotSteady(info);
            ShotSteady(info);
            ShotExplosive(info);
            ShotSteady(info);
            ShotSteady(info);
            ShotSteady(info);
            return info;
        }

        public RotationInfo ChimASSteadyRotation()
        {
            RotationInfo info = new RotationInfo();
            ShotChimera(info, 4);
            ShotArcane(info, 0);
            ShotSteady(info);
            ShotSteady(info);
            ShotSteady(info);
            ShotSteady(info);

            if (info.rotationTime < 10.0)
            {
                info.rotationTime = 10.0;
            }

            return info;

        }

        protected void ShotChimera(RotationInfo info, int steadyshots)
        {
            double chimCritDmgModifier = yellowCritDmgModifier + 0.02 * character.HunterTalents.MarkedForDeath;
            double critHitModifier = (calculatedStats.BasicStats.PhysicalCrit * chimCritDmgModifier + 1.0) * calculatedStats.BasicStats.Hit;

            double chimeraDmg = weaponDamageAverage * 1.25;

            double serpentStingDmg = (effectiveRAPAgainstMob + hawkRAPBonus) * 0.20 + 660; // TODO: Level80
            double serpentTalentModifiers = 1.0 + character.HunterTalents.ImprovedStings * 0.10;
            serpentStingDmg *= serpentTalentModifiers;
            
            chimeraDmg += serpentStingDmg * 0.4;

            double impSteadyChance = 1.0 - Math.Pow(1.0 - character.HunterTalents.ImprovedSteadyShot * 0.05, steadyshots);
            chimeraDmg *= 1.0 + 0.15 * impSteadyChance;

            chimeraDmg *= critHitModifier * talentModifiers;

            info.rotationDmg += chimeraDmg + 2.0/3.0 * serpentStingDmg * talentModifiers; // Add Serpent Sting dmg for 10 sec
            info.rotationTime += 1.5;

        }

        protected void ShotExplosive(RotationInfo info)
        {
            double explosiveCrit = calculatedStats.BasicStats.PhysicalCrit + 0.03 * character.HunterTalents.TNT + 0.02 * character.HunterTalents.SurvivalInstincts;
            double critHitModifier = (explosiveCrit * abilitiesCritDmgModifier + 1.0) * calculatedStats.BasicStats.Hit;

            double explosiveShotDmg = 0.08 * (effectiveRAPAgainstMob + hawkRAPBonus) + 238; // TODO: Level80

            explosiveShotDmg *= critHitModifier * talentModifiers;


            info.rotationDmg += explosiveShotDmg * 3.0;
            info.rotationTime += 1.5;

        }

        protected void ShotSteady(RotationInfo info)
        {
            double steadyCritDmgModifier = abilitiesCritDmgModifier + 0.02 * character.HunterTalents.MarkedForDeath;
            double critHitModifier = (calculatedStats.BasicStats.PhysicalCrit * steadyCritDmgModifier + 1.0) * calculatedStats.BasicStats.Hit;
            double steadyShotDmg = weaponDamageAverage + ammoDamage + (effectiveRAPAgainstMob + hawkRAPBonus) * 0.20 + 108; // TODO: Level80

            steadyShotDmg *= critHitModifier * talentModifiers;

            double steadyShotCastTime = 2.0 / totalStaticHaste;

            info.rotationDmg += steadyShotDmg * talentedArmorReduction;
            info.rotationTime += steadyShotCastTime > 1.5 ? steadyShotCastTime : 1.5;
        }

        protected void ShotArcane(RotationInfo info, int steadyshots)
        {
            double arcaneShotCrit = calculatedStats.BasicStats.PhysicalCrit + 0.02 * character.HunterTalents.SurvivalInstincts;

            double critHitModifier = (arcaneShotCrit * abilitiesCritDmgModifier + 1.0) * calculatedStats.BasicStats.Hit;
            double arcaneShotDmg = (effectiveRAPAgainstMob + hawkRAPBonus) * 0.15 + 273; // TODO: Level80

            double asTalentModifiers = 1.0 + character.HunterTalents.ImprovedArcaneShot * 0.05;

            double impSteadyChance = 1.0 - Math.Pow(1.0 - character.HunterTalents.ImprovedSteadyShot * 0.05, steadyshots);
            asTalentModifiers *= 1.0 + 0.15 * impSteadyChance;

            arcaneShotDmg *= critHitModifier * talentModifiers * asTalentModifiers;
            info.rotationDmg += arcaneShotDmg;
            info.rotationTime += 1.5;
        }

        protected void ShotSerpentSting(RotationInfo info)
        {
            double serpentStingDmg = (effectiveRAPAgainstMob + hawkRAPBonus) * 0.20 + 660; // TODO: Level80

            double serpentTalentModifiers = 1.0 + character.HunterTalents.ImprovedStings * 0.10;

            serpentStingDmg *= calculatedStats.BasicStats.Hit * talentModifiers * serpentTalentModifiers;

            info.rotationDmg += serpentStingDmg;
            info.rotationTime += 1.5;
        }
    }

}
