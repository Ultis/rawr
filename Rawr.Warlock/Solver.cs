using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Rawr.Warlock 
{
    public class Solver 
    {
        #region Variables
        public EventList events { get; protected set; }
        public float time;
        public float petManaGain;
        Spell lifeTap;

        public List<Spell> SpellPriority { get; protected set; }
        public float OverallDamage { get; protected set; }
        public float lag;

        public Stats PlayerStats { get; set; }
        public Character character { get; set; }
        public float HitChance { get; set; }
        public float critCount { get; set; }
        public float timeTillNextSpell { get; set; }
        public float DPS { get; protected set; }
        public float PetDPS { get; protected set; }
        public float TotalDPS { get; protected set; }
        public List<ManaSource> ManaSources { get; set; }
        public CalculationOptionsWarlock CalculationOptions { get; protected set; }

        public string Name { get; protected set; }
        public string Rotation { get; protected set; }

        public bool UseDSBelow35 { get; protected set; }
        public bool UseDSBelow25 { get; protected set; }
        public int BackdraftCounter { get; protected set; }
        public float maxTime { get; protected set; }
        public float currentMana { get; protected set; }
        public float DSCastEnd { get; protected set; }
        public float ImmolateEnd { get; protected set; }
        public Stats simStats { get; protected set; }
        public Spell fillerSpell { get; protected set; }

        public Spell drainSoul { get; protected set; }
        public Spell corruption { get; protected set; }
        public Spell haunt { get; protected set; }
        public Spell immolate { get; protected set; }
        public Spell shadowBolt { get; protected set; }
        #endregion

        public class ManaSource 
        {
            public string Name { get; set; }
            public float Value { get; set; }

            public ManaSource(string name, float value) 
            {
                Name = name;
                Value = value;
            }
        }

		public class EventList //[Astryl] Refactored this to not use SortedList, which isn't part of Silverlight
		{
			public List<float> Keys = new List<float>();
			public List<Event> Values = new List<Event>();

			public void Add(float _key, Event _Value)
			{
				float key = _key;
				int i = 0;
				for (i = 0; i < Keys.Count; i++)
				{
					if (Keys[i] == key) key += 0.0001f;
					if (Keys[i] > key) break;
				}

				Keys.Insert(i, key);
				Values.Insert(i, _Value);
			}

			public int Count { get { return Keys.Count; } }
			public void RemoveAt(int index)
			{
				Keys.RemoveAt(index);
				Values.RemoveAt(index);
			}

			public bool ContainsKey(float key)
			{
				return Keys.Contains(key);
			}
		}

		public class SpellList
		{
			public List<float> Keys = new List<float>();
			public List<Spell> Values = new List<Spell>();

			public void Add(float _key, Spell _Value)
			{
				float key = _key;
				int i = 0;
				for (i = 0; i < Keys.Count; i++)
				{
					if (Keys[i] == key) key += 0.0001f;
					if (Keys[i] > key) break;
				}

				Keys.Insert(i, key);
				Values.Insert(i, _Value);
			}

			public int Count { get { return Keys.Count; } }
			public void RemoveAt(int index)
			{
				Keys.RemoveAt(index);
				Values.RemoveAt(index);
			}

			public bool ContainsKey(float key)
			{
				return Keys.Contains(key);
			}
		}

        public class Event 
        {
            public Spell Spell { get; protected set; }
            public String Name { get; protected set; }
            public float RealTime { get; protected set; }

            public Event() { }

            public Event(Spell _spell, String _name) 
            {
                Spell = _spell;
                Name = _name;
            }
        }

        //TODO: refactor ...
        public Spell GetCastSpellNew(float time, Stats simStats) 
        {
            timeTillNextSpell = 100;
            foreach (Spell spell in SpellPriority) 
            {
                if (timeTillNextSpell < spell.CastTime) continue;

                float casttime = (spell.CastTime + lag);
                
                switch (spell.Name) 
                {
                    case "Haunt": 
                    {
                        if (spell.SpellStatistics.CooldownReset <= time) 
                        {
                            if ((time + GetGlobalCooldown(spell) + casttime) > (spell.SpellStatistics.CooldownReset + 4) || spell.SpellStatistics.CooldownReset == 0) 
                            {
                                return spell;
                            } 
                            else 
                            {
                                foreach (Spell tempspell in SpellPriority) 
                                {
                                    if (tempspell.Name == "Haunt") continue;

                                    float casttimetemp = tempspell.CastTime + lag;
                                    if ((time + casttimetemp + lag + casttime + lag) <= (spell.SpellStatistics.CooldownReset + 4))
                                    {
                                        return tempspell;
                                    }
                                }
                            }
                            return spell;
                        }
                        timeTillNextSpell = (spell.SpellStatistics.CooldownReset > 0 ? Math.Min(timeTillNextSpell, spell.SpellStatistics.CooldownReset + 4f - time - casttime) : timeTillNextSpell);
                        break;
                    }
                    case "Conflagrate" : 
                    {
                        if (spell.SpellStatistics.CooldownReset <= time) 
                        {
                            for (int index = 0; index < events.Count; index++) 
                            {
                                if (events.Values[index].Name == "Immolate") 
                                { 
                                    return spell; 
                                }
                            }
                            if (spell.Cooldown > 0) 
                            {
                                timeTillNextSpell = (spell.SpellStatistics.CooldownReset > 0 ? Math.Min(timeTillNextSpell, spell.SpellStatistics.CooldownReset) : timeTillNextSpell);
                            } 
                            else 
                            {
                                timeTillNextSpell = (spell.SpellStatistics.CooldownReset > 0 ? Math.Min(timeTillNextSpell, spell.SpellStatistics.CooldownReset - time - casttime) : timeTillNextSpell);
                            }
                        }
                        break;
                    }
                    default: 
                    {
                        if (spell.Name == fillerSpell.Name) 
                        {
                            if (UseDSBelow25 || UseDSBelow35) 
                            {
                                if ((time < DSCastEnd) && (time <= (timeTillNextSpell - drainSoul.CastTime)))
                                    return null;

                                if ((((time) >= (maxTime * 0.65f) && UseDSBelow35)
                                  || ((time) >= (maxTime * 0.75f) && UseDSBelow25)
                                  && ((time) <= (timeTillNextSpell - GetGlobalCooldown(drainSoul)))))
                                {
                                    return drainSoul;
                                }
                            }
                        }
                        if ((time /* + casttime*/) >= spell.SpellStatistics.CooldownReset) 
                        {
                            return spell;
                        }
                        if (spell.Cooldown > 0) 
                        {
                            timeTillNextSpell = (spell.Cooldown - spell.CastTime);
                        } 
                        else 
                        {
                            timeTillNextSpell = (spell.DebuffDuration > 0 ? Math.Min(timeTillNextSpell, spell.DebuffDuration) : timeTillNextSpell);
                            timeTillNextSpell -= (spell.CastTime > 0 ? spell.CastTime : spell.GlobalCooldown);
                        }
                        
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

            CalculationOptions = character.CalculationOptions as CalculationOptionsWarlock;
            lag = CalculationOptions.Delay / 1000f;

            HitChance = PlayerStats.SpellHit * 100f + CalculationOptions.TargetHit;
            if (character.Race == CharacterRace.Draenei && !character.ActiveBuffsContains("Heroic Presence")) 
            { 
                HitChance += 1; 
            }

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
                    if ((spelltmp.Name == "Unstable Affliction" || spelltmp.Name == "Immolate") && UAImmoChosen) { continue; }
                    if (spelltmp.Name.Contains("Curse") && CurseChosen) { continue; }
                    SpellPriority.Add(spelltmp);
                    spelltmp.SpellStatistics.HitChance = (float)Math.Min(1f, HitChance / 100f);
                    if (spelltmp.Name == "Unstable Affliction" || spelltmp.Name == "Immolate") { UAImmoChosen = true; }
                    if (spelltmp.Name.Contains("Curse")) { CurseChosen = true; }
                }
            }

            if (SpellPriority.Count == 0) SpellPriority.Add(shadowBolt);
            fillerSpell = SpellPriority[SpellPriority.Count - 1];
            //            UseBSBelow35 = CalculationOptions.UseBSBelow35;
            //            UseBSBelow25 = CalculationOptions.UseBSBelow25;
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
            {
                Rotation += "\r\n- " + spell.Name;
            }
        }

        //TODO: refactor ...
        public void Calculate(CharacterCalculationsWarlock calculatedStats) 
        {
            if (SpellPriority.Count == 0) { return; }
            simStats = PlayerStats.Clone();
			SpellList CastList = new SpellList();
            events = new EventList();
            maxTime = CalculationOptions.FightLength * 60f;
            currentMana = 0;

            bool ImmolateIsUp = false;
            int AffEffectsNumber = CalculationOptions.AffEffectsNumber;
            int ShadowEmbrace = 0;
            int NightfallCounter = 0;
            int EradicationCounter = 0;
            int ISBCounter = 0;
            int MoltenCoreCounter = 0;
            int CounterBuffedIncinerate = 0;
            int CounterBuffedChaosBolt = 0;
            int CounterShadowEmbrace = 0;
            int CounterShadowDotTicks = 0;
            int CounterFireDotTicks = 0;
            int CounterAffEffects = 0;
            int CounterDrainTicks = 0;
            float Procs2T7 = 0;
            float newtime = 0;
            float lastUsedTime = 0;
            float modCast = 0;

            #region Calculate cast rotation
            Event currentEvent = new Event(null, "Done casting");
            CalculationOptions.castseq = "";
            StringBuilder builder = new StringBuilder();
            while (newtime < maxTime) 
            {
                time = newtime;
                if (currentEvent.Spell != null) 
                {
                    lastUsedTime = time;
                    builder.Append(Math.Round(time, 2) + "\t" + currentEvent.Spell.Name + "\t" + currentEvent.Name + "\r\n");
                }
                switch (currentEvent.Name) 
                {
                    #region Event "Done Casting"
                    case "Done casting": 
                        {
                            Spell spell = GetCastSpellNew(time, simStats);
                            if (spell == null)
                            {
                                events.Add(time + timeTillNextSpell, new Event(null, "Done casting"));
                                break;
                            }
                            /*else if (time < DSCastEnd)
                            {
                                removeEvent(drainSoul);
                                DSCastEnd = 0;
                            }*/
                            if (!CastList.ContainsKey(time))
                            {
                                CastList.Add(time, spell);
                            }
                            spell.SpellStatistics.HitCount++;

                            //modCast used to store spell cast time incase it needs to be modified by backdraft proc
                            modCast = spell.CastTime;
                            //backdraft: cast time and global cooldown of your next three Destruction spells is reduced by 30%. Lasts 15 sec.
                            if (BackdraftCounter > 0 && spell.SpellTree == SpellTree.Destruction && spell.CastTime > 0)
                            {
                                //lower cast speed according to backdraft and consume charge; 1sec GCD hardcap
                                modCast = (float)Math.Max(1.0f, modCast * (1 - (character.WarlockTalents.Backdraft * 0.1)));
                                BackdraftCounter--;
                                if (BackdraftCounter == 0) removeEvent("Backdraft Ends");
                            }

                            if (spell.Name == "Drain Soul" || spell.Name == "Drain Life")
                            {
                                float debuff = spell.TimeBetweenTicks;
                                while (debuff <= spell.DebuffDuration)
                                {
                                    events.Add(time + debuff + lag, new Event(spell, "Dot tick"));
                                    events.Add(time + debuff + lag, new Event(spell, "Done casting"));
                                    debuff += spell.TimeBetweenTicks;
                                }
                            }
                            else
                            {
                                float doneTime = (spell.CastTime > 0 ? modCast : spell.GlobalCooldown);

                                events.Add((time + doneTime + lag), new Event(spell, "Done casting"));

                                if (spell.DebuffDuration > 0f)
                                {
                                    float debuff = spell.DebuffDuration;
                                    //while (debuff <= (spell.DebuffDuration + spell.CastTime))
                                    while (debuff >= spell.TimeBetweenTicks)
                                    {
                                        events.Add(time + debuff + lag + modCast, new Event(spell, "Dot tick"));
                                        debuff -= spell.TimeBetweenTicks;
                                    }
                                    if (spell.SpellTree == SpellTree.Affliction)
                                    {
                                        AffEffectsNumber++;
                                        events.Add(time + doneTime + lag, new Event(spell, "Aff effect"));
                                    }
                                }                                
                            }

                            if (spell.Cooldown > 0f)
                            {
                                spell.SpellStatistics.CooldownReset = time + spell.Cooldown;
                            }
                            else if (spell.DebuffDuration > 0f)
                            {
                                spell.SpellStatistics.CooldownReset = time + spell.DebuffDuration;
                            }

                            switch (spell.Name)
                            {
                                case "Haunt":
                                    {
                                        if (character.WarlockTalents.ShadowEmbrace > 0)
                                        {
                                            ShadowEmbrace = Math.Min(ShadowEmbrace + 1, 2);
                                            if (!removeEvent("Shadow Embrace debuff"))
                                            {
                                                AffEffectsNumber++;
                                            }
                                            events.Add(time + spell.CastTime + spell.DebuffDuration + lag, new Event(spell, "Shadow Embrace debuff"));
                                        }
                                        float nextCorTick = 0;
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
                                            float debuff = nextCorTick;
                                            float lastDebuff = debuff;
                                            while (debuff <= time + spell.CastTime + nextCorTickEvent.Spell.DebuffDuration)
                                            {
                                                lastDebuff = debuff;
                                                events.Add(debuff + lag, new Event(nextCorTickEvent.Spell, "Dot tick"));
                                                debuff += nextCorTickEvent.Spell.TimeBetweenTicks;
                                            }
                                            nextCorTickEvent.Spell.SpellStatistics.CooldownReset = lastDebuff;
                                            events.Add(time + spell.CastTime + nextCorTickEvent.Spell.DebuffDuration + lag, new Event(nextCorTickEvent.Spell, "Aff effect"));
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
                                            events.Add(time + (modCast + 12) + lag, new Event(spell, "Shadow Embrace debuff"));
                                        }
                                        break;
                                    }
                                case "Drain Soul":
                                    {
                                        DSCastEnd = time + spell.CastTime + lag;
                                        break;
                                    }
                                case "Conflagrate":
                                    {

                                        //The Glyph of Conflag (if present) will not consume your Immolate or Shadowflame spell on the target.
                                        //In other words, when this glyph is not present, we must consume the shadowflame or immolate spell.
                                        if (!character.WarlockTalents.GlyphConflag)
                                        {
                                            if (!removeEvent("Shadowflame"))
                                            {
                                                removeEvent(immolate);
                                                ImmolateIsUp = false;
                                                immolate.SpellStatistics.CooldownReset = time;

                                            }
                                        }

                                        //When you cast Conflagrate, the cast time and global cooldown of your next three Destruction spells is reduced by 10/20/30%. Lasts 15 sec.
                                        if (character.WarlockTalents.Backdraft > 0)
                                        {
                                            BackdraftCounter = 3;
                                            events.Add(time + 15, new Event(spell, "Backdraft Ends"));
                                        }
                                        break;
                                    }
                                case "Incinerate":
                                    {
                                        if (ImmolateIsUp) CounterBuffedIncinerate++;
                                        break;
                                    }
                                case "Chaos Bolt":
                                    {
                                        if (ImmolateIsUp) CounterBuffedChaosBolt++;
                                        break;
                                    }
                                case "Immolate":
                                    {
                                        ImmolateIsUp = true;
                                        ImmolateEnd = time + modCast + spell.DebuffDuration + lag;
                                        events.Add(ImmolateEnd, new Event(spell, "Immolate"));
                                        break;
                                    }
                                case "Shadowflame":
                                    {
                                        events.Add(time + spell.CastTime + spell.DebuffDuration + lag, new Event(spell, "Shadowflame"));
                                        break;
                                    }
                            }

                            if (spell.MagicSchool == MagicSchool.Shadow)
                            {
                                if (spell.Name != "Drain Soul")
                                {
                                    MoltenCoreCounter++;
                                }
                            }
                            break;
                        }
                    #endregion

                    #region Event "Dot Tick"
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
                            }
                            if (spell.MagicSchool == MagicSchool.Shadow)
                            {
                                if (spell.Name != "Drain Soul")
                                {
                                    MoltenCoreCounter++;
                                }
                            }
                            break;
                        }
                    #endregion

                    #region Other Events
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
                                case "Backdraft Ends":
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
                    #endregion
                }
                newtime = events.Keys[0];
                currentEvent = events.Values[0];
                events.RemoveAt(0);
            }
            CalculationOptions.castseq = builder.ToString();
            time = lastUsedTime;
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
            if (character.WarlockTalents.Nightfall > 0 || character.WarlockTalents.GlyphCorruption)
            {
                NightfallProcs = NightfallCounter * (character.WarlockTalents.Nightfall * 0.02f + (character.WarlockTalents.GlyphCorruption ? 0.04f : 0));
                if (shadowBolt != null)
                {
                    shadowBolt.SpellStatistics.ManaUsed += (float)(NightfallProcs * shadowBolt.ManaCost * (shadowBolt.CastTime - GetGlobalCooldown(shadowBolt)) / (shadowBolt.CastTime));
                }
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
            float metaUptime = 0;
            if (character.WarlockTalents.Metamorphosis > 0)
            {
                metaUptime = (30 + (character.WarlockTalents.GlyphMetamorphosis ? 1 : 0) * 6) / (180 * (1 - character.WarlockTalents.Nemesis * 0.1f));
                if (metaUptime > 1) metaUptime = 1;
            }
            #endregion

            #region Mana Recovery
            foreach (Spell spell in SpellPriority)
            {
                float manaCost = spell.ManaCost * (spell.Name == "Shadow Bolt" && character.WarlockTalents.GlyphSB ? 0.9f : 1);
                if (spell.DebuffDuration > 0)
                {
                    spell.SpellStatistics.ManaUsed += manaCost * spell.SpellStatistics.TickCount / (spell.DebuffDuration / spell.TimeBetweenTicks);
                    currentMana -= manaCost * spell.SpellStatistics.TickCount / (spell.DebuffDuration / spell.TimeBetweenTicks);
                }
                else
                {
                    spell.SpellStatistics.ManaUsed += manaCost * spell.SpellStatistics.HitCount;
                    currentMana -= manaCost * spell.SpellStatistics.HitCount;
                }
            }
            if (corruption != null)
            {
                fillerSpell.SpellStatistics.ManaUsed -= (float)(corDrops * fillerSpell.ManaCost * corruption.CastTime / fillerSpell.CastTime);
                currentMana += corDrops * fillerSpell.ManaCost * corruption.CastTime / fillerSpell.CastTime;
            }

            float manaGain = simStats.Mana;
            currentMana += manaGain;
            ManaSources.Add(new ManaSource("Intellect", manaGain));
            if (simStats.Mp5 > 0)
            {
                manaGain = simStats.Mp5 / 5f * time;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("MP5", manaGain));
            }
			//TODO: FIX THIS
			//if (CalculationOptions.FSRRatio < 100)
			//{
			//    manaGain = Math.Floor(StatConversion.GetSpiritRegenSec(simStats.Spirit, simStats.Intellect)) * (1f - CalculationOptions.FSRRatio / 100f) * time;
			//    currentMana += manaGain;
			//    ManaSources.Add(new ManaSource("OutFSR", manaGain));
			//}
            if (CalculationOptions.Replenishment > 0)
            {
                manaGain = simStats.Mana * 0.002f * (CalculationOptions.Replenishment / 100f) * time;
                currentMana += manaGain;
                ManaSources.Add(new ManaSource("Replenishment", manaGain));
            }

            //24PPM/60 * 0.02(%mana) = 0.008
            if (simStats.ManaRestoreFromBaseManaPPM > 0)
            {
                float hitCount = 0;
                foreach (Spell spell in SpellPriority)
                    hitCount += spell.SpellStatistics.HitCount;
                manaGain = BaseStats.GetBaseStats(character).Mana * 0.008f * (CalculationOptions.JoW / 100f) * time;
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
                //need to gain mana back up to 0. This will be done by lifetapping. Note that this reduces the number of filler spell casts (and thus their mana need)
                // mana gain = #LT * lifetapMana + numberOfFillerCastsReplaced * fillerManaCost
                // numberOfFillerCastsReplaced = #LT * CastTime(LifeTap) / CastTime(filler)
                // mana gain = #LT * lifetapMana + #LT * CastTime(LifeTap) * fillerManaCost / CastTime(filler)
                // mana gain = #LT * (lifetapMana + fillerManaCost * CastTime(LifeTap) / castTime(filler) )
                // #LT = mana gain / (lifetapMana + fillerManaCost * CastTime(LifeTap) / castTime(filler) )

                float fillerManaCost = fillerSpell.SpellStatistics.HitCount / fillerSpell.SpellStatistics.ManaUsed;
                float numberOfTaps = (float)(currentMana / (lifeTap.ManaCost + fillerManaCost * (lifeTap.CastTime) / (fillerSpell.CastTime)));
                manaGain -= currentMana;
                currentMana = 0;

                fillerSpell.SpellStatistics.HitCount -= (float)(numberOfTaps * (lifeTap.CastTime) / (fillerSpell.CastTime));
                fillerSpell.SpellStatistics.ManaUsed -= (float)(numberOfTaps * (lifeTap.CastTime) / (fillerSpell.CastTime) * fillerManaCost);

                if (simStats.LifeTapBonusSpirit > 0 && simStats.WarlockFelArmor > 0)
                    simStats.SpellPower += (float)(300 * 0.3f * Math.Min(numberOfTaps * 10 / time, 1));
                if (character.WarlockTalents.GlyphLifeTap)
                    simStats.SpellPower += (float)(simStats.Spirit * 0.2f * Math.Min(numberOfTaps * 40 / time, 1));
                if (character.WarlockTalents.ManaFeed > 0)
                    petManaGain += manaGain;
                ManaSources.Add(new ManaSource("Life Tap", manaGain));
                Rotation += String.Format(CultureInfo.InvariantCulture, "\r\n\nNumber of Life Taps: {0}", numberOfTaps);
            }

            /*            if (MPS > regen && character.Race == CharacterRace.BloodElf)
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
            float CastsPerSecond = 0;
            float HitsPerSecond = 0;
            float DotTicksPerSecond = 0;
            float PossibleCrits = 0;
            foreach (Spell spell in CastList.Values)
            {
                //Spell spell = cast.Value;
                CastsPerSecond++;
                HitsPerSecond += spell.SpellStatistics.HitChance;
                if (spell.CritChance > 0) PossibleCrits++;
            }
            CastsPerSecond /= time;
            HitsPerSecond /= time;
            DotTicksPerSecond = (CounterShadowDotTicks + CounterFireDotTicks) / time;


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
                if (searingPain != null) pyroclasmProcs += searingPain.SpellStatistics.HitCount * searingPain.CritChance;
                if (conflagrate != null) pyroclasmProcs += conflagrate.SpellStatistics.HitCount * conflagrate.CritChance;
                //6% additional SP on pyroclasm proc; max 100% uptime
                simStats.SpellPower *= (float)(1 + character.WarlockTalents.Pyroclasm * 0.02f * Math.Min(10 * pyroclasmProcs / time, 1));
            }

            foreach (SpecialEffect effect in simStats.SpecialEffects())
            {
                switch (effect.Trigger)
                {
                    case Trigger.Use:
                        simStats += effect.GetAverageStats();
                        break;
                    case Trigger.DamageSpellHit:
                    case Trigger.SpellHit:
                        simStats += effect.GetAverageStats((float)HitsPerSecond, 1f, 1f, (float)time);
                        break;
                    case Trigger.DamageSpellCrit:
                    case Trigger.SpellCrit:
                        simStats += effect.GetAverageStats((float)PossibleCrits, simStats.SpellCrit, 1f, (float)time);
                        break;
                    case Trigger.DamageSpellCast:
                    case Trigger.SpellCast:
                        simStats += effect.GetAverageStats((float)CastsPerSecond, 1f, 1f, (float)time);
                        break;
                    case Trigger.DoTTick:
                        simStats += effect.GetAverageStats((float)DotTicksPerSecond, 1f, 1f, (float)time);
                        break;
                    case Trigger.DamageDone:
                        simStats += effect.GetAverageStats((float)HitsPerSecond, 1f, 1f, (float)time);
                        break;
                }
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
                    spell.SpellStatistics.HitCount -= (float)(corDrops * (corruption.CastTime) / (spell.CastTime));
                float directDamage = spell.AvgDirectDamage * spell.SpellStatistics.HitCount;
                float dotDamage = spell.AvgDotDamage * spell.SpellStatistics.TickCount;
                if (haunt != null && spell.MagicSchool == MagicSchool.Shadow)
                    dotDamage *= 1.2f + (character.WarlockTalents.GlyphHaunt ? 1 : 0) * 0.03f;
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
                            directDamage += (float)(NightfallProcs * spell.AvgDirectDamage * ((spell.CastTime) - GetGlobalCooldown(spell)) / (spell.CastTime));
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
                            break;
                        }
                    case "Incinerate":
                        {
                            if (simStats.CorruptionTriggersCrit > 0)
                                directDamage = spell.AvgHit * (1f - (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f)) * spell.SpellStatistics.HitCount + spell.AvgCrit * (spell.CritChance + Procs2T7 / spell.SpellStatistics.HitCount * 0.1f) * spell.SpellStatistics.HitCount;
                            directDamage += CounterBuffedIncinerate * (spell.AvgBuffedDamage - spell.AvgDirectDamage);
                            break;
                        }
                    case "Immolate":
                    case "Drain Life":
                    case "Drain Soul":
                        {
                            dotDamage -= (float)(dotDamage * hauntMisses * 4 / maxTime);
                            if (character.WarlockTalents.SoulSiphon == 1)
                                dotDamage *= 1 + Math.Max(0.24f, character.WarlockTalents.SoulSiphon * 0.02f * CounterAffEffects / CounterDrainTicks);
                            else if (character.WarlockTalents.SoulSiphon == 2)
                                dotDamage *= 1 + Math.Max(0.60f, character.WarlockTalents.SoulSiphon * 0.04f * CounterAffEffects / CounterDrainTicks);
                            break;
                        }
                    case "Searing Pain":
                        {
                            if (character.WarlockTalents.GlyphSearingPain)
                                directDamage = spell.AvgHit * (1f - spell.CritChance - 0.2f) + spell.AvgCrit * (spell.CritChance + 0.2f);
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
                            //fire and brimstone -> 10% added damage if immolate is up
                            directDamage += CounterBuffedChaosBolt * (spell.AvgBuffedDamage - spell.AvgDirectDamage);
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
                directDamage *= (1 + simStats.WarlockFirestoneDirectDamageMultiplier);
                dotDamage *= (1f + simStats.WarlockSpellstoneDotDamageMultiplier);
                if (character.WarlockTalents.Metamorphosis > 0)
                {
                    directDamage *= 1 + 0.2f * metaUptime;
                    dotDamage *= 1 + 0.2f * metaUptime;
                }
                if (character.WarlockTalents.MasterDemonologist > 0 && CalculationOptions.Pet == "Felguard")
                {
                    directDamage *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                    dotDamage *= 1 + character.WarlockTalents.MasterDemonologist * 0.01f;
                }

                spell.SpellStatistics.DamageDone = ((directDamage > 0 ? directDamage : 0) + (dotDamage > 0 ? dotDamage : 0)) * spell.SpellStatistics.HitChance;
                if (spell.Name != "Chaos Bolt") {
                    spell.SpellStatistics.DamageDone *= (1f - (CalculationOptions.TargetLevel-character.Level) * 0.02f);
                }

                OverallDamage += spell.SpellStatistics.DamageDone;
            }
            #endregion

            DPS = (float)(OverallDamage / time);

            #region Finalize procs
            /*if (simStats.WarlockGrandFirestone > 0.0f)
            {   // 25% proc chance, 0s internal cd, adds Fire damage
                float ProcChance = 0.25f;
                float ProcActual = 1f - (float)Math.Pow(1f - ProcChance, 1f / ProcChance); // This is the real procchance after the Cumulative chance.
                float EffCooldown = 0f + (float)Math.Log(ProcChance) / (float)Math.Log(ProcActual) / (float)HitsPerSecond / ProcActual;
                Spell Firestone = new FirestoneProc(simStats, character);
                DPS += Firestone.AvgDamage / EffCooldown * (1f + simStats.BonusFireDamageMultiplier) * (1f + simStats.BonusDamageMultiplier) * HitChance / 100f;
            }*/

            foreach (SpecialEffect effect in simStats.SpecialEffects())
            {
                if (effect.Stats.ShadowDamage > 0)
                {
                    switch (effect.Trigger)
                    {
                        case Trigger.DoTTick: //extract or timbals
                            DPS += effect.GetAverageStats(1f / (float)DotTicksPerSecond, 1f, 0f, (float)time).ShadowDamage
                                * (1f + simStats.BonusShadowDamageMultiplier)
                                * (1f + simStats.BonusDamageMultiplier)
                                * HitChance / 100f
                                * (1f + simStats.SpellCrit / 2f);
                            break;
                    }
                }
            }

            if (character.WarlockTalents.Metamorphosis > 0 && CalculationOptions.UseImmoAura)
                if (character.WarlockTalents.GlyphMetamorphosis)
                    DPS += metaUptime * 21 / 36 * (481 + simStats.SpellPower * 0.143f);
                else DPS += metaUptime * 15 / 30 * (481 + simStats.SpellPower * 0.143f);
            #endregion

            calculatedStats.DpsPoints = DPS;
            PetDPS = new PetCalculations(simStats, character).getPetDPS(this);
            calculatedStats.PetDPSPoints = PetDPS;
            TotalDPS = DPS + PetDPS;
        }

        public bool removeEvent(Spell spell)
        {
            bool atLeastOneRemoved = false;
            int index = 0;
            while (index < events.Count)
            {
                if (events.Values[index].Spell == spell)
                {
                    events.RemoveAt(index);
                    atLeastOneRemoved = true;
                    continue;
                }
                index++;
            }
            return atLeastOneRemoved;
        }

        public bool removeEvent(String name)
        {
            bool atLeastOneRemoved = false;
            int index = 0;
            while (index < events.Count)
            {
                if (events.Values[index].Name == name)
                {
                    events.RemoveAt(index);
                    atLeastOneRemoved = true;
                    continue;
                }
                index++;
            }
            return atLeastOneRemoved;
        }

        public bool removeEvent(Spell spell, String name)
        {
            bool atLeastOneRemoved = false;
            int index = 0;
            while (index < events.Count)
            {
                if (events.Values[index].Spell == spell && events.Values[index].Name == name)
                {
                    events.RemoveAt(index);
                    atLeastOneRemoved = true;
                    continue;
                }
                index++;
            }
            return atLeastOneRemoved;
        }

        public float GetGlobalCooldown(Spell spell)
        {
            return Math.Max(1.0f, spell.BaseGlobalCooldown / (1 + simStats.SpellHaste));
        }

        public Spell GetSpellByName(string name)
        {
            foreach (Spell spell in SpellPriority)
            {
                if (spell.Name.Contains(name))
                {
                    return spell;
                }
            }
            return null;
        }
    }
}