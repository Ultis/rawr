using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin {
    public class AbilityModel {
        private Ability Ability;
        private Character Character;
        private Stats Stats;
        private PaladinTalents Talents;
        private CalculationOptionsProtPaladin CalcOpts;
        private BossOptions BossOpts;

        public readonly AttackTable AttackTable;

        public string Name { get; private set; }
        public float Damage { get; private set; }
        public float Threat { get; private set; }
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
                    if (Character.OffHand == null || Character.OffHand.Type != ItemType.Shield || (Talents.Divinity == 0 && Talents.SealsOfThePure == 0 && Talents.EternalGlory == 0)) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = 3113.187994f + (0.419f * AP) + (0.21f * SP);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier)
                                * (Talents.GlyphOfFocusedShield ? 1.3f : 1.0f);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfWrath:
                    baseDamage = 4015.02439f + (0.039f * AP) + (0.117f * SP);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HolyWrath:
                    baseDamage = 2435.781339f + (0.61f * SP);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

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

                    baseDamage *= (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                * (1.0f - ArmorReduction);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfTheRighteous:
                    if (Character.MainHand == null || (Character.MainHand.Type != ItemType.OneHandAxe && Character.MainHand.Type != ItemType.OneHandMace && Character.MainHand.Type != ItemType.OneHandSword) || Talents.HammerOfTheRighteous == 0) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = Lookup.WeaponDamage(Character, Stats, true) * 0.3f;

                    baseDamage *= (1f + (Talents.Crusade * 0.1f) + (Talents.GlyphOfHammerOfTheRighteous ? 0.1f : 0f))
                                * (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                * (1.0f - ArmorReduction);

                    critMultiplier = 1.0f;
                    break;
                case Ability.HammerOfTheRighteousProc:
                    if (Character.MainHand == null || (Character.MainHand.Type != ItemType.OneHandAxe && Character.MainHand.Type != ItemType.OneHandMace && Character.MainHand.Type != ItemType.OneHandSword) || Talents.HammerOfTheRighteous == 0) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = 728.8813374f + (0.18f * AP);

                    baseDamage *= (1f + (Talents.Crusade * 0.1f) + (Talents.GlyphOfHammerOfTheRighteous ? 0.1f : 0f))
                                * (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.JudgementOfRighteousness:
                    baseDamage = 1f + (0.2f * AP) + (0.32f * SP);

                    baseDamage *= (1f + (Talents.WrathOfTheLightbringer * 0.5f) + (Talents.GlyphOfJudgement ? 0.1f : 0f))
                                * (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;
                case Ability.JudgementOfTruth:
                    {
                        float censureStacks = 5;

                        baseDamage = 1 + (0.223f * SP) + (0.142f * AP) * (1.0f + 0.1f * censureStacks);

                        baseDamage *= (1f + (Talents.WrathOfTheLightbringer * 0.5f) + (Talents.GlyphOfJudgement ? 0.1f : 0f))
                                    * (1.0f + Stats.BonusHolyDamageMultiplier);

                        critMultiplier = 1.0f;
                    }
                    break;
                case Ability.MeleeSwing:
                    baseDamage = Stats.WeaponDamage;

                    baseDamage *= (1.0f + Stats.BonusPhysicalDamageMultiplier)
                                * (1.0f - (Lookup.GlancingReduction(Character, targetLevel) * AttackTable.Glance))
                                * (1.0f - ArmorReduction);

                    critMultiplier = 1.0f;
                    break;
                case Ability.SealOfRighteousness:
                    baseDamage = Lookup.WeaponSpeed(Character, Stats) * ((0.022f * SP) + (0.011f * AP));

                    baseDamage *= (1.0f + 0.06f * Talents.SealsOfThePure)
                                * (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 0.0f;
                    break;
                case Ability.SealOfTruth:
                    {
                        float censureStacks = 5;

                        baseDamage = Stats.WeaponDamage * 0.018f * censureStacks;
                        baseDamage *= (1f + (0.01f * SP) + (0.0193f * AP));

                        baseDamage *= (1.0f + 0.06f * Talents.SealsOfThePure)
                                    * (1.0f + Stats.BonusHolyDamageMultiplier);

                        critMultiplier = 1.0f;
                        break;
                    }
                case Ability.ShieldOfTheRighteous:
                    if (Character.OffHand == null || Character.OffHand.Type != ItemType.Shield || Talents.ShieldOfTheRighteous == 0) {
                        Damage = 0f;
                        return;
                    }

                    baseDamage = 610.4895857f * (0.6f * AP);

                    baseDamage *= (1.0f + (Talents.GlyphOfShieldOfTheRighteous ? 0.1f : 0f))
                                * (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 1.0f;
                    break;

                /************
                 * DoTs
                 ************/
                case Ability.CensureTick:
                    {
                        float censureStacks = 5;

                        baseDamage = ((0.01f * SP) + (0.0193f * AP)) * censureStacks;

                        baseDamage *= (1.0f + 0.06f * Talents.SealsOfThePure)
                                    * (1.0f + Stats.BonusHolyDamageMultiplier);

                        critMultiplier = 1.0f;
                        break;
                    }
                case Ability.Consecration:
                    baseDamage = 81.32998299f + (0.027f * SP) + (0.027f * AP); // Per tick

                    duration = 10.0f * (1f + (Talents.GlyphOfConsecration ? 0.2f : 0f));

                    baseDamage *= duration
                                * (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 0.0f;
                    break;

                /************
                 * Defensive
                 ************/
                case Ability.RetributionAura:
                    baseDamage = 121.4802229f + (SP * 0.033f);

                    baseDamage *= (1.0f + Stats.BonusHolyDamageMultiplier);

                    critMultiplier = 0.0f;
                    break;
            }
            #endregion

            baseDamage *= (1.0f + Stats.BonusDamageMultiplier);

            #region Miss Chance, Avoidance Chance
            if (Lookup.IsSpell(Ability))
            {
                if (Ability == Ability.Consecration)
                    // Probability calculation, since each tick can be resisted individually.
                    baseDamage = Lookup.GetConsecrationTickChances(duration, baseDamage, AttackTable.Miss);
                else
                    // Missed spell attacks
                    baseDamage *= (1.0f - AttackTable.Miss);
            } else {
                // Avoidable attacks
                if (Lookup.IsAvoidable(Ability))
                    baseDamage *= (1.0f - AttackTable.AnyMiss);
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
                ArmorReduction = Lookup.EffectiveTargetArmorReduction(Character, Stats, BossOpts.Armor, BossOpts.Level);

            Name        = Lookup.Name(Ability);

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
