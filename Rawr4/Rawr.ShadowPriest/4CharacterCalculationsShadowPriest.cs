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
        private Stats basicStats;

        public Character LocalCharacter { get; set; }
        
        public Stats BasicStats
        {
            get { return basicStats; }
            set { basicStats = value; }
        }

        public override float OverallPoints
        {
            get
            {
                float f = 0f;
                foreach (float f2 in _subPoints)
                    f += f2;
                return f;
            }
            set { }
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
            dictValues.Add("Health", basicStats.Health.ToString());
            dictValues.Add("Mana", basicStats.Mana.ToString());
            dictValues.Add("Stamina", basicStats.Stamina.ToString());
            dictValues.Add("Intellect", basicStats.Intellect.ToString());
            dictValues.Add("Spirit", basicStats.Spirit.ToString());
            dictValues.Add("Hit", basicStats.HitRating.ToString());
            dictValues.Add("Spell Power", basicStats.SpellPower.ToString());
            dictValues.Add("Crit", basicStats.CritRating.ToString());
            dictValues.Add("Haste", basicStats.HasteRating.ToString());
            dictValues.Add("Mastery", basicStats.MasteryRating.ToString());

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

            /*
                    "Simulation:Rotation",
                    "Simulation:Castlist",
                    "Simulation:DPS",
//                    "Simulation:SustainDPS",

                    "Holy:PW Shield",
                    "Holy:Smite",
                    "Holy:Holy Fire",
                    "Holy:Penance"
             */
            return dictValues;
        }
        #endregion


    }

    

}
