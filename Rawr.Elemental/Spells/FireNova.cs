using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Elemental.Spells
{
    public class FireNova : Spell
    {
        private int additionalTargets = 0;

        public FireNova()
            : base()
        {
        }

        public FireNova(ISpellArgs args)
            : base()
        {
            Initialize(args);
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 893;
            baseMaxDamage = 997;
            spCoef = .75f / 3.5f;
            manaCost = .22f * Constants.BaseMana;
            cooldown = 10;
        }

        public override void Initialize(ISpellArgs args)
        {
            additionalTargets = args.AdditionalTargets;

            shortName = "FN" + (1 + additionalTargets);

            spCoef += .1f * args.Talents.ImprovedFireNova;

            cooldown -= 2 * args.Talents.ImprovedFireNova;

            if (args.Talents.GlyphofFireNova)
                cooldown -= 3;

            base.Initialize(args);
        }

        public override float MinHit
        {
            get
            {
                return (base.MinHit * (1 + additionalTargets));
            }
        }

        public override float MaxHit
        {
            get
            {
                return (base.MaxHit * (1 + additionalTargets));
            }
        }

        public float MinCrit
        {
            get
            {
                return (base.MinCrit * (1 + additionalTargets));
            }
        }

        public float MaxCrit
        {
            get
            {
                return (base.MaxCrit * (1 + additionalTargets));
            }
        }
    }
}
