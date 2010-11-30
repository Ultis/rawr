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

            baseMinDamage = 531;
            baseMaxDamage = 531;
            spCoef = 1.5f / 3.5f / 2f;
            dotSpCoef = .1f;
            periodicTick = 855f / 6f;
            periodicTicks = 6f;
            periodicTickTime = 3f;
            manaCost = .17f * Constants.BaseMana;
            cooldown = 6f;
            shortName = "FS";
        }

        public override void Initialize(ISpellArgs args)
        {
            //for reference
            //dotTick = totalCoef * (periodicTick * dotBaseCoef + spellPower * dotSpCoef) * (1 + dotCanCrit * critModifier * CritChance)

            totalCoef += .01f * args.Talents.Concussion;
            manaCost *= 1 - .02f * args.Talents.Convection;
            dotBaseCoef *= 1 + args.Stats.BonusFlameShockDoTDamage;
            dotSpCoef *= 1 + args.Stats.BonusFlameShockDoTDamage;
            cooldown -= .2f * args.Talents.Reverberation;
            spellPower += args.Stats.SpellFireDamageRating;
            totalCoef *= 1 + args.Stats.BonusFireDamageMultiplier;
            periodicTicks += args.Stats.BonusFlameShockDuration / periodicTickTime; // t9 2 piece

            if (args.Talents.GlyphofFlameShock)
                periodicTicks = (Single)(periodicTicks * .5);
            if (args.Talents.GlyphofShocking)
                gcd -= .5f;

            dotCanCrit = 1;

            ApplyDotHaste(args);
            base.Initialize(args);
        }

        /// <summary>
        /// Adds the closest amount of ticks to the passed time in seconds. Adds at least 2 ticks.
        /// </summary>
        /// <param name="seconds">added time in seconds</param>
        /// <returns>Number of added DoT ticks</returns>
        public float AddTicks(float seconds)
        {
            float addTicks = (float)Math.Round(seconds / periodicTickTime);
            if (addTicks < 2)
                addTicks = 2;
            PeriodicTicks += addTicks;
            return addTicks;
        }

        public FlameShock(ISpellArgs args)
            : this()
        {
            Initialize(args);
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