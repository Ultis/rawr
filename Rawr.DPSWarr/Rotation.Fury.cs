/**********
 * Owner: Ebs
 **********/
using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSWarr.Skills;

namespace Rawr.DPSWarr {
    public class FuryRotation : Rotation {
        public FuryRotation(DPSWarrCharacter dpswarrchar) {
            DPSWarrChar = dpswarrchar;

            _cachedLatentGCD = 1.5f + DPSWarrChar.CalcOpts.Latency + DPSWarrChar.CalcOpts.AllowedReact;
            AbilityWrapper.LatentGCD = _cachedLatentGCD;
            _cachedNumGCDsO20 = FightDurationO20 / LatentGCD;
            _cachedNumGCDsU20 = FightDurationU20 / LatentGCD;
        }

        public override void Initialize()
        {
            InitAbilities();
        }

        /// <summary>
        /// Copied this function from Arms, going to try and rewrite it so it works for Fury
        /// </summary>
        public float SettleAll(float totalPercTimeLost, float rageUsedByMaintenance, float percTimeU20, float availRageO20, out float percFailRageO20)
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

            float preloopAvailGCDsO20 = GCDsAvailableO20, preloopAvailRageO20 = availRageO20;

            float oldGCDs_CS = 0f, oldGCDs_WW = 0f, oldGCDs_BT = 0f, oldGCDs_BS = 0f, oldGCDs_RB = 0f, oldGCDs_HS = 0f, oldGCDs_CL = 0f, oldGCDs_VR = 0f, oldGCDs_SL = 0f, oldActs_IR = 0f;


            AbilityWrapper CS = GetWrapper<ColossusSmash>();
            AbilityWrapper BT = GetWrapper<Bloodthirst>();
            AbilityWrapper BS = GetWrapper<BloodSurge>();
            AbilityWrapper RB = GetWrapper<RagingBlow>();
            AbilityWrapper HS = GetWrapper<HeroicStrike>();
            AbilityWrapper CL = GetWrapper<Cleave>();
            AbilityWrapper IR = GetWrapper<InnerRage>();

            AbilityWrapper WW = GetWrapper<Whirlwind>();
            AbilityWrapper SL = GetWrapper<Slam>();
            AbilityWrapper VR = GetWrapper<VictoryRush>();

            BT.NumActivatesO20 = preloopAvailGCDsO20 / 2f;
            DPSWarrChar.Whiteattacks.SlamActsOverDurO20 = 0;
            float origAvailRageO20 = preloopAvailRageO20;
            float repassAvailRageO20 = 0f;
            percFailRageO20 = 1f;

            int Iterator = 0;
            #region >20%
            while (Iterator < 50 && (
                     Math.Abs(CS.NumActivatesO20 - oldGCDs_CS) > 0.1f ||
                     Math.Abs(WW.NumActivatesO20 - oldGCDs_WW) > 0.1f ||
                     Math.Abs(BT.NumActivatesO20 - oldGCDs_BT) > 0.1f ||
                     Math.Abs(BS.NumActivatesO20 - oldGCDs_BS) > 0.1f ||
                     Math.Abs(RB.NumActivatesO20 - oldGCDs_RB) > 0.1f ||
                     Math.Abs(HS.NumActivatesO20 - oldGCDs_HS) > 0.1f ||
                     Math.Abs(CL.NumActivatesO20 - oldGCDs_CL) > 0.1f ||
                     Math.Abs(VR.NumActivatesO20 - oldGCDs_VR) > 0.1f ||
                     Math.Abs(SL.NumActivatesO20 - oldGCDs_SL) > 0.1f ||
                     Math.Abs(IR.NumActivatesO20 - oldActs_IR) > 0.1f))
            {
                // Reset items so we can keep iterating
                oldGCDs_CS = CS.NumActivatesO20; oldGCDs_BT = BT.NumActivatesO20; oldGCDs_BS = BS.NumActivatesO20; oldGCDs_RB = RB.NumActivatesO20;
                oldGCDs_HS = HS.NumActivatesO20; oldGCDs_CL = CL.NumActivatesO20;
                CS.NumActivatesO20 = BT.NumActivatesO20 = BS.NumActivatesO20 = RB.NumActivatesO20 = HS.NumActivatesO20 = CL.NumActivatesO20 = 0;
                availRageO20 = origAvailRageO20;
                availRageO20 += (float)((DPSWarrChar.Whiteattacks.MHActivatesO20*DPSWarrChar.Whiteattacks.MHAtkTable.AnyLand)*
                                6.5*DPSWarrChar.CombatFactors.MH.Speed);
                if (DPSWarrChar.CombatFactors.useOH) {
                    availRageO20 += (float)((DPSWarrChar.Whiteattacks.OHActivatesO20 * DPSWarrChar.Whiteattacks.OHAtkTable.AnyLand) *
                                    6.5 * DPSWarrChar.CombatFactors.OH.Speed) / 2f;
                }

                float acts;

                // GCDsAvailableO20 is an expensive operation, trying to
                // see if I can speed it up by not calling it so much
                float gcdsAvailableO20 = GCDsAvailableO20;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageO20 < 0f || percFailRageO20 != 1f) 
                {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    percFailRageO20 *= 1f + repassAvailRageO20 / (availRageO20 - repassAvailRageO20);
                    if (percFailRageO20 > 1f)
                        percFailRageO20 = 1f;
                } else
                {
                    percFailRageO20 = 1f;
                }

                // Inner Rage, Gives a 15% Damage Buff but 50% Rage Cost Debuff, should only be used when Rage is too high
                if (IR.Ability.Validated && percFailRageO20 == 1f)
                {
                    acts = IR.Ability.Activates * percTimeInDPSAndO20;
                    IR.NumActivatesO20 = acts;
                }

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.Ability.Validated) {
                    acts = Math.Min(gcdsAvailableO20, CS.Ability.Activates * percTimeInDPSAndO20 * percFailRageO20);
                    CS.NumActivatesO20 = acts;
                    availRageO20 -= CS.RageO20 * RageModTotal * RageModBattleTrance;
                    gcdsAvailableO20 -= CS.GCDUsageO20;
                }

                // Raging Blow
                if (RB.Ability.Validated)
                {
                    acts = Math.Min(gcdsAvailableO20, RB.Ability.Activates * percTimeInDPSAndO20 * percFailRageO20);
                    RB.NumActivatesO20 = acts;
                    availRageO20 -= RB.RageO20 * RageModTotal * RageModBattleTrance;
                    gcdsAvailableO20 -= RB.GCDUsageO20;
                }

                // Bloodthirst
                if (BT.Ability.Validated) {
                    acts = Math.Min(gcdsAvailableO20, BT.Ability.Activates * percTimeInDPSAndO20 * percFailRageO20);
                    BT.NumActivatesO20 = acts;
                    availRageO20 -= BT.RageO20 * RageModTotal;
                    gcdsAvailableO20 -= BT.GCDUsageO20;
                }

//                DoIterations(); // JOTHAY NOTE: Need to determine exactly what this is doing, may be able to push it to a GetActivates Function
                // Bloodsurge
                if (BS.Ability.Validated) {
                    acts = Math.Min(gcdsAvailableO20, (BS.Ability as BloodSurge).GetActivates(BT.NumActivatesO20, percTimeO20) * percTimeInDPS * percFailRageO20);
                    BS.NumActivatesO20 = acts;// *(1f - WWspace - CSspace - BTspace);
                    availRageO20 -= BS.RageO20 * RageModTotal;
                    gcdsAvailableO20 -= BS.GCDUsageO20;
                }


                // Slam
                if (SL.Ability.Validated)
                {
                    acts = Math.Min(gcdsAvailableO20, SL.Ability.Activates * percTimeInDPSAndO20);
                    SL.NumActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageModTotal * RageModBattleTrance;
                    gcdsAvailableO20 -= SL.GCDUsageO20;
                }
                else { SL.NumActivatesO20 = 0f; }

                // Whirlwind
                if (WW.Ability.Validated)
                {
                    acts = Math.Min(gcdsAvailableO20, WW.Ability.Activates * percTimeInDPSAndO20 * percFailRageO20);
                    WW.NumActivatesO20 = acts;// *(1f - CSspace);
                    availRageO20 -= WW.RageO20 * RageModTotal * RageModBattleTrance;
                    gcdsAvailableO20 -= WW.GCDUsageO20;
                }


                // Victory Rush
                if (VR.Ability.Validated) {
                    // If Slam does more damage and we aren't failing at rage, then we ignore Victory Rush
                    if ((SL.Ability.Validated && VR.Ability.DamageOnUse > SL.Ability.DamageOnUse)
                        // if we are rage failing, VR is free so we might use that instead
                        || (percFailRageO20 != 1f)
                        // NOTE: Relearn this if
                        || (HS.Ability.Validated && percFailRageO20 == 1f && VR.Ability.DamageOnUse > SL.Ability.DamageOnUse))
                    {
                        acts = Math.Min(gcdsAvailableO20, VR.Ability.Activates * percTimeInDPSAndO20); // Since VR is Free, we don't reduc for Rage Fails
                        VR.NumActivatesO20 = acts;// *(1f - WWspace - CSspace - BTspace - BSspace - RBspace);
                        //availRage -= VR.Rage; // it's free
                        gcdsAvailableO20 -= VR.GCDUsageO20;
                    }
                }

                // Cleaves before Heroic Strikes
                if (percFailRageO20 >= 1f && CL.Ability.Validated && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/)
                {
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float multTargsPerc = DPSWarrChar.BossOpts.MultiTargsTime / FightDuration;
                    acts = CL.Ability.Activates*multTargsPerc*percTimeInDPSAndO20
                           *(1f/RageModTotal)
                           *(1f/RageModBattleTrance);
                    float clRageLimitedActs = (availRageO20/CL.Ability.RageCost)*multTargsPerc*percTimeInDPSAndO20
                                              *(1f/RageModTotal)
                                              *(1f/RageModBattleTrance);
                    CL.NumActivatesO20 = Math.Min(clRageLimitedActs, acts);
                    availRageO20 -= CL.RageO20 * RageModTotal * RageModBattleTrance;
                }

                // Heroic Strikes, limited by rage and Cleaves
                if (percFailRageO20 >= 1f && HS.Ability.Validated && availRageO20 > 0 /*&& gcdsAvailableO20 > 0*/)
                {
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    acts = HS.Ability.Activates * percTimeInDPSAndO20 - CL.NumActivatesO20;
                    float hsRageLimitedActs = (availRageO20 / (HS.Ability.RageCost * RageModTotal * RageModBattleTrance)) * percTimeInDPSAndO20
                        - CL.NumActivatesO20;
                    HS.NumActivatesO20 = Math.Min(hsRageLimitedActs, acts);
                    availRageO20 -= HS.RageO20 * RageModTotal * RageModBattleTrance;
                }

 
                (HS.Ability as HeroicStrike).InciteBonusCrits(HS.NumActivatesO20);
                (IR.Ability as InnerRage).FreeRageO20 = repassAvailRageO20 = availRageO20;
                // check for not enough rage to maintain rotation
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeededO20 = 0f, rageGenOtherO20 = 0f;
            foreach (AbilityWrapper aw in TheAbilityList) {
                DPS_TTL += aw.DPS_O20;
                _HPS_TTL += aw.HPS_O20;
                if (aw.RageO20 > 0)
                    rageNeededO20 += aw.RageO20;
                else
                    rageGenOtherO20 -= aw.RageO20;
            }

            DPS_TTL += DPSWarrChar.Whiteattacks.GetMHdps(DPSWarrChar.Whiteattacks.MHActivatesO20, TimeOver20Perc) * percTimeInDPS;
            DPS_TTL += DPSWarrChar.Whiteattacks.GetOHdps(DPSWarrChar.Whiteattacks.OHActivatesO20, TimeOver20Perc) * percTimeInDPS;

            return DPS_TTL;
        }

        /// <summary>
        /// Copied this function from Arms, going to try and rewrite it so it works for Fury
        /// </summary>
        public float SettleAllU20(float totalPercTimeLost, float rageUsedByMaintenance, float percTimeU20, float availRageU20, out float percFailRageU20)
        {
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndU20 = TimeUndr20Perc * percTimeInDPS;
            availRageU20 -= rageUsedByMaintenance * TimeUndr20Perc;
            /* The following are dependant on other attacks as they are proccing abilities or are the fallback item
             * We need to loop these until the activates are relatively unchanged
             * Heroic Strike, when there is rage to do so, handled by the Heroic Strike class
             * After iterating how many Overrides can be done and still do other abilities, then do the white dps
             *
             * Starting Assumptions:
             * No ability ever procs so Execute sucks up all the cooldowns
             * Heroic Strike and Cleave won't be used at all
             * Execute FreeRage is 0, no extra rage available
             * 
             * Hoped Ending Results:
             * All abilities will have proc'd and abilities that can proc from other ones will have their activates settled
             * Execute will get extra rage leftovers if there are any (since you won't use HS/CL <20%)
            */

            float preloopAvailGCDsU20 = GCDsAvailableU20, preloopAvailRageU20 = availRageU20;

            float origavailGCDsU20 = preloopAvailGCDsU20;
            AbilityWrapper CS = GetWrapper<ColossusSmash>();
            AbilityWrapper EX = GetWrapper<Execute>();
            AbilityWrapper RB = GetWrapper<RagingBlow>();
            AbilityWrapper BT = GetWrapper<Bloodthirst>();
            AbilityWrapper BS = GetWrapper<BloodSurge>();
            Execute EX_ability = EX.Ability as Execute;

            EX.NumActivatesU20 = origavailGCDsU20 * percTimeInDPS;
            DPSWarrChar.Whiteattacks.SlamActsOverDurU20 = 0f;
            float origAvailRageU20 = preloopAvailRageU20;
            availRageU20 += DPSWarrChar.Whiteattacks.WhiteRageGenOverDurU20 * percTimeInDPS;
            EX_ability.DumbActivates = EX.NumActivatesU20;
            EX_ability.FreeRage = availRageU20;
            float repassAvailRageU20 = 0f;
            percFailRageU20 = 1f;

            bool rbOverExec = RB.Ability.Validated
                && EX.Ability.Validated
                && (EX.Ability.DamageOnUseOverride / (EX.Ability.GCDTime / LatentGCD))
                    < (RB.Ability.DamageOnUseOverride / (RB.Ability.GCDTime / LatentGCD))
                && DPSWarrChar.CalcOpts.M_ExecuteSpamStage2;

            int Iterator = 0;
            #region <20%

            float oldBTGCDs = 0f;
            float oldRBGCDs = 0f;
            float oldEXGCDs = 0f;
            float oldCSGCDs = 0f;
            while (Iterator < 50 && (false
                     || Math.Abs(CS.NumActivatesU20 - oldCSGCDs) > 0.1f
                     || Math.Abs(EX.NumActivatesU20 - oldEXGCDs) > 0.1f
                     || Math.Abs(RB.NumActivatesU20 - oldRBGCDs) > 0.1f
                     || Math.Abs(BT.NumActivatesU20 - oldBTGCDs) > 0.1f
                     ))
            {
                //oldBTGCDs = BT.NumActivatesU20;
                oldRBGCDs = RB.NumActivatesU20;
                oldCSGCDs = CS.NumActivatesU20;
                oldEXGCDs = EX.NumActivatesU20;
                // Set these all back to 0 so we can start fresh but factor the previous values where needed
                CS.NumActivatesU20 = EX.NumActivatesU20 = BT.NumActivatesU20 = RB.NumActivatesU20 = 0f;
                // Reset the Rage
                availRageU20 = origAvailRageU20;
                // TODO: I'd like to cache whiteRageGenOverDur but it changes with slams. Research a better solution
                availRageU20 += (float)((DPSWarrChar.Whiteattacks.MHActivatesU20 * DPSWarrChar.Whiteattacks.MHAtkTable.AnyLand) *
                                6.5 * DPSWarrChar.CombatFactors.MH.Speed);
                if (DPSWarrChar.CombatFactors.useOH) {
                    availRageU20 += (float)((DPSWarrChar.Whiteattacks.OHActivatesU20 * DPSWarrChar.Whiteattacks.OHAtkTable.AnyLand) *
                                    6.5 * DPSWarrChar.CombatFactors.OH.Speed) / 2f;
                }
                //availRageU20 += DPSWarrChar.Whiteattacks.WhiteRageGenOverDurU20 * percTimeInDPS;

                float acts;
                // GCDsAvailableU20 is an expensive operation, trying to
                // see if I can speed it up by not calling it so much
                float gcdsAvailableU20 = GCDsAvailableU20;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageU20 < 0f || percFailRageU20 != 1f)
                {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    percFailRageU20 *= 1f + repassAvailRageU20 / (availRageU20 - repassAvailRageU20); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    if (percFailRageU20 > 1f) { percFailRageU20 = 1f; }
                }
                else { percFailRageU20 = 1f; }

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.Ability.Validated)
                {
                    acts = Math.Min(gcdsAvailableU20, CS.Ability.Activates * percTimeInDPSAndU20 * percFailRageU20);
                    CS.NumActivatesU20 = acts;
                    availRageU20 -= CS.RageU20 * RageModTotal;
                    gcdsAvailableU20 -= CS.GCDUsageU20;
                }

                if (RB.Ability.Validated && rbOverExec && gcdsAvailableU20 > 0)
                {
                    acts = Math.Min(gcdsAvailableU20, RB.Ability.Activates) * percTimeInDPSAndU20 * percFailRageU20;
                    RB.NumActivatesU20 = acts;// *(1f - WWspace - CSspace - BTspace - BSspace);
                    availRageU20 -= RB.RageU20 * RageModTotal;
                    gcdsAvailableU20 -= RB.GCDUsageU20;
                }
                // Execute for remainder of GCDs
                if (EX.Ability.Validated && gcdsAvailableU20 > 0)
                {
                    acts = gcdsAvailableU20 * percTimeInDPS;
                    EX.NumActivatesU20 = (EX.Ability as Execute).DumbActivates = acts;
                    availRageU20 -= EX.RageU20 * RageModTotal;

                }
                else { EX.NumActivatesU20 = 0f; }

                (EX.Ability as Execute).FreeRage = repassAvailRageU20 = availRageU20; // check for not enough rage to maintain rotation and set Execute's FreeRage to this value
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeededU20 = 0f, rageGenOtherU20 = 0f;
            foreach (AbilityWrapper aw in TheAbilityList)
            {
                DPS_TTL += aw.DPS_U20;
                _HPS_TTL += aw.HPS_U20;
                if (aw.RageU20 > 0) { rageNeededU20 += aw.RageU20; }
                else { rageGenOtherU20 -= aw.RageU20; }
            }

            DPS_TTL += DPSWarrChar.Whiteattacks.GetMHdps(DPSWarrChar.Whiteattacks.MHActivatesU20, TimeUndr20Perc) * percTimeInDPS;
            DPS_TTL += DPSWarrChar.Whiteattacks.GetOHdps(DPSWarrChar.Whiteattacks.OHActivatesU20, TimeUndr20Perc) * percTimeInDPS;

            return DPS_TTL;
        }

        /// <summary>
        /// Copied this function from Arms, going to try and rewrite it so it works for Fury
        /// </summary>
        public void MakeRotationAndDoDPS(bool setCalcs, float percTimeUnder20) {
            if (DPSWarrChar.Char.MainHand == null) { return; }
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
            availRage += RageGenOverDurOther + RageGainedWhileMoving;

            // ==== Maintenance and Anti-Impedence Priorities ====
            if (_needDisplayCalcs) GCDUsage += "Maintenance: Things that you do periodically to Buff yourself or the raid\n";
            float rageUsedByMaintenance = DoMaintenanceActivates(TotalPercTimeLost);

            // ==== Standard Priorities ===============
            if (_needDisplayCalcs) GCDUsage += "Abilities: Things that you do to damage the Target. These are not in order of priority.\n";
            _DPS_TTL_O20 = SettleAll(TotalPercTimeLost, rageUsedByMaintenance, percTimeUnder20, availRage, out PercFailRageOver20);
            if (percTimeUnder20 != 0f) { _DPS_TTL_U20 = SettleAllU20(TotalPercTimeLost, rageUsedByMaintenance, percTimeUnder20, availRage, out PercFailRageUnder20); }
            if (_needDisplayCalcs) {
                // We need to add Inner Rage now that we know how many there are
                AbilityWrapper aw = GetWrapper<InnerRage>();
                GCDUsage = aw.AllNumActivates > 0 ? GCDUsage.Insert(GCDUsage.IndexOf("Abilities") - 2,
                    string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                                aw.AllNumActivates, aw.NumActivatesO20, aw.NumActivatesU20,
                                aw.Ability.Name, (!aw.Ability.UsesGCD ? " (Doesn't use GCDs)" : "")
                                ))
                    : GCDUsage;
            }

            CalcDeepWounds();
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
                List<AbilityWrapper> dmgAbils = DamagingAbilities;
                foreach (AbilityWrapper aw in dmgAbils)
                {
                    if (aw.AllNumActivates > 0 && !aw.Ability.IsMaint && !(aw.Ability is HeroicLeap))
                    {
                        GCDUsage += string.Format("{0:000.000}={1:000.000}+{2:000.000} : {3}{4}\n",
                            aw.AllNumActivates, aw.NumActivatesO20, aw.NumActivatesU20,
                            aw.Ability.Name,
                            aw.Ability.UsesGCD ? "" : " (Doesn't use GCDs)");
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
                this.calcs.WhiteDPSMH = DPSWarrChar.Whiteattacks.GetMHdps(DPSWarrChar.Whiteattacks.MHActivatesO20, TimeOver20Perc);
                this.calcs.WhiteDPSMH_U20 = DPSWarrChar.Whiteattacks.GetMHdps(DPSWarrChar.Whiteattacks.MHActivatesU20, TimeUndr20Perc);
                this.calcs.WhiteDPSOH = DPSWarrChar.Whiteattacks.GetOHdps(DPSWarrChar.Whiteattacks.OHActivatesO20, TimeOver20Perc);
                this.calcs.WhiteDPSOH_U20 = DPSWarrChar.Whiteattacks.GetOHdps(DPSWarrChar.Whiteattacks.OHActivatesU20, TimeUndr20Perc);
                {
                    if (this.calcs.WhiteDPSMH > 0 && this.calcs.WhiteDPSMH_U20 > 0 && this.calcs.WhiteDPSOH > 0 && this.calcs.WhiteDPSOH_U20 > 0) {
                        this.calcs.WhiteDPS = (this.calcs.WhiteDPSMH + this.calcs.WhiteDPSOH) * (1f - percTimeUnder20) + (this.calcs.WhiteDPSMH_U20 + this.calcs.WhiteDPSOH_U20) * (percTimeUnder20);
                    } else if (this.calcs.WhiteDPSMH_U20 > 0 && this.calcs.WhiteDPSOH_U20 > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMH_U20 + this.calcs.WhiteDPSOH_U20;
                    } else if (this.calcs.WhiteDPSMH > 0 && this.calcs.WhiteDPSOH > 0) { this.calcs.WhiteDPS = this.calcs.WhiteDPSMH + this.calcs.WhiteDPSOH;
                    } else { this.calcs.WhiteDPS = 0f; }
                }
                this.calcs.WhiteDmg = (this.DPSWarrChar.Whiteattacks.MHDamageOnUse + this.DPSWarrChar.Whiteattacks.OHDamageOnUse);

                {
                    if (_DPS_TTL_O20 > 0 && _DPS_TTL_U20 > 0) { this.calcs.TotalDPS = _DPS_TTL_O20 * (1f - percTimeUnder20) + _DPS_TTL_U20 * percTimeUnder20; }
                    else if (_DPS_TTL_U20 > 0) { this.calcs.TotalDPS = _DPS_TTL_U20; }
                    else if (_DPS_TTL_O20 > 0) { this.calcs.TotalDPS = _DPS_TTL_O20; }
                    else { this.calcs.TotalDPS = 0f; }
                }

                this.calcs.WhiteRageO20 = DPSWarrChar.Whiteattacks.MHRageGenOverDurO20 + DPSWarrChar.Whiteattacks.OHRageGenOverDurO20;
                this.calcs.OtherRageO20 = this.RageGenOverDurOtherO20;
                this.calcs.NeedyRageO20 = this.RageNeededOverDurO20;
                this.calcs.FreeRageO20 = calcs.WhiteRageO20 + calcs.OtherRageO20 - calcs.NeedyRageO20;

                this.calcs.WhiteRageU20 = DPSWarrChar.Whiteattacks.MHRageGenOverDurU20 + DPSWarrChar.Whiteattacks.OHRageGenOverDurU20;
                this.calcs.OtherRageU20 = this.RageGenOverDurOtherU20;
                this.calcs.NeedyRageU20 = this.RageNeededOverDurU20;
                this.calcs.FreeRageU20 = calcs.WhiteRageU20 + calcs.OtherRageU20 - calcs.NeedyRageU20;
            }
        }

        public override void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations)
        {
            #region Cata Calcs
            try {
                base.MakeRotationandDoDPS(setCalcs, needsDisplayCalculations);
                MakeRotationAndDoDPS(setCalcs, TimeUndr20Perc);
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error in creating Fury Rotation Details",
                    Function = "MakeRotationandDoDPS()",
                    TheException = ex,
                }.Show();
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

            availRage += Whiteattacks.MHRageGenOverDur + Whiteattacks.OHRageGenOverDur;

            /*bool HSok = CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            bool CLok = 
                BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0
                && CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Cleave_];*/

            Whiteattacks.Slam_ActsOverDurO20 = Whiteattacks.Slam_ActsOverDurU20 = 0f;// _SL_GCDs;
 
            DPS_TTL += Whiteattacks.MhDPS * (1f - timeLostPerc) + Whiteattacks.OhDPS * (1f - timeLostPerc);
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
                this.calcs.WhiteDPS = (Whiteattacks.MhDPS + Whiteattacks.OhDPS) * (1f - timeLostPerc);

                this.calcs.WhiteRageO20  = Whiteattacks.MHRageGenOverDur + Whiteattacks.OHRageGenOverDur;
                this.calcs.OtherRageO20 = RageGenOverDur_Other;
                this.calcs.NeedyRageO20 = RageNeededOverDur;
                this.calcs.FreeRageO20 = calcs.WhiteRageO20 + calcs.OtherRageO20 - calcs.NeedyRageO20;
                this.calcs.WhiteDPSMH = this.Whiteattacks.MhDPS;
                this.calcs.WhiteDPSMHU20 = -1f; // Fury doesn't use this
                this.calcs.WhiteDPSOH = this.Whiteattacks.OhDPS;
                this.calcs.WhiteDmg   = this.Whiteattacks.MhDamageOnUse + this.Whiteattacks.OhDamageOnUse;
            }
#endif
            #endregion
        }
    }
}