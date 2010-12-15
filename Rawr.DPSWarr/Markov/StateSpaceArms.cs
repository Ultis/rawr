using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.DPSWarr.Markov
{
    public class ArmsGenerator : StateSpaceGenerator<Skills.Ability>
    {
        public ArmsGenerator(Character c, Stats s, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo) {
            Char = c; Talents = c.WarriorTalents; StatS = s; combatFactors = cf; WhiteAtks = wa; CalcOpts = co; BossOpts = bo;
            //
            Rot = new ArmsRotation(c, s, cf, wa, co, bo);
            Rot.Initialize();
            LatentGCD = 1.5 + co.FullLatency;
        }

        #region Variables
        protected double LatentGCD;
        public ArmsRotation Rot = null;
        Character Char;
        WarriorTalents Talents;
        Stats StatS;
        CombatFactors combatFactors;
        Skills.WhiteAttacks WhiteAtks;
        CalculationOptionsDPSWarr CalcOpts;
        BossOptions BossOpts;
        #endregion

        public class StateArms : State<Skills.Ability>
        {
            /// <summary>The Current Rage we have for abilities</summary>
            public double Current_Rage;
            public double CDRem_BattleShout;
            public double TimeTilNext_RendTickProcgTfB; public bool HaveBuff_OPTfB;
            public double TimeSinceLast_CS; public bool HaveBuff_SD;
            public double TimeSinceLast_RD; public bool HaveBuff_RD;
            public double TimeSinceLast_IR; public bool HaveBuff_IR;
            public double CDRem_MS; public bool AbilRdy_MS;
            public bool ThereAreMultipleMobs;
        }

        protected override State<Skills.Ability> GetInitialState()
        {
            // Lets assume that they started the fight with a Charge to have to rage to work with
            StateArms newState = GetState(
                Rot.GetWrapper<Skills.Charge>().ability.RageCost * -1, // Starting Rage
                0, // It has been <never> since the last Battle Shout, so we're setting it to the inverse Cd limit here just to make sure it can be used the first chance we get
                20, // We don't know how long it will be until the next TfB proc because we don't have Rend up yet
                Rot.GetWrapper<Skills.ColossusSmash>().ability.Cd, // It has been <never> since the last Colossus Smash, so we're setting it to the Cd limit here just to make sure it can be used the first chance we get
                false, // We don't have the TfB or OP buff yet, because we haven't done anything that would proc it/them
                false, // We don't have the Sudden Death buff because we haven't done anything that would proc it
                Rot.GetWrapper<Skills.Rend>().ability.Duration, // It has been <never> since the last Rend, so we're setting it to the Duration limit here just to make sure it can be used the first chance we get
                false, // The Rend buff is not up yet
                0, // Mortal Strike is ready and waiting
                true, // Mortal Strike is ready and waiting
                false, // We are not handling this yet (Multiple Mobs)
                Rot.GetWrapper<Skills.InnerRage>().ability.Cd, // It has been <never> since the last Inner Raged, so we're setting it to the Cd limit here just to make sure it can be used the first chance we get
                false
                );
            System.Diagnostics.Debug.WriteLine("{0}, {13}, {2}, {12}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {14:00.00}, {15}, {1}",
                "Mov#",
                "Action",
                "Rage",
                "NTfB",
                "LsCS",
                "HvTB",
                "HvSD",
                "LsRD",
                "HvRD",
                "MSCDr",
                "MSRdy",
                "MulM",
                "BSCDr",
                "Time",
                "LsIR",
                "HvIR"
                );
            System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                MoveCounter,
                "Intial State (we charged to start combat)",
                newState.Current_Rage, 
                newState.TimeTilNext_RendTickProcgTfB,
                newState.TimeSinceLast_CS,
                newState.HaveBuff_OPTfB,
                newState.HaveBuff_SD,
                newState.TimeSinceLast_RD,
                newState.HaveBuff_RD,
                newState.CDRem_MS,
                newState.AbilRdy_MS,
                newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                ));
            return newState;
        }

        private double WhiteRageForAGCD { get { return (Rot.WhiteAtks.whiteRageGenOverDur * LatentGCD) / BossOpts.BerserkTimer; } }

        public int MoveCounter = 0;
        public float totalTimePassed = 0;
        protected override List<StateTransition<Skills.Ability>> GetStateTransitions(State<Skills.Ability> state)
        {
            StateArms s = state as StateArms;
            StateArms newState;
            List<StateTransition<Skills.Ability>> list = new List<StateTransition<Skills.Ability>>();

            Rotation.AbilWrapper CS = Rot.GetWrapper<Skills.ColossusSmash>();
            Rotation.AbilWrapper MS = Rot.GetWrapper<Skills.MortalStrike>();
            Rotation.AbilWrapper OP = Rot.GetWrapper<Skills.OverPower>();
            Rotation.AbilWrapper TB = Rot.GetWrapper<Skills.TasteForBlood>();
            Rotation.AbilWrapper SL = Rot.GetWrapper<Skills.Slam>();
            //Rotation.AbilWrapper EX = Rot.GetWrapper<Skills.Execute>();
            //Rotation.AbilWrapper HS = Rot.GetWrapper<Skills.HeroicStrike>();
            //Rotation.AbilWrapper CL = Rot.GetWrapper<Skills.Cleave>();
            Rotation.AbilWrapper RD = Rot.GetWrapper<Skills.Rend>();
            //Rotation.AbilWrapper TH = Rot.GetWrapper<Skills.ThunderClap>();
            Rotation.AbilWrapper IR = Rot.GetWrapper<Skills.InnerRage>();
            Rotation.AbilWrapper BS = Rot.GetWrapper<Skills.BattleShout>();
            //Rotation.AbilWrapper DC = Rot.GetWrapper<Skills.DeadlyCalm>();
            //Rotation.AbilWrapper SoO = Rot.GetWrapper<Skills.StrikesOfOpportunity>();

            #region Inner Rage: We have >75 Rage
            if (s.Current_Rage > 75 && s.TimeSinceLast_IR > IR.ability.Duration)
            {
                // Sudden death is active, we can execute
                double dur = CalcOpts.React / 1000;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = IR.ability,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Max(0f, Math.Min(100f, s.Current_Rage + (WhiteRageForAGCD * dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        s.HaveBuff_SD,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        0,
                        true
                    ),
                    TransitionProbability = 1.0,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Inner Rage",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion
            //else
            #region Colossus Smash: We either have Sudden Death and it's been more than 6 sec since we last did it or its been >20 sec since we last did it
            if (((s.HaveBuff_SD && s.TimeSinceLast_CS >= 6) || s.TimeSinceLast_CS >= CS.ability.Cd) && s.Current_Rage > CS.ability.RageCost)
            {
                double dur = LatentGCD;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = CS.ability,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Max(0f, Math.Min(100f, s.Current_Rage - CS.ability.RageCost + (WhiteRageForAGCD * dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        0, // we just used it
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        true, // procs off itself
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * CS.ability.MHAtkTable.AnyLand,
                });
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = CS.ability,
                    TransitionDuration = dur,
                    TargetState = GetState(
                        Math.Max(0f, Math.Min(100f, s.Current_Rage - CS.ability.RageCost + (WhiteRageForAGCD * dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        0, // we just used it
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        false, // didn't proc, current proc consumed
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Colossus Smash",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion
            else
            #region Rend: We need Rend up at all times
            if (!s.HaveBuff_RD && s.Current_Rage > RD.ability.RageCost)
            {
                double dur = LatentGCD;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = RD.ability,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Max(0f, Math.Min(100f, s.Current_Rage - RD.ability.RageCost + (WhiteRageForAGCD * dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        6, //s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        true, // The application of Rend generated it
                        true, // proc'd
                        0f,
                        true, // we just applied it
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * CS.ability.MHAtkTable.AnyLand,
                });
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = RD.ability,
                    TransitionDuration = dur,
                    TargetState = GetState(
                        Math.Max(0f, Math.Min(100f, s.Current_Rage - RD.ability.RageCost + (WhiteRageForAGCD * dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        6, //s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        true, // The application of Rend generated it
                        s.HaveBuff_SD, // didn't proc
                        0f,
                        true, // we just applied it
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Rend",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion
            else
            #region A Point where we are almost ready to MS and don't want to delay it, so we do nothing
            if (s.CDRem_MS != 0 && s.CDRem_MS < LatentGCD)
            {
                // do nothing, don't want to reset GCD and delay MS
                // later we'll consider delaying for an extra Slam or Execute
                double dur = LatentGCD - s.CDRem_MS;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = null,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Min(100, s.Current_Rage + WhiteRageForAGCD * (dur / LatentGCD)),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        s.HaveBuff_SD,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        true,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1.0,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Delayed for Mortal Strike",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion
            else
            #region A Point where we are ready to MS, so we do
            if (s.CDRem_MS == 0 && s.Current_Rage > MS.ability.RageCost)
            {
                // Time to MS!
                // This is the chance we don't proc Sudden Death
                double dur = LatentGCD;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = MS.ability,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - MS.ability.RageCost + WhiteRageForAGCD * dur / LatentGCD)),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        true,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        MS.ability.Cd,
                        false,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * MS.ability.MHAtkTable.AnyLand,
                });
                // This is the chance we do proc Sudden Death
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = MS.ability,
                    TransitionDuration = dur,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - MS.ability.RageCost + WhiteRageForAGCD * dur / LatentGCD)),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        s.HaveBuff_SD,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        MS.ability.Cd,
                        false,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Mortal Strike",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion
            else
            #region A Point where we are waiting for Taste For Blood
            if ((s.TimeTilNext_RendTickProcgTfB == 0 || s.HaveBuff_OPTfB) && s.Current_Rage > OP.ability.RageCost)
            {
                // TfB should proc now, we can use the ability after react time
                // The Split is due to Sudden Death proc chance
                double dur = LatentGCD;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = s.TimeTilNext_RendTickProcgTfB == 0 ? (Skills.Ability)TB.ability : (Skills.Ability)OP.ability,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - TB.ability.RageCost + (WhiteRageForAGCD * dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        6f,
                        s.TimeSinceLast_CS + dur,
                        false,
                        true,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * TB.ability.MHAtkTable.AnyLand,
                });
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = s.TimeTilNext_RendTickProcgTfB == 0 ? (Skills.Ability)TB.ability : (Skills.Ability)OP.ability,
                    TransitionDuration = dur,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - TB.ability.RageCost + (WhiteRageForAGCD * dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        6f,
                        s.TimeSinceLast_CS + dur,
                        false,
                        s.HaveBuff_SD,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Taste for Blood or Overpower",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion
            else
            #region A Point where we are ready to Slam AND have enough rage to do so
            if (s.Current_Rage > SL.ability.RageCost)
            {
                // do slam if nothing else
                double dur = LatentGCD;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = SL.ability,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - SL.ability.RageCost + WhiteRageForAGCD * (Talents.ImprovedSlam / 3) * (dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        true,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * SL.ability.MHAtkTable.AnyLand,
                });
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = SL.ability,
                    TransitionDuration = dur,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - SL.ability.RageCost + WhiteRageForAGCD * (Talents.ImprovedSlam / 3) * (dur / LatentGCD))),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        s.HaveBuff_SD,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Slam",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion
            else
            #region We don't have enough Rage to do anything else, so lets Battle Shout if we can
            if (s.CDRem_BattleShout <= 0) {
                // We don't have enough rage to do anything on this GCD
                double dur = LatentGCD;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = null,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Min(100, s.Current_Rage + BS.ability.RageCost * -1 + WhiteRageForAGCD * (dur / LatentGCD)),
                        BS.ability.Cd,
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        s.HaveBuff_SD,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1.0,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Battle Shout to Generate Rage",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion
            else
            #region A Point where we couldn't use any ability because of lack of Rage
            {
                // We don't have enough rage to do anything on this GCD
                double dur = Math.Max(0.01,
                    Math.Min(s.TimeTilNext_RendTickProcgTfB,
                    Math.Min(s.TimeSinceLast_CS - CS.ability.Cd,
                    Math.Min(s.TimeSinceLast_RD - RD.ability.Duration,
                    Math.Min(s.CDRem_MS,
                             LatentGCD))))
                    );
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = null,
                    TransitionDuration = dur,
                    TargetState = newState = GetState(
                        Math.Min(100, s.Current_Rage + WhiteRageForAGCD * (dur / LatentGCD)),
                        Math.Max(0f, s.CDRem_BattleShout - dur),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.TimeSinceLast_CS + dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        s.HaveBuff_SD,
                        s.TimeSinceLast_RD + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_RD + dur < RD.ability.Cd),
                        Math.Max(0f, s.CDRem_MS - dur),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs,
                        s.TimeSinceLast_IR + dur,
                        s.HaveBuff_RD && (s.TimeSinceLast_IR + dur < IR.ability.Duration)
                    ),
                    TransitionProbability = 1.0,
                });
                MoveCounter++; totalTimePassed = (float)dur;
                System.Diagnostics.Debug.WriteLine(string.Format("{0:0000}, {13:000.000}, {2:000.0}, {12:00.00}, {3:0.00}, {4:00.00}, {5}, {6}, {7:00.0000}, {8}, {9:0.00}, {10}, {11}, {14:00.00}, {15}, {1}",
                    MoveCounter,
                    "Did Nothing, no rage to use",
                    newState.Current_Rage,
                    newState.TimeTilNext_RendTickProcgTfB,
                    newState.TimeSinceLast_CS,
                    newState.HaveBuff_OPTfB,
                    newState.HaveBuff_SD,
                    newState.TimeSinceLast_RD,
                    newState.HaveBuff_RD,
                    newState.CDRem_MS,
                    newState.AbilRdy_MS,
                    newState.ThereAreMultipleMobs, newState.CDRem_BattleShout, totalTimePassed, newState.TimeSinceLast_IR, newState.HaveBuff_IR
                    ));
            }
            #endregion

            return list;
        }
        private Dictionary<string, StateArms> stateDictionary = new Dictionary<string, StateArms>();
        public StateArms GetState(double _rage, double _BattleShoutcooldownleft, double _timetillnextTfBproccingrendtick, double _timeSinceLastCS,
                                  bool _OverpowerTfBbuff, bool _SuddenDeathbuff, double _timeSinceLastRend, bool _Rendbuff,
                                  double _MortalStrikecooldownleft, bool _MortalStrikeReady,
                                  bool _ThereAreMultipleMobs, double _timeSinceLastIR, bool _IRbuff)
        {
            string name = string.Format(
                //"Rage {0:000.0},BSCdRem {6:0},TfBBuff {1},SDBuff {2},RDBuff {5},IRBuff {7},MSCdRem {3:0.00},MM {4}",//GCD {1:0.0000},White {2:0.0000},TfB {3:0.0000},
                "Rage {0:000.0},BSCdRem {1:0},TfBBuff {2},SDBuff {3},RDBuff {4},IRBuff {5},MSRdy {6},MM {7}",
                Math.Round(_rage, 1),
                Math.Round(_BattleShoutcooldownleft, 0),
                _OverpowerTfBbuff ? "+" : "-",
                _SuddenDeathbuff ? "+" : "-",
                _Rendbuff ? "+" : "-",
                _IRbuff ? "+" : "-",
                _MortalStrikeReady ? "+" : "-",
                _ThereAreMultipleMobs ? "+" : "-"
                );
            StateArms state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new StateArms()
                {
                    Name = name,
                    Current_Rage = Math.Round(_rage, 1),
                    CDRem_BattleShout = Math.Round(_BattleShoutcooldownleft, 2),
                    TimeTilNext_RendTickProcgTfB = Math.Round(_timetillnextTfBproccingrendtick, 2),
                    TimeSinceLast_CS = Math.Round(_timeSinceLastCS, 2),
                    HaveBuff_OPTfB = _OverpowerTfBbuff,
                    HaveBuff_SD = _SuddenDeathbuff,
                    TimeSinceLast_RD = _timeSinceLastRend,
                    HaveBuff_RD = _Rendbuff,
                    CDRem_MS = Math.Round(_MortalStrikecooldownleft, 2),
                    AbilRdy_MS = _MortalStrikeReady,
                    ThereAreMultipleMobs = _ThereAreMultipleMobs,
                    TimeSinceLast_IR = _timeSinceLastIR,
                    HaveBuff_IR = _IRbuff,
                };
                stateDictionary[name] = state;
            }
            return state;
        }
        public StateArms GetState(float _rage, float _BattleShoutcooldownleft, float _timetillnextTfBproccingrendtick, float _timesinceLastCS,
                                  bool _OverpowerTfBbuff, bool _SuddenDeathbuff, float _timeSinceLastRend, bool _Rendbuff,
                                  float _MortalStrikecooldownleft, bool _MortalStrikeReady,
                                  bool _ThereAreMultipleMobs, float _timeSinceLastIR, bool _IRbuff)
        {
            return GetState((double)_rage, (double)_BattleShoutcooldownleft, (double)_timetillnextTfBproccingrendtick, (double)_timesinceLastCS,
                                   _OverpowerTfBbuff, _SuddenDeathbuff, _timeSinceLastRend, _Rendbuff,
                                   (double)_MortalStrikecooldownleft, _MortalStrikeReady,
                                   _ThereAreMultipleMobs, (double)_timeSinceLastIR, _IRbuff);
        }
    }

    public class StateSpaceGeneratorArmsTest {
        public void StateSpaceGeneratorArmsTest1(Character c, Stats s, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo, bool needsDisplayCalculations)
        {
            ArmsGenerator gen = new ArmsGenerator(c, s, cf, wa, co, bo);
            var stateSpace = gen.GenerateStateSpace();
            string output = "\n===== DPSWarr ARMS MARKOVIAN OUTPUT =====\n\n";
            //foreach (State<Rawr.DPSWarr.Skills.Ability> a in stateSpace) { output += a.ToString() + "\n"; }
            //output += "\nDone Generating States";
            try {
                MarkovProcess<Skills.Ability> mp = new MarkovProcess<Skills.Ability>(stateSpace);

                double averageDamage = 0.0;
                output += "\naverageDamage: 0.0000";
                foreach (KeyValuePair<Skills.Ability, double> kvp in mp.AbilityWeight) {
                    averageDamage += kvp.Key.DamageOnUse * kvp.Value;
                    output += string.Format("\naverageDamage: {0:0.0000} | {1:0.0000} = {2:0.0000} * {3:0.00000000} : {4}", averageDamage, kvp.Key.DamageOnUse * kvp.Value, kvp.Key.DamageOnUse, kvp.Value, kvp.Key.Name);
                }

                double dps = averageDamage / mp.AverageTransitionDuration;
                dps += gen.Rot.WhiteAtks.MhDPS;

                output += string.Format("\nTotal DPS: {0}\n", dps);
                System.Diagnostics.Debug.WriteLine(output);
            }
            catch (Exception ex)
            {
                Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error in creating Arms Markov Calculations",
                    ex.Message, ex.InnerException,
                    "StateSpaceGeneratorArmsTest1()", "StateSpace Count: " + stateSpace.Count.ToString(), ex.StackTrace);
                eb.Show();
            }
        }
    }
}
