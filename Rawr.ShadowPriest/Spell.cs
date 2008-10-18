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
        public float CritChance { get; protected set; }
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
            CritChance = 0f;
            CritCoef = critCoef;
            GlobalCooldown = (float)Math.Max(1.0f, 1.5f / (1 + stats.SpellHaste));
            Cooldown = cooldown;
            SpellStatistics = new SpellStatistics();
            MagicSchool = magicSchool;
        }

        public Spell(Stats stats, string name, float minDamage, float maxDamage, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, float cooldown, Color col):
            this(stats, name, minDamage, maxDamage, manaCost, castTime, critCoef, dotDuration, damageCoef, range, cooldown, col, MagicSchool.Shadow)
        {
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

        public ShadowWordPain(Stats stats, Character character)
            : base(stats, "Shadow Word: Pain", 1236, 1236, 575, 0, 0, 15, 1.1f, 30, 0f, Color.Red)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            //DebuffDuration = DebuffDuration + talents.GetTalent("Improved Shadow Word: Pain").PointsInvested*3;

            MinDamage = (MinDamage +
                (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                //                * (1 + stats.SpellCrit)
                * (1 + character.PriestTalents.ImprovedShadowWordPain * 0.03f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);
            
            MaxDamage = (MaxDamage +
                (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                //                * (1 + stats.SpellCrit)
                * (1 + character.PriestTalents.ImprovedShadowWordPain * 0.03f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Round(ManaCost * (1 - character.PriestTalents.MentalAgility * 0.02f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
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

        public VampiricTouch(Stats stats, Character character)
            : base(stats, "Vampiric Touch", 650, 650, 425, 1.5f, 0, 15f, 1f, 30, 0f, Color.Blue)
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
            
            MinDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                //                * (1 + stats.SpellCrit)
                * (1 + character.PriestTalents.Darkness * 0.02f) 
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (MaxDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                //                * (1 + stats.SpellCrit)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);
            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
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
        public MindBlast(Stats stats, Character character)
            : base(stats, "Mind Blast", 711, 752, 450, 1.5f, 1.5f, 0, 0.4286f, 30, 8f, Color.Gold)
        {
            Calculate(stats, character);
        }
        
        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating)
                * (DamageCoef + character.PriestTalents.Misery * 0.05f))
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);
            
            MaxDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) 
                * (DamageCoef + character.PriestTalents.Misery * 0.05f))
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);
            
            CastTime = (float)Math.Max(1.0f, CastTime / (1 + stats.SpellHaste));
            Cooldown -= character.PriestTalents.ImprovedMindBlast * 0.5f;
            CritCoef += character.PriestTalents.ShadowPower * 0.1f;
            CritChance = stats.SpellCrit + character.PriestTalents.MindMelt * 0.02f;
            ManaCost = (int)Math.Floor(ManaCost
                * (1f - character.PriestTalents.FocusedMind * 0.05f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
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

        public ShadowWordDeath(Stats stats, Character character)
            : base(stats, "Shadow Word: Death", 572, 664, 309, 0, 1.5f, 0, 0.4286f, 30, 12f, Color.Gold)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Round(ManaCost
                * (1 - character.PriestTalents.MentalAgility * 0.02f)
                * (1 - character.PriestTalents.FocusedMind * 0.05f));

            CritChance = stats.SpellCrit;
            CritCoef += character.PriestTalents.ShadowPower * 0.1f;
            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
        }
    }

    public class MindFlay : Spell
    {
        public MindFlay(Stats stats, Character character)
            : base(stats, "Mind Flay", 528, 528, 230, 3f, 0, 0, 0.57f, 20, 0f, Color.LightBlue)
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

            MinDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating)
                * (DamageCoef + character.PriestTalents.Misery * 0.05f))
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            MaxDamage = (MinDamage + (stats.SpellDamageRating + stats.SpellShadowDamageRating)
                * (DamageCoef + character.PriestTalents.Misery * 0.05f))
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Round(ManaCost
                * (1 - character.PriestTalents.MentalAgility * 0.02f)
                * (1 - character.PriestTalents.FocusedMind * 0.05f));

            CritChance = stats.SpellCrit + character.PriestTalents.MindMelt * 0.02f;
            CritCoef += character.PriestTalents.ShadowPower * 0.1f;
            CastTime = (float)Math.Max(1.0f, CastTime / (1 + stats.SpellHaste));
            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
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
        public PowerWordShield(Stats stats, Character character)
            : base(stats, "Power Word: Shield", 1295, 1295, 600, 0, 0, 0, 1.5f / 3.5f, 30, 4f, Color.Brown)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (MinDamage + stats.SpellDamageRating
                * (DamageCoef + character.PriestTalents.BorrowedTime * 0.04f))
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.ImprovedPowerWordShield * 0.05f);

            ManaCost = (int)Math.Round(ManaCost
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
            : base(stats, "Vampiric Embrace", 0, 0, (int)Math.Round(stats.Mana * 0.02), 0, 0, 60, 0, 30, 10f, Color.Green)
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

            HealthConvertionCoef += character.PriestTalents.ImprovedVampiricEmbrace * 0.05f;

            ManaCost = (int)Math.Round(ManaCost
                * (1 - character.PriestTalents.MentalAgility * 0.02f));
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

        public DevouringPlague(Stats stats, Character character)
            : base(stats, "Devouring Plague", 1216, 1216, 1145, 0, 0, 24, 1.1f, 30, 180f, Color.Purple)
        {
            Calculate(stats, character);
        }

        protected void Calculate(Stats stats, Character character)
        {
            MinDamage = MaxDamage = (MinDamage +
                (stats.SpellDamageRating + stats.SpellShadowDamageRating) * DamageCoef)
                //* (1 + stats.SpellCrit)
                * (1 + character.PriestTalents.TwinDisciplines * 0.01f)
                * (1 + character.PriestTalents.Darkness * 0.02f)
                * (1 + character.PriestTalents.Shadowform * 0.15f);

            ManaCost = (int)Math.Round(ManaCost * (1 - character.PriestTalents.MentalAgility * 0.02f));

            Range = (int)Math.Round(Range * (1 + character.PriestTalents.ShadowReach * 0.1f));
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
