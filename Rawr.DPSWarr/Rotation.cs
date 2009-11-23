using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class Rotation {
        // Constructors
        public Rotation()
        {
            Char = null;
            StatS = null;
            Talents = null;
            CombatFactors = null;
            CalcOpts = null;
        }

        #region Variables
       
        public float _HPS_TTL;
        public string GCDUsage = "";
        protected CharacterCalculationsDPSWarr calcs = null;
        
        public bool _needDisplayCalcs = true;
        
        protected float FightDuration;
        protected float TimeLostGDCs;
        protected float RageGainedWhileMoving;
        public float TimesFeared = 0f;

        public float _Thunder_GCDs = 0f, _TH_DPS = 0f, _TH_HPS = 0f;
        protected float _ZRage_GCDs = 0f, _ZRage_HPS = 0f;
        protected float _Battle_GCDs = 0f, _Battle_HPS = 0f;
        protected float _Comm_GCDs = 0f, _Comm_HPS = 0f;
        protected float _Demo_GCDs = 0f, _Demo_HPS = 0f;
        protected float _Sunder_GCDs = 0f, _Sunder_HPS = 0f;
        protected float _Ham_GCDs = 0f, _Ham_DPS = 0f, _Ham_HPS = 0f;
        protected float _ER_GCDs = 0f, _ER_HPS = 0f;
        public float _Shatt_GCDs = 0f, _Shatt_DPS = 0f, _Shatt_HPS = 0f;
        protected float _Death_GCDs = 0f, _Death_HPS = 0f;
        protected float _Reck_GCDs = 0f, _Reck_HPS = 0f;
        public float _EX_GCDs = 0f, _EX_DPS = 0f, _EX_HPS = 0f;
        protected float _SW_DPS = 0f, _SW_HPS = 0f, _SW_GCDs = 0f;
        protected float RageGenWhite = 0f; protected float RageGenOther = 0f; protected float RageNeeded = 0f;
        public float _HS_PerHit = 0f, _HS_DPS = 0f, _HS_Acts = 0f;
        public float _CL_PerHit = 0f, _CL_DPS = 0f, _CL_Acts = 0f;
        protected float _WhiteDPSMH = 0f; protected float _WhiteDPSOH = 0f;
        protected float _WhiteDPS = 0f; protected float _WhitePerHit = 0f;
        public float _DW_PerHit = 0f, _DW_DPS = 0f;
        #region Abilities
        // Anti-DeBuff
        public Skills.HeroicFury HF;
        public Skills.EveryManForHimself EM;
        public Skills.Charge CH;
        public Skills.Intercept IN;
        public Skills.Intervene IV;
        // Rage Generators
        public Skills.SecondWind SndW;
        public Skills.BerserkerRage BZ;
        public Skills.Bloodrage BR;
        // Maintenance
        public Skills.BattleShout BTS;
        public Skills.CommandingShout CS;
        public Skills.DemoralizingShout DS;
        public Skills.SunderArmor SN;
        public Skills.ThunderClap TH;
        public Skills.Hamstring HMS;
        public Skills.EnragedRegeneration ER;
        // Periodics
        public Skills.ShatteringThrow ST;
        public Skills.SweepingStrikes SW;
        public Skills.DeathWish Death;
        public Skills.Recklessness RK;
        // Fury that's shared with Arms
        public Skills.WhirlWind WW;
        // Arms that's shared with Fury
        public Skills.Slam SL;
        public float _SL_DPS = 0f, _SL_HPS = 0f, _SL_GCDs = 0f;
        public float _WW_DPS = 0f, _WW_HPS = 0f, _WW_GCDs = 0f;
        
        // Generic
        public Skills.DeepWounds DW;
        public Skills.Cleave CL;
        public Skills.HeroicStrike HS;
        public Skills.Execute EX;
        #endregion

        #endregion
        #region Get/Set
        protected Character Char { get; set; }
        protected WarriorTalents Talents { get; set; }
        protected Stats StatS { get; set; }
        protected CombatFactors CombatFactors { get; set; }
        public Skills.WhiteAttacks WhiteAtks { get; protected set; }
        protected CalculationOptionsDPSWarr CalcOpts { get; set; }
        
        protected float LatentGCD { get { return 1.5f + CalcOpts.Latency; } }
        
        /// <summary>
        /// How many GCDs are in the rotation, based on fight duration and latency
        /// </summary>
        protected float NumGCDs { get { return CalcOpts.AllowFlooring ? (float)Math.Floor(FightDuration / LatentGCD) : FightDuration / LatentGCD; } }
        
        /// <summary>
        /// How many GCDs have been used by the rotation
        /// </summary>
        protected float GCDsUsed;

        /// <summary>
        /// How many GCDs are still available in the rotation
        /// </summary>
        protected float GCDsAvailable { get { return Math.Max(0f, NumGCDs - GCDsUsed); } }
        
        #endregion
        #region Functions
        public virtual void Initialize(CharacterCalculationsDPSWarr calcs) {
            this.calcs = calcs;
            StatS = calcs.AverageStats;

            initAbilities();
            //doIterations();

            // Whites
            calcs.Whites = WhiteAtks;
            // Anti-Debuff
            calcs.HF = HF;
            calcs.EM = EM;
            calcs.CH = CH;
            calcs.IN = IN;
            calcs.IV = IV;
            // Rage Generators
            calcs.SndW = SndW;
            calcs.BZ = BZ;
            calcs.BR = BR;
            // Maintenance
            calcs.BTS = BTS;
            calcs.CS = CS;
            calcs.DS = DS;
            calcs.SN = SN;
            calcs.TH = TH;
            calcs.HMS = HMS;
            calcs.ER = ER;
            // Periodics
            calcs.ST = ST;
            calcs.SW = SW;
            calcs.Death = Death;
            calcs.RK = RK;
            
            // Shared Instants
            calcs.WW = WW;
            calcs.SL = SL;
            // Arms
            
            // Generic
            calcs.CL = CL;
            calcs.DW = DW;
            calcs.HS = HS;
            calcs.EX = EX;
        }
        public virtual void Initialize() { initAbilities(); /*doIterations();*/ }

        public virtual void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations) {
            _needDisplayCalcs = needsDisplayCalculations;
        }

        protected virtual void initAbilities() {
            // Whites
            //WhiteAtks = new Skills.WhiteAtks(Char, StatS, CombatFactors);
            // Anti-Debuff
            HF  = new Skills.HeroicFury(        Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            EM = new Skills.EveryManForHimself(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            CH = new Skills.Charge(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            IN = new Skills.Intercept(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            IV = new Skills.Intervene(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            // Rage Generators
            SndW = new Skills.SecondWind(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            BZ = new Skills.BerserkerRage(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            BR = new Skills.Bloodrage(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            // Maintenance
            BTS = new Skills.BattleShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            CS = new Skills.CommandingShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            DS = new Skills.DemoralizingShout(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            SN = new Skills.SunderArmor(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            TH = new Skills.ThunderClap(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            HMS = new Skills.Hamstring(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            ER = new Skills.EnragedRegeneration(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            // Periodics
            ST = new Skills.ShatteringThrow(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            SW = new Skills.SweepingStrikes(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            Death = new Skills.DeathWish(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            RK = new Skills.Recklessness(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);

            // Slam used by Bloodsurge, WW used by Bladestorm, so they're shared
            SL = new Skills.Slam(               Char, StatS, CombatFactors, WhiteAtks, CalcOpts); // actually arms but BS needs it
            WW = new Skills.WhirlWind(          Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            
            DW = new Skills.DeepWounds(         Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            CL = new Skills.Cleave(             Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            HS = new Skills.HeroicStrike(       Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            EX = new Skills.Execute(            Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
        }
        public virtual void doIterations() { }

        protected virtual void calcDeepWounds() {
            // Main Hand
            float mhActivates =
                /*Yellow  */CriticalYellowsOverDurMH + 
                /*White   */WhiteAtks.MhActivates * WhiteAtks.MHAtkTable.Crit;

            // Off Hand
            float ohActivates = (CombatFactors.useOH ?
                // No OnAttacks for OH
                /*Yellow*/CriticalYellowsOverDurOH +
                /*White */WhiteAtks.OhActivates * WhiteAtks.OHAtkTable.Crit
                : 0f);

            // Push to the Ability
            DW.SetAllAbilityActivates(mhActivates, ohActivates); 
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;
        }

        #region Attacks over Duration
        /// <summary>This is used by Deep Wounds</summary>
        /// <returns>Total Critical attacks over fight duration for Maintenance, Fury and Arms abilities. Does not include Heroic Strikes or Cleaves</returns>
        public float CriticalYellowsOverDur { get { return CriticalYellowsOverDurMH + CriticalYellowsOverDurOH; } }
        protected virtual float CriticalYellowsOverDurMH {
            get {
                bool useOH = CombatFactors.useOH;

                float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Crit * HS.AvgTargets
                               + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Crit * CL.AvgTargets;

                float yellow = 0f
                    // Maintenance
                    + _Thunder_GCDs * TH.MHAtkTable.Crit * TH.AvgTargets
                    + _Ham_GCDs * HMS.MHAtkTable.Crit * HMS.AvgTargets
                    + _Shatt_GCDs * ST.MHAtkTable.Crit * ST.AvgTargets
                    // Fury Abilities
                    + (_WW_GCDs * WW.MHAtkTable.Crit * WW.AvgTargets)
                    + _SL_GCDs * SL.MHAtkTable.Crit * SL.AvgTargets
                    // Generic
                    + _EX_GCDs * EX.MHAtkTable.Crit * EX.AvgTargets
                ;

                return yellow + onAttack;
            }
        }
        protected virtual float CriticalYellowsOverDurOH {
            get {
                if (!CombatFactors.useOH) { return 0; }
                float yellow = 0f
                    // Maintenance
                    // Fury Abilities
                    + (_WW_GCDs * WW.OHAtkTable.Crit * WW.AvgTargets)
                    // Arms Abilities
                ;

                return yellow;
            }
        }
        public float CriticalAtksOverDur { get { return CriticalAtksOverDurMH + CriticalAtksOverDurOH; } }
        protected float CriticalAtksOverDurMH {
            get {
                float yellows = CriticalYellowsOverDurMH;
                float whites = WhiteAtks.CriticalAtksOverDurMH;
                return yellows + whites;
            }
        }
        protected float CriticalAtksOverDurOH {
            get {
                if (!CombatFactors.useOH) { return 0; }
                float yellows = CriticalYellowsOverDurOH;
                float whites = WhiteAtks.CriticalAtksOverDurOH;
                return yellows + whites;
            }
        }
        /// <summary>This is used by Overpower when you have the glyph</summary>
        /// <returns>Total Parried attacks over fight duration for Maintenance, Fury and Arms abilities</returns>
        public virtual float ParriedYellowsOverDur {
            get {
                float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Parry * HS.AvgTargets
                               + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Parry * CL.AvgTargets;

                float yellow = 0f
                    // Maintenance
                    + _Thunder_GCDs * TH.MHAtkTable.Parry * TH.AvgTargets
                    + _Ham_GCDs * HMS.MHAtkTable.Parry * HMS.AvgTargets
                    + _Shatt_GCDs * ST.MHAtkTable.Parry * ST.AvgTargets
                    // Fury Abilities
                    + (CombatFactors.useOH ?
                        (_WW_GCDs * WW.MHAtkTable.Parry * WW.AvgTargets + _WW_GCDs * WW.OHAtkTable.Parry * WW.AvgTargets)
                        : _WW_GCDs * WW.MHAtkTable.Parry * WW.AvgTargets)
                    + _SL_GCDs * SL.MHAtkTable.Parry * SL.AvgTargets
                    // Arms Abilities
                    // Generic
                    + _EX_GCDs * EX.MHAtkTable.Parry * EX.AvgTargets
                ;

                return yellow + onAttack;
            }
        }
        /// <summary>This is used by Overpower</summary>
        /// <returns>Total Dodged attacks over fight duration for Maintenance, Fury and Arms abilities</returns>
        public virtual float DodgedYellowsOverDur {
            get {
                bool useOH = CombatFactors.useOH;

                float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Dodge * HS.AvgTargets
                               + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Dodge * CL.AvgTargets;

                float yellow = 0f
                    // Maintenance
                    + _Thunder_GCDs * TH.MHAtkTable.Dodge * TH.AvgTargets
                    + _Ham_GCDs * HMS.MHAtkTable.Dodge * HMS.AvgTargets
                    + _Shatt_GCDs * ST.MHAtkTable.Dodge * ST.AvgTargets
                    // Fury Abilities
                    + (useOH ? (_WW_GCDs * WW.MHAtkTable.Dodge * WW.AvgTargets + _WW_GCDs * WW.OHAtkTable.Dodge * WW.AvgTargets) / 2f : _WW_GCDs * WW.MHAtkTable.Dodge * WW.AvgTargets)
                    + _SL_GCDs * SL.MHAtkTable.Dodge * SL.AvgTargets
                    // Arms Abilities
                    // Generic
                    + _EX_GCDs * EX.MHAtkTable.Dodge * EX.AvgTargets
                ;

                return yellow + onAttack;
            }
        }
        // Yellow Only Landed Attacks Over Dur
        public float LandedYellowsOverDur { get { return LandedYellowsOverDurMH + LandedYellowsOverDurOH; } }
        protected virtual float AttemptedYellowsOverDurMH {
            get {
                float onAttack = WhiteAtks.HSOverridesOverDur * HS.AvgTargets
                               + WhiteAtks.CLOverridesOverDur * CL.AvgTargets;

                float yellow = 0f
                    // Maintenance
                    + _Thunder_GCDs * TH.AvgTargets
                    + _Ham_GCDs * HMS.AvgTargets
                    + _Shatt_GCDs * ST.AvgTargets
                    // Fury Abilities
                    + (_WW_GCDs * WW.AvgTargets)
                    // Arms Abilities
                    + _SL_GCDs * SL.AvgTargets
                    // Generic
                    + _EX_GCDs * EX.AvgTargets
                ;

                return yellow + onAttack;

            }
        }
        protected virtual float AttemptedYellowsOverDurOH {
            get {
                if (!CombatFactors.useOH) { return 0; }
                float yellow = 0f
                    // Maintenance
                    // Fury Abilities
                    + (_WW_GCDs * WW.AvgTargets)
                    // Arms Abilities

                ;

                return yellow;
            }
        }
        protected virtual float LandedYellowsOverDurMH {
            get {
                return
                    // OnAttack
                      WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.AnyLand * HS.AvgTargets
                    + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.AnyLand * CL.AvgTargets
                    // Maintenance
                    + _Thunder_GCDs * TH.MHAtkTable.AnyLand * TH.AvgTargets
                    + _Ham_GCDs * HMS.MHAtkTable.AnyLand * HMS.AvgTargets
                    + _Shatt_GCDs * ST.MHAtkTable.AnyLand * ST.AvgTargets
                    // Fury Abilities
                    + _WW_GCDs * WW.MHAtkTable.AnyLand * WW.AvgTargets
                    // Arms Abilities
                    + _SL_GCDs * SL.MHAtkTable.AnyLand * SL.AvgTargets
                    // Generic
                    + _EX_GCDs * EX.MHAtkTable.AnyLand * EX.AvgTargets;
            }
        }
        protected virtual float LandedYellowsOverDurOH {
            get {
                if (!CombatFactors.useOH) { return 0; }
                float yellow = 0f
                    // Maintenance
                    // Fury Abilities
                    + (_WW_GCDs * WW.OHAtkTable.AnyLand * WW.AvgTargets)
                    // Arms Abilities

                ;

                return yellow;
            }
        }
        // All Landed Attacks Over Dur, Yellow and White
        public float AttemptedAtksOverDur { get { return AttemptedAtksOverDurMH + AttemptedAtksOverDurOH; } }
        public float AttemptedAtksOverDurMH {
            get {
                float white = WhiteAtks.MhActivates;
                float yellow = AttemptedYellowsOverDurMH;
                return white + yellow;
            }
        }
        public float AttemptedAtksOverDurOH {
            get {
                if (!CombatFactors.useOH) return 0f;
                float white = WhiteAtks.OhActivates;
                float yellow = AttemptedYellowsOverDurOH;
                return white + yellow;
            }
        }
        public float LandedAtksOverDur { get { return LandedAtksOverDurMH + LandedAtksOverDurOH; } }
        public virtual float LandedAtksOverDurMH {
            get {
                float white = WhiteAtks.LandedAtksOverDurMH;
                float yellow = LandedYellowsOverDurMH;

                float result = white + yellow;

                return result;
            }
        }
        public virtual float LandedAtksOverDurOH {
            get {
                if (!CombatFactors.useOH) { return 0; }
                float white = WhiteAtks.LandedAtksOverDurOH;
                float yellow = LandedYellowsOverDurOH;

                float result = white + yellow;

                return result;
            }
        }
        public virtual float CritHsSlamOverDur {
            get {
                float critsPerRot = 0;
                if (CalcOpts.FuryStance) {
                    critsPerRot = HS.Activates * HS.MHAtkTable.Crit +
                                  SL.Activates * SL.MHAtkTable.Crit;
                } else {
                    critsPerRot = _HS_Acts * HS.MHAtkTable.Crit +
                                  _SL_GCDs * SL.MHAtkTable.Crit;
                }
                return critsPerRot;
            }
        }

        #endregion

        public float MaintainCDs { get { return _Thunder_GCDs + _Sunder_GCDs + _Demo_GCDs + _Ham_GCDs + _Battle_GCDs + _Comm_GCDs + _ER_GCDs + _Death_GCDs + _Reck_GCDs; } }
        #endregion
        #region Rage Calcs
        protected virtual float RageGenOverDur_IncDmg {
            get {
                // Invalidate bad things
                if (!CalcOpts.AoETargets || CalcOpts.AoETargetsFreq < 1 || CalcOpts.AoETargetsDMG < 1) { return 0f; }
                float RageMod = 2.5f / 453.3f;
                float damagePerSec = 0f;
                float freq = CalcOpts.AoETargetsFreq;
                float dmg = CalcOpts.AoETargetsDMG * (1f + StatS.DamageTakenMultiplier) + StatS.BossAttackPower / 14f;
                float acts = FightDuration / freq;
                // Add Berserker Rage's
                float zerkerMOD = 1f;
                if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BerserkerRage_]) {
                    SpecialEffect effect = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusAgilityMultiplier = 1f }, // this is just so we can use a Perc Mod without having to make a new stat
                        10f, 30f);
                    float upTime = effect.GetAverageUptime(0, 1f, CombatFactors._c_mhItemSpeed, (CalcOpts.SE_UseDur ? FightDuration : 0f));
                    zerkerMOD *= (1f + upTime);
                }
                float dmgCap = 100f / (RageMod * zerkerMOD); // Can't get any more rage than 100 at any given time
                damagePerSec = (acts * Math.Min(dmgCap, dmg)) / FightDuration;
                
                return (damagePerSec * RageMod * zerkerMOD) * FightDuration;
            }
        }
        protected virtual float RageGenOverDur_Anger { get { return (Talents.AngerManagement / 3.0f) * CalcOpts.Duration; } }
        protected virtual float RageGenOverDur_Blood {
            get {
                if (!BR.Validated) { return 0f; }
                float rage = BR.RageCost;
                float acts = BR.Activates;
                return rage * acts;
            }
        }
        
        private float _RageGenOverDur_Other = -1f;
        protected virtual float RageGenOverDur_Other {
            get {
                if (_RageGenOverDur_Other == -1f) {
                    float rage = RageGenOverDur_Anger               // Anger Management Talent
                               + RageGenOverDur_Blood               // Bloodrage
                               + RageGenOverDur_IncDmg              // Damage from the bosses
                               + (100f * StatS.ManaorEquivRestore)  // 0.02f becomes 2f
                               + StatS.BonusRageGen;                // Bloodrage, Berserker Rage, Might Rage Pot

                    // 4pcT7
                    if (StatS.BonusWarrior_T7_4P_RageProc != 0f) {
                        rage += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * (Talents.DeepWounds > 0f ? 1f : 0f) * CalcOpts.Duration;
                        rage += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * (!CalcOpts.FuryStance && CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_] ? 1f / 3f : 0f) * CalcOpts.Duration;
                    }
                    _RageGenOverDur_Other = rage;
                }

                return _RageGenOverDur_Other;
            }
        }
        protected virtual float RageNeededOverDur {
            get {
                if (Char.MainHand == null) { return 0f; }
                // Fury
                float SlamRage = SL.GetRageUseOverDur(_SL_GCDs);
                float WWRage = WW.GetRageUseOverDur(_WW_GCDs);
                
                // Arms
                
                // Maintenance
                float ThunderRage    = TH.GetRageUseOverDur( _TH_DPS     );
                float SunderRage     = SN.GetRageUseOverDur( _Sunder_GCDs);
                float DemoRage       = DS.GetRageUseOverDur( _Demo_GCDs  );
                float HamstringRage  = HMS.GetRageUseOverDur(_Ham_GCDs   );
                float BattleRage     = BTS.GetRageUseOverDur(_Battle_GCDs);
                float ExecSpamRage   = EX.GetRageUseOverDur( _EX_GCDs    );
                // Total
                float rage = WWRage + SlamRage +
                    ThunderRage + SunderRage + DemoRage + HamstringRage + BattleRage + ExecSpamRage;
                return rage;
            }
        }
        #endregion

        #region AddAnItem(s)
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, Add DPS, Pull Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float availGCDs, ref float GCDsused, ref float availRage, float TotalPercTimeLost, ref float _Abil_GCDs, ref float DPS_TTL, ref float HPS_TTL, ref float _Abil_DPS, ref float _Abil_HPS, Skills.Ability abil) {
            if (!abil.Validated) { return; }
            float acts = Math.Min(availGCDs, abil.Activates * (1f - TotalPercTimeLost));
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += Math.Min(NumGCDs, Abil_GCDs);
            if (_needDisplayCalcs) GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = Math.Max(0f, NumGCDs - GCDsused);
            _Abil_DPS = abil.GetDPS(Abil_GCDs);
            _Abil_HPS = abil.GetHPS(Abil_GCDs);
            DPS_TTL += _Abil_DPS;
            HPS_TTL += _Abil_HPS;
            float rageadd = abil.GetRageUseOverDur(Abil_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, No DPS, Pull Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float availGCDs, ref float GCDsused, ref float availRage, float TotalPercTimeLost, ref float _Abil_GCDs, ref float HPS_TTL, ref float _Abil_HPS, Skills.Ability abil) {
            if (!abil.Validated) { return; }
            float acts = Math.Min(availGCDs, abil.Activates * (1f - TotalPercTimeLost));
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += Math.Min(NumGCDs, Abil_GCDs);
            if (_needDisplayCalcs) GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = Math.Max(0f, NumGCDs - GCDsused);
            _Abil_HPS = abil.GetHPS(Abil_GCDs);
            HPS_TTL += _Abil_HPS;
            float rageadd = abil.GetRageUseOverDur(Abil_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, No DPS, Add Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float availGCDs, ref float GCDsused, ref float availRage, float TotalPercTimeLost, ref float _Abil_GCDs, ref float HPS_TTL, ref float _Abil_HPS, Skills.Ability abil, bool flag) {
            if (!abil.Validated) { return; }
            float acts = Math.Min(availGCDs, abil.Activates * (1f - TotalPercTimeLost));
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += Math.Min(NumGCDs, Abil_GCDs);
            if (_needDisplayCalcs) GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = Math.Max(0f, NumGCDs - GCDsused);
            _Abil_HPS = abil.GetHPS(Abil_GCDs);
            HPS_TTL += _Abil_HPS;
            float rageadd = abil.GetRageUseOverDur(Abil_GCDs);
            RageGenOther += rageadd;
            availRage += rageadd;
        }
        #endregion

        #region Lost Time due to Combat Factors
        private float _emActs, _emRecovery, _emRecoveryTotal;

        /// <summary>
        /// Calculates percentage of time lost due to moving, being rooted, etc
        /// </summary>
        /// <param name="MS">Placeholder right now for juggernaut handling.  Fury should pass null</param>
        /// <returns>Percentage of time lost as a float</returns>
        protected float CalculateTimeLost(Skills.MortalStrike MS)
        {
            _emActs = 0f; _emRecovery = 0f; _emRecoveryTotal = 0f;
            TimeLostGDCs = 0;
            RageGainedWhileMoving = 0; 

            float percTimeInMovement = CalculateMovement(MS);
            float percTimeInFear = CalculateFear();
            float percTimeInStun = CalculateStun();
            float percTimeInRoot = CalculateRoot();
            return Math.Min(1f, percTimeInStun + percTimeInMovement + percTimeInFear + percTimeInRoot);
        }

        private float CalculateRoot()
        {
            float percTimeInRoot = 0f;

            if (CalcOpts.RootingTargets && CalcOpts.RootingTargetsFreq > 0)
            {
                float timelostwhilerooted = 0f;
                float BaseRootDur = Math.Max(0f, (CalcOpts.RootingTargetsDur / 1000f * (1f - StatS.SnareRootDurReduc)));
                //float rootedActs = Math.Max(0f, FightDuration / CalcOpts.RootingTargetsFreq);
                float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Ceiling(FightDuration / CalcOpts.RootingTargetsFreq) : FightDuration / CalcOpts.RootingTargetsFreq;
                float rootedActs = Abil_Acts;
                float rootedPer = Math.Max(0f, BaseRootDur);
                float rootedEaten = Math.Min(NumGCDs, (rootedPer * rootedActs) / LatentGCD);

                #region Recovery Efforts
                /*if (_Rooted_Acts > 0f) {
                    float bzacts = BZ.Activates;
                    _ZRage_GCDs = Math.Min(_Rooted_Acts, bzacts);
                    _BZ_RecovPer = Math.Max(0f, (BaseRootDur - Math.Max(0f, CalcOpts.React / 1000f)));
                    _BZ_RecovTTL = Math.Min(_Rooted_Eaten, (_BZ_RecovPer * _ZRage_GCDs) / LatentGCD);
                }*/
                if (Char.Race == CharacterRace.Human && rootedActs > 0)
                {
                    // Every Man for Himself can break it
                    float emacts = CalcOpts.AllowFlooring ? (float)Math.Floor(EM.Activates) : EM.Activates - _emActs;
                    _emActs = Math.Min(rootedActs, emacts);
                    _emRecovery = Math.Max(0f, (rootedPer - Math.Max(0f, CalcOpts.React / 1000f)));
                    _emRecoveryTotal = Math.Min(rootedEaten, (_emRecovery * _emActs) / LatentGCD);
                }
                #endregion

                // We'll use % of time lost to stuns to affect each ability equally
                // othwerwise we are only seriously affecting things at
                // the bottom of priorities, which isn't fair (poor Slam)
                timelostwhilerooted = rootedActs * BaseRootDur;
                //- _ZRage_GCDs * _BZ_RecovPer;
                timelostwhilerooted = CalcOpts.AllowFlooring ? (float)Math.Ceiling(timelostwhilerooted) : timelostwhilerooted;
                percTimeInRoot = timelostwhilerooted / FightDuration;
                //
                if (_needDisplayCalcs)
                {
                    GCDUsage += (rootedActs > 0 ? rootedActs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + rootedPer.ToString("0.00") + "secs : Lost to Roots\n" : "");
                    GCDUsage += (_emActs > 0 ? _emActs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + _emRecovery.ToString("0.00") + "secs : - " + EM.Name + "\n" : "");
                }
                SndW.NumStunsOverDur += rootedActs;
            }
            return percTimeInRoot;
        }

        private float CalculateStun()
        {
            float percTimeInStun = 0f;
            #region Being Stunned
            if (CalcOpts.StunningTargets && CalcOpts.StunningTargetsFreq > 0)
            {
                float BaseStunDur = Math.Max(0f, (CalcOpts.StunningTargetsDur / 1000f * (1f - StatS.StunDurReduc)));
                float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Ceiling(FightDuration / CalcOpts.StunningTargetsFreq) : FightDuration / CalcOpts.StunningTargetsFreq;
                float hfActs = 0, hfRecovPer = 0, hfRecovTotal = 0;
                float stunnedActs = Abil_Acts;
                float stunnedPer = Math.Max(0f, BaseStunDur);
                float stunnedEaten = Math.Min(NumGCDs, (stunnedPer * stunnedActs) / LatentGCD);

                float timelostwhilestunned = stunnedActs * stunnedPer;
                #region Recovery efforts
                if (Talents.HeroicFury > 0 && stunnedActs > 0f)
                {
                    float hfacts = CalcOpts.AllowFlooring ? (float)Math.Floor(HF.Activates) : HF.Activates;
                    hfActs = Math.Min(stunnedActs, hfacts);
                    hfRecovPer = Math.Max(0f, (stunnedPer - Math.Max(0f, CalcOpts.React / 1000f)));
                    hfRecovTotal = Math.Min(stunnedEaten, (hfRecovPer * hfActs) / LatentGCD);
                    timelostwhilestunned -= hfActs * hfRecovPer;
                }
                if (Char.Race == CharacterRace.Human && (stunnedActs - hfActs > 0))
                {
                    float emacts = CalcOpts.AllowFlooring ? (float)Math.Floor(EM.Activates) : EM.Activates - _emActs;
                    _emActs = Math.Min(stunnedActs - hfActs, emacts);
                    _emRecovery = Math.Max(0f, (stunnedPer - Math.Max(0f, CalcOpts.React / 1000f)));
                    _emRecoveryTotal = Math.Min(stunnedEaten, (_emRecovery * _emActs) / LatentGCD);
                    timelostwhilestunned -= _emActs * _emRecovery;
                }
                #endregion

                // We'll use % of time lost to stuns to affect each ability equally
                // othwerwise we are only seriously affecting things at
                // the bottom of priorities, which isn't fair (poor Slam)

                timelostwhilestunned = CalcOpts.AllowFlooring ? (float)Math.Ceiling(timelostwhilestunned) : timelostwhilestunned;
                percTimeInStun = timelostwhilestunned / FightDuration;
                //
                if (_needDisplayCalcs)
                {
                    GCDUsage += (stunnedActs > 0 ? stunnedActs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + stunnedPer.ToString("0.00") + "secs : Lost to Stuns\n" : "");
                    GCDUsage += (hfActs > 0 ? hfActs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + hfRecovPer.ToString("0.00") + "secs : - " + HF.Name + "\n" : "");
                    GCDUsage += (_emActs > 0 ? _emActs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + _emRecovery.ToString("0.00") + "secs : - " + EM.Name + "\n" : "");
                }
                SndW.NumStunsOverDur = stunnedActs;
            }
            #endregion
            return percTimeInStun;
        }

        private float CalculateFear()
        {
            float percTimeInFear = 0f;
            if (CalcOpts.FearingTargets && CalcOpts.Fears.Count > 0)
            {
                float timelostwhilefeared = 0f;
                float BaseFearDur = 0f, fearActs = 0f, reducedDur = 0f,
                      MaxTimeRegain = 0f,
                      ChanceYouAreFeared = 1f;
                float BZMaxActs = CalcOpts.AllowFlooring ? (float)Math.Floor(BZ.ActivatesOverride) : BZ.ActivatesOverride;
                float BZActualActs = 0f;
                float EMMaxActs = CalcOpts.AllowFlooring ? (float)Math.Floor(EM.Activates) : EM.Activates;
                float EMActualActs = 0f;
                TimesFeared = 0f;
                foreach (Fear f in CalcOpts.Fears)
                {
                    BaseFearDur = Math.Max(0f, (f.Duration / 1000f * (1f - StatS.FearDurReduc)));
                    TimesFeared += fearActs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(FightDuration / f.Frequency) : FightDuration / f.Frequency;

                    if ((BZMaxActs - BZActualActs > 0f) && (fearActs > 0f))
                    {
                        MaxTimeRegain = Math.Max(0f, (BaseFearDur - LatentGCD - CalcOpts.React / 1000f));
                        float BZNewActs = Math.Min(BZMaxActs - BZActualActs, fearActs);
                        BZActualActs += BZNewActs;
                        // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                        reducedDur = Math.Max(0f, BaseFearDur - MaxTimeRegain);
                        float percBZdVsUnBZd = BZNewActs / fearActs;
                        timelostwhilefeared += (reducedDur * fearActs * percBZdVsUnBZd * ChanceYouAreFeared)
                                             + (BaseFearDur * fearActs * (1f - percBZdVsUnBZd) * ChanceYouAreFeared);
                    }
                    else if (Char.Race == CharacterRace.Human && (EMMaxActs - EMActualActs > 0f) && (fearActs > 0f))
                    {
                        MaxTimeRegain = Math.Max(0f, (BaseFearDur - LatentGCD - CalcOpts.React / 1000f));
                        float EMNewActs = Math.Min(EMMaxActs - EMActualActs, fearActs);
                        EMActualActs += EMNewActs;
                        // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                        reducedDur = Math.Max(0f, BaseFearDur - MaxTimeRegain);
                        float percEMdVsUnEMd = EMNewActs / fearActs;
                        timelostwhilefeared += (reducedDur * fearActs * percEMdVsUnEMd * ChanceYouAreFeared)
                                             + (BaseFearDur * fearActs * (1f - percEMdVsUnEMd) * ChanceYouAreFeared);
                    }
                    else if (fearActs > 0f)
                    {
                        timelostwhilefeared += BaseFearDur * fearActs * ChanceYouAreFeared;
                    }
                }
                _ZRage_GCDs = BZActualActs;
                _emActs = EMActualActs;
                if (_needDisplayCalcs)
                {
                    GCDUsage += (TimesFeared > 0 ? TimesFeared.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + BaseFearDur.ToString("0.00") + "secs : Lost to Fears\n" : "");
                    GCDUsage += (_ZRage_GCDs > 0 ? _ZRage_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + (BaseFearDur - reducedDur).ToString("0.00") + "secs : - " + BZ.Name + "\n" : "");
                    GCDUsage += (_emActs > 0 ? _emActs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + (BaseFearDur - reducedDur).ToString("0.00") + "secs : - " + EM.Name + "\n" : "");
                }
                TimeLostGDCs += Math.Min(NumGCDs, (BaseFearDur * TimesFeared) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * _ZRage_GCDs) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * _emActs) / LatentGCD);
                TimeLostGDCs = Math.Max(0f, NumGCDs - TimeLostGDCs);

                timelostwhilefeared = CalcOpts.AllowFlooring ? (float)Math.Ceiling(timelostwhilefeared) : timelostwhilefeared;
                percTimeInFear = timelostwhilefeared / FightDuration;
            }
            
            return percTimeInFear;
        }

        private float CalculateMovement(Skills.MortalStrike MS)
        {
            float percTimeInMovement = 0f;
            if (CalcOpts.MovingTargets && CalcOpts.Moves.Count > 0)
            {
                /* = Movement Speed =
                 * According to a post I found on WoWWiki, Standard (Run) Movement
                 * Speed is 7 yards per 1 sec.
                 * Cat's Swiftness (and similar) bring this to 7.56 (7x1.08)
                 * If you are moving for 5 seconds, this is 35 yards (37.8 w/ bonus)
                 * All the movement effects have a min 8 yards, so you have to be
                 * moving for 1.142857 seconds (1.08 seconds w/ bonus) before Charge
                 * would be viable. If you had to be moving more than Charge's Max
                 * Range (25 yards, editable by certain bonuses) then we'd benefit
                 * again from move speed bonuses, etc.
                 * 
                 * Charge Max = 25
                 * that's 25/7.00 = 3.571428571428571 seconds at 7.00 yards per sec
                 * that's 25/7.56 = 3.306878306873070 seconds at 7.56 yards per sec
                 * Charge (Glyph of Charge) Max = 25+5=30
                 * that's 30/7.00 = 4.285714285714286 seconds at 7.00 yards per sec
                 * that's 30/7.56 = 3.968253968253968 seconds at 7.56 yards per sec
                 * 
                 * = Now let's try and get some of those GCDs back =
                 * Let's assume that if the movement duration is longer
                 * than the before mentioned (1.142857|1.08) seconds,
                 * you are far enough away that you can use a Movement
                 * Ability (Charge, Intercept or  Intervene)
                 * Since some of these abilities are usable in combat
                 * only by talents, we have to make those checks first
                 * Since some stuff is kind of weird, we're going to
                 * enforce an ~3 sec est minimum move time before activating
                 * Charge
                 */
                float MovementSpeed = 7f * (1f + StatS.MovementSpeed); // 7 yards per sec * 1.08 (if have bonus) = 7.56
                float BaseMoveDur = 0f, movedActs = 0f, reducedDur = 0f,
                      MinMovementTimeRegain = 0f, MaxMovementTimeRegain = 0f,
                      ChanceYouHaveToMove = 1f;
                float ChargeMaxActs = CalcOpts.AllowFlooring ? (float)Math.Floor(CH.Activates) : CH.Activates;
                float ChargeActualActs = 0f;
                float timelostwhilemoving = 0f;
                float moveGCDs = 0f;
                foreach (Move m in CalcOpts.Moves)
                {
                    BaseMoveDur = (m.Duration / 1000f * (1f - StatS.MovementSpeed));
                    moveGCDs += movedActs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(FightDuration / m.Frequency) : FightDuration / m.Frequency;

                    if ((ChargeMaxActs - ChargeActualActs > 0f) && (movedActs > 0f))
                    {
                        MaxMovementTimeRegain = Math.Max(0f,
                            Math.Min((BaseMoveDur - CalcOpts.React / 1000f),
                                     (CH.MaxRange / MovementSpeed - CalcOpts.React / 1000f)));
                        MinMovementTimeRegain = Math.Max(0f,
                            Math.Min((BaseMoveDur - CalcOpts.React / 1000f),
                                     (CH.MinRange / MovementSpeed - CalcOpts.React / 1000f)));
                        if (BaseMoveDur >= MinMovementTimeRegain)
                        {
                            float ChargeNewActs = Math.Min(ChargeMaxActs - ChargeActualActs, movedActs);
                            ChargeActualActs += ChargeNewActs;
                            // Use up to the maximum, leaving a 0 boundary so we don't mess up later numbers
                            reducedDur = Math.Max(0f, BaseMoveDur - MaxMovementTimeRegain);
                            float percChargedVsUncharged = ChargeNewActs / movedActs;
                            timelostwhilemoving += (reducedDur * movedActs * percChargedVsUncharged * ChanceYouHaveToMove)
                                                 + (BaseMoveDur * movedActs * (1f - percChargedVsUncharged) * ChanceYouHaveToMove);
                        }
                    }
                    else if (movedActs > 0f)
                    {
                        timelostwhilemoving += BaseMoveDur * movedActs * ChanceYouHaveToMove;
                    }
                }
                float actsCharge = ChargeActualActs;
                if (_needDisplayCalcs)
                {
                    GCDUsage += (moveGCDs > 0 ? moveGCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + BaseMoveDur.ToString("0.00") + "secs : Lost to Movement\n" : "");
                    GCDUsage += (actsCharge > 0 ? actsCharge.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " x " + (BaseMoveDur - reducedDur).ToString("0.00") + "secs : - " + CH.Name + "\n" : "");
                }
                TimeLostGDCs += Math.Min(NumGCDs, (BaseMoveDur * moveGCDs) / LatentGCD);
                TimeLostGDCs -= Math.Min(TimeLostGDCs, (reducedDur * actsCharge) / LatentGCD);
                RageGainedWhileMoving += CH.GetRageUseOverDur(actsCharge);
                // Need to add the special effect from Juggernaut to Mortal Strike, not caring about Slam right now
                if (Talents.Juggernaut > 0 && MS != null)
                {
                    Stats stats = new Stats
                    {
                        BonusWarrior_T8_4P_MSBTCritIncrease = 0.25f *
                            (new SpecialEffect(Trigger.Use, null, 10, CH.Cd)
                             ).GetAverageUptime(FightDuration / actsCharge, 1f, CombatFactors._c_mhItemSpeed, FightDuration)
                    };
                    stats.Accumulate(StatS);
                    // I'm not sure if this is gonna work, but hell, who knows
                    MS.BonusCritChance = stats.BonusWarrior_T8_4P_MSBTCritIncrease;
                    //MS = new Skills.MortalStrike(Char, stats, CombatFactors, WhiteAtks, CalcOpts);
                }
                timelostwhilemoving = (CalcOpts.AllowFlooring ? (float)Math.Ceiling(timelostwhilemoving) : timelostwhilemoving);
                percTimeInMovement = timelostwhilemoving / FightDuration;
            }
            return percTimeInMovement;
        }
        #endregion

        public void AddValidatedSpecialEffects(Stats statsTotal, WarriorTalents talents)
        {
            if (ST.Validated)
            {
                SpecialEffect shatt = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.20f, },
                    ST.Duration, ST.Cd,
                    ST.MHAtkTable.AnyLand);
                statsTotal.AddSpecialEffect(shatt);
            }
            if (BTS.Validated)
            {
                SpecialEffect bs = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = (548f * (1f + talents.CommandingPresence * 0.05f)), },
                    BTS.Duration, BTS.Cd);
                statsTotal.AddSpecialEffect(bs);
            }
            if (CS.Validated)
            {
                //float value = (2255f * (1f + talents.CommandingPresence * 0.05f));
                SpecialEffect cs = new SpecialEffect(Trigger.Use,
                    new Stats() { Health = 2255f * (1f + talents.CommandingPresence * 0.05f), },
                    CS.Duration, CS.Cd);
                statsTotal.AddSpecialEffect(cs);
            }
            if (DS.Validated)
            {
                //float value = (410f * (1f + talents.ImprovedDemoralizingShout * 0.08f));
                SpecialEffect ds = new SpecialEffect(Trigger.Use,
                    new Stats() { BossAttackPower = 410f * (1f + talents.ImprovedDemoralizingShout * 0.08f) * -1f, },
                    DS.Duration, DS.Cd);
                statsTotal.AddSpecialEffect(ds);
            }
            if (TH.Validated)
            {
                //float value = (0.10f * (1f + (float)Math.Ceiling(talents.ImprovedThunderClap * 10f / 3f) / 100f));
                SpecialEffect tc = new SpecialEffect(Trigger.Use,
                    new Stats() { BossAttackSpeedMultiplier = (-0.10f * (1f + talents.ImprovedThunderClap / 30f)), },
                    TH.Duration, TH.Cd, TH.MHAtkTable.AnyLand);
                statsTotal.AddSpecialEffect(tc);
            }
            if (SN.Validated)
            {
                //float value = 0.04f;
                SpecialEffect sn = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.04f, },
                    SN.Duration, SN.Cd, SN.MHAtkTable.AnyLand, 5);
                statsTotal.AddSpecialEffect(sn);
            }
            float landedAtksInterval = LandedAtksOverDur / CalcOpts.Duration;
            float critRate = CriticalAtksOverDur / AttemptedAtksOverDur;
            if (SW.Validated)
            {
                SpecialEffect sweep = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusTargets = 1f, },
                    landedAtksInterval * 5f, SW.Cd);
                statsTotal.AddSpecialEffect(sweep);
            }
            if (RK.Validated && CalcOpts.FuryStance)
            {
                SpecialEffect reck = new SpecialEffect(Trigger.Use,
                    new Stats() { PhysicalCrit = 1f - critRate, },
                    landedAtksInterval * 3f, RK.Cd);
                statsTotal.AddSpecialEffect(reck);
            }
            if (talents.Flurry > 0 && CalcOpts.FuryStance)
            {
                //float value = talents.Flurry * 0.05f;
                SpecialEffect flurry = new SpecialEffect(Trigger.MeleeCrit,
                    new Stats() { PhysicalHaste = talents.Flurry * 0.05f, }, landedAtksInterval * 3f, 0f);
                statsTotal.AddSpecialEffect(flurry);
            }
        }
    }
}
