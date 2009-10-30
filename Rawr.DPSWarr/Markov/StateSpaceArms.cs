using System;
using System.Collections.Generic;
using System.Text;
using Rawr.Base.Algorithms;

namespace Rawr.DPSWarr.Markov
{
    public class StateSpaceGeneratorArmsTest {
        public class Ability : Skills.Ability { }

        public class ArmsGenerator : StateSpaceGenerator<Ability>
        {
            public class StateArms : State<Ability>
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

            Ability MS = new Ability();// Skills.MortalStrike(Character,Stats,CombatFactors,Skills.WhiteAttacks,CalculationOptionsDPSWarr);
            Ability OP = new Ability();// Skills.Overpower(Character,Stats,CombatFactors,Skills.WhiteAttacks,CalculationOptionsDPSWarr);
            Ability RD = new Ability();// Skills.Rend(Character,Stats,CombatFactors,Skills.WhiteAttacks,CalculationOptionsDPSWarr);
            protected double LatentGCD = 1.7;
            public ArmsRotation Rot = null;

            protected override State<Ability> GetInitialState()
            {
                return GetState(0, LatentGCD, Rot.MS.Whiteattacks.MhEffectiveSpeed, 6, false, false, 0, false);
            }

            protected override List<StateTransition<Ability>> GetStateTransitions(State<Ability> state)
            {
                StateArms s = state as StateArms;
                List<StateTransition<Ability>> list = new List<StateTransition<Ability>>();
                if (s.TimeTilNext_GCD == 0)
                {
                    if (s.CDRem_MS < LatentGCD)
                    {
                        // do nothing, don't want to reset GCD and delay MS
                        // later we'll consider delaying for an extra Slam or Execute
                        double dur = Math.Min(s.CDRem_MS, s.TimeTilNext_WhiteAttack);
                        list.Add(new StateTransition<Ability>(){
                            Ability = null,
                            TransitionDuration = dur,
                            TargetState = GetState(
                                s.Current_Rage,
                                Math.Max(0f, s.TimeTilNext_GCD - dur),
                                Math.Max(0f, s.TimeTilNext_WhiteAttack - dur),
                                s.TimeTilNext_RendTickProcgTfB > dur
                                                                ? s.TimeTilNext_RendTickProcgTfB - dur
                                                                : 6f + s.TimeTilNext_RendTickProcgTfB - dur,
                                s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - dur) == 0),
                                s.HaveBuff_SD,
                                Math.Max(0f, s.CDRem_MS - dur),
                                s.ThereAreMultipleMobs
                            ),
                            TransitionProbability = 1.0,
                        });
                    }
                    //else if (s.CDRem_MS == 0 && s.Current_Rage > Rot.MS.RageCost)
                    {
                        // Time to MS!
                        //trans.Ability = Rot.MS;
                    }
                    //else if ((s.TimeTilNext_RendTickProcgTfB == 0 || s.HaveBuff_OPTfB) && s.Current_Rage > Rot.OP.RageCost)
                    {
                        // tfb should proc now, we can use the ability after react time
                        //trans.Ability = s.TimeTilNext_RendTickProcgTfB == 0 ? (Skills.Ability)Rot.TB : (Skills.Ability)Rot.OP;
                    }
                    //else if (s.HaveBuff_SD && s.Current_Rage > Rot.SD.RageCost)
                    {
                        // Sudden death is active, we can execute
                        //trans.Ability = Rot.SD;
                    }
                    //else if (s.Current_Rage > Rot.SL.RageCost)
                    {
                        // do slam if nothing else
                        //trans.Ability = Rot.SL;
                    }
                }
                else if (s.TimeTilNext_WhiteAttack == 0)
                {
                    if (s.Current_Rage > 70f)
                    {
                        if (s.ThereAreMultipleMobs && s.Current_Rage > Rot.CL.FullRageCost)
                        {
                            // do Cleave on next White swing
                            /*trans.Ability = Rot.CL;
                            trans.TransitionDuration = Math.Min(s.TimeTilNext_GCD, s.TimeTilNext_WhiteAttack);
                            trans.TargetState = GetState(
                                s.Current_Rage,
                                Math.Max(0f, s.TimeTilNext_GCD - trans.TransitionDuration),
                                Rot.WhiteAtks.MhEffectiveSpeed,
                                s.TimeTilNext_RendTickProcgTfB > trans.TransitionDuration
                                                                ? s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration
                                                                : 6f + s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration,
                                s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration) == 0),
                                s.HaveBuff_SD,
                                Math.Max(0f, s.CDRem_MS - trans.TransitionDuration),
                                s.ThereAreMultipleMobs
                            );*/
                        }
                        else if (s.Current_Rage > Rot.HS.FullRageCost)
                        {
                            // do Heroic Strike on next White swing
                            /*trans.Ability = Rot.HS;
                            trans.TransitionDuration = Math.Min(s.TimeTilNext_GCD, s.TimeTilNext_WhiteAttack);
                            trans.TargetState = GetState(
                                s.Current_Rage,
                                Math.Max(0f, s.TimeTilNext_GCD - trans.TransitionDuration),
                                Rot.WhiteAtks.MhEffectiveSpeed,
                                s.TimeTilNext_RendTickProcgTfB > trans.TransitionDuration
                                                                ? s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration
                                                                : 6f + s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration,
                                s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration) == 0),
                                s.HaveBuff_SD,
                                Math.Max(0f, s.CDRem_MS - trans.TransitionDuration),
                                s.ThereAreMultipleMobs
                            );*/
                        }
                    }
                    else
                    {
                        // let it white swing
                        /*trans.Ability = null; // need to make this be White Attacks -> a MH Swing
                        trans.TransitionDuration = Math.Min(s.TimeTilNext_GCD, s.TimeTilNext_WhiteAttack);
                        trans.TargetState = GetState(
                            s.Current_Rage,
                            Math.Max(0f, s.TimeTilNext_GCD - trans.TransitionDuration),
                            Rot.WhiteAtks.MhEffectiveSpeed,
                            s.TimeTilNext_RendTickProcgTfB > trans.TransitionDuration
                                                            ? s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration
                                                            : 6f + s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration,
                            s.HaveBuff_OPTfB || (Math.Max(0f, s.TimeTilNext_RendTickProcgTfB - trans.TransitionDuration) == 0),
                            s.HaveBuff_SD,
                            Math.Max(0f, s.CDRem_MS - trans.TransitionDuration),
                            s.ThereAreMultipleMobs
                        );*/
                    }
                }
                return list;
            }
            private Dictionary<string, StateArms> stateDictionary = new Dictionary<string, StateArms>();
            public StateArms GetState(double _rage, double _gcdtime, double _whiteattacktime, double _timetillnextTfBproccingrendtick,
                                       bool _OverpowerTfBbuff, bool _SuddenDeathbuff, double _MortalStrikecooldownleft, bool _ThereAreMultipleMobs)
            {
                string name = string.Format("Rage{0},GCD{1},White{2},TfB{3},TfBBuff{4},SDBuff{5},MSCD{6},MM{7}",
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

        /*public void StateSpaceGeneratorArmsTest1()
        {
            ArmsGenerator gen = new ArmsGenerator();

            var stateSpace = gen.GenerateStateSpace();

            MarkovProcess<Ability> mp = new MarkovProcess<Ability>(stateSpace);
            double unit = mp.AbilityWeight[gen.AB0];

            Assert.AreEqual(mp.AbilityWeight[gen.AB0] / unit, 1.0, 0.000001, "AB0");
            Assert.AreEqual(mp.AbilityWeight[gen.AB1] / unit, 1.0, 0.000001, "AB1");
            Assert.AreEqual(mp.AbilityWeight[gen.AB2] / unit, 0.8, 0.000001, "AB2");
            Assert.AreEqual(mp.AbilityWeight[gen.ABar] / unit, 0.36, 0.000001, "ABar");
            Assert.AreEqual(mp.AbilityWeight[gen.ABar3] / unit, 0.64, 0.000001, "ABar3");
            Assert.AreEqual(mp.AbilityWeight[gen.MBAM2] / unit, 0.2, 0.000001, "MBAM2");
            Assert.AreEqual(mp.AbilityWeight[gen.MBAM3] / unit, 0.16, 0.000001, "MBAM3");
            Assert.AreEqual(mp.AbilityWeight[gen.MBAM] / unit, 0.3024, 0.000001, "MBAM");
        }*/
    }
}
