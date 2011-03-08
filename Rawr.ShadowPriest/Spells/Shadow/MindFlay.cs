using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindFlay : DoTSpell
    {
        /// <summary>
        /// Mind Flay is a dot that lasts 3 seconds.
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Dark Evangelism (Maybe), Archangel, Darkness (to be handeled in Char stats), Shadowform, Pain And suffering, Sin and punishment, Harnessed Shadows
        /// Glyphs:
        /// Prime-Mind Flay
        //TODO: Archangel, Harnessed Shadows (Mastery), Sin and Punishment (cooldown of SF), Pain and suffering (refresh of SW:P)
        /// </summary>
        public MindFlay()
            : base()
        {
        }
        protected override void SetDotValues()
        {
            base.SetDotValues();

            tickHasteCoEf = 0f;
            debuffDurationBase = 3f;

        }


        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseScaling = 0.177000001072884f;
            manaCost = 0.08f * Constants.BaseMana;
            shortName = "MF";
            name = "Mind Flay";
        }
        public override void Initialize(ISpellArgs args)
        {
            //for reference
            //dotTick = totalCoef * (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + critModifier * CritChance)

            //totalCoef += .01f + args.Talents.TwinDisciplines;
            //totalCoef += .01f + args.Talents.Evangelism;
            //totalCoef += .01f + args.Talents.Shadowform;

           //if (args.Talents.GlyphofMindFlay)
                //periodicTick *= 1.1f; //Add 10% periodic damage

                base.Initialize(args);
        }
        public override float SpellPowerCoef
        {
            get
            {
                return 1.098f;
            }
        }

        #region hide
        public MindFlay(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static MindFlay operator +(MindFlay A, MindFlay B)
        {
            MindFlay C = (MindFlay)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static MindFlay operator *(MindFlay A, float b)
        {
            MindFlay C = (MindFlay)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion

    }
}

