using System;

namespace Rawr.Elemental.Spells
{
public class FrostShock : Shock
    {
        public FrostShock()
        {
            #region Base Values
            baseMinDamage = 802;
            baseMaxDamage = 848;
            spCoef = 1.5f / 3.5f * .9f;
            manaCost = .18f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "FrS";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            totalCoef += .01f * shamanTalents.Concussion;
            spCoef *= 1f + .1f * shamanTalents.BoomingEchoes;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            cooldown -= 1f * shamanTalents.BoomingEchoes;
            spellPower += stats.SpellFrostDamageRating;
            totalCoef *= 1 + stats.BonusFrostDamageMultiplier;
            if (shamanTalents.GlyphofShocking)
                gcd -= .5f;

            base.Initialize(stats, shamanTalents);
        }

        public FrostShock(Stats stats, ShamanTalents shamanTalents)
            : this()
        {
            Initialize(stats, shamanTalents);
        }

        public static FrostShock operator +(FrostShock A, FrostShock B)
        {
            FrostShock C = (FrostShock)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static FrostShock operator *(FrostShock A, float b)
        {
            FrostShock C = (FrostShock)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
    }
}