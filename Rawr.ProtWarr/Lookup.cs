using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public static class Lookup
    {
        public static float TargetArmorReduction(Character character, Stats stats)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;

            // Needs Armor Penetration Support, but requires adding rating support into CalculationsProtWarrior
            return Math.Max(0.0f, Math.Min(0.75f, calcOpts.TargetArmor / (calcOpts.TargetArmor + (467.5f * character.Level - 22167.5f))));
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

        public static float LevelModifier(Character character)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            return (calcOpts.TargetLevel - 80f) * 0.2f;
        }

        public static float CritChance(Character character, Stats stats)
        {
            // Not sure about how LotP is being integrated in here vs. other buffs or buffed crit rating, look into cleaning this up
            return Math.Min(1.0f, (stats.PhysicalCrit + ((stats.CritRating + stats.LotPCritRating) * ProtWarr.CritRatingToCrit) +
                                    (stats.Agility * ProtWarr.AgilityToCrit) - LevelModifier(character)) / 100f);
        }

        public static float CritChance(Character character, Stats stats, Ability ability)
        {
            // Grab base melee crit chance before adding ability-specific crit chance
            float abilityCritChance = CritChance(character, stats);

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

        public static float AvoidanceChance(Character character, Stats stats, HitResult avoidanceType)
        {
            float defSkill      = (float)Math.Floor(stats.DefenseRating * ProtWarr.DefenseRatingToDefense);
            float baseAvoid     = 0.0f;
            float modifiedAvoid = 0.0f;

            switch (avoidanceType)
            {
                case HitResult.Dodge:
                    baseAvoid       = stats.Dodge + (stats.BaseAgility * ProtWarr.AgilityToDodge) - LevelModifier(character);
                    modifiedAvoid   = ((stats.Agility - stats.BaseAgility) * ProtWarr.AgilityToDodge) + 
                                        (stats.DodgeRating * ProtWarr.DodgeRatingToDodge) + (defSkill * ProtWarr.DefenseToDodge);
                    modifiedAvoid   = 1.0f / (1.0f / 88.129021f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Parry:
                    baseAvoid       = stats.Parry - LevelModifier(character);
                    modifiedAvoid   = (stats.ParryRating * ProtWarr.ParryRatingToParry) + (defSkill * ProtWarr.DefenseToParry);
                    modifiedAvoid   = 1.0f / (1.0f / 47.003525f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Miss:
                    baseAvoid       = stats.Miss * 100f - LevelModifier(character);
                    modifiedAvoid   = (defSkill * ProtWarr.DefenseToMiss);
                    modifiedAvoid   = 1.0f / (1.0f / 50.0f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Block:
                    // The 5% base block should be moved into stats.Block as a base value like the others
                    baseAvoid       = 5.0f + stats.Block - LevelModifier(character);
                    modifiedAvoid   = (stats.BlockRating * ProtWarr.BlockRatingToBlock) + (defSkill * ProtWarr.DefenseToBlock);
                    break;
                case HitResult.Crit:
                    modifiedAvoid   = (defSkill * ProtWarr.DefenseToCritReduction) + (stats.Resilience * ProtWarr.ResilienceRatingToCritReduction);
                    break;
            }

            // Many of the base values are whole numbers, so need to get it back to decimal. 
            // May want to look at making this more consistant in the future.
            return (baseAvoid + modifiedAvoid) / 100.0f;
        }

        public static float AttackTable(Character character, Stats stats, HitResult resultType)
        {
            // This calls AttackTable with no ability, generally used for white hits that can glance
            return AttackTable(character, stats, resultType, Ability.None);
        }

        public static float AttackTable(Character character, Stats stats, HitResult resultType, Ability ability)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            float hitBonus          = (stats.HitRating * ProtWarr.HitRatingToHit + stats.PhysicalHit) / 100f;
            float expertiseBonus    = (stats.ExpertiseRating * ProtWarr.ExpertiseRatingToExpertise + stats.Expertise) 
                                        * ProtWarr.ExpertiseToDodgeParryReduction / 100f;


            float chanceMiss    = 0.0f;
            float chanceDodge   = 0.0f;
            float chanceParry   = 0.0f;
            float chanceGlance  = 0.0f;
            float chanceCrit    = 0.0f;
            float chanceHit     = 0.0f;

            float tableSize     = 0.0f;
            float hitResult     = 0.0f;

            if ((calcOpts.TargetLevel - 80f) < 3)
                chanceMiss = Math.Min(1.0f - tableSize, Math.Max(0.0f, 0.05f + 0.005f * (calcOpts.TargetLevel - 80f) - hitBonus));
            else
                chanceMiss = Math.Min(1.0f - tableSize, Math.Max(0.0f, 0.08f - hitBonus));
            tableSize += chanceMiss;

            if (resultType != HitResult.Miss) // Skip the section below if not relevant
            {
                chanceDodge = Math.Min(1.0f - tableSize, Math.Max(0.0f, 0.064f - expertiseBonus));
                tableSize += chanceDodge;

                if (resultType != HitResult.Dodge) // Skip the section below if not relevant
                {
                    if ((calcOpts.TargetLevel - 80f) < 3)
                        chanceParry = Math.Min(1.0f - tableSize, Math.Max(0.0f, 0.065f - expertiseBonus));
                    else
                        chanceParry = Math.Min(1.0f - tableSize, Math.Max(0.0f, 0.1375f - expertiseBonus));
                    tableSize += chanceParry;

                    if (resultType != HitResult.Parry && resultType != HitResult.AnyMiss) // Skip the section below if not relevant
                    {
                        // Only white attacks can glance, so ignore it for ability hits
                        if (ability == Ability.None)
                        {
                            chanceGlance = Math.Min(1.0f - tableSize, Math.Max(0.0f, 0.1f + (calcOpts.TargetLevel - character.Level) * 0.05f));
                            tableSize += chanceGlance;
                        }

                        if (resultType != HitResult.Glance) // Skip the section below if not relevant
                        {
                            // Ability-specific crit chance has to be checked here
                            if (ability == Ability.None)
                                chanceCrit = Math.Min(1.0f - tableSize, CritChance(character, stats));
                            else
                                chanceCrit = Math.Min(1.0f - tableSize, CritChance(character, stats, ability));
                            tableSize += chanceCrit;

                            chanceHit = 1.0f - tableSize;
                        }
                    }
                }
            }

            switch (resultType)
            {
                case HitResult.AnyMiss:
                    hitResult = chanceMiss + chanceDodge + chanceParry;
                    break;
                case HitResult.AnyHit:
                    hitResult = 1.0f - (chanceMiss + chanceDodge + chanceParry);
                    break;
                case HitResult.Crit:
                    hitResult = chanceCrit;
                    break;
                case HitResult.Dodge:
                    hitResult = chanceDodge;
                    break;
                case HitResult.Miss:
                    hitResult = chanceMiss;
                    break;
                case HitResult.Parry:
                    hitResult = chanceParry;
                    break;
                case HitResult.Glance:
                    hitResult = chanceGlance;
                    break;
                case HitResult.Hit:
                    hitResult = chanceHit;
                    break;
            }

            return hitResult;
        }

        public static float DefendTable(Character character, Stats stats, HitResult resultType)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            
            float chanceMiss    = AvoidanceChance(character, stats, HitResult.Miss);
            float chanceDodge   = AvoidanceChance(character, stats, HitResult.Dodge);
            float chanceParry   = AvoidanceChance(character, stats, HitResult.Parry);
            float chanceBlock   = AvoidanceChance(character, stats, HitResult.Block);
            float chanceCrit    = Math.Max(0.0f, (5.0f + LevelModifier(character)) - AvoidanceChance(character, stats, HitResult.Crit));
            float chanceHit     = 0.0f;

            float tableSize = 0.0f;
            float hitResult = 0.0f;

            chanceMiss = Math.Min(1.0f - tableSize, chanceMiss);
            tableSize += chanceMiss;

            if (resultType != HitResult.Miss) // Skip the section below if not relevant
            {
                chanceDodge = Math.Min(1.0f - tableSize, chanceDodge);
                tableSize += chanceDodge;

                if (resultType != HitResult.Dodge) // Skip the section below if not relevant
                {
                    chanceParry = Math.Min(1.0f - tableSize, chanceParry);
                    tableSize += chanceParry;

                    if (resultType != HitResult.Parry && resultType != HitResult.AnyMiss) // Skip the section below if not relevant
                    {
                        chanceBlock = Math.Min(1.0f - tableSize, chanceBlock);
                        tableSize += chanceBlock;

                        if (resultType != HitResult.Block) // Skip the section below if not relevant
                        {
                            chanceCrit = Math.Min(1.0f - tableSize, chanceCrit); ;
                            tableSize += chanceCrit;

                            chanceHit = 1.0f - tableSize;
                        }
                    }
                }
            }

            switch (resultType)
            {
                case HitResult.AnyMiss:
                    hitResult = chanceMiss + chanceDodge + chanceParry;
                    break;
                case HitResult.AnyHit:
                    hitResult = 1.0f - (chanceMiss + chanceDodge + chanceParry);
                    break;
                case HitResult.Crit:
                    hitResult = chanceCrit;
                    break;
                case HitResult.Dodge:
                    hitResult = chanceDodge;
                    break;
                case HitResult.Miss:
                    hitResult = chanceMiss;
                    break;
                case HitResult.Parry:
                    hitResult = chanceParry;
                    break;
                case HitResult.Block:
                    hitResult = chanceBlock;
                    break;
                case HitResult.Hit:
                    hitResult = chanceHit;
                    break;
            }

            return hitResult;
        }

        public static float AbilityDamage(Character character, Stats stats, Ability ability)
        {
            CalculationOptionsProtWarr  calcOpts    = character.CalculationOptions as CalculationOptionsProtWarr;
            WarriorTalents              talents     = character.WarriorTalents;

            float   damageMultiplier    = StanceDamageMultipler(character, stats);
            float   baseDamage          = 0.0f;
            bool    useCrit             = true;
            bool    useArmor            = true;
            bool    useAvoidance        = true;

            switch (ability)
            {
                case Ability.Cleave:
                    baseDamage = WeaponDamage(character, stats, false, false) + (222.0f * (1.0f + talents.ImprovedCleave * 0.4f));
                    break;
                case Ability.ConcussionBlow:
                    baseDamage = stats.AttackPower * 0.75f;
                    break;
                case Ability.DamageShield:
                    baseDamage = (stats.BlockValue * (1.0f + stats.BonusBlockValueMultiplier)) * (talents.DamageShield * 0.1f);
                    useAvoidance = false;
                    break;
                case Ability.DeepWounds:
                    // Currently double-dips from multipliers on the base damage, although Blizz will probably fix soon
                    baseDamage = (WeaponDamage(character, stats, false, false) * damageMultiplier) * (talents.DeepWounds * 0.16f);
                    damageMultiplier *= (1.0f + stats.BonusBleedDamageMultiplier);
                    useArmor = false;
                    useCrit = false;
                    useAvoidance = false;
                    break;
                case Ability.Devastate:
                    // Assumes 5 stacks of Sunder Armor debuff
                    baseDamage = (WeaponDamage(character, stats, true, false) * 0.5f) + (101.0f * 5.0f);
                    break;
                case Ability.HeroicStrike:
                    baseDamage = WeaponDamage(character, stats, false, false) + 495.0f;
                    break;
                case Ability.HeroicThrow:
                    baseDamage = 12.0f + (stats.AttackPower * 0.5f);
                    break;
                case Ability.Rend:
                    baseDamage = 380.0f + WeaponDamage(character, stats, true, false);
                    damageMultiplier *= (1.0f + talents.ImprovedRend * 0.2f) * (1.0f + stats.BonusBleedDamageMultiplier);
                    useArmor = false;
                    useCrit = false;
                    break;
                case Ability.Revenge:
                    baseDamage = 1615.0f + (stats.AttackPower * 0.207f);
                    damageMultiplier *= (1.0f + talents.ImprovedRevenge * 0.1f);
                    break;
                case Ability.ShieldSlam:
                    baseDamage = stats.BlockValue * (1.0f + stats.BonusBlockValueMultiplier);
                    damageMultiplier *= (1.0f + talents.GagOrder * 0.05f);
                    break;
                case Ability.Shockwave:
                    baseDamage = stats.AttackPower * 0.75f;
                    useAvoidance = false;
                    break;
                case Ability.Slam:
                    baseDamage = WeaponDamage(character, stats, true, false) + 250.0f;
                    break;
                case Ability.ThunderClap:
                    baseDamage = 300.0f + (stats.AttackPower * 0.12f);
                    damageMultiplier *= (1.0f + talents.ImprovedThunderClap * 0.1f);
                    useAvoidance = false;
                    break;
            }

            // Not all abilities can crit
            if (useCrit)
            {
                float critChance = AttackTable(character, stats, HitResult.Crit, ability);
                baseDamage = (baseDamage * (1.0f - critChance)) + (baseDamage * (2.0f + (1.0f * stats.BonusCritMultiplier)) * critChance);
            }

            // Not all abilities will bypass armor
            if (useArmor)
                damageMultiplier *= (1.0f - TargetArmorReduction(character, stats));

            // Not all abilities can be dodged and parried
            if (useAvoidance)
                damageMultiplier *= AttackTable(character, stats, HitResult.AnyHit, ability);
            else
                damageMultiplier *= (1.0f - AttackTable(character, stats, HitResult.Miss, ability));

            return baseDamage * damageMultiplier;
        }

        public static float AbilityThreat(Character character, Stats stats, Ability ability)
        {
            // Base threat is always going to be the damage of the ability, if it is damaging
            float abilityThreat = AbilityDamage(character, stats, ability);

            switch (ability)
            {
                case Ability.Cleave:
                    abilityThreat += 225.0f; 
                    break;
                case Ability.Devastate:
                    abilityThreat += (stats.AttackPower * 0.05f); 
                    break;
                case Ability.HeroicStrike:
                    abilityThreat += 259.0f;
                    break;
                case Ability.HeroicThrow:
                    abilityThreat *= 1.5f;
                    break;
                case Ability.Revenge:
                    abilityThreat += 121.0f;
                    break;
                case Ability.ShieldBash:
                    abilityThreat += 36.0f;
                    break;
                case Ability.ShieldSlam:
                    abilityThreat += 770.0f;
                    break;
                case Ability.Slam:
                    abilityThreat += 140.0f;
                    break;
                case Ability.SunderArmor:
                    abilityThreat += 345.0f + (stats.AttackPower * 0.05f);
                    break;
                case Ability.ThunderClap:
                    abilityThreat *= 1.85f;
                    break;
            }

            abilityThreat *= StanceThreatMultipler(character, stats);

            return abilityThreat;
        }

        public static float WeaponDamage(Character character, Stats stats, bool normalized, bool canCrit)
        {
            //Assume a lvl 70 green dagger if the main hand is empty
            float weaponDamage      = 0.0f;
            float weaponSpeed       = 1.7f;
            float weaponMinDamage   = 77.0f;
            float weaponMaxDamage   = 144.0f;
            float normalizedSpeed   = 1.7f;

            if (character != null && character.MainHand != null)
            {
                weaponSpeed     = character.MainHand.Speed;
                weaponMinDamage = character.MainHand.MinDamage;
                weaponMaxDamage = character.MainHand.MaxDamage;
                if (character.MainHand.Type == Item.ItemType.Dagger)
                    normalizedSpeed = 1.7f;
                else
                    normalizedSpeed = 2.4f;
            }

            // Normal Non-Normalized Hits
            if (!normalized)
            {
                float attackSpeed = Math.Max(1.0f, weaponSpeed / (1.0f + (stats.HasteRating * ProtWarr.HasteRatingToHaste / 100.0f)));
                weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (attackSpeed * stats.AttackPower / 14.0f)) + stats.WeaponDamage;
            }
            // Normalized Hits
            else
                weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (normalizedSpeed * stats.AttackPower / 14.0f)) + stats.WeaponDamage;

            // Abilities that use weapon damage won't use crit on the base damage
            if (canCrit)
            {
                float critChance = AttackTable(character, stats, HitResult.Crit);
                weaponDamage = (weaponDamage * (1.0f - critChance)) + (weaponDamage * (2.0f + (1.0f * stats.BonusCritMultiplier)) * critChance);
            }

            return weaponDamage;
        }

        public static float WeaponDPS(Character character, Stats stats)
        {
            CalculationOptionsProtWarr calcOpts = character.CalculationOptions as CalculationOptionsProtWarr;
            
            float weaponSpeed = 0.0f;
            if (character.MainHand != null)
                weaponSpeed = character.MainHand.Speed;
            float attackSpeed = Math.Max(1.0f, weaponSpeed / (1.0f + (stats.HasteRating * ProtWarr.HasteRatingToHaste / 100.0f)));

            // Probably should pull this glancing stuff out into somewhere else
            float glanceMultiplier  = (Math.Min(0.91f, 1.3f - (0.05f * (calcOpts.TargetLevel - character.Level) * 5.0f)) +
                                        Math.Max(0.99f, 1.2f - (0.03f * (calcOpts.TargetLevel - character.Level) * 5.0f))) / 2;

            // Formula for this is:
            // [Weapon Damage] * [Damage Multipliers] * [Average Glancing Blow Reduction] * [Armor Reduction] * [Hit Rate]
            float averageDamage =   WeaponDamage(character, stats, false, true) 
                                    * StanceDamageMultipler(character, stats) 
                                    * (1.0f - (glanceMultiplier * AttackTable(character, stats, HitResult.Glance)))
                                    * (1.0f - TargetArmorReduction(character, stats))
                                    * AttackTable(character, stats, HitResult.AnyHit);

            return (averageDamage / attackSpeed);
        }
    }
}