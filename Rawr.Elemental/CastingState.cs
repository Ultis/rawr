using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Sequencer
{
    public class CastingState
    {
        public float SpellHaste { get; set; }
        public float HasteRating { get; set; }
        public float SpellCrit { get; set; } // needed for procs
        public float SpellHit { get; set; } // needed for procs
        public float MPS { get; set; } 
        public float BonusSpellPower { get; set; } // only from procs
        public bool ElementalMastery { get; set; }
        
        public CastingState Clone()
        {
            CastingState clone = (CastingState)this.MemberwiseClone();
            return clone;
        }

        public static CastingState operator +(CastingState a, CastingState b)
        {
            CastingState c = a.Clone();
            c.SpellCrit += b.SpellCrit;
            c.SpellHaste += b.SpellHaste;
            c.SpellHit += b.SpellHit;
            c.MPS += b.MPS;
            c.HasteRating += b.HasteRating;
            c.BonusSpellPower += b.BonusSpellPower;
            c.ElementalMastery |= b.ElementalMastery;
            return c;
        }
    }
}
