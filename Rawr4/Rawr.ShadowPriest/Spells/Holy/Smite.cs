using System;

namespace Rawr.ShadowPriest.Spells
{
    public class Smite : Spell
    {
        public Smite()
            : base()
        {
        }
        /// <summary>
        /// Smite is
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
            shortName = "SM";
            name = "Smite";

        }

        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }

        #region hide
        public Smite(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static Smite operator +(Smite A, Smite B)
        {
            Smite C = (Smite)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static Smite operator *(Smite A, float b)
        {
            Smite C = (Smite)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion
    }
}
