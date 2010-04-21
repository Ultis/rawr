using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Spells
{
    public class MagmaTotem : Spell
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

            periodicTick = 371.0f;
            periodicTicks = 10f;
            periodicTickTime = 2.0f;
            manaCost = .27f * Constants.BaseMana;
            shortName = "MT";
            dotCanCrit = 1;
            gcd = 1;
            castTime = 0;
        }

        public override void Initialize(ISpellArgs args)
        {
            additionalTargets = args.AdditionalTargets;
            if (additionalTargets < 0)
                additionalTargets = 0;
            else if (additionalTargets > 10)
                additionalTargets = 10;

            periodicTick *= 1 + (.05f * args.Talents.CallOfFlame);

            manaCost *= 1 - (.05f * args.Talents.TotemicFocus);
            dotSpCoef *= 1 + (.05f * args.Talents.CallOfFlame);
            dotBaseCoef *= 1 + (.05f * args.Talents.CallOfFlame);

            shortName = "MT" + (1 + additionalTargets);

            base.Initialize(args);
        }

        new public float PeriodicTick
        { get { return base.PeriodicTick * (additionalTargets + 1); } }

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
