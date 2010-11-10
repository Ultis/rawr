using System;

namespace Rawr.ShadowPriest.Spells
{
    public class ShadowFiend : DD
    {
        public ShadowFiend()
            : base()
        {
        }
        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            shortName = "SF";
            name = "Shadow Fiend";
            cooldown = 180f;
        }
        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }
        
        #region hide
        public ShadowFiend(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static ShadowFiend operator +(ShadowFiend A, ShadowFiend B)
        {
            ShadowFiend C = (ShadowFiend)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ShadowFiend operator *(ShadowFiend A, float b)
        {
            ShadowFiend C = (ShadowFiend)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion

    }
}
