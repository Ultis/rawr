using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    public class Solver
    {
        public EventList events { get; protected set; }
        public double time = 0f;
        public double petManaGain = 0;
        Spell lifeTap;
        TimeSpan calcTime;
        public List<Spell> SpellPriority { get; protected set; }
        public float OverallDamage { get; protected set; }

        public Stats PlayerStats { get; set; }
        public Character character { get; set; }
        public float HitChance { get; set; }
        public float critCount { get; set; }
        public float DPS { get; protected set; }
        public float PetDPS { get; protected set; }
        public float TotalDPS { get; protected set; }
        public List<ManaSource> ManaSources { get; set; }
        public CalculationOptionsWarlock CalculationOptions { get; protected set; }

        public string Name { get; protected set; }
        public string Rotation { get; protected set; }

        public bool UseDSBelow35 { get; protected set; }
        public bool UseDSBelow25 { get; protected set; }
        public bool PatchOn { get; protected set; }
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
                            if (spell.SpellStatistics.CooldownReset < time)
                            {
                                for (int index = 0; index < events.Count; index++)
                                    if (events.Values[index].Name == "Immolate")
                                        return spell;
                                timeTillNextSpell = Math.Min(timeTillNextSpell, spell.SpellStatistics.CooldownReset - GetCastTime(spell));
                            }
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
//                            timeTillNextSpell = Math.Min(timeTillNextSpell, spell.SpellStatistics.CooldownReset - GetCastTime(spell));
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
            PlayerStats.SpellHaste = character.StatConversion.GetSpellHasteFromRating(PlayerStats.HasteRating) / 100f;

            CalculationOptions = character.CalculationOptions as CalculationOptionsWarlock;

            HitChance = PlayerStats.SpellHit * 100f + CalculationOptions.TargetHit;
            if (character.Race == Character.CharacterRace.Draenei && !character.ActiveBuffsContains("Heroic Presence"))
                HitChance += 1;

            ManaSources = new List<ManaSource>();
            SpellPriority = new List<Spell>(CalculationOptions.SpellPriority.Count);
            shadowBolt = SpellFactory.CreateSpell("Shadow Bolt", PlayerStats, character);
            lifeTap = SpellFactory.CreateSpell("Life Tap", PlayerStats, character);
            bool UAImmoChosen = false;
            bool CurseChosen = false;
            foreach (string spellname in CalculationOptions.SpellPriority)
            {
                Spell spelltmp = SpellFactory.CreateSpell(spellname, PlayerStats, character);
                if (spelltmp != null)
                {
                    if ((spelltmp.Name == "Unstable Affliction" || spelltmp.Name == "Immolate") && UAImmoChosen)
                        continue;
                    if (spelltmp.Name.Contains("Curse") && CurseChosen)
                        continue;
                    SpellPriority.Add(spelltmp);
                    spelltmp.SpellStatistics.HitChance = (float)Math.Min(1f, HitChance / 100f + character.WarlockTalents.Suppression * 1f / 100f);
                    if (spelltmp.Name == "Unstable Affliction" || spelltmp.Name == "Immolate")
                        UAImmoChosen = true;
                    if (spelltmp.Name.Contains("Curse"))
                        CurseChosen = true;
                }
            }
            if (SpellPriority.Count == 0) SpellPriority.Add(shadowBolt);
            fillerSpell = SpellPriority[SpellPriority.Count - 1];
//            UseBSBelow35 = CalculationOptions.UseBSBelow35;
//            UseBSBelow25 = CalculationOptions.UseBSBelow25;
//            PatchOn = CalculationOptions.PatchOn;
            PatchOn = false;
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
            maxTime = CalculationOptions.FightLength * 60f;

            bool ImmolateIsUp = false;
            bool ShadowflameIsUp = false;
            bool CleanBreak = false;
            int AffEffectsNumber = CalculationOptions.AffEffectsNumber;
            int ShadowEmbrace = 0;
            int NightfallCounter = 0;
            int EradicationCounter = 0;
            int ISBCounter = 0;
            int MoltenCoreCounter = 0;
            int CounterBuffedIncinerate = 0;
            int CounterBuffedConflag = 0;
            int CounterShadowEmbrace = 0;
            int CounterShadowDotTicks = 0;
            int CounterFireDotTicks = 0;
            int CounterAffEffects = 0;
            int CounterDrainTicks = 0;
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
                switch (currentEvent.Name)
                {
                    case "Done casting":
                    {
                        Spell spell = currentEvent.Spell;

                        if (spell.Cooldown > 0f)
                            spell.SpellStatistics.CooldownReset = time + GetCastTime(spell) + spell.Cooldown;
                        else if (spell.DebuffDuration > 0f)
                        {
                            spell.SpellStatistics.CooldownReset = time + GetCastTime(spell) + spell.DebuffDuration;
                            if (spell.Name == "Curse of Agony" && CalculationOptions.GlyphCoA)
                                spell.SpellStatistics.CooldownReset += 4;
                            if (spell.Name == "Chaos Bolt" && CalculationOptions.GlyphChaosBolt)
                                spell.SpellStatistics.CooldownReset -= 2;
                        }

                        switch (spell.Name)
                        {
                            case "Haunt":
                            {
                                events.Add(time + 12, new Event(spell, "Haunt"));
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
                        Spell spell = currentEvent.Spell;
                        spell.SpellStatistics.TickCount++;
                        if (spell.MagicSchool == MagicSchool.Shadow)
                        {
                            CounterShadowDotTicks++;
                            CounterShadowEmbrace += ShadowEmbrace;
                        }
                        else CounterFireDotTicks++;
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
            if (character.WarlockTalents.Nightfall > 0 || CalculationOptions.GlyphCorruption)
            {
                NightfallProcs = NightfallCounter * (character.WarlockTalents.Nightfall * 0.02f + (CalculationOptions.GlyphCorruption ? 0.04f : 0));
                if (shadowBolt != null)
                    shadowBolt.SpellStatistics.ManaUsed += NightfallProcs * shadowBolt.ManaCost * (GetCastTime(shadowBolt) - shadowBolt.GlobalCooldown) / GetCastTime(shadowBolt);
            }
            float hauntMisses = 0;
            if (haunt != null)
                hauntMisses = haunt.SpellStatistics.HitCount * (1 - haunt.SpellStatistics.HitChance);
            PetCalculations pet = new PetCalculations(simStats, character);
            pet.getPetDPS(this);
            float pactUptime = 0;
            if (character.WarlockTalents.DemonicPact > 0)
            {
                pactUptime = (float)(pet.critCount * 12 / time);
                if (pactUptime > 1) pactUptime = 1;
            }
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
            if (simStats.Mp5 > 0)
            {
                manaGain = simStats.Mp5 / 5f * time;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("MP5", manaGain));
            }
            if (CalculationOptions.FSRRatio < 100)
            {
                manaGain = Math.Floor(character.StatConversion.GetSpiritRegenSec(simStats.Spirit, simStats.Intellect)) * (1f - CalculationOptions.FSRRatio / 100f) * time;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("OutFSR", manaGain));
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
            petManaGain = 0;
            if (character.WarlockTalents.ImprovedSoulLeech > 0)
            {
                manaGain = 0;
                foreach (Spell spell in SpellPriority)
                    if (spell.Name == "Shadow Bolt" || spell.Name == "Shadowburn" || spell.Name == "Chaos Bolt" || spell.Name == "Soul Fire" || spell.Name == "Incinerate" || spell.Name == "Searing Pain" || spell.Name == "Conflagrate")
                        manaGain += spell.SpellStatistics.HitCount * simStats.Mana * character.WarlockTalents.ImprovedSoulLeech * 0.01f * 0.3f;
                currentMana += manaGain;
                petManaGain = manaGain;
                ManaSources.Add(new ManaSource("Improved Soul Leech", manaGain));
            }
            manaGain = 0;
            if (currentMana < 0)
            {
                float numberOfTaps = (float)currentMana / lifeTap.ManaCost;
                manaGain -= currentMana;
                currentMana = 0;

                float fillerManaCost = fillerSpell.SpellStatistics.HitCount / fillerSpell.SpellStatistics.ManaUsed;
                fillerSpell.SpellStatistics.HitCount -= numberOfTaps * GetCastTime(lifeTap) / GetCastTime(fillerSpell);
                fillerSpell.SpellStatistics.ManaUsed -= numberOfTaps * GetCastTime(lifeTap) / GetCastTime(fillerSpell) * fillerManaCost;

                if (simStats.LifeTapBonusSpirit > 0 && simStats.WarlockFelArmor > 0)
                    simStats.SpellPower += (float)(300 * 0.3f * Math.Min(numberOfTaps * 10 / time, 1));
                if (CalculationOptions.GlyphLifeTap)
                    simStats.SpellPower += (float)(simStats.Spirit * 0.2f * Math.Min(numberOfTaps * 20 / time, 1));
                if (character.WarlockTalents.ManaFeed > 0)
                    petManaGain += manaGain;
                ManaSources.Add(new ManaSource("Life Tap", manaGain));
                Rotation += String.Format("\r\n\nNumber of Life Taps: {0}", numberOfTaps);
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
                tmpregen = CalculationOptions.ManaAmt / maxTime * (1f + simStats.BonusManaPotion);
                ManaSources.Add(new ManaSource("Mana Potion", tmpregen));
                regen += tmpregen;
                Rotation += "\r\n- Used Mana Potion";
            }*/
            #endregion

            #region Damage buffs
            double CastsPerSecond = 0;
            double HitsPerSecond = 0;
            double DotTicksPerSecond = 0;
            double PossibleCrits = 0;
            foreach (KeyValuePair<double, Spell> cast in CastList)
            {
                Spell spell = cast.Value;
                CastsPerSecond++;
                HitsPerSecond += spell.SpellStatistics.HitChance;
                if (spell.CritChance > 0) PossibleCrits++;
            }
            CastsPerSecond /= time;
            HitsPerSecond /= time;
            DotTicksPerSecond = (CounterShadowDotTicks + CounterFireDotTicks)/ time;

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
            if (character.WarlockTalents.DemonicPact > 0)
            {
                float pactBuff = simStats.SpellPower * pactUptime * character.WarlockTalents.DemonicPact * 0.02f;
                if (character.ActiveBuffsContains("Totem of Wrath (Spell Power)")) pactBuff -= 280;
                else if (character.ActiveBuffsContains("Flametongue Totem")) pactBuff -= 144;
                else if (character.ActiveBuffsContains("Improved Divine Spirit")) pactBuff -= 80;
                if (pactBuff < 0) pactBuff = 0;
                simStats.SpellPower += pactBuff;
            }
            if (character.WarlockTalents.EmpoweredImp > 0 && CalculationOptions.Pet == "Imp")
            {
                float empImpProcs = pet.critCount * character.WarlockTalents.EmpoweredImp / 3;
                simStats.SpellCrit += (float)(empImpProcs / PossibleCrits * 0.2f);
            }
            float pyroclasmProcs = 0;
            if (character.WarlockTalents.Pyroclasm > 0 && (GetSpellByName("Searing Pain") != null || GetSpellByName("Conflagrate") != null))
            {
                Spell searingPain = GetSpellByName("Searing Pain");
                Spell conflagrate = GetSpellByName("Conflagrate");
                if ( searingPain != null) pyroclasmProcs += searingPain.SpellStatistics.HitCount * searingPain.CritChance;
                if (conflagrate != null) pyroclasmProcs += conflagrate.SpellStatistics.HitCount * conflagrate.CritChance;
                simStats.SpellPower += (float)(character.WarlockTalents.Pyroclasm * 0.02f * 10 * pyroclasmProcs / time);
            }
            #endregion

            #region Spell updates
            critCount = 0;
            foreach (Spell spell in SpellPriority)
            {
                spell.Calculate(simStats, character);
                critCount += spell.SpellStatistics.HitCount * spell.SpellStatistics.HitChance * spell.CritChance;
            }
            #endregion

            #region Damage calculations
            foreach (Spell spell in SpellPriority)
            {
                if (spell == fillerSpell && corruption != null)
                    spell.SpellStatistics.HitCount -= corDrops * GetCastTime(corruption) / GetCastTime(spell);
                float directDamage = spell.AvgDirectDamage * spell.SpellStatistics.HitCount;
                float dotDamage = spell.AvgDotDamage * spell.SpellStatistics.TickCount;
                if (haunt != null && spell.MagicSchool == MagicSchool.Shadow)
                    dotDamage *= 1.2f + (CalculationOptions.GlyphHaunt ? 1 : 0) * 0.03f;
                if (character.WarlockTalents.MasterDemonologist > 0 && CalculationOptions.Pet == "Imp" && spell.MagicSchool == MagicSchool.Fire)
                    spell.CritChance *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                if (character.WarlockTalents.MasterDemonologist > 0 && CalculationOptions.Pet == "Succubus" && spell.MagicSchool == MagicSchool.Shadow)
                    spell.CritChance *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;

                switch (spell.Name)
                {
                    case "Shadow Bolt":
                        {
                            if (simStats.CorruptionTriggersCrit > 0)
                                directDamage = spell.AvgHit * (1f - (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f)) * spell.SpellStatistics.HitCount + spell.AvgCrit * (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f) * spell.SpellStatistics.HitCount;
                            directDamage += NightfallProcs * spell.AvgDirectDamage * (GetCastTime(spell) - spell.GlobalCooldown) / GetCastTime(spell);
                            break;
                        }
                    case "Unstable Affliction":
                        {
                            dotDamage -= (float)(spell.SpellStatistics.DamageDone * hauntMisses * 4 / maxTime);
                            break;
                        }
                    case "Corruption":
                        {
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
                                directDamage = spell.AvgHit * (1f - (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f)) * spell.SpellStatistics.HitCount + spell.AvgCrit * (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f) * spell.SpellStatistics.HitCount;
                            directDamage += CounterBuffedIncinerate * (spell.AvgBuffedDamage - spell.AvgDirectDamage);
                            directDamage *= 1 + (CalculationOptions.GlyphIncinerate ? 1 : 0) * 0.05f;
                            break;
                        }
                    case "Immolate":
                        {
                            if (CalculationOptions.GlyphImmolate)
                            {
                                directDamage *= 0.9f;
                                dotDamage *= 1.2f;
                            }
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
                            if (character.WarlockTalents.MasterDemonologist > 0 && CalculationOptions.Pet == "Imp")
                            {
                                directDamage *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                                dotDamage *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                            }

                            break;
                        }
                    case (MagicSchool.Shadow):
                        {
                            directDamage *= 1 + PlayerStats.BonusShadowDamageMultiplier;
                            dotDamage *= 1 + PlayerStats.BonusShadowDamageMultiplier;
                            if (character.WarlockTalents.MasterDemonologist > 0 && CalculationOptions.Pet == "Succubus")
                            {
                                directDamage *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                                dotDamage *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                            }
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
                if (CounterShadowEmbrace > 0 && spell.MagicSchool == MagicSchool.Shadow)
                    dotDamage *= 1 + (float)CounterShadowEmbrace / (float)CounterShadowDotTicks * character.WarlockTalents.ShadowEmbrace * 0.01f;
                directDamage *= 1 + simStats.WarlockGrandFirestone * 0.01f
                    * 1 + character.WarlockTalents.Metamorphosis * 0.2f * (30 + (CalculationOptions.GlyphMetamorphosis ? 1 : 0) * 6) / (180 * (1 - character.WarlockTalents.Nemesis * 0.1f));
                dotDamage *= 1 + simStats.WarlockGrandSpellstone * 0.01f
                    * 1 + character.WarlockTalents.Metamorphosis * 0.2f * (30 + (CalculationOptions.GlyphMetamorphosis ? 1 : 0) * 6) / (180 * (1 - character.WarlockTalents.Nemesis * 0.1f));
                if (character.WarlockTalents.MasterDemonologist > 0 && CalculationOptions.Pet == "Felguard")
                {
                    directDamage *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                    dotDamage *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                }

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
                int dots = 0;
                foreach (Spell spell in SpellPriority)
                    if ((spell.DebuffDuration > 0) && (spell.DpS > 0)) dots++;
                float ProcChance = 0.1f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance);
                float EffCooldown = (float)(15f + (1 / DotTicksPerSecond) + Math.Log(ProcChance) / Math.Log(ProcActual) / DotTicksPerSecond / ProcActual);
                Spell Extract = new ExtractProc(simStats, character);
                DPS += (Extract.AvgDamage / EffCooldown) * (1f + simStats.BonusShadowDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * HitChance / 100f;
            }
            if (character.WarlockTalents.Metamorphosis > 0)
                DPS += (30 + (CalculationOptions.GlyphMetamorphosis ? 1 : 0) * 6) / (180 * (1 - character.WarlockTalents.Nemesis * 0.1f))
                     * GetCastTime(15) / 30 * (451 + simStats.SpellPower * 1.85f) / (float)time;
            #endregion

            calculatedStats.DpsPoints = DPS;
            PetDPS = new PetCalculations(simStats, character).getPetDPS(this);
            calculatedStats.PetDPSPoints = PetDPS;
            TotalDPS = DPS + PetDPS;

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