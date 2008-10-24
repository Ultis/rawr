using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ShadowPriest
{
    public class SolverBase
    {
        public List<Spell> SpellPriority { get; protected set; }
        public float OverallDamage { get; protected set; }
        public float DPS { get; protected set; }
        public float OverallMana { get; protected set; }
        public float SustainDPS { get; protected set; }
        public Dictionary<float, Spell> Sequence { get; protected set; }

        public CalculationOptionsShadowPriest CalculationOptions { get; set; }
        public Stats PlayerStats { get; set; }
        public Character character { get; set; }
        public float HitChance { get; set; }
        public List<Trinket> Trinkets { get; set; }

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

        public SolverBase(Stats playerStats, Character _char) 
        {
            Name = "Base";
            Rotation = "None";

            Stats Twinkets = new Stats();
            if (playerStats.SpellPowerFor15SecOnUse90Sec > 0.0f)
                Twinkets.SpellPower += playerStats.SpellPowerFor15SecOnUse90Sec * 15f / 90f;
            if (playerStats.SpellPowerFor15SecOnUse2Min > 0.0f)
                Twinkets.SpellPower += playerStats.SpellPowerFor15SecOnUse2Min * 15f / 120f;
            if (playerStats.SpellPowerFor20SecOnUse2Min > 0.0f)
                Twinkets.SpellPower += playerStats.SpellPowerFor20SecOnUse2Min * 20f / 120f;
            if (playerStats.HasteRatingFor20SecOnUse2Min > 0.0f)
                Twinkets.HasteRating += playerStats.HasteRatingFor20SecOnUse2Min * 20f / 120f;
            if (playerStats.HasteRatingFor20SecOnUse5Min > 0.0f)
                Twinkets.HasteRating += playerStats.HasteRatingFor20SecOnUse5Min * 20f / 300f;
            // This is a very very wrong way of adding haste from Trinkets, due to the multiplicative nature of Haste.
            Twinkets.SpellHaste += Twinkets.HasteRating / 15.76923275f / 100f;


            PlayerStats = playerStats + Twinkets;
            character = _char;
            CalculationOptions = character.CalculationOptions as CalculationOptionsShadowPriest;
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            foreach(string spellname in CalculationOptions.SpellPriority)
                SpellPriority.Add(SpellFactory.CreateSpell(spellname, PlayerStats, character));

            HitChance = PlayerStats.SpellHit * 100f + CalculationOptions.TargetHit;
            if (!character.ActiveBuffsConflictingBuffContains("Spell Hit Chance Taken"))
                HitChance += character.PriestTalents.Misery * 1f;
            if (character.Race == Character.CharacterRace.Draenei && !character.ActiveBuffsContains("Heroic Presence"))
                HitChance += 1;
            HitChance = (float)Math.Min(100f, HitChance);

            Trinkets = new List<Trinket>();
            Sequence = new Dictionary<float, Spell>();
        }

        public virtual void Calculate(CharacterCalculationsShadowPriest calculatedStats)
        {
            DPS = 0;
            SustainDPS = 0;
        }

        public Spell GetCastSpell(float timer)
        {
            foreach (Spell spell in SpellPriority)
            {
                if ((spell.DebuffDuration > 0) && (spell.CastTime > 0) && (spell.SpellStatistics.CooldownReset < (spell.CastTime + timer)))
                    return spell;   // Special case for dots that have cast time (Holy Fire / Vampiric Touch)
                if (spell.SpellStatistics.CooldownReset <= timer)
                    return spell;
            }
            return null;
        }

        public Spell GetSpellByName(string name)
        {
            foreach (Spell spell in SpellPriority)
            {
                if (spell.Name == name)
                    return spell;
            }
            return null;
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

    public class SolverShadow : SolverBase
    {   // Models Full Rotation
        protected float ShadowHitChance { get; set; }
        protected Spell SWP { get; set; }
        protected Spell MF { get; set; }
        protected Spell VE { get; set; }
        protected Spell SWD { get; set; }
        protected Spell MB { get; set; }
        protected Spell DP { get; set; }
        protected bool bPnS { get; set; }
        
        public SolverShadow(Stats BasicStats, Character character)
            : base(BasicStats, character)
        {
            Name = "Full Shadow";
            Rotation = "Priority Based:";
            foreach (Spell spell in SpellPriority)
                Rotation += "\r\n" + spell.Name;
            ShadowHitChance = (float)Math.Min(100f, HitChance + character.PriestTalents.ShadowFocus * 1f);
            SWP = GetSpellByName("Shadow Word: Pain");
            MF = GetSpellByName("Mind Flay");
            VE = GetSpellByName("Vampiric Embrace");
            SWD = GetSpellByName("Shadow Word: Death");
            MB = GetSpellByName("Mind Blast");
            DP = GetSpellByName("Devouring Plague");

            if (VE != null)
            {   // Functional yet abysmal method of moving VE to bottom of priority.
                SpellPriority.Remove(VE);
                SpellPriority.Add(VE);
            }

            // With Pain and Suffering, Mind Flay has a 33/66/100% chance to refresh SW:P.
            // This mean we cast SW:P once, and reap the benefits later.
            if (bPnS = character.PriestTalents.PainAndSuffering > 0)
            {   // Move SW:Pain to the end of the list in a very non-fashionable and hacky way.
                SpellPriority.Remove(SWP);
                SpellPriority.Add(SWP);
            }
        }

        public override void Calculate(CharacterCalculationsShadowPriest calculatedStats)
        {         
            if (SpellPriority.Count == 0)
                return;

            Stats simStats = PlayerStats.Clone();
            bool bTwistedFaith = character.PriestTalents.TwistedFaith > 0;
            float timer = 0;
            int sequence = SpellPriority.Count-1;
            List<Spell> CastList = new List<Spell>();

            // Initial SW:Pain application
            if (bPnS)
                SWP.SpellStatistics.CooldownReset = timer + SWP.DebuffDuration;

            while (timer < (CalculationOptions.FightLength * 60f))
            {
                Spell spell = GetCastSpell(timer);
                if (spell == null)
                {
                    timer += 0.1f;
                    continue;
                }

                CastList.Add(spell);

                if (bPnS && spell == MF)
                    SWP.SpellStatistics.CooldownReset = timer + SWP.DebuffDuration;
                else if (spell.DebuffDuration > 0f || spell.Cooldown > 0f)
                    spell.SpellStatistics.CooldownReset = timer + (spell.DebuffDuration > spell.Cooldown ? spell.DebuffDuration : spell.Cooldown);

                timer += (spell.CastTime > 0) ? spell.CastTime : spell.GlobalCooldown;

                if (spell == SpellPriority[sequence])
                    sequence++;
                else
                    sequence = 0;
                if (SpellPriority[sequence] == MF)
                {   // Spell sequence just reset, lets take advantage of that.
                    int i = SpellPriority.IndexOf(MF);
                    CastList.RemoveRange(CastList.Count - i, i);
                    break;
                }
            }


            timer = 0;
            foreach (Spell spell in CastList)
            {
                timer += (spell.CastTime > 0) ? spell.CastTime : spell.GlobalCooldown;
                float Damage = spell.AvgDamage;
                float Cost = spell.ManaCost - simStats.ManaRestorePerCast_5_15 * 0.05f;
                switch (spell.Name)
                {
                    case "Vampiric Touch":
                    case "Mind Flay":
                        // Reapplyable DoTs, a resist means you lose 1 GCD to reapply. (~= cost of 1/2 Mind Flay)
                        Damage -= MF.AvgDamage / 2 * (1f - ShadowHitChance / 100f);
                        // Also invokes a mana penalty by needing to cast it again.
                        Cost *= (2f - ShadowHitChance / 100f);
                        break;
                    case "Shadow Word: Pain":
                        // SW:Pain follows same rules as other reapplyable dots, but needs special handling with
                        // Pain and Suffering.
                        if (bPnS)
                            break;
                        Damage -= MF.AvgDamage / 2 * (1f - ShadowHitChance / 100f);
                        Cost *= (2f - ShadowHitChance / 100f);
                        break;
                    case "Mind Blast":
                    case "Devouring Plague":
                    case "Shadow Word: Death":
                        // Spells that have cooldowns, a resist means full loss of damage.
                        Damage *= ShadowHitChance / 100f;
                        break;
                    default:
                        break;
                }
                if (bTwistedFaith && (spell == MF || spell == MB))
                    Damage *= (1f + character.PriestTalents.TwistedFaith * 0.02f);
                Damage *= (1f + simStats.BonusShadowDamageMultiplier);
                spell.SpellStatistics.DamageDone += Damage;
                spell.SpellStatistics.ManaUsed += Cost;
                OverallDamage += Damage;
                OverallMana += Cost;
            }

            DPS = OverallDamage / timer + (bPnS ? SWP.DpS * (1f + simStats.BonusShadowDamageMultiplier) : 0);
            if (simStats.TimbalsProc > 0.0f)
            {
                int dots = (MF != null)?3:0;
                foreach (Spell spell in SpellPriority)
                    if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                Spell Timbal = new Timbal(simStats, character);

                DPS += Timbal.AvgDamage / (15f + 3f / (1f - (float)Math.Pow(1f - 0.1f, dots))) * (1f + simStats.BonusShadowDamageMultiplier) * ShadowHitChance / 100f;
            }
            
            DPS *= (1f - CalculationOptions.TargetLevel * 0.02f); // Level based Partial resists.

            float MPS = OverallMana / timer;
            float TimeInFSR = 1f;
            float regen = (calculatedStats.RegenInFSR * TimeInFSR + calculatedStats.RegenOutFSR * (1f - TimeInFSR)) / 5;
            regen += simStats.Mana / (CalculationOptions.FightLength * 60f);
            regen += simStats.Mana * 0.4f * (CalculationOptions.Shadowfiend / 100f) / ((5f - character.PriestTalents.ImprovedFade * 1f) * 60f);
            regen += simStats.Mana * 0.0025f * (CalculationOptions.Replenishment / 100f);
            SustainDPS = (MPS < regen) ? DPS : (DPS * regen / MPS);

        }
    }

    public class Solver : SolverBase
    {
        protected float ShadowHitChance { get; set; }
        protected float ShadowCritChance { get; set; }

        public Solver(Stats BasicStats, Character character)
            : base(BasicStats, character)
        {

            ShadowHitChance = (float)Math.Min(100f, HitChance + character.PriestTalents.ShadowFocus * 1f);
        }

        public override void Calculate(CharacterCalculationsShadowPriest calculatedStats)
        {
            if(SpellPriority.Count == 0)
                return;

            Stats currentStats;
            Random rnd = new Random(DateTime.Now.Millisecond);
            int castCount = 0, critableCount = 0, ssCount = 0;
            float timer = 0.0f, drumsTimer = 0.0f, drumsUpTimer = 0.0f;
            
            while (timer < CalculationOptions.FightLength * 60f)
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
                    currentStats.HasteRating += 80.0f;
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
                spell.SpellStatistics.ManaUsed += seqspell.ManaCost;
                Sequence.Add(timer, seqspell);
                timer += seqspell.CastTime > 0.0f ? seqspell.CastTime : seqspell.GlobalCooldown;
                castCount++;
            }

            int missCount = (int)Math.Round((Sequence.Count - ssCount) * (100.0f - ShadowHitChance) / 100.0f);
            int mbmissCount = (int)Math.Round((double)missCount / 2.0f);
            int critCount = (int)Math.Round(critableCount * new MindBlast(PlayerStats, character).CritChance / 100.0f);
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
                OverallMana += spell.SpellStatistics.ManaUsed;
            }

            DPS = OverallDamage / (CalculationOptions.FightLength * 60f);
            SustainDPS = OverallMana / (CalculationOptions.FightLength * 60f); // Meeehh
        }
    }
}
