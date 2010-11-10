using System;


namespace Rawr.ShadowPriest.Spells
{
    public class ShadowWordPain : Dot
    {
        /// <summary>
        /// Shadow Word Pain is a dot that lasts for 18 seconds.
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Mental Agility, Dark Evangelism, Darkness (to be handeled in Char stats), Improved Shadow Word:Pain, Shadowform, Pain And suffering, Shadowy Apparition, Harnessed Shadows
        /// Glyphs:
        /// Prime-Shadow Word: Pain
        //TODO: Pain and suffering, Shadowy Apparition, Get base Values from Beta, Harnessed Shadows (mastery)
        /// </summary>
        public ShadowWordPain() : base()
        { 
        }

        protected override void SetDotValues()
        {
            base.SetDotValues();

            debuffDuration = 21;
            tickPeriod = 3f;
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 221; //check
            baseMaxDamage = 221; //check
            spCoef = 1.5f / 3.5f / 2f; //Check
            dotSpCoef = .1f; //Check
            periodicTick = 556f / 4f; //Check
            periodicTicks = 6f; //Check
            periodicTickTime = 3f; //Check
            manaCost = 0.22f * Constants.BaseMana;
            shortName = "SW:P";
            name = "Shadow Word: Pain";
        }

        public override void Initialize(Rawr.ShadowPriest.Spells.ISpellArgs args)
        {
            //for reference
            //dotTick = totalCoef * (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + critModifier * CritChance)

            totalCoef += .01f * args.Talents.ImprovedShadowWordPain;
            manaCost *= 1 - .2f * args.Talents.MentalAgility;
            totalCoef += .01f + args.Talents.TwinDisciplines;
            totalCoef += .01f + args.Talents.Evangelism;
            totalCoef += .01f + args.Talents.ImprovedShadowWordPain;
            totalCoef += .01f + args.Talents.Shadowform;

            if (args.Talents.GlyphofShadowWordPain)
                periodicTick *= 1.1f; //Add 10% periodic damage
            
            ApplyDotHaste(args);
            base.Initialize(args);
        }

        #region hide
        public ShadowWordPain (ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static ShadowWordPain operator +(ShadowWordPain A, ShadowWordPain B)
        {
            ShadowWordPain C = (ShadowWordPain)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ShadowWordPain operator *(ShadowWordPain A, float b)
        {
            ShadowWordPain C = (ShadowWordPain)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion
    }
}
