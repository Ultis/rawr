using System;

namespace Rawr.RestoSham.StateMachine
{
    public sealed class SpellState
    {
        private Decimal _Cooldown = 0M;
        private Decimal _ActiveOnTarget = 0M;
        public byte Priority { get; private set; }

        public bool HasCooldown { get { return TrackedSpell.HasCooldown; } }
        public bool HasHot { get { return TrackedSpell is Hot; } }
        public Spell TrackedSpell { get; private set; }
        public bool IsOnCooldown { get { return (_Cooldown > 0M); } }
        public bool IsActive { get { return (_ActiveOnTarget > 0M); } }
        
        public SpellState(Spell spellToTrack, byte priority)
        {
            TrackedSpell = spellToTrack;
            _Cooldown = 0M;
            _ActiveOnTarget = 0M;
            Priority = priority;
        }
        public void Advance(float ticks)
        {
            Decimal t = Decimal.Round(Convert.ToDecimal(ticks), 1);
            if (IsOnCooldown)
            {
                _Cooldown -= t;
                if (_Cooldown < 0M)
                    _Cooldown = 0M;
            }
            if (IsActive)
            {
                _ActiveOnTarget -= t;
                if (_ActiveOnTarget < 0M)
                    _ActiveOnTarget = 0M;
            }
        }
        public void Cast()
        {
            _Cooldown = Convert.ToDecimal(TrackedSpell.Cooldown);
            Hot h = TrackedSpell as Hot;
            if (h != null)
                _ActiveOnTarget = Convert.ToDecimal(h.HotDuration);
        }

        public static bool operator ==(SpellState x, SpellState y)
        {
            if (x.TrackedSpell.SpellId != y.TrackedSpell.SpellId)
                return false;
            if (x._Cooldown != y._Cooldown)
                return false;
            if (x._ActiveOnTarget != y._ActiveOnTarget)
                return false;

            return true;
        }

        public static bool operator !=(SpellState x, SpellState y)
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
                return (bool)(this == (SpellState)o);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return TrackedSpell.SpellId.GetHashCode() + _Cooldown.GetHashCode() + _ActiveOnTarget.GetHashCode();
        }

        public SpellState Clone()
        {
            SpellState clone = new SpellState(TrackedSpell, Priority);
            clone._ActiveOnTarget = _ActiveOnTarget;
            clone._Cooldown = _Cooldown;
            return clone;
        }
    }
}
