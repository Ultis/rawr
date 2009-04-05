using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
    public class AbilityModel
    {
        private Ability Ability;
        private Character Character;
        private Stats Stats;
        private PaladinTalents Talents;
        private CalculationOptionsProtPaladin Options;

        public readonly AttackTable AttackTable;

        public string Name { get; private set; }
        public float Damage { get; private set; }
        public float Threat { get; private set; }
        public float DamageMultiplier { get; private set; }
        public float ArmorReduction { get; private set; }
        public float CritPercentage
        {
            get { return AttackTable.Critical; }
        }

        private void CalculateDamage()
        {
            float baseDamage        = 0.0f;
            float critMultiplier    = 1.0f;

            critMultiplier = (2.0f * (1.0f + Stats.BonusCritMultiplier));

            switch (Ability)
            {
                // White Damage
                case Ability.None:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false);
                    break;               
                case Ability.ShieldOfRighteousness:
                    baseDamage = (Stats.BlockValue + Stats.ShieldOfRighteousnessBlockValue + 1f / 3f * Stats.JudgementBlockValue) * 1.3f + 520f;
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
					ArmorReduction = 0.0f;
                    break;
                case Ability.HammerOfTheRighteous:
                    if (Talents.HammerOfTheRighteous > 0 && Character.MainHand != null)
                    {
                        baseDamage = (Stats.AttackPower / 14 + (Character.MainHand.MinDamage + Character.MainHand.MaxDamage) / 2 / Character.MainHand.Speed) * 4f;
                        DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + Stats.BonusHammerOfTheRighteousMultiplier);
                        ArmorReduction = 0.0f;
                    }
                    break;
				// Seal of Vengeance is the tiny damage that applies on each swing; Holy Vengeance is the DoT
				// While trivial threat and damage, it's modeled for compatibility with Seal of Righteousness
				case Ability.SealOfVengeance:
					baseDamage = (1.0f + 0.02f * Stats.SpellPower);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) *
                        (1f + Stats.BonusSealOfVengeanceDamageMultiplier);
					ArmorReduction = 0.0f;
					break;
                // Judgement of Vengeance assumes 5 stacks of Holy Vengeance
                case Ability.JudgementOfVengeance:
                    baseDamage = (1.0f + 0.28f * Stats.SpellPower + 0.175f * Stats.AttackPower) * 1.5f;
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure);
                    if (Talents.GlyphOfJudgement)
                    {
                        DamageMultiplier *= (1.0f + 0.01f);
                    }                    
					ArmorReduction = 0.0f;
                    break;
                case Ability.SealOfRighteousness:
                    baseDamage = (Lookup.WeaponSpeed(Character, Stats) * 0.022f * Stats.AttackPower) + 
                                 (Lookup.WeaponSpeed(Character, Stats) * 0.044f * Stats.SpellPower);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) * 
                        (1f + Stats.BonusSealOfRighteousnessDamageMultiplier);
                    if (Talents.GlyphOfSealOfRighteousness)
                    {
                        DamageMultiplier *= (1.0f + 0.1f);
                    }
					ArmorReduction = 0.0f;
					break;
				case Ability.JudgementOfRighteousness:
                    baseDamage = (1.0f + 0.2f * Stats.AttackPower + 0.32f * Stats.SpellPower);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure);
                    if (Talents.GlyphOfJudgement)
                    {
                        DamageMultiplier *= (1.0f + 0.01f);
                    }
					ArmorReduction = 0.0f;
					break;
				// 5 stacks of Holy Vengeance are assumed
				// TODO: implement stacking mechanic for beginning-of-fight TPS
                case Ability.HolyVengeance:
                    baseDamage = 5f * (0.016f * Stats.SpellPower + 0.032f * Stats.AttackPower);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) *
                        (1f + Stats.BonusSealOfVengeanceDamageMultiplier);
					ArmorReduction = 0.0f;
                    break;
                case Ability.HolyShield:
                    if (Talents.HolyShield > 0)
                    {
                        baseDamage = (211f + (.056f * Stats.AttackPower) + (.09f * Stats.SpellPower)) * 1.3f;
                        DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
                        ArmorReduction = 0.0f;
                    }
                    break;
				// TODO: Split Consecration into X number of individually resistable stacks
				case Ability.Consecration:
                    baseDamage = 904f + 0.32f * (Stats.SpellPower + Stats.ConsecrationSpellPower) + 0.32f * Stats.AttackPower;
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
					ArmorReduction = 0.0f;
					break;
				case Ability.Exorcism:
                    baseDamage = (1028f + 0.15f * Stats.SpellPower + 0.15f * Stats.AttackPower);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1f + Talents.SanctityOfBattle * 0.05f);
                        if (Talents.GlyphOfExorcism)
                        DamageMultiplier *= (1.0f + 0.30f);
					ArmorReduction = 0.0f;
					break;
				case Ability.AvengersShield:
                    baseDamage = 846f + 0.07f * Stats.SpellPower + 0.07f * Stats.AttackPower;
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
					ArmorReduction = 0.0f;
					break;
            }

            // All damage multipliers
            baseDamage *= DamageMultiplier;
            // Average critical strike bonuses
            baseDamage = (baseDamage * (1.0f - AttackTable.Critical)) + (baseDamage * critMultiplier * AttackTable.Critical);
            // Average glancing blow reduction
            baseDamage *= (1.0f - (Lookup.GlancingReduction(Character) * AttackTable.Glance));
            // Average resist reduction
            baseDamage *= (1.0f - (Lookup.GlancingReduction(Character) * AttackTable.Resist));
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
            float holyThreatModifier = 1.9f;

            switch (Ability)
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
                    abilityThreat *= holyThreatModifier;
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
            Options     = Character.CalculationOptions as CalculationOptionsProtPaladin;
            Talents     = Character.PaladinTalents;
            AttackTable = new AttackTable(character, stats, ability);

            Name                = Lookup.Name(Ability);
            ArmorReduction      = Lookup.TargetArmorReduction(Character, Stats);
            DamageMultiplier    = Lookup.StanceDamageMultipler(Character, Stats);
            DamageMultiplier   *= Lookup.CreatureTypeDamageMultiplier(Character);

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
