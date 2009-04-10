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
            float critMultiplier    = (2.0f * (1.0f + Stats.BonusCritMultiplier));
            float duration = 0.0f;
            float AP = Stats.AttackPower;
            float SP = Stats.SpellPower;

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
                        baseDamage = (AP / 14 + (Character.MainHand.MinDamage + Character.MainHand.MaxDamage) / 2 / Character.MainHand.Speed) * 4f;
                        DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + Stats.BonusHammerOfTheRighteousMultiplier);
                        ArmorReduction = 0.0f;
                    }
                    break;
				// Seal of Vengeance is the tiny damage that applies on each swing; Holy Vengeance is the DoT
				// While trivial threat and damage, it's modeled for compatibility with Seal of Righteousness
				case Ability.SealOfVengeance:
					baseDamage = (1.0f + 0.02f * SP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) *
                        (1f + Stats.BonusSealOfVengeanceDamageMultiplier);
					ArmorReduction = 0.0f;
					break;
                // Judgement of Vengeance assumes 5 stacks of Holy Vengeance
                case Ability.JudgementOfVengeance:
                    baseDamage = (1.0f + 0.28f * SP + 0.175f * AP) * 1.5f;
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure);
                    if (Talents.GlyphOfJudgement)
                    {
                        DamageMultiplier *= (1.0f + 0.10f);
                    }                    
					ArmorReduction = 0.0f;
                    break;
                case Ability.SealOfRighteousness:
                    baseDamage = (Lookup.WeaponSpeed(Character, Stats) * 0.022f * AP) + 
                                 (Lookup.WeaponSpeed(Character, Stats) * 0.044f * SP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) * 
                        (1f + Stats.BonusSealOfRighteousnessDamageMultiplier);
                    if (Talents.GlyphOfSealOfRighteousness)
                    {
                        DamageMultiplier *= (1.0f + 0.10f);
                    }
					ArmorReduction = 0.0f;
					break;
				case Ability.JudgementOfRighteousness:
                    baseDamage = (1.0f + 0.2f * AP + 0.32f * SP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure);
                    if (Talents.GlyphOfJudgement)
                    {
                        DamageMultiplier *= (1.0f + 0.10f);
                    }
					ArmorReduction = 0.0f;
					break;
				// 5 stacks of Holy Vengeance are assumed
				// TODO: implement stacking mechanic for beginning-of-fight TPS
                case Ability.HolyVengeance:
                    baseDamage = 5f * (0.016f * SP + 0.032f * AP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) *
                        (1f + Stats.BonusSealOfVengeanceDamageMultiplier);
					ArmorReduction = 0.0f;
                    break;
                case Ability.HolyShield:
                    if (Talents.HolyShield > 0)
                    {
                        baseDamage = (211f + (.056f * AP) + (.09f * SP)) * 1.3f;
                        DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
                        ArmorReduction = 0.0f;
                    }
                    break;
				case Ability.Consecration:
                    baseDamage = 113f + 0.04f * (SP + Stats.ConsecrationSpellPower) + 0.04f * AP;
                    duration = 8.0f;
                    if (Talents.GlyphOfConsecration)
                    {
                        duration += 2.0f;;
                    }
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
					ArmorReduction = 0.0f;
					break;
				case Ability.Exorcism:
                    baseDamage = (1028f + 0.15f * SP + 0.15f * AP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1f + Talents.SanctityOfBattle * 0.05f);
                    if (Talents.GlyphOfExorcism)
                    {
                        DamageMultiplier *= (1.0f + 0.20f);
                    }
					ArmorReduction = 0.0f;
					break;
				case Ability.AvengersShield:
					baseDamage = (846f + 0.07f * SP + 0.07f * AP) * 1.3f;// it has a range though: 846.14-1034.14
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
					ArmorReduction = 0.0f;
					break;
                case Ability.HolyWrath:// what about aoe cap of max 10 targets ?
                    baseDamage = 1050f;//1050 - 1234
                    // holy *= Lookup.CreatureTypeDamageMultiplier(Character);//, holy);
                    baseDamage += (AP * 0.07f) + (SP * 0.07f); 
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);//any other ?
					ArmorReduction = 0.0f;
					break;
				case Ability.HammerOfWrath:
					baseDamage = 1139f;//1139.3 to 1257.3
					baseDamage += (AP * 0.15f) + (SP * 0.15f);
					DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
					ArmorReduction = 0.0f;
					break;
			    case Ability.RetributionAura:
                    baseDamage = 112f;
                    baseDamage += (SP * 0.0666f); 
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1f + Talents.SanctifiedRetribution * 0.5f);
					ArmorReduction = 0.0f;
					break;
            }

            // All damage multipliers, 1HWS etc...
            baseDamage *= DamageMultiplier; 
            // Average critical strike bonuses
            baseDamage *= (1.0f + critMultiplier * AttackTable.Critical);//(baseDamage * (1.0f - AttackTable.Critical)) + (baseDamage * critMultiplier * AttackTable.Critical);
            
            if (Lookup.HasPartials(Ability))
            {
                // Partial resist reduction
                float partialDamageResisted = StatConversion.GetAverageResistance(Character.Level, Options.TargetLevel, 0, Stats.SpellPenetration) * AttackTable.Resist;
                baseDamage *= (1.0f - partialDamageResisted);
                
                  // Magic damage, Spell resist
                  //   if (!DamageType.Physical)
                  //    {
                  //        // Get Targets base resistance for DamageType
                  //        float tmpvalue2 = (float)GetResistance(DamageType);
                  //        // Ignore resistance by self SPELL_AURA_MOD_TARGET_RESISTANCE aura
                  //        tmpvalue2 += (float)GetTotalAuraModifierByMiscMask(SPELL_AURA_MOD_TARGET_RESISTANCE, schoolMask);
                  //
                  //        tmpvalue2 *= (float)(0.15f / getLevel());
                  //        if (tmpvalue2 < 0.0f)
                  //            tmpvalue2 = 0.0f;
                  //        if (tmpvalue2 > 0.75f)
                  //            tmpvalue2 = 0.75f;
                  //        int ran = urand(0, 100);
                  //        int faq[4] = {24,6,4,6};
                  //        int m = 0;
                  //        float Binom = 0.0f;
                  //        for (int i = 0; i < 4; i++)
                  //        {
                  //            Binom += 2400 *( Math.Pow(tmpvalue2, i) * Math.Pow( (1-tmpvalue2), (4-i)))/faq[i];
                  //            if (ran > Binom )
                  //                ++m;
                  //            else
                  //                break;
                  //        }
                  //        if (AttackType == DOT && m == 4)
                  //            *resist += (int)(damage - 1);
                  //        else
                  //            resist += (int)(damage * m / 4);
                  //        if(resist > damage)
                  //            resist = damage;
                  //    }
                  //    else
                  //        resist = 0;
                  //
                  //    int RemainingDamage = damage - resist;

            }
            if (Lookup.IsSpell(Ability))
            {
                if (Ability == Ability.Consecration)
                {
                    baseDamage = duration * ( baseDamage * (1.0f - AttackTable.Miss));
                }
                // Missed spell attacks TODO: expand Ability Model to include a check for damage type, not only spell.
                else
                baseDamage *= (1.0f - AttackTable.Miss);
            }
            else
            {
                // Average glancing blow reduction
                baseDamage *= (1.0f - (Lookup.GlancingReduction(Character) * AttackTable.Glance));
                // Armor reduction
                baseDamage *= (1.0f - ArmorReduction);
                // Missed attacks
                baseDamage *= (1.0f - AttackTable.AnyMiss);
            }
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
            	case Ability.HolyWrath:
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
