using System;


namespace Rawr.ShadowPriest.Spells
{
    public class ShadowWordPain : DoTSpell
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

            tickHasteCoEf = 0.1666666f;
            debuffDurationBase = 18f;

        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseScaling = 0.233999999737739f;
            manaCost = 0.22f * Constants.BaseMana;
            shortName = "SW:P";
            name = "Shadow Word: Pain";
        }

        public override void Initialize(Rawr.ShadowPriest.Spells.ISpellArgs args)
        {            
            base.Initialize(args);
            //for reference
            //dotTick = totalCoef * (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + critModifier * CritChance)

            //totalCoef += .01f * args.Talents.ImprovedShadowWordPain;
            manaCost *= 1 - .2f * args.Talents.MentalAgility;
            //totalCoef += .01f + args.Talents.TwinDisciplines;
            //totalCoef += .01f + args.Talents.Evangelism;
            //totalCoef += .01f + args.Talents.ImprovedShadowWordPain;
            //totalCoef += .01f + args.Talents.Shadowform;

            //if (args.Talents.GlyphofShadowWordPain)
                //periodicTick *= 1.1f; //Add 10% periodic damage

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
