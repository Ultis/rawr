using System;

namespace Rawr.Elemental.Spells
{
public class FrostShock : Shock
    {
        public FrostShock() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 848;
            baseMaxDamage = 897;
            spCoef = 1.5f / 3.5f * .9f;
            manaCost = .18f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "FrS";
        }

        public override void Initialize(ISpellArgs args)
        {
            totalCoef += .01f * args.Talents.ElementalPrecision;
            totalCoef += .02f * args.Talents.Concussion;
            manaCost *= 1 - .05f * args.Talents.Convection;
            cooldown -= .5f * args.Talents.Reverberation;
            crit += .01f * args.Talents.Acuity;
            spellPower += args.Stats.SpellFrostDamageRating;
            totalCoef *= 1 + args.Stats.BonusFrostDamageMultiplier;
            if (args.Talents.GlyphofShocking)
                gcd -= .5f;

            base.Initialize(args);
        }

        public FrostShock(ISpellArgs args)
            : this()
        {
            Initialize(args);
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