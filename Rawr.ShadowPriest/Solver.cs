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

        public class SpellComparerDpCT: IComparer<Spell>
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

        public class SpellComparerDpM : IComparer<Spell>
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
                return (int)Math.Round((b.DpM - a.DpM) * 100f);
            }
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
                if (se.Stats.ManaRestore == 0 && se.Stats.Mp5 == 0)
                {
                    if (se.Trigger == Trigger.Use)
                        Twinkets += se.GetAverageStats();
                    else if (se.Trigger == Trigger.DamageSpellCast
                        || se.Trigger == Trigger.SpellCast)
                    {
                        if (se.Stats.HasteRating > 0)
                        {
                            Twinkets += se.GetAverageStats(2f, 1f);
                        }
                        else if (se.Stats.HighestStat > 0)
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

            #region old
            /*
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
            if (playerStats.SpellHasteFor10SecOnCast_10_45 > 0.0f)
                // HACK FOR EMBRACE OF THE SPIDER. I HATE HASTE.
                Twinkets.HasteRating += playerStats.SpellHasteFor10SecOnCast_10_45 * 10f / 75f;
            // This is a very very wrong way of adding haste from Trinkets, due to the multiplicative nature of Haste.
            Twinkets.SpellHaste += StatConversion.GetSpellHasteFromRating(Twinkets.HasteRating);
             */
            #endregion
            if (Twinkets.HasteRating > 0)
            {
                playerStats.SpellHaste -= StatConversion.GetSpellHasteFromRating(playerStats.HasteRating);
                playerStats.SpellHaste += StatConversion.GetSpellHasteFromRating(playerStats.HasteRating + Twinkets.HasteRating);
            }

            Twinkets.Spirit = (float)Math.Round(Twinkets.Spirit * (1 + playerStats.BonusSpiritMultiplier));
            Twinkets.Intellect = (float)Math.Round(Twinkets.Intellect * (1 + playerStats.BonusIntellectMultiplier));
            Twinkets.SpellPower += (float)Math.Round(Twinkets.Spirit * playerStats.SpellDamageFromSpiritPercentage);
            playerStats += Twinkets;

            CalculationOptions = character.CalculationOptions as CalculationOptionsShadowPriest;

            HitChance = playerStats.SpellHit * 100f + CalculationOptions.TargetHit;
            if (!character.ActiveBuffsConflictingBuffContains("Spell Hit Chance Taken"))
                HitChance += character.PriestTalents.Misery * 1f;
            if (character.Race == Character.CharacterRace.Draenei && !character.ActiveBuffsContains("Heroic Presence"))
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
            SustainDPS = 0;
        }

        public Spell GetCastSpell(float timer)
        {
            foreach (Spell spell in SpellPriority)
            {
                if ((spell.DebuffDuration > 0) && (spell.CastTime > 0) && (spell.SpellStatistics.CooldownReset < (spell.CastTime + timer)))
                    return spell;   // Special case for dots that have cast time (Holy Fire / Vampiric Touch)
                if (spell.SpellStatistics.CooldownReset <= timer && spell.Cooldown > 0)
                    return spell;
                if (spell.SpellStatistics.CooldownReset > 0 
                    && (spell.SpellStatistics.CooldownReset - (spell.DebuffDuration > 0 ? spell.CastTime : 0) - timer < spell.GlobalCooldown))
                    return null;
                if (spell.SpellStatistics.CooldownReset <= timer)
                    return spell;
            }
            return null;
        }

        public Spell GetSpellByName(string name)
        {
            foreach (Spell spell in SpellPriority)
            {
                if (spell.Name.Contains(name))
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
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            foreach (string spellname in CalculationOptions.SpellPriority)
            {
                Spell spelltmp = SpellFactory.CreateSpell(spellname, PlayerStats, character);
                if (spelltmp != null) SpellPriority.Add(spelltmp);
            }

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
            // This mean we cast SW:P once, and reap the benefits later. Only use PnS if SWP is in the prio list.
            bPnS = (SWP != null) && (character.PriestTalents.PainAndSuffering > 0);
            if (bPnS)
            {   // Move SW:Pain to the end of the list in a very non-fashionable and hacky way.
                SpellPriority.Remove(SWP);
                SpellPriority.Add(SWP);
            }


            Name = "Shadow w/Mind Flay";
            Rotation = "Priority Based:";
            foreach (Spell spell in SpellPriority)
                Rotation += "\r\n- " + spell.Name;
            ShadowHitChance = (float)Math.Min(100f, HitChance + character.PriestTalents.ShadowFocus * 1f);
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
            {
                SWP.SpellStatistics.CooldownReset = timer + SWP.DebuffDuration;
                SWP.SpellStatistics.HitCount++;
                SWP.SpellStatistics.ManaUsed = SWP.ManaCost;
            }

            if (VE != null)
            {
                VE.SpellStatistics.HitCount = CalculationOptions.FightLength;
                VE.SpellStatistics.ManaUsed = CalculationOptions.FightLength * VE.ManaCost;
            }

            #region Pass 1: Create the cast sequence
            bool CleanBreak = false;
            while (timer < (60f * 60f)) // Instead of  CalculationOptions.FightLength, try to use a 60 minute fight.
            {
                Spell spell = GetCastSpell(timer);
                if (spell == null)
                {
                    timer += 0.1f;
                    continue;
                }

                CastList.Add(spell);
                timer += CalculationOptions.Delay / 1000f;
                spell.SpellStatistics.HitCount++;

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
                    CleanBreak = true;
                    break;
                }
            }
            #endregion

            #region Pass 2: Calculate Statistics for Procs
            timer = 0;
            float CastsPerSecond = 0, HitsPerSecond = 0, CritsPerSecond = 0;
            foreach (Spell spell in CastList)
            {
                timer += (spell.CastTime > 0) ? spell.CastTime : spell.GlobalCooldown;
                timer += CalculationOptions.Delay / 1000f;
                CastsPerSecond++;
                HitsPerSecond += ShadowHitChance / 100f;
                if (spell == SWD || spell == MB)
                    CritsPerSecond++;
                //if (spell == MF)
                //    HitsPerSecond += 2;   // MF can hit 3 times / cast
            }
            CastsPerSecond /= timer;
            CritsPerSecond /= timer;
            HitsPerSecond /= timer;

            // Deal with Spirit Tap
            float STCrit = 0f;
            if (MB != null)
                STCrit += MB.CritChance;
            if (SWD != null)
                STCrit += SWD.CritChance;
            if (MB != null && SWD != null)
                STCrit /= 2;
            float ImpSTUptime = CritsPerSecond * STCrit * 8f;    // Spirit Tap lasts 8 seconds. Very little overlap time due to CD on MB/SW:D
            float NewSpirit = simStats.Spirit * ImpSTUptime * character.PriestTalents.ImprovedSpiritTap * 0.05f;
            float NewSPP = NewSpirit * simStats.SpellDamageFromSpiritPercentage;
            simStats.Spirit += NewSpirit;
            simStats.SpellPower += NewSPP;
            simStats.SpellPower += simStats.Spirit * (character.PriestTalents.GlyphofShadow ? 0.1f : 0f);

            // Deal with Twinkets
            foreach (SpecialEffect se in simStats.SpecialEffects())
            {
                if (se.Stats.SpellPower > 0)
                {
                    if (se.Trigger == Trigger.SpellCast
                        || se.Trigger == Trigger.DamageSpellCast)
                        simStats.SpellPower += se.GetAverageStats(1f / CastsPerSecond, 1f, 0f, CalculationOptions.FightLength * 60).SpellPower;
                    else if (se.Trigger == Trigger.SpellHit
                        || se.Trigger == Trigger.DamageSpellHit)
                        simStats.SpellPower += se.GetAverageStats(1f / CastsPerSecond, ShadowHitChance / 100f, 0f, CalculationOptions.FightLength * 60).SpellPower;
                    else if (se.Trigger == Trigger.SpellCrit
                        || se.Trigger == Trigger.DamageSpellCrit)
                        simStats.SpellPower += se.GetAverageStats(1f / CastsPerSecond, simStats.SpellCrit, 0f, CalculationOptions.FightLength * 60).SpellPower;
                }
            }
            #region old
                /*
            // HASTE IS NOT REEVALUATED SO DONT EVEN TRY.
            if (simStats.SpellPowerFor10SecOnHit_10_45 > 0)
            {   // These have 10% Proc Chance (Sundial of the Exiled)
                float ProcChance = 0.1f;
//                float ProcCumulative = 1f / ProcChance / HitsPerSecond; // This is how many seconds you need if chance would be cumulative.
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance); // This is the real procchance after the Cumulative chance.
                float EffCooldown = 45f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / HitsPerSecond / ProcActual; 
                simStats.SpellPower += simStats.SpellPowerFor10SecOnHit_10_45 * 10f / EffCooldown;
            }
            if (simStats.SpellPowerFor10SecOnCast_15_45 > 0)
            {   // 15% Proc Chance (Dying Curse)
                float ProcChance = 0.15f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance);
                float EffCooldown = 45f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / CastsPerSecond / ProcActual;
                simStats.SpellPower += simStats.SpellPowerFor10SecOnCast_15_45 * 10f / EffCooldown;
            }
                 */
            #endregion
            #endregion

            #region Pass 3: Redo Stats for all spells
            foreach (Spell spell in SpellPriority)
            {
                spell.Calculate(simStats, character);
            }
            #endregion


            #region Pass 4: Calculate Damage and Mana Usage
            foreach (Spell spell in CastList)
            {
                float Damage = spell.AvgDamage;
                float Cost = spell.ManaCost;
                switch (spell.Name)
                {
                    case "Vampiric Touch":
                        // Reapplyable DoTs, a resist means you lose 1 GCD to reapply. (~= cost of 1 GCD worth of MF)
                        Damage -= MF.DpS * MF.GlobalCooldown * (1f - ShadowHitChance / 100f);
                        // Also invokes a mana penalty by needing to cast it again.
                        Cost *= (2f - ShadowHitChance / 100f);
                        break;
                    case "Mind Flay":
                        // Reapplyable DoTs, a resist means you lose 1 GCD to reapply. (~= cost of 1 GCD worth of MF)
                        Damage -= MF.DpS * MF.GlobalCooldown * (1f - ShadowHitChance / 100f);
                        if ((character.PriestTalents.GlyphofShadowWordPain) && (SWP != null))
                            Damage *= 1.1f;
                        // Also invokes a mana penalty by needing to cast it again.
                        Cost *= (2f - ShadowHitChance / 100f);
                        break;
                    case "Shadow Word: Pain":
                        // SW:Pain follows same rules as other reapplyable dots, but needs special handling with
                        // Pain and Suffering.
                        if (bPnS)
                            break;
                        Damage -= MF.DpS * MF.GlobalCooldown * (1f - ShadowHitChance / 100f);
                        Cost *= (2f - ShadowHitChance / 100f);
                        break;
                    case "Mind Blast":
                    case "Shadow Word: Death":
                        // Spells that have cooldowns, a resist means full loss of damage.
                        Damage *= ShadowHitChance / 100f;
                        break;
                    case "Devouring Plague":
                        Damage *= ShadowHitChance / 100f;
                        Damage *= (1f + simStats.BonusDiseaseDamageMultiplier);
                        break;
                    default:
                        break;
                }
                if (bTwistedFaith && (spell == MF || spell == MB))
                    Damage *= (1f + character.PriestTalents.TwistedFaith * 0.02f);
                Damage *= (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier);
                spell.SpellStatistics.DamageDone += Damage;
                spell.SpellStatistics.ManaUsed += Cost;
                OverallDamage += Damage;
                OverallMana += Cost;
            }
            #endregion

            if (!CleanBreak)
                Rotation += "\r\nWARNING: Did not find a clean rotation!\r\nThis may make Haste inaccurate!";
            Rotation += string.Format("\r\nRotation reset after {0} seconds.", Math.Round(timer, 2));

            #region Pass 5: Do spell statistics.
            foreach (Spell spell in SpellPriority)
            {
                spell.SpellStatistics.HitCount /= timer;
                spell.SpellStatistics.CritCount = spell.SpellStatistics.HitCount * spell.CritChance;
                spell.SpellStatistics.MissCount = spell.SpellStatistics.HitCount * (1 - ShadowHitChance / 100f);
                spell.SpellStatistics.ManaUsed /= timer;
                if (bPnS && spell == SWP)
                    spell.SpellStatistics.DamageDone = spell.DpS * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier);
                else
                    spell.SpellStatistics.DamageDone /= timer;
            }
            #endregion

            DPS = OverallDamage / timer + (bPnS ? SWP.DpS * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) : 0);

            // Finalize Trinkets
           foreach (SpecialEffect se in simStats.SpecialEffects())
            {
                if (se.Trigger == Trigger.DoTTick)
                {
                    int dots = 0;
                    foreach (Spell spell in SpellPriority)
                        if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                    if (se.Stats.ShadowDamage > 0)
                        DPS += se.GetAverageStats(dots / 3, 1f, 0f, CalculationOptions.FightLength * 60f).ShadowDamage
                            * (1f + simStats.BonusShadowDamageMultiplier)
                            * (1f + simStats.BonusDamageMultiplier)
                            * (character.PriestTalents.ShadowWeaving > 0 ? 1.1f : 1.0f)
                            * (1f + character.PriestTalents.Darkness * 0.02f)
                            * (1f + character.PriestTalents.Shadowform * 0.15f);
                }
                else if (se.Trigger == Trigger.DamageSpellHit
                    || se.Trigger == Trigger.SpellHit)
                {
                    if (se.Stats.ShadowDamage > 0)
                        DPS += se.GetAverageStats(1f / CastsPerSecond, 1f, 0f, CalculationOptions.FightLength * 60f).ShadowDamage
                            * (1f + simStats.BonusShadowDamageMultiplier)
                            * (1f + simStats.BonusDamageMultiplier)
                            * (character.PriestTalents.ShadowWeaving > 0 ? 1.1f : 1.0f)
                            * (1f + character.PriestTalents.Darkness * 0.02f)
                            * (1f + character.PriestTalents.Shadowform * 0.15f);
                }
            }
            #region old
                /*
            if (simStats.TimbalsProc > 0.0f)
            {   // 10% proc chance, 15s internal cd, shoots a Shadow Bolt
                //int dots = (MF != null)?3:0; // Apparently Flay no longer procs this
                int dots = 0;
                foreach (Spell spell in SpellPriority)
                    if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                Spell Timbal = new TimbalProc(simStats, character);
                float ProcChance = 0.1f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance);
                float EffCooldown = 16.5f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / (dots / 3) / ProcActual;

                DPS += Timbal.AvgDamage / EffCooldown * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * ShadowHitChance / 100f;
                //DPS += Timbal.AvgDamage / (15f + 3f / (1f - (float)Math.Pow(1f - 0.1f, dots))) * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * ShadowHitChance / 100f;
            }
            if (simStats.ExtractOfNecromanticPowerProc > 0.0f)
            {   // 10% proc chance, 15s internal cd, shoots a Shadow Bolt
                // Although, All dots tick about every 3s, so in avg cooldown gains another 1.5s, putting it at 16.5
                int dots = 0;
                foreach (Spell spell in SpellPriority)
                    if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                Spell Extract = new ExtractProc(simStats, character);
                float ProcChance = 0.1f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance);
                float EffCooldown = 16.5f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / (dots / 3) / ProcActual;

                DPS += (Extract.AvgDamage / EffCooldown) * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * ShadowHitChance / 100f;
                //DPS += Extract.AvgDamage / (16.5f + 3f / (1f - (float)Math.Pow(1f - 0.1f, dots))) * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * ShadowHitChance / 100f;
            }
            if (simStats.PendulumOfTelluricCurrentsProc > 0.0f)
            {   // 15% proc chance, 45s internal cd, shoots a Shadow bolt
                float ProcChance = 0.15f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance); // This is the real procchance after the Cumulative chance.
                float EffCooldown = 45f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / HitsPerSecond / ProcActual;
                simStats.SpellPower += simStats.SpellPowerFor10SecOnHit_10_45 * 10f / EffCooldown;
                Spell Pendulum = new PendulumProc(simStats, character);
                DPS += Pendulum.AvgDamage / EffCooldown * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * ShadowHitChance / 100f;
            }
                 */
            #endregion

            Rotation += "\r\n\r\nMana Buffs:";

            SustainDPS = DPS;
            float MPS = OverallMana / timer;
            float SpiritRegen = (float)Math.Floor(StatConversion.GetSpiritRegenSec(simStats.Spirit, simStats.Intellect));
            float regen = 0, tmpregen = 0;
            tmpregen = SpiritRegen * simStats.SpellCombatManaRegeneration * (CalculationOptions.FSRRatio / 100f);
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("Meditation", tmpregen));
                regen += tmpregen;
            }
            tmpregen = SpiritRegen * (1f - CalculationOptions.FSRRatio / 100f);
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("OutFSR", tmpregen));
                regen += tmpregen;
            }
            tmpregen = simStats.Mp5 / 5;
            ManaSources.Add(new ManaSource("MP5", tmpregen));
            regen += tmpregen;
            tmpregen = SpiritRegen * character.PriestTalents.ImprovedSpiritTap * 0.1f * ImpSTUptime;
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("Imp. Spirit Tap", tmpregen));
                regen += tmpregen;
            }
            tmpregen = simStats.Mana / (CalculationOptions.FightLength * 60f);
            ManaSources.Add(new ManaSource("Intellect", tmpregen));
            regen += tmpregen;
            tmpregen = simStats.Mana * 0.0025f * (CalculationOptions.Replenishment / 100f);
            ManaSources.Add(new ManaSource("Replenishment", tmpregen));
            regen += tmpregen;
            tmpregen = SWP.BaseMana * (simStats.ManaRestoreFromBaseManaPerHit > 0 ? 0.02f * 0.25f : 0f) * HitsPerSecond * (CalculationOptions.JoW / 100f);
            if (tmpregen > 0)
            {
                ManaSources.Add(new ManaSource("Judgement of Wisdom", tmpregen));
                regen += tmpregen;
            }
            foreach (SpecialEffect se in simStats.SpecialEffects())
            {
                if (se.Stats.ManaRestore > 0 || se.Stats.Mp5 > 0)
                {
                    if (se.Trigger == Trigger.Use)
                        tmpregen = se.GetAverageStats().ManaRestore
                            + se.GetAverageStats().Mp5 / 5;
                    else if (se.Trigger == Trigger.SpellCast
                        || se.Trigger == Trigger.DamageSpellCast)
                        tmpregen = se.GetAverageStats(1f / CastsPerSecond, 1f, 0f, CalculationOptions.FightLength * 60).ManaRestore
                            + se.GetAverageStats(1f / CastsPerSecond, 1f, 0f, CalculationOptions.FightLength * 60f).Mp5 / 5;
                    else if (se.Trigger == Trigger.SpellHit
                        || se.Trigger == Trigger.DamageSpellHit)
                        tmpregen = se.GetAverageStats(1f / CastsPerSecond, ShadowHitChance / 100f, 0f, CalculationOptions.FightLength * 60).ManaRestore
                            + se.GetAverageStats(1f / CastsPerSecond, ShadowHitChance / 100f, 0f, CalculationOptions.FightLength * 60).Mp5 / 5;
                    else if (se.Trigger == Trigger.SpellCrit
                        || se.Trigger == Trigger.DamageSpellCrit)
                        tmpregen = se.GetAverageStats(1f / CastsPerSecond, simStats.SpellCrit, 0f, CalculationOptions.FightLength * 60).ManaRestore
                            + se.GetAverageStats(1f / CastsPerSecond, simStats.SpellCrit, 0f, CalculationOptions.FightLength * 60).Mp5 / 5;
                    else
                        tmpregen = 0;
                    if (tmpregen > 0)
                    {
                        ManaSources.Add(new ManaSource(se.ToString(), tmpregen));
                        regen += tmpregen;
                    }
                }
            }

            if (MPS > regen && character.Race == Character.CharacterRace.BloodElf)
            {   // Arcane Torrent is 6% max mana every 2 minutes.
                tmpregen = simStats.Mana * 0.06f / 120f;
                ManaSources.Add(new ManaSource("Arcane Torrent", tmpregen));
                regen += tmpregen;
                Rotation += "\r\n- Used Arcane Torrent";
            }

            if (MPS > regen && CalculationOptions.ManaAmt > 0)
            {   // Not enough mana, use Mana Potion
                tmpregen = CalculationOptions.ManaAmt / (CalculationOptions.FightLength * 60f) * (1f + simStats.BonusManaPotion);
                ManaSources.Add(new ManaSource("Mana Potion", tmpregen));
                regen += tmpregen;
                Rotation += "\r\n- Used Mana Potion";
            }

            if (MPS > regen)
            {   // Not enough mana, use Shadowfiend
                float sf_rat = (CalculationOptions.Shadowfiend / 100f) / ((5f - character.PriestTalents.VeiledShadows * 1f) * 60f);
                tmpregen = simStats.Mana * 0.5f * sf_rat;
                ManaSources.Add(new ManaSource("Shadowfiend", tmpregen));
                regen += tmpregen;
                SustainDPS -= MF.DpS * sf_rat;
                Rotation += "\r\n- Used Shadowfiend";
            }

            if (MPS > regen && character.PriestTalents.Dispersion > 0)
            {   // Not enough mana, so eat a Dispersion, remove DPS worth of 6 seconds of mindflay.
                float disp_rat = 6f / (60f * 3f);
                tmpregen = simStats.Mana * 0.06f * disp_rat;
                ManaSources.Add(new ManaSource("Dispersion", tmpregen));
                regen += tmpregen;
                SustainDPS -= MF.DpS * disp_rat;
                Rotation += "\r\n- Used Dispersion";
            }

            DPS *= (1f - StatConversion.GetAverageResistance(character.Level, character.Level + CalculationOptions.TargetLevel, 0, 0)); // Level based Partial resists.
            SustainDPS *= (1f - CalculationOptions.TargetLevel * 0.02f);

            SustainDPS = (MPS < regen) ? SustainDPS : (SustainDPS * regen / MPS);

            calculatedStats.DpsPoints = DPS;
            calculatedStats.SustainPoints = SustainDPS;

            // Lets just say that 15% of resilience scales all health by 150%.
            float Resilience = (float)Math.Min(15f, StatConversion.GetResilienceFromRating(simStats.Resilience) * 100f) / 15f;
            calculatedStats.SurvivalPoints = calculatedStats.BasicStats.Health * (Resilience * 1.5f + 1f) * CalculationOptions.Survivability / 100f;
        }
    }
   
    public class SolverHoly : SolverBase
    {   // Models Full Rotation
        protected Spell SWP { get; set; }
        protected Spell SWD { get; set; }
        protected Spell MB { get; set; }
        protected Spell DP { get; set; }
        protected Spell SM { get; set; }
        protected Spell PE { get; set; }

        public SolverHoly(Stats BasicStats, Character character)
            : base(BasicStats, character)
        {

            SpellPriority = new List<Spell>(Spell.HolySpellList.Count);
            SpellComparerDpCT _scDpCT = new SpellComparerDpCT();
            SpellComparerDpM _scDpM = new SpellComparerDpM();
            foreach (string spellname in Spell.HolySpellList)
            {
                Spell spelltmp = SpellFactory.CreateSpell(spellname, PlayerStats, character);
                if (spelltmp != null) SpellPriority.Add(spelltmp);
            }

            SWP = GetSpellByName("Shadow Word: Pain");
            SWD = GetSpellByName("Shadow Word: Death");
            MB = GetSpellByName("Mind Blast");
            DP = GetSpellByName("Devouring Plague");
            SM = GetSpellByName("Smite");
            PE = GetSpellByName("Penance");

            // Sort spells and remove any spell with worse mana efficiency than smite.
            SpellPriority.Sort(_scDpM);
            int i = SpellPriority.IndexOf(SM) + 1;
            SpellPriority.RemoveRange(i, SpellPriority.Count - i);

            // Sort spells, and remove any spell with worse damage pr cast time than smite.
            SpellPriority.Sort(_scDpCT);
            i = SpellPriority.IndexOf(SM) + 1; 
            SpellPriority.RemoveRange(i, SpellPriority.Count - i);

            if (character.PriestTalents.Penance > 0)
                Name = "Holy Smite w/Penance";
            else
                Name = "Holy Smite";
            Rotation = "Priority Based:";
            foreach (Spell spell in SpellPriority)
                Rotation += "\r\n- " + spell.Name;
            Rotation += "\r\n\r\nMana Buffs:";
        }

        public override void Calculate(CharacterCalculationsShadowPriest calculatedStats)
        {
            if (SpellPriority.Count == 0)
                return;

            Stats simStats = PlayerStats.Clone();
            float timer = 0;
            int sequence = SpellPriority.Count - 1;
            List<Spell> CastList = new List<Spell>();

            while (timer < (CalculationOptions.FightLength * 60f))
            {
                Spell spell = GetCastSpell(timer);
                if (spell == null)
                {
                    timer += 0.1f;
                    continue;
                }

                CastList.Add(spell);

                if (spell.DebuffDuration > 0f || spell.Cooldown > 0f)
                    spell.SpellStatistics.CooldownReset = timer + (spell.DebuffDuration > spell.Cooldown ? spell.DebuffDuration : spell.Cooldown);

                timer += (spell.CastTime > 0) ? spell.CastTime : spell.GlobalCooldown;

                if (spell == SpellPriority[sequence])
                    sequence++;
                else
                    sequence = 0;
                if (SpellPriority[sequence] == SM)
                {   // Spell sequence just reset, lets take advantage of that.
                    int i = SpellPriority.IndexOf(SM);
                    CastList.RemoveRange(CastList.Count - i, i);
                    break;
                }
            }

            float ShadowHitChance = (float)Math.Min(HitChance + character.PriestTalents.ShadowFocus * 0.01f, 1f);
            timer = 0;
            foreach (Spell spell in CastList)
            {
                timer += (spell.CastTime > 0) ? spell.CastTime : spell.GlobalCooldown;
                float Damage = spell.AvgDamage;
                float Cost = spell.ManaCost - simStats.ManaRestoreOnCast_5_15 * 0.05f;
                if (spell.Name == "Smite")
                {
                    Damage -= SM.DpS * SM.GlobalCooldown * (1f - HitChance / 100f);
                    Cost *= (2f - HitChance / 100f);
                    if (character.PriestTalents.GlyphofSmite)
                        Damage *= (1f + 0.2f * 7 / 10);
                }
                else if (spell.Name == "Shadow Word: Pain")
                {
                    Damage -= SM.DpS * SM.GlobalCooldown * (1f - ShadowHitChance / 100f);
                    Cost *= (2f - ShadowHitChance / 100f);
                }
                else if (spell.Name == "Holy Fire")
                {
                    Damage *= HitChance / 100f;
                }
                else if (spell.Name == "Mind Blast")
                {
                    Damage *= ShadowHitChance / 100f;
                }
                else if (spell.Name == "Shadow Word: Death")
                {
                    Damage *= ShadowHitChance / 100f;
                }
                else if (spell.Name == "Penance")
                {
                    Damage *= HitChance / 100f;
                }
                else if (spell.Name == "Devouring Plague")
                {
                    Damage *= ShadowHitChance / 100f;
                    Damage *= (1f + simStats.BonusDiseaseDamageMultiplier);
                }

                Damage *= (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier); // If there is shadow bonus damage there is holy bonus damage.
                spell.SpellStatistics.DamageDone += Damage;
                spell.SpellStatistics.ManaUsed += Cost;
                OverallDamage += Damage;
                OverallMana += Cost;
            }

            DPS = OverallDamage / timer;
            if (simStats.TimbalsProc > 0.0f)
            {
                int dots = (PE != null) ? 3 : 0;
                foreach (Spell spell in SpellPriority)
                    if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                Spell Timbal = new TimbalProc(simStats, character);

                DPS += Timbal.AvgDamage / (15f + 3f / (1f - (float)Math.Pow(1f - 0.1f, dots))) * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * HitChance / 100f;
            }

            SustainDPS = DPS;
            float MPS = OverallMana / timer;
            float TimeInFSR = 1f;
            float regen = (calculatedStats.RegenInFSR * TimeInFSR + calculatedStats.RegenOutFSR * (1f - TimeInFSR)) / 5;
            regen += simStats.Mana / (CalculationOptions.FightLength * 60f);
            regen += simStats.Mana * 0.0025f * (CalculationOptions.Replenishment / 100f);

            if (MPS > regen && CalculationOptions.ManaAmt > 0)
            {   // Not enough mana, use Mana Potion
                regen += CalculationOptions.ManaAmt / (CalculationOptions.FightLength * 60f) * (1f + simStats.BonusManaPotion);
                Rotation += "\r\n- Used Mana Potion";
            }

            if (MPS > regen)
            {   // Not enough mana, use Shadowfiend
                float sf_rat = (CalculationOptions.Shadowfiend / 100f) / ((5f - character.PriestTalents.VeiledShadows * 1f) * 60f);
                regen += simStats.Mana * 0.4f * sf_rat;
                SustainDPS -= SM.DpS * sf_rat;
                Rotation += "\r\n- Used Shadowfiend";
            }

            if (MPS > regen)
            {   // Not enough still, use Hymn of Hope
                float hh_rat = 8f / (60f * 5f);
                regen += simStats.Mana * 0.02f * hh_rat;
                SustainDPS -= SM.DpS * hh_rat;
                Rotation += "\r\n- Used Hymn of Hope";
            }

            DPS *= (1f - CalculationOptions.TargetLevel * 0.02f); // Level based Partial resists.
            SustainDPS *= (1f - CalculationOptions.TargetLevel * 0.02f);

            SustainDPS = (MPS < regen) ? SustainDPS : (SustainDPS * regen / MPS);

            calculatedStats.DpsPoints = DPS;
            calculatedStats.SustainPoints = SustainDPS;
            // If opponent has 25% crit, each 39.42308044 resilience gives -1% damage from dots and -1% chance to be crit. Also reduces crits by 2%.
            // This effectively means you gain 12.5% extra health from removing 12.5% dot and 12.5% crits at resilience cap (492.5 (39.42308044*12.5))
            // In addition, the remaining 12.5% crits are reduced by 25% (12.5%*200%damage*75% = 18.75%)
            // At resilience cap I'd say that your hp's are scaled by 1.125*1.1875 = ~30%. Probably wrong but good enough.
            calculatedStats.SurvivalPoints = calculatedStats.BasicStats.Health * CalculationOptions.Survivability / 100f * (1 + 0.3f * calculatedStats.BasicStats.Resilience / 492.7885f);
        }
    }


// Decrepated

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
        {/*
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
            SustainDPS = OverallMana / (CalculationOptions.FightLength * 60f); // Meeehh*/
        }
    }
}
