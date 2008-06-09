using System;
using System.Collections.Generic;

namespace Rawr.RestoSham
  {
    class CharacterCalculationsRestoSham : CharacterCalculationsBase
      {
        private float _overallPoints = 0f;
        public override float OverallPoints
          {
            get { return _overallPoints; }
            set { _overallPoints = value; }
          }


        private float[] _subPoints = new float[] {0f, 0f};
        public override float[] SubPoints
          {
            get { return _subPoints; }
            set { _subPoints = value; }
          }


        private Stats _basicStats = null;
        public Stats BasicStats
          {
            get { return _basicStats; }
            set { _basicStats = value; }
          }
        
        
        public float Mp5OutsideFSR { get; set; }
        public float SpellCrit { get; set; }
        public float TotalManaPool { get; set; }
        public float AverageHeal { get; set; }
        public float AverageCastTime { get; set; }
        public float AverageManaCost { get; set; }
        public float TotalHealed { get; set; }
        public float FightHPS { get; set; }
        
        
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
          {
            Dictionary<string, string> values = new Dictionary<string,string>();
            
            values.Add("Health", Math.Round(BasicStats.Health, 0).ToString());
            values.Add("Stamina", Math.Round(BasicStats.Stamina, 0).ToString());
            values.Add("Intellect", BasicStats.Intellect.ToString());
            values.Add("Spirit", BasicStats.Spirit.ToString());
            values.Add("Healing", BasicStats.Healing.ToString());
            values.Add("Mana", Math.Round(BasicStats.Mana, 0).ToString());
            values.Add("MP5 (in FSR)", BasicStats.Mp5.ToString());
            values.Add("MP5 (outside FSR)", Math.Round(Mp5OutsideFSR, 0).ToString());
            values.Add("Heal Spell Crit", string.Format("{0}%*{1} spell crit rating",
                       Math.Round(SpellCrit * 100, 2), BasicStats.SpellCritRating.ToString()));
            values.Add("Spell Haste", string.Format("{0}%*{1} spell haste rating",
                       Math.Round(BasicStats.SpellHasteRating / 15.7, 2), BasicStats.SpellHasteRating.ToString()));
            values.Add("Total Mana Pool", Math.Round(TotalManaPool, 0).ToString());
            values.Add("Average Heal", Math.Round(AverageHeal, 0).ToString());
            values.Add("Average Cast Time", Math.Round(AverageCastTime, 2).ToString());
            values.Add("Average Mana Cost", Math.Round(AverageManaCost, 0).ToString());
            values.Add("Total Healed", Math.Round(TotalHealed, 0).ToString("n0"));
            values.Add("Fight HPS", Math.Round(FightHPS, 0).ToString());
          
            return values;
          }
      }
  }
