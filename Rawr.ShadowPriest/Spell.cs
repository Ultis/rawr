using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Rawr.ShadowPriest
{
    public enum MagicSchool
    {
        Fire = 2,
        Nature,
        Frost,
        Shadow,
        Arcane
    }

    public static class SpellFactory
    {
        public static Spell CreateSpell(string name, Stats stats, Character character)
        {
            switch (name)
            {
                case "Vampiric Embrace":
                    return new VampiricEmbrace(stats, character);
                case "Vampiric Touch":
                    return new VampiricTouch(stats, character);
                case "Shadow Word: Pain":
                    return new ShadowWordPain(stats, character);
                case "Devouring Plague":
                    return new DevouringPlague(stats, character);
                case "Mind Blast":
                    return new MindBlast(stats, character);
                case "Shadow Word: Death":
                    return new ShadowWordDeath(stats, character);
                case "Mind Flay":
                    return new MindFlay(stats, character);
                case "Smite":
                    return new Smite(stats, character);
                case "Holy Fire":
                    return new HolyFire(stats, character);
                case "Penance":
                    return new Penance(stats, character);
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

        public static readonly List<string> ShadowSpellList = new List<string>() { "Vampiric Embrace", "Vampiric Touch", "Mind Blast", "Devouring Plague", "Shadow Word: Pain", "Shadow Word: Death", "Mind Flay" };
        public static readonly List<string> HolySpellList = new List<string>() { "Penance", "Holy Fire", "Devouring Plague", "Shadow Word: Pain", "Mind Blast", "Shadow Word: Death", "Smite" };

        public string Name { get; protected set; }

        public MagicSchool MagicSchool { get; protected set; }
        public int Rank { get; protected set; }
        public float MinDamage { get; protected set; }
        public float MaxDamage { get; protected set; }
        public float DamageCoef { get; protected set; }
        public int Range { get; protected set; }
        public int ManaCost { get; protected set; }
        public float CastTime { get; protected set; }
        public float CritChance { get; protected set; }
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

        public Spell(string name, Stats stats, Character character, List<SpellData> SpellRankTable, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, float cooldown, Color col, MagicSchool magicSchool)
        {
            DamageCoef = damageCoef;
            DebuffDuration = dotDuration;
            GraphColor = col;
            foreach (SpellData sd in SpellRankTable)
                if (character.Level >= sd.Level)
                {
                    Rank = sd.Rank;
                    MinDamage = sd.MinDamage;
                    MaxDamage = sd.MaxDamage;
                }
            //Name = string.Format("{0}, Rank {1}", name, Rank);
            Name = name;
            ManaCost = manaCost;
            CastTime = castTime;
            DebuffDuration = dotDuration;
            DamageCoef = damageCoef;
            Range = range;
            CritChance = 0f;
            CritCoef = critCoef;
            GlobalCooldown = (float)Math.Max(1.0f, 1.5f / (1 + stats.SpellHaste));
            Cooldown = cooldown;
            SpellStatistics = new SpellStatistics();
            MagicSchool = magicSchool;

            if (character.Level >= 70 && character.Level <= 80)
                BaseMana = 2620 + (character.Level - 70) * (3863 - 2620) / 10;
            else
                BaseMana = 2620;
        }

        public Spell(string name, Stats stats, Character character, List<SpellData> SpellRankTable, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, float cooldown, Color col) :
            this(name, stats, character, SpellRankTable, manaCost, castTime, critCoef, dotDuration, damageCoef, range, cooldown, col, MagicSchool.Shadow)
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

    public class ShadowWordPain : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 4, 30, 30),
            new SpellData(2, 10, 60, 60),
            new SpellData(3, 18, 120, 120),
            new SpellData(4, 26, 210, 210),
            new SpellData(5, 34, 330, 330),
            new SpellData(6, 42, 462, 462),
            new SpellData(7, 50, 606, 606),
            new SpellData(8, 58, 768, 768),
            new SpellData(9, 65, 906, 906),
            new SpellData(10, 70, 1116, 1116),
            new SpellData(11, 75, 1176, 1176),
            new SpellData(12, 80, 1380, 1380)
        };       

        public ShadowWordPain(Stats stats, Character character)
            : base("Shadow Word: Pain", stats, character, SpellRankTable, 22, 0, 0, 18f, 18f / 15f / 1.1f, 30, 0f, Color.Red)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (MinDamage +
                (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.ImprovedShadowWordPain * 0.03f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + (character.PriestTalents.Shadowform > 0 ? stats.SpellCrit : 0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);
            
            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));

            if (stats.SWPDurationIncrease > 0)
            {
                MinDamage = MaxDamage = MinDamage * (DebuffDuration + stats.SWPDurationIncrease) / DebuffDuration;
                DebuffDuration += stats.SWPDurationIncrease;
            }

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

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

        protected void Calculate(Stats stats, Character character)
        {
            if (character.PriestTalents.VampiricTouch == 0)
            {
                MinDamage = MaxDamage = 0;
                return;
            }

            MinDamage = MaxDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + (character.PriestTalents.Shadowform > 0 ? stats.SpellCrit : 0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f));

            CastTime = (float)Math.Max(1.0f, CastTime / (1 + stats.SpellHaste));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
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
        
        protected void Calculate(Stats stats, Character character)
        {
            DamageCoef *= (1f + character.PriestTalents.Misery * 0.05f);

            MinDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + stats.BonusMindBlastMultiplier)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (MaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + stats.BonusMindBlastMultiplier)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            CastTime = (float)Math.Max(1.0f, CastTime / (1 + stats.SpellHaste));
            Cooldown -= character.PriestTalents.ImprovedMindBlast * 0.5f;

            CritCoef = (CritCoef * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.PriestTalents.ShadowPower * 0.2f) + 1f;

            CritChance = stats.SpellCrit + character.PriestTalents.MindMelt * 0.02f;
            
            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.FocusedMind * 0.05f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
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

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (MaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            CritChance = stats.SpellCrit;

            CritCoef = (CritCoef * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.PriestTalents.ShadowPower * 0.2f) + 1f;

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
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

        protected void Calculate(Stats stats, Character character)
        {
            if (character.PriestTalents.MindFlay == 0)
            {
                MinDamage = MaxDamage = 0;
                return;
            }

            DamageCoef *= 0.9f; // Coefficient penalty for snare effect.
            DamageCoef *= (1f + character.PriestTalents.Misery * 0.05f);

            MinDamage = MaxDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.MentalAgility * 0.02f)
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.FocusedMind * 0.05f));

            CritChance = stats.SpellCrit + character.PriestTalents.MindMelt * 0.02f;

            CritCoef = (CritCoef * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.PriestTalents.ShadowPower * 0.2f) + 1f;

            CastTime = (float)Math.Max(1.0f, CastTime / (1 + stats.SpellHaste));
            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\n{2}DpCT: {2}\r\nDpM: {3}\r\nTick Hit: {4}-{5}\r\nTick Crit: {6}-{7}\r\nCrit Chance: {8}%\r\nCast: {9}\r\nCost: {10}\r\n{11}",
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

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (MinDamage * (1 + character.PriestTalents.ImprovedPowerWordShield * 0.05f)
                + stats.SpellPower * 1.88f * DamageCoef
                + stats.SpellPower * character.PriestTalents.BorrowedTime * 0.04f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.ImprovedPowerWordShield * 0.05f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
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

        protected void Calculate(Stats stats, Character character)
        {
            if (character.PriestTalents.VampiricEmbrace == 0)
            {
                HealthConvertionCoef = 0;
                return;
            }

            HealthConvertionCoef *= (1 + character.PriestTalents.ImprovedVampiricEmbrace / 3f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
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

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (MinDamage +
                (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + (character.PriestTalents.Shadowform > 0 ? stats.SpellCrit : 0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class Timbal : Spell
    {
        static readonly List<SpellData> SpellRankTable = new List<SpellData>() {
            new SpellData(1, 0, 285, 475)
        };

        public Timbal(Stats stats, Character character)
            : base("Timbal", stats, character, SpellRankTable, 0, 0f, 0f, 0, 0f, 0, 0f, Color.Black)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MinDamage
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = MaxDamage
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            //CritCoef += character.PriestTalents.ShadowPower * 0.1f;
            CritChance = stats.SpellCrit;

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class Smite : Spell
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

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            MaxDamage = (MaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            CastTime = (float)Math.Max(1.0f, (CastTime - character.PriestTalents.DivineFury * 0.1f) / (1 + stats.SpellHaste));

            CritCoef = CritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f;

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                );

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
        }
    }

    public class HolyFire : Spell
    {
        class SpellDataDot : SpellData
        {
            public int DotDamage { get; protected set; }
            public SpellDataDot(int rank, int level, int minDamage, int maxDamage, int dotDamage)
                : base(rank, level, minDamage, maxDamage)
            {
                DotDamage = dotDamage;
            }
        }
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
                    DotDamage = ((SpellDataDot)sd).DotDamage;
                }

        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            MaxDamage = (MaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);


            DotDamage = (DotDamage + stats.SpellPower * 1f / 6f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            CastTime = (float)Math.Max(1.0f, (CastTime - character.PriestTalents.DivineFury * 0.1f) / (1 + stats.SpellHaste));

            CritCoef = CritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f;

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                );

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
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

        protected void Calculate(Stats stats, Character character)
        {
            if (character.PriestTalents.Penance == 0)
            {
                MinDamage = MaxDamage = 0;
                return;
            }

            MinDamage = MaxDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.FocusedPower * 0.02f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);


            CastTime = (float)Math.Max(1.0f, CastTime / (1 + stats.SpellHaste));

            CritCoef = CritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f;

            Cooldown *= (1f - character.PriestTalents.Aspiration * 0.1f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana
                * (1f - character.PriestTalents.ImprovedHealing * 0.05f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
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
