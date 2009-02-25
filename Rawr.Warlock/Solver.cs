using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    public class Solver
    {
        public EventList events { get; protected set; }
        double time = 0f;
        Spell lifeTap;
        TimeSpan calcTime;
        public List<Spell> SpellPriority { get; protected set; }
        public float OverallDamage { get; protected set; }

        public Stats PlayerStats { get; set; }
        public Character character { get; set; }
        public float HitChance { get; set; }
        public float DPS { get; protected set; }
        public List<ManaSource> ManaSources { get; set; }
        public CalculationOptionsWarlock CalculationOptions { get; protected set; }

        public string Name { get; protected set; }
        public string Rotation { get; protected set; }

        public bool UseDSBelow35 { get; protected set; }
        public bool UseDSBelow25 { get; protected set; }
        public bool LTOnFiller { get; protected set; }
        public int LTUsePercent { get; protected set; }
        public int BackdraftCounter { get; protected set; }
        public double maxTime { get; protected set; }
        public double currentMana { get; protected set; }
        public double DSCastEnd { get; protected set; }
        public double ImmolateEnd { get; protected set; }
        public Stats simStats { get; protected set; }
        public Spell fillerSpell { get; protected set; }
        public Spell drainSoul { get; protected set; }
        public Spell corruption { get; protected set; }
        public Spell haunt { get; protected set; }
        public Spell immolate { get; protected set; }
        public Spell shadowBolt { get; protected set; }

        public class ManaSource
        {
            public string Name { get; set; }
            public double Value { get; set; }

            public ManaSource(string name, double value)
            {
                Name = name; Value = value;
            }
        }

        public class EventList : SortedList<double, Event>
        {
            public new void Add(double _key, Event _Value)
            {
                double key = _key;
                foreach (double basekey in base.Keys)
                {
                    if (key == basekey) key += 0.00001f;
                    else if (key < basekey) break;
                }
                base.Add(key, _Value);
            }
        }

        public class Event
        {
            public Spell Spell { get; protected set; }
            public String Name { get; protected set; }

            public Event() { }

            public Event (Spell _spell, String _name)
            {
                Spell = _spell;
                Name = _name;
            }
        }

        public Spell GetCastSpellNew(double time, Stats simStats)
        {
            double timeTillNextSpell = maxTime + 100;
            foreach (Spell spell in SpellPriority)
            {
                switch (spell.Name)
                {
                    case "Haunt":
                        {
                            if (spell.SpellStatistics.CooldownReset <= time)
                            {
                                if (time + spell.GlobalCooldown + CalculationOptions.Delay / 1000f + GetCastTime(spell) > spell.SpellStatistics.CooldownReset + 4)
                                {
                                    return spell;
                                }
                                else foreach (Spell tempspell in SpellPriority)
                                {
                                    if ((tempspell.DebuffDuration > 0) && (GetCastTime(tempspell) > 0) && (tempspell.SpellStatistics.CooldownReset < (GetCastTime(tempspell) + time)) && (GetCastTime(tempspell) + CalculationOptions.Delay / 1000f < spell.SpellStatistics.CooldownReset + 4 - time - GetCastTime(spell)))
                                        return tempspell;   // Special case for dots that have cast time
                                    else if ((tempspell.SpellStatistics.CooldownReset) <= time && (GetCastTime(tempspell) + CalculationOptions.Delay / 1000f < spell.SpellStatistics.CooldownReset + 4 - time - GetCastTime(spell)))
                                        return tempspell;
                                }
                                return spell;
                            }
                            timeTillNextSpell = Math.Min(timeTillNextSpell, spell.SpellStatistics.CooldownReset - GetCastTime(spell));
                            break;
                        }
                    case "Conflagrate":
                        {
                            for (int index = 0; index < events.Count; index++)
                                if (events.Values[index].Name == "Immolate")
                                    return spell;
                            timeTillNextSpell = Math.Min(timeTillNextSpell, spell.SpellStatistics.CooldownReset - GetCastTime(spell));
                            break;
                        }
                    default:
                        {
                            if (spell.Name == fillerSpell.Name)
                            {
                                if (UseDSBelow25 || UseDSBelow35)
                                {
//                                    if (time < DSCastEnd && time <= timeTillNextSpell - GetCastTime(drainSoul))
                                    if (time < DSCastEnd)
                                        return null;
                                    if (((time >= maxTime * 0.65f && UseDSBelow35)
                                      || (time >= maxTime * 0.75f && UseDSBelow25))
//                                      && (time <= timeTillNextSpell - drainSoul.GlobalCooldown))
                                       )
                                        return drainSoul;
                                }
                            }
                            if ((spell.DebuffDuration > 0) && (GetCastTime(spell) > 0) && (spell.SpellStatistics.CooldownReset < (time + GetCastTime(spell)))
//                             && (time <= timeTillNextSpell - GetCastTime(spell)))
                               )
                                return spell;   // Special case for dots that have cast time
                            else if (time >= spell.SpellStatistics.CooldownReset
//                                 && (time <= timeTillNextSpell - GetCastTime(spell)))
                                    )
                                return spell;
                            timeTillNextSpell = Math.Min(timeTillNextSpell, spell.SpellStatistics.CooldownReset - GetCastTime(spell));
                            break;
                        }
                }
            }
            return null;
        }

        public Solver(Stats playerStats, Character _char)
        {
            character = _char;
            Name = "Base";
            Rotation = "None";

            PlayerStats = playerStats.Clone();
            if (playerStats.SpellPowerFor15SecOnUse90Sec > 0.0f)
                PlayerStats.SpellPower += playerStats.SpellPowerFor15SecOnUse90Sec * 15f / 90f;
            if (playerStats.SpellPowerFor15SecOnUse2Min > 0.0f)
                PlayerStats.SpellPower += playerStats.SpellPowerFor15SecOnUse2Min * 15f / 120f;
            if (playerStats.SpellPowerFor20SecOnUse2Min > 0.0f)
                PlayerStats.SpellPower += playerStats.SpellPowerFor20SecOnUse2Min * 20f / 120f;
            if (playerStats.HasteRatingFor20SecOnUse2Min > 0.0f)
                PlayerStats.HasteRating += playerStats.HasteRatingFor20SecOnUse2Min * 20f / 120f;
            if (playerStats.HasteRatingFor20SecOnUse5Min > 0.0f)
                PlayerStats.HasteRating += playerStats.HasteRatingFor20SecOnUse5Min * 20f / 300f;
            if (playerStats.SpellHasteFor10SecOnCast_10_45 > 0.0f)
                PlayerStats.HasteRating += playerStats.SpellHasteFor10SecOnCast_10_45 * 10f / 75f;

            CalculationOptions = character.CalculationOptions as CalculationOptionsWarlock;

            HitChance = PlayerStats.SpellHit * 100f + CalculationOptions.TargetHit;
            if (character.Race == Character.CharacterRace.Draenei && !character.ActiveBuffsContains("Heroic Presence"))
                HitChance += 1;

            ManaSources = new List<ManaSource>();
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            shadowBolt = SpellFactory.CreateSpell("Shadow Bolt", PlayerStats, character);
            lifeTap = SpellFactory.CreateSpell("Life Tap", PlayerStats, character);
            foreach (string spellname in CalculationOptions.SpellPriority)
            {
                Spell spelltmp = SpellFactory.CreateSpell(spellname, PlayerStats, character);
                if (spelltmp != null)
                {
                    SpellPriority.Add(spelltmp);
                    if (spelltmp.SpellTree == SpellTree.Affliction) spelltmp.SpellStatistics.HitChance = (float)Math.Min(1f, HitChance / 100f + character.WarlockTalents.Suppression * 1f / 100f);
                    else if (spelltmp.SpellTree == SpellTree.Destruction) spelltmp.SpellStatistics.HitChance = (float)Math.Min(1f, HitChance / 100f + character.WarlockTalents.Cataclysm * 1f / 100f);
                    else spelltmp.SpellStatistics.HitChance = (float)Math.Min(1f, HitChance / 100f);
                }
            }
            if (SpellPriority.Count == 0) SpellPriority.Add(shadowBolt);
            fillerSpell = SpellPriority[SpellPriority.Count - 1];
//            bool UseBSBelow35 = CalculationOptions.UseBSBelow35;
//            bool UseBSBelow25 = CalculationOptions.UseBSBelow25;
            UseDSBelow35 = false;
            UseDSBelow25 = false;
            if (UseDSBelow35 || UseDSBelow25)
            {
                drainSoul = SpellFactory.CreateSpell("Drain Soul", PlayerStats, character);
                SpellPriority.Add(drainSoul);
            }

            corruption = GetSpellByName("Corruption");
            haunt = GetSpellByName("Haunt");
            immolate = GetSpellByName("Immolate");
            shadowBolt = GetSpellByName("Shadow Bolt");

            Name = "User defined";
            Rotation = "Priority Based:";
            foreach (Spell spell in SpellPriority)
                Rotation += "\r\n- " + spell.Name;
        }

        public void Calculate(CharacterCalculationsWarlock calculatedStats)
        {
            DateTime startTime = DateTime.Now;

            if (SpellPriority.Count == 0) return;
            simStats = PlayerStats.Clone();
            SortedList<double, Spell> CastList = new SortedList<double, Spell>();
            events = new EventList();
            LTUsePercent = (int)CalculationOptions.LTUsePercent;
//            maxTime = CalculationOptions.FightLength * 60f;
            maxTime = 3600f;

            bool ImmolateIsUp = false;
            bool ShadowflameIsUp = false;
            bool CleanBreak = false;
            bool cleanBreak = CalculationOptions.cleanBreak;
            int AffEffectsNumber = CalculationOptions.AffEffectsNumber;
            int ShadowEmbrace = 0;
            int NightfallCounter = 0;
            int EradicationCounter = 0;
            int ISBCounter = 0;
            int MoltenCoreCounter = 0;
            int CounterBuffedIncinerate = 0;
            int CounterBuffedConflag = 0;
            int CounterShadowEmbrace = 0;
            int CounterDotTicks = 0;
            int CounterAffEffects = 0;
            int CounterDrainTicks = 0;
            int sequence = SpellPriority.Count - 1;
            float Procs2T7 = 0;
            double elapsedTime = 0;
            Spell lastSpell = null;

            #region Calculate cast rotation
            currentMana = 0;
            time += GetCastTime(SpellPriority[0]);
            Event currentEvent = new Event(SpellPriority[0], "Done casting");
            CastList.Add(0, SpellPriority[0]);
            while ((time < maxTime || currentEvent.Name != "Done casting") && !CleanBreak)
            {
//                currentMana += 1000;
                switch (currentEvent.Name)
                {
                    case "Done casting":
                    {
                        Spell spell = currentEvent.Spell;

                        if (spell == SpellPriority[sequence])
                            sequence++;
                        else
                            sequence = 0;
                        if (sequence > SpellPriority.Count - 1) sequence--;
                        if (SpellPriority[sequence] == fillerSpell && cleanBreak == true)
                        {   // Spell sequence just reset, lets take advantage of that.
                            int i = SpellPriority.IndexOf(fillerSpell);
                            while (i > 0)
                            {
                                CastList.RemoveAt(CastList.Count - i);
                                i--;
                            }
                            Rotation = "Clean break after " + time + "seconds";
                            CleanBreak = true;
                            break;
                        }

                        if (spell.Cooldown > 0f)
                            spell.SpellStatistics.CooldownReset = time + GetCastTime(spell) + spell.Cooldown;
                        else if (spell.DebuffDuration > 0f)
                        {
                            spell.SpellStatistics.CooldownReset = time + GetCastTime(spell) + spell.DebuffDuration;
                            if (spell.Name == "Curse of Agony" && CalculationOptions.GlyphCoA)
                                spell.SpellStatistics.CooldownReset += 4;
                        }

                        switch (spell.Name)
                        {
                            case "Haunt":
                            {
                                if (!removeEvent(spell, "Aff effect")) AffEffectsNumber++;
                                events.Add(time + 12, new Event(spell, "Aff effect"));
                                if (character.WarlockTalents.ShadowEmbrace > 0)
                                {
                                    ShadowEmbrace = Math.Min(ShadowEmbrace + 1, 2);
                                     if (!removeEvent("Shadow Embrace debuff")) AffEffectsNumber++;
                                    events.Add(time + 12, new Event(spell, "Shadow Embrace debuff"));
                                }
                                double nextCorTick = 0;
                                Event nextCorTickEvent = null;
                                for (int index = 0; index < events.Count; index++)
                                    if (events.Values[index].Spell.Name == "Corruption")
                                    {
                                        nextCorTick = events.Keys[index];
                                        nextCorTickEvent = events.Values[index];
                                        break;
                                    }
                                if (character.WarlockTalents.EverlastingAffliction > 0 && nextCorTick > 0)
                                {
                                    removeEvent(nextCorTickEvent.Spell, "Dot tick");
                                    removeEvent(nextCorTickEvent.Spell, "Aff effect");
                                    double debuff = nextCorTick;
                                    double lastDebuff = debuff;
                                    while (debuff <= time + nextCorTickEvent.Spell.DebuffDuration)
                                    {
                                        lastDebuff = debuff;
                                        events.Add(debuff, new Event(nextCorTickEvent.Spell, "Dot tick"));
                                        debuff += nextCorTickEvent.Spell.TimeBetweenTicks;
                                    }
                                    nextCorTickEvent.Spell.SpellStatistics.CooldownReset = lastDebuff;
                                    events.Add(time + nextCorTickEvent.Spell.DebuffDuration, new Event(nextCorTickEvent.Spell, "Aff effect"));
                                }
                                break;
                            }
                            case "Shadow Bolt":
                            {
                                ISBCounter++;
                                if (character.WarlockTalents.ShadowEmbrace > 0)
                                {
                                    ShadowEmbrace = Math.Min(ShadowEmbrace + 1, 2);
                                    removeEvent("Shadow Embrace debuff");
                                    events.Add(time + 12, new Event(spell, "Shadow Embrace debuff"));
                                }
                                break;
                            }
                            case "Drain Soul":
                            {
                                DSCastEnd = time + GetCastTime(spell);
                                break;
                            }
                            case "Conflagrate":
                            {
                                if (ImmolateEnd - time <= 5) CounterBuffedConflag++;
                                if (!CalculationOptions.GlyphConflag)
                                    if (ShadowflameIsUp)
                                    {
                                        removeEvent("Shadowflame");
                                        ShadowflameIsUp = false;
                                    }
                                    else
                                    {
                                        removeEvent("Immolate");
                                        ImmolateIsUp = false;
                                        immolate.SpellStatistics.CooldownReset = time;
                                    }
                                if (character.WarlockTalents.Backdraft > 0)
                                {
                                    BackdraftCounter = 3;
                                    events.Add(time + 15, new Event(spell, "Backdraft"));
                                }
                                break;
                            }
                            case "Incinerate":
                            {
                                if(ImmolateIsUp) CounterBuffedIncinerate++;
                                break;
                            }
                            case "Immolate":
                            {
                                ImmolateIsUp = true;
                                ImmolateEnd = time + 15;
                                events.Add(time + 15, new Event(spell, "Immolate"));
                                break;
                            }
                            case "Shadowflame":
                                {
                                    ShadowflameIsUp = true;
                                    events.Add(time + 8, new Event(spell, "Shadowflame"));
                                    break;
                                }
                        }
//                        if (time >= maxTime * 0.65f && spell.MagicSchool == MagicSchool.Shadow)
//                            damage = (float)((1 - Math.Min((time - maxTime * 0.65f) / GetCastTime(spell),1)) * damage + Math.Min((time - maxTime * 0.65f) / GetCastTime(spell),1) * damage * (1 + character.WarlockTalents.DeathsEmbrace * 0.04f));
                        if (spell.MagicSchool == MagicSchool.Shadow && spell.Name != "Drain Soul") MoltenCoreCounter++;
//                        if (time >= maxTime * 0.75f && spell.Name == "Drain Soul") damage *= 4f;

                        spell.SpellStatistics.HitCount++;

                        spell = lastSpell = GetCastSpellNew(time, simStats);
                        if (spell == null)
                        {
                            events.Add(events.Keys[0], currentEvent);
                            break;
                        }
                        else if (time < DSCastEnd)
                        {
                            removeEvent(drainSoul);
                            DSCastEnd = 0;
                        }
                        CastList.Add(time, spell);

                        if (spell.Name != "Drain Soul")
                            events.Add((time + Math.Max(GetCastTime(spell), spell.GlobalCooldown) + CalculationOptions.Delay / 1000f), new Event(spell, "Done casting"));
                        if (spell.DebuffDuration > 0f)
                        {
                            float debuff = GetCastTime(spell.TimeBetweenTicks);
                            while (debuff <= spell.DebuffDuration + (spell.Name == "Curse of Agony" && CalculationOptions.GlyphCoA ? 4 : 0))
                            {
                                if (spell.Name == "Drain Soul")
                                {
                                    events.Add(time + debuff, new Event(spell, "Dot tick"));
                                    events.Add(time + debuff, new Event(spell, "Done casting"));
                                }
                                else events.Add(time + (spell.CastTime > 0 ? GetCastTime(spell) : 0) + debuff, new Event(spell, "Dot tick"));
                                debuff += GetCastTime(spell.TimeBetweenTicks);
                            }
                            if (spell.SpellTree == SpellTree.Affliction && spell.Name != "Drain Soul")
                            {
                                AffEffectsNumber++;
                                events.Add(time + spell.DebuffDuration, new Event(spell, "Aff effect"));
                            }
                        }
                        break;
                    }
                    case "Dot tick":
                    {
                        CounterShadowEmbrace += ShadowEmbrace;
                        CounterDotTicks++;
                        Spell spell = currentEvent.Spell;
                        spell.SpellStatistics.TickCount++;
                        switch (currentEvent.Spell.Name)
                        {
                            case "Corruption":
                                {
                                    NightfallCounter++;
                                    EradicationCounter++;
                                    Procs2T7 += 0.15f;
                                    break;
                                }
                            case "Drain Life":
                                {
                                    NightfallCounter++;
                                    CounterAffEffects += AffEffectsNumber;
                                    CounterDrainTicks++;
                                    break;
                                }
                            case "Drain Soul":
                                {
                                    CounterAffEffects += AffEffectsNumber;
                                    CounterDrainTicks++;
                                    break;
                                }
                            case "Immolate":
                                {
                                    Procs2T7 += 0.15f;
                                    break;
                                }
                            case "Shadowflame":
                                {
                                    ShadowflameIsUp = false;
                                    break;
                                }
                        }
//                        if (time >= maxTime * 0.65f && spell.MagicSchool == MagicSchool.Shadow)
//                            damage *= 1 + character.WarlockTalents.DeathsEmbrace * 0.04f;
                        if (spell.MagicSchool == MagicSchool.Shadow && spell.Name != "Drain Soul")
                            MoltenCoreCounter++;
                        break;
                    }
                    default:
                    {
                        switch (currentEvent.Name)
                        {
                            case "Shadow Embrace debuff":
                                {
                                    ShadowEmbrace = 0;
                                    AffEffectsNumber--;
                                    break;
                                }
                            case "Aff effect":
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
                }
                if (!CleanBreak)
                {
                    elapsedTime = events.Keys[0] - time;
                    time = events.Keys[0];
                    currentEvent = events.Values[0];
                    events.RemoveAt(0);
                }
            }
            #endregion

            #region Variable calculation
            float corDrops = 0;
            if (character.WarlockTalents.EverlastingAffliction > 0 && haunt != null)
            {
                corDrops = haunt.SpellStatistics.HitCount * (1 - haunt.SpellStatistics.HitChance) + haunt.SpellStatistics.HitCount * haunt.SpellStatistics.HitChance * (Math.Min(1 - character.WarlockTalents.EverlastingAffliction * 0.2f, 0));
                if (corruption != null)
                {
                    corruption.SpellStatistics.ManaUsed += corDrops * corruption.ManaCost;
                    currentMana -= corDrops * corruption.ManaCost;
                }
            }
            float NightfallProcs = 0;
            if (character.WarlockTalents.Nightfall > 0)
            {
                NightfallProcs = NightfallCounter * (character.WarlockTalents.Nightfall * 0.02f + (CalculationOptions.GlyphCorruption ? 0.04f : 0));
                if (shadowBolt != null)
                    shadowBolt.SpellStatistics.ManaUsed += NightfallProcs * shadowBolt.ManaCost * (GetCastTime(shadowBolt) - shadowBolt.GlobalCooldown) / GetCastTime(shadowBolt);
            }
            float directShadowHits = 0;
            float ISBCharges = 0;
            if (character.WarlockTalents.ImprovedShadowBolt > 0)
            {
                ISBCharges = shadowBolt.SpellStatistics.HitCount * shadowBolt.CritChance * 4;
                foreach (Spell spell in SpellPriority)
                    if (spell.AvgDirectDamage > 0 && spell.MagicSchool == MagicSchool.Shadow)
                        directShadowHits += spell.SpellStatistics.HitCount;
            }
            float hauntMisses = 0;
            if (haunt != null)
                hauntMisses = haunt.SpellStatistics.HitCount * (1 - haunt.SpellStatistics.HitChance);
            #endregion

            #region Mana Recovery
            foreach (Spell spell in SpellPriority)
            {
                float manaCost = spell.ManaCost * (spell.Name == "Shadow Bolt" && CalculationOptions.GlyphSB ? 0.9f : 1);
                spell.SpellStatistics.ManaUsed += manaCost * spell.SpellStatistics.HitCount;
                currentMana -= manaCost * spell.SpellStatistics.HitCount;
            }
            if (corruption != null)
            {
                fillerSpell.SpellStatistics.ManaUsed -= corDrops * fillerSpell.ManaCost * GetCastTime(corruption) / GetCastTime(fillerSpell);
                currentMana += corDrops * fillerSpell.ManaCost * GetCastTime(corruption) / GetCastTime(fillerSpell);
            }

            double manaGain = simStats.Mana;
            currentMana += manaGain;
            ManaSources.Add(new ManaSource("Intellect", manaGain));
            manaGain = simStats.Mp5 / 5f * time;
            currentMana += manaGain;
            ManaSources.Add(new ManaSource("MP5", manaGain));
            if (CalculationOptions.FSRRatio < 100)
            {
                manaGain = Math.Floor(character.StatConversion.GetSpiritRegenSec(simStats.Spirit, simStats.Intellect)) * (1f - CalculationOptions.FSRRatio / 100f) * time;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("OutFSR", manaGain));
            }
            if (CalculationOptions.PetSacrificed == true && CalculationOptions.Pet == "Felhunter")
            {
                manaGain = simStats.Mana * 0.03f / 4f * time;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("Sacrificed Felhunter", manaGain));
            }
            if (CalculationOptions.PetSacrificed == true && CalculationOptions.Pet == "Felguard")
            {
                manaGain = simStats.Mana * 0.02f / 4f * time;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("Sacrificed Felguard", manaGain));
            }
            if (CalculationOptions.Replenishment > 0)
            {
                manaGain = simStats.Mana * 0.0025f * (CalculationOptions.Replenishment / 100f) * time;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("Replenishment", manaGain));
            }
            if (simStats.ManaRestoreFromBaseManaPerHit > 0)
            {
                float hitCount = 0;
                foreach (Spell spell in SpellPriority)
                    hitCount += spell.SpellStatistics.HitCount;
                manaGain = 3856 * simStats.ManaRestoreFromBaseManaPerHit * (CalculationOptions.JoW / 100f) * hitCount;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("Judgement of Wisdom", manaGain));
            }
            if (character.WarlockTalents.ImprovedSoulLeech > 0)
            {
                manaGain = 0;
                foreach (Spell spell in SpellPriority)
                    if (spell.Name == "Shadow Bolt" || spell.Name == "Shadowburn" || spell.Name == "Chaos Bolt" || spell.Name == "Soul Fire" || spell.Name == "Incinerate" || spell.Name == "Searing Pain" || spell.Name == "Conflagrate")
                        manaGain += spell.SpellStatistics.HitCount * simStats.Mana * character.WarlockTalents.ImprovedSoulLeech * 0.01f * 0.3f;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("Improved Soul Leech", manaGain));
            }
            manaGain = 0;
            while (currentMana < 0)
            {
                currentMana -= lifeTap.ManaCost;
                manaGain -= lifeTap.ManaCost;
                fillerSpell.SpellStatistics.HitCount -= GetCastTime(lifeTap) / GetCastTime(fillerSpell);
                fillerSpell.SpellStatistics.ManaUsed -= fillerSpell.SpellStatistics.ManaUsed / fillerSpell.SpellStatistics.HitCount * (GetCastTime(lifeTap)) / GetCastTime(fillerSpell);
                if (simStats.LifeTapBonusSpirit > 0)
                    simStats.SpellPower += (float)(300 * 0.3 * 10 / time);
            }
            ManaSources.Add(new ManaSource("Life Tap", manaGain));

/*            if (MPS > regen && character.Race == Character.CharacterRace.BloodElf)
            {   // Arcane Torrent is 6% max mana every 2 minutes.
                tmpregen = simStats.Mana * 0.06f / 120f;
                ManaSources.Add(new ManaSource("Arcane Torrent", tmpregen));
                regen += tmpregen;
                Rotation += "\r\n- Used Arcane Torrent";
            }

            if (MPS > regen && CalculationOptions.ManaAmt > 0)
            {   // Not enough mana, use Mana Potion
                tmpregen = CalculationOptions.ManaAmt / maxTime * (1f + simStats.BonusManaPotion);
                ManaSources.Add(new ManaSource("Mana Potion", tmpregen));
                regen += tmpregen;
                Rotation += "\r\n- Used Mana Potion";
            }*/
            #endregion

            #region Damage trinkets
            double CastsPerSecond = 0;
            double HitsPerSecond = 0;
            foreach (KeyValuePair<double, Spell> cast in CastList)
            {
                Spell spell = cast.Value;
                CastsPerSecond++;
                HitsPerSecond += spell.SpellStatistics.HitChance;
            }
            CastsPerSecond /= time;
            HitsPerSecond /= time;

            if (simStats.SpellPowerFor10SecOnHit_10_45 > 0)
            {
                float ProcChance = 0.1f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance);
                double EffCooldown = 45f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / HitsPerSecond / ProcActual;
                simStats.SpellPower += (float)(simStats.SpellPowerFor10SecOnHit_10_45 * 10f / EffCooldown);
            }
            if (simStats.SpellPowerFor10SecOnCast_15_45 > 0)
            {
                float ProcChance = 0.15f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance);
                double EffCooldown = 45f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / CastsPerSecond / ProcActual;
                simStats.SpellPower += (float)(simStats.SpellPowerFor10SecOnCast_15_45 * 10f / EffCooldown);
            }
            #endregion

            #region Spell updates
            foreach (Spell spell in SpellPriority)
            {
                spell.Calculate(simStats, character);
            }
            #endregion

            #region Damage calculations
            foreach (Spell spell in SpellPriority)
            {
                if (spell == fillerSpell && corruption != null)
                    spell.SpellStatistics.HitCount -= corDrops * GetCastTime(corruption) / GetCastTime(spell);
                float directDamage = spell.AvgDirectDamage * spell.SpellStatistics.HitCount;
                float dotDamage = spell.AvgDotDamage * spell.SpellStatistics.TickCount;
                if (haunt != null)
                    dotDamage *= 1.2f;

                switch (spell.Name)
                {
                    case "Shadow Bolt":
                        {
                            if (simStats.CorruptionTriggersCrit > 0)
                                directDamage = spell.AvgHit * (1f - (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f)) * spell.SpellStatistics.HitCount + spell.AvgCrit * (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f) * spell.SpellStatistics.HitCount;
                            directDamage += NightfallProcs * spell.AvgDirectDamage * (GetCastTime(spell) - spell.GlobalCooldown) / GetCastTime(spell);
                            if (character.WarlockTalents.ImprovedShadowBolt > 0)
                                directDamage += spell.SpellStatistics.HitCount / directShadowHits * ISBCharges * character.WarlockTalents.ImprovedShadowBolt * 0.02f * spell.AvgDirectDamage;
                            break;
                        }
                    case "Unstable Affliction":
                        {
                            if (character.WarlockTalents.Pandemic > 0)
                                dotDamage *= 1 + (character.WarlockTalents.Pandemic == 3 ? 1f : character.WarlockTalents.Pandemic * 0.33f) * simStats.SpellCrit;
                            dotDamage -= (float)(spell.SpellStatistics.DamageDone * hauntMisses * 4 / maxTime);
                            break;
                        }
                    case "Corruption":
                        {
                            if (character.WarlockTalents.Pandemic > 0)
                                dotDamage *= 1 + (character.WarlockTalents.Pandemic == 3 ? 1f : character.WarlockTalents.Pandemic * 0.33f) * simStats.SpellCrit;
                            dotDamage -= (float)(spell.SpellStatistics.DamageDone * hauntMisses * 4 / maxTime);
                            break;
                        }
                    case "Conflagrate":
                        {
                            if (character.WarlockTalents.FireAndBrimstone > 0)
                                directDamage = (spell.SpellStatistics.HitCount - CounterBuffedConflag) * spell.AvgDirectDamage
                                             + CounterBuffedConflag * (spell.AvgHit * (1f - spell.CritChance - character.WarlockTalents.FireAndBrimstone * 0.05f) + spell.AvgCrit * (spell.CritChance + character.WarlockTalents.FireAndBrimstone * 0.05f));
                            break;
                        }
                    case "Incinerate":
                        {
                            if (simStats.CorruptionTriggersCrit > 0)
                                directDamage = spell.AvgHit * (1f - (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f)) + spell.AvgCrit * (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f);
                            directDamage += CounterBuffedIncinerate * (spell.AvgBuffedDamage - spell.AvgDirectDamage);
                            break;
                        }
                    case "Immolate":
                        {
                            if (CalculationOptions.GlyphImmolate)
                            {
                                directDamage *= 0.9f;
                                dotDamage *= 1.2f;
                            }
                            dotDamage -= (float)(dotDamage * hauntMisses * 4 / maxTime);
                            break;
                        }
                    case "Drain Life":
                    case "Drain Soul":
                        {
                            dotDamage -= (float)(dotDamage * hauntMisses * 4 / maxTime);
                            if (character.WarlockTalents.SoulSiphon == 1)
                                dotDamage *= 1 + Math.Max(0.24f,character.WarlockTalents.SoulSiphon * 0.02f * CounterAffEffects / CounterDrainTicks);
                            else if (character.WarlockTalents.SoulSiphon == 2)
                                dotDamage *= 1 + Math.Max(0.60f,character.WarlockTalents.SoulSiphon * 0.04f * CounterAffEffects / CounterDrainTicks);
                            break;
                        }
                    case "Searing Pain":
                        {
                            if (CalculationOptions.GlyphSearingPain)
                                directDamage = spell.AvgHit * (1f - spell.CritChance - 0.2f) + spell.AvgCrit * (spell.CritChance + 0.2f);
                            break;
                        }
                    case "Siphon Life":
                        {
                            if (CalculationOptions.GlyphSiphonLife)
                                dotDamage *= 1.2f;
                            dotDamage -= (float)(dotDamage * hauntMisses * 4 / maxTime);
                            break;
                        }
                    case "Curse of Agony":
                    case "Curse of Doom":
                    case "Seed of Corruption":
                    case "Rain of Fire":
                    case "Hellfire":
                        {
                            dotDamage -= (float)(dotDamage * hauntMisses * 4 / maxTime);
                            break;
                        }
                    case "Haunt":
                    case "Death Coil":
                    case "Shadowflame":
                    case "Shadowburn":
                    case "Shadowfury":
                    case "Soul Fire":
                    case "Chaos Bolt":
                        {
                            break;
                        }
                }
                switch (spell.MagicSchool)
                {
                    case (MagicSchool.Fire):
                        {
                            directDamage *= 1 + PlayerStats.BonusFireDamageMultiplier;
                            dotDamage *= 1 + PlayerStats.BonusFireDamageMultiplier;
                            if (character.WarlockTalents.MoltenCore > 0)
                            {
                                float MoltenCoreUptime = (float)Math.Min(1, MoltenCoreCounter * character.WarlockTalents.MoltenCore * 0.05f * 12 / maxTime);
                                directDamage *= 1 + MoltenCoreUptime * 0.1f;
                                dotDamage *= 1 + MoltenCoreUptime * 0.1f;
                            }
                            break;
                        }
                    case (MagicSchool.Shadow):
                        {
                            directDamage *= 1 + PlayerStats.BonusShadowDamageMultiplier;
                            dotDamage *= 1 + PlayerStats.BonusShadowDamageMultiplier;
                            break;
                        }
                }
                if (character.WarlockTalents.Eradication > 0)
                {
                    float EradicationUptime;
                    //by lack of a proper calculation the uptime is taken from a Wowhead comment
                    if (character.WarlockTalents.Eradication == 1)
                        //EradicationUptime = Math.Min(1, Math.Min(maxTime / 30 * 12 / maxTime, EradicationCounter * 0.04f * 12 / maxTime));
                        EradicationUptime = 0.113664f;
                    else if (character.WarlockTalents.Eradication == 2)
                        //EradicationUptime = Math.Min(1, Math.Min(maxTime / 30 * 12 / maxTime, EradicationCounter * 0.07f * 12 / maxTime));
                        EradicationUptime = 0.164752f;
                    //else EradicationUptime = Math.Min(1, Math.Min(maxTime / 30 * 12 / maxTime, EradicationCounter * 0.1f * 12 / maxTime));
                    else EradicationUptime = 0.200016f;
                    directDamage *= 1 + EradicationUptime * 0.2f;
                }
                if (CounterShadowEmbrace > 0)
                    dotDamage *= 1 + (float)CounterShadowEmbrace / (float)CounterDotTicks * character.WarlockTalents.ShadowEmbrace * 0.01f;
                directDamage *= 1 + simStats.WarlockGrandFirestone * 0.01f;
                dotDamage *= 1 + simStats.WarlockGrandSpellstone * 0.01f;

                spell.SpellStatistics.DamageDone = ((directDamage > 0 ? directDamage : 0) + (dotDamage > 0 ? dotDamage : 0)) * spell.SpellStatistics.HitChance;
                if (spell.Name != "Chaos Bolt")
                    spell.SpellStatistics.DamageDone *= (1f - CalculationOptions.TargetLevel * 0.02f);

                OverallDamage += spell.SpellStatistics.DamageDone;
            }
            #endregion
            
            DPS = (float)(OverallDamage / time);

            #region Finalize procs
            if (simStats.LightweaveEmbroideryProc > 0.0f)
            {   // 50% proc chance, 45s internal cd, shoots a Holy bolt
                float ProcChance = 0.5f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance); // This is the real procchance after the Cumulative chance.
                float EffCooldown = 45f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / (float)HitsPerSecond / ProcActual;
                Spell Lightweave = new LightweaveProc(simStats, character);
                DPS += Lightweave.AvgDamage / EffCooldown * (1f + simStats.BonusDamageMultiplier) * HitChance / 100f;
            }
            /*if (simStats.WarlockGrandFirestone > 0.0f)
            {   // 25% proc chance, 0s internal cd, adds Fire damage
                float ProcChance = 0.25f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance); // This is the real procchance after the Cumulative chance.
                float EffCooldown = 0f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / (float)HitsPerSecond / ProcActual;
                Spell Firestone = new FirestoneProc(simStats, character);
                DPS += Firestone.AvgDamage / EffCooldown * (1f + simStats.BonusFireDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * HitChance / 100f;
            }*/
            if (simStats.ExtractOfNecromanticPowerProc > 0.0f)
            {   // 10% proc chance, 15s internal cd, shoots a Shadow Bolt
                // Although, All dots tick about every 3s, so in avg cooldown gains another 1.5s, putting it at 16.5
                int dots = 0;
                foreach (Spell spell in SpellPriority)
                    if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                float ProcChance = 0.1f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance);
                float EffCooldown = 16.5f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / (dots / 3) / ProcActual;
                Spell Extract = new ExtractProc(simStats, character);
                DPS += (Extract.AvgDamage / EffCooldown) * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * HitChance / 100f;
            }
            #endregion

            calculatedStats.DpsPoints = DPS;

            DateTime stopTime = DateTime.Now;
            calcTime = stopTime - startTime;
        }

        public bool removeEvent(Spell spell)
        {
            bool atLeastOneRemoved = false;
            for (int index = 0; index < events.Count; index++)
                if (events.Values[index].Spell == spell)
                {
                    events.RemoveAt(index);
                    atLeastOneRemoved = true;
                }
            return atLeastOneRemoved;
        }

        public bool removeEvent(String name)
        {
            bool atLeastOneRemoved = false;
            for (int index = 0; index < events.Count; index++)
                if (events.Values[index].Name == name)
                {
                    events.RemoveAt(index);
                    atLeastOneRemoved = true;
                }
            return atLeastOneRemoved;
        }

        public bool removeEvent(Spell spell, String name)
        {
            bool atLeastOneRemoved = false;
            for (int index = 0; index < events.Count; index++)
                if (events.Values[index].Spell == spell && events.Values[index].Name == name)
                {
                    events.RemoveAt(index);
                    atLeastOneRemoved = true;
                }
            return atLeastOneRemoved;
        }

        public float GetCastTime (Spell spell)
        {
            float castTime = Math.Max(spell.CastTime / (1 + (simStats.HasteRating / 32.79f) / 100), Math.Max(1.0f, spell.GlobalCooldown / (1 + (simStats.HasteRating / 32.79f) / 100)));
            if (BackdraftCounter > 0 && spell.SpellTree == SpellTree.Destruction)
            {
                castTime *= 1 - character.WarlockTalents.Backdraft * 0.1f;
                BackdraftCounter--;
            }
            if (spell.Name == "Unstable Affliction" && CalculationOptions.GlyphUA)
                castTime -= 0.2f;
            return castTime;
        }

        public float GetCastTime (float TimeBetweenTicks)
        {
            return TimeBetweenTicks / (1 + (simStats.HasteRating / 32.79f) / 100);
        }

        public Spell GetSpellByName(string name)
        {
            foreach (Spell spell in SpellPriority)
                if (spell.Name.Contains(name))
                    return spell;
            return null;
        }
    }
}