using System;

namespace Rawr.Elemental.Spells
{
public class Thunderstorm : Spell
    {
        public Thunderstorm()
        {
            #region Base Values
            baseMinDamage = 1450;
            baseMaxDamage = 1656;
            castTime = 0f;
            spCoef = 1.5f / 7f * .9f; // NOT CORRECT YET, assuming 1.5f/7f * 0.9f (aoe with additional effect)
            manaCost = 0f;
            cooldown = 45f;
            shortName = "TS";
            #endregion
        }

        public new void Initialize(Stats stats, ShamanTalents shamanTalents)
        {
            totalCoef += .01f * shamanTalents.Concussion;
            crit += .05f * shamanTalents.CallOfThunder;
            crit += .01f * shamanTalents.TidalMastery;
            spellPower += stats.SpellNatureDamageRating;
            totalCoef *= 1 + stats.BonusNatureDamageMultiplier;

            if (shamanTalents.GlyphofThunder)
                cooldown -= 10f;

            base.Initialize(stats, shamanTalents);
        }

        public Thunderstorm(Stats stats, ShamanTalents shamanTalents) : this()
        {
            Initialize(stats, shamanTalents);
        }

        public static float getProcsPerSecond(bool glyphed, int fightDuration)
        {
            if (glyphed)
            {
                if (manaRestoreGlyphed == null)
                    manaRestoreGlyphed = new SpecialEffect(Trigger.Use, new Stats { }, 0f, 35f, 1f);
                return manaRestoreGlyphed.GetAverageProcsPerSecond(35f, 1f, 1f, fightDuration);
            }
            else
            {
                if (manaRestoreUnglyphed == null)
                    manaRestoreUnglyphed = new SpecialEffect(Trigger.Use, new Stats { }, 0f, 45f, 1f);
                return manaRestoreUnglyphed.GetAverageProcsPerSecond(45f, 1f, 1f, fightDuration);
            }
        }

        private static SpecialEffect manaRestoreGlyphed = null;
        private static SpecialEffect manaRestoreUnglyphed = null;

        public static Thunderstorm operator +(Thunderstorm A, Thunderstorm B)
        {
            Thunderstorm C = (Thunderstorm)A.MemberwiseClone();
            add(A, B, C);
            return C;
        }

        public static Thunderstorm operator *(Thunderstorm A, float b)
        {
            Thunderstorm C = (Thunderstorm)A.MemberwiseClone();
            multiply(A, b, C);
            return C;
        }
    }
}