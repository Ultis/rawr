using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Rogue {
    class CharacterCalculationsRogue : CharacterCalculationsBase {
        #region Points
        private float _overallPoints = 0f;
        public override float OverallPoints {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DPSPoints {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }
        #endregion

        #region Basic Stats
        private Stats _basicStats;
        public Stats BasicStats {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        private int _targetLevel;
        public int TargetLevel {
            get { return _targetLevel; }
            set { _targetLevel = value; }
        }
        #endregion

        #region Offensive Stats
        private float _crit;
        public float Crit {
            get { return _crit; }
            set { _crit = value; }
        }
        #endregion

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            float levelDifference = TargetLevel - 70;

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues["Attack Power"] = BasicStats.AttackPower.ToString();
            dictValues["Hit"] = Math.Round(BasicStats.HitRating * RogueConversions.HitRatingToHit + BasicStats.Hit, 2).ToString() + string.Format("%*Hit Rating {0}", BasicStats.HitRating);
            dictValues["Expertise"] = (Math.Round(BasicStats.ExpertiseRating * RogueConversions.ExpertiseRatingToExpertise + BasicStats.Expertise)).ToString() + string.Format("%*Expertise Rating {0}", BasicStats.ExpertiseRating);
            dictValues["Haste"] = Math.Round(BasicStats.HasteRating * RogueConversions.HasteRatingToHaste, 2).ToString() + string.Format("%*Haste Rating {0}", BasicStats.HasteRating);
            dictValues["Armor Penetration"] = BasicStats.ArmorPenetration.ToString();
            dictValues["Crit"] = Math.Round(BasicStats.CritRating * RogueConversions.CritRatingToCrit + BasicStats.Crit, 2).ToString() + string.Format("%*Crit Rating {0}", BasicStats.CritRating);
            dictValues["Weapon Damage"] = BasicStats.WeaponDamage.ToString();

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation) {
            /* "Health",
             * "Haste Rating",
             * "Expertise Rating",
             * "Hit Rating",
             * "Agility",
             * "Attack Power"
             */
            switch (calculation) {
                case "Health": return BasicStats.Health;
                case "Haste Rating": return BasicStats.HasteRating;
                case "Expertise Rating": return BasicStats.ExpertiseRating;
                case "Hit Rating": return BasicStats.HitRating;
                case "Agility": return BasicStats.Agility;
                case "Attack Power": return BasicStats.AttackPower;
            }

            return 0f;
        }
    }
}
