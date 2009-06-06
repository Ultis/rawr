using System;
using System.Net;
using System.ComponentModel;

namespace Rawr
{
    enum AdditiveStat : int
    {
        Agility,
        AllResist,
        ArcaneResistance,
        Armor,
        BonusArmor,
        ArmorPenetrationRating,
        AshtongueTrinketProc,
        AttackPower,
        AverageAgility,
        AverageArmor,
        BaseAgility,
        Block,
        BlockRating,
        BlockValue,
        Bloodlust,
        BloodlustProc,
        BonusCommandingShoutHP,
        BonusLacerateDamageMultiplier,
        BonusMangleBearDamage,
        BonusMangleCatDamage,
        BonusRipDamagePerCPPerTick,
        BonusRipDuration,
        BonusSavageRoarDuration,
        BonusShredDamage,
        BonusStreadyShotCrit,
        ExtraSpiritWhileCasting,
        CatFormStrength,
        ClearcastOnBleedChance,
        PhysicalCrit,
        CritRating,
        CritMeleeRating,
        CritBonusDamage,
        CritChanceReduction,
        Defense,
        DefenseRating,
        Dodge,
        DodgeRating,
        DrumsOfBattle,
        DrumsOfWar,
        Expertise,
        ExpertiseRating,
        ExposeWeakness,
        FireResistance,
        FrostResistance,
        HasteRating,
        HasteRatingOnPhysicalAttack,
        Healing,
        Health,
        PhysicalHit,
        HitRating,
        Hp5,
        IdolCritRating,
        InnervateCooldownReduction,
        InsectSwarmDmg,
        Intellect,
        InterruptProtection,
        LightningCapacitorProc,
        LightweaveEmbroideryProc,
        ThunderCapacitorProc,
        LotPCritRating,
        WarlockFelArmor,
        WarlockDemonArmor,
        WarlockGrandSpellstone,
        WarlockGrandFirestone,
        Mana,
        ManaRestore5min,
        ManaRestorePerCast,
        ManaRestoreOnCast_5_15,
        ManaRestoreOnCast_10_45,
        ManaRestoreFromBaseManaPerHit,
        ManaRestoreFromMaxManaPerSecond,
        ManaRestoreOnCrit_25_45,
        MangleCostReduction,
        RakeCostReduction,
        ShredCostReduction,
        Miss,
        MoonfireDmg,
        MoonfireExtension,
        BladeWardProc,
        Mp5,
        Mp5OnCastFor20SecOnUse2Min,
        NatureResistance,
        Parry,
        ParryRating,
        PendulumOfTelluricCurrentsProc,
        ExtractOfNecromanticPowerProc,
        DarkmoonCardDeathProc,
        PVPTrinket,
        RangedAttackPower,
        RangedHitRating,
        RangedCritRating,
        RangedHasteRating,
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
        SpellPowerFor10SecOnCrit_20_45,
        SpellPowerFor10SecOnHit_10_45,
        SpellPowerFor10SecOnHeal_10_45,
        SpellPowerFor10SecOnCast_10_45,
        SpellPowerFor10SecOnCast_15_45,
        SpellDamageFor10SecOnHit_5,
        SpellPowerFor10SecOnResist,
        SpellPowerFor15SecOnCrit_20_45,
        SpellPowerFor15SecOnCast_50_45,
        SpellPowerFor15SecOnUse90Sec,
        SpellPowerFor15SecOnUse2Min,
        SpellPowerFor20SecOnUse2Min,
        SpellPowerFor20SecOnUse5Min,
        SpellDamageFromIntellectPercentage,
        SpellDamageFromSpiritPercentage,
        SpellPowerFromAttackPowerPercentage,
        SpellDamageRating,
        SpellFireDamageRating,
        SpellFrostDamageRating,
        HasteRatingFor20SecOnUse2Min,
        HasteRatingFor20SecOnUse5Min,
        SpellHasteFor5SecOnCrit_50,
        SpellHasteFor6SecOnCast_15_45,
        SpellHasteFor10SecOnCast_10_45,
        SpellHasteFor10SecOnHeal_10_45,
        SpellHasteFor6SecOnHit_10_45,
        SpellHasteRating,
        SpellHit,
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
        BonusHealingReceived,
        UnseenMoonDamageBonus,
        WeaponDamage,
        WindfuryAPBonus,
        WrathDmg,
        DruidAshtongueTrinket,
        BonusPetCritChance,
        BonusWarlockSchoolDamageOnCast,
        BonusWarlockDotExtension,
        RegrowthExtraTicks,
        LifebloomFinalHealBonus,
        BonusHealingTouchMultiplier,
        TreeOfLifeAura,
        ReduceRejuvenationCost,
        ReduceRegrowthCost,
        ReduceHealingTouchCost,
        RejuvenationHealBonus,
        RejuvenationSpellpower,
        LifebloomTickHealBonus,
        HealingTouchFinalHealBonus,
        ManaregenFor8SecOnUse5Min,
        SpiritFor20SecOnUse2Min,
        ManaregenOver12SecOnUse3Min,
        ManaregenOver12SecOnUse5Min,
        ManacostReduceWithin15OnHealingCast,
        BangleProc,
        FullManaRegenFor15SecOnSpellcast,
        SpellPower,
        BonusRageOnCrit,
        BonusCPOnCrit,
        BonusEnergyOnTigersFury,
        FinisherEnergyOnAvoid,
        MangleCooldownReduction,
        BonusFerociousBiteCrit,
        BonusObliterateDamage,
        BonusScourgeStrikeDamage,
        BonusInsectSwarmDamage,
        BonusNukeCritChance,
        BonusHoTOnDirectHeals,
        TigersFuryCooldownReduction,
        #region Added by Rawr.HolyPriest
        PriestInnerFire,
        RenewDurationIncrease,
        ManaGainOnGreaterHealOverheal,
        PrayerOfMendingExtraJumps,
        GreaterHealCostReduction,
        WeakenedSoulDurationDecrease,
        PrayerOfHealingExtraCrit,
        PWSBonusSpellPowerProc,
        #endregion
        #region Added by Rawr.ShadowPriest
        SWPDurationIncrease,
        MindBlastCostReduction,
        ShadowWordDeathCritIncrease,
        DevouringPlagueBonusDamage,
        MindBlastHasteProc,
        #endregion
        #region Added by Rawr.Mage
        AldorRegaliaInterruptProtection,
        ArcaneBlastBonus,
        BonusManaGem,
        EvocationExtension,
        MageAllResist,
        MageIceArmor,
        MageMageArmor,
        MageMoltenArmor,
        Mage4T8,
        #endregion
        #region Added by Rawr.Tree
        LifebloomCostReduction,
        NourishBonusPerHoT,
        NourishSpellpower,
        RejuvenationInstantTick,
        #endregion
        #region Added by Rawr.Enhance
        TotemLLAttackPower,
        TotemShockSpellPower,
        TotemShockAttackPower,
        TotemSSHaste,
        TotemSSDamage,
        TotemWFAttackPower,
        BonusFlurryHaste,
        BonusLSDamage,
        BonusLLSSDamage,
        BonusMWFreq,
        #endregion
        #region Added by Rawr.Elemental
        BonusCritChance,
        BonusThunderCritChance,
        BonusShamanHit,
        ManaRegenIntPer5,
        ShamanCastTimeReduction,
        LightningOverloadProc,
        BonusLavaBurstCritDamage,
        ChainLightningCooldownReduction,
        BonusFlameShockDoTDamage,
        BonusFlametongueDamage,
        ShockManaCostReduction,
        LightningBoltDamageModifier,
        LightningBoltCostReduction,
        LightningSpellPower,
        LightningBoltHasteProc_15_45,
        LavaBurstBonus,
        #endregion
        #region Added by Rawr.Restosham
        ManaSpringMp5Increase,
        WaterShieldIncrease,
        CHHWHealIncrease,
        CHManaReduction,
        CHHealIncrease,
        LHWManaReduction,
        RTCDDecrease,
        CHCTDecrease,
        Earthliving,
        #endregion
        #region Rawr.Healadin
        FlashOfLightSpellPower,
        FlashOfLightMultiplier,
        FlashOfLightCrit,
        HolyLightManaReduction,
        HolyLightSpellPower,
        HolyLightPercentManaReduction,
        HolyLightCrit,
        SacredShieldICDReduction,
        HolyShockHoTOnCrit,
        HolyShockCrit,
        Heal1Min,
        Healed,
        ManaRestore,
        SpellsManaReduction,        // Seems this applies before talents, so different from ManaRestore with 100% proc on SpellCast
        #endregion
        #region Rawr.Retribution
        DivineStormMultiplier,
        CrusaderStrikeMultiplier,
        ExorcismMultiplier,
        HammerOfWrathMultiplier,
        DivineStormCrit,
        CrusaderStrikeCrit,
        DivineStormDamage,
        CrusaderStrikeDamage,
        ConsecrationSpellPower,
        JudgementCDReduction,
        #endregion
        #region Added by Rawr.ProtPaladin
        DivineProtectionDurationBonus,
        JudgementBlockValue,
        ShieldOfRighteousnessBlockValue,
        #endregion
        #region Warlock set bonuses
        LifeTapBonusSpirit,
        #endregion
        #region Added by Rawr.Moonkin
        StarfireProc,
        EclipseBonus,
        #endregion
        #region Warrior set bonuses
        DevastateCritIncrease,
        DreadnaughtBonusRageProc,
        #endregion
        #region Rogue set bonuses
        BonusSnDDuration,
        BonusSnDHaste,
        BonusCPGDamage,
        BonusEvisEnvenomDamage,
        BonusFreeFinisher,
        CPOnFinisher,
        RogueComboMoveEnergyReduction,
        RogueRuptureDamageBonus,
        #endregion
        #region DK Sigil Bonuses
        BonusBloodStrikeDamage,
        BonusDeathCoilDamage,
        BonusDeathStrikeDamage,
        BonusFrostStrikeDamage,
        BonusHeartStrikeDamage,
        BonusIcyTouchDamage,
        #endregion
        #region DK set bonuses
        BonusAntiMagicShellDamageReduction,
        BonusDeathCoilCrit,
        BonusDeathStrikeCrit,
        BonusFrostStrikeCrit,
        BonusIceboundFortitudeDuration,
        BonusObliterateCrit,
        BonusPerDiseaseBloodStrikeDamage,
        BonusPerDiseaseHeartStrikeDamage,
        BonusPerDiseaseObliterateDamage,
        BonusPerDiseaseScourgeStrikeDamage,
        BonusPlagueStrikeCrit,
        BonusRPFromDeathStrike,
        BonusRPFromObliterate,
        BonusRPFromScourgeStrike,
        BonusRuneStrikeMultiplier,
        BonusScourgeStrikeCrit,
        #endregion
        #region Runeforges
        BonusFrostWeaponDamage,
        CinderglacierProc,
        #endregion
        ArcaneDamage,
        FireDamage,
        ShadowDamage,

        NUM_AdditiveStat // This should always be the last entry.
    }

    enum MultiplicativeStat : int
    {
        BonusMangleBearThreat,
        BonusAgilityMultiplier,
        BonusArcaneDamageMultiplier,
        BaseArmorMultiplier,
        BonusArmorMultiplier,
        BonusBleedDamageMultiplier,
        BonusBlockValueMultiplier,
        BonusAttackPowerMultiplier,
        BonusCritMultiplier,
        BonusFireDamageMultiplier,
        BonusFrostDamageMultiplier,
        BonusIntellectMultiplier,
        BonusMageNukeMultiplier,
        BonusWarlockNukeMultiplier,
        BonusNatureDamageMultiplier,
        BonusPetDamageMultiplier,
        BonusPhysicalDamageMultiplier,
        BonusDamageMultiplier,
        BonusRipDamageMultiplier,
        BonusSpellCritMultiplier,
        BonusSpellPowerMultiplier,
        BonusSpellPowerDemonicPactMultiplier,
        BonusSpiritMultiplier,
        BonusHealthMultiplier,
        BonusManaMultiplier,
        BonusCritHealMultiplier,
        BonusStaminaMultiplier,
        BonusStrengthMultiplier,
        BonusSwipeDamageMultiplier,
        BonusMangleDamageMultiplier,
        BonusShredDamageMultiplier,
        BonusRakeDamageMultiplier,
        BonusFerociousBiteDamageMultiplier,
        BonusMaulDamageMultiplier,
        BonusEnrageDamageMultiplier,
        BonusShadowDamageMultiplier,
        BonusHolyDamageMultiplier,
        BonusDiseaseDamageMultiplier,
        ThreatIncreaseMultiplier,
        BonusWarlockDotDamageMultiplier,
        BonusRangedAttackPowerMultiplier,
        BonusSteadyShotDamageMultiplier,
        BonusManaregenWhileCastingMultiplier,
        PhysicalHaste,
        RangedHaste,
        SpellHaste,
        HealingReceivedMultiplier,
        DamageTakenMultiplier,
        #region Added by Rawr.HolyPriest
        BonusPoHManaCostReductionMultiplier,
        BonusGHHealingMultiplier,
        #endregion
        #region Added by Rawr.ShadowPriest
        BonusMindBlastMultiplier,
        #endregion
        #region Added by Rawr.Elemental
        BonusLavaBurstDamage,
        #endregion
        #region Added by Rawr.ProtPaladin
        BonusSealOfCorruptionDamageMultiplier,
        BonusSealOfRighteousnessDamageMultiplier,
        BonusSealOfVengeanceDamageMultiplier,
        HammerOfTheRighteousMultiplier,
        #endregion
        #region Warlock set bonuses
        CorruptionTriggersCrit,
        Warlock2T8,
        Warlock4T8,
        #endregion
        #region Warrior set bonuses
        BonusShieldSlamDamage,
        BonusSlamDamage,
        #endregion
        #region Boss Stats
        BossAttackSpeedMultiplier,
        #endregion

        NUM_MultiplicativeStat // This should always be the last entry.
    }

    enum InverseMultiplicativeStat : int
    {
        ArmorPenetration,
        ThreatReductionMultiplier,

        NUM_InverseMultiplicativeStat // This should always be the last entry.
    }

    enum NonStackingStat : int
    {
        BonusManaPotion,
        ArcaneResistanceBuff,
        FireResistanceBuff,
        FrostResistanceBuff,
        NatureResistanceBuff,
        ShadowResistanceBuff,
        MovementSpeed,
        GreatnessProc,
        HighestStat,
        ManacostReduceWithin15OnUse1Min,
        ShieldFromHealed,

        NUM_NonStackingStat // This should always be the last entry.
    }

    // Pulling this from ProtWar/ProtPally so that it can be used in other common areas.
    public enum HitResult
    {
        AnyMiss,
        AnyHit,
        Miss,
        Dodge,
        Parry,
        Block,
        Glance,
        Resist,
        Crit,
        Hit,

        NUM_HitResult, // Always the last entry in the enum.
    }

    public partial class Stats
    {

        /// <summary>
        /// The properties for each stat. In order to add additional stats for Rawr to track,
        /// first add properties here, for each stat. Apply a Category attribute to assign it to
        /// a category in the item editor. Optionally, apply a DisplayName attribute. If no
        /// DisplayName attribute is applied, the property name will be used, with spaces between
        /// each word, detected by capitalization (AttackPower becomes "Attack Power"). If the
        /// stat is a multiplier, add the Multiplicative attribute.
        /// </summary>
        #region Stat Properties

        #region Base Stats
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
        [Category("Base Stats")]
        [CommonStat]
        public float Health
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Health]; }
            set { _rawAdditiveData[(int)AdditiveStat.Health] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        public float Mana
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mana]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mana] = value; }
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
        [Category("Misc")]
        public float BaseAgility
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BaseAgility]; }
            set { _rawAdditiveData[(int)AdditiveStat.BaseAgility] = value; }
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
        [CommonStat(MinRange = 20f)]
        public float AttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.AttackPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [CommonStat(MinRange = 20f)]
        public float Strength
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Strength]; }
            set { _rawAdditiveData[(int)AdditiveStat.Strength] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float WeaponDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WeaponDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.WeaponDamage] = value; }
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
        [Percentage]
        [Category("Combat Values")]
        [DisplayName("% Armor Penetration")]
        public float ArmorPenetration
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ArmorPenetration]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ArmorPenetration] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Armor Penetration Rating")]
        [CommonStat]
        public float ArmorPenetrationRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArmorPenetrationRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArmorPenetrationRating] = value; }
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
        [CommonStat]
        public float Spirit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Spirit]; }
            set { _rawAdditiveData[(int)AdditiveStat.Spirit] = value; }
        }
        #endregion

        #region Resistances
        [DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Frost Res")]
        public float FrostResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FrostResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.FrostResistance] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Nature Res")]
        public float NatureResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.NatureResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.NatureResistance] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Fire Res")]
        public float FireResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FireResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.FireResistance] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Shadow Res")]
        public float ShadowResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShadowResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShadowResistance] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Arcane Res")]
        public float ArcaneResistance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArcaneResistance]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArcaneResistance] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Resistances")]
        [DisplayName("Resist")]
        public float AllResist
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AllResist]; }
            set { _rawAdditiveData[(int)AdditiveStat.AllResist] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Resist")]
        public float MageAllResist
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageAllResist]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageAllResist] = value; }
        }
        #endregion

        #region Buffs / Debuffs

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
        [DisplayName("% Arcane Damage")]
        public float BonusArcaneDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusArcaneDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusArcaneDamageMultiplier] = value; }
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
        [DisplayName("% Frost Damage")]
        public float BonusFrostDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostDamageMultiplier] = value; }
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

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Spirit")]
        public float BonusSpiritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpiritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpiritMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Crit Dmg")]
        public float BonusCritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Spell Crit Dmg")]
        public float BonusSpellCritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellCritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellCritMultiplier] = value; }
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
        [DisplayName("% Dmg")]
        public float BonusDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplier] = value; }
        }
        #endregion

        #region Combat Values
        [Percentage]
        [DefaultValueAttribute(0f)]
        [DisplayName("% Spell Crit")]
        [Category("Combat Values")]
        public float SpellCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCrit] = value; }
        }
        #endregion

        #region Deprecated
        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        [DisplayName("Spell Crit Rating")]
        [CommonStat]
        public float SpellCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCritRating] = value; }
        }
        #endregion

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Spell Power")]
        [CommonStat]
        public float SpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPower] = value; }
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
        [DisplayName("Spell Arcane Damage")]
        [CommonStat]
        public float SpellArcaneDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellArcaneDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellArcaneDamageRating] = value; }
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
        [Category("Base Stats")]
        [DisplayName("Spell Penetration")]
        public float SpellPenetration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPenetration]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPenetration] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        [DisplayName("Spell Hit Rating")]
        [CommonStat]
        public float SpellHitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHitRating] = value; }
        }

        [Percentage]
        [DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        [DisplayName("% Spell Hit")]
        public float SpellHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        [DisplayName("Spell Haste Rating")]
        [CommonStat]
        public float SpellHasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Combat Values")]
        [DisplayName("% Spell Haste")]
        public float SpellHaste
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.SpellHaste]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.SpellHaste] = value; }
        }

        // percentage mana generation while casting
        [DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        [DisplayName("Combat Mana Regeneration")]
        public float SpellCombatManaRegeneration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCombatManaRegeneration]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCombatManaRegeneration] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Physical Crit")]
        [Category("Combat Values")]
        public float PhysicalCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PhysicalCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.PhysicalCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Crit Rating")]
        [CommonStat]
        public float CritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Hunter")]
        [DisplayName("Ranged Crit Rating")]
        [CommonStat]
        public float RangedCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RangedCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.RangedCritRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        [DisplayName("Melee Crit")]
        [CommonStat]
        public float CritMeleeRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritMeleeRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritMeleeRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Equipment Effects")]
        [DisplayName("% Crit Bonus Damage")]
        public float CritBonusDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritBonusDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritBonusDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Hunter")]
        [DisplayName("Ranged Hit Rating")]
        [CommonStat]
        public float RangedHitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RangedHitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.RangedHitRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Hit Rating")]
        [CommonStat]
        public float HitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.HitRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Hit")]
        [Category("Combat Values")]
        public float PhysicalHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PhysicalHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.PhysicalHit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Dodge Rating")]
        [CommonStat]
        public float DodgeRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DodgeRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.DodgeRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Dodge")]
        [Category("Combat Values")]
        public float Dodge
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Dodge]; }
            set { _rawAdditiveData[(int)AdditiveStat.Dodge] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Combat Values")]
        public float Parry
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Parry]; }
            set { _rawAdditiveData[(int)AdditiveStat.Parry] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Parry Rating")]
        [CommonStat]
        public float ParryRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ParryRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ParryRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        [Percentage]
        [DisplayName("% Block")]
        public float Block
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Block]; }
            set { _rawAdditiveData[(int)AdditiveStat.Block] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Block Rating")]
        [CommonStat]
        public float BlockRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BlockRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.BlockRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Block Value")]
        [CommonStat]
        public float BlockValue
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BlockValue]; }
            set { _rawAdditiveData[(int)AdditiveStat.BlockValue] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        public float Defense
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Defense]; }
            set { _rawAdditiveData[(int)AdditiveStat.Defense] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Defense Rating")]
        [CommonStat]
        public float DefenseRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DefenseRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.DefenseRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Resilience")]
        [CommonStat]
        public float Resilience
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Resilience]; }
            set { _rawAdditiveData[(int)AdditiveStat.Resilience] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        public float Expertise
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Expertise]; }
            set { _rawAdditiveData[(int)AdditiveStat.Expertise] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Expertise Rating")]
        [CommonStat]
        public float ExpertiseRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExpertiseRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExpertiseRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Haste Rating")]
        [CommonStat]
        public float HasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Hunter")]
        [DisplayName("Ranged Haste Rating")]
        [CommonStat]
        public float RangedHasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RangedHasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.RangedHasteRating] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Old Equipment Procs")]
        public float HasteRatingOnPhysicalAttack
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRatingOnPhysicalAttack]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRatingOnPhysicalAttack] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Combat Values")]
        [DisplayName("% Physical Haste")]
        public float PhysicalHaste
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.PhysicalHaste]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.PhysicalHaste] = value; }
        }


        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Combat Values")]
        [DisplayName("% Ranged Haste")]
        public float RangedHaste
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.RangedHaste]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.RangedHaste] = value; }
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
        [Category("Base Stats")]
        [DisplayName("Mana per 5 sec")]
        [CommonStat]
        public float Mp5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mp5]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mp5] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [DisplayName("Health per 5 sec")]
        [CommonStat]
        public float Hp5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Hp5]; }
            set { _rawAdditiveData[(int)AdditiveStat.Hp5] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float BloodlustProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BloodlustProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.BloodlustProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float TerrorProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TerrorProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.TerrorProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Combat Values")]
        [DisplayName("% Miss")]
        public float Miss
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Miss]; }
            set { _rawAdditiveData[(int)AdditiveStat.Miss] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Mage")]
        public float AldorRegaliaInterruptProtection
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AldorRegaliaInterruptProtection]; }
            set { _rawAdditiveData[(int)AdditiveStat.AldorRegaliaInterruptProtection] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float InterruptProtection
        {
            get { return _rawAdditiveData[(int)AdditiveStat.InterruptProtection]; }
            set { _rawAdditiveData[(int)AdditiveStat.InterruptProtection] = value; }
        }

        #region Rogue bonuses
        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float BonusSnDDuration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusSnDDuration]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusSnDDuration] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float CPOnFinisher
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CPOnFinisher]; }
            set { _rawAdditiveData[(int)AdditiveStat.CPOnFinisher] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float BonusEvisEnvenomDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusEvisEnvenomDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusEvisEnvenomDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float BonusFreeFinisher
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFreeFinisher]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFreeFinisher] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float BonusSnDHaste
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusSnDHaste]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusSnDHaste] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float BonusCPGDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCPGDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCPGDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float RogueT7TwoPieceBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RogueRuptureDamageBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.RogueRuptureDamageBonus] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float RogueT7FourPieceBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RogueComboMoveEnergyReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.RogueComboMoveEnergyReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float RogueT8TwoPieceBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RogueRuptureDamageBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.RogueRuptureDamageBonus] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Rogue")]
        public float RogueT8FourPieceBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RogueComboMoveEnergyReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.RogueComboMoveEnergyReduction] = value; }
        }
        #endregion

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusObliterateDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusObliterateDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusObliterateDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusScourgeStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusScourgeStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusScourgeStrikeDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusBloodStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusBloodStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusBloodStrikeDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusHeartStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusHeartStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusHeartStrikeDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusDeathCoilDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDeathCoilDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDeathCoilDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusFrostStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFrostStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFrostStrikeDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% White Damage as Frost Damage")]
        public float BonusFrostWeaponDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFrostWeaponDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFrostWeaponDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        [DisplayName("Cinderglacier Proc")]
        public float CinderglacierProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CinderglacierProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.CinderglacierProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusDeathStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDeathStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDeathStrikeDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusIcyTouchDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusIcyTouchDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusIcyTouchDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusAntiMagicShellDamageReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusAntiMagicShellDamageReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusAntiMagicShellDamageReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Death Coil Crit")]
        public float BonusDeathCoilCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDeathCoilCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDeathCoilCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Death Strike Crit")]
        public float BonusDeathStrikeCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusDeathStrikeCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusDeathStrikeCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Frost Strike Crit")]
        public float BonusFrostStrikeCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFrostStrikeCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFrostStrikeCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        public float BonusIceboundFortitudeDuration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusIceboundFortitudeDuration]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusIceboundFortitudeDuration] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Obliterate Crit")]
        public float BonusObliterateCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusObliterateCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusObliterateCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Damage Per Disease For Blood Strike")]
        public float BonusPerDiseaseBloodStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusPerDiseaseBloodStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusPerDiseaseBloodStrikeDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Damage Per Disease For Heart Strike")]
        public float BonusPerDiseaseHeartStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusPerDiseaseHeartStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusPerDiseaseHeartStrikeDamage] = value; }
        }


        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Damage Per Disease For Obliterate")]
        public float BonusPerDiseaseObliterateDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusPerDiseaseObliterateDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusPerDiseaseObliterateDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Damage Per Disease For Scourge Strike")]
        public float BonusPerDiseaseScourgeStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusPerDiseaseScourgeStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusPerDiseaseScourgeStrikeDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Plague Strike Crit")]
        public float BonusPlagueStrikeCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusPlagueStrikeCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusPlagueStrikeCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        [DisplayName("Bonus RP From Death Strike")]
        public float BonusRPFromDeathStrike
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRPFromDeathStrike]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRPFromDeathStrike] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        [DisplayName("Bonus RP From Obliterate")]
        public float BonusRPFromObliterate
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRPFromObliterate]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRPFromObliterate] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Death Knight")]
        [DisplayName("Bonus RP From Scourge Strike")]
        public float BonusRPFromScourgeStrike
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRPFromScourgeStrike]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRPFromScourgeStrike] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Rune Strike Damage Multiplier")]
        public float BonusRuneStrikeMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRuneStrikeMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRuneStrikeMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Death Knight")]
        [DisplayName("% Bonus Scourge Strike Crit")]
        public float BonusScourgeStrikeCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusScourgeStrikeCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusScourgeStrikeCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Warrior")]
        [DisplayName("Bonus Commanding Shout HP")]
        public float BonusCommandingShoutHP
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCommandingShoutHP]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCommandingShoutHP] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float BonusShredDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusShredDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusShredDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float BonusLacerateDamageMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusLacerateDamageMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusLacerateDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float BonusMangleCatDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusMangleCatDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusMangleCatDamage] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float BonusMangleBearDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusMangleBearDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusMangleBearDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Rip Damage Per Combo Point Per Tick")]
        [Category("Feral")]
        public float BonusRipDamagePerCPPerTick
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRipDamagePerCPPerTick]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRipDamagePerCPPerTick] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Rip Duration")]
        [Category("Feral")]
        public float BonusRipDuration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRipDuration]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRipDuration] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Savage Roar Duration")]
        [Category("Feral")]
        public float BonusSavageRoarDuration
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusSavageRoarDuration]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusSavageRoarDuration] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("Clearcast On Bleed Chance")]
        [Category("Feral")]
        public float ClearcastOnBleedChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ClearcastOnBleedChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.ClearcastOnBleedChance] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Windfury")]
        [Category("Deprecated")]
        public float WindfuryAPBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WindfuryAPBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.WindfuryAPBonus] = value; }
        }

        [Percentage]
        [DisplayName("% Base Mana / Hit")]
        [Category("Buffs / Debuffs")]
        [DefaultValueAttribute(0f)]
        public float ManaRestoreFromBaseManaPerHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromBaseManaPerHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromBaseManaPerHit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Old Equipment Procs")]
        public float ManaRestorePerCast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCast]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCast] = value; }
        }

        // 5% proc rate, 15 sec internal cooldown
        [DisplayName("Mana Restore On Cast (5%)")]
        [Category("Old Equipment Procs")]
        [DefaultValueAttribute(0f)]
        public float ManaRestoreOnCast_5_15
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCast_5_15]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCast_5_15] = value; }
        }

        // 10% proc rate, 45 sec internal cooldown
        [DisplayName("Mana Restore On Cast (10%)")]
        [Category("Old Equipment Procs")]
        [DefaultValueAttribute(0f)]
        public float ManaRestoreOnCast_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCast_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCast_10_45] = value; }
        }

        [Percentage]
        [DisplayName("% Max Mana / Sec")]
        [Category("Buffs / Debuffs")]
        [DefaultValueAttribute(0f)]
        public float ManaRestoreFromMaxManaPerSecond
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromMaxManaPerSecond]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromMaxManaPerSecond] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float MangleCatCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MangleCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.MangleCostReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float RakeCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RakeCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.RakeCostReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        public float ShredCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShredCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShredCostReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        public float ExposeWeakness
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExposeWeakness]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExposeWeakness] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float Bloodlust
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Bloodlust]; }
            set { _rawAdditiveData[(int)AdditiveStat.Bloodlust] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        public float DrumsOfWar
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DrumsOfWar]; }
            set { _rawAdditiveData[(int)AdditiveStat.DrumsOfWar] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        public float DrumsOfBattle
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DrumsOfBattle]; }
            set { _rawAdditiveData[(int)AdditiveStat.DrumsOfBattle] = value; }
        }

        [Percentage]
        [DisplayName("% Arcane Blast Bonus")]
        [Category("Mage")]
        [DefaultValueAttribute(0f)]
        public float ArcaneBlastBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArcaneBlastBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArcaneBlastBonus] = value; }
        }

        [DisplayName("Kirin Tor Garb 4 Piece Bonus")]
        [Category("Mage")]
        [DefaultValueAttribute(0f)]
        public float Mage4T8
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mage4T8]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mage4T8] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float SpellDamageFromIntellectPercentage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFromIntellectPercentage]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFromIntellectPercentage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float SpellDamageFromSpiritPercentage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFromSpiritPercentage]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFromSpiritPercentage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float SpellPowerFromAttackPowerPercentage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFromAttackPowerPercentage]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFromAttackPowerPercentage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (50% 5 sec/Crit)")]
        [Category("Old Equipment Procs")]
        public float SpellHasteFor5SecOnCrit_50
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor5SecOnCrit_50]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor5SecOnCrit_50] = value; }
        }

        // 15% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (15% 6 sec/Cast)")]
        [Category("Old Equipment Procs")]
        public float SpellHasteFor6SecOnCast_15_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnCast_15_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnCast_15_45] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (10% 10 sec/Cast)")]
        [Category("Old Equipment Procs")]
        public float SpellHasteFor10SecOnCast_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor10SecOnCast_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor10SecOnCast_10_45] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (10% 10 sec/Cast)")]
        [Category("Old Equipment Procs")]
        public float SpellHasteFor10SecOnHeal_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor10SecOnHeal_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor10SecOnHeal_10_45] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (10% 6 sec/Hit)")]
        [Category("Old Equipment Procs")]
        public float SpellHasteFor6SecOnHit_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnHit_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnHit_10_45] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec/Resist)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor10SecOnResist
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnResist]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnResist] = value; }
        }

        // trinket effect, does not sum up over gear, 2 trinkets with this effect is not equivalent to 1 trinket with double effect
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (20 sec/2 min)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor20SecOnUse2Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (20 sec/5 min)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor20SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor20SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor20SecOnUse5Min] = value; }
        }

        // not used by any item
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (15 sec/2 min)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor15SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnUse2Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mana Restore on Crit (25%, 45sec)")]
        [Category("Old Equipment Procs")]
        public float ManaRestoreOnCrit_25_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCrit_25_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCrit_25_45] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (15 sec/1.5 min)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor15SecOnUse90Sec
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnUse90Sec]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnUse90Sec] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (20 sec/2 min)")]
        [Category("Old Equipment Procs")]
        public float HasteRatingFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRatingFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRatingFor20SecOnUse2Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (20 sec/5 min)")]
        [Category("Old Equipment Procs")]
        public float HasteRatingFor20SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRatingFor20SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRatingFor20SecOnUse5Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mp5 on Cast (20 sec/2 min)")]
        [Category("Old Equipment Procs")]
        public float Mp5OnCastFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mp5OnCastFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mp5OnCastFor20SecOnUse2Min] = value; }
        }

        // 5% chance, no cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec)")]
        [Category("Old Equipment Procs")]
        public float SpellDamageFor10SecOnHit_5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnHit_5]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageFor10SecOnHit_5] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mana Gem Effect")]
        [Category("Mage")]
        public float BonusManaGem
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusManaGem]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusManaGem] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mana Restore")]
        [Category("Equipment Effects")]
        public float ManaRestore
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestore]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestore] = value; }
        }


        // 10% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor10SecOnHit_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnHit_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnHit_10_45] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor10SecOnHeal_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnHeal_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnHeal_10_45] = value; }
        }

        // 15% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor10SecOnCast_15_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCast_15_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCast_15_45] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor10SecOnCast_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCast_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCast_10_45] = value; }
        }

        // 20% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (15 sec)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor15SecOnCrit_20_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnCrit_20_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnCrit_20_45] = value; }
        }

        // 50% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (15 sec)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor15SecOnCast_50_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnCast_50_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnCast_50_45] = value; }
        }

        // 20% chance, 45 sec internal cooldown
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Old Equipment Procs")]
        public float SpellPowerFor10SecOnCrit_20_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCrit_20_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCrit_20_45] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Equipment Effects")]
        [DisplayName("PvP Trinket")]
        public float PVPTrinket
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PVPTrinket]; }
            set { _rawAdditiveData[(int)AdditiveStat.PVPTrinket] = value; }
        }

        // Starfire idol
        [DefaultValueAttribute(0f)]
        [DisplayName("Starfire damage bonus")]
        [Category("Moonkin")]
        public float StarfireDmg
        {
            get { return _rawAdditiveData[(int)AdditiveStat.StarfireDmg]; }
            set { _rawAdditiveData[(int)AdditiveStat.StarfireDmg] = value; }
        }

        // Tree 2-piece T5
        [DefaultValueAttribute(0f)]
        [Category("Tree")]
        public float RegrowthExtraTicks
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RegrowthExtraTicks]; }
            set { _rawAdditiveData[(int)AdditiveStat.RegrowthExtraTicks] = value; }
        }

        // Tree PvP Idols and 4-piece T5
        [DefaultValueAttribute(0f)]
        [Category("Tree")]
        [DisplayName("Lifebloom Final Heal")]
        public float LifebloomFinalHealBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LifebloomFinalHealBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.LifebloomFinalHealBonus] = value; }
        }

        // Tree 4-piece T6
        [DefaultValueAttribute(0f)]
        [Category("Tree")]
        public float BonusHealingTouchMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusHealingTouchMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusHealingTouchMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Tree of Life Aura")]
        [Category("Tree")]
        public float TreeOfLifeAura
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TreeOfLifeAura]; }
            set { _rawAdditiveData[(int)AdditiveStat.TreeOfLifeAura] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Reduced Rejuvenation Mana Cost")]
        [Category("Tree")]
        public float ReduceRejuvenationCost
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ReduceRejuvenationCost]; }
            set { _rawAdditiveData[(int)AdditiveStat.ReduceRejuvenationCost] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Reduced Regrowth Mana Cost")]
        [Category("Tree")]
        public float ReduceRegrowthCost
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ReduceRegrowthCost]; }
            set { _rawAdditiveData[(int)AdditiveStat.ReduceRegrowthCost] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Reduced Healing Touch Mana Cost")]
        [Category("Tree")]
        public float ReduceHealingTouchCost
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ReduceHealingTouchCost]; }
            set { _rawAdditiveData[(int)AdditiveStat.ReduceHealingTouchCost] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Healing by Rejuvenation")]
        [Category("Tree")]
        public float RejuvenationHealBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RejuvenationHealBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.RejuvenationHealBonus] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell power of Rejuvenation")]
        [Category("Tree")]
        public float RejuvenationSpellpower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RejuvenationSpellpower]; }
            set { _rawAdditiveData[(int)AdditiveStat.RejuvenationSpellpower] = value; }
        }

        // Tier 8 Set bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Instant Tick by Rejuvenation")]
        [Category("Tree")]
        public float RejuvenationInstantTick
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RejuvenationInstantTick]; }
            set { _rawAdditiveData[(int)AdditiveStat.RejuvenationInstantTick] = value; }
        }


        [DefaultValueAttribute(0f)]
        [DisplayName("Healing by Lifebloom Ticks")]
        [Category("Tree")]
        public float LifebloomTickHealBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LifebloomTickHealBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.LifebloomTickHealBonus] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Healing Touch Final Heal")]
        [Category("Tree")]
        public float HealingTouchFinalHealBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HealingTouchFinalHealBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.HealingTouchFinalHealBonus] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Mage")]
        public float EvocationExtension
        {
            get { return _rawAdditiveData[(int)AdditiveStat.EvocationExtension]; }
            set { _rawAdditiveData[(int)AdditiveStat.EvocationExtension] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Equipment Effects")]
        public float LightningCapacitorProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningCapacitorProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningCapacitorProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Old Equipment Procs")]
        public float LightweaveEmbroideryProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightweaveEmbroideryProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightweaveEmbroideryProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Equipment Effects")]
        public float ThunderCapacitorProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ThunderCapacitorProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ThunderCapacitorProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        [DisplayName("LotP Crit")]
        public float LotPCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LotPCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.LotPCritRating] = value; }
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

        [DefaultValueAttribute(0f)]
        [Category("Warlock")]
        public float WarlockFelArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WarlockFelArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.WarlockFelArmor] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Warlock")]
        public float WarlockDemonArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WarlockDemonArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.WarlockDemonArmor] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Warlock")]
        public float WarlockGrandSpellstone
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WarlockGrandSpellstone]; }
            set { _rawAdditiveData[(int)AdditiveStat.WarlockGrandSpellstone] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Warlock")]
        public float WarlockGrandFirestone
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WarlockGrandFirestone]; }
            set { _rawAdditiveData[(int)AdditiveStat.WarlockGrandFirestone] = value; }
        }

        // Unseen Moon idol
        [DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage (10 sec on Moonfire)")]
        [Category("Moonkin")]
        public float UnseenMoonDamageBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.UnseenMoonDamageBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.UnseenMoonDamageBonus] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        public float ShatteredSunMightProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShatteredSunMightProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShatteredSunMightProc] = value; }
        }

        [DefaultValue(0f)]
        [Category("Deprecated")]
        public float ShatteredSunRestoProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShatteredSunRestoProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShatteredSunRestoProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Deprecated")]
        public float CritChanceReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritChanceReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritChanceReduction] = value; }
        }


        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        [DisplayName("Strength in Cat Form")]
        public float CatFormStrength
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CatFormStrength]; }
            set { _rawAdditiveData[(int)AdditiveStat.CatFormStrength] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        [DisplayName("Time average Agility")]
        public float AverageAgility
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AverageAgility]; }
            set { _rawAdditiveData[(int)AdditiveStat.AverageAgility] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        [DisplayName("Average Armor")]
        public float AverageArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.AverageArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.AverageArmor] = value; }
        }

        [DisplayName("Bonus Healing Received")]
        [Category("Equipment Effects")]
        [DefaultValueAttribute(0f)]
        public float BonusHealingReceived
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusHealingReceived]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusHealingReceived] = value; }
        }

        [DisplayName("Shattered Sun Caster Neck proc")]
        [Category("Deprecated")]
        [DefaultValueAttribute(0f)]
        public float ShatteredSunAcumenProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShatteredSunAcumenProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShatteredSunAcumenProc] = value; }
        }

        [DisplayName("Pendulum of Telluric Currents proc")]
        [Category("Old Equipment Procs")]
        [DefaultValueAttribute(0f)]
        public float PendulumOfTelluricCurrentsProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PendulumOfTelluricCurrentsProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.PendulumOfTelluricCurrentsProc] = value; }
        }

        [DisplayName("Extract of Necromantic Power proc")]
        [Category("Equipment Effects")]
        [DefaultValueAttribute(0f)]
        public float ExtractOfNecromanticPowerProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExtractOfNecromanticPowerProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExtractOfNecromanticPowerProc] = value; }
        }

        [DisplayName("Darkmoon Card: Death proc")]
        [Category("Equipment Effects")]
        [DefaultValueAttribute(0f)]
        public float DarkmoonCardDeathProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DarkmoonCardDeathProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.DarkmoonCardDeathProc] = value; }
        }

        [DisplayName("Timbal's Focusing Crystal proc")]
        [Category("Equipment Effects")]
        [DefaultValueAttribute(0f)]
        public float TimbalsProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TimbalsProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.TimbalsProc] = value; }
        }

        [DisplayName("Spell damage (8 sec, 25% chance on Starfire)")]
        [Category("Moonkin")]
        [DefaultValueAttribute(0f)]
        public float DruidAshtongueTrinket
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DruidAshtongueTrinket]; }
            set { _rawAdditiveData[(int)AdditiveStat.DruidAshtongueTrinket] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mana every 5 min")]
        [Category("Old Equipment Procs")]
        public float ManaRestore5min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestore5min]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestore5min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Greatness Proc")]
        [Category("Old Equipment Procs")]
        public float GreatnessProc
        {
            get { return _rawNoStackData[(int)NonStackingStat.GreatnessProc]; }
            set { _rawNoStackData[(int)NonStackingStat.GreatnessProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("% Shield from Heal Amount")]
        [Percentage]
        [Category("Equipment Effects")]
        public float ShieldFromHealed
        {
            get { return _rawNoStackData[(int)NonStackingStat.ShieldFromHealed]; }
            set { _rawNoStackData[(int)NonStackingStat.ShieldFromHealed] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Highest Stat")]
        [Category("Equipment Effects")]
        public float HighestStat
        {
            get { return _rawNoStackData[(int)NonStackingStat.HighestStat]; }
            set { _rawNoStackData[(int)NonStackingStat.HighestStat] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Healed every 1 min")]
        [Category("Old Equipment Procs")]
        public float Heal1Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Heal1Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.Heal1Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Healed")]
        [Category("Equipment Effects")]
        public float Healed
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Healed]; }
            set { _rawAdditiveData[(int)AdditiveStat.Healed] = value; }
        }

        // Bandit's Insignia
        [DefaultValueAttribute(0f)]
        [DisplayName("Arcane Damage")]
        [Category("Equipment Effects")]
        public float ArcaneDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ArcaneDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.ArcaneDamage] = value; }
        }

        // Bandit's Insignia
        [DefaultValueAttribute(0f)]
        [DisplayName("Shadow Damage")]
        [Category("Equipment Effects")]
        public float ShadowDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShadowDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShadowDamage] = value; }
        }

        // Hand Mounted Pyro Rocket
        [DefaultValueAttribute(0f)]
        [DisplayName("Fire Damage")]
        [Category("Equipment Effects")]
        public float FireDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FireDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.FireDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Crusader Strike Damage")]
        [Category("Retribution")]
        public float CrusaderStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CrusaderStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.CrusaderStrikeDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Consecration Spell Power")]
        [Category("Retribution")]
        public float ConsecrationSpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ConsecrationSpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.ConsecrationSpellPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Divine Storm Damage")]
        [Category("Retribution")]
        public float DivineStormDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DivineStormDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.DivineStormDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Divine Storm Damage")]
        [Category("Retribution")]
        public float DivineStormMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DivineStormMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.DivineStormMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Crusader Strike Damage")]
        [Category("Retribution")]
        public float CrusaderStrikeMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CrusaderStrikeMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.CrusaderStrikeMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Exorcism Damage")]
        [Category("Retribution")]
        public float ExorcismMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExorcismMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExorcismMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Hammer of Wrath Damage")]
        [Category("Retribution")]
        public float HammerOfWrathMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HammerOfWrathMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.HammerOfWrathMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Crusader Strike Crit")]
        [Category("Retribution")]
        public float CrusaderStrikeCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CrusaderStrikeCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.CrusaderStrikeCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Divine Storm Crit")]
        [Category("Retribution")]
        public float DivineStormCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DivineStormCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.DivineStormCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("sec Judgement CD Reduction")]
        [Category("Retribution")]
        public float JudgementCDReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.JudgementCDReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.JudgementCDReduction] = value; }
        }


        [DefaultValueAttribute(0f)]
        [DisplayName("Flash of Light Spell Power")]
        [Category("Healadin")]
        public float FlashOfLightSpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FlashOfLightSpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.FlashOfLightSpellPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("secs off Sacred Shield ICD")]
        [Category("Healadin")]
        public float SacredShieldICDReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SacredShieldICDReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.SacredShieldICDReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("% HoT on Holy Shock Crit")]
        [Percentage]
        [Category("Healadin")]
        public float HolyShockHoTOnCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyShockHoTOnCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyShockHoTOnCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Flash of Light Healing")]
        [Category("Healadin")]
        public float FlashOfLightMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FlashOfLightMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.FlashOfLightMultiplier] = value; }
        }


        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Flash of Light Crit")]
        [Category("Healadin")]
        public float FlashOfLightCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FlashOfLightCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.FlashOfLightCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Holy Light Spell Power")]
        [Category("Healadin")]
        public float HolyLightSpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyLightSpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyLightSpellPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spells Mana Cost Reduction")]
        [Category("Equipment Effects")]
        public float SpellsManaReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellsManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellsManaReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Holy Light Mana Cost Reduction")]
        [Category("Healadin")]
        public float HolyLightManaCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyLightManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyLightManaReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Holy Light Crit")]
        [Category("Healadin")]
        public float HolyLightCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyLightCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyLightCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Holy Shock Crit")]
        [Category("Healadin")]
        public float HolyShockCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyShockCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyShockCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Holy Light Cost Reduction")]
        [Category("Healadin")]
        public float HolyLightPercentManaReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyLightPercentManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyLightPercentManaReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Spirit after Life Tap")]
        [Category("Warlock")]
        public float LifeTapBonusSpirit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LifeTapBonusSpirit]; }
            set { _rawAdditiveData[(int)AdditiveStat.LifeTapBonusSpirit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Warlock")]
        public float BonusWarlockSchoolDamageOnCast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusWarlockSchoolDamageOnCast]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusWarlockSchoolDamageOnCast] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Warlock")]
        public float BonusWarlockDotExtension
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusWarlockDotExtension]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusWarlockDotExtension] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Hunter")]
        public float RangedAttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RangedAttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.RangedAttackPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage] /*its not really multiplicative, but it is stored as .02 instead of 2 because crit is % based 0-1, so this attribute makes it display correctly */
        [DisplayName("% Extra Pet Crit Chance")]
        [Category("Hunter")]
        public float BonusPetCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusPetCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusPetCritChance] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage] /*Same as above*/
        [DisplayName("% Extra Steady Shot Crit")]
        [Category("Hunter")]
        public float BonusSteadyShotCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusStreadyShotCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusStreadyShotCrit] = value; }
        }

        /* Regen trinkets */
        [DefaultValueAttribute(0f)]
        [DisplayName("Mana each sec. for 8 sec. (5 min cd)")]
        [Category("Old Equipment Procs")]
        public float ManaregenFor8SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaregenFor8SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaregenFor8SecOnUse5Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spirit for 20 sec. (2 min cd)")]
        [Category("Old Equipment Procs")]
        public float SpiritFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpiritFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpiritFor20SecOnUse2Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Spirit while casting")]
        [Category("Equipment Effects")]
        public float ExtraSpiritWhileCasting
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExtraSpiritWhileCasting]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExtraSpiritWhileCasting] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mana restores over 12 sec. (2 min cd)")]
        [Category("Old Equipment Procs")]
        public float ManaregenOver12SecOnUse3Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaregenOver12SecOnUse3Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaregenOver12SecOnUse3Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mana restores over 12 sec. (5 min cd)")]
        [Category("Old Equipment Procs")]
        public float ManaregenOver12SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaregenOver12SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaregenOver12SecOnUse5Min] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mana cost of next spell reduce (within 15 sec.)")]
        [Category("Old Equipment Procs")]
        public float ManacostReduceWithin15OnHealingCast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManacostReduceWithin15OnHealingCast]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManacostReduceWithin15OnHealingCast] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Bangle Proc")]
        [Category("Old Equipment Procs")]
        public float BangleProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BangleProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.BangleProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("% chance on spellcast to allow 100% mana regen for 15 sec")]
        [Category("Old Equipment Procs")]
        public float FullManaRegenFor15SecOnSpellcast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FullManaRegenFor15SecOnSpellcast]; }
            set { _rawAdditiveData[(int)AdditiveStat.FullManaRegenFor15SecOnSpellcast] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Extra heal over time from direct healing spells")]
        [Category("Old Equipment Procs")]
        public float BonusHoTOnDirectHeals
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusHoTOnDirectHeals]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusHoTOnDirectHeals] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Lifebloom cost reduction")]
        [Category("Tree")]
        public float LifebloomCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LifebloomCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.LifebloomCostReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Nourish bonus per HoT")]
        [Category("Tree")]
        public float NourishBonusPerHoT
        {
            get { return _rawAdditiveData[(int)AdditiveStat.NourishBonusPerHoT]; }
            set { _rawAdditiveData[(int)AdditiveStat.NourishBonusPerHoT] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Nourish spell power")]
        [Category("Tree")]
        public float NourishSpellpower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.NourishSpellpower]; }
            set { _rawAdditiveData[(int)AdditiveStat.NourishSpellpower] = value; }
        }


        #region Added by Rawr.Enhance

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float TotemLLAttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TotemLLAttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.TotemLLAttackPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float TotemShockSpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TotemShockSpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.TotemShockSpellPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float TotemShockAttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TotemShockAttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.TotemShockAttackPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float TotemSSHaste
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TotemSSHaste]; }
            set { _rawAdditiveData[(int)AdditiveStat.TotemSSHaste] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float TotemSSDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TotemSSDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.TotemSSDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float TotemWFAttackPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TotemWFAttackPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.TotemWFAttackPower] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float BonusFlurryHaste
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFlurryHaste]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFlurryHaste] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float BonusLSDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusLSDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusLSDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float BonusLLSSDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusLLSSDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusLLSSDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Enhance")]
        public float BonusMWFreq
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusMWFreq]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusMWFreq] = value; }
        }

        #endregion
        #region Added by Rawr.Elemental
        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float BonusCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCritChance] = value; }
        }


        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float BonusThunderCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusThunderCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusThunderCritChance] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float BonusShamanHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusShamanHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusShamanHit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float ManaRegenIntPer5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRegenIntPer5]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRegenIntPer5] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float ShamanCastTimeReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShamanCastTimeReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShamanCastTimeReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float LightningOverloadProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningOverloadProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningOverloadProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("% bonus critical strike damage for Lava Burst")]
        [Category("Elemental")]
        public float BonusLavaBurstCritDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusLavaBurstCritDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusLavaBurstCritDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float ChainLightningCooldownReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ChainLightningCooldownReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.ChainLightningCooldownReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float BonusFlameShockDoTDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFlameShockDoTDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFlameShockDoTDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float BonusFlametongueDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFlametongueDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFlametongueDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float ShockManaCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShockManaCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShockManaCostReduction] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float LightningBoltCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningBoltCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningBoltCostReduction] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float LightningBoltDamageModifier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningBoltDamageModifier]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningBoltDamageModifier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float LightningSpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningSpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningSpellPower] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float LightningBoltHasteProc_15_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningBoltHasteProc_15_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningBoltHasteProc_15_45] = value; }
        }
        [DefaultValueAttribute(0f)]
        [Category("Elemental")]
        public float LavaBurstBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LavaBurstBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.LavaBurstBonus] = value; }
        }

        #endregion
        #region Added by Rawr.RestoSham
        [DefaultValueAttribute(0f)]
        [DisplayName("Mp5 increase for Mana Spring")]
        [Category("RestoSham")]
        public float ManaSpringMp5Increase
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaSpringMp5Increase]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaSpringMp5Increase] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("second off Riptide Cooldown")]
        [Category("RestoSham")]
        public float RTCDDecrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RTCDDecrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.RTCDDecrease] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("tenths of a second cast time off Chain Heal")]
        [Category("RestoSham")]
        public float CHCTDecrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CHCTDecrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.CHCTDecrease] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Instances of Earthliving Weapon active")]
        [Category("RestoSham")]
        public float Earthliving
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Earthliving]; }
            set { _rawAdditiveData[(int)AdditiveStat.Earthliving] = value; }
        }
        #endregion
        #region Added by Rawr.ProtPaladin

        [DefaultValueAttribute(0f)]
        [DisplayName("Judgement Block Value")]
        [Category("ProtPaladin")]
        public float JudgementBlockValue
        {
            get { return _rawAdditiveData[(int)AdditiveStat.JudgementBlockValue]; }
            set { _rawAdditiveData[(int)AdditiveStat.JudgementBlockValue] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("ShoR Block Value")]
        [Category("ProtPaladin")]
        public float ShieldOfRighteousnessBlockValue
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShieldOfRighteousnessBlockValue]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShieldOfRighteousnessBlockValue] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Divine Protection Duration Bonus")]
        [Category("ProtPaladin")]
        public float DivineProtectionDurationBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DivineProtectionDurationBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.DivineProtectionDurationBonus] = value; }
        }

        #endregion
        #region Added by Rawr.Moonkin

        // Insect Swarm idol
        [DefaultValueAttribute(0f)]
        [DisplayName("Insect Swarm damage bonus")]
        [Category("Moonkin")]
        public float InsectSwarmDmg
        {
            get { return _rawAdditiveData[(int)AdditiveStat.InsectSwarmDmg]; }
            set { _rawAdditiveData[(int)AdditiveStat.InsectSwarmDmg] = value; }
        }

        // Moonfire idol
        [DefaultValueAttribute(0f)]
        [DisplayName("Moonfire damage bonus")]
        [Category("Moonkin")]
        public float MoonfireDmg
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MoonfireDmg]; }
            set { _rawAdditiveData[(int)AdditiveStat.MoonfireDmg] = value; }
        }

        // Wrath idol
        [DefaultValueAttribute(0f)]
        [DisplayName("Wrath damage bonus")]
        [Category("Moonkin")]
        public float WrathDmg
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WrathDmg]; }
            set { _rawAdditiveData[(int)AdditiveStat.WrathDmg] = value; }
        }

        // Moonkin Aura idol
        [DefaultValueAttribute(0f)]
        [DisplayName("Moonkin Aura bonus")]
        [Category("Moonkin")]
        public float IdolCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.IdolCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.IdolCritRating] = value; }
        }

        // Moonkin 4-piece T4 bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("Innervate CD Reduction")]
        [Category("Moonkin")]
        public float InnervateCooldownReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.InnervateCooldownReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.InnervateCooldownReduction] = value; }
        }

        // Moonkin 4-piece T5 bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("Starfire Crit Bonus")]
        [Category("Moonkin")]
        public float StarfireBonusWithDot
        {
            get { return _rawAdditiveData[(int)AdditiveStat.StarfireBonusWithDot]; }
            set { _rawAdditiveData[(int)AdditiveStat.StarfireBonusWithDot] = value; }
        }

        // Moonkin 2-piece T6 bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("Moonfire Extension")]
        [Category("Moonkin")]
        public float MoonfireExtension
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MoonfireExtension]; }
            set { _rawAdditiveData[(int)AdditiveStat.MoonfireExtension] = value; }
        }
        // Moonkin 4-piece T6 bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("Starfire Crit")]
        [Category("Moonkin")]
        public float StarfireCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.StarfireCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.StarfireCritChance] = value; }
        }

        // Moonkin 2-piece T7 bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("Insect Swarm Damage")]
        [Category("Moonkin")]
        public float BonusInsectSwarmDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusInsectSwarmDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusInsectSwarmDamage] = value; }
        }

        // Moonkin 4-piece T7 bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("SF/W Crit")]
        [Category("Moonkin")]
        public float BonusNukeCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusNukeCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusNukeCritChance] = value; }
        }

        // Moonkin 2-piece T8 bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("Eclipse Bonus")]
        [Category("Moonkin")]
        public float EclipseBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.EclipseBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.EclipseBonus] = value; }
        }
        // Moonkin 4-piece T8 bonus
        [DefaultValueAttribute(0f)]
        [DisplayName("Starfire Proc")]
        [Category("Moonkin")]
        public float StarfireProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.StarfireProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.StarfireProc] = value; }
        }
        #endregion
        #endregion

        #region MultiplicativeStats
        // threat dealt is damage * (1 + ThreatIncreaseMultiplier) * (1 - ThreatReductionMultiplier)
        [Percentage]
        [DisplayName("% Threat Increase")]
        [Category("Buffs / Debuffs")]
        [DefaultValueAttribute(0f)]
        public float ThreatIncreaseMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.ThreatIncreaseMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.ThreatIncreaseMultiplier] = value; }
        }

        [Percentage]
        [DisplayName("% Boss Attack Speed Reduction")]
        [Category("Buffs / Debuffs")]
        [DefaultValueAttribute(0f)]
        public float BossAttackSpeedMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BossAttackSpeedMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BossAttackSpeedMultiplier] = value; }
        }

        [Percentage]
        [Category("Equipment Effects")]
        [DefaultValueAttribute(0f)]
        [DisplayName("% Mana")]
        public float BonusManaMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaMultiplier] = value; }
        }

        [Percentage]
        [Category("Equipment Effects")]
        [DefaultValueAttribute(0f)]
        [DisplayName("% Crit Heal")]
        public float BonusCritHealMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritHealMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritHealMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Bonus Bleed Damage Multiplier")]
        [Category("Buffs / Debuffs")]
        public float BonusBleedDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusBleedDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusBleedDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Mangle (Bear) Threat")]
        [Category("Feral")]
        public float BonusMangleBearThreat
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleBearThreat]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleBearThreat] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Shield Slam Damage")]
        [Category("Warrior")]
        public float BonusShieldSlamDamage
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusShieldSlamDamage]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusShieldSlamDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% increased critical strike chance on Devastate")]
        public float DevastateCritIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DevastateCritIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.DevastateCritIncrease] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Slam Damage")]
        public float BonusSlamDamage
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSlamDamage]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSlamDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warrior")]
        [DisplayName("% Extra Rage Proc")]
        public float DreadnaughtBonusRageProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DreadnaughtBonusRageProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.DreadnaughtBonusRageProc] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Mage")]
        public float BonusMageNukeMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMageNukeMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMageNukeMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warlock")]
        public float BonusWarlockNukeMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockNukeMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockNukeMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Chance to increase SB/Inc crit chance")]
        [Category("Old Equipment Procs")]
        public float CorruptionTriggersCrit
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.CorruptionTriggersCrit]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.CorruptionTriggersCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% damage bonus on Unstable Affliction and 10% on Immolate")]
        [Category("Warlock")]
        public float Warlock2T8
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.Warlock2T8]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.Warlock2T8] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% crit chance on Shadowbolt and Incinerate")]
        [Category("Warlock")]
        public float Warlock4T8
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.Warlock4T8]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.Warlock4T8] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Warlock")]
        public float BonusWarlockDotDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockDotDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockDotDamageMultiplier] = value; }
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
        [DisplayName("% Strength")]
        public float BonusStrengthMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusStrengthMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusStrengthMultiplier] = value; }
        }

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
        [DisplayName("% Health")]
        public float BonusHealthMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealthMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealthMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Int")]
        public float BonusIntellectMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusIntellectMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusIntellectMultiplier] = value; }
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
        [DisplayName("% Base Armor")]
        public float BaseArmorMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BaseArmorMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BaseArmorMultiplier] = value; }
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
        [Category("Warlock")]
        [DisplayName("% Warlock Spell Power")]
        public float BonusSpellPowerDemonicPactMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellPowerDemonicPactMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellPowerDemonicPactMultiplier] = value; }
        }

        #region Added by Rawr.Hunter
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Hunter")]
        public float BonusRangedAttackPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRangedAttackPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRangedAttackPowerMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Hunter")]
        [DisplayName("% Bonus Steady Shot Damage")]
        public float BonusSteadyShotDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSteadyShotDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSteadyShotDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Hunter")]
        [DisplayName("% Bonus Pet Damage")]
        public float BonusPetDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetDamageMultiplier] = value; }
        }
        #endregion
        #region Added by Rawr.DPSWarr
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Rage on Crit")]
        [Category("Warrior")]
        public float BonusRageOnCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusRageOnCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusRageOnCrit] = value; }
        }
        #endregion
        #region Added by Rawr.Feral
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Feral")]
        [DisplayName("% Rip Dmg")]
        public float BonusRipDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRipDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRipDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Feral")]
        [DisplayName("% Ferocious Bite Dmg")]
        public float BonusFerociousBiteDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFerociousBiteDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFerociousBiteDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Feral")]
        [DisplayName("% Swipe Dmg")]
        public float BonusSwipeDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSwipeDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSwipeDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Feral")]
        [DisplayName("% Mangle Dmg")]
        public float BonusMangleDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Feral")]
        [DisplayName("% Maul Dmg")]
        public float BonusMaulDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMaulDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMaulDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Feral")]
        [DisplayName("% Rake Dmg")]
        public float BonusRakeDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRakeDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRakeDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Feral")]
        [DisplayName("% Shred Dmg")]
        public float BonusShredDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusShredDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusShredDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Feral")]
        [DisplayName("% Enrage Dmg")]
        public float BonusEnrageDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusEnrageDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusEnrageDamageMultiplier] = value; }
        }
        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Ferocious Bite Crit Chance")]
        [Category("Feral")]
        public float BonusFerociousBiteCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFerociousBiteCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFerociousBiteCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        [DisplayName("Bonus CP on Crit")]
        public float BonusCPOnCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCPOnCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCPOnCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        [DisplayName("Mangle Cooldown Reduction")]
        public float MangleCooldownReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MangleCooldownReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.MangleCooldownReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Feral")]
        [DisplayName("Tiger's Fury Cooldown Reduction")]
        public float TigersFuryCooldownReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TigersFuryCooldownReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.TigersFuryCooldownReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus Energy On Tiger's Fury")]
        [Category("Feral")]
        public float BonusEnergyOnTigersFury
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusEnergyOnTigersFury]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusEnergyOnTigersFury] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Finisher Energy On Avoid")]
        [Category("Feral")]
        public float FinisherEnergyOnAvoid
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FinisherEnergyOnAvoid]; }
            set { _rawAdditiveData[(int)AdditiveStat.FinisherEnergyOnAvoid] = value; }
        }
        #endregion
        #region Added by Rawr.HolyPriest
        /* See SpellCombatManaRegeneration, a stat that does exactly what the Primal Mooncloth set does
        [DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Bonus Manaregen While Casting")]
        public float BonusManaregenWhileCastingMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaregenWhileCastingMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaregenWhileCastingMultiplier] = value; }
        }*/

        [DefaultValueAttribute(0f)]
        [DisplayName("Inner Fire (Priest)")]
        [Category("Priest")]
        public float PriestInnerFire
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PriestInnerFire]; }
            set { _rawAdditiveData[(int)AdditiveStat.PriestInnerFire] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("seconds increased duration of Shadow Word: Pain (Priest)")]
        [Category("Priest")]
        public float SWPDurationIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SWPDurationIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.SWPDurationIncrease] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Priest")]
        [DisplayName("% increased damage from Mind Blast (Priest)")]
        public float BonusMindBlastMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMindBlastMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMindBlastMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Priest")]
        [DisplayName("% reduced cost on Mind Blast (Priest)")]
        public float MindBlastCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MindBlastCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.MindBlastCostReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Priest")]
        [DisplayName("% increased critical hit chance on Shadow Word: Death (Priest)")]
        public float ShadowWordDeathCritIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShadowWordDeathCritIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShadowWordDeathCritIncrease] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Priest")]
        [DisplayName("% increased damage on Devouring Plague. (Priest)")]
        public float DevouringPlagueBonusDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DevouringPlagueBonusDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.DevouringPlagueBonusDamage] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Priest")]
        [DisplayName("Extra haste rating for 4 seconds after casting Mind Blast. (Priest)")]
        public float MindBlastHasteProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MindBlastHasteProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.MindBlastHasteProc] = value; }
        }


        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Priest")]
        [DisplayName("% reduce the mana cost of PoH (Priest)")]
        public float BonusPoHManaCostReductionMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPoHManaCostReductionMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPoHManaCostReductionMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Priest")]
        [DisplayName("% increased healing from Greater Heal (Priest)")]
        public float BonusGHHealingMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusGHHealingMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusGHHealingMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("seconds increased duration of Renew (Priest)")]
        [Category("Priest")]
        public float RenewDurationIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RenewDurationIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.RenewDurationIncrease] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Mana gained on Greater Heal Overheals (Priest)")]
        [Category("Priest")]
        public float ManaGainOnGreaterHealOverheal
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaGainOnGreaterHealOverheal]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaGainOnGreaterHealOverheal] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Priest: Num extra jumps on Prayer of Mending (Priest)")]
        [Category("Priest")]
        public float PrayerOfMendingExtraJumps
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PrayerOfMendingExtraJumps]; }
            set { _rawAdditiveData[(int)AdditiveStat.PrayerOfMendingExtraJumps] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("% reduced cost of Greater Heal (Priest)")]
        [Category("Priest")]
        [Percentage]
        public float GreaterHealCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.GreaterHealCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.GreaterHealCostReduction] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("seconds reduced duration of Weakened Sou (Priest)l")]
        [Category("Priest")]
        public float WeakenedSoulDurationDecrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WeakenedSoulDurationDecrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.WeakenedSoulDurationDecrease] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("% increased critical chance on Prayer of Healing. (Priest)")]
        [Category("Priest")]
        [Percentage]
        public float PrayerOfHealingExtraCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PrayerOfHealingExtraCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.PrayerOfHealingExtraCrit] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Bonus spell power for 5 seconds after casting Power Word: Shield. (Priest)")]
        [Category("Priest")]
        public float PWSBonusSpellPowerProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PWSBonusSpellPowerProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.PWSBonusSpellPowerProc] = value; }
        }
        #endregion
        #region Added by Rawr.Elemental
        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Elemental")]
        public float BonusLavaBurstDamage
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusLavaBurstDamage]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusLavaBurstDamage] = value; }
        }
        #endregion
        #region Added by Rawr.Restosham
        // Tier 7 Shaman Set
        [DefaultValue(0f)]
        [Category("RestoSham")]
        [DisplayName("Increases the healing done by your Chain Heal and Healing Wave by 5%")]
        [Percentage]
        public float CHHWHealIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CHHWHealIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.CHHWHealIncrease] = value; }
        }
        [DefaultValue(0f)]
        [Category("RestoSham")]
        [DisplayName("Your Water Shield is 10% stronger")]
        [Percentage]
        public float WaterShieldIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WaterShieldIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.WaterShieldIncrease] = value; }
        }

        [DefaultValue(0f)]
        [Category("RestoSham")]
        [DisplayName("Reduce Lesser Healing Wave mana cost by 5%")]
        public float LHWManaReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LHWManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.LHWManaReduction] = value; }
        }

        [DefaultValue(0f)]
        [Category("RestoSham")]
        [DisplayName("Reduce Chain Heal mana cost by 10%")]
        public float CHManaReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CHManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.CHManaReduction] = value; }
        }

        [DefaultValue(0f)]
        [Category("RestoSham")]
        [DisplayName("Increase healing done by Chain Heal by 5%")]
        public float CHHealIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CHHealIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.CHHealIncrease] = value; }
        }
        #endregion
        #region Added by Rawr.ProtPaladin

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("ProtPaladin")]
        [DisplayName("% Hammer of the Righteous Damage")]
        public float BonusHammerOfTheRighteousMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.HammerOfTheRighteousMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.HammerOfTheRighteousMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("ProtPaladin")]
        [DisplayName("% Seal of Corruption Damage")]
        public float BonusSealOfCorruptionDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSealOfCorruptionDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSealOfCorruptionDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("ProtPaladin")]
        [DisplayName("% Seal of Righteousness Damage")]
        public float BonusSealOfRighteousnessDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSealOfRighteousnessDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSealOfRighteousnessDamageMultiplier] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Percentage]
        [Category("ProtPaladin")]
        [DisplayName("% Seal of Vengeance Damage")]
        public float BonusSealOfVengeanceDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSealOfVengeanceDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSealOfVengeanceDamageMultiplier] = value; }
        }

        #endregion

        #endregion

        #region InverseMultiplicativeStats
        // threat dealt is damage * (1 + ThreatIncreaseMultiplier) * (1 - ThreatReductionMultiplier)
        [Percentage]
        [Category("Buffs / Debuffs")]
        [DisplayName("% Threat Reduction")]
        [DefaultValueAttribute(0f)]
        public float ThreatReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ThreatReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ThreatReductionMultiplier] = value; }
        }

        [Percentage]
        [DisplayName("% Damage Taken")]
        [Category("Buffs / Debuffs")]
        [DefaultValueAttribute(0f)]
        public float DamageTakenMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.DamageTakenMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.DamageTakenMultiplier] = value; }
        }
        #endregion

        #region NoStackStats
        [DefaultValueAttribute(0f)]
        [DisplayName("% Increased Mana Potion")]
        [Percentage]
        [Category("Equipment Effects")]
        public float BonusManaPotion
        {
            get { return _rawNoStackData[(int)NonStackingStat.BonusManaPotion]; }
            set { _rawNoStackData[(int)NonStackingStat.BonusManaPotion] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Frost Res")]
        [Category("Resistances")]
        public float FrostResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.FrostResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.FrostResistanceBuff] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Nature Res")]
        [Category("Resistances")]
        public float NatureResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.NatureResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.NatureResistanceBuff] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Fire Res")]
        [Category("Resistances")]
        public float FireResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.FireResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.FireResistanceBuff] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Shadow Res")]
        [Category("Resistances")]
        public float ShadowResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.ShadowResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.ShadowResistanceBuff] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Arcane Res")]
        [Category("Resistances")]
        public float ArcaneResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.ArcaneResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.ArcaneResistanceBuff] = value; }
        }

        [DefaultValueAttribute(0f)]
        [Category("Misc")]
        public float MovementSpeed
        {
            get { return _rawNoStackData[(int)NonStackingStat.MovementSpeed]; }
            set { _rawNoStackData[(int)NonStackingStat.MovementSpeed] = value; }
        }

        [DefaultValueAttribute(0f)]
        [DisplayName("Less mana per spell (15 sec/1 min)")]
        [Category("Old Equipment Procs")]
        public float ManacostReduceWithin15OnUse1Min
        {
            get { return _rawNoStackData[(int)NonStackingStat.ManacostReduceWithin15OnUse1Min]; }
            set { _rawNoStackData[(int)NonStackingStat.ManacostReduceWithin15OnUse1Min] = value; }
        }

        #endregion

    }
}
