using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindBlast : Spell
    {
        public MindBlast() : base()
        {
        }
        /// <summary>
        /// Mind Blast is a Direct Damge Spell
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Archangel, Improved Mind Blast, Shadowform, Paralysis
        //TODO: Get base Values from Beta, Archangel, Paralysis (though is only a stun so may not be needed)
        /// </summary>
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 1028;
            baseMaxDamage = 1086;
            baseCastTime = 1.5f;
            spCoef = 0f;
            manaCost = 0.17f * Constants.BaseMana;
            shortName = "MB";
        }

        public override void Initialize(ISpellArgs args)
        {
            totalCoef += .01f * args.Talents.TwinDisciplines;
            cooldown -= .5f * args.Talents.ImprovedMindBlast;
            totalCoef += .01f + args.Talents.Shadowform;

            base.Initialize(args);
        }

        #region hide
        public MindBlast(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static MindBlast operator +(MindBlast A, MindBlast B)
        {
            MindBlast C = (MindBlast)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static MindBlast operator *(MindBlast A, float b)
        {
            MindBlast C = (MindBlast)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion
    }
}
