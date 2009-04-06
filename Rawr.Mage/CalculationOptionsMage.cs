using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace Rawr.Mage
{
    [Serializable]
    public class SpellWeight
    {
        public SpellId Spell { get; set; }
        public float Weight { get; set; }
    }

    public class CooldownRestriction
    {
        public double TimeStart { get; set; }
        public double TimeEnd { get; set; }
        public StateDescription.StateDescriptionDelegate IsMatch { get; set; }
    }

    [Serializable]
    public sealed class CalculationOptionsMage : ICalculationOptionBase
    {
        private int playerLevel;
        private float levelScalingFactor;

        public int PlayerLevel
        {
            get
            {
                return playerLevel;
            }
            set
            {
                playerLevel = value;
                levelScalingFactor = (float)((52f / 82f) * Math.Pow(63f / 131f, (playerLevel - 70) / 10f));
            }
        }

        public float LevelScalingFactor
        {
            get
            {
                return levelScalingFactor;
            }
        }

        public int TargetLevel { get; set; }
        public int AoeTargetLevel { get; set; }
        public float Latency { get; set; }
        public int AoeTargets { get; set; }
        public float ArcaneResist { get; set; }
        public float FireResist { get; set; }
        public float FrostResist { get; set; }
        public float NatureResist { get; set; }
        public float ShadowResist { get; set; }
        public float HolyResist { get; set; }
        public float FightDuration { get; set; }
        public float TpsLimit { get; set; }
        public bool HeroismAvailable { get; set; }
        public bool PowerInfusionAvailable { get; set; }
        public bool PotionOfWildMagic { get; set; }
        public bool PotionOfSpeed { get; set; }
        public bool FlameCap { get; set; }
        public float MoltenFuryPercentage { get; set; }
        public bool MaintainScorch { get; set; }
        public bool MaintainSnare { get; set; }
        public float InterruptFrequency { get; set; }
        public float EvocationWeapon { get; set; }
        public float EvocationSpirit { get; set; }
        public float AoeDuration { get; set; }
        public bool SmartOptimization { get; set; }
        public float DpsTime { get; set; }
        public bool DrumsOfBattle { get; set; }
        public bool AutomaticArmor { get; set; }
        public bool IncrementalOptimizations { get; set; }
        public float SnaredTime { get; set; }
        public bool Mode31 { get; set; }
        public float WarlockSpellPower { get; set; }

        [XmlIgnore]
        public Cooldown[] IncrementalSetStateIndexes;
        [XmlIgnore]
        public Cooldown[] IncrementalSetSortedStates;
        [XmlIgnore]
        public int[] IncrementalSetSegments;
        [XmlIgnore]
        public CycleId[] IncrementalSetSpells;
        [XmlIgnore]
        public string IncrementalSetArmor;
        [XmlIgnore]
        public VariableType[] IncrementalSetVariableType;
        [XmlIgnore]
        public Rawr.Mage.SequenceReconstruction.Sequence SequenceReconstruction;
        [XmlIgnore]
        public bool AdviseAdvancedSolver;
        [XmlIgnore]
        public CharacterCalculationsMage Calculations; // calculations that are result of the last display in rawr

        [XmlIgnore]
        private Character _character;
        [XmlIgnore]
        public Character Character
        {
            get
            {
                return _character;
            }
            set
            {
                value.CalculationsInvalidated += new EventHandler(Character_ItemsChanged);
                _character = value;
            }
        }

        private void Character_ItemsChanged(object sender, EventArgs e)
        {
            IncrementalSetStateIndexes = null;
            IncrementalSetSegments = null;
            IncrementalSetSortedStates = null;
            IncrementalSetSpells = null;
            CooldownRestrictionList = null;
        }

        public bool ReconstructSequence { get; set; }

        public bool ComparisonSegmentCooldowns { get; set; }
        public bool DisplaySegmentCooldowns { get; set; }
        public bool ComparisonIntegralMana { get; set; }
        public bool DisplayIntegralMana { get; set; }
        public int ComparisonAdvancedConstraintsLevel { get; set; }
        public int DisplayAdvancedConstraintsLevel { get; set; }
        public int FixedSegmentDuration { get; set; }
        public bool VariableSegmentDuration { get; set; }
        public string AdditionalSegmentSplits { get; set; }
        public double LowerBoundHint { get; set; }
        public string CooldownRestrictions { get; set; }
        public bool EnableHastedEvocation { get; set; }
        [XmlIgnore]
        public List<CooldownRestriction> CooldownRestrictionList;
        public bool CooldownRestrictionsValid(Segment segment, CastingState state)
        {
            if (CooldownRestrictionList == null) return true;
            foreach (CooldownRestriction restriction in CooldownRestrictionList)
            {
                if (segment.TimeStart >= restriction.TimeStart && segment.TimeEnd <= restriction.TimeEnd)
                {
                    if (!restriction.IsMatch(state.Cooldown)) return false;
                }
            }
            return true;
        }

        public MIPMethod MIPMethod { get; set; }

        public float Innervate { get; set; }
        public float ManaTide { get; set; }
        public float Fragmentation { get; set; }
        public float SurvivabilityRating { get; set; }
        public bool Aldor { get; set; }
        public int HeroismControl { get; set; } // 0 = optimal, 1 = before 20%, 2 = no cooldowns, 3 = after 20%
        public bool AverageCooldowns { get; set; }
        public bool EvocationEnabled { get; set; }
        public bool ManaPotionEnabled { get; set; }
        public bool ManaGemEnabled { get; set; }
        public bool DisableCooldowns { get; set; }
        public int MaxHeapLimit { get; set; }
        public float DrinkingTime { get; set; }
        public float TargetDamage { get; set; }
        public bool FarmingMode { get; set; }
        public float FocusMagicTargetCritRate { get; set; }
        public bool UnlimitedMana { get; set; }
        public float AbsorptionPerSecond { get; set; }

        public List<SpellWeight> CustomSpellMix { get; set; }
        public bool CustomSpellMixEnabled { get; set; }
        public bool CustomSpellMixOnly { get; set; }

        public float MeleeDps { get; set; }
        public float MeleeCrit { get; set; }
        public float MeleeDot { get; set; }
        public float PhysicalDps { get; set; }
        public float PhysicalCrit { get; set; }
        public float PhysicalDot { get; set; }
        public float FireDps { get; set; }
        public float FireCrit { get; set; }
        public float FireDot { get; set; }
        public float FrostDps { get; set; }
        public float FrostCrit { get; set; }
        public float FrostDot { get; set; }
        public float ArcaneDps { get; set; }
        public float ArcaneCrit { get; set; }
        public float ArcaneDot { get; set; }
        public float ShadowDps { get; set; }
        public float ShadowCrit { get; set; }
        public float ShadowDot { get; set; }
        public float NatureDps { get; set; }
        public float NatureCrit { get; set; }
        public float NatureDot { get; set; }
        public float HolyDps { get; set; }
        public float HolyCrit { get; set; }
        public float HolyDot { get; set; }

        public float BurstWindow { get; set; }
        public float BurstImpacts { get; set; }
        //public float ChanceToLiveLimit { get; set; }
        public float ChanceToLiveScore { get; set; }

        public float EffectSpiritBonus { get; set; }
        public float EffectCritBonus { get; set; }
        public float EffectShadowSilenceFrequency { get; set; }
        public float EffectShadowSilenceDuration { get; set; }
        public float EffectShadowManaDrainFrequency { get; set; }
        public float EffectShadowManaDrain { get; set; }
        public float EffectArcaneOtherBinary { get; set; }
        public float EffectFireOtherBinary { get; set; }
        public float EffectFrostOtherBinary { get; set; }
        public float EffectShadowOtherBinary { get; set; }
        public float EffectNatureOtherBinary { get; set; }
        public float EffectHolyOtherBinary { get; set; }
        public float EffectArcaneOther { get; set; }
        public float EffectFireOther { get; set; }
        public float EffectFrostOther { get; set; }
        public float EffectShadowOther { get; set; }
        public float EffectNatureOther { get; set; }
        public float EffectHolyOther { get; set; }

        [XmlIgnore]
        public string ShattrathFaction
        {
            get
            {
                return Aldor ? "Aldor" : "Scryers";
            }
            set
            {
                Aldor = (value == "Aldor");
            }
        }

        public CalculationOptionsMage Clone()
        {
            CalculationOptionsMage clone = (CalculationOptionsMage)MemberwiseClone();
            clone.IncrementalSetArmor = null;
            clone.IncrementalSetStateIndexes = null;
            clone.IncrementalSetSegments = null;
            clone.IncrementalSetSpells = null;
            return clone;
        }

        private CalculationOptionsMage()
        {
            TargetLevel = 83;
            AoeTargetLevel = 80;
            Latency = 0.1f;
            AoeTargets = 9;
            FightDuration = 300;
            HeroismAvailable = true;
            MoltenFuryPercentage = 0.3f;
            PotionOfWildMagic = true;
            PotionOfSpeed = true;
            FlameCap = false;
            DpsTime = 1;
            InterruptFrequency = 0;
            EvocationWeapon = 0;
            AoeDuration = 0;
            SmartOptimization = false;
            DrumsOfBattle = false;
            AutomaticArmor = true;
            TpsLimit = 0;
            IncrementalOptimizations = true;
            ReconstructSequence = false;
            Innervate = 0;
            ManaTide = 0;
            Fragmentation = 0;
            EvocationSpirit = 0;
            SurvivabilityRating = 0.0001f;
            Aldor = true;
            EvocationEnabled = true;
            ManaPotionEnabled = true;
            ManaGemEnabled = true;
            MaxHeapLimit = 300;
            DrinkingTime = 300;
            BurstWindow = 5f;
            BurstImpacts = 5f;
            //ChanceToLiveLimit = 99f;
            PlayerLevel = 80;
            FocusMagicTargetCritRate = 0.2f;
            SnaredTime = 1f;
            FixedSegmentDuration = 30;
            WarlockSpellPower = 2800;
        }

        public CalculationOptionsMage(Character character)
            : this()
        {
            _character = character;
            character.CalculationsInvalidated += new EventHandler(Character_ItemsChanged);
        }

        string ICalculationOptionBase.GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMage));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
    }
}
