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
        public Character Character
        {
            set
            {
                value.ItemsChanged += new EventHandler(Character_ItemsChanged);
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

        public float Innervate { get; set; }
        public float ManaTide { get; set; }
        public float Fragmentation { get; set; }
        public float SurvivabilityRating { get; set; }
        public bool Aldor { get; set; }
        public bool WotLK { get; set; }
        public int HeroismControl { get; set; }
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
            Type t = typeof(CalculationOptionsMage);
            return (int)t.GetProperty(name).GetValue(this, null);
        }

        public void SetTalentByName(string name, int value)
        {
            Type t = typeof(CalculationOptionsMage);
            t.GetProperty(name).SetValue(this, value, null);
        }

        public int Pyromaniac { get; set; }
        public int ElementalPrecision { get; set; }
        public int FrostChanneling { get; set; }
        public int MasterOfElements { get; set; }
        public int ArcaneConcentration { get; set; }
        public int MindMastery { get; set; }
        public int ArcaneInstability { get; set; }
        public int ArcanePotency { get; set; }
        public int ArcaneFocus { get; set; }
        public int PlayingWithFire { get; set; }
        public int MoltenFury { get; set; }
        public int FirePower { get; set; }
        public int PiercingIce { get; set; }
        public int SpellPower { get; set; }
        public int Ignite { get; set; }
        public int IceShards { get; set; }
        public int CriticalMass { get; set; }
        public int Combustion { get; set; }
        public int ImprovedFrostbolt { get; set; }
        public int EmpoweredFrostbolt { get; set; }
        public int ImprovedFireball { get; set; }
        public int EmpoweredFireball { get; set; }
        public int ArcaneImpact { get; set; }
        public int EmpoweredArcaneMissiles { get; set; }
        public int Incinerate { get; set; }
        public int ImprovedScorch { get; set; }
        public int WintersChill { get; set; }
        public int BurningSoul { get; set; }
        public int ImprovedArcaneMissiles { get; set; }
        public int WandSpecialization { get; set; }
        public int BlastWave { get; set; }
        public int DragonsBreath { get; set; }
        public int ArcanePower { get; set; }
        public int IcyVeins { get; set; }
        public int ColdSnap { get; set; }
        public int IceFloes { get; set; }
        public int SummonWaterElemental { get; set; }
        public int ArcaneMind { get; set; }
        public int ArcaneFortitude { get; set; }
        public int MagicAbsorption { get; set; }
        public int FrostWarding { get; set; }
        public int ArcaneMeditation { get; set; }
        public int ArcaneSubtlety { get; set; }
        public int ImprovedFireBlast { get; set; }
        public int ImprovedFlamestrike { get; set; }
        public int ImprovedFrostNova { get; set; }
        public int ImprovedConeOfCold { get; set; }
        public int ArcticWinds { get; set; }
        public int FrozenCore { get; set; }
        public int Pyroblast { get; set; }
        public int PrismaticCloak { get; set; }

        // not implemented
        public int IceBarrier { get; set; }
        public int Shatter { get; set; }
        public int ArcticReach { get; set; }
        public int ImprovedBlizzard { get; set; }
        public int Permafrost { get; set; }
        public int Frostbite { get; set; }
        public int BlazingSpeed { get; set; }
        public int ImprovedFireWard { get; set; }
        public int FlameThrowing { get; set; }
        public int Impact { get; set; }
        public int Slow { get; set; }
        public int PresenceOfMind { get; set; }
        public int ImprovedBlink { get; set; }
        public int ImprovedCounterspell { get; set; }
        public int ImprovedManaShield { get; set; }
        public int MagicAttunement { get; set; }

        // WotLK
        public int SpellImpact { get; set; }
        public int StudentOfTheMind { get; set; }
        public int IncantersAbsorption { get; set; }
        public int NetherwindPresence { get; set; }
        public int ArcaneBarrage { get; set; }
        public int MissileBarrage { get; set; }
        public int ArcaneFlows { get; set; }

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
            character.ItemsChanged += new EventHandler(Character_ItemsChanged);
            // pull talents
            #region Mage Talents Import
            try
            {
                WebRequestWrapper wrw = new WebRequestWrapper();

                if (character.Class == Character.CharacterClass.Mage && character.Name != null && character.Realm != null)
                {
                    XmlDocument docTalents = wrw.DownloadCharacterTalentTree(character.Name, character.Region, character.Realm);

                    //<talentTab>
                    //  <talentTree value="2550050300230151333125100000000000000000000002030302010000000000000"/>
                    //</talentTab>
                    if (docTalents != null)
                    {
                        string talentCode = docTalents.SelectSingleNode("page/characterInfo/talentTab/talentTree").Attributes["value"].Value;
                        ArcaneSubtlety = int.Parse(talentCode.Substring(0, 1));
                        ArcaneFocus = int.Parse(talentCode.Substring(1, 1));
                        ImprovedArcaneMissiles = int.Parse(talentCode.Substring(2, 1));
                        WandSpecialization = int.Parse(talentCode.Substring(3, 1));
                        MagicAbsorption = int.Parse(talentCode.Substring(4, 1));
                        ArcaneConcentration = int.Parse(talentCode.Substring(5, 1));
                        MagicAttunement = int.Parse(talentCode.Substring(6, 1));
                        ArcaneImpact = int.Parse(talentCode.Substring(7, 1));
                        ArcaneFortitude = int.Parse(talentCode.Substring(8, 1));
                        ImprovedManaShield = int.Parse(talentCode.Substring(9, 1));
                        ImprovedCounterspell = int.Parse(talentCode.Substring(10, 1));
                        ArcaneMeditation = int.Parse(talentCode.Substring(11, 1));
                        ImprovedBlink = int.Parse(talentCode.Substring(12, 1));
                        PresenceOfMind = int.Parse(talentCode.Substring(13, 1));
                        ArcaneMind = int.Parse(talentCode.Substring(14, 1));
                        PrismaticCloak = int.Parse(talentCode.Substring(15, 1));
                        ArcaneInstability = int.Parse(talentCode.Substring(16, 1));
                        ArcanePotency = int.Parse(talentCode.Substring(17, 1));
                        EmpoweredArcaneMissiles = int.Parse(talentCode.Substring(18, 1));
                        ArcanePower = int.Parse(talentCode.Substring(19, 1));
                        SpellPower = int.Parse(talentCode.Substring(20, 1));
                        MindMastery = int.Parse(talentCode.Substring(21, 1));
                        Slow = int.Parse(talentCode.Substring(22, 1));
                        ImprovedFireball = int.Parse(talentCode.Substring(23, 1));
                        Impact = int.Parse(talentCode.Substring(24, 1));
                        Ignite = int.Parse(talentCode.Substring(25, 1));
                        FlameThrowing = int.Parse(talentCode.Substring(26, 1));
                        ImprovedFireBlast = int.Parse(talentCode.Substring(27, 1));
                        Incinerate = int.Parse(talentCode.Substring(28, 1));
                        ImprovedFlamestrike = int.Parse(talentCode.Substring(29, 1));
                        Pyroblast = int.Parse(talentCode.Substring(30, 1));
                        BurningSoul = int.Parse(talentCode.Substring(31, 1));
                        ImprovedScorch = int.Parse(talentCode.Substring(32, 1));
                        ImprovedFireWard = int.Parse(talentCode.Substring(33, 1));
                        MasterOfElements = int.Parse(talentCode.Substring(34, 1));
                        PlayingWithFire = int.Parse(talentCode.Substring(35, 1));
                        CriticalMass = int.Parse(talentCode.Substring(36, 1));
                        BlastWave = int.Parse(talentCode.Substring(37, 1));
                        BlazingSpeed = int.Parse(talentCode.Substring(38, 1));
                        FirePower = int.Parse(talentCode.Substring(39, 1));
                        Pyromaniac = int.Parse(talentCode.Substring(40, 1));
                        Combustion = int.Parse(talentCode.Substring(41, 1));
                        MoltenFury = int.Parse(talentCode.Substring(42, 1));
                        EmpoweredFireball = int.Parse(talentCode.Substring(43, 1));
                        DragonsBreath = int.Parse(talentCode.Substring(44, 1));
                        FrostWarding = int.Parse(talentCode.Substring(45, 1));
                        ImprovedFrostbolt = int.Parse(talentCode.Substring(46, 1));
                        ElementalPrecision = int.Parse(talentCode.Substring(47, 1));
                        IceShards = int.Parse(talentCode.Substring(48, 1));
                        Frostbite = int.Parse(talentCode.Substring(49, 1));
                        ImprovedFrostNova = int.Parse(talentCode.Substring(50, 1));
                        Permafrost = int.Parse(talentCode.Substring(51, 1));
                        PiercingIce = int.Parse(talentCode.Substring(52, 1));
                        IcyVeins = int.Parse(talentCode.Substring(53, 1));
                        ImprovedBlizzard = int.Parse(talentCode.Substring(54, 1));
                        ArcticReach = int.Parse(talentCode.Substring(55, 1));
                        FrostChanneling = int.Parse(talentCode.Substring(56, 1));
                        Shatter = int.Parse(talentCode.Substring(57, 1));
                        FrozenCore = int.Parse(talentCode.Substring(58, 1));
                        ColdSnap = int.Parse(talentCode.Substring(59, 1));
                        ImprovedConeOfCold = int.Parse(talentCode.Substring(60, 1));
                        IceFloes = int.Parse(talentCode.Substring(61, 1));
                        WintersChill = int.Parse(talentCode.Substring(62, 1));
                        IceBarrier = int.Parse(talentCode.Substring(63, 1));
                        ArcticWinds = int.Parse(talentCode.Substring(64, 1));
                        EmpoweredFrostbolt = int.Parse(talentCode.Substring(65, 1));
                        SummonWaterElemental = int.Parse(talentCode.Substring(66, 1));
                    }
                }
            }
            catch (Exception)
            {
            }
            #endregion
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
