using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Rawr.ShadowPriest
{
    public class Spell
    {
        public string Name { get; protected set; }

        public float MinDamage { get; protected set; }
        public float MaxDamage { get; protected set; }
        public float DamageCoef { get; protected set; }
        public int Range { get; protected set; }
        public int ManaCost { get; protected set; }
        public float CastTime { get; protected set; }
        public float CritCoef { get; protected set; }
        public int Rank { get; protected set; }
        public float DebuffDuration { get; protected set; }
        public int Targets { get; protected set; }

        public float GlobalCooldown { get; protected set; }
        public Color GraphColor { get; protected set; }

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

        public Spell(Stats stats, string name, int rank, float minDamage, float maxDamage, int manaCost, float castTime, float critCoef, float dotDuration, float damageCoef, int range, Color col)
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
            Rank = rank;
            DamageCoef = damageCoef;
            Range = range;
            CritCoef = critCoef;
            GlobalCooldown = 1.5f * (1 - stats.SpellHasteRating / 15.7f / 100f);
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
            : base(stats, "Shadow Word: Pain", 10, 1236, 1236, 575, 0, 0, 15, 1.1f, 30, Color.Red)
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
            : base(stats, "Vampiric Touch", 3, 650, 650, 425, 1.5f, 0, 15f, 1f, 30, Color.Blue)
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
            
            DebuffDuration += talents.GetTalent("Shadow Form").PointsInvested * 3;

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
            : base(stats, "Mind Blast", 11, 711, 752, 450, 1.5f, 1.5f, 0, 0.4286f, 30, Color.Gold)
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
            : base(stats, "Shadow Word Death", 2, 572, 664, 309, 0, 1.5f, 0, 0.4286f, 30, Color.Gold)
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
            : base(stats, "Mind Flay", 7, 528, 528, 230, 3f, 0, 3f, 0.57f, 20, Color.LightBlue)
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
                          (AvgDamage / DebuffDuration * 3).ToString("0"),
                           CastTime.ToString("0.00"),
                          ManaCost.ToString("0"));
        }
    }

    public class PowerWordShield : Spell
    {
        public PowerWordShield(Stats stats, TalentTree talents)
            : base(stats, "Power Word Shield", 12, 1315, 1315, 600, 0, 0, 0, 0.3f, 30, Color.Brown)
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
            : base(stats, "Vampiric Embrace", 1, 0, 0, (int)Math.Round(stats.Mana * 0.02), 0, 0, 60, 0, 30, Color.Green)
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
}
