using System;

namespace Rawr.Elemental.Spells
{
public class ChainLightning : Spell, ILightningOverload
    {
        private int additionalTargets = 0;
        private float loCoef, lightningSpellpower = 0f, lspCoef;
        private float loChance;

        public ChainLightning() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 1020;
            baseMaxDamage = 1165;
            baseCastTime = 2f;
            castTime = 2f;
            spCoef = 2f / 3.5f;
            lspCoef = spCoef;
            loCoef = spCoef / 2f;
            manaCost = 0.26f * Constants.BaseMana;
            cooldown = 3f;
            lightningSpellpower = 0f;
        }

        public override void Initialize(ISpellArgs args)
        {
            additionalTargets = args.AdditionalTargets;
            // jumps
            if (additionalTargets < 0)
                additionalTargets = 0;
            
            spellPower += args.Stats.SpellNatureDamageRating;
            totalCoef *= 1 + args.Stats.BonusNatureDamageMultiplier;

            if (additionalTargets > 4)
                additionalTargets = 4;
            if (!args.Talents.GlyphofChainLightning && additionalTargets > 2)
                additionalTargets = 2;
            totalCoef *= new float[] { 1f, 1.7f, 2.19f, 2.533f, 2.7731f }[additionalTargets];

            loChance = LOChance(args.Stats.MasteryRating);
            cooldown = 3f;
            manaCost *= 1f - (0.05f * args.Talents.Convection);
            manaCost -= ((args.Stats.Mana * (.01f * args.Talents.RollingThunder)) * .6f) * (additionalTargets + 1);
            manaCost -= args.Stats.NatureSpellsManaCostReduction;
            totalCoef += .02f * args.Talents.Concussion;
            spCoef += .32f;
            loCoef += .2f;
            crit += .01f * args.Talents.Acuity;
            totalCoef += .01f * args.Talents.ElementalPrecision;

            shortName = "CL" + (1 + additionalTargets);

            base.Initialize(args);
        }

        public ChainLightning(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public int AdditionalTargets
        {
            get { return additionalTargets; }
        }

        public static ChainLightning operator +(ChainLightning A, ChainLightning B)
        {
            ChainLightning C = (ChainLightning)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ChainLightning operator *(ChainLightning A, float b)
        {
            ChainLightning C = (ChainLightning)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

        private int lightningOverload = 0;

        public override float CCCritChance
        {
            get { return Math.Min(1f, CritChance * (1f + AdditionalTargets) * (1f + loChance)); }
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
            return ((.16f + (masteryRating * .02f) * lightningOverload / 3f) * (1 + AdditionalTargets));
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
            return totalCoef * (((baseMinDamage + baseMaxDamage) / 2f) / 1.5f * baseCoef + spellPower * loCoef + lightningSpellpower * lspCoef) * (1 + CritChance * critModifier);
        }
    }
}