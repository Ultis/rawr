using System.Collections.Generic;

namespace Rawr.Rogue
{
	/* Yes, this is a weird class.  This class attempts to get around 2 problems:  
	 * 
	 * First, is the duplication of code everywhere we need a  talent.  For example, 
	 * the talent "Murder" was shown everywhere as ".2f*RogueTalents.Murder".  So, 
	 * for Mutilate, SS, Rupture, Garrote, Backstab, etc, we can replace that 
	 * expression with "Talents.Murder.Bonus".  Removal of evil duplication.
	 * 
	 * Secondly, RogueTalents returns an [int], and not an Object. (By Object, I mean
	 * encapsulation, not inheritance).  So, this class wraps each RogueTalent in
	 * a delegate syntax, so I can map the number of talent points to a bonus value.
	 * So, for example, take this line:
	 * 
	 * public static readonly Talents ImprovedEviscerate = new Talents(() => _talents.ImprovedEviscerate, .07f, .14f, .20f);
	 * 
	 * This translates to: "ImprovedEviscerate talent will return a bonus of .07f 
	 * with one talent point, .14f with 2 talent points, and .20f with 3 talent points". 
	 * By default, a "bonus" of 0 is added in to each talent when the talent has zero
	 * points in it.
	 * 
	 * In the case of multiple effects, there is a class wrapper to group the bonuses
	 * under the specific Talent, soPuncturing Wounds is accessed like this:
	 * 
	 * Talents.PuncturingWounds.Mutilate.Bonus
	 * Talents.PuncturingWounds.Backstab.Bonus
	 */
	public class Talents
	{
		private delegate int TalentDelegate();

		private Talents(TalentDelegate talent, params float[] multipliers)
		{
			_talent = talent;
			_bonuses.Add(0f);
			_bonuses.AddRange(multipliers);
		}

		private readonly TalentDelegate _talent;
		private readonly List<float> _bonuses = new List<float>();
		private static RogueTalents _talents = new RogueTalents();

		//---------------------------------------------------------------------
		//Assassination Talents
		//---------------------------------------------------------------------
		public static readonly Talents ImprovedEviscerate = new Talents(() => _talents.ImprovedEviscerate, .07f, .14f, .20f);
		public static readonly Talents Malice = new Talents(() => _talents.Malice, 1f, 2f, 3f, 4f, 5f);
		public static readonly Talents Ruthlessness = new Talents(() => _talents.Ruthlessness, .2f, .4f, .6f);
		public static readonly Talents BloodSpatter = new Talents(() => _talents.BloodSpatter, 0.15f, .030f);

		public class PuncturingWounds
		{
			public static readonly Talents Mutilate = new Talents(() => _talents.PuncturingWounds, 0.05f, 0.10f, 0.15f);
			public static readonly Talents Backstab = new Talents(() => _talents.PuncturingWounds, 0.1f, 0.2f, 0.3f);
		}

		public static readonly Talents Vigor = new Talents(() => _talents.Vigor, 10f);
		public static readonly Talents ImprovedExposeArmor = new Talents(() => _talents.ImprovedExposeArmor, 5f, 10f);
		public static readonly Talents Lethality = new Talents(() => _talents.Lethality, 0.06f, 0.12f, 0.18f, 0.24f, 0.30f);
		public static readonly Talents VilePoisons = new Talents(() => _talents.VilePoisons, 0.07f, 0.14f, 0.2f);
		
		public class ImprovedPoisons
		{
			public static readonly Talents DeadlyPoison = new Talents(() => _talents.ImprovedPoisons, 0.02f, 0.04f, 0.06f, 0.08f, 0.10f);
			public static readonly Talents InstantPoison = new Talents(() => _talents.ImprovedPoisons, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f);
		}
		//NEED:  Cold Blood
		public static readonly Talents QuickRecovery = new Talents(() => _talents.QuickRecovery, 0.4f, .8f);
		public static readonly Talents SealFate = new Talents(() => _talents.SealFate, .2f, .4f, .6f, .8f, 1f);
		public static readonly Talents Murder = new Talents(() => _talents.Murder, .02f, .04f);
		public static readonly Talents DeadlyBrew = new Talents(() => _talents.DeadlyBrew, 0, 0);

		public static readonly Talents FocusedAttacks = new Talents(() => _talents.FocusedAttacks, .66f, 1.32f, 2f);//energy per rank (e.g. .33*2, .66*2, 1*2)

		public static readonly Talents FindWeakness = new Talents(() => _talents.FindWeakness, .2f, .4f, .6f);

		public class MasterPoisoner
		{
			public static readonly Talents Crit = new Talents(() => _talents.MasterPoisoner, 1f, 2f, 3f);
			public static readonly Talents DeadlyPoisonApplication = new Talents(() => _talents.MasterPoisoner, .15f, .30f, .45f);
		}

		public static readonly Talents TurnTheTables = new Talents(() => _talents.TurnTheTables, .2f, .4f, .6f);
		public static readonly Talents CutToTheChase = new Talents(() => _talents.CutToTheChase, .2f, .4f, .6f, .8f, 1f);

		public class HungerForBlood
		{
			public static readonly Talents EnergyPerSecond = new Talents(() => _talents.HungerForBlood, -0.25f);
			public static readonly Talents Damage = new Talents(() => _talents.HungerForBlood, 0.15f + (Glyphs.HungerforBlood ? .03f : 0f));
		}


		//---------------------------------------------------------------------
		//Combat Talents
		//---------------------------------------------------------------------
		public static readonly Talents ImprovedSinisterStrike = new Talents(() => _talents.ImprovedSinisterStrike, 3f, 5f);
		public static readonly Talents DualWieldSpecialization = new Talents(() => _talents.DualWieldSpecialization, 0.5f, 0.1f, 0.15f, 0.2f, 0.25f);
		public static readonly Talents ImprovedSliceAndDice = new Talents(() => _talents.ImprovedSliceAndDice, 0.25f, 0.50f);
		public static readonly Talents Precision = new Talents(() => _talents.Precision, 1f, 2f, 3f, 4f, 5f);
		//NEED  Endurance - is there a need for this one?
		//NEED: Riposte (might be another CPG class)
		public static readonly Talents CloseQuartersCombat = new Talents(() => _talents.CloseQuartersCombat, 1f, 2f, 3f, 4f, 5f);
		public static readonly Talents Aggression = new Talents(() => _talents.Aggression, 0.03f, 0.06f, 0.09f, 0.12f, 0.15f);
		public static readonly Talents LightningReflexes = new Talents(() => _talents.LightningReflexes, .04f, .07f, .10f);
		public static readonly Talents MaceSpecialization = new Talents(() => _talents.MaceSpecialization, .03f, .06f, .9f, .12f, .15f);
		public static readonly Talents BladeFlurry = new Talents(() => _talents.BladeFlurry, 0.2f);
		public static readonly Talents SwordSpecialization = new Talents(() => _talents.SwordSpecialization, 0.01f, 0.02f, 0.03f, 0.04f, 0.5f);
		public static readonly Talents WeaponExpertise = new Talents(() => _talents.WeaponExpertise, 5, 10);
		public static readonly Talents BladeTwisting = new Talents(() => _talents.BladeTwisting, 0.05f, 0.1f);
		public static readonly Talents Vitality = new Talents(() => _talents.Vitality, .08f, .16f, .25f);
		public static readonly Talents AdrenalineRush = new Talents(() => _talents.AdrenalineRush, 0.08333333f);

        public static readonly Talents CombatPotency = new Talents(() => _talents.CombatPotency, .6f, 1.2f, 1.8f, 2.4f, 3f); //energy per success

		//NEED: Unfair Advantage??
		public static readonly Talents SurpriseAttacks = new Talents(() => _talents.SurpriseAttacks, 0.1f);

		public class SavageCombat
		{
			public static readonly Talents AttackPower = new Talents(() => _talents.SavageCombat, .2f, .4f);
			public static readonly Talents Damage = new Talents(() => _talents.SavageCombat, .2f, .4f);
		}

		public static readonly Talents PreyOnTheWeak = new Talents(() => _talents.PreyOnTheWeak, .2f, .4f, .6f, .8f, 1f);

		//NEED  Killing Spree (might be another CPG class, but generates 0 CPs).

		//---------------------------------------------------------------------
		//Subtlety Talents
		//---------------------------------------------------------------------
		public static readonly Talents RelentlessStrikes = new Talents(() => _talents.RelentlessStrikes, 1f, 2f, 3f, 4f, 5f);  //Average energy returned per combo point
		public static readonly Talents Opportunity = new Talents(() => _talents.Opportunity, 0.1f, 0.2f);
		//NEED:  Ghostly Strike (might be another CPG class)

		public class SerratedBlades
		{
			public static readonly Talents ArmorPenetration = new Talents(() => _talents.SerratedBlades, 0.00f, 0.00f, 0.00f);  //NEEDS UPDATING:  Armor Pen bonus is 0 on Rawr and the website
			public static readonly Talents Rupture = new Talents(() => _talents.SerratedBlades, 0.1f, 0.2f, 0.3f);  //NEEDS UPDATING:  Armor Pen bonus is 0 on Rawr and the website
		}

		public static readonly Talents DirtyDeeds = new Talents(() => _talents.DirtyDeeds, 0.035f, 0.07f);
		public static readonly Talents Deadliness = new Talents(() => _talents.Deadliness, 0.02f, 0.04f, 0.06f, 0.08f, 0.10f);

		//NEED Premeditation
		public class SinisterCalling
		{
			public static readonly Talents Agility = new Talents(() => _talents.SinisterCalling, 0.03f, 0.06f, 0.09f, 0.12f, 0.15f);
			public static readonly Talents HemoAndBackstab = new Talents(() => _talents.SinisterCalling, 0.02f, 0.04f, 0.06f, 0.08f, 0.10f);
		}

		//NEED  HAT 
		//NEED  Shadowstep
		//NEED  Filthy Tricks

		public class SlaughterFromTheShadows
		{
			public static readonly Talents BackstabAndAmbushEnergyCost = new Talents(() => _talents.SlaughterFromTheShadows, 3f, 6f, 9f, 12f, 15f);
			public static readonly Talents HemoEnergyCost = new Talents(() => _talents.SlaughterFromTheShadows, 1f, 2f, 3f, 4f, 5f);
		}

        //---------------------------------------------------------------------
        //Glyphs
        //---------------------------------------------------------------------
        public class Glyphs
        {
            public static bool Backstab { get { return _talents.GlyphOfBackstab; } }
            public static bool Eviscerate { get { return _talents.GlyphOfEviscerate; } }
            public static bool Mutilate { get { return _talents.GlyphOfMutilate; } }
            public static bool HungerforBlood { get { return _talents.GlyphOfHungerforBlood; } }
            public static bool KillingSpree { get { return _talents.GlyphOfKillingSpree; } }
            public static bool Vigor { get { return _talents.GlyphOfVigor; } }
            public static bool FanOfKnives { get { return _talents.GlyphOfFanOfKnives; } }
            public static bool ExposeArmor { get { return _talents.GlyphOfExposeArmor; } }
            public static bool SinisterStrike { get { return _talents.GlyphOfSinisterStrike; } }
            public static bool SliceandDice { get { return _talents.GlyphOfSliceandDice; } }
            public static bool Feint { get { return _talents.GlyphOfFeint; } }
            public static bool GhostlyStrike { get { return _talents.GlyphOfGhostlyStrike; } }
            public static bool Rupture { get { return _talents.GlyphOfRupture; } }
            public static bool BladeFlurry { get { return _talents.GlyphOfBladeFlurry; } }
            public static bool AdrenalineRush { get { return _talents.GlyphOfAdrenalineRush; } }
        }

		//---------------------------------------------------------------------
		//Class Initializers and Methods
		//---------------------------------------------------------------------
		public static void Initialize(RogueTalents talents)
		{
			_talents = talents;
		}

		public float Bonus
		{
			get { return _bonuses[_talent()]; }
		}

		public float Multiplier
		{
			get { return 1f + Bonus; }
		}

		public int Points { get { return _talent(); } }
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
