using System;

namespace Rawr.ShadowPriest.Spells
{

    public class Penance : Spell
    {
        public Penance()
            : base()
        {
        }
        /// <summary>
        /// Penance is
        /// It Benifits from:
        /// Talents:
        /// 
        /// Glyphs:
        /// 
        //TODO: All
        /// </summary>
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 1028;
            baseMaxDamage = 1086;
            baseCastTime = 1.5f;
            spCoef = 0f;
            manaCost = 0.17f * Constants.BaseMana;
            shortName = "Pen";
            name = "Penance";

        }

        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }

        #region hide
        public Penance(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static Penance operator +(Penance A, Penance B)
        {
            Penance C = (Penance)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static Penance operator *(Penance A, float b)
        {
            Penance C = (Penance)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion
    }
}
