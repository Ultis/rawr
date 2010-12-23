using System;
using System.Collections.Generic;
using System.Linq;
using Rawr.Base.Algorithms;

namespace Rawr.RestoSham.StateMachine
{
    public enum SequenceType
    {
        Burst,
        Sustained
    }

    public sealed class StateGenerator : StateSpaceGenerator<Spell>
    {
        private List<Spell> _AvailableSpells;
        private List<RestoShamState> _TargetStates;
        private SequenceType _SequenceType;
        private int _ManaPool = 0;
        private int _MP5 = 0;
        private Spell _NoOp = new HealingSpell() { SpellId = 1, SpellName = "None", SpellAbrv = "N", BaseCastTime = 5f, BaseEffect = 0f };
        private SpellState _NoOpState = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateGenerator"/> class.
        /// </summary>
        public StateGenerator(List<Spell> availableSpells, SequenceType sequence, float totalMana, float mp5)
            : base()
        {
            if (availableSpells == null)
                throw new ArgumentNullException("availableSpells");

            _AvailableSpells = availableSpells;
            _TargetStates = new List<RestoShamState>();
            _SequenceType = sequence;
            _ManaPool = Convert.ToInt32(totalMana);
            _MP5 = Convert.ToInt32(mp5);
            _NoOp.Calculate();
            _NoOpState = new SpellState(_NoOp, (byte)_AvailableSpells.Count);
        }

        protected override State<Spell> GetInitialState()
        {
            if (_TargetStates.Count == 0)
                _TargetStates.Add(new RestoShamState(_AvailableSpells, _ManaPool));

            return _TargetStates.FirstOrDefault(i => i.Name == "INITIAL");
        }

        protected override List<StateTransition<Spell>> GetStateTransitions(State<Spell> state)
        {
            RestoShamState restoState = state as RestoShamState;
            List<StateTransition<Spell>> transitions = new List<StateTransition<Spell>>();

            lock (state)
            {
                bool spellCast = false;

                if (restoState.FightTime > 60f)
                {
                    State<Spell> ss = GetInitialState();
                    spellCast = true;
                    transitions.Add(new StateTransition<Spell>()
                    {
                        Ability = _NoOpState.TrackedSpell,
                        TransitionProbability = 1d,
                        TransitionDuration = 0f,
                        TargetState = ss
                    });
                }

                if (!spellCast && restoState.ManaPool < 4000)
                {
                    SpellState ss = restoState.SpellStates.FirstOrDefault(i => i.TrackedSpell.ManaBack > i.TrackedSpell.ManaCost);
                    if (ss != null)
                    {
                        spellCast = true;
                        transitions.Add(new StateTransition<Spell>()
                        {
                            Ability = ss.TrackedSpell,
                            TransitionProbability = 1d,
                            TransitionDuration = ss.TrackedSpell.CastTime,
                            TargetState = CastSpell(restoState, ss)
                        });
                    }
                    else
                    {
                        spellCast = true;
                        transitions.Add(new StateTransition<Spell>()
                        {
                            Ability = _NoOp,
                            TransitionProbability = 1d,
                            TransitionDuration = _NoOp.CastTime,
                            TargetState = CastSpell(restoState, _NoOpState)
                        });
                    }
                }

                if (!spellCast)
                {
                    foreach (SpellState ss in restoState.GetPrioritySpellStates())
                    {
                        if (!ss.IsActive && !ss.IsOnCooldown)
                        {
                            // cast this spell
                            spellCast = true;
                            transitions.Add(new StateTransition<Spell>()
                            {
                                Ability = ss.TrackedSpell,
                                TransitionProbability = 1d,
                                TransitionDuration = ss.TrackedSpell.CastTime,
                                TargetState = CastSpell(restoState, ss)
                            });
                            break;
                        }
                    }
                }

                if (!spellCast)
                {
                    if (restoState.TidalWavesBuff > 0)
                    {
                        SpellState ss = null;
                        if (_SequenceType == SequenceType.Burst)
                            ss = restoState.GetNonPrioritySpellStates().Where(i => i.TrackedSpell.ConsumesTidalWaves).OrderByDescending(i => i.TrackedSpell.EPS).First();
                        else
                            ss = restoState.GetNonPrioritySpellStates().Where(i => i.TrackedSpell.ConsumesTidalWaves).OrderByDescending(i => i.TrackedSpell.EPM).First();

                        transitions.Add(new StateTransition<Spell>()
                        {
                            Ability = ss.TrackedSpell,
                            TransitionProbability = 1d,
                            TransitionDuration = ss.TrackedSpell.CastTime,
                            TargetState = CastSpell(restoState, ss)
                        });
                    }
                    else
                    {
                        SpellState ss = null;
                        if (_SequenceType == SequenceType.Burst)
                            ss = restoState.GetNonPrioritySpellStates().OrderByDescending(i => i.TrackedSpell.EPS).First();
                        else
                            ss = restoState.GetNonPrioritySpellStates().OrderByDescending(i => i.TrackedSpell.EPM).First();

                        transitions.Add(new StateTransition<Spell>()
                        {
                            Ability = ss.TrackedSpell,
                            TransitionProbability = 1d,
                            TransitionDuration = ss.TrackedSpell.CastTime,
                            TargetState = CastSpell(restoState, ss)
                        });
                    }
                    int transitionCount = transitions.Count;
                    if (transitionCount > 1)
                        transitions.ForEach(i => i.TransitionProbability = (double)(1d / (double)transitionCount));
                }
            }

            return transitions;
        }

        private State<Spell> CastSpell(RestoShamState restoState, SpellState ss)
        {
            RestoShamState next = new RestoShamState(restoState);
            next.CastSpell(ss.TrackedSpell.SpellId, _MP5);

            // Does this state already exist?
            /*lock (_TargetStates)
            {
                foreach (RestoShamState rss in _TargetStates.Where(i => i.TidalWavesBuff == next.TidalWavesBuff && i.TemporaryBuffs.Count == next.TemporaryBuffs.Count && i.ManaPool == next.ManaPool && i.FightTime == next.FightTime))
                {
                    if (rss.StatesMatch(next))
                        return rss;
                }
                _TargetStates.Add(next);
            }*/
            return next;
        }
    }
}
