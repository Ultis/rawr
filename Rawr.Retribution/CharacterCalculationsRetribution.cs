using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    class CharacterCalculationsRetribution : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DPSPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public RotationSolution Rotation { get; set; }

        public float WhiteDPS { get; set; }
        public float SealDPS { get; set; }
        public float CrusaderStrikeDPS { get; set; }
        public float DivineStormDPS { get; set; }
        public float JudgementDPS { get; set; }
        public float ConsecrationDPS { get; set; }
        public float ExorcismDPS { get; set; }
        public float HammerOfWrathDPS { get; set; }

        public float ToMiss { get; set; }
        public float ToDodge { get; set; }
        public float ToResist { get; set; }

        public float WeaponDamage { get; set; }
        public float AttackSpeed { get; set; }
        public Stats BasicStats { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health", BasicStats.Health.ToString("N0"));
            dictValues.Add("Strength", BasicStats.Strength.ToString("N0"));
            dictValues.Add("Agility", string.Format("{0:0}*Provides {1:P} crit chance", BasicStats.Agility, (BasicStats.Agility / 6250f)));
			dictValues.Add("Attack Power", BasicStats.AttackPower.ToString("N0"));
            dictValues.Add("Crit Chance", string.Format("{0:P}*{1:0} crit rating", BasicStats.PhysicalCrit, BasicStats.CritRating));
			dictValues.Add("Miss Chance", string.Format("{0:P}*{1:P} hit ({2:0} rating)\n", ToMiss, BasicStats.PhysicalHit, BasicStats.HitRating));
			dictValues.Add("Dodge Chance", string.Format("{0:P}*{1:0} expertise ({2:0} rating)", ToDodge, BasicStats.Expertise, BasicStats.ExpertiseRating));
            dictValues.Add("Melee Haste", string.Format("{0:P}*{1:0} haste rating", BasicStats.PhysicalHaste, BasicStats.HasteRating));

            dictValues.Add("Weapon Damage", WeaponDamage.ToString("N2"));
            dictValues.Add("Attack Speed", AttackSpeed.ToString("N2"));

            dictValues.Add("White", WhiteDPS.ToString("N0"));
            dictValues.Add("Seal", SealDPS.ToString("N0"));
            dictValues.Add("Crusader Strike", CrusaderStrikeDPS.ToString("N0"));
            dictValues.Add("Judgement", JudgementDPS.ToString("N0"));
            dictValues.Add("Consecration", ConsecrationDPS.ToString("N0"));
            dictValues.Add("Exorcism", ExorcismDPS.ToString("N0"));
            dictValues.Add("Divine Storm", DivineStormDPS.ToString("N0"));
            dictValues.Add("Hammer of Wrath", HammerOfWrathDPS.ToString("N0"));
            dictValues.Add("Total DPS", OverallPoints.ToString("N0"));

            dictValues.Add("Crusader Strike CD", Rotation.CrusaderStrikeCD.ToString("N2"));
            dictValues.Add("Judgement CD", Rotation.JudgementCD.ToString("N2"));
            dictValues.Add("Consecration CD", Rotation.ConsecrationCD.ToString("N2"));
            dictValues.Add("Exorcism CD", Rotation.ExorcismCD.ToString("N2"));
            dictValues.Add("Divine Storm CD", Rotation.DivineStormCD.ToString("N2"));
            dictValues.Add("Hammer of Wrath CD", Rotation.HammerOfWrathCD.ToString("N2"));

            return dictValues;
        }
    }
}
