/**********
 * Owner: Ebs
 **********/
using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    public class FuryRotation : Rotation
    {
        public FuryRotation(Character character, Stats stats, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CombatFactors = cf;
            CalcOpts = (co == null ? new CalculationOptionsDPSWarr() : co);
            WhiteAtks = wa;
            // Initialize();
        }

        #region FuryRotVariables
        public Skills.BloodThirst BT;
        public Skills.BloodSurge BS;

        private const float ROTATION_LENGTH = 8.0f;
        private const float FREE_GCDS = 1.0f;
        
        float _bloodsurgeRPS;
        public float _BS_DPS = 0f, _BS_HPS = 0f, _BS_GCDs = 0f;
        public float _BT_DPS = 0f, _BT_HPS = 0f, _BT_GCDs = 0f;

        private float timeLostPerc = 0f;
        #endregion

        public override void Initialize(CharacterCalculationsDPSWarr calcs)
        {
            base.Initialize(calcs);
            calcs.BT = BT;
            calcs.BS = BS;
        }

        protected override void initAbilities()
        {
            base.initAbilities();
            BT = new Skills.BloodThirst(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS, CALCOPTS);
            BS = new Skills.BloodSurge(CHARACTER, STATS, COMBATFACTORS, WHITEATTACKS, CALCOPTS, SL, WW, BT);
        }

        protected void new_doIterations()
        {
            base.doIterations();

            HS.OverridesOverDur = 0f;
            WhiteAtks.HSOverridesOverDur = 0f;
            WhiteAtks.CLOverridesOverDur = 0f;

            //float bsBaseRage = BS.RageUseOverDur;
            float hsRageUsed = (FreeRageOverDur - BS.RageUseOverDur) / (1f + HS.FullRageCost * (Talents.Bloodsurge * 0.20f / 3f));
            
        }
        protected override void calcDeepWounds()
        {
            // Main Hand
            float mhActivates =
                /*Yellow  */CriticalYellowsOverDurMH +
                /*White   */WhiteAtks.MhActivates * (1f - timeLostPerc) * WhiteAtks.MHAtkTable.Crit;

            // Off Hand
            float ohActivates = (CombatFactors.useOH ?
                // No OnAttacks for OH
                /*Yellow*/CriticalYellowsOverDurOH +
                /*White */WhiteAtks.OhActivates * (1f - timeLostPerc) * WhiteAtks.OHAtkTable.Crit
                : 0f);

            // Push to the Ability
            DW.SetAllAbilityActivates(mhActivates, ohActivates);
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;
        }
        public override void doIterations() {
            int line = 0;
            try {
                base.doIterations();
                // Fury Iteration
                bool hsok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
                bool clok = CalcOpts.MultipleTargets
                         && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];

                float hsPercOvd, clPercOvd; // what percentage of overrides are cleave and hs
                hsPercOvd = (hsok ? 1f : 0f);
                if (CalcOpts.MultipleTargets && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_])
                    hsPercOvd -= CalcOpts.MultipleTargetsPerc / 100f;
                clPercOvd = (clok ? 1f - hsPercOvd : 0f);

                _bloodsurgeRPS = BS.RageUseOverDur;
                float hsRageUsed = FreeRageOverDur * hsPercOvd;
                float clRageUsed = FreeRageOverDur * clPercOvd;

                WhiteAtks.HSOverridesOverDur = HS.OverridesOverDur = hsRageUsed / HS.FullRageCost;
                WhiteAtks.CLOverridesOverDur = CL.OverridesOverDur = clRageUsed / CL.FullRageCost;

                float /*oldHSActivates = 0f,*/ newHSActivates = HS.Activates;
                float /*oldCLActivates = 0f,*/ newCLActivates = CL.Activates;
                BS.maintainActs = MaintainCDs;
                /*int loopIterator;
                for (loopIterator = 0;
                     CalcOpts.FuryStance
                        && loopIterator < 50
                        && (Math.Abs(oldHSActivates / newHSActivates - 1f)  > 0.01f
                            || Math.Abs(oldCLActivates / newCLActivates - 1f) > 0.01f);
                      loopIterator++)
                {
                    oldHSActivates = HS.Activates;
                    oldCLActivates = CL.Activates;*/
                    //
                    BS.hsActivates = newHSActivates; // bloodsurge only cares about HSes, not Cleaves
                    _bloodsurgeRPS = (BS.RageUseOverDur);
                    hsRageUsed = FreeRageOverDur * hsPercOvd;
                    clRageUsed = FreeRageOverDur * clPercOvd;
                    WhiteAtks.HSOverridesOverDur = HS.OverridesOverDur = hsRageUsed / HS.FullRageCost;
                    WhiteAtks.CLOverridesOverDur = CL.OverridesOverDur = clRageUsed / CL.FullRageCost;
                    //
                    /*newHSActivates = HS.Activates;
                    newCLActivates = CL.Activates;
                }*/

                BS.hsActivates = newHSActivates;
                //BS.hsActivates += newCLActivates;
                if (CalcOpts.FuryStance)
                {
                    _HS_DPS = HS.GetDPS(newHSActivates);
                    _CL_DPS = CL.GetDPS(newCLActivates);
                    _HS_PerHit = HS.DamageOnUse * hsPercOvd;
                    _CL_PerHit = CL.DamageOnUse * clPercOvd;
                }
            } catch (Exception ex) {
                new ErrorBoxDPSWarr("Error in performing Fury Iterations",
                    ex.Message, "doIterations()", "No Additional Info", ex.StackTrace, line);
            }
        }

        #region LandedAtks
        protected override float LandedYellowsOverDurMH {
            get {
                return base.LandedYellowsOverDurMH +
                    _BT_GCDs * BT.MHAtkTable.AnyLand * BT.AvgTargets +
                    _BS_GCDs * BS.MHAtkTable.AnyLand * BS.AvgTargets;
            }
        }
        protected override float CriticalYellowsOverDurMH {
            get {
                return base.CriticalYellowsOverDurMH
                    + _BT_GCDs * BT.MHAtkTable.Crit * BT.AvgTargets
                    + _BS_GCDs * BS.MHAtkTable.Crit * BS.AvgTargets;
            }
        }
        protected override float AttemptedYellowsOverDurMH {
            get {
                return base.AttemptedYellowsOverDurMH
                    + _BT_GCDs * BT.AvgTargets
                    + _BS_GCDs * BS.AvgTargets;
            }
        }
        public override float ParriedYellowsOverDur {
            get {
                return base.ParriedYellowsOverDur
                    + _BT_GCDs * BT.MHAtkTable.Parry * BT.AvgTargets
                    + _BS_GCDs * BS.MHAtkTable.Parry * BS.AvgTargets;
            }
        }
        public override float DodgedYellowsOverDur {
            get {
                return base.DodgedYellowsOverDur
                    + _BT_GCDs * BT.MHAtkTable.Dodge * BT.AvgTargets
                    + _BS_GCDs * BS.MHAtkTable.Dodge * BS.AvgTargets;
            }
        }
        public override float CritHsSlamOverDur {
            get {
                return base.CritHsSlamOverDur
                    + _BS_GCDs * BS.MHAtkTable.Crit
                    + HS.Activates * HS.MHAtkTable.Crit;
            }
        }
        #endregion
        protected override float RageNeededOverDur
        {
            get
            {
                return (base.RageNeededOverDur + BT.GetRageUseOverDur(_BT_GCDs) + _bloodsurgeRPS) * 
                    (1f - timeLostPerc);
            }
        }

        

        #region NewRotation
        /*private float bloodsurge_percUsed;  // Since we can only bloodsurge once every
                                            // 8secs, this keeps track of how many times we
                                            // can actually slam vs refresh an ability
        private float rotationSlipTime = 0f;// for when maint abilities fail
            
        private float gcdCounter;                  // where we are in the fight
        
        // maintenance
        private float numSunderGCDs = 0f, numThunderGCDs = 0f, numDemoShoutGCDs = 0f,
                      numBattleShoutGCDs = 0f, numCommandingShoutGCDs = 0f;
        
        // cooldowns on the GCD
        private float numDeathwishGCDs = 0f, numBerserkerRageGCDs = 0f;
        
        // dps
        private float numBloodthirstGCDs = 0f, numWhirlwindGCDs = 0f, numBloodsurgeGCDs = 0f;

        
        private void Preprocess(Skills.Ability ability) { Preprocess(ability, 1f); }
        /// <summary>
        /// Preprocesses the rotation variables for each maintenance ability
        /// </summary>
        /// <param name="ability">An ability that may be maintained</param>
        /// <param name="stacks">The number of stacks applied at the START of the fight (ex: Battle Shout would be 0, Sunder Armor would be 5, Demo Shout would be 1)</param>
        private void Preprocess(Skills.Ability ability, float stacks)
        {
            if (ability.Validated)
            {
                float slip = 1f / DS.MHAtkTable.AnyLand; // initial application
                float reapplyPeriod = (int)(DS.Duration / ROTATION_LENGTH);
                
                bloodsurge_percUsed -= 1f / reapplyPeriod; // keep it up
                rotationSlipTime += (1f - slip);// if I on average have to do
                                                // 1.08 demos to apply it, then
                                                // 8% of my rotations have a 1.5sec slip
                numDemoShoutGCDs += stacks * slip;
                gcdCounter += stacks * slip;
            }
        }

        public void new_MakeRotationandDoDPS(bool setCalcs)
        {
            float LatentGCD = 1.5f + CalcOpts.GetLatency();
            float NumGCDs = CalcOpts.Duration / LatentGCD;
            GCDUsage += "NumGCDs: " + NumGCDs.ToString() + "\n\n";
            
            // Maintenance abilities

            // First, apply initial debuffs
            bloodsurge_percUsed = 1f; // Since we can only bloodsurge once every 8secs, 
                                            // this keeps track of how many times we can actually slam vs refresh an ability
            
            // maintenance
            numSunderGCDs = 0f; numThunderGCDs = 0f; numDemoShoutGCDs = 0f;
            numBattleShoutGCDs = 0f; numCommandingShoutGCDs = 0f;
            // cooldowns on the GCD
            numDeathwishGCDs = 0f; numBerserkerRageGCDs = 0f;
            // dps
            numBloodthirstGCDs = 0f; numWhirlwindGCDs = 0f; numBloodsurgeGCDs = 0f;

            gcdCounter = 0f; // where we are in the fight

            //////////////////////////////////////////////////////////////////////////
            // The following code block does two things in one pass:                //
            // 1) Determines how long it takes to apply initial debuffs (stored in  //
            //         gcdCounter)                                                  //
            // 2) Determines how frequently we actually get to bloodsurge based on  //
            //         free GCDs (stored in bloodsurge_percUsed)                    //
            //////////////////////////////////////////////////////////////////////////
            Preprocess(SN, 5f);
            Preprocess(TH);
            Preprocess(DS);
            // Assuming these are already applied at the start of the fight
            Preprocess(BTS, 0f);
            Preprocess(CS, 0f);
            
            if (bloodsurge_percUsed < 0f)
                bloodsurge_percUsed = Math.Max(bloodsurge_percUsed, 0f);

        }
         */
        #endregion
        public override void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalcs)
        {
            base.MakeRotationandDoDPS(setCalcs, needsDisplayCalcs);
            //new_MakeRotationandDoDPS(setCalcs);
            // Starting Numbers
            float DPS_TTL = 0f, HPS_TTL = 0f;
            float FightDuration = CalcOpts.Duration;
            float LatentGCD = 1.5f + CalcOpts.Latency;
            float NumGCDs = FightDuration / LatentGCD;
            if (_needDisplayCalcs) GCDUsage += "NumGCDs: " + NumGCDs.ToString() + "\n\n";
            float GCDsused = 0f;
            float availGCDs = Math.Max(0f, NumGCDs - GCDsused);
            float availRage = 0f;
            float rageadd = 0f;
            //float timelostwhilestunned = 0f;
            float percTimeInStun = 0f;

            if (Char.MainHand == null) { return; }
            //doIterations();

            // ==== Rage Generation Priorities ========
            availRage += RageGenOverDur_Other;
            
            /*Bloodrage         */
            AddAnItem(ref availRage, percTimeInStun, ref _Blood_GCDs, ref HPS_TTL, ref _Blood_HPS, BR);
            /*Berserker Rage    */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _ZRage_GCDs, ref HPS_TTL, ref _ZRage_HPS, BZ, false);

            // ==== Maintenance Priorities ============
            /*Battle Shout      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Battle_GCDs, ref HPS_TTL, ref _Battle_HPS, BTS);
            /*Commanding Shout  */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Comm_GCDs, ref HPS_TTL, ref _Comm_HPS, CS);
            /*Demoralizing Shout*/
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Demo_GCDs, ref HPS_TTL, ref _Demo_HPS, DS);
            /*Sunder Armor      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Sunder_GCDs, ref HPS_TTL, ref _Sunder_HPS, SN);
            /*Thunder Clap      */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Thunder_GCDs, ref DPS_TTL, ref HPS_TTL, ref _TH_DPS, ref _TH_HPS, TH);
            /*Hamstring         */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Ham_GCDs, ref HPS_TTL, ref _Ham_HPS, HMS);
            /*Shattering Throw  */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Shatt_GCDs, ref DPS_TTL, ref HPS_TTL, ref _Shatt_DPS, ref _Shatt_HPS, ST);
            /*Enraged Regeneratn*/
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _ER_GCDs, ref HPS_TTL, ref _ER_HPS, ER);
            /*Sweeping Strikes  */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _SW_GCDs, ref HPS_TTL, ref _SW_HPS, SW);
            /*Death Wish        */
            AddAnItem(ref NumGCDs, ref availGCDs, ref GCDsused, ref availRage, percTimeInStun, ref _Death_GCDs, ref HPS_TTL, ref _Death_HPS, Death);

            /*float Reck_GCDs = Math.Min(availGCDs, RK.Activates);
            _Reck_GCDs = Reck_GCDs;
            GCDsused += Math.Min(NumGCDs, Reck_GCDs);
            GCDUsage += RK.Name + ": " + Reck_GCDs.ToString() + "\n";
            availGCDs = Math.Max(0f, NumGCDs - GCDsused);
            rageadd = RK.GetRageUsePerSecond(Reck_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;*/

            //doIterations();

            #region CalcOpts GCD Losses
            float timeLost = 0f;
            #region Stuns
            if (CalcOpts.StunningTargets && CalcOpts.StunningTargetsFreq > 0f && CalcOpts.StunningTargetsDur > 0f) 
            {
                float numStuns = FightDuration / CalcOpts.StunningTargetsFreq;
                if (!CalcOpts.SE_UseDur) numStuns = (float)Math.Floor(numStuns);
                // ways to counter, no GCD, just use reaction time
                if (Talents.HeroicFury > 0f)
                {
                    float numUses = Math.Min(numStuns, HF.Activates);
                    numStuns -= numUses;
                    timeLost += numUses * (CalcOpts.React / 1000f + CalcOpts.Latency); // not using GetReact because it's not on the GCD
                }
                if (Char.Race == CharacterRace.Human)
                {
                    float numUses = Math.Min(numStuns, EM.Activates);
                    numStuns -= numUses;
                    timeLost += numUses * CalcOpts.React / 1000f + CalcOpts.Latency; // not using GetReact because it's not on the GCD
                }

                if (numStuns > 0f)
                {
                    float stunDur = CalcOpts.StunningTargetsDur / 1000f * (1f - Talents.IronWill * 2f / 30f);
                    timeLost += stunDur * numStuns;
                }
            }
            #endregion


            float timeLostPerc = timeLost / FightDuration;
            #endregion

            // Priority 1 : Whirlwind on every CD
            float WW_GCDs = Math.Min(availGCDs, WW.Activates) * (1f - timeLostPerc);
            _WW_GCDs = WW_GCDs;
            GCDsused += Math.Min(NumGCDs, WW_GCDs);
            if (_needDisplayCalcs) GCDUsage += WW.Name + ": " + WW_GCDs.ToString() + "\n";
            availGCDs = Math.Max(0f, NumGCDs - GCDsused);
            _WW_DPS = WW.GetDPS(WW_GCDs);
            _WW_HPS = WW.GetHPS(WW_GCDs);
            DPS_TTL += _WW_DPS;
            HPS_TTL += _WW_HPS;
            rageadd = WW.GetRageUseOverDur(WW_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            // Priority 2 : Bloodthirst on every CD
            float BT_GCDs = Math.Min(availGCDs, BT.Activates) * (1f - timeLostPerc);
            _BT_GCDs = BT_GCDs;
            GCDsused += Math.Min(NumGCDs, BT_GCDs);
            if (_needDisplayCalcs) GCDUsage += BT.Name + ": " + BT_GCDs.ToString() + "\n";
            availGCDs = Math.Max(0f, NumGCDs - GCDsused);
            _BT_DPS = BT.GetDPS(BT_GCDs);
            _BT_HPS = BT.GetHPS(BT_GCDs);
            DPS_TTL += _BT_DPS;
            HPS_TTL += _BT_HPS;
            rageadd = BT.GetRageUseOverDur(BT_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            doIterations();
            // Priority 3 : Bloodsurge Blood Proc (Do an Instant Slam) if available
            float BS_GCDs = Math.Min(availGCDs, BS.Activates) * (1f - timeLostPerc);
            _BS_GCDs = BS_GCDs;
            GCDsused += Math.Min(NumGCDs, BS_GCDs);
            if (_needDisplayCalcs) GCDUsage += BS.Name + ": " + BS_GCDs.ToString() + "\n";
            availGCDs = Math.Max(0f, NumGCDs - GCDsused);
            _BS_DPS = BS.GetDPS(BS_GCDs);
            _BS_HPS = BS.GetHPS(BS_GCDs);
            DPS_TTL += _BS_DPS;
            HPS_TTL += _BS_HPS;
            rageadd = BS.GetRageUseOverDur(BS_GCDs);
            availRage -= rageadd;
            RageNeeded += rageadd;

            //Sword Spec, Doesn't eat GCDs
            /*float SS_Acts = SS.GetActivates(GetLandedYellowsOverDur());
            _SS_Acts = SS_Acts;
            _SS_DPS = SS.GetDPS(SS_Acts);
            DPS_TTL += _SS_DPS;*/
            // TODO: Add Rage since it's a white hit

            //doIterations();
            // Priority 4 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            // After iterating how many Overrides can be done and still do other abilities, then do the white dps

            availRage += WhiteAtks.MHRageGenOverDur + WhiteAtks.OHRageGenOverDur;

            bool HSok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool CLok = CalcOpts.MultipleTargets && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];

            WhiteAtks.Slam_Freq = 0f;// _SL_GCDs;
            if ((HSok || CLok) && availRage > 0f) {
                //float numHSOverDur = availRage / HS.FullRageCost;
                //HS.OverridesOverDur = numHSOverDur;
                //WhiteAtks.HSOverridesOverDur = numHSOverDur;
                //WhiteAtks.CLOverridesOverDur = 0f;
                RageGenWhite = WhiteAtks.whiteRageGenOverDur * (1f - timeLostPerc);
                _WhiteDPSMH = WhiteAtks.MhDPS * (1f - timeLostPerc); // MhWhiteDPS
                _WhiteDPSOH = WhiteAtks.OhDPS * (1f - timeLostPerc);
                _WhiteDPS = _WhiteDPSMH + _WhiteDPSOH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                /*_HS_DPS = HS.DPS;
                _HS_PerHit = HS.DamageOnUse;
                _CL_DPS = CL.DPS;
                _CL_PerHit = CL.DamageOnUse;*/
                DPS_TTL += _WhiteDPS;
                DPS_TTL += _HS_DPS;
                DPS_TTL += _CL_DPS;
            } else {
                RageGenWhite = WHITEATTACKS.whiteRageGenOverDur * (1f - timeLostPerc);
                //availRage += RageGenWhite;
                WhiteAtks.HSOverridesOverDur = 0f;
                WhiteAtks.CLOverridesOverDur = 0f;
                _WhiteDPSMH = WhiteAtks.MhDPS * (1f - timeLostPerc); // MhWhiteDPS
                _WhiteDPSOH = WhiteAtks.OhDPS * (1f - timeLostPerc); // OhWhiteDPS
                _WhiteDPS = _WhiteDPSMH;
                _WhitePerHit = WhiteAtks.MhDamageOnUse; // MhAvgSwingDmg
                _HS_DPS = _HS_PerHit = _HS_Acts = 0;
                _CL_DPS = _CL_PerHit = _CL_Acts = 0;
                DPS_TTL += _WhiteDPS;
            }
            calcDeepWounds();
            _DW_PerHit = DW.TickSize;
            _DW_DPS = DW.DPS;
            DPS_TTL += _DW_DPS;

            if (_needDisplayCalcs) GCDUsage += "\nAvail: " + availGCDs.ToString();

            // Return result
            _HPS_TTL = HPS_TTL;

            if (setCalcs)
            {
                this.calcs.TotalDPS = DPS_TTL;
                this.calcs.WhiteDPS = this._WhiteDPS;

                this.calcs.WhiteRage  = this.RageGenWhite;
                this.calcs.OtherRage  = this.RageGenOther;
                this.calcs.NeedyRage  = this.RageNeeded;
                this.calcs.FreeRage   = this.RageGenWhite + this.RageGenOther - this.RageNeeded;
                this.calcs.WhiteDPSMH = this.WhiteAtks.MhDPS;
                this.calcs.WhiteDPSOH = this.WhiteAtks.OhDPS;
                this.calcs.WhiteDmg   = this.WhiteAtks.MhDamageOnUse + this.WhiteAtks.OhDamageOnUse;
            }
        }

        protected float FreeRageOverDur
        {
            get
            {
                /*if (Char.MainHand == null) { return 0f; }
                float white = WHITEATTACKS.whiteRageGenOverDurNoHS * (1f - timeLostPerc);
                //float sword = SS.GetRageUseOverDur(_SS_Acts);
                float other = RageGenOverDur_Other;
                float needy = RageNeededOverDur;
                return white + other - needy;*/
                return WHITEATTACKS.whiteRageGenOverDurNoHS * (1f - timeLostPerc) +
                       RageGenOverDur_Other -
                       RageNeededOverDur;
            }
        }
    }
}