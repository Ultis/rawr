using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class RotationResult
    {
        public RotationResult(CharacterCalculationsTree calculatedStats, Stats stats)
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

            #region Setup Mana regen
            SpiritMPS = StatConversion.GetSpiritRegenSec(stats.Intellect, stats.Spirit);
            replenishment = stats.ManaRestoreFromMaxManaPerSecond; 
            Mana = stats.Mana;
            GearMPS = stats.Mp5 / 5f;
            SpiritInCombatFraction = stats.SpellCombatManaRegeneration;
            ProcsMPS = stats.ManaRestore;
            #endregion
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

        public float HPS { get { return HotsHPS + WildGrowthHPS + SwiftmendHPS + PrimaryHPS + ValAnyrHPS; } }
        public float MPS { get { return HotsMPS + WildGrowthMPS + SwiftmendMPS + PrimaryMPS; } }
        public float CastsPerMinute { get { return HotsCastsPerMinute + WildGrowthCPM + SwiftmendCPM + PrimaryCPM; } }
        public float HealsPerMinute { get { return HotsHealsPerMinute + WildGrowthHealsPerMinute + SwiftmendHealsPerMinute + PrimaryHealsPerMinute; } }
        public float CritsPerMinute { get { return HotsCritsPerMinute + SwiftmendCritsPerMinute + PrimaryCritsPerMinute; } }

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
        public float RejuvHPS { get { return RejuvAvg * rejuvenate.HPSHoT + RejuvCPS * rejuvenate.AverageHealingwithCrit; } }
        public float RejuvMPS { get { return RejuvCPS * rejuvenate.ManaCost; } }
        public float RejuvHealsPerMinute { get { return RejuvCPM * rejuvenate.PeriodicTicks; } }
        #endregion

        #region Regrowth
        public float RegrowthCF = 0f;
        public float RegrowthCPS { get { return RegrowthCF / regrowth.CastTime; } }
        public float RegrowthCPM { get { return 60f * RegrowthCF / regrowth.CastTime; } }
        public float RegrowthAvg { get { return RegrowthCPS * regrowth.Duration; } }
        public float RegrowthHPS { get { return RegrowthAvg * regrowth.HPSHoT + RegrowthCPS * regrowth.AverageHealingwithCrit; } }
        public float RegrowthMPS { get { return RegrowthCPS * regrowth.ManaCost; } }
        public float RegrowthHealsPerMinute { get { return RegrowthCPM * (1f + regrowth.PeriodicTicks); } }
        public float RegrowthCritsPerMinute { get { return RegrowthCPM * regrowth.CritPercent / 100f; } }
        #endregion

        #region Lifebloom
        public float LifebloomCF = 0f;
        public float LifebloomCPS { get { return LifebloomCF / lifebloom.CastTime; } }
        public float LifebloomCPM { get { return 60f * LifebloomCF / lifebloom.CastTime; } }
        public float LifebloomAvg { get { return LifebloomCPS * lifebloom.Duration; } }
        public float LifebloomHPS { get { return LifebloomAvg * lifebloom.HPSHoT + LifebloomCPS * lifebloom.AverageHealingwithCrit; } }
        public float LifebloomMPS { get { return LifebloomCPS * lifebloom.ManaCost; } }
        public float LifebloomHealsPerMinute { get { return LifebloomCPM * (1f + lifebloom.PeriodicTicks); } }
        public float LifebloomCritsPerMinute { get { return LifebloomCPM * lifebloom.CritPercent / 100f; } }
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
        public float WildGrowthCPS { get { return WildGrowthCF * wildGrowth.CastTime; } }
        public float WildGrowthCPM { get { return 60f * WildGrowthCPS; } set { WildGrowthCF = value / (60f * wildGrowth.CastTime); } }
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

        #region Primary
        public Spell PrimaryHeal = null;
        public float PrimaryRawHPS = 0f;
        public float PrimaryHPS { get { return PrimaryHeal == null ? 0 : PrimaryCF * PrimaryRawHPS; } }
        public float PrimaryMPS { get { return PrimaryHeal == null ? 0 : PrimaryCPS * PrimaryHeal.ManaCost; } }
        public float MaxPrimaryCF { get { return 1f - (HotsCF + WildGrowthCF + SwiftmendCF); } }
        public float PrimaryCF = 0f;
        public float PrimaryCPS { get { return PrimaryHeal == null ? 0 : PrimaryCF / PrimaryHeal.CastTime; } }
        public float PrimaryCPM { get { return PrimaryHeal == null ? 0 : 60f * PrimaryCF / PrimaryHeal.CastTime; } }
        public float PrimaryHealsPerMinute { get { return PrimaryHeal == null ? 0 : PrimaryCPM * (PrimaryHeal is Rejuvenation ? PrimaryHeal.PeriodicTicks : 1f); } }
        public float PrimaryCritsPerMinute { get { return PrimaryHeal == null ? 0 : PrimaryHealsPerMinute * PrimaryHeal.CritPercent / 100f; } }
        #endregion

        //trueHotsHPS += Activity * regrowth.HPSHoT;
        //trueHotsHPS += Activity * lifebloom.HPSHoT;
        // rejuv heals per minute

        #region Passive Mana Regen
        public float Mana; // set by constructor
        public float SpiritMPS; // set by constructor
        public float GearMPS; // set by constructor
        public float SpiritInCombatFraction; // set by constructor
        private float replenishment; // set by constructor
        public float ReplenishmentUptime = 1f;
        public float ReplenishmentMPS { get { return Mana * replenishment * ReplenishmentUptime; } }
        public float ProcsMPS; // set by constructor
        public float MPSInFSR { get { return ProcsMPS + GearMPS + ReplenishmentMPS + SpiritMPS * SpiritInCombatFraction; } }
        public float MPSOutFSR { get { return ProcsMPS + GearMPS + ReplenishmentMPS + SpiritMPS; } }
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

        public float TotalTime;
        public float TimeToOOM { get { return EffMPS > 0 ? Math.Min((ExtraMana + Mana) / EffMPS, TotalTime) : TotalTime; } }
        public float TimeAfterOOM { get { return TotalTime - TimeToOOM; } }

        public float EffMPSWithoutPrimary { get { return HotsMPS + WildGrowthMPS + SwiftmendMPS - ManaRegen; } }
        public float EffMPS { get { return EffMPSWithoutPrimary + PrimaryMPS; } }

        public float IdleCF { get { return MaxPrimaryCF - PrimaryCF; } }

        //float ManaAfterHots = (rot.ExtraMana + rot.Mana) - EffectiveBurn * calcOpts.FightDuration;
        //float PrimaryHealMPSAvailable = ManaAfterHots / calcOpts.FightDuration;

        public float TotalModifier { get { return TotalTime > TimeToOOM ? (TimeToOOM + BurnRegenFraction * TimeAfterOOM) / TotalTime : 1f; } }
        public float TimeToRegenAll { get { return Mana / MPSOutFSR; } }
        public float TimeToBurnAll { get { return Mana / EffMPS; } }
        public float BurnRegenFraction { get { return TimeToBurnAll / (TimeToRegenAll + TimeToBurnAll); } }

        public float ValAnyrShield = 0;
        public float ValAnyrHPS { get { return ValAnyrShield * (HotsHPS + WildGrowthHPS + SwiftmendHPS + PrimaryHPS); } }

        public float TotalHealing { get { return HPS * TotalTime * TotalModifier; } }
        public float TotalCastsPerMinute { get { return CastsPerMinute * TotalModifier; } }
        public float TotalCritsPerMinute { get { return CritsPerMinute * TotalModifier; } }
        public float TotalHealsPerMinute { get { return HealsPerMinute * TotalModifier; } }

        public float RejuvenationHealsPerMinute { get { return RejuvHealsPerMinute + (PrimaryHeal is Rejuvenation ? PrimaryHealsPerMinute : 0); } }
        
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
        public float RejuvFraction, RegrowthFraction, averageLifebloomStacks, LifebloomFraction;
        public float nourish0, nourish1, nourish2, nourish3, nourish4;
        public LifeBloomType lifeBloomType;
        public SpellList primaryHeal;
        public HealTargetTypes healTarget;
        public int SwiftmendPerMin, WildGrowthPerMin;
        public float livingSeedEfficiency;
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
            RotationResult rot = new RotationResult(calculatedStats, stats);

            rot.TotalTime = calcOpts.FightDuration;

            #region Maintained Hots
            rot.RejuvCF = rotSettings.RejuvFraction;
            rot.RegrowthCF = rotSettings.RegrowthFraction;
            rot.LifebloomCF = rotSettings.LifebloomFraction;
            rot.LifebloomStackType = rotSettings.lifeBloomType;
            rot.LifebloomStacks = rotSettings.averageLifebloomStacks;
            #endregion
                
            #region Wild Growth
            // If talent isn't chosen disregard WildGrowth
            rot.WildGrowthCPM = (calculatedStats.LocalCharacter.DruidTalents.WildGrowth > 0) ? rotSettings.WildGrowthPerMin : 0;
            #endregion

            #region Swiftmend
            if ((rot.RejuvCF > 0 || rot.RegrowthCF > 0) && (rotSettings.SwiftmendPerMin > 0) && (calculatedStats.LocalCharacter.DruidTalents.Swiftmend > 0))
            {
                rot.swiftmend = new Swiftmend(calculatedStats, stats, rot.RejuvCF > 0 ? rot.rejuvenate : null, rot.RegrowthCF > 0 ? rot.regrowth : null);
                rot.SwiftmendCPM = rotSettings.SwiftmendPerMin;

                #region Consumed HoTs if not refreshed
                /*// Handle consumed HoTs
                // Loss of HoTs HPS if not refreshed          // Use only one of these 2 sections
                hotsHPS -= swift.rejuvUseChance * swift.rejuvTicksLost * rejuvenate.PeriodicTick * swiftmendPMin / 60.0f;
                hotsHPS -= swift.regrowthUseChance * swift.regrowthTicksLost * regrowth.PeriodicTick * swiftmendPMin / 60.0f;/
                #endregion
                #region Replace HoTs if refreshed
                // Extra MPS if HoTs refreshed              // Use only one of these 2 sections
                hotsMPS += swift.rejuvUseChance * swift.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.ManaCost * rotSettings.SwiftmendPerMin / 60.0f;
                hotsMPS += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.ManaCost * rotSettings.SwiftmendPerMin / 60.0f;
                // Replacing Regrowths gives extra direct heals
                hotsHPS += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.AverageHealingwithCrit * rotSettings.SwiftmendPerMin / 60.0f;
                // Replacing HoTs take extra time
                hotsCastFrac += swift.rejuvUseChance * swift.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.CastTime * rotSettings.SwiftmendPerMin / 60.0f;
                hotsCastFrac += swift.regrowthUseChance * swift.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.CastTime * rotSettings.SwiftmendPerMin / 60.0f;\
                 */
                #endregion

            }
            #endregion

            #region Mana regeneration
            rot.ReplenishmentUptime = calcOpts.ReplenishmentUptime / 100f; 
            rot.OutOfCombatFraction = 1f - .01f * calcOpts.FSRRatio;
            #endregion

            #region Mana potion
            rot.PotionMana = new int[] { 0, 1800, 2200, 2400, 4300 }[calcOpts.ManaPot];
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

            // TODO: Should we add this limit back in?
            // lets assume the mana return is maximally 95% of your mana
            // thus take the smaller value of 95% of mana pool and total mana regenerated
            // manaFromInnervate = Math.Min(manaFromInnervate, .95f * stats.Mana);
            #endregion

            #region Select Primary Heal and apply Nature's Grace
            switch (rotSettings.primaryHeal)
            {
                case SpellList.Nourish:
                    rot.PrimaryHeal = rot.nourish[0];
                    break;
                case SpellList.Regrowth:
                    rot.PrimaryHeal = rotSettings.healTarget == HealTargetTypes.TankHealing ? rot.regrowthAgain : rot.regrowth;
                    break;
                case SpellList.Rejuvenation:
                    rot.PrimaryHeal = rot.rejuvenate;
                    break;
                case SpellList.HealingTouch:
                default:
                    rot.PrimaryHeal = rot.healingTouch;
                    break;
            }

            /*if (rot.PrimaryHeal is Nourish)
            {
                rot.nourish[0].calculateNewNaturesGrace(rot.nourish[0].CritPercent / 100f);
                rot.nourish[1].calculateNewNaturesGrace(rot.nourish[1].CritPercent / 100f);
                rot.nourish[2].calculateNewNaturesGrace(rot.nourish[2].CritPercent / 100f);
                rot.nourish[3].calculateNewNaturesGrace(rot.nourish[3].CritPercent / 100f);
                rot.nourish[4].calculateNewNaturesGrace(rot.nourish[4].CritPercent / 100f);
            }
            else
            {
                rot.PrimaryHeal.calculateNewNaturesGrace(rot.PrimaryHeal.CritPercent / 100f);
            }*/
            #endregion

            #region Primary Heal raw hps
            if (rot.PrimaryHeal is Nourish)
            {
                if (rotSettings.healTarget == HealTargetTypes.TankHealing)
                {
                    float _hps = rotSettings.nourish0 * rot.nourish[0].HPS +
                        rotSettings.nourish1 * rot.nourish[1].HPS +
                        rotSettings.nourish2 * rot.nourish[2].HPS +
                        rotSettings.nourish3 * rot.nourish[3].HPS +
                        rotSettings.nourish4 * rot.nourish[4].HPS;
                    rot.PrimaryRawHPS = _hps;
                }
                else
                {
                    float _hpct = rotSettings.nourish0 * rot.nourish[0].HPCT +
                        rotSettings.nourish1 * rot.nourish[1].HPCT +
                        rotSettings.nourish2 * rot.nourish[2].HPCT +
                        rotSettings.nourish3 * rot.nourish[3].HPCT +
                        rotSettings.nourish4 * rot.nourish[4].HPCT;
                    rot.PrimaryRawHPS = _hpct;
                }
            }
            else
            {
                if (rotSettings.healTarget == HealTargetTypes.RaidHealing)
                {
                    rot.PrimaryRawHPS = rot.PrimaryHeal.HPCT;
                }
                else
                {
                    rot.PrimaryRawHPS = rot.PrimaryHeal.HPS;
                }
            }
            #endregion

            #region Correct cast fractions and calculate primary spell cast fraction
            // Priority:
            // 1. Lifebloom stack
            // 2. Wild growth and Swiftmend (Swiftmend cannot be reduced, only enabled/disabled)
            // 3. Hots
            // 4. Idle time
            // 5. Primary spell

            // First, try to reduce Hots
            if (rot.MaxPrimaryCF < 0f)
            {
                float Static = rot.LifebloomStackCF + rot.WildGrowthCF + rot.SwiftmendCF;
                float Factor = Math.Max(0f, (1f - Static) / (1f - rot.MaxPrimaryCF - Static));
                rot.RejuvCF *= Factor;
                rot.RegrowthCF *= Factor;
                rot.LifebloomCF *= Factor;
                if (rot.RejuvCPM + rot.RegrowthCPM < rot.SwiftmendCPM) rot.SwiftmendCPM = 0f;
            }

            // Next, try to reduce wild growth and swiftmend
            if (rot.MaxPrimaryCF < 0f)
            {
                rot.SwiftmendCPM = 0;
                float Static = rot.LifebloomStackCF;
                float Factor = Math.Min(1f, Math.Max(0f, (1f - Static) / (1f - rot.MaxPrimaryCF - Static)));
                // Rejuv/Regrowth/Lifebloom are already 0 at this point
                rot.WildGrowthCF *= Factor;
            }

            // Finally, try to reduce lifebloom stacks
            if (rot.MaxPrimaryCF < 0f)
            {
                rot.LifebloomStackCF = 1f;
                // Now, rot.MaxPrimaryCF *must* be 1f exactly.
            }

            rot.PrimaryCF = Math.Max(0f, rot.MaxPrimaryCF - calcOpts.IdleCastTimePercent / 100f);
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
            WildGrowth wildGrowth = new WildGrowth(calculatedStats, stats);
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

            float RejuvFraction = rejuvenate.CastTime / rejuvenate.Duration;
            float RegrowthFraction = regrowth.CastTime / regrowth.Duration;
            float LifebloomFraction = lifebloomRollingStack.CastTime / lifebloomRollingStack.Duration;

            // RJ + N1 
            SingleTargetBurstResult RjN1 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjN, 
                rejuvenate.HPSHoT, 0, 0, 0, (1f - RejuvFraction) * nourish[1].HPS);
            // RJ + LB + N2 
            SingleTargetBurstResult RjLbN2 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbN, 
                rejuvenate.HPSHoT, 0, lifebloomRollingStack.HPSHoT, 0, (1f - RejuvFraction - LifebloomFraction) * nourish[2].HPS);
            // RJ + RG + LB + N3 
            SingleTargetBurstResult RjRgLbN3 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRgLbN,
                rejuvenate.HPSHoT, regrowth.HPSHoT + regrowth.AverageHealingwithCrit / regrowth.Duration, lifebloomRollingStack.HPSHoT, 0, 
                (1f - RejuvFraction - RegrowthFraction - LifebloomFraction) * nourish[3].HPS);
            // RJ + RG 
            SingleTargetBurstResult RjRg = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRg, 
                rejuvenate.HPSHoT, (1f - RejuvFraction) * regrowthAgain.HPS, 0, 0, 0);
            // RJ + LB + RG
            SingleTargetBurstResult RjLbRg = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbRg, 
                rejuvenate.HPSHoT, (1f - RejuvFraction - LifebloomFraction) * regrowthAgain.HPS, lifebloomRollingStack.HPSHoT, 0, 0);
            // RG
            SingleTargetBurstResult Rg = new SingleTargetBurstResult(SingleTargetBurstRotations.Rg, 
                0, regrowthAgain.HPS, 0, 0, 0);
            // RG + N1
            SingleTargetBurstResult RgN1 = new SingleTargetBurstResult(SingleTargetBurstRotations.RgN,
                0, regrowth.HPSHoT + regrowth.AverageHealingwithCrit / regrowth.Duration, 0, 0, (1f - RegrowthFraction) * nourish[1].HPS);
            // N0
            SingleTargetBurstResult N0 = new SingleTargetBurstResult(SingleTargetBurstRotations.N, 
                0, 0, 0, 0, nourish[0].HPS);

            // RJ + N1 S
            SingleTargetBurstResult RjN1S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjN_s, 
                rejuvenate.HPSHoT, 0, 0, swift2HPS, (1f - RejuvFraction - swift2Fraction) * nourish[1].HPS);
            // RJ + LB + N2 S
            SingleTargetBurstResult RjLbN2S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbN_s, 
                rejuvenate.HPSHoT, 0, lifebloomRollingStack.HPSHoT, swift2HPS, 
                (1f - RejuvFraction - LifebloomFraction - swift2Fraction) * nourish[2].HPS);
            // RJ + RG + LB + N3 S
            SingleTargetBurstResult RjRgLbN3S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRgLbN_s,
                rejuvenate.HPSHoT, regrowth.HPSHoT + regrowth.AverageHealingwithCrit / regrowth.Duration, lifebloomRollingStack.HPSHoT, swift1HPS, 
                (1f - RejuvFraction - RegrowthFraction - LifebloomFraction - swift1Fraction) * nourish[3].HPS);
            // RJ + RG S
            SingleTargetBurstResult RjRgS = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRg_s, 
                rejuvenate.HPSHoT, (1f - RejuvFraction - swift1Fraction) * regrowthAgain.HPS, 0, swift1HPS, 0);
            // RJ + LB + RG S
            SingleTargetBurstResult RjLbRgS = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbRg_s, 
                rejuvenate.HPSHoT, (1f - RejuvFraction - LifebloomFraction - swift1Fraction) * regrowthAgain.HPS, 
                lifebloomRollingStack.HPSHoT, swift1HPS, 0);
            // RG S
            SingleTargetBurstResult RgS = new SingleTargetBurstResult(SingleTargetBurstRotations.Rg_s, 
                0, (1f - swift3Fraction) * regrowthAgain.HPS, 0, swift3HPS, 0);
            // RG + N1 S
            SingleTargetBurstResult RgN1S = new SingleTargetBurstResult(SingleTargetBurstRotations.RgN_s,
                0, regrowth.HPSHoT + regrowth.AverageHealingwithCrit / regrowth.Duration, 0, swift3HPS, (1f - RegrowthFraction - swift3Fraction) * nourish[1].HPS);

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
