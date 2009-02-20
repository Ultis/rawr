using System;

namespace Rawr.Elemental.Sequencer
{
    public abstract class Spell
    {
        protected float baseMinDamage = 0f;
        protected float baseMaxDamage = 0f;
        protected float castTime = 0f;
        protected float periodicTick = 0f;
        protected float periodicTicks = 0f;
        protected float periodicTickTime = 3f;
        protected float manaCost = 0f;
        protected float gcd = 1.5f;
        protected float crit = 0f;
        protected float critModifier = 0.5f;
        protected float cooldown = 0f;
        protected float missChance = 0.17f;

        protected float totalCoef = 1f;
        protected float baseCoef = 1f;
        protected float spCoef = 0f;
        protected float dotBaseCoef = 1f;
        protected float dotSpCoef = 0f;
        protected float spellPower = 0f;

        public void ApplyEM(float modifier)
        {
            crit += modifier * .2f;
            manaCost *= 1 - modifier * .2f;
        }

        //public abstract void getCastInfo(CastingState state, out float timeToCast, out float timeAfterCast);
        //public abstract void cast(CastingState state, bool hasEM, out float manaCost);
        //public abstract void calculate(float hits, float crits, float bonusPower, float
    }

    public class Thunderstorm : Spell
    {
        public Thunderstorm(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 1450;
            baseMaxDamage = 1656;
            castTime = 0f;
            spCoef = .193f; // NOT CORRECT YET, assuming 1.5f/7f * 0.9f (aoe with additional effect)
            manaCost = 0f;
            cooldown = 45f;
            #endregion

            totalCoef *= 1f + .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            crit += .05f * shamanTalents.TidalMastery;
            spellPower += stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            totalCoef *= 1 + stats.LightningBoltDamageModifier / 100f;
        }
    }

    public class LightningBolt : Spell
    {
        private int ranksOverload = 0;

        public LightningBolt(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 715;
            baseMaxDamage = 815;
            castTime = 2.5f;
            spCoef = .7143f;
            manaCost = 0.1f * Constants.BaseMana;
            #endregion

            castTime -= .1f * shamanTalents.LightningMastery;
            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef *= 1f + .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            spCoef *= 1f + .02f * shamanTalents.Shamanism;
            crit += .05f * shamanTalents.TidalMastery;
            manaCost *= 1 - stats.LightningBoltCostReduction / 100f; // Set bonus
            spellPower += stats.LightningSpellPower; // Static effect
            spellPower += stats.SpellNatureDamageRating; // Static effect
            if (calcOpts.glyphOfLightningBolt) totalCoef *= 1.04f;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier; 
            totalCoef *= 1 + stats.LightningBoltDamageModifier / 100f;

            critModifier += .5f * stats.BonusSpellCritMultiplier;
            ranksOverload = shamanTalents.LightningOverload;
        }

        public void getCastInfo(CastingState state, out float timeToCast, out float timeAfterCast, out float expectedManaCost)
        {
            float haste = state.SpellHaste + state.HasteRating / 3278.998947f;
            float Gcd = (float)Math.Round(gcd / (1 + haste), 4);
            float CastTime = (float)Math.Round(castTime / (1 + haste), 4);

            timeToCast = CastTime;
            if (Gcd > 0)
            {
                if (Gcd < 1f) Gcd = 1f;
                Gcd -= CastTime;
                if (Gcd < 0) Gcd = 0;
            }
            timeAfterCast = Gcd;

            expectedManaCost = manaCost;
            if (state.ElementalMastery) expectedManaCost *= .8f;
        }

        public void cast(CastingState state, bool hasEM, out float ManaCost, out float Hit, out float Crit, out float Miss, out float BonusPower)
        {
            ManaCost = manaCost;
            Miss = .17f - state.SpellHit;
            if (Miss < 0) Miss = 0;
            Hit = 1f - Miss;
            Crit = state.SpellCrit + crit;
            if (state.ElementalMastery)
            {
                ManaCost *= .8f;
                Crit += .2f;
            } 
            if (Crit > 1f) Crit = 1f;
            BonusPower = state.BonusSpellPower;

            // Lightning overload
            // Hit = Hit + .04f * ranksOverload * Hit * Hit; // only overload on succesful hits
        }

            public float extraFromOverload(float hit, float crit)
        {
            return crit * (1 + .04f * ranksOverload * hit); // only overload on succesful hits
        }

        public float calculateDamage(float hits, float crits, float bonusPower, float basePower)
        {
            // apply lightning overload
            hits *= 1 + .04f * ranksOverload * .5f; // only half damage
            crits *= 1 + 0.04f * ranksOverload * .5f; // only half damage

            float damage = baseCoef * ((baseMinDamage + baseMaxDamage) / 2) * hits;
            damage += spCoef * (basePower * hits + bonusPower);
            damage *= 1 + critModifier * crits / hits;
            damage *= totalCoef;
            return damage;
        }
    }
    /*
    public class ChainLightning : Spell
    {
        public ChainLightning(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts, int additionalTargets)
        {
            #region Base Values
            baseMinDamage = 973;
            baseMaxDamage = 1111;
            castTime = 2f;
            spCoef = 2f / 3.5f;
            manaCost = 0.26f * Constants.BaseMana;
            cooldown = 6f;
            #endregion

            // jumps
            if (additionalTargets < 0) additionalTargets = 0;
            if (additionalTargets > 4) additionalTargets = 4;
            totalCoef *= new float[] { 1f, 1.7f, 2.19f, 2.533f, 2.7731f }[additionalTargets];

            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef *= 1f + .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            castTime -= .1f * shamanTalents.LightningMastery;
            cooldown -= new float[] { 0, .75f, 1.5f, 2.5f }[shamanTalents.StormEarthAndFire];
            // emulate lightning overload by increasing crit chance
            // totalCoef *= 1f + .04f * shamanTalents.LightningOverload * .5f;
            crit += .05f * shamanTalents.TidalMastery;
            spellPower += stats.LightningSpellPower + stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;

            base.Initialize(stats);
        }

        public void increaseCritFromOverload(int ranks)
        {
            crit *= (1f + .04f * ranks);
        }
    }*/

    public class LavaBurst : Spell
    {
        public LavaBurst(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts, float fs)
        {
            #region Base Values
            baseMinDamage = 1192;
            baseMaxDamage = 1518;
            castTime = 2f;
            spCoef = 2f / 3.5f;
            manaCost = 0.1f * Constants.BaseMana;
            cooldown = 8f;
            #endregion

            manaCost *= 1f - .02f * shamanTalents.Convection;
            totalCoef *= 1f + .01f * shamanTalents.Concussion;
            totalCoef *= 1f + .02f * shamanTalents.CallOfFlame;
            castTime -= .1f * shamanTalents.LightningMastery;
            spCoef += .04f * shamanTalents.Shamanism;
            // emulate lightning overload by increasing crit chance
            crit += .05f * shamanTalents.TidalMastery;
            critModifier += .5f * new float[] { 0f, 0.06f, 0.12f, 0.24f }[shamanTalents.LavaFlows];
            critModifier += .5f * stats.BonusLavaBurstCritDamage / 100f;
            baseMinDamage += stats.LavaBurstBonus;
            baseMaxDamage += stats.LavaBurstBonus;
            spellPower += stats.SpellFireDamageRating;
            if (calcOpts.glyphOfLava) spCoef += .1f;
            totalCoef *= 1 + stats.BonusFireDamageMultiplier;

            //base.Initialize(stats);

            crit = (1 - fs) * crit + fs;
        }
    }

    public class FlameShock : Spell
    {
        public FlameShock(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 500;
            baseMaxDamage = 500;
            spCoef = .2142f;
            dotSpCoef = .1f;
            periodicTick = 139f; // 556f / 4f;
            periodicTicks = 4f;
            manaCost = 0.17f * Constants.BaseMana;
            cooldown = 6f;
            #endregion

            totalCoef *= 1 + .01f * shamanTalents.Concussion;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            dotBaseCoef *= 1 + .2f * shamanTalents.StormEarthAndFire;
            dotSpCoef *= 1 + .2f * shamanTalents.StormEarthAndFire;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            spellPower += stats.SpellFireDamageRating;
            totalCoef *= 1 + stats.BonusFireDamageMultiplier;

            if (calcOpts.glyphOfFlameShock) periodicTicks += 2;
            if (calcOpts.glyphOfShocking) gcd -= 0.5f;

            //base.Initialize(stats);
        }
    }

    public class EarthShock : Spell
    {
        public EarthShock(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 849;
            baseMaxDamage = 895;
            spCoef = .3858f;
            manaCost = 0.18f * Constants.BaseMana;
            cooldown = 6f;
            #endregion

            totalCoef *= 1 + .01f * shamanTalents.Concussion;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            spellPower += stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;
            if (calcOpts.glyphOfShocking) gcd -= 0.5f;

            //base.Initialize(stats);
        }
    }

    public class FrostShock : Spell
    {
        public FrostShock(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            baseMinDamage = 802;
            baseMaxDamage = 848;
            spCoef = .3858f;
            manaCost = 0.18f * Constants.BaseMana;
            cooldown = 6f;
            #endregion

            totalCoef *= 1 + .01f * shamanTalents.Concussion;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            spellPower += stats.SpellFrostDamageRating;
            totalCoef *= 1 + stats.BonusFrostDamageMultiplier;
            if (calcOpts.glyphOfShocking) gcd -= 0.5f;

            //base.Initialize(stats);
        }
    }

    public class ElementalMastery : Spell
    {
        public ElementalMastery(Stats stats, ShamanTalents shamanTalents, CalculationOptionsElemental calcOpts)
        {
            #region Base Values
            missChance = 0;
            cooldown = 180f - (calcOpts.glyphOfElementalMastery ? 30f : 0f);
            gcd = 0; // no global cooldown ;)
            #endregion

            //base.Initialize(stats);
        }
    }

}