using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class SpellMix
    {
        private Character cacheCharacter;
        private float cacheLatency;
        private Stats cacheStats;

        public SpellMix(SpellMix copy)
        {
            cacheCharacter = copy.cacheCharacter;
            cacheLatency = copy.cacheLatency;
            cacheStats = copy.cacheStats;

            #region Setup Spells
            regrowth = new Regrowth(cacheCharacter, cacheStats, false);
            regrowthAgain = new Regrowth(cacheCharacter, cacheStats, true);
            lifebloom = new Lifebloom(cacheCharacter, cacheStats);
            lifebloomSlowStack = new Lifebloom(cacheCharacter, cacheStats, 3, false);
            lifebloomFastStack = new Lifebloom(cacheCharacter, cacheStats, 3, true);
            lifebloomSlow2Stack = new Lifebloom(cacheCharacter, cacheStats, 2, false);
            lifebloomFast2Stack = new Lifebloom(cacheCharacter, cacheStats, 2, true);
            lifebloomRollingStack = new LifebloomStack(cacheCharacter, cacheStats);

            rejuvenate = new Rejuvenation(cacheCharacter, cacheStats);
            nourish = new Nourish[5];
            nourish[0] = new Nourish(cacheCharacter, cacheStats, 0);
            nourish[1] = new Nourish(cacheCharacter, cacheStats, 1);
            nourish[2] = new Nourish(cacheCharacter, cacheStats, 2);
            nourish[3] = new Nourish(cacheCharacter, cacheStats, 3);
            nourish[4] = new Nourish(cacheCharacter, cacheStats, 4);
            healingTouch = new HealingTouch(cacheCharacter, cacheStats);
            wildGrowth = new WildGrowth(cacheCharacter, cacheStats);
            #endregion

            #region Add latency
            regrowth.latency = cacheLatency;
            regrowthAgain.latency = cacheLatency;
            lifebloom.latency = cacheLatency;
            lifebloomSlowStack.latency = cacheLatency;
            lifebloomFastStack.latency = cacheLatency;
            lifebloomSlow2Stack.latency = cacheLatency;
            lifebloomFast2Stack.latency = cacheLatency;
            lifebloomRollingStack.latency = cacheLatency;
            rejuvenate.latency = cacheLatency;
            nourish[0].latency = cacheLatency;
            nourish[1].latency = cacheLatency;
            nourish[2].latency = cacheLatency;
            nourish[3].latency = cacheLatency;
            nourish[4].latency = cacheLatency;
            healingTouch.latency = cacheLatency;
            wildGrowth.latency = cacheLatency;
            #endregion

            RejuvCF = copy.RejuvCF;
            RegrowthChainCast = copy.RegrowthChainCast;
            RegrowthCF = copy.RegrowthCF;
            LifebloomCF = copy.LifebloomCF;
            NourishCF = copy.NourishCF;
            nourishRawHPCT = copy.nourishRawHPCT;

            LifebloomStackType = copy.LifebloomStackType;
            lifebloomStacks = copy.lifebloomStacks;
            lifebloomStackCF = copy.lifebloomStackCF;
            WildGrowthCF = copy.WildGrowthCF;

            ValAnyrShield = copy.ValAnyrShield;

            CreateSwiftmend();
            SwiftmendCPM = copy.SwiftmendCPM;
        }

        public SpellMix(Character character, Stats stats, float latency)
        {
            cacheCharacter = character;
            cacheLatency = latency;
            cacheStats = stats;

            #region Setup Spells
            regrowth = new Regrowth(character, stats, false);
            regrowthAgain = new Regrowth(character, stats, true);
            lifebloom = new Lifebloom(character, stats);
            lifebloomSlowStack = new Lifebloom(character, stats, 3, false);
            lifebloomFastStack = new Lifebloom(character, stats, 3, true);
            lifebloomSlow2Stack = new Lifebloom(character, stats, 2, false);
            lifebloomFast2Stack = new Lifebloom(character, stats, 2, true);
            lifebloomRollingStack = new LifebloomStack(character, stats);

            rejuvenate = new Rejuvenation(character, stats);
            nourish = new Nourish[5];
            nourish[0] = new Nourish(character, stats, 0);
            nourish[1] = new Nourish(character, stats, 1);
            nourish[2] = new Nourish(character, stats, 2);
            nourish[3] = new Nourish(character, stats, 3);
            nourish[4] = new Nourish(character, stats, 4);
            healingTouch = new HealingTouch(character, stats);
            wildGrowth = new WildGrowth(character, stats);
            #endregion

            #region Add latency
            regrowth.latency = latency;
            regrowthAgain.latency = latency;
            lifebloom.latency = latency;
            lifebloomSlowStack.latency = latency;
            lifebloomFastStack.latency = latency;
            lifebloomSlow2Stack.latency = latency;
            lifebloomFast2Stack.latency = latency;
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

            RejuvCF = 0f;
            RegrowthChainCast = false;
            RegrowthCF = 0f;
            LifebloomCF = 0f;
            NourishCF = 0f;
            nourishRawHPCT = 0f;

            LifebloomStackType = LifeBloomType.Slow;
            lifebloomStacks = 0f;
            lifebloomStackCF = 0f;
            WildGrowthCF = 0f;

            if (stats.ShieldFromHealed > 0) ValAnyrShield = stats.ShieldFromHealed;

            CreateSwiftmend();
            SwiftmendCPM = 0f;
            RevitalizeChance = stats.RevitalizeChance;
        }

        public void ApplyCombatStats(Stats stats)
        {
            cacheStats = stats;
            rejuvenate.applyStats(stats);
            regrowth.applyStats(stats);
            regrowthAgain.applyStats(stats);
            lifebloom.applyStats(stats);
            lifebloomSlowStack.applyStats(stats);
            lifebloomFastStack.applyStats(stats);
            lifebloomRollingStack.applyStats(stats);
            nourish[0].applyStats(stats);
            nourish[1].applyStats(stats);
            nourish[2].applyStats(stats);
            nourish[3].applyStats(stats);
            nourish[4].applyStats(stats);
            healingTouch.applyStats(stats);
            wildGrowth.applyStats(stats);
            CreateSwiftmend();
        }

        #region Spells
        public Rejuvenation rejuvenate;
        public Regrowth regrowth;
        public Regrowth regrowthAgain;
        public Lifebloom lifebloom;
        public Lifebloom lifebloomSlowStack;
        public Lifebloom lifebloomFastStack;
        public Lifebloom lifebloomSlow2Stack;
        public Lifebloom lifebloomFast2Stack;
        public LifebloomStack lifebloomRollingStack;

        public Nourish[] nourish;
        public HealingTouch healingTouch;
        public WildGrowth wildGrowth;
        public Swiftmend swiftmend = null;
        #endregion

        public float HPS { get { return HotsHPS + WildGrowthHPS + SwiftmendHPS + NourishHPS + ValAnyrHPS; } }
        public float HPM { get { return HPS / MPS; } }
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
        public float RejuvHPS { get { return RejuvCF * rejuvenate.HPCT; } }
        public float RejuvMPS { get { return RejuvCPS * rejuvenate.ManaCost; } }
        public float RejuvHealsPerMinute { get { return RejuvCPM * rejuvenate.PeriodicTicks; } }
        #endregion

        #region Regrowth
        public float RegrowthCF = 0f;
        private bool regrowthChainCast = false;
        public bool RegrowthChainCast { get { return regrowthChainCast; } set { regrowthChainCast = value; regrowthSpell = value ? regrowthAgain : regrowth; } }
        private Spell regrowthSpell = null;
        public float RegrowthCPS { get { return RegrowthCF / regrowthSpell.CastTime; } }
        public float RegrowthCPM { get { return 60f * RegrowthCF / regrowthSpell.CastTime; } }
        public float RegrowthAvg { get { return RegrowthCPS * regrowthSpell.Duration; } }
        public float RegrowthHPS { get { return RegrowthCF * (regrowthChainCast ? regrowthSpell.HPCT_DH : regrowthSpell.HPCT); } }
        public float RegrowthMPS { get { return RegrowthCPS * regrowthSpell.ManaCost; } }
        public float RegrowthHealsPerMinute { get { return RegrowthCPM * (1f + (regrowthChainCast ? 0 : regrowthSpell.PeriodicTicks)); } }
        public float RegrowthCritsPerMinute { get { return RegrowthCPM * regrowthSpell.CritPercent / 100f; } }
        #endregion

        #region Lifebloom
        public float LifebloomCF = 0f;
        public float LifebloomCPS { get { return LifebloomCF / lifebloom.CastTime; } }
        public float LifebloomCPM { get { return 60f * LifebloomCF / lifebloom.CastTime; } }
        public float LifebloomAvg { get { return LifebloomCPS * lifebloom.Duration; } }
        public float LifebloomHPS { get { return LifebloomCF * lifebloom.HPCT; } }
        public float LifebloomMPS { get { return LifebloomCPS * lifebloom.ManaCost; } }
        public float LifebloomHealsPerMinute { get { return LifebloomCPM * (1f + lifebloom.PeriodicTicks); } }
        public float LifebloomCritsPerMinute { get { return LifebloomCPM * lifebloom.CritPercent / 100f; } }
        #endregion

        #region Nourish
        public float NourishCF = 0f;
        public float NourishHPS { get { return NourishCF * nourishRawHPCT; } }
        public float NourishMPS { get { return NourishCPS * nourish[0].ManaCost; } }
        public float NourishCPS { get { return NourishCF / nourish[0].CastTime; } }
        public float NourishCPM { get { return 60f * NourishCF / nourish[0].CastTime; } }
        public float NourishHealsPerMinute { get { return NourishCPM; } }
        public float NourishCritsPerMinute { get { return NourishHealsPerMinute * nourish[0].CritPercent / 100f; } }

        private float nourishRawHPCT = 0f;
        public void CalculateNourishHPS(float hots0, float hots1, float hots2, float hots3, float hots4)
        {
            nourishRawHPCT = (hots0 * nourish[0].HPCT_DH
                + hots1 * nourish[1].HPCT_DH
                + hots2 * nourish[2].HPCT_DH
                + hots3 * nourish[3].HPCT_DH
                + hots4 * nourish[4].HPCT_DH);
        }
        #endregion

        #region Lifebloom stacks
        private LifeBloomType lifebloomStackType;
        public LifeBloomType LifebloomStackType
        {
            get { return lifebloomStackType; }
            set
            {
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
                    case LifeBloomType.Slow2:
                        lifebloomStackSpell = lifebloomSlow2Stack;
                        lifebloomStackDuration = lifebloomStackSpell.Duration + 1f; // need a second before reapplying
                        break;
                    case LifeBloomType.Fast2:
                        lifebloomStackSpell = lifebloomFast2Stack;
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
        public Spell LifebloomStackSpell { get { return lifebloomStackSpell; } } 
        private float lifebloomStackDuration;

        private float lifebloomStacks = 0;
        public float LifebloomStacks { get { return lifebloomStacks; } set { lifebloomStacks = value; lifebloomStackCF = -1f; } }

        private float lifebloomStackCF = -1f;
        public float LifebloomStackCF
        {
            get
            {
                if (lifebloomStackCF != -1f) return lifebloomStackCF;
                return lifebloomStackSpell.CastTime * LifebloomStacks / lifebloomStackDuration;
            }
            set { lifebloomStackCF = value; }
        }
        public float LifebloomStackCPS
        {
            get
            {
                return lifebloomStackSpell.NumberOfCasts * LifebloomStackCF / lifebloomStackSpell.CastTime;
            }
        }
        public float LifebloomStackCPM
        {
            get
            {
                return 60f * LifebloomStackCPS;
            }
        }
        public float LifebloomStackAvg
        {
            get
            {
                return LifebloomStackCPS / lifebloomStackSpell.NumberOfCasts * lifebloomStackSpell.Duration;
            }
        }
        public float LifebloomStackBPS
        {
            get
            {
                return lifebloomStackType == LifeBloomType.Rolling ? 0 : LifebloomStackCPS / lifebloomStackSpell.NumberOfCasts;
            }
        }
        public float LifebloomStackHPS
        {
            get
            {
                return LifebloomStackAvg * lifebloomStackSpell.HPS_HOT + LifebloomStackBPS * lifebloomStackSpell.AverageHealingwithCrit;
            }
        }
        public float LifebloomStackMPS
        {
            get
            {
                return LifebloomStackCPS * lifebloomStackSpell.ManaCost;
            }
        }
        public float LifebloomStackHealsPerMinute
        {
            get
            {
                return 60f * LifebloomStackBPS * (1f + lifebloomStackSpell.PeriodicTicks);
            }
        }
        public float LifebloomStackCritsPerMinute
        {
            get
            {
                return lifebloomStackType == LifeBloomType.Rolling ? 0 :
                    60f * LifebloomStackBPS * lifebloomStackSpell.CritPercent / 100f;
            }
        }
        public float LifebloomStackHPS_DH { get { return LifebloomStackBPS * lifebloomStackSpell.AverageHealingwithCrit; } }
        public float LifebloomStackHPS_HOT { get { return LifebloomStackAvg * lifebloomStackSpell.HPS_HOT; } }
        #endregion

        #region Wild Growth
        private float wildgrowthCF = 0f;
        public float WildGrowthCF { get { return wildgrowthCF; } set { if (cacheCharacter.DruidTalents.WildGrowth > 0) wildgrowthCF = value; else wildgrowthCF = 0; } }
        public float WildGrowthCPS { get { return WildGrowthCF / wildGrowth.CastTime; } }
        public float WildGrowthCPM { get { return 60f * WildGrowthCPS; } set { WildGrowthCF = value * (float)wildGrowth.CastTime / 60f; } }
        public float WildGrowthAvg { get { return WildGrowthCPS * wildGrowth.Duration; } }
        public float WildGrowthHPS { get { return WildGrowthAvg * wildGrowth.maxTargets * wildGrowth.PeriodicTick; } }
        public float WildGrowthMPS { get { return WildGrowthCPS * wildGrowth.ManaCost; } }
        public float WildGrowthHealsPerMinute { get { return WildGrowthCPM * wildGrowth.maxTargets * wildGrowth.PeriodicTicks; } }
        #endregion

        #region Swiftmend
        public void CreateSwiftmend()
        {
            swiftmend = new Swiftmend(cacheCharacter, cacheStats, RejuvCF>0?rejuvenate:null, RegrowthCF>0?regrowth:null);
            swiftmend.latency = cacheLatency;
        }
        public float SwiftmendCF
        {
            get
            {
                if (swiftmend == null) return 0;
                float cf = SwiftmendCPS * swiftmend.CastTime;
                cf += swiftmend.rejuvUseChance * swiftmend.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.CastTime * SwiftmendCPS;
                cf += swiftmend.regrowthUseChance * swiftmend.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.CastTime * SwiftmendCPS;
                return cf;
            }
        }
        public float SwiftmendCPS { get { return SwiftmendCPM / 60f; } }
        private float swiftmendCPM;
        public float SwiftmendCPM { get { return swiftmendCPM; } set { if (cacheCharacter.DruidTalents.Swiftmend > 0) swiftmendCPM = value; else swiftmendCPM = 0; } }
        public float SwiftmendAvg { get { return swiftmend == null ? 0 : SwiftmendCPS * swiftmend.Duration; } }
        public float SwiftmendHPS
        {
            get
            {
                if (swiftmend == null) return 0;
                float hps = swiftmend.TotalAverageHealing * SwiftmendCPS;
                hps += swiftmend.regrowthUseChance * swiftmend.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.AverageHealingwithCrit * SwiftmendCPS;
                return hps;
            }
        }

        public float SwiftmendMPS
        {
            get
            {
                if (swiftmend == null) return 0;
                float mps = SwiftmendCPS * swiftmend.ManaCost;
                mps += swiftmend.rejuvUseChance * swiftmend.rejuvTicksLost / rejuvenate.PeriodicTicks * rejuvenate.ManaCost * SwiftmendCPS;
                mps += swiftmend.regrowthUseChance * swiftmend.regrowthTicksLost / regrowth.PeriodicTicks * regrowth.ManaCost * SwiftmendCPS;
                return mps;
            }
        }
        public float SwiftmendHealsPerMinute { get { return SwiftmendCPM; } }
        public float SwiftmendCritsPerMinute { get { return swiftmend == null ? 0 : SwiftmendCPM * swiftmend.CritPercent / 100.0f; } }
        #endregion

        public float TotalCF { get { return (float)Math.Round(HotsCF + WildGrowthCF + SwiftmendCF + NourishCF, 4); } }
        public float IdleCF { get { return 1f - TotalCF; } }

        public float ValAnyrShield = 0;
        public float ValAnyrHPS { get { return ValAnyrShield * (HotsHPS + WildGrowthHPS + SwiftmendHPS + NourishHPS); } }

        public float RejuvenationHealsPerMinute { get { return RejuvHealsPerMinute; } }

        public float RevitalizeChance = 0f;
        public float RevitalizeProcsPerMinute { get { return RevitalizeChance * (RejuvCPM * rejuvenate.PeriodicTicks + WildGrowthCPM * wildGrowth.PeriodicTicks * wildGrowth.maxTargets); } }
    }

    public class SustainedResult
    {
        public SustainedResult(CharacterCalculationsTree calculatedStats, Stats stats, float fightLength, float latency)
        {
            spellMix = new SpellMix(calculatedStats.LocalCharacter, stats, latency);

            #region Setup Mana regen
            SpiritMPS = StatConversion.GetSpiritRegenSec(stats.Intellect, stats.Spirit);
            replenishment = stats.ManaRestoreFromMaxManaPerSecond; 
            Mana = stats.Mana;
            GearMPS = stats.Mp5 / 5f;
            SpiritInCombatFraction = stats.SpellCombatManaRegeneration;
            ProcsMPS = stats.ManaRestore / fightLength;
            #endregion
        }

        public void ApplyCombatStats(Stats stats) 
        {
            spellMix.ApplyCombatStats(stats);
        }

        public SpellMix spellMix;

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
        public float ExtraMana { get { return InnervateMana; } }
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

        public float EffMPS { get { return spellMix.MPS - ManaRegen; } }

        public float TotalModifier { get { return TotalTime > TimeToOOM ? (TimeToOOM + BurnRegenFraction * TimeAfterOOM) / TotalTime : 1f; } }
        public float TimeToRegenAll { get { return Mana / MPSOutFSR; } }
        public float TimeToBurnAll { get { return Mana / EffMPS; } }
        public float BurnRegenFraction { get { return TimeToBurnAll / (TimeToRegenAll + TimeToBurnAll); } }

        public float TotalHealing { get { return spellMix.HPS * TotalTime * TotalModifier; } }
        public float TotalCastsPerMinute { get { return spellMix.CastsPerMinute * TotalModifier; } }
        public float TotalCritsPerMinute { get { return spellMix.CritsPerMinute * TotalModifier; } }
        public float TotalHealsPerMinute { get { return spellMix.HealsPerMinute * TotalModifier; } }

        public RotationSettings rotSettings;
    }

    public enum SingleTargetBurstRotations
    {
        AutoSelect = 0,
        AutoSelectForceSwiftmend,
        RjN,
        RjLbN,
        RjRgN,
        RjRgLbN,
        RjRg,
        RjLbRg,
        Rg,
        RgN,        
        N,
        RjN_s,
        RjLbN_s,
        RjRgN_s,
        RjRgLbN_s,
        RjRg_s,
        RjLbRg_s,
        Rg_s,
        RgN_s
    };

    public enum HealTargetTypes { TankHealing = 0, RaidHealing };

    public enum SpellList { HealingTouch = 0, Nourish, Regrowth, Rejuvenation };

    public enum LifeBloomType { Slow = 0, Fast, Rolling, Slow2, Fast2 };

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
        public SpellMix spellMix;
        public float HPS { get { return spellMix.HPS; } }
        public float HPM { get { return spellMix.HPM; } }

        public SingleTargetBurstRotations rotation;

        public SingleTargetBurstResult(SingleTargetBurstRotations rotation, SpellMix spells, 
            float rjCF, float rgCF, float lbCF, float smCPM, float nCF, bool rgAgain)
        {
            spellMix = new SpellMix(spells);
            spellMix.RegrowthChainCast = rgAgain;

            spellMix.RejuvCF = rjCF;
            spellMix.RegrowthCF = rgCF;
            spellMix.LifebloomStackCF = lbCF;

            spellMix.CreateSwiftmend();
            spellMix.SwiftmendCPM = smCPM;

            spellMix.NourishCF = nCF;

            int numberOfHots = 0
                + (rjCF > 0 ? 1 : 0)
                + (rgCF > 0 ? 1 : 0)
                + (lbCF > 0 ? 1 : 0);
            if (numberOfHots == 0) spellMix.CalculateNourishHPS(1, 0, 0, 0, 0);
            if (numberOfHots == 1) spellMix.CalculateNourishHPS(0, 1, 0, 0, 0);
            if (numberOfHots == 2) spellMix.CalculateNourishHPS(0, 0, 1, 0, 0);
            if (numberOfHots == 3) spellMix.CalculateNourishHPS(0, 0, 0, 1, 0);
            if (numberOfHots == 4) spellMix.CalculateNourishHPS(0, 0, 0, 0, 1);

            this.rotation = rotation;
        }
    }

    public class Solver
    {
        public static SustainedResult SimulateHealing(CharacterCalculationsTree calculatedStats, Stats stats, CalculationOptionsTree calcOpts, RotationSettings rotSettings)
        {
            SpellProfile profile = calcOpts.Current;

            SustainedResult rot = new SustainedResult(calculatedStats, stats, profile.FightDuration, rotSettings.latency);

            #region Setup spell mix
            rot.spellMix.LifebloomStackType = rotSettings.lifeBloomType;
            rot.spellMix.LifebloomStacks = rotSettings.averageLifebloomStacks;
            float maintainedRejuvCF = rotSettings.averageRejuv * rot.spellMix.rejuvenate.CastTime / rot.spellMix.rejuvenate.Duration;
            float maintainedRegrowthCF = rotSettings.averageRegrowth * rot.spellMix.regrowth.CastTime / rot.spellMix.regrowth.Duration;
            rot.spellMix.RejuvCF = rotSettings.RejuvFraction + maintainedRejuvCF;
            rot.spellMix.RegrowthCF = rotSettings.RegrowthFraction + maintainedRegrowthCF;
            rot.spellMix.LifebloomCF = rotSettings.LifebloomFraction;
            rot.spellMix.NourishCF = rotSettings.NourishFraction;
            rot.spellMix.WildGrowthCPM = rotSettings.WildGrowthPerMin;
            rot.spellMix.CreateSwiftmend();
            rot.spellMix.SwiftmendCPM = rotSettings.SwiftmendPerMin;
            rot.spellMix.CalculateNourishHPS(rotSettings.nourish0, rotSettings.nourish1,
                rotSettings.nourish2, rotSettings.nourish3, rotSettings.nourish4);
            #endregion

            rot.rotSettings = rotSettings;

            rot.TotalTime = profile.FightDuration;

            #region Mana regeneration
            rot.ReplenishmentUptime = profile.ReplenishmentUptime / 100f; 
            rot.OutOfCombatFraction = 0f;
            rot.RevitalizePPM = (float)profile.RevitalizePPM / 100f;
            #endregion

            #region Innervates
            // 3 min CD, takes 15 seconds to apply
            rot.NumberOfInnervates = (float)Math.Ceiling((profile.FightDuration - 15f) / 180f);
            rot.ManaPerInnervate = 2.25f * profile.Innervates;       // Use self innervate?
            if (calculatedStats.LocalCharacter.DruidTalents.GlyphOfInnervate) rot.ManaPerInnervate += 0.45f;
            rot.ManaPerInnervate *= Rawr.Tree.TreeConstants.BaseMana;
            #endregion

            #region Correct cast fractions 
            rot.RejuvCF_unreduced = rot.spellMix.RejuvCF;
            rot.RegrowthCF_unreduced = rot.spellMix.RegrowthCF;
            rot.LifebloomCF_unreduced = rot.spellMix.LifebloomCF;
            rot.NourishCF_unreduced = rot.spellMix.NourishCF;
            rot.WildGrowthCF_unreduced = rot.spellMix.WildGrowthCF;
            rot.SwiftmendCF_unreduced = rot.spellMix.SwiftmendCF;
            rot.LifebloomStackCF_unreduced = rot.spellMix.LifebloomStackCF;

            // Priority:
            // 1. Maintained Hots
            // 2. Wild growth and Swiftmend (Swiftmend cannot be reduced, only enabled/disabled)
            // 3. Spells not to be adjusted
            // 4. Idle time
            // 5. Spells to be adjusted

            float IdleCF = profile.IdleCastTimePercent / 100f;
            rot.IdleCF_unreduced = IdleCF;

            if (rot.spellMix.TotalCF + IdleCF > 1f)
            {
                float Static = maintainedRegrowthCF + maintainedRejuvCF + rot.spellMix.LifebloomStackCF + rot.spellMix.WildGrowthCF + rot.spellMix.SwiftmendCF;
                if (!profile.AdjustLifebloom) Static += rot.spellMix.LifebloomCF;
                if (!profile.AdjustNourish) Static += rot.spellMix.NourishCF;
                if (!profile.AdjustRejuv) Static += (rot.spellMix.RejuvCF - maintainedRejuvCF);
                if (!profile.AdjustRegrowth) Static += (rot.spellMix.RegrowthCF - maintainedRejuvCF);
                float Factor = Math.Max(0f, (1f - Static - IdleCF) / (rot.spellMix.TotalCF - Static));
                if (profile.AdjustNourish) rot.spellMix.NourishCF *= Factor;
                if (profile.AdjustRejuv) rot.spellMix.RejuvCF = (rot.spellMix.RejuvCF - maintainedRejuvCF) * Factor + maintainedRejuvCF;
                if (profile.AdjustRegrowth) rot.spellMix.RegrowthCF = (rot.spellMix.RegrowthCF - maintainedRegrowthCF) * Factor + maintainedRegrowthCF;
                if (profile.AdjustLifebloom) rot.spellMix.LifebloomCF *= Factor;
            }

            if (rot.spellMix.TotalCF + IdleCF > 1f)
            {
                float Static = maintainedRegrowthCF + maintainedRejuvCF + rot.spellMix.LifebloomStackCF + rot.spellMix.WildGrowthCF + rot.spellMix.SwiftmendCF;
                float Factor = Math.Max(0f, (1f - Static - IdleCF) / (rot.spellMix.TotalCF - Static));
                rot.spellMix.NourishCF *= Factor;
                rot.spellMix.RejuvCF = (rot.spellMix.RejuvCF - maintainedRejuvCF) * Factor + maintainedRejuvCF;
                rot.spellMix.RegrowthCF = (rot.spellMix.RegrowthCF - maintainedRegrowthCF) * Factor + maintainedRegrowthCF;
                rot.spellMix.LifebloomCF *= Factor;
            }

            // Remove Swiftmend if not enough HoTs and rj/rg are not primary heals
            //if (rot.RejuvAvg + rot.RegrowthAvg < rot.SwiftmendCPM && !(rot.PrimaryHeal is Rejuvenation || rot.PrimaryHeal is Regrowth)) rot.SwiftmendCPM = 0f;

            // Next, try to reduce wild growth and swiftmend
            if (rot.spellMix.TotalCF > 1f)
            {
                rot.spellMix.SwiftmendCPM = 0;
                float Static = rot.spellMix.LifebloomStackCF + rot.spellMix.RejuvCF + rot.spellMix.RegrowthCF;
                float Factor = Math.Min(1f, Math.Max(0f, (1f - Static) / (rot.spellMix.TotalCF - Static)));
                // Rejuv/Regrowth/Lifebloom are already 0 at this point
                rot.spellMix.WildGrowthCF *= Factor;
            }

            // Finally, try to reduce lifebloom stacks
            if (rot.spellMix.TotalCF > 1f)
            {
                float Factor = 1f / rot.spellMix.TotalCF;
                rot.spellMix.LifebloomStackCF *= Factor;
                rot.spellMix.RejuvCF *= Factor;
                rot.spellMix.RegrowthCF *= Factor;
                // Now, rot.MaxPrimaryCF *must* be 1f exactly.
            }

            #endregion

            #region Correct going OOM
            rot.TimeToOOM_unreduced = rot.TimeToOOM;
            rot.IdleCF_unreducedOOM = rot.spellMix.IdleCF;
            rot.RejuvCF_unreducedOOM = rot.spellMix.RejuvCF;
            rot.RegrowthCF_unreducedOOM = rot.spellMix.RegrowthCF;
            rot.LifebloomCF_unreducedOOM = rot.spellMix.LifebloomCF;
            rot.NourishCF_unreducedOOM = rot.spellMix.NourishCF;
            rot.WildGrowthCF_unreducedOOM = rot.spellMix.WildGrowthCF;

            // Based on MPS (and possibly HPM).
            // Actually, if you cast less, you will also see less returns. This is calculated elsewhere
            // in the Special Effects. Repeated calculations are necessary.
            // We assume this effect is convergent.

            float f = Math.Max(0.0f, (rot.TimeToOOM * (rot.spellMix.MPS - rot.ManaRegen) + rot.TotalTime * rot.ManaRegen) / (rot.spellMix.MPS * rot.TotalTime));
            // We want our MPS to become f*MPS

            float MPStoGain = rot.spellMix.MPS * (1f - f);

            for (int i = 0; i < 5; i++)
            {
                if (profile.ReduceOOMRejuvOrder == i)
                {
                    float r = Math.Min(1f, (MPStoGain / rot.spellMix.rejuvenate.ManaCost * rot.spellMix.rejuvenate.CastTime));
                    rot.spellMix.RejuvCF = Math.Max(rot.spellMix.RejuvCF * (1f - profile.ReduceOOMRejuv / 100f), rot.spellMix.RejuvCF - r);
                    MPStoGain -= rot.spellMix.rejuvenate.ManaCost * (rot.RejuvCF_unreducedOOM - rot.spellMix.RejuvCF) / rot.spellMix.rejuvenate.CastTime;
                }
                else if (profile.ReduceOOMRegrowthOrder == i)
                {
                    float r = Math.Min(1f, (MPStoGain / rot.spellMix.regrowth.ManaCost * rot.spellMix.regrowth.CastTime));
                    rot.spellMix.RegrowthCF = Math.Max(rot.spellMix.RegrowthCF * (1f - profile.ReduceOOMRegrowth / 100f), rot.spellMix.RegrowthCF - r);
                    MPStoGain -= rot.spellMix.regrowth.ManaCost * (rot.RegrowthCF_unreducedOOM - rot.spellMix.RegrowthCF) / rot.spellMix.regrowth.CastTime;
                }
                else if (profile.ReduceOOMLifebloomOrder == i)
                {
                    float r = Math.Min(1f, (MPStoGain / rot.spellMix.lifebloom.ManaCost * rot.spellMix.lifebloom.CastTime));
                    rot.spellMix.LifebloomCF = Math.Max(rot.spellMix.LifebloomCF * (1f - profile.ReduceOOMLifebloom / 100f), rot.spellMix.LifebloomCF - r);
                    MPStoGain -= rot.spellMix.lifebloom.ManaCost * (rot.LifebloomCF_unreducedOOM - rot.spellMix.LifebloomCF) / rot.spellMix.lifebloom.CastTime;
                }
                else if (profile.ReduceOOMNourishOrder == i)
                {
                    float r = Math.Min(1f, (MPStoGain / rot.spellMix.nourish[0].ManaCost * rot.spellMix.nourish[0].CastTime));
                    rot.spellMix.NourishCF = Math.Max(rot.spellMix.NourishCF * (1f - profile.ReduceOOMNourish / 100f), rot.spellMix.NourishCF - r);
                    MPStoGain -= rot.spellMix.nourish[0].ManaCost * (rot.NourishCF_unreducedOOM - rot.spellMix.NourishCF) / rot.spellMix.nourish[0].CastTime;
                }
                else if (profile.ReduceOOMWildGrowthOrder == i)
                {
                    float r = Math.Min(1f, (MPStoGain / rot.spellMix.wildGrowth.ManaCost * rot.spellMix.wildGrowth.CastTime));
                    rot.spellMix.WildGrowthCF = Math.Max(rot.spellMix.WildGrowthCF * (1f - profile.ReduceOOMWildGrowth / 100f), rot.spellMix.WildGrowthCF - r);
                    MPStoGain -= rot.spellMix.wildGrowth.ManaCost * (rot.WildGrowthCF_unreducedOOM - rot.spellMix.WildGrowthCF) / rot.spellMix.wildGrowth.CastTime;
                }
            }
            #endregion

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
                case SingleTargetBurstRotations.RjRgN:
                    return "RjRgN*";
                case SingleTargetBurstRotations.RjRgLbN:
                    return "RjRgLbN*";
                case SingleTargetBurstRotations.RjRg:
                    return "RjRg*";
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
                case SingleTargetBurstRotations.RjRgN_s:
                    return "RjRgN*+S";
                case SingleTargetBurstRotations.RjRgLbN_s:
                    return "RjRgLbN*+S";
                case SingleTargetBurstRotations.RjRg_s:
                    return "RjRg*+S";
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
                    return SingleTargetBurstRotations.RjRgN;
                case 5:
                    return SingleTargetBurstRotations.RjRgLbN;
                case 6:
                    return SingleTargetBurstRotations.RjRg;
                case 7:
                    return SingleTargetBurstRotations.RjLbRg;
                case 8:
                    return SingleTargetBurstRotations.Rg;
                case 9:
                    return SingleTargetBurstRotations.RgN;
                case 10:
                    return SingleTargetBurstRotations.N;
                case 11:
                    return SingleTargetBurstRotations.RjN_s;
                case 12:
                    return SingleTargetBurstRotations.RjLbN_s;
                case 13:
                    return SingleTargetBurstRotations.RjRgN_s;
                case 14:
                    return SingleTargetBurstRotations.RjRgLbN_s;
                case 15:
                    return SingleTargetBurstRotations.RjRg_s;
                case 16:
                    return SingleTargetBurstRotations.RjLbRg_s;
                case 17:
                    return SingleTargetBurstRotations.Rg_s;
                case 18:
                    return SingleTargetBurstRotations.RgN_s;
            }
        }

        public static SingleTargetBurstResult[] CalculateSingleTargetBurst(CharacterCalculationsTree calculatedStats, Stats stats, CalculationOptionsTree calcOpts, SingleTargetBurstRotations rotation)
        {
            SpellMix spellMix = new SpellMix(calculatedStats.LocalCharacter, stats, 0f); // Latency

            SpellProfile profile = calcOpts.Current;

            switch (profile.LifebloomStackType)
            {
                case 0:
                    spellMix.LifebloomStackType = LifeBloomType.Slow;
                    break;
                case 1:
                    spellMix.LifebloomStackType = LifeBloomType.Fast;
                    break;
                case 2:
                    spellMix.LifebloomStackType = LifeBloomType.Rolling ;
                    break;
                case 3:
                    spellMix.LifebloomStackType = LifeBloomType.Slow2;
                    break;
                case 4:
                    spellMix.LifebloomStackType = LifeBloomType.Fast2;
                    break;
                default:
                    // err...
                    spellMix.LifebloomStackType = LifeBloomType.Rolling;
                    break;
            }

            float RejuvFraction = spellMix.rejuvenate.CastTime / spellMix.rejuvenate.Duration;
            float RegrowthFraction = spellMix.regrowth.CastTime / spellMix.regrowth.Duration;
            float LifebloomFraction = spellMix.LifebloomStackSpell.CastTime / spellMix.LifebloomStackSpell.Duration;
            float SwiftFraction = spellMix.swiftmend.CastTime * 4f / 60f;

            // RJ + N1 
            SingleTargetBurstResult RjN1 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjN,
                spellMix, RejuvFraction, 0, 0, 0, (1f - RejuvFraction), false);
            // RJ + LB + N2 
            SingleTargetBurstResult RjLbN2 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbN,
                spellMix, RejuvFraction, 0, LifebloomFraction, 0, (1f - RejuvFraction - LifebloomFraction), false);
            // RJ + RG + N2
            SingleTargetBurstResult RjRgN2 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRgN,
                spellMix, RejuvFraction, RegrowthFraction, 0, 0, (1f - RejuvFraction - RegrowthFraction), false);
            // RJ + RG + LB + N3 
            SingleTargetBurstResult RjRgLbN3 = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRgLbN,
                spellMix, RejuvFraction, RegrowthFraction, LifebloomFraction, 0,
                (1f - RejuvFraction - RegrowthFraction - LifebloomFraction), false);
            // RJ + RG 
            SingleTargetBurstResult RjRg = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRg,
                spellMix, RejuvFraction, (1f - RejuvFraction), 0, 0, 0, true);
            // RJ + LB + RG
            SingleTargetBurstResult RjLbRg = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbRg,
                spellMix, RejuvFraction, (1f - RejuvFraction - LifebloomFraction), LifebloomFraction, 0, 0, true);
            // RG
            SingleTargetBurstResult Rg = new SingleTargetBurstResult(SingleTargetBurstRotations.Rg,
                spellMix, 0, 1f, 0, 0, 0, true);
            // RG + N1
            SingleTargetBurstResult RgN1 = new SingleTargetBurstResult(SingleTargetBurstRotations.RgN,
                spellMix, 0, RegrowthFraction, 0, 0, (1f - RegrowthFraction), false);
            // N0
            SingleTargetBurstResult N0 = new SingleTargetBurstResult(SingleTargetBurstRotations.N,
                spellMix, 0, 0, 0, 0, 1f, false);

            // RJ + N1 S
            SingleTargetBurstResult RjN1S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjN_s,
                spellMix, RejuvFraction, 0, 0, 4, (1f - RejuvFraction - SwiftFraction), false);
            // RJ + LB + N2 S
            SingleTargetBurstResult RjLbN2S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbN_s,
                spellMix, RejuvFraction, 0, LifebloomFraction, 4,
                (1f - RejuvFraction - LifebloomFraction - SwiftFraction), false);
            // RJ + RG + N2 S
            SingleTargetBurstResult RjRgN2S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRgN_s,
                spellMix, RejuvFraction, RegrowthFraction, 0, 4,
                (1f - RejuvFraction - RegrowthFraction - SwiftFraction), false);
            // RJ + RG + LB + N3 S
            SingleTargetBurstResult RjRgLbN3S = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRgLbN_s,
                spellMix, RejuvFraction, RegrowthFraction, LifebloomFraction, 4,
                (1f - RejuvFraction - RegrowthFraction - LifebloomFraction - SwiftFraction), false);
            // RJ + RG S
            SingleTargetBurstResult RjRgS = new SingleTargetBurstResult(SingleTargetBurstRotations.RjRg_s,
                spellMix, RejuvFraction, (1f - RejuvFraction - SwiftFraction), 0, 4, 0, true);
            // RJ + LB + RG S
            SingleTargetBurstResult RjLbRgS = new SingleTargetBurstResult(SingleTargetBurstRotations.RjLbRg_s,
                spellMix, RejuvFraction, (1f - RejuvFraction - LifebloomFraction - SwiftFraction),
                LifebloomFraction, 4, 0, true);
            // RG S
            SingleTargetBurstResult RgS = new SingleTargetBurstResult(SingleTargetBurstRotations.Rg_s, 
                spellMix, 0, (1f - SwiftFraction), 0, 4, 0, true);
            // RG + N1 S
            SingleTargetBurstResult RgN1S = new SingleTargetBurstResult(SingleTargetBurstRotations.RgN_s,
                spellMix, 0, RegrowthFraction, 0, 4, (1f - RegrowthFraction - SwiftFraction), false);

            SingleTargetBurstResult result = new SingleTargetBurstResult(SingleTargetBurstRotations.AutoSelect, 
                spellMix, 0, 0, 0, 0, 0, false);

            if (rotation == SingleTargetBurstRotations.AutoSelect ||
                rotation == SingleTargetBurstRotations.AutoSelectForceSwiftmend)
            {
                if (rotation != SingleTargetBurstRotations.AutoSelectForceSwiftmend)
                {
                    if (RjN1.HPS > result.HPS) result = RjN1;
                    if (RjLbN2.HPS > result.HPS) result = RjLbN2;
                    if (RjRgN2.HPS > result.HPS) result = RjRgN2;
                    if (RjRgLbN3.HPS > result.HPS) result = RjRgLbN3;
                    if (RjRg.HPS > result.HPS) result = RjRg;
                    if (RjLbRg.HPS > result.HPS) result = RjLbRg;
                    if (Rg.HPS > result.HPS) result = Rg;
                    if (N0.HPS > result.HPS) result = N0;
                }

                if (RjN1S.HPS > result.HPS) result = RjN1S;
                if (RjLbN2S.HPS > result.HPS) result = RjLbN2S;
                if (RjRgN2S.HPS > result.HPS) result = RjRgN2S;
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
                    case SingleTargetBurstRotations.RjRgN:
                        result = RjRgN2;
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
                    case SingleTargetBurstRotations.RjRgN_s:
                        result = RjRgN2S;
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

            List<SingleTargetBurstResult> rots = new List<SingleTargetBurstResult>();
            rots.Add(result);
            rots.Add(result);
            rots.Add(RjN1);
            rots.Add(RjLbN2);
            rots.Add(RjRgN2);
            rots.Add(RjRgLbN3);
            rots.Add(RjRg);
            rots.Add(RjLbRg);
            rots.Add(Rg);
            rots.Add(RgN1);
            rots.Add(N0);
            rots.Add(RjN1S);
            rots.Add(RjLbN2S);
            rots.Add(RjRgN2S);
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
