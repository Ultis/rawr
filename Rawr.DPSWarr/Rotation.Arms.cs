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
        public override float RageNeededOverDur {
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
        public override float FreeRageOverDur {
            get {
                float sword = SS.GetRageUseOverDur(_SS_Acts);
                return base.FreeRageOverDur + sword;
            }
        }
        #endregion
        protected override void calcDeepWounds() {
            // The goggles! They do nothing!
        }

        public override float MakeRotationandDoDPS() {
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
            float timelostwhilemoving = 0f;
            float percTimeInMovement = 0f;

            if (Char.MainHand == null) { return 0f; }

            // ==== Reasons GCDs would be lost ========
            // Having to Move
            if (CalcOpts.MovingTargets) {
                timelostwhilemoving = CalcOpts.MovingTargetsTime * (1f - StatS.MovementSpeed);
                percTimeInMovement = timelostwhilemoving / FightDuration;
                _Move_GCDs = timelostwhilemoving / LatentGCD;
                GCDsused += (float)Math.Min(NumGCDs, _Move_GCDs);
                GCDUsage += (_Move_GCDs > 0 ? _Move_GCDs.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + " : Spent Moving\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
            }
            float IronWillBonus = (float)Math.Ceiling(20f / 3f * Talents.IronWill) / 100f;
            float BaseStunDur = (float)Math.Max(0f, (CalcOpts.StunningTargetsDur / 1000f * (1f - IronWillBonus)));
            // Being Stunned or Charmed
            if (CalcOpts.StunningTargets && CalcOpts.StunningTargetsFreq > 0) {
                // Assume you are Stunned for 3 GCDs (1.5+latency)*3 = ~1.6*3 = ~4.8 seconds per stun
                // Iron Will reduces the Duration of the stun by 7%,14%,20%
                // 100% perc means you are stunned the entire fight, the boss is stunning you every third GCD, basically only refreshing his stun
                //  50% perc means you are stunned half the fight, the boss is stunning you every sixth GCD
                float stunnedActs = (float)Math.Max(0f, FightDuration / CalcOpts.StunningTargetsFreq);
                //float acts = (float)Math.Min(availGCDs, stunnedGCDs);
                float Abil_Acts = CalcOpts.AllowFlooring ? (float)Math.Floor(stunnedActs) : stunnedActs;
                _Stunned_Acts = Abil_Acts;
                float reduc = Math.Max(0f, BaseStunDur);
                GCDsused += (float)Math.Min(NumGCDs, (reduc * Abil_Acts) / LatentGCD);
                GCDUsage += (Abil_Acts > 0 ? Abil_Acts.ToString(CalcOpts.AllowFlooring ? "000" : "000.00") + "x" + reduc.ToString() + "secs-IronWillBonus : Stunned\n" : "");
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);
                // Now let's try and get some of those GCDs back
                if (Talents.HeroicFury > 0 && _Stunned_Acts > 0f) {
                    float hfacts = HF.Activates;
                    _HF_Acts = (float)Math.Min(_Stunned_Acts, hfacts);
                    reduc = Math.Max(0f, (BaseStunDur - Math.Max(0f,/*(*/CalcOpts.React/*-250)/1000f*/)));
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

            float TotalPercTimeLost = Math.Min(1f, percTimeInStun + percTimeInMovement);

            // ==== Rage Generation Priorities ========
            availRage += RageGenOverDur_Other;

            SndW.NumStunsOverDur = _Stunned_Acts;
            /*Second Wind       */
            AddAnItem(ref availRage, TotalPercTimeLost, ref _Second_Acts, ref HPS_TTL, ref _Second_HPS, SndW);
            /*Bloodrage         */
            AddAnItem(ref availRage, TotalPercTimeLost, ref _Blood_GCDs, ref HPS_TTL, ref _Blood_HPS, BR);
            /*Berserker Rage    */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _ZRage_GCDs, ref HPS_TTL, ref _ZRage_HPS, BZ, false);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _Battle_GCDs, ref HPS_TTL, ref _Battle_HPS, BTS);
            /*Commanding Shout  */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _Comm_GCDs, ref HPS_TTL, ref _Comm_HPS, CS);
            /*Demoralizing Shout*/
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _Demo_GCDs, ref HPS_TTL, ref _Demo_HPS, DS);
            /*Sunder Armor      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _Sunder_GCDs, ref HPS_TTL, ref _Sunder_HPS, SN);
            /*Thunder Clap      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _Thunder_GCDs, ref DPS_TTL, ref HPS_TTL, ref _TH_DPS, ref _TH_HPS, TH);
            /*Hamstring         */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _Ham_GCDs, ref HPS_TTL, ref _Ham_HPS, HMS);
            /*Shattering Throw  */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _Shatt_GCDs, ref DPS_TTL, ref HPS_TTL, ref _Shatt_DPS, ref _Shatt_HPS, ST);
            /*Enraged Regeneratn*/
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _ER_GCDs, ref HPS_TTL, ref _ER_HPS, ER);
            /*Sweeping Strikes  */
            AddAnItem(ref availRage, TotalPercTimeLost, ref _SW_GCDs, ref HPS_TTL, ref _SW_HPS, SW);
            /*Death Wish        */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _Death_GCDs, ref HPS_TTL, ref _Death_HPS, Death);

            // ==== Standard Priorities ===============

            // These are solid and not dependant on other attacks
            /*Bladestorm        */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _BLS_GCDs, ref DPS_TTL, ref HPS_TTL, ref _BLS_DPS, ref _BLS_HPS, BLS, 4f);
            /*Mortal Strike     */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _MS_GCDs, ref DPS_TTL, ref HPS_TTL, ref _MS_DPS, ref _MS_HPS, MS);
            /*Rend              */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _RD_GCDs, ref DPS_TTL, ref HPS_TTL, ref _RD_DPS, ref _RD_HPS, RD);
            /*Taste for Blood   */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, TotalPercTimeLost, ref _TB_GCDs, ref DPS_TTL, ref HPS_TTL, ref _TB_DPS, ref _TB_HPS, TB);

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
                float acts = (float)Math.Min(availGCDs, OP.GetActivates(GetDodgedYellowsOverDur(), GetParriedYellowsOverDur(), _SS_Acts) * (1f - TotalPercTimeLost));
                float Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _OP_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);

                //Sudden Death
                acts = (float)Math.Min(availGCDs, SD.GetActivates(GetLandedAtksOverDur()) * (1f - TotalPercTimeLost));
                Abil_GCDs = CalcOpts.AllowFlooring ? (float)Math.Floor(acts) : acts;
                _SD_GCDs = Abil_GCDs;
                GCDsused += (float)Math.Min(NumGCDs, Abil_GCDs);
                availGCDs = (float)Math.Max(0f, NumGCDs - GCDsused);

                //Slam for remainder of GCDs
                _SL_GCDs = SL.Validated ? availGCDs * (1f - SL.Whiteattacks.AvoidanceStreak) * (1f - TotalPercTimeLost) : 0f;
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
            float oldDPS_White = WhiteAtks.MhDPS * (1f - TotalPercTimeLost);
            float origAvailRage = availRage;
            loopCounter = 0;

            bool hsok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool clok = CalcOpts.MultipleTargets
                     && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];

            if (hsok || clok) {
                WhiteAtks.HSOverridesOverDur = 0f;
                WhiteAtks.CLOverridesOverDur = 0f;
                RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - TotalPercTimeLost);
                availRage += RageGenWhite;

                // Assign Rage to each ability
                float hsPercOvd, clPercOvd; // what percentage of overrides are cleave and hs
                hsPercOvd = (hsok ? 1 : 0);
                if (clok) { hsPercOvd -= CalcOpts.MultipleTargetsPerc / 100f; }
                clPercOvd = (clok ? 1f - hsPercOvd : 0);

                float RageForCL = clok ? (!hsok ? availRage : availRage * clPercOvd) : 0f;
                float RageForHS = hsok ? availRage - RageForCL : 0f;
                RageForHS = Math.Max(RageForHS, 0.0f);
                RageForCL = Math.Max(RageForCL, 0.0f);
                float numHSOverDur = RageForHS / HS.FullRageCost;
                float numCLOverDur = RageForCL / CL.FullRageCost;
                HS.OverridesOverDur = numHSOverDur;
                CL.OverridesOverDur = numCLOverDur;
                WhiteAtks.HSOverridesOverDur = numHSOverDur / WhiteAtks.MhEffectiveSpeed;
                WhiteAtks.CLOverridesOverDur = numCLOverDur / WhiteAtks.MhEffectiveSpeed;
                float oldHSActivates = 0f,
                    newHSActivates = HS.Activates;
                float oldCLActivates = 0f,
                    newCLActivates = CL.Activates;
                while (/*loopCounter < 50
                        &&*/
                             Math.Abs(newHSActivates - oldHSActivates) > 0.01f
                        || Math.Abs(newCLActivates - oldCLActivates) > 0.01f)
                {
                    oldHSActivates = HS.Activates;
                    oldCLActivates = CL.Activates;
                    // Reset the rage
                    availRage = origAvailRage;
                    RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - TotalPercTimeLost);
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
                _WhiteDPSMH = WhiteAtks.MhDPS * (1f - TotalPercTimeLost); // MhWhiteDPS with loss of time in stun and movement
                _WhiteDPS = _WhiteDPSMH;
                DPS_TTL += _WhiteDPS;
            } else {
                RageGenWhite = WHITEATTACKS.whiteRageGenOverDur;
                availRage += RageGenWhite;
                WhiteAtks.HSOverridesOverDur = 0f;
                WhiteAtks.CLOverridesOverDur = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS * (1f - TotalPercTimeLost); // MhWhiteDPS with loss of time in stun and movement
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _HS_Acts = 0f; _HS_DPS = 0f; _HS_PerHit = 0f;
                _CL_Acts = 0f; _CL_DPS = 0f; _CL_PerHit = 0f;
                DPS_TTL += _WhiteDPS;
            }
            // Deep Wounds Activates
            float mhActivates =
                /*OnAttack*/_HS_Acts * HS.MHAtkTable.Crit +
                /*OnAttack*/_CL_Acts * CL.MHAtkTable.Crit +
                /*Yellow  */GetCriticalYellowsOverDurMH() +
                /*White   */(WhiteAtks.MhActivates * (1f - TotalPercTimeLost)) * WhiteAtks.MHAtkTable.Crit;

            // Push to the Ability
            DW.SetAllAbilityActivates(mhActivates, 0f);
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;

            GCDUsage += "\n" + availGCDs.ToString("000") + " : Avail GCDs";

            // Return result
            _HPS_TTL = HPS_TTL;
            return DPS_TTL;
        }
    }
}