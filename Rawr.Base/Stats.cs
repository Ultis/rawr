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
		BonusArmor,
		ArmorPenetration,
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
        BonusCPGDamage,
        BonusEvisEnvenomDamage,
        BonusFreeFinisher,
        BonusLacerateDamageMultiplier,
        BonusManaGem,
        BonusMangleBearDamage,
        BonusMangleCatDamage,
        BonusRipDamagePerCPPerTick,
		BonusRipDuration,
        BonusShredDamage,
        BonusSnDDuration,
        BonusSnDHaste,
		BonusStreadyShotCrit,
        ExtraSpiritWhileCasting,
        CatFormStrength,
        CHHealIncrease,
        CHManaReduction,
        CPOnFinisher,
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
        EvocationExtension,
        ExecutionerProc,
        Expertise,
        ExpertiseRating,
        ExposeWeakness,
        FireResistance,
        FlashOfLightSpellPower,
        FlashOfLightMultiplier,
        FlashOfLightCrit,
        FrostResistance,
        HasteRating,
        HasteRatingOnPhysicalAttack,
        Healing,
        Health,
        PhysicalHit,
        HitRating,
        HolyLightManaReduction,
        HolyLightSpellPower,
        HolyLightPercentManaReduction,
        HolyLightCrit,
        HolyShockCrit,
        Hp5,
        IdolCritRating,
        InnervateCooldownReduction,
        Intellect,
        InterruptProtection,
        JudgementOfCommandAttackPowerBonus,
        LHWManaReduction,
        LightningCapacitorProc,
        LightweaveEmbroideryProc,
        ThunderCapacitorProc,
        LotPCritRating,
        MageAllResist,
        MageIceArmor,
        MageMageArmor,
        MageMoltenArmor,
        WarlockFelArmor,
        WarlockDemonArmor,
        WarlockGrandSpellstone,
        WarlockGrandFirestone,
        Mana,
        ManaSpringMp5Increase,
        ManaRestore5min,
        ManaRestorePerCast,
        ManaRestorePerCrit,
        ManaRestoreOnCast_5_15,
        ManaRestoreOnCast_10_45,
        ManaRestoreFromMaxManaPerHit,
        ManaRestoreFromMaxManaPerSecond,
        ManaRestoreOnCrit_25,
        MangleCostReduction,
		RakeCostReduction,
		ShredCostReduction,
        MementoProc,
        Miss,
        MoonfireDmg,
		MoonfireExtension,
		MongooseProc,
		BerserkingProc,
        MongooseProcAverage,
        MongooseProcConstant,
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
        SpellPowerFor10SecOnCast_15_45,
        SpellDamageFor10SecOnHit_5,
        SpellPowerFor10SecOnResist,
        SpellPowerFor15SecOnCrit_20_45,
        SpellPowerFor15SecOnManaGem,
        SpellPowerFor15SecOnUse90Sec,
        SpellPowerFor15SecOnUse2Min,
        SpellPowerFor20SecOnUse2Min,
        SpellPowerFor20SecOnUse5Min,
        SpellPowerFor6SecOnCrit,
        SpellDamageFromIntellectPercentage,
        SpellDamageFromSpiritPercentage,
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
        TrollDivinity,
        UnseenMoonDamageBonus,
        WeaponDamage,
        WindfuryAPBonus,
        WrathDmg,
        DruidAshtongueTrinket,
        AverageHeal, 
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
        LifebloomTickHealBonus,
        HealingTouchFinalHealBonus,
        HealingDoneFor15SecOnUse2Min,
        ManaregenFor8SecOnUse5Min,
        SpiritFor20SecOnUse2Min,
        ManaregenOver20SecOnUse3Min,
        ManaregenOver20SecOnUse5Min,
        ManacostReduceWithin15OnHealingCast,
        BangleProc,
        FullManaRegenFor15SecOnSpellcast,
		SpellPower,
        RenewDurationIncrease,
        SWPDurationIncrease,
        MindBlastCostReduction,
        ShadowWordDeathCritIncrease,
		ManaGainOnGreaterHealOverheal,
        PrayerOfMendingExtraJumps,
        GreaterHealCostReduction,
        WeakenedSoulDurationDecrease,
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
		LightningBoltCostReduction
        #endregion
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
        BonusCrusaderStrikeDamageMultiplier,
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
        BonusShieldSlamDamage,
        BonusSlamDamage,
        DreadnaughtBonusRageProc,
        BonusSpellCritMultiplier,
        BonusSpellPowerMultiplier,
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
        BonusPoHManaCostReductionMultiplier,
		BonusGHHealingMultiplier,
        BonusMindBlastMultiplier,
		PhysicalHaste,
		SpellHaste,
		HealingReceivedMultiplier,
		DamageTakenMultiplier,
        #region Added by Rawr.Elemental
        BonusLavaBurstDamage
        #endregion
    }

    enum InverseMultiplicativeStat : int
    {
        ThreatReductionMultiplier
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
        ManacostReduceWithin15OnUse1Min,
        #region Added by Rawr.*Priest for Glyphs
        GLYPH_FlashHeal,
        GLYPH_Dispel,
        GLYPH_PowerWordShield,
        GLYPH_CircleOfHealing,
        GLYPH_Renew,
        GLYPH_PrayerOfHealing,
        GLYPH_HolyNova,
        GLYPH_Lightwell,
        GLYPH_MassDispel,

        GLYPH_ShadowWordPain,
        GLYPH_Shadow,
        GLYPH_ShadowWordDeath,
        #endregion
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class Common : System.Attribute
    {
        public static bool IsCommonProperty(PropertyInfo property)
        {
            foreach (System.Attribute attribute in property.GetCustomAttributes(false))
                if (attribute is Common)
                    return true;
			return false;
		}
    }

    /// <summary>
    /// A Stats object represents a collection of stats on an object, such as an Item, Enchant, or Buff.
    /// </summary>
    /// 

    [Serializable]
    public unsafe class Stats
    {
        internal float[] _rawAdditiveData = new float[AdditiveStatCount];
        internal float[] _rawMultiplicativeData = new float[MultiplicativeStatCount];
        internal float[] _rawInverseMultiplicativeData = new float[InverseMultiplicativeStatCount];
        internal float[] _rawNoStackData = new float[NonStackingStatCount];

        //internal float[] _sparseData;
        internal int[] _sparseIndices;
        internal int _sparseAdditiveCount;
        internal int _sparseMultiplicativeCount;
        internal int _spareInverseMultiplicativeCount;
        internal int _sparseNoStackCount;

        public void InvalidateSparseData()
        {
            //_sparseData = null;
            _sparseIndices = null;
        }

        public void GenerateSparseData()
        {
            //List<float> data = new List<float>();
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
            _spareInverseMultiplicativeCount = 0;
            for (int i = 0; i < _rawInverseMultiplicativeData.Length; i++)
            {
                if (_rawInverseMultiplicativeData[i] != 0.0f)
                {
                    _spareInverseMultiplicativeCount++;
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
        [Common]
        public float Armor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Armor]; }
            set { _rawAdditiveData[(int)AdditiveStat.Armor] = value; }
        }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Category("Base Stats")]
        [Common]
		public float BonusArmor
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusArmor]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusArmor] = value; }
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
        [Common]
        public float Agility
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Agility]; }
            set { _rawAdditiveData[(int)AdditiveStat.Agility] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Agility before gear")]
        public float BaseAgility
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BaseAgility]; }
            set { _rawAdditiveData[(int)AdditiveStat.BaseAgility] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [Common]
        public float Stamina
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Stamina]; }
            set { _rawAdditiveData[(int)AdditiveStat.Stamina] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [Common]
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
        [Common]
        public float Strength
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Strength]; }
            set { _rawAdditiveData[(int)AdditiveStat.Strength] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [Common]
        public float WeaponDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WeaponDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.WeaponDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [Common]
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
        [DisplayName("Penetration Rating")]
        [Common]
		public float ArmorPenetrationRating
		{
			get { return _rawAdditiveData[(int)AdditiveStat.ArmorPenetrationRating]; }
			set { _rawAdditiveData[(int)AdditiveStat.ArmorPenetrationRating] = value; }
		}

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [Common]
        public float Intellect
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Intellect]; }
            set { _rawAdditiveData[(int)AdditiveStat.Intellect] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Base Stats")]
        [Common]
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

        [Percentage]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("% Spell Crit")]
        public float SpellCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Crit Rating")]
        [Common]
        public float SpellCritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellCritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellCritRating] = value; }
        }

        [Obsolete]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Damage")]
        [Common]
		public float SpellDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellDamageRating] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Category("Spell Combat Ratings")]
        [DisplayName("Spell Power")]
        [Common]
		public float SpellPower
		{
			get { return _rawAdditiveData[(int)AdditiveStat.SpellPower]; }
			set { _rawAdditiveData[(int)AdditiveStat.SpellPower] = value; }
		}

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Shadow Damage")]
        [Common]
        public float SpellShadowDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellShadowDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellShadowDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Fire Damage")]
        [Common]
        public float SpellFireDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellFireDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellFireDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Frost Damage")]
        [Common]
        public float SpellFrostDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellFrostDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellFrostDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Arcane Damage")]
        [Common]
        public float SpellArcaneDamageRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellArcaneDamageRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellArcaneDamageRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Spell Combat Ratings")]
        [DisplayName("Spell Nature Damage")]
        [Common]
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
        [DisplayName("Spell Hit Rating")]
        [Common]
        public float SpellHitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHitRating] = value; }
        }

        [Percentage]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("% Spell Hit")]
        public float SpellHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHit] = value; }
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
        [DisplayName("Spell Haste Rating")]
        [Common]
        public float SpellHasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [Category("Spell Combat Ratings")]
        [DisplayName("% Spell Haste")]
        public float SpellHaste
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.SpellHaste]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.SpellHaste] = value; }
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
        [Percentage]
        [DisplayName("% Physical Crit")]
        [Category("Combat Values")]
        public float PhysicalCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PhysicalCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.PhysicalCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Crit Rating")]
        [Common]
        public float CritRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Melee Crit")]
        [Common]
        public float CritMeleeRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritMeleeRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritMeleeRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Crit Bonus Damage")]
        public float CritBonusDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritBonusDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritBonusDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Hit Rating")]
        [Common]
        public float HitRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HitRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.HitRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Hit")]
        [Category("Combat Values")]
        public float PhysicalHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PhysicalHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.PhysicalHit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Dodge Rating")]
        [Common]
        public float DodgeRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DodgeRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.DodgeRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Dodge")]
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
        [DisplayName("Parry Rating")]
        [Common]
        public float ParryRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ParryRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ParryRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Values")]
        [Percentage]
        [DisplayName("% Block")]
        public float Block
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Block]; }
            set { _rawAdditiveData[(int)AdditiveStat.Block] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Block Rating")]
        [Common]
        public float BlockRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BlockRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.BlockRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Block Value")]
        [Common]
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
        [DisplayName("Defense Rating")]
        [Common]
        public float DefenseRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DefenseRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.DefenseRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Resilience")]
        [Common]
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
        [DisplayName("Expertise Rating")]
        [Common]
        public float ExpertiseRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExpertiseRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExpertiseRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Haste Rating")]
        [Common]
        public float HasteRating
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRating]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRating] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float HasteRatingOnPhysicalAttack
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRatingOnPhysicalAttack]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRatingOnPhysicalAttack] = value; }
        }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Physical Haste")]
		public float PhysicalHaste
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.PhysicalHaste]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.PhysicalHaste] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Healing Received")]
		public float HealingReceivedMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.HealingReceivedMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.HealingReceivedMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Mana per 5 sec")]
        [Common]
        public float Mp5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.Mp5]; }
            set { _rawAdditiveData[(int)AdditiveStat.Mp5] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Combat Ratings")]
        [DisplayName("Health per 5 sec")]
        [Common]
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
        public float InterruptProtection
        {
            get { return _rawAdditiveData[(int)AdditiveStat.InterruptProtection]; }
            set { _rawAdditiveData[(int)AdditiveStat.InterruptProtection] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusSnDDuration 
		{
            get { return _rawAdditiveData[(int)AdditiveStat.BonusSnDDuration]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusSnDDuration] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float CPOnFinisher 
		{
            get { return _rawAdditiveData[(int)AdditiveStat.CPOnFinisher]; }
            set { _rawAdditiveData[(int)AdditiveStat.CPOnFinisher] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusEvisEnvenomDamage 
		{
            get { return _rawAdditiveData[(int)AdditiveStat.BonusEvisEnvenomDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusEvisEnvenomDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusObliterateDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusObliterateDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusObliterateDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusScourgeStrikeDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusScourgeStrikeDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusScourgeStrikeDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusFreeFinisher 
		{
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFreeFinisher]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFreeFinisher] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusSnDHaste 
		{
            get { return _rawAdditiveData[(int)AdditiveStat.BonusSnDHaste]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusSnDHaste] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusCPGDamage 
		{
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCPGDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCPGDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Bonus Commanding Shout HP")]
        public float BonusCommandingShoutHP {
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
        public float BonusLacerateDamageMultiplier
        {
			get { return _rawAdditiveData[(int)AdditiveStat.BonusLacerateDamageMultiplier]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusLacerateDamageMultiplier] = value; }
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
		[DisplayName("Bonus Rip Duration")]
		public float BonusRipDuration
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusRipDuration]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusRipDuration] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Windfury")]
        public float WindfuryAPBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WindfuryAPBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.WindfuryAPBonus] = value; }
        }

        [Percentage]
        [DisplayName("% Max Mana / Hit")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestoreFromMaxManaPerHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromMaxManaPerHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromMaxManaPerHit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestorePerCast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCast]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCast] = value; }
        }

        // 5% proc rate, 15 sec internal cooldown
        [DisplayName("Mana Restore On Cast (5%)")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestoreOnCast_5_15
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCast_5_15]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCast_5_15] = value; }
        }

        // 10% proc rate, 45 sec internal cooldown
        [DisplayName("Mana Restore On Cast (10%)")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestoreOnCast_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCast_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCast_10_45] = value; }
        }

        // 10% proc rate, 45 sec internal cooldown
        [DisplayName("Mana Restore Per Crit")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestorePerCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestorePerCrit] = value; }
        }

        [Percentage]
        [DisplayName("% Max Mana / Sec")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRestoreFromMaxManaPerSecond
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromMaxManaPerSecond]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreFromMaxManaPerSecond] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MangleCatCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MangleCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.MangleCostReduction] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float RakeCostReduction
		{
			get { return _rawAdditiveData[(int)AdditiveStat.RakeCostReduction]; }
			set { _rawAdditiveData[(int)AdditiveStat.RakeCostReduction] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float ShredCostReduction
		{
			get { return _rawAdditiveData[(int)AdditiveStat.ShredCostReduction]; }
			set { _rawAdditiveData[(int)AdditiveStat.ShredCostReduction] = value; }
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

        [Percentage]
        [DisplayName("% Arcane Blast Bonus")]
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
        [DisplayName("Spell Power Increase for 6 sec on Crit")]
        public float SpellPowerFor6SecOnCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor6SecOnCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor6SecOnCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (50% 5 sec/Crit)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor5SecOnCrit_50
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor5SecOnCrit_50]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor5SecOnCrit_50] = value; }
        }

        // 15% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (15% 6 sec/Cast)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor6SecOnCast_15_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnCast_15_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnCast_15_45] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (10% 10 sec/Cast)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor10SecOnCast_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor10SecOnCast_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor10SecOnCast_10_45] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (10% 10 sec/Cast)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor10SecOnHeal_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor10SecOnHeal_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor10SecOnHeal_10_45] = value; }
        }
        
        // 10% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (10% 6 sec/Hit)")]
        [Category("Equipment Procs")]
        public float SpellHasteFor6SecOnHit_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnHit_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellHasteFor6SecOnHit_10_45] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec/Resist)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor10SecOnResist
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnResist]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnResist] = value; }
        }

        // trinket effect, does not sum up over gear, 2 trinkets with this effect is not equivalent to 1 trinket with double effect
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor20SecOnUse2Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (20 sec/5 min)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor20SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor20SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor20SecOnUse5Min] = value; }
        }

        // not used by any item
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (15 sec/2 min)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor15SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnUse2Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mana Restore on Crit (25%)")]
        [Category("Equipment Procs")]
        public float ManaRestoreOnCrit_25
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCrit_25]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestoreOnCrit_25] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (15 sec/1.5 min)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor15SecOnUse90Sec
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnUse90Sec]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnUse90Sec] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (20 sec/2 min)")]
        [Category("Equipment Procs")]
        public float HasteRatingFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRatingFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRatingFor20SecOnUse2Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Haste (20 sec/5 min)")]
        [Category("Equipment Procs")]
        public float HasteRatingFor20SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HasteRatingFor20SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.HasteRatingFor20SecOnUse5Min] = value; }
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
        [DisplayName("Spell Power (15 sec/Gem)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor15SecOnManaGem
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnManaGem]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnManaGem] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor10SecOnHit_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnHit_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnHit_10_45] = value; }
        }

        // 10% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor10SecOnHeal_10_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnHeal_10_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnHeal_10_45] = value; }
        }

        // 15% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor10SecOnCast_15_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCast_15_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCast_15_45] = value; }
        }

        // 20% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (15 sec)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor15SecOnCrit_20_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnCrit_20_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor15SecOnCrit_20_45] = value; }
        }

        // 20% chance, 45 sec internal cooldown
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spell Power (10 sec)")]
        [Category("Equipment Procs")]
        public float SpellPowerFor10SecOnCrit_20_45
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCrit_20_45]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpellPowerFor10SecOnCrit_20_45] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("PVP Trinket")]
        public float PVPTrinket
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PVPTrinket]; }
            set { _rawAdditiveData[(int)AdditiveStat.PVPTrinket] = value; }
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

		// Moonkin 2-piece T7 bonus
		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float BonusInsectSwarmDamage
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusInsectSwarmDamage]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusInsectSwarmDamage] = value; }
		}

		// Moonkin 4-piece T7 bonus
		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float BonusNukeCritChance
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusNukeCritChance]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusNukeCritChance] = value; }
		}

        // Tree 2-piece T5
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float RegrowthExtraTicks
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RegrowthExtraTicks]; }
            set { _rawAdditiveData[(int)AdditiveStat.RegrowthExtraTicks] = value; }
        }

        // Tree PvP Idols and 4-piece T5
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Lifebloom Final Heal")]
        public float LifebloomFinalHealBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LifebloomFinalHealBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.LifebloomFinalHealBonus] = value; }
        }

        // Tree 4-piece T6
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusHealingTouchMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusHealingTouchMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusHealingTouchMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Tree of Life Aura")]
        public float TreeOfLifeAura
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TreeOfLifeAura]; }
            set { _rawAdditiveData[(int)AdditiveStat.TreeOfLifeAura] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Reduced Rejuvenation Mana Cost")]
        public float ReduceRejuvenationCost
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ReduceRejuvenationCost]; }
            set { _rawAdditiveData[(int)AdditiveStat.ReduceRejuvenationCost] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Reduced Regrowth Mana Cost")]
        public float ReduceRegrowthCost
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ReduceRegrowthCost]; }
            set { _rawAdditiveData[(int)AdditiveStat.ReduceRegrowthCost] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Reduced Healing Touch Mana Cost")]
        public float ReduceHealingTouchCost
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ReduceHealingTouchCost]; }
            set { _rawAdditiveData[(int)AdditiveStat.ReduceHealingTouchCost] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Healing by Rejuvenation")]
        public float RejuvenationHealBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RejuvenationHealBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.RejuvenationHealBonus] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Healing by Lifebloom Ticks")]
        public float LifebloomTickHealBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LifebloomTickHealBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.LifebloomTickHealBonus] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Healing Touch Final Heal")]
        public float HealingTouchFinalHealBonus
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HealingTouchFinalHealBonus]; }
            set { _rawAdditiveData[(int)AdditiveStat.HealingTouchFinalHealBonus] = value; }
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
        [Category("Equipment Procs")]
        public float LightweaveEmbroideryProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightweaveEmbroideryProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightweaveEmbroideryProc] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Category("Equipment Procs")]
        public float ThunderCapacitorProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ThunderCapacitorProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ThunderCapacitorProc] = value; }
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
        public float MageIceArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageIceArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageIceArmor] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MageMageArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageMageArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageMageArmor] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MageMoltenArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MageMoltenArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.MageMoltenArmor] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float WarlockFelArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WarlockFelArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.WarlockFelArmor] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float WarlockDemonArmor
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WarlockDemonArmor]; }
            set { _rawAdditiveData[(int)AdditiveStat.WarlockDemonArmor] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float WarlockGrandSpellstone
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WarlockGrandSpellstone]; }
            set { _rawAdditiveData[(int)AdditiveStat.WarlockGrandSpellstone] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float WarlockGrandFirestone
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WarlockGrandFirestone]; }
            set { _rawAdditiveData[(int)AdditiveStat.WarlockGrandFirestone] = value; }
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
        public float CritChanceReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.CritChanceReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.CritChanceReduction] = value; }
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

        [DisplayName("Executioner Proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ExecutionerProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExecutionerProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExecutionerProc] = value; }
        }

        [DisplayName("Mongoose Proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MongooseProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MongooseProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.MongooseProc] = value; }
		}

		[DisplayName("Berserking Proc")]
		[Category("Equipment Procs")]
		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float BerserkingProc
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BerserkingProc]; }
			set { _rawAdditiveData[(int)AdditiveStat.BerserkingProc] = value; }
		}

        [DisplayName("Troll Divinity")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float TrollDivinity
        {
            get { return _rawAdditiveData[(int)AdditiveStat.TrollDivinity]; }
            set { _rawAdditiveData[(int)AdditiveStat.TrollDivinity] = value; }
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

        [DisplayName("Pendulum of Telluric Currents proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float PendulumOfTelluricCurrentsProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PendulumOfTelluricCurrentsProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.PendulumOfTelluricCurrentsProc] = value; }
        }

        [DisplayName("Extract of Necromantic Power proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ExtractOfNecromanticPowerProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExtractOfNecromanticPowerProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExtractOfNecromanticPowerProc] = value; }
        }

        [DisplayName("Darkmoon Card: Death proc")]
        [Category("Equipment Procs")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float DarkmoonCardDeathProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.DarkmoonCardDeathProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.DarkmoonCardDeathProc] = value; }
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
        [DisplayName("Mana every 5 min")]
        [Category("Equipment Procs")]
        public float ManaRestore5min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRestore5min]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRestore5min] = value; }
        }


        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Flash of Light Spell Power")]
        [Category("Equipment Procs")]
        public float FlashOfLightSpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FlashOfLightSpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.FlashOfLightSpellPower] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Flash of Light Healing")]
        [Category("Equipment Procs")]
        public float FlashOfLightMultiplier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FlashOfLightMultiplier]; }
            set { _rawAdditiveData[(int)AdditiveStat.FlashOfLightMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Flash of Light Crit")]
        [Category("Equipment Procs")]
        public float FlashOfLightCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FlashOfLightCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.FlashOfLightCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Holy Light Spell Power")]
        [Category("Equipment Procs")]
        public float HolyLightSpellPower
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyLightSpellPower]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyLightSpellPower] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Holy Light Mana Cost Reduction")]
        [Category("Equipment Procs")]
        public float HolyLightManaCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyLightManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyLightManaReduction] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Holy Light Crit")]
        [Category("Equipment Procs")]
        public float HolyLightCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyLightCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyLightCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Holy Shock Crit")]
        [Category("Equipment Procs")]
        public float HolyShockCrit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyShockCrit]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyShockCrit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Holy Light Cost Reduction")]
        [Category("Equipment Procs")]
        public float HolyLightPercentManaReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HolyLightPercentManaReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.HolyLightPercentManaReduction] = value; }
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
		[Percentage] /*its not really multiplicative, but it is stored as .02 instead of 2 because crit is % based 0-1, so this attribute makes it display correctly */ 
		[DisplayName("% Extra Pet Crit Chance")]
		public float BonusPetCritChance
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusPetCritChance]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusPetCritChance] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage] /*Same as above*/
		[DisplayName("% Extra Steady Shot Crit")]
		public float BonusSteadyShotCrit
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusStreadyShotCrit]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusStreadyShotCrit] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float AshtongueTrinketProc
		{
			get { return _rawAdditiveData[(int)AdditiveStat.AshtongueTrinketProc]; }
			set { _rawAdditiveData[(int)AdditiveStat.AshtongueTrinketProc] = value; }
		}

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Healing for 15sec (2 min cd)")]
        [Category("Equipment Procs")]
        public float HealingDoneFor15SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.HealingDoneFor15SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.HealingDoneFor15SecOnUse2Min] = value; }
        }

        /* Regen trinkets */
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mana each sec. for 8 sec. (5 min cd)")]
        [Category("Equipment Procs")]
        public float ManaregenFor8SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaregenFor8SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaregenFor8SecOnUse5Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spirit for 20 sec. (2 min cd)")]
        [Category("Equipment Procs")]
        public float SpiritFor20SecOnUse2Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SpiritFor20SecOnUse2Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.SpiritFor20SecOnUse2Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Spirit while casting")]
        [Category("Equipment Procs")]
        public float ExtraSpiritWhileCasting
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ExtraSpiritWhileCasting]; }
            set { _rawAdditiveData[(int)AdditiveStat.ExtraSpiritWhileCasting] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mana restores over 20 sec. (2 min cd)")]
        [Category("Equipment Procs")]
        public float ManaregenOver20SecOnUse3Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaregenOver20SecOnUse3Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaregenOver20SecOnUse3Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mana restores over 20 sec. (5 min cd)")]
        [Category("Equipment Procs")]
        public float ManaregenOver20SecOnUse5Min
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaregenOver20SecOnUse5Min]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaregenOver20SecOnUse5Min] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Mana cost of next spell reduce (within 15 sec.)")]
        [Category("Equipment Procs")]
        public float ManacostReduceWithin15OnHealingCast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManacostReduceWithin15OnHealingCast]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManacostReduceWithin15OnHealingCast] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Bangle Proc")]
        [Category("Equipment Procs")]
        public float BangleProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BangleProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.BangleProc] = value; }
        }
        
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("% chance on spellcast to allow 100% mana regen for 15 sec")]
        public float FullManaRegenFor15SecOnSpellcast
        {
            get { return _rawAdditiveData[(int)AdditiveStat.FullManaRegenFor15SecOnSpellcast]; }
            set { _rawAdditiveData[(int)AdditiveStat.FullManaRegenFor15SecOnSpellcast] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Extra heal over time from direct healing spells")]
        public float BonusHoTOnDirectHeals
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusHoTOnDirectHeals]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusHoTOnDirectHeals] = value; }
        }

        

        #region Added by Rawr.Elemental
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusCritChance] = value; }
        }

        
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusThunderCritChance
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusThunderCritChance]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusThunderCritChance] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusShamanHit
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusShamanHit]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusShamanHit] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ManaRegenIntPer5
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaRegenIntPer5]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaRegenIntPer5] = value; }
        }
        
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ShamanCastTimeReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShamanCastTimeReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShamanCastTimeReduction] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float LightningOverloadProc
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningOverloadProc]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningOverloadProc] = value; }
        }
     
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusLavaBurstCritDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusLavaBurstCritDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusLavaBurstCritDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ChainLightningCooldownReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ChainLightningCooldownReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.ChainLightningCooldownReduction] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusFlameShockDoTDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFlameShockDoTDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFlameShockDoTDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusFlametongueDamage
        {
            get { return _rawAdditiveData[(int)AdditiveStat.BonusFlametongueDamage]; }
            set { _rawAdditiveData[(int)AdditiveStat.BonusFlametongueDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ShockManaCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShockManaCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShockManaCostReduction] = value; }
        }
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float LightningBoltCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningBoltCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningBoltCostReduction] = value; }
        }
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float LightningBoltDamageModifier
        {
            get { return _rawAdditiveData[(int)AdditiveStat.LightningBoltDamageModifier]; }
            set { _rawAdditiveData[(int)AdditiveStat.LightningBoltDamageModifier] = value; }
        }

        #endregion
#endregion

        #region MultiplicativeStats
        // threat dealt is damage * (1 + ThreatIncreaseMultiplier) * (1 - ThreatReductionMultiplier)
        [Percentage]
        [DisplayName("% Threat Increase")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ThreatIncreaseMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.ThreatIncreaseMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.ThreatIncreaseMultiplier] = value; }
        }

        [Percentage]
        [DisplayName("% Mana")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusManaMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaMultiplier] = value; }
        }

        [Percentage]
        [DisplayName("% Crit Heal")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float BonusCritHealMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritHealMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritHealMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% CStrike Dmg")]
        public float BonusCrusaderStrikeDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCrusaderStrikeDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCrusaderStrikeDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("Bonus Bleed Damage Multiplier")]
        public float BonusBleedDamageMultiplier {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusBleedDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusBleedDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Mangle (Bear) Threat")]
        public float BonusMangleBearThreat
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleBearThreat]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleBearThreat] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Shield Slam Damage")]
        public float BonusShieldSlamDamage
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusShieldSlamDamage]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusShieldSlamDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Slam Damage")]
        public float BonusSlamDamage
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSlamDamage]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSlamDamage] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("Extra Rage Proc")]
        public float DreadnaughtBonusRageProc
        {
            get { return _rawAdditiveData[(int)MultiplicativeStat.DreadnaughtBonusRageProc]; }
            set { _rawAdditiveData[(int)MultiplicativeStat.DreadnaughtBonusRageProc] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        public float BonusMageNukeMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMageNukeMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMageNukeMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        public float BonusWarlockNukeMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockNukeMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockNukeMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        public float BonusWarlockDotDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockDotDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusWarlockDotDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Agility")]
        public float BonusAgilityMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusAgilityMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusAgilityMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Strength")]
        public float BonusStrengthMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusStrengthMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusStrengthMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Stamina")]
        public float BonusStaminaMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusStaminaMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusStaminaMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Health")]
        public float BonusHealthMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealthMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusHealthMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Int")]
        public float BonusIntellectMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusIntellectMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusIntellectMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Armor")]
        public float BonusArmorMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusArmorMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusArmorMultiplier] = value; }
        }

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Base Armor")]
		public float BaseArmorMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BaseArmorMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BaseArmorMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Block Value")]
        public float BonusBlockValueMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusBlockValueMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusBlockValueMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% AP")]
        public float BonusAttackPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusAttackPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusAttackPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% SP")]
        public float BonusSpellPowerMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellPowerMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellPowerMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Fire Damage")]
        public float BonusFireDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFireDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFireDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Shadow Damage")]
        public float BonusShadowDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusShadowDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusShadowDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Holy Damage")]
		public float BonusHolyDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusHolyDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusHolyDamageMultiplier] = value; }
		}

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Arcane Damage")]
        public float BonusArcaneDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusArcaneDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusArcaneDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Nature Damage")]
        public float BonusNatureDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusNatureDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusNatureDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Frost Damage")]
        public float BonusFrostDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFrostDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Disease Damage")]
        public float BonusDiseaseDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDiseaseDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDiseaseDamageMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Spirit")]
        public float BonusSpiritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpiritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpiritMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Crit Dmg")]
        public float BonusCritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusCritMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Spell Crit Dmg")]
        public float BonusSpellCritMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellCritMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSpellCritMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Rip Dmg")]
        public float BonusRipDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRipDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRipDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Ferocious Bite Dmg")]
		public float BonusFerociousBiteDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusFerociousBiteDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusFerociousBiteDamageMultiplier] = value; }
		}

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Swipe Dmg")]
        public float BonusSwipeDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSwipeDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSwipeDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Mangle Dmg")]
		public float BonusMangleDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMangleDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Maul Dmg")]
		public float BonusMaulDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMaulDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMaulDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Rake Dmg")]
		public float BonusRakeDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRakeDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRakeDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Shred Dmg")]
		public float BonusShredDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusShredDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusShredDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Enrage Dmg")]
		public float BonusEnrageDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusEnrageDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusEnrageDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Physical Dmg")]
		public float BonusPhysicalDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPhysicalDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPhysicalDamageMultiplier] = value; }
		}

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% Dmg")]
        public float BonusDamageMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusDamageMultiplier] = value; }
        }
		
		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		public float BonusRangedAttackPowerMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusRangedAttackPowerMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusRangedAttackPowerMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Bonus Steady Shot Damage")]
		public float BonusSteadyShotDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusSteadyShotDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusSteadyShotDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[Percentage]
		[DisplayName("% Bonus Pet Damage")]
		public float BonusPetDamageMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetDamageMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPetDamageMultiplier] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("Bonus Rage on Crit")]
		public float BonusRageOnCrit
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusRageOnCrit]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusRageOnCrit] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("Bonus Ferocious Bite Crit Chance")]
		public float BonusFerociousBiteCrit
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusFerociousBiteCrit]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusFerociousBiteCrit] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("Bonus CP on Crit")]
		public float BonusCPOnCrit
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusCPOnCrit]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusCPOnCrit] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("Mangle Cooldown Reduction")]
		public float MangleCooldownReduction
		{
			get { return _rawAdditiveData[(int)AdditiveStat.MangleCooldownReduction]; }
			set { _rawAdditiveData[(int)AdditiveStat.MangleCooldownReduction] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("Bonus Energy On Tiger's Fury")]
		public float BonusEnergyOnTigersFury
		{
			get { return _rawAdditiveData[(int)AdditiveStat.BonusEnergyOnTigersFury]; }
			set { _rawAdditiveData[(int)AdditiveStat.BonusEnergyOnTigersFury] = value; }
		}

		[System.ComponentModel.DefaultValueAttribute(0f)]
		[DisplayName("Finisher Energy On Avoid")]
		public float FinisherEnergyOnAvoid
		{
			get { return _rawAdditiveData[(int)AdditiveStat.FinisherEnergyOnAvoid]; }
			set { _rawAdditiveData[(int)AdditiveStat.FinisherEnergyOnAvoid] = value; }
		}

        // Holy Priest set bonuses

        /* See SpellCombatManaRegeneration, a stat that does exactly what the Primal Mooncloth set does
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Multiplicative]
        [DisplayName("% Bonus Manaregen While Casting")]
        public float BonusManaregenWhileCastingMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaregenWhileCastingMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusManaregenWhileCastingMultiplier] = value; }
        }*/

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName(" seconds increased duration of Shadow Word: Pain")]
        public float SWPDurationIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.SWPDurationIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.SWPDurationIncrease] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% increased damage from Mind Blast")]
        public float BonusMindBlastMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusMindBlastMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusMindBlastMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% reduced cost on Mind Blast")]
        public float MindBlastCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.MindBlastCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.MindBlastCostReduction] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% increased critical hit chance on Shadow Word: Death")]
        public float ShadowWordDeathCritIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ShadowWordDeathCritIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.ShadowWordDeathCritIncrease] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% reduce the mana cost of PoH")]
        public float BonusPoHManaCostReductionMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusPoHManaCostReductionMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusPoHManaCostReductionMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% increased healing from Greater Heal")]
        public float BonusGHHealingMultiplier
        {
            get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusGHHealingMultiplier]; }
            set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusGHHealingMultiplier] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName(" second increased duration of Renew")]
        public float RenewDurationIncrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.RenewDurationIncrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.RenewDurationIncrease] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName(" mana gained on Greater Heal Overheals")]
        public float ManaGainOnGreaterHealOverheal
        {
            get { return _rawAdditiveData[(int)AdditiveStat.ManaGainOnGreaterHealOverheal]; }
            set { _rawAdditiveData[(int)AdditiveStat.ManaGainOnGreaterHealOverheal] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName(" extra jumps on Prayer of Mending")]
        public float PrayerOfMendingExtraJumps
        {
            get { return _rawAdditiveData[(int)AdditiveStat.PrayerOfMendingExtraJumps]; }
            set { _rawAdditiveData[(int)AdditiveStat.PrayerOfMendingExtraJumps] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("% reduced cost of Greater Heal")]
        [Percentage]
        public float GreaterHealCostReduction
        {
            get { return _rawAdditiveData[(int)AdditiveStat.GreaterHealCostReduction]; }
            set { _rawAdditiveData[(int)AdditiveStat.GreaterHealCostReduction] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName(" second reduced duration of Weakened Soul")]
        public float WeakenedSoulDurationDecrease
        {
            get { return _rawAdditiveData[(int)AdditiveStat.WeakenedSoulDurationDecrease]; }
            set { _rawAdditiveData[(int)AdditiveStat.WeakenedSoulDurationDecrease] = value; }
        }

        #region Added by Rawr.Elemental
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        public float BonusLavaBurstDamage
        {
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.BonusLavaBurstDamage]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.BonusLavaBurstDamage] = value; }
        }
        #endregion

        #endregion

        #region InverseMultiplicativeStats
        // threat dealt is damage * (1 + ThreatIncreaseMultiplier) * (1 - ThreatReductionMultiplier)
        [Percentage]
        [DisplayName("% Threat Reduction")]
        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float ThreatReductionMultiplier
        {
            get { return _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ThreatReductionMultiplier]; }
            set { _rawInverseMultiplicativeData[(int)InverseMultiplicativeStat.ThreatReductionMultiplier] = value; }
        }
		
		[Percentage]
		[DisplayName("% Damage Taken")]
		[System.ComponentModel.DefaultValueAttribute(0f)]
		public float DamageTakenMultiplier
		{
			get { return _rawMultiplicativeData[(int)MultiplicativeStat.DamageTakenMultiplier]; }
			set { _rawMultiplicativeData[(int)MultiplicativeStat.DamageTakenMultiplier] = value; }
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

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Frost Res")]
        public float FrostResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.FrostResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.FrostResistanceBuff] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Nature Res")]
        public float NatureResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.NatureResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.NatureResistanceBuff] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Fire Res")]
        public float FireResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.FireResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.FireResistanceBuff] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Shadow Res")]
        public float ShadowResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.ShadowResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.ShadowResistanceBuff] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Arcane Res")]
        public float ArcaneResistanceBuff
        {
            get { return _rawNoStackData[(int)NonStackingStat.ArcaneResistanceBuff]; }
            set { _rawNoStackData[(int)NonStackingStat.ArcaneResistanceBuff] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        public float MovementSpeed
        {
            get { return _rawNoStackData[(int)NonStackingStat.MovementSpeed]; }
            set { _rawNoStackData[(int)NonStackingStat.MovementSpeed] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName("Less mana per spell (15 sec/1 min)")]
        [Category("Equipment Procs")]
        public float ManacostReduceWithin15OnUse1Min
        {
            get { return _rawNoStackData[(int)NonStackingStat.ManacostReduceWithin15OnUse1Min]; }
            set { _rawNoStackData[(int)NonStackingStat.ManacostReduceWithin15OnUse1Min] = value; }
        }

        #region Added by Rawr.*Priest for Glyphs
        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% reduced cost on Flash Heal")]
        public float GLYPH_FlashHeal
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_FlashHeal]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_FlashHeal] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% of max health healed on Dispel.")]
        public float GLYPH_Dispel
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_Dispel]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_Dispel] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% of Absorb healed on cast.")]
        public float GLYPH_PowerWordShield
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_PowerWordShield]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_PowerWordShield] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName(" extra target(s) hit by Circle of Healing.")]
        public float GLYPH_CircleOfHealing
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_CircleOfHealing]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_CircleOfHealing] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [DisplayName(" second shorter duration on Renew, but increased healing each tick.")]
        public float GLYPH_Renew
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_Renew]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_Renew] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% of healed amount also healed over time.")]
        public float GLYPH_PrayerOfHealing
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_PrayerOfHealing]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_PrayerOfHealing] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% increased healing and reduced damage from Holy Nova.")]
        public float GLYPH_HolyNova
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_HolyNova]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_HolyNova] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% increased healing from Lightwell.")]
        public float GLYPH_Lightwell
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_Lightwell]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_Lightwell] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% cheaper Mass Dispel.")]
        public float GLYPH_MassDispel
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_MassDispel]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_MassDispel] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% increased Mind Flay damage on targets with Shadow Word: Pain.")]
        public float GLYPH_ShadowWordPain
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_ShadowWordPain]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_ShadowWordPain] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% of spirit added as spell power after spell criticals while in Shadowform.")]
        public float GLYPH_Shadow
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_Shadow]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_Shadow] = value; }
        }

        [System.ComponentModel.DefaultValueAttribute(0f)]
        [Percentage]
        [DisplayName("% increased damage on Shadow Word: Death when target is below 35%.")]
        public float GLYPH_ShadowWordDeath
        {
            get { return _rawNoStackData[(int)NonStackingStat.GLYPH_ShadowWordDeath]; }
            set { _rawNoStackData[(int)NonStackingStat.GLYPH_ShadowWordDeath] = value; }
        }
        #endregion

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

        public void Accumulate(Stats data)
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
                for (int a = 0; a < data._sparseAdditiveCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    pRawAdditiveData[index] += data._rawAdditiveData[index];
                }
                for (int a = 0; a < data._sparseMultiplicativeCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    float* pa = pRawMultiplicativeData + index;
                    *pa = (1 + *pa) * (1 + data._rawMultiplicativeData[index]) - 1;
                }
                for (int a = 0; a < data._spareInverseMultiplicativeCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    _rawInverseMultiplicativeData[index] = 1 - (1 - _rawInverseMultiplicativeData[index]) * (1 - data._rawInverseMultiplicativeData[index]);
                }
                for (int a = 0; a < data._sparseNoStackCount; a++, i++)
                {
                    int index = data._sparseIndices[i];
                    float* pa = pRawNoStackData + index;
                    float value = data._rawNoStackData[index];
                    if (value > *pa) *pa = value;
                }
            }
            else
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
                    if (IsPercentage(info))
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
			clone._rawInverseMultiplicativeData = (float[])clone._rawInverseMultiplicativeData.Clone();
            clone._rawNoStackData = (float[]) clone._rawNoStackData.Clone();
            return clone;
        }

		public void ConvertStatsToWotLKEquivalents()
		{
			HitRating = Math.Max(HitRating, SpellHitRating);
			CritRating = Math.Max(CritRating, SpellCritRating);
			HasteRating = Math.Max(HasteRating, SpellHasteRating);
			SpellPower = Math.Max(SpellPower, Math.Max(SpellDamageRating, (float)Math.Floor(Healing / 1.88f)));
			ArmorPenetrationRating = Math.Max(ArmorPenetrationRating, (float)Math.Floor(ArmorPenetration / 7f));

			SpellHitRating = SpellCritRating = SpellHasteRating = SpellDamageRating = Healing = ArmorPenetration = 0;
		}

        #region Multiplicative Handling
        private static PropertyInfo[] _propertyInfoCache = null;
        private static List<PropertyInfo> _percentageProperties = new List<PropertyInfo>();
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
                if (info.GetCustomAttributes(typeof(PercentageAttribute), false).Length > 0)
                {
                    _percentageProperties.Add(info);
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

        public static bool IsPercentage(PropertyInfo info)
        {
            return _percentageProperties.Contains(info);
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
    public class PercentageAttribute : Attribute
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
