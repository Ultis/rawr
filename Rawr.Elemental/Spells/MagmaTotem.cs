using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Spells
{
    public class MagmaTotem : Totem
    {
        private int additionalTargets = 0;

        public MagmaTotem(ISpellArgs args) :
            base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            dotSpCoef = 1/10f;

            periodicTick = 268f;
            periodicTicks = 10f;
            periodicTickTime = 2.0f;
            manaCost = .18f * Constants.BaseMana;
            shortName = "MT";
            dotCanCrit = 1;
        }

        public override void Initialize(ISpellArgs args)
        {
            additionalTargets = args.AdditionalTargets;
            if (additionalTargets < 0)
                additionalTargets = 0;
            else if (additionalTargets > 9)
                additionalTargets = 9;

            totalCoef *= 1 + (.1f * args.Talents.CallOfFlame);
            totalCoef *= (1 + additionalTargets);
            shortName = "MT" + (1 + additionalTargets);

            periodicTicks *= 1 + (.2f * args.Talents.TotemicFocus);

            base.Initialize(args);
        }

        public int AdditionalTargets
        {
            get { return additionalTargets; }
        }

        public static MagmaTotem operator +(MagmaTotem A, MagmaTotem B)
        {
            MagmaTotem C = (MagmaTotem)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static MagmaTotem operator *(MagmaTotem A, float b)
        {
            MagmaTotem C = (MagmaTotem)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
    }
}
