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

        /*public Rotation(Character character, Stats stats) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CombatFactors = new CombatFactors(Char, StatS);
            CalcOpts = Char == null || Char.CalculationOptions == null ? new CalculationOptionsDPSWarr() : Char.CalculationOptions as CalculationOptionsDPSWarr;
            //WhiteAtks = new Skills.WhiteAttacks(Char, StatS);
            // Initialize();
        }*/
        #region Variables
        protected Character CHARACTER;
        protected WarriorTalents TALENTS;
        protected Stats STATS;
        protected CombatFactors COMBATFACTORS;
        protected Skills.WhiteAttacks WHITEATTACKS;
        protected CalculationOptionsDPSWarr CALCOPTS;

        public float _HPS_TTL;
        public string GCDUsage = "";

        public float _Thunder_GCDs = 0f, _TH_DPS = 0f, _TH_HPS = 0f;
        public float _Blood_GCDs = 0f, _Blood_HPS = 0f;
        public float _ZRage_GCDs = 0f, _ZRage_HPS = 0f;
        public float _Second_Acts = 0f, _Second_HPS = 0f;
        public float _Battle_GCDs = 0f, _Battle_HPS = 0f;
        public float _Comm_GCDs = 0f, _Comm_HPS = 0f;
        public float _Demo_GCDs = 0f, _Demo_HPS = 0f;
        public float _Sunder_GCDs = 0f, _Sunder_HPS = 0f;
        public float _Ham_GCDs = 0f, _Ham_DPS = 0f, _Ham_HPS = 0f;
        public float _ER_GCDs = 0f, _ER_HPS = 0f;
        public float _Shatt_GCDs = 0f, _Shatt_DPS = 0f, _Shatt_HPS = 0f;
        public float _Death_GCDs = 0f, _Death_HPS = 0f;
        public float _Reck_GCDs = 0f, _Reck_HPS = 0f;
        public float _SW_DPS = 0f, _SW_HPS = 0f, _SW_GCDs = 0f;
        public float RageGenWhite = 0f; public float RageGenOther = 0f; public float RageNeeded = 0f;
        public float _HS_PerHit = 0f, _HS_DPS = 0f, _HS_Acts = 0f;
        public float _CL_PerHit = 0f, _CL_DPS = 0f, _CL_Acts = 0f;
        public float _WhiteDPSMH = 0f; public float _WhiteDPSOH = 0f;
        public float _WhiteDPS = 0f; public float _WhitePerHit = 0f;
        public float _DW_PerHit = 0f, _DW_DPS = 0f;
        #region Abilities
        // Anti-DeBuff
        public Skills.HeroicFury HF;
        public Skills.EveryManForHimself EM;
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
        #endregion

        #endregion
        #region Get/Set
        public Character Char { get { return CHARACTER; } set { CHARACTER = value; } }
        public WarriorTalents Talents { get { return TALENTS; } set { TALENTS = value; } }
        public Stats StatS { get { return STATS; } set { STATS = value; } }
        public CombatFactors CombatFactors { get { return COMBATFACTORS; } set { COMBATFACTORS = value; } }
        public Skills.WhiteAttacks WhiteAtks { get { return WHITEATTACKS; } set { WHITEATTACKS = value; } }
        public CalculationOptionsDPSWarr CalcOpts { get { return CALCOPTS; } set { CALCOPTS = value; } }
        #endregion
        #region Functions
        public virtual void Initialize(CharacterCalculationsDPSWarr calcs) {
            initAbilities();
            doIterations();

            /*WHITEATTACKS.OvdFreqHS = HS.Activates * WHITEATTACKS.MhEffectiveSpeed / CalcOpts.Duration;
            WHITEATTACKS.OvdFreqCL = CL.Activates * WHITEATTACKS.MhEffectiveSpeed / CalcOpts.Duration;

            calcs.WhiteDPSMH = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.MhDPS); // MhWhiteDPS
            calcs.WhiteDPSOH = (CHARACTER.OffHand  == null ? 0f : WHITEATTACKS.OhDPS);
            calcs.WhiteDPS = calcs.WhiteDPSMH + calcs.WhiteDPSOH;
            calcs.WhiteDmg = (CHARACTER.MainHand == null ? 0f : WHITEATTACKS.MhDamageOnUse); //MhAvgSwingDmg

            WHITEATTACKS.OvdFreqHS = 0;
            WHITEATTACKS.OvdFreqCL = 0;*/

            // Whites
            calcs.Whites = WhiteAtks;
            // Anti-Debuff
            calcs.HF = HF;
            calcs.EM = EM;
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
        }
        public void Initialize() { initAbilities(); doIterations(); }

        public virtual float MakeRotationandDoDPS() { return -1.0f; }

        protected virtual void initAbilities() {
            // Whites
            WhiteAtks = new Skills.WhiteAttacks(CHARACTER, STATS, COMBATFACTORS);
            // Anti-Debuff
            HF  = new Skills.HeroicFury(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            EM  = new Skills.EveryManForHimself(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Rage Generators
            SndW= new Skills.SecondWind(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BZ  = new Skills.BerserkerRage(     CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BR  = new Skills.Bloodrage(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Maintenance
            BTS = new Skills.BattleShout(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            CS  = new Skills.CommandingShout(   CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            DS  = new Skills.DemoralizingShout( CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SN  = new Skills.SunderArmor(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            TH  = new Skills.ThunderClap(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HMS = new Skills.Hamstring(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            ER  = new Skills.EnragedRegeneration(CHARACTER,STATS, COMBATFACTORS, WHITEATTACKS);
            // Periodics
            ST = new Skills.ShatteringThrow(    CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SW = new Skills.SweepingStrikes(    CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            Death = new Skills.DeathWish(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            RK = new Skills.Recklessness(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);

            // Slam used by Bloodsurge, WW used by Bladestorm, so they're shared
            SL = new Skills.Slam(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS); // actually arms but BS needs it
            WW = new Skills.WhirlWind(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            
            DW = new Skills.DeepWounds(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            CL = new Skills.Cleave(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HS = new Skills.HeroicStrike(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
        }
        protected virtual void doIterations() { }

        protected void calcDeepWounds() {
            // Main Hand
            float mhActivates =
                /*OnAttack*/_HS_Acts * HS.MHAtkTable.Crit +
                /*OnAttack*/_CL_Acts * CL.MHAtkTable.Crit +
                /*Yellow  */GetCriticalYellowsOverDurMH() + 
                /*White   */WhiteAtks.MhActivates * WhiteAtks.MHAtkTable.Crit;

            // Off Hand
            float ohActivates = (CHARACTER.OffHand != null && Char.OffHand.Speed == 0f) ?
                // No OnAttacks for OH
                /*Yellow*/GetCriticalYellowsOverDurOH() +
                /*White */WhiteAtks.OhActivates * WhiteAtks.OHAtkTable.Crit
                : 0f;

            // Push to the Ability
            DW.SetAllAbilityActivates(mhActivates, ohActivates);
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;
        }

        #region Attacks over Duration
        /// <summary>This is used by Deep Wounds</summary>
        /// <returns>Total Critical attacks over fight duration for Maintenance, Fury and Arms abilities. Does not include Heroic Strikes or Cleaves</returns>
        public float GetCriticalYellowsOverDur() { return GetCriticalYellowsOverDurMH() + GetCriticalYellowsOverDurOH(); }
        public virtual float GetCriticalYellowsOverDurMH() {
            bool useOH = CombatFactors.OH != null && CombatFactors.OHSpeed > 0;

            float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Crit * HS.AvgTargets
                           + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Crit * CL.AvgTargets;

            float yellow = 0f
                // Maintenance
                + _Thunder_GCDs * TH.MHAtkTable.Crit * TH.AvgTargets
                + _Ham_GCDs * HMS.MHAtkTable.Crit * HMS.AvgTargets
                + _Shatt_GCDs * ST.MHAtkTable.Crit * ST.AvgTargets
                // Fury Abilities
                + (_WW_GCDs * WW.MHAtkTable.Crit * WW.AvgTargets * 6) / (useOH ? 2 : 1)
                + _SL_GCDs * SL.MHAtkTable.Crit * SL.AvgTargets
            ;

            return yellow + onAttack;
        }
        public virtual float GetCriticalYellowsOverDurOH() {
            if (!CombatFactors.useOH) { return 0; }
            float yellow = 0f
                // Maintenance
                // Fury Abilities
                + (_WW_GCDs * WW.OHAtkTable.Crit * WW.AvgTargets) / 2
                // Arms Abilities
            ;

            return yellow;
        }
        public float GetCriticalAtksOverDur() { return GetCriticalAtksOverDurMH() + GetCriticalAtksOverDurOH(); }
        public float GetCriticalAtksOverDurMH() {
            float yellows = GetCriticalYellowsOverDurMH();
            float whites = WhiteAtks.CriticalAtksOverDurMH;
            return yellows + whites;
        }
        public float GetCriticalAtksOverDurOH() {
            if (!CombatFactors.useOH) { return 0; }
            float yellows = GetCriticalYellowsOverDurOH();
            float whites = WhiteAtks.CriticalAtksOverDurOH;
            return yellows + whites;
        }
        /// <summary>This is used by Overpower when you have the glyph</summary>
        /// <returns>Total Parried attacks over fight duration for Maintenance, Fury and Arms abilities</returns>
        public virtual float GetParriedYellowsOverDur() {
            bool useOH = CombatFactors.OH != null && CombatFactors.OHSpeed > 0;

            float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Parry * HS.AvgTargets
                           + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Parry * CL.AvgTargets;

            float yellow = 0f
                // Maintenance
                + _Thunder_GCDs * TH.MHAtkTable.Parry * TH.AvgTargets
                + _Ham_GCDs * HMS.MHAtkTable.Parry * HMS.AvgTargets
                + _Shatt_GCDs * ST.MHAtkTable.Parry * ST.AvgTargets
                // Fury Abilities
                + (useOH ? (_WW_GCDs * WW.MHAtkTable.Parry * WW.AvgTargets + _WW_GCDs * WW.OHAtkTable.Parry * WW.AvgTargets) / 2 : _WW_GCDs * WW.MHAtkTable.Parry * WW.AvgTargets)
                + _SL_GCDs * SL.MHAtkTable.Parry * SL.AvgTargets
                // Arms Abilities
                
            ;

            return yellow + onAttack;
        }
        /// <summary>This is used by Overpower</summary>
        /// <returns>Total Dodged attacks over fight duration for Maintenance, Fury and Arms abilities</returns>
        public virtual float GetDodgedYellowsOverDur() {
            bool useOH = CombatFactors.OH != null && CombatFactors.OHSpeed > 0;

            float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Dodge * HS.AvgTargets
                           + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Dodge * CL.AvgTargets;
            
            float yellow = 0f
                // Maintenance
                + _Thunder_GCDs * TH.MHAtkTable.Dodge * TH.AvgTargets
                + _Ham_GCDs * HMS.MHAtkTable.Dodge * HMS.AvgTargets
                + _Shatt_GCDs * ST.MHAtkTable.Dodge * ST.AvgTargets
                // Fury Abilities
                + (useOH ? (_WW_GCDs * WW.MHAtkTable.Dodge * WW.AvgTargets + _WW_GCDs * WW.OHAtkTable.Dodge * WW.AvgTargets) / 2 : _WW_GCDs * WW.MHAtkTable.Dodge * WW.AvgTargets)
                + _SL_GCDs * SL.MHAtkTable.Dodge * SL.AvgTargets
                // Arms Abilities
                
                
            ;

            return yellow + onAttack;
        }
        // Yellow Only Landed Attacks Over Dur
        public float GetLandedYellowsOverDur() { return GetLandedYellowsOverDurMH() + GetLandedYellowsOverDurOH(); }
        public virtual float GetLandedYellowsOverDurMH() {
            bool useOH = CombatFactors.OH != null && CombatFactors.OHSpeed > 0;

            float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.AnyLand * HS.AvgTargets
                           + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.AnyLand * CL.AvgTargets;

            float yellow = 0f
                // Maintenance
                + _Thunder_GCDs * TH.MHAtkTable.AnyLand * TH.AvgTargets
                + _Ham_GCDs * HMS.MHAtkTable.AnyLand * HMS.AvgTargets
                + _Shatt_GCDs * ST.MHAtkTable.AnyLand * ST.AvgTargets
                // Fury Abilities
                + (_WW_GCDs * WW.MHAtkTable.AnyLand * WW.AvgTargets) / (useOH ? 2 : 1)
                // Arms Abilities
                + _SL_GCDs * SL.MHAtkTable.AnyLand * SL.AvgTargets
            ;

            return yellow + onAttack;
        }
        public virtual float GetLandedYellowsOverDurOH() {
            if (!CombatFactors.useOH) { return 0; }
            float yellow = 0f
                // Maintenance
                // Fury Abilities
                + (_WW_GCDs * WW.OHAtkTable.AnyLand * WW.AvgTargets) / 2
                // Arms Abilities
                
            ;

            return yellow;
        }
        // All Landed Attacks Over Dur, Yellow and White
        public float GetLandedAtksOverDurNoSS() { return GetLandedAtksOverDurNoSSMH() + GetLandedAtksOverDurNoSSOH(); }
        public float GetLandedAtksOverDurNoSSMH() {
            float white = WhiteAtks.LandedAtksOverDurMH;
            float yellow = GetLandedYellowsOverDurMH();

            float result = white + yellow;

            return result;
        }
        public float GetLandedAtksOverDurNoSSOH() {
            if (!CombatFactors.useOH) { return 0; }
            float white = WhiteAtks.LandedAtksOverDurOH;
            float yellow = GetLandedYellowsOverDurOH();

            float result = white + yellow;

            return result;
        }
        public float GetLandedAtksOverDur() { return GetLandedAtksOverDurMH() + GetLandedAtksOverDurOH(); }
        public virtual float GetLandedAtksOverDurMH() {
            return GetLandedAtksOverDurNoSSMH();
        }
        public virtual float GetLandedAtksOverDurOH() {
            bool useOH = Talents.TitansGrip > 0 && CombatFactors.OH != null && CombatFactors.OHSpeed > 0;
            if (!useOH) { return 0; }
            return GetLandedAtksOverDurNoSSOH();
        }
        public float CritHsSlamOverDur {
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

        public float MaintainCDs { get { return _Thunder_GCDs + _Sunder_GCDs + _Demo_GCDs + _Ham_GCDs + _Battle_GCDs + _Comm_GCDs + _ER_GCDs; } }
        #endregion
        #region Rage Calcs
        public virtual float RageGenOverDur_Anger { get { return (Talents.AngerManagement / 3.0f) * CalcOpts.Duration; } }
        public virtual float RageGenOverDur_Wrath { get { return (Talents.UnbridledWrath * 3.0f / 60.0f) * CalcOpts.Duration; } }
        public virtual float RageGenOverDur_Other {
            get {
                if (Char.MainHand == null) { return 0f; }
                float rage = RageGenOverDur_Anger
                           + RageGenOverDur_Wrath;

                // 4pcT7
                if (StatS.BonusWarrior_T7_4P_RageProc != 0f) {
                    rage += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * (Talents.DeepWounds > 0f ? 1f : 0f) * CalcOpts.Duration;
                    rage += (StatS.BonusWarrior_T7_4P_RageProc * 0.1f) * (!CalcOpts.FuryStance && CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_] ? 1f / 3f : 0f) * CalcOpts.Duration;
                }

                return rage;
            }
        }
        public virtual float RageNeededOverDur {
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
                // Total
                float rage = WWRage + SlamRage +
                    ThunderRage + SunderRage + DemoRage + HamstringRage + BattleRage;
                return rage;
            }
        }
        public virtual float FreeRageOverDur {
            get {
                if (Char.MainHand == null) { return 0f; }
                float white = WHITEATTACKS.whiteRageGenOverDur;
                //float sword = SS.GetRageUseOverDur(_SS_Acts);
                float other = RageGenOverDur_Other;
                float needy = RageNeededOverDur;
                return white + other - needy;
            }
        }
        #endregion

        #region AddAnItem(s)
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, Add DPS, Pull Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, float percTimeInStun, ref float _Abil_GCDs, ref float DPS_TTL, ref float HPS_TTL, ref float _Abil_DPS, ref float _Abil_HPS, Skills.Ability abil) {
            float acts = (float)Math.Min(availGCDs, abil.Activates * (1f - percTimeInStun));
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
            GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _Abil_DPS = abil.GetDPS(Abil_GCDs);
            _Abil_HPS = abil.GetHPS(Abil_GCDs);
            DPS_TTL += _Abil_DPS;
            HPS_TTL += _Abil_HPS;
            float rageadd = abil.GetRageUseOverDur(Abil_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, Add DPS, Pull Rage, Use GCD Multiplier</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, float percTimeInStun, ref float _Abil_GCDs, ref float DPS_TTL, ref float HPS_TTL, ref float _Abil_DPS, ref float _Abil_HPS, Skills.Ability abil, float GCDMulti) {
            float acts = (float)Math.Min(availGCDs, abil.Activates * (1f - percTimeInStun));
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs * GCDMulti);
            GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + GCDMulti.ToString() + " : " + abil.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _Abil_DPS = abil.GetDPS(Abil_GCDs);
            _Abil_HPS = abil.GetHPS(Abil_GCDs);
            DPS_TTL += _Abil_DPS;
            HPS_TTL += _Abil_HPS;
            float rageadd = abil.GetRageUseOverDur(Abil_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, No DPS, Pull Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, float percTimeInStun, ref float _Abil_GCDs, ref float HPS_TTL, ref float _Abil_HPS, Skills.Ability abil) {
            float acts = (float)Math.Min(availGCDs, abil.Activates * (1f - percTimeInStun));
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
            GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _Abil_HPS = abil.GetHPS(Abil_GCDs);
            HPS_TTL += _Abil_HPS;
            float rageadd = abil.GetRageUseOverDur(Abil_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: Pull GCDs, No DPS, Add Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float NumGCDs, ref float availGCDs, ref float GCDsused, ref float availRage, float percTimeInStun, ref float _Abil_GCDs, ref float HPS_TTL, ref float _Abil_HPS, Skills.Ability abil, bool flag) {
            float acts = (float)Math.Min(availGCDs, abil.Activates * (1f - percTimeInStun));
            float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_GCDs = Abil_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
            GCDUsage += (Abil_GCDs > 0 ? Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n" : "");
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _Abil_HPS = abil.GetHPS(Abil_GCDs);
            HPS_TTL += _Abil_HPS;
            float rageadd = abil.GetRageUseOverDur(Abil_GCDs);
            RageGenOther += rageadd;
            availRage += rageadd;
        }
        /// <summary>Adds an Ability alteration schtuff. Flags: No GCDs, No DPS, Add Rage, Don't Use GCD Multiplier</summary>
        public void AddAnItem(ref float availRage, float percTimeInStun, ref float _Abil_Acts, ref float HPS_TTL, ref float _Abil_HPS, Skills.Ability abil) {
            float acts = abil.Activates * (1f - percTimeInStun);
            float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
            _Abil_Acts = Abil_Acts;
            GCDUsage += (Abil_Acts > 0 ? Abil_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + " (Doesn't use GCDs)\n" : "");
            _Abil_HPS = abil.GetHPS(Abil_Acts);
            HPS_TTL += _Abil_HPS;
            float rageadd = abil.GetRageUseOverDur(Abil_Acts);
            RageGenOther += rageadd;
            availRage += rageadd;
        }
        #endregion

        
        
    }
}
