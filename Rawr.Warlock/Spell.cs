using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Rawr.Warlock
{
    public enum MagicSchool
    {
        Fire = 2,
        Nature,
        Frost,
        Shadow,
        Arcane
    }

    public enum SpellTree
    {
        Affliction,
        Demonology,
        Destruction
    }

    public static class SpellFactory
    {
        public static Spell CreateSpell(string name, Stats stats, Character character)
        {
            switch (name)
            {
                case "Shadow Bolt":
                    return new ShadowBolt(stats, character);
                case "Incinerate":
                    return new Incinerate(stats, character);
                case "Immolate":
                    return new Immolate(stats, character);
                case "Curse of Agony":
                    return new CurseOfAgony(stats, character);
                case "Curse of Doom":
                    return new CurseOfDoom(stats, character);
                case "Corruption":
                    return new Corruption(stats, character);
                case "Siphon Life":
                    if (character.WarlockTalents.SiphonLife > 0)
                        return new SiphonLife(stats, character);
                    else
                        return null;
                case "Unstable Affliction":
                    if (character.WarlockTalents.UnstableAffliction > 0)
                        return new UnstableAffliction(stats, character);
                    else
                        return null;
                /*case "Life Tap":
                      return new LifeTap(stats, character);*/
                /*case "Dark Pact":
                      if (character.WarlockTalents.DarkPact > 0)
                          return new DarkPact(stats, character);
                      else
                          return null;*/
                case "Death Coil":
                    return new DeathCoil(stats, character);
                case "Drain Life":
                    return new DrainLife(stats, character);
                case "Drain Soul":
                    return new DrainSoul(stats, character);
                case "Haunt":
                    if (character.WarlockTalents.Haunt > 0)
                        return new Haunt(stats, character);
                    else
                        return null;
                case "Seed of Corruption":
                    return new SeedOfCorruption(stats, character);
                case "Rain of Fire":
                    return new RainOfFire(stats, character);
                case "Hellfire":
                    return new Hellfire(stats, character);
                case "Searing Pain":
                    return new SearingPain(stats, character);
                case "Shadowflame":
                    return new Shadowflame(stats, character);
                case "Soul Fire":
                    return new SoulFire(stats, character);
                case "Shadowburn":
                    if (character.WarlockTalents.Shadowburn > 0)
                        return new Shadowburn(stats, character);
                    else
                        return null;
                case "Conflagrate":
                    if (character.WarlockTalents.Conflagrate > 0)
                        return new Conflagrate(stats, character);
                    else
                        return null;
                case "Shadowfury":
                    if (character.WarlockTalents.Shadowfury > 0)
                        return new Shadowfury(stats, character);
                    else
                        return null;
                case "Chaos Bolt":
                    if (character.WarlockTalents.ChaosBolt > 0)
                        return new ChaosBolt(stats, character);
                    else
                        return null;
                default:
                    return null;
            }
        }
    }

    public class SpellStatistics
    {
        public float CritCount { get; set; }
        public float MissCount { get; set; }
        public float HitCount { get; set; }
        public float CooldownReset { get; set; }
        public float DamageDone { get; set; }
        public float ManaUsed { get; set; }
    }


    public class Spell
    {
        public class SpellData
        {
            public int Rank { get; protected set; }
            public int Level { get; protected set; }
            public int MinDamage { get; protected set; }
            public int MaxDamage { get; protected set; }

            public SpellData(int rank, int level, int minDamage, int maxDamage)
            {
                Rank = rank;
                Level = level;
                MinDamage = minDamage;
                MaxDamage = maxDamage;
            }
        }

        public class SpellDataDot : SpellData
        {
            public int DotDamage { get; protected set; }
            public SpellDataDot(int rank, int level, int minDamage, int maxDamage, int dotDamage)
                : base(rank, level, minDamage, maxDamage)
            {
                DotDamage = dotDamage;
            }
        }

        public static readonly List<string> SpellList = new List<string>() { "Shadow Bolt", "Curse of Agony", "Curse of Doom", "Corruption", "Siphon Life", "Unstable Affliction", "Life Tap", "Dark Pact", "Death Coil", "Drain Life", "Drain Soul", "Haunt", "Seed of Corruption", "Shadowflame", "Shadowburn", "Shadowfury", "Incinerate", "Immolate", "Rain of Fire", "Hellfire", "Searing Pain", "Soul Fire", "Conflagrate", "Chaos Bolt" };
        public static readonly List<string> ShadowSpellList = new List<string>() { "Shadow Bolt", "Curse of Agony", "Curse of Doom", "Corruption", "Siphon Life", "Unstable Affliction", "Life Tap", "Dark Pact", "Death Coil", "Drain Life", "Drain Soul", "Haunt", "Seed of Corruption", "Shadowflame", "Shadowburn", "Shadowfury" };
        public static readonly List<string> FireSpellList = new List<string>() { "Incinerate", "Immolate", "Rain of Fire", "Hellfire", "Searing Pain", "Soul Fire", "Conflagrate", "Chaos Bolt" };

        public string Name { get; protected set; }

        public MagicSchool MagicSchool { get; protected set; }
        public SpellTree SpellTree { get; protected set; }
        public int Rank { get; protected set; }
        public float BaseMinDamage { get; protected set; }
        public float MinDamage { get; protected set; }
        public float BaseMaxDamage { get; protected set; }
        public float MaxDamage { get; protected set; }
        public float BaseDamageCoef { get; protected set; }
        public float DamageCoef { get; protected set; }
        public float BaseRange { get; protected set; }
        public int Range { get; protected set; }
        public int BaseManaCost { get; protected set; }
        public int ManaCost { get; protected set; }
        public float BaseCastTime { get; protected set; }
        public float CastTime { get; protected set; }
        public float CritChance { get; protected set; }
        public float BaseCritCoef { get; protected set; }
        public float CritCoef { get; protected set; }
        public float DebuffDuration { get; protected set; }
        public int Targets { get; protected set; }

        public float Cooldown { get; protected set; }
        public float GlobalCooldown { get; protected set; }
        public Color GraphColor { get; protected set; }

        public SpellStatistics SpellStatistics { get; protected set; }

        public int BaseMana;

        #region Properties

        public float AvgHit
        {
            get
            {
                return (MinDamage + MaxDamage) / 2;
            }
        }

        public virtual float AvgDamage
        {
            get
            {
                return AvgHit * (1f - CritChance) + AvgCrit * CritChance;
            }
        }

        public virtual float DpCT
        {
            get
            {
                if (CastTime > 0)
                    return AvgDamage / CastTime;
                return AvgDamage / GlobalCooldown;
            }
        }

        public virtual float DpS
        {
            get
            {
                if (DebuffDuration > 0)
                    return AvgDamage / DebuffDuration;
                if (CastTime > 0)
                    return AvgDamage / CastTime;
                return AvgDamage / GlobalCooldown;
            }
        }

        public virtual float DpM
        {
            get
            {
                return AvgDamage / ManaCost;
            }
        }

        public float AvgCrit
        {
            get
            {
                return (MinCrit + MaxCrit) / 2;
            }
        }

        public float MaxCrit
        {
            get
            {
                return MaxDamage * CritCoef;
            }
        }

        public float MinCrit
        {
            get
            {
                return MinDamage * CritCoef;
            }
        }

        #endregion

        public Spell(string name, Stats stats, Character character, List<SpellData> SpellRankTable, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, float cooldown, Color col, MagicSchool magicSchool, SpellTree spelltree)
        {
            DamageCoef = damageCoef;
            DebuffDuration = dotDuration;
            GraphColor = col;
            foreach (SpellData sd in SpellRankTable)
                if (character.Level >= sd.Level)
                {
                    Rank = sd.Rank;
                    BaseMinDamage = MinDamage = sd.MinDamage;
                    BaseMaxDamage = MaxDamage = sd.MaxDamage;
                }
            //Name = string.Format("{0}, Rank {1}", name, Rank);
            Name = name;
            BaseManaCost = ManaCost = manaCost;
            BaseCastTime = CastTime = castTime;
            DebuffDuration = dotDuration;
            BaseDamageCoef = DamageCoef = damageCoef;
            BaseRange = Range = range;
            CritChance = 0f;
            BaseCritCoef = CritCoef = critCoef;
            GlobalCooldown = (float)Math.Max(1.0f, 1.5f / (1 + stats.SpellHaste));
            Cooldown = cooldown;
            SpellStatistics = new SpellStatistics();
            MagicSchool = magicSchool;
            SpellTree = spelltree;

            if (character.Level >= 70 && character.Level <= 80)
                BaseMana = 2871 + (character.Level - 70) * (3856 - 2871) / 10;
            else
                BaseMana = 2871;
        }

        public Spell(string name, Stats stats, Character character, List<SpellData> SpellRankTable, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, float cooldown, Color col) :
            this(name, stats, character, SpellRankTable, manaCost, castTime, critCoef, dotDuration, damageCoef, range, cooldown, col, MagicSchool.Shadow, SpellTree.Affliction)
        {
        }

        public virtual void Calculate(Stats stats, Character character)
        {
        }

        public override string ToString()
        {
            if (DebuffDuration > 0f)
                return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nTick: {4}-{5}\r\nDuration: {6}s\r\nCost: {7}\r\n{8}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpCT.ToString("0.00"),
                          DpM.ToString("0.00"),
                          Math.Floor(AvgDamage / DebuffDuration * 3).ToString("0"),
                          Math.Ceiling(AvgDamage / DebuffDuration * 3).ToString("0"),
                          DebuffDuration.ToString("0"),
                          ManaCost.ToString("0"),
                          Name);

            return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nHit: {4}-{5}, Avg {6}\r\nCrit: {7}-{8}, Avg {9}\r\nCrit Chance: {10}%\r\nCast: {11}\r\nCost: {12}\r\n{13}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpCT.ToString("0.00"),
                DpM.ToString("0.00"),
                MinDamage.ToString("0"), MaxDamage.ToString("0"), AvgHit.ToString("0"),
                MinCrit.ToString("0"), MaxCrit.ToString("0"), AvgCrit.ToString("0"),
                (CritChance * 100f).ToString("0.00"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"),
                Name);
        }
    }

    public class ShadowBolt : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(13, 79, 690, 770)
        };

        public ShadowBolt(Stats stats, Character character)
            : base("Shadow Bolt", stats, character, SpellRankTable, 17, 3f, 1.5f, 0f, 3f / 3.5f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Bane * 0.1f));
        }
    }

    //Incinerate: 582-676 dmg + 145-169 if Immolate is up. Immolate assumed for now.
    public class Incinerate : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(4, 80, 727, 845)
        };

        public Incinerate(Stats stats, Character character)
            : base("Incinerate", stats, character, SpellRankTable, 14, 2.5f, 1.5f, 0f, 2.5f / 3.5f, 30, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
            
            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Emberstorm * 0.05f));
        }
    }

    //Curse of Agony: avg calculated. increasing damageticks not implemented.Only one Curse per Warlock can be active on any one target.
    public class CurseOfAgony : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(9, 79, 1740, 1740)
        };

        public CurseOfAgony(Stats stats, Character character)
            : base("Curse of Agony", stats, character, SpellRankTable, 10, 0f, 0, 24f, 1.2f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + (character.WarlockTalents.ImprovedCurseOfAgony * 0.05f + character.WarlockTalents.ShadowMastery * 0.02f + character.WarlockTalents.Contagion * 0.01f))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + (character.WarlockTalents.ImprovedCurseOfAgony * 0.05f + character.WarlockTalents.ShadowMastery * 0.02f + character.WarlockTalents.Contagion * 0.01f))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Curse of Doom: does 7300dmg after 1 min. Only one Curse per Warlock can be active on any one target.
    public class CurseOfDoom : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(3, 80, 7300, 7300)
        };

        public CurseOfDoom(Stats stats, Character character)
            : base("Curse of Doom", stats, character, SpellRankTable, 15, 0f, 0, 60f, 2f, 30, 60f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                //does not benefit from ShadowMastery atm                                         
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                //does not benefit from ShadowMastery atm                                              
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Corruption: ticks every 3 sec. Haunt does not refresh but add 15 extra sec to this.
    public class Corruption : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(10, 77, 1080, 1080)
        };

        public Corruption(Stats stats, Character character)
            : base("Corruption", stats, character, SpellRankTable, 14, 0f, 0, 18f, 1.2f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EmpoweredCorruption * 0.12f + character.WarlockTalents.EverlastingAffliction * 0.01f * 6))
                      * (1 + (character.WarlockTalents.ImprovedCorruption * 0.02f + character.WarlockTalents.ShadowMastery * 0.02f + character.WarlockTalents.Contagion * 0.01f))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EmpoweredCorruption * 0.12f + character.WarlockTalents.EverlastingAffliction * 0.01f * 6))
                      * (1 + (character.WarlockTalents.ImprovedCorruption * 0.02f + character.WarlockTalents.ShadowMastery * 0.02f + character.WarlockTalents.Contagion * 0.01f))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Siphon Life: Transfers 81 health every 3 sec to the caster. Only dmg done implemented.
    public class SiphonLife : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(8, 80, 810, 810)
        };

        public SiphonLife(Stats stats, Character character)
            : base("Siphon Life", stats, character, SpellRankTable, 16, 0f, 0, 30f, 1f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EverlastingAffliction * 0.01f * 10))
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EverlastingAffliction * 0.01f * 10))
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Unstable Affliction: if dispelled, deals 2070 dmg to dispeller. Not calculated.
    public class UnstableAffliction : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(5, 80, 1150, 1150)
        };

        public UnstableAffliction(Stats stats, Character character)
            : base("Unstable Affliction", stats, character, SpellRankTable, 15, 1.5f, 0, 15f, 1f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EverlastingAffliction * 0.01f * 5))
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EverlastingAffliction * 0.01f * 5))
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Death Coil: Causes 790 dmg, Caster gains 100% of dmg done in health. Only dmg done implemented.
    public class DeathCoil : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(6, 78, 790, 790)
        };

        public DeathCoil(Stats stats, Character character)
            : base("Death Coil", stats, character, SpellRankTable, 23, 0f, 0, 0f, 0.22f, 30, 120f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Drain Life: channeled - 133 health from target to caster every second. Only dmg done implemented.
    public class DrainLife : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(9, 78, 665, 665)
        };

        public DrainLife(Stats stats, Character character)
            : base("Drain Life", stats, character, SpellRankTable, 17, 5f, 0, 0f, 5f / 2 / 3.5f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Drain Soul: Channeled - 710 dmg over 15 sec, ticks every 3 sec. If target <25% health Drain Soul causes 4 times the dmg. < 25% multiplier, Ticks and Shard gained not implemented.
    public class DrainSoul : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(6, 77, 710, 710)
        };

        public DrainSoul(Stats stats, Character character)
            : base("Drain Soul", stats, character, SpellRankTable, 14, 15f, 0, 0f, 5f / 2 / 3.5f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Haunt:  increases all dmg over time effects on target by 20% for 12 sec + returns 100% of dmg to caster. dotdmg increase and health gained not implemented.
    public class Haunt : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(4, 80, 645, 753)
        };

        public Haunt(Stats stats, Character character)
            : base("Haunt", stats, character, SpellRankTable, 12, 1.5f, 1.5f, 0f, 1.5f / 3.5f, 30, 8f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }
    //Seed of Corruption: Aoe effect not added. Shorter duration not added. Only one Corruption spell per Warlock can be active on any one target.
    public class SeedOfCorruption : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(3, 80, 1518, 1518)
        };

        public SeedOfCorruption(Stats stats, Character character)
            : base("Seed of Corruption", stats, character, SpellRankTable, 34, 2f, 1.5f, 18f, 1.5f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f + character.WarlockTalents.Contagion * 0.01f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f + character.WarlockTalents.Contagion * 0.01f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            CritChance = stats.SpellCrit + character.WarlockTalents.ImprovedCorruption * 0.01f;

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }

    //Shadowflame: check coeffeccients. aoe not implemented.
    public class Shadowflame : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
           new SpellDataDot(2, 80,  615, 671, 644)
        };

        public float BaseDotDamage { get; protected set; }
        public float DotDamage { get; protected set; }

        public override float AvgDamage
        {
            get
            {
                return AvgHit * (1f - CritChance) + AvgCrit * CritChance + DotDamage;
            }
        }

        public override float DpCT
        {
            get
            {
                return AvgDamage / CastTime;
            }
        }

        public override float DpS
        {
            get
            {
                return AvgDamage / CastTime;
            }
        }

        public Shadowflame(Stats stats, Character character)
            : base("Shadowflame", stats, character, SpellRankTable, 2, 0f, 1.5f, 8f, 1.5f / 3.5f, 10, 15f, Color.Gold, MagicSchool.Shadow, SpellTree.Destruction)
        {
            Calculate(stats, character);
            foreach (SpellData sd in SpellRankTable)
                if (character.Level > sd.Level)
                {
                    BaseDotDamage = DotDamage = ((SpellDataDot)sd).DotDamage;
                }

        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellFireDamageRating) * 1f / 6f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            CastTime = (float)Math.Max(1.0f, (BaseCastTime /*- character.PriestTalents.DivineFury * 0.1f*/) / (1 + stats.SpellHaste));

            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit;

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.HolyReach * 0.1f*/));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nHit: {4}-{5}, Avg {6}\r\nCrit: {7}-{8}, Avg {9}\r\nTick: {10}-{11}\r\nCrit Chance: {12}%\r\nCast: {13}\r\nCost: {14}\r\n{15}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpCT.ToString("0.00"),
                DpM.ToString("0.00"),
                MinDamage.ToString("0"), MaxDamage.ToString("0"), AvgHit.ToString("0"),
                MinCrit.ToString("0"), MaxCrit.ToString("0"), AvgCrit.ToString("0"),
                Math.Floor(DotDamage / 8f).ToString("0"), Math.Ceiling(DotDamage / 8f).ToString("0"),
                (CritChance * 100f).ToString("0.00"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"),
                Name);
        }
    }

    //Shadowburn: Soulshard if target dies not implemented.
    public class Shadowburn : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(10, 80, 775, 865)
        };

        public Shadowburn(Stats stats, Character character)
            : base("Shadowburn", stats, character, SpellRankTable, 20, 0f, 1.5f, 0f, 1.5f / 3.5f, 20, 15f, Color.Red, MagicSchool.Shadow, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }
    //Shadowfury: does not trigger gcd, check needed. aoe-effect not implemented.
    public class Shadowfury : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(5, 80, 968, 1152)
        };

        public Shadowfury(Stats stats, Character character)
            : base("Shadowfury", stats, character, SpellRankTable, 27, 0f, 1.5f, 0f, 0.195f, 30, 20f, Color.Red, MagicSchool.Shadow, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Immolate: check coefficients and crit for DD/dot -> 100% DD, 20% DoT, not implemented.
    public class Immolate : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
           new SpellDataDot(11, 80,  460, 460, 785)
        };

        public float BaseDotDamage { get; protected set; }
        public float DotDamage { get; protected set; }

        public override float AvgDamage
        {
            get
            {
                return AvgHit * (1f - CritChance) + AvgCrit * CritChance + DotDamage;
            }
        }

        public override float DpCT
        {
            get
            {
                return AvgDamage / CastTime;
            }
        }

        public override float DpS
        {
            get
            {
                return AvgDamage / CastTime;
            }
        }

        public Immolate(Stats stats, Character character)
            : base("Immolate", stats, character, SpellRankTable, 17, 2f, 1.5f, 15f, 2f / 3.5f, 30, 0f, Color.Gold, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
            foreach (SpellData sd in SpellRankTable)
                if (character.Level > sd.Level)
                {
                    BaseDotDamage = DotDamage = ((SpellDataDot)sd).DotDamage;
                }

        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ImprovedImmolate * 0.1f ))
                      * (1 + character.WarlockTalents.Malediction * 0.01f + character.WarlockTalents.FireAndBrimstone * 0.03f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ImprovedImmolate * 0.1f ))
                      * (1 + character.WarlockTalents.Malediction * 0.01f + character.WarlockTalents.FireAndBrimstone * 0.03f);


            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellFireDamageRating) * 1f / 6f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f + character.WarlockTalents.FireAndBrimstone * 0.03f);

            CastTime = (float)Math.Max(1.0f, (BaseCastTime /*- character.PriestTalents.DivineFury * 0.1f*/) / (1 + stats.SpellHaste));

            //CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit;

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Bane * 0.1f));

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.HolyReach * 0.1f*/));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nHit: {4}-{5}, Avg {6}\r\nCrit: {7}-{8}, Avg {9}\r\nTick: {10}-{11}\r\nCrit Chance: {12}%\r\nCast: {13}\r\nCost: {14}\r\n{15}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpCT.ToString("0.00"),
                DpM.ToString("0.00"),
                MinDamage.ToString("0"), MaxDamage.ToString("0"), AvgHit.ToString("0"),
                MinCrit.ToString("0"), MaxCrit.ToString("0"), AvgCrit.ToString("0"),
                Math.Floor(DotDamage / 15f).ToString("0"), Math.Ceiling(DotDamage / 15f).ToString("0"),
                (CritChance * 100f).ToString("0.00"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"),
                Name);
        }
    }

    //Rain of Fire: Channeled. Aoe not implemented. Check coefficient.
    public class RainOfFire : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(7, 79, 2700, 2700)
        };

        public RainOfFire(Stats stats, Character character)
            : base("Rain of Fire", stats, character, SpellRankTable, 57, 8f, 1.5f, 0f, 1.15f, 30, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Hellfire: Channeled. Damage inflicted to caster and aoe not implemented.
    public class Hellfire : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(5, 78, 6765, 6765)
        };

        public Hellfire(Stats stats, Character character)
            : base("Hellfire", stats, character, SpellRankTable, 64, 15f, 0, 0f, 15f / 2 / 3.5f, 10, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Searing Pain: Threatgain not implemented.
    public class SearingPain : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(10, 79, 343, 405)
        };

        public SearingPain(Stats stats, Character character)
            : base("Searing Pain", stats, character, SpellRankTable, 8, 1.5f, 1.5f, 0f, 1.5f / 3.5f, 30, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit;
            if (character.WarlockTalents.ImprovedSearingPain == 1) CritChance += 4f;
            else if (character.WarlockTalents.ImprovedSearingPain == 2) CritChance += 7f;
            else if (character.WarlockTalents.ImprovedSearingPain == 3) CritChance += 10f;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Soul Fire: Reagent cost not implemented. Check Coefficient
    public class SoulFire : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(6, 80, 1323, 1657)
        };

        public SoulFire(Stats stats, Character character)
            : base("Soul Fire", stats, character, SpellRankTable, 9, 6f, 1.5f, 0f, 1.15f, 30, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Bane * 0.4f));

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Conflagrate: Can only be done when Immolate of Shadowflame is on target. Will consume the dot. Both not implemented.
    public class Conflagrate : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(8, 80, 766, 945)
        };

        public Conflagrate(Stats stats, Character character)
            : base("Conflagrate", stats, character, SpellRankTable, 12, 0f, 1.5f, 0f, 1.5f / 3.5f, 30, 10f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Chaos Bolt: not resistable and absorbable still? Not implemented.
    public class ChaosBolt : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(4, 80, 1036, 1314)
        };

        public ChaosBolt(Stats stats, Character character)
            : base("Chaos Bolt", stats, character, SpellRankTable, 7, 2.5f, 1.5f, 0f, 2.5f / 3.5f, 30, 12f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit /*+ character.PriestTalents.MindMelt * 0.02f*/;

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Bane * 0.1f));

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    public class TimbalProc : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 0, 285, 475)
        };

        public TimbalProc(Stats stats, Character character)
            : base("TimbalProc", stats, character, SpellRankTable, 0, 0f, 1.5f, 0, 0f, 0, 0f, Color.Black)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = BaseMinDamage
                /* (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f)*/;

            MaxDamage = BaseMaxDamage
                /* (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f)*/;

            //CritCoef += character.PriestTalents.ShadowPower * 0.1f;
            CritChance = stats.SpellCrit;

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.ShadowReach * 0.1f)*/));
        }
    }

    public class ExtractProc : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 0, 788, 1312)
        };

        public ExtractProc(Stats stats, Character character)
            : base("ExtractProc", stats, character, SpellRankTable, 0, 0f, 1.5f, 0, 0f, 0, 0f, Color.Black)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = BaseMinDamage
                /* (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f)*/;

            MaxDamage = BaseMaxDamage
                /* (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f)*/;

            //CritCoef += character.PriestTalents.ShadowPower * 0.1f;
            CritChance = stats.SpellCrit;

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.ShadowReach * 0.1f)*/));
        }
    }

    public class PendulumProc : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 0, 1168, 1752)
        };

        public PendulumProc(Stats stats, Character character)
            : base("PendulumProc", stats, character, SpellRankTable, 0, 0f, 1.5f, 0, 0f, 0, 0f, Color.Black)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = BaseMinDamage
                /* (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f)*/;

            MaxDamage = BaseMaxDamage
                /* (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f)*/;

            //CritCoef += character.PriestTalents.ShadowPower * 0.1f;
            CritChance = stats.SpellCrit;

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.ShadowReach * 0.1f)*/));
        }
    }
}


/*
    public class VampiricTouch : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData( 1, 50, 450, 450),
            new SpellData( 2, 60, 600, 600),
            new SpellData( 3, 70, 650, 650),
            new SpellData( 4, 75, 735, 735),
            new SpellData( 5, 80, 850, 850)
        };

        public VampiricTouch(Stats stats, Character character)
            : base("Vampiric Touch", stats, character, SpellRankTable, 16, 1.5f, 0, 15f, 15f / 15f * 1.9f, 30, 0f, Color.Blue)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            if (character.PriestTalents.VampiricTouch == 0)
            {
                MinDamage = MaxDamage = 0;
                return;
            }

            MinDamage = MaxDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + (character.PriestTalents.Shadowform > 0 ? stats.SpellCrit : 0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f));

            CastTime = (float)Math.Max(1.0f, BaseCastTime / (1 + stats.SpellHaste));

            Range = (int)Math.Round(BaseRange * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class MindBlast : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData( 1, 10,  39,  43),
            new SpellData( 2, 16,  72,  78),
            new SpellData( 3, 22, 116, 120),
            new SpellData( 4, 28, 167, 177),
            new SpellData( 5, 34, 217, 231),
            new SpellData( 6, 40, 279, 297),
            new SpellData( 7, 46, 346, 366),
            new SpellData( 8, 52, 425, 449),
            new SpellData( 9, 58, 503, 531),
            new SpellData(10, 63, 557, 587),
            new SpellData(11, 69, 711, 752),
            new SpellData(12, 74, 837, 883),
            new SpellData(13, 79, 992, 1048)
        };

        public MindBlast(Stats stats, Character character)
            : base("Mind Blast", stats, character, SpellRankTable, 17, 1.5f, 1.5f, 0, 1.5f / 3.5f, 30, 8f, Color.Gold)
        {
            Calculate(stats, character);
        }
        
        public override void Calculate(Stats stats, Character character)
        {
            DamageCoef = BaseDamageCoef * (1f + character.PriestTalents.Misery * 0.05f);

            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + stats.BonusMindBlastMultiplier)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + stats.BonusMindBlastMultiplier)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            CastTime = (float)Math.Max(1.0f, BaseCastTime / (1 + stats.SpellHaste));
            Cooldown -= character.PriestTalents.ImprovedMindBlast * 0.5f;

            CritCoef = (BaseCritCoef * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.PriestTalents.ShadowPower * 0.2f) + 1f;

            CritChance = stats.SpellCrit + character.PriestTalents.MindMelt * 0.02f;
            
            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.FocusedMind * 0.05f)
                * (1f - stats.MindBlastCostReduction));

            Range = (int)Math.Round(BaseRange * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class ShadowWordDeath : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData( 1, 62, 450, 522),
            new SpellData( 2, 70, 572, 664),
            new SpellData( 3, 75, 639, 741),
            new SpellData( 4, 80, 750, 870)
        };

        public ShadowWordDeath(Stats stats, Character character)
            : base("Shadow Word: Death", stats, character, SpellRankTable, 12, 0, 1.5f, 0, 1.5f / 3.5f, 30, 12f, Color.Gold)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            CritChance = stats.SpellCrit
                + stats.ShadowWordDeathCritIncrease;

            CritCoef = (BaseCritCoef * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.PriestTalents.ShadowPower * 0.2f) + 1f;

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class MindFlay : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData( 1, 20,  45,  45),
            new SpellData( 2, 28, 108, 108),
            new SpellData( 3, 36, 159, 159),
            new SpellData( 4, 44, 222, 222),
            new SpellData( 5, 52, 282, 282),
            new SpellData( 6, 60, 363, 363),
            new SpellData( 7, 68, 450, 450),
            new SpellData( 8, 74, 492, 492),
            new SpellData( 9, 80, 588, 588)
        };

        public MindFlay(Stats stats, Character character)
            : base("Mind Flay", stats, character, SpellRankTable, 9, 3f, 1.5f, 0, 3f / 3.5f, 20, 0f, Color.LightBlue)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            if (character.PriestTalents.MindFlay == 0)
            {
                MinDamage = MaxDamage = 0;
                return;
            }

            // Coefficient penalty for snare effect.
            DamageCoef = BaseDamageCoef * 0.9f * (1f + character.PriestTalents.Misery * 0.05f);

            MinDamage = MaxDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.MentalAgility * 0.02f)
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.FocusedMind * 0.05f));

            CritChance = stats.SpellCrit + character.PriestTalents.MindMelt * 0.02f;

            CritCoef = (BaseCritCoef * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.PriestTalents.ShadowPower * 0.2f) + 1f;

            CastTime = (float)Math.Max(1.0f, BaseCastTime / (1 + stats.SpellHaste));
            Range = (int)Math.Round(BaseRange * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nTick Hit: {4}-{5}\r\nTick Crit: {6}-{7}\r\nCrit Chance: {8}%\r\nCast: {9}\r\nCost: {10}\r\n{11}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpCT.ToString("0.00"),
                          DpM.ToString("0.00"),
                          Math.Floor(AvgHit / CastTime).ToString("0"), Math.Ceiling(AvgHit / CastTime).ToString("0"),
                          Math.Floor(AvgCrit / CastTime).ToString("0"), Math.Ceiling(AvgCrit / CastTime).ToString("0"),
                          (CritChance * 100f).ToString("0.00"),
                          CastTime.ToString("0.00"),
                          ManaCost.ToString("0"),
                          Name);
        }
    }

    public class PowerWordShield : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData( 1,  6,   44,   44),
            new SpellData( 2, 12,   88,   88),
            new SpellData( 3, 18,  158,  158),
            new SpellData( 4, 24,  234,  234),
            new SpellData( 5, 30,  301,  301),
            new SpellData( 6, 36,  381,  381),
            new SpellData( 7, 42,  484,  484),
            new SpellData( 8, 48,  605,  605),
            new SpellData( 9, 54,  763,  763),
            new SpellData(10, 60,  942,  942),
            new SpellData(11, 65, 1125, 1125),
            new SpellData(12, 70, 1265, 1265),
            new SpellData(13, 75, 1920, 1920),
            new SpellData(14, 80, 2230, 2230)
        };

        public PowerWordShield(Stats stats, Character character)
            : base("Power Word: Shield", stats, character, SpellRankTable, 23, 0, 0, 0, 1.5f / 3.5f, 30, 4f, Color.Brown)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (BaseMinDamage * (1 + character.PriestTalents.ImprovedPowerWordShield * 0.05f)
                + stats.SpellPower * 1.88f * DamageCoef
                + stats.SpellPower * character.PriestTalents.BorrowedTime * 0.04f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.ImprovedPowerWordShield * 0.05f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                * (1 - character.PriestTalents.MentalAgility * 0.02f));
        }

        public override string ToString()
        {
            return String.Format("{0} *Cost: {1}\r\n{2}",
                MinDamage.ToString("0"),
                ManaCost.ToString("0"),
                Name);
        }
    }

    public class VampiricEmbrace : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 30, 0, 0)
        };
        public float HealthConvertionCoef { get; protected set; }

        public VampiricEmbrace(Stats stats, Character character)
            : base("Vampiric Embrace", stats, character, SpellRankTable, 2, 0, 0, 60f, 0, 30, 10f, Color.Green)
        {
            HealthConvertionCoef = 0.15f;
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            if (character.PriestTalents.VampiricEmbrace == 0)
            {
                HealthConvertionCoef = 0;
                return;
            }

            HealthConvertionCoef *= (1 + character.PriestTalents.ImprovedVampiricEmbrace / 3f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));
        }

        public override string ToString()
        {
            return String.Format("{0} *Cost: {1}\r\n{2}",
                (HealthConvertionCoef * 100).ToString("0") + "%",
                ManaCost.ToString("0"),
                Name);
        }
    }

    public class DevouringPlague : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 20, 152, 152),
            new SpellData(2, 28, 272, 272),
            new SpellData(3, 36, 400, 400),
            new SpellData(4, 44, 544, 544),
            new SpellData(5, 52, 712, 712),
            new SpellData(6, 60, 904, 904),
            new SpellData(7, 68, 1088, 1088),
            new SpellData(8, 73, 1144, 1144),
            new SpellData(9, 79, 1376, 1376)
        };

        public DevouringPlague(Stats stats, Character character)
            : base("Devouring Plague", stats, character, SpellRankTable, 25, 0, 0, 24, 24f / 15f * 0.925f, 30, 24, Color.Purple)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (BaseMinDamage +
                (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + (character.PriestTalents.Shadowform > 0 ? stats.SpellCrit : 0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

 * public class Smite : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData( 1,  1,  13,  17),
            new SpellData( 2,  6,  25,  31),
            new SpellData( 3, 14,  54,  62),
            new SpellData( 4, 22,  91, 104),
            new SpellData( 5, 30, 150, 170),
            new SpellData( 6, 38, 212, 240),
            new SpellData( 7, 46, 287, 323),
            new SpellData( 8, 54, 371, 415),
            new SpellData( 9, 61, 405, 453),
            new SpellData(10, 69, 549, 616),
            new SpellData(11, 74, 604, 676),
            new SpellData(12, 79, 707, 793)
        };

        public Smite(Stats stats, Character character)
            : base("Smite", stats, character, SpellRankTable, 15, 2.5f, 1.5f, 0, 2.5f / 3.5f, 30, 0f, Color.Yellow)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.PriestTalents.DivineFury * 0.1f) / (1 + stats.SpellHaste));

            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f;

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                );

            Range = (int)Math.Round(BaseRange * (1 + character.PriestTalents.HolyReach * 0.1f));
        }
    }

    public class HolyFire : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellDataDot( 1, 20,  102,  128,  21),
            new SpellDataDot( 2, 24,  137,  173,  28),
            new SpellDataDot( 3, 30,  200,  252,  42),
            new SpellDataDot( 4, 36,  267,  339,  56),
            new SpellDataDot( 5, 42,  348,  440,  70),
            new SpellDataDot( 6, 48,  430,  546,  91),
            new SpellDataDot( 7, 54,  529,  671, 112),
            new SpellDataDot( 8, 60,  639,  811, 126),
            new SpellDataDot( 9, 66,  705,  895, 147),
            new SpellDataDot(10, 72,  732,  928, 287),
            new SpellDataDot(11, 78,  890, 1130, 350)
        };

        public float BaseDotDamage { get; protected set; }
        public float DotDamage { get; protected set; }

        public override float AvgDamage
        {
            get
            {
                return AvgHit * (1f - CritChance) + AvgCrit * CritChance + DotDamage;
            }
        }

        public override float DpCT
        {
            get
            {
                return AvgDamage / CastTime;
            }
        }

        public override float DpS
        {
            get
            {
                return AvgDamage / CastTime;
            }
        }

        public HolyFire(Stats stats, Character character)
            : base("Holy Fire", stats, character, SpellRankTable, 11, 2.0f, 1.5f, 7f, 2f / 3.5f, 30, 10f, Color.Gold)
        {
            Calculate(stats, character);
            foreach (SpellData sd in SpellRankTable)
                if (character.Level > sd.Level)
                {
                    BaseDotDamage = DotDamage = ((SpellDataDot)sd).DotDamage;
                }

        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);


            DotDamage = (BaseDotDamage + stats.SpellPower * 1f / 6f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.PriestTalents.DivineFury * 0.1f) / (1 + stats.SpellHaste));

            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f;

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                );

            Range = (int)Math.Round(BaseRange * (1 + character.PriestTalents.HolyReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nHit: {4}-{5}, Avg {6}\r\nCrit: {7}-{8}, Avg {9}\r\nTick: {10}-{11}\r\nCrit Chance: {12}%\r\nCast: {13}\r\nCost: {14}\r\n{15}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpCT.ToString("0.00"),
                DpM.ToString("0.00"),
                MinDamage.ToString("0"), MaxDamage.ToString("0"), AvgHit.ToString("0"),
                MinCrit.ToString("0"), MaxCrit.ToString("0"), AvgCrit.ToString("0"),
                Math.Floor(DotDamage / 7f).ToString("0"), Math.Ceiling(DotDamage / 7f).ToString("0"),
                (CritChance * 100f).ToString("0.00"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"),
                Name);
        }
    }

    public class Penance : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 60, 552, 552),
            new SpellData(2, 70, 672, 672),
            new SpellData(3, 75, 768, 768),
            new SpellData(4, 80, 864, 864)
        };

        public Penance(Stats stats, Character character)
            : base("Penance", stats, character, SpellRankTable, 16, 2f, 1.5f, 0, 2.4f / 3.5f, 30, 10f, Color.Orange)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            if (character.PriestTalents.Penance == 0)
            {
                MinDamage = MaxDamage = 0;
                return;
            }

            MinDamage = MaxDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);


            CastTime = (float)Math.Max(1.0f, BaseCastTime / (1 + stats.SpellHaste));

            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f;

            Cooldown *= (1f - character.PriestTalents.Aspiration * 0.1f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ImprovedHealing * 0.05f));

            Range = (int)Math.Round(BaseRange * (1 + character.PriestTalents.HolyReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nTick Hit: {4}-{5}, Total {6}\r\nTick Crit: {7}-{8}, Total {9}\r\nCrit Chance: {10}%\r\nCast: {11}\r\nCost: {12}\r\n{13}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpCT.ToString("0.00"),
                DpM.ToString("0.00"),
                Math.Floor(MinDamage/3).ToString("0"), Math.Ceiling(MinDamage/3).ToString("0"), AvgHit.ToString("0"),
                Math.Floor(MinCrit/3).ToString("0"), Math.Ceiling(MinCrit/3).ToString("0"), AvgCrit.ToString("0"),
                (CritChance * 100f).ToString("0.00"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"),
                Name);
        }
    }
}
*/

/*using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
    enum MagicSchool
    {
        Arcane,
        Fire,
        Frost,
        Shadow,
        Nature
    }

    enum SpellTree
    {
        Affliction, 
        Destruction,
        Demonology
    }

    internal abstract class Spell
    {
        //base stats
        public string Name { get; set; }
        public MagicSchool MagicSchool { get; set; }
        public SpellTree SpellTree { get; set; }
        public float BaseMinDamage { get; set; }
        public float BaseMaxDamage { get; set; }
        public float BaseCastTime { get; set; }
        public float BasePeriodicDamage { get; set; }
        public float BaseDotDuration { get; set; }
        public float BaseManaCost { get; set; }
        public float DirectDamageCoefficient { get; set; }
        public float DotDamageCoefficient { get; set; }

        public Spell(string name, MagicSchool magicSchool, SpellTree spellTree, float minDamage, float maxDamage, float periodicDamage, float dotDuration, float castTime, float manaCost)
        {
            Name = name;
            MagicSchool = magicSchool;
            SpellTree = spellTree;
            BaseMinDamage = minDamage;
            BaseMaxDamage = maxDamage;
            BasePeriodicDamage = periodicDamage;
            BaseDotDuration = dotDuration;
            BaseCastTime = castTime;
            BaseManaCost = manaCost;
            DirectDamageCoefficient = BaseCastTime / 3.5f;
            DotDamageCoefficient = BaseDotDuration / 15;
            CritBonus = 1;
            BonusMultiplier = 1;
        }

        //derived stats
        public float CritRate { get; set; }
        public float CritBonus { get; set; }
        public float ChanceToHit { get; set; }
        public float Damage { get; set; }
        public float Frequency { get; set; }
        public float CastRatio { get; set; }
        public float CastTime { get; set; }
        public float ManaPerSecond { get; set; }
        public float ManaCost { get; set; }
        public float BonusMultiplier { get; set; }
        //public float HealthPerSecond { get; set; }

        public void CalculateDerivedStats(CharacterCalculationsWarlock calculations)
        {
            //hit rate
            ChanceToHit = CalculationsWarlock.ChanceToHit(calculations.CalculationOptions.TargetLevel, calculations.HitPercent);
            if (SpellTree == SpellTree.Affliction)
                ChanceToHit = Math.Min(1, ChanceToHit + 0.1f * calculations.CalculationOptions.Suppression);
            if (SpellTree == SpellTree.Destruction)
                ChanceToHit = Math.Min(1, ChanceToHit + 0.1f * calculations.CalculationOptions.Cataclysm);

            //cast time
            CastTime = BaseCastTime / (1 + 0.01f * calculations.HastePercent);
            if (calculations.BasicStats.Bloodlust > 0)
//600 should be FightLength
                CastTime /= (1 + 0.3f * 40 / 600);
            CastTime += calculations.CalculationOptions.Latency;
            if (CastTime < calculations.GlobalCooldown + calculations.CalculationOptions.Latency)
                CastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;
            //for DoTs, factor in the chance to miss (because you have to re-apply)
            if (BaseDotDuration != 0)
                CastTime /= ChanceToHit;
            if (CastTime < calculations.GlobalCooldown + calculations.CalculationOptions.Latency)
                CastTime = calculations.GlobalCooldown + calculations.CalculationOptions.Latency;

            //frequency
            if (BaseDotDuration == 0)
                Frequency = CastTime;
            else
                Frequency = BaseDotDuration + CastTime + calculations.CalculationOptions.DotGap - (BaseCastTime + calculations.CalculationOptions.Latency);

            //mana cost
            ManaCost = BaseManaCost;
            if (SpellTree == SpellTree.Destruction)
                ManaCost *= (1 - 0.01f * calculations.CalculationOptions.Cataclysm);
            if (SpellTree == SpellTree.Affliction)
                ManaCost *= (1 - 0.02f * calculations.CalculationOptions.Suppression);
            if (BaseDotDuration != 0)
                ManaCost /= ChanceToHit;
            ManaCost = (float)Math.Round(ManaCost);
            ManaPerSecond = ManaCost / Frequency;

            //cast ratio
            CastRatio = CastTime / Frequency;
        }

        public void CalculateDamage(CharacterCalculationsWarlock calculations)
        {
            if (calculations.BasicStats.BonusSpellCritMultiplier > 0)
                CritBonus = 1.09f;
            CritBonus *= 0.5f + 0.1f * calculations.CalculationOptions.Ruin;

            float plusDamage = 0;
            switch(MagicSchool)
            {
                case MagicSchool.Shadow:
                    plusDamage = calculations.ShadowDamage;
                    BonusMultiplier *= calculations.BasicStats.BonusSpellPowerMultiplier * calculations.BasicStats.BonusShadowDamageMultiplier;
                    break;
                case MagicSchool.Fire:
                    plusDamage = calculations.FireDamage;
                    BonusMultiplier *= calculations.BasicStats.BonusSpellPowerMultiplier * calculations.BasicStats.BonusFireDamageMultiplier;
                    break;
            }

            float averageDamage = (BaseMinDamage + BaseMaxDamage) / 2;
            float dotDamage = BasePeriodicDamage;
            if (averageDamage > 0 && BasePeriodicDamage > 0)
            {
                DirectDamageCoefficient *= averageDamage / (averageDamage + dotDamage);
                DotDamageCoefficient *= dotDamage / (averageDamage + dotDamage);
            }

            if (averageDamage != 0)
            {
                averageDamage += plusDamage * DirectDamageCoefficient;
                averageDamage *= 1 + 0.01f * calculations.CritPercent * CritBonus;
                if (BaseDotDuration == 0)
                    averageDamage *= ChanceToHit;
                else if (this is Immolate)
                    averageDamage *= 1 + 0.1f * calculations.CalculationOptions.ImprovedImmolate;
            }

            if (dotDamage != 0)
                dotDamage += plusDamage * DotDamageCoefficient;

            Damage = (float)Math.Round((averageDamage + dotDamage) * BonusMultiplier);
        }
    }

//to do: ISB:  * (1 + 0.1f * calculations.IsbUptime)
    internal class ShadowBolt : Spell
    {
        public ShadowBolt(CharacterCalculationsWarlock calculations)
            : base("Shadow Bolt", MagicSchool.Shadow, SpellTree.Destruction, 690, 770, 0, 0, 3, 1008)
        {
            BaseCastTime -= 0.1f * calculations.CalculationOptions.Bane;
            DirectDamageCoefficient += 0.04f * calculations.CalculationOptions.ShadowAndFlame;
            BonusMultiplier *= 1 + calculations.BasicStats.BonusWarlockNukeMultiplier;
        }
    }

    internal class Incinerate : Spell
    {
        public Incinerate(CharacterCalculationsWarlock calculations)
            : base("Incinerate", MagicSchool.Fire, SpellTree.Destruction, 727, 845, 0, 0, 2.5f, 830)
        {
            BaseCastTime -= 0.05f * calculations.CalculationOptions.Emberstorm;
            DirectDamageCoefficient += 0.04f * calculations.CalculationOptions.ShadowAndFlame;
            BonusMultiplier *= 1 + calculations.BasicStats.BonusWarlockNukeMultiplier;
        }
    }

    internal class Immolate : Spell
    {
        public Immolate(CharacterCalculationsWarlock calculations)
            : base("Immolate", MagicSchool.Fire, SpellTree.Destruction, 460, 460, 785, 15, 2, 1008)
        {
            BaseCastTime -= 0.1f * calculations.CalculationOptions.Bane;
            BaseDotDuration += calculations.BasicStats.BonusWarlockDotExtension;
        }
    }
    internal class CurseOfAgony : Spell
    {
        public CurseOfAgony(CharacterCalculationsWarlock calculations)
            : base("Curse of Agony", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 1740, 24, 1.5f, 593)
        {
            BonusMultiplier += 0.05f * calculations.CalculationOptions.ImprovedCurseOfAgony;
            BonusMultiplier += 0.01f * calculations.CalculationOptions.Contagion;
            DotDamageCoefficient = 1.2f;
            BaseCastTime -= 0.5f * calculations.CalculationOptions.AmplifyCurse;
        }
    }

    internal class CurseOfDoom : Spell
    {
        public CurseOfDoom(CharacterCalculationsWarlock calculations)
            : base("Curse of Doom", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 7300, 60, 1.5f, 890)
        {
            DotDamageCoefficient = 2f;
            BaseCastTime -= 0.5f * calculations.CalculationOptions.AmplifyCurse;
        }
    }

    internal class CurseOfRecklessness : Spell
    {
        public CurseOfRecklessness(CharacterCalculationsWarlock calculations)
            : base("Curse of Recklessness", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 120, 1.5f, 356)
        {
            BaseCastTime -= 0.5f * calculations.CalculationOptions.AmplifyCurse;
        }
    }

    internal class CurseOfTheElements : Spell
    {
        public CurseOfTheElements(CharacterCalculationsWarlock calculations)
            : base("Curse of the Elements", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 300, 1.5f, 593)
        {
            BaseCastTime -= 0.5f * calculations.CalculationOptions.AmplifyCurse;
        }
    }

    internal class CurseOfWeakness : Spell
    {
        public CurseOfWeakness(CharacterCalculationsWarlock calculations)
            : base("Curse of Weakness", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 120, 1.5f, 593)
        {
            BaseCastTime -= 0.5f * calculations.CalculationOptions.AmplifyCurse;
        }
    }

    internal class CurseOfTongues : Spell
    {
        public CurseOfTongues(CharacterCalculationsWarlock calculations)
            : base("Curse of Tongues", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 30, 1.5f, 237)
        {
            BaseCastTime -= 0.5f * calculations.CalculationOptions.AmplifyCurse;
        }
    }

    internal class Corruption : Spell
    {
        public Corruption(CharacterCalculationsWarlock calculations)
            : base("Corruption", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 1080, 18, 1.5f, 830)
        {
            BaseDotDuration += calculations.BasicStats.BonusWarlockDotExtension;
            BonusMultiplier += 0.02f * calculations.CalculationOptions.ImprovedCorruption;
            BonusMultiplier += 0.01f * calculations.CalculationOptions.Contagion;
            DotDamageCoefficient = 1.2f + 0.12f * calculations.CalculationOptions.EmpoweredCorruption;
        }
    }

    internal class SiphonLife : Spell
    {
        public SiphonLife(CharacterCalculationsWarlock calculations)
            : base("Siphon Life", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 810, 30, 1.5f, 949)
        {
            DotDamageCoefficient = 1;
        }
    }

    internal class UnstableAffliction : Spell
    {
        public UnstableAffliction(CharacterCalculationsWarlock calculations)
            : base("Unstable Affliction", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 1150, 15, 1.5f, 890)
        {
        }
    }

    internal class LifeTap : Spell
    {
        public LifeTap(CharacterCalculationsWarlock calculations)
            : base("Life Tap", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 0, 1.5f, -1490)
        {
            BaseManaCost -= calculations.BasicStats.Spirit * 3;
            BaseManaCost *= 1 + 0.1f * calculations.CalculationOptions.ImprovedLifeTap;
        }
    }

    internal class DarkPact : Spell
    {
        public DarkPact(CharacterCalculationsWarlock calculations)
            : base("Dark Pact", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 0, 1.5f, -1200)
        {
            BaseManaCost -= calculations.ShadowDamage * 0.96f;
        }
    }

    internal class DeathCoil : Spell
    {
        public DeathCoil(CharacterCalculationsWarlock calculations)
            : base("Death Coil", MagicSchool.Shadow, SpellTree.Affliction, 790, 790, 0, 0, 1.5f, 1364)
        {
        }
    }

    internal class DrainLife : Spell
    {
        public DrainLife(CharacterCalculationsWarlock calculations)
            : base("Drain Life", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 665, 5, 5, 1008)
        {
//            BonusMultiplier += (0.02f * calculations.CalculationOptions.SoulSiphon) * CharacterCalculationsWarlock.AfflictionDebuffs;
        }
    }

    //to do: add mana drained
    internal class DrainMana : Spell
    {
        public DrainMana(CharacterCalculationsWarlock calculations)
            : base("Drain Mana", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 0, 0, 5, 1008)
        {
        }
    }

    //to do: add 4* dmg when target is at or below 25%
    internal class DrainSoul : Spell
    {
        public DrainSoul(CharacterCalculationsWarlock calculations)
            : base("Drain Soul", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 710, 15, 15, 830)
        {
//            BonusMultiplier += (0.02f * calculations.CalculationOptions.SoulSiphon) * CharacterCalculationsWarlock.AfflictionDebuffs;
        }
    }

    //to do: add 20% increased DoT dmg for 20 secs
    internal class Haunt : Spell
    {
        public Haunt(CharacterCalculationsWarlock calculations)
            : base("Haunt", MagicSchool.Shadow, SpellTree.Affliction, 645, 753, 0, 0, 1.5f, 712)
        {
        }
    }

    //to do: add 1633 to 1897 dmg to all other enemies on DoT end
    internal class SeedOfCorruption : Spell
    {
        public SeedOfCorruption(CharacterCalculationsWarlock calculations)
            : base("Seed of Corruption", MagicSchool.Shadow, SpellTree.Affliction, 0, 0, 1518, 18, 2, 2016)
        {
//            CritMultiplier += 0.01f * calculations.CalculationOptions.ImprovedCorruption;
        }
    }

    //to do: AoE
    internal class Hellfire : Spell
    {
        public Hellfire(CharacterCalculationsWarlock calculations)
            : base("Hellfire", MagicSchool.Fire, SpellTree.Destruction, 0, 0, 6765, 15, 15, 3796)
        {
        }
    }

    //to do: AoE
    internal class RainOfFire : Spell
    {
        public RainOfFire(CharacterCalculationsWarlock calculations)
            : base("Rain of Fire", MagicSchool.Fire, SpellTree.Destruction, 0, 0, 2700, 8, 8, 3381)
        {
            BaseManaCost -= calculations.ShadowDamage * 0.8f;
            BaseManaCost *= (1 + 0.1f * calculations.CalculationOptions.ImprovedLifeTap);
        }
    }

    internal class SearingPain : Spell
    {
        public SearingPain(CharacterCalculationsWarlock calculations)
            : base("Searing Pain", MagicSchool.Fire, SpellTree.Destruction, 343, 405, 0, 0, 1.5f, 474)
        {
        }
    }

    //to do: add AoE
    internal class Shadowflame : Spell
    {
        public Shadowflame(CharacterCalculationsWarlock calculations)
            : base("Shadowflame", MagicSchool.Shadow, SpellTree.Destruction, 615, 671, 644, 8, 1.5f, 1483)
        {
        }
    }

    internal class SoulFire : Spell
    {
        public SoulFire(CharacterCalculationsWarlock calculations)
            : base("Soul Fire", MagicSchool.Fire, SpellTree.Destruction, 1323, 1657, 0, 0, 6, 534)
        {
        }
    }

    //to do: add CD
    internal class Shadowburn : Spell
    {
        public Shadowburn(CharacterCalculationsWarlock calculations)
            : base("Shadowburn", MagicSchool.Shadow, SpellTree.Destruction, 775, 865, 0, 0, 1.5f, 1186)
        {
        }
    }

    //to do: add CD
    internal class Conflagrate : Spell
    {
        public Conflagrate(CharacterCalculationsWarlock calculations)
            : base("Conflagrate", MagicSchool.Fire, SpellTree.Destruction, 766, 954, 0, 0, 1.5f, 712)
        {
        }
    }

    //to do: add AoE
    internal class Shadowfury : Spell
    {
        public Shadowfury(CharacterCalculationsWarlock calculations)
            : base("Shadowfury", MagicSchool.Shadow, SpellTree.Destruction, 968, 1152, 0, 0, 1.5f, 1601)
        {
        }
    }

    //to do: add CD
    internal class ChaosBolt : Spell
    {
        public ChaosBolt(CharacterCalculationsWarlock calculations)
            : base("Chaos Bolt", MagicSchool.Fire, SpellTree.Destruction, 1036, 1314, 0, 0, 2.5f, 415)
        {
        }
    }
}
*/