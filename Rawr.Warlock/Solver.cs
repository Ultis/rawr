using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    public class SolverBase
    {
        public List<Spell> SpellPriority { get; protected set; }
        public float OverallDamage { get; protected set; }
        public float DPS { get; protected set; }
        public float OverallMana { get; protected set; }
        public float SustainDPS { get; protected set; }
        public Dictionary<float, Spell> Sequence { get; protected set; }

        public CalculationOptionsWarlock CalculationOptions { get; set; }
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
            Twinkets.SpellHaste += character.StatConversion.GetSpellHasteFromRating(Twinkets.HasteRating) / 100f;


            PlayerStats = playerStats + Twinkets;
            CalculationOptions = character.CalculationOptions as CalculationOptionsWarlock;

            HitChance = PlayerStats.SpellHit * 100f + CalculationOptions.TargetHit;
            if (character.Race == Character.CharacterRace.Draenei && !character.ActiveBuffsContains("Heroic Presence"))
                HitChance += 1;
            HitChance = (float)Math.Min(100f, HitChance);

            Trinkets = new List<Trinket>();
            Sequence = new Dictionary<float, Spell>();
            ManaSources = new List<ManaSource>();
        }

        public virtual void Calculate(CharacterCalculationsWarlock calculatedStats)
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
        protected float AfflictionHitChance { get; set; }
        protected float DestructionHitChance { get; set; }
        protected Spell SB { get; set; }
        protected Spell Inc { get; set; }
        protected Spell Sb { get; set; }
        protected Spell CB { get; set; }
        protected Spell Cor { get; set; }
        protected Spell Ha { get; set; }
        protected Spell DL { get; set; }
        protected Spell Sf { get; set; }
        protected Spell SP { get; set; }
        protected Spell Con { get; set; }
        protected Spell LT { get; set; }

        protected bool bEA { get; set; }

        public SolverShadow(Stats BasicStats, Character character)
            : base(BasicStats, character)
        {
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            foreach (string spellname in CalculationOptions.SpellPriority)
            {
                Spell spelltmp = SpellFactory.CreateSpell(spellname, PlayerStats, character);
                if (spelltmp != null) SpellPriority.Add(spelltmp);
            }

            SB = GetSpellByName("Shadow Bolt");
            Inc = GetSpellByName("Incinerate");

            Sb = GetSpellByName("Shadowburn");
            CB = GetSpellByName("Chaos Bolt");
            Cor = GetSpellByName("Corruption");
            Ha = GetSpellByName("Haunt");
            DL = GetSpellByName("Drain Life");
            Sf = GetSpellByName("Shadowfury");
            SP = GetSpellByName("Searing Pain");
            Con = GetSpellByName("Conflagrate");
            LT = SpellFactory.CreateSpell("Life Tap", PlayerStats, character);

//Is this needed in any way?
            /*if (SB != null)
            {   // Functional yet abysmal method of moving VE to bottom of priority.
                SpellPriority.Remove(SB);
                SpellPriority.Add(SB);
            }*/

            // With Everlasting Affliction, Haunt and Drain Life have a 20/40/60/80/100% chance to refresh Corruption.
            // This mean we cast Corruption once, and reap the benefits later. Only use EA if Cor is in the prio list.
//different talent levels not implemented
            bEA = (Cor != null) && (character.WarlockTalents.EverlastingAffliction > 0);

            Name = "User defined";
            Rotation = "Priority Based:";
            foreach (Spell spell in SpellPriority)
                Rotation += "\r\n- " + spell.Name;
            AfflictionHitChance = (float)Math.Min(100f, HitChance + character.WarlockTalents.Suppression * 1f);
            DestructionHitChance = (float)Math.Min(100f, HitChance + character.WarlockTalents.Cataclysm * 1f);
        }

        public override void Calculate(CharacterCalculationsWarlock calculatedStats)
        {
            if (SpellPriority.Count == 0)
                return;

            Stats simStats = PlayerStats.Clone();
            bool bTwistedFaith = character.PriestTalents.TwistedFaith > 0;
            float timer = 0;
            int sequence = SpellPriority.Count - 1;
            List<Spell> CastList = new List<Spell>();

            // Initial Corruption application
            if (bEA)
            {
                Cor.SpellStatistics.CooldownReset = timer + Cor.DebuffDuration;
                Cor.SpellStatistics.HitCount++;
                Cor.SpellStatistics.ManaUsed = Cor.ManaCost;
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

                if (bEA && (spell == Ha || spell == DL))
                    Cor.SpellStatistics.CooldownReset = timer + Cor.DebuffDuration;
                if (spell.DebuffDuration > 0f || spell.Cooldown > 0f)
                    spell.SpellStatistics.CooldownReset = timer + (spell.DebuffDuration > spell.Cooldown ? spell.DebuffDuration : spell.Cooldown);

                timer += (spell.CastTime > 0) ? spell.CastTime : spell.GlobalCooldown;

                if (spell == SpellPriority[sequence])
                    sequence++;
                else
                    sequence = 0;
                if (SpellPriority[sequence] == SB)
                {   // Spell sequence just reset, lets take advantage of that.
                    int i = SpellPriority.IndexOf(SB);
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
                if (spell.SpellTree == SpellTree.Affliction) HitsPerSecond += AfflictionHitChance / 100f;
                else if (spell.SpellTree == SpellTree.Destruction) HitsPerSecond += DestructionHitChance / 100f;
                else HitsPerSecond++;
                if (spell.CritChance > 0)
                    CritsPerSecond++;
                //if (spell == MF)
                //    HitsPerSecond += 2;   // MF can hit 3 times / cast
            }
            CastsPerSecond /= timer;
            CritsPerSecond /= timer;
            HitsPerSecond /= timer;

            // Deal with Twinkets
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
                float Cost = spell.ManaCost - simStats.ManaRestoreOnCast_5_15 * 0.05f;
                switch (spell.Name)
                {
                    case "Curse of Agony":
                    case "Curse of Doom":
                    case "Corruption":
                    case "Siphon Life":
                    case "Unstable Affliction":
                    case "Drain Life":
                    case "Drain Soul":
                        // Reapplyable DoTs / channeled spells, a resist means you lose 1 GCD to reapply. (~= cost of 1 GCD worth of MF)
                        Damage -= SB.DpS * SB.GlobalCooldown * (1f - AfflictionHitChance / 100f);
                        // Also invokes a mana penalty by needing to cast it again.
                        Cost *= (2f - AfflictionHitChance / 100f);
                        break;
                    case "Shadow Bolt":
                    case "Death Coil":
                    case "Haunt":
                    case "Seed of Corruption":
                    case "Shadowflame":
                    case "Shadowburn":
                    case "Shadowfury":
                    case "Incinerate":
                    case "Immolate":
                    case "Rain of Fire":
                    case "Hellfire":
                    case "Searing Pain":
                    case "Soul Fire":
                    case "Conflagrate":
                    case "Chaos Bolt":
                        // Spells that have cooldowns, a resist means full loss of damage.
                        Damage *= AfflictionHitChance / 100f;
                        break;
                    default:
                        break;
                }
                if (spell.MagicSchool == MagicSchool.Fire)Damage *= (1f + simStats.BonusFireDamageMultiplier) * (1f + simStats.BonusDamageMultiplier);
                else Damage *= (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier);
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
                spell.SpellStatistics.MissCount = spell.SpellStatistics.HitCount * (1 - AfflictionHitChance / 100f);
                spell.SpellStatistics.ManaUsed /= timer;
                spell.SpellStatistics.DamageDone /= timer;
            }
            #endregion

            DPS = OverallDamage / timer + (bEA ? Cor.DpS * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) : 0);

            // Finalize Trinkets
            if (simStats.TimbalsProc > 0.0f)
            {   // 10% proc chance, 15s internal cd, shoots a Shadow Bolt
                int dots = 0;
                foreach (Spell spell in SpellPriority)
                    if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                Spell Timbal = new TimbalProc(simStats, character);

                DPS += Timbal.AvgDamage / (15f + 3f / (1f - (float)Math.Pow(1f - 0.1f, dots))) * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * AfflictionHitChance / 100f;
            }
            if (simStats.ExtractOfNecromanticPowerProc > 0.0f)
            {   // 10% proc chance, 15s internal cd, shoots a Shadow Bolt
                int dots = 0;
                foreach (Spell spell in SpellPriority)
                    if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                Spell Extract = new ExtractProc(simStats, character);

                DPS += Extract.AvgDamage / (15f + 3f / (1f - (float)Math.Pow(1f - 0.1f, dots))) * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * AfflictionHitChance / 100f;
            }
            if (simStats.PendulumOfTelluricCurrentsProc > 0.0f)
            {   // 15% proc chance, 45s internal cd, shoots a Shadow bolt
                float ProcChance = 0.15f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance); // This is the real procchance after the Cumulative chance.
                float EffCooldown = 45f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / HitsPerSecond / ProcActual;
                simStats.SpellPower += simStats.SpellPowerFor10SecOnHit_10_45 * 10f / EffCooldown;
                Spell Pendulum = new PendulumProc(simStats, character);
                DPS += Pendulum.AvgDamage / EffCooldown * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * AfflictionHitChance / 100f;
            }

            Rotation += "\r\n\r\nMana Buffs:";

            SustainDPS = DPS;
            float MPS = OverallMana / timer;
            float SpiritRegen = (float)Math.Floor(character.StatConversion.GetSpiritRegenSec(simStats.Spirit, simStats.Intellect));
            float regen = 0, tmpregen = 0;
            tmpregen = SpiritRegen * (1f - CalculationOptions.FSRRatio / 100f);
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("OutFSR", tmpregen));
                regen += tmpregen;
            }
            tmpregen = simStats.Mp5 / 5;
            ManaSources.Add(new ManaSource("MP5", tmpregen));
            regen += tmpregen;
            tmpregen = (CalculationOptions.Pet == "Felhunter" ? 1 : 0) * (CalculationOptions.PetSacrificed ? 1 : 0) * simStats.Mana * 0.03f / 4f;
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("Sacrificed Felhunter", tmpregen));
                regen += tmpregen;
            }
            tmpregen = (CalculationOptions.Pet == "Felguard" ? 1 : 0) * (CalculationOptions.PetSacrificed ? 1 : 0) * simStats.Mana * 0.02f / 4f;
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("Sacrificed Felguard", tmpregen));
                regen += tmpregen;
            }
            tmpregen = simStats.Mana * character.WarlockTalents.ImprovedSoulLeech * 0.3f * 0.02f *
                ( (SB != null ? SB.SpellStatistics.HitCount : 0)
                + (Sb != null ? Sb.SpellStatistics.HitCount : 0)
                + (CB != null ? CB.SpellStatistics.HitCount : 0)
                + (Sf != null ? Sf.SpellStatistics.HitCount : 0)
                + (Inc != null ? Inc.SpellStatistics.HitCount : 0)
                + (SP != null ? SP.SpellStatistics.HitCount : 0)
                + (Con != null ? Con.SpellStatistics.HitCount : 0));
            if (tmpregen > 0f)
            {
                ManaSources.Add(new ManaSource("Imp. Soul Leech", tmpregen));
                regen += tmpregen;
            }
            tmpregen = simStats.Mana / (CalculationOptions.FightLength * 60f);
            ManaSources.Add(new ManaSource("Intellect", tmpregen));
            regen += tmpregen;
            tmpregen = simStats.Mana * 0.0025f * (CalculationOptions.Replenishment / 100f);
            ManaSources.Add(new ManaSource("Replenishment", tmpregen));
            regen += tmpregen;
            tmpregen = simStats.Mana * simStats.ManaRestoreFromMaxManaPerHit * HitsPerSecond * (CalculationOptions.JoW / 100f);
            if (tmpregen > 0)
            {
                ManaSources.Add(new ManaSource("Judgement of Wisdom", tmpregen));
                regen += tmpregen;
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
            {   // Not enough mana, so use Life Tap, remove DPS worth of Life Tap cast time of the filler spell.
                tmpregen = 0;
                int numberOfTaps = 0;
                while (MPS > regen + tmpregen)
                {
                    tmpregen -= LT.ManaCost / (CalculationOptions.FightLength * 60f);
                    numberOfTaps++;
                }
                ManaSources.Add(new ManaSource("Life Tap", tmpregen));
                regen += tmpregen;
                SustainDPS -= SB.DpS * numberOfTaps * LT.CastTime;
                Rotation += string.Format("\r\n- Used {0} Life Taps", numberOfTaps);
            }

            /*            if (MPS > regen)
                        {   // Not enough mana, use Shadowfiend
                            float sf_rat = (CalculationOptions.Shadowfiend / 100f) / ((5f - character.PriestTalents.VeiledShadows * 1f) * 60f);
                            tmpregen = simStats.Mana * 0.4f * sf_rat;
                            ManaSources.Add(new ManaSource("Shadowfiend", tmpregen));
                            regen += tmpregen;
                            SustainDPS -= MF.DpS * sf_rat;
                            Rotation += "\r\n- Used Shadowfiend";
                        }*/

            DPS *= (1f - CalculationOptions.TargetLevel * 0.02f); // Level based Partial resists.
            SustainDPS *= (1f - CalculationOptions.TargetLevel * 0.02f);

            SustainDPS = (MPS < regen) ? SustainDPS : (SustainDPS * regen / MPS);

            calculatedStats.DpsPoints = DPS;
            calculatedStats.SustainPoints = SustainDPS;

            // Lets just say that 15% of resilience scales all health by 150%.
            float Resilience = (float)Math.Min(15f, character.StatConversion.GetResilienceFromRating(simStats.Resilience)) / 15f;
            calculatedStats.SurvivalPoints = calculatedStats.BasicStats.Health * (Resilience * 1.5f + 1f) * CalculationOptions.Survivability / 100f;
        }
    }
}