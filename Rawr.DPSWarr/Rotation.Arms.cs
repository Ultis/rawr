/**********
 * Owner: Jothay
 **********/
using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class ArmsRotation : Rotation {
        public ArmsRotation(Character character, Stats stats, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CombatFactors = cf;
            CalcOpts = (co == null ? new CalculationOptionsDPSWarr() : co);
            WhiteAtks = wa;

            FightDuration = CalcOpts.Duration;

            // Initialize();
        }
        #region Variables
        // Ability Declarations
        public Skills.Bladestorm BLS;
        public Skills.MortalStrike MS;
        public Skills.Rend RD;
        public Skills.OverPower OP;
        public Skills.TasteForBlood TB;
        public Skills.Suddendeath SD;
        public Skills.Swordspec SS;
        public Skills.FakeWhite FW;
        // GCD Totals
        public float _MS_DPS  = 0f, _MS_HPS  = 0f, _MS_GCDs  = 0f;
        public float _RD_DPS  = 0f, _RD_HPS  = 0f, _RD_GCDs  = 0f;
        public float _OP_DPS  = 0f, _OP_HPS  = 0f, _OP_GCDs  = 0f;
        public float _TB_DPS  = 0f, _TB_HPS  = 0f, _TB_GCDs  = 0f;
        public float _SD_DPS  = 0f, _SD_HPS  = 0f, _SD_GCDs  = 0f;
        public float _SS_DPS  = 0f, _SS_HPS  = 0f, _SS_Acts  = 0f;
        public float _BLS_DPS = 0f, _BLS_HPS = 0f, _BLS_GCDs = 0f;
        public float _FW_DPS  = 0f, _FW_HPS  = 0f, _FW_GCDs  = 0f;
        // GCD Losses
        public float _Disarm_Acts  = 0f, _Disarm_Per  = 0f, _Disarm_Eaten  = 0f;
        public float                _BZ_RecovPer = 0f, _BZ_RecovTTL = 0f; // Berserker Rage (Anti-Fear)
        public float _IN_Acts = 0f, _IN_RecovPer = 0f, _IN_RecovTTL = 0f; // Intercept (Warbringer)
        public float _IV_Acts = 0f, _IV_RecovPer = 0f, _IV_RecovTTL = 0f; // Intervene (Warbringer)
        #endregion
        #region Initialization
        public override void Initialize(CharacterCalculationsDPSWarr calcs) {
            base.Initialize(calcs);
            calcs.WW = WW;
            calcs.BLS = BLS;
            calcs.MS = MS;
            calcs.RD = RD;
            calcs.OP = OP;
            calcs.TB = TB;
            calcs.SD = SD;
            calcs.SS = SS;
            calcs.FW = FW;
        }
        protected override void initAbilities() {
            base.initAbilities();
            WW = new Skills.WhirlWind(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            BLS = new Skills.Bladestorm(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, WW);
            MS = new Skills.MortalStrike(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            RD = new Skills.Rend(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            SS = new Skills.Swordspec(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            OP = new Skills.OverPower(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, SS);
            TB = new Skills.TasteForBlood(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
            SD = new Skills.Suddendeath(Char, StatS, CombatFactors, WhiteAtks, CalcOpts, EX);
            FW = new Skills.FakeWhite(Char, StatS, CombatFactors, WhiteAtks, CalcOpts);
        }
        #endregion
        #region Various Attacks Over Dur
        protected override float CriticalYellowsOverDurMH {
            get {
                bool useOH = CombatFactors.useOH;
                return base.CriticalYellowsOverDurMH
                    + (_BLS_GCDs * BLS.MHAtkTable.Crit * BLS.AvgTargets * 7) / (useOH ? 2 : 1)
                    + _MS_GCDs * MS.MHAtkTable.Crit * MS.AvgTargets
                    + _OP_GCDs * OP.MHAtkTable.Crit * OP.AvgTargets
                    + _TB_GCDs * TB.MHAtkTable.Crit * TB.AvgTargets
                    + _SD_GCDs * SD.MHAtkTable.Crit * SD.AvgTargets;
            }
        }
        protected override float LandedYellowsOverDurMH {
            get {
                bool useOH = CombatFactors.useOH;
                return base.LandedYellowsOverDurMH
                    + (_BLS_GCDs * BLS.MHAtkTable.AnyLand * BLS.AvgTargets * 7) / (useOH ? 2 : 1)
                    + _MS_GCDs * MS.MHAtkTable.AnyLand * MS.AvgTargets
                    + _OP_GCDs * OP.MHAtkTable.AnyLand * OP.AvgTargets
                    + _TB_GCDs * TB.MHAtkTable.AnyLand * TB.AvgTargets
                    + _SD_GCDs * SD.MHAtkTable.AnyLand * SD.AvgTargets;
            }
        }
        public override float ParriedYellowsOverDur {
            get {
                bool useOH = CombatFactors.useOH;
                return base.ParriedYellowsOverDur
                    + (useOH ? (_BLS_GCDs * BLS.MHAtkTable.Parry * BLS.AvgTargets + _BLS_GCDs * BLS.OHAtkTable.Parry * BLS.AvgTargets) / 2 : _BLS_GCDs * BLS.MHAtkTable.Parry * BLS.AvgTargets) * 7
                    + _MS_GCDs * MS.MHAtkTable.Parry * MS.AvgTargets
                    + _OP_GCDs * OP.MHAtkTable.Parry * OP.AvgTargets
                    + _TB_GCDs * TB.MHAtkTable.Parry * TB.AvgTargets
                    + _SD_GCDs * SD.MHAtkTable.Parry * SD.AvgTargets;
            }
        }
        protected override float CriticalYellowsOverDurOH {
            get {
                return base.CriticalYellowsOverDurOH + (_BLS_GCDs * BLS.OHAtkTable.Crit * BLS.AvgTargets * 7) / 2;
            }
        }
        public override float DodgedYellowsOverDur {
            get {
                bool useOH = CombatFactors.useOH;
                return base.DodgedYellowsOverDur
                    + (useOH ? (_BLS_GCDs * BLS.MHAtkTable.Dodge * BLS.AvgTargets + _BLS_GCDs * BLS.OHAtkTable.Dodge * BLS.AvgTargets) / 2 : _BLS_GCDs * BLS.MHAtkTable.Dodge * BLS.AvgTargets) * 7
                    + _MS_GCDs * MS.MHAtkTable.Dodge * MS.AvgTargets
                    + _OP_GCDs * OP.MHAtkTable.Dodge * OP.AvgTargets
                    + _TB_GCDs * TB.MHAtkTable.Dodge * TB.AvgTargets
                    + _SD_GCDs * SD.MHAtkTable.Dodge * SD.AvgTargets;
            }
        }
        protected override float LandedYellowsOverDurOH {
            get {
                if (!CombatFactors.useOH) return 0f;
                return base.LandedYellowsOverDurOH
                    + (_BLS_GCDs * BLS.OHAtkTable.AnyLand * BLS.AvgTargets * 7f);
            }
        }
        protected override float AttemptedYellowsOverDurMH {
            get {
                bool useOH = CombatFactors.useOH;
                return base.LandedYellowsOverDurMH
                    + (_BLS_GCDs * BLS.AvgTargets * 7f)
                    + _MS_GCDs * MS.AvgTargets
                    + _OP_GCDs * OP.AvgTargets
                    + _TB_GCDs * TB.AvgTargets
                    + _SD_GCDs * SD.AvgTargets
                    + _SS_Acts * SS.AvgTargets;
            }
        }
        protected override float AttemptedYellowsOverDurOH {
            get {
                if (!CombatFactors.useOH) return 0f;
                return base.AttemptedYellowsOverDurOH
                    + (_BLS_GCDs * BLS.AvgTargets * 7f);
            }
        }
        public override float LandedAtksOverDurMH {
            get {
                float landednoss = LandedAtksOverDurNoSSMH;
                float ssActs = SS.GetActivates(LandedYellowsOverDurMH, WhiteAtks.HSOverridesOverDur, WhiteAtks.CLOverridesOverDur);

                ssActs *= WhiteAtks.MHAtkTable.AnyLand;

                return landednoss + Math.Max(0f, ssActs);
            }
        }
        public override float LandedAtksOverDurOH {
            get {
                if (!CombatFactors.useOH) { return 0; }
                float landednoss = LandedAtksOverDurNoSSOH;
                float ssActs = SS.GetActivates(LandedYellowsOverDurOH, WhiteAtks.HSOverridesOverDur, WhiteAtks.CLOverridesOverDur);

                ssActs *= WhiteAtks.MHAtkTable.AnyLand;

                return landednoss + Math.Max(0f, ssActs);
            }
        }

        public float LandedAtksOverDurNoSS { get { return LandedAtksOverDurNoSSMH + LandedAtksOverDurNoSSOH; } }
        public float LandedAtksOverDurNoSSMH {
            get {
                float white = WhiteAtks.LandedAtksOverDurMH;
                float yellow = LandedYellowsOverDurMH;

                float result = white + yellow;

                return result;
            }
        }
        public float LandedAtksOverDurNoSSOH {
            get {
                if (!CombatFactors.useOH) { return 0; }
                float white = WhiteAtks.LandedAtksOverDurOH;
                float yellow = LandedYellowsOverDurOH;

                float result = white + yellow;

                return result;
            }
        }
        
        #endregion
        #region Rage Calcs
        protected override float RageNeededOverDur {
            get {
                float SweepingRage = SW.GetRageUseOverDur(_SW_GCDs);
                float BladestormRage = BLS.GetRageUseOverDur(_BLS_GCDs);
                float MSRage = MS.GetRageUseOverDur(_MS_GCDs);
                float RendRage = RD.GetRageUseOverDur(_RD_GCDs);
                float OPRage = OP.GetRageUseOverDur(_OP_GCDs);
                float TBRage = TB.GetRageUseOverDur(_TB_GCDs);
                float SDRage = SD.GetRageUseOverDur(_SD_GCDs);
                return base.RageNeededOverDur + SweepingRage + BladestormRage + MSRage + 
                    RendRage + OPRage + TBRage + SDRage;
            }
        }
        #endregion
        protected override void calcDeepWounds() {
            base.calcDeepWounds();
        }

        #region Markov
        private void MakeMarkovRotation(bool setCalcs, float percTimeUnder20) {

        }
        #endregion

        public void MakeRotationandDoDPS(bool setCalcs, float percTimeUnder20) {
            if (Char.MainHand == null) { return; }
            _HPS_TTL = 0f;
            if (_needDisplayCalcs) GCDUsage += NumGCDs.ToString("000") + " : Total GCDs\n\n";
            
            float TotalPercTimeLost = CalculateTimeLost(MS);
            
            if (_needDisplayCalcs) { GCDUsage += (TotalPercTimeLost != 0f ? "\n" : ""); }

            // ==== Rage Generation Priorities ========
            float availRage = 0f;
            availRage += RageGenOverDur_Other + RageGainedWhileMoving;

            // Second Wind
            if (SndW.Validated)
            {
                availRage += SndW.RageUseOverDur;
                _HPS_TTL += SndW.GetHPS(SndW.Activates);
                GCDUsage += (SndW.Activates > 0 ? SndW.Activates.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SndW.Name + " (Doesn't use GCDs)\n" : "");
            }
            // Bloodrage
            if (BR.Validated)
            {
                GCDUsage += (BR.Activates > 0 ? BR.Activates.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + BR.Name + " (Doesn't use GCDs)\n" : "");
                _HPS_TTL += BR.GetHPS(BR.Activates);
            }
            // ==== Standard Priorities ===============
            float DPS_TTL = SettleAll(TotalPercTimeLost, percTimeUnder20, availRage);

            calcDeepWounds();
            DPS_TTL += _DW_DPS;

            //if (_needDisplayCalcs) { GCDUsage += "\n" + availGCDs.ToString("000") + " : Avail GCDs"; }

            // Return result
            if (setCalcs) {
                this.calcs.TotalDPS = DPS_TTL;
                this.calcs.WhiteDPS = this._WhiteDPS;
                this.calcs.WhiteDPSMH = this._WhiteDPSMH;
                this.calcs.WhiteDmg = this.WhiteAtks.MhDamageOnUse;

                this.calcs.WhiteRage = this.RageGenWhite;
                this.calcs.OtherRage = this.RageGenOther;
                this.calcs.NeedyRage = this.RageNeeded;
                this.calcs.FreeRage = this.RageGenWhite + this.RageGenOther - this.RageNeeded;
            }
        }

        public float SettleAll(float totalPercTimeLost, float percTimeUnder20,float availRage)
        {
            GCDsUsed = TimeLostGDCs;
            availRage += DoMaintenanceActivates(totalPercTimeLost);
            //availGCDs = NumGCDs - GCDsused;
            /* The following are dependant on other attacks as they are proccing abilities or are the fallback item
             * We need to loop these until the activates are relatively unchanged
             * Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
             * Alternate to Cleave is MultiTargs is active, but only to the perc of time where Targs is active
             * After iterating how many Overrides can be done and still do other abilities, then do the white dps
             *
             * Starting Assumptions:
             * No ability ever procs so Slam sucks up all the cooldowns (except under <20% with that active, where Exec sucks all of them)
             * Heroic Strike and Cleave won't be used at all
             * Sudden Death Free Rage is minimum cost, no extra rage available
             * Execute Free Rage is minimum cost, no extra rage available
             * 
             * Hoped Ending Results:
             * All abilities will have proc'd and abilities that can proc from other ones will have their activates settled
             * Heroic Strikes and Cleave will activate when there's enough rage to support them AND Executes
             * Sudden Death will get extra rage leftovers if there are any
             * Execute will get extra rage leftovers if there are any (since you won't use HS/CL <20%)
            */

            float preloopAvailGCDs = GCDsAvailable, preloopGCDsUsed = GCDsUsed, preloopAvailRage = availRage;

            float origNumGCDs = (CalcOpts.AllowFlooring ? (float)Math.Floor(FightDuration / LatentGCD) : FightDuration / LatentGCD) * (1f - percTimeUnder20),
                  origavailGCDs = preloopAvailGCDs * (1f - percTimeUnder20),
                  origGCDsused = preloopGCDsUsed * (1f - percTimeUnder20);
            float oldBLSGCDs = 0f, oldMSGCDs = 0f,
                  oldRDGCDs = 0f,    oldOPGCDs = 0f,  oldTBGCDs = 0f,
                  oldSDGCDs = 0f,    oldEXGCDs = 0f,  oldSLGCDs = 0f,
                  oldSSActs = 0f;
            _ZRage_GCDs = 0f;
            _Battle_GCDs = 0f; _Comm_GCDs = 0f; _Demo_GCDs = 0f; _Sunder_GCDs = 0f; _Thunder_GCDs = 0f;
            _Ham_GCDs = 0f; _Shatt_GCDs = 0f; _ER_GCDs = 0f; _SW_GCDs = 0f; _Death_GCDs = 0f;
            _BLS_GCDs = 0f; _MS_GCDs = 0f; _RD_GCDs = 0f; _OP_GCDs = 0f; _TB_GCDs = 0f; _SD_GCDs = 0f;
            _EX_GCDs = 0f; _SL_GCDs = origavailGCDs ; _SS_Acts = 0f;
            WhiteAtks.Slam_Freq = _SL_GCDs;
            SD.FreeRage = SD.RageCost;
            EX.FreeRage = EX.RageCost;
            float oldHSActivates = 0f, RageForHS = 0f, numHSOverDur = 0f, newHSActivates = HS.OverridesOverDur = WhiteAtks.HSOverridesOverDur = 0f;
            float oldCLActivates = 0f, RageForCL = 0f, numCLOverDur = 0f, newCLActivates = CL.OverridesOverDur = WhiteAtks.CLOverridesOverDur = 0f;
            float origAvailRage = preloopAvailRage * (1f - percTimeUnder20);
            bool hsok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool clok = CalcOpts.MultipleTargets && CalcOpts.MultipleTargetsPerc > 0
                     && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];
            RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - totalPercTimeLost) * (1f - percTimeUnder20);
            availRage += RageGenWhite;
            availRage -= SL.GetRageUseOverDur(_SL_GCDs);
            float repassAvailRage = 0f;
            float PercFailRage = 1f, PercFailRageUnder20 = 1f;

            int Iterator = 0;
            #region >20%
            // Run the loop for >20%
            float MSBaseCd = 6f - Talents.ImprovedMortalStrike / 3f;
            float MS_WeightedValue = MS.DamageOnUse + DW.TickSize * MS.MHAtkTable.Crit,
                  SD_WeightedValue = SD.DamageOnUse + DW.TickSize * SD.MHAtkTable.Crit,
                  SL_WeightedValue = SL.DamageOnUse + DW.TickSize * SL.MHAtkTable.Crit;
            float OnePt5Plus1 = LatentGCD + (OP.Cd + CalcOpts.AllowedReact),
                  Two1pt5 = LatentGCD * 2f,
                  Two1pt0 = (OP.Cd + CalcOpts.AllowedReact) * 2f;
            float TasteForBloodMOD = (Talents.TasteForBlood == 3 ? 1f / 6f : (Talents.TasteForBlood == 2 ? 0.144209288653733f : (Talents.TasteForBlood == 1 ? 0.104925207394343f : 0)));
            float OtherMOD = (MSBaseCd + CalcOpts.Latency);
            float SDMOD = 1f - 0.03f * Talents.SuddenDeath;
            float avoid = (1f - CombatFactors._c_mhdodge - CombatFactors._c_ymiss);
            float atleast1=0f, atleast2=0f, atleast3=0f, extLength1, extLength2, extLength3, averageTimeBetween,
                  OnePt5Plus1_Occurs, Two1pt5_Occurs, Two1PtZero_Occurs;
            float LeavingUntilNextMS_1, MSatExtra1, msNormally1, lengthFor1;
            float LeavingUntilNextMS_2, MSatExtra2, msNormally2, lengthFor2;
            float LeavingUntilNextMS_3, MSatExtra3, msNormally3, lengthFor3;
            float timeInBetween = MSBaseCd - 1.5f;
            float useExeifMSHasMoreThan, useSlamifMSHasMoreThan;
            string canUse1, canUse2, canUse3;
            float HPS;
            #region Abilities
            while (
                    Iterator < 50 &&
                    (
                     Math.Abs(_BLS_GCDs - oldBLSGCDs) > 0.1f ||
                     Math.Abs(_MS_GCDs - oldMSGCDs) > 0.1f ||
                     Math.Abs(_RD_GCDs - oldRDGCDs) > 0.1f ||
                     Math.Abs(_OP_GCDs - oldOPGCDs) > 0.1f ||
                     Math.Abs(_TB_GCDs - oldTBGCDs) > 0.1f ||
                     Math.Abs(_SD_GCDs - oldSDGCDs) > 0.1f ||
                     Math.Abs(_SL_GCDs - oldSLGCDs) > 0.1f ||
                     (percTimeUnder20 > 0
                        && Math.Abs(_EX_GCDs - oldEXGCDs) > 0.1f) ||
                     (Talents.SwordSpecialization > 0
                        && CombatFactors._c_mhItemType == ItemType.TwoHandSword
                        && Math.Abs(_SS_Acts - oldSSActs) > 0.1f)
                    )
                  )
            {
                // Reset a couple of items so we can keep iterating
                //availGCDs = origavailGCDs;
                GCDsUsed = origGCDsused;
                oldBLSGCDs = _BLS_GCDs; oldMSGCDs = _MS_GCDs; oldRDGCDs = _RD_GCDs; oldOPGCDs = _OP_GCDs; oldTBGCDs = _TB_GCDs;
                oldSDGCDs = _SD_GCDs; oldEXGCDs = _EX_GCDs; oldSLGCDs = _SL_GCDs; oldSSActs = _SS_Acts;
                WhiteAtks.Slam_Freq = _SL_GCDs;
                availRage = origAvailRage;
                RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - totalPercTimeLost) * (1f - percTimeUnder20);
                availRage += RageGenWhite;

                float acts;
                float Abil_GCDs;
                // ==== Primary Ability Priorities ====
                // Rend
                if (RD.Validated) {
                    acts = Math.Min(GCDsAvailable, RD.Activates * (1f - totalPercTimeLost) * (1f - percTimeUnder20));
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _RD_GCDs = Abil_GCDs;
                    GCDsUsed += Math.Min(origNumGCDs, Abil_GCDs);
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= RD.GetRageUseOverDur(Abil_GCDs);
                }

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRage < 0f || PercFailRage != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRage *= 1f + repassAvailRage / (availRage - repassAvailRage); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                } else { PercFailRage = 1f; }
                
                // Bladestorm
                if (BLS.Validated) {
                    acts = Math.Min(GCDsAvailable, BLS.Activates * (1f - totalPercTimeLost) * (1f - percTimeUnder20) * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _BLS_GCDs = Abil_GCDs;
                    GCDsUsed += Math.Min(origNumGCDs, Abil_GCDs * 4f);
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= BLS.GetRageUseOverDur(Abil_GCDs);
                }
                // Mortal Strike
                if (MS.Validated) {
                    // TODO: THIS WAS ADDED TO FIX A BUG, JOTHAY'S UNAVAILABLE FOR COMMENT
                    acts = Math.Min(GCDsAvailable, MS.Activates * (1f - totalPercTimeLost) * (1f - percTimeUnder20) * PercFailRage);
                    #region Mortal Strike Delays
                    /* ===== Delays in MS from Slam or Execute =====
                     * This is a test for MS Delays (idea coming from Landsoul's sheet 2.502)
                     * Note: The numbers displayed are from a specific example, formula
                     * results in Rawr may differ
                     */
                    if (PercFailRage == 1f)
                    {
                        HPS = LandedAtksOverDur;

                        // In-Between MS is: 3.50 seconds
                        // moved up out of the loop
                        //use exe if MS more than 0.320164 sec
                        useExeifMSHasMoreThan = LatentGCD * MS_WeightedValue / (MSBaseCd * (SD_WeightedValue / LatentGCD + 0.03f * Talents.SuddenDeath * HPS * (SD_WeightedValue - SL_WeightedValue)) + MS_WeightedValue);
                        //use slam if MS more than 0.413178 sec
                        useSlamifMSHasMoreThan = LatentGCD * MS_WeightedValue / (MSBaseCd * (SL_WeightedValue / LatentGCD + 0.03f * Talents.SuddenDeath * HPS * (SD_WeightedValue - SL_WeightedValue)) + MS_WeightedValue);

                        //1.5 and 1.0 global is 2.60 seconds
                        // moved out of the loop
                        //leaving until next MS 0.90 seconds
                        LeavingUntilNextMS_1 = timeInBetween - OnePt5Plus1;
                        //Occurs 84.20% of the time
                        //can use exe or slam for 3rd gcd before next ms
                        canUse1 = (useSlamifMSHasMoreThan < LeavingUntilNextMS_1 ? "exe or slam" : (useExeifMSHasMoreThan < LeavingUntilNextMS_1 ? "exe" : "nothing"));
                        //puts MS at extra 0.65 length for 5.70
                        MSatExtra1 = LatentGCD - LeavingUntilNextMS_1;
                        lengthFor1 = (MSBaseCd + CalcOpts.AllowedReact + MSatExtra1);
                        //MS is normally at a length of 5.696
                        msNormally1 = (canUse1 == "exe or slam" ? lengthFor1 : MSBaseCd + CalcOpts.AllowedReact);
                        //Extended length is 100.00% of the time

                        //Two 1.5 globals are 3.10 seconds
                        // move up out of the loop
                        //leaving until next MS 0.40 seconds
                        LeavingUntilNextMS_2 = timeInBetween - Two1pt5;
                        //Occurs 15.52% of the time
                        //can use exe for 3rd gcd before next ms
                        canUse2 = (useSlamifMSHasMoreThan < LeavingUntilNextMS_2 ? "exe or slam" : (useExeifMSHasMoreThan < LeavingUntilNextMS_2 ? "exe" : "nothing"));
                        //puts MS at extra 1.15	length for 6.20
                        MSatExtra2 = LatentGCD - LeavingUntilNextMS_2;
                        lengthFor2 = (MSBaseCd + CalcOpts.AllowedReact + MSatExtra2);
                        //MS is normally at a length of 5.049
                        msNormally2 = (canUse2 == "exe or slam" ? lengthFor2 : MSBaseCd + CalcOpts.AllowedReact);
                        //Extended length is 19.25% of the time

                        //Two 1.0 globals are 2.10 seconds
                        // moved up out of the loop
                        //leaving until next MS 1.40 seconds
                        LeavingUntilNextMS_3 = timeInBetween - Two1pt0;
                        //Occurs 0.28% of the time
                        //can use exe or slam for last gcd before next ms
                        canUse3 = (useSlamifMSHasMoreThan < LeavingUntilNextMS_3 ? "exe or slam" : (useExeifMSHasMoreThan < LeavingUntilNextMS_3 ? "exe" : "nothing"));
                        //puts MS at extra 0.15	length for 5.20
                        MSatExtra3 = LatentGCD - LeavingUntilNextMS_3;
                        lengthFor3 = (MSBaseCd + CalcOpts.AllowedReact + MSatExtra3);
                        //MS is normally at a length of 5.196
                        msNormally3 = (canUse3 == "exe or slam" ? lengthFor3 : MSBaseCd + CalcOpts.AllowedReact);
                        //Extended length is 100.00% of the time

                        float Abilities = ((_BLS_GCDs + _RD_GCDs + _SL_GCDs + acts + oldHSActivates + WhiteAtks.MhActivates + _SD_GCDs) / FightDuration);

                        OnePt5Plus1_Occurs = 0f;
                        Two1PtZero_Occurs = TasteForBloodMOD
                                                * OtherMOD
                                                * (
                                                    (1f - 3f * TasteForBloodMOD)
                                                    * Abilities
                                                    * WhiteAtks.MHAtkTable.Dodge
                                                   )
                                                * OtherMOD;
                        OnePt5Plus1_Occurs = (1f - 3f * TasteForBloodMOD)
                                               * Abilities
                                               * WhiteAtks.MHAtkTable.Dodge
                                               * OtherMOD
                                             + TasteForBloodMOD
                                               * OtherMOD
                                             - Two1PtZero_Occurs;
                        Two1pt5_Occurs = 1f - OnePt5Plus1_Occurs - Two1PtZero_Occurs;

                        // Exec procs in MS
                        atleast1 = (1f - (float)Math.Pow(SDMOD, OnePt5Plus1_Occurs * ((canUse1 == "nothing" ? 1f : 2f) * avoid + 1f * (1f - CombatFactors._c_ymiss)) + Two1pt5_Occurs * (canUse2 == "niether" ? 2f : 3f) * avoid + Two1PtZero_Occurs * ((canUse2 == "nothing" ? 0f : 1f) * avoid + 2f * (1f - CombatFactors._c_ymiss)) + (MSBaseCd - (1.5f + CalcOpts.AllowedReact)) / CombatFactors._c_mhItemSpeed));
                        atleast2 = (1f - (float)Math.Pow(SDMOD, OnePt5Plus1_Occurs * ((canUse1 == "nothing" ? 1f : 2f) * avoid + 1f * (1f - CombatFactors._c_ymiss)) + Two1pt5_Occurs * (canUse2 == "nothing" ? 2f : 3f) * avoid + (MSBaseCd - (1.5f + CalcOpts.AllowedReact)) / CombatFactors._c_mhItemSpeed))
                                 * (1f - (float)Math.Pow(SDMOD, OnePt5Plus1_Occurs * ((canUse1 == "nothing" ? 0f : 1f) * avoid + 1.5f / CombatFactors._c_mhItemSpeed) + Two1pt5_Occurs * ((canUse2 == "nothing" ? 0f : 1f) * avoid + 1.5f / CombatFactors._c_mhItemSpeed)));
                        atleast3 = (1f - (float)Math.Pow(SDMOD, Two1pt5_Occurs * (canUse2 == "nothing" ? 2f : 3f) * avoid + (MSBaseCd - (1.5f + CalcOpts.AllowedReact)) / CombatFactors._c_mhItemSpeed))
                                 * (1f - (float)Math.Pow(SDMOD, Two1pt5_Occurs * ((canUse2 == "nothing" ? 0f : 1f) * avoid + 1.5f / CombatFactors._c_mhItemSpeed)))
                                 * (1f - (float)Math.Pow(SDMOD, Two1pt5_Occurs * ((canUse2 == "nothing" ? 0f : 1f) * avoid)));

                        extLength1 = (canUse1 == "exe" ? 0.5f * (atleast1 + atleast2 + atleast3) : (canUse1 == "exe or slam" ? 1f : 0f));
                        extLength2 = (canUse2 == "exe" ? 0.5f * (atleast1 + atleast2 + atleast3) : (canUse2 == "exe or slam" ? 1f : 0f));
                        extLength3 = (canUse3 == "exe" ? 0.5f * (atleast1 + atleast2 + atleast3) : (canUse3 == "exe or slam" ? 1f : 0f));

                        //for avg of 5.628472631 between MS'es, from crowding
                        averageTimeBetween = OnePt5Plus1_Occurs * (lengthFor1 * extLength1 + msNormally1 * (1f - extLength1))
                                           + Two1pt5_Occurs * (lengthFor2 * extLength2 + msNormally2 * (1f - extLength2))
                                           + Two1PtZero_Occurs * (lengthFor3 * extLength3 + msNormally3 * (1f - extLength3));
                        float RendSpace = _RD_GCDs / origNumGCDs;
                        float BLS_Space = (1 - RendSpace) * (_BLS_GCDs * 4) / origNumGCDs;
                        MS.Cd = LatentGCD / (LatentGCD * (1 - BLS_Space) / averageTimeBetween);
                        
                    }
                    #endregion
                    acts = Math.Min(GCDsAvailable, MS.Activates * (1f - totalPercTimeLost) * (1f - percTimeUnder20) * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _MS_GCDs = Abil_GCDs;
                    GCDsUsed += Math.Min(origNumGCDs, Abil_GCDs);
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= MS.GetRageUseOverDur(Abil_GCDs);
                }
                // Taste for Blood
                if (TB.Validated)
                {
                    acts = Math.Min(GCDsAvailable, TB.Activates * (1f - totalPercTimeLost) * (1f - percTimeUnder20) * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _TB_GCDs = Abil_GCDs;
                    float OPGCDReduc = (OP.Cd < LatentGCD ? (OP.Cd + CalcOpts.Latency) / LatentGCD : 1f);
                    GCDsUsed += Math.Min(origNumGCDs, Abil_GCDs * OPGCDReduc);
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= TB.GetRageUseOverDur(Abil_GCDs);
                }
                // Overpower
                if (OP.Validated) {
                    acts = Math.Min(GCDsAvailable, (1 - 3*_TB_GCDs/origNumGCDs) *  OP.GetActivates(DodgedYellowsOverDur, ParriedYellowsOverDur, _SS_Acts) * (1f - totalPercTimeLost) * (1f - percTimeUnder20) * PercFailRage);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _OP_GCDs = Abil_GCDs;
                    float OPGCDReduc = (OP.Cd < LatentGCD ? (OP.Cd + CalcOpts.Latency) / LatentGCD : 1f);
                    GCDsUsed += Math.Min(origNumGCDs, Abil_GCDs * OPGCDReduc);
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= OP.GetRageUseOverDur(_OP_GCDs);
                }
               
                // Sudden Death
                // the atleast (1 to 3) comes from MS Delays, this does already factor talent rate in
                if (SD.Validated) {
                    #region Sudden Death Delays
                    if (false) {
                        //float execSpace = LatentGCD * (atleast1 + atleast2 + atleast3) / MS.Cd;
                        //float attemptspersec = execSpace / LatentGCD * (1f - 0f/*AB81 rage slip*/);
                        //acts = attemptspersec * FightDuration;
                        //acts *= (1f - TotalPercTimeLost) * (1f - PercTimeUnder20);
                    } else {
                        acts = Math.Min(GCDsAvailable, SD.GetActivates(AttemptedAtksOverDur) * (1f - totalPercTimeLost) * (1f - percTimeUnder20) * PercFailRage);
                    }
                    #endregion
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _SD_GCDs = Abil_GCDs;
                    SD.FreeRage = 0f; // we will do Extra Rage later
                    /*float rageuse = SD.GetRageUseOverDur(_SD_GCDs);
                    float extraRage = availRage - rageuse;
                    if        (availRage > 0f && extraRage >  0f) {
                        // There's Rage available and more than enough to support it
                    } else if (availRage > 0f && extraRage <= 0f) {
                        // There's Rage available but not enough to support it
                        // so we need to drop off some SD GCDs
                        float perc2Drop = availRage / rageuse;
                        _SD_GCDs *= perc2Drop;
                    } else if (availRage <= 0f) {
                        // There's no Rage available to support this ability
                        _SD_GCDs = 0f;
                    }*/
                    GCDsUsed += Math.Min(origNumGCDs, _SD_GCDs);
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= SD.GetRageUseOverDur(_SD_GCDs);
                }
                // Slam for remainder of GCDs
                if (SL.Validated && PercFailRage == 1f)
                {
                    acts = Math.Min(GCDsAvailable, GCDsAvailable/*SL.Activates*/ * (1f - totalPercTimeLost));
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    if (SL.GetRageUseOverDur(Abil_GCDs) > availRage) Abil_GCDs = Math.Max(0f, availRage) / SL.RageCost;
                    _SL_GCDs = Abil_GCDs;
                    GCDsUsed += Math.Min(origNumGCDs, _SL_GCDs);
                    //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= SL.GetRageUseOverDur(_SL_GCDs);
                } else { _SL_GCDs = 0f; }

                repassAvailRage = availRage; // check for not enough rage to maintain rotation
                Iterator++;
            }
            #endregion
            #region OnAttacks
            if (availRage > 0f && PercFailRage == 1f && (hsok || clok))
            { // We need extra rage beyond the rotation to HS/CL and we don't HS/CL when parts of our rotation were failing for lack of rage
                float savedAvailRage = availRage - RageGenWhite;
                Iterator = 0;
                do {
                    RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - totalPercTimeLost) * (1f - percTimeUnder20);
                    availRage = savedAvailRage + RageGenWhite;
                    oldHSActivates = HS.Activates;
                    oldCLActivates = CL.Activates;

                    availRage += SD.GetRageUseOverDur(_SD_GCDs); // add back the non-extra rage using
                    float possibleFreeRage = availRage / (FightDuration * (1f - percTimeUnder20));
                    SD.FreeRage = possibleFreeRage;//50f;
                    availRage -= SD.GetRageUseOverDur(_SD_GCDs);

                    // Assign Rage to each ability
                    float RageForHSCL = availRage * (1f - percTimeUnder20);
                    RageForCL = clok ? (!hsok ? RageForHSCL : RageForHSCL * (CalcOpts.MultipleTargetsPerc / 100f)) : 0f;
                    RageForHS = hsok ? RageForHSCL - RageForCL : 0f;

                    float val1 = (RageForHS / HS.FullRageCost), val2 = (RageForCL / CL.FullRageCost);
                    if (CalcOpts.AllowFlooring) { val1 = (float)Math.Floor(val1); val2 = (float)Math.Floor(val2); }
                    HS.OverridesOverDur = WhiteAtks.HSOverridesOverDur = val1;
                    CL.OverridesOverDur = WhiteAtks.CLOverridesOverDur = val2;
                    availRage -= RageForHSCL;

                    // Final Prep for Next iter
                    newHSActivates = HS.Activates;
                    newCLActivates = CL.Activates;
                    Iterator++;
                } while (Iterator < 50 && (
                        (hsok && Math.Abs(newHSActivates - oldHSActivates) > 0.01f) ||
                        (clok && Math.Abs(newCLActivates - oldCLActivates) > 0.01f)));
            }
            #endregion
            #endregion
          
            int bah = Iterator;
            // Add each of the abilities' DPS and HPS values and other aesthetics
            if (_needDisplayCalcs) {
                if (PercFailRage != 1.0f || PercFailRageUnder20 != 1.0f) {
                    GCDUsage += (PercFailRage < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due to Rage Stavation before Exec Spam.\n", (1f - PercFailRage)) : "");
                    GCDUsage += (PercFailRageUnder20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due to Rage Stavation during Exec Spam.\n", (1f - PercFailRageUnder20)) : "");
                    GCDUsage += "\n";
                }
                GCDUsage += (_ZRage_GCDs > 0 ? _ZRage_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + BZ.Name + "\n" : "");
                GCDUsage += (_Battle_GCDs > 0 ? _Battle_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + BTS.Name + "\n" : "");
                GCDUsage += (_Comm_GCDs > 0 ? _Comm_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + CS.Name + "\n" : "");
                GCDUsage += (_Demo_GCDs > 0 ? _Demo_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + DS.Name + "\n" : "");
                GCDUsage += (_Sunder_GCDs > 0 ? _Sunder_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SN.Name + "\n" : "");
                GCDUsage += (_Thunder_GCDs > 0 ? _Thunder_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + TH.Name + "\n" : "");
                GCDUsage += (_Ham_GCDs > 0 ? _Ham_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + HMS.Name + "\n" : "");
                GCDUsage += (_Shatt_GCDs > 0 ? _Shatt_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + ST.Name + "\n" : "");
                GCDUsage += (_SW_GCDs > 0 ? _SW_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SW.Name + " (Doesn't Use GCDs)\n" : "");
                GCDUsage += (_ER_GCDs > 0 ? _ER_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + ER.Name + "\n" : "");
                GCDUsage += (_Death_GCDs > 0 ? _Death_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + Death.Name + "\n" : "");
                GCDUsage += (_ZRage_GCDs + _Battle_GCDs + _Comm_GCDs + _Demo_GCDs + _Sunder_GCDs + _Thunder_GCDs
                             + _Ham_GCDs + _Shatt_GCDs + _SW_GCDs + _ER_GCDs + _Death_GCDs + _ZRage_GCDs > 0f ? "\n" : "");
                GCDUsage += (_BLS_GCDs > 0 ? _BLS_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x4 : " + BLS.Name + "\n" : "");
                GCDUsage += (_MS_GCDs > 0 ? _MS_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + MS.Name + "\n" : "");
                GCDUsage += (_RD_GCDs > 0 ? _RD_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + RD.Name + "\n" : "");
                GCDUsage += (_OP_GCDs > 0 ? _OP_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x2 : " + OP.Name + "\n" : "");
                GCDUsage += (_TB_GCDs > 0 ? _TB_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + TB.Name + "\n" : "");
                GCDUsage += (_SD_GCDs > 0 ? _SD_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SD.Name + "\n" : "");
                GCDUsage += (_SL_GCDs > 0 ? _SL_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SL.Name + "\n" : "");
                GCDUsage += (_EX_GCDs > 0 ? _EX_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + EX.Name + "\n" : "");
                GCDUsage += "\n" + GCDsAvailable.ToString("000") + " : Avail GCDs";
            }
            _ZRage_HPS = BZ.GetHPS(_ZRage_GCDs);
            _Battle_HPS = BTS.GetHPS(_Battle_GCDs);
            _Comm_HPS = CS.GetHPS(_Comm_GCDs);
            _Demo_HPS = DS.GetHPS(_Demo_GCDs);
            _Sunder_HPS = SN.GetHPS(_Sunder_GCDs);
            _TH_HPS = TH.GetHPS(_Thunder_GCDs); _TH_DPS = TH.GetDPS(_Thunder_GCDs);
            _Ham_HPS = HMS.GetHPS(_Ham_GCDs); _Ham_DPS = HMS.GetDPS(_Ham_GCDs);
            _Shatt_HPS = ST.GetHPS(_Shatt_GCDs); _Shatt_DPS = ST.GetDPS(_Shatt_GCDs);
            _SW_HPS = SW.GetHPS(_SW_GCDs);
            _ER_HPS = ER.GetHPS(_ER_GCDs);
            _Death_HPS = Death.GetHPS(_Death_GCDs);

            _BLS_DPS = BLS.GetDPS(_BLS_GCDs/*, (1f - PercTimeUnder20)*/); _BLS_HPS = BLS.GetHPS(_BLS_GCDs);
            _MS_DPS = MS.GetDPS(_MS_GCDs/*, (1f-PercTimeUnder20)*/); _MS_HPS = MS.GetHPS(_MS_GCDs);
            _RD_DPS = RD.GetDPS(_RD_GCDs); _RD_HPS = RD.GetHPS(_RD_GCDs);
            _OP_DPS = OP.GetDPS(_OP_GCDs); _OP_HPS = OP.GetHPS(_OP_GCDs);
            _TB_DPS = TB.GetDPS(_TB_GCDs); _TB_HPS = TB.GetHPS(_TB_GCDs);
            _SD_DPS = SD.GetDPS(_SD_GCDs/*, (1f - PercTimeUnder20)*/); _SD_HPS = SD.GetHPS(_SD_GCDs);
            if (percTimeUnder20 > 0) { _EX_DPS = EX.GetDPS(_EX_GCDs/*, PercTimeUnder20*/); _EX_HPS = EX.GetHPS(_EX_GCDs); }
            _SL_DPS = SL.GetDPS(_SL_GCDs); _SL_HPS = SL.GetHPS(_SL_GCDs);
            if (Talents.SwordSpecialization > 0 && CombatFactors._c_mhItemType == ItemType.TwoHandSword) { _SS_DPS = SS.GetDPS(_SS_Acts); } else { _SS_DPS = 0f; }

            float DPS_TTL = _TH_DPS + _Ham_DPS + _Shatt_DPS + _BLS_DPS + _MS_DPS + _RD_DPS + _OP_DPS + _TB_DPS + _SD_DPS + _EX_DPS + _SL_DPS + _SS_DPS;
            _HPS_TTL += _ZRage_HPS + _Battle_HPS + _Comm_HPS + _Demo_HPS + _Sunder_HPS + _Sunder_HPS + _TH_HPS + _Ham_HPS + _Shatt_HPS + _SW_HPS + _ER_HPS + _Death_HPS + _BLS_HPS + _MS_HPS + _RD_HPS + _OP_HPS + _TB_HPS + _SD_HPS + _EX_HPS + _SL_HPS + _SS_HPS;

            RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - totalPercTimeLost);
            RageNeeded = 0f
                        + BTS.GetRageUseOverDur(_Battle_GCDs)
                        + CS.GetRageUseOverDur(_Comm_GCDs)
                        + DS.GetRageUseOverDur(_Demo_GCDs)
                        + SN.GetRageUseOverDur(_Sunder_GCDs)
                        + TH.GetRageUseOverDur(_Thunder_GCDs)
                        + HMS.GetRageUseOverDur(_Ham_GCDs)
                        + ST.GetRageUseOverDur(_Shatt_GCDs)
                        + SW.GetRageUseOverDur(_SW_GCDs)
                        + ER.GetRageUseOverDur(_ER_GCDs)
                        + Death.GetRageUseOverDur(_Death_GCDs)

                        + BLS.GetRageUseOverDur(_BLS_GCDs)
                        + MS.GetRageUseOverDur(_MS_GCDs)
                        + RD.GetRageUseOverDur(_RD_GCDs)
                        + OP.GetRageUseOverDur(_OP_GCDs)
                        + TB.GetRageUseOverDur(_TB_GCDs)
                        + SD.GetRageUseOverDur(_SD_GCDs)
                        + EX.GetRageUseOverDur(_EX_GCDs)
                        + SL.GetRageUseOverDur(_SL_GCDs);
            RageGenOther = RageGenOverDur_Other
                        + BZ.GetRageUseOverDur(_ZRage_GCDs)
                        //+ BR.GetRageUseOverDur(_Blood_GCDs)
                        + SS.GetRageUseOverDur(_SS_Acts);
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
            _WhiteDPSMH = WhiteAtks.MhDPS * (1f - totalPercTimeLost); // MhWhiteDPS with loss of time in stun and movement
            _WhiteDPS = _WhiteDPSMH;
            DPS_TTL += _WhiteDPS;
            
            return DPS_TTL;
        }

        /// <summary>
        /// Adds every maintenance ability to the rotation
        /// </summary>
        /// <param name="totalPercTimeLost">Time lost due to Boss Handler options</param>
        /// <returns>Change in rage from these abilities</returns>
        private float DoMaintenanceActivates(float totalPercTimeLost)
        {
            float netRage = 0f;
            // Berserker Rage, adds rage
            netRage += AddMaintenanceAbility(totalPercTimeLost, BZ, true);
            // Sword Spec, Doesn't eat GCDs
            netRage += AddMaintenanceAbility(totalPercTimeLost, SS, true, false);
            // Battle Shout
            netRage -= AddMaintenanceAbility(totalPercTimeLost, BTS);
            // Commanding Shout
            netRage -= AddMaintenanceAbility(totalPercTimeLost, CS);
            // Demoralizing Shout
            netRage -= AddMaintenanceAbility(totalPercTimeLost, DS);
            // Sunder Armor
            netRage -= AddMaintenanceAbility(totalPercTimeLost, SN);
            // Thunder Clap
            netRage -= AddMaintenanceAbility(totalPercTimeLost, TH);
            // Hamstring
            netRage -= AddMaintenanceAbility(totalPercTimeLost, HMS);
            // Shattering Throw
            netRage -= AddMaintenanceAbility(totalPercTimeLost, ST);
            // Enraged Regeneration
            netRage -= AddMaintenanceAbility(totalPercTimeLost, ER);
            // Sweeping Strikes
            netRage -= AddMaintenanceAbility(totalPercTimeLost, SW);
            // Death Wish
            netRage -= AddMaintenanceAbility(totalPercTimeLost, Death);
            if (netRage != 0f && _needDisplayCalcs) GCDUsage += Environment.NewLine;
            return netRage;
        }

        /// <summary>
        /// Adds a maintenance ability to the rotation if it's been validated.  Assumes it 
        /// costs rage and eats GCDs
        /// </summary>
        /// <param name="totalPercTimeLost">Time lost due to stun/fear/movement</param>
        /// <param name="abil">The ability to add</param>
        /// <returns>The final result from Abil.GetRageUseOverDur</returns>
        private float AddMaintenanceAbility(float totalPercTimeLost, Skills.Ability abil)
        {
            return AddMaintenanceAbility(totalPercTimeLost, abil, false, true);
        }

        /// <summary>
        /// Adds a maintenance ability to the rotation if it's been validated.  Assumes it eats GCDs
        /// </summary>
        /// <param name="totalPercTimeLost">Time lost due to stun/fear/movement</param>
        /// <param name="abil">The ability to add</param>
        /// <param name="generatesRage">True if it generates rage, false if it costs rage</param>
        /// <returns>The final result from Abil.GetRageUseOverDur</returns>
        private float AddMaintenanceAbility(float totalPercTimeLost, Skills.Ability abil, bool generatesRage)
        {
            return AddMaintenanceAbility(totalPercTimeLost, abil, generatesRage, true);
        }

        /// <summary>
        /// Adds a maintenance ability to the rotation if it's been validated
        /// </summary>
        /// <param name="totalPercTimeLost">Time lost due to stun/fear/movement</param>
        /// <param name="abil">The ability to add</param>
        /// <param name="generatesRage">True if it generates rage, false if it costs rage</param>
        /// <param name="usesGCDs">Whether the ability uses GCDs to use</param>
        /// <returns>The final result from Abil.GetRageUseOverDur</returns>
        private float AddMaintenanceAbility(float totalPercTimeLost, Skills.Ability abil, bool generatesRage, bool usesGCDs)
        {
            if (!abil.Validated) return 0f;

            float Abil_GCDs = Math.Min(GCDsAvailable, abil.Activates * (1f - totalPercTimeLost));
            Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(Abil_GCDs) : Abil_GCDs;
            
            if (usesGCDs) GCDsUsed += Abil_GCDs;
            //availGCDs -= Abil_GCDs;
            if (_needDisplayCalcs && Abil_GCDs > 0)
                GCDUsage += Abil_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + abil.Name + "\n";
            
            RageNeeded += abil.GetRageUseOverDur(Abil_GCDs) * (generatesRage?-1f:1f);
            _HPS_TTL += abil.GetHPS(Abil_GCDs);
            return abil.GetRageUseOverDur(Abil_GCDs);
        }

        

        public override void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations) {
            base.MakeRotationandDoDPS(setCalcs, needsDisplayCalculations);
            float PercTimeUnder20 = 0f;
            /*if(CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_]){
                PercTimeUnder20 = CalcOpts.Under20Perc;
            }*/
            MakeRotationandDoDPS(setCalcs, PercTimeUnder20);
        }
    }
}