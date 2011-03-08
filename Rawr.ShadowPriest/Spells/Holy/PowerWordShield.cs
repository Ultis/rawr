using System;

namespace Rawr.ShadowPriest.Spells
{
    public class PowerWordShield : DD
    {
         public PowerWordShield()
            : base()
        {
        }
        /// <summary>
        /// PowerWordShield is
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

            manaCost = 0.17f * Constants.BaseMana;
            shortName = "PW:D";
            name = "Power Word: Shield";

        }

        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }

        #region hide
        public PowerWordShield(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static PowerWordShield operator +(PowerWordShield A, PowerWordShield B)
        {
            PowerWordShield C = (PowerWordShield)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static PowerWordShield operator *(PowerWordShield A, float b)
        {
            PowerWordShield C = (PowerWordShield)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion
    }
}
