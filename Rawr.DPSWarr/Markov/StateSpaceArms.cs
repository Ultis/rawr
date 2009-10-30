using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.DPSWarr.Markov
{
    public class ArmsGenerator : StateSpaceGenerator<Skills.Ability>
    {
        public ArmsGenerator(Character c, Stats s, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co) {
            Char = c; Talents = c.WarriorTalents; StatS = s; combatFactors = cf; WhiteAtks = wa; CalcOpts = co;
            //
            Rot = new ArmsRotation(c, s, cf, wa, co);
            Rot.Initialize();
            LatentGCD = 1.5 + co.FullLatency;
        }

        protected double LatentGCD;
        public ArmsRotation Rot = null;
        Character Char;
        WarriorTalents Talents;
        Stats StatS;
        CombatFactors combatFactors;
        Skills.WhiteAttacks WhiteAtks;
        CalculationOptionsDPSWarr CalcOpts;

        public class StateArms : State<Skills.Ability>
        {
            public double Current_Rage;
            public double TimeTilNext_GCD;
            public double TimeTilNext_WhiteAttack;
            public double TimeTilNext_RendTickProcgTfB;
            public bool HaveBuff_OPTfB;
            public bool HaveBuff_SD;
            public double CDRem_MS;
            public bool ThereAreMultipleMobs;
        }

        protected override State<Skills.Ability> GetInitialState()
        {
            return GetState(0, 0, 0, 6, false, false, 0, false);
            //return GetState(0, LatentGCD, Rot.MS.Whiteattacks.MhEffectiveSpeed, 6, false, false, 0, false);
        }

        protected override List<StateTransition<Skills.Ability>> GetStateTransitions(State<Skills.Ability> state)
        {
            StateArms s = state as StateArms;
            List<StateTransition<Skills.Ability>> list = new List<StateTransition<Skills.Ability>>();
            if (s.TimeTilNext_WhiteAttack == 0)
            {
                if (s.Current_Rage > 70f && s.ThereAreMultipleMobs && s.Current_Rage > Rot.CL.RageCost)
                {
                    // do Cleave on next White swing
                    double dur = Math.Min(s.TimeTilNext_GCD, s.TimeTilNext_WhiteAttack);
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = Rot.CL,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            s.Current_Rage - Rot.CL.RageCost,
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, Rot.FW.Cd - dur),
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                            s.HaveBuff_SD,
                            Math.Max(0f, s.CDRem_MS - dur),
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0,
                    });
                }
                else if (s.Current_Rage > 70f && s.Current_Rage > Rot.HS.RageCost)
                {
                    // do Heroic Strike on next White swing
                    double dur = Math.Min(s.TimeTilNext_GCD, s.TimeTilNext_WhiteAttack);
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = Rot.HS,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            s.Current_Rage - Rot.HS.RageCost,
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, Rot.FW.Cd - dur),
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                            s.HaveBuff_SD,
                            Math.Max(0f, s.CDRem_MS - dur),
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0,
                    });
                }
                else
                {
                    // let it white swing
                    double dur = Math.Min(s.TimeTilNext_GCD, Rot.FW.Cd);
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = Rot.FW,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            Math.Min(100, s.Current_Rage + Rot.WhiteAtks.MHSwingRage),
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, Rot.FW.Cd - dur),
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                            s.HaveBuff_SD,
                            Math.Max(0f, s.CDRem_MS - dur),
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0,
                    });
                }
            }
            else if (s.TimeTilNext_GCD == 0)
            {
                if (s.CDRem_MS != 0 && s.CDRem_MS < LatentGCD)
                {
                    // do nothing, don't want to reset GCD and delay MS
                    // later we'll consider delaying for an extra Slam or Execute
                    double dur = Math.Min(s.CDRem_MS, s.TimeTilNext_WhiteAttack);
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = null,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            s.Current_Rage,
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, s.TimeTilNext_WhiteAttack - dur),
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                            s.HaveBuff_SD,
                            Math.Max(0f, s.CDRem_MS - dur),
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0,
                    });
                }
                else if (s.CDRem_MS == 0 && s.Current_Rage > Rot.MS.RageCost)
                {
                    // Time to MS!
                    double dur = Math.Min(LatentGCD, s.TimeTilNext_WhiteAttack);
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = Rot.MS,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            s.Current_Rage - Rot.MS.RageCost,
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, s.TimeTilNext_WhiteAttack - dur),
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/
                                                                                                         ,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                            true,
                            Rot.MS.Cd,
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0 * (0.03 * Talents.SuddenDeath) * Rot.MS.MHAtkTable.AnyLand,
                    });
                    if (list[list.Count - 1].TransitionProbability != 1.0)
                    {
                        list.Add(new StateTransition<Skills.Ability>()
                        {
                            Ability = Rot.MS,
                            TransitionDuration = dur,
                            TargetState = GetState(
                                s.Current_Rage - Rot.MS.RageCost,
                                Math.Max(0f, s.TimeTilNext_GCD - dur),
                                Math.Max(0f, s.TimeTilNext_WhiteAttack - dur),
                                s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/
                                                                                                             ,
                                s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                                s.HaveBuff_SD,
                                Rot.MS.Cd,
                                s.ThereAreMultipleMobs
                            ),
                            TransitionProbability = 1 - list[list.Count - 1].TransitionProbability,
                        });
                    }
                }
                else if ((s.TimeTilNext_RendTickProcgTfB == 0 || s.HaveBuff_OPTfB) && s.Current_Rage > Rot.OP.RageCost)
                {
                    // tfb should proc now, we can use the ability after react time
                    double dur = Math.Min(LatentGCD, s.TimeTilNext_WhiteAttack);
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = s.TimeTilNext_RendTickProcgTfB == 0 ? (Skills.Ability)Rot.TB : (Skills.Ability)Rot.OP,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            s.Current_Rage - Rot.MS.RageCost,
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, s.TimeTilNext_WhiteAttack - dur),
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/,
                            false,
                            s.HaveBuff_SD,
                            Math.Max(0f, s.CDRem_MS - dur),
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0,
                    });
                }
                else if (s.HaveBuff_SD && s.Current_Rage > Rot.SD.RageCost)
                {
                    // Sudden death is active, we can execute
                    double dur = Math.Min(LatentGCD, s.TimeTilNext_WhiteAttack);
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = Rot.SD,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            s.Current_Rage - (Rot.SD.RageCost + Rot.SD.UsedExtraRage),
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, s.TimeTilNext_WhiteAttack - dur),
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                            false,
                            Math.Max(0f, s.CDRem_MS - dur),
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0,
                    });
                }
                else if (s.Current_Rage > Rot.SL.RageCost)
                {
                    // do slam if nothing else
                    double dur = Math.Min(LatentGCD, s.TimeTilNext_WhiteAttack);
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = Rot.SL,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            s.Current_Rage - Rot.SL.RageCost,
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, s.TimeTilNext_WhiteAttack + Rot.SL.CastTime - dur),// Slam delays the swing timer
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                            s.HaveBuff_SD,
                            Math.Max(0f, s.CDRem_MS - dur),
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0,
                    });
                }else{
                    // We don't have enough rage to do anything on this GCD
                    double dur = s.TimeTilNext_WhiteAttack;
                    list.Add(new StateTransition<Skills.Ability>()
                    {
                        Ability = null,
                        TransitionDuration = dur,
                        TargetState = GetState(
                            s.Current_Rage,
                            Math.Max(0f, s.TimeTilNext_GCD - dur),
                            Math.Max(0f, s.TimeTilNext_WhiteAttack - dur),
                            s.TimeTilNext_RendTickProcgTfB /*> dur
                                                            ? s.TimeTilNext_RendTickProcgTfB - dur
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - dur*/,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                            s.HaveBuff_SD,
                            Math.Max(0f, s.CDRem_MS - dur),
                            s.ThereAreMultipleMobs
                        ),
                        TransitionProbability = 1.0,
                    });
                }
            }
            return list;
        }
        private Dictionary<string, StateArms> stateDictionary = new Dictionary<string, StateArms>();
        public StateArms GetState(double _rage, double _gcdtime, double _whiteattacktime, double _timetillnextTfBproccingrendtick,
                                   bool _OverpowerTfBbuff, bool _SuddenDeathbuff, double _MortalStrikecooldownleft, bool _ThereAreMultipleMobs)
        {
            string name = string.Format("Rage {0:000.0000},GCD {1:0.0000},White {2:0.0000},TfB {3:0.0000},TfBBuff {4},SDBuff {5},MSCD {6:0.0000},MM {7}",
                                        _rage,
                                        _gcdtime,
                                        _whiteattacktime,
                                        _timetillnextTfBproccingrendtick,
                                        _OverpowerTfBbuff ? "+" : "-",
                                        _SuddenDeathbuff ? "+" : "-",
                                        _MortalStrikecooldownleft,
                                        _ThereAreMultipleMobs ? "+" : "-");
            StateArms state;
            if (!stateDictionary.TryGetValue(name, out state))
            {
                state = new StateArms()
                {
                    Name = name,
                    Current_Rage = _rage,
                    TimeTilNext_GCD = _gcdtime,
                    TimeTilNext_WhiteAttack = _whiteattacktime,
                    TimeTilNext_RendTickProcgTfB = _timetillnextTfBproccingrendtick,
                    HaveBuff_OPTfB = _OverpowerTfBbuff,
                    HaveBuff_SD = _SuddenDeathbuff,
                    CDRem_MS = _MortalStrikecooldownleft,
                    ThereAreMultipleMobs = _ThereAreMultipleMobs,
                };
                stateDictionary[name] = state;
            }
            return state;
        }
        public StateArms GetState(float _rage, float _gcdtime, float _whiteattacktime, float _timetillnextTfBproccingrendtick,
                                   bool _OverpowerTfBbuff, bool _SuddenDeathbuff, float _MortalStrikecooldownleft, bool _ThereAreMultipleMobs)
        {
            return GetState((double)_rage, (double)_gcdtime, (double)_whiteattacktime, (double)_timetillnextTfBproccingrendtick,
                                   _OverpowerTfBbuff, _SuddenDeathbuff, (double)_MortalStrikecooldownleft, _ThereAreMultipleMobs);
        }
    }

    public class StateSpaceGeneratorArmsTest {
        public void StateSpaceGeneratorArmsTest1(Character c, Stats s, CombatFactors cf, Skills.WhiteAttacks wa, CalculationOptionsDPSWarr co)
        {
            ArmsGenerator gen = new ArmsGenerator(c, s, cf, wa, co);

            var stateSpace = gen.GenerateStateSpace();

            MarkovProcess<Skills.Ability> mp = new MarkovProcess<Skills.Ability>(stateSpace);

            double averageDamage = 0.0;
            foreach (KeyValuePair<Skills.Ability, double> kvp in mp.AbilityWeight)
            {
                averageDamage += kvp.Key.DamageOnUse * kvp.Value;
            }

            double dps = averageDamage / mp.AverageTransitionDuration;
        }
    }
}
