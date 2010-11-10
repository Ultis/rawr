using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindSear : DD
    {
        /// <summary>
        /// Mind Sear is a Cahnnelled AOE.
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Darkness (to be handeled in Char stats), Shadowform, Twisted Faith
        //TODO: ALL
        /// </summary>
        public MindSear() : base()
        { 
        }
        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            manaCost = 0.28f * Constants.BaseMana;
            shortName = "Sear";
            name = "Mind Sear";
        }
                #region hide
        public MindSear(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static MindSear operator +(MindSear A, MindSear B)
        {
            MindSear C = (MindSear)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static MindSear operator *(MindSear A, float b)
        {
            MindSear C = (MindSear)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion

    }
}
