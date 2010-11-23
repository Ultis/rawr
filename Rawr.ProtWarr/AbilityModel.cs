using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Rawr.ProtWarr
{
    public class AbilityModel
    {
        private Character Character;
        private Stats Stats;
        private WarriorTalents Talents;
        private CalculationOptionsProtWarr Options;

        public readonly AttackTable AttackTable;

        public Ability Ability { get; private set; }
        public string Name { get; private set; }
        public float Damage { get; private set; }
        public float Threat { get; private set; }
        public float DamageMultiplier { get; private set; }
        public float ArmorReduction { get; private set; }
        public float CritPercentage
        {
            get { return AttackTable.Critical; }
        }
        public float HitPercentage
        {
            get { return AttackTable.AnyHit; }
        }
        public bool IsAvoidable { get; private set; }
        public bool IsWeaponAttack { get; private set; }

        private void CalculateDamage()
        {
            float baseDamage        = 0.0f;
            float critMultiplier    = 1.0f + Lookup.BonusCritMultiplier(Character, Stats, Ability);

            switch (Ability)
            {
                // White Damage
                case Ability.None:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false);
                    break;
                case Ability.Cleave:
                    baseDamage = 6.0f + (Stats.AttackPower * 0.562f);
                    DamageMultiplier *= (1.0f + Talents.Thunderstruck * 0.03f) * (1.0f + Talents.WarAcademy * 0.05f);
                    break;
                case Ability.ConcussionBlow:
                    baseDamage = Stats.AttackPower * 0.75f;
                    break;
                case Ability.DeepWounds:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false) * (Talents.DeepWounds * 0.16f);
                    DamageMultiplier *= (1.0f + Stats.BonusBleedDamageMultiplier);
                    ArmorReduction = 0.0f;
                    break;
                case Ability.Devastate:
                    // Assumes 3 stacks of Sunder Armor debuff
                    baseDamage = (Lookup.WeaponDamage(Character, Stats, true) * 1.5f) + (336.0f * 3.0f);
                    if (Talents.GlyphOfDevastate)
                        DamageMultiplier *= 1.05f;
                    DamageMultiplier *= (1.0f + Stats.BonusDevastateDamage);
                    break;
                case Ability.HeroicStrike:
                    baseDamage = 8.0f + (Stats.AttackPower * 0.75f);
                    DamageMultiplier *= (1.0f + Talents.WarAcademy * 0.05f);
                    break;
                case Ability.HeroicThrow:
                    baseDamage = 12.0f + (Stats.AttackPower * 0.5f); // LOOK UP COEFFICIENT
                    break;
                case Ability.Rend:
                    baseDamage = (Lookup.WeaponDamage(Character, Stats, false) * 1.25f) + 525.0f;
                    DamageMultiplier *= (1.0f + Stats.BonusBleedDamageMultiplier) * (1.0f + Talents.Thunderstruck * 0.03f);
                    ArmorReduction = 0.0f;
                    break;
                case Ability.Revenge:
                    baseDamage = (1816.5f * (1.0f + Talents.ImprovedRevenge * 0.3f)) + (Stats.AttackPower * 0.31f); // LOOK UP COEFFICIENT
                    if (Talents.GlyphOfRevenge)
                        DamageMultiplier *= 1.1f;
                    break;
                case Ability.ShieldSlam:
                    baseDamage = Stats.AttackPower * 0.6f; // LOOK UP BASE DAMAGE
                    if (Talents.GlyphOfShieldSlam)
                        DamageMultiplier *= 1.1f;
                    DamageMultiplier *= (1.0f + Stats.BonusShieldSlamDamage);
                    break;
                case Ability.Shockwave:
                    baseDamage = Stats.AttackPower * 0.75f;
                    DamageMultiplier *= (1.0f + Stats.BonusShockwaveDamage);
                    break;
                case Ability.Slam:
                    baseDamage = (Lookup.WeaponDamage(Character, Stats, false) * 1.25f) + 538.75f;
                    break;
                case Ability.ThunderClap:
                    baseDamage = 300.0f + (Stats.AttackPower * 0.12f); // LOOK UP COEFFICIENT
                    DamageMultiplier *= (1.0f + Talents.Thunderstruck * 0.03f); 
                    break;
                case Ability.VictoryRush:
                    baseDamage = Stats.AttackPower * 0.56f;
                    DamageMultiplier *= (1.0f + Talents.WarAcademy * 0.05f);
                    break;
            }

            // All damage multipliers
            baseDamage *= DamageMultiplier;
            // Armor reduction
            baseDamage *= (1.0f - ArmorReduction);
            // Combat table adjustments
            baseDamage *= 
                AttackTable.Hit + 
                AttackTable.Critical * critMultiplier + 
                AttackTable.Glance * Lookup.GlancingReduction(Character, Options.TargetLevel);

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

        public AbilityModel(Character character, Stats stats, CalculationOptionsProtWarr options, Ability ability)
        {
            Character   = character;
            Stats       = stats;
            Ability     = ability;
            Options     = options;
            Talents     = Character.WarriorTalents;
            AttackTable = new AttackTable(character, stats, options, ability);

            Name                = Lookup.Name(Ability);
            ArmorReduction      = Lookup.TargetArmorReduction(Character, Stats, Options.TargetArmor);
            DamageMultiplier    = Lookup.StanceDamageMultipler(Character, Stats);
            IsAvoidable         = Lookup.IsAvoidable(Ability);
            IsWeaponAttack      = Lookup.IsWeaponAttack(Ability);

            CalculateDamage();
            CalculateThreat();
        }
    }

    public class AbilityModelList : KeyedCollection<Ability, AbilityModel>
    {
        protected override Ability GetKeyForItem(AbilityModel abilityModel)
        {
            return abilityModel.Ability;
        }

        public void Add(Ability ability, Character character, Stats stats, CalculationOptionsProtWarr options)
        {
            this.Add(new AbilityModel(character, stats, options, ability));
        }
    }
}
