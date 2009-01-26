using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public static class Lookup
    {
        public static float LevelModifier(Character character)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            return (calcOpts.TargetLevel - 80f) * 0.2f;
        }

        public static float TargetArmorReduction(Character character, Stats stats)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;

            // Needs Armor Penetration Support, but requires adding rating support into CalculationsProtWarrior
            return Math.Max(0.0f, Math.Min(0.75f, calcOpts.TargetArmor / (calcOpts.TargetArmor + (467.5f * character.Level - 22167.5f))));
        }

        public static float TargetCritChance(Character character, Stats stats)
        {
            return Math.Max(0.0f, ((5.0f + Lookup.LevelModifier(character)) / 100.0f) - AvoidanceChance(character, stats, HitResult.Crit));
        }

        public static float TargetAvoidanceChance(Character character, Stats stats, HitResult avoidanceType)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;

            switch (avoidanceType)
            {
                case HitResult.Miss:
                    if ((calcOpts.TargetLevel - 80f) < 3)
                        return 0.05f + 0.005f * (calcOpts.TargetLevel - 80f);
                    else
                        return 0.08f;

                case HitResult.Dodge:
                    return 0.064f;

                case HitResult.Parry:
                    if ((calcOpts.TargetLevel - 80f) < 3)
                        return 0.065f;
                    else
                        return 0.1375f;

                case HitResult.Glance:
                    return 0.1f + ((calcOpts.TargetLevel - character.Level) * 0.05f);

                default:
                    return 0.0f;
            }
        }

        public static float StanceDamageMultipler(Character character, Stats stats)
        {
            // In Defensive Stance
            return (0.9f * (1.0f + character.WarriorTalents.ImprovedDefensiveStance * 0.05f) * (1.0f + stats.BonusDamageMultiplier));
        }

        public static float StanceThreatMultipler(Character character, Stats stats)
        {
            // In Defensive Stance
            return (2.0735f * (1.0f + stats.ThreatIncreaseMultiplier));
        }

        public static float BonusExpertisePercentage(Character character, Stats stats)
        {
            return (float)Math.Round((stats.ExpertiseRating * ProtWarr.ExpertiseRatingToExpertise + stats.Expertise))
                    * ProtWarr.ExpertiseToDodgeParryReduction / 100.0f;
        }

        public static float BonusHitPercentage(Character character, Stats stats)
        {
            return ((stats.HitRating * ProtWarr.HitRatingToHit + stats.PhysicalHit) / 100.0f);
        }

        public static float BonusCritPercentage(Character character, Stats stats)
        {
            // Not sure about how LotP is being integrated in here vs. other buffs or buffed crit rating, look into cleaning this up
            return Math.Min(1.0f, (stats.PhysicalCrit + ((stats.CritRating + stats.LotPCritRating) * ProtWarr.CritRatingToCrit) +
                                    (stats.Agility * ProtWarr.AgilityToCrit) - LevelModifier(character)) / 100.0f);
        }

        public static float BonusCritPercentage(Character character, Stats stats, Ability ability)
        {
            // Grab base melee crit chance before adding ability-specific crit chance
            float abilityCritChance = BonusCritPercentage(character, stats);

            switch (ability)
            {
                case Ability.Cleave:
                    abilityCritChance += character.WarriorTalents.Incite * 0.05f;
                    break;
                case Ability.Devastate:
                    abilityCritChance += character.WarriorTalents.SwordAndBoard * 0.05f;
                    break;
                case Ability.HeroicStrike:
                    abilityCritChance += character.WarriorTalents.Incite * 0.05f;
                    break;
                case Ability.ShieldSlam:
                    abilityCritChance += character.WarriorTalents.CriticalBlock * 0.05f;
                    break;
                case Ability.ThunderClap:
                    abilityCritChance += character.WarriorTalents.Incite * 0.05f;
                    break;
                case Ability.SunderArmor:
                case Ability.ShieldBash:
                case Ability.Rend:
                case Ability.DeepWounds:
                    abilityCritChance = 0.0f;
                    break;
            }

            return Math.Min(1.0f, abilityCritChance);
        }

        public static float WeaponDamage(Character character, Stats stats, bool normalized)
        {
            float weaponDamage = 0.0f;

            if (character.MainHand != null)
            {
                float weaponSpeed     = character.MainHand.Speed;
                float weaponMinDamage = character.MainHand.MinDamage;
                float weaponMaxDamage = character.MainHand.MaxDamage;
                float normalizedSpeed = 1.0f;
                if (character.MainHand.Type == Item.ItemType.Dagger)
                    normalizedSpeed = 1.7f;
                else
                    normalizedSpeed = 2.4f;
            
                // Non-Normalized Hits
                if (!normalized)
                    weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (weaponSpeed * stats.AttackPower / 14.0f)) + stats.WeaponDamage;
                // Normalized Hits
                else
                    weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (normalizedSpeed * stats.AttackPower / 14.0f)) + stats.WeaponDamage;
            }

            return weaponDamage;
        }

        public static float WeaponSpeed(Character character, Stats stats)
        {
            if (character.MainHand != null)
                return Math.Max(1.0f, character.MainHand.Speed / (1.0f + (stats.HasteRating * ProtWarr.HasteRatingToHaste / 100.0f)));
            else
                return 1.0f;
        }

        public static float GlancingReduction(Character character)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            return (Math.Min(0.91f, 1.3f - (0.05f * (calcOpts.TargetLevel - character.Level) * 5.0f)) +
                    Math.Max(0.99f, 1.2f - (0.03f * (calcOpts.TargetLevel - character.Level) * 5.0f))) / 2;
        }

        public static bool IsAvoidable(Ability ability)
        {
            switch (ability)
            {
                case Ability.DamageShield:
                case Ability.DeepWounds:
                case Ability.Shockwave:
                case Ability.ThunderClap:
                    return false;
                default:
                    return true;
            }
        }

        public static float AvoidanceChance(Character character, Stats stats, HitResult avoidanceType)
        {
            float defSkill = (float)Math.Floor(stats.DefenseRating * ProtWarr.DefenseRatingToDefense);
            float baseAvoid = 0.0f;
            float modifiedAvoid = 0.0f;

            switch (avoidanceType)
            {
                case HitResult.Dodge:
                    baseAvoid = stats.Dodge + (stats.BaseAgility * ProtWarr.AgilityToDodge) - LevelModifier(character);
                    modifiedAvoid = ((stats.Agility - stats.BaseAgility) * ProtWarr.AgilityToDodge) +
                                        (stats.DodgeRating * ProtWarr.DodgeRatingToDodge) + (defSkill * ProtWarr.DefenseToDodge);
                    modifiedAvoid = 1.0f / (1.0f / 88.129021f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Parry:
                    baseAvoid = stats.Parry - LevelModifier(character);
                    modifiedAvoid = (stats.ParryRating * ProtWarr.ParryRatingToParry) + (defSkill * ProtWarr.DefenseToParry);
                    modifiedAvoid = 1.0f / (1.0f / 47.003525f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Miss:
                    baseAvoid = stats.Miss * 100f - LevelModifier(character);
                    modifiedAvoid = (defSkill * ProtWarr.DefenseToMiss);
                    modifiedAvoid = 1.0f / (1.0f / 50.0f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Block:
                    // The 5% base block should be moved into stats.Block as a base value like the others
                    baseAvoid = 5.0f + stats.Block - LevelModifier(character);
                    modifiedAvoid = (stats.BlockRating * ProtWarr.BlockRatingToBlock) + (defSkill * ProtWarr.DefenseToBlock);
                    break;
                case HitResult.Crit:
                    modifiedAvoid = (defSkill * ProtWarr.DefenseToCritReduction) + (stats.Resilience * ProtWarr.ResilienceRatingToCritReduction);
                    break;
            }

            // Many of the base values are whole numbers, so need to get it back to decimal. 
            // May want to look at making this more consistant in the future.
            return (baseAvoid + modifiedAvoid) / 100.0f;
        }
        
    }
}