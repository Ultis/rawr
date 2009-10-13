using System;

namespace Rawr.Elemental.Spells
{
public class FlameShock : Shock
    {
        public FlameShock() : base()
        {
        }

        protected override void SetBaseValues()
        {
            base.SetBaseValues();

            baseMinDamage = 500;
            baseMaxDamage = 500;
            spCoef = 1.5f / 3.5f / 2f;
            dotSpCoef = .1f;
            periodicTick = 556f / 4f;
            periodicTicks = 6f;
            manaCost = .17f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "FS";
        }

        public override void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            //for reference
            //dotTick = (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + dotCanCrit * critModifier * CritChance)

            totalCoef += .01f * shamanTalents.Concussion;
            totalCoef += .1f * shamanTalents.BoomingEchoes;
            manaCost *= 1 - .02f * shamanTalents.Convection;
            dotBaseCoef *= 1 + .2f * shamanTalents.StormEarthAndFire;
            dotSpCoef *= 1 + .2f * shamanTalents.StormEarthAndFire;
            dotBaseCoef *= 1 + .01f * shamanTalents.Concussion;
            dotSpCoef *= 1 + .01f * shamanTalents.Concussion;
            dotBaseCoef *= 1 + stats.BonusFireDamageMultiplier;
            dotSpCoef *= 1 + stats.BonusFireDamageMultiplier;
            manaCost *= 1 - .45f * shamanTalents.ShamanisticFocus;
            cooldown -= .2f * shamanTalents.Reverberation;
            cooldown -= 1f * shamanTalents.BoomingEchoes;
            spellPower += stats.SpellFireDamageRating;
            totalCoef *= 1 + stats.BonusFireDamageMultiplier;
            periodicTicks += stats.BonusFlameShockDuration / 3f; // t9 2 piece

            if (shamanTalents.GlyphofShocking)
                gcd -= .5f;

            dotCanCrit = stats.FlameShockDoTCanCrit; // T8 2 piece

            base.Initialize(stats, shamanTalents);
        }

        public FlameShock(Stats stats, ShamanTalents shamanTalents)
            : this()
        {
            Initialize(stats, shamanTalents);
        }

        public static FlameShock operator +(FlameShock A, FlameShock B)
        {
            FlameShock C = (FlameShock)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static FlameShock operator *(FlameShock A, float b)
        {
            FlameShock C = (FlameShock)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }

    }
}