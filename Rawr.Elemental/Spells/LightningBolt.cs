using System;

namespace Rawr.Elemental.Spells
{
    public class LightningBolt : Spell, ILightningOverload
    {
        private float loChance;
        
        public LightningBolt() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 719;
            baseMaxDamage = 821;
            baseCastTime = 2.5f;
            castTime = 2.5f;
            spCoef = 2.5f / 3.5f;
            lspCoef = spCoef;
            loCoef = spCoef / 1.5f;
            manaCost = 0.06f * Constants.BaseMana;
            lightningSpellpower = 0f;
            shortName = "LB";
        }

        public override void Initialize(ISpellArgs args)
        {
            castTime -= .5f;
            spCoef += .2f;
            loCoef += .2f;
            loChance = LOChance(args.Stats.MasteryRating);

            manaCost *= 1f - (0.05f * args.Talents.Convection);
            manaCost -= (args.Stats.Mana * (.01f * args.Talents.RollingThunder)) * .6f;
            totalCoef += .02f * args.Talents.Concussion;
            spCoef += .2f;
            loCoef += .2f;
            crit += .01f * args.Talents.Acuity;
            totalCoef += .01f * args.Talents.ElementalPrecision;

            spellPower += args.Stats.SpellNatureDamageRating; // Nature SP
            if (args.Talents.GlyphofLightningBolt) totalCoef += .04f;
            totalCoef *= 1 + args.Stats.BonusNatureDamageMultiplier;

            base.Initialize(args);
        }

        public LightningBolt(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static LightningBolt operator +(LightningBolt A, LightningBolt B)
        {
            LightningBolt C = (LightningBolt)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static LightningBolt operator *(LightningBolt A, float b)
        {
            LightningBolt C = (LightningBolt)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

        private int lightningOverload = 0;
        private float loCoef, lightningSpellpower = 0f, lspCoef;

        public override float CCCritChance
        {
            get { return Math.Min(1f, CritChance * (1f + loChance)); }
        }

        public override float MinHit
        {
            get { return totalCoef * (baseMinDamage * baseCoef + spellPower * spCoef + lightningSpellpower * lspCoef); }
        }

        public override float MaxHit
        {
            get { return totalCoef * (baseMaxDamage * baseCoef + spellPower * spCoef + lightningSpellpower * lspCoef); }
        }

        public float LOChance(float masteryRating)
        {
            return (masteryRating * .02f) * lightningOverload;
        }

        public override float TotalDamage
        {
            get { return base.TotalDamage + loChance * LightningOverloadDamage(); }
        }

        public override float DirectDpS
        {
            get { return (AvgDamage + loChance * LightningOverloadDamage()) / CastTime; }
        }

        public float LightningOverloadDamage()
        {
            return totalCoef * ((baseMinDamage + baseMaxDamage) / 4f * baseCoef + spellPower * loCoef + lightningSpellpower * lspCoef) * (1 + CritChance * critModifier);;
        }
    }
}