using System;

namespace Rawr.Elemental.Spells
{
public class EarthShock : Shock
    {
        public EarthShock() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();
            baseMinDamage = 906;
            baseMaxDamage = 955;
            spCoef = 1.5f / 3.5f * .9f;
            manaCost = .18f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "ES";
        }

        public override void Initialize(ISpellArgs args)
        {
            crit += .01f * args.Talents.Acuity;
            manaCost *= 1f - .05f * args.Talents.Convection;
            manaCost -= args.Stats.NatureSpellsManaCostReduction;
            totalCoef += .01f * args.Talents.Concussion;
            cooldown -= .05f * args.Talents.Reverberation;
            totalCoef += .01f * args.Talents.ElementalPrecision;

            spellPower += args.Stats.SpellNatureDamageRating;
            totalCoef *= 1 + args.Stats.BonusNatureDamageMultiplier;
            if (args.Talents.GlyphofShocking)
                gcd -= .5f;

            base.Initialize(args);
        }

        public EarthShock(ISpellArgs args) : this()
        {
            Initialize(args);
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