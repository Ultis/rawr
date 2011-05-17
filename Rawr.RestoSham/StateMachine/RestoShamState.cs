using System;
using System.Collections.Generic;
using System.Linq;
using Rawr.Base.Algorithms;

namespace Rawr.RestoSham.StateMachine
{
    internal sealed class RestoShamState : State<Spell>, IEquatable<RestoShamState>
    {
        private Guid _StateId = Guid.NewGuid();

        internal int TidalWavesBuff { get; set; }
        internal List<SpellState> SpellStates { get; private set; }
        internal List<TemporaryBuff> TemporaryBuffs { get; private set; }
        internal int ManaPool { get; private set; }
        internal float FightTime { get; private set; }

        private SpellState GetSpellState(int spellId)
        {
            return SpellStates.FirstOrDefault(i => i.TrackedSpell.SpellId == spellId);
        }

        internal IOrderedEnumerable<SpellState> GetPrioritySpellStates()
        {
            return SpellStates.Where(i => (i.HasHot || i.HasCooldown) && i.TrackedSpell.ManaCost < ManaPool).OrderBy(i => i.Priority);
        }

        internal IEnumerable<SpellState> GetNonPrioritySpellStates()
        {
            return SpellStates.Where(i => !i.HasHot && !i.HasCooldown && i.TrackedSpell is HealingSpell && i.TrackedSpell.ManaCost < ManaPool);
        }

        internal RestoShamState(List<Spell> availableSpells, int mana)
        {
            if (availableSpells == null)
                throw new ArgumentNullException("availableSpells");

            Index = 0;
            Name = "INITIAL";
            TidalWavesBuff = 0;
            TemporaryBuffs = new List<TemporaryBuff>();
            SpellStates = new List<SpellState>(availableSpells.Count);
            ManaPool = mana;
            FightTime = 0f;

            for (int i = 0; i < availableSpells.Count; i++)
            {
                SpellStates.Add(new SpellState(availableSpells[i], (byte)i));
            }
        }

        internal RestoShamState(RestoShamState parentState)
        {
            TemporaryBuffs = new List<TemporaryBuff>();
            SpellStates = new List<SpellState>(parentState.SpellStates.Count);
            TidalWavesBuff = parentState.TidalWavesBuff;
            ManaPool = parentState.ManaPool;
            FightTime = parentState.FightTime;

            foreach (SpellState ss in parentState.SpellStates)
            {
                SpellStates.Add(ss.Clone());
            }
        }

        internal void CastSpell(int spellId, int mp5)
        {
            float castTime = 10f;
            if (spellId > 1)
            {
                Spell spellCastObject = GetModifiedSpell(spellId);

                if (spellCastObject.TemporaryBuffs != null && spellCastObject.TemporaryBuffs.Count > 0)
                    TemporaryBuffs.AddRange(spellCastObject.TemporaryBuffs);

                if (spellCastObject.ConsumesTidalWaves && TidalWavesBuff > 0)
                    TidalWavesBuff--;
                else if (spellCastObject.ProvidesTidalWaves)
                    TidalWavesBuff = 2;

                ManaPool -= spellCastObject.ManaCost;
                ManaPool += spellCastObject.ManaBack;
                castTime = spellCastObject.CastTime;
            }
            else { TidalWavesBuff = 0; }

            ManaPool += (int)(mp5 / 5 * castTime);
            
            foreach (SpellState ss in SpellStates)
            {
                if (ss.TrackedSpell.SpellId == spellId)
                {
                    if (ss.TrackedSpell.Instant)
                    {
                        ss.Cast();
                        ss.Advance(castTime);
                    }
                    else
                    {
                        ss.Advance(castTime);
                        ss.Cast();
                    }
                }
                else
                    ss.Advance(castTime);
            }

            FightTime += castTime;
        }

        internal Spell GetModifiedSpell(int spellId)
        {
            SpellState castSpell = GetSpellState(spellId);
            return GetModifiedSpell(castSpell);
        }

        private Spell GetModifiedSpell(SpellState castSpell)
        {
            Spell spellCastObject = castSpell.TrackedSpell.Clone();

            if (spellCastObject.ConsumesTidalWaves && TidalWavesBuff > 0)
            {
                if (spellCastObject.TidalWavesBuff.Stats.SpellHaste > 0f)
                    spellCastObject.CastTimeReduction += (spellCastObject.TidalWavesBuff.Stats.SpellHaste * spellCastObject.CastTime);
                else if (spellCastObject.TidalWavesBuff.Stats.SpellCrit > 0f)
                    spellCastObject.CritRateModifier += spellCastObject.TidalWavesBuff.Stats.SpellCrit;

                spellCastObject.Calculate();
            }
            return spellCastObject;
        }

        internal bool StatesMatch(RestoShamState other)
        {
            foreach (SpellState ss in GetPrioritySpellStates())
            {
                SpellState st = other.GetSpellState(ss.TrackedSpell.SpellId);
                if (st != ss)
                    return false;
            }
            return true;
        }

        public bool Equals(RestoShamState other)
        {
            return this._StateId == other._StateId;
        }

        public void GenerateStateName()
        {
            string n = string.Format("{0}:{1}", TidalWavesBuff, ManaPool);
            foreach (SpellState ss in SpellStates.Where(i => i.HasCooldown).OrderBy(i => i.TrackedSpell.SpellId))
            {
                n = string.Format("{0}:{1}", n, ss.IsOnCooldown ? 0 : 1);
            }
            this.Name = n;
        }
    }
}
