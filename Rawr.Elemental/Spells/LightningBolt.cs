using System;

namespace Rawr.Elemental.Spells
{
    public class LightningBolt : Spell, ILightningOverload
    {
        public LightningBolt() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 719;
            baseMaxDamage = 819;
            baseCastTime = 2.5f;
            castTime = 2.5f;
            spCoef = 2.5f / 3.5f;
            lspCoef = spCoef;
            loCoef = spCoef / 2f;
            manaCost = 0.1f * Constants.BaseMana;
            lightningSpellpower = 0f;
            shortName = "LB";
        }

        public override void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            castTime -= .1f * shamanTalents.LightningMastery;
            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef += .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            spCoef += .03f * shamanTalents.Shamanism;
            loCoef += .03f * shamanTalents.Shamanism;
            crit += .05f * shamanTalents.TidalMastery;
            manaCost *= 1 - stats.LightningBoltCostReduction / 100f; // T7 2 piece
            spellPower += stats.SpellNatureDamageRating; // Nature SP
            lightningSpellpower += stats.LightningSpellPower; // Totem (relic) is not affected by shamanism
            if (shamanTalents.GlyphofLightningBolt) totalCoef += .04f;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            totalCoef *= 1 + stats.LightningBoltDamageModifier / 100f; // T6 4 piece
            
            lightningOverload = shamanTalents.LightningOverload;

            base.Initialize(stats, shamanTalents);
            critModifier *= 1 + stats.LightningBoltCritDamageModifier; // T8 4 piece
        }

        public LightningBolt(Stats stats, ShamanTalents shamanTalents)
            : this()
        {
            Initialize(stats, shamanTalents);
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
            get { return Math.Min(1f, CritChance * (1f + LOChance())); }
        }

        public override float MinHit
        {
            get { return totalCoef * (baseMinDamage * baseCoef + spellPower * spCoef + lightningSpellpower * lspCoef); }
        }

        public override float MaxHit
        {
            get { return totalCoef * (baseMaxDamage * baseCoef + spellPower * spCoef + lightningSpellpower * lspCoef); }
        }

        public float LOChance()
        {
            return .11f * lightningOverload;
        }

        public override float TotalDamage
        {
            get { return base.TotalDamage + LOChance() * LightningOverloadDamage(); }
        }

        public override float DirectDpS
        {
            get { return (AvgDamage + LOChance() * LightningOverloadDamage()) / CastTime; }
        }

        public float LightningOverloadDamage()
        {
            return totalCoef * ((baseMinDamage + baseMaxDamage) / 4f * baseCoef + spellPower * loCoef + lightningSpellpower * lspCoef) * (1 + CritChance * critModifier);;
        }
    }
}