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

        public float WhiteDPS { get; set; }
        public float SealDPS { get; set; }
        public float CrusaderStrikeDPS { get; set; }
        public float DivineStormDPS { get; set; }
        public float JudgementDPS { get; set; }
        public float ConsecrationDPS { get; set; }
        public float ExorcismDPS { get; set; }
        public float SealDPS20 { get; set; }
        public float CrusaderStrikeDPS20 { get; set; }
        public float DivineStormDPS20 { get; set; }
        public float JudgementDPS20 { get; set; }
        public float ConsecrationDPS20 { get; set; }
        public float ExorcismDPS20 { get; set; }
        public float HammerOfWrathDPS20 { get; set; }

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
			dictValues.Add("Hit Rating", string.Format("{0:0}*Negates {1:P} miss chance", BasicStats.HitRating, BasicStats.PhysicalHit));
			dictValues.Add("Expertise", string.Format("{0:0}*Negates {1:P} dodge chance", BasicStats.Expertise, (BasicStats.Expertise / 400f)));
			dictValues.Add("Haste Rating", string.Format("{0:0}*Increases attack speed by {1:P}", BasicStats.HasteRating, BasicStats.PhysicalHaste));

            dictValues.Add("Weapon Damage", WeaponDamage.ToString("N2"));
            dictValues.Add("Attack Speed", AttackSpeed.ToString("N2"));

            dictValues.Add("White", WhiteDPS.ToString("N2"));
            dictValues.Add("Seal", SealDPS.ToString("N2"));
            dictValues.Add("Crusader Strike", CrusaderStrikeDPS.ToString("N2"));
            dictValues.Add("Judgement", JudgementDPS.ToString("N2"));
            dictValues.Add("Consecration", ConsecrationDPS.ToString("N2"));
            dictValues.Add("Exorcism", ExorcismDPS.ToString("N2"));
            dictValues.Add("Divine Storm", DivineStormDPS.ToString("N2"));
            dictValues.Add("Hammer of Wrath", HammerOfWrathDPS20.ToString("N2"));
            dictValues.Add("Total DPS", DPSPoints.ToString("N2"));

            return dictValues;
        }
    }
}
