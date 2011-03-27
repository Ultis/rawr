using System;
using System.Collections.Generic;

namespace Rawr.Retribution
{
    public abstract class Ability<T> where T : TalentsBase
    {
        public Ability(Character character, Stats stats, AbilityType abilityType, DamageType damageType)
        {
            _character = character;
            _stats = stats;
            _talents = (T)character.CurrentTalents;
            AbilityType = abilityType;
            DamageType = damageType;

            _GCD = (AbilityType == AbilityType.Spell ? 1.5f / (1 + Stats.SpellHaste) : 1.5f);
        }

        protected Character _character;
        public Character Character { get { return _character; } }
        protected T _talents;
        public T Talents { get { return (T)_talents; } }
        protected Stats _stats;
        public Stats Stats { get { return _stats; } }

        public virtual BaseCombatTable CT { get; protected set; }

        public AbilityType AbilityType { get; set; }
        protected DamageType _damageType;
        public virtual DamageType DamageType
        {
            get { return _damageType; }
            set
            { 
                _damageType = value;
                switch (value)
                {
                    case DamageType.Physical:
                        AbilityDamageMulitplier[Multiplier.Physical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusPhysicalDamageMultiplier);
                        AbilityDamageMulitplier[Multiplier.Armor] = 1f - StatConversion.GetArmorDamageReduction(Character.Level, _character.BossOptions.Armor, Stats.TargetArmorReduction, Stats.ArmorPenetration); ;
                        AbilityDamageMulitplier[Multiplier.Others] = (1f + PaladinConstants.TWO_H_SPEC);
                        break;
                    case DamageType.Arcane:
                        AbilityDamageMulitplier[Multiplier.Magical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusArcaneDamageMultiplier);
                        break;
                    case DamageType.Fire:
                        AbilityDamageMulitplier[Multiplier.Magical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusFireDamageMultiplier);
                        break;
                    case DamageType.Frost:
                        AbilityDamageMulitplier[Multiplier.Magical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusFrostDamageMultiplier);
                        break;
                    case DamageType.Nature:
                        AbilityDamageMulitplier[Multiplier.Magical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusNatureDamageMultiplier);
                        break;
                    case DamageType.Shadow:
                        AbilityDamageMulitplier[Multiplier.Magical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusShadowDamageMultiplier);
                        break;
                    case DamageType.Holy:
                        AbilityDamageMulitplier[Multiplier.Magical] = (1f + Stats.BonusDamageMultiplier) * (1f + Stats.BonusHolyDamageMultiplier);
                        break;
                    case DamageType.NoDD:
                        AbilityDamageMulitplier[Multiplier.Magical] = 1f;
                        break;
                }
            }
        }

        public double UsagePerSec { get; set; }
        public double CritsPerSec()
        {
            return UsagePerSec * CT.ChanceToCrit;
        }

        protected float _Cooldown = 1f;
        public float Cooldown { get { return _Cooldown; }
                                set { _Cooldown = value; } }
        protected float _GCD = 1.5f;
        public virtual float GCD { get { return _GCD; } }

        protected float _AbilityDamage = 1f;
        public float AbilityDamage { get { return _AbilityDamage; } 
                                     set { _AbilityDamage = value;
                                           HitDamage = value * GetMulitplier(); } }
        protected float _HitDamage;
        public float HitDamage { get { return _HitDamage; }
                                 set { _HitDamage = value;
                                       AverageDamage = value * CT.CombatTableMultiplier() * Targets(); } }
        protected float _AverageDamage;
        public virtual float AverageDamage { get { return _AverageDamage; }
                                             set { _AverageDamage = value; } }

        public virtual float Targets() { return 1f; }
        public virtual float TickCount() { return 1f; }

        public float GetDPS()
        {
            return (float)(AverageDamage * UsagePerSec);
        }

        public Dictionary<Multiplier, float> AbilityDamageMulitplier = new Dictionary<Multiplier, float>();
        public virtual float GetMulitplier()
        {
            float multiplier = 1f;
            foreach (KeyValuePair<Multiplier, float> kvp in AbilityDamageMulitplier)
            {
                multiplier *= kvp.Value;
            }
            return multiplier;
        }
        protected string GetMultiplierOutput()
        {
            string Output = "\n\nMultiplier:";
            for (Multiplier multi = Multiplier.Armor; multi <= Multiplier.Others; multi++)
                if (AbilityDamageMulitplier.ContainsKey(multi) && (AbilityDamageMulitplier[multi] != 1f))
                {
                    Output += string.Format("\n{0:0.00} {1,-10}", AbilityDamageMulitplier[multi], multi);
                }
            return Output + string.Format("\n{0:0.00} Total Multiplier", GetMulitplier());
        }

        public override string ToString()
        {
            return string.Format("{0:0} Average Damage\n{1:0} Average Hit" + CT.ToString() + GetMultiplierOutput(), AverageDamage, HitDamage);
        }
    }


    public static class AbilityHelper
    {
        public static float BaseWeaponSpeed(Character character)
        {
            return (character.MainHand == null || character.MainHand.Speed == 0.0f) ? 3.5f : character.MainHand.Speed;
        }
        public static float WeaponSpeed(Character character, float Haste)
        {
            return BaseWeaponSpeed(character) / (1f + Haste);
        }
        public static float BaseWeaponDamage(ItemInstance MainHand)
        {
            return (MainHand == null) ? 371.5f : (MainHand.MinDamage + MainHand.MaxDamage) / 2f;
        }
        public static float WeaponDamage(Character character, float AttackPower, bool Normalized = false)
        {
            return BaseWeaponDamage(character.MainHand) + AttackPower * (Normalized ? 3.3f : BaseWeaponSpeed(character)) / 14f; ;
        }
    }
}