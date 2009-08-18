using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public static class Lookup
    {
        
        public static float GetEffectiveTargetArmor(int AttackerLevel, float TargetArmor,
            float ArmorIgnoreDebuffs, float ArmorIgnoreBuffs, float ArmorPenetrationRating)
        {
            float ArmorConstant = 400 + 85 * AttackerLevel + 4.5f * 85 * (AttackerLevel - 59);
            TargetArmor *= (1f - ArmorIgnoreDebuffs) * (1f - ArmorIgnoreBuffs);
            float ArPCap = Math.Min((TargetArmor + ArmorConstant) / 3f, TargetArmor);
            float Amount = StatConversion.GetArmorPenetrationFromRating(ArmorPenetrationRating);
            TargetArmor -= ArPCap * Math.Min(1.0f, Amount);

            return TargetArmor;
        }
        
        public static float GetArmorPenetrationCap(int AttackerLevel, float TargetArmor,
            float ArmorIgnoreDebuffs, float ArmorIgnoreBuffs, float ArmorPenetrationRating)
        {
            float ArmorConstant = 400 + 85 * AttackerLevel + 4.5f * 85 * (AttackerLevel - 59);
            TargetArmor *= (1f - ArmorIgnoreDebuffs) * (1f - ArmorIgnoreBuffs);
            float ArPCap = Math.Min((TargetArmor + ArmorConstant) / 3f, TargetArmor);
            
            //float ArPCapRating = 100.0f * ProtPaladin.ArPToArmorPenetration; // 1231.62 rating = 100%

            return ArPCap;
        }
        
        /// <summary>
        /// A mob's or player's crit chance is modified by the difference between the attacker's Attack Rating 
        /// and the defender's Defense. The attack rating equals the skill with the currently equipped weapon 
        /// (WS = Weapon Skill), being level * 5 for mobs and the same for player chars with maximum weapon skill. 
        /// Each point of AR exceeding the target's Defense will increase chance to crit by 0.04%.
        /// </summary>
        /// <returns>Returns the modifier of chance for two combatants. (0.006 = 0.6%)</returns>
        public static float CombatRatingModifier(Character character, Stats stats)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            float AttackerAttackRating = calcOpts.TargetLevel * 5.0f;
            float DefenderDefenseSkill = stats.Defense;
            float Modifier = (AttackerAttackRating - DefenderDefenseSkill) * 0.0004f;
            return Modifier;
        }

        public static float TargetArmorReduction(Character character, Stats stats)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            int targetArmor = calcOpts.TargetArmor;
            float damageReduction = StatConversion.GetArmorDamageReduction(character.Level, targetArmor,
                stats.ArmorPenetration, 0f, stats.ArmorPenetrationRating); 
            return damageReduction;
        }

        public static float EffectiveTargetArmorReduction(Character character, Stats stats)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            float targetArmor = GetEffectiveTargetArmor(character.Level, calcOpts.TargetArmor, 0.0f, stats.ArmorPenetration, stats.ArmorPenetrationRating);
            float damageReduction = StatConversion.GetArmorDamageReduction(calcOpts.TargetLevel, targetArmor, 0f, 0f, 0f); 
            
            return damageReduction;
        }

        public static float TargetCritChance(Character character, Stats stats)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            return Math.Max(0.0f, (0.05f + CombatRatingModifier(character, stats)) - AvoidanceChance(character, stats, HitResult.Crit));
        }

        public static float TargetAvoidanceChance(Character character, Stats stats, HitResult avoidanceType)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            switch (avoidanceType)
            {

                case HitResult.Miss  : return StatConversion.WHITE_MISS_CHANCE_CAP[  calcOpts.TargetLevel - 80];
                case HitResult.Dodge : return StatConversion.WHITE_DODGE_CHANCE_CAP[ calcOpts.TargetLevel - 80];
                case HitResult.Parry : return StatConversion.WHITE_PARRY_CHANCE_CAP[ calcOpts.TargetLevel - 80];
                case HitResult.Glance: return StatConversion.WHITE_GLANCE_CHANCE_CAP[calcOpts.TargetLevel - 80];
                case HitResult.Block : return StatConversion.WHITE_BLOCK_CHANCE_CAP[ calcOpts.TargetLevel - 80];
                case HitResult.Resist:
                    // Patial resists don't belong in the combat table, they are a damage multiplier (reduction)
                    // The Chance to get any Partial Resist
                    float partialChance = 1.0f - StatConversion.GetResistanceTable(character.Level, calcOpts.TargetLevel, 0.0f, stats.SpellPenetration)[0];
                    return partialChance;
                default: return 0.0f;
            }
        }

        // Creature Type Damage Bonus from Crusade
        public static float CreatureTypeDamageMultiplier(Character character) {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            switch (calcOpts.TargetType) {
                case "Humanoid": case "Demon": case "Elemental": return (1f + character.PaladinTalents.Crusade * 0.01f);
                case "Undead": return (1f + character.PaladinTalents.Crusade * 0.01f) * (1f + (character.PaladinTalents.GlyphOfSenseUndead ? 0.01f : 0f));
                default: return 1f;
            }
        }

        public static float StanceDamageMultipler(Character character, Stats stats) { return (1.0f) * (1.0f + stats.BonusDamageMultiplier); }
        public static float StanceThreatMultipler(Character character, Stats stats) { return (1.42f * (1.0f + stats.ThreatIncreaseMultiplier)); }
        public static float StanceDamageReduction(Character character, Stats stats) { return StanceDamageReduction(character, stats, DamageType.Physical); }
        public static float StanceDamageReduction(Character character, Stats stats, DamageType damageType) {
            PaladinTalents talents = character.PaladinTalents;
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            float damageTaken = 1.0f * (1.0f + stats.DamageTakenMultiplier);
            //Talents
            damageTaken *= (1f - talents.ImprovedRighteousFury * 0.02f) * (1f - talents.ShieldOfTheTemplar * 0.01f);
            if (talents.GlyphOfDivinePlea) { damageTaken *= (1f - 0.03f); }
            			
            switch (damageType) {
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

        public static float BonusArmorPenetrationPercentage(Character character, Stats stats) { return StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating,CharacterClass.Paladin); }
        public static float BonusExpertisePercentage(Character character, Stats stats)        { return StatConversion.GetDodgeParryReducFromExpertise(StatConversion.GetExpertiseFromRating(stats.ExpertiseRating,CharacterClass.Paladin) + stats.Expertise,CharacterClass.Paladin); }
        public static float BonusPhysicalHastePercentage(Character character, Stats stats)    { return StatConversion.GetHasteFromRating(stats.HasteRating,CharacterClass.Paladin) + stats.PhysicalHaste; }
        public static float BonusSpellHastePercentage(Character character, Stats stats)       { return StatConversion.GetSpellHasteFromRating(stats.HasteRating,CharacterClass.Paladin) + stats.SpellHaste; }

        public static float HitChance(Character character, Stats stats) {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            float physicalHit = StatConversion.GetPhysicalHitFromRating(stats.HitRating,CharacterClass.Paladin) + stats.PhysicalHit;
            return Math.Min(1f, (1f - StatConversion.WHITE_MISS_CHANCE_CAP[calcOpts.TargetLevel-80]) + physicalHit);
        }

        public static float SpellHitChance(Character character, Stats stats) {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            float spellHit = StatConversion.GetSpellHitFromRating(stats.HitRating, CharacterClass.Paladin) + stats.SpellHit;

            int DeltaLevel = calcOpts.TargetLevel - character.Level;

            if      (DeltaLevel == 3) return Math.Min(1f, 0.83f + spellHit); // 83% chance to hit
            else if (DeltaLevel == 2) return Math.Min(1f, 0.94f + spellHit); // 94% chance to hit
            else if (DeltaLevel == 1) return Math.Min(1f, 0.95f + spellHit); // 95% chance to hit
            else                      return Math.Min(1f, 0.96f + spellHit); // 96% chance to hit
        }

        public static float CritChance(Character character, Stats stats) {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

            return Math.Max(0f,Math.Min(1f,StatConversion.GetCritFromRating(stats.CritRating,CharacterClass.Paladin)
                                           + StatConversion.GetCritFromAgility(stats.Agility,CharacterClass.Paladin)
                                           + StatConversion.NPC_LEVEL_CRIT_MOD[calcOpts.TargetLevel-80]
                                           + stats.PhysicalCrit));
        }

        public static float SpellCritChance(Character character, Stats stats) {
            //CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            float spellCrit = Math.Min(1f, StatConversion.GetSpellCritFromRating(stats.CritRating,CharacterClass.Paladin) + StatConversion.GetSpellCritFromIntellect(stats.Intellect,CharacterClass.Paladin) + stats.SpellCrit);
            
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
            } else {
                weaponSpeed     = character.MainHand.Speed;
                weaponMinDamage = character.MainHand.MinDamage;
                weaponMaxDamage = character.MainHand.MaxDamage;
                
                if (character.MainHand.Type == ItemType.Dagger)
                    normalizedSpeed = 1.7f;
            }
            // Non-Normalized Hits
            if (!normalized)
                weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (weaponSpeed * stats.AttackPower / 14.0f));
            // Normalized Hits
            else
                weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (normalizedSpeed * stats.AttackPower / 14.0f));

            return weaponDamage;
        }

        public static float WeaponSpeed(Character character, Stats stats) {
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
            
            return (lowEnd + highEnd) / 2.0f;
        }

        public static float ArmorReduction(Character character, Stats stats) // incoming damage
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            return Math.Max(0.0f, Math.Min(0.75f, stats.Armor / (stats.Armor + (467.5f * calcOpts.TargetLevel - 22167.5f))));
        }

        public static float ActiveBlockReduction(Character character, Stats stats)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            PaladinTalents talents = character.PaladinTalents;
            
            // This formula assumes judging on cd, and needs to be modified as soon as we support custom rotations.
            return (stats.BlockValue + stats.JudgementBlockValue + stats.ShieldOfRighteousnessBlockValue);
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

            if      ((calcOpts.TargetLevel - character.Level) == 0) resistScale = 400.0f;
            else if ((calcOpts.TargetLevel - character.Level) == 1) resistScale = 405.0f;
            else if ((calcOpts.TargetLevel - character.Level) == 2) resistScale = 410.0f;
            else
                // This number is still being tested by many and may be slightly higher
                // update: it seems 510 is a more realistic value
                // NB: resistScale is the resistance constant for 50% Mean Average Damage depending on Level difference.
                resistScale = 510.0f;

            return Math.Max(0.0f, (1.0f - (totalResist / (resistScale + totalResist))) * damageReduction);
        }

        public static float AvoidanceChance(Character character, Stats stats, HitResult avoidanceType)
        {
            CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;
            return StatConversion.GetDRAvoidanceChance(character,stats,avoidanceType,calcOpts.TargetLevel);
            
            /*float defSkillFomRating = (float)Math.Floor(StatConversion.GetDefenseFromRating(stats.DefenseRating,CharacterClass.Paladin));
            float baseAvoid = 0.0f;
            float modifiedAvoid = 0.0f;
            float modifier = CombatRatingModifier(character, stats);

            switch (avoidanceType)
            {
                case HitResult.Dodge:
                    baseAvoid = stats.Dodge + StatConversion.GetDodgeFromAgility(stats.BaseAgility,CharacterClass.Paladin) - modifier;
                    modifiedAvoid = StatConversion.GetDodgeFromAgility(stats.Agility - stats.BaseAgility,CharacterClass.Paladin)
                                    + StatConversion.GetDodgeFromRating(stats.DodgeRating,CharacterClass.Paladin)
                                    + StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER * defSkillFomRating;
                    modifiedAvoid *= 100.0f;
                    modifiedAvoid = 0.01f / (1.0f / 88.1290208866f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Parry:
                    baseAvoid = stats.Parry - modifier;
                    modifiedAvoid = StatConversion.GetParryFromRating(stats.ParryRating,CharacterClass.Paladin)
                                    + StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER * defSkillFomRating;
                    modifiedAvoid *= 100.0f;
                    modifiedAvoid = 0.01f / (1.0f / 47.003525644f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Miss:
                    baseAvoid = stats.Miss - modifier;
                    modifiedAvoid = StatConversion.Get(defSkillFomRating * ProtPaladin.DefenseToMiss,CharacterClass.Paladin);
                    modifiedAvoid *= 100.0f;
                    modifiedAvoid = 0.01f / (1.0f / 16.0f + 0.9560f / modifiedAvoid);
                    break;
                case HitResult.Block:
                    baseAvoid = stats.Block - modifier;
                    modifiedAvoid = StatConversion.(stats.BlockRating * ProtPaladin.BlockRatingToBlock,CharacterClass.Paladin)
                                    + StatConversion.DEFENSE_RATING_AVOIDANCE_MULTIPLIER * defSkillFomRating;
                    break;
                case HitResult.Crit:
                    modifiedAvoid = StatConversion.(defSkillFomRating * ProtPaladin.DefenseToCritReduction,CharacterClass.Paladin)
                                    + StatConversion.(stats.Resilience * ProtPaladin.ResilienceRatingToCritReduction,CharacterClass.Paladin);
                    break;
            }
            float avoidanceChance = baseAvoid + modifiedAvoid;
            return avoidanceChance;*/
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
