using System;

namespace Rawr.Elemental.Spells
{
public class ElementalMastery : Spell
    {
        public ElementalMastery()
        {
            #region Base Values
            missChance = 0;
            cooldown = 180f;
            gcd = 0; // no global cooldown ;)
            shortName = "EM";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            if (shamanTalents.GlyphofElementalMastery)
                cooldown -= 30f;

            base.Initialize(stats, shamanTalents);
        }

        public ElementalMastery(Stats stats, ShamanTalents shamanTalents)
            : this()
        {
            Initialize(stats, shamanTalents);
        }


        public static ElementalMastery operator +(ElementalMastery A, ElementalMastery B)
        {
            ElementalMastery C = (ElementalMastery)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ElementalMastery operator *(ElementalMastery A, float b)
        {
            ElementalMastery C = (ElementalMastery)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
    }
}