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
        double weaponDamageAverage;
        double ammoDamage;
        double talentModifiers;
        double armorReduction;


        public ShotRotationCalculator(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter options, double totalStaticHaste, double effectiveRAPAgainstMob, double abilitiesCritDmgModifier, double weaponDamageAverage, double ammoDamage, double talentModifiers)
        {
            this.character = character;
            this.calculatedStats = calculatedStats;
            this.options = options;
            this.hawkRAPBonus = 155 * (1.0 + 0.5 * character.HunterTalents.AspectMastery); // TODO: Level80
            this.totalStaticHaste = totalStaticHaste;
            this.effectiveRAPAgainstMob = effectiveRAPAgainstMob;
            this.abilitiesCritDmgModifier = abilitiesCritDmgModifier;
            this.weaponDamageAverage = weaponDamageAverage;
            this.ammoDamage = ammoDamage;
            this.talentModifiers = talentModifiers;
            double targetArmor = options.TargetArmor - calculatedStats.BasicStats.ArmorPenetration;
            this.armorReduction = 1.0 - (targetArmor / (467.5 * options.TargetLevel + targetArmor - 22167.5));
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
            ShotArcane(info, 2); // 2 may not be right in every situation
            ShotSteady(info);
            ShotSteady(info);
            ShotSteady(info);
            ShotArcane(info, 3);
            ShotSteady(info);
            ShotSteady(info);
            if (info.rotationTime < 13.5) // enough haste for gcd cap on steady?
            {
                ShotSteady(info);
            }

            return info;

        }

        protected void ShotSteady(RotationInfo info)
        {
            double critHitModifier = (calculatedStats.BasicStats.Crit * abilitiesCritDmgModifier + 1.0) * calculatedStats.BasicStats.Hit;
            double steadyShotDmg = weaponDamageAverage + ammoDamage + (effectiveRAPAgainstMob + hawkRAPBonus) * 0.20 + 108; // TODO: Level80

            steadyShotDmg *= critHitModifier * talentModifiers;

            double steadyShotCastTime = 2.0 / totalStaticHaste;

            info.rotationDmg += steadyShotDmg * armorReduction;
            info.rotationTime += steadyShotCastTime > 1.5 ? steadyShotCastTime : 1.5;
        }

        protected void ShotArcane(RotationInfo info, int steadyshots)
        {
            double arcaneShotCrit = calculatedStats.BasicStats.Crit + 0.02 * character.HunterTalents.SurvivalInstincts;

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