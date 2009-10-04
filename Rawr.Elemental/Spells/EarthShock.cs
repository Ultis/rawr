using System;

namespace Rawr.Elemental.Spells
{
public class EarthShock : Shock
    {
        public EarthShock()
        {
            #region Base Values
            baseMinDamage = 854;
            baseMaxDamage = 900;
            spCoef = 1.5f / 3.5f * .9f;
            manaCost = .18f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "ES";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            totalCoef += .01f * shamanTalents.Concussion;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            spellPower += stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            if (shamanTalents.GlyphofShocking)
                gcd -= .5f;

            base.Initialize(stats, shamanTalents);
        }

        public EarthShock(Stats stats, ShamanTalents shamanTalents) : this()
        {
            Initialize(stats, shamanTalents);
        }

        public static EarthShock operator +(EarthShock A, EarthShock B)
        {
            EarthShock C = (EarthShock)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static EarthShock operator *(EarthShock A, float b)
        {
            EarthShock C = (EarthShock)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

    }
}