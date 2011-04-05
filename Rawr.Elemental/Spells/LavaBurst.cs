using System;

namespace Rawr.Elemental.Spells
{
    public class LavaBurst : Spell
    {
        private float fs = 0f;

        public LavaBurst() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 1267;
            baseMaxDamage = 1615;
            baseCastTime = 2f;
            castTime = 2f;
            spCoef = 2f / 3.5f;
            manaCost = .1f * Constants.BaseMana;
            cooldown = 8f;
            shortName = "LvB";
        }

        public override void Initialize(ISpellArgs args)
        {
            Initialize(args, fs);
        }

        public void Initialize(ISpellArgs args, float fs)
        {
            castTime -= .5f;
            spCoef += .2f;
            crit += args.Talents.Acuity;            
            manaCost *= 1f - .05f * args.Talents.Convection;
            manaCost -= args.Stats.NatureSpellsManaCostReduction;
            totalCoef += .02f * args.Talents.Concussion;
            totalCoef += .01f * args.Talents.ElementalPrecision;
            totalCoef += .05f * args.Talents.CallOfFlame;
            totalCoef += args.Stats.BonusDamageMultiplierLavaBurst; // t9 4 piece
            critModifier += new float[] { 0f, 0.06f, 0.12f, 0.24f }[args.Talents.LavaFlows];
            spellPower += args.Stats.SpellFireDamageRating;
            if (args.Talents.GlyphofLava)
                spCoef += .1f;
            totalCoef *= 1 + args.Stats.BonusFireDamageMultiplier;

            base.Initialize(args);

            this.fs = fs;
            crit = (1 - fs) * crit + fs;
        }

        public LavaBurst(ISpellArgs args, float fs)
            : this()
        {
            this.fs = fs;
            Initialize(args, fs);
        }

        public static LavaBurst operator +(LavaBurst A, LavaBurst B)
        {
            LavaBurst C = (LavaBurst)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static LavaBurst operator *(LavaBurst A, float b)
        {
            LavaBurst C = (LavaBurst)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

    }
}