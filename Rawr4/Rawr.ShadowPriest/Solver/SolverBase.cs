using System;
using System.Collections.Generic;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
    public class SolverBase
    {
        public List<Spell> SpellPriority { get; protected set; }
        public List<Spell> SpellSimulation { get; protected set; }
        public float OverallDamage { get; protected set; }
        public float DPS { get; protected set; }
        public float MovementDamageLoss { get; protected set; }
        public Dictionary<float, Spell> Sequence { get; protected set; }

        public CalculationOptionsShadowPriest CalcOpts { get; set; }
        public BossOptions BossOpts { get; set; }
        public Stats PlayerStats { get; set; }
        public Character character { get; set; }
        public float HitChance { get; set; }
        public List<Trinket> Trinkets { get; set; }
        public List<ManaSource> ManaSources { get; set; }

        public string Name { get; protected set; }
        public string Rotation { get; protected set; }

        public class Trinket
        {
            public float DamageBonus { get; set; }
            public float HasteBonus { get; set; }
            public float Cooldown { get; set; }
            public float CooldownTimer { get; set; }
            public float UpTime { get; set; }
            public float UpTimeTimer { get; set; }
        }

        public class ManaSource
        {
            public string Name { get; set; }
            public float Value { get; set; }

            public ManaSource(string name, float value)
            {
                Name = name; Value = value;
            }
        }

        public class SpellComparerDpCT : IComparer<Spell>
        {
            public int Compare(Spell a, Spell b)
            {
                if (a == null)
                {
                    if (b == null)
                        return 0;
                    return -1;
                }
                else if (b == null)
                    return 1;
                return (int)Math.Round(b.DpCT - a.DpCT);
            }
        }

        protected float _GetDirectDamage(Stats stats)
        {
            return stats.ArcaneDamage
                + stats.FireDamage
                + stats.FrostDamage
                + stats.HolyDamage
                + stats.NatureDamage
                + stats.ShadowDamage
                + stats.ValkyrDamage;
        }

        public SolverBase(Stats playerStats, Character _char)
        {
            character = _char;
            Name = "Base";
            Rotation = "None";

            // USE trinkets & effects.
            Stats Twinkets = new Stats();
            foreach (SpecialEffect se in playerStats.SpecialEffects())
            {
                if (se.Stats.ManaRestore == 0 && se.Stats.Mp5 == 0 && se.Stats.SpellPower == 0)
                {
                    if (se.Trigger == Trigger.Use)
                        Twinkets += se.GetAverageStats();
                    else if (se.Trigger == Trigger.DamageSpellCast
                        || se.Trigger == Trigger.SpellCast)
                    {
                        if (se.Stats.HighestStat > 0)
                        {
                            float greatnessProc = se.GetAverageStats(2f, 1f).HighestStat;
                            if (playerStats.Spirit > playerStats.Intellect)
                                Twinkets.Spirit += greatnessProc;
                            else
                                Twinkets.Intellect += greatnessProc;
                        }
                    }
                }
            }

            Twinkets.Spirit = (float)Math.Round(Twinkets.Spirit * (1 + playerStats.BonusSpiritMultiplier));
            Twinkets.Intellect = (float)Math.Round(Twinkets.Intellect * (1 + playerStats.BonusIntellectMultiplier));
            playerStats.Accumulate(Twinkets);

            CalcOpts = character.CalculationOptions as CalculationOptionsShadowPriest;
            BossOpts = character.BossOptions;

            HitChance = playerStats.SpellHit * 100f + 100 - (StatConversion.GetSpellMiss(character.Level - BossOpts.Level, false) * 100);

            if (character.Race == CharacterRace.Draenei)
                HitChance += 1;
            HitChance = (float)Math.Min(100f, HitChance);

            Trinkets = new List<Trinket>();
            Sequence = new Dictionary<float, Spell>();
            ManaSources = new List<ManaSource>();

            PlayerStats = playerStats;
        }

        public virtual void Calculate(CharacterCalculationsShadowPriest calculatedStats)
        {
            DPS = 0;
        }
        /*
        public Spell GetCastSpell(float timer)
        {   // FIXME: Rewrite this freaking crappy shit.
            /*
            foreach (Spell spell in SpellPriority)
            {
                if ((spell.DebuffDuration > 0) && (spell.CastTime > 0) && (spell.SpellStatistics.CooldownReset < (spell.CastTime + timer)))
                    return spell;   // Special case for dots that have cast time (Holy Fire / Vampiric Touch)
                //if (spell.SpellStatistics.CooldownReset <= timer && spell.Cooldown > 0)
                if (spell.SpellStatistics.CooldownReset <= timer)
                    return spell;
                if (spell.SpellStatistics.CooldownReset > 0
                    && (spell.SpellStatistics.CooldownReset - (spell.DebuffDuration > 0 ? spell.CastTime : 0) - timer < spell.GlobalCooldown * 0.2f))
                    return null;
                if (spell.SpellStatistics.CooldownReset <= timer)
                    return spell;
            }
             /
            return null;
        }

        public float OldGetCastSpell(float timer, out Spell castSpell, Spell SWD)
        {   // FIXME: Rewrite this freaking crappy shit.
            /*
            castSpell = null;
            foreach (Spell spell in SpellPriority)
            {
                castSpell = spell;
                if ((spell.DebuffDuration > 0) && (spell.CastTime > 0) && (spell.SpellStatistics.CooldownReset < (spell.CastTime + timer)))
                    return 0;   // Special case for dots that have cast time (Holy Fire / Vampiric Touch)
                //if (spell.SpellStatistics.CooldownReset <= timer && spell.Cooldown > 0)
                if (spell.SpellStatistics.CooldownReset <= timer)
                    return 0;
                if (spell.SpellStatistics.CooldownReset > 0)
                {
                    float nextCast = spell.SpellStatistics.CooldownReset - (spell.DebuffDuration > 0 ? spell.CastTime : 0) - timer;
                    if (nextCast < 0.5f)
                        return nextCast;
                    /if (nextCast <= spell.GlobalCooldown && SWD != null & SWD.SpellStatistics.CooldownReset <= timer)
                    {
                        castSpell = SWD;
                        return 0f;
                    }/
                }
                if (spell.SpellStatistics.CooldownReset <= timer)
                    return 0;
            }/
            castSpell = null;
             
            return 0;
        }

        public Spell NewGetCastSpell(float timer)
        {
            float[] nextRelativeCastTime = new float[SpellPriority.Count];

            int cnt = 0;
            foreach (Spell spell in SpellPriority)
            {
                if (spell.DebuffDuration > 0)
                    nextRelativeCastTime[cnt++] = spell.SpellStatistics.CooldownReset - spell.CastTime - timer;
                else if (spell.Cooldown > 0)
                    nextRelativeCastTime[cnt++] = spell.SpellStatistics.CooldownReset - timer;
                else
                {   // This is our filler.
                    nextRelativeCastTime[cnt++] = 0;
                    break;
                }
            }

            for (int x = 0; x < cnt; x++)
                if (nextRelativeCastTime[x] < 0)
                    return SpellPriority[x];

            Spell spellCastNext = SpellPriority[cnt - 1];
            float fillerDPS = spellCastNext.DpCT;
            float nextDelayCost = fillerDPS;
            for (int x = 0; x < cnt - 1; x++)
            {
                float delayCost = SpellPriority[x].DpCT - nextRelativeCastTime[x] * fillerDPS;
                if (delayCost > nextDelayCost)
                {
                    nextDelayCost = delayCost;
                    spellCastNext = SpellPriority[x];
                }
            }
            return spellCastNext;
        }
        */
        public void RecalculateHaste(Stats stats, float addedHasteRating)
        {
            foreach (Spell spell in SpellPriority)
                spell.RecalcHaste(stats, addedHasteRating);
        }

        public void UpTrinket(float timer)
        {
            foreach (Trinket trinket in Trinkets)
                if (trinket.CooldownTimer <= timer)
                {
                    trinket.CooldownTimer = timer + trinket.Cooldown;
                    trinket.UpTimeTimer = timer + trinket.UpTime;
                }
        }

        public void GetTrinketBuff(float timer, Stats stats)
        {
            foreach (Trinket trinket in Trinkets)
                if (trinket.UpTimeTimer > timer)
                {
                    stats.HasteRating += trinket.HasteBonus;
                    stats.SpellPower += trinket.DamageBonus;
                }
        }
    }
}
