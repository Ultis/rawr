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

            baseMinDamage = 1192;
            baseMaxDamage = 1518;
            baseCastTime = 2f;
            castTime = 2f;
            spCoef = 2f / 3.5f;
            manaCost = .1f * Constants.BaseMana;
            cooldown = 8f;
            shortName = "LvB";
        }

        public override void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            Initialize(stats, shamanTalents, fs);
        }

        public void Initialize(Stats stats, ShamanTalents shamanTalents, float fs)
        {
            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef += .01f * shamanTalents.Concussion;
            totalCoef += .02f * shamanTalents.CallOfFlame;
            totalCoef += stats.BonusLavaBurstDamageMultiplier; // t9 4 piece
            castTime -= .1f * shamanTalents.LightningMastery;
            spCoef += .04f * shamanTalents.Shamanism;
            critModifier += new float[] { 0f, 0.06f, 0.12f, 0.24f }[shamanTalents.LavaFlows];
            critModifier += stats.BonusLavaBurstCritDamage / 100f; // t7 4 piece
            baseMinDamage += stats.LavaBurstBonus; // Totem (relic)
            baseMaxDamage += stats.LavaBurstBonus; // Totem (relic) 
            spellPower += stats.SpellFireDamageRating;
            if (shamanTalents.GlyphofLava)
                spCoef += .1f;
            totalCoef *= 1 + stats.BonusFireDamageMultiplier;

            base.Initialize(stats, shamanTalents);

            this.fs = fs;
            crit = (1 - fs) * crit + fs;
        }

        public LavaBurst(Stats stats, ShamanTalents shamanTalents, float fs)
            : this()
        {
            this.fs = fs;
            Initialize(stats, shamanTalents, fs);
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