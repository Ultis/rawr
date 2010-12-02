using System;
using System.Collections.Generic;
using System.Linq;
using Rawr.Base.Algorithms;

namespace Rawr.RestoSham.StateMachine
{
    public sealed class RestoShamState : State<Spell>
    {
        public int TidalWavesBuff { get; set; }
        private List<SpellState> _SpellStates;
        public List<TemporaryBuff> TemporaryBuffs { get; private set; }

        public SpellState GetSpellState(int spellId)
        {
            return _SpellStates.FirstOrDefault(i => i.TrackedSpell.SpellId == spellId);
        }

        public IOrderedEnumerable<SpellState> GetPrioritySpellStates()
        {
            return _SpellStates.Where(i => i.HasHot || i.HasCooldown).OrderBy(i => i.Priority);
        }

        public IEnumerable<SpellState> GetNonPrioritySpellStates()
        {
            return _SpellStates.Where(i => !i.HasHot && !i.HasCooldown);
        }

        public RestoShamState(List<Spell> availableSpells)
        {
            if (availableSpells == null)
                throw new ArgumentNullException("availableSpells");

            Index = 0;
            Name = "INITIAL";
            TidalWavesBuff = 0;
            TemporaryBuffs = new List<TemporaryBuff>();
            _SpellStates = new List<SpellState>(availableSpells.Count);

            for (int i = 0; i < availableSpells.Count; i++)
            {
                _SpellStates.Add(new SpellState(availableSpells[i], (byte)i));
            }
        }

        public RestoShamState(RestoShamState parentState)
        {
            TemporaryBuffs = new List<TemporaryBuff>();
            _SpellStates = new List<SpellState>(parentState._SpellStates.Count);

            foreach (SpellState ss in parentState._SpellStates)
            {
                _SpellStates.Add(ss.Clone());
            }
        }

        internal void CastSpell(int spellId, float ticks)
        {
            foreach (SpellState ss in _SpellStates)
            {
                if (ss.TrackedSpell.SpellId == spellId)
                    ss.Cast();
                ss.Advance(ticks);
            }
        }
        /*
        public static bool operator ==(RestoShamState x, RestoShamState y)
        {
            try
            {
                if (x.TidalWavesBuff != y.TidalWavesBuff)
                    return false;
                if (x._SpellStates.Count != y._SpellStates.Count)
                    return false;
                if (x.TemporaryBuffs.Count != y.TemporaryBuffs.Count)
                    return false;

                for (int i = 0; i < x.TemporaryBuffs.Count; i++)
                {
                    if (x.TemporaryBuffs[i].Name != y.TemporaryBuffs[i].Name)
                        return false;
                    if (x.TemporaryBuffs[1].CastCount != y.TemporaryBuffs[i].CastCount)
                        return false;
                    if (x.TemporaryBuffs[1].Duration != y.TemporaryBuffs[i].Duration)
                        return false;
                }

                for (int i = 0; i < x._SpellStates.Count; i++)
                {
                    if (x._SpellStates[i] != y._SpellStates[i])
                        return false;
                }

                return true;
            }
            catch { return false; }
        }

        public static bool operator !=(RestoShamState x, RestoShamState y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="o">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object o)
        {
            try
            {
                return (bool)(this == (RestoShamState)o);
            }
            catch
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return TidalWavesBuff.GetHashCode() + _SpellStates.GetHashCode() + TemporaryBuffs.GetHashCode();
        }
        */
        internal bool CooldownsMatch(RestoShamState next)
        {
            foreach (SpellState ss in GetPrioritySpellStates())
            {
                SpellState st = next.GetSpellState(ss.TrackedSpell.SpellId);
                if (st != ss)
                    return false;
            }
            return true;
        }
    }
}
