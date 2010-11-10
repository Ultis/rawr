using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
    [Rawr.Calculations.RawrModelInfo("ShadowPriest", "Spell_Shadow_Shadowform", CharacterClass.Priest)]
    public class CharacterCalculationsShadowPriest : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }
        public Stats CombatStats { get; set; }
        public int TargetLevel { get; set; }

        public Spell DevouringPlauge;
        public Spell MindBlast;
        public Spell MindFlay;
        public Spell MindSear;
        public Spell ShadowFiend;
        public Spell ShadowWordDeath;
        public Spell ShadowWordPain;
        public Spell VampiricTouch;
        public Spell PowerWordShield;
        public Spell MindSpike;

        public Character LocalCharacter { get; set; }
        

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f };
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

        public float SurvivalPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        #region the overridden method (GetCharacterDisplayCalculationValues)
        /// <summary>
        /// Builds a dictionary containing the values to display for each of the
        /// calculations defined in CharacterDisplayCalculationLabels. The key
        /// should be the Label of each display calculation, and the value
        /// should be the value to display, optionally appended with '*'
        /// followed by any string you'd like displayed as a tooltip on the
        /// value.
        /// </summary>
        /// <returns>
        /// A Dictionary<string, string> containing the values to display for
        /// each of the calculations defined in
        /// CharacterDisplayCalculationLabels.
        /// </returns>
        public override Dictionary<string, string>
            GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues
                = new Dictionary<string, string>();
            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", BasicStats.Spirit.ToString());
            dictValues.Add("Hit", BasicStats.HitRating.ToString());
            dictValues.Add("Spell Power", BasicStats.SpellPower.ToString());
            dictValues.Add("Crit", BasicStats.CritRating.ToString());
            dictValues.Add("Haste", BasicStats.HasteRating.ToString());
            dictValues.Add("Mastery", BasicStats.MasteryRating.ToString());

            dictValues.Add("Vampiric Touch", new VampiricTouch().AverageDamage.ToString());
            dictValues.Add("SW Pain", new ShadowWordPain().AverageDamage.ToString());
            dictValues.Add("Devouring Plague", new DevouringPlauge().AverageDamage.ToString());
            dictValues.Add("Imp. Devouring Plague", "TBD");
            dictValues.Add("SW Death", new DevouringPlauge().AverageDamage.ToString());
            dictValues.Add("Mind Blast", new MindBlast().AverageDamage.ToString());
            dictValues.Add("Mind Flay", new MindFlay().AverageDamage.ToString());
            dictValues.Add("Shadow Fiend", new ShadowFiend().AverageDamage.ToString());
            dictValues.Add("Mind Spike", new MindSpike().AverageDamage.ToString());
            dictValues.Add("Mind Sear", new MindSear().AverageDamage.ToString());
            dictValues.Add("PW Shield", new PowerWordShield().AverageDamage.ToString());

            Rotation r = new Rotation(new SpellBox());

            dictValues.Add("Rotation", r.ToString() + "*" + r.ToDetailedString() );

            //"Simulation:Castlist",
            //"Simulation:DPS",

            return dictValues;
        }
        #endregion


    }

    

}
