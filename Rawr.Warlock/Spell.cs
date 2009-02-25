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
                case "Life Tap":
                      return new LifeTap(stats, character);
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
        public float TickCount { get; set; }
        public float HitChance { get; set; }
        public double CooldownReset { get; set; }
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
            public int DotDamage { get; protected set; }

            public SpellData(int rank, int level, int minDamage, int maxDamage, int dotDamage)
            {
                Rank = rank;
                Level = level;
                MinDamage = minDamage;
                MaxDamage = maxDamage;
                DotDamage = dotDamage;
            }
        }

        public static readonly List<string> SpellList = new List<string>() { "Shadow Bolt", "Curse of Agony", "Curse of Doom", "Corruption", "Siphon Life", "Unstable Affliction", "Death Coil", "Drain Life", "Drain Soul", "Haunt", "Seed of Corruption", "Shadowflame", "Shadowburn", "Shadowfury", "Incinerate", "Immolate", "Rain of Fire", "Hellfire", "Searing Pain", "Soul Fire", "Conflagrate", "Chaos Bolt" };

        public string Name { get; protected set; }

        public MagicSchool MagicSchool { get; protected set; }
        public SpellTree SpellTree { get; protected set; }
        public int Rank { get; protected set; }
        public float BaseMinDamage { get; protected set; }
        public float MinDamage { get; protected set; }
        public float BaseMaxDamage { get; protected set; }
        public float MaxDamage { get; protected set; }
        public float MinBuffedDamage { get; protected set; }
        public float MaxBuffedDamage { get; protected set; }
        public float BaseDotDamage { get; protected set; }
        public float DotDamage { get; protected set; }
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
        public float TimeBetweenTicks { get; protected set; }
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

        public virtual float AvgDirectDamage
        {
            get
            {
                return AvgHit * (1f - CritChance) + AvgCrit * CritChance;
            }
        }
        
        public virtual float AvgBuffedDamage
        {
            get
            {
                return ((MinBuffedDamage + MaxBuffedDamage) / 2) * (1f - CritChance) + ((MinBuffedDamage * CritCoef + MaxBuffedDamage * CritCoef) / 2) * CritChance;
            }
        }

        public virtual float AvgDotDamage
        {
            get
            {
                return DotDamage / (DebuffDuration / TimeBetweenTicks);
            }
        }

        public virtual float AvgDamage
        {
            get
            {
                return AvgHit * (1f - CritChance) + AvgCrit * CritChance + DotDamage;
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
                    return AvgDamage / (DebuffDuration + CastTime);
                else if (CastTime > 0)
                    return AvgDamage / CastTime;
                else return AvgDamage / GlobalCooldown;
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
                    BaseDotDamage = DotDamage = sd.DotDamage;
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
            Cooldown = cooldown;
            SpellStatistics = new SpellStatistics();
            if (character.WarlockTalents.AmplifyCurse > 0 && Name.StartsWith("Curse of"))
                GlobalCooldown = (float)Math.Max(0.5f, 1.0f / (1 + stats.SpellHaste));
            else GlobalCooldown = (float)Math.Max(1.0f, 1.5f / (1 + stats.SpellHaste));
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
            if (DebuffDuration > 0f && CritChance > 0f)
                return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nHit: {4}-{5}, Avg {6}\r\nCrit: {7}-{8}, Avg {9}\r\nTick: {10}-{11}\r\nCrit Chance: {12}%\r\nCast: {13}\r\nCost: {14}\r\n{15}",
                    AvgDamage.ToString("0"),
                    DpS.ToString("0.00"),
                    DpCT.ToString("0.00"),
                    DpM.ToString("0.00"),
                    MinDamage.ToString("0"), MaxDamage.ToString("0"), AvgHit.ToString("0"),
                    MinCrit.ToString("0"), MaxCrit.ToString("0"), AvgCrit.ToString("0"),
                    Math.Floor(DotDamage / DebuffDuration * TimeBetweenTicks).ToString("0"), Math.Ceiling(DotDamage / DebuffDuration * TimeBetweenTicks).ToString("0"),
                    (CritChance * 100f).ToString("0.00"),
                    CastTime.ToString("0.00"),
                    ManaCost.ToString("0"),
                    Name);
            if (DebuffDuration > 0f)
                return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nTick: {4}-{5}\r\nDuration: {6}s\r\nCost: {7}\r\n{8}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpCT.ToString("0.00"),
                          DpM.ToString("0.00"),
                          Math.Floor(AvgDamage / DebuffDuration * TimeBetweenTicks).ToString("0"),
                          Math.Ceiling(AvgDamage / DebuffDuration * TimeBetweenTicks).ToString("0"),
                          DebuffDuration.ToString("0"),
                          ManaCost.ToString("0"),
                          Name);
            else if (CritChance > 0f)
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
            else return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nHit: {4}-{5}, Avg {6}\r\nCast: {7}\r\nCost: {8}\r\n{9}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpCT.ToString("0.00"),
                DpM.ToString("0.00"),
                MinDamage.ToString("0"), MaxDamage.ToString("0"), AvgHit.ToString("0"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"),
                Name);
        }
    }

    public class ShadowBolt : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(13, 79, 690, 770, 0)
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

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Bane * 0.1f));
        }
    }

    //Incinerate: 582-676 dmg + 145-169 if Immolate is up.
    public class Incinerate : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(4, 80, 582, 676, 0)
        };

        public Incinerate(Stats stats, Character character)
            : base("Incinerate", stats, character, SpellRankTable, 14, 2.5f, 1.5f, 0f, 2.5f / 3.5f, 30, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MinBuffedDamage = (BaseMinDamage + 145 + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxBuffedDamage = (BaseMaxDamage + 169 + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Emberstorm * 0.05f));
        }
    }

    //Curse of Agony: avg calculated. increasing damageticks not implemented.Only one Curse per Warlock can be active on any one target.
    public class CurseOfAgony : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(9, 79, 0, 0, 1740)
        };

        public CurseOfAgony(Stats stats, Character character)
            : base("Curse of Agony", stats, character, SpellRankTable, 10, 0f, 0, 24f, 1.2f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                      * (1 + (character.WarlockTalents.ImprovedCurseOfAgony * 0.05f + character.WarlockTalents.ShadowMastery * 0.02f + character.WarlockTalents.Contagion * 0.01f))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
            
            TimeBetweenTicks = 2f;
        }
    }

    //Curse of Doom: does 7300dmg after 1 min. Only one Curse per Warlock can be active on any one target.
    public class CurseOfDoom : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(3, 80, 0, 0, 7300)
        };

        public CurseOfDoom(Stats stats, Character character)
            : base("Curse of Doom", stats, character, SpellRankTable, 15, 0f, 0, 60f, 2f, 30, 60f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                //does not benefit from ShadowMastery atm
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));

            TimeBetweenTicks = 60f;
        }
    }

    //Haunt does not refresh but add 15 extra sec to this.
    public class Corruption : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(10, 77, 0, 0, 1080)
        };

        public Corruption(Stats stats, Character character)
            : base("Corruption", stats, character, SpellRankTable, 14, 0f, 0, 18f, 1.2f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EmpoweredCorruption * 0.12f + character.WarlockTalents.EverlastingAffliction * 0.01f * 6))
                      * (1 + (character.WarlockTalents.ImprovedCorruption * 0.02f + character.WarlockTalents.ShadowMastery * 0.03f + character.WarlockTalents.Contagion * 0.01f))
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));

            TimeBetweenTicks = 3f;
        }
    }

    //Siphon Life: Transfers 81 health every 3 sec to the caster. Only dmg done implemented.
    public class SiphonLife : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(8, 80, 0, 0, 810)
        };

        public SiphonLife(Stats stats, Character character)
            : base("Siphon Life", stats, character, SpellRankTable, 16, 0f, 0, 30f, 1f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EverlastingAffliction * 0.01f * 10))
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));

            TimeBetweenTicks = 3f;
        }
    }

    //Unstable Affliction: if dispelled, deals 2070 dmg to dispeller. Not calculated.
    public class UnstableAffliction : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(5, 80, 0, 0, 1150)
        };

        public UnstableAffliction(Stats stats, Character character)
            : base("Unstable Affliction", stats, character, SpellRankTable, 15, 1.5f, 0, 15f, 1f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * (DamageCoef + character.WarlockTalents.EverlastingAffliction * 0.01f * 5))
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));

            TimeBetweenTicks = 3f;
        }
    }

    //Death Coil: Causes 790 dmg, Caster gains 100% of dmg done in health. Only dmg done implemented.
    public class DeathCoil : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(6, 78, 790, 790, 0)
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
            new SpellData(9, 78, 0, 0, 665)
        };

        public DrainLife(Stats stats, Character character)
            : base("Drain Life", stats, character, SpellRankTable, 17, 5f, 0, 0f, 5f / 2 / 3.5f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));

            TimeBetweenTicks = 1f / (1 + stats.SpellHaste);
        }
    }

    //Drain Soul: Shard gained not implemented.
    public class DrainSoul : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(6, 77, 0, 0, 710)
        };

        public DrainSoul(Stats stats, Character character)
            : base("Drain Soul", stats, character, SpellRankTable, 14, 15f, 0, 15f, 5f / 2 / 3.5f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));

            TimeBetweenTicks = 3f / (1 + stats.SpellHaste);
        }
    }

    //Haunt:  increases all dmg over time effects on target by 20% for 12 sec + returns 100% of dmg to caster. dotdmg increase and health gained not implemented.
    public class Haunt : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(4, 80, 645, 753, 0)
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

            CritChance = stats.SpellCrit * 0.05f + character.WarlockTalents.Backlash * 0.01f;

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));
        }
    }
    //Seed of Corruption: Aoe effect not added. Shorter duration not added. Only one Corruption spell per Warlock can be active on any one target.
    public class SeedOfCorruption : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(3, 80, 0, 0, 1518)
        };

        public SeedOfCorruption(Stats stats, Character character)
            : base("Seed of Corruption", stats, character, SpellRankTable, 34, 2f, 1.5f, 18f, 1.5f, 30, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                       * (1 + character.WarlockTalents.ShadowMastery * 0.02f + character.WarlockTalents.Contagion * 0.01f)
                       * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Suppression * 0.02f));

            CritChance = stats.SpellCrit + character.WarlockTalents.ImprovedCorruption * 0.01f + character.WarlockTalents.Backlash * 0.01f;

            Range = (int)Math.Round(BaseRange * (1 + character.WarlockTalents.GrimReach * 0.1));

            TimeBetweenTicks = 3f;
        }
    }

    //Shadowflame: aoe not implemented.
    public class Shadowflame : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
           new SpellData(2, 80,  615, 671, 644)
        };

        public Shadowflame(Stats stats, Character character)
            : base("Shadowflame", stats, character, SpellRankTable, 2, 0f, 1.5f, 8f, 1.5f / 3.5f, 10, 15f, Color.Gold, MagicSchool.Shadow, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            float DirDamageCoef = DamageCoef * (BaseMinDamage + BaseMaxDamage) / 2 / (BaseMinDamage + BaseMaxDamage + DotDamage);
            float DotDamageCoef = DebuffDuration / 15 * DotDamage / (BaseMinDamage + BaseMaxDamage + DotDamage);
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DirDamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DirDamageCoef)
                      * (1 + character.WarlockTalents.ShadowMastery * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DotDamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.HolyReach * 0.1f*/));

            TimeBetweenTicks = 2f;
        }
    }

    //Shadowburn: Soulshard if target dies not implemented.
    public class Shadowburn : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(10, 80, 775, 865, 0)
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

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }
    //Shadowfury: does not trigger gcd, check needed. aoe-effect not implemented.
    public class Shadowfury : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(5, 80, 968, 1152, 0)
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

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    public class Immolate : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
           new SpellData(11, 80,  460, 460, 785)
        };

        public Immolate(Stats stats, Character character)
            : base("Immolate", stats, character, SpellRankTable, 17, 2f, 1.5f, 15f, 2f / 3.5f, 30, 0f, Color.Gold, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            //float DirDamageCoef = DamageCoef * BaseMinDamage / (BaseMinDamage + DotDamage);
            //float DotDamageCoef = DebuffDuration / 15 * DotDamage / (BaseMinDamage + DotDamage);
            float DirDamageCoef = 0.2f;
            float DotDamageCoef = 1f;
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DirDamageCoef + character.WarlockTalents.ImprovedImmolate * 0.1f ))
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f + character.WarlockTalents.FireAndBrimstone * 0.03f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DirDamageCoef + character.WarlockTalents.ImprovedImmolate * 0.1f ))
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f + character.WarlockTalents.FireAndBrimstone * 0.03f);

            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DotDamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f + character.WarlockTalents.FireAndBrimstone * 0.03f);

            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Bane * 0.1f));

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.HolyReach * 0.1f*/));

            TimeBetweenTicks = 3f;
        }
    }

    //Rain of Fire: Channeled. Aoe not implemented. Check coefficient.
    public class RainOfFire : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(7, 79, 0, 0, 2700)
        };

        public RainOfFire(Stats stats, Character character)
            : base("Rain of Fire", stats, character, SpellRankTable, 57, 8f, 1.5f, 0f, 1.15f, 30, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));

            TimeBetweenTicks = 1f;
        }
    }

    //Hellfire: Channeled. Damage inflicted to caster and aoe not implemented.
    public class Hellfire : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(5, 78, 0, 0, 6765)
        };

        public Hellfire(Stats stats, Character character)
            : base("Hellfire", stats, character, SpellRankTable, 64, 15f, 0, 0f, 15f / 2 / 3.5f, 10, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            DotDamage = (BaseDotDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));

            TimeBetweenTicks = 1f;
        }
    }

    //Searing Pain: Threatgain not implemented.
    public class SearingPain : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(10, 79, 343, 405, 0)
        };

        public SearingPain(Stats stats, Character character)
            : base("Searing Pain", stats, character, SpellRankTable, 8, 1.5f, 1.5f, 0f, 1.5f / 3.5f, 30, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            if (character.WarlockTalents.ImprovedSearingPain == 1) CritChance += 0.04f;
            else if (character.WarlockTalents.ImprovedSearingPain == 2) CritChance += 0.07f;
            else if (character.WarlockTalents.ImprovedSearingPain == 3) CritChance += 0.1f;
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Soul Fire: Reagent cost not implemented. Check Coefficient
    public class SoulFire : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(6, 80, 1323, 1657, 0)
        };

        public SoulFire(Stats stats, Character character)
            : base("Soul Fire", stats, character, SpellRankTable, 9, 6f, 1.5f, 0f, 1.15f, 30, 0f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Bane * 0.4f));

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Conflagrate: Can only be done when Immolate of Shadowflame is on target. Will consume the dot. Both not implemented.
    public class Conflagrate : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(8, 80, 766, 945, 0)
        };

        public Conflagrate(Stats stats, Character character)
            : base("Conflagrate", stats, character, SpellRankTable, 12, 0f, 1.5f, 0f, 1.5f / 3.5f, 30, 10f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * DamageCoef)
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    //Chaos Bolt: not resistable and absorbable still? Not implemented.
    public class ChaosBolt : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(4, 80, 1036, 1314, 0)
        };

        public ChaosBolt(Stats stats, Character character)
            : base("Chaos Bolt", stats, character, SpellRankTable, 7, 2.5f, 1.5f, 0f, 2.5f / 3.5f, 30, 12f, Color.Red, MagicSchool.Fire, SpellTree.Destruction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = (BaseMinDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            MaxDamage = (BaseMaxDamage + (stats.SpellPower + stats.SpellFireDamageRating) * (DamageCoef + character.WarlockTalents.ShadowAndFlame * 0.4f ))
                      * (1 + character.WarlockTalents.Emberstorm * 0.02f)
                      * (1 + character.WarlockTalents.Malediction * 0.01f);

            ManaCost = (int)Math.Floor(BaseManaCost / 100f * BaseMana
                     * (1 - character.WarlockTalents.Cataclysm * 0.01f));

            CritChance = stats.SpellCrit + character.WarlockTalents.Devastation * 0.05f + character.WarlockTalents.Backlash * 0.01f;
            CritCoef = BaseCritCoef * (1f + stats.BonusSpellCritMultiplier)
                     + character.WarlockTalents.Ruin * 0.1f;

            CastTime = (float)Math.Max(1.0f, (BaseCastTime - character.WarlockTalents.Bane * 0.1f));

            Range = (int)Math.Round(BaseRange * (1/* + character.PriestTalents.ShadowReach * 0.1f*/));
        }
    }

    public class LifeTap : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(8, 80, 0, 0, 0)
        };

        public LifeTap(Stats stats, Character character)
            : base("Life Tap", stats, character, SpellRankTable, 0, 0f, 0f, 0f, 0f, 0, 0f, Color.Red, MagicSchool.Shadow, SpellTree.Affliction)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            ManaCost = (int)Math.Floor((-1490 - stats.Spirit * 3)
                     * (1 + character.WarlockTalents.ImprovedLifeTap * 0.1f));
        }
    }

    public class TimbalProc : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 0, 285, 475, 0)
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
            new SpellData(1, 0, 788, 1312, 0)
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
            new SpellData(1, 0, 1168, 1752, 0)
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

    public class LightweaveProc : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 0, 1000, 1200, 0)
        };

        public LightweaveProc(Stats stats, Character character)
            : base("LightweaveProc", stats, character, SpellRankTable, 0, 0f, 1.5f, 0, 0f, 0, 0f, Color.Black)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = BaseMinDamage;

            MaxDamage = BaseMaxDamage;

            CritCoef += 0;
            CritChance = stats.SpellCrit;

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.ShadowReach * 0.1f)*/));
        }
    }

    public class FirestoneProc : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 0, 116, 174, 0)
        };

        public FirestoneProc(Stats stats, Character character)
            : base("FirestoneProc", stats, character, SpellRankTable, 0, 0f, 1.5f, 0, 0f, 0, 0f, Color.Black)
        {
            Calculate(stats, character);
        }

        public override void Calculate(Stats stats, Character character)
        {
            MinDamage = BaseMinDamage;
            MaxDamage = BaseMaxDamage;

            CritCoef += 0;
            CritChance = stats.SpellCrit;

            Range = (int)Math.Round(BaseRange * (1 /*+ character.PriestTalents.ShadowReach * 0.1f)*/));
        }
    }
}