using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public static class Lookup
    {
        public static float LevelModifier(Character character)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            return (calcOpts.TargetLevel - character.Level) * 0.2f;
        }

        public static float TargetArmorReduction(Character character, Stats stats)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
			int targetArmor = calcOpts.TargetArmor;
			float damageReduction = ArmorCalculations.GetDamageReduction(character.Level, targetArmor,
				stats.ArmorPenetration, stats.ArmorPenetrationRating); 
			return damageReduction;
        }

        public static float TargetCritChance(Character character, Stats stats)
        {
            return Math.Max(0.0f, ((5.0f + Lookup.LevelModifier(character)) / 100.0f) - AvoidanceChance(character, stats, HitResult.Crit));
        }

        public static float TargetAvoidanceChance(Character character, Stats stats, HitResult avoidanceType)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            switch (avoidanceType)
            {

                case HitResult.Miss:
            
                    // Assuming 8.20% miss chance against a lvl 83 target.
                    if ((calcOpts.TargetLevel - character.Level) == 3)
                        return 0.082f;
                    if ((calcOpts.TargetLevel - character.Level) == 2)
                        return 0.054f;
                    if ((calcOpts.TargetLevel - character.Level) == 1)
                        return 0.052f;
                    else
                        return 0.05f;

                case HitResult.Dodge:
                    return 0.065f;

                case HitResult.Parry:
                    if ((calcOpts.TargetLevel - character.Level) < 3)
                        return 0.065f;
                    else
                        return 0.14f;

                case HitResult.Glance:
                    return 0.06f + ((calcOpts.TargetLevel - character.Level) * 0.06f);

                case HitResult.Block:
                    // Assuming 6.50% blocked attacks against a lvl 83 target.
                    if ((calcOpts.TargetLevel - character.Level) == 3)
                        return 0.065f;
                    if ((calcOpts.TargetLevel - character.Level) == 2)
                        return 0.054f;
                    if ((calcOpts.TargetLevel - character.Level) == 1)
                        return 0.052f;
                    else
                        return 0.05f;

                case HitResult.Resist:
                    return 0.06f + ((calcOpts.TargetLevel - character.Level) * 0.06f);//TODO: Find correct value for % chance to get partial resists.

                default:
                    return 0.0f;
            }
        }

        // Creature Type Damage Bonus from Crusade
        public static float CreatureTypeDamageMultiplier(Character character)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            switch (calcOpts.TargetType)
            {
                case "Humanoid":
                case "Demon":
                case "Elemental":
                    return (1f + character.PaladinTalents.Crusade * 0.01f);
                case "Undead":
                    return (1f + character.PaladinTalents.Crusade * 0.01f) * (1f + (character.PaladinTalents.GlyphOfSenseUndead ? 0.01f : 0f));
                default: return 1f;
            }
        }

        public static float StanceDamageMultipler(Character character, Stats stats)
        {
            return (1.0f) * (1.0f + stats.BonusDamageMultiplier);
        }

        public static float StanceThreatMultipler(Character character, Stats stats)
        {
            return (1.42f * (1.0f + stats.ThreatIncreaseMultiplier));
        }

        public static float StanceDamageReduction(Character character, Stats stats)
        {
            return StanceDamageReduction(character, stats, DamageType.Physical);
        }

        public static float StanceDamageReduction(Character character, Stats stats, DamageType damageType)
        {
            PaladinTalents talents = character.PaladinTalents;
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            float damageTaken = 1.0f * (1.0f + stats.DamageTakenMultiplier);
            //Talents
            damageTaken *= (1f - talents.ImprovedRighteousFury * 0.02f) * (1f - talents.ShieldOfTheTemplar * 0.01f);
            if (talents.GlyphOfDivinePlea)
            {
                damageTaken *= (1f - 0.03f);
            }
            			
            switch (damageType)
            {
                case DamageType.Arcane:
                case DamageType.Fire:
                case DamageType.Frost:
                case DamageType.Nature:
                case DamageType.Shadow:
                case DamageType.Holy:
                    return damageTaken * (1.0f - talents.GuardedByTheLight * 0.03f);
                default:
                    return damageTaken;

            }
        }

        public static float BonusArmorPenetrationPercentage(Character character, Stats stats)
        {
            return ((stats.ArmorPenetrationRating * ProtPaladin.ArPToArmorPenetration) / 100.0f);
        }

        public static float BonusExpertisePercentage(Character character, Stats stats)
        {
        	return ((stats.ExpertiseRating * ProtPaladin.ExpertiseRatingToExpertise + stats.Expertise)
                    * ProtPaladin.ExpertiseToDodgeParryReduction) / 100.0f;
        }

        public static float BonusPhysicalHastePercentage(Character character, Stats stats)
        {
            return ((stats.HasteRating * ProtPaladin.HasteRatingToPhysicalHaste) / 100.0f) + stats.PhysicalHaste;
        }

        public static float BonusSpellHastePercentage(Character character, Stats stats)
        {
            return ((stats.HasteRating * ProtPaladin.HasteRatingToSpellHaste) / 100.0f) + stats.SpellHaste;
        }


        public static float HitChance(Character character, Stats stats)
        {
            float physicalHit = ((stats.HitRating * ProtPaladin.HitRatingToHit) / 100.0f) + stats.PhysicalHit;
            
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            
            // Assuming 8.20% miss chance against a lvl 83 target.
            if ((calcOpts.TargetLevel - character.Level) == 3)    // 91.80% chance to hit
                return Math.Min(1.0f, 0.9180f + physicalHit);
            if ((calcOpts.TargetLevel - character.Level) == 2)    // 94.60% chance to hit
                return Math.Min(1.0f, 0.9460f + physicalHit);
            if ((calcOpts.TargetLevel - character.Level) == 1)    // 94.80% chance to hit
                return Math.Min(1.0f, 0.9480f + physicalHit);
            else                                                  // 95% chance to hit
                return Math.Min(1.0f, 0.95f + physicalHit);
        }

        public static float SpellHitChance(Character character, Stats stats)
        {
            float spellHit = ((stats.HitRating * ProtPaladin.HitRatingToSpellHit) / 100.0f) + stats.SpellHit;
            
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            
            if ((calcOpts.TargetLevel - character.Level) == 3)    // 83% chance to hit
                return Math.Min(1.0f, 0.83f + spellHit);
            if ((calcOpts.TargetLevel - character.Level) == 2)    // 94% chance to hit
                return Math.Min(1.0f, 0.94f + spellHit);
            if ((calcOpts.TargetLevel - character.Level) == 1)    // 95% chance to hit
                return Math.Min(1.0f, 0.95f + spellHit);
            else                                                  // 96% chance to hit
                return Math.Min(1.0f, 0.96f + spellHit);
        }

        public static float CritChance(Character character, Stats stats)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            if ((calcOpts.TargetLevel - character.Level) < 3)
                // This formula may or may not be accurate anymore, as the modifier on +1/2 mobs is untested
                return Math.Min(1.0f, (((stats.CritRating * ProtPaladin.CritRatingToCrit) + (stats.Agility * ProtPaladin.AgilityToCrit)
                                    - LevelModifier(character)) / 100.0f) + stats.PhysicalCrit);
            else
                // 4.8% chance to crit reduction as tested on bosses
                return Math.Min(1.0f, (((stats.CritRating * ProtPaladin.CritRatingToCrit) + (stats.Agility * ProtPaladin.AgilityToCrit)
                                    - 4.8f) / 100.0f) + stats.PhysicalCrit);
        }

        public static float SpellCritChance(Character character, Stats stats)
        {
            //CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            float spellCrit = Math.Min(1.0f, (((stats.CritRating * ProtPaladin.CritRatingToCrit) + (stats.Intellect * ProtPaladin.IntellectToCrit)) / 100.0f) + stats.SpellCrit);
            
            /*
             * Unlike the melee combat system, spell crit makes absolutely no difference to hit chance. 
             * All spells, regardless of whether they are treated as binary or not, roll hit and crit separately. 
             * Conceptually, the game rolls for your hit chance first, and if the spell hits you have a separate roll for whether it crits. 
             * Overall chance to crit over all spells cast is thus affected by hit rate. 
             * To calculate overall crit rate, multiplying the two chances together: 
             * Crit rate over all spell casts = crit * hit
             * 
             * For example, a caster with no spell hit rating gear or talents, 
             * against a mob 3 levels higher (83% hit chance), and 30% crit rating from gear and talents: 
             * crit rate over all spell casts = 30% * 83% = 24.9%
             * 
             * A level 80 player against a level 83 boss needs +26.232*k hit rating, to achieve +k% chance to hit with spells.
             * In addition, direct damage spells suffer from partial resistance, but again, that has no effect on whether a spell hits or not.
             */

            return spellCrit * SpellHitChance(character, stats); //moved target level dependency to spell hit chance
        }
        
        public static float BonusCritPercentage(Character character, Stats stats, Ability ability)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            // Grab base melee crit chance before adding ability-specific crit chance
            float abilityCritChance = CritChance(character, stats);
            float spellCritChance = SpellCritChance(character, stats);
            
            switch (ability)
            {                
                case Ability.Consecration:
                case Ability.HolyShield:
                case Ability.HolyVengeance:
                case Ability.SealOfRighteousness:
                case Ability.RetributionAura:
            	case Ability.RighteousDefense:
                    abilityCritChance = 0.0f;// can't crit
                    break;
                case Ability.None:
                case Ability.JudgementOfRighteousness:
                case Ability.JudgementOfVengeance:
                case Ability.AvengersShield:
                case Ability.HammerOfTheRighteous:
                case Ability.ShieldOfRighteousness:
                case Ability.HammerOfWrath:
                    abilityCritChance *= 1.0f;// crit chance = melee
                    break;
                case Ability.HolyWrath:
                case Ability.SealOfVengeance:
                case Ability.HandOfReckoning:
                case Ability.Exorcism:
                    if (calcOpts.TargetType == "Undead" || calcOpts.TargetType == "Demon")
                    {
                        // 100% chance the spell will crit, if it hits.
                        abilityCritChance = SpellHitChance(character, stats);
                        break;
                    }
                    abilityCritChance = spellCritChance;// crit chance = spell
                    break;
            }
            return Math.Min(1.0f, abilityCritChance);
        }

        public static float WeaponDamage(Character character, Stats stats, bool normalized)
        {
            float weaponDamage = 0.0f;
            float normalizedSpeed = 2.4f;
            float weaponSpeed     = 0.0f;
            float weaponMinDamage = 0.0f;
            float weaponMaxDamage = 0.0f;
            
            if (character.MainHand == null) // unarmed
            {
                weaponSpeed     = 2.0f;
                weaponMinDamage = 1.0f;
                weaponMaxDamage = 2.0f;
            }
            else
            {
                weaponSpeed     = character.MainHand.Speed;
                weaponMinDamage = character.MainHand.MinDamage;
                weaponMaxDamage = character.MainHand.MaxDamage;
                
                if (character.MainHand.Type == Item.ItemType.Dagger)
                    normalizedSpeed = 1.7f;
            }
            // Non-Normalized Hits
            if (!normalized)
                weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (weaponSpeed * stats.AttackPower / 14.0f));// + stats.WeaponDamage;
            // Normalized Hits
            else
                weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (normalizedSpeed * stats.AttackPower / 14.0f));// + stats.WeaponDamage;

            return weaponDamage;
        }

        public static float WeaponSpeed(Character character, Stats stats)
        {
            if (character.MainHand != null)
                return Math.Max(1.0f, character.MainHand.Speed / (1.0f + BonusPhysicalHastePercentage(character, stats)));
            else
                return Math.Max(1.0f, 2.0f/ (1.0f + BonusPhysicalHastePercentage(character, stats)));
        }

        public static float GlancingReduction(Character character)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            // The character is a melee class, lowEnd is element of [0.01, 0.91]
            float lowEnd = Math.Max(0.01f, Math.Min(0.91f, 1.3f - (0.05f * (float)(calcOpts.TargetLevel - character.Level) * 5.0f)));
            // The character is a melee class, highEnd is element of [0.20, 0.99]
            float highEnd = Math.Max(0.20f, Math.Min(0.99f, 1.2f - (0.03f * (float)(calcOpts.TargetLevel - character.Level) * 5.0f)));
            
            return (lowEnd + highEnd) / 2;
        }

        public static float ArmorReduction(Character character, Stats stats)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            return Math.Max(0.0f, Math.Min(0.75f, stats.Armor / (stats.Armor + (467.5f * calcOpts.TargetLevel - 22167.5f))));
        }

        public static float BlockReduction(Character character, Stats stats)
        {
            if (stats.JudgementBlockValue > 0) return stats.BlockValue + 5f / 9f * stats.JudgementBlockValue;
            else return stats.BlockValue;
        }

        public static float MagicReduction(Character character, Stats stats, DamageType school)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            float damageReduction = Lookup.StanceDamageReduction(character, stats, school);
            float totalResist = stats.AllResist;
            float resistScale = 0.0f;

            switch (school)
            {
                case DamageType.Arcane: totalResist += stats.ArcaneResistance; break;
                case DamageType.Fire: totalResist += stats.FireResistance; break;
                case DamageType.Frost: totalResist += stats.FrostResistance; break;
                case DamageType.Nature: totalResist += stats.NatureResistance; break;
                case DamageType.Shadow: totalResist += stats.ShadowResistance; break;
            }

            if ((calcOpts.TargetLevel - character.Level) < 3)
                resistScale = 400.0f;
            else
                // This number is still being tested by many and may be slightly higher
                resistScale = 500.0f;

            return Math.Max(0.0f, (1.0f - (totalResist / (resistScale + totalResist))) * damageReduction);
        }

        public static float AvoidanceChance(Character character, Stats stats, HitResult avoidanceType)
        {
            float defSkill = (float)Math.Floor(stats.DefenseRating * ProtPaladin.DefenseRatingToDefense);
            float baseAvoid = 0.0f;
            float modifiedAvoid = 0.0f;

            switch (avoidanceType)
            {
                case HitResult.Dodge:
                    baseAvoid = stats.Dodge + (stats.BaseAgility * ProtPaladin.AgilityToDodge) - LevelModifier(character);
                    modifiedAvoid = ((stats.Agility - stats.BaseAgility) * ProtPaladin.AgilityToDodge) +
                                        (stats.DodgeRating * ProtPaladin.DodgeRatingToDodge) + (defSkill * ProtPaladin.DefenseToDodge);
                    modifiedAvoid = 1.0f / (1.0f / 88.1290208866f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Parry:
                    baseAvoid = stats.Parry - LevelModifier(character);
                    modifiedAvoid = (stats.ParryRating * ProtPaladin.ParryRatingToParry) + (defSkill * ProtPaladin.DefenseToParry);
                    modifiedAvoid = 1.0f / (1.0f / 47.003525644f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Miss:
                    baseAvoid = stats.Miss * 100f - LevelModifier(character);
                    modifiedAvoid = (defSkill * ProtPaladin.DefenseToMiss);
                    modifiedAvoid = 1.0f / (1.0f / 16.0f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Block:
                    baseAvoid = stats.Block - LevelModifier(character);
                    modifiedAvoid = (stats.BlockRating * ProtPaladin.BlockRatingToBlock) + (defSkill * ProtPaladin.DefenseToBlock);
                    break;
                case HitResult.Crit:
                    modifiedAvoid = (defSkill * ProtPaladin.DefenseToCritReduction) + (stats.Resilience * ProtPaladin.ResilienceRatingToCritReduction);
                    break;
            }

            // Many of the base values are whole numbers, so need to get it back to decimal. 
            // May want to look at making this more consistant in the future.
            return (baseAvoid + modifiedAvoid) / 100.0f;
        }

        // Combination nCk
        public static float NComb(float n, float k)
        {
            float result = 1;

            for (float i = Math.Max(k,n-k) + 1; i <= n; ++i)
                result *= i;

            for (float i = 2; i <= Math.Min(k,n-k); ++i)
                result /= i;

            return result;
        }

        public static float GetConsecrationTickChances(float Ticks, float TickDamage, float Miss)
        {                                      // 10+0 ticks
            float[] ConsecrationTable = new float[11];// Debug Array
            float p = 1.0f - Miss;
            int n = (int)Math.Floor(Ticks); // Number of possible ticks, backwards compatible to Ticks as time.
            int k;
            float Damage = 0.0f;

            for (k = n; k > -1 ; k--)
            {   // The probability P(X=k) that k out of n possible ticks hit :
                float ProbabilityOfTicks = NComb(n, k) * (float)Math.Pow(p, k) * (float)Math.Pow((1 - p), (n-k));
                // The damage those ticks deal
                Damage += TickDamage * ProbabilityOfTicks * k;
                
                ConsecrationTable[k] += ProbabilityOfTicks;// Debug Array
            }
            // Total average damage over all probabilities
            return Damage;
        }

        public static bool IsAvoidable(Ability ability)
        {
            switch (ability)
            {
                case Ability.None:
                case Ability.HammerOfTheRighteous:
              //case Ability.SealOfCommand:
              //case Ability.SealOfBlood:
                    return true;
                default:
                    return false;
            }
        }

        public static bool HasPartials(Ability ability)
        {   
            switch (ability)
            {
                case Ability.ShieldOfRighteousness:
                case Ability.HammerOfTheRighteous:
                case Ability.SealOfVengeance: 
                case Ability.HolyVengeance:
                case Ability.JudgementOfVengeance:
                case Ability.SealOfRighteousness:
                case Ability.JudgementOfRighteousness:
                case Ability.Exorcism:
                case Ability.HammerOfWrath:
                case Ability.AvengersShield:
                case Ability.HolyShield:
                case Ability.RetributionAura:
                case Ability.Consecration:
            	case Ability.HolyWrath:
                    return true;
                default:
                return false;
            }
        }

        public static bool CanCrit(Ability ability)
        {   
            switch (ability)
            {
                case Ability.Consecration:
                case Ability.HolyShield:
                case Ability.HolyVengeance:
                case Ability.SealOfRighteousness:
                case Ability.RetributionAura:
                case Ability.RighteousDefense:
                    return false;
                default:
                return true;
            }
        }

        public static bool IsSpell(Ability ability)
        {   
            switch (ability)
            {
                case Ability.SealOfVengeance: 
                case Ability.HolyVengeance:
                case Ability.Exorcism:
            	case Ability.HolyWrath:
                case Ability.HolyShield:
                case Ability.RetributionAura:
                case Ability.Consecration:
                case Ability.HandOfReckoning:
                case Ability.RighteousDefense:
                //case Ability.SealOfRighteousness:
                    return true;
                default:
                return false;
            }
        }

        public static string Name(Ability ability)
        {
            switch (ability)
            {
                case Ability.None: return "Swing";
                case Ability.HolyWrath: return "Holy Wrath";
                case Ability.HandOfReckoning: return "Hand of Reckoning";
                case Ability.ShieldOfRighteousness: return "Shield of Righteousness";
                case Ability.HammerOfTheRighteous: return "Hammer of the Righteous";
                case Ability.SealOfVengeance: return "Seal of Vengeance";
                case Ability.HolyVengeance: return "Holy Vengeance";
                case Ability.JudgementOfVengeance: return "Judgement of Vengeance";
                case Ability.SealOfRighteousness: return "Seal of Righteousness";
                case Ability.JudgementOfRighteousness: return "Judgement of Righteousness";
                case Ability.Exorcism: return "Exorcism";
                case Ability.HammerOfWrath: return "Hammer of Wrath";
                case Ability.AvengersShield: return "Avenger's Shield";
                case Ability.HolyShield: return "Holy Shield";
                case Ability.RetributionAura: return "Retribution Aura";
                case Ability.Consecration: return "Consecration";
                default: return "";
            }
        }
    }
}