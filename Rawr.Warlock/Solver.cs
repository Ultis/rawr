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

        public int BackdraftCounter { get; protected set; }
        public int LTUsePercent { get; protected set; }
        public bool LTOnFiller { get; protected set; }
        public double currentMana { get; protected set; }
        public Stats simStats { get; protected set; }
        public Spell fillerSpell { get; protected set; }

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
            public double Value { get; set; }

            public ManaSource(string name, double value)
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
        protected bool NFticks { get; set; }

        public SolverShadow(Stats BasicStats, Character character)
            : base(BasicStats, character)
        {
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            foreach (string spellname in CalculationOptions.SpellPriority)
            {
                Spell spelltmp = SpellFactory.CreateSpell(spellname, PlayerStats, character);
                if (spelltmp != null) SpellPriority.Add(spelltmp);
            }

            //fillers
            SB = GetSpellByName("Shadow Bolt");
            Inc = GetSpellByName("Incinerate");

            //other spells
            Sb = GetSpellByName("Shadowburn");
            CB = GetSpellByName("Chaos Bolt");
            Cor = GetSpellByName("Corruption");
            Ha = GetSpellByName("Haunt");
            DL = GetSpellByName("Drain Life");
            Sf = GetSpellByName("Shadowfury");
            SP = GetSpellByName("Searing Pain");
            Con = GetSpellByName("Conflagrate");
            LT = SpellFactory.CreateSpell("Life Tap", PlayerStats, character);

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
            float timer = 0;
            int sequence = SpellPriority.Count - 1;
            List<Spell> CastList = new List<Spell>();
            float NFticks = 0;
            float CorPandticks = 0;

            // Initial Corruption application
            if (bEA)
            {
                Cor.SpellStatistics.CooldownReset = timer + Cor.DebuffDuration;
                Cor.SpellStatistics.HitCount++;
                Cor.SpellStatistics.ManaUsed = Cor.ManaCost;
                NFticks += Cor.DebuffDuration / Cor.TimeBetweenTicks;
                CorPandticks += Cor.DebuffDuration / Cor.TimeBetweenTicks;
            }

            #region Pass 1: Create the cast sequence
            bool CleanBreak = false;
            int calcTime = 3600;
            bool below35 = false;
            while (timer < calcTime) // Instead of  CalculationOptions.FightLength, try to use a 60 minute fight.
            {
                if (timer < calcTime * 0.35f && below35 == false)
                {
                    below35 = true;
                    CastList.Add (LT);
                }
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
                {
//                    NFticks += Math.Floor((Cor.SpellStatistics.CooldownReset - timer) / Cor.TimeBetweenTicks);
//                    CorPandticks += Math.Floor((Cor.SpellStatistics.CooldownReset - timer) / Cor.TimeBetweenTicks);
                    Cor.SpellStatistics.CooldownReset = timer + Cor.DebuffDuration;
                }
                else if (spell.DebuffDuration > 0f || spell.Cooldown > 0f)
                {
                    if (spell.Name == "Corruption")
                    {
                        NFticks += Cor.DebuffDuration / Cor.TimeBetweenTicks;
                        CorPandticks += Cor.DebuffDuration / Cor.TimeBetweenTicks;
                    }
                    else if (spell.Name == "Drain Life")
                        NFticks += Cor.DebuffDuration / Cor.TimeBetweenTicks;
//                    else if (spell.Name == " Unstable Affliction")
//                        UAPandticks += UA.DebuffDuration / UA.TimeBetweenTicks;
                    spell.SpellStatistics.CooldownReset = timer + (spell.DebuffDuration > spell.Cooldown ? spell.DebuffDuration : spell.Cooldown);
                }
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
                if (spell.Name != "LT") break;
                timer += (spell.CastTime > 0) ? spell.CastTime : spell.GlobalCooldown;
                timer += CalculationOptions.Delay / 1000f;
                CastsPerSecond++;
                if (spell.SpellTree == SpellTree.Affliction) HitsPerSecond += AfflictionHitChance / 100f;
                else if (spell.SpellTree == SpellTree.Destruction) HitsPerSecond += DestructionHitChance / 100f;
                else HitsPerSecond++;
                if (spell.CritChance > 0) CritsPerSecond++;
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
                below35 = false;
                if (spell.Name == "LT")
                {
                    below35 = true;
                    continue;
                }
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
                    case "Rain of Fire":
                    case "Hellfire":
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
                if (spell.MagicSchool == MagicSchool.Fire) Damage *= (1f + simStats.BonusFireDamageMultiplier) * (1f + simStats.BonusDamageMultiplier);
                else Damage *= (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * (1f + character.WarlockTalents.DeathsEmbrace * 0.04f);
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

            // Nightfall
            if (character.WarlockTalents.Nightfall > 0)
            {
                float NFprocs = (float)Math.Floor(NFticks * character.WarlockTalents.Nightfall * 0.02f);
                OverallDamage += SB.AvgDamage * NFprocs;
                OverallDamage -= SB.AvgDamage * NFprocs * SB.GlobalCooldown / SB.CastTime;
                OverallMana += NFprocs * SB.ManaCost - simStats.ManaRestoreOnCast_5_15 * 0.05f;
            }

            // Pandemic
/*            if (character.WarlockTalents.Pandemic > 0)
            {
                if ((character.WarlockTalents.Pandemic * 0.33f) == 0.99f ? 100f : character.WarlockTalents.Pandemic * 0.33f)
                int CorPandprocs = Math.Floor(CorPandticks * simStats.CritChance);
                int UAPandprocs = Math.Floor(UAPandticks * simStats.CritChance);
                OverallDamage += ((character.WarlockTalents.Pandemic * 0.33f) == 0.99f ? 100f : character.WarlockTalents.Pandemic * 0.33f) * Cor.AvgDamage * CorPandprocs;
                OverallDamage += ((character.WarlockTalents.Pandemic * 0.33f) == 0.99f ? 100f : character.WarlockTalents.Pandemic * 0.33f) * UA.AvgDamage * UAPandprocs;
            }*/

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
            tmpregen = simStats.Mana * simStats.ManaRestoreFromBaseManaPerHit * HitsPerSecond * (CalculationOptions.JoW / 100f);
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

    public class SolverNew : SolverBase
    {
        public EventList events { get; protected set; }
        protected float AfflictionHitChance { get; set; }
        protected float DemonologyHitChance { get; set; }
        protected float DestructionHitChance { get; set; }
        double time = 0f;
        Spell lifeTap;
        TimeSpan calcTime;

        public class EventList : SortedList<double, Event>
        {
            public new void Add(double _key, Event _Value)
            {
                double key = _key;
                foreach (double basekey in base.Keys)
                {
                    if (key == basekey) key += 0.00001f;
                }
                base.Add(key, _Value);
            }
        }

        public class Event
        {
            public string eventName { get; protected set; }
            public string name { get; protected set; }
        }

        public class DoneCastingEvent : Event
        {
            public DoneCastingEvent()
            {
                eventName = "DoneCastingEvent";
            }

            public DoneCastingEvent(Spell spell)
            {
                eventName = "DoneCastingEvent";
                name = spell.Name;
                Spell castSpell = spell;
            }
        }

        public class DotTickEvent : Event
        {
            public Spell dotSpell;

            public DotTickEvent(Spell spell)
            {
                eventName = "DotTickEvent";
                name = spell.Name;
                dotSpell = spell;
            }
        }

        public class DebuffEndEvent : Event
        {
            public DebuffEndEvent(string _debuffName)
            {
                eventName = "DebuffEndEvent";
                name = _debuffName;
            }
        }

        public Spell GetCastSpellNew(double timer, double currentMana, Stats simStats)
        {
            foreach (Spell spell in SpellPriority)
            {
                switch (spell.Name)
                {
                    case "Haunt":
                        {
                            if (spell.SpellStatistics.CooldownReset <= timer)
                            {
                                if (timer + spell.GlobalCooldown + CalculationOptions.Delay / 1000f + getCastTime(spell) > spell.SpellStatistics.CooldownReset + 4)
                                {
                                    return spell;
                                }
                                else foreach (Spell tempspell in SpellPriority)
                                {
                                    if ((tempspell.DebuffDuration > 0) && (getCastTime(tempspell) > 0) && (tempspell.SpellStatistics.CooldownReset < (getCastTime(tempspell) + timer)) && (getCastTime(tempspell) + CalculationOptions.Delay / 1000f < spell.SpellStatistics.CooldownReset + 4 - timer - getCastTime(spell)))
                                        return tempspell;   // Special case for dots that have cast time
                                    else if ((tempspell.SpellStatistics.CooldownReset) <= timer && (getCastTime(tempspell) + CalculationOptions.Delay / 1000f < spell.SpellStatistics.CooldownReset + 4 - timer - getCastTime(spell)))
                                        return tempspell;
                                    else return spell;
                                }
                            }
                            break;
                        }
                    case "Conflagrate":
                        {
                            bool immolateIsUp = false;
                            for (int index = 0; index < events.Count; index++)
                                if (events.Values[index].name == "Immolate")
                                {
                                    immolateIsUp = true;
                                    break;
                                }
                            if (immolateIsUp) return spell;
                            break;
                        }
                    default:
                        {
                            if ((spell.DebuffDuration > 0) && (getCastTime(spell) > 0) && (spell.SpellStatistics.CooldownReset < (getCastTime(spell) + timer)))
                                return spell;   // Special case for dots that have cast time
                            else if (spell.SpellStatistics.CooldownReset <= timer)
                                return spell;
                            break;
                        }
                }
            }
            return null;
        }

        public SolverNew(Stats BasicStats, Character character)
            : base(BasicStats, character)
        {
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            Spell shadowBolt = SpellFactory.CreateSpell("Shadow Bolt", PlayerStats, character);
            foreach (string spellname in CalculationOptions.SpellPriority)
            {
                Spell spelltmp = SpellFactory.CreateSpell(spellname, PlayerStats, character);
                if (spelltmp != null) SpellPriority.Add(spelltmp);
            }
            if (SpellPriority.Count == 0) SpellPriority.Add(shadowBolt);
            lifeTap = SpellFactory.CreateSpell("Life Tap", PlayerStats, character);
            fillerSpell = SpellPriority[SpellPriority.Count - 1];

            Name = "User defined";
            Rotation = "Priority Based:";
            foreach (Spell spell in SpellPriority)
                Rotation += "\r\n- " + spell.Name;

            AfflictionHitChance = (float)Math.Min(1f, HitChance / 100f + character.WarlockTalents.Suppression * 1f / 100f);
            DemonologyHitChance = (float)Math.Min(1f, HitChance / 100f);
            DestructionHitChance = (float)Math.Min(1f, HitChance / 100f + character.WarlockTalents.Cataclysm * 1f / 100f);
        }

        public override void Calculate(CharacterCalculationsWarlock calculatedStats)
        {
            DateTime startTime = DateTime.Now;

            if (SpellPriority.Count == 0) return;
            simStats = PlayerStats.Clone();
            SortedList<double, String> CastList = new SortedList<double, String>();
            events = new EventList();
            LTUsePercent = (int)CalculationOptions.LTUsePercent;

            Spell fillerSpell = SpellPriority[SpellPriority.Count - 1];
            bool HauntIsUp = false;
            bool ImmolateIsUp = false;
            LTOnFiller = CalculationOptions.LTOnFiller;
            int AffEffectsNumber = CalculationOptions.AffEffectsNumber;
            int ShadowEmbrace = 0;
            int ISBProcsLeft = 0;
            int NightfallCounter = 0;
            int EradicationCounter = 0;
            int ISBCounter = 0;
            int MoltenCoreCounter = 0;
            double elapsedTime = 0f;
            double manaGain = 0f;

            currentMana = simStats.Mana;
            ManaSources.Add(new ManaSource("Intellect", currentMana));
            ManaSources.Add(new ManaSource("Life Tap", 0f));
            ManaSources.Add(new ManaSource("MP5", 0f));
            if (CalculationOptions.FSRRatio < 100)
                ManaSources.Add(new ManaSource("OutFSR", 0f));
            if (CalculationOptions.PetSacrificed == true && CalculationOptions.Pet == "Felhunter")
                ManaSources.Add(new ManaSource("Sacrificed Felhunter", 0f));
            if (CalculationOptions.PetSacrificed == true && CalculationOptions.Pet == "Felguard")
                ManaSources.Add(new ManaSource("Sacrificed Felguard", 0f));
            if (CalculationOptions.Replenishment > 0)
                ManaSources.Add(new ManaSource("Replenishment", 0f));
            if (CalculationOptions.JoW > 0)
                ManaSources.Add(new ManaSource("Judgement of Wisdom", 0f));

            Event currentEvent = new DoneCastingEvent();
            while (time < CalculationOptions.FightLength * 60f)
            {
                addMana(elapsedTime, simStats);
                switch (currentEvent.eventName)
                {
                    case "DoneCastingEvent":
                    {
                        Spell spell = GetCastSpellNew(time, currentMana, simStats);
                        if (spell == null)
                        {
                            events.Add(events.Keys[0], currentEvent);
                            break;
                        }
                        if ((spell.ManaCost > currentMana)
                        || (LTUsePercent > 0 && LTUsePercent < 100 && simStats.Mana * LTUsePercent / 100 > currentMana)
                        || (LTUsePercent == 100 && currentMana - lifeTap.ManaCost < simStats.Mana))
                            if (!LTOnFiller || spell == fillerSpell)
                                spell = lifeTap;
                            else if (spell.ManaCost > currentMana)
                            {
                                if (events.Keys.Count == 0)
                                    events.Add(time + 1, currentEvent);
                                else events.Add(events.Keys[0], currentEvent);
                                break;
                            }
                        CastList.Add(time, spell.Name);

                        float hitChance = 1;
                        if (spell.SpellTree == SpellTree.Affliction) hitChance = AfflictionHitChance;
                        else if (spell.SpellTree == SpellTree.Demonology) hitChance = DemonologyHitChance;
                        else if (spell.SpellTree == SpellTree.Destruction) hitChance = DestructionHitChance;

                        if (spell.ManaCost > 0)
                        {
                            spell.SpellStatistics.ManaUsed += spell.ManaCost;
                            currentMana -= spell.ManaCost;
                        }

                        currentMana -= spell.ManaCost;
                        manaGain = Math.Min(3856 * simStats.ManaRestoreFromBaseManaPerHit * (CalculationOptions.JoW / 100f), simStats.Mana - currentMana);
                        foreach (ManaSource manaSource in ManaSources)
                            if (manaSource.Name == "Judgement of Wisdom")
                            {
                                manaSource.Value += manaGain;
                                break;
                            }

                        if (spell.Name == "Shadow Bolt" || spell.Name == "Shadowburn" || spell.Name == "Chaos Bolt" || spell.Name == "Soul Fire" || spell.Name == "Incinerate" || spell.Name == "Searing Pain" || spell.Name == "Conflagrate")
                        {
                            manaGain = Math.Min(simStats.Mana * character.WarlockTalents.ImprovedSoulLeech * 0.01f * 0.3f, simStats.Mana - currentMana);
                            foreach (ManaSource manaSource in ManaSources)
                                if (manaSource.Name == "Imp. Soul Leech")
                                {
                                    manaSource.Value += manaGain;
                                    break;
                                }
                        }
                        if (spell.Name == "Life Tap")
                        {
                            manaGain = Math.Min(0 - lifeTap.ManaCost, simStats.Mana - currentMana);
                            foreach (ManaSource manaSource in ManaSources)
                                if (manaSource.Name == "Life Tap")
                                {
                                    manaSource.Value += manaGain;
                                    currentMana += manaGain;
                                    break;
                                }
                        }

                        if (spell.Cooldown > 0f)
                            spell.SpellStatistics.CooldownReset = time + getCastTime(spell) + spell.Cooldown;
                        else if (spell.DebuffDuration > 0f)
                            spell.SpellStatistics.CooldownReset = time + getCastTime(spell) + spell.DebuffDuration;
                        events.Add((time + Math.Max(getCastTime(spell), spell.GlobalCooldown) + CalculationOptions.Delay / 1000f), new DoneCastingEvent(spell));

                        float damage = spell.AvgDirectDamage * (1f + PlayerStats.BonusDamageMultiplier);
                        switch (spell.Name)
                        {
                            case "Haunt":
                            {
                                HauntIsUp = true;
                                removeEvent("Haunt Debuff");
                                if (!removeEvent("AffEffectHaunt")) AffEffectsNumber++;
                                events.Add(time + 12, new DebuffEndEvent("AffEffectHaunt"));
                                if (character.WarlockTalents.ShadowEmbrace > 0)
                                {
                                    ShadowEmbrace = Math.Min(ShadowEmbrace + 1, 2);
                                     if (!removeEvent("Shadow Embrace Debuff")) AffEffectsNumber++;
                                    events.Add(time + 12, new DebuffEndEvent("Shadow Embrace Debuff"));
                                }
                                double nextCorTick = 0;
                                DotTickEvent nextCorTickEvent = null;
                                for (int index = 0; index < events.Count; index++)
                                    if (events.Values[index].name == "Corruption")
                                    {
                                        nextCorTick = events.Keys[index];
                                        nextCorTickEvent = (DotTickEvent)events.Values[index];
                                        break;
                                    }
                                if (nextCorTick == 0) break;
                                if (character.WarlockTalents.EverlastingAffliction > 0)
                                {
                                    removeEvent("Corruption");
                                    removeEvent("AffEffectCorruption");
                                    double debuff = nextCorTick;
                                    double lastDebuff = debuff;
                                    while (debuff <= time + nextCorTickEvent.dotSpell.DebuffDuration)
                                    {
                                        lastDebuff = debuff;
                                        events.Add(debuff, new DotTickEvent(nextCorTickEvent.dotSpell));
                                        debuff += nextCorTickEvent.dotSpell.TimeBetweenTicks;
                                    }
                                    nextCorTickEvent.dotSpell.SpellStatistics.CooldownReset = lastDebuff;
                                    events.Add(time + nextCorTickEvent.dotSpell.DebuffDuration, new DebuffEndEvent("AffEffectCorruption"));
                                }
                                break;
                            }
                            case "Shadow Bolt":
                            {
                                ISBCounter++;
                                if (character.WarlockTalents.ShadowEmbrace > 0)
                                {
                                    ShadowEmbrace = Math.Min(ShadowEmbrace + 1, 2);
                                    removeEvent("Shadow Embrace Debuff");
                                    events.Add(time + 12, new DebuffEndEvent("Shadow Embrace Debuff"));
                                }
                                break;
                            }
                            case "Drain Life":
                            case "Drain Soul":
                            {
                                if (character.WarlockTalents.SoulSiphon == 1)
                                    damage *= 1 + Math.Max(0.24f,character.WarlockTalents.SoulSiphon * 0.02f * AffEffectsNumber);
                                else if (character.WarlockTalents.SoulSiphon == 2)
                                    damage *= 1 + Math.Max(0.60f,character.WarlockTalents.SoulSiphon * 0.04f * AffEffectsNumber);
                                break;
                            }
                            case "Conflagrate":
                            {
                                double immEnd = 0f;
                                for (int index = 0; index < events.Count; index++)
                                    if (events.Values[index].name == "Immolate")
                                        immEnd = events.Keys[index];
                                if (immEnd - time <= 5)
                                    damage = spell.AvgHit * (1f - (spell.CritChance + character.WarlockTalents.FireAndBrimstone * 0.05f)) + spell.AvgCrit * (spell.CritChance +  character.WarlockTalents.FireAndBrimstone * 0.05f);
                                removeEvent("Immolate");
                                ImmolateIsUp = false;
                                BackdraftCounter = 3;
                                events.Add(time + 15, new DebuffEndEvent("Backdraft"));
                                break;
                            }
                            case "Incinerate":
                            {
                                if(ImmolateIsUp) damage = spell.AvgBuffedDamage * (1f + PlayerStats.BonusDamageMultiplier);
                                break;
                            }
                            case "Immolate":
                            {
                                ImmolateIsUp = true;
                                events.Add(time + 15, new DebuffEndEvent("Immolate"));
                                break;
                            }
                        }
                        if (spell.MagicSchool == MagicSchool.Fire) damage *= (1f + PlayerStats.BonusFireDamageMultiplier);
                        else if (spell.MagicSchool == MagicSchool.Shadow) damage *= (1f + PlayerStats.BonusShadowDamageMultiplier);

                        if (spell.DebuffDuration > 0f)
                        {
                            float debuff = spell.TimeBetweenTicks;
                            while (debuff <= spell.DebuffDuration)
                            {
                                events.Add(time + getCastTime(spell) + debuff, new DotTickEvent(spell));
                                debuff += spell.TimeBetweenTicks;
                            }
                            if (spell.SpellTree == SpellTree.Affliction)
                            {
                                AffEffectsNumber++;
                                if (spell.Name == "Corruption") events.Add(time + spell.DebuffDuration, new DebuffEndEvent("AffEffectCorruption"));
                                else events.Add(time + spell.DebuffDuration, new DebuffEndEvent("AffEffect"));
                            }
                        }
                        if (time >= CalculationOptions.FightLength * 60f * 0.65f && spell.MagicSchool == MagicSchool.Shadow)
                            damage *= 1 + character.WarlockTalents.DeathsEmbrace * 0.04f;
                        if (ISBProcsLeft > 0 && spell.MagicSchool == MagicSchool.Shadow)
                        {
                            ISBProcsLeft--;
                            damage *= 1 + character.WarlockTalents.ImprovedShadowBolt * 0.02f;
                        }
                        if (spell.MagicSchool == MagicSchool.Shadow && spell.Name != "Drain Soul")
                            MoltenCoreCounter++;
                        if (time >= CalculationOptions.FightLength * 60f * 0.75f && spell.Name == "Drain Soul") damage *= 4f;
                        if (spell.Name != "Chaos Bolt" ) damage *= (1f - CalculationOptions.TargetLevel * 0.02f);
                        if (simStats.WarlockGrandFirestone == 1) damage *= 1.1f;

                        spell.SpellStatistics.HitCount++;
                        spell.SpellStatistics.DamageDone += damage;
                        OverallDamage += damage;
                        break;
                    }
                    case "DebuffEndEvent":
                    {
                        switch (((DebuffEndEvent)currentEvent).name)
                        {
                            case "Haunt":
                            {
                                HauntIsUp = false;
                                break;
                            }
                            case "Shadow Embrace":
                            {
                                ShadowEmbrace = 0;
                                AffEffectsNumber--;
                                break;
                            }
                            case "AffEffectHaunt":
                            case "AffEffectCorruption":
                            case "AffEffect":
                            {
                                AffEffectsNumber--;
                                break;
                            }
                            case "Backdraft":
                            {
                                BackdraftCounter = 0;
                                break;
                            }
                            case "Immolate":
                            {
                                ImmolateIsUp = false;
                                break;
                            }
                        }
                        break;
                    }
                    case "DotTickEvent":
                    {
                        DotTickEvent dotEvent = (DotTickEvent)currentEvent;
                        Spell spell = dotEvent.dotSpell;
                        float damage = dotEvent.dotSpell.AvgDotDamage;
                        switch (dotEvent.name)
                        {
                            case "Corruption":
                                {
                                    if (character.WarlockTalents.Pandemic > 0)
                                        damage *= 1 + (character.WarlockTalents.Pandemic == 3 ? 1f : character.WarlockTalents.Pandemic * 0.33f) * simStats.SpellCrit;
                                    NightfallCounter++;
                                    EradicationCounter++;
                                    break;
                                }
                            case "Drain Life":
                                {
                                    NightfallCounter++;
                                    break;
                                }
                            case "Unstable Affliction":
                                {
                                    if (character.WarlockTalents.Pandemic > 0)
                                        damage *= 1 + (character.WarlockTalents.Pandemic == 3 ? 1f : character.WarlockTalents.Pandemic * 0.33f) * spell.CritChance;
                                    break;
                                }
                        }
                        if (ShadowEmbrace > 0)
                            damage *= 1 + ShadowEmbrace * character.WarlockTalents.ShadowEmbrace * 0.01f;
                        if (time >= CalculationOptions.FightLength * 60f * 0.65f && spell.MagicSchool == MagicSchool.Shadow)
                            damage *= 1 + character.WarlockTalents.DeathsEmbrace * 0.04f;
                        if (HauntIsUp)
                            damage *= 1.2f;
                        if (spell.MagicSchool == MagicSchool.Shadow && spell.Name != "Drain Soul")
                            MoltenCoreCounter++;
                        if (simStats.WarlockGrandSpellstone == 1) damage *= 1.1f;

                        spell.SpellStatistics.DamageDone += damage;
                        OverallDamage += damage;
                    }
                    break;
                }
                elapsedTime = events.Keys[0] - time;
                time = events.Keys[0];
                currentEvent = events.Values[0];
                events.RemoveAt(0);
            }

            if (character.WarlockTalents.Nightfall > 0)
            {
                float NightfallProcs = NightfallCounter * character.WarlockTalents.Nightfall * 0.02f;
                foreach (Spell spell in SpellPriority)
                    if (spell.Name == "ShadowBolt")
                    {
                        spell.SpellStatistics.DamageDone +=  NightfallProcs * spell.AvgDirectDamage * (getCastTime(spell) - spell.GlobalCooldown) / getCastTime(spell);
                        OverallDamage += NightfallProcs * spell.AvgDirectDamage * (getCastTime(spell) - spell.GlobalCooldown) / getCastTime(spell);
                        spell.SpellStatistics.ManaUsed +=  NightfallProcs * spell.ManaCost * (getCastTime(spell) - spell.GlobalCooldown) / getCastTime(spell);
                        break;
                    }
            }
            if (character.WarlockTalents.EverlastingAffliction > 0)
            {
                foreach (Spell spell in SpellPriority)
                    if (spell.Name == "Haunt")
                    {
                        float CorDrops = spell.SpellStatistics.HitCount * character.WarlockTalents.EverlastingAffliction * 0.2f;
                        foreach (Spell spellCor in SpellPriority)
                            if (spellCor.Name == "Corruption")
                            {
                                spellCor.SpellStatistics.ManaUsed += CorDrops * spellCor.ManaCost;
                                foreach (Spell spellSB in SpellPriority)
                                    if (spellSB.Name == "Shadow Bolt")
                                    {
                                        spellSB.SpellStatistics.DamageDone -= CorDrops * spellSB.AvgDirectDamage * spellCor.GlobalCooldown / getCastTime(spellSB);
                                        OverallDamage -= CorDrops * spellSB.AvgDirectDamage * spellCor.GlobalCooldown / getCastTime(spellSB);
                                        spellSB.SpellStatistics.ManaUsed -= CorDrops * spellSB.ManaCost * spellCor.GlobalCooldown / getCastTime(spellSB);
                                        break;
                                    }
                                break;
                            }
                        break;
                    }
            }
            if (character.WarlockTalents.MoltenCore > 0)
            {
                float MoltenCoreUptime = Math.Min(1, MoltenCoreCounter * character.WarlockTalents.MoltenCore * 0.05f * 12 / (CalculationOptions.FightLength * 60));
                foreach (Spell spell in SpellPriority)
                    if (spell.MagicSchool == MagicSchool.Fire)
                    {
                        spell.SpellStatistics.DamageDone += spell.SpellStatistics.DamageDone * MoltenCoreUptime * 0.1f;
                        OverallDamage += spell.SpellStatistics.DamageDone * MoltenCoreUptime * 0.1f;
                    }
            }
            if (character.WarlockTalents.Eradication > 0)
            {
                float EradicationUptime;
                //by lack of a proper calculation the uptime is taken from a Wowhead comment
                if (character.WarlockTalents.Eradication == 1)
                    //EradicationUptime = Math.Min(1, Math.Min(CalculationOptions.FightLength * 60 / 30 * 12 / (CalculationOptions.FightLength * 60), EradicationCounter * 0.04f * 12 / (CalculationOptions.FightLength * 60)));
                    EradicationUptime = 0.113664f;
                else if (character.WarlockTalents.Eradication == 2)
                    //EradicationUptime = Math.Min(1, Math.Min(CalculationOptions.FightLength * 60 / 30 * 12 / (CalculationOptions.FightLength * 60), EradicationCounter * 0.07f * 12 / (CalculationOptions.FightLength * 60)));
                    EradicationUptime = 0.164752f;
                //else EradicationUptime = Math.Min(1, Math.Min(CalculationOptions.FightLength * 60 / 30 * 12 / (CalculationOptions.FightLength * 60), EradicationCounter * 0.1f * 12 / (CalculationOptions.FightLength * 60))); 
                else EradicationUptime = 0.200016f;
                foreach (Spell spell in SpellPriority)
                {
                    spell.SpellStatistics.DamageDone *= 1 + EradicationUptime * 0.2f;
                    OverallDamage *= 1 + EradicationUptime * 0.2f;
                }
            }
            if (character.WarlockTalents.ImprovedShadowBolt > 0)
            {
                float ISBCharges = 0;
                float totalShadowDD = 0;
                foreach (Spell spell in SpellPriority)
                {
                    if (spell.Name == "Shadow Bolt")
                        ISBCharges = spell.SpellStatistics.HitCount * spell.CritChance * 4;
                    if (spell.AvgDirectDamage > 0 && spell.MagicSchool == MagicSchool.Shadow)
                        totalShadowDD += spell.SpellStatistics.HitCount;
                }
                foreach (Spell spell in SpellPriority)
                    if (spell.AvgDirectDamage > 0 && spell.MagicSchool == MagicSchool.Shadow)
                    {
                        spell.SpellStatistics.DamageDone += spell.SpellStatistics.HitCount / totalShadowDD * ISBCharges * character.WarlockTalents.ImprovedShadowBolt * 0.02f * spell.AvgDirectDamage;
                        OverallDamage += spell.SpellStatistics.HitCount / totalShadowDD * ISBCharges * character.WarlockTalents.ImprovedShadowBolt * 0.02f * spell.AvgDirectDamage;
                    }
            }
            foreach (Spell spell in SpellPriority)
            {
                float hitChance = 1;
                if (spell.SpellTree == SpellTree.Affliction) hitChance = AfflictionHitChance;
                else if (spell.SpellTree == SpellTree.Demonology) hitChance = DemonologyHitChance;
                else if (spell.SpellTree == SpellTree.Destruction) hitChance = DestructionHitChance;
                OverallDamage -= spell.SpellStatistics.DamageDone - spell.SpellStatistics.DamageDone * hitChance;
                spell.SpellStatistics.DamageDone *= hitChance;
                float misses = spell.SpellStatistics.HitCount * hitChance;
                if (spell.Name == "Haunt")
                {
                    foreach (Spell spellDoT in SpellPriority)
                    {
                        if (spell.AvgDotDamage > 0 && spell.AvgDirectDamage > 0)
                        {
                            OverallDamage -= spell.SpellStatistics.DamageDone * (spell.AvgDotDamage / spell.AvgDamage) * misses * 4 / (CalculationOptions.FightLength * 60);
                            spell.SpellStatistics.DamageDone -= spell.SpellStatistics.DamageDone * (spell.AvgDotDamage / spell.AvgDamage) * misses * 4 / (CalculationOptions.FightLength * 60);
                        }
                        else if (spell.AvgDotDamage > 0)
                        {
                            OverallDamage -= spell.SpellStatistics.DamageDone * misses * 4 / (CalculationOptions.FightLength * 60);
                            spell.SpellStatistics.DamageDone -= spell.SpellStatistics.DamageDone * misses * 4 / (CalculationOptions.FightLength * 60);
                        }
                    }

                }
            }
            DPS = (float)(OverallDamage / time);
            calculatedStats.DpsPoints = DPS;

            DateTime stopTime = DateTime.Now;
            calcTime = stopTime - startTime;
        }

        public bool removeEvent(String name)
        {
            bool atLeastOneRemoved = false;
            for (int index = 0; index < events.Count; index++)
                if (events.Values[index].name == name)
                {
                    events.RemoveAt(index);
                    atLeastOneRemoved = true;
                }
            return atLeastOneRemoved;
        }

        public void addMana (double elapsedTime, Stats simStats)
        {
            double manaGain;
            foreach (ManaSource manaSource in ManaSources)
            switch (manaSource.Name)
            {
                case "OutFSR":
                {
                    manaGain = Math.Min(Math.Floor(character.StatConversion.GetSpiritRegenSec(simStats.Spirit, simStats.Intellect)) * (1f - CalculationOptions.FSRRatio / 100f) * elapsedTime, simStats.Mana - currentMana);
                    manaSource.Value += manaGain;
                    currentMana += manaGain;
                    break;
                }
                case "MP5":
                {
                    manaGain = Math.Min(simStats.Mp5 / 5f * elapsedTime, simStats.Mana - currentMana);
                    manaSource.Value += manaGain;
                    currentMana += manaGain;
                    break;
                }
                case "Sacrificed Felhunter":
                {
                    manaGain = Math.Min(simStats.Mana * 0.03f / 4f * elapsedTime, simStats.Mana - currentMana);
                    manaSource.Value += manaGain;
                    currentMana += manaGain;
                    break;
                }
                case "Sacrificed Felguard":
                {
                    manaGain = Math.Min(simStats.Mana * 0.02f / 4f * elapsedTime, simStats.Mana - currentMana);
                    manaSource.Value += manaGain;
                    currentMana += manaGain;
                    break;
                }
                case "Replenishment":
                {
                    manaGain = Math.Min(simStats.Mana * 0.0025f * (CalculationOptions.Replenishment / 100f) * elapsedTime, simStats.Mana - currentMana);
                    manaSource.Value += manaGain;
                    currentMana += manaGain;
                    break;
                }
            }
/*            if (MPS > regen && character.Race == Character.CharacterRace.BloodElf)
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
            }*/
        }

        public float getCastTime (Spell spell)
        {
            float castTime = Math.Max(spell.CastTime / (1 + (simStats.HasteRating / 32.79f) / 100), Math.Max(1.0f, spell.GlobalCooldown / (1 + (simStats.HasteRating / 32.79f) / 100)));
            if (BackdraftCounter > 0)
            {
                castTime *= 1 - character.WarlockTalents.Backdraft * 0.1f;
                BackdraftCounter--;
            }
            return castTime;
        }
    }
}