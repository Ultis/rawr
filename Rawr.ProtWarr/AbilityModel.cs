using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Rawr.ProtWarr
{
    public class AbilityModel
    {
        private Player Player;

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
            float critMultiplier    = 1.0f + Lookup.BonusCritMultiplier(Player, Ability);

            switch (Ability)
            {
                // White Damage
                case Ability.None:
                    baseDamage = Lookup.WeaponDamage(Player, false);
                    DamageMultiplier *= (1f + Player.Stats.BonusWhiteDamageMultiplier);
                    break;
                case Ability.Cleave:
                    baseDamage = 6.0f + (Player.Stats.AttackPower * 0.562f);
                    DamageMultiplier *= (1.0f + Player.Talents.Thunderstruck * 0.03f);
                    break;
                case Ability.ConcussionBlow:
                    baseDamage = Player.Stats.AttackPower * 0.75f;
                    break;
                case Ability.DeepWounds:
                    baseDamage = Lookup.WeaponDamage(Player, false) * (Player.Talents.DeepWounds * 0.16f);
                    DamageMultiplier *= (1.0f + Player.Stats.BonusBleedDamageMultiplier);
                    ArmorReduction = 0.0f;
                    break;
                case Ability.Devastate:
                    // Assumes 3 stacks of Sunder Armor debuff
                    baseDamage = (Lookup.WeaponDamage(Player, true) * 1.5f) + (336.0f * 3.0f);
                    if (Player.Talents.GlyphOfDevastate)
                        DamageMultiplier *= 1.05f;
                    DamageMultiplier *= (1.0f + Player.Stats.BonusDevastateDamageMultiplier) * (1.0f + 0.05f * Player.Talents.WarAcademy);
                    break;
                case Ability.HeroicStrike:
                    baseDamage = 8.0f + (Player.Stats.AttackPower * 0.6f);
                    break;
                case Ability.HeroicThrow:
                    baseDamage = 12.0f + (Player.Stats.AttackPower * 0.5f);
                    break;
                case Ability.Rend:
                    baseDamage = 525.0f + (Lookup.WeaponDamage(Player, false) * 1.5f);
                    DamageMultiplier *= (1.0f + Player.Stats.BonusBleedDamageMultiplier) * (1.0f + Player.Talents.Thunderstruck * 0.03f);
                    ArmorReduction = 0.0f;
                    break;
                case Ability.Revenge:
                    baseDamage = 1798.0f + (Player.Stats.AttackPower * 0.3105f);
                    if (Player.Talents.GlyphOfRevenge)
                        DamageMultiplier *= 1.1f;
                    DamageMultiplier *= (1.0f + Player.Talents.ImprovedRevenge * 0.3f);
                    break;
                case Ability.ShieldSlam:
                    baseDamage = 2779.0f + (Player.Stats.AttackPower * 0.6f);
                    if (Player.Talents.GlyphOfShieldSlam)
                        DamageMultiplier *= 1.1f;
                    if (Player.Options.UseShieldBlock)
                        DamageMultiplier *= (1.0f + (0.5f * Player.Talents.HeavyRepercussions * (10.0f / Player.Options.ShieldBlockInterval)));
                    DamageMultiplier *= (1.0f + Player.Stats.BonusShieldSlamDamageMultiplier);
                    break;
                case Ability.Shockwave:
                    baseDamage = Player.Stats.AttackPower * 0.75f;
                    DamageMultiplier *= (1.0f + Player.Stats.BonusShockwaveDamageMultiplier);
                    break;
                case Ability.Slam:
                    baseDamage = 538.75f + (Lookup.WeaponDamage(Player, false) * 1.25f);
                    break;
                case Ability.ThunderClap:
                    baseDamage = 302.0f + (Player.Stats.AttackPower * 0.12f);
                    DamageMultiplier *= (1.0f + Player.Talents.Thunderstruck * 0.03f); 
                    break;
                case Ability.VictoryRush:
                    baseDamage = Player.Stats.AttackPower * 0.56f;
                    DamageMultiplier *= (1.0f + Player.Talents.WarAcademy * 0.05f);
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
                AttackTable.Glance * Lookup.GlancingReduction(Player);

            Damage = baseDamage;
        }

        private void CalculateThreat()
        {
            // Base threat is always going to be the damage of the ability, if it is damaging
            float abilityThreat = Damage;

            switch (Ability)
            {
                case Ability.Cleave:
                    abilityThreat += 30.0f;
                    break;
                case Ability.HeroicStrike:
                    abilityThreat += 28.0f;
                    break;
                case Ability.HeroicThrow:
                    abilityThreat *= 1.5f;
                    break;
                case Ability.Revenge:
                    abilityThreat += 7.0f;
                    break;
                case Ability.ShieldBash:
                    abilityThreat += 36.0f;
                    break;
                case Ability.ShieldSlam:
                    abilityThreat += 231.0f;
                    break;
                case Ability.Slam:
                    abilityThreat += 140.0f;
                    break;
                case Ability.SunderArmor:
                    abilityThreat += 395.0f + (Player.Stats.AttackPower * 0.05f);
                    break;
            }

            abilityThreat *= Lookup.StanceThreatMultipler(Player);

            Threat = abilityThreat;
        }

        public AbilityModel(Player player, Ability ability)
        {
            Player      = player;
            Ability     = ability;
            AttackTable = new AttackTable(Player, Ability);

            Name                = Lookup.Name(Ability);
            ArmorReduction      = Lookup.TargetArmorReduction(Player);
            DamageMultiplier    = Lookup.StanceDamageMultipler(Player);
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

        public void Add(Ability ability, Player player)
        {
            this.Add(new AbilityModel(player, ability));
        }
    }
}
