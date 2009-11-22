using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using Rawr.Mage.SequenceReconstruction;

namespace Rawr.Mage
{
    public enum VariableType
    {
        None = 0,
        IdleRegen,
        Wand,
        Evocation,
        EvocationIV,
        EvocationHero,
        EvocationIVHero,
        ManaPotion,
        ManaGem,
        Drinking,
        TimeExtension,
        AfterFightRegen,
        ManaOverflow,
        Spell,
        SummonWaterElemental,
        SummonMirrorImage,
        ConjureManaGem,
        Ward
    }

    public struct SolutionVariable
    {
        public int Segment;
        public CastingState State;
        public Cycle Cycle;
        public VariableType Type;

        public bool IsZeroTime
        {
            get
            {
                return Type == VariableType.ManaPotion || Type == VariableType.ManaGem || Type == VariableType.ManaOverflow;
            }
        }

        public bool IsMatch(int effects, VariableType cooldownType)
        {
            return ((effects != 0 && State != null && State.EffectsActive(effects) && (cooldownType == VariableType.None || Type == cooldownType)) || (effects == 0 && Type == cooldownType));
        }
    }

    public sealed class CharacterCalculationsMage : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DpsRating
        {
            get
            {
                return _subPoints[0];
            }
        }

        public float SurvivabilityRating
        {
            get
            {
                return _subPoints[1];
            }
        }

        public CalculationsMage Calculations { get; set; }
        public Stats BaseStats { get; set; }
        public Stats EvocationStats { get; set; }
        public CastingState BaseState { get; set; }
        public CalculationOptionsMage CalculationOptions { get; set; }
        public MageTalents MageTalents { get; set; }

        public Character Character { get; set; }

        //public bool WaterElemental { get; set; }
        //public float WaterElementalDps { get; set; }
        //public float WaterElementalDuration { get; set; }
        //public float WaterElementalDamage { get; set; }

        public float StartingMana { get; set; }

        public string MageArmor { get; set; }

        public bool ManaGemEffect { get; set; }
        public double EvocationDuration;
        public double EvocationRegen;
        public double EvocationDurationIV;
        public double EvocationRegenIV;
        public double EvocationDurationHero;
        public double EvocationRegenHero;
        public double EvocationDurationIVHero;
        public double EvocationRegenIVHero;
        //public double ManaPotionTime = 0.1f;
        public int MaxManaPotion;
        public int MaxManaGem;
        public double MaxEvocation;
        public double EvocationTps;
        public double ManaGemTps;
        public double ManaPotionTps;
        public double ManaGemValue;
        public double MaxManaGemValue;
        public double ManaPotionValue;
        public double MaxManaPotionValue;

        public double PowerInfusionDuration;
        public double PowerInfusionCooldown;
        public double MirrorImageDuration;
        public double MirrorImageCooldown;
        public double IcyVeinsCooldown;
        public double ColdsnapCooldown;
        public double ArcanePowerCooldown;
        public double ArcanePowerDuration;
        public double WaterElementalCooldown;
        public double WaterElementalDuration;
        public double EvocationCooldown;
        public double ManaGemEffectDuration;

        public double[] Solution;
        public List<SolutionVariable> SolutionVariable;
        public float Tps;
        public double UpperBound = double.PositiveInfinity;
        public double LowerBound = 0;
        public List<Segment> SegmentList;
        public List<EffectCooldown> CooldownList;
        public Dictionary<int, EffectCooldown> EffectCooldown;

        public List<EffectCooldown> GetEffectList(int effects)
        {
            return CooldownList.FindAll(effect => (effects & effect.Mask) == effect.Mask);
        }

        public string EffectsDescription(int effects)
        {
            List<string> buffList = new List<string>();
            List<EffectCooldown> cooldownList = CooldownList;
            for (int i = 0; i < cooldownList.Count; i++)
            {
                EffectCooldown effect = cooldownList[i];
                if ((effects & effect.Mask) == effect.Mask)
                {
                    buffList.Add(effect.Name);
                }
            }
            return string.Join("+", buffList.ToArray());
        }

        public ArraySet ArraySet { get; set; }

        public int ColumnIdleRegen = -1;
        public int ColumnWand = -1;
        public int ColumnEvocation = -1;
        public int ColumnManaGem = -1;
        public int ColumnManaPotion = -1;
        public int ColumnDrumsOfBattle = -1;
        public int ColumnDrinking = -1;
        public int ColumnTimeExtension = -1;
        public int ColumnAfterFightRegen = -1;
        public int ColumnManaOverflow = -1;
        public int ColumnSummonWaterElemental = -1;

        public Cycle ConjureManaGem { get; set; }
        public int MaxConjureManaGem { get; set; }
        public Cycle Ward { get; set; }
        public int MaxWards { get; set; }

        public Spell Wand { get; set; }

        public float ChanceToDie { get; set; }
        public float MeanIncomingDps { get; set; }

        public List<Buff> ActiveBuffs { get; set; }

        public bool NeedsDisplayCalculations { get; set; }

        public SpecialEffect[] SpellPowerEffects { get; set; }
        public SpecialEffect[] HasteRatingEffects { get; set; }
        public SpecialEffect[] DamageProcEffects { get; set; }
        public SpecialEffect[] ManaRestoreEffects { get; set; }
        public SpecialEffect[] Mp5Effects { get; set; }
        public EffectCooldown[] ItemBasedEffectCooldowns { get; set; }

        #region Base State Stats
        public float BaseSpellHit { get; set; }
        public float BaseArcaneHitRate { get; set; }
        public float BaseFireHitRate { get; set; }
        public float BaseFrostHitRate { get; set; }
        public float BaseNatureHitRate { get; set; }
        public float BaseShadowHitRate { get; set; }
        public float BaseFrostFireHitRate { get; set; }
        public float BaseHolyHitRate { get; set; }

        public float ArcaneThreatMultiplier { get; set; }
        public float FireThreatMultiplier { get; set; }
        public float FrostThreatMultiplier { get; set; }
        public float NatureThreatMultiplier { get; set; }
        public float ShadowThreatMultiplier { get; set; }
        public float FrostFireThreatMultiplier { get; set; }
        public float HolyThreatMultiplier { get; set; }

        public float BaseSpellModifier { get; set; }
        public float BaseArcaneSpellModifier { get; set; }
        public float BaseFireSpellModifier { get; set; }
        public float BaseFrostSpellModifier { get; set; }
        public float BaseNatureSpellModifier { get; set; }
        public float BaseShadowSpellModifier { get; set; }
        public float BaseFrostFireSpellModifier { get; set; }
        public float BaseHolySpellModifier { get; set; }

        public float BaseAdditiveSpellModifier { get; set; }
        public float BaseArcaneAdditiveSpellModifier { get; set; }
        public float BaseFireAdditiveSpellModifier { get; set; }
        public float BaseFrostAdditiveSpellModifier { get; set; }
        public float BaseNatureAdditiveSpellModifier { get; set; }
        public float BaseShadowAdditiveSpellModifier { get; set; }
        public float BaseFrostFireAdditiveSpellModifier { get; set; }
        public float BaseHolyAdditiveSpellModifier { get; set; }

        public float BaseCritRate { get; set; }
        public float BaseArcaneCritRate { get; set; }
        public float BaseFireCritRate { get; set; }
        public float BaseFrostCritRate { get; set; }
        public float BaseNatureCritRate { get; set; }
        public float BaseShadowCritRate { get; set; }
        public float BaseFrostFireCritRate { get; set; }
        public float BaseHolyCritRate { get; set; }

        public float BaseArcaneCritBonus { get; set; }
        public float BaseFireCritBonus { get; set; }
        public float BaseFrostCritBonus { get; set; }
        public float BaseNatureCritBonus { get; set; }
        public float BaseShadowCritBonus { get; set; }
        public float BaseFrostFireCritBonus { get; set; }
        public float BaseHolyCritBonus { get; set; }

        public float CombustionFireCritBonus { get; set; }
        public float CombustionFrostFireCritBonus { get; set; }

        public float BaseArcaneSpellPower { get; set; }
        public float BaseFireSpellPower { get; set; }
        public float BaseFrostSpellPower { get; set; }
        public float BaseNatureSpellPower { get; set; }
        public float BaseShadowSpellPower { get; set; }
        public float BaseHolySpellPower { get; set; }

        public float SpiritRegen { get; set; }
        public float ManaRegen { get; set; }
        public float ManaRegen5SR { get; set; }
        public float ManaRegenDrinking { get; set; }
        public float HealthRegen { get; set; }
        public float HealthRegenCombat { get; set; }
        public float HealthRegenEating { get; set; }
        public float MeleeMitigation { get; set; }
        public float Defense { get; set; }
        public float PhysicalCritReduction { get; set; }
        public float SpellCritReduction { get; set; }
        public float CritDamageReduction { get; set; }
        public float Dodge { get; set; }

        public float BaseCastingSpeed { get; set; }
        public float BaseGlobalCooldown { get; set; }

        public double IncomingDamageAmpMelee { get; set; }
        public double IncomingDamageAmpPhysical { get; set; }
        public double IncomingDamageAmpArcane { get; set; }
        public double IncomingDamageAmpFire { get; set; }
        public double IncomingDamageAmpFrost { get; set; }
        public double IncomingDamageAmpNature { get; set; }
        public double IncomingDamageAmpShadow { get; set; }
        public double IncomingDamageAmpHoly { get; set; }

        public double IncomingDamageDpsMelee { get; set; }
        public double IncomingDamageDpsPhysical { get; set; }
        public double IncomingDamageDpsArcane { get; set; }
        public double IncomingDamageDpsFire { get; set; }
        public double IncomingDamageDpsFrost { get; set; }
        public double IncomingDamageDpsNature { get; set; }
        public double IncomingDamageDpsShadow { get; set; }
        public double IncomingDamageDpsHoly { get; set; }

        public double IncomingDamageDps { get; set; }

        #endregion

        #region Spell Templates
        private WaterboltTemplate _WaterboltTemplate;
        public WaterboltTemplate WaterboltTemplate
        {
            get
            {
                if (_WaterboltTemplate == null)
                {
                    _WaterboltTemplate = new WaterboltTemplate(this);
                }
                return _WaterboltTemplate;
            }
        }

        private MirrorImageTemplate _MirrorImageTemplate;
        public MirrorImageTemplate MirrorImageTemplate
        {
            get
            {
                if (_MirrorImageTemplate == null)
                {
                    _MirrorImageTemplate = new MirrorImageTemplate(this);
                }
                return _MirrorImageTemplate;
            }
        }

        private FireBlastTemplate _FireBlastTemplate;
        public FireBlastTemplate FireBlastTemplate
        {
            get
            {
                if (_FireBlastTemplate == null)
                {
                    _FireBlastTemplate = new FireBlastTemplate(this);
                }
                return _FireBlastTemplate;
            }
        }

        private LightningBoltTemplate _LightningBoltTemplate;
        public LightningBoltTemplate LightningBoltTemplate
        {
            get
            {
                if (_LightningBoltTemplate == null)
                {
                    _LightningBoltTemplate = new LightningBoltTemplate(this);
                }
                return _LightningBoltTemplate;
            }
        }

        private ThunderBoltTemplate _ThunderBoltTemplate;
        public ThunderBoltTemplate ThunderBoltTemplate
        {
            get
            {
                if (_ThunderBoltTemplate == null)
                {
                    _ThunderBoltTemplate = new ThunderBoltTemplate(this);
                }
                return _ThunderBoltTemplate;
            }
        }

        private LightweaveBoltTemplate _LightweaveBoltTemplate;
        public LightweaveBoltTemplate LightweaveBoltTemplate
        {
            get
            {
                if (_LightweaveBoltTemplate == null)
                {
                    _LightweaveBoltTemplate = new LightweaveBoltTemplate(this);
                }
                return _LightweaveBoltTemplate;
            }
        }

        private ArcaneBoltTemplate _ArcaneBoltTemplate;
        public ArcaneBoltTemplate ArcaneBoltTemplate
        {
            get
            {
                if (_ArcaneBoltTemplate == null)
                {
                    _ArcaneBoltTemplate = new ArcaneBoltTemplate(this);
                }
                return _ArcaneBoltTemplate;
            }
        }

        private PendulumOfTelluricCurrentsTemplate _PendulumOfTelluricCurrentsTemplate;
        public PendulumOfTelluricCurrentsTemplate PendulumOfTelluricCurrentsTemplate
        {
            get
            {
                if (_PendulumOfTelluricCurrentsTemplate == null)
                {
                    _PendulumOfTelluricCurrentsTemplate = new PendulumOfTelluricCurrentsTemplate(this);
                }
                return _PendulumOfTelluricCurrentsTemplate;
            }
        }

        private FrostboltTemplate _FrostboltTemplate;
        public FrostboltTemplate FrostboltTemplate
        {
            get
            {
                if (_FrostboltTemplate == null)
                {
                    _FrostboltTemplate = new FrostboltTemplate(this);
                }
                return _FrostboltTemplate;
            }
        }

        private FrostfireBoltTemplate _FrostfireBoltTemplate;
        public FrostfireBoltTemplate FrostfireBoltTemplate
        {
            get
            {
                if (_FrostfireBoltTemplate == null)
                {
                    _FrostfireBoltTemplate = new FrostfireBoltTemplate(this);
                }
                return _FrostfireBoltTemplate;
            }
        }

        private ArcaneMissilesTemplate _ArcaneMissilesTemplate;
        public ArcaneMissilesTemplate ArcaneMissilesTemplate
        {
            get
            {
                if (_ArcaneMissilesTemplate == null)
                {
                    _ArcaneMissilesTemplate = new ArcaneMissilesTemplate(this);
                }
                return _ArcaneMissilesTemplate;
            }
        }

        private FireballTemplate _FireballTemplate;
        public FireballTemplate FireballTemplate
        {
            get
            {
                if (_FireballTemplate == null)
                {
                    _FireballTemplate = new FireballTemplate(this);
                }
                return _FireballTemplate;
            }
        }

        private PyroblastTemplate _PyroblastTemplate;
        public PyroblastTemplate PyroblastTemplate
        {
            get
            {
                if (_PyroblastTemplate == null)
                {
                    _PyroblastTemplate = new PyroblastTemplate(this);
                }
                return _PyroblastTemplate;
            }
        }

        private ScorchTemplate _ScorchTemplate;
        public ScorchTemplate ScorchTemplate
        {
            get
            {
                if (_ScorchTemplate == null)
                {
                    _ScorchTemplate = new ScorchTemplate(this);
                }
                return _ScorchTemplate;
            }
        }

        private ArcaneBarrageTemplate _ArcaneBarrageTemplate;
        public ArcaneBarrageTemplate ArcaneBarrageTemplate
        {
            get
            {
                if (_ArcaneBarrageTemplate == null)
                {
                    _ArcaneBarrageTemplate = new ArcaneBarrageTemplate(this);
                }
                return _ArcaneBarrageTemplate;
            }
        }

        private ArcaneBlastTemplate _ArcaneBlastTemplate;
        public ArcaneBlastTemplate ArcaneBlastTemplate
        {
            get
            {
                if (_ArcaneBlastTemplate == null)
                {
                    _ArcaneBlastTemplate = new ArcaneBlastTemplate(this);
                }
                return _ArcaneBlastTemplate;
            }
        }

        private IceLanceTemplate _IceLanceTemplate;
        public IceLanceTemplate IceLanceTemplate
        {
            get
            {
                if (_IceLanceTemplate == null)
                {
                    _IceLanceTemplate = new IceLanceTemplate(this);
                }
                return _IceLanceTemplate;
            }
        }

        private ArcaneExplosionTemplate _ArcaneExplosionTemplate;
        public ArcaneExplosionTemplate ArcaneExplosionTemplate
        {
            get
            {
                if (_ArcaneExplosionTemplate == null)
                {
                    _ArcaneExplosionTemplate = new ArcaneExplosionTemplate(this);
                }
                return _ArcaneExplosionTemplate;
            }
        }

        private FlamestrikeTemplate _FlamestrikeTemplate;
        public FlamestrikeTemplate FlamestrikeTemplate
        {
            get
            {
                if (_FlamestrikeTemplate == null)
                {
                    _FlamestrikeTemplate = new FlamestrikeTemplate(this);
                }
                return _FlamestrikeTemplate;
            }
        }

        private BlizzardTemplate _BlizzardTemplate;
        public BlizzardTemplate BlizzardTemplate
        {
            get
            {
                if (_BlizzardTemplate == null)
                {
                    _BlizzardTemplate = new BlizzardTemplate(this);
                }
                return _BlizzardTemplate;
            }
        }

        private BlastWaveTemplate _BlastWaveTemplate;
        public BlastWaveTemplate BlastWaveTemplate
        {
            get
            {
                if (_BlastWaveTemplate == null)
                {
                    _BlastWaveTemplate = new BlastWaveTemplate(this);
                }
                return _BlastWaveTemplate;
            }
        }
        
        private DragonsBreathTemplate _DragonsBreathTemplate;
        public DragonsBreathTemplate DragonsBreathTemplate
        {
            get
            {
                if (_DragonsBreathTemplate == null)
                {
                    _DragonsBreathTemplate = new DragonsBreathTemplate(this);
                }
                return _DragonsBreathTemplate;
            }
        }

        private ConeOfColdTemplate _ConeOfColdTemplate;
        public ConeOfColdTemplate ConeOfColdTemplate
        {
            get
            {
                if (_ConeOfColdTemplate == null)
                {
                    _ConeOfColdTemplate = new ConeOfColdTemplate(this);
                }
                return _ConeOfColdTemplate;
            }
        }

        private SlowTemplate _SlowTemplate;
        public SlowTemplate SlowTemplate
        {
            get
            {
                if (_SlowTemplate == null)
                {
                    _SlowTemplate = new SlowTemplate(this);
                }
                return _SlowTemplate;
            }
        }

        private LivingBombTemplate _LivingBombTemplate;
        public LivingBombTemplate LivingBombTemplate
        {
            get
            {
                if (_LivingBombTemplate == null)
                {
                    _LivingBombTemplate = new LivingBombTemplate(this);
                }
                return _LivingBombTemplate;
            }
        }

        private FireWardTemplate _FireWardTemplate;
        public FireWardTemplate FireWardTemplate
        {
            get
            {
                if (_FireWardTemplate == null)
                {
                    _FireWardTemplate = new FireWardTemplate(this);
                }
                return _FireWardTemplate;
            }
        }

        private FrostWardTemplate _FrostWardTemplate;
        public FrostWardTemplate FrostWardTemplate
        {
            get
            {
                if (_FrostWardTemplate == null)
                {
                    _FrostWardTemplate = new FrostWardTemplate(this);
                }
                return _FrostWardTemplate;
            }
        }

        private ConjureManaGemTemplate _ConjureManaGemTemplate;
        public ConjureManaGemTemplate ConjureManaGemTemplate
        {
            get
            {
                if (_ConjureManaGemTemplate == null)
                {
                    _ConjureManaGemTemplate = new ConjureManaGemTemplate(this);
                }
                return _ConjureManaGemTemplate;
            }
        }

        private ArcaneDamageTemplate _ArcaneDamageTemplate;
        public ArcaneDamageTemplate ArcaneDamageTemplate
        {
            get
            {
                if (_ArcaneDamageTemplate == null)
                {
                    _ArcaneDamageTemplate = new ArcaneDamageTemplate(this);
                }
                return _ArcaneDamageTemplate;
            }
        }

        private FireDamageTemplate _FireDamageTemplate;
        public FireDamageTemplate FireDamageTemplate
        {
            get
            {
                if (_FireDamageTemplate == null)
                {
                    _FireDamageTemplate = new FireDamageTemplate(this);
                }
                return _FireDamageTemplate;
            }
        }

        private FrostDamageTemplate _FrostDamageTemplate;
        public FrostDamageTemplate FrostDamageTemplate
        {
            get
            {
                if (_FrostDamageTemplate == null)
                {
                    _FrostDamageTemplate = new FrostDamageTemplate(this);
                }
                return _FrostDamageTemplate;
            }
        }

        private ShadowDamageTemplate _ShadowDamageTemplate;
        public ShadowDamageTemplate ShadowDamageTemplate
        {
            get
            {
                if (_ShadowDamageTemplate == null)
                {
                    _ShadowDamageTemplate = new ShadowDamageTemplate(this);
                }
                return _ShadowDamageTemplate;
            }
        }

        private NatureDamageTemplate _NatureDamageTemplate;
        public NatureDamageTemplate NatureDamageTemplate
        {
            get
            {
                if (_NatureDamageTemplate == null)
                {
                    _NatureDamageTemplate = new NatureDamageTemplate(this);
                }
                return _NatureDamageTemplate;
            }
        }

        private HolyDamageTemplate _HolyDamageTemplate;
        public HolyDamageTemplate HolyDamageTemplate
        {
            get
            {
                if (_HolyDamageTemplate == null)
                {
                    _HolyDamageTemplate = new HolyDamageTemplate(this);
                }
                return _HolyDamageTemplate;
            }
        }
        #endregion

        public string ReconstructSequence()
        {
            CalculationOptions.SequenceReconstruction = null;
            CalculationOptions.AdviseAdvancedSolver = false;
            if (!CalculationOptions.ReconstructSequence) return "*Disabled";
            if (CalculationOptions.FightDuration > 900) return "*Unavailable";

            StringBuilder timing = new StringBuilder();
            double bestUnexplained = double.PositiveInfinity;
            string bestTiming = "*";

            SequenceItem.Calculations = this;
            double unexplained;
            Sequence sequence = GenerateRawSequence(false);
            if (!sequence.SortGroups(displaySolver))
            {
                //sequence = GenerateRawSequence(true);
                //sequence.SortGroups(displaySolver);
            }
            foreach (SequenceItem item in sequence.sequence)
            {
                item.MinTime = SegmentList[item.Segment].TimeStart;
                item.MaxTime = SegmentList[item.Segment].TimeEnd - item.Duration;
            }


            // mana gem/pot/evo positioning
            if (CalculationOptions.CooldownRestrictionList == null && !CalculationOptions.EncounterEnabled)
            {
                sequence.RepositionManaConsumption();
            }

            sequence.RemoveIndex(ColumnTimeExtension);
            sequence.Compact(true);
#if !RAWR3
            if (displaySolver == null || SolverLogForm.Instance.IsSolverEnabled(displaySolver))
#endif
            {
                CalculationOptions.SequenceReconstruction = sequence;
            }

            // evaluate sequence
            unexplained = sequence.Evaluate(timing, Sequence.EvaluationMode.Unexplained);
            if (unexplained < bestUnexplained)
            {
                bestUnexplained = unexplained;
                bestTiming = timing.ToString();
            }

            if (unexplained > 0 && !(CalculationOptions.DisplaySegmentCooldowns && CalculationOptions.DisplayIntegralMana && CalculationOptions.DisplayAdvancedConstraintsLevel >= 5))
            {
                CalculationOptions.AdviseAdvancedSolver = true;
                bestTiming = "*Sequence Reconstruction was not fully successful, it is recommended that you enable more options in\r\nadvanced solver (segment cooldowns, integral mana consumables, advanced constraints options)!\r\n\r\n" + bestTiming.TrimStart('*');
            }

            return bestTiming;
        }

        private Sequence GenerateRawSequence(bool ignoreSegments)
        {
            Sequence sequence = new Sequence();

            double totalTime = 0.0;
            double totalGem = 0.0;
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (Solution[i] > 0.01 && SolutionVariable[i].Type != VariableType.ManaOverflow)
                {
                    SequenceItem item = new SequenceItem(i, Solution[i]);
                    if (ignoreSegments) item.Segment = 0;
                    sequence.Add(item);
                    if (!item.IsManaPotionOrGem) totalTime += item.Duration;
                    if (item.VariableType == VariableType.ManaGem) totalGem += Solution[i];
                }
            }
            if (CalculationOptions.TargetDamage == 0.0 && totalTime < CalculationOptions.FightDuration - 0.00001)
            {
                sequence.Add(new SequenceItem(ColumnIdleRegen, CalculationOptions.FightDuration - totalTime));
            }

            // evaluate sequence

            /*unexplained = sequence.Evaluate(timing, Sequence.EvaluationMode.Unexplained);
            if (unexplained < bestUnexplained)
            {
                bestUnexplained = unexplained;
                bestTiming = timing.ToString();
            }*/

            sequence.GroupMoltenFury();
            SequenceGroup heroismGroup = sequence.GroupHeroism();
            if (CalculationOptions.HeroismControl == 3)
            {
                heroismGroup.MinTime = CalculationOptions.FightDuration - CalculationOptions.MoltenFuryPercentage * CalculationOptions.FightDuration;
            }
            sequence.GroupCombustion();
            sequence.GroupArcanePower();
            sequence.GroupPotionOfWildMagic();
            sequence.GroupPotionOfSpeed();
            foreach (EffectCooldown cooldown in ItemBasedEffectCooldowns)
            {
                sequence.GroupSpecialEffect(cooldown);
            }
            List<SequenceGroup> list = sequence.GroupManaGemEffect();
            if (list != null && ManaGemEffect && CalculationOptions.DisplaySegmentCooldowns && ColumnManaOverflow != -1)
            {
                float manaBurn = 0;
                for (int i = 0; i < SolutionVariable.Count; i++)
                {
                    if (Solution[i] > 0.01 && SolutionVariable[i].Segment == 0 && SolutionVariable[i].Type == VariableType.Spell)
                    {
                        CastingState state = SolutionVariable[i].State;
                        if (state != null && !state.EffectsActive((int)StandardEffect.ManaGemEffect))
                        {
                            float burn = SolutionVariable[i].Cycle.ManaPerSecond;
                            if (burn > manaBurn) manaBurn = burn;
                        }
                    }
                }

                double overflow = Solution[ColumnManaOverflow];
                double tmin = 0;
                if (manaBurn > 0) tmin = (ManaGemValue * (1 + BaseStats.BonusManaGem) - overflow) / manaBurn;

                foreach (SequenceGroup g in list)
                {
                    if (g.Segment == 0) g.MinTime = tmin;
                }
            }
            sequence.GroupIcyVeins(); // should come after trinkets because of coldsnap
            sequence.GroupWaterElemental();
            sequence.GroupMirrorImage();
            sequence.GroupBerserking();
            list = sequence.GroupFlameCap();
            // very very special case for now
            if (list != null && list.Count == 2 && CalculationOptions.FightDuration < 400 && totalGem >= 1)
            {
                foreach (SequenceGroup group in list)
                {
                    foreach (CooldownConstraint constraint in group.Constraint)
                    {
                        if (constraint.EffectCooldown.StandardEffect == StandardEffect.FlameCap)
                        {
                            constraint.Cooldown = 300.0;
                        }
                    }
                }
            }
            sequence.GroupEvocation();
            return sequence;
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            if (RequiresAsynchronousDisplayCalculation)
            {
#if RAWR3
                Dictionary<string, string> ret; // = GetCharacterDisplayCalculationValuesInternal(false);
                displaySolver = new Solver(Character, CalculationOptions, CalculationOptions.DisplaySegmentCooldowns, CalculationOptions.DisplayIntegralMana, CalculationOptions.DisplayAdvancedConstraintsLevel, MageArmor, false, CalculationOptions.SmartOptimization, true, true);
                CharacterCalculationsMage smp = displaySolver.GetCharacterCalculations(null, Calculations);
                ret = smp.GetCharacterDisplayCalculationValuesInternal(true);
                ret["Dps"] = String.Format("{0:F}*{1:F}% Error margin", smp.DpsRating, Math.Abs(DpsRating - smp.DpsRating) / DpsRating * 100);
#else
                Dictionary<string, string> ret = GetCharacterDisplayCalculationValuesInternal(false);
                ret["Dps"] = "...";
                ret["Total Damage"] = "...";
                ret["Score"] = "...";
                ret["Tps"] = "...";
                ret["Spell Cycles"] = "...";
                ret["By Spell"] = "...";
                ret["Status"] = "Score: ..., Dps: ..., Survivability: ...";
                displaySolver = new Solver(Character, CalculationOptions, CalculationOptions.DisplaySegmentCooldowns, CalculationOptions.DisplayIntegralMana, CalculationOptions.DisplayAdvancedConstraintsLevel, MageArmor, false, CalculationOptions.SmartOptimization, true, true);
                SolverLogForm.Instance.EnableSolver(displaySolver);
                CalculationOptions.SequenceReconstruction = null;
#endif
                return ret;
            }
            else
            {
                return GetCharacterDisplayCalculationValuesInternal(true);
            }
        }

        public override void CancelAsynchronousCharacterDisplayCalculation()
        {
            displaySolver.CancelAsync();
        }

        public override bool RequiresAsynchronousDisplayCalculation
        {
            get
            {
                return CalculationOptions.DisplaySegmentCooldowns != CalculationOptions.ComparisonSegmentCooldowns || CalculationOptions.DisplayIntegralMana != CalculationOptions.ComparisonIntegralMana || (CalculationOptions.DisplaySegmentCooldowns == true && CalculationOptions.DisplayAdvancedConstraintsLevel != CalculationOptions.ComparisonAdvancedConstraintsLevel);
            }
        }

        private Solver displaySolver;

        public override Dictionary<string, string> GetAsynchronousCharacterDisplayCalculationValues()
        {
            CharacterCalculationsMage smp = displaySolver.GetCharacterCalculations(null, Calculations);
            smp.displaySolver = displaySolver;
            Dictionary<string, string> ret = smp.GetCharacterDisplayCalculationValuesInternal(true);
#if !RAWR3
            SolverLogForm.Instance.DisableSolver(displaySolver);
#endif
            ret["Dps"] = String.Format("{0:F}*{1:F}% Error margin", smp.DpsRating, Math.Abs(DpsRating - smp.DpsRating) / DpsRating * 100);
            return ret;
        }

        public static bool DebugCooldownSegmentation { get; set; }
        public Dictionary<string, string> DisplayCalculationValues { get; private set; }
        public Dictionary<string, SpellContribution> DamageSources { get; private set; }
        public Dictionary<string, float> ManaSources { get; private set; }
        public Dictionary<string, float> ManaUsage { get; private set; }

        internal Dictionary<string, string> GetCharacterDisplayCalculationValuesInternal(bool computeReconstruction)
        {
            Dictionary<string, string> dictValues = DisplayCalculationValues = new Dictionary<string, string>();
            dictValues.Add("Stamina", BaseStats.Stamina.ToString());
            dictValues.Add("Intellect", BaseStats.Intellect.ToString());
            dictValues.Add("Spirit", BaseStats.Spirit.ToString());
            dictValues.Add("Armor", BaseStats.Armor.ToString());
            dictValues.Add("Health", BaseStats.Health.ToString());
            dictValues.Add("Mana", BaseStats.Mana.ToString());
            dictValues.Add("Crit Rate", String.Format("{0:F}%*{1} Crit Rating", 100 * Math.Max(0, BaseState.CritRate), BaseStats.CritRating));
            float levelScalingFactor = (float)((52f / 82f) * Math.Pow(63f / 131f, (CalculationOptions.PlayerLevel - 70) / 10f));
            // hit rating = hitrate * 800 / levelScalingFactor
            dictValues.Add("Hit Rate", String.Format("{0:F}%*{1} Hit Rating\r\nArcane\t{2:F}%{3}\r\nFire\t{4:F}%{5}\r\nFrost\t{6:F}%{7}", 100 * BaseState.SpellHit, BaseStats.HitRating, 100 * BaseState.ArcaneHitRate, (BaseState.ArcaneHitRate < 1) ? (" (" + (int)Math.Ceiling((1 - BaseState.ArcaneHitRate) * 800 / levelScalingFactor) + " hit rating to cap)") : "", 100 * BaseState.FireHitRate, (BaseState.FireHitRate < 1) ? (" (" + (int)Math.Ceiling((1 - BaseState.FireHitRate) * 800 / levelScalingFactor) + " hit rating to cap)") : "", 100 * BaseState.FrostHitRate, (BaseState.FrostHitRate < 1) ? (" (" + (int)Math.Ceiling((1 - BaseState.FrostHitRate) * 800 / levelScalingFactor) + " hit rating to cap)") : ""));
            dictValues.Add("Spell Penetration", BaseStats.SpellPenetration.ToString());
            dictValues.Add("Casting Speed", String.Format("{0}*{1} Haste Rating", BaseState.CastingSpeed, BaseState.SpellHasteRating));
            dictValues.Add("Arcane Damage", BaseState.ArcaneSpellPower.ToString());
            dictValues.Add("Fire Damage", BaseState.FireSpellPower.ToString());
            dictValues.Add("Frost Damage", BaseState.FrostSpellPower.ToString());
            dictValues.Add("MP5", BaseStats.Mp5.ToString());
            dictValues.Add("Mana Regen", Math.Floor(BaseState.ManaRegen * 5).ToString() + String.Format("*Mana Regen in 5SR: {0}\r\nMana Regen Drinking: {1}", Math.Floor(BaseState.ManaRegen5SR * 5), Math.Floor(BaseState.ManaRegenDrinking * 5)));
            dictValues.Add("Health Regen", Math.Floor(BaseState.HealthRegenCombat * 5).ToString() + String.Format("*Health Regen Eating: {0}", Math.Floor(BaseState.HealthRegenEating * 5)));
            dictValues.Add("Arcane Resist", (BaseStats.ArcaneResistance).ToString());
            dictValues.Add("Fire Resist", (BaseStats.FireResistance).ToString());
            dictValues.Add("Nature Resist", (BaseStats.NatureResistance).ToString());
            dictValues.Add("Frost Resist", (BaseStats.FrostResistance).ToString());
            dictValues.Add("Shadow Resist", (BaseStats.ShadowResistance).ToString());
            dictValues.Add("Physical Mitigation", String.Format("{0:F}%", 100 * BaseState.MeleeMitigation));
            dictValues.Add("Resilience", BaseStats.Resilience.ToString());
            dictValues.Add("Defense", BaseState.Defense.ToString());
            dictValues.Add("Crit Reduction", String.Format("{0:F}%*Spell Crit Reduction: {0:F}%\r\nPhysical Crit Reduction: {1:F}%\r\nCrit Damage Reduction: {2:F}%", BaseState.SpellCritReduction * 100, BaseState.PhysicalCritReduction * 100, BaseState.CritDamageReduction * 100));
            dictValues.Add("Dodge", String.Format("{0:F}%", 100 * BaseState.Dodge));
            dictValues.Add("Chance to Die", String.Format("{0:F}%", 100 * ChanceToDie));
            dictValues.Add("Mean Incoming Dps", String.Format("{0:F}", MeanIncomingDps));
            List<CycleId> cycleList = new List<CycleId>() { CycleId.FBSc, CycleId.FBFBlast, CycleId.FBPyro, CycleId.FFBPyro, CycleId.FBScPyro, CycleId.FFBScPyro, CycleId.FBLBPyro, CycleId.FrBFB, CycleId.FrBIL, CycleId.FrBILFB, CycleId.FBScLBPyro, CycleId.ScLBPyro, CycleId.FFBLBPyro, CycleId.FFBScLBPyro, CycleId.FrBFBIL, CycleId.ABSpam04MBAM, CycleId.ABSpam024MBAM, CycleId.ABSpam0234MBAM, CycleId.AB4AM0234MBAM, CycleId.AB3AM023MBAM, CycleId.AB2AM };
            List<SpellId> spellList = new List<SpellId>() { SpellId.ArcaneMissiles, SpellId.ArcaneBarrage, SpellId.Scorch, SpellId.Fireball, SpellId.Pyroblast, SpellId.FrostboltFOF, SpellId.FireBlast, SpellId.ArcaneExplosion, SpellId.FlamestrikeSingle, SpellId.Blizzard, SpellId.BlastWave, SpellId.DragonsBreath, SpellId.ConeOfCold, SpellId.FrostfireBoltFOF, SpellId.LivingBomb, SpellId.IceLance };
            foreach (CycleId cycle in cycleList)
            {
                Cycle s = BaseState.GetCycle(cycle);
                if (s != null)
                {
                    dictValues.Add(s.Name, string.Format("{0:F} Dps*{1:F} Mps\r\n{2:F} Tps\r\nAverage Cast Time: {3:F} sec", s.DamagePerSecond, s.ManaPerSecond, s.ThreatPerSecond, s.CastTime));
                }
            }
            Spell bs;
            string spellFormatString = "{0:F} Dps*{1:F} Mps\r\n{2:F} Tps\r\n{3:F} sec\r\n{13:F} Mana\r\n{8:F} - {9:F} Hit\r\n{10:F} - {11:F} Crit{12}\r\n{4:F}x Amplify\r\n{5:F}% Crit Rate\r\n{6:F}% Hit Rate\r\n{7:F} Crit Multiplier";
            foreach (SpellId spell in spellList)
            {
                bs = BaseState.GetSpell(spell);
                if (bs != null)
                {
                    dictValues.Add(bs.Name, string.Format(spellFormatString, ((Cycle)bs).DamagePerSecond, ((Cycle)bs).ManaPerSecond, bs.ThreatPerSecond, bs.CastTime - bs.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, bs.MinHitDamage / bs.HitProcs, bs.MaxHitDamage / bs.HitProcs, bs.MinCritDamage / bs.HitProcs, bs.MaxCritDamage / bs.HitProcs, ((bs.DotDamage > 0) ? ("\n" + bs.DotDamage.ToString("F") + " Dot") : ""), bs.Cost));
                }
            }
            if (Wand != null)
            {
                dictValues.Add(Wand.Name, string.Format(spellFormatString, ((Cycle)Wand).DamagePerSecond, ((Cycle)Wand).ManaPerSecond, Wand.ThreatPerSecond, Wand.CastTime - Wand.Latency, Wand.SpellModifier, Wand.CritRate * 100, Wand.HitRate * 100, Wand.CritBonus, Wand.MinHitDamage / Wand.HitProcs, Wand.MaxHitDamage / Wand.HitProcs, Wand.MinCritDamage / Wand.HitProcs, Wand.MaxCritDamage / Wand.HitProcs, ((Wand.DotDamage > 0) ? ("\n" + Wand.DotDamage.ToString("F") + " Dot") : ""), Wand.Cost));
            }
            bs = BaseState.GetSpell(SpellId.ArcaneBlast0);
            dictValues.Add("Arcane Blast(0)", string.Format(spellFormatString, ((Cycle)bs).DamagePerSecond, ((Cycle)bs).ManaPerSecond, bs.ThreatPerSecond, bs.CastTime - bs.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, bs.MinHitDamage / bs.HitProcs, bs.MaxHitDamage / bs.HitProcs, bs.MinCritDamage / bs.HitProcs, bs.MaxCritDamage / bs.HitProcs, ((bs.DotDamage > 0) ? ("\n" + bs.DotDamage.ToString("F") + " Dot") : ""), bs.ABCost));
            bs = BaseState.GetSpell(SpellId.ArcaneBlast4);
            dictValues.Add("Arcane Blast(4)", string.Format(spellFormatString, ((Cycle)bs).DamagePerSecond, ((Cycle)bs).ManaPerSecond, bs.ThreatPerSecond, bs.CastTime - bs.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, bs.MinHitDamage / bs.HitProcs, bs.MaxHitDamage / bs.HitProcs, bs.MinCritDamage / bs.HitProcs, bs.MaxCritDamage / bs.HitProcs, ((bs.DotDamage > 0) ? ("\n" + bs.DotDamage.ToString("F") + " Dot") : ""), bs.ABCost));
            bs = BaseState.GetSpell(SpellId.ArcaneMissilesMB);
            dictValues.Add("MBAM", string.Format(spellFormatString, ((Cycle)bs).DamagePerSecond, ((Cycle)bs).ManaPerSecond, bs.ThreatPerSecond, bs.CastTime - bs.Latency, bs.SpellModifier, bs.CritRate * 100, bs.HitRate * 100, bs.CritBonus, bs.MinHitDamage / bs.HitProcs, bs.MaxHitDamage / bs.HitProcs, bs.MinCritDamage / bs.HitProcs, bs.MaxCritDamage / bs.HitProcs, ((bs.DotDamage > 0) ? ("\n" + bs.DotDamage.ToString("F") + " Dot") : ""), bs.Cost));
            AbsorbSpell abss = (AbsorbSpell)BaseState.GetSpell(SpellId.FireWard);
            dictValues.Add("Fire Ward", string.Format("{0:F} Absorb*{1:F} Mps\r\nAverage Cast Time: {2:F}\r\n{3:F} Mana", abss.Absorb, ((Cycle)abss).ManaPerSecond, abss.CastTime - abss.Latency, abss.ABCost));
            abss = (AbsorbSpell)BaseState.GetSpell(SpellId.FrostWard);
            dictValues.Add("Frost Ward", string.Format("{0:F} Absorb*{1:F} Mps\r\nAverage Cast Time: {2:F}\r\n{3:F} Mana", abss.Absorb, ((Cycle)abss).ManaPerSecond, abss.CastTime - abss.Latency, abss.ABCost));
            float totalDamage = (CalculationOptions.TargetDamage > 0.0f) ? CalculationOptions.TargetDamage : DpsRating * CalculationOptions.FightDuration;
            dictValues.Add("Total Damage", String.Format("{0:F}*Upper Bound: {1:F}\r\nLower Bound: {2:F}", totalDamage, UpperBound, LowerBound));
            dictValues.Add("Score", String.Format("{0:F}", OverallPoints));
            dictValues.Add("Dps", String.Format("{0:F}", DpsRating));
            dictValues.Add("Tps", String.Format("{0:F}", Tps));
            dictValues.Add("Status", String.Format("Score: {0:F}, Dps: {1:F}, Survivability: {2:F}", OverallPoints, DpsRating, SurvivabilityRating));
            dictValues.Add("Sequence", computeReconstruction ? ReconstructSequence() : "...");
            StringBuilder sb = new StringBuilder("*");
            if (MageArmor != null) sb.AppendLine(MageArmor);
            Dictionary<string, double> combinedSolution = new Dictionary<string, double>();
            Dictionary<string, int> combinedSolutionData = new Dictionary<string, int>();
            double idleRegen = 0;
            double evocation = 0;
            double evocationIV = 0;
            double evocationHero = 0;
            double evocationIVHero = 0;
            double manaPotion = 0;
            double manaGem = 0;
            double drums = 0;
            double we = 0;
            double mi = 0;
            double cmg = 0;
            double ward = 0;
            bool segmentedOutput = DebugCooldownSegmentation;
            DamageSources = new Dictionary<string, SpellContribution>();
            ManaSources = new Dictionary<string, float>();
            ManaUsage = new Dictionary<string, float>();
            ManaSources["Initial Mana"] = StartingMana;
            ManaSources["Replenishment"] = 0.0f;
            ManaSources["Mana Gem"] = 0.0f;
            ManaSources["Mana Potion"] = 0.0f;
            ManaSources["MP5"] = 0.0f;
            ManaSources["Intellect/Spirit"] = 0.0f;
            ManaSources["Evocation"] = 0.0f;
            ManaSources["Judgement of Wisdom"] = 0.0f;
            ManaSources["Innervate"] = 0.0f;
            ManaSources["Mana Tide"] = 0.0f;
            ManaSources["Drinking"] = 0.0f;
            ManaSources["Water Elemental"] = 0.0f;
            ManaSources["Other"] = 0.0f;
            ManaUsage["Overflow"] = 0.0f;
            ManaUsage["Summon Water Elemental"] = 0.0f;
            ManaUsage["Summon Mirror Image"] = 0.0f;
            float spiritFactor = 0.003345f;
            for (int i = 0; i < SolutionVariable.Count; i++)
            {
                if (Solution[i] > 0.01)
                {
                    switch (SolutionVariable[i].Type)
                    {
                        case VariableType.IdleRegen:
                            idleRegen += Solution[i];
                            // manaRegen = -(calculationResult.BaseState.ManaRegen * (1 - calculationOptions.Fragmentation) + calculationResult.BaseState.ManaRegen5SR * calculationOptions.Fragmentation);
                            // ManaRegen = SpiritRegen + characterStats.Mp5 / 5f + SpiritRegen * 4 * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration + characterStats.ManaRestoreFromMaxManaPerSecond * characterStats.Mana;
                            // ManaRegen5SR = SpiritRegen * characterStats.SpellCombatManaRegeneration + characterStats.Mp5 / 5f + SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration + characterStats.ManaRestoreFromMaxManaPerSecond * characterStats.Mana;
                            if (!CalculationOptions.EffectDisableManaSources)
                            {
                                ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen * (1 - CalculationOptions.Fragmentation) + BaseState.SpiritRegen * BaseStats.SpellCombatManaRegeneration * CalculationOptions.Fragmentation);
                                ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                                ManaSources["Innervate"] += (float)Solution[i] * ((15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration) * (1 - CalculationOptions.Fragmentation) + (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration) * CalculationOptions.Fragmentation);
                                ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                                ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            }
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F} sec", "Idle Regen", Solution[i], SegmentList[SolutionVariable[i].Segment]));
                            break;
                        case VariableType.Evocation:
                            evocation += Solution[i];
                            //double evoManaRegen5SR = ((0.001f + evocationStats.Spirit * spiritFactor * (float)Math.Sqrt(evocationStats.Intellect)) * evocationStats.SpellCombatManaRegeneration + evocationStats.Mp5 / 5f + calculationResult.BaseState.SpiritRegen * (5 - characterStats.SpellCombatManaRegeneration) * 20 * calculationOptions.Innervate / calculationOptions.FightDuration + calculationOptions.ManaTide * 0.24f * characterStats.Mana / calculationOptions.FightDuration);
                            //double evocationRegen = evoManaRegen5SR + 0.15f * evocationStats.Mana / 2f * calculationResult.BaseState.CastingSpeed;
                            //calculationResult.EvocationRegenIV = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2;
                            //calculationResult.EvocationRegenHero = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.3;
                            //calculationResult.EvocationRegenIVHero = evoManaRegen5SR + 0.15f * evocationMana / 2f * calculationResult.BaseState.CastingSpeed * 1.2 * 1.3;
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (0.001f + EvocationStats.Spirit * spiritFactor * (float)Math.Sqrt(EvocationStats.Intellect)) * EvocationStats.SpellCombatManaRegeneration;
                            ManaSources["MP5"] += (float)Solution[i] * EvocationStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                            ManaSources["Replenishment"] += (float)Solution[i] * EvocationStats.ManaRestoreFromMaxManaPerSecond * EvocationStats.Mana;
                            ManaSources["Evocation"] += (float)Solution[i] * 0.15f * EvocationStats.Mana / 2f * BaseState.CastingSpeed;
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Evocation", Solution[i] / EvocationDuration, SegmentList[SolutionVariable[i].Segment]));
                            break;
                        case VariableType.EvocationIV:
                            evocationIV += Solution[i];
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (0.001f + EvocationStats.Spirit * spiritFactor * (float)Math.Sqrt(EvocationStats.Intellect)) * EvocationStats.SpellCombatManaRegeneration;
                            ManaSources["MP5"] += (float)Solution[i] * EvocationStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                            ManaSources["Replenishment"] += (float)Solution[i] * EvocationStats.ManaRestoreFromMaxManaPerSecond * EvocationStats.Mana;
                            ManaSources["Evocation"] += (float)Solution[i] * 0.15f * EvocationStats.Mana / 2f * BaseState.CastingSpeed * 1.2f;
                            if (segmentedOutput)
                            {
                                if (SolutionVariable[i].State != null && SolutionVariable[i].State.EffectsActive((int)StandardEffect.IcyVeins))
                                {
                                    sb.AppendLine(String.Format("{2} {0}: {1:F}", "Icy Veins+Evocation", Solution[i], SegmentList[SolutionVariable[i].Segment]));
                                }
                                else
                                {
                                    sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Evocation (Icy Veins)", Solution[i] / EvocationDurationIV, SegmentList[SolutionVariable[i].Segment]));
                                }
                            }
                            break;
                        case VariableType.EvocationHero:
                            evocationHero += Solution[i];
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (0.001f + EvocationStats.Spirit * spiritFactor * (float)Math.Sqrt(EvocationStats.Intellect)) * EvocationStats.SpellCombatManaRegeneration;
                            ManaSources["MP5"] += (float)Solution[i] * EvocationStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                            ManaSources["Replenishment"] += (float)Solution[i] * EvocationStats.ManaRestoreFromMaxManaPerSecond * EvocationStats.Mana;
                            ManaSources["Evocation"] += (float)Solution[i] * 0.15f * EvocationStats.Mana / 2f * BaseState.CastingSpeed * 1.3f;
                            if (segmentedOutput)
                            {
                                if (SolutionVariable[i].State != null && SolutionVariable[i].State.EffectsActive((int)StandardEffect.Heroism))
                                {
                                    sb.AppendLine(String.Format("{2} {0}: {1:F}", "Heroism+Evocation", Solution[i], SegmentList[SolutionVariable[i].Segment]));
                                }
                                else
                                {
                                    sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Evocation (Heroism)", Solution[i] / EvocationDurationHero, SegmentList[SolutionVariable[i].Segment]));
                                }
                            }
                            break;
                        case VariableType.EvocationIVHero:
                            evocationIVHero += Solution[i];
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (0.001f + EvocationStats.Spirit * spiritFactor * (float)Math.Sqrt(EvocationStats.Intellect)) * EvocationStats.SpellCombatManaRegeneration;
                            ManaSources["MP5"] += (float)Solution[i] * EvocationStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                            ManaSources["Replenishment"] += (float)Solution[i] * EvocationStats.ManaRestoreFromMaxManaPerSecond * EvocationStats.Mana;
                            ManaSources["Evocation"] += (float)Solution[i] * 0.15f * EvocationStats.Mana / 2f * BaseState.CastingSpeed * 1.2f * 1.3f;
                            if (segmentedOutput)
                            {
                                if (SolutionVariable[i].State != null && SolutionVariable[i].State.EffectsActive((int)StandardEffect.IcyVeins | (int)StandardEffect.Heroism))
                                {
                                    sb.AppendLine(String.Format("{2} {0}: {1:F}", "Icy Veins+Heroism+Evocation", Solution[i], SegmentList[SolutionVariable[i].Segment]));
                                }
                                else
                                {
                                    sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Evocation (Icy Veins+Heroism)", Solution[i] / EvocationDurationIVHero, SegmentList[SolutionVariable[i].Segment]));
                                }
                            }
                            break;
                        case VariableType.ManaPotion:
                            manaPotion += Solution[i];
                            // (1 + characterStats.BonusManaPotion) * calculationResult.ManaPotionValue
                            ManaSources["Mana Potion"] += (float)(Solution[i] * (1 + BaseStats.BonusManaPotion) * ManaPotionValue);
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Mana Potion", Solution[i], SegmentList[SolutionVariable[i].Segment]));
                            break;
                        case VariableType.ManaGem:
                            manaGem += Solution[i];
                            ManaSources["Mana Gem"] += (float)(Solution[i] * (1 + BaseStats.BonusManaGem) * ManaGemValue);
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Mana Gem", Solution[i], SegmentList[SolutionVariable[i].Segment]));
                            break;
                        case VariableType.Drinking:
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen);
                            ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                            ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            if (CalculationOptions.PlayerLevel < 75)
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 240f;
                            }
                            else if (CalculationOptions.PlayerLevel < 80)
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 306f;
                            }
                            else
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 432f;
                            }
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Drinking", Solution[i]));
                            break;
                        case VariableType.TimeExtension:
                            break;
                        case VariableType.ManaOverflow:
                            ManaUsage["Overflow"] += (float)Solution[i];
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Mana Overflow", Solution[i], SegmentList[SolutionVariable[i].Segment]));
                            break;
                        case VariableType.AfterFightRegen:
                            ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen);
                            ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                            ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                            ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                            ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                            if (CalculationOptions.PlayerLevel < 75)
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 240f;
                            }
                            else if (CalculationOptions.PlayerLevel < 80)
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 306f;
                            }
                            else
                            {
                                ManaSources["Drinking"] += (float)Solution[i] * 432f;
                            }
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Drinking Regen", Solution[i]));
                            break;
                        case VariableType.SummonWaterElemental:
                            {
                                we += Solution[i];
                                ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen * BaseStats.SpellCombatManaRegeneration);
                                ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                                ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                                ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                                ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                                ManaUsage["Summon Water Elemental"] += (float)Solution[i] * (int)(0.16 * SpellTemplate.BaseMana[CalculationOptions.PlayerLevel]) / BaseGlobalCooldown;
                                if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Summon Water Elemental", Solution[i] / BaseGlobalCooldown, SegmentList[SolutionVariable[i].Segment]));
                                Spell waterbolt = WaterboltTemplate.GetSpell(SolutionVariable[i].State);
                                SpellContribution contrib;
                                if (!DamageSources.TryGetValue(waterbolt.Name, out contrib))
                                {
                                    contrib = new SpellContribution() { Name = waterbolt.Name };
                                    DamageSources[waterbolt.Name] = contrib;
                                }
                                contrib.Hits += (float)Solution[i] / waterbolt.CastTime;
                                contrib.Damage += waterbolt.DamagePerSecond * (float)Solution[i];
                            }
                            break;
                        case VariableType.SummonMirrorImage:
                            {
                                mi += Solution[i];
                                ManaSources["Intellect/Spirit"] += (float)Solution[i] * (BaseState.SpiritRegen * BaseStats.SpellCombatManaRegeneration);
                                ManaSources["MP5"] += (float)Solution[i] * BaseStats.Mp5 / 5f;
                                ManaSources["Innervate"] += (float)Solution[i] * (15732 * CalculationOptions.Innervate / CalculationOptions.FightDuration);
                                ManaSources["Mana Tide"] += (float)Solution[i] * CalculationOptions.ManaTide * 0.24f * BaseStats.Mana / CalculationOptions.FightDuration;
                                ManaSources["Replenishment"] += (float)Solution[i] * BaseStats.ManaRestoreFromMaxManaPerSecond * BaseStats.Mana;
                                ManaUsage["Summon Mirror Image"] += (float)Solution[i] * (int)(0.10 * SpellTemplate.BaseMana[CalculationOptions.PlayerLevel]) / BaseGlobalCooldown;
                                if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Summon Mirror Image", Solution[i] / BaseGlobalCooldown, SegmentList[SolutionVariable[i].Segment]));
                                Spell mirrorImage = MirrorImageTemplate.GetSpell(SolutionVariable[i].State);
                                SpellContribution contrib;
                                if (!DamageSources.TryGetValue("Mirror Image", out contrib))
                                {
                                    contrib = new SpellContribution() { Name = "Mirror Image" };
                                    DamageSources["Mirror Image"] = contrib;
                                }
                                contrib.Hits += 3 * (MageTalents.GlyphOfMirrorImage ? 4 : 3) * (float)Solution[i] / mirrorImage.CastTime;
                                contrib.Damage += mirrorImage.DamagePerSecond * (float)Solution[i];
                            }
                            break;
                        case VariableType.ConjureManaGem:
                            cmg += Solution[i];
                            Cycle smg = SolutionVariable[i].Cycle;
                            smg.AddManaUsageContribution(ManaUsage, (float)Solution[i]);
                            smg.AddManaSourcesContribution(ManaSources, (float)Solution[i]);
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", "Conjure Mana Gem", Solution[i] / ConjureManaGem.CastTime, SegmentList[SolutionVariable[i].Segment]));
                            break;
                        case VariableType.Ward:
                            ward += Solution[i];
                            Cycle sward = SolutionVariable[i].Cycle;
                            sward.AddManaUsageContribution(ManaUsage, (float)Solution[i]);
                            sward.AddManaSourcesContribution(ManaSources, (float)Solution[i]);
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F}x", Ward.Name, Solution[i] / Ward.CastTime, SegmentList[SolutionVariable[i].Segment]));
                            break;
                        case VariableType.Wand:
                        case VariableType.Spell:
                            double value;
                            Cycle s = SolutionVariable[i].Cycle;
                            s.AddDamageContribution(DamageSources, (float)Solution[i]);
                            s.AddManaUsageContribution(ManaUsage, (float)Solution[i]);
                            s.AddManaSourcesContribution(ManaSources, (float)Solution[i]);
                            string label = ((SolutionVariable[i].State.BuffLabel.Length > 0) ? (SolutionVariable[i].State.BuffLabel + "+") : "") + s.Name;
                            combinedSolution.TryGetValue(label, out value);
                            combinedSolution[label] = value + Solution[i];
                            combinedSolutionData[label] = i;
                            if (segmentedOutput) sb.AppendLine(String.Format("{2} {0}: {1:F} sec", label, Solution[i], SegmentList[SolutionVariable[i].Segment]));
                            break;
                    }
                }
            }
            if (!segmentedOutput)
            {
                if (idleRegen > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F} sec", "Idle Regen", idleRegen));
                }
                if (evocation > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x ({2:F} mps)", "Evocation", evocation / EvocationDuration, EvocationRegen));
                }
                if (evocationIV > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x ({2:F} mps)", "Evocation (Icy Veins)", evocationIV / EvocationDurationIV, EvocationRegenIV));
                }
                if (evocationHero > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x ({2:F} mps)", "Evocation (Heroism)", evocationHero / EvocationDurationHero, EvocationRegenHero));
                }
                if (evocationIVHero > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x ({2:F} mps)", "Evocation (Icy Veins+Heroism)", evocationIVHero / EvocationDurationIVHero, EvocationRegenIVHero));
                }
                if (manaPotion > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Mana Potion", manaPotion));
                }
                if (manaGem > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Mana Gem", manaGem));
                }
                if (drums > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Drums of Battle", drums / BaseGlobalCooldown));
                }
                if (we > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Summon Water Elemental", we / BaseGlobalCooldown));
                }
                if (mi > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Summon Mirror Image", mi / BaseGlobalCooldown));
                }
                if (cmg > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", "Conjure Mana Gem", cmg / ConjureManaGem.CastTime));
                }
                if (ward > 0)
                {
                    sb.AppendLine(String.Format("{0}: {1:F}x", Ward.Name, ward / Ward.CastTime));
                }
                foreach (KeyValuePair<string, double> kvp in combinedSolution)
                {
                    Cycle s = SolutionVariable[combinedSolutionData[kvp.Key]].Cycle;
                    if (s != null)
                    {
                        sb.AppendLine(String.Format("{0}: {1:F} sec ({2:F} dps, {3:F} mps, {4:F} tps)", kvp.Key, kvp.Value, s.DamagePerSecond, s.ManaPerSecond, s.ThreatPerSecond));
                    }
                    else
                    {
                        sb.AppendLine(String.Format("{0}: {1:F} sec", kvp.Key, kvp.Value));
                    }
                }
            }
            //if (WaterElemental) sb.AppendLine(String.Format("Water Elemental: {0:F}x", WaterElementalDuration / 45f));
            dictValues.Add("Spell Cycles", sb.ToString());
            sb = new StringBuilder("*");
            List<SpellContribution> contribList = new List<SpellContribution>(DamageSources.Values);
            contribList.Sort();
            foreach (SpellContribution contrib in contribList)
            {
                sb.AppendFormat("{0}: {1:F}%, {2:F} Damage, {3:F} Hits\r\n", contrib.Name, 100.0 * contrib.Damage / totalDamage, contrib.Damage, contrib.Hits);
            }
            dictValues.Add("By Spell", sb.ToString());
            dictValues.Add("Minimum Range", String.Format("{0:F}", MinimumRange));
            dictValues.Add("Threat Reduction", String.Format("{0:F}%", ThreatReduction * 100));
            CalculationOptions.Calculations = this;
            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BaseStats.Health;
                case "Nature Resistance": return BaseStats.NatureResistance;
                case "Fire Resistance": return BaseStats.FireResistance;
                case "Frost Resistance": return BaseStats.FrostResistance;
                case "Shadow Resistance": return BaseStats.ShadowResistance;
                case "Arcane Resistance": return BaseStats.ArcaneResistance;
                case "Chance to Live": return 100 * (1 - ChanceToDie);
                case "Hit Rating": return BaseStats.HitRating;
                case "Haste Rating": return BaseStats.HasteRating;
                case "PVP Trinket": return BaseStats.PVPTrinket;
                case "Movement Speed": return BaseStats.MovementSpeed * 100f;
                case "Minimum Range": return MinimumRange;
                case "Threat Reduction": return ThreatReduction;
                case "Arcane Nondps Talents": return ArcaneNondpsTalents;
                case "Fire Nondps Talents": return FireNondpsTalents;
                case "Frost Nondps Talents": return FrostNondpsTalents;
                case "Partially Modeled Talents": return PartiallyModeledTalents;
                case "Talent Score": return TalentScore;
            }
            return 0;
        }

        public float ArcaneNondpsTalents
        {
            get
            {
                return MageTalents.ArcaneSubtlety + MageTalents.ArcaneFortitude + MageTalents.MagicAbsorption + MageTalents.MagicAttunement + MageTalents.ArcaneShielding + MageTalents.ImprovedCounterspell + MageTalents.ImprovedBlink + MageTalents.PresenceOfMind + MageTalents.PrismaticCloak + MageTalents.IncantersAbsorption + MageTalents.Slow;
            }
        }

        public float FireNondpsTalents
        {
            get
            {
                return MageTalents.ImprovedFireBlast + MageTalents.BurningDetermination + MageTalents.FlameThrowing + MageTalents.Impact + MageTalents.BurningSoul + MageTalents.MoltenShields + MageTalents.BlastWave + MageTalents.BlazingSpeed + MageTalents.FieryPayback + MageTalents.DragonsBreath + MageTalents.Firestarter;
            }
        }

        public float FrostNondpsTalents
        {
            get
            {
                return MageTalents.Frostbite + MageTalents.FrostWarding + MageTalents.Permafrost + MageTalents.ImprovedBlizzard + MageTalents.ArcticReach + MageTalents.FrozenCore + MageTalents.ImprovedConeOfCold + MageTalents.IceBarrier + MageTalents.ShatteredBarrier + MageTalents.DeepFreeze;
            }
        }

        public float PartiallyModeledTalents
        {
            get
            {
                return MageTalents.ArcaneSubtlety + MageTalents.MagicAttunement + MageTalents.ArcaneShielding + MageTalents.ImprovedCounterspell + MageTalents.ImprovedBlink + MageTalents.PresenceOfMind + MageTalents.IncantersAbsorption + MageTalents.Slow + MageTalents.ImprovedFireBlast + MageTalents.BurningDetermination + MageTalents.FlameThrowing + MageTalents.Impact + MageTalents.BurningSoul + MageTalents.MoltenShields + MageTalents.BlastWave + MageTalents.BlazingSpeed + MageTalents.FieryPayback + MageTalents.DragonsBreath + MageTalents.Firestarter + MageTalents.Frostbite + MageTalents.FrostWarding + MageTalents.Permafrost + MageTalents.ImprovedBlizzard + MageTalents.ArcticReach + MageTalents.ImprovedConeOfCold + MageTalents.IceBarrier + MageTalents.ShatteredBarrier + MageTalents.DeepFreeze;
            }
        }

        public float TalentScore
        {
            get
            {
                if (CalculationOptions.TalentScore == null || CalculationOptions.TalentScore.Length != MageTalents.Data.Length)
                {
                    return 0.0f;
                }
                float score = 0f;
                for (int i = 0; i < MageTalents.Data.Length; i++)
                {
                    score += MageTalents.Data[i] * CalculationOptions.TalentScore[i];
                }
                return score;
            }
        }

        public float MinimumRange
        {
            get
            {
                float minRange = float.PositiveInfinity;
                foreach (SpellContribution contrib in DamageSources.Values)
                {
                    if (contrib.Range < minRange)
                    {
                        minRange = contrib.Range;
                    }
                }
                return minRange;
            }
        }

        public float ThreatReduction
        {
            get
            {
                return 1 - Tps / DpsRating;
            }
        }
    }
}
