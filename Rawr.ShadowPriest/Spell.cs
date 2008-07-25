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
        public static Spell CreateSpell(string name, Stats stats, TalentTree talents)
        {
            switch (name)
            {
                case "Vampiric Embrace":
                    return new VampiricEmbrace(stats, talents);
                case "Vampiric Touch":
                    return new VampiricTouch(stats, talents);
                case "Shadow Word: Pain":
                    return new ShadowWordPain(stats, talents);
                case "Devouring Plague":
                    return new DevouringPlague(stats, talents);
                case "Starshards":
                    return new Starshards(stats, talents);
                case "Mind Blast":
                    return new MindBlast(stats, talents);
                case "Shadow Word: Death":
                    return new ShadowWordDeath(stats, talents);
                case "Mind Flay":
                    return new MindFlay(stats, talents);
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
    }

    public class Spell
    {
        public static readonly List<string> SpellList = new List<string>() { "Vampiric Embrace", "Vampiric Touch", "Shadow Word: Pain", "Devouring Plague", "Starshards", "Mind Blast", "Shadow Word: Death", "Mind Flay" };

        public string Name { get; protected set; }

        public MagicSchool MagicSchool { get; protected set; }
        public float MinDamage { get; protected set; }
        public float MaxDamage { get; protected set; }
        public float DamageCoef { get; protected set; }
        public int Range { get; protected set; }
        public int ManaCost { get; protected set; }
        public float CastTime { get; protected set; }
        public float CritCoef { get; protected set; }
        public float DebuffDuration { get; protected set; }
        public int Targets { get; protected set; }

        public float Cooldown { get; protected set; }
        public float GlobalCooldown { get; protected set; }
        public Color GraphColor { get; protected set; }

        public SpellStatistics SpellStatistics { get; protected set; }

        #region Properties

        public float AvgDamage
        {
            get
            {
                return (MinDamage + MaxDamage) / 2;
            }
        }

        public virtual float DpS
        {
            get
            {
                return AvgDamage / CastTime;
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
                return (MinDamage * CritCoef + MaxCrit) / 2;
            }
        }

        public float MaxCrit
        {
            get
            {
                return MaxDamage * CritCoef;
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
            CritCoef = critCoef;
            GlobalCooldown = 1.5f * (1 - stats.SpellHasteRating / 15.7f / 100f);
            Cooldown = cooldown;
            SpellStatistics = new SpellStatistics();
            MagicSchool = magicSchool;
        }

        public Spell(Stats stats, string name, float minDamage, float maxDamage, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, float cooldown, Color col):
            this(stats, name, minDamage, maxDamage, manaCost, castTime, critCoef, dotDuration, damageCoef, range, cooldown, col, MagicSchool.Shadow)
        {
        }       

        public static float GetGlobalCooldown(Stats stats)
        {
            return 1.5f * (1 - stats.SpellHasteRating / 15.7f / 100f);
        }
       
        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpM: {2}\r\nMin: {3}\r\nMax: {4}\r\nAvg Crit: {5}\r\nMax Crit: {6}\r\nCast: {7}\r\nCost: {8}",
                AvgDamage.ToString("0"),
                DpS.ToString("0.00"),
                DpM.ToString("0.00"),
                MinDamage.ToString("0"),
                MaxDamage.ToString("0"),
                AvgCrit.ToString("0"),
                MaxCrit.ToString("0"),
                CastTime.ToString("0.00"),
                ManaCost.ToString("0"));
        }
    }

    public class ShadowWordPain : Spell
    {
        public override float DpS
        {
            get
            {
                return AvgDamage / DebuffDuration;
            }
        }

        public ShadowWordPain(Stats stats, TalentTree talents)
            : base(stats, "Shadow Word: Pain", 1236, 1236, 575, 0, 0, 15, 1.1f, 30, 0f, Color.Red)
        {
            Calculate(stats, talents);
        }

        protected void Calculate(Stats stats, TalentTree talents)
        {
            DebuffDuration = DebuffDuration + talents.GetTalent("Improved Shadow Word: Pain").PointsInvested*3;

            MinDamage = (MinDamage + 
                (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f)
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);
            
            MaxDamage = (MaxDamage + 
                (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f) 
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);

            ManaCost = (int)Math.Round(ManaCost * (1 - talents.GetTalent("Mental Agility").PointsInvested * 0.02f));

            Range = (int)Math.Round(Range * (1 + talents.GetTalent("Shadow Reach").PointsInvested*0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpM: {2}\r\nTick: {3}\r\nCost: {4}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpM.ToString("0.00"),
                          (AvgDamage/DebuffDuration*3).ToString("0"),
                          ManaCost.ToString("0"));
        }
    }

    public class VampiricTouch : Spell
    {
        public override float DpS
        {
            get
            {
                return AvgDamage / DebuffDuration;
            }
        }

        public VampiricTouch(Stats stats, TalentTree talents)
            : base(stats, "Vampiric Touch", 650, 650, 425, 1.5f, 0, 15f, 1f, 30, 0f, Color.Blue)
        {
            Calculate(stats, talents);
        }

        protected void Calculate(Stats stats, TalentTree talents)
        {
            if (talents.GetTalent("Vampiric Touch").PointsInvested == 0)
            {
                MinDamage = MaxDamage = 0;
                return;
            }            
            
            MinDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f) 
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);

            MaxDamage = (MaxDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f) 
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);
            Range = (int)Math.Round(Range * (1 + talents.GetTalent("Shadow Reach").PointsInvested * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpM: {2}\r\nTick: {3}\r\nCost: {4}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpM.ToString("0.00"),
                          (AvgDamage / DebuffDuration * 3).ToString("0"),
                          ManaCost.ToString("0"));
        }
    }

    public class MindBlast : Spell
    {
        public MindBlast(Stats stats, TalentTree talents)
            : base(stats, "Mind Blast", 711, 752, 450, 1.5f, 1.5f, 0, 0.4286f, 30, 8f, Color.Gold)
        {
            Calculate(stats, talents);
        }
        
        protected void Calculate(Stats stats, TalentTree talents)
        {
            MinDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef) 
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f) 
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);
            
            MaxDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f) 
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);
            
            CastTime = CastTime * (1 - stats.SpellHasteRating / 15.7f / 100f);
            Cooldown -= talents.GetTalent("Improved Mind Blast").PointsInvested * 0.5f;
            Range = (int)Math.Round(Range * (1 + talents.GetTalent("Shadow Reach").PointsInvested * 0.1f));
        }
    }

    public class ShadowWordDeath : Spell
    {
        public override float DpS
        {
            get
            {
                return AvgDamage / GlobalCooldown;
            }
        }

        public ShadowWordDeath(Stats stats, TalentTree talents)
            : base(stats, "Shadow Word: Death", 572, 664, 309, 0, 1.5f, 0, 0.4286f, 30, 12f, Color.Gold)
        {
            Calculate(stats, talents);
        }

        protected void Calculate(Stats stats, TalentTree talents)
        {
            MinDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f)
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);

            MaxDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f)
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);

            ManaCost = (int)Math.Round(ManaCost
                * (1 - talents.GetTalent("Mental Agility").PointsInvested * 0.02f)
                * (1 - talents.GetTalent("Focused Mind").PointsInvested * 0.05f));

            Range = (int)Math.Round(Range * (1 + talents.GetTalent("Shadow Reach").PointsInvested * 0.1f));
        }
    }

    public class MindFlay : Spell
    {
        public MindFlay(Stats stats, TalentTree talents)
            : base(stats, "Mind Flay", 528, 528, 230, 3f, 0, 0, 0.57f, 20, 0f, Color.LightBlue)
        {
            Calculate(stats, talents);
        }

        protected void Calculate(Stats stats, TalentTree talents)
        {
            if (talents.GetTalent("Mind Flay").PointsInvested == 0)
            {
                MinDamage = MaxDamage = 0;
                return;
            }

            MinDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f)
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);

            MaxDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f)
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);

            ManaCost = (int)Math.Round(ManaCost
                * (1 - talents.GetTalent("Mental Agility").PointsInvested * 0.02f)
                * (1 - talents.GetTalent("Focused Mind").PointsInvested * 0.05f));

            CastTime = CastTime * (1 - stats.SpellHasteRating / 15.7f / 100f);
            Range = (int)Math.Round(Range * (1 + talents.GetTalent("Shadow Reach").PointsInvested * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpM: {2}\r\nTick: {3}\r\nCast: {4}\r\nCost: {5}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpM.ToString("0.00"),
                          (AvgDamage / CastTime * 3).ToString("0"),
                           CastTime.ToString("0.00"),
                          ManaCost.ToString("0"));
        }
    }

    public class PowerWordShield : Spell
    {
        public PowerWordShield(Stats stats, TalentTree talents)
            : base(stats, "Power Word: Shield", 1315, 1315, 600, 0, 0, 0, 0.3f, 30, 4f, Color.Brown)
        {
            Calculate(stats, talents);
        }

        protected void Calculate(Stats stats, TalentTree talents)
        {
            MinDamage = MaxDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Improved Power Word: Shield").PointsInvested * 0.05f);

            ManaCost = (int)Math.Round(ManaCost
                * (1 - talents.GetTalent("Mental Agility").PointsInvested * 0.02f));

            CastTime = CastTime * (1 - stats.SpellHasteRating / 15.7f / 100f);
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

        public VampiricEmbrace(Stats stats, TalentTree talents)
            : base(stats, "Vampiric Embrace", 0, 0, (int)Math.Round(stats.Mana * 0.02), 0, 0, 60, 0, 30, 10f, Color.Green)
        {
            HealthConvertionCoef = 0.15f;
            Calculate(stats, talents);
        }

        protected void Calculate(Stats stats, TalentTree talents)
        {
            if (talents.GetTalent("Vampiric Embrace").PointsInvested == 0)
            {
                HealthConvertionCoef = 0;
                return;
            }

            HealthConvertionCoef += talents.GetTalent("Improved Vampiric Embrace").PointsInvested * 0.05f;

            ManaCost = (int)Math.Round(ManaCost
                * (1 - talents.GetTalent("Mental Agility").PointsInvested * 0.02f));
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
        public override float DpS
        {
            get
            {
                return AvgDamage / DebuffDuration;
            }
        }

        public DevouringPlague(Stats stats, TalentTree talents)
            : base(stats, "Devouring Plague", 1216, 1216, 1145, 0, 0, 24, 1.1f, 30, 180f, Color.Purple)
        {
            Calculate(stats, talents);
        }

        protected void Calculate(Stats stats, TalentTree talents)
        {
            MinDamage = MaxDamage = (MinDamage +
                (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + talents.GetTalent("Darkness").PointsInvested * 0.02f)
                * (1 + talents.GetTalent("Shadow Form").PointsInvested * 0.15f);

            ManaCost = (int)Math.Round(ManaCost * (1 - talents.GetTalent("Mental Agility").PointsInvested * 0.02f));

            Range = (int)Math.Round(Range * (1 + talents.GetTalent("Shadow Reach").PointsInvested * 0.1f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpM: {2}\r\nTick: {3}\r\nCost: {4}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpM.ToString("0.00"),
                          (AvgDamage / DebuffDuration * 3).ToString("0"),
                          ManaCost.ToString("0"));
        }
    }

    public class Starshards : Spell
    {
        public override float DpS
        {
            get
            {
                return AvgDamage / DebuffDuration;
            }
        }

        public Starshards(Stats stats, TalentTree talents)
            : base(stats, "Starshards", 785, 785, 0, 0, 0, 15, 1.1f, 30, 30f, Color.Purple, MagicSchool.Arcane)
        {
            Calculate(stats, talents);
        }

        protected void Calculate(Stats stats, TalentTree talents)
        {
            MinDamage = MaxDamage = (MinDamage +
                (stats.SpellDamageRating + stats.SpellArcaneDamageRating) * DamageCoef);

            ManaCost = (int)Math.Round(ManaCost * (1 - talents.GetTalent("Mental Agility").PointsInvested * 0.02f));
        }

        public override string ToString()
        {
            return String.Format("{0} *DpS: {1}\r\nDpM: {2}\r\nTick: {3}\r\nCost: {4}",
                          AvgDamage.ToString("0"),
                          DpS.ToString("0.00"),
                          DpM.ToString("0.00"),
                          (AvgDamage / DebuffDuration * 3).ToString("0"),
                          ManaCost.ToString("0"));
        }
    }
}
