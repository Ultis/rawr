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
        public CharacterCalculationsShadowPriest()
        {
            _results = new Dictionary<string, string>();
        }

        public Stats BasicStats { get; set; }
        public Stats CombatStats { get; set; }
        public int TargetLevel { get; set; }

        public Spell DevouringPlauge;
        public Spell MindBlast;
        public Spell MindFlay;
        public Spell ShadowFiend;
        public Spell ShadowWordDeath;
        public Spell ShadowWordPain;
        public Spell VampiricTouch;
        public Spell PowerWordShield;
        public Spell MindSpike;

        public string Rotation;
        public string RotationDetails;

        public Character LocalCharacter { get; set; }
        
        private float _overallPoints = 0f;

        public override float OverallPoints
        {
            get
            {
                var points = 0f;
                foreach (float subPoint in SubPoints)
                {
                    points += subPoint;
                }
                return points;
            }
            set { throw new NotSupportedException("Setting overall points directly is not supported"); }
        }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        private Dictionary<string, string> _results;

        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float BurstPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float DpsPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float SurvivalPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
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
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            _results.Add("Vampiric Touch", VampiricTouch.AverageDamage.ToString() + "*" + Spellinfo(VampiricTouch));
            _results.Add("SW Pain", ShadowWordPain.AverageDamage.ToString() + "*" + Spellinfo(ShadowWordPain));
            _results.Add("Devouring Plague", DevouringPlauge.AverageDamage.ToString() + "*" + Spellinfo(DevouringPlauge));
            _results.Add("Imp. Devouring Plague", "TBD");
            _results.Add("SW Death", ShadowWordDeath.AverageDamage.ToString() + "*" + Spellinfo(ShadowWordDeath));
            _results.Add("Mind Blast", MindBlast.AverageDamage.ToString() + "*" + Spellinfo(MindBlast));
            _results.Add("Mind Flay", MindFlay.AverageDamage.ToString() + "*" + Spellinfo(MindFlay));
            _results.Add("Shadowfiend", ShadowFiend.AverageDamage.ToString() + "*" + Spellinfo(ShadowFiend));
            _results.Add("Mind Spike", MindSpike.AverageDamage.ToString() + "*" + Spellinfo(MindSpike));
            _results.Add("PW Shield", "TBD"); // PowerWordShield.AverageDamage.ToString());

            _results.Add("Rotation", Rotation + "*" + RotationDetails);
            //dictValues.Add("DPS", DpsPoints.ToString());

            //"Simulation:Castlist",
            //"Simulation:DPS",

            return _results;
        }

        private string Spellinfo(Spell spell)
        {
            string details = "No Details";
            string type = spell.GetType().BaseType.Name.ToString();
            switch (type)
            {
                case "DoTSpell":
                    DoTSpell dot = spell as DoTSpell;
                    details =
                        "Damage over time spell" +
                        "\nNumber of ticks: " + dot.TickNumber.ToString() +
                        "\nTick Damage:" + dot.TickDamage.ToString() + "|" + dot.TickCritDamage.ToString() +
                        "\nCast Time:" + dot.CastTime.ToString() + "Secs" +
                        "\nDebuff Duration:" + dot.DebuffDuration.ToString() +
                        "\nMana Cost:" + dot.ManaCost.ToString() +
                        "\nTime Between Ticks:" + dot.TickPeriod.ToString() + "Secs";
                    break;
                case "DD":
                    DD dd = spell as DD;
                    details =
                        "Direct Damage Spell" +
                        "\nMin Damage: " + dd.MinDamage.ToString() + "|" + dd.MinCritDamage.ToString() +
                        "\nMax Damage:" + dd.MaxDamage.ToString() + "|" + dd.MaxCritDamage.ToString() +
                        "\nCast Time:" + dd.CastTime.ToString() + "Secs" +
                        "\nCool Down:" + dd.Cooldown.ToString() + "Secs" +
                        "\nMana Cost:" + dd.ManaCost.ToString();


                    break;
            }
            return details;
        }
        #endregion

        public void AddResult(string key, string value)
        {
            _results.Add(key, value);
        }
    }
}
