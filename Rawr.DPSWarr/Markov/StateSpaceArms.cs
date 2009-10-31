using System;
using System.Collections.Generic;
#if !RAWR3
using Rawr.Base.Algorithms;
#endif
using System.Text;

namespace Rawr.DPSWarr.Markov
{
#if !RAWR3
    public class ArmsGenerator : StateSpaceGenerator<Skills.Ability>
    {
        public ArmsGenerator(Character c, Stats s, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co) {
            Char = c; Talents = c.WarriorTalents; StatS = s; combatFactors = cf; WhiteAtks = wa; CalcOpts = co;
            //
            Rot = new ArmsRotation(c, s, cf, wa, co);
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
        #endregion

        public class StateArms : State<Skills.Ability>
        {
            public double Current_Rage;
            public double TimeTilNext_RendTickProcgTfB;
            public bool HaveBuff_OPTfB;
            public bool HaveBuff_SD;
            public double CDRem_MS;
            public bool AbilRdy_MS;
            public bool ThereAreMultipleMobs;
        }

        protected override State<Skills.Ability> GetInitialState()
        {
            return GetState(0, 6, false, false, 0, true, false);
            //return GetState(0, 6, false, false, 0, true, false);
        }

        private double WhiteRageForAGCD {
            get {
                return (Rot.WhiteAtks.whiteRageGenOverDur
                        * LatentGCD)
                        / CalcOpts.Duration;
            }
        }

        protected override List<StateTransition<Skills.Ability>> GetStateTransitions(State<Skills.Ability> state)
        {
            StateArms s = state as StateArms;
            List<StateTransition<Skills.Ability>> list = new List<StateTransition<Skills.Ability>>();

            if (s.CDRem_MS != 0 && s.CDRem_MS < LatentGCD)
            {
                // do nothing, don't want to reset GCD and delay MS
                // later we'll consider delaying for an extra Slam or Execute
                double dur = LatentGCD - s.CDRem_MS;
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = null,
                    TransitionDuration = dur,
                    TargetState = GetState(
                        Math.Min(100, s.Current_Rage + WhiteRageForAGCD * (dur / LatentGCD)),
                        s.TimeTilNext_RendTickProcgTfB > dur ? s.TimeTilNext_RendTickProcgTfB - dur : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                        s.HaveBuff_SD,
                        Math.Max(0f, s.CDRem_MS - LatentGCD),
                        true,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1.0,
                });
            }
            else if (s.CDRem_MS == 0 && s.Current_Rage > Rot.MS.RageCost)
            {
                // Time to MS!
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = Rot.MS,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - Rot.MS.RageCost + WhiteRageForAGCD)),
                        s.TimeTilNext_RendTickProcgTfB > LatentGCD ? s.TimeTilNext_RendTickProcgTfB - LatentGCD : 6f + s.TimeTilNext_RendTickProcgTfB - LatentGCD,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - LatentGCD) == 0),
                        true,
                        Rot.MS.Cd,
                        false,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * Rot.MS.MHAtkTable.AnyLand,
                });
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = Rot.MS,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - Rot.MS.RageCost + WhiteRageForAGCD)),
                        s.TimeTilNext_RendTickProcgTfB > LatentGCD ? s.TimeTilNext_RendTickProcgTfB - LatentGCD : 6f + s.TimeTilNext_RendTickProcgTfB - LatentGCD,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - LatentGCD) == 0),
                        s.HaveBuff_SD,
                        Rot.MS.Cd,
                        false,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
            }
            else if ((s.TimeTilNext_RendTickProcgTfB == 0 || s.HaveBuff_OPTfB) && s.Current_Rage > Rot.OP.RageCost)
            {
                // TfB should proc now, we can use the ability after react time
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = s.TimeTilNext_RendTickProcgTfB == 0 ? (Skills.Ability)Rot.TB : (Skills.Ability)Rot.OP,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - Rot.TB.RageCost + WhiteRageForAGCD)),
                        6f,
                        false,
                        true,
                        Math.Max(0f, s.CDRem_MS - LatentGCD),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * Rot.TB.MHAtkTable.AnyLand,
                });
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = s.TimeTilNext_RendTickProcgTfB == 0 ? (Skills.Ability)Rot.TB : (Skills.Ability)Rot.OP,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - Rot.TB.RageCost + WhiteRageForAGCD)),
                        6f,
                        false,
                        s.HaveBuff_SD,
                        Math.Max(0f, s.CDRem_MS - LatentGCD),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
            }
            else if (s.HaveBuff_SD && s.Current_Rage > Rot.SD.RageCost)
            {
                // Sudden death is active, we can execute
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = Rot.SD,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - (Rot.SD.RageCost + Rot.SD.UsedExtraRage) + WhiteRageForAGCD)),
                        s.TimeTilNext_RendTickProcgTfB > LatentGCD ? s.TimeTilNext_RendTickProcgTfB - LatentGCD : 6f + s.TimeTilNext_RendTickProcgTfB - LatentGCD,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - LatentGCD) == 0),
                        true, // procs off itself
                        Math.Max(0f, s.CDRem_MS - LatentGCD),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * Rot.SD.MHAtkTable.AnyLand,
                });
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = Rot.SD,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - (Rot.SD.RageCost + Rot.SD.UsedExtraRage) + WhiteRageForAGCD)),
                        s.TimeTilNext_RendTickProcgTfB > LatentGCD ? s.TimeTilNext_RendTickProcgTfB - LatentGCD : 6f + s.TimeTilNext_RendTickProcgTfB - LatentGCD,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - LatentGCD) == 0),
                        false, // didn't proc, current proc consumed
                        Math.Max(0f, s.CDRem_MS - LatentGCD),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
            }
            else if (s.Current_Rage > Rot.SL.RageCost)
            {
                // do slam if nothing else
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = Rot.SL,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - Rot.SL.RageCost + WhiteRageForAGCD * (Talents.ImprovedSlam / 3))),
                        s.TimeTilNext_RendTickProcgTfB > LatentGCD ? s.TimeTilNext_RendTickProcgTfB - LatentGCD : 6f + s.TimeTilNext_RendTickProcgTfB - LatentGCD,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - LatentGCD) == 0),
                        true,
                        Math.Max(0f, s.CDRem_MS - LatentGCD),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * Rot.SL.MHAtkTable.AnyLand,
                });
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = Rot.SL,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Max(0, Math.Min(100, s.Current_Rage - Rot.SL.RageCost + WhiteRageForAGCD * (Talents.ImprovedSlam / 3))),
                        s.TimeTilNext_RendTickProcgTfB > LatentGCD ? s.TimeTilNext_RendTickProcgTfB - LatentGCD : 6f + s.TimeTilNext_RendTickProcgTfB - LatentGCD,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - LatentGCD) == 0),
                        s.HaveBuff_SD,
                        Math.Max(0f, s.CDRem_MS - LatentGCD),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                });
            }
            else
            {
                // We don't have enough rage to do anything on this GCD
                list.Add(new StateTransition<Skills.Ability>()
                {
                    Ability = null,
                    TransitionDuration = LatentGCD,
                    TargetState = GetState(
                        Math.Min(100, s.Current_Rage + WhiteRageForAGCD),
                        s.TimeTilNext_RendTickProcgTfB > LatentGCD ? s.TimeTilNext_RendTickProcgTfB - LatentGCD : 6f + s.TimeTilNext_RendTickProcgTfB - LatentGCD,
                        s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - LatentGCD) == 0),
                        s.HaveBuff_SD,
                        Math.Max(0f, s.CDRem_MS - LatentGCD),
                        s.AbilRdy_MS,
                        s.ThereAreMultipleMobs
                    ),
                    TransitionProbability = 1.0,
                });
            }
            return list;
        }
        private Dictionary<string, StateArms> stateDictionary = new Dictionary<string, StateArms>();
        public StateArms GetState(double _rage, double _timetillnextTfBproccingrendtick,
                                  bool _OverpowerTfBbuff, bool _SuddenDeathbuff,
                                  double _MortalStrikecooldownleft, bool _MortalStrikeReady,
                                  bool _ThereAreMultipleMobs)
        {
            string name = string.Format(
                "Rage {0:000.0000},TfBBuff {1},SDBuff {2},MSCdRem {3:0.0000},MM {4}",//GCD {1:0.0000},White {2:0.0000},TfB {3:0.0000},
                Math.Round(_rage, 0),
                //_timetillnextTfBproccingrendtick,
                _OverpowerTfBbuff ? "+" : "-",
                _SuddenDeathbuff ? "+" : "-",
                Math.Round(_MortalStrikecooldownleft,1),//_MortalStrikeReady ? "+" : "-",
                _ThereAreMultipleMobs ? "+" : "-");
            StateArms state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new StateArms()
                {
                    Name = name,
                    Current_Rage = Math.Round(_rage, 4),
                    TimeTilNext_RendTickProcgTfB = Math.Round(_timetillnextTfBproccingrendtick, 4),
                    HaveBuff_OPTfB = _OverpowerTfBbuff,
                    HaveBuff_SD = _SuddenDeathbuff,
                    CDRem_MS = Math.Round(_MortalStrikecooldownleft, 4),
                    AbilRdy_MS = _MortalStrikeReady,
                    ThereAreMultipleMobs = _ThereAreMultipleMobs,
                };
                stateDictionary[name] = state;
            }
            return state;
        }
        public StateArms GetState(float _rage, float _timetillnextTfBproccingrendtick,
                                  bool _OverpowerTfBbuff, bool _SuddenDeathbuff,
                                  float _MortalStrikecooldownleft, bool _MortalStrikeReady,
                                  bool _ThereAreMultipleMobs)
        {
            return GetState((double)_rage, (double)_timetillnextTfBproccingrendtick,
                                   _OverpowerTfBbuff, _SuddenDeathbuff,
                                   (double)_MortalStrikecooldownleft, _MortalStrikeReady,
                                   _ThereAreMultipleMobs);
        }
    }

    public class StateSpaceGeneratorArmsTest {
        public void StateSpaceGeneratorArmsTest1(Character c, Stats s, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co)
        {
            ArmsGenerator gen = new ArmsGenerator(c, s, cf, wa, co);
            var stateSpace = gen.GenerateStateSpace();
            string output = "";
            foreach (State<Rawr.DPSWarr.Skills.Ability> a in stateSpace) {
                output += a.ToString() + "\n";
            }
            output += "\ndone";
            try {
                MarkovProcess<Skills.Ability> mp = new MarkovProcess<Skills.Ability>(stateSpace);

                double averageDamage = 0.0;
                foreach (KeyValuePair<Skills.Ability, double> kvp in mp.AbilityWeight) {
                    averageDamage += kvp.Key.DamageOnUse * kvp.Value;
                }

                double dps = averageDamage / mp.AverageTransitionDuration;
                dps += gen.Rot.WhiteAtks.MhDPS;
            } catch (Exception ex) {
                new ErrorBoxDPSWarr("Error in creating Arms Markov Calculations",
                    ex.Message, "StateSpaceGeneratorArmsTest1()",
                    "StateSpace Count: " + stateSpace.Count.ToString(),
                    ex.StackTrace, 0);
            }
        }
    }
#endif
}
