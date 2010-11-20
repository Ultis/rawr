using System;

namespace Rawr.ShadowPriest.Spells
{
    public class ShadowWordDeath : DD
    {
        public ShadowWordDeath()
            : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseScaling = 0.319000005722046f;
            manaCost = 0.12f * Constants.BaseMana;
            cooldown = 10f;
            shortName = "SW:D";
            name = "Shadow Word: Death";
        }
        public override float SpellPowerCoef
        {
            get
            {
                return 0.282000005245209f;
            }
        }
        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }
                #region hide
        public ShadowWordDeath(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        public static ShadowWordDeath operator +(ShadowWordDeath A, ShadowWordDeath B)
        {
            ShadowWordDeath C = (ShadowWordDeath)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ShadowWordDeath operator *(ShadowWordDeath A, float b)
        {
            ShadowWordDeath C = (ShadowWordDeath)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
        #endregion

    }
}
