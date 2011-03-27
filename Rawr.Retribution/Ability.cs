using System;
using System.Collections.Generic;

namespace Rawr.Retribution
{
    public abstract class Ability<T> where T : TalentsBase
    {
        public Ability(Character character, Stats stats, AbilityType abilityType, DamageType damageType, bool hasGCD = true)
        {
            _character = character;
            _stats = stats;
            _talents = (T)character.CurrentTalents;
            AbilityType = abilityType;
            DamageType = damageType;

            HasGCD = hasGCD;
            if (hasGCD)
                _GCD = (AbilityType == AbilityType.Spell ? 1.5f / (1 + Stats.SpellHaste) : 1.5f);
            else
                _GCD = 0f;
        }

        #region Base
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
        #endregion

        #region DPS / CD
        public double UsagePerSec { get; set; }
        public double CritsPerSec()
        {
            return UsagePerSec * CT.ChanceToCrit;
        }
        public float GetDPS()
        {
            return (float)(AverageDamage * UsagePerSec);
        }

        protected float _Cooldown = 1f;
        public float Cooldown { get { return _Cooldown; }
                                set { _Cooldown = value; } }
        public bool HasGCD;
        protected float _GCD;
        public virtual float GCD { get { return _GCD; } }
        public virtual string GetGeneralOutput()
        {
            string fmtstring = "";
            if (HasGCD)
                fmtstring += "\n{0:F2} sec GCD";
            if (Cooldown != 1f)
                fmtstring += "\n{1:F2} sec Cooldown";
            if (Targets() != 1f)
                fmtstring += "\n{2:F2} Targets";
            if (TickCount() != 1f)
                fmtstring += "\n{3:F2} Ticks";

            if (fmtstring.Length > 0)
                return "General:" + string.Format(fmtstring, GCD, Cooldown, Targets(), TickCount());
            else
                return "";
        }

        protected float _AbilityDamage = 1f;
        public float AbilityDamage { get { return _AbilityDamage; } 
                                     set { _AbilityDamage = value * TickCount() * (Meteor ? 1f : Targets());
                                           HitDamage = _AbilityDamage * GetMulitplier(); } }
        protected float _HitDamage;
        public float HitDamage { get { return _HitDamage; }
                                 set { _HitDamage = value;
                                       AverageDamage = value * CT.CombatTableMultiplier(); } }
        protected float _AverageDamage;
        public virtual float AverageDamage { get { return _AverageDamage; }
                                             set { _AverageDamage = value; } }
        public virtual string GetDamageOutput()
        {
            string fmtString = "Damage:";
            string addString = "";
                
            if (TickCount() != 1f)
                addString += " / Tick";
            if (Targets() != 1f)
                addString += " / Target";
            
            fmtString += "\n{0:N0} Average Damage" + addString + 
                         "\n{1:N0} Average Hit" + addString;

            return string.Format(fmtString, (AverageDamage / TickCount() / Targets()), (HitDamage / TickCount() / Targets()));
        }
        public virtual float Targets() { return 1f; }
        public virtual float TickCount() { return 1f; }
        public bool Meteor = false;
        #endregion

        #region Multiplier
        public Dictionary<Multiplier, float> AbilityDamageMulitplier = new Dictionary<Multiplier, float>();
        public string AbilityDamageMultiplierOthersString = String.Empty;
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
            string Output = "Multiplier:";
            for (Multiplier multi = Multiplier.Armor; multi <= Multiplier.Others; multi++)
                if (AbilityDamageMulitplier.ContainsKey(multi) && (AbilityDamageMulitplier[multi] != 1f))
                {
                    Output += string.Format("\n{0:0.00} {1,-10}", AbilityDamageMulitplier[multi], multi);
                }
            if (AbilityDamageMultiplierOthersString != String.Empty) 
                Output = Output.Replace(Multiplier.Others.ToString(), AbilityDamageMultiplierOthersString);
            return Output + string.Format("\n{0:0.00} Total Multiplier", GetMulitplier());
        }
        #endregion

        public override string ToString()
        {
            string Output = GetGeneralOutput();
            return (Output.Length > 0 ? Output + "\n\n" : "") +
                   GetDamageOutput() + "\n\n" +
                   CT.ToString() + "\n\n" +
                   GetMultiplierOutput();
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