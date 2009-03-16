using System;
using System.Collections.Generic;

namespace Rawr.Tree
{
    public abstract class SpecialEffect
    {
        public abstract Stats estimate(float TotalTime, float castsPS, float healsPS, float critsPS, out float Healing);
        public static float EstimateUptime(float duration, float icd, float pps, float FightDuration, out float procs)
        {
            procs = 0;
            float averageTimeToProc = pps == 0 ? 0 : 1 / pps;
            float totalCD = icd + averageTimeToProc;
            if (averageTimeToProc > FightDuration) return 0f;
            FightDuration -= averageTimeToProc;
            if (duration == 0) // instant proc
            {
                procs = (float)Math.Floor(FightDuration / totalCD) + 1;
                return procs / FightDuration; // return value per second
            }
            else
            {
                procs = (float)Math.Floor(FightDuration / totalCD) + 1;
                float activity = (procs - 1) * duration +
                    Math.Min(Math.Max(0, FightDuration % totalCD), duration);
                activity /= FightDuration;
                return activity; // return uptime per second
            }
        }
    }

    class GeneralProc : SpecialEffect
    {
        protected Stats statsOnProc;
        protected float healingOnProc;
        protected float manaOnProc;
        protected float procOnCrit, procOnHeal, procOnCast;
        protected float Duration;
        protected float internalCD;

        public GeneralProc(Stats stats, float healing, float mana, float duration, float iCD, float onCast, float onHeal, float onCrit, float maxStack)
        {
            statsOnProc = stats;
            healingOnProc = healing;
            manaOnProc = mana;
            Duration = duration;
            internalCD = iCD;
            procOnCast = onCast;
            procOnHeal = onHeal;
            procOnCrit = onCrit;
        }

        public override Stats estimate(float TotalTime, float castsPS, float healsPS, float critsPS, out float Healing)
        {
            float procs;
            float up = EstimateUptime(Duration, internalCD, healsPS * procOnHeal + castsPS * procOnCast + critsPS * procOnCrit, TotalTime, out procs);
            float upT = TotalTime * up;
            // up = average uptime for over time effects
            // upT = amount of procs for instant effects
            Stats result = statsOnProc * up;
            result.Mp5 += 5 * up * manaOnProc; // upT * manaOnProc / TotalTime
            Healing = upT * healingOnProc;
            return result;
        }
    }

    class GeneralUse : SpecialEffect
    {
        protected Stats statsOnProc;
        protected float healingOnProc;
        protected float manaOnProc;
        protected float Duration;
        protected float internalCD;
        protected float Cooldown = 0f;
        protected bool gcdOnUse;
        protected float timeLeft = 0f;
        protected bool activateOnClone = false;

        public GeneralUse(Stats stats, float healing, float mana, float duration, float cooldown, bool gcd)
        {
            statsOnProc = stats;
            healingOnProc = healing;
            manaOnProc = mana;
            Duration = duration;
            internalCD = cooldown;
            gcdOnUse = gcd;
        }

        public override Stats estimate(float TotalTime, float castsPS, float healsPS, float critsPS, out float Healing)
        {
            float procs;
            float up = EstimateUptime(Duration, internalCD, 0, TotalTime, out procs);
            // up = average uptime for over time effects
            Stats result = statsOnProc * up;
            result.Mp5 += 5 * procs * manaOnProc; 
            Healing = procs * healingOnProc;
            return result;
        }
    }

    public class SpecialEffects
    {
        private List<SpecialEffect> _effects;
        public List<SpecialEffect> Effects
        {
            get { return _effects; }
        }

        protected SpecialEffects()
        {
            _effects = new List<SpecialEffect>();
        }

        public SpecialEffects(Stats stats)
        {
            _effects = new List<SpecialEffect>();
            convertState(stats);
        }

        protected void createSpellPowerProc(float value, float duration, float iCD, float onCast, float onHeal, float onCrit, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { SpellPower = value }, 0f, 0f, duration, iCD, onCast, onHeal, onCrit,maxStack));
            }
        }

        protected void createSpellPowerUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { SpellPower = value }, 0f, 0f, duration, CD, gcd));
            }
        }

        protected void createSpellHasteProc(float value, float duration, float iCD, float onCast, float onHeal, float onCrit, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { HasteRating = value }, 0f, 0f, duration, iCD, onCast, onHeal, onCrit, maxStack));
            }
        }

        protected void createSpellHasteUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { HasteRating = value }, 0f, 0f, duration, CD, gcd));
            }
        }

        protected void createMP5Proc(float value, float duration, float iCD, float onCast, float onHeal, float onCrit, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { Mp5 = value }, 0f, 0f, duration, iCD, onCast, onHeal, onCrit, maxStack));
            }
        }

        protected void createMP5Use(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { Mp5 = value }, 0f, 0f, duration, CD, gcd));
            }
        }

        protected void createManaProc(float value, float duration, float iCD, float onCast, float onHeal, float onCrit, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { }, 0f, value, duration, iCD, onCast, onHeal, onCrit, maxStack));
            }
        }

        protected void createManaUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { }, 0f, value, duration, CD, gcd));
            }
        }

        protected void createSPCProc(float value, float duration, float iCD, float onCast, float onHeal, float onCrit, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { SpellCombatManaRegeneration = value }, 0f, 0f, duration, iCD, onCast, onHeal, onCrit, maxStack));
            }
        }

        protected void createSpiritUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats { Spirit = value }, 0f, 0f, duration, CD, gcd));
            }
        }

        protected void createHealingProc(float value, float duration, float iCD, float onCast, float onHeal, float onCrit, float maxStack)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralProc(new Stats { }, value, 0f, duration, iCD, onCast, onHeal, onCrit, maxStack));
            }
        }

        protected void createHealingUse(float value, float duration, float CD, bool gcd)
        {
            if (value != 0)
            {
                _effects.Add(new GeneralUse(new Stats {  }, value, 0f, duration, CD, gcd));
            }
        }

        protected void convertState(Stats stats)
        {
            // Flow of Knowledge
            createSpellPowerProc(stats.SpellPowerFor10SecOnCast_10_45, 10, 45, .1f, 0, 0, 1);
            // Forge Ember
            createSpellPowerProc(stats.SpellPowerFor10SecOnHeal_10_45, 10, 45, 0, .1f, 0, 1);
            // Dying Curse
            createSpellPowerProc(stats.SpellPowerFor10SecOnCast_15_45, 10, 45, .15f, 0, 0, 1);
            // Shiffar's Nexus-Horn
            createSpellPowerProc(stats.SpellPowerFor10SecOnCrit_20_45, 10, 45, 0, 0, .2f, 1);
            // Sextant of Unstable Currents
            createSpellPowerProc(stats.SpellPowerFor15SecOnCrit_20_45, 15, 45, 0, 0, .2f, 1);

            createSpellPowerUse(stats.SpellPowerFor15SecOnUse90Sec, 15, 90, false);
            createSpellPowerUse(stats.SpellPowerFor15SecOnUse2Min, 15, 120, false);
            createSpellPowerUse(stats.SpellPowerFor20SecOnUse2Min, 20, 120, false);
            createSpellPowerUse(stats.SpellPowerFor20SecOnUse5Min, 20, 300, false);

            // Embrace of the Spider
            createSpellHasteProc(stats.SpellHasteFor10SecOnCast_10_45, 10, 45, .1f, 0, 0, 1);
            // The Egg of Mortal Essence
            createSpellHasteProc(stats.SpellHasteFor10SecOnHeal_10_45, 10, 45, 0, .1f, 0, 1);
            // Mystical Skyfire Diamond
            createSpellHasteProc(stats.SpellHasteFor6SecOnCast_15_45, 6, 45, .15f, 0, 0, 1);
            // The Skull of Gul'dan
            createSpellHasteUse(stats.HasteRatingFor20SecOnUse2Min, 20, 120, false);

            // Pendant of the Violet Eye
            if (stats.Mp5OnCastFor20SecOnUse2Min > 0) 
            {
                float Speed = (1f + stats.SpellHaste) * (1f + stats.HasteRating * 0.000304971132f);
                float gcd = (float)Math.Min(1.5f / Speed, 1);
                float maxStack = 20 / gcd; 
                // add 50% for the amount of mp5 from the "building the stack" time
                createMP5Use(stats.Mp5OnCastFor20SecOnUse2Min * maxStack * 1.5f, 20, 120, false);
            }

            // Soul of the Dead
            createManaProc(stats.ManaRestoreOnCrit_25_45, 0, 45, 0, 0, .25f, 1);
            // Jet'ze's Bell and Spark of Life and similar effects
            createMP5Proc(5f / 15f * stats.ManaRestoreOnCast_10_45, 15, 45, .1f, 0, 0, 1);
            // ISD Meta gem
            createMP5Proc(stats.ManaRestoreOnCast_5_15, 0, 15, .05f, 0, 0, 1);
            // Figurine - Seaspray Albatross
            createMP5Use(5f / 12f * stats.ManaregenOver12SecOnUse3Min, 12, 180, false);
            // Figurine - Talasite Owl
            createMP5Use(5f / 12f * stats.ManaregenOver12SecOnUse5Min, 12, 300, false);
            // Figurine - Sapphire Owl
            createMP5Use(5f / 12f * stats.ManaRestore5min, 12, 300, false);
            // Darkmoon Card: Blue Dragon
            if (stats.FullManaRegenFor15SecOnSpellcast > 0)
                createSPCProc(1f, 15f, 0, stats.FullManaRegenFor15SecOnSpellcast / 100f, 0, 0, 1);
            // Spirit-World glass
            createSpiritUse(stats.SpiritFor20SecOnUse2Min, 20, 120, false);

            // Forethought Talisman: BonusHoTOnDirectHeals
            createHealingProc(stats.BonusHoTOnDirectHeals, 0, 45f, .2f, 0, 0, 1);
            // Living Ice Crystals
            createHealingUse(stats.Heal1Min, 0, 60, true);
            // Talisman of Troll Divinity
            if (stats.TrollDivinity > 0)
            {
                // For 20 seconds, direct healing adds a stack of 58 +healing for 10 seconds
                // Stacks 5 times, 2 minute cd
                // Direct heals: Nourish (1.5) HT (3) Regrowth (2)
                // Assumption: every 2 seconds, a direct healing spell is cast
                // 2*(1+2+3+4)+22*5 = 130, total duration 30 seconds
                // But remember that the spellpower will increase for others in the raid too!
                createSpellPowerUse(58f * 130f / 30f, 30, 120, false);
            }

            // TODO: Darkmoon Card: Illusion
        }

        public static Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                SpellPowerFor10SecOnCast_10_45 = stats.SpellPowerFor10SecOnCast_10_45,
                SpellPowerFor10SecOnHeal_10_45 = stats.SpellPowerFor10SecOnHeal_10_45,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                SpellPowerFor10SecOnCrit_20_45 = stats.SpellPowerFor10SecOnCrit_20_45,
                SpellPowerFor15SecOnCrit_20_45 = stats.SpellPowerFor15SecOnCrit_20_45,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                SpellPowerFor20SecOnUse5Min = stats.SpellPowerFor20SecOnUse5Min,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                SpellHasteFor10SecOnHeal_10_45 = stats.SpellHasteFor10SecOnHeal_10_45,
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestoreOnCrit_25_45 = stats.ManaRestoreOnCrit_25_45,
                ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                ManaregenOver12SecOnUse3Min = stats.ManaregenOver12SecOnUse3Min,
                ManaregenOver12SecOnUse5Min = stats.ManaregenOver12SecOnUse5Min,
                ManaRestore5min = stats.ManaRestore5min,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                BonusHoTOnDirectHeals = stats.BonusHoTOnDirectHeals,
                Heal1Min = stats.Heal1Min,
                TrollDivinity = stats.TrollDivinity,
            };
        }

        public static bool HasRelevantStats(Stats stats)
        {
            return (
                stats.SpellPowerFor10SecOnCast_10_45 +
                stats.SpellPowerFor10SecOnHeal_10_45 +
                stats.SpellPowerFor10SecOnCast_15_45 +
                stats.SpellPowerFor10SecOnCrit_20_45 +
                stats.SpellPowerFor15SecOnCrit_20_45 +
                stats.SpellPowerFor15SecOnUse2Min +
                stats.SpellPowerFor15SecOnUse90Sec +
                stats.SpellPowerFor20SecOnUse2Min +
                stats.SpellPowerFor20SecOnUse5Min +
                stats.SpellHasteFor10SecOnCast_10_45 +
                stats.SpellHasteFor10SecOnHeal_10_45 +
                stats.SpellHasteFor6SecOnCast_15_45 +
                stats.HasteRatingFor20SecOnUse2Min +
                stats.Mp5OnCastFor20SecOnUse2Min +
                stats.ManaRestoreOnCrit_25_45 +
                stats.ManaRestoreOnCast_10_45 +
                stats.ManaRestoreOnCast_5_15 +
                stats.ManaregenOver12SecOnUse3Min +
                stats.ManaregenOver12SecOnUse5Min +
                stats.ManaRestore5min +
                stats.FullManaRegenFor15SecOnSpellcast +
                stats.SpiritFor20SecOnUse2Min +
                stats.BonusHoTOnDirectHeals +
                stats.Heal1Min +
                stats.TrollDivinity) > 0;
        }

        private SpecialEffects Clone()
        {
            SpecialEffects clone = new SpecialEffects();
            clone._effects = new List<SpecialEffect>();
            foreach (SpecialEffect e in _effects)
            {
                clone._effects.Add(e);
            }
            return clone;
        }

        public Stats estimateAll(float TotalTime, float castsPS, float healsPS, float critsPS, out float Healing)
        {
            float heal = 0;
            Stats result = new Stats();
            foreach (SpecialEffect e in _effects)
            {
                float h;
                result += e.estimate(TotalTime, castsPS, healsPS, critsPS, out h);
                heal += h;
            }
            Healing = heal;
            return result;
        }

        public static SpecialEffects operator +(SpecialEffects a, SpecialEffects b)
        {
            SpecialEffects c = a.Clone();
            c._effects.AddRange(b._effects.FindAll(e => !a._effects.Contains(e)));
            return c;
        }
    }
}