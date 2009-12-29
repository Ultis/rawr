using System;
using System.Collections.Generic;
#if RAWR3
using System.Windows.Media;
#else
using System.Diagnostics;
using System.Drawing;
#endif

namespace Rawr.Warlock 
{
    public enum SpellTree { Affliction, Demonology, Destruction }

    public static class SpellFactory
    {
        public static Spell CreateSpell(string name, Stats stats, Character character)
        {
            WarlockTalents talents = character.WarlockTalents;
            switch (name)
            {
                #region shadow school
                case "Shadow Bolt": return new ShadowBolt(stats, character);
                case "Haunt": return (talents.Haunt > 0 ? new Haunt(stats, character) : null);
                case "Corruption": return new Corruption(stats, character);
                case "Curse of Agony": return new CurseOfAgony(stats, character);
                case "Curse of Doom": return new CurseOfDoom(stats, character);
                case "Unstable Affliction": return (talents.UnstableAffliction > 0 ? new UnstableAffliction(stats, character) : null);
                case "Death Coil": return new DeathCoil(stats, character);
                case "Drain Life": return new DrainLife(stats, character);
                case "Drain Soul": return new DrainSoul(stats, character);
                case "Seed of Corruption": return new SeedOfCorruption(stats, character);
                case "Shadowflame": return new Shadowflame(stats, character);
                case "Shadowburn": return (talents.Shadowburn > 0 ? new Shadowburn(stats, character) : null);
                case "Shadowfury": return (talents.Shadowfury > 0 ? new Shadowfury(stats, character) : null);
                case "Life Tap": return new LifeTap(stats, character);
                case "Dark Pact": return (talents.DarkPact > 0 ? new DarkPact(stats, character) : null);
                #endregion

                #region fire school
                case "Immolate": return new Immolate(stats, character);
                case "Conflagrate": return (talents.Conflagrate > 0 ? new Conflagrate(stats, character) : null);
                case "Chaos Bolt": return (talents.ChaosBolt > 0 ? new ChaosBolt(stats, character) : null);
                case "Incinerate": return new Incinerate(stats, character);
                case "Searing Pain": return new SearingPain(stats, character);
                case "Soul Fire": return new SoulFire(stats, character);
                case "Rain of Fire": return new RainOfFire(stats, character);
                case "Hellfire": return new Hellfire(stats, character);
                #endregion

                default: return null;
            }
        }
    }

    /// <summary>
    /// The template class from which all warlock spells are derived.
    /// </summary>
    public abstract class Spell : IComparable<Spell>
    {
        #region nested types
        /// <summary>
        /// A wrapper class to hold a single rank of spell data.
        /// </summary>
        public class SpellRank 
        {
            public int Rank { get; protected set; }
            public int Level { get; protected set; }
            public double BaseMinDamage { get; protected set; }
            public double BaseMaxDamage { get; protected set; }
            public double BaseTickDamage { get; protected set; }
            public double BaseCost { get; protected set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="rank">The spell rank.</param>
            /// <param name="level">The level at which this rank becomes available.</param>
            /// <param name="baseMinDamage">The base minimum direct damage inflicted when the spell hits the target.</param>
            /// <param name="baseMaxDamage">The base maximum direct damage inflicted when the spell hits the target.</param>
            /// <param name="baseTickDamage">The base damage that is inflicted per tick.</param>
            /// <param name="baseCost">The percentage of base mana that it will cost to cast this spell.</param>
            /// <remarks>Only the highest rank of spell data is used at the moment.</remarks>
            public SpellRank(int rank, int level, double baseMinDamage, double baseMaxDamage, double baseTickDamage, double baseCost) 
            {
                Rank = rank;
                Level = level;
                BaseMinDamage = baseMinDamage;
                BaseMaxDamage = baseMaxDamage;
                BaseTickDamage = baseTickDamage;
                BaseCost = baseCost;
            }
        }
        #endregion

        #region General properties
        /// <summary>
        /// The spell name.
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// The highest rank for this spell.
        /// </summary>
        public int Rank { get; protected set; }
        /// <summary>
        /// Binary spells are spells which can only have full effect (hit) or no effect (resist); partial resists are not possible. 
        /// Normally, damage spells are only binary if they have an additional non-damage effect. 
        /// Some examples: Death Coil, Chaos Bolt
        /// </summary>
        public bool Binary { get; protected set; }
        /// <summary>
        /// Indicates if the spell is channeled or not.
        /// </summary>
        public bool Channeled { get; protected set; }
        /// <summary>
        /// Indicates if the spell has an area of effect.
        /// </summary>
        public bool AreaOfEffect { get; protected set; }
        /// <summary>
        /// Indicates if the spell causes damage to the enemy target, or not.
        /// Useful for including non-damaging spells (e.g. LifeTap / DarkPact) in the rotation.
        /// </summary>
        public bool Harmful { get; protected set; }
        /// <summary>
        /// Indicates if the (direct damage) spell can crit, or not.
        /// </summary>
        public bool MayCrit { get; protected set; }
        /// <summary>
        /// Indicates if the spell periodic damage ticks can crit, or not.
        /// </summary>
        public bool TicksMayCrit { get; protected set; }
        /// <summary>
        /// The accumulated stats [gear, talents, buffs, etc] to be used in the calculations.
        /// </summary>
        public Stats Stats { get; protected set; }
        /// <summary>
        /// The current character profile to be used in the calculations.
        /// </summary>
        public Character Character { get; protected set; }
        /// <summary>
        /// The color to be used on the graphs.
        /// </summary>
        public Color GraphColor { get; protected set; }
        /// <summary>
        /// A summary of the spell's major statistics [number of casts, hits, misses, crits, total damage, etc] as calculated during the combat simulation.
        /// </summary>
        public SpellStatistics SpellStatistics { get; protected set; }
        /// <summary>
        /// Every spell belongs to a particular school of magic.
        /// </summary>
        public MagicSchool MagicSchool { get; protected set; }
        /// <summary>
        /// Every spell also belongs to a particular talent tree.
        /// </summary>
        public SpellTree SpellTree { get; protected set; }
        #endregion

        #region properties modified by talents / glyphs
        /// <summary>
        /// The base mana cost percentage. 
        /// </summary>
        public double BaseCost { get; protected set; }
        public double BaseCastTime { get; protected set; }
        public double BaseGlobalCooldown { get; protected set; }
        public double BaseMinDamage { get; protected set; }
        public double BaseMaxDamage { get; protected set; }
        public double BaseTickDamage { get; protected set; }
        public double BaseTickTime { get; protected set; }
        public double BaseDirectDamageCoefficient { get; protected set; }
        public double BaseTickDamageCoefficient { get; protected set; }
        public double BaseMultiplier { get; protected set; }
        public double BaseCritChance { get; set; }
        public double BaseCritMultiplier { get; protected set; }
        public double BaseCritBonusMultiplier { get; protected set; }
        public int BaseRange { get; protected set; }
        public double Duration { get; protected set; }
        public double Cooldown { get; protected set; }
        public int NumberOfTicks { get; protected set; }
        #endregion

        #region aura flags
        public Boolean BackdraftIsActive { get; protected set; }
        #endregion

        #region Calculation methods
        /// <summary>
        /// Calculates the CastTime including haste.
        /// </summary>
        public virtual double CastTime()
        {
            if (BaseCastTime > 0)
            {
                return Math.Max(1.0f, (BaseCastTime / (1 + Stats.SpellHaste)));
            }
            return BaseCastTime;
        }

        /// <summary>
        /// Calculates the GCD including haste.
        /// </summary>
        public virtual double GlobalCooldown()
        {
            if (BaseGlobalCooldown == 1.0)
            {
                return Math.Max(0.5, (BaseGlobalCooldown / (1 + Stats.SpellHaste)));
            }
            return Math.Max(1.0, (BaseGlobalCooldown / (1 + Stats.SpellHaste)));
        }

        /// <summary>
        /// Calculates the Range from BaseRange and any talent modifiers.
        /// </summary>
        public virtual int Range()
        {
            if (SpellTree == SpellTree.Destruction)
            {
                return (int)Math.Round(BaseRange * (1 + Character.WarlockTalents.DestructiveReach * 0.1f));
            }
            if (SpellTree == SpellTree.Affliction)
            {
                return (int)Math.Round(BaseRange * (1 + Character.WarlockTalents.GrimReach * 0.1));   
            }
            //demonology does not have any Range modifiers
            return BaseRange;
        }
        /// <summary>
        /// Calculates the Cost (percentage) from BaseCost (percentage) and any talent modifiers.
        /// </summary>
        public virtual double Cost()
        {
            if (SpellTree == SpellTree.Destruction)
            {
                double cataclysm;
                switch (Character.WarlockTalents.Cataclysm)
                {
                    case 1: cataclysm = 0.04; break;
                    case 2: cataclysm = 0.07; break;
                    case 3: cataclysm = 0.10; break;
                    default:
                        cataclysm = 0.00; break;
                }
                return (BaseCost * (1 - cataclysm));
            }
            if (SpellTree == SpellTree.Affliction)
            {
                return (BaseCost * (1 - (Character.WarlockTalents.Suppression * 0.02)));
            }
            //demonology spells do not have any mana cost reductions at the moment
            return BaseCost;
        }

        /// <summary>
        /// Calculate the mana required to cast the spell.
        /// </summary>
        public virtual double Mana()
        {
            Stats statsBase = BaseStats.GetBaseStats(Character);
            return Math.Floor(statsBase.Mana * Cost());
        }

        /// <summary>
        /// Calculates the minimum (non-critical) spell damage hit.
        /// </summary>
        public virtual double MinDamage()
        {
            double minDamage = CalculateDamage(BaseMinDamage, BaseDirectDamageCoefficient);
            //firestone increases direct damage by 1%
            minDamage *= (1 + Stats.WarlockFirestoneDirectDamageMultiplier);

            return minDamage;
        }
        /// <summary>
        /// Calculates the maximum (non-critical) spell damage hit.
        /// </summary>
        public virtual double MaxDamage()
        {
            double maxDamage = CalculateDamage(BaseMaxDamage, BaseDirectDamageCoefficient);
            //firestone increases direct damage by 1%
            maxDamage *= (1 + Stats.WarlockFirestoneDirectDamageMultiplier);

            return maxDamage;
        }

        /// <summary>
        /// Calculates the (non-critical) damage per tick
        /// </summary>
        public virtual double TickDamage()
        {
            double tickDamage = CalculateDamage(BaseTickDamage, BaseTickDamageCoefficient);
            //spellstone increases periodic damage by 1%
            tickDamage *= (1 + Stats.WarlockSpellstoneDotDamageMultiplier);

            return tickDamage;
        }

        /// <summary>
        /// Common function to calculate (minimum, maximum or tick) damage.
        /// </summary>
        /// <param name="baseValue"></param>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        protected double CalculateDamage(double baseValue, double coefficient)
        {
            double spellpower = Stats.SpellPower;
            double additional = (MagicSchool == MagicSchool.Shadow)
                              ? Stats.SpellShadowDamageRating
                              : Stats.SpellFireDamageRating;

            spellpower += additional;

            double damage = (baseValue + (spellpower * BaseDirectDamageCoefficient));

            BaseMultiplier *= (1 + (Character.WarlockTalents.Malediction * 0.01));

            damage *= BaseMultiplier;

            //add buffs category: Damage(%) [sanctified ret / ferocious inspiration / arcane empowerment]
            damage *= (1 + Stats.BonusDamageMultiplier);

            //add buffs category: Spell Damage Taken [curse of the elements / ebon plaguebringer / earth & moon]
            if (MagicSchool == MagicSchool.Shadow)
            {
                damage *= (1 + Stats.BonusShadowDamageMultiplier);
            }
            else if (MagicSchool == MagicSchool.Fire)
            {
                damage *= (1 + Stats.BonusFireDamageMultiplier);
            }

            return damage;
        }

        /// <summary>
        /// Returns the average damage per (non-critical) hit.
        /// </summary>
        public double AvgHit { get { return (MinDamage() + MaxDamage()) / 2; } }
        /// <summary>
        /// Returns the average damage per hit (including crits).
        /// </summary>
        public double AvgDamage { get { return (AvgHit * (1 - BaseCritChance)) + (AvgCrit * BaseCritChance) + (TickDamage() * NumberOfTicks); } }
        /// <summary>
        /// Returns the average direct damage per hit (including crits)
        /// </summary>
        public double AvgDirectDamage { get { return (AvgHit * (1 - BaseCritChance)) + (AvgCrit * BaseCritChance); } }
        /// <summary>
        /// Returns the average dot damage per hit (including crits).
        /// </summary>
        public double AvgDotDamage { get { return ((TickDamage() * NumberOfTicks) * (1 - BaseCritChance)) + ((TickDamage() * NumberOfTicks) * BaseCritChance * BaseCritBonusMultiplier); } }
        /// <summary>
        /// Returns the damage per casttime. The GCD is used as the casttime for instant-cast spells.
        /// </summary>
        public double DpCT { get { return AvgDamage / (CastTime() > 0 ? CastTime() : GlobalCooldown()); } }
        /// <summary>
        /// Returns the damage per second.
        /// </summary>
        public double DpS { get { return AvgDamage / (Duration > 0 ? (Duration + CastTime()) : (CastTime() > 0 ? CastTime() : GlobalCooldown())); } }
        /// <summary>
        /// Returns the damage per mana.
        /// </summary>
        public double DpM { get { return AvgDamage / Mana(); } }
        /// <summary>
        /// Returns the minimum crit damage.
        /// </summary>
        public double MinCrit { get { return MinDamage() * BaseCritBonusMultiplier; } }
        /// <summary>
        /// Returns the maximum crit damage.
        /// </summary>
        public double MaxCrit { get { return MaxDamage() * BaseCritBonusMultiplier; } }
        /// <summary>
        /// Returns the average crit damage.
        /// </summary>
        public double AvgCrit { get { return (MinCrit + MaxCrit) / 2; } }
        #endregion

        #region Combat simulation helpers
        /// <summary>
        /// Stores the time offset (in the combat simulation) when this spell is scheduled to be re-cast.
        /// </summary>
        public double ScheduledTime { get; set; }

        /// <summary>
        /// Returns the time delay (in seconds) before this spell can be re-cast.
        /// </summary>
        public double GetTimeDelay()
        {
            //if the spell has a cooldown, return it.
            //if there is no cooldown, return its duration instead.
            //if there is no duration, or cooldown, then return its casttime.
            //if there is no casttime (i.e. instant cast spells), return the GCD.
            return (Cooldown > 0) ? Cooldown : (Duration > 0) ? Duration : (CastTime() > 0) ? CastTime() : GlobalCooldown() ;
        }

        /// <summary>
        /// This method contains the logic which decides if a spell missed, hit or crit the target and updates tracked statistics accordingly.
        /// An event will be raised to notify subscribers to trigger additional procs & effects only if the spell cast was successful.
        /// </summary>
        public void Execute()
        {
            SpellStatistics.CastCount += 1; 
            SpellStatistics.HitCount += 1; 
            SpellStatistics.DirectDamage += MaxDamage();
            SpellStatistics.TickDamage += TickDamage();
            SpellStatistics.OverallDamage += (MaxDamage() + (TickDamage() * NumberOfTicks));
            SpellStatistics.ManaUsed += Mana();

            //raise an event so that subscribers can be notified whenever this spell has been cast
            OnSpellCast();
        }

        /// <summary>
        /// This event is raised by the 'Execute' method whenever a spell is cast.
        /// Subscribers must be attached or removed via the "+=" or "-=" operators.
        /// </summary>
        /// <remarks>
        /// <![CDATA[by using the generic EventHandler<T> event type we do not need to declare a separate delegate type.]]>
        /// </remarks>
        protected internal event EventHandler SpellCast;

        /// <summary>
        /// This method ensures that subscribers are notified when the event has been raised.
        /// </summary>
        protected void OnSpellCast()
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler handler = SpellCast;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Aura handlers
        /// <summary>
        /// This method handles event notifications raised by the 'Backdraft' aura class.
        /// </summary>
        /// <param name="sender">An instance of the Backdraft aura class.</param>
        /// <param name="e">An instance of the generic EventArgs class - set to EventArgs.Empty because its unused.</param>
        protected internal virtual void BackdraftAuraHandler(Object sender, EventArgs e)
        {
            if (SpellTree == SpellTree.Destruction)
            {
                Backdraft backdraft = (Backdraft)sender;
                BackdraftIsActive = backdraft.Active;
            }
        }

        /// <summary>
        /// This method handles event notifications raised by the ShadowEmbrace aura class.
        /// </summary>
        /// <param name="sender">An instance of the ShadowEmbrace aura class</param>
        /// <param name="e">An instance of the generic EventArgs class - set to EventArgs.Empty because its unused.</param>
        protected internal virtual void ShadowEmbraceAuraHandler(Object sender, EventArgs e)
        {
            ShadowEmbrace shadowEmbrace = (ShadowEmbrace) sender;
            if (shadowEmbrace.Active)
            {
                Trace.WriteLine(String.Format("thread:[{0}] | ShadowEmbrace aura is active - {1} [shadow school] periodic damage increased by 5%", System.Threading.Thread.CurrentThread.ManagedThreadId, Name));
            }
            else
            {
                Trace.WriteLine(String.Format("thread:[{0}] | ShadowEmbrace aura has been removed - {1} periodic damage loses 5% damage bonus", System.Threading.Thread.CurrentThread.ManagedThreadId, Name));
            }
        }
        #endregion

        #region constructors
        protected Spell(String name, Stats stats, Character character, 
                IEnumerable<SpellRank> spellRanks,
                Color color, MagicSchool magicSchool, SpellTree spellTree) 
        {
            Name = name;
            Stats = stats;
            Character = character;

            foreach (SpellRank spellRank in spellRanks) 
            {
                if (character.Level < spellRank.Level) continue;
                Rank = spellRank.Rank;
                BaseMinDamage = spellRank.BaseMinDamage;
                BaseMaxDamage = spellRank.BaseMaxDamage;
                BaseTickDamage = spellRank.BaseTickDamage;
                BaseCost = spellRank.BaseCost;
            }

            GraphColor = color;
            MagicSchool = magicSchool;
            SpellTree = spellTree;

            //all properties default to zero, except for the following:
            BaseCastTime = 3;
            BaseTickTime = 3;
            
            BaseMultiplier = 1;
            BaseCritMultiplier = BaseCritBonusMultiplier = 1;
            BaseRange = 30;
            Harmful = true;
            
            SpellStatistics = new SpellStatistics();

            if (character.WarlockTalents.AmplifyCurse > 0 && Name.StartsWith("Curse of", StringComparison.Ordinal)) 
            {
                BaseGlobalCooldown = 1.0;
            } 
            else 
            {
                BaseGlobalCooldown = 1.5;
            }
        }
        #endregion

        public override string ToString() 
        {
            if (Duration > 0f && BaseCritChance > 0f)
            {
                return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCrit: {7:0}-{8:0}, Avg {9:0}\r\nTick: {10:0}-{11:0}\r\nCrit Chance: {12:0.00}%\r\nCast: {13:0.00}\r\nCost: {14:0}\r\n{15}",
                                    AvgDamage,
                                    DpS,
                                    DpCT,
                                    DpM,
                                    MinDamage(),
                                    MaxDamage(),
                                    AvgHit,
                                    MinCrit,
                                    MaxCrit,
                                    AvgCrit,
                                    Math.Floor(TickDamage() * NumberOfTicks),
                                    Math.Ceiling(TickDamage() * NumberOfTicks),
                                    (BaseCritChance * 100f),
                                    CastTime(),
                                    Mana(),
                                    Name);
            }
            
            if (Duration > 0)
            {
                return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nTick: {4:0}-{5:0}\r\nDuration: {6:0.00}s\r\nCost: {7}\r\n{8}",
                                     AvgDamage,
                                     DpS,
                                     DpCT,
                                     DpM,
                                     Math.Floor(AvgDamage),
                                     Math.Ceiling(AvgDamage),
                                     Duration,
                                     Mana(),
                                     Name);
            }


            if (BaseCritChance > 0)
            {
                return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCrit: {7:0}-{8:0}, Avg {9:0}\r\nCrit Chance: {10:0.00%}\r\nCast: {11:0.00}\r\nCost: {12}\r\n{13}",
                                     AvgDamage,
                                     DpS,
                                     DpCT,
                                     DpM,
                                     MinDamage(),
                                     MaxDamage(),
                                     AvgHit,
                                     MinCrit,
                                     MaxCrit,
                                     AvgCrit,
                                     BaseCritChance,
                                     CastTime(),
                                     Mana(),
                                     Name);
            }

            return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCast: {7:0.00}\r\nCost: {8}\r\n{9}",
                                 AvgDamage,
                                 DpS,
                                 DpCT,
                                 DpM,
                                 MinDamage(),
                                 MaxDamage(),
                                 AvgHit,
                                 CastTime(),
                                 Mana(),
                                 Name);
        }

        #region IComparable implementation
        /// <summary>
        /// Compares the scheduledtime (which is calculated during the combat simulation) of the current spell to another spell.
        /// </summary>
        /// <param name="other">Some other spell to compare with.</param>
        /// <returns>Returns an int that specifies whether the current instance is less than, equal to or greater than the value of the specified instance.</returns>
        public int CompareTo(Spell other)
        {
            return ScheduledTime.CompareTo(other.ScheduledTime);
        }
        #endregion

    }

    /// <summary>
    /// Sends a shadowy bolt at the enemy, causing 690 to 770 Shadow damage.
    /// </summary>
    public class ShadowBolt : Spell 
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank> 
        { 
            new SpellRank(13, 79, 690, 770, 0, 0.17) 
        };

        public ShadowBolt(Stats stats, Character character)
            : base("Shadow Bolt", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Destruction) 
        {
            MayCrit = true;
            BaseCastTime = 3;
            BaseDirectDamageCoefficient = (BaseCastTime / 3.5);
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowAndFlame * 0.04));
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));
            BaseCritChance = stats.SpellCrit 
                           + (character.WarlockTalents.Devastation * 0.05) 
                           + stats.Warlock4T8 
                           + stats.Warlock2T10;
            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }

        public override double CastTime()
        {
            return Math.Max(1.0, ((BaseCastTime - (Character.WarlockTalents.Bane * 0.1)) / (1 + Stats.SpellHaste)));
        }

        public override double Cost()
        {
            double cost = base.Cost();
            cost *= (1 - (Character.WarlockTalents.GlyphSB ? 0.10 : 0));
            return cost;
        }

        public override string ToString()
        {
            return string.Format("{0:0}*" +
                                "{1} (Rank {2})\r\n" + 
                                "{3:0} mana\r\n" + 
                                "{4:0.00} sec cast\r\nSends a shadowy bolt at the enemy, causing {5:0} to {6:0} Shadow damage.\r\n\r\n" + 
                                "Stats:\r\n" +
                                " - DpS:\t{7:0}\r\n" +
                                " - DpCT:\t{8:0}\r\n" + 
                                " - DpM:\t{9:0}\r\n" +
                                " - Crit:\t{10:0.00%}\r\n\r\n" +
                                "Damage:\r\n" + 
                                " - Hit:\t{11:0} - {12:0}\t[Avg: {13:0}]\r\n" + 
                                " - Crit:\t{14:0} - {15:0}\t[Avg: {16:0}]\r\n" + 
                                " - Avg:\t{17:0}",
                AvgDamage,
                Name, Rank,
                Mana(),
                CastTime(),
                BaseMinDamage, BaseMaxDamage,
                DpS,
                DpCT,
                DpM,
                BaseCritChance,
                MinDamage(), MaxDamage(), AvgHit,
                MinCrit, MaxCrit, AvgCrit, 
                AvgDamage);
        }
    }

    /// <summary>
    /// Deals 582 to 676 Fire damage to your target and an additional 145.5 to 169.0 Fire damage if the target is affected by an Immolate spell.
    /// </summary>
    public class Incinerate : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(4, 80, 582, 676, 0, 0.14)
        };

        public Incinerate(Stats stats, Character character)
            : base("Incinerate", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Fire, SpellTree.Destruction) 
        {
            MayCrit = true;
            BaseCastTime = 2.5;
            BaseDirectDamageCoefficient = (BaseCastTime / 3.5);
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowAndFlame * 0.04));
            BaseMultiplier *= (1 + (character.WarlockTalents.Emberstorm * 0.03)
                                 + (character.WarlockTalents.GlyphIncinerate ? 0.05 : 0));

            BaseCritChance  = stats.SpellCrit 
                            + (character.WarlockTalents.Devastation * 0.05) 
                            + stats.Warlock4T8
                            + stats.Warlock2T10;

            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) 
            //            * (1f + (character.WarlockTalents.Ruin * 0.2f)) 
            //            + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }

        public override double CastTime()
        {
            return Math.Max(1.0, ((BaseCastTime - (Character.WarlockTalents.Emberstorm * 0.05)) / (1 + Stats.SpellHaste)));
        }

        public override string ToString()
        {
            return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCrit: {7:0}-{8:0}, Avg {9:0}\r\nCrit Chance: {10:0.00%}\r\nCast: {11:0.00}\r\nCost: {12:0}\r\n{13}",
                AvgDamage,
                DpS,
                DpCT,
                DpM,
                MinDamage(),
                MaxDamage(),
                AvgHit,
                MinCrit,
                MaxCrit,
                AvgCrit,
                BaseCritChance,
                CastTime(),
                Mana(),
                Name);
        }
    }

    /// <summary>
    /// Curses the target with agony, causing 1740 Shadow damage over 24 sec.  This damage is dealt slowly at first, and builds up as the Curse reaches its full duration.  Only one Curse per Warlock can be active on any one target.
    /// </summary>
    //  TODO: the increasing damage ticks are currently not implemented - using average ticks for the moment
    public class CurseOfAgony : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(9, 79, 0, 0, 145, 0.10)
        };

        public CurseOfAgony(Stats stats, Character character)
            : base("Curse of Agony", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            BaseCastTime = 0;
            BaseTickTime = 2;
            NumberOfTicks = 12;
            Duration = 24;

            if (character.WarlockTalents.GlyphCoA)
            {
                Duration += 4;
                NumberOfTicks += 2;
            }

            //The coefficient is capped at 120% (10% per tick), which is an exception to the general rule (for DoT spells) of : C = Duration / 15
            //http://www.wowwiki.com/Spell_damage_and_healing#Exceptions
            BaseTickDamageCoefficient = 0.10;

            BaseMultiplier *= (1 + (character.WarlockTalents.ImprovedCurseOfAgony * 0.05)
                                 + (character.WarlockTalents.ShadowMastery * 0.03) 
                                 + (character.WarlockTalents.Contagion * 0.01)
                               );
        }
    }

    /// <summary>
    /// Curses the target with impending doom, causing 7300 Shadow damage after 1 min.  If the target yields experience or honor when it dies from this damage, a Doomguard will be summoned.  Cannot be cast on players.
    /// </summary>
    public class CurseOfDoom : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(3, 80, 0, 0, 7300, 0.15)
        };

        public CurseOfDoom(Stats stats, Character character)
            : base("Curse of Doom", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            BaseCastTime = 0;
            BaseTickTime = Duration = 60;
            NumberOfTicks = 1;

            //The CoD spell coefficient has been capped at 200%, so its also an exception to the general spell coeff formula for DoT spells
            BaseTickDamageCoefficient = 2;
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));
        }
    }

    /// <summary>
    /// Corrupts the target, causing 1080 Shadow damage over 18 sec.
    /// </summary>
    public class Corruption : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(10, 77, 0, 0, 180, 0.14)
        };

        public Corruption(Stats stats, Character character)
            : base("Corruption", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            BaseCastTime = 0;
            BaseTickTime = 3;
            NumberOfTicks = 6;
            Duration = 18;

            BaseTickDamageCoefficient = (BaseTickTime / 15f);    //i.e. 20% per tick
            BaseTickDamageCoefficient += (character.WarlockTalents.EmpoweredCorruption * 0.02 ); //each talent point increases damage by 12/24/36%, which must be divided by numberofticks
            BaseTickDamageCoefficient += (character.WarlockTalents.EverlastingAffliction * 0.01);

            BaseMultiplier *= (1 + (character.WarlockTalents.ImprovedCorruption * 0.02)
                                 + (character.WarlockTalents.ShadowMastery * 0.03)
                                 + (character.WarlockTalents.Contagion * 0.01)
                                 + stats.Warlock4T9
                               );
            BaseMultiplier *= (1 + (character.WarlockTalents.SiphonLife * 0.05));

            BaseCritChance = stats.SpellCrit 
                           + (character.WarlockTalents.Malediction * 0.03f)
                           + stats.Warlock2T10;

            if (character.WarlockTalents.Pandemic > 0)
            {
                TicksMayCrit = true;
                BaseCritBonusMultiplier = 2;
            }

            if (character.WarlockTalents.GlyphQuickDecay)
            {
                BaseTickTime *= (1 / (1 + stats.SpellHaste));
            }
        }

        public override string ToString()
        {
            return String.Format("{0:0}*{1} [Rank{2}]\r\n" + 
                "Instant Cast\r\n" +
                "Corrupts the target, causing {3:0} Shadow damage over {4:0} sec.",
                AvgDamage, Name, Rank, AvgDotDamage, Duration );
        }
    }

    /// <summary>
    /// Shadow energy slowly destroys the target, causing 1150 damage over 15 sec.  In addition, if the Unstable Affliction is dispelled it will cause 2070 damage to the dispeller and silence them for 5 sec.
    /// </summary>
    public class UnstableAffliction : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank> 
        {
            new SpellRank(5, 80, 0, 0, 230, 0.15)
        };

        public UnstableAffliction(Stats stats, Character character) 
            : base("Unstable Affliction", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            BaseCastTime = 1.5;
            BaseTickTime = 3;
            NumberOfTicks = 5;
            Duration = 15;

            BaseTickDamageCoefficient = (BaseTickTime / 15);
            BaseTickDamageCoefficient += (character.WarlockTalents.EverlastingAffliction * 0.01);

            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03)
                                 + stats.Warlock2T8
                                 + stats.Warlock4T9
                              ); 
            BaseMultiplier *= (1 + (character.WarlockTalents.SiphonLife * 0.05));

            BaseCritChance = stats.SpellCrit + (character.WarlockTalents.Malediction * 0.03f);

            if (character.WarlockTalents.Pandemic > 0)
            {
                BaseCritBonusMultiplier = 2;
                TicksMayCrit = true;
            }
        }

        public override double CastTime()
        {
            double castTime = base.CastTime();
            if (Character.WarlockTalents.GlyphUA)
            {
                castTime -= 0.2;
            }

            return castTime;
        }
        
        public override string ToString()
        {
            return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCrit: {7:0}-{8:0}, Avg {9:0}\r\nTick: {10:0}-{11:0}\r\nCrit Chance: {12:0.00}%\r\nCast: {13:0.00}\r\nCost: {14:0}\r\n{15}",
                    AvgDamage,
                    DpS,
                    DpCT,
                    DpM,
                    MinDamage(),
                    MaxDamage(),
                    AvgHit,
                    MinCrit,
                    MaxCrit,
                    AvgCrit,
                    Math.Floor(TickDamage() * NumberOfTicks),
                    Math.Ceiling(TickDamage() * NumberOfTicks),
                    (BaseCritChance * 100f),
                    CastTime(),
                    Mana(),
                    Name);
        }
    }

    /// <summary>
    /// Causes the enemy target to run in horror for 3 sec and causes 790 Shadow damage.  The caster gains 300% of the damage caused in health.
    /// </summary>
    public class DeathCoil : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(6, 78, 790, 790, 0, 0.23)
        };

        public DeathCoil(Stats stats, Character character)
            : base("Death Coil", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            MayCrit = true;
            Binary = true;
            BaseCastTime = 0;
            Cooldown = 120;
            Duration = 3;
            if (character.WarlockTalents.GlyphDeathCoil )
            {
                Duration += 0.5;
            }
            BaseDirectDamageCoefficient = ((1.5 / 3.5) / 2);
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }
    }

    /// <summary>
    /// Transfers 133 health every 1 sec from the target to the caster.  Lasts 5 sec.
    /// </summary>
    public class DrainLife : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(9, 78, 0, 0, 133, 0.17)
        };

        public DrainLife(Stats stats, Character character)
            : base("Drain Life", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            BaseCastTime = 0;
            BaseTickTime = 1;
            NumberOfTicks = 5;
            Duration = 5;
            Channeled = true;
            Binary = true;
            BaseTickDamageCoefficient = ((BaseTickTime / 3.5) / 2);
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));
        }
    }

    /// <summary>
    /// Drains the soul of the target, causing 710 Shadow damage over 15 sec.  
    /// If the target is at or below 25% health, Drain Soul causes four times the normal damage. 
    /// </summary>
    /// <remarks>Soul shard gain is not implemented because its irrelevant for the purposes of our simulation.</remarks>
    public class DrainSoul : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(6, 77, 0, 0, 142, 0.14)
        };

        public DrainSoul(Stats stats, Character character)
            : base("Drain Soul", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            Channeled = true;
            Binary = true;
            BaseCastTime = 0;
            BaseTickTime = 3;
            NumberOfTicks = 5;
            Duration = 15;
            BaseDirectDamageCoefficient = ((BaseTickTime / 3.5f) / 2f);
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));
        }
    }

    /// <summary>
    /// You send a ghostly soul into the target, dealing 645 to 753 Shadow damage and increasing all damage done by your Shadow damage-over-time effects on the target by 20% for 12 sec. 
    /// When the Haunt spell ends or is dispelled, the soul returns to you, healing you for 100% of the damage it did to the target.
    /// </summary>
    /// todo: Glyph of Haunt to be implemented under the Haunted aura.
    public class Haunt : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(4, 80, 645, 753, 0, 0.12)
        };

        public Haunt(Stats stats, Character character)
            : base("Haunt", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            BaseCastTime = 1.5;
            Duration = 12;
            Cooldown = 8;
            MayCrit = true;
            BaseDirectDamageCoefficient = (BaseCastTime / 3.5);
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));
            BaseCritBonusMultiplier = (character.WarlockTalents.Pandemic > 0 ? 2 : 1);
        }
    }

    /// <summary>
    /// Embeds a demon seed in the enemy target, causing 1518 Shadow damage over 18 sec.  
    /// When the target takes 1518 total damage or dies, the seed will inflict 1633 to 1897 Shadow damage to all enemies within 15 yards of the target. 
    /// Only one Corruption spell per Warlock can be active on any one target.
    /// </summary>
    // TODO: damage cap not implemented
    public class SeedOfCorruption : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(3, 80, 1633, 1897, 1518, 0.34)
        };

        public SeedOfCorruption(Stats stats, Character character)
            : base("Seed of Corruption", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            BaseCastTime = 2;
            BaseTickTime = 3;
            NumberOfTicks = 6;
            Duration = 18;

            double avgBaseDirectDamage = ((BaseMinDamage + BaseMaxDamage) / 2);

            // coefficients derived as follows: 
            //    dd portion = \frac{2}{3.5}*\frac{x}{x+y}
            //   dot portion = \frac{15}{15}*\frac{y}{x+y}
            // [source: http://elitistjerks.com/f47/t19038-spell_coefficients/#Warlock ]
            // (use http://www.forkosh.dreamhost.com/source_mathtex.html#preview to view the formulas)
            // According to wowhead, the dot portiion = 25% per tick, and the dd portion = 21.29% (but no math provided)
            // TODO: verify the SoC coefficients 
            // (using the EJ formulas for now)

            BaseDirectDamageCoefficient = (BaseCastTime / 3.5f) * (avgBaseDirectDamage / (avgBaseDirectDamage + BaseTickDamage));
            BaseTickDamageCoefficient = (Duration / 15f) * (BaseTickDamage / (avgBaseDirectDamage + BaseTickDamage));

            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03)
                                 + (character.WarlockTalents.Contagion * 0.01)
                              );
            BaseMultiplier *= (1 + (character.WarlockTalents.SiphonLife * 0.05));

            BaseCritChance = stats.SpellCrit 
                       + (character.WarlockTalents.ImprovedCorruption * 0.01f);
            
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }

        public override string ToString()
        {
            return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCrit: {7:0}-{8:0}, Avg {9:0}\r\nTick: {10:0}-{11:0}\r\nCrit Chance: {12:0.00}%\r\nCast: {13:0.00}\r\nCost: {14:0}\r\n{15}",
                    AvgDamage,
                    DpS,
                    DpCT,
                    DpM,
                    MinDamage(),
                    MaxDamage(),
                    AvgHit,
                    MinCrit,
                    MaxCrit,
                    AvgCrit,
                    Math.Floor(TickDamage()),
                    Math.Ceiling(TickDamage()),
                    (BaseCritChance * 100f),
                    CastTime(),
                    Mana(),
                    Name);
        }
    }

    /// <summary>
    /// Targets in a cone in front of the caster take 615 to 671 Shadow damage and an additional 644 Fire damage over 8 sec.
    /// </summary>
    /// <remarks>The directdamage portion is shadow, while the dot portion is fire!</remarks>
    public class Shadowflame : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
           new SpellRank(2, 80,  615, 671, 161, 0.25)
        };

        public Shadowflame(Stats stats, Character character)
            : base("Shadowflame", stats, character, SpellRankTable, Color.FromArgb(255, 255, 215, 0), MagicSchool.Shadow, SpellTree.Destruction) 
        {
            BaseRange = 10;
            BaseCastTime = 0;
            BaseTickTime = 2;
            NumberOfTicks = 4;
            Duration = 8;
            Cooldown = 15;
            MayCrit = true;

            // TODO: verify the Shadowflame coefficients 
            // The spell coefficients are hardcoded in SimCraft as:
            //      DD = 14.29f & DoT = 28%

            // WoWWiki states the spell coefficients formula as:
            //      x = Duration / 15       => (8/15) = 0.53333
            //      y = CastTime / 3.5      => (1.5/3.5) = 0.42857
            //      CDD = y^2 / (x + y)     => (0.42857)^2 / (0.53333 + 0.42857) => 0.190948 = 19.095%
            //      CDoT = x^2 / (x + y)    => (0.53333)^2 / (0.53333 + 0.42857) => 0.29571  = 29.571%
            //      CTotal = CDD + CDot     => 48.67%
            // [source: http://www.wowwiki.com/Spell_damage_and_healing#Hybrid_spells_.28Combined_standard_and_over-time_spells.29]

            // EJ states the coefficients are derived as follows: 
            //   (use http://www.forkosh.dreamhost.com/source_mathtex.html#preview to view the formulas)
            //    DD portion = \frac{2}{3.5}*\frac{x}{x+y}  
            //   Dot portion = \frac{15}{15}*\frac{y}{x+y}
            // where x is the avgDirectDamage ((615+671)/2 =643), and y (=644) is the basedotdamage
            // so this works out to:
            //     CDD = (2/3.5) * (643/(643 + 644)) => 0.5714 * 0.4996 => 0.28549 => 28.55%
            //    CDoT = (8/15 ) * (644/(643 + 644)) => 0.5333 * 0.5003 => 0.26687 => 26.69% [or 6.67% per tick]
            //  CTotal = CDD + CDot => 55.25%
            // [source: http://elitistjerks.com/f47/t19038-spell_coefficients/#Warlock ]
            // Anyhoo - using the EJ formulas (for now) because their theorycrafting has always been pretty accurate)

            var avgBaseDirectDamage = ((BaseMinDamage + BaseMaxDamage) / 2);

            BaseDirectDamageCoefficient = (BaseCastTime / 3.5f) * (avgBaseDirectDamage / (avgBaseDirectDamage + BaseTickDamage));
            //shadowmastery applies to shadow spells, which is the directdamage portion of shadowflame
            BaseDirectDamageCoefficient *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));

            BaseTickDamageCoefficient = (Duration / 15f) * (BaseTickDamage / (avgBaseDirectDamage + BaseTickDamage));
            //emberstorm applies to fire spells, which is the dot portion of shadowflame
            BaseTickDamageCoefficient *= (1 + (character.WarlockTalents.Emberstorm * 0.03));

            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));

            BaseCritChance = stats.SpellCrit + (character.WarlockTalents.Devastation * 0.05);
        }

        public override string ToString()
        {
            return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCrit: {7:0}-{8:0}, Avg {9:0}\r\nTick: {10:0}-{11:0}\r\nCrit Chance: {12:0.00}%\r\nCast: {13:0.00}\r\nCost: {14:0}\r\n{15}",
                    AvgDamage,
                    DpS,
                    DpCT,
                    DpM,
                    MinDamage(),
                    MaxDamage(),
                    AvgHit,
                    MinCrit,
                    MaxCrit,
                    AvgCrit,
                    Math.Floor(TickDamage()),
                    Math.Ceiling(TickDamage()),
                    (BaseCritChance * 100f),
                    CastTime(),
                    Mana(),
                    Name);
        }
    }

    /// <summary>
    /// Instantly blasts the target for 775 to 865 Shadow damage. 
    /// </summary>
    /// <remarks>Soul shard gain is not implemented because its irrelevant for the purposes of our simulation.</remarks>
    public class Shadowburn : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(10, 80, 775, 865, 0, 0.20)
        };

        public Shadowburn(Stats stats, Character character)
            : base("Shadowburn", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Destruction) 
        {
            BaseCastTime = 0;
            BaseRange = 20;
            Cooldown = 15;
            MayCrit = true;
            BaseDirectDamageCoefficient = (1.5 / 3.5);
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowAndFlame * 0.04));
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));
            BaseCritChance = stats.SpellCrit 
                       + (character.WarlockTalents.Devastation * 0.05f);
            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1 + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }
    }

    /// <summary>
    /// Shadowfury is unleashed, causing 968 to 1152 Shadow damage and stunning all enemies within 8 yds for 3 sec.
    /// </summary>
    /// todo: implement aoe effect
    public class Shadowfury : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(5, 80, 968, 1152, 0, 0.27)
        };

        public Shadowfury(Stats stats, Character character)
            : base("Shadowfury", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Destruction) 
        {
            BaseCastTime = 0;
            MayCrit = true;
            Cooldown = Duration = 20;
            BaseRange = 8;
            BaseDirectDamageCoefficient = (1.5 / 3.5);
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowMastery * 0.03));
            BaseCritChance = stats.SpellCrit + (character.WarlockTalents.Devastation * 0.05f);
            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }
    }

    /// <summary>
    /// Burns the enemy for 460 Fire damage and then an additional 785 Fire damage over 15 sec.
    /// </summary>
    public class Immolate : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
           new SpellRank(11, 80,  460, 460, 157, 0.17)
        };

        public Immolate(Stats stats, Character character) 
            : base("Immolate", stats, character, SpellRankTable, Color.FromArgb(255, 255, 215, 0), MagicSchool.Fire, SpellTree.Destruction) 
        {
            BaseCastTime = 2;
            BaseTickTime = 3;
            NumberOfTicks = 5;
            Duration = 15;
            MayCrit = true;

            //immolate coefficients were hotfixed to base 20% (before talents & other bonuses) in patch 3.0
            //so this is another exception to the spell coeff formula for hybrid spells
            BaseDirectDamageCoefficient = 0.20;
            BaseDirectDamageCoefficient *= (1 + (character.WarlockTalents.Emberstorm * 0.03)
                                              + (character.WarlockTalents.ImprovedImmolate * 0.1)
                                              + (stats.Warlock2T8 / 2)
                                              + stats.Warlock4T9
                                           );

            BaseTickDamageCoefficient = 0.20;
            BaseTickDamageCoefficient *= (1 + (character.WarlockTalents.Emberstorm * 0.03)
                                            + (character.WarlockTalents.Aftermath * 0.03)
                                            + (character.WarlockTalents.ImprovedImmolate * 0.10)
                                            + (character.WarlockTalents.GlyphImmolate ? 0.10 : 0)
                                            + (stats.Warlock2T8 / 2)
                                            + stats.Warlock4T9
                                         );

            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
            BaseCritChance = stats.SpellCrit + (character.WarlockTalents.Devastation * 0.05f);
        }

        public override double CastTime()
        {
            return Math.Max(1.0, ((BaseCastTime - (Character.WarlockTalents.Bane * 0.1)) / (1 + Stats.SpellHaste)));
        }

        public override string ToString()
        {
            return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCrit: {7:0}-{8:0}, Avg {9:0}\r\nTick: {10:0}-{11:0}\r\nCrit Chance: {12:0.00}%\r\nCast: {13:0.00}\r\nCost: {14:0}\r\n{15}",
                    AvgDamage,
                    DpS,
                    DpCT,
                    DpM,
                    MinDamage(),
                    MaxDamage(),
                    AvgHit,
                    MinCrit,
                    MaxCrit,
                    AvgCrit,
                    Math.Floor(TickDamage()),
                    Math.Ceiling(TickDamage()),
                    (BaseCritChance * 100f),
                    CastTime(),
                    Mana(),
                    Name);
        }
    }

    /// <summary>
    /// Calls down a fiery rain to burn enemies in the area of effect for 2700 Fire damage over 8 sec.
    /// </summary>
    /// <remarks>
    /// According to http://www.wowwiki.com/Spell_damage_and_healing#Area_of_effect_spells, the coefficient is calculated as follows:
    ///     (C) = (Duration / 7) => 8/7  => ~114% (or 28.57% per tick)
    /// This is supported by theorycrafting on EJ -> http://elitistjerks.com/f47/t19038-spell_coefficients/#Warlock (note: the actual values are out of date on EJ, but its formula is still valid).
    ///     (C) = (CastTime / 3.5), which is then divided by 2 (for AOE spells), which results in ~114% (28.57% per tick).
    ///         => ((2 / 3.5) / 2) => 28.57% per tick
    /// However, http://www.wowwiki.com/Rain_of_Fire states that RoF receives 33% of the bonus from +damage gear (which is then split over the 4 ticks),
    /// while http://www.wowwiki.com/Spell_power_coefficient has the RoF coefficient as 57.26% per tick ...
    /// Anyhoo - going with 28.57% per tick for now :)
    /// TODO: the damage cap is currently not implemented
    /// </remarks>
    public class RainOfFire : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(7, 79, 0, 0, 675, 0.57)
        };

        public RainOfFire(Stats stats, Character character)
            : base("Rain of Fire", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Fire, SpellTree.Destruction) 
        {
            BaseCastTime = 0;
            BaseTickTime = 2;
            NumberOfTicks = 4;
            Duration = 8;
            Channeled = true;
            AreaOfEffect = true;
            MayCrit = true;
            BaseTickDamageCoefficient = ((Duration / 7) / NumberOfTicks);
            BaseMultiplier *= (1 + (character.WarlockTalents.Emberstorm * 0.03));
            BaseCritChance = stats.SpellCrit + (character.WarlockTalents.Devastation * 0.05f);
            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }
    }

    /// <summary>
    /// Ignites the area surrounding the caster, causing 451 Fire self-damage and 451 Fire damage to all nearby enemies every 1 sec.  Lasts 15 sec.
    /// </summary>
    /// <remarks>
    /// coefficient -> http://www.wowwiki.com/Spell_damage_and_healing#Area_of_effect_spells
    ///      C = Duration / 7 => 15/7  => 2.1428 => 214% (or 14.28% per tick over 15 second duration)
    /// TODO: damage cap not implemented
    /// </remarks>
    public class Hellfire : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(5, 78, 0, 0, 451, 0.64)
        };

        public Hellfire(Stats stats, Character character)
            : base("Hellfire", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Fire, SpellTree.Destruction) 
        {
            BaseCastTime = 0;
            BaseTickTime = 1;
            NumberOfTicks = 15;
            Duration = 15;
            Channeled = true;
            AreaOfEffect = true;
            BaseRange = 10;
            BaseTickDamageCoefficient = ((Duration / 7) / NumberOfTicks);
            BaseMultiplier *= (1 + (character.WarlockTalents.Emberstorm * 0.03));
        }
    }

    /// <summary>
    /// Inflict searing pain on the enemy target, causing 343 to 405 Fire damage.  Causes a high amount of threat.
    /// </summary>
    /// <remarks>
    /// TODO: Threatgain not implemented.
    /// </remarks>
    public class SearingPain : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(10, 79, 343, 405, 0, 0.08)
        };

        public SearingPain(Stats stats, Character character)
            : base("Searing Pain", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Fire, SpellTree.Destruction) 
        {
            BaseCastTime = 1.5;
            MayCrit = true;
            BaseDirectDamageCoefficient = (BaseCastTime / 3.5);
            BaseMultiplier *= (1 + (character.WarlockTalents.Emberstorm * 0.03));
            BaseCritChance = stats.SpellCrit 
                       + (character.WarlockTalents.Devastation * 0.05f)
                       + (character.WarlockTalents.ImprovedSearingPain > 0 ? 0.01f + 0.03f * character.WarlockTalents.ImprovedSearingPain: 0);
            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f + (character.WarlockTalents.GlyphSearingPain ? 0.2f : 0)) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }
    }

    /// <summary>
    /// Burn the enemy's soul, causing 1323 to 1657 Fire damage.
    /// </summary>
    /// <remarks>
    /// Soul Fire: Reagent cost not implemented.
    /// The spell coefficient formula for direct damage spells is usually:
    ///      C = CastTime / 3.5f
    /// However, soulfire is an exception to this rule because it has been capped at 115%.
    /// source: http://www.wowwiki.com/Spell_damage_and_healing#Exceptions & http://elitistjerks.com/f47/t19038-spell_coefficients/#Warlock
    /// </remarks>
    public class SoulFire : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(6, 80, 1323, 1657, 0, 0.09)
        };

        public SoulFire(Stats stats, Character character)
            : base("Soul Fire", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Fire, SpellTree.Destruction) 
        {
            MayCrit = true;
            BaseCastTime = 6;
            BaseDirectDamageCoefficient = 1.15;
            BaseMultiplier *= (1 + (character.WarlockTalents.Emberstorm * 0.03));
            BaseCritChance = stats.SpellCrit 
                           + (character.WarlockTalents.Devastation * 0.05)
                           + stats.Warlock2T10;

            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }

        public override double CastTime()
        {
            return Math.Max(1.0, ((BaseCastTime - (Character.WarlockTalents.Bane * 0.4)) / (1 + Stats.SpellHaste)));
        }
    }

    /// <summary>
    /// Causes (or consumes) an Immolate or Shadowflame effect on the enemy target to instantly deal damage equal to 60% of your Immolate or Shadowflame, and causes an additional 20% damage over 6 sec.
    /// </summary>
    public class Conflagrate : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(8, 80, 0, 0, 0, 0.16)
        };

        public Conflagrate(Stats stats, Character character)
            : base("Conflagrate", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Fire, SpellTree.Destruction) 
        {
            BaseCastTime = 0;
            BaseTickTime = 2;
            NumberOfTicks = 3;
            Duration = 6;
            Cooldown = 10;
            MayCrit = true;
            TicksMayCrit = true;    //TODO: need to re-check this - wowhead say no, simcraft say yes ..

            BaseCritChance = stats.SpellCrit 
                       + (character.WarlockTalents.Devastation * 0.05f)
                       + (character.WarlockTalents.FireAndBrimstone * 0.05f);

            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
        }

        /// <summary>
        /// The direct damage portion of Conflag instantly hits for 60% of Immolate or Shadowflame damage.
        /// </summary>
        /// TODO: check for immolate or shadowflame buff
        public override double MinDamage()
        {
            Spell immolate = new Immolate(Stats, Character);
            double damage = immolate.MinDamage() + (immolate.TickDamage() * immolate.NumberOfTicks);
            return (damage * 0.60);
        }

        /// <summary>
        /// The direct damage portion of Conflag instantly hits for 60% of Immolate or Shadowflame damage.
        /// </summary>
        public override double MaxDamage()
        {
            Spell immolate = new Immolate(Stats, Character);
            double damage = immolate.MaxDamage() + (immolate.TickDamage() * immolate.NumberOfTicks);
            return (damage * 0.60);
        }

        /// <summary>
        /// The dot portion of Conflag deals 20% of Immolate or Shadowflame damage over 6 seconds [3 ticks]...
        /// </summary>
        public override double TickDamage()
        {
            Spell immolate = new Immolate(Stats, Character);
            double damage = immolate.MaxDamage() + (immolate.TickDamage() * immolate.NumberOfTicks);
            double tickDamage = ((damage * 0.20) / NumberOfTicks);
            return tickDamage; 
        }

        public override string ToString()
        {
            return String.Format("{0:0} *DpS: {1:0}\r\nDpCT: {2:0}\r\nDpM: {3:0}\r\nHit: {4:0}-{5:0}, Avg {6:0}\r\nCrit: {7:0}-{8:0}, Avg {9:0}\r\nCrit Chance: {10:0.00%}\r\nCast: {11:0.00}\r\nCost: {12}\r\n{13}",
                      AvgDamage,
                      DpS,
                      DpCT,
                      DpM,
                      MinDamage(),
                      MaxDamage(),
                      AvgHit,
                      MinCrit,
                      MaxCrit,
                      AvgCrit,
                      (BaseCritChance * 100f),
                      CastTime(),
                      Mana(),
                      Name);
        }
    }

    /// <summary>
    /// Sends a bolt of chaotic fire at the enemy, dealing 837 to 1061 Fire damage. Chaos Bolt cannot be resisted, and pierces through all absorption effects.
    /// </summary>
    public class ChaosBolt : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(4, 80, 1429, 1813, 0, 0.07)
        };

        public ChaosBolt(Stats stats, Character character)
            : base("Chaos Bolt", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Fire, SpellTree.Destruction) 
        {
            Binary = true;
            MayCrit = true;
            Cooldown = 12;
            BaseCastTime = 2.5;
            BaseDirectDamageCoefficient = BaseCastTime / 3.5;
            BaseMultiplier *= (1 + (character.WarlockTalents.ShadowAndFlame * 0.04));
            BaseMultiplier *= (1 + (character.WarlockTalents.Emberstorm * 0.03));
            BaseCritChance = stats.SpellCrit + (character.WarlockTalents.Devastation * 0.05f);
            //BaseCritBonusMultiplier = (BaseCritMultiplier * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.WarlockTalents.Ruin * 0.2f) + 1f;
            BaseCritBonusMultiplier *= (1 + (character.WarlockTalents.Ruin * 0.20));
            if (character.WarlockTalents.GlyphChaosBolt)
            {
                Cooldown -= 2f;
            }
        }

        public override double CastTime()
        {
            return Math.Max(1.0, ((BaseCastTime - (Character.WarlockTalents.Bane * 0.1)) / (1 + Stats.SpellHaste)));
        }

        public override string ToString()
        {
            double spellpowerBonus = (Stats.SpellPower * BaseDirectDamageCoefficient);
            double burstTotal = (AvgHit + spellpowerBonus) * (1 + (Character.WarlockTalents.Emberstorm * 0.03));
            double talentBonus = (burstTotal - (AvgHit + spellpowerBonus));

            return String.Format("{0:0}*{1} (Rank {2})\r\n" + 
                                 "{3:0} sec cast [{4:0} sec cooldown]\r\n" + 
                                 "{5:0} mana\r\n" +
                                 "Sends a bolt of chaotic fire at the enemy, dealing {6:0} to {7:0} Fire damage.\r\nChaos Bolt cannot be resisted, and pierces through all absorption effects.\r\n\r\n" +
                                 "Burst Breakdown ({8:0}):\r\n" + 
                                 "{9:0}\t{1} average hit\r\n" + 
                                 "{10:0}\tfrom Spellpower [{11:0.00%} coefficient]\r\n" + 
                                 "{12:0}\tfrom Talent: Emberstorm [{13:0.00%} increased fire damage]",
                                 AvgDamage, Name, Rank,
                                 CastTime(), Cooldown,
                                 Mana(),
                                 MinDamage(), MaxDamage(),
                                 burstTotal,
                                 AvgHit,
                                 spellpowerBonus, BaseDirectDamageCoefficient,
                                 talentBonus, (Character.WarlockTalents.Emberstorm * 0.03)
                                );
        }
    }

    /// <summary>
    /// Converts [1490 + SPI * 3] health into [1490 + SPI * 3] mana.  Spirit increases quantity of health converted.
    /// </summary>
    public class LifeTap : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(8, 80, 0, 0, 0, 0)
        };

        public LifeTap(Stats stats, Character character)
            : base("Life Tap", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            Harmful = false;
        }

        public override double Mana()
        {
            return Math.Floor((1490 + (Stats.Spirit * 3)) * (1 + (Character.WarlockTalents.ImprovedLifeTap * 0.10)));
        }

        public override string ToString()
        {
            double lifetapFromSpirit = (Stats.Spirit*3);
            double lifetapFromTalent = (Mana() - lifetapFromSpirit - 1490);

            return String.Format("{0}*{1} (Rank {2})\r\nInstant\r\nConverts {0} health into {0} mana.\r\n\r\nBreakdown:\r\n{3}\tfrom base health\r\n{4}\tfrom Spirit (multiplied by 3)\r\n{5}\tfrom Talent: Improved Life Tap",
                Mana(), 
                Name, 
                Rank, 
                1490,
                lifetapFromSpirit,
                lifetapFromTalent);
        }
    }

    /// <summary>
    /// Drains 1200 of your summoned demon's Mana, returning 100% to you.
    /// </summary>
    public class DarkPact : Spell
    {
        static readonly List<SpellRank> SpellRankTable = new List<SpellRank>
        {
            new SpellRank(5, 80, 0, 0, 0, 0)
        };

        public DarkPact(Stats stats, Character character)
            : base("Dark Pact", stats, character, SpellRankTable, Color.FromArgb(255, 255, 0, 0), MagicSchool.Shadow, SpellTree.Affliction) 
        {
            Harmful = false;
            //http://www.wowwiki.com/Dark_Pact - receives 96% of your +shadow damage bonus
            BaseDirectDamageCoefficient = 0.96f;
        }

        /// <summary>
        /// Calculates the amount of mana returned by DarkPact.
        /// This scales with spellpower from patch 3.3.
        /// </summary>
        public override double Mana()
        {
            return Math.Floor(1200 + (Stats.SpellPower + Stats.SpellShadowDamageRating) * BaseDirectDamageCoefficient);
        }

        public override string ToString()
        {
            double darkpactFromSpellpower = ((Stats.SpellPower + Stats.SpellShadowDamageRating) * BaseDirectDamageCoefficient);

            return String.Format("{0:0}*{1} (Rank {2})\r\nInstant\r\nDrains {0:0} of your summoned demon's Mana, returning 100% to you.\r\n\r\nBreakdown:\r\n{3:0}\tfrom base mana\r\n{4:0}\tfrom {5:0} Spellpower [{6:0%} coefficient]",
                Mana(), 
                Name, 
                Rank, 
                1200,
                darkpactFromSpellpower,
                Stats.SpellPower,
                BaseDirectDamageCoefficient);
        }
    }

}