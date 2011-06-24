using System;
using System.Collections.Generic;
using System.Linq;

namespace Rawr.Retribution
{
    public abstract class Ability<T, S, C> where T : TalentsBase where S : Stats where C : ICalculationOptionBase
    {
        public Ability(string name, Character character, S stats, AbilityType abilityType, DamageType damageType, PLAYER_ROLES role, bool hasGCD = true, bool noMultiplier = false)
        {
            Meteor = false;
            _name = name;
            _character = character; 
            _stats = stats;
            _talents = (T)character.CurrentTalents;
            AbilityType = abilityType;
            DamageType = damageType;
            NoMultiplier = noMultiplier;
            Role = role;

            HasGCD = hasGCD;
            _normGCD = 1.5f + Latency;
            if (hasGCD)
                GCD = (AbilityType == AbilityType.Spell ? 1.5f / (1 + Stats.SpellHaste) : 1.5f) + Latency;
            else
                GCD = 0f;

            _triggers = new List<Ability<T,S,C>>();
        }

        #region Base
        protected string _name;
        public string Name { get { return _name; } }
        protected Character _character;
        public Character Character { get { return _character; } }
        protected T _talents;
        public T Talents { get { return _talents; } }
        protected S _stats;
        public S Stats { get { return _stats; } }
        public C CalcOps { get { return (C)_character.CalculationOptions; } }
        public PLAYER_ROLES Role;

        public virtual BaseCombatTable CT { get; protected set; }

        public AbilityType AbilityType { get; set; }
        protected DamageType _damageType;
        public virtual DamageType DamageType
        {
            get { return _damageType; }
            set
            { 
                _damageType = value;
                AbilityDamageMulitplier[Multiplier.General] = 1f + Stats.BonusDamageMultiplier;
                switch (value)
                {
                    case DamageType.Physical:
                        AbilityDamageMulitplier[Multiplier.Physical] = 1f + Stats.BonusPhysicalDamageMultiplier;
                        AbilityDamageMulitplier[Multiplier.Armor] = 1f - StatConversion.GetArmorDamageReduction(Character.Level, _character.BossOptions.Armor, Stats.TargetArmorReduction, Stats.ArmorPenetration);
                        break;
                    case DamageType.Arcane:
                        AbilityDamageMulitplier[Multiplier.Magical] = 1f + Stats.BonusArcaneDamageMultiplier;
                        break;
                    case DamageType.Fire:
                        AbilityDamageMulitplier[Multiplier.Magical] = 1f + Stats.BonusFireDamageMultiplier;
                        break;
                    case DamageType.Frost:
                        AbilityDamageMulitplier[Multiplier.Magical] = 1f + Stats.BonusFrostDamageMultiplier;
                        break;
                    case DamageType.Nature:
                        AbilityDamageMulitplier[Multiplier.Magical] = 1f + Stats.BonusNatureDamageMultiplier;
                        break;
                    case DamageType.Shadow:
                        AbilityDamageMulitplier[Multiplier.Magical] = 1f + Stats.BonusShadowDamageMultiplier;
                        break;
                    case DamageType.Holy:
                        AbilityDamageMulitplier[Multiplier.Magical] = 1f + Stats.BonusHolyDamageMultiplier;
                        break;
                }
            }
        }
        #endregion

        #region CD
        public float Latency = .1f;
        protected float _Cooldown = 1f;
        public float Cooldown { get { return _Cooldown; }
                                set { _Cooldown = value; } }
        public float CooldownWithLatency { get { return _Cooldown + Latency; } }

        public bool HasGCD;
        protected float _GCD;
        public virtual float GCD { get { return _GCD; } set { _GCDPerc = value / _normGCD; 
                                                              _GCD = value;  } }
        protected float _normGCD;
        protected double _GCDPerc;
        public virtual double GCDPercentage { get { return _GCDPerc; } }

        public float GetMaxNumberOfActivates(float fighttime)
        {
            return fighttime / CooldownWithLatency;
        }

        public virtual string GetGeneralOutput()
        {
            string fmtstring = "";
            if (HasGCD)
                fmtstring += "\n{0:F2} sec GCD";
            if (Cooldown != 1f)
                fmtstring += "\n{1:F2} sec Cooldown";
            if (AvgTargets != 1f)
                fmtstring += "\n{2:F2} Targets";
            if (TickCount != 1f)
                fmtstring += "\n{3:F2} Ticks";

            if (fmtstring.Length > 0)
                return "General:" + string.Format(fmtstring, GCD, Cooldown, AvgTargets, TickCount);
            return "";
        }
        #endregion

        #region DPS
        public double UsagePerSec { get; set; }
        public double CritsPerSec()
        {
            return UsagePerSec * CT.ChanceToCrit;
        }
        public float GetDPS()
        {
            return (float)(AverageDamageWithTriggers * UsagePerSec);
        }

        protected float _AbilityDamage = 1f;
        public float AbilityDamage { get { return _AbilityDamage; } 
                                     set { _AbilityDamage = value * TickCount * (Meteor ? 1f : AvgTargets);
                                           HitDamage = _AbilityDamage * GetMulitplier(); } }
        protected float _HitDamage;
        public float HitDamage { get { return _HitDamage; }
                                 set { _HitDamage = value;
                                       AverageDamage = value * CT.CombatTableMultiplier(); } }
        protected float _AverageDamage;
        public virtual float AverageDamage { get { return _AverageDamage; }
                                             set { _AverageDamage = value;
                                                   AverageDamageWithTriggers = _AverageDamage + GetAverageDamageFromTriggers(); } }
        public virtual string GetDamageOutput()
        {
            string fmtString = "Damage:";
            string addString = "";
                
            if (TickCount != 1f)
                addString += " / Tick";
            if (AvgTargets != 1f)
                addString += " / Target";
            if (_triggers.Count > 0)
                fmtString += "\n{0:N0} Average Damage incl Trigger" + addString;

            fmtString += "\n{1:N0} Average Damage" + addString + 
                         "\n{2:N0} Average Hit" + addString;

            return string.Format(fmtString, (AverageDamageWithTriggers / TickCount / AvgTargets), (AverageDamage / TickCount / AvgTargets), (HitDamage / TickCount / AvgTargets));
        }
        public float AvgTargets = 1f;
        private void CalcAverageTarget()
        {
            if (_MaxTargets != -1)
            {
                if (Character.BossOptions.MultiTargs && Character.BossOptions.Targets != null && Character.BossOptions.Targets.Count > 0)
                {
                    float value = 0;
                    foreach (TargetGroup tg in Character.BossOptions.Targets)
                    {
                        if (!tg.Validate) { continue; } // Bad one, skip it
                        if (!tg.AffectsRole[Role]) { continue; } // Doesn't apply to us
                        float upTime = ((Character.BossOptions.BerserkTimer / tg.Frequency) * (tg.Duration / 1000f)) / Character.BossOptions.BerserkTimer
                                       * tg.Chance // Chance it happens to us
                                       * tg.FightUptimePercent; // The Phase uptime
                        value += (Math.Min(10 - (tg.NearBoss ? 1 : 0), Math.Min(_MaxTargets - (tg.NearBoss ? 1 : 0), tg.NumTargs - (tg.NearBoss ? 1 : 0) + Stats.BonusTargets))) * upTime;
                    }
                    AvgTargets = 1f + value;
                }
                else { AvgTargets = 1f; }
            }
            else { AvgTargets = 1f; }
        }
        protected int _MaxTargets = 1;
        public int MaxTargets { get { return _MaxTargets; } 
                                set { _MaxTargets = value; CalcAverageTarget(); } }
        protected float _TickCount = 1f;
        public float TickCount { get { return _TickCount; } 
                                 set { _TickCount = value; } }
        public bool Meteor;
        #endregion

        #region Multiplier
        public bool NoMultiplier;
        public Dictionary<Multiplier, float> AbilityDamageMulitplier = new Dictionary<Multiplier, float>();
        public string AbilityDamageMultiplierOthersString = String.Empty;
        public virtual float GetMulitplier()
        {
            float multiplier = 1f;
            if (!NoMultiplier) 
                foreach (KeyValuePair<Multiplier, float> kvp in AbilityDamageMulitplier)
                    multiplier *= kvp.Value;
            return multiplier;
        }
        protected string GetMultiplierOutput()
        {
            if (!NoMultiplier)
            {
                string Output = "\n\nMultiplier:";
                for (Multiplier multi = Multiplier.General; multi <= Multiplier.Others; multi++)
                    if (AbilityDamageMulitplier.ContainsKey(multi) && (AbilityDamageMulitplier[multi] != 1f))
                    {
                        Output += string.Format("\n{0:0.00} {1,-10}", AbilityDamageMulitplier[multi], multi);
                    }
                if (AbilityDamageMultiplierOthersString != String.Empty)
                    Output = Output.Replace(Multiplier.Others.ToString(), AbilityDamageMultiplierOthersString);
                return Output + string.Format("\n{0:0.00} Total Multiplier", GetMulitplier());
            }
            return "";
        }
        #endregion

        #region Triggered Abilities
        protected List<Ability<T, S, C>> _triggers;
        public void AddTrigger(Ability<T, S, C> ability) {
            _triggers.Add(ability);
            _AverageDamageWithTriggers += ability.AverageDamageWithTriggers;
        }
        protected float GetAverageDamageFromTriggers()
        {
            return _triggers.Sum(ability => ability.AverageDamageWithTriggers);
        }

        protected float _AverageDamageWithTriggers;
        public float AverageDamageWithTriggers { get { return _AverageDamageWithTriggers; }
                                                 set { _AverageDamageWithTriggers = value; } }
        public string GetTriggerOutput()
        {
            if (_triggers.Count == 0)
                return "";

            string abilityTooltip = "\n\nTriggers:";
            foreach (Ability<T, S, C> ability in _triggers)
                abilityTooltip += string.Format("\n{0:N0} Damage from {1}", ability.AverageDamage, ability.Name) + ability.GetTriggerOutput();
            return abilityTooltip;
        }
        #endregion

        public override string ToString()
        {
            string Output = GetGeneralOutput();
            return (Output.Length > 0 ? Output + "\n\n" : "") +
                   GetDamageOutput() + "\n\n" +
                   CT.ToString() + 
                   GetMultiplierOutput() +
                   GetTriggerOutput();
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
            return BaseWeaponDamage(character.MainHand) + AttackPower * (Normalized ? 3.3f : BaseWeaponSpeed(character)) / 14f;
        }
    }
}