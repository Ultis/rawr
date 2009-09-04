using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class Rotation {
        // Constructors
        public Rotation(Character character, Stats stats) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CombatFactors = new CombatFactors(Char, StatS);
            CalcOpts = Char == null || Char.CalculationOptions == null ? new CalculationOptionsDPSWarr() : Char.CalculationOptions as CalculationOptionsDPSWarr;
            //WhiteAtks = new Skills.WhiteAttacks(Char, StatS);
            // Initialize();
        }
        #region Variables
        private Character CHARACTER;
        private WarriorTalents TALENTS;
        private Stats STATS;
        private CombatFactors COMBATFACTORS;
        private Skills.WhiteAttacks WHITEATTACKS;
        private CalculationOptionsDPSWarr CALCOPTS;

        public float _HPS_TTL;

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
        // Fury
        public Skills.WhirlWind WW;
        public Skills.BloodThirst BT;
        public Skills.BloodSurge BS;
        // Arms
        public Skills.Bladestorm BLS;
        public Skills.MortalStrike MS;
        public Skills.Rend RD;
        public Skills.OverPower OP;
        public Skills.TasteForBlood TB;
        public Skills.Suddendeath SD;
        public Skills.Slam SL;
        public Skills.Swordspec SS;
        // Generic
        public Skills.DeepWounds DW;
        public Skills.Cleave CL;
        public Skills.HeroicStrike HS;
        
        public const float ROTATION_LENGTH_FURY = 8.0f;
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
        public void Initialize(CharacterCalculationsDPSWarr calcs) {
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
            // Fury
            calcs.WW = WW;
            calcs.BT = BT;
            calcs.BS = BS;
            // Arms
            calcs.BLS = BLS;
            calcs.MS = MS;
            calcs.RD = RD;
            calcs.OP = OP;
            calcs.TB = TB;
            calcs.SD = SD;
            calcs.SL = SL;
            calcs.SS = SS;
            // Generic
            calcs.CL = CL;
            calcs.DW = DW;
            calcs.HS = HS;
        }
        public void Initialize() { initAbilities(); doIterations(); }

        private void initAbilities() {
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
            // Fury
            WW = new Skills.WhirlWind(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BT = new Skills.BloodThirst(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SL = new Skills.Slam(               CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS); // actually arms but BS needs it
            BS = new Skills.BloodSurge(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,SL,WW,BT);
            // Arms
            BLS= new Skills.Bladestorm(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,WW);
            MS = new Skills.MortalStrike(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            RD = new Skills.Rend(               CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SS = new Skills.Swordspec(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            OP = new Skills.OverPower(          CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS,SS);
            TB = new Skills.TasteForBlood(      CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SD = new Skills.Suddendeath(        CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            // Generic DPS
            DW = new Skills.DeepWounds(         CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            CL = new Skills.Cleave(             CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            HS = new Skills.HeroicStrike(       CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
        }
        private void doIterations() {
            // Fury Iteration
            bool hsok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool clok = CalcOpts.MultipleTargets
                     && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];

            float hsPercOvd, clPercOvd; // what percentage of overrides are cleave and hs
            hsPercOvd = (hsok?1f:0f);
            if (CalcOpts.MultipleTargets && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_])
                hsPercOvd -= CalcOpts.MultipleTargetsPerc / 100f;
            clPercOvd = (clok ? 1f-hsPercOvd : 0f);

            float hsRageUsed = FreeRageOverDur * hsPercOvd;
            float clRageUsed = FreeRageOverDur * clPercOvd;

            WhiteAtks.HSOverridesOverDur = HS.OverridesOverDur = hsRageUsed / HS.FullRageCost;
            WhiteAtks.CLOverridesOverDur = CL.OverridesOverDur = clRageUsed / CL.FullRageCost;

            float oldHSActivates = 0f, newHSActivates = HS.Activates;
            float oldCLActivates = 0f, newCLActivates = CL.Activates;
            BS.maintainActs = MaintainCDs;
            for (int loopIterator = 0; 
                 CalcOpts.FuryStance
                    && loopIterator < 50
                    &&    (Math.Abs(newHSActivates - oldHSActivates) > 0.01f
                        || Math.Abs(newCLActivates - oldCLActivates) > 0.01f);
                  loopIterator++)
            {
                oldHSActivates = HS.Activates;
                oldCLActivates = CL.Activates;
                //
                BS.hsActivates = oldHSActivates; // bloodsurge only cares about HSes, not Cleaves
                _bloodsurgeRPS = HS.bloodsurgeRPS = CL.bloodsurgeRPS = (BS.RageUseOverDur);
                hsRageUsed = FreeRageOverDur * hsPercOvd;
                clRageUsed = FreeRageOverDur * clPercOvd;
                WhiteAtks.HSOverridesOverDur = HS.OverridesOverDur = hsRageUsed / HS.FullRageCost;
                WhiteAtks.CLOverridesOverDur = CL.OverridesOverDur = clRageUsed / CL.FullRageCost;
                //
                newHSActivates = HS.Activates;
                newCLActivates = CL.Activates;
            }
            BS.hsActivates  = newHSActivates;
            BS.hsActivates += newCLActivates;
            if (CalcOpts.FuryStance) {
                _HS_DPS  = HS.DPS;
                _CL_DPS += CL.DPS;
                _HS_PerHit = HS.DamageOnUse * hsPercOvd;
                _CL_PerHit = CL.DamageOnUse * clPercOvd;
            }
        }

        private void calcDeepWounds() {
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

        /// <summary>This is used by Deep Wounds</summary>
        /// <returns>Total Critical attacks over fight duration for Maintenance, Fury and Arms abilities. Does not include Heroic Strikes or Cleaves</returns>
        public float GetCriticalYellowsOverDur() { return GetCriticalYellowsOverDurMH() + GetCriticalYellowsOverDurOH(); }
        public float GetCriticalYellowsOverDurMH() {
            bool useOH = CombatFactors.OH != null && CombatFactors.OHSpeed > 0;

            float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Crit * HS.AvgTargets
                           + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Crit * CL.AvgTargets;

            float yellow = 0f
                // Maintenance
                + _Thunder_GCDs * TH.MHAtkTable.Crit * TH.AvgTargets
                + _Ham_GCDs * HMS.MHAtkTable.Crit * HMS.AvgTargets
                + _Shatt_GCDs * ST.MHAtkTable.Crit * ST.AvgTargets
                // Fury Abilities
                + _BT_GCDs * BT.MHAtkTable.Crit * BT.AvgTargets
                + (_WW_GCDs * WW.MHAtkTable.Crit * WW.AvgTargets * 6) / (useOH ? 2 : 1)
                + _BS_GCDs * BS.MHAtkTable.Crit * BS.AvgTargets
                // Arms Abilities
                + (_BLS_GCDs * BLS.MHAtkTable.Crit * BLS.AvgTargets * 6) / (useOH ? 2 : 1)
                + _MS_GCDs * MS.MHAtkTable.Crit * MS.AvgTargets
                + _OP_GCDs * OP.MHAtkTable.Crit * OP.AvgTargets
                + _TB_GCDs * TB.MHAtkTable.Crit * TB.AvgTargets
                + _SD_GCDs * SD.MHAtkTable.Crit * SD.AvgTargets
                + _SL_GCDs * SL.MHAtkTable.Crit * SL.AvgTargets
            ;

            return yellow + onAttack;
        }
        public float GetCriticalYellowsOverDurOH() {
            bool useOH = Talents.TitansGrip > 0 && CombatFactors.OH != null && CombatFactors.OHSpeed > 0;
            if (!useOH) { return 0; }
            float yellow = 0f
                // Maintenance
                // Fury Abilities
                + (_WW_GCDs * WW.OHAtkTable.Crit * WW.AvgTargets) / 2
                // Arms Abilities
                + (_BLS_GCDs * BLS.OHAtkTable.Crit * BLS.AvgTargets * 6) / 2
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
            bool useOH = Talents.TitansGrip > 0 && CombatFactors.OH != null && CombatFactors.OHSpeed > 0;
            if (!useOH) { return 0; }
            float yellows = GetCriticalYellowsOverDurOH();
            float whites = WhiteAtks.CriticalAtksOverDurOH;
            return yellows + whites;
        }
        /// <summary>This is used by Overpower when you have the glyph</summary>
        /// <returns>Total Parried attacks over fight duration for Maintenance, Fury and Arms abilities</returns>
        public float GetParriedYellowsOverDur() {
            bool useOH = CombatFactors.OH != null && CombatFactors.OHSpeed > 0;

            float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Parry * HS.AvgTargets
                           + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Parry * CL.AvgTargets;

            float yellow = 0f
                // Maintenance
                + _Thunder_GCDs * TH.MHAtkTable.Parry * TH.AvgTargets
                + _Ham_GCDs * HMS.MHAtkTable.Parry * HMS.AvgTargets
                + _Shatt_GCDs * ST.MHAtkTable.Parry * ST.AvgTargets
                // Fury Abilities
                + _BT_GCDs * BT.MHAtkTable.Parry * BT.AvgTargets
                + (useOH ? (_WW_GCDs * WW.MHAtkTable.Parry * WW.AvgTargets + _WW_GCDs * WW.OHAtkTable.Parry * WW.AvgTargets) / 2 : _WW_GCDs * WW.MHAtkTable.Parry * WW.AvgTargets)
                + _BS_GCDs * BS.MHAtkTable.Parry * BS.AvgTargets
                // Arms Abilities
                + (useOH ? (_BLS_GCDs * BLS.MHAtkTable.Parry * BLS.AvgTargets + _BLS_GCDs * BLS.OHAtkTable.Parry * BLS.AvgTargets) / 2 : _BLS_GCDs * BLS.MHAtkTable.Parry * BLS.AvgTargets) * 6
                + _MS_GCDs * MS.MHAtkTable.Parry * MS.AvgTargets
                + _OP_GCDs * OP.MHAtkTable.Parry * OP.AvgTargets
                + _TB_GCDs * TB.MHAtkTable.Parry * TB.AvgTargets
                + _SD_GCDs * SD.MHAtkTable.Parry * SD.AvgTargets
                + _SL_GCDs * SL.MHAtkTable.Parry * SL.AvgTargets
            ;

            return yellow + onAttack;
        }
        /// <summary>This is used by Overpower</summary>
        /// <returns>Total Dodged attacks over fight duration for Maintenance, Fury and Arms abilities</returns>
        public float GetDodgedYellowsOverDur() {
            bool useOH = CombatFactors.OH != null && CombatFactors.OHSpeed > 0;

            float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.Dodge * HS.AvgTargets
                           + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.Dodge * CL.AvgTargets;
            
            float yellow = 0f
                // Maintenance
                + _Thunder_GCDs * TH.MHAtkTable.Dodge * TH.AvgTargets
                + _Ham_GCDs * HMS.MHAtkTable.Dodge * HMS.AvgTargets
                + _Shatt_GCDs * ST.MHAtkTable.Dodge * ST.AvgTargets
                // Fury Abilities
                + _BT_GCDs * BT.MHAtkTable.Dodge * BT.AvgTargets
                + (useOH ? (_WW_GCDs * WW.MHAtkTable.Dodge * WW.AvgTargets + _WW_GCDs * WW.OHAtkTable.Dodge * WW.AvgTargets) / 2 : _WW_GCDs * WW.MHAtkTable.Dodge * WW.AvgTargets)
                + _BS_GCDs * BS.MHAtkTable.Dodge * BS.AvgTargets
                // Arms Abilities
                + (useOH ? (_BLS_GCDs * BLS.MHAtkTable.Dodge * BLS.AvgTargets + _BLS_GCDs * BLS.OHAtkTable.Dodge * BLS.AvgTargets) / 2 : _BLS_GCDs * BLS.MHAtkTable.Dodge * BLS.AvgTargets) * 6
                + _MS_GCDs * MS.MHAtkTable.Dodge * MS.AvgTargets
                + _OP_GCDs * OP.MHAtkTable.Dodge * OP.AvgTargets
                + _TB_GCDs * TB.MHAtkTable.Dodge * TB.AvgTargets
                + _SD_GCDs * SD.MHAtkTable.Dodge * SD.AvgTargets
                + _SL_GCDs * SL.MHAtkTable.Dodge * SL.AvgTargets
            ;

            return yellow + onAttack;
        }
        // Yellow Only Landed Attacks Over Dur
        public float GetLandedYellowsOverDur() { return GetLandedYellowsOverDurMH() + GetLandedYellowsOverDurOH(); }
        public float GetLandedYellowsOverDurMH() {
            bool useOH = CombatFactors.OH != null && CombatFactors.OHSpeed > 0;

            float onAttack = WhiteAtks.HSOverridesOverDur * HS.MHAtkTable.AnyLand * HS.AvgTargets
                           + WhiteAtks.CLOverridesOverDur * CL.MHAtkTable.AnyLand * CL.AvgTargets;

            float yellow = 0f
                // Maintenance
                + _Thunder_GCDs * TH.MHAtkTable.AnyLand * TH.AvgTargets
                + _Ham_GCDs * HMS.MHAtkTable.AnyLand * HMS.AvgTargets
                + _Shatt_GCDs * ST.MHAtkTable.AnyLand * ST.AvgTargets
                // Fury Abilities
                + _BT_GCDs * BT.MHAtkTable.AnyLand * BT.AvgTargets
                + (_WW_GCDs * WW.MHAtkTable.AnyLand * WW.AvgTargets) / (useOH ? 2 : 1)
                + _BS_GCDs * BS.MHAtkTable.AnyLand * BS.AvgTargets
                // Arms Abilities
                + (_BLS_GCDs * BLS.MHAtkTable.AnyLand * BLS.AvgTargets * 6) / (useOH ? 2 : 1)
                + _MS_GCDs * MS.MHAtkTable.AnyLand * MS.AvgTargets
                + _OP_GCDs * OP.MHAtkTable.AnyLand * OP.AvgTargets
                + _TB_GCDs * TB.MHAtkTable.AnyLand * TB.AvgTargets
                + _SD_GCDs * SD.MHAtkTable.AnyLand * SD.AvgTargets
                + _SL_GCDs * SL.MHAtkTable.AnyLand * SL.AvgTargets
            ;

            return yellow + onAttack;
        }
        public float GetLandedYellowsOverDurOH() {
            bool useOH = Talents.TitansGrip > 0 && CombatFactors.OH != null && CombatFactors.OHSpeed > 0;
            if (!useOH) { return 0; }
            float yellow = 0f
                // Maintenance
                // Fury Abilities
                + (_WW_GCDs * WW.OHAtkTable.AnyLand * WW.AvgTargets) / 2
                // Arms Abilities
                + (_BLS_GCDs * BLS.OHAtkTable.AnyLand * BLS.AvgTargets * 6) / 2
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
            bool useOH = Talents.TitansGrip > 0 && CombatFactors.OH != null && CombatFactors.OHSpeed > 0;
            if (!useOH) { return 0; }
            float white = WhiteAtks.LandedAtksOverDurOH;
            float yellow = GetLandedYellowsOverDurOH();

            float result = white + yellow;

            return result;
        }
        public float GetLandedAtksOverDur() { return GetLandedAtksOverDurMH() + GetLandedAtksOverDurOH(); }
        public float GetLandedAtksOverDurMH() {
            float landednoss = GetLandedAtksOverDurNoSSMH();
            float ssActs = SS.GetActivates(GetLandedYellowsOverDurMH());

            ssActs *= WhiteAtks.MHAtkTable.AnyLand;

            return landednoss + (float)Math.Max(0f, ssActs);
        }
        public float GetLandedAtksOverDurOH() {
            bool useOH = Talents.TitansGrip > 0 && CombatFactors.OH != null && CombatFactors.OHSpeed > 0;
            if (!useOH) { return 0; }
            float landednoss = GetLandedAtksOverDurNoSSOH();
            float ssActs = SS.GetActivates(GetLandedYellowsOverDurOH());

            ssActs *= WhiteAtks.MHAtkTable.AnyLand;

            return landednoss + (float)Math.Max(0f, ssActs);
        }
        // Stuff for Fury
        public float CritHsSlamOverDur {
            get {
                float critsPerRot = 0;
                if (CalcOpts.FuryStance) {
                    critsPerRot = HS.Activates * HS.MHAtkTable.Crit +
                                  SL.Activates * SL.MHAtkTable.Crit;
                }else{
                    critsPerRot = _HS_Acts     * HS.MHAtkTable.Crit +
                                  _SL_GCDs     * SL.MHAtkTable.Crit;
                }
                return critsPerRot;
            }
        }
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
                float BTRage         = BT.GetRageUseOverDur(_BT_GCDs);
                float WWRage         = WW.GetRageUseOverDur(_WW_GCDs);
                float BloodSurgeRage = _bloodsurgeRPS;// BS.RageUsePerSecond;
                // Arms
                float SweepingRage   = SW.GetRageUseOverDur( _SW_GCDs    );
                float BladestormRage = BLS.GetRageUseOverDur(_BLS_GCDs   );
                float MSRage         = MS.GetRageUseOverDur( _MS_GCDs    );
                float RendRage       = RD.GetRageUseOverDur( _RD_GCDs    );
                float OPRage         = OP.GetRageUseOverDur( _OP_GCDs    );
                float TBRage         = TB.GetRageUseOverDur( _TB_GCDs    );
                float SDRage         = SD.GetRageUseOverDur( _SD_GCDs    );
                float SlamRage       = SL.GetRageUseOverDur( _SL_GCDs    );
                // Maintenance
                float ThunderRage    = TH.GetRageUseOverDur( _TH_DPS     );
                float SunderRage     = SN.GetRageUseOverDur( _Sunder_GCDs);
                float DemoRage       = DS.GetRageUseOverDur( _Demo_GCDs  );
                float HamstringRage  = HMS.GetRageUseOverDur(_Ham_GCDs   );
                float BattleRage     = BTS.GetRageUseOverDur(_Battle_GCDs);
                // Total
                float rage = BTRage + WWRage + MSRage + OPRage + TBRage + SDRage + SlamRage +
                    BloodSurgeRage + SweepingRage + BladestormRage + RendRage +
                    ThunderRage + SunderRage + DemoRage + HamstringRage + BattleRage;
                return rage;
            }
        }
        public virtual float FreeRageOverDur {
            get {
                if (Char.MainHand == null) { return 0f; }
                float white = WHITEATTACKS.whiteRageGenOverDur;
                float sword = SS.GetRageUseOverDur(_SS_Acts);
                float other = RageGenOverDur_Other;
                float needy = RageNeededOverDur;
                return white + sword + other - needy;
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

        #region FuryRotVariables
        float _bloodsurgeRPS;
        public float _BS_DPS = 0f, _BS_HPS = 0f, _BS_GCDs = 0f;
        public float _BT_DPS = 0f, _BT_HPS = 0f, _BT_GCDs = 0f;
        public float _WW_DPS = 0f, _WW_HPS = 0f, _WW_GCDs = 0f;
        #endregion
        public float MakeRotationandDoDPS_Fury() {
            // Starting Numbers
            float DPS_TTL = 0f, HPS_TTL = 0f;
            float FightDuration = CalcOpts.Duration;
            float LatentGCD = 1.5f + CalcOpts.GetLatency();
            float NumGCDs = FightDuration / LatentGCD;
            GCDUsage += "NumGCDs: " + NumGCDs.ToString() + "\n\n";
            float GCDsused = 0f;
            float availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;
            float rageadd = 0f;
            //float timelostwhilestunned = 0f;
            float percTimeInStun = 0f;

            if (Char.MainHand == null) { return 0f; }

            doIterations();

            // ==== Rage Generation Priorities ========
            availRage += RageGenOverDur_Other;

            /*Bloodrage         */AddAnItem(                                       ref availRage,percTimeInStun,ref _Blood_GCDs,              ref HPS_TTL,               ref _Blood_HPS,  BR);
            /*Berserker Rage    */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _ZRage_GCDs,              ref HPS_TTL,               ref _ZRage_HPS,  BZ,false);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Battle_GCDs,             ref HPS_TTL,               ref _Battle_HPS, BTS);
            /*Commanding Shout  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Comm_GCDs,               ref HPS_TTL,               ref _Comm_HPS,   CS);
            /*Demoralizing Shout*/AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Demo_GCDs,               ref HPS_TTL,               ref _Demo_HPS,   DS);
            /*Sunder Armor      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Sunder_GCDs,             ref HPS_TTL,               ref _Sunder_HPS, SN);
            /*Thunder Clap      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Thunder_GCDs,ref DPS_TTL,ref HPS_TTL,ref _TH_DPS,   ref _TH_HPS,     TH);
            /*Hamstring         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Ham_GCDs,                ref HPS_TTL,               ref _Ham_HPS,    HMS);
            /*Shattering Throw  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Shatt_GCDs,  ref DPS_TTL,ref HPS_TTL,ref _Shatt_DPS,ref _Shatt_HPS,  ST);
            /*Enraged Regeneratn*/AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _ER_GCDs,                 ref HPS_TTL,               ref _ER_HPS,     ER);
            /*Sweeping Strikes  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _SW_GCDs,                 ref HPS_TTL,               ref _SW_HPS,     SW);
            /*Death Wish        */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Death_GCDs,              ref HPS_TTL,               ref _Death_HPS,  Death);

            /*float Reck_GCDs = (float)Math.Min(availGCDs, RK.Activates);
            _Reck_GCDs = Reck_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, Reck_GCDs);
            GCDUsage += RK.Name + ": " + Reck_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            rageadd = RK.GetRageUsePerSecond(Reck_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;*/

            doIterations();

            // Priority 1 : Whirlwind on every CD
            float WW_GCDs = (float)Math.Min(availGCDs, WW.Activates);
            _WW_GCDs = WW_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, WW_GCDs);
            GCDUsage += WW.Name + ": " + WW_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _WW_DPS = WW.GetDPS(WW_GCDs);
            _WW_HPS = WW.GetHPS(WW_GCDs);
            DPS_TTL += _WW_DPS;
            HPS_TTL += _WW_HPS;
            rageadd = WW.GetRageUseOverDur(WW_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 2 : Bloodthirst on every CD
            float BT_GCDs = (float)Math.Min(availGCDs, BT.Activates);
            _BT_GCDs = BT_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, BT_GCDs);
            GCDUsage += BT.Name + ": " + BT_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _BT_DPS = BT.GetDPS(BT_GCDs);
            _BT_HPS = BT.GetHPS(BT_GCDs);
            DPS_TTL += _BT_DPS;
            HPS_TTL += _BT_HPS;
            rageadd = BT.GetRageUseOverDur(BT_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            doIterations();
            // Priority 3 : Bloodsurge Blood Proc (Do an Instant Slam) if available
            float BS_GCDs = (float)Math.Min(availGCDs, BS.Activates);
            _BS_GCDs = BS_GCDs;
            GCDsused += (float)Math.Min(NumGCDs, BS_GCDs);
            GCDUsage += BS.Name + ": " + BS_GCDs.ToString() + "\n";
            availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            _BS_DPS = BS.GetDPS(BS_GCDs);
            _BS_HPS = BS.GetHPS(BS_GCDs);
            DPS_TTL += _BS_DPS;
            HPS_TTL += _BS_HPS;
            rageadd = BS.GetRageUseOverDur(BS_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            //Sword Spec, Doesn't eat GCDs
            float SS_Acts = SS.GetActivates(GetLandedYellowsOverDur());
            _SS_Acts = SS_Acts;
            _SS_DPS  = SS.GetDPS(SS_Acts);
            DPS_TTL += _SS_DPS;
            // TODO: Add Rage since it's a white hit
            
            doIterations();
            // Priority 4 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            // After iterating how many Overrides can be done and still do other abilities, then do the white dps

            availRage += WhiteAtks.MHRageGenOverDur + WhiteAtks.OHRageGenOverDur;

            bool HSok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool CLok = CalcOpts.MultipleTargets && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];

            WhiteAtks.Slam_Freq = _SL_GCDs;
            if (HSok || CLok) {
                float numHSOverDur = availRage / HS.FullRageCost;
                HS.OverridesOverDur = numHSOverDur;
                WhiteAtks.HSOverridesOverDur = numHSOverDur;
                WhiteAtks.CLOverridesOverDur = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS; // MhWhiteDPS
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _HS_DPS = HS.DPS;
                _HS_PerHit = HS.DamageOnUse;
                _CL_DPS = CL.DPS;
                _CL_PerHit = CL.DamageOnUse;
                DPS_TTL += _WhiteDPS;
                DPS_TTL += _HS_DPS;
                //DPS_TTL += _CL_DPS;
            } else {
                RageGenWhite = WHITEATTACKS.whiteRageGenOverDur;
                availRage += RageGenWhite;
                WhiteAtks.HSOverridesOverDur = 0f;
                WhiteAtks.CLOverridesOverDur = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS; // MhWhiteDPS
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _HS_DPS = _HS_PerHit = _HS_Acts = 0;
                _CL_DPS = _CL_PerHit = _CL_Acts = 0;
                DPS_TTL += _WhiteDPS;
            }
            calcDeepWounds();

            GCDUsage += "\nAvail: " + availGCDs.ToString();

            // Return result
            _HPS_TTL = HPS_TTL;
            return DPS_TTL;
        }
        #region ArmsRotVariables
        public float _MS_DPS = 0f, _MS_HPS = 0f, _MS_GCDs = 0f;
        public float _RD_DPS = 0f, _RD_HPS = 0f, _RD_GCDs = 0f;
        public float _OP_DPS = 0f, _OP_HPS = 0f, _OP_GCDs = 0f;
        public float _TB_DPS = 0f, _TB_HPS = 0f, _TB_GCDs = 0f;
        public float _SD_DPS = 0f, _SD_HPS = 0f, _SD_GCDs = 0f;
        public float _SL_DPS = 0f, _SL_HPS = 0f, _SL_GCDs = 0f;
        public float _SS_DPS = 0f, _SS_HPS = 0f, _SS_Acts = 0f;
        public float _BLS_DPS = 0f, _BLS_HPS = 0f, _BLS_GCDs = 0f;
        public float _SW_DPS = 0f, _SW_HPS = 0f, _SW_GCDs = 0f;
        public float _DW_PerHit   = 0f, _DW_DPS       = 0f; 
        //
        public float _Trink1_GCDs = 0f;
        public float _Trink2_GCDs = 0f;
        //
        public float _Thunder_GCDs = 0f, _TH_DPS = 0f, _TH_HPS = 0f;
        public float _Blood_GCDs  = 0f, _Blood_HPS = 0f;
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
        // GCD Losses
        public float _Stunned_Acts = 0f;
        public float _HF_Acts      = 0f;
        public float _EM_Acts      = 0f;
        //
        public float _HS_PerHit = 0f, _HS_DPS = 0f, _HS_Acts = 0f;
        public float _CL_PerHit = 0f, _CL_DPS = 0f, _CL_Acts = 0f;
        public float _WhiteDPSMH  = 0f; public float _WhiteDPSOH   = 0f;
        public float _WhiteDPS    = 0f; public float _WhitePerHit  = 0f;
        public float RageGenWhite = 0f; public float RageGenOther  = 0f; public float RageNeeded = 0f;
        public string GCDUsage = "";
        #endregion
        public float MakeRotationandDoDPS_Arms() {
            // Starting Numbers
            float DPS_TTL = 0f, HPS_TTL = 0f;
            float FightDuration = CalcOpts.Duration;
            float LatentGCD = 1.5f + CalcOpts.GetLatency();
            float NumGCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(FightDuration / LatentGCD) : FightDuration / LatentGCD;
            GCDUsage += NumGCDs.ToString("000") + " : Total GCDs\n\n";
            float GCDsused = 0f;
            float availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;
            float rageadd = 0f;
            float timelostwhilestunned = 0f;
            float percTimeInStun = 0f;

            if (Char.MainHand == null) { return 0f; }

            // ==== Reasons GCDs would be lost ========
            float IronWillBonus = (float)Math.Round(20f/3f*Talents.IronWill,MidpointRounding.AwayFromZero) / 100f;
            float BaseStunDur = (float)Math.Max(0f, (CalcOpts.StunningTargetsDur/1000f * (1f - IronWillBonus)));
            // Being Stunned or Charmed
            if(CalcOpts.StunningTargets){
                // Assume you are Stunned for 3 GCDs (1.5+latency)*3 = ~1.6*3 = ~4.8 seconds per stun
                // Iron Will reduces the Duration of the stun by 7%,14%,20%
                // 100% perc means you are stunned the entire fight, the boss is stunning you every third GCD, basically only refreshing his stun
                //  50% perc means you are stunned half the fight, the boss is stunning you every sixth GCD
                float stunnedActs = (float)Math.Max(0f, FightDuration / CalcOpts.StunningTargetsFreq);
                //float acts = (float)Math.Min(availGCDs, stunnedGCDs);
                float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Floor(stunnedActs) : stunnedActs;
                _Stunned_Acts = Abil_Acts;
                float reduc = Math.Max(0f,BaseStunDur);
                GCDsused += (float)Math.Min(NumGCDs, (reduc * Abil_Acts) / LatentGCD);
                GCDUsage += (Abil_Acts > 0 ? Abil_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs-IronWillBonus : Stunned\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                // Now let's try and get some of those GCDs back
                if (Talents.HeroicFury > 0 && _Stunned_Acts > 0f) {
                    float hfacts = HF.Activates;
                    _HF_Acts = (float)Math.Min(_Stunned_Acts, hfacts);
                    reduc =  Math.Max(0f,(BaseStunDur - Math.Max(0f,/*(*/CalcOpts.React/*-250)/1000f*/)));
                    GCDsused -= (float)Math.Min(NumGCDs, (reduc * hfacts) / LatentGCD);
                    GCDUsage += (_HF_Acts > 0 ? _HF_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs : " + HF.Name + " (adds back to GCDs when stunned)\n" : "");
                    availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                }
                if (CHARACTER.Race == CharacterRace.Human && (_Stunned_Acts - _HF_Acts > 0)) {
                    float emacts = EM.Activates;
                    _EM_Acts = (float)Math.Min(_Stunned_Acts - _HF_Acts, emacts);
                    reduc = Math.Max(0f, (BaseStunDur - Math.Max(0f,/*(*/CalcOpts.React/*-250)/1000f*/)));
                    GCDsused -= (float)Math.Min(NumGCDs, (reduc * emacts) / LatentGCD);
                    GCDUsage += (_EM_Acts > 0 ? _EM_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs : " + EM.Name + " (adds back to GCDs when stunned)\n" : "");
                    availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                }

                // Now to give Stunned GCDs back and later we'll use %
                // of time lost to stuns to affect each ability equally
                // othwerwise we are only seriously affecting things at
                // the bottom of priorities, which isn't fair (poor Slam)
                GCDsused -= (_Stunned_Acts * BaseStunDur) / LatentGCD;
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                timelostwhilestunned = _Stunned_Acts * BaseStunDur
                                       - (BaseStunDur - LatentGCD) * _HF_Acts
                                       - (BaseStunDur - LatentGCD) * _EM_Acts;
                percTimeInStun = timelostwhilestunned / FightDuration;
            }

            // ==== Rage Generation Priorities ========
            availRage += RageGenOverDur_Other;

            SndW.NumStunsOverDur = _Stunned_Acts;
            /*Second Wind       */AddAnItem(                                       ref availRage,percTimeInStun,ref _Second_Acts,             ref HPS_TTL,               ref _Second_HPS, SndW);
            /*Bloodrage         */AddAnItem(                                       ref availRage,percTimeInStun,ref _Blood_GCDs,              ref HPS_TTL,               ref _Blood_HPS,  BR);
            /*Berserker Rage    */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _ZRage_GCDs,              ref HPS_TTL,               ref _ZRage_HPS,  BZ,false);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Battle_GCDs,             ref HPS_TTL,               ref _Battle_HPS, BTS);
            /*Commanding Shout  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Comm_GCDs,               ref HPS_TTL,               ref _Comm_HPS,   CS);
            /*Demoralizing Shout*/AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Demo_GCDs,               ref HPS_TTL,               ref _Demo_HPS,   DS);
            /*Sunder Armor      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Sunder_GCDs,             ref HPS_TTL,               ref _Sunder_HPS, SN);
            /*Thunder Clap      */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Thunder_GCDs,ref DPS_TTL,ref HPS_TTL,ref _TH_DPS,   ref _TH_HPS,     TH);
            /*Hamstring         */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Ham_GCDs,                ref HPS_TTL,               ref _Ham_HPS,    HMS);
            /*Shattering Throw  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Shatt_GCDs,  ref DPS_TTL,ref HPS_TTL,ref _Shatt_DPS,ref _Shatt_HPS,  ST);
            /*Enraged Regeneratn*/AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _ER_GCDs,                 ref HPS_TTL,               ref _ER_HPS,     ER);
            /*Sweeping Strikes  */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _SW_GCDs,                 ref HPS_TTL,               ref _SW_HPS,     SW);
            /*Death Wish        */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _Death_GCDs,              ref HPS_TTL,               ref _Death_HPS,  Death);
            
            // ==== Standard Priorities ===============

            // These are solid and not dependant on other attacks
            /*Bladestorm        */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _BLS_GCDs,    ref DPS_TTL,ref HPS_TTL,ref _BLS_DPS,  ref _BLS_HPS,    BLS, 4f);
            /*Mortal Strike     */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _MS_GCDs,     ref DPS_TTL,ref HPS_TTL,ref _MS_DPS,   ref _MS_HPS,     MS);
            /*Rend              */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _RD_GCDs,     ref DPS_TTL,ref HPS_TTL,ref _RD_DPS,   ref _RD_HPS,     RD);
            /*Taste for Blood   */AddAnItem(ref NumGCDs,ref availGCDs,ref GCDsused,ref availRage,percTimeInStun,ref _TB_GCDs,     ref DPS_TTL,ref HPS_TTL,ref _TB_DPS,   ref _TB_HPS,     TB);

            // The following are dependant on other attacks as they are proccing abilities or are the fallback item
            // We need to loop these until the activates are relatively unchanged
            float origavailGCDs = availGCDs;
            float origGCDsused = GCDsused;
            float oldOPGCDs = 0f, oldSDGCDs = 0f, oldSLGCDs = 0f, oldSSActs = 0f;
            _OP_GCDs = 0f;
            _SD_GCDs = 0f;
            _SL_GCDs = origavailGCDs;
            _SS_Acts = 0f;
            int loopCounter = 0;
            while (
                    loopCounter < 500 &&
                    ((float)Math.Abs(_OP_GCDs - oldOPGCDs) > 0.1f ||
                     (float)Math.Abs(_SD_GCDs - oldSDGCDs) > 0.1f ||
                     (float)Math.Abs(_SL_GCDs - oldSLGCDs) > 0.1f ||
                     (Talents.SwordSpecialization > 0
                        && CombatFactors.MH.Type == ItemType.TwoHandSword
                        && (float)Math.Abs(_SS_Acts - oldSSActs) > 0.1f)
                    )
                  )
            {
                // Reset a couple of items so we can keep iterating
                availGCDs = origavailGCDs;
                GCDsused = origGCDsused;
                oldOPGCDs = _OP_GCDs; oldSDGCDs = _SD_GCDs; oldSLGCDs = _SL_GCDs; oldSSActs = _SS_Acts;
                WhiteAtks.Slam_Freq = _SL_GCDs;
                //Overpower
                float acts = (float)Math.Min(availGCDs, OP.GetActivates(GetDodgedYellowsOverDur(), GetParriedYellowsOverDur(), _SS_Acts) * (1f - percTimeInStun));
                float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _OP_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);

                //Sudden Death
                acts = (float)Math.Min(availGCDs, SD.GetActivates(GetLandedAtksOverDur()) * (1f - percTimeInStun));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _SD_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);

                //Slam for remainder of GCDs
                _SL_GCDs = SL.Validated ? availGCDs * (1f - SL.Whiteattacks.AvoidanceStreak) * (1f - percTimeInStun) : 0f;
                GCDsused += (float)Math.Min(NumGCDs, _SL_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);

                //Sword Spec, Doesn't eat GCDs
                float SS_Acts = SS.GetActivates(GetLandedYellowsOverDur());
                _SS_Acts = SS_Acts;
                loopCounter++;
            }
            // Can't manage FreeRage for SD yet
            rageadd = SL.GetRageUseOverDur(_SL_GCDs)
                    + OP.GetRageUseOverDur(_OP_GCDs)
                    - SS.GetRageUseOverDur(_SS_Acts);
            availRage -= rageadd;
            RageNeeded += rageadd;
            // manage it now
            SD.FreeRage = availRage;
            rageadd = SD.GetRageUseOverDur(_SD_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;
            // move on
            GCDUsage += (_OP_GCDs > 0 ? _OP_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x2 : " + OP.Name + "\n" : "");
            GCDUsage += (_SD_GCDs > 0 ? _SD_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SD.Name + "\n" : "");
            GCDUsage += (_SL_GCDs > 0 ? _SL_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SL.Name + "\n" : "");
            _OP_DPS = OP.GetDPS(_OP_GCDs); _OP_HPS = OP.GetHPS(_OP_GCDs);
            _SD_DPS = SD.GetDPS(_SD_GCDs); _SD_HPS = SD.GetHPS(_SD_GCDs);
            _SL_DPS = SL.GetDPS(_SL_GCDs); _SL_HPS = SL.GetHPS(_SL_GCDs);
            if (Talents.SwordSpecialization > 0 && CombatFactors.MH.Type == ItemType.TwoHandSword) { _SS_DPS = SS.GetDPS(_SS_Acts); } else { _SS_DPS = 0f; }
            DPS_TTL += _OP_DPS + _SD_DPS + _SL_DPS + _SS_DPS;
            HPS_TTL += _OP_HPS + _SD_HPS + _SL_HPS + _SS_HPS;

            // Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active, but only to the perc of time where Targs is active
            // After iterating how many Overrides can be done and still do other abilities, then do the white dps
            WhiteAtks.Slam_Freq = _SL_GCDs;
            float oldDPS_White = WhiteAtks.MhDPS * (1f - (CalcOpts.StunningTargets ? percTimeInStun : 0f));
            float origAvailRage = availRage;
            loopCounter = 0;

            bool hsok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool clok = CalcOpts.MultipleTargets
                     && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];

            if (hsok || clok) {
                WhiteAtks.HSOverridesOverDur = 0f;
                WhiteAtks.CLOverridesOverDur = 0f;
                RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - percTimeInStun);
                availRage += RageGenWhite;

                // Assign Rage to each ability
                float hsPercOvd, clPercOvd; // what percentage of overrides are cleave and hs
                hsPercOvd = (hsok ? 1 : 0);
                if (clok) { hsPercOvd -= CalcOpts.MultipleTargetsPerc / 100f; }
                clPercOvd = (clok ? 1f - hsPercOvd : 0);

                float RageForCL      = clok ? (!hsok ? availRage : availRage * clPercOvd) : 0f;
                float RageForHS      = hsok ? availRage - RageForCL : 0f;
                float numHSOverDur   = RageForHS / HS.FullRageCost;
                float numCLOverDur   = RageForCL / CL.FullRageCost;
                HS.OverridesOverDur  = numHSOverDur;
                CL.OverridesOverDur  = numCLOverDur;
                WhiteAtks.HSOverridesOverDur = numHSOverDur / WhiteAtks.MhEffectiveSpeed;
                WhiteAtks.CLOverridesOverDur = numCLOverDur / WhiteAtks.MhEffectiveSpeed;
                float oldHSActivates = 0f,
                    newHSActivates = HS.Activates;
                float oldCLActivates = 0f,
                    newCLActivates = CL.Activates;
                while (/*loopCounter < 50
                        &&*/ Math.Abs(newHSActivates - oldHSActivates) > 0.01f
                        || Math.Abs(newCLActivates - oldCLActivates) > 0.01f)
                {
                    oldHSActivates = HS.Activates;
                    oldCLActivates = CL.Activates;
                    // Reset the rage
                    availRage = origAvailRage;
                    RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - percTimeInStun);
                    availRage += RageGenWhite;
                    // Assign Rage to each ability
                    RageForCL = clok ? (!hsok ? availRage : availRage * (CalcOpts.MultipleTargetsPerc / 100f)) : 0f;
                    RageForHS = hsok ? availRage - RageForCL : 0f;
                    //
                    HS.OverridesOverDur = WhiteAtks.HSOverridesOverDur = (RageForHS / HS.FullRageCost);
                    CL.OverridesOverDur = WhiteAtks.CLOverridesOverDur = (RageForCL / CL.FullRageCost);
                    //
                    newHSActivates = HS.Activates;
                    newCLActivates = CL.Activates;
                    // Iterate
                    //loopCounter++;
                }
                // Add HS dps
                _HS_Acts = numHSOverDur;
                _HS_DPS = HS.DPS;
                _HS_PerHit = HS.DamageOnUse;
                DPS_TTL += _HS_DPS;
                // Add CL dps
                _CL_Acts = numCLOverDur;
                _CL_DPS = CL.DPS;
                _CL_PerHit = CL.DamageOnUse;
                DPS_TTL += _CL_DPS;
                // White
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _WhiteDPSMH = WhiteAtks.MhDPS * (1f - (CalcOpts.StunningTargets ? percTimeInStun : 0f)); // MhWhiteDPS with loss of time in stun
                _WhiteDPS = _WhiteDPSMH;
                DPS_TTL += _WhiteDPS;
            }else{
                RageGenWhite = WHITEATTACKS.whiteRageGenOverDur;
                availRage += RageGenWhite;
                WhiteAtks.HSOverridesOverDur = 0f;
                WhiteAtks.CLOverridesOverDur = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS * (1f - (CalcOpts.StunningTargets ? percTimeInStun : 0f)); // MhWhiteDPS with loss of time in stun
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _HS_Acts = 0f; _HS_DPS = 0f; _HS_PerHit = 0f;
                _CL_Acts = 0f; _CL_DPS = 0f; _CL_PerHit = 0f;
                DPS_TTL += _WhiteDPS;
            }
            calcDeepWounds();

            GCDUsage += "\n" + availGCDs.ToString("000") + " : Avail GCDs";

            // Return result
            _HPS_TTL = HPS_TTL;
            return DPS_TTL;
        }
    }
}
