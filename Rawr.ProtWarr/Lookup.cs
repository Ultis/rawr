using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public static class Lookup
    {
        public static float GlobalCooldownSpeed(bool withLatency)
        {
            float globalCooldownSpeed = 1.5f;
            
            if (withLatency)
                globalCooldownSpeed += 0.1f;

            return globalCooldownSpeed;
        }

        public static float TargetArmorReduction(Player player)
        {
            return StatConversion.GetArmorDamageReduction(player.Character.Level, player.Boss.Armor,
                player.Stats.TargetArmorReduction, 0.0f);
        }

        public static float TargetAvoidanceChance(Player player, HitResult avoidanceType)
        {
            switch (avoidanceType)
            {
                case HitResult.Miss:
                    return StatConversion.WHITE_MISS_CHANCE_CAP[player.Boss.Level - 85];
                case HitResult.Dodge:
                    return StatConversion.YELLOW_DODGE_CHANCE_CAP[player.Boss.Level - 85];
                case HitResult.Parry:
                    return StatConversion.YELLOW_PARRY_CHANCE_CAP[player.Boss.Level - 85];
                case HitResult.Glance:
                    return 0.06f + ((player.Boss.Level - player.Character.Level) * 0.06f);
                case HitResult.Crit:
                    return -StatConversion.NPC_LEVEL_CRIT_MOD[player.Boss.Level - player.Character.Level]; // StatConversion returns as negative
                default:
                    return 0.0f;
            }
        }

        public static float TargetCritChance(Player player)
        {
            return Math.Max(0.0f, 0.05f - AvoidanceChance(player, HitResult.Crit));
        }

        public static float TargetWeaponSpeed(Player player)
        {
            return player.Options.BossAttackSpeed * (1.0f - player.Stats.BossAttackSpeedMultiplier);
        }

        public static float StanceDamageMultipler(Player player)
        {
            // In Defensive Stance
            return (1.0f * (1.0f + player.Stats.BonusDamageMultiplier) * (1.0f + player.Stats.BonusPhysicalDamageMultiplier));
        }

        public static float StanceThreatMultipler(Player player)
        {
            // In Defensive Stance
            return (3.0f * (1.0f + player.Stats.ThreatIncreaseMultiplier));
        }

        public static float StanceDamageReduction(Player player)
        {
            return StanceDamageReduction(player, DamageType.Physical);
        }

        public static float StanceDamageReduction(Player player, DamageType damageType)
        {
            // In Defensive Stance
            float damageTaken = 0.9f * (1.0f + player.Stats.DamageTakenMultiplier);
            
            switch (damageType)
            {
                case DamageType.Arcane:
                case DamageType.Fire:
                case DamageType.Frost:
                case DamageType.Nature:
                case DamageType.Shadow:
                case DamageType.Holy:
                    return damageTaken;
                default:
                    return damageTaken;
            }
        }

        public static float BonusMasteryBlockPercentage(Player player)
        {
            return 0.015f * (8.0f + StatConversion.GetMasteryFromRating(player.Stats.MasteryRating, CharacterClass.Warrior));
        }

        public static float BonusExpertisePercentage(Player player)
        {
            return StatConversion.GetDodgeParryReducFromExpertise(player.Stats.Expertise + 
                StatConversion.GetExpertiseFromRating(player.Stats.ExpertiseRating), CharacterClass.Warrior);
        }

        public static float BonusHastePercentage(Player player)
        {
            return StatConversion.GetPhysicalHasteFromRating(player.Stats.HasteRating, CharacterClass.Warrior) + player.Stats.PhysicalHaste;
        }

        public static float BonusHitPercentage(Player player)
        {
            return StatConversion.GetPhysicalHitFromRating(player.Stats.HitRating, CharacterClass.Warrior) + player.Stats.PhysicalHit;
        }

        public static float BonusCritMultiplier(Player player, Ability ability)
        {
            return (2.0f * (1.0f + player.Stats.BonusCritMultiplier) - 1.0f);
        }

        public static float BonusCritPercentage(Player player)
        {
            return Math.Max(0.0f, Math.Min(1.0f, 
                StatConversion.GetPhysicalCritFromRating(player.Stats.CritRating, CharacterClass.Warrior) + 
                StatConversion.GetPhysicalCritFromAgility(player.Stats.Agility, CharacterClass.Warrior) +
                player.Stats.PhysicalCrit));
        }

        public static float BonusCritPercentage(Player player, Ability ability)
        {
            // Grab base melee crit chance before adding ability-specific crit chance
            float abilityCritChance = BonusCritPercentage(player);

            switch (ability)
            {
                case Ability.Devastate:
                    abilityCritChance += player.Stats.DevastateCritIncrease + (player.Talents.SwordAndBoard * 0.05f);
                    break;
                case Ability.HeroicStrike:
                    abilityCritChance += player.Talents.Incite * 0.05f;
                    break;
                case Ability.ShieldSlam:
                    abilityCritChance += player.Talents.Cruelty * 0.05f;
                    break;
                case Ability.DeepWounds:
                case Ability.Rend:
                case Ability.ShieldBash:
                case Ability.SunderArmor:
                    abilityCritChance = 0.0f;
                    break;
            }

            return Math.Min(1.0f, abilityCritChance);
        }

        public static float WeaponDamage(Player player, bool normalized)
        {
            float weaponDamage = 1.0f;

            if (player.Character.MainHand != null)
            {
                float weaponSpeed     = player.Character.MainHand.Speed;
                float weaponMinDamage = player.Character.MainHand.MinDamage;
                float weaponMaxDamage = player.Character.MainHand.MaxDamage;
                float normalizedSpeed = 1.0f;
                if (player.Character.MainHand.Type == ItemType.Dagger)
                    normalizedSpeed = 1.7f;
                else
                    normalizedSpeed = 2.4f;
            
                // Non-Normalized Hits
                if (!normalized)
                    weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (weaponSpeed * player.Stats.AttackPower / 14.0f)) + player.Stats.WeaponDamage;
                // Normalized Hits
                else
                    weaponDamage = ((weaponMinDamage + weaponMaxDamage) / 2.0f + (normalizedSpeed * player.Stats.AttackPower / 14.0f)) + player.Stats.WeaponDamage;
            }

            return weaponDamage;
        }

        public static float WeaponSpeed(Player player)
        {
            if (player.Character.MainHand != null)
                return Math.Max(1.0f, player.Character.MainHand.Speed / (1.0f + BonusHastePercentage(player)));
            else
                return 1.0f;
        }

        public static float GlancingReduction(Player player)
        {
            return (Math.Min(0.91f, 1.3f - (0.05f * (player.Boss.Level - player.Character.Level) * 5.0f)) +
                    Math.Max(0.99f, 1.2f - (0.03f * (player.Boss.Level - player.Character.Level) * 5.0f))) / 2;
        }

        public static float ArmorReduction(Player player)
        {
            return StatConversion.GetArmorDamageReduction(player.Boss.Level, player.Stats.Armor, 0f, 0f);
        }

        public static float MagicReduction(Player player, DamageType school)
        {
            float totalResist = 0.0f;
            switch (school)
            {
                case DamageType.Arcane: totalResist = player.Stats.ArcaneResistance; break;
                case DamageType.Fire: totalResist = player.Stats.FireResistance; break;
                case DamageType.Frost: totalResist = player.Stats.FrostResistance; break;
                case DamageType.Nature: totalResist = player.Stats.NatureResistance; break;
                case DamageType.Shadow: totalResist = player.Stats.ShadowResistance; break;
            }

            float damageReduction = Lookup.StanceDamageReduction(player, school);
            float averageResistance = StatConversion.GetAverageResistance(player.Boss.Level, player.Character.Level, totalResist, 0.0f);

            return Math.Max(0.0f, (1.0f - averageResistance) * damageReduction);
        }

        public static float AvoidanceChance(Player player, HitResult avoidanceType)
        {
            switch (avoidanceType)
            {
                case HitResult.Crit:
                    return StatConversion.GetDRAvoidanceChance(player.Character, player.Stats, avoidanceType, player.Boss.Level) + (player.Talents.BastionOfDefense * 0.03f);
                case HitResult.CritBlock:
                    return Lookup.BonusMasteryBlockPercentage(player) + player.Stats.CriticalBlock;
                default:
                    return Math.Max(0.0f, StatConversion.GetDRAvoidanceChance(player.Character, player.Stats, avoidanceType, player.Boss.Level));
            }
        }

        public static bool IsAvoidable(Ability ability)
        {
            switch (ability)
            {
                case Ability.DeepWounds:
                case Ability.Shockwave:
                case Ability.ThunderClap:
                    return false;
                default:
                    return true;
            }
        }

        public static bool IsWeaponAttack(Ability ability)
        {
            switch (ability)
            {
                case Ability.None:
                case Ability.Cleave:
                case Ability.ConcussionBlow:
                case Ability.Devastate:
                case Ability.HeroicStrike:
                case Ability.Revenge:
                case Ability.ShieldSlam:
                case Ability.Shockwave:
                case Ability.Slam:
                case Ability.VictoryRush:
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
                case Ability.Cleave: return "Cleave";
                case Ability.ConcussionBlow: return "Concussion Blow";
                case Ability.DeepWounds: return "Deep Wounds";
                case Ability.Devastate: return "Devastate";
                case Ability.HeroicStrike: return "Heroic Strike";
                case Ability.HeroicThrow: return "Heroic Throw";
                case Ability.Rend: return "Rend";
                case Ability.Revenge: return "Revenge";
                case Ability.ShieldBash: return "Shield Bash";
                case Ability.ShieldSlam: return "Shield Slam";
                case Ability.Shockwave: return "Shockwave";
                case Ability.Slam: return "Slam";
                case Ability.SunderArmor: return "Sunder Armor";
                case Ability.ThunderClap: return "Thunder Clap";
                case Ability.VictoryRush: return "Victory Rush";
                default: return "";
            }
        }
    }
}