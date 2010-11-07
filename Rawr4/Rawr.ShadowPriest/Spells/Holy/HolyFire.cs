using System;

namespace Rawr.ShadowPriest.Spells
{
    public class HolyFire : Spell
    {
        public HolyFire()
            : base()
        {
        }
        /// <summary>
        /// HolyFire is
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
            shortName = "HF";
            name = "Holy Fire";

        }

        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }

        #region hide
        public HolyFire(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static HolyFire operator +(HolyFire A, HolyFire B)
        {
            HolyFire C = (HolyFire)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static HolyFire operator *(HolyFire A, float b)
        {
            HolyFire C = (HolyFire)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion
    }
}
