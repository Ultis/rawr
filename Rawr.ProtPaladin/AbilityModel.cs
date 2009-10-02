using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin {
    public class AbilityModel {
        private Ability Ability;
        //private DamageType DamageType;
        //private AttackType AttackType;
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
        public float CritPercentage { get { return AttackTable.Critical; } }

        private void CalculateDamage() {
            float baseDamage = 0.0f;
            float critMultiplier = 0.0f;
            float duration = 0.0f;
            float AP = Stats.AttackPower;
            float SP = Stats.SpellPower;

            #region Ability Base Damage
            switch (Ability) {
                // White Damage
                case Ability.None:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false);
                    // Glancing blow reduction
                    DamageMultiplier *= (1.0f + Stats.BonusPhysicalDamageMultiplier);
                    DamageMultiplier *= (1.0f - (Lookup.GlancingReduction(Character) * AttackTable.Glance));
                    critMultiplier = 1.0f;
                    DamageMultiplier *= (1.0f - ArmorReduction);
                    break;               
                case Ability.ShieldOfRighteousness:
                    float blockValue = Stats.BlockValue + Stats.ShieldOfRighteousnessBlockValue + Stats.JudgementBlockValue;

                    if (blockValue < 30 * Character.Level) {
                        baseDamage = blockValue + 520;
                    } else if (blockValue < 39.5 * Character.Level) {
                        baseDamage = 30 * Character.Level + (0.95f * (blockValue - 30 * Character.Level)) - (0.000625f * (float)Math.Pow(blockValue - 30 * Character.Level, 2)) + 520;
                    } else {
                        baseDamage = 30 * Character.Level + (0.95f * 9.5f * Character.Level) - (0.000625f * (float)Math.Pow(9.5 * Character.Level, 2)) + 520;
                    }

                    baseDamage += Stats.BonusShieldOfRighteousnessDamage;

                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfTheRighteous:
                    if (Talents.HammerOfTheRighteous > 0 && Character.MainHand != null)
                    {
                        baseDamage = (AP / 14 + (Character.MainHand.MinDamage + Character.MainHand.MaxDamage) / 2 / Character.MainHand.Speed) * 4f;
                        DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + Stats.BonusHammerOfTheRighteousMultiplier);
                        critMultiplier = 1.0f;
                    }
                    break;
                // Seal of Vengeance is the tiny damage that applies on each swing; Holy Vengeance is the DoT
                // While trivial threat and damage, it's modeled for compatibility with Seal of Righteousness
                case Ability.SealOfVengeance:
                    baseDamage = Lookup.WeaponDamage(Character, Stats, false) * 0.33f;
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) *
                        (1f + Stats.BonusSealOfVengeanceDamageMultiplier);
                    critMultiplier = 1.0f;
                    break;
                // Judgement of Vengeance assumes 5 stacks of Holy Vengeance
                case Ability.JudgementOfVengeance:
                    baseDamage = (0.22f * SP + 0.14f * AP) * (1.0f + 0.5f);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure);
                    if (Talents.GlyphOfJudgement) { DamageMultiplier *= (1.0f + 0.10f); }                    
                    critMultiplier = 1.0f;
                    break;
                case Ability.SealOfRighteousness:
                    baseDamage = (Lookup.WeaponSpeed(Character, Stats) * 0.022f * AP) + 
                                 (Lookup.WeaponSpeed(Character, Stats) * 0.044f * SP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) * 
                        (1f + Stats.BonusSealOfRighteousnessDamageMultiplier);
                    if (Talents.GlyphOfSealOfRighteousness) { DamageMultiplier *= (1.0f + 0.10f); }
                    critMultiplier = 0.0f;
                    break;
                case Ability.JudgementOfRighteousness:
                    baseDamage = (1.0f + 0.2f * AP + 0.32f * SP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure);
                    if (Talents.GlyphOfJudgement) { DamageMultiplier *= (1.0f + 0.10f); }
                    critMultiplier = 1.0f;
                    break;
                // 5 stacks of Holy Vengeance are assumed
                // TODO: implement stacking mechanic for beginning-of-fight TPS
                case Ability.HolyVengeance:
                    baseDamage = 5f * (0.016f * SP + 0.032f * AP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1.0f + 0.03f * Talents.SealsOfThePure) *
                        (1f + Stats.BonusSealOfVengeanceDamageMultiplier);
                    critMultiplier = 0.0f;
                    break;
                case Ability.HolyShield:
                    if (Talents.HolyShield > 0) {
                        baseDamage = (211f + (0.056f * AP) + (0.09f * SP)) * 1.3f;
                        DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
                        critMultiplier = 0.0f;
                    }
                    break;
                case Ability.Consecration:
                    baseDamage = 113f + 0.04f * (SP + Stats.ConsecrationSpellPower) + 0.04f * AP;
                    duration = 8.0f;
                    if (Talents.GlyphOfConsecration) { duration += 2.0f; }
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
                    critMultiplier = 0.0f;
                    break;
                case Ability.Exorcism:
                    baseDamage = (1028f + 0.15f * SP + 0.15f * AP);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1f + Talents.SanctityOfBattle * 0.05f);
                    if (Talents.GlyphOfExorcism) { DamageMultiplier *= (1.0f + 0.20f); }
                    critMultiplier = 0.5f;
                    break;
                case Ability.AvengersShield:
                    baseDamage = (846f + 0.07f * SP + 0.07f * AP) * 1.3f;// it has a range though: 846.14-1034.14
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
                    critMultiplier = 1.0f;
                    break;
                case Ability.HolyWrath:// what about aoe cap of max 10 targets ?
                    baseDamage = 1050f;//1050 - 1234
                    // holy *= Lookup.CreatureTypeDamageMultiplier(Character);//, holy);
                    baseDamage += (AP * 0.07f) + (SP * 0.07f); 
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);//any other ?
                    critMultiplier = 0.5f;
                    break;
                case Ability.HammerOfWrath:
                    baseDamage = 1139f;//1139.3 to 1257.3
                    baseDamage += (AP * 0.15f) + (SP * 0.15f);
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier);
                    critMultiplier = 1.0f;
                    break;
                case Ability.RetributionAura:
                    baseDamage = 112f;
                    baseDamage += (SP * 0.0666f); 
                    DamageMultiplier *= (1f + Stats.BonusHolyDamageMultiplier) * (1f + Talents.SanctifiedRetribution * 0.5f);
                    critMultiplier = 0.0f;
                    break;
            }
            #endregion

            // All damage multipliers, 1HWS, Armor etc...do we need to split buff/debuff ?
            baseDamage *= DamageMultiplier;

            #region Miss Chance, Avoidance Chance
            if (Lookup.IsSpell(Ability)) {
                // Probability calculation, since each tick can be resisted individually.
                if (Ability == Ability.Consecration) {
                    baseDamage = Lookup.GetConsecrationTickChances(duration, baseDamage, AttackTable.Miss);
                } // Missed spell attacks TODO: expand Ability Model to include a check for damage type, not only spell.
                else { baseDamage *= (1.0f - AttackTable.Miss); }
            } else {
                // Missed attacks
                if (Lookup.IsAvoidable(Ability))
                    baseDamage *= (1.0f - AttackTable.AnyMiss);
                else
                    baseDamage *= (1.0f - AttackTable.Miss);
            }
            #endregion

            #region Partial Resists
            // Partial Resists
            if (Lookup.HasPartials(Ability))
            {
                // Magical Damage Resisted when a partial happens, averaged over all slices. DIRTY
                //float averagePartialReduction = StatConversion.GetAverageResistance(Character.Level, Options.TargetLevel, 0, Stats.SpellPenetration);
                
                // Detailed table of Partial slices. CLEAN
                float[] partialChanceTable = StatConversion.GetResistanceTable(Character.Level, Options.TargetLevel, 0.0f, Stats.SpellPenetration);
                
                // Here goes nothing, Damage averaged over the different partial slices that can happen.
                float partialDamage = 0.0f;
                
                for (int i = 0; i < 11; i++)
                {
                    partialDamage += partialChanceTable[i] * (1.0f - 0.1f * (float)i) * baseDamage;
                }
                // Chance a Patial happens. ANY PARTIAL
                //float partialChance = Lookup.TargetAvoidanceChance(Character, Stats, HitResult.Resist);
                // Rough average over all hits that fall into partial chance. DIRTY
                //float partialDamage1 = partialChance * (1.0f - averagePartialReduction) * baseDamage;
                // Those hits that don't have a chance to get a partial. CLEAN
                //float restDamage1 = (1.0f - partialChance) * baseDamage;
                
                baseDamage = partialDamage;

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
                  //            resist += (int)(damage - 1);
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
            #endregion

            #region Critical Damage
            // Average critical strike bonuses
            if (Lookup.CanCrit(Ability))
            {
            //switch (AttackType)
            //{
            //    case (AttackType.Melee):
            //        critMultiplier = 1.0f;
            //    case (AttackType.Spell):
            //        critMultiplier = 0.5f;
            //    case (AttackType.Ranged):
            //        critMultiplier = 0.5f;
            //    case (AttackType.DOT):
            //        critMultiplier = 0.0f;
            //}
				baseDamage += baseDamage * critMultiplier * AttackTable.Critical;
            }
            //else
            //    baseDamage *= 1.0f;
            #endregion

            // Final Damage the Ability deals
            Damage = baseDamage;
        }

        private void CalculateThreat() {
            // Base threat is always going to be the damage of the ability, if it is damaging
            float abilityThreat = Damage;
            float holyThreatModifier = 1.8f;

            switch (Ability) {
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

        public AbilityModel(Character character, Stats stats, Ability ability) {
            Character   = character;
            Stats       = stats;
            Ability     = ability;
            Options     = Character.CalculationOptions as CalculationOptionsProtPaladin;
            Talents     = Character.PaladinTalents;
            AttackTable = new AttackTable(character, stats, ability);

            Name                = Lookup.Name(Ability);
            ArmorReduction      = Lookup.EffectiveTargetArmorReduction(Character, Stats);//TODO: Separate spells, no need to calculate armor, if it doesn't affect the ability anyways
            DamageMultiplier    = Lookup.StanceDamageMultipler(Character, Stats);
            DamageMultiplier   *= Lookup.CreatureTypeDamageMultiplier(Character);

            CalculateDamage();
            CalculateThreat();
        }
    }

    public class AbilityModelList : Dictionary<Ability, AbilityModel> { // [Astryl] Changed this to a generic Dictionary<>, and commented out all but one of the members, since they were redundant
		//public AbilityModel this[Ability ability]
		//{
		//    get { return ((AbilityModel)(Dictionary[ability])); }
		//    set { Dictionary[ability] = value; }
		//}

		//public void Add(Ability ability, AbilityModel abilityModel)
		//{
		//    Dictionary.Add(ability, abilityModel);
		//}

		public void Add(Ability ability, Character character, Stats stats)
		{
			this.Add(ability, new AbilityModel(character, stats, ability));
		}

		//public void Remove(Ability ability)
		//{
		//    Dictionary.Remove(ability);
		//}

		//public bool Contains(Ability ability)
		//{
		//    return Dictionary.Contains(ability);
		//}
    }
}
