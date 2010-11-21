/**********
 * Owner: Jothay
 **********/
using System;
using System.Collections.Generic;
using System.Text;
using Rawr.DPSWarr.Skills;

namespace Rawr.DPSWarr {
    public class ArmsRotation : Rotation {
        public ArmsRotation(Character character, Stats stats, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo) {
            Char = character;
            StatS = stats;
            Talents = Char == null || Char.WarriorTalents == null ? new WarriorTalents() : Char.WarriorTalents;
            //TalentsCata = Char == null || Char.WarriorTalentsCata == null ? new WarriorTalentsCata() : Char.WarriorTalentsCata;
            CombatFactors = cf;
            CalcOpts = (co == null ? new CalculationOptionsDPSWarr() : co);
            BossOpts = (bo == null ? new BossOptions() : bo);
            WhiteAtks = wa;

            _cachedLatentGCD = 1.5f + CalcOpts.Latency + CalcOpts.AllowedReact;
            _cachedNumGCDsO20 = FightDuration / LatentGCD * (1f - (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f));
            _cachedNumGCDsU20 = FightDuration / LatentGCD * (CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 0f);
            // Initialize();
        }
        #region Variables
        // Ability Declarations
        //public Skills.FakeWhite FW;
        #endregion
        #region Initialization
        public override void Initialize(CharacterCalculationsDPSWarr calcs) {
            base.Initialize(calcs);
            //calcs.FW = FW;
        }
        #endregion
        #region Rage Calcs
        #endregion
        protected override void calcDeepWounds() { base.calcDeepWounds(); }

        #region Charge
        /// <summary>
        /// This is a 2D array because both the Juggernaught talent and the Glyph of Rapid Charge affect this thing
        /// <para>No Jug,No Glyph | No Jug,Glyph</para>
        /// <para>   Jug,No Glyph |    Jug,Glyph</para>
        /// </summary>
        private static readonly SpecialEffect[][] _SE_ChargeUse = {
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, null, 10, ((15f + 0 * 5f) * (1f - (false ? 0.07f : 0f)))), new SpecialEffect(Trigger.Use, null, 10, ((15f + 0 * 5f) * (1f - (true ? 0.07f : 0f)))) },
            new SpecialEffect[] { new SpecialEffect(Trigger.Use, null, 10, ((15f + 1 * 5f) * (1f - (false ? 0.07f : 0f)))), new SpecialEffect(Trigger.Use, null, 10, ((15f + 1 * 5f) * (1f - (true ? 0.07f : 0f)))) },
        };
        #endregion

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
             * Sudden Death Free Rage is minimum cost, no extra rage available
             * Execute Free Rage is minimum cost, no extra rage available
             * 
             * Hoped Ending Results:
             * All abilities will have proc'd and abilities that can proc from other ones will have their activates settled
             * Heroic Strikes and Cleave will activate when there's enough rage to support them AND Executes
             * Sudden Death will get extra rage leftovers if there are any
             * Execute will get extra rage leftovers if there are any (since you won't use HS/CL <20%)
            */

            float preloopAvailGCDsO20 = GCDsAvailableO20, preloopGCDsUsedO20 = GCDsUsedO20, preloopAvailRageO20 = availRageO20;

            float origNumGCDsO20 = (FightDuration / LatentGCD) * percTimeO20,
                  origavailGCDsO20 = preloopAvailGCDsO20,
                  origGCDsusedO20 = preloopGCDsUsedO20;
            float oldBLSGCDs = 0f, oldMSGCDs = 0f, oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f, /*oldEXGCDs = 0f,*/ oldSLGCDs = 0f,
                  oldCSGCDs = 0f, oldHSGCDs = 0f, oldCLGCDs = 0f, oldTHGCDs = 0f, oldVRGCDs = 0f, oldSoOActs = 0f;

            AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper SoO = GetWrapper<StrikesOfOpportunity>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            //AbilWrapper EX = GetWrapper<Execute>();
            AbilWrapper HS = GetWrapper<HeroicStrike>();
            AbilWrapper CL = GetWrapper<Cleave>();
            AbilWrapper MS = GetWrapper<MortalStrike>();
            AbilWrapper OP = GetWrapper<OverPower>();
            AbilWrapper VR = GetWrapper<VictoryRush>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
            AbilWrapper TH = GetWrapper<ThunderClap>();
            AbilWrapper TB = GetWrapper<TasteForBlood>();

            //Execute EX_ability = EX.ability as Execute;
            
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
                     Math.Abs(BLS.numActivatesO20 - oldBLSGCDs) > 0.1f ||
                     Math.Abs(MS.numActivatesO20 - oldMSGCDs) > 0.1f ||
                     Math.Abs(RD.numActivatesO20 - oldRDGCDs) > 0.1f ||
                     Math.Abs(OP.numActivatesO20 - oldOPGCDs) > 0.1f ||
                     Math.Abs(TB.numActivatesO20 - oldTBGCDs) > 0.1f ||
                     Math.Abs(CS.numActivatesO20 - oldCSGCDs) > 0.1f ||
                     Math.Abs(HS.numActivatesO20 - oldHSGCDs) > 0.1f ||
                     Math.Abs(CL.numActivatesO20 - oldCLGCDs) > 0.1f ||
                     Math.Abs(TH.numActivatesO20 - oldTHGCDs) > 0.1f ||
                     Math.Abs(VR.numActivatesO20 - oldVRGCDs) > 0.1f ||
                     Math.Abs(SL.numActivatesO20 - oldSLGCDs) > 0.1f ||
                     Math.Abs(SoO.numActivatesO20 - oldSoOActs) > 0.1f
                     //|| (percTimeUnder20 > 0 && Math.Abs(EX.numActivatesO20 - oldEXGCDs) > 0.1f)
                     ))
            {
                // Reset a couple of items so we can keep iterating
                //availGCDs = origavailGCDs;
                WhiteAtks.Slam_ActsOverDurO20 = SL.numActivatesO20;
                oldBLSGCDs = BLS.numActivatesO20; oldMSGCDs = MS.numActivatesO20; oldRDGCDs = RD.numActivatesO20; oldOPGCDs = OP.numActivatesO20; oldTBGCDs = TB.numActivatesO20;
                /*oldEXGCDs = EX.numActivatesO20;*/ oldSLGCDs = SL.numActivatesO20;
                oldCSGCDs = CS.numActivatesO20; oldVRGCDs = VR.numActivatesO20; oldHSGCDs = HS.numActivatesO20; oldCLGCDs = CL.numActivatesO20;
                oldSoOActs = SoO.numActivatesO20;
                BLS.numActivatesO20 = MS.numActivatesO20 = RD.numActivatesO20 = OP.numActivatesO20 = TB.numActivatesO20 = CS.numActivatesO20 = //EX.numActivatesO20 =
                    SL.numActivatesO20 = TH.numActivatesO20 = VR.numActivatesO20 = HS.numActivatesO20 = CL.numActivatesO20 = 0;
                availRageO20 = origAvailRageO20;
                availRageO20 += WhiteAtks.whiteRageGenOverDur * percTimeInDPSAndO20;

                float acts;
                float RDspace, BLSspace, MSspace, TFBspace, OPspace, CSspace, SLspace, HSspace, CLspace, THspace, VRspace;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageO20 < 0f || PercFailRageO20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRageO20 *= 1f + repassAvailRageO20 / (availRageO20 - repassAvailRageO20); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    if (PercFailRageO20 > 1f) { PercFailRageO20 = 1f; }
                } else { PercFailRageO20 = 1f; }
                
                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated) {
                    acts = Math.Min(GCDsAvailableO20, (CS.ability as ColossusSmash).GetActivates(LandedAtksOverDurO20, percTimeO20) * percTimeInDPS * PercFailRageO20);
                    CS.numActivatesO20 = acts;
                    availRageO20 -= CS.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                }
                CSspace = CS.numActivatesO20 / NumGCDsO20 * CS.ability.UseTime / LatentGCD;

                // Rend
                if (RD.ability.Validated) {
                    acts = Math.Min(GCDsAvailableO20, RD.ability.Activates * percTimeInDPS * PercFailRageO20 * (Talents.BloodAndThunder < 2 ? percTimeO20 : 1f));
                    RD.numActivatesO20 = acts;
                    availRageO20 -= RD.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                }
                RDspace = RD.numActivatesO20 / NumGCDsO20 * RD.ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.ability.Validated) {
                    acts = Math.Min(GCDsAvailableO20, TH.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    TH.numActivatesO20 = acts * (1f - RDspace);
                    (RD.ability as Rend).ThunderAppsO20 = TH.numActivatesO20 * Talents.BloodAndThunder * 0.50f;
                    availRageO20 -= TH.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                }
                THspace = TH.numActivatesO20 / NumGCDsO20 * TH.ability.UseTime / LatentGCD;

                // Bladestorm
                if (BLS.ability.Validated) {
                    acts = Math.Min(GCDsAvailableO20, BLS.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    BLS.numActivatesO20 = acts * (1f - RDspace);
                    availRageO20 -= BLS.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                }
                BLSspace = BLS.numActivatesO20 / NumGCDsO20 * BLS.ability.UseTime / LatentGCD;

                // Mortal Strike
                if (MS.ability.Validated) {
                    acts = Math.Min(GCDsAvailableO20, MS.ability.Activates * percTimeInDPSAndO20 * PercFailRageO20);
                    MS.numActivatesO20 = acts * (1f - BLSspace);
                    availRageO20 -= MS.RageO20 * RageMOD_Total;
                }
                MSspace = MS.numActivatesO20 / NumGCDsO20 * MS.ability.UseTime / LatentGCD;

                // Taste for Blood
                if (TB.ability.Validated) {
                    acts = Math.Min(GCDsAvailableO20, (TB.ability.Activates) * percTimeInDPSAndO20 * PercFailRageO20);
                    TB.numActivatesO20 = acts * (1f - BLSspace);
                    availRageO20 -= TB.RageO20 * RageMOD_Total;
                }
                TFBspace = TB.numActivatesO20 / NumGCDsO20 * TB.ability.UseTime / LatentGCD;

                // Overpower
                if (OP.ability.Validated) {
                    acts = Math.Min(GCDsAvailableO20, (OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesO20) * percTimeInDPSAndO20 * PercFailRageO20);
                    OP.numActivatesO20 = acts * (1f - TFBspace - RDspace - BLSspace - MSspace);
                    availRageO20 -= OP.RageO20 * RageMOD_Total;
                }
                OPspace = OP.numActivatesO20 / NumGCDsO20 * OP.ability.UseTime / LatentGCD;

                // Victory Rush
                if (VR.ability.Validated) {
                    // If Slam does more damage and we aren't failing at rage, then we ignore Victory Rush
                    if ((SL.ability.Validated && VR.ability.DamageOnUse > SL.ability.DamageOnUse)
                        || (HS.ability.Validated && PercFailRageO20 == 1f && VR.ability.DamageOnUse > SL.ability.DamageOnUse))
                    {
                        acts = Math.Min(GCDsAvailableO20, VR.ability.Activates * percTimeInDPSAndO20); // Since VR is Free, we don't reduc for Rage Fails
                        VR.numActivatesO20 = acts * (1f - BLSspace);
                        //availRage -= VR.Rage; // it's free
                    }
                }
                VRspace = VR.numActivatesO20 / NumGCDsO20 * VR.ability.UseTime / LatentGCD;
               
                /* Heroic Strike/Cleave now that they are on GCDs.
                 * These should be rage dumps and will replace Slam in the rotation when used
                 * Computing them together as you use HS for single, CL for Multiple */

                // Heroic Strikes/Cleaves
                if (PercFailRageO20 == 1f && (HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailableO20, HS.ability.Activates * percTimeInDPSAndO20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRageO20 / CL.ability.RageCost * (MultTargsPerc);
                    float hsActs = availRageO20 / HS.ability.RageCost * (1f - MultTargsPerc);
                    CL.numActivatesO20 = Math.Min(clActs, acts * (MultTargsPerc));
                    HS.numActivatesO20 = Math.Min(hsActs, acts * (1f - MultTargsPerc));
                    availRageO20 -= (HS.RageO20 + CL.RageO20) * RageMOD_Total * RageMOD_BattleTrance;
                } else if (PercFailRageO20 == 1f && (HS.ability.Validated && !CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailableO20, HS.ability.Activates * percTimeInDPSAndO20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float hsActs = availRageO20 / HS.ability.RageCost * RageMOD_BattleTrance;
                    CL.numActivatesO20 = 0f;
                    HS.numActivatesO20 = Math.Min(hsActs, acts);
                    availRageO20 -= HS.RageO20 * RageMOD_Total;
                } else if (PercFailRageO20 == 1f && (!HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailableO20, CL.ability.Activates * percTimeInDPSAndO20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRageO20 / CL.ability.RageCost * MultTargsPerc;
                    CL.numActivatesO20 = Math.Min(clActs, acts * MultTargsPerc);
                    HS.numActivatesO20 = 0f;
                    availRageO20 -= CL.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                } else { CL.numActivatesO20 = HS.numActivatesO20 = 0f; }

                // Slam
                if (SL.ability.Validated && PercFailRageO20 != 1) {
                    acts = Math.Min(GCDsAvailableO20, GCDsAvailableO20/*SL.Activates*/ * percTimeInDPS);
                    if (SL.ability.GetRageUseOverDur(acts) > availRageO20) acts = Math.Max(0f, availRageO20) / SL.ability.RageCost;
                    SL.numActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                } else if (SL.ability.Validated) {
                    acts = Math.Min(GCDsAvailableO20, GCDsAvailableO20/*SL.Activates*/ * percTimeInDPS);
                    SL.numActivatesO20 = acts;
                    availRageO20 -= SL.RageO20 * RageMOD_Total * RageMOD_BattleTrance;
                } else { SL.numActivatesO20 = 0f; }

                HSspace = HS.numActivatesO20 / NumGCDsO20 * HS.ability.UseTime / LatentGCD;
                CLspace = CL.numActivatesO20 / NumGCDsO20 * CL.ability.UseTime / LatentGCD;
                SLspace = SL.numActivatesO20 / NumGCDsO20 * SL.ability.UseTime / LatentGCD;
                (HS.ability as HeroicStrike).InciteBonusCrits(HS.numActivatesO20);
                WhiteAtks.Slam_ActsOverDurO20 = SL.numActivatesO20;

                // Strikes of Opportunity Procs
                if (SoO.ability.Validated) {
                    SoO.numActivatesO20 = (SoO.ability as StrikesOfOpportunity).GetActivates(AttemptedAtksOverDurO20, percTimeO20);
                    //availRage -= SoO.Rage; // Not sure if it should affect Rage
                }

                float TotalSpace = (RDspace + THspace + BLSspace + MSspace + OPspace + TFBspace + CSspace + SLspace + HSspace + CLspace + VRspace);
                repassAvailRageO20 = availRageO20; // check for not enough rage to maintain rotation
                InvalidateCache();
                Iterator++;
            }
            #endregion

            // Need to add the special effect from Juggernaut to Mortal Strike, not caring about Slam right now
            if (Talents.Juggernaut > 0 && GetWrapper<Charge>().numActivatesO20 > 0) {
                float uptime = _SE_ChargeUse[Talents.Juggernaut][Talents.GlyphOfRapidCharge?1:0].GetAverageUptime(FightDuration / GetWrapper<Charge>().numActivatesO20, 1f, CombatFactors._c_mhItemSpeed, FightDuration);
                // I'm not sure if this is gonna work, but hell, who knows
                (MS.ability as MortalStrike).JuggernautBonusCritChance = 0.25f * uptime;
                //MS = new Skills.MortalStrike(Char, stats, CombatFactors, WhiteAtks, CalcOpts);
            }

            float DPS_TTL = 0f;
            float rageNeededO20 = 0f, rageGenOtherO20 = 0f;
            foreach (AbilWrapper aw in GetAbilityList()) {
                if (aw.ability is Rend) {
                    DPS_TTL += (aw.ability as Rend).GetDPS(aw.numActivatesO20, TH.numActivatesO20, percTimeO20);
                } else {
                    DPS_TTL += aw.DPSO20;
                }
                _HPS_TTL += aw.HPSO20;
                if (aw.RageO20 > 0) { rageNeededO20 += aw.RageO20; }
                else { rageGenOtherO20 -= aw.RageO20; }
            }

            DPS_TTL += WhiteAtks.MhDPS * percTimeInDPSAndO20;

            return DPS_TTL;
        }

        public float SettleAll_U20(float totalPercTimeLost, float rageUsedByMaintenance, float percTimeU20, float availRageU20, out float PercFailRageU20)
        {
            float percTimeO20 = (1f - percTimeU20);
            float percTimeInDPS = (1f - totalPercTimeLost);
            float percTimeInDPSAndU20 = percTimeU20 * percTimeInDPS;
            availRageU20 -= rageUsedByMaintenance * percTimeU20;
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

            float preloopAvailGCDsU20 = GCDsAvailableU20, preloopGCDsUsedU20 = GCDsUsedU20, preloopAvailRageU20 = availRageU20;

            float origNumGCDsU20 = (FightDuration / LatentGCD) * percTimeU20,
                  origavailGCDsU20 = preloopAvailGCDsU20,
                  origGCDsusedU20 = preloopGCDsUsedU20;
            float oldBLSGCDs = 0f, //oldMSGCDs = 0f,
                  oldRDGCDs = 0f, oldOPGCDs = 0f, oldTBGCDs = 0f,
                  oldEXGCDs = 0f, //oldSLGCDs = 0f,
                  oldCSGCDs = 0f, /*oldHSGCDs = 0f,*/ oldTHGCDs = 0f, /*oldVRGCDs = 0f,*/ oldSoOActs = 0f/*, oldCLGCDs = 0f*/;

            //AbilWrapper SL = GetWrapper<Slam>();
            AbilWrapper SoO = GetWrapper<StrikesOfOpportunity>();
            AbilWrapper CS = GetWrapper<ColossusSmash>();
            AbilWrapper EX = GetWrapper<Execute>();
            //AbilWrapper HS = GetWrapper<HeroicStrike>();
            //AbilWrapper CL = GetWrapper<Cleave>();
            //AbilWrapper MS = GetWrapper<MortalStrike>();
            AbilWrapper OP = GetWrapper<OverPower>();
            //AbilWrapper VR = GetWrapper<VictoryRush>();
            AbilWrapper BLS = GetWrapper<Bladestorm>();
            AbilWrapper RD = GetWrapper<Rend>();
            AbilWrapper TH = GetWrapper<ThunderClap>();
            AbilWrapper TB = GetWrapper<TasteForBlood>();

            Execute EX_ability = EX.ability as Execute;

            EX.numActivatesU20 = origavailGCDsU20;
            WhiteAtks.Slam_ActsOverDurU20 = 0f;
            float origAvailRageU20 = preloopAvailRageU20;
            availRageU20 += WhiteAtks.whiteRageGenOverDur * percTimeInDPSAndU20;
            availRageU20 -= EX.RageU20;
            availRageU20 += EX.numActivatesU20 * 5 * Talents.SuddenDeath; // adds back [5|10] rage
            float repassAvailRageU20 = 0f;
            PercFailRageU20 = 1f;

            /*
             * There's two major lines of reasoning here
             * - If Execute does more damage, use it instead of suchNsuch ability
             * - If the ability does more damage, use it instead (so that we can gradually shift
             *   from one rotation to the other based on rising gear levels)
             * - However, most of these abilities are only coming up so that we can do Taste for Blood.
             *   If an Overpower isn't doing as much damage as an Execute... there's no point to Rend,
             *   Thunderclap (refreshing rend), or TfB so we might as well turn them all *off* at once
             */

            bool WeWantBLS = BLS.ability.Validated
                && EX.ability.Validated
                && ((EX.ability.DamageOnUseOverride * EX.ability.AvgTargets) / (EX.ability.GCDTime / LatentGCD))
                    < ((BLS.ability.DamageOnUseOverride * BLS.ability.AvgTargets) / (BLS.ability.GCDTime / LatentGCD))
                && CalcOpts.M_ExecuteSpamStage2;

            bool WeWantTfB = TB.ability.Validated
                && EX.ability.Validated
                && ((EX.ability.DamageOnUseOverride * EX.ability.AvgTargets) / (EX.ability.GCDTime / LatentGCD))
                    < ((TB.ability.DamageOnUseOverride * TB.ability.AvgTargets) / (TB.ability.GCDTime / LatentGCD))
                && CalcOpts.M_ExecuteSpamStage2;

            int Iterator = 0;
            #region <20%
            while (
                    Iterator < 50 && (
                     Math.Abs(BLS.numActivatesU20 - oldBLSGCDs) > 0.1f ||
                     //Math.Abs(MS.numActivatesU20 - oldMSGCDs) > 0.1f ||
                     Math.Abs(RD.numActivatesU20 - oldRDGCDs) > 0.1f ||
                     Math.Abs(OP.numActivatesU20 - oldOPGCDs) > 0.1f ||
                     Math.Abs(TB.numActivatesU20 - oldTBGCDs) > 0.1f ||
                     Math.Abs(CS.numActivatesU20 - oldCSGCDs) > 0.1f ||
                     //Math.Abs(HS.numActivatesU20 - oldHSGCDs) > 0.1f ||
                     //Math.Abs(CL.numActivatesU20 - oldCLGCDs) > 0.1f ||
                     Math.Abs(TH.numActivatesU20 - oldTHGCDs) > 0.1f ||
                     //Math.Abs(VR.numActivatesU20 - oldVRGCDs) > 0.1f ||
                     //Math.Abs(SL.numActivatesU20 - oldSLCDs) > 0.1f ||
                     Math.Abs(SoO.numActivatesU20 - oldSoOActs) > 0.1f ||
                     (percTimeU20 > 0 && Math.Abs(EX.numActivatesU20 - oldEXGCDs) > 0.1f)))
            {
                // Reset a couple of items so we can keep iterating
                //availGCDs = origavailGCDs;
                oldBLSGCDs = BLS.numActivatesU20; /*oldMSGCDs = MS.numActivatesU20;*/ oldRDGCDs = RD.numActivatesU20; oldOPGCDs = OP.numActivatesU20; oldTBGCDs = TB.numActivatesU20;
                oldEXGCDs = EX.numActivatesU20; /*oldSLGCDs = SL.numActivatesU20;*/
                oldCSGCDs = CS.numActivatesU20;
                BLS.numActivatesU20 = /*MS.numActivatesU20 =*/ RD.numActivatesU20 = OP.numActivatesU20 = TB.numActivatesU20 = CS.numActivatesU20 =
                    EX.numActivatesU20 = /*SL.numActivatesU20 = HS.numActivatesU20 = CL.numActivatesU20 = VR.numActivatesU20 =*/ TH.numActivatesU20 = 0;
                availRageU20 = origAvailRageU20;
                availRageU20 += WhiteAtks.whiteRageGenOverDur * percTimeInDPSAndU20;

                float acts;
                float CSspace, RDspace, BLSspace, /*MSspace,*/ TFBspace, OPspace, EXspace/*, SLspace, HSspace, CLspace*/, THspace/*, VRspace*/;

                // ==== Primary Ability Priorities ====

                // Reduc abilities due to lack of Rage for maintaining the rotation
                if (repassAvailRageU20 < 0f || PercFailRageU20 != 1f) {
                    // total the amount of rage you really need and turn it into a percentage that we failed
                    PercFailRageU20 *= 1f + repassAvailRageU20 / (availRageU20 - repassAvailRageU20); // if repassAvailRage was -100 and availRage was 900, then this becomes 1 + (-100 / 900 - (-100)) = 1 - 100/1000 = 90%
                    if (PercFailRageU20 > 1f) { PercFailRageU20 = 1f; }
                } else { PercFailRageU20 = 1f; }

                // Colossus Smash, Highest Ability Prio because it gives 100% ArP when used
                if (CS.ability.Validated) {
                    acts = Math.Min(GCDsAvailableU20, (CS.ability as ColossusSmash).GetActivates(LandedAtksOverDurU20, percTimeU20) * percTimeInDPS * PercFailRageU20);
                    CS.numActivatesU20 = acts;
                    availRageU20 -= CS.RageU20 * RageMOD_Total;
                }
                CSspace = CS.numActivatesU20 / NumGCDsU20 * CS.ability.UseTime / LatentGCD;

                // Rend
                if (RD.ability.Validated && WeWantTfB && Talents.BloodAndThunder < 2) { // Ignore Rend when we have BnT at 100%
                    acts = Math.Min(GCDsAvailableU20, RD.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    RD.numActivatesU20 = acts;
                    availRageU20 -= RD.RageU20 * RageMOD_Total;
                }
                RDspace = RD.numActivatesU20 / NumGCDsU20 * RD.ability.UseTime / LatentGCD;

                // Thunder Clap
                if (TH.ability.Validated && WeWantTfB) {
                    acts = Math.Min(GCDsAvailableU20, TH.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    TH.numActivatesU20 = acts * (1f - RDspace);
                    (RD.ability as Rend).ThunderAppsU20 = TH.numActivatesU20 * Talents.BloodAndThunder * 0.50f;
                    availRageU20 -= TH.RageU20 * RageMOD_Total;
                }
                THspace = TH.numActivatesU20 / NumGCDsU20 * TH.ability.UseTime / LatentGCD;

                // Bladestorm
                if (WeWantBLS) {
                    // We only want to use Bladestorm during Exec phase IF it is going to do more damage, which requires Multiple Targets to be up
                    acts = Math.Min(GCDsAvailableU20, BLS.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    BLS.numActivatesU20 = acts * (1f - RDspace);
                    availRageU20 -= BLS.RageU20 * RageMOD_Total;
                }
                BLSspace = BLS.numActivatesU20 / NumGCDsU20 * BLS.ability.UseTime / LatentGCD;

                /*// Mortal Strike // MS doesn't get used in Exec phase
                if (MS.ability.Validated) {
                    acts = Math.Min(GCDsAvailableU20, MS.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    MS.numActivatesU20 = acts * (1f - BLSspace);
                    availRageU20 -= MS.RageU20 * RageMOD_Total;
                }
                MSspace = MS.numActivatesU20 / NumGCDsU20 * MS.ability.UseTime / LatentGCD;*/

                // Taste for Blood
                if (WeWantTfB) {
                    acts = Math.Min(GCDsAvailableU20, TB.ability.Activates * percTimeInDPSAndU20 * PercFailRageU20);
                    TB.numActivatesU20 = acts * (1f - BLSspace);
                    availRageU20 -= TB.RageU20 * RageMOD_Total;
                }
                TFBspace = TB.numActivatesU20 / NumGCDsU20 * TB.ability.UseTime / LatentGCD;

                // Overpower
                if (OP.ability.Validated && WeWantTfB) { // same check, no need to make it twice
                    acts = Math.Min(GCDsAvailableU20, (OP.ability as OverPower).GetActivates(DodgedAttacksOverDur, SoO.numActivatesU20) * percTimeInDPSAndU20 * PercFailRageU20);
                    OP.numActivatesU20 = acts * (1f - TFBspace - RDspace - BLSspace /*- MSspace*/);
                    availRageU20 -= OP.RageU20 * RageMOD_Total;
                }
                OPspace = OP.numActivatesU20 / NumGCDsU20 * OP.ability.UseTime / LatentGCD;

                // Skip Victory Rush, it's useless here

                // Heroic Strikes/Cleaves
                // I dont think we should HS/CL during this phase because Exec should do more damage for less/same Rage
                /*if (PercFailRageUnder20 == 1f && (HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, HS.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRage / CL.ability.RageCost * (MultTargsPerc);
                    float hsActs = availRage / HS.ability.RageCost * (1f - MultTargsPerc);
                    CL.numActivatesU20 = Math.Min(clActs, acts * (MultTargsPerc));
                    HS.numActivatesU20 = Math.Min(hsActs, acts * (1f - MultTargsPerc));
                    availRage -= (HS.Rage + CL.Rage) * RageMOD_Total;
                } else if (PercFailRageUnder20 == 1f && (HS.ability.Validated && !CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, HS.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float hsActs = availRage / HS.ability.RageCost;
                    CL.numActivatesU20 = 0f;
                    HS.numActivatesU20 = Math.Min(hsActs, acts);
                    availRage -= HS.Rage * RageMOD_Total;
                } else if (PercFailRageUnder20 == 1f && (!HS.ability.Validated && CL.ability.Validated)) {
                    acts = Math.Min(GCDsAvailable, CL.ability.Activates * percTimeInDPSAndUnder20);
                    float MultTargsPerc = BossOpts.MultiTargsTime / FightDuration;
                    // We are trying to limit this cause to whatever rage is remaining and
                    // not go overboard to make this thing think we are PercFailRaging
                    float clActs = availRage / CL.ability.RageCost * MultTargsPerc;
                    CL.numActivatesU20 = Math.Min(clActs, acts * MultTargsPerc);
                    HS.numActivatesU20 = 0f;
                    availRage -= CL.Rage * RageMOD_Total;
                } else { VR.numActivatesU20 = SL.numActivatesU20 = CL.numActivatesU20 = HS.numActivatesU20 = 0f; } */

                // Execute for remainder of GCDs
                if (EX.ability.Validated /*&& PercFailRageUnder20 == 1f*/) {
                    acts = Math.Min(GCDsAvailableU20, GCDsAvailableU20 * percTimeInDPS);
                    //if (EX.ability.GetRageUseOverDur(acts) > availRage) acts = Math.Max(0f, availRage) / EX.ability.RageCost;
                    EX.numActivatesU20 = acts;
                    (EX.ability as Execute).DumbActivates = EX.numActivatesU20;
                    availRageU20 -= EX.RageU20 * RageMOD_Total;
                    availRageU20 += EX.numActivatesU20 * (Talents.SuddenDeath * 5f);
                } else { EX.numActivatesU20 = 0f; }
                EXspace = EX.numActivatesU20 / NumGCDsU20 * EX.ability.UseTime / LatentGCD;

                // Strikes of Opportunity Procs
                if (SoO.ability.Validated) {
                    SoO.numActivatesU20 = (SoO.ability as StrikesOfOpportunity).GetActivates(AttemptedAtksOverDurU20, percTimeU20);
                    //availRage -= SoO.RageU20; // Not sure if it should affect Rage
                }

                float TotalSpace = (CSspace + RDspace + THspace + BLSspace /*+ MSspace*/ + OPspace + TFBspace /*+ SLspace*/ + EXspace /*+ HSspace + CLspace*/);
                (EX.ability as Execute).FreeRage = repassAvailRageU20 = availRageU20; // check for not enough rage to maintain rotation and set Execute's FreeRage to this value
                InvalidateCache();
                Iterator++;
            }
            #endregion

            float DPS_TTL = 0f;
            float rageNeededU20 = 0f, rageGenOtherU20 = 0f;
            foreach (AbilWrapper aw in GetAbilityList()) {
                if (aw.ability is Rend) {
                    DPS_TTL += (aw.ability as Rend).GetDPS(aw.numActivatesU20, TH.numActivatesU20, percTimeU20);
                } else {
                    DPS_TTL += aw.DPSU20;
                }
                _HPS_TTL += aw.HPSU20;
                if (aw.RageU20 > 0) { rageNeededU20 += aw.RageU20; }
                else { rageGenOtherU20 -= aw.RageU20; }
            }

            DPS_TTL += WhiteAtks.MhDPS * percTimeInDPSAndU20;
            
            return DPS_TTL;
        }

        public void MakeRotationandDoDPS(bool setCalcs, float percTimeUnder20) {
            if (Char.MainHand == null) { return; }
            _HPS_TTL = 0f;
            if (_needDisplayCalcs) GCDUsage += string.Format("All=Over20%+Under20%. Only applicable if using Exec Spam\n{0:000.000}={1:000.000}+{2:000.000} : Total GCDs\n\n", NumGCDsAll, NumGCDsO20, NumGCDsU20);

            // ==== Impedences ========================
            if (_needDisplayCalcs) GCDUsage += "Impedences: Things that prevent you from DPS'g\n";
            float TotalPercTimeLost = CalculateTimeLost();
            if (_needDisplayCalcs && TotalPercTimeLost <= 0f) GCDUsage += "None\n\n";
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

            calcDeepWounds();
            _DPS_TTL_O20 += DW.TickSize;

            // Add each of the abilities' DPS and HPS values and other aesthetics
            if (_needDisplayCalcs) {
                if (PercFailRageOver20 != 1.0f || PercFailRageUnder20 != 1.0f) {
                    GCDUsage += (PercFailRageOver20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due\nto Rage Starvation before Execute Spam.\n", (1f - PercFailRageOver20)) : "");
                    GCDUsage += (PercFailRageUnder20 < 1.0f ? string.Format("WARNING! You are losing {0:0.0%} of your abilities due\nto Rage Starvation during Execute Spam.\n", (1f - PercFailRageUnder20)) : "");
                    GCDUsage += "\n";
                }
                List<AbilWrapper> dmgAbils = GetDamagingAbilities();
                foreach (AbilWrapper aw in dmgAbils) {
                    if (aw.allNumActivates > 0 && !aw.ability.isMaint) {
                        if (aw.ability.GCDTime < 1.5f || aw.ability.GCDTime > 3f) {
                            // Overpower (And TfB procs) use less than a GCD to recouperate.
                            // Bladestorm is channelled over 6 seconds (approx 4 GCDs)
                            GCDUsage += string.Format("{0:000.000}=({1:000.000}+{2:000.000})@{3:0.000}s={4:000.000} : {5}\n",
                                aw.allNumActivates, aw.numActivatesO20, aw.numActivatesU20,
                                aw.ability.GCDTime,
                                (aw.allNumActivates * aw.ability.GCDTime / (CalcOpts.FullLatency + 1.5f)),
                                aw.ability.Name
                            );
                        } else {
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
            if (setCalcs) {
                this.calcs.WhiteDPS = WhiteAtks.MhDPS;
                this.calcs.WhiteDPSMH = WhiteAtks.MhDPS;
                this.calcs.WhiteDmg = this.WhiteAtks.MhDamageOnUse;

                this.calcs.TotalDPS = _DPS_TTL_O20 + _DPS_TTL_U20;

                this.calcs.WhiteRageO20 = WhiteAtks.MHRageGenOverDurO20;
                this.calcs.OtherRageO20 = this.RageGenOverDur_OtherO20;
                this.calcs.NeedyRageO20 = this.RageNeededOverDurO20;
                this.calcs.FreeRageO20 = calcs.WhiteRageO20 + calcs.OtherRageO20 - calcs.NeedyRageO20;

                this.calcs.WhiteRageU20 = WhiteAtks.MHRageGenOverDurU20;
                this.calcs.OtherRageU20 = this.RageGenOverDur_OtherU20;
                this.calcs.NeedyRageU20 = this.RageNeededOverDurU20;
                this.calcs.FreeRageU20 = calcs.WhiteRageU20 + calcs.OtherRageU20 - calcs.NeedyRageU20;
            }
        }

        public override void MakeRotationandDoDPS(bool setCalcs, bool needsDisplayCalculations) {
            try {
                base.MakeRotationandDoDPS(setCalcs, needsDisplayCalculations);
                float PercTimeUnder20 = 0f;
                if (CalcOpts.M_ExecuteSpam) { PercTimeUnder20 = (float)BossOpts.Under20Perc; }
                MakeRotationandDoDPS(setCalcs, PercTimeUnder20);
            } catch (Exception ex) {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error in creating Arms Rotation Details",
                    ex.Message, ex.InnerException,
                    "MakeRotationandDoDPS()", "No Additional Info", ex.StackTrace);
                eb.Show();
            }
        }
    }
}
