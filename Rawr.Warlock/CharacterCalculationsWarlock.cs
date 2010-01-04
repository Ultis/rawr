using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Rawr.Warlock 
{
    public class CharacterCalculationsWarlock : CharacterCalculationsBase
    {
        #region priority queues
        private readonly PriorityQueue<Spell> _warlockAbilities = new PriorityQueue<Spell>();
        private readonly PriorityQueue<Spell> _petAbilities = new PriorityQueue<Spell>();
        #endregion

        #region properties
        private Character Character { get; set; }
        private Stats TotalStats { get; set; }
        private CalculationOptionsWarlock Options { get; set; }
        #endregion

        //replaced by a generic dictionary<string, double> type
        //public class ManaSource
        //{
        //    public string Name { get; set; }
        //    public float Value { get; set; }

        //    public ManaSource(string name, float value)
        //    {
        //        Name = name;
        //        Value = value;
        //    }
        //}
        //public List<ManaSource> ManaSources { get; set; }
        public Dictionary<string, double> ManaSources = new Dictionary<string, double>();

        /// <summary>
        /// A collection of spells that will be used during combat.
        /// </summary>
        public List<Spell> SpellPriority { get; protected set; }


        #region combat auras
        public Backdraft Backdraft = new Backdraft();
        public Decimation Decimation = new Decimation();
        public ShadowEmbrace ShadowEmbrace = new ShadowEmbrace();
        public Pyroclasm Pyroclasm = new Pyroclasm();
        #endregion


        /// <summary>
        /// The events that occurred during combat.
        /// </summary>
        public ArrayList Events = new ArrayList();

        public string Name { get; protected set; }
        public string Abilities { get; protected set; }
        
        public override float OverallPoints 
        {
            get 
            {
                float f = 0f;
                foreach (float f2 in _subPoints) { f += f2; }
                return f;
            }
            set { }
        }

        private float[] _subPoints = new[] { 0f, 0f };
        public override float[] SubPoints 
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DpsPoints 
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float PetDPSPoints 
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float OverallDamage { get; protected set; }
        public float TotalManaCost { get; protected set; }

        public float Time
        {
            get
            {
                return (Options.Duration);
            }
        }
        public float ActiveTime { get; set; }

        #region constructors
        public CharacterCalculationsWarlock( )
        {
            Name = "Priority-based";
            Abilities = "Abilities:";
            SpellPriority = new List<Spell>();
        }

        public CharacterCalculationsWarlock( Character character, Stats stats ) : this()
        {
            Character = character;
            TotalStats = stats;

            if (Character.CalculationOptions == null)
            {
                Character.CalculationOptions = new CalculationOptionsWarlock();
            }

            Options = (CalculationOptionsWarlock)Character.CalculationOptions;
            if (Options.SpellPriority.Count > 0)
            {
                _warlockAbilities.Clear();

                foreach (string s in Options.SpellPriority)
                {
                    Abilities += "\r\n- " + s;

                    Spell spell = SpellFactory.CreateSpell(s, stats, character);
                    if (spell == null) continue;    //i.e. skip over the following lines if null
                    SpellPriority.Add(spell);       //used by "DPS Sources" comparison calcs
                    _warlockAbilities.Enqueue(spell);

                    //wire up the event handlers
                    #region backdraft notifications
                    if (character.WarlockTalents.Backdraft > 0)
                    {
                        if (spell.SpellTree == SpellTree.Destruction)
                        {
                            Backdraft.AuraUpdate += spell.BackdraftAuraHandler;
                            spell.SpellCast += Backdraft.SpellCastHandler;
                        }
                    }
                    #endregion
                    
                }
            }
        }
        #endregion

        #region methods
        public void Calculate()
        {
            //SpecialEffect eradication = new SpecialEffect(Trigger.DamageDone);

            if (_warlockAbilities.Count > 0)
            {
                SimulateCombat(_warlockAbilities);
            }
            
            //if (_petAbilities.Count > 0)
            //{
            //    SimulateCombat(_petAbilities);    
            //}

            int threadid = System.Threading.Thread.CurrentThread.ManagedThreadId;

            //calculate totals 
            foreach (Spell spell in SpellPriority)
            {
                OverallDamage += (float)spell.SpellStatistics.OverallDamage();
                TotalManaCost += (float)spell.SpellStatistics.ManaUsed;

                Trace.WriteLine(String.Format("thread:[{0}] | - {1}: #Hits={2} [Damage={3:0}, Average={4:0}], #Crits={5} [Damage={6:0}, Average={7:0}], #Misses={8}", 
                                              threadid, spell.Name, 
                                              spell.SpellStatistics.HitCount, spell.SpellStatistics.HitDamage ,spell.SpellStatistics.HitAverage(),
                                              spell.SpellStatistics.CritCount, spell.SpellStatistics.CritDamage, spell.SpellStatistics.CritAverage(),
                                              spell.SpellStatistics.MissCount
                                             )
                               );
            }

            DpsPoints = (OverallDamage / ActiveTime);
            Trace.WriteLine(string.Format("thread:[{0}] | ActiveTime={1}", threadid, ActiveTime));

            //StringBuilder sb =  new StringBuilder();
            //foreach (Spell spell in Events)
            //{
            //    sb.AppendLine(string.Format("{0:0.00} {1} casts {2} [damage: {3:0}]", spell.ScheduledTime, Character.Name, spell.Name, spell.MaxHitDamage));
            //}

            //Options.castseq = sb.ToString();
        }

        /// <summary>
        /// A discrete event simulation of combat over time.
        /// </summary>
        private void SimulateCombat(PriorityQueue<Spell> queue)
        {
            int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

            float timer = 0;
            float timelimit = Options.Duration;  //in seconds
            float latency = (Options.Latency / 1000);      //in milliseconds

            DateTime start = DateTime.Now;

            List<string> scheduledTicks = new List<string>();

            Trace.WriteLine("-------------------");
            Trace.WriteLine(String.Format("thread:[{0}] | time: {1:0.00} - simulation starts [timelimit: {2:0.00}]", threadID, timer, timelimit));

            while (queue.Count != 0)
            {
                #region a quick sanity-check to ensure that the simulation never runs longer than the timelimit
                if (timer >= timelimit)
                {
                    //flush the queue & terminate
                    queue.Clear();
                    break;
                }
                #endregion

                //get the spell at the front of the queue
                Spell spell = queue.Dequeue();

                //get the casttime (and take latency into account)
                float casttime = spell.CastTime();
                if (casttime == 0)
                {
                    //instant cast spell, so we have to add the GCD
                    casttime += spell.GlobalCooldown();
                }
                casttime += latency;

                //There could be some occassions where the spell is not actually ready for casting,
                //for example, it might still be on cooldown, or perhaps its duration has not expired.
                //[ or perhaps the rawr user forgot to add a filler spell ... doh! /facepalm ]
                //In this situation we advance the simulation timer forward to achieve the effect of the spell becoming ready for casting.
                if (timer < (spell.ScheduledTime))
                {
                    timer = (spell.ScheduledTime);
                }

                //check if there is enough time left to cast this spell
                if ((timer + casttime) < timelimit)
                {
                    Trace.WriteLine(String.Format("thread:[{0}] | time: {1:0.00} - begins casting: {2} [{3:0.00}s cast, {4:0.00}s latency] - finish casting @ {5:0.00}s", threadID, timer, spell.Name, spell.CastTime(), latency, (timer + casttime)));

                    //the casttime is the time spent waving your hands around in the air
                    timer += (casttime);

                    //TODO: check if any dot spells ticked while we were busy casting this spell
                    //TODO: recalculate stats to account for all combat effects (e.g. +spell, +haste, +spi, +crit bonuses etc)
                    //spell.Stats = updatedStats;
                    //or
                    //spell.Execute(updatedStats);

                    //how long to wait before this spell can be re-cast?
                    float delay = spell.GetTimeDelay();

                    //we are using a PriorityQueue in this time-based event simulation
                    //add the delay to the timer - this becomes the spell's new priority in the priorityqueue
                    //(the priorityqueue is a min-heap queue, i.e. items are removed in ascending order of priority)
                    //however, in cases where items have the same priority value, they are treated as FIFO.
                    spell.ScheduledTime = timer + delay;

                    Trace.WriteLine(String.Format("thread:[{0}] | time: {1:0.00} - finished casting: {2} - re-cast @ approximately {3:0.00}s", threadID, timer, spell.Name, spell.ScheduledTime));

                    //the spell lands on the target, so calculate damage and trigger any related effects
                    spell.Execute();
                    //Trace.WriteLine(String.Format("thread:[{0}] | time: {1:0.00} - {2} spell hits the target for {3:0} damage", threadID, timer, spell.Name, spell.MaxHitDamage() ));

                    //append to the events history
                    Events.Add(spell);

                    if (spell.ScheduledTime < timelimit)
                    {
                        //this spell can be re-cast before the simulation ends, so add it back to the queue
                        queue.Enqueue(spell);
                    }
                    else
                    {
                        //the simulation will end before this spell can be re-cast, so we wont bother adding it to the queue
                        Trace.WriteLine(String.Format("thread:[{0}] | time: {1:0.00} - removing {2} spell - the simulation ends before it can be re-casted", threadID, timer, spell.Name));
                    }
                }
                else
                {
                    //There is still some simulation time left, but not enough to cast the current spell.
                    //This means that the simulation is almost finished.
                    //However, there might be enough time to cast the next spell in the queue ...
                    if (queue.Count > 0)
                    {
                        Spell next = queue.Peek();
                        Trace.WriteLine(String.Format("thread:[{0}] | time: {1:0.00} - not enough time to cast {2} [{3:0.00}s cast, {4:0.00}s lat] - trying next spell: {5}", threadID, timer, spell.Name, spell.CastTime(), latency, next.Name));
                    }
                    else
                    {
                        Trace.WriteLine(String.Format("thread:[{0}] | time: {1:0.00} - not enough time to cast {2} - [this was the last spell in the queue]", threadID, timer, spell.Name));
                    }
                }
            }

            ActiveTime = timer;
            Trace.WriteLine(String.Format("thread:[{0}] | time: {1:0.00} - simulation ends [no spells left in the queue]", threadID, timer));
            DateTime stop = DateTime.Now;
            Trace.WriteLine(String.Format("thread:[{0}] | simulation time: {1} seconds", threadID, (stop - start).Seconds));
        }

        private double GetManaRegenFromSpiritAndIntellect()
        {
            return (Math.Floor(5f * StatConversion.GetSpiritRegenSec(TotalStats.Spirit, TotalStats.Intellect)));
        }

        private double GetManaRegenInCombat()
        {
            return (GetManaRegenFromSpiritAndIntellect() * TotalStats.SpellCombatManaRegeneration + TotalStats.Mp5);
        }

        private double GetManaRegenOutOfCombat()
        {
            return (GetManaRegenFromSpiritAndIntellect() + TotalStats.Mp5);
        }
        #endregion

        /// <summary>
        /// Builds a dictionary containing the values to display for each of the calculations defined in 
        /// CharacterDisplayCalculationLabels. The key should be the Label of each display calculation, 
        /// and the value should be the value to display, optionally appended with '*' followed by any 
        /// string you'd like displayed as a tooltip on the value.
        /// </summary>
        /// <returns>A Dictionary<string, string> containing the values to display for each of the 
        /// calculations defined in CharacterDisplayCalculationLabels.</returns>
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() 
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            #region Simulation stats
            dictValues.Add("Rotation", String.Format("{0}*{1}", Name, Abilities));
            dictValues.Add("Warlock DPS", String.Format("{0:0}", DpsPoints));
            dictValues.Add("Pet DPS", String.Format("{0:0}", PetDPSPoints));
            dictValues.Add("Total DPS", String.Format("{0:0}", OverallPoints));
            dictValues.Add("Damage Done", String.Format("{0:0}", OverallDamage));
            dictValues.Add("Mana Used", String.Format("{0:0}", TotalManaCost));
            #endregion

            #region HP/Mana stats
            dictValues.Add("Health", String.Format("{0:0}", TotalStats.Health));
            dictValues.Add("Mana", String.Format("{0:0}", TotalStats.Mana));
            #endregion

            #region Base stats
            dictValues.Add("Strength", String.Format("{0}", TotalStats.Strength));
            dictValues.Add("Agility", String.Format("{0}", TotalStats.Agility));
            dictValues.Add("Stamina", String.Format("{0}", TotalStats.Stamina));
            dictValues.Add("Intellect", String.Format("{0}", TotalStats.Intellect));
            dictValues.Add("Spirit", String.Format("{0}", TotalStats.Spirit));
            dictValues.Add("Armor", String.Format("{0}", TotalStats.Armor));
            #endregion

            #region Spell stats
            //pet scaling consts: http://www.wowwiki.com/Warlock_minions
            const float petInheritedAttackPowerPercentage = 0.57f;
            const float petInheritedSpellPowerPercentage = 0.15f;

            dictValues.Add("Bonus Damage", String.Format("{0}*Shadow Damage\t{1}\r\nFire Damage\t{2}\r\n\r\nYour Fire Damage increases your pet's Attack Power by {3} and Spell Damage by {4}.",
                TotalStats.SpellPower,
                TotalStats.SpellPower + TotalStats.SpellShadowDamageRating,
                TotalStats.SpellPower + TotalStats.SpellFireDamageRating,
                Math.Round((TotalStats.SpellPower + TotalStats.SpellFireDamageRating) * petInheritedAttackPowerPercentage, 0),
                Math.Round((TotalStats.SpellPower + TotalStats.SpellFireDamageRating) * petInheritedSpellPowerPercentage, 0)
                ));

            #region Hit / Miss chance
            //float bonusHit = TotalStats.SpellHit;
            float onePercentOfHitRating = (1 / StatConversion.GetSpellHitFromRating(1));
            float hitFromRating = StatConversion.GetSpellHitFromRating(TotalStats.HitRating);
            float hitFromTalents = (Character.WarlockTalents.Suppression * 0.01f);
            float hitFromBuffs = (TotalStats.SpellHit - hitFromRating - hitFromTalents);
            float targetHit = (Options.TargetHit / 100f);
            float totalHit = (targetHit + TotalStats.SpellHit);
            float missChance = (totalHit > 1 ? 0 : (1 - totalHit));

            dictValues.Add("Hit Rating", String.Format("{0}*{1:0.00%} Hit Chance (max 100%) | {2:0.00%} Miss Chance \r\n\r\n" 
                + "{3:0.00%}\t Base Hit Chance on a Level {4:0} target\r\n" 
                + "{5:0.00%}\t from {6:0} Hit Rating [gear, food and/or flasks]\r\n" 
                + "{7:0.00%}\t from Talent: Suppression\r\n" 
                + "{8:0.00%}\t from Buffs: Racial and/or Spell Hit Chance Taken\r\n\r\n" 
                + "{9}\r\n\r\n" 
                + "Hit Rating soft caps:\r\n" 
                + "420 - Heroic Presence\r\n" 
                + "368 - Suppression\r\n" 
                + "342 - Suppression and Heroic Presence\r\n"
                + "289 - Suppression, Improved Faerie Fire / Misery\r\n" 
                + "263 - Suppression, Improved Faerie Fire / Misery and  Heroic Presence",
                TotalStats.HitRating, totalHit, missChance,
                targetHit, Options.TargetLevel,
                hitFromRating, TotalStats.HitRating,
                hitFromTalents,
                hitFromBuffs,
                (totalHit > 1) ? String.Format("You are {0} hit rating above the 446 hard cap [no hit from gear, talents or buffs]", Math.Floor((totalHit - 1) * onePercentOfHitRating))
                               : String.Format("You are {0} hit rating below the 446 hard cap [no hit from gear, talents or buffs]", Math.Ceiling((1 - totalHit) * onePercentOfHitRating))
                ));
            #endregion
            
            #region Crit %
            Stats statsBase = BaseStats.GetBaseStats(Character);
            float critFromRating = StatConversion.GetSpellCritFromRating(TotalStats.CritRating);
            float critFromIntellect = StatConversion.GetSpellCritFromIntellect(TotalStats.Intellect);
            float critFromBuffs = TotalStats.SpellCrit - statsBase.SpellCrit - critFromRating - critFromIntellect 
                                - (Character.WarlockTalents.DemonicTactics * 0.02f) 
                                - (Character.WarlockTalents.Backlash * 0.01f);

            dictValues.Add("Crit Chance", String.Format("{0:0.00%}*" 
                                                + "{1:0.00%}\tfrom {2:0} Spell Crit rating\r\n" 
                                                + "{3:0.00%}\tfrom {4:0} Intellect\r\n" 
                                                + "{5:0.000%}\tfrom Warlock Class Bonus\r\n" 
                                                + "{6:0%}\tfrom Talent: Demonic Tactics\r\n" 
                                                + "{7:0%}\tfrom Talent: Backlash\r\n" 
                                                + "{8:0%}\tfrom Buffs",
                    TotalStats.SpellCrit,
                    critFromRating, TotalStats.CritRating,
                    critFromIntellect, TotalStats.Intellect,
                    statsBase.SpellCrit,
                    (Character.WarlockTalents.DemonicTactics * 0.02f),
                    (Character.WarlockTalents.Backlash * 0.01f),
                    critFromBuffs
                ));
            #endregion

            dictValues.Add("Haste Rating", String.Format("{0}%*{1}%\tfrom {2} Haste rating\r\n{3}%\tfrom Buffs\r\n{4}s\tGlobal Cooldown",
                (TotalStats.SpellHaste * 100f).ToString("0.00"),
                (StatConversion.GetSpellHasteFromRating(TotalStats.HasteRating) * 100f).ToString("0.00"),
                TotalStats.HasteRating,
                (TotalStats.SpellHaste * 100f - StatConversion.GetSpellHasteFromRating(TotalStats.HasteRating) * 100f).ToString("0.00"),
                Math.Max(1.0f, 1.5f / (1 + TotalStats.SpellHaste)).ToString("0.00")));

            dictValues.Add("Mana Regen", String.Format("{0}*{0} mana regenerated every 5 seconds while not casting\r\n{1} mana regenerated every 5 seconds while casting", 
                GetManaRegenOutOfCombat(),
                GetManaRegenInCombat()));
            #endregion

            #region Shadow school
            dictValues.Add("Shadow Bolt", new ShadowBolt(TotalStats, Character).ToString());
            if (Character.WarlockTalents.Haunt > 0)
                dictValues.Add("Haunt", new Haunt(TotalStats, Character).ToString());
            else
                dictValues.Add("Haunt", "- *Required talent not available");
            dictValues.Add("Corruption", new Corruption(TotalStats, Character).ToString());
            dictValues.Add("Curse of Agony", new CurseOfAgony(TotalStats, Character).ToString());
            dictValues.Add("Curse of Doom", new CurseOfDoom(TotalStats, Character).ToString());
            if (Character.WarlockTalents.UnstableAffliction > 0)
                dictValues.Add("Unstable Affliction", new UnstableAffliction(TotalStats, Character).ToString());
            else
                dictValues.Add("Unstable Affliction", "- *Required talent not available");
            dictValues.Add("Death Coil", new DeathCoil(TotalStats, Character).ToString());
            dictValues.Add("Drain Life", new DrainLife(TotalStats, Character).ToString());
            dictValues.Add("Drain Soul", new DrainSoul(TotalStats, Character).ToString());
            dictValues.Add("Seed of Corruption", new SeedOfCorruption(TotalStats, Character).ToString());
            dictValues.Add("Shadowflame", new Shadowflame(TotalStats, Character).ToString());
            if (Character.WarlockTalents.Shadowburn > 0)
                dictValues.Add("Shadowburn", new Shadowburn(TotalStats, Character).ToString());
            else
                dictValues.Add("Shadowburn", "- *Required talent not available");
            if (Character.WarlockTalents.Shadowfury > 0)
                dictValues.Add("Shadowfury", new Shadowfury(TotalStats, Character).ToString());
            else
                dictValues.Add("Shadowfury", "- *Required talent not available");
            dictValues.Add("Life Tap", new LifeTap(TotalStats, Character).ToString());
            if (Character.WarlockTalents.DarkPact > 0)
                dictValues.Add("Dark Pact", new DarkPact(TotalStats, Character).ToString());
            else
                dictValues.Add("Dark Pact", "- *Required talent not available");
            
            #endregion

            #region Fire school
            dictValues.Add("Incinerate", new Incinerate(TotalStats, Character).ToString());
            dictValues.Add("Immolate", new Immolate(TotalStats, Character).ToString());
            if (Character.WarlockTalents.Conflagrate > 0)
                dictValues.Add("Conflagrate", new Conflagrate(TotalStats, Character).ToString());
            else
                dictValues.Add("Conflagrate", "- *Required talent not available");
            if (Character.WarlockTalents.ChaosBolt > 0)
                dictValues.Add("Chaos Bolt", new ChaosBolt(TotalStats, Character).ToString());
            else
                dictValues.Add("Chaos Bolt", "- *Required talent not available");
            dictValues.Add("Rain of Fire", new RainOfFire(TotalStats, Character).ToString());
            dictValues.Add("Hellfire", new Hellfire(TotalStats, Character).ToString());

            if (Character.WarlockTalents.Metamorphosis > 0)
                dictValues.Add("Immolation Aura", new ImmolationAura(TotalStats, Character).ToString());
            else 
                dictValues.Add("Immolation Aura", "- *Required talent not available");

            dictValues.Add("Searing Pain", new SearingPain(TotalStats, Character).ToString());
            dictValues.Add("Soul Fire", new SoulFire(TotalStats, Character).ToString());
            #endregion

            return dictValues;
        }

    }
}