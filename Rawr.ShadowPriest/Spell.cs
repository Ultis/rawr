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
                default:
                    return null;
            }
        }
    }

    public class SpellStatistics
    {
        public int CritCount { get; set; }
        public int MissCount { get; set; }
        public int HitCount { get; set; }
        public float CooldownReset { get; set; }
        public float DamageDone { get; set; }
        public float ManaUsed { get; set; }
    }

    public class Spell
    {
        public static readonly List<string> ShadowSpellList = new List<string>() { "Vampiric Embrace", "Vampiric Touch", "Mind Blast", "Devouring Plague", "Shadow Word: Pain", "Shadow Word: Death", "Mind Flay" };
        public static readonly List<string> HolySpellList = new List<string>() { "Holy Fire", "Devouring Plague", "Shadow Word: Pain", "Mind Blast", "Shadow Word: Death", "Smite" };

        public string Name { get; protected set; }

        public MagicSchool MagicSchool { get; protected set; }
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

        public static Dictionary<int, int> BaseMana = new Dictionary<int, int>();

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

        public Spell(Stats stats, string name, float minDamage, float maxDamage, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, float cooldown, Color col, MagicSchool magicSchool)
        {
            Name = name;
            DamageCoef = damageCoef;
            DebuffDuration = dotDuration;
            GraphColor = col;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
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

            BaseMana[70] = 2620;
            BaseMana[71] = 0;
            BaseMana[72] = 0;
            BaseMana[73] = 0;
            BaseMana[74] = 0;
            BaseMana[75] = 0;
            BaseMana[76] = 0;
            BaseMana[77] = 0;
            BaseMana[78] = 0;
            BaseMana[79] = 0;
            BaseMana[80] = 3863;

        }

        public Spell(Stats stats, string name, float minDamage, float maxDamage, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, float cooldown, Color col):
            this(stats, name, minDamage, maxDamage, manaCost, castTime, critCoef, dotDuration, damageCoef, range, cooldown, col, MagicSchool.Shadow)
        {
        }       
       
        public override string ToString()
        {
            if (DebuffDuration > 0f)
                return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nTick: {4}-{5}\r\nDuration: {6}s\r\nCost: {7}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpCT.ToString("0.00"),
                          DpM.ToString("0.00"),
                          Math.Floor(AvgDamage / DebuffDuration * 3).ToString("0"),
                          Math.Ceiling(AvgDamage / DebuffDuration * 3).ToString("0"),
                          DebuffDuration.ToString("0"),
                          ManaCost.ToString("0"));

            return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nHit: {4}-{5}, Avg {6}\r\nCrit: {7}-{8}, Avg {9}\r\nCrit Chance: {10}%\r\nCast: {11}\r\nCost: {12}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpCT.ToString("0.00"),
                DpM.ToString("0.00"),
                MinDamage.ToString("0"), MaxDamage.ToString("0"), AvgHit.ToString("0"),
                MinCrit.ToString("0"), MaxCrit.ToString("0"), AvgCrit.ToString("0"),
                (CritChance * 100f).ToString("0.00"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"));
        }
    }

    public class ShadowWordPain : Spell
    {
        public ShadowWordPain(Stats stats, Character character)
            : base(stats, "Shadow Word: Pain", 1116, 1116, 22, 0, 0, 18f, 18f / 15f / 1.1f, 30, 0f, Color.Red)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (MinDamage +
                (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.ImprovedShadowWordPain * 0.03f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + (character.PriestTalents.Shadowform > 0 ? stats.SpellCrit : 0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);
            
            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
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
        public VampiricTouch(Stats stats, Character character)
            : base(stats, "Vampiric Touch", 715, 715, 16, 1.5f, 0, 15f, 15f / 15f * 1.9f, 30, 0f, Color.Blue)
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
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + (character.PriestTalents.Shadowform > 0 ? stats.SpellCrit : 0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
                * (1f - character.PriestTalents.ShadowFocus * 0.02f));

            CastTime = (float)Math.Max(1.0f, CastTime / (1 + stats.SpellHaste));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class MindBlast : Spell
    {
        public MindBlast(Stats stats, Character character)
            : base(stats, "Mind Blast", 711, 752, 17, 1.5f, 1.5f, 0, 1.5f / 3.5f, 30, 8f, Color.Gold)
        {
            Calculate(stats, character);
        }
        
        protected void Calculate(Stats stats, Character character)
        {
            DamageCoef *= (1f + character.PriestTalents.Misery * 0.05f);

            MinDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + stats.BonusMindBlastMultiplier)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (MaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + stats.BonusMindBlastMultiplier)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            CastTime = (float)Math.Max(1.0f, CastTime / (1 + stats.SpellHaste));
            Cooldown -= character.PriestTalents.ImprovedMindBlast * 0.5f;

            CritCoef = (CritCoef * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.PriestTalents.ShadowPower * 0.2f) + 1f;

            CritChance = stats.SpellCrit + character.PriestTalents.MindMelt * 0.02f;
            
            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.FocusedMind * 0.05f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class ShadowWordDeath : Spell
    {
        public ShadowWordDeath(Stats stats, Character character)
            : base(stats, "Shadow Word: Death", 572, 664, 12, 0, 1.5f, 0, 1.5f / 3.5f, 30, 12f, Color.Gold)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (MaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            CritChance = stats.SpellCrit;

            CritCoef = (CritCoef * (1f + stats.BonusSpellCritMultiplier) - 1f) * (1f + character.PriestTalents.ShadowPower * 0.2f) + 1f;

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class MindFlay : Spell
    {
        public MindFlay(Stats stats, Character character)
            : base(stats, "Mind Flay", 450, 450, 9, 3f, 1.5f, 0, 3f / 3.5f, 20, 0f, Color.LightBlue)
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
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
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
            return String.Format("{0} *DpS: {1}\r\n{2}DpCT: {2}\r\nDpM: {3}\r\nTick Hit: {4}-{5}\r\nTick Crit: {6}-{7}\r\nCrit Chance: {8}%\r\nCast: {9}\r\nCost: {10}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpCT.ToString("0.00"),
                          DpM.ToString("0.00"),
                          Math.Floor(AvgHit / CastTime).ToString("0"), Math.Ceiling(AvgHit / CastTime).ToString("0"),
                          Math.Floor(AvgCrit / CastTime).ToString("0"), Math.Ceiling(AvgCrit / CastTime).ToString("0"),
                          (CritChance * 100f).ToString("0.00"),
                          CastTime.ToString("0.00"),
                          ManaCost.ToString("0"));
        }
    }

    public class PowerWordShield : Spell
    {
        public PowerWordShield(Stats stats, Character character)
            : base(stats, "Power Word: Shield", 1295, 1295, 23, 0, 0, 0, 1.5f / 3.5f, 30, 4f, Color.Brown)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (MinDamage + stats.SpellPower
                * (DamageCoef + character.PriestTalents.BorrowedTime * 0.04f))
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.ImprovedPowerWordShield * 0.05f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
                * (1 - character.PriestTalents.MentalAgility * 0.02f));
        }

        public override string ToString()
        {
            return String.Format("{0} *Cost: {1}",
                MinDamage.ToString("0"),
                ManaCost.ToString("0"));
        }
    }

    public class VampiricEmbrace : Spell
    {
        public float HealthConvertionCoef { get; protected set; }

        public VampiricEmbrace(Stats stats, Character character)
            : base(stats, "Vampiric Embrace", 0, 0, 2, 0, 0, 60f, 0, 30, 10f, Color.Green)
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

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));
        }

        public override string ToString()
        {
            return String.Format("{0} *Cost: {1}",
                (HealthConvertionCoef * 100).ToString("0") + "%",
                ManaCost.ToString("0"));
        }
    }

    public class DevouringPlague : Spell
    {
        public DevouringPlague(Stats stats, Character character)
            : base(stats, "Devouring Plague", 1088, 1088, 25, 0, 0, 24, 24f / 15f * 0.925f, 30, 24, Color.Purple)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (MinDamage +
                (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + (character.PriestTalents.Shadowform > 0 ? stats.SpellCrit : 0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
                * (1f - character.PriestTalents.ShadowFocus * 0.02f)
                * (1f - character.PriestTalents.MentalAgility * 0.02f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class Timbal : Spell
    {
        public Timbal(Stats stats, Character character)
            : base(stats, "Timbal", 285, 475, 0, 0f, 0f, 0, 0f, 0, 0f, Color.Black)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MinDamage 
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + ((character.PriestTalents.ShadowWeaving > 0) ? 0.1f : 0.0f))
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = MaxDamage
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
        public Smite(Stats stats, Character character)
            : base(stats, "Smite", 549, 616, 15, 2.5f, 1.5f, 0, 2.5f / 3.5f, 30, 0f, Color.Yellow)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            MaxDamage = (MaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.SearingLight * 0.15f);

            CastTime = (float)Math.Max(1.0f, (CastTime - character.PriestTalents.DivineFury * 0.1f) / (1 + stats.SpellHaste));

            CritCoef = CritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f;

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
                );

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
        }
    }

    public class HolyFire : Spell
    {
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
            : base(stats, "Holy Fire", 719, 910, 11, 2.0f, 1.5f, 7f, 0.5f, 30, 10f, Color.Yellow)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = (MinDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            MaxDamage = (MaxDamage + (stats.SpellPower + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            DotDamage = (147 + stats.SpellPower * 0.1666667f)
                * (1 + character.PriestTalents.SearingLight * 0.05f);

            CastTime = (float)Math.Max(1.0f, (CastTime - character.PriestTalents.DivineFury * 0.1f) / (1 + stats.SpellHaste));

            CritCoef = CritCoef * (1f + stats.BonusSpellCritMultiplier);

            CritChance = stats.SpellCrit + character.PriestTalents.HolySpecialization * 0.01f;

            ManaCost = (int)Math.Floor(ManaCost / 100f * BaseMana[70]
                );

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.HolyReach * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpCT: {2}\r\nDpM: {3}\r\nHit: {4}-{5}, Avg {6}\r\nCrit: {7}-{8}, Avg {9}\r\nTick: {10}-{11}\r\nCrit Chance: {12}%\r\nCast: {13}\r\nCost: {14}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpCT.ToString("0.00"),
                DpM.ToString("0.00"),
                MinDamage.ToString("0"), MaxDamage.ToString("0"), AvgHit.ToString("0"),
                MinCrit.ToString("0"), MaxCrit.ToString("0"), AvgCrit.ToString("0"),
                Math.Floor(DotDamage / 7f).ToString("0"), Math.Ceiling(DotDamage / 7f).ToString("0"),
                (CritChance * 100f).ToString("0.00"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"));
        }
    }
}
