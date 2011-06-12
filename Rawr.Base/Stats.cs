using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Serialization;

namespace Rawr
{
    #region Stat Enums
    public enum AdditiveStat : int {
        #region Stats Used by Almost Everyone
        #region ===== Core stats =====
        Strength,
        Agility,
        Stamina,
        Intellect,
        Spirit,
        BaseAgility, // Improper use: Not a stat, used as a helper variable in StatConversion.GetDRAvoidanceChance(), needs fixing.
        #endregion
        #region ===== Mana Related Stats =====
        Mana,
        ManaRestore,
        ManaRestoreFromMaxManaPerSecond,
        ManaorEquivRestore,
        Mp5,
        SpellCombatManaRegeneration,
        SpellsManaCostReduction, // Seems this applies before talents, so different from ManaRestore with 100% proc on SpellCast, initially used by Spark of Hope. This is most likely deprecated
        NatureSpellsManaReduction,
        HolySpellsManaReduction,
        #endregion
        #region ===== Health Related Stats ====
        Health,
        Hp5,
        HealthRestore,
        HealthRestoreFromMaxHealth,
        BonusHealingReceived,
        Healed, // This stores Healing values for targets, not self, like Forethought Talisman's Passive Effect
        #endregion
        #region ===== Offensive Stats =====
        // Rating
        AttackPower,
        SpellPower,
        MasteryRating,
        ExpertiseRating,
        HitRating,
        CritRating,
        HasteRating,
        RangedAttackPower,
        RangedHitRating,
        RangedCritRating,
        RangedHasteRating,
        Resilience,
        SpellPenetration,
        WeaponDamage,
        // Converted Values
        Expertise,     // ie: Glyph of Seal of Truth
        SpellDamageFromIntellectPercentage,
        // Percentage
        PhysicalHit,
        PhysicalCrit,
        SpellHit,
        SpellCrit,
        CritBonusDamage,
        BonusTargets,
        BonusCritChance,
        #endregion
        #region ===== Defensive Stats =====
        // Rating
        Armor,
        BonusArmor,
        BlockRating, // Should be removed, maybe?
        ParryRating,
        DodgeRating,
        // Percentages
        DamageAbsorbed,
        Miss,
        Block,
        Dodge,
        Parry,
        SpellReflectChance,
        // Resistances
        ArcaneResistance,
        FireResistance,
        FrostResistance,
        NatureResistance,
        ShadowResistance,
        #endregion
        #region ===== Target Stats =====
        SpellCritOnTarget,
        #endregion
        #region ===== Item Proc Stats =====
        Paragon,
        TerrorProc,
        ExtractOfNecromanticPowerProc,
        DarkmoonCardDeathProc,
        PVPTrinket,
        MoteOfAnger,
        #endregion
        #region ===== Other Stats =====
        BonusRageGen,
        InterruptProtection,
        PhysicalDamage,
        ArcaneDamage,
        HolyDamage,
        NatureDamage,
        ShadowDamage,
        FireDamage,
        FrostDamage,
        HolySummonedDamage,
        FireSummonedDamage,
        SpellArcaneDamageRating,
        SpellFireDamageRating,
        SpellFrostDamageRating,
        SpellNatureDamageRating,
        SpellShadowDamageRating,
        ScopeDamage,
        CritChanceReduction,
        #endregion
        #endregion
        #region Added by Death Knights [Verified Jothay 2011-03-29]
        BonusDamageBloodStrike,
        BonusDamageDeathCoil,
        BonusDamageDeathStrike,
        BonusDamageFrostStrike,
        BonusDamageHeartStrike,
        BonusDamageIcyTouch,
        BonusDamageObliterate,
        BonusDamageScourgeStrike,
        BonusCritChanceDeathCoil,   // T11
        BonusCritChanceFrostStrike, // T11, Killing Machine Talent
        BonusCritChanceObliterate,  // Killing Machine Talent
        RPp5,
        CinderglacierProc,
        #endregion
        #region Added by Hunters       [Verified Jothay 2011-03-29]
        BonusPetCritChance,
        PetStamina,
        PetStrength,
        PetSpirit,
        PetAttackPower,
        #endregion
        #region Added by Mages
        BonusManaGem,
        MageIceArmor,
        MageMageArmor,
        MageMoltenArmor,
        #endregion
    }
    public enum MultiplicativeStat : int {
        #region Stats Used by Almost Everyone [Verified Jothay 2011-03-29]
        #region ===== Core Stats =====
        // Primary
        BonusStaminaMultiplier,
        BonusStrengthMultiplier,
        BonusAgilityMultiplier,
        BonusIntellectMultiplier,
        BonusSpiritMultiplier,
        // Secondary
        BonusAttackPowerMultiplier,
        BonusSpellPowerMultiplier,
        BonusCritDamageMultiplier,
        BonusSpellCritDamageMultiplier,
        PhysicalHaste,
        RangedHaste,
        SpellHaste,
        #endregion
        #region ===== Mana Related Stats =====
        BonusManaMultiplier,
        #endregion
        #region ===== Health Related Stats =====
        BonusHealthMultiplier,
        BonusPeriodicHealingMultiplier,
        BonusHealingDoneMultiplier,
        HealingReceivedMultiplier,
        BonusCritHealMultiplier,
        #endregion
        #region ===== Offensive Stats =====
        BonusBleedDamageMultiplier,
        BonusDamageMultiplier,
        BonusWhiteDamageMultiplier,
        BonusPeriodicDamageMultiplier,
        #endregion
        #region ===== Defensive Stats =====
        ThreatIncreaseMultiplier,
        BaseArmorMultiplier,
        BonusArmorMultiplier,
        BonusBlockValueMultiplier,
        #endregion
        #region ===== Item Proc Stats =====
        #endregion
        #region ===== Other Stats =====
        BonusArcaneDamageMultiplier,
        BonusFireDamageMultiplier,
        BonusFrostDamageMultiplier,
        BonusNatureDamageMultiplier,
        BonusPhysicalDamageMultiplier,
        BonusShadowDamageMultiplier,
        BonusHolyDamageMultiplier,
        BonusDiseaseDamageMultiplier,
        #endregion
        #region ===== Boss Handler Stats =====
        FireDamageTakenMultiplier,
        #endregion
        #endregion
        #region Added by Death Knights [Verified Jothay 2011-03-29]
        // Both
        BonusFrostWeaponDamage, // Razorice Rune (Enchant)
        #endregion
        #region Added by Druids        [Verified Jothay 2011-03-29]
        BonusDamageMultiplierLacerate, // T11
        BonusDamageMultiplierRakeTick, // T11
        #endregion
        #region Added by Hunters       [Verified Jothay 2011-03-29]
        BonusRangedAttackPowerMultiplier,
        BonusPetAttackPowerMultiplier,
        BonusPetDamageMultiplier,
        #endregion
        #region Added by Paladins      [Verified Jothay 2011-03-29]
        // Retribution
        BonusDamageMultiplierTemplarsVerdict, // T11
        #endregion
        #region Added by Shamans       [Verified Jothay 2011-03-29]
        // Elemental
        BonusDamageMultiplierLavaBurst,
        #endregion      
    }
    public enum InverseMultiplicativeStat : int {
        #region Stats Used by Almost Everyone [Verified Jothay 2011-03-29]
        ArmorPenetration,
        ThreatReductionMultiplier,
        TargetArmorReduction,
        ArmorReductionMultiplier,
        ManaCostReductionMultiplier,
        DamageTakenReductionMultiplier,
        SpellDamageTakenReductionMultiplier,
        PhysicalDamageTakenReductionMultiplier,
        BossAttackSpeedReductionMultiplier,
        BossPhysicalDamageDealtMultiplier,
        #endregion
        #region Added by Death Knights        [Verified Jothay 2011-03-29]
        AntiMagicShellDamageReduction,
        #endregion
    }
    public enum NonStackingStat : int {
        #region Resistances       [Verified Jothay 2011-03-29]
        ArcaneResistanceBuff,
        FireResistanceBuff,
        FrostResistanceBuff,
        NatureResistanceBuff,
        ShadowResistanceBuff,
        #endregion
        #region Pots              [Verified Jothay 2011-03-29]
        BonusManaPotionEffectMultiplier,
        #endregion
        #region Boss Handler      [Verified Jothay 2011-03-29]
        MovementSpeed,
        SilenceDurReduc,
        StunDurReduc,
        SnareRootDurReduc,
        FearDurReduc,
        DisarmDurReduc,
        #endregion
        #region Special Procs     [Verified Jothay 2011-03-29]
        HighestStat,
        HighestSecondaryStat,
        ShieldFromHealed,
        BattlemasterHealthProc,
        #endregion
        #region Added by Paladins [Verified Jothay 2011-03-29]
        // Retribution
        BonusRet_T11_4P_InqHP,
        JudgementCDReduction,
        RighteousVengeanceCanCrit,
        #endregion
        #region Added by Rogues   [Verified Jothay 2011-03-29]
        Rogue_T11_2P,
        Rogue_T11_4P,
        #endregion
        #region Added by Shamans  [Verified Jothay 2011-03-29]
        // Enhance
        Enhance_T11_2P,
        Enhance_T11_4P,
        #endregion
        #region Added by Warlocks [Verified Jothay 2011-03-29]
        Warlock_T11_2P,
        Warlock_T11_4P,
        #endregion
    }
    #endregion

    public enum HitResult {
        AnyMiss,
        AnyHit,
        Miss,
        Dodge,
        Parry,
        Block,
        CritBlock,
        Glance,
        Resist,
        Crit,
        Hit,
    }

    public enum MagicSchool {
        Fire = 2,
        Nature,
        Frost,
        Shadow,
        Arcane,
    }

    #region Attribute Definitions
#if SILVERLIGHT
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DisplayNameAttribute : Attribute
    {
        public DisplayNameAttribute(string name)
        {
            _name = name;
        }

        private readonly string _name;

        public string DisplayName { get { return _name; } }
    }
#endif

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GenerateSerializerAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GenerateArraySerializerAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PercentageAttribute : Attribute { }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public sealed class CommonStat : System.Attribute {
        public static float GetCommonStatMinimumRange(PropertyInfo property) {
            foreach (System.Attribute attribute in property.GetCustomAttributes(false))
            {
                CommonStat commonStat = attribute as CommonStat;
                if (commonStat != null)
                {
                    return commonStat.MinRange;
                }
            }
            return -1f;
        }

        public float MinRange = 0f;
    }

    public delegate bool StatFilter(float value);
    #endregion

    /// <summary>A Stats object represents a collection of stats on an object, such as an Item, Enchant, or Buff.</summary>
#if SILVERLIGHT
    public class Stats
#else    
    public unsafe class Stats
#endif
    {
        #region Indice Work
        internal float[] _rawAdditiveData = new float[AdditiveStatCount];
        internal float[] _rawMultiplicativeData = new float[MultiplicativeStatCount];
        internal float[] _rawInverseMultiplicativeData = new float[InverseMultiplicativeStatCount];
        internal float[] _rawNoStackData = new float[NonStackingStatCount];
        [DefaultValueAttribute(null)]
        [XmlArray("SpecialEffects")]
        [XmlArrayItem(IsNullable = false)]
        public SpecialEffect[] _rawSpecialEffectData = null;

        [XmlIgnore]
        public float[] rawAdditiveData { get { return _rawAdditiveData; } set { _rawAdditiveData = value; } }
        [XmlIgnore]
        public float[] rawMultiplicativeData { get { return _rawMultiplicativeData; } set { _rawMultiplicativeData = value; } }
        [XmlIgnore]
        public float[] rawInverseMultiplicativeData { get { return _rawInverseMultiplicativeData; } set { _rawInverseMultiplicativeData = value; } }
        [XmlIgnore]
        public float[] rawNoStackData { get { return _rawNoStackData; } set { _rawNoStackData = value; } }

        //private static SpecialEffect[] _emptySpecialEffectData = new SpecialEffect[0];
        private const int _defaultSpecialEffectDataCapacity = 4;
        [DefaultValueAttribute(0f)]
        [XmlElement("SpecialEffectCount")]
        public int _rawSpecialEffectDataSize;

        //internal float[] _sparseData;
        internal int[] _sparseIndices;
        internal int _sparseAdditiveCount;
        internal int _sparseMultiplicativeCount;
        internal int _sparseInverseMultiplicativeCount;
        internal int _sparseNoStackCount;

        [XmlIgnore]
        public int[] sparseIndices { get { return _sparseIndices; } set { _sparseIndices = value; } }
        [XmlIgnore]
        public int sparseAdditiveCount { get { return _sparseAdditiveCount; } set { _sparseAdditiveCount = value; } }
        [XmlIgnore]
        public int sparseMultiplicativeCount { get { return _sparseMultiplicativeCount; } set { _sparseMultiplicativeCount = value; } }
        [XmlIgnore]
        public int sparseInverseMultiplicativeCount { get { return _sparseInverseMultiplicativeCount; } set { _sparseInverseMultiplicativeCount = value; } }
        [XmlIgnore]
        public int sparseNoStackCount { get { return _sparseNoStackCount; } set { _sparseNoStackCount = value; } }

        public void Clear()
        {
            Array.Clear(_rawAdditiveData, 0, _rawAdditiveData.Length);
            Array.Clear(_rawMultiplicativeData, 0, _rawMultiplicativeData.Length);
            Array.Clear(_rawInverseMultiplicativeData, 0, _rawInverseMultiplicativeData.Length);
            Array.Clear(_rawNoStackData, 0, _rawNoStackData.Length);
            _rawSpecialEffectDataSize = 0;
        }

        public void InvalidateSparseData() {
            //_sparseData = null;
            _sparseIndices = null;
        }

        public void GenerateSparseData() {
            //List<float> data = new List<float>();
            if (_sparseIndices != null) return;
            lock (_rawAdditiveData)
            {
                if (_sparseIndices == null)
                {
                    List<int> indices = new List<int>();
                    _sparseAdditiveCount = 0;
                    for (int i = 0; i < _rawAdditiveData.Length; i++)
                    {
                        if (_rawAdditiveData[i] != 0.0f)
                        {
                            _sparseAdditiveCount++;
                            //data.Add(_rawAdditiveData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseMultiplicativeCount = 0;
                    for (int i = 0; i < _rawMultiplicativeData.Length; i++)
                    {
                        if (_rawMultiplicativeData[i] != 0.0f)
                        {
                            _sparseMultiplicativeCount++;
                            //data.Add(_rawMultiplicativeData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseInverseMultiplicativeCount = 0;
                    for (int i = 0; i < _rawInverseMultiplicativeData.Length; i++)
                    {
                        if (_rawInverseMultiplicativeData[i] != 0.0f)
                        {
                            _sparseInverseMultiplicativeCount++;
                            //data.Add(_rawInverseMultiplicativeData[i]);
                            indices.Add(i);
                        }
                    }
                    _sparseNoStackCount = 0;
                    for (int i = 0; i < _rawNoStackData.Length; i++)
                    {
                        if (_rawNoStackData[i] != 0.0f)
                        {
                            _sparseNoStackCount++;
                            //data.Add(_rawNoStackData[i]);
                            indices.Add(i);
                        }
                    }
                    //_sparseData = data.ToArray();
                    _sparseIndices = indices.ToArray();
                }
            }
        }
        #endregion

        #region ===== Additive Stats ==================
        #region Stats Used by Almost Everyone
        #region ===== Core Stats =====
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat(MinRange = 20f)]
        public float Strength
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Strength]; }
            set { _rawAdditiveData[(int)AdditiveStat.Strength] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat(MinRange = 10f)]
        public float Agility
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Agility]; }
            set { _rawAdditiveData[(int)AdditiveStat.Agility] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat]
        public float Stamina
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Stamina]; }
            set { _rawAdditiveData[(int)AdditiveStat.Stamina] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat(MinRange = 10f)]
        public float Intellect
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Intellect]; }
            set { _rawAdditiveData[(int)AdditiveStat.Intellect] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat(MinRange = 10f)]
        public float Spirit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Spirit]; }
            set { _rawAdditiveData[(int)AdditiveStat.Spirit] = value; }
        }
        /// <summary>
        /// Improper use: Not a stat, used as a helper variable in StatConversion.GetDRAvoidanceChance(), needs fixing.
        /// </summary>
        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float BaseAgility
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BaseAgility]; }
            set { _rawAdditiveData[(int)AdditiveStat.BaseAgility] = value; }
        }
        #endregion
        #region ===== Mana Related Stats =====
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Mana
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mana]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mana] = value; }
        }
        /// <summary>
        /// This stat stores Mana restorations such as Runic Mana Potion
        /// or Mana Restore procs like Figurine - Talasite Owl's Use Effect
        /// </summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("Mana Restored")]
        [Category("Equipment Effects")]
        public float ManaRestore
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestore]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestore] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Max Mana / Sec")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float ManaRestoreFromMaxManaPerSecond
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromMaxManaPerSecond]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromMaxManaPerSecond] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Mana or Equivalent Restore")]
        [Percentage]
        [Category("Equipment Effects")]
        public float ManaorEquivRestore
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaorEquivRestore]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaorEquivRestore] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Mana per 5 sec")]
        [Category("Base Stats")]
        [CommonStat]
        public float Mp5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mp5]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mp5] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Combat Mana Regeneration")]
        [Category("Equipment Effects")]
        public float SpellCombatManaRegeneration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCombatManaRegeneration]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCombatManaRegeneration] = value; }
        }
        /// <summary>This is most likely deprecated</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("Spells Mana Cost Reduction")]
        [Category("Equipment Effects")]
        public float SpellsManaCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellsManaCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellsManaCostReduction] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Nature Spells Mana Cost Reduction")]
        [Category("Equipment Effects")]
        public float NatureSpellsManaCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.NatureSpellsManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.NatureSpellsManaReduction] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Holy Spells Mana Cost Reduction")]
        [Category("Equipment Effects")]
        public float HolySpellsManaCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolySpellsManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolySpellsManaReduction] = value; }
        }
        #endregion
        #region ===== Health Related Stats =====
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat]
        public float Health
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Health]; }
            set { _rawAdditiveData[(int)AdditiveStat.Health] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Health per 5 sec")]
        [Category("Base Stats")]
        [CommonStat]
        public float Hp5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Hp5]; }
            set { _rawAdditiveData[(int)AdditiveStat.Hp5] = value; }
        }
        /// <summary>This stat stores Health restorations such as Runic Healing Potion</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("Health Restored")]
        [Category("Equipment Effects")]
        public float HealthRestore
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HealthRestore]; }
            set { _rawAdditiveData[(int)AdditiveStat.HealthRestore] = value; }
        }
        /// <summary>
        /// This stat stores Health restorations such as Invigorating Earthsiege Diamond's
        /// 'Sometimes Heal on your Crits' (2% of Health Restored)
        /// </summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("% of Max Health Restored")]
        [Category("Equipment Effects")]
        [Percentage]
        public float HealthRestoreFromMaxHealth
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HealthRestoreFromMaxHealth]; }
            set { _rawAdditiveData[(int)AdditiveStat.HealthRestoreFromMaxHealth] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Healing Received")]
        [Category("Equipment Effects")]
        public float BonusHealingReceived
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusHealingReceived]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusHealingReceived] = value; }
        }
        /// <summary>This stores Healing values for targets, not self, like Forethought Talisman's Passive Effect</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("Healed")]
        [Category("Equipment Effects")]
        public float Healed
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Healed]; }
            set { _rawAdditiveData[(int)AdditiveStat.Healed] = value; }
        }
        #endregion
        #region ===== Offensive Stats =====
        // Rating
        [DefaultValueAttribute(0f)]
        [DisplayName("Attack Power")]
        [Category("Base Stats")]
        [CommonStat(MinRange = 20f)]
        public float AttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.AttackPower] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power")]
        [Category("Base Stats")]
        [CommonStat]
        public float SpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPower] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Mastery Rating")]
        [Category("Base Stats")]
        [CommonStat]
        public float MasteryRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MasteryRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.MasteryRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Expertise Rating")]
        [Category("Base Stats")]
        [CommonStat]
        public float ExpertiseRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExpertiseRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExpertiseRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Hit Rating")]
        [Category("Base Stats")]
        [CommonStat]
        public float HitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.HitRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Crit Rating")]
        [Category("Base Stats")]
        [CommonStat]
        public float CritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Haste Rating")]
        [Category("Base Stats")]
        [CommonStat(MinRange = 10f)]
        public float HasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Ranged Attack Power")]
        [Category("Hunter")]
        [CommonStat]
        public float RangedAttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RangedAttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.RangedAttackPower] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Ranged Hit Rating")]
        [Category("Hunter")]
        [CommonStat]
        public float RangedHitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RangedHitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.RangedHitRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Ranged Crit Rating")]
        [Category("Hunter")]
        [CommonStat]
        public float RangedCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RangedCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.RangedCritRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Ranged Haste Rating")]
        [Category("Hunter")]
        [CommonStat]
        public float RangedHasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RangedHasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.RangedHasteRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Resilience")]
        [Category("Base Stats")]
        [CommonStat]
        public float Resilience
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Resilience]; }
            set { _rawAdditiveData[(int)AdditiveStat.Resilience] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Penetration")]
        [Category("Base Stats")]
        public float SpellPenetration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPenetration]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPenetration] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float WeaponDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WeaponDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.WeaponDamage] = value; }
        }
        // Converted Values
        [DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        public float Expertise
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Expertise]; }
            set { _rawAdditiveData[(int)AdditiveStat.Expertise] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float SpellDamageFromIntellectPercentage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFromIntellectPercentage]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFromIntellectPercentage] = value; }
        }
        // Percentage
        [DefaultValueAttribute(0f)]
        [DisplayName("% Physical Hit")]
        [Percentage]
        [Category("Combat Values")]
        public float PhysicalHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PhysicalHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.PhysicalHit] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Physical Crit")]
        [Percentage]
        [Category("Combat Values")]
        public float PhysicalCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PhysicalCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.PhysicalCrit] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Spell Hit")]
        [Percentage]
        [Category("Combat Values")]
        public float SpellHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHit] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Spell Crit")]
        [Percentage]
        [Category("Combat Values")]
        public float SpellCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCrit] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Crit Bonus Damage")]
        [Percentage]
        [Category("Equipment Effects")]
        public float CritBonusDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritBonusDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritBonusDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Targets")]
        [Category("Boss Handler")]
        public float BonusTargets
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusTargets]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusTargets] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat]
        public float BonusCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCritChance] = value; }
        }
        #endregion
        #region ===== Defensive Stats =====
        // Rating
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat(MinRange = 80f)]
        public float Armor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Armor]; }
            set { _rawAdditiveData[(int)AdditiveStat.Armor] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat(MinRange = 80f)]
        public float BonusArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusArmor] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Block Rating")]
        [Category("Base Stats")]
        [CommonStat]
        public float BlockRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BlockRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.BlockRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Parry Rating")]
        [Category("Base Stats")]
        [CommonStat]
        public float ParryRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ParryRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ParryRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Dodge Rating")]
        [Category("Base Stats")]
        [CommonStat]
        public float DodgeRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DodgeRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.DodgeRating] = value; }
        }
        // Percentages
        [DefaultValueAttribute(0f)]
        [DisplayName("Damage Absorbed")]
        [Category("Equipment Effects")]
        [CommonStat]
        public float DamageAbsorbed
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DamageAbsorbed]; }
            set { _rawAdditiveData[(int)AdditiveStat.DamageAbsorbed] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Miss")]
        [Percentage]
        [Category("Combat Values")]
        public float Miss
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Miss]; }
            set { _rawAdditiveData[(int)AdditiveStat.Miss] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Block")]
        [Percentage]
        [Category("Combat Values")]
        public float Block
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Block]; }
            set { _rawAdditiveData[(int)AdditiveStat.Block] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Dodge")]
        [Percentage]
        [Category("Combat Values")]
        public float Dodge
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Dodge]; }
            set { _rawAdditiveData[(int)AdditiveStat.Dodge] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Parry")]
        [Percentage]
        [Category("Combat Values")]
        public float Parry
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Parry]; }
            set { _rawAdditiveData[(int)AdditiveStat.Parry] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Chance to Reflect Spell")]
        [Percentage]
        [Category("Equipment Effects")]
        public float SpellReflectChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellReflectChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellReflectChance] = value; }
        }
        // Resistances
        [DefaultValueAttribute(0f)]
        [DisplayName("Arcane Resistance")]
        [Category("Resistances")]
        public float ArcaneResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArcaneResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArcaneResistance] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Fire Resistance")]
        [Category("Resistances")]
        public float FireResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FireResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.FireResistance] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Frost Resistance")]
        [Category("Resistances")]
        public float FrostResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FrostResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.FrostResistance] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Nature Resistance")]
        [Category("Resistances")]
        public float NatureResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.NatureResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.NatureResistance] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Shadow Resistance")]
        [Category("Resistances")]
        public float ShadowResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShadowResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShadowResistance] = value; }
        }
        #endregion
        #region ===== Target Stats =====
        [DefaultValueAttribute(0f)]
        [DisplayName("% Spell Crit on target")]
        [Percentage]
        [Category("Combat Values")]
        public float SpellCritOnTarget
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCritOnTarget]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCritOnTarget] = value; }
        }
        #endregion
        #region ===== Item Proc Stats =====
        [DefaultValueAttribute(0f)]
        [DisplayName("Strength or Agility")]
        [Category("Equipment Effects")]
        public float Paragon
        {
            get { return _rawAdditiveData[(int) AdditiveStat.Paragon]; }
            set { _rawAdditiveData[(int)AdditiveStat.Paragon] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float TerrorProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TerrorProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.TerrorProc] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Extract of Necromantic Power proc")]
        [Category("Equipment Effects")]
        public float ExtractOfNecromanticPowerProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExtractOfNecromanticPowerProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExtractOfNecromanticPowerProc] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Darkmoon Card: Death proc")]
        [Category("Equipment Effects")]
        public float DarkmoonCardDeathProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DarkmoonCardDeathProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.DarkmoonCardDeathProc] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("PvP Trinket")]
        [Category("Equipment Effects")]
        public float PVPTrinket
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PVPTrinket]; }
            set { _rawAdditiveData[(int)AdditiveStat.PVPTrinket] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Weapon Swing")]
        [Percentage]
        [Category("Equipment Effects")]
        public float MoteOfAnger
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MoteOfAnger]; }
            set { _rawAdditiveData[(int)AdditiveStat.MoteOfAnger] = value; }
        }
        #endregion
        #region ===== Other Stats =====
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Rage Generated")]
        [Category("Warrior")]
        public float BonusRageGen
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRageGen]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRageGen] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float InterruptProtection
        {
            get { return _rawAdditiveData[(int)AdditiveStat.InterruptProtection]; }
            set { _rawAdditiveData[(int)AdditiveStat.InterruptProtection] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Physical Damage")]
        [Category("Equipment Effects")]
        public float PhysicalDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PhysicalDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.PhysicalDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Arcane Damage")]
        [Category("Equipment Effects")]
        public float ArcaneDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArcaneDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArcaneDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Holy Damage")]
        [Category("Equipment Effects")]
        public float HolyDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Nature Damage")]
        [Category("Equipment Effects")]
        public float NatureDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.NatureDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.NatureDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Shadow Damage")]
        [Category("Equipment Effects")]
        public float ShadowDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShadowDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShadowDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Fire Damage")]
        [Category("Equipment Effects")]
        public float FireDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FireDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.FireDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Frost Damage")]
        [Category("Equipment Effects")]
        public float FrostDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FrostDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.FrostDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Holy Summoned Damage")]
        [Category("Equipment Effects")]
        public float HolySummonedDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolySummonedDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolySummonedDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Fire Summoned Damage")]
        [Category("Equipment Effects")]
        public float FireSummonedDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FireSummonedDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.FireSummonedDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Arcane Damage")]
        [CommonStat]
        public float SpellArcaneDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellArcaneDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellArcaneDamageRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Fire Damage")]
        [CommonStat]
        public float SpellFireDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellFireDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellFireDamageRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Frost Damage")]
        [CommonStat]
        public float SpellFrostDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellFrostDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellFrostDamageRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Nature Damage")]
        [CommonStat]
        public float SpellNatureDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellNatureDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellNatureDamageRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Shadow Damage")]
        [CommonStat]
        public float SpellShadowDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellShadowDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellShadowDamageRating] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        [CommonStat]
        public float ScopeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ScopeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.ScopeDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Reduction to chance to be critically hit")]
        [Percentage]
        [Category("Engineering")]
        public float CritChanceReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritChanceReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritChanceReduction] = value; }
        }
        #endregion
        #endregion
        #region Added by Death Knights
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Blood Strike Damage")]
        [Category("Death Knight")]
        public float BonusDamageBloodStrike
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDamageBloodStrike]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDamageBloodStrike] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Death Coil Damage")]
        [Category("Death Knight")]
        public float BonusDamageDeathCoil
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDamageDeathCoil]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDamageDeathCoil] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Death Strike Damage")]
        [Category("Death Knight")]
        public float BonusDamageDeathStrike
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDamageDeathStrike]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDamageDeathStrike] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Frost Strike Damage")]
        [Category("Death Knight")]
        public float BonusDamageFrostStrike
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDamageFrostStrike]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDamageFrostStrike] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Heart Strike Damage")]
        [Category("Death Knight")]
        public float BonusDamageHeartStrike
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDamageHeartStrike]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDamageHeartStrike] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Icy Touch Damage")]
        [Category("Death Knight")]
        public float BonusDamageIcyTouch
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDamageIcyTouch]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDamageIcyTouch] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Obliterate Damage")]
        [Category("Death Knight")]
        public float BonusDamageObliterate
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDamageObliterate]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDamageObliterate] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Scourge Strike Damage")]
        [Category("Death Knight")]
        public float BonusDamageScourgeStrike
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDamageScourgeStrike]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDamageScourgeStrike] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Crit Chance for Death Coil")]
        [Percentage]
        [Category("Death Knight")]
        public float BonusCritChanceDeathCoil
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCritChanceDeathCoil]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCritChanceDeathCoil] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Crit Chance for Frost Strike")]
        [Percentage]
        [Category("Death Knight")]
        public float BonusCritChanceFrostStrike
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCritChanceFrostStrike]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCritChanceFrostStrike] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Crit Chance for Obliterate")]
        [Percentage]
        [Category("Death Knight")]
        public float BonusCritChanceObliterate
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCritChanceObliterate]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCritChanceObliterate] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        [DisplayName("Runic Power per 5 Seconds")]
        public float RPp5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RPp5]; }
            set { _rawAdditiveData[(int)AdditiveStat.RPp5] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Cinderglacier Proc")]
        [Category("Death Knight")]
        public float CinderglacierProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CinderglacierProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.CinderglacierProc] = value; }
        }
        #endregion
        #region Added by Hunters
        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Extra Pet Crit Chance")]
        [Category("Hunter")]
        public float BonusPetCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusPetCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusPetCritChance] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Pet Stamina")]
        [Category("Hunter")]
        public float PetStamina
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PetStamina]; }
            set { _rawAdditiveData[(int)AdditiveStat.PetStamina] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Pet Strength")]
        [Category("Hunter")]
        public float PetStrength
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PetStrength]; }
            set { _rawAdditiveData[(int)AdditiveStat.PetStrength] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Pet Spirit")]
        [Category("Hunter")]
        public float PetSpirit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PetSpirit]; }
            set { _rawAdditiveData[(int)AdditiveStat.PetSpirit] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Pet Attack Power")]
        [Category("Hunter")]
        public float PetAttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PetAttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.PetAttackPower] = value; }
        }
        #endregion
        #region Added by Mages
        [DefaultValueAttribute(0f)]
        [DisplayName("Mana Gem Effect")]
        [Category("Mage")]
        public float BonusManaGem
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusManaGem]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusManaGem] = value; }
        }
        [DisplayName("Ice Armor")]
        [Category("Mage")]
        [DefaultValueAttribute(0f)]
        public float MageIceArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageIceArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageIceArmor] = value; }
        }
        [DisplayName("Mage Armor")]
        [Category("Mage")]
        [DefaultValueAttribute(0f)]
        public float MageMageArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageMageArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageMageArmor] = value; }
        }
        [DisplayName("Molten Armor")]
        [Category("Mage")]
        [DefaultValueAttribute(0f)]
        public float MageMoltenArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageMoltenArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageMoltenArmor] = value; }
        }
        #endregion
        #endregion
        #region ===== Multiplicative Stats ============
        #region Stats Used by Almost Everyone
        #region ===== Core Stats =====
        // Primary
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Stamina")]
        public float BonusStaminaMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusStaminaMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusStaminaMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Strength")]
        public float BonusStrengthMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusStrengthMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusStrengthMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Agility")]
        public float BonusAgilityMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusAgilityMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusAgilityMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Intellect")]
        public float BonusIntellectMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusIntellectMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusIntellectMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Spirit")]
        public float BonusSpiritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpiritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpiritMultiplier] = value; }
        }
        // Secondary
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% AP")]
        public float BonusAttackPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusAttackPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusAttackPowerMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% SP")]
        public float BonusSpellPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellPowerMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Bonus Crit Damage")]
        public float BonusCritDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Spell Crit Damage")]
        public float BonusSpellCritDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellCritDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellCritDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Physical Haste")]
        [Percentage]
        [Category("Combat Values")]
        public float PhysicalHaste
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.PhysicalHaste]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.PhysicalHaste] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Ranged Haste")]
        [Percentage]
        [Category("Hunter")]
        public float RangedHaste
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.RangedHaste]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.RangedHaste] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Spell Haste")]
        [Percentage]
        [Category("Combat Values")]
        public float SpellHaste
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.SpellHaste]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.SpellHaste] = value; }
        }
        #endregion
        #region ===== Mana Related Stats =====
        [DefaultValueAttribute(0f)]
        [DisplayName("% Maximum Mana")]
        [Percentage]
        [Category("Equipment Effects")]
        public float BonusManaMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaMultiplier] = value; }
        }
        #endregion
        #region ===== Health Related Stats =====
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Health")]
        public float BonusHealthMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealthMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealthMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% HOT Heal")]
        public float BonusPeriodicHealingMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPeriodicHealingMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPeriodicHealingMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Healing Done")]
        public float BonusHealingDoneMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealingDoneMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealingDoneMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Healing Received")]
        public float HealingReceivedMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.HealingReceivedMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.HealingReceivedMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Crit Heal")]
        [Percentage]
        [Category("Equipment Effects")]
        public float BonusCritHealMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritHealMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritHealMultiplier] = value; }
        }
        #endregion
        #region ===== Offensive Stats =====
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Bleed Damage Multiplier")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float BonusBleedDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusBleedDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusBleedDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Damage Multiplier")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float BonusDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% White Damage Bonus Multiplier")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float BonusWhiteDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusWhiteDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusWhiteDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus DoT Damage Multiplier")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float BonusPeriodicDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPeriodicDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPeriodicDamageMultiplier] = value; }
        }
        #endregion
        #region ===== Defensive Stats =====
        /// <summary>
        /// Threat dealt is Damage * (1f + ThreatIncreaseMultiplier) * (1f - ThreatReductionMultiplier)
        /// </summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("% Threat Increase")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float ThreatIncreaseMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.ThreatIncreaseMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.ThreatIncreaseMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Base Armor")]
        public float BaseArmorMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BaseArmorMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BaseArmorMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Armor")]
        public float BonusArmorMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusArmorMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusArmorMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Block Value")]
        public float BonusBlockValueMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusBlockValueMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusBlockValueMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Damage Taken Reduction")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float DamageTakenReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.DamageTakenReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.DamageTakenReductionMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Spell Damage Taken Reduction")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float SpellDamageTakenReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.SpellDamageTakenReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.SpellDamageTakenReductionMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Physical Damage Taken Reduction")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float PhysicalDamageTakenReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.PhysicalDamageTakenReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.PhysicalDamageTakenReductionMultiplier] = value; }
        }
        #endregion
        #region ===== Target Stats =====
        [DefaultValueAttribute(0f)]
        [DisplayName("% Boss Attack Speed Reduction")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float BossAttackSpeedReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.BossAttackSpeedReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.BossAttackSpeedReductionMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Boss Physical Damage Dealt Reduction")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float BossPhysicalDamageDealtReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.BossPhysicalDamageDealtMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.BossPhysicalDamageDealtMultiplier] = value; }
        }
        #endregion
        #region ===== Item Proc Stats =====
        #endregion
        #region ===== Other Stats =====
        [DefaultValueAttribute(0f)]
        [DisplayName("% Arcane Damage")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float BonusArcaneDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusArcaneDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusArcaneDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Fire Damage")]
        public float BonusFireDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFireDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFireDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Frost Damage")]
        public float BonusFrostDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Nature Damage")]
        public float BonusNatureDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusNatureDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusNatureDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Physical Dmg")]
        public float BonusPhysicalDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPhysicalDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPhysicalDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Shadow Damage")]
        public float BonusShadowDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusShadowDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusShadowDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Holy Damage")]
        public float BonusHolyDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusHolyDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusHolyDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Disease Damage")]
        public float BonusDiseaseDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDiseaseDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDiseaseDamageMultiplier] = value; }
        }
        #endregion
        #region ===== Boss Handler Stats =====
        [DefaultValueAttribute(0f)]
        [DisplayName("% Fire Damage Taken Multiplier")]
        [Percentage]
        [Category("Boss Handler")]
        public float FireDamageTakenMultiplier {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.FireDamageTakenMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.FireDamageTakenMultiplier] = value; }
        }
        #endregion
        #endregion
        #region Added by Death Knights
        /// <summary>Razorice Rune (Enchant)</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("% White Damage as Frost Damage")]
        [Percentage]
        [Category("Death Knight")]
        public float BonusFrostWeaponDamage
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostWeaponDamage]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostWeaponDamage] = value; }
        }
        #endregion
        #region Added by Druids
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Lacerate Damage")]
        [Percentage]
        [Category("Feral")]
        public float BonusDamageMultiplierLacerate
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplierLacerate]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplierLacerate] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Rake Tick Damage Multiplier")]
        [Percentage]
        [Category("Feral")]
        public float BonusDamageMultiplierRakeTick
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplierRakeTick]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplierRakeTick] = value; }
        }
        #endregion
        #region Added by Hunters
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Hunter")]
        [CommonStat]
        public float BonusRangedAttackPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRangedAttackPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRangedAttackPowerMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Hunter")]
        [CommonStat]
        public float BonusPetAttackPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetAttackPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetAttackPowerMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Pet Damage")]
        [Percentage]
        [Category("Hunter")]
        public float BonusPetDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetDamageMultiplier] = value; }
        }
        #endregion
        #region Added by Paladins
        /// <summary>T11</summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Templar's Verdict Damage")]
        [Percentage]
        [Category("Retribution")]
        public float BonusDamageMultiplierTemplarsVerdict
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplierTemplarsVerdict]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplierTemplarsVerdict] = value; }
        }
        #endregion
        #region Added by Shamans
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Lava Burst Damage")]
        [Percentage]
        [Category("Elemental")]
        public float BonusDamageMultiplierLavaBurst
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplierLavaBurst]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplierLavaBurst] = value; }
        }
        #endregion
        #endregion
        #region ===== Inverse Multiplicative Stats ====
        #region Stats Used by Almost Everyone
        [DefaultValueAttribute(0f)]
        [DisplayName("% Armor Penetration")]
        [Percentage]
        [Category("Combat Values")]
        public float ArmorPenetration
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ArmorPenetration]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ArmorPenetration] = value; }
        }
        /// <summary>
        /// Threat dealt is<br/>
        /// Damage * (1f + stats.ThreatIncreaseMultiplier) * (1f - stats.ThreatReductionMultiplier)
        /// </summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("% Threat Reduction")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float ThreatReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ThreatReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ThreatReductionMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Target Armor Reduction")]
        [Percentage]
        [Category("Combat Values")]
        public float TargetArmorReduction
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.TargetArmorReduction]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.TargetArmorReduction] = value; }
        }
        /// <summary>
        /// This is a negative multiplier on *you*<br/>
        /// 0.50f = 50% Armor Reduction. Use as:<br/>
        /// Armor *= (1f - ArmorReductionMultiplier);
        /// </summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("% Player Armor Reduction")]
        [Percentage]
        [Category("Boss Handler")]
        public float ArmorReductionMultiplier {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ArmorReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ArmorReductionMultiplier] = value; }
        }
        /// <summary>
        /// 50% = Abilities cost 50% less than they normally would. An Ability with a Mana Cost of "10% of base mana" would become 5%.
        /// </summary>
        [DefaultValueAttribute(0f)]
        [DisplayName("% Reduction of Base Ability Mana Cost")]
        [Percentage]
        [Category("Buffs / Debuffs")]
        public float ManaCostReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ManaCostReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ManaCostReductionMultiplier] = value; }
        }
        #endregion
        #region Added by Death Knights
        [DefaultValueAttribute(0f)]
        [DisplayName("AMS Damage Reduction")]
        [Category("Death Knight")]
        public float AntiMagicShellDamageReduction
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.AntiMagicShellDamageReduction]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.AntiMagicShellDamageReduction] = value; }
        }
        #endregion
        #endregion
        #region ===== NonStacking Stats ===============
        #region Resistances
        [DefaultValueAttribute(0f)]
        [DisplayName("Arcane Resistance")]
        [Category("Resistances")]
        public float ArcaneResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.ArcaneResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.ArcaneResistanceBuff] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Fire Resistance")]
        [Category("Resistances")]
        public float FireResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.FireResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.FireResistanceBuff] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Frost Resistance")]
        [Category("Resistances")]
        public float FrostResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.FrostResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.FrostResistanceBuff] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Nature Resistance")]
        [Category("Resistances")]
        public float NatureResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.NatureResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.NatureResistanceBuff] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Shadow Resistance")]
        [Category("Resistances")]
        public float ShadowResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.ShadowResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.ShadowResistanceBuff] = value; }
        }
        #endregion
        #region Pots
        [DefaultValueAttribute(0f)]
        [DisplayName("% Bonus Mana Potion Effect")]
        [Percentage]
        [Category("Equipment Effects")]
        public float BonusManaPotionEffectMultiplier
        {
            get { return _rawNoStackData[(int)NonStackingStat.BonusManaPotionEffectMultiplier]; }
            set { _rawNoStackData[(int)NonStackingStat.BonusManaPotionEffectMultiplier] = value; }
        }
        #endregion
        #region Boss Handler
        [DefaultValueAttribute(0f)]
        [DisplayName("% Increased Movement Speed")]
        [Percentage]
        [Category("Boss Handler")]
        public float MovementSpeed
        {
            get { return _rawNoStackData[(int)NonStackingStat.MovementSpeed]; }
            set { _rawNoStackData[(int)NonStackingStat.MovementSpeed] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Silence Duration Reduction")]
        [Percentage]
        [Category("Boss Handler")]
        public float SilenceDurReduc
        {
            get { return _rawNoStackData[(int)NonStackingStat.SilenceDurReduc]; }
            set { _rawNoStackData[(int)NonStackingStat.SilenceDurReduc] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Stun Duration Reduction")]
        [Percentage]
        [Category("Boss Handler")]
        public float StunDurReduc
        {
            get { return _rawNoStackData[(int)NonStackingStat.StunDurReduc]; }
            set { _rawNoStackData[(int)NonStackingStat.StunDurReduc] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Snare/Root Duration Reduction")]
        [Percentage]
        [Category("Boss Handler")]
        public float SnareRootDurReduc
        {
            get { return _rawNoStackData[(int)NonStackingStat.SnareRootDurReduc]; }
            set { _rawNoStackData[(int)NonStackingStat.SnareRootDurReduc] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Fear Duration Reduction")]
        [Percentage]
        [Category("Boss Handler")]
        public float FearDurReduc
        {
            get { return _rawNoStackData[(int)NonStackingStat.FearDurReduc]; }
            set { _rawNoStackData[(int)NonStackingStat.FearDurReduc] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Disarm Duration Reduction")]
        [Percentage]
        [Category("Boss Handler")]
        public float DisarmDurReduc
        {
            get { return _rawNoStackData[(int)NonStackingStat.DisarmDurReduc]; }
            set { _rawNoStackData[(int)NonStackingStat.DisarmDurReduc] = value; }
        }
        #endregion
        #region Special Procs
        [DefaultValueAttribute(0f)]
        [DisplayName("Highest Stat")]
        [Category("Equipment Effects")]
        public float HighestStat
        {
            get { return _rawNoStackData[(int)NonStackingStat.HighestStat]; }
            set { _rawNoStackData[(int)NonStackingStat.HighestStat] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Highest Secondary Stat")]
        [Category("Equipment Effects")]
        public float HighestSecondaryStat
        {
            get { return _rawNoStackData[(int)NonStackingStat.HighestSecondaryStat]; }
            set { _rawNoStackData[(int)NonStackingStat.HighestSecondaryStat] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("% Shield from Heal Amount")]
        [Percentage]
        [Category("Equipment Effects")]
        public float ShieldFromHealedProc
        {
            get { return _rawNoStackData[(int)NonStackingStat.ShieldFromHealed]; }
            set { _rawNoStackData[(int)NonStackingStat.ShieldFromHealed] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Temporary Health")]
        [Category("Equipment Effects")]
        public float BattlemasterHealthProc
        {
            get { return _rawNoStackData[(int)NonStackingStat.BattlemasterHealthProc]; }
            set { _rawNoStackData[(int)NonStackingStat.BattlemasterHealthProc] = value; }
        }
        #endregion
        #region Added by Paladins
        [DefaultValueAttribute(0f)]
        [DisplayName("Inq Holy Power")]
        [Category("Retribution")]
        public float BonusRet_T11_4P_InqHP
        {
            get { return _rawNoStackData[(int)NonStackingStat.BonusRet_T11_4P_InqHP]; }
            set { _rawNoStackData[(int)NonStackingStat.BonusRet_T11_4P_InqHP] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("sec Judgement CD Reduction")]
        [Category("Retribution")]
        public float JudgementCDReduction
        {
            get { return _rawNoStackData[(int)NonStackingStat.JudgementCDReduction]; }
            set { _rawNoStackData[(int)NonStackingStat.JudgementCDReduction] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Righteous Vengeance Can Crit")]
        [Category("Retribution")]
        public float RighteousVengeanceCanCrit
        {
            get { return _rawNoStackData[(int)NonStackingStat.RighteousVengeanceCanCrit]; }
            set { _rawNoStackData[(int)NonStackingStat.RighteousVengeanceCanCrit] = value; }
        }
        #endregion
        #region Added by Rogues
        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float Rogue_T11_2P
        {
            get { return _rawNoStackData[(int)NonStackingStat.Rogue_T11_2P]; }
            set { _rawNoStackData[(int)NonStackingStat.Rogue_T11_2P] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float Rogue_T11_4P
        {
            get { return _rawNoStackData[(int)NonStackingStat.Rogue_T11_4P]; }
            set { _rawNoStackData[(int)NonStackingStat.Rogue_T11_4P] = value; }
        }
        #endregion
        #region Added by Shamans
        [DefaultValueAttribute(0f)]
        [DisplayName("Enhance T11 2 Piece Bonus")]
        [Category("Enhance")]
        public float Enhance_T11_2P
        {
            get { return _rawNoStackData[(int)NonStackingStat.Enhance_T11_2P]; }
            set { _rawNoStackData[(int)NonStackingStat.Enhance_T11_2P] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Enhance T11 4 Piece Bonus")]
        [Category("Enhance")]
        public float Enhance_T11_4P
        {
            get { return _rawNoStackData[(int)NonStackingStat.Enhance_T11_4P]; }
            set { _rawNoStackData[(int)NonStackingStat.Enhance_T11_4P] = value; }
        }
        #endregion
        #region Added by Warlocks
        [DefaultValueAttribute(0f)]
        [DisplayName("Reduces the cast time of your Chaos Bolt, Hand of Gul'dan, and Haunt spells by 10%.")]
        [Percentage]
        [Category("Warlock")]
        public float Warlock_T11_2P
        {
            get { return _rawNoStackData[(int)NonStackingStat.Warlock_T11_2P]; }
            set { _rawNoStackData[(int)NonStackingStat.Warlock_T11_2P] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Periodic damage from your Immolate and Unstable Affliction spells has a 2% chance to cause your 2 next Fel Flame spells to deal 300% increased damage.")]
        [Percentage]
        [Category("Warlock")]
        public float Warlock_T11_4P
        {
            get { return _rawNoStackData[(int)NonStackingStat.Warlock_T11_4P]; }
            set { _rawNoStackData[(int)NonStackingStat.Warlock_T11_4P] = value; }
        }
        #endregion
        #endregion

        #region Special Effects in the Stats Class
        public void EnsureSpecialEffectCapacity(int min)
        {
            if (_rawSpecialEffectData == null || _rawSpecialEffectData.Length < min)
            {
                int num = (_rawSpecialEffectData == null || _rawSpecialEffectData.Length == 0) ? _defaultSpecialEffectDataCapacity : (_rawSpecialEffectData.Length * 2);
                if (num < min)
                {
                    num = min;
                }
                SpecialEffect[] destinationArray = new SpecialEffect[num];
                if (_rawSpecialEffectDataSize > 0)
                {
                    Array.Copy(_rawSpecialEffectData, 0, destinationArray, 0, _rawSpecialEffectDataSize);
                }
                _rawSpecialEffectData = destinationArray;
            }
        }
        public void AddSpecialEffect(SpecialEffect specialEffect)
        {
            EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + 1);
            _rawSpecialEffectData[_rawSpecialEffectDataSize] = specialEffect;
            _rawSpecialEffectDataSize++;
        }
        public void RemoveSpecialEffect(SpecialEffect specialEffect)
        {
            bool found = false;
            for (int i = 0; i < _rawSpecialEffectDataSize; i++)
            {
                if (found)
                {
                    _rawSpecialEffectData[i - 1] = _rawSpecialEffectData[i];
                }
                else if (specialEffect == _rawSpecialEffectData[i])
                {
                    found = true;
                }
            }
            if (found)
            {
                _rawSpecialEffectDataSize--;
                _rawSpecialEffectData[_rawSpecialEffectDataSize] = null;
            }
        }
        public void ClearSpecialEffects()
        {
            _rawSpecialEffectDataSize = 0;
            _rawSpecialEffectData = null;
        }
        public struct SpecialEffectEnumerator : IEnumerator<SpecialEffect>, IDisposable, System.Collections.IEnumerator, IEnumerable<SpecialEffect>
        {
            internal Stats stats;
            private int index;
            private SpecialEffect current;
            internal Predicate<SpecialEffect> match;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                while (index < stats._rawSpecialEffectDataSize)
                {
                    current = stats._rawSpecialEffectData[index];
                    index++;
                    if (match == null || match(current)) return true;
                }
                index = stats._rawSpecialEffectDataSize + 1;
                current = null;
                return false;
            }

            public SpecialEffect Current
            {
                get
                {
                    return current;
                }
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if ((index == 0) || (index == (stats._rawSpecialEffectDataSize + 1)))
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                index = 0;
                current = null;
            }

            public SpecialEffectEnumerator GetEnumerator()
            {
                return this;
            }

            IEnumerator<SpecialEffect> System.Collections.Generic.IEnumerable<SpecialEffect>.GetEnumerator()
            {
                return this;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this;
            }
        }
        public SpecialEffectEnumerator SpecialEffects()
        {
            return new SpecialEffectEnumerator() { stats = this };
        }
        public SpecialEffectEnumerator SpecialEffects(Predicate<SpecialEffect> match)
        {
            return new SpecialEffectEnumerator() { stats = this, match = match };
        }
        public bool ContainsSpecialEffect(Predicate<SpecialEffect> match)
        {
            for (int i = 0; i < _rawSpecialEffectDataSize; i++)
            {
                SpecialEffect effect = _rawSpecialEffectData[i];
                if (match(effect)) return true;
            }
            return false;
        }
        public bool ContainsSpecialEffect()
        {
            return _rawSpecialEffectDataSize > 0;
        }
        #endregion

        #region Operators (+ - * == != > >= < <=)
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
            while (--i >= 0)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] + b._rawAdditiveData[i];
            }
            i = c._rawMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawMultiplicativeData[i] = (1 + a._rawMultiplicativeData[i]) * (1 + b._rawMultiplicativeData[i]) - 1;
            }
            i = c._rawInverseMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawInverseMultiplicativeData[i] = 1 - (1 - a._rawInverseMultiplicativeData[i]) * (1 - b._rawInverseMultiplicativeData[i]);
            }

            i = c._rawNoStackData.Length;
            while (--i >= 0)
            {
                c._rawNoStackData[i] = Math.Max(a._rawNoStackData[i], b._rawNoStackData[i]);
            }
            int specialEffectCount = a._rawSpecialEffectDataSize + b._rawSpecialEffectDataSize;
            if (specialEffectCount > 0)
            {
                c._rawSpecialEffectData = new SpecialEffect[specialEffectCount];
                c._rawSpecialEffectDataSize = specialEffectCount;
                if (a._rawSpecialEffectDataSize > 0)
                {
                    Array.Copy(a._rawSpecialEffectData, c._rawSpecialEffectData, a._rawSpecialEffectDataSize);
                }
                if (b._rawSpecialEffectDataSize > 0)
                {
                    Array.Copy(b._rawSpecialEffectData, 0, c._rawSpecialEffectData, a._rawSpecialEffectDataSize, b._rawSpecialEffectDataSize);
                }
            }
            return c;
        }
        /// <summary>
        /// Adds together two stats, when using a + operator. When adding additional stats for
        /// Rawr to track, after adding the stat property, also add a line for it to this method,
        /// to properly combine the stat, as appropriate.
        /// </summary>
        /// <param name="a">The first Stats object to combine.</param>
        /// <param name="b">The second Stats object to combine.</param>
        /// <returns>The combined Stats object.</returns>
        public static Stats operator -(Stats a, Stats b)
        {
            Stats c = new Stats();

            int i = c._rawAdditiveData.Length;
            while (--i >= 0)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] - b._rawAdditiveData[i];
            }
            i = c._rawMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawMultiplicativeData[i] = (1 + a._rawMultiplicativeData[i]) / (1 + b._rawMultiplicativeData[i]) - 1;
            }
            i = c._rawInverseMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawInverseMultiplicativeData[i] = 1 - (1 - a._rawInverseMultiplicativeData[i]) / (1 - b._rawInverseMultiplicativeData[i]);
            }

            //i = c._rawNoStackData.Length;
            //while (--i >= 0)
            //{
            //    c._rawNoStackData[i] = Math.Max(a._rawNoStackData[i], b._rawNoStackData[i]);
            //}
            //int specialEffectCount = a._rawSpecialEffectDataSize + b._rawSpecialEffectDataSize;
            //if (specialEffectCount > 0)
            //{
            //    c._rawSpecialEffectData = new SpecialEffect[specialEffectCount];
            //    c._rawSpecialEffectDataSize = specialEffectCount;
            //    if (a._rawSpecialEffectDataSize > 0)
            //    {
            //        Array.Copy(a._rawSpecialEffectData, c._rawSpecialEffectData, a._rawSpecialEffectDataSize);
            //    }
            //    if (b._rawSpecialEffectDataSize > 0)
            //    {
            //        Array.Copy(b._rawSpecialEffectData, 0, c._rawSpecialEffectData, a._rawSpecialEffectDataSize, b._rawSpecialEffectDataSize);
            //    }
            //}
            return c;
        }
        /// <summary>
        /// Multiplies every stat in a stats by a float, when using a * operator.
        /// </summary>
        /// <param name="a">The Stats object to multiply.</param>
        /// <param name="b">The float by which to multiply every stat.</param>
        /// <returns>The new Stats object.</returns>
        public static Stats operator *(Stats a, float b)
        {
            Stats c = new Stats();

            int i = c._rawAdditiveData.Length;
            while (--i >= 0)
            {
                c._rawAdditiveData[i] = a._rawAdditiveData[i] * b;
            }
            i = c._rawMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawMultiplicativeData[i] = a._rawMultiplicativeData[i] * b;
            }
            i = c._rawInverseMultiplicativeData.Length;
            while (--i >= 0)
            {
                c._rawInverseMultiplicativeData[i] = a._rawInverseMultiplicativeData[i] * b;
            }

            i = c._rawNoStackData.Length;
            while (--i >= 0)
            {
                c._rawNoStackData[i] = a._rawNoStackData[i] * b;
            }
            // undefined for special effects
            return c;
        }
        public static bool operator ==(Stats x, Stats y)
        {
            if (ReferenceEquals(x, y) || (ReferenceEquals(x, null) && ReferenceEquals(y, null)))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            if (x._rawSpecialEffectDataSize > 0 || y._rawSpecialEffectDataSize > 0) return false;
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
            return x >= y && x != y;
        }
        public static bool operator >=(Stats x, Stats y)
        {
            return AllCompare(x, y, ArrayUtils.CompareOption.GreaterThan | ArrayUtils.CompareOption.Equal);
        }
        public static bool operator <(Stats x, Stats y)
        {
            return x <= y && x != y;
        }
        public static bool operator <=(Stats x, Stats y)
        {
            return AllCompare(x, y, ArrayUtils.CompareOption.LessThan | ArrayUtils.CompareOption.Equal);
        }
        public bool Equals(Stats other)
        {
            return this == other;
        }
        public ArrayUtils.CompareResult CompareTo(Stats other)
        {
            if (ReferenceEquals(other, null))
                return 0;
            if (this._rawSpecialEffectDataSize > 0 || other._rawSpecialEffectDataSize > 0)
            {
                return ArrayUtils.CompareResult.Unequal; // not sure if we need to actually go through and compare each
            }
            if (this._sparseIndices != null && other._sparseIndices != null)
            {
                bool haveGreaterThan = false, haveLessThan = false;
                int j = 0;
                int i = 0;
                int b = 0;
                for (int a = 0; a < other._sparseAdditiveCount; a++, i++)
                {
                    int index = other._sparseIndices[i];
                    while (b < _sparseAdditiveCount && _sparseIndices[j] < index)
                    {
                        b++;
                        j++;
                        haveGreaterThan = true;
                        if (haveLessThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                    if (b < _sparseAdditiveCount && _sparseIndices[j] == index)
                    {
                        int val = _rawAdditiveData[index].CompareTo(other._rawAdditiveData[index]);
                        if (val < 0)
                        {
                            haveLessThan = true;
                            if (haveGreaterThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        else if (val > 0)
                        {
                            haveGreaterThan = true;
                            if (haveLessThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        b++;
                        j++;
                    }
                    else
                    {
                        haveLessThan = true;
                        if (haveGreaterThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                }
                while (b < _sparseAdditiveCount)
                {
                    b++;
                    j++;
                    haveGreaterThan = true;
                    if (haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }
                b = 0;
                for (int a = 0; a < other._sparseMultiplicativeCount; a++, i++)
                {
                    int index = other._sparseIndices[i];
                    while (b < _sparseMultiplicativeCount && _sparseIndices[j] < index)
                    {
                        b++;
                        j++;
                        haveGreaterThan = true;
                        if (haveLessThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                    if (b < _sparseMultiplicativeCount && _sparseIndices[j] == index)
                    {
                        int val = _rawMultiplicativeData[index].CompareTo(other._rawMultiplicativeData[index]);
                        if (val < 0)
                        {
                            haveLessThan = true;
                            if (haveGreaterThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        else if (val > 0)
                        {
                            haveGreaterThan = true;
                            if (haveLessThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        b++;
                        j++;
                    }
                    else
                    {
                        haveLessThan = true;
                        if (haveGreaterThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                }
                while (b < _sparseMultiplicativeCount)
                {
                    b++;
                    j++;
                    haveGreaterThan = true;
                    if (haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }
                b = 0;
                for (int a = 0; a < other._sparseInverseMultiplicativeCount; a++, i++)
                {
                    int index = other._sparseIndices[i];
                    while (b < _sparseInverseMultiplicativeCount && _sparseIndices[j] < index)
                    {
                        b++;
                        j++;
                        haveGreaterThan = true;
                        if (haveLessThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                    if (b < _sparseInverseMultiplicativeCount && _sparseIndices[j] == index)
                    {
                        int val = _rawInverseMultiplicativeData[index].CompareTo(other._rawInverseMultiplicativeData[index]);
                        if (val < 0)
                        {
                            haveLessThan = true;
                            if (haveGreaterThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        else if (val > 0)
                        {
                            haveGreaterThan = true;
                            if (haveLessThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        b++;
                        j++;
                    }
                    else
                    {
                        haveLessThan = true;
                        if (haveGreaterThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                }
                while (b < _sparseInverseMultiplicativeCount)
                {
                    b++;
                    j++;
                    haveGreaterThan = true;
                    if (haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }
                b = 0;
                for (int a = 0; a < other._sparseNoStackCount; a++, i++)
                {
                    int index = other._sparseIndices[i];
                    while (b < _sparseNoStackCount && _sparseIndices[j] < index)
                    {
                        b++;
                        j++;
                        haveGreaterThan = true;
                        if (haveLessThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                    if (b < _sparseNoStackCount && _sparseIndices[j] == index)
                    {
                        int val = _rawNoStackData[index].CompareTo(other._rawNoStackData[index]);
                        if (val < 0)
                        {
                            haveLessThan = true;
                            if (haveGreaterThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        else if (val > 0)
                        {
                            haveGreaterThan = true;
                            if (haveLessThan)
                            {
                                return ArrayUtils.CompareResult.Unequal;
                            }
                        }
                        b++;
                        j++;
                    }
                    else
                    {
                        haveLessThan = true;
                        if (haveGreaterThan)
                        {
                            return ArrayUtils.CompareResult.Unequal;
                        }
                    }
                }
                while (b < _sparseNoStackCount)
                {
                    b++;
                    j++;
                    haveGreaterThan = true;
                    if (haveLessThan)
                    {
                        return ArrayUtils.CompareResult.Unequal;
                    }
                }
                if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;
                else if (haveGreaterThan) return ArrayUtils.CompareResult.GreaterThan;
                else if (haveLessThan) return ArrayUtils.CompareResult.LessThan;
                else return ArrayUtils.CompareResult.Equal;
            }
            else
            {
                return ArrayUtils.And(ArrayUtils.AllCompare(this._rawAdditiveData, other._rawAdditiveData), ArrayUtils.And(
                    ArrayUtils.AllCompare(this._rawMultiplicativeData, other._rawMultiplicativeData),
                    ArrayUtils.AllCompare(this._rawNoStackData, other._rawNoStackData)));
            }
        }
        public override int GetHashCode() { return _rawAdditiveData.GetHashCode(); }
        public override bool Equals(object obj)
        {
            return this == (obj as Stats);
        }
        private static bool AllCompare(Stats x, Stats y, ArrayUtils.CompareOption comparison)
        {
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                throw new ArgumentNullException();
            if (x._rawSpecialEffectDataSize > 0 || y._rawSpecialEffectDataSize > 0) return false;
            return ArrayUtils.AllCompare(x._rawAdditiveData, y._rawAdditiveData, comparison)
                && ArrayUtils.AllCompare(x._rawMultiplicativeData, y._rawMultiplicativeData, comparison)
                && ArrayUtils.AllCompare(x._rawInverseMultiplicativeData, y._rawInverseMultiplicativeData, comparison)
                && ArrayUtils.AllCompare(x._rawNoStackData, y._rawNoStackData, comparison);
        }
        #endregion

        public void Accumulate(Stats data)
        {
            if (data._sparseIndices != null)
            {
                int i = 0;
                for (int a = 0; a < data._sparseAdditiveCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    _rawAdditiveData[index] += data._rawAdditiveData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativeCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    _rawMultiplicativeData[index] = (1 + _rawMultiplicativeData[index]) * (1 + data._rawMultiplicativeData[index]) - 1;
                }
                for (int a = 0; a < data._sparseInverseMultiplicativeCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    _rawInverseMultiplicativeData[index] = 1 - (1 - _rawInverseMultiplicativeData[index]) * (1 - data._rawInverseMultiplicativeData[index]);
                }
                for (int a = 0; a < data._sparseNoStackCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    float value = data._rawNoStackData[index];
                    if (value > _rawNoStackData[index]) _rawNoStackData[index] = value;
                }
            }
            else
            {
                float[] add = data._rawAdditiveData;
                for (int i = 0; i < _rawAdditiveData.Length; i++)
                {
                    _rawAdditiveData[i] += add[i];
                }
                add = data._rawMultiplicativeData;
                for (int i = 0; i < _rawMultiplicativeData.Length; i++)
                {
                    _rawMultiplicativeData[i] = (1 + _rawMultiplicativeData[i]) * (1 + add[i]) - 1;
                }
                add = data._rawInverseMultiplicativeData;
                for (int i = 0; i < _rawInverseMultiplicativeData.Length; i++)
                {
                    _rawInverseMultiplicativeData[i] = 1 - (1 - _rawInverseMultiplicativeData[i]) * (1 - add[i]);
                }
                add = data._rawNoStackData;
                for (int i = 0; i < _rawNoStackData.Length; i++)
                {
                    if (add[i] > _rawNoStackData[i]) _rawNoStackData[i] = add[i];
                }
            }
            if (data._rawSpecialEffectDataSize > 0)
            {
                EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + data._rawSpecialEffectDataSize);
                Array.Copy(data._rawSpecialEffectData, 0, _rawSpecialEffectData, _rawSpecialEffectDataSize, data._rawSpecialEffectDataSize);
                _rawSpecialEffectDataSize += data._rawSpecialEffectDataSize;
            }
        }
        public void Accumulate(Stats data, float weight)
        {
            if (data._sparseIndices != null)
            {
                int i = 0;
                for (int a = 0; a < data._sparseAdditiveCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    _rawAdditiveData[index] += weight * data._rawAdditiveData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativeCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    _rawMultiplicativeData[index] = (1 + _rawMultiplicativeData[index]) * (1 + weight * data._rawMultiplicativeData[index]) - 1;
                }
                for (int a = 0; a < data._sparseInverseMultiplicativeCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    _rawInverseMultiplicativeData[index] = 1 - (1 - _rawInverseMultiplicativeData[index]) * (1 - weight * data._rawInverseMultiplicativeData[index]);
                }
                for (int a = 0; a < data._sparseNoStackCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    float value = weight * data._rawNoStackData[index];
                    if (value > _rawNoStackData[index]) _rawNoStackData[index] = value;
                }
            }
            else
            {
                float[] add = data._rawAdditiveData;
                for (int i = 0; i < _rawAdditiveData.Length; i++)
                {
                    _rawAdditiveData[i] += weight * add[i];
                }
                add = data._rawMultiplicativeData;
                for (int i = 0; i < _rawMultiplicativeData.Length; i++)
                {
                    _rawMultiplicativeData[i] = (1 + _rawMultiplicativeData[i]) * (1 + weight * add[i]) - 1;
                }
                add = data._rawInverseMultiplicativeData;
                for (int i = 0; i < _rawInverseMultiplicativeData.Length; i++)
                {
                    _rawInverseMultiplicativeData[i] = 1 - (1 - _rawInverseMultiplicativeData[i]) * (1 - weight * add[i]);
                }
                add = data._rawNoStackData;
                for (int i = 0; i < _rawNoStackData.Length; i++)
                {
                    if (weight * add[i] > _rawNoStackData[i]) _rawNoStackData[i] = weight * add[i];
                }
            }
            if (data._rawSpecialEffectDataSize > 0)
            {
                EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + data._rawSpecialEffectDataSize);
                Array.Copy(data._rawSpecialEffectData, 0, _rawSpecialEffectData, _rawSpecialEffectDataSize, data._rawSpecialEffectDataSize);
                _rawSpecialEffectDataSize += data._rawSpecialEffectDataSize;
            }
        }
#if SILVERLIGHT
        public void AccumulateUnsafe(Stats data)
        {
            Accumulate(data);
        }
        public void AccumulateUnsafe(Stats data, bool generateSparseIfNeeded)
        {
            if (generateSparseIfNeeded && data._sparseIndices == null) data.GenerateSparseData();
            Accumulate(data);
        }
#else
        private float* pRawAdditiveData;
        private float* pRawMultiplicativeData;
        private float* pRawNoStackData;

        public void BeginUnsafe(float* pRawAdditiveData, float* pRawMultiplicativeData, float* pRawNoStackData)
        {
            this.pRawAdditiveData = pRawAdditiveData;
            this.pRawMultiplicativeData = pRawMultiplicativeData;
            this.pRawNoStackData = pRawNoStackData;
        }

        public void EndUnsafe()
        {
            pRawAdditiveData = null;
            pRawMultiplicativeData = null;
            pRawNoStackData = null;
        }

        public void AccumulateUnsafe(Stats data)
        {
            AccumulateUnsafe(data, false);
        }

        public void AccumulateUnsafe(Stats data, bool generateSparseIfNeeded)
        {
            if (generateSparseIfNeeded && data._sparseIndices == null) data.GenerateSparseData();
            if (data._sparseIndices != null)
            {
                int i = 0;
                float* pRawAdditiveData = this.pRawAdditiveData;
                int limit = data._sparseAdditiveCount;
                for (; i < limit; i++)
                {
                    int index = data._sparseIndices[i];
                    pRawAdditiveData[index] += data._rawAdditiveData[index];
                }
                float* pRawMultiplicativeData = this.pRawMultiplicativeData;
                limit += data._sparseMultiplicativeCount;
                for (; i < limit; i++)
                {
                    int index = data._sparseIndices[i];
                    float* pa = pRawMultiplicativeData + index;
                    *pa = (1 + *pa) * (1 + data._rawMultiplicativeData[index]) - 1;
                }
                float[] _rawInverseMultiplicativeData = this._rawInverseMultiplicativeData;
                limit += data._sparseInverseMultiplicativeCount;
                for (; i < limit; i++)
                {
                    int index = data._sparseIndices[i];
                    _rawInverseMultiplicativeData[index] = 1 - (1 - _rawInverseMultiplicativeData[index]) * (1 - data._rawInverseMultiplicativeData[index]);
                }
                float* pRawNoStackData = this.pRawNoStackData;
                limit += data._sparseNoStackCount;
                for (; i < limit; i++)
                {
                    int index = data._sparseIndices[i];
                    float* pa = pRawNoStackData + index;
                    float value = data._rawNoStackData[index];
                    if (value > *pa) *pa = value;
                }
            }
            else
            {
                AccumulateUnsafeDense(data);
            }
            if (data._rawSpecialEffectDataSize > 0)
            {
                EnsureSpecialEffectCapacity(_rawSpecialEffectDataSize + data._rawSpecialEffectDataSize);
                if (data._rawSpecialEffectDataSize == 1)
                {
                    // special case for majority case
                    _rawSpecialEffectData[_rawSpecialEffectDataSize++] = data._rawSpecialEffectData[0];
                }
                else
                {
                    Array.Copy(data._rawSpecialEffectData, 0, _rawSpecialEffectData, _rawSpecialEffectDataSize, data._rawSpecialEffectDataSize);
                    _rawSpecialEffectDataSize += data._rawSpecialEffectDataSize;
                }
            }
        }

        private void AccumulateUnsafeDense(Stats data)
        {
            fixed (float* add = data._rawAdditiveData)
            {
                float* pa = pRawAdditiveData;
                float* paend = pa + AdditiveStatCount;
                float* pa2 = add;
                for (; pa < paend; pa++, pa2++)
                {
                    *pa += *pa2;
                }
            }
            fixed (float* add = data._rawMultiplicativeData)
            {
                float* pa = pRawMultiplicativeData;
                float* paend = pa + MultiplicativeStatCount;
                float* pa2 = add;
                for (; pa < paend; pa++, pa2++)
                {
                    *pa = (1 + *pa) * (1 + *pa2) - 1;
                }
            }
            float[] arr = data._rawInverseMultiplicativeData;
            for (int i = 0; i < _rawInverseMultiplicativeData.Length; i++)
            {
                _rawInverseMultiplicativeData[i] = 1 - (1 - _rawInverseMultiplicativeData[i]) * (1 - arr[i]);
            }
            fixed (float* add = data._rawNoStackData)
            {
                float* pa = pRawNoStackData;
                float* paend = pa + NonStackingStatCount;
                float* pa2 = add;
                for (; pa < paend; pa++, pa2++)
                {
                    if (*pa2 > *pa) *pa = *pa2;
                }
            }
        }
#endif

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo info in PropertyInfoCache)
            {
                float value = (float)info.GetValue(this, null);
                if (value != 0)
                {
                    if (IsPercentage(info))
                    {
                        value *= 100;
                    }

                    value = (float)Math.Round(value * 100f) / 100f;

                    sb.AppendFormat("{0}{1}, ", value, Extensions.DisplayName(info));
                }
            }
            foreach (SpecialEffect effect in SpecialEffects())
            {
                sb.AppendFormat("{0}, ", effect);
            }

            return sb.ToString().TrimEnd(' ', ',');
        }

        public Stats Clone()
        {
            Stats clone = (Stats)this.MemberwiseClone();
            clone._rawAdditiveData = (float[])clone._rawAdditiveData.Clone();
            clone._rawMultiplicativeData = (float[])clone._rawMultiplicativeData.Clone();
            clone._rawInverseMultiplicativeData = (float[])clone._rawInverseMultiplicativeData.Clone();
            clone._rawNoStackData = (float[])clone._rawNoStackData.Clone();
            if (_rawSpecialEffectData != null) clone._rawSpecialEffectData = (SpecialEffect[])_rawSpecialEffectData.Clone();
            return clone;
        }

        #region Multiplicative Handling
        [XmlIgnore]
        public static PropertyInfo[] _propertyInfoCache = null;
        [XmlIgnore]
        public static List<PropertyInfo> _percentageProperties = new List<PropertyInfo>();
        [XmlIgnore]
        public static int AdditiveStatCount = 0;
        [XmlIgnore]
        public static int MultiplicativeStatCount = 0;
        [XmlIgnore]
        public static int InverseMultiplicativeStatCount = 0;
        [XmlIgnore]
        public static int NonStackingStatCount = 0;

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
                if (info.GetCustomAttributes(typeof(PercentageAttribute), false).Length > 0)
                {
                    _percentageProperties.Add(info);
                }
            }

            AdditiveStatCount = EnumHelper.GetValues(typeof(AdditiveStat)).Length;
            MultiplicativeStatCount = EnumHelper.GetValues(typeof(MultiplicativeStat)).Length;
            InverseMultiplicativeStatCount = EnumHelper.GetValues(typeof(InverseMultiplicativeStat)).Length;
            NonStackingStatCount = EnumHelper.GetValues(typeof(NonStackingStat)).Length;
        }

        public static PropertyInfo[] PropertyInfoCache
        {
            get
            {
                return _propertyInfoCache;
            }
        }

        public static bool IsPercentage(PropertyInfo info)
        {
            return _percentageProperties.Contains(info);
        }
        #endregion

        public class PropertyComparer : IComparer<PropertyInfo>
        {
            public int Compare(PropertyInfo x, PropertyInfo y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        [XmlIgnore]
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
#if SILVERLIGHT
            Dictionary<PropertyInfo, float> dict = new Dictionary<PropertyInfo, float>();
#else
            SortedList<PropertyInfo, float> dict = new SortedList<PropertyInfo, float>(new PropertyComparer());
#endif
            foreach (PropertyInfo info in PropertyInfoCache)
            {
                if (info.PropertyType.IsAssignableFrom(typeof(float)))
                {
                    float value = (float)info.GetValue(this, null);
                    if (filter(value))
                    {
                        dict[info] = value;
                    }
                }
            }
#if SILVERLIGHT
            dict.OrderBy(kvp => kvp.Key, new PropertyComparer());
#endif
            return dict;
        }
    }

    public static class Extensions {
        // requires .net 3.5 public static string LongName(this PropertyInfo info)
        // allows it to be called like
        //   info.LongName()
        // instead of
        //   Extensions.LongName(info)

        public static PropertyInfo UnDisplayName(string displayName) {
            foreach (PropertyInfo info in Stats.PropertyInfoCache) {
                if (DisplayName(info).Trim() == displayName.Trim())
                    return info;
            }
            return null;
        }

        public static string DisplayName(PropertyInfo info) {
            string prettyName = null;

            object[] attributes = info.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (attributes.Length == 1 && attributes[0] is DisplayNameAttribute && (attributes[0] as DisplayNameAttribute).DisplayName != null) {
                prettyName = (attributes[0] as DisplayNameAttribute).DisplayName;
            } else {
                prettyName = SpaceCamel(info.Name);
            }
            if (!prettyName.StartsWith("%"))
                prettyName = " " + prettyName;
            return prettyName;
        }
        public static string SpaceCamel(String name) {
            return System.Text.RegularExpressions.Regex.Replace(
                    name,
                    "([A-Z])",
                    " $1"
#if SILVERLIGHT
                    ).Trim();
#else
                    ,System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
#endif
        }
        public static string UnSpaceCamel(String name) {
            return System.Text.RegularExpressions.Regex.Replace(
                    name,
                    "( )",
                    ""
#if SILVERLIGHT
                    ).Trim();
#else
                    ,System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
#endif
        }
    }
}
