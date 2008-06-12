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
        AldorRegaliaInterruptProtection,
        ArcaneBlastBonus,
        ArcaneResistance,
        Armor,
        ArmorPenetration,
		AshtongueTrinketProc,
        AttackPower,
        AverageAgility,
        AverageArmor,
        Block,
        BlockRating,
        BlockValue,
        Bloodlust,
        BloodlustProc,
        BonusCommandingShoutHP,
        BonusLacerateDamage,
        BonusManaGem,
        BonusMangleBearDamage,
        BonusMangleCatDamage,
        BonusRipDamagePerCPPerTick,
        BonusShredDamage,
        CatFormStrength,
        CHHealIncrease,
        CHManaReduction,
        Crit,
        CritRating,
        CrushChanceReduction,
        Defense,
        DefenseRating,
        Dodge,
        DodgeRating,
        DrumsOfBattle,
        DrumsOfWar,
        EvocationExtension,
        Expertise,
        ExpertiseRating,
        ExposeWeakness,
        FireResistance,
        FoLHeal,
        FoLCrit,
        FoLBoL,
        FrostResistance,
        HasteRating,
        Healing,
        Health,
        Hit,
        HitRating,
        HLCost,
        HLHeal,
        HLBoL,
        HLCrit,
        Hp5,
        IdolCritRating,
        InnervateCooldownReduction,
        Intellect,
        JudgementOfCommandAttackPowerBonus,
        LHWManaReduction,
        LightningCapacitorProc,
        LotPCritRating,
        MageAllResist,
        MageSpellCrit,
        Mana,
        ManaSpringMp5Increase,
        ManaRestorePerCast,
        ManaRestorePerCast_5_15,
        ManaRestorePerHit,
        MangleCostReduction,
        MementoProc,
        Miss,
        MoonfireDmg,
        MoonfireExtension,
        MongooseProc,
        MongooseProcAverage,
        MongooseProcConstant,
        Mp5,
        Mp5OnCastFor20SecOnUse2Min,
        NatureResistance,
        Parry,
        ParryRating,
		RangedAttackPower,
        Resilience,
        ScopeDamage, 
        ShadowResistance,
        ShatteredSunAcumenProc,
        ShatteredSunMightProc,
        ShatteredSunRestoProc,
        SpellArcaneDamageRating,
        SpellCombatManaRegeneration,
        SpellCrit,
        SpellCritRating,
        SpellDamageFor10SecOnCrit_20_45,
        SpellDamageFor10SecOnHit_10_45,
        SpellDamageFor10SecOnHit_5,
        SpellDamageFor10SecOnResist,
        SpellDamageFor15SecOnCrit_20_45,
        SpellDamageFor15SecOnManaGem,
        SpellDamageFor15SecOnUse90Sec,
        SpellDamageFor15SecOnUse2Min,
        SpellDamageFor20SecOnUse2Min,
        SpellDamageFor6SecOnCrit,
        SpellDamageFromIntellectPercentage,
        SpellDamageFromSpiritPercentage,
        SpellDamageRating,
        SpellFireDamageRating,
        SpellFrostCritRating,
        SpellFrostDamageRating,
        SpellHasteFor20SecOnUse2Min,
        SpellHasteFor20SecOnUse5Min,
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
        WrathDmg,
        DruidAshtongueTrinket,
        AverageHeal, 
		BonusPetCritChance,
        BonusWarlockSchoolDamageOnCast, 
        BonusWarlockDotExtension
    }

    enum MultiplicativeStat : int
    {
        BonusMangleBearThreat,
        BonusAgilityMultiplier,
        BonusArcaneSpellPowerMultiplier,
        BonusArmorMultiplier,
        BonusBlockValueMultiplier,
        BonusAttackPowerMultiplier,
        BonusCritMultiplier,
        BonusCrusaderStrikeDamageMultiplier,
        BonusFireSpellPowerMultiplier,
        BonusFrostSpellPowerMultiplier,
        BonusIntellectMultiplier,
        BonusMageNukeMultiplier,
        BonusWarlockNukeMultiplier, 
        BonusNatureSpellPowerMultiplier,
		BonusPetDamageMultiplier,
        BonusPhysicalDamageMultiplier,
        BonusRipDamageMultiplier,
        BonusShieldSlamDamage,
        BonusSpellCritMultiplier,
        BonusSpellPowerMultiplier,
        BonusSpiritMultiplier,
        BonusHealthMultiplier,
        BonusStaminaMultiplier,
        BonusStrengthMultiplier,
        BonusSwipeDamageMultiplier,
        BonusShadowSpellPowerMultiplier,
        FoLMultiplier,
        ThreatIncreaseMultiplier,
        BonusWarlockDotDamageMultiplier,
		BonusRangedAttackPowerMultiplier
    }

    enum InverseMultiplicativeStat : int
    {
        ThreatReductionMultiplier,
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
        private float[] _rawAdditiveData = new float[AdditiveStatCount];
        private float[] _rawMultiplicativeData = new float[MultiplicativeStatCount];
        private float[] _rawInverseMultiplicativeData = new float[InverseMultiplicativeStatCount];
        private float[] _rawNoStackData = new float[NonStackingStatCount];
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
        public float JudgementOfCommandAttackPowerBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.JudgementOfCommandAttackPowerBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.JudgementOfCommandAttackPowerBonus] = value; }
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
        [DisplayName("Resist")]
        public float MageAllResist
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageAllResist]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageAllResist] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Crit")]
        public float SpellCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCrit] = value; }
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
        [Category("Combat Values")]
        public float Crit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Crit]; }
            set { _rawAdditiveData[(int)AdditiveStat.Crit] = value; }
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
        [Category("Combat Values")]
        public float Hit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Hit]; }
            set { _rawAdditiveData[(int)AdditiveStat.Hit] = value; }
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
        [Category("Combat Values")]
        public float Dodge
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Dodge]; }
            set { _rawAdditiveData[(int)AdditiveStat.Dodge] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        public float Parry
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Parry]; }
            set { _rawAdditiveData[(int)AdditiveStat.Parry] = value; }
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
        [Category("Combat Values")]
        public float Block
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Block]; }
            set { _rawAdditiveData[(int)AdditiveStat.Block] = value; }
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
        [Category("Combat Values")]
        public float Defense
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Defense]; }
            set { _rawAdditiveData[(int)AdditiveStat.Defense] = value; }
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
        [DisplayName("Resilience")]
        public float Resilience
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Resilience]; }
            set { _rawAdditiveData[(int)AdditiveStat.Resilience] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        public float Expertise
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Expertise]; }
            set { _rawAdditiveData[(int)AdditiveStat.Expertise] = value; }
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
        public float AldorRegaliaInterruptProtection
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AldorRegaliaInterruptProtection]; }
            set { _rawAdditiveData[(int)AdditiveStat.AldorRegaliaInterruptProtection] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Bonus Commanding Shout HP")]
        public float BonusCommandingShoutHP
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCommandingShoutHP]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCommandingShoutHP] = value; }
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

        // 5% proc rate, 15 sec internal cooldown
        [DisplayName("Mana Restore Per Cast (5%)")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestorePerCast_5_15
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCast_5_15]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCast_5_15] = value; }
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
        [DisplayName("Spell Damage (15 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellDamageFor15SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor15SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor15SecOnUse2Min] = value; }
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
        [DisplayName("Spell Haste (20 sec/5 min)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor20SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor20SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor20SecOnUse5Min] = value; }
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
        [DisplayName("Mana Gem Effect")]
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

        [System.ComponentModel.DefaultValue(0f)]
        public float ShatteredSunRestoProc
        {
          get { return _rawAdditiveData[(int)AdditiveStat.ShatteredSunRestoProc]; }
          set { _rawAdditiveData[(int)AdditiveStat.ShatteredSunRestoProc] = value; }
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

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Average Armor")]
        public float AverageArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AverageArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.AverageArmor] = value; }
        }

        [DisplayName("Mongoose Proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MongooseProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MongooseProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.MongooseProc] = value; }
        }

        [DisplayName("Mongoose Proc Average")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MongooseProcAverage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MongooseProcAverage]; }
            set { _rawAdditiveData[(int)AdditiveStat.MongooseProcAverage] = value; }
        }

        [DisplayName("Mongoose Proc Constant")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MongooseProcConstant
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MongooseProcConstant]; }
            set { _rawAdditiveData[(int)AdditiveStat.MongooseProcConstant] = value; }
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

        [DisplayName("Spell damage (8 sec, 25% chance on Starfire)")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float DruidAshtongueTrinket
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DruidAshtongueTrinket]; }
            set { _rawAdditiveData[(int)AdditiveStat.DruidAshtongueTrinket] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("FoL Bonus Heal")]
        [Category("Equipment Procs")]
        public float FoLHeal
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FoLHeal]; }
            set { _rawAdditiveData[(int)AdditiveStat.FoLHeal] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("FoL BoL Bonus")]
        [Category("Equipment Procs")]
        public float FoLBoL
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FoLBoL]; }
            set { _rawAdditiveData[(int)AdditiveStat.FoLBoL] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("FoL Bonus Crit")]
        [Category("Equipment Procs")]
        public float FoLCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FoLCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.FoLCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("HL Bonus Heal")]
        [Category("Equipment Procs")]
        public float HLHeal
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HLHeal]; }
            set { _rawAdditiveData[(int)AdditiveStat.HLHeal] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("HL Reduced Cost")]
        [Category("Equipment Procs")]
        public float HLCost
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HLCost]; }
            set { _rawAdditiveData[(int)AdditiveStat.HLCost] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("HL Bonus Crit")]
        [Category("Equipment Procs")]
        public float HLCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HLCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.HLCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("HL BoL Bonus")]
        [Category("Equipment Procs")]
        public float HLBoL
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HLBoL]; }
            set { _rawAdditiveData[(int)AdditiveStat.HLBoL] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mp5 for 15sec")]
        [Category("Equipment Procs")]
        public float MementoProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MementoProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.MementoProc] = value; }
        }


        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Average Healing")]
        [Category("Equipment Procs")]
        public float AverageHeal
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AverageHeal]; }
            set { _rawAdditiveData[(int)AdditiveStat.AverageHeal] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusWarlockSchoolDamageOnCast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusWarlockSchoolDamageOnCast]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusWarlockSchoolDamageOnCast] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusWarlockDotExtension
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusWarlockDotExtension]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusWarlockDotExtension] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mp5 increase for Mana Spring")]
        public float ManaSpringMp5Increase
        {
          get { return _rawAdditiveData[(int)AdditiveStat.ManaSpringMp5Increase]; }
          set { _rawAdditiveData[(int)AdditiveStat.ManaSpringMp5Increase] = value; }
        }

        [System.ComponentModel.DefaultValue(0f)]
        [DisplayName("Reduce Lesser Healing Wave mana cost by 5%")]
        public float LHWManaReduction
        {
          get { return _rawAdditiveData[(int)AdditiveStat.LHWManaReduction]; }
          set { _rawAdditiveData[(int)AdditiveStat.LHWManaReduction] = value; }
        }

        [System.ComponentModel.DefaultValue(0f)]
        [DisplayName("Reduce Chain Heal mana cost by 10%")]
        public float CHManaReduction
        {
          get { return _rawAdditiveData[(int)AdditiveStat.CHManaReduction]; }
          set { _rawAdditiveData[(int)AdditiveStat.CHManaReduction] = value; }
        }

        [System.ComponentModel.DefaultValue(0f)]
        [DisplayName("Increase healing done by Chain Heal by 5%")]
        public float CHHealIncrease
        {
          get { return _rawAdditiveData[(int)AdditiveStat.CHHealIncrease]; }
          set { _rawAdditiveData[(int)AdditiveStat.CHHealIncrease] = value; }
        }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Category("Base Stats")]
		public float RangedAttackPower
		{
			get { return _rawAdditiveData[(int)AdditiveStat.RangedAttackPower]; }
			set { _rawAdditiveData[(int)AdditiveStat.RangedAttackPower] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative] /*its not really multiplicative, but it is stored as .02 instead of 2 because crit is % based 0-1, so this attribute makes it display correctly 
						  Need to determine if there are any side effects*/ 
		[DisplayName("% Extra Pet Crit Chance")]
		public float BonusPetCritChance
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusPetCritChance]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusPetCritChance] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float AshtongueTrinketProc
		{
			get { return _rawAdditiveData[(int)AdditiveStat.AshtongueTrinketProc]; }
			set { _rawAdditiveData[(int)AdditiveStat.AshtongueTrinketProc] = value; }
		}
#endregion

        #region MultiplicativeStats
        // threat dealt is damage * (1 + ThreatIncreaseMultiplier) * (1 - ThreatReductionMultiplier)
        [Multiplicative]
        [DisplayName("% Threat Increase")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ThreatIncreaseMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.ThreatIncreaseMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.ThreatIncreaseMultiplier] = value; }
        }

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
        [DisplayName("% Shield Slam Damage")]
        public float BonusShieldSlamDamage
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusShieldSlamDamage]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusShieldSlamDamage] = value; }
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
        public float BonusWarlockNukeMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockNukeMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockNukeMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        public float BonusWarlockDotDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockDotDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockDotDamageMultiplier] = value; }
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
        [DisplayName("% Health")]
        public float BonusHealthMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealthMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealthMultiplier] = value; }
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
        [DisplayName("% Block Value")]
        public float BonusBlockValueMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusBlockValueMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusBlockValueMultiplier] = value; }
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

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("FoL Multiplier")]
        [Category("Equipment Procs")]
        public float FoLMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.FoLMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.FoLMultiplier] = value; }
        }
		
		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		public float BonusRangedAttackPowerMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRangedAttackPowerMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRangedAttackPowerMultiplier] = value; }
		}
		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Multiplicative]
		[DisplayName("% Bonus Pet Damage")]
		public float BonusPetDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetDamageMultiplier] = value; }
		}
        #endregion

        #region InverseMultiplicativeStats
        // threat dealt is damage * (1 + ThreatIncreaseMultiplier) * (1 - ThreatReductionMultiplier)
        [Multiplicative]
        [DisplayName("% Threat Reduction")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ThreatReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ThreatReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ThreatReductionMultiplier] = value; }
        }
        #endregion

        #region NoStackStats
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mana Potion Effect")]
        [Category("Equipment Procs")]
        public float BonusManaPotion
        {
            get { return _rawNoStackData[(int)NonStackingStat.BonusManaPotion]; }
            set { _rawNoStackData[(int)NonStackingStat.BonusManaPotion] = value; }
        }


#endregion
	
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
           
            int i = c._rawAdditiveData.Length;
            while(--i >=0)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] + b._rawAdditiveData[i];
            }
            i = c._rawMultiplicativeData.Length;
            while(--i >=0)
            {
                c._rawMultiplicativeData[i] = (1+a._rawMultiplicativeData[i]) * (1+b._rawMultiplicativeData[i])-1;
            }
            i = c._rawInverseMultiplicativeData.Length;
            while(--i >=0)
            {
                c._rawInverseMultiplicativeData[i] = 1-(1-a._rawInverseMultiplicativeData[i]) * (1-b._rawInverseMultiplicativeData[i]);
            }
            
            i = c._rawNoStackData.Length;
            while(--i >=0)
            {
                c._rawNoStackData[i] = Math.Max(a._rawNoStackData[i], b._rawNoStackData[i]);
            }
            return c;
        }

        public unsafe void Accumulate(Stats data)
        {
            int i = _rawAdditiveData.Length;
            fixed (float* rawAdditiveData = _rawAdditiveData, rawAdditiveData2 = data._rawAdditiveData)
            {
                while (--i >= 0)
                {
                    rawAdditiveData[i] += rawAdditiveData2[i];
                }
            }
            i = _rawMultiplicativeData.Length;
            fixed (float* rawMultiplicativeData = _rawMultiplicativeData, rawMultiplicativeData2 = data._rawMultiplicativeData)
            {
                while (--i >= 0)
                {
                    rawMultiplicativeData[i] = (1 + rawMultiplicativeData[i]) * (1 + rawMultiplicativeData2[i]) - 1;
                }
            }
            i = _rawInverseMultiplicativeData.Length;
            fixed (float* rawInverseMultiplicativeData = _rawInverseMultiplicativeData, rawInverseMultiplicativeData2 = data._rawInverseMultiplicativeData)
            {
                while (--i >= 0)
                {
                    rawInverseMultiplicativeData[i] = 1 - (1 - rawInverseMultiplicativeData[i]) * (1 - rawInverseMultiplicativeData2[i]);
                }
            }
            i = _rawNoStackData.Length;
            fixed (float* rawNoStackData = _rawNoStackData, rawNoStackData2 = data._rawNoStackData)
            {
                while (--i >= 0)
                {
                    rawNoStackData[i] = Math.Max(rawNoStackData[i], rawNoStackData2[i]);
                }
            }
        }
      
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
                && ArrayUtils.AllCompare(x._rawInverseMultiplicativeData, y._rawInverseMultiplicativeData, comparison)
                && ArrayUtils.AllCompare(x._rawNoStackData, y._rawNoStackData, comparison) ;
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
            clone._rawMultiplicativeData = (float[]) clone._rawMultiplicativeData.Clone();
            clone._rawNoStackData = (float[]) clone._rawNoStackData.Clone();
            return clone;
        }

        #region Multiplicative Handling
        private static PropertyInfo[] _propertyInfoCache = null;
        private static List<PropertyInfo> _multiplicativeProperties = new List<PropertyInfo>();
        private static int AdditiveStatCount = 0;
        private static int MultiplicativeStatCount = 0;
        private static int InverseMultiplicativeStatCount = 0;
        private static int NonStackingStatCount = 0;

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

            AdditiveStatCount = Enum.GetValues(typeof(AdditiveStat)).Length;
            MultiplicativeStatCount = Enum.GetValues(typeof(MultiplicativeStat)).Length;
            InverseMultiplicativeStatCount = Enum.GetValues(typeof(InverseMultiplicativeStat)).Length;
            NonStackingStatCount = Enum.GetValues(typeof(NonStackingStat)).Length;
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
