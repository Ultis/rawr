using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Reflection.Emit;

namespace Rawr
{
    enum AdditiveStat:int
    {
        Agility,
        AllResist,
        ArcaneBlastBonus,
        ArcaneResistance,
        Armor,
        ArmorPenetration,
        AttackPower,
        AverageAgility,
        BlockRating,
        BlockValue,
        Bloodlust,
        BloodlustProc,
        BonusLacerateDamage,
        BonusManaGem,
        BonusMangleBearDamage,
        BonusMangleCatDamage,
        BonusRipDamagePerCPPerTick,
        BonusShredDamage,
        CatFormStrength,
        CritRating,
        CrushChanceReduction,
        DefenseRating,
        DodgeRating,
        DrumsOfBattle,
        DrumsOfWar,
        EvocationExtension,
        ExpertiseRating,
        ExposeWeakness,
        FireResistance,
        FrostResistance,
        HasteRating,
        Healing,
        Health,
        HitRating,
        Hp5,
        IdolCritRating,
        InnervateCooldownReduction,
        Intellect,
        LightningCapacitorProc,
        LotPCritRating,
        MageSpellCrit,
        Mana,
        ManaRestorePerCast,
        ManaRestorePerHit,
        MangleCostReduction,
        Miss,
        MoonfireDmg,
        MoonfireExtension,
        Mp5,
        Mp5OnCastFor20SecOnUse2Min,
        NatureResistance,
        ParryRating,
        Resilience,
        ScopeDamage, 
        ShadowResistance,
        ShatteredSunAcumenProc,
        ShatteredSunMightProc,
        SpellArcaneDamageRating,
        SpellCombatManaRegeneration,
        SpellCritRating,
        SpellDamageFor10SecOnCrit_20_45,
        SpellDamageFor10SecOnHit_10_45,
        SpellDamageFor10SecOnHit_5,
        SpellDamageFor10SecOnResist,
        SpellDamageFor15SecOnCrit_20_45,
        SpellDamageFor15SecOnManaGem,
        SpellDamageFor15SecOnUse90Sec,
        SpellDamageFor20SecOnUse2Min,
        SpellDamageFor6SecOnCrit,
        SpellDamageFromIntellectPercentage,
        SpellDamageFromSpiritPercentage,
        SpellDamageRating,
        SpellFireDamageRating,
        SpellFrostCritRating,
        SpellFrostDamageRating,
        SpellHasteFor20SecOnUse2Min,
        SpellHasteFor5SecOnCrit_50,
        SpellHasteFor6SecOnCast_15_45,
        SpellHasteFor6SecOnHit_10_45,
        SpellHasteRating,
        SpellHitRating,
        SpellNatureDamageRating,
        SpellPenetration,
        SpellShadowDamageRating,
        Spirit,
        Stamina,
        StarfireBonusWithDot,
        StarfireCritChance,
        StarfireDmg,
        Strength,
        TerrorProc,
        TimbalsProc,
        UnseenMoonDamageBonus,
        WeaponDamage,
        WindfuryAPBonus,
        WrathDmg
    }

    enum MultiplicativeStat : int
    {
        BonusMangleBearThreat,
        BonusAgilityMultiplier,
        BonusArcaneSpellPowerMultiplier,
        BonusArmorMultiplier,
        BonusAttackPowerMultiplier,
        BonusCritMultiplier,
        BonusCrusaderStrikeDamageMultiplier,
        BonusFireSpellPowerMultiplier,
        BonusFrostSpellPowerMultiplier,
        BonusIntellectMultiplier,
        BonusMageNukeMultiplier,
        BonusNatureSpellPowerMultiplier,
        BonusPhysicalDamageMultiplier,
        BonusRipDamageMultiplier,
        BonusSpellCritMultiplier,
        BonusSpellPowerMultiplier,
        BonusSpiritMultiplier,
        BonusStaminaMultiplier,
        BonusStrengthMultiplier,
        BonusSwipeDamageMultiplier,
        BonusShadowSpellPowerMultiplier
    }

    enum NonStackingStat : int
    {
        BonusManaPotion,
    }

    /// <summary>
    /// A Stats object represents a collection of stats on an object, such as an Item, Enchant, or Buff.
    /// </summary>
    /// 

    [Serializable]
    public class Stats
    {
        private float[] _rawAdditiveData = new float[Enum.GetValues(typeof(AdditiveStat)).Length];
        private float[] _rawMultiplicativeData = new float[Enum.GetValues(typeof(MultiplicativeStat)).Length];
        private float[] _rawNoStackData = new float[Enum.GetValues(typeof(NonStackingStat)).Length];
        /// <summary>
        /// The properties for each stat. In order to add additional stats for Rawr to track,
        /// first add properties here, for each stat. Apply a Category attribute to assign it to
        /// a category in the item editor. Optionally, apply a DisplayName attribute. If no
        /// DisplayName attribute is applied, the property name will be used, with spaces between
        /// each word, detected by capitalization (AttackPower becomes "Attack Power"). If the
        /// stat is a multiplier, add the Multiplicative attribute.
        /// </summary>
        #region Stat Properties

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Armor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Armor]; }
            set { _rawAdditiveData[(int)AdditiveStat.Armor] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Health
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Health]; }
            set { _rawAdditiveData[(int)AdditiveStat.Health] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Mana
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mana]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mana] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Agility
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Agility]; }
            set { _rawAdditiveData[(int)AdditiveStat.Agility] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Stamina
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Stamina]; }
            set { _rawAdditiveData[(int)AdditiveStat.Stamina] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float AttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.AttackPower] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Strength
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Strength]; }
            set { _rawAdditiveData[(int)AdditiveStat.Strength] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float WeaponDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WeaponDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.WeaponDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float ScopeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ScopeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.ScopeDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Penetration")]
        public float ArmorPenetration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArmorPenetration]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArmorPenetration] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Intellect
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Intellect]; }
            set { _rawAdditiveData[(int)AdditiveStat.Intellect] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Spirit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Spirit]; }
            set { _rawAdditiveData[(int)AdditiveStat.Spirit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Frost Res")]
        public float FrostResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FrostResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.FrostResistance] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Nature Res")]
        public float NatureResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.NatureResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.NatureResistance] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Fire Res")]
        public float FireResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FireResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.FireResistance] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Shadow Res")]
        public float ShadowResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShadowResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShadowResistance] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Arcane Res")]
        public float ArcaneResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArcaneResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArcaneResistance] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Resist")]
        public float AllResist
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AllResist]; }
            set { _rawAdditiveData[(int)AdditiveStat.AllResist] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Crit")]
        public float SpellCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCritRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Frost Crit")]
        public float SpellFrostCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellFrostCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellFrostCritRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Damage")]
        public float SpellDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Shadow Damage")]
        public float SpellShadowDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellShadowDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellShadowDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Fire Damage")]
        public float SpellFireDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellFireDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellFireDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Frost Damage")]
        public float SpellFrostDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellFrostDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellFrostDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Arcane Damage")]
        public float SpellArcaneDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellArcaneDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellArcaneDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Nature Damage")]
        public float SpellNatureDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellNatureDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellNatureDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Penetration")]
        public float SpellPenetration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPenetration]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPenetration] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Hit")]
        public float SpellHitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHitRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        public float Healing
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Healing]; }
            set { _rawAdditiveData[(int)AdditiveStat.Healing] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Haste")]
        public float SpellHasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteRating] = value; }
        }

        // percentage mana generation while casting
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Combat Mana Regeneration")]
        public float SpellCombatManaRegeneration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCombatManaRegeneration]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCombatManaRegeneration] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Crit")]
        public float CritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Hit")]
        public float HitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.HitRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Dodge")]
        public float DodgeRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DodgeRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.DodgeRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Parry")]
        public float ParryRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ParryRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ParryRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Block")]
        public float BlockRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BlockRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.BlockRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Block Value")]
        public float BlockValue
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BlockValue]; }
            set { _rawAdditiveData[(int)AdditiveStat.BlockValue] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Defense")]
        public float DefenseRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DefenseRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.DefenseRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        public float Resilience
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Resilience]; }
            set { _rawAdditiveData[(int)AdditiveStat.Resilience] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Expertise")]
        public float ExpertiseRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExpertiseRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExpertiseRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Haste")]
        public float HasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Mana per 5 sec")]
        public float Mp5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mp5]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mp5] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Health per 5 sec")]
        public float Hp5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Hp5]; }
            set { _rawAdditiveData[(int)AdditiveStat.Hp5] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float BloodlustProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BloodlustProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.BloodlustProc] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float TerrorProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TerrorProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.TerrorProc] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("% Miss")]
        public float Miss
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Miss]; }
            set { _rawAdditiveData[(int)AdditiveStat.Miss] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusShredDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusShredDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusShredDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusLacerateDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusLacerateDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusLacerateDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusMangleCatDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusMangleCatDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusMangleCatDamage] = value; }
        }
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusMangleBearDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusMangleBearDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusMangleBearDamage] = value; }
        }


        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Bonus Rip Damage Per Combo Point Per Tick")]
        public float BonusRipDamagePerCPPerTick
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRipDamagePerCPPerTick]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRipDamagePerCPPerTick] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Windfury")]
        public float WindfuryAPBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WindfuryAPBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.WindfuryAPBonus] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestorePerHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestorePerHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestorePerHit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestorePerCast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCast]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCast] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MangleCatCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MangleCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.MangleCostReduction] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ExposeWeakness
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExposeWeakness]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExposeWeakness] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float Bloodlust
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Bloodlust]; }
            set { _rawAdditiveData[(int)AdditiveStat.Bloodlust] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float DrumsOfWar
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DrumsOfWar]; }
            set { _rawAdditiveData[(int)AdditiveStat.DrumsOfWar] = value; }
        }

        // threat dealt is damage * (1 + ThreatMultiplier)
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ThreatMultiplier
        {
            get;
            set;
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float DrumsOfBattle
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DrumsOfBattle]; }
            set { _rawAdditiveData[(int)AdditiveStat.DrumsOfBattle] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ArcaneBlastBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArcaneBlastBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArcaneBlastBonus] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float SpellDamageFromIntellectPercentage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFromIntellectPercentage]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFromIntellectPercentage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float SpellDamageFromSpiritPercentage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFromSpiritPercentage]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFromSpiritPercentage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage Increase for 6 sec on Crit")]
        public float SpellDamageFor6SecOnCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor6SecOnCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor6SecOnCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (50% 5 sec/Crit)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor5SecOnCrit_50
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor5SecOnCrit_50]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor5SecOnCrit_50] = value; }
        }

        // 15% change, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (15% 6 sec/Cast)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor6SecOnCast_15_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnCast_15_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnCast_15_45] = value; }
        }

        // 10% change, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (10% 6 sec/Hit)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor6SecOnHit_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnHit_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnHit_10_45] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec/Resist)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnResist
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnResist]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnResist] = value; }
        }

        // trinket effect, does not sum up over gear, 2 trinkets with this effect is not equivalent to 1 trinket with double effect
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor20SecOnUse2Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (15 sec/1.5 min)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnUse90Sec
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor15SecOnUse90Sec]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor15SecOnUse90Sec] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor20SecOnUse2Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mp5 on Cast (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float Mp5OnCastFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mp5OnCastFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mp5OnCastFor20SecOnUse2Min] = value; }
        }

        // 5% chance, no cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnHit_5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnHit_5]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnHit_5] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float BonusManaGem
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusManaGem]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusManaGem] = value; }
        }

 
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (15 sec/Gem)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnManaGem
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor15SecOnManaGem]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor15SecOnManaGem] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnHit_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnHit_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnHit_10_45] = value; }
        }

        // 20% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (15 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnCrit_20_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor15SecOnCrit_20_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor15SecOnCrit_20_45] = value; }
        }

        // 20% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor10SecOnCrit_20_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnCrit_20_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnCrit_20_45] = value; }
        }

        // Starfire idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Starfire damage bonus")]
        [Category("Equipment Procs")]
        public float StarfireDmg
        {
            get { return _rawAdditiveData[(int)AdditiveStat.StarfireDmg]; }
            set { _rawAdditiveData[(int)AdditiveStat.StarfireDmg] = value; }
        }

        // Moonfire idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Moonfire damage bonus")]
        [Category("Equipment Procs")]
        public float MoonfireDmg
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MoonfireDmg]; }
            set { _rawAdditiveData[(int)AdditiveStat.MoonfireDmg] = value; }
        }

        // Wrath idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Wrath damage bonus")]
        [Category("Equipment Procs")]
        public float WrathDmg
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WrathDmg]; }
            set { _rawAdditiveData[(int)AdditiveStat.WrathDmg] = value; }
        }

        // Moonkin Aura idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Moonkin Aura bonus")]
        [Category("Equipment Procs")]
        public float IdolCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.IdolCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.IdolCritRating] = value; }
        }

        // Moonkin 4-piece T4 bonus
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float InnervateCooldownReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.InnervateCooldownReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.InnervateCooldownReduction] = value; }
        }

        // Moonkin 4-piece T5 bonus
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float StarfireBonusWithDot
        {
            get { return _rawAdditiveData[(int)AdditiveStat.StarfireBonusWithDot]; }
            set { _rawAdditiveData[(int)AdditiveStat.StarfireBonusWithDot] = value; }
        }

        // Moonkin 2-piece T6 bonus
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MoonfireExtension
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MoonfireExtension]; }
            set { _rawAdditiveData[(int)AdditiveStat.MoonfireExtension] = value; }
        }
        // Moonkin 4-piece T6 bonus
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float StarfireCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.StarfireCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.StarfireCritChance] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float EvocationExtension
        {
            get { return _rawAdditiveData[(int)AdditiveStat.EvocationExtension]; }
            set { _rawAdditiveData[(int)AdditiveStat.EvocationExtension] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float LightningCapacitorProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningCapacitorProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningCapacitorProc] = value; }
        }
        
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("LotP Crit")]
        public float LotPCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LotPCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.LotPCritRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MageSpellCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageSpellCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageSpellCrit] = value; }
        }

        // Unseen Moon idol
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec on Moonfire)")]
        [Category("Equipment Procs")]
        public float UnseenMoonDamageBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.UnseenMoonDamageBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.UnseenMoonDamageBonus] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ShatteredSunMightProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShatteredSunMightProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShatteredSunMightProc] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float CrushChanceReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CrushChanceReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.CrushChanceReduction] = value; }
        }


        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Strength in Cat Form")]
        public float CatFormStrength
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CatFormStrength]; }
            set { _rawAdditiveData[(int)AdditiveStat.CatFormStrength] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Time average Agility")]
        public float AverageAgility
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AverageAgility]; }
            set { _rawAdditiveData[(int)AdditiveStat.AverageAgility] = value; }
        }

        [DisplayName("Shattered Sun Caster Neck proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ShatteredSunAcumenProc
        { 
            get { return _rawAdditiveData[(int)AdditiveStat.ShatteredSunAcumenProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShatteredSunAcumenProc] = value; }
        }

        [DisplayName("Timbal's Focusing Crystal proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float TimbalsProc 
        { 
            get { return _rawAdditiveData[(int)AdditiveStat.TimbalsProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.TimbalsProc] = value; }
        }


#endregion

#region MultiplicativeStats
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% CStrike Dmg")]
        public float BonusCrusaderStrikeDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCrusaderStrikeDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCrusaderStrikeDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Mangle (Bear) Threat")]
        public float BonusMangleBearThreat
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleBearThreat]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleBearThreat] = value; }
        }


        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        public float BonusMageNukeMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMageNukeMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMageNukeMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Agility")]
        public float BonusAgilityMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusAgilityMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusAgilityMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Strength")]
        public float BonusStrengthMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusStrengthMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusStrengthMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Stamina")]
        public float BonusStaminaMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusStaminaMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusStaminaMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Int")]
        public float BonusIntellectMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusIntellectMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusIntellectMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Armor")]
        public float BonusArmorMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusArmorMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusArmorMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% AP")]
        public float BonusAttackPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusAttackPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusAttackPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% SP")]
        public float BonusSpellPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Fire Damage")]
        public float BonusFireSpellPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFireSpellPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFireSpellPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Shadow Damage")]
        public float BonusShadowSpellPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusShadowSpellPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusShadowSpellPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Arcane Damage")]
        public float BonusArcaneSpellPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusArcaneSpellPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusArcaneSpellPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Nature Damage")]
        public float BonusNatureSpellPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusNatureSpellPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusNatureSpellPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Frost Damage")]
        public float BonusFrostSpellPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostSpellPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostSpellPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Spirit")]
        public float BonusSpiritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpiritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpiritMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Crit Dmg")]
        public float BonusCritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Spell Crit Dmg")]
        public float BonusSpellCritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellCritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellCritMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Rip Dmg")]
        public float BonusRipDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRipDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRipDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Swipe Dmg")]
        public float BonusSwipeDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSwipeDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSwipeDamageMultiplier] = value; }
        }


        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Physical Dmg")]
        public float BonusPhysicalDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPhysicalDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPhysicalDamageMultiplier] = value; }
        }

        #endregion

#region NoStackStats
        [System.ComponentModel.DefaultValueAttribute(0f)]
<<<<<<< .mine
=======
        [DisplayName("Spell Damage (10 sec on Moonfire)")]
>>>>>>> .r17541
        [Category("Equipment Procs")]
        public float BonusManaPotion
        {
            get { return _rawNoStackData[(int)NonStackingStat.BonusManaPotion]; }
            set { _rawNoStackData[(int)NonStackingStat.BonusManaPotion] = value; }
        }


<<<<<<< .mine
#endregion
=======
		[System.ComponentModel.DefaultValueAttribute(0f)]
        public float CrushChanceReduction { get { return _rawData[109]; } set { _rawData[109] = value; } }
>>>>>>> .r17541

<<<<<<< .mine
        /// <summary>
        /// Adds together two stats, when using a + operator. When adding additional stats for
        /// Rawr to track, after adding the stat property, also add a line for it to this method,
        /// to properly combine the stat, as appropriate.
        /// </summary>
        /// <param name="a">The first Stats object to combine.</param>
        /// <param name="b">The second Stats object to combine.</param>
        /// <returns>The combined Stats object.</returns>
        public static Stats operator +(Stats a, Stats b)
        {
            Stats c = new Stats();

=======
        [DisplayName("Shattered Sun Caster Neck proc")]
        [Category("Equipment Procs")]
>>>>>>> .r17541
<<<<<<< .mine
            int count = c._rawAdditiveData.Length;
            for (int i = 0; i < count; i++)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] + b._rawAdditiveData[i];
            }
            count = c._rawMultiplicativeData.Length;
            for (int i = 0; i < count; i++)
            {
                c._rawMultiplicativeData[i] = (1+a._rawMultiplicativeData[i]) * (1+b._rawMultiplicativeData[i])-1;
            }
            count = c._rawNoStackData.Length;
            for (int i = 0; i < count; i++)
            {
                c._rawNoStackData[i] = Math.Max(a._rawNoStackData[i], b._rawNoStackData[i]);
            }
            return c;
        }
=======
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ShatteredSunAcumenProc { get { return _rawData[110]; } set { _rawData[110] = value; } }
>>>>>>> .r17541

<<<<<<< .mine
        public bool Equals(Stats other)
        {
            return this == other;
        }
        public ArrayUtils.CompareResult CompareTo(Stats other)
        {
            if (ReferenceEquals(other, null))
                return 0;
            return ArrayUtils.And(ArrayUtils.AllCompare(this._rawAdditiveData, other._rawAdditiveData), ArrayUtils.And(
                ArrayUtils.AllCompare(this._rawMultiplicativeData, other._rawMultiplicativeData),
                ArrayUtils.AllCompare(this._rawNoStackData, other._rawNoStackData)));
        }
        //int IComparable.CompareTo(object other)
        //{
        //    return CompareTo(other as Stats);
        //}
=======
        [DisplayName("Timbal's Focusing Crystal proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float TimbalsProc { get { return _rawData[111]; } set { _rawData[111] = value; } }
>>>>>>> .r17541

        public override int GetHashCode()
        {
            return _rawAdditiveData.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this == (obj as Stats);
        }
        public static bool operator ==(Stats x, Stats y)
        {
            if (ReferenceEquals(x, y) || (ReferenceEquals(x, null) && ReferenceEquals(y, null)))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            return ArrayUtils.AllEqual(x._rawAdditiveData, y._rawAdditiveData) &&
                ArrayUtils.AllEqual(x._rawMultiplicativeData, y._rawMultiplicativeData) &&
                ArrayUtils.AllEqual(x._rawNoStackData, y._rawNoStackData);
        }
        public static bool operator !=(Stats x, Stats y)
        {
            return !(x == y);
        }
        public static bool operator >(Stats x, Stats y)
        {
            return AllCompare(x, y, ArrayUtils.CompareOption.GreaterThan);
        }
        public static bool operator >=(Stats x, Stats y)
        {
            return AllCompare(x, y, ArrayUtils.CompareOption.GreaterThan | ArrayUtils.CompareOption.Equal);
        }
        public static bool operator <(Stats x, Stats y)
        {
            return AllCompare(x, y, ArrayUtils.CompareOption.LessThan);
        }
        public static bool operator <=(Stats x, Stats y)
        {
            return AllCompare(x, y, ArrayUtils.CompareOption.LessThan | ArrayUtils.CompareOption.Equal);
        }
        private static bool AllCompare(Stats x, Stats y, ArrayUtils.CompareOption comparison)
        {
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                throw new ArgumentNullException();
            return ArrayUtils.AllCompare(x._rawAdditiveData, y._rawAdditiveData, comparison) 
                && ArrayUtils.AllCompare(x._rawMultiplicativeData, y._rawMultiplicativeData, comparison)  
                && ArrayUtils.AllCompare(x._rawNoStackData, y._rawNoStackData, comparison) ;
        }

<<<<<<< .mine
        //with hands held high into the sky so blue
        public Stats()
=======
		/// <summary>
		/// Adds together two stats, when using a + operator. When adding additional stats for
		/// Rawr to track, after adding the stat property, also add a line for it to this method,
		/// to properly combine the stat, as appropriate.
		/// </summary>
		/// <param name="a">The first Stats object to combine.</param>
		/// <param name="b">The second Stats object to combine.</param>
		/// <returns>The combined Stats object.</returns>
		public static Stats operator +(Stats a, Stats b)
		{
			return new Stats()
			{
				//NOTE: This is hard-coded, not using reflection, due to speed and maintainability.
				//GetValue and SetValue via reflection are *extremely* slow, and cause noticable lag in the app.
				//We also tried at one point to dynamically generate this code at runtime, which worked, but was
				//very complex and not maintainable by anyone who didn't already know wtf it was doing. So,
				//we're back to just hard-coding it, which isn't that big of a deal.
				Armor = a.Armor + b.Armor,
				Health = a.Health + b.Health,
				Agility = a.Agility + b.Agility,
				Stamina = a.Stamina + b.Stamina,
				AttackPower = a.AttackPower + b.AttackPower,
				Strength = a.Strength + b.Strength,
				Intellect = a.Intellect + b.Intellect,
				Spirit = a.Spirit + b.Spirit,
				WeaponDamage = a.WeaponDamage + b.WeaponDamage,
				ArmorPenetration = a.ArmorPenetration + b.ArmorPenetration,
				FrostResistance = a.FrostResistance + b.FrostResistance,
				NatureResistance = a.NatureResistance + b.NatureResistance,
				FireResistance = a.FireResistance + b.FireResistance,
				ShadowResistance = a.ShadowResistance + b.ShadowResistance,
				ArcaneResistance = a.ArcaneResistance + b.ArcaneResistance,
				AllResist = a.AllResist + b.AllResist,
				CritRating = a.CritRating + b.CritRating,
				LotPCritRating = a.LotPCritRating + b.LotPCritRating,
				HitRating = a.HitRating + b.HitRating,
				DodgeRating = a.DodgeRating + b.DodgeRating,
				ParryRating = a.ParryRating + b.ParryRating,
				BlockRating = a.BlockRating + b.BlockRating,
                BlockValue = a.BlockValue + b.BlockValue,
				DefenseRating = a.DefenseRating + b.DefenseRating,
				Resilience = a.Resilience + b.Resilience,
				ExpertiseRating = a.ExpertiseRating + b.ExpertiseRating,
				HasteRating = a.HasteRating + b.HasteRating,
				Mp5 = a.Mp5 + b.Mp5,
                Hp5 = a.Hp5 + b.Hp5,
				BloodlustProc = a.BloodlustProc + b.BloodlustProc,
				TerrorProc = a.TerrorProc + b.TerrorProc,
				Miss = a.Miss + b.Miss,
				BonusShredDamage = a.BonusShredDamage + b.BonusShredDamage,
				BonusMangleDamage = a.BonusMangleDamage + b.BonusMangleDamage,
				BonusRipDamagePerCPPerTick = a.BonusRipDamagePerCPPerTick + b.BonusRipDamagePerCPPerTick,
				MangleCostReduction = a.MangleCostReduction + b.MangleCostReduction,
				ExposeWeakness = a.ExposeWeakness + b.ExposeWeakness,
				Bloodlust = a.Bloodlust + b.Bloodlust,
				DrumsOfBattle = a.DrumsOfBattle + b.DrumsOfBattle,
				DrumsOfWar = a.DrumsOfWar + b.DrumsOfWar,
				BonusAgilityMultiplier = (1f + a.BonusAgilityMultiplier) * (1f + b.BonusAgilityMultiplier) - 1f,
                BonusStrengthMultiplier = (1f + a.BonusStrengthMultiplier) * (1f + b.BonusStrengthMultiplier) - 1f,
				BonusStaminaMultiplier = (1f + a.BonusStaminaMultiplier) * (1f + b.BonusStaminaMultiplier) - 1f,
				BonusArmorMultiplier = (1f + a.BonusArmorMultiplier) * (1f + b.BonusArmorMultiplier) - 1f,
				BonusAttackPowerMultiplier = (1f + a.BonusAttackPowerMultiplier) * (1f + b.BonusAttackPowerMultiplier) - 1f,
				BonusCritMultiplier = (1f + a.BonusCritMultiplier) * (1f + b.BonusCritMultiplier) - 1f,
				BonusRipDamageMultiplier = (1f + a.BonusRipDamageMultiplier) * (1f + b.BonusRipDamageMultiplier) - 1f,
				BonusIntellectMultiplier = (1f + a.BonusIntellectMultiplier) * (1f + b.BonusIntellectMultiplier) - 1f,
				BonusSpellCritMultiplier = (1f + a.BonusSpellCritMultiplier) * (1f + b.BonusSpellCritMultiplier) - 1f,
				BonusSpellPowerMultiplier = (1f + a.BonusSpellPowerMultiplier) * (1f + b.BonusSpellPowerMultiplier) - 1f,
                BonusFireSpellPowerMultiplier = (1f + a.BonusFireSpellPowerMultiplier) * (1f + b.BonusFireSpellPowerMultiplier) - 1f,
                BonusFrostSpellPowerMultiplier = (1f + a.BonusFrostSpellPowerMultiplier) * (1f + b.BonusFrostSpellPowerMultiplier) - 1f,
                BonusArcaneSpellPowerMultiplier = (1f + a.BonusArcaneSpellPowerMultiplier) * (1f + b.BonusArcaneSpellPowerMultiplier) - 1f,
                BonusShadowSpellPowerMultiplier = (1f + a.BonusShadowSpellPowerMultiplier) * (1f + b.BonusShadowSpellPowerMultiplier) - 1f,
                BonusNatureSpellPowerMultiplier = (1f + a.BonusNatureSpellPowerMultiplier) * (1f + b.BonusNatureSpellPowerMultiplier) - 1f,
                BonusSpiritMultiplier = (1f + a.BonusSpiritMultiplier) * (1f + b.BonusSpiritMultiplier) - 1f,
                ThreatMultiplier = (1f + a.ThreatMultiplier) * (1f + b.ThreatMultiplier) - 1f,
                SpellCritRating = a.SpellCritRating + b.SpellCritRating,
                SpellFrostCritRating = a.SpellFrostCritRating + b.SpellFrostCritRating,
                SpellDamageRating = a.SpellDamageRating + b.SpellDamageRating,
				SpellFireDamageRating = a.SpellFireDamageRating + b.SpellFireDamageRating,
				SpellHasteRating = a.SpellHasteRating + b.SpellHasteRating,
				SpellHitRating = a.SpellHitRating + b.SpellHitRating,
				SpellShadowDamageRating = a.SpellShadowDamageRating + b.SpellShadowDamageRating,
				SpellFrostDamageRating = a.SpellFrostDamageRating + b.SpellFrostDamageRating,
                SpellArcaneDamageRating = a.SpellArcaneDamageRating + b.SpellArcaneDamageRating,
                SpellNatureDamageRating = a.SpellNatureDamageRating + b.SpellNatureDamageRating,
                SpellPenetration = a.SpellPenetration + b.SpellPenetration,
                Mana = a.Mana + b.Mana,
                LightningCapacitorProc = a.LightningCapacitorProc + b.LightningCapacitorProc,
                ArcaneBlastBonus = a.ArcaneBlastBonus + b.ArcaneBlastBonus,
                SpellDamageFor6SecOnCrit = a.SpellDamageFor6SecOnCrit + b.SpellDamageFor6SecOnCrit,
                EvocationExtension = a.EvocationExtension + b.EvocationExtension,
                BonusMageNukeMultiplier = (1f + a.BonusMageNukeMultiplier) * (1f + b.BonusMageNukeMultiplier) - 1f,
                ManaRestorePerHit = a.ManaRestorePerHit + b.ManaRestorePerHit,
                ManaRestorePerCast = a.ManaRestorePerCast + b.ManaRestorePerCast,
                BonusManaGem = a.BonusManaGem + b.BonusManaGem,
                BonusManaPotion = Math.Max(a.BonusManaPotion, b.BonusManaPotion), // does not stack
                SpellDamageFor10SecOnHit_10_45 = a.SpellDamageFor10SecOnHit_10_45 + b.SpellDamageFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = a.SpellDamageFromIntellectPercentage + b.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = a.SpellDamageFromSpiritPercentage + b.SpellDamageFromSpiritPercentage,
                SpellDamageFor10SecOnResist = a.SpellDamageFor10SecOnResist + b.SpellDamageFor10SecOnResist,
                SpellDamageFor15SecOnCrit_20_45 = a.SpellDamageFor15SecOnCrit_20_45 + b.SpellDamageFor15SecOnCrit_20_45,
                SpellCombatManaRegeneration = a.SpellCombatManaRegeneration + b.SpellCombatManaRegeneration,
                SpellHasteFor5SecOnCrit_50 = a.SpellHasteFor5SecOnCrit_50 + b.SpellHasteFor5SecOnCrit_50,
                SpellHasteFor6SecOnCast_15_45 = a.SpellHasteFor6SecOnCast_15_45 + b.SpellHasteFor6SecOnCast_15_45,
                SpellDamageFor10SecOnHit_5 = a.SpellDamageFor10SecOnHit_5 + b.SpellDamageFor10SecOnHit_5,
                SpellHasteFor6SecOnHit_10_45 = a.SpellHasteFor6SecOnHit_10_45 + b.SpellHasteFor6SecOnHit_10_45,
                SpellDamageFor10SecOnCrit_20_45 = a.SpellDamageFor10SecOnCrit_20_45 + b.SpellDamageFor10SecOnCrit_20_45,
                Healing = a.Healing + b.Healing,
                StarfireDmg = a.StarfireDmg + b.StarfireDmg,
                WrathDmg = a.WrathDmg + b.WrathDmg,
                MoonfireDmg = a.MoonfireDmg + b.MoonfireDmg,
                IdolCritRating = a.IdolCritRating + b.IdolCritRating,
                UnseenMoonDamageBonus = a.UnseenMoonDamageBonus + b.UnseenMoonDamageBonus,
                BonusPhysicalDamageMultiplier = (1f + a.BonusPhysicalDamageMultiplier)*(1f+b.BonusPhysicalDamageMultiplier) -1f,
                BonusCrusaderStrikeDamageMultiplier = (1f + a.BonusCrusaderStrikeDamageMultiplier)*(1f+b.BonusCrusaderStrikeDamageMultiplier) -1f,
				MageSpellCrit = a.MageSpellCrit + b.MageSpellCrit,
				ShatteredSunMightProc = a.ShatteredSunMightProc + b.ShatteredSunMightProc,
				CrushChanceReduction = a.CrushChanceReduction + b.CrushChanceReduction,
				WindfuryAPBonus = a.WindfuryAPBonus + b.WindfuryAPBonus,
                ShatteredSunAcumenProc = a.ShatteredSunAcumenProc + b.ShatteredSunAcumenProc,
                TimbalsProc = a.TimbalsProc + b.TimbalsProc
			};
		}

		public bool Equals(Stats other)
		{
			return this == other;
		}
		public ArrayUtils.CompareResult CompareTo(Stats other)
		{
			if (ReferenceEquals(other, null)) return 0;
			return ArrayUtils.AllCompare(this._rawData, other._rawData);
		}
		//int IComparable.CompareTo(object other)
		//{
		//    return CompareTo(other as Stats);
		//}

        public override int GetHashCode()
>>>>>>> .r17541
        {
        }

        //as the ocean opens up to swallow you
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo info in PropertyInfoCache)
            {
                float value = (float) info.GetValue(this, null);
                if (value != 0)
                {
                    if (IsMultiplicative(info))
                    {
                        value *= 100;
                    }

                    value = (float) Math.Round(value * 100f) / 100f;

                    sb.AppendFormat("{0}{1}, ", value, Extensions.DisplayName(info));
                }
            }

            return sb.ToString().TrimEnd(' ', ',');
        }

        public Stats Clone()
        {
            Stats clone = (Stats) this.MemberwiseClone();
            clone._rawAdditiveData = (float[]) clone._rawAdditiveData.Clone();
            return clone;
        }

        #region Multiplicative Handling
        private static PropertyInfo[] _propertyInfoCache = null;
        private static List<PropertyInfo> _multiplicativeProperties = new List<PropertyInfo>();

        static Stats()
        {
            List<PropertyInfo> items = new List<PropertyInfo>();

            foreach (PropertyInfo info in typeof(Stats).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.ExactBinding))
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    items.Add(info);
                }
            }
            _propertyInfoCache = items.ToArray();

            foreach (PropertyInfo info in _propertyInfoCache)
            {
                if (info.GetCustomAttributes(typeof(MultiplicativeAttribute), false).Length > 0)
                {
                    _multiplicativeProperties.Add(info);
                }
            }
        }

        public static PropertyInfo[] PropertyInfoCache
        {
            get
            {
                return _propertyInfoCache;
            }
        }

        public static bool IsMultiplicative(PropertyInfo info)
        {
            return _multiplicativeProperties.Contains(info);
        }
        #endregion

        private class PropertyComparer : IComparer<PropertyInfo>
        {
            public int Compare(PropertyInfo x, PropertyInfo y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public static String[] StatNames
        {
            get
            {
                String[] names = new string[PropertyInfoCache.Length];
                for (int i = 0; i < PropertyInfoCache.Length; i++)
                {
                    names[i] = Extensions.DisplayName(PropertyInfoCache[i]);
                }
                Array.Sort(names);
                return names;
            }
        }

        public IDictionary<PropertyInfo, float> Values(StatFilter filter)
        {
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
            foreach (PropertyInfo info in PropertyInfoCache)
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    float value = (float) info.GetValue(this, null);
                    if (filter(value))
                    {
                        dict[info] = value;
                    }
                }
            }
            return dict;
        }
    }

    public delegate bool StatFilter(float value);

    [AttributeUsage(AttributeTargets.Property)]
    public class MultiplicativeAttribute : Attribute
    {
    }

    public static class Extensions
    {
        // requires .net 3.5 public static string LongName(this PropertyInfo info)
        // allows it to be called like
        //   info.LongName()
        // instead of
        //   Extensions.LongName(info)

        public static PropertyInfo UnDisplayName(string displayName)
        {
            foreach (PropertyInfo info in Stats.PropertyInfoCache)
            {
                if (DisplayName(info).Trim() == displayName.Trim())
                    return info;
            }
            return null;
        }

        public static string DisplayName(PropertyInfo info)
        {
            string prettyName = null;

            object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length == 1 && attributes[0] is DisplayNameAttribute && (attributes[0] as DisplayNameAttribute).DisplayName != null)
            {
                prettyName = (attributes[0] as DisplayNameAttribute).DisplayName;
            }
            else
            {
                prettyName = SpaceCamel(info.Name);
            }
            if (!prettyName.StartsWith("%"))
                prettyName = " " + prettyName;
            return prettyName;
        }
        public static string SpaceCamel(String name)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                    name,
                    "([A-Z])",
                    " $1",
                    System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }

        public static string UnSpaceCamel(String name)
        {
            return System.Text.RegularExpressions.Regex.Replace(
                    name,
                    "( )",
                    "",
                    System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
    }
}
