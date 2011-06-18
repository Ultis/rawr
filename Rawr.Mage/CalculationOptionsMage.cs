using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace Rawr.Mage
{
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

    public struct CooldownStackingCacheEntry
    {
        public double Effect1Duration;
        public double Effect1Cooldown;
        public double Effect2Duration;
        public double Effect2Cooldown;
        public double MaximumStackingDuration;
    }

    [GenerateSerializer]
    public sealed class CalculationOptionsMage : ICalculationOptionBase, INotifyPropertyChanged, ICharacterCalculationOptions
    {
        private int playerLevel;
        private float levelScalingFactor;
        private float spellScalingFactor;
        private float baseMana;

        public int PlayerLevel
        {
            get
            {
                return playerLevel;
            }
            set
            {
                playerLevel = value;
                if (playerLevel < 80)
                {
                    levelScalingFactor = (float)((52f / 82f) * Math.Pow(63f / 131f, (playerLevel - 70) / 10f));
                }
                else
                {
                    //levelScalingFactor = (float)((1638f / 5371f) * Math.Pow(7f / 41f, (playerLevel - 80) / 5f));                    
                    //levelScalingFactor = (float)((1638f / 5371f) * Math.Pow(0.2561350976370563f, (playerLevel - 80) / 5f));
                    switch (value)
                    {
                        case 80:
                            levelScalingFactor = 10f / 32.7899894714355f;
                            break;
                        case 81:
                            levelScalingFactor = 10f / 43.0560150146484f;
                            break;
                        case 82:
                            levelScalingFactor = 10f / 56.5397491455078f;
                            break;
                        case 83:
                            levelScalingFactor = 10f / 74.2754516601563f;
                            break;
                        case 84:
                            levelScalingFactor = 10f / 97.5272369384766f;
                            break;
                        case 85:
                            levelScalingFactor = 10f / 128.057159423828f;
                            break;
                    }
                }
                #region Spell Scaling Data
                switch (value)
                {
                    case 1:
                        spellScalingFactor = 31.0214f;
                        break;
                    case 2:
                        spellScalingFactor = 31.4306f;
                        break;
                    case 3:
                        spellScalingFactor = 31.5124f;
                        break;
                    case 4:
                        spellScalingFactor = 31.6433f;
                        break;
                    case 5:
                        spellScalingFactor = 31.7334f;
                        break;
                    case 6:
                        spellScalingFactor = 31.8217f;
                        break;
                    case 7:
                        spellScalingFactor = 31.8933f;
                        break;
                    case 8:
                        spellScalingFactor = 31.9606f;
                        break;
                    case 9:
                        spellScalingFactor = 32.5713f;
                        break;
                    case 10:
                        spellScalingFactor = 33.0673f;
                        break;
                    case 11:
                        spellScalingFactor = 33.4947f;
                        break;
                    case 12:
                        spellScalingFactor = 33.8535f;
                        break;
                    case 13:
                        spellScalingFactor = 34.1437f;
                        break;
                    case 14:
                        spellScalingFactor = 35.6445f;
                        break;
                    case 15:
                        spellScalingFactor = 37.1693f;
                        break;
                    case 16:
                        spellScalingFactor = 38.7032f;
                        break;
                    case 17:
                        spellScalingFactor = 40.2310f;
                        break;
                    case 18:
                        spellScalingFactor = 41.7376f;
                        break;
                    case 19:
                        spellScalingFactor = 43.2080f;
                        break;
                    case 20:
                        spellScalingFactor = 44.6273f;
                        break;
                    case 21:
                        spellScalingFactor = 46.8581f;
                        break;
                    case 22:
                        spellScalingFactor = 49.1244f;
                        break;
                    case 23:
                        spellScalingFactor = 51.4189f;
                        break;
                    case 24:
                        spellScalingFactor = 53.7345f;
                        break;
                    case 25:
                        spellScalingFactor = 56.0640f;
                        break;
                    case 26:
                        spellScalingFactor = 58.4003f;
                        break;
                    case 27:
                        spellScalingFactor = 60.7361f;
                        break;
                    case 28:
                        spellScalingFactor = 63.0644f;
                        break;
                    case 29:
                        spellScalingFactor = 65.3780f;
                        break;
                    case 30:
                        spellScalingFactor = 67.6698f;
                        break;
                    case 31:
                        spellScalingFactor = 71.3599f;
                        break;
                    case 32:
                        spellScalingFactor = 75.1661f;
                        break;
                    case 33:
                        spellScalingFactor = 79.0884f;
                        break;
                    case 34:
                        spellScalingFactor = 83.1267f;
                        break;
                    case 35:
                        spellScalingFactor = 87.2807f;
                        break;
                    case 36:
                        spellScalingFactor = 91.5504f;
                        break;
                    case 37:
                        spellScalingFactor = 95.9356f;
                        break;
                    case 38:
                        spellScalingFactor = 100.4360f;
                        break;
                    case 39:
                        spellScalingFactor = 105.0520f;
                        break;
                    case 40:
                        spellScalingFactor = 109.7830f;
                        break;
                    case 41:
                        spellScalingFactor = 114.6280f;
                        break;
                    case 42:
                        spellScalingFactor = 119.5890f;
                        break;
                    case 43:
                        spellScalingFactor = 124.6640f;
                        break;
                    case 44:
                        spellScalingFactor = 129.8530f;
                        break;
                    case 45:
                        spellScalingFactor = 135.1570f;
                        break;
                    case 46:
                        spellScalingFactor = 140.5750f;
                        break;
                    case 47:
                        spellScalingFactor = 146.1060f;
                        break;
                    case 48:
                        spellScalingFactor = 151.7520f;
                        break;
                    case 49:
                        spellScalingFactor = 157.5110f;
                        break;
                    case 50:
                        spellScalingFactor = 163.3840f;
                        break;
                    case 51:
                        spellScalingFactor = 169.3700f;
                        break;
                    case 52:
                        spellScalingFactor = 175.4690f;
                        break;
                    case 53:
                        spellScalingFactor = 181.6810f;
                        break;
                    case 54:
                        spellScalingFactor = 188.0050f;
                        break;
                    case 55:
                        spellScalingFactor = 194.4420f;
                        break;
                    case 56:
                        spellScalingFactor = 200.9920f;
                        break;
                    case 57:
                        spellScalingFactor = 207.6540f;
                        break;
                    case 58:
                        spellScalingFactor = 428.8540f;
                        break;
                    case 59:
                        spellScalingFactor = 485.2760f;
                        break;
                    case 60:
                        spellScalingFactor = 499.9380f;
                        break;
                    case 61:
                        spellScalingFactor = 514.8210f;
                        break;
                    case 62:
                        spellScalingFactor = 529.9250f;
                        break;
                    case 63:
                        spellScalingFactor = 545.2500f;
                        break;
                    case 64:
                        spellScalingFactor = 560.7950f;
                        break;
                    case 65:
                        spellScalingFactor = 576.5600f;
                        break;
                    case 66:
                        spellScalingFactor = 592.5450f;
                        break;
                    case 67:
                        spellScalingFactor = 608.7490f;
                        break;
                    case 68:
                        spellScalingFactor = 625.1710f;
                        break;
                    case 69:
                        spellScalingFactor = 641.8120f;
                        break;
                    case 70:
                        spellScalingFactor = 658.6700f;
                        break;
                    case 71:
                        spellScalingFactor = 675.7450f;
                        break;
                    case 72:
                        spellScalingFactor = 693.0370f;
                        break;
                    case 73:
                        spellScalingFactor = 710.5460f;
                        break;
                    case 74:
                        spellScalingFactor = 728.2700f;
                        break;
                    case 75:
                        spellScalingFactor = 746.2090f;
                        break;
                    case 76:
                        spellScalingFactor = 764.3630f;
                        break;
                    case 77:
                        spellScalingFactor = 782.7320f;
                        break;
                    case 78:
                        spellScalingFactor = 801.3140f;
                        break;
                    case 79:
                        spellScalingFactor = 820.1090f;
                        break;
                    case 80:
                        spellScalingFactor = 839.117248535156f;
                        break;
                    case 81:
                        spellScalingFactor = 858.337585449219f;
                        break;
                    case 82:
                        spellScalingFactor = 877.769592285156f;
                        break;
                    case 83:
                        spellScalingFactor = 897.412719726563f;
                        break;
                    case 84:
                        spellScalingFactor = 917.266418457031f;
                        break;
                    case 85:
                    default:
                        spellScalingFactor = 937.330017089844f;
                        break;
                }
                #endregion
                baseMana = SpellTemplate.BaseMana[playerLevel];
                OnPropertyChanged("PlayerLevel");
            }
        }

        public float LevelScalingFactor
        {
            get
            {
                return levelScalingFactor;
            }
        }

        public float SpellScalingFactor
        {
            get
            {
                return spellScalingFactor;
            }
        }

        public float BaseMana
        {
            get
            {
                return baseMana;
            }
        }

        public float GetSpellValueRound(float value)
        {
            return (float)Math.Round(spellScalingFactor * value);
        }

        public float GetSpellValue(float value)
        {
            return spellScalingFactor * value;
        }

        public const float SetBonus4T8ProcRate = 0.25f;

        private bool _ModePTR;
        public bool ModePTR
        {
            get { return _ModePTR; }
            set { _ModePTR = value; OnPropertyChanged("ModePTR"); }
        }

        private bool _BossHandler;
        public bool BossHandler
        {
            get { return _BossHandler; }
            set { _BossHandler = value; OnPropertyChanged("BossHandler"); }
        }

        private float _IgniteMunching;
        public float IgniteMunching
        {
            get { return _IgniteMunching; }
            set { _IgniteMunching = value; OnPropertyChanged("IgniteMunching"); }
        }

        private bool _UseMageWard;
        public bool UseMageWard
        {
            get { return _UseMageWard; }
            set { _UseMageWard = value; OnPropertyChanged("UseMageWard"); }
        }

        private float _FrostbiteUtilization;
        public float FrostbiteUtilization
        {
            get { return _FrostbiteUtilization; }
            set { _FrostbiteUtilization = value; OnPropertyChanged("FrostbiteUtilization"); }
        }

        private bool _MaxUseAssumption;
        public bool MaxUseAssumption
        {
            get { return _MaxUseAssumption; }
            set 
            { 
                _MaxUseAssumption = value;
                UpdateCooldownStackingCache();
                OnPropertyChanged("MaxUseAssumption"); 
            }
        }

        private int _TargetLevel;
        [XmlElement("TargetLevel")]
        public int CustomTargetLevel
        {
            get { return _TargetLevel; }
            set { _TargetLevel = value; OnPropertyChanged("CustomTargetLevel"); }
        }

        [XmlIgnore]
        public int TargetLevel
        {
            get
            {
                if (BossHandler)
                {
                    return Character.BossOptions.Level;
                }
                else
                {
                    return CustomTargetLevel;
                }
            }
        }

        private int _AoeTargetLevel;
        public int AoeTargetLevel
        {
            get { return _AoeTargetLevel; }
            set { _AoeTargetLevel = value; OnPropertyChanged("AoeTargetLevel"); }
        }

        private float _LatencyCast;
        public float LatencyCast
        {
            get { return _LatencyCast; }
            set { _LatencyCast = value; OnPropertyChanged("LatencyCast"); }
        }

        private float _LatencyGCD;
        public float LatencyGCD
        {
            get { return _LatencyGCD; }
            set { _LatencyGCD = value; OnPropertyChanged("LatencyGCD"); }
        }

        private float _LatencyChannel;
        public float LatencyChannel
        {
            get { return _LatencyChannel; }
            set { _LatencyChannel = value; OnPropertyChanged("LatencyChannel"); }
        }

        private float _MovementFrequency;
        public float MovementFrequency
        {
            get { return _MovementFrequency; }
            set { _MovementFrequency = value; OnPropertyChanged("MovementFrequency"); }
        }

        private float _MovementDuration;
        public float MovementDuration
        {
            get { return _MovementDuration; }
            set { _MovementDuration = value; OnPropertyChanged("MovementDuration"); }
        }

        private int _AoeTargets;
        public int AoeTargets
        {
            get { return _AoeTargets; }
            set { _AoeTargets = value; OnPropertyChanged("AoeTargets"); }
        }

        private float _ArcaneResist;
        [XmlElement("ArcaneResist")]
        public float CustomArcaneResist
        {
            get { return _ArcaneResist; }
            set { _ArcaneResist = value; OnPropertyChanged("CustomArcaneResist"); }
        }

        [XmlIgnore]
        public float ArcaneResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Arcane;
                }
                else
                {
                    return CustomArcaneResist;
                }
            }
        }

        private float _FireResist;
        [XmlElement("FireResist")]
        public float CustomFireResist
        {
            get { return _FireResist; }
            set { _FireResist = value; OnPropertyChanged("CustomFireResist"); }
        }

        [XmlIgnore]
        public float FireResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Fire;
                }
                else
                {
                    return CustomFireResist;
                }
            }
        }

        private float _FrostResist;
        [XmlElement("FrostResist")]
        public float CustomFrostResist
        {
            get { return _FrostResist; }
            set { _FrostResist = value; OnPropertyChanged("CustomFrostResist"); }
        }

        [XmlIgnore]
        public float FrostResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Frost;
                }
                else
                {
                    return CustomFrostResist;
                }
            }
        }

        private float _NatureResist;
        [XmlElement("NatureResist")]
        public float CustomNatureResist
        {
            get { return _NatureResist; }
            set { _NatureResist = value; OnPropertyChanged("CustomNatureResist"); }
        }

        [XmlIgnore]
        public float NatureResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Nature;
                }
                else
                {
                    return CustomNatureResist;
                }
            }
        }


        private float _ShadowResist;
        [XmlElement("ShadowResist")]
        public float CustomShadowResist
        {
            get { return _ShadowResist; }
            set { _ShadowResist = value; OnPropertyChanged("CustomShadowResist"); }
        }

        [XmlIgnore]
        public float ShadowResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Shadow;
                }
                else
                {
                    return CustomShadowResist;
                }
            }
        }

        private float _HolyResist;
        [XmlElement("HolyResist")]
        public float CustomHolyResist
        {
            get { return _HolyResist; }
            set { _HolyResist = value; OnPropertyChanged("CustomHolyResist"); }
        }

        [XmlIgnore]
        public float HolyResist
        {
            get
            {
                if (BossHandler)
                {
                    return (float)Character.BossOptions.Resist_Holy;
                }
                else
                {
                    return CustomHolyResist;
                }
            }
        }

        private float _FightDuration;
        [XmlElement("FightDuration")]
        public float CustomFightDuration
        {
            get { return _FightDuration; }
            set 
            {
                _FightDuration = value;
                UpdateCooldownStackingCache();
                OnPropertyChanged("CustomFightDuration");
            }
        }

        public float FightDuration
        {
            get
            {
                if (BossHandler && Character != null)
                {
                    return Character.BossOptions.BerserkTimer;
                }
                else
                {
                    return CustomFightDuration;
                }
            }
        }

        public float EffectiveEffectFightDuration(SpecialEffect effect)
        {
            if (effect.LimitedToExecutePhase)
            {
                return FightDuration * MoltenFuryPercentage;
            }
            return FightDuration;
        }

        private static Dictionary<float, List<CooldownStackingCacheEntry>> cooldownStackingCacheMapNoMaxUseAssumption = new Dictionary<float, List<CooldownStackingCacheEntry>>();
        private static Dictionary<float, List<CooldownStackingCacheEntry>> cooldownStackingCacheMap = new Dictionary<float, List<CooldownStackingCacheEntry>>();

        [XmlIgnore]
        public List<CooldownStackingCacheEntry> CooldownStackingCache { get; private set; }

        private void UpdateCooldownStackingCache()
        {
            var map = MaxUseAssumption ? cooldownStackingCacheMap : cooldownStackingCacheMapNoMaxUseAssumption;
            lock(map)
            {
                List<CooldownStackingCacheEntry> cache;
                map.TryGetValue(FightDuration, out cache);
                if (cache == null)
                {
                    cache = new List<CooldownStackingCacheEntry>();
                    map[FightDuration] = cache;
                }
                CooldownStackingCache = cache;
            }
        }

        private float _TpsLimit;
        public float TpsLimit
        {
            get { return _TpsLimit; }
            set { _TpsLimit = value; OnPropertyChanged("TpsLimit"); }
        }

        private bool _HeroismAvailable;
        public bool HeroismAvailable
        {
            get { return _HeroismAvailable; }
            set { _HeroismAvailable = value; OnPropertyChanged("HeroismAvailable"); }
        }

        private bool _PowerInfusionAvailable;
        public bool PowerInfusionAvailable
        {
            get { return _PowerInfusionAvailable; }
            set { _PowerInfusionAvailable = value; OnPropertyChanged("PowerInfusionAvailable"); }
        }

        private int _FlameOrb; // 0 = disabled, 1 = averaged, 2 = cooldown
        public int FlameOrb
        {
            get { return _FlameOrb; }
            set { _FlameOrb = value; OnPropertyChanged("FlameOrb"); }
        }

        [XmlIgnore]
        public string FlameOrbText
        {
            get 
            {
                switch (_FlameOrb)
                {
                    case 0:
                    default:
                        return "Disabled";
                    case 1:
                        return "Averaged";
                    case 2:
                        return "Cooldown";
                }
            }
            set 
            {
                switch (value)
                {
                    case "Disabled":
                    default:
                        _FlameOrb = 0;
                        break;
                    case "Averaged":
                        _FlameOrb = 1;
                        break;
                    case "Cooldown":
                        _FlameOrb = 2;
                        break;
                }
                OnPropertyChanged("FlameOrbText"); 
            }
        }

        private bool _VolcanicPotion;
        public bool VolcanicPotion
        {
            get { return _VolcanicPotion; }
            set { _VolcanicPotion = value; OnPropertyChanged("VolcanicPotion"); }
        }

        private bool _FlameCap;
        public bool FlameCap
        {
            get { return _FlameCap; }
            set { _FlameCap = value; OnPropertyChanged("FlameCap"); }
        }

        private bool _ArcaneLight;
        public bool ArcaneLight
        {
            get { return _ArcaneLight; }
            set { _ArcaneLight = value; OnPropertyChanged("ArcaneLight"); }
        }

        private bool _ProcCombustion;
        public bool ProcCombustion
        {
            get { return _ProcCombustion; }
            set { _ProcCombustion = value; OnPropertyChanged("ProcCombustion"); }
        }

        private float _MoltenFuryPercentage;
        [XmlElement("MoltenFuryPercentage")]
        public float CustomMoltenFuryPercentage
        {
            get { return _MoltenFuryPercentage; }
            set { _MoltenFuryPercentage = value; OnPropertyChanged("CustomMoltenFuryPercentage"); }
        }

        [XmlIgnore]
        public float MoltenFuryPercentage
        {
            get
            {
                if (BossHandler)
                {
                    return (float)(Character.BossOptions.Under20Perc + Character.BossOptions.Under35Perc);
                }
                else
                {
                    return CustomMoltenFuryPercentage;
                }
            }
        }

        private bool _MaintainScorch;
        public bool MaintainScorch
        {
            get { return _MaintainScorch; }
            set { _MaintainScorch = value; OnPropertyChanged("MaintainScorch"); }
        }

        private bool _MaintainSnare;
        public bool MaintainSnare
        {
            get { return _MaintainSnare; }
            set { _MaintainSnare = value; OnPropertyChanged("MaintainSnare"); }
        }

        private float _InterruptFrequency;
        public float InterruptFrequency
        {
            get { return _InterruptFrequency; }
            set { _InterruptFrequency = value; OnPropertyChanged("InterruptFrequency"); }
        }

        private float _AoeDuration;
        public float AoeDuration
        {
            get { return _AoeDuration; }
            set { _AoeDuration = value; OnPropertyChanged("AoeDuration"); }
        }

        private bool _SmartOptimization;
        public bool SmartOptimization
        {
            get { return _SmartOptimization; }
            set { _SmartOptimization = value; OnPropertyChanged("SmartOptimization"); }
        }

        private float _DpsTime;
        public float DpsTime
        {
            get { return _DpsTime; }
            set { _DpsTime = value; OnPropertyChanged("DpsTime"); }
        }

        private bool _DrumsOfBattle;
        public bool DrumsOfBattle
        {
            get { return _DrumsOfBattle; }
            set { _DrumsOfBattle = value; OnPropertyChanged("DrumsOfBattle"); }
        }

        private bool _AutomaticArmor;
        public bool AutomaticArmor
        {
            get { return _AutomaticArmor; }
            set { _AutomaticArmor = value; OnPropertyChanged("AutomaticArmor"); }
        }

        private bool _ForceIncrementalOptimizations;
        public bool ForceIncrementalOptimizations
        {
            get { return _ForceIncrementalOptimizations; }
            set { _ForceIncrementalOptimizations = value; OnPropertyChanged("ForceIncrementalOptimizations"); }
        }

        private bool _IncrementalOptimizations;
        public bool IncrementalOptimizations
        {
            get { return _IncrementalOptimizations; }
            set { _IncrementalOptimizations = value; OnPropertyChanged("IncrementalOptimizations"); }
        }

        private float _SnaredTime;
        public float SnaredTime
        {
            get { return _SnaredTime; }
            set { _SnaredTime = value; OnPropertyChanged("SnaredTime"); }
        }

        [XmlIgnore]
        public int[] IncrementalSetStateIndexes;
        [XmlIgnore]
        public int[] IncrementalSetSortedStates;
        [XmlIgnore]
        public int[] IncrementalSetSegments;
        [XmlIgnore]
        public CycleId[] IncrementalSetSpells;
        [XmlIgnore]
        public string IncrementalSetArmor;
        [XmlIgnore]
        public VariableType[] IncrementalSetVariableType;
        [XmlIgnore]
        public int[] IncrementalSetManaSegment;
        [XmlIgnore]
        public Rawr.Mage.SequenceReconstruction.Sequence SequenceReconstruction;
        [XmlIgnore]
        public bool AdviseAdvancedSolver;
        [XmlIgnore]
        public DisplayCalculations Calculations; // calculations that are result of the last display in rawr

        // cached cycle solutions
        //[XmlIgnore]
        //public float FrBDFFFBIL_KFrB;
        //[XmlIgnore]
        //public float FrBDFFFBIL_KFFB;
        //[XmlIgnore]
        //public float FrBDFFFBIL_KFFBS;
        //[XmlIgnore]
        //public float FrBDFFFBIL_KILS;
        //[XmlIgnore]
        //public float FrBDFFFBIL_KDFS;

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
                value.BossOptions.PropertyChanged += new PropertyChangedEventHandler(BossOptions_PropertyChanged);
                _character = value;
                if (BossHandler)
                {
                    UpdateCooldownStackingCache();
                }
            }
        }

        private void BossOptions_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (BossHandler)
            {
                if (e.PropertyName == "BerserkTimer")
                {
                    UpdateCooldownStackingCache();
                }
            }
        }

        private void Character_ItemsChanged(object sender, EventArgs e)
        {
            IncrementalSetStateIndexes = null;
            IncrementalSetSegments = null;
            IncrementalSetSortedStates = null;
            IncrementalSetSpells = null;
            IncrementalSetManaSegment = null;
            IncrementalSetVariableType = null;
            CooldownRestrictionList = null;
        }

        private bool _ReconstructSequence;
        public bool ReconstructSequence
        {
            get { return _ReconstructSequence; }
            set { _ReconstructSequence = value; OnPropertyChanged("ReconstructSequence"); }
        }

        private bool _ComparisonSegmentCooldowns;
        public bool ComparisonSegmentCooldowns
        {
            get { return _ComparisonSegmentCooldowns; }
            set { _ComparisonSegmentCooldowns = value; OnPropertyChanged("ComparisonSegmentCooldowns"); }
        }

        private bool _DisplaySegmentCooldowns;
        public bool DisplaySegmentCooldowns
        {
            get { return _DisplaySegmentCooldowns; }
            set { _DisplaySegmentCooldowns = value; OnPropertyChanged("DisplaySegmentCooldowns"); }
        }

        private bool _ComparisonIntegralMana;
        public bool ComparisonIntegralMana
        {
            get { return _ComparisonIntegralMana; }
            set { _ComparisonIntegralMana = value; OnPropertyChanged("ComparisonIntegralMana"); }
        }

        private bool _DisplayIntegralMana;
        public bool DisplayIntegralMana
        {
            get { return _DisplayIntegralMana; }
            set { _DisplayIntegralMana = value; OnPropertyChanged("DisplayIntegralMana"); }
        }

        private bool _ComparisonSegmentMana;
        public bool ComparisonSegmentMana
        {
            get { return _ComparisonSegmentMana; }
            set { _ComparisonSegmentMana = value; OnPropertyChanged("ComparisonSegmentMana"); }
        }

        private bool _DisplaySegmentMana;
        public bool DisplaySegmentMana
        {
            get { return _DisplaySegmentMana; }
            set { _DisplaySegmentMana = value; OnPropertyChanged("DisplaySegmentMana"); }
        }

        private int _ComparisonAdvancedConstraintsLevel;
        public int ComparisonAdvancedConstraintsLevel
        {
            get { return _ComparisonAdvancedConstraintsLevel; }
            set { _ComparisonAdvancedConstraintsLevel = value; OnPropertyChanged("ComparisonAdvancedConstraintsLevel"); }
        }

        private int _DisplayAdvancedConstraintsLevel;
        public int DisplayAdvancedConstraintsLevel
        {
            get { return _DisplayAdvancedConstraintsLevel; }
            set { _DisplayAdvancedConstraintsLevel = value; OnPropertyChanged("DisplayAdvancedConstraintsLevel"); }
        }

        private int _FixedSegmentDuration;
        public int FixedSegmentDuration
        {
            get { return _FixedSegmentDuration; }
            set { _FixedSegmentDuration = value; OnPropertyChanged("FixedSegmentDuration"); }
        }

        private bool _VariableSegmentDuration;
        public bool VariableSegmentDuration
        {
            get { return _VariableSegmentDuration; }
            set { _VariableSegmentDuration = value; OnPropertyChanged("VariableSegmentDuration"); }
        }

        private string _AdditionalSegmentSplits;
        public string AdditionalSegmentSplits
        {
            get { return _AdditionalSegmentSplits; }
            set { _AdditionalSegmentSplits = value; OnPropertyChanged("AdditionalSegmentSplits"); }
        }

        private double _LowerBoundHint;
        public double LowerBoundHint
        {
            get { return _LowerBoundHint; }
            set { _LowerBoundHint = value; OnPropertyChanged("LowerBoundHint"); }
        }

        private string _CooldownRestrictions;
        public string CooldownRestrictions
        {
            get { return _CooldownRestrictions; }
            set { _CooldownRestrictions = value; OnPropertyChanged("CooldownRestrictions"); }
        }

        private bool _EnableHastedEvocation;
        public bool EnableHastedEvocation
        {
            get { return _EnableHastedEvocation; }
            set { _EnableHastedEvocation = value; OnPropertyChanged("EnableHastedEvocation"); }
        }

        private bool _Enable2T10Evocation;
        public bool Enable2T10Evocation
        {
            get { return _Enable2T10Evocation; }
            set { _Enable2T10Evocation = value; OnPropertyChanged("Enable2T10Evocation"); }
        }

        private bool _AdvancedHasteProcs;
        public bool AdvancedHasteProcs
        {
            get { return _AdvancedHasteProcs; }
            set { _AdvancedHasteProcs = value; OnPropertyChanged("AdvancedHasteProcs"); }
        }

        /*private bool _EncounterEnabled;
        public bool EncounterEnabled
        {
            get { return _EncounterEnabled; }
            set { _EncounterEnabled = value; OnPropertyChanged("EncounterEnabled"); }
        }

        public Encounter Encounter { get; set; }*/

        [XmlIgnore]
        public List<CooldownRestriction> CooldownRestrictionList;
        public bool CooldownRestrictionsValid(Segment segment, CastingState state)
        {
            if (CooldownRestrictionList == null) return true;
            foreach (CooldownRestriction restriction in CooldownRestrictionList)
            {
                if (segment.TimeStart >= restriction.TimeStart && segment.TimeEnd <= restriction.TimeEnd)
                {
                    if (!restriction.IsMatch(state.Effects)) return false;
                }
            }
            return true;
        }

        public float GetDamageMultiplier(SequenceReconstruction.SequenceItem variable)
        {
            return GetDamageMultiplier(variable.Timestamp, variable.Timestamp + variable.Duration);
        }

        public float GetDamageMultiplier(Segment segment)
        {
            return GetDamageMultiplier(segment.TimeStart, segment.TimeEnd);
        }

        public float GetDamageMultiplier(double timeStart, double timeEnd)
        {
            float mult = 1.0f;
            if (BossHandler)
            {
                // we can assume that the phase boundaries coincide with segment boundaries
                // that is, a phase is composed of a number of whole segments
                // check if this segment belongs to any damage multiplier phases
                foreach (var buffState in Character.BossOptions.BuffStates)
                {
                    if (buffState.Chance > 0 && buffState.Stats.BonusDamageMultiplier > 0)
                    {
                        foreach (var phase in buffState.PhaseTimes)
                        {
                            if (timeStart >= phase.Value[0] && timeEnd <= phase.Value[1])
                            {
                                // weight by state frequency (duration is in milliseconds)
                                mult *= (1 + buffState.Stats.BonusDamageMultiplier) * buffState.Duration / buffState.Frequency / 1000.0f;
                            }
                        }
                    }
                }
            }
            return mult;
        }

        private MIPMethod _MIPMethod;
        public MIPMethod MIPMethod
        {
            get { return _MIPMethod; }
            set { _MIPMethod = value; OnPropertyChanged("MIPMethod"); }
        }

        private bool _IncludeManaNeutralCycleMix;
        public bool IncludeManaNeutralCycleMix
        {
            get { return _IncludeManaNeutralCycleMix; }
            set { _IncludeManaNeutralCycleMix = value; OnPropertyChanged("IncludeManaNeutralCycleMix"); }
        }

        private bool _DisableManaRegenCycles;
        public bool DisableManaRegenCycles
        {
            get { return _DisableManaRegenCycles; }
            set { _DisableManaRegenCycles = value; OnPropertyChanged("DisableManaRegenCycles"); }
        }

        private float _Innervate;
        public float Innervate
        {
            get { return _Innervate; }
            set { _Innervate = value; OnPropertyChanged("Innervate"); }
        }

        private float _Fragmentation;
        public float Fragmentation
        {
            get { return _Fragmentation; }
            set { _Fragmentation = value; OnPropertyChanged("Fragmentation"); }
        }

        private float _SurvivabilityRating;
        public float SurvivabilityRating
        {
            get { return _SurvivabilityRating; }
            set { _SurvivabilityRating = value; OnPropertyChanged("SurvivabilityRating"); }
        }

        private bool _PVP;
        public bool PVP
        {
            get { return _PVP; }
            set { _PVP = value; OnPropertyChanged("PVP"); }
        }

        private int _HeroismControl;
        // 0 = optimal, 1 = before 20%, 2 = no cooldowns, 3 = after 20%
        public int HeroismControl
        {
            get { return _HeroismControl; }
            set { _HeroismControl = value; OnPropertyChanged("HeroismControl"); }
        }

        [XmlIgnore]
        public string HeroismControlText
        {
            get
            {
                switch (_HeroismControl)
                {
                    case 0:
                    default:
                        return "Optimal";
                    case 1:
                        return "Before 35%";
                    case 2:
                        return "No Cooldowns";
                    case 3:
                        return "After 35%";
                }
            }
            set
            {
                switch (value)
                {
                    case "Optimal":
                    default:
                        _HeroismControl = 0;
                        break;
                    case "Before 35%":
                        _HeroismControl = 1;
                        break;
                    case "No Cooldowns":
                        _HeroismControl = 2;
                        break;
                    case "After 35%":
                        _HeroismControl = 3;
                        break;
                }
                OnPropertyChanged("HeroismControlText");
            }
        }

        private bool _AverageCooldowns;
        public bool AverageCooldowns
        {
            get { return _AverageCooldowns; }
            set { _AverageCooldowns = value; OnPropertyChanged("AverageCooldowns"); }
        }

        private bool _EvocationEnabled;
        public bool EvocationEnabled
        {
            get { return _EvocationEnabled; }
            set { _EvocationEnabled = value; OnPropertyChanged("EvocationEnabled"); }
        }

        private bool _ManaPotionEnabled;
        public bool ManaPotionEnabled
        {
            get { return _ManaPotionEnabled; }
            set { _ManaPotionEnabled = value; OnPropertyChanged("ManaPotionEnabled"); }
        }

        private bool _ManaGemEnabled;
        public bool ManaGemEnabled
        {
            get { return _ManaGemEnabled; }
            set { _ManaGemEnabled = value; OnPropertyChanged("ManaGemEnabled"); }
        }

        private int _MirrorImage; // 0 = disabled, 1 = averaged, 2 = cooldown
        public int MirrorImage
        {
            get { return _MirrorImage; }
            set { _MirrorImage = value; OnPropertyChanged("MirrorImage"); }
        }

        [XmlIgnore]
        public string MirrorImageText
        {
            get
            {
                switch (_MirrorImage)
                {
                    case 0:
                    default:
                        return "Disabled";
                    case 1:
                        return "Averaged";
                    case 2:
                        return "Cooldown";
                }
            }
            set
            {
                switch (value)
                {
                    case "Disabled":
                    default:
                        _MirrorImage = 0;
                        break;
                    case "Averaged":
                        _MirrorImage = 1;
                        break;
                    case "Cooldown":
                        _MirrorImage = 2;
                        break;
                }
                OnPropertyChanged("MirrorImageText");
            }
        }

        private bool _DisableCooldowns;
        public bool DisableCooldowns
        {
            get { return _DisableCooldowns; }
            set { _DisableCooldowns = value; OnPropertyChanged("DisableCooldowns"); }
        }

        private int _MaxHeapLimit;
        public int MaxHeapLimit
        {
            get { return _MaxHeapLimit; }
            set { _MaxHeapLimit = value; OnPropertyChanged("MaxHeapLimit"); }
        }

        private float _DrinkingTime;
        public float DrinkingTime
        {
            get { return _DrinkingTime; }
            set { _DrinkingTime = value; OnPropertyChanged("DrinkingTime"); }
        }

        private float _TargetDamage;
        public float TargetDamage
        {
            get { return _TargetDamage; }
            set { _TargetDamage = value; OnPropertyChanged("TargetDamage"); }
        }

        private bool _FarmingMode;
        public bool FarmingMode
        {
            get { return _FarmingMode; }
            set { _FarmingMode = value; OnPropertyChanged("FarmingMode"); }
        }

        private float _FocusMagicTargetCritRate;
        public float FocusMagicTargetCritRate
        {
            get { return _FocusMagicTargetCritRate; }
            set { _FocusMagicTargetCritRate = value; OnPropertyChanged("FocusMagicTargetCritRate"); }
        }

        private float _DarkIntentWarlockCritRate;
        public float DarkIntentWarlockCritRate
        {
            get { return _DarkIntentWarlockCritRate; }
            set { _DarkIntentWarlockCritRate = value; OnPropertyChanged("DarkIntentWarlockCritRate"); }
        }

        private float _PyromaniacUptime;
        public float PyromaniacUptime
        {
            get { return _PyromaniacUptime; }
            set { _PyromaniacUptime = value; OnPropertyChanged("PyromaniacUptime"); }
        }

        private bool _UnlimitedMana;
        public bool UnlimitedMana
        {
            get { return _UnlimitedMana; }
            set { _UnlimitedMana = value; OnPropertyChanged("UnlimitedMana"); }
        }

        private float _AbsorptionPerSecond;
        public float AbsorptionPerSecond
        {
            get { return _AbsorptionPerSecond; }
            set { _AbsorptionPerSecond = value; OnPropertyChanged("AbsorptionPerSecond"); }
        }


        public List<SpellWeight> CustomSpellMix { get; set; }
        private bool _CustomSpellMixEnabled;
        public bool CustomSpellMixEnabled
        {
            get { return _CustomSpellMixEnabled; }
            set { _CustomSpellMixEnabled = value; OnPropertyChanged("CustomSpellMixEnabled"); }
        }

        private int _GlobalRestarts;
        public int GlobalRestarts
        {
            get { return _GlobalRestarts; }
            set { _GlobalRestarts = value; OnPropertyChanged("GlobalRestarts"); }
        }

        private int _MaxRedecompose;
        public int MaxRedecompose
        {
            get { return _MaxRedecompose; }
            set { _MaxRedecompose = value; OnPropertyChanged("MaxRedecompose"); }
        }

        private bool _CustomSpellMixOnly;
        public bool CustomSpellMixOnly
        {
            get { return _CustomSpellMixOnly; }
            set { _CustomSpellMixOnly = value; OnPropertyChanged("CustomSpellMixOnly"); }
        }


        private float _MeleeDps;
        public float MeleeDps
        {
            get { return _MeleeDps; }
            set { _MeleeDps = value; OnPropertyChanged("MeleeDps"); }
        }

        private float _MeleeCrit;
        public float MeleeCrit
        {
            get { return _MeleeCrit; }
            set { _MeleeCrit = value; OnPropertyChanged("MeleeCrit"); }
        }

        private float _MeleeDot;
        public float MeleeDot
        {
            get { return _MeleeDot; }
            set { _MeleeDot = value; OnPropertyChanged("MeleeDot"); }
        }

        private float _PhysicalDps;
        public float PhysicalDps
        {
            get { return _PhysicalDps; }
            set { _PhysicalDps = value; OnPropertyChanged("PhysicalDps"); }
        }

        private float _PhysicalCrit;
        public float PhysicalCrit
        {
            get { return _PhysicalCrit; }
            set { _PhysicalCrit = value; OnPropertyChanged("PhysicalCrit"); }
        }

        private float _PhysicalDot;
        public float PhysicalDot
        {
            get { return _PhysicalDot; }
            set { _PhysicalDot = value; OnPropertyChanged("PhysicalDot"); }
        }

        private float _FireDps;
        public float FireDps
        {
            get { return _FireDps; }
            set { _FireDps = value; OnPropertyChanged("FireDps"); }
        }

        private float _FireCrit;
        public float FireCrit
        {
            get { return _FireCrit; }
            set { _FireCrit = value; OnPropertyChanged("FireCrit"); }
        }

        private float _FireDot;
        public float FireDot
        {
            get { return _FireDot; }
            set { _FireDot = value; OnPropertyChanged("FireDot"); }
        }

        private float _FrostDps;
        public float FrostDps
        {
            get { return _FrostDps; }
            set { _FrostDps = value; OnPropertyChanged("FrostDps"); }
        }

        private float _FrostCrit;
        public float FrostCrit
        {
            get { return _FrostCrit; }
            set { _FrostCrit = value; OnPropertyChanged("FrostCrit"); }
        }

        private float _FrostDot;
        public float FrostDot
        {
            get { return _FrostDot; }
            set { _FrostDot = value; OnPropertyChanged("FrostDot"); }
        }

        private float _ArcaneDps;
        public float ArcaneDps
        {
            get { return _ArcaneDps; }
            set { _ArcaneDps = value; OnPropertyChanged("ArcaneDps"); }
        }

        private float _ArcaneCrit;
        public float ArcaneCrit
        {
            get { return _ArcaneCrit; }
            set { _ArcaneCrit = value; OnPropertyChanged("ArcaneCrit"); }
        }

        private float _ArcaneDot;
        public float ArcaneDot
        {
            get { return _ArcaneDot; }
            set { _ArcaneDot = value; OnPropertyChanged("ArcaneDot"); }
        }

        private float _ShadowDps;
        public float ShadowDps
        {
            get { return _ShadowDps; }
            set { _ShadowDps = value; OnPropertyChanged("ShadowDps"); }
        }

        private float _ShadowCrit;
        public float ShadowCrit
        {
            get { return _ShadowCrit; }
            set { _ShadowCrit = value; OnPropertyChanged("ShadowCrit"); }
        }

        private float _ShadowDot;
        public float ShadowDot
        {
            get { return _ShadowDot; }
            set { _ShadowDot = value; OnPropertyChanged("ShadowDot"); }
        }

        private float _NatureDps;
        public float NatureDps
        {
            get { return _NatureDps; }
            set { _NatureDps = value; OnPropertyChanged("NatureDps"); }
        }

        private float _NatureCrit;
        public float NatureCrit
        {
            get { return _NatureCrit; }
            set { _NatureCrit = value; OnPropertyChanged("NatureCrit"); }
        }

        private float _NatureDot;
        public float NatureDot
        {
            get { return _NatureDot; }
            set { _NatureDot = value; OnPropertyChanged("NatureDot"); }
        }

        private float _HolyDps;
        public float HolyDps
        {
            get { return _HolyDps; }
            set { _HolyDps = value; OnPropertyChanged("HolyDps"); }
        }

        private float _HolyCrit;
        public float HolyCrit
        {
            get { return _HolyCrit; }
            set { _HolyCrit = value; OnPropertyChanged("HolyCrit"); }
        }

        private float _HolyDot;
        public float HolyDot
        {
            get { return _HolyDot; }
            set { _HolyDot = value; OnPropertyChanged("HolyDot"); }
        }


        private float _BurstWindow;
        public float BurstWindow
        {
            get { return _BurstWindow; }
            set { _BurstWindow = value; OnPropertyChanged("BurstWindow"); }
        }

        private float _BurstImpacts;
        public float BurstImpacts
        {
            get { return _BurstImpacts; }
            set { _BurstImpacts = value; OnPropertyChanged("BurstImpacts"); }
        }

        //public float ChanceToLiveLimit { get; set; }
        private float _ChanceToLiveScore;
        public float ChanceToLiveScore
        {
            get { return _ChanceToLiveScore; }
            set { _ChanceToLiveScore = value; OnPropertyChanged("ChanceToLiveScore"); }
        }

        private float _ChanceToLiveAttenuation;
        public float ChanceToLiveAttenuation
        {
            get { return _ChanceToLiveAttenuation; }
            set { _ChanceToLiveAttenuation = value; OnPropertyChanged("ChanceToLiveAttenuation"); }
        }

        private float _EffectSpiritMultiplier;
        public float EffectSpiritMultiplier
        {
            get { return _EffectSpiritMultiplier; }
            set { _EffectSpiritMultiplier = value; OnPropertyChanged("EffectSpiritMultiplier"); }
        }

        private float _EffectCritBonus;
        public float EffectCritBonus
        {
            get { return _EffectCritBonus; }
            set { _EffectCritBonus = value; OnPropertyChanged("EffectCritBonus"); }
        }

        private float _EffectCritDamageBonus;
        public float EffectCritDamageBonus
        {
            get { return _EffectCritDamageBonus; }
            set { _EffectCritDamageBonus = value; OnPropertyChanged("EffectCritDamageBonus"); }
        }

        private float _EffectDamageMultiplier;
        public float EffectDamageMultiplier
        {
            get { return _EffectDamageMultiplier; }
            set { _EffectDamageMultiplier = value; OnPropertyChanged("EffectDamageMultiplier"); }
        }

        private float _EffectHasteMultiplier;
        public float EffectHasteMultiplier
        {
            get { return _EffectHasteMultiplier; }
            set { _EffectHasteMultiplier = value; OnPropertyChanged("EffectHasteMultiplier"); }
        }

        private float _EffectCostMultiplier;
        public float EffectCostMultiplier
        {
            get { return _EffectCostMultiplier; }
            set { _EffectCostMultiplier = value; OnPropertyChanged("EffectCostMultiplier"); }
        }

        private float _EffectRegenMultiplier;
        public float EffectRegenMultiplier
        {
            get { return _EffectRegenMultiplier; }
            set { _EffectRegenMultiplier = value; OnPropertyChanged("EffectRegenMultiplier"); }
        }

        private bool _EffectDisableManaSources;
        public bool EffectDisableManaSources
        {
            get { return _EffectDisableManaSources; }
            set { _EffectDisableManaSources = value; OnPropertyChanged("EffectDisableManaSources"); }
        }

        private float _EffectShadowSilenceFrequency;
        public float EffectShadowSilenceFrequency
        {
            get { return _EffectShadowSilenceFrequency; }
            set { _EffectShadowSilenceFrequency = value; OnPropertyChanged("EffectShadowSilenceFrequency"); }
        }

        private float _EffectShadowSilenceDuration;
        public float EffectShadowSilenceDuration
        {
            get { return _EffectShadowSilenceDuration; }
            set { _EffectShadowSilenceDuration = value; OnPropertyChanged("EffectShadowSilenceDuration"); }
        }

        private float _EffectShadowManaDrainFrequency;
        public float EffectShadowManaDrainFrequency
        {
            get { return _EffectShadowManaDrainFrequency; }
            set { _EffectShadowManaDrainFrequency = value; OnPropertyChanged("EffectShadowManaDrainFrequency"); }
        }

        private float _EffectShadowManaDrain;
        public float EffectShadowManaDrain
        {
            get { return _EffectShadowManaDrain; }
            set { _EffectShadowManaDrain = value; OnPropertyChanged("EffectShadowManaDrain"); }
        }

        private float _EffectArcaneOtherBinary;
        public float EffectArcaneOtherBinary
        {
            get { return _EffectArcaneOtherBinary; }
            set { _EffectArcaneOtherBinary = value; OnPropertyChanged("EffectArcaneOtherBinary"); }
        }

        private float _EffectFireOtherBinary;
        public float EffectFireOtherBinary
        {
            get { return _EffectFireOtherBinary; }
            set { _EffectFireOtherBinary = value; OnPropertyChanged("EffectFireOtherBinary"); }
        }

        private float _EffectFrostOtherBinary;
        public float EffectFrostOtherBinary
        {
            get { return _EffectFrostOtherBinary; }
            set { _EffectFrostOtherBinary = value; OnPropertyChanged("EffectFrostOtherBinary"); }
        }

        private float _EffectShadowOtherBinary;
        public float EffectShadowOtherBinary
        {
            get { return _EffectShadowOtherBinary; }
            set { _EffectShadowOtherBinary = value; OnPropertyChanged("EffectShadowOtherBinary"); }
        }

        private float _EffectNatureOtherBinary;
        public float EffectNatureOtherBinary
        {
            get { return _EffectNatureOtherBinary; }
            set { _EffectNatureOtherBinary = value; OnPropertyChanged("EffectNatureOtherBinary"); }
        }

        private float _EffectHolyOtherBinary;
        public float EffectHolyOtherBinary
        {
            get { return _EffectHolyOtherBinary; }
            set { _EffectHolyOtherBinary = value; OnPropertyChanged("EffectHolyOtherBinary"); }
        }

        private float _EffectArcaneOther;
        public float EffectArcaneOther
        {
            get { return _EffectArcaneOther; }
            set { _EffectArcaneOther = value; OnPropertyChanged("EffectArcaneOther"); }
        }

        private float _EffectFireOther;
        public float EffectFireOther
        {
            get { return _EffectFireOther; }
            set { _EffectFireOther = value; OnPropertyChanged("EffectFireOther"); }
        }

        private float _EffectFrostOther;
        public float EffectFrostOther
        {
            get { return _EffectFrostOther; }
            set { _EffectFrostOther = value; OnPropertyChanged("EffectFrostOther"); }
        }

        private float _EffectShadowOther;
        public float EffectShadowOther
        {
            get { return _EffectShadowOther; }
            set { _EffectShadowOther = value; OnPropertyChanged("EffectShadowOther"); }
        }

        private float _EffectNatureOther;
        public float EffectNatureOther
        {
            get { return _EffectNatureOther; }
            set { _EffectNatureOther = value; OnPropertyChanged("EffectNatureOther"); }
        }

        private float _EffectHolyOther;
        public float EffectHolyOther
        {
            get { return _EffectHolyOther; }
            set { _EffectHolyOther = value; OnPropertyChanged("EffectHolyOther"); }
        }

        public CalculationOptionsMage Clone()
        {
            CalculationOptionsMage clone = (CalculationOptionsMage)MemberwiseClone();
            clone.IncrementalSetArmor = null;
            clone.IncrementalSetStateIndexes = null;
            clone.IncrementalSetSegments = null;
            clone.IncrementalSetSpells = null;
            clone.IncrementalSetVariableType = null;
            clone.IncrementalSetManaSegment = null;
            return clone;
        }

        public CalculationOptionsMage()
        {
            CustomTargetLevel = 88;
            AoeTargetLevel = 85;
            LatencyCast = 0.01f;
            LatencyGCD = 0.01f;
            LatencyChannel = 0.2f;
            AoeTargets = 9;
            CustomFightDuration = 300;
            HeroismAvailable = true;
            CustomMoltenFuryPercentage = 0.3f;
            VolcanicPotion = true;
            FlameCap = false;
            DpsTime = 1;
            InterruptFrequency = 0;
            AoeDuration = 0;
            SmartOptimization = true;
            DrumsOfBattle = false;
            AutomaticArmor = true;
            TpsLimit = 0;
            IncrementalOptimizations = true;
            ReconstructSequence = false;
            Innervate = 0;
            Fragmentation = 1;
            SurvivabilityRating = 0.0001f;
            FlameOrb = 1;
            EvocationEnabled = true;
            ManaPotionEnabled = true;
            ManaGemEnabled = true;
            MaxHeapLimit = 300;
            DrinkingTime = 300;
            BurstWindow = 5f;
            BurstImpacts = 5f;
            MirrorImage = 1;
            //ChanceToLiveLimit = 99f;
            PlayerLevel = 85;
            FocusMagicTargetCritRate = 0.2f;
            DarkIntentWarlockCritRate = 0.6f;
            SnaredTime = 1f;
            FixedSegmentDuration = 30;
            EffectSpiritMultiplier = 1.0f;
            EffectDamageMultiplier = 1.0f;
            EffectHasteMultiplier = 1.0f;
            EffectRegenMultiplier = 1.0f;
            EffectCostMultiplier = 1.0f;
            ChanceToLiveAttenuation = 0.1f;
            MaxUseAssumption = true;
            FrostbiteUtilization = 1.0f;
            Enable2T10Evocation = true;
            ComparisonAdvancedConstraintsLevel = 1;
            DisplayAdvancedConstraintsLevel = 1;
            ComparisonSegmentMana = true;
            DisplaySegmentMana = true;
            IncludeManaNeutralCycleMix = true;
            IgniteMunching = 0.08f;
            ArcaneLight = true;
            MaxRedecompose = 50;
            ProcCombustion = true;
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

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
