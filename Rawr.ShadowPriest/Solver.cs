using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ShadowPriest
{
    public class Solver
    {
        public List<Spell> SpellPriority { get; protected set; }
        public float OverallDamage { get; protected set; }
        public float OverallDps { get; protected set; }
        public Dictionary<float, Spell> Sequence { get; protected set; }

        protected CalculationOptionsShadowPriest CalculationOptions { get; set; }
        protected Stats PlayerStats { get; set; }
        protected Character character { get; set; }

        protected float HitChance { get; set; }
        protected float ShadowHitChance { get; set; }
        protected float ShadowCritChance { get; set; }
        protected List<Trinket> Trinkets { get; set; }
        
        public Solver(Stats playerStats, Character _char, CalculationOptionsShadowPriest calculationOptions) 
        {
            PlayerStats = playerStats;
            character = _char;
            CalculationOptions = calculationOptions;
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            foreach(string spellname in calculationOptions.SpellPriority)
                SpellPriority.Add(SpellFactory.CreateSpell(spellname, playerStats, character));

            HitChance = PlayerStats.SpellHitRating / 12.6f;
            ShadowHitChance = 83.0f + (PlayerStats.SpellHitRating + (character.PriestTalents.ShadowFocus + character.PriestTalents.Misery) * 12.6f * 2.0f) / 12.6f;
            if (ShadowHitChance > 99.0f)
                ShadowHitChance = 99.0f;

            Trinkets = new List<Trinket>();
            ShadowCritChance = PlayerStats.SpellCrit;
            Sequence = new Dictionary<float, Spell>();
            if (playerStats.SpellDamageFor15SecOnUse2Min > 0.0f)
                Trinkets.Add(new Trinket() { Cooldown = 120.0f, DamageBonus = playerStats.SpellDamageFor15SecOnUse2Min, UpTime = 15.0f });
//            if (playerStats.SpellDamageFor15SecOnUse90Sec > 0.0f)
//                Trinkets.Add(new Trinket() { Cooldown = 90.0f, DamageBonus = playerStats.SpellDamageFor15SecOnUse90Sec, UpTime = 15.0f });
//            if (playerStats.SpellDamageFor20SecOnUse2Min > 0.0f)
//                Trinkets.Add(new Trinket() { Cooldown = 120.0f, DamageBonus = playerStats.SpellDamageFor20SecOnUse2Min, UpTime = 20.0f });
//            if (playerStats.SpellHasteFor20SecOnUse2Min > 0.0f)
//                Trinkets.Add(new Trinket() { Cooldown = 120.0f, HasteBonus = playerStats.SpellHasteFor20SecOnUse2Min, UpTime = 20.0f });
//            if (playerStats.SpellHasteFor20SecOnUse5Min > 0.0f)
//                Trinkets.Add(new Trinket() { Cooldown = 300.0f, HasteBonus = playerStats.SpellHasteFor20SecOnUse5Min, UpTime = 20.0f });
        }

        private Spell GetCastSpell(float timer)
        {
            for (int i = 0; i < SpellPriority.Count; i++)
            {
                if (SpellPriority[i].SpellStatistics.CooldownReset <= timer)
                    return SpellPriority[i];
            }
            return null;
        }

        private void UpTrinket(float timer)
        {
            foreach (Trinket trinket in Trinkets)
                if (trinket.CooldownTimer <= timer)
                {
                    trinket.CooldownTimer = timer + trinket.Cooldown;
                    trinket.UpTimeTimer = timer + trinket.UpTime;
                }
        }

        private void GetTrinketBuff(float timer, Stats stats)
        {
            foreach (Trinket trinket in Trinkets)
                if (trinket.UpTimeTimer > timer)
                {
                    stats.SpellHasteRating += trinket.HasteBonus;
                    stats.SpellDamageRating += trinket.DamageBonus;
                }
        }
               

        public void Calculate()
        {
            if(SpellPriority.Count == 0)
                return;

            Stats currentStats;
            Random rnd = new Random(DateTime.Now.Millisecond);
            int castCount = 0, critableCount = 0, ssCount = 0;
            float timer = 0.0f, drumsTimer = 0.0f, drumsUpTimer = 0.0f;
            
            while (timer < CalculationOptions.FightDuration)
            {
                timer += CalculationOptions.Lag/1000.0f;
                timer += CalculationOptions.WaitTime/1000.0f;
                
                if(CalculationOptions.DrumsCount > 0 && drumsTimer < timer)
                {
                    drumsTimer = timer + 120.0f;
                    drumsUpTimer = timer + CalculationOptions.DrumsCount * 30.0f;
                    if(CalculationOptions.UseYourDrum)
                    {
                        timer += SpellPriority[0].GlobalCooldown;
                        continue;
                    }
                }

                UpTrinket(timer);
                
                currentStats = PlayerStats.Clone();
                if (CalculationOptions.DrumsCount > 0 && drumsUpTimer > timer)
                    currentStats.SpellHasteRating += 80.0f;
                GetTrinketBuff(timer, currentStats);

                Spell spell = GetCastSpell(timer);
                if (spell == null)
                {
                    timer += 0.1f;
                    continue;
                }

                Spell seqspell = SpellFactory.CreateSpell(spell.Name, currentStats, character);
                if (spell.CritCoef > 1.0f)
                    critableCount++;
                if (spell.MagicSchool != MagicSchool.Shadow)
                    ssCount++;
                if (spell.DebuffDuration > 0.0f || spell.Cooldown > 0.0f)
                    spell.SpellStatistics.CooldownReset = timer + (spell.DebuffDuration > spell.Cooldown ? spell.DebuffDuration : spell.Cooldown);

                spell.SpellStatistics.HitCount++;
                Sequence.Add(timer, seqspell);
                timer += seqspell.CastTime > 0.0f ? seqspell.CastTime : seqspell.GlobalCooldown;
                castCount++;
            }

            int missCount = (int)Math.Round((Sequence.Count - ssCount) * (100.0f - ShadowHitChance) / 100.0f);
            int mbmissCount = (int)Math.Round((double)missCount / 2.0f);
            int critCount = (int)Math.Round(critableCount * ShadowCritChance / 100.0f);
            int mbcritCount = (int)Math.Round((double)critCount / 2.0f);
            int ssmissCount = (int)Math.Round(ssCount * (100.0f - HitChance) / 100.0f);
            foreach (Spell spell in SpellPriority)
            {
                if(spell.CritCoef > 1.0f)
                {
                    if (spell.Name == "Mind Blast")
                    {
                        spell.SpellStatistics.HitCount -= mbmissCount;
                        spell.SpellStatistics.MissCount = mbmissCount;
                        spell.SpellStatistics.CritCount = mbcritCount;
                    }
                    else
                    {
                        spell.SpellStatistics.HitCount -= (missCount - mbmissCount);
                        spell.SpellStatistics.MissCount = (missCount - mbmissCount);
                        spell.SpellStatistics.CritCount = (critCount - mbcritCount);
                    }

                    spell.SpellStatistics.DamageDone += spell.SpellStatistics.CritCount * spell.AvgCrit;
                }

                if (spell.MagicSchool != MagicSchool.Shadow)
                {
                    spell.SpellStatistics.HitCount -= ssmissCount;
                    spell.SpellStatistics.MissCount = ssmissCount;
                }

                spell.SpellStatistics.DamageDone += (spell.SpellStatistics.HitCount - spell.SpellStatistics.CritCount) * spell.AvgDamage;

                OverallDamage += spell.SpellStatistics.DamageDone;
            }
            OverallDps = OverallDamage / CalculationOptions.FightDuration;
        }

        protected class Trinket
        {
            public float DamageBonus { get; set; }
            public float HasteBonus { get; set; }
            public float Cooldown { get; set; }
            public float CooldownTimer { get; set; }
            public float UpTime { get; set; }
            public float UpTimeTimer { get; set; }
        }
    }
}
