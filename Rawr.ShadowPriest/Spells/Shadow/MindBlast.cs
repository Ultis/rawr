using System;

namespace Rawr.ShadowPriest.Spells
{
    public class MindBlast : DD
    {
        public MindBlast() : base()
        {
        }
        /// <summary>
        /// Mind Blast is a Direct Damge Spell
        /// It Benifits from:
        /// Talents:
        /// Twin Disciplines, Archangel, Improved Mind Blast, Shadowform, Paralysis
        /// </summary>
        //TODO: Get base Values from Beta, Archangel, Paralysis (though is only a stun so may not be needed)
        protected override void SetDDValues()
        {
            base.SetDDValues();

            baseSpread = 0.0549999997019768f;
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseScaling = 1.11899995803833f;
            castTimeBase = 1.5f;
            cooldown = 8f;
            manaCost = 0.17f * Constants.BaseMana;
            shortName = "MB";
            name = "Mind Blast";
        }

        public override float SpellPowerCoef
        { get { return 0.428999990224838f; } }

        public override void Initialize(ISpellArgs args)
        {
            //totalCoef += .01f * args.Talents.TwinDisciplines;
            cooldown -= .5f * args.Talents.ImprovedMindBlast;
            //totalCoef += .01f + args.Talents.Shadowform;

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
