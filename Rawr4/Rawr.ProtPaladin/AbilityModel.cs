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
        private CalculationOptionsProtPaladin CalcOpts;
        private BossOptions BossOpts;

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

            int targetLevel = BossOpts.Level;

            #region Ability Base Damage
            switch (Ability) {
                /************
                 * Spells
                 ************/
                case Ability.AvengersShield:
                    if (Character.OffHand == null || (Talents.Divinity == 0 && Talents.SealsOfThePure == 0 && Talents.EternalGlory == 0)) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = 3113.187994f + (0.419f * AP) + (0.21f * SP);

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier)
                                      * (Talents.GlyphOfFocusedShield ? 1.3f : 1.0f);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfWrath:
                    baseDamage = 4015.02439f + (0.039f * AP) + (0.117f * SP);

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier);
                    
                    critMultiplier = 1.0f;
                    break;
                case Ability.HolyWrath:
                    baseDamage = 2435.781339f + (0.61f * SP);

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier);
                    
                    critMultiplier = 0.5f;
                    break;

                /************
                 * Melee
                 ************/
                case Ability.CrusaderStrike:
                    if (Character.MainHand == null) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = Lookup.WeaponDamage(Character, Stats, true);
                    baseDamage *= 1.2f
                                + (1.2f * (Talents.Crusade * 0.1f))
                                + (1.2f * (Talents.WrathOfTheLightbringer * 0.5f));

                    DamageMultiplier *= (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                      * (1.0f - ArmorReduction);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfTheRighteous:
                    if (Talents.HammerOfTheRighteous == 0 || Character.MainHand == null) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = Lookup.WeaponDamage(Character, Stats, true);
                    baseDamage *= 0.3f
                                + (0.3f * (Talents.Crusade * 0.1f))
                                + (0.3f * (Talents.GlyphOfHammerOfTheRighteous ? 0.1f : 0f));

                    DamageMultiplier *= (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                      * (1.0f + Stats.BonusHammerOfTheRighteousMultiplier)
                                      * (1.0f - ArmorReduction);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfTheRighteousProc:
                    if (Talents.HammerOfTheRighteous == 0 || Character.MainHand == null) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = 728.8813374f + (0.18f * AP);
                    baseDamage *= 1f
                                + (Talents.Crusade * 0.1f)
                                + (Talents.GlyphOfHammerOfTheRighteous ? 0.1f : 0f);

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier)
                                      * (1.0f + Stats.BonusHammerOfTheRighteousMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.Judgement:
                    Damage = 0f;
                    return;
                case Ability.JudgementOfRighteousness:
                    baseDamage = 1f + (0.2f * AP) + (0.32f * SP);
                    baseDamage *= 1f
                                + (Talents.WrathOfTheLightbringer * 0.5f)
                                + (Talents.GlyphOfJudgement ? 0.1f : 0f);

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.JudgementOfTruth:
                    float censureStacks = 5;

                    baseDamage = 1 + (0.223f * SP) + (0.142f * AP) * (1.0f + 0.1f * censureStacks);
                    baseDamage *= 1f
                                + (Talents.WrathOfTheLightbringer * 0.5f)
                                + (Talents.GlyphOfJudgement ? 0.1f : 0f);

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier)
                                      * (Talents.GlyphOfJudgement ? 1.1f : 1.0f);

                    critMultiplier = 1.0f;
                    break;

                    // Leaving off here





                case Ability.MeleeSwing:
                    baseDamage        = Stats.WeaponDamage;

                    DamageMultiplier *= (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                      * (1.0f - (Lookup.GlancingReduction(Character, targetLevel) * AttackTable.Glance))
                                      * (1.0f - ArmorReduction);

                    critMultiplier    = 1.0f;
                    break;
                case Ability.ShieldOfTheRighteous:
                    float blockValue  = Stats.BlockValue + Stats.ShieldOfRighteousnessBlockValue + Stats.JudgementBlockValue + Stats.HolyShieldBlockValue;

                    float blockValueDRStart = 30 * Character.Level;

                    if (blockValue < blockValueDRStart) {
                        baseDamage    = blockValue;
                    } else if (blockValue < 39.5 * Character.Level) {
                        baseDamage    = blockValueDRStart + (0.95f * (blockValue - blockValueDRStart)) - (0.000625f * (float)Math.Pow(blockValue - blockValueDRStart, 2));
                    } else {
                        baseDamage    = blockValueDRStart + (0.95f * 9.5f * Character.Level) - (0.000625f * (float)Math.Pow(9.5 * Character.Level, 2));
                    }

                    baseDamage       += 520f
                                      + Stats.BonusShieldOfRighteousnessDamage;

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier);
                    critMultiplier    = 1.0f;
                    break;
                // Seal of Vengeance is the tiny damage that applies on each swing; Holy Vengeance is the DoT
                // While trivial threat and damage, it's modeled for compatibility with Seal of Righteousness
                case Ability.SealOfTruth:
                    baseDamage        = Stats.WeaponDamage * 0.33f;

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier)
                                      * (1.0f + 0.03f * Talents.SealsOfThePure)
                                      * (1.0f + Stats.BonusSealOfVengeanceDamageMultiplier);

                    critMultiplier    = 1.0f;
                    break;
                // 5 stacks of Holy Vengeance are assumed
                // TODO: implement stacking mechanic for beginning-of-fight TPS
                case Ability.Censure:
                    baseDamage        = 5f * (0.016f * SP + 0.032f * AP);

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier)
                                      * (1.0f + 0.03f * Talents.SealsOfThePure)
                                      * (1.0f + Stats.BonusSealOfVengeanceDamageMultiplier);

                    critMultiplier    = 0.0f;
                    break;
                case Ability.SealOfRighteousness:
                    baseDamage        = Lookup.WeaponSpeed(Character, Stats) * ((0.022f * AP) + (0.044f * SP));

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier)
                                      * (1.0f + 0.03f * Talents.SealsOfThePure)
                                      * (1.0f + Stats.BonusSealOfRighteousnessDamageMultiplier);

                    critMultiplier    = 0.0f;
                    break;
                case Ability.Consecration:
                    baseDamage        = 113f + (0.04f * (SP + Stats.ConsecrationSpellPower)) + (0.04f * AP);
                    duration          = (Talents.GlyphOfConsecration ? 10.0f : 8.0f);
                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier);
                    critMultiplier    = 0.0f;
                    break;
                case Ability.RetributionAura:
                    baseDamage        = 112f + (SP * 0.0666f);

                    DamageMultiplier *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier    = 0.0f;
                    break;
            }
            #endregion

            // All damage multipliers, 1HWS, Armor etc...do we need to split buff/debuff ?
            baseDamage *= DamageMultiplier;

            #region Miss Chance, Avoidance Chance
            if (Lookup.IsSpell(Ability))
            {
                if (Ability == Ability.Consecration)
                    // Probability calculation, since each tick can be resisted individually.
                    baseDamage = Lookup.GetConsecrationTickChances(duration, baseDamage, AttackTable.Miss);
                else
                    // Missed spell attacks
                    // TODO: expand Ability Model to include a check for damage type, not only spell.
                    baseDamage *= (1.0f - AttackTable.Miss);
            } else {
                // Missed attacks
                if (Lookup.IsAvoidable(Ability))
                    baseDamage *= (1.0f - AttackTable.AnyMiss);
                else
                    baseDamage *= (1.0f - AttackTable.Miss);
            }
            #endregion

            // Average critical strike bonuses
            if (Lookup.CanCrit(Ability))
                baseDamage += baseDamage * critMultiplier * AttackTable.Critical;

            // Final Damage the Ability deals
            Damage = baseDamage;
        }

        private void CalculateThreat() {
            // With Righteous Fury, threat is Damage plus 200%, or triple the damage.
            Threat = Damage * 3f;
        }

        public AbilityModel(Character character, Stats stats, Ability ability, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts) {
            Character   = character;
            Stats       = stats;
            Ability     = ability;
            CalcOpts    = calcOpts;
            BossOpts    = bossOpts;
            
            Talents     = Character.PaladinTalents;
            AttackTable = new AttackTable(character, stats, ability, CalcOpts, BossOpts);

            if (!Lookup.IsSpell(Ability))
                ArmorReduction  = Lookup.EffectiveTargetArmorReduction(Character, Stats, BossOpts.Armor, BossOpts.Level);

            Name                = Lookup.Name(Ability);
            DamageMultiplier    = Lookup.StanceDamageMultipler(Character, Stats);

            CalculateDamage();
            CalculateThreat();
        }
    }

    public class AbilityModelList : Dictionary<Ability, AbilityModel>
    {
        public void Add(Ability ability, Character character, Stats stats, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts)
        {
            this.Add(ability, new AbilityModel(character, stats, ability, calcOpts, bossOpts));
        }
    }
}
