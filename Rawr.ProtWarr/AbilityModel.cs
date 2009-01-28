using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class AbilityModel
    {
        private Ability Ability;
        private Character Character;
        private Stats Stats;
        private WarriorTalents Talents;
        private CalculationOptionsProtWarr Options;

        public readonly AttackTable AttackTable;

        private string _name;
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        private float _damage;
        public float Damage
        {
            get { return _damage; }
            private set { _damage = value; }
        }

        private float _threat;
        public float Threat
        {
            get { return _threat; }
            private set { _threat = value; }
        }

        private float _damageMultiplier;
        public float DamageMultiplier
        {
            get { return _damageMultiplier; }
            private set { _damageMultiplier = value; }
        }

        private float _armorReduction;
        public float ArmorReduction
        {
            get { return _armorReduction; }
            private set { _armorReduction = value; }
        }

        public float CritPercentage
        {
            get { return AttackTable.Critical; }
        }


        private void CalculateDamage()
        {
            float baseDamage        = 0.0f;
            float critMultiplier    = 1.0f;

            if (Ability == Ability.None)
                critMultiplier = (2.0f * (1.0f + Stats.BonusCritMultiplier));
            else
                critMultiplier = ((2.0f * (1.0f + Stats.BonusCritMultiplier)) * (1.0f + (Talents.Impale * 0.1f)));

            switch (Ability)
            {
                // White Damage
                case Ability.None:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false);
                    break;
                case Ability.Cleave:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false) + (222.0f * (1.0f + Talents.ImprovedCleave * 0.4f));
                    break;
                case Ability.ConcussionBlow:
                    baseDamage = Stats.AttackPower * 0.75f;
                    break;
                case Ability.DamageShield:
                    baseDamage = Stats.BlockValue * (Talents.DamageShield * 0.1f);
                    break;
                case Ability.DeepWounds:
                    // Currently double-dips from multipliers on the base damage, although Blizz will probably fix soon
                    baseDamage = (Lookup.WeaponDamage(Character, Stats, false) * DamageMultiplier) * (Talents.DeepWounds * 0.16f);
                    DamageMultiplier *= (1.0f + Stats.BonusBleedDamageMultiplier);
                    ArmorReduction = 0.0f;
                    break;
                case Ability.Devastate:
                    // Assumes 5 stacks of Sunder Armor debuff
                    baseDamage = (Lookup.WeaponDamage(Character, Stats, true) * 0.5f) + (101.0f * 5.0f);
                    break;
                case Ability.HeroicStrike:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false) + 495.0f;
                    break;
                case Ability.HeroicThrow:
                    baseDamage = 12.0f + (Stats.AttackPower * 0.5f);
                    break;
                case Ability.Rend:
                    baseDamage = 380.0f + Lookup.WeaponDamage(Character, Stats, true);
                    DamageMultiplier *= (1.0f + Talents.ImprovedRend * 0.2f) * (1.0f + Stats.BonusBleedDamageMultiplier);
                    ArmorReduction = 0.0f;
                    break;
                case Ability.Revenge:
                    baseDamage = 1615.0f + (Stats.AttackPower * 0.207f);
                    DamageMultiplier *= (1.0f + Talents.ImprovedRevenge * 0.1f);
                    break;
                case Ability.ShieldSlam:
                    baseDamage = 1015.0f + Stats.BlockValue;
                    DamageMultiplier *= (1.0f + Stats.BonusShieldSlamDamage);
                    break;
                case Ability.Shockwave:
                    baseDamage = Stats.AttackPower * 0.75f;
                    break;
                case Ability.Slam:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false) + 250.0f;
                    break;
                case Ability.ThunderClap:
                    baseDamage = 300.0f + (Stats.AttackPower * 0.12f);
                    DamageMultiplier *= (1.0f + Talents.ImprovedThunderClap * 0.1f);
                    break;
            }

            // All damage multipliers
            baseDamage *= DamageMultiplier;
            // Average critical strike bonuses
            baseDamage = (baseDamage * (1.0f - AttackTable.Critical)) + (baseDamage * critMultiplier * AttackTable.Critical);
            // Average glancing blow reduction
            baseDamage *= (1.0f - (Lookup.GlancingReduction(Character) * AttackTable.Glance));
            // Armor reduction
            baseDamage *= (1.0f - ArmorReduction);
            // Missed attacks
            baseDamage *= (1.0f - AttackTable.AnyMiss);

            Damage = baseDamage;
        }

        private void CalculateThreat()
        {
            // Base threat is always going to be the damage of the ability, if it is damaging
            float abilityThreat = Damage;

            switch (Ability)
            {
                case Ability.Cleave:
                    abilityThreat += 225.0f;
                    break;
                case Ability.Devastate:
                    abilityThreat += (Stats.AttackPower * 0.05f);
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
                    abilityThreat += 345.0f + (Stats.AttackPower * 0.05f);
                    break;
                case Ability.ThunderClap:
                    abilityThreat *= 1.85f;
                    break;
            }

            abilityThreat *= Lookup.StanceThreatMultipler(Character, Stats);

            Threat = abilityThreat;
        }

        public AbilityModel(Character character, Stats stats, Ability ability)
        {
            Character   = character;
            Stats       = stats;
            Ability     = ability;
            Options     = Character.CalculationOptions as CalculationOptionsProtWarr;
            Talents     = Character.WarriorTalents;
            AttackTable = new AttackTable(character, stats, ability);

            ArmorReduction      = Lookup.TargetArmorReduction(Character, Stats);
            DamageMultiplier    = Lookup.StanceDamageMultipler(Character, Stats);

            CalculateDamage();
            CalculateThreat();
        }
    }

    public class AbilityModelList : System.Collections.DictionaryBase
    {
        public AbilityModel this[Ability ability]
        {
            get { return ((AbilityModel)(Dictionary[ability])); }
            set { Dictionary[ability] = value; }
        }

        public void Add(Ability ability, AbilityModel abilityModel)
        {
            Dictionary.Add(ability, abilityModel);
        }

        public void Add(Ability ability, Character character, Stats stats)
        {
            Dictionary.Add(ability, new AbilityModel(character, stats, ability));
        }

        public void Remove(Ability ability)
        {
            Dictionary.Remove(ability);
        }

        public bool Contains(Ability ability)
        {
            return Dictionary.Contains(ability);
        }
    }
}
