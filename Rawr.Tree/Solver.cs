using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class RotationResult
    {
        public RotationResult(CharacterCalculationsTree calculatedStats, Stats stats, float latency)
        {
            #region Setup Spells
            regrowth = new Regrowth(calculatedStats, stats, false);
            regrowthAgain = new Regrowth(calculatedStats, stats, true);
            lifebloom = new Lifebloom(calculatedStats, stats);
            lifebloomSlowStack = new Lifebloom(calculatedStats, stats, 3, false);
            lifebloomFastStack = new Lifebloom(calculatedStats, stats, 3, true);
            lifebloomRollingStack = new LifebloomStack(calculatedStats, stats);

            rejuvenate = new Rejuvenation(calculatedStats, stats);
            nourish = new Nourish[5];
            nourish[0] = new Nourish(calculatedStats, stats, 0);
            nourish[1] = new Nourish(calculatedStats, stats, 1);
            nourish[2] = new Nourish(calculatedStats, stats, 2);
            nourish[3] = new Nourish(calculatedStats, stats, 3);
            nourish[4] = new Nourish(calculatedStats, stats, 4);
            healingTouch = new HealingTouch(calculatedStats, stats);
            wildGrowth = new WildGrowth(calculatedStats, stats);
            #endregion

            #region Add latency
            regrowth.latency = latency;
            regrowthAgain.latency = latency;
            lifebloom.latency = latency;
            lifebloomSlowStack.latency = latency;
            lifebloomFastStack.latency = latency;
            lifebloomRollingStack.latency = latency;
            rejuvenate.latency = latency;
            nourish[0].latency = latency;
            nourish[1].latency = latency;
            nourish[2].latency = latency;
            nourish[3].latency = latency;
            nourish[4].latency = latency;
            healingTouch.latency = latency;
            wildGrowth.latency = latency;
            #endregion

            #region Setup Mana regen
            SpiritMPS = StatConversion.GetSpiritRegenSec(stats.Intellect, stats.Spirit);
            replenishment = stats.ManaRestoreFromMaxManaPerSecond; 
            Mana = stats.Mana;
            GearMPS = stats.Mp5 / 5f;
            SpiritInCombatFraction = stats.SpellCombatManaRegeneration;
            ProcsMPS = stats.ManaRestore;
            #endregion

            RevitalizeChance = stats.RevitalizeChance;
        }

        public void ApplyCombatStats(Stats stats) 
        {
            // to be done
        }

        #region Spells
        public Rejuvenation rejuvenate;
        public Regrowth regrowth;
        public Regrowth regrowthAgain;
        public Lifebloom lifebloom;
        public Lifebloom lifebloomSlowStack;
        public Lifebloom lifebloomFastStack;
        public LifebloomStack lifebloomRollingStack;

        public Nourish[] nourish;
        public HealingTouch healingTouch;
        public WildGrowth wildGrowth;
        public Swiftmend swiftmend = null;
        #endregion

        public float HPS { get { return HotsHPS + WildGrowthHPS + SwiftmendHPS + NourishHPS + ValAnyrHPS; } }
        public float MPS { get { return HotsMPS + WildGrowthMPS + SwiftmendMPS + NourishMPS; } }
        public float CastsPerMinute { get { return HotsCastsPerMinute + WildGrowthCPM + SwiftmendCPM + NourishCPM; } }
        public float HealsPerMinute { get { return HotsHealsPerMinute + WildGrowthHealsPerMinute + SwiftmendHealsPerMinute + NourishHealsPerMinute; } }
        public float CritsPerMinute { get { return HotsCritsPerMinute + SwiftmendCritsPerMinute + NourishCritsPerMinute; } }

        #region Hots
        public float HotsCF { get { return RejuvCF + RegrowthCF + LifebloomCF + LifebloomStackCF; } }
        public float HotsHPS { get { return RejuvHPS + RegrowthHPS + LifebloomHPS + LifebloomStackHPS; } }
        public float HotsMPS { get { return RejuvMPS + RegrowthMPS + LifebloomMPS + LifebloomStackMPS; } }
        public float HotsCastsPerMinute { get { return RejuvCPM + RegrowthCPM + LifebloomCPM + LifebloomStackCPM; } }
        public float HotsHealsPerMinute { get { return RejuvHealsPerMinute + RegrowthHealsPerMinute + LifebloomHealsPerMinute + LifebloomStackHealsPerMinute; } }
        public float HotsCritsPerMinute { get { return RegrowthCritsPerMinute + LifebloomCritsPerMinute + LifebloomStackCritsPerMinute; } }
        #endregion

        #region Rejuvenation
        public float RejuvCF = 0f;
        public float RejuvCPS { get { return RejuvCF / rejuvenate.CastTime; } }
        public float RejuvCPM { get { return 60f * RejuvCF / rejuvenate.CastTime; } }
        public float RejuvAvg { get { return RejuvCPS * rejuvenate.Duration; } }
        //public float RejuvHPS { get { return RejuvAvg * rejuvenate.HPSHoT + RejuvCPS * rejuvenate.AverageHealingwithCrit; } }
        public float RejuvHPS { get { return RejuvCF * rejuvenate.HPCT; } }
        public float RejuvMPS { get { return RejuvCPS * rejuvenate.ManaCost; } }
        public float RejuvHealsPerMinute { get { return RejuvCPM * rejuvenate.PeriodicTicks; } }
        #endregion

        #region Regrowth
        public float RegrowthCF = 0f;
        public float RegrowthCPS { get { return RegrowthCF / regrowth.CastTime; } }
        public float RegrowthCPM { get { return 60f * RegrowthCF / regrowth.CastTime; } }
        public float RegrowthAvg { get { return RegrowthCPS * regrowth.Duration; } }
        //public float RegrowthHPS { get { return RegrowthAvg * regrowth.HPSHoT + RegrowthCPS * regrowth.AverageHealingwithCrit; } }
        public float RegrowthHPS { get { return RegrowthCF * regrowth.HPCT; } }
        public float RegrowthMPS { get { return RegrowthCPS * regrowth.ManaCost; } }
        public float RegrowthHealsPerMinute { get { return RegrowthCPM * (1f + regrowth.PeriodicTicks); } }
        public float RegrowthCritsPerMinute { get { return RegrowthCPM * regrowth.CritPercent / 100f; } }
        #endregion

        #region Lifebloom
        public float LifebloomCF = 0f;
        public float LifebloomCPS { get { return LifebloomCF / lifebloom.CastTime; } }
        public float LifebloomCPM { get { return 60f * LifebloomCF / lifebloom.CastTime; } }
        public float LifebloomAvg { get { return LifebloomCPS * lifebloom.Duration; } }
        //public float LifebloomHPS { get { return LifebloomAvg * lifebloom.HPSHoT + LifebloomCPS * lifebloom.AverageHealingwithCrit; } }
        public float LifebloomHPS { get { return LifebloomCF * lifebloom.HPCT; } }
        public float LifebloomMPS { get { return LifebloomCPS * lifebloom.ManaCost; } }
        public float LifebloomHealsPerMinute { get { return LifebloomCPM * (1f + lifebloom.PeriodicTicks); } }
        public float LifebloomCritsPerMinute { get { return LifebloomCPM * lifebloom.CritPercent / 100f; } }
        #endregion

        #region Nourish
        public float NourishCF = 0f;
        public float NourishRawHPS = 0f;
        public float NourishHPS { get { return NourishCF * NourishRawHPS; } }
        public float NourishMPS { get { return NourishCPS * nourish[0].ManaCost; } }
        public float NourishCPS { get { return NourishCF / nourish[0].CastTime; } }
        public float NourishCPM { get { return 60f * NourishCF / nourish[0].CastTime; } }
        public float NourishHealsPerMinute { get { return NourishCPM; } }
        public float NourishCritsPerMinute { get { return NourishHealsPerMinute * nourish[0].CritPercent / 100f; } }
        #endregion

        #region Lifebloom stacks
        private LifeBloomType lifebloomStackType;
        public LifeBloomType LifebloomStackType { get { return lifebloomStackType; } 
            set {
                lifebloomStackType = value;
                switch (value)
                {
                    case LifeBloomType.Slow:
                        lifebloomStackSpell = lifebloomSlowStack;
                        lifebloomStackDuration = lifebloomStackSpell.Duration + 1f; // need a second before reapplying
                        break;
                    case LifeBloomType.Fast:
                        lifebloomStackSpell = lifebloomFastStack;
                        lifebloomStackDuration = lifebloomStackSpell.Duration + 1f; // need a second before reapplying
                        break;
                    default:
                        lifebloomStackSpell = lifebloomRollingStack;
                        lifebloomStackDuration = lifebloomStackSpell.Duration;
                        break;
                }
                lifebloomStackCF = -1f;
            }
        }
        private Spell lifebloomStackSpell = null;
        private float lifebloomStackDuration = 0;

        private float lifebloomStacks;
        public float LifebloomStacks { get { return lifebloomStacks; } set { lifebloomStacks = value; lifebloomStackCF = -1f; } }

        private float lifebloomStackCF = -1f;
        public float LifebloomStackCF
        {
            get
            {
                if (LifebloomStacks <= 0) return 0;
                if (lifebloomStackCF != -1f) return lifebloomStackCF;
                return lifebloomStackSpell.CastTime * LifebloomStacks / lifebloomStackDuration;
            }
            set { lifebloomStackCF = value; }
        }
        public float LifebloomStackCPS { get { return LifebloomStackCF <= 0 ? 0 : 
            lifebloomStackSpell.NumberOfCasts * LifebloomStackCF / lifebloomStackSpell.CastTime; } }
        public float LifebloomStackCPM { get { return LifebloomStackCF <= 0 ? 0 : 
            60f * LifebloomStackCPS; } }
        public float LifebloomStackAvg { get { return LifebloomStackCF <= 0 ? 0 : 
            LifebloomStackCPS / lifebloomStackSpell.NumberOfCasts * lifebloomStackSpell.Duration; } }
        public float LifebloomStackBPS { get { return LifebloomStackCF <= 0 ? 0 :
            lifebloomStackType == LifeBloomType.Rolling ? 0 : LifebloomStackCPS / lifebloomStackSpell.NumberOfCasts; } }
        public float LifebloomStackHPS { get { return LifebloomStackCF <= 0 ? 0 :
            LifebloomStackAvg * lifebloomStackSpell.HPSHoT + LifebloomStackBPS * lifebloomStackSpell.AverageHealingwithCrit; } }
        public float LifebloomStackMPS { get { return LifebloomStackCF <= 0 ? 0 : 
            LifebloomStackCPS * lifebloom.ManaCost; } }
        public float LifebloomStackHealsPerMinute { get { return LifebloomStackCF <= 0 ? 0 : 
            60f * LifebloomStackBPS * (1f + lifebloomStackSpell.PeriodicTicks); } }
        public float LifebloomStackCritsPerMinute { get { return LifebloomStackCF <= 0 || lifebloomStackType == LifeBloomType.Rolling ? 0 :
            60f * LifebloomStackBPS * lifebloomStackSpell.CritPercent / 100f; } }
        public float LifebloomStackHPS_DH { get { return LifebloomStackCF <= 0 ? 0 : 
            LifebloomStackBPS * lifebloomStackSpell.AverageHealingwithCrit; } }
        public float LifebloomStackHPS_HOT { get { return LifebloomStackCF <= 0 ? 0 : 
            LifebloomStackAvg * lifebloomStackSpell.HPSHoT; } }
        #endregion

        #region Wild Growth
        public float WildGrowthCF = 0f;
        public float WildGrowthCPS { get { return WildGrowthCF / wildGrowth.CastTime; } }
        public float WildGrowthCPM { get { return 60f * WildGrowthCPS; } set { WildGrowthCF = value * (float)wildGrowth.CastTime / 60f; } }
        public float WildGrowthAvg { get { return WildGrowthCPS * wildGrowth.Duration; } }
        public float WildGrowthHPS { get { return WildGrowthAvg * wildGrowth.maxTargets * wildGrowth.PeriodicTick; } }
        public float WildGrowthMPS { get { return WildGrowthCPS * wildGrowth.ManaCost; } }
        public float WildGrowthHealsPerMinute { get { return WildGrowthCPM * wildGrowth.maxTargets * wildGrowth.PeriodicTicks; } }
        #endregion

        #region Swiftmend
        public float SwiftmendCF { get {
            if (swiftmend == null) return 0;
            float cf = SwiftmendCPS * swiftmend.CastTime;
            cf += swiftmend.rejuvUseChance * swiftmend.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.CastTime * SwiftmendCPS;
            cf += swiftmend.regrowthUseChance * swiftmend.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.CastTime * SwiftmendCPS;
            return cf; 
        } }
        public float SwiftmendCPS { get { return SwiftmendCPM / 60f; } }
        public float SwiftmendCPM = 0f;
        public float SwiftmendAvg { get { return swiftmend == null ? 0 : SwiftmendCPS * swiftmend.Duration; } }
        public float SwiftmendHPS { get {
            if (swiftmend == null) return 0;
            float hps = swiftmend.TotalAverageHealing * SwiftmendCPS;
            hps += swiftmend.regrowthUseChance * swiftmend.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.AverageHealingwithCrit * SwiftmendCPS;
            return hps; 
        } }

        public float SwiftmendMPS { get {
            if (swiftmend == null) return 0;
            float mps = SwiftmendCPS * swiftmend.ManaCost;
            mps += swiftmend.rejuvUseChance * swiftmend.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.ManaCost * SwiftmendCPS;
            mps += swiftmend.regrowthUseChance * swiftmend.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.ManaCost * SwiftmendCPS;
            return mps;
        } }
        public float SwiftmendHealsPerMinute { get { return SwiftmendCPM; } }
        public float SwiftmendCritsPerMinute { get { return swiftmend == null ? 0 : SwiftmendCPM * swiftmend.CritPercent / 100.0f; } }
        #endregion

        #region Passive Mana Regen
        public float Mana; // set by constructor
        public float SpiritMPS; // set by constructor
        public float GearMPS; // set by constructor
        public float SpiritInCombatFraction; // set by constructor
        private float replenishment; // set by constructor
        public float ReplenishmentUptime = 1f;
        public float ReplenishmentMPS { get { return Mana * replenishment * ReplenishmentUptime; } }
        public float RevitalizePPM = 0f;
        public float RevitalizeMPS { get { return RevitalizePPM * 0.01f * Mana; } }
        public float ProcsMPS; // set by constructor
        public float MPSInFSR { get { return RevitalizeMPS + ProcsMPS + GearMPS + ReplenishmentMPS + SpiritMPS * SpiritInCombatFraction; } }
        public float MPSOutFSR { get { return RevitalizeMPS + ProcsMPS + GearMPS + ReplenishmentMPS + SpiritMPS; } }
        public float OutOfCombatFraction = 0;
        public float ManaRegen { get { return OutOfCombatFraction * MPSOutFSR + (1f - OutOfCombatFraction) * MPSInFSR; } }
        #endregion

        #region Active Mana Regen
        public float ExtraMana { get { return PotionMana + InnervateMana; } }
        public float PotionMana = 0f;
        public float PotionMPS { get { return PotionMana / TotalTime; } }             
        public float InnervateMana { get { return ManaPerInnervate * NumberOfInnervates; } }
        public float InnervateMPS { get { return InnervateMana / TotalTime; } }
        public float ManaPerInnervate = 0f;
        public float NumberOfInnervates = 0f;
        #endregion

        #region Saved unreduces cast fractions
        public float RejuvCF_unreduced = 0f;
        public float RegrowthCF_unreduced = 0f;
        public float LifebloomCF_unreduced = 0f;
        public float LifebloomStackCF_unreduced = 0f;
        public float NourishCF_unreduced = 0f;
        public float WildGrowthCF_unreduced = 0f;
        public float SwiftmendCF_unreduced = 0f;
        public float IdleCF_unreduced = 1f;

        public float RejuvCF_unreducedOOM = 0f;
        public float RegrowthCF_unreducedOOM = 0f;
        public float LifebloomCF_unreducedOOM = 0f;
        public float NourishCF_unreducedOOM = 0f;
        public float WildGrowthCF_unreducedOOM = 0f;
        public float IdleCF_unreducedOOM = 1f;
        #endregion

        public float TotalTime;
        public float TimeToOOM { get { return EffMPS > 0 ? Math.Min((ExtraMana + Mana) / EffMPS, TotalTime) : TotalTime; } }
        public float TimeToOOM_unreduced = 0f;
        public float TimeAfterOOM { get { return TotalTime - TimeToOOM; } }

        public float EffMPS { get { return MPS - ManaRegen; } }

        // HotsCF includes Lifebloom stacks...
        public float TotalCF { get { return (float)Math.Round(HotsCF + WildGrowthCF + SwiftmendCF + NourishCF, 4); } }
        public float IdleCF { get { return 1f - TotalCF; } }

        public float TotalModifier { get { return TotalTime > TimeToOOM ? (TimeToOOM + BurnRegenFraction * TimeAfterOOM) / TotalTime : 1f; } }
        public float TimeToRegenAll { get { return Mana / MPSOutFSR; } }
        public float TimeToBurnAll { get { return Mana / EffMPS; } }
        public float BurnRegenFraction { get { return TimeToBurnAll / (TimeToRegenAll + TimeToBurnAll); } }

        public float ValAnyrShield = 0;
        public float ValAnyrHPS { get { return ValAnyrShield * (HotsHPS + WildGrowthHPS + SwiftmendHPS + NourishHPS); } }

        public float TotalHealing { get { return HPS * TotalTime * TotalModifier; } }
        public float TotalCastsPerMinute { get { return CastsPerMinute * TotalModifier; } }
        public float TotalCritsPerMinute { get { return CritsPerMinute * TotalModifier; } }
        public float TotalHealsPerMinute { get { return HealsPerMinute * TotalModifier; } }

        public float RejuvenationHealsPerMinute { get { return RejuvHealsPerMinute; } }

        public float RevitalizeChance = 0f;
        public float RevitalizeProcsPerMinute { get { return RevitalizeChance * (RejuvHealsPerMinute + WildGrowthHealsPerMinute); } }
        
        public RotationSettings rotSettings;
    }

    public enum SingleTargetBurstRotations
    {
        AutoSelect = 0,
        AutoSelectForceSwiftmend,
        RjN,
        RjLbN,
        RjRgLbN,
        RjRg,
        RjLbRg,
        Rg,
        RgN,
        N,
        RjN_s,
        RjLbN_s,
        RjRgLbN_s,
        RjRg_s,
        RjLbRg_s,
        Rg_s,
        RgN_s
    };

    public enum HealTargetTypes { TankHealing = 0, RaidHealing };

    public enum SpellList { HealingTouch = 0, Nourish, Regrowth, Rejuvenation };

    public enum LifeBloomType { Slow = 0, Fast, Rolling };

    public class RotationSettings
    {
        public float averageLifebloomStacks, averageRejuv, averageRegrowth;
        public LifeBloomType lifeBloomType;
        public int SwiftmendPerMin, WildGrowthPerMin;
        public float RejuvFraction, RegrowthFraction, LifebloomFraction, NourishFraction;
        public bool adjustRejuv, adjustRegrowth, adjustLifebloom, adjustNourish;
        public float nourish0, nourish1, nourish2, nourish3, nourish4;
        public float livingSeedEfficiency;
        public HealTargetTypes healTarget;
        public float latency;
        public float reduceOOMRejuv, reduceOOMRegrowth, reduceOOMLifebloom, reduceOOMNourish, reduceOOMWildGrowth;
    }

    public class SingleTargetBurstResult
    {
        public float HPS { get { return rejuvContribution+regrowthContribution+lifebloomContribution+swiftmendContribution+nourishContribution; } }
        public SingleTargetBurstRotations rotation;
        public float rejuvContribution;
        public float regrowthContribution;
        public float lifebloomContribution;
        public float swiftmendContribution;
        public float nourishContribution;

        public SingleTargetBurstResult(SingleTargetBurstRotations rotation, float rejuvContribution, float regrowthContribution, float lifebloomContribution, float swiftmendContribution, float nourishContribution)
        {
            this.rotation = rotation;
            this.rejuvContribution = rejuvContribution;
            this.regrowthContribution = regrowthContribution;
            this.lifebloomContribution = lifebloomContribution;
            this.swiftmendContribution = swiftmendContribution;
            this.nourishContribution = nourishContribution;
        }
    }


    public class Solver
    {
        public static RotationResult SimulateHealing(CharacterCalculationsTree calculatedStats, Stats stats, CalculationOptionsTree calcOpts, RotationSettings rotSettings)
        {
            RotationResult rot = new RotationResult(calculatedStats, stats, rotSettings.latency);
            rot.rotSettings = rotSettings;

            rot.TotalTime = calcOpts.FightDuration;

            #region Maintained Hots
            rot.LifebloomStackType = rotSettings.lifeBloomType;
            rot.LifebloomStacks = rotSettings.averageLifebloomStacks;
            float maintainedRejuvCF = rotSettings.averageRejuv * rot.rejuvenate.CastTime / rot.rejuvenate.Duration;
            float maintainedRegrowthCF = rotSettings.averageRegrowth * rot.regrowth.CastTime / rot.regrowth.Duration;
            #endregion

            rot.RejuvCF_unreduced = rot.RejuvCF = rotSettings.RejuvFraction + maintainedRejuvCF;
            rot.RegrowthCF_unreduced = rot.RegrowthCF = rotSettings.RegrowthFraction + maintainedRegrowthCF;
            rot.LifebloomCF_unreduced = rot.LifebloomCF = rotSettings.LifebloomFraction;
            rot.NourishCF_unreduced = rot.NourishCF = rotSettings.NourishFraction;
                
            #region Wild Growth
            // If talent isn't chosen disregard WildGrowth
            rot.WildGrowthCPM = (calculatedStats.LocalCharacter.DruidTalents.WildGrowth > 0) ? rotSettings.WildGrowthPerMin : 0;
            rot.WildGrowthCF_unreduced = rot.WildGrowthCF;
            #endregion

            #region Swiftmend
            if (calculatedStats.LocalCharacter.DruidTalents.Swiftmend > 0)
            {
                rot.swiftmend = new Swiftmend(calculatedStats, stats,
                    rot.RejuvCF > 0 ? rot.rejuvenate : null,
                    rot.RegrowthCF > 0 ? rot.regrowth : null);
                rot.swiftmend.latency = rotSettings.latency;
                rot.SwiftmendCPM = rotSettings.SwiftmendPerMin;
            }
            else
            {
                rot.SwiftmendCPM = 0;
            }
            rot.SwiftmendCF_unreduced = rot.SwiftmendCF;
            #endregion

            #region Mana regeneration
            rot.ReplenishmentUptime = calcOpts.ReplenishmentUptime / 100f; 
            rot.OutOfCombatFraction = 1f - .01f * calcOpts.FSRRatio;
            rot.RevitalizePPM = (float)calcOpts.RevitalizePPM / 100f;
            #endregion

            #region Mana potion
            rot.PotionMana = new int[] { 0, 1800, 2200, 2400, 3125, 4300 }[calcOpts.ManaPot];
            rot.PotionMana *= (stats.BonusManaPotion + 1f);
            #endregion

            #region Innervates
            // 3 min CD, takes 15 seconds to apply
            rot.NumberOfInnervates = (float)Math.Ceiling((calcOpts.FightDuration - 15f) / 180f);      
            rot.ManaPerInnervate = 2.25f * calcOpts.Innervates;       // Use self innervate?

            if (calculatedStats.LocalCharacter.DruidTalents.GlyphOfInnervate)
            {
                rot.ManaPerInnervate += 0.45f;
            }

            rot.ManaPerInnervate *= Rawr.Tree.TreeConstants.BaseMana;
            #endregion

            #region Nourish raw hps
            float _hpct = rotSettings.nourish0 * rot.nourish[0].HPCT +
                rotSettings.nourish1 * rot.nourish[1].HPCT +
                rotSettings.nourish2 * rot.nourish[2].HPCT +
                rotSettings.nourish3 * rot.nourish[3].HPCT +
                rotSettings.nourish4 * rot.nourish[4].HPCT;
            rot.NourishRawHPS = _hpct / rot.nourish[0].CastTime;
            #endregion

            #region Correct cast fractions 
            // Priority:
            // 1. Maintained Hots
            // 2. Wild growth and Swiftmend (Swiftmend cannot be reduced, only enabled/disabled)
            // 3. Spells not to be adjusted
            // 4. Idle time
            // 5. Spells to be adjusted

            float IdleCF = calcOpts.IdleCastTimePercent / 100f;
            rot.IdleCF_unreduced = IdleCF;

            if (rot.TotalCF + IdleCF > 1f)
            {
                float Static = maintainedRegrowthCF + maintainedRejuvCF + rot.LifebloomStackCF + rot.WildGrowthCF + rot.SwiftmendCF;
                if (!calcOpts.AdjustLifebloom) Static += rot.LifebloomCF;
                if (!calcOpts.AdjustNourish) Static += rot.NourishCF;
                if (!calcOpts.AdjustRejuv) Static += (rot.RejuvCF - maintainedRejuvCF);
                if (!calcOpts.AdjustRegrowth) Static += (rot.RegrowthCF - maintainedRejuvCF);
                float Factor = Math.Max(0f, (1f - Static - IdleCF) / (rot.TotalCF - Static));
                if (calcOpts.AdjustNourish) rot.NourishCF *= Factor;
                if (calcOpts.AdjustRejuv) rot.RejuvCF = (rot.RejuvCF - maintainedRejuvCF) * Factor + maintainedRejuvCF;
                if (calcOpts.AdjustRegrowth) rot.RegrowthCF = (rot.RegrowthCF - maintainedRegrowthCF) * Factor + maintainedRegrowthCF;
                if (calcOpts.AdjustLifebloom) rot.LifebloomCF *= Factor;
            }

            if (rot.TotalCF + IdleCF > 1f)
            {
                float Static = maintainedRegrowthCF + maintainedRejuvCF + rot.LifebloomStackCF + rot.WildGrowthCF + rot.SwiftmendCF;
                float Factor = Math.Max(0f, (1f - Static - IdleCF) / (rot.TotalCF - Static));
                rot.NourishCF *= Factor;
                rot.RejuvCF = (rot.RejuvCF - maintainedRejuvCF) * Factor + maintainedRejuvCF;
                rot.RegrowthCF = (rot.RegrowthCF - maintainedRegrowthCF) * Factor + maintainedRegrowthCF;
                rot.LifebloomCF *= Factor;
            }

            // Remove Swiftmend if not enough HoTs and rj/rg are not primary heals
            //if (rot.RejuvAvg + rot.RegrowthAvg < rot.SwiftmendCPM && !(rot.PrimaryHeal is Rejuvenation || rot.PrimaryHeal is Regrowth)) rot.SwiftmendCPM = 0f;

            // Next, try to reduce wild growth and swiftmend
            if (rot.TotalCF > 1f)
            {
                rot.SwiftmendCPM = 0;
                float Static = rot.LifebloomStackCF + rot.RejuvCF + rot.RegrowthCF;
                float Factor = Math.Min(1f, Math.Max(0f, (1f - Static) / (rot.TotalCF - Static)));
                // Rejuv/Regrowth/Lifebloom are already 0 at this point
                rot.WildGrowthCF *= Factor;
            }

            // Finally, try to reduce lifebloom stacks
            if (rot.TotalCF > 1f)
            {
                float Factor = 1f / rot.TotalCF;
                rot.LifebloomStackCF *= Factor;
                rot.RejuvCF *= Factor;
                rot.RegrowthCF *= Factor;
                // Now, rot.MaxPrimaryCF *must* be 1f exactly.
            }

            #endregion

            #region Correct going OOM
            // Based on MPS (and possibly HPM).
            // Actually, if you cast less, you will also see less returns. This is calculated elsewhere
            // in the Special Effects. Repeated calculations are necessary.
            // We assume this effect is convergent.

            float f = Math.Max(0.0f, (rot.TimeToOOM * (rot.MPS - rot.ManaRegen) + rot.TotalTime * rot.ManaRegen) / (rot.MPS * rot.TotalTime));
            // We want our MPS to become f*MPS

            float MPStoGain = rot.MPS * (1f - f);
            float max = Math.Max(rotSettings.reduceOOMRejuv, rotSettings.reduceOOMRegrowth);
            max = Math.Max(max, rotSettings.reduceOOMLifebloom);
            max = Math.Max(max, rotSettings.reduceOOMNourish);
            max = Math.Max(max, rotSettings.reduceOOMWildGrowth);
            MPStoGain *= max;

            float total = rotSettings.reduceOOMRejuv + rotSettings.reduceOOMRegrowth + rotSettings.reduceOOMLifebloom
                + rotSettings.reduceOOMNourish + rotSettings.reduceOOMWildGrowth;

            rot.TimeToOOM_unreduced = rot.TimeToOOM;
            rot.IdleCF_unreducedOOM = rot.IdleCF;
            rot.RejuvCF_unreducedOOM = rot.RejuvCF;
            rot.RegrowthCF_unreducedOOM = rot.RegrowthCF;
            rot.LifebloomCF_unreducedOOM = rot.LifebloomCF;
            rot.NourishCF_unreducedOOM = rot.NourishCF;
            rot.WildGrowthCF_unreducedOOM = rot.WildGrowthCF;

            if (total > 0)
            {
                float MPSfromRejuv = MPStoGain * rotSettings.reduceOOMRejuv / total;
                float MPSfromRegrowth = MPStoGain * rotSettings.reduceOOMRegrowth / total;
                float MPSfromLifebloom = MPStoGain * rotSettings.reduceOOMLifebloom / total;
                float MPSfromNourish = MPStoGain * rotSettings.reduceOOMNourish / total;
                float MPSfromWildGrowth = MPStoGain * rotSettings.reduceOOMWildGrowth / total;

                rot.RejuvCF = Math.Max(0.0f, rot.RejuvCPS - MPSfromRejuv / rot.rejuvenate.ManaCost) * rot.rejuvenate.CastTime;
                rot.RegrowthCF = Math.Max(0.0f, rot.RegrowthCPS - MPSfromRegrowth / rot.regrowth.ManaCost) * rot.regrowth.CastTime;
                rot.LifebloomCF = Math.Max(0.0f, rot.LifebloomCPS - MPSfromLifebloom / rot.lifebloom.ManaCost) * rot.lifebloom.CastTime;
                rot.NourishCF = Math.Max(0.0f, rot.NourishCPS - MPSfromNourish / rot.nourish[0].ManaCost) * rot.nourish[0].CastTime;
                rot.WildGrowthCF = Math.Max(0.0f, rot.WildGrowthCPS - MPSfromWildGrowth / rot.wildGrowth.ManaCost) * rot.wildGrowth.CastTime;
            }

            #endregion

            if (stats.ShieldFromHealed > 0) rot.ValAnyrShield = stats.ShieldFromHealed;

            return rot;
        }

        public static String SingleTargetRotationToText(SingleTargetBurstRotations rotation) {
            switch (rotation) {
                case SingleTargetBurstRotations.AutoSelect:
                    return "Autoselect";
                case SingleTargetBurstRotations.AutoSelectForceSwiftmend:
                    return "Autoselect + Swiftmend";
                case SingleTargetBurstRotations.RjN:
                    return "RjN*";
                case SingleTargetBurstRotations.RjLbN:
                    return "RjLbN*";
                case SingleTargetBurstRotations.RjRgLbN:
                    return "RjRgLbN*";
                case SingleTargetBurstRotations.RjRg:
                    return "RjRgN*";
                case SingleTargetBurstRotations.RjLbRg:
                    return "RjLbRg*";
                case SingleTargetBurstRotations.Rg:
                    return "Rg*";
                case SingleTargetBurstRotations.RgN:
                    return "RgN*";
                case SingleTargetBurstRotations.N:
                    return "N*";
                case SingleTargetBurstRotations.RjN_s:
                    return "RjN*+S";
                case SingleTargetBurstRotations.RjLbN_s:
                    return "RjLbN*+S";
                case SingleTargetBurstRotations.RjRgLbN_s:
                    return "RjRgLbN*+S";
                case SingleTargetBurstRotations.RjRg_s:
                    return "RjRgN*+S";
                case SingleTargetBurstRotations.RjLbRg_s:
                    return "RjLbRg*+S";
                case SingleTargetBurstRotations.Rg_s:
                    return "Rg*+S";
                case SingleTargetBurstRotations.RgN_s:
                    return "RgN*+S";
                default:
                    return "Unknown";
            }
        }

        public static SingleTargetBurstRotations SingleTargetIndexToRotation(int index)
        {
            switch (index)
            {
                case 0:
                default:
                    return SingleTargetBurstRotations.AutoSelect;
                case 1:
                    return SingleTargetBurstRotations.AutoSelectForceSwiftmend;
                case 2:
                    return SingleTargetBurstRotations.RjN;
                case 3:
                    return SingleTargetBurstRotations.RjLbN;
                case 4:
                    return SingleTargetBurstRotations.RjRgLbN;
                case 5:
                    return SingleTargetBurstRotations.RjRg;
                case 6:
                    return SingleTargetBurstRotations.RjLbRg;
                case 7:
                    return SingleTargetBurstRotations.Rg;
                case 8:
                    return SingleTargetBurstRotations.RgN;
                case 9:
                    return SingleTargetBurstRotations.N;
                case 10:
                    return SingleTargetBurstRotations.RjN_s;
                case 11:
                    return SingleTargetBurstRotations.RjLbN_s;
                case 12:
                    return SingleTargetBurstRotations.RjRgLbN_s;
                case 13:
                    return SingleTargetBurstRotations.RjRg_s;
                case 14:
                    return SingleTargetBurstRotations.RjLbRg_s;
                case 15:
                    return SingleTargetBurstRotations.Rg_s;
                case 16:
                    return SingleTargetBurstRotations.RgN_s;
            }
        }

        public static SingleTargetBurstResult[] CalculateSingleTargetBurst(CharacterCalculationsTree calculatedStats, Stats stats, CalculationOptionsTree calcOpts, SingleTargetBurstRotations rotation)
        {
            #region Setup Spells
            Spell regrowth = new Regrowth(calculatedStats, stats, false);
            Spell regrowthAgain = new Regrowth(calculatedStats, stats, true);
            Spell lifebloom = new Lifebloom(calculatedStats, stats);
            Spell lifebloomSlowStack = new Lifebloom(calculatedStats, stats, 3, false);
            Spell lifebloomFastStack = new Lifebloom(calculatedStats, stats, 3, true);
            Spell lifebloomRollingStack = new LifebloomStack(calculatedStats, stats);

            Spell rejuvenate = new Rejuvenation(calculatedStats, stats);
            Spell[] nourish = new Nourish[5];
            nourish[0] = new Nourish(calculatedStats, stats, 0);
            nourish[1] = new Nourish(calculatedStats, stats, 1);
            nourish[2] = new Nourish(calculatedStats, stats, 2);
            nourish[3] = new Nourish(calculatedStats, stats, 3);
            nourish[4] = new Nourish(calculatedStats, stats, 4);
            Spell healingTouch = new HealingTouch(calculatedStats, stats);
            //WildGrowth wildGrowth = new WildGrowth(calculatedStats, stats);
            #endregion

            #region Add latency
            regrowth.latency = calcOpts.Latency / 1000f;
            regrowthAgain.latency = calcOpts.Latency / 1000f;
            lifebloom.latency = calcOpts.Latency / 1000f;
            lifebloomSlowStack.latency = calcOpts.Latency / 1000f;
            lifebloomFastStack.latency = calcOpts.Latency / 1000f;
            lifebloomRollingStack.latency = calcOpts.Latency / 1000f;
            rejuvenate.latency = calcOpts.Latency / 1000f;
            nourish[0].latency = calcOpts.Latency / 1000f;
            nourish[1].latency = calcOpts.Latency / 1000f;
            nourish[2].latency = calcOpts.Latency / 1000f;
            nourish[3].latency = calcOpts.Latency / 1000f;
            nourish[4].latency = calcOpts.Latency / 1000f;
            healingTouch.latency = calcOpts.Latency / 1000f;
            //wildGrowth.latency = calcOpts.Latency / 1000f;
            #endregion
            
            #region Setup Swiftmend
            float swift1HPS = 0f;
            float swift2HPS = 0f;
            float swift3HPS = 0f;
            float swift1Fraction = 0f;
            float swift2Fraction = 0f;
            float swift3Fraction = 0f;

            if (calculatedStats.LocalCharacter.DruidTalents.Swiftmend > 0)
            {
                Swiftmend swift1 = new Swiftmend(calculatedStats, stats, rejuvenate, regrowth);
                Swiftmend swift2 = new Swiftmend(calculatedStats, stats, rejuvenate, null);
                Swiftmend swift3 = new Swiftmend(calculatedStats, stats, null, regrowth);

                swift1.latency = calcOpts.Latency / 1000f;
                swift2.latency = calcOpts.Latency / 1000f;
                swift3.latency = calcOpts.Latency / 1000f;

                swift1Fraction = swift1.CastTime / 15.0f;
                swift1HPS = swift1.TotalAverageHealing / 15.0f;
                swift2Fraction = swift2.CastTime / 15.0f;
                swift2HPS = swift2.TotalAverageHealing / 15.0f;
                swift3Fraction = swift3.CastTime / 15.0f;
                swift3HPS = swift3.TotalAverageHealing / 15.0f;
                if (!calculatedStats.LocalCharacter.DruidTalents.GlyphOfSwiftmend)
                {
                    swift1HPS += swift1.regrowthUseChance * regrowth.AverageHealingwithCrit / 15f;
                    swift1Fraction += swift1.rejuvUseChance * rejuvenate.CastTime / 15f;
                    swift1Fraction += swift1.regrowthUseChance * regrowth.CastTime / 15f;

                    swift2Fraction += rejuvenate.CastTime / 15f;

                    swift3HPS += regrowth.AverageHealingwithCrit / 15f;
                    swift3Fraction += regrowth.CastTime / 15f;
                }
            }
            #endregion

            Spell lifebloomSpell = null;

            switch (calcOpts.LifebloomStackType)
            {
                case 0:
                    lifebloomSpell = lifebloomSlowStack;
                    break;
                case 1:
                    lifebloomSpell = lifebloomFastStack;
                    break;
                case 2:
                    lifebloomSpell = lifebloomRollingStack;
                    break;
                default:
                    // err...
                    lifebloomSpell = lifebloom;
                    break;
            }

            float RejuvFraction = rejuvenate.CastTime / rejuvenate.Duration;
            float RegrowthFraction = regrowth.CastTime / regrowth.Duration;
            float LifebloomFraction = lifebloomSpell.CastTime / lifebloomSpell.Duration;

            // RJ + N1 
            SingleTargetBurstResult RjN1 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjN,
                RejuvFraction * rejuvenate.HPCT, 0, 0, 0, (1f - RejuvFraction) * nourish[1].HPCT);
            // RJ + LB + N2 
            SingleTargetBurstResult RjLbN2 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbN,
                RejuvFraction * rejuvenate.HPCT, 0, LifebloomFraction * lifebloomSpell.HPCT, 0, (1f - RejuvFraction - LifebloomFraction) * nourish[2].HPCT);
            // RJ + RG + LB + N3 
            SingleTargetBurstResult RjRgLbN3 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRgLbN,
                RejuvFraction * rejuvenate.HPCT, RegrowthFraction * regrowth.HPCT, LifebloomFraction * lifebloomSpell.HPCT, 0,
                (1f - RejuvFraction - RegrowthFraction - LifebloomFraction) * nourish[3].HPCT);
            // RJ + RG 
            SingleTargetBurstResult RjRg = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRg,
                RejuvFraction * rejuvenate.HPCT, (1f - RejuvFraction) * regrowthAgain.HPCT_DH, 0, 0, 0);
            // RJ + LB + RG
            SingleTargetBurstResult RjLbRg = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbRg,
                RejuvFraction * rejuvenate.HPCT, (1f - RejuvFraction - LifebloomFraction) * regrowthAgain.HPCT_DH, LifebloomFraction * lifebloomSpell.HPCT, 0, 0);
            // RG
            SingleTargetBurstResult Rg = new SingleTargetBurstResult(SingleTargetBurstRotations.Rg, 
                0, regrowthAgain.HPS, 0, 0, 0);
            // RG + N1
            SingleTargetBurstResult RgN1 = new SingleTargetBurstResult(SingleTargetBurstRotations.RgN,
                0, RegrowthFraction * regrowth.HPCT, 0, 0, (1f - RegrowthFraction) * nourish[1].HPCT);
            // N0
            SingleTargetBurstResult N0 = new SingleTargetBurstResult(SingleTargetBurstRotations.N, 
                0, 0, 0, 0, nourish[0].HPS);

            // RJ + N1 S
            SingleTargetBurstResult RjN1S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjN_s,
                RejuvFraction * rejuvenate.HPCT, 0, 0, swift2HPS, (1f - RejuvFraction - swift2Fraction) * nourish[1].HPCT);
            // RJ + LB + N2 S
            SingleTargetBurstResult RjLbN2S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbN_s,
                RejuvFraction * rejuvenate.HPCT, 0, LifebloomFraction * lifebloomSpell.HPCT, swift2HPS,
                (1f - RejuvFraction - LifebloomFraction - swift2Fraction) * nourish[2].HPCT);
            // RJ + RG + LB + N3 S
            SingleTargetBurstResult RjRgLbN3S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRgLbN_s,
                RejuvFraction * rejuvenate.HPCT, RegrowthFraction * regrowth.HPCT, LifebloomFraction * lifebloomSpell.HPCT, swift1HPS, 
                (1f - RejuvFraction - RegrowthFraction - LifebloomFraction - swift1Fraction) * nourish[3].HPCT);
            // RJ + RG S
            SingleTargetBurstResult RjRgS = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRg_s,
                RejuvFraction * rejuvenate.HPCT, (1f - RejuvFraction - swift1Fraction) * regrowthAgain.HPCT_DH, 0, swift1HPS, 0);
            // RJ + LB + RG S
            SingleTargetBurstResult RjLbRgS = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbRg_s,
                RejuvFraction * rejuvenate.HPCT, (1f - RejuvFraction - LifebloomFraction - swift1Fraction) * regrowthAgain.HPCT_DH,
                LifebloomFraction * lifebloomSpell.HPCT, swift1HPS, 0);
            // RG S
            SingleTargetBurstResult RgS = new SingleTargetBurstResult(SingleTargetBurstRotations.Rg_s, 
                0, (1f - swift3Fraction) * regrowthAgain.HPCT_DH, 0, swift3HPS, 0);
            // RG + N1 S
            SingleTargetBurstResult RgN1S = new SingleTargetBurstResult(SingleTargetBurstRotations.RgN_s,
                0, RegrowthFraction * regrowth.HPCT, 0, swift3HPS, (1f - RegrowthFraction - swift3Fraction) * nourish[1].HPCT);

            SingleTargetBurstResult result = new SingleTargetBurstResult(SingleTargetBurstRotations.AutoSelect, 0, 0, 0, 0, 0);

            if (rotation == SingleTargetBurstRotations.AutoSelect ||
                rotation == SingleTargetBurstRotations.AutoSelectForceSwiftmend)
            {
                if (rotation != SingleTargetBurstRotations.AutoSelectForceSwiftmend)
                {
                    if (RjN1.HPS > result.HPS) result = RjN1;
                    if (RjLbN2.HPS > result.HPS) result = RjLbN2;
                    if (RjRgLbN3.HPS > result.HPS) result = RjRgLbN3;
                    if (RjRg.HPS > result.HPS) result = RjRg;
                    if (RjLbRg.HPS > result.HPS) result = RjLbRg;
                    if (Rg.HPS > result.HPS) result = Rg;
                    if (N0.HPS > result.HPS) result = N0;
                }

                if (RjN1S.HPS > result.HPS) result = RjN1S;
                if (RjLbN2S.HPS > result.HPS) result = RjLbN2S;
                if (RjRgLbN3S.HPS > result.HPS) result = RjRgLbN3S;
                if (RjRgS.HPS > result.HPS) result = RjRgS;
                if (RjLbRgS.HPS > result.HPS) result = RjLbRgS;
                if (RgS.HPS > result.HPS) result = RgS;
                if (RgN1S.HPS > result.HPS) result = RgN1S;
            }
            else
            {
                switch (rotation)
                {
                    case SingleTargetBurstRotations.RjN:
                        result = RjN1;
                        break;
                    case SingleTargetBurstRotations.RjLbN:
                        result = RjLbN2;
                        break;
                    case SingleTargetBurstRotations.RjRgLbN:
                        result = RjRgLbN3;
                        break;
                    case SingleTargetBurstRotations.RjRg:
                        result = RjRg;
                        break;
                    case SingleTargetBurstRotations.RjLbRg:
                        result = RjLbRg;
                        break;
                    case SingleTargetBurstRotations.Rg:
                        result = Rg;
                        break;
                    case SingleTargetBurstRotations.RgN:
                        result = RgN1;
                        break;
                    case SingleTargetBurstRotations.N:
                        result = N0;
                        break;
                    case SingleTargetBurstRotations.RjN_s:
                        result = RjN1S;
                        break;
                    case SingleTargetBurstRotations.RjLbN_s:
                        result = RjLbN2S;
                        break;
                    case SingleTargetBurstRotations.RjRgLbN_s:
                        result = RjRgLbN3S;
                        break;
                    case SingleTargetBurstRotations.RjRg_s:
                        result = RjRgS;
                        break;
                    case SingleTargetBurstRotations.RjLbRg_s:
                        result = RjLbRgS;
                        break;
                    case SingleTargetBurstRotations.Rg_s:
                        result = RgS;
                        break;
                    case SingleTargetBurstRotations.RgN_s:
                        result = RgN1S;
                        break;
                    default:
                        break;
                }
            }

            #region Apply bonus from Val'anyr
            if (stats.ShieldFromHealed > 0)
            {
                result.rejuvContribution *= (1 + stats.ShieldFromHealed);
                result.regrowthContribution *= (1 + stats.ShieldFromHealed);
                result.lifebloomContribution *= (1 + stats.ShieldFromHealed);
                result.swiftmendContribution *= (1 + stats.ShieldFromHealed);
                result.nourishContribution *= (1 + stats.ShieldFromHealed);
            }
            #endregion

            List<SingleTargetBurstResult> rots = new List<SingleTargetBurstResult>();
            rots.Add(result);
            rots.Add(result);
            rots.Add(RjN1);
            rots.Add(RjLbN2);
            rots.Add(RjRgLbN3);
            rots.Add(RjRg);
            rots.Add(RjLbRg);
            rots.Add(Rg);
            rots.Add(RgN1);
            rots.Add(N0);
            rots.Add(RjN1S);
            rots.Add(RjLbN2S);
            rots.Add(RjRgLbN3S);
            rots.Add(RjRgS);
            rots.Add(RjLbRgS);
            rots.Add(RgS);
            rots.Add(RgN1S);

            return rots.ToArray();
        }
    }

    public static class TreeConstants
    {
        // Master is now in Base.BaseStats and Base.StatConversion
        public static float BaseMana;  // Keep since this is more convenient reference than calling the whole BaseStats function every time // = 3496f;
    }
}
