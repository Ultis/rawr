/**********
 * Owner: Jothay
 **********/
using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr {
    public class ArmsRotation : Rotation {
        public ArmsRotation(Character character, Stats stats) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CombatFactors = new CombatFactors(Char, StatS);
            CalcOpts = Char == null || Char.CalculationOptions == null ? new CalculationOptionsDPSWarr() : Char.CalculationOptions as CalculationOptionsDPSWarr;
            WhiteAtks = new Skills.WhiteAttacks(Char, StatS, CombatFactors);
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
        // GCD Totals
        public float _MS_DPS  = 0f, _MS_HPS  = 0f, _MS_GCDs  = 0f;
        public float _RD_DPS  = 0f, _RD_HPS  = 0f, _RD_GCDs  = 0f;
        public float _OP_DPS  = 0f, _OP_HPS  = 0f, _OP_GCDs  = 0f;
        public float _TB_DPS  = 0f, _TB_HPS  = 0f, _TB_GCDs  = 0f;
        public float _SD_DPS  = 0f, _SD_HPS  = 0f, _SD_GCDs  = 0f;
        public float _SS_DPS  = 0f, _SS_HPS  = 0f, _SS_Acts  = 0f;
        public float _BLS_DPS = 0f, _BLS_HPS = 0f, _BLS_GCDs = 0f;
        // GCD Losses
        public float _Move_GCDs = 0f;
        public float _Stunned_Acts = 0f;
        public float _Feared_Acts = 0f;
        public float _Rooted_Acts = 0f;
        public float _HF_Acts = 0f;
        public float _EM_Acts = 0f;
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
        }
        protected override void initAbilities() {
            base.initAbilities();
            WW = new Skills.WhirlWind(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            BLS = new Skills.Bladestorm(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS, WW);
            MS = new Skills.MortalStrike(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            RD = new Skills.Rend(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SS = new Skills.Swordspec(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            OP = new Skills.OverPower(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS, SS);
            TB = new Skills.TasteForBlood(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
            SD = new Skills.Suddendeath(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS);
        }
        #endregion
        #region Various Attacks Over Dur
        public override float GetCriticalYellowsOverDurMH() {
            bool useOH = CombatFactors.useOH;
            return base.GetCriticalYellowsOverDurMH()
                + (_BLS_GCDs * BLS.MHAtkTable.Crit * BLS.AvgTargets * 6) / (useOH ? 2 : 1)
                + _MS_GCDs * MS.MHAtkTable.Crit * MS.AvgTargets
                + _OP_GCDs * OP.MHAtkTable.Crit * OP.AvgTargets
                + _TB_GCDs * TB.MHAtkTable.Crit * TB.AvgTargets
                + _SD_GCDs * SD.MHAtkTable.Crit * SD.AvgTargets;
        }
        public override float GetLandedYellowsOverDurMH() {
            bool useOH = CombatFactors.useOH;
            return base.GetLandedYellowsOverDurMH()
                + (_BLS_GCDs * BLS.MHAtkTable.AnyLand * BLS.AvgTargets * 6) / (useOH ? 2 : 1)
                + _MS_GCDs * MS.MHAtkTable.AnyLand * MS.AvgTargets
                + _OP_GCDs * OP.MHAtkTable.AnyLand * OP.AvgTargets
                + _TB_GCDs * TB.MHAtkTable.AnyLand * TB.AvgTargets
                + _SD_GCDs * SD.MHAtkTable.AnyLand * SD.AvgTargets;
        }
        public override float GetParriedYellowsOverDur() {
            bool useOH = CombatFactors.useOH ;
            return base.GetParriedYellowsOverDur()
                + (useOH ? (_BLS_GCDs * BLS.MHAtkTable.Parry * BLS.AvgTargets + _BLS_GCDs * BLS.OHAtkTable.Parry * BLS.AvgTargets) / 2 : _BLS_GCDs * BLS.MHAtkTable.Parry * BLS.AvgTargets) * 6
                + _MS_GCDs * MS.MHAtkTable.Parry * MS.AvgTargets
                + _OP_GCDs * OP.MHAtkTable.Parry * OP.AvgTargets
                + _TB_GCDs * TB.MHAtkTable.Parry * TB.AvgTargets
                + _SD_GCDs * SD.MHAtkTable.Parry * SD.AvgTargets;
                
        }
        public override float GetCriticalYellowsOverDurOH() {
            return base.GetCriticalYellowsOverDurOH() + (_BLS_GCDs * BLS.OHAtkTable.Crit * BLS.AvgTargets * 6) / 2;
        }
        public override float GetDodgedYellowsOverDur() {
            bool useOH = CombatFactors.useOH;
            return base.GetDodgedYellowsOverDur()
                + (useOH ? (_BLS_GCDs * BLS.MHAtkTable.Dodge * BLS.AvgTargets + _BLS_GCDs * BLS.OHAtkTable.Dodge * BLS.AvgTargets) / 2 : _BLS_GCDs * BLS.MHAtkTable.Dodge * BLS.AvgTargets) * 6
                + _MS_GCDs * MS.MHAtkTable.Dodge * MS.AvgTargets
                + _OP_GCDs * OP.MHAtkTable.Dodge * OP.AvgTargets
                + _TB_GCDs * TB.MHAtkTable.Dodge * TB.AvgTargets
                + _SD_GCDs * SD.MHAtkTable.Dodge * SD.AvgTargets;
        }
        public override float GetLandedYellowsOverDurOH() {
            return base.GetLandedYellowsOverDurOH()
                + (_BLS_GCDs * BLS.OHAtkTable.AnyLand * BLS.AvgTargets * 6) / 2;
        }
        public override float GetLandedAtksOverDurMH() {
            float landednoss = GetLandedAtksOverDurNoSSMH();
            float ssActs = SS.GetActivates(GetLandedYellowsOverDurMH());

            ssActs *= WhiteAtks.MHAtkTable.AnyLand;

            return landednoss + (float)Math.Max(0f, ssActs);
        }
        public override float GetLandedAtksOverDurOH() {
            if (!CombatFactors.useOH) { return 0; }
            float landednoss = GetLandedAtksOverDurNoSSOH();
            float ssActs = SS.GetActivates(GetLandedYellowsOverDurOH());

            ssActs *= WhiteAtks.MHAtkTable.AnyLand;

            return landednoss + (float)Math.Max(0f, ssActs);
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
            // The goggles! They do nothing!
        }

        public void MakeRotationandDoDPS(bool setCalcs, float PercTimeUnder20) {
            // Starting Numbers
            float DPS_TTL = 0f, HPS_TTL = 0f;
            float FightDuration = CalcOpts.Duration;
            float LatentGCD = 1.5f + CalcOpts.GetLatency();
            float NumGCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(FightDuration / LatentGCD) : FightDuration / LatentGCD;
            GCDUsage += NumGCDs.ToString("000") + " : Total GCDs\n\n";
            float GCDsused = 0f;
            float availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;
            float timelostwhilestunned = 0f;
            float percTimeInStun = 0f;
            float timelostwhilemoving = 0f;
            float percTimeInMovement = 0f;
            float timelostwhilefeared = 0f;
            float percTimeInFear = 0f;
            float timelostwhilerooted = 0f;
            float percTimeInRoot = 0f;

            if (Char.MainHand == null) { return; }

            // ==== Reasons GCDs would be lost ========
            #region Having to Move
            if (CalcOpts.MovingTargets) {
                float val = CalcOpts.MovingTargetsTime * (1f - StatS.MovementSpeed);
                timelostwhilemoving = (CalcOpts.AllowFlooring ? (float)Math.Ceiling(val) : val);
                percTimeInMovement = timelostwhilemoving / FightDuration;
                _Move_GCDs = timelostwhilemoving / LatentGCD;
                GCDsused += (float)Math.Min(NumGCDs, _Move_GCDs);
                GCDUsage += (_Move_GCDs > 0 ? _Move_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : Spent Moving\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            }
            #endregion
            #region Being Stunned
            if (CalcOpts.StunningTargets && CalcOpts.StunningTargetsFreq > 0) {
                float BaseStunDur = (float)Math.Max(0f, (CalcOpts.StunningTargetsDur / 1000f * (1f - StatS.StunDurReduc)));
                float stunnedActs = (float)Math.Max(0f, FightDuration / CalcOpts.StunningTargetsFreq);
                //float acts = (float)Math.Min(availGCDs, stunnedGCDs);
                float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Ceiling(stunnedActs) : stunnedActs;
                _Stunned_Acts = Abil_Acts;
                float reduc = Math.Max(0f, BaseStunDur);
                GCDsused += (float)Math.Min(NumGCDs, (reduc * Abil_Acts) / LatentGCD);
                GCDUsage += (Abil_Acts > 0 ? Abil_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs-IronWillBonus : Stunned\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                // Now let's try and get some of those GCDs back
                if (Talents.HeroicFury > 0 && _Stunned_Acts > 0f) {
                    float hfacts = CalcOpts.AllowFlooring ? (float)Math.Floor(HF.Activates) : HF.Activates;
                    _HF_Acts = (float)Math.Min(_Stunned_Acts, hfacts);
                    reduc = Math.Max(0f, (BaseStunDur - Math.Max(0f,/*(*/CalcOpts.React/*-250)/1000f*/)));
                    GCDsused -= (float)Math.Min(NumGCDs, (reduc * hfacts) / LatentGCD);
                    GCDUsage += (_HF_Acts > 0 ? _HF_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs : " + HF.Name + " (adds back to GCDs when stunned)\n" : "");
                    availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                }
                if (CHARACTER.Race == CharacterRace.Human && (_Stunned_Acts - _HF_Acts > 0)) {
                    float emacts = CalcOpts.AllowFlooring ? (float)Math.Floor(EM.Activates) : EM.Activates;
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
                timelostwhilestunned = CalcOpts.AllowFlooring ? (float)Math.Ceiling(timelostwhilestunned) : timelostwhilestunned;
                percTimeInStun = timelostwhilestunned / FightDuration;
            }
            #endregion
            #region Being Feared
            if (CalcOpts.FearingTargets && CalcOpts.FearingTargetsFreq > 0) {
                float BaseFearDur = (float)Math.Max(0f, (CalcOpts.FearingTargetsDur / 1000f * (1f - StatS.FearDurReduc)));
                float fearedActs = (float)Math.Max(0f, FightDuration / CalcOpts.FearingTargetsFreq);
                //float acts = (float)Math.Min(availGCDs, fearedActs);
                float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Ceiling(fearedActs) : fearedActs;
                _Feared_Acts = Abil_Acts;
                float reduc = Math.Max(0f, BaseFearDur);
                GCDsused += (float)Math.Min(NumGCDs, (reduc * Abil_Acts) / LatentGCD);
                GCDUsage += (Abil_Acts > 0 ? Abil_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs : Feared\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                // Now let's try and get some of those GCDs back
                if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BerserkerRage_] && _Feared_Acts > 0f) {
                    float bzacts = CalcOpts.AllowFlooring ? (float)Math.Floor(BZ.Activates) : BZ.Activates;
                    _ZRage_GCDs = (float)Math.Min(_Feared_Acts, bzacts);
                    reduc = Math.Max(0f, (BaseFearDur - Math.Max(0f,/*(*/CalcOpts.React/*-250)/1000f*/)));
                    GCDsused -= (float)Math.Min(NumGCDs, (reduc * bzacts) / LatentGCD);
                    GCDUsage += (_ZRage_GCDs > 0 ? _ZRage_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs : " + BZ.Name + " (adds back to GCDs when feared)\n" : "");
                    availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                }
                // Now to give Feared GCDs back and later we'll use %
                // of time lost to stuns to affect each ability equally
                // othwerwise we are only seriously affecting things at
                // the bottom of priorities, which isn't fair (poor Slam)
                GCDsused -= (_Feared_Acts * BaseFearDur) / LatentGCD;
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                timelostwhilefeared = _Feared_Acts * BaseFearDur
                                      - (BaseFearDur - LatentGCD) * _ZRage_GCDs;
                timelostwhilefeared = CalcOpts.AllowFlooring ? (float)Math.Ceiling(timelostwhilefeared) : timelostwhilefeared;
                percTimeInFear = timelostwhilefeared / FightDuration;
            }
            #endregion
            #region Being Snared/Rooted
            if (CalcOpts.RootingTargets && CalcOpts.RootingTargetsFreq > 0) {
                float BaseRootDur = (float)Math.Max(0f, (CalcOpts.RootingTargetsDur / 1000f * (1f - StatS.SnareRootDurReduc)));
                float rootedActs = (float)Math.Max(0f, FightDuration / CalcOpts.RootingTargetsFreq);
                //float acts = (float)Math.Min(availGCDs, fearedActs);
                float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Ceiling(rootedActs) : rootedActs;
                _Rooted_Acts = Abil_Acts;
                float reduc = Math.Max(0f, BaseRootDur);
                GCDsused += (float)Math.Min(NumGCDs, (reduc * Abil_Acts) / LatentGCD);
                GCDUsage += (Abil_Acts > 0 ? Abil_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs : Rooted\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                // Now let's try and get some of those GCDs back
                /*if (CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BerserkerRage_] && _Feared_Acts > 0f) {
                    float bzacts = BZ.Activates;
                    _ZRage_GCDs = (float)Math.Min(_Feared_Acts, bzacts);
                    reduc = Math.Max(0f, (BaseRootDur - Math.Max(0f,/*(*//*CalcOpts.React/*-250)/1000f*//*)));
                    GCDsused -= (float)Math.Min(NumGCDs, (reduc * bzacts) / LatentGCD);
                    GCDUsage += (_ZRage_GCDs > 0 ? _ZRage_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs : " + BZ.Name + " (adds back to GCDs when Rooted)\n" : "");
                    availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                }*/
                // Now to give Rooted GCDs back and later we'll use %
                // of time lost to stuns to affect each ability equally
                // othwerwise we are only seriously affecting things at
                // the bottom of priorities, which isn't fair (poor Slam)
                GCDsused -= (_Rooted_Acts * BaseRootDur) / LatentGCD;
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                timelostwhilerooted = _Rooted_Acts * BaseRootDur;
                                       //- (BaseRootDur - LatentGCD) * _ZRage_GCDs;
                timelostwhilerooted = CalcOpts.AllowFlooring ? (float)Math.Ceiling(timelostwhilerooted) : timelostwhilerooted;
                percTimeInRoot = timelostwhilerooted / FightDuration;
            }
            #endregion

            float TotalPercTimeLost = Math.Min(1f, percTimeInStun + percTimeInMovement + percTimeInFear + percTimeInRoot);

            // ==== Rage Generation Priorities ========
            availRage += RageGenOverDur_Other;

            // Second Wind
            SndW.NumStunsOverDur = _Stunned_Acts + _Rooted_Acts;
            AddAnItem(ref availRage, TotalPercTimeLost, ref _Second_Acts, ref HPS_TTL, ref _Second_HPS, SndW);
            // Bloodrage
            AddAnItem(ref availRage, TotalPercTimeLost, ref _Blood_GCDs, ref HPS_TTL, ref _Blood_HPS, BR);

            // ==== Standard Priorities ===============
            SettleAll(TotalPercTimeLost, PercTimeUnder20, ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, ref DPS_TTL, ref HPS_TTL);

            // Deep Wounds Activates
            float mhActivates =
                /*OnAttack*/_HS_Acts * HS.MHAtkTable.Crit +
                /*OnAttack*/_CL_Acts * CL.MHAtkTable.Crit +
                /*Yellow  */GetCriticalYellowsOverDurMH() +
                /*White   */(WhiteAtks.MhActivates * (1f - TotalPercTimeLost)) * WhiteAtks.MHAtkTable.Crit;

            // Push DW Activates to the Ability
            DW.SetAllAbilityActivates(mhActivates, 0f);
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;
            DPS_TTL += _DW_DPS;

            GCDUsage += "\n" + availGCDs.ToString("000") + " : Avail GCDs";

            // Return result
            _HPS_TTL = HPS_TTL;

            if (setCalcs) {
                this.calcs.TotalDPS = DPS_TTL;
                this.calcs.WhiteDPS = this._WhiteDPS;

                this.calcs.WhiteRage = this.RageGenWhite;
                this.calcs.OtherRage = this.RageGenOther;
                this.calcs.NeedyRage = this.RageNeeded;
                this.calcs.FreeRage = this.RageGenWhite + this.RageGenOther - this.RageNeeded;
            }
        }

        public void SettleAll(float TotalPercTimeLost, float PercTimeUnder20,
            ref float NumGCDs, ref float availGCDs, ref float GCDsused,
            ref float availRage,
            ref float DPS_TTL, ref float HPS_TTL)
        {
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

            float preloopAvailGCDs = availGCDs, preloopGCDsUsed = GCDsused, preloopAvailRage = availRage;

            float FightDuration = CalcOpts.Duration;
            float origNumGCDs = NumGCDs * (1f - PercTimeUnder20),
                  origavailGCDs = preloopAvailGCDs * (1f - PercTimeUnder20),
                  origGCDsused = preloopGCDsUsed * (1f - PercTimeUnder20);
            float oldZRGCDs = 0f,
                  oldBTSGCDs = 0f,   oldCSGCDs = 0f,  oldDemoGCDs = 0f,
                  oldSNGCDs = 0f,    oldTHGCDs = 0f,  oldHMSGCDs = 0f,
                  oldSTGCDs = 0f,    oldERGCDs = 0f,  oldSWGCDs = 0f,
                  oldDeathGCDs = 0f, oldBLSGCDs = 0f, oldMSGCDs = 0f,
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
            float origAvailRage = preloopAvailRage * (1f - PercTimeUnder20);
            bool hsok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool clok = CalcOpts.MultipleTargets && CalcOpts.MultipleTargetsPerc > 0
                     && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];
            RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20);
            availRage += RageGenWhite;
            availRage -= SL.GetRageUseOverDur(_SL_GCDs);

            int Iterator = 0;
            #region >20%
            // Run the loop for >20%
            while (
                    Iterator < 50 &&
                    (
                     (float)Math.Abs(_ZRage_GCDs - oldZRGCDs) > 0.1f ||
                     (float)Math.Abs(_Battle_GCDs - oldBTSGCDs) > 0.1f ||
                     (float)Math.Abs(_Comm_GCDs - oldCSGCDs) > 0.1f ||
                     (float)Math.Abs(_Demo_GCDs - oldDemoGCDs) > 0.1f ||
                     (float)Math.Abs(_Sunder_GCDs - oldSNGCDs) > 0.1f ||
                     (float)Math.Abs(_Thunder_GCDs - oldTHGCDs) > 0.1f ||
                     (float)Math.Abs(_Ham_GCDs - oldHMSGCDs) > 0.1f ||
                     (float)Math.Abs(_Shatt_GCDs - oldSTGCDs) > 0.1f ||
                     (float)Math.Abs(_ER_GCDs - oldERGCDs) > 0.1f ||
                     (float)Math.Abs(_SW_GCDs - oldSWGCDs) > 0.1f ||
                     (float)Math.Abs(_Death_GCDs - oldDeathGCDs) > 0.1f ||
                     (float)Math.Abs(_BLS_GCDs - oldBLSGCDs) > 0.1f ||
                     (float)Math.Abs(_MS_GCDs - oldMSGCDs) > 0.1f ||
                     (float)Math.Abs(_RD_GCDs - oldRDGCDs) > 0.1f ||
                     (float)Math.Abs(_OP_GCDs - oldOPGCDs) > 0.1f ||
                     (float)Math.Abs(_TB_GCDs - oldTBGCDs) > 0.1f ||
                     (float)Math.Abs(_SD_GCDs - oldSDGCDs) > 0.1f ||
                     (float)Math.Abs(_SL_GCDs - oldSLGCDs) > 0.1f ||
                     (hsok && Math.Abs(newHSActivates - oldHSActivates) > 0.01f) ||
                     (clok && Math.Abs(newCLActivates - oldCLActivates) > 0.01f) ||
                     (PercTimeUnder20 > 0
                        && (float)Math.Abs(_EX_GCDs - oldEXGCDs) > 0.1f) ||
                     (Talents.SwordSpecialization > 0
                        && CombatFactors.MH.Type == ItemType.TwoHandSword
                        && (float)Math.Abs(_SS_Acts - oldSSActs) > 0.1f)
                    )
                  )
            {
                // Reset a couple of items so we can keep iterating
                availGCDs = origavailGCDs;
                GCDsused = origGCDsused;
                oldZRGCDs = _ZRage_GCDs; 
                oldBTSGCDs = _Battle_GCDs; oldCSGCDs = _Comm_GCDs; oldDemoGCDs = _Demo_GCDs; oldSNGCDs = _Sunder_GCDs; oldTHGCDs = _Thunder_GCDs;
                oldHMSGCDs = _Ham_GCDs; oldSTGCDs = _Shatt_GCDs; oldERGCDs = _ER_GCDs; oldSWGCDs = _SW_GCDs; oldDeathGCDs = _Death_GCDs;
                oldBLSGCDs = _BLS_GCDs; oldMSGCDs = _MS_GCDs; oldRDGCDs = _RD_GCDs; oldOPGCDs = _OP_GCDs; oldTBGCDs = _TB_GCDs;
                oldSDGCDs = _SD_GCDs; oldEXGCDs = _EX_GCDs; oldSLGCDs = _SL_GCDs; oldSSActs = _SS_Acts;
                WhiteAtks.Slam_Freq = _SL_GCDs;
                oldHSActivates = HS.Activates;
                oldCLActivates = CL.Activates;
                availRage = origAvailRage;
                RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20);
                availRage += RageGenWhite;

                // ==== Rage Generation Priorities ========
                // Berserker Rage
                float acts = (float)Math.Min(availGCDs, BZ.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _ZRage_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage += BZ.GetRageUseOverDur(Abil_GCDs);

                // Sword Spec, Doesn't eat GCDs
                acts = SS.GetActivates(GetLandedYellowsOverDur());
                _SS_Acts = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                availRage += SS.GetRageUseOverDur(_SS_Acts);

                // ==== Maintenance Priorities ========
                // Battle Shout
                acts = (float)Math.Min(availGCDs, BTS.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                _Battle_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= BTS.GetRageUseOverDur(Abil_GCDs);

                // Commanding Shout
                acts = (float)Math.Min(availGCDs, CS.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                _Comm_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= CS.GetRageUseOverDur(Abil_GCDs);

                // Demoralizing Shout
                acts = (float)Math.Min(availGCDs, DS.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                _Demo_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= DS.GetRageUseOverDur(Abil_GCDs);

                // Sunder Armor
                acts = (float)Math.Min(availGCDs, SN.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                _Sunder_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= SN.GetRageUseOverDur(Abil_GCDs);

                // Thunder Clap
                acts = (float)Math.Min(availGCDs, TH.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                _Thunder_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= TH.GetRageUseOverDur(Abil_GCDs);

                // Hamstring
                acts = (float)Math.Min(availGCDs, HMS.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                _Ham_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= HMS.GetRageUseOverDur(Abil_GCDs);

                // Shattering Throw
                acts = (float)Math.Min(availGCDs, ST.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _Shatt_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= ST.GetRageUseOverDur(Abil_GCDs);

                // Enraged Regeneration
                acts = (float)Math.Min(availGCDs, ER.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _ER_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= ER.GetRageUseOverDur(Abil_GCDs);

                // Sweeping Strikes
                acts = (float)Math.Min(availGCDs, SW.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _SW_GCDs = Abil_GCDs;
                availRage -= SW.GetRageUseOverDur(Abil_GCDs);

                // Death Wish
                acts = (float)Math.Min(availGCDs, Death.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _Death_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= Death.GetRageUseOverDur(Abil_GCDs);

                // ==== Primary Ability Priorities ====
                // Bladestorm
                acts = (float)Math.Min(availGCDs, BLS.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _BLS_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs * 4f);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= BLS.GetRageUseOverDur(Abil_GCDs);

                // Mortal Strike
                acts = (float)Math.Min(availGCDs, MS.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _MS_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= MS.GetRageUseOverDur(Abil_GCDs);

                // Rend
                acts = (float)Math.Min(availGCDs, RD.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _RD_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= RD.GetRageUseOverDur(Abil_GCDs);

                // Overpower
                acts = (float)Math.Min(availGCDs, OP.GetActivates(GetDodgedYellowsOverDur(), GetParriedYellowsOverDur(), _SS_Acts) * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _OP_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= OP.GetRageUseOverDur(_OP_GCDs);

                // Taste for Blood
                acts = (float)Math.Min(availGCDs, TB.Activates * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _TB_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= TB.GetRageUseOverDur(Abil_GCDs);

                // Sudden Death
                acts = (float)Math.Min(availGCDs, SD.GetActivates(GetLandedAtksOverDur()) * (1f - TotalPercTimeLost) * (1f - PercTimeUnder20));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _SD_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);

                // Slam for remainder of GCDs
                acts = (float)Math.Min(availGCDs, availGCDs/*SL.Activates*/ * (1f - TotalPercTimeLost));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _SL_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(origNumGCDs, _SL_GCDs);
                availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= SL.GetRageUseOverDur(_SL_GCDs);

                float possibleFreeRage = availRage / (FightDuration * (1f - PercTimeUnder20));
                SD.FreeRage = possibleFreeRage;//50f;
                availRage -= SD.GetRageUseOverDur(_SD_GCDs);

                // Assign Rage to each ability
                float RageForHSCL = availRage * (1f - PercTimeUnder20);
                RageForCL = clok ? (!hsok ? RageForHSCL : RageForHSCL * (CalcOpts.MultipleTargetsPerc / 100f)) : 0f;
                RageForHS = hsok ? RageForHSCL - RageForCL : 0f;

                float val1 = (RageForHS / HS.FullRageCost),val2 = (RageForCL / CL.FullRageCost);
                if (CalcOpts.AllowFlooring) { val1 = (float)Math.Floor(val1); val2 = (float)Math.Floor(val2); }
                HS.OverridesOverDur = WhiteAtks.HSOverridesOverDur = val1;
                CL.OverridesOverDur = WhiteAtks.CLOverridesOverDur = val2;
                availRage -= RageForHSCL;

                // Final Prep for Next iter
                newHSActivates = HS.Activates;
                newCLActivates = CL.Activates;
                Iterator++;
            }
            #endregion
            #region <20%
            if (PercTimeUnder20 > 0f) {
                Iterator = 0;
                origNumGCDs = origNumGCDs * PercTimeUnder20;
                origavailGCDs = preloopAvailGCDs * PercTimeUnder20;
                origGCDsused = preloopGCDsUsed * PercTimeUnder20;
                float newoldZRGCDs = _ZRage_GCDs, newoldBTSGCDs = _Battle_GCDs, newoldCSGCDs = _Comm_GCDs,
                newoldDemoGCDs = _Demo_GCDs, newoldSNGCDs = _Sunder_GCDs, newoldTHGCDs = _Thunder_GCDs,
                newoldHMSGCDs = _Ham_GCDs, newoldSTGCDs = _Shatt_GCDs, newoldERGCDs = _ER_GCDs,
                newoldSWGCDs = _SW_GCDs, newoldDeathGCDs = _Death_GCDs, newoldBLSGCDs = _BLS_GCDs,
                newoldMSGCDs = _MS_GCDs, newoldRDGCDs = _RD_GCDs, newoldOPGCDs = _OP_GCDs,
                newoldTBGCDs = _TB_GCDs, newoldSDGCDs = _SD_GCDs, newoldEXGCDs = origavailGCDs,
                newoldSSActs = 0f;
                origAvailRage = preloopAvailRage * PercTimeUnder20;
                RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - TotalPercTimeLost) * PercTimeUnder20;
                availRage += RageGenWhite;
                // Run the loop for <20%
                while (
                        Iterator < 50 &&
                        (
                         (float)Math.Abs(_ZRage_GCDs - newoldZRGCDs) > 0.1f ||
                         (float)Math.Abs(_Battle_GCDs - newoldBTSGCDs) > 0.1f ||
                         (float)Math.Abs(_Comm_GCDs - newoldCSGCDs) > 0.1f ||
                         (float)Math.Abs(_Demo_GCDs - newoldDemoGCDs) > 0.1f ||
                         (float)Math.Abs(_Sunder_GCDs - newoldSNGCDs) > 0.1f ||
                         (float)Math.Abs(_Thunder_GCDs - newoldTHGCDs) > 0.1f ||
                         (float)Math.Abs(_Ham_GCDs - newoldHMSGCDs) > 0.1f ||
                         (float)Math.Abs(_Shatt_GCDs - newoldSTGCDs) > 0.1f ||
                         (float)Math.Abs(_ER_GCDs - newoldERGCDs) > 0.1f ||
                         (float)Math.Abs(_SW_GCDs - newoldSWGCDs) > 0.1f ||
                         (float)Math.Abs(_Death_GCDs - newoldDeathGCDs) > 0.1f ||
                         (float)Math.Abs(_BLS_GCDs - newoldBLSGCDs) > 0.1f ||
                         (float)Math.Abs(_MS_GCDs - newoldMSGCDs) > 0.1f ||
                         (float)Math.Abs(_RD_GCDs - newoldRDGCDs) > 0.1f ||
                         (float)Math.Abs(_OP_GCDs - newoldOPGCDs) > 0.1f ||
                         (float)Math.Abs(_TB_GCDs - newoldTBGCDs) > 0.1f ||
                         (float)Math.Abs(_SD_GCDs - newoldSDGCDs) > 0.1f ||
                         (PercTimeUnder20 > 0
                            && (float)Math.Abs(_EX_GCDs - newoldEXGCDs) > 0.1f) ||
                         (Talents.SwordSpecialization > 0
                            && CombatFactors.MH.Type == ItemType.TwoHandSword
                            && (float)Math.Abs(_SS_Acts - newoldSSActs) > 0.1f)
                        )
                      )
                {
                    // Reset a couple of items so we can keep iterating
                    availGCDs = origavailGCDs;
                    GCDsused = origGCDsused;
                    newoldZRGCDs = _ZRage_GCDs;
                    newoldBTSGCDs = _Battle_GCDs; newoldCSGCDs = _Comm_GCDs; newoldDemoGCDs = _Demo_GCDs; newoldSNGCDs = _Sunder_GCDs; newoldTHGCDs = _Thunder_GCDs;
                    newoldHMSGCDs = _Ham_GCDs; newoldSTGCDs = _Shatt_GCDs; newoldERGCDs = _ER_GCDs; newoldSWGCDs = _SW_GCDs; newoldDeathGCDs = _Death_GCDs;
                    newoldBLSGCDs = _BLS_GCDs; newoldMSGCDs = _MS_GCDs; newoldRDGCDs = _RD_GCDs; newoldOPGCDs = _OP_GCDs; newoldTBGCDs = _TB_GCDs;
                    newoldSDGCDs = _SD_GCDs; newoldEXGCDs = _EX_GCDs; newoldSSActs = _SS_Acts;
                    availRage = origAvailRage;
                    RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - TotalPercTimeLost) * PercTimeUnder20;
                    availRage += RageGenWhite;

                    // ==== Rage Generation Priorities ========
                    // Berserker Rage
                    float acts = (float)Math.Min(availGCDs, BZ.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _ZRage_GCDs = oldZRGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage += BZ.GetRageUseOverDur(Abil_GCDs);

                    // Sword Spec, Doesn't eat GCDs
                    float SS_Acts = SS.GetActivates(GetLandedYellowsOverDur());
                    _SS_Acts = SS_Acts;
                    availRage += SS.GetRageUseOverDur(_SS_Acts);

                    // ==== Maintenance Priorities ========
                    // Battle Shout
                    acts = (float)Math.Min(availGCDs, BTS.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                    _Battle_GCDs = oldZRGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= BTS.GetRageUseOverDur(Abil_GCDs);

                    // Commanding Shout
                    acts = (float)Math.Min(availGCDs, CS.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                    _Comm_GCDs = oldCSGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= CS.GetRageUseOverDur(Abil_GCDs);

                    // Demoralizing Shout
                    acts = (float)Math.Min(availGCDs, DS.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                    _Demo_GCDs = oldDemoGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= DS.GetRageUseOverDur(Abil_GCDs);

                    // Sunder Armor
                    acts = (float)Math.Min(availGCDs, SN.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                    _Sunder_GCDs = oldSNGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= SN.GetRageUseOverDur(Abil_GCDs);

                    // Thunder Clap
                    acts = (float)Math.Min(availGCDs, TH.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                    _Thunder_GCDs = oldTHGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= TH.GetRageUseOverDur(Abil_GCDs);

                    // Hamstring
                    acts = (float)Math.Min(availGCDs, HMS.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                    _Ham_GCDs = oldHMSGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= HMS.GetRageUseOverDur(Abil_GCDs);

                    // Shattering Throw
                    acts = (float)Math.Min(availGCDs, ST.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _Shatt_GCDs = oldSTGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= ST.GetRageUseOverDur(Abil_GCDs);

                    // Enraged Regeneration
                    acts = (float)Math.Min(availGCDs, ER.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Ceiling(acts) : acts;
                    _ER_GCDs = oldERGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= ER.GetRageUseOverDur(Abil_GCDs);

                    // Sweeping Strikes
                    acts = (float)Math.Min(availGCDs, SW.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _SW_GCDs = oldSWGCDs + Abil_GCDs;
                    availRage -= SW.GetRageUseOverDur(Abil_GCDs);

                    // Death Wish
                    acts = (float)Math.Min(availGCDs, Death.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _Death_GCDs = oldDeathGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= Death.GetRageUseOverDur(Abil_GCDs);

                    // ==== Primary Ability Priorities ====
                    // Rend
                    acts = (float)Math.Min(availGCDs, RD.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _RD_GCDs = oldRDGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= RD.GetRageUseOverDur(Abil_GCDs);

                    // Overpower
                    acts = (float)Math.Min(availGCDs, OP.GetActivates(GetDodgedYellowsOverDur(), GetParriedYellowsOverDur(), _SS_Acts) * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _OP_GCDs = oldOPGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= OP.GetRageUseOverDur(_OP_GCDs);

                    // Taste for Blood
                    acts = (float)Math.Min(availGCDs, TB.Activates * (1f - TotalPercTimeLost) * PercTimeUnder20);
                    Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                    _TB_GCDs = oldTBGCDs + Abil_GCDs;
                    GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                    availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                    availRage -= TB.GetRageUseOverDur(Abil_GCDs);

                    // Execute Spamming <20%
                    if (PercTimeUnder20 > 0f) {
                        EX.PercTimeUnder20 = PercTimeUnder20;
                        acts = (float)Math.Min(availGCDs,
                            availGCDs/*EX.Activates*/ * (1f - TotalPercTimeLost)
                            //- (_ZRage_GCDs + _Battle_GCDs + _Comm_GCDs + _Demo_GCDs
                               //+ _Sunder_GCDs + _Thunder_GCDs + _Ham_GCDs + _Shatt_GCDs + _ER_GCDs + _Death_GCDs
                               //+ _RD_GCDs + _TB_GCDs + _OP_GCDs) * PercTimeUnder20
                            );
                        Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                        _EX_GCDs = Abil_GCDs;
                        GCDsused += (float)Math.Min(origNumGCDs, Abil_GCDs);
                        availGCDs = (float)Math.Max(0f, origNumGCDs - GCDsused);
                        float possibleFreeRage = availRage / (FightDuration * PercTimeUnder20);
                        EX.FreeRage = possibleFreeRage;
                        availRage -= EX.GetRageUseOverDur(_EX_GCDs);
                    }

                    Iterator++;
                }
            }
            #endregion
            int bah = Iterator;
            // Add each of the abilities' DPS and HPS values and other aesthetics
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

            GCDUsage += (_BLS_GCDs> 0 ? _BLS_GCDs.ToString(CalcOpts.AllowFlooring? "000" : "000.00") + "x4 : "+BLS.Name+ "\n" : "");
            GCDUsage += (_MS_GCDs > 0 ? _MS_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + MS.Name + "\n" : "");
            GCDUsage += (_RD_GCDs > 0 ? _RD_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + RD.Name + "\n" : "");
            GCDUsage += (_OP_GCDs > 0 ? _OP_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x2 : "+OP.Name + "\n" : "");
            GCDUsage += (_TB_GCDs > 0 ? _TB_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + TB.Name + "\n" : "");
            GCDUsage += (_SD_GCDs > 0 ? _SD_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SD.Name + "\n" : "");
            GCDUsage += (_SL_GCDs > 0 ? _SL_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + SL.Name + "\n" : "");
            GCDUsage += (_EX_GCDs > 0 ? _EX_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : " + EX.Name + "\n" : "");

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

            _BLS_DPS=BLS.GetDPS(_BLS_GCDs);_BLS_HPS=BLS.GetHPS(_BLS_GCDs);
            _MS_DPS = MS.GetDPS(_MS_GCDs); _MS_HPS = MS.GetHPS(_MS_GCDs);
            _RD_DPS = RD.GetDPS(_RD_GCDs); _RD_HPS = RD.GetHPS(_RD_GCDs);
            _OP_DPS = OP.GetDPS(_OP_GCDs); _OP_HPS = OP.GetHPS(_OP_GCDs);
            _TB_DPS = TB.GetDPS(_TB_GCDs); _TB_HPS = TB.GetHPS(_TB_GCDs);
            _SD_DPS = SD.GetDPS(_SD_GCDs); _SD_HPS = SD.GetHPS(_SD_GCDs);
            if (PercTimeUnder20 > 0) { _EX_DPS = EX.GetDPS(_EX_GCDs); _EX_HPS = EX.GetHPS(_EX_GCDs); }
            _SL_DPS = SL.GetDPS(_SL_GCDs); _SL_HPS = SL.GetHPS(_SL_GCDs);
            if (Talents.SwordSpecialization > 0 && CombatFactors.MH.Type == ItemType.TwoHandSword) { _SS_DPS = SS.GetDPS(_SS_Acts); } else { _SS_DPS = 0f; }

            DPS_TTL += _TH_DPS + _Ham_DPS + _Shatt_DPS + _BLS_DPS + _MS_DPS + _RD_DPS + _OP_DPS + _TB_DPS + _SD_DPS + _EX_DPS + _SL_DPS + _SS_DPS;
            HPS_TTL += _Battle_HPS + _Comm_HPS + _Demo_HPS + _Sunder_HPS + _Sunder_HPS + _TH_HPS + _Ham_HPS + _Shatt_HPS + _SW_HPS + _ER_HPS + _Death_HPS + _BLS_HPS + _MS_HPS + _RD_HPS + _OP_HPS + _TB_HPS + _SD_HPS + _EX_HPS + _SL_HPS + _SS_HPS;

            RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - TotalPercTimeLost);
            RageNeeded += 0f
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
            RageGenOther += 0f
                        + BZ.GetRageUseOverDur(_ZRage_GCDs)
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
            _WhiteDPSMH = WhiteAtks.MhDPS * (1f - TotalPercTimeLost); // MhWhiteDPS with loss of time in stun and movement
            _WhiteDPS = _WhiteDPSMH;
            DPS_TTL += _WhiteDPS;
        }

        public override void MakeRotationandDoDPS(bool setCalcs) {
            float PercTimeUnder20 = 0f;
            if(CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_]){
                PercTimeUnder20 = 0.15f;
            }
            MakeRotationandDoDPS(setCalcs, PercTimeUnder20);
        }
    }
}