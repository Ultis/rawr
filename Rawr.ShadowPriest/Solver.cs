using System;
using System.Collections.Generic;
using System.Linq;
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
        protected TalentTree PlayerTalentTree { get; set; }

        protected float HitChance { get; set; }
        protected float ShadowHitChance { get; set; }
        protected float ShadowCritChance { get; set; }
        protected List<Trinket> Trinkets { get; set; }
        
        public Solver(Stats playerStats, TalentTree playerTalentTree, CalculationOptionsShadowPriest calculationOptions) 
        {
            PlayerStats = playerStats;
            PlayerTalentTree = playerTalentTree;
            CalculationOptions = calculationOptions;
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            foreach(string spellname in calculationOptions.SpellPriority)
                SpellPriority.Add(SpellFactory.CreateSpell(spellname, playerStats, playerTalentTree));

            HitChance = PlayerStats.SpellHitRating / 12.6f;
            ShadowHitChance = 83 + (PlayerStats.SpellHitRating + playerTalentTree.GetTalent("Shadow Focus").PointsInvested * 12.6f * 2) / 12.6f;
            if (ShadowHitChance > 99)
                ShadowHitChance = 99;

            Trinkets = new List<Trinket>();
            ShadowCritChance = PlayerStats.SpellCrit + playerTalentTree.GetTalent("Shadow Power").PointsInvested * 3;
            Sequence = new Dictionary<float, Spell>();
            if (playerStats.SpellDamageFor15SecOnUse2Min > 0)
                Trinkets.Add(new Trinket() { Cooldown = 120, DamageBonus = playerStats.SpellDamageFor15SecOnUse2Min, UpTime = 15 });
            if (playerStats.SpellDamageFor15SecOnUse90Sec > 0)
                Trinkets.Add(new Trinket() { Cooldown = 90, DamageBonus = playerStats.SpellDamageFor15SecOnUse90Sec, UpTime = 15 });
            if (playerStats.SpellDamageFor20SecOnUse2Min > 0)
                Trinkets.Add(new Trinket() { Cooldown = 120, DamageBonus = playerStats.SpellDamageFor20SecOnUse2Min, UpTime = 20 });
            if (playerStats.SpellHasteFor20SecOnUse2Min > 0)
                Trinkets.Add(new Trinket() { Cooldown = 120, HasteBonus = playerStats.SpellHasteFor20SecOnUse2Min, UpTime = 20 });
            if (playerStats.SpellHasteFor20SecOnUse5Min > 0)
                Trinkets.Add(new Trinket() { Cooldown = 300, HasteBonus = playerStats.SpellHasteFor20SecOnUse5Min, UpTime = 20 });
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
            float timer = 0, drumsTimer = 0, drumsUpTimer = 0;
            
            while (timer < CalculationOptions.FightDuration)
            {
                timer += CalculationOptions.Lag/1000;
                timer += CalculationOptions.WaitTime/1000;
                
                if(CalculationOptions.DrumsCount > 0 && drumsTimer < timer)
                {
                    drumsTimer = timer + 120;
                    drumsUpTimer = timer + CalculationOptions.DrumsCount * 30;
                    if(CalculationOptions.UseYourDrum)
                    {
                        timer += SpellPriority[0].GlobalCooldown;
                        continue;
                    }
                }

                UpTrinket(timer);
                
                currentStats = PlayerStats.Clone();
                if (CalculationOptions.DrumsCount > 0 && drumsUpTimer > timer)
                    currentStats.SpellHasteRating += 80;
                GetTrinketBuff(timer, currentStats);

                Spell spell = GetCastSpell(timer);
                if (spell == null)
                {
                    timer += 0.1f;
                    continue;
                }

                Spell seqspell = SpellFactory.CreateSpell(spell.Name, currentStats, PlayerTalentTree);
                if (spell.CritCoef > 1)
                    critableCount++;
                if (spell.MagicSchool != MagicSchool.Shadow)
                    ssCount++;
                if (spell.DebuffDuration > 0 || spell.Cooldown > 0)
                    spell.SpellStatistics.CooldownReset = timer + (spell.DebuffDuration > spell.Cooldown ? spell.DebuffDuration : spell.Cooldown);

                spell.SpellStatistics.HitCount++;
                Sequence.Add(timer, seqspell);
                timer += seqspell.CastTime > 0 ? seqspell.CastTime : seqspell.GlobalCooldown;
                castCount++;
            }

            int missCount = (int)Math.Round((Sequence.Count - ssCount) * (100 - ShadowHitChance) / 100);
            int mbmissCount = (int)Math.Round((double)missCount / 2);
            int critCount = (int)Math.Round(critableCount * ShadowCritChance / 100);
            int mbcritCount = (int)Math.Round((double)critCount / 2);
            int ssmissCount = (int)Math.Round(ssCount * (100 - HitChance) / 100);
            foreach (Spell spell in SpellPriority)
            {
                if(spell.CritCoef > 1)
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
