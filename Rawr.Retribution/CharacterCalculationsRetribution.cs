using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class CharacterCalculationsRetribution : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints { get { return _overallPoints; } set { _overallPoints = value; } }
        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints { get { return _subPoints; } set { _subPoints = value; } }
        public float DPSPoints { get { return _subPoints[0]; } set { _subPoints[0] = value; } }

        public RotationSolution Solution { get; set; }
        public Ability[] Rotation { get; set; }
        public int RotationIndex { get; set; }

        public float WhiteDPS { get; set; }
        public float SealDPS { get; set; }
        public float CrusaderStrikeDPS { get; set; }
        public float DivineStormDPS { get; set; }
        public float JudgementDPS { get; set; }
        public float ConsecrationDPS { get; set; }
        public float ExorcismDPS { get; set; }
        public float HandOfReckoningDPS { get; set; }
        public float HammerOfWrathDPS { get; set; }
        public float OtherDPS { get; set; }

        public Skill WhiteSkill { get; set; }
        public Skill SealSkill { get; set; }
        public Skill CrusaderStrikeSkill { get; set; }
        public Skill DivineStormSkill { get; set; }
        public Skill JudgementSkill { get; set; }
        public Skill ConsecrationSkill { get; set; }
        public Skill ExorcismSkill { get; set; }
        public Skill HandOfReckoningSkill { get; set; }
        public Skill HammerOfWrathSkill { get; set; }

        public float ToMiss { get; set; }
        public float ToDodge { get; set; }
        public float ToResist { get; set; }

        public float AverageSoVStack { get; set; }
        public float SoVOvertake { get; set; }
        public float WeaponDamage { get; set; }
        public float AttackSpeed { get; set; }
        public Stats BasicStats { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues["Status"] = string.Format("{0} dps", DPSPoints.ToString("N0"));
            dictValues["Health"] = BasicStats.Health.ToString("N0");
            dictValues["Strength"] = BasicStats.Strength.ToString("N0");
            dictValues["Agility"] = string.Format("{0:0}", BasicStats.Agility);
            dictValues["Attack Power"] = BasicStats.AttackPower.ToString("N0");
            dictValues["Crit Chance"] = string.Format("{0:P}*{1:0} crit rating", BasicStats.PhysicalCrit, BasicStats.CritRating);
            dictValues["Miss Chance"] = string.Format("{0:P}*{1:P} hit ({2:0} rating)\n", ToMiss, BasicStats.PhysicalHit, BasicStats.HitRating);
            dictValues["Dodge Chance"] = string.Format("{0:P}*{1:P} expertise ({2:0} rating)", ToDodge, BasicStats.Expertise * .0025f, BasicStats.ExpertiseRating);
            dictValues["Melee Haste"] = string.Format("{0:P}*{1:0} haste rating", BasicStats.PhysicalHaste, BasicStats.HasteRating);

            dictValues["Weapon Damage"] = WeaponDamage.ToString("N2");
            dictValues["Attack Speed"] = AttackSpeed.ToString("N2");

            dictValues["White"] = string.Format("{0}*{1}", WhiteDPS.ToString("N0"), WhiteSkill.ToString());
            dictValues["Seal"] = string.Format("{0}*{1}", SealDPS.ToString("N0"), SealSkill.ToString());
            dictValues["Crusader Strike"] = string.Format("{0}*{1}", CrusaderStrikeDPS.ToString("N0"), CrusaderStrikeSkill.ToString());
            dictValues["Judgement"] = string.Format("{0}*{1}", JudgementDPS.ToString("N0"), JudgementSkill.ToString());
            dictValues["Consecration"] = string.Format("{0}*{1}", ConsecrationDPS.ToString("N0"), ConsecrationSkill.ToString());
            dictValues["Exorcism"] = string.Format("{0}*{1}", ExorcismDPS.ToString("N0"), ExorcismSkill.ToString());
            dictValues["Divine Storm"] = string.Format("{0}*{1}", DivineStormDPS.ToString("N0"), DivineStormSkill.ToString());
            dictValues["Hammer of Wrath"] = string.Format("{0}*{1}", HammerOfWrathDPS.ToString("N0"), HammerOfWrathSkill.ToString());
            dictValues["Hand of Reckoning"] = string.Format("{0}*{1}", HandOfReckoningDPS.ToString("N0"), HandOfReckoningSkill.ToString());
            dictValues["Other"] = OtherDPS.ToString("N0");
            dictValues["Total DPS"] = OverallPoints.ToString("N0");

            dictValues["Average SoV Stack"] = AverageSoVStack.ToString("N2");
            dictValues["Chosen Rotation"] = Rotation == null ? "n/a" : RotationParameters.ShortRotationString(Rotation);
            dictValues["SoV Overtake"] = string.Format("{0} sec", SoVOvertake.ToString("N2"));
            dictValues["Crusader Strike CD"] = Solution.CrusaderStrikeCD.ToString("N2");
            dictValues["Judgement CD"] = Solution.JudgementCD.ToString("N2");
            dictValues["Consecration CD"] = Solution.ConsecrationCD.ToString("N2");
            dictValues["Exorcism CD"] = Solution.ExorcismCD.ToString("N2");
            dictValues["Divine Storm CD"] = Solution.DivineStormCD.ToString("N2");
            dictValues["Hammer of Wrath CD"] = Solution.HammerOfWrathCD.ToString("N2");

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "Melee Avoid %": return (WhiteSkill.Combats.GetMeleeAvoid() * 100f);
            }
            return 0f;
        }
    }
}