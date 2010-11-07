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
        #region Variable Declarations and Definitions

        public Stats BasicStats { get; set; }

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

        #endregion

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

            dictValues.Add("Vampiric Touch", new VampiricTouch().AvgHit.ToString());
            dictValues.Add("SW Pain", new ShadowWordPain().AvgHit.ToString());
            dictValues.Add("Devouring Plague", new DevouringPlauge().AvgHit.ToString());
            dictValues.Add("Imp. Devouring Plague", "TBD");
            dictValues.Add("SW Death", new DevouringPlauge().AvgHit.ToString());
            dictValues.Add("Mind Blast", new MindBlast().AvgHit.ToString());
            dictValues.Add("Mind Flay", new MindFlay().AvgHit.ToString());
            dictValues.Add("Shadow Fiend", new ShadowFiend().AvgHit.ToString());
            dictValues.Add("Mind Spike", new MindSpike().AvgHit.ToString());
            dictValues.Add("Mind Sear", new MindSear().AvgHit.ToString());

            dictValues.Add("PW Shield", new PowerWordShield().AvgHit.ToString());
            dictValues.Add("Smite", new Smite().AvgHit.ToString());
            dictValues.Add("Holy Fire", new HolyFire().AvgHit.ToString());
            dictValues.Add("Penance", new Penance().AvgHit.ToString());

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
