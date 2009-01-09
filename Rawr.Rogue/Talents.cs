using System.Collections.Generic;

namespace Rawr.Rogue
{
    public class Talents
    {
        private delegate int TalentDelegate();

        private Talents(TalentDelegate talent, params float[] multipliers):this(talent, 1, multipliers){}
        private Talents(TalentDelegate talent, int secondaryMultiplierBonus, params float[] multipliers)
        {
            _talent = talent;
            _secondaryMultiplierBonus = secondaryMultiplierBonus;
            _multipliers.Add(0f);
            _multipliers.AddRange(multipliers);
            _items.Add(this);
        }

        private readonly TalentDelegate _talent;
        private readonly int _secondaryMultiplierBonus;
        private readonly List<float> _multipliers = new List<float>();
        private static readonly List<Talents> _items = new List<Talents>();
        private static RogueTalents _talents = new RogueTalents();

        public static readonly Talents ImprovedEviscerate = new Talents(() => _talents.ImprovedEviscerate, .07f, .14f, .20f);
        public static readonly Talents FindWeakness = new Talents(() => _talents.FindWeakness, .02f, .04f, .06f);
        public static readonly Talents Aggression = new Talents(() => _talents.Aggression, 0.03f, 0.06f, 0.09f, 0.12f, 0.15f);
        public static readonly Talents BladeTwisting = new Talents(() => _talents.BladeTwisting, 0.05f, 0.1f);
        public static readonly Talents SurpriseAttacks = new Talents(() => _talents.SurpriseAttacks, 0.1f);
        public static readonly Talents Opportunity = new Talents(() => _talents.Opportunity, 0.1f, 0.2f);
        public static readonly Talents Lethality = new Talents(() => _talents.Lethality, 0.06f, 0.12f, 0.18f, 0.24f, 0.30f);
        public static readonly Talents DirtyDeeds = new Talents(() => _talents.DirtyDeeds, 0.035f, 0.07f);
        public static readonly Talents SinisterCalling = new Talents(() => _talents.SinisterCalling, 0.01f, 0.02f, 0.03f, 0.04f, 0.05f);
        public static readonly Talents VilePoisons = new Talents(() => _talents.VilePoisons, 0.07f, 0.14f, 0.2f);
        public static readonly Talents ImprovedPoisons = new Talents(() => _talents.ImprovedPoisons, 0.02f, 0.04f, 0.06f, 0.08f, 0.10f);
        public static readonly Talents SwordSpecialization = new Talents(() => _talents.SwordSpecialization, 0.01f, 0.02f, 0.03f, 0.04f, 0.5f);
        public static readonly Talents SerratedBlades = new Talents(() => _talents.SerratedBlades, 0.01f, 0.02f, 0.03f);
        public static readonly Talents BloodSpatter = new Talents(() => _talents.BloodSpatter, 0.15f, .030f);
        public static readonly Talents QuickRecovery = new Talents(() => _talents.QuickRecovery, 0.4f, .8f);

        public static readonly Talents PuncturingWounds = new Talents(() => _talents.PuncturingWounds, 2, 0.05f, 0.10f, 0.15f);
        public static readonly Talents SlaughterFromTheShadows = new Talents(() => _talents.SlaughterFromTheShadows, 3, 1f, 2f, 3f, 4f, 5f);
        public static readonly Talents ImprovedSinisterStrike = new Talents(() => _talents.ImprovedSinisterStrike, 3f, 5f);

        public static void Initialize(RogueTalents talents)
        {
            _talents = talents;
        }

        public float Bonus
        {
            get { return _multipliers[_talent()]; }
        }

        public float Multiplier
        {
            get { return 1f + Bonus; }
        }

        public float SecondaryBonus
        {
            get { return _secondaryMultiplierBonus*Bonus; }
        }

        public bool HasPoints { get { return _talent() > 0; } }

        public static float Add(params Talents[] talents)
        {
            var dmg = 1f;
            foreach (var talent in talents)
            {
                dmg += talent.Bonus;
            }
            return dmg;
        }

        public static float Multiply(params Talents[] talents)
        {
            var dmg = 1f;
            foreach (var talent in talents)
            {
                dmg *= talent.Multiplier;
            }
            return dmg;
        }
    }
}
