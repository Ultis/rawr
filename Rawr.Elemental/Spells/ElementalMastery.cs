using System;

namespace Rawr.Elemental.Spells
{
public class ElementalMastery : Spell
    {
        public ElementalMastery() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();
            missChance = 0;
            cooldown = 180f;
            gcd = 0; // no global cooldown ;)
            shortName = "EM";
        }

        public override void Initialize(ISpellArgs args)
        {
            base.Initialize(args);
        }

        public ElementalMastery(ISpellArgs args)
            : this()
        {
            Initialize(args);
        }

        private static SpecialEffect uptimeGlyphed = null;
        private static SpecialEffect uptimeUnglyphed = null;

        public static ElementalMastery operator +(ElementalMastery A, ElementalMastery B)
        {
            ElementalMastery C = (ElementalMastery)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static ElementalMastery operator *(ElementalMastery A, float b)
        {
            ElementalMastery C = (ElementalMastery)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
    }
}