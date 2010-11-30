/**********
 * Owner: Ebs
 **********/
using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSWarr.Skills;

namespace Rawr.DPSWarr {
    public class FuryRotation : Rotation {
        public FuryRotation(Character character, Stats stats, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            CombatFactors = cf;
            CalcOpts = (co == null ? new CalculationOptionsDPSWarr() : co);
            BossOpts = (bo == null ? new BossOptions() : bo);
            WhiteAtks = wa;

            _cachedLatentGCD = 1.5f + CalcOpts.Latency + CalcOpts.AllowedReact;
            AbilWrapper.LatentGCD = _cachedLatentGCD;
            _cachedNumGCDsO20 = FightDurationO20 / LatentGCD;
            _cachedNumGCDsU20 = FightDurationU20 / LatentGCD;
        }

        #region FuryRotVariables
        private const float ROTATION_LENGTH = 8.0f;
        private const float FREE_GCDS = 1.0f;
        private float percHS, percCL;
        private float timeLostPerc = 0f;
        #endregion

        public override void Initialize()
        {
            initAbilities();
            // doIterations();
            bool hsok = CalcOpts.M_HeroicStrike;
            bool clok = 
                BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0
                && CalcOpts.M_Cleave;

            percHS = (hsok ? 1f : 0f);
            if (clok)
            {
                //percHS -= (float)BossOpts.MultiTargsPerc;
                {
                    float time = 0;
                    foreach(TargetGroup tg in BossOpts.Targets) {
                        if (tg.Chance <= 0 || tg.Frequency <= 0 || tg.Duration <= 0) continue;
                        time += tg.Frequency / BossOpts.BerserkTimer * tg.Duration / 1000f;
                    }
                    float perc = time / BossOpts.BerserkTimer;
                    percHS -= Math.Max(0f, Math.Min(1f, perc)); 
                }
            }
            percCL = (clok ? 1f - percHS : 0f);
        }

        public override void Initialize(CharacterCalculationsDPSWarr calcs)
        {
            base.Initialize(calcs);
            
            bool hsok = CalcOpts.M_HeroicStrike;
            bool clok =
                BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0
                && CalcOpts.M_Cleave;
            BloodSurge _BS = GetWrapper<BloodSurge>().ability as BloodSurge;
            percHS = (hsok ? 1f : 0f);
            if (clok)
            {
                //percHS -= (float)BossOpts.MultiTargsPerc;
                {
                    float time = 0;
                    foreach (TargetGroup tg in BossOpts.Targets)
                    {
                        if (tg.Chance <= 0 || tg.Frequency <= 0 || tg.Duration <= 0) continue;
                        time += tg.Frequency / BossOpts.BerserkTimer * tg.Duration / 1000f;
                    }
                    float perc = time / BossOpts.BerserkTimer;
                    percHS -= Math.Max(0f, Math.Min(1f, perc));
                }
            }
            percCL = (clok ? 1f - percHS : 0f);
            //if (_BS != null) _BS.maintainActs = MaintainCDs;
        }

        protected void new_doIterations()
        {
            base.doIterations();
        /*    AbilWrapper HS = GetWrapper<HeroicStrike>();
            OnAttack _HS = HS.ability as OnAttack;
            HS.numActivates = 0f;
            WhiteAtks.HSOverridesOverDur = 0f;
            WhiteAtks.CLOverridesOverDur = 0f;

            //float bsBaseRage = BS.RageUseOverDur;
            float hsRageUsed = (FreeRageOverDur - BS.RageUseOverDur) / (1f + _HS.FullRageCost * (Talents.Bloodsurge * 0.20f / 3f));
          */  
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
        }
        public override void doIterations() {
            try
            {
                base.doIterations();
                // Fury Iteration
                BloodSurge BS = GetWrapper<BloodSurge>().ability as BloodSurge;
                float ovdRage;
                float oldBS;
                do {
                    oldBS = BS.Activates;
                    ovdRage = FreeRageOverDur;
                } while (Math.Abs(1f - (BS.Activates       != 0 ? oldBS / BS.Activates       : 1f)) > 0.005f);
            } catch (Exception ex) {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error in performing Fury Iterations",
                    ex.Message, ex.InnerException,
                    "doIterations()", "No Additional Info", ex.StackTrace);
                eb.Show();
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
                return WhiteAtks.whiteRageGenOverDur * (1f - timeLostPerc) +
                       RageGenOverDur_Other -
                       RageNeededOverDur;
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

        /// <summary>
        /// Copied this function from Arms, going to try and rewrite it so it works for Fury
        /// </summary>
        public float SettleAll(float totalPercTimeLost, float rageUsedByMaintenance, float percTimeU20, float availRageO20, out float PercFailRageO20)
        {
            float percTimeO20 = (1f - percTimeU20);
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndO20 = percTimeInDPS * percTimeO20;
            availRageO20 -= rageUsedByMaintenance * percTimeO20;
            /* The following are dependant on other attacks as they are proccing abilities or are the fallback item
             * We need to loop these until the activates are relatively unchanged
             * Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
             * Alternate to Cleave is MultiTargs is active, but only to the perc of time where Targs is active
             * After iterating how many Overrides can be done and still do other abilities, then do the white dps
             *
             * Starting Assumptions:
             * No ability ever procs so Slam sucks up all the cooldowns (except under <20% with that active, where Exec sucks all of them)
             * Heroic Strike and Cleave won't be used at all
             * 
             * Hoped Ending Results:
             * Since we shouldn't actually do normal Slam as Fury, Slams should end up zero (Bloodsurge DPS will be named as such instead of Slam)
             * All abilities will have proc'd and abilities that can proc from other ones will have their activates settled
             * Heroic Strikes and Cleave will activate when there's enough rage to support them AND Executes
             * Execute will get extra rage leftovers if there are any (since you won't use HS/CL <20%)
            */

            float preloopAvailGCDsO20 = GCDsAvailableO20, preloopGCDsUsedO20 = GCDsUsedO20, preloopAvailRageO20 = availRageO20;

            float origNumGCDsO20 = (FightDuration / LatentGCD) * percTimeO20,
                  origavailGCDsO20 = preloopAvailGCDsO20,
                  origGCDsusedO20 = preloopGCDsUsedO20;
            float oldGCDs_CS = 0f, oldGCDs_WW = 0f, oldGCDs_BT = 0f, oldGCDs_BS = 0f, oldGCDs_RB = 0f, oldGCDs_HS = 0f, oldGCDs_CL = 0f, oldGCDs_VR = 0f, oldGCDs_SL = 0f, oldActs_IR = 0f;

            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper WW = GetWrapper<WhirlWind>();
            AbilWrapper BT = GetWrapper<BloodThirst>();
            AbilWrapper BS = GetWrapper<BloodSurge>();
            AbilWrapper RB = GetWrapper<RagingBlow>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();
            AbilWrapper VR = GetWrapper<VictoryRush>();
            AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper IR = GetWrapper<InnerRage>();

            SL.numActivatesO20 = origavailGCDsO20;
            WhiteAtks.Slam_ActsOverDurO20 = SL.numActivatesO20;
            float origAvailRageO20 = preloopAvailRageO20;
            bool hsok = CalcOpts.M_HeroicStrike;
            bool clok = BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0
                     && CalcOpts.M_Cleave;
            availRageO20 += WhiteAtks.whiteRageGenOverDur * percTimeInDPS * percTimeO20;
            availRageO20 -= SL.RageO20;
            float repassAvailRageO20 = 0f;
            PercFailRageO20 = 1f;

            int Iterator = 0;
            #region >20%
            while (Iterator < 50 && (
                     Math.Abs(CS.numActivatesO20 - oldGCDs_CS) > 0.1f ||
                     Math.Abs(WW.numActivatesO20 - oldGCDs_WW) > 0.1f ||
                     Math.Abs(BT.numActivatesO20 - oldGCDs_BT) > 0.1f ||
                     Math.Abs(BS.numActivatesO20 - oldGCDs_BS) > 0.1f ||
                     Math.Abs(RB.numActivatesO20 - oldGCDs_RB) > 0.1f ||
                     Math.Abs(HS.numActivatesO20 - oldGCDs_HS) > 0.1f ||
                     Math.Abs(CL.numActivatesO20 - oldGCDs_CL) > 0.1f ||
                     Math.Abs(VR.numActivatesO20 - oldGCDs_VR) > 0.1f ||
                     Math.Abs(SL.numActivatesO20 - oldGCDs_SL) > 0.1f ||
                     Math.Abs(IR.numActivatesO20 - oldActs_IR) > 0.1f))
            {
                // Reset items so we can keep iterating
                WhiteAtks.Slam_ActsOverDurO20 = SL.numActivatesO20;
                oldGCDs_CS = CS.numActivatesO20; oldGCDs_WW = WW.numActivatesO20; oldGCDs_BT = BT.numActivatesO20; oldGCDs_BS = BS.numActivatesO20; oldGCDs_RB = RB.numActivatesO20;
                oldGCDs_SL = SL.numActivatesO20; oldGCDs_VR = VR.numActivatesO20; oldGCDs_HS = HS.numActivatesO20; oldGCDs_CL = CL.numActivatesO20;
                CS.numActivatesO20 = WW.numActivatesO20 = BT.numActivatesO20 = BS.numActivatesO20 = RB.numActivatesO20 = 
                    SL.numActivatesO20 = VR.numActivatesO20 = HS.numActivatesO20 = CL.numActivatesO20 = 0;
                availRageO20 = origAvailRageO20;
                availRageO20 += WhiteAtks.whiteRageGenOverDur * percTimeInDPSAndO20;

                float acts, CSspace, WWspace, BTspace, BSspace, RBspace, SLspace, HSspace, CLspace, VRspace, IRspace;

                // GCDsAvailableO20 is an expensive operation, trying to
                // see if I can speed it up by not calling it so much
                float gcdsAvailableO20 = GCDsAvailableO20;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageO20 < 0f || PercFailRageO20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    PercFailRageO20 *= 1f + repassAvailRageO20 / (availRageO20 - repassAvailRageO20);
                    if (PercFailRageO20 > 1f) { PercFailRageO20 = 1f; }
                } else { PercFailRageO20 = 1f; }

                // Inner Rage, Gives a 15% Damage Buff but 50% Rage Cost Debuff, should only be used when Rage is too high
                if (IR.ability.Validated && PercFailRageO20 == 1f) {
                    acts = IR.ability.Activates * percTimeInDPSAndO20;
                    IR.numActivatesO20 = acts;
                    //availRageO20 -= IR.RageO20 * RageMOD_Total;
                    //gcdsAvailableO20 -= IR.GCDUsageO20;
                }
                IRspace = IR.numActivatesO20 / NumGCDsO20 * IR.ability.UseTime / LatentGCD;

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated) {
                    // We do not need to use the CS.GetActivates(...) function because we will never have the Sudden Death talent as Fury
                    acts = Math.Min(gcdsAvailableO20, CS.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    CS.numActivatesO20 = acts;
                    availRageO20 -= CS.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableO20 -= CS.GCDUsageO20;
                }
                CSspace = CS.numActivatesO20 / NumGCDsO20 * CS.ability.UseTime / LatentGCD;

                // Whirlwind
                if (WW.ability.Validated) {
                    acts = Math.Min(gcdsAvailableO20, WW.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    WW.numActivatesO20 = acts;// *(1f - CSspace);
                    availRageO20 -= WW.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableO20 -= WW.GCDUsageO20;
                }
                WWspace = WW.numActivatesO20 / NumGCDsO20 * WW.ability.UseTime / LatentGCD;

                // Bloodthirst
                if (BT.ability.Validated) {
                    acts = Math.Min(gcdsAvailableO20, BT.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    BT.numActivatesO20 = acts;// *(1f - WWspace - CSspace);
                    availRageO20 -= BT.RageO20 * RageMOD_Total;
                    gcdsAvailableO20 -= BT.GCDUsageO20;
                }
                BTspace = BT.numActivatesO20 / NumGCDsO20 * BT.ability.UseTime / LatentGCD;

                doIterations(); // JOTHAY NOTE: Need to determine exactly what this is doing, may be able to push it to a GetActivates Function
                // Bloodsurge
                if (BS.ability.Validated) {
                    acts = Math.Min(gcdsAvailableO20, (BS.ability as BloodSurge).GetActivates(BT.numActivatesO20, percTimeO20)) * percTimeInDPS * PercFailRageO20;//(OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesO20) * percTimeInDPSAndO20 * PercFailRageO20);
                    BS.numActivatesO20 = acts;// *(1f - WWspace - CSspace - BTspace);
                    availRageO20 -= BS.RageO20 * RageMOD_Total;
                    gcdsAvailableO20 -= BS.GCDUsageO20;
                }
                BSspace = BS.numActivatesO20 / NumGCDsO20 * BS.ability.UseTime / LatentGCD;

                // Raging Blow
                if (RB.ability.Validated) {
                    acts = Math.Min(gcdsAvailableO20, RB.ability.Activates) * percTimeInDPSAndO20 * PercFailRageO20;//(OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesO20) * percTimeInDPSAndO20 * PercFailRageO20);
                    RB.numActivatesO20 = acts;// *(1f - WWspace - CSspace - BTspace - BSspace);
                    availRageO20 -= RB.RageO20 * RageMOD_Total;
                    gcdsAvailableO20 -= RB.GCDUsageO20;
                }
                RBspace = RB.numActivatesO20 / NumGCDsO20 * RB.ability.UseTime / LatentGCD;

                // Victory Rush
                if (VR.ability.Validated) {
                    // If Slam does more damage and we aren't failing at rage, then we ignore Victory Rush
                    if ((SL.ability.Validated && VR.ability.DamageOnUse > SL.ability.DamageOnUse)
                        // if we are rage failing, VR is free so we might use that instead
                        || (PercFailRageO20 != 1f)
                        // NOTE: Relearn this if
                        || (HS.ability.Validated && PercFailRageO20 == 1f && VR.ability.DamageOnUse > SL.ability.DamageOnUse))
                    {
                        acts = Math.Min(gcdsAvailableO20, VR.ability.Activates * percTimeInDPSAndO20); // Since VR is Free, we don't reduc for Rage Fails
                        VR.numActivatesO20 = acts;// *(1f - WWspace - CSspace - BTspace - BSspace - RBspace);
                        //availRage -= VR.Rage; // it's free
                        gcdsAvailableO20 -= VR.GCDUsageO20;
                    }
                }
                VRspace = VR.numActivatesO20 / NumGCDsO20 * VR.ability.UseTime / LatentGCD;

                /* Heroic Strike/Cleave now that they are on GCDs.
                 * These should be rage dumps and will replace Slam in the rotation when used
                 * Computing them together as you use HS for single, CL for Multiple */

                // Heroic Strikes/Cleaves
                if (PercFailRageO20 == 1f && (HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(gcdsAvailableO20, HS.ability.Activates * percTimeInDPSAndO20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRageO20 / CL.ability.RageCost * (MultTargsPerc);
                    float hsActs = availRageO20 / HS.ability.RageCost * (1f - MultTargsPerc);
                    CL.numActivatesO20 = Math.Min(clActs, acts * (MultTargsPerc));
                    HS.numActivatesO20 = Math.Min(hsActs, acts * (1f - MultTargsPerc));
                    availRageO20 -= (HS.RageO20 + CL.RageO20) * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableO20 -= (HS.GCDUsageO20 + CL.GCDUsageO20);
                }
                else if (PercFailRageO20 == 1f && (HS.ability.Validated && !CL.ability.Validated))
                {
                    acts = Math.Min(gcdsAvailableO20, HS.ability.Activates * percTimeInDPSAndO20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float hsActs = availRageO20 / HS.ability.RageCost * RageMOD_BattleTrance;
                    CL.numActivatesO20 = 0f;
                    HS.numActivatesO20 = Math.Min(hsActs, acts);
                    availRageO20 -= HS.RageO20 * RageMOD_Total;
                    gcdsAvailableO20 -= (HS.GCDUsageO20);
                }
                else if (PercFailRageO20 == 1f && (!HS.ability.Validated && CL.ability.Validated))
                {
                    acts = Math.Min(gcdsAvailableO20, CL.ability.Activates * percTimeInDPSAndO20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRageO20 / CL.ability.RageCost * MultTargsPerc;
                    CL.numActivatesO20 = Math.Min(clActs, acts * MultTargsPerc);
                    HS.numActivatesO20 = 0f;
                    availRageO20 -= CL.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableO20 -= (CL.GCDUsageO20);
                }
                else { CL.numActivatesO20 = HS.numActivatesO20 = 0f; }

                // Slam
                if (SL.ability.Validated && PercFailRageO20 != 1)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ gcdsAvailableO20/*SL.Activates*/ * percTimeInDPS/*)*/;
                    if (SL.ability.GetRageUseOverDur(acts) > availRageO20) acts = Math.Max(0f, availRageO20) / SL.ability.RageCost;
                    SL.numActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableO20 -= SL.GCDUsageO20;
                }
                else if (SL.ability.Validated)
                {
                    acts = /*Math.Min(gcdsAvailableO20,*/ gcdsAvailableO20/*SL.Activates*/ * percTimeInDPS/*)*/;
                    SL.numActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableO20 -= SL.GCDUsageO20;
                }
                else { SL.numActivatesO20 = 0f; }

                HSspace = HS.numActivatesO20 / NumGCDsO20 * HS.ability.UseTime / LatentGCD;
                CLspace = CL.numActivatesO20 / NumGCDsO20 * CL.ability.UseTime / LatentGCD;
                SLspace = SL.numActivatesO20 / NumGCDsO20 * SL.ability.UseTime / LatentGCD;
                (HS.ability as HeroicStrike).InciteBonusCrits(HS.numActivatesO20);
                WhiteAtks.Slam_ActsOverDurO20 = SL.numActivatesO20;

                float TotalSpace = (CSspace + WWspace + BTspace + BSspace + RBspace + CSspace + SLspace + HSspace + CLspace + VRspace);
                (IR.ability as InnerRage).FreeRageO20 = repassAvailRageO20 = availRageO20; // check for not enough rage to maintain rotation
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeededO20 = 0f, rageGenOtherO20 = 0f;
            foreach (AbilWrapper aw in GetAbilityList()) {
                DPS_TTL += aw.DPSO20;
                _HPS_TTL += aw.HPSO20;
                if (aw.RageO20 > 0) { rageNeededO20 += aw.RageO20; }
                else { rageGenOtherO20 -= aw.RageO20; }
            }

            DPS_TTL += WhiteAtks.MhDPS * percTimeInDPSAndO20;

            return DPS_TTL;
        }

        /// <summary>
        /// Copied this function from Arms, going to try and rewrite it so it works for Fury
        /// </summary>
        public float SettleAll_U20(float totalPercTimeLost, float rageUsedByMaintenance, float percTimeU20, float availRageU20, out float PercFailRageU20)
        {
            float percTimeO20 = (1f - percTimeU20);
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndU20 = percTimeInDPS * percTimeU20;
            availRageU20 -= rageUsedByMaintenance * percTimeU20;
            /* The following are dependant on other attacks as they are proccing abilities or are the fallback item
             * We need to loop these until the activates are relatively unchanged
             * Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
             * Alternate to Cleave is MultiTargs is active, but only to the perc of time where Targs is active
             * After iterating how many Overrides can be done and still do other abilities, then do the white dps
             *
             * Starting Assumptions:
             * No ability ever procs so Slam sucks up all the cooldowns (except under <20% with that active, where Exec sucks all of them)
             * Heroic Strike and Cleave won't be used at all
             * 
             * Hoped Ending Results:
             * Since we shouldn't actually do normal Slam as Fury, Slams should end up zero (Bloodsurge DPS will be named as such instead of Slam)
             * All abilities will have proc'd and abilities that can proc from other ones will have their activates settled
             * Heroic Strikes and Cleave will activate when there's enough rage to support them AND Executes
             * Execute will get extra rage leftovers if there are any (since you won't use HS/CL <20%)
            */

            float preloopAvailGCDsU20 = GCDsAvailableU20, preloopGCDsUsedU20 = GCDsUsedU20, preloopAvailRageU20 = availRageU20;

            float origNumGCDsU20 = (FightDuration / LatentGCD) * percTimeU20,
                  origavailGCDsU20 = preloopAvailGCDsU20,
                  origGCDsusedU20 = preloopGCDsUsedU20;
            float oldGCDs_CS = 0f, oldGCDs_WW = 0f, oldGCDs_BT = 0f, oldGCDs_BS = 0f, oldGCDs_RB = 0f, oldGCDs_HS = 0f, oldGCDs_CL = 0f, oldGCDs_VR = 0f, oldGCDs_SL = 0f, oldActs_IR = 0f, oldGCDs_EX = 0f;

            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper WW = GetWrapper<WhirlWind>();
            AbilWrapper BT = GetWrapper<BloodThirst>();
            AbilWrapper BS = GetWrapper<BloodSurge>();
            AbilWrapper RB = GetWrapper<RagingBlow>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();
            AbilWrapper VR = GetWrapper<VictoryRush>();
            AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper IR = GetWrapper<InnerRage>();

            AbilWrapper EX = GetWrapper<Execute>();
            Execute EXAbil = EX.ability as Execute;

            EX.numActivatesU20 = origavailGCDsU20;
            WhiteAtks.Slam_ActsOverDurU20 = SL.numActivatesU20 = 0f;
            float origAvailRageU20 = preloopAvailRageU20;
            bool hsok = CalcOpts.M_HeroicStrike;
            bool clok = BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0
                     && CalcOpts.M_Cleave;
            availRageU20 += WhiteAtks.whiteRageGenOverDur * percTimeInDPS * percTimeU20;
            availRageU20 -= SL.RageU20;
            float repassAvailRageU20 = 0f;
            PercFailRageU20 = 1f;

            int Iterator = 0;
            #region >20%
            while (Iterator < 50 && (
                     Math.Abs(CS.numActivatesU20 - oldGCDs_CS) > 0.1f ||
                     Math.Abs(WW.numActivatesU20 - oldGCDs_WW) > 0.1f ||
                     Math.Abs(BT.numActivatesU20 - oldGCDs_BT) > 0.1f ||
                     Math.Abs(BS.numActivatesU20 - oldGCDs_BS) > 0.1f ||
                     Math.Abs(RB.numActivatesU20 - oldGCDs_RB) > 0.1f ||
                     Math.Abs(HS.numActivatesU20 - oldGCDs_HS) > 0.1f ||
                     Math.Abs(CL.numActivatesU20 - oldGCDs_CL) > 0.1f ||
                     Math.Abs(VR.numActivatesU20 - oldGCDs_VR) > 0.1f ||
                     Math.Abs(SL.numActivatesU20 - oldGCDs_SL) > 0.1f ||
                     Math.Abs(EX.numActivatesU20 - oldGCDs_EX) > 0.1f ||
                     Math.Abs(IR.numActivatesU20 - oldActs_IR) > 0.1f))
            {
                // Reset items so we can keep iterating
                WhiteAtks.Slam_ActsOverDurU20 = SL.numActivatesU20;
                oldGCDs_CS = CS.numActivatesU20; oldGCDs_WW = WW.numActivatesU20; oldGCDs_BT = BT.numActivatesU20; oldGCDs_BS = BS.numActivatesU20; oldGCDs_RB = RB.numActivatesU20;
                oldGCDs_SL = SL.numActivatesU20; oldGCDs_VR = VR.numActivatesU20; oldGCDs_HS = HS.numActivatesU20; oldGCDs_CL = CL.numActivatesU20; oldGCDs_EX = EX.numActivatesU20;
                CS.numActivatesU20 = WW.numActivatesU20 = BT.numActivatesU20 = BS.numActivatesU20 = RB.numActivatesU20 =
                    SL.numActivatesU20 = VR.numActivatesU20 = HS.numActivatesU20 = CL.numActivatesU20 = 0;
                availRageU20 = origAvailRageU20;
                availRageU20 += WhiteAtks.whiteRageGenOverDur * percTimeInDPSAndU20;

                float acts, CSspace, WWspace, BTspace, BSspace, RBspace, SLspace, HSspace, CLspace, VRspace, IRspace, EXspace;

                // GCDsAvailableU20 is an expensive operation, trying to
                // see if I can speed it up by not calling it so much
                float gcdsAvailableU20 = GCDsAvailableU20;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageU20 < 0f || PercFailRageU20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    PercFailRageU20 *= 1f + repassAvailRageU20 / (availRageU20 - repassAvailRageU20);
                    if (PercFailRageU20 > 1f) { PercFailRageU20 = 1f; }
                } else { PercFailRageU20 = 1f; }

                // Inner Rage, Gives a 15% Damage Buff but 50% Rage Cost Debuff, should only be used when Rage is too high
                if (IR.ability.Validated && PercFailRageU20 == 1f)
                {
                    acts = IR.ability.Activates * percTimeInDPSAndU20;
                    IR.numActivatesU20 = acts;
                    //availRageU20 -= IR.RageU20 * RageMOD_Total;
                    //gcdsAvailableU20 -= IR.GCDUsageU20;
                }
                IRspace = IR.numActivatesU20 / NumGCDsU20 * IR.ability.UseTime / LatentGCD;

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated)
                {
                    // We do not need to use the CS.GetActivates(...) function because we will never have the Sudden Death talent as Fury
                    acts = Math.Min(gcdsAvailableU20, CS.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    CS.numActivatesU20 = acts;
                    availRageU20 -= CS.RageU20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableU20 -= CS.GCDUsageU20;
                }
                CSspace = CS.numActivatesU20 / NumGCDsU20 * CS.ability.UseTime / LatentGCD;

                // Whirlwind
                if (WW.ability.Validated)
                {
                    acts = Math.Min(gcdsAvailableU20, WW.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    WW.numActivatesU20 = acts;// *(1f - CSspace);
                    availRageU20 -= WW.RageU20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableU20 -= WW.GCDUsageU20;
                }
                WWspace = WW.numActivatesU20 / NumGCDsU20 * WW.ability.UseTime / LatentGCD;

                // Bloodthirst
                if (BT.ability.Validated)
                {
                    acts = Math.Min(gcdsAvailableU20, BT.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    BT.numActivatesU20 = acts;// *(1f - WWspace - CSspace);
                    availRageU20 -= BT.RageU20 * RageMOD_Total;
                    gcdsAvailableU20 -= BT.GCDUsageU20;
                }
                BTspace = BT.numActivatesU20 / NumGCDsU20 * BT.ability.UseTime / LatentGCD;

                doIterations(); // JOTHAY NOTE: Need to determine exactly what this is doing, may be able to push it to a GetActivates Function
                // Bloodsurge
                if (BS.ability.Validated)
                {
                    acts = Math.Min(gcdsAvailableU20, (BS.ability as BloodSurge).GetActivates(BT.numActivatesU20, percTimeU20)) * percTimeInDPS * PercFailRageU20;//(OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesU20) * percTimeInDPSAndU20 * PercFailRageU20);
                    BS.numActivatesU20 = acts;// *(1f - WWspace - CSspace - BTspace);
                    availRageU20 -= BS.RageU20 * RageMOD_Total;
                    gcdsAvailableU20 -= BS.GCDUsageU20;
                }
                BSspace = BS.numActivatesU20 / NumGCDsU20 * BS.ability.UseTime / LatentGCD;

                // Raging Blow
                if (RB.ability.Validated)
                {
                    acts = Math.Min(gcdsAvailableU20, RB.ability.Activates) * percTimeInDPSAndU20 * PercFailRageU20;//(OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesU20) * percTimeInDPSAndU20 * PercFailRageU20);
                    RB.numActivatesU20 = acts;// *(1f - WWspace - CSspace - BTspace - BSspace);
                    availRageU20 -= RB.RageU20 * RageMOD_Total;
                    gcdsAvailableU20 -= RB.GCDUsageU20;
                }
                RBspace = RB.numActivatesU20 / NumGCDsU20 * RB.ability.UseTime / LatentGCD;

                // Victory Rush
                if (VR.ability.Validated)
                {
                    // If Slam does more damage and we aren't failing at rage, then we ignore Victory Rush
                    if ((SL.ability.Validated && VR.ability.DamageOnUse > SL.ability.DamageOnUse)
                        // if we are rage failing, VR is free so we might use that instead
                        || (PercFailRageU20 != 1f)
                        // NOTE: Relearn this if
                        || (HS.ability.Validated && PercFailRageU20 == 1f && VR.ability.DamageOnUse > SL.ability.DamageOnUse))
                    {
                        acts = Math.Min(gcdsAvailableU20, VR.ability.Activates * percTimeInDPSAndU20); // Since VR is Free, we don't reduc for Rage Fails
                        VR.numActivatesU20 = acts;// *(1f - WWspace - CSspace - BTspace - BSspace - RBspace);
                        //availRage -= VR.Rage; // it's free
                        gcdsAvailableU20 -= VR.GCDUsageU20;
                    }
                }
                VRspace = VR.numActivatesU20 / NumGCDsU20 * VR.ability.UseTime / LatentGCD;

                /* Heroic Strike/Cleave now that they are on GCDs.
                 * These should be rage dumps and will replace Slam in the rotation when used
                 * Computing them together as you use HS for single, CL for Multiple */

                // Heroic Strikes/Cleaves
                if (PercFailRageU20 == 1f && (HS.ability.Validated && CL.ability.Validated))
                {
                    acts = Math.Min(gcdsAvailableU20, HS.ability.Activates * percTimeInDPSAndU20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRageU20 / CL.ability.RageCost * (MultTargsPerc);
                    float hsActs = availRageU20 / HS.ability.RageCost * (1f - MultTargsPerc);
                    CL.numActivatesU20 = Math.Min(clActs, acts * (MultTargsPerc));
                    HS.numActivatesU20 = Math.Min(hsActs, acts * (1f - MultTargsPerc));
                    availRageU20 -= (HS.RageU20 + CL.RageU20) * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableU20 -= (HS.GCDUsageU20 + CL.GCDUsageU20);
                }
                else if (PercFailRageU20 == 1f && (HS.ability.Validated && !CL.ability.Validated))
                {
                    acts = Math.Min(gcdsAvailableU20, HS.ability.Activates * percTimeInDPSAndU20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float hsActs = availRageU20 / HS.ability.RageCost * RageMOD_BattleTrance;
                    CL.numActivatesU20 = 0f;
                    HS.numActivatesU20 = Math.Min(hsActs, acts);
                    availRageU20 -= HS.RageU20 * RageMOD_Total;
                    gcdsAvailableU20 -= (HS.GCDUsageU20);
                }
                else if (PercFailRageU20 == 1f && (!HS.ability.Validated && CL.ability.Validated))
                {
                    acts = Math.Min(gcdsAvailableU20, CL.ability.Activates * percTimeInDPSAndU20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRageU20 / CL.ability.RageCost * MultTargsPerc;
                    CL.numActivatesU20 = Math.Min(clActs, acts * MultTargsPerc);
                    HS.numActivatesU20 = 0f;
                    availRageU20 -= CL.RageU20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableU20 -= (CL.GCDUsageU20);
                }
                else { CL.numActivatesU20 = HS.numActivatesU20 = 0f; }

                // Slam
                /*if (SL.ability.Validated && PercFailRageU20 != 1) {
                    acts =  gcdsAvailableU20 * percTimeInDPS;
                    if (SL.ability.GetRageUseOverDur(acts) > availRageU20) acts = Math.Max(0f, availRageU20) / SL.ability.RageCost;
                    SL.numActivatesU20 = acts;
                    availRageU20 -= SL.RageU20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableU20 -= SL.GCDUsageU20;
                } else if (SL.ability.Validated) {
                    acts = gcdsAvailableU20 * percTimeInDPS;
                    SL.numActivatesU20 = acts;
                    availRageU20 -= SL.RageU20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableU20 -= SL.GCDUsageU20;
                } else {*/
                SL.numActivatesU20 = 0f; //}

                HSspace = HS.numActivatesU20 / NumGCDsU20 * HS.ability.UseTime / LatentGCD;
                CLspace = CL.numActivatesU20 / NumGCDsU20 * CL.ability.UseTime / LatentGCD;
                SLspace = SL.numActivatesU20 / NumGCDsU20 * SL.ability.UseTime / LatentGCD;
                (HS.ability as HeroicStrike).InciteBonusCrits(HS.numActivatesU20);
                WhiteAtks.Slam_ActsOverDurU20 = SL.numActivatesU20;

                // Execute
                if (EX.ability.Validated) {
                    acts = gcdsAvailableU20 * percTimeInDPS;
                    EX.numActivatesU20 = acts;
                    availRageU20 -= EX.RageU20 * RageMOD_Total * RageMOD_BattleTrance;
                    gcdsAvailableU20 -= EX.GCDUsageU20;
                }
                EXspace = EX.numActivatesU20 / NumGCDsU20 * EX.ability.UseTime / LatentGCD;

                float TotalSpace = (CSspace + WWspace + BTspace + BSspace + RBspace + CSspace + SLspace + HSspace + CLspace + VRspace + EXspace);
                EXAbil.FreeRage = (IR.ability as InnerRage).FreeRageU20 = repassAvailRageU20 = availRageU20; // check for not enough rage to maintain rotation
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeededU20 = 0f, rageGenOtherU20 = 0f;
            foreach (AbilWrapper aw in GetAbilityList())
            {
                DPS_TTL += aw.DPSU20;
                _HPS_TTL += aw.HPSU20;
                if (aw.RageU20 > 0) { rageNeededU20 += aw.RageU20; }
                else { rageGenOtherU20 -= aw.RageU20; }
            }

            DPS_TTL += WhiteAtks.MhDPS * percTimeInDPSAndU20;

            return DPS_TTL;
        }

        /// <summary>
        /// Copied this function from Arms, going to try and rewrite it so it works for Fury
        /// </summary>
        public void MakeRotationandDoDPS(bool setCalcs, float percTimeUnder20) {
            if (Char.MainHand == null) { return; }
            _HPS_TTL = 0f;
            if (_needDisplayCalcs) { GCDUsage += string.Format("All=Over20%+Under20%. Only applicable if using Exec Spam\n{0:000.000}={1:000.000}+{2:000.000} : Total GCDs\n\n", NumGCDsAll, NumGCDsO20, NumGCDsU20); }

            // ==== Impedences ========================
            if (_needDisplayCalcs) { GCDUsage += "Impedences: Things that prevent you from DPS'g\n"; }
            float TotalPercTimeLost = CalculateTimeLost();
            if (_needDisplayCalcs && TotalPercTimeLost <= 0f) { GCDUsage += "None\n\n"; }
            else if (_needDisplayCalcs) { GCDUsage += (TotalPercTimeLost != 0f ? "\n" : ""); }

            // ==== Rage Generation Priorities ========
            float availRage = 0f;
            float PercFailRageUnder20 = 1f, PercFailRageOver20 = 1f;
            availRage += RageGenOverDur_Other + RageGainedWhileMoving;

            // ==== Maintenance and Anti-Impedence Priorities ====
            if (_needDisplayCalcs) GCDUsage += "Maintenance: Things that you do periodically to Buff yourself or the raid\n";
            float rageUsedByMaintenance = DoMaintenanceActivates(TotalPercTimeLost);

            // ==== Standard Priorities ===============
            if (_needDisplayCalcs) GCDUsage += "Abilities: Things that you do to damage the Target. These are not in order of priority.\n";
            _DPS_TTL_O20 = SettleAll(TotalPercTimeLost, rageUsedByMaintenance, percTimeUnder20, availRage, out PercFailRageOver20);
            if (percTimeUnder20 != 0f) { _DPS_TTL_U20 = SettleAll_U20(TotalPercTimeLost, rageUsedByMaintenance, percTimeUnder20, availRage, out PercFailRageUnder20); }
            if (_needDisplayCalcs) {
                // We need to add Inner Rage now that we know how many there are
                AbilWrapper aw = GetWrapper<InnerRage>();
                GCDUsage = aw.allNumActivates > 0 ? GCDUsage.Insert(GCDUsage.IndexOf("Abilities") - 2,
                    string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                                aw.ability.Name, (!aw.ability.UsesGCD ? " (Doesn't use GCDs)" : "")
                                ))
                    : GCDUsage;
            }

            calcDeepWounds();
            _DPS_TTL_O20 += DW.TickSize;
            _DPS_TTL_U20 += DW.TickSize;

            // Add each of the abilities' DPS and HPS values and other aesthetics
            if (_needDisplayCalcs)
            {
                if (PercFailRageOver20 != 1.0f || PercFailRageUnder20 != 1.0f)
                {
                    GCDUsage += (PercFailRageOver20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due\nto Rage Starvation before Execute Spam.\n", (1f - PercFailRageOver20)) : "");
                    GCDUsage += (PercFailRageUnder20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due\nto Rage Starvation during Execute Spam.\n", (1f - PercFailRageUnder20)) : "");
                    GCDUsage += "\n";
                }
                List<AbilWrapper> dmgAbils = GetDamagingAbilities();
                foreach (AbilWrapper aw in dmgAbils)
                {
                    if (aw.allNumActivates > 0 && !aw.ability.isMaint && !(aw.ability is HeroicLeap))
                    {
                        if (aw.ability.GCDTime < 1.5f || aw.ability.GCDTime > 3f)
                        {
                            // Overpower (And TfB procs) use less than a GCD to recouperate.
                            // Bladestorm is channelled over 6 seconds (approx 4 GCDs)
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000} : {5}\n",
                                aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                                aw.ability.GCDTime,
                                (aw.allNumActivates * aw.ability.GCDTime / (CalcOpts.FullLatency + 1.5f)),
                                aw.ability.Name
                            );
                        }
                        else
                        {
                            GCDUsage += string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                                aw.ability.Name,
                                aw.ability.UsesGCD ? "" : " (Doesn't use GCDs)");
                        }
                    }
                }
                GCDUsage += string.Format("\n{0:000.000}={1:000.000}+{2:000.000} : Available GCDs (should be at or near zero)",
                    GCDsAvailableO20 + (percTimeUnder20 != 0f ? GCDsAvailableU20 : 0f),
                    GCDsAvailableO20,
                    (percTimeUnder20 != 0f ? GCDsAvailableU20 : 0f));
            }

            // Return result
            if (setCalcs)
            {
                this.calcs.WhiteDPSMH    = WhiteAtks.GetMHDPS(WhiteAtks.MhActivatesO20, TimeOver20Perc);
                this.calcs.WhiteDPSMHU20 = WhiteAtks.GetMHDPS(WhiteAtks.MhActivatesU20, TimeUndr20Perc);
                this.calcs.WhiteDPSOH    = WhiteAtks.GetOHDPS(WhiteAtks.OhActivatesO20, TimeOver20Perc);
                this.calcs.WhiteDPSOHU20 = WhiteAtks.GetOHDPS(WhiteAtks.OhActivatesU20, TimeUndr20Perc);
                {
                    if (this.calcs.WhiteDPSMH > 0 && this.calcs.WhiteDPSMHU20 > 0 && this.calcs.WhiteDPSOH > 0 && this.calcs.WhiteDPSOHU20 > 0) {
                        this.calcs.WhiteDPS = (this.calcs.WhiteDPSMH + this.calcs.WhiteDPSMHU20 + this.calcs.WhiteDPSOH + this.calcs.WhiteDPSOHU20) / 2f;
                    } else if (this.calcs.WhiteDPSMHU20 > 0 && this.calcs.WhiteDPSOHU20 > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMHU20 + this.calcs.WhiteDPSOHU20;
                    } else if (this.calcs.WhiteDPSMH > 0 && this.calcs.WhiteDPSOH > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMH + this.calcs.WhiteDPSOH;
                    } else { this.calcs.WhiteDPS = 0f; }
                }
                this.calcs.WhiteDmg = (this.WhiteAtks.MhDamageOnUse + this.WhiteAtks.OhDamageOnUse);

                {
                    if (_DPS_TTL_O20 > 0 && _DPS_TTL_U20 > 0) { this.calcs.TotalDPS = (_DPS_TTL_O20 + _DPS_TTL_U20) / 2f; }
                    else if (_DPS_TTL_U20 > 0) { this.calcs.TotalDPS = _DPS_TTL_U20; }
                    else if (_DPS_TTL_O20 > 0) { this.calcs.TotalDPS = _DPS_TTL_O20; }
                    else { this.calcs.TotalDPS = 0f; }
                }

                this.calcs.WhiteRageO20 = WhiteAtks.MHRageGenOverDurO20 + WhiteAtks.OHRageGenOverDurO20;
                this.calcs.OtherRageO20 = this.RageGenOverDur_OtherO20;
                this.calcs.NeedyRageO20 = this.RageNeededOverDurO20;
                this.calcs.FreeRageO20 = calcs.WhiteRageO20 + calcs.OtherRageO20 - calcs.NeedyRageO20;

                this.calcs.WhiteRageU20 = WhiteAtks.MHRageGenOverDurU20 + WhiteAtks.OHRageGenOverDurU20;
                this.calcs.OtherRageU20 = this.RageGenOverDur_OtherU20;
                this.calcs.NeedyRageU20 = this.RageNeededOverDurU20;
                this.calcs.FreeRageU20 = calcs.WhiteRageU20 + calcs.OtherRageU20 - calcs.NeedyRageU20;
            }
        }

        public override void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalcs)
        {
            #region Cata Calcs
            try {
                base.MakeRotationandDoDPS(setCalcs, needsDisplayCalcs);
                MakeRotationandDoDPS(setCalcs, TimeUndr20Perc);
            } catch (Exception ex) {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error in creating Fury Rotation Details",
                    ex.Message, ex.InnerException,
                    "MakeRotationandDoDPS(...)", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
            #endregion
            #region WotLK based Calcs
#if FALSE
            base.MakeRotationandDoDPS(setCalcs, needsDisplayCalcs);
            //new_MakeRotationandDoDPS(setCalcs);
            // Starting Numbers
            float DPS_TTL = 0f, HPS_TTL = 0f;
            float LatentGCD = 1.5f + CalcOpts.Latency;
            if (_needDisplayCalcs) GCDUsage += "NumGCDs: " + NumGCDsO20.ToString() + "\n\n";
            float GCDsused = 0f;
            float availGCDs = Math.Max(0f, NumGCDsO20 - GCDsused);
            float availRage = 0f;
            float rageadd = 0f;
            //float timelostwhilestunned = 0f;
            //float percTimeInStun = 0f;

            if (Char.MainHand == null) { return; }
            //doIterations();

            // ==== Rage Generation Priorities ========
            availRage += RageGenOverDur_Other;
            
            /*Bloodrage         */
            //AddAnItem(ref availRage, percTimeInStun, ref _Blood_GCDs, ref HPS_TTL, ref _Blood_HPS, BR);
            /*Berserker Rage    */
            
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
            #region Stuns
            /*if (CalcOpts.StunningTargets && CalcOpts.Stuns.Count > 0f) 
            {
                float numStuns = FightDuration / CalcOpts.StunningTargetsFreq;
                if (!CalcOpts.SE_UseDur) numStuns = (float)Math.Floor(numStuns);
                // ways to counter, no GCD, just use reaction time
                if (Talents.HeroicFury > 0f)
                {
                    float numUses = Math.Min(numStuns, GetWrapper<HeroicFury>().ability.Activates);
                    numStuns -= numUses;
                    timeLost += numUses * (CalcOpts.React / 1000f + CalcOpts.Latency); // not using GetReact because it's not on the GCD
                    GetWrapper<HeroicFury>().numActivates += numUses;
                }
                if (Char.Race == CharacterRace.Human)
                {
                    float numUses = Math.Min(numStuns, GetWrapper<EveryManForHimself>().ability.Activates);
                    numStuns -= numUses;
                    timeLost += numUses * CalcOpts.React / 1000f + CalcOpts.Latency; // not using GetReact because it's not on the GCD
                    GetWrapper<EveryManForHimself>().numActivates += numUses;
                }

                if (numStuns > 0f)
                {
                    float stunDur = CalcOpts.StunningTargetsDur / 1000f * (1f - Talents.IronWill * 2f / 30f);
                    timeLost += stunDur * numStuns;
                }
            }*/
            #endregion
            float otherTimeLost = CalculateTimeLost();
            DoMaintenanceActivates(otherTimeLost);

            float timeLostPerc = otherTimeLost;
            #endregion

            AbilWrapper WW = GetWrapper<WhirlWind>();
            AbilWrapper BT = GetWrapper<BloodThirst>();
            AbilWrapper BS = GetWrapper<BloodSurge>();
            AbilWrapper RB = GetWrapper<RagingBlow>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();

            // Priority 1 : Whirlwind on every CD
            float WW_GCDs = Math.Min(availGCDs, WW.ability.Activates) * (1f - timeLostPerc);
            WW.numActivatesO20 = WW_GCDs;
            GCDsused += Math.Min(NumGCDsO20, WW_GCDs);
            if (_needDisplayCalcs) GCDUsage += WW.ability.Name + ": " + WW_GCDs.ToString() + "\n";
            availGCDs = Math.Max(0f, NumGCDsO20 - GCDsused);
            DPS_TTL += WW.DPSO20;
            HPS_TTL += WW.HPSO20;
            rageadd = WW.allRage;
            availRage -= rageadd;
            
            // Priority 2 : Bloodthirst on every CD
            float BT_GCDs = Math.Min(availGCDs, BT.ability.Activates) * (1f - timeLostPerc);
            BT.numActivatesO20 = BT_GCDs;
            GCDsused += Math.Min(NumGCDsO20, BT_GCDs);
            if (_needDisplayCalcs) GCDUsage += BT.ability.Name + ": " + BT_GCDs.ToString() + "\n";
            availGCDs = Math.Max(0f, NumGCDsO20 - GCDsused);
            DPS_TTL += BT.DPSO20;
            HPS_TTL += BT.HPSO20;
            rageadd = BT.allRage;
            availRage -= rageadd;
            
            doIterations();
            // Priority 3 : Bloodsurge Blood Proc (Do an Instant Slam) if available
            float BS_GCDs = Math.Min(availGCDs, BS.ability.Activates) * (1f - timeLostPerc);
            BS.numActivatesO20 = BS_GCDs;
            GCDsused += Math.Min(NumGCDsO20, BS_GCDs);
            if (_needDisplayCalcs) GCDUsage += BS.ability.Name + ": " + BS_GCDs.ToString() + "\n";
            availGCDs = Math.Max(0f, NumGCDsO20 - GCDsused);
            DPS_TTL += BS.DPSO20;
            HPS_TTL += BS.HPSO20;
            rageadd = BS.allRage;
            availRage -= rageadd;
            
            // Priority 4 : Raging Blow if available
            float RB_GCDs = Math.Min(availGCDs, RB.ability.Activates) * (1f - timeLostPerc);
            RB.numActivatesO20 = RB_GCDs;
            GCDsused += Math.Min(NumGCDsO20, RB_GCDs);
            if (_needDisplayCalcs) GCDUsage += RB.ability.Name + ": " + RB_GCDs.ToString() + "\n";
            availGCDs = Math.Max(0f, NumGCDsO20 - GCDsused);
            DPS_TTL += RB.DPSO20;
            HPS_TTL += RB.HPSO20;
            rageadd = RB.allRage;
            availRage -= rageadd;

            InvalidateCache();

            // Heroic Strike/Cleave now that they are on GCDs. These should be rage dumps
            // Computing them together as you use HS for single, CL for Multiple
            if (/*PercFailRage == 1f &&*/ (HS.ability.Validated || CL.ability.Validated))
            {
                float acts = Math.Min(GCDsAvailableO20, HS.ability.Activates /** percTimeInDPSAndOver20*/);
                float Abil_GCDs = acts;
                CL.numActivatesO20 = Abil_GCDs * (BossOpts.MultiTargsTime / FightDuration);
                HS.numActivatesO20 = Abil_GCDs - CL.numActivatesO20;
                (HS.ability as HeroicStrike).InciteBonusCrits(HS.numActivatesO20);
                //availGCDs = Math.Max(0f, origNumGCDs - GCDsused);
                availRage -= HS.RageO20 + CL.RageO20;
            }
            //HSspace = HS.numActivates / NumGCDs * HS.ability.UseTime / LatentGCD;
            //CLspace = CL.numActivates / NumGCDs * CL.ability.UseTime / LatentGCD;

            // Priority 4 : Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
            // Alternate to Cleave is MultiTargs is active
            // After iterating how many Overrides can be done and still do other abilities, then do the white dps

            availRage += WhiteAtks.MHRageGenOverDur + WhiteAtks.OHRageGenOverDur;

            /*bool HSok = CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool CLok = 
                BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0
                && CalcOpts.Maintenance[(int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_];*/

            WhiteAtks.Slam_ActsOverDurO20 = WhiteAtks.Slam_ActsOverDurU20 = 0f;// _SL_GCDs;
 
            DPS_TTL += WhiteAtks.MhDPS * (1f - timeLostPerc) + WhiteAtks.OhDPS * (1f - timeLostPerc);
            DPS_TTL += GetWrapper<HeroicStrike>().DPSO20;
            DPS_TTL += GetWrapper<Cleave>().DPSO20;
 
            calcDeepWounds();
            DPS_TTL += DW.DPS;
            if (_needDisplayCalcs) GCDUsage += "\nAvail: " + availGCDs.ToString();

            // Return result
            _HPS_TTL = HPS_TTL;

            if (setCalcs)
            {
                this.calcs.TotalDPS = DPS_TTL;
                this.calcs.WhiteDPS = (WhiteAtks.MhDPS + WhiteAtks.OhDPS) * (1f - timeLostPerc);

                this.calcs.WhiteRageO20  = WhiteAtks.MHRageGenOverDur + WhiteAtks.OHRageGenOverDur;
                this.calcs.OtherRageO20 = RageGenOverDur_Other;
                this.calcs.NeedyRageO20 = RageNeededOverDur;
                this.calcs.FreeRageO20 = calcs.WhiteRageO20 + calcs.OtherRageO20 - calcs.NeedyRageO20;
                this.calcs.WhiteDPSMH = this.WhiteAtks.MhDPS;
                this.calcs.WhiteDPSMHU20 = -1f; // Fury doesn't use this
                this.calcs.WhiteDPSOH = this.WhiteAtks.OhDPS;
                this.calcs.WhiteDmg   = this.WhiteAtks.MhDamageOnUse + this.WhiteAtks.OhDamageOnUse;
            }
#endif
            #endregion
        }
    }
}