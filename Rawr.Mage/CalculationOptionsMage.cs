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

    [Serializable]
    public sealed class CalculationOptionsMage : ICalculationOptionBase
    {
        public int PlayerLevel { get; set; }
        public int TargetLevel { get; set; }
        public int AoeTargetLevel { get; set; }
        public float Latency { get; set; }
        public int AoeTargets { get; set; }
        public float ArcaneResist { get; set; }
        public float FireResist { get; set; }
        public float FrostResist { get; set; }
        public float NatureResist { get; set; }
        public float ShadowResist { get; set; }
        public float FightDuration { get; set; }
        public float TpsLimit { get; set; }
        public float ShadowPriest { get; set; }
        public bool HeroismAvailable { get; set; }
        public bool DestructionPotion { get; set; }
        public bool FlameCap { get; set; }
        public bool ABCycles { get; set; }
        public float MoltenFuryPercentage { get; set; }
        public bool MaintainScorch { get; set; }
        public float InterruptFrequency { get; set; }
        public bool JudgementOfWisdom { get; set; }
        public float EvocationWeapon { get; set; }
        public float EvocationSpirit { get; set; }
        public float AoeDuration { get; set; }
        public bool SmartOptimization { get; set; }
        public float DpsTime { get; set; }
        public bool DrumsOfBattle { get; set; }
        public bool AutomaticArmor { get; set; }
        public bool IncrementalOptimizations { get; set; }
        public float SlowedTime { get; set; }

        [XmlIgnore]
        public int[] IncrementalSetStateIndexes;
        [XmlIgnore]
        public int[] IncrementalSetSortedStates;
        [XmlIgnore]
        public int[] IncrementalSetSegments;
        [XmlIgnore]
        public SpellId[] IncrementalSetSpells;
        [XmlIgnore]
        public string IncrementalSetArmor;
        [XmlIgnore]
        public VariableType[] IncrementalSetVariableType;
        [XmlIgnore]
        public Rawr.Mage.SequenceReconstruction.Sequence SequenceReconstruction;
        [XmlIgnore]
        public CharacterCalculationsMage Calculations;

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
                value.ItemsChanged += new EventHandler(Character_ItemsChanged);
                _character = value;
            }
        }

        private void Character_ItemsChanged(object sender, EventArgs e)
        {
            IncrementalSetStateIndexes = null;
            IncrementalSetSegments = null;
            IncrementalSetSortedStates = null;
            IncrementalSetSpells = null;
        }

        public bool ReconstructSequence { get; set; }

        public bool ComparisonSegmentCooldowns { get; set; }
        public bool DisplaySegmentCooldowns { get; set; }
        public bool ComparisonIntegralMana { get; set; }
        public bool DisplayIntegralMana { get; set; }

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

        public int GetTalentByName(string name)
        {
            Type t = typeof(MageTalents);
            return (int)t.GetProperty(name).GetValue(Character.MageTalents, null);
        }

        public void SetTalentByName(string name, int value)
        {
            Type t = typeof(MageTalents);
            t.GetProperty(name).SetValue(Character.MageTalents, value, null);
        }

        public bool GetGlyphByName(string name)
        {
            Type t = typeof(CalculationOptionsMage);
            return (bool)t.GetProperty(name).GetValue(this, null);
        }

        public void SetGlyphByName(string name, bool value)
        {
            Type t = typeof(CalculationOptionsMage);
            t.GetProperty(name).SetValue(this, value, null);
        }

        public bool GlyphOfFireball { get; set; }
        public bool GlyphOfFrostbolt { get; set; }
        public bool GlyphOfIceArmor { get; set; }
        public bool GlyphOfImprovedScorch { get; set; }
        public bool GlyphOfMageArmor { get; set; }
        public bool GlyphOfManaGem { get; set; }
        public bool GlyphOfMoltenArmor { get; set; }
        public bool GlyphOfWaterElemental { get; set; }
        public bool GlyphOfArcaneExplosion { get; set; }

        private CalculationOptionsMage()
        {
            TargetLevel = 73;
            AoeTargetLevel = 70;
            Latency = 0.05f;
            AoeTargets = 9;
            ArcaneResist = 0;
            FireResist = 0;
            FrostResist = 0;
            NatureResist = 0;
            ShadowResist = 0;
            FightDuration = 300;
            ShadowPriest = 175;
            HeroismAvailable = true;
            MoltenFuryPercentage = 0.15f;
            DestructionPotion = true;
            FlameCap = true;
            ABCycles = true;
            DpsTime = 1;
            MaintainScorch = true;
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
            PlayerLevel = 70;
        }

        public CalculationOptionsMage(Character character)
            : this()
        {
            _character = character;
            character.ItemsChanged += new EventHandler(Character_ItemsChanged);
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
